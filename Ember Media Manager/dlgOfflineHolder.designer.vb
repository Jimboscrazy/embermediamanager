<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgOfflineHolder
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgOfflineHolder))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.cbSources = New System.Windows.Forms.ComboBox
        Me.lblSources = New System.Windows.Forms.Label
        Me.txtMovieName = New System.Windows.Forms.TextBox
        Me.lblMovie = New System.Windows.Forms.Label
        Me.GetIMDB_Button = New System.Windows.Forms.Button
        Me.EditMovie_Button = New System.Windows.Forms.Button
        Me.pbProgress = New System.Windows.Forms.ProgressBar
        Me.lvStatus = New System.Windows.Forms.ListView
        Me.colCondition = New System.Windows.Forms.ColumnHeader
        Me.colStatus = New System.Windows.Forms.ColumnHeader
        Me.Create_Button = New System.Windows.Forms.Button
        Me.cbUseFanart = New System.Windows.Forms.CheckBox
        Me.lblTagline = New System.Windows.Forms.Label
        Me.txtTagline = New System.Windows.Forms.TextBox
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.OK_Button.Location = New System.Drawing.Point(245, 427)
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
        Me.pnlTop.Size = New System.Drawing.Size(341, 64)
        Me.pnlTop.TabIndex = 58
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(61, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Add Offline movie"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(58, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(278, 29)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Offline Media Manager"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 7)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(36, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'cbSources
        '
        Me.cbSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSources.FormattingEnabled = True
        Me.cbSources.Location = New System.Drawing.Point(21, 87)
        Me.cbSources.Name = "cbSources"
        Me.cbSources.Size = New System.Drawing.Size(303, 21)
        Me.cbSources.TabIndex = 59
        '
        'lblSources
        '
        Me.lblSources.AutoSize = True
        Me.lblSources.Location = New System.Drawing.Point(20, 71)
        Me.lblSources.Name = "lblSources"
        Me.lblSources.Size = New System.Drawing.Size(61, 13)
        Me.lblSources.TabIndex = 60
        Me.lblSources.Text = "Folder Path"
        '
        'txtMovieName
        '
        Me.txtMovieName.Location = New System.Drawing.Point(21, 135)
        Me.txtMovieName.Name = "txtMovieName"
        Me.txtMovieName.Size = New System.Drawing.Size(303, 20)
        Me.txtMovieName.TabIndex = 61
        '
        'lblMovie
        '
        Me.lblMovie.AutoSize = True
        Me.lblMovie.Location = New System.Drawing.Point(18, 118)
        Me.lblMovie.Name = "lblMovie"
        Me.lblMovie.Size = New System.Drawing.Size(135, 13)
        Me.lblMovie.TabIndex = 62
        Me.lblMovie.Text = "Movie (Place Holder folder)"
        '
        'GetIMDB_Button
        '
        Me.GetIMDB_Button.Location = New System.Drawing.Point(21, 158)
        Me.GetIMDB_Button.Name = "GetIMDB_Button"
        Me.GetIMDB_Button.Size = New System.Drawing.Size(80, 21)
        Me.GetIMDB_Button.TabIndex = 63
        Me.GetIMDB_Button.Text = "Search IMDB"
        '
        'EditMovie_Button
        '
        Me.EditMovie_Button.Enabled = False
        Me.EditMovie_Button.Location = New System.Drawing.Point(107, 158)
        Me.EditMovie_Button.Name = "EditMovie_Button"
        Me.EditMovie_Button.Size = New System.Drawing.Size(80, 21)
        Me.EditMovie_Button.TabIndex = 64
        Me.EditMovie_Button.Text = "Edit Info"
        Me.EditMovie_Button.Visible = False
        '
        'pbProgress
        '
        Me.pbProgress.Location = New System.Drawing.Point(22, 254)
        Me.pbProgress.MarqueeAnimationSpeed = 25
        Me.pbProgress.Name = "pbProgress"
        Me.pbProgress.Size = New System.Drawing.Size(301, 20)
        Me.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pbProgress.TabIndex = 65
        Me.pbProgress.Visible = False
        '
        'lvStatus
        '
        Me.lvStatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCondition, Me.colStatus})
        Me.lvStatus.FullRowSelect = True
        Me.lvStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvStatus.Location = New System.Drawing.Point(21, 279)
        Me.lvStatus.MultiSelect = False
        Me.lvStatus.Name = "lvStatus"
        Me.lvStatus.Size = New System.Drawing.Size(303, 142)
        Me.lvStatus.TabIndex = 66
        Me.lvStatus.UseCompatibleStateImageBehavior = False
        Me.lvStatus.View = System.Windows.Forms.View.Details
        '
        'colCondition
        '
        Me.colCondition.Text = "Condition"
        Me.colCondition.Width = 236
        '
        'colStatus
        '
        Me.colStatus.Text = "Status"
        Me.colStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Create_Button
        '
        Me.Create_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Create_Button.Enabled = False
        Me.Create_Button.Location = New System.Drawing.Point(159, 427)
        Me.Create_Button.Name = "Create_Button"
        Me.Create_Button.Size = New System.Drawing.Size(80, 23)
        Me.Create_Button.TabIndex = 67
        Me.Create_Button.Text = "Create"
        '
        'cbUseFanart
        '
        Me.cbUseFanart.AutoSize = True
        Me.cbUseFanart.Location = New System.Drawing.Point(21, 185)
        Me.cbUseFanart.Name = "cbUseFanart"
        Me.cbUseFanart.Size = New System.Drawing.Size(189, 17)
        Me.cbUseFanart.TabIndex = 68
        Me.cbUseFanart.Text = "Use Fanart for Place Holder Movie"
        Me.cbUseFanart.UseVisualStyleBackColor = True
        '
        'lblTagline
        '
        Me.lblTagline.AutoSize = True
        Me.lblTagline.Location = New System.Drawing.Point(17, 210)
        Me.lblTagline.Name = "lblTagline"
        Me.lblTagline.Size = New System.Drawing.Size(74, 13)
        Me.lblTagline.TabIndex = 70
        Me.lblTagline.Text = "Movie Tagline"
        '
        'txtTagline
        '
        Me.txtTagline.Location = New System.Drawing.Point(20, 227)
        Me.txtTagline.Name = "txtTagline"
        Me.txtTagline.Size = New System.Drawing.Size(303, 20)
        Me.txtTagline.TabIndex = 69
        Me.txtTagline.Text = "Insert DVD"
        '
        'dlgOfflineHolder
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(341, 453)
        Me.Controls.Add(Me.lblTagline)
        Me.Controls.Add(Me.txtTagline)
        Me.Controls.Add(Me.cbUseFanart)
        Me.Controls.Add(Me.Create_Button)
        Me.Controls.Add(Me.lvStatus)
        Me.Controls.Add(Me.pbProgress)
        Me.Controls.Add(Me.EditMovie_Button)
        Me.Controls.Add(Me.GetIMDB_Button)
        Me.Controls.Add(Me.lblMovie)
        Me.Controls.Add(Me.txtMovieName)
        Me.Controls.Add(Me.lblSources)
        Me.Controls.Add(Me.cbSources)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.OK_Button)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgOfflineHolder"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "OfflienMedia Manager"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents cbSources As System.Windows.Forms.ComboBox
    Friend WithEvents lblSources As System.Windows.Forms.Label
    Friend WithEvents txtMovieName As System.Windows.Forms.TextBox
    Friend WithEvents lblMovie As System.Windows.Forms.Label
    Friend WithEvents GetIMDB_Button As System.Windows.Forms.Button
    Friend WithEvents EditMovie_Button As System.Windows.Forms.Button
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents lvStatus As System.Windows.Forms.ListView
    Friend WithEvents colCondition As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents Create_Button As System.Windows.Forms.Button
    Friend WithEvents cbUseFanart As System.Windows.Forms.CheckBox
    Friend WithEvents lblTagline As System.Windows.Forms.Label
    Friend WithEvents txtTagline As System.Windows.Forms.TextBox

End Class
