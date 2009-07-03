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

Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Text.RegularExpressions


Public Class frmMain

#Region "Declarations"

    ' ########################################
    ' ############# DECLARATIONS #############
    ' ########################################
    Friend WithEvents bwFolderData As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwMediaInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwLoadInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwDownloadPic As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwPrelim As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwScraper As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwRefreshMovies As New System.ComponentModel.BackgroundWorker

    Friend WithEvents bsMedia As New BindingSource

    Public alActors As New ArrayList

    Private aniType As Integer = 0 '0 = down, 1 = mid, 2 = up
    Private aniRaise As Boolean = False
    Private aniFilterRaise As Boolean = False
    Private MainPoster As New Images
    Private MainFanart As New Images
    Private pbGenre() As PictureBox = Nothing
    Private pnlGenre() As Panel = Nothing
    Private tmpTitle As String = String.Empty
    Private ReportDownloadPercent As Boolean = False
    Private IMDB As New IMDB.Scraper
    Private dtMedia As New DataTable
    Private currRow As Integer = -1
    Private prevRow As Integer = -1
    Private currText As String = String.Empty
    Private prevText As String = String.Empty
    Private FilterArray As New ArrayList
    Private ScraperDone As Boolean = False
    Private LoadingDone As Boolean = False
    Private isCL As Boolean = False
    Private GenreImage As Image

    Private Enum PicType As Integer
        Actor = 0
        Poster = 1
        Fanart = 2
    End Enum

    Private Structure Results
        Dim scrapeType As Master.ScrapeType
        Dim scrapeMod As Master.ScrapeModifier
        Dim Options As Master.ScrapeOptions
        Dim fileInfo As String
        Dim setEnabled As Boolean
        Dim ResultType As PicType
        Dim Movie As Master.DBMovie
        Dim Path As String
        Dim Result As Image
    End Structure

    Private Structure Arguments
        Dim setEnabled As Boolean
        Dim scrapeType As Master.ScrapeType
        Dim scrapeMod As Master.ScrapeModifier
        Dim Options As Master.ScrapeOptions
        Dim pType As PicType
        Dim pURL As String
        Dim Path As String
        Dim Movie As Master.DBMovie
        Dim ID As Integer
    End Structure

#End Region '*** Declarations



