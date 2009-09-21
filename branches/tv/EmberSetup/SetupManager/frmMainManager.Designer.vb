<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainManager
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
        Me.components = New System.ComponentModel.Container
        Me.lstVersions = New System.Windows.Forms.ListView
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.Label3 = New System.Windows.Forms.Label
        Me.cbPlatform = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtEMMVersion = New System.Windows.Forms.TextBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.btnRescan = New System.Windows.Forms.Button
        Me.btnRefresh = New System.Windows.Forms.Button
        Me.lstFiles = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.AllwaysExcludeFileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveExclusionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AllwaysExcludeFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveFolderExclusionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Label8 = New System.Windows.Forms.Label
        Me.gbCommands = New System.Windows.Forms.GroupBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtDescriptions = New System.Windows.Forms.TextBox
        Me.txtCommand = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.cbType = New System.Windows.Forms.ComboBox
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnSave = New System.Windows.Forms.Button
        Me.btnNew = New System.Windows.Forms.Button
        Me.lstCommands = New System.Windows.Forms.ListView
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.pnlWork = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.btnSaveVersion = New System.Windows.Forms.Button
        Me.btnOriginPath = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.gbCommands.SuspendLayout()
        Me.pnlWork.SuspendLayout()
        Me.SuspendLayout()
        '
        'lstVersions
        '
        Me.lstVersions.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader5})
        Me.lstVersions.FullRowSelect = True
        Me.lstVersions.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstVersions.HideSelection = False
        Me.lstVersions.Location = New System.Drawing.Point(1, 23)
        Me.lstVersions.Name = "lstVersions"
        Me.lstVersions.Size = New System.Drawing.Size(121, 225)
        Me.lstVersions.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lstVersions.TabIndex = 0
        Me.lstVersions.UseCompatibleStateImageBehavior = False
        Me.lstVersions.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Width = 97
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(0, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Versions"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Location = New System.Drawing.Point(123, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(861, 497)
        Me.Panel1.TabIndex = 2
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Location = New System.Drawing.Point(3, 0)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(858, 497)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.cbPlatform)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Controls.Add(Me.CheckBox1)
        Me.TabPage1.Controls.Add(Me.btnRescan)
        Me.TabPage1.Controls.Add(Me.btnRefresh)
        Me.TabPage1.Controls.Add(Me.lstFiles)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(850, 471)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Files"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(264, 372)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Platform"
        '
        'cbPlatform
        '
        Me.cbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbPlatform.FormattingEnabled = True
        Me.cbPlatform.Items.AddRange(New Object() {"x86", "x64"})
        Me.cbPlatform.Location = New System.Drawing.Point(314, 369)
        Me.cbPlatform.Name = "cbPlatform"
        Me.cbPlatform.Size = New System.Drawing.Size(84, 21)
        Me.cbPlatform.TabIndex = 10
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtEMMVersion)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.btnSaveVersion)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 391)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(836, 74)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Ember Information"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 22)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "EMM Version"
        '
        'txtEMMVersion
        '
        Me.txtEMMVersion.Location = New System.Drawing.Point(82, 19)
        Me.txtEMMVersion.Name = "txtEMMVersion"
        Me.txtEMMVersion.ReadOnly = True
        Me.txtEMMVersion.Size = New System.Drawing.Size(78, 20)
        Me.txtEMMVersion.TabIndex = 0
        Me.txtEMMVersion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(753, 371)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(91, 17)
        Me.CheckBox1.TabIndex = 7
        Me.CheckBox1.Text = "Show All Files"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'btnRescan
        '
        Me.btnRescan.Location = New System.Drawing.Point(133, 367)
        Me.btnRescan.Name = "btnRescan"
        Me.btnRescan.Size = New System.Drawing.Size(121, 23)
        Me.btnRescan.TabIndex = 6
        Me.btnRescan.Text = "ReScan"
        Me.btnRescan.UseVisualStyleBackColor = True
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(6, 367)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(121, 23)
        Me.btnRefresh.TabIndex = 5
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'lstFiles
        '
        Me.lstFiles.CheckBoxes = True
        Me.lstFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.lstFiles.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lstFiles.FullRowSelect = True
        Me.lstFiles.Location = New System.Drawing.Point(0, 1)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(847, 360)
        Me.lstFiles.TabIndex = 0
        Me.lstFiles.UseCompatibleStateImageBehavior = False
        Me.lstFiles.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Path"
        Me.ColumnHeader1.Width = 208
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "File"
        Me.ColumnHeader2.Width = 319
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Ember Path"
        Me.ColumnHeader3.Width = 220
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Platform"
        Me.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader4.Width = 76
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AllwaysExcludeFileToolStripMenuItem, Me.RemoveExclusionToolStripMenuItem, Me.AllwaysExcludeFolderToolStripMenuItem, Me.RemoveFolderExclusionToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(205, 92)
        '
        'AllwaysExcludeFileToolStripMenuItem
        '
        Me.AllwaysExcludeFileToolStripMenuItem.Name = "AllwaysExcludeFileToolStripMenuItem"
        Me.AllwaysExcludeFileToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.AllwaysExcludeFileToolStripMenuItem.Text = "Allways Exclude File"
        '
        'RemoveExclusionToolStripMenuItem
        '
        Me.RemoveExclusionToolStripMenuItem.Name = "RemoveExclusionToolStripMenuItem"
        Me.RemoveExclusionToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.RemoveExclusionToolStripMenuItem.Text = "Remove File Exclusion"
        '
        'AllwaysExcludeFolderToolStripMenuItem
        '
        Me.AllwaysExcludeFolderToolStripMenuItem.Name = "AllwaysExcludeFolderToolStripMenuItem"
        Me.AllwaysExcludeFolderToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.AllwaysExcludeFolderToolStripMenuItem.Text = "Allways Exclude Folder"
        '
        'RemoveFolderExclusionToolStripMenuItem
        '
        Me.RemoveFolderExclusionToolStripMenuItem.Name = "RemoveFolderExclusionToolStripMenuItem"
        Me.RemoveFolderExclusionToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.RemoveFolderExclusionToolStripMenuItem.Text = "Remove Folder Exclusion"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.gbCommands)
        Me.TabPage2.Controls.Add(Me.btnRemove)
        Me.TabPage2.Controls.Add(Me.btnSave)
        Me.TabPage2.Controls.Add(Me.btnNew)
        Me.TabPage2.Controls.Add(Me.lstCommands)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(850, 471)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Upgrade Commands"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Label8.Location = New System.Drawing.Point(288, 355)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(185, 18)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Commands for Version: "
        '
        'gbCommands
        '
        Me.gbCommands.Controls.Add(Me.Label7)
        Me.gbCommands.Controls.Add(Me.Label6)
        Me.gbCommands.Controls.Add(Me.txtDescriptions)
        Me.gbCommands.Controls.Add(Me.txtCommand)
        Me.gbCommands.Controls.Add(Me.Label5)
        Me.gbCommands.Controls.Add(Me.cbType)
        Me.gbCommands.Location = New System.Drawing.Point(6, 378)
        Me.gbCommands.Name = "gbCommands"
        Me.gbCommands.Size = New System.Drawing.Size(841, 87)
        Me.gbCommands.TabIndex = 5
        Me.gbCommands.TabStop = False
        Me.gbCommands.Text = "Command"
        Me.gbCommands.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(159, 22)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Description"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(0, 53)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Command"
        '
        'txtDescriptions
        '
        Me.txtDescriptions.Location = New System.Drawing.Point(225, 19)
        Me.txtDescriptions.Name = "txtDescriptions"
        Me.txtDescriptions.Size = New System.Drawing.Size(610, 20)
        Me.txtDescriptions.TabIndex = 3
        '
        'txtCommand
        '
        Me.txtCommand.Location = New System.Drawing.Point(60, 50)
        Me.txtCommand.Name = "txtCommand"
        Me.txtCommand.Size = New System.Drawing.Size(775, 20)
        Me.txtCommand.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(23, 22)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(31, 13)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "Type"
        '
        'cbType
        '
        Me.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbType.FormattingEnabled = True
        Me.cbType.Items.AddRange(New Object() {"DB"})
        Me.cbType.Location = New System.Drawing.Point(60, 19)
        Me.cbType.Name = "cbType"
        Me.cbType.Size = New System.Drawing.Size(69, 21)
        Me.cbType.TabIndex = 0
        '
        'btnRemove
        '
        Me.btnRemove.Enabled = False
        Me.btnRemove.Location = New System.Drawing.Point(168, 350)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(75, 23)
        Me.btnRemove.TabIndex = 4
        Me.btnRemove.Text = "Remove"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnSave
        '
        Me.btnSave.Enabled = False
        Me.btnSave.Location = New System.Drawing.Point(87, 350)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 3
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnNew
        '
        Me.btnNew.Enabled = False
        Me.btnNew.Location = New System.Drawing.Point(6, 350)
        Me.btnNew.Name = "btnNew"
        Me.btnNew.Size = New System.Drawing.Size(75, 23)
        Me.btnNew.TabIndex = 2
        Me.btnNew.Text = "New"
        Me.btnNew.UseVisualStyleBackColor = True
        '
        'lstCommands
        '
        Me.lstCommands.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader6, Me.ColumnHeader7})
        Me.lstCommands.ContextMenuStrip = Me.ContextMenuStrip1
        Me.lstCommands.FullRowSelect = True
        Me.lstCommands.Location = New System.Drawing.Point(2, 3)
        Me.lstCommands.Name = "lstCommands"
        Me.lstCommands.Size = New System.Drawing.Size(847, 341)
        Me.lstCommands.TabIndex = 1
        Me.lstCommands.UseCompatibleStateImageBehavior = False
        Me.lstCommands.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Type"
        Me.ColumnHeader6.Width = 63
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Command"
        Me.ColumnHeader7.Width = 761
        '
        'pnlWork
        '
        Me.pnlWork.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlWork.Controls.Add(Me.Label4)
        Me.pnlWork.Location = New System.Drawing.Point(315, 181)
        Me.pnlWork.Name = "pnlWork"
        Me.pnlWork.Size = New System.Drawing.Size(373, 76)
        Me.pnlWork.TabIndex = 12
        Me.pnlWork.Visible = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(132, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(104, 20)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Please Wait"
        '
        'btnSaveVersion
        '
        Me.btnSaveVersion.Enabled = False
        Me.btnSaveVersion.Location = New System.Drawing.Point(182, 17)
        Me.btnSaveVersion.Name = "btnSaveVersion"
        Me.btnSaveVersion.Size = New System.Drawing.Size(121, 23)
        Me.btnSaveVersion.TabIndex = 0
        Me.btnSaveVersion.Text = "Save Version"
        Me.btnSaveVersion.UseVisualStyleBackColor = True
        '
        'btnOriginPath
        '
        Me.btnOriginPath.Location = New System.Drawing.Point(3, 435)
        Me.btnOriginPath.Name = "btnOriginPath"
        Me.btnOriginPath.Size = New System.Drawing.Size(121, 23)
        Me.btnOriginPath.TabIndex = 4
        Me.btnOriginPath.Text = "Origin Path"
        Me.btnOriginPath.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(3, 464)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(121, 23)
        Me.btnClose.TabIndex = 5
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(308, 17)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(121, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Pack Version Files"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'frmMainManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 499)
        Me.Controls.Add(Me.pnlWork)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnOriginPath)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lstVersions)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmMainManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ember Setup Manager"
        Me.Panel1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.gbCommands.ResumeLayout(False)
        Me.gbCommands.PerformLayout()
        Me.pnlWork.ResumeLayout(False)
        Me.pnlWork.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lstVersions As System.Windows.Forms.ListView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents btnSaveVersion As System.Windows.Forms.Button
    Friend WithEvents lstFiles As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnOriginPath As System.Windows.Forms.Button
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnRescan As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cbPlatform As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtEMMVersion As System.Windows.Forms.TextBox
    Friend WithEvents pnlWork As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents AllwaysExcludeFileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveExclusionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllwaysExcludeFolderToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveFolderExclusionToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents lstCommands As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbCommands As System.Windows.Forms.GroupBox
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnSave As System.Windows.Forms.Button
    Friend WithEvents btnNew As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cbType As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDescriptions As System.Windows.Forms.TextBox
    Friend WithEvents txtCommand As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label

End Class
