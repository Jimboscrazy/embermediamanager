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
Option Strict On

Imports System.Xml.Serialization

Namespace Media
    <XmlRoot("movie")> _
    Public Class Movie
        Private _imdbid As String
        Private _title As String
        Private _originaltitle As String
        Private _year As String
        Private _rating As String
        Private _votes As String
        Private _mpaa As String
        Private _top250 As String
        Private _outline As String
        Private _plot As String
        Private _tagline As String
        Private _trailer As String
        Private _certification As String
        Private _genre As String
        Private _runtime As String
        Private _releaseDate As String
        Private _studio As String
        Private _studioreal As String
        Private _director As String
        Private _credits As String
        Private _playcount As String
        Private _watched As String
        Private _file As String
        Private _path As String
        Private _filenameandpath As String
        Private _status As String
        Private _thumbs As New Poster
        Private _fanart As New Fanart
        Private _actors As New List(Of Person)
        Private _fileInfo As New MediaInfo.Fileinfo
        Private _sets As New List(Of [Set])

        <XmlIgnore()> _
        Public Property IMDBID() As String
            Get
                Return Me._imdbid.Replace("tt", String.Empty).Trim
            End Get
            Set(ByVal value As String)
                Me._imdbid = value
            End Set
        End Property

        <XmlElement("id")> _
        Public Property ID() As String
            Get
                Return If(Strings.Left(Me._imdbid, 2) = "tt", Me._imdbid.Trim, String.Concat("tt", Me._imdbid))
            End Get
            Set(ByVal value As String)
                Me._imdbid = If(Strings.Left(value, 2) = "tt", value.Trim, String.Concat("tt", value))
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property IDSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._imdbid)
            End Get
        End Property

        <XmlElement("title")> _
        Public Property Title() As String
            Get
                Return Me._title
            End Get
            Set(ByVal value As String)
                Me._title = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property TitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._title)
            End Get
        End Property

        <XmlElement("originaltitle")> _
        Public Property OriginalTitle() As String
            Get
                Return Me._originaltitle
            End Get
            Set(ByVal value As String)
                Me._originaltitle = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property OriginalTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._originaltitle)
            End Get
        End Property

        <XmlElement("year")> _
        Public Property Year() As String
            Get
                Return Me._year
            End Get
            Set(ByVal value As String)
                Me._year = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property YearSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._year)
            End Get
        End Property

        <XmlElement("rating")> _
        Public Property Rating() As String
            Get
                Return Me._rating.Replace(",", ".")
            End Get
            Set(ByVal value As String)
                Me._rating = value.Replace(",", ".")
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property RatingSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._rating)
            End Get
        End Property

        <XmlElement("votes")> _
        Public Property Votes() As String
            Get
                Return Me._votes
            End Get
            Set(ByVal value As String)
                Me._votes = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property VotesSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._votes)
            End Get
        End Property

        <XmlElement("mpaa")> _
        Public Property MPAA() As String
            Get
                Return Me._mpaa
            End Get
            Set(ByVal value As String)
                Me._mpaa = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property MPAASpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._mpaa)
            End Get
        End Property

        <XmlElement("top250")> _
        Public Property Top250() As String
            Get
                Return Me._top250
            End Get
            Set(ByVal value As String)
                Me._top250 = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property Top250Specified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._top250)
            End Get
        End Property

        <XmlElement("outline")> _
        Public Property Outline() As String
            Get
                Return Me._outline
            End Get
            Set(ByVal value As String)
                Me._outline = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property OutlineSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._outline)
            End Get
        End Property

        <XmlElement("plot")> _
        Public Property Plot() As String
            Get
                Return Me._plot
            End Get
            Set(ByVal value As String)
                Me._plot = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property PlotSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._plot)
            End Get
        End Property

        <XmlElement("tagline")> _
        Public Property Tagline() As String
            Get
                Return Me._tagline
            End Get
            Set(ByVal value As String)
                Me._tagline = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property TaglineSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._tagline)
            End Get
        End Property

        <XmlElement("trailer")> _
        Public Property Trailer() As String
            Get
                Return Me._trailer
            End Get
            Set(ByVal value As String)
                Me._trailer = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property TrailerSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._trailer)
            End Get
        End Property

        <XmlElement("certification")> _
        Public Property Certification() As String
            Get
                Return Me._certification
            End Get
            Set(ByVal value As String)
                Me._certification = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property CertificationSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._certification)
            End Get
        End Property

        <XmlElement("genre")> _
        Public Property Genre() As String
            Get
                Return Me._genre
            End Get
            Set(ByVal value As String)
                Me._genre = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property GenreSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._genre)
            End Get
        End Property

        <XmlElement("studio")> _
        Public Property Studio() As String
            Get
                Return Me._studio
            End Get
            Set(ByVal value As String)
                Me._studio = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property StudioSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._studio)
            End Get
        End Property

        <XmlElement("studioreal")> _
        Public Property StudioReal() As String
            Get
                Return Me._studioreal
            End Get
            Set(ByVal value As String)
                Me._studioreal = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property StudioRealSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._studioreal)
            End Get
        End Property

        <XmlElement("runtime")> _
        Public Property Runtime() As String
            Get
                Return Me._runtime
            End Get
            Set(ByVal value As String)
                Me._runtime = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property RuntimeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._runtime)
            End Get
        End Property

        <XmlElement("releasedate")> _
        Public Property ReleaseDate() As String
            Get
                Return Me._releaseDate
            End Get
            Set(ByVal value As String)
                Me._releaseDate = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ReleaseDateSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._releaseDate)
            End Get
        End Property

        <XmlElement("director")> _
        Public Property Director() As String
            Get
                Return Me._director
            End Get
            Set(ByVal value As String)
                Me._director = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property DirectorSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._director)
            End Get
        End Property

        <XmlElement("credits")> _
        Public Property Credits() As String
            Get
                Return Me._credits
            End Get
            Set(ByVal value As String)
                Me._credits = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property CreditsSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._credits)
            End Get
        End Property

        <XmlElement("playcount")> _
        Public Property PlayCount() As String
            Get
                Return Me._playcount
            End Get
            Set(ByVal value As String)
                Me._playcount = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property PlayCountSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._playcount)
            End Get
        End Property

        <XmlElement("watched")> _
        Public Property Watched() As String
            Get
                Return Me._watched
            End Get
            Set(ByVal value As String)
                Me._watched = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property WatchedSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._watched)
            End Get
        End Property

        <XmlElement("file")> _
        Public Property File() As String
            Get
                Return Me._file
            End Get
            Set(ByVal value As String)
                Me._file = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property FileSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._file)
            End Get
        End Property

        <XmlElement("path")> _
        Public Property Path() As String
            Get
                Return Me._path
            End Get
            Set(ByVal value As String)
                Me._path = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property PathSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._path)
            End Get
        End Property

        <XmlElement("filenameandpath")> _
        Public Property FileNameAndPath() As String
            Get
                Return Me._filenameandpath
            End Get
            Set(ByVal value As String)
                Me._filenameandpath = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property FileNameAndPathSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._filenameandpath)
            End Get
        End Property

        <XmlElement("status")> _
        Public Property Status() As String
            Get
                Return Me._status
            End Get
            Set(ByVal value As String)
                Me._status = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property StatusSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._status)
            End Get
        End Property

        <XmlElement("thumbs")> _
        Public Property Thumbs() As Poster
            Get
                Return Me._thumbs
            End Get
            Set(ByVal value As Poster)
                Me._thumbs = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ThumbsSpecified() As Boolean
            Get
                Return Me._thumbs.Thumb.Count > 0
            End Get
        End Property

        <XmlElement("fanart")> _
        Public Property Fanart() As Fanart
            Get
                Return Me._fanart
            End Get
            Set(ByVal value As Fanart)
                Me._fanart = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property FanartSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._fanart.URL)
            End Get
        End Property

        <XmlElement("actor")> _
        Public Property Actors() As List(Of Person)
            Get
                Return Me._actors
            End Get
            Set(ByVal Value As List(Of Person))
                Me._actors = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ActorsSpecified() As Boolean
            Get
                Return Me._actors.Count > 0
            End Get
        End Property

        <XmlElement("fileinfo")> _
        Public Property FileInfo() As MediaInfo.Fileinfo
            Get
                Return Me._fileInfo
            End Get
            Set(ByVal value As MediaInfo.Fileinfo)
                Me._fileInfo = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property FileInfoSpecified() As Boolean
            Get
                If Me._fileInfo.StreamDetails.Video.Count > 0 OrElse _
                Me._fileInfo.StreamDetails.Audio.Count > 0 OrElse _
                 Me._fileInfo.StreamDetails.Subtitle.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        <XmlElement("sets")> _
        Public Property Sets() As List(Of [Set])
            Get
                Return _sets
            End Get
            Set(ByVal value As List(Of [Set]))
                _sets = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property SetsSpecified() As Boolean
            Get
                Return Me._sets.Count > 0
            End Get
        End Property

        Public Sub AddSet(ByVal SetName As String, ByVal Order As Integer)
            Dim tSet = From bSet As [Set] In _sets Where bSet.SetContainer.Set = SetName
            If tSet.Count > 0 Then
                If Order > 0 Then tSet(0).SetContainer.Order = Order.ToString
            Else
                Me._sets.Add(New [Set] With {.SetContainer = New SetContainer With {.Set = SetName, .Order = If(Order > 0, Order.ToString, String.Empty)}})
            End If
        End Sub

        Public Sub RemoveSet(ByVal SetName As String)
            Dim tSet = From bSet As [Set] In _sets Where bSet.SetContainer.Set = SetName
            If tSet.Count > 0 Then
                _sets.Remove(tSet(0))
            End If
        End Sub

        Sub New()
            Me.Clear()
        End Sub

        Public Sub New(ByVal sID As String, ByVal sTitle As String)
            Me.Clear()
            Me._imdbid = sID
            Me._title = sTitle
        End Sub

        Public Sub Clear()
            Me._imdbid = String.Empty
            Me._title = String.Empty
            Me._originaltitle = String.Empty
            Me._year = String.Empty
            Me._rating = String.Empty
            Me._votes = String.Empty
            Me._mpaa = String.Empty
            Me._top250 = String.Empty
            Me._outline = String.Empty
            Me._plot = String.Empty
            Me._tagline = String.Empty
            Me._trailer = String.Empty
            Me._certification = String.Empty
            Me._genre = String.Empty
            Me._runtime = String.Empty
            Me._releaseDate = String.Empty
            Me._studio = String.Empty
            Me._studioreal = String.Empty
            Me._director = String.Empty
            Me._credits = String.Empty
            Me._playcount = String.Empty
            Me._watched = String.Empty
            Me._file = String.Empty
            Me._path = String.Empty
            Me._filenameandpath = String.Empty
            Me._status = String.Empty
            Me._thumbs = New Poster
            Me._fanart = New Fanart
            Me._actors.Clear()
            Me._fileInfo = New MediaInfo.Fileinfo
            Me._sets.Clear()
        End Sub
    End Class

    Public Class Person
        Private _name As String
        Private _role As String
        Private _thumb As String

        <XmlElement("name")> _
        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal Value As String)
                Me._name = Value
            End Set
        End Property

        <XmlElement("role")> _
        Public Property Role() As String
            Get
                Return Me._role
            End Get
            Set(ByVal Value As String)
                Me._role = Value
            End Set
        End Property

        <XmlElement("thumb")> _
        Public Property Thumb() As String
            Get
                Return Me._thumb
            End Get
            Set(ByVal Value As String)
                Me._thumb = Value
            End Set
        End Property

        Public Sub New(ByVal sName As String)
            Me._name = sName
        End Sub

        Public Sub New(ByVal sName As String, ByVal sRole As String, ByVal sThumb As String)
            Me._name = sName
            Me._role = sRole
            Me._thumb = sThumb
        End Sub

        Public Sub New()
            Me.Clean()
        End Sub

        Public Sub Clean()
            Me._name = String.Empty
            Me._role = String.Empty
            Me._thumb = String.Empty
        End Sub
    End Class

    Public Class Fanart
        Private _url As String
        Private _thumb As New List(Of Thumb)

        <XmlAttribute("url")> _
        Public Property URL() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        <XmlElement("thumb")> _
        Public Property Thumb() As List(Of Thumb)
            Get
                Return Me._thumb
            End Get
            Set(ByVal value As List(Of Thumb))
                Me._thumb = value
            End Set
        End Property

        Public Sub New()
            Me.Clean()
        End Sub

        Public Sub Clean()
            Me._thumb.Clear()
            Me._url = String.Empty
        End Sub

    End Class

    Public Class Thumb
        Private _preview As String
        Private _text As String

        <XmlAttribute("preview")> _
        Public Property Preview() As String
            Get
                Return Me._preview
            End Get
            Set(ByVal Value As String)
                Me._preview = Value
            End Set
        End Property

        <XmlText()> _
        Public Property [Text]() As String
            Get
                Return Me._text
            End Get
            Set(ByVal Value As String)
                Me._text = Value
            End Set
        End Property
    End Class

    Public Class Poster
        Private _thumb As New List(Of Posters)

        <XmlElement("thumb")> _
        Public Property Thumb() As List(Of Posters)
            Get
                Return Me._thumb
            End Get
            Set(ByVal value As List(Of Posters))
                Me._thumb = value
            End Set
        End Property

        Public Sub New()
            Me.Clean()
        End Sub

        Public Sub Clean()
            Me._thumb.Clear()
        End Sub

    End Class

    Public Class Posters
        Private _url As String

        <XmlText()> _
        Public Property URL() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        Public Sub New()
            Me.Clean()
        End Sub

        Public Sub Clean()
            Me._url = String.Empty
        End Sub

    End Class

    Public Class [Image]
        Private _url As String
        Private _description As String
        Private _webimage As System.Drawing.Image

        Public Property URL() As String
            Get
                Return Me._url
            End Get
            Set(ByVal value As String)
                Me._url = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return Me._description
            End Get
            Set(ByVal value As String)
                Me._description = value
            End Set
        End Property

        Public Property WebImage() As System.Drawing.Image
            Get
                Return Me._webimage
            End Get
            Set(ByVal value As System.Drawing.Image)
                Me._webimage = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._url = String.Empty
            Me._description = String.Empty
            Me._webimage = Nothing
        End Sub

    End Class

    Public Class [Set]
        Private _set As New SetContainer

        <XmlElement("set")> _
        Public Property SetContainer() As SetContainer
            Get
                Return Me._set
            End Get
            Set(ByVal value As SetContainer)
                Me._set = value
            End Set
        End Property
    End Class

    Public Class SetContainer
        Private _set As String
        Private _order As String

        <XmlText()> _
        Public Property [Set]() As String
            Get
                Return _set
            End Get
            Set(ByVal value As String)
                _set = value
            End Set
        End Property

        <XmlAttribute("order")> _
        Public Property Order() As String
            Get
                Return _order
            End Get
            Set(ByVal value As String)
                _order = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property OrderSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._order)
            End Get
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            _set = String.Empty
            _order = String.Empty
        End Sub
    End Class
End Namespace