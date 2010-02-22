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

Imports System.IO

Public Class dlgEditEpisode
    Private lvwActorSorter As ListViewColumnSorter
    Private tmpRating As String
    Private Poster As New Images With {.IsEdit = True}
    Private Fanart As New Images With {.IsEdit = True}

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Try
            Me.SetInfo()

            Master.DB.SaveTVEpToDB(Master.currShow, False, True, False, True)

            Me.CleanUp()

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgEditEpisode_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SetUp()

        Me.lvwActorSorter = New ListViewColumnSorter()
        Me.lvActors.ListViewItemSorter = Me.lvwActorSorter

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
        dFileInfoEdit.Show(True)

        If Not (Master.eSettings.EpisodeDashFanart OrElse Master.eSettings.EpisodeDotFanart) Then
            Me.TabControl1.TabPages.Remove(TabPage3)
        End If

        Me.FillInfo()
    End Sub

    Private Sub FillInfo()
        With Me
            If Not String.IsNullOrEmpty(Master.currShow.TVEp.Title) Then .txtTitle.Text = Master.currShow.TVEp.Title
            If Not String.IsNullOrEmpty(Master.currShow.TVEp.Plot) Then .txtPlot.Text = Master.currShow.TVEp.Plot
            If Not String.IsNullOrEmpty(Master.currShow.TVEp.Aired) Then .txtAired.Text = Master.currShow.TVEp.Aired
            If Not String.IsNullOrEmpty(Master.currShow.TVEp.Director) Then .txtDirector.Text = Master.currShow.TVEp.Director
            If Not String.IsNullOrEmpty(Master.currShow.TVEp.Credits) Then .txtCredits.Text = Master.currShow.TVEp.Credits
            If Not String.IsNullOrEmpty(Master.currShow.TVEp.Season.ToString) Then .txtSeason.Text = Master.currShow.TVEp.Season.ToString
            If Not String.IsNullOrEmpty(Master.currShow.TVEp.Episode.ToString) Then .txtEpisode.Text = Master.currShow.TVEp.Episode.ToString

            Dim lvItem As ListViewItem
            .lvActors.Items.Clear()
            For Each imdbAct As MediaContainers.Person In Master.currShow.TVEp.Actors
                lvItem = .lvActors.Items.Add(imdbAct.Name)
                lvItem.SubItems.Add(imdbAct.Role)
                lvItem.SubItems.Add(imdbAct.Thumb)
            Next

            Dim tRating As Single = NumUtils.ConvertToSingle(Master.currShow.TVEp.Rating)
            .tmpRating = tRating.ToString
            .pbStar1.Tag = tRating
            .pbStar2.Tag = tRating
            .pbStar3.Tag = tRating
            .pbStar4.Tag = tRating
            .pbStar5.Tag = tRating
            If tRating > 0 Then .BuildStars(tRating)

            If TabControl1.TabPages.Contains(TabPage3) Then
                Fanart.FromFile(Master.currShow.EpFanartPath)
                If Not IsNothing(Fanart.Image) Then
                    .pbFanart.Image = Fanart.Image

                    .lblFanartSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), .pbFanart.Image.Width, .pbFanart.Image.Height)
                    .lblFanartSize.Visible = True
                End If
                .btnSetFanartScrape.Enabled = False
            End If

            Poster.FromFile(Master.currShow.EpPosterPath)
            If Not IsNothing(Poster.Image) Then
                .pbPoster.Image = Poster.Image

                .lblPosterSize.Text = String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), .pbPoster.Image.Width, .pbPoster.Image.Height)
                .lblPosterSize.Visible = True
            End If
            .btnSetPosterScrape.Enabled = False
        End With
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

        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnManual_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnManual.Click
        Try
            If dlgManualEdit.ShowDialog(Master.currShow.EpNfoPath) = Windows.Forms.DialogResult.OK Then
                Master.currShow.TVEp = NFO.LoadTVEpFromNFO(Master.currShow.EpNfoPath, Master.currShow.TVEp.Season, Master.currShow.TVEp.Episode)
                Me.FillInfo()
            End If
        Catch ex As Exception
            ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SetInfo()
        Try
            With Me

                Master.currShow.TVEp.Title = .txtTitle.Text.Trim
                Master.currShow.TVEp.Plot = .txtPlot.Text.Trim
                Master.currShow.TVEp.Aired = .txtAired.Text.Trim
                Master.currShow.TVEp.Director = .txtDirector.Text.Trim
                Master.currShow.TVEp.Credits = .txtCredits.Text.Trim
                Master.currShow.TVEp.Season = Convert.ToInt32(.txtSeason.Text.Trim)
                Master.currShow.TVEp.Episode = Convert.ToInt32(.txtEpisode.Text.Trim)
                Master.currShow.TVEp.Rating = .tmpRating

                Master.currShow.TVEp.Actors.Clear()

                If .lvActors.Items.Count > 0 Then
                    For Each lviActor As ListViewItem In .lvActors.Items
                        Dim addActor As New MediaContainers.Person
                        addActor.Name = lviActor.Text.Trim
                        addActor.Role = lviActor.SubItems(1).Text.Trim
                        addActor.Thumb = lviActor.SubItems(2).Text.Trim

                        Master.currShow.TVEp.Actors.Add(addActor)
                    Next
                End If

                If Not IsNothing(.Fanart.Image) Then
                    Dim fPath As String = .Fanart.SaveAsEpFanart(Master.currShow)
                    Master.currShow.EpFanartPath = fPath
                Else
                    .Fanart.DeleteEpFanart(Master.currShow)
                    Master.currShow.EpFanartPath = String.Empty
                End If

                If Not IsNothing(.Poster.Image) Then
                    Dim pPath As String = .Poster.SaveAsEpPoster(Master.currShow)
                    Master.currShow.EpPosterPath = pPath
                Else
                    .Poster.DeleteEpPosters(Master.currShow)
                    Master.currShow.EpPosterPath = String.Empty
                End If
            End With
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

    Private Sub txtEpisode_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtEpisode.KeyPress
        e.Handled = StringUtils.NumericOnly(e.KeyChar, True)
    End Sub

    Private Sub txtSeason_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSeason.KeyPress
        e.Handled = StringUtils.NumericOnly(e.KeyChar, True)
    End Sub

    Private Sub SetUp()
        Dim mTitle As String = String.Empty
        mTitle = Master.currShow.TVEp.Title
        Dim sTitle As String = String.Concat(Master.eLang.GetString(656, "Edit Episode"), If(String.IsNullOrEmpty(mTitle), String.Empty, String.Concat(" - ", mTitle)))
        Me.Text = sTitle
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.Label2.Text = Master.eLang.GetString(656, "Edit the details for the selected episode.")
        Me.Label1.Text = Master.eLang.GetString(657, "Edit Episode")
        Me.TabPage1.Text = Master.eLang.GetString(26, "Details")
        Me.btnManual.Text = Master.eLang.GetString(230, "Manual Edit")
        Me.lblActors.Text = Master.eLang.GetString(231, "Actors:")
        Me.colName.Text = Master.eLang.GetString(232, "Name")
        Me.colRole.Text = Master.eLang.GetString(233, "Role")
        Me.colThumb.Text = Master.eLang.GetString(234, "Thumb")
        Me.lblPlot.Text = Master.eLang.GetString(241, "Plot:")
        Me.lblRating.Text = Master.eLang.GetString(245, "Rating:")
        Me.lblAired.Text = Master.eLang.GetString(658, "Aired:")
        Me.lblSeason.Text = Master.eLang.GetString(659, "Season:")
        Me.lblEpisode.Text = Master.eLang.GetString(660, "Episode:")
        Me.lblTitle.Text = Master.eLang.GetString(246, "Title:")
        Me.TabPage2.Text = Master.eLang.GetString(148, "Poster")
        Me.btnRemovePoster.Text = Master.eLang.GetString(247, "Remove Poster")
        Me.btnSetPosterScrape.Text = Master.eLang.GetString(248, "Change Poster (Scrape)")
        Me.btnSetPoster.Text = Master.eLang.GetString(249, "Change Poster (Local)")
        Me.TabPage3.Text = Master.eLang.GetString(149, "Fanart")
        Me.btnRemoveFanart.Text = Master.eLang.GetString(250, "Remove Fanart")
        Me.btnSetFanartScrape.Text = Master.eLang.GetString(251, "Change Fanart (Scrape)")
        Me.btnSetFanart.Text = Master.eLang.GetString(252, "Change Fanart (Local)")
        Me.btnSetPosterDL.Text = Master.eLang.GetString(265, "Change Poster (Download)")
        Me.btnSetFanartDL.Text = Master.eLang.GetString(266, "Change Fanart (Download)")
        Me.lblDirector.Text = Master.eLang.GetString(239, "Director:")
        Me.lblCredits.Text = Master.eLang.GetString(228, "Credits:")
        Me.TabPage4.Text = Master.eLang.GetString(256, "Frame Extraction")
        Me.btnFrameLoad.Text = Master.eLang.GetString(661, "Load Episode")
        Me.btnFrameSave.Text = Master.eLang.GetString(662, "Save as Poster")
        Me.TabPage5.Text = Master.eLang.GetString(59, "Meta Data")

    End Sub

End Class
