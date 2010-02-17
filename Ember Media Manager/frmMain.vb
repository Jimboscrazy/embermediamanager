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
    Friend WithEvents bwMediaInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwLoadInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwLoadShowInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwLoadSeasonInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwLoadEpInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwDownloadPic As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwScraper As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwRefreshMovies As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwCleanDB As New System.ComponentModel.BackgroundWorker

    'Private ExternalModulesManager As ModulesManager
    Private bsMedia As New BindingSource
    Private bsShows As New BindingSource
    Private bsSeasons As New BindingSource
    Private bsEpisodes As New BindingSource
    Private alActors As New List(Of String)
    Private aniType As Integer = 0 '0 = down, 1 = mid, 2 = up
    Private aniShowType As Integer = 0 '0 = down, 1 = mid, 2 = up
    Private aniRaise As Boolean = False
    Private aniFilterRaise As Boolean = False
    Private MainPoster As New Images
    Private MainFanart As New Images
    Private pbGenre() As PictureBox = Nothing
    Private pnlGenre() As Panel = Nothing
    Private tmpTitle As String = String.Empty
    Private tmpTVDB As String = String.Empty
    Private ReportDownloadPercent As Boolean = False
    Private IMDB As New IMDB.Scraper
    Private fScanner As New Scanner
    Private dtMedia As New DataTable
    Private dtShows As New DataTable
    Private dtSeasons As New DataTable
    Private dtEpisodes As New DataTable
    Private currText As String = String.Empty
    Private prevText As String = String.Empty
    Private FilterArray As New List(Of String)
    Private ScraperDone As Boolean = False
    Private LoadingDone As Boolean = False
    Private GenreImage As Image
    Private InfoCleared As Boolean = False
    Private isCL As Boolean = False
    Private sHTTP As New HTTP

    'Loading Delays
    Private currRow As Integer = -1
    Private prevRow As Integer = -1
    Private currShowRow As Integer = -1
    Private prevShowRow As Integer = -1
    Private currSeasonRow As Integer = -1
    Private prevSeasonRow As Integer = -1
    Private currEpRow As Integer = -1
    Private prevEpRow As Integer = -1

    'Theme Information
    Private _postermaxheight As Integer = 160
    Private _postermaxwidth As Integer = 160
    Private _genrepanelcolor As Color = Color.Gainsboro
    Private _ipup As Integer = 500
    Private _ipmid As Integer = 280
    Private tTheme As New Theming
    Private currThemeType As Theming.ThemeType

    'filters
    Private filSearch As String = String.Empty
    Private filGenre As String = String.Empty
    Private filYear As String = String.Empty
    Private filMissing As String = String.Empty
    Private filSource As String = String.Empty

    Private Structure Results
        Dim scrapeType As Enums.ScrapeType
        Dim Options As Structures.ScrapeOptions
        Dim fileInfo As String
        Dim setEnabled As Boolean
        Dim Movie As Structures.DBMovie
        Dim Path As String
        Dim Result As Image
    End Structure

    Private Structure Arguments
        Dim setEnabled As Boolean
        Dim scrapeType As Enums.ScrapeType
        Dim Options As Structures.ScrapeOptions
        Dim pURL As String
        Dim Path As String
        Dim Movie As Structures.DBMovie
        Dim ID As Integer
        Dim Season As Integer
    End Structure

    Public Property PosterMaxWidth() As Integer
        Get
            Return _postermaxwidth
        End Get
        Set(ByVal value As Integer)
            _postermaxwidth = value
        End Set
    End Property

    Public Property PosterMaxHeight() As Integer
        Get
            Return _postermaxheight
        End Get
        Set(ByVal value As Integer)
            _postermaxheight = value
        End Set
    End Property

    Public Property IPUp() As Integer
        Get
            Return _ipup
        End Get
        Set(ByVal value As Integer)
            _ipup = value
        End Set
    End Property

    Public Property IPMid() As Integer
        Get
            Return _ipmid
        End Get
        Set(ByVal value As Integer)
            _ipmid = value
        End Set
    End Property

    Public Property GenrePanelColor() As Color
        Get
            Return _genrepanelcolor
        End Get
        Set(ByVal value As Color)
            _genrepanelcolor = value
        End Set
    End Property

#End Region '*** Declarations


