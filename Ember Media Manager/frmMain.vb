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

Imports System
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Bitmap
Imports System.Text.RegularExpressions


Public Class frmMain

#Region "Declarations"

    ' ########################################
    ' ############# DECLARATIONS #############
    ' ########################################
    Friend WithEvents bwFolderData As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwMediaInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwLoadInfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwDownloadPic As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwPrelim As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwScraper As New System.ComponentModel.BackgroundWorker

    Public alActors As New ArrayList

    Private loadType As Integer = 0
    Private aniType As Integer = 0 '0 = down, 1 = mid, 2 = up
    Private aniRaise As Boolean = False
    Private MainPoster As New Images
    Private MainFanart As New Images
    Private pbGenre() As PictureBox = Nothing
    Private pnlGenre() As Panel = Nothing
    Private firsttime As Boolean = False
    Private tmpTitle As String = String.Empty
    Private ReportDownloadPercent As Boolean = False
    Private IMDB As New IMDB.Scraper
    Private dtMedia As New DataTable
    Private currRow As Integer = -1
    Private prevRow As Integer = -1
    Private currText As String = String.Empty
    Private prevText As String = String.Empty

    Private Enum PicType As Integer
        Actor = 0
        Poster = 1
        Fanart = 2
    End Enum

    Private Structure Results
        Dim fileInfo As String
        Dim setEnabled As Boolean
        Dim ResultType As PicType
        Dim Result As Image
    End Structure

    Private Structure Arguments
        Dim setEnabled As Boolean
        Dim scrapeType As Master.ScrapeType
        Dim pType As PicType
        Dim pURL As String
    End Structure
#End Region '*** Declarations



