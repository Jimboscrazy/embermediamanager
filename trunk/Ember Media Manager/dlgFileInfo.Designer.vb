<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgFileInfo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgFileInfo))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.lvVideoStream = New System.Windows.Forms.ListView
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lvAudioStream = New System.Windows.Forms.ListView
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.lvSubsStream = New System.Windows.Forms.ListView
        Me.txtWidth = New System.Windows.Forms.TextBox
        Me.txtHeight = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtDuration = New System.Windows.Forms.TextBox
        Me.cbVideoCodec = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.rbProgressive = New System.Windows.Forms.RadioButton
        Me.rbInterlaced = New System.Windows.Forms.RadioButton
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtARatio = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.cbAudioLanguage = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.cbAudioCodec = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.cbAudioChannels = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.cbSubsLanguage = New System.Windows.Forms.ComboBox
        Me.chbSubsExternal = New System.Windows.Forms.CheckBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.Label11 = New System.Windows.Forms.Label
        Me.cbDefVCodec = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.cbDefACodec = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.btnRemoveSet = New System.Windows.Forms.Button
        Me.btnEditSet = New System.Windows.Forms.Button
        Me.btnNewSet = New System.Windows.Forms.Button
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(516, 442)
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
        'lvVideoStream
        '
        Me.lvVideoStream.Location = New System.Drawing.Point(6, 19)
        Me.lvVideoStream.Name = "lvVideoStream"
        Me.lvVideoStream.Size = New System.Drawing.Size(266, 87)
        Me.lvVideoStream.TabIndex = 1
        Me.lvVideoStream.UseCompatibleStateImageBehavior = False
        Me.lvVideoStream.View = System.Windows.Forms.View.Details
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.txtARatio)
        Me.GroupBox1.Controls.Add(Me.rbInterlaced)
        Me.GroupBox1.Controls.Add(Me.rbProgressive)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.cbVideoCodec)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtDuration)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtHeight)
        Me.GroupBox1.Controls.Add(Me.txtWidth)
        Me.GroupBox1.Controls.Add(Me.lvVideoStream)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 11)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(647, 115)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Video Streams"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.cbAudioChannels)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.cbAudioCodec)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.lvAudioStream)
        Me.GroupBox2.Controls.Add(Me.cbAudioLanguage)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 132)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(647, 115)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Audio Streams"
        '
        'lvAudioStream
        '
        Me.lvAudioStream.Location = New System.Drawing.Point(6, 19)
        Me.lvAudioStream.Name = "lvAudioStream"
        Me.lvAudioStream.Size = New System.Drawing.Size(266, 87)
        Me.lvAudioStream.TabIndex = 1
        Me.lvAudioStream.UseCompatibleStateImageBehavior = False
        Me.lvAudioStream.View = System.Windows.Forms.View.Details
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chbSubsExternal)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Controls.Add(Me.lvSubsStream)
        Me.GroupBox3.Controls.Add(Me.cbSubsLanguage)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 253)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(647, 115)
        Me.GroupBox3.TabIndex = 6
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Subs  Streams"
        '
        'lvSubsStream
        '
        Me.lvSubsStream.Location = New System.Drawing.Point(6, 19)
        Me.lvSubsStream.Name = "lvSubsStream"
        Me.lvSubsStream.Size = New System.Drawing.Size(266, 87)
        Me.lvSubsStream.TabIndex = 1
        Me.lvSubsStream.UseCompatibleStateImageBehavior = False
        Me.lvSubsStream.View = System.Windows.Forms.View.Details
        '
        'txtWidth
        '
        Me.txtWidth.Location = New System.Drawing.Point(362, 24)
        Me.txtWidth.Name = "txtWidth"
        Me.txtWidth.Size = New System.Drawing.Size(48, 20)
        Me.txtWidth.TabIndex = 2
        '
        'txtHeight
        '
        Me.txtHeight.Location = New System.Drawing.Point(475, 24)
        Me.txtHeight.Name = "txtHeight"
        Me.txtHeight.Size = New System.Drawing.Size(48, 20)
        Me.txtHeight.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(306, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 19)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Width"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(416, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Height"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(532, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Duration"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtDuration
        '
        Me.txtDuration.Location = New System.Drawing.Point(591, 24)
        Me.txtDuration.Name = "txtDuration"
        Me.txtDuration.Size = New System.Drawing.Size(48, 20)
        Me.txtDuration.TabIndex = 6
        '
        'cbVideoCodec
        '
        Me.cbVideoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoCodec.FormattingEnabled = True
        Me.cbVideoCodec.Location = New System.Drawing.Point(362, 49)
        Me.cbVideoCodec.Name = "cbVideoCodec"
        Me.cbVideoCodec.Size = New System.Drawing.Size(93, 21)
        Me.cbVideoCodec.TabIndex = 8
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(308, 51)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(51, 15)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "Codec"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'rbProgressive
        '
        Me.rbProgressive.AutoSize = True
        Me.rbProgressive.Location = New System.Drawing.Point(475, 50)
        Me.rbProgressive.Name = "rbProgressive"
        Me.rbProgressive.Size = New System.Drawing.Size(80, 17)
        Me.rbProgressive.TabIndex = 10
        Me.rbProgressive.TabStop = True
        Me.rbProgressive.Text = "Progressive"
        Me.rbProgressive.UseVisualStyleBackColor = True
        '
        'rbInterlaced
        '
        Me.rbInterlaced.AutoSize = True
        Me.rbInterlaced.Location = New System.Drawing.Point(569, 50)
        Me.rbInterlaced.Name = "rbInterlaced"
        Me.rbInterlaced.Size = New System.Drawing.Size(72, 17)
        Me.rbInterlaced.TabIndex = 11
        Me.rbInterlaced.TabStop = True
        Me.rbInterlaced.Text = "Interlaced"
        Me.rbInterlaced.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(278, 75)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(81, 19)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Aspect Ratio"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtARatio
        '
        Me.txtARatio.Location = New System.Drawing.Point(362, 74)
        Me.txtARatio.Name = "txtARatio"
        Me.txtARatio.Size = New System.Drawing.Size(48, 20)
        Me.txtARatio.TabIndex = 12
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(285, 21)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 19)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Language"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbAudioLanguage
        '
        Me.cbAudioLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAudioLanguage.FormattingEnabled = True
        Me.cbAudioLanguage.Location = New System.Drawing.Point(362, 19)
        Me.cbAudioLanguage.Name = "cbAudioLanguage"
        Me.cbAudioLanguage.Size = New System.Drawing.Size(93, 21)
        Me.cbAudioLanguage.TabIndex = 14
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(285, 48)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 19)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Codec"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbAudioCodec
        '
        Me.cbAudioCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAudioCodec.FormattingEnabled = True
        Me.cbAudioCodec.Location = New System.Drawing.Point(362, 46)
        Me.cbAudioCodec.Name = "cbAudioCodec"
        Me.cbAudioCodec.Size = New System.Drawing.Size(93, 21)
        Me.cbAudioCodec.TabIndex = 16
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(288, 48)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(71, 19)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Codec"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(288, 75)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(71, 19)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Channels"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbAudioChannels
        '
        Me.cbAudioChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbAudioChannels.FormattingEnabled = True
        Me.cbAudioChannels.Location = New System.Drawing.Point(362, 73)
        Me.cbAudioChannels.Name = "cbAudioChannels"
        Me.cbAudioChannels.Size = New System.Drawing.Size(93, 21)
        Me.cbAudioChannels.TabIndex = 19
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(288, 28)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(71, 19)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Language"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbSubsLanguage
        '
        Me.cbSubsLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSubsLanguage.FormattingEnabled = True
        Me.cbSubsLanguage.Location = New System.Drawing.Point(362, 28)
        Me.cbSubsLanguage.Name = "cbSubsLanguage"
        Me.cbSubsLanguage.Size = New System.Drawing.Size(93, 21)
        Me.cbSubsLanguage.TabIndex = 21
        '
        'chbSubsExternal
        '
        Me.chbSubsExternal.AutoSize = True
        Me.chbSubsExternal.Location = New System.Drawing.Point(474, 30)
        Me.chbSubsExternal.Name = "chbSubsExternal"
        Me.chbSubsExternal.Size = New System.Drawing.Size(64, 17)
        Me.chbSubsExternal.TabIndex = 23
        Me.chbSubsExternal.Text = "External"
        Me.chbSubsExternal.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnRemoveSet)
        Me.GroupBox4.Controls.Add(Me.btnEditSet)
        Me.GroupBox4.Controls.Add(Me.btnNewSet)
        Me.GroupBox4.Controls.Add(Me.Label13)
        Me.GroupBox4.Controls.Add(Me.ComboBox1)
        Me.GroupBox4.Controls.Add(Me.Label12)
        Me.GroupBox4.Controls.Add(Me.cbDefACodec)
        Me.GroupBox4.Controls.Add(Me.Label11)
        Me.GroupBox4.Controls.Add(Me.ListView1)
        Me.GroupBox4.Controls.Add(Me.cbDefVCodec)
        Me.GroupBox4.Location = New System.Drawing.Point(18, 374)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(367, 94)
        Me.GroupBox4.TabIndex = 7
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Defaults by File Type"
        '
        'ListView1
        '
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.ListView1.Location = New System.Drawing.Point(6, 19)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(101, 70)
        Me.ListView1.TabIndex = 24
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(164, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(78, 19)
        Me.Label11.TabIndex = 15
        Me.Label11.Text = "Video Codec"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbDefVCodec
        '
        Me.cbDefVCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDefVCodec.FormattingEnabled = True
        Me.cbDefVCodec.Location = New System.Drawing.Point(248, 16)
        Me.cbDefVCodec.Name = "cbDefVCodec"
        Me.cbDefVCodec.Size = New System.Drawing.Size(93, 21)
        Me.cbDefVCodec.TabIndex = 14
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(164, 40)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(78, 19)
        Me.Label12.TabIndex = 26
        Me.Label12.Text = "Audio Codec"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbDefACodec
        '
        Me.cbDefACodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbDefACodec.FormattingEnabled = True
        Me.cbDefACodec.Location = New System.Drawing.Point(248, 40)
        Me.cbDefACodec.Name = "cbDefACodec"
        Me.cbDefACodec.Size = New System.Drawing.Size(93, 21)
        Me.cbDefACodec.TabIndex = 25
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(148, 65)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(94, 19)
        Me.Label13.TabIndex = 28
        Me.Label13.Text = "Audio Channels"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(248, 65)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(93, 21)
        Me.ComboBox1.TabIndex = 27
        '
        'btnRemoveSet
        '
        Me.btnRemoveSet.Enabled = False
        Me.btnRemoveSet.Image = CType(resources.GetObject("btnRemoveSet.Image"), System.Drawing.Image)
        Me.btnRemoveSet.Location = New System.Drawing.Point(111, 66)
        Me.btnRemoveSet.Name = "btnRemoveSet"
        Me.btnRemoveSet.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveSet.TabIndex = 31
        Me.btnRemoveSet.UseVisualStyleBackColor = True
        '
        'btnEditSet
        '
        Me.btnEditSet.Enabled = False
        Me.btnEditSet.Image = CType(resources.GetObject("btnEditSet.Image"), System.Drawing.Image)
        Me.btnEditSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditSet.Location = New System.Drawing.Point(111, 43)
        Me.btnEditSet.Name = "btnEditSet"
        Me.btnEditSet.Size = New System.Drawing.Size(23, 23)
        Me.btnEditSet.TabIndex = 30
        Me.btnEditSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEditSet.UseVisualStyleBackColor = True
        '
        'btnNewSet
        '
        Me.btnNewSet.Enabled = False
        Me.btnNewSet.Image = CType(resources.GetObject("btnNewSet.Image"), System.Drawing.Image)
        Me.btnNewSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewSet.Location = New System.Drawing.Point(111, 19)
        Me.btnNewSet.Name = "btnNewSet"
        Me.btnNewSet.Size = New System.Drawing.Size(23, 23)
        Me.btnNewSet.TabIndex = 29
        Me.btnNewSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNewSet.UseVisualStyleBackColor = True
        '
        'dlgFileInfo
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(674, 477)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgFileInfo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "File Info"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lvVideoStream As System.Windows.Forms.ListView
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents lvAudioStream As System.Windows.Forms.ListView
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lvSubsStream As System.Windows.Forms.ListView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtHeight As System.Windows.Forms.TextBox
    Friend WithEvents txtWidth As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbVideoCodec As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDuration As System.Windows.Forms.TextBox
    Friend WithEvents rbInterlaced As System.Windows.Forms.RadioButton
    Friend WithEvents rbProgressive As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtARatio As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents cbAudioCodec As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cbAudioLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cbAudioChannels As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents chbSubsExternal As System.Windows.Forms.CheckBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cbSubsLanguage As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents cbDefACodec As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents cbDefVCodec As System.Windows.Forms.ComboBox
    Friend WithEvents btnRemoveSet As System.Windows.Forms.Button
    Friend WithEvents btnEditSet As System.Windows.Forms.Button
    Friend WithEvents btnNewSet As System.Windows.Forms.Button

End Class
