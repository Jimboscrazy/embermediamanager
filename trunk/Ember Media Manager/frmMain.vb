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
    Friend WithEvents bwValidateNfo As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bsMedia As New BindingSource

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
    Private FilterArray As New ArrayList
    Private SingelScrapeDone As Boolean = False
    Private isCL As Boolean = False

    Private Enum PicType As Integer
        Actor = 0
        Poster = 1
        Fanart = 2
    End Enum

    Private Structure Results
        Dim scrapeType As Master.ScrapeType
        Dim scrapeMod As ScrapeModifier
        Dim fileInfo As String
        Dim setEnabled As Boolean
        Dim ResultType As PicType
        Dim Movie As Media.Movie
        Dim Path As String
        Dim Result As Image
    End Structure

    Private Structure Arguments
        Dim setEnabled As Boolean
        Dim scrapeType As Master.ScrapeType
        Dim scrapeMod As ScrapeModifier
        Dim pType As PicType
        Dim pURL As String
        Dim Path As String
        Dim Movie As Media.Movie
        Dim isFile As Boolean
    End Structure

    Private Enum ScrapeModifier As Integer
        All = 0
        NFO = 1
        Poster = 2
        Fanart = 3
        Extra = 4
    End Enum

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
        ' Do some stuff before closing
        '\\

        Try
            Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision)
            Master.eSettings.WindowLoc = Me.Location
            Master.eSettings.WindowSize = Me.Size
            Master.eSettings.WindowState = Me.WindowState
            Master.eSettings.Save()

            If Not Me.bwPrelim.IsBusy AndAlso Not Me.bwFolderData.IsBusy AndAlso Not isCL Then
                Me.SaveMovieList()
            End If

            If Me.bwFolderData.IsBusy Then Me.bwFolderData.CancelAsync()
            If Me.bwMediaInfo.IsBusy Then Me.bwMediaInfo.CancelAsync()
            If Me.bwLoadInfo.IsBusy Then Me.bwLoadInfo.CancelAsync()
            If Me.bwDownloadPic.IsBusy Then Me.bwDownloadPic.CancelAsync()
            If Me.bwPrelim.IsBusy Then Me.bwPrelim.CancelAsync()
            If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()
            If Me.bwValidateNfo.IsBusy Then Me.bwValidateNfo.CancelAsync()

            Do While Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy OrElse Me.bwValidateNfo.IsBusy
                btnCancel.Visible = False
                lblCanceling.Visible = True
                pbCanceling.Visible = True
                pnlCancel.Visible = True
                Application.DoEvents()
            Loop

            If Not isCL Then Master.SQLcn.Close()

            If Not Master.eSettings.PersistImgCache Then
                If Directory.Exists(Master.TempPath) Then
                    Directory.Delete(Master.TempPath, True)
                End If
            End If
        Catch
            'force close
            Application.Exit()
        End Try

    End Sub

    Private Sub frmMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        Try
            If Me.dgvMediaList.Columns.Count > 0 Then
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()
                Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width / 2) - (Me.pbFanart.Width / 2)
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width / 2) - (Me.pnlNoInfo.Width / 2), (Me.scMain.Panel2.Height / 2) - (Me.pnlNoInfo.Height / 2))
                Me.pnlCancel.Location = New Point((Me.scMain.Panel2.Width / 2) - (Me.pnlNoInfo.Width / 2), 100)
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
            Using dSettings As New dlgSettings
                Dim dResult As Windows.Forms.DialogResult = dSettings.ShowDialog
                If dResult = Windows.Forms.DialogResult.OK OrElse dResult = Windows.Forms.DialogResult.Retry Then

                    Me.SetColors()

                    If Me.dgvMediaList.RowCount > 0 Then
                        Me.dgvMediaList.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                        Me.dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieFanartCol
                        Me.dgvMediaList.Columns(6).Visible = Not Master.eSettings.MovieInfoCol
                        Me.dgvMediaList.Columns(7).Visible = Not Master.eSettings.MovieTrailerCol
                        Me.dgvMediaList.Columns(8).Visible = Not Master.eSettings.MovieSubCol
                        Me.dgvMediaList.Columns(9).Visible = Not Master.eSettings.MovieExtraCol

                        'Trick to autosize the first column, but still allow resizing by user
                        Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                        Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                    End If

                    If dResult = Windows.Forms.DialogResult.Retry Then
                        Me.LoadMedia(1)
                    Else
                        If Not bwPrelim.IsBusy AndAlso Not bwFolderData.IsBusy Then
                            Me.FillList(0)
                        End If
                    End If



                End If



            End Using

            Me.SetMenus()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '//
        ' Add our handlers, load settings, set form colors, and try to load movies at startup
        '\\

        Me.Visible = False

        'setup some dummies so we don't get exceptions when resizing form/info panel
        ReDim Preserve Me.pnlGenre(0)
        ReDim Preserve Me.pbGenre(0)
        Me.pnlGenre(0) = New Panel()
        Me.pbGenre(0) = New PictureBox()

        AddHandler IMDB.MovieInfoDownloaded, AddressOf MovieInfoDownloaded
        AddHandler IMDB.ProgressUpdated, AddressOf MovieInfoDownloadedPercent

        Dim sPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Log", Path.DirectorySeparatorChar, "errlog.txt")
        If File.Exists(sPath) Then
            Master.MoveFileWithStream(sPath, sPath.Insert(sPath.LastIndexOf("."), "-old"))
            File.Delete(sPath)
        End If

        If Not Directory.Exists(Master.TempPath) Then
            Directory.CreateDirectory(Master.TempPath)
        End If

        Master.eSettings.Load()

        Dim MoviePath As String = String.Empty
        Dim isFile As Boolean = False
        Dim hasSpec As Boolean = True
        Dim Args() As String = Environment.GetCommandLineArgs
        If Args.Count = 3 Then

            isCL = True

            Select Case Args(1)
                Case "-file"
                    isFile = True
                    hasSpec = True
                Case "-folder"
                    isFile = False
                    hasSpec = True
            End Select

            If File.Exists(Args(2).Replace("""", String.Empty)) Then
                MoviePath = Args(2).Replace("""", String.Empty)
            End If
            Try
                If Not String.IsNullOrEmpty(MoviePath) AndAlso hasSpec Then
                    Master.currPath = MoviePath
                    Master.isFile = isFile
                    Master.currNFO = Master.GetNfoPath(MoviePath, isFile)
                    Master.currMovie = If(Not String.IsNullOrEmpty(Master.currNFO), Master.LoadMovieFromNFO(Master.currNFO), New Media.Movie)
                    Me.tmpTitle = Master.FilterName(If(isFile, Path.GetFileNameWithoutExtension(MoviePath), Directory.GetParent(MoviePath).Name))
                    Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing)
                    While Not Me.SingelScrapeDone
                        Application.DoEvents()
                    End While
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Me.Close()
        Else
            Try
                Me.btnMarkAll.Text = If(Master.eSettings.MarkAll, "Mark All", "Unmark All")

                If Master.eSettings.CheckUpdates Then
                    Dim tmpNew As Integer = Master.CheckUpdate
                    If tmpNew > Convert.ToInt32(My.Application.Info.Version.Revision) Then
                        Using dNewVer As New dlgNewVersion
                            dNewVer.ShowDialog(tmpNew)
                        End Using
                    End If
                End If

                Me.Location = Master.eSettings.WindowLoc
                Me.Size = Master.eSettings.WindowSize
                Me.WindowState = Master.eSettings.WindowState

                Me.SetColors()

                Me.SetMenus()

                Me.pnlInfoPanel.Height = 25
                Me.ClearInfo()

                If Master.eSettings.Version = String.Format("r{0}", My.Application.Info.Version.Revision) Then
                    Master.ConnectDB(False)
                    Me.FillList(0)
                Else
                    If dlgWizard.ShowDialog = Windows.Forms.DialogResult.OK Then
                        Master.ConnectDB(True)
                        Me.LoadMedia(1)
                    Else
                        Master.ConnectDB(True)
                        Me.FillList(0)
                    End If
                End If


            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Me.Visible = True
            Me.Activate()
        End If

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

    Private Sub tsbRefreshMedia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbRefreshMedia.Click

        '//
        ' Reload media type when "Rescan Media" is clicked
        '\\

        Me.LoadMedia(1)

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

            'move focus somewhere to stop highlighting some info boxes
            Me.txtSearch.Focus()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnMIRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMIRefresh.Click

        '//
        ' Refresh Media Info
        '\\

        Me.LoadInfo(Master.currPath, False, True, Master.isFile, True)

    End Sub
    Private Sub dgvMediaList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellDoubleClick

        '//
        ' Show the NFO Editor
        '\\

        Try

            If Me.bwFolderData.IsBusy OrElse Me.bwMediaInfo.IsBusy OrElse Me.bwLoadInfo.IsBusy OrElse _
            Me.bwDownloadPic.IsBusy OrElse Me.bwPrelim.IsBusy OrElse Me.bwScraper.IsBusy OrElse _
            Me.bwValidateNfo.IsBusy Then Return

            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Me.dgvMediaList.Rows(indX).Cells(0).Value
            Master.currPath = Me.dgvMediaList.Rows(indX).Cells(1).Value
            Master.isFile = Me.dgvMediaList.Rows(indX).Cells(2).Value
            Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
            Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)
            Me.tslStatus.Text = Master.currPath
            Me.tmpTitle = Me.dgvMediaList.Rows(indX).Cells(3).Value

            Using dEditMovie As New dlgEditMovie

                Select Case dEditMovie.ShowDialog(ID)
                    Case Windows.Forms.DialogResult.OK
                        Me.ReCheckItems(ID)
                        Me.SetListItemAfterEdit(ID, indX)
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, True)
                End Select

            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_Sorted(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvMediaList.Sorted

        '//
        ' Select first item in the media list after sort
        '\\

        If Me.dgvMediaList.RowCount > 0 Then
            Me.dgvMediaList.Rows(0).Selected = True
            Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Rows(0).Cells(3)
            Me.SetFilterColors()
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
        Using dAbout As New dlgAbout
            dAbout.ShowDialog()
        End Using

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
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                Me.dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            End If

            If Me.Created Then
                Me.MoveMPAA()
                Me.MoveGenres()

                Master.ResizePB(Me.pbFanart, Me.pbFanartCache, Me.scMain.Panel2.Height - 90, Me.scMain.Panel2.Width)
                Me.pbFanart.Left = (Me.scMain.Panel2.Width / 2) - (Me.pbFanart.Width / 2)
                Me.pnlNoInfo.Location = New Point((Me.scMain.Panel2.Width / 2) - (Me.pnlNoInfo.Width / 2), (Me.scMain.Panel2.Height / 2) - (Me.pnlNoInfo.Height / 2))
                Me.pnlCancel.Location = New Point((Me.scMain.Panel2.Width / 2) - (Me.pnlNoInfo.Width / 2), 100)
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

    Private Sub dgvMediaList_CellPainting(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvMediaList.CellPainting

        '//
        ' Add icons to media list column headers
        '\\

        Try
            If e.ColumnIndex >= 4 AndAlso e.ColumnIndex <= 9 AndAlso e.RowIndex = -1 Then
                e.PaintBackground(e.ClipBounds, False)

                Dim pt As Point = e.CellBounds.Location
                Dim offset As Integer = (e.CellBounds.Width - Me.ilColumnIcons.ImageSize.Width) / 2

                pt.X += offset
                pt.Y = 1
                Me.ilColumnIcons.Draw(e.Graphics, pt, e.ColumnIndex - 4)

                e.Handled = True

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub MediaTagsOnlyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MediaTagsOnlyToolStripMenuItem.Click

        '//
        ' Scrape all movies in list for MediaInfo only
        '\\

        Me.ScrapeData(Master.ScrapeType.MIOnly, Nothing)

    End Sub

    Private Sub dgvMediaList_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvMediaList.CellEnter

        '//
        ' Load media information for the selected item
        '\\

        Try
            Me.currRow = e.RowIndex
            Me.tmrWait.Enabled = False
            Me.tmrLoad.Enabled = False
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
            Me.tmrLoad.Enabled = False
        End If
    End Sub

    Private Sub tmrLoad_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrLoad.Tick
        Me.tmrWait.Enabled = False
        Me.tmrLoad.Enabled = False
        Try
            If Me.dgvMediaList.SelectedRows.Count > 0 Then
                'set tmpTitle to title in list - used for searching IMDB
                Me.tmpTitle = Me.dgvMediaList.Item(3, Me.currRow).Value.ToString
                If Not Me.dgvMediaList.Item(4, Me.currRow).Value AndAlso Not Me.dgvMediaList.Item(5, Me.currRow).Value AndAlso Not Me.dgvMediaList.Item(6, Me.currRow).Value Then
                    Me.ClearInfo()
                    Me.pnlNoInfo.Visible = True
                    Master.currPath = Me.dgvMediaList.Item(1, Me.currRow).Value.ToString
                    Master.isFile = Me.dgvMediaList.Item(2, Me.currRow).Value.ToString
                    Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
                    Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)
                    Me.tslStatus.Text = Master.currPath
                    Me.mnuMediaList.Enabled = True
                Else
                    Me.pnlNoInfo.Visible = False

                    If Me.bwLoadInfo.IsBusy Then
                        Me.bwLoadInfo.CancelAsync()
                        Application.DoEvents()
                    End If
                    'try to load the info from the NFO
                    Me.LoadInfo(Me.dgvMediaList.Item(1, Me.currRow).Value.ToString, True, False, Me.dgvMediaList.Item(2, Me.currRow).Value)
                End If
            End If
        Catch
        End Try
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
                bsMedia.Filter = "title LIKE '%" & txtSearch.Text & "%'"
            Else
                bsMedia.RemoveFilter()
            End If
        Catch
        End Try
        Me.tmrSearch.Enabled = False
    End Sub
    Private Sub tsbUpdateXBMC_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUpdateXBMC.ButtonClick
        Try
            For Each xCom As emmSettings.XBMCCom In Master.eSettings.XBMCComs
                Me.DoXCom(xCom)
            Next
        Catch
        End Try
    End Sub

    Private Sub CleanFoldersToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CleanFoldersToolStripMenuItem.Click
        '//
        ' Clean all items in folders that match user selected types
        '\\

        Dim sWarning As String = String.Empty
        With Master.eSettings
            If .CleanDotFanartJPG Then sWarning += String.Concat("<movie>.fanart.jpg", vbNewLine)
            If .CleanFanartJPG Then sWarning += String.Concat("<movie>.jpg", vbNewLine)
            If .CleanFolderJPG Then sWarning += String.Concat("folder.jpg", vbNewLine)
            If .CleanMovieFanartJPG Then sWarning += String.Concat("<movie>-fanart.jpg", vbNewLine)
            If .CleanMovieJPG Then sWarning += String.Concat("movie.jpg", vbNewLine)
            If .CleanMovieNameJPG Then sWarning += String.Concat("<movie>.jpg", vbNewLine)
            If .CleanMovieNFO Then sWarning += String.Concat("movie.nfo", vbNewLine)
            If .CleanMovieNFOB Then sWarning += String.Concat("<movie>.nfo", vbNewLine)
            If .CleanMovieTBN Then sWarning += String.Concat("movie.tbn", vbNewLine)
            If .CleanMovieTBNB Then sWarning += String.Concat("<movie>.tbn", vbNewLine)
            If .CleanPosterJPG Then sWarning += String.Concat("poster.jpg", vbNewLine)
            If .CleanPosterTBN Then sWarning += String.Concat("poster.tbn", vbNewLine)
            If .CleanExtraThumbs Then sWarning += String.Concat("/extrathumbs/", vbNewLine)
        End With
        If MsgBox(String.Concat("WARNING: If you continue, all files of the following types will be permanently deleted:", vbNewLine, vbNewLine, sWarning, vbNewLine, "Are you sure you want to continue?"), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then
            Me.ScrapeData(Master.ScrapeType.CleanFolders, Nothing)
        End If
    End Sub

    Private Sub ConvertFileSourceToFolderSourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertFileSourceToFolderSourceToolStripMenuItem.Click

        '//
        ' Convert a file source into a folder source by separating everything into separate folders
        '\\

        If MsgBox(String.Concat("WARNING: If you continue, all files from file-type sources will be sorted into separate folders.", vbNewLine, vbNewLine, "Are you sure you want to continue?"), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then

            Dim dirArray() As String
            Dim alMedia As New ArrayList
            Dim hasFileSource As Boolean = False

            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.mnuMediaList.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tspbLoading.Style = ProgressBarStyle.Marquee
            Me.tslLoading.Text = "Sorting Files:"
            Me.tslLoading.Visible = True
            Me.tspbLoading.Visible = True

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
                Me.tsbAutoPilot.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                Me.mnuMediaList.Enabled = True
                Me.tabsMain.Enabled = True
                Me.tspbLoading.Style = ProgressBarStyle.Marquee
                Me.tslLoading.Text = "Sorting Files:"
                Me.tslLoading.Visible = False
                Me.tspbLoading.Visible = False
            End If
        End If
    End Sub

    Private Sub chkFilterNew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterNew.CheckedChanged
        Try
            Me.chkFilterDupe.Enabled = Not Me.chkFilterNew.Checked
            If Me.chkFilterNew.Checked Then
                Me.FilterArray.Add("new = 1")
                Me.chkFilterDupe.Checked = False
            Else
                Me.FilterArray.Remove("new = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub chkFilterMark_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterMark.CheckedChanged
        Try
            Me.chkFilterDupe.Enabled = Not Me.chkFilterMark.Checked
            If Me.chkFilterMark.Checked Then
                Me.FilterArray.Add("mark = 1")
                Me.chkFilterDupe.Checked = False
            Else
                Me.FilterArray.Remove("mark = 1")
            End If
            Me.RunFilter()
        Catch
        End Try
    End Sub

    Private Sub rbFilterAnd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFilterAnd.CheckedChanged
        If Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked Then Me.RunFilter()
    End Sub

    Private Sub rbFilterOr_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbFilterOr.CheckedChanged
        If Me.chkFilterMark.Checked OrElse Me.chkFilterNew.Checked Then Me.RunFilter()
    End Sub

    Private Sub chkFilterDupe_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkFilterDupe.CheckedChanged
        Me.chkFilterMark.Enabled = Not chkFilterDupe.Checked
        Me.chkFilterNew.Enabled = Not chkFilterDupe.Checked
        Me.rbFilterAnd.Enabled = Not chkFilterDupe.Checked
        Me.rbFilterOr.Enabled = Not chkFilterDupe.Checked
        If Me.chkFilterDupe.Checked Then
            Me.chkFilterMark.Checked = False
            Me.chkFilterNew.Checked = False
        End If
        Me.RunFilter()
    End Sub

    Private Sub dgvMediaList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dgvMediaList.KeyPress

        For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
            If drvRow.Cells(3).Value.ToString.ToLower.StartsWith(e.KeyChar.ToString.ToLower) Then
                drvRow.Selected = True
                Me.dgvMediaList.CurrentCell = drvRow.Cells(3)
                Exit For
            End If
        Next

    End Sub

    Private Sub cmnuMark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuMark.Click
        Dim indX = From selX As DataRow In dtMedia.Rows Where selX.Item(0) = Me.dgvMediaList.SelectedRows(0).Cells(0).Value
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Boolean, 0, "id")
                SQLcommand.CommandText = "UPDATE movies SET mark = (?) WHERE id = (?);"
                parMark.Value = If(cmnuMark.Text = "Unmark", False, True)
                parID.Value = indX(0).Item(0)
                SQLcommand.ExecuteNonQuery()
            End Using
            SQLtransaction.Commit()
        End Using
        indX(0).Item(11) = If(cmnuMark.Text = "Unmark", False, True)
        Me.SetFilterColors()
    End Sub

    Private Sub cmnuLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuLock.Click
        Dim indX = From selX As DataRow In dtMedia.Rows Where selX.Item(0) = Me.dgvMediaList.SelectedRows(0).Cells(0).Value
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")
                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Boolean, 0, "id")
                SQLcommand.CommandText = "UPDATE movies SET lock = (?) WHERE id = (?);"
                parLock.Value = If(cmnuLock.Text = "Unlock", False, True)
                parID.Value = indX(0).Item(0)
                SQLcommand.ExecuteNonQuery()
            End Using
            SQLtransaction.Commit()
        End Using
        indX(0).Item(14) = If(cmnuLock.Text = "Unlock", False, True)
        Me.SetFilterColors()
    End Sub

    Private Sub cmnuRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuRescrape.Click

        '//
        ' Begin the process to scrape IMDB with the current ID
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing)
    End Sub

    Private Sub cmnuSearchNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuSearchNew.Click

        '//
        ' Begin the process to search IMDB for data
        '\\

        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, True)
    End Sub

    Private Sub cmnuEditMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmnuEditMovie.Click

        '//
        ' Show the NFO Editor
        '\\

        Try
            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Dim ID As Integer = Me.dgvMediaList.Item(0, indX).Value

            Using dEditMovie As New dlgEditMovie
                Select Case dEditMovie.ShowDialog(ID)
                    Case Windows.Forms.DialogResult.OK
                        Me.ReCheckItems(ID)
                        Me.SetListItemAfterEdit(ID, indX)
                    Case Windows.Forms.DialogResult.Retry
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing)
                    Case Windows.Forms.DialogResult.Abort
                        Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, True)
                End Select
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dgvMediaList_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvMediaList.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim dgvHTI As DataGridView.HitTestInfo = sender.HitTest(e.X, e.Y)
            If dgvHTI.Type = DataGridViewHitTestType.Cell Then
                cmnuTitle.Text = String.Concat(">> ", Me.dgvMediaList.Item(3, dgvHTI.RowIndex).Value, " <<")
                If Not Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected Then
                    Me.mnuMediaList.Enabled = False
                    Me.dgvMediaList.ClearSelection()
                    Me.dgvMediaList.Rows(dgvHTI.RowIndex).Selected = True
                    Me.dgvMediaList.CurrentCell = Me.dgvMediaList.Item(3, dgvHTI.RowIndex)
                End If
                cmnuMark.Text = If(Me.dgvMediaList.Item(11, dgvHTI.RowIndex).Value, "Unmark", "Mark")
                cmnuLock.Text = If(Me.dgvMediaList.Item(14, dgvHTI.RowIndex).Value, "Unlock", "Lock")
            End If
        End If
    End Sub

    Private Sub mnuAllAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, ScrapeModifier.All)
    End Sub

    Private Sub mnuAllAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, ScrapeModifier.NFO)

    End Sub

    Private Sub mnuAllAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, ScrapeModifier.Poster)

    End Sub

    Private Sub mnuAllAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, ScrapeModifier.Fanart)

    End Sub

    Private Sub mnuAllAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.FullAuto, ScrapeModifier.Extra)
    End Sub

    Private Sub mnuAllAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskAll.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, ScrapeModifier.All)

    End Sub

    Private Sub mnuAllAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, ScrapeModifier.NFO)
    End Sub

    Private Sub mnuAllAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, ScrapeModifier.Poster)
    End Sub

    Private Sub mnuAllAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, ScrapeModifier.Fanart)
    End Sub

    Private Sub mnuAllAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAllAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.FullAsk, ScrapeModifier.Extra)
    End Sub

    Private Sub mnuMissAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, ScrapeModifier.All)

    End Sub

    Private Sub mnuMissAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, ScrapeModifier.NFO)

    End Sub

    Private Sub mnuMissAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, ScrapeModifier.Poster)

    End Sub

    Private Sub mnuMissAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, ScrapeModifier.Fanart)

    End Sub

    Private Sub mnuMissAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAuto, ScrapeModifier.Extra)

    End Sub

    Private Sub mnuMissAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskAll.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, ScrapeModifier.All)
    End Sub

    Private Sub mnuMissAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, ScrapeModifier.NFO)

    End Sub

    Private Sub mnuMissAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, ScrapeModifier.Poster)

    End Sub

    Private Sub mnuMissAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, ScrapeModifier.Fanart)

    End Sub

    Private Sub mnuMissAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMissAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.UpdateAsk, ScrapeModifier.Extra)

    End Sub

    Private Sub mnuNewAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, ScrapeModifier.All)

    End Sub

    Private Sub mnuNewAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, ScrapeModifier.NFO)

    End Sub

    Private Sub mnuNewAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, ScrapeModifier.Poster)

    End Sub

    Private Sub mnuNewAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, ScrapeModifier.Fanart)

    End Sub

    Private Sub mnuNewAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.NewAuto, ScrapeModifier.Extra)

    End Sub

    Private Sub mnuNewAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskAll.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, ScrapeModifier.All)

    End Sub

    Private Sub mnuNewAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, ScrapeModifier.NFO)

    End Sub

    Private Sub mnuNewAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, ScrapeModifier.Poster)

    End Sub

    Private Sub mnuNewAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, ScrapeModifier.Fanart)

    End Sub

    Private Sub mnuNewAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuNewAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.NewAsk, ScrapeModifier.Extra)

    End Sub

    Private Sub mnuMarkAutoAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoAll.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, ScrapeModifier.All)

    End Sub

    Private Sub mnuMarkAutoNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoNfo.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, ScrapeModifier.NFO)

    End Sub

    Private Sub mnuMarkAutoPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoPoster.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, ScrapeModifier.Poster)

    End Sub

    Private Sub mnuMarkAutoFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoFanart.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, ScrapeModifier.Fanart)

    End Sub

    Private Sub mnuMarkAutoExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAutoExtra.Click

        Me.ScrapeData(Master.ScrapeType.MarkAuto, ScrapeModifier.Extra)

    End Sub

    Private Sub mnuMarkAskAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskAll.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, ScrapeModifier.All)

    End Sub

    Private Sub mnuMarkAskNfo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskNfo.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, ScrapeModifier.NFO)

    End Sub

    Private Sub mnuMarkAskPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskPoster.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, ScrapeModifier.Poster)

    End Sub

    Private Sub mnuMarkAskFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskFanart.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, ScrapeModifier.Fanart)

    End Sub

    Private Sub mnuMarkAskExtra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuMarkAskExtra.Click

        Me.ScrapeData(Master.ScrapeType.MarkAsk, ScrapeModifier.Extra)

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        btnCancel.Visible = False
        lblCanceling.Visible = True
        pbCanceling.Visible = True

        If Me.bwScraper.IsBusy Then Me.bwScraper.CancelAsync()
        If Me.bwValidateNfo.IsBusy Then Me.bwValidateNfo.CancelAsync()

        Do While Me.bwScraper.IsBusy OrElse Me.bwValidateNfo.IsBusy
            Application.DoEvents()
        Loop
    End Sub

    Private Sub OpenContainingFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenContainingFolderToolStripMenuItem.Click
        Process.Start("explorer.exe", Directory.GetParent(Me.dgvMediaList.SelectedRows(0).Cells(1).Value).FullName)
    End Sub

    Private Sub DeleteMovieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteMovieToolStripMenuItem.Click
        If MsgBox(String.Concat("WARNING: THIS WILL PERMANENTLY DELETE THE SELECTED MOVIE FROM THE HARD DRIVE", vbNewLine, vbNewLine, "Are you sure you want to continue?"), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Are You Sure?") = MsgBoxResult.Yes Then
            Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
            Master.DeleteFiles(False, Me.dgvMediaList.Rows(indX).Cells(1).Value, Me.dgvMediaList.Rows(indX).Cells(2).Value)
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                    SQLcommand.CommandText = String.Concat("DELETE FROM movies WHERE id = ", Me.dgvMediaList.Rows(indX).Cells(0).Value, ";")
                    SQLcommand.ExecuteNonQuery()
                End Using
                SQLtransaction.Commit()
            End Using
            Me.FillList(indX)
        End If
    End Sub

    Private Sub CopyExistingFanartToBackdropsFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyExistingFanartToBackdropsFolderToolStripMenuItem.Click
        '//
        ' Copy all existing fanart to the backdrops folder
        '\\

        Me.ScrapeData(Master.ScrapeType.CopyBD, Nothing)
    End Sub

    Private Sub btnMarkAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMarkAll.Click
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                SQLcommand.CommandText = "UPDATE movies SET mark = (?);"
                parMark.Value = If(btnMarkAll.Text = "Unmark All", False, True)
                SQLcommand.ExecuteNonQuery()
            End Using
            SQLtransaction.Commit()
        End Using
        For Each drvRow As DataRow In dtMedia.Rows
            drvRow.Item(11) = If(btnMarkAll.Text = "Unmark All", False, True)
        Next
        Me.SetFilterColors()
        btnMarkAll.Text = If(btnMarkAll.Text = "Unmark All", "Mark All", "Unmark All")
        Master.eSettings.MarkAll = If(btnMarkAll.Text = "Unmark All", False, True)
    End Sub

    Private Sub ExportMoviesListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportMoviesListToolStripMenuItem.Click
        Using dExportMovies As New dlgExportMovies
            dExportMovies.ShowDialog()
        End Using
    End Sub

    Private Sub SetsManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetsManagerToolStripMenuItem.Click
        Using dSetsManager As New dlgSetsManager
            dSetsManager.ShowDialog()
        End Using
    End Sub

    Private Sub ClearAllCachesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearAllCachesToolStripMenuItem.Click
        If Directory.Exists(Master.TempPath) Then
            Directory.Delete(Master.TempPath, True)
        End If

        Directory.CreateDirectory(Master.TempPath)
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

        'remove files from the db that have been deleted from the hd
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                SQLcommand.CommandText = "DELETE FROM movies WHERE id = (?);"
                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                If Me.dtMedia.Rows.Count > 0 Then
                    For Each drvRow As DataRow In Me.dtMedia.Rows
                        If Not File.Exists(drvRow.Item(1)) Then
                            parID.Value = drvRow.Item(0)
                            SQLcommand.ExecuteNonQuery()
                        End If
                    Next
                End If
            End Using
            SQLtransaction.Commit()
        End Using

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
            For Each movieSource As String In alMedia

                dirArray = Split(movieSource, "|")
                If dirArray(1).ToString = "Folders" Then
                    Master.EnumerateDirectory(dirArray(0).ToString)
                Else
                    Master.EnumerateFiles(dirArray(0).ToString)
                End If

                If Me.bwPrelim.CancellationPending Then
                    e.Cancel = True
                    Return
                End If
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
        If Not e.Cancelled Then
            Try
                If Master.MediaList.Count = 0 Then
                    Me.tslStatus.Text = "Unable to load directories. Please check settings."
                    Me.tspbLoading.Visible = False
                    Me.tslLoading.Visible = False
                    Me.tabsMain.Enabled = True
                    Me.tsbRefreshMedia.Enabled = True
                    Me.tsbAutoPilot.Enabled = False
                    Me.mnuMediaList.Enabled = False

                Else
                    Me.tslLoading.Text = "Loading Media:"
                    Me.tspbLoading.Style = ProgressBarStyle.Continuous
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True
                    Me.tspbLoading.Maximum = (Master.MediaList.Count + 1)


                    Me.bwFolderData = New System.ComponentModel.BackgroundWorker
                    Me.bwFolderData.WorkerReportsProgress = True
                    Me.bwFolderData.WorkerSupportsCancellation = True
                    Me.bwFolderData.RunWorkerAsync()
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Sub

    Private Sub bwFolderData_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwFolderData.DoWork

        '//
        ' Thread to fill a datatable with basic media data
        '\\

        Dim currentIndex As Integer = 0
        Dim cleanName As String = String.Empty
        Dim mName As String = String.Empty
        Dim mIMDB As String = String.Empty
        Dim aContents(6) As Boolean
        Dim tmpMovie As New Media.Movie

        Try
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                    SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO movies (path, type, title, poster, fanart, info, trailer, sub, extra, new, mark, source, imdb, lock) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?);")
                    Dim parPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPath", DbType.String, 0, "path")
                    Dim parType As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parType", DbType.Boolean, 0, "type")
                    Dim parTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTitle", DbType.String, 0, "title")
                    Dim parPoster As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPoster", DbType.Boolean, 0, "poster")
                    Dim parFanart As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFanart", DbType.Boolean, 0, "fanart")
                    Dim parInfo As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parInfo", DbType.Boolean, 0, "info")
                    Dim parTrailer As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTrailer", DbType.Boolean, 0, "trailer")
                    Dim parSub As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSub", DbType.Boolean, 0, "sub")
                    Dim parExtra As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parExtra", DbType.Boolean, 0, "extra")
                    Dim parNew As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNew", DbType.Boolean, 0, "new")
                    Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                    Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                    Dim parIMDB As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parIMDB", DbType.String, 0, "imdb")
                    Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")

                    'process the folder type media
                    For Each sFile As Master.FileAndSource In Master.MediaList
                        If Me.bwFolderData.CancellationPending Then
                            e.Cancel = True
                            Return
                        End If

                        If Not String.IsNullOrEmpty(sFile.Filename) Then
                            If Master.eSettings.UseNameFromNfo Then
                                tmpMovie = Master.LoadMovieFromNFO(Master.GetNfoPath(sFile.Filename, sFile.isFile))
                                mName = tmpMovie.Title
                                mIMDB = tmpMovie.IMDBID
                                tmpMovie = Nothing
                                If String.IsNullOrEmpty(mName) Then
                                    If Master.eSettings.UseFolderName AndAlso Not sFile.isFile Then
                                        If Directory.GetParent(sFile.Filename).Name.ToLower = "video_ts" Then
                                            mName = Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name
                                        Else
                                            mName = Directory.GetParent(sFile.Filename).Name
                                        End If
                                    Else
                                        mName = Path.GetFileNameWithoutExtension(sFile.Filename)
                                    End If
                                End If
                            Else
                                If Master.eSettings.UseFolderName AndAlso Not sFile.isFile Then
                                    If Directory.GetParent(sFile.Filename).Name.ToLower = "video_ts" Then
                                        mName = Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name
                                    Else
                                        mName = Directory.GetParent(sFile.Filename).Name
                                    End If
                                Else
                                    mName = Path.GetFileNameWithoutExtension(sFile.Filename)
                                End If
                            End If

                            cleanName = Master.FilterName(mName)

                            Me.bwFolderData.ReportProgress(currentIndex, cleanName)

                            If Not String.IsNullOrEmpty(cleanName) Then

                                aContents = Master.GetFolderContents(sFile.Filename, sFile.isFile)

                                parPath.Value = sFile.Filename
                                parType.Value = sFile.isFile
                                parTitle.Value = cleanName
                                parPoster.Value = aContents(0)
                                parFanart.Value = aContents(1)
                                parInfo.Value = aContents(2)
                                parTrailer.Value = aContents(3)
                                parSub.Value = aContents(4)
                                parExtra.Value = aContents(5)
                                Using SQLNewcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                                    SQLNewcommand.CommandText = String.Concat("SELECT id FROM movies WHERE path = """, sFile.Filename, """;")
                                    Dim SQLreader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                                    If SQLreader.HasRows Then
                                        parNew.Value = False
                                    Else
                                        parNew.Value = True
                                    End If
                                End Using
                                If parNew.Value Then
                                    parLock.Value = False
                                    If Master.eSettings.MarkNew Then
                                        parMark.Value = True
                                    Else
                                        parMark.Value = False
                                    End If
                                Else
                                    Using SQLNewcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                                        SQLNewcommand.CommandText = String.Concat("SELECT mark, lock FROM movies WHERE path = """, sFile.Filename, """;")
                                        Dim SQLreader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                                        parMark.Value = SQLreader("mark")
                                        parLock.Value = SQLreader("lock")
                                    End Using
                                End If
                                parSource.Value = sFile.Source
                                parIMDB.Value = mIMDB
                                SQLcommand.ExecuteNonQuery()

                                aContents = Nothing
                                mName = String.Empty
                                mIMDB = String.Empty
                                currentIndex += 1
                            End If
                        End If
                    Next
                End Using
                SQLtransaction.Commit()
            End Using

            tmpMovie = Nothing
            aContents = Nothing

            If Me.bwFolderData.CancellationPending Then
                e.Cancel = True
                Return
            End If

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

        If Not e.Cancelled Then
            Me.FillList(0)
        End If

    End Sub

    Private Sub bwMediaInfo_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwMediaInfo.DoWork

        '//
        ' Thread to procure technical and tag information about media via MediaInfo.dll
        '\\

        Try
            Dim Args As Arguments = e.Argument
            If Me.UpdateMediaInfo(Args.Path, Args.Movie) Then
                If Master.eSettings.UseStudioTags = True Then
                    Args.Movie.Studio = String.Concat(Args.Movie.StudioReal, Master.FITagData(Args.Movie.FileInfo))
                End If
                Master.SaveMovieToNFO(Args.Movie, Args.Path, Args.isFile)

                If Me.bwMediaInfo.CancellationPending Then
                    e.Cancel = True
                    Return
                End If

                e.Result = New Results With {.fileinfo = Master.FIToString(Args.Movie.FileInfo, Args.Movie.Studio), .setEnabled = Args.setEnabled, .Path = Args.Path, .Movie = Args.Movie}

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

        If Not e.Cancelled Then
            Dim Res As Results = e.Result

            Try
                If Not Res.fileInfo = "error" Then
                    Me.pbMILoading.Visible = False
                    Me.txtMediaInfo.Text = Res.fileInfo
                    If Master.eSettings.UseStudioTags Then
                        If Not String.IsNullOrEmpty(Res.Movie.Studio) Then
                            Master.GetAVImages(Res.Movie.Studio, Res.Path)
                            Me.pnlInfoIcons.Width = 346
                            Me.pbStudio.Left = 277
                        Else
                            Master.GetAVImages("", Res.Path)
                        End If
                    Else
                        Me.pnlInfoIcons.Width = 70
                        Me.pbStudio.Left = 0
                    End If
                    If Master.eSettings.UseMIDuration Then
                        If Not String.IsNullOrEmpty(Master.currMovie.Runtime) Then
                            Me.lblRuntime.Text = String.Format("Runtime: {0}", Master.currMovie.Runtime)
                        End If
                    End If
                    Me.btnMIRefresh.Focus()
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            If Res.setEnabled Then
                Me.tabsMain.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                If Me.dgvMediaList.RowCount > 0 Then
                    Me.tsbAutoPilot.Enabled = True
                    Me.mnuMediaList.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub bwDownloadPic_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownloadPic.DoWork

        '//
        ' Thread to download image from the internet (multi-threaded because sometimes
        ' the web server is slow to respond or not reachable, hanging the GUI)
        '\\

        Dim Args As Arguments = e.Argument
        Dim tImage As Image = Nothing

        Dim wrRequest As WebRequest = WebRequest.Create(Args.pURL)
        wrRequest.Timeout = 10000
        Using wrResponse As WebResponse = wrRequest.GetResponse()
            tImage = Image.FromStream(wrResponse.GetResponseStream())
        End Using
        e.Result = New Results With {.ResultType = Args.pType, .Result = tImage}

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

            If Me.bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If
            Me.MainFanart.Load(Master.currPath, Master.isFile, Master.ImageType.Fanart)

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If
            Me.MainPoster.Load(Master.currPath, Master.isFile, Master.ImageType.Posters)

            'wait for mediainfo to update the nfo
            Do While bwMediaInfo.IsBusy
                Application.DoEvents()
            Loop

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
            End If
            'read nfo if it's there
            Master.currNFO = Master.GetNfoPath(Master.currPath, Master.isFile)
            Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)

            If bwLoadInfo.CancellationPending Then
                e.Cancel = True
                Return
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
            If Not e.Cancelled Then
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
                    Me.mnuMediaList.Enabled = True
                    Me.tabsMain.Enabled = True
                    Me.EnableFilters(True)
                End If
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
        Dim iCount As Integer = 0
        Dim sPath As String = String.Empty
        Dim nfoPath As String = String.Empty
        Dim fArt As New Media.Fanart
        Dim pThumbs As New Media.Poster
        Dim Poster As New Images
        Dim Fanart As New Images
        Dim tmpMovie As New Media.Movie

        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                SQLcommand.CommandText = "UPDATE movies SET poster = (?), fanart = (?), info = (?) WHERE ID = (?);"
                Dim parPoster As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPoster", DbType.Boolean, 0, "poster")
                Dim parFanart As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFanart", DbType.Boolean, 0, "fanart")
                Dim parInfo As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parInfo", DbType.Boolean, 0, "info")
                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")

                Try
                    If Me.dtMedia.Rows.Count > 0 Then
                        Select Case Args.scrapeType
                            Case Master.ScrapeType.FullAsk, Master.ScrapeType.NewAsk, Master.ScrapeType.MarkAsk
                                For Each drvRow As DataRow In Me.dtMedia.Rows

                                    If Args.scrapeType = Master.ScrapeType.NewAsk Then
                                        If Not drvRow.Item(10) Then Continue For
                                    ElseIf Args.scrapeType = Master.ScrapeType.MarkAsk Then
                                        If Not drvRow.Item(11) Then Continue For
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel

                                    Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                    iCount += 1

                                    If drvRow.Item(14) Then Continue For

                                    sPath = drvRow.Item(1).ToString

                                    nfoPath = Master.GetNfoPath(sPath, drvRow.Item(2))

                                    parID.Value = drvRow.Item(0)
                                    parPoster.Value = drvRow.Item(4)
                                    parFanart.Value = drvRow.Item(5)
                                    parInfo.Value = drvRow.Item(6)

                                    tmpMovie = Master.LoadMovieFromNFO(nfoPath)
                                    If Not String.IsNullOrEmpty(tmpMovie.IMDBID) Then
                                        IMDB.GetMovieInfo(tmpMovie.IMDBID, tmpMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                                        Master.scrapeMovie = tmpMovie
                                    Else
                                        Master.scrapeMovie = IMDB.GetSearchMovieInfo(drvRow.Item(3).ToString, tmpMovie, Args.scrapeType)
                                    End If
                                    tmpMovie = Nothing

                                    If Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) Then
                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Master.eSettings.UseStudioTags AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.NFO) Then
                                            If UpdateMediaInfo(sPath, Master.scrapeMovie) Then
                                                Master.scrapeMovie.Studio = String.Concat(Master.scrapeMovie.StudioReal, Master.FITagData(Master.scrapeMovie.FileInfo))
                                            End If
                                        End If

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If (Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Poster) Then

                                            If Poster.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Posters) Then
                                                If Poster.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs, True) Then
                                                    If Not IsNothing(Poster.Image) Then
                                                        Poster.SaveAsPoster(sPath, drvRow.Item(2))
                                                        parPoster.Value = True
                                                        Master.scrapeMovie.Thumbs = pThumbs
                                                    Else
                                                        MsgBox("A poster of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                        Dim dImgSelect As New dlgImgSelect
                                                        If dImgSelect.ShowDialog(Master.scrapeMovie.IMDBID, sPath, Master.ImageType.Posters) = Windows.Forms.DialogResult.OK Then
                                                            parPoster.Value = True
                                                            Master.scrapeMovie.Thumbs = pThumbs
                                                        End If
                                                        dImgSelect = Nothing
                                                    End If
                                                End If
                                            End If
                                        End If
                                        pThumbs = Nothing

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Master.eSettings.UseTMDB AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Fanart) Then

                                            If Fanart.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Fanart) Then
                                                If Fanart.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Fanart, fArt, Nothing, True) Then

                                                    If Not IsNothing(Fanart.Image) Then
                                                        Fanart.SaveAsFanart(sPath, drvRow.Item(2))
                                                        parFanart.Value = True
                                                        Master.scrapeMovie.Fanart = fArt
                                                    Else
                                                        MsgBox("Fanart of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")

                                                        Using dImgSelect As New dlgImgSelect
                                                            If dImgSelect.ShowDialog(Master.scrapeMovie.IMDBID, sPath, Master.ImageType.Fanart) = Windows.Forms.DialogResult.OK Then
                                                                parFanart.Value = True
                                                                Master.scrapeMovie.Fanart = fArt
                                                            End If
                                                        End Using
                                                    End If
                                                End If
                                            End If
                                        End If
                                        fArt = Nothing

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.NFO) Then
                                            Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                            parInfo.Value = True
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Extra) Then
                                        If Master.eSettings.AutoThumbs > 0 AndAlso Not drvRow.Item(2) Then
                                            If Master.CreateRandomThumbs(sPath, Master.eSettings.AutoThumbs) Then parFanart.Value = True
                                        End If
                                    End If
                                    SQLcommand.ExecuteNonQuery()
                                Next

                            Case Master.ScrapeType.FullAuto, Master.ScrapeType.NewAuto, Master.ScrapeType.MarkAuto
                                For Each drvRow As DataRow In Me.dtMedia.Rows

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel

                                    If Args.scrapeType = Master.ScrapeType.NewAuto Then
                                        If Not drvRow.Item(10) Then Continue For
                                    ElseIf Args.scrapeType = Master.ScrapeType.MarkAuto Then
                                        If Not drvRow.Item(11) Then Continue For
                                    End If

                                    Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                    iCount += 1

                                    If drvRow.Item(14) Then Continue For

                                    sPath = drvRow.Item(1).ToString

                                    nfoPath = Master.GetNfoPath(sPath, drvRow.Item(2))

                                    parID.Value = drvRow.Item(0)
                                    parPoster.Value = drvRow.Item(4)
                                    parFanart.Value = drvRow.Item(5)
                                    parInfo.Value = drvRow.Item(6)

                                    tmpMovie = Master.LoadMovieFromNFO(nfoPath)
                                    If Not String.IsNullOrEmpty(tmpMovie.IMDBID) Then
                                        IMDB.GetMovieInfo(tmpMovie.IMDBID, tmpMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False)
                                        Master.scrapeMovie = tmpMovie
                                    Else
                                        Master.scrapeMovie = IMDB.GetSearchMovieInfo(drvRow.Item(3).ToString, tmpMovie, Args.scrapeType)
                                    End If
                                    tmpMovie = Nothing

                                    If Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) Then
                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Master.eSettings.UseStudioTags AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.NFO) Then
                                            If UpdateMediaInfo(sPath, Master.scrapeMovie) Then
                                                Master.scrapeMovie.Studio = String.Concat(Master.scrapeMovie.StudioReal, Master.FITagData(Master.scrapeMovie.FileInfo))
                                            End If
                                        End If

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If (Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Poster) Then

                                            If Poster.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Posters) Then
                                                If Poster.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs) Then
                                                    If Not IsNothing(Poster.Image) Then
                                                        Poster.SaveAsPoster(sPath, drvRow.Item(2))
                                                        Master.scrapeMovie.Thumbs = pThumbs
                                                        parPoster.Value = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                        pThumbs = Nothing

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Master.eSettings.UseTMDB AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Fanart) Then
                                            If Fanart.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Fanart) Then
                                                If Fanart.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Fanart, fArt, Nothing) Then
                                                    If Not IsNothing(Fanart.Image) Then
                                                        Fanart.SaveAsFanart(sPath, drvRow.Item(2))
                                                        Master.scrapeMovie.Fanart = fArt
                                                        parFanart.Value = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                        fArt = Nothing

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.NFO) Then
                                            Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                            parInfo.Value = True
                                        End If
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Extra) Then
                                        If Master.eSettings.AutoThumbs > 0 AndAlso Not drvRow.Item(2) Then
                                            If Master.CreateRandomThumbs(sPath, Master.eSettings.AutoThumbs) Then parFanart.Value = True
                                        End If
                                    End If

                                    SQLcommand.ExecuteNonQuery()
                                Next
                            Case Master.ScrapeType.MIOnly
                                For Each drvRow As DataRow In Me.dtMedia.Rows
                                    If Me.bwScraper.CancellationPending Then GoTo doCancel

                                    Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                    iCount += 1

                                    If drvRow.Item(14) Then Continue For

                                    sPath = drvRow.Item(1).ToString

                                    nfoPath = Master.GetNfoPath(sPath, drvRow.Item(2))

                                    Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)

                                    If UpdateMediaInfo(sPath, Master.scrapeMovie) Then
                                        Master.scrapeMovie.Studio = String.Concat(Master.scrapeMovie.StudioReal, Master.FITagData(Master.scrapeMovie.FileInfo))
                                    End If

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    Master.SaveMovieToNFO(Master.scrapeMovie, drvRow.Item(1).ToString, drvRow.Item(2))
                                Next

                            Case Master.ScrapeType.CleanFolders
                                For Each drvRow As DataRow In Me.dtMedia.Rows

                                    Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                    iCount += 1

                                    If drvRow.Item(14) Then Continue For

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    Master.DeleteFiles(True, drvRow.Item(1).ToString, drvRow.Item(2))
                                Next
                            Case Master.ScrapeType.CopyBD
                                For Each drvRow As DataRow In Me.dtMedia.Rows

                                    Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                    iCount += 1

                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    sPath = Fanart.GetFanartPath(drvRow.Item(1).ToString, drvRow.Item(2))
                                    If Not String.IsNullOrEmpty(sPath) Then
                                        If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                                            If Master.eSettings.VideoTSParent Then
                                                File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), "-fanart.jpg")), True)
                                            Else
                                                If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                                    File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")), True)
                                                Else
                                                    File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)), True)
                                                End If
                                            End If
                                        Else
                                            If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                                                File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(drvRow.Item(1).ToString), "-fanart.jpg")), True)
                                            Else
                                                File.Copy(sPath, Path.Combine(Master.eSettings.BDPath, Path.GetFileName(sPath)), True)
                                            End If
                                        End If
                                    End If
                                Next
                            Case Master.ScrapeType.UpdateAuto
                                For Each drvRow As DataRow In Me.dtMedia.Rows

                                    Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                    iCount += 1

                                    If drvRow.Item(14) Then Continue For

                                    parID.Value = drvRow.Item(0)
                                    parPoster.Value = drvRow.Item(4)
                                    parFanart.Value = drvRow.Item(5)
                                    parInfo.Value = drvRow.Item(6)
                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Not drvRow.Item(4) AndAlso Args.scrapeMod = ScrapeModifier.Poster) OrElse (Not drvRow.Item(5) AndAlso Args.scrapeMod = ScrapeModifier.Fanart) OrElse (Not drvRow.Item(6) AndAlso Args.scrapeMod = ScrapeModifier.NFO) OrElse _
                                    ((Not drvRow.Item(4) OrElse Not drvRow.Item(5) OrElse Not drvRow.Item(6)) AndAlso Args.scrapeMod = ScrapeModifier.All) Then

                                        sPath = drvRow.Item(1).ToString

                                        nfoPath = Master.GetNfoPath(sPath, drvRow.Item(2))
                                        Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)

                                        If String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) OrElse Not IMDB.GetMovieInfo(Master.scrapeMovie.IMDBID, Master.scrapeMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False) Then
                                            Master.scrapeMovie = IMDB.GetSearchMovieInfo(drvRow.Item(3).ToString, New Media.Movie, Args.scrapeType)
                                        End If
                                        If Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) Then
                                            If Not drvRow.Item(6) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.NFO) Then
                                                If Master.eSettings.UseStudioTags Then
                                                    If UpdateMediaInfo(sPath, Master.scrapeMovie) Then
                                                        Master.scrapeMovie.Studio = String.Concat(Master.scrapeMovie.StudioReal, Master.FITagData(Master.scrapeMovie.FileInfo))
                                                    End If
                                                End If
                                                Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(6))
                                                parInfo.Value = True
                                            End If
                                        End If

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Not drvRow.Item(4) AndAlso Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Poster) Then
                                            If Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB Then
                                                If Poster.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Posters) Then
                                                    If Poster.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs) Then
                                                        If Not IsNothing(Poster.Image) Then
                                                            Poster.SaveAsPoster(sPath, drvRow.Item(2))
                                                            parPoster.Value = True
                                                            If File.Exists(nfoPath) AndAlso Args.scrapeMod = ScrapeModifier.All Then
                                                                'need to load movie from nfo here in case the movie already had
                                                                'an nfo.... scrapeMovie would not be set to the proper movie
                                                                Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)
                                                                Master.scrapeMovie.Thumbs = pThumbs
                                                                Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Not drvRow.Item(5) AndAlso Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Fanart) Then
                                            If Master.eSettings.UseTMDB Then
                                                If Fanart.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Fanart) Then
                                                    If Fanart.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Fanart, fArt, Nothing) Then
                                                        If Not IsNothing(Fanart.Image) Then
                                                            Fanart.SaveAsFanart(sPath, drvRow.Item(2))
                                                            parFanart.Value = True
                                                            If File.Exists(nfoPath) AndAlso Args.scrapeMod = ScrapeModifier.All Then
                                                                'need to load movie from nfo here in case the movie already had
                                                                'an nfo.... scrapeMovie would not be set to the proper movie
                                                                Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)
                                                                Master.scrapeMovie.Fanart = fArt
                                                                Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                        fArt = Nothing

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Master.eSettings.AutoThumbs > 0 AndAlso Not drvRow.Item(2) AndAlso Not Directory.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "extrathumbs")) AndAlso _
                                        (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Extra) Then
                                            If Master.CreateRandomThumbs(sPath, Master.eSettings.AutoThumbs) Then parFanart.Value = True
                                        End If
                                    End If

                                    SQLcommand.ExecuteNonQuery()
                                Next
                            Case Master.ScrapeType.UpdateAsk
                                For Each drvRow As DataRow In Me.dtMedia.Rows
                                    Me.bwScraper.ReportProgress(iCount, drvRow.Item(3).ToString)
                                    iCount += 1

                                    If drvRow.Item(14) Then Continue For

                                    parID.Value = drvRow.Item(0)
                                    parPoster.Value = drvRow.Item(4)
                                    parFanart.Value = drvRow.Item(5)
                                    parInfo.Value = drvRow.Item(6)
                                    If Me.bwScraper.CancellationPending Then GoTo doCancel
                                    If (Not drvRow.Item(4) AndAlso Args.scrapeMod = ScrapeModifier.Poster) OrElse (Not drvRow.Item(5) AndAlso Args.scrapeMod = ScrapeModifier.Fanart) OrElse (Not drvRow.Item(6) AndAlso Args.scrapeMod = ScrapeModifier.NFO) OrElse _
                                    ((Not drvRow.Item(4) OrElse Not drvRow.Item(5) OrElse Not drvRow.Item(6)) AndAlso Args.scrapeMod = ScrapeModifier.All) Then

                                        sPath = drvRow.Item(1).ToString

                                        nfoPath = Master.GetNfoPath(sPath, drvRow.Item(2))
                                        Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) OrElse Not IMDB.GetMovieInfo(Master.scrapeMovie.IMDBID, Master.scrapeMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast, False) Then
                                            Master.scrapeMovie = IMDB.GetSearchMovieInfo(drvRow.Item(3).ToString, New Media.Movie, Args.scrapeType)
                                        End If
                                        If Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) Then
                                            If Not drvRow.Item(6) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.NFO) Then
                                                If Master.eSettings.UseStudioTags Then
                                                    If UpdateMediaInfo(sPath, Master.scrapeMovie) Then
                                                        Master.scrapeMovie.Studio = String.Concat(Master.scrapeMovie.StudioReal, Master.FITagData(Master.scrapeMovie.FileInfo))
                                                    End If
                                                End If

                                                Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                                parInfo.Value = True
                                            End If
                                        End If

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Not drvRow.Item(4) AndAlso Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Poster) Then
                                            If Master.eSettings.UseIMPA OrElse Master.eSettings.UseTMDB OrElse Master.eSettings.UseMPDB Then

                                                If Poster.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Posters) Then
                                                    If Poster.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Posters, Nothing, pThumbs, True) Then
                                                        If Not IsNothing(Poster.Image) Then
                                                            Poster.SaveAsPoster(sPath, drvRow.Item(2))
                                                            parPoster.Value = True
                                                            If File.Exists(nfoPath) AndAlso Args.scrapeMod = ScrapeModifier.All Then
                                                                'need to load movie from nfo here in case the movie already had
                                                                'an nfo.... scrapeMovie would not be set to the proper movie
                                                                Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)
                                                                Master.scrapeMovie.Thumbs = pThumbs
                                                                Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                                            End If
                                                        Else
                                                            MsgBox("A poster of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                            Using dImgSelect As New dlgImgSelect
                                                                If dImgSelect.ShowDialog(Master.scrapeMovie.IMDBID, sPath, Master.ImageType.Posters) = Windows.Forms.DialogResult.OK Then
                                                                    parPoster.Value = True
                                                                    If File.Exists(nfoPath) AndAlso Args.scrapeMod = ScrapeModifier.All Then
                                                                        'need to load movie from nfo here in case the movie already had
                                                                        'an nfo.... scrapeMovie would not be set to the proper movie
                                                                        Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)
                                                                        Master.scrapeMovie.Thumbs = pThumbs
                                                                        Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                                                    End If
                                                                End If
                                                            End Using
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                        pThumbs = Nothing

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Not drvRow.Item(5) AndAlso Not String.IsNullOrEmpty(Master.scrapeMovie.IMDBID) AndAlso (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Fanart) Then
                                            If Master.eSettings.UseTMDB Then

                                                If Fanart.IsAllowedToDownload(sPath, drvRow.Item(2), Master.ImageType.Fanart) Then
                                                    If Fanart.GetPreferredImage(Master.scrapeMovie.IMDBID, Master.ImageType.Fanart, fArt, Nothing, True) Then

                                                        If Not IsNothing(Fanart.Image) Then
                                                            Fanart.SaveAsFanart(sPath, drvRow.Item(2))
                                                            parFanart.Value = True
                                                            If File.Exists(nfoPath) AndAlso Args.scrapeMod = ScrapeModifier.All Then
                                                                'need to load movie from nfo here in case the movie already had
                                                                'an nfo.... scrapeMovie would not be set to the proper movie
                                                                Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)
                                                                Master.scrapeMovie.Fanart = fArt
                                                                Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                                            End If
                                                        Else
                                                            MsgBox("Fanart of your preferred size could not be found. Please choose another", MsgBoxStyle.Information, "No Preferred Size")
                                                            Using dImgSelect As New dlgImgSelect
                                                                If dImgSelect.ShowDialog(Master.scrapeMovie.IMDBID, sPath, Master.ImageType.Fanart) = Windows.Forms.DialogResult.OK Then
                                                                    parFanart.Value = True

                                                                    If File.Exists(nfoPath) AndAlso Args.scrapeMod = ScrapeModifier.All Then
                                                                        'need to load movie from nfo here in case the movie already had
                                                                        'an nfo.... scrapeMovie would not be set to the proper movie
                                                                        Master.scrapeMovie = Master.LoadMovieFromNFO(nfoPath)
                                                                        Master.scrapeMovie.Fanart = fArt
                                                                        Master.SaveMovieToNFO(Master.scrapeMovie, sPath, drvRow.Item(2))
                                                                    End If
                                                                End If
                                                            End Using
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                        fArt = Nothing

                                        If Me.bwScraper.CancellationPending Then GoTo doCancel
                                        If Master.eSettings.AutoThumbs > 0 AndAlso Not drvRow.Item(2) AndAlso Not Directory.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "extrathumbs")) AndAlso _
                                        (Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.Extra) Then
                                            If Master.CreateRandomThumbs(sPath, Master.eSettings.AutoThumbs) Then parFanart.Value = True
                                        End If
                                    End If

                                    SQLcommand.ExecuteNonQuery()
                                Next

                        End Select
                    End If

                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            End Using
doCancel:
            SQLtransaction.Commit()

        End Using

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
    End Sub

    Private Sub bwScraper_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwScraper.RunWorkerCompleted

        '//
        ' Thread finished: re-fill media list and load info for first item
        '\\

        Me.pnlCancel.Visible = False

        Select Case e.Result
            Case Master.ScrapeType.CleanFolders
                Me.LoadMedia(1)
            Case Else

                Try
                    If Me.dgvMediaList.SelectedRows.Count > 0 Then
                        Me.FillList(Me.dgvMediaList.SelectedRows(0).Index)
                    Else
                        Me.FillList(0)
                    End If

                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

                Me.tslLoading.Visible = False
                Me.tspbLoading.Visible = False
                Me.tslStatus.Text = String.Empty

                Me.tsbAutoPilot.Enabled = True
                Me.tsbRefreshMedia.Enabled = True
                Me.mnuMediaList.Enabled = True
                Me.tabsMain.Enabled = True
                Me.EnableFilters(True)
        End Select

    End Sub

    Private Sub bwValidateNfo_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwValidateNfo.DoWork

        '//
        ' Thread to check each nfo to make sure it is valid
        '\\

        Dim nfoPath As String = String.Empty
        Dim Args As Arguments = e.Argument

        If Args.scrapeMod = ScrapeModifier.All OrElse Args.scrapeMod = ScrapeModifier.NFO Then
            For i As Integer = 0 To Me.dtMedia.Rows.Count - 1
                If bwValidateNfo.CancellationPending Then
                    e.Cancel = True
                    Return
                End If

                nfoPath = Master.GetNfoPath(Me.dtMedia.Rows(i).Item(1), Me.dtMedia.Rows(i).Item(2))
                Me.bwValidateNfo.ReportProgress(i, nfoPath)
                If Not Master.IsConformingNfo(nfoPath) Then
                    Me.dtMedia.Rows(i).Item(6) = False
                End If
            Next
        End If

        If bwValidateNfo.CancellationPending Then
            e.Cancel = True
            Return
        End If

        e.Result = New Results With {.scrapeType = Args.scrapeType, .scrapeMod = Args.scrapeMod}

    End Sub

    Private Sub bwValidateNfo_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwValidateNfo.ProgressChanged
        Me.tslStatus.Text = e.UserState.ToString
        Me.tspbLoading.Value = e.ProgressPercentage
    End Sub

    Private Sub bwValidateNfo_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwValidateNfo.RunWorkerCompleted

        '//
        ' Thread finished: start scraping
        '\\

        Dim Res As Results = e.Result

        If Not bwScraper.CancellationPending AndAlso Not e.Cancelled Then
            Me.tspbLoading.Value = 0
            Me.tspbLoading.Maximum = Me.dtMedia.Rows.Count
            If Res.scrapeType = Master.ScrapeType.UpdateAuto Then
                Me.tslLoading.Text = "Updating Media (Movies Missing Items - Auto):"
            Else
                Me.tslLoading.Text = "Updating Media (Movies Missing Items - Ask):"
            End If
            Me.tslLoading.Visible = True
            Me.tspbLoading.Visible = True

            If Not bwScraper.IsBusy Then
                bwScraper.WorkerSupportsCancellation = True
                bwScraper.WorkerReportsProgress = True
                bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = Res.scrapeType, .scrapeMod = Res.scrapeMod})
            End If
        Else
            pnlCancel.Visible = False
        End If
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
                .scMain.Panel1.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .pnlSearch.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .pnlFilter.BackColor = Color.FromArgb(Master.eSettings.InfoPanelColor)
                .lblFilter.BackColor = Color.FromArgb(Master.eSettings.HeaderColor)
                .lblFilter.ForeColor = Color.FromArgb(Master.eSettings.HeaderTextColor)


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
            Me.tslStatus.Text = "Performing preliminary tasks..."
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

            Me.SaveMovieList()
            Me.txtSearch.Text = String.Empty

            Me.dgvMediaList.DataSource = Nothing

            Master.MediaList.Clear()

            Me.pnlInfoPanel.Height = 25
            Me.btnDown.Enabled = False
            Me.btnMid.Enabled = False
            Me.btnUp.Enabled = False

            Me.ClearInfo()
            Me.EnableFilters(False)

            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.mnuMediaList.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tabMovies.Text = "Movies"

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
                Me.bwMediaInfo.RunWorkerAsync(New Arguments With {.setEnabled = setEnabled, .Path = sPath, .Movie = Master.currMovie, .isFile = isFile})
            End If

            If doInfo Then
                Me.ClearInfo()
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
            If Not String.IsNullOrEmpty(Master.currMovie.Title) AndAlso (Not String.IsNullOrEmpty(Master.currMovie.Year) AndAlso Convert.ToInt32(Master.currMovie.Year) > 0) Then
                Me.lblTitle.Text = String.Format("{0} ({1})", Master.currMovie.Title, Master.currMovie.Year)
            ElseIf Not String.IsNullOrEmpty(Master.currMovie.Title) AndAlso (String.IsNullOrEmpty(Master.currMovie.Year) OrElse Convert.ToInt32(Master.currMovie.Year) = 0) Then
                Me.lblTitle.Text = Master.currMovie.Title
            ElseIf String.IsNullOrEmpty(Master.currMovie.Title) AndAlso (Not String.IsNullOrEmpty(Master.currMovie.Year) AndAlso Convert.ToInt32(Master.currMovie.Year) > 0) Then
                Me.lblTitle.Text = String.Format("Unknown Movie ({0})", Master.currMovie.Year)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Votes) Then
                Me.lblVotes.Text = String.Format("{0} Votes", Master.currMovie.Votes)
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Runtime) Then
                Me.lblRuntime.Text = String.Format("Runtime: {0}", If(Master.currMovie.Runtime.Contains("|"), Strings.Left(Master.currMovie.Runtime, Master.currMovie.Runtime.IndexOf("|")), Master.currMovie.Runtime)).Trim
            End If

            If Not String.IsNullOrEmpty(Master.currMovie.Top250) AndAlso IsNumeric(Master.currMovie.Top250) AndAlso Convert.ToInt32(Master.currMovie.Top250) > 0 Then
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

            Dim tmpRating As Double = Master.ConvertToDouble(Master.currMovie.Rating)
            If tmpRating > 0 Then Me.BuildStars(tmpRating)

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

            Me.txtMediaInfo.Text = Master.FIToString(Master.currMovie.FileInfo, Master.currMovie.Studio)

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

    Private Sub createGenreThumbs(ByVal strGenres As String)

        '//
        ' Parse the genre string and create panels/pictureboxes for each one
        '\\

        Dim genreArray() As String

        Try
            genreArray = Strings.Split(strGenres, " / ")
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

    Private Sub ScrapeData(ByVal sType As Master.ScrapeType, ByVal sMod As ScrapeModifier, Optional ByVal doSearch As Boolean = False)

        Try
            Dim chkCount As Integer = 0
            Dim nfoPath As String = String.Empty

            Me.tsbAutoPilot.Enabled = False
            Me.tsbRefreshMedia.Enabled = False
            Me.mnuMediaList.Enabled = False
            Me.tabsMain.Enabled = False
            Me.tspbLoading.Style = ProgressBarStyle.Continuous
            Me.EnableFilters(False)

            If Not sType = Master.ScrapeType.SingleScrape Then
                btnCancel.Visible = True
                lblCanceling.Visible = False
                pbCanceling.Visible = False
                Me.pnlCancel.Visible = True
            End If

            Select Case sType
                Case Master.ScrapeType.FullAsk, Master.ScrapeType.FullAuto, Master.ScrapeType.MIOnly, Master.ScrapeType.CleanFolders, Master.ScrapeType.CopyBD
                    Me.tspbLoading.Style = ProgressBarStyle.Continuous
                    Me.tspbLoading.Maximum = Me.dtMedia.Rows.Count
                    Select Case sType
                        Case Master.ScrapeType.FullAsk
                            Me.tslLoading.Text = "Updating Media (All Movies - Ask):"
                        Case Master.ScrapeType.FullAuto
                            Me.tslLoading.Text = "Updating Media (All Movies - Auto):"
                        Case Master.ScrapeType.MIOnly
                            Me.tslLoading.Text = "Updating Media (All Movies - MI Only):"
                        Case Master.ScrapeType.CleanFolders
                            Me.tslLoading.Text = "Cleaning Files:"
                        Case Master.ScrapeType.CopyBD
                            Me.tslLoading.Text = "Copying Fanart to Backdrops Folder:"
                    End Select
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwScraper.IsBusy Then
                        bwScraper.WorkerReportsProgress = True
                        bwScraper.WorkerSupportsCancellation = True
                        bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .scrapeMod = sMod})
                    End If

                Case Master.ScrapeType.UpdateAsk, Master.ScrapeType.UpdateAuto

                    Me.tspbLoading.Maximum = Me.dtMedia.Rows.Count
                    Me.tslLoading.Text = "Checking for valid NFO:"
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True

                    If Not bwValidateNfo.IsBusy Then
                        bwValidateNfo.WorkerSupportsCancellation = True
                        bwValidateNfo.WorkerReportsProgress = True
                        bwValidateNfo.RunWorkerAsync(New Arguments With {.scrapeType = sType, .scrapeMod = sMod})
                    End If

                Case Master.ScrapeType.NewAsk, Master.ScrapeType.NewAuto, Master.ScrapeType.MarkAsk, Master.ScrapeType.MarkAuto
                    For Each drvRow As DataRow In Me.dtMedia.Rows
                        If drvRow.Item(10) AndAlso (sType = Master.ScrapeType.NewAsk OrElse sType = Master.ScrapeType.NewAuto) Then
                            chkCount += 1
                        End If
                        If drvRow.Item(11) AndAlso (sType = Master.ScrapeType.MarkAsk OrElse sType = Master.ScrapeType.MarkAuto) Then
                            chkCount += 1
                        End If
                    Next

                    If chkCount > 0 Then
                        Select Case sType
                            Case Master.ScrapeType.NewAsk
                                Me.tslLoading.Text = "Updating Media (New Movies - Ask):"
                            Case Master.ScrapeType.NewAuto
                                Me.tslLoading.Text = "Updating Media (New Movies - Auto):"
                            Case Master.ScrapeType.MarkAsk
                                Me.tslLoading.Text = "Updating Media (Marked Movies - Ask):"
                            Case Master.ScrapeType.MarkAuto
                                Me.tslLoading.Text = "Updating Media (Marked Movies - Auto):"
                        End Select
                        Me.tspbLoading.Maximum = chkCount
                        Me.tslLoading.Visible = True
                        Me.tspbLoading.Visible = True

                        If Not bwScraper.IsBusy Then
                            bwScraper.WorkerSupportsCancellation = True
                            bwScraper.WorkerReportsProgress = True
                            bwScraper.RunWorkerAsync(New Arguments With {.scrapeType = sType, .scrapeMod = sMod})
                        End If
                    Else
                        Me.tslLoading.Visible = False
                        Me.tspbLoading.Visible = False
                        Me.tslStatus.Text = String.Empty
                        Me.tsbAutoPilot.Enabled = True
                        Me.tsbRefreshMedia.Enabled = True
                        Me.mnuMediaList.Enabled = True
                        Me.tabsMain.Enabled = True
                        Me.EnableFilters(True)
                    End If
                Case Master.ScrapeType.SingleScrape
                    Me.ClearInfo(True)
                    Me.tslStatus.Text = String.Format("Re-scraping {0}", Master.currMovie.Title)
                    Me.tslLoading.Text = "Scraping:"
                    Me.tspbLoading.Maximum = 13
                    Me.ReportDownloadPercent = True
                    Me.tslLoading.Visible = True
                    Me.tspbLoading.Visible = True
                    Me.dgvMediaList.Enabled = False

                    If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) AndAlso doSearch = False Then
                        IMDB.GetMovieInfoAsync(Master.currMovie.IMDBID, Master.currMovie, Master.eSettings.FullCrew, Master.eSettings.FullCast)
                    Else
                        Master.tmpMovie = New Media.Movie
                        Using dSearch As New dlgIMDBSearchResults
                            If dSearch.ShowDialog(Me.tmpTitle) = Windows.Forms.DialogResult.OK Then
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
                                If isCL Then
                                    Me.SingelScrapeDone = True
                                Else
                                    Me.dgvMediaList.Enabled = True
                                    Me.tslLoading.Visible = False
                                    Me.tspbLoading.Visible = False
                                    Me.tslStatus.Text = String.Empty
                                    Me.tsbAutoPilot.Enabled = True
                                    Me.tsbRefreshMedia.Enabled = True
                                    Me.mnuMediaList.Enabled = True
                                    Me.tabsMain.Enabled = True
                                    Me.EnableFilters(True)
                                    Me.LoadInfo(Master.currPath, True, False, Master.isFile)
                                End If
                            End If
                        End Using
                    End If
            End Select
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Function UpdateMediaInfo(ByVal sPath As String, ByRef miMovie As Media.Movie) As Boolean
        Try

            Dim pExt As String = Path.GetExtension(sPath).ToLower
            If Not pExt = ".rar" Then
                Dim MI As New MediaInfo.MInfo
                Dim miFileInfo = New MediaInfo.Fileinfo
                MI.GetMovieMIFromPath(miFileInfo, sPath)
                miMovie.FileInfo = miFileInfo
                If Master.eSettings.UseMIDuration AndAlso miMovie.FileInfo.StreamDetails.Video.Count > 0 AndAlso Not String.IsNullOrEmpty(miMovie.FileInfo.StreamDetails.Video.Item(0).Duration) Then
                    Dim sDuration As Match = Regex.Match(miMovie.FileInfo.StreamDetails.Video.Item(0).Duration, "(([0-9]+)h)?\s?(([0-9]+)mn)?")
                    Dim sHour As Integer = If(Not String.IsNullOrEmpty(sDuration.Groups(2).Value), (Convert.ToInt32(sDuration.Groups(2).Value)), 0) * 60
                    Dim sMin As Integer = If(Not String.IsNullOrEmpty(sDuration.Groups(4).Value), (Convert.ToInt32(sDuration.Groups(4).Value)), 0)
                    miMovie.Runtime = String.Format("{0} min", sHour + sMin)
                End If
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
                    If Me.UpdateMediaInfo(Master.currPath, Master.currMovie) Then
                        Master.currMovie.Studio = String.Concat(Master.currMovie.StudioReal, Master.FITagData(Master.currMovie.FileInfo))
                    End If
                End If
                If Master.eSettings.SingleScrapeImages Then
                    Dim tmpImages As New Images
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.isFile, Master.ImageType.Posters) Then
                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Posters)
                        End Using
                    End If
                    If tmpImages.IsAllowedToDownload(Master.currPath, Master.isFile, Master.ImageType.Fanart) Then
                        Using dImgSelect As New dlgImgSelect
                            dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Fanart)
                        End Using
                    End If
                    tmpImages.Dispose()
                    tmpImages = Nothing
                    If Master.eSettings.AutoThumbs > 0 AndAlso Not Master.isFile Then
                        Master.CreateRandomThumbs(Master.currPath, Master.eSettings.AutoThumbs)
                    End If
                End If
                If Not isCL Then
                    Dim indX As Integer = Me.dgvMediaList.SelectedRows(0).Index
                    Dim ID As Integer = Me.dgvMediaList.Rows(indX).Cells(0).Value

                    Using dEditMovie As New dlgEditMovie
                        Select Case dEditMovie.ShowDialog(ID)
                            Case Windows.Forms.DialogResult.OK
                                Me.ReCheckItems(ID)
                                Me.SetListItemAfterEdit(ID, indX)
                            Case Windows.Forms.DialogResult.Retry
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing)
                            Case Windows.Forms.DialogResult.Abort
                                Me.ScrapeData(Master.ScrapeType.SingleScrape, Nothing, True)
                        End Select
                    End Using
                Else
                    Master.SaveMovieToNFO(Master.currMovie, Master.currPath, Master.isFile)
                End If
            Else
                MsgBox("Unable to retrieve movie details from the internet. Please check your connection and try again.", MsgBoxStyle.Exclamation, "Error Retrieving Details")
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If isCL Then
            Me.SingelScrapeDone = True
        Else
            Me.dgvMediaList.Enabled = True
            Me.tslLoading.Visible = False
            Me.tspbLoading.Visible = False
            Me.tslStatus.Text = String.Empty
            Me.tsbAutoPilot.Enabled = True
            Me.tsbRefreshMedia.Enabled = True
            Me.mnuMediaList.Enabled = True
            Me.tabsMain.Enabled = True
        End If
    End Sub

    Private Sub ReCheckItems(ByVal ID As Integer)
        Dim tPath = From drvRow As DataRow In dtMedia.Rows Where drvRow.Item(0) = ID Select drvRow.Item(1)
        Dim sPath As String = tPath(0)
        Try
            Dim parPath As String = Directory.GetParent(sPath).FullName
            Dim tmpName As String = String.Empty
            Dim tmpNameNoStack As String = String.Empty
            Dim hasNfo As Boolean = False
            Dim hasPoster As Boolean = False
            Dim hasFanart As Boolean = False
            Dim hasTrailer As Boolean = False
            Dim hasSub As Boolean = False
            Dim hasExtra As Boolean = False

            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                tmpName = Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name)
                If File.Exists(String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    hasExtra = True
                End If
            Else
                tmpName = Path.Combine(parPath, Master.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath)))
                tmpNameNoStack = Path.Combine(parPath, Path.GetFileNameWithoutExtension(sPath))
                If File.Exists(String.Concat(Directory.GetParent(sPath).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    hasExtra = True
                End If
            End If

            'fanart
            If File.Exists(String.Concat(tmpName, "-fanart.jpg")) OrElse File.Exists(String.Concat(tmpName, ".fanart.jpg")) OrElse _
            File.Exists(String.Concat(tmpNameNoStack, "-fanart.jpg")) OrElse File.Exists(String.Concat(tmpNameNoStack, ".fanart.jpg")) OrElse _
            File.Exists(Path.Combine(parPath, "fanart.jpg")) OrElse File.Exists(Path.Combine(parPath, "video_ts-fanart.jpg")) OrElse _
            File.Exists(Path.Combine(parPath, "video_ts.fanart.jpg")) Then
                hasFanart = True
            End If

            'poster
            If File.Exists(String.Concat(tmpName, ".jpg")) OrElse File.Exists(Path.Combine(parPath, "movie.jpg")) OrElse _
                File.Exists(Path.Combine(parPath, "poster.jpg")) OrElse File.Exists(Path.Combine(parPath, "folder.jpg")) OrElse _
                File.Exists(String.Concat(tmpName, ".tbn")) OrElse File.Exists(Path.Combine(parPath, "movie.tbn")) OrElse _
                File.Exists(Path.Combine(parPath, "poster.tbn")) OrElse File.Exists(Path.Combine(parPath, "video_ts.tbn")) OrElse _
                File.Exists(Path.Combine(parPath, "video_ts.jpg")) OrElse File.Exists(String.Concat(tmpNameNoStack, ".jpg")) OrElse _
                File.Exists(String.Concat(tmpNameNoStack, ".tbn")) Then
                hasPoster = True
            End If

            'nfo
            If File.Exists(String.Concat(tmpName, ".nfo")) OrElse File.Exists(String.Concat(tmpNameNoStack, ".nfo")) OrElse File.Exists(Path.Combine(parPath, "movie.nfo")) OrElse _
                File.Exists(Path.Combine(parPath, "video_ts.nfo")) Then
                hasNfo = True
            End If

            'sub
            Dim sExt() As String = Split(".sst,.srt,.sub,.ssa,.aqt,.smi,.sami,.jss,.mpl,.rt,.idx,.ass", ",")

            For Each t As String In sExt
                If File.Exists(String.Concat(tmpName, t)) OrElse File.Exists(String.Concat(tmpName, t)) OrElse _
                    File.Exists(String.Concat(tmpNameNoStack, t)) OrElse File.Exists(String.Concat(tmpNameNoStack, t)) Then
                    hasSub = True
                    Exit For
                End If
            Next

            For Each t As String In Master.eSettings.ValidExts
                If File.Exists(String.Concat(tmpName, "-trailer", t)) OrElse File.Exists(String.Concat(tmpName, "[trailer]", t)) OrElse _
                File.Exists(String.Concat(tmpNameNoStack, "-trailer", t)) OrElse File.Exists(String.Concat(tmpNameNoStack, "[trailer]", t)) Then
                    hasTrailer = True
                    Exit For
                End If
            Next

            Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                    SQLcommand.CommandText = "UPDATE movies SET poster = (?), fanart = (?), info = (?), trailer = (?), sub = (?), extra = (?) WHERE ID = (?);"
                    Dim parPoster As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPoster", DbType.Boolean, 0, "poster")
                    Dim parFanart As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFanart", DbType.Boolean, 0, "fanart")
                    Dim parInfo As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parInfo", DbType.Boolean, 0, "info")
                    Dim parTrailer As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTrailer", DbType.Boolean, 0, "trailer")
                    Dim parSub As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSub", DbType.Boolean, 0, "sub")
                    Dim parExtra As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parExtra", DbType.Boolean, 0, "extra")
                    Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")

                    parPoster.Value = hasPoster
                    parFanart.Value = hasFanart
                    parInfo.Value = hasNfo
                    parTrailer.Value = hasTrailer
                    parSub.Value = hasSub
                    parExtra.Value = hasExtra
                    parID.Value = ID

                    SQLcommand.ExecuteNonQuery()
                End Using
                SQLtransaction.Commit()
            End Using

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetMenus()

        With Master.eSettings
            If .CleanDotFanartJPG OrElse .CleanFanartJPG OrElse .CleanFolderJPG OrElse .CleanMovieFanartJPG OrElse _
            .CleanMovieJPG OrElse .CleanMovieNameJPG OrElse .CleanMovieNFO OrElse .CleanMovieNFOB OrElse _
            .CleanMovieTBN OrElse .CleanMovieTBNB OrElse .CleanPosterJPG OrElse .CleanPosterTBN OrElse .CleanExtraThumbs Then
                Me.CleanFoldersToolStripMenuItem.Enabled = True
            Else
                Me.CleanFoldersToolStripMenuItem.Enabled = False
            End If

            If .XBMCComs.Count > 0 Then
                Me.tsbUpdateXBMC.Enabled = True
                tsbUpdateXBMC.DropDownItems.Clear()
                For Each xCom As emmSettings.XBMCCom In .XBMCComs
                    tsbUpdateXBMC.DropDownItems.Add(String.Concat("Update ", xCom.Name, " Only"), Nothing, New System.EventHandler(AddressOf XComSubClick))
                Next
            Else
                Me.tsbUpdateXBMC.Enabled = False
            End If

            Me.CopyExistingFanartToBackdropsFolderToolStripMenuItem.Enabled = Directory.Exists(.BDPath)

            Me.ClearAllCachesToolStripMenuItem.Enabled = Master.eSettings.UseImgCache
        End With
    End Sub

    Private Sub SortFiles(ByVal sPath As String)
        Dim tmpAL As New ArrayList
        Dim tmpPath As String = String.Empty
        Dim tmpName As String = String.Empty

        Try
            If Directory.Exists(sPath) Then
                Dim di As New DirectoryInfo(sPath)
                Dim lFi As New List(Of FileInfo)

                lFi.AddRange(di.GetFiles())

                For Each sFile As FileInfo In lFi
                    tmpName = Master.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sFile.Name))
                    tmpName = tmpName.Replace(".fanart", String.Empty)
                    tmpName = tmpName.Replace("-fanart", String.Empty)
                    tmpPath = Path.Combine(sPath, tmpName)
                    If Not Directory.Exists(tmpPath) Then
                        Directory.CreateDirectory(tmpPath)
                    End If

                    File.Move(sFile.FullName, Path.Combine(tmpPath, sFile.Name))
                Next
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetFilterColors()
        For Each drvRow As DataGridViewRow In Me.dgvMediaList.Rows
            If drvRow.Cells(11).Value Then
                drvRow.Cells(3).Style.ForeColor = Color.Crimson
                drvRow.Cells(3).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                drvRow.Cells(3).Style.SelectionForeColor = Color.Crimson
            ElseIf drvRow.Cells(10).Value Then
                drvRow.Cells(3).Style.ForeColor = Color.Green
                drvRow.Cells(3).Style.Font = New Font("Microsoft Sans Serif", 9, FontStyle.Bold)
                drvRow.Cells(3).Style.SelectionForeColor = Color.Green
            Else
                drvRow.Cells(3).Style.ForeColor = Color.Black
                drvRow.Cells(3).Style.Font = New Font("Microsoft Sans Serif", 8.25, FontStyle.Regular)
                drvRow.Cells(3).Style.SelectionForeColor = Color.FromKnownColor(KnownColor.HighlightText)
            End If

            If drvRow.Cells(14).Value Then
                drvRow.Cells(3).Style.BackColor = Color.MistyRose
                drvRow.Cells(4).Style.BackColor = Color.MistyRose
                drvRow.Cells(5).Style.BackColor = Color.MistyRose
                drvRow.Cells(6).Style.BackColor = Color.MistyRose
                drvRow.Cells(7).Style.BackColor = Color.MistyRose
                drvRow.Cells(8).Style.BackColor = Color.MistyRose
                drvRow.Cells(9).Style.BackColor = Color.MistyRose
                drvRow.Cells(3).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(4).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(5).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(6).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(7).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(8).Style.SelectionBackColor = Color.DarkMagenta
                drvRow.Cells(9).Style.SelectionBackColor = Color.DarkMagenta
            Else
                drvRow.Cells(3).Style.BackColor = Color.White
                drvRow.Cells(4).Style.BackColor = Color.White
                drvRow.Cells(5).Style.BackColor = Color.White
                drvRow.Cells(6).Style.BackColor = Color.White
                drvRow.Cells(7).Style.BackColor = Color.White
                drvRow.Cells(8).Style.BackColor = Color.White
                drvRow.Cells(9).Style.BackColor = Color.White
                drvRow.Cells(3).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(4).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(5).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(6).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(7).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(8).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
                drvRow.Cells(9).Style.SelectionBackColor = Color.FromKnownColor(KnownColor.Highlight)
            End If
        Next
    End Sub

    Private Sub EnableFilters(ByVal isEnabled As Boolean)
        Me.chkFilterDupe.Enabled = isEnabled
        Me.chkFilterMark.Enabled = isEnabled
        Me.chkFilterNew.Enabled = isEnabled
        Me.rbFilterOr.Enabled = isEnabled
        Me.rbFilterAnd.Enabled = isEnabled
    End Sub

    Private Sub ClearFilters()
        Me.chkFilterDupe.Enabled = True
        Me.chkFilterDupe.Checked = False
        Me.chkFilterMark.Enabled = True
        Me.chkFilterMark.Checked = False
        Me.chkFilterNew.Enabled = True
        Me.chkFilterNew.Checked = False
        Me.rbFilterOr.Enabled = True
        Me.rbFilterOr.Checked = False
        Me.rbFilterAnd.Enabled = True
        Me.rbFilterAnd.Checked = True
    End Sub

    Private Sub RunFilter()
        If Me.Created Then

            If Me.chkFilterDupe.Checked Then
                Me.FillList(0, True)
            Else
                If FilterArray.Count > 0 Then
                    Dim FilterString As String = String.Empty

                    If rbFilterAnd.Checked Then
                        FilterString = Strings.Join(FilterArray.ToArray, " AND ")
                    Else
                        FilterString = Strings.Join(FilterArray.ToArray, " OR ")
                    End If

                    bsMedia.Filter = FilterString
                Else
                    bsMedia.RemoveFilter()
                End If
                Me.FillList(0)
            End If
        End If
    End Sub

    Private Sub XComSubClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim xComName As String = sender.ToString.Replace("Update ", String.Empty).Replace(" Only", String.Empty).Trim
        Dim xCom = From x As emmSettings.XBMCCom In Master.eSettings.XBMCComs Where x.Name = xComName
        If xCom.Count > 0 Then
            DoXCom(xCom(0))
        End If
    End Sub

    Private Sub DoXCom(ByVal xCom As emmSettings.XBMCCom)
        Try
            Dim Wr As HttpWebRequest = HttpWebRequest.Create(String.Format("http://{0}:{1}/xbmcCmds/xbmcHttp?command=ExecBuiltIn&parameter=XBMC.updatelibrary(video)", xCom.IP, xCom.Port))
            Wr.Method = "GET"
            Wr.Timeout = 2500
            If Not String.IsNullOrEmpty(xCom.Username) AndAlso Not String.IsNullOrEmpty(xCom.Password) Then
                Wr.Credentials = New NetworkCredential(xCom.Username, xCom.Password)
            End If
            Using Wres As HttpWebResponse = Wr.GetResponse
                Dim Sr As String = New StreamReader(Wres.GetResponseStream()).ReadToEnd
                If Not Sr.Contains("OK") Then
                    MsgBox(String.Concat("There was a problem communicating with ", xCom.Name, vbNewLine, "Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), MsgBoxStyle.Exclamation, String.Concat("Unable to Start XBMC Update for ", xCom.Name))
                End If
            End Using
            Wr = Nothing
        Catch
            MsgBox(String.Concat("There was a problem communicating with ", xCom.Name, vbNewLine, "Please ensure that the XBMC webserver is enabled and that you have entered the correct IP and Port in Settings."), MsgBoxStyle.Exclamation, String.Concat("Unable to Start XBMC Update for ", xCom.Name))
        End Try
    End Sub

    Private Sub SaveMovieList()
        Try
            Me.ClearFilters()
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                    SQLcommand.CommandText = "UPDATE movies SET new = (?);"
                    Dim parNew As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNew", DbType.Boolean, 0, "new")
                    parNew.Value = False
                    SQLcommand.ExecuteNonQuery()
                End Using
                SQLtransaction.Commit()
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                SQLcommand.CommandText = "VACUUM;"
                SQLcommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FillList(ByVal iIndex As Integer, Optional ByVal DupesOnly As Boolean = False)
        Try
            Dim sqlDA As New SQLite.SQLiteDataAdapter
            Me.bsMedia.DataSource = Nothing
            Me.dgvMediaList.DataSource = Nothing
            Me.ClearInfo()
            Me.pnlInfoPanel.Height = 25
            Me.btnDown.Enabled = False
            Me.btnMid.Enabled = True
            Me.btnUp.Enabled = True
            Application.DoEvents()

            Me.dtMedia = New DataTable
            If DupesOnly Then
                If Master.eSettings.UseNameFromNfo Then
                    sqlDA = New SQLite.SQLiteDataAdapter("SELECT * FROM movies WHERE imdb IN (SELECT imdb FROM movies GROUP BY imdb HAVING ( COUNT(imdb) > 1 ))  ORDER BY title", Master.SQLcn)
                Else
                    sqlDA = New SQLite.SQLiteDataAdapter("SELECT * FROM movies WHERE title IN (SELECT title FROM movies GROUP BY title HAVING ( COUNT(title) > 1 ))  ORDER BY title", Master.SQLcn)
                End If
            Else
                sqlDA = New SQLite.SQLiteDataAdapter("SELECT * FROM movies ORDER BY title", Master.SQLcn)
            End If

            Dim sqlCB As New SQLite.SQLiteCommandBuilder(sqlDA)
            sqlDA.Fill(Me.dtMedia)

            If Me.dtMedia.Rows.Count > 0 Then

                With Me
                    .bsMedia.DataSource = dtMedia
                    .dgvMediaList.DataSource = bsMedia

                    'why did the resizable property all the sudden become opposite? resizable = false now means it IS resizable
                    'wasn't like that before and was reported (after release of v alpha 022, but no telling how long it's been
                    'like that) that the info columns were resizable
                    .dgvMediaList.Columns(0).Visible = False
                    .dgvMediaList.Columns(1).Visible = False
                    .dgvMediaList.Columns(2).Visible = False
                    .dgvMediaList.Columns(3).Resizable = False
                    .dgvMediaList.Columns(3).ReadOnly = True
                    .dgvMediaList.Columns(3).MinimumWidth = 83
                    .dgvMediaList.Columns(3).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(4).Width = 20
                    .dgvMediaList.Columns(4).Resizable = True
                    .dgvMediaList.Columns(4).ReadOnly = True
                    .dgvMediaList.Columns(4).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(4).Visible = Not Master.eSettings.MoviePosterCol
                    .dgvMediaList.Columns(5).Width = 20
                    .dgvMediaList.Columns(5).Resizable = True
                    .dgvMediaList.Columns(5).ReadOnly = True
                    .dgvMediaList.Columns(5).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(5).Visible = Not Master.eSettings.MovieFanartCol
                    .dgvMediaList.Columns(6).Width = 20
                    .dgvMediaList.Columns(6).Resizable = True
                    .dgvMediaList.Columns(6).ReadOnly = True
                    .dgvMediaList.Columns(6).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(6).Visible = Not Master.eSettings.MovieInfoCol
                    .dgvMediaList.Columns(7).Width = 20
                    .dgvMediaList.Columns(7).Resizable = True
                    .dgvMediaList.Columns(7).ReadOnly = True
                    .dgvMediaList.Columns(7).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(7).Visible = Not Master.eSettings.MovieTrailerCol
                    .dgvMediaList.Columns(8).Width = 20
                    .dgvMediaList.Columns(8).Resizable = True
                    .dgvMediaList.Columns(8).ReadOnly = True
                    .dgvMediaList.Columns(8).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(8).Visible = Not Master.eSettings.MovieSubCol
                    .dgvMediaList.Columns(9).Width = 20
                    .dgvMediaList.Columns(9).Resizable = True
                    .dgvMediaList.Columns(9).ReadOnly = True
                    .dgvMediaList.Columns(9).SortMode = DataGridViewColumnSortMode.Automatic
                    .dgvMediaList.Columns(9).Visible = Not Master.eSettings.MovieExtraCol
                    .dgvMediaList.Columns(10).Visible = False
                    .dgvMediaList.Columns(11).Visible = False
                    .dgvMediaList.Columns(12).Visible = False
                    .dgvMediaList.Columns(13).Visible = False
                    .dgvMediaList.Columns(14).Visible = False


                    'Trick to autosize the first column, but still allow resizing by user
                    .dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    .dgvMediaList.Columns(3).AutoSizeMode = DataGridViewAutoSizeColumnMode.None

                    'Trick to work around the blank table bug in the DGV
                    .dgvMediaList.Sort(.dgvMediaList.Columns(3), ComponentModel.ListSortDirection.Descending)
                    .dgvMediaList.Sort(.dgvMediaList.Columns(3), ComponentModel.ListSortDirection.Ascending)

                    .SetFilterColors()

                    'Set current cell and automatically load the info for the first movie in the list
                    .dgvMediaList.Rows(iIndex).Cells(3).Selected = True
                    .dgvMediaList.CurrentCell = .dgvMediaList.Rows(iIndex).Cells(3)

                    .btnUp.Enabled = True
                    .btnMid.Enabled = True

                    .tsbAutoPilot.Enabled = True
                    .mnuMediaList.Enabled = True
                End With
            Else
                Me.tsbAutoPilot.Enabled = False
                Me.mnuMediaList.Enabled = False
                Me.tslStatus.Text = String.Empty
                Me.btnUp.Enabled = False
                Me.btnDown.Enabled = False
                Me.btnMid.Enabled = False
                Me.ClearInfo()
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.tsbRefreshMedia.Enabled = True
        Me.tslLoading.Visible = False
        Me.tspbLoading.Visible = False
        Me.tspbLoading.Value = 0

        Me.tabMovies.Text = String.Format("Movies ({0})", Me.dgvMediaList.RowCount)
        Me.EnableFilters(True)

        Me.loadType = 0
    End Sub

    Public Sub SetListItemAfterEdit(ByVal iID As Integer, ByVal iRow As Integer)
        Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
            SQLcommand.CommandText = String.Concat("SELECT title, mark FROM movies WHERE id = ", iID, ";")
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                Me.SetFilterColors()
                Me.dgvMediaList.Item(3, iRow).Value = SQLreader("title")
                Me.dgvMediaList.Item(11, iRow).Value = SQLreader("mark")
            End Using
        End Using

        Me.SetFilterColors()
    End Sub
#End Region '*** Routines/Functions

End Class