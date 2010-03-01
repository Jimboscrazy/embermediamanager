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

    Private sHTTP As New HTTP
    Private sInfo As EmberAPI.Structures.ScrapeInfo

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
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Me.lvSearchResults.SelectedItems.Count > 0 Then
            Me.Label3.Text = Master.eLang.GetString(780, "Downloading show info...")
            Me.pnlLoading.Visible = True
            Dim sResults As Scraper.TVSearchResults = DirectCast(Me.lvSearchResults.SelectedItems(0).Tag, Scraper.TVSearchResults)
            Me.sInfo.TVDBID = sResults.ID.ToString
            Me.sInfo.SelectedLang = sResults.Language.ShortLang
            Scraper.sObject.DownloadSeriesAsync(sInfo)
        End If

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
            ' *#### AddHandler Master.TVScraper.ScraperEvent, AddressOf TVScraperEvent
            AddHandler ModulesManager.Instance.TVScraperEvent, AddressOf TVScraperEvent
            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using

            Me.SetUp()
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub TVScraperEvent(ByVal eType As EmberAPI.Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
        Select Case eType
            Case EmberAPI.Enums.TVScraperEventType.SearchResultsDownloaded
                Dim lItem As ListViewItem
                Dim sResults As List(Of Scraper.TVSearchResults) = DirectCast(Parameter, List(Of Scraper.TVSearchResults))

                Me.lvSearchResults.Items.Clear()
                Me.ClearInfo()

                If Not IsNothing(sResults) AndAlso sResults.Count > 0 Then
                    For Each sRes As Scraper.TVSearchResults In sResults
                        lItem = New ListViewItem(sRes.Name)
                        lItem.SubItems.Add(sRes.Language.LongLang)
                        lItem.Tag = sRes
                        Me.lvSearchResults.Items.Add(lItem)
                    Next
                End If

                Me.pnlLoading.Visible = False
            Case EmberAPI.Enums.TVScraperEventType.ShowDownloaded
                Me.DialogResult = System.Windows.Forms.DialogResult.OK
                Me.Close()
        End Select
    End Sub

    Private Sub ClearInfo()
        Me.ControlsVisible(False)
        Me.lblTitle.Text = String.Empty
        Me.lblAired.Text = String.Empty
        Me.pbBanner.Image = Nothing
        Scraper.sObject.CancelAsync()
    End Sub

    Public Overloads Function ShowDialog(ByVal _sInfo As EmberAPI.Structures.ScrapeInfo) As Windows.Forms.DialogResult

        Me.sInfo = _sInfo
        Me.Text = String.Concat(Master.eLang.GetString(301, "Search Results - "), sInfo.ShowTitle)
        Scraper.sObject.GetSearchResultsAsync(sInfo)

        Return MyBase.ShowDialog()
    End Function

    Private Sub lvSearchResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvSearchResults.SelectedIndexChanged
        Me.ClearInfo()
        If Me.lvSearchResults.SelectedItems.Count > 0 Then
            Dim SelectedShow As Scraper.TVSearchResults = DirectCast(Me.lvSearchResults.SelectedItems(0).Tag, Scraper.TVSearchResults)
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
            Me.OK_Button.Enabled = True
        End If
        Me.ControlsVisible(True)
    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(781, "TV Search Results")
        Me.Label1.Text = Me.Text
        Me.Label2.Text = Master.eLang.GetString(782, "View details of each result to find the proper TV show.")
        Me.lblAiredHeader.Text = Master.eLang.GetString(658, "Aired:")
        Me.lblPlotHeader.Text = Master.eLang.GetString(783, "Plot Summary:")

        Me.lvSearchResults.Columns(0).Text = Master.eLang.GetString(21, "Title")
        Me.lvSearchResults.Columns(1).Text = Master.eLang.GetString(610, "Language")

        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")

    End Sub
End Class
