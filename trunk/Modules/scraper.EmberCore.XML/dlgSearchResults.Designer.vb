﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgSearchResults
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgSearchResults))
        Me.OK_Button = New System.Windows.Forms.Button
        Me.Cancel_Button = New System.Windows.Forms.Button
        Me.tvResults = New System.Windows.Forms.TreeView
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.pnlTop = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.lblYearHeader = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.pnlLoading = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.lblPlotHeader = New System.Windows.Forms.Label
        Me.lblDirectorHeader = New System.Windows.Forms.Label
        Me.lblDirector = New System.Windows.Forms.Label
        Me.txtOutline = New System.Windows.Forms.TextBox
        Me.pbPoster = New System.Windows.Forms.PictureBox
        Me.lblGenreHeader = New System.Windows.Forms.Label
        Me.lblGenre = New System.Windows.Forms.Label
        Me.pnlTop.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlLoading.SuspendLayout()
        CType(Me.pbPoster, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'OK_Button
        '
        Me.OK_Button.Enabled = False
        Me.OK_Button.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK_Button.Location = New System.Drawing.Point(497, 400)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 22)
        Me.OK_Button.TabIndex = 4
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel_Button.Location = New System.Drawing.Point(570, 400)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 22)
        Me.Cancel_Button.TabIndex = 5
        Me.Cancel_Button.Text = "Cancel"
        '
        'tvResults
        '
        Me.tvResults.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvResults.HideSelection = False
        Me.tvResults.Location = New System.Drawing.Point(4, 96)
        Me.tvResults.Name = "tvResults"
        Me.tvResults.Size = New System.Drawing.Size(287, 299)
        Me.tvResults.TabIndex = 0
        '
        'lblTitle
        '
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(300, 71)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(337, 19)
        Me.lblTitle.TabIndex = 3
        Me.lblTitle.Text = "Title"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblYear.Location = New System.Drawing.Point(362, 90)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(31, 13)
        Me.lblYear.TabIndex = 6
        Me.lblYear.Text = "0000"
        '
        'pnlTop
        '
        Me.pnlTop.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pnlTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTop.Controls.Add(Me.Label2)
        Me.pnlTop.Controls.Add(Me.Label1)
        Me.pnlTop.Controls.Add(Me.PictureBox1)
        Me.pnlTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTop.Location = New System.Drawing.Point(0, 0)
        Me.pnlTop.Name = "pnlTop"
        Me.pnlTop.Size = New System.Drawing.Size(643, 64)
        Me.pnlTop.TabIndex = 57
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(61, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(276, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "View details of each result to find the proper movie."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(58, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(257, 32)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Movie Search Results"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(7, 8)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'lblYearHeader
        '
        Me.lblYearHeader.AutoSize = True
        Me.lblYearHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblYearHeader.Location = New System.Drawing.Point(300, 90)
        Me.lblYearHeader.Name = "lblYearHeader"
        Me.lblYearHeader.Size = New System.Drawing.Size(33, 13)
        Me.lblYearHeader.TabIndex = 61
        Me.lblYearHeader.Text = "Year:"
        '
        'btnSearch
        '
        Me.btnSearch.Image = CType(resources.GetObject("btnSearch.Image"), System.Drawing.Image)
        Me.btnSearch.Location = New System.Drawing.Point(271, 70)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(23, 23)
        Me.btnSearch.TabIndex = 67
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(4, 71)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(261, 22)
        Me.txtSearch.TabIndex = 66
        '
        'pnlLoading
        '
        Me.pnlLoading.BackColor = System.Drawing.Color.White
        Me.pnlLoading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlLoading.Controls.Add(Me.Label3)
        Me.pnlLoading.Controls.Add(Me.ProgressBar1)
        Me.pnlLoading.Location = New System.Drawing.Point(365, 223)
        Me.pnlLoading.Name = "pnlLoading"
        Me.pnlLoading.Size = New System.Drawing.Size(200, 54)
        Me.pnlLoading.TabIndex = 68
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 5)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(70, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Searching ..."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(3, 32)
        Me.ProgressBar1.MarqueeAnimationSpeed = 25
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(192, 17)
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me.ProgressBar1.TabIndex = 0
        '
        'lblPlotHeader
        '
        Me.lblPlotHeader.AutoSize = True
        Me.lblPlotHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblPlotHeader.Location = New System.Drawing.Point(297, 280)
        Me.lblPlotHeader.Name = "lblPlotHeader"
        Me.lblPlotHeader.Size = New System.Drawing.Size(83, 13)
        Me.lblPlotHeader.TabIndex = 72
        Me.lblPlotHeader.Text = "Plot Summary:"
        Me.lblPlotHeader.Visible = False
        '
        'lblDirectorHeader
        '
        Me.lblDirectorHeader.AutoSize = True
        Me.lblDirectorHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblDirectorHeader.Location = New System.Drawing.Point(300, 110)
        Me.lblDirectorHeader.Name = "lblDirectorHeader"
        Me.lblDirectorHeader.Size = New System.Drawing.Size(51, 13)
        Me.lblDirectorHeader.TabIndex = 71
        Me.lblDirectorHeader.Text = "Director:"
        Me.lblDirectorHeader.Visible = False
        '
        'lblDirector
        '
        Me.lblDirector.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDirector.Location = New System.Drawing.Point(362, 110)
        Me.lblDirector.Name = "lblDirector"
        Me.lblDirector.Size = New System.Drawing.Size(269, 16)
        Me.lblDirector.TabIndex = 70
        Me.lblDirector.Text = "Director"
        Me.lblDirector.Visible = False
        '
        'txtOutline
        '
        Me.txtOutline.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtOutline.Location = New System.Drawing.Point(300, 296)
        Me.txtOutline.Multiline = True
        Me.txtOutline.Name = "txtOutline"
        Me.txtOutline.Size = New System.Drawing.Size(337, 100)
        Me.txtOutline.TabIndex = 69
        Me.txtOutline.TabStop = False
        Me.txtOutline.Visible = False
        '
        'pbPoster
        '
        Me.pbPoster.Location = New System.Drawing.Point(527, 122)
        Me.pbPoster.Name = "pbPoster"
        Me.pbPoster.Size = New System.Drawing.Size(110, 130)
        Me.pbPoster.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbPoster.TabIndex = 73
        Me.pbPoster.TabStop = False
        Me.pbPoster.Visible = False
        '
        'lblGenreHeader
        '
        Me.lblGenreHeader.AutoSize = True
        Me.lblGenreHeader.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(238, Byte))
        Me.lblGenreHeader.Location = New System.Drawing.Point(301, 127)
        Me.lblGenreHeader.Name = "lblGenreHeader"
        Me.lblGenreHeader.Size = New System.Drawing.Size(54, 13)
        Me.lblGenreHeader.TabIndex = 75
        Me.lblGenreHeader.Text = "Genre(s):"
        Me.lblGenreHeader.Visible = False
        '
        'lblGenre
        '
        Me.lblGenre.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGenre.Location = New System.Drawing.Point(363, 127)
        Me.lblGenre.Name = "lblGenre"
        Me.lblGenre.Size = New System.Drawing.Size(142, 52)
        Me.lblGenre.TabIndex = 74
        Me.lblGenre.Text = "Genre"
        Me.lblGenre.Visible = False
        '
        'dlgSearchResults
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(643, 426)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblGenreHeader)
        Me.Controls.Add(Me.lblGenre)
        Me.Controls.Add(Me.pnlLoading)
        Me.Controls.Add(Me.pbPoster)
        Me.Controls.Add(Me.lblPlotHeader)
        Me.Controls.Add(Me.lblDirectorHeader)
        Me.Controls.Add(Me.lblDirector)
        Me.Controls.Add(Me.txtOutline)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.txtSearch)
        Me.Controls.Add(Me.lblYearHeader)
        Me.Controls.Add(Me.pnlTop)
        Me.Controls.Add(Me.lblYear)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.tvResults)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(649, 452)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(649, 452)
        Me.Name = "dlgSearchResults"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Search Results"
        Me.pnlTop.ResumeLayout(False)
        Me.pnlTop.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlLoading.ResumeLayout(False)
        Me.pnlLoading.PerformLayout()
        CType(Me.pbPoster, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents tvResults As System.Windows.Forms.TreeView
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents pnlTop As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents lblYearHeader As System.Windows.Forms.Label
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents pnlLoading As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblPlotHeader As System.Windows.Forms.Label
    Friend WithEvents lblDirectorHeader As System.Windows.Forms.Label
    Friend WithEvents lblDirector As System.Windows.Forms.Label
    Friend WithEvents txtOutline As System.Windows.Forms.TextBox
    Friend WithEvents pbPoster As System.Windows.Forms.PictureBox
    Friend WithEvents lblGenreHeader As System.Windows.Forms.Label
    Friend WithEvents lblGenre As System.Windows.Forms.Label

End Class
