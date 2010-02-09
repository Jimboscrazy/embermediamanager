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
Imports System.Text.RegularExpressions

Public Class dlgTVImageSelect
    Private _id As Integer = -1
    Private _season As Integer = -999
    Private _dbtvlist As New List(Of Master.DBTV)

    Private ShowPosterList As New List(Of ShowPoster)
    Private FanartList As New List(Of Fanart)
    Private SeasonList As New List(Of Season)
    Private EpisodeList As New List(Of Episode)
    Private GenericPosterList As New List(Of Poster)

    Private _showposter As New Images
    Private _showfanart As New Images
    Private _seasonimagelist As New List(Of SeasonImage)
    Private _episodeimagelist As New List(Of EpisodeImage)

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


    Private Class SeasonImage
        Private _season As Integer
        Private _poster As Images
        Private _fanart As Images
        Private _posterneedssave As Boolean
        Private _fanartneedssave As Boolean

        Public Property Season() As Integer
            Get
                Return Me._season
            End Get
            Set(ByVal value As Integer)
                Me._season = value
            End Set
        End Property

        Public Property Poster() As Images
            Get
                Return Me._poster
            End Get
            Set(ByVal value As Images)
                Me._poster = value
            End Set
        End Property

        Public Property Fanart() As Images
            Get
                Return Me._fanart
            End Get
            Set(ByVal value As Images)
                Me._fanart = value
            End Set
        End Property

        Public Property PosterNeedsSave() As Boolean
            Get
                Return Me._posterneedssave
            End Get
            Set(ByVal value As Boolean)
                Me._posterneedssave = value
            End Set
        End Property

        Public Property FanartNeedsSave() As Boolean
            Get
                Return Me._fanartneedssave
            End Get
            Set(ByVal value As Boolean)
                Me._fanartneedssave = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._season = -1
            Me._poster = New Images
            Me._fanart = New Images
            Me._posterneedssave = False
            Me._fanartneedssave = False
        End Sub
    End Class

    Private Class EpisodeImage
        Private _season As Integer
        Private _episode As Integer
        Private _poster As Images
        Private _fanart As Images
        Private _posterneedssave As Boolean
        Private _fanartneedssave As Boolean

        Public Property Season() As Integer
            Get
                Return Me._season
            End Get
            Set(ByVal value As Integer)
                Me._season = value
            End Set
        End Property

        Public Property Episode() As Integer
            Get
                Return Me._episode
            End Get
            Set(ByVal value As Integer)
                Me._episode = value
            End Set
        End Property

        Public Property Poster() As Images
            Get
                Return Me._poster
            End Get
            Set(ByVal value As Images)
                Me._poster = value
            End Set
        End Property

        Public Property Fanart() As Images
            Get
                Return Me._fanart
            End Get
            Set(ByVal value As Images)
                Me._fanart = value
            End Set
        End Property

        Public Property PosterNeedsSave() As Boolean
            Get
                Return Me._posterneedssave
            End Get
            Set(ByVal value As Boolean)
                Me._posterneedssave = value
            End Set
        End Property

        Public Property FanartNeedsSave() As Boolean
            Get
                Return Me._fanartneedssave
            End Get
            Set(ByVal value As Boolean)
                Me._fanartneedssave = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._season = -1
            Me._episode = -1
            Me._poster = New Images
            Me._fanart = New Images
            Me._posterneedssave = False
            Me._fanartneedssave = False
        End Sub
    End Class

    Private Class ShowPoster
        Private _info As TVDB.TVDBShowPoster
        Private _image As Images

        Public Property Info() As TVDB.TVDBShowPoster
            Get
                Return Me._info
            End Get
            Set(ByVal value As TVDB.TVDBShowPoster)
                Me._info = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._info = New TVDB.TVDBShowPoster
            Me._image = New Images
        End Sub
    End Class

    Private Class Fanart
        Private _info As TVDB.TVDBFanart
        Private _image As Images

        Public Property Info() As TVDB.TVDBFanart
            Get
                Return Me._info
            End Get
            Set(ByVal value As TVDB.TVDBFanart)
                Me._info = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._info = New TVDB.TVDBFanart
            Me._image = New Images
        End Sub
    End Class

    Private Class Season
        Private _info As TVDB.TVDBSeasonPoster
        Private _image As Images

        Public Property Info() As TVDB.TVDBSeasonPoster
            Get
                Return Me._info
            End Get
            Set(ByVal value As TVDB.TVDBSeasonPoster)
                Me._info = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._info = New TVDB.TVDBSeasonPoster
            Me._image = New Images
        End Sub
    End Class

    Private Class Episode
        Private _episode As Integer
        Private _season As Integer
        Private _image As Images

        Public Property Episode() As Integer
            Get
                Return Me._episode
            End Get
            Set(ByVal value As Integer)
                Me._episode = value
            End Set
        End Property

        Public Property Season() As Integer
            Get
                Return Me._season
            End Get
            Set(ByVal value As Integer)
                Me._season = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._episode = -1
            Me._season = -1
            Me._image = New Images
        End Sub
    End Class

    Private Class Poster
        Private _info As TVDB.TVDBPoster
        Private _image As Images

        Public Property Info() As TVDB.TVDBPoster
            Get
                Return Me._info
            End Get
            Set(ByVal value As TVDB.TVDBPoster)
                Me._info = value
            End Set
        End Property

        Public Property Image() As Images
            Get
                Return Me._image
            End Get
            Set(ByVal value As Images)
                Me._image = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._info = New TVDB.TVDBPoster
            Me._image = New Images
        End Sub
    End Class

    Private Sub GenerateList()
        Try
            Me.tvList.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Show Poster"), .Tag = "showp"})
            Me.tvList.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Show Fanart"), .Tag = "showf"})

            Dim TnS As TreeNode
            For Each cSeason As SeasonImage In Me._seasonimagelist
                Try
                    TnS = New TreeNode(String.Format(Master.eLang.GetString(999, "Season {0}"), cSeason.Season))
                    TnS.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Season Poster"), .Tag = String.Concat("p", cSeason.Season.ToString)})
                    TnS.Nodes.Add(New TreeNode With {.Text = Master.eLang.GetString(999, "Season Fanart"), .Tag = String.Concat("f", cSeason.Season.ToString)})
                    Me.tvList.Nodes.Add(TnS)
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub DownloadAllImages()
        Dim cEp As Episode
        Dim cSeason As Season
        Dim cShowP As ShowPoster
        Dim cShowF As Fanart
        Dim cPost As Poster
        Dim iProgress As Integer = 1

        Try
            Me.bwLoadImages.ReportProgress(Master.tmpTVDBShow.Episodes.Count + Master.tmpTVDBShow.SeasonPosters.Count + Master.tmpTVDBShow.ShowPosters.Count + Master.tmpTVDBShow.Fanart.Count + Master.tmpTVDBShow.Posters.Count, "max")

            For Each Epi As Media.EpisodeDetails In Master.tmpTVDBShow.Episodes
                Try
                    cEp = New Episode
                    cEp.Season = Epi.Season
                    cEp.Episode = Epi.Episode
                    If Not File.Exists(Epi.LocalFile) Then
                        If Not String.IsNullOrEmpty(Epi.PosterURL) Then
                            cEp.Image.FromWeb(Epi.PosterURL)
                            Directory.CreateDirectory(Directory.GetParent(Epi.LocalFile).FullName)
                            cEp.Image.Save(Epi.LocalFile)
                        Else
                            cEp.Image.Image = Nothing
                        End If
                    Else
                        cEp.Image.FromFile(Epi.LocalFile)
                    End If
                    EpisodeList.Add(cEp)
                    Me.bwLoadImages.ReportProgress(iProgress, "progress")
                    iProgress += 1
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next

            For Each Seas As TVDB.TVDBSeasonPoster In Master.tmpTVDBShow.SeasonPosters
                Try
                    cSeason = New Season
                    cSeason.Info = Seas
                    If Not File.Exists(Seas.LocalFile) Then
                        If Not String.IsNullOrEmpty(Seas.URL) Then
                            cSeason.Image.FromWeb(Seas.URL)
                            Directory.CreateDirectory(Directory.GetParent(Seas.LocalFile).FullName)
                            cSeason.Image.Save(Seas.LocalFile)
                        Else
                            cSeason.Image.Image = Nothing
                        End If
                    Else
                        cSeason.Image.FromFile(Seas.LocalFile)
                    End If
                    SeasonList.Add(cSeason)
                    Me.bwLoadImages.ReportProgress(iProgress, "progress")
                    iProgress += 1
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next

            For Each SPost As TVDB.TVDBShowPoster In Master.tmpTVDBShow.ShowPosters
                Try
                    cShowP = New ShowPoster
                    cShowP.Info = SPost
                    If Not File.Exists(SPost.LocalFile) Then
                        If Not String.IsNullOrEmpty(SPost.URL) Then
                            cShowP.Image.FromWeb(SPost.URL)
                            Directory.CreateDirectory(Directory.GetParent(SPost.LocalFile).FullName)
                            cShowP.Image.Save(SPost.LocalFile)
                        Else
                            cShowP.Image.Image = Nothing
                        End If
                    Else
                        cShowP.Image.FromFile(SPost.LocalFile)
                    End If
                    ShowPosterList.Add(cShowP)
                    Me.bwLoadImages.ReportProgress(iProgress, "progress")
                    iProgress += 1
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next

            For Each SFan As TVDB.TVDBFanart In Master.tmpTVDBShow.Fanart
                Try
                    cShowF = New Fanart
                    cShowF.Info = SFan
                    If Not File.Exists(SFan.LocalFile) Then
                        If Not String.IsNullOrEmpty(SFan.URL) Then
                            cShowF.Image.FromWeb(SFan.URL)
                            Directory.CreateDirectory(Directory.GetParent(SFan.LocalFile).FullName)
                            cShowF.Image.Save(SFan.LocalFile)
                        Else
                            cShowF.Image.Image = Nothing
                        End If
                    Else
                        cShowF.Image.FromFile(SFan.LocalFile)
                    End If
                    FanartList.Add(cShowF)
                    Me.bwLoadImages.ReportProgress(iProgress, "progress")
                    iProgress += 1
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next

            For Each Post As TVDB.TVDBPoster In Master.tmpTVDBShow.Posters
                Try
                    cPost = New Poster
                    cPost.Info = Post
                    If Not File.Exists(Post.LocalFile) Then
                        If Not String.IsNullOrEmpty(Post.URL) Then
                            cPost.Image.FromWeb(Post.URL)
                            Directory.CreateDirectory(Directory.GetParent(Post.LocalFile).FullName)
                            cPost.Image.Save(Post.LocalFile)
                        Else
                            cPost.Image.Image = Nothing
                        End If
                    Else
                        cPost.Image.FromFile(Post.LocalFile)
                    End If
                    GenericPosterList.Add(cPost)
                    Me.bwLoadImages.ReportProgress(iProgress, "progress")
                    iProgress += 1
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub SetDefaults()

        Dim iSeason As Integer = -1
        Dim iEpisode As Integer = -1
        Dim iProgress As Integer = 3

        Dim tEp As Episode
        Dim tSea As Season

        Try
            Me.bwLoadData.ReportProgress(Me._seasonimagelist.Count + Me._episodeimagelist.Count + 2, "defaults")

            If IsNothing(Me._showposter.Image) Then
                Dim tSP As ShowPoster = ShowPosterList.SingleOrDefault(Function(p) Not IsNothing(p.Image.Image))
                If Not IsNothing(tSP) Then Me._showposter.Image = tSP.Image.Image
            End If
            Me.bwLoadData.ReportProgress(1, "progress")

            If IsNothing(Me._showfanart.Image) Then
                Dim tSF As Fanart = FanartList.SingleOrDefault(Function(f) Not IsNothing(f.Image.Image))
                If Not IsNothing(tSF) Then Me._showfanart.Image = tSF.Image.Image
            End If
            Me.bwLoadData.ReportProgress(2, "progress")

            For Each cSeason As SeasonImage In Me._seasonimagelist
                Try
                    iSeason = cSeason.Season
                    If IsNothing(cSeason.Poster.Image) Then
                        tSea = SeasonList.SingleOrDefault(Function(p) Not IsNothing(p.Image.Image) AndAlso p.Info.Season = iSeason)
                        If Not IsNothing(tSea) Then cSeason.Poster.Image = tSea.Image.Image
                    End If
                    If IsNothing(cSeason.Fanart.Image) AndAlso Not IsNothing(Me._showfanart.Image) Then cSeason.Fanart.Image = Me._showfanart.Image

                    Me.bwLoadData.ReportProgress(iProgress, "progress")
                    iProgress += 1
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next

            For Each cEpisode As EpisodeImage In Me._episodeimagelist
                Try
                    iSeason = cEpisode.Season
                    iEpisode = cEpisode.Episode
                    If IsNothing(cEpisode.Poster.Image) Then
                        tEp = EpisodeList.SingleOrDefault(Function(p) Not IsNothing(p.Image.Image) AndAlso p.Episode = iEpisode AndAlso p.Season = iSeason)
                        If Not IsNothing(tEp) Then cEpisode.Poster.Image = tEp.Image.Image
                    End If
                    If IsNothing(cEpisode.Fanart.Image) AndAlso Not IsNothing(Me._showfanart.Image) Then cEpisode.Fanart.Image = Me._showfanart.Image
                    Me.bwLoadData.ReportProgress(iProgress, "progress")
                    iProgress += 1
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub SaveAll()
        Dim iEp As Integer = -1
        Dim iSea As Integer = -1

        Try
            Dim spPath As String = Me._showposter.SaveAsShowPoster(Me._dbtvlist(0))
            Dim sfPath As String = Me._showposter.SaveAsShowFanart(Me._dbtvlist(0))

            For Each Episode As Master.DBTV In Me._dbtvlist
                Try
                    Episode.ShowPosterPath = spPath
                    Episode.ShowFanartPath = sfPath

                    iEp = Episode.TVEp.Episode
                    iSea = Episode.TVEp.Season
                    Dim cEp = From cEpisode As EpisodeImage In _episodeimagelist Where cEpisode.Episode = iEp AndAlso cEpisode.Season = iSea Take 1
                    If cEp.Count > 0 Then
                        If Not IsNothing(cEp(0).Poster.Image) AndAlso cEp(0).PosterNeedsSave Then Episode.EpPosterPath = cEp(0).Poster.SaveAsEpPoster(Episode)
                        If Not IsNothing(cEp(0).Fanart.Image) AndAlso cEp(0).FanartNeedsSave Then Episode.EpFanartPath = cEp(0).Fanart.SaveAsEpFanart(Episode)
                    End If

                    Dim cSea = From cSeason As SeasonImage In _seasonimagelist Where cSeason.Season = iSea Take 1
                    If cSea.Count > 0 Then
                        If Not IsNothing(cSea(0).Poster.Image) AndAlso cSea(0).PosterNeedsSave Then Episode.SeasonPosterPath = cSea(0).Poster.SaveAsSeasonPoster(Episode)
                        If Not IsNothing(cSea(0).Fanart.Image) AndAlso cSea(0).FanartNeedsSave Then Episode.SeasonFanartPath = cSea(0).Fanart.SaveAsSeasonFanart(Episode)
                    End If
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

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
                    Me.pbCurrent.Image = Me._showposter.Image
                    iCount = ShowPosterList.Count
                    For i = 0 To iCount - 1
                        Me.AddImage(ShowPosterList(i).Image.Image, String.Format("{0}x{1}", ShowPosterList(i).Image.Image.Width, ShowPosterList(i).Image.Image.Height), i)
                    Next

                    For i = 0 To GenericPosterList.Count - 1
                        Me.AddImage(GenericPosterList(i).Image.Image, String.Format("{0}x{1}", GenericPosterList(i).Image.Image.Width, GenericPosterList(i).Image.Image.Height), i + iCount)
                    Next
                ElseIf e.Node.Tag.ToString = "showf" Then
                    Me.SelSeason = -999
                    Me.SelIsPoster = False
                    Me.pbCurrent.Image = Me._showfanart.Image
                    For i = 0 To FanartList.Count - 1
                        Me.AddImage(FanartList(i).Image.Image, String.Format("{0}x{1}", FanartList(i).Image.Image.Width, FanartList(i).Image.Image.Height), i)
                    Next
                Else
                    Dim tMatch As Match = Regex.Match(e.Node.Tag.ToString, "(?<type>f|p)(?<num>[0-9]+)")
                    If tMatch.Success Then
                        If tMatch.Groups("type").Value = "f" Then
                            Me.SelSeason = Convert.ToInt32(tMatch.Groups("num").Value)
                            Me.SelIsPoster = False
                            Me.pbCurrent.Image = Me._seasonimagelist.SingleOrDefault(Function(f) f.Season = Convert.ToInt32(tMatch.Groups("num").Value)).Fanart.Image
                            For i = 0 To FanartList.Count - 1
                                Me.AddImage(FanartList(i).Image.Image, String.Format("{0}x{1}", FanartList(i).Image.Image.Width, FanartList(i).Image.Image.Height), i)
                            Next
                        ElseIf tMatch.Groups("type").Value = "p" Then
                            Me.SelSeason = Convert.ToInt32(tMatch.Groups("num").Value)
                            Me.SelIsPoster = True
                            Me.pbCurrent.Image = Me._seasonimagelist.SingleOrDefault(Function(f) f.Season = Convert.ToInt32(tMatch.Groups("num").Value)).Poster.Image
                            iCount = 0
                            For Each SImage As Season In SeasonList.Where(Function(s) s.Info.Season = Convert.ToInt32(tMatch.Groups("num").Value))
                                Me.AddImage(SImage.Image.Image, String.Format("{0}x{1}", SImage.Image.Image.Width, SImage.Image.Image.Height), iCount)
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

    Private Sub AddImage(ByVal iImage As Image, ByVal sDescription As String, ByVal iIndex As Integer)

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
            Me.pnlImages.Controls.Add(Me.pnlImage(iIndex))
            Me.pnlImage(iIndex).Controls.Add(Me.pbImage(iIndex))
            Me.pnlImage(iIndex).Controls.Add(Me.lblImage(iIndex))
            Me.pnlImage(iIndex).BringToFront()
            AddHandler pbImage(iIndex).Click, AddressOf pbImage_Click
            AddHandler pbImage(iIndex).DoubleClick, AddressOf pbImage_DoubleClick
            AddHandler pnlImage(iIndex).Click, AddressOf pnlImage_Click
            AddHandler lblImage(iIndex).Click, AddressOf lblImage_Click

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
        Me.DoSelect(Convert.ToInt32(DirectCast(sender, PictureBox).Name), DirectCast(sender, PictureBox).Image)
    End Sub

    Private Sub pbImage_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Using dImgView As New dlgImgView
            dImgView.ShowDialog(DirectCast(sender, PictureBox).Image)
        End Using
    End Sub

    Private Sub pnlImage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DoSelect(Convert.ToInt32(DirectCast(sender, Panel).Name), Me.pbImage(Convert.ToInt32(DirectCast(sender, Panel).Name)).Image)
    End Sub

    Private Sub lblImage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.DoSelect(Convert.ToInt32(DirectCast(sender, Label).Name), Me.pbImage(Convert.ToInt32(DirectCast(sender, Label).Name)).Image)
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

    Private Sub DoSelect(ByVal iIndex As Integer, ByVal SelImage As Image)
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
                    Me._showposter.Image = SelImage
                Else
                    Me._showfanart.Image = SelImage
                End If
            Else
                If Me.SelIsPoster Then
                    Me._seasonimagelist.SingleOrDefault(Function(s) s.Season = Me.SelSeason).Poster.Image = SelImage
                Else
                    Me._seasonimagelist.SingleOrDefault(Function(s) s.Season = Me.SelSeason).Fanart.Image = SelImage
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwLoadData_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadData.DoWork
        Dim cEI As EpisodeImage
        Dim cSI As SeasonImage
        Dim iProgress As Integer = 1

        Try
            Using SQLCount As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLCount.CommandText = String.Concat("SELECT COUNT(ID) AS eCount FROM TVEps WHERE TVShowID = ", Me._id, ";")
                Using SQLRCount As SQLite.SQLiteDataReader = SQLCount.ExecuteReader
                    If Convert.ToInt32(SQLRCount("eCount")) > 0 Then
                        Me.bwLoadData.ReportProgress(Convert.ToInt32(SQLRCount("eCount")), "max")
                        Using SQLCommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            SQLCommand.CommandText = String.Concat("SELECT ID FROM TVEps WHERE TVShowID = ", Me._id, ";")
                            Using SQLReader As SQLite.SQLiteDataReader = SQLCommand.ExecuteReader
                                While SQLReader.Read
                                    Me._dbtvlist.Add(Master.DB.LoadTVEpFromDB(Convert.ToInt64(SQLReader("ID")), True))
                                    Me.bwLoadData.ReportProgress(iProgress, "progress")
                                    iProgress += 1
                                End While
                            End Using
                        End Using
                    End If
                End Using
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.bwLoadData.ReportProgress(Me._dbtvlist.Count, "current")
        iProgress = 1

        For Each sEpisode As Master.DBTV In Me._dbtvlist
            Try
                If IsNothing(Me._showposter.Image) AndAlso Not String.IsNullOrEmpty(sEpisode.ShowPosterPath) Then
                    Me._showposter.FromFile(sEpisode.ShowPosterPath)
                End If

                If IsNothing(Me._showfanart.Image) AndAlso Not String.IsNullOrEmpty(sEpisode.ShowFanartPath) Then
                    Me._showfanart.FromFile(sEpisode.ShowFanartPath)
                End If

                cEI = New EpisodeImage
                cEI.Season = sEpisode.TVEp.Season
                cEI.Episode = sEpisode.TVEp.Episode
                If Not String.IsNullOrEmpty(sEpisode.EpPosterPath) Then
                    cEI.Poster.FromFile(sEpisode.EpPosterPath)
                Else
                    cEI.Poster.Image = Nothing
                End If
                If Not String.IsNullOrEmpty(sEpisode.EpFanartPath) Then
                    cEI.Fanart.FromFile(sEpisode.EpFanartPath)
                Else
                    cEI.Fanart.Image = Nothing
                End If
                Me._episodeimagelist.Add(cEI)

                If _seasonimagelist.Where(Function(s) s.Season = cEI.Season).Count = 0 Then
                    cSI = New SeasonImage
                    cSI.Season = cEI.Season
                    If Not String.IsNullOrEmpty(sEpisode.SeasonPosterPath) Then
                        cSI.Poster.FromFile(sEpisode.SeasonPosterPath)
                    Else
                        cSI.Poster.Image = Nothing
                    End If
                    If Not String.IsNullOrEmpty(sEpisode.SeasonFanartPath) Then
                        cSI.Fanart.FromFile(sEpisode.SeasonFanartPath)
                    Else
                        cSI.Fanart.Image = Nothing
                    End If
                    Me._seasonimagelist.Add(cSI)
                End If

                Me.bwLoadData.ReportProgress(iProgress, "progress")
                iProgress += 1
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Next

        Me.SetDefaults()
    End Sub

    Private Sub bwLoadData_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadData.ProgressChanged
        Try
            If e.UserState.ToString = "progress" Then
                Me.pbStatus.Value = e.ProgressPercentage
            ElseIf e.UserState.ToString = "current" Then
                Me.lblStatus.Text = Master.eLang.GetString(999, "Loading Current Images...")
                Me.pbStatus.Value = 0
                Me.pbStatus.Maximum = e.ProgressPercentage
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

    Private Sub bwLoadData_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwLoadData.RunWorkerCompleted

        If Not e.Cancelled Then
            Me.GenerateList()

            Me.lblStatus.Text = Master.eLang.GetString(999, "(Down)Loading New Images...")
            Me.bwLoadImages.WorkerReportsProgress = True
            Me.bwLoadImages.WorkerSupportsCancellation = True
            Me.bwLoadImages.RunWorkerAsync()
        End If

    End Sub

    Private Sub dlgTVImageSelect_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.bwLoadData.WorkerReportsProgress = True
        Me.bwLoadData.WorkerSupportsCancellation = True
        Me.bwLoadData.RunWorkerAsync()
    End Sub

    Private Sub bwLoadImages_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwLoadImages.DoWork
        Me.DownloadAllImages()
    End Sub

    Private Sub bwLoadImages_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwLoadImages.ProgressChanged
        Try
            If e.UserState.ToString = "progress" Then
                Me.pbStatus.Value = e.ProgressPercentage
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
        End If

    End Sub
End Class