#Region "Form/Controls"

    Private Sub frmMain_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged

    End Sub

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
            RemoveGenreToolStripMenuItem.Enabled = GenreListToolStripComboBox.Tag.ToString.Contains(GenreListToolStripComboBox.Text)
            AddGenreToolStripMenuItem.Enabled = Not GenreListToolStripComboBox.Tag.ToString.Contains(GenreListToolStripComboBox.Text)
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
            If Not isCL Then
                Master.eSettings.WindowLoc = Me.Location
                Master.eSettings.WindowSize = Me.Size
                Master.eSettings.WindowState = Me.WindowState
                Master.eSettings.InfoPanelState = Me.aniType
                Master.eSettings.ShowInfoPanelState = Me.aniShowType
                Master.eSettings.FilterPanelState = Me.aniFilterRaise
                Master.eSettings.SpliterPanelState = Me.scMain.SplitterDistance
            End If
            If Not Me.WindowState = FormWindowState.Minimized Then Master.eSettings.Save()

            If Me.fScanner.IsBusy OrElse isCL Then
                doSave = False
            End If

            If Me.fScanner.IsBusy Then Me.fScanner.Cancel()
            If Me.bwMediaInfo.IsBusy Then Me.bwMediaInfo.CancelAsync()
            If Me.bwLoadInfo.IsBusy Then Me.bwLoadInfo.CancelAsync()
            If Me.bwLoadShowInfo.IsBusy Then Me.bwLoadShowInfo.CancelAsync()
            If Me.bwLoadSeasonInfo.IsBusy Then Me.bwLoadSeasonInfo.CancelAsync()
            If Me.bwLoadEpInfo.IsBusy Then Me.bwLoadEpInfo.CancelAsync()
            If Me.bwDownloadPic.IsBusy Then Me.bwDownloadPic.CancelAsync()
            If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()
            If Me.bwRefreshMovies.IsBusy Then Me.bwRefreshMovies.CancelAsync()
            If Me.bwCleanDB.IsBusy Then Me.bwCleanDB.CancelAsync()
            If Master.TVScraper.IsBusy Then Master.TVScraper.Cancel()

            lblCanceling.Text = Master.eLang.GetString(99, "Canceling All Processes...")
            btnCancel.Visible = False
            lblCanceling.Visible = True
            pbCanceling.Visible = True
            pnlCancel.Visible = True
            Me.Refresh()

            While Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy _
            OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy _
            OrElse Me.bwCleanDB.IsBusy OrElse Me.bwLoadShowInfo.IsBusy OrElse Me.bwLoadEpInfo.IsBusy _
            OrElse Me.bwLoadSeasonInfo.IsBusy OrElse Master.TVScraper.IsBusy
                Application.DoEvents()
            End While

            If doSave Then Master.DB.SaveMovieList()

            If Not isCL Then Master.DB.Close()

            If Not Master.eSettings.PersistImgCache Then
                Me.ClearCache()
            End If
        Catch
            'force close
            Application.Exit()
        End Try

    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()
                ImageUtils.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)
                Me.pnlNoInfo.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), Convert.ToInt32((Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2))
                Me.pnlCancel.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), 100)
                Me.pnlFilterGenre.Location = New Point(Me.gbSpecific.Left + Me.txtFilterGenre.Left, (Me.pnlFilter.Top + Me.txtFilterGenre.Top + Me.gbSpecific.Top) - Me.pnlFilterGenre.Height)
                Me.pnlFilterSource.Location = New Point(Me.gbSpecific.Left + Me.txtFilterSource.Left, (Me.pnlFilter.Top + Me.txtFilterSource.Top + Me.gbSpecific.Top) - Me.pnlFilterSource.Height)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click

        '//
        ' Show the settings
        '\\

        Try
            Using dSettings As New dlgSettings
                Dim dResult As Structures.SettingsResult = dSettings.ShowDialog
                If Not dResult.DidCancel Then

                    Me.SetUp(False)

                    If Me.dgvMediaList.RowCount > 0 Then
                        Me.dgvMediaList.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                        Me.dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieFanartCol
                        Me.dgvMediaList.Columns(6).Visible = Not Master.eSettings.MovieInfoCol
                        Me.dgvMediaList.Columns(7).Visible = Not Master.eSettings.MovieTrailerCol
                        Me.dgvMediaList.Columns(8).Visible = Not Master.eSettings.MovieSubCol
                        Me.dgvMediaList.Columns(9).Visible = Not Master.eSettings.MovieExtraCol
                    End If

                    'might as well wait for these
                    While Me.bwMediaInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy
                        Application.DoEvents()
                    End While

                    If dResult.NeedsRefresh OrElse dResult.NeedsUpdate Then
                        If dResult.NeedsRefresh Then
                            If Not Me.fScanner.IsBusy Then
                                While Me.bwLoadInfo.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwCleanDB.IsBusy
                                    Application.DoEvents()
                                End While
                                Me.RefreshAllMovies()
                            End If
                        End If
                        If dResult.NeedsUpdate Then
                            If Not Me.fScanner.IsBusy Then
                                While Me.bwLoadInfo.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwCleanDB.IsBusy
                                    Application.DoEvents()
                                End While
                                Me.LoadMedia(New Structures.Scans With {.Movies = True, .TV = True})
                            End If
                        End If
                    Else
                        If Not Me.fScanner.IsBusy AndAlso Not Me.bwLoadInfo.IsBusy AndAlso Not Me.bwScraper.IsBusy AndAlso Not Me.bwRefreshMovies.IsBusy AndAlso Not Me.bwCleanDB.IsBusy Then
                            Me.FillList(0)
                        End If
                    End If

                    Me.SetMenus(True)
                Else
                    Me.SetMenus(False)
                End If
            End Using

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim sPath As String = String.Concat(Functions.AppPath, "Log", Path.DirectorySeparatorChar, "errlog.txt")
        If File.Exists(sPath) Then
            If File.Exists(sPath.Insert(sPath.LastIndexOf("."), "-old")) Then File.Delete(sPath.Insert(sPath.LastIndexOf("."), "-old"))
            FileUtils.Common.MoveFileWithStream(sPath, sPath.Insert(sPath.LastIndexOf("."), "-old"))
            File.Delete(sPath)
        End If

        Master.eSettings.Load()
        Master.eLang.LoadLanguage(Master.eSettings.Language)
        Functions.CreateDefaultOptions()
        '//
        ' Add our handlers, load settings, set form colors, and try to load movies at startup
        '\\

        Me.Visible = False
        Dim Args() As String = Environment.GetCommandLineArgs
        'Setup/Load Modules Manager and set runtime objects (ember application) so they can be exposed to modules
        'ExternalModulesManager = New ModulesManager
        ModulesManager.Instance.RuntimeObjects.MenuMediaList = Me.mnuMediaList
        ModulesManager.Instance.RuntimeObjects.MediaList = Me.dgvMediaList
        ModulesManager.Instance.LoadAllModules()
        'setup some dummies so we don't get exceptions when resizing form/info panel
        ReDim Preserve Me.pnlGenre(0)
        ReDim Preserve Me.pbGenre(0)
        Me.pnlGenre(0) = New Panel()
        Me.pbGenre(0) = New PictureBox()

        AddHandler IMDB.MovieInfoDownloaded, AddressOf MovieInfoDownloaded
        AddHandler IMDB.ProgressUpdated, AddressOf MovieInfoDownloadedPercent
        AddHandler fScanner.ScannerUpdated, AddressOf ScannerUpdated
        AddHandler fScanner.ScanningCompleted, AddressOf ScanningCompleted
        AddHandler Master.TVScraper.ScraperEvent, AddressOf TVScraperEvent

        Functions.DGVDoubleBuffer(Me.dgvMediaList)
        Functions.DGVDoubleBuffer(Me.dgvTVShows)
        Functions.DGVDoubleBuffer(Me.dgvTVSeasons)
        Functions.DGVDoubleBuffer(Me.dgvTVEpisodes)

        'old place of log stuff

        If Not Directory.Exists(Master.TempPath) Then Directory.CreateDirectory(Master.TempPath)


        If Args.Count > 1 Then
            Dim MoviePath As String = String.Empty
            Dim isSingle As Boolean = False
            Dim hasSpec As Boolean = False
            Dim clScrapeType As Enums.ScrapeType = Nothing
            isCL = True
            Dim clExport As Boolean = False
            Dim clExportResizePoster As Integer = 0
            Dim clExportTemplate As String = "template"
            Dim clAsk As Boolean = False
            For i As Integer = 1 To Args.Count - 1

                Select Case Args(i).ToLower
                    Case "-fullask"
                        clScrapeType = Enums.ScrapeType.FullAsk
                        clAsk = True
                    Case "-fullauto"
                        clScrapeType = Enums.ScrapeType.FullAuto
                        clAsk = False
                    Case "-missask"
                        clScrapeType = Enums.ScrapeType.UpdateAsk
                        clAsk = True
                    Case "-missauto"
                        clScrapeType = Enums.ScrapeType.UpdateAuto
                        clAsk = False
                    Case "-newask"
                        clScrapeType = Enums.ScrapeType.NewAsk
                        clAsk = True
                    Case "-newauto"
                        clScrapeType = Enums.ScrapeType.NewAuto
                        clAsk = False
                    Case "-markask"
                        clScrapeType = Enums.ScrapeType.MarkAsk
                        clAsk = True
                    Case "-markauto"
                        clScrapeType = Enums.ScrapeType.MarkAuto
                        clAsk = False
                    Case "-file"
                        If Args.Count - 1 > i Then
                            isSingle = False
                            hasSpec = True
                            clScrapeType = Enums.ScrapeType.SingleScrape
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
                            clScrapeType = Enums.ScrapeType.SingleScrape
                            If File.Exists(Args(i + 1).Replace("""", String.Empty)) Then
                                MoviePath = Args(i + 1).Replace("""", String.Empty)
                                i += 1
                            End If
                        Else
                            Exit For
                        End If
                    Case "-export"
                        If Args.Count - 1 > i Then
                            MoviePath = Args(i + 1).Replace("""", String.Empty)
                            clExport = True
                        Else
                            Exit For
                        End If
                    Case "-template"
                        If Args.Count - 1 > i Then
                            clExportTemplate = Args(i + 1).Replace("""", String.Empty)
                        Else
                            Exit For
                        End If
                    Case "-resize"
                        If Args.Count - 1 > i Then
                            clExportResizePoster = Convert.ToUInt16(Args(i + 1).Replace("""", String.Empty))
                        Else
                            Exit For
                        End If
                    Case "-all"
                        Functions.SetScraperMod(Enums.ModType.All, True)
                    Case "-nfo"
                        Functions.SetScraperMod(Enums.ModType.NFO, True)
                    Case "-posters"
                        Functions.SetScraperMod(Enums.ModType.Poster, True)
                    Case "-fanart"
                        Functions.SetScraperMod(Enums.ModType.Fanart, True)
                    Case "-extra"
                        Functions.SetScraperMod(Enums.ModType.Extra, True)
                    Case "--verbose"
                        clAsk = True
                    Case Else
                        'If File.Exists(Args(2).Replace("""", String.Empty)) Then
                        'MoviePath = Args(2).Replace("""", String.Empty)
                        'End If
                End Select
            Next
            APIXML.CacheXMLs()
            Master.DB.Connect(False, False)
            If clExport = True Then
                dlgExportMovies.CLExport(MoviePath, clExportTemplate, clExportResizePoster)
            End If
            If Not IsNothing(clScrapeType) Then
                If Functions.HasModifier AndAlso Not clScrapeType = Enums.ScrapeType.SingleScrape Then
                    Try
                        LoadMedia(New Structures.Scans With {.Movies = True})
                        While Not Me.LoadingDone
                            Application.DoEvents()
                        End While
                        ScrapeData(clScrapeType, Master.DefaultOptions, Nothing, clAsk)
                    Catch ex As Exception
                        ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                    End Try
                Else
                    Try

                        If Not String.IsNullOrEmpty(MoviePath) AndAlso hasSpec Then
                            Master.currMovie = Master.DB.LoadMovieFromDB(MoviePath)
                            Me.tmpTitle = StringUtils.FilterName(If(isSingle, Directory.GetParent(MoviePath).Name, Path.GetFileNameWithoutExtension(MoviePath)))
                            If Master.currMovie.Movie Is Nothing Then
                                Master.currMovie.Movie = New MediaContainers.Movie
                                Master.currMovie.Movie.Title = tmpTitle
                                Dim sFile As New Scanner.MovieContainer
                                sFile.Filename = MoviePath
                                sFile.isSingle = isSingle
                                sFile.UseFolder = If(isSingle, True, False)
                                fScanner.GetMovieFolderContents(sFile)
                                If Not String.IsNullOrEmpty(sFile.Nfo) Then
                                    Master.currMovie.Movie = NFO.LoadMovieFromNFO(sFile.Nfo, sFile.isSingle)
                                Else
                                    Master.currMovie.Movie = NFO.LoadMovieFromNFO(sFile.Filename, sFile.isSingle)
                                End If
                                If String.IsNullOrEmpty(Master.currMovie.Movie.Title) Then
                                    'no title so assume it's an invalid nfo, clear nfo path if exists
                                    sFile.Nfo = String.Empty
                                    If FileUtils.Common.isVideoTS(sFile.Filename) Then
                                        Master.currMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name)
                                    ElseIf FileUtils.Common.isBDRip(sFile.Filename) Then
                                        Master.currMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).FullName).Name)
                                    Else
                                        If sFile.UseFolder AndAlso sFile.isSingle Then
                                            Master.currMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(sFile.Filename).Name)
                                        Else
                                            Master.currMovie.ListTitle = StringUtils.FilterName(Path.GetFileNameWithoutExtension(sFile.Filename))
                                        End If
                                    End If
                                    If String.IsNullOrEmpty(Master.currMovie.Movie.SortTitle) Then Master.currMovie.Movie.SortTitle = Master.currMovie.ListTitle
                                Else
                                    Dim tTitle As String = StringUtils.FilterTokens(Master.currMovie.Movie.Title)
                                    If String.IsNullOrEmpty(Master.currMovie.Movie.SortTitle) Then Master.currMovie.Movie.SortTitle = tTitle
                                    If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(Master.currMovie.Movie.Year) Then
                                        Master.currMovie.ListTitle = String.Format("{0} ({1})", tTitle, Master.currMovie.Movie.Year)
                                    Else
                                        Master.currMovie.ListTitle = tTitle
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
                            Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, Nothing, clAsk)
                        Else
                            Me.ScraperDone = True
                        End If
                    Catch ex As Exception
                        Me.ScraperDone = True
                        ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                    End Try
                End If

                While Not Me.ScraperDone
                    Application.DoEvents()
                End While
            End If

            frmSplash.CloseForm()
            Me.Close()

        Else
            Try
                APIXML.CacheXMLs()

                Me.SetUp(True)
                Me.cbSearch.SelectedIndex = 0

                If Master.eSettings.CheckUpdates Then
                    Dim tmpNew As Integer = Functions.CheckUpdate
                    If tmpNew > Convert.ToInt32(My.Application.Info.Version.Revision) Then
                        Using dNewVer As New dlgNewVersion
                            dNewVer.ShowDialog(tmpNew)
                        End Using
                    End If
                End If

                Me.Location = Master.eSettings.WindowLoc
                Me.Size = Master.eSettings.WindowSize
                Me.WindowState = Master.eSettings.WindowState


                Me.aniType = Master.eSettings.InfoPanelState
                Select Case Me.aniType
                    Case 0
                        Me.pnlInfoPanel.Height = 25
                        Me.btnDown.Enabled = False
                        Me.btnMid.Enabled = True
                        Me.btnUp.Enabled = True
                    Case 1
                        Me.pnlInfoPanel.Height = Me.IPMid
                        Me.btnMid.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnUp.Enabled = True
                    Case 2
                        Me.pnlInfoPanel.Height = Me.IPUp
                        Me.btnUp.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnMid.Enabled = True
                End Select

                Me.aniShowType = Master.eSettings.ShowInfoPanelState

                Me.aniFilterRaise = Master.eSettings.FilterPanelState
                If Me.aniFilterRaise Then
                    Me.pnlFilter.Height = Functions.Quantize(Me.gbSpecific.Height + Me.lblFilter.Height + 15, 5)
                    Me.btnFilterDown.Enabled = True
                    Me.btnFilterUp.Enabled = False
                Else
                    Me.pnlFilter.Height = 25
                    Me.btnFilterDown.Enabled = False
                    Me.btnFilterUp.Enabled = True
                End If

                Me.scMain.SplitterDistance = Master.eSettings.SpliterPanelState

                Me.ClearInfo()

                Application.DoEvents()

                If Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision) Then
                    Master.DB.Connect(False, False)
                    Me.FillList(0)
                    Me.Visible = True
                Else
                    Master.DB.Connect(True, False)
                    If dlgWizard.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Me.SetUp(False) 'just in case user changed languages
                        Me.Visible = True
                        Me.LoadMedia(New Structures.Scans With {.Movies = True, .TV = True})
                    Else
                        Me.FillList(0)
                        Me.Visible = True
                    End If

                End If

                Me.SetMenus(True)
                Functions.GetListOfSources()

            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Me.Activate()

        End If

    End Sub

    Private Sub lstActors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstActors.SelectedIndexChanged
        '//
        ' Begin thread to download actor image if one exists
        '\\
        Try
            If Not Me.alActors.Item(Me.lstActors.SelectedIndex).ToString = "none" Then

                If Not IsNothing(Me.pbActors.Image) Then
                    Me.pbActors.Image.Dispose()
                    Me.pbActors.Image = Nothing
                End If

                Me.pbActLoad.Visible = True

                If Me.bwDownloadPic.IsBusy Then
                    Me.bwDownloadPic.CancelAsync()
                    While Me.bwDownloadPic.IsBusy
                        Application.DoEvents()
                    End While
                End If

                Me.bwDownloadPic = New System.ComponentModel.BackgroundWorker
                Me.bwDownloadPic.WorkerSupportsCancellation = True
                Me.bwDownloadPic.RunWorkerAsync(New Arguments With {.pURL = Me.alActors.Item(Me.lstActors.SelectedIndex).ToString})

            Else
                Me.pbActors.Image = My.Resources.actor_silhouette
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub tsbRefreshMedia_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRefreshMedia.ButtonClick

        '//
        ' Reload media type when "Rescan Media" is clicked
        '\\

        Me.LoadMedia(New Structures.Scans With {.Movies = True, .TV = True})

    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click

        '//
        ' Begin animation to raise panel all the way up
        '\\

        If Me.tabsMain.SelectedIndex = 0 Then
            Me.aniType = 2
        Else
            Me.aniShowType = 2
        End If
        Me.aniRaise = True
        Me.tmrAni.Start()

    End Sub


    Private Sub btnMid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMid.Click

        '//
        ' Begin animation to raise/lower panel to mid point
        '\\

        If Me.pnlInfoPanel.Height = Me.IPUp Then
            Me.aniRaise = False
        Else
            Me.aniRaise = True
        End If

        If Me.tabsMain.SelectedIndex = 0 Then
            Me.aniType = 1
        Else
            Me.aniShowType = 1
        End If

        Me.tmrAni.Start()

    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click

        '//
        ' Begin animation to lower panel all the way down
        '\\

        If Me.tabsMain.SelectedIndex = 0 Then
            Me.aniType = 0
        Else
            Me.aniShowType = 0
        End If
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
                Select Case If(Me.tabsMain.SelectedIndex = 0, Me.aniType, Me.aniShowType)
                    Case 0
                        Me.pnlInfoPanel.Height = 25

                    Case 1
                        Me.pnlInfoPanel.Height = Me.IPMid

                    Case 2
                        Me.pnlInfoPanel.Height = Me.IPUp

                End Select
            End If


            Me.MoveGenres()
            Me.MoveMPAA()

            Dim aType As Integer = If(Me.tabsMain.SelectedIndex = 0, Me.aniType, Me.aniShowType)
            Select Case aType
                Case 0
                    If Me.pnlInfoPanel.Height = 25 Then
                        Me.tmrAni.Stop()
                        Me.btnDown.Enabled = False
                        Me.btnMid.Enabled = True
                        Me.btnUp.Enabled = True
                    End If
                Case 1
                    If Me.pnlInfoPanel.Height = Me.IPMid Then
                        Me.tmrAni.Stop()
                        Me.btnMid.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnUp.Enabled = True
                    End If
                Case 2
                    If Me.pnlInfoPanel.Height = Me.IPUp Then
                        Me.tmrAni.Stop()
                        'Me.btnUp.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnMid.Enabled = True
                    End If
            End Select

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnMIRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMetaDataRefresh.Click

        '//
        ' Refresh Media Info
        '\\

        If Me.tabsMain.SelectedIndex = 0 Then
            Me.LoadInfo(Convert.ToInt32(Master.currMovie.ID), Master.currMovie.Filename, False, True, True)
        End If

    End Sub

    Private Sub dgvMediaList_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellClick
        If Me.dgvMediaList.SelectedRows.Count > 0 Then
            If Me.dgvMediaList.RowCount > 0 Then
                Me.tmpTitle = Me.dgvMediaList.SelectedRows(0).Cells(15).Value.ToString
                If Me.dgvMediaList.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvMediaList.SelectedRows.Count))
                ElseIf Me.dgvMediaList.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvMediaList.SelectedRows(0).Cells(1).Value.ToString)
                End If
            End If

            Me.currRow = Me.dgvMediaList.SelectedRows(0).Index
        End If
    End Sub

    Private Sub dgvTVShows_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVShows.CellClick
        If Me.dgvTVShows.SelectedRows.Count > 0 Then
            If Me.dgvTVShows.RowCount > 0 Then
                Me.tmpTitle = Me.dgvTVShows.SelectedRows(0).Cells(1).Value.ToString
                Me.tmpTVDB = Me.dgvTVShows.SelectedRows(0).Cells(9).Value.ToString
                If Me.dgvTVShows.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvTVShows.SelectedRows.Count))
                ElseIf Me.dgvTVShows.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvTVShows.SelectedRows(0).Cells(1).Value.ToString)
                End If
            End If

            Me.currShowRow = Me.dgvTVShows.SelectedRows(0).Index
        End If
    End Sub

    Private Sub dgvTVSeasons_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVSeasons.CellClick
        If Me.dgvTVSeasons.SelectedRows.Count > 0 Then
            If Me.dgvTVSeasons.RowCount > 0 Then
                If Me.dgvTVSeasons.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvTVSeasons.SelectedRows.Count))
                ElseIf Me.dgvTVSeasons.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvTVSeasons.SelectedRows(0).Cells(2).Value.ToString)
                End If
            End If

            Me.currSeasonRow = Me.dgvTVSeasons.SelectedRows(0).Index
        End If
    End Sub

    Private Sub dgvTVEpisodes_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVEpisodes.CellClick

        If Me.dgvTVEpisodes.SelectedRows.Count > 0 Then
            If Me.dgvTVEpisodes.RowCount > 0 Then
                If Me.dgvTVEpisodes.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvTVEpisodes.SelectedRows.Count))
                ElseIf Me.dgvTVEpisodes.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvTVEpisodes.SelectedRows(0).Cells(2).Value.ToString)
                End If
            End If

            Me.currEpRow = Me.dgvTVEpisodes.SelectedRows(0).Index
        End If
    End Sub

    Private Sub dgvMediaList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellDoubleClick

        Try

            If Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwCleanDB.IsBusy Then Return

            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
            Master.currMovie = Master.DB.LoadMovieFromDB(ID)
            Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString

            Using dEditMovie As New dlgEditMovie

                Select Case dEditMovie.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshMovie(ID) Then
                            Me.FillList(0)
                        End If
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
                    Case Else
                        If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value.ToString, True, False)
                End Select

            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMediaList.Sorted

        If Me.dgvMediaList.RowCount > 0 Then
            Me.dgvMediaList.Rows(0).Selected = True
            Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Rows(0).Cells(3)
        End If

    End Sub

    Private Sub dgvTVShows_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTVShows.Sorted

        If Me.dgvTVShows.RowCount > 0 Then
            Me.dgvTVShows.Rows(0).Selected = True
            Me.dgvTVShows.CurrentCell = Me.dgvTVShows.Rows(0).Cells(1)
        End If

    End Sub

    Private Sub dgvTVSeasons_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTVSeasons.Sorted

        If Me.dgvTVSeasons.RowCount > 0 Then
            Me.dgvTVSeasons.Rows(0).Selected = True
            Me.dgvTVSeasons.CurrentCell = Me.dgvTVSeasons.Rows(0).Cells(2)
        End If

    End Sub

    Private Sub dgvTVEpisodes_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTVEpisodes.Sorted

        If Me.dgvTVEpisodes.RowCount > 0 Then
            Me.dgvTVEpisodes.Rows(0).Selected = True
            Me.dgvTVEpisodes.CurrentCell = Me.dgvTVEpisodes.Rows(0).Cells(2)
        End If

    End Sub

    Private Sub pbGenre_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs)

        '//
        ' Draw genre text over the image when mouse hovers
        '\\

        Try
            If Master.eSettings.AllwaysDisplayGenresText Then Return
            Dim iLeft As Integer = 0
            Me.GenreImage = DirectCast(sender, PictureBox).Image
            Dim bmGenre As New Bitmap(Me.GenreImage)
            Dim grGenre As Graphics = Graphics.FromImage(bmGenre)
            Dim drawString As String = DirectCast(sender, PictureBox).Name.ToString
            Dim drawFont As New Font("Microsoft Sans Serif", 14, FontStyle.Bold, GraphicsUnit.Pixel)
            Dim drawBrush As New SolidBrush(Color.White)
            Dim drawWidth As Single = grGenre.MeasureString(drawString, drawFont).Width
            Dim drawSize As Integer = Convert.ToInt32((14 * (bmGenre.Width / drawWidth)) - 0.5)
            drawFont = New Font("Microsoft Sans Serif", If(drawSize > 14, 14, drawSize), FontStyle.Bold, GraphicsUnit.Pixel)
            Dim drawHeight As Single = grGenre.MeasureString(drawString, drawFont).Height
            iLeft = Convert.ToInt32((bmGenre.Width - grGenre.MeasureString(drawString, drawFont).Width) / 2)
            grGenre.DrawString(drawString, drawFont, drawBrush, iLeft, (bmGenre.Height - drawHeight))
            DirectCast(sender, PictureBox).Image = bmGenre
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub pbGenre_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)

        '//
        ' Reset genre image when mouse leaves to "clear" the text
        '\\

        Try
            If Master.eSettings.AllwaysDisplayGenresText Then Return
            DirectCast(sender, PictureBox).Image = GenreImage
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                    If Master.isWindows Then
                        System.Diagnostics.Process.Start(String.Concat("""", Me.txtFilePath.Text, """"))
                    Else
                        Using Explorer As New Diagnostics.Process
                            Explorer.StartInfo.FileName = "xdg-open"
                            Explorer.StartInfo.Arguments = String.Format("""{0}""", Me.txtFilePath.Text)
                            Explorer.Start()
                        End Using
                    End If

                End If
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub scMain_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles scMain.SplitterMoved

        '//
        ' Some generic resizing when pane sizes are changed
        '\\

        Try
            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()

                ImageUtils.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)
                Me.pnlNoInfo.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), Convert.ToInt32((Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2))
                Me.pnlCancel.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), 100)
                Me.pnlFilterGenre.Location = New Point(Me.gbSpecific.Left + Me.txtFilterGenre.Left, (Me.pnlFilter.Top + Me.txtFilterGenre.Top + Me.gbSpecific.Top) - Me.pnlFilterGenre.Height)
                Me.pnlFilterSource.Location = New Point(Me.gbSpecific.Left + Me.txtFilterSource.Left, (Me.pnlFilter.Top + Me.txtFilterSource.Top + Me.gbSpecific.Top) - Me.pnlFilterSource.Height)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbPoster_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbPoster.DoubleClick

        '//
        ' Show the Poster in the Image Viewer
        '\\

        Try
            If Not IsNothing(Me.pbPoster.Image) Then
                Using dImgView As New dlgImgView
                    dImgView.ShowDialog(Me.pbPosterCache.Image)
                End Using
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub pbFanart_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbFanart.DoubleClick

        '//
        ' Show the Fanart in the Image Viewer
        '\\

        Try
            If Not IsNothing(Me.pbFanartCache.Image) Then
                Using dImgView As New dlgImgView
                    dImgView.ShowDialog(Me.pbFanartCache.Image)
                End Using
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvMediaList.CellPainting

        Try
            'icons
            If e.ColumnIndex >= 4 AndAlso e.ColumnIndex <= 9 AndAlso e.RowIndex = -1 Then
                e.PaintBackground(e.ClipBounds, False)

                Dim pt As Point = e.CellBounds.Location
                Dim offset As Integer = Convert.ToInt32((e.CellBounds.Width - Me.ilColumnIcons.ImageSize.Width) / 2)

                pt.X += offset
                pt.Y = 1
                Me.ilColumnIcons.Draw(e.Graphics, pt, e.ColumnIndex - 4)

                e.Handled = True

            End If

            If e.ColumnIndex = 3 AndAlso e.RowIndex >= 0 Then
                If Convert.ToBoolean(Me.dgvMediaList.Item(11, e.RowIndex).Value) Then
                    e.CellStyle.ForeColor = Color.Crimson
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    e.CellStyle.SelectionForeColor = Color.Crimson
                ElseIf Convert.ToBoolean(Me.dgvMediaList.Item(10, e.RowIndex).Value) Then
                    e.CellStyle.ForeColor = Color.Green
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    e.CellStyle.SelectionForeColor = Color.Green
                Else
                    e.CellStyle.ForeColor = Color.Black
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
                    e.CellStyle.SelectionForeColor = Color.FromKnownColor(KnownColor.HighlightText)
                End If
            End If

            If e.ColumnIndex >= 3 AndAlso e.ColumnIndex <= 9 AndAlso e.RowIndex >= 0 Then

                If Convert.ToBoolean(Me.dgvMediaList.Item(14, e.RowIndex).Value) Then
                    e.CellStyle.BackColor = Color.LightSteelBlue
                    e.CellStyle.SelectionBackColor = Color.DarkTurquoise
                ElseIf Convert.ToBoolean(Me.dgvMediaList.Item(47, e.RowIndex).Value) Then
                    e.CellStyle.BackColor = Color.MistyRose
                    e.CellStyle.SelectionBackColor = Color.DarkMagenta
                Else
                    e.CellStyle.BackColor = Color.White
                    e.CellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                End If
            End If

            Me.tabMovies.Text = String.Format("{0} ({1})", Master.eLang.GetString(36, "Movies"), Me.dgvMediaList.RowCount)

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVShows_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvTVShows.CellPainting

        Try
            'icons
            If e.ColumnIndex >= 2 AndAlso e.ColumnIndex <= 4 AndAlso e.RowIndex = -1 Then
                e.PaintBackground(e.ClipBounds, False)

                Dim pt As Point = e.CellBounds.Location
                Dim offset As Integer = Convert.ToInt32((e.CellBounds.Width - Me.ilColumnIcons.ImageSize.Width) / 2)

                pt.X += offset
                pt.Y = 1
                Me.ilColumnIcons.Draw(e.Graphics, pt, e.ColumnIndex - 2)

                e.Handled = True

            End If

            If e.ColumnIndex = 1 AndAlso e.RowIndex >= 0 Then
                If Convert.ToBoolean(Me.dgvTVShows.Item(6, e.RowIndex).Value) Then
                    e.CellStyle.ForeColor = Color.Crimson
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    e.CellStyle.SelectionForeColor = Color.Crimson
                ElseIf Convert.ToBoolean(Me.dgvTVShows.Item(5, e.RowIndex).Value) Then
                    e.CellStyle.ForeColor = Color.Green
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    e.CellStyle.SelectionForeColor = Color.Green
                Else
                    e.CellStyle.ForeColor = Color.Black
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
                    e.CellStyle.SelectionForeColor = Color.FromKnownColor(KnownColor.HighlightText)
                End If
            End If

            If e.ColumnIndex >= 1 AndAlso e.ColumnIndex <= 4 AndAlso e.RowIndex >= 0 Then

                If Convert.ToBoolean(Me.dgvTVShows.Item(10, e.RowIndex).Value) Then
                    e.CellStyle.BackColor = Color.LightSteelBlue
                    e.CellStyle.SelectionBackColor = Color.DarkTurquoise
                Else
                    e.CellStyle.BackColor = Color.White
                    e.CellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                End If
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVSeasons_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvTVSeasons.CellPainting

        Try
            'icons
            If (e.ColumnIndex = 4 OrElse e.ColumnIndex = 5) AndAlso e.RowIndex = -1 Then
                e.PaintBackground(e.ClipBounds, False)

                Dim pt As Point = e.CellBounds.Location
                Dim offset As Integer = Convert.ToInt32((e.CellBounds.Width - Me.ilColumnIcons.ImageSize.Width) / 2)

                pt.X += offset
                pt.Y = 1
                Me.ilColumnIcons.Draw(e.Graphics, pt, e.ColumnIndex - 4)

                e.Handled = True

            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVEpisodes_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvTVEpisodes.CellPainting

        Try
            'icons
            If e.ColumnIndex >= 3 AndAlso e.ColumnIndex <= 5 AndAlso e.RowIndex = -1 Then
                e.PaintBackground(e.ClipBounds, False)

                Dim pt As Point = e.CellBounds.Location
                Dim offset As Integer = Convert.ToInt32((e.CellBounds.Width - Me.ilColumnIcons.ImageSize.Width) / 2)

                pt.X += offset
                pt.Y = 1

                Me.ilColumnIcons.Draw(e.Graphics, pt, e.ColumnIndex - 3)

                e.Handled = True
            End If

            If e.ColumnIndex = 2 AndAlso e.RowIndex >= 0 Then
                If Convert.ToBoolean(Me.dgvTVEpisodes.Item(7, e.RowIndex).Value) Then
                    e.CellStyle.ForeColor = Color.Crimson
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    e.CellStyle.SelectionForeColor = Color.Crimson
                ElseIf Convert.ToBoolean(Me.dgvTVEpisodes.Item(6, e.RowIndex).Value) Then
                    e.CellStyle.ForeColor = Color.Green
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    e.CellStyle.SelectionForeColor = Color.Green
                Else
                    e.CellStyle.ForeColor = Color.Black
                    e.CellStyle.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
                    e.CellStyle.SelectionForeColor = Color.FromKnownColor(KnownColor.HighlightText)
                End If
            End If

            If e.ColumnIndex >= 2 AndAlso e.ColumnIndex <= 5 AndAlso e.RowIndex >= 0 Then

                If Convert.ToBoolean(Me.dgvTVEpisodes.Item(10, e.RowIndex).Value) Then
                    e.CellStyle.BackColor = Color.LightSteelBlue
                    e.CellStyle.SelectionBackColor = Color.DarkTurquoise
                Else
                    e.CellStyle.BackColor = Color.White
                    e.CellStyle.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                End If
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellEnter

        Try
            Me.tmrWaitShow.Enabled = False
            Me.tmrWaitSeason.Enabled = False
            Me.tmrWaitEp.Enabled = False
            Me.tmrWait.Enabled = False
            Me.tmrLoadShow.Enabled = False
            Me.tmrLoadSeason.Enabled = False
            Me.tmrLoadEp.Enabled = False
            Me.tmrLoad.Enabled = False

            Me.currRow = e.RowIndex
            Me.tmrWait.Enabled = True

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVShows_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVShows.CellEnter

        Try
            If Me.dgvTVShows.Enabled Then
                Me.tmrWait.Enabled = False
                Me.tmrWaitSeason.Enabled = False
                Me.tmrWaitEp.Enabled = False
                Me.tmrWaitShow.Enabled = False
                Me.tmrLoad.Enabled = False
                Me.tmrLoadSeason.Enabled = False
                Me.tmrLoadEp.Enabled = False
                Me.tmrLoadShow.Enabled = False


                Me.currShowRow = e.RowIndex
                Me.tmrWaitShow.Enabled = True
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVSeasons_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVSeasons.CellEnter

        Try
            Me.tmrWaitShow.Enabled = False
            Me.tmrWait.Enabled = False
            Me.tmrWaitEp.Enabled = False
            Me.tmrWaitSeason.Enabled = False
            Me.tmrLoadShow.Enabled = False
            Me.tmrLoad.Enabled = False
            Me.tmrLoadEp.Enabled = False
            Me.tmrLoadSeason.Enabled = False


            Me.currSeasonRow = e.RowIndex
            Me.tmrWaitSeason.Enabled = True

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVEpisodes_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVEpisodes.CellEnter

        Try
            Me.tmrWaitShow.Enabled = False
            Me.tmrWaitSeason.Enabled = False
            Me.tmrWait.Enabled = False
            Me.tmrWaitEp.Enabled = False
            Me.tmrLoadShow.Enabled = False
            Me.tmrLoadSeason.Enabled = False
            Me.tmrLoad.Enabled = False
            Me.tmrLoadEp.Enabled = False


            Me.currEpRow = e.RowIndex
            Me.tmrWaitEp.Enabled = True

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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

                If Me.dgvMediaList.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvMediaList.SelectedRows.Count))
                ElseIf Me.dgvMediaList.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvMediaList.SelectedRows(0).Cells(1).Value.ToString)
                End If

                Me.SelectRow(Me.dgvMediaList.SelectedRows(0).Index)
            End If
        Catch
        End Try
    End Sub

    Private Sub tmrWaitShow_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrWaitShow.Tick
        Me.tmrLoadSeason.Enabled = False
        Me.tmrLoadEp.Enabled = False
        Me.tmrWaitSeason.Enabled = False
        Me.tmrWaitEp.Enabled = False

        If Me.prevShowRow = Me.currShowRow Then
            Me.tmrLoadShow.Enabled = True
        Else
            Me.prevShowRow = Me.currShowRow
            Me.tmrLoadShow.Enabled = False
        End If
    End Sub

    Private Sub tmrLoadShow_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLoadShow.Tick
        Me.tmrWaitShow.Enabled = False
        Me.tmrLoadShow.Enabled = False
        Try
            If Me.dgvTVShows.SelectedRows.Count > 0 Then

                If Me.dgvTVShows.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvTVShows.SelectedRows.Count))
                ElseIf Me.dgvTVShows.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvTVShows.SelectedRows(0).Cells(1).Value.ToString)
                End If

                Me.SelectShowRow(Me.dgvTVShows.SelectedRows(0).Index)
            End If
        Catch
        End Try
    End Sub

    Private Sub tmrWaitSeason_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrWaitSeason.Tick
        Me.tmrLoadShow.Enabled = False
        Me.tmrLoadEp.Enabled = False
        Me.tmrWaitShow.Enabled = False
        Me.tmrWaitEp.Enabled = False

        If Me.prevSeasonRow = Me.currSeasonRow Then
            Me.tmrLoadSeason.Enabled = True
        Else
            Me.prevSeasonRow = Me.currSeasonRow
            Me.tmrLoadSeason.Enabled = False
        End If
    End Sub

    Private Sub tmrLoadSeason_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLoadSeason.Tick
        Me.tmrWaitSeason.Enabled = False
        Me.tmrLoadSeason.Enabled = False
        Try
            If Me.dgvTVSeasons.SelectedRows.Count > 0 Then

                If Me.dgvTVSeasons.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvTVSeasons.SelectedRows.Count))
                ElseIf Me.dgvMediaList.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvTVSeasons.SelectedRows(0).Cells(2).Value.ToString)
                End If

                Me.SelectSeasonRow(Me.dgvTVSeasons.SelectedRows(0).Index)
            End If
        Catch
        End Try
    End Sub

    Private Sub tmrWaitEp_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrWaitEp.Tick
        Me.tmrLoadSeason.Enabled = False
        Me.tmrLoadShow.Enabled = False
        Me.tmrWaitSeason.Enabled = False
        Me.tmrWaitShow.Enabled = False

        If Me.prevEpRow = Me.currEpRow Then
            Me.tmrLoadEp.Enabled = True
        Else
            Me.prevEpRow = Me.currEpRow
            Me.tmrLoadEp.Enabled = False
        End If
    End Sub

    Private Sub tmrLoadEp_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLoadEp.Tick
        Me.tmrWaitEp.Enabled = False
        Me.tmrLoadEp.Enabled = False
        Try
            If Me.dgvTVEpisodes.SelectedRows.Count > 0 Then

                If Me.dgvTVEpisodes.SelectedRows.Count > 1 Then
                    Me.SetStatus(String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvTVEpisodes.SelectedRows.Count))
                ElseIf Me.dgvTVEpisodes.SelectedRows.Count = 1 Then
                    Me.SetStatus(Me.dgvTVEpisodes.SelectedRows(0).Cells(2).Value.ToString)
                End If

                Me.SelectEpisodeRow(Me.dgvTVEpisodes.SelectedRows(0).Index)
            End If
        Catch
        End Try
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
        e.Handled = StringUtils.AlphaNumericOnly(e.KeyChar, True)
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged

        Me.currText = Me.txtSearch.Text

        Me.tmrSearchWait.Enabled = False
        Me.tmrSearch.Enabled = False
        Me.tmrSearchWait.Enabled = True
    End Sub

    Private Sub tmrSearchWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSearchWait.Tick
        Me.tmrSearch.Enabled = False
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
                    Case Master.eLang.GetString(100, "Actor")
                        Me.filSearch = Me.txtSearch.Text
                    Case Master.eLang.GetString(62, "Director")
                        Me.filSearch = String.Concat("Director LIKE '%", Me.txtSearch.Text, "%'")
                        Me.FilterArray.Add(Me.filSearch)
                End Select

                Me.RunFilter(Me.cbSearch.Text = Master.eLang.GetString(100, "Actor"))

            Else
                If Not String.IsNullOrEmpty(Me.filSearch) Then
                    Me.FilterArray.Remove(Me.filSearch)
                    Me.filSearch = String.Empty
                End If
                Me.RunFilter(True)
            End If

        Catch
        End Try
        Me.tmrSearch.Enabled = False
    End Sub

    Private Sub tsbUpdateXBMC_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUpdateXBMC.ButtonClick
        Try
            For Each xCom As Settings.XBMCCom In Master.eSettings.XBMCComs
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
                Me.ScrapeData(Enums.ScrapeType.CleanFolders, Nothing, Nothing)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ConvertFileSourceToFolderSourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertFileSourceToFolderSourceToolStripMenuItem.Click
        Using dSortFiles As New dlgSortFiles
            If dSortFiles.ShowDialog() = Windows.Forms.DialogResult.OK Then Me.LoadMedia(New Structures.Scans With {.Movies = True})
        End Using
    End Sub

    Private Sub chkFilterMissing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkFilterMissing.Click
        Try
            Dim MissingFilter As New List(Of String)
            If Me.chkFilterMissing.Checked Then
                With Master.eSettings
                    If .MissingFilterPoster Then MissingFilter.Add("HasPoster = 0")
                    If .MissingFilterFanart Then MissingFilter.Add("HasFanart = 0")
                    If .MissingFilterNFO Then MissingFilter.Add("HasNfo = 0")
                    If .MissingFilterTrailer Then MissingFilter.Add("HasTrailer = 0")
                    If .MissingFilterSubs Then MissingFilter.Add("HasSub = 0")
                    If .MissingFilterExtras Then MissingFilter.Add("HasExtra = 0")
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

            Dim alGenres As New List(Of String)
            alGenres.AddRange(clbFilterGenres.CheckedItems.OfType(Of String).ToList)

            Me.txtFilterGenre.Text = Strings.Join(alGenres.ToArray, " AND ")

            For i As Integer = 0 To alGenres.Count - 1
                alGenres.Item(i) = String.Format("Genre LIKE '%{0}%'", alGenres.Item(i))
            Next

            Me.filGenre = Strings.Join(alGenres.ToArray, " AND ")

            Me.FilterArray.Add(Me.filGenre)
        End If

        If (Not String.IsNullOrEmpty(Me.cbFilterYear.Text) AndAlso Not Me.cbFilterYear.Text = Master.eLang.All) OrElse Me.clbFilterGenres.CheckedItems.Count > 0 OrElse _
        Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked OrElse Me.chkFilterLock.Checked OrElse Not Me.clbFilterSource.CheckedItems.Count > 0 OrElse _
        Me.chkFilterDupe.Checked OrElse Me.chkFilterMissing.Checked OrElse Me.chkFilterTolerance.Checked OrElse Not Me.cbFilterFileSource.Text = Master.eLang.All Then Me.RunFilter()
    End Sub

    Private Sub rbFilterOr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFilterOr.Click

        If clbFilterGenres.CheckedItems.Count > 0 Then
            Me.txtFilterGenre.Text = String.Empty
            Me.FilterArray.Remove(Me.filGenre)

            Dim alGenres As New List(Of String)
            alGenres.AddRange(clbFilterGenres.CheckedItems.OfType(Of String).ToList)

            Me.txtFilterGenre.Text = Strings.Join(alGenres.ToArray, " OR ")

            For i As Integer = 0 To alGenres.Count - 1
                alGenres.Item(i) = String.Format("Genre LIKE '%{0}%'", alGenres.Item(i))
            Next

            Me.filGenre = Strings.Join(alGenres.ToArray, " OR ")

            Me.FilterArray.Add(Me.filGenre)
        End If

        If (Not String.IsNullOrEmpty(Me.cbFilterYear.Text) AndAlso Not Me.cbFilterYear.Text = Master.eLang.All) OrElse Me.clbFilterGenres.CheckedItems.Count > 0 OrElse _
        Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked OrElse Me.chkFilterLock.Checked OrElse Not Me.clbFilterSource.CheckedItems.Count > 0 OrElse _
        Me.chkFilterDupe.Checked OrElse Me.chkFilterMissing.Checked OrElse Me.chkFilterTolerance.Checked OrElse Not Me.cbFilterFileSource.Text = Master.eLang.All Then Me.RunFilter()
    End Sub

    Private Sub chkFilterDupe_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterDupe.Click

        Try
            Me.RunFilter(True)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub dgvMediaList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvMediaList.KeyPress

        Try
            If Not StringUtils.AlphaNumericOnly(e.KeyChar) Then
                For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
                    If drvRow.Cells(3).Value.ToString.ToLower.StartsWith(e.KeyChar.ToString.ToLower) Then
                        drvRow.Selected = True
                        Me.dgvMediaList.CurrentCell = drvRow.Cells(3)
                        Exit For
                    End If
                Next
            ElseIf e.KeyChar = Chr(13) Then
                If Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse _
                Me.bwDownloadPic.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy _
                OrElse Me.bwCleanDB.IsBusy Then Return

                Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
                Master.currMovie = Master.DB.LoadMovieFromDB(ID)
                Me.SetStatus(Master.currMovie.Filename)
                Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString

                Using dEditMovie As New dlgEditMovie

                    Select Case dEditMovie.ShowDialog()
                        Case Windows.Forms.DialogResult.OK
                            Me.SetListItemAfterEdit(ID, indX)
                            If Me.RefreshMovie(ID) Then
                                Me.FillList(0)
                            End If
                        Case Windows.Forms.DialogResult.Retry
                            Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions)
                        Case Windows.Forms.DialogResult.Abort
                            Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
                        Case Else
                            If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value.ToString, True, False)
                    End Select

                End Using

            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub dgvTVShows_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvTVShows.KeyPress

        Try
            If Not StringUtils.AlphaNumericOnly(e.KeyChar) Then
                For Each drvRow As DataGridViewRow In Me.dgvTVShows.Rows
                    If drvRow.Cells(1).Value.ToString.ToLower.StartsWith(e.KeyChar.ToString.ToLower) Then
                        drvRow.Selected = True
                        Me.dgvTVShows.CurrentCell = drvRow.Cells(1)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub dgvTVSeasons_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvTVSeasons.KeyPress

        Try
            If Not StringUtils.AlphaNumericOnly(e.KeyChar) Then
                For Each drvRow As DataGridViewRow In Me.dgvTVSeasons.Rows
                    If drvRow.Cells(4).Value.ToString = e.KeyChar.ToString Then
                        drvRow.Selected = True
                        Me.dgvTVSeasons.CurrentCell = drvRow.Cells(3)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub dgvTVEpisodes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvTVEpisodes.KeyPress

        Try
            If Not StringUtils.AlphaNumericOnly(e.KeyChar) Then
                For Each drvRow As DataGridViewRow In Me.dgvTVEpisodes.Rows
                    If drvRow.Cells(2).Value.ToString.ToLower.StartsWith(e.KeyChar.ToString.ToLower) Then
                        drvRow.Selected = True
                        Me.dgvTVEpisodes.CurrentCell = drvRow.Cells(2)
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub cmnuMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMark.Click
        Try
            Dim setMark As Boolean = False
            If Me.dgvMediaList.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    'if any one item is set as unmarked, set menu to mark
                    'else they are all marked, so set menu to unmark
                    If Not Convert.ToBoolean(sRow.Cells(11).Value) Then
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
                        parMark.Value = If(Me.dgvMediaList.SelectedRows.Count > 1, setMark, Not Convert.ToBoolean(sRow.Cells(11).Value))
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(11).Value = parMark.Value
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            setMark = False
            For Each sRow As DataGridViewRow In Me.dgvMediaList.Rows
                If Convert.ToBoolean(sRow.Cells(11).Value) Then
                    setMark = True
                    Exit For
                End If
            Next
            Me.btnMarkAll.Text = If(setMark, Master.eLang.GetString(105, "Unmark All"), Master.eLang.GetString(35, "Mark All"))

            If Me.chkFilterMark.Checked Then
                Me.dgvMediaList.ClearSelection()
                Me.dgvMediaList.CurrentCell = Nothing
                If Me.dgvMediaList.RowCount <= 0 Then Me.ClearInfo()
            End If

            Me.dgvMediaList.Invalidate()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuLock.Click
        Try
            Dim setLock As Boolean = False
            If Me.dgvMediaList.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    'if any one item is set as unlocked, set menu to lock
                    'else they are all locked so set menu to unlock
                    If Not Convert.ToBoolean(sRow.Cells(14).Value) Then
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
                        parLock.Value = If(Me.dgvMediaList.SelectedRows.Count > 1, setLock, Not Convert.ToBoolean(sRow.Cells(14).Value))
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(14).Value = parLock.Value
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            If Me.chkFilterLock.Checked Then
                Me.dgvMediaList.ClearSelection()
                Me.dgvMediaList.CurrentCell = Nothing
                If Me.dgvMediaList.RowCount <= 0 Then Me.ClearInfo()
            End If

            Me.dgvMediaList.Invalidate()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                        If Not sRow.Cells(26).Value.ToString.Contains(Me.GenreListToolStripComboBox.Text) Then
                            If Not String.IsNullOrEmpty(sRow.Cells(26).Value.ToString) Then
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

            Me.LoadInfo(Convert.ToInt32(Me.dgvMediaList.Item(0, Me.dgvMediaList.CurrentCell.RowIndex).Value), Me.dgvMediaList.Item(1, Me.dgvMediaList.CurrentCell.RowIndex).Value.ToString, True, False)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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

            Me.LoadInfo(Convert.ToInt32(Me.dgvMediaList.Item(0, Me.dgvMediaList.CurrentCell.RowIndex).Value), Me.dgvMediaList.Item(1, Me.dgvMediaList.CurrentCell.RowIndex).Value.ToString, True, False)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                        If sRow.Cells(26).Value.ToString.Contains(Me.GenreListToolStripComboBox.Text) Then
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

            Me.LoadInfo(Convert.ToInt32(Me.dgvMediaList.Item(0, Me.dgvMediaList.CurrentCell.RowIndex).Value), Me.dgvMediaList.Item(1, Me.dgvMediaList.CurrentCell.RowIndex).Value.ToString, True, False)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub cmnuRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRescrape.Click

        '//
        ' Begin the process to scrape IMDB with the current ID
        '\\

        Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, Convert.ToInt32(Me.dgvMediaList.SelectedRows(0).Cells(0).Value))
    End Sub

    Private Sub cmnuSearchNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuSearchNew.Click

        '//
        ' Begin the process to search IMDB for data
        '\\

        Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, Convert.ToInt32(Me.dgvMediaList.SelectedRows(0).Cells(0).Value), True)
    End Sub

    Private Sub cmnuEditMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuEditMovie.Click

        Try
            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
            Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString

            Using dEditMovie As New dlgEditMovie
                Select Case dEditMovie.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        Me.SetListItemAfterEdit(ID, indX)
                        If Me.RefreshMovie(ID) Then
                            Me.FillList(0)
                        End If
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, ID)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
                    Case Else
                        If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value.ToString, True, False)
                End Select
            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvMediaList.MouseDown
        Try
            If e.Button = Windows.Forms.MouseButtons.Right And Me.dgvMediaList.RowCount > 0 Then
                Dim dgvHTI As DataGridView.HitTestInfo = dgvMediaList.HitTest(e.X, e.Y)
                If dgvHTI.Type = DataGridViewHitTestType.Cell Then

                    Me.tmpTitle = Me.dgvMediaList.Item(15, dgvHTI.RowIndex).Value.ToString

                    If Me.dgvMediaList.SelectedRows.Count > 1 AndAlso Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                        Dim setMark As Boolean = False
                        Dim setLock As Boolean = False

                        Me.cmnuTitle.Text = Master.eLang.GetString(106, ">> Multiple <<")
                        Me.cmnuEditMovie.Visible = False
                        Me.cmnuRescrape.Visible = False
                        Me.cmnuSearchNew.Visible = False
                        Me.cmuRenamer.Visible = False
                        Me.cmnuMetaData.Visible = False
                        Me.cmnuSep2.Visible = False
                        Me.ToolStripSeparator2.Visible = False

                        For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                            'if any one item is set as unmarked, set menu to mark
                            'else they are all marked, so set menu to unmark
                            If Not Convert.ToBoolean(sRow.Cells(11).Value) Then
                                setMark = True
                                If setLock Then Exit For
                            End If
                            'if any one item is set as unlocked, set menu to lock
                            'else they are all locked so set menu to unlock
                            If Not Convert.ToBoolean(sRow.Cells(14).Value) Then
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
                        Me.cmuRenamer.Visible = True
                        Me.cmnuMetaData.Visible = True
                        Me.cmnuSep.Visible = True
                        Me.cmnuSep2.Visible = True

                        If Not Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                            Me.mnuMediaList.Enabled = False
                        End If

                        cmnuTitle.Text = String.Concat(">> ", Me.dgvMediaList.Item(3, dgvHTI.RowIndex).Value, " <<")

                        If Not Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                            Me.dgvMediaList.ClearSelection()
                            Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected = True
                            Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Item(3, dgvHTI.RowIndex)
                        End If

                        Me.cmnuMark.Text = If(Convert.ToBoolean(Me.dgvMediaList.Item(11, dgvHTI.RowIndex).Value), Master.eLang.GetString(107, "Unmark"), Master.eLang.GetString(23, "Mark"))
                        Me.cmnuLock.Text = If(Convert.ToBoolean(Me.dgvMediaList.Item(14, dgvHTI.RowIndex).Value), Master.eLang.GetString(108, "Unlock"), Master.eLang.GetString(24, "Lock"))

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
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub mnuAllAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoTrailer.Click

        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoMI.Click

        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskAll.Click

        Functions.SetScraperMod(Enums.ModType.All, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskNfo.Click

        Functions.SetScraperMod(Enums.ModType.NFO, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskPoster.Click

        Functions.SetScraperMod(Enums.ModType.Poster, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskFanart.Click

        Functions.SetScraperMod(Enums.ModType.Fanart, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskExtra.Click

        Functions.SetScraperMod(Enums.ModType.Extra, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskTrailer.Click

        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskMI.Click

        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        btnCancel.Visible = False
        lblCanceling.Visible = True
        pbCanceling.Visible = True

        If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()
        If Me.bwRefreshMovies.IsBusy Then Me.bwRefreshMovies.CancelAsync()
        If Me.bwNewScraper.IsBusy Then Me.bwNewScraper.CancelAsync()

        While Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwNewScraper.IsBusy
            Application.DoEvents()
        End While
    End Sub

    Private Sub OpenContainingFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenContainingFolderToolStripMenuItem.Click
        Dim doOpen As Boolean = True
        If Me.dgvMediaList.SelectedRows.Count > 10 Then
            If Not MsgBox(String.Format(Master.eLang.GetString(635, "You have selected {0} folders to open. Are you sure you want to do this?"), Me.dgvMediaList.SelectedRows.Count), MsgBoxStyle.YesNo Or MsgBoxStyle.Question, Master.eLang.GetString(104, "Are You Sure?")) = MsgBoxResult.Yes Then doOpen = False
        End If

        If doOpen Then
            For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                Using Explorer As New Diagnostics.Process

                    If Master.isWindows Then
                        Explorer.StartInfo.FileName = "explorer.exe"
                        Explorer.StartInfo.Arguments = String.Format("/select,""{0}""", sRow.Cells(1).Value)
                    Else
                        Explorer.StartInfo.FileName = "xdg-open"
                        Explorer.StartInfo.Arguments = String.Format("""{0}""", Path.GetDirectoryName(sRow.Cells(1).Value.ToString))
                    End If
                    Explorer.Start()
                End Using
            Next
        End If
    End Sub

    Private Sub DeleteMovieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMovieToolStripMenuItem.Click
        Try
            Dim MoviesToDelete As New List(Of Long)
            Dim MovieId As Int64 = -1

            For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                MovieId = Convert.ToInt64(sRow.Cells(0).Value)
                If Not MoviesToDelete.Contains(MovieId) Then
                    MoviesToDelete.Add(MovieId)
                End If
            Next

            If MoviesToDelete.Count > 0 Then
                Using dlg As New dlgDeleteConfirm
                    If dlg.ShowDialog(MoviesToDelete) = Windows.Forms.DialogResult.OK Then
                        Me.FillList(0)
                    End If
                End Using
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RemoveFromDatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromDatabaseToolStripMenuItem.Click
        Try
            Me.ClearInfo()

            For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                Master.DB.DeleteFromDB(Convert.ToInt64(sRow.Cells(0).Value))
            Next

            Me.FillList(0)

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub CopyExistingFanartToBackdropsFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyExistingFanartToBackdropsFolderToolStripMenuItem.Click
        '//
        ' Copy all existing fanart to the backdrops folder
        '\\

        Me.ScrapeData(Enums.ScrapeType.CopyBD, Nothing, Nothing)
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
            dgvMediaList.Refresh()
            btnMarkAll.Text = If(Not MarkAll, Master.eLang.GetString(35, "Mark All"), Master.eLang.GetString(105, "Unmark All"))
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
        Me.ClearCache()
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

            Dim pHeight As Integer = Functions.Quantize(Me.gbSpecific.Height + Me.lblFilter.Height + 15, 5)

            If Master.eSettings.InfoPanelAnim Then
                If Me.aniFilterRaise Then
                    Me.pnlFilter.Height += 5
                Else
                    Me.pnlFilter.Height -= 5
                End If
            Else
                If Me.aniFilterRaise Then
                    Me.pnlFilter.Height = pHeight
                Else
                    Me.pnlFilter.Height = 25
                End If
            End If
            If Me.pnlFilter.Height = 25 Then
                Me.tmrFilterAni.Stop()
                Me.btnFilterUp.Enabled = True
                Me.btnFilterDown.Enabled = False
            ElseIf Me.pnlFilter.Height = pHeight Then
                Me.tmrFilterAni.Stop()
                Me.btnFilterUp.Enabled = False
                Me.btnFilterDown.Enabled = True
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cbFilterFileSource_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterFileSource.SelectedIndexChanged
        Try
            While Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwCleanDB.IsBusy
                Application.DoEvents()
            End While

            For i As Integer = Me.FilterArray.Count - 1 To 0 Step -1
                If Me.FilterArray(i).ToString.StartsWith("FileSource =") Then
                    Me.FilterArray.RemoveAt(i)
                End If
            Next

            If Not cbFilterFileSource.Text = Master.eLang.All Then
                Me.FilterArray.Add(String.Format("FileSource = '{0}'", cbFilterFileSource.Text.Replace(" | ", "|")))
            End If

            Me.RunFilter()
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub OfflineMediaManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OfflineMediaManagerToolStripMenuItem.Click
        Using dOfflineHolder As New dlgOfflineHolder
            If dOfflineHolder.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Me.LoadMedia(New Structures.Scans With {.Movies = True})
            End If
        End Using
    End Sub

    Private Sub mnuAllAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.FullAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.UpdateAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.NewAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.NewAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskTrailer.Click
        Functions.SetScraperMod(Enums.ModType.Trailer, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAsk, Master.DefaultOptions)
    End Sub

    Private Sub CustomUpdaterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomUpdaterToolStripMenuItem.Click
        Using dUpdate As New dlgUpdateMedia
            Dim CustomUpdater As Structures.CustomUpdaterStruct = Nothing
            CustomUpdater = dUpdate.ShowDialog()
            If Not CustomUpdater.Canceled Then
                Me.ScrapeData(CustomUpdater.ScrapeType, CustomUpdater.Options)
            End If
        End Using
    End Sub

    Private Sub mnuAllAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoMI.Click
        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.FullAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskMI.Click
        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoMI.Click
        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.NewAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskMI.Click
        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.NewAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoMI.Click
        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskMI.Click
        Functions.SetScraperMod(Enums.ModType.Meta, True)
        Me.ScrapeData(Enums.ScrapeType.MarkAsk, Master.DefaultOptions)
    End Sub

    Private Sub RenamerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenamerToolStripMenuItem.Click
        Using dBulkRename As New dlgBulkRenamer
            Try
                If dBulkRename.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Me.LoadMedia(New Structures.Scans With {.Movies = True})
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub

    Private Sub cmnuRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRefresh.Click
        Try
            Me.dgvMediaList.Cursor = Cursors.WaitCursor
            Me.SetControlsEnabled(False, True)

            Dim doFill As Boolean = False
            Dim doBatch As Boolean = Me.dgvMediaList.SelectedRows.Count > 1

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                    doFill = Me.RefreshMovie(Convert.ToInt64(sRow.Cells(0).Value), doBatch)
                Next
                SQLtransaction.Commit()
            End Using

            Me.dgvMediaList.Cursor = Cursors.Default
            Me.SetControlsEnabled(True, True)

            If doFill Then FillList(0) Else DoTitleCheck()
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RefreshAllMoviesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshAllMoviesToolStripMenuItem.Click
        RefreshAllMovies()
    End Sub

    Private Sub RefreshAllMovies()
        If Me.dtMedia.Rows.Count > 0 Then

            Me.SetControlsEnabled(False)
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

                Dim alGenres As New List(Of String)
                alGenres.AddRange(clbFilterGenres.CheckedItems.OfType(Of String).ToList)

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

    Private Sub clbFilterSource_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles clbFilterSource.LostFocus
        Try
            Me.pnlFilterSource.Visible = False
            Me.pnlFilterSource.Tag = "NO"

            If clbFilterSource.CheckedItems.Count > 0 Then
                Me.txtFilterSource.Text = String.Empty
                Me.FilterArray.Remove(Me.filSource)

                Dim alSource As New List(Of String)
                alSource.AddRange(clbFilterSource.CheckedItems.OfType(Of String).ToList)

                Me.txtFilterSource.Text = Strings.Join(alSource.ToArray, " | ")

                For i As Integer = 0 To alSource.Count - 1
                    alSource.Item(i) = String.Format("Source = '{0}'", alSource.Item(i))
                Next

                Me.filSource = Strings.Join(alSource.ToArray, " OR ")

                Me.FilterArray.Add(Me.filSource)
                Me.RunFilter()
            Else
                If Not String.IsNullOrEmpty(Me.filSource) Then
                    Me.txtFilterSource.Text = String.Empty
                    Me.FilterArray.Remove(Me.filSource)
                    Me.filSource = String.Empty
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
        ElseIf Not Me.pnlFilterGenre.Tag.ToString = "NO" Then
            Me.pnlFilterGenre.Tag = String.Empty
            Me.pnlFilterGenre.Visible = True
            Me.clbFilterGenres.Focus()
        Else
            Me.pnlFilterGenre.Tag = String.Empty
        End If
    End Sub

    Private Sub txtFilterSource_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFilterSource.Click
        Me.pnlFilterSource.Location = New Point(Me.gbSpecific.Left + Me.txtFilterSource.Left, (Me.pnlFilter.Top + Me.txtFilterSource.Top + Me.gbSpecific.Top) - Me.pnlFilterSource.Height)
        If Me.pnlFilterSource.Visible Then
            Me.pnlFilterSource.Visible = False
        ElseIf Not Me.pnlFilterSource.Tag.ToString = "NO" Then
            Me.pnlFilterSource.Tag = String.Empty
            Me.pnlFilterSource.Visible = True
            Me.clbFilterSource.Focus()
        Else
            Me.pnlFilterSource.Tag = String.Empty
        End If
    End Sub

    Private Sub lblGFilClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblGFilClose.Click
        Me.txtFilterGenre.Focus()
        Me.pnlFilterGenre.Tag = String.Empty
    End Sub

    Private Sub lblSFilClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblSFilClose.Click
        Me.txtFilterSource.Focus()
        Me.pnlFilterSource.Tag = String.Empty
    End Sub

    Private Sub cbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSearch.SelectedIndexChanged
        Me.txtSearch.Text = String.Empty
    End Sub

    Private Sub cbFilterYearMod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterYearMod.SelectedIndexChanged
        Try
            If Not String.IsNullOrEmpty(cbFilterYear.Text) AndAlso Not cbFilterYear.Text = Master.eLang.All Then
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
            If Not String.IsNullOrEmpty(cbFilterYearMod.Text) AndAlso Not cbFilterYear.Text = Master.eLang.All Then
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

                If cbFilterYear.Text = Master.eLang.All Then
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

    Private Sub dgvTVShows_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvTVShows.KeyDown
        'stop enter key from selecting next list item
        e.Handled = e.KeyCode = Keys.Enter
    End Sub

    Private Sub dgvTVSeasons_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvTVSeasons.KeyDown
        'stop enter key from selecting next list item
        e.Handled = e.KeyCode = Keys.Enter
    End Sub

    Private Sub dgvTVEpisodes_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvTVEpisodes.KeyDown
        'stop enter key from selecting next list item
        e.Handled = e.KeyCode = Keys.Enter
    End Sub

    Private Sub cmnuRenameAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRenameAuto.Click

        Try
            Cursor.Current = Cursors.WaitCursor
            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
            FileFolderRenamer.RenameSingle(Master.currMovie, Master.eSettings.FoldersPattern, Master.eSettings.FilesPattern, True, True, True)
            Me.SetListItemAfterEdit(ID, indX)
            If Me.RefreshMovie(ID) Then
                Me.FillList(0)
            End If
            Me.SetStatus(Master.currMovie.Filename)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Cursor.Current = Cursors.Default
    End Sub
    Private Sub cmnuRenameManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRenameManual.Click
        Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
        Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
        Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString
        Using dRenameManual As New dlgRenameManual
            Select Case dRenameManual.ShowDialog()
                Case Windows.Forms.DialogResult.OK
                    Me.SetListItemAfterEdit(ID, indX)
                    If Me.RefreshMovie(ID) Then
                        Me.FillList(0)
                    End If
                    Me.SetStatus(Master.currMovie.Filename)
            End Select
        End Using
    End Sub

    Private Sub cmnuMetaData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMetaData.Click
        Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
        Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
        Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString
        Using dEditMeta As New dlgFileInfo
            Select Case dEditMeta.ShowDialog(False)
                Case Windows.Forms.DialogResult.OK
                    Me.SetListItemAfterEdit(ID, indX)
                    If Me.RefreshMovie(ID) Then
                        Me.FillList(0)
                    End If
            End Select
        End Using
    End Sub

    Private Sub btnSortDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSortDate.Click
        If Me.btnSortDate.Tag.ToString = "DESC" Then
            Me.btnSortDate.Tag = "ASC"
            Me.btnSortDate.Image = My.Resources.desc
            Me.dgvMediaList.Sort(Me.dgvMediaList.Columns(0), ComponentModel.ListSortDirection.Descending)
        Else
            Me.btnSortDate.Tag = "DESC"
            Me.btnSortDate.Image = My.Resources.asc
            Me.dgvMediaList.Sort(Me.dgvMediaList.Columns(0), ComponentModel.ListSortDirection.Ascending)
        End If
    End Sub

    Private Sub btnSortTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSortTitle.Click
        If Me.btnSortTitle.Tag.ToString = "DESC" Then
            Me.btnSortTitle.Tag = "ASC"
            Me.btnSortTitle.Image = My.Resources.desc
            Me.dgvMediaList.Sort(Me.dgvMediaList.Columns(50), ComponentModel.ListSortDirection.Descending)
        Else
            Me.btnSortTitle.Tag = "DESC"
            Me.btnSortTitle.Image = My.Resources.asc
            Me.dgvMediaList.Sort(Me.dgvMediaList.Columns(50), ComponentModel.ListSortDirection.Ascending)
        End If
    End Sub

    Private Sub btnIMDBRating_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIMDBRating.Click
        If Me.btnIMDBRating.Tag.ToString = "DESC" Then
            Me.btnIMDBRating.Tag = "ASC"
            Me.btnIMDBRating.Image = My.Resources.desc
            Me.dgvMediaList.Sort(Me.dgvMediaList.Columns(18), ComponentModel.ListSortDirection.Descending)
        Else
            Me.btnIMDBRating.Tag = "DESC"
            Me.btnIMDBRating.Image = My.Resources.asc
            Me.dgvMediaList.Sort(Me.dgvMediaList.Columns(18), ComponentModel.ListSortDirection.Ascending)
        End If
    End Sub

    Private Sub CleanDatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CleanDatabaseToolStripMenuItem.Click
        Me.SetControlsEnabled(False)
        Me.tspbLoading.Style = ProgressBarStyle.Marquee
        Me.EnableFilters(False)

        Me.tslLoading.Text = Master.eLang.GetString(644, "Cleaning Database:")
        Me.tspbLoading.Visible = True
        Me.tslLoading.Visible = True

        Me.bwCleanDB.WorkerSupportsCancellation = True
        Me.bwCleanDB.RunWorkerAsync()
    End Sub

    Private Sub tabsMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabsMain.SelectedIndexChanged

        Me.ClearInfo()

        Select Case tabsMain.SelectedIndex
            Case 0
                Me.pnlFilter.Visible = True
                Me.pnlListTop.Height = 56
                Me.btnMarkAll.Visible = True
                Me.scTV.Visible = False
                Me.dgvMediaList.Visible = True
                Me.ApplyTheme(Theming.ThemeType.Movies)
                If Me.bwLoadEpInfo.IsBusy Then Me.bwLoadEpInfo.CancelAsync()
                If Me.bwLoadSeasonInfo.IsBusy Then Me.bwLoadSeasonInfo.CancelAsync()
                If Me.bwLoadShowInfo.IsBusy Then Me.bwLoadShowInfo.CancelAsync()
                If Me.bwDownloadPic.IsBusy Then Me.bwDownloadPic.CancelAsync()
                If Me.dgvMediaList.RowCount > 0 Then
                    Me.SetControlsEnabled(True)
                    Me.dgvMediaList.Focus()
                End If
            Case 1
                Me.ToolsToolStripMenuItem.Enabled = False
                Me.tsbAutoPilot.Enabled = False
                Me.dgvMediaList.Visible = False
                Me.pnlFilter.Visible = False
                Me.pnlListTop.Height = 23
                Me.btnMarkAll.Visible = False
                Me.scTV.Visible = True
                Me.ApplyTheme(Theming.ThemeType.Show)
                If Me.bwLoadInfo.IsBusy Then Me.bwLoadInfo.CancelAsync()
                If Me.bwDownloadPic.IsBusy Then Me.bwDownloadPic.CancelAsync()
                If Me.dgvTVShows.RowCount > 0 Then
                    If Me.currShowRow = -1 Then
                        Me.dgvTVShows.ClearSelection()
                        Me.dgvTVShows.Rows(0).Selected = True
                        Me.currShowRow = 0
                    End If

                    Me.dgvTVShows.Focus()

                End If
        End Select
    End Sub

    Private Sub dgvTVShows_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVShows.CellDoubleClick

        Try

            If Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadShowInfo.IsBusy OrElse Me.bwLoadSeasonInfo.IsBusy OrElse Me.bwLoadEpInfo.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwCleanDB.IsBusy Then Return

            Dim indX As Integer = Me.dgvTVShows.SelectedRows(0).Index
            Dim ID As Integer = Convert.ToInt32(Me.dgvTVShows.Item(0, indX).Value)
            Master.currShow = Master.DB.LoadTVShowFromDB(ID)

            Using dEditShow As New dlgEditShow

                Select Case dEditShow.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshShow(ID, False, True, False, False) Then
                            Me.FillList(0)
                        End If
                End Select

            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVShows_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvTVShows.MouseDown
        Try
            If e.Button = Windows.Forms.MouseButtons.Right And Me.dgvTVShows.RowCount > 0 Then
                Dim dgvHTI As DataGridView.HitTestInfo = dgvTVShows.HitTest(e.X, e.Y)
                If dgvHTI.Type = DataGridViewHitTestType.Cell Then

                    Me.tmpTitle = Me.dgvTVShows.Item(1, dgvHTI.RowIndex).Value.ToString
                    Me.tmpTVDB = Me.dgvTVShows.Item(9, dgvHTI.RowIndex).Value.ToString

                    If Me.dgvTVShows.SelectedRows.Count > 1 AndAlso Me.dgvTVShows.Rows(dgvHTI.RowIndex).Selected Then
                        Dim setMark As Boolean = False
                        Dim setLock As Boolean = False

                        Me.cmnuShowTitle.Text = Master.eLang.GetString(106, ">> Multiple <<")
                        Me.ToolStripSeparator8.Visible = False
                        Me.cmnuEditShow.Visible = False

                        For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                            'if any one item is set as unmarked, set menu to mark
                            'else they are all marked, so set menu to unmark
                            If Not Convert.ToBoolean(sRow.Cells(6).Value) Then
                                setMark = True
                                If setLock Then Exit For
                            End If
                            'if any one item is set as unlocked, set menu to lock
                            'else they are all locked so set menu to unlock
                            If Not Convert.ToBoolean(sRow.Cells(10).Value) Then
                                setLock = True
                                If setMark Then Exit For
                            End If
                        Next

                        Me.cmnuMarkShow.Text = If(setMark, Master.eLang.GetString(23, "Mark"), Master.eLang.GetString(107, "Unmark"))
                        Me.cmnuLockShow.Text = If(setLock, Master.eLang.GetString(24, "Lock"), Master.eLang.GetString(108, "Unlock"))

                    Else
                        Me.ToolStripSeparator8.Visible = True
                        Me.cmnuEditShow.Visible = True

                        If Not Me.dgvTVShows.Rows(dgvHTI.RowIndex).Selected Then
                            Me.mnuShows.Enabled = False
                        End If

                        Me.cmnuShowTitle.Text = String.Concat(">> ", Me.dgvTVShows.Item(1, dgvHTI.RowIndex).Value, " <<")
                        Me.cmnuMarkShow.Text = If(Convert.ToBoolean(Me.dgvTVShows.Item(6, dgvHTI.RowIndex).Value), Master.eLang.GetString(107, "Unmark"), Master.eLang.GetString(23, "Mark"))
                        Me.cmnuLockShow.Text = If(Convert.ToBoolean(Me.dgvTVShows.Item(10, dgvHTI.RowIndex).Value), Master.eLang.GetString(108, "Unlock"), Master.eLang.GetString(24, "Lock"))

                        If Not Me.dgvTVShows.Rows(dgvHTI.RowIndex).Selected Then
                            Me.dgvTVShows.ClearSelection()
                            Me.dgvTVShows.Rows(dgvHTI.RowIndex).Selected = True
                            Me.dgvTVShows.CurrentCell = Me.dgvTVShows.Item(3, dgvHTI.RowIndex)
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuEditShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuEditShow.Click
        Try
            Dim indX As Integer = Me.dgvTVShows.SelectedRows(0).Index
            Dim ID As Integer = Convert.ToInt32(Me.dgvTVShows.Item(0, indX).Value)
            Master.currShow = Master.DB.LoadTVShowFromDB(ID)

            Using dEditShow As New dlgEditShow

                Select Case dEditShow.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshShow(ID, False, True, False, False) Then
                            Me.FillList(0)
                        End If
                End Select

            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub dgvTVEpisodes_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVEpisodes.CellDoubleClick

        Try

            If Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadShowInfo.IsBusy OrElse Me.bwLoadEpInfo.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwCleanDB.IsBusy Then Return

            Dim indX As Integer = Me.dgvTVEpisodes.SelectedRows(0).Index
            Dim ID As Integer = Convert.ToInt32(Me.dgvTVEpisodes.Item(0, indX).Value)
            Master.currShow = Master.DB.LoadTVEpFromDB(ID, True)

            Using dEditEpisode As New dlgEditEpisode

                Select Case dEditEpisode.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshEpisode(ID) Then
                            Me.FillEpisodes(Convert.ToInt32(Master.currShow.ShowID), Master.currShow.TVEp.Season)
                        End If
                End Select

            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvTVEpisodes_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvTVEpisodes.MouseDown
        Try
            If e.Button = Windows.Forms.MouseButtons.Right And Me.dgvTVEpisodes.RowCount > 0 Then
                Dim dgvHTI As DataGridView.HitTestInfo = dgvTVEpisodes.HitTest(e.X, e.Y)
                If dgvHTI.Type = DataGridViewHitTestType.Cell Then

                    If Me.dgvTVEpisodes.SelectedRows.Count > 1 AndAlso Me.dgvTVEpisodes.Rows(dgvHTI.RowIndex).Selected Then
                        Dim setMark As Boolean = False
                        Dim setLock As Boolean = False

                        Me.cmnuEpTitle.Text = Master.eLang.GetString(106, ">> Multiple <<")
                        Me.cmnuEditEpisode.Visible = False
                        Me.ToolStripSeparator9.Visible = False

                        For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                            'if any one item is set as unmarked, set menu to mark
                            'else they are all marked, so set menu to unmark
                            If Not Convert.ToBoolean(sRow.Cells(7).Value) Then
                                setMark = True
                                If setLock Then Exit For
                            End If
                            'if any one item is set as unlocked, set menu to lock
                            'else they are all locked so set menu to unlock
                            If Not Convert.ToBoolean(sRow.Cells(10).Value) Then
                                setLock = True
                                If setMark Then Exit For
                            End If
                        Next

                        Me.cmnuMarkEp.Text = If(setMark, Master.eLang.GetString(23, "Mark"), Master.eLang.GetString(107, "Unmark"))
                        Me.cmnuLockEp.Text = If(setLock, Master.eLang.GetString(24, "Lock"), Master.eLang.GetString(108, "Unlock"))
                    Else
                        Me.cmnuEditEpisode.Visible = True
                        Me.ToolStripSeparator9.Visible = True

                        If Not Me.dgvTVEpisodes.Rows(dgvHTI.RowIndex).Selected Then
                            Me.mnuEpisodes.Enabled = False
                        End If

                        cmnuEpTitle.Text = String.Concat(">> ", Me.dgvTVEpisodes.Item(2, dgvHTI.RowIndex).Value, " <<")
                        Me.cmnuMarkEp.Text = If(Convert.ToBoolean(Me.dgvTVEpisodes.Item(7, dgvHTI.RowIndex).Value), Master.eLang.GetString(107, "Unmark"), Master.eLang.GetString(23, "Mark"))
                        Me.cmnuLockEp.Text = If(Convert.ToBoolean(Me.dgvTVEpisodes.Item(10, dgvHTI.RowIndex).Value), Master.eLang.GetString(108, "Unlock"), Master.eLang.GetString(24, "Lock"))

                        If Me.bwLoadEpInfo.IsBusy Then Me.bwLoadEpInfo.CancelAsync()

                        If Not Me.dgvTVEpisodes.Rows(dgvHTI.RowIndex).Selected Then
                            Me.dgvTVEpisodes.ClearSelection()
                            Me.dgvTVEpisodes.Rows(dgvHTI.RowIndex).Selected = True
                            Me.dgvTVEpisodes.CurrentCell = Me.dgvTVEpisodes.Item(2, dgvHTI.RowIndex)
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuEditEpisode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuEditEpisode.Click
        Try
            Dim indX As Integer = Me.dgvTVEpisodes.SelectedRows(0).Index
            Dim ID As Integer = Convert.ToInt32(Me.dgvTVEpisodes.Item(0, indX).Value)
            Master.currShow = Master.DB.LoadTVEpFromDB(ID, True)

            Using dEditEpisode As New dlgEditEpisode

                Select Case dEditEpisode.ShowDialog()
                    Case Windows.Forms.DialogResult.OK
                        If Me.RefreshEpisode(ID) Then
                            Me.FillEpisodes(Convert.ToInt32(Master.currShow.ShowID), Master.currShow.TVEp.Season)
                        End If
                End Select

            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
#End Region '*** Form/Controls


#Region "Background Workers"

    ' ########################################
    ' ########## WORKER COMPONENTS ###########
    ' ########################################

    Private Sub bwMediaInfo_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwMediaInfo.DoWork

        '//
        ' Thread to procure technical and tag information about media via MediaInfo.dll
        '\\
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)

        Try
            Me.UpdateMediaInfo(Args.Movie)
            Master.DB.SaveMovieToDB(Args.Movie, False, False, True)

            If Me.bwMediaInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            e.Result = New Results With {.fileinfo = NFO.FIToString(Args.Movie.Movie.FileInfo), .setEnabled = Args.setEnabled, .Path = Args.Path, .Movie = Args.Movie}
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            e.Result = New Results With {.fileinfo = "error", .setEnabled = Args.setEnabled}
            e.Cancel = True
        End Try
    End Sub

    Private Sub bwMediaInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwMediaInfo.RunWorkerCompleted

        '//
        ' Thread finished: fill textbox with result
        '\\

        If Not e.Cancelled Then
            Dim Res As Results = DirectCast(e.Result, Results)

            Try
                If Not Res.fileInfo = "error" Then
                    Me.pbMILoading.Visible = False
                    Me.txtMetaData.Text = Res.fileInfo
                    If Master.eSettings.ScanMediaInfo Then
                        Me.SetAVImages(APIXML.GetAVImages(Res.Movie.Movie.FileInfo, Res.Movie.Filename))
                        Me.pnlInfoIcons.Width = pbVideo.Width + pbVType.Width + pbResolution.Width + pbAudio.Width + pbChannels.Width + pbStudio.Width + 6
                        Me.pbStudio.Left = pbVideo.Width + pbVType.Width + pbResolution.Width + pbAudio.Width + pbChannels.Width + 5
                    Else
                        Me.pnlInfoIcons.Width = pbStudio.Width + 1
                        Me.pbStudio.Left = 0
                    End If
                    If Master.eSettings.UseMIDuration Then
                        If Not String.IsNullOrEmpty(Res.Movie.Movie.Runtime) Then
                            Me.lblRuntime.Text = String.Format(Master.eLang.GetString(112, "Runtime: {0}"), Res.Movie.Movie.Runtime)
                        End If
                    End If
                    Me.btnMetaDataRefresh.Focus()
                End If
            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            If Res.setEnabled Then
                Me.tabsMain.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                If Me.dgvMediaList.RowCount > 0 Then
                    Me.SetControlsEnabled(True)
                End If
            End If
        End If
    End Sub

    Private Sub bwDownloadPic_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadPic.DoWork

        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        Try
            Dim dImage As Image = sHTTP.DownloadImage(Args.pURL)

            If Me.bwDownloadPic.CancellationPending Then
                e.Cancel = True
                Return
            End If

            e.Result = New Results With {.Result = dImage}
        Catch ex As Exception
            e.Result = New Results With {.Result = Nothing}
            e.Cancel = True
        End Try

    End Sub

    Private Sub bwDownloadPic_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadPic.RunWorkerCompleted

        '//
        ' Thread finished: display pic if it was able to get one
        '\\

        Me.pbActLoad.Visible = False

        If e.Cancelled Then
            Me.pbActors.Image = My.Resources.actor_silhouette
        Else
            Dim Res As Results = DirectCast(e.Result, Results)

            If Not IsNothing(Res.Result) Then
                Me.pbActors.Image = Res.Result
            Else
                Me.pbActors.Image = My.Resources.actor_silhouette
            End If
        End If
    End Sub

    Private Sub bwLoadInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadInfo.DoWork

        '//
        ' Thread to load media images and information (from nfo) then display on screen
        '\\

        Try

            Dim Args As Arguments = DirectCast(e.Argument, Arguments)
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
            While bwMediaInfo.IsBusy
                Application.DoEvents()
            End While

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            e.Cancel = True
        End Try


    End Sub

    Private Sub bwLoadInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadInfo.RunWorkerCompleted

        '//
        ' Thread finished: display it
        '\\

        Try
            If Not e.Cancelled Then
                Me.fillScreenInfoWithMovie()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwLoadShowInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadShowInfo.DoWork

        Try

            Dim Args As Arguments = DirectCast(e.Argument, Arguments)
            Me.MainFanart.Clear()
            Me.MainPoster.Clear()

            If bwLoadShowInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            Master.currShow = Master.DB.LoadTVShowFromDB(Args.ID)

            If bwLoadShowInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayFanart Then Me.MainFanart.FromFile(Master.currShow.ShowFanartPath)

            If bwLoadShowInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayPoster Then Me.MainPoster.FromFile(Master.currShow.ShowPosterPath)

            If bwLoadShowInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            e.Cancel = True
        End Try


    End Sub

    Private Sub bwLoadShowInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadShowInfo.RunWorkerCompleted

        '//
        ' Thread finished: display it
        '\\

        Try
            If Not e.Cancelled Then
                Me.fillScreenInfoWithShow()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwLoadSeasonInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadSeasonInfo.DoWork

        Try

            Dim Args As Arguments = DirectCast(e.Argument, Arguments)
            Me.MainPoster.Clear()
            Me.MainFanart.Clear()

            Master.currShow = Master.DB.LoadTVSeasonFromDB(Args.ID, Args.Season)

            If bwLoadSeasonInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayPoster Then
                If Not String.IsNullOrEmpty(Master.currShow.SeasonPosterPath) Then
                    Me.MainPoster.FromFile(Master.currShow.SeasonPosterPath)
                Else
                    Me.MainPoster.FromFile(Master.currShow.ShowPosterPath)
                End If
            End If

            If bwLoadSeasonInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayFanart Then
                If Not String.IsNullOrEmpty(Master.currShow.SeasonFanartPath) Then
                    Me.MainFanart.FromFile(Master.currShow.SeasonFanartPath)
                Else
                    Me.MainFanart.FromFile(Master.currShow.ShowFanartPath)
                End If
            End If

            If bwLoadSeasonInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            e.Cancel = True
        End Try


    End Sub

    Private Sub bwLoadSeasonInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadSeasonInfo.RunWorkerCompleted

        Try
            If Not e.Cancelled Then
                Me.fillScreenInfoWithSeason()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwLoadEpInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadEpInfo.DoWork

        Try

            Dim Args As Arguments = DirectCast(e.Argument, Arguments)
            Me.MainPoster.Clear()
            Me.MainFanart.Clear()

            If bwLoadEpInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            Master.currShow = Master.DB.LoadTVEpFromDB(Args.ID, True)

            If bwLoadEpInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayPoster Then Me.MainPoster.FromFile(Master.currShow.EpPosterPath)

            If bwLoadEpInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If

            If Not Master.eSettings.NoDisplayFanart Then
                If Not String.IsNullOrEmpty(Master.currShow.EpFanartPath) Then
                    Me.MainFanart.FromFile(Master.currShow.EpFanartPath)
                Else
                    Me.MainFanart.FromFile(Master.currShow.ShowFanartPath)
                End If
            End If

            'wait for mediainfo to update the nfo
            While bwMediaInfo.IsBusy
                Application.DoEvents()
            End While

            If bwLoadEpInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If


        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            e.Cancel = True
        End Try


    End Sub

    Private Sub bwLoadEpInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadEpInfo.RunWorkerCompleted

        '//
        ' Thread finished: display it
        '\\

        Try
            If Not e.Cancelled Then
                Me.fillScreenInfoWithEpisode()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Delegate Sub MydtMediaUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
    Sub dtMediaUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
        drow.Item(i) = v
    End Sub

    Delegate Sub MydtShowsUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
    Sub dtShowsUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
        drow.Item(i) = v
    End Sub

    Delegate Sub MydtEpsUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
    Sub dtEpsUpdate(ByVal drow As DataRow, ByVal i As Integer, ByVal v As Object)
        drow.Item(i) = v
    End Sub

    Private Sub bwScraper_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwScraper.DoWork

        '//
        ' Thread to handle scraping
        '\\

        Dim myDelegate As MydtMediaUpdate = New MydtMediaUpdate(AddressOf dtMediaUpdate)
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        Dim TMDB As New TMDB.Scraper
        Dim IMPA As New IMPA.Scraper
        Dim Trailer As New Trailers
        Dim iCount As Integer = 0
        Dim tURL As String = String.Empty
        Dim Poster As New Images
        Dim Fanart As New Images
        Dim scrapeMovie As New Structures.DBMovie
        Dim doSave As Boolean = False
        Dim pResults As New Containers.ImgResult
        Dim fResults As New Containers.ImgResult
        Dim didEts As Boolean = False
        Dim tTitle As String = String.Empty
        Dim OldTitle As String = String.Empty

        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction

            Try
                If Me.dtMedia.Rows.Count > 0 Then

                    Select Case Args.scrapeType
                        Case Enums.ScrapeType.FullAuto, Enums.ScrapeType.NewAuto, Enums.ScrapeType.MarkAuto, Enums.ScrapeType.FullAsk, Enums.ScrapeType.NewAsk, Enums.ScrapeType.MarkAsk, Enums.ScrapeType.FilterAsk, Enums.ScrapeType.FilterAuto
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Select Case Args.scrapeType
                                    Case Enums.ScrapeType.NewAsk, Enums.ScrapeType.NewAuto
                                        If Not Convert.ToBoolean(drvRow.Item(10)) Then Continue For
                                    Case Enums.ScrapeType.MarkAsk, Enums.ScrapeType.MarkAuto
                                        If Not Convert.ToBoolean(drvRow.Item(11)) Then Continue For
                                    Case Enums.ScrapeType.FilterAsk, Enums.ScrapeType.FilterAuto
                                        Dim index As Integer = Me.bsMedia.Find("id", drvRow.Item(0))
                                        If Not index >= 0 Then Continue For
                                End Select

                                If Me.bwScraper.CancellationPending Then GoTo doCancel

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(15).ToString)

                                If Convert.ToBoolean(drvRow.Item(14)) Then Continue For

                                doSave = False

                                scrapeMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(drvRow.Item(0)))

                                OldTitle = scrapeMovie.Movie.Title

                                If Master.GlobalScrapeMod.NFO Then
                                    If Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) Then
                                        IMDB.GetMovieInfo(scrapeMovie.Movie.IMDBID, scrapeMovie.Movie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False, Args.Options)
                                    Else
                                        scrapeMovie.Movie = IMDB.GetSearchMovieInfo(drvRow.Item(15).ToString, New MediaContainers.Movie, Args.scrapeType, Args.Options)
                                    End If
                                    doSave = True
                                End If

                                If Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) Then

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Master.eSettings.ScanMediaInfo AndAlso Master.GlobalScrapeMod.Meta Then
                                        UpdateMediaInfo(scrapeMovie)
                                        doSave = True
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Master.GlobalScrapeMod.Poster Then
                                        Poster.Clear()
                                        If Poster.IsAllowedToDownload(scrapeMovie, Enums.ImageType.Posters) Then
                                            pResults = New Containers.ImgResult
                                            If Poster.GetPreferredImage(scrapeMovie.Movie.IMDBID, Enums.ImageType.Posters, pResults, scrapeMovie.Filename, False, If(Args.scrapeType = Enums.ScrapeType.FullAsk OrElse Args.scrapeType = Enums.ScrapeType.NewAsk OrElse Args.scrapeType = Enums.ScrapeType.MarkAsk, True, False)) Then
                                                If Not IsNothing(Poster.Image) Then
                                                    pResults.ImagePath = Poster.SaveAsPoster(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                                        scrapeMovie.PosterPath = pResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Thumb = pResults.Posters
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Enums.ScrapeType.FullAsk OrElse Args.scrapeType = Enums.ScrapeType.NewAsk OrElse Args.scrapeType = Enums.ScrapeType.MarkAsk Then
                                                    MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        pResults = dImgSelect.ShowDialog(scrapeMovie, Enums.ImageType.Posters)
                                                        If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                                            scrapeMovie.PosterPath = pResults.ImagePath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                                scrapeMovie.Movie.Thumb = pResults.Posters
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    didEts = False
                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Master.GlobalScrapeMod.Fanart Then
                                        Fanart.Clear()
                                        If Fanart.IsAllowedToDownload(scrapeMovie, Enums.ImageType.Fanart) Then
                                            fResults = New Containers.ImgResult
                                            didEts = True
                                            If Fanart.GetPreferredImage(scrapeMovie.Movie.IMDBID, Enums.ImageType.Fanart, fResults, scrapeMovie.Filename, Master.GlobalScrapeMod.Extra, If(Args.scrapeType = Enums.ScrapeType.FullAsk OrElse Args.scrapeType = Enums.ScrapeType.NewAsk OrElse Args.scrapeType = Enums.ScrapeType.MarkAsk, True, False)) Then
                                                If Not IsNothing(Fanart.Image) Then
                                                    fResults.ImagePath = Fanart.SaveAsFanart(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                                        scrapeMovie.FanartPath = fResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Fanart = fResults.Fanart
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Enums.ScrapeType.FullAsk OrElse Args.scrapeType = Enums.ScrapeType.NewAsk OrElse Args.scrapeType = Enums.ScrapeType.MarkAsk Then
                                                    MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))

                                                    Using dImgSelect As New dlgImgSelect
                                                        fResults = dImgSelect.ShowDialog(scrapeMovie, Enums.ImageType.Fanart)
                                                        If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                                            scrapeMovie.FanartPath = fResults.ImagePath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                                scrapeMovie.Movie.Fanart = fResults.Fanart
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Master.GlobalScrapeMod.Trailer Then
                                        tURL = Trailer.DownloadSingleTrailer(scrapeMovie.Filename, scrapeMovie.Movie.IMDBID, Convert.ToBoolean(drvRow.Item(2)), scrapeMovie.Movie.Trailer)
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

                                If Master.GlobalScrapeMod.Extra Then
                                    If Master.eSettings.AutoET AndAlso Not didEts Then
                                        Fanart.GetPreferredFAasET(scrapeMovie.Movie.IMDBID, scrapeMovie.Filename)
                                    End If
                                    If Master.eSettings.AutoThumbs > 0 AndAlso Convert.ToBoolean(drvRow.Item(2)) Then
                                        Dim ETasFA As String = ThumbGenerator.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs, False)
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

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                If Not String.IsNullOrEmpty(scrapeMovie.Movie.Title) Then
                                    tTitle = StringUtils.FilterTokens(scrapeMovie.Movie.Title)
                                    If Not OldTitle = scrapeMovie.Movie.Title OrElse String.IsNullOrEmpty(scrapeMovie.Movie.SortTitle) Then scrapeMovie.Movie.SortTitle = tTitle
                                    If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.Year) Then
                                        scrapeMovie.ListTitle = String.Format("{0} ({1})", tTitle, scrapeMovie.Movie.Year)
                                    Else
                                        scrapeMovie.ListTitle = tTitle
                                    End If
                                Else
                                    If FileUtils.Common.isVideoTS(drvRow.Item(1).ToString) Then
                                        scrapeMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(drvRow.Item(1).ToString).FullName).Name)
                                    ElseIf FileUtils.Common.isBDRip(drvRow.Item(1).ToString) Then
                                        scrapeMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(Directory.GetParent(drvRow.Item(1).ToString).FullName).FullName).Name)
                                    Else
                                        If Convert.ToBoolean(drvRow.Item(46)) AndAlso Convert.ToBoolean(drvRow.Item(2)) Then
                                            scrapeMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(drvRow.Item(1).ToString).Name)
                                        Else
                                            scrapeMovie.ListTitle = StringUtils.FilterName(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString))
                                        End If
                                    End If
                                    If Not OldTitle = scrapeMovie.Movie.Title OrElse String.IsNullOrEmpty(scrapeMovie.Movie.SortTitle) Then scrapeMovie.Movie.SortTitle = scrapeMovie.ListTitle
                                End If

                                Me.Invoke(myDelegate, New Object() {drvRow, 3, scrapeMovie.ListTitle})
                                Me.Invoke(myDelegate, New Object() {drvRow, 50, scrapeMovie.Movie.SortTitle})

                                If doSave AndAlso Master.eSettings.AutoRenameMulti AndAlso Master.GlobalScrapeMod.NFO Then
                                    FileFolderRenamer.RenameSingle(scrapeMovie, Master.eSettings.FoldersPattern, Master.eSettings.FilesPattern, True, doSave AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID), False)
                                Else
                                    Master.DB.SaveMovieToDB(scrapeMovie, False, True, doSave AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID))
                                End If

                                'use this one to check for need of load info
                                Me.bwScraper.ReportProgress(iCount, String.Format("[[{0}]]", drvRow.Item(0).ToString))
                                iCount += 1
                            Next

                        Case Enums.ScrapeType.UpdateAsk, Enums.ScrapeType.UpdateAuto

                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(15).ToString)

                                If Convert.ToBoolean(drvRow.Item(14)) Then Continue For

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                If (Not Convert.ToBoolean(drvRow.Item(4)) AndAlso Master.GlobalScrapeMod.Poster) OrElse (Not Convert.ToBoolean(drvRow.Item(5)) AndAlso Master.GlobalScrapeMod.Fanart) OrElse _
                                (Not Convert.ToBoolean(drvRow.Item(6)) AndAlso Master.GlobalScrapeMod.NFO) OrElse (Not Convert.ToBoolean(drvRow.Item(7)) AndAlso Master.GlobalScrapeMod.Trailer) OrElse _
                                (Not Convert.ToBoolean(drvRow.Item(9)) AndAlso Master.GlobalScrapeMod.Extra) Then

                                    doSave = False

                                    scrapeMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(drvRow.Item(0)))

                                    OldTitle = scrapeMovie.Movie.Title

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel

                                    If Not Convert.ToBoolean(drvRow.Item(6)) AndAlso Master.GlobalScrapeMod.NFO Then

                                        If String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) OrElse Not IMDB.GetMovieInfo(scrapeMovie.Movie.IMDBID, scrapeMovie.Movie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False, Args.Options) Then
                                            scrapeMovie.Movie = IMDB.GetSearchMovieInfo(drvRow.Item(15).ToString, New MediaContainers.Movie, Args.scrapeType, Args.Options)
                                            doSave = True
                                        End If

                                        If Master.eSettings.ScanMediaInfo AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) AndAlso Master.GlobalScrapeMod.Meta Then
                                            UpdateMediaInfo(scrapeMovie)
                                            doSave = True
                                        End If

                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Not Convert.ToBoolean(drvRow.Item(4)) AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) AndAlso Master.GlobalScrapeMod.Poster Then
                                        Poster.Clear()
                                        If Poster.IsAllowedToDownload(scrapeMovie, Enums.ImageType.Posters) Then
                                            pResults = New Containers.ImgResult
                                            If Poster.GetPreferredImage(scrapeMovie.Movie.IMDBID, Enums.ImageType.Posters, pResults, scrapeMovie.Filename, False, If(Args.scrapeType = Enums.ScrapeType.UpdateAsk, True, False)) Then
                                                If Not IsNothing(Poster.Image) Then
                                                    pResults.ImagePath = Poster.SaveAsPoster(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                                        scrapeMovie.PosterPath = pResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Thumb = pResults.Posters
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Enums.ScrapeType.UpdateAsk Then
                                                    MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        pResults = dImgSelect.ShowDialog(scrapeMovie, Enums.ImageType.Posters)
                                                        If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                                            scrapeMovie.PosterPath = pResults.ImagePath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                                scrapeMovie.Movie.Thumb = pResults.Posters
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    didEts = False
                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Not Convert.ToBoolean(drvRow.Item(5)) AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) AndAlso Master.GlobalScrapeMod.Fanart Then
                                        Fanart.Clear()
                                        If Fanart.IsAllowedToDownload(scrapeMovie, Enums.ImageType.Fanart) Then
                                            fResults = New Containers.ImgResult
                                            didEts = True
                                            If Fanart.GetPreferredImage(scrapeMovie.Movie.IMDBID, Enums.ImageType.Fanart, fResults, scrapeMovie.Filename, Master.GlobalScrapeMod.Extra, If(Args.scrapeType = Enums.ScrapeType.UpdateAsk, True, False)) Then
                                                If Not IsNothing(Fanart.Image) Then
                                                    fResults.ImagePath = Fanart.SaveAsFanart(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                                        scrapeMovie.FanartPath = fResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Fanart = fResults.Fanart
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Enums.ScrapeType.UpdateAsk Then
                                                    MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        fResults = dImgSelect.ShowDialog(scrapeMovie, Enums.ImageType.Fanart)
                                                        If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                                            scrapeMovie.FanartPath = fResults.ImagePath
                                                            Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                            If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                                scrapeMovie.Movie.Fanart = fResults.Fanart
                                                            End If
                                                        End If
                                                    End Using
                                                End If
                                            End If
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If Not Convert.ToBoolean(drvRow.Item(7)) AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.IMDBID) AndAlso Master.GlobalScrapeMod.Trailer Then
                                        tURL = Trailer.DownloadSingleTrailer(scrapeMovie.Filename, scrapeMovie.Movie.IMDBID, Convert.ToBoolean(drvRow.Item(2)), scrapeMovie.Movie.Trailer)
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

                                    If Not Convert.ToBoolean(drvRow.Item(9)) AndAlso Master.GlobalScrapeMod.Extra AndAlso Convert.ToBoolean(drvRow.Item(2)) Then
                                        If Master.eSettings.AutoET AndAlso Not didEts Then
                                            Fanart.GetPreferredFAasET(scrapeMovie.Movie.IMDBID, scrapeMovie.Filename)
                                        End If

                                        If Master.eSettings.AutoThumbs > 0 Then
                                            Dim ETasFA As String = ThumbGenerator.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs, False)
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

                                    If Not String.IsNullOrEmpty(scrapeMovie.Movie.Title) Then
                                        tTitle = StringUtils.FilterTokens(scrapeMovie.Movie.Title)
                                        If Not OldTitle = scrapeMovie.Movie.Title OrElse String.IsNullOrEmpty(scrapeMovie.Movie.SortTitle) Then scrapeMovie.Movie.SortTitle = tTitle
                                        If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.Year) Then
                                            scrapeMovie.ListTitle = String.Format("{0} ({1})", tTitle, scrapeMovie.Movie.Year)
                                        Else
                                            scrapeMovie.ListTitle = tTitle
                                        End If
                                    Else
                                        If FileUtils.Common.isVideoTS(drvRow.Item(1).ToString) Then
                                            scrapeMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(drvRow.Item(1).ToString).FullName).Name)
                                        ElseIf FileUtils.Common.isBDRip(drvRow.Item(1).ToString) Then
                                            scrapeMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(Directory.GetParent(drvRow.Item(1).ToString).FullName).FullName).Name)
                                        Else
                                            If Convert.ToBoolean(drvRow.Item(46)) AndAlso Convert.ToBoolean(drvRow.Item(2)) Then
                                                scrapeMovie.ListTitle = StringUtils.FilterName(Directory.GetParent(drvRow.Item(1).ToString).Name)
                                            Else
                                                scrapeMovie.ListTitle = StringUtils.FilterName(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString))
                                            End If
                                        End If
                                        If Not OldTitle = scrapeMovie.Movie.Title OrElse String.IsNullOrEmpty(scrapeMovie.Movie.SortTitle) Then scrapeMovie.Movie.SortTitle = scrapeMovie.ListTitle
                                    End If

                                    Me.Invoke(myDelegate, New Object() {drvRow, 3, scrapeMovie.ListTitle})
                                    Me.Invoke(myDelegate, New Object() {drvRow, 50, scrapeMovie.Movie.SortTitle})
                                    If doSave Then Me.Invoke(myDelegate, New Object() {drvRow, 6, True})

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel

                                    If doSave AndAlso Master.eSettings.AutoRenameMulti AndAlso Master.GlobalScrapeMod.NFO Then
                                        FileFolderRenamer.RenameSingle(scrapeMovie, Master.eSettings.FoldersPattern, Master.eSettings.FilesPattern, True, doSave, False)
                                    Else
                                        Master.DB.SaveMovieToDB(scrapeMovie, False, True, doSave)
                                    End If

                                End If

                                'use this one to check for need of load info
                                Me.bwScraper.ReportProgress(iCount, String.Format("[[{0}]]", drvRow.Item(0).ToString))
                                iCount += 1

                            Next
                            ' above 2 arent scraper.. will need to relocate it
                        Case Enums.ScrapeType.CleanFolders
                            Dim fDeleter As New FileUtils.Delete
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(15))
                                iCount += 1
                                If Convert.ToBoolean(drvRow.Item(14)) Then Continue For

                                If Me.bwScraper.CancellationPending Then GoTo doCancel

                                scrapeMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(drvRow.Item(0)))
                                If fDeleter.DeleteFiles(True, scrapeMovie) Then
                                    Me.RefreshMovie(Convert.ToInt64(drvRow.Item(0)), True, True)
                                    Me.bwScraper.ReportProgress(iCount, String.Format("[[{0}]]", drvRow.Item(0).ToString))
                                End If
                            Next



                        Case Enums.ScrapeType.CopyBD
                            Dim sPath As String = String.Empty
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(15).ToString)
                                iCount += 1

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                sPath = drvRow.Item(40).ToString
                                If Not String.IsNullOrEmpty(sPath) Then
                                    If FileUtils.Common.isVideoTS(sPath) Then
                                        If Master.eSettings.VideoTSParent Then
                                            FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), "-fanart.jpg")))
                                        Else
                                            If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                                FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")))
                                            Else
                                                FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)))
                                            End If
                                        End If
                                    ElseIf FileUtils.Common.isBDRip(sPath) Then
                                        If Master.eSettings.VideoTSParent Then
                                            FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName).FullName, Directory.GetParent(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName).Name), "-fanart.jpg")))
                                        Else
                                            If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                                FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName).Name, "-fanart.jpg")))
                                            Else
                                                FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)))
                                            End If
                                        End If
                                    Else
                                        If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                            FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString), "-fanart.jpg")))
                                        Else
                                            FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)))
                                        End If

                                    End If
                                End If
                            Next
                    End Select
                End If

            Catch ex As Exception
                ScraperDone = True
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

