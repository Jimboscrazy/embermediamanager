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



Imports System
Imports System.IO
Imports System.Text.RegularExpressions

Public Class dlgEditMovie
    Friend WithEvents bwThumbs As New System.ComponentModel.BackgroundWorker
    Private lvwActorSorter As ListViewColumnSorter
    Private lvwThumbSorter As ListViewColumnSorter
    Private tmpRating As String = String.Empty
    Private Poster As New Images With {.IsEdit = True}
    Private Fanart As New Images With {.IsEdit = True}
    Private Thumbs As New List(Of ExtraThumbs)
    Private DeleteList As New List(Of String)
    Private ExtraIndex As Integer = 0
    Private CachePath As String = String.Empty
    Private hasCleared As Boolean = False
    Private fResults As New Containers.ImgResult
    Private pResults As New Containers.ImgResult
    Private isAborting As Boolean = False
    Private PreviousFrameValue As Integer

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Me.SetInfo()

            Master.DB.SaveMovieToDB(Master.currMovie, False, False, True)

            Me.CleanUp()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.CleanUp()
        Master.currMovie = Master.DB.LoadMovieFromDB(Master.currMovie.ID)
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRescrape.Click
        Me.CleanUp()
        ' ***
        Me.DialogResult = System.Windows.Forms.DialogResult.Retry
        Me.Close()
    End Sub

    Private Sub btnChangeMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeMovie.Click
        Me.CleanUp()
        ' ***
        Me.DialogResult = System.Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub

    Private Sub dlgEditMovie_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        Me.Poster.Dispose()
        Me.Poster = Nothing

        Me.Fanart.Dispose()
        Me.Fanart = Nothing

        Me.Thumbs.Clear()
        Me.Thumbs = Nothing
    End Sub

    Private Sub dlgEditMovie_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.SetUp()

            Me.lvwActorSorter = New ListViewColumnSorter()
            Me.lvActors.ListViewItemSorter = Me.lvwActorSorter
            Me.lvwThumbSorter = New ListViewColumnSorter() With {.SortByText = True, .Order = SortOrder.Ascending, .NumericSort = True}
            Me.lvThumbs.ListViewItemSorter = Me.lvwThumbSorter

            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using

            Dim dFileInfoEdit As New dlgFileInfo
            dFileInfoEdit.TopLevel = False
            dFileInfoEdit.FormBorderStyle = FormBorderStyle.None
            dFileInfoEdit.BackColor = Color.White
            dFileInfoEdit.Cancel_Button.Visible = False
            Me.pnlFileInfo.Controls.Add(dFileInfoEdit)
            Dim oldwidth As Integer = dFileInfoEdit.Width
            dFileInfoEdit.Width = pnlFileInfo.Width
            dFileInfoEdit.Height = pnlFileInfo.Height
            dFileInfoEdit.Show(False)

            Me.LoadGenres()
            Me.LoadRatings()

            Me.FillInfo()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetUp()
        Dim mTitle As String = Master.currMovie.Movie.Title
        Dim mPathPieces() As String = Master.currMovie.Filename.Split(Path.DirectorySeparatorChar)
        Dim mShortPath As String = Master.currMovie.Filename
        If Not String.IsNullOrEmpty(mShortPath) AndAlso FileUtils.Common.isVideoTS(mShortPath) Then
            mShortPath = String.Concat(Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 3), Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 2), Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 1))
        ElseIf Not String.IsNullOrEmpty(mShortPath) AndAlso FileUtils.Common.isBDRip(mShortPath) Then
            mShortPath = String.Concat(Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 4), Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 3), Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 2), Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 1))
        Else
            mShortPath = String.Concat(Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 2), Path.DirectorySeparatorChar, mPathPieces(mPathPieces.Count - 1))
        End If
        Dim sTitle As String = String.Concat(Master.eLang.GetString(25, "Edit Movie"), If(String.IsNullOrEmpty(mTitle), String.Empty, String.Concat(" - ", mTitle)), If(String.IsNullOrEmpty(mShortPath), String.Empty, String.Concat(" | ", mShortPath)))
        Me.Text = sTitle
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.Label2.Text = Master.eLang.GetString(224, "Edit the details for the selected movie.")
        Me.Label1.Text = Master.eLang.GetString(25, "Edit Movie")
        Me.TabPage1.Text = Master.eLang.GetString(26, "Details")
        Me.lblLocalTrailer.Text = Master.eLang.GetString(225, "Local Trailer Found")
        Me.lblStudio.Text = Master.eLang.GetString(226, "Studio:")
        Me.lblTrailer.Text = Master.eLang.GetString(227, "Trailer URL:")
        Me.lblReleaseDate.Text = Master.eLang.GetString(236, "Release Date:")
        Me.lblCredits.Text = Master.eLang.GetString(228, "Credits:")
        Me.lblCerts.Text = Master.eLang.GetString(237, "Certification(s):")
        Me.lblRuntime.Text = Master.eLang.GetString(238, "Runtime:")
        Me.lblMPAADesc.Text = Master.eLang.GetString(229, "MPAA Rating Description:")
        Me.btnManual.Text = Master.eLang.GetString(230, "Manual Edit")
        Me.lblActors.Text = Master.eLang.GetString(231, "Actors:")
        Me.colName.Text = Master.eLang.GetString(232, "Name")
        Me.colRole.Text = Master.eLang.GetString(233, "Role")
        Me.colThumb.Text = Master.eLang.GetString(234, "Thumb")
        Me.lblGenre.Text = Master.eLang.GetString(51, "Genre(s):")
        Me.lblMPAA.Text = Master.eLang.GetString(235, "MPAA Rating:")
        Me.lblDirector.Text = Master.eLang.GetString(239, "Director:")
        Me.lblTop250.Text = Master.eLang.GetString(240, "Top 250:")
        Me.lblPlot.Text = Master.eLang.GetString(241, "Plot:")
        Me.lblOutline.Text = Master.eLang.GetString(242, "Plot Outline:")
        Me.lblTagline.Text = Master.eLang.GetString(243, "Tagline:")
        Me.lblVotes.Text = Master.eLang.GetString(244, "Votes:")
        Me.lblRating.Text = Master.eLang.GetString(245, "Rating:")
        Me.lblYear.Text = Master.eLang.GetString(49, "Year:")
        Me.lblTitle.Text = Master.eLang.GetString(246, "Title:")
        Me.TabPage2.Text = Master.eLang.GetString(148, "Poster")
        Me.btnRemovePoster.Text = Master.eLang.GetString(247, "Remove Poster")
        Me.btnSetPosterScrape.Text = Master.eLang.GetString(248, "Change Poster (Scrape)")
        Me.btnSetPoster.Text = Master.eLang.GetString(249, "Change Poster (Local)")
        Me.TabPage3.Text = Master.eLang.GetString(149, "Fanart")
        Me.btnRemoveFanart.Text = Master.eLang.GetString(250, "Remove Fanart")
        Me.btnSetFanartScrape.Text = Master.eLang.GetString(251, "Change Fanart (Scrape)")
        Me.btnSetFanart.Text = Master.eLang.GetString(252, "Change Fanart (Local)")
        Me.TabPage5.Text = Master.eLang.GetString(153, "Extrathumbs")
        Me.Label4.Text = Master.eLang.GetString(253, "You have extrathumbs queued to be transferred to the movie directory.")
        Me.btnTransferNow.Text = Master.eLang.GetString(254, "Transfer Now")
        Me.btnSetAsFanart.Text = Master.eLang.GetString(255, "Set As Fanart")
        Me.TabPage4.Text = Master.eLang.GetString(256, "Frame Extraction")
        Me.GroupBox1.Text = Master.eLang.GetString(257, "Auto-Generate")
        Me.Label5.Text = Master.eLang.GetString(258, "# to Create:")
        Me.btnAutoGen.Text = Master.eLang.GetString(259, "Auto-Gen")
        Me.btnFrameSave.Text = Master.eLang.GetString(260, "Save Extrathumb")
        Me.Label3.Text = Master.eLang.GetString(261, "Extracting Frame...")
        Me.btnFrameLoad.Text = Master.eLang.GetString(263, "Load Movie")
        Me.chkMark.Text = Master.eLang.GetString(23, "Mark")
        Me.btnRescrape.Text = Master.eLang.GetString(716, "Re-scrape")
        Me.btnChangeMovie.Text = Master.eLang.GetString(32, "Change Movie")
        Me.btnClearCache.Text = Master.eLang.GetString(264, "Clear Cache")
        Me.btnSetPosterDL.Text = Master.eLang.GetString(265, "Change Poster (Download)")
        Me.btnSetFanartDL.Text = Master.eLang.GetString(266, "Change Fanart (Download)")
        Me.Label6.Text = String.Concat(Master.eLang.GetString(642, "Sort Title"), ":")
    End Sub

    Private Sub pbStar1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar1.MouseLeave
        Try
            Dim tmpDBL As Single = 0
            Single.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbStar1.MouseMove
        Try
            If e.X < 12 Then
                Me.pbStar1.Tag = 1
                Me.BuildStars(1)
            Else
                Me.pbStar1.Tag = 2
                Me.BuildStars(2)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar2.MouseLeave
        Try
            Dim tmpDBL As Single = 0
            Single.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar2_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbStar2.MouseMove
        Try
            If e.X < 12 Then
                Me.pbStar2.Tag = 3
                Me.BuildStars(3)
            Else
                Me.pbStar2.Tag = 4
                Me.BuildStars(4)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar3.MouseLeave
        Try
            Dim tmpDBL As Single = 0
            Single.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar3_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbStar3.MouseMove
        Try
            If e.X < 12 Then
                Me.pbStar3.Tag = 5
                Me.BuildStars(5)
            Else
                Me.pbStar3.Tag = 6
                Me.BuildStars(6)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar4_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar4.MouseLeave
        Try
            Dim tmpDBL As Single = 0
            Single.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar4_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbStar4.MouseMove
        Try
            If e.X < 12 Then
                Me.pbStar4.Tag = 7
                Me.BuildStars(7)
            Else
                Me.pbStar4.Tag = 8
                Me.BuildStars(8)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar5_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar5.MouseLeave
        Try
            Dim tmpDBL As Single = 0
            Single.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar5_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles pbStar5.MouseMove
        Try
            If e.X < 12 Then
                Me.pbStar5.Tag = 9
                Me.BuildStars(9)
            Else
                Me.pbStar5.Tag = 10
                Me.BuildStars(10)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar1.Click
        Me.tmpRating = Me.pbStar1.Tag.ToString
    End Sub

    Private Sub pbStar2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar2.Click
        Me.tmpRating = Me.pbStar2.Tag.ToString
    End Sub

    Private Sub pbStar3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar3.Click
        Me.tmpRating = Me.pbStar3.Tag.ToString
    End Sub

    Private Sub pbStar4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar4.Click
        Me.tmpRating = Me.pbStar4.Tag.ToString
    End Sub

    Private Sub pbStar5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar5.Click
        Me.tmpRating = Me.pbStar5.Tag.ToString
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Me.DeleteActors()
    End Sub

    Private Sub DeleteActors()
        Try
            If Me.lvActors.Items.Count > 0 Then
                While Me.lvActors.SelectedItems.Count > 0
                    Me.lvActors.Items.Remove(Me.lvActors.SelectedItems(0))
                End While
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Private Sub EditActor()
        Try
            Dim lvwItem As ListViewItem = Me.lvActors.SelectedItems(0)
            Dim eActor As New MediaContainers.Person With {.Name = lvwItem.Text, .Role = lvwItem.SubItems(1).Text, .Thumb = lvwItem.SubItems(2).Text}
            Using dAddEditActor As New dlgAddEditActor
                eActor = dAddEditActor.ShowDialog(False, eActor)
            End Using
            If Not IsNothing(eActor) Then
                lvwItem.Text = eActor.Name
                lvwItem.SubItems(1).Text = eActor.Role
                lvwItem.SubItems(2).Text = eActor.Thumb
                lvwItem.Selected = True
                lvwItem.EnsureVisible()
            End If
            eActor = Nothing
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub lvActors_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvActors.ColumnClick
        ' Determine if the clicked column is already the column that is 
        ' being sorted.
        Try
            If (e.Column = Me.lvwActorSorter.SortColumn) Then
                ' Reverse the current sort direction for this column.
                If (Me.lvwActorSorter.Order = SortOrder.Ascending) Then
                    Me.lvwActorSorter.Order = SortOrder.Descending
                Else
                    Me.lvwActorSorter.Order = SortOrder.Ascending
                End If
            Else
                ' Set the column number that is to be sorted; default to ascending.
                Me.lvwActorSorter.SortColumn = e.Column
                Me.lvwActorSorter.Order = SortOrder.Ascending
            End If

            ' Perform the sort with these new sort options.
            Me.lvActors.Sort()
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManual.Click

        Try
            If dlgManualEdit.ShowDialog(Master.currMovie.NfoPath) = Windows.Forms.DialogResult.OK Then
                Master.currMovie.Movie = NFO.LoadMovieFromNFO(Master.currMovie.NfoPath, Master.currMovie.isSingle)
                Me.FillInfo(False)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnAddActor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddActor.Click
        Try
            Dim eActor As New MediaContainers.Person
            Using dAddEditActor As New dlgAddEditActor
                eActor = dAddEditActor.ShowDialog(True)
            End Using
            If Not IsNothing(eActor) Then
                Dim lvItem As ListViewItem = Me.lvActors.Items.Add(eActor.Name)
                lvItem.SubItems.Add(eActor.Role)
                lvItem.SubItems.Add(eActor.Thumb)
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnEditActor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditActor.Click
        EditActor()
    End Sub

    Private Sub btnClearCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearCache.Click
        Try
            If Directory.Exists(CachePath) Then
                FileUtils.Delete.DeleteDirectory(CachePath)
            End If

            btnClearCache.Visible = False
        Catch ex As Exception
            MsgBox(Master.eLang.GetString(267, "One or more cache resources is currently in use and cannot be deleted at the moment."), MsgBoxStyle.Information, Master.eLang.GetString(268, "Cannot Clear Cache"))
        End Try
    End Sub

    Private Sub FillInfo(Optional ByVal DoAll As Boolean = True)
        Try
            With Me
                If String.IsNullOrEmpty(Master.currMovie.NfoPath) Then
                    .btnManual.Enabled = False
                End If

                Me.chkMark.Checked = Master.currMovie.IsMark

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Title) Then
                    .txtTitle.Text = Master.currMovie.Movie.Title
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.SortTitle) Then
                    If Master.currMovie.Movie.SortTitle <> StringUtils.FilterTokens(Master.currMovie.Movie.Title) Then
                        .txtSortTitle.Text = Master.currMovie.Movie.SortTitle
                    End If
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Tagline) Then
                    .txtTagline.Text = Master.currMovie.Movie.Tagline
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Year) Then
                    .mtxtYear.Text = Master.currMovie.Movie.Year
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Votes) Then
                    .txtVotes.Text = Master.currMovie.Movie.Votes
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Outline) Then
                    .txtOutline.Text = Master.currMovie.Movie.Outline
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Plot) Then
                    .txtPlot.Text = Master.currMovie.Movie.Plot
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Top250) Then
                    .txtTop250.Text = Master.currMovie.Movie.Top250
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Runtime) Then
                    .txtRuntime.Text = Master.currMovie.Movie.Runtime
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.ReleaseDate) Then
                    .txtReleaseDate.Text = Master.currMovie.Movie.ReleaseDate
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Director) Then
                    .txtDirector.Text = Master.currMovie.Movie.Director
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Credits) Then
                    .txtCredits.Text = Master.currMovie.Movie.Credits
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Certification) Then
                    If Not String.IsNullOrEmpty(Master.eSettings.CertificationLang) Then
                        Dim lCert() As String = Master.currMovie.Movie.Certification.Trim.Split(Convert.ToChar("/"))
                        Dim fCert = From eCert In lCert Where Regex.IsMatch(eCert, String.Concat(Regex.Escape(Master.eSettings.CertificationLang), "\:(.*?)"))
                        If fCert.Count > 0 Then
                            .txtCerts.Text = fCert(0).ToString.Trim
                        Else
                            .txtCerts.Text = Master.currMovie.Movie.Certification
                        End If
                    Else
                        .txtCerts.Text = Master.currMovie.Movie.Certification
                    End If
                End If

                Me.lblLocalTrailer.Visible = Not String.IsNullOrEmpty(Master.currMovie.TrailerPath)
                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Trailer) Then
                    .txtTrailer.Text = Master.currMovie.Movie.Trailer
                Else
                    If String.IsNullOrEmpty(Master.currMovie.TrailerPath) Then
                        .btnPlayTrailer.Enabled = False
                    End If
                End If

                .btnDLTrailer.Enabled = Master.eSettings.DownloadTrailers

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Studio) Then
                    .txtStudio.Text = Master.currMovie.Movie.Studio
                End If

                Me.SelectMPAA()

                For i As Integer = 0 To .lbGenre.Items.Count - 1
                    .lbGenre.SetItemChecked(i, False)
                Next
                If Not String.IsNullOrEmpty(Master.currMovie.Movie.Genre) Then
                    Dim genreArray() As String
                    genreArray = Strings.Split(Master.currMovie.Movie.Genre, " / ")
                    For g As Integer = 0 To UBound(genreArray)
                        If .lbGenre.FindString(genreArray(g).Trim) > 0 Then
                            .lbGenre.SetItemChecked(.lbGenre.FindString(genreArray(g).Trim), True)
                        End If
                    Next

                    If .lbGenre.CheckedItems.Count = 0 Then
                        .lbGenre.SetItemChecked(0, True)
                    End If
                Else
                    .lbGenre.SetItemChecked(0, True)
                End If


                Dim lvItem As ListViewItem
                .lvActors.Items.Clear()
                For Each imdbAct As MediaContainers.Person In Master.currMovie.Movie.Actors
                    lvItem = .lvActors.Items.Add(imdbAct.Name)
                    lvItem.SubItems.Add(imdbAct.Role)
                    lvItem.SubItems.Add(imdbAct.Thumb)
                Next

                Dim tRating As Single = NumUtils.ConvertToSingle(Master.currMovie.Movie.Rating)
                .tmpRating = tRating.ToString
                .pbStar1.Tag = tRating
                .pbStar2.Tag = tRating
                .pbStar3.Tag = tRating
                .pbStar4.Tag = tRating
                .pbStar5.Tag = tRating
                If tRating > 0 Then .BuildStars(tRating)

                If DoAll Then

                    If Not Master.currMovie.isSingle Then
                        TabControl1.TabPages.Remove(TabPage4)
                        TabControl1.TabPages.Remove(TabPage5)
                    Else
                        Dim pExt As String = Path.GetExtension(Master.currMovie.Filename).ToLower
                        If pExt = ".rar" OrElse pExt = ".iso" OrElse pExt = ".img" OrElse _
                        pExt = ".bin" OrElse pExt = ".cue" OrElse pExt = ".dat" Then
                            TabControl1.TabPages.Remove(TabPage4)
                        Else
                            .bwThumbs.RunWorkerAsync()
                        End If
                    End If

                    Fanart.FromFile(Master.currMovie.FanartPath)
                    If Not IsNothing(Fanart.Image) Then
                        .pbFanart.Image = Fanart.Image

                        .lblFanartSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), .pbFanart.Image.Width, .pbFanart.Image.Height)
                        .lblFanartSize.Visible = True
                    End If

                    Poster.FromFile(Master.currMovie.PosterPath)
                    If Not IsNothing(Poster.Image) Then
                        .pbPoster.Image = Poster.Image

                        .lblPosterSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), .pbPoster.Image.Width, .pbPoster.Image.Height)
                        .lblPosterSize.Visible = True
                    End If

                    If Not Master.eSettings.UseTMDB Then
                        .btnSetFanartScrape.Enabled = False
                        If Not Master.eSettings.UseIMPA AndAlso Not Master.eSettings.UseMPDB Then
                            .btnSetPosterScrape.Enabled = False
                        End If
                    End If

                    If Master.eSettings.AutoThumbs > 0 Then
                        .txtThumbCount.Text = Master.eSettings.AutoThumbs.ToString
                    End If
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Movie.IMDBID) AndAlso Master.eSettings.UseImgCache Then
                    CachePath = String.Concat(Master.TempPath, Path.DirectorySeparatorChar, Master.currMovie.Movie.IMDBID.Replace("tt", String.Empty))
                    If Directory.Exists(CachePath) Then
                        Me.btnClearCache.Visible = True
                    End If
                End If
            End With
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetInfo()
        Try
            With Me

                Master.currMovie.IsMark = Me.chkMark.Checked

                If Not String.IsNullOrEmpty(.txtTitle.Text) Then
                    If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(.mtxtYear.Text.Trim) Then
                        Master.currMovie.ListTitle = String.Format("{0} ({1})", StringUtils.FilterTokens(.txtTitle.Text.Trim), .mtxtYear.Text.Trim)
                    Else
                        Master.currMovie.ListTitle = StringUtils.FilterTokens(.txtTitle.Text.Trim)
                    End If
                    Master.currMovie.Movie.Title = .txtTitle.Text.Trim
                End If

                If Not String.IsNullOrEmpty(.txtSortTitle.Text) Then
                    Master.currMovie.Movie.SortTitle = .txtSortTitle.Text.Trim
                Else
                    Master.currMovie.Movie.SortTitle = StringUtils.FilterTokens(.txtTitle.Text.Trim)
                End If

                Master.currMovie.Movie.Tagline = .txtTagline.Text.Trim
                Master.currMovie.Movie.Year = .mtxtYear.Text.Trim
                Master.currMovie.Movie.Votes = .txtVotes.Text.Trim
                Master.currMovie.Movie.Outline = .txtOutline.Text.Trim
                Master.currMovie.Movie.Plot = .txtPlot.Text.Trim
                Master.currMovie.Movie.Top250 = .txtTop250.Text.Trim
                Master.currMovie.Movie.Director = .txtDirector.Text.Trim

                Master.currMovie.Movie.Certification = .txtCerts.Text.Trim

                If .lbMPAA.SelectedIndices.Count > 0 AndAlso Not .lbMPAA.SelectedIndex <= 0 Then
                    Master.currMovie.Movie.MPAA = String.Concat(.lbMPAA.SelectedItem.ToString, " ", .txtMPAADesc.Text).Trim
                Else
                    If Master.eSettings.UseCertForMPAA AndAlso (Not Master.eSettings.CertificationLang = "USA" OrElse (Master.eSettings.CertificationLang = "USA" AndAlso .lbMPAA.SelectedIndex = 0)) Then
                        Dim lCert() As String = .txtCerts.Text.Trim.Split(Convert.ToChar("/"))
                        Dim fCert = From eCert In lCert Where Regex.IsMatch(eCert, String.Concat(Regex.Escape(Master.eSettings.CertificationLang), "\:(.*?)"))
                        If fCert.Count > 0 Then
                            Master.currMovie.Movie.MPAA = If(Master.eSettings.CertificationLang = "USA", StringUtils.USACertToMPAA(fCert(0).ToString.Trim), fCert(0).ToString.Trim)
                        Else
                            Master.currMovie.Movie.MPAA = String.Empty
                        End If
                    Else
                        Master.currMovie.Movie.MPAA = String.Empty
                    End If
                End If

                Master.currMovie.Movie.Rating = .tmpRating
                Master.currMovie.Movie.Runtime = .txtRuntime.Text.Trim
                Master.currMovie.Movie.ReleaseDate = .txtReleaseDate.Text.Trim
                Master.currMovie.Movie.Credits = .txtCredits.Text.Trim
                Master.currMovie.Movie.Trailer = .txtTrailer.Text.Trim
                Master.currMovie.Movie.Studio = .txtStudio.Text.Trim

                If .lbGenre.CheckedItems.Count > 0 Then

                    If .lbGenre.CheckedIndices.Contains(0) Then
                        Master.currMovie.Movie.Genre = String.Empty
                    Else
                        Dim strGenre As String = String.Empty
                        Dim isFirst As Boolean = True
                        Dim iChecked = From iCheck In .lbGenre.CheckedItems
                        strGenre = Strings.Join(iChecked.ToArray, " / ")
                        Master.currMovie.Movie.Genre = strGenre.Trim
                    End If
                End If

                Master.currMovie.Movie.Actors.Clear()

                If .lvActors.Items.Count > 0 Then
                    For Each lviActor As ListViewItem In .lvActors.Items
                        Dim addActor As New MediaContainers.Person
                        addActor.Name = lviActor.Text.Trim
                        addActor.Role = lviActor.SubItems(1).Text.Trim
                        addActor.Thumb = lviActor.SubItems(2).Text.Trim

                        Master.currMovie.Movie.Actors.Add(addActor)
                    Next
                End If

                If Master.currMovie.ClearExtras Then
                    .Fanart.DeleteFanart(Master.currMovie)
                    .Poster.DeletePosters(Master.currMovie)
                End If

                If Not IsNothing(.Fanart.Image) Then
                    Dim fPath As String = .Fanart.SaveAsFanart(Master.currMovie)
                    Master.currMovie.FanartPath = fPath
                Else
                    .Fanart.DeleteFanart(Master.currMovie)
                    Master.currMovie.FanartPath = String.Empty
                End If

                If Not IsNothing(.Poster.Image) Then
                    Dim pPath As String = .Poster.SaveAsPoster(Master.currMovie)
                    Master.currMovie.PosterPath = pPath
                Else
                    .Poster.DeletePosters(Master.currMovie)
                    Master.currMovie.PosterPath = String.Empty
                End If

                If Not Master.eSettings.NoSaveImagesToNfo AndAlso pResults.Posters.Count > 0 Then Master.currMovie.Movie.Thumb = pResults.Posters
                If Not Master.eSettings.NoSaveImagesToNfo AndAlso fResults.Fanart.Thumb.Count > 0 Then Master.currMovie.Movie.Fanart = pResults.Fanart

                .SaveExtraThumbsList()

                .TransferETs()

            End With
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub BuildStars(ByVal sinRating As Single)

        '//
        ' Convert # rating to star images
        '\\

        Try
            'f'in MS and them leaving control arrays out of VB.NET
            With Me
                .pbStar1.Image = Nothing
                .pbStar2.Image = Nothing
                .pbStar3.Image = Nothing
                .pbStar4.Image = Nothing
                .pbStar5.Image = Nothing

                If sinRating >= 0.5 Then ' if rating is less than .5 out of ten, consider it a 0
                    Select Case (sinRating / 2)
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
                        Case Else
                            .pbStar1.Image = My.Resources.star
                            .pbStar2.Image = My.Resources.star
                            .pbStar3.Image = My.Resources.star
                            .pbStar4.Image = My.Resources.star
                            .pbStar5.Image = My.Resources.star
                    End Select
                End If
            End With
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetPoster.Click
        Try
            With ofdImage
                .InitialDirectory = Directory.GetParent(Master.currMovie.Filename).FullName
                .Filter = "Supported Images(*.jpg, *.jpeg, *.tbn)|*.jpg;*.jpeg;*.tbn|jpeg (*.jpg, *.jpeg)|*.jpg;*.jpeg|tbn (*.tbn)|*.tbn"
                .FilterIndex = 0
            End With

            If ofdImage.ShowDialog() = DialogResult.OK Then
                Poster.FromFile(ofdImage.FileName)
                pbPoster.Image = Poster.Image

                Me.lblPosterSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), Me.pbPoster.Image.Width, Me.pbPoster.Image.Height)
                Me.lblPosterSize.Visible = True
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetFanart.Click
        Try
            With ofdImage
                .InitialDirectory = Directory.GetParent(Master.currMovie.Filename).FullName
                .Filter = "JPEGs|*.jpg"
                .FilterIndex = 4
            End With

            If ofdImage.ShowDialog() = DialogResult.OK Then
                Fanart.FromFile(ofdImage.FileName)
                pbFanart.Image = Fanart.Image

                Me.lblFanartSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), Me.pbFanart.Image.Width, Me.pbFanart.Image.Height)
                Me.lblFanartSize.Visible = True
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetPosterScrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetPosterScrape.Click
        Try
            Dim sPath As String = Path.Combine(Master.TempPath, "poster.jpg")

            ModulesManager.Instance.ScraperSelectImageOfType(Master.currMovie, Enums.ImageType.Posters, pResults, True)
            If Not String.IsNullOrEmpty(pResults.ImagePath) Then
                Poster.FromFile(sPath)
                pbPoster.Image = Poster.Image
                Me.lblPosterSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), Me.pbPoster.Image.Width, Me.pbPoster.Image.Height)
                Me.lblPosterSize.Visible = True
            End If

            If Master.eSettings.UseImgCache AndAlso Directory.Exists(CachePath) Then
                Me.btnClearCache.Visible = True
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetFanartScrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetFanartScrape.Click
        Try
            Dim sPath As String = Path.Combine(Master.TempPath, "fanart.jpg")

            ModulesManager.Instance.ScraperSelectImageOfType(Master.currMovie, Enums.ImageType.Fanart, fResults, True)
            If Not String.IsNullOrEmpty(fResults.ImagePath) Then
                Fanart.FromFile(sPath)
                pbFanart.Image = Fanart.Image

                Me.lblFanartSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), Me.pbFanart.Image.Width, Me.pbFanart.Image.Height)
                Me.lblFanartSize.Visible = True
            End If

            If Master.eSettings.UseImgCache AndAlso Directory.Exists(CachePath) Then
                Me.btnClearCache.Visible = True
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnFrameLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrameLoad.Click
        Try
            Using ffmpeg As New Process()

                ffmpeg.StartInfo.FileName = Functions.GetFFMpeg
                ffmpeg.StartInfo.Arguments = String.Format("-ss 0 -i ""{0}"" -an -f rawvideo -vframes 1 -s 1280x720 -vcodec mjpeg -y ""{1}""", Master.currMovie.Filename, Path.Combine(Master.TempPath, "frame.jpg"))
                ffmpeg.EnableRaisingEvents = False
                ffmpeg.StartInfo.UseShellExecute = False
                ffmpeg.StartInfo.CreateNoWindow = True
                ffmpeg.StartInfo.RedirectStandardOutput = True
                ffmpeg.StartInfo.RedirectStandardError = True
                ffmpeg.Start()
                Using d As StreamReader = ffmpeg.StandardError

                    Do
                        Dim s As String = d.ReadLine()
                        If s.Contains("Duration: ") Then
                            Dim sTime As String = Regex.Match(s, "Duration: (?<dur>.*?),").Groups("dur").ToString
                            If Not sTime = "N/A" Then
                                Dim ts As TimeSpan = CDate(CDate(String.Format("{0} {1}", DateTime.Today.ToString("d"), sTime))).Subtract(CDate(DateTime.Today))
                                Dim intSeconds As Integer = ((ts.Hours * 60) + ts.Minutes) * 60 + ts.Seconds
                                tbFrame.Maximum = intSeconds
                            Else
                                tbFrame.Maximum = 0
                            End If
                            tbFrame.Value = 0
                            tbFrame.Enabled = True
                        End If
                    Loop While Not d.EndOfStream
                End Using
                ffmpeg.WaitForExit()
                ffmpeg.Close()
            End Using

            If tbFrame.Maximum > 0 AndAlso File.Exists(Path.Combine(Master.TempPath, "frame.jpg")) Then
                Using fsFImage As New FileStream(Path.Combine(Master.TempPath, "frame.jpg"), FileMode.Open, FileAccess.Read)
                    pbFrame.Image = Image.FromStream(fsFImage)
                End Using
                btnFrameLoad.Enabled = False
                btnFrameSave.Enabled = True
            Else
                tbFrame.Maximum = 0
                tbFrame.Value = 0
                tbFrame.Enabled = False
                pbFrame.Image = Nothing
            End If
            PreviousFrameValue = 0

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            tbFrame.Maximum = 0
            tbFrame.Value = 0
            tbFrame.Enabled = False
            pbFrame.Image = Nothing
        End Try
    End Sub

    Private Sub tbFrame_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tbFrame.KeyUp
        If tbFrame.Value <> PreviousFrameValue Then
            GrabTheFrame()
        End If
    End Sub

    Private Sub tbFrame_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tbFrame.MouseUp
        If tbFrame.Value <> PreviousFrameValue Then
            GrabTheFrame()
        End If
    End Sub

    Private Sub tbFrame_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbFrame.Scroll
        Try
            Dim sec2Time As New TimeSpan(0, 0, tbFrame.Value)
            lblTime.Text = String.Format("{0}:{1:00}:{2:00}", sec2Time.Hours, sec2Time.Minutes, sec2Time.Seconds)

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub GrabTheFrame()
        Try

            tbFrame.Enabled = False
            Dim ffmpeg As New Process()

            ffmpeg.StartInfo.FileName = Functions.GetFFMpeg
            ffmpeg.StartInfo.Arguments = String.Format("-ss {0} -i ""{1}"" -an -f rawvideo -vframes 1 -vcodec mjpeg -y ""{2}""", tbFrame.Value, Master.currMovie.Filename, Path.Combine(Master.TempPath, "frame.jpg"))
            ffmpeg.EnableRaisingEvents = False
            ffmpeg.StartInfo.UseShellExecute = False
            ffmpeg.StartInfo.CreateNoWindow = True
            ffmpeg.StartInfo.RedirectStandardOutput = True
            ffmpeg.StartInfo.RedirectStandardError = True

            pnlFrameProgress.Visible = True
            btnFrameSave.Enabled = False

            ffmpeg.Start()

            ffmpeg.WaitForExit()
            ffmpeg.Close()

            If File.Exists(Path.Combine(Master.TempPath, "frame.jpg")) Then
                Using fsFImage As FileStream = New FileStream(Path.Combine(Master.TempPath, "frame.jpg"), FileMode.Open, FileAccess.Read)
                    pbFrame.Image = Image.FromStream(fsFImage)
                End Using
                tbFrame.Enabled = True
                btnFrameSave.Enabled = True
                pnlFrameProgress.Visible = False
                PreviousFrameValue = tbFrame.Value
            Else
                lblTime.Text = String.Empty
                tbFrame.Maximum = 0
                tbFrame.Value = 0
                tbFrame.Enabled = False
                btnFrameSave.Enabled = False
                btnFrameLoad.Enabled = True
                pbFrame.Image = Nothing
                pnlFrameProgress.Visible = False
                PreviousFrameValue = tbFrame.Value
            End If

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            PreviousFrameValue = 0
            lblTime.Text = String.Empty
            tbFrame.Maximum = 0
            tbFrame.Value = 0
            tbFrame.Enabled = False
            btnFrameSave.Enabled = False
            btnFrameLoad.Enabled = True
            pbFrame.Image = Nothing
        End Try
    End Sub

    Private Sub btnFrameSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrameSave.Click
        Try
            Dim tPath As String = Path.Combine(Master.TempPath, "extrathumbs")

            If Not Directory.Exists(tPath) Then
                Directory.CreateDirectory(tPath)
            End If

            Dim iMod As Integer = Functions.GetExtraModifier(tPath)

            Dim exImage As New Images
            exImage.ResizeExtraThumb(Path.Combine(Master.TempPath, "frame.jpg"), Path.Combine(tPath, String.Concat("thumb", (iMod + 1), ".jpg")))
            exImage.Dispose()
            exImage = Nothing

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        btnFrameSave.Enabled = False
    End Sub

    Private Sub LoadThumbs()
        Dim tPath As String = Path.Combine(Directory.GetParent(Master.currMovie.Filename).FullName, "extrathumbs")
        If Directory.Exists(tPath) Then
            Dim di As New DirectoryInfo(tPath)
            Dim lFI As New List(Of FileInfo)
            Dim i As Integer = 0
            Try
                Try
                    lFI.AddRange(di.GetFiles("thumb*.jpg"))
                Catch
                End Try

                If lFI.Count > 0 Then
                    For Each thumb As FileInfo In lFI.OrderBy(Function(t) Convert.ToInt32(Regex.Match(t.Name, "(\d+)").Groups(0).ToString))
                        If Not Me.DeleteList.Contains(thumb.Name) Then
                            Using fsImage As New FileStream(thumb.FullName, FileMode.Open, FileAccess.Read)
                                Thumbs.Add(New ExtraThumbs With {.Image = Image.FromStream(fsImage), .Name = thumb.Name, .Index = i, .Path = thumb.FullName})
                                ilThumbs.Images.Add(thumb.Name, Thumbs.Item(i).Image)
                            End Using
                            i += 1
                        End If
                    Next
                End If
            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
            lFI = Nothing
            di = Nothing
        End If
    End Sub

    Private Sub lvThumbs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvThumbs.KeyDown
        If e.KeyCode = Keys.Delete Then Me.DeleteExtraThumbs()
    End Sub

    Private Sub lvThumbs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvThumbs.SelectedIndexChanged
        If Me.lvThumbs.SelectedIndices.Count > 0 Then
            Try
                Me.pbExtraThumbs.Image = Me.Thumbs.Item(Convert.ToInt32(Me.lvThumbs.SelectedItems(0).Tag)).Image
                Me.ExtraIndex = Convert.ToInt32(Me.lvThumbs.SelectedItems(0).Tag)
                Me.btnSetAsFanart.Enabled = True
            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Sub


    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            If lvThumbs.Items.Count > 0 AndAlso lvThumbs.SelectedIndices(0) > 0 Then
                Dim iIndex As Integer = lvThumbs.SelectedIndices(0)
                lvThumbs.Items(iIndex).Text = String.Concat("  ", CStr(Convert.ToInt32(lvThumbs.Items(iIndex).Text.Trim) - 1))
                lvThumbs.Items(iIndex - 1).Text = String.Concat("  ", CStr(Convert.ToInt32(lvThumbs.Items(iIndex - 1).Text.Trim) + 1))
                lvThumbs.Sort()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Try
            If lvThumbs.Items.Count > 0 AndAlso lvThumbs.SelectedIndices(0) < (lvThumbs.Items.Count - 1) Then
                Dim iIndex As Integer = lvThumbs.SelectedIndices(0)
                lvThumbs.Items(iIndex).Text = String.Concat("  ", CStr(Convert.ToInt32(lvThumbs.Items(iIndex).Text.Trim) + 1))
                lvThumbs.Items(iIndex + 1).Text = String.Concat("  ", CStr(Convert.ToInt32(lvThumbs.Items(iIndex + 1).Text.Trim) - 1))
                lvThumbs.Sort()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SaveExtraThumbsList()

        Dim tPath As String = String.Empty
        Try
            If Master.eSettings.VideoTSParent AndAlso FileUtils.Common.isVideoTS(Master.currMovie.Filename) Then
                tPath = Path.Combine(Directory.GetParent(Directory.GetParent(Master.currMovie.Filename).FullName).FullName, "extrathumbs")
            ElseIf Master.eSettings.VideoTSParent AndAlso FileUtils.Common.isBDRip(Master.currMovie.Filename) Then
                tPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Master.currMovie.Filename).FullName).FullName).FullName, "extrathumbs")
            Else
                tPath = Path.Combine(Directory.GetParent(Master.currMovie.Filename).FullName, "extrathumbs")
            End If

            If Master.currMovie.ClearExtras AndAlso Not hasCleared Then
                FileUtils.Delete.DeleteDirectory(tPath)
                hasCleared = True
            Else
                'first delete the ones from the delete list
                For Each del As String In DeleteList
                    File.Delete(Path.Combine(tPath, del))
                Next

                'now name the rest something arbitrary so we don't get any conflicts
                For Each lItem As ListViewItem In lvThumbs.Items
                    FileSystem.Rename(Path.Combine(tPath, lItem.Name), Path.Combine(tPath, String.Concat("temp", lItem.Name)))
                Next

                'now rename them properly
                For Each lItem As ListViewItem In lvThumbs.Items
                    FileSystem.Rename(Path.Combine(tPath, String.Concat("temp", lItem.Name)), Path.Combine(tPath, String.Concat("thumb", lItem.Text.Trim, ".jpg")))
                Next
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub btnRemoveThumb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveThumb.Click
        Me.DeleteExtraThumbs()
    End Sub

    Private Sub DeleteExtraThumbs()
        Try
            Dim iIndex As Integer = 0
            While Me.lvThumbs.SelectedItems.Count > 0
                iIndex = lvThumbs.SelectedItems(0).Index
                DeleteList.Add(lvThumbs.Items(iIndex).Name)
                lvThumbs.Items.Remove(lvThumbs.SelectedItems(0))
                pbExtraThumbs.Image = Nothing
                btnSetAsFanart.Enabled = False
            End While
            RenumberThumbs()
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RenumberThumbs()
        For i As Integer = 0 To lvThumbs.Items.Count - 1
            lvThumbs.Items(i).Text = String.Concat("  ", CStr(i + 1))
            lvThumbs.Sort()
        Next
    End Sub

    Private Sub bwThumbs_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwThumbs.DoWork
        If Not Master.currMovie.ClearExtras OrElse hasCleared Then LoadThumbs()
    End Sub

    Private Sub bwThumbs_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwThumbs.RunWorkerCompleted
        Try
            Dim lItem As ListViewItem
            If Thumbs.Count > 0 Then
                For Each thumb As ExtraThumbs In Thumbs
                    lItem = lvThumbs.Items.Add(thumb.Name, String.Concat("  ", CStr(thumb.Index + 1)), thumb.Name)
                    lItem.Tag = thumb.Index
                Next
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub LoadGenres()

        '//
        ' Read all the genres from the xml and load into the list
        '\\

        Me.lbGenre.Items.Add(Master.eLang.None)

        Me.lbGenre.Items.AddRange(APIXML.GetGenreList)

    End Sub

    Private Sub LoadRatings()

        '//
        ' Read all the ratings from the xml and load into the list
        '\\

        Me.lbMPAA.Items.Add(Master.eLang.None)

        Me.lbMPAA.Items.AddRange(APIXML.GetRatingList)

    End Sub

    Private Sub SelectMPAA()
        If Not String.IsNullOrEmpty(Master.currMovie.Movie.MPAA) Then
            Try
                If Master.eSettings.UseCertForMPAA AndAlso Not Master.eSettings.CertificationLang = "USA" AndAlso APIXML.RatingXML.Element("ratings").Element(Master.eSettings.CertificationLang.ToLower).Descendants("movie").Count > 0 Then
                    Dim l As Integer = Me.lbMPAA.FindString(Strings.Trim(Master.currMovie.Movie.MPAA))
                    Me.lbMPAA.SelectedIndex = l
                    If Me.lbMPAA.SelectedItems.Count = 0 Then
                        Me.lbMPAA.SelectedIndex = 0
                    End If

                    Me.lbMPAA.TopIndex = 0

                    txtMPAADesc.Enabled = False
                ElseIf Me.lbMPAA.Items.Count >= 6 Then
                    Dim strMPAA As String = Master.currMovie.Movie.MPAA
                    If strMPAA.ToLower.StartsWith("rated g") Then
                        Me.lbMPAA.SelectedIndex = 1
                    ElseIf strMPAA.ToLower.StartsWith("rated pg-13") Then
                        Me.lbMPAA.SelectedIndex = 3
                    ElseIf strMPAA.ToLower.StartsWith("rated pg") Then
                        Me.lbMPAA.SelectedIndex = 2
                    ElseIf strMPAA.ToLower.StartsWith("rated r") Then
                        Me.lbMPAA.SelectedIndex = 4
                    ElseIf strMPAA.ToLower.StartsWith("rated nc-17") Then
                        Me.lbMPAA.SelectedIndex = 5
                    Else
                        Me.lbMPAA.SelectedIndex = 0
                    End If

                    If Me.lbMPAA.SelectedIndex > 0 AndAlso Not String.IsNullOrEmpty(strMPAA) Then
                        Dim strMPAADesc As String = strMPAA
                        strMPAADesc = Strings.Replace(strMPAADesc, "rated g", String.Empty, 1, -1, CompareMethod.Text).Trim
                        If Not String.IsNullOrEmpty(strMPAADesc) Then strMPAADesc = Strings.Replace(strMPAADesc, "rated pg-13", String.Empty, 1, -1, CompareMethod.Text).Trim
                        If Not String.IsNullOrEmpty(strMPAADesc) Then strMPAADesc = Strings.Replace(strMPAADesc, "rated pg", String.Empty, 1, -1, CompareMethod.Text).Trim
                        If Not String.IsNullOrEmpty(strMPAADesc) Then strMPAADesc = Strings.Replace(strMPAADesc, "rated r", String.Empty, 1, -1, CompareMethod.Text).Trim
                        If Not String.IsNullOrEmpty(strMPAADesc) Then strMPAADesc = Strings.Replace(strMPAADesc, "rated nc-17", String.Empty, 1, -1, CompareMethod.Text).Trim
                        txtMPAADesc.Text = strMPAADesc
                    End If
                End If

            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            Me.lbMPAA.SelectedIndex = 0
        End If
    End Sub

    Private Sub btnRemovePoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemovePoster.Click
        Me.pbPoster.Image = Nothing
        Me.Poster.Image = Nothing
    End Sub

    Private Sub btnRemoveFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveFanart.Click
        Me.pbFanart.Image = Nothing
        Me.Fanart.Image = Nothing
    End Sub

    Private Sub btnAutoGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAutoGen.Click
        Try
            If Convert.ToInt32(txtThumbCount.Text) > 0 Then
                pnlFrameProgress.Visible = True
                Me.Refresh()
                ThumbGenerator.CreateRandomThumbs(Master.currMovie, Convert.ToInt32(txtThumbCount.Text), True)
                pnlFrameProgress.Visible = False
                Me.RefreshExtraThumbs()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub txtThumbCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtThumbCount.GotFocus
        Me.AcceptButton = Me.btnAutoGen
    End Sub

    Private Sub txtThumbCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtThumbCount.KeyPress
        e.Handled = StringUtils.NumericOnly(e.KeyChar)
    End Sub

    Private Sub txtThumbCount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtThumbCount.LostFocus
        Me.AcceptButton = Me.OK_Button
    End Sub

    Private Sub txtThumbCount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtThumbCount.TextChanged
        btnAutoGen.Enabled = Not String.IsNullOrEmpty(txtThumbCount.Text)
    End Sub

    Private Sub RefreshExtraThumbs()
        Try
            Thumbs.Clear()
            lvThumbs.Clear()
            ilThumbs.Images.Clear()
            Me.bwThumbs.RunWorkerAsync()
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnThumbsRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThumbsRefresh.Click
        Me.RefreshExtraThumbs()
    End Sub

    Private Sub btnStudio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudio.Click
        Using dStudio As New dlgStudioSelect
            Dim tStudio As String = dStudio.ShowDialog(Master.currMovie.Movie.IMDBID)
            If Not String.IsNullOrEmpty(tStudio) Then
                Me.txtStudio.Text = tStudio
            End If
        End Using
    End Sub

    Private Sub lbGenre_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lbGenre.ItemCheck
        If e.Index = 0 Then
            For i As Integer = 1 To lbGenre.Items.Count - 1
                Me.lbGenre.SetItemChecked(i, False)
            Next
        Else
            Me.lbGenre.SetItemChecked(0, False)
        End If
    End Sub

    Private Sub btnSetAsFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetAsFanart.Click
        Me.Fanart.FromFile(Me.Thumbs.Item(Me.ExtraIndex).Path)
        Me.pbFanart.Image = pbExtraThumbs.Image
        Me.lblFanartSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), Me.pbFanart.Image.Width, Me.pbFanart.Image.Height)
        Me.btnSetAsFanart.Enabled = False
    End Sub

    Private Sub btnPlayTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPlayTrailer.Click
        Try

            Dim tPath As String = String.Empty

            If Not String.IsNullOrEmpty(Master.currMovie.TrailerPath) Then
                tPath = String.Concat("""", Master.currMovie.TrailerPath, """")
            ElseIf Not String.IsNullOrEmpty(Me.txtTrailer.Text) Then
                tPath = String.Concat("""", Me.txtTrailer.Text, """")
            End If

            If Not String.IsNullOrEmpty(tPath) Then
                If Master.isWindows Then
                    Process.Start(tPath)
                Else
                    Using Explorer As New Process
                        Explorer.StartInfo.FileName = "xdg-open"
                        Explorer.StartInfo.Arguments = tPath
                        Explorer.Start()
                    End Using
                End If
            End If

        Catch
            MsgBox(Master.eLang.GetString(270, "The trailer could not be played. This could be due to an invalid URI or you do not have the proper player to play the trailer type."), MsgBoxStyle.Critical, Master.eLang.GetString(271, "Error Playing Trailer"))
        End Try
    End Sub

    Private Sub btnDLTrailer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDLTrailer.Click
        Dim tURL As String = ModulesManager.Instance.ScraperDownloadTrailer(Master.currMovie)
        If Not String.IsNullOrEmpty(tURL) Then
            Me.btnPlayTrailer.Enabled = True
            If tURL.Substring(0, 7) = "http://" Then
                Me.txtTrailer.Text = tURL
            Else
                Master.currMovie.TrailerPath = tURL
                Me.lblLocalTrailer.Visible = True
            End If
        End If
    End Sub

    Private Sub txtTrailer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTrailer.TextChanged
        If StringUtils.isValidURL(txtTrailer.Text) Then
            Me.btnPlayTrailer.Enabled = True
        Else
            Me.btnPlayTrailer.Enabled = False
        End If
    End Sub

    Private Sub btnTransferNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTransferNow.Click
        Me.TransferETs()
        Me.RefreshExtraThumbs()
        Me.pnlETQueue.Visible = False
    End Sub

    Private Sub TransferETs()
        Try
            If Directory.Exists(Path.Combine(Master.TempPath, "extrathumbs")) Then
                Dim ePath As String = String.Empty
                If Master.eSettings.VideoTSParent AndAlso FileUtils.Common.isVideoTS(Master.currMovie.Filename) Then
                    ePath = Path.Combine(Directory.GetParent(Directory.GetParent(Master.currMovie.Filename).FullName).FullName, "extrathumbs")
                ElseIf Master.eSettings.VideoTSParent AndAlso FileUtils.Common.isBDRip(Master.currMovie.Filename) Then
                    ePath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(Master.currMovie.Filename).FullName).FullName).FullName, "extrathumbs")
                Else
                    ePath = Path.Combine(Directory.GetParent(Master.currMovie.Filename).FullName, "extrathumbs")
                End If

                If Master.currMovie.ClearExtras AndAlso Not hasCleared Then
                    FileUtils.Delete.DeleteDirectory(ePath)
                    hasCleared = True
                End If

                Dim iMod As Integer = Functions.GetExtraModifier(ePath)
                Dim iVal As Integer = iMod + 1
                Dim hasET As Boolean = Not iMod = 0
                Dim fList As New List(Of String)

                Try
                    fList.AddRange(Directory.GetFiles(Path.Combine(Master.TempPath, "extrathumbs"), "thumb*.jpg"))
                Catch
                End Try

                If fList.Count > 0 Then

                    If Not hasET Then
                        Directory.CreateDirectory(ePath)
                    End If

                    For Each sFile As String In fList
                        FileUtils.Common.MoveFileWithStream(sFile, Path.Combine(ePath, String.Concat("thumb", iVal, ".jpg")))
                        iVal += 1
                    Next
                End If

                Master.currMovie.ExtraPath = ePath

                FileUtils.Delete.DeleteDirectory(Path.Combine(Master.TempPath, "extrathumbs"))
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        Try
            If TabControl1.SelectedIndex = 3 Then
                If File.Exists(String.Concat(Master.TempPath, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    Me.pnlETQueue.Visible = True
                Else
                    Me.pnlETQueue.Visible = False
                End If
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub CleanUp()
        Try
            If File.Exists(Path.Combine(Master.TempPath, "poster.jpg")) Then
                File.Delete(Path.Combine(Master.TempPath, "poster.jpg"))
            End If

            If File.Exists(Path.Combine(Master.TempPath, "fanart.jpg")) Then
                File.Delete(Path.Combine(Master.TempPath, "fanart.jpg"))
            End If

            If File.Exists(Path.Combine(Master.TempPath, "frame.jpg")) Then
                File.Delete(Path.Combine(Master.TempPath, "frame.jpg"))
            End If

            If Directory.Exists(Path.Combine(Master.TempPath, "extrathumbs")) Then
                FileUtils.Delete.DeleteDirectory(Path.Combine(Master.TempPath, "extrathumbs"))
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetPosterDL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetPosterDL.Click
        Try
            Using dImgManual As New dlgImgManual
                If dImgManual.ShowDialog(Enums.ImageType.Posters) = DialogResult.OK Then
                    Poster.FromFile(Path.Combine(Master.TempPath, "poster.jpg"))
                    pbPoster.Image = Poster.Image

                    Me.lblPosterSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), Me.pbPoster.Image.Width, Me.pbPoster.Image.Height)
                    Me.lblPosterSize.Visible = True
                End If
            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetFanartDL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetFanartDL.Click
        Try
            Using dImgManual As New dlgImgManual
                If dImgManual.ShowDialog(Enums.ImageType.Fanart) = DialogResult.OK Then
                    Fanart.FromFile(Path.Combine(Master.TempPath, "fanart.jpg"))
                    pbFanart.Image = Fanart.Image

                    Me.lblFanartSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), Me.pbFanart.Image.Width, Me.pbFanart.Image.Height)
                    Me.lblFanartSize.Visible = True
                End If
            End Using
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Friend Class ExtraThumbs
        Private _image As Image
        Private _name As String
        Private _index As Integer
        Private _path As String

        Friend Property Image() As Image
            Get
                Return _image
            End Get
            Set(ByVal value As Image)
                _image = value
            End Set
        End Property

        Friend Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Friend Property Index() As Integer
            Get
                Return _index
            End Get
            Set(ByVal value As Integer)
                _index = value
            End Set
        End Property

        Friend Property Path() As String
            Get
                Return _path
            End Get
            Set(ByVal value As String)
                _path = value
            End Set
        End Property

        Friend Sub New()
            Clear()
        End Sub

        Private Sub Clear()
            _image = Nothing
            _name = String.Empty
            _index = Nothing
            _path = String.Empty
        End Sub
    End Class

    Private Sub dlgEditMovie_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub lvActors_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvActors.DoubleClick
        EditActor()
    End Sub

    Private Sub lvActors_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvActors.KeyDown
        If e.KeyCode = Keys.Delete Then Me.DeleteActors()
    End Sub

    Private Sub DelayTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DelayTimer.Tick
        DelayTimer.Stop()
        GrabTheFrame()
    End Sub

    Private Sub btnActorUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActorUp.Click
        Try
            If Me.lvActors.Items.Count > 0 AndAlso Not IsNothing(Me.lvActors.SelectedItems(0)) AndAlso Me.lvActors.SelectedIndices(0) > 0 Then
                Dim iIndex As Integer = Me.lvActors.SelectedIndices(0)
                Me.lvActors.Items.Insert(iIndex - 1, DirectCast(Me.lvActors.SelectedItems(0).Clone, ListViewItem))
                Me.lvActors.Items.RemoveAt(iIndex + 1)
                Me.lvActors.Items(iIndex - 1).Selected = True
                Me.lvActors.Select()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnActorDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActorDown.Click
        If Me.lvActors.Items.Count > 0 AndAlso Not IsNothing(Me.lvActors.SelectedItems(0)) AndAlso Me.lvActors.SelectedIndices(0) < (Me.lvActors.Items.Count - 1) Then
            Dim iIndex As Integer = Me.lvActors.SelectedIndices(0)
            Me.lvActors.Items.Insert(iIndex + 2, DirectCast(Me.lvActors.SelectedItems(0).Clone, ListViewItem))
            Me.lvActors.Items.RemoveAt(iIndex)
            Me.lvActors.Items(iIndex + 1).Selected = True
            Me.lvActors.Select()
        End If
    End Sub
End Class