#Region "Form/Controls"

    ' ########################################
    ' ######### FORM/CONTROLS EVENTS #########
    ' ########################################

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '//
        ' Do some stuff before closing
        '\\

        Try

            Dim doSave As Boolean = True

            Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision)
            Master.eSettings.WindowLoc = Me.Location
            Master.eSettings.WindowSize = Me.Size
            Master.eSettings.WindowState = Me.WindowState
            Master.eSettings.InfoPanelState = Me.aniType
            Master.eSettings.FilterPanelState = Me.aniFilterRaise
            Master.eSettings.SpliterPanelState = Me.scMain.SplitterDistance
            Master.eSettings.Save()

            If Me.bwPrelim.IsBusy OrElse Me.bwFolderData.IsBusy OrElse isCL Then
                doSave = False
            End If

            If Me.bwFolderData.IsBusy Then Me.bwFolderData.CancelAsync()
            If Me.bwMediaInfo.IsBusy Then Me.bwMediaInfo.CancelAsync()
            If Me.bwLoadInfo.IsBusy Then Me.bwLoadInfo.CancelAsync()
            If Me.bwDownloadPic.IsBusy Then Me.bwDownloadPic.CancelAsync()
            If Me.bwPrelim.IsBusy Then Me.bwPrelim.CancelAsync()
            If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()
            If Me.bwRefreshMovies.IsBusy Then Me.bwRefreshMovies.CancelAsync()

            lblCanceling.Text = "Canceling All Processes..."
            btnCancel.Visible = False
            lblCanceling.Visible = True
            pbCanceling.Visible = True
            pnlCancel.Visible = True
            Me.Refresh()

            Do While Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy
                Application.DoEvents()
            Loop

            If doSave Then Me.SaveMovieList()

            If Not isCL Then Master.DB.Close()

            If Not Master.eSettings.PersistImgCache Then
                If Directory.Exists(Master.TempPath) Then
                    Directory.Delete(Master.TempPath, True)
                End If
            End If
        Catch
            'force close
            Application.Exit()
        End Try

    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            If Me.dgvMediaList.Columns.Count > 0 Then
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()
                Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, (Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2)
                Me.pnlCancel.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, 100)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click

        '//
        ' Show the settings
        '\\

        Try
            Using dSettings As New dlgSettings
                Dim dResult As Windows.Forms.DialogResult = dSettings.ShowDialog
                If dResult = Windows.Forms.DialogResult.OK OrElse dResult = Windows.Forms.DialogResult.Retry Then

                    Me.SetColors()

                    If Me.dgvMediaList.RowCount > 0 Then
                        Me.dgvMediaList.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                        Me.dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieFanartCol
                        Me.dgvMediaList.Columns(6).Visible = Not Master.eSettings.MovieInfoCol
                        Me.dgvMediaList.Columns(7).Visible = Not Master.eSettings.MovieTrailerCol
                        Me.dgvMediaList.Columns(8).Visible = Not Master.eSettings.MovieSubCol
                        Me.dgvMediaList.Columns(9).Visible = Not Master.eSettings.MovieExtraCol

                        'Trick to autosize the first column, but still allow resizing by user
                        Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    End If

                    'might as well wait for these
                    Do While Me.bwMediaInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy
                        Application.DoEvents()
                    Loop

                    If dResult = Windows.Forms.DialogResult.Retry Then
                        If Not Me.bwFolderData.IsBusy AndAlso Not Me.bwPrelim.IsBusy Then
                            Do While Me.bwLoadInfo.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy
                                Application.DoEvents()
                            Loop
                            Me.LoadMedia(1)
                        End If
                    Else
                        If Not Me.bwFolderData.IsBusy AndAlso Not Me.bwPrelim.IsBusy AndAlso Not Me.bwLoadInfo.IsBusy AndAlso Not Me.bwScraper.IsBusy AndAlso Not Me.bwRefreshMovies.IsBusy Then
                            Me.FillList(0)
                        End If
                    End If

                    Me.SetMenus(True)
                Else
                    Me.SetMenus(False)
                End If
            End Using

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '//
        ' Add our handlers, load settings, set form colors, and try to load movies at startup
        '\\

        Me.Visible = False
        Dim Args() As String = Environment.GetCommandLineArgs
        ' Check if is allready running
        If Args.Count = 1 Then
            If CheckInstanceOfApp() Then
                Application.Exit()
                Return
            End If
        End If
        'setup some dummies so we don't get exceptions when resizing form/info panel
        ReDim Preserve Me.pnlGenre(0)
        ReDim Preserve Me.pbGenre(0)
        Me.pnlGenre(0) = New Panel()
        Me.pbGenre(0) = New PictureBox()

        AddHandler IMDB.MovieInfoDownloaded, AddressOf MovieInfoDownloaded
        AddHandler IMDB.ProgressUpdated, AddressOf MovieInfoDownloadedPercent

        Dim sPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Log", Path.DirectorySeparatorChar, "errlog.txt")
        If File.Exists(sPath) Then
            If File.Exists(sPath.Insert(sPath.LastIndexOf("."), "-old")) Then File.Delete(sPath.Insert(sPath.LastIndexOf("."), "-old"))
            Master.MoveFileWithStream(sPath, sPath.Insert(sPath.LastIndexOf("."), "-old"))
            File.Delete(sPath)
        End If

        If Not Directory.Exists(Master.TempPath) Then Directory.CreateDirectory(Master.TempPath)

        Master.eSettings.Load()
        Master.CreateDefaultOptions()

        Dim MoviePath As String = String.Empty
        Dim isSingle As Boolean = False
        Dim hasSpec As Boolean = False
        Dim clScrapeType As Master.ScrapeType = Nothing
        Dim clScrapeMod As Master.ScrapeModifier = Nothing

        If Args.Count > 1 Then
            isCL = True
            Dim clAsk As Boolean = False

            If Args.Count = 3 OrElse Args.Count = 4 Then
                Select Case Args(1).ToLower
                    Case "-fullask"
                        clScrapeType = Master.ScrapeType.FullAsk
                        clAsk = True
                    Case "-fullauto"
                        clScrapeType = Master.ScrapeType.FullAuto
                        clAsk = False
                    Case "-missask"
                        clScrapeType = Master.ScrapeType.UpdateAsk
                        clAsk = True
                    Case "-missauto"
                        clScrapeType = Master.ScrapeType.UpdateAuto
                        clAsk = False
                    Case "-newask"
                        clScrapeType = Master.ScrapeType.NewAsk
                        clAsk = True
                    Case "-newauto"
                        clScrapeType = Master.ScrapeType.NewAuto
                        clAsk = False
                    Case "-markask"
                        clScrapeType = Master.ScrapeType.MarkAsk
                        clAsk = True
                    Case "-markauto"
                        clScrapeType = Master.ScrapeType.MarkAuto
                        clAsk = False
                    Case "-file"
                        isSingle = False
                        hasSpec = True
                        clScrapeType = Master.ScrapeType.SingleScrape
                    Case "-folder"
                        isSingle = True
                        hasSpec = True
                        clScrapeType = Master.ScrapeType.SingleScrape
                End Select

                Select Case Args(2).ToLower
                    Case "-all"
                        clScrapeMod = Master.ScrapeModifier.All
                    Case "-nfo"
                        clScrapeMod = Master.ScrapeModifier.NFO
                    Case "-posters"
                        clScrapeMod = Master.ScrapeModifier.Poster
                    Case "-fanart"
                        clScrapeMod = Master.ScrapeModifier.Fanart
                    Case "-extra"
                        clScrapeMod = Master.ScrapeModifier.Extra
                    Case Else
                        If File.Exists(Args(2).Replace("""", String.Empty)) Then
                            MoviePath = Args(2).Replace("""", String.Empty)
                        End If
                End Select
            End If

            If Args.Count = 4 Then
                If Args(3) = "-verbose" Then
                    clAsk = True
                End If
            End If

            If Not IsNothing(clScrapeType) Then
                If Not IsNothing(clScrapeMod) AndAlso Not clScrapeType = Master.ScrapeType.SingleScrape Then
                    Try
                        Master.DB.Connect(False, False)
                        LoadMedia(1)
                        Do While Not Me.LoadingDone
                            Application.DoEvents()
                        Loop
                        ScrapeData(clScrapeType, clScrapeMod, Master.DefaultOptions, clAsk)
                    Catch ex As Exception
                        Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                    End Try
                Else
                    Try

                        If Not String.IsNullOrEmpty(MoviePath) AndAlso hasSpec Then
                            Master.currMovie = Master.DB.LoadMovieFromDB(MoviePath)
                            Me.tmpTitle = Master.FilterName(If(isSingle, Directory.GetParent(MoviePath).Name, Path.GetFileNameWithoutExtension(MoviePath)))
                            Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, Nothing, clAsk)
                        Else
                            Me.ScraperDone = True
                        End If
                    Catch ex As Exception
                        Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                    End Try
                End If

                Do While Not Me.ScraperDone
                    Application.DoEvents()
                Loop
            End If


            Me.Close()

        Else
            Try

                If Master.eSettings.CheckUpdates Then
                    Dim tmpNew As Integer = Master.CheckUpdate
                    If tmpNew > Convert.ToInt32(My.Application.Info.Version.Revision) Then
                        Using dNewVer As New dlgNewVersion
                            dNewVer.ShowDialog(tmpNew)
                        End Using
                    End If
                End If

                Me.Location = Master.eSettings.WindowLoc
                Me.Size = Master.eSettings.WindowSize
                Me.WindowState = Master.eSettings.WindowState

                Master.CacheXMLs()
                Me.SetColors()
                Me.SetToolTips()

                Me.aniType = Master.eSettings.InfoPanelState
                Select Case Me.aniType
                    Case 0
                        Me.pnlInfoPanel.Height = 25
                        Me.btnDown.Enabled = False
                        Me.btnMid.Enabled = True
                        Me.btnUp.Enabled = True
                    Case 1
                        Me.pnlInfoPanel.Height = 280
                        Me.btnMid.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnUp.Enabled = True
                    Case 2
                        Me.pnlInfoPanel.Height = 500
                        Me.btnUp.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnMid.Enabled = True
                End Select

                Me.aniFilterRaise = Master.eSettings.FilterPanelState
                If Me.aniFilterRaise Then
                    Me.pnlFilter.Height = 85
                    Me.btnFilterDown.Enabled = True
                    Me.btnFilterUp.Enabled = False
                Else
                    Me.pnlFilter.Height = 25
                    Me.btnFilterDown.Enabled = False
                    Me.btnFilterUp.Enabled = True
                End If

                Me.scMain.SplitterDistance = Master.eSettings.SpliterPanelState

                Me.ClearInfo()

                If Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision) Then
                    Master.DB.Connect(False, False)
                    Me.FillList(0)
                    Me.Visible = True
                Else
                    '!!CHANGE!! - Change to (True, False) for next release
                    Master.DB.Connect(True, True)
                    If dlgWizard.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Me.Visible = True
                        Me.LoadMedia(1)
                    Else
                        Me.FillList(0)
                        Me.Visible = True
                    End If
                End If

                Me.SetMenus(True)

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Me.Activate()

        End If

    End Sub
    Private Function CheckInstanceOfApp() As Boolean
        Dim appProc() As Process
        Dim strModName, strProcName As String
        strModName = Process.GetCurrentProcess.MainModule.ModuleName
        strProcName = System.IO.Path.GetFileNameWithoutExtension(strModName)
        appProc = Process.GetProcessesByName(strProcName)
        If appProc.Length > 1 Then
            CType(My.Application.SplashScreen, frmSplash).CloseForm()
            Dim f = New Form()
            f.TopMost = True
            MessageBox.Show(f, "There is an instance of this Ember Media Manager running.")
            f.Close()
            f = Nothing
            Return True
        End If
        Return False
    End Function
    Private Sub lstActors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstActors.SelectedIndexChanged
        '//
        ' Begin thread to download actor image if one exists
        '\\
        Try
            If Not Me.alActors.Item(Me.lstActors.SelectedIndex) = "none" Then

                If Not IsNothing(Me.pbActors.Image) Then
                    Me.pbActors.Image.Dispose()
                    Me.pbActors.Image = Nothing
                End If

                Me.pbActLoad.Visible = True

                If Me.bwDownloadPic.IsBusy Then
                    Me.bwDownloadPic.CancelAsync()
                End If


                Me.bwDownloadPic = New System.ComponentModel.BackgroundWorker
                Me.bwDownloadPic.WorkerSupportsCancellation = True
                Me.bwDownloadPic.RunWorkerAsync(New Arguments With {.pType = PicType.Actor, .pURL = Me.alActors.Item(Me.lstActors.SelectedIndex)})

            Else
                Me.pbActors.Image = My.Resources.actor_silhouette
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub tsbRefreshMedia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRefreshMedia.Click

        '//
        ' Reload media type when "Rescan Media" is clicked
        '\\

        Me.LoadMedia(1)

    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click

        '//
        ' Begin animation to raise panel all the way up
        '\\

        Me.aniType = 2
        Me.aniRaise = True
        Me.tmrAni.Start()

    End Sub


    Private Sub btnMid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMid.Click

        '//
        ' Begin animation to raise/lower panel to mid point
        '\\

        If Me.pnlInfoPanel.Height = 500 Then
            Me.aniRaise = False
        Else
            Me.aniRaise = True
        End If
        Me.aniType = 1
        Me.tmrAni.Start()

    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click

        '//
        ' Begin animation to lower panel all the way down
        '\\

        Me.aniType = 0
        Me.aniRaise = False
        Me.tmrAni.Start()

    End Sub

    Private Sub tmrAni_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrAni.Tick

        '//
        ' Just some crappy animation to make the GUI slightly more interesting
        '\\

        Try
            If Master.eSettings.InfoPanelAnim Then
                If Me.aniRaise Then
                    Me.pnlInfoPanel.Height += 5
                Else
                    Me.pnlInfoPanel.Height -= 5
                End If
            Else
                Select Case Me.aniType
                    Case 0
                        Me.pnlInfoPanel.Height = 25
     
                    Case 1
                        Me.pnlInfoPanel.Height = 280

                    Case 2
                        Me.pnlInfoPanel.Height = 500

                End Select
            End If


            Me.MoveGenres()
            Me.MoveMPAA()

            If Me.aniType = 0 Then
                If Me.pnlInfoPanel.Height = 25 Then
                    Me.tmrAni.Stop()
                    Me.btnDown.Enabled = False
                    Me.btnMid.Enabled = True
                    Me.btnUp.Enabled = True
                End If
            ElseIf Me.aniType = 1 Then
                If Me.pnlInfoPanel.Height = 280 Then
                    Me.tmrAni.Stop()
                    Me.btnMid.Enabled = False
                    Me.btnDown.Enabled = True
                    Me.btnUp.Enabled = True
                End If
            ElseIf Me.aniType = 2 Then
                If Me.pnlInfoPanel.Height = 500 Then
                    Me.tmrAni.Stop()
                    Me.btnUp.Enabled = False
                    Me.btnDown.Enabled = True
                    Me.btnMid.Enabled = True
                End If
            End If

            'move focus somewhere to stop highlighting some info boxes
            Me.txtSearch.Focus()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnMIRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMIRefresh.Click

        '//
        ' Refresh Media Info
        '\\

        Me.LoadInfo(Master.currMovie.ID, Master.currMovie.FaS.Filename, False, True, True)

    End Sub
    Private Sub dgvMediaList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellDoubleClick

        '//
        ' Show the NFO Editor
        '\\

        Try

            If Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse _
            Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy Then Return

            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Me.dgvMediaList.Rows(indX).Cells(0).Value
            Master.currMovie = Master.DB.LoadMovieFromDB(ID)
            Me.tslStatus.Text = Master.currMovie.FaS.Filename
            Me.tmpTitle = Me.dgvMediaList.Rows(indX).Cells(3).Value

            Using dEditMovie As New dlgEditMovie

                Select Case dEditMovie.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshMovie(ID) Then
                            Me.FillList(0)
                        Else
                            Me.SetListItemAfterEdit(ID, indX)
                        End If
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID, True)
                End Select

            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMediaList.Sorted

        '//
        ' Select first item in the media list after sort
        '\\

        If Me.dgvMediaList.RowCount > 0 Then
            Me.dgvMediaList.Rows(0).Selected = True
            Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Rows(0).Cells(3)
            Me.SetFilterColors()
        End If

    End Sub

    Private Sub pbGenre_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)

        '//
        ' Draw genre text over the image when mouse hovers
        '\\

        Try
            Dim iLeft As Integer = 0
            Me.GenreImage = sender.image
            Dim bmGenre As New Bitmap(Me.GenreImage)
            Dim grGenre As Graphics = Graphics.FromImage(bmGenre)
            Dim drawString As String = sender.Name
            Dim drawFont As New Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Pixel)
            Dim drawBrush As New SolidBrush(Color.White)
            Dim drawWidth As Single = grGenre.MeasureString(drawString, drawFont).Width
            Dim drawSize As Integer = (14 * (bmGenre.Width / drawWidth)) - 0.5
            drawFont = New Font("Microsoft Sans Serif", If(drawSize > 14, 14, drawSize), FontStyle.Bold, GraphicsUnit.Pixel)
            Dim drawHeight As Single = grGenre.MeasureString(drawString, drawFont).Height
            iLeft = (bmGenre.Width - grGenre.MeasureString(drawString, drawFont).Width) / 2
            grGenre.DrawString(drawString, drawFont, drawBrush, iLeft, (bmGenre.Height - drawHeight))
            sender.Image = bmGenre
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub pbGenre_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)

        '//
        ' Reset genre image when mouse leaves to "clear" the text
        '\\

        Try
            sender.image = GenreImage
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click

        '//
        ' Get me out of here!
        '\\

        Application.Exit()

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click

        '//
        ' Give credit where credit is due
        '\\
        Using dAbout As New dlgAbout
            dAbout.ShowDialog()
        End Using

    End Sub


    Private Sub btnPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlay.Click

        '//
        ' Launch video using system default player
        '\\

        Try
            If Not String.IsNullOrEmpty(Me.txtFilePath.Text) Then
                If File.Exists(Me.txtFilePath.Text) Then
                    System.Diagnostics.Process.Start(String.Concat("""", Me.txtFilePath.Text, """"))
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub scMain_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles scMain.SplitterMoved

        '//
        ' Some generic resizing when pane sizes are changed
        '\\

        Try
            If Me.dgvMediaList.Columns.Count > 0 Then
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()

                Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, (Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2)
                Me.pnlCancel.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, 100)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbPoster_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbPoster.DoubleClick

        '//
        ' Show the Poster in the Image Viewer
        '\\

        Try
            If Not IsNothing(Me.pbPoster.Image) Then
                dlgImgView.ShowDialog(Me.pbPosterCache.Image)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub pbFanart_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbFanart.DoubleClick

        '//
        ' Show the Fanart in the Image Viewer
        '\\

        Try
            If Not IsNothing(Me.pbFanartCache.Image) Then
                dlgImgView.ShowDialog(Me.pbFanartCache.Image)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvMediaList.CellPainting

        '//
        ' Add icons to media list column headers
        '\\

        Try
            If e.ColumnIndex >= 4 AndAlso e.ColumnIndex <= 9 AndAlso e.RowIndex = -1 Then
                e.PaintBackground(e.ClipBounds, False)

                Dim pt As Point = e.CellBounds.Location
                Dim offset As Integer = (e.CellBounds.Width - Me.ilColumnIcons.ImageSize.Width) / 2

                pt.X += offset
                pt.Y = 1
                Me.ilColumnIcons.Draw(e.Graphics, pt, e.ColumnIndex - 4)

                e.Handled = True

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellEnter

        '//
        ' Load media information for the selected item
        '\\

        Try

            Me.currRow = e.RowIndex
            Me.tmrWait.Enabled = False
            Me.tmrLoad.Enabled = False
            Me.tmrWait.Enabled = True

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub tmrWait_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrWait.Tick
        If Me.prevRow = Me.currRow Then
            Me.tmrLoad.Enabled = True
        Else
            Me.prevRow = Me.currRow
            Me.tmrLoad.Enabled = False
        End If
    End Sub

    Private Sub tmrLoad_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLoad.Tick
        Me.tmrWait.Enabled = False
        Me.tmrLoad.Enabled = False
        Try
            If Me.dgvMediaList.SelectedRows.Count > 0 Then
                If Me.bwLoadInfo.IsBusy Then
                    Me.bwLoadInfo.CancelAsync()
                    Do While Me.bwLoadInfo.IsBusy
                        Application.DoEvents()
                    Loop
                End If
                Me.SelectRow(Me.currRow)
            End If
        Catch
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        Me.currText = Me.txtSearch.Text

        Me.tmrSearchWait.Enabled = False
        Me.tmrSearchWait.Enabled = True
    End Sub

    Private Sub tmrSearchWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSearchWait.Tick
        If Me.prevText = Me.currText Then
            Me.tmrSearch.Enabled = True
        Else
            Me.prevText = Me.currText
        End If
    End Sub

    Private Sub tmrSearch_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSearch.Tick
        Me.tmrSearchWait.Enabled = False
        Try
            If Not String.IsNullOrEmpty(txtSearch.Text) Then
                bsMedia.Filter = String.Concat("title LIKE '%", txtSearch.Text, "%'")
            Else
                bsMedia.RemoveFilter()
            End If
        Catch
        End Try
        Me.tmrSearch.Enabled = False
    End Sub
    Private Sub tsbUpdateXBMC_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUpdateXBMC.ButtonClick
        Try
            For Each xCom As emmSettings.XBMCCom In Master.eSettings.XBMCComs
                Me.DoXCom(xCom)
            Next
        Catch
        End Try
    End Sub

    Private Sub CleanFoldersToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CleanFoldersToolStripMenuItem.Click
        '//
        ' Clean all items in folders that match user selected types
        '\\

        Dim sWarning As String = String.Empty
        Dim sWarningFile As String = String.Empty
        With Master.eSettings
            If .ExpertCleaner Then
                sWarning = String.Concat("WARNING: If you continue, all non-whitelisted file types will be deleted!", vbNewLine, vbNewLine, "Are you sure you want to continue?")
            Else
                If .CleanDotFanartJPG Then sWarningFile += String.Concat("<movie>.fanart.jpg", vbNewLine)
                If .CleanFanartJPG Then sWarningFile += String.Concat("<movie>.jpg", vbNewLine)
                If .CleanFolderJPG Then sWarningFile += String.Concat("folder.jpg", vbNewLine)
                If .CleanMovieFanartJPG Then sWarningFile += String.Concat("<movie>-fanart.jpg", vbNewLine)
                If .CleanMovieJPG Then sWarningFile += String.Concat("movie.jpg", vbNewLine)
                If .CleanMovieNameJPG Then sWarningFile += String.Concat("<movie>.jpg", vbNewLine)
                If .CleanMovieNFO Then sWarningFile += String.Concat("movie.nfo", vbNewLine)
                If .CleanMovieNFOB Then sWarningFile += String.Concat("<movie>.nfo", vbNewLine)
                If .CleanMovieTBN Then sWarningFile += String.Concat("movie.tbn", vbNewLine)
                If .CleanMovieTBNB Then sWarningFile += String.Concat("<movie>.tbn", vbNewLine)
                If .CleanPosterJPG Then sWarningFile += String.Concat("poster.jpg", vbNewLine)
                If .CleanPosterTBN Then sWarningFile += String.Concat("poster.tbn", vbNewLine)
                If .CleanExtraThumbs Then sWarningFile += String.Concat("/extrathumbs/", vbNewLine)
                sWarning = String.Concat("WARNING: If you continue, all files of the following types will be permanently deleted:", vbNewLine, vbNewLine, sWarningFile, vbNewLine, "Are you sure you want to continue?")
            End If
        End With
        If MsgBox(sWarning, MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then
            Me.ScrapeData(Master.ScrapeType.CleanFolders, Nothing, Nothing)
        End If
    End Sub

    Private Sub ConvertFileSourceToFolderSourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertFileSourceToFolderSourceToolStripMenuItem.Click

        Using dSortFiles As New dlgSortFiles
            If dSortFiles.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.LoadMedia(1)
        End Using
    End Sub

    Private Sub chkFilterNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFilterNew.Click
        Try
            Me.chkFilterDupe.Enabled = Not Me.chkFilterNew.Checked
            If Me.chkFilterNew.Checked Then
                Me.FilterArray.Add("new = 1")
                Me.chkFilterDupe.Checked = False
            Else
                Me.FilterArray.Remove("new = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub chkFilterMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterMark.Click
        Try
            Me.chkFilterDupe.Enabled = Not Me.chkFilterMark.Checked
            If Me.chkFilterMark.Checked Then
                Me.FilterArray.Add("mark = 1")
                Me.chkFilterDupe.Checked = False
            Else
                Me.FilterArray.Remove("mark = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub rbFilterAnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFilterAnd.Click
        If Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked OrElse Not Me.cbFilterSource.Text = "All" Then Me.RunFilter()
    End Sub

    Private Sub rbFilterOr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFilterOr.Click
        If Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked OrElse Not Me.cbFilterSource.Text = "All" Then Me.RunFilter()
    End Sub

    Private Sub chkFilterDupe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterDupe.Click
        Me.chkFilterMark.Enabled = Not chkFilterDupe.Checked
        Me.chkFilterNew.Enabled = Not chkFilterDupe.Checked
        Me.rbFilterAnd.Enabled = Not chkFilterDupe.Checked
        Me.rbFilterOr.Enabled = Not chkFilterDupe.Checked
        Me.cbFilterSource.Enabled = Not chkFilterDupe.Checked
        If Me.chkFilterDupe.Checked Then
            Me.chkFilterMark.Checked = False
            Me.chkFilterNew.Checked = False
            RemoveHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged
            Me.cbFilterSource.Text = "All"
            AddHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged
        End If
        Me.RunFilter()
    End Sub

    Private Sub dgvMediaList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvMediaList.KeyPress

        For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
            If drvRow.Cells(3).Value.ToString.ToLower.StartsWith(e.KeyChar.ToString.ToLower) Then
                drvRow.Selected = True
                Me.dgvMediaList.CurrentCell = drvRow.Cells(3)
                Exit For
            End If
        Next

    End Sub

    Private Sub cmnuMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMark.Click
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                SQLcommand.CommandText = "UPDATE movies SET mark = (?) WHERE id = (?);"
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    parMark.Value = If(cmnuMark.Text = "Unmark", False, True)
                    parID.Value = sRow.Cells(0).Value
                    SQLcommand.ExecuteNonQuery()
                    sRow.Cells(11).Value = parMark.Value
                Next
            End Using
            SQLtransaction.Commit()
        End Using

        Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLNewcommand.CommandText = String.Concat("SELECT COUNT(id) AS mcount FROM movies WHERE mark = 1;")
            Using SQLcount As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                If SQLcount("mcount") > 0 Then
                    Me.btnMarkAll.Text = "Unmark All"
                Else
                    Me.btnMarkAll.Text = "Mark All"
                End If
            End Using
        End Using

        Me.SetFilterColors()
    End Sub

    Private Sub cmnuLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuLock.Click
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")
                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                SQLcommand.CommandText = "UPDATE movies SET lock = (?) WHERE id = (?);"
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    parLock.Value = If(cmnuLock.Text = "Unlock", False, True)
                    parID.Value = sRow.Cells(0).Value
                    SQLcommand.ExecuteNonQuery()
                    sRow.Cells(14).Value = If(cmnuLock.Text = "Unlock", False, True)
                Next
            End Using
            SQLtransaction.Commit()
        End Using
        Me.SetFilterColors()
    End Sub

    Private Sub cmnuRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRescrape.Click

        '//
        ' Begin the process to scrape IMDB with the current ID
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, Me.dgvMediaList.SelectedRows(0).Cells(0).Value)
    End Sub

    Private Sub cmnuSearchNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuSearchNew.Click

        '//
        ' Begin the process to search IMDB for data
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, Me.dgvMediaList.SelectedRows(0).Cells(0).Value, True)
    End Sub

    Private Sub cmnuEditMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuEditMovie.Click

        '//
        ' Show the NFO Editor
        '\\

        Try
            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Me.dgvMediaList.Item(0, indX).Value

            Using dEditMovie As New dlgEditMovie
                Select Case dEditMovie.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshMovie(ID) Then
                            Me.FillList(0)
                        Else
                            Me.SetListItemAfterEdit(ID, indX)
                        End If
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID, True)
                End Select
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvMediaList.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim dgvHTI As DataGridView.HitTestInfo = sender.HitTest(e.X, e.Y)
            If dgvHTI.Type = DataGridViewHitTestType.Cell Then

                If Me.dgvMediaList.SelectedRows.Count > 1 AndAlso Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                    Dim setMark As Boolean = False
                    Dim setLock As Boolean = False
                    Me.cmnuTitle.Text = ">> Multiple <<"
                    Me.cmnuEditMovie.Visible = False
                    Me.cmnuRescrape.Visible = False
                    Me.cmnuSearchNew.Visible = False
                    Me.cmnuSep.Visible = False
                    Me.cmnuSep2.Visible = False

                    For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                        'if any one item is set as unmarked, set menu to mark
                        'else they are all marked, so set menu to unmark
                        If Not sRow.Cells(11).Value Then
                            setMark = True
                        End If
                        'if any one item is set as unlocked, set menu to lock
                        'else they are all locked so set menu to unlock
                        If Not sRow.Cells(14).Value Then
                            setLock = True
                        End If
                    Next

                    cmnuMark.Text = If(Me.dgvMediaList.Item(11, dgvHTI.RowIndex).Value, "Unmark", "Mark")
                    cmnuLock.Text = If(Me.dgvMediaList.Item(14, dgvHTI.RowIndex).Value, "Unlock", "Lock")
                    cmnuMark.Text = If(setMark, "Mark", "Unmark")
                    cmnuLock.Text = If(setLock, "Lock", "Unlock")
                Else
                    Me.cmnuEditMovie.Visible = True
                    Me.cmnuRescrape.Visible = True
                    Me.cmnuSearchNew.Visible = True
                    Me.cmnuSep.Visible = True
                    Me.cmnuSep2.Visible = True

                    If Not Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                        Me.mnuMediaList.Enabled = False
                    End If

                    cmnuTitle.Text = String.Concat(">> ", Me.dgvMediaList.Item(3, dgvHTI.RowIndex).Value, " <<")

                    If Me.bwLoadInfo.IsBusy Then
                        Me.bwLoadInfo.CancelAsync()
                        Do While Me.bwLoadInfo.IsBusy
                            Application.DoEvents()
                        Loop
                    End If

                    Me.dgvMediaList.ClearSelection()
                    Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected = True
                    Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Item(3, dgvHTI.RowIndex)
                    Me.cmnuMark.Text = If(Me.dgvMediaList.Item(11, dgvHTI.RowIndex).Value, "Unmark", "Mark")
                    Me.cmnuLock.Text = If(Me.dgvMediaList.Item(14, dgvHTI.RowIndex).Value, "Unlock", "Lock")
                End If
            End If
        End If
    End Sub

    Private Sub mnuAllAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.ScrapeModifier.All, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.ScrapeModifier.NFO, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.ScrapeModifier.Poster, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.ScrapeModifier.Fanart, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.ScrapeModifier.Extra, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskAll.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.ScrapeModifier.All, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.ScrapeModifier.NFO, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.ScrapeModifier.Poster, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.ScrapeModifier.Fanart, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.ScrapeModifier.Extra, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.ScrapeModifier.All, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.ScrapeModifier.NFO, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.ScrapeModifier.Poster, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.ScrapeModifier.Fanart, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.ScrapeModifier.Extra, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskAll.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.ScrapeModifier.All, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.ScrapeModifier.NFO, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.ScrapeModifier.Poster, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.ScrapeModifier.Fanart, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.ScrapeModifier.Extra, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.ScrapeModifier.All, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.ScrapeModifier.NFO, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.ScrapeModifier.Poster, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.ScrapeModifier.Fanart, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.ScrapeModifier.Extra, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskAll.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.ScrapeModifier.All, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.ScrapeModifier.NFO, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.ScrapeModifier.Poster, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.ScrapeModifier.Fanart, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.ScrapeModifier.Extra, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.ScrapeModifier.All, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.ScrapeModifier.NFO, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.ScrapeModifier.Poster, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.ScrapeModifier.Fanart, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.ScrapeModifier.Extra, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskAll.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.ScrapeModifier.All, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.ScrapeModifier.NFO, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.ScrapeModifier.Poster, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.ScrapeModifier.Fanart, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.ScrapeModifier.Extra, Master.DefaultOptions)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        btnCancel.Visible = False
        lblCanceling.Visible = True
        pbCanceling.Visible = True

        If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()
        If Me.bwRefreshMovies.IsBusy Then Me.bwRefreshMovies.CancelAsync()

        Do While Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy
            Application.DoEvents()
        Loop
    End Sub

    Private Sub OpenContainingFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenContainingFolderToolStripMenuItem.Click
        For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
            Process.Start("explorer.exe", Directory.GetParent(sRow.Cells(1).Value).FullName)
        Next
    End Sub

    Private Sub DeleteMovieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMovieToolStripMenuItem.Click
        Dim mMovie As Master.DBMovie
        If MsgBox(String.Concat("WARNING: THIS WILL PERMANENTLY DELETE THE SELECTED MOVIE(S) FROM THE HARD DRIVE", vbNewLine, vbNewLine, "Are you sure you want to continue?"), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Are You Sure?") = MsgBoxResult.Yes Then
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction 'Only on Batch Mode
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    mMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(sRow.Cells(0).Value))
                    Master.DeleteFiles(False, mMovie)
                    Master.DB.DeleteFromDB(Convert.ToInt64(sRow.Cells(0).Value), True)
                Next
                SQLtransaction.Commit()
            End Using
            Me.FillList(0)
        End If
    End Sub

    Private Sub CopyExistingFanartToBackdropsFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyExistingFanartToBackdropsFolderToolStripMenuItem.Click
        '//
        ' Copy all existing fanart to the backdrops folder
        '\\

        Me.ScrapeData(Master.ScrapeType.CopyBD, Nothing, Nothing)
    End Sub

    Private Sub btnMarkAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarkAll.Click
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                SQLcommand.CommandText = "UPDATE movies SET mark = (?);"
                parMark.Value = If(btnMarkAll.Text = "Unmark All", False, True)
                SQLcommand.ExecuteNonQuery()
            End Using
            SQLtransaction.Commit()
        End Using
        For Each drvRow As DataRow In dtMedia.Rows
            drvRow.Item(11) = If(btnMarkAll.Text = "Unmark All", False, True)
        Next
        Me.SetFilterColors()
        btnMarkAll.Text = If(btnMarkAll.Text = "Unmark All", "Mark All", "Unmark All")
    End Sub

    Private Sub ExportMoviesListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportMoviesListToolStripMenuItem.Click
        Using dExportMovies As New dlgExportMovies
            dExportMovies.ShowDialog()
        End Using
    End Sub

    Private Sub SetsManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetsManagerToolStripMenuItem.Click
        Using dSetsManager As New dlgSetsManager
            dSetsManager.ShowDialog()
        End Using
    End Sub

    Private Sub ClearAllCachesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearAllCachesToolStripMenuItem.Click
        If Directory.Exists(Master.TempPath) Then
            Directory.Delete(Master.TempPath, True)
        End If

        Directory.CreateDirectory(Master.TempPath)
    End Sub

    Private Sub btnFilterUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilterUp.Click
        '//
        ' Begin animation to raise panel all the way up
        '\\

        Me.aniFilterRaise = True
        Me.tmrFilterAni.Start()
    End Sub

    Private Sub btnFilterDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFilterDown.Click
        '//
        ' Begin animation to lower panel all the way down
        '\\

        Me.aniFilterRaise = False
        Me.tmrFilterAni.Start()
    End Sub

    Private Sub tmrFilterAni_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrFilterAni.Tick
        '//
        ' Just some crappy animation to make the GUI slightly more interesting
        '\\

        Try
            If Master.eSettings.InfoPanelAnim Then
                If Me.aniFilterRaise Then
                    Me.pnlFilter.Height += 5
                Else
                    Me.pnlFilter.Height -= 5
                End If
            Else
                If Me.aniFilterRaise Then
                    Me.pnlFilter.Height = 85
                Else
                    Me.pnlFilter.Height = 25
                End If
            End If
            If Me.pnlFilter.Height = 25 Then
                Me.tmrFilterAni.Stop()
                Me.btnFilterUp.Enabled = True
                Me.btnFilterDown.Enabled = False
            ElseIf Me.pnlFilter.Height = 85 Then
                Me.tmrFilterAni.Stop()
                Me.btnFilterUp.Enabled = False
                Me.btnFilterDown.Enabled = True
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cbFilterSource_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Do While Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy
                Application.DoEvents()
            Loop

            Me.chkFilterDupe.Enabled = cbFilterSource.Text = "All"
            For i As Integer = Me.FilterArray.Count - 1 To 0 Step -1
                If Strings.Left(Me.FilterArray(i), 8) = "source =" Then
                    Me.FilterArray.RemoveAt(i)
                End If
            Next

            If Not cbFilterSource.Text = "All" Then
                Me.FilterArray.Add(String.Format("source = '{0}'", cbFilterSource.Text))
                Me.chkFilterDupe.Checked = False
            End If
            Me.RunFilter()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub mnuRevertStudioTags_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRevertStudioTags.Click

        Me.ScrapeData(Master.ScrapeType.RevertStudios, Nothing, Nothing)

    End Sub

    Private Sub OfflineMediaManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OfflineMediaManagerToolStripMenuItem.Click
        Using dOfflineHolder As New dlgOfflineHolder
            If dOfflineHolder.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Me.LoadMedia(1)
            End If
        End Using
    End Sub

    Private Sub mnuAllAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoTrailer.Click
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskTrailer.Click
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoTrailer.Click
        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskTrailer.Click
        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoTrailer.Click
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskTrailer.Click
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoTrailer.Click
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskTrailer.Click
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.ScrapeModifier.Trailer, Master.DefaultOptions)
    End Sub

    Private Sub CustomUpdaterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomUpdaterToolStripMenuItem.Click
        Using dUpdate As New dlgUpdateMedia
            Dim CustomUpdater As Master.CustomUpdaterStruct = Nothing
            CustomUpdater = dUpdate.ShowDialog()
            If Not CustomUpdater.Canceled Then
                Me.ScrapeData(CustomUpdater.ScrapeType, CustomUpdater.Modifier, CustomUpdater.Options)
            End If
        End Using
    End Sub

    Private Sub mnuAllAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoMI.Click
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.ScrapeModifier.MI, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskMI.Click
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.ScrapeModifier.MI, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoMI.Click
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.ScrapeModifier.MI, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskMI.Click
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.ScrapeModifier.MI, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoMI.Click
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.ScrapeModifier.MI, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskMI.Click
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.ScrapeModifier.MI, Master.DefaultOptions)
    End Sub

    Private Sub RenamerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenamerToolStripMenuItem.Click
        Using dBulkRename As New dlgBulkRenamer
            Try
                If dBulkRename.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Me.LoadMedia(1)
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub

    Private Sub cmnuRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRefresh.Click
        Me.dgvMediaList.Cursor = Cursors.WaitCursor
        Me.dgvMediaList.Enabled = False

        Dim doFill As Boolean = False

        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                doFill = Me.RefreshMovie(sRow.Cells(0).Value)
            Next
            SQLtransaction.Commit()
        End Using

        Me.dgvMediaList.Cursor = Cursors.Default
        Me.dgvMediaList.Enabled = True

        If doFill Then FillList(0)
    End Sub

    Private Sub RefreshAllMoviesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshAllMoviesToolStripMenuItem.Click
        Dim aContents(6) As String
        Dim tmpMovie As New Media.Movie

        If Me.dtMedia.Rows.Count > 0 Then

            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.mnuMediaList.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tspbLoading.Style = ProgressBarStyle.Continuous
            Me.EnableFilters(False)

            Me.tspbLoading.Maximum = Me.dtMedia.Rows.Count + 1
            Me.tspbLoading.Value = 0
            Me.tslLoading.Text = "Refreshing Media:"
            Me.tspbLoading.Visible = True
            Me.tslLoading.Visible = True

            Me.bwRefreshMovies.WorkerReportsProgress = True
            Me.bwRefreshMovies.WorkerSupportsCancellation = True
            Me.bwRefreshMovies.RunWorkerAsync()

        End If
    End Sub