doCancel:
            If Not Args.scrapeType = Enums.ScrapeType.CopyBD Then
                SQLtransaction.Commit()
            End If
        End Using
        e.Result = Args.scrapeType

        pResults = Nothing
        fResults = Nothing

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
                If Me.dgvMediaList.SelectedRows(0).Cells(0).Value.ToString = e.UserState.ToString.Replace("[[", String.Empty).Replace("]]", String.Empty).Trim Then
                    Me.LoadInfo(Convert.ToInt32(Me.dgvMediaList.SelectedRows(0).Cells(0).Value), Me.dgvMediaList.SelectedRows(0).Cells(1).Value.ToString, True, False)
                End If
            Else
                Me.SetStatus(e.UserState.ToString)
                Me.tspbLoading.Value = e.ProgressPercentage
            End If
        End If

        Me.dgvMediaList.Invalidate()
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

                If DirectCast(e.Result, Enums.ScrapeType) = Enums.ScrapeType.CleanFolders Then
                    'only rescan media if expert cleaner and videos are not whitelisted 
                    'since the db is updated during cleaner now.
                    If Master.eSettings.ExpertCleaner AndAlso Not Master.eSettings.CleanWhitelistVideo Then
                        Me.LoadMedia(New Structures.Scans With {.Movies = True})
                    Else
                        Me.FillList(0)
                    End If
                Else
                    If Me.dgvMediaList.SelectedRows.Count > 0 Then
                        Me.FillList(Me.dgvMediaList.SelectedRows(0).Index)
                    Else
                        Me.FillList(0)
                    End If
                End If
            End If
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.SetStatus(String.Empty)

            Me.SetControlsEnabled(True, True)
            Me.EnableFilters(True)

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwRefreshMovies_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwRefreshMovies.DoWork
        Dim iCount As Integer = 0
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            For Each sRow As DataRow In Me.dtMedia.Rows
                If Me.bwScraper.CancellationPending Then Return
                Me.bwRefreshMovies.ReportProgress(iCount, sRow.Item(1))
                Me.RefreshMovie(Convert.ToInt64(sRow.Item(0)), True)
                iCount += 1
            Next
            SQLtransaction.Commit()
        End Using
    End Sub

    Private Sub bwRefreshMovies_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwRefreshMovies.ProgressChanged
        Me.SetStatus(e.UserState.ToString)
        Me.tspbLoading.Value = e.ProgressPercentage
    End Sub

    Private Sub bwRefreshMovies_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwRefreshMovies.RunWorkerCompleted

        Me.tslLoading.Text = String.Empty
        Me.tspbLoading.Visible = False
        Me.tslLoading.Visible = False
        Me.SetControlsEnabled(True)
        Me.EnableFilters(True)

        Me.FillList(0)
    End Sub

    Private Sub bwCleanDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCleanDB.DoWork
        Master.DB.Clean(True, True)
    End Sub

    Private Sub bwCleanDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwCleanDB.RunWorkerCompleted

        Me.tslLoading.Text = String.Empty
        Me.tspbLoading.Visible = False
        Me.tslLoading.Visible = False
        Me.SetControlsEnabled(True)
        Me.EnableFilters(True)

        Me.FillList(0)
    End Sub

