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
    Friend WithEvents bwDownloadPic As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwScraper As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwRefreshMovies As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwCleanDB As New System.ComponentModel.BackgroundWorker

    Private bsMedia As New BindingSource
    Private bsShows As New BindingSource
    Private bsSeasons As New BindingSource
    Private bsEpisodes As New BindingSource
    Private alActors As New List(Of String)
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
    Private fScanner As New Scanner
    Private dtMedia As New DataTable
    Private dtShows As New DataTable
    Private dtSeasons As New DataTable
    Private dtEpisodes As New DataTable
    Private currRow As Integer = -1
    Private prevRow As Integer = -1
    Private currText As String = String.Empty
    Private prevText As String = String.Empty
    Private FilterArray As New List(Of String)
    Private ScraperDone As Boolean = False
    Private LoadingDone As Boolean = False
    Private GenreImage As Image
    Private InfoCleared As Boolean = False
    Private ThemeXML As New XDocument
    Private PosterMaxHeight As Integer = 160
    Private PosterMaxWidth As Integer = 160
    Private GenrePanelColor As Color = Color.Gainsboro
    Private IPUp As Integer = 500
    Private IPMid As Integer = 280
    Private isCL As Boolean = False

    'filters
    Private filSearch As String = String.Empty
    Private isActorSearch As Boolean = False
    Private filGenre As String = String.Empty
    Private filYear As String = String.Empty
    Private filMissing As String = String.Empty
    Private filSource As String = String.Empty

    Private Enum PicType As Integer
        Actor = 0
        Poster = 1
        Fanart = 2
    End Enum

    Private Structure Results
        Dim scrapeType As Master.ScrapeType
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
            If Me.bwDownloadPic.IsBusy Then Me.bwDownloadPic.CancelAsync()
            If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()
            If Me.bwRefreshMovies.IsBusy Then Me.bwRefreshMovies.CancelAsync()
            If Me.bwCleanDB.IsBusy Then Me.bwCleanDB.CancelAsync()

            lblCanceling.Text = Master.eLang.GetString(99, "Canceling All Processes...")
            btnCancel.Visible = False
            lblCanceling.Visible = True
            pbCanceling.Visible = True
            pnlCancel.Visible = True
            Me.Refresh()

            Do While Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwCleanDB.IsBusy
                Application.DoEvents()
            Loop

            If doSave Then Master.DB.SaveMovieList()

            If Not isCL Then Master.DB.Close()

            If Not Master.eSettings.PersistImgCache Then
                If Directory.Exists(Master.TempPath) Then
                    FileManip.Delete.DeleteDirectory(Master.TempPath)
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
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)
                Me.pnlNoInfo.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), Convert.ToInt32((Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2))
                Me.pnlCancel.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), 100)
                Me.pnlFilterGenre.Location = New Point(Me.gbSpecific.Left + Me.txtFilterGenre.Left, (Me.pnlFilter.Top + Me.txtFilterGenre.Top + Me.gbSpecific.Top) - Me.pnlFilterGenre.Height)
                Me.pnlFilterSource.Location = New Point(Me.gbSpecific.Left + Me.txtFilterSource.Left, (Me.pnlFilter.Top + Me.txtFilterSource.Top + Me.gbSpecific.Top) - Me.pnlFilterSource.Height)
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
                Dim dResult As Master.SettingsResult = dSettings.ShowDialog
                If Not dResult.DidCancel Then

                    Me.SetUp(False)

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
                    If dResult.NeedsRefresh OrElse dResult.NeedsUpdate Then
                        If dResult.NeedsRefresh Then
                            If Not Me.fScanner.IsBusy Then
                                Do While Me.bwLoadInfo.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwCleanDB.IsBusy
                                    Application.DoEvents()
                                Loop
                                Me.RefreshAllMovies()
                            End If
                        End If
                        If dResult.NeedsUpdate Then
                            If Not Me.fScanner.IsBusy Then
                                Do While Me.bwLoadInfo.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwCleanDB.IsBusy
                                    Application.DoEvents()
                                Loop
                                Me.LoadMedia(1)
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
        AddHandler fScanner.ProgressUpdated, AddressOf ScannerProgressUpdated
        AddHandler fScanner.ScanningCompleted, AddressOf ScanningCompleted

        Dim sPath As String = String.Concat(Master.AppPath, "Log", Path.DirectorySeparatorChar, "errlog.txt")
        If File.Exists(sPath) Then
            If File.Exists(sPath.Insert(sPath.LastIndexOf("."), "-old")) Then File.Delete(sPath.Insert(sPath.LastIndexOf("."), "-old"))
            FileManip.Common.MoveFileWithStream(sPath, sPath.Insert(sPath.LastIndexOf("."), "-old"))
            File.Delete(sPath)
        End If

        If Not Directory.Exists(Master.TempPath) Then Directory.CreateDirectory(Master.TempPath)


        If Args.Count > 1 Then
            Dim MoviePath As String = String.Empty
            Dim isSingle As Boolean = False
            Dim hasSpec As Boolean = False
            Dim clScrapeType As Master.ScrapeType = Nothing
            isCL = True
            Dim clExport As Boolean = False
            Dim clExportResizePoster As Integer = 0
            Dim clExportTemplate As String = "template"
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
                        Master.SetScraperMod(Master.ModType.All, True)
                    Case "-nfo"
                        Master.SetScraperMod(Master.ModType.NFO, True, True)
                    Case "-posters"
                        Master.SetScraperMod(Master.ModType.Poster, True, True)
                    Case "-fanart"
                        Master.SetScraperMod(Master.ModType.Fanart, True, True)
                    Case "-extra"
                        Master.SetScraperMod(Master.ModType.Extra, True, True)
                    Case "--verbose"
                        clAsk = True
                    Case Else
                        'If File.Exists(Args(2).Replace("""", String.Empty)) Then
                        'MoviePath = Args(2).Replace("""", String.Empty)
                        'End If
                End Select
            Next
            XML.CacheXMLs()
            Master.DB.Connect(False, False)
            If clExport = True Then
                dlgExportMovies.CLExport(MoviePath, clExportTemplate, clExportResizePoster)
            End If
            If Not IsNothing(clScrapeType) Then
                If Master.HasModifier AndAlso Not clScrapeType = Master.ScrapeType.SingleScrape Then
                    Try
                        LoadMedia(1)
                        Do While Not Me.LoadingDone
                            Application.DoEvents()
                        Loop
                        ScrapeData(clScrapeType, Master.DefaultOptions, Nothing, clAsk)
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
                                    If Directory.GetParent(sFile.Filename).Name.ToLower = "video_ts" Then
                                        Master.currMovie.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name)
                                    Else
                                        If sFile.UseFolder AndAlso sFile.isSingle Then
                                            Master.currMovie.ListTitle = StringManip.FilterName(Directory.GetParent(sFile.Filename).Name)
                                        Else
                                            Master.currMovie.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(sFile.Filename))
                                        End If
                                    End If
                                    If String.IsNullOrEmpty(Master.currMovie.Movie.SortTitle) Then Master.currMovie.Movie.SortTitle = Master.currMovie.ListTitle
                                Else
                                    Dim tTitle As String = StringManip.FilterTokens(Master.currMovie.Movie.Title)
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
                            Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, Nothing, clAsk)
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
                XML.CacheXMLs()

                Me.SetUp(True)
                Me.cbSearch.SelectedIndex = 0

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


                Me.aniType = Master.eSettings.InfoPanelState
                Select Case Me.aniType
                    Case 0
                        Me.pnlInfoPanel.Height = 25
                        Me.btnDown.Enabled = False
                        Me.btnMid.Enabled = True
                        Me.btnUp.Enabled = True
                    Case 1
                        Me.pnlInfoPanel.Height = IPMid
                        Me.btnMid.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnUp.Enabled = True
                    Case 2
                        Me.pnlInfoPanel.Height = IPUp
                        Me.btnUp.Enabled = False
                        Me.btnDown.Enabled = True
                        Me.btnMid.Enabled = True
                End Select

                Me.aniFilterRaise = Master.eSettings.FilterPanelState
                If Me.aniFilterRaise Then
                    Me.pnlFilter.Height = Master.Quantize(Me.gbSpecific.Height + Me.lblFilter.Height + 15, 5)
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
                        Me.LoadMedia(1)
                    Else
                        Me.FillList(0)
                        Me.Visible = True
                    End If

                End If

                Me.SetMenus(True)
                Master.GetListOfSources()

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
            If Not Me.alActors.Item(Me.lstActors.SelectedIndex).ToString = "none" Then

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
                Me.bwDownloadPic.RunWorkerAsync(New Arguments With {.pType = PicType.Actor, .pURL = Me.alActors.Item(Me.lstActors.SelectedIndex).ToString})

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

        If Me.pnlInfoPanel.Height = IPUp Then
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
                        Me.pnlInfoPanel.Height = IPMid

                    Case 2
                        Me.pnlInfoPanel.Height = IPUp

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
                If Me.pnlInfoPanel.Height = IPMid Then
                    Me.tmrAni.Stop()
                    Me.btnMid.Enabled = False
                    Me.btnDown.Enabled = True
                    Me.btnUp.Enabled = True
                End If
            ElseIf Me.aniType = 2 Then
                If Me.pnlInfoPanel.Height = IPUp Then
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

    Private Sub btnMIRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMetaDataRefresh.Click

        '//
        ' Refresh Media Info
        '\\

        Me.LoadInfo(Convert.ToInt32(Master.currMovie.ID), Master.currMovie.Filename, False, True, True)

    End Sub

    Private Sub dgvMediaList_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellClick
        If Me.dgvMediaList.SelectedRows.Count > 0 Then
            If Me.dgvMediaList.RowCount > 0 Then
                Me.tmpTitle = Me.dgvMediaList.Item(15, Me.dgvMediaList.SelectedRows(0).Index).Value.ToString
                If Me.dgvMediaList.SelectedRows.Count > 1 Then
                    Me.tslStatus.Text = String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvMediaList.SelectedRows.Count)
                ElseIf Me.dgvMediaList.SelectedRows.Count = 1 Then
                    Me.tslStatus.Text = Me.dgvMediaList.Item(1, Me.dgvMediaList.SelectedRows(0).Index).Value.ToString
                End If
            End If

            Me.currRow = e.RowIndex
            Me.tmrWait.Enabled = False
            Me.tmrLoad.Enabled = False
            Me.tmrWait.Enabled = True
        End If
    End Sub
    Private Sub dgvMediaList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellDoubleClick

        '//
        ' Show the NFO Editor
        '\\

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
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
                    Case Else
                        If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value.ToString, True, False)
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)
                Me.pnlNoInfo.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), Convert.ToInt32((Me.scMain.Panel2.Height - Me.pnlNoInfo.Height) / 2))
                Me.pnlCancel.Location = New Point(Convert.ToInt32((Me.scMain.Panel2.Width - Me.pnlNoInfo.Width) / 2), 100)
                Me.pnlFilterGenre.Location = New Point(Me.gbSpecific.Left + Me.txtFilterGenre.Left, (Me.pnlFilter.Top + Me.txtFilterGenre.Top + Me.gbSpecific.Top) - Me.pnlFilterGenre.Height)
                Me.pnlFilterSource.Location = New Point(Me.gbSpecific.Left + Me.txtFilterSource.Left, (Me.pnlFilter.Top + Me.txtFilterSource.Top + Me.gbSpecific.Top) - Me.pnlFilterSource.Height)
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

                If Me.dgvMediaList.SelectedRows.Count > 1 Then
                    Me.tslStatus.Text = String.Format(Master.eLang.GetString(627, "Selected Items: {0}"), Me.dgvMediaList.SelectedRows.Count)
                ElseIf Me.dgvMediaList.SelectedRows.Count = 1 Then
                    Me.tslStatus.Text = Me.dgvMediaList.Item(1, Me.dgvMediaList.SelectedRows(0).Index).Value.ToString
                End If

                If Me.bwLoadInfo.IsBusy Then
                    Me.bwLoadInfo.CancelAsync()
                    Do While Me.bwLoadInfo.IsBusy
                        Application.DoEvents()
                    Loop
                End If
                Me.SelectRow(Me.dgvMediaList.SelectedRows(0).Index)
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
                If Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse _
                Me.bwDownloadPic.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy _
                OrElse Me.bwCleanDB.IsBusy Then Return

                Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
                Master.currMovie = Master.DB.LoadMovieFromDB(ID)
                Me.tslStatus.Text = Master.currMovie.Filename
                Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString

                Using dEditMovie As New dlgEditMovie

                    Select Case dEditMovie.ShowDialog()
                        Case Windows.Forms.DialogResult.OK
                            Me.SetListItemAfterEdit(ID, indX)
                            If Me.RefreshMovie(ID) Then
                                Me.FillList(0)
                            End If
                        Case Windows.Forms.DialogResult.Retry
                            Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions)
                        Case Windows.Forms.DialogResult.Abort
                            Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
                        Case Else
                            If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value.ToString, True, False)
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

            Me.LoadInfo(Convert.ToInt32(Me.dgvMediaList.Item(0, Me.dgvMediaList.CurrentCell.RowIndex).Value), Me.dgvMediaList.Item(1, Me.dgvMediaList.CurrentCell.RowIndex).Value.ToString, True, False)
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub cmnuRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRescrape.Click

        '//
        ' Begin the process to scrape IMDB with the current ID
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, Convert.ToInt32(Me.dgvMediaList.SelectedRows(0).Cells(0).Value))
    End Sub

    Private Sub cmnuSearchNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuSearchNew.Click

        '//
        ' Begin the process to search IMDB for data
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, Convert.ToInt32(Me.dgvMediaList.SelectedRows(0).Cells(0).Value), True)
    End Sub

    Private Sub cmnuEditMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuEditMovie.Click

        '//
        ' Show the NFO Editor
        '\\

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
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, ID)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
                    Case Else
                        If Me.InfoCleared Then Me.LoadInfo(ID, Me.dgvMediaList.Item(1, indX).Value.ToString, True, False)
                End Select
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub mnuAllAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuAllAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMissAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuNewAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuMarkAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True)
        Me.ScrapeData(Master.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True)
        Me.ScrapeData(Master.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True)
        Me.ScrapeData(Master.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True)
        Me.ScrapeData(Master.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoTrailer.Click

        Master.SetScraperMod(Master.ModType.Trailer, True)
        Me.ScrapeData(Master.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAutoMI.Click

        Master.SetScraperMod(Master.ModType.Meta, True)
        Me.ScrapeData(Master.ScrapeType.FilterAuto, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskAll.Click

        Master.SetScraperMod(Master.ModType.All, True)
        Me.ScrapeData(Master.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskNfo.Click

        Master.SetScraperMod(Master.ModType.NFO, True)
        Me.ScrapeData(Master.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskPoster.Click

        Master.SetScraperMod(Master.ModType.Poster, True)
        Me.ScrapeData(Master.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskFanart.Click

        Master.SetScraperMod(Master.ModType.Fanart, True)
        Me.ScrapeData(Master.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskExtra.Click

        Master.SetScraperMod(Master.ModType.Extra, True)
        Me.ScrapeData(Master.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskTrailer.Click

        Master.SetScraperMod(Master.ModType.Trailer, True)
        Me.ScrapeData(Master.ScrapeType.FilterAsk, Master.DefaultOptions)

    End Sub

    Private Sub mnuFilterAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuFilterAskMI.Click

        Master.SetScraperMod(Master.ModType.Meta, True)
        Me.ScrapeData(Master.ScrapeType.FilterAsk, Master.DefaultOptions)

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
        Dim doOpen As Boolean = True
        If Me.dgvMediaList.SelectedRows.Count > 10 Then
            If Not MsgBox(String.Format(Master.eLang.GetString(635, "You have selected {0} folders to open. Are you sure you want to do this?"), Me.dgvMediaList.SelectedRows.Count), MsgBoxStyle.YesNo Or MsgBoxStyle.Question, Master.eLang.GetString(104, "Are You Sure?")) = MsgBoxResult.Yes Then doOpen = False
        End If

        If doOpen Then
            For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                Using Explorer As New Diagnostics.Process
                    Explorer.StartInfo.FileName = "explorer.exe"
                    Explorer.StartInfo.Arguments = String.Format("/select,""{0}""", sRow.Cells(1).Value)
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RemoveFromDatabaseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveFromDatabaseToolStripMenuItem.Click
        Try

            For Each sRow As DataGridViewRow In Me.dgvMediaList.SelectedRows
                Master.DB.DeleteFromDB(Convert.ToInt64(sRow.Cells(0).Value))
            Next

            Me.FillList(0)

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
            dgvMediaList.Refresh()
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
            FileManip.Delete.DeleteDirectory(Master.TempPath)
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

            Dim pHeight As Integer = Master.Quantize(Me.gbSpecific.Height + Me.lblFilter.Height + 15, 5)

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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub cbFilterFileSource_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbFilterFileSource.SelectedIndexChanged
        Try
            Do While Me.fScanner.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwRefreshMovies.IsBusy OrElse Me.bwCleanDB.IsBusy
                Application.DoEvents()
            Loop

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
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskTrailer.Click
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoTrailer.Click
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuMissAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskTrailer.Click
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.UpdateAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoTrailer.Click
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskTrailer.Click
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAutoTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoTrailer.Click
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAskTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskTrailer.Click
        Master.SetScraperMod(Master.ModType.Trailer, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.DefaultOptions)
    End Sub

    Private Sub CustomUpdaterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomUpdaterToolStripMenuItem.Click
        Using dUpdate As New dlgUpdateMedia
            Dim CustomUpdater As Master.CustomUpdaterStruct = Nothing
            CustomUpdater = dUpdate.ShowDialog()
            If Not CustomUpdater.Canceled Then
                Me.ScrapeData(CustomUpdater.ScrapeType, CustomUpdater.Options)
            End If
        End Using
    End Sub

    Private Sub mnuAllAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoMI.Click
        Master.SetScraperMod(Master.ModType.Meta, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuAllAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskMI.Click
        Master.SetScraperMod(Master.ModType.Meta, True, True)
        Me.ScrapeData(Master.ScrapeType.FullAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoMI.Click
        Master.SetScraperMod(Master.ModType.Meta, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuNewAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskMI.Click
        Master.SetScraperMod(Master.ModType.Meta, True, True)
        Me.ScrapeData(Master.ScrapeType.NewAsk, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAutoMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoMI.Click
        Master.SetScraperMod(Master.ModType.Meta, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAuto, Master.DefaultOptions)
    End Sub

    Private Sub mnuMarkAskMI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskMI.Click
        Master.SetScraperMod(Master.ModType.Meta, True, True)
        Me.ScrapeData(Master.ScrapeType.MarkAsk, Master.DefaultOptions)
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
                    doFill = Me.RefreshMovie(Convert.ToInt64(sRow.Cells(0).Value), doBatch)
                Next
                SQLtransaction.Commit()
            End Using

            Me.dgvMediaList.Cursor = Cursors.Default
            Me.dgvMediaList.Enabled = True

            If doFill Then FillList(0) Else DoTitleCheck()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RefreshAllMoviesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RefreshAllMoviesToolStripMenuItem.Click
        RefreshAllMovies()
    End Sub

    Private Sub RefreshAllMovies()
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
            Me.tslStatus.Text = Master.currMovie.Filename
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                    Me.tslStatus.Text = Master.currMovie.Filename
            End Select
        End Using
    End Sub

    Private Sub cmnuMetaData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMetaData.Click
        Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
        Dim ID As Integer = Convert.ToInt32(Me.dgvMediaList.Item(0, indX).Value)
        Me.tmpTitle = Me.dgvMediaList.Item(15, indX).Value.ToString
        Using dEditMeta As New dlgFileInfo
            Select Case dEditMeta.ShowDialog()
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
        Me.ToolsToolStripMenuItem.Enabled = False
        Me.tsbAutoPilot.Enabled = False
        Me.tsbRefreshMedia.Enabled = False
        Me.mnuMediaList.Enabled = False
        Me.tabsMain.Enabled = False
        Me.tspbLoading.Style = ProgressBarStyle.Marquee
        Me.EnableFilters(False)

        Me.tslLoading.Text = Master.eLang.GetString(999, "Cleaning Database:")
        Me.tspbLoading.Visible = True
        Me.tslLoading.Visible = True

        Me.bwCleanDB.WorkerSupportsCancellation = True
        Me.bwCleanDB.RunWorkerAsync()
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            e.Result = New Results With {.fileinfo = "error", .setEnabled = Args.setEnabled}
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
                        XML.GetAVImages(Res.Movie)
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

        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
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

        Dim Res As Results = DirectCast(e.Result, Results)

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
                Dim lenSize As Integer
                Dim rect As Rectangle

                If Not IsNothing(Me.MainPoster.Image) Then
                    Me.pbPosterCache.Image = Me.MainPoster.Image
                    ImageManip.ResizePB(Me.pbPoster, Me.pbPosterCache, PosterMaxHeight, PosterMaxWidth)
                    ImageManip.SetGlassOverlay(Me.pbPoster)
                    Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)

                    If Master.eSettings.ShowDims Then
                        g = Graphics.FromImage(pbPoster.Image)
                        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        strSize = String.Format("{0} x {1}", Me.MainPoster.Image.Width, Me.MainPoster.Image.Height)
                        lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                        rect = New Rectangle(Convert.ToInt32((pbPoster.Image.Width - lenSize) / 2 - 15), Me.pbPoster.Height - 25, lenSize + 30, 25)
                        ImageManip.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
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

                ImageManip.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = Convert.ToInt32((Me.scMain.Panel2.Width - Me.pbFanart.Width) / 2)

                If Not IsNothing(pbFanart.Image) AndAlso Master.eSettings.ShowDims Then
                    g = Graphics.FromImage(pbFanart.Image)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    strSize = String.Format("{0} x {1}", Me.MainFanart.Image.Width, Me.MainFanart.Image.Height)
                    lenSize = Convert.ToInt32(g.MeasureString(strSize, New Font("Arial", 8, FontStyle.Bold)).Width)
                    rect = New Rectangle((pbFanart.Image.Width - lenSize) - 40, 10, lenSize + 30, 25)
                    ImageManip.DrawGradEllipse(g, rect, Color.FromArgb(250, 120, 120, 120), Color.FromArgb(0, 255, 255, 255))
                    g.DrawString(strSize, New Font("Arial", 8, FontStyle.Bold), New SolidBrush(Color.White), (pbFanart.Image.Width - lenSize) - 25, 15)
                End If

                Me.InfoCleared = False

                If Not bwScraper.IsBusy AndAlso Not bwRefreshMovies.IsBusy AndAlso Not bwCleanDB.IsBusy Then
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

        Dim myDelegate As MydtMediaUpdate = New MydtMediaUpdate(AddressOf dtMediaUpdate)
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        Dim TMDB As New TMDB.Scraper
        Dim IMPA As New IMPA.Scraper
        Dim Trailer As New Trailers
        Dim iCount As Integer = 0
        Dim tURL As String = String.Empty
        Dim Poster As New Images
        Dim Fanart As New Images
        Dim scrapeMovie As New Master.DBMovie
        Dim doSave As Boolean = False
        Dim pResults As New Master.ImgResult
        Dim fResults As New Master.ImgResult
        Dim didEts As Boolean = False
        Dim tTitle As String = String.Empty
        Dim OldTitle As String = String.Empty

        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction

            Try
                If Me.dtMedia.Rows.Count > 0 Then

                    Select Case Args.scrapeType
                        Case Master.ScrapeType.FullAuto, Master.ScrapeType.NewAuto, Master.ScrapeType.MarkAuto, Master.ScrapeType.FullAsk, Master.ScrapeType.NewAsk, Master.ScrapeType.MarkAsk, Master.ScrapeType.FilterAsk, Master.ScrapeType.FilterAuto
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Select Case Args.scrapeType
                                    Case Master.ScrapeType.NewAsk, Master.ScrapeType.NewAuto
                                        If Not Convert.ToBoolean(drvRow.Item(10)) Then Continue For
                                    Case Master.ScrapeType.MarkAsk, Master.ScrapeType.MarkAuto
                                        If Not Convert.ToBoolean(drvRow.Item(11)) Then Continue For
                                    Case Master.ScrapeType.FilterAsk, Master.ScrapeType.FilterAuto
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
                                        scrapeMovie.Movie = IMDB.GetSearchMovieInfo(drvRow.Item(15).ToString, New Media.Movie, Args.scrapeType, Args.Options)
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
                                        If Poster.IsAllowedToDownload(scrapeMovie, Master.ImageType.Posters) Then
                                            pResults = New Master.ImgResult
                                            If Poster.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Posters, pResults, scrapeMovie.Filename, False, If(Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk, True, False)) Then
                                                If Not IsNothing(Poster.Image) Then
                                                    pResults.ImagePath = Poster.SaveAsPoster(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                                        scrapeMovie.PosterPath = pResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Thumb = pResults.Posters
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk Then
                                                    MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        pResults = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Posters)
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
                                        If Fanart.IsAllowedToDownload(scrapeMovie, Master.ImageType.Fanart) Then
                                            fResults = New Master.ImgResult
                                            didEts = True
                                            If Fanart.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Fanart, fResults, scrapeMovie.Filename, Master.GlobalScrapeMod.Extra, If(Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk, True, False)) Then
                                                If Not IsNothing(Fanart.Image) Then
                                                    fResults.ImagePath = Fanart.SaveAsFanart(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                                        scrapeMovie.FanartPath = fResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Fanart = fResults.Fanart
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.FullAsk OrElse Args.scrapeType = Master.ScrapeType.NewAsk OrElse Args.scrapeType = Master.ScrapeType.MarkAsk Then
                                                    MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))

                                                    Using dImgSelect As New dlgImgSelect
                                                        fResults = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Fanart)
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
                                        Dim ETasFA As String = Master.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs, False)
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
                                    tTitle = StringManip.FilterTokens(scrapeMovie.Movie.Title)
                                    If Not OldTitle = scrapeMovie.Movie.Title OrElse String.IsNullOrEmpty(scrapeMovie.Movie.SortTitle) Then scrapeMovie.Movie.SortTitle = tTitle
                                    If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.Year) Then
                                        scrapeMovie.ListTitle = String.Format("{0} ({1})", tTitle, scrapeMovie.Movie.Year)
                                    Else
                                        scrapeMovie.ListTitle = tTitle
                                    End If
                                Else
                                    If Directory.GetParent(drvRow.Item(1).ToString).Name.ToLower = "video_ts" Then
                                        scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(drvRow.Item(1).ToString).FullName).Name)
                                    Else
                                        If Convert.ToBoolean(drvRow.Item(46)) AndAlso Convert.ToBoolean(drvRow.Item(2)) Then
                                            scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(drvRow.Item(1).ToString).Name)
                                        Else
                                            scrapeMovie.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString))
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

                        Case Master.ScrapeType.UpdateAsk, Master.ScrapeType.UpdateAuto

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
                                            scrapeMovie.Movie = IMDB.GetSearchMovieInfo(drvRow.Item(15).ToString, New Media.Movie, Args.scrapeType, Args.Options)
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
                                        If Poster.IsAllowedToDownload(scrapeMovie, Master.ImageType.Posters) Then
                                            pResults = New Master.ImgResult
                                            If Poster.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Posters, pResults, scrapeMovie.Filename, False, If(Args.scrapeType = Master.ScrapeType.UpdateAsk, True, False)) Then
                                                If Not IsNothing(Poster.Image) Then
                                                    pResults.ImagePath = Poster.SaveAsPoster(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                                                        scrapeMovie.PosterPath = pResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 4, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Thumb = pResults.Posters
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.UpdateAsk Then
                                                    MsgBox(Master.eLang.GetString(113, "A poster of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        pResults = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Posters)
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
                                        If Fanart.IsAllowedToDownload(scrapeMovie, Master.ImageType.Fanart) Then
                                            fResults = New Master.ImgResult
                                            didEts = True
                                            If Fanart.GetPreferredImage(scrapeMovie.Movie.IMDBID, Master.ImageType.Fanart, fResults, scrapeMovie.Filename, Master.GlobalScrapeMod.Extra, If(Args.scrapeType = Master.ScrapeType.UpdateAsk, True, False)) Then
                                                If Not IsNothing(Fanart.Image) Then
                                                    fResults.ImagePath = Fanart.SaveAsFanart(scrapeMovie)
                                                    If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                                                        scrapeMovie.FanartPath = fResults.ImagePath
                                                        Me.Invoke(myDelegate, New Object() {drvRow, 5, True})
                                                        If Master.GlobalScrapeMod.NFO AndAlso Not Master.eSettings.NoSaveImagesToNfo Then
                                                            scrapeMovie.Movie.Fanart = fResults.Fanart
                                                        End If
                                                    End If
                                                ElseIf Args.scrapeType = Master.ScrapeType.UpdateAsk Then
                                                    MsgBox(Master.eLang.GetString(115, "Fanart of your preferred size could not be found. Please choose another."), MsgBoxStyle.Information, Master.eLang.GetString(114, "No Preferred Size"))
                                                    Using dImgSelect As New dlgImgSelect
                                                        fResults = dImgSelect.ShowDialog(scrapeMovie, Master.ImageType.Fanart)
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
                                            Dim ETasFA As String = Master.CreateRandomThumbs(scrapeMovie, Master.eSettings.AutoThumbs, False)
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
                                        tTitle = StringManip.FilterTokens(scrapeMovie.Movie.Title)
                                        If Not OldTitle = scrapeMovie.Movie.Title OrElse String.IsNullOrEmpty(scrapeMovie.Movie.SortTitle) Then scrapeMovie.Movie.SortTitle = tTitle
                                        If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(scrapeMovie.Movie.Year) Then
                                            scrapeMovie.ListTitle = String.Format("{0} ({1})", tTitle, scrapeMovie.Movie.Year)
                                        Else
                                            scrapeMovie.ListTitle = tTitle
                                        End If
                                    Else
                                        If Directory.GetParent(drvRow.Item(1).ToString).Name.ToLower = "video_ts" Then
                                            scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(drvRow.Item(1).ToString).FullName).Name)
                                        Else
                                            If Convert.ToBoolean(drvRow.Item(46)) AndAlso Convert.ToBoolean(drvRow.Item(2)) Then
                                                scrapeMovie.ListTitle = StringManip.FilterName(Directory.GetParent(drvRow.Item(1).ToString).Name)
                                            Else
                                                scrapeMovie.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString))
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

                        Case Master.ScrapeType.CleanFolders
                            Dim fDeleter As New FileManip.Delete
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
                        Case Master.ScrapeType.CopyBD
                            Dim sPath As String = String.Empty
                            For Each drvRow As DataRow In Me.dtMedia.Rows

                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(15).ToString)
                                iCount += 1

                                If Me.bwScraper.CancellationPending Then GoTo doCancel
                                sPath = drvRow.Item(40).ToString
                                If Not String.IsNullOrEmpty(sPath) Then
                                    If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                                        If Master.eSettings.VideoTSParent Then
                                            FileManip.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), "-fanart.jpg")))
                                        Else
                                            If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                                FileManip.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")))
                                            Else
                                                FileManip.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)))
                                            End If
                                        End If
                                    Else
                                        If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                            FileManip.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString), "-fanart.jpg")))
                                        Else
                                            FileManip.Common.MoveFileWithStream(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)))
                                        End If
                                    End If
                                End If
                            Next
                        Case Master.ScrapeType.RevertStudios
                            For Each drvRow As DataRow In Me.dtMedia.Rows
                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(15).ToString)
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
            If Not Args.scrapeType = Master.ScrapeType.CopyBD Then
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
                Me.tslStatus.Text = e.UserState.ToString
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

                If DirectCast(e.Result, Master.ScrapeType) = Master.ScrapeType.CleanFolders Then
                    'only rescan media if expert cleaner and videos are not whitelisted 
                    'since the db is updated during cleaner now.
                    If Master.eSettings.ExpertCleaner AndAlso Not Master.eSettings.CleanWhitelistVideo Then
                        Me.LoadMedia(1)
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
                Me.RefreshMovie(Convert.ToInt64(sRow.Item(0)), True)
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

    Private Sub bwCleanDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwCleanDB.DoWork
        Master.DB.Clean()
    End Sub

    Private Sub bwCleanDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwCleanDB.RunWorkerCompleted

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

    Private Sub LoadTheme(ByVal tType As String)
        Dim tPath As String = String.Concat(Master.AppPath, "Themes", Path.DirectorySeparatorChar, String.Format("{0}-{1}.xml", tType, Master.eSettings.MovieTheme))
        If File.Exists(tPath) Then

            'Just to make sure Theme will setup ok (Issues r893,r894)
            'force size
            Me.MinimumSize = New Size(1024, 768)
            Me.Size = Me.MinimumSize

            ThemeXML = XDocument.Load(tPath)
            'top panel
            Try
                Dim xTop = From xTheme In ThemeXML...<theme>...<toppanel>
                If xTop.Count > 0 Then
                    If Not String.IsNullOrEmpty(xTop.<backcolor>.Value) Then Me.pnlTop.BackColor = Color.FromArgb(Convert.ToInt32(xTop.<backcolor>.Value))
                    Me.pnlInfoIcons.BackColor = Me.pnlTop.BackColor
                    Me.pnlRating.BackColor = Me.pnlTop.BackColor
                    Me.pbVideo.BackColor = Me.pnlTop.BackColor
                    Me.pbResolution.BackColor = Me.pnlTop.BackColor
                    Me.pbAudio.BackColor = Me.pnlTop.BackColor
                    Me.pbChannels.BackColor = Me.pnlTop.BackColor
                    Me.pbStudio.BackColor = Me.pnlTop.BackColor
                    Me.pbStar1.BackColor = Me.pnlTop.BackColor
                    Me.pbStar2.BackColor = Me.pnlTop.BackColor
                    Me.pbStar3.BackColor = Me.pnlTop.BackColor
                    Me.pbStar4.BackColor = Me.pnlTop.BackColor
                    Me.pbStar5.BackColor = Me.pnlTop.BackColor

                    If Not String.IsNullOrEmpty(xTop.<forecolor>.Value) Then Me.lblTitle.ForeColor = Color.FromArgb(Convert.ToInt32(xTop.<forecolor>.Value))
                    Me.lblVotes.ForeColor = Me.lblTitle.ForeColor
                    Me.lblRuntime.ForeColor = Me.lblTitle.ForeColor
                    Me.lblTagline.ForeColor = Me.lblTitle.ForeColor
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            'images
            Try
                Dim xImages = From xTheme In ThemeXML...<theme>...<images>
                If xImages.Count > 0 Then
                    If Not String.IsNullOrEmpty(xImages.<fanartbackcolor>.Value) Then Me.scMain.Panel2.BackColor = Color.FromArgb(Convert.ToInt32(xImages.<fanartbackcolor>.Value))
                    Me.pbFanart.BackColor = Me.scMain.Panel2.BackColor
                    If Not String.IsNullOrEmpty(xImages.<posterbackcolor>.Value) Then Me.pbPoster.BackColor = Color.FromArgb(Convert.ToInt32(xImages.<posterbackcolor>.Value))
                    If Not String.IsNullOrEmpty(xImages.<postermaxheight>.Value) Then Me.PosterMaxHeight = Convert.ToInt32(xImages.<postermaxheight>.Value)
                    If Not String.IsNullOrEmpty(xImages.<postermaxwidth>.Value) Then Me.PosterMaxWidth = Convert.ToInt32(xImages.<postermaxwidth>.Value)
                    If Not String.IsNullOrEmpty(xImages.<mpaabackcolor>.Value) Then Me.pnlMPAA.BackColor = Color.FromArgb(Convert.ToInt32(xImages.<mpaabackcolor>.Value))
                    Me.pbMPAA.BackColor = Me.pnlMPAA.BackColor
                    If Not String.IsNullOrEmpty(xImages.<genrebackcolor>.Value) Then Me.GenrePanelColor = Color.FromArgb(Convert.ToInt32(xImages.<genrebackcolor>.Value))
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            'info panel
            SetIPTheme(pnlInfoPanel)
        End If
    End Sub

    Private Sub SetIPTheme(ByVal cControl As Control)
        Try
            Dim ControlName As String

            Dim cFont As String = "Microsoft Sans Serif"
            Dim cFontSize As Integer = 8
            Dim cFontStyle As FontStyle = FontStyle.Bold

            'info panel
            Dim xIPMain = From xTheme In ThemeXML...<theme>...<infopanel> Select xTheme.<backcolor>.Value, xTheme.<ipup>.Value, xTheme.<ipmid>.Value
            If xIPMain.Count > 0 Then
                If Not String.IsNullOrEmpty(xIPMain(0).backcolor) Then Me.pnlInfoPanel.BackColor = Color.FromArgb(Convert.ToInt32(xIPMain(0).backcolor))
                If Not String.IsNullOrEmpty(xIPMain(0).ipup) Then Me.IPUp = Convert.ToInt32(xIPMain(0).ipup)
                If Not String.IsNullOrEmpty(xIPMain(0).ipmid) Then Me.IPMid = Convert.ToInt32(xIPMain(0).ipmid)
            End If

            For Each xControl As Control In cControl.Controls
                Try
                    ControlName = xControl.Name
                    Dim xIP = From xTheme In ThemeXML...<theme>...<infopanel>...<object> Where ControlName = xTheme.@name
                    If xIP.Count > 0 Then
                        If Not String.IsNullOrEmpty(xIP.<width>.Value) Then xControl.Width = Convert.ToInt32(xIP.<width>.Value)
                        If Not String.IsNullOrEmpty(xIP.<height>.Value) Then xControl.Height = Convert.ToInt32(xIP.<height>.Value)
                        If Not String.IsNullOrEmpty(xIP.<left>.Value) Then xControl.Left = Convert.ToInt32(xIP.<left>.Value)
                        If Not String.IsNullOrEmpty(xIP.<top>.Value) Then xControl.Top = Convert.ToInt32(xIP.<top>.Value)
                        If Not String.IsNullOrEmpty(xIP.<backcolor>.Value) Then xControl.BackColor = Color.FromArgb(Convert.ToInt32(xIP.<backcolor>.Value))
                        If Not String.IsNullOrEmpty(xIP.<forecolor>.Value) Then xControl.ForeColor = Color.FromArgb(Convert.ToInt32(xIP.<forecolor>.Value))
                        If Not String.IsNullOrEmpty(xIP.<anchor>.Value) Then xControl.Anchor = DirectCast(Convert.ToInt32(xIP.<anchor>.Value), AnchorStyles)
                        If Not String.IsNullOrEmpty(xIP.<anchor>.Value) Then xControl.Anchor = DirectCast(Convert.ToInt32(xIP.<anchor>.Value), AnchorStyles)

                        cFont = "Microsoft Sans Serif"
                        cFontSize = 8
                        cFontStyle = FontStyle.Regular

                        If Not String.IsNullOrEmpty(xIP.<font>.Value) Then cFont = xIP.<font>.Value
                        If Not String.IsNullOrEmpty(xIP.<fontsize>.Value) Then cFontSize = Convert.ToInt32(xIP.<fontsize>.Value)
                        If Not String.IsNullOrEmpty(xIP.<fontstyle>.Value) Then cFontStyle = DirectCast(Convert.ToInt32(xIP.<fontstyle>.Value), FontStyle)
                        xControl.Font = New Font(cFont, cFontSize, cFontStyle)
                    End If
                    If xControl.HasChildren Then SetIPTheme(xControl)
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetUp(ByVal doTheme As Boolean)

        Try
            With Me
                .btnSortDate.Tag = String.Empty
                .pnlFilterGenre.Tag = String.Empty
                .pnlFilterSource.Tag = String.Empty
                .btnSortTitle.Tag = String.Empty
                .btnIMDBRating.Tag = String.Empty
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
                .RemoveToolStripMenuItem.Text = Master.eLang.GetString(999, "Remove")
                .DeleteMovieToolStripMenuItem.Text = Master.eLang.GetString(34, "Delete Movie")
                .RemoveFromDatabaseToolStripMenuItem.Text = Master.eLang.GetString(999, "Remove From Database")
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
                .CustomUpdaterToolStripMenuItem.Text = Master.eLang.GetString(81, "Custom Scraper...")
                .tsbRefreshMedia.Text = Master.eLang.GetString(82, "Update Library")
                .tsbUpdateXBMC.Text = Master.eLang.GetString(83, "Initiate XBMC Update")
                .Label6.Text = Master.eLang.GetString(579, "File Source:")
                .GroupBox1.Text = Master.eLang.GetString(600, "Extra Sorting")
                .btnSortDate.Text = Master.eLang.GetString(601, "Date Added")
                .cmnuMetaData.Text = Master.eLang.GetString(603, "Edit Meta Data")
                .btnSortTitle.Text = Master.eLang.GetString(642, "Sort Title")
                .btnIMDBRating.Text = Master.eLang.GetString(651, "IMDB Rating")

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

                If doTheme Then .LoadTheme("movie")

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
            Application.DoEvents()

            Me.ClearInfo()
            Me.ClearFilters()
            Me.EnableFilters(False)

            Me.ToolsToolStripMenuItem.Enabled = False
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.mnuMediaList.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tabMovies.Text = Master.eLang.GetString(36, "Movies")
            Me.txtSearch.Text = String.Empty

            Me.fScanner.CancelAndWait()
            Me.dgvMediaList.DataSource = Nothing

            Me.fScanner.Start(SourceName)

        Catch ex As Exception
            Me.LoadingDone = True
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
                Me.txtMetaData.Clear()
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

                .MainPoster.Clear()
                .MainFanart.Clear()

                .txtMetaData.Text = String.Empty
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

            Me.alActors = New List(Of String)

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
            If tmpRating > 0 Then
                Me.BuildStars(tmpRating)
            Else
                ''ToolTips.SetToolTip(pbStar1, "Rating: N/A")
                ''ToolTips.SetToolTip(pbStar2, "Rating: N/A")
                ''ToolTips.SetToolTip(pbStar3, "Rating: N/A")
                ''ToolTips.SetToolTip(pbStar4, "Rating: N/A")
                ''ToolTips.SetToolTip(pbStar5, "Rating: N/A")
            End If

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
                Me.pnlGenre(i).BackColor = Me.GenrePanelColor
                Me.pbGenre(i).BackColor = Me.GenrePanelColor
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
                    Dim drawSize As Integer = Convert.ToInt32((14 * (bmGenre.Width / drawWidth)) - 0.5)
                    drawFont = New Font("Microsoft Sans Serif", If(drawSize > 14, 14, drawSize), FontStyle.Bold, GraphicsUnit.Pixel)
                    Dim drawHeight As Single = grGenre.MeasureString(drawString, drawFont).Height
                    iLeft = Convert.ToInt32((bmGenre.Width - grGenre.MeasureString(drawString, drawFont).Width) / 2)
                    grGenre.DrawString(drawString, drawFont, drawBrush, iLeft, (bmGenre.Height - drawHeight))
                    pbGenre(i).Image = bmGenre
                End If
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ScrapeData(ByVal sType As Master.ScrapeType, ByVal Options As Master.ScrapeOptions, Optional ByVal ID As Integer = 0, Optional ByVal doSearch As Boolean = False)

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
                Case Master.ScrapeType.FullAsk, Master.ScrapeType.FullAuto, Master.ScrapeType.CleanFolders, Master.ScrapeType.CopyBD, Master.ScrapeType.RevertStudios, Master.ScrapeType.UpdateAsk, Master.ScrapeType.UpdateAuto, Master.ScrapeType.FilterAsk, Master.ScrapeType.FilterAuto
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
                        Case Master.ScrapeType.FilterAsk
                            Me.tslLoading.Text = Master.eLang.GetString(622, "Scraping Media (Current Filter - Ask):")
                        Case Master.ScrapeType.FilterAuto
                            Me.tslLoading.Text = Master.eLang.GetString(623, "Scraping Media (Current Filter - Auto):")
                    End Select
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    bwScraper.WorkerReportsProgress = True
                    bwScraper.WorkerSupportsCancellation = True
                    bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .Options = Options})

                Case Master.ScrapeType.NewAsk, Master.ScrapeType.NewAuto, Master.ScrapeType.MarkAsk, Master.ScrapeType.MarkAuto
                    For Each drvRow As DataRow In Me.dtMedia.Rows
                        If Convert.ToBoolean(drvRow.Item(10)) AndAlso (sType = Master.ScrapeType.NewAsk OrElse sType = Master.ScrapeType.NewAuto) Then
                            chkCount += 1
                        End If
                        If Convert.ToBoolean(drvRow.Item(11)) AndAlso (sType = Master.ScrapeType.MarkAsk OrElse sType = Master.ScrapeType.MarkAuto) Then
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

                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .Options = Options})
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
                            Me.btnMarkAll.Enabled = True
                            Me.pnlCancel.Visible = False
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
                        Dim fResults As New Master.ImgResult
                        Dim pResults As New Master.ImgResult

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
                                    If Poster.GetPreferredImage(Master.currMovie.Movie.IMDBID, Master.ImageType.Posters, pResults, Master.currMovie.Filename, False) Then
                                        If Not IsNothing(Poster.Image) Then
                                            Master.currMovie.PosterPath = Poster.SaveAsPoster(Master.currMovie)
                                            If Not Master.eSettings.NoSaveImagesToNfo Then Master.currMovie.Movie.Thumb = pResults.Posters
                                        End If
                                    End If
                                End If
                                pResults = Nothing

                                If Fanart.IsAllowedToDownload(Master.currMovie, Master.ImageType.Fanart) Then
                                    If Fanart.GetPreferredImage(Master.currMovie.Movie.IMDBID, Master.ImageType.Fanart, fResults, Master.currMovie.Filename, True) Then
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
                                Master.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs, False)
                            End If
                        Catch
                        End Try
                        Me.ScraperDone = True
                    Else
                        Master.tmpMovie.Clear()
                        If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) AndAlso doSearch = False Then
                            Master.tmpMovie = Master.currMovie.Movie
                            IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.tmpMovie, Master.DefaultOptions, Master.eSettings.FullCrew, Master.eSettings.FullCast)
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
                                        Master.currMovie.ClearExtras = True
                                        Master.currMovie.PosterPath = String.Empty
                                        Master.currMovie.FanartPath = String.Empty
                                        Master.currMovie.TrailerPath = String.Empty
                                        Master.currMovie.ExtraPath = String.Empty
                                        Master.currMovie.SubPath = String.Empty
                                        Master.currMovie.NfoPath = String.Empty
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
                    Application.DoEvents()
                    Me.UpdateMediaInfo(Master.currMovie)
                End If

                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    Using dImgSelectFanart As New dlgImgSelect
                        Dim AllowFA As Boolean = tmpImages.IsAllowedToDownload(Master.currMovie, Master.ImageType.Fanart, True)

                        If AllowFA Then dImgSelectFanart.PreLoad(Master.currMovie, Master.ImageType.Fanart, True)

                        If tmpImages.IsAllowedToDownload(Master.currMovie, Master.ImageType.Posters, True) Then
                            Me.tslLoading.Text = Master.eLang.GetString(572, "Scraping Posters:")
                            Application.DoEvents()
                            Using dImgSelect As New dlgImgSelect
                                Dim pResults As Master.ImgResult = dImgSelect.ShowDialog(Master.currMovie, Master.ImageType.Posters, True)
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
                            Dim fResults As Master.ImgResult = dImgSelectFanart.ShowDialog
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
                    Dim tURL As String = cTrailer.ShowTDialog(Master.currMovie.Movie.IMDBID, Master.currMovie.Filename, Master.currMovie.Movie.Trailer)
                    If Not String.IsNullOrEmpty(tURL) AndAlso tURL.Substring(0, 7) = "http://" Then
                        Master.currMovie.Movie.Trailer = tURL
                    End If
                    cTrailer = Nothing
                End If

                If Master.eSettings.AutoThumbs > 0 AndAlso Master.currMovie.isSingle Then
                    Me.tslLoading.Text = Master.eLang.GetString(575, "Generating Extrathumbs:")
                    Application.DoEvents()
                    Master.CreateRandomThumbs(Master.currMovie, Master.eSettings.AutoThumbs, True)
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
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, ID)
                            Case Windows.Forms.DialogResult.Abort
                                Master.currMovie.ClearExtras = False
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Master.DefaultOptions, ID, True)
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Master.currMovie.ClearExtras = False

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

    Private Function RefreshMovie(ByVal ID As Long, Optional ByVal BatchMode As Boolean = False, Optional ByVal FromNfo As Boolean = True, Optional ByVal ToNfo As Boolean = False) As Boolean
        Dim dRow = From drvRow In dtMedia.Rows Where Convert.ToInt64(DirectCast(drvRow, DataRow).Item(0)) = ID Select drvRow
        Dim tmpMovie As New Media.Movie
        Dim tmpMovieDb As New Master.DBMovie
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
                    If Directory.GetParent(tmpMovieDb.Filename).Name.ToLower = "video_ts" Then
                        tmpMovieDb.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(tmpMovieDb.Filename).FullName).Name)
                        tmpMovieDb.Movie.Title = StringManip.FilterName(Directory.GetParent(Directory.GetParent(tmpMovieDb.Filename).FullName).Name, False)
                    Else
                        If tmpMovieDb.UseFolder AndAlso tmpMovieDb.isSingle Then
                            tmpMovieDb.ListTitle = StringManip.FilterName(Directory.GetParent(tmpMovieDb.Filename).Name)
                            tmpMovieDb.Movie.Title = StringManip.FilterName(Directory.GetParent(tmpMovieDb.Filename).Name, False)
                        Else
                            tmpMovieDb.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(tmpMovieDb.Filename))
                            tmpMovieDb.Movie.Title = StringManip.FilterName(Path.GetFileNameWithoutExtension(tmpMovieDb.Filename), False)
                        End If
                    End If
                    If Not OldTitle = tmpMovieDb.Movie.Title OrElse String.IsNullOrEmpty(tmpMovieDb.Movie.SortTitle) Then tmpMovieDb.Movie.SortTitle = tmpMovieDb.ListTitle
                Else
                    Dim tTitle As String = StringManip.FilterTokens(tmpMovieDb.Movie.Title)
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

                tmpMovieDb.FileSource = XML.GetFileSource(tmpMovieDb.Filename)
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
                    cbFilterFileSource.Items.AddRange(XML.GetSourceList)
                    cbFilterFileSource.SelectedIndex = 0
                    AddHandler cbFilterFileSource.SelectedIndexChanged, AddressOf cbFilterFileSource_SelectedIndexChanged

                End If
            End With

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                            If Directory.GetParent(drvRow.Cells(1).Value.ToString).Name.ToLower = "video_ts" Then
                                pTitle = Directory.GetParent(Directory.GetParent(drvRow.Cells(1).Value.ToString).FullName).Name
                            Else
                                If Convert.ToBoolean(drvRow.Cells(46).Value) AndAlso Convert.ToBoolean(drvRow.Cells(2).Value) Then
                                    pTitle = Directory.GetParent(drvRow.Cells(1).Value.ToString).Name
                                Else
                                    pTitle = Path.GetFileNameWithoutExtension(drvRow.Cells(1).Value.ToString)
                                End If
                            End If

                            LevFail = StringManip.ComputeLevenshtein(StringManip.FilterName(drvRow.Cells(15).Value.ToString, False, True).ToLower, StringManip.FilterName(pTitle, False, True).ToLower) > Master.eSettings.LevTolerance

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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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

                If Me.chkFilterDupe.Checked Then
                    Me.FillList(0, True)
                ElseIf Not String.IsNullOrEmpty(Me.filSearch) AndAlso isActorSearch Then
                    Me.FillList(0, False, Me.filSearch)
                Else
                    If doFill Then
                        Me.FillList(0)
                    End If
                End If

            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub SourceSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim SourceName As String = sender.ToString.Replace(Master.eLang.GetString(144, "Update"), String.Empty).Replace(Master.eLang.GetString(145, "Only"), String.Empty).Trim
        If Not String.IsNullOrEmpty(SourceName) Then
            Me.LoadMedia(1, SourceName)
        End If
    End Sub

    Private Sub XComSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim xComName As String = sender.ToString.Replace(Master.eLang.GetString(144, "Update"), String.Empty).Replace(Master.eLang.GetString(145, "Only"), String.Empty).Trim
        Dim xCom = From x As emmSettings.XBMCCom In Master.eSettings.XBMCComs Where x.Name = xComName
        If xCom.Count > 0 Then
            DoXCom(xCom(0))
        End If
    End Sub

    Private Sub DoXCom(ByVal xCom As emmSettings.XBMCCom)
        Try
            Dim Wr As WebRequest = HttpWebRequest.Create(String.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?command=ExecBuiltIn&parameter=XBMC.updatelibrary(video)", xCom.IP, xCom.Port))
            Wr.Method = "GET"
            Wr.Timeout = 2500
            If Not String.IsNullOrEmpty(xCom.Username) AndAlso Not String.IsNullOrEmpty(xCom.Password) Then
                Wr.Credentials = New NetworkCredential(xCom.Username, xCom.Password)
            End If
            Using Wres As WebResponse = Wr.GetResponse
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

    Private Sub FillList(ByVal iIndex As Integer, Optional ByVal DupesOnly As Boolean = False, Optional ByVal Actor As String = "")
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
                    .dgvTVShows.Columns(2).Visible = Not Master.eSettings.MoviePosterCol
                    .dgvTVShows.Columns(2).ToolTipText = Master.eLang.GetString(148, "Poster")
                    .dgvTVShows.Columns(3).Width = 20
                    .dgvTVShows.Columns(3).Resizable = DataGridViewTriState.False
                    .dgvTVShows.Columns(3).ReadOnly = True
                    .dgvTVShows.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvTVShows.Columns(3).Visible = Not Master.eSettings.MovieFanartCol
                    .dgvTVShows.Columns(3).ToolTipText = Master.eLang.GetString(149, "Fanart")
                    .dgvTVShows.Columns(4).Width = 20
                    .dgvTVShows.Columns(4).Resizable = DataGridViewTriState.False
                    .dgvTVShows.Columns(4).ReadOnly = True
                    .dgvTVShows.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvTVShows.Columns(4).Visible = Not Master.eSettings.MovieInfoCol
                    .dgvTVShows.Columns(4).ToolTipText = Master.eLang.GetString(150, "Nfo")
                    For i As Integer = 5 To .dgvTVShows.Columns.Count - 1
                        .dgvTVShows.Columns(i).Visible = False
                    Next

                    .dgvTVShows.Columns(0).ValueType = GetType(Int32)

                    'Trick to autosize the first column, but still allow resizing by user
                    .dgvTVShows.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .dgvTVShows.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                    .dgvTVShows.Sort(.dgvTVShows.Columns(1), ComponentModel.ListSortDirection.Ascending)

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

        Catch ex As Exception
            Me.LoadingDone = True
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If Not isCL Then
            Me.tsbRefreshMedia.Enabled = True
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.tspbLoading.Value = 0

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

            Me.tmpTitle = Me.dgvMediaList.Item(15, iRow).Value.ToString
            If Not Convert.ToBoolean(Me.dgvMediaList.Item(4, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvMediaList.Item(5, iRow).Value) AndAlso Not Convert.ToBoolean(Me.dgvMediaList.Item(6, iRow).Value) Then
                Me.ClearInfo()
                Me.pnlNoInfo.Visible = True
                Master.currMovie = Master.DB.LoadMovieFromDB(Convert.ToInt64(Me.dgvMediaList.Item(0, iRow).Value))
            Else
                Me.pnlNoInfo.Visible = False

                Me.LoadInfo(Convert.ToInt32(Me.dgvMediaList.Item(0, iRow).Value), Me.dgvMediaList.Item(1, iRow).Value.ToString, True, False)
            End If

            If Not Me.fScanner.IsBusy AndAlso Not Me.bwMediaInfo.IsBusy AndAlso Not Me.bwLoadInfo.IsBusy AndAlso Not Me.bwRefreshMovies.IsBusy AndAlso Not Me.bwCleanDB.IsBusy Then
                Me.mnuMediaList.Enabled = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub ScannerProgressUpdated(ByVal iPercent As Integer, ByVal sText As String)
        '//
        ' Update the status bar with the name of the current media name and increase progress bar
        '\\

        If Not isCL Then
            Me.tspbLoading.Value = iPercent
            Me.tslStatus.Text = sText
        End If
    End Sub

    Private Sub ScanningCompleted(ByVal iStatus As Integer, ByVal iMax As Integer)

        Select Case iStatus
            Case 0
                If isCL Then
                    Me.ScraperDone = True
                Else
                    Me.FillList(0)
                    'Me.tslStatus.Text = Master.eLang.GetString(111, "Unable to load directories. Please check settings.")
                    Me.tspbLoading.Visible = False
                    Me.tslLoading.Visible = False
                    Me.tabsMain.Enabled = True
                    Me.tsbRefreshMedia.Enabled = True
                    Me.ToolsToolStripMenuItem.Enabled = False
                    Me.tsbAutoPilot.Enabled = False
                    Me.mnuMediaList.Enabled = False
                End If
            Case 1
                Me.tslLoading.Text = Master.eLang.GetString(7, "Loading Media:")
                Me.tspbLoading.Style = ProgressBarStyle.Continuous
                Me.tslLoading.Visible = True
                Me.tspbLoading.Visible = True
                Me.tspbLoading.Maximum = iMax
            Case 2
                Me.FillList(0)
        End Select

    End Sub

    Private Sub FillSeasons(ByVal ShowID As Integer)
        Me.bsSeasons.DataSource = Nothing
        Me.dgvTVSeasons.DataSource = Nothing
        Me.bsEpisodes.DataSource = Nothing
        Me.dgvTVEpisodes.DataSource = Nothing

        Application.DoEvents()

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
                .dgvTVSeasons.Columns(2).ToolTipText = Master.eLang.GetString(999, "Season")
                .dgvTVSeasons.Columns(2).HeaderText = Master.eLang.GetString(999, "Season")
                .dgvTVSeasons.Columns(3).Visible = False
                .dgvTVSeasons.Columns(4).Width = 20
                .dgvTVSeasons.Columns(4).Resizable = DataGridViewTriState.False
                .dgvTVSeasons.Columns(4).ReadOnly = True
                .dgvTVSeasons.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVSeasons.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                .dgvTVSeasons.Columns(4).ToolTipText = Master.eLang.GetString(148, "Poster")
                For i As Integer = 5 To .dgvTVSeasons.Columns.Count - 1
                    .dgvTVSeasons.Columns(i).Visible = False
                Next

                .dgvTVSeasons.Columns(0).ValueType = GetType(Int32)

                'Trick to autosize the first column, but still allow resizing by user
                .dgvTVSeasons.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .dgvTVSeasons.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                .dgvTVSeasons.Sort(.dgvTVSeasons.Columns(2), ComponentModel.ListSortDirection.Ascending)

            End With
        End If
    End Sub

    Private Sub FillEpisodes(ByVal ShowID As Integer, ByVal Season As Integer)
        Me.bsEpisodes.DataSource = Nothing
        Me.dgvTVEpisodes.DataSource = Nothing

        Application.DoEvents()

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
                .dgvTVEpisodes.Columns(2).ToolTipText = Master.eLang.GetString(999, "Title")
                .dgvTVEpisodes.Columns(2).HeaderText = Master.eLang.GetString(999, "Title")
                .dgvTVEpisodes.Columns(3).Width = 20
                .dgvTVEpisodes.Columns(3).Resizable = DataGridViewTriState.False
                .dgvTVEpisodes.Columns(3).ReadOnly = True
                .dgvTVEpisodes.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVEpisodes.Columns(3).Visible = Not Master.eSettings.MoviePosterCol
                .dgvTVEpisodes.Columns(3).ToolTipText = Master.eLang.GetString(148, "Poster")
                .dgvTVEpisodes.Columns(4).Width = 20
                .dgvTVEpisodes.Columns(4).Resizable = DataGridViewTriState.False
                .dgvTVEpisodes.Columns(4).ReadOnly = True
                .dgvTVEpisodes.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                .dgvTVEpisodes.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                .dgvTVEpisodes.Columns(4).ToolTipText = Master.eLang.GetString(150, "Nfo")
                For i As Integer = 5 To .dgvTVEpisodes.Columns.Count - 1
                    .dgvTVEpisodes.Columns(i).Visible = False
                Next

                .dgvTVEpisodes.Columns(0).ValueType = GetType(Int32)
                .dgvTVEpisodes.Columns(11).ValueType = GetType(Int32)

                'Trick to autosize the first column, but still allow resizing by user
                .dgvTVEpisodes.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                .dgvTVEpisodes.Columns(2).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                .dgvTVEpisodes.Sort(.dgvTVEpisodes.Columns(11), ComponentModel.ListSortDirection.Ascending)

            End With
        End If
    End Sub
#End Region '*** Routines/Functions

    Private Sub dgvTVShows_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVShows.CellClick
        If Me.dgvTVShows.SelectedRows.Count > 0 Then
            Me.FillSeasons(Convert.ToInt32(Me.dgvTVShows.Item(0, Me.dgvTVShows.SelectedRows(0).Index).Value))
        End If
    End Sub

    Private Sub dgvTVSeasons_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvTVSeasons.CellClick
        If Me.dgvTVSeasons.SelectedRows.Count > 0 Then
            Me.FillEpisodes(Convert.ToInt32(Me.dgvTVSeasons.SelectedRows(0).Cells(0).Value), Convert.ToInt32(Me.dgvTVSeasons.SelectedRows(0).Cells(3).Value))
        End If
    End Sub

    Private Sub tabsMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabsMain.SelectedIndexChanged
        Select Case tabsMain.SelectedIndex
            Case 0
                If Me.dgvMediaList.RowCount > 0 Then
                    Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                End If
            Case 1
                If Me.dgvTVShows.RowCount > 0 Then
                    Me.dgvTVShows.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    Me.dgvTVShows.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                End If
        End Select
    End Sub
End Class