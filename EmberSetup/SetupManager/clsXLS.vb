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
Imports System.Xml
Imports System.Xml.Serialization

Public Class DefaultStrings

    #Region "Fields"

    Public Shared Tables As String() = {"CREATE TABLE IF NOT EXISTS Movies(" &                                 "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &                                 "MoviePath TEXT NOT NULL, " &                                 "Type BOOL NOT NULL DEFAULT False , " &                                 "ListTitle TEXT NOT NULL, " &                                 "HasPoster BOOL NOT NULL DEFAULT False, " &                                 "HasFanart BOOL NOT NULL DEFAULT False, " &                                 "HasNfo BOOL NOT NULL DEFAULT False, " &                                 "HasTrailer BOOL NOT NULL DEFAULT False, " &                                 "HasSub BOOL NOT NULL DEFAULT False, " &                                 "HasExtra BOOL NOT NULL DEFAULT False, " &                                 "New BOOL DEFAULT False, " &                                 "Mark BOOL NOT NULL DEFAULT False, " &                                 "Source TEXT NOT NULL, " &                                 "Imdb TEXT, " &                                 "Lock BOOL NOT NULL DEFAULT False, " &                                 "Title TEXT, " &                                 "OriginalTitle TEXT, " &                                 "Year TEXT, " &                                 "Rating TEXT, " &                                 "Votes TEXT, " &                                 "MPAA TEXT, " &                                 "Top250 TEXT, " &                                 "Outline TEXT, " &                                 "Plot TEXT, " &                                 "Tagline TEXT, " &                                 "Certification TEXT, " &                                 "Genre TEXT, " &                                 "Studio TEXT, " &                                 "Runtime TEXT, " &                                 "ReleaseDate TEXT, " &                                 "Director TEXT, " &                                 "Credits TEXT, " &                                 "Playcount TEXT, " &                                 "Watched TEXT, " &                                 "File TEXT, " &                                 "Path TEXT, " &                                 "FileNameAndPath TEXT, " &                                 "Status TEXT, " &                                 "Trailer TEXT, " &                                 "PosterPath TEXT, " &                                 "FanartPath TEXT, " &                                 "ExtraPath TEXT, " &                                 "NfoPath TEXT, " &                                 "TrailerPath TEXT, " &                                 "SubPath TEXT, " &                                 "FanartURL TEXT, " &                                 "UseFolder BOOL NOT NULL DEFAULT False, " &                                 "OutOfTolerance BOOL NOT NULL DEFAULT False, " &                                 "FileSource TEXT, " &                                 "NeedsSave BOOL NOT NULL DEFAULT False," &                                 "SortTitle TEXT" &                                 ");",                                 "CREATE UNIQUE INDEX IF NOT EXISTS UniquePath ON Movies (MoviePath);",                                 "CREATE TABLE IF NOT EXISTS Sets(" &                                 "SetName TEXT NOT NULL PRIMARY KEY" &                                  ");",                                 "CREATE TABLE IF NOT EXISTS MoviesSets(" &                                 "MovieID INTEGER NOT NULL, " &                                 "SetName TEXT NOT NULL, " &                                 "SetOrder TEXT NOT NULL, " &                                 "PRIMARY KEY (MovieID,SetName) " &                                  ");",                                 "CREATE TABLE IF NOT EXISTS MoviesVStreams(" &                                 "MovieID INTEGER NOT NULL, " &                                 "StreamID INTEGER NOT NULL, " &                                 "Video_Width TEXT, " &                                 "Video_Height TEXT," &                                 "Video_Codec TEXT, " &                                 "Video_Duration TEXT, " &                                 "Video_ScanType TEXT, " &                                 "Video_AspectDisplayRatio TEXT, " &                                 "Video_Language TEXT, " &                                 "Video_LongLanguage TEXT, " &                                 "PRIMARY KEY (MovieID,StreamID) " &                                 ");",                                 "CREATE TABLE IF NOT EXISTS MoviesAStreams(" &                                 "MovieID INTEGER NOT NULL, " &                                 "StreamID INTEGER NOT NULL, " &                                 "Audio_Language TEXT, " &                                 "Audio_LongLanguage TEXT, " &                                 "Audio_Codec TEXT, " &                                 "Audio_Channel TEXT, " &                                 "PRIMARY KEY (MovieID,StreamID) " &                                 ");",                                 "CREATE TABLE IF NOT EXISTS MoviesSubs(" &                                 "MovieID INTEGER NOT NULL, " &                                 "StreamID INTEGER NOT NULL, " &                                 "Subs_Language TEXT, " &                                 "Subs_LongLanguage TEXT, " &                                 "PRIMARY KEY (MovieID,StreamID) " &                                  ");",                                 "CREATE TABLE IF NOT EXISTS MoviesPosters(" &                                 "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &                                 "MovieID INTEGER NOT NULL, " &                                 "thumbs TEXT" &                                 ");",                                 "CREATE TABLE IF NOT EXISTS MoviesFanart(" &                                 "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &                                 "MovieID INTEGER NOT NULL, " &                                 "preview TEXT, " &                                 "thumbs TEXT" &                                 ");",                                 "CREATE TABLE IF NOT EXISTS Actors(" &                                 "Name TEXT PRIMARY KEY, " &                                 "thumb TEXT" &                                 ");",                                 "CREATE TABLE IF NOT EXISTS MoviesActors(" &                                 "MovieID INTEGER NOT NULL, " &                                 "ActorName TEXT NOT NULL, " &                                 "Role TEXT, " &                                 "PRIMARY KEY (MovieID,ActorName) " &                                 ");",                                 "CREATE TABLE IF NOT EXISTS Sources(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, path TEXT NOT NULL, Recursive BOOL NOT NULL DEFAULT False , Foldername BOOL NOT NULL DEFAULT False, Single BOOL NOT NULL DEFAULT False, LastScan TEXT NOT NULL DEFAULT '1900/01/01');",                                 "CREATE UNIQUE INDEX IF NOT EXISTS UniqueSource ON Sources (Path);",                                 "CREATE TABLE IF NOT EXISTS TVShows(" &                                     "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &                                     "Title TEXT, " &                                     "HasPoster BOOL NOT NULL DEFAULT False, " &                                     "HasFanart BOOL NOT NULL DEFAULT False, " &                                     "HasNfo BOOL NOT NULL DEFAULT False, " &                                     "New BOOL DEFAULT False, " &                                     "Mark BOOL NOT NULL DEFAULT False, " &                                     "TVShowPath TEXT NOT NULL, " &                                     "Source TEXT NOT NULL, " &                                     "TVDB TEXT, " &                                     "Lock BOOL NOT NULL DEFAULT False, " &                                     "EpisodeGuide TEXT, " &                                     "Plot TEXT, " &                                     "Genre TEXT, " &                                     "Premiered TEXT, " &                                     "Studio TEXT, " &                                     "MPAA TEXT, " &                                     "Rating TEXT, " &                                     "PosterPath TEXT, " &                                     "FanartPath TEXT, " &                                     "NfoPath TEXT, " &                                     "NeedsSave BOOL NOT NULL DEFAULT False" &                                     ");",                                 "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVShowPath ON TVShows (TVShowPath);",                                 "CREATE TABLE IF NOT EXISTS TVEpPaths(" &                                         "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &                                         "TVEpPath TEXT NOT NULL" &                                         ");",                                 "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVEpPath ON TVEpPaths (TVEpPath);",                                 "CREATE TABLE IF NOT EXISTS TVEps(" &                                         "ID INTEGER PRIMARY KEY AUTOINCREMENT, " &                                         "TVShowID INTEGER NOT NULL, " &                                         "Title TEXT, " &                                         "HasPoster BOOL NOT NULL DEFAULT False, " &                                         "HasFanart BOOL NOT NULL DEFAULT False, " &                                         "HasNfo BOOL NOT NULL DEFAULT False, " &                                         "New BOOL DEFAULT False, " &                                         "Mark BOOL NOT NULL DEFAULT False, " &                                         "TVEpPathID INTEGER NOT NULL, " &                                         "Source TEXT NOT NULL, " &                                         "Lock BOOL NOT NULL DEFAULT False, " &                                         "Season INTEGER, " &                                         "Episode INTEGER, " &                                         "Rating TEXT, " &                                         "Plot TEXT, " &                                         "Aired TEXT, " &                                         "Director TEXT, " &                                         "Credits TEXT, " &                                         "PosterPath TEXT, " &                                         "FanartPath TEXT, " &                                         "NfoPath TEXT, " &                                         "NeedsSave BOOL NOT NULL DEFAULT False" &                                         ");",                                 "CREATE TABLE IF NOT EXISTS TVShowActors(" &                                     "TVShowID INTEGER NOT NULL, " &                                     "ActorName TEXT NOT NULL, " &                                     "Role TEXT, " &                                     "PRIMARY KEY (TVShowID,ActorName) " &                                     ");",                                 "CREATE TABLE IF NOT EXISTS TVEpActors(" &                                     "TVEpID INTEGER NOT NULL, " &                                     "ActorName TEXT NOT NULL, " &                                     "Role TEXT, " &                                     "PRIMARY KEY (TVEpID,ActorName) " &                                     ");",                                     "CREATE TABLE IF NOT EXISTS TVSeason(" &                                     "TVShowID INTEGER NOT NULL, " &                                     "TVEpID INTEGER NOT NULL, " &                                     "SeasonText TEXT, " &                                     "Season INTEGER NOT NULL, " &                                     "HasPoster BOOL NOT NULL DEFAULT False, " &                                     "HasFanart BOOL NOT NULL DEFAULT False, " &                                     "PosterPath TEXT, " &                                     "FanartPath TEXT, " &                                     "PRIMARY KEY (TVShowID,TVEpID)" &                                     ");",                                 "CREATE TABLE IF NOT EXISTS TVVStreams(" &                                     "TVEpID INTEGER NOT NULL, " &                                     "StreamID INTEGER NOT NULL, " &                                     "Video_Width TEXT, " &                                     "Video_Height TEXT," &                                     "Video_Codec TEXT, " &                                     "Video_Duration TEXT, " &                                     "Video_ScanType TEXT, " &                                     "Video_AspectDisplayRatio TEXT, " &                                     "Video_Language TEXT, " &                                     "Video_LongLanguage TEXT, " &                                     "PRIMARY KEY (TVEpID,StreamID) " &                                     ");",                                 "CREATE TABLE IF NOT EXISTS TVAStreams(" &                                     "TVEpID INTEGER NOT NULL, " &                                     "StreamID INTEGER NOT NULL, " &                                     "Audio_Language TEXT, " &                                     "Audio_LongLanguage TEXT, " &                                     "Audio_Codec TEXT, " &                                     "Audio_Channel TEXT, " &                                     "PRIMARY KEY (TVEpID,StreamID) " &                                     ");",                                 "CREATE TABLE IF NOT EXISTS TVSubs(" &                                     "TVEpID INTEGER NOT NULL, " &                                     "StreamID INTEGER NOT NULL, " &                                     "Subs_Language TEXT, " &                                     "Subs_LongLanguage TEXT, " &                                     "PRIMARY KEY (TVEpID,StreamID) " &                                      ");",                                  "CREATE TABLE IF NOT EXISTS TVSources(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, path TEXT NOT NULL, LastScan TEXT NOT NULL DEFAULT '1900/01/01');",                                  "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVSource ON TVSources (Path);"                                     }

    #End Region 'Fields