#End Region '*** Background Workers


#Region "Routines/Functions"

    ' ########################################
    ' ###### GENERAL ROUTINES/FUNCTIONS ######
    ' ########################################
    Private Sub ApplyTheme(ByVal tType As Theming.ThemeType)
        Me.pnlInfoPanel.SuspendLayout()

        Me.currThemeType = tType

        tTheme.ApplyTheme(tType)

        Select Case If(Me.tabsMain.SelectedIndex = 0, aniType, aniShowType)
            Case 1
                If Me.btnMid.Visible Then
                    Me.pnlInfoPanel.Height = Me._ipmid
                    Me.btnUp.Enabled = True
                    Me.btnMid.Enabled = False
                    Me.btnDown.Enabled = True
                ElseIf Me.btnUp.Visible Then
                    Me.pnlInfoPanel.Height = Me._ipup
                    If Me.tabsMain.SelectedIndex = 0 Then
                        aniType = 2
                    Else
                        aniShowType = 2
                    End If
                    Me.btnUp.Enabled = False
                    Me.btnMid.Enabled = True
                    Me.btnDown.Enabled = True
                Else
                    Me.pnlInfoPanel.Height = 25
                    If Me.tabsMain.SelectedIndex = 0 Then
                        aniType = 0
                    Else
                        aniShowType = 0
                    End If
                    Me.btnUp.Enabled = True
                    Me.btnMid.Enabled = True
                    Me.btnDown.Enabled = False
                End If
            Case 2
                If Me.btnUp.Visible Then
                    Me.pnlInfoPanel.Height = Me._ipup
                    Me.btnUp.Enabled = False
                    Me.btnMid.Enabled = True
                    Me.btnDown.Enabled = True
                ElseIf Me.btnMid.Visible Then
                    Me.pnlInfoPanel.Height = Me._ipmid

                    If Me.tabsMain.SelectedIndex = 0 Then
                        aniType = 1
                    Else
                        aniShowType = 1
                    End If

                    Me.btnUp.Enabled = True
                    Me.btnMid.Enabled = False
                    Me.btnDown.Enabled = True
                Else
                    Me.pnlInfoPanel.Height = 25
                    If Me.tabsMain.SelectedIndex = 0 Then
                        aniType = 0
                    Else
                        aniShowType = 0
                    End If
                    Me.btnUp.Enabled = True
                    Me.btnMid.Enabled = True
                    Me.btnDown.Enabled = False
                End If
            Case Else
                Me.pnlInfoPanel.Height = 25
                If Me.tabsMain.SelectedIndex = 0 Then
                    aniType = 0
                Else
                    aniShowType = 0
                End If

                Me.btnUp.Enabled = True
                Me.btnMid.Enabled = True
                Me.btnDown.Enabled = False
        End Select

        Me.pbActLoad.Visible = False
        Me.pbActors.Image = My.Resources.actor_silhouette

        Me.pnlInfoPanel.ResumeLayout()
    End Sub

    Private Sub SetUp(ByVal doTheme As Boolean)

        Try
            With Me
                .MinimumSize = New Size(1024, 768)

                .btnSortDate.Tag = String.Empty
                .pnlFilterGenre.Tag = String.Empty
                .pnlFilterSource.Tag = String.Empty
                .btnSortTitle.Tag = String.Empty
                .btnIMDBRating.Tag = String.Empty
                .FileToolStripMenuItem.Text = Master.eLang.GetString(1, "&File")
                .ExitToolStripMenuItem.Text = Master.eLang.GetString(2, "E&xit")
                .EditToolStripMenuItem.Text = Master.eLang.GetString(3, "&Edit")
                .SettingsToolStripMenuItem.Text = Master.eLang.GetString(4, "&Settings...")
                .ModuleSettingToolStripMenuItem.Text = Master.eLang.GetString(999, "Module Settings")
                .HelpToolStripMenuItem.Text = Master.eLang.GetString(5, "&Help")
                .AboutToolStripMenuItem.Text = Master.eLang.GetString(6, "&About...")
                .tslLoading.Text = Master.eLang.GetString(7, "Loading Media:")
                .ToolsToolStripMenuItem.Text = Master.eLang.GetString(8, "&Tools")
                .CleanFoldersToolStripMenuItem.Text = Master.eLang.GetString(9, "&Clean Files")
                .ConvertFileSourceToFolderSourceToolStripMenuItem.Text = Master.eLang.GetString(10, "&Sort Files Into Folders")
                .CopyExistingFanartToBackdropsFolderToolStripMenuItem.Text = Master.eLang.GetString(11, "Copy Existing Fanart To &Backdrops Folder")
                .RenamerToolStripMenuItem.Text = Master.eLang.GetString(13, "Bulk &Renamer")
                .SetsManagerToolStripMenuItem.Text = Master.eLang.GetString(14, "Sets &Manager")
                .OfflineMediaManagerToolStripMenuItem.Text = Master.eLang.GetString(15, "&Offline Media Manager")
                .ExportMoviesListToolStripMenuItem.Text = Master.eLang.GetString(16, "&Export Movies List")
                .ClearAllCachesToolStripMenuItem.Text = Master.eLang.GetString(17, "Clear &All Caches")
                .RefreshAllMoviesToolStripMenuItem.Text = Master.eLang.GetString(18, "Re&load All Movies")
                .lblGFilClose.Text = Master.eLang.GetString(19, "Close")
                .lblSFilClose.Text = Master.eLang.GetString(19, "Close")
                .Label4.Text = Master.eLang.GetString(20, "Genres")
                .Label8.Text = Master.eLang.GetString(602, "Sources")
                .cmnuTitle.Text = Master.eLang.GetString(21, "Title")
                .cmnuRefresh.Text = Master.eLang.GetString(22, "Reload")
                .cmnuMark.Text = Master.eLang.GetString(23, "Mark")
                .cmnuLock.Text = Master.eLang.GetString(24, "Lock")
                .cmnuEditMovie.Text = Master.eLang.GetString(25, "Edit Movie")
                .cmuRenamer.Text = Master.eLang.GetString(168, "Rename")
                .cmnuRenameAuto.Text = Master.eLang.GetString(630, "Auto")
                .cmnuRenameManual.Text = Master.eLang.GetString(631, "Manual")
                .GenresToolStripMenuItem.Text = Master.eLang.GetString(20, "Genres")
                .LblGenreStripMenuItem2.Text = Master.eLang.GetString(27, ">> Select Genre <<")
                .AddGenreToolStripMenuItem.Text = Master.eLang.GetString(28, "Add")
                .SetGenreToolStripMenuItem.Text = Master.eLang.GetString(29, "Set")
                .RemoveGenreToolStripMenuItem.Text = Master.eLang.GetString(30, "Remove")
                .cmnuRescrape.Text = Master.eLang.GetString(31, "Re-scrape IMDB")
                .cmnuSearchNew.Text = Master.eLang.GetString(32, "Change Movie")
                .OpenContainingFolderToolStripMenuItem.Text = Master.eLang.GetString(33, "Open Containing Folder")
                .RemoveToolStripMenuItem.Text = Master.eLang.GetString(30, "Remove")
                .DeleteMovieToolStripMenuItem.Text = Master.eLang.GetString(34, "Delete Movie")
                .RemoveFromDatabaseToolStripMenuItem.Text = Master.eLang.GetString(646, "Remove From Database")
                .btnMarkAll.Text = Master.eLang.GetString(35, "Mark All")
                .tabMovies.Text = Master.eLang.GetString(36, "Movies")
                .tabTV.Text = Master.eLang.GetString(653, "TV")
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
                .lblCertsHeader.Text = Master.eLang.GetString(56, "Certification(s)")
                .lblReleaseDateHeader.Text = Master.eLang.GetString(57, "Release Date")
                .btnMetaDataRefresh.Text = Master.eLang.GetString(58, "Refresh")
                .lblMetaDataHeader.Text = Master.eLang.GetString(59, "Meta Data")
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
                .CurrentFilterToolStripMenuItem.Text = Master.eLang.GetString(624, "Current Filter")
                .AutomaticForceBestMatchToolStripMenuItem2.Text = .FullAutoToolStripMenuItem.Text
                .mnuFilterAutoAll.Text = .mnuAllAutoAll.Text
                .mnuFilterAutoNfo.Text = .mnuAllAutoNfo.Text
                .mnuFilterAutoPoster.Text = .mnuAllAutoPoster.Text
                .mnuFilterAutoFanart.Text = .mnuAllAutoFanart.Text
                .mnuFilterAutoExtra.Text = .mnuAllAutoExtra.Text
                .mnuFilterAutoTrailer.Text = .mnuAllAutoTrailer.Text
                .mnuFilterAutoMI.Text = .mnuAllAutoMI.Text
                .AskRequireInputIfNoExactMatchToolStripMenuItem1.Text = .FullAskToolStripMenuItem.Text
                .mnuFilterAskAll.Text = .mnuAllAutoAll.Text
                .mnuFilterAskNfo.Text = .mnuAllAutoNfo.Text
                .mnuFilterAskPoster.Text = .mnuAllAutoPoster.Text
                .mnuFilterAskFanart.Text = .mnuAllAutoFanart.Text
                .mnuFilterAskExtra.Text = .mnuAllAutoExtra.Text
                .mnuFilterAskTrailer.Text = .mnuAllAutoTrailer.Text
                .mnuFilterAskMI.Text = .mnuAllAutoMI.Text
                .mnuMoviesUpdate.Text = Master.eLang.GetString(36, "Movies")
                .mnuTVShowUpdate.Text = Master.eLang.GetString(698, "TV Shows")
                .cmnuEditEpisode.Text = Master.eLang.GetString(656, "Edit Episode")
                .cmnuEditShow.Text = Master.eLang.GetString(663, "Edit Show")
                .CustomUpdaterToolStripMenuItem.Text = Master.eLang.GetString(81, "Custom Scraper...")
                .tsbRefreshMedia.Text = Master.eLang.GetString(82, "Update Library")
                .tsbUpdateXBMC.Text = Master.eLang.GetString(83, "Initiate XBMC Update")
                .Label6.Text = Master.eLang.GetString(579, "File Source:")
                .GroupBox1.Text = Master.eLang.GetString(600, "Extra Sorting")
                .btnSortDate.Text = Master.eLang.GetString(601, "Date Added")
                .cmnuMetaData.Text = Master.eLang.GetString(603, "Edit Meta Data")
                .btnSortTitle.Text = Master.eLang.GetString(642, "Sort Title")
                .btnIMDBRating.Text = Master.eLang.GetString(707, "IMDB Rating")
                .DonateToolStripMenuItem.Text = Master.eLang.GetString(708, "Donate")
                .CleanDatabaseToolStripMenuItem.Text = Master.eLang.GetString(709, "Clean Database")

                Dim TT As ToolTip = New System.Windows.Forms.ToolTip(.components)
                .tsbAutoPilot.ToolTipText = Master.eLang.GetString(84, "Scrape/download data from the internet for multiple movies.")
                .tsbRefreshMedia.ToolTipText = Master.eLang.GetString(85, "Scans sources for new content and cleans database.")
                .tsbUpdateXBMC.ToolTipText = Master.eLang.GetString(86, "Sends a command to XBMC to begin its internal ""Update Library"" process.")
                TT.SetToolTip(.btnMarkAll, Master.eLang.GetString(87, "Mark or Unmark all movies in the list."))
                TT.SetToolTip(.txtSearch, Master.eLang.GetString(88, "Search the movie titles by entering text here."))
                TT.SetToolTip(.btnPlay, Master.eLang.GetString(89, "Play the movie file with the system default media player."))
                TT.SetToolTip(.btnMetaDataRefresh, Master.eLang.GetString(90, "Rescan and save the meta data for the selected movie."))
                TT.SetToolTip(.chkFilterDupe, Master.eLang.GetString(91, "Display only movies that have duplicate IMDB IDs."))
                TT.SetToolTip(.chkFilterTolerance, Master.eLang.GetString(92, "Display only movies whose title matching is out of tolerance."))
                TT.SetToolTip(.chkFilterMissing, Master.eLang.GetString(93, "Display only movies that have items missing."))
                TT.SetToolTip(.chkFilterNew, Master.eLang.GetString(94, "Display only new movies."))
                TT.SetToolTip(.chkFilterMark, Master.eLang.GetString(95, "Display only marked movies."))
                TT.SetToolTip(.chkFilterLock, Master.eLang.GetString(96, "Display only locked movies."))
                TT.SetToolTip(.txtFilterSource, Master.eLang.GetString(97, "Display only movies from the selected source."))
                TT.SetToolTip(.cbFilterFileSource, Master.eLang.GetString(580, "Display only movies from the selected file source."))
                TT.Active = True

                .cbSearch.Items.Clear()
                .cbSearch.Items.AddRange(New Object() {Master.eLang.GetString(21, "Title"), Master.eLang.GetString(100, "Actor"), Master.eLang.GetString(62, "Director")})

                If doTheme Then .ApplyTheme(Theming.ThemeType.Movies)

            End With
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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

    Public Sub LoadMedia(ByVal Scan As Structures.Scans, Optional ByVal SourceName As String = "")

        '//
        ' Begin threads to fill datagrid with media data
        '\\


        Try
            Me.SetStatus(Master.eLang.GetString(116, "Performing Preliminary Tasks (Gathering Data)..."))
            Me.tspbLoading.ProgressBar.Style = ProgressBarStyle.Marquee
            Me.tspbLoading.MarqueeAnimationSpeed = 25
            Me.tspbLoading.Visible = True

            Application.DoEvents()

            Me.ClearInfo()
            Me.ClearFilters()
            Me.EnableFilters(False)

            Me.SetControlsEnabled(False)
            Me.tabMovies.Text = Master.eLang.GetString(36, "Movies")
            Me.tabTV.Text = Master.eLang.GetString(653, "TV")
            Me.txtSearch.Text = String.Empty

            Me.fScanner.CancelAndWait()

            If Scan.Movies Then
                Me.dgvMediaList.DataSource = Nothing
            End If

            If Scan.TV Then
                Me.dgvTVShows.DataSource = Nothing
                Me.dgvTVSeasons.DataSource = Nothing
                Me.dgvTVEpisodes.DataSource = Nothing
            End If

            Me.fScanner.Start(Scan, SourceName)

        Catch ex As Exception
            Me.LoadingDone = True
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Me.ShowNoInfo(False)

            If doMI Then
                If Me.bwMediaInfo.IsBusy Then Me.bwMediaInfo.CancelAsync()

                Me.txtMetaData.Clear()
                Me.pbMILoading.Visible = True

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
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub LoadShowInfo(ByVal ID As Integer)

        Try
            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.tabsMain.Enabled = False
            Me.ShowNoInfo(False)

            If Not Me.currThemeType = Theming.ThemeType.Show Then Me.ApplyTheme(Theming.ThemeType.Show)

            Me.ClearInfo()

            Me.bwLoadShowInfo = New System.ComponentModel.BackgroundWorker
            Me.bwLoadShowInfo.WorkerSupportsCancellation = True
            Me.bwLoadShowInfo.RunWorkerAsync(New Arguments With {.ID = ID})

            Me.FillSeasons(ID)

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub LoadSeasonInfo(ByVal ShowID As Integer, ByVal Season As Integer)

        Try
            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.tabsMain.Enabled = False
            Me.ShowNoInfo(False)

            If Not Me.currThemeType = Theming.ThemeType.Show Then
                Me.ApplyTheme(Theming.ThemeType.Show)
                Me.ClearInfo()
            End If

            Me.bwLoadSeasonInfo = New System.ComponentModel.BackgroundWorker
            Me.bwLoadSeasonInfo.WorkerSupportsCancellation = True
            Me.bwLoadSeasonInfo.RunWorkerAsync(New Arguments With {.ID = ShowID, .Season = Season})

            Me.FillEpisodes(ShowID, Season)

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub LoadEpisodeInfo(ByVal ID As Integer)

        Try
            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.tabsMain.Enabled = False
            Me.ShowNoInfo(False)

            If Not Me.currThemeType = Theming.ThemeType.Episode Then Me.ApplyTheme(Theming.ThemeType.Episode)

            Me.ClearInfo()

            Me.bwLoadEpInfo = New System.ComponentModel.BackgroundWorker
            Me.bwLoadEpInfo.WorkerSupportsCancellation = True
            Me.bwLoadEpInfo.RunWorkerAsync(New Arguments With {.ID = ID})
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Public Sub ClearInfo()

        '//
        ' Reset all info fields
        '\\
        Try
            With Me
                .InfoCleared = True

                If .bwDownloadPic.IsBusy Then .bwDownloadPic.CancelAsync()
                If .bwLoadInfo.IsBusy Then .bwLoadInfo.CancelAsync()
                If .bwLoadShowInfo.IsBusy Then .bwLoadShowInfo.CancelAsync()
                If .bwLoadSeasonInfo.IsBusy Then .bwLoadSeasonInfo.CancelAsync()
                If .bwLoadEpInfo.IsBusy Then .bwLoadEpInfo.CancelAsync()

                While .bwDownloadPic.IsBusy OrElse .bwLoadInfo.IsBusy OrElse .bwLoadShowInfo.IsBusy OrElse _
                        .bwLoadSeasonInfo.IsBusy OrElse .bwLoadEpInfo.IsBusy
                    Application.DoEvents()
                End While

                If Not IsNothing(.pbFanart.Image) Then
                    .pbFanart.Image.Dispose()
                    .pbFanart.Image = Nothing
                End If
                .MainFanart.Clear()

                If Not IsNothing(.pbPoster.Image) Then
                    .pbPoster.Image.Dispose()
                    .pbPoster.Image = Nothing
                End If
                .pnlPoster.Visible = False

                .MainPoster.Clear()

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
                ToolTips.SetToolTip(pbStar1, "")
                ToolTips.SetToolTip(pbStar2, "")
                ToolTips.SetToolTip(pbStar3, "")
                ToolTips.SetToolTip(pbStar4, "")
                ToolTips.SetToolTip(pbStar5, "")


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

                .txtMetaData.Text = String.Empty
                .pnlTop.Visible = False

                Application.DoEvents()
            End With
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub fillScreenInfoWithMovie()

        Dim g As Graphics
        Dim strSize As String
        Dim lenSize As Integer
        Dim rect As Rectangle

        Try
            Me.SuspendLayout()
            Me.pnlTop.Visible = True
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

            Me.alActors = New List(Of String)

            If Master.currMovie.Movie.Actors.Count > 0 Then
                Me.pbActors.Image = My.Resources.actor_silhouette
                For Each imdbAct As MediaContainers.Person In Master.currMovie.Movie.Actors
                    If Not String.IsNullOrEmpty(imdbAct.Thumb) Then
                        If Not imdbAct.Thumb.ToLower.IndexOf("addtiny.gif") > 0 AndAlso Not imdbAct.Thumb.ToLower.IndexOf("no_photo") > 0 Then
                            Me.alActors.Add(imdbAct.Thumb)
                        Else
                            Me.alActors.Add("none")
                        End If
                    Else
                        Me.alActors.Add("none")
                    End If

                    If String.IsNullOrEmpty(imdbAct.Role.Trim) Then
                        Me.lstActors.Items.Add(imdbAct.Name.Trim)
                    Else
                        Me.lstActors.Items.Add(String.Format("{0} as {1}", imdbAct.Name.Trim, imdbAct.Role.Trim))
                    End If
                Next
                Me.lstActors.SelectedIndex = 0
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.MPAA) Then
                Dim tmpRatingImg As Image = APIXML.GetRatingImage(Master.currMovie.Movie.MPAA)
                If Not IsNothing(tmpRatingImg) Then
                    Me.pbMPAA.Image = tmpRatingImg
                    Me.MoveMPAA()
                    Me.pnlMPAA.Visible = True
                Else
                    Me.pnlMPAA.Visible = False
                End If
            End If

            Dim tmpRating As Single = NumUtils.ConvertToSingle(Master.currMovie.Movie.Rating)
            If tmpRating > 0 Then
                Me.BuildStars(tmpRating)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Genre) Then
                Me.createGenreThumbs(Master.currMovie.Movie.Genre)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Movie.Studio) Then
                Me.pbStudio.Image = APIXML.GetStudioImage(Master.currMovie.Movie.Studio.ToLower) 'ByDef all images file a lower case
            Else
                Me.pbStudio.Image = APIXML.GetStudioImage("####")
            End If

            If Master.eSettings.ScanMediaInfo Then
                Me.SetAVImages(APIXML.GetAVImages(Master.currMovie.Movie.FileInfo, Master.currMovie.Filename))
                Me.pnlInfoIcons.Width = pbVideo.Width + pbVType.Width + pbResolution.Width + pbAudio.Width + pbChannels.Width + pbStudio.Width + 6
                Me.pbStudio.Left = pbVideo.Width + pbVType.Width + pbResolution.Width + pbAudio.Width + pbChannels.Width + 5
            Else
                Me.pnlInfoIcons.Width = pbStudio.Width + 1
                Me.pbStudio.Left = 0
            End If

            Me.lblDirector.Text = Master.currMovie.Movie.Director

            Me.txtIMDBID.Text = Master.currMovie.Movie.IMDBID

            Me.txtFilePath.Text = Master.currMovie.Filename

            Me.lblReleaseDate.Text = Master.currMovie.Movie.ReleaseDate
            Me.txtCerts.Text = Master.currMovie.Movie.Certification

            Me.txtMetaData.Text = NFO.FIToString(Master.currMovie.Movie.FileInfo)

            If Not IsNothing(Me.MainPoster.Image) Then
                Me.pbPosterCache.Image = Me.MainPoster.Image
                ImageUtils.ResizePB(Me.pbPoster, Me.pbPosterCache, Me.PosterMaxHeight, Me.PosterMaxWidth)
                ImageUtils.SetGlassOverlay(Me.pbPoster)
                Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)

                If Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbPoster.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainPoster.Image.Width, Me.MainPoster.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle(Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2 - 15), Me.pbPoster.Height - 25, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2), Me.pbPoster.Height - 20)
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

            If Not IsNothing(Me.MainFanart.Image) Then
                Me.pbFanartCache.Image = Me.MainFanart.Image

                ImageUtils.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)

                If Not IsNothing(pbFanart.Image) AndAlso Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbFanart.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainFanart.Image.Width, Me.MainFanart.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle((pbFanart.Image.Width - lenSize) - 40, 10, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbFanart.Image.Width - lenSize) - 25, 15)
                End If
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

            Me.InfoCleared = False

            If Not bwScraper.IsBusy AndAlso Not bwRefreshMovies.IsBusy AndAlso Not bwCleanDB.IsBusy Then
                Me.SetControlsEnabled(True)
                Me.EnableFilters(True)
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.ResumeLayout()
    End Sub

    Private Sub fillScreenInfoWithShow()
        Dim g As Graphics
        Dim strSize As String
        Dim lenSize As Integer
        Dim rect As Rectangle

        Try
            Me.SuspendLayout()
            Me.pnlTop.Visible = True
            If Not String.IsNullOrEmpty(Master.currShow.TVShow.Title) Then
                Me.lblTitle.Text = Master.currShow.TVShow.Title
            End If

            Me.txtPlot.Text = Master.currShow.TVShow.Plot
            Me.lblRuntime.Text = String.Format(Master.eLang.GetString(645, "Premiered: {0}"), If(String.IsNullOrEmpty(Master.currShow.TVShow.Premiered), "?", Master.currShow.TVShow.Premiered))

            Me.alActors = New List(Of String)

            If Master.currShow.TVShow.Actors.Count > 0 Then
                Me.pbActors.Image = My.Resources.actor_silhouette
                For Each imdbAct As MediaContainers.Person In Master.currShow.TVShow.Actors
                    If Not String.IsNullOrEmpty(imdbAct.Thumb) Then
                        If Not imdbAct.Thumb.ToLower.IndexOf("addtiny.gif") > 0 AndAlso Not imdbAct.Thumb.ToLower.IndexOf("no_photo") > 0 Then
                            Me.alActors.Add(imdbAct.Thumb)
                        Else
                            Me.alActors.Add("none")
                        End If
                    Else
                        Me.alActors.Add("none")
                    End If

                    If String.IsNullOrEmpty(imdbAct.Role.Trim) Then
                        Me.lstActors.Items.Add(imdbAct.Name.Trim)
                    Else
                        Me.lstActors.Items.Add(String.Format("{0} as {1}", imdbAct.Name.Trim, imdbAct.Role.Trim))
                    End If
                Next
                Me.lstActors.SelectedIndex = 0
            End If

            If Not String.IsNullOrEmpty(Master.currShow.TVShow.MPAA) Then
                Dim tmpRatingImg As Image = APIXML.GetTVRatingImage(Master.currShow.TVShow.MPAA)
                If Not IsNothing(tmpRatingImg) Then
                    Me.pbMPAA.Image = tmpRatingImg
                    Me.MoveMPAA()
                    Me.pnlMPAA.Visible = True
                Else
                    Me.pnlMPAA.Visible = False
                End If
            End If

            Dim tmpRating As Single = NumUtils.ConvertToSingle(Master.currShow.TVShow.Rating)
            If tmpRating > 0 Then
                Me.BuildStars(tmpRating)
            End If

            If Not String.IsNullOrEmpty(Master.currShow.TVShow.Genre) Then
                Me.createGenreThumbs(Master.currShow.TVShow.Genre)
            End If

            If Not String.IsNullOrEmpty(Master.currShow.TVShow.Studio) Then
                Me.pbStudio.Image = APIXML.GetStudioImage(Master.currShow.TVShow.Studio)
            Else
                Me.pbStudio.Image = APIXML.GetStudioImage("####")
            End If

            Me.pnlInfoIcons.Width = pbStudio.Width + 1
            Me.pbStudio.Left = 0

            If Not IsNothing(Me.MainPoster.Image) Then
                Me.pbPosterCache.Image = Me.MainPoster.Image
                ImageUtils.ResizePB(Me.pbPoster, Me.pbPosterCache, Me.PosterMaxHeight, Me.PosterMaxWidth)
                ImageUtils.SetGlassOverlay(Me.pbPoster)
                Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)

                If Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbPoster.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainPoster.Image.Width, Me.MainPoster.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle(Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2 - 15), Me.pbPoster.Height - 25, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2), Me.pbPoster.Height - 20)
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

            If Not IsNothing(Me.MainFanart.Image) Then
                Me.pbFanartCache.Image = Me.MainFanart.Image

                ImageUtils.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)

                If Not IsNothing(pbFanart.Image) AndAlso Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbFanart.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainFanart.Image.Width, Me.MainFanart.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle((pbFanart.Image.Width - lenSize) - 40, 10, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbFanart.Image.Width - lenSize) - 25, 15)
                End If
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

            Me.InfoCleared = False

            If Not bwScraper.IsBusy AndAlso Not bwRefreshMovies.IsBusy AndAlso Not bwCleanDB.IsBusy Then
                Me.SetControlsEnabled(True)
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.ResumeLayout()
    End Sub

    Private Sub fillScreenInfoWithSeason()
        Dim g As Graphics
        Dim strSize As String
        Dim lenSize As Integer
        Dim rect As Rectangle

        Try
            Me.SuspendLayout()
            Me.pnlTop.Visible = True
            If Not String.IsNullOrEmpty(Master.currShow.TVShow.Title) Then
                Me.lblTitle.Text = Master.currShow.TVShow.Title
            End If

            Me.txtPlot.Text = Master.currShow.TVShow.Plot
            Me.lblRuntime.Text = String.Format(Master.eLang.GetString(645, "Premiered: {0}"), If(String.IsNullOrEmpty(Master.currShow.TVShow.Premiered), "?", Master.currShow.TVShow.Premiered))

            Me.alActors = New List(Of String)

            If Master.currShow.TVShow.Actors.Count > 0 Then
                Me.pbActors.Image = My.Resources.actor_silhouette
                For Each imdbAct As MediaContainers.Person In Master.currShow.TVShow.Actors
                    If Not String.IsNullOrEmpty(imdbAct.Thumb) Then
                        If Not imdbAct.Thumb.ToLower.IndexOf("addtiny.gif") > 0 AndAlso Not imdbAct.Thumb.ToLower.IndexOf("no_photo") > 0 Then
                            Me.alActors.Add(imdbAct.Thumb)
                        Else
                            Me.alActors.Add("none")
                        End If
                    Else
                        Me.alActors.Add("none")
                    End If

                    If String.IsNullOrEmpty(imdbAct.Role.Trim) Then
                        Me.lstActors.Items.Add(imdbAct.Name.Trim)
                    Else
                        Me.lstActors.Items.Add(String.Format("{0} as {1}", imdbAct.Name.Trim, imdbAct.Role.Trim))
                    End If
                Next
                Me.lstActors.SelectedIndex = 0
            End If

            If Not String.IsNullOrEmpty(Master.currShow.TVShow.MPAA) Then
                Dim tmpRatingImg As Image = APIXML.GetTVRatingImage(Master.currShow.TVShow.MPAA)
                If Not IsNothing(tmpRatingImg) Then
                    Me.pbMPAA.Image = tmpRatingImg
                    Me.MoveMPAA()
                    Me.pnlMPAA.Visible = True
                Else
                    Me.pnlMPAA.Visible = False
                End If
            End If

            Dim tmpRating As Single = NumUtils.ConvertToSingle(Master.currShow.TVShow.Rating)
            If tmpRating > 0 Then
                Me.BuildStars(tmpRating)
            End If

            If Not String.IsNullOrEmpty(Master.currShow.TVShow.Genre) Then
                Me.createGenreThumbs(Master.currShow.TVShow.Genre)
            End If

            If Not String.IsNullOrEmpty(Master.currShow.TVShow.Studio) Then
                Me.pbStudio.Image = APIXML.GetStudioImage(Master.currShow.TVShow.Studio)
            Else
                Me.pbStudio.Image = APIXML.GetStudioImage("####")
            End If

            Me.pnlInfoIcons.Width = pbStudio.Width + 1
            Me.pbStudio.Left = 0

            If Not IsNothing(Me.MainPoster.Image) Then
                Me.pbPosterCache.Image = Me.MainPoster.Image
                ImageUtils.ResizePB(Me.pbPoster, Me.pbPosterCache, Me.PosterMaxHeight, Me.PosterMaxWidth)
                ImageUtils.SetGlassOverlay(Me.pbPoster)
                Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)

                If Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbPoster.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainPoster.Image.Width, Me.MainPoster.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle(Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2 - 15), Me.pbPoster.Height - 25, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2), Me.pbPoster.Height - 20)
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

            If Not IsNothing(Me.MainFanart.Image) Then
                Me.pbFanartCache.Image = Me.MainFanart.Image

                ImageUtils.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)

                If Not IsNothing(pbFanart.Image) AndAlso Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbFanart.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainFanart.Image.Width, Me.MainFanart.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle((pbFanart.Image.Width - lenSize) - 40, 10, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbFanart.Image.Width - lenSize) - 25, 15)
                End If
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

            Me.InfoCleared = False

            If Not bwScraper.IsBusy AndAlso Not bwRefreshMovies.IsBusy AndAlso Not bwCleanDB.IsBusy Then
                Me.SetControlsEnabled(True)
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.ResumeLayout()
    End Sub

    Private Sub fillScreenInfoWithEpisode()
        Dim g As Graphics
        Dim strSize As String
        Dim lenSize As Integer
        Dim rect As Rectangle

        Try
            Me.SuspendLayout()
            Me.pnlTop.Visible = True
            Me.lblTitle.Text = Master.currShow.TVEp.Title
            Me.txtPlot.Text = Master.currShow.TVEp.Plot
            Me.lblDirector.Text = Master.currShow.TVEp.Director
            Me.txtFilePath.Text = Master.currShow.Filename
            Me.lblRuntime.Text = String.Format(Master.eLang.GetString(647, "Aired: {0}"), If(String.IsNullOrEmpty(Master.currShow.TVEp.Aired), "?", Master.currShow.TVEp.Aired))

            Me.lblTagline.Text = String.Format(Master.eLang.GetString(648, "Season: {0}, Episode: {1}"), _
                            If(String.IsNullOrEmpty(Master.currShow.TVEp.Season.ToString), "?", Master.currShow.TVEp.Season.ToString), _
                            If(String.IsNullOrEmpty(Master.currShow.TVEp.Episode.ToString), "?", Master.currShow.TVEp.Episode.ToString))



            Me.alActors = New List(Of String)

            If Master.currShow.TVEp.Actors.Count > 0 Then
                Me.pbActors.Image = My.Resources.actor_silhouette
                For Each imdbAct As MediaContainers.Person In Master.currShow.TVEp.Actors
                    If Not String.IsNullOrEmpty(imdbAct.Thumb) Then
                        If Not imdbAct.Thumb.ToLower.IndexOf("addtiny.gif") > 0 AndAlso Not imdbAct.Thumb.ToLower.IndexOf("no_photo") > 0 Then
                            Me.alActors.Add(imdbAct.Thumb)
                        Else
                            Me.alActors.Add("none")
                        End If
                    Else
                        Me.alActors.Add("none")
                    End If

                    If String.IsNullOrEmpty(imdbAct.Role.Trim) Then
                        Me.lstActors.Items.Add(imdbAct.Name.Trim)
                    Else
                        Me.lstActors.Items.Add(String.Format("{0} as {1}", imdbAct.Name.Trim, imdbAct.Role.Trim))
                    End If
                Next
                Me.lstActors.SelectedIndex = 0
            End If

            Me.pnlMPAA.Visible = False

            Dim tmpRating As Single = NumUtils.ConvertToSingle(Master.currShow.TVEp.Rating)
            If tmpRating > 0 Then
                Me.BuildStars(tmpRating)
            End If

            If Not String.IsNullOrEmpty(Master.currShow.TVShow.Studio) Then
                Me.pbStudio.Image = APIXML.GetStudioImage(Master.currShow.TVShow.Studio)
            Else
                Me.pbStudio.Image = APIXML.GetStudioImage("####")
            End If

            If Master.eSettings.ScanMediaInfo Then
                Me.SetAVImages(APIXML.GetAVImages(Master.currShow.TVEp.FileInfo, Master.currShow.Filename))
                Me.pnlInfoIcons.Width = pbVideo.Width + pbVType.Width + pbResolution.Width + pbAudio.Width + pbChannels.Width + pbStudio.Width + 6
                Me.pbStudio.Left = pbVideo.Width + pbVType.Width + pbResolution.Width + pbAudio.Width + pbChannels.Width + 5
            Else
                Me.pnlInfoIcons.Width = pbStudio.Width + 1
                Me.pbStudio.Left = 0
            End If

            Me.txtMetaData.Text = NFO.FIToString(Master.currShow.TVEp.FileInfo)

            If Not IsNothing(Me.MainPoster.Image) Then
                Me.pbPosterCache.Image = Me.MainPoster.Image
                ImageUtils.ResizePB(Me.pbPoster, Me.pbPosterCache, Me.PosterMaxHeight, Me.PosterMaxWidth)
                ImageUtils.SetGlassOverlay(Me.pbPoster)
                Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)

                If Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbPoster.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainPoster.Image.Width, Me.MainPoster.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle(Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2 - 15), Me.pbPoster.Height - 25, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2), Me.pbPoster.Height - 20)
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

            If Not IsNothing(Me.MainFanart.Image) Then
                Me.pbFanartCache.Image = Me.MainFanart.Image

                ImageUtils.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)

                If Not IsNothing(pbFanart.Image) AndAlso Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbFanart.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainFanart.Image.Width, Me.MainFanart.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle((pbFanart.Image.Width - lenSize) - 40, 10, lenSize + 30, 25)
                    ImageUtils.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbFanart.Image.Width - lenSize) - 25, 15)
                End If
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

            Me.InfoCleared = False

            If Not bwScraper.IsBusy AndAlso Not bwRefreshMovies.IsBusy AndAlso Not bwCleanDB.IsBusy Then
                Me.SetControlsEnabled(True)
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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

                ToolTips.SetToolTip(.pbStar1, String.Format("Rating: {0:N}", sinRating))
                ToolTips.SetToolTip(.pbStar2, String.Format("Rating: {0:N}", sinRating))
                ToolTips.SetToolTip(.pbStar3, String.Format("Rating: {0:N}", sinRating))
                ToolTips.SetToolTip(.pbStar4, String.Format("Rating: {0:N}", sinRating))
                ToolTips.SetToolTip(.pbStar5, String.Format("Rating: {0:N}", sinRating))


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
                        Case Else
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                            .pbStar5.Image = My.Resources.star
                    End Select
                End If
            End With
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                Me.pnlGenre(i).BackColor = Me.GenrePanelColor
                Me.pbGenre(i).BackColor = Me.GenrePanelColor
                Me.pnlGenre(i).BorderStyle = BorderStyle.FixedSingle
                Me.pbGenre(i).SizeMode = PictureBoxSizeMode.StretchImage
                Me.pbGenre(i).Image = APIXML.GetGenreImage(genreArray(i).Trim)
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
                    Dim drawSize As Integer = Convert.ToInt32((14 * (bmGenre.Width / drawWidth)) - 0.5)
                    drawFont = New Font("Microsoft Sans Serif", If(drawSize > 14, 14, drawSize), FontStyle.Bold, GraphicsUnit.Pixel)
                    Dim drawHeight As Single = grGenre.MeasureString(drawString, drawFont).Height
                    iLeft = Convert.ToInt32((bmGenre.Width - grGenre.MeasureString(drawString, drawFont).Width) / 2)
                    grGenre.DrawString(drawString, drawFont, drawBrush, iLeft, (bmGenre.Height - drawHeight))
                    pbGenre(i).Image = bmGenre
                End If
            Next
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
    'Move this to Top when finished
    Friend WithEvents bwNewScraper As New System.ComponentModel.BackgroundWorker
    Private MediaListScraperIndex As Integer = -1
    Private dScrapeRow As DataRow

    Private Sub NewScrapeData(ByVal sType As Enums.ScrapeType, ByVal Options As Structures.ScrapeOptions)
        btnCancel.Visible = True
        lblCanceling.Visible = False
        pbCanceling.Visible = False
        Me.pnlCancel.Visible = True
        Me.tspbLoading.Style = ProgressBarStyle.Continuous
        Me.SetControlsEnabled(False)
        Me.tspbLoading.Value = Me.tspbLoading.Minimum
        Me.tspbLoading.Maximum = Me.dgvMediaList.SelectedRows.Count
        Select Case sType
            Case Enums.ScrapeType.FullAsk
                Me.tslLoading.Text = Master.eLang.GetString(127, "Scraping Media (All Movies - Ask):")
            Case Enums.ScrapeType.FullAuto
                Me.tslLoading.Text = Master.eLang.GetString(128, "Scraping Media (All Movies - Auto):")
            Case Enums.ScrapeType.CleanFolders
                Me.tslLoading.Text = Master.eLang.GetString(129, "Cleaning Files:")
            Case Enums.ScrapeType.CopyBD
                Me.tslLoading.Text = Master.eLang.GetString(130, "Copying Fanart to Backdrops Folder:")
            Case Enums.ScrapeType.UpdateAuto
                Me.tslLoading.Text = Master.eLang.GetString(132, "Scraping Media (Movies Missing Items - Auto):")
            Case Enums.ScrapeType.UpdateAsk
                Me.tslLoading.Text = Master.eLang.GetString(133, "Scraping Media (Movies Missing Items - Ask):")
            Case Enums.ScrapeType.FilterAsk
                Me.tslLoading.Text = Master.eLang.GetString(622, "Scraping Media (Current Filter - Ask):")
            Case Enums.ScrapeType.FilterAuto
                Me.tslLoading.Text = Master.eLang.GetString(623, "Scraping Media (Current Filter - Auto):")
        End Select
        Me.tslLoading.Visible = True
        Me.tspbLoading.Visible = True
        Application.DoEvents()
        Functions.SetScraperMod(Enums.ModType.All, True)
        bwNewScraper.WorkerSupportsCancellation = True
        bwNewScraper.WorkerReportsProgress = True
        bwNewScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .Options = Options})
    End Sub
    Private Sub NewScrapeDataEnd()
        Me.tslLoading.Visible = False
        Me.tspbLoading.Visible = False
        btnCancel.Visible = False
        lblCanceling.Visible = False
        pbCanceling.Visible = False
        Me.pnlCancel.Visible = False
        Me.SetControlsEnabled(True)
    End Sub

    Private Sub bwNewScraper_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwNewScraper.DoWork
        'Will Need to make a cleanup on Arguments when old scraper code is removed
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        'We gonna use MediaList selected items from now on ... no need to mark movies anymore ... but maybe we still can use Mark !?
        For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
            If bwNewScraper.CancellationPending Then Exit For
            bwNewScraper.ReportProgress(1)
            Dim indX As Integer = sRow.Index

            MediaListScraperIndex = indX
            Dim MovieId As Integer = Convert.ToInt32(sRow.Cells(0).Value)
            'Do this where so will not need do everytime that row need's updates
            dScrapeRow = DirectCast((From drvRow In dtMedia.Rows Where Convert.ToInt64(DirectCast(drvRow, DataRow).Item(0)) = MovieId Select drvRow)(0), DataRow)
            Dim DBScrapeMovie As EmberAPI.Structures.DBMovie = Master.DB.LoadMovieFromDB(MovieId)

            AddHandler ModulesManager.Instance.ScraperUpdateMediaList, AddressOf ScraperUpdateMediaList
            ModulesManager.Instance.ScrapeOnly(DBScrapeMovie, Args.Options)
            dScrapeRow.Item(6) = True

            If bwNewScraper.CancellationPending Then Exit For
            ModulesManager.Instance.PostScrapeOnly(DBScrapeMovie, Args.scrapeType)
            RemoveHandler ModulesManager.Instance.ScraperUpdateMediaList, AddressOf ScraperUpdateMediaList

            If bwNewScraper.CancellationPending Then Exit For
            If Master.eSettings.ScanMediaInfo AndAlso Not String.IsNullOrEmpty(DBScrapeMovie.Movie.IMDBID) AndAlso Master.GlobalScrapeMod.Meta Then
                UpdateMediaInfo(DBScrapeMovie)
            End If

            If Args.scrapeType = Enums.ScrapeType.FilterAsk OrElse Args.scrapeType = Enums.ScrapeType.FullAsk OrElse Args.scrapeType = Enums.ScrapeType.MarkAsk _
                OrElse Args.scrapeType = Enums.ScrapeType.NewAsk OrElse Args.scrapeType = Enums.ScrapeType.UpdateAsk Then
                'Any Non Auto open EditMovie
                Using dEditMovie As New dlgEditMovie
                    Select Case dEditMovie.ShowDialog()
                    End Select
                End Using
            End If
            If bwNewScraper.CancellationPending Then Exit For

            If Master.eSettings.AutoRenameMulti AndAlso Master.GlobalScrapeMod.NFO Then
                FileFolderRenamer.RenameSingle(DBScrapeMovie, Master.eSettings.FoldersPattern, Master.eSettings.FilesPattern, False, Not String.IsNullOrEmpty(DBScrapeMovie.Movie.IMDBID), False)
            Else
                Master.DB.SaveMovieToDB(DBScrapeMovie, False, False, Not String.IsNullOrEmpty(DBScrapeMovie.Movie.IMDBID))
            End If

        Next
    End Sub
    Private Sub bwNewScraper_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwNewScraper.ProgressChanged
        Me.tspbLoading.Value += e.ProgressPercentage
    End Sub
    Private Sub bwNewScraper_Completed(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwNewScraper.RunWorkerCompleted
        NewScrapeDataEnd()
    End Sub
    Private Sub ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean)
        dScrapeRow.Item(col) = v
    End Sub

    Private Sub ScrapeData(ByVal sType As Enums.ScrapeType, ByVal Options As Structures.ScrapeOptions, Optional ByVal ID As Integer = 0, Optional ByVal doSearch As Boolean = False)

        Try
            Dim chkCount As Integer = 0
            Dim nfoPath As String = String.Empty

            If Not isCL Then
                Me.SetStatus(String.Empty) 'clear status for scrapers that do not report
                Me.SetControlsEnabled(False)
                Me.tspbLoading.Style = ProgressBarStyle.Continuous
                Me.EnableFilters(False)

                If Not sType = Enums.ScrapeType.SingleScrape Then
                    Select Case sType
                        Case Enums.ScrapeType.CleanFolders
                            lblCanceling.Text = Master.eLang.GetString(119, "Canceling File Cleaner...")
                            btnCancel.Text = Master.eLang.GetString(120, "Cancel Cleaner")
                        Case Enums.ScrapeType.CopyBD
                            lblCanceling.Text = Master.eLang.GetString(121, "Canceling Backdrop Copy...")
                            btnCancel.Text = Master.eLang.GetString(122, "Cancel Copy")
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
                Case Enums.ScrapeType.FullAsk, Enums.ScrapeType.FullAuto, Enums.ScrapeType.CleanFolders, Enums.ScrapeType.CopyBD, Enums.ScrapeType.UpdateAsk, Enums.ScrapeType.UpdateAuto, Enums.ScrapeType.FilterAsk, Enums.ScrapeType.FilterAuto
                    Me.tspbLoading.Maximum = Me.dtMedia.Rows.Count
                    Select Case sType
                        Case Enums.ScrapeType.FullAsk
                            Me.tslLoading.Text = Master.eLang.GetString(127, "Scraping Media (All Movies - Ask):")
                        Case Enums.ScrapeType.FullAuto
                            Me.tslLoading.Text = Master.eLang.GetString(128, "Scraping Media (All Movies - Auto):")
                        Case Enums.ScrapeType.CleanFolders
                            Me.tslLoading.Text = Master.eLang.GetString(129, "Cleaning Files:")
                        Case Enums.ScrapeType.CopyBD
                            Me.tslLoading.Text = Master.eLang.GetString(130, "Copying Fanart to Backdrops Folder:")
                        Case Enums.ScrapeType.UpdateAuto
                            Me.tslLoading.Text = Master.eLang.GetString(132, "Scraping Media (Movies Missing Items - Auto):")
                        Case Enums.ScrapeType.UpdateAsk
                            Me.tslLoading.Text = Master.eLang.GetString(133, "Scraping Media (Movies Missing Items - Ask):")
                        Case Enums.ScrapeType.FilterAsk
                            Me.tslLoading.Text = Master.eLang.GetString(622, "Scraping Media (Current Filter - Ask):")
                        Case Enums.ScrapeType.FilterAuto
                            Me.tslLoading.Text = Master.eLang.GetString(623, "Scraping Media (Current Filter - Auto):")
                    End Select
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    bwScraper.WorkerReportsProgress = True
                    bwScraper.WorkerSupportsCancellation = True
                    bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .Options = Options})

                Case Enums.ScrapeType.NewAsk, Enums.ScrapeType.NewAuto, Enums.ScrapeType.MarkAsk, Enums.ScrapeType.MarkAuto
                    For Each drvRow As DataRow In Me.dtMedia.Rows
                        If Convert.ToBoolean(drvRow.Item(10)) AndAlso (sType = Enums.ScrapeType.NewAsk OrElse sType = Enums.ScrapeType.NewAuto) Then
                            chkCount += 1
                        End If
                        If Convert.ToBoolean(drvRow.Item(11)) AndAlso (sType = Enums.ScrapeType.MarkAsk OrElse sType = Enums.ScrapeType.MarkAuto) Then
                            chkCount += 1
                        End If
                    Next

                    If chkCount > 0 Then
                        Select Case sType
                            Case Enums.ScrapeType.NewAsk
                                Me.tslLoading.Text = Master.eLang.GetString(134, "Scraping Media (New Movies - Ask):")
                            Case Enums.ScrapeType.NewAuto
                                Me.tslLoading.Text = Master.eLang.GetString(135, "Scraping Media (New Movies - Auto):")
                            Case Enums.ScrapeType.MarkAsk
                                Me.tslLoading.Text = Master.eLang.GetString(136, "Scraping Media (Marked Movies - Ask):")
                            Case Enums.ScrapeType.MarkAuto
                                Me.tslLoading.Text = Master.eLang.GetString(137, "Scraping Media (Marked Movies - Auto):")
                        End Select
                        Me.tspbLoading.Maximum = chkCount
                        Me.tslLoading.Visible = True
                        Me.tspbLoading.Visible = True

                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .Options = Options})
                    Else
                        If isCL Then
                            Me.ScraperDone = True
                        Else
                            Me.tslLoading.Visible = False
                            Me.tspbLoading.Visible = False
                            Me.SetStatus(String.Empty)
                            Me.SetControlsEnabled(True, True)
                            Me.EnableFilters(True)
                            Me.pnlCancel.Visible = False
                        End If
                    End If
                Case Enums.ScrapeType.SingleScrape
                    Me.ClearInfo()
                    Me.SetStatus(String.Format(Master.eLang.GetString(138, "Re-scraping {0}"), Master.currMovie.Movie.Title))
                    Me.tslLoading.Text = Master.eLang.GetString(139, "Scraping:")
                    Me.tspbLoading.Maximum = 13
                    Me.ReportDownloadPercent = True
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True
                    Me.SetControlsEnabled(False, True)

                    If isCL AndAlso doSearch = False Then
                        Dim Poster As New Images
                        Dim Fanart As New Images
                        Dim fResults As New Containers.ImgResult
                        Dim pResults As New Containers.ImgResult

                        Try
                            If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) Then
                                IMDB.GetMovieInfo(Master.tmpMovie.IMDBID, Master.currMovie.Movie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False, Master.DefaultOptions)
                            Else
                                Master.currMovie.Movie = IMDB.GetSearchMovieInfo(Me.tmpTitle, Master.tmpMovie, Enums.ScrapeType.SingleScrape, Master.DefaultOptions)
                            End If

                            If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) Then
                                If Master.eSettings.ScanMediaInfo Then
                                    UpdateMediaInfo(Master.currMovie)
                                End If

                                If Poster.IsAllowedToDownload(Master.currMovie, Enums.ImageType.Posters) Then
                                    If Poster.GetPreferredImage(Master.currMovie.Movie.IMDBID, Enums.ImageType.Posters, pResults, Master.currMovie.Filename, False) Then
                                        If Not IsNothing(Poster.Image) Then
                                            Master.currMovie.PosterPath = Poster.SaveAsPoster(Master.currMovie)
                                            If Not Master.eSettings.NoSaveImagesToNfo Then Master.currMovie.Movie.Thumb = pResults.Posters
                                        End If
                                    End If
                                End If
                                pResults = Nothing

                                If Fanart.IsAllowedToDownload(Master.currMovie, Enums.ImageType.Fanart) Then
                                    If Fanart.GetPreferredImage(Master.currMovie.Movie.IMDBID, Enums.ImageType.Fanart, fResults, Master.currMovie.Filename, True) Then
                                        If Not IsNothing(Fanart.Image) Then
                                            Master.currMovie.FanartPath = Fanart.SaveAsFanart(Master.currMovie)
                                            If Not Master.eSettings.NoSaveImagesToNfo Then Master.currMovie.Movie.Fanart = fResults.Fanart
                                        End If
                                    End If
                                End If
                                fResults = Nothing

                                Master.DB.SaveMovieToDB(Master.currMovie, True, False, True)
                            End If

                            If Master.eSettings.AutoThumbs > 0 AndAlso Master.currMovie.isSingle Then
                                ThumbGenerator.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs, False)
                            End If
                        Catch
                        End Try
                        Me.ScraperDone = True
                    Else
                        Master.tmpMovie.Clear()
                        If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) AndAlso doSearch = False Then
                            Master.tmpMovie = Master.currMovie.Movie
                            IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.DefaultOptions)
                            ' Note: possible place to invoke scrape modules
                            ' ScrapeMovieWithModules(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.DefaultOptions)
                        Else
                            Using dSearch As New dlgIMDBSearchResults
                                If dSearch.ShowDialog(Me.tmpTitle) = Windows.Forms.DialogResult.OK Then
                                    If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                                        Me.ClearInfo()
                                        Me.SetStatus(String.Format(Master.eLang.GetString(138, "Re-scraping {0}"), Master.tmpMovie.Title))
                                        Me.tslLoading.Text = Master.eLang.GetString(139, "Scraping:")
                                        Me.tspbLoading.Maximum = 13
                                        Me.tspbLoading.Style = ProgressBarStyle.Continuous
                                        Me.ReportDownloadPercent = True
                                        Me.tslLoading.Visible = True
                                        Me.tspbLoading.Visible = True
                                        Master.currMovie.ClearExtras = True
                                        Master.currMovie.PosterPath = String.Empty
                                        Master.currMovie.FanartPath = String.Empty
                                        Master.currMovie.TrailerPath = String.Empty
                                        Master.currMovie.ExtraPath = String.Empty
                                        Master.currMovie.SubPath = String.Empty
                                        Master.currMovie.NfoPath = String.Empty
                                        IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.DefaultOptions)
                                        ' Note: possible place to invoke scrape modules
                                        ' ScrapeMovieWithModules(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.DefaultOptions)
                                    End If
                                Else
                                    If isCL Then
                                        Me.ScraperDone = True
                                    Else
                                        Me.tslLoading.Visible = False
                                        Me.tspbLoading.Visible = False
                                        Me.SetStatus(String.Empty)
                                        Me.SetControlsEnabled(True, True)
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
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
    Private Sub UpdateMediaInfo(ByRef miMovie As Structures.DBMovie)
        Try
            'clear it out
            miMovie.Movie.FileInfo = New MediaInfo.Fileinfo

            Dim pExt As String = Path.GetExtension(miMovie.Filename).ToLower
            If Not pExt = ".rar" AndAlso (Master.CanScanDiscImage OrElse Not (pExt = ".iso" OrElse _
               pExt = ".img" OrElse pExt = ".bin" OrElse pExt = ".cue" OrElse pExt = ".nrg")) Then
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
            If miMovie.Movie.FileInfo.StreamDetails.Video.Count = 0 AndAlso miMovie.Movie.FileInfo.StreamDetails.Audio.Count = 0 AndAlso miMovie.Movie.FileInfo.StreamDetails.Subtitle.Count = 0 Then
                Dim _mi As MediaInfo.Fileinfo
                _mi = MediaInfo.ApplyDefaults(pExt)
                If Not _mi Is Nothing Then miMovie.Movie.FileInfo = _mi
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                    Application.DoEvents()
                    Me.UpdateMediaInfo(Master.currMovie)
                End If

                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    Using dImgSelectFanart As New dlgImgSelect
                        Dim AllowFA As Boolean = tmpImages.IsAllowedToDownload(Master.currMovie, Enums.ImageType.Fanart, True)

                        If AllowFA Then dImgSelectFanart.PreLoad(Master.currMovie, Enums.ImageType.Fanart, True)

                        If tmpImages.IsAllowedToDownload(Master.currMovie, Enums.ImageType.Posters, True) Then
                            Me.tslLoading.Text = Master.eLang.GetString(572, "Scraping Posters:")
                            Application.DoEvents()
                            Using dImgSelect As New dlgImgSelect
                                Dim pResults As Containers.ImgResult = dImgSelect.ShowDialog(Master.currMovie, Enums.ImageType.Posters, True)
                                If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                    Master.currMovie.PosterPath = pResults.ImagePath
                                    If Not Master.eSettings.NoSaveImagesToNfo AndAlso pResults.Posters.Count > 0 Then Master.currMovie.Movie.Thumb = pResults.Posters
                                End If
                                pResults = Nothing
                            End Using
                        End If

                        If AllowFA Then
                            Me.tslLoading.Text = Master.eLang.GetString(573, "Scraping Fanart:")
                            Application.DoEvents()
                            Dim fResults As Containers.ImgResult = dImgSelectFanart.ShowDialog
                            If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                Master.currMovie.FanartPath = fResults.ImagePath
                                If Not Master.eSettings.NoSaveImagesToNfo AndAlso fResults.Fanart.Thumb.Count > 0 Then Master.currMovie.Movie.Fanart = fResults.Fanart
                            End If
                            fResults = Nothing
                        End If

                    End Using
                    tmpImages.Dispose()
                    tmpImages = Nothing
                End If

                If Master.eSettings.SingleScrapeTrailer Then
                    Me.tslLoading.Text = Master.eLang.GetString(574, "Scraping Trailers:")
                    Application.DoEvents()
                    Dim cTrailer As New Trailers
                    If cTrailer.IsAllowedToDownload(Master.currMovie.Filename, True, Master.currMovie.Movie.Trailer) Then
                        Using dTrailer As New dlgTrailer
                            Dim tURL As String = dTrailer.ShowDialog(Master.currMovie.Movie.IMDBID, Master.currMovie.Filename)
                            If Not String.IsNullOrEmpty(tURL) AndAlso tURL.Substring(0, 7) = "http://" Then
                                Master.currMovie.Movie.Trailer = tURL
                            End If
                        End Using
                    End If
                    cTrailer = Nothing
                End If

                If Master.eSettings.AutoThumbs > 0 AndAlso Master.currMovie.isSingle Then
                    Me.tslLoading.Text = Master.eLang.GetString(575, "Generating Extrathumbs:")
                    Application.DoEvents()
                    ThumbGenerator.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs, True)
                End If

                If Not isCL Then
                    Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                    Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
                    Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString

                    Me.tslLoading.Text = Master.eLang.GetString(576, "Verifying Movie Details:")
                    Application.DoEvents()

                    Using dEditMovie As New dlgEditMovie
                        Select Case dEditMovie.ShowDialog()
                            Case Windows.Forms.DialogResult.OK
                                If Master.eSettings.AutoRenameSingle Then
                                    FileFolderRenamer.RenameSingle(Master.currMovie, Master.eSettings.FoldersPattern, Master.eSettings.FilesPattern, False, False, True)
                                End If
                                Me.SetListItemAfterEdit(ID, indX)
                                If Me.RefreshMovie(ID) Then
                                    Me.FillList(0)
                                End If
                            Case Windows.Forms.DialogResult.Retry
                                Master.currMovie.ClearExtras = False
                                Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, ID)
                            Case Windows.Forms.DialogResult.Abort
                                Master.currMovie.ClearExtras = False
                                Me.ScrapeData(Enums.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
                            Case Else
                                If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value.ToString, True, False)
                        End Select
                    End Using

                Else
                    If Master.eSettings.AutoRenameSingle Then
                        FileFolderRenamer.RenameSingle(Master.currMovie, Master.eSettings.FoldersPattern, Master.eSettings.FilesPattern, False, True, False)
                    Else
                        Master.DB.SaveMovieToDB(Master.currMovie, True, False, True)
                    End If
                End If
            Else
                MsgBox(Master.eLang.GetString(141, "Unable to retrieve movie details from the internet. Please check your connection and try again."), MsgBoxStyle.Exclamation, Master.eLang.GetString(142, "Error Retrieving Details"))
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Master.currMovie.ClearExtras = False

        If isCL Then
            Me.ScraperDone = True
        Else
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.SetStatus(String.Empty)
            Me.SetControlsEnabled(True, True)
            Me.EnableFilters(True)
        End If
    End Sub

    Private Function RefreshMovie(ByVal ID As Long, Optional ByVal BatchMode As Boolean = False, Optional ByVal FromNfo As Boolean = True, Optional ByVal ToNfo As Boolean = False) As Boolean
        Dim dRow = From drvRow In dtMedia.Rows Where Convert.ToInt64(DirectCast(drvRow, DataRow).Item(0)) = ID Select drvRow
        Dim tmpMovie As New MediaContainers.Movie
        Dim tmpMovieDb As New Structures.DBMovie
        Dim OldTitle As String = String.Empty

        Dim myDelegate As New MydtMediaUpdate(AddressOf dtMediaUpdate)

        Try

            tmpMovieDb = Master.DB.LoadMovieFromDB(ID)

            OldTitle = tmpMovieDb.Movie.Title

            If Directory.Exists(Directory.GetParent(tmpMovieDb.Filename).FullName) Then

                If FromNfo Then
                    If String.IsNullOrEmpty(tmpMovieDb.NfoPath) Then
                        Dim sNFO As String = NFO.GetNfoPath(tmpMovieDb.Filename, tmpMovieDb.isSingle)
                        tmpMovieDb.NfoPath = sNFO
                        tmpMovie = NFO.LoadMovieFromNFO(sNFO, tmpMovieDb.isSingle)
                    Else
                        tmpMovie = NFO.LoadMovieFromNFO(tmpMovieDb.NfoPath, tmpMovieDb.isSingle)
                    End If
                    tmpMovieDb.Movie = tmpMovie
                End If

                If String.IsNullOrEmpty(tmpMovieDb.Movie.Title) Then
                    If FileUtils.Common.isVideoTS(tmpMovieDb.Filename) Then
                        tmpMovieDb.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(tmpMovieDb.Filename).FullName).Name)
                        tmpMovieDb.Movie.Title = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(tmpMovieDb.Filename).FullName).Name, False)
                    ElseIf FileUtils.Common.isBDRip(tmpMovieDb.Filename) Then
                        tmpMovieDb.ListTitle = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(Directory.GetParent(tmpMovieDb.Filename).FullName).FullName).Name)
                        tmpMovieDb.Movie.Title = StringUtils.FilterName(Directory.GetParent(Directory.GetParent(Directory.GetParent(tmpMovieDb.Filename).FullName).FullName).Name, False)
                    Else
                        If tmpMovieDb.UseFolder AndAlso tmpMovieDb.isSingle Then
                            tmpMovieDb.ListTitle = StringUtils.FilterName(Directory.GetParent(tmpMovieDb.Filename).Name)
                            tmpMovieDb.Movie.Title = StringUtils.FilterName(Directory.GetParent(tmpMovieDb.Filename).Name, False)
                        Else
                            tmpMovieDb.ListTitle = StringUtils.FilterName(Path.GetFileNameWithoutExtension(tmpMovieDb.Filename))
                            tmpMovieDb.Movie.Title = StringUtils.FilterName(Path.GetFileNameWithoutExtension(tmpMovieDb.Filename), False)
                        End If
                    End If
                    If Not OldTitle = tmpMovieDb.Movie.Title OrElse String.IsNullOrEmpty(tmpMovieDb.Movie.SortTitle) Then tmpMovieDb.Movie.SortTitle = tmpMovieDb.ListTitle
                Else
                    Dim tTitle As String = StringUtils.FilterTokens(tmpMovieDb.Movie.Title)
                    If Not OldTitle = tmpMovieDb.Movie.Title OrElse String.IsNullOrEmpty(tmpMovieDb.Movie.SortTitle) Then tmpMovieDb.Movie.SortTitle = tTitle
                    If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(tmpMovieDb.Movie.Year) Then
                        tmpMovieDb.ListTitle = String.Format("{0} ({1})", tTitle, tmpMovieDb.Movie.Year)
                    Else
                        tmpMovieDb.ListTitle = tTitle
                    End If
                End If

                Me.Invoke(myDelegate, New Object() {dRow(0), 3, tmpMovieDb.ListTitle})
                Me.Invoke(myDelegate, New Object() {dRow(0), 15, tmpMovieDb.Movie.Title})
                Me.Invoke(myDelegate, New Object() {dRow(0), 50, tmpMovieDb.Movie.SortTitle})

                'update genre
                Me.Invoke(myDelegate, New Object() {dRow(0), 26, tmpMovieDb.Movie.Genre})

                tmpMovieDb.FileSource = APIXML.GetFileSource(tmpMovieDb.Filename)
                Dim mContainer As New Scanner.MovieContainer With {.Filename = tmpMovieDb.Filename, .isSingle = tmpMovieDb.isSingle}
                fScanner.GetMovieFolderContents(mContainer)
                tmpMovieDb.PosterPath = mContainer.Poster
                Me.Invoke(myDelegate, New Object() {dRow(0), 4, If(String.IsNullOrEmpty(mContainer.Poster), False, True)})
                tmpMovieDb.FanartPath = mContainer.Fanart
                Me.Invoke(myDelegate, New Object() {dRow(0), 5, If(String.IsNullOrEmpty(mContainer.Fanart), False, True)})
                'assume invalid nfo if no title
                tmpMovieDb.NfoPath = If(String.IsNullOrEmpty(tmpMovieDb.Movie.Title), String.Empty, mContainer.Nfo)
                Me.Invoke(myDelegate, New Object() {dRow(0), 6, If(String.IsNullOrEmpty(tmpMovieDb.NfoPath), False, True)})
                tmpMovieDb.TrailerPath = mContainer.Trailer
                Me.Invoke(myDelegate, New Object() {dRow(0), 7, If(String.IsNullOrEmpty(mContainer.Trailer), False, True)})
                tmpMovieDb.SubPath = mContainer.Subs
                Me.Invoke(myDelegate, New Object() {dRow(0), 8, If(String.IsNullOrEmpty(mContainer.Subs), False, True)})
                tmpMovieDb.ExtraPath = mContainer.Extra
                Me.Invoke(myDelegate, New Object() {dRow(0), 9, If(String.IsNullOrEmpty(mContainer.Extra), False, True)})

                Me.Invoke(myDelegate, New Object() {dRow(0), 1, tmpMovieDb.Filename})
                tmpMovieDb.IsMark = Convert.ToBoolean(DirectCast(dRow(0), DataRow).Item(11))
                tmpMovieDb.IsLock = Convert.ToBoolean(DirectCast(dRow(0), DataRow).Item(14))

                Master.DB.SaveMovieToDB(tmpMovieDb, False, BatchMode, ToNfo)

            Else
                Master.DB.DeleteFromDB(ID, BatchMode)
                Return True
            End If

            If Not BatchMode Then
                Me.DoTitleCheck()
                Me.LoadInfo(Convert.ToInt32(ID), tmpMovieDb.Filename, True, False)
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return False
    End Function

    Private Function RefreshShow(ByVal ID As Long, ByVal BatchMode As Boolean, ByVal FromNfo As Boolean, ByVal ToNfo As Boolean, ByVal WithEpisodes As Boolean) As Boolean
        Dim dRow = From drvRow In dtShows.Rows Where Convert.ToInt64(DirectCast(drvRow, DataRow).Item(0)) = ID Select drvRow
        Dim tmpShowDb As New Structures.DBTV
        Dim tmpShow As New MediaContainers.TVShow

        Dim myDelegate As New MydtShowsUpdate(AddressOf dtShowsUpdate)

        Try
            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing
            If Not BatchMode Then SQLtransaction = Master.DB.BeginTransaction

            tmpShowDb = Master.DB.LoadTVShowFromDB(ID)

            If Directory.Exists(tmpShowDb.ShowPath) Then

                If FromNfo Then
                    If String.IsNullOrEmpty(tmpShowDb.ShowNfoPath) Then
                        Dim sNFO As String = NFO.GetShowNfoPath(tmpShowDb.ShowPath)
                        tmpShowDb.ShowNfoPath = sNFO
                        tmpShow = NFO.LoadTVShowFromNFO(sNFO)
                    Else
                        tmpShow = NFO.LoadTVShowFromNFO(tmpShowDb.ShowNfoPath)
                    End If
                    tmpShowDb.TVShow = tmpShow
                End If

                If String.IsNullOrEmpty(tmpShowDb.TVShow.Title) Then
                    tmpShowDb.TVShow.Title = StringUtils.FilterTVShowName(Path.GetFileNameWithoutExtension(tmpShowDb.ShowPath), False)
                End If

                Me.Invoke(myDelegate, New Object() {dRow(0), 1, tmpShowDb.TVShow.Title})

                Dim sContainer As New Scanner.TVShowContainer With {.ShowPath = tmpShowDb.ShowPath}
                fScanner.GetShowFolderContents(sContainer)
                tmpShowDb.ShowPosterPath = sContainer.Poster
                Me.Invoke(myDelegate, New Object() {dRow(0), 2, If(String.IsNullOrEmpty(sContainer.Poster), False, True)})
                tmpShowDb.ShowFanartPath = sContainer.Fanart
                Me.Invoke(myDelegate, New Object() {dRow(0), 3, If(String.IsNullOrEmpty(sContainer.Fanart), False, True)})
                'assume invalid nfo if no title
                tmpShowDb.ShowNfoPath = If(String.IsNullOrEmpty(tmpShowDb.TVShow.Title), String.Empty, sContainer.Nfo)
                Me.Invoke(myDelegate, New Object() {dRow(0), 4, If(String.IsNullOrEmpty(tmpShowDb.ShowNfoPath), False, True)})

                tmpShowDb.IsMarkShow = Convert.ToBoolean(DirectCast(dRow(0), DataRow).Item(6))
                tmpShowDb.IsLockShow = Convert.ToBoolean(DirectCast(dRow(0), DataRow).Item(10))

                Master.DB.SaveTVShowToDB(tmpShowDb, False, WithEpisodes, ToNfo)

                If WithEpisodes Then
                    Using SQLCommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                        SQLCommand.CommandText = String.Concat("SELECT ID FROM TVEps WHERE TVShowID = ", ID, ";")
                        Using SQLReader As SQLite.SQLiteDataReader = SQLCommand.ExecuteReader
                            While SQLReader.Read
                                Me.RefreshEpisode(Convert.ToInt64(SQLReader("ID")), True)
                            End While
                        End Using
                    End Using
                End If

            Else
                Master.DB.DeleteTVShowFromDB(ID, WithEpisodes)
                Return True
            End If

            If Not BatchMode Then
                SQLtransaction.Commit()
                SQLtransaction = Nothing

                Me.LoadShowInfo(Convert.ToInt32(ID))
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return False
    End Function

    Private Function RefreshEpisode(ByVal ID As Long, Optional ByVal BatchMode As Boolean = False, Optional ByVal FromNfo As Boolean = True, Optional ByVal ToNfo As Boolean = False) As Boolean
        Dim dRow = From drvRow In dtEpisodes.Rows Where Convert.ToInt64(DirectCast(drvRow, DataRow).Item(0)) = ID Select drvRow
        Dim tmpShowDb As New Structures.DBTV
        Dim tmpEp As New MediaContainers.EpisodeDetails

        Dim myDelegate As New MydtShowsUpdate(AddressOf dtShowsUpdate)

        Try

            tmpShowDb = Master.DB.LoadTVEpFromDB(ID, True)

            If Directory.Exists(tmpShowDb.ShowPath) Then

                If FromNfo Then
                    If String.IsNullOrEmpty(tmpShowDb.EpNfoPath) Then
                        Dim sNFO As String = NFO.GetEpNfoPath(tmpShowDb.Filename)
                        tmpShowDb.EpNfoPath = sNFO
                        tmpEp = NFO.LoadTVEpFromNFO(sNFO, tmpShowDb.TVEp.Season, tmpShowDb.TVEp.Episode)
                    Else
                        tmpEp = NFO.LoadTVEpFromNFO(tmpShowDb.EpNfoPath, tmpShowDb.TVEp.Season, tmpShowDb.TVEp.Episode)
                    End If
                    tmpShowDb.TVEp = tmpEp
                End If

                If String.IsNullOrEmpty(tmpShowDb.TVEp.Title) Then
                    tmpShowDb.TVEp.Title = StringUtils.FilterTVEpName(Path.GetFileNameWithoutExtension(tmpShowDb.Filename), tmpShowDb.TVShow.Title, False)
                End If

                If dRow.Count > 0 Then Me.Invoke(myDelegate, New Object() {dRow(0), 2, tmpShowDb.TVEp.Title})

                Dim eContainer As New Scanner.EpisodeContainer With {.Filename = tmpShowDb.Filename}
                fScanner.GetEpFolderContents(eContainer)
                tmpShowDb.EpPosterPath = eContainer.Poster
                If dRow.Count > 0 Then Me.Invoke(myDelegate, New Object() {dRow(0), 3, If(String.IsNullOrEmpty(eContainer.Poster), False, True)})
                tmpShowDb.EpFanartPath = eContainer.Fanart
                If dRow.Count > 0 Then Me.Invoke(myDelegate, New Object() {dRow(0), 4, If(String.IsNullOrEmpty(eContainer.Fanart), False, True)})
                'assume invalid nfo if no title
                tmpShowDb.EpNfoPath = If(String.IsNullOrEmpty(tmpShowDb.TVEp.Title), String.Empty, eContainer.Nfo)
                If dRow.Count > 0 Then Me.Invoke(myDelegate, New Object() {dRow(0), 5, If(String.IsNullOrEmpty(tmpShowDb.EpNfoPath), False, True)})

                If dRow.Count > 0 Then tmpShowDb.IsMarkEp = Convert.ToBoolean(DirectCast(dRow(0), DataRow).Item(6))
                If dRow.Count > 0 Then tmpShowDb.IsLockEp = Convert.ToBoolean(DirectCast(dRow(0), DataRow).Item(10))

                Master.DB.SaveTVEpToDB(tmpShowDb, False, BatchMode, ToNfo)

            Else
                Master.DB.DeleteTVEpFromDB(ID, BatchMode)
                Return True
            End If

            If Not BatchMode Then
                Me.LoadEpisodeInfo(Convert.ToInt32(ID))
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return False
    End Function

    Private Sub SetMenus(ByVal ReloadFilters As Boolean)

        Dim mnuItem As ToolStripItem

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
                    For Each xCom As Settings.XBMCCom In .XBMCComs
                        tsbUpdateXBMC.DropDownItems.Add(String.Format(Master.eLang.GetString(143, "Update {0} Only"), xCom.Name), Nothing, New System.EventHandler(AddressOf XComSubClick))
                    Next
                Else
                    Me.tsbUpdateXBMC.Enabled = False
                End If

                Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Enabled = Directory.Exists(.BDPath)

                Me.ClearAllCachesToolStripMenuItem.Enabled = .UseImgCache

                Me.mnuAllAutoExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuAllAskExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuMissAutoExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuMissAskExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuMarkAutoExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuMarkAskExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuNewAutoExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuNewAskExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuFilterAutoExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET
                Me.mnuFilterAskExtra.Enabled = .AutoThumbs > 0 OrElse .AutoET

                Me.mnuAllAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuAllAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMissAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMissAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMarkAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuMarkAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuNewAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuNewAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuFilterAutoPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB
                Me.mnuFilterAskPoster.Enabled = .UseTMDB OrElse .UseIMPA OrElse .UseMPDB

                Me.mnuAllAutoFanart.Enabled = .UseTMDB
                Me.mnuAllAskFanart.Enabled = .UseTMDB
                Me.mnuMissAutoFanart.Enabled = .UseTMDB
                Me.mnuMissAskFanart.Enabled = .UseTMDB
                Me.mnuMarkAutoFanart.Enabled = .UseTMDB
                Me.mnuMarkAskFanart.Enabled = .UseTMDB
                Me.mnuNewAutoFanart.Enabled = .UseTMDB
                Me.mnuNewAskFanart.Enabled = .UseTMDB
                Me.mnuFilterAutoFanart.Enabled = .UseTMDB
                Me.mnuFilterAskFanart.Enabled = .UseTMDB

                Me.mnuAllAskMI.Enabled = .ScanMediaInfo
                Me.mnuAllAutoMI.Enabled = .ScanMediaInfo
                Me.mnuNewAskMI.Enabled = .ScanMediaInfo
                Me.mnuNewAutoMI.Enabled = .ScanMediaInfo
                Me.mnuMarkAskMI.Enabled = .ScanMediaInfo
                Me.mnuMarkAutoMI.Enabled = .ScanMediaInfo
                Me.mnuFilterAskMI.Enabled = .ScanMediaInfo
                Me.mnuFilterAutoMI.Enabled = .ScanMediaInfo

                Me.mnuAllAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuAllAskTrailer.Enabled = .DownloadTrailers
                Me.mnuMissAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuMissAskTrailer.Enabled = .DownloadTrailers
                Me.mnuNewAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuNewAskTrailer.Enabled = .DownloadTrailers
                Me.mnuMarkAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuMarkAskTrailer.Enabled = .DownloadTrailers
                Me.mnuFilterAutoTrailer.Enabled = .DownloadTrailers
                Me.mnuFilterAskTrailer.Enabled = .DownloadTrailers

                Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLNewcommand.CommandText = String.Concat("SELECT COUNT(id) AS mcount FROM movies WHERE mark = 1;")
                    Using SQLcount As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                        If Convert.ToInt32(SQLcount("mcount")) > 0 Then
                            Me.btnMarkAll.Text = Master.eLang.GetString(105, "Unmark All")
                        Else
                            Me.btnMarkAll.Text = Master.eLang.GetString(35, "Mark All")
                        End If
                    End Using
                End Using

                Me.mnuMoviesUpdate.DropDownItems.Clear()
                mnuItem = Me.mnuMoviesUpdate.DropDownItems.Add(Master.eLang.GetString(649, "Update All"), Nothing, New System.EventHandler(AddressOf SourceSubClick))
                mnuItem.Tag = String.Empty
                Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLNewcommand.CommandText = String.Concat("SELECT Name FROM Sources;")
                    Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                        While SQLReader.Read
                            mnuItem = Me.mnuMoviesUpdate.DropDownItems.Add(String.Format(Master.eLang.GetString(143, "Update {0} Only"), SQLReader("Name")), Nothing, New System.EventHandler(AddressOf SourceSubClick))
                            mnuItem.Tag = SQLReader("Name").ToString
                        End While
                    End Using
                End Using

                Me.mnuTVShowUpdate.DropDownItems.Clear()
                mnuItem = Me.mnuTVShowUpdate.DropDownItems.Add(Master.eLang.GetString(649, "Update All"), Nothing, New System.EventHandler(AddressOf TVSourceSubClick))
                mnuItem.Tag = String.Empty
                Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLNewcommand.CommandText = String.Concat("SELECT Name FROM TVSources;")
                    Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                        While SQLReader.Read
                            mnuItem = Me.mnuTVShowUpdate.DropDownItems.Add(String.Format(Master.eLang.GetString(143, "Update {0} Only"), SQLReader("Name")), Nothing, New System.EventHandler(AddressOf TVSourceSubClick))
                            mnuItem.Tag = SQLReader("Name").ToString
                        End While
                    End Using
                End Using

                GenreListToolStripComboBox.Items.Clear()
                Me.clbFilterGenres.Items.Clear()
                Dim lGenre() As Object = APIXML.GetGenreList
                GenreListToolStripComboBox.Items.AddRange(lGenre)
                clbFilterGenres.Items.AddRange(lGenre)

                'not technically a menu, but it's a good place to put it
                If ReloadFilters Then

                    clbFilterSource.Items.Clear()
                    Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                        SQLNewcommand.CommandText = String.Concat("SELECT Name FROM Sources;")
                        Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                            While SQLReader.Read
                                clbFilterSource.Items.Add(SQLReader("Name"))
                            End While
                        End Using
                    End Using

                    RemoveHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged
                    Me.cbFilterYear.Items.Clear()
                    cbFilterYear.Items.Add(Master.eLang.All)
                    For i As Integer = (Year(Today) + 1) To 1888 Step -1
                        Me.cbFilterYear.Items.Add(i)
                    Next
                    cbFilterYear.SelectedIndex = 0
                    AddHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged

                    RemoveHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged
                    cbFilterYearMod.SelectedIndex = 0
                    AddHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged

                    RemoveHandler cbFilterFileSource.SelectedIndexChanged, AddressOf cbFilterFileSource_SelectedIndexChanged
                    cbFilterFileSource.Items.Clear()
                    cbFilterFileSource.Items.Add(Master.eLang.All)
                    cbFilterFileSource.Items.AddRange(APIXML.GetSourceList)
                    cbFilterFileSource.SelectedIndex = 0
                    AddHandler cbFilterFileSource.SelectedIndexChanged, AddressOf cbFilterFileSource_SelectedIndexChanged

                End If
            End With

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub DoTitleCheck()

        Try

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLcommand.CommandText = "UPDATE movies SET OutOfTolerance = (?) WHERE ID = (?);"
                    Dim parOutOfTolerance As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parOutOfTolerance", DbType.Boolean, 0, "OutOfTolerance")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "ID")
                    Dim LevFail As Boolean = False
                    Dim pTitle As String = String.Empty
                    For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows

                        If Master.eSettings.LevTolerance > 0 Then
                            If FileUtils.Common.isVideoTS(drvRow.Cells(1).Value.ToString) Then
                                pTitle = Directory.GetParent(Directory.GetParent(drvRow.Cells(1).Value.ToString).FullName).Name
                            ElseIf FileUtils.Common.isBDRip(drvRow.Cells(1).Value.ToString) Then
                                pTitle = Directory.GetParent(Directory.GetParent(Directory.GetParent(drvRow.Cells(1).Value.ToString).FullName).FullName).Name
                            Else
                                If Convert.ToBoolean(drvRow.Cells(46).Value) AndAlso Convert.ToBoolean(drvRow.Cells(2).Value) Then
                                    pTitle = Directory.GetParent(drvRow.Cells(1).Value.ToString).Name
                                Else
                                    pTitle = Path.GetFileNameWithoutExtension(drvRow.Cells(1).Value.ToString)
                                End If
                            End If

                            LevFail = StringUtils.ComputeLevenshtein(StringUtils.FilterName(drvRow.Cells(15).Value.ToString, False, True).ToLower, StringUtils.FilterName(pTitle, False, True).ToLower) > Master.eSettings.LevTolerance

                            parOutOfTolerance.Value = LevFail
                            drvRow.Cells(47).Value = LevFail
                            parID.Value = drvRow.Cells(0).Value
                        Else
                            parOutOfTolerance.Value = False
                            drvRow.Cells(47).Value = False
                            parID.Value = drvRow.Cells(0).Value
                        End If
                        SQLcommand.ExecuteNonQuery()
                    Next
                End Using

                SQLtransaction.Commit()
            End Using

            Me.dgvMediaList.Invalidate()
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
        Me.txtFilterSource.Enabled = isEnabled
        Me.cbFilterFileSource.Enabled = isEnabled
        Me.txtFilterGenre.Enabled = isEnabled
        Me.cbFilterYearMod.Enabled = isEnabled
        Me.cbFilterYear.Enabled = isEnabled
        Me.btnClearFilters.Enabled = isEnabled
    End Sub

    Private Sub ClearFilters(Optional ByVal Reload As Boolean = False)
        Try
            Me.bsMedia.RemoveFilter()
            Me.FilterArray.Clear()
            Me.filSearch = String.Empty
            Me.filGenre = String.Empty
            Me.filYear = String.Empty
            Me.filSource = String.Empty

            RemoveHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
            Me.txtSearch.Text = String.Empty
            AddHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
            If Me.cbSearch.Items.Count > 0 Then
                Me.cbSearch.SelectedIndex = 0
            End If
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
            Me.txtFilterSource.Text = String.Empty
            For i As Integer = 0 To Me.clbFilterSource.Items.Count - 1
                Me.clbFilterSource.SetItemChecked(i, False)
            Next

            RemoveHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged
            If Me.cbFilterYear.Items.Count > 0 Then
                Me.cbFilterYear.SelectedIndex = 0
            End If
            AddHandler cbFilterYear.SelectedIndexChanged, AddressOf cbFilterYear_SelectedIndexChanged

            RemoveHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged
            If Me.cbFilterYearMod.Items.Count > 0 Then
                Me.cbFilterYearMod.SelectedIndex = 0
            End If
            AddHandler cbFilterYearMod.SelectedIndexChanged, AddressOf cbFilterYearMod_SelectedIndexChanged

            RemoveHandler cbFilterFileSource.SelectedIndexChanged, AddressOf cbFilterFileSource_SelectedIndexChanged
            If Me.cbFilterFileSource.Items.Count > 0 Then
                Me.cbFilterFileSource.SelectedIndex = 0
            End If
            AddHandler cbFilterFileSource.SelectedIndexChanged, AddressOf cbFilterFileSource_SelectedIndexChanged

            If Reload Then Me.FillList(0)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RunFilter(Optional ByVal doFill As Boolean = False)

        Try
            If Me.Visible Then

                Me.ClearInfo()

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

                If doFill Then Me.FillList(0)
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SourceSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim SourceName As String = DirectCast(sender, ToolStripItem).Tag.ToString
        Me.LoadMedia(New Structures.Scans With {.Movies = True}, SourceName)
    End Sub

    Private Sub TVSourceSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim SourceName As String = DirectCast(sender, ToolStripItem).Tag.ToString
        Me.LoadMedia(New Structures.Scans With {.TV = True}, SourceName)
    End Sub

    Private Sub XComSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim xComName As String = sender.ToString.Replace(Master.eLang.GetString(144, "Update"), String.Empty).Replace(Master.eLang.GetString(145, "Only"), String.Empty).Trim
        Dim xCom = From x As Settings.XBMCCom In Master.eSettings.XBMCComs Where x.Name = xComName
        If xCom.Count > 0 Then
            DoXCom(xCom(0))
        End If
    End Sub

    Private Sub DoXCom(ByVal xCom As Settings.XBMCCom)
        Try
            Dim Wr As HttpWebRequest = DirectCast(HttpWebRequest.Create(String.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?command=ExecBuiltIn&parameter=XBMC.updatelibrary(video)", xCom.IP, xCom.Port)), HttpWebRequest)
            Wr.Timeout = 2500

            If Not String.IsNullOrEmpty(xCom.Username) AndAlso Not String.IsNullOrEmpty(xCom.Password) Then
                Wr.Credentials = New NetworkCredential(xCom.Username, xCom.Password)
            End If

            Using Wres As HttpWebResponse = DirectCast(Wr.GetResponse, HttpWebResponse)
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

    Private Sub FillList(ByVal iIndex As Integer)
        Try
            Me.bsMedia.DataSource = Nothing
            Me.dgvMediaList.DataSource = Nothing
            Me.bsShows.DataSource = Nothing
            Me.dgvTVShows.DataSource = Nothing
            Me.bsSeasons.DataSource = Nothing
            Me.dgvTVSeasons.DataSource = Nothing
            Me.bsEpisodes.DataSource = Nothing
            Me.dgvTVEpisodes.DataSource = Nothing

            Me.ClearInfo()
            Application.DoEvents()

            If Not String.IsNullOrEmpty(Me.filSearch) AndAlso Me.cbSearch.Text = Master.eLang.GetString(100, "Actor") Then
                Master.DB.FillDataTable(Me.dtMedia, String.Concat("SELECT * FROM movies WHERE ID IN (SELECT MovieID FROM MoviesActors WHERE ActorName LIKE '%", Me.filSearch, "%') ORDER BY ListTitle COLLATE NOCASE;"))
            Else
                If Me.chkFilterDupe.Checked Then
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

                        .dgvMediaList.Columns(0).Visible = False
                        .dgvMediaList.Columns(1).Visible = False
                        .dgvMediaList.Columns(2).Visible = False
                        .dgvMediaList.Columns(3).Resizable = DataGridViewTriState.True
                        .dgvMediaList.Columns(3).ReadOnly = True
                        .dgvMediaList.Columns(3).MinimumWidth = 83
                        .dgvMediaList.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(3).ToolTipText = Master.eLang.GetString(21, "Title")
                        .dgvMediaList.Columns(3).HeaderText = Master.eLang.GetString(21, "Title")
                        .dgvMediaList.Columns(4).Width = 20
                        .dgvMediaList.Columns(4).Resizable = DataGridViewTriState.False
                        .dgvMediaList.Columns(4).ReadOnly = True
                        .dgvMediaList.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                        .dgvMediaList.Columns(4).ToolTipText = Master.eLang.GetString(148, "Poster")
                        .dgvMediaList.Columns(5).Width = 20
                        .dgvMediaList.Columns(5).Resizable = DataGridViewTriState.False
                        .dgvMediaList.Columns(5).ReadOnly = True
                        .dgvMediaList.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieFanartCol
                        .dgvMediaList.Columns(5).ToolTipText = Master.eLang.GetString(149, "Fanart")
                        .dgvMediaList.Columns(6).Width = 20
                        .dgvMediaList.Columns(6).Resizable = DataGridViewTriState.False
                        .dgvMediaList.Columns(6).ReadOnly = True
                        .dgvMediaList.Columns(6).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(6).Visible = Not Master.eSettings.MovieInfoCol
                        .dgvMediaList.Columns(6).ToolTipText = Master.eLang.GetString(150, "Nfo")
                        .dgvMediaList.Columns(7).Width = 20
                        .dgvMediaList.Columns(7).Resizable = DataGridViewTriState.False
                        .dgvMediaList.Columns(7).ReadOnly = True
                        .dgvMediaList.Columns(7).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(7).Visible = Not Master.eSettings.MovieTrailerCol
                        .dgvMediaList.Columns(7).ToolTipText = Master.eLang.GetString(151, "Trailer")
                        .dgvMediaList.Columns(8).Width = 20
                        .dgvMediaList.Columns(8).Resizable = DataGridViewTriState.False
                        .dgvMediaList.Columns(8).ReadOnly = True
                        .dgvMediaList.Columns(8).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(8).Visible = Not Master.eSettings.MovieSubCol
                        .dgvMediaList.Columns(8).ToolTipText = Master.eLang.GetString(152, "Subtitles")
                        .dgvMediaList.Columns(9).Width = 20
                        .dgvMediaList.Columns(9).Resizable = DataGridViewTriState.False
                        .dgvMediaList.Columns(9).ReadOnly = True
                        .dgvMediaList.Columns(9).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvMediaList.Columns(9).Visible = Not Master.eSettings.MovieExtraCol
                        .dgvMediaList.Columns(9).ToolTipText = Master.eLang.GetString(153, "Extrathumbs")
                        For i As Integer = 10 To .dgvMediaList.Columns.Count - 1
                            .dgvMediaList.Columns(i).Visible = False
                        Next

                        .dgvMediaList.Columns(0).ValueType = GetType(Int32)

                        If Master.isWindows Then .dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        ResizeMediaList()

                        .dgvMediaList.Sort(.dgvMediaList.Columns(3), ComponentModel.ListSortDirection.Ascending)

                        If .dgvMediaList.RowCount > 0 AndAlso Me.tabsMain.SelectedIndex = 0 Then
                            'Set current cell and automatically load the info for the first movie in the list
                            .dgvMediaList.Rows(iIndex).Cells(3).Selected = True
                            .dgvMediaList.CurrentCell = .dgvMediaList.Rows(iIndex).Cells(3)

                            Me.SetControlsEnabled(True)
                        End If

                    End With
                End If

                Me.dgvTVShows.Enabled = False
                Master.DB.FillDataTable(Me.dtShows, "SELECT * FROM TVShows ORDER BY Title COLLATE NOCASE;")

                If Me.dtShows.Rows.Count > 0 Then

                    With Me
                        .bsShows.DataSource = .dtShows
                        .dgvTVShows.DataSource = .bsShows

                        .dgvTVShows.Columns(0).Visible = False
                        .dgvTVShows.Columns(1).Resizable = DataGridViewTriState.True
                        .dgvTVShows.Columns(1).ReadOnly = True
                        .dgvTVShows.Columns(1).MinimumWidth = 83
                        .dgvTVShows.Columns(1).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvTVShows.Columns(1).ToolTipText = Master.eLang.GetString(21, "Title")
                        .dgvTVShows.Columns(1).HeaderText = Master.eLang.GetString(21, "Title")
                        .dgvTVShows.Columns(2).Width = 20
                        .dgvTVShows.Columns(2).Resizable = DataGridViewTriState.False
                        .dgvTVShows.Columns(2).ReadOnly = True
                        .dgvTVShows.Columns(2).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvTVShows.Columns(2).Visible = Not Master.eSettings.ShowPosterCol
                        .dgvTVShows.Columns(2).ToolTipText = Master.eLang.GetString(148, "Poster")
                        .dgvTVShows.Columns(3).Width = 20
                        .dgvTVShows.Columns(3).Resizable = DataGridViewTriState.False
                        .dgvTVShows.Columns(3).ReadOnly = True
                        .dgvTVShows.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvTVShows.Columns(3).Visible = Not Master.eSettings.ShowFanartCol
                        .dgvTVShows.Columns(3).ToolTipText = Master.eLang.GetString(149, "Fanart")
                        .dgvTVShows.Columns(4).Width = 20
                        .dgvTVShows.Columns(4).Resizable = DataGridViewTriState.False
                        .dgvTVShows.Columns(4).ReadOnly = True
                        .dgvTVShows.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                        .dgvTVShows.Columns(4).Visible = Not Master.eSettings.ShowNfoCol
                        .dgvTVShows.Columns(4).ToolTipText = Master.eLang.GetString(150, "Nfo")
                        For i As Integer = 5 To .dgvTVShows.Columns.Count - 1
                            .dgvTVShows.Columns(i).Visible = False
                        Next

                        .dgvTVShows.Columns(0).ValueType = GetType(Int32)

                        If Master.isWindows Then .dgvTVShows.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        ResizeTVLists(1)

                        .dgvTVShows.Sort(.dgvTVShows.Columns(1), ComponentModel.ListSortDirection.Ascending)

                        If .dgvTVShows.RowCount > 0 AndAlso Me.tabsMain.SelectedIndex = 1 Then
                            'Set current cell and automatically load the info for the first movie in the list
                            .dgvTVShows.Rows(iIndex).Cells(3).Selected = True
                            .dgvTVShows.CurrentCell = .dgvTVShows.Rows(iIndex).Cells(3)

                            Me.SetControlsEnabled(True)
                        End If

                        Me.SetTVCount()

                    End With
                End If
                Me.dgvTVShows.Enabled = True

                If Me.dtMedia.Rows.Count = 0 AndAlso Me.dtShows.Rows.Count = 0 Then
                    Me.SetControlsEnabled(False)
                    Me.SetStatus(String.Empty)
                    Me.ClearInfo()
                End If
            End If
        Catch ex As Exception
            Me.LoadingDone = True
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If Not isCL Then
            Me.tsbRefreshMedia.Enabled = True
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.tspbLoading.Value = 0
            Me.tabsMain.Enabled = True
            Me.DoTitleCheck()
            Me.EnableFilters(True)
        End If
    End Sub

    Public Sub SetListItemAfterEdit(ByVal iID As Integer, ByVal iRow As Integer)

        Try
            Dim dRow = From drvRow In dtMedia.Rows Where Convert.ToInt32(DirectCast(drvRow, DataRow).Item(0)) = iID Select drvRow

            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT mark, SortTitle FROM movies WHERE id = ", iID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    DirectCast(dRow(0), DataRow).Item(11) = Convert.ToBoolean(SQLreader("mark"))
                    If Not DBNull.Value.Equals(SQLreader("SortTitle")) Then DirectCast(dRow(0), DataRow).Item(50) = SQLreader("SortTitle").ToString
                End Using
            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SelectRow(ByVal iRow As Integer)

        Try
            Me.tmpTitle = Me.dgvMediaList.Item(15, iRow).Value.ToString
            If Not Convert.ToBoolean(Me.dgvMediaList.Item(4, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvMediaList.Item(5, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvMediaList.Item(6, iRow).Value) Then
                Me.ClearInfo()
                Me.ShowNoInfo(True, 0)
                Master.currMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(Me.dgvMediaList.Item(0, iRow).Value))

                If Not Me.fScanner.IsBusy AndAlso Not Me.bwMediaInfo.IsBusy AndAlso Not Me.bwLoadInfo.IsBusy AndAlso Not Me.bwLoadShowInfo.IsBusy AndAlso Not Me.bwLoadSeasonInfo.IsBusy AndAlso Not Me.bwLoadEpInfo.IsBusy AndAlso Not Me.bwRefreshMovies.IsBusy AndAlso Not Me.bwCleanDB.IsBusy Then
                    Me.mnuMediaList.Enabled = True
                End If
            Else
                Me.LoadInfo(Convert.ToInt32(Me.dgvMediaList.Item(0, iRow).Value), Me.dgvMediaList.Item(1, iRow).Value.ToString, True, False)
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SelectShowRow(ByVal iRow As Integer)

        Try
            Me.tmpTitle = Me.dgvTVShows.Item(1, iRow).Value.ToString
            Me.tmpTVDB = Me.dgvTVShows.Item(9, iRow).Value.ToString
            If Not Convert.ToBoolean(Me.dgvTVShows.Item(2, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvTVShows.Item(3, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvTVShows.Item(4, iRow).Value) Then
                Me.ClearInfo()
                Me.ShowNoInfo(True, 1)
                Master.currShow = Master.DB.LoadTVShowFromDB(Convert.ToInt64(Me.dgvTVShows.Item(0, iRow).Value))
                Me.FillSeasons(Convert.ToInt32(Me.dgvTVShows.Item(0, iRow).Value))

                If Not Me.fScanner.IsBusy AndAlso Not Me.bwMediaInfo.IsBusy AndAlso Not Me.bwLoadInfo.IsBusy AndAlso Not Me.bwLoadShowInfo.IsBusy AndAlso Not Me.bwLoadSeasonInfo.IsBusy AndAlso Not Me.bwLoadEpInfo.IsBusy AndAlso Not Me.bwRefreshMovies.IsBusy AndAlso Not Me.bwCleanDB.IsBusy Then
                    Me.mnuShows.Enabled = True
                End If
            Else
                Me.LoadShowInfo(Convert.ToInt32(Me.dgvTVShows.Item(0, iRow).Value))
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SelectSeasonRow(ByVal iRow As Integer)

        Try
            If String.IsNullOrEmpty(Master.currShow.ShowPosterPath) AndAlso String.IsNullOrEmpty(Master.currShow.ShowFanartPath) AndAlso _
               String.IsNullOrEmpty(Master.currShow.ShowNfoPath) AndAlso Not Convert.ToBoolean(Me.dgvTVSeasons.Item(4, iRow).Value) AndAlso _
               Not Convert.ToBoolean(Me.dgvTVSeasons.Item(5, iRow).Value) Then
                Me.ClearInfo()
                If Not Me.currThemeType = Theming.ThemeType.Show Then Me.ApplyTheme(Theming.ThemeType.Show)
                Me.ShowNoInfo(True, 1)
                Me.FillEpisodes(Convert.ToInt32(Me.dgvTVSeasons.Item(0, iRow).Value), Convert.ToInt32(Me.dgvTVSeasons.Item(3, iRow).Value))

                If Not Me.fScanner.IsBusy AndAlso Not Me.bwMediaInfo.IsBusy AndAlso Not Me.bwLoadInfo.IsBusy AndAlso Not Me.bwLoadShowInfo.IsBusy AndAlso Not Me.bwLoadSeasonInfo.IsBusy AndAlso Not Me.bwLoadEpInfo.IsBusy AndAlso Not Me.bwRefreshMovies.IsBusy AndAlso Not Me.bwCleanDB.IsBusy Then
                    Me.mnuSeasons.Enabled = True
                End If
            Else
                Me.LoadSeasonInfo(Convert.ToInt32(Me.dgvTVSeasons.Item(0, iRow).Value), Convert.ToInt32(Me.dgvTVSeasons.Item(3, iRow).Value))
            End If


        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SelectEpisodeRow(ByVal iRow As Integer)

        Try
            If Not Convert.ToBoolean(Me.dgvTVEpisodes.Item(3, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvTVEpisodes.Item(4, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvTVEpisodes.Item(5, iRow).Value) Then
                Me.ClearInfo()
                Me.ShowNoInfo(True, 2)
                Master.currShow = Master.DB.LoadTVEpFromDB(Convert.ToInt32(Me.dgvTVEpisodes.Item(0, iRow).Value), True)

                If Not Me.fScanner.IsBusy AndAlso Not Me.bwMediaInfo.IsBusy AndAlso Not Me.bwLoadInfo.IsBusy AndAlso Not Me.bwLoadShowInfo.IsBusy AndAlso Not Me.bwLoadSeasonInfo.IsBusy AndAlso Not Me.bwLoadEpInfo.IsBusy AndAlso Not Me.bwRefreshMovies.IsBusy AndAlso Not Me.bwCleanDB.IsBusy Then
                    Me.mnuEpisodes.Enabled = True
                End If
            Else
                Me.LoadEpisodeInfo(Convert.ToInt32(Me.dgvTVEpisodes.SelectedRows(0).Cells(0).Value))
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub ScannerUpdated(ByVal iType As Integer, ByVal sText As String)
        Select Case iType
            Case 1
                Me.SetStatus(String.Concat("Added Episode: ", sText))
            Case Else
                Me.SetStatus(String.Concat("Added Movie: ", sText))
        End Select
    End Sub

    Private Sub ScanningCompleted()

        If isCL Then
            Me.ScraperDone = True
        Else
            Me.SetStatus(String.Empty)
            Me.FillList(0)
            Me.tspbLoading.Visible = False
            Me.tslLoading.Visible = False
        End If

    End Sub

    Private Sub FillSeasons(ByVal ShowID As Integer)
        Me.bsSeasons.DataSource = Nothing
        Me.dgvTVSeasons.DataSource = Nothing
        Me.bsEpisodes.DataSource = Nothing
        Me.dgvTVEpisodes.DataSource = Nothing

        Application.DoEvents()

        Me.dgvTVSeasons.Enabled = False

        Master.DB.FillDataTable(Me.dtSeasons, String.Concat("SELECT * FROM TVSeason WHERE TVShowID = ", ShowID, " GROUP BY Season ORDER BY Season COLLATE NOCASE;"))

        If Me.dtSeasons.Rows.Count > 0 Then

            With Me
                .bsSeasons.DataSource = .dtSeasons
                .dgvTVSeasons.DataSource = .bsSeasons

                .dgvTVSeasons.Columns(0).Visible = False
                .dgvTVSeasons.Columns(1).Visible = False
                .dgvTVSeasons.Columns(2).Resizable = DataGridViewTriState.True
                .dgvTVSeasons.Columns(2).ReadOnly = True
                .dgvTVSeasons.Columns(2).MinimumWidth = 83
                .dgvTVSeasons.Columns(2).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVSeasons.Columns(2).ToolTipText = Master.eLang.GetString(650, "Season")
                .dgvTVSeasons.Columns(2).HeaderText = Master.eLang.GetString(650, "Season")
                .dgvTVSeasons.Columns(3).Visible = False
                .dgvTVSeasons.Columns(4).Width = 20
                .dgvTVSeasons.Columns(4).Resizable = DataGridViewTriState.False
                .dgvTVSeasons.Columns(4).ReadOnly = True
                .dgvTVSeasons.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVSeasons.Columns(4).Visible = Not Master.eSettings.SeasonPosterCol
                .dgvTVSeasons.Columns(4).ToolTipText = Master.eLang.GetString(148, "Poster")
                .dgvTVSeasons.Columns(5).Width = 20
                .dgvTVSeasons.Columns(5).Resizable = DataGridViewTriState.False
                .dgvTVSeasons.Columns(5).ReadOnly = True
                .dgvTVSeasons.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVSeasons.Columns(5).Visible = Not Master.eSettings.SeasonFanartCol
                .dgvTVSeasons.Columns(5).ToolTipText = Master.eLang.GetString(149, "Fanart")
                For i As Integer = 6 To .dgvTVSeasons.Columns.Count - 1
                    .dgvTVSeasons.Columns(i).Visible = False
                Next

                .dgvTVSeasons.Columns(0).ValueType = GetType(Int32)

                If Master.isWindows Then .dgvTVSeasons.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                ResizeTVLists(2)

                .dgvTVSeasons.Sort(.dgvTVSeasons.Columns(2), ComponentModel.ListSortDirection.Ascending)

            End With
        End If

        Me.dgvTVSeasons.SelectedRows(0).Selected = False
        Me.dgvTVSeasons.Enabled = True
    End Sub

    Private Sub FillEpisodes(ByVal ShowID As Integer, ByVal Season As Integer)
        Me.bsEpisodes.DataSource = Nothing
        Me.dgvTVEpisodes.DataSource = Nothing

        Application.DoEvents()

        Me.dgvTVEpisodes.Enabled = False

        Master.DB.FillDataTable(Me.dtEpisodes, String.Concat("SELECT * FROM TVEps WHERE TVShowID = ", ShowID, " AND Season = ", Season, " ORDER BY Episode;"))

        If Me.dtEpisodes.Rows.Count > 0 Then

            With Me
                .bsEpisodes.DataSource = .dtEpisodes
                .dgvTVEpisodes.DataSource = .bsEpisodes

                .dgvTVEpisodes.Columns(0).Visible = False
                .dgvTVEpisodes.Columns(1).Visible = False
                .dgvTVEpisodes.Columns(2).Resizable = DataGridViewTriState.True
                .dgvTVEpisodes.Columns(2).ReadOnly = True
                .dgvTVEpisodes.Columns(2).MinimumWidth = 83
                .dgvTVEpisodes.Columns(2).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVEpisodes.Columns(2).ToolTipText = Master.eLang.GetString(21, "Title")
                .dgvTVEpisodes.Columns(2).HeaderText = Master.eLang.GetString(21, "Title")
                .dgvTVEpisodes.Columns(3).Width = 20
                .dgvTVEpisodes.Columns(3).Resizable = DataGridViewTriState.False
                .dgvTVEpisodes.Columns(3).ReadOnly = True
                .dgvTVEpisodes.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVEpisodes.Columns(3).Visible = Not Master.eSettings.EpisodePosterCol
                .dgvTVEpisodes.Columns(3).ToolTipText = Master.eLang.GetString(148, "Poster")
                .dgvTVEpisodes.Columns(4).Width = 20
                .dgvTVEpisodes.Columns(4).Resizable = DataGridViewTriState.False
                .dgvTVEpisodes.Columns(4).ReadOnly = True
                .dgvTVEpisodes.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVEpisodes.Columns(4).Visible = Not Master.eSettings.EpisodeFanartCol
                .dgvTVEpisodes.Columns(4).ToolTipText = Master.eLang.GetString(149, "Fanart")
                .dgvTVEpisodes.Columns(5).Width = 20
                .dgvTVEpisodes.Columns(5).Resizable = DataGridViewTriState.False
                .dgvTVEpisodes.Columns(5).ReadOnly = True
                .dgvTVEpisodes.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVEpisodes.Columns(5).Visible = Not Master.eSettings.EpisodeNfoCol
                .dgvTVEpisodes.Columns(5).ToolTipText = Master.eLang.GetString(150, "Nfo")
                For i As Integer = 6 To .dgvTVEpisodes.Columns.Count - 1
                    .dgvTVEpisodes.Columns(i).Visible = False
                Next

                .dgvTVEpisodes.Columns(0).ValueType = GetType(Int32)
                .dgvTVEpisodes.Columns(11).ValueType = GetType(Int32)

                If Master.isWindows Then .dgvTVEpisodes.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                ResizeTVLists(3)

                .dgvTVEpisodes.Sort(.dgvTVEpisodes.Columns(11), ComponentModel.ListSortDirection.Ascending)

                .dgvTVEpisodes.SelectedRows(0).Selected = False

            End With
        End If

        Me.dgvTVEpisodes.Enabled = True
    End Sub

    Private Sub DonateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DonateToolStripMenuItem.Click
        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=11135532")
    End Sub

    Private Sub ShowNoInfo(ByVal ShowIt As Boolean, Optional ByVal tType As Integer = 0)
        If ShowIt Then
            Select Case tType
                Case 0
                    Me.Label1.Text = Master.eLang.GetString(55, "No Information is Available for This Movie")
                    If Not Me.currThemeType = Theming.ThemeType.Movies Then Me.ApplyTheme(Theming.ThemeType.Movies)
                Case 1
                    Me.Label1.Text = Master.eLang.GetString(651, "No Information is Available for This Show")
                    If Not Me.currThemeType = Theming.ThemeType.Show Then Me.ApplyTheme(Theming.ThemeType.Show)
                Case 2
                    Me.Label1.Text = Master.eLang.GetString(652, "No Information is Available for This Episode")
                    If Not Me.currThemeType = Theming.ThemeType.Episode Then Me.ApplyTheme(Theming.ThemeType.Episode)
            End Select
        End If

        Me.pnlNoInfo.Visible = ShowIt
    End Sub

    Private Sub SetTVCount()
        Dim ShowCount As Integer = 0
        Dim EpCount As Integer = 0

        Using SQLCommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLCommand.CommandText = "SELECT COUNT(ID) AS COUNT FROM TVShows"
            ShowCount = Convert.ToInt32(SQLCommand.ExecuteScalar)

            SQLCommand.CommandText = "SELECT COUNT(ID) AS COUNT FROM TVEps"
            EpCount = Convert.ToInt32(SQLCommand.ExecuteScalar)
        End Using

        If ShowCount > 0 AndAlso EpCount > 0 Then
            Me.tabTV.Text = String.Format("{0} ({1}/{2})", Master.eLang.GetString(653, "TV"), ShowCount, EpCount)
        End If
    End Sub

    Private Sub SetControlsEnabled(ByVal isEnabled As Boolean, Optional ByVal withLists As Boolean = False)
        Me.ToolsToolStripMenuItem.Enabled = isEnabled
        Me.tsbAutoPilot.Enabled = isEnabled
        Me.tsbRefreshMedia.Enabled = isEnabled
        Me.mnuMediaList.Enabled = isEnabled
        Me.mnuShows.Enabled = isEnabled
        Me.mnuSeasons.Enabled = isEnabled
        Me.mnuEpisodes.Enabled = isEnabled
        Me.txtSearch.Enabled = isEnabled
        Me.tabsMain.Enabled = isEnabled
        Me.btnMarkAll.Enabled = isEnabled

        If withLists Then
            Me.dgvMediaList.TabStop = isEnabled
            Me.dgvTVShows.TabStop = isEnabled
            Me.dgvTVSeasons.TabStop = isEnabled
            Me.dgvTVEpisodes.TabStop = isEnabled
            Me.dgvMediaList.Enabled = isEnabled
            Me.dgvTVShows.Enabled = isEnabled
            Me.dgvTVSeasons.Enabled = isEnabled
            Me.dgvTVEpisodes.Enabled = isEnabled
        End If
    End Sub

    Private Sub SetStatus(ByVal sText As String)
        Me.tslStatus.Text = sText.Replace("&", "&&")
    End Sub

    Private Sub ClearCache()
        If Directory.Exists(Master.TempPath) Then
            Dim dInfo As New DirectoryInfo(Master.TempPath)
            For Each dDir As DirectoryInfo In dInfo.GetDirectories.Where(Function(d) Not d.Name.ToLower = "shows")
                FileUtils.Delete.DeleteDirectory(dDir.FullName)
            Next

            For Each fFile As FileInfo In dInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                fFile.Delete()
            Next
        Else
            Directory.CreateDirectory(Master.TempPath)
        End If
    End Sub
#End Region '*** Routines/Functions

    Private Sub cmnuRescrapeShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRescrapeShow.Click

        Master.TVScraper.SingleScrape(Convert.ToInt32(Me.dgvTVShows.Item(0, Me.dgvTVShows.SelectedRows(0).Index).Value), Me.dgvTVShows.Item(1, Me.dgvTVShows.SelectedRows(0).Index).Value.ToString, Me.dgvTVShows.Item(9, Me.dgvTVShows.SelectedRows(0).Index).Value.ToString, Master.eSettings.TVDBMirror, Master.eSettings.TVDBLanguage, Master.eSettings.TVDBLanguages)

    End Sub

    Private Sub TVScraperEvent(ByVal eType As TVDB.Scraper.EventType, ByVal iProgress As Integer, ByVal Parameter As Object)
        Select Case eType
            Case TVDB.Scraper.EventType.LoadingEpisodes
                Me.tspbLoading.Style = ProgressBarStyle.Marquee
                Me.tspbLoading.MarqueeAnimationSpeed = 25
                Me.tslLoading.Text = Master.eLang.GetString(999, "Loading All Episodes:")
                Me.tspbLoading.Visible = True
                Me.tslLoading.Visible = True
            Case TVDB.Scraper.EventType.SavingStarted
                Me.tspbLoading.Style = ProgressBarStyle.Marquee
                Me.tspbLoading.MarqueeAnimationSpeed = 25
                Me.tslLoading.Text = Master.eLang.GetString(999, "Saving All Images:")
                Me.tspbLoading.Visible = True
                Me.tslLoading.Visible = True
            Case TVDB.Scraper.EventType.ScraperDone
                Me.RefreshShow(Master.currShow.ShowID, False, False, False, True)

                Me.tspbLoading.Visible = False
                Me.tslLoading.Visible = False
                Me.tslStatus.Visible = False
            Case TVDB.Scraper.EventType.Searching
                Me.tspbLoading.Style = ProgressBarStyle.Marquee
                Me.tspbLoading.MarqueeAnimationSpeed = 25
                Me.tslLoading.Text = Master.eLang.GetString(999, "Searching theTVDB:")
                Me.tspbLoading.Visible = True
                Me.tslLoading.Visible = True
            Case TVDB.Scraper.EventType.SelectImages
                Me.tspbLoading.Style = ProgressBarStyle.Marquee
                Me.tspbLoading.MarqueeAnimationSpeed = 25
                Me.tslLoading.Text = Master.eLang.GetString(999, "Select Images:")
                Me.tspbLoading.Visible = True
                Me.tslLoading.Visible = True
            Case TVDB.Scraper.EventType.StartingDownload
                Me.tspbLoading.Style = ProgressBarStyle.Marquee
                Me.tspbLoading.MarqueeAnimationSpeed = 25
                Me.tslLoading.Text = Master.eLang.GetString(999, "Downloading Show Zip:")
                Me.tspbLoading.Visible = True
                Me.tslLoading.Visible = True
            Case TVDB.Scraper.EventType.Verifying
                Me.tspbLoading.Style = ProgressBarStyle.Marquee
                Me.tspbLoading.MarqueeAnimationSpeed = 25

                Select Case iProgress
                    Case 0 ' show
                        Me.tslLoading.Text = Master.eLang.GetString(999, "Verifying TV Show:")
                        Me.tspbLoading.Visible = True
                        Me.tslLoading.Visible = True
                        Using dEditShow As New dlgEditShow
                            If dEditShow.ShowDialog() = Windows.Forms.DialogResult.OK Then
                                Master.TVScraper.SaveImages()
                            End If
                        End Using
                    Case 1 ' season
                    Case 2 ' episode
                        Me.tslLoading.Text = Master.eLang.GetString(999, "Verifying TV Episode:")
                        Me.tspbLoading.Visible = True
                        Me.tslLoading.Visible = True
                        Using dEditEp As New dlgEditEpisode
                            If dEditEp.ShowDialog = Windows.Forms.DialogResult.OK Then
                                Me.RefreshEpisode(Master.currShow.EpID)
                            End If
                        End Using
                        Me.tspbLoading.Visible = False
                        Me.tslLoading.Visible = False
                End Select

            Case TVDB.Scraper.EventType.Progress
                Select Case Parameter.ToString
                    Case "max"
                        Me.tspbLoading.Style = ProgressBarStyle.Continuous
                        Me.tspbLoading.Maximum = iProgress
                    Case "progress"
                        Me.tspbLoading.Value = iProgress
                End Select
                Me.tspbLoading.Visible = True
                Me.tslLoading.Visible = True

            Case TVDB.Scraper.EventType.Cancelled
                Me.tspbLoading.Visible = False
                Me.tslLoading.Visible = False

                Me.LoadShowInfo(Convert.ToInt32(Master.currShow.ShowID))
            Case TVDB.Scraper.EventType.ImageView
                Using dImgView As New dlgImgView
                    dImgView.ShowDialog(DirectCast(Parameter, Image))
                End Using
        End Select
    End Sub

    Private Sub ModuleSettingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModuleSettingToolStripMenuItem.Click
        ModulesManager.Instance.Setup()
    End Sub

    Private Sub SetAVImages(ByVal aImage As Image())
        Try
            Me.pbResolution.Image = aImage(0)
            Me.pbVideo.Image = aImage(1)
            Me.pbVType.Image = aImage(2)
            Me.pbAudio.Image = aImage(3)
            Me.pbChannels.Image = aImage(4)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMediaList.Resize
        ResizeMediaList()
    End Sub

    Private Sub dgvTVShows_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTVShows.Resize
        ResizeTVLists(1)
    End Sub

    Private Sub dgvTVSeason_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTVSeasons.Resize
        ResizeTVLists(2)
    End Sub

    Private Sub dgvTVEpisodes_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvTVEpisodes.Resize
        ResizeTVLists(3)
    End Sub

    Private Sub ResizeMediaList()
        If Not Master.isWindows Then
            If Me.dgvMediaList.ColumnCount > 0 Then
                Me.dgvMediaList.Columns(3).Width = Me.dgvMediaList.Width - _
                If(Master.eSettings.MoviePosterCol, 0, 20) - _
                If(Master.eSettings.MovieFanartCol, 0, 20) - _
                If(Master.eSettings.MovieInfoCol, 0, 20) - _
                If(Master.eSettings.MovieTrailerCol, 0, 20) - _
                If(Master.eSettings.MovieSubCol, 0, 20) - _
                If(Master.eSettings.MovieExtraCol, 0, 20) - _
                If(Me.dgvMediaList.DisplayRectangle.Height > Me.dgvMediaList.ClientRectangle.Height, 0, SystemInformation.VerticalScrollBarWidth)
            End If
        End If
    End Sub

    Private Sub ResizeTVLists(ByVal iType As Integer)
        '0 = all.... needed???

        If Not Master.isWindows Then
            If (iType = 0 OrElse iType = 1) AndAlso Me.dgvTVShows.ColumnCount > 0 Then
                Me.dgvTVShows.Columns(1).Width = Me.dgvTVShows.Width - _
                If(Master.eSettings.ShowPosterCol, 0, 20) - _
                If(Master.eSettings.ShowFanartCol, 0, 20) - _
                If(Master.eSettings.ShowNfoCol, 0, 20) - _
                If(Me.dgvTVShows.DisplayRectangle.Height > Me.dgvTVShows.ClientRectangle.Height, 0, SystemInformation.VerticalScrollBarWidth)
            End If

            If (iType = 0 OrElse iType = 2) AndAlso Me.dgvTVSeasons.ColumnCount > 0 Then
                Me.dgvTVSeasons.Columns(2).Width = Me.dgvTVSeasons.Width - _
                If(Master.eSettings.SeasonPosterCol, 0, 20) - _
                If(Master.eSettings.SeasonFanartCol, 0, 20) - _
                If(Me.dgvTVSeasons.DisplayRectangle.Height > Me.dgvTVSeasons.ClientRectangle.Height, 0, SystemInformation.VerticalScrollBarWidth)
            End If

            If (iType = 0 OrElse iType = 3) AndAlso Me.dgvTVEpisodes.ColumnCount > 0 Then
                Me.dgvTVEpisodes.Columns(2).Width = Me.dgvTVShows.Width - _
                If(Master.eSettings.EpisodePosterCol, 0, 20) - _
                If(Master.eSettings.EpisodeFanartCol, 0, 20) - _
                If(Master.eSettings.EpisodeNfoCol, 0, 20) - _
                If(Me.dgvTVEpisodes.DisplayRectangle.Height > Me.dgvTVEpisodes.ClientRectangle.Height, 0, SystemInformation.VerticalScrollBarWidth)
            End If

        End If
    End Sub

    Private Sub cmnuReloadShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuReloadShow.Click
        Try
            Me.dgvTVShows.Cursor = Cursors.WaitCursor
            Me.dgvTVSeasons.Cursor = Cursors.WaitCursor
            Me.dgvTVEpisodes.Cursor = Cursors.WaitCursor
            Me.SetControlsEnabled(False, True)

            Dim doFill As Boolean = False

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                    doFill = Me.RefreshShow(Convert.ToInt64(sRow.Cells(0).Value), True, True, False, True)
                Next
                SQLtransaction.Commit()
            End Using

            Me.dgvTVShows.Cursor = Cursors.Default
            Me.dgvTVSeasons.Cursor = Cursors.Default
            Me.dgvTVEpisodes.Cursor = Cursors.Default
            Me.SetControlsEnabled(True, True)

            If doFill Then FillList(0)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuMarkShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMarkShow.Click
        Try
            Dim setMark As Boolean = False
            If Me.dgvTVShows.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                    'if any one item is set as unmarked, set menu to mark
                    'else they are all marked, so set menu to unmark
                    If Not Convert.ToBoolean(sRow.Cells(6).Value) Then
                        setMark = True
                        Exit For
                    End If
                Next
            End If

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE TVShows SET mark = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                        parMark.Value = If(Me.dgvTVShows.SelectedRows.Count > 1, setMark, Not Convert.ToBoolean(sRow.Cells(6).Value))
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(6).Value = parMark.Value
                        Using SQLECommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            Dim parEMark As SQLite.SQLiteParameter = SQLECommand.Parameters.Add("parEMark", DbType.Boolean, 0, "mark")
                            Dim parEID As SQLite.SQLiteParameter = SQLECommand.Parameters.Add("parEID", DbType.Int32, 0, "id")
                            SQLECommand.CommandText = "UPDATE TVEps SET mark = (?) WHERE TVShowID = (?);"
                            parEMark.Value = parMark.Value
                            parEID.Value = parID.Value
                            SQLECommand.ExecuteNonQuery()

                            For Each eRow As DataGridViewRow In Me.dgvTVEpisodes.Rows
                                eRow.Cells(6).Value = parMark.Value
                            Next
                        End Using
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            Me.dgvTVShows.Invalidate()
            Me.dgvTVEpisodes.Invalidate()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuLockShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuLockShow.Click
        Try
            Dim setLock As Boolean = False
            If Me.dgvTVShows.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                    'if any one item is set as unlocked, set menu to lock
                    'else they are all locked so set menu to unlock
                    If Not Convert.ToBoolean(sRow.Cells(10).Value) Then
                        setLock = True
                        Exit For
                    End If
                Next
            End If

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE TVShows SET lock = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                        parLock.Value = If(Me.dgvTVShows.SelectedRows.Count > 1, setLock, Not Convert.ToBoolean(sRow.Cells(10).Value))
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(10).Value = parLock.Value
                        Using SQLECommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            Dim parELock As SQLite.SQLiteParameter = SQLECommand.Parameters.Add("parELock", DbType.Boolean, 0, "lock")
                            Dim parEID As SQLite.SQLiteParameter = SQLECommand.Parameters.Add("parEID", DbType.Int32, 0, "id")
                            SQLECommand.CommandText = "UPDATE TVEps SET lock = (?) WHERE TVShowID = (?);"
                            parELock.Value = parLock.Value
                            parEID.Value = parID.Value
                            SQLECommand.ExecuteNonQuery()

                            For Each eRow As DataGridViewRow In Me.dgvTVEpisodes.Rows
                                eRow.Cells(10).Value = parLock.Value
                            Next
                        End Using
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            Me.dgvTVShows.Invalidate()
            Me.dgvTVEpisodes.Invalidate()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuReloadEp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuReloadEp.Click
        Try
            Me.dgvTVShows.Cursor = Cursors.WaitCursor
            Me.dgvTVSeasons.Cursor = Cursors.WaitCursor
            Me.dgvTVEpisodes.Cursor = Cursors.WaitCursor
            Me.SetControlsEnabled(False, True)

            Dim doFill As Boolean = False
            Dim doBatch As Boolean = Me.dgvTVEpisodes.SelectedRows.Count > 1
            Dim iID As Integer = -1
            Dim iSeason As Integer = -1

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                    doFill = Me.RefreshEpisode(Convert.ToInt64(sRow.Cells(0).Value), doBatch)
                    iID = Convert.ToInt32(sRow.Cells(0).Value)
                    iSeason = Convert.ToInt32(sRow.Cells(11).Value)
                Next
                SQLtransaction.Commit()
            End Using

            Me.dgvTVShows.Cursor = Cursors.Default
            Me.dgvTVSeasons.Cursor = Cursors.Default
            Me.dgvTVEpisodes.Cursor = Cursors.Default
            Me.SetControlsEnabled(True, True)

            If doFill Then FillEpisodes(iID, iSeason)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuMarkEp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMarkEp.Click
        Try
            Dim setMark As Boolean = False
            If Me.dgvTVEpisodes.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                    'if any one item is set as unmarked, set menu to mark
                    'else they are all marked, so set menu to unmark
                    If Not Convert.ToBoolean(sRow.Cells(7).Value) Then
                        setMark = True
                        Exit For
                    End If
                Next
            End If

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE TVEps SET mark = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                        parMark.Value = If(Me.dgvTVEpisodes.SelectedRows.Count > 1, setMark, Not Convert.ToBoolean(sRow.Cells(7).Value))
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(7).Value = parMark.Value
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            Me.dgvTVEpisodes.Invalidate()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuLockEp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuLockEp.Click
        Try
            Dim setLock As Boolean = False
            If Me.dgvTVEpisodes.SelectedRows.Count > 1 Then
                For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                    'if any one item is set as unlocked, set menu to lock
                    'else they are all locked so set menu to unlock
                    If Not Convert.ToBoolean(sRow.Cells(10).Value) Then
                        setLock = True
                        Exit For
                    End If
                Next
            End If

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                    SQLcommand.CommandText = "UPDATE TVShows SET lock = (?) WHERE id = (?);"
                    For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                        parLock.Value = If(Me.dgvTVEpisodes.SelectedRows.Count > 1, setLock, Not Convert.ToBoolean(sRow.Cells(10).Value))
                        parID.Value = sRow.Cells(0).Value
                        SQLcommand.ExecuteNonQuery()
                        sRow.Cells(10).Value = parLock.Value
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            Me.dgvTVEpisodes.Invalidate()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cmnuRescrapeEp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRescrapeEp.Click
        Master.TVScraper.ScrapeEpisode(Convert.ToInt32(Me.dgvTVEpisodes.Item(1, Me.dgvTVEpisodes.SelectedRows(0).Index).Value), Me.tmpTitle, Me.tmpTVDB, Master.eSettings.TVDBMirror, Master.eSettings.TVDBLanguage, Master.eSettings.TVDBLanguages, Convert.ToInt32(Me.dgvTVEpisodes.Item(11, Me.dgvTVEpisodes.SelectedRows(0).Index).Value), Convert.ToInt32(Me.dgvTVEpisodes.Item(12, Me.dgvTVEpisodes.SelectedRows(0).Index).Value))
    End Sub
    Private Sub cmnuRemoveTVShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRemoveTVShow.Click
        Me.ClearInfo()

        Using SQLTrans As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                Master.DB.DeleteTVShowFromDB(Convert.ToInt32(sRow.Cells(0).Value), True)
            Next
            SQLTrans.Commit()
        End Using

        Me.FillList(0)
    End Sub

    Private Sub cmnuDeleteTVShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuDeleteTVShow.Click
        'TODO: Add method for confirmation dialog
        If MsgBox(Master.eLang.GetString(999, "Are you sure you want to delete the selected TV Show(s) and all of the accompanying episodes?"), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, Master.eLang.GetString(999, "Are you sure?")) = MsgBoxResult.Yes Then
            Me.ClearInfo()

            Using SQLTrans As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sRow As DataGridViewRow In Me.dgvTVShows.SelectedRows
                    FileUtils.Delete.DeleteDirectory(sRow.Cells(7).Value.ToString)
                    Master.DB.DeleteTVShowFromDB(Convert.ToInt32(sRow.Cells(0).Value), True)
                Next
                SQLTrans.Commit()
            End Using

            Me.FillList(0)
        End If
    End Sub

    Private Sub cmnuRemoveTVEp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRemoveTVEp.Click
        Me.ClearInfo()

        Using SQLTrans As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
            For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                Master.DB.DeleteTVEpFromDB(Convert.ToInt32(sRow.Cells(0).Value), True)
            Next
            SQLTrans.Commit()
        End Using

        Me.FillEpisodes(Convert.ToInt32(Me.dgvTVSeasons.Item(0, Me.currSeasonRow).Value), Convert.ToInt32(Me.dgvTVSeasons.Item(3, Me.currSeasonRow).Value))
    End Sub

    Private Sub cmnuDeleteTVEp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuDeleteTVEp.Click
        'TODO: Add method for confirmation dialog
        If MsgBox(Master.eLang.GetString(999, "Are you sure you want to delete the selected Episode?"), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, Master.eLang.GetString(999, "Are you sure?")) = MsgBoxResult.Yes Then
            Dim ePath As String = String.Empty

            Me.ClearInfo()

            Using SQLTrans As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLCommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    For Each sRow As DataGridViewRow In Me.dgvTVEpisodes.SelectedRows
                        SQLCommand.CommandText = String.Concat("SELECT TVEpPath FROM TVEpPaths WHERE ID = ", sRow.Cells(8).Value.ToString, ";")
                        Using SQLReader As SQLite.SQLiteDataReader = SQLCommand.ExecuteReader
                            If SQLReader.HasRows Then
                                ePath = Path.Combine(Directory.GetParent(SQLReader("TVEpPath").ToString).FullName, Path.GetFileNameWithoutExtension(SQLReader("TVEpPath").ToString))
                                File.Delete(SQLReader("TVEpPath").ToString)
                                File.Delete(String.Concat(ePath, ".nfo"))
                                File.Delete(String.Concat(ePath, ".tbn"))
                                File.Delete(String.Concat(ePath, ".jpg"))
                                File.Delete(String.Concat(ePath, "-fanart.jpg"))
                                File.Delete(String.Concat(ePath, ".fanart.jpg"))
                                Master.DB.DeleteTVEpFromDB(Convert.ToInt32(sRow.Cells(0).Value), True)
                            End If
                        End Using
                    Next
                End Using
                SQLTrans.Commit()
            End Using

            Me.FillEpisodes(Convert.ToInt32(Me.dgvTVSeasons.Item(0, Me.currSeasonRow).Value), Convert.ToInt32(Me.dgvTVSeasons.Item(3, Me.currSeasonRow).Value))
        End If
    End Sub
    Private Sub SelectAllAskToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllAskToolStripMenuItem.Click
        NewScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)
    End Sub

    Private Sub SelectAllAskMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllAskMenuToolStripMenuItem.Click
        NewScrapeData(Enums.ScrapeType.FullAsk, Master.DefaultOptions)
    End Sub
End Class

