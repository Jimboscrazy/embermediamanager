<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgAddEditActor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgAddEditActor))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.txtName = New System.Windows.Forms.TextBox
        Me.txtRole = New System.Windows.Forms.TextBox
        Me.txtThumb = New System.Windows.Forms.TextBox
        Me.lblName = New System.Windows.Forms.Label
        Me.lblRole = New System.Windows.Forms.Label
        Me.lblThumb = New System.Windows.Forms.Label
        Me.pbActLoad = New System.Windows.Forms.PictureBox
        Me.pbActors = New System.Windows.Forms.PictureBox
        Me.btnVerify = New System.Windows.Forms.Button
        Me.bwDownloadPic = New System.ComponentModel.BackgroundWorker
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.pbActLoad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbActors, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(220, 173)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 4
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 5
        Me.Cancel_Button.Text = "Cancel"
        '
        'txtName
        '
        Me.txtName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.txtName.Location = New System.Drawing.Point(12, 24)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(174, 22)
        Me.txtName.TabIndex = 0
        '
        'txtRole
        '
        Me.txtRole.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.txtRole.Location = New System.Drawing.Point(192, 24)
        Me.txtRole.Name = "txtRole"
        Me.txtRole.Size = New System.Drawing.Size(174, 22)
        Me.txtRole.TabIndex = 1
        '
        'txtThumb
        '
        Me.txtThumb.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.txtThumb.Location = New System.Drawing.Point(12, 67)
        Me.txtThumb.Name = "txtThumb"
        Me.txtThumb.Size = New System.Drawing.Size(354, 22)
        Me.txtThumb.TabIndex = 2
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblName.Location = New System.Drawing.Point(10, 9)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(72, 13)
        Me.lblName.TabIndex = 4
        Me.lblName.Text = "Actor Name:"
        '
        'lblRole
        '
        Me.lblRole.AutoSize = True
        Me.lblRole.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblRole.Location = New System.Drawing.Point(192, 10)
        Me.lblRole.Name = "lblRole"
        Me.lblRole.Size = New System.Drawing.Size(64, 13)
        Me.lblRole.TabIndex = 5
        Me.lblRole.Text = "Actor Role:"
        '
        'lblThumb
        '
        Me.lblThumb.AutoSize = True
        Me.lblThumb.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblThumb.Location = New System.Drawing.Point(10, 52)
        Me.lblThumb.Name = "lblThumb"
        Me.lblThumb.Size = New System.Drawing.Size(110, 13)
        Me.lblThumb.TabIndex = 6
        Me.lblThumb.Text = "Actor Thumb (URL):"
        '
        'pbActLoad
        '
        Me.pbActLoad.Image = CType(resources.GetObject("pbActLoad.Image"), System.Drawing.Image)
        Me.pbActLoad.Location = New System.Drawing.Point(153, 128)
        Me.pbActLoad.Name = "pbActLoad"
        Me.pbActLoad.Size = New System.Drawing.Size(41, 39)
        Me.pbActLoad.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbActLoad.TabIndex = 28
        Me.pbActLoad.TabStop = False
        Me.pbActLoad.Visible = False
        '
        'pbActors
        '
        Me.pbActors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbActors.Location = New System.Drawing.Point(132, 95)
        Me.pbActors.Name = "pbActors"
        Me.pbActors.Size = New System.Drawing.Size(81, 106)
        Me.pbActors.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbActors.TabIndex = 29
        Me.pbActors.TabStop = False
        '
        'btnVerify
        '
        Me.btnVerify.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.btnVerify.Location = New System.Drawing.Point(12, 94)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(114, 23)
        Me.btnVerify.TabIndex = 3
        Me.btnVerify.Text = "Verify Thumb URL"
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'bwDownloadPic
        '
        '
        'dlgAddEditActor
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(378, 206)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnVerify)
        Me.Controls.Add(Me.pbActLoad)
        Me.Controls.Add(Me.pbActors)
        Me.Controls.Add(Me.lblThumb)
        Me.Controls.Add(Me.lblRole)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtThumb)
        Me.Controls.Add(Me.txtRole)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgAddEditActor"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "New Actor"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.pbActLoad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbActors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents txtRole As System.Windows.Forms.TextBox
    Friend WithEvents txtThumb As System.Windows.Forms.TextBox
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblRole As System.Windows.Forms.Label
    Friend WithEvents lblThumb As System.Windows.Forms.Label
    Friend WithEvents pbActLoad As System.Windows.Forms.PictureBox
    Friend WithEvents pbActors As System.Windows.Forms.PictureBox
    Friend WithEvents btnVerify As System.Windows.Forms.Button
    Friend WithEvents bwDownloadPic As System.ComponentModel.BackgroundWorker

End Class