End Class

Public Class FileOfList

    #Region "Fields"

    Public Filename As String
    Public Hash As String
    Public Path As String
    Public Platform As String

    #End Region 'Fields

End Class

<XmlRoot("UpgradeFile")> _
Public Class FilesList

    #Region "Fields"

    <XmlArray("Files")> _
    <XmlArrayItem("File")> _
    Public Files As List(Of FileOfList)

    #End Region 'Fields

    #Region "Methods"

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(FilesList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

Public Class FileToInstall

    #Region "Fields"

    Public EmberPath As String
    Public Filename As String
    Public Hash As String
    Public OriginalPath As String
    Public Platform As String

    #End Region 'Fields

End Class

Public Class InstallCommand

    #Region "Fields"

    <XmlElement("Description")> _
    Public CommandDescription As String
    <XmlElement("Execute")> _
    Public CommandExecute As String
    <XmlAttribute("Type")> _
    Public CommandType As String

    #End Region 'Fields

End Class

<XmlRoot("CommandFile")> _
Public Class InstallCommands

    #Region "Fields"

    <XmlArray("Commands")> _
    <XmlArrayItem("Command")> _
    Public Command As List(Of InstallCommand)

    #End Region 'Fields

    #Region "Methods"

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(InstallCommands))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

Public Class Settings

    #Region "Fields"

    Public FTPHost As String
    Public FTPPassword As String
    Public FTPUser As String

    #End Region 'Fields

    #Region "Methods"

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(Settings))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

