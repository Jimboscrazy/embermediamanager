<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgUpdateMedia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgUpdateMedia))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.cdColor = New System.Windows.Forms.ColorDialog
        Me.tmrWait = New System.Windows.Forms.Timer(Me.components)
        Me.tmrPreview = New System.Windows.Forms.Timer(Me.components)
        Me.tmrNameWait = New System.Windows.Forms.Timer(Me.components)
        Me.tmrName = New System.Windows.Forms.Timer(Me.components)
        Me.cdFont = New System.Windows.Forms.FontDialog
        Me.tmrTopWait = New System.Windows.Forms.Timer(Me.components)
        Me.tmrTop = New System.Windows.Forms.Timer(Me.components)
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog
        Me.rbUpdateModifier_All = New System.Windows.Forms.RadioButton
        Me.gbUpdateModifier = New System.Windows.Forms.GroupBox
        Me.rbUpdate_Marked = New System.Windows.Forms.RadioButton
        Me.rbUpdateModifier_New = New System.Windows.Forms.RadioButton
        Me.rbUpdateModifier_Missing = New System.Windows.Forms.RadioButton
        Me.gbUpdateType = New System.Windows.Forms.GroupBox
        Me.rbUpdate_Ask = New System.Windows.Forms.RadioButton
        Me.rbUpdate_Auto = New System.Windows.Forms.RadioButton
        Me.gbUpdateItems = New System.Windows.Forms.GroupBox
        Me.cbItems = New System.Windows.Forms.CheckedListBox
        Me.cbItems_All = New System.Windows.Forms.CheckBox
        Me.Update_Button = New System.Windows.Forms.Button
        Me.gbSettingsModifier = New System.Windows.Forms.GroupBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtAutoThumbs = New System.Windows.Forms.TextBox
        Me.chkUseETasFA = New System.Windows.Forms.CheckBox
        Me.chkCastWithImg = New System.Windows.Forms.CheckBox
        Me.chkFullCast = New System.Windows.Forms.CheckBox
        Me.chkFullCrew = New System.Windows.Forms.CheckBox
        Me.GroupBox20 = New System.Windows.Forms.GroupBox
        Me.lbTrailerSites = New System.Windows.Forms.CheckedListBox
        Me.gbMediaInfo = New System.Windows.Forms.GroupBox
        Me.chkUseMIStudioTag = New System.Windows.Forms.CheckBox
        Me.chkUseMIDuration = New System.Windows.Forms.CheckBox
        Me.GroupBox10 = New System.Windows.Forms.GroupBox
        Me.lblLocks = New System.Windows.Forms.Label
        Me.chkLockTrailer = New System.Windows.Forms.CheckBox
        Me.chkLockGenre = New System.Windows.Forms.CheckBox
        Me.chkLockRealStudio = New System.Windows.Forms.CheckBox
        Me.chkLockRating = New System.Windows.Forms.CheckBox
        Me.chkLockTagline = New System.Windows.Forms.CheckBox
        Me.chkLockTitle = New System.Windows.Forms.CheckBox
        Me.chkLockOutline = New System.Windows.Forms.CheckBox
        Me.chkLockPlot = New System.Windows.Forms.CheckBox
        Me.gbInfoBox = New System.Windows.Forms.GroupBox
        Me.lblInfoBox = New System.Windows.Forms.Label
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbUpdateModifier.SuspendLayout()
        Me.gbUpdateType.SuspendLayout()
        Me.gbUpdateItems.SuspendLayout()
        Me.gbSettingsModifier.SuspendLayout()
        Me.GroupBox20.SuspendLayout()
        Me.gbMediaInfo.SuspendLayout()
        Me.GroupBox10.SuspendLayout()
        Me.gbInfoBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.OK_Button.Location = New System.Drawing.Point(648, 330)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(80, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Close"
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
        Me.pnlTop.Size = New System.Drawing.Size(735, 64)
        Me.pnlTop.TabIndex = 58
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(64, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Update Media"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(61, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(176, 29)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Update Media"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 7)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'tmrWait
        '
        Me.tmrWait.Interval = 250
        '
        'tmrPreview
        '
        Me.tmrPreview.Interval = 250
        '
        'tmrNameWait
        '
        Me.tmrNameWait.Interval = 250
        '
        'tmrName
        '
        Me.tmrName.Interval = 250
        '
        'cdFont
        '
        Me.cdFont.Font = New System.Drawing.Font("Arial", 21.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '
        'tmrTopWait
        '
        Me.tmrTopWait.Interval = 250
        '
        'tmrTop
        '
        Me.tmrTop.Interval = 250
        '
        'rbUpdateModifier_All
        '
        Me.rbUpdateModifier_All.AutoSize = True
        Me.rbUpdateModifier_All.Checked = True
        Me.rbUpdateModifier_All.Location = New System.Drawing.Point(6, 19)
        Me.rbUpdateModifier_All.Name = "rbUpdateModifier_All"
        Me.rbUpdateModifier_All.Size = New System.Drawing.Size(74, 17)
        Me.rbUpdateModifier_All.TabIndex = 59
        Me.rbUpdateModifier_All.TabStop = True
        Me.rbUpdateModifier_All.Text = "Update All"
        Me.rbUpdateModifier_All.UseVisualStyleBackColor = True
        '
        'gbUpdateModifier
        '
        Me.gbUpdateModifier.Controls.Add(Me.rbUpdate_Marked)
        Me.gbUpdateModifier.Controls.Add(Me.rbUpdateModifier_New)
        Me.gbUpdateModifier.Controls.Add(Me.rbUpdateModifier_Missing)
        Me.gbUpdateModifier.Controls.Add(Me.rbUpdateModifier_All)
        Me.gbUpdateModifier.Location = New System.Drawing.Point(6, 70)
        Me.gbUpdateModifier.Name = "gbUpdateModifier"
        Me.gbUpdateModifier.Size = New System.Drawing.Size(390, 48)
        Me.gbUpdateModifier.TabIndex = 60
        Me.gbUpdateModifier.TabStop = False
        Me.gbUpdateModifier.Text = "Selection Filter"
        '
        'rbUpdate_Marked
        '
        Me.rbUpdate_Marked.AutoSize = True
        Me.rbUpdate_Marked.Enabled = False
        Me.rbUpdate_Marked.Location = New System.Drawing.Point(291, 19)
        Me.rbUpdate_Marked.Name = "rbUpdate_Marked"
        Me.rbUpdate_Marked.Size = New System.Drawing.Size(98, 17)
        Me.rbUpdate_Marked.TabIndex = 62
        Me.rbUpdate_Marked.Text = "Marked Movies"
        Me.rbUpdate_Marked.UseVisualStyleBackColor = True
        '
        'rbUpdateModifier_New
        '
        Me.rbUpdateModifier_New.AutoSize = True
        Me.rbUpdateModifier_New.Enabled = False
        Me.rbUpdateModifier_New.Location = New System.Drawing.Point(194, 19)
        Me.rbUpdateModifier_New.Name = "rbUpdateModifier_New"
        Me.rbUpdateModifier_New.Size = New System.Drawing.Size(84, 17)
        Me.rbUpdateModifier_New.TabIndex = 61
        Me.rbUpdateModifier_New.Text = "New Movies"
        Me.rbUpdateModifier_New.UseVisualStyleBackColor = True
        '
        'rbUpdateModifier_Missing
        '
        Me.rbUpdateModifier_Missing.AutoSize = True
        Me.rbUpdateModifier_Missing.Location = New System.Drawing.Point(93, 19)
        Me.rbUpdateModifier_Missing.Name = "rbUpdateModifier_Missing"
        Me.rbUpdateModifier_Missing.Size = New System.Drawing.Size(88, 17)
        Me.rbUpdateModifier_Missing.TabIndex = 60
        Me.rbUpdateModifier_Missing.Text = "Missing Items"
        Me.rbUpdateModifier_Missing.UseVisualStyleBackColor = True
        '
        'gbUpdateType
        '
        Me.gbUpdateType.Controls.Add(Me.rbUpdate_Ask)
        Me.gbUpdateType.Controls.Add(Me.rbUpdate_Auto)
        Me.gbUpdateType.Location = New System.Drawing.Point(6, 124)
        Me.gbUpdateType.Name = "gbUpdateType"
        Me.gbUpdateType.Size = New System.Drawing.Size(390, 49)
        Me.gbUpdateType.TabIndex = 61
        Me.gbUpdateType.TabStop = False
        Me.gbUpdateType.Text = "Update Mode"
        '
        'rbUpdate_Ask
        '
        Me.rbUpdate_Ask.AutoSize = True
        Me.rbUpdate_Ask.Checked = True
        Me.rbUpdate_Ask.Location = New System.Drawing.Point(178, 18)
        Me.rbUpdate_Ask.Name = "rbUpdate_Ask"
        Me.rbUpdate_Ask.Size = New System.Drawing.Size(205, 17)
        Me.rbUpdate_Ask.TabIndex = 64
        Me.rbUpdate_Ask.TabStop = True
        Me.rbUpdate_Ask.Text = "Ask (Require Input If No Exact Match)"
        Me.rbUpdate_Ask.UseVisualStyleBackColor = True
        '
        'rbUpdate_Auto
        '
        Me.rbUpdate_Auto.AutoSize = True
        Me.rbUpdate_Auto.Location = New System.Drawing.Point(6, 18)
        Me.rbUpdate_Auto.Name = "rbUpdate_Auto"
        Me.rbUpdate_Auto.Size = New System.Drawing.Size(165, 17)
        Me.rbUpdate_Auto.TabIndex = 63
        Me.rbUpdate_Auto.Text = "Automatic (Force Best Match)"
        Me.rbUpdate_Auto.UseVisualStyleBackColor = True
        '
        'gbUpdateItems
        '
        Me.gbUpdateItems.Controls.Add(Me.cbItems)
        Me.gbUpdateItems.Controls.Add(Me.cbItems_All)
        Me.gbUpdateItems.Location = New System.Drawing.Point(7, 179)
        Me.gbUpdateItems.Name = "gbUpdateItems"
        Me.gbUpdateItems.Size = New System.Drawing.Size(147, 145)
        Me.gbUpdateItems.TabIndex = 62
        Me.gbUpdateItems.TabStop = False
        Me.gbUpdateItems.Text = "What to Update"
        '
        'cbItems
        '
        Me.cbItems.BackColor = System.Drawing.SystemColors.Control
        Me.cbItems.FormattingEnabled = True
        Me.cbItems.Items.AddRange(New Object() {"NFO", "Poster", "Fanart", "Extrathumbs", "Media Info", "Trailers"})
        Me.cbItems.Location = New System.Drawing.Point(3, 42)
        Me.cbItems.Name = "cbItems"
        Me.cbItems.Size = New System.Drawing.Size(138, 94)
        Me.cbItems.TabIndex = 65
        '
        'cbItems_All
        '
        Me.cbItems_All.AutoSize = True
        Me.cbItems_All.Location = New System.Drawing.Point(6, 22)
        Me.cbItems_All.Name = "cbItems_All"
        Me.cbItems_All.Size = New System.Drawing.Size(65, 17)
        Me.cbItems_All.TabIndex = 0
        Me.cbItems_All.Text = "All Items"
        Me.cbItems_All.UseVisualStyleBackColor = True
        '
        'Update_Button
        '
        Me.Update_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Update_Button.Enabled = False
        Me.Update_Button.Location = New System.Drawing.Point(557, 330)
        Me.Update_Button.Name = "Update_Button"
        Me.Update_Button.Size = New System.Drawing.Size(80, 23)
        Me.Update_Button.TabIndex = 63
        Me.Update_Button.Text = "Update"
        '
        'gbSettingsModifier
        '
        Me.gbSettingsModifier.Controls.Add(Me.Label15)
        Me.gbSettingsModifier.Controls.Add(Me.txtAutoThumbs)
        Me.gbSettingsModifier.Controls.Add(Me.chkUseETasFA)
        Me.gbSettingsModifier.Controls.Add(Me.chkCastWithImg)
        Me.gbSettingsModifier.Controls.Add(Me.chkFullCast)
        Me.gbSettingsModifier.Controls.Add(Me.chkFullCrew)
        Me.gbSettingsModifier.Controls.Add(Me.GroupBox20)
        Me.gbSettingsModifier.Controls.Add(Me.gbMediaInfo)
        Me.gbSettingsModifier.Controls.Add(Me.GroupBox10)
        Me.gbSettingsModifier.Location = New System.Drawing.Point(405, 70)
        Me.gbSettingsModifier.Name = "gbSettingsModifier"
        Me.gbSettingsModifier.Size = New System.Drawing.Size(324, 254)
        Me.gbSettingsModifier.TabIndex = 64
        Me.gbSettingsModifier.TabStop = False
        Me.gbSettingsModifier.Text = "Setting Overrides"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(50, 191)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(75, 13)
        Me.Label15.TabIndex = 68
        Me.Label15.Text = "# Extrathumbs"
        '
        'txtAutoThumbs
        '
        Me.txtAutoThumbs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtAutoThumbs.Location = New System.Drawing.Point(12, 188)
        Me.txtAutoThumbs.Name = "txtAutoThumbs"
        Me.txtAutoThumbs.Size = New System.Drawing.Size(35, 20)
        Me.txtAutoThumbs.TabIndex = 67
        '
        'chkUseETasFA
        '
        Me.chkUseETasFA.Location = New System.Drawing.Point(12, 215)
        Me.chkUseETasFA.Name = "chkUseETasFA"
        Me.chkUseETasFA.Size = New System.Drawing.Size(126, 30)
        Me.chkUseETasFA.TabIndex = 66
        Me.chkUseETasFA.Text = "Use Extrathumb if no Fanart Found"
        Me.chkUseETasFA.UseVisualStyleBackColor = True
        '
        'chkCastWithImg
        '
        Me.chkCastWithImg.AutoSize = True
        Me.chkCastWithImg.Location = New System.Drawing.Point(144, 100)
        Me.chkCastWithImg.Name = "chkCastWithImg"
        Me.chkCastWithImg.Size = New System.Drawing.Size(179, 17)
        Me.chkCastWithImg.TabIndex = 65
        Me.chkCastWithImg.Text = "Scrape Only Actors With Images"
        Me.chkCastWithImg.UseVisualStyleBackColor = True
        '
        'chkFullCast
        '
        Me.chkFullCast.AutoSize = True
        Me.chkFullCast.Location = New System.Drawing.Point(144, 84)
        Me.chkFullCast.Name = "chkFullCast"
        Me.chkFullCast.Size = New System.Drawing.Size(103, 17)
        Me.chkFullCast.TabIndex = 63
        Me.chkFullCast.Text = "Scrape Full Cast"
        Me.chkFullCast.UseVisualStyleBackColor = True
        '
        'chkFullCrew
        '
        Me.chkFullCrew.AutoSize = True
        Me.chkFullCrew.Location = New System.Drawing.Point(144, 116)
        Me.chkFullCrew.Name = "chkFullCrew"
        Me.chkFullCrew.Size = New System.Drawing.Size(106, 17)
        Me.chkFullCrew.TabIndex = 64
        Me.chkFullCrew.Text = "Scrape Full Crew"
        Me.chkFullCrew.UseVisualStyleBackColor = True
        '
        'GroupBox20
        '
        Me.GroupBox20.Controls.Add(Me.lbTrailerSites)
        Me.GroupBox20.Location = New System.Drawing.Point(144, 137)
        Me.GroupBox20.Name = "GroupBox20"
        Me.GroupBox20.Size = New System.Drawing.Size(174, 106)
        Me.GroupBox20.TabIndex = 62
        Me.GroupBox20.TabStop = False
        Me.GroupBox20.Text = "Supported Trailer Sites:"
        '
        'lbTrailerSites
        '
        Me.lbTrailerSites.BackColor = System.Drawing.SystemColors.Control
        Me.lbTrailerSites.CheckOnClick = True
        Me.lbTrailerSites.FormattingEnabled = True
        Me.lbTrailerSites.Items.AddRange(New Object() {"YouTube/TMDB", "AllTrailers", "MattTrailer", "AZMovies", "IMDB"})
        Me.lbTrailerSites.Location = New System.Drawing.Point(8, 19)
        Me.lbTrailerSites.Name = "lbTrailerSites"
        Me.lbTrailerSites.Size = New System.Drawing.Size(159, 79)
        Me.lbTrailerSites.TabIndex = 1
        '
        'gbMediaInfo
        '
        Me.gbMediaInfo.Controls.Add(Me.chkUseMIStudioTag)
        Me.gbMediaInfo.Controls.Add(Me.chkUseMIDuration)
        Me.gbMediaInfo.Location = New System.Drawing.Point(144, 19)
        Me.gbMediaInfo.Name = "gbMediaInfo"
        Me.gbMediaInfo.Size = New System.Drawing.Size(174, 58)
        Me.gbMediaInfo.TabIndex = 59
        Me.gbMediaInfo.TabStop = False
        Me.gbMediaInfo.Text = "Media Info"
        '
        'chkUseMIStudioTag
        '
        Me.chkUseMIStudioTag.AutoSize = True
        Me.chkUseMIStudioTag.Location = New System.Drawing.Point(7, 36)
        Me.chkUseMIStudioTag.Name = "chkUseMIStudioTag"
        Me.chkUseMIStudioTag.Size = New System.Drawing.Size(153, 17)
        Me.chkUseMIStudioTag.TabIndex = 67
        Me.chkUseMIStudioTag.Text = "Use Media Info Studio Tag"
        Me.chkUseMIStudioTag.UseVisualStyleBackColor = True
        '
        'chkUseMIDuration
        '
        Me.chkUseMIDuration.AutoSize = True
        Me.chkUseMIDuration.Location = New System.Drawing.Point(7, 18)
        Me.chkUseMIDuration.Name = "chkUseMIDuration"
        Me.chkUseMIDuration.Size = New System.Drawing.Size(160, 17)
        Me.chkUseMIDuration.TabIndex = 66
        Me.chkUseMIDuration.Text = "Use MI Duration for Runtime"
        Me.chkUseMIDuration.UseVisualStyleBackColor = True
        '
        'GroupBox10
        '
        Me.GroupBox10.Controls.Add(Me.lblLocks)
        Me.GroupBox10.Controls.Add(Me.chkLockTrailer)
        Me.GroupBox10.Controls.Add(Me.chkLockGenre)
        Me.GroupBox10.Controls.Add(Me.chkLockRealStudio)
        Me.GroupBox10.Controls.Add(Me.chkLockRating)
        Me.GroupBox10.Controls.Add(Me.chkLockTagline)
        Me.GroupBox10.Controls.Add(Me.chkLockTitle)
        Me.GroupBox10.Controls.Add(Me.chkLockOutline)
        Me.GroupBox10.Controls.Add(Me.chkLockPlot)
        Me.GroupBox10.Location = New System.Drawing.Point(6, 19)
        Me.GroupBox10.Name = "GroupBox10"
        Me.GroupBox10.Size = New System.Drawing.Size(132, 161)
        Me.GroupBox10.TabIndex = 58
        Me.GroupBox10.TabStop = False
        Me.GroupBox10.Text = " Locks"
        '
        'lblLocks
        '
        Me.lblLocks.AutoSize = True
        Me.lblLocks.Location = New System.Drawing.Point(6, 15)
        Me.lblLocks.Name = "lblLocks"
        Me.lblLocks.Size = New System.Drawing.Size(113, 13)
        Me.lblLocks.TabIndex = 66
        Me.lblLocks.Text = "(Do not allow updates)"
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
        'gbInfoBox
        '
        Me.gbInfoBox.Controls.Add(Me.lblInfoBox)
        Me.gbInfoBox.Location = New System.Drawing.Point(160, 179)
        Me.gbInfoBox.Name = "gbInfoBox"
        Me.gbInfoBox.Size = New System.Drawing.Size(236, 145)
        Me.gbInfoBox.TabIndex = 65
        Me.gbInfoBox.TabStop = False
        Me.gbInfoBox.Text = "Info"
        '
        'lblInfoBox
        '
        Me.lblInfoBox.Location = New System.Drawing.Point(6, 16)
        Me.lblInfoBox.Name = "lblInfoBox"
        Me.lblInfoBox.Size = New System.Drawing.Size(224, 120)
        Me.lblInfoBox.TabIndex = 0
        Me.lblInfoBox.Text = "Some Messages Here about selection"
        '
        'dlgUpdateMedia
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(735, 354)
        Me.Controls.Add(Me.gbInfoBox)
        Me.Controls.Add(Me.gbSettingsModifier)
        Me.Controls.Add(Me.Update_Button)
        Me.Controls.Add(Me.gbUpdateItems)
        Me.Controls.Add(Me.gbUpdateType)
        Me.Controls.Add(Me.gbUpdateModifier)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.OK_Button)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgUpdateMedia"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Update Media"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbUpdateModifier.ResumeLayout(False)
        Me.gbUpdateModifier.PerformLayout()
        Me.gbUpdateType.ResumeLayout(False)
        Me.gbUpdateType.PerformLayout()
        Me.gbUpdateItems.ResumeLayout(False)
        Me.gbUpdateItems.PerformLayout()
        Me.gbSettingsModifier.ResumeLayout(False)
        Me.gbSettingsModifier.PerformLayout()
        Me.GroupBox20.ResumeLayout(False)
        Me.gbMediaInfo.ResumeLayout(False)
        Me.gbMediaInfo.PerformLayout()
        Me.GroupBox10.ResumeLayout(False)
        Me.GroupBox10.PerformLayout()
        Me.gbInfoBox.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents cdColor As System.Windows.Forms.ColorDialog
    Friend WithEvents tmrWait As System.Windows.Forms.Timer
    Friend WithEvents tmrPreview As System.Windows.Forms.Timer
    Friend WithEvents tmrNameWait As System.Windows.Forms.Timer
    Friend WithEvents tmrName As System.Windows.Forms.Timer
    Friend WithEvents cdFont As System.Windows.Forms.FontDialog
    Friend WithEvents tmrTopWait As System.Windows.Forms.Timer
    Friend WithEvents tmrTop As System.Windows.Forms.Timer
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents rbUpdateModifier_All As System.Windows.Forms.RadioButton
    Friend WithEvents gbUpdateModifier As System.Windows.Forms.GroupBox
    Friend WithEvents rbUpdateModifier_Missing As System.Windows.Forms.RadioButton
    Friend WithEvents rbUpdateModifier_New As System.Windows.Forms.RadioButton
    Friend WithEvents rbUpdate_Marked As System.Windows.Forms.RadioButton
    Friend WithEvents gbUpdateType As System.Windows.Forms.GroupBox
    Friend WithEvents rbUpdate_Auto As System.Windows.Forms.RadioButton
    Friend WithEvents rbUpdate_Ask As System.Windows.Forms.RadioButton
    Friend WithEvents gbUpdateItems As System.Windows.Forms.GroupBox
    Friend WithEvents cbItems_All As System.Windows.Forms.CheckBox
    Friend WithEvents Update_Button As System.Windows.Forms.Button
    Friend WithEvents gbSettingsModifier As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox10 As System.Windows.Forms.GroupBox
    Friend WithEvents chkLockTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockGenre As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockRealStudio As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockRating As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockTagline As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockTitle As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockOutline As System.Windows.Forms.CheckBox
    Friend WithEvents chkLockPlot As System.Windows.Forms.CheckBox
    Friend WithEvents gbMediaInfo As System.Windows.Forms.GroupBox
    Friend WithEvents chkUseMIStudioTag As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseMIDuration As System.Windows.Forms.CheckBox
    Friend WithEvents cbItems As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox20 As System.Windows.Forms.GroupBox
    Friend WithEvents lbTrailerSites As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkCastWithImg As System.Windows.Forms.CheckBox
    Friend WithEvents chkFullCast As System.Windows.Forms.CheckBox
    Friend WithEvents chkFullCrew As System.Windows.Forms.CheckBox
    Friend WithEvents gbInfoBox As System.Windows.Forms.GroupBox
    Friend WithEvents lblInfoBox As System.Windows.Forms.Label
    Friend WithEvents lblLocks As System.Windows.Forms.Label
    Friend WithEvents chkUseETasFA As System.Windows.Forms.CheckBox
    Friend WithEvents txtAutoThumbs As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label

End Class
