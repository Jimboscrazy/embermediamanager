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
        Me.BottomToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.TopToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.RightToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.LeftToolStripPanel = New System.Windows.Forms.ToolStripPanel
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.StatusStrip = New System.Windows.Forms.StatusStrip
        Me.tslStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.tslLoading = New System.Windows.Forms.ToolStripStatusLabel
        Me.tspbLoading = New System.Windows.Forms.ToolStripProgressBar
        Me.tmrAni = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip = New System.Windows.Forms.MenuStrip
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CleanFoldersToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRevertStudioTags = New System.Windows.Forms.ToolStripMenuItem
        Me.RenamerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.SetsManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OfflineMediaManagerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.ExportMoviesListToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.ClearAllCachesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RefreshAllMoviesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.scMain = New System.Windows.Forms.SplitContainer
        Me.dgvMediaList = New System.Windows.Forms.DataGridView
        Me.mnuMediaList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmnuTitle = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuRefresh = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuMark = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuLock = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuEditMovie = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuSep = New System.Windows.Forms.ToolStripSeparator
        Me.cmnuRescrape = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuSearchNew = New System.Windows.Forms.ToolStripMenuItem
        Me.cmnuSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.OpenContainingFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.DeleteMovieToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.pnlSearch = New System.Windows.Forms.Panel
        Me.picSearch = New System.Windows.Forms.PictureBox
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.btnMarkAll = New System.Windows.Forms.Button
        Me.tabsMain = New System.Windows.Forms.TabControl
        Me.tabMovies = New System.Windows.Forms.TabPage
        Me.pnlFilter = New System.Windows.Forms.Panel
        Me.cbFilterSource = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnFilterDown = New System.Windows.Forms.Button
        Me.btnFilterUp = New System.Windows.Forms.Button
        Me.chkFilterDupe = New System.Windows.Forms.CheckBox
        Me.rbFilterOr = New System.Windows.Forms.RadioButton
        Me.rbFilterAnd = New System.Windows.Forms.RadioButton
        Me.chkFilterMark = New System.Windows.Forms.CheckBox
        Me.lblFilter = New System.Windows.Forms.Label
        Me.chkFilterNew = New System.Windows.Forms.CheckBox
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
        Me.btnMIRefresh = New System.Windows.Forms.Button
        Me.lblMIHeader = New System.Windows.Forms.Label
        Me.txtMediaInfo = New System.Windows.Forms.TextBox
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
        Me.CustomUpdaterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tsbRefreshMedia = New System.Windows.Forms.ToolStripButton
        Me.tsbUpdateXBMC = New System.Windows.Forms.ToolStripSplitButton
        Me.pbFanartCache = New System.Windows.Forms.PictureBox
        Me.pbFanart = New System.Windows.Forms.PictureBox
        Me.ilColumnIcons = New System.Windows.Forms.ImageList(Me.components)
        Me.tmrWait = New System.Windows.Forms.Timer(Me.components)
        Me.tmrLoad = New System.Windows.Forms.Timer(Me.components)
        Me.tmrSearchWait = New System.Windows.Forms.Timer(Me.components)
        Me.tmrSearch = New System.Windows.Forms.Timer(Me.components)
        Me.tmrFilterAni = New System.Windows.Forms.Timer(Me.components)
        Me.StatusStrip.SuspendLayout()
        Me.MenuStrip.SuspendLayout()
        Me.scMain.Panel1.SuspendLayout()
        Me.scMain.Panel2.SuspendLayout()
        Me.scMain.SuspendLayout()
        CType(Me.dgvMediaList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuMediaList.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        CType(Me.picSearch, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabsMain.SuspendLayout()
        Me.pnlFilter.SuspendLayout()
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
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Image = CType(resources.GetObject("ExitToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingsToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Image = CType(resources.GetObject("SettingsToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(125, 22)
        Me.SettingsToolStripMenuItem.Text = "Settings..."
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Image = CType(resources.GetObject("AboutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.AboutToolStripMenuItem.Text = "About..."
        '
        'StatusStrip
        '
        Me.StatusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslStatus, Me.tslLoading, Me.tspbLoading})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 712)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1016, 22)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'tslStatus
        '
        Me.tslStatus.AutoSize = False
        Me.tslStatus.Name = "tslStatus"
        Me.tslStatus.Size = New System.Drawing.Size(525, 17)
        Me.tslStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tslLoading
        '
        Me.tslLoading.AutoSize = False
        Me.tslLoading.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tslLoading.Name = "tslLoading"
        Me.tslLoading.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
        Me.tslLoading.Size = New System.Drawing.Size(300, 17)
        Me.tslLoading.Text = "Loading Media:"
        Me.tslLoading.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.tslLoading.Visible = False
        '
        'tspbLoading
        '
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
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.EditToolStripMenuItem, Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1016, 24)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CleanFoldersToolStripMenuItem, Me.ConvertFileSourceToFolderSourceToolStripMenuItem, Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem, Me.mnuRevertStudioTags, Me.RenamerToolStripMenuItem, Me.ToolStripSeparator4, Me.SetsManagerToolStripMenuItem, Me.OfflineMediaManagerToolStripMenuItem, Me.ToolStripMenuItem3, Me.ExportMoviesListToolStripMenuItem, Me.ToolStripSeparator5, Me.ClearAllCachesToolStripMenuItem, Me.RefreshAllMoviesToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'CleanFoldersToolStripMenuItem
        '
        Me.CleanFoldersToolStripMenuItem.Image = CType(resources.GetObject("CleanFoldersToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CleanFoldersToolStripMenuItem.Name = "CleanFoldersToolStripMenuItem"
        Me.CleanFoldersToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.CleanFoldersToolStripMenuItem.Text = "Clean Files"
        '
        'ConvertFileSourceToFolderSourceToolStripMenuItem
        '
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Image = CType(resources.GetObject("ConvertFileSourceToFolderSourceToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Name = "ConvertFileSourceToFolderSourceToolStripMenuItem"
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.ConvertFileSourceToFolderSourceToolStripMenuItem.Text = "Sort Files Into Folders"
        '
        'CopyExistingFanartToBackdropsFolderToolStripMenuItem
        '
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Image = CType(resources.GetObject("CopyExistingFanartToBackdropsFolderToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Name = "CopyExistingFanartToBackdropsFolderToolStripMenuItem"
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Text = "Copy Existing Fanart To Backdrops Folder"
        '
        'mnuRevertStudioTags
        '
        Me.mnuRevertStudioTags.Image = CType(resources.GetObject("mnuRevertStudioTags.Image"), System.Drawing.Image)
        Me.mnuRevertStudioTags.Name = "mnuRevertStudioTags"
        Me.mnuRevertStudioTags.Size = New System.Drawing.Size(292, 22)
        Me.mnuRevertStudioTags.Text = "Revert Media Info Studio Tags"
        '
        'RenamerToolStripMenuItem
        '
        Me.RenamerToolStripMenuItem.Image = CType(resources.GetObject("RenamerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RenamerToolStripMenuItem.Name = "RenamerToolStripMenuItem"
        Me.RenamerToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.RenamerToolStripMenuItem.Text = "Bulk Renamer"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(289, 6)
        '
        'SetsManagerToolStripMenuItem
        '
        Me.SetsManagerToolStripMenuItem.Image = CType(resources.GetObject("SetsManagerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SetsManagerToolStripMenuItem.Name = "SetsManagerToolStripMenuItem"
        Me.SetsManagerToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.SetsManagerToolStripMenuItem.Text = "Sets Manager"
        '
        'OfflineMediaManagerToolStripMenuItem
        '
        Me.OfflineMediaManagerToolStripMenuItem.Image = CType(resources.GetObject("OfflineMediaManagerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OfflineMediaManagerToolStripMenuItem.Name = "OfflineMediaManagerToolStripMenuItem"
        Me.OfflineMediaManagerToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.OfflineMediaManagerToolStripMenuItem.Text = "Offline Media Manager"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(289, 6)
        '
        'ExportMoviesListToolStripMenuItem
        '
        Me.ExportMoviesListToolStripMenuItem.Image = CType(resources.GetObject("ExportMoviesListToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExportMoviesListToolStripMenuItem.Name = "ExportMoviesListToolStripMenuItem"
        Me.ExportMoviesListToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.ExportMoviesListToolStripMenuItem.Text = "Export Movies List"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(289, 6)
        '
        'ClearAllCachesToolStripMenuItem
        '
        Me.ClearAllCachesToolStripMenuItem.Image = CType(resources.GetObject("ClearAllCachesToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ClearAllCachesToolStripMenuItem.Name = "ClearAllCachesToolStripMenuItem"
        Me.ClearAllCachesToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.ClearAllCachesToolStripMenuItem.Text = "Clear All Caches"
        '
        'RefreshAllMoviesToolStripMenuItem
        '
        Me.RefreshAllMoviesToolStripMenuItem.Image = CType(resources.GetObject("RefreshAllMoviesToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RefreshAllMoviesToolStripMenuItem.Name = "RefreshAllMoviesToolStripMenuItem"
        Me.RefreshAllMoviesToolStripMenuItem.Size = New System.Drawing.Size(292, 22)
        Me.RefreshAllMoviesToolStripMenuItem.Text = "Reload All Movies"
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
        Me.scMain.Panel1.Controls.Add(Me.dgvMediaList)
        Me.scMain.Panel1.Controls.Add(Me.Panel1)
        Me.scMain.Panel1.Controls.Add(Me.pnlFilter)
        Me.scMain.Panel1.Margin = New System.Windows.Forms.Padding(3)
        Me.scMain.Panel1MinSize = 165
        '
        'scMain.Panel2
        '
        Me.scMain.Panel2.BackColor = System.Drawing.Color.Gainsboro
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
        Me.scMain.SplitterDistance = 308
        Me.scMain.TabIndex = 7
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
        Me.dgvMediaList.Size = New System.Drawing.Size(308, 547)
        Me.dgvMediaList.TabIndex = 10
        '
        'mnuMediaList
        '
        Me.mnuMediaList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmnuTitle, Me.ToolStripSeparator3, Me.cmnuRefresh, Me.cmnuMark, Me.cmnuLock, Me.ToolStripMenuItem1, Me.cmnuEditMovie, Me.cmnuSep, Me.cmnuRescrape, Me.cmnuSearchNew, Me.cmnuSep2, Me.OpenContainingFolderToolStripMenuItem, Me.ToolStripSeparator2, Me.DeleteMovieToolStripMenuItem})
        Me.mnuMediaList.Name = "mnuMediaList"
        Me.mnuMediaList.Size = New System.Drawing.Size(202, 232)
        '
        'cmnuTitle
        '
        Me.cmnuTitle.Enabled = False
        Me.cmnuTitle.Image = CType(resources.GetObject("cmnuTitle.Image"), System.Drawing.Image)
        Me.cmnuTitle.Name = "cmnuTitle"
        Me.cmnuTitle.Size = New System.Drawing.Size(201, 22)
        Me.cmnuTitle.Text = "Title"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(198, 6)
        '
        'cmnuRefresh
        '
        Me.cmnuRefresh.Image = CType(resources.GetObject("cmnuRefresh.Image"), System.Drawing.Image)
        Me.cmnuRefresh.Name = "cmnuRefresh"
        Me.cmnuRefresh.Size = New System.Drawing.Size(201, 22)
        Me.cmnuRefresh.Text = "Reload"
        '
        'cmnuMark
        '
        Me.cmnuMark.Image = CType(resources.GetObject("cmnuMark.Image"), System.Drawing.Image)
        Me.cmnuMark.Name = "cmnuMark"
        Me.cmnuMark.Size = New System.Drawing.Size(201, 22)
        Me.cmnuMark.Text = "Mark"
        '
        'cmnuLock
        '
        Me.cmnuLock.Image = CType(resources.GetObject("cmnuLock.Image"), System.Drawing.Image)
        Me.cmnuLock.Name = "cmnuLock"
        Me.cmnuLock.Size = New System.Drawing.Size(201, 22)
        Me.cmnuLock.Text = "Lock"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(198, 6)
        '
        'cmnuEditMovie
        '
        Me.cmnuEditMovie.Image = CType(resources.GetObject("cmnuEditMovie.Image"), System.Drawing.Image)
        Me.cmnuEditMovie.Name = "cmnuEditMovie"
        Me.cmnuEditMovie.Size = New System.Drawing.Size(201, 22)
        Me.cmnuEditMovie.Text = "Edit Movie"
        '
        'cmnuSep
        '
        Me.cmnuSep.Name = "cmnuSep"
        Me.cmnuSep.Size = New System.Drawing.Size(198, 6)
        '
        'cmnuRescrape
        '
        Me.cmnuRescrape.Image = CType(resources.GetObject("cmnuRescrape.Image"), System.Drawing.Image)
        Me.cmnuRescrape.Name = "cmnuRescrape"
        Me.cmnuRescrape.Size = New System.Drawing.Size(201, 22)
        Me.cmnuRescrape.Text = "Re-scrape IMDB"
        '
        'cmnuSearchNew
        '
        Me.cmnuSearchNew.Image = CType(resources.GetObject("cmnuSearchNew.Image"), System.Drawing.Image)
        Me.cmnuSearchNew.Name = "cmnuSearchNew"
        Me.cmnuSearchNew.Size = New System.Drawing.Size(201, 22)
        Me.cmnuSearchNew.Text = "Change Movie"
        '
        'cmnuSep2
        '
        Me.cmnuSep2.Name = "cmnuSep2"
        Me.cmnuSep2.Size = New System.Drawing.Size(198, 6)
        '
        'OpenContainingFolderToolStripMenuItem
        '
        Me.OpenContainingFolderToolStripMenuItem.Image = CType(resources.GetObject("OpenContainingFolderToolStripMenuItem.Image"), System.Drawing.Image)
        Me.OpenContainingFolderToolStripMenuItem.Name = "OpenContainingFolderToolStripMenuItem"
        Me.OpenContainingFolderToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.OpenContainingFolderToolStripMenuItem.Text = "Open Containing Folder"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(198, 6)
        '
        'DeleteMovieToolStripMenuItem
        '
        Me.DeleteMovieToolStripMenuItem.Image = CType(resources.GetObject("DeleteMovieToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteMovieToolStripMenuItem.Name = "DeleteMovieToolStripMenuItem"
        Me.DeleteMovieToolStripMenuItem.Size = New System.Drawing.Size(201, 22)
        Me.DeleteMovieToolStripMenuItem.Text = "Delete Movie"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.pnlSearch)
        Me.Panel1.Controls.Add(Me.btnMarkAll)
        Me.Panel1.Controls.Add(Me.tabsMain)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(308, 56)
        Me.Panel1.TabIndex = 14
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSearch.Controls.Add(Me.picSearch)
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Location = New System.Drawing.Point(0, 23)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(308, 33)
        Me.pnlSearch.TabIndex = 11
        '
        'picSearch
        '
        Me.picSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picSearch.Image = CType(resources.GetObject("picSearch.Image"), System.Drawing.Image)
        Me.picSearch.Location = New System.Drawing.Point(284, 8)
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
        Me.txtSearch.Size = New System.Drawing.Size(270, 20)
        Me.txtSearch.TabIndex = 0
        '
        'btnMarkAll
        '
        Me.btnMarkAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMarkAll.Image = CType(resources.GetObject("btnMarkAll.Image"), System.Drawing.Image)
        Me.btnMarkAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMarkAll.Location = New System.Drawing.Point(226, 1)
        Me.btnMarkAll.Name = "btnMarkAll"
        Me.btnMarkAll.Size = New System.Drawing.Size(81, 21)
        Me.btnMarkAll.TabIndex = 13
        Me.btnMarkAll.Text = "Mark All"
        Me.btnMarkAll.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMarkAll.UseVisualStyleBackColor = True
        '
        'tabsMain
        '
        Me.tabsMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabsMain.Controls.Add(Me.tabMovies)
        Me.tabsMain.Location = New System.Drawing.Point(3, 3)
        Me.tabsMain.Name = "tabsMain"
        Me.tabsMain.SelectedIndex = 0
        Me.tabsMain.Size = New System.Drawing.Size(306, 35)
        Me.tabsMain.TabIndex = 8
        '
        'tabMovies
        '
        Me.tabMovies.Location = New System.Drawing.Point(4, 22)
        Me.tabMovies.Name = "tabMovies"
        Me.tabMovies.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMovies.Size = New System.Drawing.Size(298, 9)
        Me.tabMovies.TabIndex = 0
        Me.tabMovies.Text = "Movies"
        Me.tabMovies.UseVisualStyleBackColor = True
        '
        'pnlFilter
        '
        Me.pnlFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlFilter.Controls.Add(Me.cbFilterSource)
        Me.pnlFilter.Controls.Add(Me.Label2)
        Me.pnlFilter.Controls.Add(Me.btnFilterDown)
        Me.pnlFilter.Controls.Add(Me.btnFilterUp)
        Me.pnlFilter.Controls.Add(Me.chkFilterDupe)
        Me.pnlFilter.Controls.Add(Me.rbFilterOr)
        Me.pnlFilter.Controls.Add(Me.rbFilterAnd)
        Me.pnlFilter.Controls.Add(Me.chkFilterMark)
        Me.pnlFilter.Controls.Add(Me.lblFilter)
        Me.pnlFilter.Controls.Add(Me.chkFilterNew)
        Me.pnlFilter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFilter.Location = New System.Drawing.Point(0, 603)
        Me.pnlFilter.Name = "pnlFilter"
        Me.pnlFilter.Size = New System.Drawing.Size(308, 85)
        Me.pnlFilter.TabIndex = 12
        '
        'cbFilterSource
        '
        Me.cbFilterSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFilterSource.FormattingEnabled = True
        Me.cbFilterSource.Location = New System.Drawing.Point(136, 60)
        Me.cbFilterSource.Name = "cbFilterSource"
        Me.cbFilterSource.Size = New System.Drawing.Size(166, 21)
        Me.cbFilterSource.TabIndex = 30
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(92, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 13)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Source:"
        '
        'btnFilterDown
        '
        Me.btnFilterDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterDown.BackColor = System.Drawing.SystemColors.Control
        Me.btnFilterDown.Enabled = False
        Me.btnFilterDown.Location = New System.Drawing.Point(268, 1)
        Me.btnFilterDown.Name = "btnFilterDown"
        Me.btnFilterDown.Size = New System.Drawing.Size(30, 22)
        Me.btnFilterDown.TabIndex = 28
        Me.btnFilterDown.Text = "v"
        Me.btnFilterDown.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnFilterDown.UseVisualStyleBackColor = False
        '
        'btnFilterUp
        '
        Me.btnFilterUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFilterUp.BackColor = System.Drawing.SystemColors.Control
        Me.btnFilterUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFilterUp.Location = New System.Drawing.Point(236, 1)
        Me.btnFilterUp.Name = "btnFilterUp"
        Me.btnFilterUp.Size = New System.Drawing.Size(30, 22)
        Me.btnFilterUp.TabIndex = 27
        Me.btnFilterUp.Text = "^"
        Me.btnFilterUp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnFilterUp.UseVisualStyleBackColor = False
        '
        'chkFilterDupe
        '
        Me.chkFilterDupe.AutoSize = True
        Me.chkFilterDupe.Location = New System.Drawing.Point(7, 23)
        Me.chkFilterDupe.Name = "chkFilterDupe"
        Me.chkFilterDupe.Size = New System.Drawing.Size(108, 17)
        Me.chkFilterDupe.TabIndex = 26
        Me.chkFilterDupe.Text = "Duplicate Movies"
        Me.chkFilterDupe.UseVisualStyleBackColor = True
        '
        'rbFilterOr
        '
        Me.rbFilterOr.AutoSize = True
        Me.rbFilterOr.Location = New System.Drawing.Point(258, 40)
        Me.rbFilterOr.Name = "rbFilterOr"
        Me.rbFilterOr.Size = New System.Drawing.Size(36, 17)
        Me.rbFilterOr.TabIndex = 25
        Me.rbFilterOr.Text = "Or"
        Me.rbFilterOr.UseVisualStyleBackColor = True
        '
        'rbFilterAnd
        '
        Me.rbFilterAnd.AutoSize = True
        Me.rbFilterAnd.Checked = True
        Me.rbFilterAnd.Location = New System.Drawing.Point(258, 22)
        Me.rbFilterAnd.Name = "rbFilterAnd"
        Me.rbFilterAnd.Size = New System.Drawing.Size(44, 17)
        Me.rbFilterAnd.TabIndex = 24
        Me.rbFilterAnd.TabStop = True
        Me.rbFilterAnd.Text = "And"
        Me.rbFilterAnd.UseVisualStyleBackColor = True
        '
        'chkFilterMark
        '
        Me.chkFilterMark.AutoSize = True
        Me.chkFilterMark.Location = New System.Drawing.Point(136, 41)
        Me.chkFilterMark.Name = "chkFilterMark"
        Me.chkFilterMark.Size = New System.Drawing.Size(99, 17)
        Me.chkFilterMark.TabIndex = 23
        Me.chkFilterMark.Text = "Marked Movies"
        Me.chkFilterMark.UseVisualStyleBackColor = True
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
        Me.lblFilter.Size = New System.Drawing.Size(298, 17)
        Me.lblFilter.TabIndex = 22
        Me.lblFilter.Text = "Filters"
        Me.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkFilterNew
        '
        Me.chkFilterNew.AutoSize = True
        Me.chkFilterNew.Location = New System.Drawing.Point(136, 23)
        Me.chkFilterNew.Name = "chkFilterNew"
        Me.chkFilterNew.Size = New System.Drawing.Size(85, 17)
        Me.chkFilterNew.TabIndex = 1
        Me.chkFilterNew.Text = "New Movies"
        Me.chkFilterNew.UseVisualStyleBackColor = True
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
        Me.pnlInfoPanel.Controls.Add(Me.btnMIRefresh)
        Me.pnlInfoPanel.Controls.Add(Me.lblMIHeader)
        Me.pnlInfoPanel.Controls.Add(Me.txtMediaInfo)
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
        Me.pnlInfoPanel.Size = New System.Drawing.Size(704, 342)
        Me.pnlInfoPanel.TabIndex = 10
        '
        'txtCerts
        '
        Me.txtCerts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCerts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCerts.Location = New System.Drawing.Point(116, 208)
        Me.txtCerts.Name = "txtCerts"
        Me.txtCerts.Size = New System.Drawing.Size(269, 20)
        Me.txtCerts.TabIndex = 41
        '
        'lblCertsHeader
        '
        Me.lblCertsHeader.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCertsHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblCertsHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCertsHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCertsHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblCertsHeader.Location = New System.Drawing.Point(116, 188)
        Me.lblCertsHeader.Name = "lblCertsHeader"
        Me.lblCertsHeader.Size = New System.Drawing.Size(269, 17)
        Me.lblCertsHeader.TabIndex = 40
        Me.lblCertsHeader.Text = "Certifications"
        Me.lblCertsHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblReleaseDate
        '
        Me.lblReleaseDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblReleaseDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReleaseDate.ForeColor = System.Drawing.Color.Black
        Me.lblReleaseDate.Location = New System.Drawing.Point(187, 48)
        Me.lblReleaseDate.Name = "lblReleaseDate"
        Me.lblReleaseDate.Size = New System.Drawing.Size(105, 16)
        Me.lblReleaseDate.TabIndex = 39
        Me.lblReleaseDate.Text = "Release"
        Me.lblReleaseDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblReleaseDateHeader
        '
        Me.lblReleaseDateHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblReleaseDateHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblReleaseDateHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblReleaseDateHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReleaseDateHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblReleaseDateHeader.Location = New System.Drawing.Point(188, 27)
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
        Me.btnMid.Location = New System.Drawing.Point(633, 1)
        Me.btnMid.Name = "btnMid"
        Me.btnMid.Size = New System.Drawing.Size(30, 22)
        Me.btnMid.TabIndex = 37
        Me.btnMid.Text = "-"
        Me.btnMid.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnMid.UseVisualStyleBackColor = False
        '
        'pbMILoading
        '
        Me.pbMILoading.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbMILoading.Image = CType(resources.GetObject("pbMILoading.Image"), System.Drawing.Image)
        Me.pbMILoading.Location = New System.Drawing.Point(535, 374)
        Me.pbMILoading.Name = "pbMILoading"
        Me.pbMILoading.Size = New System.Drawing.Size(41, 39)
        Me.pbMILoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbMILoading.TabIndex = 36
        Me.pbMILoading.TabStop = False
        Me.pbMILoading.Visible = False
        '
        'btnMIRefresh
        '
        Me.btnMIRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMIRefresh.Location = New System.Drawing.Point(623, 278)
        Me.btnMIRefresh.Name = "btnMIRefresh"
        Me.btnMIRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnMIRefresh.TabIndex = 34
        Me.btnMIRefresh.Text = "Refresh"
        Me.btnMIRefresh.UseVisualStyleBackColor = True
        '
        'lblMIHeader
        '
        Me.lblMIHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMIHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblMIHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblMIHeader.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMIHeader.ForeColor = System.Drawing.SystemColors.HighlightText
        Me.lblMIHeader.Location = New System.Drawing.Point(399, 282)
        Me.lblMIHeader.Name = "lblMIHeader"
        Me.lblMIHeader.Size = New System.Drawing.Size(294, 17)
        Me.lblMIHeader.TabIndex = 35
        Me.lblMIHeader.Text = "Media Info"
        Me.lblMIHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtMediaInfo
        '
        Me.txtMediaInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMediaInfo.BackColor = System.Drawing.Color.Gainsboro
        Me.txtMediaInfo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMediaInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMediaInfo.ForeColor = System.Drawing.Color.Black
        Me.txtMediaInfo.Location = New System.Drawing.Point(401, 303)
        Me.txtMediaInfo.Multiline = True
        Me.txtMediaInfo.Name = "txtMediaInfo"
        Me.txtMediaInfo.ReadOnly = True
        Me.txtMediaInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMediaInfo.Size = New System.Drawing.Size(296, 184)
        Me.txtMediaInfo.TabIndex = 33
        Me.txtMediaInfo.TabStop = False
        '
        'btnPlay
        '
        Me.btnPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPlay.Image = Global.Ember_Media_Manager.My.Resources.Resources.Play_Icon
        Me.btnPlay.Location = New System.Drawing.Point(365, 254)
        Me.btnPlay.Name = "btnPlay"
        Me.btnPlay.Size = New System.Drawing.Size(20, 20)
        Me.btnPlay.TabIndex = 32
        Me.btnPlay.UseVisualStyleBackColor = True
        '
        'txtFilePath
        '
        Me.txtFilePath.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilePath.Location = New System.Drawing.Point(3, 254)
        Me.txtFilePath.Name = "txtFilePath"
        Me.txtFilePath.Size = New System.Drawing.Size(360, 20)
        Me.txtFilePath.TabIndex = 31
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
        Me.lblFilePathHeader.Size = New System.Drawing.Size(381, 17)
        Me.lblFilePathHeader.TabIndex = 30
        Me.lblFilePathHeader.Text = "File Path"
        Me.lblFilePathHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtIMDBID
        '
        Me.txtIMDBID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIMDBID.Location = New System.Drawing.Point(3, 208)
        Me.txtIMDBID.Name = "txtIMDBID"
        Me.txtIMDBID.Size = New System.Drawing.Size(108, 20)
        Me.txtIMDBID.TabIndex = 29
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
        Me.lblDirector.Location = New System.Drawing.Point(2, 48)
        Me.lblDirector.Name = "lblDirector"
        Me.lblDirector.Size = New System.Drawing.Size(180, 16)
        Me.lblDirector.TabIndex = 27
        Me.lblDirector.Text = "Director"
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
        Me.lblDirectorHeader.Size = New System.Drawing.Size(179, 17)
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
        Me.pnlActors.Location = New System.Drawing.Point(397, 29)
        Me.pnlActors.Name = "pnlActors"
        Me.pnlActors.Size = New System.Drawing.Size(302, 244)
        Me.pnlActors.TabIndex = 19
        '
        'pbActLoad
        '
        Me.pbActLoad.Image = CType(resources.GetObject("pbActLoad.Image"), System.Drawing.Image)
        Me.pbActLoad.Location = New System.Drawing.Point(241, 109)
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
        Me.lblOutlineHeader.Size = New System.Drawing.Size(382, 17)
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
        Me.txtOutline.Size = New System.Drawing.Size(382, 78)
        Me.txtOutline.TabIndex = 16
        Me.txtOutline.Text = "Blah Blah Blah Plot Outline"
        '
        'pnlTop250
        '
        Me.pnlTop250.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTop250.Controls.Add(Me.lblTop250)
        Me.pnlTop250.Controls.Add(Me.pbTop250)
        Me.pnlTop250.Location = New System.Drawing.Point(318, 28)
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
        Me.lblPlotHeader.Size = New System.Drawing.Size(388, 17)
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
        Me.txtPlot.Location = New System.Drawing.Point(5, 303)
        Me.txtPlot.Multiline = True
        Me.txtPlot.Name = "txtPlot"
        Me.txtPlot.ReadOnly = True
        Me.txtPlot.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPlot.Size = New System.Drawing.Size(386, 184)
        Me.txtPlot.TabIndex = 7
        '
        'btnDown
        '
        Me.btnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDown.BackColor = System.Drawing.SystemColors.Control
        Me.btnDown.Location = New System.Drawing.Point(665, 1)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(30, 22)
        Me.btnDown.TabIndex = 6
        Me.btnDown.Text = "v"
        Me.btnDown.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnDown.UseVisualStyleBackColor = False
        '
        'btnUp
        '
        Me.btnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnUp.BackColor = System.Drawing.SystemColors.Control
        Me.btnUp.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUp.Location = New System.Drawing.Point(601, 1)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(30, 22)
        Me.btnUp.TabIndex = 1
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
        Me.lblInfoPanelHeader.Size = New System.Drawing.Size(696, 17)
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
        Me.pnlTop.Size = New System.Drawing.Size(704, 74)
        Me.pnlTop.TabIndex = 9
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.Black
        Me.lblTitle.Location = New System.Drawing.Point(4, 2)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(151, 20)
        Me.lblTitle.TabIndex = 25
        Me.lblTitle.Text = "Movie Title (2009)"
        Me.lblTitle.UseMnemonic = False
        '
        'pnlInfoIcons
        '
        Me.pnlInfoIcons.Controls.Add(Me.pbVType)
        Me.pnlInfoIcons.Controls.Add(Me.pbStudio)
        Me.pnlInfoIcons.Controls.Add(Me.pbVideo)
        Me.pnlInfoIcons.Controls.Add(Me.pbAudio)
        Me.pnlInfoIcons.Controls.Add(Me.pbResolution)
        Me.pnlInfoIcons.Controls.Add(Me.pbChannels)
        Me.pnlInfoIcons.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlInfoIcons.Location = New System.Drawing.Point(312, 0)
        Me.pnlInfoIcons.Name = "pnlInfoIcons"
        Me.pnlInfoIcons.Size = New System.Drawing.Size(390, 72)
        Me.pnlInfoIcons.TabIndex = 31
        '
        'pbVType
        '
        Me.pbVType.BackColor = System.Drawing.Color.Gainsboro
        Me.pbVType.Location = New System.Drawing.Point(65, 15)
        Me.pbVType.Name = "pbVType"
        Me.pbVType.Size = New System.Drawing.Size(64, 44)
        Me.pbVType.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbVType.TabIndex = 36
        Me.pbVType.TabStop = False
        '
        'pbStudio
        '
        Me.pbStudio.BackColor = System.Drawing.Color.Gainsboro
        Me.pbStudio.Location = New System.Drawing.Point(325, 15)
        Me.pbStudio.Name = "pbStudio"
        Me.pbStudio.Size = New System.Drawing.Size(64, 44)
        Me.pbStudio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbStudio.TabIndex = 31
        Me.pbStudio.TabStop = False
        '
        'pbVideo
        '
        Me.pbVideo.BackColor = System.Drawing.Color.Gainsboro
        Me.pbVideo.Location = New System.Drawing.Point(0, 15)
        Me.pbVideo.Name = "pbVideo"
        Me.pbVideo.Size = New System.Drawing.Size(64, 44)
        Me.pbVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbVideo.TabIndex = 33
        Me.pbVideo.TabStop = False
        '
        'pbAudio
        '
        Me.pbAudio.BackColor = System.Drawing.Color.Gainsboro
        Me.pbAudio.Location = New System.Drawing.Point(195, 15)
        Me.pbAudio.Name = "pbAudio"
        Me.pbAudio.Size = New System.Drawing.Size(64, 44)
        Me.pbAudio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbAudio.TabIndex = 35
        Me.pbAudio.TabStop = False
        '
        'pbResolution
        '
        Me.pbResolution.BackColor = System.Drawing.Color.Gainsboro
        Me.pbResolution.Location = New System.Drawing.Point(130, 15)
        Me.pbResolution.Name = "pbResolution"
        Me.pbResolution.Size = New System.Drawing.Size(64, 44)
        Me.pbResolution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbResolution.TabIndex = 34
        Me.pbResolution.TabStop = False
        '
        'pbChannels
        '
        Me.pbChannels.BackColor = System.Drawing.Color.Gainsboro
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
        Me.lblRuntime.Size = New System.Drawing.Size(94, 13)
        Me.lblRuntime.TabIndex = 32
        Me.lblRuntime.Text = "Runtime: ###mins"
        Me.lblRuntime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblTagline
        '
        Me.lblTagline.AutoSize = True
        Me.lblTagline.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTagline.ForeColor = System.Drawing.Color.Black
        Me.lblTagline.Location = New System.Drawing.Point(5, 55)
        Me.lblTagline.Name = "lblTagline"
        Me.lblTagline.Size = New System.Drawing.Size(98, 13)
        Me.lblTagline.TabIndex = 26
        Me.lblTagline.Text = """Just some tagline"""
        Me.lblTagline.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblTagline.UseMnemonic = False
        '
        'pnlRating
        '
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
        Me.lblVotes.Size = New System.Drawing.Size(79, 13)
        Me.lblVotes.TabIndex = 22
        Me.lblVotes.Text = "###### Votes"
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
        Me.tsMain.Size = New System.Drawing.Size(704, 25)
        Me.tsMain.Stretch = True
        Me.tsMain.TabIndex = 6
        '
        'tsbAutoPilot
        '
        Me.tsbAutoPilot.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FullToolStripMenuItem, Me.UpdateOnlyToolStripMenuItem, Me.NewMoviesToolStripMenuItem, Me.MarkedMoviesToolStripMenuItem, Me.CustomUpdaterToolStripMenuItem})
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
        Me.mnuAllAutoAll.Size = New System.Drawing.Size(174, 22)
        Me.mnuAllAutoAll.Text = "All Items"
        '
        'mnuAllAutoNfo
        '
        Me.mnuAllAutoNfo.Name = "mnuAllAutoNfo"
        Me.mnuAllAutoNfo.Size = New System.Drawing.Size(174, 22)
        Me.mnuAllAutoNfo.Text = "NFO Only"
        '
        'mnuAllAutoPoster
        '
        Me.mnuAllAutoPoster.Name = "mnuAllAutoPoster"
        Me.mnuAllAutoPoster.Size = New System.Drawing.Size(174, 22)
        Me.mnuAllAutoPoster.Text = "Posters Only"
        '
        'mnuAllAutoFanart
        '
        Me.mnuAllAutoFanart.Name = "mnuAllAutoFanart"
        Me.mnuAllAutoFanart.Size = New System.Drawing.Size(174, 22)
        Me.mnuAllAutoFanart.Text = "Fanart Only"
        '
        'mnuAllAutoExtra
        '
        Me.mnuAllAutoExtra.Name = "mnuAllAutoExtra"
        Me.mnuAllAutoExtra.Size = New System.Drawing.Size(174, 22)
        Me.mnuAllAutoExtra.Text = "Extra Thumbs Only"
        '
        'mnuAllAutoTrailer
        '
        Me.mnuAllAutoTrailer.Name = "mnuAllAutoTrailer"
        Me.mnuAllAutoTrailer.Size = New System.Drawing.Size(174, 22)
        Me.mnuAllAutoTrailer.Text = "Trailer Only"
        '
        'mnuAllAutoMI
        '
        Me.mnuAllAutoMI.Name = "mnuAllAutoMI"
        Me.mnuAllAutoMI.Size = New System.Drawing.Size(174, 22)
        Me.mnuAllAutoMI.Text = "Media Tags Only"
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
        Me.mnuAllAskPoster.Text = "Posters Only"
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
        Me.mnuAllAskMI.Text = "Media Tags Only"
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
        Me.mnuMissAutoPoster.Text = "Posters Only"
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
        Me.mnuMissAskPoster.Text = "Posters Only"
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
        Me.mnuNewAutoPoster.Text = "Posters Only"
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
        Me.mnuNewAutoMI.Text = "Media Tags Only"
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
        Me.mnuNewAskPoster.Text = "Posters Only"
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
        Me.mnuNewAskMI.Text = "Media Tags Only"
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
        Me.mnuMarkAutoPoster.Text = "Posters Only"
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
        Me.mnuMarkAutoMI.Text = "Media Tags Only"
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
        Me.mnuMarkAskPoster.Text = "Posters Only"
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
        Me.mnuMarkAskMI.Text = "Media Tags Only"
        '
        'CustomUpdaterToolStripMenuItem
        '
        Me.CustomUpdaterToolStripMenuItem.Name = "CustomUpdaterToolStripMenuItem"
        Me.CustomUpdaterToolStripMenuItem.Size = New System.Drawing.Size(188, 22)
        Me.CustomUpdaterToolStripMenuItem.Text = "Custom Updater..."
        '
        'tsbRefreshMedia
        '
        Me.tsbRefreshMedia.Image = CType(resources.GetObject("tsbRefreshMedia.Image"), System.Drawing.Image)
        Me.tsbRefreshMedia.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRefreshMedia.Name = "tsbRefreshMedia"
        Me.tsbRefreshMedia.Size = New System.Drawing.Size(104, 22)
        Me.tsbRefreshMedia.Text = "Update Library"
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
        Me.pbFanart.Location = New System.Drawing.Point(4, 99)
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
        CType(Me.dgvMediaList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuMediaList.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        CType(Me.picSearch, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabsMain.ResumeLayout(False)
        Me.pnlFilter.ResumeLayout(False)
        Me.pnlFilter.PerformLayout()
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
    Friend WithEvents tsbRefreshMedia As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnMIRefresh As System.Windows.Forms.Button
    Friend WithEvents lblMIHeader As System.Windows.Forms.Label
    Friend WithEvents txtMediaInfo As System.Windows.Forms.TextBox
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
    Friend WithEvents dgvMediaList As System.Windows.Forms.DataGridView
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents picSearch As System.Windows.Forms.PictureBox
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents tmrSearchWait As System.Windows.Forms.Timer
    Friend WithEvents tmrSearch As System.Windows.Forms.Timer
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
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
    Friend WithEvents cbFilterSource As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents mnuRevertStudioTags As System.Windows.Forms.ToolStripMenuItem
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
End Class
