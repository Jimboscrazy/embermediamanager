﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgAddonFile
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
        Me.btnBrowse = New System.Windows.Forms.Button
        Me.txtPath = New System.Windows.Forms.TextBox
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblFile = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Location = New System.Drawing.Point(289, 85)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(362, 85)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(405, 11)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowse.TabIndex = 20
        Me.btnBrowse.Text = "..."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(104, 12)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.Size = New System.Drawing.Size(298, 22)
        Me.txtPath.TabIndex = 19
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(104, 37)
        Me.txtDescription.MaxLength = 150
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(325, 39)
        Me.txtDescription.TabIndex = 21
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(12, 37)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(86, 13)
        Me.lblDescription.TabIndex = 22
        Me.lblDescription.Text = "Description:"
        Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblFile
        '
        Me.lblFile.Location = New System.Drawing.Point(12, 16)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(86, 13)
        Me.lblFile.TabIndex = 23
        Me.lblFile.Text = "File:"
        Me.lblFile.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'dlgAddonFile
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(435, 114)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.btnBrowse)
        Me.Controls.Add(Me.txtPath)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgAddonFile"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Addon File"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblFile As System.Windows.Forms.Label

End Class
