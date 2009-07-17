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
    Private InfoCleared As Boolean = False

    'filters
    Private filSearch As String = String.Empty
    Private isActorSearch As Boolean = False
    Private filGenre As String = String.Empty
    Private filYear As String = String.Empty
    Private filMissing As String = String.Empty

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
        Dim SourceName As String
    End Structure

#End Region '*** Declarations



#Region "Form/Controls"

    ' ########################################
    ' ######### FORM/CONTROLS EVENTS #########
    ' ########################################

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub GenreListToolStripComboBox_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles GenreListToolStripComboBox.DropDown
        Me.GenreListToolStripComboBox.Items.Remove(Master.eLang.GetString(98, "Select Genre..."))
    End Sub

    Private Sub GenreListToolStripComboBox_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GenreListToolStripComboBox.SelectedIndexChanged
        If dgvMediaList.SelectedRows.Count > 1 Then
            RemoveGenreToolStripMenuItem.Enabled = True
            AddGenreToolStripMenuItem.Enabled = True
        Else
            RemoveGenreToolStripMenuItem.Enabled = GenreListToolStripComboBox.Tag.contains(GenreListToolStripComboBox.Text)
            AddGenreToolStripMenuItem.Enabled = Not GenreListToolStripComboBox.Tag.contains(GenreListToolStripComboBox.Text)
        End If
        SetGenreToolStripMenuItem.Enabled = True
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

            lblCanceling.Text = Master.eLang.GetString(99, "Canceling All Processes...")
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
                    Master.DeleteDirectory(Master.TempPath)
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
                ImageManip.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, (Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2)
                Me.pnlCancel.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, 100)
                Me.pnlFilterGenre.Location = New Point(Me.gbSpecific.Left + Me.txtFilterGenre.Left, (Me.pnlFilter.Top + Me.txtFilterGenre.Top + Me.gbSpecific.Top) - Me.pnlFilterGenre.Height)
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

                    Me.SetUp()

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


        If Args.Count > 1 Then
            Dim MoviePath As String = String.Empty
            Dim isSingle As Boolean = False
            Dim hasSpec As Boolean = False
            Dim clScrapeType As Master.ScrapeType = Nothing
            Dim clScrapeMod As Master.ScrapeModifier = Nothing
            isCL = True
            Dim clAsk As Boolean = False
            For i As Integer = 1 To Args.Count - 1

                Select Case Args(i).ToLower
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
                        If Args.Count - 1 > i Then
                            isSingle = False
                            hasSpec = True
                            clScrapeType = Master.ScrapeType.SingleScrape
                            If File.Exists(Args(i + 1).Replace("""", String.Empty)) Then
                                MoviePath = Args(i + 1).Replace("""", String.Empty)
                                i += 1
                            End If
                        Else
                            Exit For
                        End If
                    Case "-folder"
                        If Args.Count - 1 > i Then
                            isSingle = True
                            hasSpec = True
                            clScrapeType = Master.ScrapeType.SingleScrape
                            If File.Exists(Args(i + 1).Replace("""", String.Empty)) Then
                                MoviePath = Args(i + 1).Replace("""", String.Empty)
                                i += 1
                            End If
                        Else
                            Exit For
                        End If
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
                    Case "--verbose"
                        clAsk = True
                    Case Else
                        'If File.Exists(Args(2).Replace("""", String.Empty)) Then
                        'MoviePath = Args(2).Replace("""", String.Empty)
                        'End If
                End Select
            Next
            Master.DB.Connect(False, False)
            If Not IsNothing(clScrapeType) Then
                If Not IsNothing(clScrapeMod) AndAlso Not clScrapeType = Master.ScrapeType.SingleScrape Then
                    Try
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
                            Me.tmpTitle = StringManip.FilterName(If(isSingle, Directory.GetParent(MoviePath).Name, Path.GetFileNameWithoutExtension(MoviePath)))
                            If Master.currMovie.Movie Is Nothing Then
                                Master.currMovie.Movie = New Media.Movie
                                Master.currMovie.Movie.Title = tmpTitle
                                Dim sFile As New Master.FileAndSource
                                Dim aContents() As String
                                sFile.Filename = MoviePath
                                aContents = Master.GetFolderContents(sFile.Filename, sFile.isSingle)
                                sFile.Poster = aContents(0)
                                sFile.Fanart = aContents(1)
                                sFile.Nfo = aContents(2)
                                sFile.Trailer = aContents(3)
                                sFile.Subs = aContents(4)
                                sFile.Extra = aContents(5)
                                If Not String.IsNullOrEmpty(sFile.Nfo) Then
                                    Master.currMovie.Movie = NFO.LoadMovieFromNFO(sFile.Nfo, sFile.isSingle)
                                Else
                                    Master.currMovie.Movie = NFO.LoadMovieFromNFO(sFile.Filename, sFile.isSingle)
                                End If
                                If String.IsNullOrEmpty(Master.currMovie.Movie.Title) Then
                                    'no title so assume it's an invalid nfo, clear nfo path if exists
                                    sFile.Nfo = String.Empty
                                    If Directory.GetParent(sFile.Filename).Name.ToLower = "video_ts" Then
                                        Master.currMovie.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name)
                                    Else
                                        If sFile.UseFolder AndAlso sFile.isSingle Then
                                            Master.currMovie.ListTitle = StringManip.FilterName(Directory.GetParent(sFile.Filename).Name)
                                        Else
                                            Master.currMovie.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(sFile.Filename))
                                        End If
                                    End If
                                Else
                                    If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(Master.currMovie.Movie.Year) Then
                                        Master.currMovie.ListTitle = String.Format("{0} ({1})", StringManip.FilterTokens(Master.currMovie.Movie.Title), Master.currMovie.Movie.Year)
                                    Else
                                        Master.currMovie.ListTitle = StringManip.FilterTokens(Master.currMovie.Movie.Title)
                                    End If
                                End If

                                If Not String.IsNullOrEmpty(Master.currMovie.ListTitle) Then
                                    Master.currMovie.NfoPath = sFile.Nfo
                                    Master.currMovie.PosterPath = sFile.Poster
                                    Master.currMovie.FanartPath = sFile.Fanart
                                    Master.currMovie.TrailerPath = sFile.Trailer
                                    Master.currMovie.SubPath = sFile.Subs
                                    Master.currMovie.ExtraPath = sFile.Extra
                                    Master.currMovie.Filename = sFile.Filename
                                    Master.currMovie.isSingle = sFile.isSingle
                                    Master.currMovie.UseFolder = sFile.UseFolder
                                    Master.currMovie.Source = sFile.Source
                                End If
                                Master.tmpMovie = Master.currMovie.Movie
                            End If
                            Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, Nothing, clAsk)
                        Else
                            Me.ScraperDone = True
                        End If
                    Catch ex As Exception
                        Me.ScraperDone = True
                        Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                    End Try
                End If

                Do While Not Me.ScraperDone
                    Application.DoEvents()
                Loop
            End If

            frmSplash.CloseForm()
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

                XML.CacheXMLs()

                Me.SetUp()
                Me.cbSearch.SelectedIndex = 0

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
                    Me.pnlFilter.Height = 155
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
                    Me.SetMenus(True)
                    Me.FillList(0)
                    Me.Visible = True
                Else
                    Master.DB.Connect(True, True)
                    Me.SetMenus(True)
                    If dlgWizard.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Me.Visible = True
                        Me.LoadMedia(1)
                    Else
                        Me.FillList(0)
                        Me.Visible = True
                    End If
                End If


            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Me.Activate()

        End If

    End Sub

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

    Private Sub tsbRefreshMedia_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRefreshMedia.ButtonClick

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

        Me.LoadInfo(Master.currMovie.ID, Master.currMovie.Filename, False, True, True)

    End Sub
    Private Sub dgvMediaList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellDoubleClick

        '//
        ' Show the NFO Editor
        '\\

        Try

            If Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse _
            Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy Then Return

            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Me.dgvMediaList.Item(0, indX).Value
            Master.currMovie = Master.DB.LoadMovieFromDB(ID)
            Me.tslStatus.Text = Master.currMovie.Filename
            Me.tmpTitle = Me.dgvMediaList.Item(3, indX).Value

            Using dEditMovie As New dlgEditMovie

                Select Case dEditMovie.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshMovie(ID) Then
                            Me.FillList(0)
                        End If
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID, True)
                    Case Else
                        If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value, True, False)
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
            Me.SetFilterColors(False)
        End If

    End Sub

    Private Sub pbGenre_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)

        '//
        ' Draw genre text over the image when mouse hovers
        '\\

        Try
            If Master.eSettings.AllwaysDisplayGenresText Then Return
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
            If Master.eSettings.AllwaysDisplayGenresText Then Return
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

                ImageManip.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, (Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2)
                Me.pnlCancel.Location = New Point((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2, 100)
                Me.pnlFilterGenre.Location = New Point(Me.gbSpecific.Left + Me.txtFilterGenre.Left, (Me.pnlFilter.Top + Me.txtFilterGenre.Top + Me.gbSpecific.Top) - Me.pnlFilterGenre.Height)
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

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        e.Handled = StringManip.AlphaNumericOnly(e.KeyChar, True)
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

            If Not String.IsNullOrEmpty(Me.txtSearch.Text) Then
                Me.FilterArray.Remove(Me.filSearch)
                Me.filSearch = String.Empty

                Select Case Me.cbSearch.Text
                    Case Master.eLang.GetString(21, "Title")
                        Me.filSearch = String.Concat("ListTitle LIKE '%", Me.txtSearch.Text, "%'")
                        Me.FilterArray.Add(Me.filSearch)
                        isActorSearch = False
                    Case Master.eLang.GetString(100, "Actor")
                        Me.filSearch = Me.txtSearch.Text
                        isActorSearch = True
                    Case Master.eLang.GetString(62, "Director")
                        Me.filSearch = String.Concat("Director LIKE '%", Me.txtSearch.Text, "%'")
                        Me.FilterArray.Add(Me.filSearch)
                        isActorSearch = False
                End Select

                Me.RunFilter()

            Else
                If Not String.IsNullOrEmpty(Me.filSearch) Then
                    Me.FilterArray.Remove(Me.filSearch)
                    Me.filSearch = String.Empty
                    Me.RunFilter(isActorSearch)
                    isActorSearch = False
                End If
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

        Try
            Dim sWarning As String = String.Empty
            Dim sWarningFile As String = String.Empty
            With Master.eSettings
                If .ExpertCleaner Then
                    sWarning = String.Concat(Master.eLang.GetString(102, "WARNING: If you continue, all non-whitelisted file types will be deleted!"), vbNewLine, vbNewLine, Master.eLang.GetString(101, "Are you sure you want to continue?"))
                Else
                    If .CleanDotFanartJPG Then sWarningFile += String.Concat("<movie>.fanart.jpg", vbNewLine)
                    If .CleanFanartJPG Then sWarningFile += String.Concat("fanart.jpg", vbNewLine)
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
                    sWarning = String.Concat(Master.eLang.GetString(103, "WARNING: If you continue, all files of the following types will be permanently deleted:"), vbNewLine, vbNewLine, sWarningFile, vbNewLine, Master.eLang.GetString(101, "Are you sure you want to continue?"))
                End If
            End With
            If MsgBox(sWarning, MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, Master.eLang.GetString(104, "Are you sure?")) = MsgBoxResult.Yes Then
                Me.ScrapeData(Master.ScrapeType.CleanFolders, Nothing, Nothing)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ConvertFileSourceToFolderSourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertFileSourceToFolderSourceToolStripMenuItem.Click

        Using dSortFiles As New dlgSortFiles
            If dSortFiles.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.LoadMedia(1)
        End Using
    End Sub

    Private Sub chkFilterMissing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFilterMissing.Click
        Try
            Dim MissingFilter As New ArrayList
            If Me.chkFilterMissing.Checked Then
                With Master.eSettings
                    If Not .MoviePosterCol Then MissingFilter.Add("HasPoster = 0")
                    If Not .MovieFanartCol Then MissingFilter.Add("HasFanart = 0")
                    If Not .MovieInfoCol Then MissingFilter.Add("HasNfo = 0")
                    If Not .MovieTrailerCol Then MissingFilter.Add("HasTrailer = 0")
                    If Not .MovieSubCol Then MissingFilter.Add("HasSub = 0")
                    If Not .MovieExtraCol Then MissingFilter.Add("HasExtra = 0")
                End With
                filMissing = Strings.Join(MissingFilter.ToArray, " OR ")
                Me.FilterArray.Add(filMissing)
            Else
                Me.FilterArray.Remove(filMissing)
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub chkFilterTolerance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFilterTolerance.Click
        Try
            If Me.chkFilterTolerance.Checked Then
                Me.FilterArray.Add("OutOfTolerance = 1")
            Else
                Me.FilterArray.Remove("OutOfTolerance = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub chkFilterNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFilterNew.Click
        Try
            If Me.chkFilterNew.Checked Then
                Me.FilterArray.Add("new = 1")
            Else
                Me.FilterArray.Remove("new = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub chkFilterMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterMark.Click
        Try
            If Me.chkFilterMark.Checked Then
                Me.FilterArray.Add("mark = 1")
            Else
                Me.FilterArray.Remove("mark = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub chkFilterLock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFilterLock.Click
        Try
            If Me.chkFilterLock.Checked Then
                Me.FilterArray.Add("Lock = 1")
            Else
                Me.FilterArray.Remove("Lock = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub rbFilterAnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFilterAnd.Click

        If clbFilterGenres.CheckedItems.Count > 0 Then
            Me.txtFilterGenre.Text = String.Empty
            Me.FilterArray.Remove(Me.filGenre)

            Dim alGenres As New ArrayList
            alGenres.AddRange(clbFilterGenres.CheckedItems)

            Me.txtFilterGenre.Text = Strings.Join(alGenres.ToArray, " AND ")

            For i As Integer = 0 To alGenres.Count - 1
                alGenres.Item(i) = String.Format("Genre LIKE '%{0}%'", alGenres.Item(i))
            Next

            Me.filGenre = Strings.Join(alGenres.ToArray, " AND ")

            Me.FilterArray.Add(Me.filGenre)
        End If

        If (Not String.IsNullOrEmpty(Me.cbFilterYear.Text) AndAlso Not Me.cbFilterYear.Text = "All") OrElse Me.clbFilterGenres.CheckedItems.Count > 0 OrElse _
        Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked OrElse Me.chkFilterLock.Checked OrElse Not Me.cbFilterSource.Text = "All" OrElse _
        Me.chkFilterDupe.Checked OrElse Me.chkFilterMissing.Checked OrElse Me.chkFilterTolerance.Checked Then Me.RunFilter()
    End Sub

    Private Sub rbFilterOr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFilterOr.Click

        If clbFilterGenres.CheckedItems.Count > 0 Then
            Me.txtFilterGenre.Text = String.Empty
            Me.FilterArray.Remove(Me.filGenre)

            Dim alGenres As New ArrayList
            alGenres.AddRange(clbFilterGenres.CheckedItems)

            Me.txtFilterGenre.Text = Strings.Join(alGenres.ToArray, " OR ")

            For i As Integer = 0 To alGenres.Count - 1
                alGenres.Item(i) = String.Format("Genre LIKE '%{0}%'", alGenres.Item(i))
            Next

            Me.filGenre = Strings.Join(alGenres.ToArray, " OR ")

            Me.FilterArray.Add(Me.filGenre)
        End If

        If (Not String.IsNullOrEmpty(Me.cbFilterYear.Text) AndAlso Not Me.cbFilterYear.Text = "All") OrElse Me.clbFilterGenres.CheckedItems.Count > 0 OrElse _
        Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked OrElse Me.chkFilterLock.Checked OrElse Not Me.cbFilterSource.Text = "All" OrElse _
        Me.chkFilterDupe.Checked OrElse Me.chkFilterMissing.Checked OrElse Me.chkFilterTolerance.Checked Then Me.RunFilter()
    End Sub

    Private Sub chkFilterDupe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterDupe.Click

        Try
            Me.RunFilter(Not Me.chkFilterDupe.Checked)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub dgvMediaList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvMediaList.KeyPress

        Try
            If Not StringManip.AlphaNumericOnly(e.KeyChar) Then
                For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
                    If drvRow.Cells(3).Value.ToString.ToLower.StartsWith(e.KeyChar.ToString.ToLower) Then
                        drvRow.Selected = True
                        Me.dgvMediaList.CurrentCell = drvRow.Cells(3)
                        Exit For
                    End If
                Next
            ElseIf e.KeyChar = Chr(13) Then
                If Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse _
                Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy Then Return

                Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                Dim ID As Integer = Me.dgvMediaList.Item(0, indX).Value
                Master.currMovie = Master.DB.LoadMovieFromDB(ID)
                Me.tslStatus.Text = Master.currMovie.Filename
                Me.tmpTitle = Me.dgvMediaList.Item(3, indX).Value

                Using dEditMovie As New dlgEditMovie

                    Select Case dEditMovie.ShowDialog()
                        Case Windows.Forms.DialogResult.OK
                            Me.SetListItemAfterEdit(ID, indX)
                            If Me.RefreshMovie(ID) Then
                                Me.FillList(0)
                            End If
                        Case Windows.Forms.DialogResult.Retry
                            Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions)
                        Case Windows.Forms.DialogResult.Abort
                            Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID, True)
                        Case Else
                            If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value, True, False)
                    End Select

                End Using

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub cmnuMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMark.Click
        Try
            Dim setMark As Boolean = False
            If Me.dgvMediaList.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    'if any one item is set as unmarked, set menu to mark
                    'else they are all marked, so set menu to unmark
                    If Not sRow.Cells(11).Value Then
                        setMark = True
                        Exit For
                    End If
                Next
            End If

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE movies SET mark = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                        parMark.Value = If(Me.dgvMediaList.SelectedRows.Count > 1, setMark, Not sRow.Cells(11).Value)
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(11).Value = parMark.Value
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            setMark = False
            For Each sRow As DataGridViewRow In Me.dgvMediaList.Rows
                If sRow.Cells(11).Value Then
                    setMark = True
                    Exit For
                End If
            Next
            Me.btnMarkAll.Text = If(setMark, Master.eLang.GetString(105, "Unmark All"), Master.eLang.GetString(35, "Mark All"))

            Me.SetFilterColors(False)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuLock.Click
        Try
            Dim setLock As Boolean = False
            If Me.dgvMediaList.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    'if any one item is set as unlocked, set menu to lock
                    'else they are all locked so set menu to unlock
                    If Not sRow.Cells(14).Value Then
                        setLock = True
                        Exit For
                    End If
                Next
            End If

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE movies SET lock = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                        parLock.Value = If(Me.dgvMediaList.SelectedRows.Count > 1, setLock, Not sRow.Cells(14).Value)
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(14).Value = parLock.Value
                    Next
                End Using
                SQLtransaction.Commit()
            End Using
            Me.SetFilterColors(False)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub AddGenreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddGenreToolStripMenuItem.Click
        Try
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parGenre As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parGenre", DbType.String, 0, "Genre")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE movies SET Genre = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                        If Not sRow.Cells(26).Value.contains(Me.GenreListToolStripComboBox.Text) Then
                            If Not String.IsNullOrEmpty(sRow.Cells(26).Value) Then
                                parGenre.Value = String.Format("{0} / {1}", sRow.Cells(26).Value, Me.GenreListToolStripComboBox.Text).Trim
                            Else
                                parGenre.Value = Me.GenreListToolStripComboBox.Text.Trim
                            End If
                            parID.Value = sRow.Cells(0).Value
                            SQLcommand.ExecuteNonQuery()
                        End If
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    Me.RefreshMovie(Convert.ToInt64(sRow.Cells(0).Value), True, False, True)
                Next
                SQLtransaction.Commit()
            End Using

            Me.LoadInfo(Me.dgvMediaList.Item(0, Me.dgvMediaList.CurrentCell.RowIndex).Value, Me.dgvMediaList.Item(1, Me.dgvMediaList.CurrentCell.RowIndex).Value, True, False)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SetGenreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetGenreToolStripMenuItem.Click
        Try
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parGenre As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parGenre", DbType.String, 0, "Genre")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE movies SET Genre = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                        parGenre.Value = Me.GenreListToolStripComboBox.Text.Trim
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    Me.RefreshMovie(Convert.ToInt64(sRow.Cells(0).Value), True, False, True)
                Next
                SQLtransaction.Commit()
            End Using

            Me.LoadInfo(Me.dgvMediaList.Item(0, Me.dgvMediaList.CurrentCell.RowIndex).Value, Me.dgvMediaList.Item(1, Me.dgvMediaList.CurrentCell.RowIndex).Value, True, False)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RemoveGenreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveGenreToolStripMenuItem.Click
        Try
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parGenre As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parGenre", DbType.String, 0, "Genre")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE movies SET Genre = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                        If sRow.Cells(26).Value.contains(Me.GenreListToolStripComboBox.Text) Then
                            parGenre.Value = sRow.Cells(26).Value.ToString.Replace(String.Concat(" / ", Me.GenreListToolStripComboBox.Text), String.Empty).Replace(String.Concat(Me.GenreListToolStripComboBox.Text, " / "), String.Empty).Replace(Me.GenreListToolStripComboBox.Text, String.Empty).Trim
                            parID.Value = sRow.Cells(0).Value
                            SQLcommand.ExecuteNonQuery()
                        End If
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    Me.RefreshMovie(Convert.ToInt64(sRow.Cells(0).Value), True, False, True)
                Next
                SQLtransaction.Commit()
            End Using

            Me.LoadInfo(Me.dgvMediaList.Item(0, Me.dgvMediaList.CurrentCell.RowIndex).Value, Me.dgvMediaList.Item(1, Me.dgvMediaList.CurrentCell.RowIndex).Value, True, False)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

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
            Me.tmpTitle = Me.dgvMediaList.Item(3, indX).Value

            Using dEditMovie As New dlgEditMovie
                Select Case dEditMovie.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        Me.SetListItemAfterEdit(ID, indX)
                        If Me.RefreshMovie(ID) Then
                            Me.FillList(0)
                        End If
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID, True)
                    Case Else
                        If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value, True, False)
                End Select
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvMediaList.MouseDown
        Try
            If e.Button = Windows.Forms.MouseButtons.Right And Me.dgvMediaList.RowCount > 0 Then
                Dim dgvHTI As DataGridView.HitTestInfo = sender.HitTest(e.X, e.Y)
                If dgvHTI.Type = DataGridViewHitTestType.Cell Then

                    If Me.dgvMediaList.SelectedRows.Count > 1 AndAlso Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                        Dim setMark As Boolean = False
                        Dim setLock As Boolean = False

                        Me.cmnuTitle.Text = Master.eLang.GetString(106, ">> Multiple <<")
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
                                If setLock Then Exit For
                            End If
                            'if any one item is set as unlocked, set menu to lock
                            'else they are all locked so set menu to unlock
                            If Not sRow.Cells(14).Value Then
                                setLock = True
                                If setMark Then Exit For
                            End If
                        Next

                        Me.cmnuMark.Text = If(setMark, Master.eLang.GetString(23, "Mark"), Master.eLang.GetString(107, "Unmark"))
                        Me.cmnuLock.Text = If(setLock, Master.eLang.GetString(24, "Lock"), Master.eLang.GetString(108, "Unlock"))

                        Me.GenreListToolStripComboBox.Items.Insert(0, Master.eLang.GetString(98, "Select Genre..."))
                        Me.GenreListToolStripComboBox.SelectedItem = Master.eLang.GetString(98, "Select Genre...")
                        Me.AddGenreToolStripMenuItem.Enabled = False
                        Me.SetGenreToolStripMenuItem.Enabled = False
                        Me.RemoveGenreToolStripMenuItem.Enabled = False
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

                        If Not Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                            Me.dgvMediaList.ClearSelection()
                            Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected = True
                            Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Item(3, dgvHTI.RowIndex)
                        End If

                        Me.cmnuMark.Text = If(Me.dgvMediaList.Item(11, dgvHTI.RowIndex).Value, Master.eLang.GetString(107, "Unmark"), Master.eLang.GetString(23, "Mark"))
                        Me.cmnuLock.Text = If(Me.dgvMediaList.Item(14, dgvHTI.RowIndex).Value, Master.eLang.GetString(108, "Unlock"), Master.eLang.GetString(24, "Lock"))

                        Me.GenreListToolStripComboBox.Tag = Me.dgvMediaList.Item(26, dgvHTI.RowIndex).Value
                        Me.GenreListToolStripComboBox.Items.Insert(0, Master.eLang.GetString(98, "Select Genre..."))
                        Me.GenreListToolStripComboBox.SelectedItem = Master.eLang.GetString(98, "Select Genre...")
                        Me.AddGenreToolStripMenuItem.Enabled = False
                        Me.SetGenreToolStripMenuItem.Enabled = False
                        Me.RemoveGenreToolStripMenuItem.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
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
            Using Explorer As New Diagnostics.Process
                Explorer.StartInfo.FileName = "explorer.exe"
                Explorer.StartInfo.Arguments = String.Format("/select,""{0}""", sRow.Cells(1).Value)
                Explorer.Start()
            End Using
        Next
    End Sub

    Private Sub DeleteMovieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMovieToolStripMenuItem.Click
        Try
            Dim mMovie As Master.DBMovie
            If MsgBox(String.Concat(Master.eLang.GetString(109, "WARNING: THIS WILL PERMANENTLY DELETE THE SELECTED MOVIE(S) FROM THE HARD DRIVE"), vbNewLine, vbNewLine, Master.eLang.GetString(101, "Are you sure you want to continue?")), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, Master.eLang.GetString(104, "Are You Sure?")) = MsgBoxResult.Yes Then
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
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub CopyExistingFanartToBackdropsFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyExistingFanartToBackdropsFolderToolStripMenuItem.Click
        '//
        ' Copy all existing fanart to the backdrops folder
        '\\

        Me.ScrapeData(Master.ScrapeType.CopyBD, Nothing, Nothing)
    End Sub

    Private Sub btnMarkAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarkAll.Click
        Try
            Dim MarkAll As Boolean = Not btnMarkAll.Text = Master.eLang.GetString(105, "Unmark All")
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                    SQLcommand.CommandText = "UPDATE movies SET mark = (?);"
                    parMark.Value = MarkAll
                    SQLcommand.ExecuteNonQuery()
                End Using
                SQLtransaction.Commit()
            End Using
            For Each drvRow As DataRow In dtMedia.Rows
                drvRow.Item(11) = MarkAll
            Next
            Me.SetFilterColors(False)
            btnMarkAll.Text = If(Not MarkAll, Master.eLang.GetString(35, "Mark All"), Master.eLang.GetString(105, "Unmark All"))
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
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
            Master.DeleteDirectory(Master.TempPath)
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
                    Me.pnlFilter.Height = 155
                Else
                    Me.pnlFilter.Height = 25
                End If
            End If
            If Me.pnlFilter.Height = 25 Then
                Me.tmrFilterAni.Stop()
                Me.btnFilterUp.Enabled = True
                Me.btnFilterDown.Enabled = False
            ElseIf Me.pnlFilter.Height = 155 Then
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

            For i As Integer = Me.FilterArray.Count - 1 To 0 Step -1
                If Strings.Left(Me.FilterArray(i), 8) = "source =" Then
                    Me.FilterArray.RemoveAt(i)
                End If
            Next

            If Not cbFilterSource.Text = "All" Then
                Me.FilterArray.Add(String.Format("source = '{0}'", cbFilterSource.Text))
            End If
            Me.RunFilter()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                    Application.DoEvents()
                    Me.LoadMedia(0)
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub

    Private Sub cmnuRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRefresh.Click
        Try
            Me.dgvMediaList.Cursor = Cursors.WaitCursor
            Me.dgvMediaList.Enabled = False

            Dim doFill As Boolean = False
            Dim doBatch As Boolean = Me.dgvMediaList.SelectedRows.Count > 1

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    doFill = Me.RefreshMovie(sRow.Cells(0).Value, doBatch)
                Next
                SQLtransaction.Commit()
            End Using

            Me.dgvMediaList.Cursor = Cursors.Default
            Me.dgvMediaList.Enabled = True

            If doFill Then FillList(0) Else SetFilterColors(True)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RefreshAllMoviesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshAllMoviesToolStripMenuItem.Click
        RefreshAllMovies()
    End Sub

    Private Sub RefreshAllMovies()
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
            Me.tslLoading.Text = Master.eLang.GetString(110, "Refreshing Media:")
            Me.tspbLoading.Visible = True
            Me.tslLoading.Visible = True

            Me.bwRefreshMovies.WorkerReportsProgress = True
            Me.bwRefreshMovies.WorkerSupportsCancellation = True
            Me.bwRefreshMovies.RunWorkerAsync()

        End If
    End Sub

    Private Sub clbFilterGenres_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles clbFilterGenres.LostFocus
        Try
            Me.pnlFilterGenre.Visible = False
            Me.pnlFilterGenre.Tag = "NO"

            If clbFilterGenres.CheckedItems.Count > 0 Then
                Me.txtFilterGenre.Text = String.Empty
                Me.FilterArray.Remove(Me.filGenre)

                Dim alGenres As New ArrayList
                alGenres.AddRange(clbFilterGenres.CheckedItems)

                If rbFilterAnd.Checked Then
                    Me.txtFilterGenre.Text = Strings.Join(alGenres.ToArray, " AND ")
                Else
                    Me.txtFilterGenre.Text = Strings.Join(alGenres.ToArray, " OR ")
                End If

                For i As Integer = 0 To alGenres.Count - 1
                    alGenres.Item(i) = String.Format("Genre LIKE '%{0}%'", alGenres.Item(i))
                Next

                If rbFilterAnd.Checked Then
                    Me.filGenre = Strings.Join(alGenres.ToArray, " AND ")
                Else
                    Me.filGenre = Strings.Join(alGenres.ToArray, " OR ")
                End If

                Me.FilterArray.Add(Me.filGenre)
                Me.RunFilter()
            Else
                If Not String.IsNullOrEmpty(Me.filGenre) Then
                    Me.txtFilterGenre.Text = String.Empty
                    Me.FilterArray.Remove(Me.filGenre)
                    Me.filGenre = String.Empty
                    Me.RunFilter()
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub txtFilterGenre_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilterGenre.Click
        Me.pnlFilterGenre.Location = New Point(Me.gbSpecific.Left + Me.txtFilterGenre.Left, (Me.pnlFilter.Top + Me.txtFilterGenre.Top + Me.gbSpecific.Top) - Me.pnlFilterGenre.Height)
        If Me.pnlFilterGenre.Visible Then
            Me.pnlFilterGenre.Visible = False
        ElseIf Not Me.pnlFilterGenre.Tag = "NO" Then
            Me.pnlFilterGenre.Tag = String.Empty
            Me.pnlFilterGenre.Visible = True
            Me.clbFilterGenres.Focus()
        Else
            Me.pnlFilterGenre.Tag = String.Empty
        End If
    End Sub

    Private Sub lblGFilClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblGFilClose.Click
        Me.txtFilterGenre.Focus()
        Me.pnlFilterGenre.Tag = String.Empty
    End Sub

    Private Sub cbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSearch.SelectedIndexChanged
        Me.txtSearch.Text = String.Empty
    End Sub

    Private Sub cbFilterYearMod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterYearMod.SelectedIndexChanged
        Try
            If Not String.IsNullOrEmpty(cbFilterYear.Text) AndAlso Not cbFilterYear.Text = "All" Then
                Me.FilterArray.Remove(Me.filYear)
                Me.filYear = String.Empty

                Me.filYear = String.Concat("Year ", cbFilterYearMod.Text, " '", cbFilterYear.Text, "'")

                Me.FilterArray.Add(Me.filYear)
                Me.RunFilter()
            Else
                If Not String.IsNullOrEmpty(Me.filYear) Then
                    Me.FilterArray.Remove(Me.filYear)
                    Me.filYear = String.Empty
                    Me.RunFilter()
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub cbFilterYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterYear.SelectedIndexChanged
        Try
            If Not String.IsNullOrEmpty(cbFilterYearMod.Text) AndAlso Not cbFilterYear.Text = "All" Then
                Me.FilterArray.Remove(Me.filYear)
                Me.filYear = String.Empty

                Me.filYear = String.Concat("Year ", cbFilterYearMod.Text, " '", cbFilterYear.Text, "'")

                Me.FilterArray.Add(Me.filYear)
                Me.RunFilter()
            Else
                If Not String.IsNullOrEmpty(Me.filYear) Then
                    Me.FilterArray.Remove(Me.filYear)
                    Me.filYear = String.Empty
                    Me.RunFilter()
                End If

                If cbFilterYear.Text = "All" Then
                    Me.cbFilterYearMod.Text = String.Empty
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub btnClearFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearFilters.Click
        Me.ClearFilters(True)
    End Sub

    Private Sub dgvMediaList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvMediaList.KeyDown
        'stop enter key from selecting next list item
        e.Handled = e.KeyCode = Keys.Enter
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

        Dim Args As Arguments = e.Argument
        Try
            Master.alMoviePaths.Clear()
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "SELECT MoviePath FROM movies;"
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    While SQLreader.Read
                        Master.alMoviePaths.Add(SQLreader("MoviePath").ToString.ToLower)
                    End While
                End Using
            End Using

            Master.MediaList.Clear()
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                If Not String.IsNullOrEmpty(Args.SourceName) Then
                    SQLcommand.CommandText = String.Format("SELECT * FROM sources WHERE Name = ""{0}"";", Args.SourceName)
                Else
                    SQLcommand.CommandText = "SELECT * FROM sources;"
                End If
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
            Master.DB.FillDataTable(dtMediaList, "SELECT MoviePath, Id, Source FROM movies ORDER BY ListTitle COLLATE NOCASE;")
            If dtMediaList.Rows.Count > 0 Then
                Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                    Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                        SQLcommand.CommandText = "DELETE FROM movies WHERE MoviePath = (?);"
                        Dim parPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMoviePath", DbType.String, 0, "MoviePath")
                        For Each mRow As DataRow In dtMediaList.Rows
                            MLFind.SearchString = mRow.Item(0)
                            MLFound = Master.MediaList.Find(AddressOf MLFind.Find)
                            If (IsNothing(MLFound) AndAlso (Args.SourceName = String.Empty OrElse mRow.Item(2) = Args.SourceName)) OrElse Not Master.eSettings.ValidExts.Contains(Path.GetExtension(mRow.Item(0)).ToLower) Then
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
                        Me.tslStatus.Text = Master.eLang.GetString(111, "Unable to load directories. Please check settings.")
                        Me.tspbLoading.Visible = False
                        Me.tslLoading.Visible = False
                        Me.tabsMain.Enabled = True
                        Me.tsbRefreshMedia.Enabled = True
                        Me.ToolsToolStripMenuItem.Enabled = False
                        Me.tsbAutoPilot.Enabled = False
                        Me.mnuMediaList.Enabled = False
                    End If

                Else
                    Me.tslLoading.Text = Master.eLang.GetString(7, "Loading Media:")
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
        Dim aContents(6) As String

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
                        'first, lets get the contents
                        aContents = Master.GetFolderContents(sFile.Filename, sFile.isSingle)
                        sFile.Poster = aContents(0)
                        sFile.Fanart = aContents(1)
                        sFile.Nfo = aContents(2)
                        sFile.Trailer = aContents(3)
                        sFile.Subs = aContents(4)
                        sFile.Extra = aContents(5)

                        If Not String.IsNullOrEmpty(sFile.Nfo) Then
                            tmpMovieDB.Movie = NFO.LoadMovieFromNFO(sFile.Nfo, sFile.isSingle)
                        Else
                            tmpMovieDB.Movie = NFO.LoadMovieFromNFO(sFile.Filename, sFile.isSingle)
                        End If

                        If String.IsNullOrEmpty(tmpMovieDB.Movie.Title) Then
                            'no title so assume it's an invalid nfo, clear nfo path if exists
                            sFile.Nfo = String.Empty

                            If Directory.GetParent(sFile.Filename).Name.ToLower = "video_ts" Then
                                tmpMovieDB.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name)
                            Else
                                If sFile.UseFolder AndAlso sFile.isSingle Then
                                    tmpMovieDB.ListTitle = StringManip.FilterName(Directory.GetParent(sFile.Filename).Name)
                                Else
                                    tmpMovieDB.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(sFile.Filename))
                                End If
                            End If
                        Else
                            If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(tmpMovieDB.Movie.Year) Then
                                tmpMovieDB.ListTitle = String.Format("{0} ({1})", StringManip.FilterTokens(tmpMovieDB.Movie.Title), tmpMovieDB.Movie.Year)
                            Else
                                tmpMovieDB.ListTitle = StringManip.FilterTokens(tmpMovieDB.Movie.Title)
                            End If
                        End If

                        Me.bwFolderData.ReportProgress(currentIndex, tmpMovieDB.ListTitle)
                        If Not String.IsNullOrEmpty(tmpMovieDB.ListTitle) Then
                            tmpMovieDB.NfoPath = sFile.Nfo
                            tmpMovieDB.PosterPath = sFile.Poster
                            tmpMovieDB.FanartPath = sFile.Fanart
                            tmpMovieDB.TrailerPath = sFile.Trailer
                            tmpMovieDB.SubPath = sFile.Subs
                            tmpMovieDB.ExtraPath = sFile.Extra
                            tmpMovieDB.Filename = sFile.Filename
                            tmpMovieDB.isSingle = sFile.isSingle
                            tmpMovieDB.UseFolder = sFile.UseFolder
                            tmpMovieDB.Source = sFile.Source
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

            e.Result = New Results With {.fileinfo = NFO.FIToString(Args.Movie.Movie.FileInfo), .setEnabled = Args.setEnabled, .Path = Args.Path, .Movie = Args.Movie}
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
                        XML.GetAVImages(Res.Movie)
                        Me.pnlInfoIcons.Width = 390
                        Me.pbStudio.Left = 325
                    Else
                        Me.pnlInfoIcons.Width = 65
                        Me.pbStudio.Left = 0
                    End If
                    If Master.eSettings.UseMIDuration Then
                        If Not String.IsNullOrEmpty(Res.Movie.Movie.Runtime) Then
                            Me.lblRuntime.Text = String.Format(Master.eLang.GetString(112, "Runtime: {0}"), Res.Movie.Movie.Runtime)
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
        Try
            Dim tImage As Image = Nothing

            Dim wrRequest As WebRequest = WebRequest.Create(Args.pURL)
            wrRequest.Timeout = 10000
            Using wrResponse As WebResponse = wrRequest.GetResponse()
                tImage = Image.FromStream(wrResponse.GetResponseStream())
            End Using
            e.Result = New Results With {.ResultType = Args.pType, .Result = tImage}
        Catch ex As Exception
            e.Result = New Results With {.ResultType = Args.pType, .Result = Nothing}
        End Try

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

            If Not Master.eSettings.NoDisplayFanart Then Me.MainFanart.FromFile(Master.currMovie.FanartPath)

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayPoster Then Me.MainPoster.FromFile(Master.currMovie.PosterPath)
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
                    ImageManip.ResizePB(Me.pbPoster, Me.pbPosterCache, 160, 160)
                    ImageManip.SetGlassOverlay(Me.pbPoster)
                    Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)

                    If Master.eSettings.ShowDims Then
                        g = Graphics.FromImage(pbPoster.Image)
                        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        strSize = String.Format("{0} x {1}", Me.MainPoster.Image.Width, Me.MainPoster.Image.Height)
                        lenSize = g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width
                        rect = New Rectangle((pbPoster.Image.Width - lenSize) / 2 - 15, Me.pbPoster.Height - 25, lenSize + 30, 25)
                        ImageManip.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
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

                ImageManip.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2

                If Not IsNothing(pbFanart.Image) AndAlso Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbFanart.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainFanart.Image.Width, Me.MainFanart.Image.Height)
                    lenSize = g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width
                    rect = New Rectangle((pbFanart.Image.Width - lenSize) - 40, 10, lenSize + 30, 25)
                    ImageManip.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbFanart.Image.Width - lenSize) - 25, 15)
                End If

                Me.InfoCleared = False

                If Not bwScraper.IsBusy AndAlso Not bwRefreshMovies.IsBusy Then
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

    Delegate Sub MydtMediaUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
    Sub dtMediaUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
        drow.Item(i) = v
    End Sub
    Private Sub bwScraper_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwScraper.DoWork

        '//
        ' Thread to handle scraping
        '\\

        Dim myDelegate As MydtMediaUpdate
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

        myDelegate = New MydtMediaUpdate(AddressOf dtMediaUpdate)

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
                                If Not String.IsNullOrEmpty(scrapeMovie.Movie.Title) Then
                                    If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.Year) Then
                                        scrapeMovie.ListTitle = String.Format("{0} ({1})", StringManip.FilterTokens(scrapeMovie.Movie.Title), scrapeMovie.Movie.Year)
                                    Else
                                        scrapeMovie.ListTitle = StringManip.FilterTokens(scrapeMovie.Movie.Title)
                                    End If
                                Else
                                    If Directory.GetParent(drvRow.Item(1)).Name.ToLower = "video_ts" Then
                                        scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(drvRow.Item(1)).FullName).Name)
                                    Else
                                        If drvRow.Item(46) AndAlso drvRow.Item(2) Then
                                            scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(drvRow.Item(1)).Name)
                                        Else
                                            scrapeMovie.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(drvRow.Item(1)))
                                        End If
                                    End If
                                End If

                                Me.Invoke(myDelegate, New Object() {drvRow, 3, scrapeMovie.ListTitle})

                                If Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) Then

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Master.eSettings.ScanMediaInfo AndAlso (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.MI) Then
                                        UpdateMediaInfo(scrapeMovie)
                                        doSave = True
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Poster) Then
                                        pThumbs.Clear()
                                        Poster.Clear()
                                        If Poster.IsAllowedToDownload(scrapeMovie, Master.ImageType.Posters) Then
                                            If Poster.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs, If(Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk, True, False)) Then
                                                If Not IsNothing(Poster.Image) Then
                                                    pPath = Poster.SaveAsPoster(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(pPath) Then
                                                        scrapeMovie.PosterPath = pPath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Thumbs = pThumbs
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk Then
                                                    MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        pPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Posters)
                                                        If Not String.IsNullOrEmpty(pPath) Then
                                                            scrapeMovie.PosterPath = pPath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
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
                                                        scrapeMovie.FanartPath = fPath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Fanart = fArt
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk Then
                                                    MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))

                                                    Using dImgSelect As New dlgImgSelect
                                                        fPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Fanart)
                                                        If Not String.IsNullOrEmpty(fPath) Then
                                                            scrapeMovie.FanartPath = fPath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
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
                                        tURL = Trailer.DownloadSingleTrailer(scrapeMovie.Filename, scrapeMovie.Movie.IMDBID, drvRow.Item(2), scrapeMovie.Movie.Trailer)
                                        If Not String.IsNullOrEmpty(tURL) Then
                                            If tURL.Substring(0, 7) = "http://" Then
                                                scrapeMovie.Movie.Trailer = tURL
                                                doSave = True
                                            Else
                                                scrapeMovie.TrailerPath = tURL
                                                Me.Invoke(myDelegate, New Object() {drvRow, 7, True})
                                            End If
                                        End If
                                    End If

                                End If

                                If Me.bwScraper.CancellationPending Then GoTo doCancel

                                If (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Extra) Then
                                    If Master.eSettings.AutoThumbs > 0 AndAlso drvRow.Item(2) Then
                                        Dim ETasFA As String = Master.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs)
                                        If Not String.IsNullOrEmpty(ETasFA) Then
                                            Me.Invoke(myDelegate, New Object() {drvRow, 9, True})
                                            scrapeMovie.ExtraPath = "TRUE"
                                            If Not ETasFA = "TRUE" Then
                                                Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                scrapeMovie.FanartPath = ETasFA
                                            End If
                                        End If
                                    End If
                                End If

                                If doSave Then
                                    Me.Invoke(myDelegate, New Object() {drvRow, 6, True})
                                End If


                                Master.DB.SaveMovieToDB(scrapeMovie, False, True, doSave AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID))

                                'use this one to check for need of load info
                                Me.bwScraper.ReportProgress(iCount, String.Format("[[{0}]]", drvRow.Item(0).ToString))
                                iCount += 1
                            Next

                        Case Master.ScrapeType.UpdateAsk, Master.ScrapeType.UpdateAuto

                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)

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

                                        If Master.eSettings.ScanMediaInfo AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) Then
                                            UpdateMediaInfo(scrapeMovie)
                                        End If

                                        If Not String.IsNullOrEmpty(scrapeMovie.Movie.Title) Then
                                            If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.Year) Then
                                                scrapeMovie.ListTitle = String.Format("{0} ({1})", StringManip.FilterTokens(scrapeMovie.Movie.Title), scrapeMovie.Movie.Year)
                                            Else
                                                scrapeMovie.ListTitle = StringManip.FilterTokens(scrapeMovie.Movie.Title)
                                            End If
                                        Else
                                            If Directory.GetParent(drvRow.Item(1)).Name.ToLower = "video_ts" Then
                                                scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(drvRow.Item(1)).FullName).Name)
                                            Else
                                                If drvRow.Item(46) AndAlso drvRow.Item(2) Then
                                                    scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(drvRow.Item(1)).Name)
                                                Else
                                                    scrapeMovie.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(drvRow.Item(1)))
                                                End If
                                            End If
                                        End If

                                        Me.Invoke(myDelegate, New Object() {drvRow, 3, scrapeMovie.Movie.Title})
                                        Me.Invoke(myDelegate, New Object() {drvRow, 6, True})
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
                                                        scrapeMovie.PosterPath = pPath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Thumbs = pThumbs
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.UpdateAsk Then
                                                    MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        pPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Posters)
                                                        If Not String.IsNullOrEmpty(pPath) Then
                                                            scrapeMovie.PosterPath = pPath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
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
                                                        scrapeMovie.FanartPath = fPath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                        If Args.scrapeMod = Master.ScrapeModifier.All Then
                                                            scrapeMovie.Movie.Fanart = fArt
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.UpdateAsk Then
                                                    MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        fPath = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Fanart)
                                                        If Not String.IsNullOrEmpty(fPath) Then
                                                            scrapeMovie.FanartPath = fPath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
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
                                        tURL = Trailer.DownloadSingleTrailer(scrapeMovie.Filename, scrapeMovie.Movie.IMDBID, drvRow.Item(2), scrapeMovie.Movie.Trailer)
                                        If Not String.IsNullOrEmpty(tURL) Then
                                            If tURL.Substring(0, 7) = "http://" Then
                                                scrapeMovie.Movie.Trailer = tURL
                                                doSave = True
                                            Else
                                                scrapeMovie.TrailerPath = tURL
                                                Me.Invoke(myDelegate, New Object() {drvRow, 7, True})
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Master.eSettings.AutoThumbs > 0 AndAlso drvRow.Item(2) AndAlso Not Directory.Exists(Path.Combine(Directory.GetParent(scrapeMovie.Filename).FullName, "extrathumbs")) AndAlso _
                                    (Args.scrapeMod = Master.ScrapeModifier.All OrElse Args.scrapeMod = Master.ScrapeModifier.Extra) Then
                                        Dim ETasFA As String = Master.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs)
                                        If Not String.IsNullOrEmpty(ETasFA) Then

                                            Me.Invoke(myDelegate, New Object() {drvRow, 9, True})
                                            scrapeMovie.ExtraPath = "TRUE"
                                            If Not ETasFA = "TRUE" Then
                                                Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                scrapeMovie.FanartPath = ETasFA
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    Master.DB.SaveMovieToDB(scrapeMovie, False, True, doSave)

                                End If

                                'use this one to check for need of load info
                                Me.bwScraper.ReportProgress(iCount, String.Format("[[{0}]]", drvRow.Item(0).ToString))
                                iCount += 1

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
                                sPath = drvRow.Item(40)
                                If Not String.IsNullOrEmpty(sPath) Then
                                    If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                                        If Master.eSettings.VideoTSParent Then
                                            Master.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), "-fanart.jpg")))
                                        Else
                                            If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                                Master.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")))
                                            Else
                                                Master.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)))
                                            End If
                                        End If
                                    Else
                                        If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                            Master.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString), "-fanart.jpg")))
                                        Else
                                            Master.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)))
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
                                    Master.DB.SaveMovieToDB(scrapeMovie, False, True, True)
                                End If
                            Next

                    End Select
                End If

            Catch ex As Exception
                ScraperDone = True
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

