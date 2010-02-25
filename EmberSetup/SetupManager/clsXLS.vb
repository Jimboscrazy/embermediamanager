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


Public Class Settings
    Public FTPUser As String
    Public FTPPassword As String
    Public FTPHost As String
    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(Settings))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub
End Class

<XmlRoot("VersionsFile")> _
Public Class UpgradeList
    <XmlArray("Versions")> _
    <XmlArrayItem("Version")> _
    Public VersionList As New List(Of Versions)

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(UpgradeList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub
End Class

Public Class Versions : Implements IComparable(Of Versions)
    <XmlAttribute("Number")> _
    Public Version As String
    Public Function CompareTo(ByVal other As Versions) As Integer Implements IComparable(Of Versions).CompareTo
        Return (Me.Version).CompareTo(other.Version)
    End Function
End Class

Public Class FileToInstall
    Public Filename As String
    Public OriginalPath As String
    Public EmberPath As String
    Public Hash As String
    Public Platform As String
End Class

<XmlRoot("UpgradeFile")> _
Public Class FilesList
    <XmlArray("Files")> _
    <XmlArrayItem("File")> _
    Public Files As List(Of FileOfList)
    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(FilesList))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub
End Class

Public Class FileOfList
    Public Path As String
    Public Filename As String
    Public Platform As String
    Public Hash As String
End Class

<XmlRoot("CommandFile")> _
Public Class InstallCommands
    <XmlArray("Commands")> _
    <XmlArrayItem("Command")> _
    Public Command As List(Of InstallCommand)

    Public Sub Save(ByVal fpath As String)
        Dim xmlSer As New XmlSerializer(GetType(InstallCommands))
        Using xmlSW As New StreamWriter(fpath)
            xmlSer.Serialize(xmlSW, Me)
        End Using
    End Sub
End Class

Public Class InstallCommand
    <XmlElement("Description")> _
    Public CommandDescription As String
    <XmlAttribute("Type")> _
    Public CommandType As String
    <XmlElement("Execute")> _
    Public CommandExecute As String
End Class

