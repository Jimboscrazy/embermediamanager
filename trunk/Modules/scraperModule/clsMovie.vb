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



Imports System.Drawing.Imaging
Imports System.Xml.Serialization

Namespace EmberProxy
    <XmlRoot("movie")> _
    Public Class Movie : Implements IComparable(Of Movie)
        Private _imdbid As String
        Private _title As String
        Private _originaltitle As String
        Private _sorttitle As String
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
        Private _director As String
        Private _credits As String
        Private _playcount As String
        Private _watched As String
        Private _file As String
        Private _path As String
        Private _filenameandpath As String
        Private _status As String
        Private _thumb As New List(Of String)
        Private _fanart As New Fanart
        Private _actors As New List(Of Person)
        Private _fileInfo As New MediaInfo.Fileinfo
        Private _xsets As New List(Of [Set])
        Private _ysets As New SetContainer
        Private _lev As Integer

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
                Return Not String.IsNullOrEmpty(Me._imdbid) AndAlso Not Me._imdbid = "tt"
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

        <XmlElement("sorttitle")> _
        Public Property SortTitle() As String
            Get
                Return Me._sorttitle
            End Get
            Set(ByVal value As String)
                Me._sorttitle = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property SortTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._sorttitle)
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

        <XmlElement("thumb")> _
        Public Property Thumb() As List(Of String)
            Get
                Return Me._thumb
            End Get
            Set(ByVal value As List(Of String))
                Me._thumb = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ThumbSpecified() As Boolean
            Get
                Return Me._thumb.Count > 0
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
                If Not IsNothing(Me._fileInfo.StreamDetails.Video) OrElse _
                Me._fileInfo.StreamDetails.Audio.Count > 0 OrElse _
                 Me._fileInfo.StreamDetails.Subtitle.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        <XmlElement("sets")> _
        Public Property YSets() As SetContainer
            Get
                Return _ysets
            End Get
            Set(ByVal value As SetContainer)
                _ysets = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property YSetsSpecified() As Boolean
            Get
                Return _ysets.Sets.Count > 0
            End Get
        End Property

        <XmlElement("set")> _
        Public Property XSets() As List(Of [Set])
            Get
                Return Me._xsets
            End Get
            Set(ByVal value As List(Of [Set]))
                _xsets = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property XSetsSpecified() As Boolean
            Get
                Return Me._xsets.Count > 0
            End Get
        End Property

        <XmlIgnore()> _
        Public Property Sets() As List(Of [Set])
            Get
                Return Me._xsets
            End Get
            Set(ByVal value As List(Of [Set]))
                Me._xsets = value
            End Set
        End Property

        <XmlIgnore()> _
        Public Property Lev() As Integer
            Get
                Return Me._lev
            End Get
            Set(ByVal value As Integer)
                Me._lev = value
            End Set
        End Property

        Public Function CompareTo(ByVal other As Movie) As Integer Implements IComparable(Of Movie).CompareTo
            Dim retVal As Integer = (Me.Lev).CompareTo(other.Lev)
            If retVal = 0 Then
                retVal = (Me.Year).CompareTo(other.Year) * -1
            End If
            Return retVal
        End Function

        Public Sub AddSet(ByVal SetName As String, ByVal Order As Integer)
            Dim tSet = From bSet As [Set] In Sets Where bSet.Set = SetName

            If tSet.Count > 0 Then
                Sets.Remove(tSet(0))
            End If

            Sets.Add(New [Set] With {.Set = SetName, .Order = If(Order > 0, Order.ToString, String.Empty)})
        End Sub

        Public Sub RemoveSet(ByVal SetName As String)
            Dim tSet = From bSet As [Set] In Sets Where bSet.Set = SetName
            If tSet.Count > 0 Then
                Sets.Remove(tSet(0))
            End If
        End Sub

        Sub New()
            Me.Clear()
        End Sub

        Public Sub New(ByVal sID As String, ByVal sTitle As String, ByVal sYear As String, ByVal iLev As Integer)
            Me.Clear()
            Me._imdbid = sID
            Me._title = sTitle
            Me._year = sYear
            Me._lev = iLev
        End Sub

        Public Sub Clear()
            Me._imdbid = String.Empty
            Me._title = String.Empty
            Me._originaltitle = String.Empty
            Me._sorttitle = String.Empty
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
            Me._director = String.Empty
            Me._credits = String.Empty
            Me._playcount = String.Empty
            Me._watched = String.Empty
            Me._file = String.Empty
            Me._path = String.Empty
            Me._filenameandpath = String.Empty
            Me._status = String.Empty
            Me._thumb.Clear()
            Me._fanart = New Fanart
            Me._actors.Clear()
            Me._fileInfo = New MediaInfo.Fileinfo
            Me._ysets = New SetContainer
            Me._xsets.Clear()
            Me._lev = 0
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

        Public Overrides Function ToString() As String
            Return Me._name
        End Function
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
            Me.Clear()
        End Sub

        Public Sub Clear()
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

    Public Class [Image]
        Private _url As String
        Private _description As String
        Private _webimage As Images
        Private _ischecked As Boolean

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

        Public Property WebImage() As Images
            Get
                Return Me._webimage
            End Get
            Set(ByVal value As Images)
                Me._webimage = value
            End Set
        End Property

        Public Property isChecked() As Boolean
            Get
                Return Me._ischecked
            End Get
            Set(ByVal value As Boolean)
                Me._ischecked = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._url = String.Empty
            Me._description = String.Empty
            Me._webimage = New Images
            Me._ischecked = False
        End Sub

    End Class

    Public Class SetContainer
        Private _set As New List(Of [Set])

        <XmlElement("set")> _
        Public Property Sets() As List(Of [Set])
            Get
                Return _set
            End Get
            Set(ByVal value As List(Of [Set]))
                _set = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property SetsSpecified() As Boolean
            Get
                Return Me._set.Count > 0
            End Get
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._set = New List(Of [Set])
        End Sub
    End Class

    Public Class [Set]
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

        <XmlIgnore()> _
        Public ReadOnly Property SetSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._set)
            End Get
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

    <XmlRoot("tvshow")> _
    Public Class TVShow
        Private _title As String
        Private _id As String
        Private _rating As String
        Private _episodeguideurl As String
        Private _plot As String
        Private _mpaa As String
        Private _genre As String
        Private _premiered As String
        Private _studio As String
        Private _actors As New List(Of Person)

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

        <XmlElement("id")> _
        Public Property ID() As String
            Get
                Return Me._id
            End Get
            Set(ByVal value As String)
                Me._id = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property TVDBSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._id)
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

        <XmlElement("episodeguideurl")> _
        Public Property EpisodeGuideURL() As String
            Get
                Return Me._episodeguideurl
            End Get
            Set(ByVal value As String)
                Me._episodeguideurl = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property EpisodeGuideURLSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._episodeguideurl)
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

        <XmlElement("premiered")> _
        Public Property Premiered() As String
            Get
                Return Me._premiered
            End Get
            Set(ByVal value As String)
                Me._premiered = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property PremieredSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._premiered)
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

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            _title = String.Empty
            _id = String.Empty
            _rating = String.Empty
            _episodeguideurl = String.Empty
            _plot = String.Empty
            _mpaa = String.Empty
            _genre = String.Empty
            _premiered = String.Empty
            _studio = String.Empty
            _actors.Clear()
        End Sub
    End Class

    <XmlRoot("episodedetails")> _
    Public Class EpisodeDetails
        Private _title As String
        Private _season As Integer
        Private _episode As Integer
        Private _aired As String
        Private _rating As String
        Private _plot As String
        Private _director As String
        Private _credits As String
        Private _actors As New List(Of Person)
        Private _fileInfo As New MediaInfo.Fileinfo
        Private _posterurl As String
        Private _localfile As String

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

        <XmlElement("season")> _
        Public Property Season() As Integer
            Get
                Return Me._season
            End Get
            Set(ByVal value As Integer)
                Me._season = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property SeasonSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._season.ToString)
            End Get
        End Property

        <XmlElement("episode")> _
        Public Property Episode() As Integer
            Get
                Return Me._episode
            End Get
            Set(ByVal value As Integer)
                Me._episode = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property EpisodeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._episode.ToString)
            End Get
        End Property

        <XmlElement("aired")> _
        Public Property Aired() As String
            Get
                Return Me._aired
            End Get
            Set(ByVal value As String)
                Me._aired = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property AiredSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._aired)
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
                If Not IsNothing(Me._fileInfo.StreamDetails.Video) OrElse _
                Me._fileInfo.StreamDetails.Audio.Count > 0 OrElse _
                 Me._fileInfo.StreamDetails.Subtitle.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        <XmlIgnore()> _
        Public Property PosterURL() As String
            Get
                Return Me._posterurl
            End Get
            Set(ByVal value As String)
                Me._posterurl = value
            End Set
        End Property

        <XmlIgnore()> _
        Public Property LocalFile() As String
            Get
                Return Me._localfile
            End Get
            Set(ByVal value As String)
                Me._localfile = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._title = String.Empty
            Me._season = -1
            Me._episode = -1
            Me._aired = String.Empty
            Me._rating = String.Empty
            Me._plot = String.Empty
            Me._director = String.Empty
            Me._credits = String.Empty
            Me._actors.Clear()
            Me._fileInfo = New MediaInfo.Fileinfo
            Me._posterurl = String.Empty
            Me._localfile = String.Empty
        End Sub
    End Class