doCancel:
            If Not Args.scrapeType = Master.ScrapeType.CopyBD AndAlso Not Args.scrapeType = Master.ScrapeType.CleanFolders Then
                SQLtransaction.Commit()
            End If
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
            If Regex.IsMatch(e.UserState.ToString, "\[\[[0-9]+\]\]") Then
                If Me.dgvMediaList.SelectedRows(0).Cells(0).Value = e.UserState.ToString.Replace("[[", String.Empty).Replace("]]", String.Empty).Trim Then
                    Me.LoadInfo(Me.dgvMediaList.SelectedRows(0).Cells(0).Value, Me.dgvMediaList.SelectedRows(0).Cells(1).Value, True, False)
                End If
            Else
                SetFilterColors(False)
                Me.tslStatus.Text = e.UserState.ToString
                Me.tspbLoading.Value = e.ProgressPercentage
            End If
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

    Public Sub SetUp()

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

                .FileToolStripMenuItem.Text = Master.eLang.GetString(1, "&File")
                .ExitToolStripMenuItem.Text = Master.eLang.GetString(2, "E&xit")
                .EditToolStripMenuItem.Text = Master.eLang.GetString(3, "&Edit")
                .SettingsToolStripMenuItem.Text = Master.eLang.GetString(4, "&Settings...")
                .HelpToolStripMenuItem.Text = Master.eLang.GetString(5, "&Help")
                .AboutToolStripMenuItem.Text = Master.eLang.GetString(6, "&About...")
                .tslLoading.Text = Master.eLang.GetString(7, "Loading Media:")
                .ToolsToolStripMenuItem.Text = Master.eLang.GetString(8, "&Tools")
                .CleanFoldersToolStripMenuItem.Text = Master.eLang.GetString(9, "&Clean Files")
                .ConvertFileSourceToFolderSourceToolStripMenuItem.Text = Master.eLang.GetString(10, "&Sort Files Into Folders")
                .CopyExistingFanartToBackdropsFolderToolStripMenuItem.Text = Master.eLang.GetString(11, "Copy Existing Fanart To &Backdrops Folder")
                .mnuRevertStudioTags.Text = Master.eLang.GetString(12, "Revert Meta Data Studio &Tags")
                .RenamerToolStripMenuItem.Text = Master.eLang.GetString(13, "Bulk &Renamer")
                .SetsManagerToolStripMenuItem.Text = Master.eLang.GetString(14, "Sets &Manager")
                .OfflineMediaManagerToolStripMenuItem.Text = Master.eLang.GetString(15, "&Offline Media Manager")
                .ExportMoviesListToolStripMenuItem.Text = Master.eLang.GetString(16, "&Export Movies List")
                .ClearAllCachesToolStripMenuItem.Text = Master.eLang.GetString(17, "Clear &All Caches")
                .RefreshAllMoviesToolStripMenuItem.Text = Master.eLang.GetString(18, "Re&load All Movies")
                .lblGFilClose.Text = Master.eLang.GetString(19, "Close")
                .Label4.Text = Master.eLang.GetString(20, "Genres")
                .cmnuTitle.Text = Master.eLang.GetString(21, "Title")
                .cmnuRefresh.Text = Master.eLang.GetString(22, "Reload")
                .cmnuMark.Text = Master.eLang.GetString(23, "Mark")
                .cmnuLock.Text = Master.eLang.GetString(24, "Lock")
                .cmnuEditMovie.Text = Master.eLang.GetString(25, "Edit Movie")
                .GenresToolStripMenuItem.Text = Master.eLang.GetString(20, "Genres")
                .LblGenreStripMenuItem2.Text = Master.eLang.GetString(27, ">> Select Genre <<")
                .AddGenreToolStripMenuItem.Text = Master.eLang.GetString(28, "Add")
                .SetGenreToolStripMenuItem.Text = Master.eLang.GetString(29, "Set")
                .RemoveGenreToolStripMenuItem.Text = Master.eLang.GetString(30, "Remove")
                .cmnuRescrape.Text = Master.eLang.GetString(31, "Re-scrape IMDB")
                .cmnuSearchNew.Text = Master.eLang.GetString(32, "Change Movie")
                .OpenContainingFolderToolStripMenuItem.Text = Master.eLang.GetString(33, "Open Containing Folder")
                .DeleteMovieToolStripMenuItem.Text = Master.eLang.GetString(34, "Delete Movie")
                .btnMarkAll.Text = Master.eLang.GetString(35, "Mark All")
                .tabMovies.Text = Master.eLang.GetString(36, "Movies")
                .btnClearFilters.Text = Master.eLang.GetString(37, "Clear Filters")
                .GroupBox3.Text = Master.eLang.GetString(38, "General")
                .chkFilterTolerance.Text = Master.eLang.GetString(39, "Out of Tolerance")
                .chkFilterMissing.Text = Master.eLang.GetString(40, "Missing Items")
                .chkFilterDupe.Text = Master.eLang.GetString(41, "Duplicates")
                .gbSpecific.Text = Master.eLang.GetString(42, "Specific")
                .chkFilterLock.Text = Master.eLang.GetString(43, "Locked")
                .GroupBox2.Text = Master.eLang.GetString(44, "Modifier")
                .rbFilterAnd.Text = Master.eLang.GetString(45, "And")
                .rbFilterOr.Text = Master.eLang.GetString(46, "Or")
                .chkFilterNew.Text = Master.eLang.GetString(47, "New")
                .chkFilterMark.Text = Master.eLang.GetString(48, "Marked")
                .Label5.Text = Master.eLang.GetString(49, "Year:")
                .Label2.Text = Master.eLang.GetString(50, "Source:")
                .Label3.Text = Master.eLang.GetString(51, "Genre(s):")
                .lblFilter.Text = Master.eLang.GetString(52, "Filters")
                .lblCanceling.Text = Master.eLang.GetString(53, "Canceling Scraper...")
                .btnCancel.Text = Master.eLang.GetString(54, "Cancel Scraper")
                .Label1.Text = Master.eLang.GetString(55, "No Information is Available for This Movie")
                .lblCertsHeader.Text = Master.eLang.GetString(56, "Certification(s)")
                .lblReleaseDateHeader.Text = Master.eLang.GetString(57, "Release Date")
                .btnMIRefresh.Text = Master.eLang.GetString(58, "Refresh")
                .lblMIHeader.Text = Master.eLang.GetString(59, "Meta Data")
                .lblFilePathHeader.Text = Master.eLang.GetString(60, "File Path")
                .lblIMDBHeader.Text = Master.eLang.GetString(61, "IMDB ID")
                .lblDirectorHeader.Text = Master.eLang.GetString(62, "Director")
                .lblActorsHeader.Text = Master.eLang.GetString(63, "Cast")
                .lblOutlineHeader.Text = Master.eLang.GetString(64, "Plot Outline")
                .lblPlotHeader.Text = Master.eLang.GetString(65, "Plot")
                .lblInfoPanelHeader.Text = Master.eLang.GetString(66, "Info")
                .tsbAutoPilot.Text = Master.eLang.GetString(67, "Scrape Media")
                .FullToolStripMenuItem.Text = Master.eLang.GetString(68, "All Movies")
                .FullAutoToolStripMenuItem.Text = Master.eLang.GetString(69, "Automatic (Force Best Match)")
                .mnuAllAutoAll.Text = Master.eLang.GetString(70, "All Items")
                .mnuAllAutoNfo.Text = Master.eLang.GetString(71, "NFO Only")
                .mnuAllAutoPoster.Text = Master.eLang.GetString(72, "Poster Only")
                .mnuAllAutoFanart.Text = Master.eLang.GetString(73, "Fanart Only")
                .mnuAllAutoExtra.Text = Master.eLang.GetString(74, "Extrathumbs Only")
                .mnuAllAutoTrailer.Text = Master.eLang.GetString(75, "Trailer Only")
                .mnuAllAutoMI.Text = Master.eLang.GetString(76, "Meta Data Only")
                .FullAskToolStripMenuItem.Text = Master.eLang.GetString(77, "Ask (Require Input If No Exact Match)")
                .mnuAllAskAll.Text = mnuAllAutoAll.Text
                .mnuAllAskNfo.Text = .mnuAllAutoNfo.Text
                .mnuAllAskPoster.Text = .mnuAllAutoPoster.Text
                .mnuAllAskFanart.Text = .mnuAllAutoFanart.Text
                .mnuAllAskExtra.Text = .mnuAllAutoExtra.Text
                .mnuAllAskTrailer.Text = .mnuAllAutoTrailer.Text
                .mnuAllAskMI.Text = .mnuAllAutoMI.Text
                .UpdateOnlyToolStripMenuItem.Text = Master.eLang.GetString(78, "Movies Missing Items")
                .UpdateAutoToolStripMenuItem.Text = .FullAutoToolStripMenuItem.Text
                .mnuMissAutoAll.Text = .mnuAllAutoAll.Text
                .mnuMissAutoNfo.Text = .mnuAllAutoNfo.Text
                .mnuMissAutoPoster.Text = .mnuAllAutoPoster.Text
                .mnuMissAutoFanart.Text = .mnuAllAutoFanart.Text
                .mnuMissAutoExtra.Text = .mnuAllAutoExtra.Text
                .mnuMissAutoTrailer.Text = .mnuAllAutoTrailer.Text
                .UpdateAskToolStripMenuItem.Text = .FullAskToolStripMenuItem.Text
                .mnuMissAskAll.Text = .mnuAllAutoAll.Text
                .mnuMissAskNfo.Text = .mnuAllAutoNfo.Text
                .mnuMissAskPoster.Text = .mnuAllAutoPoster.Text
                .mnuMissAskFanart.Text = .mnuAllAutoFanart.Text
                .mnuMissAskExtra.Text = .mnuAllAutoExtra.Text
                .mnuMissAskTrailer.Text = .mnuAllAutoTrailer.Text
                .NewMoviesToolStripMenuItem.Text = Master.eLang.GetString(79, "New Movies")
                .AutomaticForceBestMatchToolStripMenuItem.Text = .FullAutoToolStripMenuItem.Text
                .mnuNewAutoAll.Text = .mnuAllAutoAll.Text
                .mnuNewAutoNfo.Text = .mnuAllAutoNfo.Text
                .mnuNewAutoPoster.Text = .mnuAllAutoPoster.Text
                .mnuNewAutoFanart.Text = .mnuAllAutoFanart.Text
                .mnuNewAutoExtra.Text = .mnuAllAutoExtra.Text
                .mnuNewAutoTrailer.Text = .mnuAllAutoTrailer.Text
                .mnuNewAutoMI.Text = .mnuAllAutoMI.Text
                .AskRequireInputToolStripMenuItem.Text = .FullAskToolStripMenuItem.Text
                .mnuNewAskAll.Text = .mnuAllAutoAll.Text
                .mnuNewAskNfo.Text = .mnuAllAutoNfo.Text
                .mnuNewAskPoster.Text = .mnuAllAutoPoster.Text
                .mnuNewAskFanart.Text = .mnuAllAutoFanart.Text
                .mnuNewAskExtra.Text = .mnuAllAutoExtra.Text
                .mnuNewAskTrailer.Text = .mnuAllAutoTrailer.Text
                .mnuNewAskMI.Text = .mnuAllAutoMI.Text
                .MarkedMoviesToolStripMenuItem.Text = Master.eLang.GetString(80, "Marked Movies")
                .AutomaticForceBestMatchToolStripMenuItem1.Text = .FullAutoToolStripMenuItem.Text
                .mnuMarkAutoAll.Text = .mnuAllAutoAll.Text
                .mnuMarkAutoNfo.Text = .mnuAllAutoNfo.Text
                .mnuMarkAutoPoster.Text = .mnuAllAutoPoster.Text
                .mnuMarkAutoFanart.Text = .mnuAllAutoFanart.Text
                .mnuMarkAutoExtra.Text = .mnuAllAutoExtra.Text
                .mnuMarkAutoTrailer.Text = .mnuAllAutoTrailer.Text
                .mnuMarkAutoMI.Text = .mnuAllAutoMI.Text
                .AskRequireInputIfNoExactMatchToolStripMenuItem.Text = .FullAskToolStripMenuItem.Text
                .mnuMarkAskAll.Text = .mnuAllAutoAll.Text
                .mnuMarkAskNfo.Text = .mnuAllAutoNfo.Text
                .mnuMarkAskPoster.Text = .mnuAllAutoPoster.Text
                .mnuMarkAskFanart.Text = .mnuAllAutoFanart.Text
                .mnuMarkAskExtra.Text = .mnuAllAutoExtra.Text
                .mnuMarkAskTrailer.Text = .mnuAllAutoTrailer.Text
                .mnuMarkAskMI.Text = .mnuAllAutoMI.Text
                .CustomUpdaterToolStripMenuItem.Text = Master.eLang.GetString(81, "Custom Scraper...")
                .tsbRefreshMedia.Text = Master.eLang.GetString(82, "Update Library")
                .tsbUpdateXBMC.Text = Master.eLang.GetString(83, "Initiate XBMC Update")

                Dim TT As ToolTip = New System.Windows.Forms.ToolTip(.components)
                .tsbAutoPilot.ToolTipText = Master.eLang.GetString(84, "Scrape/download data from the internet for multiple movies.")
                .tsbRefreshMedia.ToolTipText = Master.eLang.GetString(85, "Scans sources for new content and cleans database.")
                .tsbUpdateXBMC.ToolTipText = Master.eLang.GetString(86, "Sends a command to XBMC to begin its internal ""Update Library"" process.")
                TT.SetToolTip(.btnMarkAll, Master.eLang.GetString(87, "Mark or Unmark all movies in the list."))
                TT.SetToolTip(.txtSearch, Master.eLang.GetString(88, "Search the movie titles by entering text here."))
                TT.SetToolTip(.btnPlay, Master.eLang.GetString(89, "Play the movie file with the system default media player."))
                TT.SetToolTip(.btnMIRefresh, Master.eLang.GetString(90, "Rescan and save the meta data for the selected movie."))
                TT.SetToolTip(.chkFilterDupe, Master.eLang.GetString(91, "Display only movies that have duplicate IMDB IDs."))
                TT.SetToolTip(.chkFilterTolerance, Master.eLang.GetString(92, "Display only movies whose title matching is out of tolerance."))
                TT.SetToolTip(.chkFilterMissing, Master.eLang.GetString(93, "Display only movies that have items missing."))
                TT.SetToolTip(.chkFilterNew, Master.eLang.GetString(94, "Display only new movies."))
                TT.SetToolTip(.chkFilterMark, Master.eLang.GetString(95, "Display only marked movies."))
                TT.SetToolTip(.chkFilterLock, Master.eLang.GetString(96, "Display only locked movies."))
                TT.SetToolTip(.cbFilterSource, Master.eLang.GetString(97, "Display only movies from the selected source."))
                TT.Active = True

                .cbSearch.Items.Clear()
                .cbSearch.Items.AddRange(New Object() {Master.eLang.GetString(21, "Title"), Master.eLang.GetString(100, "Actor"), Master.eLang.GetString(62, "Director")})

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

    Public Sub LoadMedia(ByVal mediaType As Integer, Optional ByVal SourceName As String = "")

        '//
        ' Begin threads to fill datagrid with media data
        '\\


        Try
            Me.tslStatus.Text = Master.eLang.GetString(116, "Performing Preliminary Tasks (Gathering Data)...")
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
            Me.ClearFilters()
            Me.EnableFilters(False)

            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.mnuMediaList.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tabMovies.Text = Master.eLang.GetString(36, "Movies")

            Me.bwPrelim = New System.ComponentModel.BackgroundWorker
            Me.bwPrelim.WorkerSupportsCancellation = True
            Me.bwPrelim.RunWorkerAsync(New Arguments With {.SourceName = SourceName})

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

            If Me.bwLoadInfo.IsBusy Then
                Me.bwLoadInfo.CancelAsync()
                While Me.bwLoadInfo.IsBusy
                    Application.DoEvents()
                End While
            End If

            If doMI Then
                If Me.bwMediaInfo.IsBusy Then
                    Me.bwMediaInfo.CancelAsync()
                    While Me.bwMediaInfo.IsBusy
                        Application.DoEvents()
                    End While
                End If
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
                .InfoCleared = True

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
                Catch
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
                Me.lblTitle.Text = String.Format(Master.eLang.GetString(117, "Unknown Movie ({0})"), Master.currMovie.Movie.Year)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Votes) Then
                Me.lblVotes.Text = String.Format(Master.eLang.GetString(118, "{0} Votes"), Master.currMovie.Movie.Votes)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Runtime) Then
                Me.lblRuntime.Text = String.Format(Master.eLang.GetString(112, "Runtime: {0}"), If(Master.currMovie.Movie.Runtime.Contains("|"), Strings.Left(Master.currMovie.Movie.Runtime, Master.currMovie.Movie.Runtime.IndexOf("|")), Master.currMovie.Movie.Runtime)).Trim
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
                Me.lstActors.SelectedIndex = 0
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.MPAA) Then
                Dim tmpRatingImg As Image = XML.GetRatingImage(Master.currMovie.Movie.MPAA)
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
                Me.pbStudio.Image = XML.GetStudioImage(Master.currMovie.Movie.Studio)
            Else
                Me.pbStudio.Image = XML.GetStudioImage("####")
            End If

            If Master.eSettings.ScanMediaInfo Then
                XML.GetAVImages(Master.currMovie)
                Me.pnlInfoIcons.Width = 390
                Me.pbStudio.Left = 325
            Else
                Me.pnlInfoIcons.Width = 65
                Me.pbStudio.Left = 0
            End If

            Me.lblDirector.Text = Master.currMovie.Movie.Director

            Me.txtIMDBID.Text = Master.currMovie.Movie.IMDBID

            Me.txtFilePath.Text = Master.currMovie.Filename

            Me.lblReleaseDate.Text = Master.currMovie.Movie.ReleaseDate
            Me.txtCerts.Text = Master.currMovie.Movie.Certification

            Me.txtMediaInfo.Text = NFO.FIToString(Master.currMovie.Movie.FileInfo)

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
                Me.pbGenre(i).Image = XML.GetGenreImage(genreArray(i).Trim)
                Me.pnlGenre(i).Left = ((Me.pnlInfoPanel.Right) - (i * 73)) - 73
                Me.pbGenre(i).Left = 2
                Me.pnlGenre(i).Top = Me.pnlInfoPanel.Top - 105
                Me.pbGenre(i).Top = 2
                Me.scMain.Panel2.Controls.Add(Me.pnlGenre(i))
                Me.pnlGenre(i).Controls.Add(Me.pbGenre(i))
                Me.pnlGenre(i).BringToFront()
                AddHandler Me.pbGenre(i).MouseEnter, AddressOf pbGenre_MouseEnter
                AddHandler Me.pbGenre(i).MouseLeave, AddressOf pbGenre_MouseLeave
                If Master.eSettings.AllwaysDisplayGenresText Then
                    Dim iLeft As Integer = 0
                    Me.GenreImage = pbGenre(i).Image
                    Dim bmGenre As New Bitmap(Me.GenreImage)
                    Dim grGenre As Graphics = Graphics.FromImage(bmGenre)
                    Dim drawString As String = pbGenre(i).Name
                    Dim drawFont As New Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Pixel)
                    Dim drawBrush As New SolidBrush(Color.White)
                    Dim drawWidth As Single = grGenre.MeasureString(drawString, drawFont).Width
                    Dim drawSize As Integer = (14 * (bmGenre.Width / drawWidth)) - 0.5
                    drawFont = New Font("Microsoft Sans Serif", If(drawSize > 14, 14, drawSize), FontStyle.Bold, GraphicsUnit.Pixel)
                    Dim drawHeight As Single = grGenre.MeasureString(drawString, drawFont).Height
                    iLeft = (bmGenre.Width - grGenre.MeasureString(drawString, drawFont).Width) / 2
                    grGenre.DrawString(drawString, drawFont, drawBrush, iLeft, (bmGenre.Height - drawHeight))
                    pbGenre(i).Image = bmGenre
                End If
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

                If Not sType = Master.ScrapeType.SingleScrape Then
                    Select Case sType
                        Case Master.ScrapeType.CleanFolders
                            lblCanceling.Text = Master.eLang.GetString(119, "Canceling File Cleaner...")
                            btnCancel.Text = Master.eLang.GetString(120, "Cancel Cleaner")
                        Case Master.ScrapeType.CopyBD
                            lblCanceling.Text = Master.eLang.GetString(121, "Canceling Backdrop Copy...")
                            btnCancel.Text = Master.eLang.GetString(122, "Cancel Copy")
                        Case Master.ScrapeType.RevertStudios
                            lblCanceling.Text = Master.eLang.GetString(123, "Canceling Reversion...")
                            btnCancel.Text = Master.eLang.GetString(124, "Cancel Reversion")
                        Case Else
                            lblCanceling.Text = Master.eLang.GetString(125, "Canceling Scraper...")
                            btnCancel.Text = Master.eLang.GetString(126, "Cancel Scraper")
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
                            Me.tslLoading.Text = Master.eLang.GetString(127, "Scraping Media (All Movies - Ask):")
                        Case Master.ScrapeType.FullAuto
                            Me.tslLoading.Text = Master.eLang.GetString(128, "Scraping Media (All Movies - Auto):")
                        Case Master.ScrapeType.CleanFolders
                            Me.tslLoading.Text = Master.eLang.GetString(129, "Cleaning Files:")
                        Case Master.ScrapeType.CopyBD
                            Me.tslLoading.Text = Master.eLang.GetString(130, "Copying Fanart to Backdrops Folder:")
                        Case Master.ScrapeType.RevertStudios
                            Me.tslLoading.Text = Master.eLang.GetString(131, "Reverting Meta Data Studio Tags:")
                        Case Master.ScrapeType.UpdateAuto
                            Me.tslLoading.Text = Master.eLang.GetString(132, "Scraping Media (Movies Missing Items - Auto):")
                        Case Master.ScrapeType.UpdateAsk
                            Me.tslLoading.Text = Master.eLang.GetString(133, "Scraping Media (Movies Missing Items - Ask):")
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
                                Me.tslLoading.Text = Master.eLang.GetString(134, "Scraping Media (New Movies - Ask):")
                            Case Master.ScrapeType.NewAuto
                                Me.tslLoading.Text = Master.eLang.GetString(135, "Scraping Media (New Movies - Auto):")
                            Case Master.ScrapeType.MarkAsk
                                Me.btnMarkAll.Enabled = False
                                Me.tslLoading.Text = Master.eLang.GetString(136, "Scraping Media (Marked Movies - Ask):")
                            Case Master.ScrapeType.MarkAuto
                                Me.btnMarkAll.Enabled = False
                                Me.tslLoading.Text = Master.eLang.GetString(137, "Scraping Media (Marked Movies - Auto):")
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
                    Me.tslStatus.Text = String.Format(Master.eLang.GetString(138, "Re-scraping {0}"), Master.currMovie.Movie.Title)
                    Me.tslLoading.Text = Master.eLang.GetString(139, "Scraping:")
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

                                Master.DB.SaveMovieToDB(Master.currMovie, True, False, True)
                            End If

                            If Master.eSettings.AutoThumbs > 0 AndAlso Master.currMovie.isSingle Then
                                Master.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs)
                            End If
                        Catch
                        End Try
                        Me.ScraperDone = True
                    Else
                        Master.tmpMovie.Clear()
                        If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) AndAlso doSearch = False Then
                            IMDB.GetMovieInfoAsync(Master.currMovie.Movie.IMDBID, Master.tmpMovie, Master.DefaultOptions, Master.eSettings.FullCrew, Master.eSettings.FullCast)
                        Else
                            Using dSearch As New dlgIMDBSearchResults
                                If dSearch.ShowDialog(Me.tmpTitle) = Windows.Forms.DialogResult.OK Then
                                    If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                                        Me.ClearInfo()
                                        Me.tslStatus.Text = String.Format(Master.eLang.GetString(138, "Re-scraping {0}"), Master.tmpMovie.Title)
                                        Me.tslLoading.Text = Master.eLang.GetString(139, "Scraping:")
                                        Me.tspbLoading.Maximum = 13
                                        Me.tspbLoading.Style = ProgressBarStyle.Continuous
                                        Me.ReportDownloadPercent = True
                                        Me.tslLoading.Visible = True
                                        Me.tspbLoading.Visible = True
                                        IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.DefaultOptions, Master.eSettings.FullCrew, Master.eSettings.FullCast)
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
                                        Me.LoadInfo(ID, Master.currMovie.Filename, True, False)
                                    End If
                                End If
                            End Using
                        End If
                    End If
            End Select
        Catch ex As Exception
            ScraperDone = True
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub UpdateMediaInfo(ByRef miMovie As Master.DBMovie)
        Try

            Dim pExt As String = Path.GetExtension(miMovie.Filename).ToLower
            If Not pExt = ".rar" Then
                Dim MI As New MediaInfo
                MI.GetMovieMIFromPath(miMovie.Movie.FileInfo, miMovie.Filename)
                If Master.eSettings.UseMIDuration AndAlso miMovie.Movie.FileInfo.StreamDetails.Video.Count > 0 Then
                    Dim tVid As MediaInfo.Video = NFO.GetBestVideo(miMovie.Movie.FileInfo)

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
            If bSuccess AndAlso Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                Master.currMovie.Movie = Master.tmpMovie
                If Master.eSettings.ScanMediaInfo Then
                    Me.tslLoading.Text = Master.eLang.GetString(140, "Scanning Meta Data:")
                    Me.tspbLoading.Value = Me.tspbLoading.Maximum
                    Me.tspbLoading.Style = ProgressBarStyle.Marquee
                    Me.tspbLoading.MarqueeAnimationSpeed = 25
                    Me.Refresh()
                    Me.UpdateMediaInfo(Master.currMovie)
                End If
                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    Using dImgSelectFanart As New dlgImgSelect
                        Dim AllowFA As Boolean = tmpImages.IsAllowedToDownload(Master.currMovie, Master.ImageType.Fanart, True)

                        If AllowFA Then dImgSelectFanart.ShowDialog(Master.currMovie, Master.ImageType.Fanart, True, True)

                        If tmpImages.IsAllowedToDownload(Master.currMovie, Master.ImageType.Posters, True) Then
                            Using dImgSelect As New dlgImgSelect
                                Dim pPath As String = dImgSelect.ShowDialog(Master.currMovie, Master.ImageType.Posters, True)
                                If Not String.IsNullOrEmpty(pPath) Then
                                    Master.currMovie.PosterPath = pPath
                                End If
                            End Using
                        End If

                        If AllowFA Then
                            Dim fPath As String = dImgSelectFanart.ShowDialog
                            If Not String.IsNullOrEmpty(fPath) Then
                                Master.currMovie.FanartPath = fPath
                            End If
                        End If

                    End Using
                    tmpImages.Dispose()
                    tmpImages = Nothing
                End If

                If Master.eSettings.SingleScrapeTrailer Then
                    Dim cTrailer As New Trailers
                    Dim tURL As String = cTrailer.ShowTDialog(Master.currMovie.Movie.IMDBID, Master.currMovie.Filename, Master.currMovie.Movie.Trailer)
                    If Not String.IsNullOrEmpty(tURL) AndAlso tURL.Substring(0, 7) = "http://" Then
                        Master.currMovie.Movie.Trailer = tURL
                    End If
                    cTrailer = Nothing
                End If

                If Master.eSettings.AutoThumbs > 0 AndAlso Master.currMovie.isSingle Then
                    Master.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs)
                End If
                If Not isCL Then
                    Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                    Dim ID As Integer = Me.dgvMediaList.Item(0, indX).Value
                    Me.tmpTitle = Me.dgvMediaList.Item(3, indX).Value

                    Using dEditMovie As New dlgEditMovie
                        Select Case dEditMovie.ShowDialog()
                            Case Windows.Forms.DialogResult.OK
                                Me.SetListItemAfterEdit(ID, indX)
                                If Me.RefreshMovie(ID) Then
                                    Me.FillList(0)
                                End If
                            Case Windows.Forms.DialogResult.Retry
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID)
                            Case Windows.Forms.DialogResult.Abort
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, Master.DefaultOptions, ID, True)
                            Case Else
                                If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value, True, False)
                        End Select
                    End Using
                Else
                    Master.DB.SaveMovieToDB(Master.currMovie, True, False, True)
                End If
            Else
                MsgBox(Master.eLang.GetString(141, "Unable to retrieve movie details from the internet. Please check your connection and try again."), MsgBoxStyle.Exclamation, Master.eLang.GetString(142, "Error Retrieving Details"))
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
            Me.EnableFilters(False)
        End If
    End Sub

    Private Function RefreshMovie(ByVal ID As Integer, Optional ByVal BatchMode As Boolean = False, Optional ByVal FromNfo As Boolean = True, Optional ByVal ToNfo As Boolean = False) As Boolean
        Dim dRow = From drvRow As DataRow In dtMedia.Rows Where drvRow.Item(0) = ID Select drvRow
        Dim sPath As String = dRow(0).Item(1)
        Dim aContents(6) As String
        Dim tmpMovie As New Media.Movie
        Dim tmpMovieDb As New Master.DBMovie

        Dim myDelegate As New MydtMediaUpdate(AddressOf dtMediaUpdate)

        Try

            If Directory.Exists(Directory.GetParent(dRow(0).Item(1)).FullName) Then

                If FromNfo Then
                    If String.IsNullOrEmpty(dRow(0).Item(42)) Then
                        Dim sNFO As String = NFO.GetNfoPath(dRow(0).Item(1), dRow(0).Item(2))
                        tmpMovieDb.NfoPath = sNFO
                        tmpMovie = NFO.LoadMovieFromNFO(sNFO, dRow(0).Item(2))
                    Else
                        tmpMovie = NFO.LoadMovieFromNFO(dRow(0).Item(42), dRow(0).Item(2))
                    End If

                    If String.IsNullOrEmpty(tmpMovie.Title) Then
                        If Directory.GetParent(dRow(0).Item(1)).Name.ToLower = "video_ts" Then
                            tmpMovieDb.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(dRow(0).Item(1)).FullName).Name)
                        Else
                            If dRow(0).Item(46) AndAlso dRow(0).Item(2) Then
                                tmpMovieDb.ListTitle = StringManip.FilterName(Directory.GetParent(dRow(0).Item(1)).Name)
                            Else
                                tmpMovieDb.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(dRow(0).Item(1)))
                            End If
                        End If
                    Else
                        If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(tmpMovie.Year) Then
                            tmpMovieDb.ListTitle = String.Format("{0} ({1})", StringManip.FilterTokens(tmpMovie.Title), tmpMovie.Year)
                        Else
                            tmpMovieDb.ListTitle = StringManip.FilterTokens(tmpMovie.Title)
                        End If
                    End If

                    Me.Invoke(myDelegate, New Object() {dRow(0), 3, tmpMovieDb.ListTitle})
                    Me.Invoke(myDelegate, New Object() {dRow(0), 15, tmpMovie.Title})

                    tmpMovieDb.Movie = tmpMovie
                Else
                    tmpMovieDb = Master.DB.LoadMovieFromDB(ID)
                End If

                'update genre
                Me.Invoke(myDelegate, New Object() {dRow(0), 26, tmpMovieDb.Movie.Genre})

                tmpMovieDb.Filename = dRow(0).Item(1)
                tmpMovieDb.isSingle = dRow(0).Item(2)
                tmpMovieDb.UseFolder = dRow(0).Item(46)
                tmpMovieDb.Source = dRow(0).Item(12)
                aContents = Master.GetFolderContents(dRow(0).Item(1), dRow(0).Item(2))
                tmpMovieDb.PosterPath = aContents(0)
                Me.Invoke(myDelegate, New Object() {dRow(0), 4, If(String.IsNullOrEmpty(aContents(0)), False, True)})
                tmpMovieDb.FanartPath = aContents(1)
                Me.Invoke(myDelegate, New Object() {dRow(0), 5, If(String.IsNullOrEmpty(aContents(1)), False, True)})
                'assume invalid nfo if no title
                tmpMovieDb.NfoPath = If(String.IsNullOrEmpty(tmpMovieDb.Movie.Title), String.Empty, aContents(2))
                Me.Invoke(myDelegate, New Object() {dRow(0), 6, If(String.IsNullOrEmpty(tmpMovieDb.NfoPath), False, True)})
                tmpMovieDb.TrailerPath = aContents(3)
                Me.Invoke(myDelegate, New Object() {dRow(0), 7, If(String.IsNullOrEmpty(aContents(3)), False, True)})
                tmpMovieDb.SubPath = aContents(4)
                Me.Invoke(myDelegate, New Object() {dRow(0), 8, If(String.IsNullOrEmpty(aContents(4)), False, True)})
                tmpMovieDb.ExtraPath = aContents(5)
                Me.Invoke(myDelegate, New Object() {dRow(0), 9, If(String.IsNullOrEmpty(aContents(5)), False, True)})

                tmpMovieDb.ID = dRow(0).Item(0)
                tmpMovieDb.IsMark = dRow(0).Item(11)
                tmpMovieDb.IsLock = dRow(0).Item(14)

                Master.DB.SaveMovieToDB(tmpMovieDb, False, BatchMode, ToNfo)

                aContents = Nothing
            Else
                Master.DB.DeleteFromDB(dRow(0).Item(0), BatchMode)
                Return True
            End If

            If Not BatchMode Then
                Me.SetFilterColors(True)
                Me.LoadInfo(dRow(0).Item(0), dRow(0).Item(1), True, False)
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
                        tsbUpdateXBMC.DropDownItems.Add(String.Format(Master.eLang.GetString(143, "Update {0} Only"), xCom.Name), Nothing, New System.EventHandler(AddressOf XComSubClick))
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
                            Me.btnMarkAll.Text = Master.eLang.GetString(105, "Unmark All")
                        Else
                            Me.btnMarkAll.Text = Master.eLang.GetString(35, "Mark All")
                        End If
                    End Using
                End Using

                Me.tsbRefreshMedia.DropDownItems.Clear()
                Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLNewcommand.CommandText = String.Concat("SELECT Name FROM Sources;")
                    Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                        While SQLReader.Read
                            Me.tsbRefreshMedia.DropDownItems.Add(String.Format(Master.eLang.GetString(143, "Update {0} Only"), SQLReader("Name")), Nothing, New System.EventHandler(AddressOf SourceSubClick))
                        End While
                    End Using
                End Using

                GenreListToolStripComboBox.Items.Clear()
                Me.clbFilterGenres.Items.Clear()
                Dim lGenre() As Object = XML.GetGenreList
                GenreListToolStripComboBox.Items.AddRange(lGenre)
                clbFilterGenres.Items.AddRange(lGenre)

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
                    cbFilterSource.SelectedIndex = 0
                    AddHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged

                    RemoveHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged
                    Me.cbFilterYear.Items.Clear()
                    cbFilterYear.Items.Add("All")
                    For i As Integer = (Year(Today) + 1) To 1888 Step -1
                        Me.cbFilterYear.Items.Add(i)
                    Next
                    cbFilterYear.SelectedIndex = 0
                    AddHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged

                    RemoveHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged
                    cbFilterYearMod.SelectedIndex = 0
                    AddHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged

                End If
            End With

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SetFilterColors(ByVal DoTitleCheck As Boolean)

        Try
            Dim LevFail As Boolean = False

            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing
            If DoTitleCheck Then SQLtransaction = Master.DB.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "UPDATE movies SET OutOfTolerance = (?) WHERE ID = (?);"
                Dim parOutOfTolerance As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parOutOfTolerance", DbType.Boolean, 0, "OutOfTolerance")
                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "ID")
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
                        If Me.chkFilterMark.Checked Then
                            drvRow.Selected = False
                            Me.dgvMediaList.CurrentCell = Nothing
                            If Me.dgvMediaList.RowCount <= 0 Then Me.ClearInfo()
                            Me.dgvMediaList.ClearSelection()
                        End If
                    End If

                    If Master.eSettings.LevTolerance > 0 AndAlso DoTitleCheck Then
                        Dim pExt As String = String.Empty
                        Dim pTitle As String = String.Empty
                        If Directory.GetParent(drvRow.Cells(1).Value).Name.ToLower = "video_ts" Then
                            pTitle = Directory.GetParent(Directory.GetParent(drvRow.Cells(1).Value).FullName).Name
                        Else
                            pExt = Path.GetExtension(drvRow.Cells(1).Value).ToLower
                            If drvRow.Cells(46).Value AndAlso drvRow.Cells(2).Value Then
                                pTitle = Directory.GetParent(drvRow.Cells(1).Value).Name
                            Else
                                pTitle = Path.GetFileNameWithoutExtension(drvRow.Cells(1).Value)
                            End If
                        End If

                        LevFail = Not pExt = ".vob" AndAlso Not pExt = ".ifo" AndAlso _
                                  StringManip.ComputeLevenshtein(StringManip.FilterName(drvRow.Cells(15).Value, False).ToLower, StringManip.FilterName(pTitle, False).ToLower) > Master.eSettings.LevTolerance

                        parOutOfTolerance.Value = LevFail
                        drvRow.Cells(47).Value = LevFail
                        parID.Value = drvRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                    Else
                        LevFail = drvRow.Cells(47).Value
                    End If

                    If drvRow.Cells(14).Value Then
                        drvRow.Cells(3).Style.BackColor = Color.LightSteelBlue
                        drvRow.Cells(4).Style.BackColor = Color.LightSteelBlue
                        drvRow.Cells(5).Style.BackColor = Color.LightSteelBlue
                        drvRow.Cells(6).Style.BackColor = Color.LightSteelBlue
                        drvRow.Cells(7).Style.BackColor = Color.LightSteelBlue
                        drvRow.Cells(8).Style.BackColor = Color.LightSteelBlue
                        drvRow.Cells(9).Style.BackColor = Color.LightSteelBlue
                        drvRow.Cells(3).Style.SelectionBackColor = Color.DarkTurquoise
                        drvRow.Cells(4).Style.SelectionBackColor = Color.DarkTurquoise
                        drvRow.Cells(5).Style.SelectionBackColor = Color.DarkTurquoise
                        drvRow.Cells(6).Style.SelectionBackColor = Color.DarkTurquoise
                        drvRow.Cells(7).Style.SelectionBackColor = Color.DarkTurquoise
                        drvRow.Cells(8).Style.SelectionBackColor = Color.DarkTurquoise
                        drvRow.Cells(9).Style.SelectionBackColor = Color.DarkTurquoise
                    ElseIf LevFail Then
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
            End Using
            If DoTitleCheck Then SQLtransaction.Commit()

            Me.tabMovies.Text = String.Format("{0} ({1})", Master.eLang.GetString(36, "Movies"), Me.dgvMediaList.RowCount)
            Me.dgvMediaList.Invalidate()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub EnableFilters(ByVal isEnabled As Boolean)
        Me.txtSearch.Enabled = isEnabled
        Me.cbSearch.Enabled = isEnabled
        Me.chkFilterDupe.Enabled = isEnabled
        Me.chkFilterTolerance.Enabled = If(Master.eSettings.LevTolerance > 0, isEnabled, False)
        Me.chkFilterMissing.Enabled = isEnabled
        Me.chkFilterMark.Enabled = isEnabled
        Me.chkFilterNew.Enabled = isEnabled
        Me.chkFilterLock.Enabled = isEnabled
        Me.rbFilterOr.Enabled = isEnabled
        Me.rbFilterAnd.Enabled = isEnabled
        Me.cbFilterSource.Enabled = isEnabled
        Me.txtFilterGenre.Enabled = isEnabled
        Me.cbFilterYearMod.Enabled = isEnabled
        Me.cbFilterYear.Enabled = isEnabled
        Me.btnClearFilters.Enabled = isEnabled
    End Sub

    Private Sub ClearFilters(Optional ByVal Reload As Boolean = False)

        Me.bsMedia.RemoveFilter()
        Me.FilterArray.Clear()
        Me.filSearch = String.Empty
        Me.filGenre = String.Empty
        Me.filYear = String.Empty

        RemoveHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
        Me.txtSearch.Text = String.Empty
        AddHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
        Me.cbSearch.SelectedIndex = 0
        Me.chkFilterDupe.Checked = False
        Me.chkFilterTolerance.Checked = False
        Me.chkFilterMissing.Checked = False
        Me.chkFilterMark.Checked = False
        Me.chkFilterNew.Checked = False
        Me.chkFilterLock.Checked = False
        Me.rbFilterOr.Checked = False
        Me.rbFilterAnd.Checked = True
        Me.txtFilterGenre.Text = String.Empty
        For i As Integer = 0 To Me.clbFilterGenres.Items.Count - 1
            Me.clbFilterGenres.SetItemChecked(i, False)
        Next
        RemoveHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged
        Me.cbFilterSource.SelectedIndex = 0
        AddHandler cbFilterSource.SelectedIndexChanged, AddressOf cbFilterSource_SelectedIndexChanged
        RemoveHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged
        Me.cbFilterYear.SelectedIndex = 0
        AddHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged
        RemoveHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged
        Me.cbFilterYearMod.SelectedIndex = 0
        AddHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged

        If Reload Then Me.FillList(0)
    End Sub

    Private Sub RunFilter(Optional ByVal doFill As Boolean = False)

        Try
            If Me.Visible Then

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

                If Me.chkFilterDupe.Checked Then
                    Me.FillList(0, True)
                ElseIf Not String.IsNullOrEmpty(Me.filSearch) AndAlso isActorSearch Then
                    Me.FillList(0, False, Me.filSearch)
                Else
                    If doFill Then
                        Me.FillList(0)
                    Else
                        Me.SetFilterColors(False)
                    End If
                End If

            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SourceSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim SourceName As String = sender.ToString.Replace(Master.eLang.GetString(144, "Update "), String.Empty).Replace(Master.eLang.GetString(145, " Only"), String.Empty).Trim
        If Not String.IsNullOrEmpty(SourceName) Then
            Me.LoadMedia(1, SourceName)
        End If
    End Sub

    Private Sub XComSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim xComName As String = sender.ToString.Replace(Master.eLang.GetString(144, "Update "), String.Empty).Replace(Master.eLang.GetString(145, " Only"), String.Empty).Trim
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
                    MsgBox(String.Format(Master.eLang.GetString(146, "There was a problem communicating with {0}{1}. Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), xCom.Name, vbNewLine), MsgBoxStyle.Exclamation, String.Format(Master.eLang.GetString(147, "Unable to Start XBMC Update for {0}"), xCom.Name))
                End If
            End Using
            Wr = Nothing
        Catch
            MsgBox(String.Format(Master.eLang.GetString(146, "There was a problem communicating with {0}{1}. Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), xCom.Name, vbNewLine), MsgBoxStyle.Exclamation, String.Format(Master.eLang.GetString(147, "Unable to Start XBMC Update for {0}"), xCom.Name))
        End Try
    End Sub

    Private Sub SaveMovieList()
        Try
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

    Private Sub FillList(ByVal iIndex As Integer, Optional ByVal DupesOnly As Boolean = False, Optional ByVal Actor As String = "")
        Try
            Me.bsMedia.DataSource = Nothing
            Me.dgvMediaList.DataSource = Nothing
            Me.ClearInfo()
            Application.DoEvents()

            If Not String.IsNullOrEmpty(Actor) Then
                Master.DB.FillDataTable(Me.dtMedia, String.Concat("SELECT * FROM movies WHERE ID IN (SELECT MovieID FROM MoviesActors WHERE ActorName LIKE '%", Actor, "%') ORDER BY ListTitle COLLATE NOCASE;"))
            Else
                If DupesOnly Then
                    Master.DB.FillDataTable(Me.dtMedia, "SELECT * FROM movies WHERE imdb IN (SELECT imdb FROM movies WHERE imdb IS NOT NULL AND LENGTH(imdb) > 0 GROUP BY imdb HAVING ( COUNT(imdb) > 1 )) ORDER BY ListTitle COLLATE NOCASE;")
                Else
                    Master.DB.FillDataTable(Me.dtMedia, "SELECT * FROM movies ORDER BY ListTitle COLLATE NOCASE;")
                End If
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
                        .dgvMediaList.Columns(3).ToolTipText = Master.eLang.GetString(21, "Title")
                        .dgvMediaList.Columns(3).HeaderText = Master.eLang.GetString(21, "Title")
                        .dgvMediaList.Columns(4).Width = 20
                        .dgvMediaList.Columns(4).Resizable = True
                        .dgvMediaList.Columns(4).ReadOnly = True
                        .dgvMediaList.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                        .dgvMediaList.Columns(4).ToolTipText = Master.eLang.GetString(148, "Poster")
                        .dgvMediaList.Columns(5).Width = 20
                        .dgvMediaList.Columns(5).Resizable = True
                        .dgvMediaList.Columns(5).ReadOnly = True
                        .dgvMediaList.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieFanartCol
                        .dgvMediaList.Columns(5).ToolTipText = Master.eLang.GetString(149, "Fanart")
                        .dgvMediaList.Columns(6).Width = 20
                        .dgvMediaList.Columns(6).Resizable = True
                        .dgvMediaList.Columns(6).ReadOnly = True
                        .dgvMediaList.Columns(6).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(6).Visible = Not Master.eSettings.MovieInfoCol
                        .dgvMediaList.Columns(6).ToolTipText = Master.eLang.GetString(150, "Nfo")
                        .dgvMediaList.Columns(7).Width = 20
                        .dgvMediaList.Columns(7).Resizable = True
                        .dgvMediaList.Columns(7).ReadOnly = True
                        .dgvMediaList.Columns(7).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(7).Visible = Not Master.eSettings.MovieTrailerCol
                        .dgvMediaList.Columns(7).ToolTipText = Master.eLang.GetString(151, "Trailer")
                        .dgvMediaList.Columns(8).Width = 20
                        .dgvMediaList.Columns(8).Resizable = True
                        .dgvMediaList.Columns(8).ReadOnly = True
                        .dgvMediaList.Columns(8).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(8).Visible = Not Master.eSettings.MovieSubCol
                        .dgvMediaList.Columns(8).ToolTipText = Master.eLang.GetString(152, "Subtitles")
                        .dgvMediaList.Columns(9).Width = 20
                        .dgvMediaList.Columns(9).Resizable = True
                        .dgvMediaList.Columns(9).ReadOnly = True
                        .dgvMediaList.Columns(9).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(9).Visible = Not Master.eSettings.MovieExtraCol
                        .dgvMediaList.Columns(9).ToolTipText = Master.eLang.GetString(153, "Extrathumbs")
                        For i As Integer = 10 To .dgvMediaList.Columns.Count - 1
                            .dgvMediaList.Columns(i).Visible = False
                        Next

                        'Trick to autosize the first column, but still allow resizing by user
                        .dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        .dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                        .dgvMediaList.Sort(.dgvMediaList.Columns(3), ComponentModel.ListSortDirection.Ascending)

                        If .dgvMediaList.RowCount > 0 Then
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

            Me.SetFilterColors(True)
            Me.EnableFilters(True)

        End If
    End Sub

    Public Sub SetListItemAfterEdit(ByVal iID As Integer, ByVal iRow As Integer)

        Try
            Dim dRow = From drvRow As DataRow In dtMedia.Rows Where drvRow.Item(0) = iID Select drvRow

            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT mark FROM movies WHERE id = ", iID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    dRow(0).Item(11) = SQLreader("mark")
                End Using
            End Using

            Me.SelectRow(iRow)
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
                Me.tslStatus.Text = Master.currMovie.Filename
            Else
                Me.pnlNoInfo.Visible = False

                Me.LoadInfo(Me.dgvMediaList.Item(0, iRow).Value, Me.dgvMediaList.Item(1, iRow).Value.ToString, True, False)
            End If
            ''''
            Me.mnuMediaList.Enabled = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
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