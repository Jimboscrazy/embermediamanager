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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgSettings))
        Me.fbdBrowse = New System.Windows.Forms.FolderBrowserDialog
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabGeneral = New System.Windows.Forms.TabPage
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.chkOverwriteNfo = New System.Windows.Forms.CheckBox
        Me.chkLogErrors = New System.Windows.Forms.CheckBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.chkCleanMovieNFOb = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieNFO = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieTBNb = New System.Windows.Forms.CheckBox
        Me.chkCleanMovieTBN = New System.Windows.Forms.CheckBox
        Me.chkCleanFolderJPG = New System.Windows.Forms.CheckBox
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
        Me.tabMovies = New System.Windows.Forms.TabPage
        Me.TabControl2 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
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
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.chkMovieTrailerCol = New System.Windows.Forms.CheckBox
        Me.chkMovieInfoCol = New System.Windows.Forms.CheckBox
        Me.chkMovieFanartCol = New System.Windows.Forms.CheckBox
        Me.chkMoviePosterCol = New System.Windows.Forms.CheckBox
        Me.btnMovieRem = New System.Windows.Forms.Button
        Me.btnMovieAddFolder = New System.Windows.Forms.Button
        Me.chkUseFolderNames = New System.Windows.Forms.CheckBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.chkLockTitle = New System.Windows.Forms.CheckBox
        Me.chkLockOutline = New System.Windows.Forms.CheckBox
        Me.chkLockPlot = New System.Windows.Forms.CheckBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.chkOverwriteFanart = New System.Windows.Forms.CheckBox
        Me.chkUseMPDB = New System.Windows.Forms.CheckBox
        Me.chkOverwritePoster = New System.Windows.Forms.CheckBox
        Me.chkUseTMDB = New System.Windows.Forms.CheckBox
        Me.chkUseIMPA = New System.Windows.Forms.CheckBox
        Me.cbPosterSize = New System.Windows.Forms.ComboBox
        Me.cbFanartSize = New System.Windows.Forms.ComboBox
        Me.lblPosterSize = New System.Windows.Forms.Label
        Me.lblFanartSize = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.chkFullCast = New System.Windows.Forms.CheckBox
        Me.chkFullCrew = New System.Windows.Forms.CheckBox
        Me.cbCert = New System.Windows.Forms.ComboBox
        Me.chkCert = New System.Windows.Forms.CheckBox
        Me.chkStudio = New System.Windows.Forms.CheckBox
        Me.tabShows = New System.Windows.Forms.TabPage
        Me.tabMusic = New System.Windows.Forms.TabPage
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnApply = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.cdColor = New System.Windows.Forms.ColorDialog
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.chkSingleScrapeImages = New System.Windows.Forms.CheckBox
        Me.TabControl1.SuspendLayout()
        Me.tabGeneral.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.gbColors.SuspendLayout()
        Me.gbFilters.SuspendLayout()
        Me.tabMovies.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabGeneral)
        Me.TabControl1.Controls.Add(Me.tabMovies)
        Me.TabControl1.Controls.Add(Me.tabShows)
        Me.TabControl1.Controls.Add(Me.tabMusic)
        Me.TabControl1.Location = New System.Drawing.Point(6, 70)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(603, 379)
        Me.TabControl1.TabIndex = 43
        '
        'tabGeneral
        '
        Me.tabGeneral.Controls.Add(Me.GroupBox4)
        Me.tabGeneral.Controls.Add(Me.GroupBox3)
        Me.tabGeneral.Controls.Add(Me.gbColors)
        Me.tabGeneral.Controls.Add(Me.gbFilters)
        Me.tabGeneral.Location = New System.Drawing.Point(4, 22)
        Me.tabGeneral.Name = "tabGeneral"
        Me.tabGeneral.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGeneral.Size = New System.Drawing.Size(595, 353)
        Me.tabGeneral.TabIndex = 0
        Me.tabGeneral.Text = "General"
        Me.tabGeneral.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.chkOverwriteNfo)
        Me.GroupBox4.Controls.Add(Me.chkLogErrors)
        Me.GroupBox4.Location = New System.Drawing.Point(207, 189)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(184, 126)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Miscellaneous"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(13, 62)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(165, 24)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "(If unchecked, non-conforming nfos will be renamed to <filename>.info)"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkOverwriteNfo
        '
        Me.chkOverwriteNfo.AutoSize = True
        Me.chkOverwriteNfo.Location = New System.Drawing.Point(13, 42)
        Me.chkOverwriteNfo.Name = "chkOverwriteNfo"
        Me.chkOverwriteNfo.Size = New System.Drawing.Size(172, 17)
        Me.chkOverwriteNfo.TabIndex = 14
        Me.chkOverwriteNfo.Text = "Overwrite Non-conforming nfos"
        Me.chkOverwriteNfo.UseVisualStyleBackColor = True
        '
        'chkLogErrors
        '
        Me.chkLogErrors.AutoSize = True
        Me.chkLogErrors.Location = New System.Drawing.Point(13, 19)
        Me.chkLogErrors.Name = "chkLogErrors"
        Me.chkLogErrors.Size = New System.Drawing.Size(105, 17)
        Me.chkLogErrors.TabIndex = 13
        Me.chkLogErrors.Text = "Log Errors to File"
        Me.chkLogErrors.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chkCleanMovieNFOb)
        Me.GroupBox3.Controls.Add(Me.chkCleanMovieNFO)
        Me.GroupBox3.Controls.Add(Me.chkCleanMovieFanartJPG)
        Me.GroupBox3.Controls.Add(Me.chkCleanFanartJPG)
        Me.GroupBox3.Controls.Add(Me.chkCleanMovieTBNb)
        Me.GroupBox3.Controls.Add(Me.chkCleanMovieTBN)
        Me.GroupBox3.Controls.Add(Me.chkCleanFolderJPG)
        Me.GroupBox3.Location = New System.Drawing.Point(207, 6)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(184, 177)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Clean Folders"
        '
        'chkCleanMovieNFOb
        '
        Me.chkCleanMovieNFOb.AutoSize = True
        Me.chkCleanMovieNFOb.Location = New System.Drawing.Point(13, 153)
        Me.chkCleanMovieNFOb.Name = "chkCleanMovieNFOb"
        Me.chkCleanMovieNFOb.Size = New System.Drawing.Size(89, 17)
        Me.chkCleanMovieNFOb.TabIndex = 12
        Me.chkCleanMovieNFOb.Text = "/<movie>.nfo"
        Me.chkCleanMovieNFOb.UseVisualStyleBackColor = True
        '
        'chkCleanMovieNFO
        '
        Me.chkCleanMovieNFO.AutoSize = True
        Me.chkCleanMovieNFO.Location = New System.Drawing.Point(13, 131)
        Me.chkCleanMovieNFO.Name = "chkCleanMovieNFO"
        Me.chkCleanMovieNFO.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanMovieNFO.TabIndex = 11
        Me.chkCleanMovieNFO.Text = "/movie.nfo"
        Me.chkCleanMovieNFO.UseVisualStyleBackColor = True
        '
        'chkCleanMovieFanartJPG
        '
        Me.chkCleanMovieFanartJPG.AutoSize = True
        Me.chkCleanMovieFanartJPG.Location = New System.Drawing.Point(13, 109)
        Me.chkCleanMovieFanartJPG.Name = "chkCleanMovieFanartJPG"
        Me.chkCleanMovieFanartJPG.Size = New System.Drawing.Size(118, 17)
        Me.chkCleanMovieFanartJPG.TabIndex = 10
        Me.chkCleanMovieFanartJPG.Text = "/<movie>-fanart.jpg"
        Me.chkCleanMovieFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanFanartJPG
        '
        Me.chkCleanFanartJPG.AutoSize = True
        Me.chkCleanFanartJPG.Location = New System.Drawing.Point(13, 87)
        Me.chkCleanFanartJPG.Name = "chkCleanFanartJPG"
        Me.chkCleanFanartJPG.Size = New System.Drawing.Size(75, 17)
        Me.chkCleanFanartJPG.TabIndex = 9
        Me.chkCleanFanartJPG.Text = "/fanart.jpg"
        Me.chkCleanFanartJPG.UseVisualStyleBackColor = True
        '
        'chkCleanMovieTBNb
        '
        Me.chkCleanMovieTBNb.AutoSize = True
        Me.chkCleanMovieTBNb.Location = New System.Drawing.Point(13, 65)
        Me.chkCleanMovieTBNb.Name = "chkCleanMovieTBNb"
        Me.chkCleanMovieTBNb.Size = New System.Drawing.Size(89, 17)
        Me.chkCleanMovieTBNb.TabIndex = 8
        Me.chkCleanMovieTBNb.Text = "/<movie>.tbn"
        Me.chkCleanMovieTBNb.UseVisualStyleBackColor = True
        '
        'chkCleanMovieTBN
        '
        Me.chkCleanMovieTBN.AutoSize = True
        Me.chkCleanMovieTBN.Location = New System.Drawing.Point(13, 43)
        Me.chkCleanMovieTBN.Name = "chkCleanMovieTBN"
        Me.chkCleanMovieTBN.Size = New System.Drawing.Size(77, 17)
        Me.chkCleanMovieTBN.TabIndex = 7
        Me.chkCleanMovieTBN.Text = "/movie.tbn"
        Me.chkCleanMovieTBN.UseVisualStyleBackColor = True
        '
        'chkCleanFolderJPG
        '
        Me.chkCleanFolderJPG.AutoSize = True
        Me.chkCleanFolderJPG.Location = New System.Drawing.Point(13, 21)
        Me.chkCleanFolderJPG.Name = "chkCleanFolderJPG"
        Me.chkCleanFolderJPG.Size = New System.Drawing.Size(74, 17)
        Me.chkCleanFolderJPG.TabIndex = 6
        Me.chkCleanFolderJPG.Text = "/folder.jpg"
        Me.chkCleanFolderJPG.UseVisualStyleBackColor = True
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
        Me.gbColors.Location = New System.Drawing.Point(400, 6)
        Me.gbColors.Name = "gbColors"
        Me.gbColors.Size = New System.Drawing.Size(188, 216)
        Me.gbColors.TabIndex = 2
        Me.gbColors.TabStop = False
        Me.gbColors.Text = "Colors"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 188)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 13)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Background Color"
        '
        'lblTopPanelText
        '
        Me.lblTopPanelText.AutoSize = True
        Me.lblTopPanelText.Location = New System.Drawing.Point(6, 160)
        Me.lblTopPanelText.Name = "lblTopPanelText"
        Me.lblTopPanelText.Size = New System.Drawing.Size(107, 13)
        Me.lblTopPanelText.TabIndex = 19
        Me.lblTopPanelText.Text = "Top Panel Text Color"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 132)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Top Panel Background Color"
        '
        'btnHeaderText
        '
        Me.btnHeaderText.BackColor = System.Drawing.Color.White
        Me.btnHeaderText.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnHeaderText.Location = New System.Drawing.Point(157, 99)
        Me.btnHeaderText.Name = "btnHeaderText"
        Me.btnHeaderText.Size = New System.Drawing.Size(22, 22)
        Me.btnHeaderText.TabIndex = 16
        Me.btnHeaderText.UseVisualStyleBackColor = False
        '
        'btnTopPanelText
        '
        Me.btnTopPanelText.BackColor = System.Drawing.Color.DimGray
        Me.btnTopPanelText.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnTopPanelText.Location = New System.Drawing.Point(157, 155)
        Me.btnTopPanelText.Name = "btnTopPanelText"
        Me.btnTopPanelText.Size = New System.Drawing.Size(22, 22)
        Me.btnTopPanelText.TabIndex = 18
        Me.btnTopPanelText.UseVisualStyleBackColor = False
        '
        'btnInfoPanelText
        '
        Me.btnInfoPanelText.BackColor = System.Drawing.Color.DimGray
        Me.btnInfoPanelText.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnInfoPanelText.Location = New System.Drawing.Point(157, 43)
        Me.btnInfoPanelText.Name = "btnInfoPanelText"
        Me.btnInfoPanelText.Size = New System.Drawing.Size(22, 22)
        Me.btnInfoPanelText.TabIndex = 14
        Me.btnInfoPanelText.UseVisualStyleBackColor = False
        '
        'lblInfoPanelText
        '
        Me.lblInfoPanelText.AutoSize = True
        Me.lblInfoPanelText.Location = New System.Drawing.Point(6, 48)
        Me.lblInfoPanelText.Name = "lblInfoPanelText"
        Me.lblInfoPanelText.Size = New System.Drawing.Size(106, 13)
        Me.lblInfoPanelText.TabIndex = 14
        Me.lblInfoPanelText.Text = "Info Panel Text Color"
        '
        'lblPanel
        '
        Me.lblPanel.AutoSize = True
        Me.lblPanel.Location = New System.Drawing.Point(6, 19)
        Me.lblPanel.Name = "lblPanel"
        Me.lblPanel.Size = New System.Drawing.Size(143, 13)
        Me.lblPanel.TabIndex = 13
        Me.lblPanel.Text = "Info Panel Background Color"
        '
        'lblHeaderText
        '
        Me.lblHeaderText.AutoSize = True
        Me.lblHeaderText.Location = New System.Drawing.Point(6, 104)
        Me.lblHeaderText.Name = "lblHeaderText"
        Me.lblHeaderText.Size = New System.Drawing.Size(144, 13)
        Me.lblHeaderText.TabIndex = 12
        Me.lblHeaderText.Text = "Info Panel Header Text Color"
        '
        'lblHeader
        '
        Me.lblHeader.AutoSize = True
        Me.lblHeader.Location = New System.Drawing.Point(6, 76)
        Me.lblHeader.Name = "lblHeader"
        Me.lblHeader.Size = New System.Drawing.Size(120, 13)
        Me.lblHeader.TabIndex = 11
        Me.lblHeader.Text = "Info Panel Header Color"
        '
        'btnTopPanel
        '
        Me.btnTopPanel.BackColor = System.Drawing.Color.Gainsboro
        Me.btnTopPanel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnTopPanel.Location = New System.Drawing.Point(157, 127)
        Me.btnTopPanel.Name = "btnTopPanel"
        Me.btnTopPanel.Size = New System.Drawing.Size(22, 22)
        Me.btnTopPanel.TabIndex = 17
        Me.btnTopPanel.UseVisualStyleBackColor = False
        '
        'btnBackground
        '
        Me.btnBackground.BackColor = System.Drawing.Color.Gainsboro
        Me.btnBackground.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnBackground.Location = New System.Drawing.Point(157, 183)
        Me.btnBackground.Name = "btnBackground"
        Me.btnBackground.Size = New System.Drawing.Size(22, 22)
        Me.btnBackground.TabIndex = 19
        Me.btnBackground.UseVisualStyleBackColor = False
        '
        'btnInfoPanel
        '
        Me.btnInfoPanel.BackColor = System.Drawing.Color.Gainsboro
        Me.btnInfoPanel.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnInfoPanel.Location = New System.Drawing.Point(157, 15)
        Me.btnInfoPanel.Name = "btnInfoPanel"
        Me.btnInfoPanel.Size = New System.Drawing.Size(22, 22)
        Me.btnInfoPanel.TabIndex = 13
        Me.btnInfoPanel.UseVisualStyleBackColor = False
        '
        'btnHeaders
        '
        Me.btnHeaders.BackColor = System.Drawing.Color.DimGray
        Me.btnHeaders.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btnHeaders.Location = New System.Drawing.Point(157, 71)
        Me.btnHeaders.Name = "btnHeaders"
        Me.btnHeaders.Size = New System.Drawing.Size(22, 22)
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
        Me.gbFilters.Size = New System.Drawing.Size(192, 309)
        Me.gbFilters.TabIndex = 0
        Me.gbFilters.TabStop = False
        Me.gbFilters.Text = "Folder/File Name Filters"
        '
        'btnDown
        '
        Me.btnDown.Location = New System.Drawing.Point(129, 277)
        Me.btnDown.Name = "btnDown"
        Me.btnDown.Size = New System.Drawing.Size(23, 23)
        Me.btnDown.TabIndex = 9
        Me.btnDown.Text = "v"
        Me.btnDown.UseVisualStyleBackColor = True
        '
        'btnUp
        '
        Me.btnUp.Location = New System.Drawing.Point(105, 277)
        Me.btnUp.Name = "btnUp"
        Me.btnUp.Size = New System.Drawing.Size(23, 23)
        Me.btnUp.TabIndex = 8
        Me.btnUp.Text = "^"
        Me.btnUp.UseVisualStyleBackColor = True
        '
        'chkProperCase
        '
        Me.chkProperCase.AutoSize = True
        Me.chkProperCase.Location = New System.Drawing.Point(6, 20)
        Me.chkProperCase.Name = "chkProperCase"
        Me.chkProperCase.Size = New System.Drawing.Size(172, 17)
        Me.chkProperCase.TabIndex = 7
        Me.chkProperCase.Text = "Convert Names to Proper Case"
        Me.chkProperCase.UseVisualStyleBackColor = True
        '
        'btnRemoveFilter
        '
        Me.btnRemoveFilter.Location = New System.Drawing.Point(163, 277)
        Me.btnRemoveFilter.Name = "btnRemoveFilter"
        Me.btnRemoveFilter.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveFilter.TabIndex = 5
        Me.btnRemoveFilter.Text = "-"
        Me.btnRemoveFilter.UseVisualStyleBackColor = True
        '
        'btnAddFilter
        '
        Me.btnAddFilter.Location = New System.Drawing.Point(68, 277)
        Me.btnAddFilter.Name = "btnAddFilter"
        Me.btnAddFilter.Size = New System.Drawing.Size(23, 23)
        Me.btnAddFilter.TabIndex = 4
        Me.btnAddFilter.Text = "+"
        Me.btnAddFilter.UseVisualStyleBackColor = True
        '
        'txtFilter
        '
        Me.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilter.Location = New System.Drawing.Point(6, 278)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(61, 20)
        Me.txtFilter.TabIndex = 3
        '
        'lstFilters
        '
        Me.lstFilters.FormattingEnabled = True
        Me.lstFilters.Location = New System.Drawing.Point(6, 46)
        Me.lstFilters.Name = "lstFilters"
        Me.lstFilters.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lstFilters.Size = New System.Drawing.Size(180, 225)
        Me.lstFilters.TabIndex = 2
        '
        'tabMovies
        '
        Me.tabMovies.Controls.Add(Me.TabControl2)
        Me.tabMovies.Location = New System.Drawing.Point(4, 22)
        Me.tabMovies.Name = "tabMovies"
        Me.tabMovies.Padding = New System.Windows.Forms.Padding(3)
        Me.tabMovies.Size = New System.Drawing.Size(595, 353)
        Me.tabMovies.TabIndex = 1
        Me.tabMovies.Text = "Movies"
        Me.tabMovies.UseVisualStyleBackColor = True
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage1)
        Me.TabControl2.Controls.Add(Me.TabPage2)
        Me.TabControl2.Location = New System.Drawing.Point(0, 6)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(593, 347)
        Me.TabControl2.TabIndex = 49
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox8)
        Me.TabPage1.Controls.Add(Me.chkTitleFromNfo)
        Me.TabPage1.Controls.Add(Me.lvMovies)
        Me.TabPage1.Controls.Add(Me.btnMovieAddFiles)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.btnMovieRem)
        Me.TabPage1.Controls.Add(Me.btnMovieAddFolder)
        Me.TabPage1.Controls.Add(Me.chkUseFolderNames)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(585, 321)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Main"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.GroupBox7)
        Me.GroupBox8.Controls.Add(Me.GroupBox6)
        Me.GroupBox8.Controls.Add(Me.GroupBox5)
        Me.GroupBox8.Location = New System.Drawing.Point(6, 138)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(362, 177)
        Me.GroupBox8.TabIndex = 64
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "File Naming"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.chkMovieNameNFO)
        Me.GroupBox7.Controls.Add(Me.chkMovieNFO)
        Me.GroupBox7.Location = New System.Drawing.Point(231, 110)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(117, 61)
        Me.GroupBox7.TabIndex = 65
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "NFO"
        '
        'chkMovieNameNFO
        '
        Me.chkMovieNameNFO.AutoSize = True
        Me.chkMovieNameNFO.Location = New System.Drawing.Point(6, 41)
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
        Me.GroupBox6.Location = New System.Drawing.Point(229, 18)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(117, 86)
        Me.GroupBox6.TabIndex = 64
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Fanart"
        '
        'chkMovieNameDotFanartJPG
        '
        Me.chkMovieNameDotFanartJPG.AutoSize = True
        Me.chkMovieNameDotFanartJPG.Location = New System.Drawing.Point(6, 63)
        Me.chkMovieNameDotFanartJPG.Name = "chkMovieNameDotFanartJPG"
        Me.chkMovieNameDotFanartJPG.Size = New System.Drawing.Size(113, 17)
        Me.chkMovieNameDotFanartJPG.TabIndex = 68
        Me.chkMovieNameDotFanartJPG.Text = "<movie>.fanart.jpg"
        Me.chkMovieNameDotFanartJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameFanartJPG
        '
        Me.chkMovieNameFanartJPG.AutoSize = True
        Me.chkMovieNameFanartJPG.Location = New System.Drawing.Point(6, 42)
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
        Me.GroupBox5.Location = New System.Drawing.Point(6, 18)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(199, 126)
        Me.GroupBox5.TabIndex = 61
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Posters"
        '
        'chkFolderJPG
        '
        Me.chkFolderJPG.AutoSize = True
        Me.chkFolderJPG.Location = New System.Drawing.Point(10, 97)
        Me.chkFolderJPG.Name = "chkFolderJPG"
        Me.chkFolderJPG.Size = New System.Drawing.Size(69, 17)
        Me.chkFolderJPG.TabIndex = 70
        Me.chkFolderJPG.Text = "folder.jpg"
        Me.chkFolderJPG.UseVisualStyleBackColor = True
        '
        'chkPosterJPG
        '
        Me.chkPosterJPG.AutoSize = True
        Me.chkPosterJPG.Location = New System.Drawing.Point(101, 74)
        Me.chkPosterJPG.Name = "chkPosterJPG"
        Me.chkPosterJPG.Size = New System.Drawing.Size(72, 17)
        Me.chkPosterJPG.TabIndex = 69
        Me.chkPosterJPG.Text = "poster.jpg"
        Me.chkPosterJPG.UseVisualStyleBackColor = True
        '
        'chkPosterTBN
        '
        Me.chkPosterTBN.AutoSize = True
        Me.chkPosterTBN.Location = New System.Drawing.Point(10, 74)
        Me.chkPosterTBN.Name = "chkPosterTBN"
        Me.chkPosterTBN.Size = New System.Drawing.Size(73, 17)
        Me.chkPosterTBN.TabIndex = 68
        Me.chkPosterTBN.Text = "poster.tbn"
        Me.chkPosterTBN.UseVisualStyleBackColor = True
        '
        'chkMovieNameJPG
        '
        Me.chkMovieNameJPG.AutoSize = True
        Me.chkMovieNameJPG.Location = New System.Drawing.Point(101, 51)
        Me.chkMovieNameJPG.Name = "chkMovieNameJPG"
        Me.chkMovieNameJPG.Size = New System.Drawing.Size(83, 17)
        Me.chkMovieNameJPG.TabIndex = 67
        Me.chkMovieNameJPG.Text = "<movie>.jpg"
        Me.chkMovieNameJPG.UseVisualStyleBackColor = True
        '
        'chkMovieJPG
        '
        Me.chkMovieJPG.AutoSize = True
        Me.chkMovieJPG.Location = New System.Drawing.Point(10, 51)
        Me.chkMovieJPG.Name = "chkMovieJPG"
        Me.chkMovieJPG.Size = New System.Drawing.Size(71, 17)
        Me.chkMovieJPG.TabIndex = 66
        Me.chkMovieJPG.Text = "movie.jpg"
        Me.chkMovieJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameTBN
        '
        Me.chkMovieNameTBN.AutoSize = True
        Me.chkMovieNameTBN.Location = New System.Drawing.Point(101, 28)
        Me.chkMovieNameTBN.Name = "chkMovieNameTBN"
        Me.chkMovieNameTBN.Size = New System.Drawing.Size(84, 17)
        Me.chkMovieNameTBN.TabIndex = 65
        Me.chkMovieNameTBN.Text = "<movie>.tbn"
        Me.chkMovieNameTBN.UseVisualStyleBackColor = True
        '
        'chkMovieTBN
        '
        Me.chkMovieTBN.AutoSize = True
        Me.chkMovieTBN.Location = New System.Drawing.Point(10, 28)
        Me.chkMovieTBN.Name = "chkMovieTBN"
        Me.chkMovieTBN.Size = New System.Drawing.Size(72, 17)
        Me.chkMovieTBN.TabIndex = 64
        Me.chkMovieTBN.Text = "movie.tbn"
        Me.chkMovieTBN.UseVisualStyleBackColor = True
        '
        'chkTitleFromNfo
        '
        Me.chkTitleFromNfo.AutoSize = True
        Me.chkTitleFromNfo.Location = New System.Drawing.Point(177, 115)
        Me.chkTitleFromNfo.Name = "chkTitleFromNfo"
        Me.chkTitleFromNfo.Size = New System.Drawing.Size(295, 17)
        Me.chkTitleFromNfo.TabIndex = 53
        Me.chkTitleFromNfo.Text = "Use Title From NFO if Available (Increases Loading Time)"
        Me.chkTitleFromNfo.UseVisualStyleBackColor = True
        '
        'lvMovies
        '
        Me.lvMovies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPath, Me.colType})
        Me.lvMovies.FullRowSelect = True
        Me.lvMovies.HideSelection = False
        Me.lvMovies.Location = New System.Drawing.Point(6, 6)
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
        Me.btnMovieAddFiles.Location = New System.Drawing.Point(478, 35)
        Me.btnMovieAddFiles.Name = "btnMovieAddFiles"
        Me.btnMovieAddFiles.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieAddFiles.TabIndex = 51
        Me.btnMovieAddFiles.Text = "Add Files Path"
        Me.btnMovieAddFiles.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.chkMovieTrailerCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieInfoCol)
        Me.GroupBox2.Controls.Add(Me.chkMovieFanartCol)
        Me.GroupBox2.Controls.Add(Me.chkMoviePosterCol)
        Me.GroupBox2.Location = New System.Drawing.Point(445, 138)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(137, 112)
        Me.GroupBox2.TabIndex = 54
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Media List Options"
        '
        'chkMovieTrailerCol
        '
        Me.chkMovieTrailerCol.AutoSize = True
        Me.chkMovieTrailerCol.Location = New System.Drawing.Point(6, 88)
        Me.chkMovieTrailerCol.Name = "chkMovieTrailerCol"
        Me.chkMovieTrailerCol.Size = New System.Drawing.Size(118, 17)
        Me.chkMovieTrailerCol.TabIndex = 31
        Me.chkMovieTrailerCol.Text = "Hide Trailer Column"
        Me.chkMovieTrailerCol.UseVisualStyleBackColor = True
        '
        'chkMovieInfoCol
        '
        Me.chkMovieInfoCol.AutoSize = True
        Me.chkMovieInfoCol.Location = New System.Drawing.Point(6, 65)
        Me.chkMovieInfoCol.Name = "chkMovieInfoCol"
        Me.chkMovieInfoCol.Size = New System.Drawing.Size(107, 17)
        Me.chkMovieInfoCol.TabIndex = 30
        Me.chkMovieInfoCol.Text = "Hide Info Column"
        Me.chkMovieInfoCol.UseVisualStyleBackColor = True
        '
        'chkMovieFanartCol
        '
        Me.chkMovieFanartCol.AutoSize = True
        Me.chkMovieFanartCol.Location = New System.Drawing.Point(6, 42)
        Me.chkMovieFanartCol.Name = "chkMovieFanartCol"
        Me.chkMovieFanartCol.Size = New System.Drawing.Size(119, 17)
        Me.chkMovieFanartCol.TabIndex = 29
        Me.chkMovieFanartCol.Text = "Hide Fanart Column"
        Me.chkMovieFanartCol.UseVisualStyleBackColor = True
        '
        'chkMoviePosterCol
        '
        Me.chkMoviePosterCol.AutoSize = True
        Me.chkMoviePosterCol.Location = New System.Drawing.Point(6, 19)
        Me.chkMoviePosterCol.Name = "chkMoviePosterCol"
        Me.chkMoviePosterCol.Size = New System.Drawing.Size(119, 17)
        Me.chkMoviePosterCol.TabIndex = 28
        Me.chkMoviePosterCol.Text = "Hide Poster Column"
        Me.chkMoviePosterCol.UseVisualStyleBackColor = True
        '
        'btnMovieRem
        '
        Me.btnMovieRem.Location = New System.Drawing.Point(478, 88)
        Me.btnMovieRem.Name = "btnMovieRem"
        Me.btnMovieRem.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieRem.TabIndex = 52
        Me.btnMovieRem.Text = "Remove Selected"
        Me.btnMovieRem.UseVisualStyleBackColor = True
        '
        'btnMovieAddFolder
        '
        Me.btnMovieAddFolder.Location = New System.Drawing.Point(478, 6)
        Me.btnMovieAddFolder.Name = "btnMovieAddFolder"
        Me.btnMovieAddFolder.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieAddFolder.TabIndex = 50
        Me.btnMovieAddFolder.Text = "Add Folders Path"
        Me.btnMovieAddFolder.UseVisualStyleBackColor = True
        '
        'chkUseFolderNames
        '
        Me.chkUseFolderNames.AutoSize = True
        Me.chkUseFolderNames.Location = New System.Drawing.Point(6, 115)
        Me.chkUseFolderNames.Name = "chkUseFolderNames"
        Me.chkUseFolderNames.Size = New System.Drawing.Size(146, 17)
        Me.chkUseFolderNames.TabIndex = 56
        Me.chkUseFolderNames.Text = "Use Folder Name for Title"
        Me.chkUseFolderNames.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox10)
        Me.TabPage2.Controls.Add(Me.GroupBox9)
        Me.TabPage2.Controls.Add(Me.GroupBox1)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(585, 321)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Scraper"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.chkLockTitle)
        Me.GroupBox10.Controls.Add(Me.chkLockOutline)
        Me.GroupBox10.Controls.Add(Me.chkLockPlot)
        Me.GroupBox10.Location = New System.Drawing.Point(261, 163)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(241, 81)
        Me.GroupBox10.TabIndex = 57
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = "Locks (Do not allow updates during scraping)"
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
        Me.chkLockOutline.Location = New System.Drawing.Point(6, 40)
        Me.chkLockOutline.Name = "chkLockOutline"
        Me.chkLockOutline.Size = New System.Drawing.Size(86, 17)
        Me.chkLockOutline.TabIndex = 40
        Me.chkLockOutline.Text = "Lock Outline"
        Me.chkLockOutline.UseVisualStyleBackColor = True
        '
        'chkLockPlot
        '
        Me.chkLockPlot.AutoSize = True
        Me.chkLockPlot.Location = New System.Drawing.Point(6, 17)
        Me.chkLockPlot.Name = "chkLockPlot"
        Me.chkLockPlot.Size = New System.Drawing.Size(71, 17)
        Me.chkLockPlot.TabIndex = 39
        Me.chkLockPlot.Text = "Lock Plot"
        Me.chkLockPlot.UseVisualStyleBackColor = True
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.chkOverwriteFanart)
        Me.GroupBox9.Controls.Add(Me.chkUseMPDB)
        Me.GroupBox9.Controls.Add(Me.chkOverwritePoster)
        Me.GroupBox9.Controls.Add(Me.chkUseTMDB)
        Me.GroupBox9.Controls.Add(Me.chkUseIMPA)
        Me.GroupBox9.Controls.Add(Me.cbPosterSize)
        Me.GroupBox9.Controls.Add(Me.cbFanartSize)
        Me.GroupBox9.Controls.Add(Me.lblPosterSize)
        Me.GroupBox9.Controls.Add(Me.lblFanartSize)
        Me.GroupBox9.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(249, 240)
        Me.GroupBox9.TabIndex = 56
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Images"
        '
        'chkOverwriteFanart
        '
        Me.chkOverwriteFanart.AutoSize = True
        Me.chkOverwriteFanart.Location = New System.Drawing.Point(15, 212)
        Me.chkOverwriteFanart.Name = "chkOverwriteFanart"
        Me.chkOverwriteFanart.Size = New System.Drawing.Size(143, 17)
        Me.chkOverwriteFanart.TabIndex = 38
        Me.chkOverwriteFanart.Text = "Overwrite Existing Fanart"
        Me.chkOverwriteFanart.UseVisualStyleBackColor = True
        '
        'chkUseMPDB
        '
        Me.chkUseMPDB.AutoSize = True
        Me.chkUseMPDB.Location = New System.Drawing.Point(15, 65)
        Me.chkUseMPDB.Name = "chkUseMPDB"
        Me.chkUseMPDB.Size = New System.Drawing.Size(188, 17)
        Me.chkUseMPDB.TabIndex = 43
        Me.chkUseMPDB.Text = "Get Images From MoviePostersDB"
        Me.chkUseMPDB.UseVisualStyleBackColor = True
        '
        'chkOverwritePoster
        '
        Me.chkOverwritePoster.AutoSize = True
        Me.chkOverwritePoster.Location = New System.Drawing.Point(15, 190)
        Me.chkOverwritePoster.Name = "chkOverwritePoster"
        Me.chkOverwritePoster.Size = New System.Drawing.Size(143, 17)
        Me.chkOverwritePoster.TabIndex = 37
        Me.chkOverwritePoster.Text = "Overwrite Existing Poster"
        Me.chkOverwritePoster.UseVisualStyleBackColor = True
        '
        'chkUseTMDB
        '
        Me.chkUseTMDB.AutoSize = True
        Me.chkUseTMDB.Location = New System.Drawing.Point(15, 19)
        Me.chkUseTMDB.Name = "chkUseTMDB"
        Me.chkUseTMDB.Size = New System.Drawing.Size(140, 17)
        Me.chkUseTMDB.TabIndex = 39
        Me.chkUseTMDB.Text = "Get Images From TMDB"
        Me.chkUseTMDB.UseVisualStyleBackColor = True
        '
        'chkUseIMPA
        '
        Me.chkUseIMPA.AutoSize = True
        Me.chkUseIMPA.Location = New System.Drawing.Point(15, 42)
        Me.chkUseIMPA.Name = "chkUseIMPA"
        Me.chkUseIMPA.Size = New System.Drawing.Size(163, 17)
        Me.chkUseIMPA.TabIndex = 40
        Me.chkUseIMPA.Text = "Get Images From IMPAwards"
        Me.chkUseIMPA.UseVisualStyleBackColor = True
        '
        'cbPosterSize
        '
        Me.cbPosterSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPosterSize.FormattingEnabled = True
        Me.cbPosterSize.Items.AddRange(New Object() {"X-Large", "Large", "Medium", "Small", "Wide"})
        Me.cbPosterSize.Location = New System.Drawing.Point(15, 105)
        Me.cbPosterSize.Name = "cbPosterSize"
        Me.cbPosterSize.Size = New System.Drawing.Size(179, 21)
        Me.cbPosterSize.TabIndex = 41
        '
        'cbFanartSize
        '
        Me.cbFanartSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbFanartSize.FormattingEnabled = True
        Me.cbFanartSize.Items.AddRange(New Object() {"Large", "Medium", "Small"})
        Me.cbFanartSize.Location = New System.Drawing.Point(15, 151)
        Me.cbFanartSize.Name = "cbFanartSize"
        Me.cbFanartSize.Size = New System.Drawing.Size(179, 21)
        Me.cbFanartSize.TabIndex = 42
        '
        'lblPosterSize
        '
        Me.lblPosterSize.AutoSize = True
        Me.lblPosterSize.Location = New System.Drawing.Point(12, 88)
        Me.lblPosterSize.Name = "lblPosterSize"
        Me.lblPosterSize.Size = New System.Drawing.Size(106, 13)
        Me.lblPosterSize.TabIndex = 14
        Me.lblPosterSize.Text = "Preferred Poster Size"
        '
        'lblFanartSize
        '
        Me.lblFanartSize.AutoSize = True
        Me.lblFanartSize.Location = New System.Drawing.Point(12, 135)
        Me.lblFanartSize.Name = "lblFanartSize"
        Me.lblFanartSize.Size = New System.Drawing.Size(106, 13)
        Me.lblFanartSize.TabIndex = 15
        Me.lblFanartSize.Text = "Preferred Fanart Size"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.chkSingleScrapeImages)
        Me.GroupBox1.Controls.Add(Me.chkFullCast)
        Me.GroupBox1.Controls.Add(Me.chkFullCrew)
        Me.GroupBox1.Controls.Add(Me.cbCert)
        Me.GroupBox1.Controls.Add(Me.chkCert)
        Me.GroupBox1.Controls.Add(Me.chkStudio)
        Me.GroupBox1.Location = New System.Drawing.Point(261, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(241, 151)
        Me.GroupBox1.TabIndex = 55
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Options"
        '
        'chkFullCast
        '
        Me.chkFullCast.AutoSize = True
        Me.chkFullCast.Location = New System.Drawing.Point(6, 19)
        Me.chkFullCast.Name = "chkFullCast"
        Me.chkFullCast.Size = New System.Drawing.Size(103, 17)
        Me.chkFullCast.TabIndex = 32
        Me.chkFullCast.Text = "Scrape Full Cast"
        Me.chkFullCast.UseVisualStyleBackColor = True
        '
        'chkFullCrew
        '
        Me.chkFullCrew.AutoSize = True
        Me.chkFullCrew.Location = New System.Drawing.Point(6, 42)
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
        Me.cbCert.Location = New System.Drawing.Point(6, 86)
        Me.cbCert.Name = "cbCert"
        Me.cbCert.Size = New System.Drawing.Size(179, 21)
        Me.cbCert.Sorted = True
        Me.cbCert.TabIndex = 35
        '
        'chkCert
        '
        Me.chkCert.AutoSize = True
        Me.chkCert.Location = New System.Drawing.Point(6, 65)
        Me.chkCert.Name = "chkCert"
        Me.chkCert.Size = New System.Drawing.Size(157, 17)
        Me.chkCert.TabIndex = 34
        Me.chkCert.Text = "Use Certification Language:"
        Me.chkCert.UseVisualStyleBackColor = True
        '
        'chkStudio
        '
        Me.chkStudio.AutoSize = True
        Me.chkStudio.Location = New System.Drawing.Point(6, 113)
        Me.chkStudio.Name = "chkStudio"
        Me.chkStudio.Size = New System.Drawing.Size(105, 17)
        Me.chkStudio.TabIndex = 36
        Me.chkStudio.Text = "Use Studio Tags"
        Me.chkStudio.UseVisualStyleBackColor = True
        '
        'tabShows
        '
        Me.tabShows.Location = New System.Drawing.Point(4, 22)
        Me.tabShows.Name = "tabShows"
        Me.tabShows.Padding = New System.Windows.Forms.Padding(3)
        Me.tabShows.Size = New System.Drawing.Size(595, 353)
        Me.tabShows.TabIndex = 2
        Me.tabShows.Text = "Shows"
        Me.tabShows.UseVisualStyleBackColor = True
        '
        'tabMusic
        '
        Me.tabMusic.Location = New System.Drawing.Point(4, 22)
        Me.tabMusic.Name = "tabMusic"
        Me.tabMusic.Size = New System.Drawing.Size(595, 353)
        Me.tabMusic.TabIndex = 3
        Me.tabMusic.Text = "Music"
        Me.tabMusic.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(534, 451)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 22
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Enabled = False
        Me.btnApply.Location = New System.Drawing.Point(371, 451)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(75, 23)
        Me.btnApply.TabIndex = 20
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(453, 451)
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
        Me.pnlTop.Size = New System.Drawing.Size(615, 64)
        Me.pnlTop.TabIndex = 57
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(61, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(219, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Configure UMM's appearance and operation."
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
        'chkSingleScrapeImages
        '
        Me.chkSingleScrapeImages.AutoSize = True
        Me.chkSingleScrapeImages.Location = New System.Drawing.Point(6, 133)
        Me.chkSingleScrapeImages.Name = "chkSingleScrapeImages"
        Me.chkSingleScrapeImages.Size = New System.Drawing.Size(181, 17)
        Me.chkSingleScrapeImages.TabIndex = 37
        Me.chkSingleScrapeImages.Text = "Scrape Images on Single Scrape"
        Me.chkSingleScrapeImages.UseVisualStyleBackColor = True
        '
        'dlgSettings
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(615, 477)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgSettings"
        Me.Text = "UMM Settings"
        Me.TabControl1.ResumeLayout(False)
        Me.tabGeneral.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.gbColors.ResumeLayout(False)
        Me.gbColors.PerformLayout()
        Me.gbFilters.ResumeLayout(False)
        Me.gbFilters.PerformLayout()
        Me.tabMovies.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox9.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fbdBrowse As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabGeneral As System.Windows.Forms.TabPage
    Friend WithEvents tabMovies As System.Windows.Forms.TabPage
    Friend WithEvents tabShows As System.Windows.Forms.TabPage
    Friend WithEvents tabMusic As System.Windows.Forms.TabPage
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
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
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
    Friend WithEvents chkStudio As System.Windows.Forms.CheckBox
    Friend WithEvents btnMovieAddFiles As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMovieTrailerCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieInfoCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieFanartCol As System.Windows.Forms.CheckBox
    Friend WithEvents chkMoviePosterCol As System.Windows.Forms.CheckBox
    Friend WithEvents btnMovieRem As System.Windows.Forms.Button
    Friend WithEvents btnMovieAddFolder As System.Windows.Forms.Button
    Friend WithEvents chkUseFolderNames As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
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
End Class
