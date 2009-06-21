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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgUpdateMedia))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.rbUpdateModifier_All = New System.Windows.Forms.RadioButton
        Me.gbUpdateModifier = New System.Windows.Forms.GroupBox
        Me.rbUpdateModifier_Marked = New System.Windows.Forms.RadioButton
        Me.rbUpdateModifier_New = New System.Windows.Forms.RadioButton
        Me.rbUpdateModifier_Missing = New System.Windows.Forms.RadioButton
        Me.gbUpdateType = New System.Windows.Forms.GroupBox
        Me.rbUpdate_Ask = New System.Windows.Forms.RadioButton
        Me.rbUpdate_Auto = New System.Windows.Forms.RadioButton
        Me.gbUpdateItems = New System.Windows.Forms.GroupBox
        Me.rbMediaInfo = New System.Windows.Forms.RadioButton
        Me.rbTrailer = New System.Windows.Forms.RadioButton
        Me.rbExtra = New System.Windows.Forms.RadioButton
        Me.rbFanart = New System.Windows.Forms.RadioButton
        Me.rbPoster = New System.Windows.Forms.RadioButton
        Me.rbNfo = New System.Windows.Forms.RadioButton
        Me.rbAll = New System.Windows.Forms.RadioButton
        Me.Update_Button = New System.Windows.Forms.Button
        Me.gbInfoBox = New System.Windows.Forms.GroupBox
        Me.lblInfoBox = New System.Windows.Forms.Label
        Me.gbOptions = New System.Windows.Forms.GroupBox
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
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbUpdateModifier.SuspendLayout()
        Me.gbUpdateType.SuspendLayout()
        Me.gbUpdateItems.SuspendLayout()
        Me.gbInfoBox.SuspendLayout()
        Me.gbOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.OK_Button.Location = New System.Drawing.Point(535, 328)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(80, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Cancel"
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
        Me.pnlTop.Size = New System.Drawing.Size(623, 64)
        Me.pnlTop.TabIndex = 58
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(64, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Create a custom updater"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(61, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(201, 29)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Custom Updater"
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
        Me.gbUpdateModifier.Controls.Add(Me.rbUpdateModifier_Marked)
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
        'rbUpdateModifier_Marked
        '
        Me.rbUpdateModifier_Marked.AutoSize = True
        Me.rbUpdateModifier_Marked.Enabled = False
        Me.rbUpdateModifier_Marked.Location = New System.Drawing.Point(291, 19)
        Me.rbUpdateModifier_Marked.Name = "rbUpdateModifier_Marked"
        Me.rbUpdateModifier_Marked.Size = New System.Drawing.Size(98, 17)
        Me.rbUpdateModifier_Marked.TabIndex = 62
        Me.rbUpdateModifier_Marked.Text = "Marked Movies"
        Me.rbUpdateModifier_Marked.UseVisualStyleBackColor = True
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
        Me.rbUpdate_Ask.Location = New System.Drawing.Point(178, 18)
        Me.rbUpdate_Ask.Name = "rbUpdate_Ask"
        Me.rbUpdate_Ask.Size = New System.Drawing.Size(205, 17)
        Me.rbUpdate_Ask.TabIndex = 64
        Me.rbUpdate_Ask.Text = "Ask (Require Input If No Exact Match)"
        Me.rbUpdate_Ask.UseVisualStyleBackColor = True
        '
        'rbUpdate_Auto
        '
        Me.rbUpdate_Auto.AutoSize = True
        Me.rbUpdate_Auto.Checked = True
        Me.rbUpdate_Auto.Location = New System.Drawing.Point(6, 18)
        Me.rbUpdate_Auto.Name = "rbUpdate_Auto"
        Me.rbUpdate_Auto.Size = New System.Drawing.Size(165, 17)
        Me.rbUpdate_Auto.TabIndex = 63
        Me.rbUpdate_Auto.TabStop = True
        Me.rbUpdate_Auto.Text = "Automatic (Force Best Match)"
        Me.rbUpdate_Auto.UseVisualStyleBackColor = True
        '
        'gbUpdateItems
        '
        Me.gbUpdateItems.Controls.Add(Me.rbMediaInfo)
        Me.gbUpdateItems.Controls.Add(Me.rbTrailer)
        Me.gbUpdateItems.Controls.Add(Me.rbExtra)
        Me.gbUpdateItems.Controls.Add(Me.rbFanart)
        Me.gbUpdateItems.Controls.Add(Me.rbPoster)
        Me.gbUpdateItems.Controls.Add(Me.rbNfo)
        Me.gbUpdateItems.Controls.Add(Me.rbAll)
        Me.gbUpdateItems.Location = New System.Drawing.Point(7, 179)
        Me.gbUpdateItems.Name = "gbUpdateItems"
        Me.gbUpdateItems.Size = New System.Drawing.Size(147, 145)
        Me.gbUpdateItems.TabIndex = 62
        Me.gbUpdateItems.TabStop = False
        Me.gbUpdateItems.Text = "Modifiers"
        '
        'rbMediaInfo
        '
        Me.rbMediaInfo.AutoSize = True
        Me.rbMediaInfo.Location = New System.Drawing.Point(6, 121)
        Me.rbMediaInfo.Name = "rbMediaInfo"
        Me.rbMediaInfo.Size = New System.Drawing.Size(99, 17)
        Me.rbMediaInfo.TabIndex = 73
        Me.rbMediaInfo.Text = "Media Info Only"
        Me.rbMediaInfo.UseVisualStyleBackColor = True
        '
        'rbTrailer
        '
        Me.rbTrailer.AutoSize = True
        Me.rbTrailer.Location = New System.Drawing.Point(6, 104)
        Me.rbTrailer.Name = "rbTrailer"
        Me.rbTrailer.Size = New System.Drawing.Size(78, 17)
        Me.rbTrailer.TabIndex = 72
        Me.rbTrailer.Text = "Trailer Only"
        Me.rbTrailer.UseVisualStyleBackColor = True
        '
        'rbExtra
        '
        Me.rbExtra.AutoSize = True
        Me.rbExtra.Location = New System.Drawing.Point(6, 87)
        Me.rbExtra.Name = "rbExtra"
        Me.rbExtra.Size = New System.Drawing.Size(107, 17)
        Me.rbExtra.TabIndex = 71
        Me.rbExtra.Text = "Extrathumbs Only"
        Me.rbExtra.UseVisualStyleBackColor = True
        '
        'rbFanart
        '
        Me.rbFanart.AutoSize = True
        Me.rbFanart.Location = New System.Drawing.Point(6, 70)
        Me.rbFanart.Name = "rbFanart"
        Me.rbFanart.Size = New System.Drawing.Size(79, 17)
        Me.rbFanart.TabIndex = 70
        Me.rbFanart.Text = "Fanart Only"
        Me.rbFanart.UseVisualStyleBackColor = True
        '
        'rbPoster
        '
        Me.rbPoster.AutoSize = True
        Me.rbPoster.Location = New System.Drawing.Point(6, 53)
        Me.rbPoster.Name = "rbPoster"
        Me.rbPoster.Size = New System.Drawing.Size(79, 17)
        Me.rbPoster.TabIndex = 69
        Me.rbPoster.Text = "Poster Only"
        Me.rbPoster.UseVisualStyleBackColor = True
        '
        'rbNfo
        '
        Me.rbNfo.AutoSize = True
        Me.rbNfo.Location = New System.Drawing.Point(6, 36)
        Me.rbNfo.Name = "rbNfo"
        Me.rbNfo.Size = New System.Drawing.Size(71, 17)
        Me.rbNfo.TabIndex = 68
        Me.rbNfo.Text = "NFO Only"
        Me.rbNfo.UseVisualStyleBackColor = True
        '
        'rbAll
        '
        Me.rbAll.AutoSize = True
        Me.rbAll.Checked = True
        Me.rbAll.Location = New System.Drawing.Point(6, 19)
        Me.rbAll.Name = "rbAll"
        Me.rbAll.Size = New System.Drawing.Size(64, 17)
        Me.rbAll.TabIndex = 67
        Me.rbAll.TabStop = True
        Me.rbAll.Text = "All Items"
        Me.rbAll.UseVisualStyleBackColor = True
        '
        'Update_Button
        '
        Me.Update_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Update_Button.Location = New System.Drawing.Point(444, 328)
        Me.Update_Button.Name = "Update_Button"
        Me.Update_Button.Size = New System.Drawing.Size(80, 23)
        Me.Update_Button.TabIndex = 63
        Me.Update_Button.Text = "Update"
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
        'gbOptions
        '
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
        Me.gbOptions.Location = New System.Drawing.Point(402, 70)
        Me.gbOptions.Name = "gbOptions"
        Me.gbOptions.Size = New System.Drawing.Size(213, 254)
        Me.gbOptions.TabIndex = 66
        Me.gbOptions.TabStop = False
        Me.gbOptions.Text = "Options"
        '
        'chkCrew
        '
        Me.chkCrew.AutoSize = True
        Me.chkCrew.Location = New System.Drawing.Point(129, 203)
        Me.chkCrew.Name = "chkCrew"
        Me.chkCrew.Size = New System.Drawing.Size(79, 17)
        Me.chkCrew.TabIndex = 18
        Me.chkCrew.Text = "Other Crew"
        Me.chkCrew.UseVisualStyleBackColor = True
        '
        'chkMusicBy
        '
        Me.chkMusicBy.AutoSize = True
        Me.chkMusicBy.Location = New System.Drawing.Point(129, 180)
        Me.chkMusicBy.Name = "chkMusicBy"
        Me.chkMusicBy.Size = New System.Drawing.Size(69, 17)
        Me.chkMusicBy.TabIndex = 17
        Me.chkMusicBy.Text = "Music By"
        Me.chkMusicBy.UseVisualStyleBackColor = True
        '
        'chkProducers
        '
        Me.chkProducers.AutoSize = True
        Me.chkProducers.Location = New System.Drawing.Point(129, 157)
        Me.chkProducers.Name = "chkProducers"
        Me.chkProducers.Size = New System.Drawing.Size(74, 17)
        Me.chkProducers.TabIndex = 16
        Me.chkProducers.Text = "Producers"
        Me.chkProducers.UseVisualStyleBackColor = True
        '
        'chkWriters
        '
        Me.chkWriters.AutoSize = True
        Me.chkWriters.Location = New System.Drawing.Point(129, 134)
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
        Me.chkPlot.Location = New System.Drawing.Point(129, 65)
        Me.chkPlot.Name = "chkPlot"
        Me.chkPlot.Size = New System.Drawing.Size(44, 17)
        Me.chkPlot.TabIndex = 12
        Me.chkPlot.Text = "Plot"
        Me.chkPlot.UseVisualStyleBackColor = True
        '
        'chkOutline
        '
        Me.chkOutline.AutoSize = True
        Me.chkOutline.Location = New System.Drawing.Point(129, 42)
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
        Me.chkDirector.Location = New System.Drawing.Point(129, 111)
        Me.chkDirector.Name = "chkDirector"
        Me.chkDirector.Size = New System.Drawing.Size(63, 17)
        Me.chkDirector.TabIndex = 9
        Me.chkDirector.Text = "Director"
        Me.chkDirector.UseVisualStyleBackColor = True
        '
        'chkTagline
        '
        Me.chkTagline.AutoSize = True
        Me.chkTagline.Location = New System.Drawing.Point(129, 19)
        Me.chkTagline.Name = "chkTagline"
        Me.chkTagline.Size = New System.Drawing.Size(61, 17)
        Me.chkTagline.TabIndex = 8
        Me.chkTagline.Text = "Tagline"
        Me.chkTagline.UseVisualStyleBackColor = True
        '
        'chkCast
        '
        Me.chkCast.AutoSize = True
        Me.chkCast.Location = New System.Drawing.Point(129, 88)
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
        Me.chkTrailer.Location = New System.Drawing.Point(6, 227)
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
        'dlgUpdateMedia
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 354)
        Me.Controls.Add(Me.gbOptions)
        Me.Controls.Add(Me.gbInfoBox)
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
        Me.Text = "Custom Updater"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbUpdateModifier.ResumeLayout(False)
        Me.gbUpdateModifier.PerformLayout()
        Me.gbUpdateType.ResumeLayout(False)
        Me.gbUpdateType.PerformLayout()
        Me.gbUpdateItems.ResumeLayout(False)
        Me.gbUpdateItems.PerformLayout()
        Me.gbInfoBox.ResumeLayout(False)
        Me.gbOptions.ResumeLayout(False)
        Me.gbOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents rbUpdateModifier_All As System.Windows.Forms.RadioButton
    Friend WithEvents gbUpdateModifier As System.Windows.Forms.GroupBox
    Friend WithEvents rbUpdateModifier_Missing As System.Windows.Forms.RadioButton
    Friend WithEvents rbUpdateModifier_New As System.Windows.Forms.RadioButton
    Friend WithEvents rbUpdateModifier_Marked As System.Windows.Forms.RadioButton
    Friend WithEvents gbUpdateType As System.Windows.Forms.GroupBox
    Friend WithEvents rbUpdate_Auto As System.Windows.Forms.RadioButton
    Friend WithEvents rbUpdate_Ask As System.Windows.Forms.RadioButton
    Friend WithEvents gbUpdateItems As System.Windows.Forms.GroupBox
    Friend WithEvents Update_Button As System.Windows.Forms.Button
    Friend WithEvents gbInfoBox As System.Windows.Forms.GroupBox
    Friend WithEvents lblInfoBox As System.Windows.Forms.Label
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
    Friend WithEvents rbMediaInfo As System.Windows.Forms.RadioButton
    Friend WithEvents rbTrailer As System.Windows.Forms.RadioButton
    Friend WithEvents rbExtra As System.Windows.Forms.RadioButton
    Friend WithEvents rbFanart As System.Windows.Forms.RadioButton
    Friend WithEvents rbPoster As System.Windows.Forms.RadioButton
    Friend WithEvents rbNfo As System.Windows.Forms.RadioButton
    Friend WithEvents rbAll As System.Windows.Forms.RadioButton

End Class
