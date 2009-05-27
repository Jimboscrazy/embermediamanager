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

Imports System.Windows.Forms
Imports System.Text.RegularExpressions

Public Class dlgIMDBSearchResults

    Friend WithEvents bwDownloadPic As New System.ComponentModel.BackgroundWorker

    Private IMDB As New IMDB.Scraper

    Private Structure Results
        Dim Result As Image
    End Structure

    Private Structure Arguments
        Dim pURL As String
    End Structure

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            If Me.chkManual.Checked AndAlso Me.btnVerify.Enabled Then
                If Not Regex.IsMatch(Me.txtIMDBID.Text.Replace("tt", String.Empty), "\d\d\d\d\d\d\d") Then
                    MsgBox("The ID you entered is not a valid IMDB ID", MsgBoxStyle.Exclamation, "Invalid Entry")
                    Exit Sub
                Else
                    If MsgBox("You have manually entered an IMDB ID but have not verified it is correct." & vbNewLine & vbNewLine & "Are you sure you want to continue without verification?", MsgBoxStyle.YesNo, "Continue without verification?") = MsgBoxResult.No Then
                        Exit Sub
                    Else
                        Master.tmpMovie.IMDBID = Me.txtIMDBID.Text.Replace("tt", String.Empty)
                    End If
                End If
            End If
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgIMDBSearchResults_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        Me.AcceptButton = Me.OK_Button
    End Sub

    Private Sub dlgIMDBSearchResults_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AddHandler IMDB.SearchMovieInfoDownloaded, AddressOf SearchMovieInfoDownloaded
        AddHandler IMDB.SearchResultsDownloaded, AddressOf SearchResultsDownloaded

        Try
            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Dim g As Graphics = Graphics.FromImage(iBackground)
            g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, 20), pnlTop.ClientRectangle)
            Me.pnlTop.BackgroundImage = iBackground
            g.Dispose()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SearchMovieInfoDownloaded(ByVal sPoster As String, ByVal bSuccess As Boolean)

        '//
        ' Info downloaded... fill form with data
        '\\

        Me.pnlLoading.Visible = False

        Try
            If bSuccess Then
                Me.ControlsVisible(True)
                Me.lblTitle.Text = Master.tmpMovie.Title
                Me.lblTagline.Text = Master.tmpMovie.Tagline
                Me.lblYear.Text = Master.tmpMovie.Year
                Me.lblDirector.Text = Master.tmpMovie.Director
                Me.lblGenre.Text = Master.tmpMovie.Genre
                Me.txtOutline.Text = Master.tmpMovie.Outline
                Me.lblIMDB.Text = Master.tmpMovie.IMDBID

                If Not String.IsNullOrEmpty(sPoster) Then
                    If Me.bwDownloadPic.IsBusy Then
                        Me.bwDownloadPic.CancelAsync()
                    End If

                    Me.bwDownloadPic = New System.ComponentModel.BackgroundWorker
                    Me.bwDownloadPic.WorkerSupportsCancellation = True
                    Me.bwDownloadPic.RunWorkerAsync(New Arguments With {.pURL = sPoster})
                End If

                Me.btnVerify.Enabled = False
            Else
                If Me.chkManual.Checked Then
                    MsgBox("Unable to retrieve movie details for the entered IMDB ID. Please check your entry and try again.", MsgBoxStyle.Exclamation, "Verification Failed")
                    Me.btnVerify.Enabled = True
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgIMDBSearchResults_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.tvResults.Focus()
    End Sub

    Private Sub tvResults_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvResults.AfterSelect
        Try
            Me.ClearInfo()
            Me.OK_Button.Enabled = True
            If Not IsNothing(e.Node.Tag) Then
                Me.pnlLoading.Visible = True
                IMDB.GetSearchMovieInfoAsync(e.Node.Tag, Master.tmpMovie)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwDownloadPic_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadPic.DoWork

        '//
        ' Thread to download image from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\

        Dim Args As Arguments = e.Argument

        Dim wrRequest As System.Net.WebRequest = System.Net.WebRequest.Create(Args.pURL)
        wrRequest.Timeout = 5000 'give it 5 seconds
        Try
            Dim wrResponse As System.Net.WebResponse = wrRequest.GetResponse()
            e.Result = New Results With {.Result = System.Drawing.Image.FromStream(wrResponse.GetResponseStream())}
        Catch
            e.Result = New Results With {.Result = Nothing}
        End Try

    End Sub

    Private Sub bwDownloadPic_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadPic.RunWorkerCompleted

        '//
        ' Thread finished: display pic if it was able to get one
        '\\

        Dim Res As Results = e.Result

        Try
            Me.pbPoster.Image = Res.Result
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ClearInfo()
        Me.ControlsVisible(False)
        Me.lblTitle.Text = String.Empty
        Me.lblTagline.Text = String.Empty
        Me.lblYear.Text = String.Empty
        Me.lblDirector.Text = String.Empty
        Me.lblGenre.Text = String.Empty
        Me.txtOutline.Text = String.Empty
        Me.lblIMDB.Text = String.Empty
        Me.pbPoster.Image = Nothing
    End Sub

    Private Sub ControlsVisible(ByVal areVisible As Boolean)
        Me.lblYearHeader.Visible = areVisible
        Me.lblDirectorHeader.Visible = areVisible
        Me.lblGenreHeader.Visible = areVisible
        Me.lblPlotHeader.Visible = areVisible
        Me.lblIMDBHeader.Visible = areVisible
        Me.txtOutline.Visible = areVisible
        Me.lblYear.Visible = areVisible
        Me.lblTagline.Visible = areVisible
        Me.lblTitle.Visible = areVisible
        Me.lblDirector.Visible = areVisible
        Me.lblGenre.Visible = areVisible
        Me.txtOutline.Visible = areVisible
        Me.lblIMDB.Visible = areVisible
        Me.pbPoster.Visible = areVisible
    End Sub

    Private Sub chkManual_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkManual.CheckedChanged
        Me.txtIMDBID.Enabled = Me.chkManual.Checked
        Me.btnVerify.Enabled = Me.chkManual.Checked
        Me.tvResults.Enabled = Not Me.chkManual.Checked

        If Not Me.chkManual.Checked Then
            txtIMDBID.Text = String.Empty
        End If
    End Sub

    Private Sub btnVerify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnVerify.Click
        If Regex.IsMatch(Me.txtIMDBID.Text.Replace("tt", String.Empty), "\d\d\d\d\d\d\d") Then
            IMDB.GetSearchMovieInfoAsync(Me.txtIMDBID.Text.Replace("tt", String.Empty), Master.tmpMovie)
        Else
            MsgBox("The ID you entered is not a valid IMDB ID", MsgBoxStyle.Exclamation, "Invalid Entry")
        End If
    End Sub

    Private Sub txtIMDBID_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIMDBID.GotFocus
        Me.AcceptButton = Me.btnVerify
    End Sub

    Private Sub txtIMDBID_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtIMDBID.TextChanged
        If Me.chkManual.Checked Then
            Me.btnVerify.Enabled = True
        End If
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not String.IsNullOrEmpty(Me.txtSearch.Text) Then
            IMDB.SearchMovieAsync(Me.txtSearch.Text)
        End If
    End Sub

    Private Sub SearchResultsDownloaded(ByVal M As IMDB.MovieSearchResults)

        '//
        ' Process the results that IMDB gave us
        '\\

        Try
            Me.tvResults.Nodes.Clear()
            Me.ClearInfo()
            If Not IsNothing(M) Then
                If M.PartialMatches.Count > 0 OrElse M.PopularTitles.Count > 0 OrElse M.ExactMatches.Count > 0 Then
                    Dim TnP As New TreeNode(String.Format("Partial Matches ({0})", M.PartialMatches.Count))
                    Dim selNode As New TreeNode

                    If M.PartialMatches.Count > 0 Then
                        For Each Movie As Media.Movie In M.PartialMatches
                            TnP.Nodes.Add(New TreeNode() With {.Text = Movie.Title, .Tag = Movie.IMDBID})
                        Next
                        TnP.Expand()
                        Me.tvResults.Nodes.Add(TnP)
                        selNode = TnP.FirstNode
                    End If

                    If M.ExactMatches.Count > 0 Then
                        If M.PartialMatches.Count > 0 Then
                            Me.tvResults.Nodes(TnP.Index).Collapse()
                        End If
                        TnP = New TreeNode("Exact Matches")
                        For Each Movie As Media.Movie In M.ExactMatches
                            TnP.Nodes.Add(New TreeNode() With {.Text = Movie.Title, .Tag = Movie.IMDBID})
                        Next
                        TnP.Expand()
                        Me.tvResults.Nodes.Add(TnP)
                        selNode = TnP.FirstNode
                    End If

                    If M.PopularTitles.Count > 0 Then
                        If M.PartialMatches.Count > 0 OrElse M.ExactMatches.Count > 0 Then
                            Me.tvResults.Nodes(TnP.Index).Collapse()
                        End If

                        TnP = New TreeNode(String.Format("Popular Titles ({0})", M.PopularTitles.Count))
                        For Each Movie As Media.Movie In M.PopularTitles
                            TnP.Nodes.Add(New TreeNode() With {.Text = Movie.Title, .Tag = Movie.IMDBID})
                        Next
                        TnP.Expand()
                        Me.tvResults.Nodes.Add(TnP)
                        selNode = TnP.FirstNode
                    End If

                    Me.tvResults.SelectedNode = selNode
                    Me.tvResults.Focus()
                Else
                    Me.tvResults.Nodes.Add(New TreeNode With {.Text = "No Matches Found"})
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub txtSearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.GotFocus
        Me.AcceptButton = Me.btnSearch
    End Sub

    Private Sub tvResults_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvResults.GotFocus
        Me.AcceptButton = Me.OK_Button
    End Sub

    Public Overloads Function ShowDialog(ByVal sMovieTitle As String) As Windows.Forms.DialogResult

        '//
        ' Overload to pass data
        '\\

        Me.Text = String.Concat("Search Results - ", sMovieTitle)
        IMDB.SearchMovieAsync(sMovieTitle)

        Return MyBase.ShowDialog()
    End Function

    Public Overloads Function ShowDialog(ByVal Res As IMDB.MovieSearchResults, ByVal sMovieTitle As String) As Windows.Forms.DialogResult

        '//
        ' Overload to pass data
        '\\

        Me.Text = String.Concat("Search Results - ", sMovieTitle)
        SearchResultsDownloaded(Res)

        Return MyBase.ShowDialog()
    End Function
End Class
