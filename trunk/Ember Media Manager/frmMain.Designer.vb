<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ModuleSettingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.tslStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.tsSpring = New System.Windows.Forms.ToolStripStatusLabel
        Me.tslLoading = New System.Windows.Forms.ToolStripStatusLabel
        Me.tspbLoading = New System.Windows.Forms.ToolStripProgressBar
        Me.tmrAni = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip = New System.Windows.Forms.MenuStrip
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CleanFoldersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RenamerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.SetsManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OfflineMediaManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.ExportMoviesListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.ClearAllCachesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RefreshAllMoviesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CleanDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DonateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.scMain = New System.Windows.Forms.SplitContainer
        Me.pnlFilterGenre = New System.Windows.Forms.Panel
        Me.lblGFilClose = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.clbFilterGenres = New System.Windows.Forms.CheckedListBox
        Me.pnlFilterSource = New System.Windows.Forms.Panel
        Me.lblSFilClose = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.clbFilterSource = New System.Windows.Forms.CheckedListBox
        Me.dgvMediaList = New System.Windows.Forms.DataGridView
        Me.mnuMediaList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuTitle = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuRefresh = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuMark = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuLock = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuEditMovie = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuMetaData = New System.Windows.Forms.ToolStripMenuItem
        Me.GenresToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LblGenreStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.GenreListToolStripComboBox = New System.Windows.Forms.ToolStripComboBox
        Me.AddGenreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SetGenreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveGenreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuSep = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuRescrape = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuSearchNew = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.OpenContainingFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.cmuRenamer = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuRenameAuto = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuRenameManual = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.RemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveFromDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteMovieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.scTV = New System.Windows.Forms.SplitContainer
        Me.dgvTVShows = New System.Windows.Forms.DataGridView
        Me.mnuShows = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuShowTitle = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuEditShow = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuRescrapeShow = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuChangeShow = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer
        Me.dgvTVSeasons = New System.Windows.Forms.DataGridView
        Me.mnuSeasons = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.dgvTVEpisodes = New System.Windows.Forms.DataGridView
        Me.mnuEpisodes = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuEpTitle = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuEditEpisode = New System.Windows.Forms.ToolStripMenuItem
        Me.pnlListTop = New System.Windows.Forms.Panel
        Me.btnMarkAll = New System.Windows.Forms.Button
        Me.pnlSearch = New System.Windows.Forms.Panel
        Me.cbSearch = New System.Windows.Forms.ComboBox
        Me.picSearch = New System.Windows.Forms.PictureBox
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.tabsMain = New System.Windows.Forms.TabControl
        Me.tabMovies = New System.Windows.Forms.TabPage
        Me.tabTV = New System.Windows.Forms.TabPage
        Me.pnlFilter = New System.Windows.Forms.Panel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnIMDBRating = New System.Windows.Forms.Button
        Me.btnSortTitle = New System.Windows.Forms.Button
        Me.btnSortDate = New System.Windows.Forms.Button
        Me.btnClearFilters = New System.Windows.Forms.Button
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.chkFilterTolerance = New System.Windows.Forms.CheckBox
        Me.chkFilterMissing = New System.Windows.Forms.CheckBox
        Me.chkFilterDupe = New System.Windows.Forms.CheckBox
        Me.gbSpecific = New System.Windows.Forms.GroupBox
        Me.txtFilterSource = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cbFilterFileSource = New System.Windows.Forms.ComboBox
        Me.chkFilterLock = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbFilterAnd = New System.Windows.Forms.RadioButton
        Me.rbFilterOr = New System.Windows.Forms.RadioButton
        Me.chkFilterNew = New System.Windows.Forms.CheckBox
        Me.cbFilterYear = New System.Windows.Forms.ComboBox
        Me.chkFilterMark = New System.Windows.Forms.CheckBox
        Me.cbFilterYearMod = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtFilterGenre = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnFilterDown = New System.Windows.Forms.Button
        Me.btnFilterUp = New System.Windows.Forms.Button
        Me.lblFilter = New System.Windows.Forms.Label
        Me.pnlCancel = New System.Windows.Forms.Panel
        Me.pbCanceling = New System.Windows.Forms.ProgressBar
        Me.lblCanceling = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.pnlNoInfo = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.pnlInfoPanel = New System.Windows.Forms.Panel
        Me.txtCerts = New System.Windows.Forms.TextBox
        Me.lblCertsHeader = New System.Windows.Forms.Label
        Me.lblReleaseDate = New System.Windows.Forms.Label
        Me.lblReleaseDateHeader = New System.Windows.Forms.Label
        Me.btnMid = New System.Windows.Forms.Button
        Me.pbMILoading = New System.Windows.Forms.PictureBox
        Me.btnMetaDataRefresh = New System.Windows.Forms.Button
        Me.lblMetaDataHeader = New System.Windows.Forms.Label
        Me.txtMetaData = New System.Windows.Forms.TextBox
        Me.btnPlay = New System.Windows.Forms.Button
        Me.txtFilePath = New System.Windows.Forms.TextBox
        Me.lblFilePathHeader = New System.Windows.Forms.Label
        Me.txtIMDBID = New System.Windows.Forms.TextBox
        Me.lblIMDBHeader = New System.Windows.Forms.Label
        Me.lblDirector = New System.Windows.Forms.Label
        Me.lblDirectorHeader = New System.Windows.Forms.Label
        Me.pnlActors = New System.Windows.Forms.Panel
        Me.pbActLoad = New System.Windows.Forms.PictureBox
        Me.lstActors = New System.Windows.Forms.ListBox
        Me.pbActors = New System.Windows.Forms.PictureBox
        Me.lblActorsHeader = New System.Windows.Forms.Label
        Me.lblOutlineHeader = New System.Windows.Forms.Label
        Me.txtOutline = New System.Windows.Forms.TextBox
        Me.pnlTop250 = New System.Windows.Forms.Panel
        Me.lblTop250 = New System.Windows.Forms.Label
        Me.pbTop250 = New System.Windows.Forms.PictureBox
        Me.lblPlotHeader = New System.Windows.Forms.Label
        Me.txtPlot = New System.Windows.Forms.TextBox
        Me.btnDown = New System.Windows.Forms.Button
        Me.btnUp = New System.Windows.Forms.Button
        Me.lblInfoPanelHeader = New System.Windows.Forms.Label
        Me.pnlPoster = New System.Windows.Forms.Panel
        Me.pbPoster = New System.Windows.Forms.PictureBox
        Me.pbPosterCache = New System.Windows.Forms.PictureBox
        Me.pnlMPAA = New System.Windows.Forms.Panel
        Me.pbMPAA = New System.Windows.Forms.PictureBox
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.lblTitle = New System.Windows.Forms.Label
        Me.pnlInfoIcons = New System.Windows.Forms.Panel
        Me.pbVType = New System.Windows.Forms.PictureBox
        Me.pbStudio = New System.Windows.Forms.PictureBox
        Me.pbVideo = New System.Windows.Forms.PictureBox
        Me.pbAudio = New System.Windows.Forms.PictureBox
        Me.pbResolution = New System.Windows.Forms.PictureBox
        Me.pbChannels = New System.Windows.Forms.PictureBox
        Me.lblRuntime = New System.Windows.Forms.Label
        Me.lblTagline = New System.Windows.Forms.Label
        Me.pnlRating = New System.Windows.Forms.Panel
        Me.pbStar5 = New System.Windows.Forms.PictureBox
        Me.pbStar4 = New System.Windows.Forms.PictureBox
        Me.pbStar3 = New System.Windows.Forms.PictureBox
        Me.pbStar2 = New System.Windows.Forms.PictureBox
        Me.pbStar1 = New System.Windows.Forms.PictureBox
        Me.lblVotes = New System.Windows.Forms.Label
        Me.tsMain = New System.Windows.Forms.ToolStrip
        Me.tsbAutoPilot = New System.Windows.Forms.ToolStripDropDownButton
        Me.FullToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.FullAutoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAutoAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAutoNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAutoPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAutoFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAutoExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAutoTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAutoMI = New System.Windows.Forms.ToolStripMenuItem
        Me.FullAskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAskAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAskNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAskPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAskFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAskExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAskTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuAllAskMI = New System.Windows.Forms.ToolStripMenuItem
        Me.UpdateOnlyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.UpdateAutoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAutoAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAutoNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAutoPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAutoFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAutoExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAutoTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.UpdateAskToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAskAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAskNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAskPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAskFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAskExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMissAskTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.NewMoviesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AutomaticForceBestMatchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAutoAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAutoNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAutoPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAutoFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAutoExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAutoTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAutoMI = New System.Windows.Forms.ToolStripMenuItem
        Me.AskRequireInputToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAskAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAskNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAskPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAskFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAskExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAskTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuNewAskMI = New System.Windows.Forms.ToolStripMenuItem
        Me.MarkedMoviesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AutomaticForceBestMatchToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAutoAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAutoNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAutoPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAutoFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAutoExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAutoTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAutoMI = New System.Windows.Forms.ToolStripMenuItem
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAskAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAskNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAskPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAskFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAskExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAskTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuMarkAskMI = New System.Windows.Forms.ToolStripMenuItem
        Me.CurrentFilterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AutomaticForceBestMatchToolStripMenuItem2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAutoAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAutoNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAutoPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAutoFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAutoExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAutoTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAutoMI = New System.Windows.Forms.ToolStripMenuItem
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAskAll = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAskNfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAskPoster = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAskFanart = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAskExtra = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAskTrailer = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFilterAskMI = New System.Windows.Forms.ToolStripMenuItem
        Me.CustomUpdaterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbRefreshMedia = New System.Windows.Forms.ToolStripSplitButton
        Me.mnuMoviesUpdate = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuTVShowUpdate = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbUpdateXBMC = New System.Windows.Forms.ToolStripSplitButton
        Me.pbFanartCache = New System.Windows.Forms.PictureBox
        Me.pbFanart = New System.Windows.Forms.PictureBox
        Me.ilColumnIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.tmrWait = New System.Windows.Forms.Timer(Me.components)
        Me.tmrLoad = New System.Windows.Forms.Timer(Me.components)
        Me.tmrSearchWait = New System.Windows.Forms.Timer(Me.components)
        Me.tmrSearch = New System.Windows.Forms.Timer(Me.components)
        Me.tmrFilterAni = New System.Windows.Forms.Timer(Me.components)
        Me.ToolTips = New System.Windows.Forms.ToolTip(Me.components)
        Me.tmrWaitShow = New System.Windows.Forms.Timer(Me.components)
        Me.tmrLoadShow = New System.Windows.Forms.Timer(Me.components)
        Me.tmrWaitSeason = New System.Windows.Forms.Timer(Me.components)
        Me.tmrLoadSeason = New System.Windows.Forms.Timer(Me.components)
        Me.tmrWaitEp = New System.Windows.Forms.Timer(Me.components)
        Me.tmrLoadEp = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        Me.pnlFilterGenre.SuspendLayout()
        Me.pnlFilterSource.SuspendLayout()
        CType(Me.dgvMediaList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuMediaList.SuspendLayout()
        Me.scTV.Panel1.SuspendLayout()
        Me.scTV.Panel2.SuspendLayout()
        Me.scTV.SuspendLayout()
        CType(Me.dgvTVShows, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuShows.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        CType(Me.dgvTVSeasons, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvTVEpisodes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuEpisodes.SuspendLayout()
        Me.pnlListTop.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        CType(Me.picSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabsMain.SuspendLayout()
        Me.pnlFilter.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.gbSpecific.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.pnlCancel.SuspendLayout()
        Me.pnlNoInfo.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlInfoPanel.SuspendLayout()
        CType(Me.pbMILoading, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlActors.SuspendLayout()
        CType(Me.pbActLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbActors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTop250.SuspendLayout()
        CType(Me.pbTop250, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPoster.SuspendLayout()
        CType(Me.pbPoster, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPosterCache, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMPAA.SuspendLayout()
        CType(Me.pbMPAA, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTop.SuspendLayout()
        Me.pnlInfoIcons.SuspendLayout()
        CType(Me.pbVType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStudio, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbVideo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbResolution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbChannels, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlRating.SuspendLayout()
        CType(Me.pbStar5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tsMain.SuspendLayout()
        CType(Me.pbFanartCache, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbFanart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BottomToolStripPanel
        '
        Me.BottomToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.BottomToolStripPanel.Name = "BottomToolStripPanel"
        Me.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.BottomToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.BottomToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'TopToolStripPanel
        '
        Me.TopToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.TopToolStripPanel.Name = "TopToolStripPanel"
        Me.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.TopToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.TopToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'RightToolStripPanel
        '
        Me.RightToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.RightToolStripPanel.Name = "RightToolStripPanel"
        Me.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.RightToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.RightToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'LeftToolStripPanel
        '
        Me.LeftToolStripPanel.Location = New System.Drawing.Point(0, 0)
        Me.LeftToolStripPanel.Name = "LeftToolStripPanel"
        Me.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal
        Me.LeftToolStripPanel.RowMargin = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.LeftToolStripPanel.Size = New System.Drawing.Size(0, 0)
        '
        'ContentPanel
        '
        Me.ContentPanel.Size = New System.Drawing.Size(150, 175)
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Image = CType(resources.GetObject("ExitToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem, Me.ModuleSettingToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "&Edit"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Image = CType(resources.GetObject("SettingsToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.SettingsToolStripMenuItem.Text = "&Settings..."
        '
        'ModuleSettingToolStripMenuItem
        '
        Me.ModuleSettingToolStripMenuItem.Image = CType(resources.GetObject("ModuleSettingToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ModuleSettingToolStripMenuItem.Name = "ModuleSettingToolStripMenuItem"
        Me.ModuleSettingToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.ModuleSettingToolStripMenuItem.Text = "Module Setting"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Image = CType(resources.GetObject("AboutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.AboutToolStripMenuItem.Text = "&About..."
        '
        'StatusStrip
        '
        Me.StatusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslStatus, Me.tsSpring, Me.tslLoading, Me.tspbLoading})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 712)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1016, 22)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'tslStatus
        '
        Me.tslStatus.Name = "tslStatus"
        Me.tslStatus.Size = New System.Drawing.Size(0, 17)
        Me.tslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsSpring
        '
        Me.tsSpring.Name = "tsSpring"
        Me.tsSpring.Size = New System.Drawing.Size(1001, 17)
        Me.tsSpring.Spring = True
        Me.tsSpring.Text = "  "
        '
        'tslLoading
        '
        Me.tslLoading.AutoSize = False
        Me.tslLoading.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslLoading.Name = "tslLoading"
        Me.tslLoading.Size = New System.Drawing.Size(424, 17)
        Me.tslLoading.Text = "Loading Media:"
        Me.tslLoading.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tslLoading.Visible = False
        '
        'tspbLoading
        '
        Me.tspbLoading.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tspbLoading.AutoSize = False
        Me.tspbLoading.Name = "tspbLoading"
        Me.tspbLoading.Size = New System.Drawing.Size(150, 16)
        Me.tspbLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.tspbLoading.Visible = False
        '
        'tmrAni
        '
        Me.tmrAni.Interval = 1
        '
        'MenuStrip
        '
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem, Me.DonateToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1016, 24)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CleanFoldersToolStripMenuItem, Me.ConvertFileSourceToFolderSourceToolStripMenuItem, Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem, Me.RenamerToolStripMenuItem, Me.ToolStripSeparator4, Me.SetsManagerToolStripMenuItem, Me.OfflineMediaManagerToolStripMenuItem, Me.ToolStripMenuItem3, Me.ExportMoviesListToolStripMenuItem, Me.ToolStripSeparator5, Me.ClearAllCachesToolStripMenuItem, Me.RefreshAllMoviesToolStripMenuItem, Me.CleanDatabaseToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ToolsToolStripMenuItem.Text = "&Tools"
        '
        'CleanFoldersToolStripMenuItem
        '
        Me.CleanFoldersToolStripMenuItem.Image = CType(resources.GetObject("CleanFoldersToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CleanFoldersToolStripMenuItem.Name = "CleanFoldersToolStripMenuItem"
        Me.CleanFoldersToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.CleanFoldersToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.CleanFoldersToolStripMenuItem.Text = "&Clean Files"
        '
        'ConvertFileSourceToFolderSourceToolStripMenuItem
        '
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Image = CType(resources.GetObject("ConvertFileSourceToFolderSourceToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Name = "ConvertFileSourceToFolderSourceToolStripMenuItem"
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Text = "&Sort Files Into Folders"
        '
        'CopyExistingFanartToBackdropsFolderToolStripMenuItem
        '
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Image = CType(resources.GetObject("CopyExistingFanartToBackdropsFolderToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Name = "CopyExistingFanartToBackdropsFolderToolStripMenuItem"
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.B), System.Windows.Forms.Keys)
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Text = "Copy Existing Fanart To &Backdrops Folder"
        '
        'RenamerToolStripMenuItem
        '
        Me.RenamerToolStripMenuItem.Image = CType(resources.GetObject("RenamerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RenamerToolStripMenuItem.Name = "RenamerToolStripMenuItem"
        Me.RenamerToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.RenamerToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.RenamerToolStripMenuItem.Text = "Bulk &Renamer"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(353, 6)
        '
        'SetsManagerToolStripMenuItem
        '
        Me.SetsManagerToolStripMenuItem.Image = CType(resources.GetObject("SetsManagerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SetsManagerToolStripMenuItem.Name = "SetsManagerToolStripMenuItem"
        Me.SetsManagerToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.SetsManagerToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.SetsManagerToolStripMenuItem.Text = "Sets &Manager"
        '
        'OfflineMediaManagerToolStripMenuItem
        '
        Me.OfflineMediaManagerToolStripMenuItem.Image = CType(resources.GetObject("OfflineMediaManagerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OfflineMediaManagerToolStripMenuItem.Name = "OfflineMediaManagerToolStripMenuItem"
        Me.OfflineMediaManagerToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OfflineMediaManagerToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.OfflineMediaManagerToolStripMenuItem.Text = "&Offline Media Manager"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(353, 6)
        '
        'ExportMoviesListToolStripMenuItem
        '
        Me.ExportMoviesListToolStripMenuItem.Image = CType(resources.GetObject("ExportMoviesListToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExportMoviesListToolStripMenuItem.Name = "ExportMoviesListToolStripMenuItem"
        Me.ExportMoviesListToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.ExportMoviesListToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.ExportMoviesListToolStripMenuItem.Text = "&Export Movies List"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(353, 6)
        '
        'ClearAllCachesToolStripMenuItem
        '
        Me.ClearAllCachesToolStripMenuItem.Image = CType(resources.GetObject("ClearAllCachesToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ClearAllCachesToolStripMenuItem.Name = "ClearAllCachesToolStripMenuItem"
        Me.ClearAllCachesToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.ClearAllCachesToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.ClearAllCachesToolStripMenuItem.Text = "Clear &All Caches"
        '
        'RefreshAllMoviesToolStripMenuItem
        '
        Me.RefreshAllMoviesToolStripMenuItem.Image = CType(resources.GetObject("RefreshAllMoviesToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RefreshAllMoviesToolStripMenuItem.Name = "RefreshAllMoviesToolStripMenuItem"
        Me.RefreshAllMoviesToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.RefreshAllMoviesToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.RefreshAllMoviesToolStripMenuItem.Text = "Re&load All Movies"
        '
        'CleanDatabaseToolStripMenuItem
        '
        Me.CleanDatabaseToolStripMenuItem.Image = CType(resources.GetObject("CleanDatabaseToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CleanDatabaseToolStripMenuItem.Name = "CleanDatabaseToolStripMenuItem"
        Me.CleanDatabaseToolStripMenuItem.ShortcutKeys = CType(((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.Alt) _
                    Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.CleanDatabaseToolStripMenuItem.Size = New System.Drawing.Size(356, 22)
        Me.CleanDatabaseToolStripMenuItem.Text = "Clean Database"
        '
        'DonateToolStripMenuItem
        '
        Me.DonateToolStripMenuItem.Image = CType(resources.GetObject("DonateToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DonateToolStripMenuItem.Name = "DonateToolStripMenuItem"
        Me.DonateToolStripMenuItem.Size = New System.Drawing.Size(73, 20)
        Me.DonateToolStripMenuItem.Text = "Donate"
        '
        'scMain
        '
        Me.scMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.scMain.Location = New System.Drawing.Point(0, 24)
        Me.scMain.Name = "scMain"
        '
        'scMain.Panel1
        '
        Me.scMain.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.scMain.Panel1.Controls.Add(Me.pnlFilterGenre)
        Me.scMain.Panel1.Controls.Add(Me.pnlFilterSource)
        Me.scMain.Panel1.Controls.Add(Me.dgvMediaList)
        Me.scMain.Panel1.Controls.Add(Me.scTV)
        Me.scMain.Panel1.Controls.Add(Me.pnlListTop)
        Me.scMain.Panel1.Controls.Add(Me.pnlFilter)
        Me.scMain.Panel1.Margin = New System.Windows.Forms.Padding(3)
        Me.scMain.Panel1MinSize = 165
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.BackColor = System.Drawing.Color.DimGray
        Me.scMain.Panel2.Controls.Add(Me.pnlCancel)
        Me.scMain.Panel2.Controls.Add(Me.pnlNoInfo)
        Me.scMain.Panel2.Controls.Add(Me.pnlInfoPanel)
        Me.scMain.Panel2.Controls.Add(Me.pnlPoster)
        Me.scMain.Panel2.Controls.Add(Me.pbPosterCache)
        Me.scMain.Panel2.Controls.Add(Me.pnlMPAA)
        Me.scMain.Panel2.Controls.Add(Me.pnlTop)
        Me.scMain.Panel2.Controls.Add(Me.tsMain)
        Me.scMain.Panel2.Controls.Add(Me.pbFanartCache)
        Me.scMain.Panel2.Controls.Add(Me.pbFanart)
        Me.scMain.Panel2.Margin = New System.Windows.Forms.Padding(3)
        Me.scMain.Size = New System.Drawing.Size(1016, 688)
        Me.scMain.SplitterDistance = 349
        Me.scMain.TabIndex = 7
        Me.scMain.TabStop = False
        '
        'pnlFilterGenre
        '
        Me.pnlFilterGenre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlFilterGenre.Controls.Add(Me.lblGFilClose)
        Me.pnlFilterGenre.Controls.Add(Me.Label4)
        Me.pnlFilterGenre.Controls.Add(Me.clbFilterGenres)
        Me.pnlFilterGenre.Location = New System.Drawing.Point(186, 426)
        Me.pnlFilterGenre.Name = "pnlFilterGenre"
        Me.pnlFilterGenre.Size = New System.Drawing.Size(166, 162)
        Me.pnlFilterGenre.TabIndex = 15
        Me.pnlFilterGenre.Visible = False
        '
        'lblGFilClose
        '
        Me.lblGFilClose.AutoSize = True
        Me.lblGFilClose.BackColor = System.Drawing.Color.DimGray
        Me.lblGFilClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblGFilClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGFilClose.ForeColor = System.Drawing.Color.White
        Me.lblGFilClose.Location = New System.Drawing.Point(130, 2)
        Me.lblGFilClose.Name = "lblGFilClose"
        Me.lblGFilClose.Size = New System.Drawing.Size(33, 13)
        Me.lblGFilClose.TabIndex = 24
        Me.lblGFilClose.Text = "Close"
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label4.Location = New System.Drawing.Point(1, 1)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(162, 17)
        Me.Label4.TabIndex = 23
        Me.Label4.Text = "Genres"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'clbFilterGenres
        '
        Me.clbFilterGenres.CheckOnClick = True
        Me.clbFilterGenres.FormattingEnabled = True
        Me.clbFilterGenres.Location = New System.Drawing.Point(1, 20)
        Me.clbFilterGenres.Name = "clbFilterGenres"
        Me.clbFilterGenres.Size = New System.Drawing.Size(162, 139)
        Me.clbFilterGenres.TabIndex = 8
        Me.clbFilterGenres.TabStop = False
        '
        'pnlFilterSource
        '
        Me.pnlFilterSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlFilterSource.Controls.Add(Me.lblSFilClose)
        Me.pnlFilterSource.Controls.Add(Me.Label8)
        Me.pnlFilterSource.Controls.Add(Me.clbFilterSource)
        Me.pnlFilterSource.Location = New System.Drawing.Point(186, 497)
        Me.pnlFilterSource.Name = "pnlFilterSource"
        Me.pnlFilterSource.Size = New System.Drawing.Size(166, 162)
        Me.pnlFilterSource.TabIndex = 16
        Me.pnlFilterSource.Visible = False
        '
        'lblSFilClose
        '
        Me.lblSFilClose.AutoSize = True
        Me.lblSFilClose.BackColor = System.Drawing.Color.DimGray
        Me.lblSFilClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblSFilClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSFilClose.ForeColor = System.Drawing.Color.White
        Me.lblSFilClose.Location = New System.Drawing.Point(130, 2)
        Me.lblSFilClose.Name = "lblSFilClose"
        Me.lblSFilClose.Size = New System.Drawing.Size(33, 13)
        Me.lblSFilClose.TabIndex = 24
        Me.lblSFilClose.Text = "Close"
        '
        'Label8
        '
        Me.Label8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.Label8.Location = New System.Drawing.Point(1, 1)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(162, 17)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "Sources"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'clbFilterSource
        '
        Me.clbFilterSource.CheckOnClick = True
        Me.clbFilterSource.FormattingEnabled = True
        Me.clbFilterSource.Location = New System.Drawing.Point(1, 20)
        Me.clbFilterSource.Name = "clbFilterSource"
        Me.clbFilterSource.Size = New System.Drawing.Size(162, 139)
        Me.clbFilterSource.TabIndex = 8
        Me.clbFilterSource.TabStop = False
        '
        'dgvMediaList
        '
        Me.dgvMediaList.AllowUserToAddRows = False
        Me.dgvMediaList.AllowUserToDeleteRows = False
        Me.dgvMediaList.AllowUserToResizeRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.dgvMediaList.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvMediaList.BackgroundColor = System.Drawing.Color.White
        Me.dgvMediaList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvMediaList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvMediaList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.dgvMediaList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMediaList.ContextMenuStrip = Me.mnuMediaList
        Me.dgvMediaList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvMediaList.GridColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.dgvMediaList.Location = New System.Drawing.Point(0, 56)
        Me.dgvMediaList.Name = "dgvMediaList"
        Me.dgvMediaList.ReadOnly = True
        Me.dgvMediaList.RowHeadersVisible = False
        Me.dgvMediaList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMediaList.ShowCellErrors = False
        Me.dgvMediaList.ShowRowErrors = False
        Me.dgvMediaList.Size = New System.Drawing.Size(349, 452)
        Me.dgvMediaList.StandardTab = True
        Me.dgvMediaList.TabIndex = 1
        '
        'mnuMediaList
        '
        Me.mnuMediaList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuTitle, Me.ToolStripSeparator3, Me.cmnuRefresh, Me.cmnuMark, Me.cmnuLock, Me.ToolStripMenuItem1, Me.cmnuEditMovie, Me.cmnuMetaData, Me.GenresToolStripMenuItem, Me.cmnuSep, Me.cmnuRescrape, Me.cmnuSearchNew, Me.cmnuSep2, Me.OpenContainingFolderToolStripMenuItem, Me.ToolStripSeparator2, Me.cmuRenamer, Me.ToolStripSeparator1, Me.RemoveToolStripMenuItem})
        Me.mnuMediaList.Name = "mnuMediaList"
        Me.mnuMediaList.Size = New System.Drawing.Size(245, 304)
        '
        'cmnuTitle
        '
        Me.cmnuTitle.Enabled = False
        Me.cmnuTitle.Image = CType(resources.GetObject("cmnuTitle.Image"), System.Drawing.Image)
        Me.cmnuTitle.Name = "cmnuTitle"
        Me.cmnuTitle.Size = New System.Drawing.Size(244, 22)
        Me.cmnuTitle.Text = "Title"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(241, 6)
        '
        'cmnuRefresh
        '
        Me.cmnuRefresh.Image = CType(resources.GetObject("cmnuRefresh.Image"), System.Drawing.Image)
        Me.cmnuRefresh.Name = "cmnuRefresh"
        Me.cmnuRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5
        Me.cmnuRefresh.Size = New System.Drawing.Size(244, 22)
        Me.cmnuRefresh.Text = "Reload"
        '
        'cmnuMark
        '
        Me.cmnuMark.Image = CType(resources.GetObject("cmnuMark.Image"), System.Drawing.Image)
        Me.cmnuMark.Name = "cmnuMark"
        Me.cmnuMark.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.M), System.Windows.Forms.Keys)
        Me.cmnuMark.Size = New System.Drawing.Size(244, 22)
        Me.cmnuMark.Text = "Mark"
        '
        'cmnuLock
        '
        Me.cmnuLock.Image = CType(resources.GetObject("cmnuLock.Image"), System.Drawing.Image)
        Me.cmnuLock.Name = "cmnuLock"
        Me.cmnuLock.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.L), System.Windows.Forms.Keys)
        Me.cmnuLock.Size = New System.Drawing.Size(244, 22)
        Me.cmnuLock.Text = "Lock"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(241, 6)
        '
        'cmnuEditMovie
        '
        Me.cmnuEditMovie.Image = CType(resources.GetObject("cmnuEditMovie.Image"), System.Drawing.Image)
        Me.cmnuEditMovie.Name = "cmnuEditMovie"
        Me.cmnuEditMovie.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.E), System.Windows.Forms.Keys)
        Me.cmnuEditMovie.Size = New System.Drawing.Size(244, 22)
        Me.cmnuEditMovie.Text = "Edit Movie"
        '
        'cmnuMetaData
        '
        Me.cmnuMetaData.Image = CType(resources.GetObject("cmnuMetaData.Image"), System.Drawing.Image)
        Me.cmnuMetaData.Name = "cmnuMetaData"
        Me.cmnuMetaData.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.D), System.Windows.Forms.Keys)
        Me.cmnuMetaData.Size = New System.Drawing.Size(244, 22)
        Me.cmnuMetaData.Text = "Edit Meta Data"
        '
        'GenresToolStripMenuItem
        '
        Me.GenresToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LblGenreStripMenuItem2, Me.GenreListToolStripComboBox, Me.AddGenreToolStripMenuItem, Me.SetGenreToolStripMenuItem, Me.RemoveGenreToolStripMenuItem})
        Me.GenresToolStripMenuItem.Image = CType(resources.GetObject("GenresToolStripMenuItem.Image"), System.Drawing.Image)
        Me.GenresToolStripMenuItem.Name = "GenresToolStripMenuItem"
        Me.GenresToolStripMenuItem.Size = New System.Drawing.Size(244, 22)
        Me.GenresToolStripMenuItem.Text = "Genres"
        '
        'LblGenreStripMenuItem2
        '
        Me.LblGenreStripMenuItem2.Enabled = False
        Me.LblGenreStripMenuItem2.Name = "LblGenreStripMenuItem2"
        Me.LblGenreStripMenuItem2.Size = New System.Drawing.Size(195, 22)
        Me.LblGenreStripMenuItem2.Text = ">> Select Genre <<"
        '
        'GenreListToolStripComboBox
        '
        Me.GenreListToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.GenreListToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.GenreListToolStripComboBox.Name = "GenreListToolStripComboBox"
        Me.GenreListToolStripComboBox.Size = New System.Drawing.Size(135, 23)
        Me.GenreListToolStripComboBox.Sorted = True
        '
        'AddGenreToolStripMenuItem
        '
        Me.AddGenreToolStripMenuItem.Name = "AddGenreToolStripMenuItem"
        Me.AddGenreToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.AddGenreToolStripMenuItem.Text = "Add"
        '
        'SetGenreToolStripMenuItem
        '
        Me.SetGenreToolStripMenuItem.Name = "SetGenreToolStripMenuItem"
        Me.SetGenreToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.SetGenreToolStripMenuItem.Text = "Set"
        '
        'RemoveGenreToolStripMenuItem
        '
        Me.RemoveGenreToolStripMenuItem.Name = "RemoveGenreToolStripMenuItem"
        Me.RemoveGenreToolStripMenuItem.Size = New System.Drawing.Size(195, 22)
        Me.RemoveGenreToolStripMenuItem.Text = "Remove"
        '
        'cmnuSep
        '
        Me.cmnuSep.Name = "cmnuSep"
        Me.cmnuSep.Size = New System.Drawing.Size(241, 6)
        '
        'cmnuRescrape
        '
        Me.cmnuRescrape.Image = CType(resources.GetObject("cmnuRescrape.Image"), System.Drawing.Image)
        Me.cmnuRescrape.Name = "cmnuRescrape"
        Me.cmnuRescrape.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.I), System.Windows.Forms.Keys)
        Me.cmnuRescrape.Size = New System.Drawing.Size(244, 22)
        Me.cmnuRescrape.Text = "Re-scrape IMDB"
        '
        'cmnuSearchNew
        '
        Me.cmnuSearchNew.Image = CType(resources.GetObject("cmnuSearchNew.Image"), System.Drawing.Image)
        Me.cmnuSearchNew.Name = "cmnuSearchNew"
        Me.cmnuSearchNew.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.C), System.Windows.Forms.Keys)
        Me.cmnuSearchNew.Size = New System.Drawing.Size(244, 22)
        Me.cmnuSearchNew.Text = "Change Movie"
        '
        'cmnuSep2
        '
        Me.cmnuSep2.Name = "cmnuSep2"
        Me.cmnuSep2.Size = New System.Drawing.Size(241, 6)
        '
        'OpenContainingFolderToolStripMenuItem
        '
        Me.OpenContainingFolderToolStripMenuItem.Image = CType(resources.GetObject("OpenContainingFolderToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OpenContainingFolderToolStripMenuItem.Name = "OpenContainingFolderToolStripMenuItem"
        Me.OpenContainingFolderToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.OpenContainingFolderToolStripMenuItem.Size = New System.Drawing.Size(244, 22)
        Me.OpenContainingFolderToolStripMenuItem.Text = "Open Containing Folder"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(241, 6)
        '
        'cmuRenamer
        '
        Me.cmuRenamer.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuRenameAuto, Me.cmnuRenameManual})
        Me.cmuRenamer.Image = CType(resources.GetObject("cmuRenamer.Image"), System.Drawing.Image)
        Me.cmuRenamer.Name = "cmuRenamer"
        Me.cmuRenamer.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.R), System.Windows.Forms.Keys)
        Me.cmuRenamer.Size = New System.Drawing.Size(244, 22)
        Me.cmuRenamer.Text = "Rename"
        '
        'cmnuRenameAuto
        '
        Me.cmnuRenameAuto.Name = "cmnuRenameAuto"
        Me.cmnuRenameAuto.Size = New System.Drawing.Size(114, 22)
        Me.cmnuRenameAuto.Text = "Auto"
        '
        'cmnuRenameManual
        '
        Me.cmnuRenameManual.Name = "cmnuRenameManual"
        Me.cmnuRenameManual.Size = New System.Drawing.Size(114, 22)
        Me.cmnuRenameManual.Text = "Manual"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(241, 6)
        '
        'RemoveToolStripMenuItem
        '
        Me.RemoveToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveFromDatabaseToolStripMenuItem, Me.DeleteMovieToolStripMenuItem})
        Me.RemoveToolStripMenuItem.Image = CType(resources.GetObject("RemoveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem"
        Me.RemoveToolStripMenuItem.Size = New System.Drawing.Size(244, 22)
        Me.RemoveToolStripMenuItem.Text = "Remove"
        '
        'RemoveFromDatabaseToolStripMenuItem
        '
        Me.RemoveFromDatabaseToolStripMenuItem.Image = CType(resources.GetObject("RemoveFromDatabaseToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RemoveFromDatabaseToolStripMenuItem.Name = "RemoveFromDatabaseToolStripMenuItem"
        Me.RemoveFromDatabaseToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.V), System.Windows.Forms.Keys)
        Me.RemoveFromDatabaseToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.RemoveFromDatabaseToolStripMenuItem.Text = "Remove from Database"
        '
        'DeleteMovieToolStripMenuItem
        '
        Me.DeleteMovieToolStripMenuItem.Image = CType(resources.GetObject("DeleteMovieToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteMovieToolStripMenuItem.Name = "DeleteMovieToolStripMenuItem"
        Me.DeleteMovieToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.DeleteMovieToolStripMenuItem.Size = New System.Drawing.Size(238, 22)
        Me.DeleteMovieToolStripMenuItem.Text = "Delete Movie"
        '
        'scTV
        '
        Me.scTV.BackColor = System.Drawing.Color.Gainsboro
        Me.scTV.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scTV.Location = New System.Drawing.Point(0, 56)
        Me.scTV.Name = "scTV"
        Me.scTV.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scTV.Panel1
        '
        Me.scTV.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.scTV.Panel1.Controls.Add(Me.dgvTVShows)
        '
        'scTV.Panel2
        '
        Me.scTV.Panel2.Controls.Add(Me.SplitContainer2)
        Me.scTV.Size = New System.Drawing.Size(349, 452)
        Me.scTV.SplitterDistance = 116
        Me.scTV.TabIndex = 3
        '
        'dgvTVShows
        '
        Me.dgvTVShows.AllowUserToAddRows = False
        Me.dgvTVShows.AllowUserToDeleteRows = False
        Me.dgvTVShows.AllowUserToResizeRows = False
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.dgvTVShows.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvTVShows.BackgroundColor = System.Drawing.Color.White
        Me.dgvTVShows.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvTVShows.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvTVShows.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.dgvTVShows.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTVShows.ContextMenuStrip = Me.mnuShows
        Me.dgvTVShows.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTVShows.GridColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.dgvTVShows.Location = New System.Drawing.Point(0, 0)
        Me.dgvTVShows.Name = "dgvTVShows"
        Me.dgvTVShows.ReadOnly = True
        Me.dgvTVShows.RowHeadersVisible = False
        Me.dgvTVShows.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTVShows.ShowCellErrors = False
        Me.dgvTVShows.ShowRowErrors = False
        Me.dgvTVShows.Size = New System.Drawing.Size(349, 116)
        Me.dgvTVShows.StandardTab = True
        Me.dgvTVShows.TabIndex = 0
        '
        'mnuShows
        '
        Me.mnuShows.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuShowTitle, Me.ToolStripMenuItem2, Me.cmnuEditShow, Me.ToolStripSeparator7, Me.cmnuRescrapeShow, Me.cmnuChangeShow})
        Me.mnuShows.Name = "mnuShows"
        Me.mnuShows.Size = New System.Drawing.Size(176, 104)
        '
        'cmnuShowTitle
        '
        Me.cmnuShowTitle.Enabled = False
        Me.cmnuShowTitle.Image = CType(resources.GetObject("cmnuShowTitle.Image"), System.Drawing.Image)
        Me.cmnuShowTitle.Name = "cmnuShowTitle"
        Me.cmnuShowTitle.Size = New System.Drawing.Size(175, 22)
        Me.cmnuShowTitle.Text = "Title"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(172, 6)
        '
        'cmnuEditShow
        '
        Me.cmnuEditShow.Image = CType(resources.GetObject("cmnuEditShow.Image"), System.Drawing.Image)
        Me.cmnuEditShow.Name = "cmnuEditShow"
        Me.cmnuEditShow.Size = New System.Drawing.Size(175, 22)
        Me.cmnuEditShow.Text = "Edit Show"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(172, 6)
        '
        'cmnuRescrapeShow
        '
        Me.cmnuRescrapeShow.Image = CType(resources.GetObject("cmnuRescrapeShow.Image"), System.Drawing.Image)
        Me.cmnuRescrapeShow.Name = "cmnuRescrapeShow"
        Me.cmnuRescrapeShow.Size = New System.Drawing.Size(175, 22)
        Me.cmnuRescrapeShow.Text = "Re-scrape theTVDB"
        '
        'cmnuChangeShow
        '
        Me.cmnuChangeShow.Image = CType(resources.GetObject("cmnuChangeShow.Image"), System.Drawing.Image)
        Me.cmnuChangeShow.Name = "cmnuChangeShow"
        Me.cmnuChangeShow.Size = New System.Drawing.Size(175, 22)
        Me.cmnuChangeShow.Text = "Change Show"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.SplitContainer2.Panel1.Controls.Add(Me.dgvTVSeasons)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.dgvTVEpisodes)
        Me.SplitContainer2.Size = New System.Drawing.Size(349, 332)
        Me.SplitContainer2.SplitterDistance = 116
        Me.SplitContainer2.TabIndex = 0
        '
        'dgvTVSeasons
        '
        Me.dgvTVSeasons.AllowUserToAddRows = False
        Me.dgvTVSeasons.AllowUserToDeleteRows = False
        Me.dgvTVSeasons.AllowUserToResizeRows = False
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.dgvTVSeasons.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvTVSeasons.BackgroundColor = System.Drawing.Color.White
        Me.dgvTVSeasons.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvTVSeasons.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvTVSeasons.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.dgvTVSeasons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTVSeasons.ContextMenuStrip = Me.mnuSeasons
        Me.dgvTVSeasons.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTVSeasons.GridColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.dgvTVSeasons.Location = New System.Drawing.Point(0, 0)
        Me.dgvTVSeasons.Name = "dgvTVSeasons"
        Me.dgvTVSeasons.ReadOnly = True
        Me.dgvTVSeasons.RowHeadersVisible = False
        Me.dgvTVSeasons.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTVSeasons.ShowCellErrors = False
        Me.dgvTVSeasons.ShowRowErrors = False
        Me.dgvTVSeasons.Size = New System.Drawing.Size(349, 116)
        Me.dgvTVSeasons.StandardTab = True
        Me.dgvTVSeasons.TabIndex = 0
        '
        'mnuSeasons
        '
        Me.mnuSeasons.Name = "mnuSeasons"
        Me.mnuSeasons.Size = New System.Drawing.Size(61, 4)
        '
        'dgvTVEpisodes
        '
        Me.dgvTVEpisodes.AllowUserToAddRows = False
        Me.dgvTVEpisodes.AllowUserToDeleteRows = False
        Me.dgvTVEpisodes.AllowUserToResizeRows = False
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.dgvTVEpisodes.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvTVEpisodes.BackgroundColor = System.Drawing.Color.White
        Me.dgvTVEpisodes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvTVEpisodes.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.dgvTVEpisodes.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable
        Me.dgvTVEpisodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTVEpisodes.ContextMenuStrip = Me.mnuEpisodes
        Me.dgvTVEpisodes.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvTVEpisodes.GridColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.dgvTVEpisodes.Location = New System.Drawing.Point(0, 0)
        Me.dgvTVEpisodes.Name = "dgvTVEpisodes"
        Me.dgvTVEpisodes.ReadOnly = True
        Me.dgvTVEpisodes.RowHeadersVisible = False
        Me.dgvTVEpisodes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTVEpisodes.ShowCellErrors = False
        Me.dgvTVEpisodes.ShowRowErrors = False
        Me.dgvTVEpisodes.Size = New System.Drawing.Size(349, 212)
        Me.dgvTVEpisodes.StandardTab = True
        Me.dgvTVEpisodes.TabIndex = 0
        '
        'mnuEpisodes
        '
        Me.mnuEpisodes.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuEpTitle, Me.ToolStripSeparator6, Me.cmnuEditEpisode})
        Me.mnuEpisodes.Name = "mnuEpisodes"
        Me.mnuEpisodes.Size = New System.Drawing.Size(139, 54)
        '
        'cmnuEpTitle
        '
        Me.cmnuEpTitle.Enabled = False
        Me.cmnuEpTitle.Image = CType(resources.GetObject("cmnuEpTitle.Image"), System.Drawing.Image)
        Me.cmnuEpTitle.Name = "cmnuEpTitle"
        Me.cmnuEpTitle.Size = New System.Drawing.Size(138, 22)
        Me.cmnuEpTitle.Text = "Title"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(135, 6)
        '
        'cmnuEditEpisode
        '
        Me.cmnuEditEpisode.Image = CType(resources.GetObject("cmnuEditEpisode.Image"), System.Drawing.Image)
        Me.cmnuEditEpisode.Name = "cmnuEditEpisode"
        Me.cmnuEditEpisode.Size = New System.Drawing.Size(138, 22)
        Me.cmnuEditEpisode.Text = "Edit Episode"
        '
        'pnlListTop
        '
        Me.pnlListTop.Controls.Add(Me.btnMarkAll)
        Me.pnlListTop.Controls.Add(Me.pnlSearch)
        Me.pnlListTop.Controls.Add(Me.tabsMain)
        Me.pnlListTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlListTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlListTop.Name = "pnlListTop"
        Me.pnlListTop.Size = New System.Drawing.Size(349, 56)
        Me.pnlListTop.TabIndex = 14
        '
        'btnMarkAll
        '
        Me.btnMarkAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMarkAll.Image = CType(resources.GetObject("btnMarkAll.Image"), System.Drawing.Image)
        Me.btnMarkAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMarkAll.Location = New System.Drawing.Point(252, 1)
        Me.btnMarkAll.Name = "btnMarkAll"
        Me.btnMarkAll.Size = New System.Drawing.Size(96, 21)
        Me.btnMarkAll.TabIndex = 5
        Me.btnMarkAll.Text = "Mark All"
        Me.btnMarkAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMarkAll.UseVisualStyleBackColor = True
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSearch.Controls.Add(Me.cbSearch)
        Me.pnlSearch.Controls.Add(Me.picSearch)
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(0, 23)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(349, 33)
        Me.pnlSearch.TabIndex = 11
        '
        'cbSearch
        '
        Me.cbSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSearch.FormattingEnabled = True
        Me.cbSearch.Location = New System.Drawing.Point(238, 5)
        Me.cbSearch.Name = "cbSearch"
        Me.cbSearch.Size = New System.Drawing.Size(83, 21)
        Me.cbSearch.TabIndex = 7
        '
        'picSearch
        '
        Me.picSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picSearch.Image = CType(resources.GetObject("picSearch.Image"), System.Drawing.Image)
        Me.picSearch.Location = New System.Drawing.Point(325, 8)
        Me.picSearch.Name = "picSearch"
        Me.picSearch.Size = New System.Drawing.Size(16, 16)
        Me.picSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.picSearch.TabIndex = 1
        Me.picSearch.TabStop = False
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSearch.Location = New System.Drawing.Point(7, 6)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(227, 20)
        Me.txtSearch.TabIndex = 6
        '
        'tabsMain
        '
        Me.tabsMain.Controls.Add(Me.tabMovies)
        Me.tabsMain.Controls.Add(Me.tabTV)
        Me.tabsMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.tabsMain.Location = New System.Drawing.Point(0, 0)
        Me.tabsMain.Name = "tabsMain"
        Me.tabsMain.SelectedIndex = 0
        Me.tabsMain.Size = New System.Drawing.Size(349, 29)
        Me.tabsMain.TabIndex = 8
        Me.tabsMain.TabStop = False
        '
        'tabMovies
        '
        Me.tabMovies.Location = New System.Drawing.Point(4, 22)
        Me.tabMovies.Name = "tabMovies"
        Me.tabMovies.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMovies.Size = New System.Drawing.Size(341, 3)
        Me.tabMovies.TabIndex = 0
        Me.tabMovies.Text = "Movies"
        Me.tabMovies.UseVisualStyleBackColor = True
        '
        'tabTV
        '
        Me.tabTV.Location = New System.Drawing.Point(4, 22)
        Me.tabTV.Name = "tabTV"
        Me.tabTV.Padding = New System.Windows.Forms.Padding(3)
        Me.tabTV.Size = New System.Drawing.Size(341, 3)
        Me.tabTV.TabIndex = 1
        Me.tabTV.Text = "TV Shows"
        Me.tabTV.UseVisualStyleBackColor = True
        '
        'pnlFilter
        '
        Me.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlFilter.Controls.Add(Me.GroupBox1)
        Me.pnlFilter.Controls.Add(Me.btnClearFilters)
        Me.pnlFilter.Controls.Add(Me.GroupBox3)
        Me.pnlFilter.Controls.Add(Me.gbSpecific)
        Me.pnlFilter.Controls.Add(Me.btnFilterDown)
        Me.pnlFilter.Controls.Add(Me.btnFilterUp)
        Me.pnlFilter.Controls.Add(Me.lblFilter)
        Me.pnlFilter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFilter.Location = New System.Drawing.Point(0, 508)
        Me.pnlFilter.Name = "pnlFilter"
        Me.pnlFilter.Size = New System.Drawing.Size(349, 180)
        Me.pnlFilter.TabIndex = 12
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnIMDBRating)
        Me.GroupBox1.Controls.Add(Me.btnSortTitle)
        Me.GroupBox1.Controls.Add(Me.btnSortDate)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 81)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(131, 76)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Extra Sorting"
        '
        'btnIMDBRating
        '
        Me.btnIMDBRating.Image = Global.Ember_Media_Manager.My.Resources.Resources.desc
        Me.btnIMDBRating.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnIMDBRating.Location = New System.Drawing.Point(7, 53)
        Me.btnIMDBRating.Name = "btnIMDBRating"
        Me.btnIMDBRating.Size = New System.Drawing.Size(117, 21)
        Me.btnIMDBRating.TabIndex = 2
        Me.btnIMDBRating.Text = "IMDB Rating"
        Me.btnIMDBRating.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnIMDBRating.UseVisualStyleBackColor = True
        '
        'btnSortTitle
        '
        Me.btnSortTitle.Image = Global.Ember_Media_Manager.My.Resources.Resources.desc
        Me.btnSortTitle.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSortTitle.Location = New System.Drawing.Point(7, 33)
        Me.btnSortTitle.Name = "btnSortTitle"
        Me.btnSortTitle.Size = New System.Drawing.Size(117, 21)
        Me.btnSortTitle.TabIndex = 1
        Me.btnSortTitle.Text = "Sort Title"
        Me.btnSortTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSortTitle.UseVisualStyleBackColor = True
        '
        'btnSortDate
        '
        Me.btnSortDate.Image = Global.Ember_Media_Manager.My.Resources.Resources.desc
        Me.btnSortDate.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSortDate.Location = New System.Drawing.Point(7, 13)
        Me.btnSortDate.Name = "btnSortDate"
        Me.btnSortDate.Size = New System.Drawing.Size(117, 21)
        Me.btnSortDate.TabIndex = 0
        Me.btnSortDate.Text = "Date Added"
        Me.btnSortDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSortDate.UseVisualStyleBackColor = True
        '
        'btnClearFilters
        '
        Me.btnClearFilters.Image = CType(resources.GetObject("btnClearFilters.Image"), System.Drawing.Image)
        Me.btnClearFilters.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnClearFilters.Location = New System.Drawing.Point(22, 160)
        Me.btnClearFilters.Name = "btnClearFilters"
        Me.btnClearFilters.Size = New System.Drawing.Size(92, 20)
        Me.btnClearFilters.TabIndex = 4
        Me.btnClearFilters.Text = "Clear Filters"
        Me.btnClearFilters.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnClearFilters.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chkFilterTolerance)
        Me.GroupBox3.Controls.Add(Me.chkFilterMissing)
        Me.GroupBox3.Controls.Add(Me.chkFilterDupe)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 22)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(131, 59)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "General"
        '
        'chkFilterTolerance
        '
        Me.chkFilterTolerance.AutoSize = True
        Me.chkFilterTolerance.Location = New System.Drawing.Point(7, 41)
        Me.chkFilterTolerance.Name = "chkFilterTolerance"
        Me.chkFilterTolerance.Size = New System.Drawing.Size(106, 17)
        Me.chkFilterTolerance.TabIndex = 2
        Me.chkFilterTolerance.Text = "Out of Tolerance"
        Me.chkFilterTolerance.UseVisualStyleBackColor = True
        '
        'chkFilterMissing
        '
        Me.chkFilterMissing.AutoSize = True
        Me.chkFilterMissing.Location = New System.Drawing.Point(7, 27)
        Me.chkFilterMissing.Name = "chkFilterMissing"
        Me.chkFilterMissing.Size = New System.Drawing.Size(89, 17)
        Me.chkFilterMissing.TabIndex = 1
        Me.chkFilterMissing.Text = "Missing Items"
        Me.chkFilterMissing.UseVisualStyleBackColor = True
        '
        'chkFilterDupe
        '
        Me.chkFilterDupe.AutoSize = True
        Me.chkFilterDupe.Location = New System.Drawing.Point(7, 13)
        Me.chkFilterDupe.Name = "chkFilterDupe"
        Me.chkFilterDupe.Size = New System.Drawing.Size(76, 17)
        Me.chkFilterDupe.TabIndex = 0
        Me.chkFilterDupe.Text = "Duplicates"
        Me.chkFilterDupe.UseVisualStyleBackColor = True
        '
        'gbSpecific
        '
        Me.gbSpecific.Controls.Add(Me.txtFilterSource)
        Me.gbSpecific.Controls.Add(Me.Label6)
        Me.gbSpecific.Controls.Add(Me.cbFilterFileSource)
        Me.gbSpecific.Controls.Add(Me.chkFilterLock)
        Me.gbSpecific.Controls.Add(Me.GroupBox2)
        Me.gbSpecific.Controls.Add(Me.chkFilterNew)
        Me.gbSpecific.Controls.Add(Me.cbFilterYear)
        Me.gbSpecific.Controls.Add(Me.chkFilterMark)
        Me.gbSpecific.Controls.Add(Me.cbFilterYearMod)
        Me.gbSpecific.Controls.Add(Me.Label5)
        Me.gbSpecific.Controls.Add(Me.txtFilterGenre)
        Me.gbSpecific.Controls.Add(Me.Label2)
        Me.gbSpecific.Controls.Add(Me.Label3)
        Me.gbSpecific.Location = New System.Drawing.Point(135, 22)
        Me.gbSpecific.Name = "gbSpecific"
        Me.gbSpecific.Size = New System.Drawing.Size(224, 155)
        Me.gbSpecific.TabIndex = 3
        Me.gbSpecific.TabStop = False
        Me.gbSpecific.Text = "Specific"
        '
        'txtFilterSource
        '
        Me.txtFilterSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilterSource.Location = New System.Drawing.Point(50, 129)
        Me.txtFilterSource.Name = "txtFilterSource"
        Me.txtFilterSource.ReadOnly = True
        Me.txtFilterSource.Size = New System.Drawing.Size(166, 20)
        Me.txtFilterSource.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 13)
        Me.Label6.TabIndex = 38
        Me.Label6.Text = "File Source:"
        '
        'cbFilterFileSource
        '
        Me.cbFilterFileSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFilterFileSource.FormattingEnabled = True
        Me.cbFilterFileSource.Location = New System.Drawing.Point(70, 104)
        Me.cbFilterFileSource.Name = "cbFilterFileSource"
        Me.cbFilterFileSource.Size = New System.Drawing.Size(146, 21)
        Me.cbFilterFileSource.TabIndex = 7
        '
        'chkFilterLock
        '
        Me.chkFilterLock.AutoSize = True
        Me.chkFilterLock.Location = New System.Drawing.Point(80, 18)
        Me.chkFilterLock.Name = "chkFilterLock"
        Me.chkFilterLock.Size = New System.Drawing.Size(62, 17)
        Me.chkFilterLock.TabIndex = 2
        Me.chkFilterLock.Text = "Locked"
        Me.chkFilterLock.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbFilterAnd)
        Me.GroupBox2.Controls.Add(Me.rbFilterOr)
        Me.GroupBox2.Location = New System.Drawing.Point(158, 10)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(58, 43)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Modifier"
        '
        'rbFilterAnd
        '
        Me.rbFilterAnd.AutoSize = True
        Me.rbFilterAnd.Checked = True
        Me.rbFilterAnd.Location = New System.Drawing.Point(6, 11)
        Me.rbFilterAnd.Name = "rbFilterAnd"
        Me.rbFilterAnd.Size = New System.Drawing.Size(44, 17)
        Me.rbFilterAnd.TabIndex = 0
        Me.rbFilterAnd.TabStop = True
        Me.rbFilterAnd.Text = "And"
        Me.rbFilterAnd.UseVisualStyleBackColor = True
        '
        'rbFilterOr
        '
        Me.rbFilterOr.AutoSize = True
        Me.rbFilterOr.Location = New System.Drawing.Point(6, 25)
        Me.rbFilterOr.Name = "rbFilterOr"
        Me.rbFilterOr.Size = New System.Drawing.Size(36, 17)
        Me.rbFilterOr.TabIndex = 1
        Me.rbFilterOr.Text = "Or"
        Me.rbFilterOr.UseVisualStyleBackColor = True
        '
        'chkFilterNew
        '
        Me.chkFilterNew.AutoSize = True
        Me.chkFilterNew.Location = New System.Drawing.Point(9, 18)
        Me.chkFilterNew.Name = "chkFilterNew"
        Me.chkFilterNew.Size = New System.Drawing.Size(48, 17)
        Me.chkFilterNew.TabIndex = 0
        Me.chkFilterNew.Text = "New"
        Me.chkFilterNew.UseVisualStyleBackColor = True
        '
        'cbFilterYear
        '
        Me.cbFilterYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFilterYear.FormattingEnabled = True
        Me.cbFilterYear.Items.AddRange(New Object() {"=", ">", "<", "!="})
        Me.cbFilterYear.Location = New System.Drawing.Point(130, 80)
        Me.cbFilterYear.Name = "cbFilterYear"
        Me.cbFilterYear.Size = New System.Drawing.Size(69, 21)
        Me.cbFilterYear.TabIndex = 6
        '
        'chkFilterMark
        '
        Me.chkFilterMark.AutoSize = True
        Me.chkFilterMark.Location = New System.Drawing.Point(9, 36)
        Me.chkFilterMark.Name = "chkFilterMark"
        Me.chkFilterMark.Size = New System.Drawing.Size(62, 17)
        Me.chkFilterMark.TabIndex = 1
        Me.chkFilterMark.Text = "Marked"
        Me.chkFilterMark.UseVisualStyleBackColor = True
        '
        'cbFilterYearMod
        '
        Me.cbFilterYearMod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFilterYearMod.FormattingEnabled = True
        Me.cbFilterYearMod.Items.AddRange(New Object() {"=", ">", "<", "<>"})
        Me.cbFilterYearMod.Location = New System.Drawing.Point(67, 80)
        Me.cbFilterYearMod.Name = "cbFilterYearMod"
        Me.cbFilterYearMod.Size = New System.Drawing.Size(59, 21)
        Me.cbFilterYearMod.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 83)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "Year:"
        '
        'txtFilterGenre
        '
        Me.txtFilterGenre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilterGenre.Location = New System.Drawing.Point(50, 58)
        Me.txtFilterGenre.Name = "txtFilterGenre"
        Me.txtFilterGenre.ReadOnly = True
        Me.txtFilterGenre.Size = New System.Drawing.Size(166, 20)
        Me.txtFilterGenre.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 132)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Source:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 60)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "Genre:"
        '
        'btnFilterDown
        '
        Me.btnFilterDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterDown.BackColor = System.Drawing.SystemColors.Control
        Me.btnFilterDown.Enabled = False
        Me.btnFilterDown.Location = New System.Drawing.Point(309, 1)
        Me.btnFilterDown.Name = "btnFilterDown"
        Me.btnFilterDown.Size = New System.Drawing.Size(30, 22)
        Me.btnFilterDown.TabIndex = 28
        Me.btnFilterDown.TabStop = False
        Me.btnFilterDown.Text = "v"
        Me.btnFilterDown.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnFilterDown.UseVisualStyleBackColor = False
        '
        'btnFilterUp
        '
        Me.btnFilterUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterUp.BackColor = System.Drawing.SystemColors.Control
        Me.btnFilterUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFilterUp.Location = New System.Drawing.Point(277, 1)
        Me.btnFilterUp.Name = "btnFilterUp"
        Me.btnFilterUp.Size = New System.Drawing.Size(30, 22)
        Me.btnFilterUp.TabIndex = 27
        Me.btnFilterUp.TabStop = False
        Me.btnFilterUp.Text = "^"
        Me.btnFilterUp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnFilterUp.UseVisualStyleBackColor = False
        '
        'lblFilter
        '
        Me.lblFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFilter.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilter.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblFilter.Location = New System.Drawing.Point(4, 3)
        Me.lblFilter.Name = "lblFilter"
        Me.lblFilter.Size = New System.Drawing.Size(339, 17)
        Me.lblFilter.TabIndex = 22
        Me.lblFilter.Text = "Filters"
        Me.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlCancel
        '
        Me.pnlCancel.BackColor = System.Drawing.Color.LightGray
        Me.pnlCancel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCancel.Controls.Add(Me.pbCanceling)
        Me.pnlCancel.Controls.Add(Me.lblCanceling)
        Me.pnlCancel.Controls.Add(Me.btnCancel)
        Me.pnlCancel.Location = New System.Drawing.Point(273, 100)
        Me.pnlCancel.Name = "pnlCancel"
        Me.pnlCancel.Size = New System.Drawing.Size(214, 63)
        Me.pnlCancel.TabIndex = 8
        Me.pnlCancel.Visible = False
        '
        'pbCanceling
        '
        Me.pbCanceling.Location = New System.Drawing.Point(5, 32)
        Me.pbCanceling.MarqueeAnimationSpeed = 25
        Me.pbCanceling.Name = "pbCanceling"
        Me.pbCanceling.Size = New System.Drawing.Size(203, 20)
        Me.pbCanceling.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pbCanceling.TabIndex = 2
        Me.pbCanceling.Visible = False
        '
        'lblCanceling
        '
        Me.lblCanceling.AutoSize = True
        Me.lblCanceling.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCanceling.Location = New System.Drawing.Point(4, 12)
        Me.lblCanceling.Name = "lblCanceling"
        Me.lblCanceling.Size = New System.Drawing.Size(148, 16)
        Me.lblCanceling.TabIndex = 1
        Me.lblCanceling.Text = "Canceling Scraper..."
        Me.lblCanceling.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(4, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(205, 55)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.TabStop = False
        Me.btnCancel.Text = "Cancel Scraper"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'pnlNoInfo
        '
        Me.pnlNoInfo.BackColor = System.Drawing.Color.LightGray
        Me.pnlNoInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlNoInfo.Controls.Add(Me.Panel2)
        Me.pnlNoInfo.Location = New System.Drawing.Point(241, 300)
        Me.pnlNoInfo.Name = "pnlNoInfo"
        Me.pnlNoInfo.Size = New System.Drawing.Size(259, 143)
        Me.pnlNoInfo.TabIndex = 8
        Me.pnlNoInfo.Visible = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Location = New System.Drawing.Point(3, 4)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(251, 133)
        Me.Panel2.TabIndex = 0
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(7, 38)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(63, 63)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(71, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(173, 78)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "No Information is Available for This Movie"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlInfoPanel
        '
        Me.pnlInfoPanel.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlInfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlInfoPanel.Controls.Add(Me.txtCerts)
        Me.pnlInfoPanel.Controls.Add(Me.lblCertsHeader)
        Me.pnlInfoPanel.Controls.Add(Me.lblReleaseDate)
        Me.pnlInfoPanel.Controls.Add(Me.lblReleaseDateHeader)
        Me.pnlInfoPanel.Controls.Add(Me.btnMid)
        Me.pnlInfoPanel.Controls.Add(Me.pbMILoading)
        Me.pnlInfoPanel.Controls.Add(Me.btnMetaDataRefresh)
        Me.pnlInfoPanel.Controls.Add(Me.lblMetaDataHeader)
        Me.pnlInfoPanel.Controls.Add(Me.txtMetaData)
        Me.pnlInfoPanel.Controls.Add(Me.btnPlay)
        Me.pnlInfoPanel.Controls.Add(Me.txtFilePath)
        Me.pnlInfoPanel.Controls.Add(Me.lblFilePathHeader)
        Me.pnlInfoPanel.Controls.Add(Me.txtIMDBID)
        Me.pnlInfoPanel.Controls.Add(Me.lblIMDBHeader)
        Me.pnlInfoPanel.Controls.Add(Me.lblDirector)
        Me.pnlInfoPanel.Controls.Add(Me.lblDirectorHeader)
        Me.pnlInfoPanel.Controls.Add(Me.pnlActors)
        Me.pnlInfoPanel.Controls.Add(Me.lblOutlineHeader)
        Me.pnlInfoPanel.Controls.Add(Me.txtOutline)
        Me.pnlInfoPanel.Controls.Add(Me.pnlTop250)
        Me.pnlInfoPanel.Controls.Add(Me.lblPlotHeader)
        Me.pnlInfoPanel.Controls.Add(Me.txtPlot)
        Me.pnlInfoPanel.Controls.Add(Me.btnDown)
        Me.pnlInfoPanel.Controls.Add(Me.btnUp)
        Me.pnlInfoPanel.Controls.Add(Me.lblInfoPanelHeader)
        Me.pnlInfoPanel.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlInfoPanel.Location = New System.Drawing.Point(0, 346)
        Me.pnlInfoPanel.Name = "pnlInfoPanel"
        Me.pnlInfoPanel.Size = New System.Drawing.Size(663, 342)
        Me.pnlInfoPanel.TabIndex = 10
        '
        'txtCerts
        '
        Me.txtCerts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCerts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCerts.Location = New System.Drawing.Point(117, 208)
        Me.txtCerts.Name = "txtCerts"
        Me.txtCerts.ReadOnly = True
        Me.txtCerts.Size = New System.Drawing.Size(227, 20)
        Me.txtCerts.TabIndex = 41
        Me.txtCerts.TabStop = False
        '
        'lblCertsHeader
        '
        Me.lblCertsHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCertsHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblCertsHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCertsHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCertsHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblCertsHeader.Location = New System.Drawing.Point(117, 188)
        Me.lblCertsHeader.Name = "lblCertsHeader"
        Me.lblCertsHeader.Size = New System.Drawing.Size(227, 17)
        Me.lblCertsHeader.TabIndex = 40
        Me.lblCertsHeader.Text = "Certifications"
        Me.lblCertsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblReleaseDate
        '
        Me.lblReleaseDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblReleaseDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReleaseDate.ForeColor = System.Drawing.Color.Black
        Me.lblReleaseDate.Location = New System.Drawing.Point(178, 48)
        Me.lblReleaseDate.Name = "lblReleaseDate"
        Me.lblReleaseDate.Size = New System.Drawing.Size(105, 16)
        Me.lblReleaseDate.TabIndex = 39
        Me.lblReleaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblReleaseDateHeader
        '
        Me.lblReleaseDateHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblReleaseDateHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblReleaseDateHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblReleaseDateHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReleaseDateHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblReleaseDateHeader.Location = New System.Drawing.Point(178, 27)
        Me.lblReleaseDateHeader.Name = "lblReleaseDateHeader"
        Me.lblReleaseDateHeader.Size = New System.Drawing.Size(105, 17)
        Me.lblReleaseDateHeader.TabIndex = 38
        Me.lblReleaseDateHeader.Text = "Release Date"
        Me.lblReleaseDateHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnMid
        '
        Me.btnMid.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMid.BackColor = System.Drawing.SystemColors.Control
        Me.btnMid.Location = New System.Drawing.Point(592, 1)
        Me.btnMid.Name = "btnMid"
        Me.btnMid.Size = New System.Drawing.Size(30, 22)
        Me.btnMid.TabIndex = 37
        Me.btnMid.TabStop = False
        Me.btnMid.Text = "-"
        Me.btnMid.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnMid.UseVisualStyleBackColor = False
        '
        'pbMILoading
        '
        Me.pbMILoading.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMILoading.Image = CType(resources.GetObject("pbMILoading.Image"), System.Drawing.Image)
        Me.pbMILoading.Location = New System.Drawing.Point(494, 374)
        Me.pbMILoading.Name = "pbMILoading"
        Me.pbMILoading.Size = New System.Drawing.Size(41, 39)
        Me.pbMILoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbMILoading.TabIndex = 36
        Me.pbMILoading.TabStop = False
        Me.pbMILoading.Visible = False
        '
        'btnMetaDataRefresh
        '
        Me.btnMetaDataRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMetaDataRefresh.Location = New System.Drawing.Point(581, 278)
        Me.btnMetaDataRefresh.Name = "btnMetaDataRefresh"
        Me.btnMetaDataRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnMetaDataRefresh.TabIndex = 34
        Me.btnMetaDataRefresh.TabStop = False
        Me.btnMetaDataRefresh.Text = "Refresh"
        Me.btnMetaDataRefresh.UseVisualStyleBackColor = True
        '
        'lblMetaDataHeader
        '
        Me.lblMetaDataHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMetaDataHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblMetaDataHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMetaDataHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMetaDataHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblMetaDataHeader.Location = New System.Drawing.Point(357, 282)
        Me.lblMetaDataHeader.Name = "lblMetaDataHeader"
        Me.lblMetaDataHeader.Size = New System.Drawing.Size(294, 17)
        Me.lblMetaDataHeader.TabIndex = 35
        Me.lblMetaDataHeader.Text = "Meta Data"
        Me.lblMetaDataHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMetaData
        '
        Me.txtMetaData.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMetaData.BackColor = System.Drawing.Color.Gainsboro
        Me.txtMetaData.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMetaData.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMetaData.ForeColor = System.Drawing.Color.Black
        Me.txtMetaData.Location = New System.Drawing.Point(357, 303)
        Me.txtMetaData.Multiline = True
        Me.txtMetaData.Name = "txtMetaData"
        Me.txtMetaData.ReadOnly = True
        Me.txtMetaData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMetaData.Size = New System.Drawing.Size(296, 184)
        Me.txtMetaData.TabIndex = 33
        Me.txtMetaData.TabStop = False
        '
        'btnPlay
        '
        Me.btnPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPlay.Image = Global.Ember_Media_Manager.My.Resources.Resources.Play_Icon
        Me.btnPlay.Location = New System.Drawing.Point(325, 254)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(20, 20)
        Me.btnPlay.TabIndex = 32
        Me.btnPlay.TabStop = False
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'txtFilePath
        '
        Me.txtFilePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilePath.Location = New System.Drawing.Point(3, 254)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.ReadOnly = True
        Me.txtFilePath.Size = New System.Drawing.Size(319, 20)
        Me.txtFilePath.TabIndex = 31
        Me.txtFilePath.TabStop = False
        '
        'lblFilePathHeader
        '
        Me.lblFilePathHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFilePathHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblFilePathHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblFilePathHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFilePathHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblFilePathHeader.Location = New System.Drawing.Point(3, 234)
        Me.lblFilePathHeader.Name = "lblFilePathHeader"
        Me.lblFilePathHeader.Size = New System.Drawing.Size(341, 17)
        Me.lblFilePathHeader.TabIndex = 30
        Me.lblFilePathHeader.Text = "File Path"
        Me.lblFilePathHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtIMDBID
        '
        Me.txtIMDBID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIMDBID.Location = New System.Drawing.Point(3, 208)
        Me.txtIMDBID.Name = "txtIMDBID"
        Me.txtIMDBID.ReadOnly = True
        Me.txtIMDBID.Size = New System.Drawing.Size(108, 20)
        Me.txtIMDBID.TabIndex = 29
        Me.txtIMDBID.TabStop = False
        '
        'lblIMDBHeader
        '
        Me.lblIMDBHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblIMDBHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblIMDBHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIMDBHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblIMDBHeader.Location = New System.Drawing.Point(3, 188)
        Me.lblIMDBHeader.Name = "lblIMDBHeader"
        Me.lblIMDBHeader.Size = New System.Drawing.Size(108, 17)
        Me.lblIMDBHeader.TabIndex = 28
        Me.lblIMDBHeader.Text = "IMDB ID"
        Me.lblIMDBHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDirector
        '
        Me.lblDirector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDirector.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDirector.ForeColor = System.Drawing.Color.Black
        Me.lblDirector.Location = New System.Drawing.Point(3, 48)
        Me.lblDirector.Name = "lblDirector"
        Me.lblDirector.Size = New System.Drawing.Size(170, 16)
        Me.lblDirector.TabIndex = 27
        Me.lblDirector.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblDirectorHeader
        '
        Me.lblDirectorHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDirectorHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblDirectorHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblDirectorHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDirectorHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblDirectorHeader.Location = New System.Drawing.Point(3, 27)
        Me.lblDirectorHeader.Name = "lblDirectorHeader"
        Me.lblDirectorHeader.Size = New System.Drawing.Size(169, 17)
        Me.lblDirectorHeader.TabIndex = 21
        Me.lblDirectorHeader.Text = "Director"
        Me.lblDirectorHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlActors
        '
        Me.pnlActors.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlActors.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlActors.Controls.Add(Me.pbActLoad)
        Me.pnlActors.Controls.Add(Me.lstActors)
        Me.pnlActors.Controls.Add(Me.pbActors)
        Me.pnlActors.Controls.Add(Me.lblActorsHeader)
        Me.pnlActors.Location = New System.Drawing.Point(356, 29)
        Me.pnlActors.Name = "pnlActors"
        Me.pnlActors.Size = New System.Drawing.Size(302, 244)
        Me.pnlActors.TabIndex = 19
        '
        'pbActLoad
        '
        Me.pbActLoad.Image = CType(resources.GetObject("pbActLoad.Image"), System.Drawing.Image)
        Me.pbActLoad.Location = New System.Drawing.Point(240, 111)
        Me.pbActLoad.Name = "pbActLoad"
        Me.pbActLoad.Size = New System.Drawing.Size(41, 39)
        Me.pbActLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbActLoad.TabIndex = 26
        Me.pbActLoad.TabStop = False
        Me.pbActLoad.Visible = False
        '
        'lstActors
        '
        Me.lstActors.BackColor = System.Drawing.Color.Gainsboro
        Me.lstActors.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lstActors.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstActors.ForeColor = System.Drawing.Color.Black
        Me.lstActors.FormattingEnabled = True
        Me.lstActors.Location = New System.Drawing.Point(3, 21)
        Me.lstActors.Name = "lstActors"
        Me.lstActors.Size = New System.Drawing.Size(214, 221)
        Me.lstActors.TabIndex = 28
        Me.lstActors.TabStop = False
        '
        'pbActors
        '
        Me.pbActors.Image = Global.Ember_Media_Manager.My.Resources.Resources.actor_silhouette
        Me.pbActors.Location = New System.Drawing.Point(220, 75)
        Me.pbActors.Name = "pbActors"
        Me.pbActors.Size = New System.Drawing.Size(81, 106)
        Me.pbActors.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbActors.TabIndex = 27
        Me.pbActors.TabStop = False
        '
        'lblActorsHeader
        '
        Me.lblActorsHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblActorsHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblActorsHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblActorsHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblActorsHeader.Location = New System.Drawing.Point(0, 0)
        Me.lblActorsHeader.Name = "lblActorsHeader"
        Me.lblActorsHeader.Size = New System.Drawing.Size(301, 17)
        Me.lblActorsHeader.TabIndex = 18
        Me.lblActorsHeader.Text = "Cast"
        Me.lblActorsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOutlineHeader
        '
        Me.lblOutlineHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOutlineHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblOutlineHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblOutlineHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutlineHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblOutlineHeader.Location = New System.Drawing.Point(3, 81)
        Me.lblOutlineHeader.Name = "lblOutlineHeader"
        Me.lblOutlineHeader.Size = New System.Drawing.Size(341, 17)
        Me.lblOutlineHeader.TabIndex = 17
        Me.lblOutlineHeader.Text = "Plot Outline"
        Me.lblOutlineHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtOutline
        '
        Me.txtOutline.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOutline.BackColor = System.Drawing.Color.Gainsboro
        Me.txtOutline.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtOutline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutline.ForeColor = System.Drawing.Color.Black
        Me.txtOutline.Location = New System.Drawing.Point(3, 103)
        Me.txtOutline.Multiline = True
        Me.txtOutline.Name = "txtOutline"
        Me.txtOutline.ReadOnly = True
        Me.txtOutline.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtOutline.Size = New System.Drawing.Size(341, 78)
        Me.txtOutline.TabIndex = 16
        Me.txtOutline.TabStop = False
        '
        'pnlTop250
        '
        Me.pnlTop250.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTop250.Controls.Add(Me.lblTop250)
        Me.pnlTop250.Controls.Add(Me.pbTop250)
        Me.pnlTop250.Location = New System.Drawing.Point(287, 27)
        Me.pnlTop250.Name = "pnlTop250"
        Me.pnlTop250.Size = New System.Drawing.Size(56, 48)
        Me.pnlTop250.TabIndex = 15
        '
        'lblTop250
        '
        Me.lblTop250.BackColor = System.Drawing.Color.Gainsboro
        Me.lblTop250.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTop250.ForeColor = System.Drawing.Color.Black
        Me.lblTop250.Location = New System.Drawing.Point(1, 30)
        Me.lblTop250.Name = "lblTop250"
        Me.lblTop250.Size = New System.Drawing.Size(52, 17)
        Me.lblTop250.TabIndex = 15
        Me.lblTop250.Text = "# 250"
        Me.lblTop250.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbTop250
        '
        Me.pbTop250.Image = CType(resources.GetObject("pbTop250.Image"), System.Drawing.Image)
        Me.pbTop250.Location = New System.Drawing.Point(1, 1)
        Me.pbTop250.Name = "pbTop250"
        Me.pbTop250.Size = New System.Drawing.Size(54, 30)
        Me.pbTop250.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbTop250.TabIndex = 14
        Me.pbTop250.TabStop = False
        '
        'lblPlotHeader
        '
        Me.lblPlotHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPlotHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblPlotHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPlotHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlotHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblPlotHeader.Location = New System.Drawing.Point(3, 282)
        Me.lblPlotHeader.Name = "lblPlotHeader"
        Me.lblPlotHeader.Size = New System.Drawing.Size(341, 17)
        Me.lblPlotHeader.TabIndex = 8
        Me.lblPlotHeader.Text = "Plot"
        Me.lblPlotHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtPlot
        '
        Me.txtPlot.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPlot.BackColor = System.Drawing.Color.Gainsboro
        Me.txtPlot.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPlot.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPlot.ForeColor = System.Drawing.Color.Black
        Me.txtPlot.Location = New System.Drawing.Point(3, 303)
        Me.txtPlot.Multiline = True
        Me.txtPlot.Name = "txtPlot"
        Me.txtPlot.ReadOnly = True
        Me.txtPlot.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPlot.Size = New System.Drawing.Size(341, 184)
        Me.txtPlot.TabIndex = 7
        Me.txtPlot.TabStop = False
        '
        'btnDown
        '
        Me.btnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDown.BackColor = System.Drawing.SystemColors.Control
        Me.btnDown.Location = New System.Drawing.Point(623, 1)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(30, 22)
        Me.btnDown.TabIndex = 6
        Me.btnDown.TabStop = False
        Me.btnDown.Text = "v"
        Me.btnDown.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnDown.UseVisualStyleBackColor = False
        '
        'btnUp
        '
        Me.btnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUp.BackColor = System.Drawing.SystemColors.Control
        Me.btnUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUp.Location = New System.Drawing.Point(560, 1)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(30, 22)
        Me.btnUp.TabIndex = 1
        Me.btnUp.TabStop = False
        Me.btnUp.Text = "^"
        Me.btnUp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnUp.UseVisualStyleBackColor = False
        '
        'lblInfoPanelHeader
        '
        Me.lblInfoPanelHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfoPanelHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblInfoPanelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblInfoPanelHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfoPanelHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblInfoPanelHeader.Location = New System.Drawing.Point(3, 3)
        Me.lblInfoPanelHeader.Name = "lblInfoPanelHeader"
        Me.lblInfoPanelHeader.Size = New System.Drawing.Size(655, 17)
        Me.lblInfoPanelHeader.TabIndex = 0
        Me.lblInfoPanelHeader.Text = "Info"
        Me.lblInfoPanelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pnlPoster
        '
        Me.pnlPoster.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlPoster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlPoster.Controls.Add(Me.pbPoster)
        Me.pnlPoster.Location = New System.Drawing.Point(9, 112)
        Me.pnlPoster.Name = "pnlPoster"
        Me.pnlPoster.Size = New System.Drawing.Size(131, 169)
        Me.pnlPoster.TabIndex = 2
        Me.pnlPoster.Visible = False
        '
        'pbPoster
        '
        Me.pbPoster.BackColor = System.Drawing.SystemColors.Control
        Me.pbPoster.Location = New System.Drawing.Point(4, 4)
        Me.pbPoster.Name = "pbPoster"
        Me.pbPoster.Size = New System.Drawing.Size(121, 159)
        Me.pbPoster.TabIndex = 0
        Me.pbPoster.TabStop = False
        '
        'pbPosterCache
        '
        Me.pbPosterCache.Location = New System.Drawing.Point(454, 107)
        Me.pbPosterCache.Name = "pbPosterCache"
        Me.pbPosterCache.Size = New System.Drawing.Size(115, 111)
        Me.pbPosterCache.TabIndex = 12
        Me.pbPosterCache.TabStop = False
        Me.pbPosterCache.Visible = False
        '
        'pnlMPAA
        '
        Me.pnlMPAA.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlMPAA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMPAA.Controls.Add(Me.pbMPAA)
        Me.pnlMPAA.Location = New System.Drawing.Point(4, 609)
        Me.pnlMPAA.Name = "pnlMPAA"
        Me.pnlMPAA.Size = New System.Drawing.Size(202, 45)
        Me.pnlMPAA.TabIndex = 11
        Me.pnlMPAA.Visible = False
        '
        'pbMPAA
        '
        Me.pbMPAA.Location = New System.Drawing.Point(1, 1)
        Me.pbMPAA.Name = "pbMPAA"
        Me.pbMPAA.Size = New System.Drawing.Size(249, 57)
        Me.pbMPAA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pbMPAA.TabIndex = 13
        Me.pbMPAA.TabStop = False
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.Gainsboro
        Me.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTop.Controls.Add(Me.lblTitle)
        Me.pnlTop.Controls.Add(Me.pnlInfoIcons)
        Me.pnlTop.Controls.Add(Me.lblRuntime)
        Me.pnlTop.Controls.Add(Me.lblTagline)
        Me.pnlTop.Controls.Add(Me.pnlRating)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 25)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(663, 74)
        Me.pnlTop.TabIndex = 9
        Me.pnlTop.Visible = False
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Black
        Me.lblTitle.Location = New System.Drawing.Point(4, 2)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(0, 20)
        Me.lblTitle.TabIndex = 25
        Me.lblTitle.UseMnemonic = False
        '
        'pnlInfoIcons
        '
        Me.pnlInfoIcons.BackColor = System.Drawing.Color.Transparent
        Me.pnlInfoIcons.Controls.Add(Me.pbVType)
        Me.pnlInfoIcons.Controls.Add(Me.pbStudio)
        Me.pnlInfoIcons.Controls.Add(Me.pbVideo)
        Me.pnlInfoIcons.Controls.Add(Me.pbAudio)
        Me.pnlInfoIcons.Controls.Add(Me.pbResolution)
        Me.pnlInfoIcons.Controls.Add(Me.pbChannels)
        Me.pnlInfoIcons.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlInfoIcons.Location = New System.Drawing.Point(271, 0)
        Me.pnlInfoIcons.Name = "pnlInfoIcons"
        Me.pnlInfoIcons.Size = New System.Drawing.Size(390, 72)
        Me.pnlInfoIcons.TabIndex = 31
        '
        'pbVType
        '
        Me.pbVType.BackColor = System.Drawing.Color.Transparent
        Me.pbVType.Location = New System.Drawing.Point(65, 15)
        Me.pbVType.Name = "pbVType"
        Me.pbVType.Size = New System.Drawing.Size(64, 44)
        Me.pbVType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbVType.TabIndex = 36
        Me.pbVType.TabStop = False
        '
        'pbStudio
        '
        Me.pbStudio.BackColor = System.Drawing.Color.Transparent
        Me.pbStudio.Location = New System.Drawing.Point(325, 15)
        Me.pbStudio.Name = "pbStudio"
        Me.pbStudio.Size = New System.Drawing.Size(64, 44)
        Me.pbStudio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbStudio.TabIndex = 31
        Me.pbStudio.TabStop = False
        '
        'pbVideo
        '
        Me.pbVideo.BackColor = System.Drawing.Color.Transparent
        Me.pbVideo.Location = New System.Drawing.Point(0, 15)
        Me.pbVideo.Name = "pbVideo"
        Me.pbVideo.Size = New System.Drawing.Size(64, 44)
        Me.pbVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbVideo.TabIndex = 33
        Me.pbVideo.TabStop = False
        '
        'pbAudio
        '
        Me.pbAudio.BackColor = System.Drawing.Color.Transparent
        Me.pbAudio.Location = New System.Drawing.Point(195, 15)
        Me.pbAudio.Name = "pbAudio"
        Me.pbAudio.Size = New System.Drawing.Size(64, 44)
        Me.pbAudio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbAudio.TabIndex = 35
        Me.pbAudio.TabStop = False
        '
        'pbResolution
        '
        Me.pbResolution.BackColor = System.Drawing.Color.Transparent
        Me.pbResolution.Location = New System.Drawing.Point(130, 15)
        Me.pbResolution.Name = "pbResolution"
        Me.pbResolution.Size = New System.Drawing.Size(64, 44)
        Me.pbResolution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbResolution.TabIndex = 34
        Me.pbResolution.TabStop = False
        '
        'pbChannels
        '
        Me.pbChannels.BackColor = System.Drawing.Color.Transparent
        Me.pbChannels.Location = New System.Drawing.Point(260, 15)
        Me.pbChannels.Name = "pbChannels"
        Me.pbChannels.Size = New System.Drawing.Size(64, 44)
        Me.pbChannels.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbChannels.TabIndex = 32
        Me.pbChannels.TabStop = False
        '
        'lblRuntime
        '
        Me.lblRuntime.AutoSize = True
        Me.lblRuntime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRuntime.ForeColor = System.Drawing.Color.Black
        Me.lblRuntime.Location = New System.Drawing.Point(213, 32)
        Me.lblRuntime.Name = "lblRuntime"
        Me.lblRuntime.Size = New System.Drawing.Size(0, 13)
        Me.lblRuntime.TabIndex = 32
        Me.lblRuntime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTagline
        '
        Me.lblTagline.AutoSize = True
        Me.lblTagline.BackColor = System.Drawing.Color.Transparent
        Me.lblTagline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTagline.ForeColor = System.Drawing.Color.Black
        Me.lblTagline.Location = New System.Drawing.Point(5, 55)
        Me.lblTagline.Name = "lblTagline"
        Me.lblTagline.Size = New System.Drawing.Size(0, 13)
        Me.lblTagline.TabIndex = 26
        Me.lblTagline.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblTagline.UseMnemonic = False
        '
        'pnlRating
        '
        Me.pnlRating.BackColor = System.Drawing.Color.Transparent
        Me.pnlRating.Controls.Add(Me.pbStar5)
        Me.pnlRating.Controls.Add(Me.pbStar4)
        Me.pnlRating.Controls.Add(Me.pbStar3)
        Me.pnlRating.Controls.Add(Me.pbStar2)
        Me.pnlRating.Controls.Add(Me.pbStar1)
        Me.pnlRating.Controls.Add(Me.lblVotes)
        Me.pnlRating.Location = New System.Drawing.Point(6, 24)
        Me.pnlRating.Name = "pnlRating"
        Me.pnlRating.Size = New System.Drawing.Size(206, 27)
        Me.pnlRating.TabIndex = 24
        '
        'pbStar5
        '
        Me.pbStar5.Location = New System.Drawing.Point(97, 1)
        Me.pbStar5.Name = "pbStar5"
        Me.pbStar5.Size = New System.Drawing.Size(24, 24)
        Me.pbStar5.TabIndex = 27
        Me.pbStar5.TabStop = False
        '
        'pbStar4
        '
        Me.pbStar4.Location = New System.Drawing.Point(73, 1)
        Me.pbStar4.Name = "pbStar4"
        Me.pbStar4.Size = New System.Drawing.Size(24, 24)
        Me.pbStar4.TabIndex = 26
        Me.pbStar4.TabStop = False
        '
        'pbStar3
        '
        Me.pbStar3.Location = New System.Drawing.Point(49, 1)
        Me.pbStar3.Name = "pbStar3"
        Me.pbStar3.Size = New System.Drawing.Size(24, 24)
        Me.pbStar3.TabIndex = 25
        Me.pbStar3.TabStop = False
        '
        'pbStar2
        '
        Me.pbStar2.Location = New System.Drawing.Point(25, 1)
        Me.pbStar2.Name = "pbStar2"
        Me.pbStar2.Size = New System.Drawing.Size(24, 24)
        Me.pbStar2.TabIndex = 24
        Me.pbStar2.TabStop = False
        '
        'pbStar1
        '
        Me.pbStar1.Location = New System.Drawing.Point(1, 1)
        Me.pbStar1.Name = "pbStar1"
        Me.pbStar1.Size = New System.Drawing.Size(24, 24)
        Me.pbStar1.TabIndex = 23
        Me.pbStar1.TabStop = False
        '
        'lblVotes
        '
        Me.lblVotes.AutoSize = True
        Me.lblVotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVotes.ForeColor = System.Drawing.Color.Black
        Me.lblVotes.Location = New System.Drawing.Point(123, 8)
        Me.lblVotes.Name = "lblVotes"
        Me.lblVotes.Size = New System.Drawing.Size(0, 13)
        Me.lblVotes.TabIndex = 22
        Me.lblVotes.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'tsMain
        '
        Me.tsMain.CanOverflow = False
        Me.tsMain.GripMargin = New System.Windows.Forms.Padding(0)
        Me.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbAutoPilot, Me.tsbRefreshMedia, Me.tsbUpdateXBMC})
        Me.tsMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.tsMain.Location = New System.Drawing.Point(0, 0)
        Me.tsMain.Name = "tsMain"
        Me.tsMain.Padding = New System.Windows.Forms.Padding(0)
        Me.tsMain.Size = New System.Drawing.Size(663, 25)
        Me.tsMain.Stretch = True
        Me.tsMain.TabIndex = 6
        '
        'tsbAutoPilot
        '
        Me.tsbAutoPilot.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FullToolStripMenuItem, Me.UpdateOnlyToolStripMenuItem, Me.NewMoviesToolStripMenuItem, Me.MarkedMoviesToolStripMenuItem, Me.CurrentFilterToolStripMenuItem, Me.CustomUpdaterToolStripMenuItem})
        Me.tsbAutoPilot.Image = CType(resources.GetObject("tsbAutoPilot.Image"), System.Drawing.Image)
        Me.tsbAutoPilot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAutoPilot.Name = "tsbAutoPilot"
        Me.tsbAutoPilot.Size = New System.Drawing.Size(107, 22)
        Me.tsbAutoPilot.Text = "Scrape Media"
        '
        'FullToolStripMenuItem
        '
        Me.FullToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FullAutoToolStripMenuItem, Me.FullAskToolStripMenuItem})
        Me.FullToolStripMenuItem.Name = "FullToolStripMenuItem"
        Me.FullToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.FullToolStripMenuItem.Text = "All Movies"
        '
        'FullAutoToolStripMenuItem
        '
        Me.FullAutoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAllAutoAll, Me.mnuAllAutoNfo, Me.mnuAllAutoPoster, Me.mnuAllAutoFanart, Me.mnuAllAutoExtra, Me.mnuAllAutoTrailer, Me.mnuAllAutoMI})
        Me.FullAutoToolStripMenuItem.Name = "FullAutoToolStripMenuItem"
        Me.FullAutoToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.FullAutoToolStripMenuItem.Text = "Automatic (Force Best Match)"
        '
        'mnuAllAutoAll
        '
        Me.mnuAllAutoAll.Name = "mnuAllAutoAll"
        Me.mnuAllAutoAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAutoAll.Text = "All Items"
        '
        'mnuAllAutoNfo
        '
        Me.mnuAllAutoNfo.Name = "mnuAllAutoNfo"
        Me.mnuAllAutoNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAutoNfo.Text = "NFO Only"
        '
        'mnuAllAutoPoster
        '
        Me.mnuAllAutoPoster.Name = "mnuAllAutoPoster"
        Me.mnuAllAutoPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAutoPoster.Text = "Poster Only"
        '
        'mnuAllAutoFanart
        '
        Me.mnuAllAutoFanart.Name = "mnuAllAutoFanart"
        Me.mnuAllAutoFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAutoFanart.Text = "Fanart Only"
        '
        'mnuAllAutoExtra
        '
        Me.mnuAllAutoExtra.Name = "mnuAllAutoExtra"
        Me.mnuAllAutoExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAutoExtra.Text = "Extrathumbs Only"
        '
        'mnuAllAutoTrailer
        '
        Me.mnuAllAutoTrailer.Name = "mnuAllAutoTrailer"
        Me.mnuAllAutoTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAutoTrailer.Text = "Trailer Only"
        '
        'mnuAllAutoMI
        '
        Me.mnuAllAutoMI.Name = "mnuAllAutoMI"
        Me.mnuAllAutoMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAutoMI.Text = "Meta Data Only"
        '
        'FullAskToolStripMenuItem
        '
        Me.FullAskToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuAllAskAll, Me.mnuAllAskNfo, Me.mnuAllAskPoster, Me.mnuAllAskFanart, Me.mnuAllAskExtra, Me.mnuAllAskTrailer, Me.mnuAllAskMI})
        Me.FullAskToolStripMenuItem.Name = "FullAskToolStripMenuItem"
        Me.FullAskToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.FullAskToolStripMenuItem.Text = "Ask (Require Input If No Exact Match)"
        '
        'mnuAllAskAll
        '
        Me.mnuAllAskAll.Name = "mnuAllAskAll"
        Me.mnuAllAskAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAskAll.Text = "All Items"
        '
        'mnuAllAskNfo
        '
        Me.mnuAllAskNfo.Name = "mnuAllAskNfo"
        Me.mnuAllAskNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAskNfo.Text = "NFO Only"
        '
        'mnuAllAskPoster
        '
        Me.mnuAllAskPoster.Name = "mnuAllAskPoster"
        Me.mnuAllAskPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAskPoster.Text = "Poster Only"
        '
        'mnuAllAskFanart
        '
        Me.mnuAllAskFanart.Name = "mnuAllAskFanart"
        Me.mnuAllAskFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAskFanart.Text = "Fanart Only"
        '
        'mnuAllAskExtra
        '
        Me.mnuAllAskExtra.Name = "mnuAllAskExtra"
        Me.mnuAllAskExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAskExtra.Text = "Extrathumbs Only"
        '
        'mnuAllAskTrailer
        '
        Me.mnuAllAskTrailer.Name = "mnuAllAskTrailer"
        Me.mnuAllAskTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAskTrailer.Text = "Trailer Only"
        '
        'mnuAllAskMI
        '
        Me.mnuAllAskMI.Name = "mnuAllAskMI"
        Me.mnuAllAskMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuAllAskMI.Text = "Meta Data Only"
        '
        'UpdateOnlyToolStripMenuItem
        '
        Me.UpdateOnlyToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UpdateAutoToolStripMenuItem, Me.UpdateAskToolStripMenuItem})
        Me.UpdateOnlyToolStripMenuItem.Name = "UpdateOnlyToolStripMenuItem"
        Me.UpdateOnlyToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.UpdateOnlyToolStripMenuItem.Text = "Movies Missing Items"
        '
        'UpdateAutoToolStripMenuItem
        '
        Me.UpdateAutoToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMissAutoAll, Me.mnuMissAutoNfo, Me.mnuMissAutoPoster, Me.mnuMissAutoFanart, Me.mnuMissAutoExtra, Me.mnuMissAutoTrailer})
        Me.UpdateAutoToolStripMenuItem.Name = "UpdateAutoToolStripMenuItem"
        Me.UpdateAutoToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.UpdateAutoToolStripMenuItem.Text = "Automatic (Force Best Match)"
        '
        'mnuMissAutoAll
        '
        Me.mnuMissAutoAll.Name = "mnuMissAutoAll"
        Me.mnuMissAutoAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAutoAll.Text = "All Items"
        '
        'mnuMissAutoNfo
        '
        Me.mnuMissAutoNfo.Name = "mnuMissAutoNfo"
        Me.mnuMissAutoNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAutoNfo.Text = "NFO Only"
        '
        'mnuMissAutoPoster
        '
        Me.mnuMissAutoPoster.Name = "mnuMissAutoPoster"
        Me.mnuMissAutoPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAutoPoster.Text = "Poster Only"
        '
        'mnuMissAutoFanart
        '
        Me.mnuMissAutoFanart.Name = "mnuMissAutoFanart"
        Me.mnuMissAutoFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAutoFanart.Text = "Fanart Only"
        '
        'mnuMissAutoExtra
        '
        Me.mnuMissAutoExtra.Name = "mnuMissAutoExtra"
        Me.mnuMissAutoExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAutoExtra.Text = "Extrathumbs Only"
        '
        'mnuMissAutoTrailer
        '
        Me.mnuMissAutoTrailer.Name = "mnuMissAutoTrailer"
        Me.mnuMissAutoTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAutoTrailer.Text = "Trailer Only"
        '
        'UpdateAskToolStripMenuItem
        '
        Me.UpdateAskToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMissAskAll, Me.mnuMissAskNfo, Me.mnuMissAskPoster, Me.mnuMissAskFanart, Me.mnuMissAskExtra, Me.mnuMissAskTrailer})
        Me.UpdateAskToolStripMenuItem.Name = "UpdateAskToolStripMenuItem"
        Me.UpdateAskToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.UpdateAskToolStripMenuItem.Text = "Ask (Require Input If No Exact Match)"
        '
        'mnuMissAskAll
        '
        Me.mnuMissAskAll.Name = "mnuMissAskAll"
        Me.mnuMissAskAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAskAll.Text = "All Items"
        '
        'mnuMissAskNfo
        '
        Me.mnuMissAskNfo.Name = "mnuMissAskNfo"
        Me.mnuMissAskNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAskNfo.Text = "NFO Only"
        '
        'mnuMissAskPoster
        '
        Me.mnuMissAskPoster.Name = "mnuMissAskPoster"
        Me.mnuMissAskPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAskPoster.Text = "Poster Only"
        '
        'mnuMissAskFanart
        '
        Me.mnuMissAskFanart.Name = "mnuMissAskFanart"
        Me.mnuMissAskFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAskFanart.Text = "Fanart Only"
        '
        'mnuMissAskExtra
        '
        Me.mnuMissAskExtra.Name = "mnuMissAskExtra"
        Me.mnuMissAskExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAskExtra.Text = "Extrathumbs Only"
        '
        'mnuMissAskTrailer
        '
        Me.mnuMissAskTrailer.Name = "mnuMissAskTrailer"
        Me.mnuMissAskTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuMissAskTrailer.Text = "Trailer Only"
        '
        'NewMoviesToolStripMenuItem
        '
        Me.NewMoviesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutomaticForceBestMatchToolStripMenuItem, Me.AskRequireInputToolStripMenuItem})
        Me.NewMoviesToolStripMenuItem.Name = "NewMoviesToolStripMenuItem"
        Me.NewMoviesToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.NewMoviesToolStripMenuItem.Text = "New Movies"
        '
        'AutomaticForceBestMatchToolStripMenuItem
        '
        Me.AutomaticForceBestMatchToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewAutoAll, Me.mnuNewAutoNfo, Me.mnuNewAutoPoster, Me.mnuNewAutoFanart, Me.mnuNewAutoExtra, Me.mnuNewAutoTrailer, Me.mnuNewAutoMI})
        Me.AutomaticForceBestMatchToolStripMenuItem.Name = "AutomaticForceBestMatchToolStripMenuItem"
        Me.AutomaticForceBestMatchToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.AutomaticForceBestMatchToolStripMenuItem.Text = "Automatic (Force Best Match)"
        '
        'mnuNewAutoAll
        '
        Me.mnuNewAutoAll.Name = "mnuNewAutoAll"
        Me.mnuNewAutoAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAutoAll.Text = "All Items"
        '
        'mnuNewAutoNfo
        '
        Me.mnuNewAutoNfo.Name = "mnuNewAutoNfo"
        Me.mnuNewAutoNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAutoNfo.Text = "NFO Only"
        '
        'mnuNewAutoPoster
        '
        Me.mnuNewAutoPoster.Name = "mnuNewAutoPoster"
        Me.mnuNewAutoPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAutoPoster.Text = "Poster Only"
        '
        'mnuNewAutoFanart
        '
        Me.mnuNewAutoFanart.Name = "mnuNewAutoFanart"
        Me.mnuNewAutoFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAutoFanart.Text = "Fanart Only"
        '
        'mnuNewAutoExtra
        '
        Me.mnuNewAutoExtra.Name = "mnuNewAutoExtra"
        Me.mnuNewAutoExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAutoExtra.Text = "Extrathumbs Only"
        '
        'mnuNewAutoTrailer
        '
        Me.mnuNewAutoTrailer.Name = "mnuNewAutoTrailer"
        Me.mnuNewAutoTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAutoTrailer.Text = "Trailer Only"
        '
        'mnuNewAutoMI
        '
        Me.mnuNewAutoMI.Name = "mnuNewAutoMI"
        Me.mnuNewAutoMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAutoMI.Text = "Meta Data Only"
        '
        'AskRequireInputToolStripMenuItem
        '
        Me.AskRequireInputToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuNewAskAll, Me.mnuNewAskNfo, Me.mnuNewAskPoster, Me.mnuNewAskFanart, Me.mnuNewAskExtra, Me.mnuNewAskTrailer, Me.mnuNewAskMI})
        Me.AskRequireInputToolStripMenuItem.Name = "AskRequireInputToolStripMenuItem"
        Me.AskRequireInputToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.AskRequireInputToolStripMenuItem.Text = "Ask (Require Input If No Exact Match)"
        '
        'mnuNewAskAll
        '
        Me.mnuNewAskAll.Name = "mnuNewAskAll"
        Me.mnuNewAskAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAskAll.Text = "All Items"
        '
        'mnuNewAskNfo
        '
        Me.mnuNewAskNfo.Name = "mnuNewAskNfo"
        Me.mnuNewAskNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAskNfo.Text = "NFO Only"
        '
        'mnuNewAskPoster
        '
        Me.mnuNewAskPoster.Name = "mnuNewAskPoster"
        Me.mnuNewAskPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAskPoster.Text = "Poster Only"
        '
        'mnuNewAskFanart
        '
        Me.mnuNewAskFanart.Name = "mnuNewAskFanart"
        Me.mnuNewAskFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAskFanart.Text = "Fanart Only"
        '
        'mnuNewAskExtra
        '
        Me.mnuNewAskExtra.Name = "mnuNewAskExtra"
        Me.mnuNewAskExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAskExtra.Text = "Extrathumbs Only"
        '
        'mnuNewAskTrailer
        '
        Me.mnuNewAskTrailer.Name = "mnuNewAskTrailer"
        Me.mnuNewAskTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAskTrailer.Text = "Trailer Only"
        '
        'mnuNewAskMI
        '
        Me.mnuNewAskMI.Name = "mnuNewAskMI"
        Me.mnuNewAskMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuNewAskMI.Text = "Meta Data Only"
        '
        'MarkedMoviesToolStripMenuItem
        '
        Me.MarkedMoviesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutomaticForceBestMatchToolStripMenuItem1, Me.AskRequireInputIfNoExactMatchToolStripMenuItem})
        Me.MarkedMoviesToolStripMenuItem.Name = "MarkedMoviesToolStripMenuItem"
        Me.MarkedMoviesToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.MarkedMoviesToolStripMenuItem.Text = "Marked Movies"
        '
        'AutomaticForceBestMatchToolStripMenuItem1
        '
        Me.AutomaticForceBestMatchToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMarkAutoAll, Me.mnuMarkAutoNfo, Me.mnuMarkAutoPoster, Me.mnuMarkAutoFanart, Me.mnuMarkAutoExtra, Me.mnuMarkAutoTrailer, Me.mnuMarkAutoMI})
        Me.AutomaticForceBestMatchToolStripMenuItem1.Name = "AutomaticForceBestMatchToolStripMenuItem1"
        Me.AutomaticForceBestMatchToolStripMenuItem1.Size = New System.Drawing.Size(271, 22)
        Me.AutomaticForceBestMatchToolStripMenuItem1.Text = "Automatic (Force Best Match)"
        '
        'mnuMarkAutoAll
        '
        Me.mnuMarkAutoAll.Name = "mnuMarkAutoAll"
        Me.mnuMarkAutoAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAutoAll.Text = "All Items"
        '
        'mnuMarkAutoNfo
        '
        Me.mnuMarkAutoNfo.Name = "mnuMarkAutoNfo"
        Me.mnuMarkAutoNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAutoNfo.Text = "NFO Only"
        '
        'mnuMarkAutoPoster
        '
        Me.mnuMarkAutoPoster.Name = "mnuMarkAutoPoster"
        Me.mnuMarkAutoPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAutoPoster.Text = "Poster Only"
        '
        'mnuMarkAutoFanart
        '
        Me.mnuMarkAutoFanart.Name = "mnuMarkAutoFanart"
        Me.mnuMarkAutoFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAutoFanart.Text = "Fanart Only"
        '
        'mnuMarkAutoExtra
        '
        Me.mnuMarkAutoExtra.Name = "mnuMarkAutoExtra"
        Me.mnuMarkAutoExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAutoExtra.Text = "Extrathumbs Only"
        '
        'mnuMarkAutoTrailer
        '
        Me.mnuMarkAutoTrailer.Name = "mnuMarkAutoTrailer"
        Me.mnuMarkAutoTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAutoTrailer.Text = "Trailer Only"
        '
        'mnuMarkAutoMI
        '
        Me.mnuMarkAutoMI.Name = "mnuMarkAutoMI"
        Me.mnuMarkAutoMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAutoMI.Text = "Meta Data Only"
        '
        'AskRequireInputIfNoExactMatchToolStripMenuItem
        '
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMarkAskAll, Me.mnuMarkAskNfo, Me.mnuMarkAskPoster, Me.mnuMarkAskFanart, Me.mnuMarkAskExtra, Me.mnuMarkAskTrailer, Me.mnuMarkAskMI})
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem.Name = "AskRequireInputIfNoExactMatchToolStripMenuItem"
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem.Size = New System.Drawing.Size(271, 22)
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem.Text = "Ask (Require Input If No Exact Match)"
        '
        'mnuMarkAskAll
        '
        Me.mnuMarkAskAll.Name = "mnuMarkAskAll"
        Me.mnuMarkAskAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAskAll.Text = "All Items"
        '
        'mnuMarkAskNfo
        '
        Me.mnuMarkAskNfo.Name = "mnuMarkAskNfo"
        Me.mnuMarkAskNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAskNfo.Text = "NFO Only"
        '
        'mnuMarkAskPoster
        '
        Me.mnuMarkAskPoster.Name = "mnuMarkAskPoster"
        Me.mnuMarkAskPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAskPoster.Text = "Poster Only"
        '
        'mnuMarkAskFanart
        '
        Me.mnuMarkAskFanart.Name = "mnuMarkAskFanart"
        Me.mnuMarkAskFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAskFanart.Text = "Fanart Only"
        '
        'mnuMarkAskExtra
        '
        Me.mnuMarkAskExtra.Name = "mnuMarkAskExtra"
        Me.mnuMarkAskExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAskExtra.Text = "Extrathumbs Only"
        '
        'mnuMarkAskTrailer
        '
        Me.mnuMarkAskTrailer.Name = "mnuMarkAskTrailer"
        Me.mnuMarkAskTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAskTrailer.Text = "Trailer Only"
        '
        'mnuMarkAskMI
        '
        Me.mnuMarkAskMI.Name = "mnuMarkAskMI"
        Me.mnuMarkAskMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuMarkAskMI.Text = "Meta Data Only"
        '
        'CurrentFilterToolStripMenuItem
        '
        Me.CurrentFilterToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AutomaticForceBestMatchToolStripMenuItem2, Me.AskRequireInputIfNoExactMatchToolStripMenuItem1})
        Me.CurrentFilterToolStripMenuItem.Name = "CurrentFilterToolStripMenuItem"
        Me.CurrentFilterToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CurrentFilterToolStripMenuItem.Text = "Current Filter"
        '
        'AutomaticForceBestMatchToolStripMenuItem2
        '
        Me.AutomaticForceBestMatchToolStripMenuItem2.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFilterAutoAll, Me.mnuFilterAutoNfo, Me.mnuFilterAutoPoster, Me.mnuFilterAutoFanart, Me.mnuFilterAutoExtra, Me.mnuFilterAutoTrailer, Me.mnuFilterAutoMI})
        Me.AutomaticForceBestMatchToolStripMenuItem2.Name = "AutomaticForceBestMatchToolStripMenuItem2"
        Me.AutomaticForceBestMatchToolStripMenuItem2.Size = New System.Drawing.Size(271, 22)
        Me.AutomaticForceBestMatchToolStripMenuItem2.Text = "Automatic (Force Best Match)"
        '
        'mnuFilterAutoAll
        '
        Me.mnuFilterAutoAll.Name = "mnuFilterAutoAll"
        Me.mnuFilterAutoAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAutoAll.Text = "All Items"
        '
        'mnuFilterAutoNfo
        '
        Me.mnuFilterAutoNfo.Name = "mnuFilterAutoNfo"
        Me.mnuFilterAutoNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAutoNfo.Text = "NFO Only"
        '
        'mnuFilterAutoPoster
        '
        Me.mnuFilterAutoPoster.Name = "mnuFilterAutoPoster"
        Me.mnuFilterAutoPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAutoPoster.Text = "Poster Only"
        '
        'mnuFilterAutoFanart
        '
        Me.mnuFilterAutoFanart.Name = "mnuFilterAutoFanart"
        Me.mnuFilterAutoFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAutoFanart.Text = "Fanart Only"
        '
        'mnuFilterAutoExtra
        '
        Me.mnuFilterAutoExtra.Name = "mnuFilterAutoExtra"
        Me.mnuFilterAutoExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAutoExtra.Text = "Extrathumbs Only"
        '
        'mnuFilterAutoTrailer
        '
        Me.mnuFilterAutoTrailer.Name = "mnuFilterAutoTrailer"
        Me.mnuFilterAutoTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAutoTrailer.Text = "Trailer Only"
        '
        'mnuFilterAutoMI
        '
        Me.mnuFilterAutoMI.Name = "mnuFilterAutoMI"
        Me.mnuFilterAutoMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAutoMI.Text = "Meta Data Only"
        '
        'AskRequireInputIfNoExactMatchToolStripMenuItem1
        '
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFilterAskAll, Me.mnuFilterAskNfo, Me.mnuFilterAskPoster, Me.mnuFilterAskFanart, Me.mnuFilterAskExtra, Me.mnuFilterAskTrailer, Me.mnuFilterAskMI})
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem1.Name = "AskRequireInputIfNoExactMatchToolStripMenuItem1"
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem1.Size = New System.Drawing.Size(271, 22)
        Me.AskRequireInputIfNoExactMatchToolStripMenuItem1.Text = "Ask (Require Input If No Exact Match)"
        '
        'mnuFilterAskAll
        '
        Me.mnuFilterAskAll.Name = "mnuFilterAskAll"
        Me.mnuFilterAskAll.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAskAll.Text = "All Items"
        '
        'mnuFilterAskNfo
        '
        Me.mnuFilterAskNfo.Name = "mnuFilterAskNfo"
        Me.mnuFilterAskNfo.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAskNfo.Text = "NFO Only"
        '
        'mnuFilterAskPoster
        '
        Me.mnuFilterAskPoster.Name = "mnuFilterAskPoster"
        Me.mnuFilterAskPoster.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAskPoster.Text = "Poster Only"
        '
        'mnuFilterAskFanart
        '
        Me.mnuFilterAskFanart.Name = "mnuFilterAskFanart"
        Me.mnuFilterAskFanart.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAskFanart.Text = "Fanart Only"
        '
        'mnuFilterAskExtra
        '
        Me.mnuFilterAskExtra.Name = "mnuFilterAskExtra"
        Me.mnuFilterAskExtra.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAskExtra.Text = "Extrathumbs Only"
        '
        'mnuFilterAskTrailer
        '
        Me.mnuFilterAskTrailer.Name = "mnuFilterAskTrailer"
        Me.mnuFilterAskTrailer.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAskTrailer.Text = "Trailer Only"
        '
        'mnuFilterAskMI
        '
        Me.mnuFilterAskMI.Name = "mnuFilterAskMI"
        Me.mnuFilterAskMI.Size = New System.Drawing.Size(168, 22)
        Me.mnuFilterAskMI.Text = "Meta Data Only"
        '
        'CustomUpdaterToolStripMenuItem
        '
        Me.CustomUpdaterToolStripMenuItem.Name = "CustomUpdaterToolStripMenuItem"
        Me.CustomUpdaterToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CustomUpdaterToolStripMenuItem.Text = "Custom Scraper..."
        '
        'tsbRefreshMedia
        '
        Me.tsbRefreshMedia.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuMoviesUpdate, Me.mnuTVShowUpdate})
        Me.tsbRefreshMedia.Image = CType(resources.GetObject("tsbRefreshMedia.Image"), System.Drawing.Image)
        Me.tsbRefreshMedia.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRefreshMedia.Name = "tsbRefreshMedia"
        Me.tsbRefreshMedia.Size = New System.Drawing.Size(116, 22)
        Me.tsbRefreshMedia.Text = "Update Library"
        '
        'mnuMoviesUpdate
        '
        Me.mnuMoviesUpdate.Name = "mnuMoviesUpdate"
        Me.mnuMoviesUpdate.Size = New System.Drawing.Size(125, 22)
        Me.mnuMoviesUpdate.Text = "Movies"
        '
        'mnuTVShowUpdate
        '
        Me.mnuTVShowUpdate.Name = "mnuTVShowUpdate"
        Me.mnuTVShowUpdate.Size = New System.Drawing.Size(125, 22)
        Me.mnuTVShowUpdate.Text = "TV Shows"
        '
        'tsbUpdateXBMC
        '
        Me.tsbUpdateXBMC.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbUpdateXBMC.Enabled = False
        Me.tsbUpdateXBMC.Image = CType(resources.GetObject("tsbUpdateXBMC.Image"), System.Drawing.Image)
        Me.tsbUpdateXBMC.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUpdateXBMC.Name = "tsbUpdateXBMC"
        Me.tsbUpdateXBMC.Size = New System.Drawing.Size(152, 22)
        Me.tsbUpdateXBMC.Text = "Initiate XBMC Update"
        '
        'pbFanartCache
        '
        Me.pbFanartCache.Location = New System.Drawing.Point(576, 107)
        Me.pbFanartCache.Name = "pbFanartCache"
        Me.pbFanartCache.Size = New System.Drawing.Size(115, 111)
        Me.pbFanartCache.TabIndex = 3
        Me.pbFanartCache.TabStop = False
        Me.pbFanartCache.Visible = False
        '
        'pbFanart
        '
        Me.pbFanart.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.pbFanart.BackColor = System.Drawing.Color.DimGray
        Me.pbFanart.Location = New System.Drawing.Point(-17, 99)
        Me.pbFanart.Name = "pbFanart"
        Me.pbFanart.Size = New System.Drawing.Size(696, 250)
        Me.pbFanart.TabIndex = 1
        Me.pbFanart.TabStop = False
        '
        'ilColumnIcons
        '
        Me.ilColumnIcons.ImageStream = CType(resources.GetObject("ilColumnIcons.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilColumnIcons.TransparentColor = System.Drawing.Color.Transparent
        Me.ilColumnIcons.Images.SetKeyName(0, "new_page.png")
        Me.ilColumnIcons.Images.SetKeyName(1, "image.png")
        Me.ilColumnIcons.Images.SetKeyName(2, "info.png")
        Me.ilColumnIcons.Images.SetKeyName(3, "favorite_film.png")
        Me.ilColumnIcons.Images.SetKeyName(4, "comment.png")
        Me.ilColumnIcons.Images.SetKeyName(5, "folder_full.png")
        '
        'tmrWait
        '
        Me.tmrWait.Interval = 250
        '
        'tmrLoad
        '
        '
        'tmrSearchWait
        '
        Me.tmrSearchWait.Interval = 250
        '
        'tmrSearch
        '
        Me.tmrSearch.Interval = 250
        '
        'tmrFilterAni
        '
        Me.tmrFilterAni.Interval = 1
        '
        'ToolTips
        '
        Me.ToolTips.AutoPopDelay = 15000
        Me.ToolTips.InitialDelay = 500
        Me.ToolTips.ReshowDelay = 100
        '
        'tmrWaitShow
        '
        Me.tmrWaitShow.Interval = 250
        '
        'tmrLoadShow
        '
        '
        'tmrWaitSeason
        '
        Me.tmrWaitSeason.Interval = 250
        '
        'tmrLoadSeason
        '
        '
        'tmrWaitEp
        '
        Me.tmrWaitEp.Interval = 250
        '
        'tmrLoadEp
        '
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1016, 734)
        Me.Controls.Add(Me.scMain)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.MenuStrip)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip
        Me.MinimumSize = New System.Drawing.Size(1024, 768)
        Me.Name = "frmMain"
        Me.Text = "Ember Media Manager"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.scMain.Panel1.ResumeLayout(False)
        Me.scMain.Panel2.ResumeLayout(False)
        Me.scMain.Panel2.PerformLayout()
        Me.scMain.ResumeLayout(False)
        Me.pnlFilterGenre.ResumeLayout(False)
        Me.pnlFilterGenre.PerformLayout()
        Me.pnlFilterSource.ResumeLayout(False)
        Me.pnlFilterSource.PerformLayout()
        CType(Me.dgvMediaList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuMediaList.ResumeLayout(False)
        Me.scTV.Panel1.ResumeLayout(False)
        Me.scTV.Panel2.ResumeLayout(False)
        Me.scTV.ResumeLayout(False)
        CType(Me.dgvTVShows, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuShows.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        CType(Me.dgvTVSeasons, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvTVEpisodes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuEpisodes.ResumeLayout(False)
        Me.pnlListTop.ResumeLayout(False)
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.picSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabsMain.ResumeLayout(False)
        Me.pnlFilter.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.gbSpecific.ResumeLayout(False)
        Me.gbSpecific.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.pnlCancel.ResumeLayout(False)
        Me.pnlCancel.PerformLayout()
        Me.pnlNoInfo.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlInfoPanel.ResumeLayout(False)
        Me.pnlInfoPanel.PerformLayout()
        CType(Me.pbMILoading, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlActors.ResumeLayout(False)
        CType(Me.pbActLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbActors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop250.ResumeLayout(False)
        CType(Me.pbTop250, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPoster.ResumeLayout(False)
        CType(Me.pbPoster, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPosterCache, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMPAA.ResumeLayout(False)
        Me.pnlMPAA.PerformLayout()
        CType(Me.pbMPAA, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.pnlInfoIcons.ResumeLayout(False)
        CType(Me.pbVType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStudio, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbVideo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbAudio, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbResolution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbChannels, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlRating.ResumeLayout(False)
        Me.pnlRating.PerformLayout()
        CType(Me.pbStar5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tsMain.ResumeLayout(False)
        Me.tsMain.PerformLayout()
        CType(Me.pbFanartCache, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbFanart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BottomToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents TopToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents RightToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents LeftToolStripPanel As System.Windows.Forms.ToolStripPanel
    Friend WithEvents ContentPanel As System.Windows.Forms.ToolStripContentPanel
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents tslStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tslLoading As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents tspbLoading As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents tmrAni As System.Windows.Forms.Timer
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents scMain As System.Windows.Forms.SplitContainer
    Friend WithEvents pbFanartCache As System.Windows.Forms.PictureBox
    Friend WithEvents pnlPoster As System.Windows.Forms.Panel
    Friend WithEvents pbPoster As System.Windows.Forms.PictureBox
    Friend WithEvents pbFanart As System.Windows.Forms.PictureBox
    Friend WithEvents tabsMain As System.Windows.Forms.TabControl
    Friend WithEvents pnlMPAA As System.Windows.Forms.Panel
    Friend WithEvents pbMPAA As System.Windows.Forms.PictureBox
    Friend WithEvents pnlInfoPanel As System.Windows.Forms.Panel
    Friend WithEvents btnPlay As System.Windows.Forms.Button
    Friend WithEvents txtFilePath As System.Windows.Forms.TextBox
    Friend WithEvents lblFilePathHeader As System.Windows.Forms.Label
    Friend WithEvents txtIMDBID As System.Windows.Forms.TextBox
    Friend WithEvents lblIMDBHeader As System.Windows.Forms.Label
    Friend WithEvents lblDirector As System.Windows.Forms.Label
    Friend WithEvents lblDirectorHeader As System.Windows.Forms.Label
    Friend WithEvents pnlActors As System.Windows.Forms.Panel
    Friend WithEvents pbActLoad As System.Windows.Forms.PictureBox
    Friend WithEvents lstActors As System.Windows.Forms.ListBox
    Friend WithEvents pbActors As System.Windows.Forms.PictureBox
    Friend WithEvents lblActorsHeader As System.Windows.Forms.Label
    Friend WithEvents lblOutlineHeader As System.Windows.Forms.Label
    Friend WithEvents txtOutline As System.Windows.Forms.TextBox
    Friend WithEvents pnlTop250 As System.Windows.Forms.Panel
    Friend WithEvents lblTop250 As System.Windows.Forms.Label
    Friend WithEvents pbTop250 As System.Windows.Forms.PictureBox
    Friend WithEvents lblPlotHeader As System.Windows.Forms.Label
    Friend WithEvents txtPlot As System.Windows.Forms.TextBox
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents lblInfoPanelHeader As System.Windows.Forms.Label
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents pnlInfoIcons As System.Windows.Forms.Panel
    Friend WithEvents pbVideo As System.Windows.Forms.PictureBox
    Friend WithEvents pbAudio As System.Windows.Forms.PictureBox
    Friend WithEvents pbResolution As System.Windows.Forms.PictureBox
    Friend WithEvents pbChannels As System.Windows.Forms.PictureBox
    Friend WithEvents pbStudio As System.Windows.Forms.PictureBox
    Friend WithEvents lblTagline As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents pnlRating As System.Windows.Forms.Panel
    Friend WithEvents pbStar5 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar4 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar3 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar2 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblVotes As System.Windows.Forms.Label
    Friend WithEvents tsMain As System.Windows.Forms.ToolStrip
    Friend WithEvents btnMetaDataRefresh As System.Windows.Forms.Button
    Friend WithEvents lblMetaDataHeader As System.Windows.Forms.Label
    Friend WithEvents txtMetaData As System.Windows.Forms.TextBox
    Friend WithEvents pbMILoading As System.Windows.Forms.PictureBox
    Friend WithEvents tsbAutoPilot As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents btnMid As System.Windows.Forms.Button
    Friend WithEvents lblRuntime As System.Windows.Forms.Label
    Friend WithEvents pbPosterCache As System.Windows.Forms.PictureBox
    Friend WithEvents txtCerts As System.Windows.Forms.TextBox
    Friend WithEvents lblCertsHeader As System.Windows.Forms.Label
    Friend WithEvents lblReleaseDate As System.Windows.Forms.Label
    Friend WithEvents lblReleaseDateHeader As System.Windows.Forms.Label
    Friend WithEvents ilColumnIcons As System.Windows.Forms.ImageList
    Friend WithEvents FullToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FullAutoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FullAskToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateOnlyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateAutoToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents UpdateAskToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrWait As System.Windows.Forms.Timer
    Friend WithEvents tmrLoad As System.Windows.Forms.Timer
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents picSearch As System.Windows.Forms.PictureBox
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents tmrSearchWait As System.Windows.Forms.Timer
    Friend WithEvents tmrSearch As System.Windows.Forms.Timer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CleanFoldersToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ConvertFileSourceToFolderSourceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pnlFilter As System.Windows.Forms.Panel
    Friend WithEvents lblFilter As System.Windows.Forms.Label
    Friend WithEvents chkFilterNew As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilterMark As System.Windows.Forms.CheckBox
    Friend WithEvents rbFilterOr As System.Windows.Forms.RadioButton
    Friend WithEvents rbFilterAnd As System.Windows.Forms.RadioButton
    Friend WithEvents chkFilterDupe As System.Windows.Forms.CheckBox
    Friend WithEvents tsbUpdateXBMC As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents mnuMediaList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuMark As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuRescrape As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuSearchNew As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuTitle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuSep As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuEditMovie As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAutoAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAutoNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAutoPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAutoFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAutoExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAskAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAskNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAskPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAskFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAskExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAutoAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAutoNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAutoPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAutoFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAutoExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAskAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAskNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAskPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAskFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAskExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewMoviesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutomaticForceBestMatchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AskRequireInputToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MarkedMoviesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAutoAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAutoNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAutoPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAutoFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAutoExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAskAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAskNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAskPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAskFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAskExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutomaticForceBestMatchToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAutoAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAutoNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAutoPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAutoFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAutoExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AskRequireInputIfNoExactMatchToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAskAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAskNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAskPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAskFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAskExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabMovies As System.Windows.Forms.TabPage
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents pbCanceling As System.Windows.Forms.ProgressBar
    Friend WithEvents lblCanceling As System.Windows.Forms.Label
    Private WithEvents pnlNoInfo As System.Windows.Forms.Panel
    Friend WithEvents pnlCancel As System.Windows.Forms.Panel
    Friend WithEvents cmnuSep2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OpenContainingFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents DeleteMovieToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuLock As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyExistingFanartToBackdropsFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnMarkAll As System.Windows.Forms.Button
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExportMoviesListToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetsManagerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ClearAllCachesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnFilterDown As System.Windows.Forms.Button
    Friend WithEvents btnFilterUp As System.Windows.Forms.Button
    Friend WithEvents tmrFilterAni As System.Windows.Forms.Timer
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents OfflineMediaManagerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAutoTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAskTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAutoTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMissAskTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAutoTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAskTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAutoTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAskTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CustomUpdaterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAutoMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuAllAskMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAutoMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuNewAskMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAutoMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuMarkAskMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents pbVType As System.Windows.Forms.PictureBox
    Friend WithEvents RenamerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuRefresh As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RefreshAllMoviesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GenresToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddGenreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetGenreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveGenreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GenreListToolStripComboBox As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents LblGenreStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbRefreshMedia As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents clbFilterGenres As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtFilterGenre As System.Windows.Forms.TextBox
    Friend WithEvents pnlFilterGenre As System.Windows.Forms.Panel
    Friend WithEvents lblGFilClose As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbSearch As System.Windows.Forms.ComboBox
    Friend WithEvents cbFilterYearMod As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbFilterYear As System.Windows.Forms.ComboBox
    Friend WithEvents gbSpecific As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnClearFilters As System.Windows.Forms.Button
    Friend WithEvents chkFilterLock As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilterMissing As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilterTolerance As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbFilterFileSource As System.Windows.Forms.ComboBox
    Friend WithEvents cmnuMetaData As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSortDate As System.Windows.Forms.Button
    Friend WithEvents pnlFilterSource As System.Windows.Forms.Panel
    Friend WithEvents lblSFilClose As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents clbFilterSource As System.Windows.Forms.CheckedListBox
    Friend WithEvents txtFilterSource As System.Windows.Forms.TextBox
    Friend WithEvents CurrentFilterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AutomaticForceBestMatchToolStripMenuItem2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AskRequireInputIfNoExactMatchToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAutoAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAutoNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAutoPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAutoFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAutoExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAutoTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAutoMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAskAll As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAskNfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAskPoster As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAskFanart As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAskExtra As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAskTrailer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFilterAskMI As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmuRenamer As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuRenameAuto As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuRenameManual As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnSortTitle As System.Windows.Forms.Button
    Friend WithEvents ToolTips As System.Windows.Forms.ToolTip
    Friend WithEvents btnIMDBRating As System.Windows.Forms.Button
    Friend WithEvents tabTV As System.Windows.Forms.TabPage
    Friend WithEvents dgvMediaList As System.Windows.Forms.DataGridView
    Friend WithEvents scTV As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents pnlListTop As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents dgvTVShows As System.Windows.Forms.DataGridView
    Friend WithEvents dgvTVSeasons As System.Windows.Forms.DataGridView
    Friend WithEvents dgvTVEpisodes As System.Windows.Forms.DataGridView
    Friend WithEvents CleanDatabaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFromDatabaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuMoviesUpdate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuTVShowUpdate As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DonateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrWaitShow As System.Windows.Forms.Timer
    Friend WithEvents tmrLoadShow As System.Windows.Forms.Timer
    Friend WithEvents tmrWaitSeason As System.Windows.Forms.Timer
    Friend WithEvents tmrLoadSeason As System.Windows.Forms.Timer
    Friend WithEvents tmrWaitEp As System.Windows.Forms.Timer
    Friend WithEvents tmrLoadEp As System.Windows.Forms.Timer
    Friend WithEvents tsSpring As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents mnuShows As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuSeasons As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuEpisodes As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmnuShowTitle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuEditShow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuEpTitle As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuEditEpisode As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents cmnuRescrapeShow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents cmnuChangeShow As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ModuleSettingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
