﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dlgExportMovies
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgExportMovies))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel
        Me.Save_Button = New System.Windows.Forms.Button
        Me.Close_Button = New System.Windows.Forms.Button
        Me.pnlBottomMain = New System.Windows.Forms.Panel
        Me.pnlSearch = New System.Windows.Forms.Panel
        Me.Reset_Button = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Search_Button = New System.Windows.Forms.Button
        Me.lblIn = New System.Windows.Forms.Label
        Me.cbSearch = New System.Windows.Forms.ComboBox
        Me.txtSearch = New System.Windows.Forms.TextBox
        Me.pnlCancel = New System.Windows.Forms.Panel
        Me.pbCompile = New System.Windows.Forms.ProgressBar
        Me.lblCompiling = New System.Windows.Forms.Label
        Me.lblFile = New System.Windows.Forms.Label
        Me.lblCanceling = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.pnlBG = New System.Windows.Forms.Panel
        Me.wbMovieList = New System.Windows.Forms.WebBrowser
        Me.TableLayoutPanel1.SuspendLayout()
        Me.pnlBottomMain.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        Me.pnlCancel.SuspendLayout()
        Me.pnlBG.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Save_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Close_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(705, 8)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Save_Button
        '
        Me.Save_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Save_Button.Enabled = False
        Me.Save_Button.Location = New System.Drawing.Point(3, 3)
        Me.Save_Button.Name = "Save_Button"
        Me.Save_Button.Size = New System.Drawing.Size(67, 23)
        Me.Save_Button.TabIndex = 0
        Me.Save_Button.Text = "Save"
        '
        'Close_Button
        '
        Me.Close_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Close_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close_Button.Location = New System.Drawing.Point(76, 3)
        Me.Close_Button.Name = "Close_Button"
        Me.Close_Button.Size = New System.Drawing.Size(67, 23)
        Me.Close_Button.TabIndex = 1
        Me.Close_Button.Text = "Close"
        '
        'pnlBottomMain
        '
        Me.pnlBottomMain.Controls.Add(Me.pnlSearch)
        Me.pnlBottomMain.Controls.Add(Me.TableLayoutPanel1)
        Me.pnlBottomMain.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottomMain.Location = New System.Drawing.Point(0, 495)
        Me.pnlBottomMain.Name = "pnlBottomMain"
        Me.pnlBottomMain.Size = New System.Drawing.Size(854, 50)
        Me.pnlBottomMain.TabIndex = 5
        '
        'pnlSearch
        '
        Me.pnlSearch.Controls.Add(Me.Reset_Button)
        Me.pnlSearch.Controls.Add(Me.Label1)
        Me.pnlSearch.Controls.Add(Me.Search_Button)
        Me.pnlSearch.Controls.Add(Me.lblIn)
        Me.pnlSearch.Controls.Add(Me.cbSearch)
        Me.pnlSearch.Controls.Add(Me.txtSearch)
        Me.pnlSearch.Enabled = False
        Me.pnlSearch.Location = New System.Drawing.Point(230, 9)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(473, 28)
        Me.pnlSearch.TabIndex = 6
        '
        'Reset_Button
        '
        Me.Reset_Button.Enabled = False
        Me.Reset_Button.Location = New System.Drawing.Point(402, 2)
        Me.Reset_Button.Name = "Reset_Button"
        Me.Reset_Button.Size = New System.Drawing.Size(67, 23)
        Me.Reset_Button.TabIndex = 6
        Me.Reset_Button.Text = "Reset"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(9, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Filter"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Search_Button
        '
        Me.Search_Button.Enabled = False
        Me.Search_Button.Location = New System.Drawing.Point(327, 2)
        Me.Search_Button.Name = "Search_Button"
        Me.Search_Button.Size = New System.Drawing.Size(67, 23)
        Me.Search_Button.TabIndex = 5
        Me.Search_Button.Text = "Apply"
        '
        'lblIn
        '
        Me.lblIn.Location = New System.Drawing.Point(203, 7)
        Me.lblIn.Name = "lblIn"
        Me.lblIn.Size = New System.Drawing.Size(34, 13)
        Me.lblIn.TabIndex = 3
        Me.lblIn.Text = "in"
        Me.lblIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cbSearch
        '
        Me.cbSearch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbSearch.FormattingEnabled = True
        Me.cbSearch.Items.AddRange(New Object() {"Title", "Year", "Video Flag", "Audio Flag"})
        Me.cbSearch.Location = New System.Drawing.Point(241, 3)
        Me.cbSearch.Name = "cbSearch"
        Me.cbSearch.Size = New System.Drawing.Size(83, 21)
        Me.cbSearch.TabIndex = 4
        '
        'txtSearch
        '
        Me.txtSearch.Location = New System.Drawing.Point(93, 4)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(104, 20)
        Me.txtSearch.TabIndex = 1
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
        Me.pnlCancel.Location = New System.Drawing.Point(242, 12)
        Me.pnlCancel.Name = "pnlCancel"
        Me.pnlCancel.Size = New System.Drawing.Size(403, 76)
        Me.pnlCancel.TabIndex = 9
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
        'pnlBG
        '
        Me.pnlBG.AutoScroll = True
        Me.pnlBG.Controls.Add(Me.pnlCancel)
        Me.pnlBG.Controls.Add(Me.wbMovieList)
        Me.pnlBG.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBG.Location = New System.Drawing.Point(0, 0)
        Me.pnlBG.Name = "pnlBG"
        Me.pnlBG.Size = New System.Drawing.Size(854, 495)
        Me.pnlBG.TabIndex = 4
        '
        'wbMovieList
        '
        Me.wbMovieList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbMovieList.Location = New System.Drawing.Point(0, 0)
        Me.wbMovieList.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbMovieList.Name = "wbMovieList"
        Me.wbMovieList.Size = New System.Drawing.Size(854, 495)
        Me.wbMovieList.TabIndex = 0
        Me.wbMovieList.Visible = False
        '
        'dlgExportMovies
        '
        Me.AcceptButton = Me.Save_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.CancelButton = Me.Close_Button
        Me.ClientSize = New System.Drawing.Size(854, 545)
        Me.Controls.Add(Me.pnlBG)
        Me.Controls.Add(Me.pnlBottomMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "dlgExportMovies"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Export Movies"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.pnlBottomMain.ResumeLayout(False)
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        Me.pnlCancel.ResumeLayout(False)
        Me.pnlBG.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Save_Button As System.Windows.Forms.Button
    Friend WithEvents Close_Button As System.Windows.Forms.Button
    Friend WithEvents pnlBottomMain As System.Windows.Forms.Panel
    Friend WithEvents pnlCancel As System.Windows.Forms.Panel
    Friend WithEvents lblCanceling As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents pnlBG As System.Windows.Forms.Panel
    Friend WithEvents wbMovieList As System.Windows.Forms.WebBrowser
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents Search_Button As System.Windows.Forms.Button
    Friend WithEvents cbSearch As System.Windows.Forms.ComboBox
    Friend WithEvents lblIn As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents pnlSearch As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Reset_Button As System.Windows.Forms.Button
    Friend WithEvents lblCompiling As System.Windows.Forms.Label
    Friend WithEvents pbCompile As System.Windows.Forms.ProgressBar

End Class
