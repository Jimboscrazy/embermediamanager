<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettingsHolder
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
        Me.cLanguage = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.tOutline = New System.Windows.Forms.CheckBox
        Me.tPlot = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.cCheckSubsLanguage = New System.Windows.Forms.CheckBox
        Me.cAddSubMetadata = New System.Windows.Forms.CheckBox
        Me.pnlSettings = New System.Windows.Forms.Panel
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkEnabled = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.pnlSettings.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cLanguage
        '
        Me.cLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cLanguage.FormattingEnabled = True
        Me.cLanguage.Location = New System.Drawing.Point(137, 19)
        Me.cLanguage.Name = "cLanguage"
        Me.cLanguage.Size = New System.Drawing.Size(146, 21)
        Me.cLanguage.Sorted = True
        Me.cLanguage.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(68, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Translate to"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(297, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 12)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "(Using Google API)"
        '
        'tOutline
        '
        Me.tOutline.AutoSize = True
        Me.tOutline.Location = New System.Drawing.Point(3, 12)
        Me.tOutline.Name = "tOutline"
        Me.tOutline.Size = New System.Drawing.Size(59, 17)
        Me.tOutline.TabIndex = 4
        Me.tOutline.Text = "Outline"
        Me.tOutline.UseVisualStyleBackColor = True
        '
        'tPlot
        '
        Me.tPlot.AutoSize = True
        Me.tPlot.Location = New System.Drawing.Point(3, 32)
        Me.tPlot.Name = "tPlot"
        Me.tPlot.Size = New System.Drawing.Size(44, 17)
        Me.tPlot.TabIndex = 5
        Me.tPlot.Text = "Plot"
        Me.tPlot.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cLanguage)
        Me.GroupBox1.Controls.Add(Me.tPlot)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.tOutline)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(396, 54)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cCheckSubsLanguage)
        Me.GroupBox2.Controls.Add(Me.cAddSubMetadata)
        Me.GroupBox2.Location = New System.Drawing.Point(3, 63)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(396, 71)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "*** Still Disabled"
        '
        'cCheckSubsLanguage
        '
        Me.cCheckSubsLanguage.AutoSize = True
        Me.cCheckSubsLanguage.Enabled = False
        Me.cCheckSubsLanguage.Location = New System.Drawing.Point(18, 42)
        Me.cCheckSubsLanguage.Name = "cCheckSubsLanguage"
        Me.cCheckSubsLanguage.Size = New System.Drawing.Size(197, 17)
        Me.cCheckSubsLanguage.TabIndex = 6
        Me.cCheckSubsLanguage.Text = "Retrieve Language from subtitle files"
        Me.cCheckSubsLanguage.UseVisualStyleBackColor = True
        '
        'cAddSubMetadata
        '
        Me.cAddSubMetadata.AutoSize = True
        Me.cAddSubMetadata.Location = New System.Drawing.Point(7, 19)
        Me.cAddSubMetadata.Name = "cAddSubMetadata"
        Me.cAddSubMetadata.Size = New System.Drawing.Size(186, 17)
        Me.cAddSubMetadata.TabIndex = 6
        Me.cAddSubMetadata.Text = "Add external subtitle to Meta Data"
        Me.cAddSubMetadata.UseVisualStyleBackColor = True
        '
        'pnlSettings
        '
        Me.pnlSettings.Controls.Add(Me.GroupBox1)
        Me.pnlSettings.Controls.Add(Me.GroupBox2)
        Me.pnlSettings.Location = New System.Drawing.Point(7, 31)
        Me.pnlSettings.Name = "pnlSettings"
        Me.pnlSettings.Size = New System.Drawing.Size(580, 259)
        Me.pnlSettings.TabIndex = 8
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.chkEnabled)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(594, 25)
        Me.Panel1.TabIndex = 83
        '
        'chkEnabled
        '
        Me.chkEnabled.AutoSize = True
        Me.chkEnabled.Location = New System.Drawing.Point(10, 5)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.Size = New System.Drawing.Size(65, 17)
        Me.chkEnabled.TabIndex = 80
        Me.chkEnabled.Text = "Enabled"
        Me.chkEnabled.UseVisualStyleBackColor = True
        '
        'frmSettingsHolder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 337)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlSettings)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSettingsHolder"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Setup"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.pnlSettings.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents tOutline As System.Windows.Forms.CheckBox
    Friend WithEvents tPlot As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cCheckSubsLanguage As System.Windows.Forms.CheckBox
    Friend WithEvents cAddSubMetadata As System.Windows.Forms.CheckBox
    Friend WithEvents pnlSettings As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkEnabled As System.Windows.Forms.CheckBox

End Class
