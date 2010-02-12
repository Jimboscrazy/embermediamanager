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

'TODO: Image Selection - filters, remove current image, revert to default
'TODO: Add setting to only get posters for the specified language and default to english posters if posters are not available in the selected language
'TODO: TV Scraper to background worker
'TODO: Show/Season/Episode Locking and Marking
'TODO: Show and Episode field locks
'TODO: Enable/Disable scraper info fields
'TODO: Fall back to episode title when trying to match scraper episodes and no season or episode # is available
'TODO: TV Show renaming
'TODO: Scrape individual episode
'TODO: Change season images
'TODO: Check if we have information for a show or episode when first scanning in
'TODO: Support VIDEO_TS/BDMV folders for TV Shows
'TODO: Setting for the APIKey to save to the nfo.
'TODO: ?????
'TODO: PROFIT!!!!


Imports System.IO
Imports System.Text.RegularExpressions

Namespace TVDB
    Partial Public Class Scraper
        Public Class dlgTVImageSelect
            Private _id As Integer = -1
            Private _season As Integer = -999

            Private ShowPosterList As New List(Of TVDBShowPoster)
            Private FanartList As New List(Of TVDBFanart)
            Private SeasonList As New List(Of TVDBSeasonPoster)
            Private GenericPosterList As New List(Of TVDBPoster)

            Private pbImage() As PictureBox
            Private pnlImage() As Panel
            Private lblImage() As Label

            Private iCounter As Integer = 0
            Private iTop As Integer = 5
            Private iLeft As Integer = 5

            Private SelSeason As Integer = -999
            Private SelIsPoster As Boolean = True

            Friend WithEvents bwLoadData As New System.ComponentModel.BackgroundWorker
            Friend WithEvents bwLoadImages As New System.ComponentModel.BackgroundWorker
            Friend WithEvents bwDownloadFanart As New System.ComponentModel.BackgroundWorker

            Private Structure ImageTag
                Dim URL As String
                Dim Path As String
                Dim isFanart As Boolean
            End Structure

            Private Sub GenerateList()
                Try
                    Me.tvList.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Show Poster"), .Tag = "showp"})
                    Me.tvList.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Show Fanart"), .Tag = "showf"})

                    Dim TnS As TreeNode
                    For Each cSeason As TVDBSeasonImage In TVDBImages.SeasonImageList
                        Try
                            TnS = New TreeNode(String.Format(Master.eLang.GetString(999, "Season {0}"), cSeason.Season))
                            TnS.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Season Poster"), .Tag = String.Concat("p", cSeason.Season.ToString)})
                            If Master.eSettings.SeasonFanartEnabled Then TnS.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Season Fanart"), .Tag = String.Concat("f", cSeason.Season.ToString)})
                            Me.tvList.Nodes.Add(TnS)
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            End Sub

            Private Function DownloadAllImages() As Boolean
                Dim iProgress As Integer = 1

                Try
                    Me.bwLoadImages.ReportProgress(tmpTVDBShow.Episodes.Count + tmpTVDBShow.SeasonPosters.Count + tmpTVDBShow.ShowPosters.Count + tmpTVDBShow.Fanart.Count + tmpTVDBShow.Posters.Count, "max")

                    For Each Epi As Master.DBTV In tmpTVDBShow.Episodes
                        Try
                            If Not File.Exists(Epi.TVEp.LocalFile) Then
                                If Not String.IsNullOrEmpty(Epi.TVEp.PosterURL) Then
                                    Epi.TVEp.Poster.FromWeb(Epi.TVEp.PosterURL)
                                    Directory.CreateDirectory(Directory.GetParent(Epi.TVEp.LocalFile).FullName)
                                    Epi.TVEp.Poster.Save(Epi.TVEp.LocalFile)
                                End If
                            Else
                                Epi.TVEp.Poster.FromFile(Epi.TVEp.LocalFile)
                            End If

                            If Me.bwLoadImages.CancellationPending Then
                                Return True
                            End If

                            Me.bwLoadImages.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                    For Each Seas As TVDBSeasonPoster In tmpTVDBShow.SeasonPosters
                        Try
                            If Not File.Exists(Seas.LocalFile) Then
                                If Not String.IsNullOrEmpty(Seas.URL) Then
                                    Seas.Image.FromWeb(Seas.URL)
                                    Directory.CreateDirectory(Directory.GetParent(Seas.LocalFile).FullName)
                                    Seas.Image.Save(Seas.LocalFile)
                                End If
                            Else
                                Seas.Image.FromFile(Seas.LocalFile)
                            End If
                            SeasonList.Add(Seas)

                            If Me.bwLoadImages.CancellationPending Then
                                Return True
                            End If

                            Me.bwLoadImages.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                    For Each SPost As TVDBShowPoster In tmpTVDBShow.ShowPosters
                        Try
                            If Not File.Exists(SPost.LocalFile) Then
                                If Not String.IsNullOrEmpty(SPost.URL) Then
                                    SPost.Image.FromWeb(SPost.URL)
                                    Directory.CreateDirectory(Directory.GetParent(SPost.LocalFile).FullName)
                                    SPost.Image.Save(SPost.LocalFile)
                                End If
                            Else
                                SPost.Image.FromFile(SPost.LocalFile)
                            End If
                            ShowPosterList.Add(SPost)

                            If Me.bwLoadImages.CancellationPending Then
                                Return True
                            End If

                            Me.bwLoadImages.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                    For Each SFan As TVDBFanart In tmpTVDBShow.Fanart
                        Try
                            If Not File.Exists(SFan.LocalThumb) Then
                                If Not String.IsNullOrEmpty(SFan.ThumbnailURL) Then
                                    SFan.Image.FromWeb(SFan.ThumbnailURL)
                                    Directory.CreateDirectory(Directory.GetParent(SFan.LocalThumb).FullName)
                                    SFan.Image.Image.Save(SFan.LocalThumb)
                                End If
                            Else
                                SFan.Image.FromFile(SFan.LocalThumb)
                            End If
                            FanartList.Add(SFan)

                            If Me.bwLoadImages.CancellationPending Then
                                Return True
                            End If

                            Me.bwLoadImages.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                    For Each Post As TVDBPoster In tmpTVDBShow.Posters
                        Try
                            If Not File.Exists(Post.LocalFile) Then
                                If Not String.IsNullOrEmpty(Post.URL) Then
                                    Post.Image.FromWeb(Post.URL)
                                    Directory.CreateDirectory(Directory.GetParent(Post.LocalFile).FullName)
                                    Post.Image.Save(Post.LocalFile)
                                End If
                            Else
                                Post.Image.FromFile(Post.LocalFile)
                            End If
                            GenericPosterList.Add(Post)

                            If Me.bwLoadImages.CancellationPending Then
                                Return True
                            End If

                            Me.bwLoadImages.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

                Return Me.SetDefaults()

            End Function

            Public Function SetDefaults() As Boolean

                Dim iSeason As Integer = -1
                Dim iEpisode As Integer = -1
                Dim iProgress As Integer = 3

                Dim tSea As TVDBSeasonPoster

                Try
                    Me.bwLoadImages.ReportProgress(TVDBImages.SeasonImageList.Count + tmpTVDBShow.Episodes.Count + 2, "defaults")

                    If IsNothing(TVDBImages.ShowPoster.Image.Image) Then
                        Dim tSP As TVDBShowPoster = ShowPosterList.FirstOrDefault(Function(p) Not IsNothing(p.Image.Image))
                        If Not IsNothing(tSP) Then
                            TVDBImages.ShowPoster.Image.Image = tSP.Image.Image
                            TVDBImages.ShowPoster.LocalFile = tSP.LocalFile
                            TVDBImages.ShowPoster.URL = tSP.URL
                        End If
                    End If

                    If Me.bwLoadImages.CancellationPending Then
                        Return True
                    End If
                    Me.bwLoadImages.ReportProgress(1, "progress")

                    If IsNothing(TVDBImages.ShowFanart.Image.Image) Then
                        Dim tSF As TVDBFanart = FanartList.FirstOrDefault(Function(f) Not IsNothing(f.Image.Image))
                        If Not IsNothing(tSF) Then
                            If Not String.IsNullOrEmpty(tSF.LocalFile) AndAlso File.Exists(tSF.LocalFile) Then
                                TVDBImages.ShowFanart.Image.FromFile(tSF.LocalFile)
                                TVDBImages.ShowFanart.LocalFile = tSF.LocalFile
                                TVDBImages.ShowFanart.URL = tSF.URL
                            ElseIf Not String.IsNullOrEmpty(tSF.LocalFile) AndAlso Not String.IsNullOrEmpty(tSF.URL) Then
                                TVDBImages.ShowFanart.Image.FromWeb(tSF.URL)
                                Directory.CreateDirectory(Directory.GetParent(tSF.LocalFile).FullName)
                                TVDBImages.ShowFanart.Image.Save(tSF.LocalFile)
                                TVDBImages.ShowFanart.LocalFile = tSF.LocalFile
                                TVDBImages.ShowFanart.URL = tSF.URL
                            End If
                        End If
                    End If

                    If Me.bwLoadImages.CancellationPending Then
                        Return True
                    End If
                    Me.bwLoadImages.ReportProgress(2, "progress")

                    For Each cSeason As TVDBSeasonImage In TVDBImages.SeasonImageList
                        Try
                            iSeason = cSeason.Season
                            If IsNothing(cSeason.Poster.Image) Then
                                tSea = SeasonList.FirstOrDefault(Function(p) Not IsNothing(p.Image.Image) AndAlso p.Season = iSeason)
                                If Not IsNothing(tSea) Then cSeason.Poster.Image = tSea.Image.Image
                            End If
                            If Master.eSettings.SeasonFanartEnabled AndAlso IsNothing(cSeason.Fanart.Image.Image) AndAlso Not IsNothing(TVDBImages.ShowFanart.Image.Image) Then cSeason.Fanart.Image.Image = TVDBImages.ShowFanart.Image.Image

                            If Me.bwLoadImages.CancellationPending Then
                                Return True
                            End If
                            Me.bwLoadImages.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                    For Each Episode As Master.DBTV In tmpTVDBShow.Episodes
                        Try
                            If Not String.IsNullOrEmpty(Episode.TVEp.LocalFile) Then
                                Episode.TVEp.Poster.FromFile(Episode.TVEp.LocalFile)
                            ElseIf Not String.IsNullOrEmpty(Episode.EpPosterPath) Then
                                Episode.TVEp.Poster.FromFile(Episode.EpPosterPath)
                            End If

                            If Master.eSettings.EpisodeFanartEnabled Then
                                If Not String.IsNullOrEmpty(Episode.EpFanartPath) Then
                                    Episode.TVEp.Fanart.FromFile(Episode.EpFanartPath)
                                ElseIf Not IsNothing(TVDBImages.ShowFanart.Image.Image) Then
                                    Episode.TVEp.Fanart.Image = TVDBImages.ShowFanart.Image.Image
                                End If
                            End If

                            If Me.bwLoadImages.CancellationPending Then
                                Return True
                            End If
                            Me.bwLoadImages.ReportProgress(iProgress, "progress")
                            iProgress += 1
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next

                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

                Return False
            End Function

            Public Overloads Function ShowDialog(ByVal ShowID As Integer) As System.Windows.Forms.DialogResult
                Me._id = ShowID
                Return MyBase.ShowDialog
            End Function

            Private Sub tvList_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvList.AfterSelect
                Dim iCount As Integer = 0

                Try
                    ClearImages()
                    If Not IsNothing(e.Node.Tag) AndAlso Not String.IsNullOrEmpty(e.Node.Tag.ToString) Then
                        If e.Node.Tag.ToString = "showp" Then
                            Me.SelSeason = -999
                            Me.SelIsPoster = True
                            Me.pbCurrent.Image = TVDBImages.ShowPoster.Image.Image
                            iCount = ShowPosterList.Count
                            For i = 0 To iCount - 1
                                Me.AddImage(ShowPosterList(i).Image.Image, String.Format("{0}x{1}", ShowPosterList(i).Image.Image.Width, ShowPosterList(i).Image.Image.Height), i, New ImageTag With {.URL = ShowPosterList(i).URL, .Path = ShowPosterList(i).LocalFile, .isFanart = False})
                            Next

                            For i = 0 To GenericPosterList.Count - 1
                                Me.AddImage(GenericPosterList(i).Image.Image, String.Format("{0}x{1}", GenericPosterList(i).Image.Image.Width, GenericPosterList(i).Image.Image.Height), i + iCount, New ImageTag With {.URL = GenericPosterList(i).URL, .Path = GenericPosterList(i).LocalFile, .isFanart = False})
                            Next
                        ElseIf e.Node.Tag.ToString = "showf" Then
                            Me.SelSeason = -999
                            Me.SelIsPoster = False
                            Me.pbCurrent.Image = TVDBImages.ShowFanart.Image.Image
                            For i = 0 To FanartList.Count - 1
                                Me.AddImage(FanartList(i).Image.Image, String.Format("{0}x{1}", FanartList(i).Image.Image.Width, FanartList(i).Image.Image.Height), i, New ImageTag With {.URL = FanartList(i).URL, .Path = FanartList(i).LocalFile, .isFanart = True})
                            Next
                        Else
                            Dim tMatch As Match = Regex.Match(e.Node.Tag.ToString, "(?<type>f|p)(?<num>[0-9]+)")
                            If tMatch.Success Then
                                If tMatch.Groups("type").Value = "f" Then
                                    Me.SelSeason = Convert.ToInt32(tMatch.Groups("num").Value)
                                    Me.SelIsPoster = False
                                    Me.pbCurrent.Image = TVDBImages.SeasonImageList.FirstOrDefault(Function(f) f.Season = Convert.ToInt32(tMatch.Groups("num").Value)).Fanart.Image.Image
                                    For i = 0 To FanartList.Count - 1
                                        Me.AddImage(FanartList(i).Image.Image, String.Format("{0}x{1}", FanartList(i).Image.Image.Width, FanartList(i).Image.Image.Height), i, New ImageTag With {.URL = FanartList(i).URL, .Path = FanartList(i).LocalFile, .isFanart = True})
                                    Next
                                ElseIf tMatch.Groups("type").Value = "p" Then
                                    Me.SelSeason = Convert.ToInt32(tMatch.Groups("num").Value)
                                    Me.SelIsPoster = True
                                    Me.pbCurrent.Image = TVDBImages.SeasonImageList.FirstOrDefault(Function(f) f.Season = Convert.ToInt32(tMatch.Groups("num").Value)).Poster.Image
                                    iCount = 0
                                    For Each SImage As TVDBSeasonPoster In SeasonList.Where(Function(s) s.Season = Convert.ToInt32(tMatch.Groups("num").Value))
                                        Me.AddImage(SImage.Image.Image, String.Format("{0}x{1}", SImage.Image.Image.Width, SImage.Image.Image.Height), iCount, Nothing)
                                        iCount += 1
                                    Next
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            End Sub

            Private Sub AddImage(ByVal iImage As Image, ByVal sDescription As String, ByVal iIndex As Integer, ByVal fTag As ImageTag)

                Try
                    ReDim Preserve Me.pnlImage(iIndex)
                    ReDim Preserve Me.pbImage(iIndex)
                    ReDim Preserve Me.lblImage(iIndex)
                    Me.pnlImage(iIndex) = New Panel()
                    Me.pbImage(iIndex) = New PictureBox()
                    Me.lblImage(iIndex) = New Label()
                    Me.pbImage(iIndex).Name = iIndex.ToString
                    Me.pnlImage(iIndex).Name = iIndex.ToString
                    Me.lblImage(iIndex).Name = iIndex.ToString
                    Me.pnlImage(iIndex).Size = New Size(187, 187)
                    Me.pbImage(iIndex).Size = New Size(181, 151)
                    Me.lblImage(iIndex).Size = New Size(181, 30)
                    Me.pnlImage(iIndex).BackColor = Color.White
                    Me.pnlImage(iIndex).BorderStyle = BorderStyle.FixedSingle
                    Me.pbImage(iIndex).SizeMode = PictureBoxSizeMode.Zoom
                    Me.lblImage(iIndex).AutoSize = False
                    Me.lblImage(iIndex).BackColor = Color.White
                    Me.lblImage(iIndex).TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                    Me.lblImage(iIndex).Text = sDescription
                    Me.pbImage(iIndex).Image = iImage
                    Me.pnlImage(iIndex).Left = iLeft
                    Me.pbImage(iIndex).Left = 3
                    Me.lblImage(iIndex).Left = 0
                    Me.pnlImage(iIndex).Top = iTop
                    Me.pbImage(iIndex).Top = 3
                    Me.lblImage(iIndex).Top = 151
                    Me.pnlImage(iIndex).Tag = fTag
                    Me.pbImage(iIndex).Tag = fTag
                    Me.lblImage(iIndex).Tag = fTag
                    Me.pnlImages.Controls.Add(Me.pnlImage(iIndex))
                    Me.pnlImage(iIndex).Controls.Add(Me.pbImage(iIndex))
                    Me.pnlImage(iIndex).Controls.Add(Me.lblImage(iIndex))
                    Me.pnlImage(iIndex).BringToFront()
                    AddHandler pbImage(iIndex).Click, AddressOf pbImage_Click
                    AddHandler pbImage(iIndex).DoubleClick, AddressOf pbImage_DoubleClick
                    AddHandler pnlImage(iIndex).Click, AddressOf pnlImage_Click
                    AddHandler lblImage(iIndex).Click, AddressOf lblImage_Click

                    AddHandler pbImage(iIndex).MouseWheel, AddressOf MouseWheelEvent
                    AddHandler pnlImage(iIndex).MouseWheel, AddressOf MouseWheelEvent
                    AddHandler lblImage(iIndex).MouseWheel, AddressOf MouseWheelEvent


                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

                Me.iCounter += 1

                If Me.iCounter = 3 Then
                    Me.iCounter = 0
                    Me.iLeft = 5
                    Me.iTop += 192
                Else
                    Me.iLeft += 192
                End If
            End Sub

            Private Sub pbImage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                Me.DoSelect(Convert.ToInt32(DirectCast(sender, PictureBox).Name), DirectCast(sender, PictureBox).Image, DirectCast(DirectCast(sender, PictureBox).Tag, ImageTag))
            End Sub

            Private Sub pbImage_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
                Dim tImage As Image = Nothing
                Dim iTag As ImageTag = DirectCast(DirectCast(sender, PictureBox).Tag, ImageTag)
                If Not IsNothing(iTag) OrElse Not iTag.isFanart Then
                    tImage = DownloadFanart(iTag)
                Else
                    tImage = DirectCast(sender, PictureBox).Image
                End If

                Using dImgView As New dlgImgView
                    dImgView.ShowDialog(tImage)
                End Using
            End Sub

            Private Sub pnlImage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                Dim iIndex As Integer = Convert.ToInt32(DirectCast(sender, Panel).Name)
                Me.DoSelect(iIndex, Me.pbImage(iIndex).Image, DirectCast(DirectCast(sender, Panel).Tag, ImageTag))
            End Sub

            Private Sub lblImage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
                Dim iindex As Integer = Convert.ToInt32(DirectCast(sender, Label).Name)
                Me.DoSelect(iindex, Me.pbImage(iindex).Image, DirectCast(DirectCast(sender, Label).Tag, ImageTag))
            End Sub

            Private Sub ClearImages()
                Try
                    Me.iCounter = 0
                    Me.iLeft = 5
                    Me.iTop = 5
                    Me.pbCurrent.Image = Nothing

                    If Me.pnlImages.Controls.Count > 0 Then
                        For i = UBound(Me.pnlImage) To 0 Step -1
                            Me.pnlImage(i).Controls.Remove(Me.lblImage(i))
                            Me.pnlImage(i).Controls.Remove(Me.pbImage(i))
                            Me.pnlImages.Controls.Remove(Me.pnlImage(i))
                        Next
                    End If
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            End Sub

            Private Sub DoSelect(ByVal iIndex As Integer, ByVal SelImage As Image, ByVal SelTag As ImageTag)
                Try
                    For i As Integer = 0 To UBound(Me.pnlImage)
                        Me.pnlImage(i).BackColor = Color.White
                        Me.lblImage(i).BackColor = Color.White
                        Me.lblImage(i).ForeColor = Color.Black
                    Next

                    Me.pnlImage(iIndex).BackColor = Color.Blue
                    Me.lblImage(iIndex).BackColor = Color.Blue
                    Me.lblImage(iIndex).ForeColor = Color.White

                    Me.pbCurrent.Image = SelImage

                    If Me.SelSeason = -999 Then
                        If Me.SelIsPoster Then
                            TVDBImages.ShowPoster.Image.Image = SelImage
                            TVDBImages.ShowPoster.LocalFile = SelTag.Path
                            TVDBImages.ShowPoster.URL = SelTag.URL
                        Else
                            TVDBImages.ShowFanart.Image.Image = SelImage
                            TVDBImages.ShowFanart.LocalFile = SelTag.Path
                            TVDBImages.ShowFanart.URL = SelTag.URL
                        End If
                    Else
                        If Me.SelIsPoster Then
                            TVDBImages.SeasonImageList.FirstOrDefault(Function(s) s.Season = Me.SelSeason).Poster.Image = SelImage
                        Else
                            TVDBImages.SeasonImageList.FirstOrDefault(Function(s) s.Season = Me.SelSeason).Fanart.Image.Image = SelImage
                        End If
                    End If
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            End Sub

            Private Sub bwLoadData_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadData.DoWork
                Dim cSI As TVDBSeasonImage
                Dim iProgress As Integer = 1
                Dim iSeason As Integer = -1

                Me.bwLoadData.ReportProgress(tmpTVDBShow.Episodes.Count, "current")

                'initialize the struct
                TVDBImages.ShowPoster = New TVDBShowPoster
                TVDBImages.ShowFanart = New TVDBFanart
                TVDBImages.SeasonImageList = New List(Of TVDBSeasonImage)

                If Me.bwLoadData.CancellationPending Then
                    e.Cancel = True
                    Return
                End If

                If Not String.IsNullOrEmpty(tmpTVDBShow.Show.ShowPosterPath) Then
                    TVDBImages.ShowPoster.Image.FromFile(tmpTVDBShow.Show.ShowPosterPath)
                    TVDBImages.ShowPoster.LocalFile = tmpTVDBShow.Show.ShowPosterPath
                End If

                If Me.bwLoadData.CancellationPending Then
                    e.Cancel = True
                    Return
                End If

                If Not String.IsNullOrEmpty(tmpTVDBShow.Show.ShowFanartPath) Then
                    TVDBImages.ShowFanart.Image.FromFile(tmpTVDBShow.Show.ShowFanartPath)
                    TVDBImages.ShowFanart.LocalFile = tmpTVDBShow.Show.ShowFanartPath
                End If

                If Me.bwLoadData.CancellationPending Then
                    e.Cancel = True
                    Return
                End If

                For Each sEpisode As Master.DBTV In tmpTVDBShow.Episodes
                    Try
                        iSeason = sEpisode.TVEp.Season

                        If IsNothing(TVDBImages.ShowPoster.Image) AndAlso Not String.IsNullOrEmpty(sEpisode.ShowPosterPath) Then
                            TVDBImages.ShowPoster.Image.FromFile(sEpisode.ShowPosterPath)
                        End If

                        If Me.bwLoadData.CancellationPending Then
                            e.Cancel = True
                            Return
                        End If

                        If Master.eSettings.EpisodeFanartEnabled AndAlso IsNothing(TVDBImages.ShowFanart.Image.Image) AndAlso Not String.IsNullOrEmpty(sEpisode.ShowFanartPath) Then
                            TVDBImages.ShowFanart.Image.FromFile(sEpisode.ShowFanartPath)
                            TVDBImages.ShowFanart.LocalFile = sEpisode.ShowFanartPath
                        End If

                        If Me.bwLoadData.CancellationPending Then
                            e.Cancel = True
                            Return
                        End If

                        If TVDBImages.SeasonImageList.Where(Function(s) s.Season = iSeason).Count = 0 Then
                            cSI = New TVDBSeasonImage
                            cSI.Season = iSeason
                            If Not String.IsNullOrEmpty(sEpisode.SeasonPosterPath) Then
                                cSI.Poster.FromFile(sEpisode.SeasonPosterPath)
                            End If
                            If Master.eSettings.SeasonFanartEnabled AndAlso Not String.IsNullOrEmpty(sEpisode.SeasonFanartPath) Then
                                cSI.Fanart.Image.FromFile(sEpisode.SeasonFanartPath)
                                cSI.Fanart.LocalFile = sEpisode.SeasonFanartPath
                            End If
                            TVDBImages.SeasonImageList.Add(cSI)
                        End If

                        If Me.bwLoadData.CancellationPending Then
                            e.Cancel = True
                            Return
                        End If

                        Me.bwLoadData.ReportProgress(iProgress, "progress")
                        iProgress += 1
                    Catch ex As Exception
                        Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                    End Try
                Next

            End Sub

            Private Sub bwLoadData_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadData.ProgressChanged
                Try
                    If e.UserState.ToString = "progress" Then
                        Me.pbStatus.Value = e.ProgressPercentage
                    ElseIf e.UserState.ToString = "current" Then
                        Me.lblStatus.Text = Master.eLang.GetString(999, "Loading Current Images...")
                        Me.pbStatus.Value = 0
                        Me.pbStatus.Maximum = e.ProgressPercentage
                    Else
                        Me.pbStatus.Value = 0
                        Me.pbStatus.Maximum = e.ProgressPercentage
                    End If
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            End Sub

            Private Sub bwLoadData_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadData.RunWorkerCompleted

                If Not e.Cancelled Then
                    Me.GenerateList()

                    Me.lblStatus.Text = Master.eLang.GetString(999, "(Down)Loading New Images...")
                    Me.bwLoadImages.WorkerReportsProgress = True
                    Me.bwLoadImages.WorkerSupportsCancellation = True
                    Me.bwLoadImages.RunWorkerAsync()
                End If

            End Sub

            Private Sub dlgTVImageSelect_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
                AddHandler pnlImages.MouseWheel, AddressOf MouseWheelEvent
                AddHandler MyBase.MouseWheel, AddressOf MouseWheelEvent
                AddHandler tvList.MouseWheel, AddressOf MouseWheelEvent

                Master.PNLDoubleBuffer(Me.pnlImages)
            End Sub

            Private Sub MouseWheelEvent(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
                If e.Delta < 0 Then
                    If (pnlImages.VerticalScroll.Value + 50) <= pnlImages.VerticalScroll.Maximum Then
                        pnlImages.VerticalScroll.Value += 50
                    Else
                        pnlImages.VerticalScroll.Value = pnlImages.VerticalScroll.Maximum
                    End If
                Else
                    If (pnlImages.VerticalScroll.Value - 50) >= pnlImages.VerticalScroll.Minimum Then
                        pnlImages.VerticalScroll.Value -= 50
                    Else
                        pnlImages.VerticalScroll.Value = pnlImages.VerticalScroll.Minimum
                    End If
                End If
            End Sub

            Private Sub dlgTVImageSelect_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
                Me.bwLoadData.WorkerReportsProgress = True
                Me.bwLoadData.WorkerSupportsCancellation = True
                Me.bwLoadData.RunWorkerAsync()
            End Sub

            Private Sub bwLoadImages_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadImages.DoWork
                e.Cancel = Me.DownloadAllImages()
            End Sub

            Private Sub bwLoadImages_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadImages.ProgressChanged
                Try
                    If e.UserState.ToString = "progress" Then
                        Me.pbStatus.Value = e.ProgressPercentage
                    ElseIf e.UserState.ToString = "defaults" Then
                        Me.lblStatus.Text = Master.eLang.GetString(999, "Setting Defaults...")
                        Me.pbStatus.Value = 0
                        Me.pbStatus.Maximum = e.ProgressPercentage
                    Else
                        Me.pbStatus.Value = 0
                        Me.pbStatus.Maximum = e.ProgressPercentage
                    End If
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            End Sub

            Private Sub bwLoadImages_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadImages.RunWorkerCompleted

                Me.pnlStatus.Visible = False

                If Not e.Cancelled Then
                    Me.tvList.Enabled = True
                    Me.tvList.Visible = True
                    Me.tvList.SelectedNode = Me.tvList.Nodes(0)
                    Me.tvList.Focus()

                    Me.btnOK.Enabled = True
                End If

            End Sub

            Private Function DownloadFanart(ByVal iTag As ImageTag) As Image
                Dim sHTTP As New HTTP

                Using tImage As New Images
                    If Not String.IsNullOrEmpty(iTag.Path) AndAlso File.Exists(iTag.Path) Then
                        tImage.FromFile(iTag.Path)
                    ElseIf Not String.IsNullOrEmpty(iTag.Path) AndAlso Not String.IsNullOrEmpty(iTag.URL) Then
                        Me.lblStatus.Text = Master.eLang.GetString(999, "Downloading Fullsize Fanart Image...")
                        Me.pbStatus.Style = ProgressBarStyle.Marquee
                        Me.pnlStatus.Visible = True

                        Application.DoEvents()

                        tImage.FromWeb(iTag.URL)
                        Directory.CreateDirectory(Directory.GetParent(iTag.Path).FullName)
                        tImage.Save(iTag.Path)

                        sHTTP = Nothing

                        Me.pnlStatus.Visible = False
                    End If

                    Return tImage.Image
                End Using
            End Function

            Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
                Master.currShow.ShowPosterPath = TVDBImages.ShowPoster.LocalFile
                If Not String.IsNullOrEmpty(TVDBImages.ShowFanart.LocalFile) AndAlso File.Exists(TVDBImages.ShowFanart.LocalFile) Then
                    TVDBImages.ShowFanart.Image.FromFile(TVDBImages.ShowFanart.LocalFile)
                    Master.currShow.ShowFanartPath = TVDBImages.ShowFanart.LocalFile
                ElseIf Not String.IsNullOrEmpty(TVDBImages.ShowFanart.URL) AndAlso Not String.IsNullOrEmpty(TVDBImages.ShowFanart.LocalFile) Then
                    TVDBImages.ShowFanart.Image.FromWeb(TVDBImages.ShowFanart.URL)
                    If Not IsNothing(TVDBImages.ShowFanart.Image.Image) Then
                        Directory.CreateDirectory(Directory.GetParent(TVDBImages.ShowFanart.LocalFile).FullName)
                        TVDBImages.ShowFanart.Image.Save(TVDBImages.ShowFanart.LocalFile)
                        Master.currShow.ShowFanartPath = TVDBImages.ShowFanart.LocalFile
                    End If
                End If

                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End Sub

            Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
                If Me.bwLoadData.IsBusy Then Me.bwLoadData.CancelAsync()
                If Me.bwLoadImages.IsBusy Then Me.bwLoadImages.CancelAsync()

                While Me.bwLoadData.IsBusy OrElse Me.bwLoadImages.IsBusy
                    Application.DoEvents()
                End While

                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            End Sub
        End Class
    End Class
End Namespace