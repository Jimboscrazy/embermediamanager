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
    Private lvwActorSorter As ListViewColumnSorter
    Private lvwThumbSorter As ListViewColumnSorter
    Private tmpRating As String = String.Empty
    Private Poster As New Images With {.IsEdit = True}
    Private Fanart As New Images With {.IsEdit = True}
    Private Thumbs As New List(Of ExtraThumbs)
    Private DeleteList As New ArrayList
    Private ExtraIndex As Integer = 0
    Private _id As Integer
    Private CachePath As String = String.Empty

    Public Overloads Function ShowDialog(ByVal id As Integer) As Windows.Forms.DialogResult

        '//
        ' Overload to pass data
        '\\

        Me._id = id

        Return MyBase.ShowDialog()
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Me.SetInfo()
            Master.SaveMovieToNFO(Master.currMovie, Master.currPath, Master.isFile)

            Me.Poster.Clear()
            Me.Poster = Nothing

            Me.Fanart.Clear()
            Me.Fanart = Nothing

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
                Directory.Delete(Path.Combine(Master.TempPath, "extrathumbs"), True)
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnRescrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRescrape.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Retry
        Me.Close()
    End Sub

    Private Sub btnChangeMovie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeMovie.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub

    Private Sub dlgEditMovie_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Activate()

            Me.lvwActorSorter = New ListViewColumnSorter()
            Me.lvActors.ListViewItemSorter = Me.lvwActorSorter
            Me.lvwThumbSorter = New ListViewColumnSorter() With {.SortByText = True, .Order = SortOrder.Ascending, .NumericSort = True}
            Me.lvThumbs.ListViewItemSorter = Me.lvwThumbSorter

            Dim iBackground As New Bitmap(Me.pnlTop.Width, Me.pnlTop.Height)
            Using g As Graphics = Graphics.FromImage(iBackground)
                g.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlTop.ClientRectangle, Color.SteelBlue, Color.LightSteelBlue, Drawing2D.LinearGradientMode.Horizontal), pnlTop.ClientRectangle)
                Me.pnlTop.BackgroundImage = iBackground
            End Using
            
            Me.LoadGenres()
            Me.LoadRatings()

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
            Using dAddEditActor As New dlgAddEditActor
                eActor = dAddEditActor.ShowDialog(True)
            End Using
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnClearCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearCache.Click
        If Directory.Exists(CachePath) Then
            Directory.Delete(CachePath, True)
        End If

        btnClearCache.Visible = False
    End Sub

    Private Sub FillInfo()
        Try
            With Me
                If Master.isFile Then
                    TabControl1.TabPages.Remove(TabPage4)
                    TabControl1.TabPages.Remove(TabPage5)
                Else
                    Dim pExt As String = Path.GetExtension(Master.currPath).ToLower
                    If pExt = ".rar" OrElse pExt = ".iso" OrElse pExt = ".img" OrElse _
                    pExt = ".bin" OrElse pExt = ".cue" Then
                        TabControl1.TabPages.Remove(TabPage4)
                    End If
                    .bwThumbs.RunWorkerAsync()
                End If

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
                    If Strings.InStr(Master.currMovie.Studio, " / ") Then
                        Master.currMovie.StudioReal = Strings.Left(Master.currMovie.Studio, Strings.InStr(Master.currMovie.Studio, " / ") - 1).Trim
                        .txtStudio.Text = Master.currMovie.StudioReal
                    End If
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.Studio) Then
                    If Strings.InStr(Master.currMovie.Studio, " / ") Then
                        .txtStudioTag.Text = Strings.Right(Master.currMovie.Studio, Master.currMovie.Studio.Length - (Strings.InStr(Master.currMovie.Studio, " / ") + 2)).Trim
                    End If
                End If

                Me.SelectMPAA()

                If Not String.IsNullOrEmpty(Master.currMovie.Genre) Then
                    Dim genreArray() As String
                    genreArray = Strings.Split(Master.currMovie.Genre, " / ")
                    For g As Integer = 0 To UBound(genreArray)
                        If .lbGenre.FindString(Strings.Trim(genreArray(g))) > 0 Then .lbGenre.SetItemChecked(.lbGenre.FindString(Strings.Trim(genreArray(g))), True)
                    Next

                    If .lbGenre.CheckedItems.Count = 0 Then
                        .lbGenre.SetItemChecked(0, True)
                    End If
                Else
                    .lbGenre.SetItemChecked(0, True)
                End If


                Dim lvItem As ListViewItem
                For Each imdbAct As Media.Person In Master.currMovie.Actors
                    lvItem = .lvActors.Items.Add(imdbAct.Name)
                    lvItem.SubItems.Add(imdbAct.Role)
                    lvItem.SubItems.Add(imdbAct.Thumb)
                Next

                Dim tRating As Double = Master.ConvertToDouble(Master.currMovie.Rating)
                .tmpRating = tRating
                .pbStar1.Tag = tRating
                .pbStar2.Tag = tRating
                .pbStar3.Tag = tRating
                .pbStar4.Tag = tRating
                .pbStar5.Tag = tRating
                If tRating > 0 Then .BuildStars(tRating)

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

                Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                    SQLcommand.CommandText = String.Concat("SELECT mark FROM movies WHERE id = ", Me._id, ";")
                    Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                        .chkMark.Checked = SQLreader("mark")
                    End Using
                End Using

                If Master.eSettings.AutoThumbs > 0 Then
                    .txtThumbCount.Text = Master.eSettings.AutoThumbs
                End If

                If Not String.IsNullOrEmpty(Master.currMovie.IMDBID) Then
                    CachePath = String.Concat(Master.TempPath, Path.DirectorySeparatorChar, Master.currMovie.IMDBID.Replace("tt", String.Empty))
                    If Directory.Exists(CachePath) Then
                        Me.btnClearCache.Visible = True
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetInfo()
        Try
            With Me

                If Not String.IsNullOrEmpty(.txtTitle.Text) Then
                    Master.currMovie.Title = .txtTitle.Text.Trim

                    'reset title in list just in case user changed it (only if Use Title From NFO is selected)
                    If Master.eSettings.UseNameFromNfo Then
                        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
                            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                                Dim parTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTitle", DbType.String, 0, "title")
                                Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                                SQLcommand.CommandText = "UPDATE movies SET title = (?) WHERE id = (?);"
                                parTitle.Value = .txtTitle.Text.Trim
                                parID.Value = Me._id
                                SQLcommand.ExecuteNonQuery()
                            End Using
                            SQLtransaction.Commit()
                        End Using
                    End If
                End If

                Master.currMovie.Tagline = .txtTagline.Text.Trim
                Master.currMovie.Year = .mtxtYear.Text.Trim
                Master.currMovie.Votes = .txtVotes.Text.Trim
                Master.currMovie.Outline = .txtOutline.Text.Trim
                Master.currMovie.Plot = .txtPlot.Text.Trim
                Master.currMovie.Top250 = .txtTop250.Text.Trim
                Master.currMovie.Director = .txtDirector.Text.Trim

                If .lbMPAA.SelectedIndices.Count > 0 Then
                    If .lbMPAA.SelectedIndex = 0 Then
                        Master.currMovie.MPAA = String.Empty
                    Else
                        Master.currMovie.MPAA = String.Concat(.lbMPAA.SelectedItem.ToString, " ", .txtMPAADesc.Text).Trim
                    End If
                Else
                    Master.currMovie.MPAA = String.Empty
                End If

                Master.currMovie.Rating = .tmpRating

                Master.currMovie.Runtime = .txtRuntime.Text.Trim
                Master.currMovie.Certification = .txtCerts.Text.Trim
                Master.currMovie.ReleaseDate = .txtReleaseDate.Text.Trim
                Master.currMovie.Credits = .txtCredits.Text.Trim
                Master.currMovie.Trailer = .txtTrailer.Text.Trim
                Master.currMovie.StudioReal = .txtStudio.Text.Trim
                Master.currMovie.Studio = String.Format("{0} / {1}", .txtStudio.Text.Trim, .txtStudioTag.Text.Trim).Trim

                If .lbGenre.CheckedItems.Count > 0 Then

                    If .lbGenre.CheckedIndices.Contains(0) Then
                        Master.currMovie.Genre = String.Empty
                    Else
                        Dim strGenre As String = String.Empty
                        Dim isFirst As Boolean = True
                        Dim iChecked = From iCheck In .lbGenre.CheckedItems
                        strGenre = Strings.Join(iChecked.ToArray, " / ")
                        Master.currMovie.Genre = strGenre.Trim
                    End If
                End If

                Master.currMovie.Actors.Clear()

                If .lvActors.Items.Count > 0 Then
                    For Each lviActor As ListViewItem In .lvActors.Items
                        Dim addActor As New Media.Person
                        addActor.Name = lviActor.Text.Trim
                        addActor.Role = lviActor.SubItems(1).Text.Trim
                        addActor.Thumb = lviActor.SubItems(2).Text.Trim

                        Master.currMovie.Actors.Add(addActor)
                    Next
                End If

                If Not IsNothing(.Fanart.Image) Then
                    .Fanart.SaveAsFanart(Master.currPath, Master.isFile)
                Else
                    .Fanart.Delete(Master.currPath, Master.isFile, Master.ImageType.Fanart)
                End If

                If Not IsNothing(.Poster.Image) Then
                    .Poster.SaveAsPoster(Master.currPath, Master.isFile)
                Else
                    .Poster.Delete(Master.currPath, Master.isFile, Master.ImageType.Posters)
                End If

                .SaveExtraThumbsList()

                If Directory.Exists(Path.Combine(Master.TempPath, "extrathumbs")) Then
                    Dim di As New DirectoryInfo(Path.Combine(Master.TempPath, "extrathumbs"))
                    Dim ePath As String = Path.Combine(Directory.GetParent(Master.currPath).FullName, "extrathumbs")

                    If Not Directory.Exists(ePath) Then
                        Directory.CreateDirectory(ePath)
                    End If

                    'we need to recheck which thumbs we already have 
                    'again in case user made changes to the order of
                    'extrathumbs after downloading new extrathumbs
                    Dim iMod As Integer = Master.GetExtraModifier(ePath)
                    Dim iVal As Integer = 1
                    If iMod = -1 Then iMod = 0
                    Dim fList As New List(Of FileInfo)
                    fList.AddRange(di.GetFiles("thumb*.jpg"))

                    For i As Integer = 0 To fList.Count - 1
                        Master.MoveFileWithStream(fList.Item(i).FullName, Path.Combine(ePath, String.Concat("thumb", iVal + iMod, ".jpg")))
                        iVal += 1
                    Next
                End If

                Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
                    Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                        Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                        Dim parID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parID", DbType.Int32, 0, "id")
                        SQLcommand.CommandText = "UPDATE movies SET mark = (?) WHERE id = (?);"
                        parMark.Value = Me.chkMark.Checked
                        parID.Value = Me._id
                        SQLcommand.ExecuteNonQuery()
                    End Using
                    SQLtransaction.Commit()
                End Using
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
        Try
            Dim sPath As String = Path.Combine(Master.TempPath, "poster.jpg")

            Using dImgSelect As New dlgImgSelect
                If dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Posters, True) = Windows.Forms.DialogResult.OK Then

                    Poster.FromFile(sPath)
                    pbPoster.Image = Poster.Image

                    Me.lblPosterSize.Text = String.Format("Size: {0}x{1}", Me.pbPoster.Image.Width, Me.pbPoster.Image.Height)
                    Me.lblPosterSize.Visible = True
                End If
            End Using

            If Directory.Exists(CachePath) Then
                Me.btnClearCache.Visible = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnSetFanartScrape_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetFanartScrape.Click
        Try
            Dim sPath As String = Path.Combine(Master.TempPath, "fanart.jpg")

            Using dImgSelect As New dlgImgSelect
                If dImgSelect.ShowDialog(Master.currMovie.IMDBID, Master.currPath, Master.ImageType.Fanart, True) = Windows.Forms.DialogResult.OK Then

                    Fanart.FromFile(sPath)
                    pbFanart.Image = Fanart.Image

                    Me.lblFanartSize.Text = String.Format("Size: {0}x{1}", Me.pbFanart.Image.Width, Me.pbFanart.Image.Height)
                    Me.lblFanartSize.Visible = True

                End If
            End Using

            If Directory.Exists(CachePath) Then
                Me.btnClearCache.Visible = True
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnFrameLoad_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFrameLoad.Click
        Try
            Using ffmpeg As New Process()

                ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
                ffmpeg.StartInfo.Arguments = String.Format("-ss 0 -i ""{0}"" -an -f rawvideo -vframes 1 -s 1280x720 -vcodec mjpeg -y ""{1}""", Master.currPath, Path.Combine(Master.TempPath, "frame.jpg"))
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
                            Dim ts As TimeSpan = CDate(CDate(DateTime.Today & " " & sTime)).Subtract(CDate(DateTime.Today))
                            Dim intSeconds As Integer = ((ts.Hours * 60) + ts.Minutes) * 60 + ts.Seconds
                            tbFrame.Maximum = intSeconds
                            tbFrame.Value = 0
                            tbFrame.Enabled = True
                        End If
                    Loop While Not d.EndOfStream
                End Using
                ffmpeg.WaitForExit()
                ffmpeg.Close()
            End Using

            If File.Exists(Path.Combine(Master.TempPath, "frame.jpg")) Then
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

            Using ffmpeg As New Process()

                ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
                ffmpeg.StartInfo.Arguments = String.Format("-ss {0} -i ""{1}"" -an -f rawvideo -vframes 1 -vcodec mjpeg -y ""{2}""", tbFrame.Value, Master.currPath, Path.Combine(Master.TempPath, "frame.jpg"))
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
            End Using

            If File.Exists(Path.Combine(Master.TempPath, "frame.jpg")) Then
                Using fsFImage As FileStream = New FileStream(Path.Combine(Master.TempPath, "frame.jpg"), FileMode.Open, FileAccess.Read)
                    pbFrame.Image = Image.FromStream(fsFImage)
                End Using
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
            Dim tPath As String = Path.Combine(Master.TempPath, "frame.jpg")
            Dim sPath As String = String.Empty

            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(Master.currPath).Name.ToLower = "video_ts" Then
                sPath = Path.Combine(Directory.GetParent(Directory.GetParent(Master.currPath).FullName).FullName, "extrathumbs")
            Else
                sPath = Path.Combine(Directory.GetParent(Master.currPath).FullName, "extrathumbs")
            End If

            If Not Directory.Exists(sPath) Then
                Directory.CreateDirectory(sPath)
            End If

            Dim iMod As Integer = Master.GetExtraModifier(Master.currPath)

            Dim exImage As New Images
            exImage.ResizeExtraThumb(tPath, Path.Combine(sPath, String.Concat("thumb", (iMod + 1), ".jpg")))
            exImage.Dispose()
            exImage = Nothing

            Me.RefreshExtraThumbs()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        btnFrameSave.Enabled = False
    End Sub

    Private Sub LoadThumbs()
        Dim tPath As String = Path.Combine(Directory.GetParent(Master.currPath).FullName, "extrathumbs")
        If Directory.Exists(tPath) Then
            Dim di As New DirectoryInfo(tPath)
            Dim lFI As New List(Of FileInfo)
            Dim i As Integer = 0
            Try
                lFI.AddRange(di.GetFiles("thumb*.jpg"))
                lFI.Sort(AddressOf Master.SortThumbFileNames)
                For Each thumb As FileInfo In lFI
                    If Not Me.DeleteList.Contains(thumb.Name) Then
                        Dim fsImage As New FileStream(thumb.FullName, FileMode.Open, FileAccess.Read)
                        Thumbs.Add(New ExtraThumbs With {.Image = Image.FromStream(fsImage), .Name = thumb.Name, .Index = i, .Path = thumb.FullName})
                        ilThumbs.Images.Add(thumb.Name, Thumbs.Item(i).Image)
                        fsImage.Close()
                        fsImage = Nothing
                        i += 1
                    End If
                Next
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Sub

    Private Sub lvThumbs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvThumbs.SelectedIndexChanged
        If Me.lvThumbs.SelectedIndices.Count > 0 Then
            Try
                Me.pbExtraThumbs.Image = Me.Thumbs.Item(Me.lvThumbs.SelectedItems(0).Tag).Image
                Me.ExtraIndex = Me.lvThumbs.SelectedItems(0).Tag
                Me.btnSetAsFanart.Enabled = True
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
            Dim iIndex As Integer = 0
            For i As Integer = (lvThumbs.SelectedItems.Count - 1) To 0 Step -1
                iIndex = lvThumbs.SelectedItems(i).Index
                DeleteList.Add(lvThumbs.Items(iIndex).Name)
                lvThumbs.Items.Remove(lvThumbs.Items(iIndex))
                pbExtraThumbs.Image = Nothing
                btnSetAsFanart.Enabled = False
            Next
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
            Dim lItem As ListViewItem
            If Thumbs.Count > 0 Then
                For Each thumb As ExtraThumbs In Thumbs
                    lItem = lvThumbs.Items.Add(thumb.Name, String.Concat("  ", CStr(thumb.Index + 1)), thumb.Name)
                    lItem.Tag = thumb.Index
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

                Dim xGenre = From xGen In xmlGenre...<name> Select xGen.@searchstring, xGen.@language
                If xGenre.Count > 0 Then
                    For i As Integer = 0 To xGenre.Count - 1
                        If Master.eSettings.GenreFilter.Contains("[All]") OrElse Master.eSettings.GenreFilter.Split(New Char() {","}).Contains(xGenre(i).language) Then
                            Me.lbGenre.Items.Add(xGenre(i).searchstring)
                        End If
                    Next
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            MsgBox("Cannot find Genres.xml." & vbNewLine & vbNewLine & "Expected path:" & vbNewLine & Path.Combine(mePath, "Genres.xml"), MsgBoxStyle.Critical, "File Not Found")
        End If

    End Sub

    Private Sub LoadRatings()

        '//
        ' Read all the ratings from the xml and load into the list
        '\\

        Me.lbMPAA.Items.Add("[none]")

        Dim mePath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Ratings")

        If File.Exists(Path.Combine(mePath, "Ratings.xml")) Then
            Try
                Dim xmlRating As XDocument = XDocument.Load(Path.Combine(mePath, "Ratings.xml"))

                If Master.eSettings.UseCertForMPAA AndAlso Not Master.eSettings.CertificationLang = "USA" AndAlso xmlRating.Element("ratings").Descendants(Master.eSettings.CertificationLang.ToLower).Count > 0 Then
                    Dim xRating = From xRat In xmlRating.Element("ratings").Element(Master.eSettings.CertificationLang.ToLower)...<name> Select xRat.@searchstring
                    If xRating.Count > 0 Then
                        For Each strRating As String In xRating
                            Me.lbMPAA.Items.Add(strRating)
                        Next
                    End If
                Else
                    Dim xRating = From xRat In xmlRating...<usa>...<name> Select xRat.@searchstring
                    If xRating.Count > 0 Then
                        For Each strRating As String In xRating
                            Me.lbMPAA.Items.Add(strRating.Trim)
                        Next
                    End If
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            MsgBox("Cannot find Ratings.xml." & vbNewLine & vbNewLine & "Expected path:" & vbNewLine & Path.Combine(mePath, "Ratings.xml"), MsgBoxStyle.Critical, "File Not Found")
        End If

    End Sub

    Private Sub SelectMPAA()
        If Not String.IsNullOrEmpty(Master.currMovie.MPAA) Then


            Dim mePath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Ratings")

            If File.Exists(Path.Combine(mePath, "Ratings.xml")) Then
                Try
                    Dim xmlRating As XDocument = XDocument.Load(Path.Combine(mePath, "Ratings.xml"))
                    If Master.eSettings.UseCertForMPAA AndAlso Not Master.eSettings.CertificationLang = "USA" AndAlso xmlRating.Element("ratings").Descendants(Master.eSettings.CertificationLang.ToLower).Count > 0 Then
                        Dim l As Integer = Me.lbMPAA.FindString(Strings.Trim(Master.currMovie.MPAA))
                        Me.lbMPAA.SelectedIndex = l
                        If Me.lbMPAA.SelectedItems.Count = 0 Then
                            Me.lbMPAA.SelectedIndex = 0
                        End If

                        Me.lbMPAA.TopIndex = 0

                        txtMPAADesc.Enabled = False
                    Else
                        Dim strMPAA As String = Master.currMovie.MPAA
                        If Strings.InStr(strMPAA.ToLower, "rated g") > 0 Then
                            Me.lbMPAA.SelectedIndex = 1
                        ElseIf Strings.InStr(strMPAA.ToLower, "rated pg-13") > 0 Then
                            Me.lbMPAA.SelectedIndex = 3
                        ElseIf Strings.InStr(strMPAA.ToLower, "rated pg") > 0 Then
                            Me.lbMPAA.SelectedIndex = 2
                        ElseIf Strings.InStr(strMPAA.ToLower, "rated r") > 0 Then
                            Me.lbMPAA.SelectedIndex = 4
                        ElseIf Strings.InStr(strMPAA.ToLower, "rated nc-17") > 0 Then
                            Me.lbMPAA.SelectedIndex = 5
                        Else
                            Me.lbMPAA.SelectedIndex = 0
                        End If

                        Dim strMPAADesc As String = strMPAA
                        strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated g", String.Empty, 1, -1, CompareMethod.Text))
                        strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated pg-13", String.Empty, 1, -1, CompareMethod.Text))
                        strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated pg", String.Empty, 1, -1, CompareMethod.Text))
                        strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated r", String.Empty, 1, -1, CompareMethod.Text))
                        strMPAADesc = Strings.Trim(Strings.Replace(strMPAADesc, "rated nc-17", String.Empty, 1, -1, CompareMethod.Text))
                        txtMPAADesc.Text = strMPAADesc
                    End If

                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Else
                MsgBox("Cannot find Ratings.xml." & vbNewLine & vbNewLine & "Expected path:" & vbNewLine & Path.Combine(mePath, "Ratings.xml"), MsgBoxStyle.Critical, "File Not Found")
            End If
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
        If Convert.ToInt32(txtThumbCount.Text) > 0 Then
            pnlFrameProgress.Visible = True
            Me.Refresh()
            Master.CreateRandomThumbs(Master.currPath, Convert.ToInt32(txtThumbCount.Text))
            pnlFrameProgress.Visible = False
            Me.RefreshExtraThumbs()
        End If
    End Sub

    Private Sub txtThumbCount_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtThumbCount.GotFocus
        Me.AcceptButton = Me.btnAutoGen
    End Sub

    Private Sub txtThumbCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtThumbCount.KeyPress
        e.Handled = Master.NumericOnly(Asc(e.KeyChar))
    End Sub

    Private Sub txtThumbCount_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtThumbCount.LostFocus
        Me.AcceptButton = Me.OK_Button
    End Sub

    Private Sub txtThumbCount_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtThumbCount.TextChanged
        btnAutoGen.Enabled = Not String.IsNullOrEmpty(txtThumbCount.Text)
    End Sub

    Private Sub RefreshExtraThumbs()
        Thumbs.Clear()
        lvThumbs.Clear()
        ilThumbs.Images.Clear()
        Me.bwThumbs.RunWorkerAsync()
    End Sub

    Private Sub btnThumbsRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnThumbsRefresh.Click
        Me.RefreshExtraThumbs()
    End Sub

    Private Sub btnStudio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStudio.Click
        Using dStudio As New dlgStudioSelect
            Dim tStudio As String = dStudio.ShowDialog(Master.currMovie.IMDBID)
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
        Me.btnSetAsFanart.Enabled = False
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

        Friend Property Index() As String
            Get
                Return _index
            End Get
            Set(ByVal value As String)
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

End Class
