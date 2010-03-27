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

Imports System.Text.RegularExpressions

Public Class dlgSearchResults

#Region "Fields"
    Public SelectIdx As Integer = -1
    Friend WithEvents bwDownloadPic As New System.ComponentModel.BackgroundWorker
    Friend WithEvents tmrLoad As New System.Windows.Forms.Timer
    Friend WithEvents tmrWait As New System.Windows.Forms.Timer


    Private sHTTP As New HTTP
    Dim UseOFDBGenre As Boolean
    Dim UseOFDBOutline As Boolean
    Dim UseOFDBPlot As Boolean
    Dim UseOFDBTitle As Boolean
    Private _currnode As Integer = -1
    Private _prevnode As Integer = -2
    Private mList As List(Of XMLScraper.ScraperLib.ScrapeResultsEntity)
#End Region 'Fields

#Region "Methods"


    Public Overloads Function ShowDialog(ByVal Res As List(Of XMLScraper.ScraperLib.ScrapeResultsEntity), ByVal sMovieTitle As String) As Windows.Forms.DialogResult
        '//
        ' Overload to pass data
        '\\

        Me.Text = String.Concat(Master.eLang.GetString(301, "Search Results - "), sMovieTitle)
        mList = Res
        SearchResultsDownloaded()

        Return MyBase.ShowDialog()
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not String.IsNullOrEmpty(Me.txtSearch.Text) Then
            Me.OK_Button.Enabled = False
            Me.ClearInfo()
            Me.Label3.Text = Master.eLang.GetString(568, "Searching IMDB...")
            Me.pnlLoading.Visible = True
        End If
    End Sub


    Private Sub bwDownloadPic_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadPic.DoWork
        Dim Args As Arguments = DirectCast(e.Argument, Arguments)

        sHTTP.StartDownloadImage(Args.pURL)

        While sHTTP.IsDownloading
            Application.DoEvents()
        End While

        e.Result = New Results With {.Result = sHTTP.Image}
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Master.tmpMovie.Clear()

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub ClearInfo()
        Me.ControlsVisible(False)
        Me.lblTitle.Text = String.Empty
        Me.lblYear.Text = String.Empty

        Master.tmpMovie.Clear()

    End Sub

    Private Sub ControlsVisible(ByVal areVisible As Boolean)
        Me.lblYearHeader.Visible = areVisible
        Me.lblYear.Visible = areVisible
        Me.lblTitle.Visible = areVisible

    End Sub

    Private Sub dlgIMDBSearchResults_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        Me.AcceptButton = Me.OK_Button
    End Sub

    Private Sub dlgSearchResults_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.tmrWait.Enabled = False
        Me.tmrWait.Interval = 250
        Me.tmrLoad.Enabled = False
        Me.tmrLoad.Interval = 250

        Try
            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgIMDBSearchResults_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
        Me.tvResults.Focus()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.Close()
    End Sub

    Private Sub SearchResultsDownloaded()
        '//
        ' Process the results that IMDB gave us
        '\\

        Try
            Me.tvResults.Nodes.Clear()
            Me.ClearInfo()
            'Dim TnP As New TreeNode(String.Format(Master.eLang.GetString(297, "Matches ({0})"), mList.Count))
            Dim selNode As New TreeNode

            For c = 0 To mList.Count - 1
                Dim title As String = Web.HttpUtility.HtmlDecode(mList(c).Title)
                Me.tvResults.Nodes.Add(New TreeNode() With {.Tag = c, .Text = String.Concat(title, If(Not String.IsNullOrEmpty(mList(c).Year.ToString), String.Format(" ({0})", mList(c).Year), String.Empty))})
            Next

            'Me.tvResults.Nodes.Add(TnP)
            'selNode = Me.tvResults.Nodes.FirstNode
            Me.pnlLoading.Visible = False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetUp()
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.Label2.Text = Master.eLang.GetString(285, "View details of each result to find the proper movie.")
        Me.Label1.Text = Master.eLang.GetString(286, "Movie Search Results")
        Me.lblYearHeader.Text = Master.eLang.GetString(49, "Year:")
        Me.Label3.Text = Master.eLang.GetString(568, "Searching ...")
    End Sub

    Private Sub tmrLoad_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrWait.Tick
        Me.tmrWait.Enabled = False
        Me.tmrLoad.Enabled = False

        Me.Label3.Text = Master.eLang.GetString(290, "Downloading details...")
    End Sub

    Private Sub tmrWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrWait.Tick
        If Not Me._prevnode = Me._currnode Then
            Me._prevnode = Me._currnode
            Me.tmrLoad.Enabled = True
        Else
            Me.tmrLoad.Enabled = False
        End If
    End Sub

    Private Sub tvResults_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvResults.AfterSelect
        Try
            SelectIdx = Convert.ToInt32(Me.tvResults.SelectedNode.Tag)
            Me.tmrWait.Enabled = False
            Me.tmrLoad.Enabled = False
            Me.ClearInfo()
            'Me.OK_Button.Enabled = False
            Me.OK_Button.Enabled = True
            Me.pnlLoading.Visible = False
            Dim idx As Integer = Convert.ToInt32(Me.tvResults.SelectedNode.Tag)
            lblTitle.Text = Web.HttpUtility.HtmlDecode(mList(idx).Title)
            lblYear.Text = mList(idx).Year.ToString
            'If Not IsNothing(Me.tvResults.SelectedNode.Tag) AndAlso Not String.IsNullOrEmpty(Me.tvResults.SelectedNode.Tag.ToString) Then
            'Me.pnlLoading.Visible = True
            'Me.tmrWait.Enabled = True
            'Else
            'Me.pnlLoading.Visible = False
            'End If
            ControlsVisible(True)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub tvResults_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvResults.GotFocus
        Me.AcceptButton = Me.OK_Button
    End Sub
    Private Sub txtSearch_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.GotFocus
        Me.AcceptButton = Me.btnSearch
    End Sub

#End Region 'Methods

#Region "Nested Types"

    Private Structure Arguments

#Region "Fields"

        Dim pURL As String

#End Region 'Fields

    End Structure

    Private Structure Results

#Region "Fields"

        Dim Result As Image

#End Region 'Fields

    End Structure

#End Region 'Nested Types

End Class