Public Class DefaultStrings
    Public Shared Tables As String() = {"CREATE TABLE IF NOT EXISTS Movies(" & _
                                "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                "MoviePath TEXT NOT NULL, " & _
                                "Type BOOL NOT NULL DEFAULT False , " & _
                                "ListTitle TEXT NOT NULL, " & _
                                "HasPoster BOOL NOT NULL DEFAULT False, " & _
                                "HasFanart BOOL NOT NULL DEFAULT False, " & _
                                "HasNfo BOOL NOT NULL DEFAULT False, " & _
                                "HasTrailer BOOL NOT NULL DEFAULT False, " & _
                                "HasSub BOOL NOT NULL DEFAULT False, " & _
                                "HasExtra BOOL NOT NULL DEFAULT False, " & _
                                "New BOOL DEFAULT False, " & _
                                "Mark BOOL NOT NULL DEFAULT False, " & _
                                "Source TEXT NOT NULL, " & _
                                "Imdb TEXT, " & _
                                "Lock BOOL NOT NULL DEFAULT False, " & _
                                "Title TEXT, " & _
                                "OriginalTitle TEXT, " & _
                                "Year TEXT, " & _
                                "Rating TEXT, " & _
                                "Votes TEXT, " & _
                                "MPAA TEXT, " & _
                                "Top250 TEXT, " & _
                                "Outline TEXT, " & _
                                "Plot TEXT, " & _
                                "Tagline TEXT, " & _
                                "Certification TEXT, " & _
                                "Genre TEXT, " & _
                                "Studio TEXT, " & _
                                "Runtime TEXT, " & _
                                "ReleaseDate TEXT, " & _
                                "Director TEXT, " & _
                                "Credits TEXT, " & _
                                "Playcount TEXT, " & _
                                "Watched TEXT, " & _
                                "File TEXT, " & _
                                "Path TEXT, " & _
                                "FileNameAndPath TEXT, " & _
                                "Status TEXT, " & _
                                "Trailer TEXT, " & _
                                "PosterPath TEXT, " & _
                                "FanartPath TEXT, " & _
                                "ExtraPath TEXT, " & _
                                "NfoPath TEXT, " & _
                                "TrailerPath TEXT, " & _
                                "SubPath TEXT, " & _
                                "FanartURL TEXT, " & _
                                "UseFolder BOOL NOT NULL DEFAULT False, " & _
                                "OutOfTolerance BOOL NOT NULL DEFAULT False, " & _
                                "FileSource TEXT, " & _
                                "NeedsSave BOOL NOT NULL DEFAULT False," & _
                                "SortTitle TEXT" & _
                                ");", _
                                "CREATE UNIQUE INDEX IF NOT EXISTS UniquePath ON Movies (MoviePath);", _
                                "CREATE TABLE IF NOT EXISTS Sets(" & _
                                "SetName TEXT NOT NULL PRIMARY KEY" & _
                                 ");", _
                                "CREATE TABLE IF NOT EXISTS MoviesSets(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "SetName TEXT NOT NULL, " & _
                                "SetOrder TEXT NOT NULL, " & _
                                "PRIMARY KEY (MovieID,SetName) " & _
                                 ");", _
                                "CREATE TABLE IF NOT EXISTS MoviesVStreams(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "StreamID INTEGER NOT NULL, " & _
                                "Video_Width TEXT, " & _
                                "Video_Height TEXT," & _
                                "Video_Codec TEXT, " & _
                                "Video_Duration TEXT, " & _
                                "Video_ScanType TEXT, " & _
                                "Video_AspectDisplayRatio TEXT, " & _
                                "Video_Language TEXT, " & _
                                "Video_LongLanguage TEXT, " & _
                                "PRIMARY KEY (MovieID,StreamID) " & _
                                ");", _
                                "CREATE TABLE IF NOT EXISTS MoviesAStreams(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "StreamID INTEGER NOT NULL, " & _
                                "Audio_Language TEXT, " & _
                                "Audio_LongLanguage TEXT, " & _
                                "Audio_Codec TEXT, " & _
                                "Audio_Channel TEXT, " & _
                                "PRIMARY KEY (MovieID,StreamID) " & _
                                ");", _
                                "CREATE TABLE IF NOT EXISTS MoviesSubs(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "StreamID INTEGER NOT NULL, " & _
                                "Subs_Language TEXT, " & _
                                "Subs_LongLanguage TEXT, " & _
                                "PRIMARY KEY (MovieID,StreamID) " & _
                                 ");", _
                                "CREATE TABLE IF NOT EXISTS MoviesPosters(" & _
                                "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                "MovieID INTEGER NOT NULL, " & _
                                "thumbs TEXT" & _
                                ");", _
                                "CREATE TABLE IF NOT EXISTS MoviesFanart(" & _
                                "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                "MovieID INTEGER NOT NULL, " & _
                                "preview TEXT, " & _
                                "thumbs TEXT" & _
                                ");", _
                                "CREATE TABLE IF NOT EXISTS Actors(" & _
                                "Name TEXT PRIMARY KEY, " & _
                                "thumb TEXT" & _
                                ");", _
                                "CREATE TABLE IF NOT EXISTS MoviesActors(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "ActorName TEXT NOT NULL, " & _
                                "Role TEXT, " & _
                                "PRIMARY KEY (MovieID,ActorName) " & _
                                ");", _
                                "CREATE TABLE IF NOT EXISTS Sources(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, path TEXT NOT NULL, Recursive BOOL NOT NULL DEFAULT False , Foldername BOOL NOT NULL DEFAULT False, Single BOOL NOT NULL DEFAULT False, LastScan TEXT NOT NULL DEFAULT '1900/01/01');", _
                                "CREATE UNIQUE INDEX IF NOT EXISTS UniqueSource ON Sources (Path);", _
                                "CREATE TABLE IF NOT EXISTS TVShows(" & _
                                    "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                    "Title TEXT, " & _
                                    "HasPoster BOOL NOT NULL DEFAULT False, " & _
                                    "HasFanart BOOL NOT NULL DEFAULT False, " & _
                                    "HasNfo BOOL NOT NULL DEFAULT False, " & _
                                    "New BOOL DEFAULT False, " & _
                                    "Mark BOOL NOT NULL DEFAULT False, " & _
                                    "TVShowPath TEXT NOT NULL, " & _
                                    "Source TEXT NOT NULL, " & _
                                    "TVDB TEXT, " & _
                                    "Lock BOOL NOT NULL DEFAULT False, " & _
                                    "EpisodeGuide TEXT, " & _
                                    "Plot TEXT, " & _
                                    "Genre TEXT, " & _
                                    "Premiered TEXT, " & _
                                    "Studio TEXT, " & _
                                    "MPAA TEXT, " & _
                                    "Rating TEXT, " & _
                                    "PosterPath TEXT, " & _
                                    "FanartPath TEXT, " & _
                                    "NfoPath TEXT, " & _
                                    "NeedsSave BOOL NOT NULL DEFAULT False" & _
                                    ");", _
                                "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVShowPath ON TVShows (TVShowPath);", _
                                "CREATE TABLE IF NOT EXISTS TVEpPaths(" & _
                                        "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                        "TVEpPath TEXT NOT NULL" & _
                                        ");", _
                                "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVEpPath ON TVEpPaths (TVEpPath);", _
                                "CREATE TABLE IF NOT EXISTS TVEps(" & _
                                        "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                        "TVShowID INTEGER NOT NULL, " & _
                                        "Title TEXT, " & _
                                        "HasPoster BOOL NOT NULL DEFAULT False, " & _
                                        "HasFanart BOOL NOT NULL DEFAULT False, " & _
                                        "HasNfo BOOL NOT NULL DEFAULT False, " & _
                                        "New BOOL DEFAULT False, " & _
                                        "Mark BOOL NOT NULL DEFAULT False, " & _
                                        "TVEpPathID INTEGER NOT NULL, " & _
                                        "Source TEXT NOT NULL, " & _
                                        "Lock BOOL NOT NULL DEFAULT False, " & _
                                        "Season INTEGER, " & _
                                        "Episode INTEGER, " & _
                                        "Rating TEXT, " & _
                                        "Plot TEXT, " & _
                                        "Aired TEXT, " & _
                                        "Director TEXT, " & _
                                        "Credits TEXT, " & _
                                        "PosterPath TEXT, " & _
                                        "FanartPath TEXT, " & _
                                        "NfoPath TEXT, " & _
                                        "NeedsSave BOOL NOT NULL DEFAULT False" & _
                                        ");", _
                                "CREATE TABLE IF NOT EXISTS TVShowActors(" & _
                                    "TVShowID INTEGER NOT NULL, " & _
                                    "ActorName TEXT NOT NULL, " & _
                                    "Role TEXT, " & _
                                    "PRIMARY KEY (TVShowID,ActorName) " & _
                                    ");", _
                                "CREATE TABLE IF NOT EXISTS TVEpActors(" & _
                                    "TVEpID INTEGER NOT NULL, " & _
                                    "ActorName TEXT NOT NULL, " & _
                                    "Role TEXT, " & _
                                    "PRIMARY KEY (TVEpID,ActorName) " & _
                                    ");", _
                                    "CREATE TABLE IF NOT EXISTS TVSeason(" & _
                                    "TVShowID INTEGER NOT NULL, " & _
                                    "TVEpID INTEGER NOT NULL, " & _
                                    "SeasonText TEXT, " & _
                                    "Season INTEGER NOT NULL, " & _
                                    "HasPoster BOOL NOT NULL DEFAULT False, " & _
                                    "HasFanart BOOL NOT NULL DEFAULT False, " & _
                                    "PosterPath TEXT, " & _
                                    "FanartPath TEXT, " & _
                                    "PRIMARY KEY (TVShowID,TVEpID)" & _
                                    ");", _
                                "CREATE TABLE IF NOT EXISTS TVVStreams(" & _
                                    "TVEpID INTEGER NOT NULL, " & _
                                    "StreamID INTEGER NOT NULL, " & _
                                    "Video_Width TEXT, " & _
                                    "Video_Height TEXT," & _
                                    "Video_Codec TEXT, " & _
                                    "Video_Duration TEXT, " & _
                                    "Video_ScanType TEXT, " & _
                                    "Video_AspectDisplayRatio TEXT, " & _
                                    "Video_Language TEXT, " & _
                                    "Video_LongLanguage TEXT, " & _
                                    "PRIMARY KEY (TVEpID,StreamID) " & _
                                    ");", _
                                "CREATE TABLE IF NOT EXISTS TVAStreams(" & _
                                    "TVEpID INTEGER NOT NULL, " & _
                                    "StreamID INTEGER NOT NULL, " & _
                                    "Audio_Language TEXT, " & _
                                    "Audio_LongLanguage TEXT, " & _
                                    "Audio_Codec TEXT, " & _
                                    "Audio_Channel TEXT, " & _
                                    "PRIMARY KEY (TVEpID,StreamID) " & _
                                    ");", _
                                "CREATE TABLE IF NOT EXISTS TVSubs(" & _
                                    "TVEpID INTEGER NOT NULL, " & _
                                    "StreamID INTEGER NOT NULL, " & _
                                    "Subs_Language TEXT, " & _
                                    "Subs_LongLanguage TEXT, " & _
                                    "PRIMARY KEY (TVEpID,StreamID) " & _
                                     ");", _
                                 "CREATE TABLE IF NOT EXISTS TVSources(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, path TEXT NOT NULL, LastScan TEXT NOT NULL DEFAULT '1900/01/01');", _
                                 "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVSource ON TVSources (Path);" _
                                    }

End Class