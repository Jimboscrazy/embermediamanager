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
    Private pImage As Image
    Private fImage As Image
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

    Private Sub frmMain_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown

        '//
        ' Make sure the window is kicked to the front when loading
        '\\

        Me.BringToFront()

    End Sub

    Private Sub frmMain_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '//
        ' Do some events before closing
        '\\

        'save the list of movies to settings so we know which ones are new

        If Not Me.bwPrelim.IsBusy AndAlso Not Me.bwFolderData.IsBusy Then
            Master.uSettings.MovieList.Clear()
            For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
                Master.uSettings.MovieList.Add(drvRow.Cells(1).Value.ToString)
            Next
            Master.uSettings.Save()
        End If

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

    ' ########################################
    ' ######### FORM/CONTROLS EVENTS #########
    ' ########################################

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
            If dlgSettings.ShowDialog() = Windows.Forms.DialogResult.OK Then

                Me.SetColors()

                If Me.dgvMediaList.RowCount > 0 Then
                    Me.dgvMediaList.Columns(2).Visible = Not Master.uSettings.MovieMediaCol
                    Me.dgvMediaList.Columns(3).Visible = Not Master.uSettings.MoviePosterCol
                    Me.dgvMediaList.Columns(4).Visible = Not Master.uSettings.MovieFanartCol
                    Me.dgvMediaList.Columns(5).Visible = Not Master.uSettings.MovieInfoCol
                    Me.dgvMediaList.Columns(6).Visible = Not Master.uSettings.MovieTrailerCol

                    'Trick to autosize the first column, but still allow resizing by user
                    Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    Me.dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                End If
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

            Dim sPath As String = Application.StartupPath & "\Log\errlog.txt"
            If File.Exists(sPath) Then
                Master.MoveFileWithStream(sPath, sPath.Insert(sPath.LastIndexOf("."), "-old"))
                File.Delete(sPath)
            End If

            Master.uSettings.Load()

            Me.SetColors()
            Me.pnlInfoPanel.Height = 25
            Me.ClearInfo()

            Me.LoadMedia(1)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub lstActors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstActors.SelectedIndexChanged

        '//
        ' Begin thread to download actor image if one exists
        '\\
        Try
            If Not Me.alActors.Item(Me.lstActors.SelectedIndex).ToString = "none" Then

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
                Me.bwDownloadPic.RunWorkerAsync(New Arguments With {.pType = PicType.Actor, .pURL = Me.alActors.Item(Me.lstActors.SelectedIndex).ToString})

            Else
                If Not IsNothing(Me.pbActors.Image) Then
                    Me.pbActors.Image.Dispose()
                    Me.pbActors.Image = Nothing
                End If
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
                If Me.pnlInfoPanel.Height = 275 Then
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
            Me.LoadInfo(Master.currPath, False, True, True, True)
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

        '  Try
        If dlgEditMovie.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Me.ReCheckItems(Me.dgvMediaList.SelectedRows(0).Index)
            Me.LoadInfo(Master.currPath, True, False, Master.isFile)
        End If
        ' Catch ex As Exception
        'Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        ' End Try
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


    Private Sub CleanFoldersToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CleanFoldersToolStripMenuItem.Click

        '//
        ' Clean all items in folders that match user selected types
        '\\

        Me.ScrapeData(Master.ScrapeType.CleanFolders)

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
        If Me.dgvMediaList.SelectedRows.Count > 0 Then
            'set tmpTitle to title in list - used for searching IMDB
            Me.tmpTitle = Me.dgvMediaList.Item(1, currRow).Value.ToString
            'try to load the info from the NFO
            Me.LoadInfo(Me.dgvMediaList.Item(0, currRow).Value.ToString, True, False, Me.dgvMediaList.Item(7, currRow).Value)
        End If
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
        Me.dgvMediaList.SuspendLayout()
        Me.dgvMediaList.CurrentCell = Nothing
        Me.dgvMediaList.ScrollBars = ScrollBars.None
        'reset the search dt when txtsearch is empty
        If String.IsNullOrEmpty(Me.txtSearch.Text) Then
            For Each Row As DataGridViewRow In Me.dgvMediaList.Rows
                Row.Visible = True
            Next Row
        Else
            For Each Row As DataGridViewRow In Me.dgvMediaList.Rows
                Row.Visible = True
            Next Row
            For Each dgvRow As DataGridViewRow In Me.dgvMediaList.Rows
                If Not Strings.Left(dgvRow.Cells(1).Value.ToString, txtSearch.Text.Length).ToLower = Me.txtSearch.Text.ToLower Then
                    dgvRow.Visible = False
                End If
            Next
        End If

        Dim foundOne As Boolean = False
        For Each Row As DataGridViewRow In dgvMediaList.Rows
            If Row.Visible Then
                If Not Me.currRow = Row.Index Then
                    Me.dgvMediaList.CurrentCell = Row.Cells(1)
                    Row.Cells(1).Selected = True
                    Me.currRow = Row.Index
                End If
                foundOne = True
                Exit For
            End If
        Next Row

        If Not foundOne Then Me.ClearInfo()

        Me.dgvMediaList.ScrollBars = ScrollBars.Both
        Me.dgvMediaList.ResumeLayout()
        Me.tmrSearch.Enabled = False
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
                alMedia = Master.uSettings.MovieFolders
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

                Me.lblMediaCount.Text = String.Format("Media Count: {0}", Master.alFolderList.Count + Master.alFileList.Count)
                Me.lblMediaCount.Visible = True

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

        Try
            Me.dtMedia.Clear()
            Me.dtMedia.Reset()

            'set up the columns for the temporary datatable
            Me.dtMedia.Columns.Add("Path", GetType(System.String))
            Me.dtMedia.Columns.Add("Name", GetType(System.String))
            Me.dtMedia.Columns.Add("Media", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Poster", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Fanart", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Info", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Trailer", GetType(System.Boolean))
            Me.dtMedia.Columns.Add("Type", GetType(System.Boolean))

            'process the folder type media
            For Each sName As String In Master.alFolderList
                If Me.bwFolderData.CancellationPending Then Return
                mPath = Master.GetMoviePath(sName)

                If Master.uSettings.UseNameFromNfo Then
                    tmpMovie = Master.LoadMovieFromNFO(Master.GetNfoPath(mPath, False))
                    mName = tmpMovie.Title
                    tmpMovie = Nothing
                    If String.IsNullOrEmpty(mName) Then
                        If Master.uSettings.UseFolderName Then
                            mName = Master.GetNameFromPath(sName)
                        Else
                            mName = Master.RemoveExtFromFile(mPath)
                        End If
                    End If
                Else
                    If Master.uSettings.UseFolderName Then
                        mName = Master.GetNameFromPath(sName)
                    Else
                        mName = Master.RemoveExtFromFile(mPath)
                    End If
                End If

                cleanName = Master.FilterName(mName)

                Me.bwFolderData.ReportProgress(currentIndex, cleanName)

                If Not (String.IsNullOrEmpty(cleanName) OrElse File.Exists(sName & "\specialfolder.nfo")) Then

                    Dim newRow(7) As Object

                    newRow(0) = mPath
                    newRow(1) = cleanName
                    aContents = Master.GetFolderContents(mPath, False)
                    newRow(2) = aContents(0)
                    newRow(3) = aContents(1)
                    newRow(4) = aContents(2)
                    newRow(5) = aContents(3)
                    newRow(6) = aContents(4)
                    newRow(7) = False

                    Me.dtMedia.LoadDataRow(newRow, True)

                    aContents = Nothing
                    mName = Nothing
                    newRow = Nothing
                    currentIndex += 1
                End If
            Next

            'process the file type media
            For Each sFile As IO.FileInfo In Master.alFileList
                If Me.bwFolderData.CancellationPending Then Return

                'parse just the movie name
                If Master.uSettings.UseNameFromNfo Then
                    tmpMovie = Master.LoadMovieFromNFO(Master.GetNfoPath(sFile.FullName.ToString, True))
                    mName = tmpMovie.Title
                    tmpMovie = Nothing
                    If String.IsNullOrEmpty(mName) Then
                        mName = Master.RemoveExtFromFile(sFile.Name.ToString).ToString
                    End If
                Else
                    mName = Master.RemoveExtFromFile(sFile.Name.ToString).ToString
                End If

                cleanName = Master.FilterName(mName).ToString

                Me.bwFolderData.ReportProgress(currentIndex, cleanName)

                If Not String.IsNullOrEmpty(cleanName) Then

                    Dim newFileRow(7) As Object

                    newFileRow(0) = sFile.FullName
                    newFileRow(1) = cleanName
                    'check what's in the folder
                    aContents = Master.GetFolderContents(sFile.FullName.ToString, True)
                    newFileRow(2) = aContents(0)
                    newFileRow(3) = aContents(1)
                    newFileRow(4) = aContents(2)
                    newFileRow(5) = aContents(3)
                    newFileRow(6) = aContents(4)
                    newFileRow(7) = True

                    Me.dtMedia.LoadDataRow(newFileRow, True)

                    aContents = Nothing
                    mName = Nothing
                    newFileRow = Nothing
                    currentIndex += 1
                End If

            Next
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
                    .dgvMediaList.Columns(2).Visible = Not Master.uSettings.MovieMediaCol
                    .dgvMediaList.Columns(3).Width = 20
                    .dgvMediaList.Columns(3).Resizable = True
                    .dgvMediaList.Columns(3).ReadOnly = True
                    .dgvMediaList.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(3).Visible = Not Master.uSettings.MoviePosterCol
                    .dgvMediaList.Columns(4).Width = 20
                    .dgvMediaList.Columns(4).Resizable = True
                    .dgvMediaList.Columns(4).ReadOnly = True
                    .dgvMediaList.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(4).Visible = Not Master.uSettings.MovieFanartCol
                    .dgvMediaList.Columns(5).Width = 20
                    .dgvMediaList.Columns(5).Resizable = True
                    .dgvMediaList.Columns(5).ReadOnly = True
                    .dgvMediaList.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(5).Visible = Not Master.uSettings.MovieInfoCol
                    .dgvMediaList.Columns(6).Width = 20
                    .dgvMediaList.Columns(6).Resizable = True
                    .dgvMediaList.Columns(6).ReadOnly = True
                    .dgvMediaList.Columns(6).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(6).Visible = Not Master.uSettings.MovieTrailerCol
                    .dgvMediaList.Columns(7).Visible = False

                    'Trick to autosize the first column, but still allow resizing by user
                    .dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .dgvMediaList.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                    'Trick to work around the blank table bug in the DGV
                    .dgvMediaList.Sort(.dgvMediaList.Columns(1), ComponentModel.ListSortDirection.Descending)
                    .dgvMediaList.Sort(.dgvMediaList.Columns(1), ComponentModel.ListSortDirection.Ascending)

                    For Each drvRow As DataGridViewRow In .dgvMediaList.Rows
                        If Not Master.uSettings.MovieList.Contains(drvRow.Cells(1).Value.ToString) Then
                            drvRow.Cells(1).Style.ForeColor = Color.Green
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

        Me.loadType = 0
    End Sub

    Private Sub bwMediaInfo_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwMediaInfo.DoWork

        '//
        ' Thread to procure technical and tag information about media via MediaInfo.dll
        '\\

        Try
            Dim Args As Arguments = e.Argument
            If Me.UpdateMediaInfo() Then
                If Master.uSettings.UseStudioTags = True Then
                    Master.currMovie.Studio = Master.currMovie.StudioReal & Master.FITagData(Master.currMovie.FileInfo)
                End If
                Master.SaveMovieToNFO(Master.currMovie, Master.currNFO)
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
                If Master.uSettings.UseStudioTags = True Then
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

            If Not IsNothing(Me.fImage) Then
                Me.fImage = Nothing
            End If

            If Not IsNothing(Me.pImage) Then
                Me.pImage = Nothing
            End If

            Dim fPath As String = Master.GetFanartPath(Master.currPath, Master.isFile)

            If Not String.IsNullOrEmpty(fPath) AndAlso File.Exists(fPath) Then
                Dim fsFImage As New FileStream(fPath, FileMode.Open, FileAccess.Read)
                Me.fImage = Image.FromStream(fsFImage)
                fsFImage.Close()
                fsFImage = Nothing
            Else
                fPath = Master.SecondaryFileCheck(Master.currPath, "fanart")
                If Not String.IsNullOrEmpty(fPath) Then
                    Dim fsFImage As New FileStream(fPath, FileMode.Open, FileAccess.Read)
                    Me.fImage = Image.FromStream(fsFImage)
                    fsFImage.Close()
                    fsFImage = Nothing
                End If
            End If

            Dim pPath As String = Master.GetPosterPath(Master.currPath, Master.isFile)

            If Not String.IsNullOrEmpty(pPath) AndAlso File.Exists(pPath) Then
                Dim fsPImage As New FileStream(pPath, FileMode.Open, FileAccess.Read)
                Me.pImage = Image.FromStream(fsPImage)
                fsPImage.Close()
                fsPImage = Nothing
            Else
                pPath = Master.SecondaryFileCheck(Master.currPath, "poster")
                If Not String.IsNullOrEmpty(pPath) Then
                    Dim fsPImage As New FileStream(pPath, FileMode.Open, FileAccess.Read)
                    Me.pImage = Image.FromStream(fsPImage)
                    fsPImage.Close()
                    fsPImage = Nothing
                End If
            End If

            'wait for mediainfo to update the nfo
            Do While bwMediaInfo.IsBusy
                Application.DoEvents()
            Loop

            'read nfo if it's there
            Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
            If Not String.IsNullOrEmpty(Master.currNFO) Then
                Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)
            Else
                Master.currNFO = Master.SecondaryFileCheck(Master.currPath, "nfo")
                If Not String.IsNullOrEmpty(Master.currNFO) Then
                    Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try


    End Sub

    Private Sub bwLoadInfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadInfo.RunWorkerCompleted

        '//
        ' Thread finished: display it
        '\\

        ' Try
        Me.fillScreenInfo()

        If Not IsNothing(Me.fImage) Then
            Me.pbFanartCache.Image = Me.fImage
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

        If Not IsNothing(Me.pImage) Then
            Me.pbPosterCache.Image = Me.pImage
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

        ' Catch ex As Exception
        '     Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        ' End Try

    End Sub

    Private Sub bwScraper_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwScraper.DoWork

        '//
        ' Thread to handle scraping
        '\\


        Dim tmpImage As Image = Nothing
        Dim Args As Arguments = e.Argument
        Dim TMDB As New TMDB.Scraper
        Dim IMPA As New IMPA.Scraper
        Dim dvView As DataView = Me.dtMedia.DefaultView
        Dim iCount As Integer = 0
        Dim sPath As String = String.Empty
        Dim sPathShort As String = String.Empty
        Dim sOrName As String = String.Empty
        Dim nfoPath As String = String.Empty
        Dim fanartPath As String = String.Empty
        Dim posterPath As String = String.Empty
        Dim fArt As New Media.Fanart

        Try
            If Me.dtMedia.Rows.Count > 0 Then

                Select Case Args.scrapeType
                    Case Master.ScrapeType.FullAsk
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            sPath = drvRow.Item(0).ToString

                            nfoPath = Master.GetNfoPath(sPath, drvRow.Item(7))

                            If File.Exists(nfoPath) Then
                                Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString(), Master.LoadMovieFromNFO(nfoPath), Args.scrapeType)
                            Else
                                Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString(), New Media.Movie, Args.scrapeType)
                            End If

                            If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                If Me.bwScraper.CancellationPending Then Return
                                If Master.uSettings.UseStudioTags Then
                                    If UpdateMediaInfo() Then
                                        Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal.ToString, Master.FITagData(Master.currMovie.FileInfo).ToString)
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.uSettings.UseIMPA OrElse Master.uSettings.UseTMDB Then

                                    posterPath = Master.GetPosterPath(sPath, drvRow.Item(7))

                                    If (Not File.Exists(posterPath)) OrElse Master.uSettings.OverwritePoster Then
                                        tmpImage = Master.GetPreferredImage(Master.ImageType.Posters, Nothing, True)

                                        If Not IsNothing(tmpImage) Then
                                            tmpImage.Save(posterPath)
                                            drvRow.Item(3) = True
                                        Else
                                            If dlgImgSelect.ShowDialog(Master.currMovie.IMDBID, posterPath, sPath, Master.ImageType.Posters) = Windows.Forms.DialogResult.OK Then
                                                drvRow.Item(3) = True
                                            End If
                                            dlgImgSelect = Nothing
                                        End If
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.uSettings.UseTMDB Then

                                    fanartPath = Master.GetFanartPath(sPath, drvRow.Item(7))

                                    If (Not File.Exists(fanartPath)) OrElse Master.uSettings.OverwriteFanart Then
                                        tmpImage = Master.GetPreferredImage(Master.ImageType.Fanart, fArt, True)

                                        If Not IsNothing(tmpImage) Then
                                            tmpImage.Save(fanartPath)
                                            drvRow.Item(4) = True
                                            Master.currMovie.Fanart = fArt
                                        Else
                                            If dlgImgSelect.ShowDialog(Master.currMovie.IMDBID, fanartPath, sPath, Master.ImageType.Fanart) = Windows.Forms.DialogResult.OK Then
                                                drvRow.Item(4) = True
                                                Master.currMovie.Fanart = fArt
                                            End If
                                            dlgImgSelect = Nothing
                                        End If
                                        fArt = Nothing
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                Master.SaveMovieToNFO(Master.currMovie, Master.GetNfoPath(sPath, drvRow.Item(7)))
                                drvRow.Item(5) = True
                            End If
                        Next

                    Case Master.ScrapeType.FullAuto
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            sPath = drvRow.Item(0).ToString

                            nfoPath = Master.GetNfoPath(sPath, drvRow.Item(7))

                            If File.Exists(nfoPath) Then
                                Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString(), Master.LoadMovieFromNFO(nfoPath), Args.scrapeType)
                            Else
                                Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString(), New Media.Movie, Args.scrapeType)
                            End If

                            If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                If Me.bwScraper.CancellationPending Then Return
                                If Master.uSettings.UseStudioTags Then
                                    If UpdateMediaInfo() Then
                                        Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal.ToString, Master.FITagData(Master.currMovie.FileInfo).ToString)
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.uSettings.UseIMPA OrElse Master.uSettings.UseTMDB Then

                                    posterPath = Master.GetPosterPath(sPath, drvRow.Item(7))

                                    If (Not File.Exists(posterPath)) OrElse Master.uSettings.OverwritePoster Then
                                        tmpImage = Master.GetPreferredImage(Master.ImageType.Posters, Nothing)
                                        If Not IsNothing(tmpImage) Then
                                            tmpImage.Save(posterPath)
                                            drvRow.Item(3) = True
                                        End If
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Master.uSettings.UseTMDB Then
                                    fanartPath = Master.GetFanartPath(sPath, drvRow.Item(7))

                                    If (Not File.Exists(fanartPath)) OrElse Master.uSettings.OverwriteFanart Then
                                        tmpImage = Master.GetPreferredImage(Master.ImageType.Fanart, fArt)
                                        If Not IsNothing(tmpImage) Then
                                            tmpImage.Save(fanartPath)
                                            Master.currMovie.Fanart = fArt
                                            drvRow.Item(4) = True
                                        End If
                                        fArt = Nothing
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                Master.SaveMovieToNFO(Master.currMovie, Master.GetNfoPath(sPath, drvRow.Item(7)))
                                drvRow.Item(5) = True
                            End If
                        Next
                    Case Master.ScrapeType.MIOnly
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            Master.currPath = drvRow.Item(0).ToString

                            nfoPath = Master.GetNfoPath(Master.currPath, drvRow.Item(7))

                            If File.Exists(nfoPath) Then
                                Master.currMovie = Master.LoadMovieFromNFO(nfoPath)
                            Else
                                Master.currMovie = New Media.Movie
                            End If

                            If Master.uSettings.UseStudioTags = True Then
                                If UpdateMediaInfo() Then
                                    Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal.ToString, Master.FITagData(Master.currMovie.FileInfo).ToString)
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            Master.SaveMovieToNFO(Master.currMovie, nfoPath)
                        Next

                    Case Master.ScrapeType.CleanFolders
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                            iCount += 1

                            sPath = drvRow.Item(0).ToString
                            sOrName = Master.GetNameFromPath(sPath).ToString
                            sPathShort = Directory.GetParent(sPath).FullName.ToString

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.uSettings.CleanFolderJPG Then
                                If File.Exists(sPathShort & "\folder.jpg") Then
                                    File.Delete(sPathShort & "\folder.jpg")
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.uSettings.CleanFanartJPG Then
                                If File.Exists(sPathShort & "\fanart.jpg") Then
                                    File.Delete(sPathShort & "\fanart.jpg")
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.uSettings.CleanMovieTBN Then
                                If File.Exists(sPathShort & "\movie.tbn") Then
                                    File.Delete(sPathShort & "\movie.tbn")
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.uSettings.CleanMovieNFO Then
                                If File.Exists(sPathShort & "\movie.nfo") Then
                                    File.Delete(sPathShort & "\movie.nfo")
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.uSettings.CleanMovieTBNB Then
                                If File.Exists(Master.RemoveExtFromPath(sPath) & ".tbn") Then
                                    File.Delete(Master.RemoveExtFromPath(sPath) & ".tbn")
                                End If
                                If File.Exists(String.Format("{0}\{1}.tbn", sPathShort, sOrName)) Then
                                    File.Delete(String.Format("{0}\{1}.tbn", sPathShort, sOrName))
                                End If
                                If File.Exists(String.Format("{0}\{1}.tbn", sPathShort, Master.CleanStackingMarkers(sOrName))) Then
                                    File.Delete(String.Format("{0}\{1}.tbn", sPathShort, Master.CleanStackingMarkers(sOrName)))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.uSettings.CleanMovieFanartJPG Then
                                If File.Exists(Master.RemoveExtFromPath(sPath) & "-fanart.jpg") Then
                                    File.Delete(Master.RemoveExtFromPath(sPath) & "-fanart.jpg")
                                End If
                                If File.Exists(String.Format("{0}\{1}-fanart.jpg", sPathShort, sOrName)) Then
                                    File.Delete(String.Format("{0}\{1}-fanart.jpg", sPathShort, sOrName))
                                End If
                                If File.Exists(String.Format("{0}\{1}-fanart.jpg", sPathShort, Master.CleanStackingMarkers(sOrName))) Then
                                    File.Delete(String.Format("{0}\{1}-fanart.jpg", sPathShort, Master.CleanStackingMarkers(sOrName)))
                                End If
                            End If

                            If Me.bwScraper.CancellationPending Then Return
                            If Master.uSettings.CleanMovieNFOB Then
                                If File.Exists(Master.RemoveExtFromPath(sPath) & ".nfo") Then
                                    File.Delete(Master.RemoveExtFromPath(sPath) & ".nfo")
                                End If
                                If File.Exists(String.Format("{0}\{1}.nfo", sPathShort, sOrName)) Then
                                    File.Delete(String.Format("{0}\{1}.nfo", sPathShort, sOrName))
                                End If
                                If File.Exists(String.Format("{0}\{1}.nfo", sPathShort, Master.CleanStackingMarkers(sOrName))) Then
                                    File.Delete(String.Format("{0}\{1}.nfo", sPathShort, Master.CleanStackingMarkers(sOrName)))
                                End If
                            End If
                        Next

                    Case Master.ScrapeType.UpdateAuto
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            If Not drvRow.Item(3) OrElse Not drvRow.Item(4) OrElse Not drvRow.Item(5) Then
                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(1).ToString)
                                iCount += 1
                                sPath = drvRow.Item(0).ToString

                                nfoPath = Master.GetNfoPath(sPath, drvRow.Item(7))
                                Master.currMovie = Master.LoadMovieFromNFO(nfoPath)

                                If Not drvRow.Item(5) Then
                                    Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString, New Media.Movie, Args.scrapeType)

                                    If Master.uSettings.UseStudioTags Then
                                        If UpdateMediaInfo() Then
                                            Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal.ToString, Master.FITagData(Master.currMovie.FileInfo).ToString)
                                        End If
                                    End If

                                    Master.SaveMovieToNFO(Master.currMovie, nfoPath)
                                    drvRow.Item(5) = True
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(3) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.uSettings.UseIMPA OrElse Master.uSettings.UseTMDB Then
                                        posterPath = Master.GetPosterPath(sPath, drvRow.Item(7))

                                        If (Not File.Exists(posterPath)) OrElse Master.uSettings.OverwritePoster Then
                                            tmpImage = Master.GetPreferredImage(Master.ImageType.Posters, Nothing)

                                            If Not IsNothing(tmpImage) Then
                                                tmpImage.Save(posterPath)
                                                drvRow.Item(3) = True
                                            End If
                                        End If
                                    End If
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(4) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.uSettings.UseTMDB Then
                                        fanartPath = Master.GetFanartPath(sPath, drvRow.Item(7))
                                        If (Not File.Exists(fanartPath)) OrElse Master.uSettings.OverwriteFanart Then
                                            tmpImage = Master.GetPreferredImage(Master.ImageType.Fanart, fArt)

                                            If Not IsNothing(tmpImage) Then
                                                tmpImage.Save(fanartPath)
                                                drvRow.Item(4) = True
                                                If File.Exists(nfoPath) Then
                                                    'need to load movie from nfo here in case the movie already had
                                                    'an nfo.... currmovie would not be set to the proper movie
                                                    Master.currMovie = Master.LoadMovieFromNFO(nfoPath)
                                                    Master.currMovie.Fanart = fArt
                                                    Master.SaveMovieToNFO(Master.currMovie, nfoPath)
                                                End If
                                                fArt = Nothing
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    Case Master.ScrapeType.UpdateAsk
                        For Each drvRow As DataRowView In dvView
                            If Me.bwScraper.CancellationPending Then Return
                            If Not drvRow.Item(3) OrElse Not drvRow.Item(4) OrElse Not drvRow.Item(5) Then
                                Me.bwScraper.ReportProgress(iCount, drvRow.Item(2).ToString)
                                iCount += 1

                                sPath = drvRow.Item(0).ToString

                                nfoPath = Master.GetNfoPath(sPath, drvRow.Item(7))
                                Master.currMovie = Master.LoadMovieFromNFO(nfoPath)

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(5) Then

                                    Master.currMovie = IMDB.GetSearchMovieInfo(drvRow.Item(1).ToString, New Media.Movie, Args.scrapeType)

                                    If Master.uSettings.UseStudioTags Then
                                        If UpdateMediaInfo() Then
                                            Master.currMovie.Studio = String.Concat(Master.currMovie.StudioReal.ToString, Master.FITagData(Master.currMovie.FileInfo).ToString)
                                        End If
                                    End If

                                    Master.SaveMovieToNFO(Master.currMovie, nfoPath)
                                    drvRow.Item(5) = True
                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(3) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.uSettings.UseIMPA OrElse Master.uSettings.UseTMDB Then
                                        posterPath = Master.GetPosterPath(sPath, drvRow.Item(7))

                                        If (Not File.Exists(posterPath)) OrElse Master.uSettings.OverwritePoster Then
                                            tmpImage = Master.GetPreferredImage(Master.ImageType.Posters, Nothing, True)

                                            If Not IsNothing(tmpImage) Then
                                                tmpImage.Save(posterPath)
                                                drvRow.Item(3) = True
                                            Else
                                                If dlgImgSelect.ShowDialog(Master.currMovie.IMDBID, posterPath, sPath, Master.ImageType.Posters) = Windows.Forms.DialogResult.OK Then
                                                    drvRow.Item(3) = True
                                                End If
                                                dlgImgSelect = Nothing
                                            End If
                                        End If
                                    End If

                                End If

                                If Me.bwScraper.CancellationPending Then Return
                                If Not drvRow.Item(4) AndAlso Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                                    If Master.uSettings.UseTMDB Then
                                        fanartPath = Master.GetFanartPath(sPath, drvRow.Item(7))

                                        If (Not File.Exists(fanartPath)) OrElse Master.uSettings.OverwriteFanart Then
                                            tmpImage = Master.GetPreferredImage(Master.ImageType.Fanart, fArt, True)

                                            If Not IsNothing(tmpImage) Then
                                                tmpImage.Save(fanartPath)
                                                drvRow.Item(4) = True
                                                Master.currMovie.Fanart = fArt
                                            Else
                                                If dlgImgSelect.ShowDialog(Master.currMovie.IMDBID, fanartPath, sPath, Master.ImageType.Fanart) = Windows.Forms.DialogResult.OK Then
                                                    drvRow.Item(4) = True

                                                    If File.Exists(nfoPath) Then
                                                        'need to load movie from nfo here in case the movie already had
                                                        'an nfo.... currmovie would not be set to the proper movie
                                                        Master.currMovie = Master.LoadMovieFromNFO(nfoPath)
                                                        Master.currMovie.Fanart = fArt
                                                        Master.SaveMovieToNFO(Master.currMovie, nfoPath)
                                                    End If
                                                    fArt = Nothing
                                                End If
                                                dlgImgSelect = Nothing
                                            End If
                                            fArt = Nothing
                                        End If
                                    End If
                                End If

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
                    Me.dgvMediaList.Update()
                    Application.DoEvents()

                    Me.dgvMediaList.Rows(0).Selected = True
                    Me.dgvMediaList.Rows(0).Visible = True
                    Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Rows(0).Cells(1)
                    'set tmpTitle to title in list - used for searching IMDB
                    Me.tmpTitle = Me.dgvMediaList.Item(1, 0).Value.ToString

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
                .pnlTop.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pnlInfoIcons.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pnlRating.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbVideo.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbResolution.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbAudio.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbChannels.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbStudio.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbStar1.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbStar2.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbStar3.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbStar4.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .pbStar5.BackColor = Color.FromArgb(Master.uSettings.TopPanelColor)
                .lblTitle.ForeColor = Color.FromArgb(Master.uSettings.TopPanelTextColor)
                .lblVotes.ForeColor = Color.FromArgb(Master.uSettings.TopPanelTextColor)
                .lblRuntime.ForeColor = Color.FromArgb(Master.uSettings.TopPanelTextColor)
                .lblTagline.ForeColor = Color.FromArgb(Master.uSettings.TopPanelTextColor)

                'background
                .scMain.Panel2.BackColor = Color.FromArgb(Master.uSettings.BackgroundColor)
                .pbFanart.BackColor = Color.FromArgb(Master.uSettings.BackgroundColor)

                'info panel
                .pnlInfoPanel.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .txtMediaInfo.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .txtMediaInfo.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)
                .txtPlot.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .txtPlot.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)
                .txtOutline.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .txtOutline.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)
                .pnlActors.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .lstActors.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .lstActors.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)
                .lblDirector.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .lblDirector.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)
                .lblReleaseDate.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .lblReleaseDate.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)
                .pnlTop250.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .lblTop250.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .lblTop250.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)

                .lblMIHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblMIHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblPlotHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblPlotHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblOutlineHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblOutlineHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblActorsHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblActorsHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblFilePathHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblFilePathHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblIMDBHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblIMDBHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblDirectorHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblDirectorHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblReleaseDateHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblReleaseDateHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblCertsHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblCertsHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)
                .lblInfoPanelHeader.BackColor = Color.FromArgb(Master.uSettings.HeaderColor)
                .lblInfoPanelHeader.ForeColor = Color.FromArgb(Master.uSettings.HeaderTextColor)

                'left panel
                .lblMediaCount.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .lblMediaCount.ForeColor = Color.FromArgb(Master.uSettings.PanelTextColor)
                .scMain.Panel1.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
                .pnlSearch.BackColor = Color.FromArgb(Master.uSettings.InfoPanelColor)
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

        Me.pnlMPAA.Top = Me.pnlInfoPanel.Top - 49

    End Sub

    Public Sub LoadMedia(ByVal mediaType As Integer)

        '//
        ' Begin threads to fill datagrid with media data
        '\\


        Try
            Master.alFolderList.Clear()
            Master.alFileList.Clear()

            Me.dgvMediaList.DataSource = Nothing

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

            'set status bar text to movie path
            Me.tslStatus.Text = sPath

            Master.currPath = sPath
            Master.isFile = isFile

            If doMI Then
                Me.txtMediaInfo.Clear()
                Me.pbMILoading.Visible = True
            End If
            Me.Refresh()

            If Me.bwDownloadPic.IsBusy Then
                Me.bwDownloadPic.CancelAsync()
            End If

            If Me.bwMediaInfo.IsBusy Then
                Me.bwMediaInfo.CancelAsync()
            End If

            If Me.bwLoadInfo.IsBusy Then
                Me.bwLoadInfo.CancelAsync()
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
                    For iDel As Integer = (.pnlGenre.Length - 1) To 0 Step -1
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

                If Not IsNothing(.pImage) Then
                    .pImage = Nothing
                End If
                If Not IsNothing(.fImage) Then
                    .fImage = Nothing
                End If
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
            For Each imdbAct As Media.Person In Master.currMovie.Actors
                If Not String.IsNullOrEmpty(imdbAct.Thumb) Then

                    'Was it XBMC or MIP that set some of the actor thumbs to
                    'the default "Add Pic" image??
                    If Not Strings.InStr(imdbAct.Thumb.ToString.ToLower, "addtiny.gif") > 0 Then
                        Me.alActors.Add(imdbAct.Thumb.ToString)
                    Else
                        Me.alActors.Add("none")
                    End If
                Else
                    Me.alActors.Add("none")
                End If

                Me.lstActors.Items.Add(String.Format("{0} as {1}", imdbAct.Name.ToString, imdbAct.Role.ToString))
            Next

            If Not String.IsNullOrEmpty(Master.currMovie.MPAA) Then
                Me.createMPAABox(Master.currMovie.MPAA)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Rating) AndAlso IsNumeric(Master.currMovie.Rating) AndAlso CDbl(Master.currMovie.Rating) > 0 Then
                Me.BuildStars(CDbl(Master.currMovie.Rating))
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

            If Master.uSettings.UseStudioTags = True Then
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

                If dblRating >= 0.5 Then ' if rating is less than .5 out of ten, consider it a 0
                    Select Case (dblRating / 2)
                        Case Is <= 0.5
                            .pbStar1.Image = My.Resources.starhalf
                        Case Is <= 1
                            .pbStar1.Image = My.Resources.star
                        Case Is <= 1.5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.starhalf
                        Case Is <= 2
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                        Case Is <= 2.5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.starhalf
                        Case Is <= 3
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                        Case Is <= 3.5
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.starhalf
                        Case Is <= 4
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                        Case Is <= 4.5
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

    Private Sub createMPAABox(ByVal strRating As String)

        '//
        ' Parse the floating MPAA box
        '\\

        Try
            Me.MoveMPAA()

            If Strings.InStr(strRating.ToLower, "rated pg-13") > 0 Then
                Me.pbMPAA.Image = My.Resources.mpaapg13
                Me.pnlMPAA.Visible = True
            ElseIf Strings.InStr(strRating.ToLower, "rated pg") > 0 Then
                Me.pbMPAA.Image = My.Resources.mpaapg
                Me.pnlMPAA.Visible = True
            ElseIf Strings.InStr(strRating.ToLower, "rated r") > 0 Then
                Me.pbMPAA.Image = My.Resources.mpaar
                Me.pnlMPAA.Visible = True
            ElseIf Strings.InStr(strRating.ToLower, "rated nc-17") > 0 Then
                Me.pbMPAA.Image = My.Resources.mpaanc17
                Me.pnlMPAA.Visible = True
            ElseIf Strings.InStr(strRating.ToLower, "rated g") > 0 Then
                Me.pbMPAA.Image = My.Resources.mpaag
                Me.pnlMPAA.Visible = True
            Else
                If Not IsNothing(Me.pbMPAA.Image) Then
                    Me.pnlMPAA.Visible = False
                    Me.pbMPAA.Image.Dispose()
                    Me.pbMPAA.Image = Nothing
                End If
            End If
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
                    Me.tslLoading.Text = "Auto-Pilot (Full - Ask)"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                    End If
                Case Master.ScrapeType.FullAuto
                    Me.tspbLoading.Maximum = Me.dgvMediaList.RowCount
                    Me.tslLoading.Text = "Auto-Pilot (Full - Auto)"
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
                        chkCount += If(Not Me.dgvMediaList.Item(3, i).Value OrElse Not Me.dgvMediaList.Item(4, i).Value OrElse Not Me.dgvMediaList.Item(5, i).Value, 1, 0)
                    Next

                    Me.tspbLoading.Maximum = chkCount
                    Me.tslLoading.Text = "Auto-Pilot (Update - Ask)"
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
                        chkCount += If(Not Me.dgvMediaList.Item(3, i).Value OrElse Not Me.dgvMediaList.Item(4, i).Value OrElse Not Me.dgvMediaList.Item(5, i).Value, 1, 0)
                    Next

                    Me.tspbLoading.Maximum = chkCount
                    Me.tslLoading.Text = "Auto-Pilot (Update - Auto)"
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
                    Me.tslLoading.Text = "Auto-Pilot (Media Info Only)"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType})
                    End If
                Case Master.ScrapeType.CleanFolders
                    Me.tspbLoading.Maximum = Me.dgvMediaList.RowCount
                    Me.tslLoading.Text = "Cleaning Folders"
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
                    Me.tslLoading.Text = "Scraping"
                    Me.tspbLoading.Maximum = 13
                    Me.ReportDownloadPercent = True
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True


                    If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) AndAlso doSearch = False Then
                        IMDB.GetMovieInfoAsync(Master.currMovie.IMDBID, Master.currMovie, Master.uSettings.FullCrew, Master.uSettings.FullCast)
                    Else
                        Master.tmpMovie = New Media.Movie
                        If dlgIMDBSearchResults.ShowDialog(Me.tmpTitle) = Windows.Forms.DialogResult.OK Then
                            If Not String.IsNullOrEmpty(Master.tmpMovie.IMDBID) Then
                                Me.ClearInfo(True)
                                Me.tslStatus.Text = String.Format("Scraping {0}", Master.tmpMovie.Title)
                                Me.tslLoading.Text = "Scraping"
                                Me.tspbLoading.Maximum = 13
                                Me.tspbLoading.Style = ProgressBarStyle.Continuous
                                Me.ReportDownloadPercent = True
                                Me.tslLoading.Visible = True
                                Me.tspbLoading.Visible = True
                                IMDB.GetMovieInfoAsync(Master.tmpMovie.IMDBID.ToString, Master.currMovie, Master.uSettings.FullCrew, Master.uSettings.FullCast)
                            End If
                        Else
                            Me.tslLoading.Visible = False
                            Me.tspbLoading.Visible = False
                            Me.tslStatus.Text = String.Empty
                        End If
                        Me.tsbAutoPilot.Enabled = True
                        Me.tsbRefreshMedia.Enabled = True
                        Me.tsbEdit.Enabled = True
                        Me.tsbRescrape.Enabled = True
                        Me.tabsMain.Enabled = True
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

            If Not Master.GetExtFromPath(Master.currPath) = ".rar" Then
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
                If Master.uSettings.UseStudioTags Then
                    Me.tslLoading.Text = "Scanning Media Info:"
                    Me.tspbLoading.Value = Me.tspbLoading.Maximum
                    Me.tspbLoading.Style = ProgressBarStyle.Marquee
                    Me.tspbLoading.MarqueeAnimationSpeed = 100
                    Me.Refresh()
                    If Me.UpdateMediaInfo() Then
                        Master.currMovie.Studio = String.Format("{0}{1}", Master.currMovie.StudioReal.ToString, Master.FITagData(Master.currMovie.FileInfo).ToString)
                    End If
                End If
                dlgEditMovie.ShowDialog()

                Me.tslLoading.Visible = False
                Me.tspbLoading.Visible = False
                Me.LoadInfo(Master.currPath, True, False, Master.isFile)
            Else
                MsgBox("Unable to retrieve movie details from the internet. Please check your connection and try again.", MsgBoxStyle.Exclamation, "Error Retrieving Details")
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.tslStatus.Text = String.Empty
        End Try

    End Sub

    Private Sub ReCheckItems(ByVal iIndex As Integer)
        Dim sPath As String = Me.dgvMediaList.Item(0, iIndex).Value.ToString
        Dim aResults(4) As Boolean
        Try
            Dim tmpName As String = Master.CleanStackingMarkers(Master.GetNameFromPath(sPath))
            Dim nPath As String = String.Concat(Directory.GetParent(sPath).FullName, "\", tmpName)
            Dim di As New DirectoryInfo(Directory.GetParent(sPath).FullName)
            Dim lFi As New List(Of FileInfo)()

            lFi.AddRange(di.GetFiles())

            For Each sfile As FileInfo In lFi
                Select Case sfile.Extension.ToLower
                    Case ".jpg"
                        If sfile.FullName = String.Concat(Master.RemoveExtFromPath(nPath), "-fanart.jpg") OrElse sfile.FullName = String.Concat(Directory.GetParent(nPath).ToString, "\fanart.jpg") Then
                            Me.dgvMediaList.Item(4, iIndex).Value = True
                        Else
                            Me.dgvMediaList.Item(4, iIndex).Value = False
                        End If
                    Case ".tbn"
                        If sfile.FullName = String.Concat(Master.RemoveExtFromPath(nPath), ".tbn") OrElse sfile.FullName = String.Concat(Directory.GetParent(nPath).ToString, "\movie.tbn") Then
                            Me.dgvMediaList.Item(3, iIndex).Value = True
                        Else
                            Me.dgvMediaList.Item(3, iIndex).Value = False
                        End If
                    Case ".nfo"
                        If sfile.FullName = String.Concat(Master.RemoveExtFromPath(nPath), ".nfo") OrElse sfile.FullName = String.Concat(Directory.GetParent(nPath).ToString, "\movie.nfo") Then
                            Me.dgvMediaList.Item(5, iIndex).Value = True
                        Else
                            Me.dgvMediaList.Item(5, iIndex).Value = False
                        End If
                    Case ".avi", ".divx", ".mkv", ".iso", ".mpg", ".mp4", ".wmv", ".wma", ".mov", ".mts", ".m2t", ".img", ".dat", ".bin", ".cue", ".vob", ".dvb", ".evo", ".asf", ".asx", ".avs", ".nsv", ".ram", ".ogg", ".ogm", ".ogv", ".flv", ".swf", ".nut", ".viv", ".rar"
                        If sfile.Name.Contains("-trailer") Then
                            Me.dgvMediaList.Item(6, iIndex).Value = True
                        Else
                            Me.dgvMediaList.Item(6, iIndex).Value = False
                        End If
                End Select

            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
#End Region
End Class