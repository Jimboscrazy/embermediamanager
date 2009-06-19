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
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class dlgOfflineHolder
    Private alMedia As New ArrayList
    Private IMDB As New IMDB.Scraper
    Private tmpTitle As String = String.Empty
    Private WorkingPath As String = Path.Combine(Master.TempPath, "OfflineHolder")
    Private destPath As String
    Private idxStsSource As Integer = -1
    Private idxStsMovie As Integer = -1
    Private idxStsImage As Integer = -1
    Friend WithEvents bwCreateHolder As New System.ComponentModel.BackgroundWorker
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If bwCreateHolder.IsBusy Then
            bwCreateHolder.CancelAsync()
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub dlgOfflineHolder_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If Directory.Exists(WorkingPath) Then
            Directory.Delete(WorkingPath, True)
        End If
    End Sub

    Private Sub dlgOfflineHolder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dirArray() As String
        Try

            'load all the movie folders from settings
            alMedia = Master.eSettings.MovieFolders
            For Each strFolders As String In Master.eSettings.MovieFolders
                dirArray = Split(strFolders, "|")
                If dirArray(1).ToString = "Folders" Then
                    Me.cbSources.Items.Add(dirArray(0).ToString)
                End If
            Next
            AddHandler IMDB.MovieInfoDownloaded, AddressOf MovieInfoDownloaded
            AddHandler IMDB.ProgressUpdated, AddressOf MovieInfoDownloadedPercent
            If Directory.Exists(WorkingPath) Then
                Directory.Delete(WorkingPath, True)
            End If
            Directory.CreateDirectory(WorkingPath)

            Master.currPath = Path.Combine(WorkingPath, "PlaceHolder.avi")
            idxStsSource = lvStatus.Items.Add("Source Folder").Index
            lvStatus.Items(idxStsSource).SubItems.Add("Invalid")
            lvStatus.Items(idxStsSource).UseItemStyleForSubItems = False
            lvStatus.Items(idxStsSource).SubItems(1).ForeColor = Color.Red
            idxStsMovie = lvStatus.Items.Add("Movie (Folder Name)").Index
            lvStatus.Items(idxStsMovie).SubItems.Add("Invalid")
            lvStatus.Items(idxStsMovie).UseItemStyleForSubItems = False
            lvStatus.Items(idxStsMovie).SubItems(1).ForeColor = Color.Red
            idxStsImage = lvStatus.Items.Add("Place Holder Image").Index
            lvStatus.Items(idxStsImage).SubItems.Add("Valid")
            lvStatus.Items(idxStsImage).UseItemStyleForSubItems = False
            lvStatus.Items(idxStsImage).SubItems(1).ForeColor = Color.Green
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub GetIMDB_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetIMDB_Button.Click
        Try
            Master.tmpMovie = New Media.Movie

            Using dSearch As New dlgIMDBSearchResults
                If dSearch.ShowDialog(Me.tmpTitle) = Windows.Forms.DialogResult.OK Then
                    If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                        Me.pbProgress.Value = 100
                        Me.pbProgress.Style = ProgressBarStyle.Marquee
                        Me.pbProgress.MarqueeAnimationSpeed = 25
                        Me.pbProgress.Visible = True
                        'Me.Refresh()
                        Me.txtMovieName.Text = String.Format("{0}", Master.tmpMovie.Title)
                        IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast)
                    End If
                End If
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub MovieInfoDownloaded(ByVal bSuccess As Boolean)
        Try
            If bSuccess Then
                If Master.eSettings.UseStudioTags Then
                    'If Me.UpdateMediaInfo(Master.currPath, Master.currMovie) Then
                    'Master.currMovie.Studio = String.Concat(Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                    'End If
                    Me.pbProgress.Visible = True
                End If
                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.isFile, Master.ImageType.Posters) Then
                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.ShowDialog(Master.tmpMovie.IMDBID, Master.currPath, Master.ImageType.Posters)
                        End Using
                    End If
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.isFile, Master.ImageType.Fanart) Then
                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.ShowDialog(Master.tmpMovie.IMDBID, Master.currPath, Master.ImageType.Fanart)
                        End Using
                    End If
                    tmpImages.Dispose()
                    tmpImages = Nothing
                    If Master.eSettings.AutoThumbs > 0 AndAlso Not Master.isFile Then
                        Master.CreateRandomThumbs(Master.currPath, Master.eSettings.AutoThumbs)
                    End If
                End If

                Master.SaveMovieToNFO(Master.tmpMovie, Master.currPath, False)
                'Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                'Dim ID As Integer = Me.dgvMediaList.Rows(indX).Cells(0).Value

                Using dEditMovie As New dlgEditMovie
                    'Select Case dEditMovie.ShowDialog(ID)
                    '   Case Windows.Forms.DialogResult.OK
                    'Me.ReCheckItems(ID)
                    'Me.SetListItemAfterEdit(ID, indX)
                    '  Case Windows.Forms.DialogResult.Retry
                    'Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing)
                    ' Case Windows.Forms.DialogResult.Abort
                    'Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, True)
                    ' End Select
                End Using
                Me.txtMovieName.Text = Master.tmpMovie.Title
            Else
                MsgBox("Unable to retrieve movie details from the internet. Please check your connection and try again.", MsgBoxStyle.Exclamation, "Error Retrieving Details")
            End If
            CheckConditions()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.pbProgress.Visible = False
    End Sub
    Private Sub MovieInfoDownloadedPercent(ByVal iPercent As Integer)
        Me.pbProgress.Value = iPercent
        Me.Refresh()
    End Sub

    Private Sub cbSources_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSources.SelectedIndexChanged
        CheckConditions()

    End Sub
    Sub CheckConditions()
        Try
            destPath = Path.Combine(cbSources.Items(cbSources.SelectedIndex), txtMovieName.Text)
            If cbSources.SelectedIndex >= 0 Then
                lvStatus.Items(idxStsSource).SubItems(1).Text = "Valid"
                lvStatus.Items(idxStsSource).SubItems(1).ForeColor = Color.Green
            Else
                lvStatus.Items(idxStsSource).SubItems(1).Text = "Invalid"
                lvStatus.Items(idxStsSource).SubItems(1).ForeColor = Color.Red
            End If
            If Not txtMovieName.Text = vbNullString Then
                If Directory.Exists(destPath) Then
                    lvStatus.Items(idxStsMovie).SubItems(1).Text = "Exists"
                    lvStatus.Items(idxStsMovie).SubItems(1).ForeColor = Color.Red
                Else
                    lvStatus.Items(idxStsMovie).SubItems(1).Text = "Valid"
                    lvStatus.Items(idxStsMovie).SubItems(1).ForeColor = Color.Green
                End If
            Else
                lvStatus.Items(idxStsMovie).SubItems(1).Text = "Invalid"
                lvStatus.Items(idxStsMovie).SubItems(1).ForeColor = Color.Red
            End If
            If cbUseFanart.Checked Then
                If File.Exists(Path.Combine(WorkingPath, "PlaceHolder-fanart.jpg")) OrElse File.Exists(Path.Combine(WorkingPath, "PlaceHolder.fanart.jpg")) Then
                    lvStatus.Items(idxStsImage).SubItems(1).Text = "Valid"
                    lvStatus.Items(idxStsImage).SubItems(1).ForeColor = Color.Green
                Else
                    lvStatus.Items(idxStsImage).SubItems(1).Text = "Invalid"
                    lvStatus.Items(idxStsImage).SubItems(1).ForeColor = Color.Red
                End If
            Else
                lvStatus.Items(idxStsImage).SubItems(1).Text = "Valid"
                lvStatus.Items(idxStsImage).SubItems(1).ForeColor = Color.Green
            End If
            For Each i As ListViewItem In lvStatus.Items
                If Not i.SubItems(1).ForeColor = Color.Green Then
                    Create_Button.Enabled = False
                    Return
                End If
            Next
            Create_Button.Enabled = True
        Catch ex As Exception
        End Try

    End Sub

    Private Sub lvStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvStatus.SelectedIndexChanged
        'no selection in here please :)
        sender.SelectedItems.Clear()
    End Sub

    Private Sub txtMovieName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMovieName.TextChanged
        CheckConditions()
    End Sub

    Private Sub cbUseFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUseFanart.CheckedChanged
        CheckConditions()
    End Sub

    Private Sub bwCreateHolder_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCreateHolder.DoWork
        Dim buildPath As String = Path.Combine(WorkingPath, "temp")
        Dim imgTemp As Bitmap
        Dim imgFinal As Bitmap
        Dim newGraphics As Graphics
        Me.bwCreateHolder.ReportProgress(0, "Preparing data")
        If Directory.Exists(buildPath) Then
            Directory.Delete(buildPath, True)
        End If
        Directory.CreateDirectory(buildPath)
        If cbUseFanart.Checked Then
            If File.Exists(Path.Combine(WorkingPath, "PlaceHolder-fanart.jpg")) Then
                imgTemp = Image.FromFile(Path.Combine(WorkingPath, "PlaceHolder-fanart.jpg"))
            Else
                imgTemp = Image.FromFile(Path.Combine(WorkingPath, "PlaceHolder.fanart.jpg"))
            End If
        Else
            'Default Image .. just hack this until i get a image
            imgTemp = New Bitmap(720, 576)
        End If
        'First let's resize it
        imgFinal = New Bitmap(720, 576)
        newGraphics = Graphics.FromImage(imgFinal)
        newGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        newGraphics.DrawImage(imgTemp, New Rectangle(0, 0, 720, 576), New Rectangle(0, 0, imgTemp.Width, imgTemp.Height), GraphicsUnit.Pixel)
        'Dont need this one anymore
        imgTemp.Dispose()
        'Save Master Image
        imgFinal.Save(Path.Combine(buildPath, "master.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg)
        ' Create string to draw.
        Dim drawString As [String] = txtTagline.Text
        Dim drawFont As New Font("Arial", 22, FontStyle.Bold)
        Dim drawBrush As New SolidBrush(Color.White)
        Dim drawPoint As New PointF(0.0F, 500.0F)
        Dim stringSize As New SizeF
        stringSize = newGraphics.MeasureString(drawString, drawFont)
        newGraphics.Dispose()
        Me.bwCreateHolder.ReportProgress(1, "Creating Movie")
        'Let cycle
        Dim f As Integer = 1
        For c As Integer = 756 To -stringSize.Width Step -2
            imgTemp = imgFinal.Clone
            newGraphics = Graphics.FromImage(imgTemp)
            drawPoint.X = c
            newGraphics.Save()
            newGraphics.DrawString(drawString, drawFont, drawBrush, drawPoint)
            imgTemp.Save(Path.Combine(buildPath, String.Format("image{0}.jpg", f)), System.Drawing.Imaging.ImageFormat.Jpeg)
            newGraphics.Dispose()
            imgTemp.Dispose()
            f += 1
        Next
        imgFinal.Dispose()
        If Me.bwCreateHolder.CancellationPending Then
            e.Cancel = True
            Return
        End If
        Me.bwCreateHolder.ReportProgress(2, "Building Movie")
        Using ffmpeg As New Process()
            ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
            ffmpeg.EnableRaisingEvents = False
            ffmpeg.StartInfo.UseShellExecute = False
            ffmpeg.StartInfo.CreateNoWindow = True
            ffmpeg.StartInfo.RedirectStandardOutput = True
            ffmpeg.StartInfo.RedirectStandardError = True
            'first get the duration
            ffmpeg.StartInfo.Arguments = String.Format(" -qscale 5 -r 25 -b 1200 -i ""{0}\image%d.jpg"" ""{1}""", buildPath, Master.currPath)
            ffmpeg.Start()
            ffmpeg.WaitForExit()
            ffmpeg.Close()
        End Using
        If Me.bwCreateHolder.CancellationPending Then
            e.Cancel = True
            Return
        End If
        Me.bwCreateHolder.ReportProgress(3, "Validating Files")


        If Me.bwCreateHolder.CancellationPending Then
            e.Cancel = True
            Return
        End If
        Me.bwCreateHolder.ReportProgress(4, "Moving Files")
        If Directory.Exists(buildPath) Then
            Directory.Delete(buildPath, True)
        End If
        myDirectoryCopy(Path.Combine(WorkingPath, "."), destPath, True)

        If Me.bwCreateHolder.CancellationPending Then
            e.Cancel = True
            Return
        End If
    End Sub
    Private Sub bwCreateHolder_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwCreateHolder.ProgressChanged
        If lvStatus.Items.Count > 0 Then
            lvStatus.Items(lvStatus.Items.Count - 1).SubItems.Add("Done")
        End If
        lvStatus.Items.Add(e.UserState)
    End Sub
    Private Sub bwCreateHolder_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwCreateHolder.RunWorkerCompleted
        If Not e.Cancelled Then
        End If
        MessageBox.Show("Place Hodler Created", "Offline Movie", MessageBoxButtons.OK)
        Me.Close()
    End Sub

    Private Sub Create_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Create_Button.Click
        cbSources.Enabled = False
        Create_Button.Enabled = False
        txtMovieName.Enabled = False
        txtTagline.Enabled = False
        cbUseFanart.Enabled = False
        Me.pbProgress.Value = 100
        Me.pbProgress.Style = ProgressBarStyle.Marquee
        Me.pbProgress.MarqueeAnimationSpeed = 25
        Me.pbProgress.Visible = True

        lvStatus.Items.Clear()
        Me.bwCreateHolder = New System.ComponentModel.BackgroundWorker
        Me.bwCreateHolder.WorkerReportsProgress = True
        Me.bwCreateHolder.WorkerSupportsCancellation = True
        Me.bwCreateHolder.RunWorkerAsync()
    End Sub

    Private Shared Sub myDirectoryCopy( _
    ByVal sourceDirName As String, _
    ByVal destDirName As String, _
    ByVal copySubDirs As Boolean)
        Dim dir As DirectoryInfo = New DirectoryInfo(sourceDirName)
        Dim dirs As DirectoryInfo() = dir.GetDirectories()
        ' If the source directory does not exist, throw an exception.
        If Not dir.Exists Then
            Throw New DirectoryNotFoundException( _
                "Source directory does not exist or could not be found: " _
                + sourceDirName)
        End If
        ' If the destination directory does not exist, create it.
        If Not Directory.Exists(destDirName) Then
            Directory.CreateDirectory(destDirName)
        End If
        ' Get the file contents of the directory to copy.
        Dim files As FileInfo() = dir.GetFiles()
        For Each file In files
            ' Create the path to the new copy of the file.
            Dim temppath As String = Path.Combine(destDirName, file.Name)
            ' Copy the file.
            file.CopyTo(temppath, False)
        Next file
        ' If copySubDirs is true, copy the subdirectories.
        If copySubDirs Then
            For Each subdir In dirs
                ' Create the subdirectory.
                Dim temppath As String = _
                    Path.Combine(destDirName, subdir.Name)
                ' Copy the subdirectories.
                myDirectoryCopy(subdir.FullName, temppath, copySubDirs)
            Next subdir
        End If
    End Sub

End Class
