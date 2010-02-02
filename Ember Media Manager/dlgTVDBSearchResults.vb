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

Public Class dlgTVDBSearchResults
    Friend WithEvents bwDownloadPic As New System.ComponentModel.BackgroundWorker

    Private TVDB As New TVDB.Scraper
    Private sHTTP As New HTTP

    Private Structure Results
        Dim Result As Image
    End Structure

    Private Structure Arguments
        Dim pURL As String
    End Structure

    Private Sub bwDownloadPic_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadPic.DoWork

        Dim Args As Arguments = DirectCast(e.Argument, Arguments)
        e.Result = New Results With {.Result = sHTTP.DownloadImage(String.Format("http://{0}/banners/{1}", Master.eSettings.TVDBMirror, Args.pURL))}

    End Sub

    Private Sub bwDownloadPic_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadPic.RunWorkerCompleted

        Dim Res As Results = DirectCast(e.Result, Results)

        Try
            Me.pbBanner.Image = Res.Result
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ControlsVisible(ByVal areVisible As Boolean)
        Me.pbBanner.Visible = areVisible
        Me.lblTitle.Visible = areVisible
        Me.lblAiredHeader.Visible = areVisible
        Me.lblAired.Visible = areVisible
        Me.lblPlotHeader.Visible = areVisible
        Me.txtOutline.Visible = areVisible
    End Sub

    Private Sub dlgTVDBSearchResults_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            AddHandler TVDB.SearchResultsDownloaded, AddressOf SearchResultsDownloaded

            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SearchResultsDownloaded(ByVal sResults As List(Of TVDB.TVSearchResults))
        Dim lItem As ListViewItem

        Me.lvSearchResults.Items.Clear()
        Me.ClearInfo()

        If Not IsNothing(sResults) AndAlso sResults.Count > 0 Then
            For Each sRes As TVDB.TVSearchResults In sResults
                lItem = New ListViewItem(sRes.Name)
                lItem.SubItems.Add(sRes.Language.LongLang)
                lItem.Tag = sRes
                Me.lvSearchResults.Items.Add(lItem)
            Next
        End If

        Me.pnlLoading.Visible = False

    End Sub

    Private Sub ClearInfo()
        Me.ControlsVisible(False)
        Me.lblTitle.Text = String.Empty
        Me.lblAired.Text = String.Empty
        Me.pbBanner.Image = Nothing
        TVDB.CancelAsync()
    End Sub

    Public Overloads Function ShowDialog(ByVal sName As String) As Windows.Forms.DialogResult

        Me.Text = String.Concat(Master.eLang.GetString(301, "Search Results - "), sName)
        TVDB.GetSearchResultsAsync(sName)

        Return MyBase.ShowDialog()
    End Function

    Private Sub lvSearchResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvSearchResults.SelectedIndexChanged
        Me.ClearInfo()
        If Me.lvSearchResults.SelectedItems.Count > 0 Then
            Dim SelectedShow As TVDB.TVSearchResults = DirectCast(Me.lvSearchResults.SelectedItems(0).Tag, TVDB.TVSearchResults)
            If Not String.IsNullOrEmpty(SelectedShow.Banner) Then
                If Me.bwDownloadPic.IsBusy Then
                    Me.bwDownloadPic.CancelAsync()
                End If

                Me.bwDownloadPic = New System.ComponentModel.BackgroundWorker
                Me.bwDownloadPic.WorkerSupportsCancellation = True
                Me.bwDownloadPic.RunWorkerAsync(New Arguments With {.pURL = SelectedShow.Banner})
            End If

            Me.OK_Button.Tag = SelectedShow.ID
            Me.lblTitle.Text = SelectedShow.Name
            Me.txtOutline.Text = SelectedShow.Overview
            Me.lblAired.Text = SelectedShow.Aired
        End If
        Me.ControlsVisible(True)
    End Sub
End Class
