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
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.pnlImages.SuspendLayout()
        CType(Me.pbCurrent, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tvList
        '
        Me.tvList.Location = New System.Drawing.Point(3, 4)
        Me.tvList.Name = "tvList"
        Me.tvList.Size = New System.Drawing.Size(214, 280)
        Me.tvList.TabIndex = 0
        '
        'pnlImages
        '
        Me.pnlImages.AutoScroll = True
        Me.pnlImages.BackColor = System.Drawing.Color.DimGray
        Me.pnlImages.Controls.Add(Me.Label1)
        Me.pnlImages.Location = New System.Drawing.Point(222, 4)
        Me.pnlImages.Name = "pnlImages"
        Me.pnlImages.Size = New System.Drawing.Size(622, 421)
        Me.pnlImages.TabIndex = 1
        '
        'pbCurrent
        '
        Me.pbCurrent.BackColor = System.Drawing.Color.DimGray
        Me.pbCurrent.Location = New System.Drawing.Point(3, 293)
        Me.pbCurrent.Name = "pbCurrent"
        Me.pbCurrent.Size = New System.Drawing.Size(214, 157)
        Me.pbCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCurrent.TabIndex = 2
        Me.pbCurrent.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Location = New System.Drawing.Point(266, 206)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Available images here"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(64, 372)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(99, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Current Image Here"
        '
        'dlgTVImageSelect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(849, 459)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.pbCurrent)
        Me.Controls.Add(Me.pnlImages)
        Me.Controls.Add(Me.tvList)
        Me.Name = "dlgTVImageSelect"
        Me.Text = "dlgTVImageSelect"
        Me.pnlImages.ResumeLayout(False)
        Me.pnlImages.PerformLayout()
        CType(Me.pbCurrent, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tvList As System.Windows.Forms.TreeView
    Friend WithEvents pnlImages As System.Windows.Forms.Panel
    Friend WithEvents pbCurrent As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
