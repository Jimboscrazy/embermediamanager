<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgAbout
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

    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblCopyright As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgAbout))
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblCopyright = New System.Windows.Forms.Label
        Me.lblProductName = New System.Windows.Forms.Label
        Me.OKButton = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.pbFFMPEG = New System.Windows.Forms.PictureBox
        Me.pbMI = New System.Windows.Forms.PictureBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.pbIMDB = New System.Windows.Forms.PictureBox
        Me.pbIMPA = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.pbTMDB = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Panel1.SuspendLayout()
        CType(Me.pbFFMPEG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMI, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbIMDB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbIMPA, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTMDB, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblVersion
        '
        Me.lblVersion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblVersion.Location = New System.Drawing.Point(207, 51)
        Me.lblVersion.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(211, 13)
        Me.lblVersion.TabIndex = 0
        Me.lblVersion.Text = "Version"
        Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCopyright
        '
        Me.lblCopyright.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCopyright.Location = New System.Drawing.Point(207, 64)
        Me.lblCopyright.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.lblCopyright.Name = "lblCopyright"
        Me.lblCopyright.Size = New System.Drawing.Size(211, 13)
        Me.lblCopyright.TabIndex = 0
        Me.lblCopyright.Text = "Copyright"
        Me.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblProductName
        '
        Me.lblProductName.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblProductName.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblProductName.Location = New System.Drawing.Point(109, 9)
        Me.lblProductName.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
        Me.lblProductName.Name = "lblProductName"
        Me.lblProductName.Size = New System.Drawing.Size(309, 21)
        Me.lblProductName.TabIndex = 2
        Me.lblProductName.Text = "Product Name"
        Me.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.Location = New System.Drawing.Point(359, 312)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 3
        Me.OKButton.Text = "&OK"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureBox2)
        Me.Panel1.Controls.Add(Me.pbFFMPEG)
        Me.Panel1.Controls.Add(Me.pbMI)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.pbIMDB)
        Me.Panel1.Controls.Add(Me.pbIMPA)
        Me.Panel1.Controls.Add(Me.pbTMDB)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(18, 90)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(416, 216)
        Me.Panel1.TabIndex = 8
        '
        'pbFFMPEG
        '
        Me.pbFFMPEG.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbFFMPEG.Image = CType(resources.GetObject("pbFFMPEG.Image"), System.Drawing.Image)
        Me.pbFFMPEG.Location = New System.Drawing.Point(36, 141)
        Me.pbFFMPEG.Name = "pbFFMPEG"
        Me.pbFFMPEG.Size = New System.Drawing.Size(145, 26)
        Me.pbFFMPEG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFFMPEG.TabIndex = 14
        Me.pbFFMPEG.TabStop = False
        '
        'pbMI
        '
        Me.pbMI.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbMI.Image = CType(resources.GetObject("pbMI.Image"), System.Drawing.Image)
        Me.pbMI.Location = New System.Drawing.Point(330, 173)
        Me.pbMI.Name = "pbMI"
        Me.pbMI.Size = New System.Drawing.Size(78, 38)
        Me.pbMI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbMI.TabIndex = 13
        Me.pbMI.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(126, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 13)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Special Thanks To:"
        '
        'pbIMDB
        '
        Me.pbIMDB.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbIMDB.Image = CType(resources.GetObject("pbIMDB.Image"), System.Drawing.Image)
        Me.pbIMDB.Location = New System.Drawing.Point(114, 173)
        Me.pbIMDB.Name = "pbIMDB"
        Me.pbIMDB.Size = New System.Drawing.Size(78, 38)
        Me.pbIMDB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIMDB.TabIndex = 11
        Me.pbIMDB.TabStop = False
        '
        'pbIMPA
        '
        Me.pbIMPA.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbIMPA.Image = CType(resources.GetObject("pbIMPA.Image"), System.Drawing.Image)
        Me.pbIMPA.Location = New System.Drawing.Point(6, 173)
        Me.pbIMPA.Name = "pbIMPA"
        Me.pbIMPA.Size = New System.Drawing.Size(78, 38)
        Me.pbIMPA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbIMPA.TabIndex = 10
        Me.pbIMPA.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(126, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(189, 91)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Carlos ""asphinx"" Nabb - Genre Images" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & """Halibutt"" - Studio Icon Pack" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & """fekker""" & _
            " - Media Info Plus" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & """billyad2000"" - Media Companion"
        '
        'pbTMDB
        '
        Me.pbTMDB.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbTMDB.Image = CType(resources.GetObject("pbTMDB.Image"), System.Drawing.Image)
        Me.pbTMDB.Location = New System.Drawing.Point(238, 143)
        Me.pbTMDB.Name = "pbTMDB"
        Me.pbTMDB.Size = New System.Drawing.Size(145, 23)
        Me.pbTMDB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pbTMDB.TabIndex = 8
        Me.pbTMDB.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Ember_Media_Manager.My.Resources.Resources.Logo
        Me.PictureBox1.Location = New System.Drawing.Point(25, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(75, 78)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 9
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(224, 173)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(78, 38)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox2.TabIndex = 15
        Me.PictureBox2.TabStop = False
        '
        'dlgAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(452, 341)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.lblCopyright)
        Me.Controls.Add(Me.lblVersion)
        Me.Controls.Add(Me.lblProductName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgAbout"
        Me.Padding = New System.Windows.Forms.Padding(9)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About UMM"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.pbFFMPEG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbMI, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbIMDB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbIMPA, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTMDB, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblProductName As System.Windows.Forms.Label
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents pbIMDB As System.Windows.Forms.PictureBox
    Friend WithEvents pbIMPA As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents pbTMDB As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents pbFFMPEG As System.Windows.Forms.PictureBox
    Friend WithEvents pbMI As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox

End Class
