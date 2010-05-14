<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAVCodecEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAVCodecEditor))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.pnlGenres = New System.Windows.Forms.Panel
        Me.dgvAudio = New System.Windows.Forms.DataGridView
        Me.btnRemoveCom = New System.Windows.Forms.Button
        Me.btnAddCom = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.dgvVideo = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Codec = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewComboBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.pnlGenres.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvVideo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlGenres
        '
        Me.pnlGenres.Controls.Add(Me.Label2)
        Me.pnlGenres.Controls.Add(Me.Label1)
        Me.pnlGenres.Controls.Add(Me.Button2)
        Me.pnlGenres.Controls.Add(Me.Button3)
        Me.pnlGenres.Controls.Add(Me.dgvVideo)
        Me.pnlGenres.Controls.Add(Me.btnRemoveCom)
        Me.pnlGenres.Controls.Add(Me.btnAddCom)
        Me.pnlGenres.Controls.Add(Me.dgvAudio)
        Me.pnlGenres.Location = New System.Drawing.Point(0, 0)
        Me.pnlGenres.Name = "pnlGenres"
        Me.pnlGenres.Size = New System.Drawing.Size(634, 366)
        Me.pnlGenres.TabIndex = 0
        '
        'dgvAudio
        '
        Me.dgvAudio.AllowUserToAddRows = False
        Me.dgvAudio.AllowUserToDeleteRows = False
        Me.dgvAudio.AllowUserToResizeColumns = False
        Me.dgvAudio.AllowUserToResizeRows = False
        Me.dgvAudio.BackgroundColor = System.Drawing.Color.White
        Me.dgvAudio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAudio.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Codec, Me.DataGridViewComboBoxColumn1})
        Me.dgvAudio.Location = New System.Drawing.Point(7, 25)
        Me.dgvAudio.MultiSelect = False
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.RowHeadersVisible = False
        Me.dgvAudio.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvAudio.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvAudio.ShowCellErrors = False
        Me.dgvAudio.ShowCellToolTips = False
        Me.dgvAudio.ShowRowErrors = False
        Me.dgvAudio.Size = New System.Drawing.Size(302, 166)
        Me.dgvAudio.TabIndex = 5
        '
        'btnRemoveCom
        '
        Me.btnRemoveCom.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRemoveCom.Image = CType(resources.GetObject("btnRemoveCom.Image"), System.Drawing.Image)
        Me.btnRemoveCom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRemoveCom.Location = New System.Drawing.Point(220, 197)
        Me.btnRemoveCom.Name = "btnRemoveCom"
        Me.btnRemoveCom.Size = New System.Drawing.Size(87, 23)
        Me.btnRemoveCom.TabIndex = 9
        Me.btnRemoveCom.Text = "Remove"
        Me.btnRemoveCom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnRemoveCom.UseVisualStyleBackColor = True
        '
        'btnAddCom
        '
        Me.btnAddCom.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddCom.Image = CType(resources.GetObject("btnAddCom.Image"), System.Drawing.Image)
        Me.btnAddCom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAddCom.Location = New System.Drawing.Point(127, 197)
        Me.btnAddCom.Name = "btnAddCom"
        Me.btnAddCom.Size = New System.Drawing.Size(87, 23)
        Me.btnAddCom.TabIndex = 11
        Me.btnAddCom.Text = "Add"
        Me.btnAddCom.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAddCom.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(536, 197)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(87, 23)
        Me.Button2.TabIndex = 13
        Me.Button2.Text = "Remove"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(443, 197)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(87, 23)
        Me.Button3.TabIndex = 15
        Me.Button3.Text = "Add"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button3.UseVisualStyleBackColor = True
        '
        'dgvVideo
        '
        Me.dgvVideo.AllowUserToAddRows = False
        Me.dgvVideo.AllowUserToDeleteRows = False
        Me.dgvVideo.AllowUserToResizeColumns = False
        Me.dgvVideo.AllowUserToResizeRows = False
        Me.dgvVideo.BackgroundColor = System.Drawing.Color.White
        Me.dgvVideo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvVideo.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
        Me.dgvVideo.Location = New System.Drawing.Point(322, 25)
        Me.dgvVideo.MultiSelect = False
        Me.dgvVideo.Name = "dgvVideo"
        Me.dgvVideo.RowHeadersVisible = False
        Me.dgvVideo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.dgvVideo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvVideo.ShowCellErrors = False
        Me.dgvVideo.ShowCellToolTips = False
        Me.dgvVideo.ShowRowErrors = False
        Me.dgvVideo.Size = New System.Drawing.Size(302, 166)
        Me.dgvVideo.TabIndex = 12
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Audio Codecs"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(319, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(73, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Video Codecs"
        '
        'Codec
        '
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        Me.Codec.DefaultCellStyle = DataGridViewCellStyle3
        Me.Codec.FillWeight = 130.0!
        Me.Codec.HeaderText = "Mediainfo Codec"
        Me.Codec.Name = "Codec"
        Me.Codec.Width = 130
        '
        'DataGridViewComboBoxColumn1
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewComboBoxColumn1.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridViewComboBoxColumn1.FillWeight = 150.0!
        Me.DataGridViewComboBoxColumn1.HeaderText = "Maped Codec"
        Me.DataGridViewComboBoxColumn1.Name = "DataGridViewComboBoxColumn1"
        Me.DataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewComboBoxColumn1.Width = 150
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridViewTextBoxColumn1.FillWeight = 130.0!
        Me.DataGridViewTextBoxColumn1.HeaderText = "Mediainfo Codec"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Width = 130
        '
        'DataGridViewTextBoxColumn2
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridViewTextBoxColumn2.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridViewTextBoxColumn2.FillWeight = 150.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "Maped Codec"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.DataGridViewTextBoxColumn2.Width = 150
        '
        'frmAVCodecEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 366)
        Me.Controls.Add(Me.pnlGenres)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAVCodecEditor"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmAVCodecEditor"
        Me.pnlGenres.ResumeLayout(False)
        Me.pnlGenres.PerformLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvVideo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlGenres As System.Windows.Forms.Panel
    Friend WithEvents dgvAudio As System.Windows.Forms.DataGridView
    Friend WithEvents btnRemoveCom As System.Windows.Forms.Button
    Friend WithEvents btnAddCom As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents dgvVideo As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Codec As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewComboBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
