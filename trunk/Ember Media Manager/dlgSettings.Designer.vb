<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgSettings))
        Dim TreeNode8 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("File System", 4, 4)
        Dim TreeNode9 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("XBMC Communication", 1, 1)
        Dim TreeNode10 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("General", 0, 0, New System.Windows.Forms.TreeNode() {TreeNode8, TreeNode9})
        Dim TreeNode11 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Files and Sources", 5, 5)
        Dim TreeNode12 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Scraper - Data", 3, 3)
        Dim TreeNode13 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Scraper - Images", 6, 6)
        Dim TreeNode14 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Movies", 2, 2, New System.Windows.Forms.TreeNode() {TreeNode11, TreeNode12, TreeNode13})
        Me.GroupBox11 = New System.Windows.Forms.GroupBox
        Me.btnEditCom = New System.Windows.Forms.Button
        Me.txtName = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.txtPassword = New System.Windows.Forms.TextBox
        Me.btnAddCom = New System.Windows.Forms.Button
        Me.txtUsername = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.txtIP = New System.Windows.Forms.TextBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.GroupBox29 = New System.Windows.Forms.GroupBox
        Me.Label36 = New System.Windows.Forms.Label
        Me.cbMovieTheme = New System.Windows.Forms.ComboBox
        Me.Label35 = New System.Windows.Forms.Label
        Me.btnDLTrans = New System.Windows.Forms.Button
        Me.Label32 = New System.Windows.Forms.Label
        Me.cbIntLang = New System.Windows.Forms.ComboBox
        Me.chkInfoPanelAnim = New System.Windows.Forms.CheckBox
        Me.chkUpdates = New System.Windows.Forms.CheckBox
        Me.chkOverwriteNfo = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkLogErrors = New System.Windows.Forms.CheckBox
        Me.cbLanguages = New System.Windows.Forms.ComboBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.tcCleaner = New System.Windows.Forms.TabControl
        Me.tpStandard = New System.Windows.Forms.TabPage
        Me.chkCleanFolderJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanExtrathumbs = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieTBN = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieNameJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieTBNb = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanPosterJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanPosterTBN = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieNFO = New System.Windows.Forms.CheckBox
        Me.chkCleanDotFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieNFOb = New System.Windows.Forms.CheckBox
        Me.tpExpert = New System.Windows.Forms.TabPage
        Me.chkWhitelistVideo = New System.Windows.Forms.CheckBox
        Me.Label27 = New System.Windows.Forms.Label
        Me.btnRemoveWhitelist = New System.Windows.Forms.Button
        Me.btnAddWhitelist = New System.Windows.Forms.Button
        Me.txtWhitelist = New System.Windows.Forms.TextBox
        Me.lstWhitelist = New System.Windows.Forms.ListBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.gbFilters = New System.Windows.Forms.GroupBox
        Me.btnDown = New System.Windows.Forms.Button
        Me.btnUp = New System.Windows.Forms.Button
        Me.chkProperCase = New System.Windows.Forms.CheckBox
        Me.btnRemoveFilter = New System.Windows.Forms.Button
        Me.btnAddFilter = New System.Windows.Forms.Button
        Me.txtFilter = New System.Windows.Forms.TextBox
        Me.lstFilters = New System.Windows.Forms.ListBox
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.lbGenre = New System.Windows.Forms.CheckedListBox
        Me.chkShowGenresText = New System.Windows.Forms.CheckBox
        Me.lblGenre = New System.Windows.Forms.Label
        Me.chkNoDisplayFanart = New System.Windows.Forms.CheckBox
        Me.chkNoDisplayPoster = New System.Windows.Forms.CheckBox
        Me.chkShowDims = New System.Windows.Forms.CheckBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkMarkNew = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.txtCheckTitleTol = New System.Windows.Forms.TextBox
        Me.Label30 = New System.Windows.Forms.Label
        Me.chkCheckTitles = New System.Windows.Forms.CheckBox
        Me.GroupBox25 = New System.Windows.Forms.GroupBox
        Me.btnRemoveToken = New System.Windows.Forms.Button
        Me.btnAddToken = New System.Windows.Forms.Button
        Me.txtSortToken = New System.Windows.Forms.TextBox
        Me.lstSortTokens = New System.Windows.Forms.ListBox
        Me.chkDisplayYear = New System.Windows.Forms.CheckBox
        Me.chkMovieExtraCol = New System.Windows.Forms.CheckBox
        Me.chkMovieSubCol = New System.Windows.Forms.CheckBox
        Me.chkMovieTrailerCol = New System.Windows.Forms.CheckBox
        Me.chkMovieInfoCol = New System.Windows.Forms.CheckBox
        Me.chkMovieFanartCol = New System.Windows.Forms.CheckBox
        Me.chkMoviePosterCol = New System.Windows.Forms.CheckBox
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.chkMovieNameMultiOnly = New System.Windows.Forms.CheckBox
        Me.GroupBox21 = New System.Windows.Forms.GroupBox
        Me.rbBracketTrailer = New System.Windows.Forms.RadioButton
        Me.rbDashTrailer = New System.Windows.Forms.RadioButton
        Me.chkVideoTSParent = New System.Windows.Forms.CheckBox
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.chkMovieNameNFO = New System.Windows.Forms.CheckBox
        Me.chkMovieNFO = New System.Windows.Forms.CheckBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.chkMovieNameDotFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkMovieNameFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkFanartJPG = New System.Windows.Forms.CheckBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.chkFolderJPG = New System.Windows.Forms.CheckBox
        Me.chkPosterJPG = New System.Windows.Forms.CheckBox
        Me.chkPosterTBN = New System.Windows.Forms.CheckBox
        Me.chkMovieNameJPG = New System.Windows.Forms.CheckBox
        Me.chkMovieJPG = New System.Windows.Forms.CheckBox
        Me.chkMovieNameTBN = New System.Windows.Forms.CheckBox
        Me.chkMovieTBN = New System.Windows.Forms.CheckBox
        Me.lvMovies = New System.Windows.Forms.ListView
        Me.colID = New System.Windows.Forms.ColumnHeader
        Me.colName = New System.Windows.Forms.ColumnHeader
        Me.colPath = New System.Windows.Forms.ColumnHeader
        Me.colRecur = New System.Windows.Forms.ColumnHeader
        Me.colFolder = New System.Windows.Forms.ColumnHeader
        Me.colSingle = New System.Windows.Forms.ColumnHeader
        Me.btnMovieRem = New System.Windows.Forms.Button
        Me.btnMovieAddFolder = New System.Windows.Forms.Button
        Me.GroupBox15 = New System.Windows.Forms.GroupBox
        Me.chkOFDBGenre = New System.Windows.Forms.CheckBox
        Me.chkOFDBPlot = New System.Windows.Forms.CheckBox
        Me.chkOFDBOutline = New System.Windows.Forms.CheckBox
        Me.chkOFDBTitle = New System.Windows.Forms.CheckBox
        Me.GroupBox14 = New System.Windows.Forms.GroupBox
        Me.txtPosterWidth = New System.Windows.Forms.TextBox
        Me.txtPosterHeight = New System.Windows.Forms.TextBox
        Me.lblPosterQual = New System.Windows.Forms.Label
        Me.tbPosterQual = New System.Windows.Forms.TrackBar
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.chkResizePoster = New System.Windows.Forms.CheckBox
        Me.lblPosterSize = New System.Windows.Forms.Label
        Me.cbPosterSize = New System.Windows.Forms.ComboBox
        Me.chkOverwritePoster = New System.Windows.Forms.CheckBox
        Me.GroupBox13 = New System.Windows.Forms.GroupBox
        Me.txtFanartWidth = New System.Windows.Forms.TextBox
        Me.txtFanartHeight = New System.Windows.Forms.TextBox
        Me.chkFanartOnly = New System.Windows.Forms.CheckBox
        Me.lblFanartQual = New System.Windows.Forms.Label
        Me.tbFanartQual = New System.Windows.Forms.TrackBar
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.chkResizeFanart = New System.Windows.Forms.CheckBox
        Me.cbFanartSize = New System.Windows.Forms.ComboBox
        Me.lblFanartSize = New System.Windows.Forms.Label
        Me.chkOverwriteFanart = New System.Windows.Forms.CheckBox
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.chkLockOutline = New System.Windows.Forms.CheckBox
        Me.chkLockPlot = New System.Windows.Forms.CheckBox
        Me.chkLockTrailer = New System.Windows.Forms.CheckBox
        Me.chkLockGenre = New System.Windows.Forms.CheckBox
        Me.chkLockRealStudio = New System.Windows.Forms.CheckBox
        Me.chkLockRating = New System.Windows.Forms.CheckBox
        Me.chkLockTagline = New System.Windows.Forms.CheckBox
        Me.chkLockTitle = New System.Windows.Forms.CheckBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.chkNoSaveImagesToNfo = New System.Windows.Forms.CheckBox
        Me.chkSingleScrapeImages = New System.Windows.Forms.CheckBox
        Me.chkUseMPDB = New System.Windows.Forms.CheckBox
        Me.chkUseTMDB = New System.Windows.Forms.CheckBox
        Me.chkUseIMPA = New System.Windows.Forms.CheckBox
        Me.chkUseETasFA = New System.Windows.Forms.CheckBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.chkNoSpoilers = New System.Windows.Forms.CheckBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtAutoThumbs = New System.Windows.Forms.TextBox
        Me.chkAutoThumbs = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkOutlineForPlot = New System.Windows.Forms.CheckBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtIMDBURL = New System.Windows.Forms.TextBox
        Me.chkCastWithImg = New System.Windows.Forms.CheckBox
        Me.chkUseCertForMPAA = New System.Windows.Forms.CheckBox
        Me.chkFullCast = New System.Windows.Forms.CheckBox
        Me.chkFullCrew = New System.Windows.Forms.CheckBox
        Me.cbCert = New System.Windows.Forms.ComboBox
        Me.chkCert = New System.Windows.Forms.CheckBox
        Me.gbRTFormat = New System.Windows.Forms.GroupBox
        Me.rbHM = New System.Windows.Forms.RadioButton
        Me.rbMins = New System.Windows.Forms.RadioButton
        Me.chkUseMIDuration = New System.Windows.Forms.CheckBox
        Me.chkScanMediaInfo = New System.Windows.Forms.CheckBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnApply = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.cdColor = New System.Windows.Forms.ColorDialog
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.ilSettings = New System.Windows.Forms.ImageList(Me.components)
        Me.tvSettings = New System.Windows.Forms.TreeView
        Me.pnlGeneral = New System.Windows.Forms.Panel
        Me.pnlXBMCCom = New System.Windows.Forms.Panel
        Me.btnRemoveCom = New System.Windows.Forms.Button
        Me.lbXBMCCom = New System.Windows.Forms.ListBox
        Me.pnlMovies = New System.Windows.Forms.Panel
        Me.GroupBox27 = New System.Windows.Forms.GroupBox
        Me.chkMissingExtra = New System.Windows.Forms.CheckBox
        Me.chkMissingSubs = New System.Windows.Forms.CheckBox
        Me.chkMissingTrailer = New System.Windows.Forms.CheckBox
        Me.chkMissingNFO = New System.Windows.Forms.CheckBox
        Me.chkMissingFanart = New System.Windows.Forms.CheckBox
        Me.chkMissingPoster = New System.Windows.Forms.CheckBox
        Me.GroupBox16 = New System.Windows.Forms.GroupBox
        Me.chkAutoBD = New System.Windows.Forms.CheckBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtBDPath = New System.Windows.Forms.TextBox
        Me.pnlScraper = New System.Windows.Forms.Panel
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox26 = New System.Windows.Forms.GroupBox
        Me.GroupBox20 = New System.Windows.Forms.GroupBox
        Me.chkDeleteAllTrailers = New System.Windows.Forms.CheckBox
        Me.chkOverwriteTrailer = New System.Windows.Forms.CheckBox
        Me.chkNoDLTrailer = New System.Windows.Forms.CheckBox
        Me.chkSingleScrapeTrailer = New System.Windows.Forms.CheckBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.txtTimeout = New System.Windows.Forms.TextBox
        Me.chkUpdaterTrailer = New System.Windows.Forms.CheckBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.lbTrailerSites = New System.Windows.Forms.CheckedListBox
        Me.chkDownloadTrailer = New System.Windows.Forms.CheckBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.gbOptions = New System.Windows.Forms.GroupBox
        Me.chkTop250 = New System.Windows.Forms.CheckBox
        Me.txtGenreLimit = New System.Windows.Forms.TextBox
        Me.lblLimit2 = New System.Windows.Forms.Label
        Me.txtActorLimit = New System.Windows.Forms.TextBox
        Me.lblLimit = New System.Windows.Forms.Label
        Me.chkCrew = New System.Windows.Forms.CheckBox
        Me.chkMusicBy = New System.Windows.Forms.CheckBox
        Me.chkProducers = New System.Windows.Forms.CheckBox
        Me.chkWriters = New System.Windows.Forms.CheckBox
        Me.chkStudio = New System.Windows.Forms.CheckBox
        Me.chkRuntime = New System.Windows.Forms.CheckBox
        Me.chkPlot = New System.Windows.Forms.CheckBox
        Me.chkOutline = New System.Windows.Forms.CheckBox
        Me.chkGenre = New System.Windows.Forms.CheckBox
        Me.chkDirector = New System.Windows.Forms.CheckBox
        Me.chkTagline = New System.Windows.Forms.CheckBox
        Me.chkCast = New System.Windows.Forms.CheckBox
        Me.chkVotes = New System.Windows.Forms.CheckBox
        Me.chkTrailer = New System.Windows.Forms.CheckBox
        Me.chkRating = New System.Windows.Forms.CheckBox
        Me.chkRelease = New System.Windows.Forms.CheckBox
        Me.chkMPAA = New System.Windows.Forms.CheckBox
        Me.chkYear = New System.Windows.Forms.CheckBox
        Me.chkTitle = New System.Windows.Forms.CheckBox
        Me.lblCurrent = New System.Windows.Forms.Label
        Me.pnlCurrent = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.pnlExtensions = New System.Windows.Forms.Panel
        Me.GroupBox28 = New System.Windows.Forms.GroupBox
        Me.txtDefFIExt = New System.Windows.Forms.TextBox
        Me.Label34 = New System.Windows.Forms.Label
        Me.btnRemoveMetaDataFT = New System.Windows.Forms.Button
        Me.btnEditMetaDataFT = New System.Windows.Forms.Button
        Me.btnNewMetaDataFT = New System.Windows.Forms.Button
        Me.lstMetadata = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.GroupBox22 = New System.Windows.Forms.GroupBox
        Me.btnRemoveNoStack = New System.Windows.Forms.Button
        Me.btnAddNoStack = New System.Windows.Forms.Button
        Me.txtNoStack = New System.Windows.Forms.TextBox
        Me.lstNoStack = New System.Windows.Forms.ListBox
        Me.gbRenamerPatterns = New System.Windows.Forms.GroupBox
        Me.chkRenameSingle = New System.Windows.Forms.CheckBox
        Me.chkRenameMulti = New System.Windows.Forms.CheckBox
        Me.lblFilePattern = New System.Windows.Forms.Label
        Me.lblFolderPattern = New System.Windows.Forms.Label
        Me.txtFilePattern = New System.Windows.Forms.TextBox
        Me.txtFolderPattern = New System.Windows.Forms.TextBox
        Me.GroupBox18 = New System.Windows.Forms.GroupBox
        Me.btnRemMovieExt = New System.Windows.Forms.Button
        Me.btnAddMovieExt = New System.Windows.Forms.Button
        Me.txtMovieExt = New System.Windows.Forms.TextBox
        Me.lstMovieExts = New System.Windows.Forms.ListBox
        Me.pnlSources = New System.Windows.Forms.Panel
        Me.btnEditSource = New System.Windows.Forms.Button
        Me.GroupBox19 = New System.Windows.Forms.GroupBox
        Me.chkAutoDetectVTS = New System.Windows.Forms.CheckBox
        Me.chkSkipStackedSizeCheck = New System.Windows.Forms.CheckBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.txtSkipLessThan = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.pnlImages = New System.Windows.Forms.Panel
        Me.GroupBox23 = New System.Windows.Forms.GroupBox
        Me.chkAutoETSize = New System.Windows.Forms.CheckBox
        Me.cbAutoETSize = New System.Windows.Forms.ComboBox
        Me.GroupBox24 = New System.Windows.Forms.GroupBox
        Me.txtETWidth = New System.Windows.Forms.TextBox
        Me.txtETHeight = New System.Windows.Forms.TextBox
        Me.chkETPadding = New System.Windows.Forms.CheckBox
        Me.Label28 = New System.Windows.Forms.Label
        Me.Label29 = New System.Windows.Forms.Label
        Me.rbETCustom = New System.Windows.Forms.RadioButton
        Me.rbETNative = New System.Windows.Forms.RadioButton
        Me.GroupBox17 = New System.Windows.Forms.GroupBox
        Me.chkUseImgCacheUpdaters = New System.Windows.Forms.CheckBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.chkPersistImgCache = New System.Windows.Forms.CheckBox
        Me.chkUseImgCache = New System.Windows.Forms.CheckBox
        Me.fbdBrowse = New System.Windows.Forms.FolderBrowserDialog
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox29.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tcCleaner.SuspendLayout()
        Me.tpStandard.SuspendLayout()
        Me.tpExpert.SuspendLayout()
        Me.gbFilters.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox25.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox21.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox15.SuspendLayout()
        Me.GroupBox14.SuspendLayout()
        CType(Me.tbPosterQual, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox13.SuspendLayout()
        CType(Me.tbFanartQual, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.gbRTFormat.SuspendLayout()
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGeneral.SuspendLayout()
        Me.pnlXBMCCom.SuspendLayout()
        Me.pnlMovies.SuspendLayout()
        Me.GroupBox27.SuspendLayout()
        Me.GroupBox16.SuspendLayout()
        Me.pnlScraper.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox26.SuspendLayout()
        Me.GroupBox20.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.gbOptions.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlExtensions.SuspendLayout()
        Me.GroupBox28.SuspendLayout()
        Me.GroupBox22.SuspendLayout()
        Me.gbRenamerPatterns.SuspendLayout()
        Me.GroupBox18.SuspendLayout()
        Me.pnlSources.SuspendLayout()
        Me.GroupBox19.SuspendLayout()
        Me.pnlImages.SuspendLayout()
        Me.GroupBox23.SuspendLayout()
        Me.GroupBox24.SuspendLayout()
        Me.GroupBox17.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox11
        '
        Me.GroupBox11.Controls.Add(Me.btnEditCom)
        Me.GroupBox11.Controls.Add(Me.txtName)
        Me.GroupBox11.Controls.Add(Me.Label16)
        Me.GroupBox11.Controls.Add(Me.txtPassword)
        Me.GroupBox11.Controls.Add(Me.btnAddCom)
        Me.GroupBox11.Controls.Add(Me.txtUsername)
        Me.GroupBox11.Controls.Add(Me.Label13)
        Me.GroupBox11.Controls.Add(Me.Label14)
        Me.GroupBox11.Controls.Add(Me.Label7)
        Me.GroupBox11.Controls.Add(Me.Label6)
        Me.GroupBox11.Controls.Add(Me.txtPort)
        Me.GroupBox11.Controls.Add(Me.txtIP)
        Me.GroupBox11.Location = New System.Drawing.Point(266, 6)
        Me.GroupBox11.Name = "GroupBox11"
        Me.GroupBox11.Size = New System.Drawing.Size(308, 141)
        Me.GroupBox11.TabIndex = 2
        Me.GroupBox11.TabStop = False
        Me.GroupBox11.Text = "XBMC Communication"
        '
        'btnEditCom
        '
        Me.btnEditCom.Enabled = False
        Me.btnEditCom.Image = CType(resources.GetObject("btnEditCom.Image"), System.Drawing.Image)
        Me.btnEditCom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditCom.Location = New System.Drawing.Point(14, 110)
        Me.btnEditCom.Name = "btnEditCom"
        Me.btnEditCom.Size = New System.Drawing.Size(91, 23)
        Me.btnEditCom.TabIndex = 5
        Me.btnEditCom.Text = "Commit Edit"
        Me.btnEditCom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEditCom.UseVisualStyleBackColor = True
        '
        'txtName
        '
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Location = New System.Drawing.Point(55, 18)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(238, 20)
        Me.txtName.TabIndex = 0
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(11, 22)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(38, 13)
        Me.Label16.TabIndex = 12
        Me.Label16.Text = "Name:"
        '
        'txtPassword
        '
        Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPassword.Location = New System.Drawing.Point(221, 78)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(72, 20)
        Me.txtPassword.TabIndex = 4
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'btnAddCom
        '
        Me.btnAddCom.Image = CType(resources.GetObject("btnAddCom.Image"), System.Drawing.Image)
        Me.btnAddCom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddCom.Location = New System.Drawing.Point(216, 110)
        Me.btnAddCom.Name = "btnAddCom"
        Me.btnAddCom.Size = New System.Drawing.Size(77, 23)
        Me.btnAddCom.TabIndex = 6
        Me.btnAddCom.Text = "Add New"
        Me.btnAddCom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddCom.UseVisualStyleBackColor = True
        '
        'txtUsername
        '
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsername.Location = New System.Drawing.Point(83, 78)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(72, 20)
        Me.txtUsername.TabIndex = 3
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(12, 80)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(58, 13)
        Me.Label13.TabIndex = 11
        Me.Label13.Text = "Username:"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(156, 81)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(56, 13)
        Me.Label14.TabIndex = 10
        Me.Label14.Text = "Password:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 52)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(53, 13)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "XBMC IP:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(178, 50)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "XBMC Port:"
        '
        'txtPort
        '
        Me.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPort.Location = New System.Drawing.Point(242, 47)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(51, 20)
        Me.txtPort.TabIndex = 2
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Location = New System.Drawing.Point(70, 47)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(85, 20)
        Me.txtIP.TabIndex = 1
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.GroupBox29)
        Me.GroupBox4.Controls.Add(Me.btnDLTrans)
        Me.GroupBox4.Controls.Add(Me.Label32)
        Me.GroupBox4.Controls.Add(Me.cbIntLang)
        Me.GroupBox4.Controls.Add(Me.chkInfoPanelAnim)
        Me.GroupBox4.Controls.Add(Me.chkUpdates)
        Me.GroupBox4.Controls.Add(Me.chkOverwriteNfo)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.chkLogErrors)
        Me.GroupBox4.Location = New System.Drawing.Point(203, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(185, 333)
        Me.GroupBox4.TabIndex = 2
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Miscellaneous"
        '
        'GroupBox29
        '
        Me.GroupBox29.Controls.Add(Me.Label36)
        Me.GroupBox29.Controls.Add(Me.cbMovieTheme)
        Me.GroupBox29.Controls.Add(Me.Label35)
        Me.GroupBox29.Location = New System.Drawing.Point(9, 154)
        Me.GroupBox29.Name = "GroupBox29"
        Me.GroupBox29.Size = New System.Drawing.Size(167, 119)
        Me.GroupBox29.TabIndex = 21
        Me.GroupBox29.TabStop = False
        Me.GroupBox29.Text = "Themes"
        '
        'Label36
        '
        Me.Label36.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label36.Location = New System.Drawing.Point(4, 16)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(159, 24)
        Me.Label36.TabIndex = 21
        Me.Label36.Text = "You must restart Ember before changes will take effect."
        Me.Label36.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'cbMovieTheme
        '
        Me.cbMovieTheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbMovieTheme.FormattingEnabled = True
        Me.cbMovieTheme.Location = New System.Drawing.Point(9, 62)
        Me.cbMovieTheme.Name = "cbMovieTheme"
        Me.cbMovieTheme.Size = New System.Drawing.Size(137, 21)
        Me.cbMovieTheme.TabIndex = 19
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Location = New System.Drawing.Point(6, 45)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(75, 13)
        Me.Label35.TabIndex = 20
        Me.Label35.Text = "Movie Theme:"
        '
        'btnDLTrans
        '
        Me.btnDLTrans.Image = CType(resources.GetObject("btnDLTrans.Image"), System.Drawing.Image)
        Me.btnDLTrans.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnDLTrans.Location = New System.Drawing.Point(12, 275)
        Me.btnDLTrans.Name = "btnDLTrans"
        Me.btnDLTrans.Size = New System.Drawing.Size(162, 53)
        Me.btnDLTrans.TabIndex = 18
        Me.btnDLTrans.Text = "Download Addons"
        Me.btnDLTrans.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnDLTrans.UseVisualStyleBackColor = True
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.Location = New System.Drawing.Point(7, 114)
        Me.Label32.Name = "Label32"
        Me.Label32.Size = New System.Drawing.Size(103, 13)
        Me.Label32.TabIndex = 17
        Me.Label32.Text = "Interface Language:"
        '
        'cbIntLang
        '
        Me.cbIntLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbIntLang.FormattingEnabled = True
        Me.cbIntLang.Location = New System.Drawing.Point(10, 131)
        Me.cbIntLang.Name = "cbIntLang"
        Me.cbIntLang.Size = New System.Drawing.Size(137, 21)
        Me.cbIntLang.TabIndex = 16
        '
        'chkInfoPanelAnim
        '
        Me.chkInfoPanelAnim.AutoSize = True
        Me.chkInfoPanelAnim.Location = New System.Drawing.Point(10, 95)
        Me.chkInfoPanelAnim.Name = "chkInfoPanelAnim"
        Me.chkInfoPanelAnim.Size = New System.Drawing.Size(138, 17)
        Me.chkInfoPanelAnim.TabIndex = 3
        Me.chkInfoPanelAnim.Text = "Enable Panel Animation"
        Me.chkInfoPanelAnim.UseVisualStyleBackColor = True
        '
        'chkUpdates
        '
        Me.chkUpdates.AutoSize = True
        Me.chkUpdates.Location = New System.Drawing.Point(10, 16)
        Me.chkUpdates.Name = "chkUpdates"
        Me.chkUpdates.Size = New System.Drawing.Size(115, 17)
        Me.chkUpdates.TabIndex = 0
        Me.chkUpdates.Text = "Check for Updates"
        Me.chkUpdates.UseVisualStyleBackColor = True
        '
        'chkOverwriteNfo
        '
        Me.chkOverwriteNfo.AutoSize = True
        Me.chkOverwriteNfo.Location = New System.Drawing.Point(10, 53)
        Me.chkOverwriteNfo.Name = "chkOverwriteNfo"
        Me.chkOverwriteNfo.Size = New System.Drawing.Size(172, 17)
        Me.chkOverwriteNfo.TabIndex = 2
        Me.chkOverwriteNfo.Text = "Overwrite Non-conforming nfos"
        Me.chkOverwriteNfo.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(10, 68)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(165, 24)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "(If unchecked, non-conforming nfos will be renamed to <filename>.info)"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkLogErrors
        '
        Me.chkLogErrors.AutoSize = True
        Me.chkLogErrors.Location = New System.Drawing.Point(10, 34)
        Me.chkLogErrors.Name = "chkLogErrors"
        Me.chkLogErrors.Size = New System.Drawing.Size(105, 17)
        Me.chkLogErrors.TabIndex = 1
        Me.chkLogErrors.Text = "Log Errors to File"
        Me.chkLogErrors.UseVisualStyleBackColor = True
        '
        'cbLanguages
        '
        Me.cbLanguages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbLanguages.FormattingEnabled = True
        Me.cbLanguages.Location = New System.Drawing.Point(16, 134)
        Me.cbLanguages.Name = "cbLanguages"
        Me.cbLanguages.Size = New System.Drawing.Size(174, 21)
        Me.cbLanguages.Sorted = True
        Me.cbLanguages.TabIndex = 17
        '
        'Label31
        '
        Me.Label31.Location = New System.Drawing.Point(3, 96)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(207, 35)
        Me.Label31.TabIndex = 16
        Me.Label31.Text = "Display Overlay if Video Contains an Audio Stream With the Following Language:"
        Me.Label31.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.tcCleaner)
        Me.GroupBox3.Location = New System.Drawing.Point(393, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(196, 336)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Clean Files"
        '
        'tcCleaner
        '
        Me.tcCleaner.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.tcCleaner.Controls.Add(Me.tpStandard)
        Me.tcCleaner.Controls.Add(Me.tpExpert)
        Me.tcCleaner.Location = New System.Drawing.Point(6, 19)
        Me.tcCleaner.Name = "tcCleaner"
        Me.tcCleaner.SelectedIndex = 0
        Me.tcCleaner.Size = New System.Drawing.Size(184, 309)
        Me.tcCleaner.TabIndex = 0
        '
        'tpStandard
        '
        Me.tpStandard.BackColor = System.Drawing.Color.White
        Me.tpStandard.Controls.Add(Me.chkCleanFolderJPG)
        Me.tpStandard.Controls.Add(Me.chkCleanExtrathumbs)
        Me.tpStandard.Controls.Add(Me.chkCleanMovieTBN)
        Me.tpStandard.Controls.Add(Me.chkCleanMovieNameJPG)
        Me.tpStandard.Controls.Add(Me.chkCleanMovieTBNb)
        Me.tpStandard.Controls.Add(Me.chkCleanMovieJPG)
        Me.tpStandard.Controls.Add(Me.chkCleanFanartJPG)
        Me.tpStandard.Controls.Add(Me.chkCleanPosterJPG)
        Me.tpStandard.Controls.Add(Me.chkCleanMovieFanartJPG)
        Me.tpStandard.Controls.Add(Me.chkCleanPosterTBN)
        Me.tpStandard.Controls.Add(Me.chkCleanMovieNFO)
        Me.tpStandard.Controls.Add(Me.chkCleanDotFanartJPG)
        Me.tpStandard.Controls.Add(Me.chkCleanMovieNFOb)
        Me.tpStandard.Location = New System.Drawing.Point(4, 25)
        Me.tpStandard.Name = "tpStandard"
        Me.tpStandard.Padding = New System.Windows.Forms.Padding(3)
        Me.tpStandard.Size = New System.Drawing.Size(176, 280)
        Me.tpStandard.TabIndex = 0
        Me.tpStandard.Text = "Standard"
        Me.tpStandard.UseVisualStyleBackColor = True
        '
        'chkCleanFolderJPG
        '
        Me.chkCleanFolderJPG.AutoSize = True
        Me.chkCleanFolderJPG.Location = New System.Drawing.Point(7, 10)
        Me.chkCleanFolderJPG.Name = "chkCleanFolderJPG"
        Me.chkCleanFolderJPG.Size = New System.Drawing.Size(74, 17)
        Me.chkCleanFolderJPG.TabIndex = 0
        Me.chkCleanFolderJPG.Text = "/folder.jpg"
        Me.chkCleanFolderJPG.UseVisualStyleBackColor = True
        '
        'chkCleanExtrathumbs
        '
        Me.chkCleanExtrathumbs.AutoSize = True
        Me.chkCleanExtrathumbs.Location = New System.Drawing.Point(7, 235)
        Me.chkCleanExtrathumbs.Name = "chkCleanExtrathumbs"
        Me.chkCleanExtrathumbs.Size = New System.Drawing.Size(93, 17)
        Me.chkCleanExtrathumbs.TabIndex = 12
        Me.chkCleanExtrathumbs.Text = "/extrathumbs/"
        Me.chkCleanExtrathumbs.UseVisualStyleBackColor = True
        '
        'chkCleanMovieTBN
        '
        Me.chkCleanMovieTBN.AutoSize = True
        Me.chkCleanMovieTBN.Location = New System.Drawing.Point(7, 29)
        Me.chkCleanMovieTBN.Name = "chkCleanMovieTBN"
        Me.chkCleanMovieTBN.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanMovieTBN.TabIndex = 1
        Me.chkCleanMovieTBN.Text = "/movie.tbn"
        Me.chkCleanMovieTBN.UseVisualStyleBackColor = True
        '
        'chkCleanMovieNameJPG
        '
        Me.chkCleanMovieNameJPG.AutoSize = True
        Me.chkCleanMovieNameJPG.Location = New System.Drawing.Point(7, 123)
        Me.chkCleanMovieNameJPG.Name = "chkCleanMovieNameJPG"
        Me.chkCleanMovieNameJPG.Size = New System.Drawing.Size(88, 17)
        Me.chkCleanMovieNameJPG.TabIndex = 6
        Me.chkCleanMovieNameJPG.Text = "/<movie>.jpg"
        Me.chkCleanMovieNameJPG.UseVisualStyleBackColor = True
        '
        'chkCleanMovieTBNb
        '
        Me.chkCleanMovieTBNb.AutoSize = True
        Me.chkCleanMovieTBNb.Location = New System.Drawing.Point(7, 48)
        Me.chkCleanMovieTBNb.Name = "chkCleanMovieTBNb"
        Me.chkCleanMovieTBNb.Size = New System.Drawing.Size(89, 17)
        Me.chkCleanMovieTBNb.TabIndex = 2
        Me.chkCleanMovieTBNb.Text = "/<movie>.tbn"
        Me.chkCleanMovieTBNb.UseVisualStyleBackColor = True
        '
        'chkCleanMovieJPG
        '
        Me.chkCleanMovieJPG.AutoSize = True
        Me.chkCleanMovieJPG.Location = New System.Drawing.Point(7, 104)
        Me.chkCleanMovieJPG.Name = "chkCleanMovieJPG"
        Me.chkCleanMovieJPG.Size = New System.Drawing.Size(76, 17)
        Me.chkCleanMovieJPG.TabIndex = 5
        Me.chkCleanMovieJPG.Text = "/movie.jpg"
        Me.chkCleanMovieJPG.UseVisualStyleBackColor = True
        '
        'chkCleanFanartJPG
        '
        Me.chkCleanFanartJPG.AutoSize = True
        Me.chkCleanFanartJPG.Location = New System.Drawing.Point(7, 141)
        Me.chkCleanFanartJPG.Name = "chkCleanFanartJPG"
        Me.chkCleanFanartJPG.Size = New System.Drawing.Size(75, 17)
        Me.chkCleanFanartJPG.TabIndex = 7
        Me.chkCleanFanartJPG.Text = "/fanart.jpg"
        Me.chkCleanFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanPosterJPG
        '
        Me.chkCleanPosterJPG.AutoSize = True
        Me.chkCleanPosterJPG.Location = New System.Drawing.Point(7, 86)
        Me.chkCleanPosterJPG.Name = "chkCleanPosterJPG"
        Me.chkCleanPosterJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanPosterJPG.TabIndex = 4
        Me.chkCleanPosterJPG.Text = "/poster.jpg"
        Me.chkCleanPosterJPG.UseVisualStyleBackColor = True
        '
        'chkCleanMovieFanartJPG
        '
        Me.chkCleanMovieFanartJPG.AutoSize = True
        Me.chkCleanMovieFanartJPG.Location = New System.Drawing.Point(7, 160)
        Me.chkCleanMovieFanartJPG.Name = "chkCleanMovieFanartJPG"
        Me.chkCleanMovieFanartJPG.Size = New System.Drawing.Size(118, 17)
        Me.chkCleanMovieFanartJPG.TabIndex = 8
        Me.chkCleanMovieFanartJPG.Text = "/<movie>-fanart.jpg"
        Me.chkCleanMovieFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanPosterTBN
        '
        Me.chkCleanPosterTBN.AutoSize = True
        Me.chkCleanPosterTBN.Location = New System.Drawing.Point(7, 67)
        Me.chkCleanPosterTBN.Name = "chkCleanPosterTBN"
        Me.chkCleanPosterTBN.Size = New System.Drawing.Size(78, 17)
        Me.chkCleanPosterTBN.TabIndex = 3
        Me.chkCleanPosterTBN.Text = "/poster.tbn"
        Me.chkCleanPosterTBN.UseVisualStyleBackColor = True
        '
        'chkCleanMovieNFO
        '
        Me.chkCleanMovieNFO.AutoSize = True
        Me.chkCleanMovieNFO.Location = New System.Drawing.Point(7, 198)
        Me.chkCleanMovieNFO.Name = "chkCleanMovieNFO"
        Me.chkCleanMovieNFO.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanMovieNFO.TabIndex = 10
        Me.chkCleanMovieNFO.Text = "/movie.nfo"
        Me.chkCleanMovieNFO.UseVisualStyleBackColor = True
        '
        'chkCleanDotFanartJPG
        '
        Me.chkCleanDotFanartJPG.AutoSize = True
        Me.chkCleanDotFanartJPG.Location = New System.Drawing.Point(7, 179)
        Me.chkCleanDotFanartJPG.Name = "chkCleanDotFanartJPG"
        Me.chkCleanDotFanartJPG.Size = New System.Drawing.Size(118, 17)
        Me.chkCleanDotFanartJPG.TabIndex = 9
        Me.chkCleanDotFanartJPG.Text = "/<movie>.fanart.jpg"
        Me.chkCleanDotFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanMovieNFOb
        '
        Me.chkCleanMovieNFOb.AutoSize = True
        Me.chkCleanMovieNFOb.Location = New System.Drawing.Point(7, 217)
        Me.chkCleanMovieNFOb.Name = "chkCleanMovieNFOb"
        Me.chkCleanMovieNFOb.Size = New System.Drawing.Size(89, 17)
        Me.chkCleanMovieNFOb.TabIndex = 11
        Me.chkCleanMovieNFOb.Text = "/<movie>.nfo"
        Me.chkCleanMovieNFOb.UseVisualStyleBackColor = True
        '
        'tpExpert
        '
        Me.tpExpert.BackColor = System.Drawing.Color.White
        Me.tpExpert.Controls.Add(Me.chkWhitelistVideo)
        Me.tpExpert.Controls.Add(Me.Label27)
        Me.tpExpert.Controls.Add(Me.btnRemoveWhitelist)
        Me.tpExpert.Controls.Add(Me.btnAddWhitelist)
        Me.tpExpert.Controls.Add(Me.txtWhitelist)
        Me.tpExpert.Controls.Add(Me.lstWhitelist)
        Me.tpExpert.Controls.Add(Me.Label25)
        Me.tpExpert.Location = New System.Drawing.Point(4, 25)
        Me.tpExpert.Name = "tpExpert"
        Me.tpExpert.Padding = New System.Windows.Forms.Padding(3)
        Me.tpExpert.Size = New System.Drawing.Size(176, 280)
        Me.tpExpert.TabIndex = 1
        Me.tpExpert.Text = "Expert"
        Me.tpExpert.UseVisualStyleBackColor = True
        '
        'chkWhitelistVideo
        '
        Me.chkWhitelistVideo.AutoSize = True
        Me.chkWhitelistVideo.Location = New System.Drawing.Point(15, 86)
        Me.chkWhitelistVideo.Name = "chkWhitelistVideo"
        Me.chkWhitelistVideo.Size = New System.Drawing.Size(150, 17)
        Me.chkWhitelistVideo.TabIndex = 0
        Me.chkWhitelistVideo.Text = "Whitelist Video Extensions"
        Me.chkWhitelistVideo.UseVisualStyleBackColor = True
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Location = New System.Drawing.Point(19, 107)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(116, 13)
        Me.Label27.TabIndex = 10
        Me.Label27.Text = "Whitelisted Extensions:"
        '
        'btnRemoveWhitelist
        '
        Me.btnRemoveWhitelist.Image = CType(resources.GetObject("btnRemoveWhitelist.Image"), System.Drawing.Image)
        Me.btnRemoveWhitelist.Location = New System.Drawing.Point(134, 252)
        Me.btnRemoveWhitelist.Name = "btnRemoveWhitelist"
        Me.btnRemoveWhitelist.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveWhitelist.TabIndex = 4
        Me.btnRemoveWhitelist.UseVisualStyleBackColor = True
        '
        'btnAddWhitelist
        '
        Me.btnAddWhitelist.Image = CType(resources.GetObject("btnAddWhitelist.Image"), System.Drawing.Image)
        Me.btnAddWhitelist.Location = New System.Drawing.Point(82, 251)
        Me.btnAddWhitelist.Name = "btnAddWhitelist"
        Me.btnAddWhitelist.Size = New System.Drawing.Size(23, 23)
        Me.btnAddWhitelist.TabIndex = 3
        Me.btnAddWhitelist.UseVisualStyleBackColor = True
        '
        'txtWhitelist
        '
        Me.txtWhitelist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtWhitelist.Location = New System.Drawing.Point(20, 252)
        Me.txtWhitelist.Name = "txtWhitelist"
        Me.txtWhitelist.Size = New System.Drawing.Size(61, 20)
        Me.txtWhitelist.TabIndex = 2
        '
        'lstWhitelist
        '
        Me.lstWhitelist.FormattingEnabled = True
        Me.lstWhitelist.Location = New System.Drawing.Point(19, 126)
        Me.lstWhitelist.Name = "lstWhitelist"
        Me.lstWhitelist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstWhitelist.Size = New System.Drawing.Size(138, 121)
        Me.lstWhitelist.TabIndex = 1
        '
        'Label25
        '
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.ForeColor = System.Drawing.Color.Red
        Me.Label25.Location = New System.Drawing.Point(10, 3)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(155, 68)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = "WARNING: Using the Expert Mode Cleaner could potentially delete wanted files. Tak" & _
            "e care when using this tool."
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'gbFilters
        '
        Me.gbFilters.Controls.Add(Me.btnDown)
        Me.gbFilters.Controls.Add(Me.btnUp)
        Me.gbFilters.Controls.Add(Me.chkProperCase)
        Me.gbFilters.Controls.Add(Me.btnRemoveFilter)
        Me.gbFilters.Controls.Add(Me.btnAddFilter)
        Me.gbFilters.Controls.Add(Me.txtFilter)
        Me.gbFilters.Controls.Add(Me.lstFilters)
        Me.gbFilters.Location = New System.Drawing.Point(6, 6)
        Me.gbFilters.Name = "gbFilters"
        Me.gbFilters.Size = New System.Drawing.Size(192, 175)
        Me.gbFilters.TabIndex = 0
        Me.gbFilters.TabStop = False
        Me.gbFilters.Text = "Folder/File Name Filters"
        '
        'btnDown
        '
        Me.btnDown.Image = CType(resources.GetObject("btnDown.Image"), System.Drawing.Image)
        Me.btnDown.Location = New System.Drawing.Point(129, 146)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(23, 23)
        Me.btnDown.TabIndex = 5
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
        Me.btnUp.Location = New System.Drawing.Point(105, 146)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(23, 23)
        Me.btnUp.TabIndex = 4
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'chkProperCase
        '
        Me.chkProperCase.AutoSize = True
        Me.chkProperCase.Location = New System.Drawing.Point(6, 16)
        Me.chkProperCase.Name = "chkProperCase"
        Me.chkProperCase.Size = New System.Drawing.Size(172, 17)
        Me.chkProperCase.TabIndex = 0
        Me.chkProperCase.Text = "Convert Names to Proper Case"
        Me.chkProperCase.UseVisualStyleBackColor = True
        '
        'btnRemoveFilter
        '
        Me.btnRemoveFilter.Image = CType(resources.GetObject("btnRemoveFilter.Image"), System.Drawing.Image)
        Me.btnRemoveFilter.Location = New System.Drawing.Point(163, 146)
        Me.btnRemoveFilter.Name = "btnRemoveFilter"
        Me.btnRemoveFilter.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveFilter.TabIndex = 6
        Me.btnRemoveFilter.UseVisualStyleBackColor = True
        '
        'btnAddFilter
        '
        Me.btnAddFilter.Image = CType(resources.GetObject("btnAddFilter.Image"), System.Drawing.Image)
        Me.btnAddFilter.Location = New System.Drawing.Point(68, 146)
        Me.btnAddFilter.Name = "btnAddFilter"
        Me.btnAddFilter.Size = New System.Drawing.Size(23, 23)
        Me.btnAddFilter.TabIndex = 3
        Me.btnAddFilter.UseVisualStyleBackColor = True
        '
        'txtFilter
        '
        Me.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilter.Location = New System.Drawing.Point(6, 147)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(61, 20)
        Me.txtFilter.TabIndex = 2
        '
        'lstFilters
        '
        Me.lstFilters.FormattingEnabled = True
        Me.lstFilters.Location = New System.Drawing.Point(6, 34)
        Me.lstFilters.Name = "lstFilters"
        Me.lstFilters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFilters.Size = New System.Drawing.Size(180, 108)
        Me.lstFilters.TabIndex = 1
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.lbGenre)
        Me.GroupBox12.Controls.Add(Me.chkShowGenresText)
        Me.GroupBox12.Controls.Add(Me.lblGenre)
        Me.GroupBox12.Controls.Add(Me.chkNoDisplayFanart)
        Me.GroupBox12.Controls.Add(Me.chkNoDisplayPoster)
        Me.GroupBox12.Controls.Add(Me.chkShowDims)
        Me.GroupBox12.Controls.Add(Me.Label8)
        Me.GroupBox12.Controls.Add(Me.chkMarkNew)
        Me.GroupBox12.Location = New System.Drawing.Point(5, 8)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(195, 280)
        Me.GroupBox12.TabIndex = 1
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Miscellaneous"
        '
        'lbGenre
        '
        Me.lbGenre.CheckOnClick = True
        Me.lbGenre.FormattingEnabled = True
        Me.lbGenre.IntegralHeight = False
        Me.lbGenre.Location = New System.Drawing.Point(27, 161)
        Me.lbGenre.Name = "lbGenre"
        Me.lbGenre.Size = New System.Drawing.Size(144, 108)
        Me.lbGenre.Sorted = True
        Me.lbGenre.TabIndex = 0
        '
        'chkShowGenresText
        '
        Me.chkShowGenresText.AutoSize = True
        Me.chkShowGenresText.Location = New System.Drawing.Point(12, 111)
        Me.chkShowGenresText.Name = "chkShowGenresText"
        Me.chkShowGenresText.Size = New System.Drawing.Size(159, 17)
        Me.chkShowGenresText.TabIndex = 57
        Me.chkShowGenresText.Text = "Allways Display Genres Text"
        Me.chkShowGenresText.UseVisualStyleBackColor = True
        '
        'lblGenre
        '
        Me.lblGenre.AutoSize = True
        Me.lblGenre.Location = New System.Drawing.Point(27, 144)
        Me.lblGenre.Name = "lblGenre"
        Me.lblGenre.Size = New System.Drawing.Size(115, 13)
        Me.lblGenre.TabIndex = 106
        Me.lblGenre.Text = "Genre Language Filter:"
        '
        'chkNoDisplayFanart
        '
        Me.chkNoDisplayFanart.AutoSize = True
        Me.chkNoDisplayFanart.Location = New System.Drawing.Point(12, 77)
        Me.chkNoDisplayFanart.Name = "chkNoDisplayFanart"
        Me.chkNoDisplayFanart.Size = New System.Drawing.Size(130, 17)
        Me.chkNoDisplayFanart.TabIndex = 2
        Me.chkNoDisplayFanart.Text = "Do Not Display Fanart"
        Me.chkNoDisplayFanart.UseVisualStyleBackColor = True
        '
        'chkNoDisplayPoster
        '
        Me.chkNoDisplayPoster.AutoSize = True
        Me.chkNoDisplayPoster.Location = New System.Drawing.Point(12, 60)
        Me.chkNoDisplayPoster.Name = "chkNoDisplayPoster"
        Me.chkNoDisplayPoster.Size = New System.Drawing.Size(130, 17)
        Me.chkNoDisplayPoster.TabIndex = 1
        Me.chkNoDisplayPoster.Text = "Do Not Display Poster"
        Me.chkNoDisplayPoster.UseVisualStyleBackColor = True
        '
        'chkShowDims
        '
        Me.chkShowDims.AutoSize = True
        Me.chkShowDims.Location = New System.Drawing.Point(12, 94)
        Me.chkShowDims.Name = "chkShowDims"
        Me.chkShowDims.Size = New System.Drawing.Size(149, 17)
        Me.chkShowDims.TabIndex = 3
        Me.chkShowDims.Text = "Display Image Dimensions"
        Me.chkShowDims.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(179, 27)
        Me.Label8.TabIndex = 56
        Me.Label8.Text = "(New movies will still display in green if not checked.)"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkMarkNew
        '
        Me.chkMarkNew.AutoSize = True
        Me.chkMarkNew.Location = New System.Drawing.Point(12, 14)
        Me.chkMarkNew.Name = "chkMarkNew"
        Me.chkMarkNew.Size = New System.Drawing.Size(112, 17)
        Me.chkMarkNew.TabIndex = 0
        Me.chkMarkNew.Text = "Mark New Movies"
        Me.chkMarkNew.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtCheckTitleTol)
        Me.GroupBox2.Controls.Add(Me.Label30)
        Me.GroupBox2.Controls.Add(Me.chkCheckTitles)
        Me.GroupBox2.Controls.Add(Me.GroupBox25)
        Me.GroupBox2.Controls.Add(Me.chkDisplayYear)
        Me.GroupBox2.Controls.Add(Me.chkMovieExtraCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieSubCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieTrailerCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieInfoCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieFanartCol)
        Me.GroupBox2.Controls.Add(Me.chkMoviePosterCol)
        Me.GroupBox2.Location = New System.Drawing.Point(204, 8)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(218, 280)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Media List Options"
        '
        'txtCheckTitleTol
        '
        Me.txtCheckTitleTol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCheckTitleTol.Enabled = False
        Me.txtCheckTitleTol.Location = New System.Drawing.Point(75, 50)
        Me.txtCheckTitleTol.Name = "txtCheckTitleTol"
        Me.txtCheckTitleTol.Size = New System.Drawing.Size(61, 20)
        Me.txtCheckTitleTol.TabIndex = 74
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Location = New System.Drawing.Point(18, 54)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(58, 13)
        Me.Label30.TabIndex = 73
        Me.Label30.Text = "Tolerance:"
        '
        'chkCheckTitles
        '
        Me.chkCheckTitles.AutoSize = True
        Me.chkCheckTitles.Location = New System.Drawing.Point(8, 35)
        Me.chkCheckTitles.Name = "chkCheckTitles"
        Me.chkCheckTitles.Size = New System.Drawing.Size(170, 17)
        Me.chkCheckTitles.TabIndex = 72
        Me.chkCheckTitles.Text = "Check Title Match Confidence"
        Me.chkCheckTitles.UseVisualStyleBackColor = True
        '
        'GroupBox25
        '
        Me.GroupBox25.Controls.Add(Me.btnRemoveToken)
        Me.GroupBox25.Controls.Add(Me.btnAddToken)
        Me.GroupBox25.Controls.Add(Me.txtSortToken)
        Me.GroupBox25.Controls.Add(Me.lstSortTokens)
        Me.GroupBox25.Location = New System.Drawing.Point(9, 73)
        Me.GroupBox25.Name = "GroupBox25"
        Me.GroupBox25.Size = New System.Drawing.Size(200, 98)
        Me.GroupBox25.TabIndex = 71
        Me.GroupBox25.TabStop = False
        Me.GroupBox25.Text = "Sort Tokens to Ignore"
        '
        'btnRemoveToken
        '
        Me.btnRemoveToken.Image = CType(resources.GetObject("btnRemoveToken.Image"), System.Drawing.Image)
        Me.btnRemoveToken.Location = New System.Drawing.Point(167, 72)
        Me.btnRemoveToken.Name = "btnRemoveToken"
        Me.btnRemoveToken.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveToken.TabIndex = 3
        Me.btnRemoveToken.UseVisualStyleBackColor = True
        '
        'btnAddToken
        '
        Me.btnAddToken.Image = CType(resources.GetObject("btnAddToken.Image"), System.Drawing.Image)
        Me.btnAddToken.Location = New System.Drawing.Point(72, 72)
        Me.btnAddToken.Name = "btnAddToken"
        Me.btnAddToken.Size = New System.Drawing.Size(23, 23)
        Me.btnAddToken.TabIndex = 2
        Me.btnAddToken.UseVisualStyleBackColor = True
        '
        'txtSortToken
        '
        Me.txtSortToken.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSortToken.Location = New System.Drawing.Point(10, 73)
        Me.txtSortToken.Name = "txtSortToken"
        Me.txtSortToken.Size = New System.Drawing.Size(61, 20)
        Me.txtSortToken.TabIndex = 1
        '
        'lstSortTokens
        '
        Me.lstSortTokens.FormattingEnabled = True
        Me.lstSortTokens.Location = New System.Drawing.Point(10, 15)
        Me.lstSortTokens.Name = "lstSortTokens"
        Me.lstSortTokens.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstSortTokens.Size = New System.Drawing.Size(180, 56)
        Me.lstSortTokens.Sorted = True
        Me.lstSortTokens.TabIndex = 0
        '
        'chkDisplayYear
        '
        Me.chkDisplayYear.AutoSize = True
        Me.chkDisplayYear.Location = New System.Drawing.Point(8, 19)
        Me.chkDisplayYear.Name = "chkDisplayYear"
        Me.chkDisplayYear.Size = New System.Drawing.Size(138, 17)
        Me.chkDisplayYear.TabIndex = 70
        Me.chkDisplayYear.Text = "Display Year in List Title"
        Me.chkDisplayYear.UseVisualStyleBackColor = True
        '
        'chkMovieExtraCol
        '
        Me.chkMovieExtraCol.AutoSize = True
        Me.chkMovieExtraCol.Location = New System.Drawing.Point(9, 258)
        Me.chkMovieExtraCol.Name = "chkMovieExtraCol"
        Me.chkMovieExtraCol.Size = New System.Drawing.Size(142, 17)
        Me.chkMovieExtraCol.TabIndex = 5
        Me.chkMovieExtraCol.Text = "Hide Extrathumb Column"
        Me.chkMovieExtraCol.UseVisualStyleBackColor = True
        '
        'chkMovieSubCol
        '
        Me.chkMovieSubCol.AutoSize = True
        Me.chkMovieSubCol.Location = New System.Drawing.Point(9, 242)
        Me.chkMovieSubCol.Name = "chkMovieSubCol"
        Me.chkMovieSubCol.Size = New System.Drawing.Size(108, 17)
        Me.chkMovieSubCol.TabIndex = 4
        Me.chkMovieSubCol.Text = "Hide Sub Column"
        Me.chkMovieSubCol.UseVisualStyleBackColor = True
        '
        'chkMovieTrailerCol
        '
        Me.chkMovieTrailerCol.AutoSize = True
        Me.chkMovieTrailerCol.Location = New System.Drawing.Point(9, 226)
        Me.chkMovieTrailerCol.Name = "chkMovieTrailerCol"
        Me.chkMovieTrailerCol.Size = New System.Drawing.Size(118, 17)
        Me.chkMovieTrailerCol.TabIndex = 3
        Me.chkMovieTrailerCol.Text = "Hide Trailer Column"
        Me.chkMovieTrailerCol.UseVisualStyleBackColor = True
        '
        'chkMovieInfoCol
        '
        Me.chkMovieInfoCol.AutoSize = True
        Me.chkMovieInfoCol.Location = New System.Drawing.Point(9, 210)
        Me.chkMovieInfoCol.Name = "chkMovieInfoCol"
        Me.chkMovieInfoCol.Size = New System.Drawing.Size(107, 17)
        Me.chkMovieInfoCol.TabIndex = 2
        Me.chkMovieInfoCol.Text = "Hide Info Column"
        Me.chkMovieInfoCol.UseVisualStyleBackColor = True
        '
        'chkMovieFanartCol
        '
        Me.chkMovieFanartCol.AutoSize = True
        Me.chkMovieFanartCol.Location = New System.Drawing.Point(9, 194)
        Me.chkMovieFanartCol.Name = "chkMovieFanartCol"
        Me.chkMovieFanartCol.Size = New System.Drawing.Size(119, 17)
        Me.chkMovieFanartCol.TabIndex = 1
        Me.chkMovieFanartCol.Text = "Hide Fanart Column"
        Me.chkMovieFanartCol.UseVisualStyleBackColor = True
        '
        'chkMoviePosterCol
        '
        Me.chkMoviePosterCol.AutoSize = True
        Me.chkMoviePosterCol.Location = New System.Drawing.Point(9, 178)
        Me.chkMoviePosterCol.Name = "chkMoviePosterCol"
        Me.chkMoviePosterCol.Size = New System.Drawing.Size(119, 17)
        Me.chkMoviePosterCol.TabIndex = 0
        Me.chkMoviePosterCol.Text = "Hide Poster Column"
        Me.chkMoviePosterCol.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.chkMovieNameMultiOnly)
        Me.GroupBox8.Controls.Add(Me.GroupBox21)
        Me.GroupBox8.Controls.Add(Me.chkVideoTSParent)
        Me.GroupBox8.Controls.Add(Me.GroupBox7)
        Me.GroupBox8.Controls.Add(Me.GroupBox6)
        Me.GroupBox8.Controls.Add(Me.GroupBox5)
        Me.GroupBox8.Location = New System.Drawing.Point(232, 113)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(356, 191)
        Me.GroupBox8.TabIndex = 5
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "File Naming"
        '
        'chkMovieNameMultiOnly
        '
        Me.chkMovieNameMultiOnly.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkMovieNameMultiOnly.Location = New System.Drawing.Point(186, 63)
        Me.chkMovieNameMultiOnly.Name = "chkMovieNameMultiOnly"
        Me.chkMovieNameMultiOnly.Size = New System.Drawing.Size(165, 42)
        Me.chkMovieNameMultiOnly.TabIndex = 5
        Me.chkMovieNameMultiOnly.Text = "Use <movie> Only for Folders with Multiple Movies"
        Me.chkMovieNameMultiOnly.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkMovieNameMultiOnly.UseVisualStyleBackColor = True
        '
        'GroupBox21
        '
        Me.GroupBox21.Controls.Add(Me.rbBracketTrailer)
        Me.GroupBox21.Controls.Add(Me.rbDashTrailer)
        Me.GroupBox21.Location = New System.Drawing.Point(137, 111)
        Me.GroupBox21.Name = "GroupBox21"
        Me.GroupBox21.Size = New System.Drawing.Size(102, 53)
        Me.GroupBox21.TabIndex = 3
        Me.GroupBox21.TabStop = False
        Me.GroupBox21.Text = "Trailer"
        '
        'rbBracketTrailer
        '
        Me.rbBracketTrailer.AutoSize = True
        Me.rbBracketTrailer.Location = New System.Drawing.Point(5, 33)
        Me.rbBracketTrailer.Name = "rbBracketTrailer"
        Me.rbBracketTrailer.Size = New System.Drawing.Size(56, 17)
        Me.rbBracketTrailer.TabIndex = 1
        Me.rbBracketTrailer.TabStop = True
        Me.rbBracketTrailer.Text = "[trailer]"
        Me.rbBracketTrailer.UseVisualStyleBackColor = True
        '
        'rbDashTrailer
        '
        Me.rbDashTrailer.AutoSize = True
        Me.rbDashTrailer.Location = New System.Drawing.Point(5, 17)
        Me.rbDashTrailer.Name = "rbDashTrailer"
        Me.rbDashTrailer.Size = New System.Drawing.Size(53, 17)
        Me.rbDashTrailer.TabIndex = 0
        Me.rbDashTrailer.TabStop = True
        Me.rbDashTrailer.Text = "-trailer"
        Me.rbDashTrailer.UseVisualStyleBackColor = True
        '
        'chkVideoTSParent
        '
        Me.chkVideoTSParent.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkVideoTSParent.Location = New System.Drawing.Point(186, 11)
        Me.chkVideoTSParent.Name = "chkVideoTSParent"
        Me.chkVideoTSParent.Size = New System.Drawing.Size(165, 48)
        Me.chkVideoTSParent.TabIndex = 1
        Me.chkVideoTSParent.Text = "YAMJ Compatible VIDEO_TS File Placement/Naming"
        Me.chkVideoTSParent.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkVideoTSParent.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.chkMovieNameNFO)
        Me.GroupBox7.Controls.Add(Me.chkMovieNFO)
        Me.GroupBox7.Location = New System.Drawing.Point(247, 111)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(102, 53)
        Me.GroupBox7.TabIndex = 4
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "NFO"
        '
        'chkMovieNameNFO
        '
        Me.chkMovieNameNFO.AutoSize = True
        Me.chkMovieNameNFO.Location = New System.Drawing.Point(6, 34)
        Me.chkMovieNameNFO.Name = "chkMovieNameNFO"
        Me.chkMovieNameNFO.Size = New System.Drawing.Size(84, 17)
        Me.chkMovieNameNFO.TabIndex = 1
        Me.chkMovieNameNFO.Text = "<movie>.nfo"
        Me.chkMovieNameNFO.UseVisualStyleBackColor = True
        '
        'chkMovieNFO
        '
        Me.chkMovieNFO.AutoSize = True
        Me.chkMovieNFO.Location = New System.Drawing.Point(6, 18)
        Me.chkMovieNFO.Name = "chkMovieNFO"
        Me.chkMovieNFO.Size = New System.Drawing.Size(72, 17)
        Me.chkMovieNFO.TabIndex = 0
        Me.chkMovieNFO.Text = "movie.nfo"
        Me.chkMovieNFO.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.chkMovieNameDotFanartJPG)
        Me.GroupBox6.Controls.Add(Me.chkMovieNameFanartJPG)
        Me.GroupBox6.Controls.Add(Me.chkFanartJPG)
        Me.GroupBox6.Location = New System.Drawing.Point(6, 111)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(125, 70)
        Me.GroupBox6.TabIndex = 2
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Fanart"
        '
        'chkMovieNameDotFanartJPG
        '
        Me.chkMovieNameDotFanartJPG.AutoSize = True
        Me.chkMovieNameDotFanartJPG.Location = New System.Drawing.Point(6, 51)
        Me.chkMovieNameDotFanartJPG.Name = "chkMovieNameDotFanartJPG"
        Me.chkMovieNameDotFanartJPG.Size = New System.Drawing.Size(113, 17)
        Me.chkMovieNameDotFanartJPG.TabIndex = 2
        Me.chkMovieNameDotFanartJPG.Text = "<movie>.fanart.jpg"
        Me.chkMovieNameDotFanartJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameFanartJPG
        '
        Me.chkMovieNameFanartJPG.AutoSize = True
        Me.chkMovieNameFanartJPG.Location = New System.Drawing.Point(6, 35)
        Me.chkMovieNameFanartJPG.Name = "chkMovieNameFanartJPG"
        Me.chkMovieNameFanartJPG.Size = New System.Drawing.Size(113, 17)
        Me.chkMovieNameFanartJPG.TabIndex = 1
        Me.chkMovieNameFanartJPG.Text = "<movie>-fanart.jpg"
        Me.chkMovieNameFanartJPG.UseVisualStyleBackColor = True
        '
        'chkFanartJPG
        '
        Me.chkFanartJPG.AutoSize = True
        Me.chkFanartJPG.Location = New System.Drawing.Point(6, 19)
        Me.chkFanartJPG.Name = "chkFanartJPG"
        Me.chkFanartJPG.Size = New System.Drawing.Size(70, 17)
        Me.chkFanartJPG.TabIndex = 0
        Me.chkFanartJPG.Text = "fanart.jpg"
        Me.chkFanartJPG.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.chkFolderJPG)
        Me.GroupBox5.Controls.Add(Me.chkPosterJPG)
        Me.GroupBox5.Controls.Add(Me.chkPosterTBN)
        Me.GroupBox5.Controls.Add(Me.chkMovieNameJPG)
        Me.GroupBox5.Controls.Add(Me.chkMovieJPG)
        Me.GroupBox5.Controls.Add(Me.chkMovieNameTBN)
        Me.GroupBox5.Controls.Add(Me.chkMovieTBN)
        Me.GroupBox5.Location = New System.Drawing.Point(6, 17)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(174, 83)
        Me.GroupBox5.TabIndex = 0
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Posters"
        '
        'chkFolderJPG
        '
        Me.chkFolderJPG.AutoSize = True
        Me.chkFolderJPG.Location = New System.Drawing.Point(6, 63)
        Me.chkFolderJPG.Name = "chkFolderJPG"
        Me.chkFolderJPG.Size = New System.Drawing.Size(69, 17)
        Me.chkFolderJPG.TabIndex = 3
        Me.chkFolderJPG.Text = "folder.jpg"
        Me.chkFolderJPG.UseVisualStyleBackColor = True
        '
        'chkPosterJPG
        '
        Me.chkPosterJPG.AutoSize = True
        Me.chkPosterJPG.Location = New System.Drawing.Point(85, 47)
        Me.chkPosterJPG.Name = "chkPosterJPG"
        Me.chkPosterJPG.Size = New System.Drawing.Size(72, 17)
        Me.chkPosterJPG.TabIndex = 6
        Me.chkPosterJPG.Text = "poster.jpg"
        Me.chkPosterJPG.UseVisualStyleBackColor = True
        '
        'chkPosterTBN
        '
        Me.chkPosterTBN.AutoSize = True
        Me.chkPosterTBN.Location = New System.Drawing.Point(6, 47)
        Me.chkPosterTBN.Name = "chkPosterTBN"
        Me.chkPosterTBN.Size = New System.Drawing.Size(73, 17)
        Me.chkPosterTBN.TabIndex = 2
        Me.chkPosterTBN.Text = "poster.tbn"
        Me.chkPosterTBN.UseVisualStyleBackColor = True
        '
        'chkMovieNameJPG
        '
        Me.chkMovieNameJPG.AutoSize = True
        Me.chkMovieNameJPG.Location = New System.Drawing.Point(85, 31)
        Me.chkMovieNameJPG.Name = "chkMovieNameJPG"
        Me.chkMovieNameJPG.Size = New System.Drawing.Size(83, 17)
        Me.chkMovieNameJPG.TabIndex = 5
        Me.chkMovieNameJPG.Text = "<movie>.jpg"
        Me.chkMovieNameJPG.UseVisualStyleBackColor = True
        '
        'chkMovieJPG
        '
        Me.chkMovieJPG.AutoSize = True
        Me.chkMovieJPG.Location = New System.Drawing.Point(6, 31)
        Me.chkMovieJPG.Name = "chkMovieJPG"
        Me.chkMovieJPG.Size = New System.Drawing.Size(71, 17)
        Me.chkMovieJPG.TabIndex = 1
        Me.chkMovieJPG.Text = "movie.jpg"
        Me.chkMovieJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameTBN
        '
        Me.chkMovieNameTBN.AutoSize = True
        Me.chkMovieNameTBN.Location = New System.Drawing.Point(85, 15)
        Me.chkMovieNameTBN.Name = "chkMovieNameTBN"
        Me.chkMovieNameTBN.Size = New System.Drawing.Size(84, 17)
        Me.chkMovieNameTBN.TabIndex = 4
        Me.chkMovieNameTBN.Text = "<movie>.tbn"
        Me.chkMovieNameTBN.UseVisualStyleBackColor = True
        '
        'chkMovieTBN
        '
        Me.chkMovieTBN.AutoSize = True
        Me.chkMovieTBN.Location = New System.Drawing.Point(6, 15)
        Me.chkMovieTBN.Name = "chkMovieTBN"
        Me.chkMovieTBN.Size = New System.Drawing.Size(72, 17)
        Me.chkMovieTBN.TabIndex = 0
        Me.chkMovieTBN.Text = "movie.tbn"
        Me.chkMovieTBN.UseVisualStyleBackColor = True
        '
        'lvMovies
        '
        Me.lvMovies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colID, Me.colName, Me.colPath, Me.colRecur, Me.colFolder, Me.colSingle})
        Me.lvMovies.FullRowSelect = True
        Me.lvMovies.HideSelection = False
        Me.lvMovies.Location = New System.Drawing.Point(5, 6)
        Me.lvMovies.Name = "lvMovies"
        Me.lvMovies.Size = New System.Drawing.Size(466, 105)
        Me.lvMovies.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvMovies.TabIndex = 0
        Me.lvMovies.UseCompatibleStateImageBehavior = False
        Me.lvMovies.View = System.Windows.Forms.View.Details
        '
        'colID
        '
        Me.colID.Width = 0
        '
        'colName
        '
        Me.colName.Text = "Name"
        Me.colName.Width = 75
        '
        'colPath
        '
        Me.colPath.Text = "Path"
        Me.colPath.Width = 150
        '
        'colRecur
        '
        Me.colRecur.Text = "Recursive"
        '
        'colFolder
        '
        Me.colFolder.Text = "Use Folder Name"
        Me.colFolder.Width = 100
        '
        'colSingle
        '
        Me.colSingle.Text = "Single Video"
        Me.colSingle.Width = 75
        '
        'btnMovieRem
        '
        Me.btnMovieRem.Image = CType(resources.GetObject("btnMovieRem.Image"), System.Drawing.Image)
        Me.btnMovieRem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovieRem.Location = New System.Drawing.Point(485, 88)
        Me.btnMovieRem.Name = "btnMovieRem"
        Me.btnMovieRem.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieRem.TabIndex = 3
        Me.btnMovieRem.Text = "Remove"
        Me.btnMovieRem.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovieRem.UseVisualStyleBackColor = True
        '
        'btnMovieAddFolder
        '
        Me.btnMovieAddFolder.Image = CType(resources.GetObject("btnMovieAddFolder.Image"), System.Drawing.Image)
        Me.btnMovieAddFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovieAddFolder.Location = New System.Drawing.Point(485, 6)
        Me.btnMovieAddFolder.Name = "btnMovieAddFolder"
        Me.btnMovieAddFolder.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieAddFolder.TabIndex = 1
        Me.btnMovieAddFolder.Text = "Add Source"
        Me.btnMovieAddFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovieAddFolder.UseVisualStyleBackColor = True
        '
        'GroupBox15
        '
        Me.GroupBox15.Controls.Add(Me.chkOFDBGenre)
        Me.GroupBox15.Controls.Add(Me.chkOFDBPlot)
        Me.GroupBox15.Controls.Add(Me.chkOFDBOutline)
        Me.GroupBox15.Controls.Add(Me.chkOFDBTitle)
        Me.GroupBox15.Location = New System.Drawing.Point(191, 5)
        Me.GroupBox15.Name = "GroupBox15"
        Me.GroupBox15.Size = New System.Drawing.Size(177, 89)
        Me.GroupBox15.TabIndex = 2
        Me.GroupBox15.TabStop = False
        Me.GroupBox15.Text = "OFDB"
        '
        'chkOFDBGenre
        '
        Me.chkOFDBGenre.Location = New System.Drawing.Point(6, 66)
        Me.chkOFDBGenre.Name = "chkOFDBGenre"
        Me.chkOFDBGenre.Size = New System.Drawing.Size(168, 17)
        Me.chkOFDBGenre.TabIndex = 3
        Me.chkOFDBGenre.Text = "Use OFDB Genre"
        Me.chkOFDBGenre.UseVisualStyleBackColor = True
        '
        'chkOFDBPlot
        '
        Me.chkOFDBPlot.Location = New System.Drawing.Point(6, 50)
        Me.chkOFDBPlot.Name = "chkOFDBPlot"
        Me.chkOFDBPlot.Size = New System.Drawing.Size(168, 17)
        Me.chkOFDBPlot.TabIndex = 2
        Me.chkOFDBPlot.Text = "Use OFDB Plot"
        Me.chkOFDBPlot.UseVisualStyleBackColor = True
        '
        'chkOFDBOutline
        '
        Me.chkOFDBOutline.Location = New System.Drawing.Point(6, 34)
        Me.chkOFDBOutline.Name = "chkOFDBOutline"
        Me.chkOFDBOutline.Size = New System.Drawing.Size(168, 17)
        Me.chkOFDBOutline.TabIndex = 1
        Me.chkOFDBOutline.Text = "Use OFDB Outline"
        Me.chkOFDBOutline.UseVisualStyleBackColor = True
        '
        'chkOFDBTitle
        '
        Me.chkOFDBTitle.Location = New System.Drawing.Point(6, 18)
        Me.chkOFDBTitle.Name = "chkOFDBTitle"
        Me.chkOFDBTitle.Size = New System.Drawing.Size(168, 17)
        Me.chkOFDBTitle.TabIndex = 0
        Me.chkOFDBTitle.Text = "Use OFDB Title"
        Me.chkOFDBTitle.UseVisualStyleBackColor = True
        '
        'GroupBox14
        '
        Me.GroupBox14.Controls.Add(Me.txtPosterWidth)
        Me.GroupBox14.Controls.Add(Me.txtPosterHeight)
        Me.GroupBox14.Controls.Add(Me.lblPosterQual)
        Me.GroupBox14.Controls.Add(Me.tbPosterQual)
        Me.GroupBox14.Controls.Add(Me.Label24)
        Me.GroupBox14.Controls.Add(Me.Label11)
        Me.GroupBox14.Controls.Add(Me.Label12)
        Me.GroupBox14.Controls.Add(Me.chkResizePoster)
        Me.GroupBox14.Controls.Add(Me.lblPosterSize)
        Me.GroupBox14.Controls.Add(Me.cbPosterSize)
        Me.GroupBox14.Controls.Add(Me.chkOverwritePoster)
        Me.GroupBox14.Location = New System.Drawing.Point(172, 3)
        Me.GroupBox14.Name = "GroupBox14"
        Me.GroupBox14.Size = New System.Drawing.Size(218, 170)
        Me.GroupBox14.TabIndex = 2
        Me.GroupBox14.TabStop = False
        Me.GroupBox14.Text = "Poster"
        '
        'txtPosterWidth
        '
        Me.txtPosterWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPosterWidth.Enabled = False
        Me.txtPosterWidth.Location = New System.Drawing.Point(65, 94)
        Me.txtPosterWidth.Name = "txtPosterWidth"
        Me.txtPosterWidth.Size = New System.Drawing.Size(38, 20)
        Me.txtPosterWidth.TabIndex = 3
        '
        'txtPosterHeight
        '
        Me.txtPosterHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPosterHeight.Enabled = False
        Me.txtPosterHeight.Location = New System.Drawing.Point(172, 94)
        Me.txtPosterHeight.Name = "txtPosterHeight"
        Me.txtPosterHeight.Size = New System.Drawing.Size(38, 20)
        Me.txtPosterHeight.TabIndex = 4
        '
        'lblPosterQual
        '
        Me.lblPosterQual.AutoSize = True
        Me.lblPosterQual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPosterQual.Location = New System.Drawing.Point(183, 143)
        Me.lblPosterQual.Name = "lblPosterQual"
        Me.lblPosterQual.Size = New System.Drawing.Size(32, 16)
        Me.lblPosterQual.TabIndex = 46
        Me.lblPosterQual.Text = "100"
        '
        'tbPosterQual
        '
        Me.tbPosterQual.AutoSize = False
        Me.tbPosterQual.LargeChange = 10
        Me.tbPosterQual.Location = New System.Drawing.Point(7, 136)
        Me.tbPosterQual.Maximum = 100
        Me.tbPosterQual.Name = "tbPosterQual"
        Me.tbPosterQual.Size = New System.Drawing.Size(179, 27)
        Me.tbPosterQual.TabIndex = 5
        Me.tbPosterQual.TickFrequency = 10
        Me.tbPosterQual.Value = 100
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(3, 119)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(75, 13)
        Me.Label24.TabIndex = 44
        Me.Label24.Text = "Poster Quality:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(3, 98)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 13)
        Me.Label11.TabIndex = 43
        Me.Label11.Text = "Max Width:"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(106, 98)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 13)
        Me.Label12.TabIndex = 42
        Me.Label12.Text = "Max Height:"
        '
        'chkResizePoster
        '
        Me.chkResizePoster.AutoSize = True
        Me.chkResizePoster.Location = New System.Drawing.Point(6, 76)
        Me.chkResizePoster.Name = "chkResizePoster"
        Me.chkResizePoster.Size = New System.Drawing.Size(159, 17)
        Me.chkResizePoster.TabIndex = 2
        Me.chkResizePoster.Text = "Automatically Resize Poster:"
        Me.chkResizePoster.UseVisualStyleBackColor = True
        '
        'lblPosterSize
        '
        Me.lblPosterSize.AutoSize = True
        Me.lblPosterSize.Location = New System.Drawing.Point(3, 16)
        Me.lblPosterSize.Name = "lblPosterSize"
        Me.lblPosterSize.Size = New System.Drawing.Size(106, 13)
        Me.lblPosterSize.TabIndex = 14
        Me.lblPosterSize.Text = "Preferred Poster Size"
        '
        'cbPosterSize
        '
        Me.cbPosterSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPosterSize.FormattingEnabled = True
        Me.cbPosterSize.Location = New System.Drawing.Point(6, 33)
        Me.cbPosterSize.Name = "cbPosterSize"
        Me.cbPosterSize.Size = New System.Drawing.Size(148, 21)
        Me.cbPosterSize.TabIndex = 0
        '
        'chkOverwritePoster
        '
        Me.chkOverwritePoster.AutoSize = True
        Me.chkOverwritePoster.Location = New System.Drawing.Point(6, 57)
        Me.chkOverwritePoster.Name = "chkOverwritePoster"
        Me.chkOverwritePoster.Size = New System.Drawing.Size(143, 17)
        Me.chkOverwritePoster.TabIndex = 1
        Me.chkOverwritePoster.Text = "Overwrite Existing Poster"
        Me.chkOverwritePoster.UseVisualStyleBackColor = True
        '
        'GroupBox13
        '
        Me.GroupBox13.Controls.Add(Me.txtFanartWidth)
        Me.GroupBox13.Controls.Add(Me.txtFanartHeight)
        Me.GroupBox13.Controls.Add(Me.chkFanartOnly)
        Me.GroupBox13.Controls.Add(Me.lblFanartQual)
        Me.GroupBox13.Controls.Add(Me.tbFanartQual)
        Me.GroupBox13.Controls.Add(Me.Label26)
        Me.GroupBox13.Controls.Add(Me.Label9)
        Me.GroupBox13.Controls.Add(Me.Label10)
        Me.GroupBox13.Controls.Add(Me.chkResizeFanart)
        Me.GroupBox13.Controls.Add(Me.cbFanartSize)
        Me.GroupBox13.Controls.Add(Me.lblFanartSize)
        Me.GroupBox13.Controls.Add(Me.chkOverwriteFanart)
        Me.GroupBox13.Location = New System.Drawing.Point(172, 175)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.Size = New System.Drawing.Size(218, 170)
        Me.GroupBox13.TabIndex = 3
        Me.GroupBox13.TabStop = False
        Me.GroupBox13.Text = "Fanart"
        '
        'txtFanartWidth
        '
        Me.txtFanartWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFanartWidth.Enabled = False
        Me.txtFanartWidth.Location = New System.Drawing.Point(65, 93)
        Me.txtFanartWidth.Name = "txtFanartWidth"
        Me.txtFanartWidth.Size = New System.Drawing.Size(38, 20)
        Me.txtFanartWidth.TabIndex = 4
        '
        'txtFanartHeight
        '
        Me.txtFanartHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFanartHeight.Enabled = False
        Me.txtFanartHeight.Location = New System.Drawing.Point(172, 93)
        Me.txtFanartHeight.Name = "txtFanartHeight"
        Me.txtFanartHeight.Size = New System.Drawing.Size(38, 20)
        Me.txtFanartHeight.TabIndex = 5
        '
        'chkFanartOnly
        '
        Me.chkFanartOnly.AutoSize = True
        Me.chkFanartOnly.Location = New System.Drawing.Point(163, 34)
        Me.chkFanartOnly.Name = "chkFanartOnly"
        Me.chkFanartOnly.Size = New System.Drawing.Size(47, 17)
        Me.chkFanartOnly.TabIndex = 1
        Me.chkFanartOnly.Text = "Only"
        Me.chkFanartOnly.UseVisualStyleBackColor = True
        '
        'lblFanartQual
        '
        Me.lblFanartQual.AutoSize = True
        Me.lblFanartQual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFanartQual.Location = New System.Drawing.Point(183, 144)
        Me.lblFanartQual.Name = "lblFanartQual"
        Me.lblFanartQual.Size = New System.Drawing.Size(32, 16)
        Me.lblFanartQual.TabIndex = 49
        Me.lblFanartQual.Text = "100"
        '
        'tbFanartQual
        '
        Me.tbFanartQual.AutoSize = False
        Me.tbFanartQual.LargeChange = 10
        Me.tbFanartQual.Location = New System.Drawing.Point(6, 136)
        Me.tbFanartQual.Maximum = 100
        Me.tbFanartQual.Name = "tbFanartQual"
        Me.tbFanartQual.Size = New System.Drawing.Size(180, 27)
        Me.tbFanartQual.TabIndex = 6
        Me.tbFanartQual.TickFrequency = 10
        Me.tbFanartQual.Value = 100
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(2, 119)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(75, 13)
        Me.Label26.TabIndex = 47
        Me.Label26.Text = "Fanart Quality:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(3, 97)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(61, 13)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "Max Width:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(106, 97)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(64, 13)
        Me.Label10.TabIndex = 42
        Me.Label10.Text = "Max Height:"
        '
        'chkResizeFanart
        '
        Me.chkResizeFanart.AutoSize = True
        Me.chkResizeFanart.Location = New System.Drawing.Point(6, 76)
        Me.chkResizeFanart.Name = "chkResizeFanart"
        Me.chkResizeFanart.Size = New System.Drawing.Size(159, 17)
        Me.chkResizeFanart.TabIndex = 3
        Me.chkResizeFanart.Text = "Automatically Resize Fanart:"
        Me.chkResizeFanart.UseVisualStyleBackColor = True
        '
        'cbFanartSize
        '
        Me.cbFanartSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFanartSize.FormattingEnabled = True
        Me.cbFanartSize.Location = New System.Drawing.Point(6, 30)
        Me.cbFanartSize.Name = "cbFanartSize"
        Me.cbFanartSize.Size = New System.Drawing.Size(148, 21)
        Me.cbFanartSize.TabIndex = 0
        '
        'lblFanartSize
        '
        Me.lblFanartSize.AutoSize = True
        Me.lblFanartSize.Location = New System.Drawing.Point(3, 14)
        Me.lblFanartSize.Name = "lblFanartSize"
        Me.lblFanartSize.Size = New System.Drawing.Size(106, 13)
        Me.lblFanartSize.TabIndex = 15
        Me.lblFanartSize.Text = "Preferred Fanart Size"
        '
        'chkOverwriteFanart
        '
        Me.chkOverwriteFanart.AutoSize = True
        Me.chkOverwriteFanart.Location = New System.Drawing.Point(6, 56)
        Me.chkOverwriteFanart.Name = "chkOverwriteFanart"
        Me.chkOverwriteFanart.Size = New System.Drawing.Size(143, 17)
        Me.chkOverwriteFanart.TabIndex = 2
        Me.chkOverwriteFanart.Text = "Overwrite Existing Fanart"
        Me.chkOverwriteFanart.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.chkLockOutline)
        Me.GroupBox10.Controls.Add(Me.chkLockPlot)
        Me.GroupBox10.Controls.Add(Me.chkLockTrailer)
        Me.GroupBox10.Controls.Add(Me.chkLockGenre)
        Me.GroupBox10.Controls.Add(Me.chkLockRealStudio)
        Me.GroupBox10.Controls.Add(Me.chkLockRating)
        Me.GroupBox10.Controls.Add(Me.chkLockTagline)
        Me.GroupBox10.Controls.Add(Me.chkLockTitle)
        Me.GroupBox10.Location = New System.Drawing.Point(1, 5)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(187, 164)
        Me.GroupBox10.TabIndex = 1
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Global Locks (Do not allow updates during scraping)"
        '
        'chkLockOutline
        '
        Me.chkLockOutline.Location = New System.Drawing.Point(6, 63)
        Me.chkLockOutline.Name = "chkLockOutline"
        Me.chkLockOutline.Size = New System.Drawing.Size(177, 17)
        Me.chkLockOutline.TabIndex = 1
        Me.chkLockOutline.Text = "Lock Outline"
        Me.chkLockOutline.UseVisualStyleBackColor = True
        '
        'chkLockPlot
        '
        Me.chkLockPlot.Location = New System.Drawing.Point(6, 47)
        Me.chkLockPlot.Name = "chkLockPlot"
        Me.chkLockPlot.Size = New System.Drawing.Size(177, 17)
        Me.chkLockPlot.TabIndex = 0
        Me.chkLockPlot.Text = "Lock Plot"
        Me.chkLockPlot.UseVisualStyleBackColor = True
        '
        'chkLockTrailer
        '
        Me.chkLockTrailer.Location = New System.Drawing.Point(6, 143)
        Me.chkLockTrailer.Name = "chkLockTrailer"
        Me.chkLockTrailer.Size = New System.Drawing.Size(177, 17)
        Me.chkLockTrailer.TabIndex = 46
        Me.chkLockTrailer.Text = "Lock Trailer"
        Me.chkLockTrailer.UseVisualStyleBackColor = True
        '
        'chkLockGenre
        '
        Me.chkLockGenre.Location = New System.Drawing.Point(6, 127)
        Me.chkLockGenre.Name = "chkLockGenre"
        Me.chkLockGenre.Size = New System.Drawing.Size(177, 17)
        Me.chkLockGenre.TabIndex = 7
        Me.chkLockGenre.Text = "Lock Genre"
        Me.chkLockGenre.UseVisualStyleBackColor = True
        '
        'chkLockRealStudio
        '
        Me.chkLockRealStudio.Location = New System.Drawing.Point(6, 111)
        Me.chkLockRealStudio.Name = "chkLockRealStudio"
        Me.chkLockRealStudio.Size = New System.Drawing.Size(177, 17)
        Me.chkLockRealStudio.TabIndex = 5
        Me.chkLockRealStudio.Text = "Lock Studio"
        Me.chkLockRealStudio.UseVisualStyleBackColor = True
        '
        'chkLockRating
        '
        Me.chkLockRating.Location = New System.Drawing.Point(6, 95)
        Me.chkLockRating.Name = "chkLockRating"
        Me.chkLockRating.Size = New System.Drawing.Size(177, 17)
        Me.chkLockRating.TabIndex = 4
        Me.chkLockRating.Text = "Lock Rating"
        Me.chkLockRating.UseVisualStyleBackColor = True
        '
        'chkLockTagline
        '
        Me.chkLockTagline.Location = New System.Drawing.Point(6, 79)
        Me.chkLockTagline.Name = "chkLockTagline"
        Me.chkLockTagline.Size = New System.Drawing.Size(177, 17)
        Me.chkLockTagline.TabIndex = 3
        Me.chkLockTagline.Text = "Lock Tagline"
        Me.chkLockTagline.UseVisualStyleBackColor = True
        '
        'chkLockTitle
        '
        Me.chkLockTitle.Location = New System.Drawing.Point(6, 31)
        Me.chkLockTitle.Name = "chkLockTitle"
        Me.chkLockTitle.Size = New System.Drawing.Size(177, 17)
        Me.chkLockTitle.TabIndex = 2
        Me.chkLockTitle.Text = "Lock Title"
        Me.chkLockTitle.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.chkNoSaveImagesToNfo)
        Me.GroupBox9.Controls.Add(Me.chkSingleScrapeImages)
        Me.GroupBox9.Controls.Add(Me.chkUseMPDB)
        Me.GroupBox9.Controls.Add(Me.chkUseTMDB)
        Me.GroupBox9.Controls.Add(Me.chkUseIMPA)
        Me.GroupBox9.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(166, 184)
        Me.GroupBox9.TabIndex = 0
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Images"
        '
        'chkNoSaveImagesToNfo
        '
        Me.chkNoSaveImagesToNfo.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkNoSaveImagesToNfo.Location = New System.Drawing.Point(6, 149)
        Me.chkNoSaveImagesToNfo.Name = "chkNoSaveImagesToNfo"
        Me.chkNoSaveImagesToNfo.Size = New System.Drawing.Size(156, 30)
        Me.chkNoSaveImagesToNfo.TabIndex = 4
        Me.chkNoSaveImagesToNfo.Text = "Do Not Save Image URLs to Nfo"
        Me.chkNoSaveImagesToNfo.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkNoSaveImagesToNfo.UseVisualStyleBackColor = True
        '
        'chkSingleScrapeImages
        '
        Me.chkSingleScrapeImages.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSingleScrapeImages.Location = New System.Drawing.Point(6, 116)
        Me.chkSingleScrapeImages.Name = "chkSingleScrapeImages"
        Me.chkSingleScrapeImages.Size = New System.Drawing.Size(156, 30)
        Me.chkSingleScrapeImages.TabIndex = 3
        Me.chkSingleScrapeImages.Text = "Scrape Images on Single Scrape"
        Me.chkSingleScrapeImages.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSingleScrapeImages.UseVisualStyleBackColor = True
        '
        'chkUseMPDB
        '
        Me.chkUseMPDB.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseMPDB.Location = New System.Drawing.Point(6, 83)
        Me.chkUseMPDB.Name = "chkUseMPDB"
        Me.chkUseMPDB.Size = New System.Drawing.Size(156, 30)
        Me.chkUseMPDB.TabIndex = 2
        Me.chkUseMPDB.Text = "Get Images From MoviePostersDB"
        Me.chkUseMPDB.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseMPDB.UseVisualStyleBackColor = True
        '
        'chkUseTMDB
        '
        Me.chkUseTMDB.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseTMDB.Location = New System.Drawing.Point(6, 19)
        Me.chkUseTMDB.Name = "chkUseTMDB"
        Me.chkUseTMDB.Size = New System.Drawing.Size(156, 30)
        Me.chkUseTMDB.TabIndex = 0
        Me.chkUseTMDB.Text = "Get Images From TMDB"
        Me.chkUseTMDB.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseTMDB.UseVisualStyleBackColor = True
        '
        'chkUseIMPA
        '
        Me.chkUseIMPA.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseIMPA.Location = New System.Drawing.Point(6, 51)
        Me.chkUseIMPA.Name = "chkUseIMPA"
        Me.chkUseIMPA.Size = New System.Drawing.Size(156, 30)
        Me.chkUseIMPA.TabIndex = 1
        Me.chkUseIMPA.Text = "Get Images From IMPAwards"
        Me.chkUseIMPA.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseIMPA.UseVisualStyleBackColor = True
        '
        'chkUseETasFA
        '
        Me.chkUseETasFA.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseETasFA.Enabled = False
        Me.chkUseETasFA.Location = New System.Drawing.Point(24, 104)
        Me.chkUseETasFA.Name = "chkUseETasFA"
        Me.chkUseETasFA.Size = New System.Drawing.Size(171, 30)
        Me.chkUseETasFA.TabIndex = 8
        Me.chkUseETasFA.Text = "Use Extrathumb if no Fanart Found"
        Me.chkUseETasFA.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseETasFA.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(22, 80)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(172, 24)
        Me.Label17.TabIndex = 63
        Me.Label17.Text = "(If checked, Ember will use only the first half of the movie to extract thumbs)"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkNoSpoilers
        '
        Me.chkNoSpoilers.AutoSize = True
        Me.chkNoSpoilers.Enabled = False
        Me.chkNoSpoilers.Location = New System.Drawing.Point(25, 64)
        Me.chkNoSpoilers.Name = "chkNoSpoilers"
        Me.chkNoSpoilers.Size = New System.Drawing.Size(80, 17)
        Me.chkNoSpoilers.TabIndex = 7
        Me.chkNoSpoilers.Text = "No Spoilers"
        Me.chkNoSpoilers.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(22, 47)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(97, 13)
        Me.Label15.TabIndex = 61
        Me.Label15.Text = "Number To Create:"
        '
        'txtAutoThumbs
        '
        Me.txtAutoThumbs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAutoThumbs.Enabled = False
        Me.txtAutoThumbs.Location = New System.Drawing.Point(122, 44)
        Me.txtAutoThumbs.Name = "txtAutoThumbs"
        Me.txtAutoThumbs.Size = New System.Drawing.Size(53, 20)
        Me.txtAutoThumbs.TabIndex = 6
        '
        'chkAutoThumbs
        '
        Me.chkAutoThumbs.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoThumbs.Location = New System.Drawing.Point(6, 14)
        Me.chkAutoThumbs.Name = "chkAutoThumbs"
        Me.chkAutoThumbs.Size = New System.Drawing.Size(188, 30)
        Me.chkAutoThumbs.TabIndex = 5
        Me.chkAutoThumbs.Text = "Automatically Extract Extrathumbs During Scrapers"
        Me.chkAutoThumbs.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoThumbs.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkOutlineForPlot)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.txtIMDBURL)
        Me.GroupBox1.Controls.Add(Me.chkCastWithImg)
        Me.GroupBox1.Controls.Add(Me.chkUseCertForMPAA)
        Me.GroupBox1.Controls.Add(Me.chkFullCast)
        Me.GroupBox1.Controls.Add(Me.chkFullCrew)
        Me.GroupBox1.Controls.Add(Me.cbCert)
        Me.GroupBox1.Controls.Add(Me.chkCert)
        Me.GroupBox1.Location = New System.Drawing.Point(306, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(269, 177)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Miscellaneous"
        '
        'chkOutlineForPlot
        '
        Me.chkOutlineForPlot.AutoSize = True
        Me.chkOutlineForPlot.Location = New System.Drawing.Point(6, 63)
        Me.chkOutlineForPlot.Name = "chkOutlineForPlot"
        Me.chkOutlineForPlot.Size = New System.Drawing.Size(188, 17)
        Me.chkOutlineForPlot.TabIndex = 3
        Me.chkOutlineForPlot.Text = "Use Outline for Plot if Plot is Empty"
        Me.chkOutlineForPlot.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(8, 137)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(66, 13)
        Me.Label18.TabIndex = 62
        Me.Label18.Text = "IMDB Mirror:"
        '
        'txtIMDBURL
        '
        Me.txtIMDBURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIMDBURL.Location = New System.Drawing.Point(10, 150)
        Me.txtIMDBURL.Name = "txtIMDBURL"
        Me.txtIMDBURL.Size = New System.Drawing.Size(192, 20)
        Me.txtIMDBURL.TabIndex = 10
        '
        'chkCastWithImg
        '
        Me.chkCastWithImg.AutoSize = True
        Me.chkCastWithImg.Location = New System.Drawing.Point(6, 31)
        Me.chkCastWithImg.Name = "chkCastWithImg"
        Me.chkCastWithImg.Size = New System.Drawing.Size(179, 17)
        Me.chkCastWithImg.TabIndex = 1
        Me.chkCastWithImg.Text = "Scrape Only Actors With Images"
        Me.chkCastWithImg.UseVisualStyleBackColor = True
        '
        'chkUseCertForMPAA
        '
        Me.chkUseCertForMPAA.AutoSize = True
        Me.chkUseCertForMPAA.Enabled = False
        Me.chkUseCertForMPAA.Location = New System.Drawing.Point(20, 120)
        Me.chkUseCertForMPAA.Name = "chkUseCertForMPAA"
        Me.chkUseCertForMPAA.Size = New System.Drawing.Size(151, 17)
        Me.chkUseCertForMPAA.TabIndex = 6
        Me.chkUseCertForMPAA.Text = "Use Certification for MPAA"
        Me.chkUseCertForMPAA.UseVisualStyleBackColor = True
        '
        'chkFullCast
        '
        Me.chkFullCast.AutoSize = True
        Me.chkFullCast.Location = New System.Drawing.Point(6, 15)
        Me.chkFullCast.Name = "chkFullCast"
        Me.chkFullCast.Size = New System.Drawing.Size(103, 17)
        Me.chkFullCast.TabIndex = 0
        Me.chkFullCast.Text = "Scrape Full Cast"
        Me.chkFullCast.UseVisualStyleBackColor = True
        '
        'chkFullCrew
        '
        Me.chkFullCrew.AutoSize = True
        Me.chkFullCrew.Location = New System.Drawing.Point(6, 47)
        Me.chkFullCrew.Name = "chkFullCrew"
        Me.chkFullCrew.Size = New System.Drawing.Size(106, 17)
        Me.chkFullCrew.TabIndex = 2
        Me.chkFullCrew.Text = "Scrape Full Crew"
        Me.chkFullCrew.UseVisualStyleBackColor = True
        '
        'cbCert
        '
        Me.cbCert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCert.Enabled = False
        Me.cbCert.FormattingEnabled = True
        Me.cbCert.Items.AddRange(New Object() {"Argentina", "Australia", "Belgium", "Brazil", "Canada", "Finland", "France", "Germany", "Hong Kong", "Iceland", "Ireland", "Netherlands", "New Zealand", "Peru", "Portugal", "Singapore", "South Korea", "Spain", "Sweden", "Switzerland", "UK", "USA"})
        Me.cbCert.Location = New System.Drawing.Point(6, 97)
        Me.cbCert.Name = "cbCert"
        Me.cbCert.Size = New System.Drawing.Size(179, 21)
        Me.cbCert.Sorted = True
        Me.cbCert.TabIndex = 5
        '
        'chkCert
        '
        Me.chkCert.AutoSize = True
        Me.chkCert.Location = New System.Drawing.Point(6, 79)
        Me.chkCert.Name = "chkCert"
        Me.chkCert.Size = New System.Drawing.Size(157, 17)
        Me.chkCert.TabIndex = 4
        Me.chkCert.Text = "Use Certification Language:"
        Me.chkCert.UseVisualStyleBackColor = True
        '
        'gbRTFormat
        '
        Me.gbRTFormat.Controls.Add(Me.rbHM)
        Me.gbRTFormat.Controls.Add(Me.rbMins)
        Me.gbRTFormat.Enabled = False
        Me.gbRTFormat.Location = New System.Drawing.Point(18, 50)
        Me.gbRTFormat.Name = "gbRTFormat"
        Me.gbRTFormat.Size = New System.Drawing.Size(169, 39)
        Me.gbRTFormat.TabIndex = 9
        Me.gbRTFormat.TabStop = False
        Me.gbRTFormat.Text = "Runtime Format"
        '
        'rbHM
        '
        Me.rbHM.AutoSize = True
        Me.rbHM.Location = New System.Drawing.Point(75, 15)
        Me.rbHM.Name = "rbHM"
        Me.rbHM.Size = New System.Drawing.Size(83, 17)
        Me.rbHM.TabIndex = 1
        Me.rbHM.Text = "X hrs X mins"
        Me.rbHM.UseVisualStyleBackColor = True
        '
        'rbMins
        '
        Me.rbMins.AutoSize = True
        Me.rbMins.Checked = True
        Me.rbMins.Location = New System.Drawing.Point(6, 15)
        Me.rbMins.Name = "rbMins"
        Me.rbMins.Size = New System.Drawing.Size(56, 17)
        Me.rbMins.TabIndex = 0
        Me.rbMins.TabStop = True
        Me.rbMins.Text = "X mins"
        Me.rbMins.UseVisualStyleBackColor = True
        '
        'chkUseMIDuration
        '
        Me.chkUseMIDuration.AutoSize = True
        Me.chkUseMIDuration.Enabled = False
        Me.chkUseMIDuration.Location = New System.Drawing.Point(18, 33)
        Me.chkUseMIDuration.Name = "chkUseMIDuration"
        Me.chkUseMIDuration.Size = New System.Drawing.Size(145, 17)
        Me.chkUseMIDuration.TabIndex = 8
        Me.chkUseMIDuration.Text = "Use Duration for Runtime"
        Me.chkUseMIDuration.UseVisualStyleBackColor = True
        '
        'chkScanMediaInfo
        '
        Me.chkScanMediaInfo.AutoSize = True
        Me.chkScanMediaInfo.Location = New System.Drawing.Point(5, 16)
        Me.chkScanMediaInfo.Name = "chkScanMediaInfo"
        Me.chkScanMediaInfo.Size = New System.Drawing.Size(104, 17)
        Me.chkScanMediaInfo.TabIndex = 7
        Me.chkScanMediaInfo.Text = "Scan Meta Data"
        Me.chkScanMediaInfo.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(730, 455)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 22
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Enabled = False
        Me.btnApply.Location = New System.Drawing.Point(567, 455)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 20
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(649, 455)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 21
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.SteelBlue
        Me.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.Label4)
        Me.pnlTop.Controls.Add(Me.PictureBox1)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(810, 64)
        Me.pnlTop.TabIndex = 57
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(61, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(223, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Configure Ember's appearance and operation."
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(58, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(108, 29)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Settings"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(7, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'ilSettings
        '
        Me.ilSettings.ImageStream = CType(resources.GetObject("ilSettings.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilSettings.TransparentColor = System.Drawing.Color.Transparent
        Me.ilSettings.Images.SetKeyName(0, "process.png")
        Me.ilSettings.Images.SetKeyName(1, "comments.png")
        Me.ilSettings.Images.SetKeyName(2, "film.png")
        Me.ilSettings.Images.SetKeyName(3, "copy_paste.png")
        Me.ilSettings.Images.SetKeyName(4, "attachment.png")
        Me.ilSettings.Images.SetKeyName(5, "folder_full.png")
        Me.ilSettings.Images.SetKeyName(6, "image.png")
        '
        'tvSettings
        '
        Me.tvSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.tvSettings.FullRowSelect = True
        Me.tvSettings.HideSelection = False
        Me.tvSettings.ImageIndex = 0
        Me.tvSettings.ImageList = Me.ilSettings
        Me.tvSettings.Location = New System.Drawing.Point(4, 70)
        Me.tvSettings.Name = "tvSettings"
        TreeNode8.ImageIndex = 4
        TreeNode8.Name = "nExts"
        TreeNode8.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode8.SelectedImageIndex = 4
        TreeNode8.Text = "File System"
        TreeNode9.ImageIndex = 1
        TreeNode9.Name = "nXBMCCom"
        TreeNode9.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode9.SelectedImageIndex = 1
        TreeNode9.Text = "XBMC Communication"
        TreeNode10.ImageIndex = 0
        TreeNode10.Name = "nGeneral"
        TreeNode10.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode10.SelectedImageIndex = 0
        TreeNode10.Text = "General"
        TreeNode11.ImageIndex = 5
        TreeNode11.Name = "nSources"
        TreeNode11.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode11.SelectedImageIndex = 5
        TreeNode11.Text = "Files and Sources"
        TreeNode12.ImageIndex = 3
        TreeNode12.Name = "nScraper"
        TreeNode12.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode12.SelectedImageIndex = 3
        TreeNode12.Text = "Scraper - Data"
        TreeNode13.ImageIndex = 6
        TreeNode13.Name = "nImages"
        TreeNode13.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode13.SelectedImageIndex = 6
        TreeNode13.Text = "Scraper - Images"
        TreeNode14.ImageIndex = 2
        TreeNode14.Name = "nMovies"
        TreeNode14.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode14.SelectedImageIndex = 2
        TreeNode14.Text = "Movies"
        Me.tvSettings.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode10, TreeNode14})
        Me.tvSettings.SelectedImageIndex = 0
        Me.tvSettings.ShowLines = False
        Me.tvSettings.ShowPlusMinus = False
        Me.tvSettings.Size = New System.Drawing.Size(199, 379)
        Me.tvSettings.TabIndex = 58
        '
        'pnlGeneral
        '
        Me.pnlGeneral.BackColor = System.Drawing.Color.White
        Me.pnlGeneral.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlGeneral.Controls.Add(Me.GroupBox3)
        Me.pnlGeneral.Controls.Add(Me.GroupBox4)
        Me.pnlGeneral.Controls.Add(Me.gbFilters)
        Me.pnlGeneral.Location = New System.Drawing.Point(208, 96)
        Me.pnlGeneral.Name = "pnlGeneral"
        Me.pnlGeneral.Size = New System.Drawing.Size(597, 353)
        Me.pnlGeneral.TabIndex = 59
        '
        'pnlXBMCCom
        '
        Me.pnlXBMCCom.BackColor = System.Drawing.Color.White
        Me.pnlXBMCCom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlXBMCCom.Controls.Add(Me.btnRemoveCom)
        Me.pnlXBMCCom.Controls.Add(Me.lbXBMCCom)
        Me.pnlXBMCCom.Controls.Add(Me.GroupBox11)
        Me.pnlXBMCCom.Location = New System.Drawing.Point(208, 96)
        Me.pnlXBMCCom.Name = "pnlXBMCCom"
        Me.pnlXBMCCom.Size = New System.Drawing.Size(597, 353)
        Me.pnlXBMCCom.TabIndex = 60
        Me.pnlXBMCCom.Visible = False
        '
        'btnRemoveCom
        '
        Me.btnRemoveCom.Image = CType(resources.GetObject("btnRemoveCom.Image"), System.Drawing.Image)
        Me.btnRemoveCom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveCom.Location = New System.Drawing.Point(137, 225)
        Me.btnRemoveCom.Name = "btnRemoveCom"
        Me.btnRemoveCom.Size = New System.Drawing.Size(123, 23)
        Me.btnRemoveCom.TabIndex = 1
        Me.btnRemoveCom.Text = "Remove Selected"
        Me.btnRemoveCom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveCom.UseVisualStyleBackColor = True
        '
        'lbXBMCCom
        '
        Me.lbXBMCCom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbXBMCCom.FormattingEnabled = True
        Me.lbXBMCCom.ItemHeight = 16
        Me.lbXBMCCom.Location = New System.Drawing.Point(6, 6)
        Me.lbXBMCCom.Name = "lbXBMCCom"
        Me.lbXBMCCom.Size = New System.Drawing.Size(254, 212)
        Me.lbXBMCCom.Sorted = True
        Me.lbXBMCCom.TabIndex = 0
        '
        'pnlMovies
        '
        Me.pnlMovies.BackColor = System.Drawing.Color.White
        Me.pnlMovies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMovies.Controls.Add(Me.GroupBox27)
        Me.pnlMovies.Controls.Add(Me.GroupBox12)
        Me.pnlMovies.Controls.Add(Me.GroupBox2)
        Me.pnlMovies.Location = New System.Drawing.Point(208, 96)
        Me.pnlMovies.Name = "pnlMovies"
        Me.pnlMovies.Size = New System.Drawing.Size(597, 353)
        Me.pnlMovies.TabIndex = 61
        Me.pnlMovies.Visible = False
        '
        'GroupBox27
        '
        Me.GroupBox27.Controls.Add(Me.chkMissingExtra)
        Me.GroupBox27.Controls.Add(Me.chkMissingSubs)
        Me.GroupBox27.Controls.Add(Me.chkMissingTrailer)
        Me.GroupBox27.Controls.Add(Me.chkMissingNFO)
        Me.GroupBox27.Controls.Add(Me.chkMissingFanart)
        Me.GroupBox27.Controls.Add(Me.chkMissingPoster)
        Me.GroupBox27.Location = New System.Drawing.Point(426, 8)
        Me.GroupBox27.Name = "GroupBox27"
        Me.GroupBox27.Size = New System.Drawing.Size(164, 123)
        Me.GroupBox27.TabIndex = 5
        Me.GroupBox27.TabStop = False
        Me.GroupBox27.Text = "Missing Items Filter"
        '
        'chkMissingExtra
        '
        Me.chkMissingExtra.AutoSize = True
        Me.chkMissingExtra.Location = New System.Drawing.Point(8, 98)
        Me.chkMissingExtra.Name = "chkMissingExtra"
        Me.chkMissingExtra.Size = New System.Drawing.Size(133, 17)
        Me.chkMissingExtra.TabIndex = 11
        Me.chkMissingExtra.Text = "Check for Extrathumbs"
        Me.chkMissingExtra.UseVisualStyleBackColor = True
        '
        'chkMissingSubs
        '
        Me.chkMissingSubs.AutoSize = True
        Me.chkMissingSubs.Location = New System.Drawing.Point(8, 82)
        Me.chkMissingSubs.Name = "chkMissingSubs"
        Me.chkMissingSubs.Size = New System.Drawing.Size(99, 17)
        Me.chkMissingSubs.TabIndex = 10
        Me.chkMissingSubs.Text = "Check for Subs"
        Me.chkMissingSubs.UseVisualStyleBackColor = True
        '
        'chkMissingTrailer
        '
        Me.chkMissingTrailer.AutoSize = True
        Me.chkMissingTrailer.Location = New System.Drawing.Point(8, 66)
        Me.chkMissingTrailer.Name = "chkMissingTrailer"
        Me.chkMissingTrailer.Size = New System.Drawing.Size(104, 17)
        Me.chkMissingTrailer.TabIndex = 9
        Me.chkMissingTrailer.Text = "Check for Trailer"
        Me.chkMissingTrailer.UseVisualStyleBackColor = True
        '
        'chkMissingNFO
        '
        Me.chkMissingNFO.AutoSize = True
        Me.chkMissingNFO.Location = New System.Drawing.Point(8, 50)
        Me.chkMissingNFO.Name = "chkMissingNFO"
        Me.chkMissingNFO.Size = New System.Drawing.Size(97, 17)
        Me.chkMissingNFO.TabIndex = 8
        Me.chkMissingNFO.Text = "Check for NFO"
        Me.chkMissingNFO.UseVisualStyleBackColor = True
        '
        'chkMissingFanart
        '
        Me.chkMissingFanart.AutoSize = True
        Me.chkMissingFanart.Location = New System.Drawing.Point(8, 34)
        Me.chkMissingFanart.Name = "chkMissingFanart"
        Me.chkMissingFanart.Size = New System.Drawing.Size(105, 17)
        Me.chkMissingFanart.TabIndex = 7
        Me.chkMissingFanart.Text = "Check for Fanart"
        Me.chkMissingFanart.UseVisualStyleBackColor = True
        '
        'chkMissingPoster
        '
        Me.chkMissingPoster.AutoSize = True
        Me.chkMissingPoster.Location = New System.Drawing.Point(8, 18)
        Me.chkMissingPoster.Name = "chkMissingPoster"
        Me.chkMissingPoster.Size = New System.Drawing.Size(105, 17)
        Me.chkMissingPoster.TabIndex = 6
        Me.chkMissingPoster.Text = "Check for Poster"
        Me.chkMissingPoster.UseVisualStyleBackColor = True
        '
        'GroupBox16
        '
        Me.GroupBox16.Controls.Add(Me.chkAutoBD)
        Me.GroupBox16.Controls.Add(Me.btnBrowse)
        Me.GroupBox16.Controls.Add(Me.txtBDPath)
        Me.GroupBox16.Location = New System.Drawing.Point(5, 307)
        Me.GroupBox16.Name = "GroupBox16"
        Me.GroupBox16.Size = New System.Drawing.Size(583, 41)
        Me.GroupBox16.TabIndex = 6
        Me.GroupBox16.TabStop = False
        Me.GroupBox16.Text = "Backdrops Folder"
        '
        'chkAutoBD
        '
        Me.chkAutoBD.AutoSize = True
        Me.chkAutoBD.Location = New System.Drawing.Point(276, 19)
        Me.chkAutoBD.Name = "chkAutoBD"
        Me.chkAutoBD.Size = New System.Drawing.Size(251, 17)
        Me.chkAutoBD.TabIndex = 2
        Me.chkAutoBD.Text = "Automatically Save Fanart To Backdrops Folder"
        Me.chkAutoBD.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(248, 15)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(23, 23)
        Me.btnBrowse.TabIndex = 1
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtBDPath
        '
        Me.txtBDPath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtBDPath.Location = New System.Drawing.Point(7, 16)
        Me.txtBDPath.Name = "txtBDPath"
        Me.txtBDPath.Size = New System.Drawing.Size(237, 20)
        Me.txtBDPath.TabIndex = 0
        '
        'pnlScraper
        '
        Me.pnlScraper.BackColor = System.Drawing.Color.White
        Me.pnlScraper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlScraper.Controls.Add(Me.TabControl1)
        Me.pnlScraper.Location = New System.Drawing.Point(208, 96)
        Me.pnlScraper.Name = "pnlScraper"
        Me.pnlScraper.Size = New System.Drawing.Size(597, 353)
        Me.pnlScraper.TabIndex = 62
        Me.pnlScraper.Visible = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(3, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(591, 346)
        Me.TabControl1.TabIndex = 69
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox26)
        Me.TabPage1.Controls.Add(Me.GroupBox10)
        Me.TabPage1.Controls.Add(Me.GroupBox20)
        Me.TabPage1.Controls.Add(Me.GroupBox15)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(583, 320)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "General"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox26
        '
        Me.GroupBox26.Controls.Add(Me.cbLanguages)
        Me.GroupBox26.Controls.Add(Me.Label31)
        Me.GroupBox26.Controls.Add(Me.gbRTFormat)
        Me.GroupBox26.Controls.Add(Me.chkScanMediaInfo)
        Me.GroupBox26.Controls.Add(Me.chkUseMIDuration)
        Me.GroupBox26.Location = New System.Drawing.Point(371, 5)
        Me.GroupBox26.Name = "GroupBox26"
        Me.GroupBox26.Size = New System.Drawing.Size(211, 164)
        Me.GroupBox26.TabIndex = 63
        Me.GroupBox26.TabStop = False
        Me.GroupBox26.Text = "Meta Data"
        '
        'GroupBox20
        '
        Me.GroupBox20.Controls.Add(Me.chkDeleteAllTrailers)
        Me.GroupBox20.Controls.Add(Me.chkOverwriteTrailer)
        Me.GroupBox20.Controls.Add(Me.chkNoDLTrailer)
        Me.GroupBox20.Controls.Add(Me.chkSingleScrapeTrailer)
        Me.GroupBox20.Controls.Add(Me.Label23)
        Me.GroupBox20.Controls.Add(Me.txtTimeout)
        Me.GroupBox20.Controls.Add(Me.chkUpdaterTrailer)
        Me.GroupBox20.Controls.Add(Me.Label22)
        Me.GroupBox20.Controls.Add(Me.lbTrailerSites)
        Me.GroupBox20.Controls.Add(Me.chkDownloadTrailer)
        Me.GroupBox20.Location = New System.Drawing.Point(1, 173)
        Me.GroupBox20.Name = "GroupBox20"
        Me.GroupBox20.Size = New System.Drawing.Size(581, 145)
        Me.GroupBox20.TabIndex = 3
        Me.GroupBox20.TabStop = False
        Me.GroupBox20.Text = "Trailers"
        '
        'chkDeleteAllTrailers
        '
        Me.chkDeleteAllTrailers.AutoSize = True
        Me.chkDeleteAllTrailers.Enabled = False
        Me.chkDeleteAllTrailers.Location = New System.Drawing.Point(19, 108)
        Me.chkDeleteAllTrailers.Name = "chkDeleteAllTrailers"
        Me.chkDeleteAllTrailers.Size = New System.Drawing.Size(147, 17)
        Me.chkDeleteAllTrailers.TabIndex = 5
        Me.chkDeleteAllTrailers.Text = "Delete All Existing Trailers"
        Me.chkDeleteAllTrailers.UseVisualStyleBackColor = True
        '
        'chkOverwriteTrailer
        '
        Me.chkOverwriteTrailer.AutoSize = True
        Me.chkOverwriteTrailer.Enabled = False
        Me.chkOverwriteTrailer.Location = New System.Drawing.Point(19, 90)
        Me.chkOverwriteTrailer.Name = "chkOverwriteTrailer"
        Me.chkOverwriteTrailer.Size = New System.Drawing.Size(103, 17)
        Me.chkOverwriteTrailer.TabIndex = 4
        Me.chkOverwriteTrailer.Text = "Overwrite Trailer"
        Me.chkOverwriteTrailer.UseVisualStyleBackColor = True
        '
        'chkNoDLTrailer
        '
        Me.chkNoDLTrailer.AutoSize = True
        Me.chkNoDLTrailer.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkNoDLTrailer.Enabled = False
        Me.chkNoDLTrailer.Location = New System.Drawing.Point(19, 52)
        Me.chkNoDLTrailer.Name = "chkNoDLTrailer"
        Me.chkNoDLTrailer.Size = New System.Drawing.Size(176, 17)
        Me.chkNoDLTrailer.TabIndex = 2
        Me.chkNoDLTrailer.Text = "Only Get URLs During Scrapers"
        Me.chkNoDLTrailer.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkNoDLTrailer.UseVisualStyleBackColor = True
        '
        'chkSingleScrapeTrailer
        '
        Me.chkSingleScrapeTrailer.AutoSize = True
        Me.chkSingleScrapeTrailer.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSingleScrapeTrailer.Enabled = False
        Me.chkSingleScrapeTrailer.Location = New System.Drawing.Point(19, 71)
        Me.chkSingleScrapeTrailer.Name = "chkSingleScrapeTrailer"
        Me.chkSingleScrapeTrailer.Size = New System.Drawing.Size(183, 17)
        Me.chkSingleScrapeTrailer.TabIndex = 3
        Me.chkSingleScrapeTrailer.Text = "Get Trailers During Single-Scrape"
        Me.chkSingleScrapeTrailer.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSingleScrapeTrailer.UseVisualStyleBackColor = True
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(427, 122)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(48, 13)
        Me.Label23.TabIndex = 64
        Me.Label23.Text = "Timeout:"
        '
        'txtTimeout
        '
        Me.txtTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeout.Enabled = False
        Me.txtTimeout.Location = New System.Drawing.Point(514, 118)
        Me.txtTimeout.Name = "txtTimeout"
        Me.txtTimeout.Size = New System.Drawing.Size(36, 20)
        Me.txtTimeout.TabIndex = 7
        '
        'chkUpdaterTrailer
        '
        Me.chkUpdaterTrailer.AutoSize = True
        Me.chkUpdaterTrailer.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUpdaterTrailer.Enabled = False
        Me.chkUpdaterTrailer.Location = New System.Drawing.Point(19, 33)
        Me.chkUpdaterTrailer.Name = "chkUpdaterTrailer"
        Me.chkUpdaterTrailer.Size = New System.Drawing.Size(211, 17)
        Me.chkUpdaterTrailer.TabIndex = 1
        Me.chkUpdaterTrailer.Text = "Get Trailers During ""All Items"" Scrapers"
        Me.chkUpdaterTrailer.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUpdaterTrailer.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(429, 13)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(117, 13)
        Me.Label22.TabIndex = 2
        Me.Label22.Text = "Supported Trailer Sites:"
        '
        'lbTrailerSites
        '
        Me.lbTrailerSites.CheckOnClick = True
        Me.lbTrailerSites.Enabled = False
        Me.lbTrailerSites.FormattingEnabled = True
        Me.lbTrailerSites.Items.AddRange(New Object() {"YouTube/TMDB", "IMDB"})
        Me.lbTrailerSites.Location = New System.Drawing.Point(430, 30)
        Me.lbTrailerSites.Name = "lbTrailerSites"
        Me.lbTrailerSites.Size = New System.Drawing.Size(120, 79)
        Me.lbTrailerSites.TabIndex = 6
        '
        'chkDownloadTrailer
        '
        Me.chkDownloadTrailer.AutoSize = True
        Me.chkDownloadTrailer.Location = New System.Drawing.Point(6, 17)
        Me.chkDownloadTrailer.Name = "chkDownloadTrailer"
        Me.chkDownloadTrailer.Size = New System.Drawing.Size(156, 17)
        Me.chkDownloadTrailer.TabIndex = 0
        Me.chkDownloadTrailer.Text = "Enable Trailer Downloading"
        Me.chkDownloadTrailer.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.gbOptions)
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(583, 320)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Options"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'gbOptions
        '
        Me.gbOptions.Controls.Add(Me.chkTop250)
        Me.gbOptions.Controls.Add(Me.txtGenreLimit)
        Me.gbOptions.Controls.Add(Me.lblLimit2)
        Me.gbOptions.Controls.Add(Me.txtActorLimit)
        Me.gbOptions.Controls.Add(Me.lblLimit)
        Me.gbOptions.Controls.Add(Me.chkCrew)
        Me.gbOptions.Controls.Add(Me.chkMusicBy)
        Me.gbOptions.Controls.Add(Me.chkProducers)
        Me.gbOptions.Controls.Add(Me.chkWriters)
        Me.gbOptions.Controls.Add(Me.chkStudio)
        Me.gbOptions.Controls.Add(Me.chkRuntime)
        Me.gbOptions.Controls.Add(Me.chkPlot)
        Me.gbOptions.Controls.Add(Me.chkOutline)
        Me.gbOptions.Controls.Add(Me.chkGenre)
        Me.gbOptions.Controls.Add(Me.chkDirector)
        Me.gbOptions.Controls.Add(Me.chkTagline)
        Me.gbOptions.Controls.Add(Me.chkCast)
        Me.gbOptions.Controls.Add(Me.chkVotes)
        Me.gbOptions.Controls.Add(Me.chkTrailer)
        Me.gbOptions.Controls.Add(Me.chkRating)
        Me.gbOptions.Controls.Add(Me.chkRelease)
        Me.gbOptions.Controls.Add(Me.chkMPAA)
        Me.gbOptions.Controls.Add(Me.chkYear)
        Me.gbOptions.Controls.Add(Me.chkTitle)
        Me.gbOptions.Location = New System.Drawing.Point(4, 4)
        Me.gbOptions.Name = "gbOptions"
        Me.gbOptions.Size = New System.Drawing.Size(294, 288)
        Me.gbOptions.TabIndex = 67
        Me.gbOptions.TabStop = False
        Me.gbOptions.Text = "Scraper Fields"
        '
        'chkTop250
        '
        Me.chkTop250.AutoSize = True
        Me.chkTop250.Location = New System.Drawing.Point(158, 251)
        Me.chkTop250.Name = "chkTop250"
        Me.chkTop250.Size = New System.Drawing.Size(66, 17)
        Me.chkTop250.TabIndex = 23
        Me.chkTop250.Text = "Top 250"
        Me.chkTop250.UseVisualStyleBackColor = True
        '
        'txtGenreLimit
        '
        Me.txtGenreLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGenreLimit.Enabled = False
        Me.txtGenreLimit.Location = New System.Drawing.Point(57, 223)
        Me.txtGenreLimit.Name = "txtGenreLimit"
        Me.txtGenreLimit.Size = New System.Drawing.Size(39, 20)
        Me.txtGenreLimit.TabIndex = 21
        '
        'lblLimit2
        '
        Me.lblLimit2.AutoSize = True
        Me.lblLimit2.Location = New System.Drawing.Point(22, 226)
        Me.lblLimit2.Name = "lblLimit2"
        Me.lblLimit2.Size = New System.Drawing.Size(31, 13)
        Me.lblLimit2.TabIndex = 22
        Me.lblLimit2.Text = "Limit:"
        Me.lblLimit2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtActorLimit
        '
        Me.txtActorLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtActorLimit.Enabled = False
        Me.txtActorLimit.Location = New System.Drawing.Point(208, 108)
        Me.txtActorLimit.Name = "txtActorLimit"
        Me.txtActorLimit.Size = New System.Drawing.Size(39, 20)
        Me.txtActorLimit.TabIndex = 19
        '
        'lblLimit
        '
        Me.lblLimit.AutoSize = True
        Me.lblLimit.Location = New System.Drawing.Point(173, 111)
        Me.lblLimit.Name = "lblLimit"
        Me.lblLimit.Size = New System.Drawing.Size(31, 13)
        Me.lblLimit.TabIndex = 20
        Me.lblLimit.Text = "Limit:"
        Me.lblLimit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkCrew
        '
        Me.chkCrew.AutoSize = True
        Me.chkCrew.Enabled = False
        Me.chkCrew.Location = New System.Drawing.Point(158, 228)
        Me.chkCrew.Name = "chkCrew"
        Me.chkCrew.Size = New System.Drawing.Size(79, 17)
        Me.chkCrew.TabIndex = 18
        Me.chkCrew.Text = "Other Crew"
        Me.chkCrew.UseVisualStyleBackColor = True
        '
        'chkMusicBy
        '
        Me.chkMusicBy.AutoSize = True
        Me.chkMusicBy.Enabled = False
        Me.chkMusicBy.Location = New System.Drawing.Point(158, 205)
        Me.chkMusicBy.Name = "chkMusicBy"
        Me.chkMusicBy.Size = New System.Drawing.Size(69, 17)
        Me.chkMusicBy.TabIndex = 17
        Me.chkMusicBy.Text = "Music By"
        Me.chkMusicBy.UseVisualStyleBackColor = True
        '
        'chkProducers
        '
        Me.chkProducers.AutoSize = True
        Me.chkProducers.Enabled = False
        Me.chkProducers.Location = New System.Drawing.Point(158, 182)
        Me.chkProducers.Name = "chkProducers"
        Me.chkProducers.Size = New System.Drawing.Size(74, 17)
        Me.chkProducers.TabIndex = 16
        Me.chkProducers.Text = "Producers"
        Me.chkProducers.UseVisualStyleBackColor = True
        '
        'chkWriters
        '
        Me.chkWriters.AutoSize = True
        Me.chkWriters.Location = New System.Drawing.Point(158, 159)
        Me.chkWriters.Name = "chkWriters"
        Me.chkWriters.Size = New System.Drawing.Size(59, 17)
        Me.chkWriters.TabIndex = 15
        Me.chkWriters.Text = "Writers"
        Me.chkWriters.UseVisualStyleBackColor = True
        '
        'chkStudio
        '
        Me.chkStudio.AutoSize = True
        Me.chkStudio.Location = New System.Drawing.Point(6, 181)
        Me.chkStudio.Name = "chkStudio"
        Me.chkStudio.Size = New System.Drawing.Size(56, 17)
        Me.chkStudio.TabIndex = 14
        Me.chkStudio.Text = "Studio"
        Me.chkStudio.UseVisualStyleBackColor = True
        '
        'chkRuntime
        '
        Me.chkRuntime.AutoSize = True
        Me.chkRuntime.Location = New System.Drawing.Point(6, 112)
        Me.chkRuntime.Name = "chkRuntime"
        Me.chkRuntime.Size = New System.Drawing.Size(65, 17)
        Me.chkRuntime.TabIndex = 13
        Me.chkRuntime.Text = "Runtime"
        Me.chkRuntime.UseVisualStyleBackColor = True
        '
        'chkPlot
        '
        Me.chkPlot.AutoSize = True
        Me.chkPlot.Location = New System.Drawing.Point(158, 66)
        Me.chkPlot.Name = "chkPlot"
        Me.chkPlot.Size = New System.Drawing.Size(44, 17)
        Me.chkPlot.TabIndex = 12
        Me.chkPlot.Text = "Plot"
        Me.chkPlot.UseVisualStyleBackColor = True
        '
        'chkOutline
        '
        Me.chkOutline.AutoSize = True
        Me.chkOutline.Location = New System.Drawing.Point(158, 43)
        Me.chkOutline.Name = "chkOutline"
        Me.chkOutline.Size = New System.Drawing.Size(59, 17)
        Me.chkOutline.TabIndex = 11
        Me.chkOutline.Text = "Outline"
        Me.chkOutline.UseVisualStyleBackColor = True
        '
        'chkGenre
        '
        Me.chkGenre.AutoSize = True
        Me.chkGenre.Location = New System.Drawing.Point(6, 204)
        Me.chkGenre.Name = "chkGenre"
        Me.chkGenre.Size = New System.Drawing.Size(55, 17)
        Me.chkGenre.TabIndex = 10
        Me.chkGenre.Text = "Genre"
        Me.chkGenre.UseVisualStyleBackColor = True
        '
        'chkDirector
        '
        Me.chkDirector.AutoSize = True
        Me.chkDirector.Location = New System.Drawing.Point(158, 136)
        Me.chkDirector.Name = "chkDirector"
        Me.chkDirector.Size = New System.Drawing.Size(63, 17)
        Me.chkDirector.TabIndex = 9
        Me.chkDirector.Text = "Director"
        Me.chkDirector.UseVisualStyleBackColor = True
        '
        'chkTagline
        '
        Me.chkTagline.AutoSize = True
        Me.chkTagline.Location = New System.Drawing.Point(158, 20)
        Me.chkTagline.Name = "chkTagline"
        Me.chkTagline.Size = New System.Drawing.Size(61, 17)
        Me.chkTagline.TabIndex = 8
        Me.chkTagline.Text = "Tagline"
        Me.chkTagline.UseVisualStyleBackColor = True
        '
        'chkCast
        '
        Me.chkCast.AutoSize = True
        Me.chkCast.Location = New System.Drawing.Point(158, 89)
        Me.chkCast.Name = "chkCast"
        Me.chkCast.Size = New System.Drawing.Size(47, 17)
        Me.chkCast.TabIndex = 7
        Me.chkCast.Text = "Cast"
        Me.chkCast.UseVisualStyleBackColor = True
        '
        'chkVotes
        '
        Me.chkVotes.AutoSize = True
        Me.chkVotes.Location = New System.Drawing.Point(6, 158)
        Me.chkVotes.Name = "chkVotes"
        Me.chkVotes.Size = New System.Drawing.Size(53, 17)
        Me.chkVotes.TabIndex = 6
        Me.chkVotes.Text = "Votes"
        Me.chkVotes.UseVisualStyleBackColor = True
        '
        'chkTrailer
        '
        Me.chkTrailer.AutoSize = True
        Me.chkTrailer.Location = New System.Drawing.Point(6, 249)
        Me.chkTrailer.Name = "chkTrailer"
        Me.chkTrailer.Size = New System.Drawing.Size(55, 17)
        Me.chkTrailer.TabIndex = 5
        Me.chkTrailer.Text = "Trailer"
        Me.chkTrailer.UseVisualStyleBackColor = True
        '
        'chkRating
        '
        Me.chkRating.AutoSize = True
        Me.chkRating.Location = New System.Drawing.Point(6, 135)
        Me.chkRating.Name = "chkRating"
        Me.chkRating.Size = New System.Drawing.Size(57, 17)
        Me.chkRating.TabIndex = 4
        Me.chkRating.Text = "Rating"
        Me.chkRating.UseVisualStyleBackColor = True
        '
        'chkRelease
        '
        Me.chkRelease.AutoSize = True
        Me.chkRelease.Location = New System.Drawing.Point(6, 89)
        Me.chkRelease.Name = "chkRelease"
        Me.chkRelease.Size = New System.Drawing.Size(91, 17)
        Me.chkRelease.TabIndex = 3
        Me.chkRelease.Text = "Release Date"
        Me.chkRelease.UseVisualStyleBackColor = True
        '
        'chkMPAA
        '
        Me.chkMPAA.AutoSize = True
        Me.chkMPAA.Location = New System.Drawing.Point(6, 66)
        Me.chkMPAA.Name = "chkMPAA"
        Me.chkMPAA.Size = New System.Drawing.Size(80, 17)
        Me.chkMPAA.TabIndex = 2
        Me.chkMPAA.Text = "MPAA/Cert"
        Me.chkMPAA.UseVisualStyleBackColor = True
        '
        'chkYear
        '
        Me.chkYear.AutoSize = True
        Me.chkYear.Location = New System.Drawing.Point(6, 43)
        Me.chkYear.Name = "chkYear"
        Me.chkYear.Size = New System.Drawing.Size(48, 17)
        Me.chkYear.TabIndex = 1
        Me.chkYear.Text = "Year"
        Me.chkYear.UseVisualStyleBackColor = True
        '
        'chkTitle
        '
        Me.chkTitle.AutoSize = True
        Me.chkTitle.Location = New System.Drawing.Point(6, 20)
        Me.chkTitle.Name = "chkTitle"
        Me.chkTitle.Size = New System.Drawing.Size(46, 17)
        Me.chkTitle.TabIndex = 0
        Me.chkTitle.Text = "Title"
        Me.chkTitle.UseVisualStyleBackColor = True
        '
        'lblCurrent
        '
        Me.lblCurrent.AutoSize = True
        Me.lblCurrent.BackColor = System.Drawing.Color.SteelBlue
        Me.lblCurrent.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrent.ForeColor = System.Drawing.Color.White
        Me.lblCurrent.Location = New System.Drawing.Point(3, 1)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(84, 24)
        Me.lblCurrent.TabIndex = 63
        Me.lblCurrent.Text = "General"
        '
        'pnlCurrent
        '
        Me.pnlCurrent.BackColor = System.Drawing.Color.SteelBlue
        Me.pnlCurrent.Location = New System.Drawing.Point(429, 69)
        Me.pnlCurrent.Name = "pnlCurrent"
        Me.pnlCurrent.Size = New System.Drawing.Size(376, 25)
        Me.pnlCurrent.TabIndex = 64
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.SteelBlue
        Me.Panel1.Controls.Add(Me.lblCurrent)
        Me.Panel1.Location = New System.Drawing.Point(208, 69)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(221, 25)
        Me.Panel1.TabIndex = 65
        '
        'pnlExtensions
        '
        Me.pnlExtensions.BackColor = System.Drawing.Color.White
        Me.pnlExtensions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlExtensions.Controls.Add(Me.GroupBox28)
        Me.pnlExtensions.Controls.Add(Me.GroupBox22)
        Me.pnlExtensions.Controls.Add(Me.gbRenamerPatterns)
        Me.pnlExtensions.Controls.Add(Me.GroupBox18)
        Me.pnlExtensions.Location = New System.Drawing.Point(208, 96)
        Me.pnlExtensions.Name = "pnlExtensions"
        Me.pnlExtensions.Size = New System.Drawing.Size(597, 353)
        Me.pnlExtensions.TabIndex = 66
        Me.pnlExtensions.Visible = False
        '
        'GroupBox28
        '
        Me.GroupBox28.Controls.Add(Me.txtDefFIExt)
        Me.GroupBox28.Controls.Add(Me.Label34)
        Me.GroupBox28.Controls.Add(Me.btnRemoveMetaDataFT)
        Me.GroupBox28.Controls.Add(Me.btnEditMetaDataFT)
        Me.GroupBox28.Controls.Add(Me.btnNewMetaDataFT)
        Me.GroupBox28.Controls.Add(Me.lstMetadata)
        Me.GroupBox28.Location = New System.Drawing.Point(416, 5)
        Me.GroupBox28.Name = "GroupBox28"
        Me.GroupBox28.Size = New System.Drawing.Size(176, 156)
        Me.GroupBox28.TabIndex = 8
        Me.GroupBox28.TabStop = False
        Me.GroupBox28.Text = "Meta Data Defaults by File Type"
        '
        'txtDefFIExt
        '
        Me.txtDefFIExt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtDefFIExt.Location = New System.Drawing.Point(69, 132)
        Me.txtDefFIExt.Name = "txtDefFIExt"
        Me.txtDefFIExt.Size = New System.Drawing.Size(35, 20)
        Me.txtDefFIExt.TabIndex = 33
        '
        'Label34
        '
        Me.Label34.Location = New System.Drawing.Point(4, 132)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(66, 19)
        Me.Label34.TabIndex = 32
        Me.Label34.Text = "File Type"
        Me.Label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnRemoveMetaDataFT
        '
        Me.btnRemoveMetaDataFT.Enabled = False
        Me.btnRemoveMetaDataFT.Image = CType(resources.GetObject("btnRemoveMetaDataFT.Image"), System.Drawing.Image)
        Me.btnRemoveMetaDataFT.Location = New System.Drawing.Point(149, 131)
        Me.btnRemoveMetaDataFT.Name = "btnRemoveMetaDataFT"
        Me.btnRemoveMetaDataFT.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveMetaDataFT.TabIndex = 31
        Me.btnRemoveMetaDataFT.UseVisualStyleBackColor = True
        '
        'btnEditMetaDataFT
        '
        Me.btnEditMetaDataFT.Enabled = False
        Me.btnEditMetaDataFT.Image = CType(resources.GetObject("btnEditMetaDataFT.Image"), System.Drawing.Image)
        Me.btnEditMetaDataFT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditMetaDataFT.Location = New System.Drawing.Point(126, 131)
        Me.btnEditMetaDataFT.Name = "btnEditMetaDataFT"
        Me.btnEditMetaDataFT.Size = New System.Drawing.Size(23, 23)
        Me.btnEditMetaDataFT.TabIndex = 30
        Me.btnEditMetaDataFT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEditMetaDataFT.UseVisualStyleBackColor = True
        '
        'btnNewMetaDataFT
        '
        Me.btnNewMetaDataFT.Enabled = False
        Me.btnNewMetaDataFT.Image = CType(resources.GetObject("btnNewMetaDataFT.Image"), System.Drawing.Image)
        Me.btnNewMetaDataFT.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewMetaDataFT.Location = New System.Drawing.Point(104, 131)
        Me.btnNewMetaDataFT.Name = "btnNewMetaDataFT"
        Me.btnNewMetaDataFT.Size = New System.Drawing.Size(23, 23)
        Me.btnNewMetaDataFT.TabIndex = 29
        Me.btnNewMetaDataFT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNewMetaDataFT.UseVisualStyleBackColor = True
        '
        'lstMetadata
        '
        Me.lstMetadata.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.lstMetadata.FullRowSelect = True
        Me.lstMetadata.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstMetadata.Location = New System.Drawing.Point(4, 18)
        Me.lstMetadata.MultiSelect = False
        Me.lstMetadata.Name = "lstMetadata"
        Me.lstMetadata.Size = New System.Drawing.Size(166, 107)
        Me.lstMetadata.TabIndex = 24
        Me.lstMetadata.UseCompatibleStateImageBehavior = False
        Me.lstMetadata.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Type"
        Me.ColumnHeader1.Width = 142
        '
        'GroupBox22
        '
        Me.GroupBox22.Controls.Add(Me.btnRemoveNoStack)
        Me.GroupBox22.Controls.Add(Me.btnAddNoStack)
        Me.GroupBox22.Controls.Add(Me.txtNoStack)
        Me.GroupBox22.Controls.Add(Me.lstNoStack)
        Me.GroupBox22.Location = New System.Drawing.Point(210, 192)
        Me.GroupBox22.Name = "GroupBox22"
        Me.GroupBox22.Size = New System.Drawing.Size(204, 156)
        Me.GroupBox22.TabIndex = 2
        Me.GroupBox22.TabStop = False
        Me.GroupBox22.Text = "No Stack Extensions"
        '
        'btnRemoveNoStack
        '
        Me.btnRemoveNoStack.Image = CType(resources.GetObject("btnRemoveNoStack.Image"), System.Drawing.Image)
        Me.btnRemoveNoStack.Location = New System.Drawing.Point(168, 127)
        Me.btnRemoveNoStack.Name = "btnRemoveNoStack"
        Me.btnRemoveNoStack.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveNoStack.TabIndex = 3
        Me.btnRemoveNoStack.UseVisualStyleBackColor = True
        '
        'btnAddNoStack
        '
        Me.btnAddNoStack.Image = CType(resources.GetObject("btnAddNoStack.Image"), System.Drawing.Image)
        Me.btnAddNoStack.Location = New System.Drawing.Point(73, 127)
        Me.btnAddNoStack.Name = "btnAddNoStack"
        Me.btnAddNoStack.Size = New System.Drawing.Size(23, 23)
        Me.btnAddNoStack.TabIndex = 2
        Me.btnAddNoStack.UseVisualStyleBackColor = True
        '
        'txtNoStack
        '
        Me.txtNoStack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtNoStack.Location = New System.Drawing.Point(11, 128)
        Me.txtNoStack.Name = "txtNoStack"
        Me.txtNoStack.Size = New System.Drawing.Size(61, 20)
        Me.txtNoStack.TabIndex = 1
        '
        'lstNoStack
        '
        Me.lstNoStack.FormattingEnabled = True
        Me.lstNoStack.Location = New System.Drawing.Point(11, 15)
        Me.lstNoStack.Name = "lstNoStack"
        Me.lstNoStack.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstNoStack.Size = New System.Drawing.Size(180, 108)
        Me.lstNoStack.Sorted = True
        Me.lstNoStack.TabIndex = 0
        '
        'gbRenamerPatterns
        '
        Me.gbRenamerPatterns.Controls.Add(Me.chkRenameSingle)
        Me.gbRenamerPatterns.Controls.Add(Me.chkRenameMulti)
        Me.gbRenamerPatterns.Controls.Add(Me.lblFilePattern)
        Me.gbRenamerPatterns.Controls.Add(Me.lblFolderPattern)
        Me.gbRenamerPatterns.Controls.Add(Me.txtFilePattern)
        Me.gbRenamerPatterns.Controls.Add(Me.txtFolderPattern)
        Me.gbRenamerPatterns.Location = New System.Drawing.Point(209, 6)
        Me.gbRenamerPatterns.Name = "gbRenamerPatterns"
        Me.gbRenamerPatterns.Size = New System.Drawing.Size(204, 176)
        Me.gbRenamerPatterns.TabIndex = 1
        Me.gbRenamerPatterns.TabStop = False
        Me.gbRenamerPatterns.Text = "Default Renaming Patterns"
        '
        'chkRenameSingle
        '
        Me.chkRenameSingle.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkRenameSingle.Location = New System.Drawing.Point(8, 135)
        Me.chkRenameSingle.Name = "chkRenameSingle"
        Me.chkRenameSingle.Size = New System.Drawing.Size(189, 30)
        Me.chkRenameSingle.TabIndex = 74
        Me.chkRenameSingle.Text = "Automatically Rename Files During Single-Scraper"
        Me.chkRenameSingle.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkRenameSingle.UseVisualStyleBackColor = True
        '
        'chkRenameMulti
        '
        Me.chkRenameMulti.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkRenameMulti.Location = New System.Drawing.Point(8, 97)
        Me.chkRenameMulti.Name = "chkRenameMulti"
        Me.chkRenameMulti.Size = New System.Drawing.Size(189, 30)
        Me.chkRenameMulti.TabIndex = 73
        Me.chkRenameMulti.Text = "Automatically Rename Files During Multi-Scraper"
        Me.chkRenameMulti.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkRenameMulti.UseVisualStyleBackColor = True
        '
        'lblFilePattern
        '
        Me.lblFilePattern.AutoSize = True
        Me.lblFilePattern.Location = New System.Drawing.Point(11, 55)
        Me.lblFilePattern.Name = "lblFilePattern"
        Me.lblFilePattern.Size = New System.Drawing.Size(65, 13)
        Me.lblFilePattern.TabIndex = 3
        Me.lblFilePattern.Text = "Files Pattern"
        '
        'lblFolderPattern
        '
        Me.lblFolderPattern.AutoSize = True
        Me.lblFolderPattern.Location = New System.Drawing.Point(11, 16)
        Me.lblFolderPattern.Name = "lblFolderPattern"
        Me.lblFolderPattern.Size = New System.Drawing.Size(78, 13)
        Me.lblFolderPattern.TabIndex = 2
        Me.lblFolderPattern.Text = "Folders Pattern"
        '
        'txtFilePattern
        '
        Me.txtFilePattern.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilePattern.Location = New System.Drawing.Point(8, 71)
        Me.txtFilePattern.Name = "txtFilePattern"
        Me.txtFilePattern.Size = New System.Drawing.Size(186, 20)
        Me.txtFilePattern.TabIndex = 1
        '
        'txtFolderPattern
        '
        Me.txtFolderPattern.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFolderPattern.Location = New System.Drawing.Point(8, 32)
        Me.txtFolderPattern.Name = "txtFolderPattern"
        Me.txtFolderPattern.Size = New System.Drawing.Size(186, 20)
        Me.txtFolderPattern.TabIndex = 0
        '
        'GroupBox18
        '
        Me.GroupBox18.Controls.Add(Me.btnRemMovieExt)
        Me.GroupBox18.Controls.Add(Me.btnAddMovieExt)
        Me.GroupBox18.Controls.Add(Me.txtMovieExt)
        Me.GroupBox18.Controls.Add(Me.lstMovieExts)
        Me.GroupBox18.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox18.Name = "GroupBox18"
        Me.GroupBox18.Size = New System.Drawing.Size(192, 342)
        Me.GroupBox18.TabIndex = 0
        Me.GroupBox18.TabStop = False
        Me.GroupBox18.Text = "Valid Movie Extensions"
        '
        'btnRemMovieExt
        '
        Me.btnRemMovieExt.Image = CType(resources.GetObject("btnRemMovieExt.Image"), System.Drawing.Image)
        Me.btnRemMovieExt.Location = New System.Drawing.Point(163, 313)
        Me.btnRemMovieExt.Name = "btnRemMovieExt"
        Me.btnRemMovieExt.Size = New System.Drawing.Size(23, 23)
        Me.btnRemMovieExt.TabIndex = 3
        Me.btnRemMovieExt.UseVisualStyleBackColor = True
        '
        'btnAddMovieExt
        '
        Me.btnAddMovieExt.Image = CType(resources.GetObject("btnAddMovieExt.Image"), System.Drawing.Image)
        Me.btnAddMovieExt.Location = New System.Drawing.Point(68, 313)
        Me.btnAddMovieExt.Name = "btnAddMovieExt"
        Me.btnAddMovieExt.Size = New System.Drawing.Size(23, 23)
        Me.btnAddMovieExt.TabIndex = 2
        Me.btnAddMovieExt.UseVisualStyleBackColor = True
        '
        'txtMovieExt
        '
        Me.txtMovieExt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMovieExt.Location = New System.Drawing.Point(6, 314)
        Me.txtMovieExt.Name = "txtMovieExt"
        Me.txtMovieExt.Size = New System.Drawing.Size(61, 20)
        Me.txtMovieExt.TabIndex = 1
        '
        'lstMovieExts
        '
        Me.lstMovieExts.FormattingEnabled = True
        Me.lstMovieExts.Location = New System.Drawing.Point(6, 16)
        Me.lstMovieExts.Name = "lstMovieExts"
        Me.lstMovieExts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstMovieExts.Size = New System.Drawing.Size(180, 290)
        Me.lstMovieExts.Sorted = True
        Me.lstMovieExts.TabIndex = 0
        '
        'pnlSources
        '
        Me.pnlSources.BackColor = System.Drawing.Color.White
        Me.pnlSources.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSources.Controls.Add(Me.btnEditSource)
        Me.pnlSources.Controls.Add(Me.GroupBox19)
        Me.pnlSources.Controls.Add(Me.GroupBox16)
        Me.pnlSources.Controls.Add(Me.lvMovies)
        Me.pnlSources.Controls.Add(Me.btnMovieRem)
        Me.pnlSources.Controls.Add(Me.btnMovieAddFolder)
        Me.pnlSources.Controls.Add(Me.GroupBox8)
        Me.pnlSources.Location = New System.Drawing.Point(208, 96)
        Me.pnlSources.Name = "pnlSources"
        Me.pnlSources.Size = New System.Drawing.Size(597, 353)
        Me.pnlSources.TabIndex = 67
        Me.pnlSources.Visible = False
        '
        'btnEditSource
        '
        Me.btnEditSource.Image = CType(resources.GetObject("btnEditSource.Image"), System.Drawing.Image)
        Me.btnEditSource.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditSource.Location = New System.Drawing.Point(485, 35)
        Me.btnEditSource.Name = "btnEditSource"
        Me.btnEditSource.Size = New System.Drawing.Size(104, 23)
        Me.btnEditSource.TabIndex = 2
        Me.btnEditSource.Text = "Edit Source"
        Me.btnEditSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEditSource.UseVisualStyleBackColor = True
        '
        'GroupBox19
        '
        Me.GroupBox19.Controls.Add(Me.chkAutoDetectVTS)
        Me.GroupBox19.Controls.Add(Me.chkSkipStackedSizeCheck)
        Me.GroupBox19.Controls.Add(Me.Label21)
        Me.GroupBox19.Controls.Add(Me.txtSkipLessThan)
        Me.GroupBox19.Controls.Add(Me.Label20)
        Me.GroupBox19.Location = New System.Drawing.Point(5, 113)
        Me.GroupBox19.Name = "GroupBox19"
        Me.GroupBox19.Size = New System.Drawing.Size(211, 191)
        Me.GroupBox19.TabIndex = 4
        Me.GroupBox19.TabStop = False
        Me.GroupBox19.Text = "Miscellaneous Options"
        '
        'chkAutoDetectVTS
        '
        Me.chkAutoDetectVTS.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoDetectVTS.Location = New System.Drawing.Point(8, 87)
        Me.chkAutoDetectVTS.Name = "chkAutoDetectVTS"
        Me.chkAutoDetectVTS.Size = New System.Drawing.Size(199, 47)
        Me.chkAutoDetectVTS.TabIndex = 70
        Me.chkAutoDetectVTS.Text = "Automatically Detect VIDEO_TS Folders Even if They Are Not Named ""VIDEO_TS"""
        Me.chkAutoDetectVTS.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoDetectVTS.UseVisualStyleBackColor = True
        '
        'chkSkipStackedSizeCheck
        '
        Me.chkSkipStackedSizeCheck.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSkipStackedSizeCheck.Location = New System.Drawing.Point(27, 54)
        Me.chkSkipStackedSizeCheck.Name = "chkSkipStackedSizeCheck"
        Me.chkSkipStackedSizeCheck.Size = New System.Drawing.Size(183, 30)
        Me.chkSkipStackedSizeCheck.TabIndex = 1
        Me.chkSkipStackedSizeCheck.Text = "Skip Size Check of Stacked Files"
        Me.chkSkipStackedSizeCheck.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSkipStackedSizeCheck.UseVisualStyleBackColor = True
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(133, 36)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(59, 13)
        Me.Label21.TabIndex = 69
        Me.Label21.Text = "Megabytes"
        '
        'txtSkipLessThan
        '
        Me.txtSkipLessThan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSkipLessThan.Location = New System.Drawing.Point(27, 32)
        Me.txtSkipLessThan.Name = "txtSkipLessThan"
        Me.txtSkipLessThan.Size = New System.Drawing.Size(100, 20)
        Me.txtSkipLessThan.TabIndex = 0
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(4, 16)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(97, 13)
        Me.Label20.TabIndex = 67
        Me.Label20.Text = "Skip files less than:"
        '
        'pnlImages
        '
        Me.pnlImages.BackColor = System.Drawing.Color.White
        Me.pnlImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlImages.Controls.Add(Me.GroupBox23)
        Me.pnlImages.Controls.Add(Me.GroupBox17)
        Me.pnlImages.Controls.Add(Me.GroupBox9)
        Me.pnlImages.Controls.Add(Me.GroupBox13)
        Me.pnlImages.Controls.Add(Me.GroupBox14)
        Me.pnlImages.Location = New System.Drawing.Point(208, 96)
        Me.pnlImages.Name = "pnlImages"
        Me.pnlImages.Size = New System.Drawing.Size(597, 353)
        Me.pnlImages.TabIndex = 68
        Me.pnlImages.Visible = False
        '
        'GroupBox23
        '
        Me.GroupBox23.Controls.Add(Me.chkAutoETSize)
        Me.GroupBox23.Controls.Add(Me.cbAutoETSize)
        Me.GroupBox23.Controls.Add(Me.txtAutoThumbs)
        Me.GroupBox23.Controls.Add(Me.GroupBox24)
        Me.GroupBox23.Controls.Add(Me.chkAutoThumbs)
        Me.GroupBox23.Controls.Add(Me.chkUseETasFA)
        Me.GroupBox23.Controls.Add(Me.Label17)
        Me.GroupBox23.Controls.Add(Me.Label15)
        Me.GroupBox23.Controls.Add(Me.chkNoSpoilers)
        Me.GroupBox23.Location = New System.Drawing.Point(393, 3)
        Me.GroupBox23.Name = "GroupBox23"
        Me.GroupBox23.Size = New System.Drawing.Size(199, 342)
        Me.GroupBox23.TabIndex = 4
        Me.GroupBox23.TabStop = False
        Me.GroupBox23.Text = "Extrathumbs"
        '
        'chkAutoETSize
        '
        Me.chkAutoETSize.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoETSize.Location = New System.Drawing.Point(6, 235)
        Me.chkAutoETSize.Name = "chkAutoETSize"
        Me.chkAutoETSize.Size = New System.Drawing.Size(188, 43)
        Me.chkAutoETSize.TabIndex = 67
        Me.chkAutoETSize.Text = "Download All Fanart Images of the Following Size as Extrathumbs"
        Me.chkAutoETSize.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoETSize.UseVisualStyleBackColor = True
        '
        'cbAutoETSize
        '
        Me.cbAutoETSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAutoETSize.Enabled = False
        Me.cbAutoETSize.FormattingEnabled = True
        Me.cbAutoETSize.Location = New System.Drawing.Point(27, 278)
        Me.cbAutoETSize.Name = "cbAutoETSize"
        Me.cbAutoETSize.Size = New System.Drawing.Size(148, 21)
        Me.cbAutoETSize.TabIndex = 65
        '
        'GroupBox24
        '
        Me.GroupBox24.Controls.Add(Me.txtETWidth)
        Me.GroupBox24.Controls.Add(Me.txtETHeight)
        Me.GroupBox24.Controls.Add(Me.chkETPadding)
        Me.GroupBox24.Controls.Add(Me.Label28)
        Me.GroupBox24.Controls.Add(Me.Label29)
        Me.GroupBox24.Controls.Add(Me.rbETCustom)
        Me.GroupBox24.Controls.Add(Me.rbETNative)
        Me.GroupBox24.Location = New System.Drawing.Point(7, 133)
        Me.GroupBox24.Name = "GroupBox24"
        Me.GroupBox24.Size = New System.Drawing.Size(185, 93)
        Me.GroupBox24.TabIndex = 64
        Me.GroupBox24.TabStop = False
        Me.GroupBox24.Text = "Sizing (Extracted Frames)"
        '
        'txtETWidth
        '
        Me.txtETWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtETWidth.Enabled = False
        Me.txtETWidth.Location = New System.Drawing.Point(57, 49)
        Me.txtETWidth.Name = "txtETWidth"
        Me.txtETWidth.Size = New System.Drawing.Size(38, 20)
        Me.txtETWidth.TabIndex = 44
        '
        'txtETHeight
        '
        Me.txtETHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtETHeight.Enabled = False
        Me.txtETHeight.Location = New System.Drawing.Point(143, 49)
        Me.txtETHeight.Name = "txtETHeight"
        Me.txtETHeight.Size = New System.Drawing.Size(38, 20)
        Me.txtETHeight.TabIndex = 45
        '
        'chkETPadding
        '
        Me.chkETPadding.AutoSize = True
        Me.chkETPadding.Enabled = False
        Me.chkETPadding.Location = New System.Drawing.Point(24, 72)
        Me.chkETPadding.Name = "chkETPadding"
        Me.chkETPadding.Size = New System.Drawing.Size(65, 17)
        Me.chkETPadding.TabIndex = 48
        Me.chkETPadding.Text = "Padding"
        Me.chkETPadding.UseVisualStyleBackColor = True
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Location = New System.Drawing.Point(19, 53)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(38, 13)
        Me.Label28.TabIndex = 47
        Me.Label28.Text = "Width:"
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Location = New System.Drawing.Point(101, 53)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(41, 13)
        Me.Label29.TabIndex = 46
        Me.Label29.Text = "Height:"
        '
        'rbETCustom
        '
        Me.rbETCustom.AutoSize = True
        Me.rbETCustom.Location = New System.Drawing.Point(7, 32)
        Me.rbETCustom.Name = "rbETCustom"
        Me.rbETCustom.Size = New System.Drawing.Size(105, 17)
        Me.rbETCustom.TabIndex = 1
        Me.rbETCustom.TabStop = True
        Me.rbETCustom.Text = "Use Custom Size"
        Me.rbETCustom.UseVisualStyleBackColor = True
        '
        'rbETNative
        '
        Me.rbETNative.AutoSize = True
        Me.rbETNative.Location = New System.Drawing.Point(7, 14)
        Me.rbETNative.Name = "rbETNative"
        Me.rbETNative.Size = New System.Drawing.Size(131, 17)
        Me.rbETNative.TabIndex = 0
        Me.rbETNative.TabStop = True
        Me.rbETNative.Text = "Use Native Resolution"
        Me.rbETNative.UseVisualStyleBackColor = True
        '
        'GroupBox17
        '
        Me.GroupBox17.Controls.Add(Me.chkUseImgCacheUpdaters)
        Me.GroupBox17.Controls.Add(Me.Label19)
        Me.GroupBox17.Controls.Add(Me.chkPersistImgCache)
        Me.GroupBox17.Controls.Add(Me.chkUseImgCache)
        Me.GroupBox17.Location = New System.Drawing.Point(3, 189)
        Me.GroupBox17.Name = "GroupBox17"
        Me.GroupBox17.Size = New System.Drawing.Size(166, 156)
        Me.GroupBox17.TabIndex = 1
        Me.GroupBox17.TabStop = False
        Me.GroupBox17.Text = "Caching"
        '
        'chkUseImgCacheUpdaters
        '
        Me.chkUseImgCacheUpdaters.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseImgCacheUpdaters.Enabled = False
        Me.chkUseImgCacheUpdaters.Location = New System.Drawing.Point(20, 35)
        Me.chkUseImgCacheUpdaters.Name = "chkUseImgCacheUpdaters"
        Me.chkUseImgCacheUpdaters.Size = New System.Drawing.Size(142, 31)
        Me.chkUseImgCacheUpdaters.TabIndex = 1
        Me.chkUseImgCacheUpdaters.Text = "Use Image Cache for Scrapers"
        Me.chkUseImgCacheUpdaters.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseImgCacheUpdaters.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(32, 86)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(129, 48)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "(When enabled, the cache will be available between sessions)"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'chkPersistImgCache
        '
        Me.chkPersistImgCache.AutoSize = True
        Me.chkPersistImgCache.Enabled = False
        Me.chkPersistImgCache.Location = New System.Drawing.Point(20, 66)
        Me.chkPersistImgCache.Name = "chkPersistImgCache"
        Me.chkPersistImgCache.Size = New System.Drawing.Size(138, 17)
        Me.chkPersistImgCache.TabIndex = 2
        Me.chkPersistImgCache.Text = "Persistent Image Cache"
        Me.chkPersistImgCache.UseVisualStyleBackColor = True
        '
        'chkUseImgCache
        '
        Me.chkUseImgCache.AutoSize = True
        Me.chkUseImgCache.Location = New System.Drawing.Point(7, 16)
        Me.chkUseImgCache.Name = "chkUseImgCache"
        Me.chkUseImgCache.Size = New System.Drawing.Size(111, 17)
        Me.chkUseImgCache.TabIndex = 0
        Me.chkUseImgCache.Text = "Use Image Cache"
        Me.chkUseImgCache.UseVisualStyleBackColor = True
        '
        'fbdBrowse
        '
        Me.fbdBrowse.Description = "Select the folder where you wish to store your backdrops."
        '
        'dlgSettings
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(810, 484)
        Me.Controls.Add(Me.pnlExtensions)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlCurrent)
        Me.Controls.Add(Me.tvSettings)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.pnlScraper)
        Me.Controls.Add(Me.pnlSources)
        Me.Controls.Add(Me.pnlMovies)
        Me.Controls.Add(Me.pnlXBMCCom)
        Me.Controls.Add(Me.pnlGeneral)
        Me.Controls.Add(Me.pnlImages)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings"
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox29.ResumeLayout(False)
        Me.GroupBox29.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.tcCleaner.ResumeLayout(False)
        Me.tpStandard.ResumeLayout(False)
        Me.tpStandard.PerformLayout()
        Me.tpExpert.ResumeLayout(False)
        Me.tpExpert.PerformLayout()
        Me.gbFilters.ResumeLayout(False)
        Me.gbFilters.PerformLayout()
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox25.ResumeLayout(False)
        Me.GroupBox25.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox21.ResumeLayout(False)
        Me.GroupBox21.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox15.ResumeLayout(False)
        Me.GroupBox14.ResumeLayout(False)
        Me.GroupBox14.PerformLayout()
        CType(Me.tbPosterQual, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox13.ResumeLayout(False)
        Me.GroupBox13.PerformLayout()
        CType(Me.tbFanartQual, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.gbRTFormat.ResumeLayout(False)
        Me.gbRTFormat.PerformLayout()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGeneral.ResumeLayout(False)
        Me.pnlXBMCCom.ResumeLayout(False)
        Me.pnlMovies.ResumeLayout(False)
        Me.GroupBox27.ResumeLayout(False)
        Me.GroupBox27.PerformLayout()
        Me.GroupBox16.ResumeLayout(False)
        Me.GroupBox16.PerformLayout()
        Me.pnlScraper.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox26.ResumeLayout(False)
        Me.GroupBox26.PerformLayout()
        Me.GroupBox20.ResumeLayout(False)
        Me.GroupBox20.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.gbOptions.ResumeLayout(False)
        Me.gbOptions.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlExtensions.ResumeLayout(False)
        Me.GroupBox28.ResumeLayout(False)
        Me.GroupBox28.PerformLayout()
        Me.GroupBox22.ResumeLayout(False)
        Me.GroupBox22.PerformLayout()
        Me.gbRenamerPatterns.ResumeLayout(False)
        Me.gbRenamerPatterns.PerformLayout()
        Me.GroupBox18.ResumeLayout(False)
        Me.GroupBox18.PerformLayout()
        Me.pnlSources.ResumeLayout(False)
        Me.GroupBox19.ResumeLayout(False)
        Me.GroupBox19.PerformLayout()
        Me.pnlImages.ResumeLayout(False)
        Me.GroupBox23.ResumeLayout(False)
        Me.GroupBox23.PerformLayout()
        Me.GroupBox24.ResumeLayout(False)
        Me.GroupBox24.PerformLayout()
        Me.GroupBox17.ResumeLayout(False)
        Me.GroupBox17.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents gbFilters As System.Windows.Forms.GroupBox
    Friend WithEvents btnAddFilter As System.Windows.Forms.Button
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents lstFilters As System.Windows.Forms.ListBox
    Friend WithEvents btnRemoveFilter As System.Windows.Forms.Button
    Friend WithEvents cdColor As System.Windows.Forms.ColorDialog
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents chkCleanFolderJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanMovieTBNb As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanMovieTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanMovieNFOb As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanMovieNFO As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanMovieFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLogErrors As System.Windows.Forms.CheckBox
    Friend WithEvents chkProperCase As System.Windows.Forms.CheckBox
    Friend WithEvents btnDown As System.Windows.Forms.Button
    Friend WithEvents btnUp As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents chkOverwriteNfo As System.Windows.Forms.CheckBox
    Friend WithEvents lvMovies As System.Windows.Forms.ListView
    Friend WithEvents colPath As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecur As System.Windows.Forms.ColumnHeader
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents chkUseMPDB As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverwriteFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverwritePoster As System.Windows.Forms.CheckBox
    Friend WithEvents cbFanartSize As System.Windows.Forms.ComboBox
    Friend WithEvents lblFanartSize As System.Windows.Forms.Label
    Friend WithEvents lblPosterSize As System.Windows.Forms.Label
    Friend WithEvents cbPosterSize As System.Windows.Forms.ComboBox
    Friend WithEvents chkUseIMPA As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseTMDB As System.Windows.Forms.CheckBox
    Friend WithEvents chkFullCast As System.Windows.Forms.CheckBox
    Friend WithEvents chkFullCrew As System.Windows.Forms.CheckBox
    Friend WithEvents cbCert As System.Windows.Forms.ComboBox
    Friend WithEvents chkCert As System.Windows.Forms.CheckBox
    Friend WithEvents chkScanMediaInfo As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMovieTrailerCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieInfoCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieFanartCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMoviePosterCol As System.Windows.Forms.CheckBox
    Friend WithEvents btnMovieRem As System.Windows.Forms.Button
    Friend WithEvents btnMovieAddFolder As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents chkFolderJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkPosterTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameNFO As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNFO As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMovieNameDotFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLockTitle As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockOutline As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockPlot As System.Windows.Forms.CheckBox
    Friend WithEvents chkSingleScrapeImages As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanMovieNameJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanMovieJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanPosterTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkCleanDotFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox11 As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox12 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMarkNew As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents GroupBox13 As System.Windows.Forms.GroupBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents chkResizeFanart As System.Windows.Forms.CheckBox
    Friend WithEvents txtFanartWidth As System.Windows.Forms.TextBox
    Friend WithEvents txtFanartHeight As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox14 As System.Windows.Forms.GroupBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents chkResizePoster As System.Windows.Forms.CheckBox
    Friend WithEvents txtPosterWidth As System.Windows.Forms.TextBox
    Friend WithEvents txtPosterHeight As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtPassword As System.Windows.Forms.TextBox
    Friend WithEvents txtUsername As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox15 As System.Windows.Forms.GroupBox
    Friend WithEvents chkOFDBPlot As System.Windows.Forms.CheckBox
    Friend WithEvents chkOFDBOutline As System.Windows.Forms.CheckBox
    Friend WithEvents chkOFDBTitle As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseCertForMPAA As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoThumbs As System.Windows.Forms.CheckBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtAutoThumbs As System.Windows.Forms.TextBox
    Friend WithEvents chkCastWithImg As System.Windows.Forms.CheckBox
    Friend WithEvents chkVideoTSParent As System.Windows.Forms.CheckBox
    Friend WithEvents ilSettings As System.Windows.Forms.ImageList
    Friend WithEvents tvSettings As System.Windows.Forms.TreeView
    Friend WithEvents pnlGeneral As System.Windows.Forms.Panel
    Friend WithEvents pnlXBMCCom As System.Windows.Forms.Panel
    Friend WithEvents pnlMovies As System.Windows.Forms.Panel
    Friend WithEvents pnlScraper As System.Windows.Forms.Panel
    Friend WithEvents lblCurrent As System.Windows.Forms.Label
    Friend WithEvents pnlCurrent As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents btnAddCom As System.Windows.Forms.Button
    Friend WithEvents btnRemoveCom As System.Windows.Forms.Button
    Friend WithEvents lbXBMCCom As System.Windows.Forms.ListBox
    Friend WithEvents btnEditCom As System.Windows.Forms.Button
    Friend WithEvents chkOFDBGenre As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoSpoilers As System.Windows.Forms.CheckBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtIMDBURL As System.Windows.Forms.TextBox
    Friend WithEvents chkCleanExtrathumbs As System.Windows.Forms.CheckBox
    Friend WithEvents pnlExtensions As System.Windows.Forms.Panel
    Friend WithEvents GroupBox18 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemMovieExt As System.Windows.Forms.Button
    Friend WithEvents btnAddMovieExt As System.Windows.Forms.Button
    Friend WithEvents txtMovieExt As System.Windows.Forms.TextBox
    Friend WithEvents lstMovieExts As System.Windows.Forms.ListBox
    Friend WithEvents chkUpdates As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockTagline As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockRating As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockRealStudio As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockGenre As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox16 As System.Windows.Forms.GroupBox
    Friend WithEvents txtBDPath As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents chkAutoBD As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseMIDuration As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieExtraCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieSubCol As System.Windows.Forms.CheckBox
    Friend WithEvents pnlSources As System.Windows.Forms.Panel
    Friend WithEvents lbGenre As System.Windows.Forms.CheckedListBox
    Friend WithEvents lblGenre As System.Windows.Forms.Label
    Friend WithEvents chkUseETasFA As System.Windows.Forms.CheckBox
    Friend WithEvents pnlImages As System.Windows.Forms.Panel
    Friend WithEvents GroupBox17 As System.Windows.Forms.GroupBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents chkPersistImgCache As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseImgCache As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseImgCacheUpdaters As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox19 As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtSkipLessThan As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents chkSkipStackedSizeCheck As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox20 As System.Windows.Forms.GroupBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents lbTrailerSites As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkDownloadTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents chkUpdaterTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtTimeout As System.Windows.Forms.TextBox
    Friend WithEvents chkSingleScrapeTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents lblPosterQual As System.Windows.Forms.Label
    Friend WithEvents tbPosterQual As System.Windows.Forms.TrackBar
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents lblFanartQual As System.Windows.Forms.Label
    Friend WithEvents tbFanartQual As System.Windows.Forms.TrackBar
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents chkFanartOnly As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoDLTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoSaveImagesToNfo As System.Windows.Forms.CheckBox
    Friend WithEvents tcCleaner As System.Windows.Forms.TabControl
    Friend WithEvents tpStandard As System.Windows.Forms.TabPage
    Friend WithEvents tpExpert As System.Windows.Forms.TabPage
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents btnRemoveWhitelist As System.Windows.Forms.Button
    Friend WithEvents btnAddWhitelist As System.Windows.Forms.Button
    Friend WithEvents txtWhitelist As System.Windows.Forms.TextBox
    Friend WithEvents lstWhitelist As System.Windows.Forms.ListBox
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents chkWhitelistVideo As System.Windows.Forms.CheckBox
    Friend WithEvents chkInfoPanelAnim As System.Windows.Forms.CheckBox
    Friend WithEvents chkOverwriteTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowDims As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoDisplayFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkNoDisplayPoster As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox21 As System.Windows.Forms.GroupBox
    Friend WithEvents rbBracketTrailer As System.Windows.Forms.RadioButton
    Friend WithEvents rbDashTrailer As System.Windows.Forms.RadioButton
    Friend WithEvents chkOutlineForPlot As System.Windows.Forms.CheckBox
    Friend WithEvents chkDeleteAllTrailers As System.Windows.Forms.CheckBox
    Friend WithEvents gbRTFormat As System.Windows.Forms.GroupBox
    Friend WithEvents rbHM As System.Windows.Forms.RadioButton
    Friend WithEvents rbMins As System.Windows.Forms.RadioButton
    Friend WithEvents gbRenamerPatterns As System.Windows.Forms.GroupBox
    Friend WithEvents lblFilePattern As System.Windows.Forms.Label
    Friend WithEvents lblFolderPattern As System.Windows.Forms.Label
    Friend WithEvents txtFilePattern As System.Windows.Forms.TextBox
    Friend WithEvents txtFolderPattern As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox22 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemoveNoStack As System.Windows.Forms.Button
    Friend WithEvents btnAddNoStack As System.Windows.Forms.Button
    Friend WithEvents txtNoStack As System.Windows.Forms.TextBox
    Friend WithEvents lstNoStack As System.Windows.Forms.ListBox
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colFolder As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSingle As System.Windows.Forms.ColumnHeader
    Friend WithEvents colID As System.Windows.Forms.ColumnHeader
    Friend WithEvents fbdBrowse As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnEditSource As System.Windows.Forms.Button
    Friend WithEvents chkShowGenresText As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisplayYear As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox23 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox24 As System.Windows.Forms.GroupBox
    Friend WithEvents chkETPadding As System.Windows.Forms.CheckBox
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents txtETWidth As System.Windows.Forms.TextBox
    Friend WithEvents txtETHeight As System.Windows.Forms.TextBox
    Friend WithEvents rbETCustom As System.Windows.Forms.RadioButton
    Friend WithEvents rbETNative As System.Windows.Forms.RadioButton
    Friend WithEvents chkMovieNameMultiOnly As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox25 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemoveToken As System.Windows.Forms.Button
    Friend WithEvents btnAddToken As System.Windows.Forms.Button
    Friend WithEvents txtSortToken As System.Windows.Forms.TextBox
    Friend WithEvents lstSortTokens As System.Windows.Forms.ListBox
    Friend WithEvents txtCheckTitleTol As System.Windows.Forms.TextBox
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents chkCheckTitles As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoDetectVTS As System.Windows.Forms.CheckBox
    Friend WithEvents cbLanguages As System.Windows.Forms.ComboBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents GroupBox26 As System.Windows.Forms.GroupBox
    Friend WithEvents Label32 As System.Windows.Forms.Label
    Friend WithEvents cbIntLang As System.Windows.Forms.ComboBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents gbOptions As System.Windows.Forms.GroupBox
    Friend WithEvents chkCrew As System.Windows.Forms.CheckBox
    Friend WithEvents chkMusicBy As System.Windows.Forms.CheckBox
    Friend WithEvents chkProducers As System.Windows.Forms.CheckBox
    Friend WithEvents chkWriters As System.Windows.Forms.CheckBox
    Friend WithEvents chkStudio As System.Windows.Forms.CheckBox
    Friend WithEvents chkRuntime As System.Windows.Forms.CheckBox
    Friend WithEvents chkPlot As System.Windows.Forms.CheckBox
    Friend WithEvents chkOutline As System.Windows.Forms.CheckBox
    Friend WithEvents chkGenre As System.Windows.Forms.CheckBox
    Friend WithEvents chkDirector As System.Windows.Forms.CheckBox
    Friend WithEvents chkTagline As System.Windows.Forms.CheckBox
    Friend WithEvents chkCast As System.Windows.Forms.CheckBox
    Friend WithEvents chkVotes As System.Windows.Forms.CheckBox
    Friend WithEvents chkTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents chkRating As System.Windows.Forms.CheckBox
    Friend WithEvents chkRelease As System.Windows.Forms.CheckBox
    Friend WithEvents chkMPAA As System.Windows.Forms.CheckBox
    Friend WithEvents chkYear As System.Windows.Forms.CheckBox
    Friend WithEvents chkTitle As System.Windows.Forms.CheckBox
    Friend WithEvents txtGenreLimit As System.Windows.Forms.TextBox
    Friend WithEvents lblLimit2 As System.Windows.Forms.Label
    Friend WithEvents txtActorLimit As System.Windows.Forms.TextBox
    Friend WithEvents lblLimit As System.Windows.Forms.Label
    Friend WithEvents GroupBox27 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMissingExtra As System.Windows.Forms.CheckBox
    Friend WithEvents chkMissingSubs As System.Windows.Forms.CheckBox
    Friend WithEvents chkMissingTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents chkMissingNFO As System.Windows.Forms.CheckBox
    Friend WithEvents chkMissingFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkMissingPoster As System.Windows.Forms.CheckBox
    Friend WithEvents btnDLTrans As System.Windows.Forms.Button
    Friend WithEvents chkTop250 As System.Windows.Forms.CheckBox
    Friend WithEvents chkRenameSingle As System.Windows.Forms.CheckBox
    Friend WithEvents chkRenameMulti As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox28 As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemoveMetaDataFT As System.Windows.Forms.Button
    Friend WithEvents btnEditMetaDataFT As System.Windows.Forms.Button
    Friend WithEvents btnNewMetaDataFT As System.Windows.Forms.Button
    Friend WithEvents lstMetadata As System.Windows.Forms.ListView
    Friend WithEvents txtDefFIExt As System.Windows.Forms.TextBox
    Friend WithEvents Label34 As System.Windows.Forms.Label
    Friend WithEvents cbAutoETSize As System.Windows.Forms.ComboBox
    Friend WithEvents chkAutoETSize As System.Windows.Forms.CheckBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents cbMovieTheme As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox29 As System.Windows.Forms.GroupBox
    Friend WithEvents Label36 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
End Class
