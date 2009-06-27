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
    Private IMDB As New IMDB.Scraper
    Private WorkingPath As String = Path.Combine(Master.TempPath, "OfflineHolder")
    Private destPath As String
    Private idxStsSource As Integer = -1
    Private idxStsMovie As Integer = -1
    Private idxStsImage As Integer = -1
    Private Preview As New Images
    Private PreviewPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "OfflineDefault.jpg")
    Private currText As String = "Insert DVD"
    Private prevText As String = String.Empty
    Private currNameText As String = String.Empty
    Private prevNameText As String = String.Empty
    Private currTopText As String = "470"
    Private prevTopText As String = String.Empty
    Private drawFont As New Font("Arial", 22, FontStyle.Bold)
    Private txtTopPos As Integer
    Friend WithEvents bwCreateHolder As New System.ComponentModel.BackgroundWorker
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
            Directory.Delete(WorkingPath, True)
        End If
    End Sub

    Private Sub dlgOfflineHolder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using

            'load all the movie folders from settings
            Using SQLNewcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
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
                Directory.Delete(WorkingPath, True)
            End If
            Directory.CreateDirectory(WorkingPath)

            If File.Exists(PreviewPath) Then
                Preview.FromFile(PreviewPath)
                CreatePreview()
            End If

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
            tbTagLine.Value = tbTagLine.Maximum - 470
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub GetIMDB_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GetIMDB_Button.Click
        Try
            Master.tmpMovie = New Media.Movie

            Using dSearch As New dlgIMDBSearchResults
                If dSearch.ShowDialog(txtMovieName) = Windows.Forms.DialogResult.OK Then
                    If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                        Me.pbProgress.Value = 100
                        Me.pbProgress.Style = ProgressBarStyle.Marquee
                        Me.pbProgress.MarqueeAnimationSpeed = 25
                        Me.pbProgress.Visible = True
                        'Me.txtMovieName.Text = String.Format("{0} [OffLine]", Master.tmpMovie.Title)
                        IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.DefaultOptions, Master.eSettings.FullCrew, Master.eSettings.FullCast)
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
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.currSingle, Master.ImageType.Posters) Then
                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.ShowDialog(Master.tmpMovie.IMDBID, Master.currPath, Master.ImageType.Posters)
                        End Using
                    End If
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.currSingle, Master.ImageType.Fanart) Then
                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.ShowDialog(Master.tmpMovie.IMDBID, Master.currPath, Master.ImageType.Fanart)
                        End Using
                    End If
                    tmpImages.Dispose()
                    tmpImages = Nothing
                End If

                Master.SaveMovieToNFO(Master.tmpMovie, Master.currPath, False)

                Me.txtMovieName.Text = String.Format("{0} [OffLine]", Master.tmpMovie.Title)
            Else
                MsgBox("Unable to retrieve movie details from the internet. Please check your connection and try again.", MsgBoxStyle.Exclamation, "Error Retrieving Details")
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.pbProgress.Visible = False
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
        Dim MovieName As String = String.Empty
        Try

            If txtMovieName.Text.IndexOfAny(Path.GetInvalidPathChars) <= 0 Then
                MovieName = txtMovieName.Text
            Else
                MovieName = txtMovieName.Text
                For Each Invalid As Char In Path.GetInvalidPathChars
                    MovieName = MovieName.Replace(Invalid, String.Empty)
                Next
            End If
            If cbSources.SelectedIndex >= 0 Then
                destPath = Path.Combine(cbSources.SelectedItem.ToString, MovieName)
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

            Dim fPath As String = Fanart.GetFanartPath(Master.currPath, False)

            If Not String.IsNullOrEmpty(fPath) Then
                chkUseFanart.Enabled = True
            Else
                chkUseFanart.Checked = False
                chkUseFanart.Enabled = False
            End If

            If chkUseFanart.Checked Then
                If Not String.IsNullOrEmpty(fPath) Then
                    Preview.FromFile(fPath)
                    lvStatus.Items(idxStsImage).SubItems(1).Text = "Valid"
                    lvStatus.Items(idxStsImage).SubItems(1).ForeColor = Color.Green
                Else
                    lvStatus.Items(idxStsImage).SubItems(1).Text = "Invalid"
                    lvStatus.Items(idxStsImage).SubItems(1).ForeColor = Color.Red
                End If
            Else
                'If File.Exists(PreviewPath) Then
                'txtTop.Text = "470"
                'Preview.FromFile(PreviewPath)
                'End If

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
                txtTop.Text = "470"
                Preview.FromFile(PreviewPath)
            End If
        End If
        CheckConditions()
    End Sub

    Private Sub bwCreateHolder_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCreateHolder.DoWork
        Dim buildPath As String = Path.Combine(WorkingPath, "Temp")
        Dim imgTemp As Bitmap
        Dim imgFinal As Bitmap
        Dim newGraphics As Graphics
        Me.bwCreateHolder.ReportProgress(0, "Preparing data")
        If Directory.Exists(buildPath) Then
            Directory.Delete(buildPath, True)
        End If
        Directory.CreateDirectory(buildPath)

        If Not IsNothing(Preview.Image) Then
            imgTemp = Preview.Image
        Else
            imgTemp = New Bitmap(720, 576)
        End If

        'First let's resize it (Mantain aspect ratio)
        imgFinal = New Bitmap(720, Convert.ToInt32(720 / (imgTemp.Width / imgTemp.Height)))
        newGraphics = Graphics.FromImage(imgFinal)
        newGraphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        newGraphics.DrawImage(imgTemp, New Rectangle(0, 0, 720, 720 / (imgTemp.Width / imgTemp.Height)), New Rectangle(0, 0, imgTemp.Width, imgTemp.Height), GraphicsUnit.Pixel)
        'Dont need this one anymore
        imgTemp.Dispose()
        'Save Master Image
        imgFinal.Save(Path.Combine(buildPath, "master.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg)
        ' Create string to draw.
        Dim drawString As String = txtTagline.Text
        Dim drawBrush As New SolidBrush(btnTextColor.BackColor)
        Dim drawPoint As New PointF(0.0F, txtTopPos)
        Dim stringSize As New SizeF
        stringSize = newGraphics.MeasureString(drawString, drawFont)
        newGraphics.Dispose()
        Me.bwCreateHolder.ReportProgress(1, "Creating Movie")
        'Let cycle
        Dim f As Integer = 1
        For c As Integer = 720 To -stringSize.Width Step -2
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
            ffmpeg.StartInfo.Arguments = String.Format(" -qscale 5 -r 25 -b 1200 -i ""{0}\image%d.jpg"" ""{1}""", buildPath, Master.currPath)
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
            Directory.Delete(buildPath, True)
        End If

        DirectoryCopy(WorkingPath, destPath)
        Me.bwCreateHolder.ReportProgress(4, "Renaming Files")
        If Directory.Exists(buildPath) Then
            Directory.Delete(buildPath, True)
        End If
        Try
            FileFolderRenamer.RenameSingle(Path.Combine(destPath, Path.GetFileName(Master.currPath)), Master.tmpMovie, "$D", "$D")
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
        txtTopPos = 720 / (pbPreview.Image.Width / Convert.ToSingle(currTopText)) ' ... and Scale it
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
        Dim bmPreview As New Bitmap(Me.Preview.Image)
        Dim grPreview As Graphics = Graphics.FromImage(bmPreview)
        Dim drawString As String = txtTagline.Text
        Dim drawBrush As New SolidBrush(btnTextColor.BackColor)
        'Dim drawPoint As New PointF(0.0F, 470.0F)
        tbTagLine.Maximum = bmPreview.Height - grPreview.MeasureString(drawString, drawFont).Height
        If Convert.ToInt32(txtTop.Text) > bmPreview.Height - grPreview.MeasureString(drawString, drawFont).Height Then
            txtTop.Text = Convert.ToInt32(bmPreview.Height - grPreview.MeasureString(drawString, drawFont).Height - 10).ToString
        End If
        Dim iLeft As Integer = (bmPreview.Width - grPreview.MeasureString(drawString, drawFont).Width) / 2
        grPreview.DrawString(drawString, drawFont, drawBrush, iLeft, Convert.ToInt32(txtTop.Text))
        pbPreview.Image = bmPreview
        If tbTagLine.Tag Is Nothing Then tbTagLine.Tag = 0
        If Not tbTagLine.Tag = bmPreview.Height Then
            tbTagLine.Top = GroupBox1.Top + 10 + (pbPreview.Height - (bmPreview.Height / (bmPreview.Width / pbPreview.Width))) / 2
            tbTagLine.Height = (bmPreview.Height / (bmPreview.Width / pbPreview.Width)) + 16
        End If
        tbTagLine.Tag = bmPreview.Height
        tbTagLine.Value = tbTagLine.Maximum - Convert.ToInt16(txtTop.Text)

    End Sub

    Private Sub txtTagline_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTagline.TextChanged
        Me.currText = Me.txtTagline.Text

        Me.tmrWait.Enabled = False
        Me.tmrWait.Enabled = True
    End Sub

    Private Sub tmrWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrWait.Tick
        If Me.prevText = Me.currText Then
            Me.tmrPreview.Enabled = True
        Else
            Me.prevText = Me.currText
        End If
    End Sub

    Private Sub tmrPreview_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrPreview.Tick
        Me.tmrWait.Enabled = False
        Me.CheckConditions()
        Me.tmrPreview.Enabled = False
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
        If Master.NumericOnly(Asc(e.KeyChar)) Then
            e.Handled = True
        Else
            e.Handled = False
            Me.tmrTopWait.Enabled = False
            Me.tmrTopWait.Enabled = True
        End If
    End Sub

    Private Sub tmrTopWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrTopWait.Tick
        If Me.prevTopText = Me.currTopText Then
            Me.tmrTop.Enabled = True
        Else
            Me.prevTopText = Me.currTopText
            Me.currTopText = Me.txtTop.Text
        End If
    End Sub

    Private Sub tmrTop_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrTop.Tick
        Me.tmrTopWait.Enabled = False
        Me.CheckConditions()
        Me.tmrTop.Enabled = False
    End Sub

    Private Sub dlgOfflineHolder_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub TrackBar1_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbTagLine.Scroll
        txtTop.Text = sender.maximum - sender.value.ToString
        'Me.currTopText = sender.value.ToString
        Me.prevTopText = Me.currTopText
        Me.currTopText = Me.txtTop.Text
        Me.CheckConditions()
    End Sub

End Class
