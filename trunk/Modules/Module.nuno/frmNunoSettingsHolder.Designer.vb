<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNunoSettingsHolder
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
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
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.pnlSettings.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(436, 296)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
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
        Me.pnlSettings.Location = New System.Drawing.Point(3, 12)
        Me.pnlSettings.Name = "pnlSettings"
        Me.pnlSettings.Size = New System.Drawing.Size(407, 174)
        Me.pnlSettings.TabIndex = 8
        '
        'frmNunoSettingsHolder
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(594, 337)
        Me.Controls.Add(Me.pnlSettings)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNunoSettingsHolder"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Setup"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.pnlSettings.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
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

End Class
