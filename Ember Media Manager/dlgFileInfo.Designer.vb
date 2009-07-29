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
        Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Video Streams", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Audio Streams", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewGroup3 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("Subtitles Stream", System.Windows.Forms.HorizontalAlignment.Left)
        Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"1", "H264", "Progressive"}, -1)
        Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"1", "AC3", "English"}, -1)
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgFileInfo))
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.lvStreams = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.btnRemoveSet = New System.Windows.Forms.Button
        Me.btnEditSet = New System.Windows.Forms.Button
        Me.btnNewSet = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.cbVideoCodec = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(465, 241)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Close"
        '
        'lvStreams
        '
        Me.lvStreams.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        ListViewGroup1.Header = "Video Streams"
        ListViewGroup1.Name = "VideoStreams"
        ListViewGroup2.Header = "Audio Streams"
        ListViewGroup2.Name = "AudioStreams"
        ListViewGroup3.Header = "Subtitles Stream"
        ListViewGroup3.Name = "SubtitlesStream"
        Me.lvStreams.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2, ListViewGroup3})
        Me.lvStreams.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        ListViewItem1.Group = ListViewGroup1
        ListViewItem2.Group = ListViewGroup2
        Me.lvStreams.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2})
        Me.lvStreams.Location = New System.Drawing.Point(4, 7)
        Me.lvStreams.Name = "lvStreams"
        Me.lvStreams.Size = New System.Drawing.Size(528, 230)
        Me.lvStreams.TabIndex = 1
        Me.lvStreams.UseCompatibleStateImageBehavior = False
        Me.lvStreams.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Width = 31
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Width = 48
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Width = 81
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Width = 96
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Width = 94
        '
        'btnRemoveSet
        '
        Me.btnRemoveSet.Enabled = False
        Me.btnRemoveSet.Image = CType(resources.GetObject("btnRemoveSet.Image"), System.Drawing.Image)
        Me.btnRemoveSet.Location = New System.Drawing.Point(264, 241)
        Me.btnRemoveSet.Name = "btnRemoveSet"
        Me.btnRemoveSet.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveSet.TabIndex = 34
        Me.btnRemoveSet.UseVisualStyleBackColor = True
        '
        'btnEditSet
        '
        Me.btnEditSet.Enabled = False
        Me.btnEditSet.Image = CType(resources.GetObject("btnEditSet.Image"), System.Drawing.Image)
        Me.btnEditSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditSet.Location = New System.Drawing.Point(235, 241)
        Me.btnEditSet.Name = "btnEditSet"
        Me.btnEditSet.Size = New System.Drawing.Size(23, 23)
        Me.btnEditSet.TabIndex = 33
        Me.btnEditSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEditSet.UseVisualStyleBackColor = True
        '
        'btnNewSet
        '
        Me.btnNewSet.Enabled = False
        Me.btnNewSet.Image = CType(resources.GetObject("btnNewSet.Image"), System.Drawing.Image)
        Me.btnNewSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewSet.Location = New System.Drawing.Point(187, 241)
        Me.btnNewSet.Name = "btnNewSet"
        Me.btnNewSet.Size = New System.Drawing.Size(23, 23)
        Me.btnNewSet.TabIndex = 32
        Me.btnNewSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNewSet.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(7, 245)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 15)
        Me.Label4.TabIndex = 36
        Me.Label4.Text = "Stream Type"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cbVideoCodec
        '
        Me.cbVideoCodec.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbVideoCodec.FormattingEnabled = True
        Me.cbVideoCodec.Items.AddRange(New Object() {"Video", "Audio", "Subtitle"})
        Me.cbVideoCodec.Location = New System.Drawing.Point(91, 243)
        Me.cbVideoCodec.Name = "cbVideoCodec"
        Me.cbVideoCodec.Size = New System.Drawing.Size(93, 21)
        Me.cbVideoCodec.TabIndex = 35
        '
        'dlgFileInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(538, 269)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.cbVideoCodec)
        Me.Controls.Add(Me.btnRemoveSet)
        Me.Controls.Add(Me.btnEditSet)
        Me.Controls.Add(Me.btnNewSet)
        Me.Controls.Add(Me.lvStreams)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgFileInfo"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Metadata Editor"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents lvStreams As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRemoveSet As System.Windows.Forms.Button
    Friend WithEvents btnEditSet As System.Windows.Forms.Button
    Friend WithEvents btnNewSet As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cbVideoCodec As System.Windows.Forms.ComboBox

End Class