<XmlRoot("VersionsFile")> _
Public Class UpgradeList

    #Region "Fields"

    <XmlArray("Versions")> _
    <XmlArrayItem("Version")> _
    Public VersionList As New List(Of Versions)

    #End Region 'Fields

    #Region "Methods"

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(UpgradeList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

Public Class Versions
    Implements IComparable(Of Versions)

    #Region "Fields"

    <XmlAttribute("Number")> _
    Public Version As String

    #End Region 'Fields

    #Region "Methods"

    Public Function CompareTo(ByVal other As Versions) As Integer Implements IComparable(Of Versions).CompareTo
        Return (Me.Version).CompareTo(other.Version)
    End Function

    #End Region 'Methods

End Class

<XmlRoot("Config")> _
Public Class _LastVersion

    #Region "Fields"

    <XmlArrayItem("File")> _
    Public Modules As New List(Of _Module)

    #End Region 'Fields

    #Region "Methods"

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(_LastVersion))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub

    #End Region 'Methods

End Class

Public Class _Module
    Implements IComparable(Of Versions)

    #Region "Fields"

    Public Name As String
    Public Platform As String
    Public Version As String

    #End Region 'Fields

    #Region "Methods"

    Public Function CompareTo(ByVal other As Versions) As Integer Implements IComparable(Of Versions).CompareTo
        Return (Me.Version).CompareTo(other.Version)
    End Function

    #End Region 'Methods

End Class