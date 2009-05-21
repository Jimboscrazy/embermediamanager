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
Imports System
Imports System.IO
Imports System.Text.RegularExpressions

Public Class dlgEditMovie
    Friend WithEvents bwThumbs As New System.ComponentModel.BackgroundWorker
    Private lvwColumnSorter As ListViewColumnSorter
    Private tmpRating As String = String.Empty
    Private Poster As New Images With {.IsEdit = True}
    Private Fanart As New Images With {.IsEdit = True}
    Private Thumbs As New List(Of ExtraThumbs)
    Private DeleteList As New ArrayList

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Me.SetInfo()
            Master.SaveMovieToNFO(Master.currMovie, Master.currPath, Master.isFile)

            Me.Poster.Clear()
            Me.Poster = Nothing

            Me.Fanart.Clear()
            Me.Fanart = Nothing
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        If Directory.Exists(Path.Combine(Application.StartupPath, "Temp")) Then
            Directory.Delete(Path.Combine(Application.StartupPath, "Temp"), True)
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgEditMovie_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.lvwColumnSorter = New ListViewColumnSorter()
            Me.lvActors.ListViewItemSorter = Me.lvwColumnSorter

            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Dim g As Graphics = Graphics.FromImage(iBackground)
            g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, 20), pnlTop.ClientRectangle)
            Me.pnlTop.BackgroundImage = iBackground
            g.Dispose()

            Me.LoadGenres()

            Me.FillInfo()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar1.MouseLeave
        Try
            Dim tmpDBL As Double = 0
            Double.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar2_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar2.MouseLeave
        Try
            Dim tmpDBL As Double = 0
            Double.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar3_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar3.MouseLeave
        Try
            Dim tmpDBL As Double = 0
            Double.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar4_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar4.MouseLeave
        Try
            Dim tmpDBL As Double = 0
            Double.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar5_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles pbStar5.MouseLeave
        Try
            Dim tmpDBL As Double = 0
            Double.TryParse(Me.tmpRating, tmpDBL)
            Me.BuildStars(tmpDBL)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub pbStar1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar1.Click
        Me.tmpRating = Me.pbStar1.Tag
    End Sub

    Private Sub pbStar2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar2.Click
        Me.tmpRating = Me.pbStar2.Tag
    End Sub

    Private Sub pbStar3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar3.Click
        Me.tmpRating = Me.pbStar3.Tag
    End Sub

    Private Sub pbStar4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar4.Click
        Me.tmpRating = Me.pbStar4.Tag
    End Sub

    Private Sub pbStar5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbStar5.Click
        Me.tmpRating = Me.pbStar5.Tag
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Try
            If Me.lvActors.Items.Count > 0 Then
                For i As Integer = Me.lvActors.SelectedItems.Count - 1 To 0 Step -1
                    Me.lvActors.Items.Remove(Me.lvActors.SelectedItems(i))
                    Me.lvActors.Enabled = True
                Next
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub lvActors_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvActors.ColumnClick
        ' Determine if the clicked column is already the column that is 
        ' being sorted.
        Try
            If (e.Column = Me.lvwColumnSorter.SortColumn) Then
                ' Reverse the current sort direction for this column.
                If (Me.lvwColumnSorter.Order = SortOrder.Ascending) Then
                    Me.lvwColumnSorter.Order = SortOrder.Descending
                Else
                    Me.lvwColumnSorter.Order = SortOrder.Ascending
                End If
            Else
                ' Set the column number that is to be sorted; default to ascending.
                Me.lvwColumnSorter.SortColumn = e.Column
                Me.lvwColumnSorter.Order = SortOrder.Ascending
            End If

            ' Perform the sort with these new sort options.
            Me.lvActors.Sort()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManual.Click
        Try
            If dlgManualEdit.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Master.currMovie = Master.LoadMovieFromNFO(Master.currNFO)
                Me.FillInfo()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnAddActor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddActor.Click
        Try
            Dim eActor As New Media.Person
            Dim dAddEditActor As New dlgAddEditActor
            eActor = dAddEditActor.ShowDialog(True)
            dAddEditActor.Dispose()
            If Not IsNothing(eActor) Then
                Dim lvItem As ListViewItem = Me.lvActors.Items.Add(eActor.Name)
                lvItem.SubItems.Add(eActor.Role)
                lvItem.SubItems.Add(eActor.Thumb)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnEditActor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditActor.Click
        Try
            Dim lvwItem As ListViewItem = Me.lvActors.SelectedItems(0)
            Dim eActor As New Media.Person With {.Name = lvwItem.Text, .Role = lvwItem.SubItems(1).Text, .Thumb = lvwItem.SubItems(2).Text}
            Dim dAddEditActor As New dlgAddEditActor
            eActor = dAddEditActor.ShowDialog(False, eActor)
            dAddEditActor.Dispose()
            If Not IsNothing(eActor) Then
                lvwItem.Text = eActor.Name
                lvwItem.SubItems(1).Text = eActor.Role
                lvwItem.SubItems(2).Text = eActor.Thumb
                lvwItem.Selected = True
                lvwItem.EnsureVisible()
            End If
            eActor = Nothing
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub FillInfo()
        Try
            With Me
                .bwThumbs.RunWorkerAsync()

                If Not String.IsNullOrEmpty(Master.currMovie.Title) Then
                    .txtTitle.Text = Master.currMovie.Title
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Tagline) Then
                    .txtTagline.Text = Master.currMovie.Tagline
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Year) Then
                    .mtxtYear.Text = Master.currMovie.Year
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Votes) Then
                    .txtVotes.Text = Master.currMovie.Votes
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Outline) Then
                    .txtOutline.Text = Master.currMovie.Outline
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Plot) Then
                    .txtPlot.Text = Master.currMovie.Plot
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Top250) Then
                    .txtTop250.Text = Master.currMovie.Top250
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Runtime) Then
                    .txtRuntime.Text = Master.currMovie.Runtime
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.ReleaseDate) Then
                    .txtReleaseDate.Text = Master.currMovie.ReleaseDate
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Director) Then
                    .txtDirector.Text = Master.currMovie.Director
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Credits) Then
                    .txtCredits.Text = Master.currMovie.Credits
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Certification) Then
                    .txtCerts.Text = Master.currMovie.Certification
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Trailer) Then
                    .txtTrailer.Text = Master.currMovie.Trailer
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.StudioReal) Then
                    .txtStudio.Text = Master.currMovie.StudioReal
                ElseIf Not String.IsNullOrEmpty(Master.currMovie.Studio) Then
                    If Strings.InStr(Master.currMovie.Studio, "/") Then
                        Master.currMovie.StudioReal = Strings.Left(Master.currMovie.Studio, Strings.InStr(Master.currMovie.Studio, "/") - 1).Trim
                        .txtStudio.Text = Master.currMovie.StudioReal
                    End If
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Studio) Then
                    If Strings.InStr(Master.currMovie.Studio, "/") Then
                        .txtStudioTag.Text = Strings.Right(Master.currMovie.Studio, Master.currMovie.Studio.Length - Strings.InStr(Master.currMovie.Studio, "/")).Trim
                    End If
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.MPAA) Then

                    Dim strMPAA As String = Master.currMovie.MPAA
                    If Strings.InStr(strMPAA.ToLower, "rated g") > 0 Then
                        .lbMPAA.SelectedIndex = 1
                    ElseIf Strings.InStr(strMPAA.ToLower, "rated pg-13") > 0 Then
                        .lbMPAA.SelectedIndex = 3
                    ElseIf Strings.InStr(strMPAA.ToLower, "rated pg") > 0 Then
                        .lbMPAA.SelectedIndex = 2
                    ElseIf Strings.InStr(strMPAA.ToLower, "rated r") > 0 Then
                        .lbMPAA.SelectedIndex = 4
                    ElseIf Strings.InStr(strMPAA.ToLower, "rated nc-17") > 0 Then
                        .lbMPAA.SelectedIndex = 5
                    Else
                        .lbMPAA.SelectedIndex = 0
                    End If

                    Dim strMPAADesc As String = strMPAA
                    strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated g", String.Empty, 1, -1, CompareMethod.Text))
                    strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated pg-13", String.Empty, 1, -1, CompareMethod.Text))
                    strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated pg", String.Empty, 1, -1, CompareMethod.Text))
                    strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated r", String.Empty, 1, -1, CompareMethod.Text))
                    strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated nc-17", String.Empty, 1, -1, CompareMethod.Text))
                    txtMPAADesc.Text = strMPAADesc
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Genre) Then
                    Dim genreArray() As String

                    genreArray = Strings.Split(Master.currMovie.Genre, "/")
                    For g As Integer = 0 To (genreArray.Length - 1)
                        Dim l As Integer = .lbGenre.FindString(Strings.Trim(genreArray(g)))
                        .lbGenre.SelectedIndex = l
                    Next

                    If .lbGenre.SelectedItems.Count = 0 Then
                        .lbGenre.SelectedIndex = 0
                    End If

                    .lbGenre.TopIndex = 0
                End If


                Dim lvItem As ListViewItem
                For Each imdbAct As Media.Person In Master.currMovie.Actors
                    lvItem = .lvActors.Items.Add(imdbAct.Name)
                    lvItem.SubItems.Add(imdbAct.Role)
                    lvItem.SubItems.Add(imdbAct.Thumb)
                Next

                If Not String.IsNullOrEmpty(Master.currMovie.Rating) AndAlso IsNumeric(Master.currMovie.Rating) AndAlso CDbl(Master.currMovie.Rating) > 0 Then
                    Dim currRating As String = Master.currMovie.Rating
                    .tmpRating = currRating
                    .BuildStars(CDbl(currRating))
                    .pbStar1.Tag = currRating
                    .pbStar2.Tag = currRating
                    .pbStar3.Tag = currRating
                    .pbStar4.Tag = currRating
                    .pbStar5.Tag = currRating
                Else
                    Dim currRating As String = Master.currMovie.Rating
                    .tmpRating = "0"
                    .pbStar1.Tag = "0"
                    .pbStar2.Tag = "0"
                    .pbStar3.Tag = "0"
                    .pbStar4.Tag = "0"
                    .pbStar5.Tag = "0"
                End If

                Fanart.Load(Master.currPath, Master.isFile, Master.ImageType.Fanart)
                If Not IsNothing(Fanart.Image) Then
                    .pbFanart.Image = Fanart.Image

                    .lblFanartSize.Text = String.Format("Size: {0}x{1}", .pbFanart.Image.Width, .pbFanart.Image.Height)
                    .lblFanartSize.Visible = True
                End If

                Poster.Load(Master.currPath, Master.isFile, Master.ImageType.Posters)
                If Not IsNothing(Poster.Image) Then
                    .pbPoster.Image = Poster.Image

                    .lblPosterSize.Text = String.Format("Size: {0}x{1}", .pbPoster.Image.Width, .pbPoster.Image.Height)
                    .lblPosterSize.Visible = True
                End If

                If Not Master.eSettings.UseTMDB Then
                    .btnSetFanartScrape.Enabled = False
                    If Not Master.eSettings.UseIMPA Then
                        .btnSetPosterScrape.Enabled = False
                    End If
                End If

                .chkMark.Checked = Master.currMark
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetInfo()
        Try
            With Me
                Master.currMovie.Title = .txtTitle.Text
                Master.currMovie.Tagline = .txtTagline.Text
                Master.currMovie.Year = .mtxtYear.Text
                Master.currMovie.Votes = .txtVotes.Text
                Master.currMovie.Outline = .txtOutline.Text
                Master.currMovie.Plot = .txtPlot.Text
                Master.currMovie.Top250 = .txtTop250.Text
                Master.currMovie.Director = .txtDirector.Text

                If .lbMPAA.SelectedIndices.Count > 0 Then
                    If .lbMPAA.SelectedIndex = 0 Then
                        Master.currMovie.MPAA = String.Empty
                    Else
                        Master.currMovie.MPAA = Strings.Trim(.lbMPAA.SelectedItem.ToString & " " & .txtMPAADesc.Text)
                    End If
                Else
                    Master.currMovie.MPAA = String.Empty
                End If

                Master.currMovie.Rating = .tmpRating
                Master.currMovie.Runtime = .txtRuntime.Text
                Master.currMovie.Certification = .txtCerts.Text
                Master.currMovie.ReleaseDate = .txtReleaseDate.Text
                Master.currMovie.Credits = .txtCredits.Text
                Master.currMovie.Trailer = .txtTrailer.Text
                Master.currMovie.StudioReal = .txtStudio.Text
                Master.currMovie.Studio = String.Format("{0} / {1}", .txtStudio.Text.Trim, .txtStudioTag.Text.Trim)

                If .lbGenre.SelectedIndices.Count > 0 Then

                    If .lbGenre.SelectedIndices.Contains(0) Then
                        Master.currMovie.Genre = String.Empty
                    Else
                        Dim strGenre As String = String.Empty
                        Dim isFirst As Boolean = True
                        Dim Selected = From Sel In .lbGenre.SelectedItems
                        strGenre = Strings.Join(Selected.ToArray, " / ")
                        'For Each strSelected As String In .lbGenre.SelectedItems
                        '    If isFirst Then
                        '        strGenre = strSelected
                        '        isFirst = False
                        '    Else
                        '        strGenre += " / " & strSelected
                        '    End If
                        'Next
                        Master.currMovie.Genre = strGenre
                    End If
                End If

                Master.currMovie.Actors.Clear()

                If .lvActors.Items.Count > 0 Then
                    For Each lviActor As ListViewItem In .lvActors.Items
                        Dim addActor As New Media.Person
                        addActor.Name = lviActor.Text
                        addActor.Role = lviActor.SubItems(1).Text
                        addActor.Thumb = lviActor.SubItems(2).Text

                        Master.currMovie.Actors.Add(addActor)
                    Next
                End If

                If Not IsNothing(Fanart.Image) Then
                    .Fanart.SaveAsFanart(Master.currPath, Master.isFile)
                End If

                If Not IsNothing(Poster.Image) Then
                    .Poster.SaveAsPoster(Master.currPath, Master.isFile)
                End If

                .SaveExtraThumbsList()

                If Directory.Exists(Path.Combine(Application.StartupPath, "Temp")) Then
                    If Directory.Exists(String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Temp", Path.DirectorySeparatorChar, "extrathumbs")) Then
                        Dim di As New DirectoryInfo(String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Temp", Path.DirectorySeparatorChar, "extrathumbs"))
                        Dim ePath As String = Path.Combine(Directory.GetParent(Master.currPath).FullName, "extrathumbs")

                        If Not Directory.Exists(ePath) Then
                            Directory.CreateDirectory(ePath)
                        End If

                        'we need to recheck which thumbs we already have 
                        'again in case user made changes to the order of
                        'extrathumbs after downloading new extrathumbs
                        Dim iMod As Integer = Master.GetExtraModifier(ePath)
                        Dim fList As New List(Of FileInfo)
                        fList.AddRange(di.GetFiles("thumb*.jpg"))

                        For i As Integer = 0 To fList.Count - 1
                            Master.MoveFileWithStream(fList.Item(i).FullName, Path.Combine(ePath, String.Concat("thumb", i + iMod, ".jpg")))
                        Next
                    End If
                    Directory.Delete(Path.Combine(Application.StartupPath, "Temp"), True)
                End If

                Master.currMark = chkMark.Checked
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub BuildStars(ByVal dblRating As Double)

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

    Private Sub btnSetPoster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetPoster.Click
        Try
            With ofdImage
                .InitialDirectory = Directory.GetParent(Master.currPath).FullName
                .Filter = "Supported Images(*.jpg, *.jpeg, *.tbn)|*.jpg;*.jpeg;*.tbn|jpeg (*.jpg, *.jpeg)|*.jpg;*.jpeg|tbn (*.tbn)|*.tbn"
                .FilterIndex = 0
            End With

            If ofdImage.ShowDialog() = DialogResult.OK Then
                Poster.FromFile(ofdImage.FileName)
                pbPoster.Image = Poster.Image

                Me.lblPosterSize.Text = String.Format("Size: {0}x{1}", Me.pbPoster.Image.Width, Me.pbPoster.Image.Height)
                Me.lblPosterSize.Visible = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetFanart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetFanart.Click
        Try
            With ofdImage
                .InitialDirectory = Directory.GetParent(Master.currPath).FullName
                .Filter = "JPEGs|*.jpg"
                .FilterIndex = 4
            End With

            If ofdImage.ShowDialog() = DialogResult.OK Then
                Fanart.FromFile(ofdImage.FileName)
                pbFanart.Image = Fanart.Image

                Me.lblFanartSize.Text = String.Format("Size: {0}x{1}", Me.pbFanart.Image.Width, Me.pbFanart.Image.Height)
                Me.lblFanartSize.Visible = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetPosterScrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetPosterScrape.Click
        Dim sPath As String
        Try
            sPath = Path.Combine(Application.StartupPath, "Temp")

            If Not Directory.Exists(sPath) Then
                Directory.CreateDirectory(sPath)
            End If

            sPath = Path.Combine(sPath, "poster.jpg")

            Dim dImgSelect As New dlgImgSelect
            If dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Posters, True) = Windows.Forms.DialogResult.OK Then

                Poster.FromFile(sPath)
                pbPoster.Image = Poster.Image

                Me.lblPosterSize.Text = String.Format("Size: {0}x{1}", Me.pbPoster.Image.Width, Me.pbPoster.Image.Height)
                Me.lblPosterSize.Visible = True
            End If
            dImgSelect.Dispose()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetFanartScrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetFanartScrape.Click
        Dim sPath As String
        Try
            sPath = Path.Combine(Application.StartupPath, "Temp")

            If Not Directory.Exists(sPath) Then
                Directory.CreateDirectory(sPath)
            End If

            sPath = Path.Combine(sPath, "fanart.jpg")

            Dim dImgSelect As New dlgImgSelect
            If dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Fanart, True) = Windows.Forms.DialogResult.OK Then

                Fanart.FromFile(sPath)
                pbFanart.Image = Fanart.Image

                Me.lblFanartSize.Text = String.Format("Size: {0}x{1}", Me.pbFanart.Image.Width, Me.pbFanart.Image.Height)
                Me.lblFanartSize.Visible = True

            End If
            dImgSelect.Dispose()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnFrameLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrameLoad.Click
        Try
            Dim ffmpeg As New Process()
            Dim tPath As String = Path.Combine(Application.StartupPath, "Temp")
            Dim duration As Single = 0.0F, current As Single = 0.0F

            If Not Directory.Exists(tPath) Then
                Directory.CreateDirectory(tPath)
            End If

            ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
            ffmpeg.StartInfo.Arguments = String.Format("-ss 0 -i ""{0}"" -an -f rawvideo -vframes 1 -s 1280x720 -vcodec mjpeg -y ""{1}""", Master.currPath, Path.Combine(tPath, "frame.jpg"))
            ffmpeg.EnableRaisingEvents = False
            ffmpeg.StartInfo.UseShellExecute = False
            ffmpeg.StartInfo.CreateNoWindow = True
            ffmpeg.StartInfo.RedirectStandardOutput = True
            ffmpeg.StartInfo.RedirectStandardError = True
            ffmpeg.Start()
            Dim d As StreamReader = ffmpeg.StandardError
            Do
                Dim s As String = d.ReadLine()
                If s.Contains("Duration: ") Then
                    Dim sTime As String = Regex.Match(s, "Duration: (?<dur>.*?),").Groups("dur").ToString
                    Dim ts As TimeSpan = CDate(CDate(DateTime.Today & " " & sTime)).Subtract(CDate(DateTime.Today))
                    Dim intSeconds As Integer = ((ts.Hours * 60) + ts.Minutes) * 60 + ts.Seconds
                    tbFrame.Maximum = intSeconds
                    tbFrame.Value = 0
                    tbFrame.Enabled = True
                End If
            Loop While Not d.EndOfStream

            ffmpeg.WaitForExit()
            ffmpeg.Close()
            ffmpeg.Dispose()

            If File.Exists(Path.Combine(tPath, "frame.jpg")) Then
                Dim fsFImage As New FileStream(Path.Combine(tPath, "frame.jpg"), FileMode.Open, FileAccess.Read)
                pbFrame.Image = Image.FromStream(fsFImage)
                fsFImage.Close()
                fsFImage.Dispose()
                btnFrameLoad.Enabled = False
                btnFrameSave.Enabled = True
            Else
                tbFrame.Maximum = 0
                tbFrame.Value = 0
                tbFrame.Enabled = False
                pbFrame.Image = Nothing
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            tbFrame.Maximum = 0
            tbFrame.Value = 0
            tbFrame.Enabled = False
            pbFrame.Image = Nothing
        End Try
    End Sub

    Private Sub tbFrame_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbFrame.Scroll
        Try
            Dim sec2Time As New TimeSpan(0, 0, tbFrame.Value)
            lblTime.Text = String.Format("{0}:{1:00}:{2:00}", sec2Time.Hours, sec2Time.Minutes, sec2Time.Seconds)

            btnGrab.Enabled = True
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnGrab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrab.Click
        Try
            btnGrab.Enabled = False

            Dim ffmpeg As New Process()
            Dim tPath As String = Path.Combine(Application.StartupPath, "Temp")
            Dim duration As Single = 0.0F, current As Single = 0.0F

            If Not Directory.Exists(tPath) Then
                Directory.CreateDirectory(tPath)
            End If

            ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
            ffmpeg.StartInfo.Arguments = String.Format("-ss {0} -i ""{1}"" -an -f rawvideo -vframes 1 -s 1280x720 -vcodec mjpeg -y ""{2}""", tbFrame.Value, Master.currPath, Path.Combine(tPath, "frame.jpg"))
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
            ffmpeg.Dispose()

            If File.Exists(Path.Combine(tPath, "frame.jpg")) Then
                Dim fsFImage As FileStream = New FileStream(Path.Combine(tPath, "frame.jpg"), FileMode.Open, FileAccess.Read)
                pbFrame.Image = Image.FromStream(fsFImage)
                fsFImage.Close()
                fsFImage.Dispose()
                btnFrameSave.Enabled = True
            Else
                tbFrame.Maximum = 0
                tbFrame.Value = 0
                tbFrame.Enabled = False
                btnFrameSave.Enabled = False
                btnFrameLoad.Enabled = True
                pbFrame.Image = Nothing
            End If

            pnlFrameProgress.Visible = False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            tbFrame.Maximum = 0
            tbFrame.Value = 0
            tbFrame.Enabled = False
            btnFrameSave.Enabled = False
            btnFrameLoad.Enabled = True
            pbFrame.Image = Nothing
            btnGrab.Enabled = False
        End Try
    End Sub

    Private Sub btnFrameSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrameSave.Click
        Try
            Dim tPath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Temp", Path.DirectorySeparatorChar, "frame.jpg")
            Dim sPath As String = Path.Combine(Directory.GetParent(Master.currPath).FullName, "extrathumbs")

            If Not Directory.Exists(sPath) Then
                Directory.CreateDirectory(sPath)
            End If

            Dim iMod As Integer = Master.GetExtraModifier(Master.currPath)

            Master.MoveFileWithStream(tPath, Path.Combine(sPath, String.Concat("thumb", (iMod + 1), ".jpg")))

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        btnFrameSave.Enabled = False
    End Sub

    Private Sub LoadThumbs()
        Dim tPath As String = Path.Combine(Directory.GetParent(Master.currPath).FullName, "extrathumbs")
        If Directory.Exists(tPath) Then
            Dim di As New DirectoryInfo(tPath)
            Dim i As Integer = 0
            Try
                For Each thumb As FileInfo In di.GetFiles("thumb*.jpg")
                    Dim fsImage As New FileStream(thumb.FullName, FileMode.Open, FileAccess.Read)
                    Thumbs.Add(New ExtraThumbs With {.Image = Image.FromStream(fsImage), .Name = thumb.Name, .Index = i})
                    ilThumbs.Images.Add(thumb.Name, Thumbs.Item(i).Image)
                    fsImage.Close()
                    fsImage = Nothing
                    i += 1
                Next
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Sub

    Private Sub lvThumbs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvThumbs.SelectedIndexChanged
        If lvThumbs.SelectedIndices.Count > 0 Then
            Try
                pbExtraThumbs.Image = Thumbs.Item(lvThumbs.SelectedIndices(0)).Image
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Sub


    Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click
        Try
            If lvThumbs.Items.Count > 0 AndAlso lvThumbs.SelectedIndices(0) > 0 Then
                Dim iIndex As Integer = lvThumbs.SelectedIndices(0)
                lvThumbs.Items(iIndex).Text = String.Concat("  ", CStr(CInt(lvThumbs.Items(iIndex).Text.Trim) - 1))
                lvThumbs.Items(iIndex - 1).Text = String.Concat("  ", CStr(CInt(lvThumbs.Items(iIndex - 1).Text.Trim) + 1))
                lvThumbs.Sort()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click
        Try
            If lvThumbs.Items.Count > 0 AndAlso lvThumbs.SelectedIndices(0) < (lvThumbs.Items.Count - 1) Then
                Dim iIndex As Integer = lvThumbs.SelectedIndices(0)
                lvThumbs.Items(iIndex).Text = String.Concat("  ", CStr(CInt(lvThumbs.Items(iIndex).Text.Trim) + 1))
                lvThumbs.Items(iIndex + 1).Text = String.Concat("  ", CStr(CInt(lvThumbs.Items(iIndex + 1).Text.Trim) - 1))
                lvThumbs.Sort()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SaveExtraThumbsList()
        Dim tPath As String = Path.Combine(Directory.GetParent(Master.currPath).FullName, "extrathumbs")

        'first delete the ones from the delete list
        Try
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
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub btnRemoveThumb_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveThumb.Click
        Try
            DeleteList.Add(lvThumbs.SelectedItems(0).Name)
            lvThumbs.Items.Remove(lvThumbs.SelectedItems(0))
            RenumberThumbs()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub RenumberThumbs()
        For i As Integer = 0 To lvThumbs.Items.Count - 1
            lvThumbs.Items(i).Text = String.Concat("  ", CStr(i + 1))
            lvThumbs.Sort()
        Next
    End Sub

    Private Sub bwThumbs_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwThumbs.DoWork
        LoadThumbs()
    End Sub

    Private Sub bwThumbs_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwThumbs.RunWorkerCompleted
        Try
            If Thumbs.Count > 0 Then
                For Each thumb As ExtraThumbs In Thumbs
                    lvThumbs.Items.Add(thumb.Name, String.Concat("  ", CStr(thumb.Index + 1)), thumb.Name)
                Next
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub LoadGenres()

        '//
        ' Read all the genres from the xml and load into the list
        '\\

        Me.lbGenre.Items.Add("[none]")

        Dim mePath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Genres")

        If File.Exists(Path.Combine(mePath, "Genres.xml")) Then
            Try
                Dim xmlGenre As XDocument = XDocument.Load(Path.Combine(mePath, "Genres.xml"))

                Dim xGenre = From xGen In xmlGenre...<name> Select xGen.@searchstring
                If xGenre.Count > 0 Then
                    For Each strGenre As String In xGenre
                        Me.lbGenre.Items.Add(strGenre)
                    Next
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            MsgBox("Cannot find Genres.xml." & vbNewLine & vbNewLine & "Expected path:" & vbNewLine & Path.Combine(mePath, "Genres.xml"), MsgBoxStyle.Critical, "File Not Found")
        End If

    End Sub

    Friend Class ExtraThumbs
        Private _image As Image
        Private _name As String
        Private _index As Integer

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

        Friend Property Index() As String
            Get
                Return _index
            End Get
            Set(ByVal value As String)
                _index = value
            End Set
        End Property

        Friend Sub New()
            Clear()
        End Sub

        Private Sub Clear()
            _image = Nothing
            _name = String.Empty
            _index = Nothing
        End Sub
    End Class
End Class
