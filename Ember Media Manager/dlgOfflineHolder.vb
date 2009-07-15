﻿' ################################################################################
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
    Private IMDB As New IMDB.Scraper
    Private WorkingPath As String = Path.Combine(Master.TempPath, "OfflineHolder")
    Private FileName As String = Path.Combine(WorkingPath, "PlaceHolder.avi")
    Private destPath As String
    Private idxStsSource As Integer = -1
    Private idxStsMovie As Integer = -1
    Private idxStsImage As Integer = -1
    Private Preview As Bitmap
    Private Overlay As New Images
    Private PreviewPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "OfflineDefault.jpg")
    Private OverlayPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Offlineoverlay.png")
    Private currText As String = "Insert DVD"
    Private prevText As String = String.Empty
    Private currNameText As String = String.Empty
    Private prevNameText As String = String.Empty
    '    Private currTopText As String
    '    Private prevTopText As String = String.Empty
    Private drawFont As New Font("Arial", 22, FontStyle.Bold)
    Private tMovie As New Master.DBMovie
    Private MovieName As String = String.Empty
    Friend WithEvents bwCreateHolder As New System.ComponentModel.BackgroundWorker
    Private Video_Width As Integer
    Private Video_Height As Integer
    Private RealImage_W As Integer
    Private RealImage_H As Integer
    Private RealImage_ratio As Double
    Private txtTopPos As Integer
    Private textHeight As SizeF
    Private def_pbPreview_w As Integer
    Private def_pbPreview_h As Integer

    Sub SetPreview(ByVal bDefault As Boolean, Optional ByVal path As String = "")
        Try

            If bDefault Then
                path = PreviewPath
                'txtTopPos = RealImage_H - 100
            End If
            If path <> "" Then
                'First let's resize it (Mantain aspect ratio)
                Dim tmpImg As Bitmap = New Bitmap(path)
                RealImage_W = Video_Width
                RealImage_ratio = tmpImg.Width / tmpImg.Height
                RealImage_H = RealImage_W / RealImage_ratio
                Preview = New Bitmap(RealImage_W, RealImage_H)
                Dim newGraphics = Graphics.FromImage(Preview)
                newGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                newGraphics.DrawImage(tmpImg, New Rectangle(0, 0, RealImage_W, RealImage_H), New Rectangle(0, 0, tmpImg.Width, tmpImg.Height), GraphicsUnit.Pixel)
                'Dont need this one anymore
                tmpImg.Dispose()
                drawFont = New Font("Arial", Convert.ToUInt16(22 / (1280 / Video_Width)), FontStyle.Bold)
                textHeight = newGraphics.MeasureString(txtTagline.Text, drawFont)
            Else
                RealImage_W = Video_Width
                RealImage_ratio = Preview.Width / Preview.Height
                RealImage_H = RealImage_W / RealImage_ratio
            End If
            If txtTopPos > RealImage_H Then
                txtTop.Text = (RealImage_H - 30).ToString
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub



    Private Sub Close_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CLOSE_Button.Click
        If bwCreateHolder.IsBusy Then
            bwCreateHolder.CancelAsync()
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub dlgOfflineHolder_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If Directory.Exists(WorkingPath) Then
            Master.DeleteDirectory(WorkingPath)
        End If
    End Sub

    Private Sub dlgOfflineHolder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            def_pbPreview_w = pbPreview.Width
            def_pbPreview_h = pbPreview.Height
            Video_Width = 1280
            Video_Height = 720
            cbFormat.SelectedIndex = 0
            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using

            'load all the movie folders from settings
            Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLNewcommand.CommandText = String.Concat("SELECT Name FROM Sources;")
                Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    While SQLReader.Read
                        Me.cbSources.Items.Add(SQLReader("Name"))
                    End While
                End Using
            End Using

            AddHandler IMDB.MovieInfoDownloaded, AddressOf MovieInfoDownloaded
            AddHandler IMDB.ProgressUpdated, AddressOf MovieInfoDownloadedPercent
            If Directory.Exists(WorkingPath) Then
                Master.DeleteDirectory(WorkingPath)
            End If
            Directory.CreateDirectory(WorkingPath)

            CreatePreview()
            tMovie.Movie = New Media.Movie
            tMovie.isSingle = True
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
            'tbTagLine.Value = tbTagLine.Maximum - 470 'Video_Height - 100
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub GetIMDB_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetIMDB_Button.Click
        Try
            Using dSearch As New dlgIMDBSearchResults
                Master.tmpMovie.Clear()
                tMovie.Movie.Clear()
                If dSearch.ShowDialog(txtMovieName.Text) = Windows.Forms.DialogResult.OK Then
                    If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                        Me.pbProgress.Value = 100
                        Me.pbProgress.Style = ProgressBarStyle.Marquee
                        Me.pbProgress.MarqueeAnimationSpeed = 25
                        Me.pbProgress.Visible = True
                        'Me.txtMovieName.Text = String.Format("{0} [OffLine]", Master.tmpMovie.Title)
                        Me.GetIMDB_Button.Enabled = False
                        IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, tMovie.Movie, Master.DefaultOptions, Master.eSettings.FullCrew, Master.eSettings.FullCast)
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
                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    If tmpImages.IsAllowedToDownload(tMovie, Master.ImageType.Posters) Then
                        Using dImgSelect As New dlgImgSelect
                            Dim pPath As String = dImgSelect.ShowDialog(tMovie, Master.ImageType.Posters)
                            If Not String.IsNullOrEmpty(pPath) Then
                                tMovie.PosterPath = pPath
                            End If
                        End Using
                    End If
                    If tmpImages.IsAllowedToDownload(tMovie, Master.ImageType.Fanart) Then
                        Using dImgSelect As New dlgImgSelect
                            Dim fPath As String = dImgSelect.ShowDialog(tMovie, Master.ImageType.Fanart)
                            If Not String.IsNullOrEmpty(fPath) Then
                                tMovie.FanartPath = fPath
                            End If
                        End Using
                    End If
                    tmpImages.Dispose()
                    tmpImages = Nothing
                End If
                ' Apply default pattern to the Folder Name
                Dim ff As New FileFolderRenamer.FileRename
                ff.Title = tMovie.Movie.Title
                ff.Year = tMovie.Movie.Year
                ff.FileName = ""
                Me.txtMovieName.Text = String.Concat(FileFolderRenamer.ProccessPattern(ff, Master.eSettings.FoldersPattern), " [Offline]")
                'Me.txtMovieName.Text = String.Format("{0} [OffLine]", tMovie.Movie.Title)
            Else
                MsgBox("Unable to retrieve movie details from the internet. Please check your connection and try again.", MsgBoxStyle.Exclamation, "Error Retrieving Details")
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.pbProgress.Visible = False
        Me.GetIMDB_Button.Enabled = True
        CheckConditions()
    End Sub

    Private Sub MovieInfoDownloadedPercent(ByVal iPercent As Integer)
        Me.pbProgress.Value = iPercent
        Me.Refresh()
    End Sub

    Private Sub cbSources_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSources.SelectedIndexChanged
        CheckConditions()
    End Sub

    Sub CheckConditions()
        Dim Fanart As New Images
        Try
            If txtMovieName.Text.IndexOfAny(Path.GetInvalidPathChars) <= 0 Then
                MovieName = txtMovieName.Text
                tMovie.Filename = Path.Combine(WorkingPath, String.Concat(MovieName, ".avi"))
            Else
                MovieName = txtMovieName.Text
                For Each Invalid As Char In Path.GetInvalidPathChars
                    MovieName = MovieName.Replace(Invalid, String.Empty)
                Next
                tMovie.Filename = Path.Combine(WorkingPath, String.Concat(MovieName, ".avi"))
            End If

            If cbSources.SelectedIndex >= 0 Then
                Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLNewcommand.CommandText = String.Concat("SELECT Path FROM Sources WHERE Name = """, cbSources.SelectedItem.ToString, """;")
                    Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                        If SQLReader.Read Then
                            destPath = Path.Combine(SQLReader("Path"), MovieName)
                            lvStatus.Items(idxStsSource).SubItems(1).Text = "Valid"
                            lvStatus.Items(idxStsSource).SubItems(1).ForeColor = Color.Green
                        End If
                    End Using
                End Using
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

            Dim fPath As String = tMovie.FanartPath

            If Not String.IsNullOrEmpty(fPath) Then
                chkUseFanart.Enabled = True
            Else
                chkUseFanart.Checked = False
                chkUseFanart.Enabled = False
            End If


            If chkUseFanart.Checked Then
                If Not String.IsNullOrEmpty(fPath) Then
                    SetPreview(False, fPath)
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

            Me.CreatePreview()
            If Not Me.pbProgress.Visible Then
                Create_Button.Enabled = True
                For Each i As ListViewItem In lvStatus.Items
                    If Not i.SubItems(1).ForeColor = Color.Green Then
                        Create_Button.Enabled = False
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
        End Try

        Fanart.Dispose()
        Fanart = Nothing
    End Sub

    Private Sub lvStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvStatus.SelectedIndexChanged
        'no selection in here please :)
        sender.SelectedItems.Clear()
    End Sub

    Private Sub txtMovieName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMovieName.TextChanged
        Me.currNameText = Me.txtMovieName.Text
        Me.tmrNameWait.Enabled = False
        Me.tmrNameWait.Enabled = True
    End Sub

    Private Sub tmrNameWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrNameWait.Tick
        If Me.prevNameText = Me.currNameText Then
            Me.tmrName.Enabled = True
        Else
            Me.prevNameText = Me.currNameText
        End If
    End Sub

    Private Sub tmrName_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrName.Tick
        Me.tmrNameWait.Enabled = False
        Me.CheckConditions()
        Me.tmrName.Enabled = False
    End Sub

    Private Sub cbUseFanart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseFanart.CheckedChanged
        If Not sender.checked Then
            If File.Exists(PreviewPath) Then
                SetPreview(True, PreviewPath)
                txtTop.Text = (Preview.Height - 150 / (1280 / Video_Width)).ToString
            End If
        Else
            Dim fPath As String = tMovie.FanartPath
            If Not fPath = String.Empty Then
                SetPreview(False, fPath)
            End If
        End If
        chkOverlay.Enabled = sender.checked
        chkBackground.Enabled = sender.checked
        If chkOverlay.Enabled Then
            chkOverlay.CheckState = CheckState.Checked
            chkBackground.CheckState = CheckState.Checked
        End If
        CreatePreview()

    End Sub

    Private Sub bwCreateHolder_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCreateHolder.DoWork
        Dim buildPath As String = Path.Combine(WorkingPath, "Temp")
        Dim imgTemp As Bitmap
        Dim imgFinal As Bitmap
        Dim newGraphics As Graphics
        Me.bwCreateHolder.ReportProgress(0, "Preparing data")
        If Directory.Exists(buildPath) Then
            Master.DeleteDirectory(buildPath)
        End If
        Directory.CreateDirectory(buildPath)

        If Not IsNothing(Preview) Then
            imgTemp = Preview
        Else
            imgTemp = New Bitmap(Video_Width, Video_Height)
        End If

        'First let's resize it (Mantain aspect ratio)
        imgFinal = New Bitmap(Video_Width, Convert.ToInt32(Video_Width / RealImage_ratio))
        newGraphics = Graphics.FromImage(imgFinal)
        newGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        newGraphics.DrawImage(imgTemp, New Rectangle(0, 0, Video_Width, Video_Width / (imgTemp.Width / imgTemp.Height)), New Rectangle(0, 0, imgTemp.Width, imgTemp.Height), GraphicsUnit.Pixel)
        'Dont need this one anymore
        imgTemp.Dispose()
        'Save Master Image
        imgFinal.Save(Path.Combine(buildPath, "master.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg)
        ' Create string to draw.
        Dim drawString As String = txtTagline.Text
        Dim drawBrush As New SolidBrush(btnTextColor.BackColor)
        Dim backgroundBrush As New SolidBrush(Color.FromArgb(180, btnBackgroundColor.BackColor.R, btnBackgroundColor.BackColor.G, btnBackgroundColor.BackColor.B))
        Dim drawPoint As New PointF(0.0F, txtTopPos)
        Dim stringSize As New SizeF
        stringSize = newGraphics.MeasureString(drawString, drawFont)
        newGraphics.Dispose()
        Me.bwCreateHolder.ReportProgress(1, "Creating Movie")
        'Let cycle
        Dim f As Integer = 1
        For c As Integer = Video_Width To -stringSize.Width Step -2
            imgTemp = imgFinal.Clone
            newGraphics = Graphics.FromImage(imgTemp)
            drawPoint.X = c
            newGraphics.Save()

            If chkBackground.Checked AndAlso chkUseFanart.Checked Then
                newGraphics.FillRectangle(backgroundBrush, 0, txtTopPos - 5, imgTemp.Width, stringSize.Height + 10)
            End If
            newGraphics.DrawString(drawString, drawFont, drawBrush, drawPoint)
            If chkOverlay.Checked AndAlso chkUseFanart.Checked Then
                newGraphics.DrawImage(Overlay.Image, 0, Convert.ToUInt16(txtTopPos - 65 / (1280 / Video_Width)), Convert.ToUInt16(Overlay.Image.Width / (1280 / Video_Width)), Convert.ToUInt16(Overlay.Image.Height / (1280 / Video_Width)))
            End If

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
            ffmpeg.StartInfo.Arguments = String.Format(" -qscale 5 -r 25 -b 1200 -i ""{0}\image%d.jpg"" ""{1}""", buildPath, tMovie.Filename)
            ffmpeg.Start()
            ffmpeg.WaitForExit()
            ffmpeg.Close()
        End Using

        If Me.bwCreateHolder.CancellationPending Then
            e.Cancel = True
            Return
        End If
        Me.bwCreateHolder.ReportProgress(4, "Moving Files")
        If Directory.Exists(buildPath) Then
            Master.DeleteDirectory(buildPath)
        End If

        DirectoryCopy(WorkingPath, destPath)

        tMovie.Filename = Path.Combine(destPath, String.Concat(MovieName, ".avi"))
        tMovie.PosterPath = Path.Combine(destPath, Path.GetFileName(tMovie.PosterPath).ToString)
        tMovie.FanartPath = Path.Combine(destPath, Path.GetFileName(tMovie.FanartPath).ToString)

        If Not String.IsNullOrEmpty(tMovie.Movie.Title) Then
            tMovie.ListTitle = tMovie.Movie.Title
        Else
            tMovie.ListTitle = MovieName.Replace("[Offline]", String.Empty).Trim
        End If

        tMovie = Master.DB.SaveMovieToDB(tMovie, True, False, True)

        Me.bwCreateHolder.ReportProgress(4, "Renaming Files")
        If Directory.Exists(buildPath) Then
            Master.DeleteDirectory(buildPath)
        End If
        Try
            FileFolderRenamer.RenameSingle(Path.Combine(destPath, Path.GetFileName(tMovie.Filename)), tMovie.Movie, "$D", "$D")
        Catch ex As Exception
        End Try

        If Me.bwCreateHolder.CancellationPending Then
            e.Cancel = True
            Return
        End If
        Me.bwCreateHolder.ReportProgress(5, "Finished")
    End Sub

    Private Sub bwCreateHolder_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwCreateHolder.ProgressChanged
        If lvStatus.Items.Count > 0 Then
            lvStatus.Items(lvStatus.Items.Count - 1).SubItems.Add("Done")
        End If
        lvStatus.Items.Add(e.UserState)
    End Sub

    Private Sub bwCreateHolder_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwCreateHolder.RunWorkerCompleted
        Me.pbProgress.Visible = False
        If Not e.Cancelled Then
            MsgBox("Offline movie place holder created!", MsgBoxStyle.OkOnly, "Offline Movie")
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
        Me.Close()
    End Sub

    Private Sub Create_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Create_Button.Click
        cbSources.Enabled = False
        Create_Button.Enabled = False
        txtMovieName.Enabled = False
        txtTagline.Enabled = False
        chkUseFanart.Enabled = False
        'Need to avoid cross thread in BackgroundWorker
        'txtTopPos = Video_Width / (pbPreview.Image.Width / Convert.ToSingle(currTopText)) ' ... and Scale it
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

    Private Sub btnTextColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTextColor.Click
        With Me.cdColor
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not .Color = Nothing Then
                    Me.btnTextColor.BackColor = .Color
                    Me.CreatePreview()
                End If
            End If
        End With
    End Sub

    Private Sub DirectoryCopy(ByVal sourceDirName As String, ByVal destDirName As String)
        Dim dir As New DirectoryInfo(sourceDirName)
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
        Dim Files As New List(Of FileInfo)

        Try
            Files.AddRange(dir.GetFiles())
        Catch
        End Try

        For Each sFile As FileInfo In Files
            Master.MoveFileWithStream(sFile.FullName, Path.Combine(destDirName, sFile.Name))
        Next

        Files = Nothing
        dir = Nothing
    End Sub

    Private Sub CreatePreview()
        If Preview Is Nothing Then Return
        Dim bmCloneOriginal As Bitmap = Preview.Clone
        tbTagLine.Maximum = Preview.Height - textHeight.Height
        If txtTopPos > tbTagLine.Maximum Then
            txtTopPos = tbTagLine.Maximum - 30
            txtTop.Text = txtTopPos
        End If
        tbTagLine.Value = tbTagLine.Maximum - txtTopPos
        Dim w, h As Integer
        w = pbPreview.Width
        h = Convert.ToInt16(pbPreview.Width / RealImage_ratio)
        If h > def_pbPreview_h Then
            h = pbPreview.Height
            w = Convert.ToInt16(pbPreview.Height * RealImage_ratio)
        End If

        pbPreview.Image = New Bitmap(w, h)

        Dim grOriginal As Graphics = Graphics.FromImage(bmCloneOriginal)
        Dim grPreview As Graphics = Graphics.FromImage(pbPreview.Image)
        Dim drawBrush As New SolidBrush(btnTextColor.BackColor)
        Dim backgroundBrush As New SolidBrush(Color.FromArgb(180, btnBackgroundColor.BackColor.R, btnBackgroundColor.BackColor.G, btnBackgroundColor.BackColor.B))
        'End If
        Dim iLeft As Integer = (bmCloneOriginal.Width - textHeight.Width) / 2
        If chkBackground.Checked AndAlso chkUseFanart.Checked Then
            grOriginal.FillRectangle(backgroundBrush, 0, txtTopPos - 5, bmCloneOriginal.Width, textHeight.Height + 10)
        End If
        grOriginal.DrawString(txtTagline.Text, drawFont, drawBrush, iLeft, txtTopPos)
        If chkOverlay.Checked AndAlso chkUseFanart.Checked Then
            grOriginal.DrawImage(Overlay.Image, 0, Convert.ToUInt16(txtTopPos - 65 / (1280 / Video_Width)), Convert.ToUInt16(Overlay.Image.Width / (1280 / Video_Width)), Convert.ToUInt16(Overlay.Image.Height / (1280 / Video_Width)))
        End If
        grPreview.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        grPreview.DrawImage(bmCloneOriginal, New Rectangle(0, 0, pbPreview.Width, pbPreview.Height), New Rectangle(0, 0, bmCloneOriginal.Width, bmCloneOriginal.Height), GraphicsUnit.Pixel)
        tbTagLine.Height = pbPreview.Image.Height + 7
    End Sub

    Private Sub txtTagline_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTagline.TextChanged
        Me.currText = Me.txtTagline.Text
    End Sub
    Private Sub btnFont_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFont.Click
        With Me.cdFont
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not IsNothing(.Font) Then
                    Me.drawFont = .Font
                    Me.CreatePreview()
                End If
            End If
        End With
    End Sub

    Private Sub txtTop_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTop.KeyPress
        If StringManip.NumericOnly(e.KeyChar) Then
            e.Handled = True
            Me.CheckConditions()
        Else
            e.Handled = False
            'Me.tmrTopWait.Enabled = False
            'Me.tmrTopWait.Enabled = True
        End If
    End Sub





    Private Sub dlgOfflineHolder_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbTagLine.Scroll
        txtTop.Text = sender.maximum - sender.value.ToString
    End Sub

    Private Sub btnBackgroundColor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBackgroundColor.Click
        With Me.cdColor
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not .Color = Nothing Then
                    Me.btnBackgroundColor.BackColor = .Color
                    Me.CreatePreview()
                End If
            End If
        End With
    End Sub

    Private Sub chkBackground_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBackground.CheckedChanged
        Me.CreatePreview()
        'CheckConditions()
    End Sub

    Private Sub chkOverlay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverlay.CheckedChanged
        If sender.checked Then
            Overlay.FromFile(OverlayPath)
        End If
        Me.CreatePreview()
        'CheckConditions()
    End Sub

    Private Sub cbFormat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFormat.SelectedIndexChanged
        Select Case (sender.selectedindex)
            Case 0
                Video_Width = 1280
                Video_Height = 720
            Case 1
                Video_Width = 720
                Video_Height = 576
        End Select
        SetPreview(Not chkUseFanart.Checked, String.Empty)
        'tbTagLine.Maximum = Preview.Height - textHeight.Height
        If Not chkUseFanart.Checked AndAlso Not Preview Is Nothing Then
            txtTop.Text = Convert.ToUInt16((Preview.Height - 150 / (1280 / Video_Width))).ToString
            'txtTopPos = Convert.ToUInt16(txtTop.Text)
        End If
        'tbTagLine.Value = tbTagLine.Maximum - txtTop.Text
        CreatePreview()
        'Me.CheckConditions()
    End Sub

    Private Sub txtTop_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTop.TextChanged
        Try
            txtTopPos = Convert.ToUInt16(txtTop.Text)
            '            tbTagLine.Value = tbTagLine.Maximum - txtTop.Text
            CreatePreview()
        Catch ex As Exception
        End Try

    End Sub
End Class