#Region "Form/Controls"

    ' ########################################
    ' ######### FORM/CONTROLS EVENTS #########
    ' ########################################

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '//
        ' Do some events before closing
        '\\

        'save the list of movies to settings so we know which ones are new

        If Not Me.bwPrelim.IsBusy AndAlso Not Me.bwFolderData.IsBusy Then
            Master.eSettings.MovieList.Clear()
            For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
                If drvRow.Cells(1).Style.ForeColor = Color.Crimson Then
                    Master.eSettings.MovieList.Add(String.Concat(drvRow.Cells(1).Value.ToString, "=Mark"))
                Else
                    Master.eSettings.MovieList.Add(drvRow.Cells(1).Value.ToString)
                End If
            Next
        End If
        Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision)
        Master.eSettings.Save()

        If Me.bwFolderData.IsBusy Then Me.bwFolderData.CancelAsync()
        If Me.bwMediaInfo.IsBusy Then Me.bwMediaInfo.CancelAsync()
        If Me.bwLoadInfo.IsBusy Then Me.bwLoadInfo.CancelAsync()
        If Me.bwDownloadPic.IsBusy Then Me.bwDownloadPic.CancelAsync()
        If Me.bwPrelim.IsBusy Then Me.bwPrelim.CancelAsync()
        If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()

        Do While Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy
            Application.DoEvents()
        Loop
    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            If Me.dgvMediaList.Columns.Count > 0 Then
                Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()
                Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width / 2) - (Me.pbFanart.Width / 2)
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width / 2) - (Me.pnlNoInfo.Width / 2), (Me.scMain.Panel2.Height / 2) - (Me.pnlNoInfo.Height / 2))
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click

        '//
        ' Show the settings
        '\\

        Try
            Dim dSettings As New dlgSettings

            If dSettings.ShowDialog() = Windows.Forms.DialogResult.OK Then

                Me.SetColors()
                Me.SetCleanFolders()

                If Me.dgvMediaList.RowCount > 0 Then
                    Me.dgvMediaList.Columns(2).Visible = Not Master.eSettings.MoviePosterCol
                    Me.dgvMediaList.Columns(3).Visible = Not Master.eSettings.MovieFanartCol
                    Me.dgvMediaList.Columns(4).Visible = Not Master.eSettings.MovieInfoCol
                    Me.dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieTrailerCol

                    'Trick to autosize the first column, but still allow resizing by user
                    Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                End If
            End If

            dSettings.Dispose()

            If Not String.IsNullOrEmpty(Master.eSettings.XBMCIP) AndAlso Not String.IsNullOrEmpty(Master.eSettings.XBMCPort) Then
                Me.tsbUpdateXBMC.Enabled = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '//
        ' Add our handlers, load settings, set form colors, and try to load movies at startup
        '\\

        Try
            'setup some dummies so we don't get exceptions when resizing form/info panel
            ReDim Preserve Me.pnlGenre(0)
            ReDim Preserve Me.pbGenre(0)
            Me.pnlGenre(0) = New Panel()
            Me.pbGenre(0) = New PictureBox()

            AddHandler IMDB.MovieInfoDownloaded, AddressOf MovieInfoDownloaded
            AddHandler IMDB.ProgressUpdated, AddressOf MovieInfoDownloadedPercent

            Me.Activate()

            Dim sPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Log", Path.DirectorySeparatorChar, "errlog.txt")
            If File.Exists(sPath) Then
                Master.MoveFileWithStream(sPath, sPath.Insert(sPath.LastIndexOf("."), "-old"))
                File.Delete(sPath)
            End If

            Master.eSettings.Load()

            Me.SetColors()
            Me.SetCleanFolders()

            If Not String.IsNullOrEmpty(Master.eSettings.XBMCIP) AndAlso Not String.IsNullOrEmpty(Master.eSettings.XBMCPort) Then
                Me.tsbUpdateXBMC.Enabled = True
            Else
                Me.tsbUpdateXBMC.Enabled = False
            End If

            Me.pnlInfoPanel.Height = 25
            Me.ClearInfo()

            If Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision) Then
                Me.LoadMedia(1)
            Else
                If dlgWizard.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Me.LoadMedia(1)
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub lstActors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstActors.SelectedIndexChanged

        '//
        ' Begin thread to download actor image if one exists
        '\\
        Try
            If Not Me.alActors.Item(Me.lstActors.SelectedIndex) = "none" Then

                If Not IsNothing(Me.pbActors.Image) Then
                    Me.pbActors.Image.Dispose()
                    Me.pbActors.Image = Nothing
                End If

                Me.pbActLoad.Visible = True

                If Me.bwDownloadPic.IsBusy Then
                    Me.bwDownloadPic.CancelAsync()
                End If


                Me.bwDownloadPic = New System.ComponentModel.BackgroundWorker
                Me.bwDownloadPic.WorkerSupportsCancellation = True
                Me.bwDownloadPic.RunWorkerAsync(New Arguments With {.pType = PicType.Actor, .pURL = Me.alActors.Item(Me.lstActors.SelectedIndex)})

            Else
                Me.pbActors.Image = My.Resources.actor_silhouette
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub tabsMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabsMain.SelectedIndexChanged

        '//
        ' Automatically scan media when user selects tab
        '
        ' ### - Make optional?
        '\\

        Try
            Select Case Me.tabsMain.SelectedIndex
                Case 1 'shows
                    ' Me.LoadMedia(2)
                Case 2 'music
                    ' Me.LoadMedia(3)
                Case Else 'movies
                    Me.LoadMedia(1)
            End Select
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub tsbRefreshMedia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRefreshMedia.Click

        '//
        ' Reload media type when "Rescan Media" is clicked
        '\\

        Try
            Select Case Me.tabsMain.SelectedIndex
                Case 1 'shows
                    ' Me.LoadMedia(2)
                Case 2 'music
                    ' Me.LoadMedia(3)
                Case Else 'movies
                    Me.LoadMedia(1)
            End Select
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click

        '//
        ' Begin animation to raise panel all the way up
        '\\

        Me.aniType = 2
        Me.aniRaise = True
        Me.tmrAni.Start()

    End Sub


    Private Sub btnMid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMid.Click

        '//
        ' Begin animation to raise/lower panel to mid point
        '\\

        If Me.pnlInfoPanel.Height = 500 Then
            Me.aniRaise = False
        Else
            Me.aniRaise = True
        End If
        Me.aniType = 1
        Me.tmrAni.Start()

    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click

        '//
        ' Begin animation to lower panel all the way down
        '\\

        Me.aniType = 0
        Me.aniRaise = False
        Me.tmrAni.Start()

    End Sub

    Private Sub tmrAni_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrAni.Tick

        '//
        ' Just some crappy animation to make the GUI slightly more interesting
        '\\

        Try
            If Me.aniRaise Then
                Me.pnlInfoPanel.Height += 5
            Else
                Me.pnlInfoPanel.Height -= 5
            End If

            Me.MoveGenres()
            Me.MoveMPAA()

            If Me.aniType = 0 Then
                If Me.pnlInfoPanel.Height = 25 Then
                    Me.tmrAni.Stop()
                    Me.btnDown.Enabled = False
                    Me.btnMid.Enabled = True
                    Me.btnUp.Enabled = True
                End If
            ElseIf Me.aniType = 1 Then
                If Me.pnlInfoPanel.Height = 280 Then
                    Me.tmrAni.Stop()
                    Me.btnMid.Enabled = False
                    Me.btnDown.Enabled = True
                    Me.btnUp.Enabled = True
                End If
            ElseIf Me.aniType = 2 Then
                If Me.pnlInfoPanel.Height = 500 Then
                    Me.tmrAni.Stop()
                    Me.btnUp.Enabled = False
                    Me.btnDown.Enabled = True
                    Me.btnMid.Enabled = True
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnMIRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMIRefresh.Click

        '//
        ' Refresh Media Info
        '\\

        Try
            Me.LoadInfo(Master.currPath, False, True, Master.isFile, True)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
    Private Sub dgvMediaList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellDoubleClick

        '//
        ' Show the NFO Editor
        '\\

        Try

            Master.currMark = If(Me.dgvMediaList.SelectedRows(0).Cells(1).Style.ForeColor = Color.Crimson, True, False)

            'set tmpTitle to title in list - used for searching IMDB
            Me.tmpTitle = Me.dgvMediaList.Item(1, Me.dgvMediaList.SelectedRows(0).Index).Value.ToString
            Master.currPath = Me.dgvMediaList.Item(0, Me.dgvMediaList.SelectedRows(0).Index).Value.ToString
            Master.isFile = Me.dgvMediaList.Item(6, Me.dgvMediaList.SelectedRows(0).Index).Value.ToString
            Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
            Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)
            Me.tslStatus.Text = Master.currPath

            Dim dEditMovie As New dlgEditMovie
            If dEditMovie.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If Master.currMark Then
                    Me.dgvMediaList.SelectedRows(0).Cells(1).Style.ForeColor = Color.Crimson
                    Me.dgvMediaList.SelectedRows(0).Cells(1).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                Else
                    Me.dgvMediaList.SelectedRows(0).Cells(1).Style.ForeColor = Color.Black
                    Me.dgvMediaList.SelectedRows(0).Cells(1).Style.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
                End If
                Me.ReCheckItems(Me.dgvMediaList.SelectedRows(0).Index)
                Me.LoadInfo(Master.currPath, True, False, Master.isFile)
            End If
            dEditMovie.Dispose()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMediaList.Sorted

        '//
        ' Select first item in the media list after sort
        '\\

        If Me.dgvMediaList.Rows.Count > 0 Then
            Me.dgvMediaList.Rows(0).Selected = True
            Me.dgvMediaList.Rows(0).Visible = True
            Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Rows(0).Cells(1)

            'set tmpTitle to title in list - used for searching IMDB
            Me.tmpTitle = Me.dgvMediaList.Item(1, 0).Value.ToString

        End If

    End Sub

    Private Sub pbGenre_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs)

        '//
        ' Draw genre text over the image when mouse hovers
        '\\

        Try
            Dim iLeft As Integer = 0
            Dim myBitMap As Image = sender.image
            Dim myGR As Graphics = Graphics.FromImage(myBitMap)
            ' Create string to draw.
            Dim drawString As String = sender.Name
            ' Create font and brush.
            Dim drawFont As New Font("Courier New", 20 - drawString.Length, FontStyle.Bold)
            Dim drawBrush As New SolidBrush(Color.White)

            ' Create point for psuedo-centered text
            Select Case drawString.Length
                Case Is >= 12
                    iLeft = 0
                Case Else
                    iLeft = 11 - drawString.Length
            End Select

            Dim drawPoint As New Point(iLeft, 85)
            ' Draw string to screen.
            myGR.DrawString(drawString, drawFont, drawBrush, drawPoint)
            sender.Image = myBitMap
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub pbGenre_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs)

        '//
        ' Reset genre image when mouse leaves to "clear" the text
        '\\

        Try
            sender.image = Master.GetGenreImage(Trim(sender.name))
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click

        '//
        ' Get me out of here!
        '\\

        Application.Exit()

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click

        '//
        ' Give credit where credit is due
        '\\

        dlgAbout.ShowDialog()

    End Sub


    Private Sub btnPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlay.Click

        '//
        ' Launch video using system default player
        '\\

        Try
            If Not String.IsNullOrEmpty(Me.txtFilePath.Text) Then
                If File.Exists(Me.txtFilePath.Text) Then
                    System.Diagnostics.Process.Start("""" & Me.txtFilePath.Text & """")
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub scMain_SplitterMoved(ByVal sender As System.Object, ByVal e As System.Windows.Forms.SplitterEventArgs) Handles scMain.SplitterMoved

        '//
        ' Some generic resizing when pane sizes are changed
        '\\

        Try
            If Me.dgvMediaList.Columns.Count > 0 Then
                Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()

                Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width / 2) - (Me.pbFanart.Width / 2)
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width / 2) - (Me.pnlNoInfo.Width / 2), (Me.scMain.Panel2.Height / 2) - (Me.pnlNoInfo.Height / 2))
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbPoster_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbPoster.DoubleClick

        '//
        ' Show the Poster in the Image Viewer
        '\\

        Try
            If Not IsNothing(Me.pbPoster.Image) Then
                dlgImgView.ShowDialog(Me.pbPosterCache.Image)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub


    Private Sub pbFanart_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbFanart.DoubleClick

        '//
        ' Show the Fanart in the Image Viewer
        '\\

        Try
            If Not IsNothing(Me.pbFanartCache.Image) Then
                dlgImgView.ShowDialog(Me.pbFanartCache.Image)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub tsbEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbEdit.Click

        '//
        ' Show the NFO Editor
        '\\

        Try
            Dim dEditMovie As New dlgEditMovie
            Select dEditMovie.ShowDialog()
                Case Windows.Forms.DialogResult.OK
                    If Master.currMark Then
                        Me.dgvMediaList.SelectedRows(0).Cells(1).Style.ForeColor = Color.Crimson
                        Me.dgvMediaList.SelectedRows(0).Cells(1).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    Else
                        Me.dgvMediaList.SelectedRows(0).Cells(1).Style.ForeColor = Color.Black
                        Me.dgvMediaList.SelectedRows(0).Cells(1).Style.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
                    End If
                    Me.ReCheckItems(Me.dgvMediaList.SelectedRows(0).Index)
                    Me.LoadInfo(Master.currPath, True, False, Master.isFile)
                Case Windows.Forms.DialogResult.Retry
                    Me.ScrapeData(Master.ScrapeType.SingleScrape)
            End Select
            dEditMovie.Dispose()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub mnuRescrapeAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRescrapeAuto.Click

        '//
        ' Begin the process to scrape IMDB for data
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape)
    End Sub

    Private Sub mnuRescrapeSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRescrapeSearch.Click

        '//
        ' Begin the process to search IMDB for data
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape, True)

    End Sub

    Private Sub dgvMediaList_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvMediaList.CellPainting

        '//
        ' Add icons to media list column headers
        '\\

        Try
            If e.ColumnIndex >= 2 AndAlso e.RowIndex = -1 Then
                e.PaintBackground(e.ClipBounds, False)

                Dim pt As Point = e.CellBounds.Location
                Dim offset As Integer = (e.CellBounds.Width - Me.ilColumnIcons.ImageSize.Width) / 2

                pt.X += offset
                pt.Y = 1
                Me.ilColumnIcons.Draw(e.Graphics, pt, e.ColumnIndex - 2)

                e.Handled = True

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FullAutoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FullAutoToolStripMenuItem.Click

        '//
        ' Scrape all movies in list and attempt to pick the best match for movies without exact matches
        '\\

        Me.ScrapeData(Master.ScrapeType.FullAuto)

    End Sub

    Private Sub FullAskToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FullAskToolStripMenuItem.Click

        '//
        ' Scrape all movies in list and ask which movie to pick when a best match or popular match is not found
        '\\

        Me.ScrapeData(Master.ScrapeType.FullAsk)

    End Sub

    Private Sub MediaTagsOnlyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MediaTagsOnlyToolStripMenuItem.Click

        '//
        ' Scrape all movies in list for MediaInfo only
        '\\

        Me.ScrapeData(Master.ScrapeType.MIOnly)

    End Sub

    Private Sub UpdateAutoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateAutoToolStripMenuItem.Click

        '//
        ' Scrape only movies in list without nfos and attempt to pick the best match for movies without exact matches
        '\\

        Me.ScrapeData(Master.ScrapeType.UpdateAuto)

    End Sub

    Private Sub UpdateAskToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateAskToolStripMenuItem.Click

        '//
        ' Scrape only movies in list without nfos and ask which movie to pick when a best match or popular match is not found
        '\\

        Me.ScrapeData(Master.ScrapeType.UpdateAsk)

    End Sub

    Private Sub dgvMediaList_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellEnter
        '//
        ' Load media information for the selected item
        '\\

        Try
            Me.currRow = e.RowIndex

            Me.tmrWait.Enabled = False
            Me.tmrWait.Enabled = True

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub tmrWait_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrWait.Tick
        If Me.prevRow = Me.currRow Then
            Me.tmrLoad.Enabled = True
        Else
            Me.prevRow = Me.currRow
        End If
    End Sub

    Private Sub tmrLoad_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLoad.Tick
        Me.tmrWait.Enabled = False
        Try
            If Me.dgvMediaList.SelectedRows.Count > 0 Then
                Master.currMark = If(Me.dgvMediaList.SelectedRows(0).Cells(1).Style.ForeColor = Color.Crimson, True, False)

                'set tmpTitle to title in list - used for searching IMDB
                Me.tmpTitle = Me.dgvMediaList.Item(1, currRow).Value.ToString
                If Not Me.dgvMediaList.Item(2, currRow).Value AndAlso Not Me.dgvMediaList.Item(3, currRow).Value AndAlso Not Me.dgvMediaList.Item(4, currRow).Value Then
                    Me.ClearInfo()
                    Me.pnlNoInfo.Visible = True
                    Master.currPath = Me.dgvMediaList.Item(0, currRow).Value.ToString
                    Master.isFile = Me.dgvMediaList.Item(6, currRow).Value.ToString
                    Master.currMovie = New Media.Movie
                    Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
                    Me.tslStatus.Text = Master.currPath
                Else
                    Me.pnlNoInfo.Visible = False
                    'try to load the info from the NFO
                    Me.LoadInfo(Me.dgvMediaList.Item(0, currRow).Value.ToString, True, False, Me.dgvMediaList.Item(6, currRow).Value)
                End If
            End If
        Catch
        End Try
        Me.tmrLoad.Enabled = False
    End Sub


    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        Me.currText = Me.txtSearch.Text

        Me.tmrSearchWait.Enabled = False
        Me.tmrSearchWait.Enabled = True
    End Sub

    Private Sub tmrSearchWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSearchWait.Tick
        If Me.prevText = Me.currText Then
            Me.tmrSearch.Enabled = True
        Else
            Me.prevText = Me.currText
        End If
    End Sub

    Private Sub tmrSearch_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrSearch.Tick
        Me.tmrSearchWait.Enabled = False
        Try
            If Not String.IsNullOrEmpty(txtSearch.Text) Then
                Dim dvFilter As DataView = dtMedia.DefaultView
                dvFilter.RowFilter = "Name Like '%" & txtSearch.Text & "%'"
                dgvMediaList.DataSource = dvFilter
            Else
                Dim dvFilter As DataView = dtMedia.DefaultView
                dvFilter.RowFilter = String.Empty
                dgvMediaList.DataSource = dvFilter
            End If
        Catch
        End Try
        Me.tmrSearch.Enabled = False
    End Sub

    Private Sub tsbUpdateXBMC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUpdateXBMC.Click
        Try
            If Not String.IsNullOrEmpty(Master.eSettings.XBMCIP) AndAlso Not String.IsNullOrEmpty(Master.eSettings.XBMCPort) Then
                Dim Wr As HttpWebRequest = HttpWebRequest.Create(String.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?command=ExecBuiltIn&parameter=XBMC.updatelibrary(video)", Master.eSettings.XBMCIP, Master.eSettings.XBMCPort))
                Wr.Method = "GET"
                Wr.Timeout = 2500
                If Not String.IsNullOrEmpty(Master.eSettings.XBMCUsername) AndAlso Not String.IsNullOrEmpty(Master.eSettings.XBMCPassword) Then
                    Wr.Credentials = New NetworkCredential(Master.eSettings.XBMCUsername, Master.eSettings.XBMCPassword)
                End If
                Dim Wres As HttpWebResponse = Wr.GetResponse
                Dim Sr As String = New StreamReader(Wres.GetResponseStream()).ReadToEnd
                If Not Sr.Contains("OK") Then
                    MsgBox("There was a problem communicating with XBMC" & vbNewLine & "Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings.", MsgBoxStyle.Exclamation, "Unable to Start XBMC Update")
                End If
                Wres.Close()
                Wres = Nothing
                Wr = Nothing
            End If
        Catch
        End Try
    End Sub

    Private Sub CleanFoldersToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CleanFoldersToolStripMenuItem.Click
        '//
        ' Clean all items in folders that match user selected types
        '\\

        Me.ScrapeData(Master.ScrapeType.CleanFolders)
    End Sub

    Private Sub ConvertFileSourceToFolderSourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertFileSourceToFolderSourceToolStripMenuItem.Click
        Dim dirArray() As String
        Dim alMedia As New ArrayList
        Dim hasFileSource As Boolean = False

        Me.tsbAutoPilot.Enabled = False
        Me.tsbRefreshMedia.Enabled = False
        Me.tsbEdit.Enabled = False
        Me.tsbRescrape.Enabled = False
        Me.tabsMain.Enabled = False
        Me.tspbLoading.Style = ProgressBarStyle.Marquee
        Me.tslLoading.Text = "Sorting Files:"
        Me.tslLoading.Visible = True
        Me.tspbLoading.Visible = True
        Application.DoEvents()

        Select Case Me.loadType
            Case 2 'shows
            Case 3 'music
            Case Else 'default to movies
                'load all the movie folders from settings
                alMedia = Master.eSettings.MovieFolders
        End Select

        For Each movieSource As String In alMedia
            dirArray = Split(movieSource, "|")
            If dirArray(1).ToString = "Files" Then
                SortFiles(dirArray(0).ToString)
                hasFileSource = True
            End If
        Next

        If hasFileSource Then
            Me.LoadMedia(1)
        Else
            MsgBox("You do not have any file-type sources to sort.", MsgBoxStyle.Information, "No Files To Sort")
        End If
    End Sub
#End Region '*** Form/Controls



#Region "Background Workers"

    ' ########################################
    ' ########## WORKER COMPONENTS ###########
    ' ########################################

    Private Sub bwPrelim_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwPrelim.DoWork

        '//
        ' Thread to count directories to prepare for loading media
        '\\

        Dim dirArray() As String
        Dim alMedia As New ArrayList

        Select Case Me.loadType
            Case 2 'shows
            Case 3 'music
            Case Else 'default to movies
                'load all the movie folders from settings
                alMedia = Master.eSettings.MovieFolders
        End Select

        Try
            'count media type folder/file... simply for the sake of reporting progress
            For Each movieSource As String In alMedia

                dirArray = Split(movieSource, "|")
                If dirArray(1).ToString = "Folders" Then
                    Master.EnumerateDirectory(dirArray(0).ToString)
                Else
                    Master.EnumerateFiles(dirArray(0).ToString)
                End If

                If Me.bwPrelim.CancellationPending Then Return
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        dirArray = Nothing
        alMedia = Nothing

    End Sub

    Private Sub bwPrelim_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwPrelim.RunWorkerCompleted

        '//
        ' Thread finished: set up progress bar, display count, and begin thread to load data
        '\\

        Try
            If Master.alFolderList.Count = 0 AndAlso Master.alFileList.Count = 0 Then
                Me.tslStatus.Text = "Unable to load directories. Please check settings."
                Me.tspbLoading.Visible = False
                Me.tslLoading.Visible = False
                Me.tabsMain.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                Me.tsbAutoPilot.Enabled = False
                Me.tsbEdit.Enabled = False
                Me.tsbRescrape.Enabled = False

            Else
                Me.tslLoading.Text = "Loading Media:"
                Me.tspbLoading.Style = ProgressBarStyle.Continuous
                Me.tslLoading.Visible = True
                Me.tspbLoading.Visible = True
                Me.tspbLoading.Maximum = (Master.alFolderList.Count + Master.alFileList.Count + 1)


                Me.bwFolderData = New System.ComponentModel.BackgroundWorker
                Me.bwFolderData.WorkerReportsProgress = True
                Me.bwFolderData.WorkerSupportsCancellation = True
                Me.bwFolderData.RunWorkerAsync()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwFolderData_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwFolderData.DoWork

        '//
        ' Thread to fill a datatable with basic media data
        '\\

        Dim currentIndex As Integer = 0
        Dim cleanName As String = String.Empty
        Dim mPath As String = String.Empty
        Dim mName As String = String.Empty
        Dim aContents(4) As Boolean
        Dim tmpMovie As New Media.Movie
        Dim tmpAL As New ArrayList

        Try
            Me.dtMedia.Clear()
            Me.dtMedia.Reset()

            'set up the columns for the temporary datatable
            Me.dtMedia.Columns.Add("Path", GetType(System.String))
            Me.dtMedia.Columns.Add("Name", GetType(System.String))
            Me.dtMedia.Columns.Add("Poster", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Fanart", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Info", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Trailer", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Type", GetType(System.Boolean))

            'process the folder type media
            For Each sName As String In Master.alFolderList
                If Me.bwFolderData.CancellationPending Then Return
                mPath = Master.GetMoviePath(sName)

                If Not String.IsNullOrEmpty(mPath) Then
                    If Master.eSettings.UseNameFromNfo Then
                        tmpMovie = Master.LoadMovieFromNFO(Master.GetNfoPath(mPath, False))
                        mName = tmpMovie.Title
                        tmpMovie = Nothing
                        If String.IsNullOrEmpty(mName) Then
                            If Master.eSettings.UseFolderName Then
                                mName = Master.GetNameFromPath(sName)
                            Else
                                mName = Master.RemoveExtFromFile(mPath)
                            End If
                        End If
                    Else
                        If Master.eSettings.UseFolderName Then
                            mName = Master.GetNameFromPath(sName)
                        Else
                            mName = Master.RemoveExtFromFile(mPath)
                        End If
                    End If

                    cleanName = Master.FilterName(mName)

                    Me.bwFolderData.ReportProgress(currentIndex, cleanName)

                    If Not String.IsNullOrEmpty(cleanName) Then

                        Dim newRow(6) As Object

                        newRow(0) = mPath
                        newRow(1) = cleanName
                        aContents = Master.GetFolderContents(mPath, False)
                        newRow(2) = aContents(0)
                        newRow(3) = aContents(1)
                        newRow(4) = aContents(2)
                        newRow(5) = aContents(3)
                        newRow(6) = False

                        Me.dtMedia.LoadDataRow(newRow, True)

                        aContents = Nothing
                        mName = Nothing
                        newRow = Nothing
                        currentIndex += 1
                    End If
                End If
            Next

            'process the file type media
            For Each sFile As FileInfo In Master.alFileList

                If Me.bwFolderData.CancellationPending Then Return

                If Not tmpAL.Contains(Master.CleanStackingMarkers(sFile.FullName)) Then

                    tmpAL.Add(Master.CleanStackingMarkers(sFile.FullName))

                    'parse just the movie name
                    If Master.eSettings.UseNameFromNfo Then
                        tmpMovie = Master.LoadMovieFromNFO(Master.GetNfoPath(sFile.FullName, True))
                        mName = tmpMovie.Title
                        tmpMovie = Nothing
                        If String.IsNullOrEmpty(mName) Then
                            mName = Master.RemoveExtFromFile(sFile.Name)
                        End If
                    Else
                        mName = Master.RemoveExtFromFile(sFile.Name)
                    End If

                    cleanName = Master.FilterName(mName)

                    Me.bwFolderData.ReportProgress(currentIndex, cleanName)

                    If Not String.IsNullOrEmpty(cleanName) Then

                        Dim newFileRow(6) As Object

                        newFileRow(0) = sFile.FullName
                        newFileRow(1) = cleanName
                        'check what's in the folder
                        aContents = Master.GetFolderContents(sFile.FullName, True)
                        newFileRow(2) = aContents(0)
                        newFileRow(3) = aContents(1)
                        newFileRow(4) = aContents(2)
                        newFileRow(5) = aContents(3)
                        newFileRow(6) = True

                        Me.dtMedia.LoadDataRow(newFileRow, True)

                        aContents = Nothing
                        mName = Nothing
                        newFileRow = Nothing
                        currentIndex += 1
                    End If
                End If
            Next

            tmpAL = Nothing
            tmpMovie = Nothing
            aContents = Nothing
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwFolderData_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwFolderData.ProgressChanged

        '//
        ' Update the status bar with the name of the current media name and increase progress bar
        '\\

        Me.tspbLoading.Value = e.ProgressPercentage
        Me.tslStatus.Text = e.UserState.ToString

    End Sub

    Private Sub bwFolderData_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwFolderData.RunWorkerCompleted

        '//
        ' Thread finished: fill datagrid with info and configure columns
        '\\


        Try
            If Me.dtMedia.Rows.Count > 0 Then

                Me.dtMedia.DefaultView.Sort = "Name ASC"
                With Me
                    'setup up the media list with the results from the media scan
                    .dgvMediaList.DataSource = .dtMedia.DefaultView

                    'why did the resizable property all the sudden become opposite? resizable = false now means it IS resizable
                    'wasn't like that before and was reported (after release of v alpha 022, but no telling how long it's been
                    'like that) that the info columns were resizable
                    .dgvMediaList.Columns(0).Visible = False
                    .dgvMediaList.Columns(1).Resizable = False
                    .dgvMediaList.Columns(1).ReadOnly = True
                    .dgvMediaList.Columns(1).MinimumWidth = 83
                    .dgvMediaList.Columns(1).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(2).Width = 20
                    .dgvMediaList.Columns(2).Resizable = True
                    .dgvMediaList.Columns(2).ReadOnly = True
                    .dgvMediaList.Columns(2).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(2).Visible = Not Master.eSettings.MoviePosterCol
                    .dgvMediaList.Columns(3).Width = 20
                    .dgvMediaList.Columns(3).Resizable = True
                    .dgvMediaList.Columns(3).ReadOnly = True
                    .dgvMediaList.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(3).Visible = Not Master.eSettings.MovieFanartCol
                    .dgvMediaList.Columns(4).Width = 20
                    .dgvMediaList.Columns(4).Resizable = True
                    .dgvMediaList.Columns(4).ReadOnly = True
                    .dgvMediaList.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(4).Visible = Not Master.eSettings.MovieInfoCol
                    .dgvMediaList.Columns(5).Width = 20
                    .dgvMediaList.Columns(5).Resizable = True
                    .dgvMediaList.Columns(5).ReadOnly = True
                    .dgvMediaList.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieTrailerCol
                    .dgvMediaList.Columns(6).Visible = False

                    'Trick to autosize the first column, but still allow resizing by user
                    .dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                    'Trick to work around the blank table bug in the DGV
                    .dgvMediaList.Sort(.dgvMediaList.Columns(1), ComponentModel.ListSortDirection.Descending)
                    .dgvMediaList.Sort(.dgvMediaList.Columns(1), ComponentModel.ListSortDirection.Ascending)

                    For Each drvRow As DataGridViewRow In .dgvMediaList.Rows
                        If Master.eSettings.MovieList.Contains(String.Concat(drvRow.Cells(1).Value.ToString, "=Mark")) Then
                            drvRow.Cells(1).Style.ForeColor = Color.Crimson
                            drvRow.Cells(1).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                        ElseIf Not Master.eSettings.MovieList.Contains(drvRow.Cells(1).Value.ToString) Then
                            If Master.eSettings.MarkNew Then
                                drvRow.Cells(1).Style.ForeColor = Color.Crimson
                            Else
                                drvRow.Cells(1).Style.ForeColor = Color.Green
                            End If
                            drvRow.Cells(1).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                        End If
                    Next

                    'Set current cell and automatically load the info for the first movie in the list
                    .tmpTitle = .dgvMediaList.Item(1, 0).Value.ToString
                    .dgvMediaList.CurrentCell = .dgvMediaList.Rows(0).Cells(1)
                    .btnUp.Enabled = True
                    .btnMid.Enabled = True

                    .tsbAutoPilot.Enabled = True
                    .tsbEdit.Enabled = True
                    .tsbRescrape.Enabled = True
                End With
            Else
                Me.tsbAutoPilot.Enabled = False
                Me.tsbEdit.Enabled = False
                Me.tsbRescrape.Enabled = False
                Me.tslStatus.Text = String.Empty
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.tslLoading.Visible = False
        Me.tspbLoading.Visible = False
        Me.tspbLoading.Value = 0

        Me.lblMediaCount.Text = String.Format("Media Count: {0}", Me.dgvMediaList.Rows.Count)
        Me.lblMediaCount.Visible = True
        Me.txtSearch.Text = String.Empty

        Me.loadType = 0
    End Sub

    Private Sub bwMediaInfo_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwMediaInfo.DoWork

        '//
        ' Thread to procure technical and tag information about media via MediaInfo.dll
        '\\

        Try
            Dim Args As Arguments = e.Argument
            If Me.UpdateMediaInfo() Then
                If Master.eSettings.UseStudioTags = True Then
                    Master.currMovie.Studio = Master.currMovie.StudioReal & Master.FITagData(Master.currMovie.FileInfo)
                End If
                Master.SaveMovieToNFO(Master.currMovie, Master.currPath, Master.isFile)
                e.Result = New Results With {.fileinfo = Master.FIToString(Master.currMovie.FileInfo), .setEnabled = Args.setEnabled}

            Else
                e.Result = New Results With {.fileinfo = "error", .setEnabled = Args.setEnabled}
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwMediaInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwMediaInfo.RunWorkerCompleted

        '//
        ' Thread finished: fill textbox with result
        '\\
        Dim Res As Results = e.Result

        Try
            If Not Res.fileInfo = "error" Then
                Me.pbMILoading.Visible = False
                Me.txtMediaInfo.SelectionLength = 0
                Me.txtMediaInfo.Text = Res.fileInfo
                If Master.eSettings.UseStudioTags = True Then
                    If Not String.IsNullOrEmpty(Master.currMovie.Studio) Then
                        Master.GetAVImages(Master.currMovie.Studio, Master.currPath)
                        Me.pnlInfoIcons.Width = 346
                        Me.pbStudio.Left = 277
                    Else
                        Master.GetAVImages("", Master.currPath)
                    End If
                Else
                    Me.pnlInfoIcons.Width = 70
                    Me.pbStudio.Left = 0
                End If

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If Res.setEnabled Then
            Me.tabsMain.Enabled = True
            Me.tsbRefreshMedia.Enabled = True
            If Me.dgvMediaList.Rows.Count > 0 Then
                Me.tsbAutoPilot.Enabled = True
                Me.tsbRescrape.Enabled = True
                Me.tsbEdit.Enabled = True
            End If
        End If

    End Sub

    Private Sub bwDownloadPic_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadPic.DoWork

        '//
        ' Thread to download image from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\

        Dim Args As Arguments = e.Argument
        Dim wrRequest As WebRequest = WebRequest.Create(Args.pURL)
        wrRequest.Timeout = 5000 'give it 5 seconds
        Try
            Dim wrResponse As WebResponse = wrRequest.GetResponse()
            e.Result = New Results With {.ResultType = Args.pType, .Result = Image.FromStream(wrResponse.GetResponseStream())}
        Catch
            e.Result = New Results With {.ResultType = Args.pType, .Result = Nothing}
        End Try

    End Sub

    Private Sub bwDownloadPic_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownloadPic.RunWorkerCompleted

        '//
        ' Thread finished: display pic if it was able to get one
        '\\

        Dim Res As Results = e.Result

        Select Case Res.ResultType
            Case PicType.Actor
                Me.pbActLoad.Visible = False
                Me.pbActors.Image = Res.Result
            Case Else
        End Select

    End Sub

    Private Sub bwLoadInfo_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadInfo.DoWork

        '//
        ' Thread to load media images and information (from nfo) then display on screen
        '\\

        Try

            Me.MainFanart.Clear()

            Me.MainPoster.Clear()

            Me.MainFanart.Load(Master.currPath, Master.isFile, Master.ImageType.Fanart)

            Me.MainPoster.Load(Master.currPath, Master.isFile, Master.ImageType.Posters)

            'wait for mediainfo to update the nfo
            Do While bwMediaInfo.IsBusy
                Application.DoEvents()
            Loop

            'read nfo if it's there
            Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
            If Not String.IsNullOrEmpty(Master.currNFO) Then
                Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try


    End Sub

    Private Sub bwLoadInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadInfo.RunWorkerCompleted

        '//
        ' Thread finished: display it
        '\\

        Try
            Me.fillScreenInfo()

            If Not IsNothing(Me.MainFanart.Image) Then
                Me.pbFanartCache.Image = Me.MainFanart.Image
            Else
                If Not IsNothing(Me.pbFanartCache.Image) Then
                    Me.pbFanartCache.Image.Dispose()
                    Me.pbFanartCache.Image = Nothing
                End If
                If Not IsNothing(Me.pbFanart.Image) Then
                    Me.pbFanart.Image.Dispose()
                    Me.pbFanart.Image = Nothing
                End If
            End If

            If Not IsNothing(Me.MainPoster.Image) Then
                Me.pbPosterCache.Image = Me.MainPoster.Image
                Master.ResizePB(Me.pbPoster, Me.pbPosterCache, 160, 160)
                Master.SetOverlay(Me.pbPoster)
                Me.pnlPoster.Size = New Size(Me.pbPoster.Width + 10, Me.pbPoster.Height + 10)
                Me.pbPoster.Location = New Point(4, 4)
                Me.pnlPoster.Visible = True
            Else
                If Not IsNothing(Me.pbPoster.Image) Then
                    Me.pbPoster.Image.Dispose()
                    Me.pbPoster.Image = Nothing
                    Me.pnlPoster.Visible = False
                End If
            End If

            Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
            Me.pbFanart.Left = (Me.scMain.Panel2.Width / 2) - (Me.pbFanart.Width / 2)

            If Not bwScraper.IsBusy Then
                Me.tsbAutoPilot.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                Me.tsbRescrape.Enabled = True
                Me.tsbEdit.Enabled = True
                Me.tabsMain.Enabled = True
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwScraper_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwScraper.DoWork

        '//
        ' Thread to handle scraping
        '\\


        Dim Args As Arguments = e.Argument
        Dim TMDB As New TMDB.Scraper
        Dim IMPA As New IMPA.Scraper
        Dim dvView As DataView = Me.dtMedia.DefaultView
        Dim iCount As Integer = 0
        Dim sPath As String = String.Empty
        Dim sPathShort As String = String.Empty
        Dim sPathNoExt As String = String.Empty
        Dim sOrName As String = String.Empty
        Dim nfoPath As String = String.Empty
        Dim fArt As New Media.Fanart
        Dim Poster As New Images
        Dim Fanart As New Images
        Dim tmpMovie As New Media.Movie

        Try
            If Me.dtMedia.Rows.Count > 0 Then
                Select Case Args.scrapeType
                    Case Master.ScrapeType.FullAsk
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            sPath = drvRow.Item(0).ToString

                            nfoPath = Master.GetNfoPath(sPath, drvRow.Item(6))

                            If File.Exists(nfoPath) Then
                                tmpMovie = Master.LoadMovieFromNFO(nfoPath)
                                If Not String.IsNullOrEmpty(tmpMovie.IMDBID) AndAlso IMDB.GetMovieInfo(tmpMovie.IMDBID, tmpMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False) Then
                                    Master.currMovie = tmpMovie
                                Else
                                    Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString, tmpMovie, Args.scrapeType)
                                End If
                                tmpMovie = Nothing
                            Else
                                Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString, New Media.Movie, Args.scrapeType)
                            End If

                            If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                If Me.bwScraper.CancellationPending Then Return
                                If Master.eSettings.UseStudioTags Then
                                    If UpdateMediaInfo() Then
                                        Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB Then

                                    If Poster.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Posters) Then
                                        Poster.GetPreferredImage(Master.ImageType.Posters, Nothing, True)
                                        If Not IsNothing(Poster.Image) Then
                                            Poster.SaveAsPoster(sPath, drvRow.Item(6))
                                            drvRow.Item(2) = True
                                        Else
                                            MsgBox("A poster of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                            Dim dImgSelect As New dlgImgSelect
                                            If dImgSelect.ShowDialog(Master.currMovie.IMDBID, sPath, Master.ImageType.Posters) = Windows.Forms.DialogResult.OK Then
                                                drvRow.Item(2) = True
                                            End If
                                            dImgSelect = Nothing
                                        End If
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.eSettings.UseTMDB Then

                                    If Fanart.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Fanart) Then
                                        Fanart.GetPreferredImage(Master.ImageType.Fanart, fArt, True)

                                        If Not IsNothing(Fanart.Image) Then
                                            Fanart.SaveAsFanart(sPath, drvRow.Item(6))
                                            drvRow.Item(3) = True
                                            Master.currMovie.Fanart = fArt
                                        Else
                                            MsgBox("Fanart of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")

                                            Dim dImgSelect As New dlgImgSelect
                                            If dImgSelect.ShowDialog(Master.currMovie.IMDBID, sPath, Master.ImageType.Fanart) = Windows.Forms.DialogResult.OK Then
                                                drvRow.Item(3) = True
                                                Master.currMovie.Fanart = fArt
                                            End If
                                            dImgSelect.Dispose()
                                        End If
                                        fArt = Nothing
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                Master.SaveMovieToNFO(Master.currMovie, sPath, drvRow.Item(6))
                                drvRow.Item(4) = True
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.AutoThumbs > 0 Then
                                Me.CreateRandomThumbs(sPath)
                            End If
                        Next

                    Case Master.ScrapeType.FullAuto
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            sPath = drvRow.Item(0).ToString

                            nfoPath = Master.GetNfoPath(sPath, drvRow.Item(6))

                            If File.Exists(nfoPath) Then
                                tmpMovie = Master.LoadMovieFromNFO(nfoPath)
                                If Not String.IsNullOrEmpty(tmpMovie.IMDBID) AndAlso IMDB.GetMovieInfo(tmpMovie.IMDBID, tmpMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False) Then
                                    Master.currMovie = tmpMovie
                                Else
                                    Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString, tmpMovie, Args.scrapeType)
                                End If
                                tmpMovie = Nothing
                            Else
                                Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString(), New Media.Movie, Args.scrapeType)
                            End If

                            If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                If Me.bwScraper.CancellationPending Then Return
                                If Master.eSettings.UseStudioTags Then
                                    If UpdateMediaInfo() Then
                                        Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB Then

                                    If Poster.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Posters) Then
                                        Poster.GetPreferredImage(Master.ImageType.Posters, Nothing)
                                        If Not IsNothing(Poster.Image) Then
                                            Poster.SaveAsPoster(sPath, drvRow.Item(6))
                                            drvRow.Item(2) = True
                                        End If
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.eSettings.UseTMDB Then
                                    If Fanart.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Fanart) Then
                                        Fanart.GetPreferredImage(Master.ImageType.Fanart, fArt)
                                        If Not IsNothing(Fanart.Image) Then
                                            Fanart.SaveAsFanart(sPath, drvRow.Item(6))
                                            Master.currMovie.Fanart = fArt
                                            drvRow.Item(3) = True
                                        End If
                                        fArt = Nothing
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                Master.SaveMovieToNFO(Master.currMovie, sPath, drvRow.Item(6))
                                drvRow.Item(4) = True
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.AutoThumbs > 0 Then
                                Me.CreateRandomThumbs(sPath)
                            End If
                        Next
                    Case Master.ScrapeType.MIOnly
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            Master.currPath = drvRow.Item(0).ToString

                            nfoPath = Master.GetNfoPath(Master.currPath, drvRow.Item(6))

                            If Not String.IsNullOrEmpty(nfoPath) Then
                                Master.currMovie = Master.LoadMovieFromNFO(nfoPath)
                            Else
                                Master.currMovie = New Media.Movie
                            End If

                            If UpdateMediaInfo() Then
                                Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            Master.SaveMovieToNFO(Master.currMovie, sPath, drvRow.Item(6))
                        Next

                    Case Master.ScrapeType.CleanFolders
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            sPath = drvRow.Item(0).ToString
                            sOrName = Master.CleanStackingMarkers(Master.RemoveExtFromFile(Master.GetNameFromPath(sPath)))
                            sPathShort = Directory.GetParent(sPath).FullName
                            sPathNoExt = Master.RemoveExtFromPath(sPath)

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanFolderJPG Then
                                If File.Exists(Path.Combine(sPathShort, "folder.jpg")) Then
                                    File.Delete(Path.Combine(sPathShort, "folder.jpg"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanFanartJPG Then
                                If File.Exists(Path.Combine(sPathShort, "fanart.jpg")) Then
                                    File.Delete(Path.Combine(sPathShort, "fanart.jpg"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanMovieTBN Then
                                If File.Exists(Path.Combine(sPathShort, "movie.tbn")) Then
                                    File.Delete(Path.Combine(sPathShort, "movie.tbn"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanMovieNFO Then
                                If File.Exists(Path.Combine(sPathShort, "movie.nfo")) Then
                                    File.Delete(Path.Combine(sPathShort, "movie.nfo"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanPosterTBN Then
                                If File.Exists(Path.Combine(sPathShort, "poster.tbn")) Then
                                    File.Delete(Path.Combine(sPathShort, "poster.tbn"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanPosterJPG Then
                                If File.Exists(Path.Combine(sPathShort, "poster.jpg")) Then
                                    File.Delete(Path.Combine(sPathShort, "poster.jpg"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanMovieJPG Then
                                If File.Exists(Path.Combine(sPathShort, "movie.jpg")) Then
                                    File.Delete(Path.Combine(sPathShort, "movie.jpg"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanMovieTBNB Then
                                If File.Exists(String.Concat(sPathNoExt, ".tbn")) Then
                                    File.Delete(String.Concat(sPathNoExt, ".tbn"))
                                End If
                                If File.Exists(Path.Combine(sPathShort, "video_ts.tbn")) Then
                                    File.Delete(Path.Combine(sPathShort, "video_ts.tbn"))
                                End If
                                If File.Exists(String.Concat(Path.Combine(sPathShort, sOrName), ".tbn")) Then
                                    File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".tbn"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanMovieFanartJPG Then
                                If File.Exists(String.Concat(sPathNoExt, "-fanart.jpg")) Then
                                    File.Delete(String.Concat(sPathNoExt, "-fanart.jpg"))
                                End If
                                If File.Exists(Path.Combine(sPathShort, "video_ts-fanart.jpg")) Then
                                    File.Delete(Path.Combine(sPathShort, "video_ts-fanart.jpg"))
                                End If
                                If File.Exists(String.Concat(Path.Combine(sPathShort, sOrName), "-fanart.jpg")) Then
                                    File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), "-fanart.jpg"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanMovieNFOB Then
                                If File.Exists(String.Concat(sPathNoExt, ".nfo")) Then
                                    File.Delete(String.Concat(sPathNoExt, ".nfo"))
                                End If
                                If File.Exists(Path.Combine(sPathShort, "video_ts.nfo")) Then
                                    File.Delete(Path.Combine(sPathShort, "video_ts.nfo"))
                                End If
                                If File.Exists(String.Concat(Path.Combine(sPathShort, sOrName), ".nfo")) Then
                                    File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".nfo"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanDotFanartJPG Then
                                If File.Exists(String.Concat(sPathNoExt, ".fanart.jpg")) Then
                                    File.Delete(String.Concat(sPathNoExt, ".fanart.jpg"))
                                End If
                                If File.Exists(Path.Combine(sPathShort, "video_ts.fanart.jpg")) Then
                                    File.Delete(Path.Combine(sPathShort, "video_ts.fanart.jpg"))
                                End If
                                If File.Exists(String.Concat(Path.Combine(sPathShort, sOrName), ".fanart.jpg")) Then
                                    File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".fanart.jpg"))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.CleanMovieNameJPG Then
                                If File.Exists(String.Concat(sPathNoExt, ".jpg")) Then
                                    File.Delete(String.Concat(sPathNoExt, ".jpg"))
                                End If
                                If File.Exists(Path.Combine(sPathShort, "video_ts.jpg")) Then
                                    File.Delete(Path.Combine(sPathShort, "video_ts.jpg"))
                                End If
                                If File.Exists(String.Concat(Path.Combine(sPathShort, sOrName), ".jpg")) Then
                                    File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".jpg"))
                                End If
                            End If

                        Next

                    Case Master.ScrapeType.UpdateAuto
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            If Not drvRow.Item(2) OrElse Not drvRow.Item(3) OrElse Not drvRow.Item(4) Then
                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                                iCount += 1
                                sPath = drvRow.Item(0).ToString

                                nfoPath = Master.GetNfoPath(sPath, drvRow.Item(6))
                                Master.currMovie = Master.LoadMovieFromNFO(nfoPath)

                                If Not drvRow.Item(4) Then
                                    If String.IsNullOrEmpty(Master.currMovie.IMDBID) OrElse Not IMDB.GetMovieInfo(Master.currMovie.IMDBID, Master.currMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False) Then
                                        Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString, New Media.Movie, Args.scrapeType)
                                    End If

                                    If Master.eSettings.UseStudioTags Then
                                        If UpdateMediaInfo() Then
                                            Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                                        End If
                                    End If

                                    Master.SaveMovieToNFO(Master.currMovie, sPath, drvRow.Item(6))
                                    drvRow.Item(4) = True
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(2) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB Then

                                        If Poster.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Posters) Then
                                            Poster.GetPreferredImage(Master.ImageType.Posters, Nothing)

                                            If Not IsNothing(Poster.Image) Then
                                                Poster.SaveAsPoster(sPath, drvRow.Item(6))
                                                drvRow.Item(2) = True
                                            End If
                                        End If
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(3) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.eSettings.UseTMDB Then
                                        If Fanart.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Fanart) Then
                                            Fanart.GetPreferredImage(Master.ImageType.Fanart, fArt)

                                            If Not IsNothing(Fanart.Image) Then
                                                Fanart.SaveAsFanart(sPath, drvRow.Item(6))
                                                drvRow.Item(3) = True
                                                If File.Exists(nfoPath) Then
                                                    'need to load movie from nfo here in case the movie already had
                                                    'an nfo.... currmovie would not be set to the proper movie
                                                    Master.currMovie = Master.LoadMovieFromNFO(nfoPath)
                                                    Master.currMovie.Fanart = fArt
                                                    Master.SaveMovieToNFO(Master.currMovie, sPath, drvRow.Item(6))
                                                End If
                                                fArt = Nothing
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.AutoThumbs > 0 AndAlso Not Directory.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "extrathumbs")) Then
                                Me.CreateRandomThumbs(sPath)
                            End If
                        Next
                    Case Master.ScrapeType.UpdateAsk
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            If Not drvRow.Item(2) OrElse Not drvRow.Item(3) OrElse Not drvRow.Item(4) Then
                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(2).ToString)
                                iCount += 1

                                sPath = drvRow.Item(0).ToString

                                nfoPath = Master.GetNfoPath(sPath, drvRow.Item(6))
                                Master.currMovie = Master.LoadMovieFromNFO(nfoPath)

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(4) Then
                                    If String.IsNullOrEmpty(Master.currMovie.IMDBID) OrElse Not IMDB.GetMovieInfo(Master.currMovie.IMDBID, Master.currMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False) Then
                                        Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString, New Media.Movie, Args.scrapeType)
                                    End If
                                    If Master.eSettings.UseStudioTags Then
                                        If UpdateMediaInfo() Then
                                            Master.currMovie.Studio = String.Concat(Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                                        End If
                                    End If

                                    Master.SaveMovieToNFO(Master.currMovie, sPath, drvRow.Item(6))
                                    drvRow.Item(4) = True
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(2) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB Then

                                        If Poster.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Posters) Then
                                            Poster.GetPreferredImage(Master.ImageType.Posters, Nothing, True)
                                            If Not IsNothing(Poster.Image) Then
                                                Poster.SaveAsPoster(sPath, drvRow.Item(6))
                                                drvRow.Item(2) = True
                                            Else
                                                MsgBox("A poster of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                Dim dImgSelect As New dlgImgSelect
                                                If dImgSelect.ShowDialog(Master.currMovie.IMDBID, sPath, Master.ImageType.Posters) = Windows.Forms.DialogResult.OK Then
                                                    drvRow.Item(2) = True
                                                End If
                                                dImgSelect.Dispose()
                                            End If
                                        End If
                                    End If

                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(3) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.eSettings.UseTMDB Then

                                        If Fanart.IsAllowedToDownload(sPath, drvRow.Item(6), Master.ImageType.Fanart) Then
                                            Fanart.GetPreferredImage(Master.ImageType.Fanart, fArt, True)

                                            If Not IsNothing(Fanart.Image) Then
                                                Fanart.SaveAsFanart(sPath, drvRow.Item(6))
                                                drvRow.Item(3) = True
                                                Master.currMovie.Fanart = fArt
                                            Else
                                                MsgBox("Fanart of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                Dim dImgSelect As New dlgImgSelect
                                                If dImgSelect.ShowDialog(Master.currMovie.IMDBID, sPath, Master.ImageType.Fanart) = Windows.Forms.DialogResult.OK Then
                                                    drvRow.Item(3) = True

                                                    If File.Exists(nfoPath) Then
                                                        'need to load movie from nfo here in case the movie already had
                                                        'an nfo.... currmovie would not be set to the proper movie
                                                        Master.currMovie = Master.LoadMovieFromNFO(nfoPath)
                                                        Master.currMovie.Fanart = fArt
                                                        Master.SaveMovieToNFO(Master.currMovie, sPath, drvRow.Item(6))
                                                    End If
                                                    fArt = Nothing
                                                End If
                                                dImgSelect.Dispose()
                                            End If
                                            fArt = Nothing
                                        End If
                                    End If
                                End If

                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.eSettings.AutoThumbs > 0 AndAlso Not Directory.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "extrathumbs")) Then
                                Me.CreateRandomThumbs(sPath)
                            End If
                        Next

                End Select
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        e.Result = Args.scrapeType

        TMDB = Nothing
        IMPA = Nothing
        Poster.Dispose()
        Poster = Nothing

        Fanart.Dispose()
        Fanart = Nothing
    End Sub

    Private Sub bwScraper_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwScraper.ProgressChanged
        Me.tslStatus.Text = e.UserState.ToString
        Me.tspbLoading.Value = e.ProgressPercentage
        Me.dgvMediaList.Update()
        Me.Refresh()
    End Sub

    Private Sub bwScraper_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwScraper.RunWorkerCompleted

        '//
        ' Thread finished: re-fill media list and load info for first item
        '\\

        Select Case e.Result
            Case Master.ScrapeType.CleanFolders
                LoadMedia(1)
            Case Else
                Try
                    Me.Invalidate()
                    Me.Refresh()
                    Me.dgvMediaList.Rows(0).Selected = True
                    Me.dgvMediaList.Rows(0).Visible = True
                    Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Rows(0).Cells(1)
                    'set tmpTitle to title in list - used for searching IMDB
                    Me.tmpTitle = Me.dgvMediaList.Item(1, 0).Value.ToString

                    Master.currMark = If(Me.dgvMediaList.Item(1, 0).Style.ForeColor = Color.Crimson, True, False)

                    If Not Me.dgvMediaList.Item(2, 0).Value AndAlso Not Me.dgvMediaList.Item(3, 0).Value AndAlso Not Me.dgvMediaList.Item(4, 0).Value Then
                        Me.ClearInfo()
                        Me.pnlNoInfo.Visible = True
                        Master.currPath = Me.dgvMediaList.Item(0, 0).Value.ToString
                        Master.isFile = Me.dgvMediaList.Item(6, 0).Value.ToString
                        Master.currMovie = New Media.Movie
                        Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
                        Me.tslStatus.Text = Master.currPath
                    Else
                        Me.pnlNoInfo.Visible = False
                        'try to load the info from the NFO
                        Me.LoadInfo(Me.dgvMediaList.Item(0, 0).Value.ToString, True, False, Me.dgvMediaList.Item(6, 0).Value)
                    End If

                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

                Me.tslLoading.Visible = False
                Me.tspbLoading.Visible = False
                Me.tslStatus.Text = String.Empty

                Me.tsbAutoPilot.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                Me.tsbEdit.Enabled = True
                Me.tsbRescrape.Enabled = True
                Me.tabsMain.Enabled = True
        End Select

    End Sub

#End Region '*** Background Workers



#Region "Routines/Functions"

    ' ########################################
    ' ###### GENERAL ROUTINES/FUNCTIONS ######
    ' ########################################

    Public Sub SetColors()

        Try
            With Me
                'top panel
                .pnlTop.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pnlInfoIcons.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pnlRating.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbVideo.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbResolution.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbAudio.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbChannels.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStudio.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar1.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar2.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar3.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar4.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .pbStar5.BackColor = Color.FromArgb(Master.eSettings.TopPanelColor)
                .lblTitle.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)
                .lblVotes.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)
                .lblRuntime.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)
                .lblTagline.ForeColor = Color.FromArgb(Master.eSettings.TopPanelTextColor)

                'background
                .scMain.Panel2.BackColor = Color.FromArgb(Master.eSettings.BackgroundColor)
                .pbFanart.BackColor = Color.FromArgb(Master.eSettings.BackgroundColor)

                'info panel
                .pnlInfoPanel.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtMediaInfo.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtMediaInfo.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .txtPlot.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtPlot.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .txtOutline.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .txtOutline.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .pnlActors.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lstActors.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lstActors.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .lblDirector.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblDirector.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .lblReleaseDate.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblReleaseDate.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .pnlTop250.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblTop250.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblTop250.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)

                .lblMIHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblMIHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblPlotHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblPlotHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblOutlineHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblOutlineHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblActorsHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblActorsHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblFilePathHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblFilePathHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblIMDBHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblIMDBHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblDirectorHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblDirectorHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblReleaseDateHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblReleaseDateHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblCertsHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblCertsHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)
                .lblInfoPanelHeader.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblInfoPanelHeader.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)

                'left panel
                .lblMediaCount.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblMediaCount.ForeColor = Color.FromArgb(Master.eSettings.PanelTextColor)
                .scMain.Panel1.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .pnlSearch.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub MoveGenres()

        '//
        ' Slide the genre images along with the panel and move with form resizing
        '\\

        Try
            For i As Integer = 0 To UBound(Me.pnlGenre)
                Me.pnlGenre(i).Left = ((Me.pnlInfoPanel.Right) - (i * 73)) - 73
                Me.pnlGenre(i).Top = Me.pnlInfoPanel.Top - 105
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub MoveMPAA()

        '//
        ' Slide the MPAA image along with the panel
        '\\

        Me.pnlMPAA.BringToFront()
        Me.pnlMPAA.Size = New Size(Me.pbMPAA.Width + 10, Me.pbMPAA.Height + 10)
        Me.pbMPAA.Location = New Point(4, 4)
        Me.pnlMPAA.Top = Me.pnlInfoPanel.Top - (Me.pnlMPAA.Height + 10)

    End Sub

    Public Sub LoadMedia(ByVal mediaType As Integer)

        '//
        ' Begin threads to fill datagrid with media data
        '\\


        Try
            If bwPrelim.IsBusy Then
                bwPrelim.CancelAsync()
                While bwPrelim.IsBusy
                    Application.DoEvents()
                End While
            End If

            If bwFolderData.IsBusy Then
                bwFolderData.CancelAsync()
                While bwFolderData.IsBusy
                    Application.DoEvents()
                End While
            End If

            Me.dgvMediaList.DataSource = Nothing

            Master.alFolderList.Clear()
            Master.alFileList.Clear()

            Me.pnlInfoPanel.Height = 25
            Me.btnDown.Enabled = False
            Me.btnMid.Enabled = False
            Me.btnUp.Enabled = False

            Me.ClearInfo()

            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.tsbEdit.Enabled = False
            Me.tsbRescrape.Enabled = False
            Me.tabsMain.Enabled = False
            Me.lblMediaCount.Visible = False

            Me.tslStatus.Text = "Performing preliminary tasks..."
            Me.loadType = mediaType

            Me.bwPrelim = New System.ComponentModel.BackgroundWorker
            Me.bwPrelim.WorkerSupportsCancellation = True
            Me.bwPrelim.RunWorkerAsync()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try


    End Sub

    Private Sub LoadInfo(ByVal sPath As String, ByVal doInfo As Boolean, ByVal doMI As Boolean, ByVal isFile As Boolean, Optional ByVal setEnabled As Boolean = False)

        '//
        ' Begin threads to load images and media info from nfos
        '\\

        Try
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.tsbRescrape.Enabled = False
            Me.tsbEdit.Enabled = False
            Me.tabsMain.Enabled = False
            Me.pnlNoInfo.Visible = False

            'set status bar text to movie path
            Me.tslStatus.Text = sPath

            Master.currPath = sPath
            Master.isFile = isFile

            If doMI Then
                Me.txtMediaInfo.Clear()
                Me.pbMILoading.Visible = True
            End If

            If Me.bwDownloadPic.IsBusy Then
                Me.bwDownloadPic.CancelAsync()
                While Me.bwDownloadPic.IsBusy
                    Application.DoEvents()
                End While
            End If

            If Me.bwMediaInfo.IsBusy Then
                Me.bwMediaInfo.CancelAsync()
                While Me.bwMediaInfo.IsBusy
                    Application.DoEvents()
                End While
            End If

            If Me.bwLoadInfo.IsBusy Then
                Me.bwLoadInfo.CancelAsync()
                While Me.bwLoadInfo.IsBusy
                    Application.DoEvents()
                End While
            End If

            If doMI Then
                Me.bwMediaInfo = New System.ComponentModel.BackgroundWorker
                Me.bwMediaInfo.WorkerSupportsCancellation = True
                Me.bwMediaInfo.RunWorkerAsync(New Arguments With {.setEnabled = setEnabled})
            End If

            If doInfo Then
                Me.ClearInfo()
                Master.currMovie.Clear()
                Me.bwLoadInfo = New System.ComponentModel.BackgroundWorker
                Me.bwLoadInfo.WorkerSupportsCancellation = True
                Me.bwLoadInfo.RunWorkerAsync()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Public Sub ClearInfo(Optional ByVal displayOnly As Boolean = False)

        '//
        ' Reset all info fields
        '\\
        Try
            With Me
                If Not IsNothing(.pbFanart.Image) Then
                    .pbFanart.Image.Dispose()
                    .pbFanart.Image = Nothing
                End If
                If Not IsNothing(.pbPoster.Image) Then
                    .pbPoster.Image.Dispose()
                    .pbPoster.Image = Nothing
                End If
                .pnlPoster.Visible = False

                'remove all the current genres
                Try
                    For iDel As Integer = UBound(.pnlGenre) To 0 Step -1
                        .scMain.Panel2.Controls.Remove(.pbGenre(iDel))
                        .scMain.Panel2.Controls.Remove(.pnlGenre(iDel))
                    Next
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

                If Not IsNothing(.pbMPAA.Image) Then
                    .pbMPAA.Image = Nothing
                End If
                .pnlMPAA.Visible = False

                .lblTitle.Text = String.Empty
                .lblVotes.Text = String.Empty
                .lblRuntime.Text = String.Empty
                .pnlTop250.Visible = False
                .lblTop250.Text = String.Empty
                .pbStar1.Image = Nothing
                .pbStar2.Image = Nothing
                .pbStar3.Image = Nothing
                .pbStar4.Image = Nothing
                .pbStar5.Image = Nothing
                .lstActors.Items.Clear()
                If Not IsNothing(.alActors) Then
                    .alActors.Clear()
                    .alActors = Nothing
                End If
                If Not IsNothing(.pbActors.Image) Then
                    .pbActors.Image.Dispose()
                    .pbActors.Image = Nothing
                End If
                .lblDirector.Text = String.Empty
                .lblReleaseDate.Text = String.Empty
                .txtCerts.Text = String.Empty
                .txtIMDBID.Text = String.Empty
                .txtFilePath.Text = String.Empty
                .txtOutline.Text = String.Empty
                .txtPlot.Text = String.Empty
                .lblTagline.Text = String.Empty
                If Not IsNothing(.pbMPAA.Image) Then
                    .pbMPAA.Image.Dispose()
                    .pbMPAA.Image = Nothing
                End If
                .pbStudio.Image = Nothing
                .pbVideo.Image = Nothing
                .pbAudio.Image = Nothing
                .pbResolution.Image = Nothing
                .pbChannels.Image = Nothing

                .MainPoster.Clear()
                .MainFanart.Clear()

                .txtMediaInfo.Text = String.Empty
                If Not displayOnly Then
                    Master.currNFO = String.Empty
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub fillScreenInfo()

        '//
        ' Get info from arralist (populated by bw) and parse to screen
        '\\

        Try
            Me.SuspendLayout()
            If Not String.IsNullOrEmpty(Master.currMovie.Title) AndAlso (Not String.IsNullOrEmpty(Master.currMovie.Year) AndAlso CInt(Master.currMovie.Year) > 0) Then
                Me.lblTitle.Text = String.Format("{0} ({1})", Master.currMovie.Title, Master.currMovie.Year)
            ElseIf Not String.IsNullOrEmpty(Master.currMovie.Title) AndAlso (String.IsNullOrEmpty(Master.currMovie.Year) OrElse CInt(Master.currMovie.Year) = 0) Then
                Me.lblTitle.Text = Master.currMovie.Title
            ElseIf String.IsNullOrEmpty(Master.currMovie.Title) AndAlso (Not String.IsNullOrEmpty(Master.currMovie.Year) AndAlso CInt(Master.currMovie.Year) > 0) Then
                Me.lblTitle.Text = String.Format("Unknown Movie ({0})", Master.currMovie.Year)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Votes) Then
                Me.lblVotes.Text = String.Format("{0} Votes", Master.currMovie.Votes)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Runtime) Then
                Me.lblRuntime.Text = String.Format("Runtime: {0}", Master.currMovie.Runtime)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Top250) AndAlso IsNumeric(Master.currMovie.Top250) AndAlso CInt(Master.currMovie.Top250) > 0 Then
                Me.pnlTop250.Visible = True
                Me.lblTop250.Text = Master.currMovie.Top250
            Else
                Me.pnlTop250.Visible = False
            End If

            Me.txtOutline.Text = Master.currMovie.Outline
            Me.txtPlot.Text = Master.currMovie.Plot
            Me.lblTagline.Text = Master.currMovie.Tagline

            Me.alActors = New ArrayList

            If Master.currMovie.Actors.Count > 0 Then
                Me.pbActors.Image = My.Resources.actor_silhouette
                For Each imdbAct As Media.Person In Master.currMovie.Actors
                    If Not String.IsNullOrEmpty(imdbAct.Thumb) Then

                        'Was it XBMC or MIP that set some of the actor thumbs to
                        'the default "Add Pic" image??
                        If Not Strings.InStr(imdbAct.Thumb.ToLower, "addtiny.gif") > 0 Then
                            Me.alActors.Add(imdbAct.Thumb)
                        Else
                            Me.alActors.Add("none")
                        End If
                    Else
                        Me.alActors.Add("none")
                    End If

                    Me.lstActors.Items.Add(String.Format("{0} as {1}", imdbAct.Name, imdbAct.Role))
                Next
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.MPAA) Then
                Dim tmpRatingImg As Image = Master.GetRatingImage(Master.currMovie.MPAA)
                If Not IsNothing(tmpRatingImg) Then
                    Me.pbMPAA.Image = tmpRatingImg
                    Me.MoveMPAA()
                    Me.pnlMPAA.Visible = True
                Else
                    Me.pnlMPAA.Visible = False
                End If
            End If

            Dim tmpRating As String = Master.currMovie.Rating
            If Not String.IsNullOrEmpty(tmpRating) AndAlso IsNumeric(tmpRating) AndAlso CDbl(tmpRating) > 0 Then
                Me.BuildStars(CDbl(tmpRating))
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Genre) Then
                Me.createGenreThumbs(Master.currMovie.Genre)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.StudioReal) Then
                Me.pbStudio.Image = Master.GetStudioImage(Strings.Trim(Master.currMovie.StudioReal))
            ElseIf Not String.IsNullOrEmpty(Master.currMovie.Studio) Then
                If Strings.InStr(Master.currMovie.Studio, "/") Then
                    Master.currMovie.StudioReal = Strings.Trim(Strings.Left(Master.currMovie.Studio, Strings.InStr(Master.currMovie.Studio, "/") - 1))
                    Me.pbStudio.Image = Master.GetStudioImage(Master.currMovie.StudioReal)
                Else
                    Me.pbStudio.Image = Master.GetStudioImage("####")
                End If
            Else
                Me.pbStudio.Image = Master.GetStudioImage("####")
            End If

            If Master.eSettings.UseStudioTags = True Then
                If Not String.IsNullOrEmpty(Master.currMovie.Studio) Then
                    Master.GetAVImages(Master.currMovie.Studio, Master.currPath)
                    Me.pnlInfoIcons.Width = 346
                    Me.pbStudio.Left = 277
                Else
                    Master.GetAVImages("", Master.currPath)
                End If
            Else
                Me.pnlInfoIcons.Width = 70
                Me.pbStudio.Left = 0
            End If

            Me.lblDirector.Text = Master.currMovie.Director

            Me.txtIMDBID.Text = Master.currMovie.IMDBID

            Me.txtFilePath.Text = Master.currPath

            Me.lblReleaseDate.Text = Master.currMovie.ReleaseDate
            Me.txtCerts.Text = Master.currMovie.Certification

            If Not IsNothing(Master.currMovie.FileInfo) Then
                Me.txtMediaInfo.Text = Master.FIToString(Master.currMovie.FileInfo)
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Me.ResumeLayout()
    End Sub

    Private Sub BuildStars(ByVal dblRating As Double)

        '//
        ' Convert # rating to star images
        '\\

        Try
            With Me
                .pbStar1.Image = Nothing
                .pbStar2.Image = Nothing
                .pbStar3.Image = Nothing
                .pbStar4.Image = Nothing
                .pbStar5.Image = Nothing

                If dblRating >= Double.Parse(0.5) Then ' if rating is less than .5 out of ten, consider it a 0
                    Select Case (dblRating / 2)
                        Case Is <= Double.Parse(0.5)
                            .pbStar1.Image = My.Resources.starhalf
                        Case Is <= 1
                            .pbStar1.Image = My.Resources.star
                        Case Is <= Double.Parse(1.5)
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.starhalf
                        Case Is <= 2
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                        Case Is <= Double.Parse(2.5)
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.starhalf
                        Case Is <= 3
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                        Case Is <= Double.Parse(3.5)
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.starhalf
                        Case Is <= 4
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                        Case Is <= Double.Parse(4.5)
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                            .pbStar5.Image = My.Resources.starhalf
                        Case Is <= 5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                            .pbStar5.Image = My.Resources.star
                    End Select
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub createGenreThumbs(ByVal strGenres As String)

        '//
        ' Parse the genre string and create panels/pictureboxes for each one
        '\\

        Dim genreArray() As String

        Try
            genreArray = Strings.Split(strGenres, "/")
            For i As Integer = 0 To UBound(genreArray)
                ReDim Preserve Me.pnlGenre(i)
                ReDim Preserve Me.pbGenre(i)
                Me.pnlGenre(i) = New Panel()
                Me.pbGenre(i) = New PictureBox()
                Me.pbGenre(i).Name = Trim(genreArray(i)).ToUpper
                Me.pnlGenre(i).Size = New Size(68, 100)
                Me.pbGenre(i).Size = New Size(62, 94)
                Me.pnlGenre(i).BackColor = Color.Gainsboro
                Me.pnlGenre(i).BorderStyle = BorderStyle.FixedSingle
                Me.pbGenre(i).SizeMode = PictureBoxSizeMode.StretchImage
                Me.pbGenre(i).Image = Master.GetGenreImage(Trim(genreArray(i)))
                Me.pnlGenre(i).Left = ((Me.pnlInfoPanel.Right) - (i * 73)) - 73
                Me.pbGenre(i).Left = 2
                Me.pnlGenre(i).Top = Me.pnlInfoPanel.Top - 105
                Me.pbGenre(i).Top = 2
                Me.scMain.Panel2.Controls.Add(Me.pnlGenre(i))
                Me.pnlGenre(i).Controls.Add(Me.pbGenre(i))
                Me.pnlGenre(i).BringToFront()
                AddHandler Me.pbGenre(i).MouseHover, AddressOf pbGenre_MouseHover
                AddHandler Me.pbGenre(i).MouseLeave, AddressOf pbGenre_MouseLeave
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub ScrapeData(ByVal sType As Master.ScrapeType, Optional ByVal doSearch As Boolean = False)

        Try
            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.tsbEdit.Enabled = False
            Me.tsbRescrape.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tspbLoading.Style = ProgressBarStyle.Continuous

            Select Case sType
                Case Master.ScrapeType.FullAsk
                    Me.tspbLoading.Style = ProgressBarStyle.Continuous
                    Me.tspbLoading.Maximum = Me.dgvMediaList.RowCount
                    Me.tslLoading.Text = "Updating Media (All Movies - Ask):"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                    End If
                Case Master.ScrapeType.FullAuto
                    Me.tspbLoading.Maximum = Me.dgvMediaList.RowCount
                    Me.tslLoading.Text = "Updating Media (All Movies - Auto):"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                    End If
                Case Master.ScrapeType.UpdateAsk
                    Dim chkCount As Integer = 0
                    'first count all the items in the list with no info just for the purpose of having a progress bar
                    For i As Integer = 0 To Me.dgvMediaList.RowCount - 1
                        chkCount += If(Not Me.dgvMediaList.Item(2, i).Value OrElse Not Me.dgvMediaList.Item(3, i).Value OrElse Not Me.dgvMediaList.Item(4, i).Value, 1, 0)
                    Next

                    Me.tspbLoading.Maximum = chkCount
                    Me.tslLoading.Text = "Updating Media (Movies Missing Items - Ask):"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If chkCount > 0 Then
                        If Not bwScraper.IsBusy Then
                            bwScraper.WorkerSupportsCancellation = True
                            bwScraper.WorkerReportsProgress = True
                            bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                        End If
                    Else
                        Me.tslLoading.Visible = False
                        Me.tspbLoading.Visible = False
                        Me.tslStatus.Text = String.Empty

                        Me.tsbAutoPilot.Enabled = True
                        Me.tsbRefreshMedia.Enabled = True
                        Me.tsbEdit.Enabled = True
                        Me.tsbRescrape.Enabled = True
                        Me.tabsMain.Enabled = True
                    End If
                Case Master.ScrapeType.UpdateAuto
                    Dim chkCount As Integer = 0

                    'first count all the items in the list with no info just for the purpose of having a progress bar
                    For i As Integer = 0 To Me.dgvMediaList.RowCount - 1
                        chkCount += If(Not Me.dgvMediaList.Item(2, i).Value OrElse Not Me.dgvMediaList.Item(3, i).Value OrElse Not Me.dgvMediaList.Item(4, i).Value, 1, 0)
                    Next

                    Me.tspbLoading.Maximum = chkCount
                    Me.tslLoading.Text = "Updating Media (Movies Missing Items - Auto):"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If chkCount > 0 Then
                        If Not bwScraper.IsBusy Then
                            bwScraper.WorkerSupportsCancellation = True
                            bwScraper.WorkerReportsProgress = True
                            bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                        End If
                    Else
                        Me.tslLoading.Visible = False
                        Me.tspbLoading.Visible = False
                        Me.tslStatus.Text = String.Empty

                        Me.tsbAutoPilot.Enabled = True
                        Me.tsbRefreshMedia.Enabled = True
                        Me.tsbEdit.Enabled = True
                        Me.tsbRescrape.Enabled = True
                        Me.tabsMain.Enabled = True
                    End If
                Case Master.ScrapeType.MIOnly
                    Me.tspbLoading.Maximum = Me.dgvMediaList.RowCount
                    Me.tslLoading.Text = "Updating Media (All Movies - MI Only):"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                    End If
                Case Master.ScrapeType.CleanFolders
                    Me.tspbLoading.Maximum = Me.dgvMediaList.RowCount
                    Me.tslLoading.Text = "Cleaning Folders:"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                    End If
                Case Master.ScrapeType.SingleScrape
                    Me.ClearInfo(True)
                    Me.tslStatus.Text = String.Format("Re-scraping {0}", Master.currMovie.Title)
                    Me.tslLoading.Text = "Scraping:"
                    Me.tspbLoading.Maximum = 13
                    Me.ReportDownloadPercent = True
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True


                    If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) AndAlso doSearch = False Then
                        IMDB.GetMovieInfoAsync(Master.currMovie.IMDBID, Master.currMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast)
                    Else
                        Master.tmpMovie = New Media.Movie
                        If dlgIMDBSearchResults.ShowDialog(Me.tmpTitle) = Windows.Forms.DialogResult.OK Then
                            If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                                Me.ClearInfo(True)
                                Me.tslStatus.Text = String.Format("Scraping {0}", Master.tmpMovie.Title)
                                Me.tslLoading.Text = "Scraping:"
                                Me.tspbLoading.Maximum = 13
                                Me.tspbLoading.Style = ProgressBarStyle.Continuous
                                Me.ReportDownloadPercent = True
                                Me.tslLoading.Visible = True
                                Me.tspbLoading.Visible = True
                                IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID, Master.currMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast)
                            End If
                        Else
                            Me.tslLoading.Visible = False
                            Me.tspbLoading.Visible = False
                            Me.tslStatus.Text = String.Empty
                            Me.tsbAutoPilot.Enabled = True
                            Me.tsbRefreshMedia.Enabled = True
                            Me.tsbEdit.Enabled = True
                            Me.tsbRescrape.Enabled = True
                            Me.tabsMain.Enabled = True
                        End If
                    End If
            End Select
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Function UpdateMediaInfo() As Boolean
        Try
            Dim MI As New MediaInfo.MInfo
            Dim miFileInfo = New MediaInfo.Fileinfo

            If Not Master.GetExtFromPath(Master.currPath) = ".rar" AndAlso Not Master.GetExtFromPath(Master.currPath) = ".iso" Then
                MI.GetMovieMIFromPath(miFileInfo, Master.currPath)

                Master.currMovie.FileInfo = miFileInfo

                MI = Nothing
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return False
        End Try

    End Function

    Private Sub MovieInfoDownloadedPercent(ByVal iPercent As Integer)
        If Me.ReportDownloadPercent = True Then
            Me.tspbLoading.Value = iPercent
            Me.Refresh()
        End If
    End Sub

    Private Sub MovieInfoDownloaded(ByVal bSuccess As Boolean)

        Try
            If bSuccess Then
                If Master.eSettings.UseStudioTags Then
                    Me.tslLoading.Text = "Scanning Media Info:"
                    Me.tspbLoading.Value = Me.tspbLoading.Maximum
                    Me.tspbLoading.Style = ProgressBarStyle.Marquee
                    Me.tspbLoading.MarqueeAnimationSpeed = 25
                    Me.Refresh()
                    If Me.UpdateMediaInfo Then
                        Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                    End If
                End If
                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.isFile, Master.ImageType.Posters) Then
                        Dim dImgSelect As New dlgImgSelect
                        dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Posters)
                        dImgSelect.Dispose()
                    End If
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.isFile, Master.ImageType.Fanart) Then
                        Dim dImgSelect As New dlgImgSelect
                        dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Fanart)
                        dImgSelect.Dispose()
                    End If
                    tmpImages.Dispose()
                    tmpImages = Nothing
                End If

                Dim dEditMovie As New dlgEditMovie
                If dEditMovie.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    If Master.currMark Then
                        Me.dgvMediaList.SelectedRows(0).Cells(1).Style.ForeColor = Color.Crimson
                        Me.dgvMediaList.SelectedRows(0).Cells(1).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                    End If
                    Me.ReCheckItems(Me.dgvMediaList.SelectedRows(0).Index)
                End If
                dEditMovie.Dispose()
                Me.LoadInfo(Master.currPath, True, False, Master.isFile)
            Else
                MsgBox("Unable to retrieve movie details from the internet. Please check your connection and try again.", MsgBoxStyle.Exclamation, "Error Retrieving Details")
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.tslLoading.Visible = False
        Me.tspbLoading.Visible = False
        Me.tslStatus.Text = String.Empty
        Me.tsbAutoPilot.Enabled = True
        Me.tsbRefreshMedia.Enabled = True
        Me.tsbEdit.Enabled = True
        Me.tsbRescrape.Enabled = True
        Me.tabsMain.Enabled = True

    End Sub

    Private Sub ReCheckItems(ByVal iIndex As Integer)
        Dim sPath As String = Me.dgvMediaList.Item(0, iIndex).Value.ToString
        Dim aResults(3) As Boolean
        Try
            Dim parPath As String = Directory.GetParent(sPath).FullName
            Dim tmpName As String = Path.Combine(parPath, Master.CleanStackingMarkers(Master.RemoveExtFromFile(Master.GetNameFromPath(sPath))))
            Dim hasNfo As Boolean = False
            Dim hasPoster As Boolean = False
            Dim hasFanart As Boolean = False
            Dim hasTrailer As Boolean = False

            'fanart
            If File.Exists(String.Concat(tmpName, "-fanart.jpg")) OrElse File.Exists(String.Concat(tmpName, ".fanart.jpg")) OrElse File.Exists(Path.Combine(parPath, "fanart.jpg")) OrElse _
            File.Exists(Path.Combine(parPath, "video_ts-fanart.jpg")) OrElse File.Exists(Path.Combine(parPath, "video_ts.fanart.jpg")) Then
                hasFanart = True
            End If

            'poster
            If File.Exists(String.Concat(tmpName, ".jpg")) OrElse File.Exists(Path.Combine(parPath, "movie.jpg")) OrElse _
                File.Exists(Path.Combine(parPath, "poster.jpg")) OrElse File.Exists(Path.Combine(parPath, "folder.jpg")) OrElse _
                File.Exists(String.Concat(tmpName, ".tbn")) OrElse File.Exists(Path.Combine(parPath, "movie.tbn")) OrElse _
                File.Exists(Path.Combine(parPath, "poster.tbn")) OrElse File.Exists(Path.Combine(parPath, "video_ts.tbn")) OrElse _
                File.Exists(Path.Combine(parPath, "video_ts.jpg")) Then
                hasPoster = True
            End If

            'nfo
            If File.Exists(String.Concat(tmpName, ".nfo")) OrElse File.Exists(Path.Combine(parPath, "movie.nfo")) OrElse _
                File.Exists(Path.Combine(parPath, "video_ts.nfo")) Then
                hasNfo = True
            End If

            Dim sExt() As String = Split(".avi,.divx,.mkv,.iso,.mpg,.mp4,.wmv,.wma,.mov,.mts,.m2t,.img,.dat,.bin,.cue,.vob,.dvb,.evo,.asf,.asx,.avs,.nsv,.ram,.ogg,.ogm,.ogv,.flv,.swf,.nut,.viv,.rar,.m2ts,.dvr-ms,.m4v")

            For Each t As String In sExt
                If File.Exists(String.Concat(tmpName, "-trailer", t)) OrElse File.Exists(String.Concat(tmpName, "[trailer]", t)) Then
                    hasTrailer = True
                    Exit For
                End If
            Next

            Me.dgvMediaList.Item(3, iIndex).Value = hasFanart
            Me.dgvMediaList.Item(2, iIndex).Value = hasPoster
            Me.dgvMediaList.Item(4, iIndex).Value = hasNfo
            Me.dgvMediaList.Item(5, iIndex).Value = hasTrailer

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetCleanFolders()

        '//
        ' Set the Clean Folders menu item enabled/disabled depending on if clean folder options are set
        '\\

        With Master.eSettings
            If .CleanDotFanartJPG OrElse .CleanFanartJPG OrElse .CleanFolderJPG OrElse .CleanMovieFanartJPG OrElse _
            .CleanMovieJPG OrElse .CleanMovieNameJPG OrElse .CleanMovieNFO OrElse .CleanMovieNFOB OrElse _
            .CleanMovieTBN OrElse .CleanMovieTBNB OrElse .CleanPosterJPG OrElse .CleanPosterTBN Then
                Me.CleanFoldersToolStripMenuItem.Enabled = True
            Else
                Me.CleanFoldersToolStripMenuItem.Enabled = False
            End If
        End With
    End Sub

    Private Sub SortFiles(ByVal sPath As String)
        Dim tmpAL As New ArrayList
        Dim tmpPath As String = String.Empty
        Dim tmpName As String = String.Empty

        If Directory.Exists(sPath) Then
            Dim di As New DirectoryInfo(sPath)
            Dim lFi As New List(Of FileInfo)

            lFi.AddRange(di.GetFiles())

            For Each sFile As FileInfo In lFi
                tmpName = Master.CleanStackingMarkers(Master.RemoveExtFromFile(sFile.Name))
                tmpName = tmpName.Replace(".fanart", String.Empty)
                tmpName = tmpName.Replace("-fanart", String.Empty)
                tmpPath = Path.Combine(sPath, tmpName)
                If Not Directory.Exists(tmpPath) Then
                    Directory.CreateDirectory(tmpPath)
                End If

                File.Move(sFile.FullName, Path.Combine(tmpPath, sFile.Name))
            Next
        End If
    End Sub

    Private Sub CreateRandomThumbs(ByVal sPath As String)
        Dim ffmpeg As New Process()
        Dim intSeconds As Integer = 0
        Dim intAdd As Integer = 0
        Dim ThumbCount As Integer = Master.eSettings.AutoThumbs
        Dim tPath As String = Path.Combine(Directory.GetParent(sPath).FullName, "extrathumbs")
        Dim iMod As Integer = Master.GetExtraModifier(Master.currPath)

        If Not Directory.Exists(tPath) Then
            Directory.CreateDirectory(tPath)
        End If

        ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
        ffmpeg.EnableRaisingEvents = False
        ffmpeg.StartInfo.UseShellExecute = False
        ffmpeg.StartInfo.CreateNoWindow = True
        ffmpeg.StartInfo.RedirectStandardOutput = True
        ffmpeg.StartInfo.RedirectStandardError = True

        'first get the duration
        ffmpeg.StartInfo.Arguments = String.Format("-i ""{0}"" -an", sPath)
        ffmpeg.Start()
        Dim d As StreamReader = ffmpeg.StandardError
        Do
            Dim s As String = d.ReadLine()
            If s.Contains("Duration: ") Then
                Dim sTime As String = Regex.Match(s, "Duration: (?<dur>.*?),").Groups("dur").ToString
                Dim ts As TimeSpan = CDate(CDate(DateTime.Today & " " & sTime)).Subtract(CDate(DateTime.Today))
                intSeconds = ((ts.Hours * 60) + ts.Minutes) * 60 + ts.Seconds
            End If
        Loop While Not d.EndOfStream

        ffmpeg.WaitForExit()
        ffmpeg.Close()

        If intSeconds > ThumbCount + 2 Then
            intSeconds = intSeconds / (ThumbCount + 2)
            intAdd = intSeconds
            intSeconds += intAdd

            For i = 0 To (ThumbCount - 1)
                If Me.bwScraper.CancellationPending Then Exit For
                'check to see if file already exists... if so, don't bother running ffmpeg since we're not
                'overwriting current thumbs anyway
                If Not File.Exists(Path.Combine(tPath, String.Concat("thumb", (i + iMod + 1), ".jpg"))) Then
                    ffmpeg.StartInfo.Arguments = String.Format("-ss {0} -i ""{1}"" -an -f rawvideo -vframes 1 -s 1280x720 -vcodec mjpeg ""{2}""", intSeconds, sPath, Path.Combine(tPath, String.Concat("thumb", (i + iMod + 1), ".jpg")))
                    ffmpeg.Start()
                    ffmpeg.WaitForExit()
                    ffmpeg.Close()
                End If
                intSeconds += intAdd
            Next
        End If
        ffmpeg.Dispose()

    End Sub
#End Region

End Class