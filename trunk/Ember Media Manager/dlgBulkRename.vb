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
    Private bindingSource1 As New BindingSource()
    Private isLoaded As Boolean = False
    Private FFRenamer As New FileFolderRenamer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub


    Public Sub Simulate()
        Try
            With Me.dgvMoviesList
                .DataSource = Nothing
                .Rows.Clear()
                .AutoGenerateColumns = True
                bindingSource1.DataSource = FFRenamer.GetMovies
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                .DataSource = bindingSource1
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
            ' Clean up Movies List if any
            'FFRemamer._movies.Clear()
            ' Load nfo movies using path from DB
            Using SQLNewcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                Dim _tmpMovie As New Media.Movie
                Dim _tmpPath As String = String.Empty
                Dim iProg As Integer = 0
                SQLNewcommand.CommandText = String.Concat("SELECT COUNT(id) AS mcount FROM movies;")
                Using SQLcount As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    Me.bwLoadInfo.ReportProgress(-1, SQLcount("mcount")) ' set maximum
                End Using
                SQLNewcommand.CommandText = String.Concat("SELECT path, type FROM movies ORDER BY title ASC;")
                Using SQLreader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    If SQLreader.HasRows Then
                        While SQLreader.Read()
                            _tmpPath = Master.GetNfoPath(SQLreader("path").ToString, SQLreader("type"))
                            If Not String.IsNullOrEmpty(_tmpPath) Then
                                MovieFile = New FileFolderRenamer.FileRename
                                _tmpMovie = Master.LoadMovieFromNFO(_tmpPath)
                                MovieFile.Title = _tmpMovie.Title
                                MovieFile.Year = _tmpMovie.Year
                                If Not IsNothing(_tmpMovie.FileInfo) AndAlso (Not IsNothing(_tmpMovie.FileInfo.StreamDetails.Video) OrElse _tmpMovie.FileInfo.StreamDetails.Audio.Count > 0) Then
                                    MovieFile.Resolution = Master.GetResFromDimensions(_tmpMovie.FileInfo)
                                    MovieFile.Audio = Master.GetBestAudio(_tmpMovie.FileInfo).Codec
                                End If
                                MovieFile.BasePath = Path.GetDirectoryName(SQLreader("path").ToString)
                                MovieFile.Path = Path.GetDirectoryName(SQLreader("path").ToString)
                                For Each i As String In ffrenamer.MovieFolders
                                    If i = MovieFile.Path.Substring(0, i.Length) Then
                                        MovieFile.Path = MovieFile.Path.Substring(String.Concat(i, Path.DirectorySeparatorChar).Length)
                                        MovieFile.BasePath = i
                                        Exit For
                                    End If
                                Next
                                MovieFile.FileName = Path.GetFileNameWithoutExtension(Master.CleanStackingMarkers(SQLreader("path").ToString))

                                FFRenamer.AddMovie(MovieFile)
                            End If
                            Me.bwLoadInfo.ReportProgress(iProg, _tmpMovie.Title)
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
            If (e.ColumnIndex = 3 OrElse e.ColumnIndex = 4) AndAlso e.RowIndex >= 0 Then
                If e.Value IsNot Nothing AndAlso Not dgvMoviesList.Rows(e.RowIndex).Cells(e.ColumnIndex - 2).Value = e.Value Then
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
                        If (e.Value IsNot Nothing) Then
                            e.Graphics.DrawString(CStr(e.Value), e.CellStyle.Font, _
                            Brushes.Red, e.CellBounds.X + 2, e.CellBounds.Y + 3, _
                            StringFormat.GenericDefault)
                        End If
                        e.Handled = True
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
                FFRenamer.ProccessFiles(txtFolder.Text, txtFile.Text)
                Simulate()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DoCancel()
    End Sub

    Private Sub dlgBulkRename_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim frmToolTip As New ToolTip()
        'testing proposes
        Dim s As String = String.Concat("$T = Title", vbCrLf, "$t = Title (Space = .)", vbCrLf, "$D = Directory", vbCrLf, "$F = File Name", vbCrLf, "$Y = Year", vbCrLf, "$R = Resolution", vbCrLf, "$A = Audio")
        lblLabel.Text = s.Replace(vbCrLf, "    ")
        frmToolTip.SetToolTip(txtFolder, s)
    End Sub


    Private Sub Rename_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rename_Button.Click
        FFRenamer.DoRename()
    End Sub

    Private Sub txtFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolder.TextChanged
        Try
            tmrSimul.Enabled = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

End Class