End Namespace

' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################

Public Class MediaInfo

    Public Enum StreamKind As UInteger
        General
        Visual
        Audio
        Text
    End Enum

    Public Enum InfoKind As UInteger
        Name
        Text
    End Enum
    <XmlRoot("fileinfo")> _
    Public Class Fileinfo

        Private _streamdetails As New StreamData

        <XmlElement("streamdetails")> _
        Property StreamDetails() As StreamData
            Get
                Return _streamdetails
            End Get
            Set(ByVal value As StreamData)
                _streamdetails = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property StreamDetailsSpecified() As Boolean
            Get
                Return (Not IsNothing(_streamdetails.Video) AndAlso _streamdetails.Video.Count > 0) OrElse _
                (Not IsNothing(_streamdetails.Audio) AndAlso _streamdetails.Audio.Count > 0) OrElse _
                (Not IsNothing(_streamdetails.Subtitle) AndAlso _streamdetails.Subtitle.Count > 0)
            End Get
        End Property

    End Class

    <XmlRoot("streamdata")> _
    Public Class StreamData

        Private _video As New List(Of Video)
        Private _audio As New List(Of Audio)
        Private _subtitle As New List(Of Subtitle)

        <XmlElement("video")> _
        Public Property Video() As List(Of Video)
            Get
                Return Me._video
            End Get
            Set(ByVal Value As List(Of Video))
                Me._video = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property VideoSpecified() As Boolean
            Get
                Return Me._video.Count > 0
            End Get
        End Property

        <XmlElement("audio")> _
         Public Property Audio() As List(Of Audio)
            Get
                Return Me._audio
            End Get
            Set(ByVal Value As List(Of Audio))
                Me._audio = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property AudioSpecified() As Boolean
            Get
                Return Me._audio.Count > 0
            End Get
        End Property

        <XmlElement("subtitle")> _
        Public Property Subtitle() As List(Of Subtitle)
            Get
                Return Me._subtitle
            End Get
            Set(ByVal Value As List(Of Subtitle))
                Me._subtitle = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property SubtitleSpecified() As Boolean
            Get
                Return Me._subtitle.Count > 0
            End Get
        End Property

    End Class

    Public Class Video

        Private _width As String = String.Empty
        Private _height As String = String.Empty
        Private _codec As String = String.Empty
        Private _duration As String = String.Empty
        Private _aspect As String = String.Empty
        Private _language As String = String.Empty
        Private _longlanguage As String = String.Empty
        Private _scantype As String = String.Empty

        <XmlElement("width")> _
        Public Property Width() As String
            Get
                Return Me._width
            End Get
            Set(ByVal Value As String)
                Me._width = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property WidthSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._width)
            End Get
        End Property

        <XmlElement("height")> _
        Public Property Height() As String
            Get
                Return Me._height
            End Get
            Set(ByVal Value As String)
                Me._height = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property HeightSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._height)
            End Get
        End Property

        <XmlElement("codec")> _
        Public Property Codec() As String
            Get
                Return Me._codec
            End Get
            Set(ByVal Value As String)
                Me._codec = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property CodecSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._codec)
            End Get
        End Property

        <XmlElement("duration")> _
        Public Property Duration() As String
            Get
                Return Me._duration
            End Get
            Set(ByVal Value As String)
                Me._duration = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property DurationSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._duration)
            End Get
        End Property

        <XmlElement("aspect")> _
        Public Property Aspect() As String
            Get
                Return Me._aspect
            End Get
            Set(ByVal Value As String)
                Me._aspect = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property AspectSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._aspect)
            End Get
        End Property

        <XmlElement("scantype")> _
        Public Property Scantype() As String
            Get
                Return Me._scantype
            End Get
            Set(ByVal Value As String)
                Me._scantype = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ScantypeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._scantype)
            End Get
        End Property

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._language)
            End Get
        End Property

        <XmlElement("longlanguage")> _
        Public Property LongLanguage() As String
            Get
                Return Me._longlanguage
            End Get
            Set(ByVal value As String)
                Me._longlanguage = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LongLanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._longlanguage)
            End Get
        End Property

    End Class

    Public Class Audio

        Private _codec As String = String.Empty
        Private _channels As String = String.Empty
        Private _language As String = String.Empty
        Private _longlanguage As String = String.Empty
        Private _haspreferred As Boolean = False

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._language)
            End Get
        End Property

        <XmlElement("longlanguage")> _
        Public Property LongLanguage() As String
            Get
                Return Me._longlanguage
            End Get
            Set(ByVal value As String)
                Me._longlanguage = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LongLanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._longlanguage)
            End Get
        End Property

        <XmlElement("codec")> _
        Public Property Codec() As String
            Get
                Return Me._codec
            End Get
            Set(ByVal Value As String)
                Me._codec = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property CodecSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._codec)
            End Get
        End Property

        <XmlElement("channels")> _
        Public Property Channels() As String
            Get
                Return Me._channels
            End Get
            Set(ByVal Value As String)
                Me._channels = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ChannelsSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._channels)
            End Get
        End Property

        <XmlIgnore()> _
        Public Property HasPreferred() As Boolean
            Get
                Return Me._haspreferred
            End Get
            Set(ByVal value As Boolean)
                Me._haspreferred = value
            End Set
        End Property

    End Class

    Public Class Subtitle

        Private _language As String = String.Empty
        Private _longlanguage As String = String.Empty

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._language)
            End Get
        End Property

        <XmlElement("longlanguage")> _
        Public Property LongLanguage() As String
            Get
                Return Me._longlanguage
            End Get
            Set(ByVal value As String)
                Me._longlanguage = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LongLanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._longlanguage)
            End Get
        End Property

    End Class

End Class


Public Class Images
    Private _image As Image
    Private _isedit As Boolean
    Public Property [Image]() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Image)
            _image = value
        End Set
    End Property

    Public Property IsEdit() As Boolean
        Get
            Return _isedit
        End Get
        Set(ByVal value As Boolean)
            _isedit = value
        End Set
    End Property

    Public Sub New()
        Clear()
    End Sub

    Public Sub Clear()
        _isedit = False
        _image = Nothing
    End Sub
End Class
