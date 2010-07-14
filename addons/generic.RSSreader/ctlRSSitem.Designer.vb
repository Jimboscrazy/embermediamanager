<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlRSSitem
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ctlRSSitem))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblDesc = New System.Windows.Forms.Label
        Me.ilImages = New System.Windows.Forms.ImageList(Me.components)
        Me.btnExpand = New System.Windows.Forms.Button
        Me.lblChannel = New System.Windows.Forms.Label
        Me.lblURL = New System.Windows.Forms.LinkLabel
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblMovie = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblSearchType = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Title"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 14)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "URL"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 14)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Description"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(84, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(338, 14)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Text = "Title"
        '
        'lblDesc
        '
        Me.lblDesc.BackColor = System.Drawing.Color.Transparent
        Me.lblDesc.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDesc.Location = New System.Drawing.Point(84, 30)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(338, 28)
        Me.lblDesc.TabIndex = 5
        Me.lblDesc.Text = "Description"
        '
        'ilImages
        '
        Me.ilImages.ImageStream = CType(resources.GetObject("ilImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ilImages.TransparentColor = System.Drawing.Color.Transparent
        Me.ilImages.Images.SetKeyName(0, "green_arrow_down.png")
        Me.ilImages.Images.SetKeyName(1, "green_arrow_up.png")
        '
        'btnExpand
        '
        Me.btnExpand.BackColor = System.Drawing.Color.Transparent
        Me.btnExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.btnExpand.FlatAppearance.BorderColor = System.Drawing.Color.Silver
        Me.btnExpand.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver
        Me.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnExpand.ImageKey = "green_arrow_down.png"
        Me.btnExpand.ImageList = Me.ilImages
        Me.btnExpand.Location = New System.Drawing.Point(583, 0)
        Me.btnExpand.Name = "btnExpand"
        Me.btnExpand.Size = New System.Drawing.Size(16, 16)
        Me.btnExpand.TabIndex = 6
        Me.btnExpand.UseVisualStyleBackColor = False
        '
        'lblChannel
        '
        Me.lblChannel.BackColor = System.Drawing.Color.Transparent
        Me.lblChannel.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChannel.Location = New System.Drawing.Point(428, 0)
        Me.lblChannel.Name = "lblChannel"
        Me.lblChannel.Size = New System.Drawing.Size(149, 14)
        Me.lblChannel.TabIndex = 7
        Me.lblChannel.Text = "Channel"
        Me.lblChannel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblURL
        '
        Me.lblURL.BackColor = System.Drawing.Color.Transparent
        Me.lblURL.Location = New System.Drawing.Point(84, 15)
        Me.lblURL.Name = "lblURL"
        Me.lblURL.Size = New System.Drawing.Size(338, 14)
        Me.lblURL.TabIndex = 8
        Me.lblURL.TabStop = True
        Me.lblURL.Text = "LinkLabel1"
        '
        'lblStatus
        '
        Me.lblStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(428, 43)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(149, 14)
        Me.lblStatus.TabIndex = 9
        Me.lblStatus.Text = "Not found in Database"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMovie
        '
        Me.lblMovie.BackColor = System.Drawing.Color.Transparent
        Me.lblMovie.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMovie.Location = New System.Drawing.Point(84, 61)
        Me.lblMovie.Name = "lblMovie"
        Me.lblMovie.Size = New System.Drawing.Size(291, 14)
        Me.lblMovie.TabIndex = 11
        Me.lblMovie.Text = "Title"
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(3, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(75, 14)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Movie Title"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblYear
        '
        Me.lblYear.BackColor = System.Drawing.Color.Transparent
        Me.lblYear.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYear.Location = New System.Drawing.Point(445, 61)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(38, 14)
        Me.lblYear.TabIndex = 13
        Me.lblYear.Text = "Year"
        Me.lblYear.Visible = False
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(387, 61)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 14)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Year"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.Label6.Visible = False
        '
        'lblSearchType
        '
        Me.lblSearchType.BackColor = System.Drawing.Color.Transparent
        Me.lblSearchType.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchType.ForeColor = System.Drawing.Color.DarkRed
        Me.lblSearchType.Location = New System.Drawing.Point(486, 61)
        Me.lblSearchType.Name = "lblSearchType"
        Me.lblSearchType.Size = New System.Drawing.Size(101, 14)
        Me.lblSearchType.TabIndex = 14
        Me.lblSearchType.Text = "(Guessing)"
        Me.lblSearchType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ctlRSSitem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.lblSearchType)
        Me.Controls.Add(Me.lblYear)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblMovie)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblURL)
        Me.Controls.Add(Me.lblChannel)
        Me.Controls.Add(Me.btnExpand)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "ctlRSSitem"
        Me.Size = New System.Drawing.Size(602, 120)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents ilImages As System.Windows.Forms.ImageList
    Friend WithEvents btnExpand As System.Windows.Forms.Button
    Friend WithEvents lblChannel As System.Windows.Forms.Label
    Friend WithEvents lblURL As System.Windows.Forms.LinkLabel
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblMovie As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblSearchType As System.Windows.Forms.Label

End Class
