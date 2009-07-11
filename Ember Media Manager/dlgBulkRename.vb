' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################



Option Explicit On
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class dlgBulkRenamer
    Friend WithEvents bwLoadInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwDoRename As New System.ComponentModel.BackgroundWorker
    Private bindingSource1 As New BindingSource()
    Private isLoaded As Boolean = False
    Private FFRenamer As New FileFolderRenamer
    Private DoneRename As Boolean = False
    Private CancelRename As Boolean = False
    Private run_once As Boolean = True
    Private _columnsize(9) As Integer


    Private Sub Close_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Close_Button.Click
        If DoneRename Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        End If

        Me.Close()
    End Sub


    Public Sub Simulate()
        Try
            With Me.dgvMoviesList
                If Not run_once Then
                    For Each c As DataGridViewColumn In .Columns
                        _columnsize(c.Index) = c.Width
                    Next
                End If
                .DataSource = Nothing
                .Rows.Clear()
                .AutoGenerateColumns = True
                If run_once Then
                    .Tag = False
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                End If
                bindingSource1.DataSource = FFRenamer.GetMovies
                .DataSource = bindingSource1
                .Columns(5).Visible = False
                .Columns(6).Visible = False
                .Columns(7).Visible = False
                .Columns(8).Visible = False
                If run_once Then
                    For Each c As DataGridViewColumn In .Columns
                        c.MinimumWidth = .Width / 5
                    Next
                    .AutoResizeColumns()
                    .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
                    For Each c As DataGridViewColumn In .Columns
                        c.MinimumWidth = 20
                    Next
                    run_once = False
                Else
                    .Tag = True
                    For Each c As DataGridViewColumn In .Columns
                        c.Width = _columnsize(c.Index)
                    Next
                    .Tag = False
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub bwLoadInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadInfo.DoWork
        '//
        ' Thread to load movieinformation (from nfo)
        '\\
        Try
            Dim MovieFile As FileFolderRenamer.FileRename
            Dim _curMovie As New Master.DBMovie
            Dim tVid As New MediaInfo.Video
            Dim tAud As New MediaInfo.Audio
            Dim tRes As String = String.Empty
            ' Load nfo movies using path from DB
            Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                Dim _tmpPath As String = String.Empty
                Dim iProg As Integer = 0
                SQLNewcommand.CommandText = String.Concat("SELECT COUNT(id) AS mcount FROM movies;")
                Using SQLcount As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    Me.bwLoadInfo.ReportProgress(-1, SQLcount("mcount")) ' set maximum
                End Using
                SQLNewcommand.CommandText = String.Concat("SELECT NfoPath ,id FROM movies ORDER BY ListTitle ASC;")
                Using SQLreader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    If SQLreader.HasRows Then
                        While SQLreader.Read()
                            _tmpPath = SQLreader("NfoPath").ToString
                            If Not String.IsNullOrEmpty(_tmpPath) Then
                                MovieFile = New FileFolderRenamer.FileRename
                                MovieFile.ID = SQLreader("id")
                                _curMovie = Master.DB.LoadMovieFromDB(MovieFile.ID)
                                If Not _curMovie.ID = -1 Then
                                    If _curMovie.Movie.Title = String.Empty Then
                                        MovieFile.Title = _curMovie.ListTitle
                                    Else
                                        MovieFile.Title = _curMovie.Movie.Title
                                    End If
                                    MovieFile.Year = _curMovie.Movie.Year
                                    MovieFile.IsLocked = _curMovie.IsLock
                                    MovieFile.BasePath = Path.GetDirectoryName(_curMovie.FaS.Filename)
                                    MovieFile.Path = Path.GetDirectoryName(_curMovie.FaS.Filename)
                                    MovieFile.IsSingle = _curMovie.FaS.isSingle
                                    If Not IsNothing(_curMovie.Movie.FileInfo) Then
                                        If _curMovie.Movie.FileInfo.StreamDetails.Video.Count > 0 Then
                                            tVid = Master.GetBestVideo(_curMovie.Movie.FileInfo)
                                            tRes = Master.GetResFromDimensions(tVid)
                                            MovieFile.Resolution = String.Format("{0}", If(String.IsNullOrEmpty(tRes), "Unknown", tRes))
                                        End If

                                        If _curMovie.Movie.FileInfo.StreamDetails.Audio.Count > 0 Then
                                            tAud = Master.GetBestAudio(_curMovie.Movie.FileInfo)
                                            MovieFile.Audio = String.Format("{0}-{1}ch", If(String.IsNullOrEmpty(tAud.Codec), "Unknown", tAud.Codec), If(String.IsNullOrEmpty(tAud.Channels), "Unknown", tAud.Channels))
                                        End If
                                    End If
                                    '
                                    Dim plen As Integer
                                    For Each i As String In FFRenamer.MovieFolders
                                        If i.EndsWith(Path.DirectorySeparatorChar) Then i = Path.GetDirectoryName(i)
                                        If i = MovieFile.Path.Substring(0, i.Length) Then
                                            plen = String.Concat(i, Path.DirectorySeparatorChar).Length
                                            If MovieFile.Path.Length >= plen Then
                                                MovieFile.Path = MovieFile.Path.Substring(plen)
                                            Else
                                                MovieFile.Path = String.Empty
                                            End If
                                            MovieFile.BasePath = i
                                            Exit For
                                        End If
                                    Next
                                    MovieFile.FileName = Path.GetFileNameWithoutExtension(Master.CleanStackingMarkers(_curMovie.FaS.Filename))

                                    FFRenamer.AddMovie(MovieFile)

                                    Me.bwLoadInfo.ReportProgress(iProg, _curMovie.ListTitle)
                                End If
                            End If

                            iProg += 1

                            If bwLoadInfo.CancellationPending Then
                                e.Cancel = True
                                Return
                            End If
                        End While
                        e.Result = True
                    Else
                        e.Cancel = True
                    End If
                End Using
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub bwLoadInfo_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadInfo.ProgressChanged
        If e.ProgressPercentage >= 0 Then
            Me.pbCompile.Value = e.ProgressPercentage
            Me.lblFile.Text = e.UserState
        Else
            Me.pbCompile.Maximum = Convert.ToInt32(e.UserState)
        End If

    End Sub
    Private Sub bwLoadInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadInfo.RunWorkerCompleted
        '//
        ' Thread finished: display it if not cancelled
        '\\
        Try
            If Not e.Cancelled Then
                Rename_Button.Enabled = True
                isLoaded = True
                tmrSimul.Enabled = True
            Else
            End If
            Me.pnlCancel.Visible = False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub DoCancel()
        Try
            Me.bwLoadInfo.CancelAsync()
            btnCancel.Visible = False
            lblCompiling.Visible = False
            pbCompile.Style = ProgressBarStyle.Marquee
            pbCompile.MarqueeAnimationSpeed = 25
            lblCanceling.Visible = True
            lblFile.Visible = False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgBulkRename_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Me.bwLoadInfo.IsBusy Then
            Me.DoCancel()
            Do While Me.bwLoadInfo.IsBusy
                Application.DoEvents()
            Loop
        End If
    End Sub
    Private Sub dlgBulkRename_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()

        Try
            ' Show Cancel Panel
            btnCancel.Visible = True
            lblCompiling.Visible = True
            pbCompile.Visible = True
            pbCompile.Style = ProgressBarStyle.Continuous
            lblCanceling.Visible = False
            pnlCancel.Visible = True
            Application.DoEvents()

            'Start worker
            Me.bwLoadInfo = New System.ComponentModel.BackgroundWorker
            Me.bwLoadInfo.WorkerSupportsCancellation = True
            Me.bwLoadInfo.WorkerReportsProgress = True
            Me.bwLoadInfo.RunWorkerAsync()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub dgvMoviesList_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvMoviesList.CellPainting
        Try
            If e.RowIndex = -1 AndAlso e.ColumnIndex >= 1 AndAlso e.ColumnIndex <= 4 Then
                e.PaintBackground(e.ClipBounds, True)
                Dim strColumn() As String = {"Folder", "Filename", "New Folder", "New Filename"}

                e.Graphics.DrawString(strColumn(e.ColumnIndex - 1), e.CellStyle.Font, _
                    New SolidBrush(e.CellStyle.ForeColor), e.CellBounds.X + 1, e.CellBounds.Y + 4, _
                    StringFormat.GenericDefault)
                e.Handled = True
            End If
            If dgvMoviesList.ColumnCount > 5 AndAlso e.RowIndex >= 0 Then
                If dgvMoviesList.Rows(e.RowIndex).Cells(5).Value Then ' Locked
                    Dim newRect As New Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, _
                        e.CellBounds.Width - 4, e.CellBounds.Height - 4)
                    Dim backColorBrush As New SolidBrush(e.CellStyle.BackColor)
                    Dim gridBrush As New SolidBrush(Me.dgvMoviesList.GridColor)
                    Dim gridLinePen As New Pen(gridBrush)
                    Try
                        ' Erase the cell.
                        If e.State And DataGridViewElementStates.Selected Then
                            e.Graphics.FillRectangle(New SolidBrush(e.CellStyle.SelectionBackColor), e.CellBounds)
                        Else
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds)
                        End If
                        ' Draw the grid lines (only the right and bottom lines;
                        ' DataGridView takes care of the others).
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, _
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, _
                            e.CellBounds.Bottom - 1)
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, _
                            e.CellBounds.Top, e.CellBounds.Right - 1, _
                            e.CellBounds.Bottom)
                        ' Draw the inset highlight box.
                        If Not IsNothing(e.Value) Then
                            'Dim f As New Font(e.CellStyle.Font, FontStyle.Strikeout)
                            e.Graphics.DrawString(CStr(e.Value), e.CellStyle.Font, _
                            Brushes.Gray, e.CellBounds.X + 1, e.CellBounds.Y + 4, _
                            StringFormat.GenericDefault)
                            Dim pointS As New Point(e.CellBounds.Left, CInt((e.CellBounds.Top + e.CellBounds.Bottom) / 2))
                            Dim pointE As New Point(e.CellBounds.Right, CInt((e.CellBounds.Top + e.CellBounds.Bottom) / 2))
                            If e.ColumnIndex = 0 Then pointS.X += 4
                            If e.ColumnIndex = 4 Then pointE.X -= 4
                            e.Graphics.DrawLine(New Pen(Color.DarkGray), pointS, pointE)
                            e.Handled = True
                        End If

                    Finally
                        gridLinePen.Dispose()
                        gridBrush.Dispose()
                        backColorBrush.Dispose()
                    End Try
                End If
            End If
            If ((e.ColumnIndex = 3 OrElse e.ColumnIndex = 4) AndAlso e.RowIndex >= 0) AndAlso Not dgvMoviesList.Rows(e.RowIndex).Cells(5).Value Then
                If Not IsNothing(e.Value) AndAlso Not dgvMoviesList.Rows(e.RowIndex).Cells(e.ColumnIndex - 2).Value = e.Value Then
                    Dim newRect As New Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, _
                        e.CellBounds.Width - 4, e.CellBounds.Height - 4)
                    Dim backColorBrush As New SolidBrush(e.CellStyle.BackColor)
                    Dim gridBrush As New SolidBrush(Me.dgvMoviesList.GridColor)
                    Dim gridLinePen As New Pen(gridBrush)
                    Try
                        ' Erase the cell.
                        If e.State And DataGridViewElementStates.Selected Then
                            e.Graphics.FillRectangle(New SolidBrush(e.CellStyle.SelectionBackColor), e.CellBounds)
                        Else
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds)
                        End If
                        ' Draw the grid lines (only the right and bottom lines;
                        ' DataGridView takes care of the others).
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, _
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, _
                            e.CellBounds.Bottom - 1)
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, _
                            e.CellBounds.Top, e.CellBounds.Right - 1, _
                            e.CellBounds.Bottom)
                        ' Draw the inset highlight box.

                        If Not IsNothing(e.Value) Then
                            Dim tb As Brush
                            If (dgvMoviesList.Rows(e.RowIndex).Cells(6).Value AndAlso e.ColumnIndex = 3) OrElse (dgvMoviesList.Rows(e.RowIndex).Cells(7).Value AndAlso e.ColumnIndex = 4) Then
                                tb = Brushes.Red
                            Else
                                tb = Brushes.Purple
                            End If
                            e.Graphics.DrawString(CStr(e.Value), e.CellStyle.Font, _
                            tb, e.CellBounds.X + 1, e.CellBounds.Y + 4, _
                            StringFormat.GenericDefault)
                            e.Handled = True
                        End If

                    Finally
                        gridLinePen.Dispose()
                        gridBrush.Dispose()
                        backColorBrush.Dispose()
                    End Try
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub txtFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFile.TextChanged
        Try
            tmrSimul.Enabled = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub tmrSimul_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSimul.Tick
        Try
            'Need to make simulate thread safe
            tmrSimul.Enabled = False
            If isLoaded Then
                FFRenamer.ProccessFiles(txtFolder.Text, txtFile.Text, txtFolderNotSingle.Text)
                Simulate()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If DoneRename Then
            CancelRename = True
        Else
            DoCancel()
        End If
    End Sub

    Private Sub dlgBulkRename_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim frmToolTip As New ToolTip()

        Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
        Using g As Graphics = Graphics.FromImage(iBackground)
            g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
            Me.pnlTop.BackgroundImage = iBackground
        End Using

        'testing proposes
        Dim s As String = String.Concat("$T = Title", vbCrLf, "$X. (Replace Space with .)", vbCrLf, "$D = Directory", vbCrLf, "$F = File Name", vbCrLf, "$Y = Year", vbCrLf, "$R = Resolution", vbCrLf, "$A = Audio", vbCrLf, "$S = Source")
        lblLabel.Text = s.Replace(vbCrLf, "    ")
        frmToolTip.SetToolTip(txtFolder, s)
        frmToolTip.SetToolTip(txtFile, s)
        txtFolder.Text = Master.eSettings.FoldersPattern
        txtFile.Text = Master.eSettings.FilesPattern
    End Sub


    Private Sub Rename_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rename_Button.Click
        DoneRename = True
        pnlCancel.Visible = True
        lblCompiling.Text = "Renaming..."
        pbCompile.Maximum = FFRenamer.GetMoviesCount
        pbCompile.Value = 0
        Application.DoEvents()
        'Start worker
        Me.bwDoRename = New System.ComponentModel.BackgroundWorker
        Me.bwDoRename.WorkerSupportsCancellation = True
        Me.bwDoRename.WorkerReportsProgress = True
        Me.bwDoRename.RunWorkerAsync()
    End Sub
    Function ShowProgressRename(ByVal mov As String, ByVal iProg As Integer)
        Me.bwDoRename.ReportProgress(iProg, mov.ToString)
        If CancelRename Then Return False
        Return True
    End Function
    Private Sub bwDoRename_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDoRename.DoWork
        FFRenamer.DoRename(AddressOf ShowProgressRename)
    End Sub
    Private Sub bwDoRename_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwDoRename.ProgressChanged
        pbCompile.Value = e.ProgressPercentage
        lblFile.Text = e.UserState
    End Sub
    Private Sub bwbwDoRename_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDoRename.RunWorkerCompleted
        pnlCancel.Visible = False
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub txtFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolder.TextChanged
        Try
            tmrSimul.Enabled = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub txtFolderNotSingle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolderNotSingle.TextChanged
        Try
            tmrSimul.Enabled = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub



    Private Sub tsmUnlockAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmUnlockAll.Click
        setLockAll(False)
    End Sub

    Private Sub tsmLockAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmLockAll.Click
        setLockAll(True)
    End Sub
    Private Sub cmsMovieList_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmsMovieList.Opening
        Dim count As Integer = FFRenamer.GetCount
        Dim lockcount As Integer = FFRenamer.GetCountLocked
        If count > 0 Then
            If lockcount > 0 Then
                tsmUnlockAll.Visible = True
                If lockcount < count Then
                    tsmLockAll.Visible = True
                Else
                    tsmLockAll.Visible = False
                End If
                If lockcount = count Then
                    tsmLockAll.Visible = False
                End If

            Else
                tsmLockAll.Visible = True
                tsmUnlockAll.Visible = False
            End If
        Else
            tsmUnlockAll.Visible = False
            tsmLockAll.Visible = False
        End If
        tsmLockMovie.Visible = False
        tsmUnlockMovie.Visible = False
        For Each row As DataGridViewRow In dgvMoviesList.SelectedRows
            If row.Cells(5).Value Then
                tsmUnlockMovie.Visible = True
            Else
                tsmLockMovie.Visible = True
            End If
        Next

    End Sub

    Private Sub tsmLockMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmLockMovie.Click
        setLock(True)
    End Sub

    Private Sub tsmUnlockMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmUnlockMovie.Click
        setLock(False)
    End Sub
    Sub setLockAll(ByVal lock As Boolean)
        Try
            FFRenamer.SetIsLocked(String.Empty, String.Empty, False)
            For Each row As DataGridViewRow In dgvMoviesList.Rows
                row.Cells(5).Value = lock
            Next
            dgvMoviesList.Refresh()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Sub setLock(ByVal lock As Boolean)
        For Each row As DataGridViewRow In dgvMoviesList.SelectedRows
            FFRenamer.SetIsLocked(row.Cells(1).Value, row.Cells(2).Value, lock)
            row.Cells(5).Value = lock
        Next
        dgvMoviesList.Refresh()
    End Sub


    Private Sub dgvMoviesList_ColumnWidthChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewColumnEventArgs) Handles dgvMoviesList.ColumnWidthChanged
        If Not dgvMoviesList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None OrElse dgvMoviesList.Columns.Count < 9 OrElse dgvMoviesList.Tag Then Return
        Dim sum As Integer = 0
        For Each c As DataGridViewColumn In dgvMoviesList.Columns
            If c.Visible Then sum += c.Width
        Next
        If sum < dgvMoviesList.Width Then
            e.Column.Width = dgvMoviesList.Width - (sum - e.Column.Width)
        End If
    End Sub
End Class
