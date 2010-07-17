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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettingsHolder))
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TextBox2 = New System.Windows.Forms.TextBox
        Me.btnRemoveSet = New System.Windows.Forms.Button
        Me.btnEditSet = New System.Windows.Forms.Button
        Me.btnNewSet = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.cbEnabled = New System.Windows.Forms.CheckBox
        Me.pnlSettings = New System.Windows.Forms.Panel
        Me.chkScrapeLink = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbToEnd = New System.Windows.Forms.RadioButton
        Me.rbOnlyTag = New System.Windows.Forms.RadioButton
        Me.ListView2 = New System.Windows.Forms.ListView
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.btnRemoveTag = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnEditTag = New System.Windows.Forms.Button
        Me.btnNewTag = New System.Windows.Forms.Button
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.TextBox4 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextBox5 = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.pnlSettings.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListView1
        '
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader4})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(7, 58)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(603, 133)
        Me.ListView1.TabIndex = 2
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 138
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "URL"
        Me.ColumnHeader2.Width = 441
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(98, 201)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Name"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(264, 201)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "URL"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(149, 198)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(108, 22)
        Me.TextBox1.TabIndex = 9
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(313, 198)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(297, 22)
        Me.TextBox2.TabIndex = 10
        '
        'btnRemoveSet
        '
        Me.btnRemoveSet.Enabled = False
        Me.btnRemoveSet.Image = CType(resources.GetObject("btnRemoveSet.Image"), System.Drawing.Image)
        Me.btnRemoveSet.Location = New System.Drawing.Point(65, 196)
        Me.btnRemoveSet.Name = "btnRemoveSet"
        Me.btnRemoveSet.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveSet.TabIndex = 37
        Me.btnRemoveSet.UseVisualStyleBackColor = True
        '
        'btnEditSet
        '
        Me.btnEditSet.Enabled = False
        Me.btnEditSet.Image = CType(resources.GetObject("btnEditSet.Image"), System.Drawing.Image)
        Me.btnEditSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditSet.Location = New System.Drawing.Point(36, 196)
        Me.btnEditSet.Name = "btnEditSet"
        Me.btnEditSet.Size = New System.Drawing.Size(23, 23)
        Me.btnEditSet.TabIndex = 36
        Me.btnEditSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEditSet.UseVisualStyleBackColor = True
        '
        'btnNewSet
        '
        Me.btnNewSet.Enabled = False
        Me.btnNewSet.Image = CType(resources.GetObject("btnNewSet.Image"), System.Drawing.Image)
        Me.btnNewSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewSet.Location = New System.Drawing.Point(7, 196)
        Me.btnNewSet.Name = "btnNewSet"
        Me.btnNewSet.Size = New System.Drawing.Size(23, 23)
        Me.btnNewSet.TabIndex = 35
        Me.btnNewSet.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNewSet.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.Controls.Add(Me.cbEnabled)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(617, 25)
        Me.Panel1.TabIndex = 82
        '
        'cbEnabled
        '
        Me.cbEnabled.AutoSize = True
        Me.cbEnabled.Location = New System.Drawing.Point(10, 5)
        Me.cbEnabled.Name = "cbEnabled"
        Me.cbEnabled.Size = New System.Drawing.Size(68, 17)
        Me.cbEnabled.TabIndex = 80
        Me.cbEnabled.Text = "Enabled"
        Me.cbEnabled.UseVisualStyleBackColor = True
        '
        'pnlSettings
        '
        Me.pnlSettings.Controls.Add(Me.Label6)
        Me.pnlSettings.Controls.Add(Me.Label5)
        Me.pnlSettings.Controls.Add(Me.TextBox5)
        Me.pnlSettings.Controls.Add(Me.Label2)
        Me.pnlSettings.Controls.Add(Me.TextBox4)
        Me.pnlSettings.Controls.Add(Me.chkScrapeLink)
        Me.pnlSettings.Controls.Add(Me.GroupBox1)
        Me.pnlSettings.Controls.Add(Me.ListView2)
        Me.pnlSettings.Controls.Add(Me.btnRemoveTag)
        Me.pnlSettings.Controls.Add(Me.Label1)
        Me.pnlSettings.Controls.Add(Me.btnEditTag)
        Me.pnlSettings.Controls.Add(Me.btnNewTag)
        Me.pnlSettings.Controls.Add(Me.TextBox3)
        Me.pnlSettings.Controls.Add(Me.Panel1)
        Me.pnlSettings.Controls.Add(Me.ListView1)
        Me.pnlSettings.Controls.Add(Me.btnRemoveSet)
        Me.pnlSettings.Controls.Add(Me.Label3)
        Me.pnlSettings.Controls.Add(Me.btnEditSet)
        Me.pnlSettings.Controls.Add(Me.Label4)
        Me.pnlSettings.Controls.Add(Me.btnNewSet)
        Me.pnlSettings.Controls.Add(Me.TextBox1)
        Me.pnlSettings.Controls.Add(Me.TextBox2)
        Me.pnlSettings.Location = New System.Drawing.Point(3, 12)
        Me.pnlSettings.Name = "pnlSettings"
        Me.pnlSettings.Size = New System.Drawing.Size(617, 401)
        Me.pnlSettings.TabIndex = 83
        '
        'chkScrapeLink
        '
        Me.chkScrapeLink.AutoSize = True
        Me.chkScrapeLink.Location = New System.Drawing.Point(101, 226)
        Me.chkScrapeLink.Name = "chkScrapeLink"
        Me.chkScrapeLink.Size = New System.Drawing.Size(135, 17)
        Me.chkScrapeLink.TabIndex = 92
        Me.chkScrapeLink.Text = "Try to Scrape the Link"
        Me.chkScrapeLink.UseVisualStyleBackColor = True
        Me.chkScrapeLink.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbToEnd)
        Me.GroupBox1.Controls.Add(Me.rbOnlyTag)
        Me.GroupBox1.Location = New System.Drawing.Point(219, 321)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 60)
        Me.GroupBox1.TabIndex = 91
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filter Tag"
        '
        'rbToEnd
        '
        Me.rbToEnd.AutoSize = True
        Me.rbToEnd.Checked = True
        Me.rbToEnd.Location = New System.Drawing.Point(6, 17)
        Me.rbToEnd.Name = "rbToEnd"
        Me.rbToEnd.Size = New System.Drawing.Size(131, 17)
        Me.rbToEnd.TabIndex = 92
        Me.rbToEnd.TabStop = True
        Me.rbToEnd.Text = "Until the end of Title"
        Me.rbToEnd.UseVisualStyleBackColor = True
        '
        'rbOnlyTag
        '
        Me.rbOnlyTag.AutoSize = True
        Me.rbOnlyTag.Location = New System.Drawing.Point(6, 38)
        Me.rbOnlyTag.Name = "rbOnlyTag"
        Me.rbOnlyTag.Size = New System.Drawing.Size(89, 17)
        Me.rbOnlyTag.TabIndex = 91
        Me.rbOnlyTag.Text = "Only the tag"
        Me.rbOnlyTag.UseVisualStyleBackColor = True
        '
        'ListView2
        '
        Me.ListView2.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader3})
        Me.ListView2.FullRowSelect = True
        Me.ListView2.Location = New System.Drawing.Point(7, 261)
        Me.ListView2.Name = "ListView2"
        Me.ListView2.Size = New System.Drawing.Size(166, 133)
        Me.ListView2.TabIndex = 83
        Me.ListView2.UseCompatibleStateImageBehavior = False
        Me.ListView2.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Tag"
        Me.ColumnHeader3.Width = 138
        '
        'btnRemoveTag
        '
        Me.btnRemoveTag.Enabled = False
        Me.btnRemoveTag.Image = CType(resources.GetObject("btnRemoveTag.Image"), System.Drawing.Image)
        Me.btnRemoveTag.Location = New System.Drawing.Point(179, 319)
        Me.btnRemoveTag.Name = "btnRemoveTag"
        Me.btnRemoveTag.Size = New System.Drawing.Size(23, 23)
        Me.btnRemoveTag.TabIndex = 88
        Me.btnRemoveTag.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(222, 262)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 84
        Me.Label1.Text = "Title Filter Tag"
        '
        'btnEditTag
        '
        Me.btnEditTag.Enabled = False
        Me.btnEditTag.Image = CType(resources.GetObject("btnEditTag.Image"), System.Drawing.Image)
        Me.btnEditTag.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnEditTag.Location = New System.Drawing.Point(179, 290)
        Me.btnEditTag.Name = "btnEditTag"
        Me.btnEditTag.Size = New System.Drawing.Size(23, 23)
        Me.btnEditTag.TabIndex = 87
        Me.btnEditTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnEditTag.UseVisualStyleBackColor = True
        '
        'btnNewTag
        '
        Me.btnNewTag.Enabled = False
        Me.btnNewTag.Image = CType(resources.GetObject("btnNewTag.Image"), System.Drawing.Image)
        Me.btnNewTag.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNewTag.Location = New System.Drawing.Point(177, 261)
        Me.btnNewTag.Name = "btnNewTag"
        Me.btnNewTag.Size = New System.Drawing.Size(23, 23)
        Me.btnNewTag.TabIndex = 86
        Me.btnNewTag.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnNewTag.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(225, 278)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(108, 22)
        Me.TextBox3.TabIndex = 85
        '
        'TextBox4
        '
        Me.TextBox4.Location = New System.Drawing.Point(313, 224)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(297, 22)
        Me.TextBox4.TabIndex = 93
        Me.TextBox4.Visible = False
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(225, 227)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 13)
        Me.Label2.TabIndex = 94
        Me.Label2.Text = "Auth String"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.Label2.Visible = False
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Width = 0
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(89, 13)
        Me.Label5.TabIndex = 96
        Me.Label5.Text = "Check Every"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TextBox5
        '
        Me.TextBox5.Location = New System.Drawing.Point(97, 33)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(38, 22)
        Me.TextBox5.TabIndex = 95
        Me.TextBox5.Text = "60"
        Me.TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(138, 36)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(62, 13)
        Me.Label6.TabIndex = 97
        Me.Label6.Text = "seconds"
        '
        'frmSettingsHolder
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
        Me.Name = "frmSettingsHolder"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Settings for RSS Reader"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.pnlSettings.ResumeLayout(False)
        Me.pnlSettings.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents btnRemoveSet As System.Windows.Forms.Button
    Friend WithEvents btnEditSet As System.Windows.Forms.Button
    Friend WithEvents btnNewSet As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents cbEnabled As System.Windows.Forms.CheckBox
    Friend WithEvents pnlSettings As System.Windows.Forms.Panel
    Friend WithEvents ListView2 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnRemoveTag As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnEditTag As System.Windows.Forms.Button
    Friend WithEvents btnNewTag As System.Windows.Forms.Button
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents chkScrapeLink As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbToEnd As System.Windows.Forms.RadioButton
    Friend WithEvents rbOnlyTag As System.Windows.Forms.RadioButton
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox

End Class
