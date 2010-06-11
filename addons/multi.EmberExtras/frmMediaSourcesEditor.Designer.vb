<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMediaSources
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMediaSources))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.pnlGenres = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnRemoveSource = New System.Windows.Forms.Button
        Me.btnAddSource = New System.Windows.Forms.Button
        Me.dgvSources = New System.Windows.Forms.DataGridView
        Me.Search = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewComboBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnSetDefaults = New System.Windows.Forms.Button
        Me.pnlGenres.SuspendLayout()
        CType(Me.dgvSources, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlGenres
        '
        Me.pnlGenres.Controls.Add(Me.btnSetDefaults)
        Me.pnlGenres.Controls.Add(Me.Label1)
        Me.pnlGenres.Controls.Add(Me.btnRemoveSource)
        Me.pnlGenres.Controls.Add(Me.btnAddSource)
        Me.pnlGenres.Controls.Add(Me.dgvSources)
        Me.pnlGenres.Location = New System.Drawing.Point(0, 0)
        Me.pnlGenres.Name = "pnlGenres"
        Me.pnlGenres.Size = New System.Drawing.Size(634, 366)
        Me.pnlGenres.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Sources"
        '
        'btnRemoveSource
        '
        Me.btnRemoveSource.Enabled = False
        Me.btnRemoveSource.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveSource.Image = CType(resources.GetObject("btnRemoveSource.Image"), System.Drawing.Image)
        Me.btnRemoveSource.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveSource.Location = New System.Drawing.Point(283, 261)
        Me.btnRemoveSource.Name = "btnRemoveSource"
        Me.btnRemoveSource.Size = New System.Drawing.Size(87, 23)
        Me.btnRemoveSource.TabIndex = 9
        Me.btnRemoveSource.Text = "Remove"
        Me.btnRemoveSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveSource.UseVisualStyleBackColor = True
        '
        'btnAddSource
        '
        Me.btnAddSource.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddSource.Image = CType(resources.GetObject("btnAddSource.Image"), System.Drawing.Image)
        Me.btnAddSource.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddSource.Location = New System.Drawing.Point(190, 261)
        Me.btnAddSource.Name = "btnAddSource"
        Me.btnAddSource.Size = New System.Drawing.Size(87, 23)
        Me.btnAddSource.TabIndex = 11
        Me.btnAddSource.Text = "Add"
        Me.btnAddSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddSource.UseVisualStyleBackColor = True
        '
        'dgvSources
        '
        Me.dgvSources.AllowUserToAddRows = False
        Me.dgvSources.AllowUserToDeleteRows = False
        Me.dgvSources.AllowUserToResizeColumns = False
        Me.dgvSources.AllowUserToResizeRows = False
        Me.dgvSources.BackgroundColor = System.Drawing.Color.White
        Me.dgvSources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSources.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Search, Me.DataGridViewComboBoxColumn1})
        Me.dgvSources.Location = New System.Drawing.Point(7, 25)
        Me.dgvSources.MultiSelect = False
        Me.dgvSources.Name = "dgvSources"
        Me.dgvSources.RowHeadersVisible = False
        Me.dgvSources.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvSources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvSources.ShowCellErrors = False
        Me.dgvSources.ShowCellToolTips = False
        Me.dgvSources.ShowRowErrors = False
        Me.dgvSources.Size = New System.Drawing.Size(362, 230)
        Me.dgvSources.TabIndex = 5
        '
        'Search
        '
        Me.Search.FillWeight = 190.0!
        Me.Search.HeaderText = "Search String"
        Me.Search.Name = "Search"
        Me.Search.Width = 190
        '
        'DataGridViewComboBoxColumn1
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewComboBoxColumn1.DefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewComboBoxColumn1.FillWeight = 150.0!
        Me.DataGridViewComboBoxColumn1.HeaderText = "Source Name"
        Me.DataGridViewComboBoxColumn1.Name = "DataGridViewComboBoxColumn1"
        Me.DataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewComboBoxColumn1.Width = 150
        '
        'btnSetDefaults
        '
        Me.btnSetDefaults.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSetDefaults.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSetDefaults.Location = New System.Drawing.Point(12, 261)
        Me.btnSetDefaults.Name = "btnSetDefaults"
        Me.btnSetDefaults.Size = New System.Drawing.Size(96, 23)
        Me.btnSetDefaults.TabIndex = 17
        Me.btnSetDefaults.Text = "Set Defaults"
        Me.btnSetDefaults.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSetDefaults.UseVisualStyleBackColor = True
        '
        'frmMediaSources
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 366)
        Me.Controls.Add(Me.pnlGenres)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmMediaSources"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmMediaSourcesEditor"
        Me.pnlGenres.ResumeLayout(False)
        Me.pnlGenres.PerformLayout()
        CType(Me.dgvSources, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGenres As System.Windows.Forms.Panel
    Friend WithEvents dgvSources As System.Windows.Forms.DataGridView
    Friend WithEvents btnRemoveSource As System.Windows.Forms.Button
    Friend WithEvents btnAddSource As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Search As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewComboBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnSetDefaults As System.Windows.Forms.Button

End Class
