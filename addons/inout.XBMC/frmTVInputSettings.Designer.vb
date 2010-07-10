<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTVInputSettings
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkEnabled = New System.Windows.Forms.CheckBox
        Me.pnlSettings = New System.Windows.Forms.Panel
        Me.gbTVNaming = New System.Windows.Forms.GroupBox
        Me.gbAllSeasonPoster = New System.Windows.Forms.GroupBox
        Me.chkSeasonAllJPG = New System.Windows.Forms.CheckBox
        Me.chkSeasonAllTBN = New System.Windows.Forms.CheckBox
        Me.gbEpisodeFanart = New System.Windows.Forms.GroupBox
        Me.chkEpisodeDotFanart = New System.Windows.Forms.CheckBox
        Me.chkEpisodeDashFanart = New System.Windows.Forms.CheckBox
        Me.gbEpisodePosters = New System.Windows.Forms.GroupBox
        Me.chkEpisodeJPG = New System.Windows.Forms.CheckBox
        Me.chkEpisodeTBN = New System.Windows.Forms.CheckBox
        Me.gbSeasonFanart = New System.Windows.Forms.GroupBox
        Me.chkSeasonDotFanart = New System.Windows.Forms.CheckBox
        Me.chkSeasonDashFanart = New System.Windows.Forms.CheckBox
        Me.chkSeasonFanartJPG = New System.Windows.Forms.CheckBox
        Me.gbSeasonPosters = New System.Windows.Forms.GroupBox
        Me.chkSeasonFolderJPG = New System.Windows.Forms.CheckBox
        Me.chkSeasonNameJPG = New System.Windows.Forms.CheckBox
        Me.chkSeasonNameTBN = New System.Windows.Forms.CheckBox
        Me.chkSeasonPosterJPG = New System.Windows.Forms.CheckBox
        Me.chkSeasonPosterTBN = New System.Windows.Forms.CheckBox
        Me.chkSeasonXTBN = New System.Windows.Forms.CheckBox
        Me.chkSeasonXXTBN = New System.Windows.Forms.CheckBox
        Me.gbShowFanart = New System.Windows.Forms.GroupBox
        Me.chkShowDotFanart = New System.Windows.Forms.CheckBox
        Me.chkShowDashFanart = New System.Windows.Forms.CheckBox
        Me.chkShowFanartJPG = New System.Windows.Forms.CheckBox
        Me.gbShowPosters = New System.Windows.Forms.GroupBox
        Me.chkShowJPG = New System.Windows.Forms.CheckBox
        Me.chkShowTBN = New System.Windows.Forms.CheckBox
        Me.chkShowPosterJPG = New System.Windows.Forms.CheckBox
        Me.chkShowPosterTBN = New System.Windows.Forms.CheckBox
        Me.chkShowFolderJPG = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        Me.pnlSettings.SuspendLayout()
        Me.gbTVNaming.SuspendLayout()
        Me.gbAllSeasonPoster.SuspendLayout()
        Me.gbEpisodeFanart.SuspendLayout()
        Me.gbEpisodePosters.SuspendLayout()
        Me.gbSeasonFanart.SuspendLayout()
        Me.gbSeasonPosters.SuspendLayout()
        Me.gbShowFanart.SuspendLayout()
        Me.gbShowPosters.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.chkEnabled)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(617, 25)
        Me.Panel1.TabIndex = 82
        '
        'chkEnabled
        '
        Me.chkEnabled.AutoSize = True
        Me.chkEnabled.Location = New System.Drawing.Point(10, 5)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.Size = New System.Drawing.Size(68, 17)
        Me.chkEnabled.TabIndex = 80
        Me.chkEnabled.Text = "Enabled"
        Me.chkEnabled.UseVisualStyleBackColor = True
        '
        'pnlSettings
        '
        Me.pnlSettings.Controls.Add(Me.gbTVNaming)
        Me.pnlSettings.Controls.Add(Me.Panel1)
        Me.pnlSettings.Location = New System.Drawing.Point(3, 12)
        Me.pnlSettings.Name = "pnlSettings"
        Me.pnlSettings.Size = New System.Drawing.Size(617, 400)
        Me.pnlSettings.TabIndex = 83
        '
        'gbTVNaming
        '
        Me.gbTVNaming.Controls.Add(Me.gbAllSeasonPoster)
        Me.gbTVNaming.Controls.Add(Me.gbEpisodeFanart)
        Me.gbTVNaming.Controls.Add(Me.gbEpisodePosters)
        Me.gbTVNaming.Controls.Add(Me.gbSeasonFanart)
        Me.gbTVNaming.Controls.Add(Me.gbSeasonPosters)
        Me.gbTVNaming.Controls.Add(Me.gbShowFanart)
        Me.gbTVNaming.Controls.Add(Me.gbShowPosters)
        Me.gbTVNaming.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbTVNaming.Location = New System.Drawing.Point(3, 31)
        Me.gbTVNaming.Name = "gbTVNaming"
        Me.gbTVNaming.Size = New System.Drawing.Size(437, 253)
        Me.gbTVNaming.TabIndex = 83
        Me.gbTVNaming.TabStop = False
        Me.gbTVNaming.Text = "File Naming"
        '
        'gbAllSeasonPoster
        '
        Me.gbAllSeasonPoster.Controls.Add(Me.chkSeasonAllJPG)
        Me.gbAllSeasonPoster.Controls.Add(Me.chkSeasonAllTBN)
        Me.gbAllSeasonPoster.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbAllSeasonPoster.Location = New System.Drawing.Point(5, 195)
        Me.gbAllSeasonPoster.Name = "gbAllSeasonPoster"
        Me.gbAllSeasonPoster.Size = New System.Drawing.Size(133, 54)
        Me.gbAllSeasonPoster.TabIndex = 8
        Me.gbAllSeasonPoster.TabStop = False
        Me.gbAllSeasonPoster.Text = "All Season Posters"
        '
        'chkSeasonAllJPG
        '
        Me.chkSeasonAllJPG.AutoSize = True
        Me.chkSeasonAllJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonAllJPG.Location = New System.Drawing.Point(6, 34)
        Me.chkSeasonAllJPG.Name = "chkSeasonAllJPG"
        Me.chkSeasonAllJPG.Size = New System.Drawing.Size(98, 17)
        Me.chkSeasonAllJPG.TabIndex = 1
        Me.chkSeasonAllJPG.Text = "season-all.jpg"
        Me.chkSeasonAllJPG.UseVisualStyleBackColor = True
        '
        'chkSeasonAllTBN
        '
        Me.chkSeasonAllTBN.AutoSize = True
        Me.chkSeasonAllTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonAllTBN.Location = New System.Drawing.Point(6, 18)
        Me.chkSeasonAllTBN.Name = "chkSeasonAllTBN"
        Me.chkSeasonAllTBN.Size = New System.Drawing.Size(99, 17)
        Me.chkSeasonAllTBN.TabIndex = 0
        Me.chkSeasonAllTBN.Text = "season-all.tbn"
        Me.chkSeasonAllTBN.UseVisualStyleBackColor = True
        '
        'gbEpisodeFanart
        '
        Me.gbEpisodeFanart.Controls.Add(Me.chkEpisodeDotFanart)
        Me.gbEpisodeFanart.Controls.Add(Me.chkEpisodeDashFanart)
        Me.gbEpisodeFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbEpisodeFanart.Location = New System.Drawing.Point(292, 67)
        Me.gbEpisodeFanart.Name = "gbEpisodeFanart"
        Me.gbEpisodeFanart.Size = New System.Drawing.Size(140, 52)
        Me.gbEpisodeFanart.TabIndex = 4
        Me.gbEpisodeFanart.TabStop = False
        Me.gbEpisodeFanart.Text = "Episode Fanart"
        '
        'chkEpisodeDotFanart
        '
        Me.chkEpisodeDotFanart.AutoSize = True
        Me.chkEpisodeDotFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEpisodeDotFanart.Location = New System.Drawing.Point(5, 31)
        Me.chkEpisodeDotFanart.Name = "chkEpisodeDotFanart"
        Me.chkEpisodeDotFanart.Size = New System.Drawing.Size(137, 17)
        Me.chkEpisodeDotFanart.TabIndex = 2
        Me.chkEpisodeDotFanart.Text = "<episode>.fanart.jpg"
        Me.chkEpisodeDotFanart.UseVisualStyleBackColor = True
        '
        'chkEpisodeDashFanart
        '
        Me.chkEpisodeDashFanart.AutoSize = True
        Me.chkEpisodeDashFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEpisodeDashFanart.Location = New System.Drawing.Point(5, 15)
        Me.chkEpisodeDashFanart.Name = "chkEpisodeDashFanart"
        Me.chkEpisodeDashFanart.Size = New System.Drawing.Size(138, 17)
        Me.chkEpisodeDashFanart.TabIndex = 1
        Me.chkEpisodeDashFanart.Text = "<episode>-fanart.jpg"
        Me.chkEpisodeDashFanart.UseVisualStyleBackColor = True
        '
        'gbEpisodePosters
        '
        Me.gbEpisodePosters.Controls.Add(Me.chkEpisodeJPG)
        Me.gbEpisodePosters.Controls.Add(Me.chkEpisodeTBN)
        Me.gbEpisodePosters.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbEpisodePosters.Location = New System.Drawing.Point(292, 15)
        Me.gbEpisodePosters.Name = "gbEpisodePosters"
        Me.gbEpisodePosters.Size = New System.Drawing.Size(140, 52)
        Me.gbEpisodePosters.TabIndex = 6
        Me.gbEpisodePosters.TabStop = False
        Me.gbEpisodePosters.Text = "Episode Posters"
        '
        'chkEpisodeJPG
        '
        Me.chkEpisodeJPG.AutoSize = True
        Me.chkEpisodeJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEpisodeJPG.Location = New System.Drawing.Point(6, 31)
        Me.chkEpisodeJPG.Name = "chkEpisodeJPG"
        Me.chkEpisodeJPG.Size = New System.Drawing.Size(103, 17)
        Me.chkEpisodeJPG.TabIndex = 1
        Me.chkEpisodeJPG.Text = "<episode>.jpg"
        Me.chkEpisodeJPG.UseVisualStyleBackColor = True
        '
        'chkEpisodeTBN
        '
        Me.chkEpisodeTBN.AutoSize = True
        Me.chkEpisodeTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEpisodeTBN.Location = New System.Drawing.Point(6, 15)
        Me.chkEpisodeTBN.Name = "chkEpisodeTBN"
        Me.chkEpisodeTBN.Size = New System.Drawing.Size(104, 17)
        Me.chkEpisodeTBN.TabIndex = 0
        Me.chkEpisodeTBN.Text = "<episode>.tbn"
        Me.chkEpisodeTBN.UseVisualStyleBackColor = True
        '
        'gbSeasonFanart
        '
        Me.gbSeasonFanart.Controls.Add(Me.chkSeasonDotFanart)
        Me.gbSeasonFanart.Controls.Add(Me.chkSeasonDashFanart)
        Me.gbSeasonFanart.Controls.Add(Me.chkSeasonFanartJPG)
        Me.gbSeasonFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbSeasonFanart.Location = New System.Drawing.Point(144, 150)
        Me.gbSeasonFanart.Name = "gbSeasonFanart"
        Me.gbSeasonFanart.Size = New System.Drawing.Size(145, 71)
        Me.gbSeasonFanart.TabIndex = 3
        Me.gbSeasonFanart.TabStop = False
        Me.gbSeasonFanart.Text = "Season Fanart"
        '
        'chkSeasonDotFanart
        '
        Me.chkSeasonDotFanart.AutoSize = True
        Me.chkSeasonDotFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonDotFanart.Location = New System.Drawing.Point(6, 47)
        Me.chkSeasonDotFanart.Name = "chkSeasonDotFanart"
        Me.chkSeasonDotFanart.Size = New System.Drawing.Size(132, 17)
        Me.chkSeasonDotFanart.TabIndex = 2
        Me.chkSeasonDotFanart.Text = "<season>.fanart.jpg"
        Me.chkSeasonDotFanart.UseVisualStyleBackColor = True
        '
        'chkSeasonDashFanart
        '
        Me.chkSeasonDashFanart.AutoSize = True
        Me.chkSeasonDashFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonDashFanart.Location = New System.Drawing.Point(6, 31)
        Me.chkSeasonDashFanart.Name = "chkSeasonDashFanart"
        Me.chkSeasonDashFanart.Size = New System.Drawing.Size(133, 17)
        Me.chkSeasonDashFanart.TabIndex = 1
        Me.chkSeasonDashFanart.Text = "<season>-fanart.jpg"
        Me.chkSeasonDashFanart.UseVisualStyleBackColor = True
        '
        'chkSeasonFanartJPG
        '
        Me.chkSeasonFanartJPG.AutoSize = True
        Me.chkSeasonFanartJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonFanartJPG.Location = New System.Drawing.Point(6, 15)
        Me.chkSeasonFanartJPG.Name = "chkSeasonFanartJPG"
        Me.chkSeasonFanartJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkSeasonFanartJPG.TabIndex = 0
        Me.chkSeasonFanartJPG.Text = "fanart.jpg"
        Me.chkSeasonFanartJPG.UseVisualStyleBackColor = True
        '
        'gbSeasonPosters
        '
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonFolderJPG)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonNameJPG)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonNameTBN)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonPosterJPG)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonPosterTBN)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonXTBN)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonXXTBN)
        Me.gbSeasonPosters.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbSeasonPosters.Location = New System.Drawing.Point(143, 15)
        Me.gbSeasonPosters.Name = "gbSeasonPosters"
        Me.gbSeasonPosters.Size = New System.Drawing.Size(145, 130)
        Me.gbSeasonPosters.TabIndex = 4
        Me.gbSeasonPosters.TabStop = False
        Me.gbSeasonPosters.Text = "Season Posters"
        '
        'chkSeasonFolderJPG
        '
        Me.chkSeasonFolderJPG.AutoSize = True
        Me.chkSeasonFolderJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonFolderJPG.Location = New System.Drawing.Point(6, 111)
        Me.chkSeasonFolderJPG.Name = "chkSeasonFolderJPG"
        Me.chkSeasonFolderJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkSeasonFolderJPG.TabIndex = 6
        Me.chkSeasonFolderJPG.Text = "folder.jpg"
        Me.chkSeasonFolderJPG.UseVisualStyleBackColor = True
        '
        'chkSeasonNameJPG
        '
        Me.chkSeasonNameJPG.AutoSize = True
        Me.chkSeasonNameJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonNameJPG.Location = New System.Drawing.Point(6, 95)
        Me.chkSeasonNameJPG.Name = "chkSeasonNameJPG"
        Me.chkSeasonNameJPG.Size = New System.Drawing.Size(98, 17)
        Me.chkSeasonNameJPG.TabIndex = 5
        Me.chkSeasonNameJPG.Text = "<season>.jpg"
        Me.chkSeasonNameJPG.UseVisualStyleBackColor = True
        '
        'chkSeasonNameTBN
        '
        Me.chkSeasonNameTBN.AutoSize = True
        Me.chkSeasonNameTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonNameTBN.Location = New System.Drawing.Point(6, 79)
        Me.chkSeasonNameTBN.Name = "chkSeasonNameTBN"
        Me.chkSeasonNameTBN.Size = New System.Drawing.Size(99, 17)
        Me.chkSeasonNameTBN.TabIndex = 4
        Me.chkSeasonNameTBN.Text = "<season>.tbn"
        Me.chkSeasonNameTBN.UseVisualStyleBackColor = True
        '
        'chkSeasonPosterJPG
        '
        Me.chkSeasonPosterJPG.AutoSize = True
        Me.chkSeasonPosterJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonPosterJPG.Location = New System.Drawing.Point(6, 63)
        Me.chkSeasonPosterJPG.Name = "chkSeasonPosterJPG"
        Me.chkSeasonPosterJPG.Size = New System.Drawing.Size(79, 17)
        Me.chkSeasonPosterJPG.TabIndex = 3
        Me.chkSeasonPosterJPG.Text = "poster.jpg"
        Me.chkSeasonPosterJPG.UseVisualStyleBackColor = True
        '
        'chkSeasonPosterTBN
        '
        Me.chkSeasonPosterTBN.AutoSize = True
        Me.chkSeasonPosterTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonPosterTBN.Location = New System.Drawing.Point(6, 47)
        Me.chkSeasonPosterTBN.Name = "chkSeasonPosterTBN"
        Me.chkSeasonPosterTBN.Size = New System.Drawing.Size(80, 17)
        Me.chkSeasonPosterTBN.TabIndex = 2
        Me.chkSeasonPosterTBN.Text = "poster.tbn"
        Me.chkSeasonPosterTBN.UseVisualStyleBackColor = True
        '
        'chkSeasonXTBN
        '
        Me.chkSeasonXTBN.AutoSize = True
        Me.chkSeasonXTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonXTBN.Location = New System.Drawing.Point(6, 31)
        Me.chkSeasonXTBN.Name = "chkSeasonXTBN"
        Me.chkSeasonXTBN.Size = New System.Drawing.Size(89, 17)
        Me.chkSeasonXTBN.TabIndex = 1
        Me.chkSeasonXTBN.Text = "seasonX.tbn"
        Me.chkSeasonXTBN.UseVisualStyleBackColor = True
        '
        'chkSeasonXXTBN
        '
        Me.chkSeasonXXTBN.AutoSize = True
        Me.chkSeasonXXTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonXXTBN.Location = New System.Drawing.Point(6, 15)
        Me.chkSeasonXXTBN.Name = "chkSeasonXXTBN"
        Me.chkSeasonXXTBN.Size = New System.Drawing.Size(95, 17)
        Me.chkSeasonXXTBN.TabIndex = 0
        Me.chkSeasonXXTBN.Text = "seasonXX.tbn"
        Me.chkSeasonXXTBN.UseVisualStyleBackColor = True
        '
        'gbShowFanart
        '
        Me.gbShowFanart.Controls.Add(Me.chkShowDotFanart)
        Me.gbShowFanart.Controls.Add(Me.chkShowDashFanart)
        Me.gbShowFanart.Controls.Add(Me.chkShowFanartJPG)
        Me.gbShowFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbShowFanart.Location = New System.Drawing.Point(5, 120)
        Me.gbShowFanart.Name = "gbShowFanart"
        Me.gbShowFanart.Size = New System.Drawing.Size(133, 70)
        Me.gbShowFanart.TabIndex = 2
        Me.gbShowFanart.TabStop = False
        Me.gbShowFanart.Text = "Show Fanart"
        '
        'chkShowDotFanart
        '
        Me.chkShowDotFanart.AutoSize = True
        Me.chkShowDotFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowDotFanart.Location = New System.Drawing.Point(6, 51)
        Me.chkShowDotFanart.Name = "chkShowDotFanart"
        Me.chkShowDotFanart.Size = New System.Drawing.Size(124, 17)
        Me.chkShowDotFanart.TabIndex = 2
        Me.chkShowDotFanart.Text = "<show>.fanart.jpg"
        Me.chkShowDotFanart.UseVisualStyleBackColor = True
        '
        'chkShowDashFanart
        '
        Me.chkShowDashFanart.AutoSize = True
        Me.chkShowDashFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowDashFanart.Location = New System.Drawing.Point(6, 35)
        Me.chkShowDashFanart.Name = "chkShowDashFanart"
        Me.chkShowDashFanart.Size = New System.Drawing.Size(125, 17)
        Me.chkShowDashFanart.TabIndex = 1
        Me.chkShowDashFanart.Text = "<show>-fanart.jpg"
        Me.chkShowDashFanart.UseVisualStyleBackColor = True
        '
        'chkShowFanartJPG
        '
        Me.chkShowFanartJPG.AutoSize = True
        Me.chkShowFanartJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowFanartJPG.Location = New System.Drawing.Point(6, 19)
        Me.chkShowFanartJPG.Name = "chkShowFanartJPG"
        Me.chkShowFanartJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkShowFanartJPG.TabIndex = 0
        Me.chkShowFanartJPG.Text = "fanart.jpg"
        Me.chkShowFanartJPG.UseVisualStyleBackColor = True
        '
        'gbShowPosters
        '
        Me.gbShowPosters.Controls.Add(Me.chkShowJPG)
        Me.gbShowPosters.Controls.Add(Me.chkShowTBN)
        Me.gbShowPosters.Controls.Add(Me.chkShowPosterJPG)
        Me.gbShowPosters.Controls.Add(Me.chkShowPosterTBN)
        Me.gbShowPosters.Controls.Add(Me.chkShowFolderJPG)
        Me.gbShowPosters.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbShowPosters.Location = New System.Drawing.Point(5, 15)
        Me.gbShowPosters.Name = "gbShowPosters"
        Me.gbShowPosters.Size = New System.Drawing.Size(133, 99)
        Me.gbShowPosters.TabIndex = 0
        Me.gbShowPosters.TabStop = False
        Me.gbShowPosters.Text = "Show Posters"
        '
        'chkShowJPG
        '
        Me.chkShowJPG.AutoSize = True
        Me.chkShowJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowJPG.Location = New System.Drawing.Point(6, 32)
        Me.chkShowJPG.Name = "chkShowJPG"
        Me.chkShowJPG.Size = New System.Drawing.Size(90, 17)
        Me.chkShowJPG.TabIndex = 5
        Me.chkShowJPG.Text = "<show>.jpg"
        Me.chkShowJPG.UseVisualStyleBackColor = True
        '
        'chkShowTBN
        '
        Me.chkShowTBN.AutoSize = True
        Me.chkShowTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowTBN.Location = New System.Drawing.Point(6, 16)
        Me.chkShowTBN.Name = "chkShowTBN"
        Me.chkShowTBN.Size = New System.Drawing.Size(91, 17)
        Me.chkShowTBN.TabIndex = 4
        Me.chkShowTBN.Text = "<show>.tbn"
        Me.chkShowTBN.UseVisualStyleBackColor = True
        '
        'chkShowPosterJPG
        '
        Me.chkShowPosterJPG.AutoSize = True
        Me.chkShowPosterJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowPosterJPG.Location = New System.Drawing.Point(6, 64)
        Me.chkShowPosterJPG.Name = "chkShowPosterJPG"
        Me.chkShowPosterJPG.Size = New System.Drawing.Size(79, 17)
        Me.chkShowPosterJPG.TabIndex = 3
        Me.chkShowPosterJPG.Text = "poster.jpg"
        Me.chkShowPosterJPG.UseVisualStyleBackColor = True
        '
        'chkShowPosterTBN
        '
        Me.chkShowPosterTBN.AutoSize = True
        Me.chkShowPosterTBN.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowPosterTBN.Location = New System.Drawing.Point(6, 48)
        Me.chkShowPosterTBN.Name = "chkShowPosterTBN"
        Me.chkShowPosterTBN.Size = New System.Drawing.Size(80, 17)
        Me.chkShowPosterTBN.TabIndex = 2
        Me.chkShowPosterTBN.Text = "poster.tbn"
        Me.chkShowPosterTBN.UseVisualStyleBackColor = True
        '
        'chkShowFolderJPG
        '
        Me.chkShowFolderJPG.AutoSize = True
        Me.chkShowFolderJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowFolderJPG.Location = New System.Drawing.Point(6, 80)
        Me.chkShowFolderJPG.Name = "chkShowFolderJPG"
        Me.chkShowFolderJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkShowFolderJPG.TabIndex = 1
        Me.chkShowFolderJPG.Text = "folder.jpg"
        Me.chkShowFolderJPG.UseVisualStyleBackColor = True
        '
        'frmTVInputSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(625, 415)
        Me.Controls.Add(Me.pnlSettings)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTVInputSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "XBMC Input Module"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlSettings.ResumeLayout(False)
        Me.gbTVNaming.ResumeLayout(False)
        Me.gbAllSeasonPoster.ResumeLayout(False)
        Me.gbAllSeasonPoster.PerformLayout()
        Me.gbEpisodeFanart.ResumeLayout(False)
        Me.gbEpisodeFanart.PerformLayout()
        Me.gbEpisodePosters.ResumeLayout(False)
        Me.gbEpisodePosters.PerformLayout()
        Me.gbSeasonFanart.ResumeLayout(False)
        Me.gbSeasonFanart.PerformLayout()
        Me.gbSeasonPosters.ResumeLayout(False)
        Me.gbSeasonPosters.PerformLayout()
        Me.gbShowFanart.ResumeLayout(False)
        Me.gbShowFanart.PerformLayout()
        Me.gbShowPosters.ResumeLayout(False)
        Me.gbShowPosters.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents pnlSettings As System.Windows.Forms.Panel
    Friend WithEvents gbTVNaming As System.Windows.Forms.GroupBox
    Friend WithEvents gbAllSeasonPoster As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeasonAllJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonAllTBN As System.Windows.Forms.CheckBox
    Friend WithEvents gbEpisodeFanart As System.Windows.Forms.GroupBox
    Friend WithEvents chkEpisodeDotFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkEpisodeDashFanart As System.Windows.Forms.CheckBox
    Friend WithEvents gbEpisodePosters As System.Windows.Forms.GroupBox
    Friend WithEvents chkEpisodeJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkEpisodeTBN As System.Windows.Forms.CheckBox
    Friend WithEvents gbSeasonFanart As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeasonDotFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonDashFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents gbSeasonPosters As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeasonFolderJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonNameJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonNameTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonPosterTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonXTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonXXTBN As System.Windows.Forms.CheckBox
    Friend WithEvents gbShowFanart As System.Windows.Forms.GroupBox
    Friend WithEvents chkShowDotFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowDashFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents gbShowPosters As System.Windows.Forms.GroupBox
    Friend WithEvents chkShowJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowPosterTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFolderJPG As System.Windows.Forms.CheckBox

End Class
