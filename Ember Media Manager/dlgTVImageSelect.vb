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

    Private Sub SetUp()
        Dim cEI As EpisodeImage
        Dim cSI As SeasonImage

        For Each sEpisode As Master.DBTV In Me._dbtvlist
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
            End If
            If Not String.IsNullOrEmpty(sEpisode.EpFanartPath) Then
                cEI.Fanart.FromFile(sEpisode.EpFanartPath)
            End If
            Me._episodeimagelist.Add(cEI)

            If _seasonimagelist.Where(Function(s) s.Season = cEI.Season).Count = 0 Then
                cSI = New SeasonImage
                cSI.Season = cEI.Season
                If Not String.IsNullOrEmpty(sEpisode.SeasonPosterPath) Then
                    cSI.Poster.FromFile(sEpisode.SeasonPosterPath)
                End If
                If Not String.IsNullOrEmpty(sEpisode.SeasonFanartPath) Then
                    cSI.Fanart.FromFile(sEpisode.SeasonFanartPath)
                End If
                Me._seasonimagelist.Add(cSI)
            End If
        Next

        Me.tvList.Nodes.Add("sposter", Master.eLang.GetString(999, "Show Poster"))
        Me.tvList.Nodes.Add("sfanart", Master.eLang.GetString(999, "Show Fanart"))

        Dim TnS As TreeNode
        For Each cSeason As SeasonImage In Me._seasonimagelist
            TnS = New TreeNode(String.Format(Master.eLang.GetString(999, "Season {0}"), cSeason.Season))
            TnS.Nodes.Add(String.Concat("p", cSeason.ToString), Master.eLang.GetString(999, "Season Poster"))
            TnS.Nodes.Add(String.Concat("f", cSeason.ToString), Master.eLang.GetString(999, "Season Fanart"))
            Me.tvList.Nodes.Add(TnS)
        Next
    End Sub

    Private Sub DownloadAllImages()
        Dim cEp As Episode
        Dim cSeason As Season
        Dim cShowP As ShowPoster
        Dim cShowF As Fanart
        Dim cPost As Poster

        For Each Epi As Media.EpisodeDetails In Master.tmpTVDBShow.Episodes
            cEp = New Episode
            cEp.Season = Epi.Season
            cEp.Episode = Epi.Episode
            If Not File.Exists(Epi.LocalFile) Then
                If Not String.IsNullOrEmpty(Epi.PosterURL) Then
                    cEp.Image.FromWeb(Epi.PosterURL)
                    Directory.CreateDirectory(Directory.GetParent(Epi.LocalFile).FullName)
                    cEp.Image.Save(Epi.LocalFile)
                End If
            Else
                cEp.Image.FromFile(Epi.LocalFile)
            End If
            EpisodeList.Add(cEp)
        Next

        For Each Seas As TVDB.TVDBSeasonPoster In Master.tmpTVDBShow.SeasonPosters
            cSeason = New Season
            cSeason.Info = Seas
            If Not File.Exists(Seas.LocalFile) Then
                If Not String.IsNullOrEmpty(Seas.URL) Then
                    cSeason.Image.FromWeb(Seas.URL)
                    Directory.CreateDirectory(Directory.GetParent(Seas.LocalFile).FullName)
                    cSeason.Image.Save(Seas.LocalFile)
                End If
            Else
                cSeason.Image.FromFile(Seas.LocalFile)
            End If
            SeasonList.Add(cSeason)
        Next

        For Each SPost As TVDB.TVDBShowPoster In Master.tmpTVDBShow.ShowPosters
            cShowP = New ShowPoster
            cShowP.Info = SPost
            If Not File.Exists(SPost.LocalFile) Then
                If Not String.IsNullOrEmpty(SPost.URL) Then
                    cShowP.Image.FromWeb(SPost.URL)
                    Directory.CreateDirectory(Directory.GetParent(SPost.LocalFile).FullName)
                    cShowP.Image.Save(SPost.LocalFile)
                End If
            Else
                cShowP.Image.FromFile(SPost.LocalFile)
            End If
            ShowPosterList.Add(cShowP)
        Next

        For Each SFan As TVDB.TVDBFanart In Master.tmpTVDBShow.Fanart
            cShowF = New Fanart
            cShowF.Info = SFan
            If Not File.Exists(SFan.LocalFile) Then
                If Not String.IsNullOrEmpty(SFan.URL) Then
                    cShowF.Image.FromWeb(SFan.URL)
                    Directory.CreateDirectory(Directory.GetParent(SFan.LocalFile).FullName)
                    cShowF.Image.Save(SFan.LocalFile)
                End If
            Else
                cShowF.Image.FromFile(SFan.LocalFile)
            End If
            FanartList.Add(cShowF)
        Next

        For Each Post As TVDB.TVDBPoster In Master.tmpTVDBShow.Posters
            cPost = New Poster
            cPost.Info = Post
            If Not File.Exists(Post.LocalFile) Then
                If Not String.IsNullOrEmpty(Post.URL) Then
                    cPost.Image.FromWeb(Post.URL)
                    Directory.CreateDirectory(Directory.GetParent(Post.LocalFile).FullName)
                    cPost.Image.Save(Post.LocalFile)
                End If
            Else
                cPost.Image.FromFile(Post.LocalFile)
            End If
            GenericPosterList.Add(cPost)
        Next
    End Sub

    Public Sub SetDefaults()

        Dim iSeason As Integer = -1
        Dim iEpisode As Integer = -1

        Me._showposter = ShowPosterList.SingleOrDefault(Function(p) Not IsNothing(p.Image)).Image
        Me._showfanart = FanartList.SingleOrDefault(Function(f) Not IsNothing(f.Image)).Image

        For Each cSeason As SeasonImage In Me._seasonimagelist
            iSeason = cSeason.Season
            cSeason.Poster = SeasonList.SingleOrDefault(Function(p) Not IsNothing(p.Image) AndAlso p.Info.Season = iSeason).Image
            cSeason.Fanart = Me._showfanart
        Next

        For Each cEpisode As EpisodeImage In Me._episodeimagelist
            iSeason = cEpisode.Season
            iEpisode = cEpisode.Episode
            cEpisode.Poster = EpisodeList.SingleOrDefault(Function(p) Not IsNothing(p.Image) AndAlso p.Episode = iEpisode AndAlso p.Season = iSeason).Image
        Next
    End Sub

    Private Sub SaveAll()
        Dim iEp As Integer = -1
        Dim iSea As Integer = -1

        Dim spPath As String = Me._showposter.SaveAsShowPoster(Me._dbtvlist(0))
        Dim sfPath As String = Me._showposter.SaveAsShowFanart(Me._dbtvlist(0))

        For Each Episode As Master.DBTV In Me._dbtvlist
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
        Next
    End Sub

    Public Overloads Function ShowDialog(ByVal TList As List(Of Master.DBTV)) As System.Windows.Forms.DialogResult
        Me._dbtvlist = TList
        Me.SetUp()
        Me.DownloadAllImages()

        Return MyBase.ShowDialog
    End Function
End Class