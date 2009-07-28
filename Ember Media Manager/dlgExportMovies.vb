' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################

Option Explicit On

Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions

Public Class dlgExportMovies
    Private isCL As Boolean = False
    Private bexportPosters As Boolean = False
    Private bexportFanart As Boolean = False
    Private bexportFlags As Boolean = False
    Dim HTMLBody As New StringBuilder
    Dim _movies As New List(Of Master.DBMovie)
    Dim bFiltered As Boolean = False
    Dim bCancelled As Boolean = False
    Dim sTemplate As String = String.Empty
    Dim TempPath As String = Path.Combine(Master.TempPath, "Export")
    Friend WithEvents bwLoadInfo As New System.ComponentModel.BackgroundWorker

    Public Shared Sub CLExport(ByVal filename As String, Optional ByVal template As String = "template", Optional ByVal resizePoster As Integer = 0)
        Dim MySelf As New dlgExportMovies
        If Not Directory.Exists(Path.GetDirectoryName(filename)) Then
            Return
        End If
        '        MySelf.exportPosters = needImages
        '        MySelf.exportFanart = needImages
        MySelf.isCL = True
        MySelf.bwLoadInfo = New System.ComponentModel.BackgroundWorker
        MySelf.bwLoadInfo.WorkerSupportsCancellation = True
        MySelf.bwLoadInfo.WorkerReportsProgress = True
        MySelf.bwLoadInfo.RunWorkerAsync()
        Do While MySelf.bwLoadInfo.IsBusy
            Application.DoEvents()
        Loop
        MySelf.BuildHTML(False, "", "", template)
        Dim srcPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, "html", Path.DirectorySeparatorChar, template, Path.DirectorySeparatorChar)
        MySelf.SaveAll(srcPath, filename, resizePoster)
    End Sub

    Private Sub dlgExportMovies_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Me.bwLoadInfo.IsBusy Then
            Me.DoCancel()
            Do While Me.bwLoadInfo.IsBusy
                Application.DoEvents()
            Loop
        End If

        Master.DeleteDirectory(TempPath)
    End Sub

    Private Sub bwLoadInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadInfo.DoWork
        '//
        ' Thread to load movieinformation (from nfo)
        '\\
        Try
            ' Clean up Movies List if any
            _movies.Clear()
            ' Load nfo movies using path from DB
            Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                Dim _tmpMovie As New Master.DBMovie
                Dim _ID As Integer
                Dim iProg As Integer = 0
                SQLNewcommand.CommandText = String.Concat("SELECT COUNT(id) AS mcount FROM movies;")
                Using SQLcount As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    Me.bwLoadInfo.ReportProgress(-1, SQLcount("mcount")) ' set maximum
                End Using
                SQLNewcommand.CommandText = String.Concat("SELECT ID FROM movies ORDER BY ListTitle ASC;")
                Using SQLreader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    If SQLreader.HasRows Then
                        While SQLreader.Read()
                            _ID = SQLreader("ID")
                            _tmpMovie = Master.DB.LoadMovieFromDB(_ID)
                            _movies.Add(_tmpMovie)
                            Me.bwLoadInfo.ReportProgress(iProg, _tmpMovie.ListTitle) '  show File
                            iProg += 1
                            If bwLoadInfo.CancellationPending Then
                                e.Cancel = True
                                Return
                            End If
                        End While
                        e.Result = True
                    Else
                        e.Cancel = True
                    End If
                End Using
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Sub ExportPoster(ByVal fpath As String, ByVal new_width As Integer)
        Dim counter As Integer = 1
        Dim finalpath As String = Path.Combine(fpath, "export")
        Directory.CreateDirectory(finalpath)
        For Each _curMovie As Master.DBMovie In _movies
            Dim posterfile As String = Path.Combine(finalpath, String.Concat(counter.ToString, ".jpg"))
            If File.Exists(_curMovie.PosterPath) Then
                If new_width > 0 Then
                    Dim im As New Images
                    im.FromFile(_curMovie.PosterPath)
                    ImageManip.ResizeImage(im.Image, new_width, new_width, False, Color.Black.ToArgb)
                    im.Save(posterfile)
                Else
                    File.Copy(_curMovie.PosterPath, posterfile, True)
                End If
            End If
            counter += 1
        Next
    End Sub

    Sub ExportFanart(ByVal fpath As String)
        Dim counter As Integer = 1
        Dim finalpath As String = Path.Combine(fpath, "export")
        Directory.CreateDirectory(finalpath)
        For Each _curMovie As Master.DBMovie In _movies
            Dim fanartfile As String = Path.Combine(finalpath, String.Concat(counter.ToString, "-fanart.jpg"))
            If File.Exists(_curMovie.FanartPath) Then

                File.Copy(_curMovie.FanartPath, fanartfile, True)
            End If
            counter += 1
        Next
    End Sub

    Private Function GetAVImages(ByVal AVMovie As Master.DBMovie, ByVal line As String) As String

        '//
        ' Parse the Flags XML and set the proper images
        '\\

        If XML.FlagsXML.Nodes.Count > 0 Then
            'Dim mePath As String = ""
            Dim mePath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Flags")
            Try
                Dim fiAV As MediaInfo.Fileinfo = AVMovie.Movie.FileInfo
                Dim atypeRef As String = String.Empty
                Dim vresImage As String = String.Empty
                Dim vsourceImage As String = String.Empty
                Dim vtypeImage As String = String.Empty
                Dim atypeImage As String = String.Empty
                Dim achanImage As String = String.Empty
                Dim tVideo As MediaInfo.Video = NFO.GetBestVideo(fiAV)
                Dim tAudio As MediaInfo.Audio = NFO.GetBestAudio(fiAV)
                Dim sourceCheck As String = String.Empty

                If Directory.GetParent(AVMovie.Filename).Name.ToLower = "video_ts" Then
                    sourceCheck = Directory.GetParent(Directory.GetParent(AVMovie.Filename).FullName).Name.ToLower
                Else
                    sourceCheck = String.Concat(Directory.GetParent(AVMovie.Filename).Name.ToLower, Path.DirectorySeparatorChar, Path.GetFileName(AVMovie.Filename).ToLower)
                End If

                'video resolution
                Dim xVResDefault = From xDef In XML.FlagsXML...<vres> Select xDef.Element("default").Element("icon").Value
                If xVResDefault.Count > 0 Then
                    vresImage = Path.Combine(mePath, xVResDefault(0).ToString)
                End If

                Dim strRes As String = NFO.GetResFromDimensions(tVideo).ToLower
                If Not String.IsNullOrEmpty(strRes) Then
                    Dim xVResFlag = From xVRes In XML.FlagsXML...<vres>...<name> Where Regex.IsMatch(strRes, xVRes.@searchstring) Select xVRes.<icon>.Value
                    If xVResFlag.Count > 0 Then
                        vresImage = Path.Combine(mePath, xVResFlag(0).ToString)
                    End If
                End If

                'video source
                Dim xVSourceDefault = From xDef In XML.FlagsXML...<vsource> Select xDef.Element("default").Element("icon").Value
                If xVSourceDefault.Count > 0 Then
                    vsourceImage = Path.Combine(mePath, xVSourceDefault(0).ToString)
                End If

                Dim xVSourceFlag = From xVSource In XML.FlagsXML...<vsource>...<name> Where Regex.IsMatch(sourceCheck, xVSource.@searchstring) Select xVSource.<icon>.Value
                If xVSourceFlag.Count > 0 Then
                    vsourceImage = Path.Combine(mePath, xVSourceFlag(0).ToString)
                End If

                'video type
                Dim xVTypeDefault = From xDef In XML.FlagsXML...<vtype> Select xDef.Element("default").Element("icon").Value
                If xVTypeDefault.Count > 0 Then
                    vtypeImage = Path.Combine(mePath, xVTypeDefault(0).ToString)
                End If

                Dim vCodec As String = tVideo.Codec.ToLower
                If Not String.IsNullOrEmpty(vCodec) Then
                    Dim xVTypeFlag = From xVType In XML.FlagsXML...<vtype>...<name> Where Regex.IsMatch(vCodec, xVType.@searchstring) Select xVType.<icon>.Value
                    If xVTypeFlag.Count > 0 Then
                        vtypeImage = Path.Combine(mePath, xVTypeFlag(0).ToString)
                    End If
                End If

                'audio type
                Dim xATypeDefault = From xDef In XML.FlagsXML...<atype> Select xDef.Element("default").Element("icon").Value
                If xATypeDefault.Count > 0 Then
                    atypeImage = Path.Combine(mePath, xATypeDefault(0).ToString)
                End If

                Dim aCodec As String = tAudio.Codec.ToLower
                If Not String.IsNullOrEmpty(aCodec) Then
                    Dim xATypeFlag = From xAType In XML.FlagsXML...<atype>...<name> Where Regex.IsMatch(aCodec, xAType.@searchstring) Select xAType.<icon>.Value, xAType.<ref>.Value
                    If xATypeFlag.Count > 0 Then
                        atypeImage = Path.Combine(mePath, xATypeFlag(0).icon.ToString)
                        If Not IsNothing(xATypeFlag(0).ref) Then
                            atypeRef = xATypeFlag(0).ref.ToString
                        End If
                    End If
                End If

                'audio channels
                Dim xAChanDefault = From xDef In XML.FlagsXML...<achan> Select xDef.Element("default").Element("icon").Value
                If xAChanDefault.Count > 0 Then
                    achanImage = Path.Combine(mePath, xAChanDefault(0).ToString)
                End If

                If Not String.IsNullOrEmpty(tAudio.Channels) Then
                    Dim xAChanFlag = From xAChan In XML.FlagsXML...<achan>...<name> Where Regex.IsMatch(tAudio.Channels, Regex.Replace(xAChan.@searchstring, "(\{[^\}]+\})", String.Empty)) And Regex.IsMatch(atypeRef, Regex.Match(xAChan.@searchstring, "\{atype=([^\}]+)\}").Groups(1).Value.ToString) Select xAChan.<icon>.Value
                    If xAChanFlag.Count > 0 Then
                        achanImage = Path.Combine(mePath, xAChanFlag(0).ToString)
                    End If
                End If

                If Not String.IsNullOrEmpty(vresImage) AndAlso XML.alFlags.Contains(vresImage.ToLower) Then
                    line = line.Replace("<$FLAG_VRES>", Path.GetFileName(vresImage))
                End If

                If Not String.IsNullOrEmpty(vsourceImage) AndAlso XML.alFlags.Contains(vsourceImage.ToLower) Then
                    line = line.Replace("<$FLAG_VSOURCE>", Path.GetFileName(vsourceImage))
                End If

                If Not String.IsNullOrEmpty(vtypeImage) AndAlso XML.alFlags.Contains(vtypeImage.ToLower) Then
                    line = line.Replace("<$FLAG_VTYPE>", Path.GetFileName(vtypeImage))
                End If

                If Not String.IsNullOrEmpty(atypeImage) AndAlso XML.alFlags.Contains(atypeImage.ToLower) Then
                    line = line.Replace("<$FLAG_ATYPE>", Path.GetFileName(atypeImage))
                End If

                If Not String.IsNullOrEmpty(achanImage) AndAlso XML.alFlags.Contains(achanImage.ToLower) Then
                    line = line.Replace("<$FLAG_ACHAN>", Path.GetFileName(achanImage))
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
        Return line
    End Function

    Private Sub BuildHTML(ByVal bSearch As Boolean, ByVal strFilter As String, ByVal strIn As String, ByVal template As String)
        Try
            ' Build HTML Documment in Code ... ugly but will work until new option
            HTMLBody.Length = 0
            Master.DeleteDirectory(TempPath)
            Dim tVid As New MediaInfo.Video
            Dim tAud As New MediaInfo.Audio
            Dim tRes As String = String.Empty
            Dim htmlPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, "html", Path.DirectorySeparatorChar, template, Path.DirectorySeparatorChar, Master.eSettings.Language, ".html")
            Dim pattern As String = File.ReadAllText(htmlPath)
            Dim movieheader As String = String.Empty
            Dim moviefooter As String = String.Empty
            Dim movierow As String = String.Empty
            If pattern.Contains("<$NEED_POSTERS>") Then
                Me.bexportPosters = True
                pattern = pattern.Replace("<$NEED_POSTERS>", String.Empty)
            End If
            If pattern.Contains("<$NEED_FANART>") Then
                Me.bexportFanart = True
                pattern = pattern.Replace("<$NEED_FANART>", String.Empty)
            End If
            If pattern.Contains("<$NEED_FLAGS>") Then
                Me.bexportFlags = True
                pattern = pattern.Replace("<$NEED_FLAGS>", String.Empty)
            End If
            Dim s = pattern.IndexOf("<$MOVIE>")
            If s >= 0 Then
                Dim e = pattern.IndexOf("<$/MOVIE>")
                If e >= 0 Then
                    movieheader = pattern.Substring(0, s)
                    movierow = pattern.Substring(s + 8, e - s - 8)
                    moviefooter = pattern.Substring(e + 9, pattern.Length - e - 9)
                Else
                    'error
                End If
            Else
                'error
            End If

            If bSearch Then
                bFiltered = True
            Else
                bFiltered = False
            End If

            HTMLBody.Append(movieheader)
            Dim counter As Integer = 1
            For Each _curMovie As Master.DBMovie In _movies
                Dim _vidDetails As String = String.Empty
                Dim _audDetails As String = String.Empty
                If Not IsNothing(_curMovie.Movie.FileInfo) Then
                    If _curMovie.Movie.FileInfo.StreamDetails.Video.Count > 0 Then
                        tVid = NFO.GetBestVideo(_curMovie.Movie.FileInfo)
                        tRes = NFO.GetResFromDimensions(tVid)
                        _vidDetails = String.Format("{0} / {1}", If(String.IsNullOrEmpty(tRes), Master.eLang.GetString(283, "Unknown"), tRes), If(String.IsNullOrEmpty(tVid.Codec), Master.eLang.GetString(283, "Unknown"), tVid.Codec)).ToUpper
                    End If

                    If _curMovie.Movie.FileInfo.StreamDetails.Audio.Count > 0 Then
                        tAud = NFO.GetBestAudio(_curMovie.Movie.FileInfo)
                        _audDetails = String.Format("{0} / {1}ch", If(String.IsNullOrEmpty(tAud.Codec), Master.eLang.GetString(283, "Unknown"), tAud.Codec), If(String.IsNullOrEmpty(tAud.Channels), Master.eLang.GetString(283, "Unknown"), tAud.Channels)).ToUpper
                    End If
                End If

                Dim row As String = movierow
                row = row.Replace("<$MOVIENAME>", Web.HttpUtility.HtmlEncode(_curMovie.ListTitle))
                row = row.Replace("<$YEAR>", _curMovie.Movie.Year)
                row = row.Replace("<$COUNT>", counter.ToString)
                row = row.Replace("<$FILENAME>", Path.GetFileName(_curMovie.Filename))
                row = row.Replace("<$DIRNAME>", Path.GetDirectoryName(_curMovie.Filename))
                row = row.Replace("<$OUTLINE>", Web.HttpUtility.HtmlEncode(_curMovie.Movie.Outline))
                row = row.Replace("<$PLOT>", Web.HttpUtility.HtmlEncode(_curMovie.Movie.Plot))
                row = row.Replace("<$GENRES>", Web.HttpUtility.HtmlEncode(_curMovie.Movie.Genre))
                row = row.Replace("<$VIDEO>", _vidDetails)
                row = row.Replace("<$AUDIO>", _audDetails)
                row = GetAVImages(_curMovie, row)
                If bSearch Then
                    If (strIn = Master.eLang.GetString(279, "Video Flag") AndAlso _vidDetails.Contains(strFilter)) OrElse _
                       (strIn = Master.eLang.GetString(280, "Audio Flag") AndAlso _audDetails.Contains(strFilter)) OrElse _
                       (strIn = Master.eLang.GetString(21, "Title") AndAlso _curMovie.Movie.Title.Contains(strFilter)) OrElse _
                       (strIn = Master.eLang.GetString(278, "Year") AndAlso _curMovie.Movie.Year.Contains(strFilter)) Then
                        HTMLBody.Append(row)
                    End If
                Else

                    HTMLBody.Append(row)
                End If
                counter += 1
            Next
            HTMLBody.Append(moviefooter)

            If Not isCL Then
                Dim outFile As String = Path.Combine(Me.TempPath, String.Concat(Master.eSettings.Language, ".html"))
                Me.SaveAll(Path.GetDirectoryName(htmlPath), outFile)
                wbMovieList.Navigate(outFile)
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwLoadInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadInfo.RunWorkerCompleted
        '//
        ' Thread finished: display it if not cancelled
        '\\
        If Not Me.isCL Then
            bCancelled = e.Cancelled
            If Not e.Cancelled Then
                wbMovieList.DocumentText = HTMLBody.ToString
            Else
                wbMovieList.DocumentText = String.Concat("<center><h1 style=""color:Red;"">", Master.eLang.GetString(284, "Cancelled"), "</h1></center>")
            End If
            Me.pnlCancel.Visible = False
        End If
    End Sub

    Private Sub bwLoadInfo_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadInfo.ProgressChanged
        If e.ProgressPercentage >= 0 Then
            Me.pbCompile.Value = e.ProgressPercentage
            Me.lblFile.Text = e.UserState
        Else
            Me.pbCompile.Maximum = Convert.ToInt32(e.UserState)
        End If

    End Sub

    Private Sub DoCancel()
        Me.bwLoadInfo.CancelAsync()
        btnCancel.Visible = False
        lblCompiling.Visible = False
        pbCompile.Style = ProgressBarStyle.Marquee
        pbCompile.MarqueeAnimationSpeed = 25
        lblCanceling.Visible = True
        lblFile.Visible = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles wbMovieList.DocumentCompleted
        If Not bCancelled Then
            wbMovieList.Visible = True
            Me.Save_Button.Enabled = True
            pnlSearch.Enabled = True
            Reset_Button.Enabled = bFiltered
        End If
    End Sub

    Private Sub SaveAll(ByVal srcPath As String, ByVal destPath As String, Optional ByVal resizePoster As Integer = 0)
        Dim destPathShort As String = Path.GetDirectoryName(destPath)
        CopyDirectory(srcPath, destPathShort, True)
        If Me.bexportFlags Then
            srcPath = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Flags", Path.DirectorySeparatorChar)
            Directory.CreateDirectory(Path.Combine(destPathShort, "Flags"))
            CopyDirectory(srcPath, Path.Combine(destPathShort, "Flags"), True)
        End If
        If Me.bexportPosters Then
            Me.ExportPoster(destPathShort, resizePoster)
        End If
        If Me.bexportFanart Then
            Me.ExportFanart(destPathShort)
        End If

        Dim myStream As Stream = File.OpenWrite(destPath)
        If Not IsNothing(myStream) Then
            myStream.Write(System.Text.Encoding.ASCII.GetBytes(HTMLBody.ToString), 0, HTMLBody.ToString.Length)
            myStream.Close()
        End If
    End Sub

    Private Sub Save_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save_Button.Click
        Dim saveHTML As New SaveFileDialog()
        saveHTML.Filter = "HTML files (*.htm)|*.htm"
        saveHTML.FilterIndex = 2
        saveHTML.RestoreDirectory = True

        If saveHTML.ShowDialog() = DialogResult.OK Then
            Dim srcPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, "html", Path.DirectorySeparatorChar, Me.sTemplate, Path.DirectorySeparatorChar)
            Me.SaveAll(srcPath, saveHTML.FileName, 0)
        End If

    End Sub

    Private Sub Search_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Search_Button.Click
        pnlSearch.Enabled = False
        BuildHTML(True, txtSearch.Text, cbSearch.Text, Me.sTemplate)
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        If txtSearch.Text <> "" And cbSearch.Text <> "" Then
            Search_Button.Enabled = True
        Else
            Search_Button.Enabled = False
        End If
    End Sub

    Private Sub cbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSearch.SelectedIndexChanged
        If txtSearch.Text <> "" And cbSearch.Text <> "" Then
            Search_Button.Enabled = True
        Else
            Search_Button.Enabled = False
        End If
    End Sub

    Private Sub Reset_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Reset_Button.Click
        pnlSearch.Enabled = False
        BuildHTML(False, String.Empty, String.Empty, Me.sTemplate)
    End Sub

    Private Sub dlgExportMovies_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not isCL Then
            Me.GetTemplates()
            Me.SetUp()
        End If
    End Sub

    Private Sub dlgMoviesReport_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        ' Show Cancel Panel
        btnCancel.Visible = True
        lblCompiling.Visible = True
        pbCompile.Visible = True
        pbCompile.Style = ProgressBarStyle.Continuous
        lblCanceling.Visible = False
        pnlCancel.Visible = True
        Application.DoEvents()

        Me.Activate()

        'Start worker
        Me.bwLoadInfo = New System.ComponentModel.BackgroundWorker
        Me.bwLoadInfo.WorkerSupportsCancellation = True
        Me.bwLoadInfo.WorkerReportsProgress = True
        Me.bwLoadInfo.RunWorkerAsync()

    End Sub

    Private Sub SetUp()

        Me.Text = Master.eLang.GetString(272, "Export Movies")
        Me.Save_Button.Text = Master.eLang.GetString(272, "Save")
        Me.Close_Button.Text = Master.eLang.GetString(19, "Close")
        Me.Reset_Button.Text = Master.eLang.GetString(274, "Reset")
        Me.Label1.Text = Master.eLang.GetString(275, "Filter")
        Me.Search_Button.Text = Master.eLang.GetString(276, "Apply")
        Me.lblIn.Text = Master.eLang.GetString(277, "in")
        Me.lblCompiling.Text = Master.eLang.GetString(165, "Compiling Movie List...")
        Me.lblCanceling.Text = Master.eLang.GetString(166, "Canceling Compilation...")
        Me.btnCancel.Text = Master.eLang.GetString(167, "Cancel")
        Me.lblTemplate.Text = Master.eLang.GetString(450, "Template")

        Me.cbSearch.Items.AddRange(New Object() {Master.eLang.GetString(21, "Title"), Master.eLang.GetString(278, "Year"), Master.eLang.GetString(279, "Video Flag"), Master.eLang.GetString(280, "Audio Flag")})
    End Sub

    Private Shared Sub CopyDirectory(ByVal SourcePath As String, ByVal DestPath As String, Optional ByVal Overwrite As Boolean = False)
        Dim SourceDir As DirectoryInfo = New DirectoryInfo(SourcePath)
        Dim DestDir As DirectoryInfo = New DirectoryInfo(DestPath)

        ' the source directory must exist, otherwise throw an exception
        If SourceDir.Exists Then
            ' if destination SubDir's parent SubDir does not exist throw an exception
            If Not DestDir.Parent.Exists Then
                Throw New DirectoryNotFoundException _
                    ("Destination directory does not exist: " + DestDir.Parent.FullName)
            End If

            If Not DestDir.Exists Then
                DestDir.Create()
            End If

            ' copy all the files of the current directory
            Dim ChildFile As FileInfo
            For Each ChildFile In SourceDir.GetFiles()
                If Path.GetExtension(ChildFile.FullName) = ".html" Then
                    Continue For
                End If
                If Overwrite Then
                    ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), True)
                Else
                    ' if Overwrite = false, copy the file only if it does not exist
                    ' this is done to avoid an IOException if a file already exists
                    ' this way the other files can be copied anyway...
                    If Not File.Exists(Path.Combine(DestDir.FullName, ChildFile.Name)) Then
                        ChildFile.CopyTo(Path.Combine(DestDir.FullName, ChildFile.Name), False)
                    End If
                End If
            Next

            ' copy all the sub-directories by recursively calling this same routine
            Dim SubDir As DirectoryInfo
            For Each SubDir In SourceDir.GetDirectories()
                CopyDirectory(SubDir.FullName, Path.Combine(DestDir.FullName, _
                    SubDir.Name), Overwrite)
            Next
        Else
            Throw New DirectoryNotFoundException("Source directory does not exist: " + SourceDir.FullName)
        End If
    End Sub

    Private Sub GetTemplates()
        Dim alDirs As New ArrayList

        Try
            alDirs.AddRange(Directory.GetDirectories(String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Langs", Path.DirectorySeparatorChar, "html")))
        Catch
        End Try

        If alDirs.Count > 0 Then
            Dim PathSplit() As String
            For Each sPath As String In alDirs
                PathSplit = sPath.Split(Path.DirectorySeparatorChar)
                Me.cbTemplates.Items.Add(PathSplit(PathSplit.Length - 1))
            Next
            Me.cbTemplates.SelectedIndex = 0
        End If
    End Sub

    Private Sub cbTemplates_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTemplates.SelectedIndexChanged
        Me.sTemplate = Me.cbTemplates.Text
        BuildHTML(False, Me.txtSearch.Text, Me.cbSearch.Text, Me.sTemplate)
    End Sub
End Class


