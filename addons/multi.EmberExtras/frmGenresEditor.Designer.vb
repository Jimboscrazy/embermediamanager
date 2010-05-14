<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGenresEditor
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.pnlGenres = New System.Windows.Forms.Panel
        Me.btnRemoveLang = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.btnChangeImg = New System.Windows.Forms.Button
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.btnAddImg = New System.Windows.Forms.Button
        Me.dgvGenres = New System.Windows.Forms.DataGridView
        Me.searchstring = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnRemoveString = New System.Windows.Forms.Button
        Me.dgvLang = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnAddLang = New System.Windows.Forms.Button
        Me.btnAddString = New System.Windows.Forms.Button
        Me.pnlGenres.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvGenres, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvLang, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlGenres
        '
        Me.pnlGenres.Controls.Add(Me.btnRemoveLang)
        Me.pnlGenres.Controls.Add(Me.GroupBox1)
        Me.pnlGenres.Controls.Add(Me.dgvGenres)
        Me.pnlGenres.Controls.Add(Me.btnRemoveString)
        Me.pnlGenres.Controls.Add(Me.dgvLang)
        Me.pnlGenres.Controls.Add(Me.btnAddLang)
        Me.pnlGenres.Controls.Add(Me.btnAddString)
        Me.pnlGenres.Location = New System.Drawing.Point(0, 0)
        Me.pnlGenres.Name = "pnlGenres"
        Me.pnlGenres.Size = New System.Drawing.Size(627, 367)
        Me.pnlGenres.TabIndex = 0
        '
        'btnRemoveLang
        '
        Me.btnRemoveLang.Enabled = False
        Me.btnRemoveLang.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveLang.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveLang.Location = New System.Drawing.Point(331, 224)
        Me.btnRemoveLang.Name = "btnRemoveLang"
        Me.btnRemoveLang.Size = New System.Drawing.Size(81, 23)
        Me.btnRemoveLang.TabIndex = 35
        Me.btnRemoveLang.Text = "Remove"
        Me.btnRemoveLang.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveLang.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnChangeImg)
        Me.GroupBox1.Controls.Add(Me.PictureBox1)
        Me.GroupBox1.Controls.Add(Me.btnAddImg)
        Me.GroupBox1.Location = New System.Drawing.Point(431, 18)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(180, 195)
        Me.GroupBox1.TabIndex = 30
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Image"
        '
        'btnChangeImg
        '
        Me.btnChangeImg.Enabled = False
        Me.btnChangeImg.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangeImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnChangeImg.Location = New System.Drawing.Point(88, 48)
        Me.btnChangeImg.Name = "btnChangeImg"
        Me.btnChangeImg.Size = New System.Drawing.Size(81, 23)
        Me.btnChangeImg.TabIndex = 12
        Me.btnChangeImg.Text = "Change"
        Me.btnChangeImg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnChangeImg.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox1.Location = New System.Drawing.Point(8, 19)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(68, 120)
        Me.PictureBox1.TabIndex = 6
        Me.PictureBox1.TabStop = False
        '
        'btnAddImg
        '
        Me.btnAddImg.Enabled = False
        Me.btnAddImg.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddImg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddImg.Location = New System.Drawing.Point(88, 19)
        Me.btnAddImg.Name = "btnAddImg"
        Me.btnAddImg.Size = New System.Drawing.Size(81, 23)
        Me.btnAddImg.TabIndex = 13
        Me.btnAddImg.Text = "New"
        Me.btnAddImg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddImg.UseVisualStyleBackColor = True
        '
        'dgvGenres
        '
        Me.dgvGenres.AllowUserToAddRows = False
        Me.dgvGenres.AllowUserToDeleteRows = False
        Me.dgvGenres.AllowUserToResizeColumns = False
        Me.dgvGenres.AllowUserToResizeRows = False
        Me.dgvGenres.BackgroundColor = System.Drawing.Color.White
        Me.dgvGenres.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvGenres.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.searchstring})
        Me.dgvGenres.Location = New System.Drawing.Point(15, 21)
        Me.dgvGenres.MultiSelect = False
        Me.dgvGenres.Name = "dgvGenres"
        Me.dgvGenres.RowHeadersVisible = False
        Me.dgvGenres.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvGenres.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvGenres.ShowCellErrors = False
        Me.dgvGenres.ShowCellToolTips = False
        Me.dgvGenres.ShowRowErrors = False
        Me.dgvGenres.Size = New System.Drawing.Size(202, 192)
        Me.dgvGenres.TabIndex = 31
        '
        'searchstring
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.searchstring.DefaultCellStyle = DataGridViewCellStyle1
        Me.searchstring.FillWeight = 180.0!
        Me.searchstring.HeaderText = "Search String"
        Me.searchstring.Name = "searchstring"
        Me.searchstring.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.searchstring.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.searchstring.Width = 180
        '
        'btnRemoveString
        '
        Me.btnRemoveString.Enabled = False
        Me.btnRemoveString.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveString.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveString.Location = New System.Drawing.Point(121, 224)
        Me.btnRemoveString.Name = "btnRemoveString"
        Me.btnRemoveString.Size = New System.Drawing.Size(87, 23)
        Me.btnRemoveString.TabIndex = 32
        Me.btnRemoveString.Text = "Remove"
        Me.btnRemoveString.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveString.UseVisualStyleBackColor = True
        '
        'dgvLang
        '
        Me.dgvLang.AllowUserToAddRows = False
        Me.dgvLang.AllowUserToDeleteRows = False
        Me.dgvLang.AllowUserToResizeColumns = False
        Me.dgvLang.AllowUserToResizeRows = False
        Me.dgvLang.BackgroundColor = System.Drawing.Color.White
        Me.dgvLang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLang.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.DataGridViewTextBoxColumn1})
        Me.dgvLang.Location = New System.Drawing.Point(245, 21)
        Me.dgvLang.MultiSelect = False
        Me.dgvLang.Name = "dgvLang"
        Me.dgvLang.RowHeadersVisible = False
        Me.dgvLang.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvLang.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvLang.ShowCellErrors = False
        Me.dgvLang.ShowCellToolTips = False
        Me.dgvLang.ShowRowErrors = False
        Me.dgvLang.Size = New System.Drawing.Size(164, 192)
        Me.dgvLang.TabIndex = 34
        '
        'Column1
        '
        Me.Column1.FillWeight = 22.0!
        Me.Column1.HeaderText = ""
        Me.Column1.Name = "Column1"
        Me.Column1.Width = 22
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn1.FillWeight = 120.0!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Language"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn1.Width = 120
        '
        'btnAddLang
        '
        Me.btnAddLang.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddLang.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddLang.Location = New System.Drawing.Point(245, 224)
        Me.btnAddLang.Name = "btnAddLang"
        Me.btnAddLang.Size = New System.Drawing.Size(81, 23)
        Me.btnAddLang.TabIndex = 36
        Me.btnAddLang.Text = "Add"
        Me.btnAddLang.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddLang.UseVisualStyleBackColor = True
        '
        'btnAddString
        '
        Me.btnAddString.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddString.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddString.Location = New System.Drawing.Point(20, 224)
        Me.btnAddString.Name = "btnAddString"
        Me.btnAddString.Size = New System.Drawing.Size(87, 23)
        Me.btnAddString.TabIndex = 33
        Me.btnAddString.Text = "Add"
        Me.btnAddString.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddString.UseVisualStyleBackColor = True
        '
        'frmGenresEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(628, 366)
        Me.Controls.Add(Me.pnlGenres)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGenresEditor"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmGenresEditor"
        Me.pnlGenres.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvGenres, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvLang, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGenres As System.Windows.Forms.Panel
    Friend WithEvents btnRemoveLang As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnChangeImg As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnAddImg As System.Windows.Forms.Button
    Friend WithEvents dgvGenres As System.Windows.Forms.DataGridView
    Friend WithEvents searchstring As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnRemoveString As System.Windows.Forms.Button
    Friend WithEvents dgvLang As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnAddLang As System.Windows.Forms.Button
    Friend WithEvents btnAddString As System.Windows.Forms.Button

End Class
