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

Imports System.Windows.Forms
Imports System.IO
Imports System.IO.Compression
Imports System.Text
Imports System.Text.RegularExpressions

Public Class dlgImgSelect
    Private TMDB As New TMDB.Scraper
    Private IMPA As New IMPA.Scraper
    Friend WithEvents bwIMPADownload As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwTMDBDownload As New System.ComponentModel.BackgroundWorker

    Private imdbID As String
    Private sPath As String
    Private sRealPath As String
    Private DLType As Master.ImageType

    Private iCounter As Integer = 0
    Private iTop As Integer = 5
    Private iLeft As Integer = 5

    Private pbImage() As PictureBox
    Private pnlImage() As Panel
    Private lblImage() As Label
    Private chkImage() As CheckBox
    Private tmpImage As Image = Nothing
    Private selIndex As Integer = -1

    Private IMPAPosters As New List(Of Media.Image)
    Private TMDBPosters As New List(Of Media.Image)

    Private _impaDone As Boolean = False
    Private _tmdbDone As Boolean = False
    Private Event IMPADone()
    Private Event TMDBDone()


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Try
            If Not IsNothing(Me.tmpImage) Then
                Dim fs As New FileStream(Me.sPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                Me.tmpImage.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg)
                fs.Close()
                fs = Nothing
            Else
                Select Case True
                    Case Me.rbXLarge.Checked
                        Me.pnlBG.Visible = False
                        Me.pnlSinglePic.Visible = True
                        Me.Refresh()
                        Application.DoEvents()
                        Me.DownloadSinglePic(Me.rbXLarge.Tag, tmpImage)
                    Case Me.rbLarge.Checked
                        Me.pnlBG.Visible = False
                        Me.pnlSinglePic.Visible = True
                        Me.Refresh()
                        Application.DoEvents()
                        Me.DownloadSinglePic(Me.rbLarge.Tag, tmpImage)
                    Case Me.rbMedium.Checked
                        Me.pnlBG.Visible = False
                        Me.pnlSinglePic.Visible = True
                        Me.Refresh()
                        Application.DoEvents()
                        tmpImage = Me.pbImage(selIndex).Image
                    Case Me.rbSmall.Checked
                        Me.pnlBG.Visible = False
                        Me.pnlSinglePic.Visible = True
                        Me.Refresh()
                        Application.DoEvents()
                        Me.DownloadSinglePic(Me.rbSmall.Tag, tmpImage)
                End Select
                If Not IsNothing(tmpImage) Then
                    Dim fs As New FileStream(Me.sPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
                    tmpImage.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg)
                    fs.Close()
                    fs = Nothing
                End If
                Me.pnlSinglePic.Visible = False
            End If

            If Me.DLType = Master.ImageType.Fanart Then
                Dim iMod As Integer = Master.GetExtraModifier(Me.sRealPath)
                If iMod = -1 Then iMod = 0
                Dim iVal As Integer = 1
                If Me.chkImage.Count > 0 Then

                    If Not Directory.Exists(String.Concat(Directory.GetParent(Me.sPath).FullName.ToString, "\extrathumbs")) Then
                        Directory.CreateDirectory(String.Concat(Directory.GetParent(Me.sPath).FullName.ToString, "\extrathumbs"))
                    End If

                    For i As Integer = 0 To UBound(Me.chkImage)
                        If Me.chkImage(i).Checked Then
                            Dim fsET As New FileStream(String.Concat(Directory.GetParent(Me.sPath).FullName.ToString, "\extrathumbs\thumb", iVal + iMod, ".jpg"), FileMode.OpenOrCreate, FileAccess.ReadWrite)
                            Me.pbImage(i).Image.Save(fsET, System.Drawing.Imaging.ImageFormat.Jpeg)
                            fsET.Close()
                            fsET = Nothing
                            iVal += 1
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Dispose()
        Me.Close()
    End Sub

    Private Sub dlgImgSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            AddHandler TMDB.PostersDownloaded, AddressOf TMDBPostersDownloaded
            AddHandler TMDB.ProgressUpdated, AddressOf TMDBProgressUpdated
            AddHandler IMPA.PostersDownloaded, AddressOf IMPAPostersDownloaded
            AddHandler IMPA.ProgressUpdated, AddressOf IMPAProgressUpdated
            AddHandler IMPADone, AddressOf IMPADoneDownloading
            AddHandler TMDBDone, AddressOf TMDBDoneDownloading

            If Me.DLType = Master.ImageType.Posters Then
                If Master.uSettings.UseTMDB AndAlso Master.uSettings.UseIMPA Then
                    Me.pnlDLStatus.Height = 150
                    Me.pnlBottom.Visible = True
                Else
                    Me.pnlDLStatus.Height = 75
                    If Master.uSettings.UseIMPA Then
                        Me.pnlBottom.Visible = True
                    End If
                End If
            Else
                Me.pnlDLStatus.Height = 75
                Me.pnlBottom.Visible = False
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub TMDBDoneDownloading()

        Try
            If Me.DLType = Master.ImageType.Posters Then
                Me._tmdbDone = True
                If Master.uSettings.UseIMPA Then
                    If Me._impaDone Then
                        Me.pnlDLStatus.Visible = False
                        Me.TMDBPosters.AddRange(Me.IMPAPosters)
                        Me.ProcessPics(Me.TMDBPosters)
                        Me.pnlBG.Visible = True
                    End If
                Else
                    Me.pnlDLStatus.Visible = False
                    Me.ProcessPics(Me.TMDBPosters)
                    Me.pnlBG.Visible = True
                End If
            Else
                Me.pnlDLStatus.Visible = False
                Me.ProcessPics(Me.TMDBPosters)
                Me.pnlBG.Visible = True
                Me.btnCheckAll.Visible = True
                Me.btnCheckNone.Visible = True
                Me.lblInfo.Visible = True
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub IMPADoneDownloading()
        Try
            Me._impaDone = True
            If Master.uSettings.UseTMDB Then
                If Me._tmdbDone = True Then
                    Me.pnlDLStatus.Visible = False
                    Me.TMDBPosters.AddRange(Me.IMPAPosters)
                    Me.ProcessPics(Me.TMDBPosters)
                    Me.pnlBG.Visible = True
                End If
            Else
                Me.pnlDLStatus.Visible = False
                Me.ProcessPics(Me.IMPAPosters)
                Me.pnlBG.Visible = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub ProcessPics(ByVal posters As List(Of Media.Image))
        Try
            Dim iIndex As Integer = 0
            If posters.Count > 0 Then
                For i As Integer = 0 To posters.Count - 1
                    If Not IsNothing(posters.Item(i).WebImage) Then
                        Me.AddImage(posters.Item(i).WebImage, posters.Item(i).Description, iIndex, posters.Item(i).URL)
                        iIndex += 1
                    End If
                Next
            Else
                If Me.DLType = Master.ImageType.Fanart Then
                    MsgBox("No Fanart found for this movie", MsgBoxStyle.Information, "No Fanart Found")
                Else
                    MsgBox("No Posters found for this movie", MsgBoxStyle.Information, "No Posters Found")
                End If
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.Dispose()
                Me.Close()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub TMDBProgressUpdated(ByVal iPercent As Integer)
        Me.pbDL1.Value = iPercent
    End Sub

    Private Sub TMDBPostersDownloaded(ByVal Posters As List(Of Media.Image))

        Try
            Me.pbDL1.Value = 0

            Me.lblDL1.Text = "Preparing images..."
            Me.lblDL1Status.Text = String.Empty
            Me.pbDL1.Maximum = Posters.Count

            Me.TMDBPosters = Posters

            Me.bwTMDBDownload.WorkerReportsProgress = True
            Me.bwTMDBDownload.RunWorkerAsync()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub IMPAProgressUpdated(ByVal iPercent As Integer)
        Me.pbDL2.Value = iPercent
    End Sub

    Private Sub IMPAPostersDownloaded(ByVal Posters As List(Of Media.Image))

        Try
            Me.pbDL2.Value = 0

            Me.lblDL2.Text = "Preparing images..."
            Me.lblDL2Status.Text = String.Empty
            Me.pbDL2.Maximum = Posters.Count

            Me.IMPAPosters = Posters

            Me.bwIMPADownload.WorkerReportsProgress = True
            Me.bwIMPADownload.RunWorkerAsync()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub GetPosters()

        Try
            If Master.uSettings.UseTMDB Then
                Me.lblDL1.Text = "Retrieving data from TheMovieDB.com..."
                Me.lblDL1Status.Text = String.Empty
                Me.pbDL1.Maximum = 3
                Me.pnlDLStatus.Visible = True
                Me.Refresh()

                Me.TMDB.GetImagesAsync(imdbID, "poster")
            End If

            If Master.uSettings.UseIMPA Then
                Me.lblDL2.Text = "Retrieving data from IMPAwards.com..."
                Me.lblDL2Status.Text = String.Empty
                Me.pbDL2.Maximum = 3
                Me.pnlDLStatus.Visible = True
                Me.Refresh()

                Me.IMPA.GetImagesAsync(imdbID)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub GetFanart()
        Try
            If Master.uSettings.UseTMDB Then
                Me.lblDL1.Text = "Retrieving data from TheMovieDB.com..."
                Me.lblDL1Status.Text = String.Empty
                Me.pbDL1.Maximum = 3
                Me.pnlDLStatus.Visible = True
                Me.Refresh()

                Me.TMDB.GetImagesAsync(imdbID, "backdrop")
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub AddImage(ByVal iImage As Image, ByVal sDescription As String, ByVal iIndex As Integer, ByVal sURL As String)

        Try
            ReDim Preserve Me.pnlImage(iIndex)
            ReDim Preserve Me.pbImage(iIndex)
            Me.pnlImage(iIndex) = New Panel()
            Me.pbImage(iIndex) = New PictureBox()
            Me.pbImage(iIndex).Name = iIndex
            Me.pnlImage(iIndex).Name = iIndex
            Me.pnlImage(iIndex).Size = New Size(256, 286)
            Me.pbImage(iIndex).Size = New Size(250, 250)
            Me.pnlImage(iIndex).BackColor = Color.White
            Me.pnlImage(iIndex).BorderStyle = BorderStyle.FixedSingle
            Me.pbImage(iIndex).SizeMode = PictureBoxSizeMode.Zoom
            Me.pnlImage(iIndex).Tag = sURL
            Me.pbImage(iIndex).Tag = sURL
            Me.pbImage(iIndex).Image = iImage
            Me.pnlImage(iIndex).Left = iLeft
            Me.pbImage(iIndex).Left = 3
            Me.pnlImage(iIndex).Top = iTop
            Me.pbImage(iIndex).Top = 3
            Me.pnlBG.Controls.Add(Me.pnlImage(iIndex))
            Me.pnlImage(iIndex).Controls.Add(Me.pbImage(iIndex))
            Me.pnlImage(iIndex).BringToFront()
            AddHandler pbImage(iIndex).Click, AddressOf pbImage_Click
            AddHandler pnlImage(iIndex).Click, AddressOf pnlImage_Click

            If Me.DLType = Master.ImageType.Fanart Then
                ReDim Preserve Me.chkImage(iIndex)
                Me.chkImage(iIndex) = New CheckBox()
                Me.chkImage(iIndex).Name = iIndex
                Me.chkImage(iIndex).Size = New Size(250, 30)
                Me.chkImage(iIndex).AutoSize = False
                Me.chkImage(iIndex).BackColor = Color.White
                Me.chkImage(iIndex).TextAlign = ContentAlignment.MiddleCenter
                Me.chkImage(iIndex).Text = String.Format("{0}x{1} ({2})", Me.pbImage(iIndex).Image.Width.ToString, Me.pbImage(iIndex).Image.Height.ToString, sDescription)
                Me.chkImage(iIndex).Left = 0
                Me.chkImage(iIndex).Top = 250
                Me.pnlImage(iIndex).Controls.Add(Me.chkImage(iIndex))
            Else
                ReDim Preserve Me.lblImage(iIndex)
                Me.lblImage(iIndex) = New Label()
                Me.lblImage(iIndex).Name = iIndex
                Me.lblImage(iIndex).Size = New Size(250, 30)
                Me.lblImage(iIndex).AutoSize = False
                Me.lblImage(iIndex).BackColor = Color.White
                Me.lblImage(iIndex).TextAlign = ContentAlignment.MiddleCenter
                If sURL.ToLower.Contains("themoviedb.org") Then
                    Me.lblImage(iIndex).Text = "Multiple"
                Else
                    Me.lblImage(iIndex).Text = String.Format("{0}x{1} ({2})", Me.pbImage(iIndex).Image.Width.ToString, Me.pbImage(iIndex).Image.Height.ToString, sDescription)
                End If
                Me.lblImage(iIndex).Tag = sURL
                Me.lblImage(iIndex).Left = 0
                Me.lblImage(iIndex).Top = 250
                Me.pnlImage(iIndex).Controls.Add(Me.lblImage(iIndex))
                AddHandler lblImage(iIndex).Click, AddressOf lblImage_Click
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.iCounter += 1

        If Me.iCounter = 3 Then
            Me.iCounter = 0
            Me.iLeft = 5
            Me.iTop += 301
        Else
            Me.iLeft += 271
        End If

    End Sub

    Private Sub DoSelect(ByVal iIndex As Integer, ByVal sURL As String)

        Try
            'set all pnl colors to white first
            'remove all the current genres
            For i As Integer = 0 To UBound(Me.pnlImage)
                Me.pnlImage(i).BackColor = Color.White

                If DLType = Master.ImageType.Fanart Then
                    Me.chkImage(i).BackColor = Color.White
                    Me.chkImage(i).ForeColor = Color.Black
                Else
                    Me.lblImage(i).BackColor = Color.White
                    Me.lblImage(i).ForeColor = Color.Black
                End If
            Next
            'set selected pnl color to blue
            Me.pnlImage(iIndex).BackColor = Color.Blue

            If DLType = Master.ImageType.Fanart Then
                Me.chkImage(iIndex).BackColor = Color.Blue
                Me.chkImage(iIndex).ForeColor = Color.White
            Else
                Me.lblImage(iIndex).BackColor = Color.Blue
                Me.lblImage(iIndex).ForeColor = Color.White
            End If

            Me.selIndex = iIndex

            If Not Me.DLType = Master.ImageType.Fanart AndAlso sURL.ToLower.Contains("themoviedb.org") Then
                Me.SetupSizes(sURL)
                Me.OK_Button.Enabled = False
                Me.tmpImage = Nothing
            Else
                Me.pnlSize.Visible = False
                Me.rbXLarge.Checked = False
                Me.rbLarge.Checked = False
                Me.rbMedium.Checked = False
                Me.rbSmall.Checked = False
                Me.OK_Button.Enabled = True
                Me.tmpImage = Me.pbImage(iIndex).Image
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetupSizes(ByVal sURL As String)
        Dim sLeft As String = Strings.Left(sURL, sURL.Length - 10)

        Me.rbXLarge.Checked = False
        Me.rbXLarge.Enabled = False
        Me.rbLarge.Checked = False
        Me.rbLarge.Enabled = False
        Me.rbMedium.Checked = False
        Me.rbSmall.Checked = False
        Me.rbSmall.Enabled = False

        Me.rbMedium.Tag = sURL

        For i As Integer = 0 To Me.TMDBPosters.Count - 1
            ' medium - - already downloaded as "cover" so we know it's there
            Select Case True
                Case Me.TMDBPosters.Item(i).URL = String.Concat(sLeft, ".jpg")
                    ' xlarge
                    Me.rbXLarge.Enabled = True
                    Me.rbXLarge.Tag = Me.TMDBPosters.Item(i).URL
                Case Me.TMDBPosters.Item(i).URL = String.Concat(sLeft, "_mid.jpg")
                    ' large 
                    Me.rbLarge.Enabled = True
                    Me.rbLarge.Tag = Me.TMDBPosters.Item(i).URL
                Case TMDBPosters.Item(i).URL = String.Concat(sLeft, "_thumb.jpg")
                    ' small
                    Me.rbSmall.Enabled = True
                    Me.rbSmall.Tag = Me.TMDBPosters.Item(i).URL
            End Select
        Next

        pnlSize.Visible = True
    End Sub

    Private Sub pbImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DoSelect(CInt(sender.Name), sender.Tag)
    End Sub

    Private Sub pnlImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DoSelect(CInt(sender.Name), sender.Tag)
    End Sub

    Private Sub lblImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DoSelect(CInt(sender.Name), sender.Tag)
    End Sub

    Private Sub dlgImgSelect_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try
            Select Case Me.DLType
                Case Master.ImageType.Posters
                    Me.Text = "Select Poster"
                    Me.GetPosters()
                Case Master.ImageType.Fanart
                    Me.Text = "Select Fanart"
                    Me.GetFanart()
            End Select
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwIMPADownload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwIMPADownload.DoWork

        '//
        ' Thread to download impa posters from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\
        For i As Integer = 0 To Me.IMPAPosters.Count - 1
            Try
                Me.bwIMPADownload.ReportProgress(i + 1, Me.IMPAPosters.Item(i).URL)
                Dim wrRequest As WebRequest = WebRequest.Create(Me.IMPAPosters.Item(i).URL)
                Dim wrResponse As WebResponse = wrRequest.GetResponse()
                If wrResponse.ContentType = "image/jpeg" Then
                    Me.IMPAPosters.Item(i).WebImage = Image.FromStream(wrResponse.GetResponseStream)
                End If
                wrResponse.Close()
                wrResponse = Nothing
                wrRequest = Nothing
            Catch
            End Try
        Next

    End Sub

    Private Sub bwIMPADownload_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwIMPADownload.ProgressChanged

        '//
        ' Update the status bar with the name of the current media name and increase progress bar
        '\\
        Try
            Dim sStatus As String = e.UserState.ToString
            Me.lblDL2Status.Text = String.Format("Downloading {0}", If(sStatus.Length > 40, Master.TruncateURL(sStatus, 40), sStatus))
            Me.pbDL2.Value = e.ProgressPercentage
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwIMPADownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwIMPADownload.RunWorkerCompleted

        '//
        ' Thread finished: process the pics
        '\\

        RaiseEvent IMPADone()

    End Sub

    Private Sub bwTMDBDownload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwTMDBDownload.DoWork

        '//
        ' Thread to download tmdb posters from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\

        For i As Integer = 0 To Me.TMDBPosters.Count - 1
            Try
                If Me.DLType = Master.ImageType.Fanart OrElse (Me.DLType = Master.ImageType.Posters AndAlso Me.TMDBPosters.Item(i).Description.ToLower = "cover") Then
                    Me.bwTMDBDownload.ReportProgress(i + 1, Me.TMDBPosters.Item(i).URL)
                    Dim wrRequest As WebRequest = WebRequest.Create(Me.TMDBPosters.Item(i).URL)
                    Dim wrResponse As WebResponse = wrRequest.GetResponse()
                    If wrResponse.ContentType = "image/jpeg" Then
                        Me.TMDBPosters.Item(i).WebImage = Image.FromStream(wrResponse.GetResponseStream)
                    End If
                    wrResponse.Close()
                    wrResponse = Nothing
                    wrRequest = Nothing
                End If
            Catch
            End Try
        Next

    End Sub

    Private Sub bwTMDBDownload_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwTMDBDownload.ProgressChanged

        '//
        ' Update the status bar with the name of the current media name and increase progress bar
        '\\
        Try
            Dim sStatus As String = e.UserState.ToString
            Me.lblDL1Status.Text = String.Format("Downloading {0}", If(sStatus.Length > 40, Master.TruncateURL(sStatus, 40), sStatus))
            Me.pbDL1.Value = e.ProgressPercentage
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwTMDBDownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwTMDBDownload.RunWorkerCompleted

        '//
        ' Thread finished: process the pics
        '\\

        RaiseEvent TMDBDone()

    End Sub

    Private Sub DownloadSinglePic(ByVal sURL As String, ByRef tImage As Image)
        Dim wrRequest As WebRequest = WebRequest.Create(sURL)
        Dim wrResponse As WebResponse = wrRequest.GetResponse()
        tImage = Image.FromStream(wrResponse.GetResponseStream)
        wrResponse.Close()
        wrResponse = Nothing
        wrRequest = Nothing
    End Sub

    Private Sub btnCheckAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckAll.Click

        For i As Integer = 0 To UBound(Me.chkImage)
            Me.chkImage(i).Checked = True
        Next

    End Sub

    Private Sub btnCheckNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckNone.Click

        For i As Integer = 0 To UBound(Me.chkImage)
            Me.chkImage(i).Checked = False
        Next

    End Sub

    Private Sub rbXLarge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbXLarge.CheckedChanged
        Me.OK_Button.Enabled = True
    End Sub

    Private Sub rbLarge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLarge.CheckedChanged
        Me.OK_Button.Enabled = True
    End Sub

    Private Sub rbMedium_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMedium.CheckedChanged
        Me.OK_Button.Enabled = True
    End Sub

    Private Sub rbSmall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSmall.CheckedChanged
        Me.OK_Button.Enabled = True
    End Sub

    Public Overloads Function ShowDialog(ByVal _imdbID As String, ByVal _sPath As String, ByVal _sRealPath As String, ByVal _DLType As Master.ImageType) As Windows.Forms.DialogResult

        '//
        ' Overload to pass data
        '\\

        Me.imdbID = _imdbID
        Me.sPath = _sPath
        Me.sRealPath = _sRealPath
        Me.DLType = _DLType

        Return MyBase.ShowDialog()
    End Function

End Class


