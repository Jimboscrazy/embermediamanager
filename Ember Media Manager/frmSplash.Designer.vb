﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSplash
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
        Me.Version = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.pbLoading = New System.Windows.Forms.ProgressBar
        Me.txtStage = New System.Windows.Forms.Label
        Me.Release = New System.Windows.Forms.Label
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Version
        '
        Me.Version.BackColor = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(10, Byte), Integer), CType(CType(10, Byte), Integer))
        Me.Version.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Version.ForeColor = System.Drawing.Color.White
        Me.Version.Location = New System.Drawing.Point(222, 5)
        Me.Version.Name = "Version"
        Me.Version.Size = New System.Drawing.Size(83, 11)
        Me.Version.TabIndex = 2
        Me.Version.Text = "Version X.X.XXXX"
        Me.Version.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = Global.Ember_Media_Manager.My.Resources.Resources.Splash
        Me.PictureBox1.Location = New System.Drawing.Point(4, 4)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(302, 304)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'pbLoading
        '
        Me.pbLoading.Location = New System.Drawing.Point(5, 295)
        Me.pbLoading.Maximum = 9
        Me.pbLoading.Name = "pbLoading"
        Me.pbLoading.Size = New System.Drawing.Size(300, 12)
        Me.pbLoading.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbLoading.TabIndex = 4
        '
        'txtStage
        '
        Me.txtStage.BackColor = System.Drawing.Color.Black
        Me.txtStage.Font = New System.Drawing.Font("Segoe UI", 6.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStage.ForeColor = System.Drawing.Color.White
        Me.txtStage.Location = New System.Drawing.Point(8, 283)
        Me.txtStage.Name = "txtStage"
        Me.txtStage.Size = New System.Drawing.Size(175, 11)
        Me.txtStage.TabIndex = 3
        Me.txtStage.Text = "Loading settings..."
        Me.txtStage.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Release
        '
        Me.Release.BackColor = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(10, Byte), Integer), CType(CType(10, Byte), Integer))
        Me.Release.Font = New System.Drawing.Font("Segoe UI", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Release.ForeColor = System.Drawing.Color.White
        Me.Release.Location = New System.Drawing.Point(194, 272)
        Me.Release.Name = "Release"
        Me.Release.Size = New System.Drawing.Size(89, 13)
        Me.Release.TabIndex = 5
        Me.Release.Text = "Release"
        Me.Release.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmSplash
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(310, 312)
        Me.ControlBox = False
        Me.Controls.Add(Me.Release)
        Me.Controls.Add(Me.pbLoading)
        Me.Controls.Add(Me.txtStage)
        Me.Controls.Add(Me.Version)
        Me.Controls.Add(Me.PictureBox1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSplash"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Version As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents pbLoading As System.Windows.Forms.ProgressBar
    Friend WithEvents txtStage As System.Windows.Forms.Label
    Friend WithEvents Release As System.Windows.Forms.Label

End Class
