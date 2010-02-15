<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgModuleSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgModuleSettings))
        Me.cdColor = New System.Windows.Forms.ColorDialog
        Me.ilSettings = New System.Windows.Forms.ImageList(Me.components)
        Me.fbdBrowse = New System.Windows.Forms.FolderBrowserDialog
        Me.ToolTips = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnOK = New System.Windows.Forms.Button
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.lstModules = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabGenreic = New System.Windows.Forms.TabPage
        Me.btnGenericDown = New System.Windows.Forms.Button
        Me.btnGenericUp = New System.Windows.Forms.Button
        Me.btnGenericSetup = New System.Windows.Forms.Button
        Me.btnGenericDisable = New System.Windows.Forms.Button
        Me.btnGenericEnable = New System.Windows.Forms.Button
        Me.tabScraper = New System.Windows.Forms.TabPage
        Me.btnPostScraperDown = New System.Windows.Forms.Button
        Me.btnPostScraperUp = New System.Windows.Forms.Button
        Me.btnPostScraperSetup = New System.Windows.Forms.Button
        Me.btnPostScraperDisable = New System.Windows.Forms.Button
        Me.btnPostScraperEnable = New System.Windows.Forms.Button
        Me.lstPostScrapers = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.btnScraperDown = New System.Windows.Forms.Button
        Me.btnScraperUp = New System.Windows.Forms.Button
        Me.btnScraperSetup = New System.Windows.Forms.Button
        Me.btnScraperDisable = New System.Windows.Forms.Button
        Me.btnScraperEnable = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lstScrapers = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Button11 = New System.Windows.Forms.Button
        Me.Button12 = New System.Windows.Forms.Button
        Me.Button13 = New System.Windows.Forms.Button
        Me.Button14 = New System.Windows.Forms.Button
        Me.Button15 = New System.Windows.Forms.Button
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader11 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader12 = New System.Windows.Forms.ColumnHeader
        Me.Button16 = New System.Windows.Forms.Button
        Me.Button17 = New System.Windows.Forms.Button
        Me.Button18 = New System.Windows.Forms.Button
        Me.Button19 = New System.Windows.Forms.Button
        Me.Button20 = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.ListView2 = New System.Windows.Forms.ListView
        Me.ColumnHeader13 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader14 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader8 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader9 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTop.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tabGenreic.SuspendLayout()
        Me.tabScraper.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.ilSettings.Images.SetKeyName(7, "television.ico")
        '
        'fbdBrowse
        '
        Me.fbdBrowse.Description = "Select the folder where you wish to store your backdrops."
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(471, 405)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 23)
        Me.btnOK.TabIndex = 22
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(58, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(202, 32)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Module Settings"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(61, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(200, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Configure Ember's Modules Settings."
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
        Me.pnlTop.Size = New System.Drawing.Size(545, 64)
        Me.pnlTop.TabIndex = 57
        '
        'lstModules
        '
        Me.lstModules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lstModules.FullRowSelect = True
        Me.lstModules.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstModules.HideSelection = False
        Me.lstModules.Location = New System.Drawing.Point(3, 0)
        Me.lstModules.Name = "lstModules"
        Me.lstModules.Size = New System.Drawing.Size(501, 308)
        Me.lstModules.TabIndex = 58
        Me.lstModules.UseCompatibleStateImageBehavior = False
        Me.lstModules.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Module"
        Me.ColumnHeader1.Width = 390
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Status"
        Me.ColumnHeader2.Width = 85
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabGenreic)
        Me.TabControl1.Controls.Add(Me.tabScraper)
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(0, 63)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(546, 340)
        Me.TabControl1.TabIndex = 61
        '
        'tabGenreic
        '
        Me.tabGenreic.Controls.Add(Me.btnGenericDown)
        Me.tabGenreic.Controls.Add(Me.btnGenericUp)
        Me.tabGenreic.Controls.Add(Me.btnGenericSetup)
        Me.tabGenreic.Controls.Add(Me.btnGenericDisable)
        Me.tabGenreic.Controls.Add(Me.btnGenericEnable)
        Me.tabGenreic.Controls.Add(Me.lstModules)
        Me.tabGenreic.Location = New System.Drawing.Point(4, 22)
        Me.tabGenreic.Name = "tabGenreic"
        Me.tabGenreic.Padding = New System.Windows.Forms.Padding(3)
        Me.tabGenreic.Size = New System.Drawing.Size(538, 314)
        Me.tabGenreic.TabIndex = 0
        Me.tabGenreic.Text = "Generic"
        Me.tabGenreic.UseVisualStyleBackColor = True
        '
        'btnGenericDown
        '
        Me.btnGenericDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenericDown.Enabled = False
        Me.btnGenericDown.Image = CType(resources.GetObject("btnGenericDown.Image"), System.Drawing.Image)
        Me.btnGenericDown.Location = New System.Drawing.Point(509, 138)
        Me.btnGenericDown.Name = "btnGenericDown"
        Me.btnGenericDown.Size = New System.Drawing.Size(23, 23)
        Me.btnGenericDown.TabIndex = 70
        Me.btnGenericDown.UseVisualStyleBackColor = True
        '
        'btnGenericUp
        '
        Me.btnGenericUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenericUp.Enabled = False
        Me.btnGenericUp.Image = CType(resources.GetObject("btnGenericUp.Image"), System.Drawing.Image)
        Me.btnGenericUp.Location = New System.Drawing.Point(509, 109)
        Me.btnGenericUp.Name = "btnGenericUp"
        Me.btnGenericUp.Size = New System.Drawing.Size(23, 23)
        Me.btnGenericUp.TabIndex = 69
        Me.btnGenericUp.UseVisualStyleBackColor = True
        '
        'btnGenericSetup
        '
        Me.btnGenericSetup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenericSetup.Enabled = False
        Me.btnGenericSetup.Image = CType(resources.GetObject("btnGenericSetup.Image"), System.Drawing.Image)
        Me.btnGenericSetup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenericSetup.Location = New System.Drawing.Point(510, 80)
        Me.btnGenericSetup.Name = "btnGenericSetup"
        Me.btnGenericSetup.Size = New System.Drawing.Size(23, 23)
        Me.btnGenericSetup.TabIndex = 67
        Me.btnGenericSetup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenericSetup.UseVisualStyleBackColor = True
        '
        'btnGenericDisable
        '
        Me.btnGenericDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenericDisable.Enabled = False
        Me.btnGenericDisable.Image = CType(resources.GetObject("btnGenericDisable.Image"), System.Drawing.Image)
        Me.btnGenericDisable.Location = New System.Drawing.Point(510, 51)
        Me.btnGenericDisable.Name = "btnGenericDisable"
        Me.btnGenericDisable.Size = New System.Drawing.Size(23, 23)
        Me.btnGenericDisable.TabIndex = 68
        Me.btnGenericDisable.UseVisualStyleBackColor = True
        '
        'btnGenericEnable
        '
        Me.btnGenericEnable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenericEnable.Enabled = False
        Me.btnGenericEnable.Image = CType(resources.GetObject("btnGenericEnable.Image"), System.Drawing.Image)
        Me.btnGenericEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGenericEnable.Location = New System.Drawing.Point(510, 22)
        Me.btnGenericEnable.Name = "btnGenericEnable"
        Me.btnGenericEnable.Size = New System.Drawing.Size(23, 23)
        Me.btnGenericEnable.TabIndex = 66
        Me.btnGenericEnable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGenericEnable.UseVisualStyleBackColor = True
        '
        'tabScraper
        '
        Me.tabScraper.Controls.Add(Me.btnPostScraperDown)
        Me.tabScraper.Controls.Add(Me.btnPostScraperUp)
        Me.tabScraper.Controls.Add(Me.btnPostScraperSetup)
        Me.tabScraper.Controls.Add(Me.btnPostScraperDisable)
        Me.tabScraper.Controls.Add(Me.btnPostScraperEnable)
        Me.tabScraper.Controls.Add(Me.lstPostScrapers)
        Me.tabScraper.Controls.Add(Me.btnScraperDown)
        Me.tabScraper.Controls.Add(Me.btnScraperUp)
        Me.tabScraper.Controls.Add(Me.btnScraperSetup)
        Me.tabScraper.Controls.Add(Me.btnScraperDisable)
        Me.tabScraper.Controls.Add(Me.btnScraperEnable)
        Me.tabScraper.Controls.Add(Me.Label3)
        Me.tabScraper.Controls.Add(Me.Label1)
        Me.tabScraper.Controls.Add(Me.lstScrapers)
        Me.tabScraper.Location = New System.Drawing.Point(4, 22)
        Me.tabScraper.Name = "tabScraper"
        Me.tabScraper.Padding = New System.Windows.Forms.Padding(3)
        Me.tabScraper.Size = New System.Drawing.Size(538, 314)
        Me.tabScraper.TabIndex = 1
        Me.tabScraper.Text = "Movies Scrapers"
        Me.tabScraper.UseVisualStyleBackColor = True
        '
        'btnPostScraperDown
        '
        Me.btnPostScraperDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPostScraperDown.Enabled = False
        Me.btnPostScraperDown.Image = CType(resources.GetObject("btnPostScraperDown.Image"), System.Drawing.Image)
        Me.btnPostScraperDown.Location = New System.Drawing.Point(511, 284)
        Me.btnPostScraperDown.Name = "btnPostScraperDown"
        Me.btnPostScraperDown.Size = New System.Drawing.Size(23, 23)
        Me.btnPostScraperDown.TabIndex = 81
        Me.btnPostScraperDown.UseVisualStyleBackColor = True
        '
        'btnPostScraperUp
        '
        Me.btnPostScraperUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPostScraperUp.Enabled = False
        Me.btnPostScraperUp.Image = CType(resources.GetObject("btnPostScraperUp.Image"), System.Drawing.Image)
        Me.btnPostScraperUp.Location = New System.Drawing.Point(511, 258)
        Me.btnPostScraperUp.Name = "btnPostScraperUp"
        Me.btnPostScraperUp.Size = New System.Drawing.Size(23, 23)
        Me.btnPostScraperUp.TabIndex = 80
        Me.btnPostScraperUp.UseVisualStyleBackColor = True
        '
        'btnPostScraperSetup
        '
        Me.btnPostScraperSetup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPostScraperSetup.Enabled = False
        Me.btnPostScraperSetup.Image = CType(resources.GetObject("btnPostScraperSetup.Image"), System.Drawing.Image)
        Me.btnPostScraperSetup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPostScraperSetup.Location = New System.Drawing.Point(511, 233)
        Me.btnPostScraperSetup.Name = "btnPostScraperSetup"
        Me.btnPostScraperSetup.Size = New System.Drawing.Size(23, 23)
        Me.btnPostScraperSetup.TabIndex = 78
        Me.btnPostScraperSetup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPostScraperSetup.UseVisualStyleBackColor = True
        '
        'btnPostScraperDisable
        '
        Me.btnPostScraperDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPostScraperDisable.Enabled = False
        Me.btnPostScraperDisable.Image = CType(resources.GetObject("btnPostScraperDisable.Image"), System.Drawing.Image)
        Me.btnPostScraperDisable.Location = New System.Drawing.Point(511, 208)
        Me.btnPostScraperDisable.Name = "btnPostScraperDisable"
        Me.btnPostScraperDisable.Size = New System.Drawing.Size(23, 23)
        Me.btnPostScraperDisable.TabIndex = 79
        Me.btnPostScraperDisable.UseVisualStyleBackColor = True
        '
        'btnPostScraperEnable
        '
        Me.btnPostScraperEnable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPostScraperEnable.Enabled = False
        Me.btnPostScraperEnable.Image = CType(resources.GetObject("btnPostScraperEnable.Image"), System.Drawing.Image)
        Me.btnPostScraperEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPostScraperEnable.Location = New System.Drawing.Point(511, 183)
        Me.btnPostScraperEnable.Name = "btnPostScraperEnable"
        Me.btnPostScraperEnable.Size = New System.Drawing.Size(23, 23)
        Me.btnPostScraperEnable.TabIndex = 77
        Me.btnPostScraperEnable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPostScraperEnable.UseVisualStyleBackColor = True
        '
        'lstPostScrapers
        '
        Me.lstPostScrapers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5, Me.ColumnHeader6})
        Me.lstPostScrapers.FullRowSelect = True
        Me.lstPostScrapers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstPostScrapers.HideSelection = False
        Me.lstPostScrapers.Location = New System.Drawing.Point(4, 182)
        Me.lstPostScrapers.Name = "lstPostScrapers"
        Me.lstPostScrapers.Size = New System.Drawing.Size(501, 126)
        Me.lstPostScrapers.TabIndex = 76
        Me.lstPostScrapers.UseCompatibleStateImageBehavior = False
        Me.lstPostScrapers.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Module"
        Me.ColumnHeader5.Width = 399
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Status"
        Me.ColumnHeader6.Width = 73
        '
        'btnScraperDown
        '
        Me.btnScraperDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnScraperDown.Enabled = False
        Me.btnScraperDown.Image = CType(resources.GetObject("btnScraperDown.Image"), System.Drawing.Image)
        Me.btnScraperDown.Location = New System.Drawing.Point(509, 120)
        Me.btnScraperDown.Name = "btnScraperDown"
        Me.btnScraperDown.Size = New System.Drawing.Size(23, 23)
        Me.btnScraperDown.TabIndex = 75
        Me.btnScraperDown.UseVisualStyleBackColor = True
        '
        'btnScraperUp
        '
        Me.btnScraperUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnScraperUp.Enabled = False
        Me.btnScraperUp.Image = CType(resources.GetObject("btnScraperUp.Image"), System.Drawing.Image)
        Me.btnScraperUp.Location = New System.Drawing.Point(509, 94)
        Me.btnScraperUp.Name = "btnScraperUp"
        Me.btnScraperUp.Size = New System.Drawing.Size(23, 23)
        Me.btnScraperUp.TabIndex = 74
        Me.btnScraperUp.UseVisualStyleBackColor = True
        '
        'btnScraperSetup
        '
        Me.btnScraperSetup.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnScraperSetup.Enabled = False
        Me.btnScraperSetup.Image = CType(resources.GetObject("btnScraperSetup.Image"), System.Drawing.Image)
        Me.btnScraperSetup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnScraperSetup.Location = New System.Drawing.Point(509, 69)
        Me.btnScraperSetup.Name = "btnScraperSetup"
        Me.btnScraperSetup.Size = New System.Drawing.Size(23, 23)
        Me.btnScraperSetup.TabIndex = 72
        Me.btnScraperSetup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnScraperSetup.UseVisualStyleBackColor = True
        '
        'btnScraperDisable
        '
        Me.btnScraperDisable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnScraperDisable.Enabled = False
        Me.btnScraperDisable.Image = CType(resources.GetObject("btnScraperDisable.Image"), System.Drawing.Image)
        Me.btnScraperDisable.Location = New System.Drawing.Point(509, 44)
        Me.btnScraperDisable.Name = "btnScraperDisable"
        Me.btnScraperDisable.Size = New System.Drawing.Size(23, 23)
        Me.btnScraperDisable.TabIndex = 73
        Me.btnScraperDisable.UseVisualStyleBackColor = True
        '
        'btnScraperEnable
        '
        Me.btnScraperEnable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnScraperEnable.Enabled = False
        Me.btnScraperEnable.Image = CType(resources.GetObject("btnScraperEnable.Image"), System.Drawing.Image)
        Me.btnScraperEnable.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnScraperEnable.Location = New System.Drawing.Point(509, 19)
        Me.btnScraperEnable.Name = "btnScraperEnable"
        Me.btnScraperEnable.Size = New System.Drawing.Size(23, 23)
        Me.btnScraperEnable.TabIndex = 71
        Me.btnScraperEnable.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnScraperEnable.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 166)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(73, 13)
        Me.Label3.TabIndex = 62
        Me.Label3.Text = "Post Scrapers"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(1, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 61
        Me.Label1.Text = "Scrapers"
        '
        'lstScrapers
        '
        Me.lstScrapers.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lstScrapers.FullRowSelect = True
        Me.lstScrapers.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lstScrapers.HideSelection = False
        Me.lstScrapers.Location = New System.Drawing.Point(2, 18)
        Me.lstScrapers.Name = "lstScrapers"
        Me.lstScrapers.Size = New System.Drawing.Size(501, 126)
        Me.lstScrapers.TabIndex = 59
        Me.lstScrapers.UseCompatibleStateImageBehavior = False
        Me.lstScrapers.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Module"
        Me.ColumnHeader3.Width = 399
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Status"
        Me.ColumnHeader4.Width = 73
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Button11)
        Me.TabPage1.Controls.Add(Me.Button12)
        Me.TabPage1.Controls.Add(Me.Button13)
        Me.TabPage1.Controls.Add(Me.Button14)
        Me.TabPage1.Controls.Add(Me.Button15)
        Me.TabPage1.Controls.Add(Me.ListView1)
        Me.TabPage1.Controls.Add(Me.Button16)
        Me.TabPage1.Controls.Add(Me.Button17)
        Me.TabPage1.Controls.Add(Me.Button18)
        Me.TabPage1.Controls.Add(Me.Button19)
        Me.TabPage1.Controls.Add(Me.Button20)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.ListView2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Size = New System.Drawing.Size(538, 314)
        Me.TabPage1.TabIndex = 2
        Me.TabPage1.Text = "TV Scrapers"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Button11
        '
        Me.Button11.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button11.Enabled = False
        Me.Button11.Image = CType(resources.GetObject("Button11.Image"), System.Drawing.Image)
        Me.Button11.Location = New System.Drawing.Point(511, 284)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(23, 23)
        Me.Button11.TabIndex = 95
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button12
        '
        Me.Button12.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button12.Enabled = False
        Me.Button12.Image = CType(resources.GetObject("Button12.Image"), System.Drawing.Image)
        Me.Button12.Location = New System.Drawing.Point(511, 258)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(23, 23)
        Me.Button12.TabIndex = 94
        Me.Button12.UseVisualStyleBackColor = True
        '
        'Button13
        '
        Me.Button13.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button13.Enabled = False
        Me.Button13.Image = CType(resources.GetObject("Button13.Image"), System.Drawing.Image)
        Me.Button13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button13.Location = New System.Drawing.Point(511, 233)
        Me.Button13.Name = "Button13"
        Me.Button13.Size = New System.Drawing.Size(23, 23)
        Me.Button13.TabIndex = 92
        Me.Button13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button13.UseVisualStyleBackColor = True
        '
        'Button14
        '
        Me.Button14.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button14.Enabled = False
        Me.Button14.Image = CType(resources.GetObject("Button14.Image"), System.Drawing.Image)
        Me.Button14.Location = New System.Drawing.Point(511, 208)
        Me.Button14.Name = "Button14"
        Me.Button14.Size = New System.Drawing.Size(23, 23)
        Me.Button14.TabIndex = 93
        Me.Button14.UseVisualStyleBackColor = True
        '
        'Button15
        '
        Me.Button15.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button15.Enabled = False
        Me.Button15.Image = CType(resources.GetObject("Button15.Image"), System.Drawing.Image)
        Me.Button15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button15.Location = New System.Drawing.Point(511, 183)
        Me.Button15.Name = "Button15"
        Me.Button15.Size = New System.Drawing.Size(23, 23)
        Me.Button15.TabIndex = 91
        Me.Button15.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button15.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader11, Me.ColumnHeader12})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView1.HideSelection = False
        Me.ListView1.Location = New System.Drawing.Point(4, 182)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(501, 126)
        Me.ListView1.TabIndex = 90
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader11
        '
        Me.ColumnHeader11.Text = "Module"
        Me.ColumnHeader11.Width = 399
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Status"
        Me.ColumnHeader12.Width = 73
        '
        'Button16
        '
        Me.Button16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button16.Enabled = False
        Me.Button16.Image = CType(resources.GetObject("Button16.Image"), System.Drawing.Image)
        Me.Button16.Location = New System.Drawing.Point(509, 120)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(23, 23)
        Me.Button16.TabIndex = 89
        Me.Button16.UseVisualStyleBackColor = True
        '
        'Button17
        '
        Me.Button17.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button17.Enabled = False
        Me.Button17.Image = CType(resources.GetObject("Button17.Image"), System.Drawing.Image)
        Me.Button17.Location = New System.Drawing.Point(509, 94)
        Me.Button17.Name = "Button17"
        Me.Button17.Size = New System.Drawing.Size(23, 23)
        Me.Button17.TabIndex = 88
        Me.Button17.UseVisualStyleBackColor = True
        '
        'Button18
        '
        Me.Button18.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button18.Enabled = False
        Me.Button18.Image = CType(resources.GetObject("Button18.Image"), System.Drawing.Image)
        Me.Button18.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button18.Location = New System.Drawing.Point(509, 69)
        Me.Button18.Name = "Button18"
        Me.Button18.Size = New System.Drawing.Size(23, 23)
        Me.Button18.TabIndex = 86
        Me.Button18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button18.UseVisualStyleBackColor = True
        '
        'Button19
        '
        Me.Button19.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button19.Enabled = False
        Me.Button19.Image = CType(resources.GetObject("Button19.Image"), System.Drawing.Image)
        Me.Button19.Location = New System.Drawing.Point(509, 44)
        Me.Button19.Name = "Button19"
        Me.Button19.Size = New System.Drawing.Size(23, 23)
        Me.Button19.TabIndex = 87
        Me.Button19.UseVisualStyleBackColor = True
        '
        'Button20
        '
        Me.Button20.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Button20.Enabled = False
        Me.Button20.Image = CType(resources.GetObject("Button20.Image"), System.Drawing.Image)
        Me.Button20.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button20.Location = New System.Drawing.Point(509, 19)
        Me.Button20.Name = "Button20"
        Me.Button20.Size = New System.Drawing.Size(23, 23)
        Me.Button20.TabIndex = 85
        Me.Button20.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button20.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 166)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 13)
        Me.Label5.TabIndex = 84
        Me.Label5.Text = "Post Scrapers"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(1, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 83
        Me.Label6.Text = "Scrapers"
        '
        'ListView2
        '
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader13, Me.ColumnHeader14})
        Me.ListView2.FullRowSelect = True
        Me.ListView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView2.HideSelection = False
        Me.ListView2.Location = New System.Drawing.Point(2, 18)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(501, 126)
        Me.ListView2.TabIndex = 82
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Module"
        Me.ColumnHeader13.Width = 399
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "Status"
        Me.ColumnHeader14.Width = 73
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Module"
        Me.ColumnHeader7.Width = 399
        '
        'ColumnHeader8
        '
        Me.ColumnHeader8.Text = "Status"
        Me.ColumnHeader8.Width = 73
        '
        'ColumnHeader9
        '
        Me.ColumnHeader9.Text = "Module"
        Me.ColumnHeader9.Width = 399
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Status"
        Me.ColumnHeader10.Width = 73
        '
        'dlgModuleSettings
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(545, 433)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.btnOK)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgModuleSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Module Settings"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.tabGenreic.ResumeLayout(False)
        Me.tabScraper.ResumeLayout(False)
        Me.tabScraper.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cdColor As System.Windows.Forms.ColorDialog
    Friend WithEvents ilSettings As System.Windows.Forms.ImageList
    Friend WithEvents fbdBrowse As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ToolTips As System.Windows.Forms.ToolTip
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents lstModules As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabGenreic As System.Windows.Forms.TabPage
    Friend WithEvents tabScraper As System.Windows.Forms.TabPage
    Friend WithEvents lstScrapers As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnGenericDown As System.Windows.Forms.Button
    Friend WithEvents btnGenericUp As System.Windows.Forms.Button
    Friend WithEvents btnGenericSetup As System.Windows.Forms.Button
    Friend WithEvents btnGenericDisable As System.Windows.Forms.Button
    Friend WithEvents btnGenericEnable As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnPostScraperDown As System.Windows.Forms.Button
    Friend WithEvents btnPostScraperUp As System.Windows.Forms.Button
    Friend WithEvents btnPostScraperSetup As System.Windows.Forms.Button
    Friend WithEvents btnPostScraperDisable As System.Windows.Forms.Button
    Friend WithEvents btnPostScraperEnable As System.Windows.Forms.Button
    Friend WithEvents lstPostScrapers As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnScraperDown As System.Windows.Forms.Button
    Friend WithEvents btnScraperUp As System.Windows.Forms.Button
    Friend WithEvents btnScraperSetup As System.Windows.Forms.Button
    Friend WithEvents btnScraperDisable As System.Windows.Forms.Button
    Friend WithEvents btnScraperEnable As System.Windows.Forms.Button
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button12 As System.Windows.Forms.Button
    Friend WithEvents Button13 As System.Windows.Forms.Button
    Friend WithEvents Button14 As System.Windows.Forms.Button
    Friend WithEvents Button15 As System.Windows.Forms.Button
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader11 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button16 As System.Windows.Forms.Button
    Friend WithEvents Button17 As System.Windows.Forms.Button
    Friend WithEvents Button18 As System.Windows.Forms.Button
    Friend WithEvents Button19 As System.Windows.Forms.Button
    Friend WithEvents Button20 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader8 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader9 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
End Class
