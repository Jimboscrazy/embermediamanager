<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMediaSettingsHolder
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMediaSettingsHolder))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.GroupBox9 = New System.Windows.Forms.GroupBox
        Me.chkUseMPDB = New System.Windows.Forms.CheckBox
        Me.chkUseTMDB = New System.Windows.Forms.CheckBox
        Me.chkUseIMPA = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lbTrailerSites = New System.Windows.Forms.CheckedListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.chkAutoThumbs = New System.Windows.Forms.CheckBox
        Me.chkSingleScrapeImages = New System.Windows.Forms.CheckBox
        Me.chkDownloadTrailer = New System.Windows.Forms.CheckBox
        Me.cbEnabled = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox9.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 334)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(30, 31)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'GroupBox9
        '
        Me.GroupBox9.Controls.Add(Me.chkUseMPDB)
        Me.GroupBox9.Controls.Add(Me.chkUseTMDB)
        Me.GroupBox9.Controls.Add(Me.chkUseIMPA)
        Me.GroupBox9.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox9.Location = New System.Drawing.Point(11, 117)
        Me.GroupBox9.Name = "GroupBox9"
        Me.GroupBox9.Size = New System.Drawing.Size(184, 100)
        Me.GroupBox9.TabIndex = 71
        Me.GroupBox9.TabStop = False
        Me.GroupBox9.Text = "Get Images From:"
        '
        'chkUseMPDB
        '
        Me.chkUseMPDB.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseMPDB.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseMPDB.Location = New System.Drawing.Point(6, 56)
        Me.chkUseMPDB.Name = "chkUseMPDB"
        Me.chkUseMPDB.Size = New System.Drawing.Size(150, 22)
        Me.chkUseMPDB.TabIndex = 2
        Me.chkUseMPDB.Text = "MoviePostersDB"
        Me.chkUseMPDB.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseMPDB.UseVisualStyleBackColor = True
        '
        'chkUseTMDB
        '
        Me.chkUseTMDB.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseTMDB.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseTMDB.Location = New System.Drawing.Point(6, 18)
        Me.chkUseTMDB.Name = "chkUseTMDB"
        Me.chkUseTMDB.Size = New System.Drawing.Size(149, 19)
        Me.chkUseTMDB.TabIndex = 0
        Me.chkUseTMDB.Text = "themoviedb.org"
        Me.chkUseTMDB.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseTMDB.UseVisualStyleBackColor = True
        '
        'chkUseIMPA
        '
        Me.chkUseIMPA.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseIMPA.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseIMPA.Location = New System.Drawing.Point(6, 37)
        Me.chkUseIMPA.Name = "chkUseIMPA"
        Me.chkUseIMPA.Size = New System.Drawing.Size(149, 20)
        Me.chkUseIMPA.TabIndex = 1
        Me.chkUseIMPA.Text = "IMPAwards"
        Me.chkUseIMPA.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkUseIMPA.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lbTrailerSites)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(201, 117)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(161, 100)
        Me.GroupBox2.TabIndex = 72
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Supported Trailer Sites:"
        '
        'lbTrailerSites
        '
        Me.lbTrailerSites.CheckOnClick = True
        Me.lbTrailerSites.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lbTrailerSites.FormattingEnabled = True
        Me.lbTrailerSites.Items.AddRange(New Object() {"YouTube/AllHTPC", "YouTube/TMDB", "IMDB"})
        Me.lbTrailerSites.Location = New System.Drawing.Point(6, 18)
        Me.lbTrailerSites.Name = "lbTrailerSites"
        Me.lbTrailerSites.Size = New System.Drawing.Size(149, 72)
        Me.lbTrailerSites.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Blue
        Me.Label1.Location = New System.Drawing.Point(37, 336)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(225, 31)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "This are Module specific Settings, and act as a filter." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "You should check Ember G" & _
            "lobal Setting also."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'CheckBox1
        '
        Me.CheckBox1.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.CheckBox1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBox1.Location = New System.Drawing.Point(10, 50)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(84, 16)
        Me.CheckBox1.TabIndex = 76
        Me.CheckBox1.Text = "Get Fanart"
        Me.CheckBox1.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'chkAutoThumbs
        '
        Me.chkAutoThumbs.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoThumbs.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAutoThumbs.Location = New System.Drawing.Point(10, 72)
        Me.chkAutoThumbs.Name = "chkAutoThumbs"
        Me.chkAutoThumbs.Size = New System.Drawing.Size(205, 16)
        Me.chkAutoThumbs.TabIndex = 75
        Me.chkAutoThumbs.Text = "Automatically Extract Extrathumbs"
        Me.chkAutoThumbs.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkAutoThumbs.UseVisualStyleBackColor = True
        '
        'chkSingleScrapeImages
        '
        Me.chkSingleScrapeImages.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSingleScrapeImages.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkSingleScrapeImages.Location = New System.Drawing.Point(10, 32)
        Me.chkSingleScrapeImages.Name = "chkSingleScrapeImages"
        Me.chkSingleScrapeImages.Size = New System.Drawing.Size(173, 15)
        Me.chkSingleScrapeImages.TabIndex = 74
        Me.chkSingleScrapeImages.Text = "Get Posters"
        Me.chkSingleScrapeImages.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkSingleScrapeImages.UseVisualStyleBackColor = True
        '
        'chkDownloadTrailer
        '
        Me.chkDownloadTrailer.AutoSize = True
        Me.chkDownloadTrailer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDownloadTrailer.Location = New System.Drawing.Point(10, 94)
        Me.chkDownloadTrailer.Name = "chkDownloadTrailer"
        Me.chkDownloadTrailer.Size = New System.Drawing.Size(169, 17)
        Me.chkDownloadTrailer.TabIndex = 73
        Me.chkDownloadTrailer.Text = "Enable Trailer Downloading"
        Me.chkDownloadTrailer.UseVisualStyleBackColor = True
        '
        'cbEnabled
        '
        Me.cbEnabled.AutoSize = True
        Me.cbEnabled.Location = New System.Drawing.Point(10, 5)
        Me.cbEnabled.Name = "cbEnabled"
        Me.cbEnabled.Size = New System.Drawing.Size(65, 17)
        Me.cbEnabled.TabIndex = 81
        Me.cbEnabled.Text = "Enabled"
        Me.cbEnabled.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.cbEnabled)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(555, 25)
        Me.Panel1.TabIndex = 82
        '
        'frmMediaSettingsHolder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(553, 368)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.chkAutoThumbs)
        Me.Controls.Add(Me.chkSingleScrapeImages)
        Me.Controls.Add(Me.chkDownloadTrailer)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox9)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMediaSettingsHolder"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Scraper Setup"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox9.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox9 As System.Windows.Forms.GroupBox
    Friend WithEvents chkUseMPDB As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseTMDB As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseIMPA As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lbTrailerSites As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents chkDownloadTrailer As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoThumbs As System.Windows.Forms.CheckBox
    Friend WithEvents chkSingleScrapeImages As System.Windows.Forms.CheckBox
    Friend WithEvents cbEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel

End Class
