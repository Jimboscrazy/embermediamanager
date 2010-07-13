<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMovieInputSettings
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.chkEnabled = New System.Windows.Forms.CheckBox
        Me.pnlSettings = New System.Windows.Forms.Panel
        Me.GroupBox8 = New System.Windows.Forms.GroupBox
        Me.GroupBox21 = New System.Windows.Forms.GroupBox
        Me.rbBracketTrailer = New System.Windows.Forms.RadioButton
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.chkMovieNameDotFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkFanartJPG = New System.Windows.Forms.CheckBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.chkFolderJPG = New System.Windows.Forms.CheckBox
        Me.chkPosterJPG = New System.Windows.Forms.CheckBox
        Me.chkMovieNameJPG = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        Me.pnlSettings.SuspendLayout()
        Me.GroupBox8.SuspendLayout()
        Me.GroupBox21.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
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
        Me.chkEnabled.Location = New System.Drawing.Point(10, 5)
        Me.chkEnabled.Name = "chkEnabled"
        Me.chkEnabled.Size = New System.Drawing.Size(68, 17)
        Me.chkEnabled.TabIndex = 80
        Me.chkEnabled.Text = "Enabled"
        Me.chkEnabled.UseVisualStyleBackColor = True
        '
        'pnlSettings
        '
        Me.pnlSettings.Controls.Add(Me.GroupBox8)
        Me.pnlSettings.Controls.Add(Me.Panel1)
        Me.pnlSettings.Location = New System.Drawing.Point(3, 12)
        Me.pnlSettings.Name = "pnlSettings"
        Me.pnlSettings.Size = New System.Drawing.Size(617, 400)
        Me.pnlSettings.TabIndex = 83
        '
        'GroupBox8
        '
        Me.GroupBox8.Controls.Add(Me.GroupBox21)
        Me.GroupBox8.Controls.Add(Me.GroupBox6)
        Me.GroupBox8.Controls.Add(Me.GroupBox5)
        Me.GroupBox8.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GroupBox8.Location = New System.Drawing.Point(3, 31)
        Me.GroupBox8.Name = "GroupBox8"
        Me.GroupBox8.Size = New System.Drawing.Size(424, 145)
        Me.GroupBox8.TabIndex = 81
        Me.GroupBox8.TabStop = False
        Me.GroupBox8.Text = "File Naming"
        '
        'GroupBox21
        '
        Me.GroupBox21.Controls.Add(Me.rbBracketTrailer)
        Me.GroupBox21.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GroupBox21.Location = New System.Drawing.Point(289, 21)
        Me.GroupBox21.Name = "GroupBox21"
        Me.GroupBox21.Size = New System.Drawing.Size(126, 117)
        Me.GroupBox21.TabIndex = 3
        Me.GroupBox21.TabStop = False
        Me.GroupBox21.Text = "Trailer"
        '
        'rbBracketTrailer
        '
        Me.rbBracketTrailer.AutoSize = True
        Me.rbBracketTrailer.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rbBracketTrailer.Location = New System.Drawing.Point(6, 17)
        Me.rbBracketTrailer.Name = "rbBracketTrailer"
        Me.rbBracketTrailer.Size = New System.Drawing.Size(107, 17)
        Me.rbBracketTrailer.TabIndex = 1
        Me.rbBracketTrailer.TabStop = True
        Me.rbBracketTrailer.Text = "<movie>[trailer]"
        Me.rbBracketTrailer.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.chkMovieNameDotFanartJPG)
        Me.GroupBox6.Controls.Add(Me.chkFanartJPG)
        Me.GroupBox6.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(138, 21)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(136, 117)
        Me.GroupBox6.TabIndex = 2
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Fanart"
        '
        'chkMovieNameDotFanartJPG
        '
        Me.chkMovieNameDotFanartJPG.AutoSize = True
        Me.chkMovieNameDotFanartJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMovieNameDotFanartJPG.Location = New System.Drawing.Point(4, 18)
        Me.chkMovieNameDotFanartJPG.Name = "chkMovieNameDotFanartJPG"
        Me.chkMovieNameDotFanartJPG.Size = New System.Drawing.Size(126, 17)
        Me.chkMovieNameDotFanartJPG.TabIndex = 2
        Me.chkMovieNameDotFanartJPG.Text = "<movie>.fanart.jpg"
        Me.chkMovieNameDotFanartJPG.UseVisualStyleBackColor = True
        '
        'chkFanartJPG
        '
        Me.chkFanartJPG.AutoSize = True
        Me.chkFanartJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFanartJPG.Location = New System.Drawing.Point(4, 38)
        Me.chkFanartJPG.Name = "chkFanartJPG"
        Me.chkFanartJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkFanartJPG.TabIndex = 0
        Me.chkFanartJPG.Text = "fanart.jpg"
        Me.chkFanartJPG.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.chkFolderJPG)
        Me.GroupBox5.Controls.Add(Me.chkPosterJPG)
        Me.GroupBox5.Controls.Add(Me.chkMovieNameJPG)
        Me.GroupBox5.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.GroupBox5.Location = New System.Drawing.Point(8, 21)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(121, 117)
        Me.GroupBox5.TabIndex = 0
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Posters"
        '
        'chkFolderJPG
        '
        Me.chkFolderJPG.AutoSize = True
        Me.chkFolderJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkFolderJPG.Location = New System.Drawing.Point(6, 58)
        Me.chkFolderJPG.Name = "chkFolderJPG"
        Me.chkFolderJPG.Size = New System.Drawing.Size(77, 17)
        Me.chkFolderJPG.TabIndex = 3
        Me.chkFolderJPG.Text = "folder.jpg"
        Me.chkFolderJPG.UseVisualStyleBackColor = True
        '
        'chkPosterJPG
        '
        Me.chkPosterJPG.AutoSize = True
        Me.chkPosterJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkPosterJPG.Location = New System.Drawing.Point(6, 38)
        Me.chkPosterJPG.Name = "chkPosterJPG"
        Me.chkPosterJPG.Size = New System.Drawing.Size(79, 17)
        Me.chkPosterJPG.TabIndex = 6
        Me.chkPosterJPG.Text = "poster.jpg"
        Me.chkPosterJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameJPG
        '
        Me.chkMovieNameJPG.AutoSize = True
        Me.chkMovieNameJPG.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkMovieNameJPG.Location = New System.Drawing.Point(6, 18)
        Me.chkMovieNameJPG.Name = "chkMovieNameJPG"
        Me.chkMovieNameJPG.Size = New System.Drawing.Size(92, 17)
        Me.chkMovieNameJPG.TabIndex = 5
        Me.chkMovieNameJPG.Text = "<movie>.jpg"
        Me.chkMovieNameJPG.UseVisualStyleBackColor = True
        '
        'frmMovieInputSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(625, 415)
        Me.Controls.Add(Me.pnlSettings)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMovieInputSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "YAMJ Input Module"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlSettings.ResumeLayout(False)
        Me.GroupBox8.ResumeLayout(False)
        Me.GroupBox21.ResumeLayout(False)
        Me.GroupBox21.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents chkEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents pnlSettings As System.Windows.Forms.Panel
    Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox21 As System.Windows.Forms.GroupBox
    Friend WithEvents rbBracketTrailer As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMovieNameDotFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents chkFolderJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameJPG As System.Windows.Forms.CheckBox

End Class
