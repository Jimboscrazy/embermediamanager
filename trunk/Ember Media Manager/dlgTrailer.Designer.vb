<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgTrailer
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
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.lbTrailers = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lstFormats = New System.Windows.Forms.ListBox
        Me.txtYouTube = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnFetchFormats = New System.Windows.Forms.Button
        Me.pnlStatus = New System.Windows.Forms.Panel
        Me.lblStatus = New System.Windows.Forms.Label
        Me.pbStatus = New System.Windows.Forms.ProgressBar
        Me.btnPlayTrailer = New System.Windows.Forms.Button
        Me.btnSetNfo = New System.Windows.Forms.Button
        Me.txtManual = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.ofdTrailer = New System.Windows.Forms.OpenFileDialog
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.pnlStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.Enabled = False
        Me.OK_Button.Location = New System.Drawing.Point(296, 390)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Download"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Enabled = False
        Me.Cancel_Button.Location = New System.Drawing.Point(369, 390)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'lbTrailers
        '
        Me.lbTrailers.FormattingEnabled = True
        Me.lbTrailers.HorizontalScrollbar = True
        Me.lbTrailers.Location = New System.Drawing.Point(6, 19)
        Me.lbTrailers.Name = "lbTrailers"
        Me.lbTrailers.Size = New System.Drawing.Size(411, 173)
        Me.lbTrailers.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.pnlStatus)
        Me.GroupBox1.Controls.Add(Me.lbTrailers)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(424, 372)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Trailer to Download"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnBrowse)
        Me.GroupBox2.Controls.Add(Me.txtManual)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.lstFormats)
        Me.GroupBox2.Controls.Add(Me.txtYouTube)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.btnFetchFormats)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 201)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(411, 165)
        Me.GroupBox2.TabIndex = 70
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Manual Trailer Entry"
        '
        'lstFormats
        '
        Me.lstFormats.FormattingEnabled = True
        Me.lstFormats.Location = New System.Drawing.Point(106, 50)
        Me.lstFormats.Name = "lstFormats"
        Me.lstFormats.Size = New System.Drawing.Size(78, 69)
        Me.lstFormats.TabIndex = 2
        '
        'txtYouTube
        '
        Me.txtYouTube.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtYouTube.Location = New System.Drawing.Point(9, 28)
        Me.txtYouTube.Name = "txtYouTube"
        Me.txtYouTube.Size = New System.Drawing.Size(392, 20)
        Me.txtYouTube.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "YouTube URL:"
        '
        'btnFetchFormats
        '
        Me.btnFetchFormats.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnFetchFormats.Enabled = False
        Me.btnFetchFormats.Location = New System.Drawing.Point(9, 49)
        Me.btnFetchFormats.Name = "btnFetchFormats"
        Me.btnFetchFormats.Size = New System.Drawing.Size(93, 23)
        Me.btnFetchFormats.TabIndex = 0
        Me.btnFetchFormats.Text = "Fetch Formats"
        '
        'pnlStatus
        '
        Me.pnlStatus.BackColor = System.Drawing.Color.White
        Me.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlStatus.Controls.Add(Me.lblStatus)
        Me.pnlStatus.Controls.Add(Me.pbStatus)
        Me.pnlStatus.Location = New System.Drawing.Point(112, 82)
        Me.pnlStatus.Name = "pnlStatus"
        Me.pnlStatus.Size = New System.Drawing.Size(200, 54)
        Me.pnlStatus.TabIndex = 69
        Me.pnlStatus.Visible = False
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(3, 10)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(104, 13)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "Compiling trailer list..."
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'pbStatus
        '
        Me.pbStatus.Location = New System.Drawing.Point(3, 32)
        Me.pbStatus.MarqueeAnimationSpeed = 25
        Me.pbStatus.Name = "pbStatus"
        Me.pbStatus.Size = New System.Drawing.Size(192, 17)
        Me.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.pbStatus.TabIndex = 0
        '
        'btnPlayTrailer
        '
        Me.btnPlayTrailer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnPlayTrailer.Enabled = False
        Me.btnPlayTrailer.Image = Global.Ember_Media_Manager.My.Resources.Resources.Play_Icon
        Me.btnPlayTrailer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnPlayTrailer.Location = New System.Drawing.Point(12, 390)
        Me.btnPlayTrailer.Name = "btnPlayTrailer"
        Me.btnPlayTrailer.Size = New System.Drawing.Size(100, 23)
        Me.btnPlayTrailer.TabIndex = 109
        Me.btnPlayTrailer.Text = "Preview Trailer"
        Me.btnPlayTrailer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnPlayTrailer.UseVisualStyleBackColor = True
        '
        'btnSetNfo
        '
        Me.btnSetNfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSetNfo.Enabled = False
        Me.btnSetNfo.Location = New System.Drawing.Point(223, 390)
        Me.btnSetNfo.Name = "btnSetNfo"
        Me.btnSetNfo.Size = New System.Drawing.Size(67, 23)
        Me.btnSetNfo.TabIndex = 110
        Me.btnSetNfo.Text = "Set To Nfo"
        '
        'txtManual
        '
        Me.txtManual.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtManual.Location = New System.Drawing.Point(9, 139)
        Me.txtManual.Name = "txtManual"
        Me.txtManual.Size = New System.Drawing.Size(365, 20)
        Me.txtManual.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 125)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Local Trailer:"
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(376, 138)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(25, 23)
        Me.btnBrowse.TabIndex = 5
        Me.btnBrowse.Text = "..."
        '
        'dlgTrailer
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(450, 415)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnSetNfo)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.btnPlayTrailer)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgTrailer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Trailer"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.pnlStatus.ResumeLayout(False)
        Me.pnlStatus.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lbTrailers As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents pnlStatus As System.Windows.Forms.Panel
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents pbStatus As System.Windows.Forms.ProgressBar
    Friend WithEvents btnPlayTrailer As System.Windows.Forms.Button
    Friend WithEvents btnSetNfo As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtYouTube As System.Windows.Forms.TextBox
    Friend WithEvents lstFormats As System.Windows.Forms.ListBox
    Friend WithEvents btnFetchFormats As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtManual As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ofdTrailer As System.Windows.Forms.OpenFileDialog

End Class