#End Region '*** Form/Controls



#Region "Background Workers"

    ' ########################################
    ' ########## WORKER COMPONENTS ###########
    ' ########################################

    Private Sub bwPrelim_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwPrelim.DoWork

        '//
        ' Thread to count directories to prepare for loading media
        '\\

        Try
            Master.alMoviePaths.Clear()
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "SELECT MoviePath FROM movies;"
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    While SQLreader.Read
                        Master.alMoviePaths.Add(SQLreader("MoviePath"))
                    End While
                End Using
            End Using

            Master.MediaList.Clear()
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "SELECT * FROM sources;"
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    While SQLreader.Read
                        Master.ScanSourceDir(SQLreader("Name"), SQLreader("Path"), SQLreader("Recursive"), SQLreader("Foldername"), SQLreader("Single"))
                        If Me.bwPrelim.CancellationPending Then
                            e.Cancel = True
                            Return
                        End If
                    End While
                End Using
            End Using

            'remove any db entries that are not in the media list
            Dim dtMediaList As New DataTable
            Dim MLFind As New MovieListFind
            Dim MLFound As New Master.FileAndSource
            Master.DB.FillDataTable(dtMediaList, "SELECT MoviePath, Id FROM movies ORDER BY ListTitle COLLATE NOCASE;")
            If dtMediaList.Rows.Count > 0 Then
                Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                    Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                        SQLcommand.CommandText = "DELETE FROM movies WHERE MoviePath = (?);"
                        Dim parPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMoviePath", DbType.String, 0, "MoviePath")
                        For Each mRow As DataRow In dtMediaList.Rows
                            MLFind.SearchString = mRow.Item(0)
                            MLFound = Master.MediaList.Find(AddressOf MLFind.Find)
                            If IsNothing(MLFound) OrElse Not Master.eSettings.ValidExts.Contains(Path.GetExtension(mRow.Item(0))) Then
                                parPath.Value = mRow.Item(0)
                                SQLcommand.ExecuteNonQuery()

                                Using SQLOthercommands As SQLite.SQLiteCommand = Master.DB.CreateCommand
                                    Dim parId As SQLite.SQLiteParameter = SQLOthercommands.Parameters.Add("parId", DbType.UInt64, 0, "MovieID")
                                    SQLOthercommands.CommandText = "DELETE FROM MoviesAStreams WHERE MovieID = (?);"
                                    parId.Value = mRow.Item(1)
                                    SQLOthercommands.ExecuteNonQuery()
                                    SQLOthercommands.CommandText = "DELETE FROM MoviesVStreams WHERE MovieID = (?);"
                                    parId.Value = mRow.Item(1)
                                    SQLOthercommands.ExecuteNonQuery()
                                    SQLOthercommands.CommandText = "DELETE FROM MoviesActors WHERE MovieID = (?);"
                                    parId.Value = mRow.Item(1)
                                    SQLOthercommands.ExecuteNonQuery()
                                    SQLOthercommands.CommandText = "DELETE FROM MoviesSubs WHERE MovieID = (?);"
                                    parId.Value = mRow.Item(1)
                                    SQLOthercommands.ExecuteNonQuery()
                                    SQLOthercommands.CommandText = "DELETE FROM MoviesPosters WHERE MovieID = (?);"
                                    parId.Value = mRow.Item(1)
                                    SQLOthercommands.ExecuteNonQuery()
                                    SQLOthercommands.CommandText = "DELETE FROM MoviesFanart WHERE MovieID = (?);"
                                    parId.Value = mRow.Item(1)
                                    SQLOthercommands.ExecuteNonQuery()
                                End Using
                            End If
                            If Me.bwPrelim.CancellationPending Then
                                e.Cancel = True
                                Return
                            End If
                        Next
                    End Using
                    SQLtransaction.Commit()
                End Using
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwPrelim_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwPrelim.RunWorkerCompleted

        '//
        ' Thread finished: set up progress bar, display count, and begin thread to load data
        '\\
        If Not e.Cancelled Then
            Try
                If Master.MediaList.Count = 0 Then

                    If isCL Then
                        Me.ScraperDone = True
                    Else
                        Me.tslStatus.Text = "Unable to load directories. Please check settings."
                        Me.tspbLoading.Visible = False
                        Me.tslLoading.Visible = False
                        Me.tabsMain.Enabled = True
                        Me.tsbRefreshMedia.Enabled = True
                        Me.ToolsToolStripMenuItem.Enabled = False
                        Me.tsbAutoPilot.Enabled = False
                        Me.mnuMediaList.Enabled = False
                    End If

                Else
                    Me.tslLoading.Text = "Loading Media:"
                    Me.tspbLoading.Style = ProgressBarStyle.Continuous
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True
                    Me.tspbLoading.Maximum = (Master.MediaList.Count + 1)

                    Me.bwFolderData = New System.ComponentModel.BackgroundWorker
                    Me.bwFolderData.WorkerReportsProgress = True
                    Me.bwFolderData.WorkerSupportsCancellation = True
                    Me.bwFolderData.RunWorkerAsync()
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Sub

    Private Sub bwFolderData_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwFolderData.DoWork

        '//
        ' Thread to fill a datatable with basic media data
        '\\
        Dim currentIndex As Integer = 0
        Dim tmpMovieDB As New Master.DBMovie
        Try
            tmpMovieDB.Movie = New Media.Movie
            'process the folder type media
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sFile As Master.FileAndSource In Master.MediaList
                    If Me.bwFolderData.CancellationPending Then
                        e.Cancel = True
                        Return
                    End If
                    If Not String.IsNullOrEmpty(sFile.Filename) AndAlso Not sFile.Source = "[!FROMDB!]" Then
                        If Not String.IsNullOrEmpty(sFile.Nfo) Then
                            tmpMovieDB.Movie = Master.LoadMovieFromNFO(sFile.Nfo, sFile.isSingle)
                        Else
                            tmpMovieDB.Movie = Master.LoadMovieFromNFO(sFile.Filename, sFile.isSingle)
                        End If

                        If String.IsNullOrEmpty(tmpMovieDB.Movie.Title) Then
                            'no title so assume it's an invalid nfo, clear nfo path if exists
                            sFile.Nfo = String.Empty

                            If sFile.UseFolder Then
                                If Directory.GetParent(sFile.Filename).Name.ToLower = "video_ts" Then
                                    tmpMovieDB.ListTitle = Master.FilterName(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name)
                                Else
                                    tmpMovieDB.ListTitle = Master.FilterName(Directory.GetParent(sFile.Filename).Name)
                                End If
                            Else
                                tmpMovieDB.ListTitle = Master.FilterName(Path.GetFileNameWithoutExtension(sFile.Filename))
                            End If
                        Else
                            tmpMovieDB.ListTitle = tmpMovieDB.Movie.Title
                        End If

                        Me.bwFolderData.ReportProgress(currentIndex, tmpMovieDB.ListTitle)
                        If Not String.IsNullOrEmpty(tmpMovieDB.ListTitle) Then
                            tmpMovieDB.FaS = sFile
                            'tmpMovieDB.Movie.FileNameAndPath
                            tmpMovieDB.IsNew = True
                            tmpMovieDB.IsLock = False
                            tmpMovieDB.IsMark = Master.eSettings.MarkNew
                            'Do the Save
                            tmpMovieDB = Master.DB.SaveMovieToDB(tmpMovieDB, True, True)
                        End If
                        currentIndex += 1
                    End If
                Next
                SQLtransaction.Commit()
            End Using
            If Me.bwFolderData.CancellationPending Then
                e.Cancel = True
                Return
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwFolderData_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwFolderData.ProgressChanged

        '//
        ' Update the status bar with the name of the current media name and increase progress bar
        '\\

        If Not isCL Then
            Me.tspbLoading.Value = e.ProgressPercentage
            Me.tslStatus.Text = e.UserState.ToString
        End If

    End Sub

    Private Sub bwFolderData_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwFolderData.RunWorkerCompleted

        '//
        ' Thread finished: fill datagrid with info and configure columns
        '\\

        If Not e.Cancelled Then
            Me.FillList(0)
        End If

    End Sub

    Private Sub bwMediaInfo_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwMediaInfo.DoWork

        '//
        ' Thread to procure technical and tag information about media via MediaInfo.dll
        '\\
        Dim Args As Arguments = e.Argument

        Try
            Me.UpdateMediaInfo(Args.Movie)
            Master.DB.SaveMovieToDB(Args.Movie, False, False, True)

            If Me.bwMediaInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            e.Result = New Results With {.fileinfo = Master.FIToString(Args.Movie.Movie.FileInfo), .setEnabled = Args.setEnabled, .Path = Args.Path, .Movie = Args.Movie}
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            e.Result = New Results With {.fileinfo = "error", .setEnabled = Args.setEnabled}
        End Try
    End Sub

    Private Sub bwMediaInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwMediaInfo.RunWorkerCompleted

        '//
        ' Thread finished: fill textbox with result
        '\\

        If Not e.Cancelled Then
            Dim Res As Results = e.Result

            Try
                If Not Res.fileInfo = "error" Then
                    Me.pbMILoading.Visible = False
                    Me.txtMediaInfo.Text = Res.fileInfo
                    If Master.eSettings.ScanMediaInfo Then
                        Master.GetAVImages(Res.Movie)
                        Me.pnlInfoIcons.Width = 390
                        Me.pbStudio.Left = 325
                    Else
                        Me.pnlInfoIcons.Width = 65
                        Me.pbStudio.Left = 0
                    End If
                    If Master.eSettings.UseMIDuration Then
                        If Not String.IsNullOrEmpty(Res.Movie.Movie.Runtime) Then
                            Me.lblRuntime.Text = String.Format("Runtime: {0}", Res.Movie.Movie.Runtime)
                        End If
                    End If
                    Me.btnMIRefresh.Focus()
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            If Res.setEnabled Then
                Me.tabsMain.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                If Me.dgvMediaList.RowCount > 0 Then
                    Me.ToolsToolStripMenuItem.Enabled = True
                    Me.tsbAutoPilot.Enabled = True
                    Me.mnuMediaList.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub bwDownloadPic_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadPic.DoWork

        '//
        ' Thread to download image from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\

        Dim Args As Arguments = e.Argument
        Dim tImage As Image = Nothing

        Dim wrRequest As WebRequest = WebRequest.Create(Args.pURL)
        wrRequest.Timeout = 10000
        Using wrResponse As WebResponse = wrRequest.GetResponse()
            tImage = Image.FromStream(wrResponse.GetResponseStream())
        End Using
        e.Result = New Results With {.ResultType = Args.pType, .Result = tImage}

    End Sub

    Private Sub bwDownloadPic_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadPic.RunWorkerCompleted

        '//
        ' Thread finished: display pic if it was able to get one
        '\\

        Dim Res As Results = e.Result

        Select Case Res.ResultType
            Case PicType.Actor
                Me.pbActLoad.Visible = False
                Me.pbActors.Image = Res.Result
            Case Else
        End Select

    End Sub

    Private Sub bwLoadInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadInfo.DoWork

        '//
        ' Thread to load media images and information (from nfo) then display on screen
        '\\

        Try

            Dim Args As Arguments = e.Argument
            Me.MainFanart.Clear()
            Me.MainPoster.Clear()

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            Master.currMovie = Master.DB.LoadMovieFromDB(Args.ID)

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayFanart Then Me.MainFanart.FromFile(Master.currMovie.FaS.Fanart)

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayPoster Then Me.MainPoster.FromFile(Master.currMovie.FaS.Poster)
            'read nfo if it's there

            'wait for mediainfo to update the nfo
            Do While bwMediaInfo.IsBusy
                Application.DoEvents()
            Loop

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try


    End Sub

    Private Sub bwLoadInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadInfo.RunWorkerCompleted

        '//
        ' Thread finished: display it
        '\\

        Try
            If Not e.Cancelled Then
                Me.fillScreenInfo()

                If Not IsNothing(Me.MainFanart.Image) Then
                    Me.pbFanartCache.Image = Me.MainFanart.Image
                Else
                    If Not IsNothing(Me.pbFanartCache.Image) Then
                        Me.pbFanartCache.Image.Dispose()
                        Me.pbFanartCache.Image = Nothing
                    End If
                    If Not IsNothing(Me.pbFanart.Image) Then
                        Me.pbFanart.Image.Dispose()
                        Me.pbFanart.Image = Nothing
                    End If
                End If

                Dim g As Graphics
                Dim strSize As String
                Dim lenSize
                Dim rect As Rectangle

                If Not IsNothing(Me.MainPoster.Image) Then
                    Me.pbPosterCache.Image = Me.MainPoster.Image
                    Master.ResizePB(Me.pbPoster, Me.pbPosterCache, 160, 160)
                    Master.SetOverlay(Me.pbPoster)
                    Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)

                    If Master.eSettings.ShowDims Then
                        g = Graphics.FromImage(pbPoster.Image)
                        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        strSize = String.Format("{0} x {1}", Me.MainPoster.Image.Width, Me.MainPoster.Image.Height)
                        lenSize = g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width
                        rect = New Rectangle((pbPoster.Image.Width - lenSize) / 2 - 15, Me.pbPoster.Height - 25, lenSize + 30, 25)
                        Master.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                        g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbPoster.Image.Width - lenSize) / 2, Me.pbPoster.Height - 20)
                    End If

                    Me.pbPoster.Location = New Point(4, 4)
                    Me.pnlPoster.Visible = True
                Else
                    If Not IsNothing(Me.pbPoster.Image) Then
                        Me.pbPoster.Image.Dispose()
                        Me.pbPoster.Image = Nothing
                        Me.pnlPoster.Visible = False
                    End If
                End If

                Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2

                If Not IsNothing(pbFanart.Image) AndAlso Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbFanart.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainFanart.Image.Width, Me.MainFanart.Image.Height)
                    lenSize = g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width
                    rect = New Rectangle((pbFanart.Image.Width - lenSize) - 40, 10, lenSize + 30, 25)
                    Master.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbFanart.Image.Width - lenSize) - 25, 15)
                End If

                If Not bwScraper.IsBusy Then
                    Me.ToolsToolStripMenuItem.Enabled = True
                    Me.tsbAutoPilot.Enabled = True
                    Me.tsbRefreshMedia.Enabled = True
                    Me.mnuMediaList.Enabled = True
                    Me.tabsMain.Enabled = True
                    Me.EnableFilters(True)
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwScraper_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwScraper.DoWork

        '//
        ' Thread to handle scraping
        '\\


        Dim Args As Arguments = e.Argument
        Dim TMDB As New TMDB.Scraper
        Dim IMPA As New IMPA.Scraper
        Dim Trailer As New Trailers
        Dim iCount As Integer = 0
        Dim tURL As String = String.Empty
        Dim fArt As New Media.Fanart
        Dim pThumbs As New Media.Poster
        Dim Poster As New Images
        Dim Fanart As New Images
        Dim scrapeMovie As New Master.DBMovie
        Dim doSave As Boolean = False
        Dim pPath As String = String.Empty
        Dim fPath As String = String.Empty

        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction

            Try
                If Me.dtMedia.Rows.Count > 0 Then

                    Select Case Args.scrapeType
                        Case Master.ScrapeType.FullAuto, Master.ScrapeType.NewAuto, Master.ScrapeType.MarkAuto, Master.ScrapeType.FullAsk, Master.ScrapeType.NewAsk, Master.ScrapeType.MarkAsk
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                If Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.NewAuto Then
                                    If Not drvRow.Item(10) Then Continue For
                                ElseIf Args.scrapeType = Master.ScrapeType.MarkAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAuto Then
                                    If Not drvRow.Item(11) Then Continue For
                                End If

                                If Me.bwScraper.CancellationPending Then GoTo doCancel

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                iCount += 1

                                If drvRow.Item(14) Then Continue For

                                doSave = False

                                scrapeMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(drvRow.Item(0)))

                                If Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.NFO Then
                                    If Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) Then
                                        IMDB.GetMovieInfo(scrapeMovie.Movie.IMDBID, scrapeMovie.Movie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False, Args.Options)
                                    Else
                                        scrapeMovie.Movie = IMDB.GetSearchMovieInfo(drvRow.Item(3).ToString, New Media.Movie, Args.scrapeType, Args.Options)
                                    End If
                                    doSave = True
                                End If

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                If Master.eSettings.ScanMediaInfo AndAlso (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.MI) Then
                                    UpdateMediaInfo(scrapeMovie)
                                    doSave = True
                                End If

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                If Not String.IsNullOrEmpty(scrapeMovie.Movie.Title) Then
                                    drvRow.Item(3) = scrapeMovie.Movie.Title
                                    scrapeMovie.ListTitle = scrapeMovie.Movie.Title
                                Else
                                    scrapeMovie.ListTitle = drvRow.Item(3)
                                End If

                                If Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) Then

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Poster) Then
                                        pThumbs.Clear()
                                        Poster.Clear()
                                        If Poster.IsAllowedToDownload(scrapeMovie, Master.ImageType.Posters) Then
                                            If Poster.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs, If(Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk, True, False)) Then
                                                If Not IsNothing(Poster.Image) Then
                                                    pPath = Poster.SaveAsPoster(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(pPath) Then
                                                        scrapeMovie.FaS.Poster = pPath
                                                        drvRow.Item(4) = True
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Thumbs = pThumbs
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk Then
                                                    MsgBox("A poster of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                    Using dImgSelect As New dlgImgSelect
                                                        pPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Posters)
                                                        If Not String.IsNullOrEmpty(pPath) Then
                                                            scrapeMovie.FaS.Poster = pPath
                                                            drvRow.Item(4) = True
                                                            If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                                scrapeMovie.Movie.Thumbs = pThumbs
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Fanart) Then
                                        fArt.Clear()
                                        Fanart.Clear()
                                        If Fanart.IsAllowedToDownload(scrapeMovie, Master.ImageType.Fanart) Then
                                            If Fanart.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Fanart, fArt, Nothing, If(Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk, True, False)) Then
                                                If Not IsNothing(Fanart.Image) Then
                                                    fPath = Fanart.SaveAsFanart(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(fPath) Then
                                                        scrapeMovie.FaS.Fanart = fPath
                                                        drvRow.Item(5) = True
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Fanart = fArt
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk Then
                                                    MsgBox("Fanart of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")

                                                    Using dImgSelect As New dlgImgSelect
                                                        fPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Fanart)
                                                        If Not String.IsNullOrEmpty(fPath) Then
                                                            scrapeMovie.FaS.Fanart = fPath
                                                            drvRow.Item(5) = True
                                                            If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                                scrapeMovie.Movie.Fanart = fArt
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Args.scrapeMod = Master.ScrapeModifier.All AndAlso Master.eSettings.UpdaterTrailers) OrElse Args.scrapeMod = Master.ScrapeModifier.Trailer Then
                                        tURL = Trailer.DownloadSingleTrailer(scrapeMovie.FaS.Filename, scrapeMovie.Movie.IMDBID, drvRow.Item(2), scrapeMovie.Movie.Trailer)
                                        If Not String.IsNullOrEmpty(tURL) Then
                                            If tURL.Substring(0, 7) = "http://" Then
                                                scrapeMovie.Movie.Trailer = tURL
                                                doSave = True
                                            Else
                                                scrapeMovie.FaS.Trailer = tURL
                                                drvRow.Item(7) = True
                                            End If
                                        End If
                                    End If

                                End If

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                If (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Extra) Then
                                    If Master.eSettings.AutoThumbs > 0 AndAlso Not drvRow.Item(2) Then
                                        If Master.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs) Then
                                            drvRow.Item(5) = True
                                            scrapeMovie.FaS.Extra = "TRUE"
                                        End If
                                    End If
                                End If

                                If doSave Then drvRow.Item(6) = True

                                Master.DB.SaveMovieToDB(scrapeMovie, False, True, doSave)
                            Next

                        Case Master.ScrapeType.UpdateAsk, Master.ScrapeType.UpdateAuto
                            For Each drvRow As DataRow In Me.dtMedia.Rows
                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                iCount += 1

                                If drvRow.Item(14) Then Continue For

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                If (Not drvRow.Item(4) AndAlso Args.scrapeMod = Master.ScrapeModifier.Poster) OrElse (Not drvRow.Item(5) AndAlso Args.scrapeMod = Master.ScrapeModifier.Fanart) OrElse _
                                (Not drvRow.Item(6) AndAlso Args.scrapeMod = Master.ScrapeModifier.NFO) OrElse (Not drvRow.Item(7) AndAlso Args.scrapeMod = Master.ScrapeModifier.Trailer) OrElse _
                                ((Not drvRow.Item(4) OrElse Not drvRow.Item(5) OrElse Not drvRow.Item(6) OrElse Not drvRow.Item(7)) AndAlso Args.scrapeMod = Master.ScrapeModifier.All) Then

                                    doSave = False

                                    scrapeMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(drvRow.Item(0)))

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel

                                    If Not drvRow.Item(6) AndAlso (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.NFO) Then

                                        If String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) OrElse Not IMDB.GetMovieInfo(scrapeMovie.Movie.IMDBID, scrapeMovie.Movie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False, Args.Options) Then
                                            scrapeMovie.Movie = IMDB.GetSearchMovieInfo(drvRow.Item(3).ToString, New Media.Movie, Args.scrapeType, Args.Options)
                                        End If

                                        If Master.eSettings.ScanMediaInfo Then
                                            UpdateMediaInfo(scrapeMovie)
                                        End If

                                        If Not String.IsNullOrEmpty(scrapeMovie.Movie.Title) Then
                                            drvRow.Item(3) = scrapeMovie.Movie.Title
                                            scrapeMovie.ListTitle = scrapeMovie.Movie.Title
                                        Else
                                            scrapeMovie.ListTitle = drvRow.Item(3)
                                        End If

                                        drvRow.Item(6) = True
                                        doSave = True
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Not drvRow.Item(4) AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) AndAlso (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Poster) Then
                                        pThumbs.Clear()
                                        Poster.Clear()
                                        If Poster.IsAllowedToDownload(scrapeMovie, Master.ImageType.Posters) Then
                                            If Poster.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs, If(Args.scrapeType = Master.ScrapeType.UpdateAsk, True, False)) Then
                                                If Not IsNothing(Poster.Image) Then
                                                    pPath = Poster.SaveAsPoster(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(pPath) Then
                                                        scrapeMovie.FaS.Poster = pPath
                                                        drvRow.Item(4) = True
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Thumbs = pThumbs
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.UpdateAsk Then
                                                    MsgBox("A poster of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                    Using dImgSelect As New dlgImgSelect
                                                        pPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Posters)
                                                        If Not String.IsNullOrEmpty(pPath) Then
                                                            scrapeMovie.FaS.Poster = pPath
                                                            drvRow.Item(4) = True
                                                            If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                                scrapeMovie.Movie.Thumbs = pThumbs
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Not drvRow.Item(5) AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) AndAlso (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Fanart) Then
                                        fArt.Clear()
                                        Fanart.Clear()
                                        If Fanart.IsAllowedToDownload(scrapeMovie, Master.ImageType.Fanart) Then
                                            If Fanart.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Fanart, fArt, Nothing, If(Args.scrapeType = Master.ScrapeType.UpdateAsk, True, False)) Then

                                                If Not IsNothing(Fanart.Image) Then
                                                    fPath = Fanart.SaveAsFanart(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(fPath) Then
                                                        scrapeMovie.FaS.Fanart = fPath
                                                        drvRow.Item(5) = True
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Fanart = fArt
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.UpdateAsk Then
                                                    MsgBox("Fanart of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                    Using dImgSelect As New dlgImgSelect
                                                        fPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Fanart)
                                                        If Not String.IsNullOrEmpty(fPath) Then
                                                            scrapeMovie.FaS.Fanart = fPath
                                                            drvRow.Item(5) = True
                                                            If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                                scrapeMovie.Movie.Fanart = fArt
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Not drvRow.Item(7) AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) AndAlso ((Args.scrapeMod = Master.ScrapeModifier.All AndAlso Master.eSettings.UpdaterTrailers) OrElse Args.scrapeMod = Master.ScrapeModifier.Trailer) Then
                                        tURL = Trailer.DownloadSingleTrailer(scrapeMovie.FaS.Filename, scrapeMovie.Movie.IMDBID, drvRow.Item(2), scrapeMovie.Movie.Trailer)
                                        If Not String.IsNullOrEmpty(tURL) Then
                                            If tURL.Substring(0, 7) = "http://" Then
                                                scrapeMovie.Movie.Trailer = tURL
                                                doSave = True
                                            Else
                                                scrapeMovie.FaS.Trailer = tURL
                                                drvRow.Item(7) = True
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Master.eSettings.AutoThumbs > 0 AndAlso Not drvRow.Item(2) AndAlso Not Directory.Exists(Path.Combine(Directory.GetParent(scrapeMovie.FaS.Filename).FullName, "extrathumbs")) AndAlso _
                                    (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Extra) Then
                                        If Master.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs) Then drvRow.Item(5) = True
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    Master.DB.SaveMovieToDB(scrapeMovie, False, True, doSave)

                                End If

                            Next

                        Case Master.ScrapeType.CleanFolders
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(3))
                                iCount += 1

                                If drvRow.Item(14) Then Continue For

                                If Me.bwScraper.CancellationPending Then GoTo doCancel

                                scrapeMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(drvRow.Item(0)))
                                If Master.DeleteFiles(True, scrapeMovie) Then Me.RefreshMovie(Convert.ToInt64(drvRow.Item(0)), True, False)
                            Next

                        Case Master.ScrapeType.CopyBD
                            Dim sPath As String = String.Empty
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                iCount += 1

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                sPath = drvRow.Item(39)
                                If Not String.IsNullOrEmpty(sPath) Then
                                    If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                                        If Master.eSettings.VideoTSParent Then
                                            File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), "-fanart.jpg")), True)
                                        Else
                                            If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                                File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")), True)
                                            Else
                                                File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)), True)
                                            End If
                                        End If
                                    Else
                                        If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                            File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString), "-fanart.jpg")), True)
                                        Else
                                            File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)), True)
                                        End If
                                    End If
                                End If
                            Next
                        Case Master.ScrapeType.RevertStudios
                            For Each drvRow As DataRow In Me.dtMedia.Rows
                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                iCount += 1

                                If Me.bwScraper.CancellationPending Then GoTo doCancel

                                scrapeMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(drvRow.Item(0)))

                                If Not String.IsNullOrEmpty(scrapeMovie.Movie.Studio) AndAlso scrapeMovie.Movie.Studio.Contains(" / ") Then
                                    scrapeMovie.Movie.Studio = Strings.Trim(Strings.Left(scrapeMovie.Movie.Studio, Strings.InStr(scrapeMovie.Movie.Studio, " / ") - 1))
                                Else
                                    scrapeMovie.Movie.Studio = String.Empty
                                End If

                                Master.DB.SaveMovieToDB(scrapeMovie, False, True, True)

                            Next

                    End Select
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

