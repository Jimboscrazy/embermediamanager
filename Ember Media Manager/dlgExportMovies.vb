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
Imports System.Text
Imports System.Text.RegularExpressions

Public Class dlgExportMovies
    Dim HTMLBody As New StringBuilder
    Dim _movies As New List(Of Master.DBMovie)
    Dim bFiltered As Boolean = False
    Dim bCancelled As Boolean = False
    Friend WithEvents bwLoadInfo As New System.ComponentModel.BackgroundWorker

    Private Sub dlgExportMovies_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Me.bwLoadInfo.IsBusy Then
            Me.DoCancel()
            Do While Me.bwLoadInfo.IsBusy
                Application.DoEvents()
            Loop
        End If
    End Sub

    Private Sub bwLoadInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadInfo.DoWork
        '//
        ' Thread to load movieinformation (from nfo)
        '\\
        Try
            ' Clean up Movies List if any
            _movies.Clear()
            ' Load nfo movies using path from DB
            Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                Dim _tmpMovie As New Master.DBMovie
                Dim _ID As Integer
                Dim iProg As Integer = 0
                SQLNewcommand.CommandText = String.Concat("SELECT COUNT(id) AS mcount FROM movies;")
                Using SQLcount As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    Me.bwLoadInfo.ReportProgress(-1, SQLcount("mcount")) ' set maximum
                End Using
                SQLNewcommand.CommandText = String.Concat("SELECT ID FROM movies ORDER BY ListTitle ASC;")
                Using SQLreader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                    If SQLreader.HasRows Then
                        While SQLreader.Read()
                            _ID = SQLreader("ID")
                            _tmpMovie = Master.DB.LoadMovieFromDB(_ID)
                            _movies.Add(_tmpMovie)
                            Me.bwLoadInfo.ReportProgress(iProg, _tmpMovie.ListTitle) '  show File
                            iProg += 1
                            If bwLoadInfo.CancellationPending Then
                                e.Cancel = True
                                Return
                            End If
                        End While
                        BuildHTML()
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
    Sub BuildHTML(Optional ByVal bSearch As Boolean = False, Optional ByVal strFilter As String = "", Optional ByVal strIn As String = "")
        Try
            ' Build HTML Documment in Code ... ugly but will work until new option
            Dim tVid As New MediaInfo.Video
            Dim tAud As New MediaInfo.Audio
            Dim tRes As String = String.Empty
            HTMLBody = New StringBuilder 'blank it
            HTMLBody.Append(My.Resources.MovieListHeader)
            HTMLBody.Append(My.Resources.MediaListLogo)
            HTMLBody.Append(My.Resources.MovieListTableStart)
            If bSearch Then
                bFiltered = True
            Else
                bFiltered = False
            End If
            ' For now fixed Cols
            Dim rowHeader As New StringBuilder
            rowHeader.Append(My.Resources.MovieListTableRowStart)
            rowHeader.AppendFormat(My.Resources.MovieListTableHeader, "Title")
            rowHeader.AppendFormat(My.Resources.MovieListTableHeader, "Year")
            rowHeader.AppendFormat(My.Resources.MovieListTableHeader, "Video")
            rowHeader.AppendFormat(My.Resources.MovieListTableHeader, "Audio")
            rowHeader.Append(My.Resources.MovieListTableRowEnd)
            HTMLBody.Append(rowHeader)
            For Each _curMovie As Master.DBMovie In _movies
                Dim _vidDetails As String = String.Empty
                Dim _audDetails As String = String.Empty
                If Not IsNothing(_curMovie.Movie.FileInfo) Then
                    If _curMovie.Movie.FileInfo.StreamDetails.Video.Count > 0 Then
                        tVid = NFO.GetBestVideo(_curMovie.Movie.FileInfo)
                        tRes = NFO.GetResFromDimensions(tVid)
                        _vidDetails = String.Format("{0} / {1}", If(String.IsNullOrEmpty(tRes), "Unknown", tRes), If(String.IsNullOrEmpty(tVid.Codec), "Unknown", tVid.Codec))
                    End If

                    If _curMovie.Movie.FileInfo.StreamDetails.Audio.Count > 0 Then
                        tAud = NFO.GetBestAudio(_curMovie.Movie.FileInfo)
                        _audDetails = String.Format("{0} / {1}ch", If(String.IsNullOrEmpty(tAud.Codec), "Unknown", tAud.Codec), If(String.IsNullOrEmpty(tAud.Channels), "Unknown", tAud.Channels))
                    End If
                End If

                Dim row As New StringBuilder
                row.Append(My.Resources.MovieListTableRowStart)
                row.AppendFormat(My.Resources.MovieListTableCol, Web.HttpUtility.HtmlEncode(_curMovie.ListTitle))
                row.AppendFormat(My.Resources.MovieListTableCol, _curMovie.Movie.Year)
                row.AppendFormat(My.Resources.MovieListTableCol, _vidDetails)
                row.AppendFormat(My.Resources.MovieListTableCol, _audDetails)

                If bSearch Then
                    If (strIn = "Video Flag" And _vidDetails.Contains(strFilter)) Or _
                       (strIn = "Audio Flag" And _audDetails.Contains(strFilter)) Or _
                       (strIn = "Title" And _curMovie.Movie.Title.Contains(strFilter)) Or _
                       (strIn = "Year" And _curMovie.Movie.Year.Contains(strFilter)) Then
                        row.Append(My.Resources.MovieListTableRowEnd)
                        HTMLBody.Append(row)
                    End If
                Else
                    row.Append(My.Resources.MovieListTableRowEnd)
                    HTMLBody.Append(row)
                End If
            Next
            HTMLBody.Append(My.Resources.MovieListTableEnd)
            HTMLBody.Append(My.Resources.MovieListFooter)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub


    Private Sub bwLoadInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadInfo.RunWorkerCompleted
        '//
        ' Thread finished: display it if not cancelled
        '\\
        bCancelled = e.Cancelled
        If Not e.Cancelled Then
            wbMovieList.DocumentText = HTMLBody.ToString
        Else
            wbMovieList.DocumentText = "<center><h1 style=""color:Red;"">Cancelled</h1></center>"
        End If
        Me.pnlCancel.Visible = False

    End Sub

    Private Sub bwLoadInfo_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadInfo.ProgressChanged
        If e.ProgressPercentage >= 0 Then
            Me.pbCompile.Value = e.ProgressPercentage
            Me.lblFile.Text = e.UserState
        Else
            Me.pbCompile.Maximum = Convert.ToInt32(e.UserState)
        End If

    End Sub

    Private Sub DoCancel()
        Me.bwLoadInfo.CancelAsync()
        btnCancel.Visible = False
        lblCompiling.Visible = False
        pbCompile.Style = ProgressBarStyle.Marquee
        pbCompile.MarqueeAnimationSpeed = 25
        lblCanceling.Visible = True
        lblFile.Visible = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(ByVal sender As System.Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles wbMovieList.DocumentCompleted
        If Not bCancelled Then
            wbMovieList.Visible = True
            Me.Save_Button.Enabled = True
            pnlSearch.Enabled = True
            Reset_Button.Enabled = bFiltered
        End If
    End Sub


    Private Sub Save_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save_Button.Click
        Dim saveHTML As New SaveFileDialog()
        Dim myStream As Stream
        saveHTML.Filter = "HTML files (*.htm)|*.htm"
        saveHTML.FilterIndex = 2
        saveHTML.RestoreDirectory = True

        If saveHTML.ShowDialog() = DialogResult.OK Then

            myStream = saveHTML.OpenFile()
            If Not IsNothing(myStream) Then
                myStream.Write(System.Text.Encoding.ASCII.GetBytes(wbMovieList.DocumentText), 0, wbMovieList.DocumentText.Length)
                myStream.Close()
            End If
        End If

    End Sub

    Private Sub Search_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Search_Button.Click
        pnlSearch.Enabled = False
        BuildHTML(True, txtSearch.Text, cbSearch.Text)
        wbMovieList.DocumentText = HTMLBody.ToString
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        If txtSearch.Text <> "" And cbSearch.Text <> "" Then
            Search_Button.Enabled = True
        Else
            Search_Button.Enabled = False
        End If
    End Sub

    Private Sub cbSearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSearch.SelectedIndexChanged
        If txtSearch.Text <> "" And cbSearch.Text <> "" Then
            Search_Button.Enabled = True
        Else
            Search_Button.Enabled = False
        End If
    End Sub

    Private Sub Reset_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Reset_Button.Click
        pnlSearch.Enabled = False
        BuildHTML(False)
        wbMovieList.DocumentText = HTMLBody.ToString
    End Sub

    Private Sub dlgMoviesReport_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        Me.Activate()

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
    End Sub
End Class


