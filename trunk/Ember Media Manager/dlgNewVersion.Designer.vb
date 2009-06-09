<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgNewVersion
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgNewVersion))
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.txtChangelog = New System.Windows.Forms.TextBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblNew = New System.Windows.Forms.Label
        Me.llClick = New System.Windows.Forms.LinkLabel
        Me.Label2 = New System.Windows.Forms.Label
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(457, 390)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'txtChangelog
        '
        Me.txtChangelog.AcceptsReturn = True
        Me.txtChangelog.AcceptsTab = True
        Me.txtChangelog.BackColor = System.Drawing.Color.White
        Me.txtChangelog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtChangelog.Location = New System.Drawing.Point(9, 96)
        Me.txtChangelog.Multiline = True
        Me.txtChangelog.Name = "txtChangelog"
        Me.txtChangelog.ReadOnly = True
        Me.txtChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtChangelog.Size = New System.Drawing.Size(515, 282)
        Me.txtChangelog.TabIndex = 1
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Ember_Media_Manager.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(9, 9)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(75, 78)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 10
        Me.PictureBox1.TabStop = False
        '
        'lblNew
        '
        Me.lblNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNew.Location = New System.Drawing.Point(247, 27)
        Me.lblNew.Name = "lblNew"
        Me.lblNew.Size = New System.Drawing.Size(277, 43)
        Me.lblNew.TabIndex = 11
        Me.lblNew.Text = "Version r{0} is now available on Ember Media Manager's Google Code page."
        Me.lblNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'llClick
        '
        Me.llClick.AutoSize = True
        Me.llClick.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llClick.Location = New System.Drawing.Point(5, 391)
        Me.llClick.Name = "llClick"
        Me.llClick.Size = New System.Drawing.Size(91, 20)
        Me.llClick.TabIndex = 12
        Me.llClick.TabStop = True
        Me.llClick.Text = "Click Here"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(93, 391)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(222, 20)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "to visit the Google Code page."
        '
        'dlgNewVersion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(536, 425)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.llClick)
        Me.Controls.Add(Me.lblNew)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.txtChangelog)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgNewVersion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "A New Version Is Available"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents txtChangelog As System.Windows.Forms.TextBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblNew As System.Windows.Forms.Label
    Friend WithEvents llClick As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label

End Class
