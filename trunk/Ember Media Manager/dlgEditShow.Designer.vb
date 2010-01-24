<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgEditShow
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgEditShow))
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.lbGenre = New System.Windows.Forms.CheckedListBox
        Me.btnStudio = New System.Windows.Forms.Button
        Me.lblStudio = New System.Windows.Forms.Label
        Me.txtStudio = New System.Windows.Forms.TextBox
        Me.lblMPAADesc = New System.Windows.Forms.Label
        Me.txtMPAADesc = New System.Windows.Forms.TextBox
        Me.btnEditActor = New System.Windows.Forms.Button
        Me.btnAddActor = New System.Windows.Forms.Button
        Me.btnManual = New System.Windows.Forms.Button
        Me.btnRemove = New System.Windows.Forms.Button
        Me.lblActors = New System.Windows.Forms.Label
        Me.lvActors = New System.Windows.Forms.ListView
        Me.colName = New System.Windows.Forms.ColumnHeader
        Me.colRole = New System.Windows.Forms.ColumnHeader
        Me.colThumb = New System.Windows.Forms.ColumnHeader
        Me.lbMPAA = New System.Windows.Forms.ListBox
        Me.lblGenre = New System.Windows.Forms.Label
        Me.lblMPAA = New System.Windows.Forms.Label
        Me.lblPlot = New System.Windows.Forms.Label
        Me.txtPlot = New System.Windows.Forms.TextBox
        Me.pbStar5 = New System.Windows.Forms.PictureBox
        Me.pbStar4 = New System.Windows.Forms.PictureBox
        Me.pbStar3 = New System.Windows.Forms.PictureBox
        Me.pbStar2 = New System.Windows.Forms.PictureBox
        Me.pbStar1 = New System.Windows.Forms.PictureBox
        Me.lblRating = New System.Windows.Forms.Label
        Me.mtxtYear = New System.Windows.Forms.MaskedTextBox
        Me.lblYear = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.btnSetPosterDL = New System.Windows.Forms.Button
        Me.btnRemovePoster = New System.Windows.Forms.Button
        Me.lblPosterSize = New System.Windows.Forms.Label
        Me.btnSetPosterScrape = New System.Windows.Forms.Button
        Me.btnSetPoster = New System.Windows.Forms.Button
        Me.pbPoster = New System.Windows.Forms.PictureBox
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.btnSetFanartDL = New System.Windows.Forms.Button
        Me.btnRemoveFanart = New System.Windows.Forms.Button
        Me.lblFanartSize = New System.Windows.Forms.Label
        Me.btnSetFanartScrape = New System.Windows.Forms.Button
        Me.btnSetFanart = New System.Windows.Forms.Button
        Me.pbFanart = New System.Windows.Forms.PictureBox
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.OK_Button = New System.Windows.Forms.Button
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.pbStar5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStar1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.pbPoster, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.pbFanart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Controls.Add(Me.PictureBox1)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(854, 64)
        Me.pnlTop.TabIndex = 57
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(61, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(183, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Edit the details for the selected show."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(58, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(131, 29)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Edit Show"
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
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(4, 70)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(844, 478)
        Me.TabControl1.TabIndex = 60
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.lbGenre)
        Me.TabPage1.Controls.Add(Me.btnStudio)
        Me.TabPage1.Controls.Add(Me.lblStudio)
        Me.TabPage1.Controls.Add(Me.txtStudio)
        Me.TabPage1.Controls.Add(Me.lblMPAADesc)
        Me.TabPage1.Controls.Add(Me.txtMPAADesc)
        Me.TabPage1.Controls.Add(Me.btnEditActor)
        Me.TabPage1.Controls.Add(Me.btnAddActor)
        Me.TabPage1.Controls.Add(Me.btnManual)
        Me.TabPage1.Controls.Add(Me.btnRemove)
        Me.TabPage1.Controls.Add(Me.lblActors)
        Me.TabPage1.Controls.Add(Me.lvActors)
        Me.TabPage1.Controls.Add(Me.lbMPAA)
        Me.TabPage1.Controls.Add(Me.lblGenre)
        Me.TabPage1.Controls.Add(Me.lblMPAA)
        Me.TabPage1.Controls.Add(Me.lblPlot)
        Me.TabPage1.Controls.Add(Me.txtPlot)
        Me.TabPage1.Controls.Add(Me.pbStar5)
        Me.TabPage1.Controls.Add(Me.pbStar4)
        Me.TabPage1.Controls.Add(Me.pbStar3)
        Me.TabPage1.Controls.Add(Me.pbStar2)
        Me.TabPage1.Controls.Add(Me.pbStar1)
        Me.TabPage1.Controls.Add(Me.lblRating)
        Me.TabPage1.Controls.Add(Me.mtxtYear)
        Me.TabPage1.Controls.Add(Me.lblYear)
        Me.TabPage1.Controls.Add(Me.lblTitle)
        Me.TabPage1.Controls.Add(Me.txtTitle)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(836, 452)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Details"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'lbGenre
        '
        Me.lbGenre.CheckOnClick = True
        Me.lbGenre.FormattingEnabled = True
        Me.lbGenre.IntegralHeight = False
        Me.lbGenre.Location = New System.Drawing.Point(7, 160)
        Me.lbGenre.Name = "lbGenre"
        Me.lbGenre.Size = New System.Drawing.Size(192, 283)
        Me.lbGenre.Sorted = True
        Me.lbGenre.TabIndex = 105
        '
        'btnStudio
        '
        Me.btnStudio.Image = CType(resources.GetObject("btnStudio.Image"), System.Drawing.Image)
        Me.btnStudio.Location = New System.Drawing.Point(805, 353)
        Me.btnStudio.Name = "btnStudio"
        Me.btnStudio.Size = New System.Drawing.Size(23, 23)
        Me.btnStudio.TabIndex = 104
        Me.btnStudio.UseVisualStyleBackColor = True
        '
        'lblStudio
        '
        Me.lblStudio.AutoSize = True
        Me.lblStudio.Location = New System.Drawing.Point(635, 339)
        Me.lblStudio.Name = "lblStudio"
        Me.lblStudio.Size = New System.Drawing.Size(40, 13)
        Me.lblStudio.TabIndex = 101
        Me.lblStudio.Text = "Studio:"
        '
        'txtStudio
        '
        Me.txtStudio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtStudio.Location = New System.Drawing.Point(635, 355)
        Me.txtStudio.Name = "txtStudio"
        Me.txtStudio.Size = New System.Drawing.Size(167, 20)
        Me.txtStudio.TabIndex = 100
        '
        'lblMPAADesc
        '
        Me.lblMPAADesc.AutoSize = True
        Me.lblMPAADesc.Location = New System.Drawing.Point(632, 247)
        Me.lblMPAADesc.Name = "lblMPAADesc"
        Me.lblMPAADesc.Size = New System.Drawing.Size(130, 13)
        Me.lblMPAADesc.TabIndex = 89
        Me.lblMPAADesc.Text = "MPAA Rating Description:"
        '
        'txtMPAADesc
        '
        Me.txtMPAADesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtMPAADesc.Location = New System.Drawing.Point(635, 264)
        Me.txtMPAADesc.Multiline = True
        Me.txtMPAADesc.Name = "txtMPAADesc"
        Me.txtMPAADesc.Size = New System.Drawing.Size(193, 68)
        Me.txtMPAADesc.TabIndex = 19
        '
        'btnEditActor
        '
        Me.btnEditActor.Image = CType(resources.GetObject("btnEditActor.Image"), System.Drawing.Image)
        Me.btnEditActor.Location = New System.Drawing.Point(246, 420)
        Me.btnEditActor.Name = "btnEditActor"
        Me.btnEditActor.Size = New System.Drawing.Size(23, 23)
        Me.btnEditActor.TabIndex = 13
        Me.btnEditActor.UseVisualStyleBackColor = True
        '
        'btnAddActor
        '
        Me.btnAddActor.Image = CType(resources.GetObject("btnAddActor.Image"), System.Drawing.Image)
        Me.btnAddActor.Location = New System.Drawing.Point(217, 420)
        Me.btnAddActor.Name = "btnAddActor"
        Me.btnAddActor.Size = New System.Drawing.Size(23, 23)
        Me.btnAddActor.TabIndex = 12
        Me.btnAddActor.UseVisualStyleBackColor = True
        '
        'btnManual
        '
        Me.btnManual.Location = New System.Drawing.Point(755, 423)
        Me.btnManual.Name = "btnManual"
        Me.btnManual.Size = New System.Drawing.Size(75, 23)
        Me.btnManual.TabIndex = 20
        Me.btnManual.Text = "Manual Edit"
        Me.btnManual.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Image = CType(resources.GetObject("btnRemove.Image"), System.Drawing.Image)
        Me.btnRemove.Location = New System.Drawing.Point(602, 420)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(23, 23)
        Me.btnRemove.TabIndex = 14
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'lblActors
        '
        Me.lblActors.AutoSize = True
        Me.lblActors.Location = New System.Drawing.Point(218, 139)
        Me.lblActors.Name = "lblActors"
        Me.lblActors.Size = New System.Drawing.Size(40, 13)
        Me.lblActors.TabIndex = 83
        Me.lblActors.Text = "Actors:"
        '
        'lvActors
        '
        Me.lvActors.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colRole, Me.colThumb})
        Me.lvActors.FullRowSelect = True
        Me.lvActors.Location = New System.Drawing.Point(217, 155)
        Me.lvActors.Name = "lvActors"
        Me.lvActors.Size = New System.Drawing.Size(408, 259)
        Me.lvActors.TabIndex = 11
        Me.lvActors.UseCompatibleStateImageBehavior = False
        Me.lvActors.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "Name"
        Me.colName.Width = 110
        '
        'colRole
        '
        Me.colRole.Text = "Role"
        Me.colRole.Width = 100
        '
        'colThumb
        '
        Me.colThumb.Text = "Thumb"
        Me.colThumb.Width = 174
        '
        'lbMPAA
        '
        Me.lbMPAA.FormattingEnabled = True
        Me.lbMPAA.Location = New System.Drawing.Point(635, 155)
        Me.lbMPAA.Name = "lbMPAA"
        Me.lbMPAA.Size = New System.Drawing.Size(193, 82)
        Me.lbMPAA.TabIndex = 18
        '
        'lblGenre
        '
        Me.lblGenre.AutoSize = True
        Me.lblGenre.Location = New System.Drawing.Point(7, 144)
        Me.lblGenre.Name = "lblGenre"
        Me.lblGenre.Size = New System.Drawing.Size(39, 13)
        Me.lblGenre.TabIndex = 80
        Me.lblGenre.Text = "Genre:"
        '
        'lblMPAA
        '
        Me.lblMPAA.AutoSize = True
        Me.lblMPAA.Location = New System.Drawing.Point(632, 139)
        Me.lblMPAA.Name = "lblMPAA"
        Me.lblMPAA.Size = New System.Drawing.Size(74, 13)
        Me.lblMPAA.TabIndex = 78
        Me.lblMPAA.Text = "MPAA Rating:"
        '
        'lblPlot
        '
        Me.lblPlot.AutoSize = True
        Me.lblPlot.Location = New System.Drawing.Point(218, 7)
        Me.lblPlot.Name = "lblPlot"
        Me.lblPlot.Size = New System.Drawing.Size(28, 13)
        Me.lblPlot.TabIndex = 73
        Me.lblPlot.Text = "Plot:"
        '
        'txtPlot
        '
        Me.txtPlot.AcceptsReturn = True
        Me.txtPlot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtPlot.Location = New System.Drawing.Point(217, 26)
        Me.txtPlot.Multiline = True
        Me.txtPlot.Name = "txtPlot"
        Me.txtPlot.Size = New System.Drawing.Size(611, 108)
        Me.txtPlot.TabIndex = 10
        '
        'pbStar5
        '
        Me.pbStar5.Location = New System.Drawing.Point(103, 112)
        Me.pbStar5.Name = "pbStar5"
        Me.pbStar5.Size = New System.Drawing.Size(24, 24)
        Me.pbStar5.TabIndex = 67
        Me.pbStar5.TabStop = False
        '
        'pbStar4
        '
        Me.pbStar4.Location = New System.Drawing.Point(79, 112)
        Me.pbStar4.Name = "pbStar4"
        Me.pbStar4.Size = New System.Drawing.Size(24, 24)
        Me.pbStar4.TabIndex = 66
        Me.pbStar4.TabStop = False
        '
        'pbStar3
        '
        Me.pbStar3.Location = New System.Drawing.Point(55, 112)
        Me.pbStar3.Name = "pbStar3"
        Me.pbStar3.Size = New System.Drawing.Size(24, 24)
        Me.pbStar3.TabIndex = 65
        Me.pbStar3.TabStop = False
        '
        'pbStar2
        '
        Me.pbStar2.Location = New System.Drawing.Point(31, 112)
        Me.pbStar2.Name = "pbStar2"
        Me.pbStar2.Size = New System.Drawing.Size(24, 24)
        Me.pbStar2.TabIndex = 64
        Me.pbStar2.TabStop = False
        '
        'pbStar1
        '
        Me.pbStar1.Location = New System.Drawing.Point(7, 112)
        Me.pbStar1.Name = "pbStar1"
        Me.pbStar1.Size = New System.Drawing.Size(24, 24)
        Me.pbStar1.TabIndex = 63
        Me.pbStar1.TabStop = False
        '
        'lblRating
        '
        Me.lblRating.AutoSize = True
        Me.lblRating.Location = New System.Drawing.Point(7, 96)
        Me.lblRating.Name = "lblRating"
        Me.lblRating.Size = New System.Drawing.Size(41, 13)
        Me.lblRating.TabIndex = 60
        Me.lblRating.Text = "Rating:"
        '
        'mtxtYear
        '
        Me.mtxtYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.mtxtYear.Location = New System.Drawing.Point(7, 69)
        Me.mtxtYear.Mask = "####"
        Me.mtxtYear.Name = "mtxtYear"
        Me.mtxtYear.PromptChar = Global.Microsoft.VisualBasic.ChrW(32)
        Me.mtxtYear.Size = New System.Drawing.Size(192, 20)
        Me.mtxtYear.TabIndex = 2
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Location = New System.Drawing.Point(7, 53)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(57, 13)
        Me.lblYear.TabIndex = 58
        Me.lblYear.Text = "Premiered:"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(7, 7)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(30, 13)
        Me.lblTitle.TabIndex = 57
        Me.lblTitle.Text = "Title:"
        '
        'txtTitle
        '
        Me.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtTitle.Location = New System.Drawing.Point(7, 26)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.Size = New System.Drawing.Size(192, 20)
        Me.txtTitle.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.btnSetPosterDL)
        Me.TabPage2.Controls.Add(Me.btnRemovePoster)
        Me.TabPage2.Controls.Add(Me.lblPosterSize)
        Me.TabPage2.Controls.Add(Me.btnSetPosterScrape)
        Me.TabPage2.Controls.Add(Me.btnSetPoster)
        Me.TabPage2.Controls.Add(Me.pbPoster)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(836, 452)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Poster"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'btnSetPosterDL
        '
        Me.btnSetPosterDL.Image = CType(resources.GetObject("btnSetPosterDL.Image"), System.Drawing.Image)
        Me.btnSetPosterDL.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSetPosterDL.Location = New System.Drawing.Point(735, 180)
        Me.btnSetPosterDL.Name = "btnSetPosterDL"
        Me.btnSetPosterDL.Size = New System.Drawing.Size(96, 83)
        Me.btnSetPosterDL.TabIndex = 27
        Me.btnSetPosterDL.Text = "Change Poster (Download)"
        Me.btnSetPosterDL.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSetPosterDL.UseVisualStyleBackColor = True
        '
        'btnRemovePoster
        '
        Me.btnRemovePoster.Image = CType(resources.GetObject("btnRemovePoster.Image"), System.Drawing.Image)
        Me.btnRemovePoster.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnRemovePoster.Location = New System.Drawing.Point(735, 363)
        Me.btnRemovePoster.Name = "btnRemovePoster"
        Me.btnRemovePoster.Size = New System.Drawing.Size(96, 83)
        Me.btnRemovePoster.TabIndex = 26
        Me.btnRemovePoster.Text = "Remove Poster"
        Me.btnRemovePoster.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnRemovePoster.UseVisualStyleBackColor = True
        '
        'lblPosterSize
        '
        Me.lblPosterSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPosterSize.Location = New System.Drawing.Point(8, 8)
        Me.lblPosterSize.Name = "lblPosterSize"
        Me.lblPosterSize.Size = New System.Drawing.Size(104, 23)
        Me.lblPosterSize.TabIndex = 25
        Me.lblPosterSize.Text = "Size: (XXXXxXXXX)"
        Me.lblPosterSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblPosterSize.Visible = False
        '
        'btnSetPosterScrape
        '
        Me.btnSetPosterScrape.Image = CType(resources.GetObject("btnSetPosterScrape.Image"), System.Drawing.Image)
        Me.btnSetPosterScrape.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSetPosterScrape.Location = New System.Drawing.Point(735, 93)
        Me.btnSetPosterScrape.Name = "btnSetPosterScrape"
        Me.btnSetPosterScrape.Size = New System.Drawing.Size(96, 83)
        Me.btnSetPosterScrape.TabIndex = 24
        Me.btnSetPosterScrape.Text = "Change Poster (Scrape)"
        Me.btnSetPosterScrape.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSetPosterScrape.UseVisualStyleBackColor = True
        '
        'btnSetPoster
        '
        Me.btnSetPoster.Image = CType(resources.GetObject("btnSetPoster.Image"), System.Drawing.Image)
        Me.btnSetPoster.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSetPoster.Location = New System.Drawing.Point(735, 6)
        Me.btnSetPoster.Name = "btnSetPoster"
        Me.btnSetPoster.Size = New System.Drawing.Size(96, 83)
        Me.btnSetPoster.TabIndex = 23
        Me.btnSetPoster.Text = "Change Poster (Local)"
        Me.btnSetPoster.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSetPoster.UseVisualStyleBackColor = True
        '
        'pbPoster
        '
        Me.pbPoster.BackColor = System.Drawing.Color.DimGray
        Me.pbPoster.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbPoster.Location = New System.Drawing.Point(6, 6)
        Me.pbPoster.Name = "pbPoster"
        Me.pbPoster.Size = New System.Drawing.Size(724, 440)
        Me.pbPoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbPoster.TabIndex = 0
        Me.pbPoster.TabStop = False
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.btnSetFanartDL)
        Me.TabPage3.Controls.Add(Me.btnRemoveFanart)
        Me.TabPage3.Controls.Add(Me.lblFanartSize)
        Me.TabPage3.Controls.Add(Me.btnSetFanartScrape)
        Me.TabPage3.Controls.Add(Me.btnSetFanart)
        Me.TabPage3.Controls.Add(Me.pbFanart)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(836, 452)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Fanart"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'btnSetFanartDL
        '
        Me.btnSetFanartDL.Image = CType(resources.GetObject("btnSetFanartDL.Image"), System.Drawing.Image)
        Me.btnSetFanartDL.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSetFanartDL.Location = New System.Drawing.Point(735, 180)
        Me.btnSetFanartDL.Name = "btnSetFanartDL"
        Me.btnSetFanartDL.Size = New System.Drawing.Size(96, 83)
        Me.btnSetFanartDL.TabIndex = 29
        Me.btnSetFanartDL.Text = "Change Fanart (Download)"
        Me.btnSetFanartDL.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSetFanartDL.UseVisualStyleBackColor = True
        '
        'btnRemoveFanart
        '
        Me.btnRemoveFanart.Image = CType(resources.GetObject("btnRemoveFanart.Image"), System.Drawing.Image)
        Me.btnRemoveFanart.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnRemoveFanart.Location = New System.Drawing.Point(735, 363)
        Me.btnRemoveFanart.Name = "btnRemoveFanart"
        Me.btnRemoveFanart.Size = New System.Drawing.Size(96, 83)
        Me.btnRemoveFanart.TabIndex = 28
        Me.btnRemoveFanart.Text = "Remove Fanart"
        Me.btnRemoveFanart.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnRemoveFanart.UseVisualStyleBackColor = True
        '
        'lblFanartSize
        '
        Me.lblFanartSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblFanartSize.Location = New System.Drawing.Point(8, 8)
        Me.lblFanartSize.Name = "lblFanartSize"
        Me.lblFanartSize.Size = New System.Drawing.Size(104, 23)
        Me.lblFanartSize.TabIndex = 27
        Me.lblFanartSize.Text = "Size: (XXXXxXXXX)"
        Me.lblFanartSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblFanartSize.Visible = False
        '
        'btnSetFanartScrape
        '
        Me.btnSetFanartScrape.Image = CType(resources.GetObject("btnSetFanartScrape.Image"), System.Drawing.Image)
        Me.btnSetFanartScrape.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSetFanartScrape.Location = New System.Drawing.Point(735, 93)
        Me.btnSetFanartScrape.Name = "btnSetFanartScrape"
        Me.btnSetFanartScrape.Size = New System.Drawing.Size(96, 83)
        Me.btnSetFanartScrape.TabIndex = 26
        Me.btnSetFanartScrape.Text = "Change Fanart (Scrape)"
        Me.btnSetFanartScrape.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSetFanartScrape.UseVisualStyleBackColor = True
        '
        'btnSetFanart
        '
        Me.btnSetFanart.Image = CType(resources.GetObject("btnSetFanart.Image"), System.Drawing.Image)
        Me.btnSetFanart.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btnSetFanart.Location = New System.Drawing.Point(735, 6)
        Me.btnSetFanart.Name = "btnSetFanart"
        Me.btnSetFanart.Size = New System.Drawing.Size(96, 83)
        Me.btnSetFanart.TabIndex = 25
        Me.btnSetFanart.Text = "Change Fanart (Local)"
        Me.btnSetFanart.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnSetFanart.UseVisualStyleBackColor = True
        '
        'pbFanart
        '
        Me.pbFanart.BackColor = System.Drawing.Color.DimGray
        Me.pbFanart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbFanart.Location = New System.Drawing.Point(6, 6)
        Me.pbFanart.Name = "pbFanart"
        Me.pbFanart.Size = New System.Drawing.Size(724, 440)
        Me.pbFanart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFanart.TabIndex = 1
        Me.pbFanart.TabStop = False
        '
        'Cancel_Button
        '
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(781, 553)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 59
        Me.Cancel_Button.Text = "Cancel"
        '
        'OK_Button
        '
        Me.OK_Button.Location = New System.Drawing.Point(708, 553)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 58
        Me.OK_Button.Text = "OK"
        '
        'dlgEditShow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(854, 582)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.pnlTop)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgEditShow"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Show"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.pbStar5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.pbPoster, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.pbFanart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents lbGenre As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnStudio As System.Windows.Forms.Button
    Friend WithEvents lblStudio As System.Windows.Forms.Label
    Friend WithEvents txtStudio As System.Windows.Forms.TextBox
    Friend WithEvents lblMPAADesc As System.Windows.Forms.Label
    Friend WithEvents txtMPAADesc As System.Windows.Forms.TextBox
    Friend WithEvents btnEditActor As System.Windows.Forms.Button
    Friend WithEvents btnAddActor As System.Windows.Forms.Button
    Friend WithEvents btnManual As System.Windows.Forms.Button
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents lblActors As System.Windows.Forms.Label
    Friend WithEvents lvActors As System.Windows.Forms.ListView
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRole As System.Windows.Forms.ColumnHeader
    Friend WithEvents colThumb As System.Windows.Forms.ColumnHeader
    Friend WithEvents lbMPAA As System.Windows.Forms.ListBox
    Friend WithEvents lblGenre As System.Windows.Forms.Label
    Friend WithEvents lblMPAA As System.Windows.Forms.Label
    Friend WithEvents lblPlot As System.Windows.Forms.Label
    Friend WithEvents txtPlot As System.Windows.Forms.TextBox
    Friend WithEvents pbStar5 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar4 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar3 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar2 As System.Windows.Forms.PictureBox
    Friend WithEvents pbStar1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblRating As System.Windows.Forms.Label
    Friend WithEvents mtxtYear As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnSetPosterDL As System.Windows.Forms.Button
    Friend WithEvents btnRemovePoster As System.Windows.Forms.Button
    Friend WithEvents lblPosterSize As System.Windows.Forms.Label
    Friend WithEvents btnSetPosterScrape As System.Windows.Forms.Button
    Friend WithEvents btnSetPoster As System.Windows.Forms.Button
    Friend WithEvents pbPoster As System.Windows.Forms.PictureBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents btnSetFanartDL As System.Windows.Forms.Button
    Friend WithEvents btnRemoveFanart As System.Windows.Forms.Button
    Friend WithEvents lblFanartSize As System.Windows.Forms.Label
    Friend WithEvents btnSetFanartScrape As System.Windows.Forms.Button
    Friend WithEvents btnSetFanart As System.Windows.Forms.Button
    Friend WithEvents pbFanart As System.Windows.Forms.PictureBox
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button

End Class
