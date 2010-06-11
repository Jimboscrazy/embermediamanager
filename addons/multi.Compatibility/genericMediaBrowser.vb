﻿Imports System.Xml.Serialization
Imports System.IO

Public Class genericMediaBrowser
    Implements Interfaces.EmberExternalModule


    Private fMediaBrowser As frmMediaBrowser
    Private _enabled As Boolean = False
    Private _name As String = "MediaBrowser compatibility"

    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberExternalModule.Enabled
        Get
            Return _enabled
        End Get
        Set(ByVal value As Boolean)
            If _enabled = value Then Return
            _enabled = value
            If _enabled Then
                'Enable()
            Else
                'Disable()
            End If
        End Set
    End Property

    Public Event GenericEvent(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object)) Implements EmberAPI.Interfaces.EmberExternalModule.GenericEvent

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberExternalModule.Init

    End Sub

    Public Function InjectSetup() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberExternalModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me.fMediaBrowser = New frmMediaBrowser
        Me.fMediaBrowser.chkEnabled.Checked = Me._enabled

        SPanel.Name = _name
        SPanel.Text = Master.eLang.GetString(91, "MediaBrowser Compatibility")
        SPanel.Prefix = "MediaBrowser_"
        SPanel.Type = Master.eLang.GetString(802, "Modules", True)
        SPanel.ImageIndex = If(Me._enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me.fMediaBrowser.pnlSettings
        AddHandler Me.fMediaBrowser.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        AddHandler fMediaBrowser.ModuleEnabledChanged, AddressOf Handle_SetupChanged

        Return SPanel
        'Return Nothing
    End Function
    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub
    Private Sub Handle_SetupChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent ModuleSetupChanged(Me._name, state, difforder)
    End Sub
    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleName
        Get
            Return "MediaBrowser Compatibility"
        End Get
    End Property

    Public Event ModuleSettingsChanged() Implements EmberAPI.Interfaces.EmberExternalModule.ModuleSettingsChanged

    Public Event ModuleSetupChanged(ByVal Name As String, ByVal State As Boolean, ByVal diffOrder As Integer) Implements EmberAPI.Interfaces.EmberExternalModule.ModuleSetupChanged

    Public ReadOnly Property ModuleType() As System.Collections.Generic.List(Of EmberAPI.Enums.ModuleEventType) Implements EmberAPI.Interfaces.EmberExternalModule.ModuleType
        Get
            Return New List(Of Enums.ModuleEventType)(New Enums.ModuleEventType() {Enums.ModuleEventType.Generic, Enums.ModuleEventType.TVImageNaming, Enums.ModuleEventType.OnMovieNFOSave})
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberExternalModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberExternalModule.SaveSetup
        Me.Enabled = Me.fMediaBrowser.chkEnabled.Checked
    End Sub

    Public Function RunGeneric(ByVal mType As EmberAPI.Enums.ModuleEventType, ByRef _params As System.Collections.Generic.List(Of Object), ByRef _refparam As Object) As EmberAPI.Interfaces.ModuleResult Implements EmberAPI.Interfaces.EmberExternalModule.RunGeneric
        Dim doContinue As Boolean
        Dim mMovie As Structures.DBMovie
        If Enabled Then
            Try
                Select Case mType
                    Case Enums.ModuleEventType.OnMovieNFOSave
                        mMovie = DirectCast(_params(0), Structures.DBMovie)
                        doContinue = DirectCast(_refparam, Boolean)
                        XMLmymovies.SaveMovieDB(mMovie)
                End Select
                _refparam = doContinue
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Function

    <XmlRoot("Title")> _
    Public Class XMLmymovies
        Private _ID As String
        Private _CollectionNumber As String
        Private _Type As String
        Private _LocalTitle As String
        Private _OriginalTitle As String
        Private _SortTitle As String
        Private _ForcedTitle As String
        Private _IMDBrating As String
        Private _ProductionYear As String
        'Private _Revenue As String
        'Private _Budget As String
        Private _Added As String
        Private _IMDbId As String
        Private _RunningTime As String
        Private _TMDbId As String
        Private _Studios As New List(Of Studio)
        Private _CDUniverseId As String
        Private _Persons As New List(Of Person)
        Private _Genres As New List(Of Genre)
        Private _Description As String
        Private _AudioTracks As New List(Of AudioTrack)
        Private _Subtitles As New List(Of Subtitle)

        <XmlElement("ID")> _
        Public Property ID() As String
            Get
                Return Me._ID
            End Get
            Set(ByVal value As String)
                Me._ID = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property IDSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._ID)
            End Get
        End Property

        <XmlElement("CollectionNumber")> _
         Public Property CollectionNumber() As String
            Get
                Return Me._CollectionNumber
            End Get
            Set(ByVal value As String)
                Me._CollectionNumber = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property CollectionNumberSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._CollectionNumber)
            End Get
        End Property

        <XmlElement("Type")> _
        Public Property Type() As String
            Get
                Return Me._Type
            End Get
            Set(ByVal value As String)
                Me._Type = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property TypeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._Type)
            End Get
        End Property

        <XmlElement("LocalTitle")> _
        Public Property LocalTitle() As String
            Get
                Return Me._LocalTitle
            End Get
            Set(ByVal value As String)
                Me._LocalTitle = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property LocalTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._LocalTitle)
            End Get
        End Property

        <XmlElement("OriginalTitle")> _
        Public Property OriginalTitle() As String
            Get
                Return Me._OriginalTitle
            End Get
            Set(ByVal value As String)
                Me._OriginalTitle = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property OriginalTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._OriginalTitle)
            End Get
        End Property

        <XmlElement("SortTitle")> _
        Public Property SortTitle() As String
            Get
                Return Me._SortTitle
            End Get
            Set(ByVal value As String)
                Me._SortTitle = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property SortTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._SortTitle)
            End Get
        End Property

        <XmlElement("ForcedTitle")> _
        Public Property ForcedTitle() As String
            Get
                Return Me._ForcedTitle
            End Get
            Set(ByVal value As String)
                Me._ForcedTitle = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property ForcedTitleSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._ForcedTitle)
            End Get
        End Property

        <XmlElement("IMDBrating")> _
        Public Property IMDBrating() As String
            Get
                Return Me._IMDBrating
            End Get
            Set(ByVal value As String)
                Me._IMDBrating = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property IMDBratingSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._IMDBrating)
            End Get
        End Property

        <XmlElement("ProductionYear")> _
        Public Property ProductionYear() As String
            Get
                Return Me._ProductionYear
            End Get
            Set(ByVal value As String)
                Me._ProductionYear = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property ProductionYearSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._ProductionYear)
            End Get
        End Property

        <XmlElement("Added")> _
        Public Property Added() As String
            Get
                Return Me._Added
            End Get
            Set(ByVal value As String)
                Me._Added = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property AddedSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._Added)
            End Get
        End Property

        <XmlElement("IMDB")> _
        Public Property IMDB() As String
            Get
                Return Me._IMDbId
            End Get
            Set(ByVal value As String)
                Me._IMDbId = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property IMDBSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._IMDbId)
            End Get
        End Property

        <XmlElement("IMDbId")> _
        Public Property IMDbId() As String
            Get
                Return Me._IMDbId
            End Get
            Set(ByVal value As String)
                Me._IMDbId = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property _IMDbIdSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._IMDbId)
            End Get
        End Property

        <XmlElement("RunningTime")> _
        Public Property RunningTime() As String
            Get
                Return Me._RunningTime
            End Get
            Set(ByVal value As String)
                Me._RunningTime = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property RunningTimeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._RunningTime)
            End Get
        End Property

        <XmlElement("TMDbId")> _
        Public Property TMDbId() As String
            Get
                Return Me._TMDbId
            End Get
            Set(ByVal value As String)
                Me._TMDbId = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property TMDbIdSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._TMDbId)
            End Get
        End Property

        <XmlArray("Studios")> _
        <XmlArrayItem("Studio")> _
        Public Property Studios() As List(Of Studio)
            Get
                Return Me._Studios
            End Get
            Set(ByVal value As List(Of Studio))
                Me._Studios = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property StudiosSpecified() As Boolean
            Get
                Return (Me._Studios.Count > 0)
            End Get
        End Property

        <XmlElement("CDUniverseId")> _
        Public Property CDUniverseId() As String
            Get
                Return Me._CDUniverseId
            End Get
            Set(ByVal value As String)
                Me._CDUniverseId = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property CDUniverseIdSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._CDUniverseId)
            End Get
        End Property

        <XmlArray("Persons")> _
        <XmlArrayItem("Person")> _
        Public Property Persons() As List(Of Person)
            Get
                Return Me._Persons
            End Get
            Set(ByVal value As List(Of Person))
                Me._Persons = value
            End Set
        End Property
        <XmlAttribute("ActorsComplete")> _
        Public ActorsComplete As String
        <XmlIgnore()> _
        Public ReadOnly Property PersonsSpecified() As Boolean
            Get
                Return (Me._Persons.Count > 0)
            End Get
        End Property

        <XmlArray("Genres")> _
        <XmlArrayItem("Genre")> _
        Public Property Genres() As List(Of Genre)
            Get
                Return Me._Genres
            End Get
            Set(ByVal value As List(Of Genre))
                Me._Genres = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property GenresSpecified() As Boolean
            Get
                Return (Me._Genres.Count > 0)
            End Get
        End Property

        <XmlElement("Description")> _
        Public Property Description() As String
            Get
                Return Me._Description
            End Get
            Set(ByVal value As String)
                Me._Description = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property DescriptionSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._Description)
            End Get
        End Property

        <XmlArray("AudioTracks")> _
        <XmlArrayItem("AudioTrack")> _
        Public Property AudioTracks() As List(Of AudioTrack)
            Get
                Return Me._AudioTracks
            End Get
            Set(ByVal value As List(Of AudioTrack))
                Me._AudioTracks = value
            End Set
        End Property
        <XmlIgnore()> _
        Public ReadOnly Property AudioTracksSpecified() As Boolean
            Get
                Return (Me._AudioTracks.Count > 0)
            End Get
        End Property

        <XmlArray("Subtitles")> _
        <XmlArrayItem("Subtitle")> _
        Public Property Subtitles() As List(Of Subtitle)
            Get
                Return Me._Subtitles
            End Get
            Set(ByVal value As List(Of Subtitle))
                Me._Subtitles = value
            End Set

        End Property
        <XmlAttribute("NotPresent")> _
        Public SubsNotPresent As String
        <XmlIgnore()> _
        Public ReadOnly Property SubtitlesSpecified() As Boolean
            Get
                Return (Me._Subtitles.Count > 0)
            End Get
        End Property

        Public Class Studio
            <XmlText()> _
            Public Studio As String
        End Class
        Public Class Person
            '<XmlAttribute("Type")> _
            'Public _type As String
            Public Name As String
            Public Type As String
            Public Role As String
        End Class
        Public Class Genre
            <XmlText()> _
            Public Genre As String
        End Class
        Public Class AudioTrack
            <XmlAttribute("Language")> _
            Public Language As String
            <XmlAttribute("Type")> _
            Public Type As String
            <XmlAttribute("Channels")> _
            Public Channels As String
        End Class
        Public Class Subtitle
            <XmlAttribute("Language")> _
            Public Language As String
        End Class

        Public Shared Function GetFromMovieDB(ByVal movie As Structures.DBMovie) As XMLmymovies
            Dim myself As New XMLmymovies
            myself.ID = movie.ID.ToString
            myself.LocalTitle = movie.Movie.Title
            myself.OriginalTitle = movie.Movie.OriginalTitle
            myself.SortTitle = movie.Movie.SortTitle
            myself.IMDbId = movie.Movie.ID
            myself.RunningTime = movie.Movie.Runtime
            myself.Description = movie.Movie.Plot
            For Each g As String In movie.Movie.Genre.Split(Convert.ToChar("/"))
                myself.Genres.Add(New Genre With {.Genre = g.Trim})
            Next
            For Each s As String In movie.Movie.Studio.Split(Convert.ToChar("/"))
                myself.Studios.Add(New Studio With {.Studio = s.Trim})
            Next
            For Each p As MediaContainers.Person In movie.Movie.Actors
                myself.Persons.Add(New Person With {.Name = p.Name, .Role = p.Role, .Type = "Actor"})
            Next
            Return myself
        End Function

        Public Shared Sub SaveMovieDB(ByVal movie As Structures.DBMovie, Optional ByVal tpath As String = "")
            Dim myself As XMLmymovies = GetFromMovieDB(movie)
            Dim xmlSer As New XmlSerializer(GetType(XMLmymovies))
            If tpath = String.Empty Then
                tpath = Path.Combine(Path.GetDirectoryName(movie.NfoPath), "mymovies.xml")
            End If
            Using xmlSW As New StreamWriter(tpath)
                xmlSer.Serialize(xmlSW, myself)
            End Using
        End Sub

    End Class
End Class