doCancel:
            SQLtransaction.Commit()
        End Using
        e.Result = Args.scrapeType

        pThumbs = Nothing
        fArt = Nothing

        TMDB = Nothing
        IMPA = Nothing
        Poster.Dispose()
        Poster = Nothing

        Fanart.Dispose()
        Fanart = Nothing
    End Sub

    Private Sub bwScraper_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwScraper.ProgressChanged
        If Not isCL Then
            Me.tslStatus.Text = e.UserState.ToString
            Me.tspbLoading.Value = e.ProgressPercentage
        End If
    End Sub

    Private Sub bwScraper_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwScraper.RunWorkerCompleted

        '//
        ' Thread finished: re-fill media list and load info for first item
        '\\

        Try
            If isCL Then
                Me.ScraperDone = True
            Else
                Me.pnlCancel.Visible = False

                Select Case e.Result
                    Case Master.ScrapeType.CleanFolders
                        'only rescan media if expert cleaner and videos are not whitelisted 
                        'since the db is updated during cleaner now.
                        If Master.eSettings.ExpertCleaner AndAlso Not Master.eSettings.CleanWhitelistVideo Then
                            Me.LoadMedia(1)
                        Else
                            Me.dgvMediaList.Refresh()
                        End If

                    Case Else
                        If Me.dgvMediaList.SelectedRows.Count > 0 Then
                            Me.FillList(Me.dgvMediaList.SelectedRows(0).Index)
                        Else
                            Me.FillList(0)
                        End If
                End Select
            End If
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.tslStatus.Text = String.Empty

            Me.ToolsToolStripMenuItem.Enabled = True
            Me.tsbAutoPilot.Enabled = True
            Me.tsbRefreshMedia.Enabled = True
            Me.mnuMediaList.Enabled = True
            Me.tabsMain.Enabled = True
            Me.EnableFilters(True)
            Me.EnableSorting(True)

            Me.btnMarkAll.Enabled = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwRefreshMovies_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwRefreshMovies.DoWork
        Dim iCount As Integer = 0
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            For Each sRow As DataRow In Me.dtMedia.Rows
                If Me.bwScraper.CancellationPending Then Return
                Me.bwRefreshMovies.ReportProgress(iCount, sRow.Item(1))
                Me.RefreshMovie(sRow.Item(0), True)
                iCount += 1
            Next
            SQLtransaction.Commit()
        End Using
    End Sub

    Private Sub bwRefreshMovies_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwRefreshMovies.ProgressChanged
        Me.tslStatus.Text = e.UserState.ToString
        Me.tspbLoading.Value = e.ProgressPercentage
    End Sub

    Private Sub bwRefreshMovies_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwRefreshMovies.RunWorkerCompleted

        Me.tslLoading.Text = String.Empty
        Me.tspbLoading.Visible = False
        Me.tslLoading.Visible = False
        Me.ToolsToolStripMenuItem.Enabled = True
        Me.tsbAutoPilot.Enabled = True
        Me.tsbRefreshMedia.Enabled = True
        Me.mnuMediaList.Enabled = True
        Me.tabsMain.Enabled = True
        Me.EnableFilters(True)

        Me.FillList(0)
    End Sub

