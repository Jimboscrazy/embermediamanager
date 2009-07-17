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
Imports System.IO.Compression
Imports System.Text
Imports System.Text.RegularExpressions

Public Class dlgImgSelect
    Private TMDB As New TMDB.Scraper
    Private IMPA As New IMPA.Scraper
    Private MPDB As New MPDB.Scraper

    Friend WithEvents bwIMPADownload As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwTMDBDownload As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwMPDBDownload As New System.ComponentModel.BackgroundWorker

    Private tMovie As New Master.DBMovie
    Private isEdit As Boolean = False
    Private DLType As Master.ImageType
    Private PreDL As Boolean = False

    Private iCounter As Integer = 0
    Private iTop As Integer = 5
    Private iLeft As Integer = 5

    Private pbImage() As PictureBox
    Private pnlImage() As Panel
    Private lblImage() As Label
    Private chkImage() As CheckBox
    Private tmpImage As New Images
    Private selIndex As Integer = -1

    Private IMPAPosters As New List(Of Media.Image)
    Private TMDBPosters As New List(Of Media.Image)
    Private MPDBPosters As New List(Of Media.Image)

    Private _impaDone As Boolean = True
    Private _tmdbDone As Boolean = True
    Private _mpdbDone As Boolean = True

    Private Event IMPADone()
    Private Event TMDBDone()
    Private Event MPDBDone()

    Private CachePath As String = String.Empty
    Private OutPath As String = String.Empty

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Try
            Dim tmpPathPlus As String = String.Empty

            If DLType = Master.ImageType.Fanart Then
                tmpPathPlus = Path.Combine(Master.TempPath, "fanart.jpg")
            Else
                tmpPathPlus = Path.Combine(Master.TempPath, "poster.jpg")
            End If

            If Not IsNothing(Me.tmpImage.Image) Then
                If isEdit Then
                    Me.tmpImage.Save(tmpPathPlus)
                    OutPath = tmpPathPlus
                Else
                    If Me.DLType = Master.ImageType.Fanart Then
                        OutPath = Me.tmpImage.SaveAsFanart(tMovie)
                    Else
                        OutPath = Me.tmpImage.SaveAsPoster(tMovie)
                    End If
                End If
            Else
                Me.pnlBG.Visible = False
                Me.pnlSinglePic.Visible = True
                Me.Refresh()
                Application.DoEvents()
                Select Case True
                    Case Me.rbXLarge.Checked
                        If Master.eSettings.UseImgCache Then
                            Me.tmpImage.FromFile(Path.Combine(CachePath, String.Concat("poster_(original)_(url=", Me.rbXLarge.Tag, ").jpg")))
                        Else
                            Me.tmpImage.FromWeb(Me.rbXLarge.Tag)
                        End If
                    Case Me.rbLarge.Checked
                        If Master.eSettings.UseImgCache Then
                            Me.tmpImage.FromFile(Path.Combine(CachePath, String.Concat("poster_(mid)_(url=", Me.rbLarge.Tag, ").jpg")))
                        Else
                            Me.tmpImage.FromWeb(Me.rbLarge.Tag)
                        End If
                    Case Me.rbMedium.Checked
                        Me.tmpImage.Image = Me.pbImage(selIndex).Image
                    Case Me.rbSmall.Checked
                        If Master.eSettings.UseImgCache Then
                            Me.tmpImage.FromFile(Path.Combine(CachePath, String.Concat("poster_(thumb)_(url=", Me.rbSmall.Tag, ").jpg")))
                        Else
                            Me.tmpImage.FromWeb(Me.rbSmall.Tag)
                        End If
                End Select

                If Not IsNothing(Me.tmpImage.Image) Then
                    If isEdit Then
                        Me.tmpImage.Save(tmpPathPlus)
                        OutPath = tmpPathPlus
                    Else
                        If Me.DLType = Master.ImageType.Fanart Then
                            OutPath = Me.tmpImage.SaveAsFanart(Me.tMovie)
                        Else
                            OutPath = Me.tmpImage.SaveAsPoster(Me.tMovie)
                        End If
                    End If
                End If
                Me.pnlSinglePic.Visible = False
            End If

            If Me.DLType = Master.ImageType.Fanart Then
                Dim iMod As Integer = 0
                Dim iVal As Integer = 1
                Dim extraPath As String = String.Empty
                Dim isChecked As Boolean = False

                For i As Integer = 0 To UBound(Me.chkImage)
                    If Me.chkImage(i).Checked Then
                        isChecked = True
                        Exit For
                    End If
                Next

                If isChecked Then

                    If isEdit Then
                        extraPath = Path.Combine(Master.TempPath, "extrathumbs")
                    Else
                        extraPath = Path.Combine(Directory.GetParent(Me.tMovie.Filename).FullName, "extrathumbs")
                        iMod = Master.GetExtraModifier(Me.tMovie.Filename)
                        iVal = iMod + 1
                    End If

                    If Not Directory.Exists(extraPath) Then
                        Directory.CreateDirectory(extraPath)
                    End If

                    For i As Integer = 0 To UBound(Me.chkImage)
                        If Me.chkImage(i).Checked Then
                            Dim fsET As New FileStream(Path.Combine(extraPath, String.Concat("thumb", iVal, ".jpg")), FileMode.Create, FileAccess.ReadWrite)
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
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        IMPA.Cancel()
        MPDB.Cancel()
        TMDB.Cancel()

        If bwIMPADownload.IsBusy Then bwIMPADownload.CancelAsync()
        If bwMPDBDownload.IsBusy Then bwMPDBDownload.CancelAsync()
        If bwTMDBDownload.IsBusy Then bwTMDBDownload.CancelAsync()

        Do While bwIMPADownload.IsBusy OrElse bwMPDBDownload.IsBusy OrElse bwTMDBDownload.IsBusy
            Application.DoEvents()
        Loop

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgImgSelect_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Me.tmpImage.Dispose()

        IMPA = Nothing
        MPDB = Nothing
        TMDB = Nothing

        IMPAPosters = Nothing
        MPDBPosters = Nothing
        TMDBPosters = Nothing
    End Sub

    Private Sub dlgImgSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not PreDL Then SetUp()
    End Sub

    Private Sub SetUp()
        Try
            AddHandler TMDB.PostersDownloaded, AddressOf TMDBPostersDownloaded
            AddHandler TMDB.ProgressUpdated, AddressOf TMDBProgressUpdated
            AddHandler IMPA.PostersDownloaded, AddressOf IMPAPostersDownloaded
            AddHandler IMPA.ProgressUpdated, AddressOf IMPAProgressUpdated
            AddHandler MPDB.PostersDownloaded, AddressOf MPDBPostersDownloaded
            AddHandler MPDB.ProgressUpdated, AddressOf MPDBProgressUpdated
            AddHandler IMPADone, AddressOf IMPADoneDownloading
            AddHandler TMDBDone, AddressOf TMDBDoneDownloading
            AddHandler MPDBDone, AddressOf MPDBDoneDownloading

            If Me.DLType = Master.ImageType.Posters Then
                Me.Text = String.Concat(Master.eLang.GetString(308, "Select Poster - "), Me.tMovie.ListTitle)
                Me.pnlDLStatus.Visible = True
            Else
                Me.Text = String.Concat(Master.eLang.GetString(309, "Select Fanart - "), Me.tMovie.ListTitle)
                Me.pnlDLStatus.Visible = False
                Me.pnlDLStatus.Height = 75
                Me.pnlDLStatus.Top = 207
            End If

            CachePath = String.Concat(Master.TempPath, Path.DirectorySeparatorChar, tMovie.Movie.IMDBID, Path.DirectorySeparatorChar, If(Me.DLType = Master.ImageType.Posters, "posters", "fanart"))

            Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
            Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
            Me.btnPreview.Text = Master.eLang.GetString(180, "Preview")
            Me.chkThumb.Text = Master.eLang.GetString(302, "Check All Thumb")
            Me.chkMid.Text = Master.eLang.GetString(303, "Check All Mid")
            Me.chkOriginal.Text = Master.eLang.GetString(304, "Check All Original")
            Me.lblInfo.Text = Master.eLang.GetString(305, "Selected item will be set as fanart. All checked items will be saved to \extrathumbs.")
            Me.lblDL3.Text = Master.eLang.GetString(306, "Performing Preliminary Tasks...")
            Me.lblDL2.Text = Me.lblDL3.Text
            Me.lblDL1.Text = Me.lblDL3.Text
            Me.Label2.Text = Master.eLang.GetString(307, "Downloading Selected Image...")
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub MPDBDoneDownloading()

        Try
            Me._mpdbDone = True
            Me.AllDoneDownloading()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub TMDBDoneDownloading()

        Try
            If Me.DLType = Master.ImageType.Posters Then
                Me._tmdbDone = True
                Me.AllDoneDownloading()
            Else
                Me.pnlDLStatus.Visible = False
                Me.ProcessPics(Me.TMDBPosters)
                Me.pnlBG.Visible = True
                Me.pnlFanart.Visible = True
                Me.lblInfo.Visible = True
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub IMPADoneDownloading()
        Try
            Me._impaDone = True
            Me.AllDoneDownloading()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub AllDoneDownloading()
        If Me._impaDone AndAlso Me._tmdbDone AndAlso Me._mpdbDone Then
            Me.pnlDLStatus.Visible = False
            Me.TMDBPosters.AddRange(Me.IMPAPosters)
            Me.TMDBPosters.AddRange(Me.MPDBPosters)
            Me.ProcessPics(Me.TMDBPosters)
            Me.pnlBG.Visible = True
        End If
    End Sub

    Private Sub ProcessPics(ByVal posters As List(Of Media.Image))
        Try
            Dim iIndex As Integer = 0

            'remove all entries with invalid images
            If Master.eSettings.UseImgCache Then
                For i As Integer = posters.Count - 1 To 0 Step -1
                    If IsNothing(posters.Item(i).WebImage) Then
                        posters.RemoveAt(i)
                    End If
                Next
            End If

            If posters.Count > 0 Then
                posters.Sort(AddressOf SortImages)

                For i As Integer = 0 To posters.Count - 1
                    If Not IsNothing(posters.Item(i).WebImage) AndAlso (Me.DLType = Master.ImageType.Fanart OrElse Not (posters.Item(i).URL.ToLower.Contains("themoviedb.org") AndAlso Not posters.Item(i).Description = "cover")) Then
                        Me.AddImage(posters.Item(i).WebImage, posters.Item(i).Description, iIndex, posters.Item(i).URL)
                        iIndex += 1
                    End If
                Next
            Else
                If Me.DLType = Master.ImageType.Fanart Then
                    MsgBox(Master.eLang.GetString(310, "No Fanart found for this movie."), MsgBoxStyle.Information, Master.eLang.GetString(311, "No Fanart Found"))
                Else
                    MsgBox(Master.eLang.GetString(312, "No Posters found for this movie."), MsgBoxStyle.Information, Master.eLang.GetString(313, "No Posters Found"))
                End If
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
                Me.Close()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub MPDBProgressUpdated(ByVal iPercent As Integer)
        Me.pbDL3.Value = iPercent
    End Sub

    Private Sub MPDBPostersDownloaded(ByVal Posters As List(Of Media.Image))

        Try
            Me.pbDL3.Value = 0

            Me.lblDL3.Text = Master.eLang.GetString(314, "Preparing images...")
            Me.lblDL3Status.Text = String.Empty
            Me.pbDL3.Maximum = Posters.Count

            Me.MPDBPosters = Posters

            Me.bwMPDBDownload.WorkerSupportsCancellation = True
            Me.bwMPDBDownload.WorkerReportsProgress = True
            Me.bwMPDBDownload.RunWorkerAsync()
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

            Me.lblDL1.Text = Master.eLang.GetString(314, "Preparing images...")
            Me.lblDL1Status.Text = String.Empty
            Me.pbDL1.Maximum = Posters.Count

            Me.TMDBPosters = Posters

            Me.bwTMDBDownload.WorkerSupportsCancellation = True
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

            Me.lblDL2.Text = Master.eLang.GetString(314, "Preparing images...")
            Me.lblDL2Status.Text = String.Empty
            Me.pbDL2.Maximum = Posters.Count

            Me.IMPAPosters = Posters

            Me.bwIMPADownload.WorkerSupportsCancellation = True
            Me.bwIMPADownload.WorkerReportsProgress = True
            Me.bwIMPADownload.RunWorkerAsync()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub GetPosters()

        Try
            Dim NoneFound As Boolean = True

            If Master.eSettings.UseImgCache Then
                Dim lFi As New List(Of FileInfo)
                Dim di As New DirectoryInfo(CachePath)

                If Not Directory.Exists(CachePath) Then
                    Directory.CreateDirectory(CachePath)
                Else
                    Try
                        lFi.AddRange(di.GetFiles("*.jpg"))
                    Catch
                    End Try
                End If


                If lFi.Count > 0 Then
                    NoneFound = False
                    Me.pnlDLStatus.Visible = False
                    Dim tImage As Media.Image
                    Dim tmpImage As New Images
                    For Each sFile As FileInfo In lFi
                        tImage = New Media.Image
                        tmpImage.FromFile(sFile.FullName)
                        tImage.WebImage = New Bitmap(tmpImage.Image)
                        Select Case True
                            Case sFile.Name.Contains("(original)")
                                tImage.Description = "original"
                            Case sFile.Name.Contains("(mid)")
                                tImage.Description = "mid"
                            Case sFile.Name.Contains("(cover)")
                                tImage.Description = "cover"
                            Case sFile.Name.Contains("(thumb)")
                                tImage.Description = "thumb"
                            Case sFile.Name.Contains("(poster)")
                                tImage.Description = "poster"
                        End Select
                        tImage.URL = Regex.Match(sFile.Name, "\(url=(.*?)\)").Groups(1).ToString
                        Me.TMDBPosters.Add(tImage)
                        tmpImage.Clear()
                    Next
                    tmpImage.Dispose()
                    tmpImage = Nothing
                    ProcessPics(TMDBPosters)
                    Me.pnlBG.Visible = True
                End If

                lFi = Nothing
                di = Nothing
            End If

            If NoneFound Then
                If Master.eSettings.UseTMDB Then
                    Me.lblDL1.Text = Master.eLang.GetString(315, "Retrieving data from TheMovieDB.com...")
                    Me.lblDL1Status.Text = String.Empty
                    Me.pbDL1.Maximum = 3
                    Me.pnlDLStatus.Visible = True
                    Me.Refresh()

                    Me._tmdbDone = False

                    Me.TMDB.GetImagesAsync(tMovie.Movie.IMDBID, "poster")
                Else
                    Me.lblDL1.Text = Master.eLang.GetString(316, "TheMovieDB.com is not enabled")
                End If

                If Master.eSettings.UseIMPA Then
                    Me.lblDL2.Text = Master.eLang.GetString(317, "Retrieving data from IMPAwards.com...")
                    Me.lblDL2Status.Text = String.Empty
                    Me.pbDL2.Maximum = 3
                    Me.pnlDLStatus.Visible = True
                    Me.Refresh()

                    Me._impaDone = False

                    Me.IMPA.GetImagesAsync(tMovie.Movie.IMDBID)
                Else
                    Me.lblDL2.Text = Master.eLang.GetString(318, "IMPAwards.com is not enabled")
                End If

                If Master.eSettings.UseMPDB Then
                    Me.lblDL3.Text = Master.eLang.GetString(319, "Retrieving data from MoviePosterDB.com...")
                    Me.lblDL3Status.Text = String.Empty
                    Me.pbDL3.Maximum = 3
                    Me.pnlDLStatus.Visible = True
                    Me.Refresh()

                    Me._mpdbDone = False

                    Me.MPDB.GetImagesAsync(tMovie.Movie.IMDBID)
                Else
                    Me.lblDL3.Text = Master.eLang.GetString(320, "MoviePostersDB.com is not enabled")
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub GetFanart()
        Try
            Dim NoneFound As Boolean = True

            If Master.eSettings.UseImgCache Then
                Dim di As New DirectoryInfo(CachePath)
                Dim lFi As New List(Of FileInfo)

                If Not Directory.Exists(CachePath) Then
                    Directory.CreateDirectory(CachePath)
                Else
                    Try
                        lFi.AddRange(di.GetFiles("*.jpg"))
                    Catch
                    End Try
                End If

                If lFi.Count > 0 Then
                    NoneFound = False
                    Me.pnlDLStatus.Visible = False
                    Dim tImage As Media.Image
                    Dim tmpImage As New Images
                    For Each sFile As FileInfo In lFi
                        tImage = New Media.Image
                        tmpImage.FromFile(sFile.FullName)
                        If Not IsNothing(tmpImage.Image) Then
                            tImage.WebImage = New Bitmap(tmpImage.Image)
                            Select Case True
                                Case sFile.Name.Contains("(original)")
                                    tImage.Description = "original"
                                Case sFile.Name.Contains("(mid)")
                                    tImage.Description = "mid"
                                Case sFile.Name.Contains("(thumb)")
                                    tImage.Description = "thumb"
                            End Select
                            tImage.URL = Regex.Match(sFile.Name, "\(url=(.*?)\)").Groups(1).ToString
                            Me.TMDBPosters.Add(tImage)
                        End If
                        tmpImage.Clear()
                    Next
                    tmpImage.Dispose()
                    tmpImage = Nothing
                    ProcessPics(TMDBPosters)
                    Me.pnlBG.Visible = True
                    Me.pnlFanart.Visible = True
                    Me.lblInfo.Visible = True
                End If

                lFi = Nothing
                di = Nothing
            End If

            If NoneFound Then
                If Master.eSettings.UseTMDB Then
                    Me.lblDL1.Text = Master.eLang.GetString(315, "Retrieving data from TheMovieDB.com...")
                    Me.lblDL1Status.Text = String.Empty
                    Me.pbDL1.Maximum = 3
                    Me.pnlDLStatus.Visible = True
                    Me.Refresh()

                    Me.TMDB.GetImagesAsync(tMovie.Movie.IMDBID, "backdrop")
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Function SortImages(ByVal x As Media.Image, ByVal y As Media.Image) As Integer
        If String.IsNullOrEmpty(x.URL) Then
            Return -1
        End If
        If String.IsNullOrEmpty(y.URL) Then
            Return 1
        End If

        Return x.URL.CompareTo(y.URL)
    End Function

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
            AddHandler pbImage(iIndex).DoubleClick, AddressOf pbImage_DoubleClick
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
                    Me.lblImage(iIndex).Text = Master.eLang.GetString(321, "Multiple")
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

            Me.pnlSize.Visible = False

            If Not Me.DLType = Master.ImageType.Fanart AndAlso sURL.ToLower.Contains("themoviedb.org") Then
                Me.SetupSizes(sURL)
                If Not rbLarge.Checked AndAlso Not rbMedium.Checked AndAlso Not rbSmall.Checked AndAlso Not rbXLarge.Checked Then
                    Me.OK_Button.Enabled = False
                End If
                Me.tmpImage.Image = Nothing
            Else
                Me.rbXLarge.Checked = False
                Me.rbLarge.Checked = False
                Me.rbMedium.Checked = False
                Me.rbSmall.Checked = False
                Me.OK_Button.Enabled = True
                Me.tmpImage.Image = Me.pbImage(iIndex).Image
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetupSizes(ByVal sURL As String)

        Try
            Dim sLeft As String = Strings.Left(sURL, sURL.Length - 10)

            Me.rbXLarge.Checked = False
            Me.rbXLarge.Enabled = False
            Me.rbXLarge.Text = Master.eLang.GetString(322, "X-Large")
            Me.rbLarge.Checked = False
            Me.rbLarge.Enabled = False
            Me.rbLarge.Text = Master.eLang.GetString(323, "Large")
            Me.rbMedium.Checked = False
            Me.rbMedium.Text = Master.eLang.GetString(324, "Medium")
            Me.rbSmall.Checked = False
            Me.rbSmall.Enabled = False
            Me.rbSmall.Text = Master.eLang.GetString(325, "Small")

            Me.rbMedium.Tag = sURL

            For i As Integer = 0 To Me.TMDBPosters.Count - 1
                Select Case True
                    Case Me.TMDBPosters.Item(i).URL = String.Concat(sLeft, ".jpg")
                        ' xlarge
                        If Not Master.eSettings.UseImgCache OrElse Not IsNothing(TMDBPosters.Item(i).WebImage) Then
                            Me.rbXLarge.Enabled = True
                            Me.rbXLarge.Tag = Me.TMDBPosters.Item(i).URL
                            If Master.eSettings.UseImgCache Then Me.rbXLarge.Text = String.Format(Master.eLang.GetString(326, "X-Large ({0}x{1})"), Me.TMDBPosters.Item(i).WebImage.Width, Me.TMDBPosters.Item(i).WebImage.Height)
                        End If
                    Case Me.TMDBPosters.Item(i).URL = String.Concat(sLeft, "_mid.jpg")
                        ' large 
                        If Not Master.eSettings.UseImgCache OrElse Not IsNothing(TMDBPosters.Item(i).WebImage) Then
                            Me.rbLarge.Enabled = True
                            Me.rbLarge.Tag = Me.TMDBPosters.Item(i).URL
                            If Master.eSettings.UseImgCache Then Me.rbLarge.Text = String.Format(Master.eLang.GetString(327, "Large ({0}x{1})"), Me.TMDBPosters.Item(i).WebImage.Width, Me.TMDBPosters.Item(i).WebImage.Height)
                        End If
                    Case Me.TMDBPosters.Item(i).URL = String.Concat(sLeft, "_thumb.jpg")
                        ' small
                        If Not Master.eSettings.UseImgCache OrElse Not IsNothing(TMDBPosters.Item(i).WebImage) Then
                            Me.rbSmall.Enabled = True
                            Me.rbSmall.Tag = Me.TMDBPosters.Item(i).URL
                            If Master.eSettings.UseImgCache Then Me.rbSmall.Text = String.Format(Master.eLang.GetString(328, "Small ({0}x{1})"), Me.TMDBPosters.Item(i).WebImage.Width, Me.TMDBPosters.Item(i).WebImage.Height)
                        End If
                    Case Me.TMDBPosters.Item(i).URL = sURL
                        If Master.eSettings.UseImgCache Then Me.rbMedium.Text = String.Format(Master.eLang.GetString(329, "Medium ({0}x{1})"), Me.TMDBPosters.Item(i).WebImage.Width, Me.TMDBPosters.Item(i).WebImage.Height)
                End Select
            Next

            Select Case Master.eSettings.PreferredPosterSize
                Case Master.PosterSize.Small
                    Me.rbSmall.Checked = Me.rbSmall.Enabled
                Case Master.PosterSize.Mid
                    Me.rbMedium.Checked = Me.rbMedium.Enabled
                Case Master.PosterSize.Lrg
                    Me.rbLarge.Checked = Me.rbLarge.Enabled
                Case Master.PosterSize.Xlrg
                    Me.rbXLarge.Checked = Me.rbXLarge.Enabled
            End Select

            pnlSize.Visible = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbImage_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            If Me.DLType = Master.ImageType.Fanart OrElse Not sender.tag.contains("themoviedb.org") Then
                Using dImgView As New dlgImgView
                    dImgView.ShowDialog(sender.image)
                End Using
            End If
        Catch
        End Try
    End Sub

    Private Sub pbImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DoSelect(Convert.ToInt32(sender.Name), sender.Tag)
    End Sub

    Private Sub pnlImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DoSelect(Convert.ToInt32(sender.Name), sender.Tag)
    End Sub

    Private Sub lblImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DoSelect(Convert.ToInt32(sender.Name), sender.Tag)
    End Sub

    Private Sub dlgImgSelect_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Try

            Me.Activate()
            If Not PreDL Then StartDownload()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub StartDownload()
        Try
            Select Case Me.DLType
                Case Master.ImageType.Posters
                    Me.GetPosters()
                Case Master.ImageType.Fanart
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
                If bwIMPADownload.CancellationPending Then
                    e.Cancel = True
                    Return
                End If
                Me.bwIMPADownload.ReportProgress(i + 1, Me.IMPAPosters.Item(i).URL)
                Dim wrRequest As WebRequest = WebRequest.Create(Me.IMPAPosters.Item(i).URL)
                wrRequest.Timeout = 10000
                Using wrResponse As WebResponse = wrRequest.GetResponse()
                    If wrResponse.ContentType.Contains("image") Then
                        Me.IMPAPosters.Item(i).WebImage = Image.FromStream(wrResponse.GetResponseStream)
                        If Master.eSettings.UseImgCache Then
                            Me.IMPAPosters.Item(i).URL = StringManip.CleanURL(Me.IMPAPosters.Item(i).URL)
                            Using fsImage As New FileStream(Path.Combine(CachePath, String.Concat("poster_(", Me.IMPAPosters.Item(i).Description, ")_(url=", Me.IMPAPosters.Item(i).URL, ").jpg")), FileMode.Create, FileAccess.Write)
                                Me.IMPAPosters.Item(i).WebImage.Save(fsImage, Imaging.ImageFormat.Jpeg)
                            End Using
                        End If
                    End If
                End Using
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
            Me.lblDL2Status.Text = String.Format(Master.eLang.GetString(330, "Downloading {0}"), If(sStatus.Length > 40, StringManip.TruncateURL(sStatus, 40), sStatus))
            Me.pbDL2.Value = e.ProgressPercentage
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwIMPADownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwIMPADownload.RunWorkerCompleted

        '//
        ' Thread finished: process the pics
        '\\

        If Not e.Cancelled Then
            Me._impaDone = True
            RaiseEvent IMPADone()
        End If

    End Sub

    Private Sub bwTMDBDownload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwTMDBDownload.DoWork

        '//
        ' Thread to download tmdb posters from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\

        For i As Integer = 0 To Me.TMDBPosters.Count - 1
            Try
                If Me.DLType = Master.ImageType.Fanart OrElse (Master.eSettings.UseImgCache OrElse Me.TMDBPosters.Item(i).Description = "cover") Then
                    If Me.bwTMDBDownload.CancellationPending Then
                        e.Cancel = True
                        Return
                    End If
                    Me.bwTMDBDownload.ReportProgress(i + 1, Me.TMDBPosters.Item(i).URL)
                    Dim wrRequest As WebRequest = WebRequest.Create(Me.TMDBPosters.Item(i).URL)
                    wrRequest.Timeout = 10000
                    Using wrResponse As WebResponse = wrRequest.GetResponse()
                        If wrResponse.ContentType.Contains("image") Then
                            Me.TMDBPosters.Item(i).WebImage = Image.FromStream(wrResponse.GetResponseStream)
                            If Master.eSettings.UseImgCache Then
                                Me.TMDBPosters.Item(i).URL = StringManip.CleanURL(Me.TMDBPosters.Item(i).URL)
                                Using fsImage As New FileStream(Path.Combine(CachePath, String.Concat(If(Me.DLType = Master.ImageType.Fanart, "fanart_(", "poster_("), Me.TMDBPosters.Item(i).Description, ")_(url=", Me.TMDBPosters.Item(i).URL, ").jpg")), FileMode.Create, FileAccess.Write)
                                    Me.TMDBPosters.Item(i).WebImage.Save(fsImage, Imaging.ImageFormat.Jpeg)
                                End Using
                            End If
                        End If
                    End Using
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
            Me.lblDL1Status.Text = String.Format(Master.eLang.GetString(330, "Downloading {0}"), If(sStatus.Length > 40, StringManip.TruncateURL(sStatus, 40), sStatus))
            Me.pbDL1.Value = e.ProgressPercentage
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwTMDBDownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwTMDBDownload.RunWorkerCompleted

        '//
        ' Thread finished: process the pics
        '\\

        If Not e.Cancelled Then
            Me._tmdbDone = True
            RaiseEvent TMDBDone()
        End If

    End Sub

    Private Sub bwMPDBDownload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwMPDBDownload.DoWork

        '//
        ' Thread to download mpdb posters from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\
        For i As Integer = 0 To Me.MPDBPosters.Count - 1
            Try
                If Me.bwMPDBDownload.CancellationPending Then
                    e.Cancel = True
                    Return
                End If
                Me.bwMPDBDownload.ReportProgress(i + 1, Me.MPDBPosters.Item(i).URL)
                Dim wrRequest As WebRequest = WebRequest.Create(Me.MPDBPosters.Item(i).URL)
                wrRequest.Timeout = 10000
                Using wrResponse As WebResponse = wrRequest.GetResponse()
                    If wrResponse.ContentType.Contains("image") Then
                        Me.MPDBPosters.Item(i).WebImage = Image.FromStream(wrResponse.GetResponseStream)
                        If Master.eSettings.UseImgCache Then
                            Me.MPDBPosters.Item(i).URL = StringManip.CleanURL(Me.MPDBPosters.Item(i).URL)
                            Using fsImage As New FileStream(Path.Combine(CachePath, String.Concat("poster_(", Me.MPDBPosters.Item(i).Description, ")_(url=", Me.MPDBPosters.Item(i).URL, ").jpg")), FileMode.Create, FileAccess.Write)
                                Me.MPDBPosters.Item(i).WebImage.Save(fsImage, Imaging.ImageFormat.Jpeg)
                            End Using
                        End If
                    End If
                End Using
                wrRequest = Nothing
            Catch
            End Try
        Next

    End Sub

    Private Sub bwMPDBDownload_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwMPDBDownload.ProgressChanged

        '//
        ' Update the status bar with the name of the current media name and increase progress bar
        '\\
        Try
            Dim sStatus As String = e.UserState.ToString
            Me.lblDL3Status.Text = String.Format(Master.eLang.GetString(330, "Downloading {0}"), If(sStatus.Length > 40, StringManip.TruncateURL(sStatus, 40), sStatus))
            Me.pbDL3.Value = e.ProgressPercentage
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwMPDBDownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwMPDBDownload.RunWorkerCompleted

        '//
        ' Thread finished: process the pics
        '\\

        If Not e.Cancelled Then
            Me._mpdbDone = True
            RaiseEvent MPDBDone()
        End If

    End Sub

    Private Sub rbXLarge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbXLarge.CheckedChanged
        Me.OK_Button.Enabled = True
        Me.btnPreview.Enabled = True
    End Sub

    Private Sub rbLarge_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLarge.CheckedChanged
        Me.OK_Button.Enabled = True
        Me.btnPreview.Enabled = True
    End Sub

    Private Sub rbMedium_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbMedium.CheckedChanged
        Me.OK_Button.Enabled = True
        Me.btnPreview.Enabled = True
    End Sub

    Private Sub rbSmall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSmall.CheckedChanged
        Me.OK_Button.Enabled = True
        Me.btnPreview.Enabled = True
    End Sub

    Public Overloads Function ShowDialog(ByVal mMovie As Master.DBMovie, ByVal _DLType As Master.ImageType, Optional ByVal _isEdit As Boolean = False, Optional ByVal _PreDL As Boolean = False) As String

        '//
        ' Overload to pass data
        '\\

        Me.tMovie = mMovie
        Me.DLType = _DLType
        Me.isEdit = _isEdit
        Me.PreDL = _PreDL

        If Not _PreDL Then
            MyBase.ShowDialog()
            Return OutPath
        Else
            Me.SetUp()
            Me.StartDownload()
            Return Nothing
        End If

    End Function

    Public Overloads Function ShowDialog() As String

            MyBase.ShowDialog()
            Return OutPath

    End Function

    Private Sub chkOriginal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOriginal.CheckedChanged
        Me.CheckAll("(original)", chkOriginal.Checked)
    End Sub

    Private Sub chkMid_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMid.CheckedChanged
        Me.CheckAll("(mid)", chkMid.Checked)
    End Sub

    Private Sub chkThumb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkThumb.CheckedChanged
        Me.CheckAll("(thumb)", chkThumb.Checked)
    End Sub

    Private Sub CheckAll(ByVal sType As String, ByVal Checked As Boolean)

        For i As Integer = 0 To UBound(Me.chkImage)
            If Me.chkImage(i).Text.ToLower.Contains(sType) Then
                Me.chkImage(i).Checked = Checked
            End If
        Next

    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click

        Try
            Dim tImage As New Images

            Me.pnlSinglePic.Visible = True
            Application.DoEvents()

            Select Case True
                Case rbXLarge.Checked
                    If Master.eSettings.UseImgCache Then
                        tImage.FromFile(Path.Combine(CachePath, String.Concat("poster_(original)_(url=", Me.rbXLarge.Tag, ").jpg")))
                    Else
                        tImage.FromWeb(Me.rbXLarge.Tag)
                    End If
                Case rbLarge.Checked
                    If Master.eSettings.UseImgCache Then
                        tImage.FromFile(Path.Combine(CachePath, String.Concat("poster_(mid)_(url=", Me.rbLarge.Tag, ").jpg")))
                    Else
                        tImage.FromWeb(Me.rbLarge.Tag)
                    End If
                Case rbMedium.Checked
                    If Master.eSettings.UseImgCache Then
                        tImage.FromFile(Path.Combine(CachePath, String.Concat("poster_(cover)_(url=", Me.rbMedium.Tag, ").jpg")))
                    Else
                        tImage.FromWeb(Me.rbMedium.Tag)
                    End If
                Case rbSmall.Checked
                    If Master.eSettings.UseImgCache Then
                        tImage.FromFile(Path.Combine(CachePath, String.Concat("poster_(thumb)_(url=", Me.rbSmall.Tag, ").jpg")))
                    Else
                        tImage.FromWeb(Me.rbSmall.Tag)
                    End If
            End Select

            Me.pnlSinglePic.Visible = False

            If Not IsNothing(tImage.Image) Then

                Using dImgView As New dlgImgView
                    dImgView.ShowDialog(tImage.Image)
                End Using

            End If

            tImage.Dispose()
            tImage = Nothing

        Catch ex As Exception
            Me.pnlSinglePic.Visible = False
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
End Class


