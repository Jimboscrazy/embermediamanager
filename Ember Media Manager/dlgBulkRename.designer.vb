<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgBulkRenamer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgBulkRenamer))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.pnlCancel = New System.Windows.Forms.Panel
        Me.pbCompile = New System.Windows.Forms.ProgressBar
        Me.lblCompiling = New System.Windows.Forms.Label
        Me.lblFile = New System.Windows.Forms.Label
        Me.lblCanceling = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.Rename_Button = New System.Windows.Forms.Button
        Me.tmrSimul = New System.Windows.Forms.Timer(Me.components)
        Me.dgvMoviesList = New System.Windows.Forms.DataGridView
        Me.lblFolderPattern = New System.Windows.Forms.Label
        Me.lblFilePattern = New System.Windows.Forms.Label
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.txtFolder = New System.Windows.Forms.TextBox
        Me.lblLabel = New System.Windows.Forms.Label
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCancel.SuspendLayout()
        CType(Me.dgvMoviesList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.OK_Button.Location = New System.Drawing.Point(898, 442)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(80, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Close"
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.SteelBlue
        Me.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.Label4)
        Me.pnlTop.Controls.Add(Me.PictureBox1)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(992, 64)
        Me.pnlTop.TabIndex = 58
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(64, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Rename Movies and files"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(61, 3)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(196, 29)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Movie Renamer"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 7)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'pnlCancel
        '
        Me.pnlCancel.BackColor = System.Drawing.Color.LightGray
        Me.pnlCancel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCancel.Controls.Add(Me.pbCompile)
        Me.pnlCancel.Controls.Add(Me.lblCompiling)
        Me.pnlCancel.Controls.Add(Me.lblFile)
        Me.pnlCancel.Controls.Add(Me.lblCanceling)
        Me.pnlCancel.Controls.Add(Me.btnCancel)
        Me.pnlCancel.Location = New System.Drawing.Point(259, 70)
        Me.pnlCancel.Name = "pnlCancel"
        Me.pnlCancel.Size = New System.Drawing.Size(403, 76)
        Me.pnlCancel.TabIndex = 68
        Me.pnlCancel.Visible = False
        '
        'pbCompile
        '
        Me.pbCompile.Location = New System.Drawing.Point(8, 36)
        Me.pbCompile.Name = "pbCompile"
        Me.pbCompile.Size = New System.Drawing.Size(388, 18)
        Me.pbCompile.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.pbCompile.TabIndex = 5
        '
        'lblCompiling
        '
        Me.lblCompiling.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCompiling.Location = New System.Drawing.Point(3, 11)
        Me.lblCompiling.Name = "lblCompiling"
        Me.lblCompiling.Size = New System.Drawing.Size(186, 20)
        Me.lblCompiling.TabIndex = 4
        Me.lblCompiling.Text = "Compiling Movie List..."
        Me.lblCompiling.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblCompiling.Visible = False
        '
        'lblFile
        '
        Me.lblFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFile.Location = New System.Drawing.Point(3, 57)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(395, 13)
        Me.lblFile.TabIndex = 3
        Me.lblFile.Text = "File ..."
        '
        'lblCanceling
        '
        Me.lblCanceling.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCanceling.Location = New System.Drawing.Point(110, 12)
        Me.lblCanceling.Name = "lblCanceling"
        Me.lblCanceling.Size = New System.Drawing.Size(186, 20)
        Me.lblCanceling.TabIndex = 1
        Me.lblCanceling.Text = "Canceling Compilation..."
        Me.lblCanceling.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblCanceling.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Image = CType(resources.GetObject("btnCancel.Image"), System.Drawing.Image)
        Me.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCancel.Location = New System.Drawing.Point(298, 3)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 30)
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Rename_Button
        '
        Me.Rename_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Rename_Button.Enabled = False
        Me.Rename_Button.Location = New System.Drawing.Point(812, 442)
        Me.Rename_Button.Name = "Rename_Button"
        Me.Rename_Button.Size = New System.Drawing.Size(80, 23)
        Me.Rename_Button.TabIndex = 67
        Me.Rename_Button.Text = "Rename"
        '
        'tmrSimul
        '
        Me.tmrSimul.Interval = 250
        '
        'dgvMoviesList
        '
        Me.dgvMoviesList.AllowUserToAddRows = False
        Me.dgvMoviesList.AllowUserToDeleteRows = False
        Me.dgvMoviesList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMoviesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMoviesList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvMoviesList.Location = New System.Drawing.Point(12, 76)
        Me.dgvMoviesList.Name = "dgvMoviesList"
        Me.dgvMoviesList.RowHeadersVisible = False
        Me.dgvMoviesList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMoviesList.Size = New System.Drawing.Size(966, 316)
        Me.dgvMoviesList.TabIndex = 68
        '
        'lblFolderPattern
        '
        Me.lblFolderPattern.AutoSize = True
        Me.lblFolderPattern.Location = New System.Drawing.Point(12, 398)
        Me.lblFolderPattern.Name = "lblFolderPattern"
        Me.lblFolderPattern.Size = New System.Drawing.Size(73, 13)
        Me.lblFolderPattern.TabIndex = 70
        Me.lblFolderPattern.Text = "Folder Pattern"
        '
        'lblFilePattern
        '
        Me.lblFilePattern.AutoSize = True
        Me.lblFilePattern.Location = New System.Drawing.Point(318, 398)
        Me.lblFilePattern.Name = "lblFilePattern"
        Me.lblFilePattern.Size = New System.Drawing.Size(60, 13)
        Me.lblFilePattern.TabIndex = 71
        Me.lblFilePattern.Text = "File Pattern"
        '
        'txtFile
        '
        Me.txtFile.Location = New System.Drawing.Point(381, 395)
        Me.txtFile.Name = "txtFile"
        Me.txtFile.Size = New System.Drawing.Size(224, 20)
        Me.txtFile.TabIndex = 72
        Me.txtFile.Text = "$T"
        '
        'txtFolder
        '
        Me.txtFolder.Location = New System.Drawing.Point(91, 395)
        Me.txtFolder.Name = "txtFolder"
        Me.txtFolder.Size = New System.Drawing.Size(200, 20)
        Me.txtFolder.TabIndex = 73
        Me.txtFolder.Text = "$T ($Y)"
        '
        'lblLabel
        '
        Me.lblLabel.AutoSize = True
        Me.lblLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblLabel.Location = New System.Drawing.Point(15, 421)
        Me.lblLabel.Name = "lblLabel"
        Me.lblLabel.Size = New System.Drawing.Size(35, 15)
        Me.lblLabel.TabIndex = 74
        Me.lblLabel.Tag = ""
        Me.lblLabel.Text = "Label"
        Me.lblLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dlgBulkRename
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(992, 477)
        Me.Controls.Add(Me.lblLabel)
        Me.Controls.Add(Me.txtFolder)
        Me.Controls.Add(Me.txtFile)
        Me.Controls.Add(Me.lblFilePattern)
        Me.Controls.Add(Me.lblFolderPattern)
        Me.Controls.Add(Me.pnlCancel)
        Me.Controls.Add(Me.dgvMoviesList)
        Me.Controls.Add(Me.Rename_Button)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.OK_Button)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgBulkRename"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Movie Renamer"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCancel.ResumeLayout(False)
        CType(Me.dgvMoviesList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Rename_Button As System.Windows.Forms.Button
    Friend WithEvents tmrSimul As System.Windows.Forms.Timer
    Friend WithEvents pnlCancel As System.Windows.Forms.Panel
    Friend WithEvents pbCompile As System.Windows.Forms.ProgressBar
    Friend WithEvents lblCompiling As System.Windows.Forms.Label
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents lblCanceling As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents dgvMoviesList As System.Windows.Forms.DataGridView
    Friend WithEvents lblFolderPattern As System.Windows.Forms.Label
    Friend WithEvents lblFilePattern As System.Windows.Forms.Label
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents txtFolder As System.Windows.Forms.TextBox
    Friend WithEvents lblLabel As System.Windows.Forms.Label

End Class