#End Region '*** Background Workers



#Region "Routines/Functions"

    ' ########################################
    ' ###### GENERAL ROUTINES/FUNCTIONS ######
    ' ########################################

    Public Sub SetColors()

        Try
            With Me
                'top panel
                .pnlTop.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pnlInfoIcons.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pnlRating.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbVideo.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbResolution.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbAudio.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbChannels.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStudio.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar1.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar2.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar3.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar4.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar5.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .lblTitle.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)
                .lblVotes.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)
                .lblRuntime.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)
                .lblTagline.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)

                'background
                .scMain.Panel2.BackColor = Color.FromArgb(Master.eSettings.BackgroundColor)
                .pbFanart.BackColor = Color.FromArgb(Master.eSettings.BackgroundColor)

                'info panel
                .pnlInfoPanel.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtMediaInfo.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtMediaInfo.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .txtPlot.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtPlot.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .txtOutline.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtOutline.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .pnlActors.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lstActors.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lstActors.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .lblDirector.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblDirector.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .lblReleaseDate.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblReleaseDate.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .pnlTop250.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblTop250.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblTop250.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)

                .lblMIHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblMIHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblPlotHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblPlotHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblOutlineHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblOutlineHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblActorsHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblActorsHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblFilePathHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblFilePathHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblIMDBHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblIMDBHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblDirectorHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblDirectorHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblReleaseDateHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblReleaseDateHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblCertsHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblCertsHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblInfoPanelHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblInfoPanelHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)

                'left panel
                .scMain.Panel1.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .pnlSearch.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .pnlFilter.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblFilter.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblFilter.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)


            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub MoveGenres()

        '//
        ' Slide the genre images along with the panel and move with form resizing
        '\\

        Try
            For i As Integer = 0 To UBound(Me.pnlGenre)
                Me.pnlGenre(i).Left = ((Me.pnlInfoPanel.Right) - (i * 73)) - 73
                Me.pnlGenre(i).Top = Me.pnlInfoPanel.Top - 105
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub MoveMPAA()

        '//
        ' Slide the MPAA image along with the panel
        '\\

        Me.pnlMPAA.BringToFront()
        Me.pnlMPAA.Size = New Size(Me.pbMPAA.Width + 10, Me.pbMPAA.Height + 10)
        Me.pbMPAA.Location = New Point(4, 4)
        Me.pnlMPAA.Top = Me.pnlInfoPanel.Top - (Me.pnlMPAA.Height + 10)

    End Sub

    Public Sub LoadMedia(ByVal mediaType As Integer)

        '//
        ' Begin threads to fill datagrid with media data
        '\\


        Try
            Me.tslStatus.Text = "Performing Preliminary Tasks (Gathering Data)..."
            If bwPrelim.IsBusy Then
                bwPrelim.CancelAsync()
                While bwPrelim.IsBusy
                    Application.DoEvents()
                End While
            End If

            If bwFolderData.IsBusy Then
                bwFolderData.CancelAsync()
                While bwFolderData.IsBusy
                    Application.DoEvents()
                End While
            End If

            Me.SaveMovieList()
            Me.txtSearch.Text = String.Empty

            Me.dgvMediaList.DataSource = Nothing

            Me.ClearInfo()
            Me.EnableFilters(False)

            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.mnuMediaList.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tabMovies.Text = "Movies"

            Me.bwPrelim = New System.ComponentModel.BackgroundWorker
            Me.bwPrelim.WorkerSupportsCancellation = True
            Me.bwPrelim.RunWorkerAsync()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try


    End Sub

    Private Sub LoadInfo(ByVal ID As Integer, ByVal sPath As String, ByVal doInfo As Boolean, ByVal doMI As Boolean, Optional ByVal setEnabled As Boolean = False)

        '//
        ' Begin threads to load images and media info from nfos
        '\\

        Try
            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.tabsMain.Enabled = False
            Me.pnlNoInfo.Visible = False

            'set status bar text to movie path
            Me.tslStatus.Text = sPath

            If Me.bwDownloadPic.IsBusy Then
                Me.bwDownloadPic.CancelAsync()
                While Me.bwDownloadPic.IsBusy
                    Application.DoEvents()
                End While
            End If

            If Me.bwMediaInfo.IsBusy Then
                Me.bwMediaInfo.CancelAsync()
                While Me.bwMediaInfo.IsBusy
                    Application.DoEvents()
                End While
            End If

            If Me.bwLoadInfo.IsBusy Then
                Me.bwLoadInfo.CancelAsync()
                While Me.bwLoadInfo.IsBusy
                    Application.DoEvents()
                End While
            End If

            If doMI Then
                Me.txtMediaInfo.Clear()
                Me.pbMILoading.Visible = True
            End If

            If doMI Then
                Me.bwMediaInfo = New System.ComponentModel.BackgroundWorker
                Me.bwMediaInfo.WorkerSupportsCancellation = True
                Me.bwMediaInfo.RunWorkerAsync(New Arguments With {.setEnabled = setEnabled, .Path = sPath, .Movie = Master.currMovie})
            End If

            If doInfo Then
                Me.ClearInfo()
                Me.bwLoadInfo = New System.ComponentModel.BackgroundWorker
                Me.bwLoadInfo.WorkerSupportsCancellation = True
                Me.bwLoadInfo.RunWorkerAsync(New Arguments With {.ID = ID})
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Public Sub ClearInfo()

        '//
        ' Reset all info fields
        '\\
        Try
            With Me
                If Not IsNothing(.pbFanart.Image) Then
                    .pbFanart.Image.Dispose()
                    .pbFanart.Image = Nothing
                End If
                If Not IsNothing(.pbPoster.Image) Then
                    .pbPoster.Image.Dispose()
                    .pbPoster.Image = Nothing
                End If
                .pnlPoster.Visible = False

                'remove all the current genres
                Try
                    For iDel As Integer = UBound(.pnlGenre) To 0 Step -1
                        .scMain.Panel2.Controls.Remove(.pbGenre(iDel))
                        .scMain.Panel2.Controls.Remove(.pnlGenre(iDel))
                    Next
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

                If Not IsNothing(.pbMPAA.Image) Then
                    .pbMPAA.Image = Nothing
                End If
                .pnlMPAA.Visible = False

                .lblTitle.Text = String.Empty
                .lblVotes.Text = String.Empty
                .lblRuntime.Text = String.Empty
                .pnlTop250.Visible = False
                .lblTop250.Text = String.Empty
                .pbStar1.Image = Nothing
                .pbStar2.Image = Nothing
                .pbStar3.Image = Nothing
                .pbStar4.Image = Nothing
                .pbStar5.Image = Nothing
                .lstActors.Items.Clear()
                If Not IsNothing(.alActors) Then
                    .alActors.Clear()
                    .alActors = Nothing
                End If
                If Not IsNothing(.pbActors.Image) Then
                    .pbActors.Image.Dispose()
                    .pbActors.Image = Nothing
                End If
                .lblDirector.Text = String.Empty
                .lblReleaseDate.Text = String.Empty
                .txtCerts.Text = String.Empty
                .txtIMDBID.Text = String.Empty
                .txtFilePath.Text = String.Empty
                .txtOutline.Text = String.Empty
                .txtPlot.Text = String.Empty
                .lblTagline.Text = String.Empty
                If Not IsNothing(.pbMPAA.Image) Then
                    .pbMPAA.Image.Dispose()
                    .pbMPAA.Image = Nothing
                End If
                .pbStudio.Image = Nothing
                .pbVideo.Image = Nothing
                .pbVType.Image = Nothing
                .pbAudio.Image = Nothing
                .pbResolution.Image = Nothing
                .pbChannels.Image = Nothing

                .MainPoster.Clear()
                .MainFanart.Clear()

                .txtMediaInfo.Text = String.Empty
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub fillScreenInfo()

        '//
        ' Get info from arralist (populated by bw) and parse to screen
        '\\

        Try
            Me.SuspendLayout()
            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Title) AndAlso Not String.IsNullOrEmpty(Master.currMovie.Movie.Year) Then
                Me.lblTitle.Text = String.Format("{0} ({1})", Master.currMovie.Movie.Title, Master.currMovie.Movie.Year)
            ElseIf Not String.IsNullOrEmpty(Master.currMovie.Movie.Title) AndAlso String.IsNullOrEmpty(Master.currMovie.Movie.Year) Then
                Me.lblTitle.Text = Master.currMovie.Movie.Title
            ElseIf String.IsNullOrEmpty(Master.currMovie.Movie.Title) AndAlso Not String.IsNullOrEmpty(Master.currMovie.Movie.Year) Then
                Me.lblTitle.Text = String.Format("Unknown Movie ({0})", Master.currMovie.Movie.Year)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Votes) Then
                Me.lblVotes.Text = String.Format("{0} Votes", Master.currMovie.Movie.Votes)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Runtime) Then
                Me.lblRuntime.Text = String.Format("Runtime: {0}", If(Master.currMovie.Movie.Runtime.Contains("|"), Strings.Left(Master.currMovie.Movie.Runtime, Master.currMovie.Movie.Runtime.IndexOf("|")), Master.currMovie.Movie.Runtime)).Trim
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Top250) AndAlso IsNumeric(Master.currMovie.Movie.Top250) AndAlso (IsNumeric(Master.currMovie.Movie.Top250) AndAlso Convert.ToInt32(Master.currMovie.Movie.Top250) > 0) Then
                Me.pnlTop250.Visible = True
                Me.lblTop250.Text = Master.currMovie.Movie.Top250
            Else
                Me.pnlTop250.Visible = False
            End If

            Me.txtOutline.Text = Master.currMovie.Movie.Outline
            Me.txtPlot.Text = Master.currMovie.Movie.Plot
            Me.lblTagline.Text = Master.currMovie.Movie.Tagline

            Me.alActors = New ArrayList

            If Master.currMovie.Movie.Actors.Count > 0 Then
                Me.pbActors.Image = My.Resources.actor_silhouette
                For Each imdbAct As Media.Person In Master.currMovie.Movie.Actors
                    If Not String.IsNullOrEmpty(imdbAct.Thumb) Then

                        'Was it XBMC or MIP that set some of the actor thumbs to
                        'the default "Add Pic" image??
                        If Not Strings.InStr(imdbAct.Thumb.ToLower, "addtiny.gif") > 0 Then
                            Me.alActors.Add(imdbAct.Thumb)
                        Else
                            Me.alActors.Add("none")
                        End If
                    Else
                        Me.alActors.Add("none")
                    End If

                    Me.lstActors.Items.Add(String.Format("{0} as {1}", imdbAct.Name, imdbAct.Role))
                Next
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.MPAA) Then
                Dim tmpRatingImg As Image = Master.GetRatingImage(Master.currMovie.Movie.MPAA)
                If Not IsNothing(tmpRatingImg) Then
                    Me.pbMPAA.Image = tmpRatingImg
                    Me.MoveMPAA()
                    Me.pnlMPAA.Visible = True
                Else
                    Me.pnlMPAA.Visible = False
                End If
            End If

            Dim tmpRating As Single = Master.ConvertToSingle(Master.currMovie.Movie.Rating)
            If tmpRating > 0 Then Me.BuildStars(tmpRating)

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Genre) Then
                Me.createGenreThumbs(Master.currMovie.Movie.Genre)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Studio) Then
                Me.pbStudio.Image = Master.GetStudioImage(Master.currMovie.Movie.Studio)
            Else
                Me.pbStudio.Image = Master.GetStudioImage("####")
            End If

            If Master.eSettings.ScanMediaInfo Then
                Master.GetAVImages(Master.currMovie)
                Me.pnlInfoIcons.Width = 390
                Me.pbStudio.Left = 325
            Else
                Me.pnlInfoIcons.Width = 65
                Me.pbStudio.Left = 0
            End If

            Me.lblDirector.Text = Master.currMovie.Movie.Director

            Me.txtIMDBID.Text = Master.currMovie.Movie.IMDBID

            Me.txtFilePath.Text = Master.currMovie.FaS.Filename

            Me.lblReleaseDate.Text = Master.currMovie.Movie.ReleaseDate
            Me.txtCerts.Text = Master.currMovie.Movie.Certification

            Me.txtMediaInfo.Text = Master.FIToString(Master.currMovie.Movie.FileInfo)

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.ResumeLayout()
    End Sub

    Private Sub BuildStars(ByVal sinRating As Single)

        '//
        ' Convert # rating to star images
        '\\

        Try
            With Me
                .pbStar1.Image = Nothing
                .pbStar2.Image = Nothing
                .pbStar3.Image = Nothing
                .pbStar4.Image = Nothing
                .pbStar5.Image = Nothing

                If sinRating >= 0.5 Then ' if rating is less than .5 out of ten, consider it a 0
                    Select Case (sinRating / 2)
                        Case Is <= 0.5
                            .pbStar1.Image = My.Resources.starhalf
                        Case Is <= 1
                            .pbStar1.Image = My.Resources.star
                        Case Is <= 1.5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.starhalf
                        Case Is <= 2
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                        Case Is <= 2.5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.starhalf
                        Case Is <= 3
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                        Case Is <= 3.5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.starhalf
                        Case Is <= 4
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                        Case Is <= 4.5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                            .pbStar5.Image = My.Resources.starhalf
                        Case Is <= 5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                            .pbStar5.Image = My.Resources.star
                    End Select
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub createGenreThumbs(ByVal strGenres As String)

        '//
        ' Parse the genre string and create panels/pictureboxes for each one
        '\\

        Dim genreArray() As String

        Try
            genreArray = Strings.Split(strGenres, " / ")
            For i As Integer = 0 To UBound(genreArray)
                ReDim Preserve Me.pnlGenre(i)
                ReDim Preserve Me.pbGenre(i)
                Me.pnlGenre(i) = New Panel()
                Me.pbGenre(i) = New PictureBox()
                Me.pbGenre(i).Name = genreArray(i).Trim.ToUpper
                Me.pnlGenre(i).Size = New Size(68, 100)
                Me.pbGenre(i).Size = New Size(62, 94)
                Me.pnlGenre(i).BackColor = Color.Gainsboro
                Me.pnlGenre(i).BorderStyle = BorderStyle.FixedSingle
                Me.pbGenre(i).SizeMode = PictureBoxSizeMode.StretchImage
                Me.pbGenre(i).Image = Master.GetGenreImage(genreArray(i).Trim)
                Me.pnlGenre(i).Left = ((Me.pnlInfoPanel.Right) - (i * 73)) - 73
                Me.pbGenre(i).Left = 2
                Me.pnlGenre(i).Top = Me.pnlInfoPanel.Top - 105
                Me.pbGenre(i).Top = 2
                Me.scMain.Panel2.Controls.Add(Me.pnlGenre(i))
                Me.pnlGenre(i).Controls.Add(Me.pbGenre(i))
                Me.pnlGenre(i).BringToFront()
                AddHandler Me.pbGenre(i).MouseEnter, AddressOf pbGenre_MouseEnter
                AddHandler Me.pbGenre(i).MouseLeave, AddressOf pbGenre_MouseLeave
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ScrapeData(ByVal sType As Master.ScrapeType, ByVal sMod As Master.ScrapeModifier, ByVal Options As Master.ScrapeOptions, Optional ByVal ID As Integer = 0, Optional ByVal doSearch As Boolean = False)

        Try
            Dim chkCount As Integer = 0
            Dim nfoPath As String = String.Empty

            If Not isCL Then
                Me.tslStatus.Text = String.Empty 'clear status for scrapers that do not report
                Me.ToolsToolStripMenuItem.Enabled = False
                Me.tsbAutoPilot.Enabled = False
                Me.tsbRefreshMedia.Enabled = False
                Me.mnuMediaList.Enabled = False
                Me.tabsMain.Enabled = False
                Me.tspbLoading.Style = ProgressBarStyle.Continuous
                Me.EnableFilters(False)
                Me.EnableSorting(False)

                If Not sType = Master.ScrapeType.SingleScrape Then
                    Select Case sType
                        Case Master.ScrapeType.CleanFolders
                            lblCanceling.Text = "Canceling File Cleaner..."
                            btnCancel.Text = "Cancel Cleaner"
                        Case Master.ScrapeType.CopyBD
                            lblCanceling.Text = "Canceling Backdrop Copy..."
                            btnCancel.Text = "Cancel Copy"
                        Case Master.ScrapeType.RevertStudios
                            lblCanceling.Text = "Canceling Reversion..."
                            btnCancel.Text = "Cancel Reversion"
                        Case Else
                            lblCanceling.Text = "Canceling Scraper..."
                            btnCancel.Text = "Cancel Scraper"
                    End Select
                    btnCancel.Visible = True
                    lblCanceling.Visible = False
                    pbCanceling.Visible = False
                    Me.pnlCancel.Visible = True
                End If
            End If

            Select Case sType
                Case Master.ScrapeType.FullAsk, Master.ScrapeType.FullAuto, Master.ScrapeType.CleanFolders, Master.ScrapeType.CopyBD, Master.ScrapeType.RevertStudios, Master.ScrapeType.UpdateAsk, Master.ScrapeType.UpdateAuto
                    Me.tspbLoading.Maximum = Me.dtMedia.Rows.Count
                    Select Case sType
                        Case Master.ScrapeType.FullAsk
                            Me.tslLoading.Text = "Updating Media (All Movies - Ask):"
                        Case Master.ScrapeType.FullAuto
                            Me.tslLoading.Text = "Updating Media (All Movies - Auto):"
                        Case Master.ScrapeType.CleanFolders
                            Me.tslLoading.Text = "Cleaning Files:"
                        Case Master.ScrapeType.CopyBD
                            Me.tslLoading.Text = "Copying Fanart to Backdrops Folder:"
                        Case Master.ScrapeType.RevertStudios
                            Me.tslLoading.Text = "Reverting Media Info Studio Tags:"
                        Case Master.ScrapeType.UpdateAuto
                            Me.tslLoading.Text = "Updating Media (Movies Missing Items - Auto):"
                        Case Master.ScrapeType.UpdateAsk
                            Me.tslLoading.Text = "Updating Media (Movies Missing Items - Ask):"
                    End Select
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .scrapeMod = sMod, .Options = Options})
                    End If

                Case Master.ScrapeType.NewAsk, Master.ScrapeType.NewAuto, Master.ScrapeType.MarkAsk, Master.ScrapeType.MarkAuto
                    For Each drvRow As DataRow In Me.dtMedia.Rows
                        If drvRow.Item(10) AndAlso (sType = Master.ScrapeType.NewAsk OrElse sType = Master.ScrapeType.NewAuto) Then
                            chkCount += 1
                        End If
                        If drvRow.Item(11) AndAlso (sType = Master.ScrapeType.MarkAsk OrElse sType = Master.ScrapeType.MarkAuto) Then
                            chkCount += 1
                        End If
                    Next

                    If chkCount > 0 Then
                        Select Case sType
                            Case Master.ScrapeType.NewAsk
                                Me.tslLoading.Text = "Updating Media (New Movies - Ask):"
                            Case Master.ScrapeType.NewAuto
                                Me.tslLoading.Text = "Updating Media (New Movies - Auto):"
                            Case Master.ScrapeType.MarkAsk
                                Me.btnMarkAll.Enabled = False
                                Me.tslLoading.Text = "Updating Media (Marked Movies - Ask):"
                            Case Master.ScrapeType.MarkAuto
                                Me.btnMarkAll.Enabled = False
                                Me.tslLoading.Text = "Updating Media (Marked Movies - Auto):"
                        End Select
                        Me.tspbLoading.Maximum = chkCount
                        Me.tslLoading.Visible = True
                        Me.tspbLoading.Visible = True

                        If Not bwScraper.IsBusy Then
                            bwScraper.WorkerSupportsCancellation = True
                            bwScraper.WorkerReportsProgress = True
                            bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .scrapeMod = sMod, .Options = Options})
                        End If
                    Else
                        If isCL Then
                            Me.ScraperDone = True
                        Else
                            Me.tslLoading.Visible = False
                            Me.tspbLoading.Visible = False
                            Me.tslStatus.Text = String.Empty
                            Me.ToolsToolStripMenuItem.Enabled = True
                            Me.tsbAutoPilot.Enabled = True
                            Me.tsbRefreshMedia.Enabled = True
                            Me.mnuMediaList.Enabled = True
                            Me.tabsMain.Enabled = True
                            Me.EnableFilters(True)
                        End If
                    End If
                Case Master.ScrapeType.SingleScrape
                    Me.ClearInfo()
                    Me.tslStatus.Text = String.Format("Re-scraping {0}", Master.currMovie.Movie.Title)
                    Me.tslLoading.Text = "Scraping:"
                    Me.tspbLoading.Maximum = 13
                    Me.ReportDownloadPercent = True
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True
                    Me.dgvMediaList.Enabled = False


                    If isCL AndAlso doSearch = False Then
                        Dim Poster As New Images
                        Dim Fanart As New Images
                        Dim fArt As New Media.Fanart
                        Dim pThumbs As New Media.Poster

                        Try
                            If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) Then
                                IMDB.GetMovieInfo(Master.tmpMovie.IMDBID, Master.currMovie.Movie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False, Master.DefaultOptions)
                            Else
                                Master.currMovie.Movie = IMDB.GetSearchMovieInfo(Me.tmpTitle, Master.tmpMovie, Master.ScrapeType.SingleScrape, Master.DefaultOptions)
                            End If

                            If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) Then
                                If Master.eSettings.ScanMediaInfo Then
                                    UpdateMediaInfo(Master.currMovie)
                                End If

                                If Poster.IsAllowedToDownload(Master.currMovie, Master.ImageType.Posters) Then
                                    If Poster.GetPreferredImage(Master.currMovie.Movie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs) Then
                                        If Not IsNothing(Poster.Image) Then
                                            Poster.SaveAsPoster(Master.currMovie)
                                            Master.currMovie.Movie.Thumbs = pThumbs
                                        End If
                                    End If
                                End If
                                pThumbs = Nothing

                                If Fanart.IsAllowedToDownload(Master.currMovie, Master.ImageType.Fanart) Then
                                    If Fanart.GetPreferredImage(Master.currMovie.Movie.IMDBID, Master.ImageType.Fanart, fArt, Nothing) Then
                                        If Not IsNothing(Fanart.Image) Then
                                            Fanart.SaveAsFanart(Master.currMovie)
                                            Master.currMovie.Movie.Fanart = fArt
                                        End If
                                    End If
                                End If
                                fArt = Nothing

                                Master.SaveMovieToNFO(Master.currMovie)
                            End If

                            If Master.eSettings.AutoThumbs > 0 AndAlso Master.currMovie.FaS.isSingle Then
                                Master.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs)
                            End If
                        Catch
                        End Try
                        Me.ScraperDone = True
                    Else
                        If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) AndAlso doSearch = False Then
                            IMDB.GetMovieInfoAsync(Master.currMovie.Movie.IMDBID, Master.currMovie.Movie, Master.DefaultOptions, Master.eSettings.FullCrew, Master.eSettings.FullCast)
                        Else
                            Master.tmpMovie = New Media.Movie

                            Using dSearch As New dlgIMDBSearchResults
                                If dSearch.ShowDialog(Me.tmpTitle) = Windows.Forms.DialogResult.OK Then
                                    If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                                        Me.ClearInfo()
                                        Me.tslStatus.Text = String.Format("Scraping {0}", Master.tmpMovie.Title)
                                        Me.tslLoading.Text = "Scraping:"
                                        Me.tspbLoading.Maximum = 13
                                        Me.tspbLoading.Style = ProgressBarStyle.Continuous
                                        Me.ReportDownloadPercent = True
                                        Me.tslLoading.Visible = True
                                        Me.tspbLoading.Visible = True
                                        IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.currMovie.Movie, Master.DefaultOptions, Master.eSettings.FullCrew, Master.eSettings.FullCast)
                                    End If
                                Else
                                    If isCL Then
                                        Me.ScraperDone = True
                                    Else
                                        Me.dgvMediaList.Enabled = True
                                        Me.tslLoading.Visible = False
                                        Me.tspbLoading.Visible = False
                                        Me.tslStatus.Text = String.Empty
                                        Me.ToolsToolStripMenuItem.Enabled = True
                                        Me.tsbAutoPilot.Enabled = True
                                        Me.tsbRefreshMedia.Enabled = True
                                        Me.mnuMediaList.Enabled = True
                                        Me.tabsMain.Enabled = True
                                        Me.EnableFilters(True)
                                        Me.EnableSorting(True)
                                        Me.LoadInfo(ID, Master.currMovie.FaS.Filename, True, False)
                                    End If
                                End If
                            End Using
                        End If
                    End If
            End Select
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub UpdateMediaInfo(ByRef miMovie As Master.DBMovie)
        Try

            Dim pExt As String = Path.GetExtension(miMovie.FaS.Filename).ToLower
            If Not pExt = ".rar" Then
                Dim MI As New MediaInfo
                MI.GetMovieMIFromPath(miMovie.Movie.FileInfo, miMovie.FaS.Filename)
                If Master.eSettings.UseMIDuration AndAlso miMovie.Movie.FileInfo.StreamDetails.Video.Count > 0 Then
                    Dim tVid As MediaInfo.Video = Master.GetBestVideo(miMovie.Movie.FileInfo)

                    If Not String.IsNullOrEmpty(tVid.Duration) Then
                        Dim sDuration As Match = Regex.Match(tVid.Duration, "(([0-9]+)h)?\s?(([0-9]+)mn)?")
                        Dim sHour As Integer = If(Not String.IsNullOrEmpty(sDuration.Groups(2).Value), (Convert.ToInt32(sDuration.Groups(2).Value)), 0)
                        Dim sMin As Integer = If(Not String.IsNullOrEmpty(sDuration.Groups(4).Value), (Convert.ToInt32(sDuration.Groups(4).Value)), 0)
                        miMovie.Movie.Runtime = If(Master.eSettings.UseHMForRuntime, String.Format("{0} hrs {1} mins", sHour, sMin), String.Format("{0} mins", (sHour * 60) + sMin))
                    End If

                End If
                MI = Nothing
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub MovieInfoDownloadedPercent(ByVal iPercent As Integer)
        If Me.ReportDownloadPercent = True Then
            Me.tspbLoading.Value = iPercent
            Me.Refresh()
        End If
    End Sub

    Private Sub MovieInfoDownloaded(ByVal bSuccess As Boolean)

        Try
            If bSuccess Then
                If Master.eSettings.ScanMediaInfo Then
                    Me.tslLoading.Text = "Scanning Media Info:"
                    Me.tspbLoading.Value = Me.tspbLoading.Maximum
                    Me.tspbLoading.Style = ProgressBarStyle.Marquee
                    Me.tspbLoading.MarqueeAnimationSpeed = 25
                    Me.Refresh()
                    Me.UpdateMediaInfo(Master.currMovie)
                End If
                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    If tmpImages.IsAllowedToDownload(Master.currMovie, Master.ImageType.Posters, True) Then
                        Using dImgSelect As New dlgImgSelect
                            Dim pPath As String = dImgSelect.ShowDialog(Master.currMovie, Master.ImageType.Posters)
                            If Not String.IsNullOrEmpty(pPath) Then
                                Master.currMovie.FaS.Poster = pPath
                            End If
                        End Using
                    End If
                    If tmpImages.IsAllowedToDownload(Master.currMovie, Master.ImageType.Fanart, True) Then
                        Using dImgSelect As New dlgImgSelect
                            Dim fPath As String = dImgSelect.ShowDialog(Master.currMovie, Master.ImageType.Fanart)
                            If Not String.IsNullOrEmpty(fPath) Then
                                Master.currMovie.FaS.Fanart = fPath
                            End If
                        End Using
                    End If
                    tmpImages.Dispose()
                    tmpImages = Nothing

                    If Master.eSettings.SingleScrapeTrailer Then
                        Dim cTrailer As New Trailers
                        Dim tURL As String = cTrailer.ShowTDialog(Master.currMovie.Movie.IMDBID, Master.currMovie.FaS.Filename, Master.currMovie.Movie.Trailer)
                        If Not String.IsNullOrEmpty(tURL) AndAlso tURL.Substring(0, 7) = "http://" Then
                            Master.currMovie.Movie.Trailer = tURL
                        End If
                        cTrailer = Nothing
                    End If

                    If Master.eSettings.AutoThumbs > 0 AndAlso Master.currMovie.FaS.isSingle Then
                        Master.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs)
                    End If
                End If
                If Not isCL Then
                    Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                    Dim ID As Integer = Me.dgvMediaList.Rows(indX).Cells(0).Value

                    Using dEditMovie As New dlgEditMovie
                        Select Case dEditMovie.ShowDialog()
                            Case Windows.Forms.DialogResult.OK
                                Me.RefreshMovie(ID)
                                Me.SetListItemAfterEdit(ID, indX)
                            Case Windows.Forms.DialogResult.Retry
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID)
                            Case Windows.Forms.DialogResult.Abort
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID, True)
                        End Select
                    End Using
                Else
                    Master.SaveMovieToNFO(Master.currMovie)
                End If
            Else
                MsgBox("Unable to retrieve movie details from the internet. Please check your connection and try again.", MsgBoxStyle.Exclamation, "Error Retrieving Details")
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If isCL Then
            Me.ScraperDone = True
        Else
            Me.dgvMediaList.Enabled = True
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.tslStatus.Text = String.Empty
            Me.ToolsToolStripMenuItem.Enabled = True
            Me.tsbAutoPilot.Enabled = True
            Me.tsbRefreshMedia.Enabled = True
            Me.mnuMediaList.Enabled = True
            Me.tabsMain.Enabled = True
        End If
    End Sub

    Private Function RefreshMovie(ByVal ID As Integer, Optional ByVal BatchMode As Boolean = False, Optional ByVal DoNfo As Boolean = True) As Boolean
        Dim dRow = From drvRow As DataRow In dtMedia.Rows Where drvRow.Item(0) = ID Select drvRow
        Dim sPath As String = dRow(0).Item(1)
        Dim aContents(6) As String
        Dim tmpMovie As New Media.Movie
        Dim tmpMovieDb As New Master.DBMovie
        Try

            If Directory.Exists(Directory.GetParent(dRow(0).Item(1)).FullName) Then
                If DoNfo Then
                    If String.IsNullOrEmpty(dRow(0).Item(40)) Then
                        tmpMovie = Master.LoadMovieFromNFO(dRow(0).Item(1), dRow(0).Item(2))
                    Else
                        tmpMovie = Master.LoadMovieFromNFO(dRow(0).Item(40), dRow(0).Item(2))
                    End If

                    If String.IsNullOrEmpty(tmpMovie.Title) Then
                        tmpMovieDb.ListTitle = dRow(0).Item(3)
                    Else
                        dRow(0).Item(3) = tmpMovie.Title
                        tmpMovieDb.ListTitle = tmpMovie.Title
                    End If

                    tmpMovieDb.Movie = tmpMovie
                Else
                    tmpMovieDb = Master.DB.LoadMovieFromDB(ID)
                End If

                    tmpMovieDb.FaS = New Master.FileAndSource
                    tmpMovieDb.FaS.Filename = dRow(0).Item(1)
                    tmpMovieDb.FaS.isSingle = dRow(0).Item(2)
                    tmpMovieDb.FaS.Source = dRow(0).Item(12)
                    aContents = Master.GetFolderContents(dRow(0).Item(1))
                    tmpMovieDb.FaS.Poster = aContents(0)
                    dRow(0).Item(4) = If(String.IsNullOrEmpty(aContents(0)), False, True)
                    tmpMovieDb.FaS.Fanart = aContents(1)
                    dRow(0).Item(5) = If(String.IsNullOrEmpty(aContents(1)), False, True)
                    tmpMovieDb.FaS.Nfo = aContents(2)
                    dRow(0).Item(6) = If(String.IsNullOrEmpty(aContents(2)), False, True)
                    tmpMovieDb.FaS.Trailer = aContents(3)
                    dRow(0).Item(7) = If(String.IsNullOrEmpty(aContents(3)), False, True)
                    tmpMovieDb.FaS.Subs = aContents(4)
                    dRow(0).Item(8) = If(String.IsNullOrEmpty(aContents(4)), False, True)
                    tmpMovieDb.FaS.Extra = aContents(5)
                    dRow(0).Item(9) = If(String.IsNullOrEmpty(aContents(5)), False, True)

                    tmpMovieDb.ID = dRow(0).Item(0)
                    tmpMovieDb.IsMark = dRow(0).Item(11)
                    tmpMovieDb.IsLock = dRow(0).Item(14)

                    Master.DB.SaveMovieToDB(tmpMovieDb, False, BatchMode)

                    aContents = Nothing
                Else
                    Master.DB.DeleteFromDB(dRow(0).Item(0), BatchMode)
                    Return True
                End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return False
    End Function

    Private Sub SetMenus(ByVal ReloadFilters As Boolean)

        Try
            With Master.eSettings
                If (Not .ExpertCleaner AndAlso (.CleanDotFanartJPG OrElse .CleanFanartJPG OrElse .CleanFolderJPG OrElse .CleanMovieFanartJPG OrElse _
                .CleanMovieJPG OrElse .CleanMovieNameJPG OrElse .CleanMovieNFO OrElse .CleanMovieNFOB OrElse _
                .CleanMovieTBN OrElse .CleanMovieTBNB OrElse .CleanPosterJPG OrElse .CleanPosterTBN OrElse .CleanExtraThumbs)) OrElse _
                (.ExpertCleaner AndAlso (.CleanWhitelistVideo OrElse .CleanWhitelistExts.Count > 0)) Then
                    Me.CleanFoldersToolStripMenuItem.Enabled = True
                Else
                    Me.CleanFoldersToolStripMenuItem.Enabled = False
                End If

                If .XBMCComs.Count > 0 Then
                    Me.tsbUpdateXBMC.Enabled = True
                    tsbUpdateXBMC.DropDownItems.Clear()
                    For Each xCom As emmSettings.XBMCCom In .XBMCComs
                        tsbUpdateXBMC.DropDownItems.Add(String.Concat("Update ", xCom.Name, " Only"), Nothing, New System.EventHandler(AddressOf XComSubClick))
                    Next
                Else
                    Me.tsbUpdateXBMC.Enabled = False
                End If

                Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Enabled = Directory.Exists(.BDPath)

                Me.ClearAllCachesToolStripMenuItem.Enabled = .UseImgCache

                Me.mnuAllAutoExtra.Enabled = .AutoThumbs > 0
                Me.mnuAllAskExtra.Enabled = .AutoThumbs > 0
                Me.mnuMissAutoExtra.Enabled = .AutoThumbs > 0
                Me.mnuMissAskExtra.Enabled = .AutoThumbs > 0
                Me.mnuMarkAutoExtra.Enabled = .AutoThumbs > 0
                Me.mnuMarkAskExtra.Enabled = .AutoThumbs > 0
                Me.mnuNewAutoExtra.Enabled = .AutoThumbs > 0
                Me.mnuNewAskExtra.Enabled = .AutoThumbs > 0
                Me.mnuMarkAutoExtra.Enabled = .AutoThumbs > 0

                Me.mnuAllAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuAllAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMissAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMissAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMarkAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMarkAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuNewAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuNewAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB

                Me.mnuAllAutoFanart.Enabled = .UseTMDB
                Me.mnuAllAskFanart.Enabled = .UseTMDB
                Me.mnuMissAutoFanart.Enabled = .UseTMDB
                Me.mnuMissAskFanart.Enabled = .UseTMDB
                Me.mnuMarkAutoFanart.Enabled = .UseTMDB
                Me.mnuMarkAskFanart.Enabled = .UseTMDB
                Me.mnuNewAutoFanart.Enabled = .UseTMDB
                Me.mnuNewAskFanart.Enabled = .UseTMDB

                Me.mnuAllAskMI.Enabled = .ScanMediaInfo
                Me.mnuAllAutoMI.Enabled = .ScanMediaInfo
                Me.mnuNewAskMI.Enabled = .ScanMediaInfo
                Me.mnuNewAutoMI.Enabled = .ScanMediaInfo
                Me.mnuMarkAskMI.Enabled = .ScanMediaInfo
                Me.mnuMarkAutoMI.Enabled = .ScanMediaInfo

                Me.mnuAllAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuAllAskTrailer.Enabled = .DownloadTrailers
                Me.mnuMissAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuMissAskTrailer.Enabled = .DownloadTrailers
                Me.mnuNewAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuNewAskTrailer.Enabled = .DownloadTrailers
                Me.mnuMarkAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuMarkAskTrailer.Enabled = .DownloadTrailers

                Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLNewcommand.CommandText = String.Concat("SELECT COUNT(id) AS mcount FROM movies WHERE mark = 1;")
                    Using SQLcount As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                        If SQLcount("mcount") > 0 Then
                            Me.btnMarkAll.Text = "Unmark All"
                        Else
                            Me.btnMarkAll.Text = "Mark All"
                        End If
                    End Using
                End Using

                'not technically a menu, but it's a good place to put it
                If ReloadFilters Then
                    RemoveHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged
                    cbFilterSource.Items.Clear()
                    cbFilterSource.Items.Add("All")
                    Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                        SQLNewcommand.CommandText = String.Concat("SELECT Name FROM Sources;")
                        Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                            While SQLReader.Read
                                cbFilterSource.Items.Add(SQLReader("Name"))
                            End While
                        End Using
                    End Using
                    cbFilterSource.Text = "All"
                    AddHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged
                End If
            End With

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SetFilterColors()
        For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
            If drvRow.Cells(11).Value Then
                drvRow.Cells(3).Style.ForeColor = Color.Crimson
                drvRow.Cells(3).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                drvRow.Cells(3).Style.SelectionForeColor = Color.Crimson
            ElseIf drvRow.Cells(10).Value Then
                drvRow.Cells(3).Style.ForeColor = Color.Green
                drvRow.Cells(3).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                drvRow.Cells(3).Style.SelectionForeColor = Color.Green
            Else
                drvRow.Cells(3).Style.ForeColor = Color.Black
                drvRow.Cells(3).Style.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
                drvRow.Cells(3).Style.SelectionForeColor = Color.FromKnownColor(KnownColor.HighlightText)
            End If

            If drvRow.Cells(14).Value Then
                drvRow.Cells(3).Style.BackColor = Color.MistyRose
                drvRow.Cells(4).Style.BackColor = Color.MistyRose
                drvRow.Cells(5).Style.BackColor = Color.MistyRose
                drvRow.Cells(6).Style.BackColor = Color.MistyRose
                drvRow.Cells(7).Style.BackColor = Color.MistyRose
                drvRow.Cells(8).Style.BackColor = Color.MistyRose
                drvRow.Cells(9).Style.BackColor = Color.MistyRose
                drvRow.Cells(3).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(4).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(5).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(6).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(7).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(8).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(9).Style.SelectionBackColor = Color.DarkMagenta
            Else
                drvRow.Cells(3).Style.BackColor = Color.White
                drvRow.Cells(4).Style.BackColor = Color.White
                drvRow.Cells(5).Style.BackColor = Color.White
                drvRow.Cells(6).Style.BackColor = Color.White
                drvRow.Cells(7).Style.BackColor = Color.White
                drvRow.Cells(8).Style.BackColor = Color.White
                drvRow.Cells(9).Style.BackColor = Color.White
                drvRow.Cells(3).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(4).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(5).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(6).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(7).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(8).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(9).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
            End If
        Next
    End Sub

    Private Sub EnableFilters(ByVal isEnabled As Boolean)
        If isEnabled Then
            If Me.chkFilterDupe.Checked Then
                Me.chkFilterDupe.Enabled = True
                Me.chkFilterMark.Enabled = False
                Me.chkFilterNew.Enabled = False
                Me.rbFilterOr.Enabled = False
                Me.rbFilterAnd.Enabled = False
                Me.cbFilterSource.Enabled = False
            ElseIf Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked OrElse Not Me.cbFilterSource.Text = "All" Then
                Me.chkFilterDupe.Enabled = False
                Me.chkFilterMark.Enabled = True
                Me.chkFilterNew.Enabled = True
                Me.rbFilterOr.Enabled = True
                Me.rbFilterAnd.Enabled = True
                Me.cbFilterSource.Enabled = True
            Else
                Me.chkFilterDupe.Enabled = True
                Me.chkFilterMark.Enabled = True
                Me.chkFilterNew.Enabled = True
                Me.rbFilterOr.Enabled = True
                Me.rbFilterAnd.Enabled = True
                Me.cbFilterSource.Enabled = True
            End If
        Else
            Me.chkFilterDupe.Enabled = False
            Me.chkFilterMark.Enabled = False
            Me.chkFilterNew.Enabled = False
            Me.rbFilterOr.Enabled = False
            Me.rbFilterAnd.Enabled = False
            Me.cbFilterSource.Enabled = False
        End If
    End Sub

    Private Sub ClearFilters()
        Me.chkFilterDupe.Enabled = True
        Me.chkFilterDupe.Checked = False
        Me.chkFilterMark.Enabled = True
        Me.chkFilterMark.Checked = False
        Me.chkFilterNew.Enabled = True
        Me.chkFilterNew.Checked = False
        Me.rbFilterOr.Enabled = True
        Me.rbFilterOr.Checked = False
        Me.rbFilterAnd.Enabled = True
        Me.rbFilterAnd.Checked = True
        Me.cbFilterSource.Enabled = True
        RemoveHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged
        Me.cbFilterSource.Text = "All"
        AddHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged

    End Sub

    Private Sub RunFilter()
        If Me.Visible Then

            If Me.chkFilterDupe.Checked Then
                Me.FillList(0, True)
            Else
                If FilterArray.Count > 0 Then
                    Dim FilterString As String = String.Empty

                    If rbFilterAnd.Checked Then
                        FilterString = Strings.Join(FilterArray.ToArray, " AND ")
                    Else
                        FilterString = Strings.Join(FilterArray.ToArray, " OR ")
                    End If

                    bsMedia.Filter = FilterString
                Else
                    bsMedia.RemoveFilter()
                End If
                Me.FillList(0)
            End If
        End If
    End Sub

    Private Sub XComSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim xComName As String = sender.ToString.Replace("Update ", String.Empty).Replace(" Only", String.Empty).Trim
        Dim xCom = From x As emmSettings.XBMCCom In Master.eSettings.XBMCComs Where x.Name = xComName
        If xCom.Count > 0 Then
            DoXCom(xCom(0))
        End If
    End Sub

    Private Sub DoXCom(ByVal xCom As emmSettings.XBMCCom)
        Try
            Dim Wr As HttpWebRequest = HttpWebRequest.Create(String.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?command=ExecBuiltIn&parameter=XBMC.updatelibrary(video)", xCom.IP, xCom.Port))
            Wr.Method = "GET"
            Wr.Timeout = 2500
            If Not String.IsNullOrEmpty(xCom.Username) AndAlso Not String.IsNullOrEmpty(xCom.Password) Then
                Wr.Credentials = New NetworkCredential(xCom.Username, xCom.Password)
            End If
            Using Wres As HttpWebResponse = Wr.GetResponse
                Dim Sr As String = New StreamReader(Wres.GetResponseStream()).ReadToEnd
                If Not Sr.Contains("OK") Then
                    MsgBox(String.Concat("There was a problem communicating with ", xCom.Name, vbNewLine, "Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), MsgBoxStyle.Exclamation, String.Concat("Unable to Start XBMC Update for ", xCom.Name))
                End If
            End Using
            Wr = Nothing
        Catch
            MsgBox(String.Concat("There was a problem communicating with ", xCom.Name, vbNewLine, "Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), MsgBoxStyle.Exclamation, String.Concat("Unable to Start XBMC Update for ", xCom.Name))
        End Try
    End Sub

    Private Sub SaveMovieList()
        Try
            Me.ClearFilters()
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLcommand.CommandText = "UPDATE movies SET new = (?);"
                    Dim parNew As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNew", DbType.Boolean, 0, "new")
                    parNew.Value = False
                    SQLcommand.ExecuteNonQuery()
                End Using
                SQLtransaction.Commit()
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "VACUUM;"
                SQLcommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FillList(ByVal iIndex As Integer, Optional ByVal DupesOnly As Boolean = False)
        Try
            Me.bsMedia.DataSource = Nothing
            Me.dgvMediaList.DataSource = Nothing
            Me.ClearInfo()
            Application.DoEvents()

            If DupesOnly Then
                Master.DB.FillDataTable(Me.dtMedia, "SELECT * FROM movies WHERE imdb IN (SELECT imdb FROM movies WHERE imdb IS NOT NULL AND LENGTH(imdb) > 0 GROUP BY imdb HAVING ( COUNT(imdb) > 1 )) ORDER BY ListTitle COLLATE NOCASE;")
            Else
                Master.DB.FillDataTable(Me.dtMedia, "SELECT * FROM movies ORDER BY ListTitle COLLATE NOCASE;")
            End If

            If isCL Then
                Me.LoadingDone = True
            Else
                If Me.dtMedia.Rows.Count > 0 Then

                    With Me
                        .bsMedia.DataSource = .dtMedia
                        .dgvMediaList.DataSource = .bsMedia

                        'why did the resizable property all the sudden become opposite? resizable = false now means it IS resizable
                        'wasn't like that before and was reported (after release of v alpha 022, but no telling how long it's been
                        'like that) that the info columns were resizable
                        .dgvMediaList.Columns(0).Visible = False
                        .dgvMediaList.Columns(1).Visible = False
                        .dgvMediaList.Columns(2).Visible = False
                        .dgvMediaList.Columns(3).Resizable = False
                        .dgvMediaList.Columns(3).ReadOnly = True
                        .dgvMediaList.Columns(3).MinimumWidth = 83
                        .dgvMediaList.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(3).ToolTipText = "Title"
                        .dgvMediaList.Columns(3).HeaderText = "Title"
                        .dgvMediaList.Columns(4).Width = 20
                        .dgvMediaList.Columns(4).Resizable = True
                        .dgvMediaList.Columns(4).ReadOnly = True
                        .dgvMediaList.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                        .dgvMediaList.Columns(4).ToolTipText = "Poster"
                        .dgvMediaList.Columns(5).Width = 20
                        .dgvMediaList.Columns(5).Resizable = True
                        .dgvMediaList.Columns(5).ReadOnly = True
                        .dgvMediaList.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieFanartCol
                        .dgvMediaList.Columns(5).ToolTipText = "Fanart"
                        .dgvMediaList.Columns(6).Width = 20
                        .dgvMediaList.Columns(6).Resizable = True
                        .dgvMediaList.Columns(6).ReadOnly = True
                        .dgvMediaList.Columns(6).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(6).Visible = Not Master.eSettings.MovieInfoCol
                        .dgvMediaList.Columns(6).ToolTipText = "Nfo"
                        .dgvMediaList.Columns(7).Width = 20
                        .dgvMediaList.Columns(7).Resizable = True
                        .dgvMediaList.Columns(7).ReadOnly = True
                        .dgvMediaList.Columns(7).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(7).Visible = Not Master.eSettings.MovieTrailerCol
                        .dgvMediaList.Columns(7).ToolTipText = "Trailer"
                        .dgvMediaList.Columns(8).Width = 20
                        .dgvMediaList.Columns(8).Resizable = True
                        .dgvMediaList.Columns(8).ReadOnly = True
                        .dgvMediaList.Columns(8).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(8).Visible = Not Master.eSettings.MovieSubCol
                        .dgvMediaList.Columns(8).ToolTipText = "Subtitles"
                        .dgvMediaList.Columns(9).Width = 20
                        .dgvMediaList.Columns(9).Resizable = True
                        .dgvMediaList.Columns(9).ReadOnly = True
                        .dgvMediaList.Columns(9).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(9).Visible = Not Master.eSettings.MovieExtraCol
                        .dgvMediaList.Columns(9).ToolTipText = "Extrathumbs"
                        For i As Integer = 10 To .dgvMediaList.Columns.Count - 1
                            .dgvMediaList.Columns(i).Visible = False
                        Next

                        'Trick to autosize the first column, but still allow resizing by user
                        .dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        .dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                        .dgvMediaList.Sort(.dgvMediaList.Columns(3), ComponentModel.ListSortDirection.Ascending)

                        If .dgvMediaList.RowCount > 0 Then
                            .SetFilterColors()

                            'Set current cell and automatically load the info for the first movie in the list
                            .dgvMediaList.Rows(iIndex).Cells(3).Selected = True
                            .dgvMediaList.CurrentCell = .dgvMediaList.Rows(iIndex).Cells(3)
                        End If

                        .ToolsToolStripMenuItem.Enabled = True
                        .tsbAutoPilot.Enabled = True
                        .mnuMediaList.Enabled = True
                    End With
                Else
                    Me.ToolsToolStripMenuItem.Enabled = False
                    Me.tsbAutoPilot.Enabled = False
                    Me.mnuMediaList.Enabled = False
                    Me.tslStatus.Text = String.Empty
                    Me.ClearInfo()
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If Not isCL Then
            Me.tsbRefreshMedia.Enabled = True
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.tspbLoading.Value = 0

            Me.tabMovies.Text = String.Format("Movies ({0})", Me.dgvMediaList.RowCount)
            Me.EnableFilters(True)

        End If
    End Sub

    Public Sub SetListItemAfterEdit(ByVal iID As Integer, ByVal iRow As Integer)

        Try
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT ListTitle, mark FROM movies WHERE id = ", iID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Me.SetFilterColors()
                    Me.dgvMediaList.Item(3, iRow).Value = SQLreader("ListTitle")
                    Me.dgvMediaList.Item(11, iRow).Value = SQLreader("mark")
                End Using
            End Using

            Me.SelectRow(iRow)

            Me.SetFilterColors()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SelectRow(ByVal iRow As Integer)

        Try
            If Me.bwLoadInfo.IsBusy Then
                Me.bwLoadInfo.CancelAsync()
                Do While Me.bwLoadInfo.IsBusy
                    Application.DoEvents()
                Loop
            End If

            Me.tmpTitle = Me.dgvMediaList.Item(3, iRow).Value.ToString
            If Not Me.dgvMediaList.Item(4, iRow).Value AndAlso Not Me.dgvMediaList.Item(5, iRow).Value AndAlso Not Me.dgvMediaList.Item(6, iRow).Value Then
                Me.ClearInfo()
                Me.pnlNoInfo.Visible = True
                Master.currMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(Me.dgvMediaList.Item(0, iRow).Value))
                Me.tslStatus.Text = Master.currMovie.FaS.Filename
                Me.mnuMediaList.Enabled = True
            Else
                Me.pnlNoInfo.Visible = False

                'try to load the info from the NFO
                Me.LoadInfo(Me.dgvMediaList.Item(0, iRow).Value, Me.dgvMediaList.Item(1, iRow).Value.ToString, True, False)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetToolTips()
        Dim TT As ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.tsbAutoPilot.ToolTipText = "Scrape/download data from the internet for multiple movies."
        Me.tsbRefreshMedia.ToolTipText = "Scans sources for new content and cleans database."
        Me.tsbUpdateXBMC.ToolTipText = "Sends a command to XBMC to begin its internal ""Update Library"" process."
        TT.SetToolTip(Me.btnMarkAll, "Mark or Unmark all movies in the list.")
        TT.SetToolTip(Me.txtSearch, "Search the movie titles by entering text here.")
        TT.SetToolTip(Me.btnPlay, "Play the movie file with the system default media player.")
        TT.SetToolTip(Me.btnMIRefresh, "Rescan and save the media info for the selected movie.")
        TT.SetToolTip(Me.chkFilterDupe, "Display only movies that have duplicate IMDB IDs.")
        TT.SetToolTip(Me.chkFilterNew, "Display only new movies.")
        TT.SetToolTip(Me.chkFilterMark, "Display only marked movies.")
        TT.SetToolTip(Me.cbFilterSource, "Display only movies from the selected source.")
        TT.Active = True

    End Sub

    Private Sub EnableSorting(ByVal Enable As Boolean)
        For Each dgvCol As DataGridViewColumn In Me.dgvMediaList.Columns
            If Enable Then
                dgvCol.SortMode = DataGridViewColumnSortMode.Automatic
            Else
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
            End If
        Next
    End Sub

    Friend Class MovieListFind

        Private _searchstring As String = String.Empty

        Public WriteOnly Property SearchString() As String
            Set(ByVal value As String)
                _searchstring = value
            End Set
        End Property

        Public Function Find(ByVal FAS As Master.FileAndSource) As Boolean
            If Not IsNothing(FAS) AndAlso FAS.Filename = _searchstring Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class


#End Region '*** Routines/Functions

End Class