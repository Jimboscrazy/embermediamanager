<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTVOutputSettings
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.gbAllSeasonPoster = New System.Windows.Forms.GroupBox
        Me.chkSeasonAllJPG = New System.Windows.Forms.CheckBox
        Me.gbEpisodeFanart = New System.Windows.Forms.GroupBox
        Me.chkEpisodeDotFanart = New System.Windows.Forms.CheckBox
        Me.gbEpisodePosters = New System.Windows.Forms.GroupBox
        Me.chkEpisodeJPG = New System.Windows.Forms.CheckBox
        Me.gbSeasonFanart = New System.Windows.Forms.GroupBox
        Me.chkSeasonFanartJPG = New System.Windows.Forms.CheckBox
        Me.gbSeasonPosters = New System.Windows.Forms.GroupBox
        Me.chkSeasonFolderJPG = New System.Windows.Forms.CheckBox
        Me.chkSeasonNameJPG = New System.Windows.Forms.CheckBox
        Me.chkSeasonPosterJPG = New System.Windows.Forms.CheckBox
        Me.gbShowFanart = New System.Windows.Forms.GroupBox
        Me.chkShowDotFanart = New System.Windows.Forms.CheckBox
        Me.chkShowFanartJPG = New System.Windows.Forms.CheckBox
        Me.gbShowPosters = New System.Windows.Forms.GroupBox
        Me.chkShowJPG = New System.Windows.Forms.CheckBox
        Me.chkShowPosterJPG = New System.Windows.Forms.CheckBox
        Me.chkShowFolderJPG = New System.Windows.Forms.CheckBox
        Me.chkSeasonDotFanart = New System.Windows.Forms.CheckBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.CheckBox3 = New System.Windows.Forms.CheckBox
        Me.CheckBox4 = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        Me.pnlSettings.SuspendLayout()
        Me.gbTVNaming.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
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
        Me.gbTVNaming.Controls.Add(Me.GroupBox1)
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
        Me.gbTVNaming.Size = New System.Drawing.Size(555, 340)
        Me.gbTVNaming.TabIndex = 83
        Me.gbTVNaming.TabStop = False
        Me.gbTVNaming.Text = "File Naming"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBox1)
        Me.GroupBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(182, 234)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(179, 54)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Season Banner"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(5, 20)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(138, 17)
        Me.CheckBox1.TabIndex = 2
        Me.CheckBox1.Text = "<season>.banner.jpg"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'gbAllSeasonPoster
        '
        Me.gbAllSeasonPoster.Controls.Add(Me.CheckBox4)
        Me.gbAllSeasonPoster.Controls.Add(Me.chkSeasonAllJPG)
        Me.gbAllSeasonPoster.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbAllSeasonPoster.Location = New System.Drawing.Point(5, 234)
        Me.gbAllSeasonPoster.Name = "gbAllSeasonPoster"
        Me.gbAllSeasonPoster.Size = New System.Drawing.Size(169, 73)
        Me.gbAllSeasonPoster.TabIndex = 8
        Me.gbAllSeasonPoster.TabStop = False
        Me.gbAllSeasonPoster.Text = "All Season Posters"
        '
        'chkSeasonAllJPG
        '
        Me.chkSeasonAllJPG.AutoSize = True
        Me.chkSeasonAllJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonAllJPG.Location = New System.Drawing.Point(6, 21)
        Me.chkSeasonAllJPG.Name = "chkSeasonAllJPG"
        Me.chkSeasonAllJPG.Size = New System.Drawing.Size(161, 17)
        Me.chkSeasonAllJPG.TabIndex = 1
        Me.chkSeasonAllJPG.Text = "set_<show>_1.banner.jpg"
        Me.chkSeasonAllJPG.UseVisualStyleBackColor = True
        '
        'gbEpisodeFanart
        '
        Me.gbEpisodeFanart.Controls.Add(Me.chkEpisodeDotFanart)
        Me.gbEpisodeFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbEpisodeFanart.Location = New System.Drawing.Point(367, 67)
        Me.gbEpisodeFanart.Name = "gbEpisodeFanart"
        Me.gbEpisodeFanart.Size = New System.Drawing.Size(175, 53)
        Me.gbEpisodeFanart.TabIndex = 4
        Me.gbEpisodeFanart.TabStop = False
        Me.gbEpisodeFanart.Text = "Episode Fanart"
        '
        'chkEpisodeDotFanart
        '
        Me.chkEpisodeDotFanart.AutoSize = True
        Me.chkEpisodeDotFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEpisodeDotFanart.Location = New System.Drawing.Point(6, 21)
        Me.chkEpisodeDotFanart.Name = "chkEpisodeDotFanart"
        Me.chkEpisodeDotFanart.Size = New System.Drawing.Size(137, 17)
        Me.chkEpisodeDotFanart.TabIndex = 2
        Me.chkEpisodeDotFanart.Text = "<episode>.fanart.jpg"
        Me.chkEpisodeDotFanart.UseVisualStyleBackColor = True
        '
        'gbEpisodePosters
        '
        Me.gbEpisodePosters.Controls.Add(Me.chkEpisodeJPG)
        Me.gbEpisodePosters.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbEpisodePosters.Location = New System.Drawing.Point(367, 15)
        Me.gbEpisodePosters.Name = "gbEpisodePosters"
        Me.gbEpisodePosters.Size = New System.Drawing.Size(175, 52)
        Me.gbEpisodePosters.TabIndex = 6
        Me.gbEpisodePosters.TabStop = False
        Me.gbEpisodePosters.Text = "Episode Posters"
        '
        'chkEpisodeJPG
        '
        Me.chkEpisodeJPG.AutoSize = True
        Me.chkEpisodeJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEpisodeJPG.Location = New System.Drawing.Point(6, 21)
        Me.chkEpisodeJPG.Name = "chkEpisodeJPG"
        Me.chkEpisodeJPG.Size = New System.Drawing.Size(165, 17)
        Me.chkEpisodeJPG.TabIndex = 1
        Me.chkEpisodeJPG.Text = "<episode>.videoimage.jpg"
        Me.chkEpisodeJPG.UseVisualStyleBackColor = True
        '
        'gbSeasonFanart
        '
        Me.gbSeasonFanart.Controls.Add(Me.CheckBox2)
        Me.gbSeasonFanart.Controls.Add(Me.chkSeasonDotFanart)
        Me.gbSeasonFanart.Controls.Add(Me.chkSeasonFanartJPG)
        Me.gbSeasonFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbSeasonFanart.Location = New System.Drawing.Point(182, 126)
        Me.gbSeasonFanart.Name = "gbSeasonFanart"
        Me.gbSeasonFanart.Size = New System.Drawing.Size(179, 101)
        Me.gbSeasonFanart.TabIndex = 3
        Me.gbSeasonFanart.TabStop = False
        Me.gbSeasonFanart.Text = "Season Fanart"
        '
        'chkSeasonFanartJPG
        '
        Me.chkSeasonFanartJPG.AutoSize = True
        Me.chkSeasonFanartJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonFanartJPG.Location = New System.Drawing.Point(5, 18)
        Me.chkSeasonFanartJPG.Name = "chkSeasonFanartJPG"
        Me.chkSeasonFanartJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkSeasonFanartJPG.TabIndex = 0
        Me.chkSeasonFanartJPG.Text = "fanart.jpg"
        Me.chkSeasonFanartJPG.UseVisualStyleBackColor = True
        '
        'gbSeasonPosters
        '
        Me.gbSeasonPosters.Controls.Add(Me.CheckBox3)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonFolderJPG)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonNameJPG)
        Me.gbSeasonPosters.Controls.Add(Me.chkSeasonPosterJPG)
        Me.gbSeasonPosters.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbSeasonPosters.Location = New System.Drawing.Point(182, 15)
        Me.gbSeasonPosters.Name = "gbSeasonPosters"
        Me.gbSeasonPosters.Size = New System.Drawing.Size(179, 105)
        Me.gbSeasonPosters.TabIndex = 4
        Me.gbSeasonPosters.TabStop = False
        Me.gbSeasonPosters.Text = "Season Posters"
        '
        'chkSeasonFolderJPG
        '
        Me.chkSeasonFolderJPG.AutoSize = True
        Me.chkSeasonFolderJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonFolderJPG.Location = New System.Drawing.Point(5, 61)
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
        Me.chkSeasonNameJPG.Location = New System.Drawing.Point(5, 21)
        Me.chkSeasonNameJPG.Name = "chkSeasonNameJPG"
        Me.chkSeasonNameJPG.Size = New System.Drawing.Size(98, 17)
        Me.chkSeasonNameJPG.TabIndex = 5
        Me.chkSeasonNameJPG.Text = "<season>.jpg"
        Me.chkSeasonNameJPG.UseVisualStyleBackColor = True
        '
        'chkSeasonPosterJPG
        '
        Me.chkSeasonPosterJPG.AutoSize = True
        Me.chkSeasonPosterJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonPosterJPG.Location = New System.Drawing.Point(5, 40)
        Me.chkSeasonPosterJPG.Name = "chkSeasonPosterJPG"
        Me.chkSeasonPosterJPG.Size = New System.Drawing.Size(79, 17)
        Me.chkSeasonPosterJPG.TabIndex = 3
        Me.chkSeasonPosterJPG.Text = "poster.jpg"
        Me.chkSeasonPosterJPG.UseVisualStyleBackColor = True
        '
        'gbShowFanart
        '
        Me.gbShowFanart.Controls.Add(Me.chkShowDotFanart)
        Me.gbShowFanart.Controls.Add(Me.chkShowFanartJPG)
        Me.gbShowFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbShowFanart.Location = New System.Drawing.Point(5, 126)
        Me.gbShowFanart.Name = "gbShowFanart"
        Me.gbShowFanart.Size = New System.Drawing.Size(167, 101)
        Me.gbShowFanart.TabIndex = 2
        Me.gbShowFanart.TabStop = False
        Me.gbShowFanart.Text = "Show Fanart"
        '
        'chkShowDotFanart
        '
        Me.chkShowDotFanart.AutoSize = True
        Me.chkShowDotFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowDotFanart.Location = New System.Drawing.Point(6, 18)
        Me.chkShowDotFanart.Name = "chkShowDotFanart"
        Me.chkShowDotFanart.Size = New System.Drawing.Size(124, 17)
        Me.chkShowDotFanart.TabIndex = 2
        Me.chkShowDotFanart.Text = "<show>.fanart.jpg"
        Me.chkShowDotFanart.UseVisualStyleBackColor = True
        '
        'chkShowFanartJPG
        '
        Me.chkShowFanartJPG.AutoSize = True
        Me.chkShowFanartJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowFanartJPG.Location = New System.Drawing.Point(6, 38)
        Me.chkShowFanartJPG.Name = "chkShowFanartJPG"
        Me.chkShowFanartJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkShowFanartJPG.TabIndex = 0
        Me.chkShowFanartJPG.Text = "fanart.jpg"
        Me.chkShowFanartJPG.UseVisualStyleBackColor = True
        '
        'gbShowPosters
        '
        Me.gbShowPosters.Controls.Add(Me.chkShowJPG)
        Me.gbShowPosters.Controls.Add(Me.chkShowPosterJPG)
        Me.gbShowPosters.Controls.Add(Me.chkShowFolderJPG)
        Me.gbShowPosters.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.gbShowPosters.Location = New System.Drawing.Point(5, 15)
        Me.gbShowPosters.Name = "gbShowPosters"
        Me.gbShowPosters.Size = New System.Drawing.Size(167, 105)
        Me.gbShowPosters.TabIndex = 0
        Me.gbShowPosters.TabStop = False
        Me.gbShowPosters.Text = "Show Posters"
        '
        'chkShowJPG
        '
        Me.chkShowJPG.AutoSize = True
        Me.chkShowJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowJPG.Location = New System.Drawing.Point(6, 21)
        Me.chkShowJPG.Name = "chkShowJPG"
        Me.chkShowJPG.Size = New System.Drawing.Size(90, 17)
        Me.chkShowJPG.TabIndex = 5
        Me.chkShowJPG.Text = "<show>.jpg"
        Me.chkShowJPG.UseVisualStyleBackColor = True
        '
        'chkShowPosterJPG
        '
        Me.chkShowPosterJPG.AutoSize = True
        Me.chkShowPosterJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowPosterJPG.Location = New System.Drawing.Point(6, 40)
        Me.chkShowPosterJPG.Name = "chkShowPosterJPG"
        Me.chkShowPosterJPG.Size = New System.Drawing.Size(79, 17)
        Me.chkShowPosterJPG.TabIndex = 3
        Me.chkShowPosterJPG.Text = "poster.jpg"
        Me.chkShowPosterJPG.UseVisualStyleBackColor = True
        '
        'chkShowFolderJPG
        '
        Me.chkShowFolderJPG.AutoSize = True
        Me.chkShowFolderJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowFolderJPG.Location = New System.Drawing.Point(6, 61)
        Me.chkShowFolderJPG.Name = "chkShowFolderJPG"
        Me.chkShowFolderJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkShowFolderJPG.TabIndex = 1
        Me.chkShowFolderJPG.Text = "folder.jpg"
        Me.chkShowFolderJPG.UseVisualStyleBackColor = True
        '
        'chkSeasonDotFanart
        '
        Me.chkSeasonDotFanart.AutoSize = True
        Me.chkSeasonDotFanart.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSeasonDotFanart.Location = New System.Drawing.Point(5, 38)
        Me.chkSeasonDotFanart.Name = "chkSeasonDotFanart"
        Me.chkSeasonDotFanart.Size = New System.Drawing.Size(132, 17)
        Me.chkSeasonDotFanart.TabIndex = 2
        Me.chkSeasonDotFanart.Text = "<season>.fanart.jpg"
        Me.chkSeasonDotFanart.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox2.Location = New System.Drawing.Point(5, 57)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(171, 17)
        Me.CheckBox2.TabIndex = 3
        Me.CheckBox2.Text = "<Episode>SxxE01.fanart.jpg"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox3
        '
        Me.CheckBox3.AutoSize = True
        Me.CheckBox3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox3.Location = New System.Drawing.Point(5, 82)
        Me.CheckBox3.Name = "CheckBox3"
        Me.CheckBox3.Size = New System.Drawing.Size(137, 17)
        Me.CheckBox3.TabIndex = 7
        Me.CheckBox3.Text = "<Episode>SxxE01.jpg"
        Me.CheckBox3.UseVisualStyleBackColor = True
        '
        'CheckBox4
        '
        Me.CheckBox4.AutoSize = True
        Me.CheckBox4.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox4.Location = New System.Drawing.Point(6, 42)
        Me.CheckBox4.Name = "CheckBox4"
        Me.CheckBox4.Size = New System.Drawing.Size(132, 17)
        Me.CheckBox4.TabIndex = 2
        Me.CheckBox4.Text = "Season 1.banner.jpg"
        Me.CheckBox4.UseVisualStyleBackColor = True
        '
        'frmTVOutputSettings
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
        Me.Name = "frmTVOutputSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "YAMJ Output Module"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlSettings.ResumeLayout(False)
        Me.gbTVNaming.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
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
    Friend WithEvents gbEpisodeFanart As System.Windows.Forms.GroupBox
    Friend WithEvents chkEpisodeDotFanart As System.Windows.Forms.CheckBox
    Friend WithEvents gbEpisodePosters As System.Windows.Forms.GroupBox
    Friend WithEvents chkEpisodeJPG As System.Windows.Forms.CheckBox
    Friend WithEvents gbSeasonFanart As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeasonFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents gbSeasonPosters As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeasonFolderJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonNameJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents gbShowFanart As System.Windows.Forms.GroupBox
    Friend WithEvents chkShowDotFanart As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents gbShowPosters As System.Windows.Forms.GroupBox
    Friend WithEvents chkShowJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowFolderJPG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkSeasonDotFanart As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox3 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox4 As System.Windows.Forms.CheckBox

End Class
