<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRSSReader
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRSSReader))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.pnlList = New System.Windows.Forms.Panel
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.tsbReload = New System.Windows.Forms.ToolStripButton
        Me.tsbPin = New System.Windows.Forms.ToolStripButton
        Me.ilMain = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OK_Button.Location = New System.Drawing.Point(554, 415)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "Close"
        '
        'pnlList
        '
        Me.pnlList.AutoScroll = True
        Me.pnlList.Location = New System.Drawing.Point(1, 35)
        Me.pnlList.Name = "pnlList"
        Me.pnlList.Size = New System.Drawing.Size(620, 375)
        Me.pnlList.TabIndex = 1
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.SteelBlue
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbReload, Me.tsbPin})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(623, 35)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'tsbReload
        '
        Me.tsbReload.AutoSize = False
        Me.tsbReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbReload.Enabled = False
        Me.tsbReload.Image = CType(resources.GetObject("tsbReload.Image"), System.Drawing.Image)
        Me.tsbReload.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.tsbReload.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbReload.Name = "tsbReload"
        Me.tsbReload.Size = New System.Drawing.Size(33, 32)
        Me.tsbReload.Text = "ToolStripButton1"
        Me.tsbReload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        '
        'tsbPin
        '
        Me.tsbPin.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbPin.Checked = True
        Me.tsbPin.CheckOnClick = True
        Me.tsbPin.CheckState = System.Windows.Forms.CheckState.Checked
        Me.tsbPin.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsbPin.Image = CType(resources.GetObject("tsbPin.Image"), System.Drawing.Image)
        Me.tsbPin.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.tsbPin.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPin.Name = "tsbPin"
        Me.tsbPin.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.tsbPin.Size = New System.Drawing.Size(23, 32)
        Me.tsbPin.Text = "o"
        Me.tsbPin.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.tsbPin.TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay
        '
        'ilMain
        '
        Me.ilMain.ImageStream = CType(resources.GetObject("ilMain.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilMain.TransparentColor = System.Drawing.Color.Transparent
        Me.ilMain.Images.SetKeyName(0, "pin.png")
        Me.ilMain.Images.SetKeyName(1, "note.png")
        Me.ilMain.Images.SetKeyName(2, "note_accept.png")
        '
        'frmRSSReader
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.OK_Button
        Me.ClientSize = New System.Drawing.Size(623, 440)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.pnlList)
        Me.Controls.Add(Me.OK_Button)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRSSReader"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RSS Reader"
        Me.TopMost = True
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents pnlList As System.Windows.Forms.Panel
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbReload As System.Windows.Forms.ToolStripButton
    Friend WithEvents ilMain As System.Windows.Forms.ImageList
    Friend WithEvents tsbPin As System.Windows.Forms.ToolStripButton

End Class
