<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgWizard
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgWizard))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnNext = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.GroupBox7 = New System.Windows.Forms.GroupBox
        Me.chkMovieNameNFO = New System.Windows.Forms.CheckBox
        Me.chkMovieNFO = New System.Windows.Forms.CheckBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.chkMovieNameDotFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkMovieNameFanartJPG = New System.Windows.Forms.CheckBox
        Me.chkFanartJPG = New System.Windows.Forms.CheckBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.chkFolderJPG = New System.Windows.Forms.CheckBox
        Me.chkPosterJPG = New System.Windows.Forms.CheckBox
        Me.chkPosterTBN = New System.Windows.Forms.CheckBox
        Me.chkMovieNameJPG = New System.Windows.Forms.CheckBox
        Me.chkMovieJPG = New System.Windows.Forms.CheckBox
        Me.chkMovieNameTBN = New System.Windows.Forms.CheckBox
        Me.chkMovieTBN = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.btnMovieRem = New System.Windows.Forms.Button
        Me.btnMovieAddFolder = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lvMovies = New System.Windows.Forms.ListView
        Me.colID = New System.Windows.Forms.ColumnHeader
        Me.colName = New System.Windows.Forms.ColumnHeader
        Me.colPath = New System.Windows.Forms.ColumnHeader
        Me.colRecur = New System.Windows.Forms.ColumnHeader
        Me.colFolder = New System.Windows.Forms.ColumnHeader
        Me.colSingle = New System.Windows.Forms.ColumnHeader
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK_Button.Enabled = False
        Me.OK_Button.Location = New System.Drawing.Point(396, 337)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(469, 337)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'btnBack
        '
        Me.btnBack.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnBack.Enabled = False
        Me.btnBack.Location = New System.Drawing.Point(213, 337)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(67, 23)
        Me.btnBack.TabIndex = 2
        Me.btnBack.Text = "< Back"
        '
        'btnNext
        '
        Me.btnNext.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnNext.Location = New System.Drawing.Point(286, 337)
        Me.btnNext.Name = "btnNext"
        Me.btnNext.Size = New System.Drawing.Size(67, 23)
        Me.btnNext.TabIndex = 3
        Me.btnNext.Text = "Next >"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(164, 7)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(372, 323)
        Me.Panel1.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(18, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(337, 212)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = resources.GetString("Label2.Text")
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(340, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Welcome to Ember Media Manager"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.GroupBox7)
        Me.Panel3.Controls.Add(Me.GroupBox6)
        Me.Panel3.Controls.Add(Me.GroupBox5)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Location = New System.Drawing.Point(164, 7)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(372, 323)
        Me.Panel3.TabIndex = 6
        Me.Panel3.Visible = False
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.chkMovieNameNFO)
        Me.GroupBox7.Controls.Add(Me.chkMovieNFO)
        Me.GroupBox7.Location = New System.Drawing.Point(224, 203)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(122, 61)
        Me.GroupBox7.TabIndex = 68
        Me.GroupBox7.TabStop = False
        Me.GroupBox7.Text = "NFO"
        '
        'chkMovieNameNFO
        '
        Me.chkMovieNameNFO.AutoSize = True
        Me.chkMovieNameNFO.Location = New System.Drawing.Point(6, 41)
        Me.chkMovieNameNFO.Name = "chkMovieNameNFO"
        Me.chkMovieNameNFO.Size = New System.Drawing.Size(84, 17)
        Me.chkMovieNameNFO.TabIndex = 69
        Me.chkMovieNameNFO.Text = "<movie>.nfo"
        Me.chkMovieNameNFO.UseVisualStyleBackColor = True
        '
        'chkMovieNFO
        '
        Me.chkMovieNFO.AutoSize = True
        Me.chkMovieNFO.Location = New System.Drawing.Point(6, 18)
        Me.chkMovieNFO.Name = "chkMovieNFO"
        Me.chkMovieNFO.Size = New System.Drawing.Size(72, 17)
        Me.chkMovieNFO.TabIndex = 68
        Me.chkMovieNFO.Text = "movie.nfo"
        Me.chkMovieNFO.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.chkMovieNameDotFanartJPG)
        Me.GroupBox6.Controls.Add(Me.chkMovieNameFanartJPG)
        Me.GroupBox6.Controls.Add(Me.chkFanartJPG)
        Me.GroupBox6.Location = New System.Drawing.Point(224, 111)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(122, 86)
        Me.GroupBox6.TabIndex = 67
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Fanart"
        '
        'chkMovieNameDotFanartJPG
        '
        Me.chkMovieNameDotFanartJPG.AutoSize = True
        Me.chkMovieNameDotFanartJPG.Location = New System.Drawing.Point(6, 63)
        Me.chkMovieNameDotFanartJPG.Name = "chkMovieNameDotFanartJPG"
        Me.chkMovieNameDotFanartJPG.Size = New System.Drawing.Size(113, 17)
        Me.chkMovieNameDotFanartJPG.TabIndex = 68
        Me.chkMovieNameDotFanartJPG.Text = "<movie>.fanart.jpg"
        Me.chkMovieNameDotFanartJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameFanartJPG
        '
        Me.chkMovieNameFanartJPG.AutoSize = True
        Me.chkMovieNameFanartJPG.Location = New System.Drawing.Point(6, 42)
        Me.chkMovieNameFanartJPG.Name = "chkMovieNameFanartJPG"
        Me.chkMovieNameFanartJPG.Size = New System.Drawing.Size(113, 17)
        Me.chkMovieNameFanartJPG.TabIndex = 67
        Me.chkMovieNameFanartJPG.Text = "<movie>-fanart.jpg"
        Me.chkMovieNameFanartJPG.UseVisualStyleBackColor = True
        '
        'chkFanartJPG
        '
        Me.chkFanartJPG.AutoSize = True
        Me.chkFanartJPG.Location = New System.Drawing.Point(6, 19)
        Me.chkFanartJPG.Name = "chkFanartJPG"
        Me.chkFanartJPG.Size = New System.Drawing.Size(70, 17)
        Me.chkFanartJPG.TabIndex = 66
        Me.chkFanartJPG.Text = "fanart.jpg"
        Me.chkFanartJPG.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.chkFolderJPG)
        Me.GroupBox5.Controls.Add(Me.chkPosterJPG)
        Me.GroupBox5.Controls.Add(Me.chkPosterTBN)
        Me.GroupBox5.Controls.Add(Me.chkMovieNameJPG)
        Me.GroupBox5.Controls.Add(Me.chkMovieJPG)
        Me.GroupBox5.Controls.Add(Me.chkMovieNameTBN)
        Me.GroupBox5.Controls.Add(Me.chkMovieTBN)
        Me.GroupBox5.Location = New System.Drawing.Point(19, 124)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(199, 126)
        Me.GroupBox5.TabIndex = 66
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Posters"
        '
        'chkFolderJPG
        '
        Me.chkFolderJPG.AutoSize = True
        Me.chkFolderJPG.Location = New System.Drawing.Point(10, 97)
        Me.chkFolderJPG.Name = "chkFolderJPG"
        Me.chkFolderJPG.Size = New System.Drawing.Size(69, 17)
        Me.chkFolderJPG.TabIndex = 70
        Me.chkFolderJPG.Text = "folder.jpg"
        Me.chkFolderJPG.UseVisualStyleBackColor = True
        '
        'chkPosterJPG
        '
        Me.chkPosterJPG.AutoSize = True
        Me.chkPosterJPG.Location = New System.Drawing.Point(101, 74)
        Me.chkPosterJPG.Name = "chkPosterJPG"
        Me.chkPosterJPG.Size = New System.Drawing.Size(72, 17)
        Me.chkPosterJPG.TabIndex = 69
        Me.chkPosterJPG.Text = "poster.jpg"
        Me.chkPosterJPG.UseVisualStyleBackColor = True
        '
        'chkPosterTBN
        '
        Me.chkPosterTBN.AutoSize = True
        Me.chkPosterTBN.Location = New System.Drawing.Point(10, 74)
        Me.chkPosterTBN.Name = "chkPosterTBN"
        Me.chkPosterTBN.Size = New System.Drawing.Size(73, 17)
        Me.chkPosterTBN.TabIndex = 68
        Me.chkPosterTBN.Text = "poster.tbn"
        Me.chkPosterTBN.UseVisualStyleBackColor = True
        '
        'chkMovieNameJPG
        '
        Me.chkMovieNameJPG.AutoSize = True
        Me.chkMovieNameJPG.Location = New System.Drawing.Point(101, 51)
        Me.chkMovieNameJPG.Name = "chkMovieNameJPG"
        Me.chkMovieNameJPG.Size = New System.Drawing.Size(83, 17)
        Me.chkMovieNameJPG.TabIndex = 67
        Me.chkMovieNameJPG.Text = "<movie>.jpg"
        Me.chkMovieNameJPG.UseVisualStyleBackColor = True
        '
        'chkMovieJPG
        '
        Me.chkMovieJPG.AutoSize = True
        Me.chkMovieJPG.Location = New System.Drawing.Point(10, 51)
        Me.chkMovieJPG.Name = "chkMovieJPG"
        Me.chkMovieJPG.Size = New System.Drawing.Size(71, 17)
        Me.chkMovieJPG.TabIndex = 66
        Me.chkMovieJPG.Text = "movie.jpg"
        Me.chkMovieJPG.UseVisualStyleBackColor = True
        '
        'chkMovieNameTBN
        '
        Me.chkMovieNameTBN.AutoSize = True
        Me.chkMovieNameTBN.Location = New System.Drawing.Point(101, 28)
        Me.chkMovieNameTBN.Name = "chkMovieNameTBN"
        Me.chkMovieNameTBN.Size = New System.Drawing.Size(84, 17)
        Me.chkMovieNameTBN.TabIndex = 65
        Me.chkMovieNameTBN.Text = "<movie>.tbn"
        Me.chkMovieNameTBN.UseVisualStyleBackColor = True
        '
        'chkMovieTBN
        '
        Me.chkMovieTBN.AutoSize = True
        Me.chkMovieTBN.Location = New System.Drawing.Point(10, 28)
        Me.chkMovieTBN.Name = "chkMovieTBN"
        Me.chkMovieTBN.Size = New System.Drawing.Size(72, 17)
        Me.chkMovieTBN.TabIndex = 64
        Me.chkMovieTBN.Text = "movie.tbn"
        Me.chkMovieTBN.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(45, 277)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(286, 30)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "TIP: Selections containing the text <movie> means that Ember Media Manager will u" & _
            "se the filename of the movie."
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(13, 15)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(341, 93)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = resources.GetString("Label4.Text")
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.White
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Image = Global.Ember_Media_Manager.My.Resources.Resources.Wizard
        Me.PictureBox1.Location = New System.Drawing.Point(3, 7)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(157, 353)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.lvMovies)
        Me.Panel2.Controls.Add(Me.btnMovieRem)
        Me.Panel2.Controls.Add(Me.btnMovieAddFolder)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Location = New System.Drawing.Point(164, 7)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(372, 323)
        Me.Panel2.TabIndex = 5
        Me.Panel2.Visible = False
        '
        'btnMovieRem
        '
        Me.btnMovieRem.Image = CType(resources.GetObject("btnMovieRem.Image"), System.Drawing.Image)
        Me.btnMovieRem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovieRem.Location = New System.Drawing.Point(263, 236)
        Me.btnMovieRem.Name = "btnMovieRem"
        Me.btnMovieRem.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieRem.TabIndex = 56
        Me.btnMovieRem.Text = "Remove"
        Me.btnMovieRem.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovieRem.UseVisualStyleBackColor = True
        '
        'btnMovieAddFolder
        '
        Me.btnMovieAddFolder.Image = CType(resources.GetObject("btnMovieAddFolder.Image"), System.Drawing.Image)
        Me.btnMovieAddFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMovieAddFolder.Location = New System.Drawing.Point(3, 236)
        Me.btnMovieAddFolder.Name = "btnMovieAddFolder"
        Me.btnMovieAddFolder.Size = New System.Drawing.Size(104, 23)
        Me.btnMovieAddFolder.TabIndex = 54
        Me.btnMovieAddFolder.Text = "Add Source"
        Me.btnMovieAddFolder.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMovieAddFolder.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(18, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(332, 93)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = resources.GetString("Label3.Text")
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Location = New System.Drawing.Point(164, 7)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(372, 323)
        Me.Panel4.TabIndex = 7
        Me.Panel4.Visible = False
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(3, 124)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(364, 189)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = resources.GetString("Label8.Text")
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(13, 75)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(337, 46)
        Me.Label6.TabIndex = 1
        Me.Label6.Text = "That wasn't so hard was it?  As mentioned earlier, you can change these or any ot" & _
            "her options in the Settings dialog."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(17, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(315, 48)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "That's it!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ember Media Manager is Ready!"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lvMovies
        '
        Me.lvMovies.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colID, Me.colName, Me.colPath, Me.colRecur, Me.colFolder, Me.colSingle})
        Me.lvMovies.FullRowSelect = True
        Me.lvMovies.HideSelection = False
        Me.lvMovies.Location = New System.Drawing.Point(5, 124)
        Me.lvMovies.Name = "lvMovies"
        Me.lvMovies.Size = New System.Drawing.Size(362, 105)
        Me.lvMovies.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvMovies.TabIndex = 57
        Me.lvMovies.UseCompatibleStateImageBehavior = False
        Me.lvMovies.View = System.Windows.Forms.View.Details
        '
        'colID
        '
        Me.colID.Width = 0
        '
        'colName
        '
        Me.colName.Text = "Name"
        Me.colName.Width = 50
        '
        'colPath
        '
        Me.colPath.Text = "Path"
        Me.colPath.Width = 125
        '
        'colRecur
        '
        Me.colRecur.Text = "Recursive"
        '
        'colFolder
        '
        Me.colFolder.Text = "Use Folder Name"
        '
        'colSingle
        '
        Me.colSingle.Text = "Single Video"
        '
        'dlgWizard
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(540, 368)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnNext)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgWizard"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ember Startup Wizard"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.GroupBox7.ResumeLayout(False)
        Me.GroupBox7.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnNext As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnMovieRem As System.Windows.Forms.Button
    Friend WithEvents btnMovieAddFolder As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox7 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMovieNameNFO As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNFO As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents chkMovieNameDotFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkFanartJPG As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents chkFolderJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkPosterJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkPosterTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieJPG As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieNameTBN As System.Windows.Forms.CheckBox
    Friend WithEvents chkMovieTBN As System.Windows.Forms.CheckBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lvMovies As System.Windows.Forms.ListView
    Friend WithEvents colID As System.Windows.Forms.ColumnHeader
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colPath As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRecur As System.Windows.Forms.ColumnHeader
    Friend WithEvents colFolder As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSingle As System.Windows.Forms.ColumnHeader

End Class
