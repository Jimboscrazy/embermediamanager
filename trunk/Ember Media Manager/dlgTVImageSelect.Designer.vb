<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgTVImageSelect
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
        Me.tvList = New System.Windows.Forms.TreeView
        Me.pnlImages = New System.Windows.Forms.Panel
        Me.pbCurrent = New System.Windows.Forms.PictureBox
        Me.pnlStatus = New System.Windows.Forms.Panel
        Me.lblStatus = New System.Windows.Forms.Label
        Me.pbStatus = New System.Windows.Forms.ProgressBar
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        CType(Me.pbCurrent, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlStatus.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvList
        '
        Me.tvList.Enabled = False
        Me.tvList.Location = New System.Drawing.Point(3, 4)
        Me.tvList.Name = "tvList"
        Me.tvList.Size = New System.Drawing.Size(214, 280)
        Me.tvList.TabIndex = 0
        Me.tvList.Visible = False
        '
        'pnlImages
        '
        Me.pnlImages.AutoScroll = True
        Me.pnlImages.BackColor = System.Drawing.SystemColors.Control
        Me.pnlImages.Location = New System.Drawing.Point(222, 4)
        Me.pnlImages.Name = "pnlImages"
        Me.pnlImages.Size = New System.Drawing.Size(622, 421)
        Me.pnlImages.TabIndex = 1
        '
        'pbCurrent
        '
        Me.pbCurrent.BackColor = System.Drawing.SystemColors.Control
        Me.pbCurrent.Location = New System.Drawing.Point(3, 293)
        Me.pbCurrent.Name = "pbCurrent"
        Me.pbCurrent.Size = New System.Drawing.Size(214, 157)
        Me.pbCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCurrent.TabIndex = 2
        Me.pbCurrent.TabStop = False
        '
        'pnlStatus
        '
        Me.pnlStatus.BackColor = System.Drawing.Color.White
        Me.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlStatus.Controls.Add(Me.lblStatus)
        Me.pnlStatus.Controls.Add(Me.pbStatus)
        Me.pnlStatus.Location = New System.Drawing.Point(264, 192)
        Me.pnlStatus.Name = "pnlStatus"
        Me.pnlStatus.Size = New System.Drawing.Size(321, 75)
        Me.pnlStatus.TabIndex = 10
        '
        'lblStatus
        '
        Me.lblStatus.Location = New System.Drawing.Point(5, 10)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(310, 13)
        Me.lblStatus.TabIndex = 7
        Me.lblStatus.Text = "Gathering Data..."
        '
        'pbStatus
        '
        Me.pbStatus.Location = New System.Drawing.Point(6, 52)
        Me.pbStatus.MarqueeAnimationSpeed = 25
        Me.pbStatus.Name = "pbStatus"
        Me.pbStatus.Size = New System.Drawing.Size(309, 19)
        Me.pbStatus.Step = 1
        Me.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbStatus.TabIndex = 6
        '
        'btnOK
        '
        Me.btnOK.Enabled = False
        Me.btnOK.Location = New System.Drawing.Point(708, 431)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(65, 22)
        Me.btnOK.TabIndex = 11
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(779, 431)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(65, 22)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'dlgTVImageSelect
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(849, 459)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.pnlStatus)
        Me.Controls.Add(Me.pbCurrent)
        Me.Controls.Add(Me.pnlImages)
        Me.Controls.Add(Me.tvList)
        Me.Name = "dlgTVImageSelect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "dlgTVImageSelect"
        CType(Me.pbCurrent, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlStatus.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tvList As System.Windows.Forms.TreeView
    Friend WithEvents pnlImages As System.Windows.Forms.Panel
    Friend WithEvents pbCurrent As System.Windows.Forms.PictureBox
    Friend WithEvents pnlStatus As System.Windows.Forms.Panel
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents pbStatus As System.Windows.Forms.ProgressBar
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
