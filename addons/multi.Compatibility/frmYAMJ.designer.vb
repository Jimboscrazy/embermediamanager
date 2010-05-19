<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmYAMJ
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
        Me.pnlSettings = New System.Windows.Forms.Panel
        Me.chkYAMJCompatibleTVImages = New System.Windows.Forms.CheckBox
        Me.chkVideoTSParent = New System.Windows.Forms.CheckBox
        Me.chkYAMJCompatibleSets = New System.Windows.Forms.CheckBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkEnabled = New System.Windows.Forms.CheckBox
        Me.btnCheckAll = New System.Windows.Forms.Button
        Me.chkYAMJnfoFields = New System.Windows.Forms.CheckBox
        Me.pnlSettings.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlSettings
        '
        Me.pnlSettings.BackColor = System.Drawing.Color.White
        Me.pnlSettings.Controls.Add(Me.chkYAMJnfoFields)
        Me.pnlSettings.Controls.Add(Me.chkYAMJCompatibleTVImages)
        Me.pnlSettings.Controls.Add(Me.chkVideoTSParent)
        Me.pnlSettings.Controls.Add(Me.chkYAMJCompatibleSets)
        Me.pnlSettings.Controls.Add(Me.Panel1)
        Me.pnlSettings.Location = New System.Drawing.Point(13, 15)
        Me.pnlSettings.Name = "pnlSettings"
        Me.pnlSettings.Size = New System.Drawing.Size(617, 327)
        Me.pnlSettings.TabIndex = 84
        '
        'chkYAMJCompatibleTVImages
        '
        Me.chkYAMJCompatibleTVImages.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkYAMJCompatibleTVImages.Location = New System.Drawing.Point(6, 75)
        Me.chkYAMJCompatibleTVImages.Name = "chkYAMJCompatibleTVImages"
        Me.chkYAMJCompatibleTVImages.Size = New System.Drawing.Size(594, 18)
        Me.chkYAMJCompatibleTVImages.TabIndex = 85
        Me.chkYAMJCompatibleTVImages.Text = "YAMJ Compatible TV Images Naming"
        Me.chkYAMJCompatibleTVImages.UseVisualStyleBackColor = True
        '
        'chkVideoTSParent
        '
        Me.chkVideoTSParent.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkVideoTSParent.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkVideoTSParent.Location = New System.Drawing.Point(6, 52)
        Me.chkVideoTSParent.Name = "chkVideoTSParent"
        Me.chkVideoTSParent.Size = New System.Drawing.Size(594, 17)
        Me.chkVideoTSParent.TabIndex = 84
        Me.chkVideoTSParent.Text = "YAMJ Compatible VIDEO_TS File Placement/Naming"
        Me.chkVideoTSParent.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.chkVideoTSParent.UseVisualStyleBackColor = True
        '
        'chkYAMJCompatibleSets
        '
        Me.chkYAMJCompatibleSets.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkYAMJCompatibleSets.Location = New System.Drawing.Point(6, 29)
        Me.chkYAMJCompatibleSets.Name = "chkYAMJCompatibleSets"
        Me.chkYAMJCompatibleSets.Size = New System.Drawing.Size(594, 17)
        Me.chkYAMJCompatibleSets.TabIndex = 83
        Me.chkYAMJCompatibleSets.Text = "YAMJ Compatible Movie Sets"
        Me.chkYAMJCompatibleSets.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.btnCheckAll)
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
        Me.chkEnabled.Location = New System.Drawing.Point(546, 5)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.Size = New System.Drawing.Size(68, 17)
        Me.chkEnabled.TabIndex = 80
        Me.chkEnabled.Text = "Enabled"
        Me.chkEnabled.UseVisualStyleBackColor = True
        Me.chkEnabled.Visible = False
        '
        'btnCheckAll
        '
        Me.btnCheckAll.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCheckAll.Location = New System.Drawing.Point(6, 3)
        Me.btnCheckAll.Name = "btnCheckAll"
        Me.btnCheckAll.Size = New System.Drawing.Size(93, 20)
        Me.btnCheckAll.TabIndex = 86
        Me.btnCheckAll.Text = "Check All"
        Me.btnCheckAll.UseVisualStyleBackColor = True
        '
        'chkYAMJnfoFields
        '
        Me.chkYAMJnfoFields.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkYAMJnfoFields.Location = New System.Drawing.Point(6, 99)
        Me.chkYAMJnfoFields.Name = "chkYAMJnfoFields"
        Me.chkYAMJnfoFields.Size = New System.Drawing.Size(594, 18)
        Me.chkYAMJnfoFields.TabIndex = 86
        Me.chkYAMJnfoFields.Text = "YAMJ Specific NFO fields"
        Me.chkYAMJnfoFields.UseVisualStyleBackColor = True
        '
        'frmYAMJ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(643, 356)
        Me.Controls.Add(Me.pnlSettings)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmYAMJ"
        Me.Text = "frmSettingsHolder"
        Me.pnlSettings.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlSettings As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents chkYAMJCompatibleSets As System.Windows.Forms.CheckBox
    Friend WithEvents chkYAMJCompatibleTVImages As System.Windows.Forms.CheckBox
    Friend WithEvents chkVideoTSParent As System.Windows.Forms.CheckBox
    Friend WithEvents btnCheckAll As System.Windows.Forms.Button
    Friend WithEvents chkYAMJnfoFields As System.Windows.Forms.CheckBox
End Class
