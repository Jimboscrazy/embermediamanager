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
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("File System", 4, 4)
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("XBMC Communication", 1, 1)
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("General", 0, 0, New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2})
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Files and Sources", 5, 5)
        Dim TreeNode5 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Scraper - Data", 3, 3)
        Dim TreeNode6 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Scraper - Images", 6, 6)
        Dim TreeNode7 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Movies", 2, 2, New System.Windows.Forms.TreeNode() {TreeNode4, TreeNode5, TreeNode6})
        Me.fbdBrowse = New System.Windows.Forms.FolderBrowserDialog
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
        Me.chkInfoPanelAnim = New System.Windows.Forms.CheckBox
        Me.chkUpdates = New System.Windows.Forms.CheckBox
        Me.chkOverwriteNfo = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkLogErrors = New System.Windows.Forms.CheckBox
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
        Me.gbColors = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblTopPanelText = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnHeaderText = New System.Windows.Forms.Button
        Me.btnTopPanelText = New System.Windows.Forms.Button
        Me.btnInfoPanelText = New System.Windows.Forms.Button
        Me.lblInfoPanelText = New System.Windows.Forms.Label
        Me.lblPanel = New System.Windows.Forms.Label
        Me.lblHeaderText = New System.Windows.Forms.Label
        Me.lblHeader = New System.Windows.Forms.Label
        Me.btnTopPanel = New System.Windows.Forms.Button
        Me.btnBackground = New System.Windows.Forms.Button
        Me.btnInfoPanel = New System.Windows.Forms.Button
        Me.btnHeaders = New System.Windows.Forms.Button
        Me.gbFilters = New System.Windows.Forms.GroupBox
        Me.btnDown = New System.Windows.Forms.Button
        Me.btnUp = New System.Windows.Forms.Button
        Me.chkProperCase = New System.Windows.Forms.CheckBox
        Me.btnRemoveFilter = New System.Windows.Forms.Button
        Me.btnAddFilter = New System.Windows.Forms.Button
        Me.txtFilter = New System.Windows.Forms.TextBox
        Me.lstFilters = New System.Windows.Forms.ListBox
        Me.chkScanRecursive = New System.Windows.Forms.CheckBox
        Me.GroupBox12 = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.chkMarkNew = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.chkMovieExtraCol = New System.Windows.Forms.CheckBox
        Me.chkMovieSubCol = New System.Windows.Forms.CheckBox
        Me.chkMovieTrailerCol = New System.Windows.Forms.CheckBox
        Me.chkMovieInfoCol = New System.Windows.Forms.CheckBox
        Me.chkMovieFanartCol = New System.Windows.Forms.CheckBox
        Me.chkMoviePosterCol = New System.Windows.Forms.CheckBox
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
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
        Me.chkTitleFromNfo = New System.Windows.Forms.CheckBox
        Me.lvMovies = New System.Windows.Forms.ListView
        Me.colPath = New System.Windows.Forms.ColumnHeader
        Me.colType = New System.Windows.Forms.ColumnHeader
        Me.btnMovieAddFiles = New System.Windows.Forms.Button
        Me.btnMovieRem = New System.Windows.Forms.Button
        Me.btnMovieAddFolder = New System.Windows.Forms.Button
        Me.chkUseFolderNames = New System.Windows.Forms.CheckBox
        Me.GroupBox15 = New System.Windows.Forms.GroupBox
        Me.chkOFDBGenre = New System.Windows.Forms.CheckBox
        Me.chkOFDBPlot = New System.Windows.Forms.CheckBox
        Me.chkOFDBOutline = New System.Windows.Forms.CheckBox
        Me.chkOFDBTitle = New System.Windows.Forms.CheckBox
        Me.GroupBox14 = New System.Windows.Forms.GroupBox
        Me.lblPosterQual = New System.Windows.Forms.Label
        Me.tbPosterQual = New System.Windows.Forms.TrackBar
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.chkResizePoster = New System.Windows.Forms.CheckBox
        Me.txtPosterWidth = New System.Windows.Forms.TextBox
        Me.txtPosterHeight = New System.Windows.Forms.TextBox
        Me.lblPosterSize = New System.Windows.Forms.Label
        Me.cbPosterSize = New System.Windows.Forms.ComboBox
        Me.chkOverwritePoster = New System.Windows.Forms.CheckBox
        Me.GroupBox13 = New System.Windows.Forms.GroupBox
        Me.chkFanartOnly = New System.Windows.Forms.CheckBox
        Me.lblFanartQual = New System.Windows.Forms.Label
        Me.tbFanartQual = New System.Windows.Forms.TrackBar
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.chkResizeFanart = New System.Windows.Forms.CheckBox
        Me.txtFanartWidth = New System.Windows.Forms.TextBox
        Me.txtFanartHeight = New System.Windows.Forms.TextBox
        Me.cbFanartSize = New System.Windows.Forms.ComboBox
        Me.lblFanartSize = New System.Windows.Forms.Label
        Me.chkOverwriteFanart = New System.Windows.Forms.CheckBox
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.chkLockTrailer = New System.Windows.Forms.CheckBox
        Me.chkLockGenre = New System.Windows.Forms.CheckBox
        Me.chkLockRealStudio = New System.Windows.Forms.CheckBox
        Me.chkLockRating = New System.Windows.Forms.CheckBox
        Me.chkLockTagline = New System.Windows.Forms.CheckBox
        Me.chkLockTitle = New System.Windows.Forms.CheckBox
        Me.chkLockOutline = New System.Windows.Forms.CheckBox
        Me.chkLockPlot = New System.Windows.Forms.CheckBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.chkNoSaveImagesToNfo = New System.Windows.Forms.CheckBox
        Me.chkUseETasFA = New System.Windows.Forms.CheckBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.chkNoSpoilers = New System.Windows.Forms.CheckBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtAutoThumbs = New System.Windows.Forms.TextBox
        Me.chkAutoThumbs = New System.Windows.Forms.CheckBox
        Me.chkSingleScrapeImages = New System.Windows.Forms.CheckBox
        Me.chkUseMPDB = New System.Windows.Forms.CheckBox
        Me.chkUseTMDB = New System.Windows.Forms.CheckBox
        Me.chkUseIMPA = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkUseMIStudioTag = New System.Windows.Forms.CheckBox
        Me.chkUseMIDuration = New System.Windows.Forms.CheckBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.txtIMDBURL = New System.Windows.Forms.TextBox
        Me.chkCastWithImg = New System.Windows.Forms.CheckBox
        Me.chkUseCertForMPAA = New System.Windows.Forms.CheckBox
        Me.chkFullCast = New System.Windows.Forms.CheckBox
        Me.chkFullCrew = New System.Windows.Forms.CheckBox
        Me.cbCert = New System.Windows.Forms.ComboBox
        Me.chkCert = New System.Windows.Forms.CheckBox
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
        Me.lbGenre = New System.Windows.Forms.CheckedListBox
        Me.lblGenre = New System.Windows.Forms.Label
        Me.GroupBox16 = New System.Windows.Forms.GroupBox
        Me.chkAutoBD = New System.Windows.Forms.CheckBox
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtBDPath = New System.Windows.Forms.TextBox
        Me.pnlScraper = New System.Windows.Forms.Panel
        Me.GroupBox20 = New System.Windows.Forms.GroupBox
        Me.chkNoDLTrailer = New System.Windows.Forms.CheckBox
        Me.chkSingleScrapeTrailer = New System.Windows.Forms.CheckBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.txtTimeout = New System.Windows.Forms.TextBox
        Me.chkUpdaterTrailer = New System.Windows.Forms.CheckBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.lbTrailerSites = New System.Windows.Forms.CheckedListBox
        Me.chkDownloadTrailer = New System.Windows.Forms.CheckBox
        Me.lblCurrent = New System.Windows.Forms.Label
        Me.pnlCurrent = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.pnlExtensions = New System.Windows.Forms.Panel
        Me.GroupBox18 = New System.Windows.Forms.GroupBox
        Me.btnRemMovieExt = New System.Windows.Forms.Button
        Me.btnAddMovieExt = New System.Windows.Forms.Button
        Me.txtMovieExt = New System.Windows.Forms.TextBox
        Me.lstMovieExts = New System.Windows.Forms.ListBox
        Me.pnlSources = New System.Windows.Forms.Panel
        Me.GroupBox19 = New System.Windows.Forms.GroupBox
        Me.chkSkipStackedSizeCheck = New System.Windows.Forms.CheckBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.txtSkipLessThan = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.pnlImages = New System.Windows.Forms.Panel
        Me.GroupBox17 = New System.Windows.Forms.GroupBox
        Me.chkUseImgCacheUpdaters = New System.Windows.Forms.CheckBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.chkPersistImgCache = New System.Windows.Forms.CheckBox
        Me.chkUseImgCache = New System.Windows.Forms.CheckBox
        Me.GroupBox11.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.tcCleaner.SuspendLayout()
        Me.tpStandard.SuspendLayout()
        Me.tpExpert.SuspendLayout()
        Me.gbColors.SuspendLayout()
        Me.gbFilters.SuspendLayout()
        Me.GroupBox12.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
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
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGeneral.SuspendLayout()
        Me.pnlXBMCCom.SuspendLayout()
        Me.pnlMovies.SuspendLayout()
        Me.GroupBox16.SuspendLayout()
        Me.pnlScraper.SuspendLayout()
        Me.GroupBox20.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.pnlExtensions.SuspendLayout()
        Me.GroupBox18.SuspendLayout()
        Me.pnlSources.SuspendLayout()
        Me.GroupBox19.SuspendLayout()
        Me.pnlImages.SuspendLayout()
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
        Me.GroupBox11.TabIndex = 4
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
        Me.btnEditCom.TabIndex = 14
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
        Me.txtName.TabIndex = 13
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
        Me.txtPassword.Location = New System.Drawing.Point(213, 78)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(80, 20)
        Me.txtPassword.TabIndex = 9
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'btnAddCom
        '
        Me.btnAddCom.Image = CType(resources.GetObject("btnAddCom.Image"), System.Drawing.Image)
        Me.btnAddCom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddCom.Location = New System.Drawing.Point(216, 110)
        Me.btnAddCom.Name = "btnAddCom"
        Me.btnAddCom.Size = New System.Drawing.Size(77, 23)
        Me.btnAddCom.TabIndex = 7
        Me.btnAddCom.Text = "Add New"
        Me.btnAddCom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddCom.UseVisualStyleBackColor = True
        '
        'txtUsername
        '
        Me.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtUsername.Location = New System.Drawing.Point(70, 78)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(80, 20)
        Me.txtUsername.TabIndex = 8
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
        Me.txtPort.TabIndex = 5
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Location = New System.Drawing.Point(70, 47)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(85, 20)
        Me.txtIP.TabIndex = 4
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.chkInfoPanelAnim)
        Me.GroupBox4.Controls.Add(Me.chkUpdates)
        Me.GroupBox4.Controls.Add(Me.chkOverwriteNfo)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.chkLogErrors)
        Me.GroupBox4.Location = New System.Drawing.Point(203, 6)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(185, 333)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Miscellaneous"
        '
        'chkInfoPanelAnim
        '
        Me.chkInfoPanelAnim.AutoSize = True
        Me.chkInfoPanelAnim.Location = New System.Drawing.Point(10, 95)
        Me.chkInfoPanelAnim.Name = "chkInfoPanelAnim"
        Me.chkInfoPanelAnim.Size = New System.Drawing.Size(138, 17)
        Me.chkInfoPanelAnim.TabIndex = 58
        Me.chkInfoPanelAnim.Text = "Enable Panel Animation"
        Me.chkInfoPanelAnim.UseVisualStyleBackColor = True
        '
        'chkUpdates
        '
        Me.chkUpdates.AutoSize = True
        Me.chkUpdates.Location = New System.Drawing.Point(10, 16)
        Me.chkUpdates.Name = "chkUpdates"
        Me.chkUpdates.Size = New System.Drawing.Size(115, 17)
        Me.chkUpdates.TabIndex = 16
        Me.chkUpdates.Text = "Check for Updates"
        Me.chkUpdates.UseVisualStyleBackColor = True
        '
        'chkOverwriteNfo
        '
        Me.chkOverwriteNfo.AutoSize = True
        Me.chkOverwriteNfo.Location = New System.Drawing.Point(10, 53)
        Me.chkOverwriteNfo.Name = "chkOverwriteNfo"
        Me.chkOverwriteNfo.Size = New System.Drawing.Size(172, 17)
        Me.chkOverwriteNfo.TabIndex = 14
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
        Me.chkLogErrors.TabIndex = 13
        Me.chkLogErrors.Text = "Log Errors to File"
        Me.chkLogErrors.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.tcCleaner)
        Me.GroupBox3.Location = New System.Drawing.Point(393, 3)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(196, 336)
        Me.GroupBox3.TabIndex = 1
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
        Me.tcCleaner.TabIndex = 19
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
        Me.chkCleanFolderJPG.TabIndex = 6
        Me.chkCleanFolderJPG.Text = "/folder.jpg"
        Me.chkCleanFolderJPG.UseVisualStyleBackColor = True
        '
        'chkCleanExtrathumbs
        '
        Me.chkCleanExtrathumbs.AutoSize = True
        Me.chkCleanExtrathumbs.Location = New System.Drawing.Point(7, 235)
        Me.chkCleanExtrathumbs.Name = "chkCleanExtrathumbs"
        Me.chkCleanExtrathumbs.Size = New System.Drawing.Size(93, 17)
        Me.chkCleanExtrathumbs.TabIndex = 18
        Me.chkCleanExtrathumbs.Text = "/extrathumbs/"
        Me.chkCleanExtrathumbs.UseVisualStyleBackColor = True
        '
        'chkCleanMovieTBN
        '
        Me.chkCleanMovieTBN.AutoSize = True
        Me.chkCleanMovieTBN.Location = New System.Drawing.Point(7, 29)
        Me.chkCleanMovieTBN.Name = "chkCleanMovieTBN"
        Me.chkCleanMovieTBN.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanMovieTBN.TabIndex = 7
        Me.chkCleanMovieTBN.Text = "/movie.tbn"
        Me.chkCleanMovieTBN.UseVisualStyleBackColor = True
        '
        'chkCleanMovieNameJPG
        '
        Me.chkCleanMovieNameJPG.AutoSize = True
        Me.chkCleanMovieNameJPG.Location = New System.Drawing.Point(7, 123)
        Me.chkCleanMovieNameJPG.Name = "chkCleanMovieNameJPG"
        Me.chkCleanMovieNameJPG.Size = New System.Drawing.Size(88, 17)
        Me.chkCleanMovieNameJPG.TabIndex = 17
        Me.chkCleanMovieNameJPG.Text = "/<movie>.jpg"
        Me.chkCleanMovieNameJPG.UseVisualStyleBackColor = True
        '
        'chkCleanMovieTBNb
        '
        Me.chkCleanMovieTBNb.AutoSize = True
        Me.chkCleanMovieTBNb.Location = New System.Drawing.Point(7, 48)
        Me.chkCleanMovieTBNb.Name = "chkCleanMovieTBNb"
        Me.chkCleanMovieTBNb.Size = New System.Drawing.Size(89, 17)
        Me.chkCleanMovieTBNb.TabIndex = 8
        Me.chkCleanMovieTBNb.Text = "/<movie>.tbn"
        Me.chkCleanMovieTBNb.UseVisualStyleBackColor = True
        '
        'chkCleanMovieJPG
        '
        Me.chkCleanMovieJPG.AutoSize = True
        Me.chkCleanMovieJPG.Location = New System.Drawing.Point(7, 104)
        Me.chkCleanMovieJPG.Name = "chkCleanMovieJPG"
        Me.chkCleanMovieJPG.Size = New System.Drawing.Size(76, 17)
        Me.chkCleanMovieJPG.TabIndex = 16
        Me.chkCleanMovieJPG.Text = "/movie.jpg"
        Me.chkCleanMovieJPG.UseVisualStyleBackColor = True
        '
        'chkCleanFanartJPG
        '
        Me.chkCleanFanartJPG.AutoSize = True
        Me.chkCleanFanartJPG.Location = New System.Drawing.Point(7, 141)
        Me.chkCleanFanartJPG.Name = "chkCleanFanartJPG"
        Me.chkCleanFanartJPG.Size = New System.Drawing.Size(75, 17)
        Me.chkCleanFanartJPG.TabIndex = 9
        Me.chkCleanFanartJPG.Text = "/fanart.jpg"
        Me.chkCleanFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanPosterJPG
        '
        Me.chkCleanPosterJPG.AutoSize = True
        Me.chkCleanPosterJPG.Location = New System.Drawing.Point(7, 86)
        Me.chkCleanPosterJPG.Name = "chkCleanPosterJPG"
        Me.chkCleanPosterJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanPosterJPG.TabIndex = 15
        Me.chkCleanPosterJPG.Text = "/poster.jpg"
        Me.chkCleanPosterJPG.UseVisualStyleBackColor = True
        '
        'chkCleanMovieFanartJPG
        '
        Me.chkCleanMovieFanartJPG.AutoSize = True
        Me.chkCleanMovieFanartJPG.Location = New System.Drawing.Point(7, 160)
        Me.chkCleanMovieFanartJPG.Name = "chkCleanMovieFanartJPG"
        Me.chkCleanMovieFanartJPG.Size = New System.Drawing.Size(118, 17)
        Me.chkCleanMovieFanartJPG.TabIndex = 10
        Me.chkCleanMovieFanartJPG.Text = "/<movie>-fanart.jpg"
        Me.chkCleanMovieFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanPosterTBN
        '
        Me.chkCleanPosterTBN.AutoSize = True
        Me.chkCleanPosterTBN.Location = New System.Drawing.Point(7, 67)
        Me.chkCleanPosterTBN.Name = "chkCleanPosterTBN"
        Me.chkCleanPosterTBN.Size = New System.Drawing.Size(78, 17)
        Me.chkCleanPosterTBN.TabIndex = 14
        Me.chkCleanPosterTBN.Text = "/poster.tbn"
        Me.chkCleanPosterTBN.UseVisualStyleBackColor = True
        '
        'chkCleanMovieNFO
        '
        Me.chkCleanMovieNFO.AutoSize = True
        Me.chkCleanMovieNFO.Location = New System.Drawing.Point(7, 198)
        Me.chkCleanMovieNFO.Name = "chkCleanMovieNFO"
        Me.chkCleanMovieNFO.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanMovieNFO.TabIndex = 11
        Me.chkCleanMovieNFO.Text = "/movie.nfo"
        Me.chkCleanMovieNFO.UseVisualStyleBackColor = True
        '
        'chkCleanDotFanartJPG
        '
        Me.chkCleanDotFanartJPG.AutoSize = True
        Me.chkCleanDotFanartJPG.Location = New System.Drawing.Point(7, 179)
        Me.chkCleanDotFanartJPG.Name = "chkCleanDotFanartJPG"
        Me.chkCleanDotFanartJPG.Size = New System.Drawing.Size(118, 17)
        Me.chkCleanDotFanartJPG.TabIndex = 13
        Me.chkCleanDotFanartJPG.Text = "/<movie>.fanart.jpg"
        Me.chkCleanDotFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanMovieNFOb
        '
        Me.chkCleanMovieNFOb.AutoSize = True
        Me.chkCleanMovieNFOb.Location = New System.Drawing.Point(7, 217)
        Me.chkCleanMovieNFOb.Name = "chkCleanMovieNFOb"
        Me.chkCleanMovieNFOb.Size = New System.Drawing.Size(89, 17)
        Me.chkCleanMovieNFOb.TabIndex = 12
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
        Me.chkWhitelistVideo.TabIndex = 11
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
        Me.btnRemoveWhitelist.TabIndex = 9
        Me.btnRemoveWhitelist.UseVisualStyleBackColor = True
        '
        'btnAddWhitelist
        '
        Me.btnAddWhitelist.Image = CType(resources.GetObject("btnAddWhitelist.Image"), System.Drawing.Image)
        Me.btnAddWhitelist.Location = New System.Drawing.Point(82, 251)
        Me.btnAddWhitelist.Name = "btnAddWhitelist"
        Me.btnAddWhitelist.Size = New System.Drawing.Size(23, 23)
        Me.btnAddWhitelist.TabIndex = 8
        Me.btnAddWhitelist.UseVisualStyleBackColor = True
        '
        'txtWhitelist
        '
        Me.txtWhitelist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtWhitelist.Location = New System.Drawing.Point(20, 252)
        Me.txtWhitelist.Name = "txtWhitelist"
        Me.txtWhitelist.Size = New System.Drawing.Size(61, 20)
        Me.txtWhitelist.TabIndex = 7
        '
        'lstWhitelist
        '
        Me.lstWhitelist.FormattingEnabled = True
        Me.lstWhitelist.Location = New System.Drawing.Point(19, 126)
        Me.lstWhitelist.Name = "lstWhitelist"
        Me.lstWhitelist.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstWhitelist.Size = New System.Drawing.Size(138, 121)
        Me.lstWhitelist.TabIndex = 6
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
        'gbColors
        '
        Me.gbColors.Controls.Add(Me.Label3)
        Me.gbColors.Controls.Add(Me.lblTopPanelText)
        Me.gbColors.Controls.Add(Me.Label1)
        Me.gbColors.Controls.Add(Me.btnHeaderText)
        Me.gbColors.Controls.Add(Me.btnTopPanelText)
        Me.gbColors.Controls.Add(Me.btnInfoPanelText)
        Me.gbColors.Controls.Add(Me.lblInfoPanelText)
        Me.gbColors.Controls.Add(Me.lblPanel)
        Me.gbColors.Controls.Add(Me.lblHeaderText)
        Me.gbColors.Controls.Add(Me.lblHeader)
        Me.gbColors.Controls.Add(Me.btnTopPanel)
        Me.gbColors.Controls.Add(Me.btnBackground)
        Me.gbColors.Controls.Add(Me.btnInfoPanel)
        Me.gbColors.Controls.Add(Me.btnHeaders)
        Me.gbColors.Location = New System.Drawing.Point(6, 186)
        Me.gbColors.Name = "gbColors"
        Me.gbColors.Size = New System.Drawing.Size(193, 153)
        Me.gbColors.TabIndex = 2
        Me.gbColors.TabStop = False
        Me.gbColors.Text = "Colors"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 133)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(95, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Background Color:"
        '
        'lblTopPanelText
        '
        Me.lblTopPanelText.AutoSize = True
        Me.lblTopPanelText.Location = New System.Drawing.Point(6, 114)
        Me.lblTopPanelText.Name = "lblTopPanelText"
        Me.lblTopPanelText.Size = New System.Drawing.Size(110, 13)
        Me.lblTopPanelText.TabIndex = 19
        Me.lblTopPanelText.Text = "Top Panel Text Color:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(147, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Top Panel Background Color:"
        '
        'btnHeaderText
        '
        Me.btnHeaderText.BackColor = System.Drawing.Color.White
        Me.btnHeaderText.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnHeaderText.Location = New System.Drawing.Point(164, 74)
        Me.btnHeaderText.Name = "btnHeaderText"
        Me.btnHeaderText.Size = New System.Drawing.Size(16, 16)
        Me.btnHeaderText.TabIndex = 16
        Me.btnHeaderText.UseVisualStyleBackColor = False
        '
        'btnTopPanelText
        '
        Me.btnTopPanelText.BackColor = System.Drawing.Color.DimGray
        Me.btnTopPanelText.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnTopPanelText.Location = New System.Drawing.Point(164, 112)
        Me.btnTopPanelText.Name = "btnTopPanelText"
        Me.btnTopPanelText.Size = New System.Drawing.Size(16, 16)
        Me.btnTopPanelText.TabIndex = 18
        Me.btnTopPanelText.UseVisualStyleBackColor = False
        '
        'btnInfoPanelText
        '
        Me.btnInfoPanelText.BackColor = System.Drawing.Color.DimGray
        Me.btnInfoPanelText.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnInfoPanelText.Location = New System.Drawing.Point(164, 36)
        Me.btnInfoPanelText.Name = "btnInfoPanelText"
        Me.btnInfoPanelText.Size = New System.Drawing.Size(16, 16)
        Me.btnInfoPanelText.TabIndex = 14
        Me.btnInfoPanelText.UseVisualStyleBackColor = False
        '
        'lblInfoPanelText
        '
        Me.lblInfoPanelText.AutoSize = True
        Me.lblInfoPanelText.Location = New System.Drawing.Point(6, 38)
        Me.lblInfoPanelText.Name = "lblInfoPanelText"
        Me.lblInfoPanelText.Size = New System.Drawing.Size(109, 13)
        Me.lblInfoPanelText.TabIndex = 14
        Me.lblInfoPanelText.Text = "Info Panel Text Color:"
        '
        'lblPanel
        '
        Me.lblPanel.AutoSize = True
        Me.lblPanel.Location = New System.Drawing.Point(6, 19)
        Me.lblPanel.Name = "lblPanel"
        Me.lblPanel.Size = New System.Drawing.Size(146, 13)
        Me.lblPanel.TabIndex = 13
        Me.lblPanel.Text = "Info Panel Background Color:"
        '
        'lblHeaderText
        '
        Me.lblHeaderText.AutoSize = True
        Me.lblHeaderText.Location = New System.Drawing.Point(6, 76)
        Me.lblHeaderText.Name = "lblHeaderText"
        Me.lblHeaderText.Size = New System.Drawing.Size(147, 13)
        Me.lblHeaderText.TabIndex = 12
        Me.lblHeaderText.Text = "Info Panel Header Text Color:"
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Location = New System.Drawing.Point(6, 57)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(123, 13)
        Me.lblHeader.TabIndex = 11
        Me.lblHeader.Text = "Info Panel Header Color:"
        '
        'btnTopPanel
        '
        Me.btnTopPanel.BackColor = System.Drawing.Color.Gainsboro
        Me.btnTopPanel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnTopPanel.Location = New System.Drawing.Point(164, 93)
        Me.btnTopPanel.Name = "btnTopPanel"
        Me.btnTopPanel.Size = New System.Drawing.Size(16, 16)
        Me.btnTopPanel.TabIndex = 17
        Me.btnTopPanel.UseVisualStyleBackColor = False
        '
        'btnBackground
        '
        Me.btnBackground.BackColor = System.Drawing.Color.Gainsboro
        Me.btnBackground.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnBackground.Location = New System.Drawing.Point(164, 131)
        Me.btnBackground.Name = "btnBackground"
        Me.btnBackground.Size = New System.Drawing.Size(16, 16)
        Me.btnBackground.TabIndex = 19
        Me.btnBackground.UseVisualStyleBackColor = False
        '
        'btnInfoPanel
        '
        Me.btnInfoPanel.BackColor = System.Drawing.Color.Gainsboro
        Me.btnInfoPanel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnInfoPanel.Location = New System.Drawing.Point(164, 17)
        Me.btnInfoPanel.Name = "btnInfoPanel"
        Me.btnInfoPanel.Size = New System.Drawing.Size(16, 16)
        Me.btnInfoPanel.TabIndex = 13
        Me.btnInfoPanel.UseVisualStyleBackColor = False
        '
        'btnHeaders
        '
        Me.btnHeaders.BackColor = System.Drawing.Color.DimGray
        Me.btnHeaders.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnHeaders.Location = New System.Drawing.Point(164, 55)
        Me.btnHeaders.Name = "btnHeaders"
        Me.btnHeaders.Size = New System.Drawing.Size(16, 16)
        Me.btnHeaders.TabIndex = 15
        Me.btnHeaders.UseVisualStyleBackColor = False
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
        Me.btnDown.TabIndex = 9
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
        Me.btnUp.Location = New System.Drawing.Point(105, 146)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(23, 23)
        Me.btnUp.TabIndex = 8
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'chkProperCase
        '
        Me.chkProperCase.AutoSize = True
        Me.chkProperCase.Location = New System.Drawing.Point(6, 16)
        Me.chkProperCase.Name = "chkProperCase"
        Me.chkProperCase.Size = New System.Drawing.Size(172, 17)
        Me.chkProperCase.TabIndex = 7
        Me.chkProperCase.Text = "Convert Names to Proper Case"
        Me.chkProperCase.UseVisualStyleBackColor = True
        '
        'btnRemoveFilter
        '
        Me.btnRemoveFilter.Image = CType(resources.GetObject("btnRemoveFilter.Image"), System.Drawing.Image)
        Me.btnRemoveFilter.Location = New System.Drawing.Point(163, 146)
        Me.btnRemoveFilter.Name = "btnRemoveFilter"
        Me.btnRemoveFilter.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveFilter.TabIndex = 5
        Me.btnRemoveFilter.UseVisualStyleBackColor = True
        '
        'btnAddFilter
        '
        Me.btnAddFilter.Image = CType(resources.GetObject("btnAddFilter.Image"), System.Drawing.Image)
        Me.btnAddFilter.Location = New System.Drawing.Point(68, 146)
        Me.btnAddFilter.Name = "btnAddFilter"
        Me.btnAddFilter.Size = New System.Drawing.Size(23, 23)
        Me.btnAddFilter.TabIndex = 4
        Me.btnAddFilter.UseVisualStyleBackColor = True
        '
        'txtFilter
        '
        Me.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilter.Location = New System.Drawing.Point(6, 147)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(61, 20)
        Me.txtFilter.TabIndex = 3
        '
        'lstFilters
        '
        Me.lstFilters.FormattingEnabled = True
        Me.lstFilters.Location = New System.Drawing.Point(6, 34)
        Me.lstFilters.Name = "lstFilters"
        Me.lstFilters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFilters.Size = New System.Drawing.Size(180, 108)
        Me.lstFilters.TabIndex = 2
        '
        'chkScanRecursive
        '
        Me.chkScanRecursive.Location = New System.Drawing.Point(6, 59)
        Me.chkScanRecursive.Name = "chkScanRecursive"
        Me.chkScanRecursive.Size = New System.Drawing.Size(187, 37)
        Me.chkScanRecursive.TabIndex = 66
        Me.chkScanRecursive.Text = "Scan Folder Sources Recursively (Increases Scanning Time)"
        Me.chkScanRecursive.UseVisualStyleBackColor = True
        '
        'GroupBox12
        '
        Me.GroupBox12.Controls.Add(Me.Label8)
        Me.GroupBox12.Controls.Add(Me.chkMarkNew)
        Me.GroupBox12.Controls.Add(Me.GroupBox2)
        Me.GroupBox12.Location = New System.Drawing.Point(209, 10)
        Me.GroupBox12.Name = "GroupBox12"
        Me.GroupBox12.Size = New System.Drawing.Size(200, 189)
        Me.GroupBox12.TabIndex = 65
        Me.GroupBox12.TabStop = False
        Me.GroupBox12.Text = "Miscellaneous"
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
        Me.chkMarkNew.TabIndex = 55
        Me.chkMarkNew.Text = "Mark New Movies"
        Me.chkMarkNew.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkMovieExtraCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieSubCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieTrailerCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieInfoCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieFanartCol)
        Me.GroupBox2.Controls.Add(Me.chkMoviePosterCol)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 62)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(188, 114)
        Me.GroupBox2.TabIndex = 54
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Media List Options"
        '
        'chkMovieExtraCol
        '
        Me.chkMovieExtraCol.AutoSize = True
        Me.chkMovieExtraCol.Location = New System.Drawing.Point(6, 95)
        Me.chkMovieExtraCol.Name = "chkMovieExtraCol"
        Me.chkMovieExtraCol.Size = New System.Drawing.Size(142, 17)
        Me.chkMovieExtraCol.TabIndex = 33
        Me.chkMovieExtraCol.Text = "Hide Extrathumb Column"
        Me.chkMovieExtraCol.UseVisualStyleBackColor = True
        '
        'chkMovieSubCol
        '
        Me.chkMovieSubCol.AutoSize = True
        Me.chkMovieSubCol.Location = New System.Drawing.Point(6, 79)
        Me.chkMovieSubCol.Name = "chkMovieSubCol"
        Me.chkMovieSubCol.Size = New System.Drawing.Size(108, 17)
        Me.chkMovieSubCol.TabIndex = 32
        Me.chkMovieSubCol.Text = "Hide Sub Column"
        Me.chkMovieSubCol.UseVisualStyleBackColor = True
        '
        'chkMovieTrailerCol
        '
        Me.chkMovieTrailerCol.AutoSize = True
        Me.chkMovieTrailerCol.Location = New System.Drawing.Point(6, 63)
        Me.chkMovieTrailerCol.Name = "chkMovieTrailerCol"
        Me.chkMovieTrailerCol.Size = New System.Drawing.Size(118, 17)
        Me.chkMovieTrailerCol.TabIndex = 31
        Me.chkMovieTrailerCol.Text = "Hide Trailer Column"
        Me.chkMovieTrailerCol.UseVisualStyleBackColor = True
        '
        'chkMovieInfoCol
        '
        Me.chkMovieInfoCol.AutoSize = True
        Me.chkMovieInfoCol.Location = New System.Drawing.Point(6, 47)
        Me.chkMovieInfoCol.Name = "chkMovieInfoCol"
        Me.chkMovieInfoCol.Size = New System.Drawing.Size(107, 17)
        Me.chkMovieInfoCol.TabIndex = 30
        Me.chkMovieInfoCol.Text = "Hide Info Column"
        Me.chkMovieInfoCol.UseVisualStyleBackColor = True
        '
        'chkMovieFanartCol
        '
        Me.chkMovieFanartCol.AutoSize = True
        Me.chkMovieFanartCol.Location = New System.Drawing.Point(6, 31)
        Me.chkMovieFanartCol.Name = "chkMovieFanartCol"
        Me.chkMovieFanartCol.Size = New System.Drawing.Size(119, 17)
        Me.chkMovieFanartCol.TabIndex = 29
        Me.chkMovieFanartCol.Text = "Hide Fanart Column"
        Me.chkMovieFanartCol.UseVisualStyleBackColor = True
        '
        'chkMoviePosterCol
        '
        Me.chkMoviePosterCol.AutoSize = True
        Me.chkMoviePosterCol.Location = New System.Drawing.Point(6, 15)
        Me.chkMoviePosterCol.Name = "chkMoviePosterCol"
        Me.chkMoviePosterCol.Size = New System.Drawing.Size(119, 17)
        Me.chkMoviePosterCol.TabIndex = 28
        Me.chkMoviePosterCol.Text = "Hide Poster Column"
        Me.chkMoviePosterCol.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.chkVideoTSParent)
        Me.GroupBox8.Controls.Add(Me.GroupBox7)
        Me.GroupBox8.Controls.Add(Me.GroupBox6)
        Me.GroupBox8.Controls.Add(Me.GroupBox5)
        Me.GroupBox8.Location = New System.Drawing.Point(229, 121)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(356, 146)
        Me.GroupBox8.TabIndex = 64
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "File Naming"
        '
        'chkVideoTSParent
        '
        Me.chkVideoTSParent.Location = New System.Drawing.Point(16, 106)
        Me.chkVideoTSParent.Name = "chkVideoTSParent"
        Me.chkVideoTSParent.Size = New System.Drawing.Size(173, 33)
        Me.chkVideoTSParent.TabIndex = 66
        Me.chkVideoTSParent.Text = "YAMJ Compatible VIDEO_TS File Placement/Naming"
        Me.chkVideoTSParent.UseVisualStyleBackColor = True
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.chkMovieNameNFO)
        Me.GroupBox7.Controls.Add(Me.chkMovieNFO)
        Me.GroupBox7.Location = New System.Drawing.Point(229, 88)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(120, 53)
        Me.GroupBox7.TabIndex = 65
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "NFO"
        '
        'chkMovieNameNFO
        '
        Me.chkMovieNameNFO.AutoSize = True
        Me.chkMovieNameNFO.Location = New System.Drawing.Point(6, 34)
        Me.chkMovieNameNFO.Name = "chkMovieNameNFO"
        Me.chkMovieNameNFO.Size = New System.Drawing.Size(84, 17)
        Me.chkMovieNameNFO.TabIndex = 69
        Me.chkMovieNameNFO.Text = "<movie>.nfo"
        Me.chkMovieNameNFO.UseVisualStyleBackColor = True
        '
        'chkMovieNFO
        '
        Me.chkMovieNFO.AutoSize = True
        Me.chkMovieNFO.Location = New System.Drawing.Point(6, 18)
        Me.chkMovieNFO.Name = "chkMovieNFO"
        Me.chkMovieNFO.Size = New System.Drawing.Size(72, 17)
        Me.chkMovieNFO.TabIndex = 68
        Me.chkMovieNFO.Text = "movie.nfo"
        Me.chkMovieNFO.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.chkMovieNameDotFanartJPG)
        Me.GroupBox6.Controls.Add(Me.chkMovieNameFanartJPG)
        Me.GroupBox6.Controls.Add(Me.chkFanartJPG)
        Me.GroupBox6.Location = New System.Drawing.Point(229, 14)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(120, 70)
        Me.GroupBox6.TabIndex = 64
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Fanart"
        '
        'chkMovieNameDotFanartJPG
        '
        Me.chkMovieNameDotFanartJPG.AutoSize = True
        Me.chkMovieNameDotFanartJPG.Location = New System.Drawing.Point(6, 51)
        Me.chkMovieNameDotFanartJPG.Name = "chkMovieNameDotFanartJPG"
        Me.chkMovieNameDotFanartJPG.Size = New System.Drawing.Size(113, 17)
        Me.chkMovieNameDotFanartJPG.TabIndex = 68
        Me.chkMovieNameDotFanartJPG.Text = "<movie>.fanart.jpg"
        Me.chkMovieNameDotFanartJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameFanartJPG
        '
        Me.chkMovieNameFanartJPG.AutoSize = True
        Me.chkMovieNameFanartJPG.Location = New System.Drawing.Point(6, 35)
        Me.chkMovieNameFanartJPG.Name = "chkMovieNameFanartJPG"
        Me.chkMovieNameFanartJPG.Size = New System.Drawing.Size(113, 17)
        Me.chkMovieNameFanartJPG.TabIndex = 67
        Me.chkMovieNameFanartJPG.Text = "<movie>-fanart.jpg"
        Me.chkMovieNameFanartJPG.UseVisualStyleBackColor = True
        '
        'chkFanartJPG
        '
        Me.chkFanartJPG.AutoSize = True
        Me.chkFanartJPG.Location = New System.Drawing.Point(6, 19)
        Me.chkFanartJPG.Name = "chkFanartJPG"
        Me.chkFanartJPG.Size = New System.Drawing.Size(70, 17)
        Me.chkFanartJPG.TabIndex = 66
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
        Me.GroupBox5.Location = New System.Drawing.Point(6, 14)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(199, 83)
        Me.GroupBox5.TabIndex = 61
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Posters"
        '
        'chkFolderJPG
        '
        Me.chkFolderJPG.AutoSize = True
        Me.chkFolderJPG.Location = New System.Drawing.Point(10, 63)
        Me.chkFolderJPG.Name = "chkFolderJPG"
        Me.chkFolderJPG.Size = New System.Drawing.Size(69, 17)
        Me.chkFolderJPG.TabIndex = 70
        Me.chkFolderJPG.Text = "folder.jpg"
        Me.chkFolderJPG.UseVisualStyleBackColor = True
        '
        'chkPosterJPG
        '
        Me.chkPosterJPG.AutoSize = True
        Me.chkPosterJPG.Location = New System.Drawing.Point(101, 47)
        Me.chkPosterJPG.Name = "chkPosterJPG"
        Me.chkPosterJPG.Size = New System.Drawing.Size(72, 17)
        Me.chkPosterJPG.TabIndex = 69
        Me.chkPosterJPG.Text = "poster.jpg"
        Me.chkPosterJPG.UseVisualStyleBackColor = True
        '
        'chkPosterTBN
        '
        Me.chkPosterTBN.AutoSize = True
        Me.chkPosterTBN.Location = New System.Drawing.Point(10, 47)
        Me.chkPosterTBN.Name = "chkPosterTBN"
        Me.chkPosterTBN.Size = New System.Drawing.Size(73, 17)
        Me.chkPosterTBN.TabIndex = 68
        Me.chkPosterTBN.Text = "poster.tbn"
        Me.chkPosterTBN.UseVisualStyleBackColor = True
        '
        'chkMovieNameJPG
        '
        Me.chkMovieNameJPG.AutoSize = True
        Me.chkMovieNameJPG.Location = New System.Drawing.Point(101, 31)
        Me.chkMovieNameJPG.Name = "chkMovieNameJPG"
        Me.chkMovieNameJPG.Size = New System.Drawing.Size(83, 17)
        Me.chkMovieNameJPG.TabIndex = 67
        Me.chkMovieNameJPG.Text = "<movie>.jpg"
        Me.chkMovieNameJPG.UseVisualStyleBackColor = True
        '
        'chkMovieJPG
        '
        Me.chkMovieJPG.AutoSize = True
        Me.chkMovieJPG.Location = New System.Drawing.Point(10, 31)
        Me.chkMovieJPG.Name = "chkMovieJPG"
        Me.chkMovieJPG.Size = New System.Drawing.Size(71, 17)
        Me.chkMovieJPG.TabIndex = 66
        Me.chkMovieJPG.Text = "movie.jpg"
        Me.chkMovieJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameTBN
        '
        Me.chkMovieNameTBN.AutoSize = True
        Me.chkMovieNameTBN.Location = New System.Drawing.Point(101, 15)
        Me.chkMovieNameTBN.Name = "chkMovieNameTBN"
        Me.chkMovieNameTBN.Size = New System.Drawing.Size(84, 17)
        Me.chkMovieNameTBN.TabIndex = 65
        Me.chkMovieNameTBN.Text = "<movie>.tbn"
        Me.chkMovieNameTBN.UseVisualStyleBackColor = True
        '
        'chkMovieTBN
        '
        Me.chkMovieTBN.AutoSize = True
        Me.chkMovieTBN.Location = New System.Drawing.Point(10, 15)
        Me.chkMovieTBN.Name = "chkMovieTBN"
        Me.chkMovieTBN.Size = New System.Drawing.Size(72, 17)
        Me.chkMovieTBN.TabIndex = 64
        Me.chkMovieTBN.Text = "movie.tbn"
        Me.chkMovieTBN.UseVisualStyleBackColor = True
        '
        'chkTitleFromNfo
        '
        Me.chkTitleFromNfo.Location = New System.Drawing.Point(6, 34)
        Me.chkTitleFromNfo.Name = "chkTitleFromNfo"
        Me.chkTitleFromNfo.Size = New System.Drawing.Size(187, 31)
        Me.chkTitleFromNfo.TabIndex = 53
        Me.chkTitleFromNfo.Text = "Use Title From NFO if Available (Increases Scanning Time)"
        Me.chkTitleFromNfo.UseVisualStyleBackColor = True
        '
        'lvMovies
        '
        Me.lvMovies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPath, Me.colType})
        Me.lvMovies.FullRowSelect = True
        Me.lvMovies.HideSelection = False
        Me.lvMovies.Location = New System.Drawing.Point(5, 6)
        Me.lvMovies.Name = "lvMovies"
        Me.lvMovies.Size = New System.Drawing.Size(466, 105)
        Me.lvMovies.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvMovies.TabIndex = 49
        Me.lvMovies.UseCompatibleStateImageBehavior = False
        Me.lvMovies.View = System.Windows.Forms.View.Details
        '
        'colPath
        '
        Me.colPath.Text = "Path"
        Me.colPath.Width = 388
        '
        'colType
        '
        Me.colType.Text = "Folders/Files"
        Me.colType.Width = 74
        '
        'btnMovieAddFiles
        '
        Me.btnMovieAddFiles.Image = CType(resources.GetObject("btnMovieAddFiles.Image"), System.Drawing.Image)
        Me.btnMovieAddFiles.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovieAddFiles.Location = New System.Drawing.Point(481, 35)
        Me.btnMovieAddFiles.Name = "btnMovieAddFiles"
        Me.btnMovieAddFiles.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieAddFiles.TabIndex = 51
        Me.btnMovieAddFiles.Text = "Files Path"
        Me.btnMovieAddFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovieAddFiles.UseVisualStyleBackColor = True
        '
        'btnMovieRem
        '
        Me.btnMovieRem.Image = CType(resources.GetObject("btnMovieRem.Image"), System.Drawing.Image)
        Me.btnMovieRem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovieRem.Location = New System.Drawing.Point(481, 88)
        Me.btnMovieRem.Name = "btnMovieRem"
        Me.btnMovieRem.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieRem.TabIndex = 52
        Me.btnMovieRem.Text = "Remove"
        Me.btnMovieRem.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovieRem.UseVisualStyleBackColor = True
        '
        'btnMovieAddFolder
        '
        Me.btnMovieAddFolder.Image = CType(resources.GetObject("btnMovieAddFolder.Image"), System.Drawing.Image)
        Me.btnMovieAddFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovieAddFolder.Location = New System.Drawing.Point(481, 6)
        Me.btnMovieAddFolder.Name = "btnMovieAddFolder"
        Me.btnMovieAddFolder.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieAddFolder.TabIndex = 50
        Me.btnMovieAddFolder.Text = "Folders Path"
        Me.btnMovieAddFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovieAddFolder.UseVisualStyleBackColor = True
        '
        'chkUseFolderNames
        '
        Me.chkUseFolderNames.AutoSize = True
        Me.chkUseFolderNames.Location = New System.Drawing.Point(6, 16)
        Me.chkUseFolderNames.Name = "chkUseFolderNames"
        Me.chkUseFolderNames.Size = New System.Drawing.Size(146, 17)
        Me.chkUseFolderNames.TabIndex = 56
        Me.chkUseFolderNames.Text = "Use Folder Name for Title"
        Me.chkUseFolderNames.UseVisualStyleBackColor = True
        '
        'GroupBox15
        '
        Me.GroupBox15.Controls.Add(Me.chkOFDBGenre)
        Me.GroupBox15.Controls.Add(Me.chkOFDBPlot)
        Me.GroupBox15.Controls.Add(Me.chkOFDBOutline)
        Me.GroupBox15.Controls.Add(Me.chkOFDBTitle)
        Me.GroupBox15.Location = New System.Drawing.Point(398, 6)
        Me.GroupBox15.Name = "GroupBox15"
        Me.GroupBox15.Size = New System.Drawing.Size(125, 89)
        Me.GroupBox15.TabIndex = 60
        Me.GroupBox15.TabStop = False
        Me.GroupBox15.Text = "OFDB"
        '
        'chkOFDBGenre
        '
        Me.chkOFDBGenre.AutoSize = True
        Me.chkOFDBGenre.Location = New System.Drawing.Point(6, 66)
        Me.chkOFDBGenre.Name = "chkOFDBGenre"
        Me.chkOFDBGenre.Size = New System.Drawing.Size(109, 17)
        Me.chkOFDBGenre.TabIndex = 3
        Me.chkOFDBGenre.Text = "Use OFDB Genre"
        Me.chkOFDBGenre.UseVisualStyleBackColor = True
        '
        'chkOFDBPlot
        '
        Me.chkOFDBPlot.AutoSize = True
        Me.chkOFDBPlot.Location = New System.Drawing.Point(6, 50)
        Me.chkOFDBPlot.Name = "chkOFDBPlot"
        Me.chkOFDBPlot.Size = New System.Drawing.Size(98, 17)
        Me.chkOFDBPlot.TabIndex = 2
        Me.chkOFDBPlot.Text = "Use OFDB Plot"
        Me.chkOFDBPlot.UseVisualStyleBackColor = True
        '
        'chkOFDBOutline
        '
        Me.chkOFDBOutline.Location = New System.Drawing.Point(6, 34)
        Me.chkOFDBOutline.Name = "chkOFDBOutline"
        Me.chkOFDBOutline.Size = New System.Drawing.Size(113, 17)
        Me.chkOFDBOutline.TabIndex = 1
        Me.chkOFDBOutline.Text = "Use OFDB Outline"
        Me.chkOFDBOutline.UseVisualStyleBackColor = True
        '
        'chkOFDBTitle
        '
        Me.chkOFDBTitle.AutoSize = True
        Me.chkOFDBTitle.Location = New System.Drawing.Point(6, 18)
        Me.chkOFDBTitle.Name = "chkOFDBTitle"
        Me.chkOFDBTitle.Size = New System.Drawing.Size(100, 17)
        Me.chkOFDBTitle.TabIndex = 0
        Me.chkOFDBTitle.Text = "Use OFDB Title"
        Me.chkOFDBTitle.UseVisualStyleBackColor = True
        '
        'GroupBox14
        '
        Me.GroupBox14.Controls.Add(Me.lblPosterQual)
        Me.GroupBox14.Controls.Add(Me.tbPosterQual)
        Me.GroupBox14.Controls.Add(Me.Label24)
        Me.GroupBox14.Controls.Add(Me.Label11)
        Me.GroupBox14.Controls.Add(Me.Label12)
        Me.GroupBox14.Controls.Add(Me.chkResizePoster)
        Me.GroupBox14.Controls.Add(Me.txtPosterWidth)
        Me.GroupBox14.Controls.Add(Me.txtPosterHeight)
        Me.GroupBox14.Controls.Add(Me.lblPosterSize)
        Me.GroupBox14.Controls.Add(Me.cbPosterSize)
        Me.GroupBox14.Controls.Add(Me.chkOverwritePoster)
        Me.GroupBox14.Location = New System.Drawing.Point(215, 3)
        Me.GroupBox14.Name = "GroupBox14"
        Me.GroupBox14.Size = New System.Drawing.Size(249, 170)
        Me.GroupBox14.TabIndex = 59
        Me.GroupBox14.TabStop = False
        Me.GroupBox14.Text = "Poster"
        '
        'lblPosterQual
        '
        Me.lblPosterQual.AutoSize = True
        Me.lblPosterQual.Location = New System.Drawing.Point(212, 142)
        Me.lblPosterQual.Name = "lblPosterQual"
        Me.lblPosterQual.Size = New System.Drawing.Size(25, 13)
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
        Me.tbPosterQual.Size = New System.Drawing.Size(200, 27)
        Me.tbPosterQual.TabIndex = 45
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
        Me.Label12.Location = New System.Drawing.Point(124, 98)
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
        Me.chkResizePoster.TabIndex = 39
        Me.chkResizePoster.Text = "Automatically Resize Poster:"
        Me.chkResizePoster.UseVisualStyleBackColor = True
        '
        'txtPosterWidth
        '
        Me.txtPosterWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPosterWidth.Enabled = False
        Me.txtPosterWidth.Location = New System.Drawing.Point(65, 94)
        Me.txtPosterWidth.Name = "txtPosterWidth"
        Me.txtPosterWidth.Size = New System.Drawing.Size(53, 20)
        Me.txtPosterWidth.TabIndex = 40
        '
        'txtPosterHeight
        '
        Me.txtPosterHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPosterHeight.Enabled = False
        Me.txtPosterHeight.Location = New System.Drawing.Point(190, 94)
        Me.txtPosterHeight.Name = "txtPosterHeight"
        Me.txtPosterHeight.Size = New System.Drawing.Size(53, 20)
        Me.txtPosterHeight.TabIndex = 41
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
        Me.cbPosterSize.Items.AddRange(New Object() {"X-Large", "Large", "Medium", "Small", "Wide"})
        Me.cbPosterSize.Location = New System.Drawing.Point(6, 33)
        Me.cbPosterSize.Name = "cbPosterSize"
        Me.cbPosterSize.Size = New System.Drawing.Size(179, 21)
        Me.cbPosterSize.TabIndex = 41
        '
        'chkOverwritePoster
        '
        Me.chkOverwritePoster.AutoSize = True
        Me.chkOverwritePoster.Location = New System.Drawing.Point(6, 57)
        Me.chkOverwritePoster.Name = "chkOverwritePoster"
        Me.chkOverwritePoster.Size = New System.Drawing.Size(143, 17)
        Me.chkOverwritePoster.TabIndex = 37
        Me.chkOverwritePoster.Text = "Overwrite Existing Poster"
        Me.chkOverwritePoster.UseVisualStyleBackColor = True
        '
        'GroupBox13
        '
        Me.GroupBox13.Controls.Add(Me.chkFanartOnly)
        Me.GroupBox13.Controls.Add(Me.lblFanartQual)
        Me.GroupBox13.Controls.Add(Me.tbFanartQual)
        Me.GroupBox13.Controls.Add(Me.Label26)
        Me.GroupBox13.Controls.Add(Me.Label9)
        Me.GroupBox13.Controls.Add(Me.Label10)
        Me.GroupBox13.Controls.Add(Me.chkResizeFanart)
        Me.GroupBox13.Controls.Add(Me.txtFanartWidth)
        Me.GroupBox13.Controls.Add(Me.txtFanartHeight)
        Me.GroupBox13.Controls.Add(Me.cbFanartSize)
        Me.GroupBox13.Controls.Add(Me.lblFanartSize)
        Me.GroupBox13.Controls.Add(Me.chkOverwriteFanart)
        Me.GroupBox13.Location = New System.Drawing.Point(216, 175)
        Me.GroupBox13.Name = "GroupBox13"
        Me.GroupBox13.Size = New System.Drawing.Size(249, 170)
        Me.GroupBox13.TabIndex = 58
        Me.GroupBox13.TabStop = False
        Me.GroupBox13.Text = "Fanart"
        '
        'chkFanartOnly
        '
        Me.chkFanartOnly.AutoSize = True
        Me.chkFanartOnly.Location = New System.Drawing.Point(191, 32)
        Me.chkFanartOnly.Name = "chkFanartOnly"
        Me.chkFanartOnly.Size = New System.Drawing.Size(47, 17)
        Me.chkFanartOnly.TabIndex = 50
        Me.chkFanartOnly.Text = "Only"
        Me.chkFanartOnly.UseVisualStyleBackColor = True
        '
        'lblFanartQual
        '
        Me.lblFanartQual.AutoSize = True
        Me.lblFanartQual.Location = New System.Drawing.Point(211, 142)
        Me.lblFanartQual.Name = "lblFanartQual"
        Me.lblFanartQual.Size = New System.Drawing.Size(25, 13)
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
        Me.tbFanartQual.Size = New System.Drawing.Size(200, 27)
        Me.tbFanartQual.TabIndex = 48
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
        Me.Label10.Location = New System.Drawing.Point(124, 97)
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
        Me.chkResizeFanart.TabIndex = 39
        Me.chkResizeFanart.Text = "Automatically Resize Fanart:"
        Me.chkResizeFanart.UseVisualStyleBackColor = True
        '
        'txtFanartWidth
        '
        Me.txtFanartWidth.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFanartWidth.Enabled = False
        Me.txtFanartWidth.Location = New System.Drawing.Point(65, 93)
        Me.txtFanartWidth.Name = "txtFanartWidth"
        Me.txtFanartWidth.Size = New System.Drawing.Size(53, 20)
        Me.txtFanartWidth.TabIndex = 40
        '
        'txtFanartHeight
        '
        Me.txtFanartHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFanartHeight.Enabled = False
        Me.txtFanartHeight.Location = New System.Drawing.Point(190, 93)
        Me.txtFanartHeight.Name = "txtFanartHeight"
        Me.txtFanartHeight.Size = New System.Drawing.Size(53, 20)
        Me.txtFanartHeight.TabIndex = 41
        '
        'cbFanartSize
        '
        Me.cbFanartSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFanartSize.FormattingEnabled = True
        Me.cbFanartSize.Items.AddRange(New Object() {"Large", "Medium", "Small"})
        Me.cbFanartSize.Location = New System.Drawing.Point(6, 30)
        Me.cbFanartSize.Name = "cbFanartSize"
        Me.cbFanartSize.Size = New System.Drawing.Size(179, 21)
        Me.cbFanartSize.TabIndex = 42
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
        Me.chkOverwriteFanart.TabIndex = 38
        Me.chkOverwriteFanart.Text = "Overwrite Existing Fanart"
        Me.chkOverwriteFanart.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.chkLockTrailer)
        Me.GroupBox10.Controls.Add(Me.chkLockGenre)
        Me.GroupBox10.Controls.Add(Me.chkLockRealStudio)
        Me.GroupBox10.Controls.Add(Me.chkLockRating)
        Me.GroupBox10.Controls.Add(Me.chkLockTagline)
        Me.GroupBox10.Controls.Add(Me.chkLockTitle)
        Me.GroupBox10.Controls.Add(Me.chkLockOutline)
        Me.GroupBox10.Controls.Add(Me.chkLockPlot)
        Me.GroupBox10.Location = New System.Drawing.Point(222, 6)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(170, 164)
        Me.GroupBox10.TabIndex = 57
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Global Locks (Do not allow updates during scraping)"
        '
        'chkLockTrailer
        '
        Me.chkLockTrailer.AutoSize = True
        Me.chkLockTrailer.Location = New System.Drawing.Point(6, 143)
        Me.chkLockTrailer.Name = "chkLockTrailer"
        Me.chkLockTrailer.Size = New System.Drawing.Size(82, 17)
        Me.chkLockTrailer.TabIndex = 46
        Me.chkLockTrailer.Text = "Lock Trailer"
        Me.chkLockTrailer.UseVisualStyleBackColor = True
        '
        'chkLockGenre
        '
        Me.chkLockGenre.AutoSize = True
        Me.chkLockGenre.Location = New System.Drawing.Point(6, 127)
        Me.chkLockGenre.Name = "chkLockGenre"
        Me.chkLockGenre.Size = New System.Drawing.Size(82, 17)
        Me.chkLockGenre.TabIndex = 45
        Me.chkLockGenre.Text = "Lock Genre"
        Me.chkLockGenre.UseVisualStyleBackColor = True
        '
        'chkLockRealStudio
        '
        Me.chkLockRealStudio.AutoSize = True
        Me.chkLockRealStudio.Location = New System.Drawing.Point(6, 111)
        Me.chkLockRealStudio.Name = "chkLockRealStudio"
        Me.chkLockRealStudio.Size = New System.Drawing.Size(108, 17)
        Me.chkLockRealStudio.TabIndex = 44
        Me.chkLockRealStudio.Text = "Lock Real Studio"
        Me.chkLockRealStudio.UseVisualStyleBackColor = True
        '
        'chkLockRating
        '
        Me.chkLockRating.AutoSize = True
        Me.chkLockRating.Location = New System.Drawing.Point(6, 95)
        Me.chkLockRating.Name = "chkLockRating"
        Me.chkLockRating.Size = New System.Drawing.Size(84, 17)
        Me.chkLockRating.TabIndex = 43
        Me.chkLockRating.Text = "Lock Rating"
        Me.chkLockRating.UseVisualStyleBackColor = True
        '
        'chkLockTagline
        '
        Me.chkLockTagline.AutoSize = True
        Me.chkLockTagline.Location = New System.Drawing.Point(6, 79)
        Me.chkLockTagline.Name = "chkLockTagline"
        Me.chkLockTagline.Size = New System.Drawing.Size(88, 17)
        Me.chkLockTagline.TabIndex = 42
        Me.chkLockTagline.Text = "Lock Tagline"
        Me.chkLockTagline.UseVisualStyleBackColor = True
        '
        'chkLockTitle
        '
        Me.chkLockTitle.AutoSize = True
        Me.chkLockTitle.Location = New System.Drawing.Point(6, 63)
        Me.chkLockTitle.Name = "chkLockTitle"
        Me.chkLockTitle.Size = New System.Drawing.Size(73, 17)
        Me.chkLockTitle.TabIndex = 41
        Me.chkLockTitle.Text = "Lock Title"
        Me.chkLockTitle.UseVisualStyleBackColor = True
        '
        'chkLockOutline
        '
        Me.chkLockOutline.AutoSize = True
        Me.chkLockOutline.Location = New System.Drawing.Point(6, 47)
        Me.chkLockOutline.Name = "chkLockOutline"
        Me.chkLockOutline.Size = New System.Drawing.Size(86, 17)
        Me.chkLockOutline.TabIndex = 40
        Me.chkLockOutline.Text = "Lock Outline"
        Me.chkLockOutline.UseVisualStyleBackColor = True
        '
        'chkLockPlot
        '
        Me.chkLockPlot.AutoSize = True
        Me.chkLockPlot.Location = New System.Drawing.Point(6, 31)
        Me.chkLockPlot.Name = "chkLockPlot"
        Me.chkLockPlot.Size = New System.Drawing.Size(71, 17)
        Me.chkLockPlot.TabIndex = 39
        Me.chkLockPlot.Text = "Lock Plot"
        Me.chkLockPlot.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.chkNoSaveImagesToNfo)
        Me.GroupBox9.Controls.Add(Me.chkUseETasFA)
        Me.GroupBox9.Controls.Add(Me.Label17)
        Me.GroupBox9.Controls.Add(Me.chkNoSpoilers)
        Me.GroupBox9.Controls.Add(Me.Label15)
        Me.GroupBox9.Controls.Add(Me.txtAutoThumbs)
        Me.GroupBox9.Controls.Add(Me.chkAutoThumbs)
        Me.GroupBox9.Controls.Add(Me.chkSingleScrapeImages)
        Me.GroupBox9.Controls.Add(Me.chkUseMPDB)
        Me.GroupBox9.Controls.Add(Me.chkUseTMDB)
        Me.GroupBox9.Controls.Add(Me.chkUseIMPA)
        Me.GroupBox9.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(207, 231)
        Me.GroupBox9.TabIndex = 56
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Images"
        '
        'chkNoSaveImagesToNfo
        '
        Me.chkNoSaveImagesToNfo.AutoSize = True
        Me.chkNoSaveImagesToNfo.Location = New System.Drawing.Point(6, 83)
        Me.chkNoSaveImagesToNfo.Name = "chkNoSaveImagesToNfo"
        Me.chkNoSaveImagesToNfo.Size = New System.Drawing.Size(182, 17)
        Me.chkNoSaveImagesToNfo.TabIndex = 65
        Me.chkNoSaveImagesToNfo.Text = "Do Not Save Image URLs to Nfo"
        Me.chkNoSaveImagesToNfo.UseVisualStyleBackColor = True
        '
        'chkUseETasFA
        '
        Me.chkUseETasFA.Enabled = False
        Me.chkUseETasFA.Location = New System.Drawing.Point(26, 197)
        Me.chkUseETasFA.Name = "chkUseETasFA"
        Me.chkUseETasFA.Size = New System.Drawing.Size(174, 30)
        Me.chkUseETasFA.TabIndex = 64
        Me.chkUseETasFA.Text = "Use Extrathumb if no Fanart Found"
        Me.chkUseETasFA.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(24, 170)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(168, 24)
        Me.Label17.TabIndex = 63
        Me.Label17.Text = "(If checked, Ember will use only the first half of the movie to extract thumbs)"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkNoSpoilers
        '
        Me.chkNoSpoilers.AutoSize = True
        Me.chkNoSpoilers.Enabled = False
        Me.chkNoSpoilers.Location = New System.Drawing.Point(27, 154)
        Me.chkNoSpoilers.Name = "chkNoSpoilers"
        Me.chkNoSpoilers.Size = New System.Drawing.Size(80, 17)
        Me.chkNoSpoilers.TabIndex = 62
        Me.chkNoSpoilers.Text = "No Spoilers"
        Me.chkNoSpoilers.UseVisualStyleBackColor = True
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(24, 136)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(97, 13)
        Me.Label15.TabIndex = 61
        Me.Label15.Text = "Number To Create:"
        '
        'txtAutoThumbs
        '
        Me.txtAutoThumbs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAutoThumbs.Enabled = False
        Me.txtAutoThumbs.Location = New System.Drawing.Point(124, 133)
        Me.txtAutoThumbs.Name = "txtAutoThumbs"
        Me.txtAutoThumbs.Size = New System.Drawing.Size(53, 20)
        Me.txtAutoThumbs.TabIndex = 45
        '
        'chkAutoThumbs
        '
        Me.chkAutoThumbs.Location = New System.Drawing.Point(6, 104)
        Me.chkAutoThumbs.Name = "chkAutoThumbs"
        Me.chkAutoThumbs.Size = New System.Drawing.Size(194, 31)
        Me.chkAutoThumbs.TabIndex = 44
        Me.chkAutoThumbs.Text = "Automatically Create Extrathumbs During Update"
        Me.chkAutoThumbs.UseVisualStyleBackColor = True
        '
        'chkSingleScrapeImages
        '
        Me.chkSingleScrapeImages.AutoSize = True
        Me.chkSingleScrapeImages.Location = New System.Drawing.Point(6, 67)
        Me.chkSingleScrapeImages.Name = "chkSingleScrapeImages"
        Me.chkSingleScrapeImages.Size = New System.Drawing.Size(181, 17)
        Me.chkSingleScrapeImages.TabIndex = 37
        Me.chkSingleScrapeImages.Text = "Scrape Images on Single Scrape"
        Me.chkSingleScrapeImages.UseVisualStyleBackColor = True
        '
        'chkUseMPDB
        '
        Me.chkUseMPDB.AutoSize = True
        Me.chkUseMPDB.Location = New System.Drawing.Point(6, 51)
        Me.chkUseMPDB.Name = "chkUseMPDB"
        Me.chkUseMPDB.Size = New System.Drawing.Size(188, 17)
        Me.chkUseMPDB.TabIndex = 43
        Me.chkUseMPDB.Text = "Get Images From MoviePostersDB"
        Me.chkUseMPDB.UseVisualStyleBackColor = True
        '
        'chkUseTMDB
        '
        Me.chkUseTMDB.AutoSize = True
        Me.chkUseTMDB.Location = New System.Drawing.Point(6, 19)
        Me.chkUseTMDB.Name = "chkUseTMDB"
        Me.chkUseTMDB.Size = New System.Drawing.Size(140, 17)
        Me.chkUseTMDB.TabIndex = 39
        Me.chkUseTMDB.Text = "Get Images From TMDB"
        Me.chkUseTMDB.UseVisualStyleBackColor = True
        '
        'chkUseIMPA
        '
        Me.chkUseIMPA.AutoSize = True
        Me.chkUseIMPA.Location = New System.Drawing.Point(6, 35)
        Me.chkUseIMPA.Name = "chkUseIMPA"
        Me.chkUseIMPA.Size = New System.Drawing.Size(163, 17)
        Me.chkUseIMPA.TabIndex = 40
        Me.chkUseIMPA.Text = "Get Images From IMPAwards"
        Me.chkUseIMPA.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkUseMIStudioTag)
        Me.GroupBox1.Controls.Add(Me.chkUseMIDuration)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Controls.Add(Me.txtIMDBURL)
        Me.GroupBox1.Controls.Add(Me.chkCastWithImg)
        Me.GroupBox1.Controls.Add(Me.chkUseCertForMPAA)
        Me.GroupBox1.Controls.Add(Me.chkFullCast)
        Me.GroupBox1.Controls.Add(Me.chkFullCrew)
        Me.GroupBox1.Controls.Add(Me.cbCert)
        Me.GroupBox1.Controls.Add(Me.chkCert)
        Me.GroupBox1.Controls.Add(Me.chkScanMediaInfo)
        Me.GroupBox1.Location = New System.Drawing.Point(5, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(211, 235)
        Me.GroupBox1.TabIndex = 55
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Options"
        '
        'chkUseMIStudioTag
        '
        Me.chkUseMIStudioTag.AutoSize = True
        Me.chkUseMIStudioTag.Enabled = False
        Me.chkUseMIStudioTag.Location = New System.Drawing.Point(34, 153)
        Me.chkUseMIStudioTag.Name = "chkUseMIStudioTag"
        Me.chkUseMIStudioTag.Size = New System.Drawing.Size(170, 17)
        Me.chkUseMIStudioTag.TabIndex = 64
        Me.chkUseMIStudioTag.Text = "Store Media Info in Studio Tag"
        Me.chkUseMIStudioTag.UseVisualStyleBackColor = True
        '
        'chkUseMIDuration
        '
        Me.chkUseMIDuration.AutoSize = True
        Me.chkUseMIDuration.Enabled = False
        Me.chkUseMIDuration.Location = New System.Drawing.Point(34, 137)
        Me.chkUseMIDuration.Name = "chkUseMIDuration"
        Me.chkUseMIDuration.Size = New System.Drawing.Size(160, 17)
        Me.chkUseMIDuration.TabIndex = 63
        Me.chkUseMIDuration.Text = "Use MI Duration for Runtime"
        Me.chkUseMIDuration.UseVisualStyleBackColor = True
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(4, 180)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(66, 13)
        Me.Label18.TabIndex = 62
        Me.Label18.Text = "IMDB Mirror:"
        '
        'txtIMDBURL
        '
        Me.txtIMDBURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIMDBURL.Location = New System.Drawing.Point(6, 193)
        Me.txtIMDBURL.Name = "txtIMDBURL"
        Me.txtIMDBURL.Size = New System.Drawing.Size(192, 20)
        Me.txtIMDBURL.TabIndex = 46
        '
        'chkCastWithImg
        '
        Me.chkCastWithImg.AutoSize = True
        Me.chkCastWithImg.Location = New System.Drawing.Point(6, 31)
        Me.chkCastWithImg.Name = "chkCastWithImg"
        Me.chkCastWithImg.Size = New System.Drawing.Size(179, 17)
        Me.chkCastWithImg.TabIndex = 39
        Me.chkCastWithImg.Text = "Scrape Only Actors With Images"
        Me.chkCastWithImg.UseVisualStyleBackColor = True
        '
        'chkUseCertForMPAA
        '
        Me.chkUseCertForMPAA.AutoSize = True
        Me.chkUseCertForMPAA.Enabled = False
        Me.chkUseCertForMPAA.Location = New System.Drawing.Point(34, 104)
        Me.chkUseCertForMPAA.Name = "chkUseCertForMPAA"
        Me.chkUseCertForMPAA.Size = New System.Drawing.Size(151, 17)
        Me.chkUseCertForMPAA.TabIndex = 38
        Me.chkUseCertForMPAA.Text = "Use Certification for MPAA"
        Me.chkUseCertForMPAA.UseVisualStyleBackColor = True
        '
        'chkFullCast
        '
        Me.chkFullCast.AutoSize = True
        Me.chkFullCast.Location = New System.Drawing.Point(6, 15)
        Me.chkFullCast.Name = "chkFullCast"
        Me.chkFullCast.Size = New System.Drawing.Size(103, 17)
        Me.chkFullCast.TabIndex = 32
        Me.chkFullCast.Text = "Scrape Full Cast"
        Me.chkFullCast.UseVisualStyleBackColor = True
        '
        'chkFullCrew
        '
        Me.chkFullCrew.AutoSize = True
        Me.chkFullCrew.Location = New System.Drawing.Point(6, 47)
        Me.chkFullCrew.Name = "chkFullCrew"
        Me.chkFullCrew.Size = New System.Drawing.Size(106, 17)
        Me.chkFullCrew.TabIndex = 33
        Me.chkFullCrew.Text = "Scrape Full Crew"
        Me.chkFullCrew.UseVisualStyleBackColor = True
        '
        'cbCert
        '
        Me.cbCert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbCert.Enabled = False
        Me.cbCert.FormattingEnabled = True
        Me.cbCert.Items.AddRange(New Object() {"Argentina", "Australia", "Belgium", "Brazil", "Canada", "Finland", "France", "Germany", "Hong Kong", "Iceland", "Ireland", "Netherlands", "New Zealand", "Peru", "Portugal", "Singapore", "South Korea", "Spain", "Sweden", "Switzerland", "UK", "USA"})
        Me.cbCert.Location = New System.Drawing.Point(6, 81)
        Me.cbCert.Name = "cbCert"
        Me.cbCert.Size = New System.Drawing.Size(179, 21)
        Me.cbCert.Sorted = True
        Me.cbCert.TabIndex = 35
        '
        'chkCert
        '
        Me.chkCert.AutoSize = True
        Me.chkCert.Location = New System.Drawing.Point(6, 63)
        Me.chkCert.Name = "chkCert"
        Me.chkCert.Size = New System.Drawing.Size(157, 17)
        Me.chkCert.TabIndex = 34
        Me.chkCert.Text = "Use Certification Language:"
        Me.chkCert.UseVisualStyleBackColor = True
        '
        'chkScanMediaInfo
        '
        Me.chkScanMediaInfo.AutoSize = True
        Me.chkScanMediaInfo.Location = New System.Drawing.Point(6, 120)
        Me.chkScanMediaInfo.Name = "chkScanMediaInfo"
        Me.chkScanMediaInfo.Size = New System.Drawing.Size(104, 17)
        Me.chkScanMediaInfo.TabIndex = 36
        Me.chkScanMediaInfo.Text = "Scan Media Info"
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
        TreeNode1.ImageIndex = 4
        TreeNode1.Name = "nExts"
        TreeNode1.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode1.SelectedImageIndex = 4
        TreeNode1.Text = "File System"
        TreeNode2.ImageIndex = 1
        TreeNode2.Name = "nXBMCCom"
        TreeNode2.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode2.SelectedImageIndex = 1
        TreeNode2.Text = "XBMC Communication"
        TreeNode3.ImageIndex = 0
        TreeNode3.Name = "nGeneral"
        TreeNode3.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode3.SelectedImageIndex = 0
        TreeNode3.Text = "General"
        TreeNode4.ImageIndex = 5
        TreeNode4.Name = "nSources"
        TreeNode4.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode4.SelectedImageIndex = 5
        TreeNode4.Text = "Files and Sources"
        TreeNode5.ImageIndex = 3
        TreeNode5.Name = "nScraper"
        TreeNode5.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode5.SelectedImageIndex = 3
        TreeNode5.Text = "Scraper - Data"
        TreeNode6.ImageIndex = 6
        TreeNode6.Name = "nImages"
        TreeNode6.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode6.SelectedImageIndex = 6
        TreeNode6.Text = "Scraper - Images"
        TreeNode7.ImageIndex = 2
        TreeNode7.Name = "nMovies"
        TreeNode7.NodeFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        TreeNode7.SelectedImageIndex = 2
        TreeNode7.Text = "Movies"
        Me.tvSettings.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode3, TreeNode7})
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
        Me.pnlGeneral.Controls.Add(Me.gbColors)
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
        Me.btnRemoveCom.TabIndex = 8
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
        Me.lbXBMCCom.TabIndex = 6
        '
        'pnlMovies
        '
        Me.pnlMovies.BackColor = System.Drawing.Color.White
        Me.pnlMovies.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMovies.Controls.Add(Me.lbGenre)
        Me.pnlMovies.Controls.Add(Me.lblGenre)
        Me.pnlMovies.Controls.Add(Me.GroupBox12)
        Me.pnlMovies.Location = New System.Drawing.Point(208, 96)
        Me.pnlMovies.Name = "pnlMovies"
        Me.pnlMovies.Size = New System.Drawing.Size(597, 353)
        Me.pnlMovies.TabIndex = 61
        Me.pnlMovies.Visible = False
        '
        'lbGenre
        '
        Me.lbGenre.CheckOnClick = True
        Me.lbGenre.FormattingEnabled = True
        Me.lbGenre.IntegralHeight = False
        Me.lbGenre.Location = New System.Drawing.Point(6, 23)
        Me.lbGenre.Name = "lbGenre"
        Me.lbGenre.Size = New System.Drawing.Size(192, 159)
        Me.lbGenre.Sorted = True
        Me.lbGenre.TabIndex = 107
        '
        'lblGenre
        '
        Me.lblGenre.AutoSize = True
        Me.lblGenre.Location = New System.Drawing.Point(6, 6)
        Me.lblGenre.Name = "lblGenre"
        Me.lblGenre.Size = New System.Drawing.Size(115, 13)
        Me.lblGenre.TabIndex = 106
        Me.lblGenre.Text = "Genre Language Filter:"
        '
        'GroupBox16
        '
        Me.GroupBox16.Controls.Add(Me.chkAutoBD)
        Me.GroupBox16.Controls.Add(Me.btnBrowse)
        Me.GroupBox16.Controls.Add(Me.txtBDPath)
        Me.GroupBox16.Location = New System.Drawing.Point(5, 303)
        Me.GroupBox16.Name = "GroupBox16"
        Me.GroupBox16.Size = New System.Drawing.Size(583, 43)
        Me.GroupBox16.TabIndex = 67
        Me.GroupBox16.TabStop = False
        Me.GroupBox16.Text = "Backdrops Folder"
        '
        'chkAutoBD
        '
        Me.chkAutoBD.AutoSize = True
        Me.chkAutoBD.Location = New System.Drawing.Point(326, 18)
        Me.chkAutoBD.Name = "chkAutoBD"
        Me.chkAutoBD.Size = New System.Drawing.Size(251, 17)
        Me.chkAutoBD.TabIndex = 2
        Me.chkAutoBD.Text = "Automatically Save Fanart To Backdrops Folder"
        Me.chkAutoBD.UseVisualStyleBackColor = True
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(290, 14)
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
        Me.txtBDPath.Size = New System.Drawing.Size(281, 20)
        Me.txtBDPath.TabIndex = 0
        '
        'pnlScraper
        '
        Me.pnlScraper.BackColor = System.Drawing.Color.White
        Me.pnlScraper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlScraper.Controls.Add(Me.GroupBox20)
        Me.pnlScraper.Controls.Add(Me.GroupBox15)
        Me.pnlScraper.Controls.Add(Me.GroupBox1)
        Me.pnlScraper.Controls.Add(Me.GroupBox10)
        Me.pnlScraper.Location = New System.Drawing.Point(208, 96)
        Me.pnlScraper.Name = "pnlScraper"
        Me.pnlScraper.Size = New System.Drawing.Size(597, 353)
        Me.pnlScraper.TabIndex = 62
        Me.pnlScraper.Visible = False
        '
        'GroupBox20
        '
        Me.GroupBox20.Controls.Add(Me.chkNoDLTrailer)
        Me.GroupBox20.Controls.Add(Me.chkSingleScrapeTrailer)
        Me.GroupBox20.Controls.Add(Me.Label23)
        Me.GroupBox20.Controls.Add(Me.txtTimeout)
        Me.GroupBox20.Controls.Add(Me.chkUpdaterTrailer)
        Me.GroupBox20.Controls.Add(Me.Label22)
        Me.GroupBox20.Controls.Add(Me.lbTrailerSites)
        Me.GroupBox20.Controls.Add(Me.chkDownloadTrailer)
        Me.GroupBox20.Location = New System.Drawing.Point(222, 176)
        Me.GroupBox20.Name = "GroupBox20"
        Me.GroupBox20.Size = New System.Drawing.Size(299, 154)
        Me.GroupBox20.TabIndex = 61
        Me.GroupBox20.TabStop = False
        Me.GroupBox20.Text = "Trailers"
        '
        'chkNoDLTrailer
        '
        Me.chkNoDLTrailer.Location = New System.Drawing.Point(19, 62)
        Me.chkNoDLTrailer.Name = "chkNoDLTrailer"
        Me.chkNoDLTrailer.Size = New System.Drawing.Size(145, 30)
        Me.chkNoDLTrailer.TabIndex = 66
        Me.chkNoDLTrailer.Text = "Only Get URLs During Updaters"
        Me.chkNoDLTrailer.UseVisualStyleBackColor = True
        '
        'chkSingleScrapeTrailer
        '
        Me.chkSingleScrapeTrailer.Location = New System.Drawing.Point(19, 92)
        Me.chkSingleScrapeTrailer.Name = "chkSingleScrapeTrailer"
        Me.chkSingleScrapeTrailer.Size = New System.Drawing.Size(148, 31)
        Me.chkSingleScrapeTrailer.TabIndex = 65
        Me.chkSingleScrapeTrailer.Text = "Get Trailers During Single-Scrape"
        Me.chkSingleScrapeTrailer.UseVisualStyleBackColor = True
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(16, 126)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(48, 13)
        Me.Label23.TabIndex = 64
        Me.Label23.Text = "Timeout:"
        '
        'txtTimeout
        '
        Me.txtTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTimeout.Location = New System.Drawing.Point(70, 124)
        Me.txtTimeout.Name = "txtTimeout"
        Me.txtTimeout.Size = New System.Drawing.Size(66, 20)
        Me.txtTimeout.TabIndex = 63
        '
        'chkUpdaterTrailer
        '
        Me.chkUpdaterTrailer.Location = New System.Drawing.Point(19, 33)
        Me.chkUpdaterTrailer.Name = "chkUpdaterTrailer"
        Me.chkUpdaterTrailer.Size = New System.Drawing.Size(148, 31)
        Me.chkUpdaterTrailer.TabIndex = 3
        Me.chkUpdaterTrailer.Text = "Get Trailers During ""All Items"" Updaters"
        Me.chkUpdaterTrailer.UseVisualStyleBackColor = True
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(172, 34)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(117, 13)
        Me.Label22.TabIndex = 2
        Me.Label22.Text = "Supported Trailer Sites:"
        '
        'lbTrailerSites
        '
        Me.lbTrailerSites.CheckOnClick = True
        Me.lbTrailerSites.FormattingEnabled = True
        Me.lbTrailerSites.Items.AddRange(New Object() {"YouTube/TMDB", "AllTrailers", "MattTrailer", "AZMovies", "IMDB"})
        Me.lbTrailerSites.Location = New System.Drawing.Point(173, 51)
        Me.lbTrailerSites.Name = "lbTrailerSites"
        Me.lbTrailerSites.Size = New System.Drawing.Size(120, 79)
        Me.lbTrailerSites.TabIndex = 1
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
        Me.pnlExtensions.Controls.Add(Me.GroupBox18)
        Me.pnlExtensions.Location = New System.Drawing.Point(208, 96)
        Me.pnlExtensions.Name = "pnlExtensions"
        Me.pnlExtensions.Size = New System.Drawing.Size(597, 353)
        Me.pnlExtensions.TabIndex = 66
        Me.pnlExtensions.Visible = False
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
        Me.btnRemMovieExt.TabIndex = 5
        Me.btnRemMovieExt.UseVisualStyleBackColor = True
        '
        'btnAddMovieExt
        '
        Me.btnAddMovieExt.Image = CType(resources.GetObject("btnAddMovieExt.Image"), System.Drawing.Image)
        Me.btnAddMovieExt.Location = New System.Drawing.Point(68, 313)
        Me.btnAddMovieExt.Name = "btnAddMovieExt"
        Me.btnAddMovieExt.Size = New System.Drawing.Size(23, 23)
        Me.btnAddMovieExt.TabIndex = 4
        Me.btnAddMovieExt.UseVisualStyleBackColor = True
        '
        'txtMovieExt
        '
        Me.txtMovieExt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMovieExt.Location = New System.Drawing.Point(6, 314)
        Me.txtMovieExt.Name = "txtMovieExt"
        Me.txtMovieExt.Size = New System.Drawing.Size(61, 20)
        Me.txtMovieExt.TabIndex = 3
        '
        'lstMovieExts
        '
        Me.lstMovieExts.FormattingEnabled = True
        Me.lstMovieExts.Location = New System.Drawing.Point(6, 16)
        Me.lstMovieExts.Name = "lstMovieExts"
        Me.lstMovieExts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstMovieExts.Size = New System.Drawing.Size(180, 290)
        Me.lstMovieExts.Sorted = True
        Me.lstMovieExts.TabIndex = 2
        '
        'pnlSources
        '
        Me.pnlSources.BackColor = System.Drawing.Color.White
        Me.pnlSources.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSources.Controls.Add(Me.GroupBox19)
        Me.pnlSources.Controls.Add(Me.GroupBox16)
        Me.pnlSources.Controls.Add(Me.lvMovies)
        Me.pnlSources.Controls.Add(Me.btnMovieAddFiles)
        Me.pnlSources.Controls.Add(Me.btnMovieRem)
        Me.pnlSources.Controls.Add(Me.btnMovieAddFolder)
        Me.pnlSources.Controls.Add(Me.GroupBox8)
        Me.pnlSources.Location = New System.Drawing.Point(208, 96)
        Me.pnlSources.Name = "pnlSources"
        Me.pnlSources.Size = New System.Drawing.Size(597, 353)
        Me.pnlSources.TabIndex = 67
        Me.pnlSources.Visible = False
        '
        'GroupBox19
        '
        Me.GroupBox19.Controls.Add(Me.chkSkipStackedSizeCheck)
        Me.GroupBox19.Controls.Add(Me.Label21)
        Me.GroupBox19.Controls.Add(Me.txtSkipLessThan)
        Me.GroupBox19.Controls.Add(Me.Label20)
        Me.GroupBox19.Controls.Add(Me.chkUseFolderNames)
        Me.GroupBox19.Controls.Add(Me.chkTitleFromNfo)
        Me.GroupBox19.Controls.Add(Me.chkScanRecursive)
        Me.GroupBox19.Location = New System.Drawing.Point(9, 121)
        Me.GroupBox19.Name = "GroupBox19"
        Me.GroupBox19.Size = New System.Drawing.Size(211, 172)
        Me.GroupBox19.TabIndex = 68
        Me.GroupBox19.TabStop = False
        Me.GroupBox19.Text = "Movie Naming/Detection"
        '
        'chkSkipStackedSizeCheck
        '
        Me.chkSkipStackedSizeCheck.Location = New System.Drawing.Point(26, 141)
        Me.chkSkipStackedSizeCheck.Name = "chkSkipStackedSizeCheck"
        Me.chkSkipStackedSizeCheck.Size = New System.Drawing.Size(183, 19)
        Me.chkSkipStackedSizeCheck.TabIndex = 70
        Me.chkSkipStackedSizeCheck.Text = "Skip Size Check of Stacked Files"
        Me.chkSkipStackedSizeCheck.UseVisualStyleBackColor = True
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(132, 119)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(59, 13)
        Me.Label21.TabIndex = 69
        Me.Label21.Text = "Megabytes"
        '
        'txtSkipLessThan
        '
        Me.txtSkipLessThan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtSkipLessThan.Location = New System.Drawing.Point(26, 115)
        Me.txtSkipLessThan.Name = "txtSkipLessThan"
        Me.txtSkipLessThan.Size = New System.Drawing.Size(100, 20)
        Me.txtSkipLessThan.TabIndex = 68
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(3, 97)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(97, 13)
        Me.Label20.TabIndex = 67
        Me.Label20.Text = "Skip files less than:"
        '
        'pnlImages
        '
        Me.pnlImages.BackColor = System.Drawing.Color.White
        Me.pnlImages.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
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
        'GroupBox17
        '
        Me.GroupBox17.Controls.Add(Me.chkUseImgCacheUpdaters)
        Me.GroupBox17.Controls.Add(Me.Label19)
        Me.GroupBox17.Controls.Add(Me.chkPersistImgCache)
        Me.GroupBox17.Controls.Add(Me.chkUseImgCache)
        Me.GroupBox17.Location = New System.Drawing.Point(3, 236)
        Me.GroupBox17.Name = "GroupBox17"
        Me.GroupBox17.Size = New System.Drawing.Size(205, 109)
        Me.GroupBox17.TabIndex = 60
        Me.GroupBox17.TabStop = False
        Me.GroupBox17.Text = "Caching"
        '
        'chkUseImgCacheUpdaters
        '
        Me.chkUseImgCacheUpdaters.AutoSize = True
        Me.chkUseImgCacheUpdaters.Location = New System.Drawing.Point(24, 35)
        Me.chkUseImgCacheUpdaters.Name = "chkUseImgCacheUpdaters"
        Me.chkUseImgCacheUpdaters.Size = New System.Drawing.Size(172, 17)
        Me.chkUseImgCacheUpdaters.TabIndex = 3
        Me.chkUseImgCacheUpdaters.Text = "Use Image Cache for Updaters"
        Me.chkUseImgCacheUpdaters.UseVisualStyleBackColor = True
        '
        'Label19
        '
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(36, 70)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(158, 24)
        Me.Label19.TabIndex = 2
        Me.Label19.Text = "(When enabled, the cache will be available between sessions)"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'chkPersistImgCache
        '
        Me.chkPersistImgCache.AutoSize = True
        Me.chkPersistImgCache.Location = New System.Drawing.Point(24, 53)
        Me.chkPersistImgCache.Name = "chkPersistImgCache"
        Me.chkPersistImgCache.Size = New System.Drawing.Size(138, 17)
        Me.chkPersistImgCache.TabIndex = 1
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
        'dlgSettings
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(810, 484)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlCurrent)
        Me.Controls.Add(Me.tvSettings)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.pnlGeneral)
        Me.Controls.Add(Me.pnlImages)
        Me.Controls.Add(Me.pnlScraper)
        Me.Controls.Add(Me.pnlSources)
        Me.Controls.Add(Me.pnlMovies)
        Me.Controls.Add(Me.pnlXBMCCom)
        Me.Controls.Add(Me.pnlExtensions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ember Settings"
        Me.GroupBox11.ResumeLayout(False)
        Me.GroupBox11.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.tcCleaner.ResumeLayout(False)
        Me.tpStandard.ResumeLayout(False)
        Me.tpStandard.PerformLayout()
        Me.tpExpert.ResumeLayout(False)
        Me.tpExpert.PerformLayout()
        Me.gbColors.ResumeLayout(False)
        Me.gbColors.PerformLayout()
        Me.gbFilters.ResumeLayout(False)
        Me.gbFilters.PerformLayout()
        Me.GroupBox12.ResumeLayout(False)
        Me.GroupBox12.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox15.ResumeLayout(False)
        Me.GroupBox15.PerformLayout()
        Me.GroupBox14.ResumeLayout(False)
        Me.GroupBox14.PerformLayout()
        CType(Me.tbPosterQual, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox13.ResumeLayout(False)
        Me.GroupBox13.PerformLayout()
        CType(Me.tbFanartQual, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGeneral.ResumeLayout(False)
        Me.pnlXBMCCom.ResumeLayout(False)
        Me.pnlMovies.ResumeLayout(False)
        Me.pnlMovies.PerformLayout()
        Me.GroupBox16.ResumeLayout(False)
        Me.GroupBox16.PerformLayout()
        Me.pnlScraper.ResumeLayout(False)
        Me.GroupBox20.ResumeLayout(False)
        Me.GroupBox20.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlExtensions.ResumeLayout(False)
        Me.GroupBox18.ResumeLayout(False)
        Me.GroupBox18.PerformLayout()
        Me.pnlSources.ResumeLayout(False)
        Me.GroupBox19.ResumeLayout(False)
        Me.GroupBox19.PerformLayout()
        Me.pnlImages.ResumeLayout(False)
        Me.GroupBox17.ResumeLayout(False)
        Me.GroupBox17.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fbdBrowse As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents gbFilters As System.Windows.Forms.GroupBox
    Friend WithEvents btnAddFilter As System.Windows.Forms.Button
    Friend WithEvents txtFilter As System.Windows.Forms.TextBox
    Friend WithEvents lstFilters As System.Windows.Forms.ListBox
    Friend WithEvents btnRemoveFilter As System.Windows.Forms.Button
    Friend WithEvents cdColor As System.Windows.Forms.ColorDialog
    Friend WithEvents gbColors As System.Windows.Forms.GroupBox
    Friend WithEvents btnHeaderText As System.Windows.Forms.Button
    Friend WithEvents btnTopPanelText As System.Windows.Forms.Button
    Friend WithEvents btnInfoPanelText As System.Windows.Forms.Button
    Friend WithEvents lblInfoPanelText As System.Windows.Forms.Label
    Friend WithEvents lblPanel As System.Windows.Forms.Label
    Friend WithEvents lblHeaderText As System.Windows.Forms.Label
    Friend WithEvents lblHeader As System.Windows.Forms.Label
    Friend WithEvents btnTopPanel As System.Windows.Forms.Button
    Friend WithEvents btnBackground As System.Windows.Forms.Button
    Friend WithEvents btnInfoPanel As System.Windows.Forms.Button
    Friend WithEvents btnHeaders As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTopPanelText As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
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
    Friend WithEvents chkTitleFromNfo As System.Windows.Forms.CheckBox
    Friend WithEvents lvMovies As System.Windows.Forms.ListView
    Friend WithEvents colPath As System.Windows.Forms.ColumnHeader
    Friend WithEvents colType As System.Windows.Forms.ColumnHeader
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
    Friend WithEvents btnMovieAddFiles As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMovieTrailerCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieInfoCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieFanartCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMoviePosterCol As System.Windows.Forms.CheckBox
    Friend WithEvents btnMovieRem As System.Windows.Forms.Button
    Friend WithEvents btnMovieAddFolder As System.Windows.Forms.Button
    Friend WithEvents chkUseFolderNames As System.Windows.Forms.CheckBox
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
    Friend WithEvents chkScanRecursive As System.Windows.Forms.CheckBox
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
    Friend WithEvents chkUseMIStudioTag As System.Windows.Forms.CheckBox
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
End Class
