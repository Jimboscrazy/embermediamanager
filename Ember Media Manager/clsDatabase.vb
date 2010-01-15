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

Imports System
Imports System.IO

Public Class Database
    Public SQLcn As New SQLite.SQLiteConnection()

    ''' <summary>
    ''' Create a SQLite command
    ''' </summary>
    ''' <returns>Created command on the global connection</returns>
    Public Function CreateCommand() As SQLite.SQLiteCommand
        Return SQLcn.CreateCommand
    End Function

    ''' <summary>
    ''' Begin a SQLite transaction
    ''' </summary>
    ''' <returns>Created transaction on the global connection</returns>
    Public Function BeginTransaction() As SQLite.SQLiteTransaction
        Return SQLcn.BeginTransaction
    End Function

    ''' <summary>
    ''' Fill DataTable with data returned from the provided command
    ''' </summary>
    ''' <param name="dTable">DataTable to fill</param>
    ''' <param name="Command">SQL Command to process</param>
    Public Sub FillDataTable(ByRef dTable As DataTable, ByVal Command As String)
        Try
            dTable.Clear()
            Dim sqlDA As New SQLite.SQLiteDataAdapter(Command, SQLcn)
            Dim sqlCB As New SQLite.SQLiteCommandBuilder(sqlDA)
            sqlDA.Fill(dTable)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub Close()
        SQLcn.Close()
    End Sub

    ''' <summary>
    ''' Connect to, create, or update the SQLite database
    ''' </summary>
    ''' <param name="Reset">Rebuild the database (preserves data)</param>
    ''' <param name="Delete">Completely remove the SQLite database (only necessary in rare cases)</param>
    Public Sub Connect(ByVal Reset As Boolean, ByVal Delete As Boolean)
        Try
            Dim NewDB As Boolean = False
            'create database if it doesn't exist
            If Not File.Exists(Path.Combine(Master.AppPath, "Media.emm")) Then
                NewDB = True
            ElseIf Delete Then
                NewDB = True
                File.Delete(Path.Combine(Master.AppPath, "Media.emm"))
            End If

            SQLcn.ConnectionString = String.Format("Data Source=""{0}"";Compress=True", Path.Combine(Master.AppPath, "Media.emm"))
            SQLcn.Open()
            Dim cQuery As String = String.Empty
            Dim sQuery As String = String.Empty
            Dim vQuery As String = String.Empty
            Using SQLtransaction As SQLite.SQLiteTransaction = SQLcn.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand

                    If Not NewDB AndAlso Reset Then
                        Dim tColumns As New DataTable
                        Dim tRestrict() As String = New String(2) {Nothing, Nothing, "movies"}
                        Dim aCol As New List(Of String)
                        tColumns = SQLcn.GetSchema("Columns", tRestrict)
                        For Each col As DataRow In tColumns.Rows
                            aCol.Add(col("column_name").ToString)
                        Next
                        cQuery = String.Format("({0})", Strings.Join(aCol.ToArray, ", "))
                        SQLcommand.CommandText = "DROP INDEX IF EXISTS UniquePath;"
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "ALTER TABLE Movies RENAME TO tmp_movies;"
                        SQLcommand.ExecuteNonQuery()

                        aCol.Clear()
                        tRestrict = New String(2) {Nothing, Nothing, "sources"}
                        tColumns = SQLcn.GetSchema("Columns", tRestrict)
                        For Each col As DataRow In tColumns.Rows
                            aCol.Add(col("column_name").ToString)
                        Next
                        sQuery = String.Format("({0})", Strings.Join(aCol.ToArray, ", "))
                        SQLcommand.CommandText = "DROP INDEX IF EXISTS UniqueSource;"
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "ALTER TABLE Sources RENAME TO tmp_sources;"
                        SQLcommand.ExecuteNonQuery()

                        aCol.Clear()
                        tRestrict = New String(2) {Nothing, Nothing, "MoviesVStreams"}
                        tColumns = SQLcn.GetSchema("Columns", tRestrict)
                        For Each col As DataRow In tColumns.Rows
                            aCol.Add(col("column_name").ToString)
                        Next
                        vQuery = String.Format("({0})", Strings.Join(aCol.ToArray, ", "))
                        SQLcommand.CommandText = "ALTER TABLE MoviesVStreams RENAME TO tmp_moviesvstreams;"
                        SQLcommand.ExecuteNonQuery()
                    End If
                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS Movies(" & _
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
                                ");"
                    SQLcommand.ExecuteNonQuery()
                    SQLcommand.CommandText = "CREATE UNIQUE INDEX IF NOT EXISTS UniquePath ON Movies (MoviePath);"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS Sets(" & _
                                "SetName TEXT NOT NULL PRIMARY KEY" & _
                                 ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS MoviesSets(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "SetName TEXT NOT NULL, " & _
                                "SetOrder TEXT NOT NULL, " & _
                                "PRIMARY KEY (MovieID,SetName) " & _
                                 ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS MoviesVStreams(" & _
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
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS MoviesAStreams(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "StreamID INTEGER NOT NULL, " & _
                                "Audio_Language TEXT, " & _
                                "Audio_LongLanguage TEXT, " & _
                                "Audio_Codec TEXT, " & _
                                "Audio_Channel TEXT, " & _
                                "PRIMARY KEY (MovieID,StreamID) " & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS MoviesSubs(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "StreamID INTEGER NOT NULL, " & _
                                "Subs_Language TEXT, " & _
                                "Subs_LongLanguage TEXT, " & _
                                "PRIMARY KEY (MovieID,StreamID) " & _
                                 ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS MoviesPosters(" & _
                                "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                "MovieID INTEGER NOT NULL, " & _
                                "thumbs TEXT" & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS MoviesFanart(" & _
                                "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                "MovieID INTEGER NOT NULL, " & _
                                "preview TEXT, " & _
                                "thumbs TEXT" & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS Actors(" & _
                                "Name TEXT PRIMARY KEY, " & _
                                "thumb TEXT" & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS MoviesActors(" & _
                                "MovieID INTEGER NOT NULL, " & _
                                "ActorName TEXT NOT NULL, " & _
                                "Role TEXT, " & _
                                "PRIMARY KEY (MovieID,ActorName) " & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS Sources(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, path TEXT NOT NULL, Recursive BOOL NOT NULL DEFAULT False , Foldername BOOL NOT NULL DEFAULT False, Single BOOL NOT NULL DEFAULT False, LastScan TEXT NOT NULL DEFAULT '1900/01/01');"
                    SQLcommand.ExecuteNonQuery()
                    SQLcommand.CommandText = "CREATE UNIQUE INDEX IF NOT EXISTS UniqueSource ON Sources (Path);"
                    SQLcommand.ExecuteNonQuery()

                    If Not NewDB AndAlso Reset Then
                        SQLcommand.CommandText = String.Concat("INSERT INTO Movies ", cQuery, " SELECT * FROM tmp_movies;")
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "DROP TABLE tmp_movies;"
                        SQLcommand.ExecuteNonQuery()

                        SQLcommand.CommandText = String.Concat("INSERT INTO Sources ", sQuery, " SELECT * FROM tmp_sources;")
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "DROP TABLE tmp_sources;"
                        SQLcommand.ExecuteNonQuery()

                        SQLcommand.CommandText = String.Concat("INSERT INTO MoviesVStreams ", vQuery, " SELECT * FROM tmp_moviesvstreams;")
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "DROP TABLE tmp_moviesvstreams;"
                        SQLcommand.ExecuteNonQuery()
                    End If

                    'TV Entries
                    If Not NewDB AndAlso Reset Then
                        Dim tColumns As New DataTable
                        Dim tRestrict() As String = New String(2) {Nothing, Nothing, "TVShows"}
                        Dim aCol As New List(Of String)
                        tColumns = SQLcn.GetSchema("Columns", tRestrict)
                        For Each col As DataRow In tColumns.Rows
                            aCol.Add(col("column_name").ToString)
                        Next
                        cQuery = String.Format("({0})", Strings.Join(aCol.ToArray, ", "))
                        SQLcommand.CommandText = "DROP INDEX IF EXISTS UniqueTVShowPath;"
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "ALTER TABLE TVShows RENAME TO tmp_tvshows;"
                        SQLcommand.ExecuteNonQuery()
                    End If

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVShows(" & _
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
                                            "PosterPath TEXT, " & _
                                            "FanartPath TEXT, " & _
                                            "NfoPath TEXT, " & _
                                            "NeedsSave BOOL NOT NULL DEFAULT False" & _
                                            ");"
                    SQLcommand.ExecuteNonQuery()
                    SQLcommand.CommandText = "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVShowPath ON TVShows (TVShowPath);"
                    SQLcommand.ExecuteNonQuery()

                    If Not NewDB AndAlso Reset Then
                        SQLcommand.CommandText = String.Concat("INSERT INTO TVShows ", cQuery, " SELECT * FROM tmp_tvshows;")
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "DROP TABLE tmp_tvshows;"
                        SQLcommand.ExecuteNonQuery()
                    End If

                    If Not NewDB AndAlso Reset Then
                        Dim tColumns As New DataTable
                        Dim tRestrict() As String = New String(2) {Nothing, Nothing, "TVEps"}
                        Dim aCol As New List(Of String)
                        tColumns = SQLcn.GetSchema("Columns", tRestrict)
                        For Each col As DataRow In tColumns.Rows
                            aCol.Add(col("column_name").ToString)
                        Next
                        cQuery = String.Format("({0})", Strings.Join(aCol.ToArray, ", "))
                        SQLcommand.CommandText = "DROP INDEX IF EXISTS UniqueTVEpPath;"
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "ALTER TABLE TVEps RENAME TO tmp_tveps;"
                        SQLcommand.ExecuteNonQuery()
                    End If

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVEps(" & _
                                            "ID INTEGER PRIMARY KEY AUTOINCREMENT, " & _
                                            "TVShowID INTEGER NOT NULL, " & _
                                            "Title TEXT, " & _
                                            "HasPoster BOOL NOT NULL DEFAULT False, " & _
                                            "HasNfo BOOL NOT NULL DEFAULT False, " & _
                                            "New BOOL DEFAULT False, " & _
                                            "Mark BOOL NOT NULL DEFAULT False, " & _
                                            "TVEpPath TEXT NOT NULL, " & _
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
                                            "NfoPath TEXT, " & _
                                            "NeedsSave BOOL NOT NULL DEFAULT False" & _
                                            ");"
                    SQLcommand.ExecuteNonQuery()
                    SQLcommand.CommandText = "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVEpPath ON TVEps (TVEpPath);"
                    SQLcommand.ExecuteNonQuery()

                    If Not NewDB AndAlso Reset Then
                        SQLcommand.CommandText = String.Concat("INSERT INTO TVEps ", cQuery, " SELECT * FROM tmp_tveps;")
                        SQLcommand.ExecuteNonQuery()
                        SQLcommand.CommandText = "DROP TABLE tmp_tveps;"
                        SQLcommand.ExecuteNonQuery()
                    End If

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVShowActors(" & _
                                "TVShowID INTEGER NOT NULL, " & _
                                "ActorName TEXT NOT NULL, " & _
                                "Role TEXT, " & _
                                "PRIMARY KEY (TVShowID,ActorName) " & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVEpActors(" & _
                                "TVEpID INTEGER NOT NULL, " & _
                                "ActorName TEXT NOT NULL, " & _
                                "Role TEXT, " & _
                                "PRIMARY KEY (TVEpID,ActorName) " & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVSeason(" & _
                                "TVShowID INTEGER NOT NULL, " & _
                                "TVEpID INTEGER NOT NULL, " & _
                                "SeasonText TEXT, " & _
                                "Season INTEGER NOT NULL, " & _
                                "HasPoster BOOL NOT NULL DEFAULT False, " & _
                                "PosterPath TEXT, " & _
                                "PRIMARY KEY (TVShowID,TVEpID)" & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVVStreams(" & _
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
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVAStreams(" & _
                                "TVEpID INTEGER NOT NULL, " & _
                                "StreamID INTEGER NOT NULL, " & _
                                "Audio_Language TEXT, " & _
                                "Audio_LongLanguage TEXT, " & _
                                "Audio_Codec TEXT, " & _
                                "Audio_Channel TEXT, " & _
                                "PRIMARY KEY (TVEpID,StreamID) " & _
                                ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVSubs(" & _
                                "TVEpID INTEGER NOT NULL, " & _
                                "StreamID INTEGER NOT NULL, " & _
                                "Subs_Language TEXT, " & _
                                "Subs_LongLanguage TEXT, " & _
                                "PRIMARY KEY (TVEpID,StreamID) " & _
                                 ");"
                    SQLcommand.ExecuteNonQuery()

                    SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS TVSources(ID INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, path TEXT NOT NULL, LastScan TEXT NOT NULL DEFAULT '1900/01/01');"
                    SQLcommand.ExecuteNonQuery()
                    SQLcommand.CommandText = "CREATE UNIQUE INDEX IF NOT EXISTS UniqueTVSource ON TVSources (Path);"
                    SQLcommand.ExecuteNonQuery()
                End Using
                SQLtransaction.Commit()
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Load all the information for a movie.
    ''' </summary>
    ''' <param name="id">ID of the movie to load, as stored in the database</param>
    ''' <returns>Master.DBMovie object</returns>
    Public Function LoadMovieFromDB(ByVal MovieID As Long) As Master.DBMovie
        Dim _movieDB As New Master.DBMovie
        Try
            _movieDB.ID = MovieID
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM movies WHERE id = ", MovieID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()

                    If Not DBNull.Value.Equals(SQLreader("ListTitle")) Then _movieDB.ListTitle = SQLreader("ListTitle").ToString
                    If Not DBNull.Value.Equals(SQLreader("MoviePath")) Then _movieDB.Filename = SQLreader("MoviePath").ToString
                    _movieDB.isSingle = Convert.ToBoolean(SQLreader("type"))
                    If Not DBNull.Value.Equals(SQLreader("FanartPath")) Then _movieDB.FanartPath = SQLreader("FanartPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("PosterPath")) Then _movieDB.PosterPath = SQLreader("PosterPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("TrailerPath")) Then _movieDB.TrailerPath = SQLreader("TrailerPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("NfoPath")) Then _movieDB.NfoPath = SQLreader("NfoPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("SubPath")) Then _movieDB.SubPath = SQLreader("SubPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("ExtraPath")) Then _movieDB.ExtraPath = SQLreader("ExtraPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("source")) Then _movieDB.Source = SQLreader("source").ToString
                    _movieDB.IsMark = Convert.ToBoolean(SQLreader("mark"))
                    _movieDB.IsLock = Convert.ToBoolean(SQLreader("lock"))
                    _movieDB.UseFolder = Convert.ToBoolean(SQLreader("UseFolder"))
                    _movieDB.OutOfTolerance = Convert.ToBoolean(SQLreader("OutOfTolerance"))
                    _movieDB.NeedsSave = Convert.ToBoolean(SQLreader("NeedsSave"))
                    If Not DBNull.Value.Equals(SQLreader("FileSource")) Then _movieDB.FileSource = SQLreader("FileSource").ToString
                    _movieDB.Movie = New Media.Movie
                    With _movieDB.Movie
                        .Clear()
                        If Not DBNull.Value.Equals(SQLreader("IMDB")) Then .ID = SQLreader("IMDB").ToString
                        If Not DBNull.Value.Equals(SQLreader("Title")) Then .Title = SQLreader("Title").ToString
                        If Not DBNull.Value.Equals(SQLreader("OriginalTitle")) Then .OriginalTitle = SQLreader("OriginalTitle").ToString
                        If Not DBNull.Value.Equals(SQLreader("SortTitle")) Then .SortTitle = SQLreader("SortTitle").ToString
                        If Not DBNull.Value.Equals(SQLreader("Year")) Then .Year = SQLreader("Year").ToString
                        If Not DBNull.Value.Equals(SQLreader("Rating")) Then .Rating = SQLreader("Rating").ToString
                        If Not DBNull.Value.Equals(SQLreader("Votes")) Then .Votes = SQLreader("Votes").ToString
                        If Not DBNull.Value.Equals(SQLreader("MPAA")) Then .MPAA = SQLreader("MPAA").ToString
                        If Not DBNull.Value.Equals(SQLreader("Top250")) Then .Top250 = SQLreader("Top250").ToString
                        If Not DBNull.Value.Equals(SQLreader("Outline")) Then .Outline = SQLreader("Outline").ToString
                        If Not DBNull.Value.Equals(SQLreader("Plot")) Then .Plot = SQLreader("Plot").ToString
                        If Not DBNull.Value.Equals(SQLreader("Tagline")) Then .Tagline = SQLreader("Tagline").ToString
                        If Not DBNull.Value.Equals(SQLreader("Trailer")) Then .Trailer = SQLreader("Trailer").ToString
                        If Not DBNull.Value.Equals(SQLreader("Certification")) Then .Certification = SQLreader("Certification").ToString
                        If Not DBNull.Value.Equals(SQLreader("Genre")) Then .Genre = SQLreader("Genre").ToString
                        If Not DBNull.Value.Equals(SQLreader("Runtime")) Then .Runtime = SQLreader("Runtime").ToString
                        If Not DBNull.Value.Equals(SQLreader("ReleaseDate")) Then .ReleaseDate = SQLreader("ReleaseDate").ToString
                        If Not DBNull.Value.Equals(SQLreader("Studio")) Then .Studio = SQLreader("Studio").ToString
                        If Not DBNull.Value.Equals(SQLreader("Director")) Then .Director = SQLreader("Director").ToString
                        If Not DBNull.Value.Equals(SQLreader("Credits")) Then .Credits = SQLreader("Credits").ToString
                        If Not DBNull.Value.Equals(SQLreader("PlayCount")) Then .PlayCount = SQLreader("PlayCount").ToString
                        If Not DBNull.Value.Equals(SQLreader("Watched")) Then .Watched = SQLreader("Watched").ToString
                        If Not DBNull.Value.Equals(SQLreader("File")) Then .File = SQLreader("File").ToString
                        If Not DBNull.Value.Equals(SQLreader("Path")) Then .Path = SQLreader("Path").ToString
                        If Not DBNull.Value.Equals(SQLreader("FileNameAndPath")) Then .FileNameAndPath = SQLreader("FileNameAndPath").ToString
                        If Not DBNull.Value.Equals(SQLreader("Status")) Then .Status = SQLreader("Status").ToString
                        If Not DBNull.Value.Equals(SQLreader("FanartURL")) Then .Fanart.URL = SQLreader("FanartURL").ToString
                    End With

                End Using
            End Using

            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT MA.MovieID, MA.ActorName , MA.Role ,Act.Name,Act.thumb FROM MoviesActors AS MA ", _
                                                       "INNER JOIN Actors AS Act ON (MA.ActorName = Act.Name) WHERE MA.MovieID = ", _movieDB.ID, " ORDER BY MA.ROWID;")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim person As Media.Person
                    While SQLreader.Read
                        person = New Media.Person
                        person.Name = SQLreader("ActorName").ToString
                        person.Role = SQLreader("Role").ToString
                        person.Thumb = SQLreader("thumb").ToString
                        _movieDB.Movie.Actors.Add(person)
                    End While
                End Using
            End Using

            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM MoviesVStreams WHERE MovieID = ", MovieID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim video As MediaInfo.Video
                    While SQLreader.Read
                        video = New MediaInfo.Video
                        If Not DBNull.Value.Equals(SQLreader("Video_Width")) Then video.Width = SQLreader("Video_Width").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Height")) Then video.Height = SQLreader("Video_Height").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Codec")) Then video.Codec = SQLreader("Video_Codec").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Duration")) Then video.Duration = SQLreader("Video_Duration").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_ScanType")) Then video.Scantype = SQLreader("Video_ScanType").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_AspectDisplayRatio")) Then video.Aspect = SQLreader("Video_AspectDisplayRatio").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Language")) Then video.Language = SQLreader("Video_Language").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_LongLanguage")) Then video.LongLanguage = SQLreader("Video_LongLanguage").ToString
                        _movieDB.Movie.FileInfo.StreamDetails.Video.Add(video)
                    End While
                End Using
            End Using

            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM MoviesAStreams WHERE MovieID = ", MovieID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim audio As MediaInfo.Audio
                    While SQLreader.Read
                        audio = New MediaInfo.Audio
                        If Not DBNull.Value.Equals(SQLreader("Audio_Language")) Then audio.Language = SQLreader("Audio_Language").ToString
                        If Not DBNull.Value.Equals(SQLreader("Audio_LongLanguage")) Then audio.LongLanguage = SQLreader("Audio_LongLanguage").ToString
                        If Not DBNull.Value.Equals(SQLreader("Audio_Codec")) Then audio.Codec = SQLreader("Audio_Codec").ToString
                        If Not DBNull.Value.Equals(SQLreader("Audio_Channel")) Then audio.Channels = SQLreader("Audio_Channel").ToString
                        _movieDB.Movie.FileInfo.StreamDetails.Audio.Add(audio)
                    End While
                End Using
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM MoviesSubs WHERE MovieID = ", MovieID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim subtitle As MediaInfo.Subtitle
                    While SQLreader.Read
                        subtitle = New MediaInfo.Subtitle
                        If Not DBNull.Value.Equals(SQLreader("Subs_Language")) Then subtitle.Language = SQLreader("Subs_Language").ToString
                        If Not DBNull.Value.Equals(SQLreader("Subs_LongLanguage")) Then subtitle.LongLanguage = SQLreader("Subs_LongLanguage").ToString
                        _movieDB.Movie.FileInfo.StreamDetails.Subtitle.Add(subtitle)
                    End While
                End Using
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM MoviesSets WHERE MovieID = ", MovieID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim sets As Media.set
                    While SQLreader.Read
                        sets = New Media.set
                        If Not DBNull.Value.Equals(SQLreader("SetName")) Then sets.Set = SQLreader("SetName").ToString
                        If Not DBNull.Value.Equals(SQLreader("SetOrder")) Then sets.Order = SQLreader("SetOrder").ToString
                        _movieDB.Movie.sets.Add(sets)
                    End While
                End Using
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM MoviesFanart WHERE MovieID = ", MovieID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim thumb As Media.Thumb
                    While SQLreader.Read
                        thumb = New Media.Thumb
                        If Not DBNull.Value.Equals(SQLreader("preview")) Then thumb.Preview = SQLreader("preview").ToString
                        If Not DBNull.Value.Equals(SQLreader("thumbs")) Then thumb.Text = SQLreader("thumbs").ToString
                        _movieDB.Movie.Fanart.Thumb.Add(thumb)
                    End While
                End Using
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM MoviesPosters WHERE MovieID = ", MovieID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    While SQLreader.Read
                        If Not DBNull.Value.Equals(SQLreader("thumbs")) Then _movieDB.Movie.Thumb.Add(SQLreader("thumbs").ToString)
                    End While
                End Using
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            _movieDB.ID = -1
        End Try
        Return _movieDB
    End Function

    ''' <summary>
    ''' Load all the information for a movie (by movie path)
    ''' </summary>
    ''' <param name="sPath">Full path to the movie file</param>
    ''' <returns>Master.DBMovie object</returns>
    Public Function LoadMovieFromDB(ByVal sPath As String) As Master.DBMovie
        Try
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                ' One more Query Better then re-write all function again
                SQLcommand.CommandText = String.Concat("SELECT ID FROM movies WHERE MoviePath = ", sPath, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    If SQLreader.Read Then
                        Return LoadMovieFromDB(Convert.ToInt64(SQLreader("ID")))
                    Else
                        Return New Master.DBMovie With {.Id = -1} ' No Movie Found
                    End If
                End Using
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return New Master.DBMovie With {.Id = -1}
    End Function

    ''' <summary>
    ''' Saves all information from a Master.DBMovie object to the database
    ''' </summary>
    ''' <param name="_movieDB">Media.Movie object to save to the database</param>
    ''' <param name="IsNew">Is this a new movie (not already present in database)?</param>
    ''' <param name="BatchMode">Is the function already part of a transaction?</param>
    ''' <param name="ToNfo">Save the information to an nfo file?</param>
    ''' <returns>Master.DBMovie object</returns>
    Public Function SaveMovieToDB(ByVal _movieDB As Master.DBMovie, ByVal IsNew As Boolean, Optional ByVal BatchMode As Boolean = False, Optional ByVal ToNfo As Boolean = False) As Master.DBMovie

        Try
            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing
            If Not BatchMode Then SQLtransaction = SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                If IsNew Then
                    SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO movies (", _
                        "MoviePath, type, ListTitle, HasPoster, HasFanart, HasNfo, HasTrailer, HasSub, HasExtra, new, mark, source, imdb, lock,", _
                        "Title, OriginalTitle, SortTitle, Year, Rating, Votes, MPAA, Top250, Outline, Plot, Tagline, Certification, Genre,", _
                        "Studio, Runtime, ReleaseDate, Director, Credits, Playcount, Watched, Status, File, Path, FileNameAndPath, Trailer, ", _
                        "PosterPath, FanartPath, NfoPath, TrailerPath, SubPath, ExtraPath, FanartURL, UseFolder, OutOfTolerance, FileSource, NeedsSave", _
                        ") VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); SELECT LAST_INSERT_ROWID() FROM movies;")
                Else
                    SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO movies (", _
                        "ID, MoviePath, type, ListTitle, HasPoster, HasFanart, HasNfo, HasTrailer, HasSub, HasExtra, new, mark, source, imdb, lock,", _
                        "Title, OriginalTitle, SortTitle, Year, Rating, Votes, MPAA, Top250, Outline, Plot, Tagline, Certification, Genre,", _
                        "Studio, Runtime, ReleaseDate, Director, Credits, Playcount, Watched, Status, File, Path, FileNameAndPath, Trailer, ", _
                        "PosterPath, FanartPath, NfoPath, TrailerPath, SubPath, ExtraPath, FanartURL, UseFolder, OutOfTolerance, FileSource, NeedsSave", _
                        ") VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); SELECT LAST_INSERT_ROWID() FROM movies;")
                    Dim parMovieID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMovieID", DbType.Int32, 0, "ID")
                    parMovieID.Value = _movieDB.ID
                End If

                Dim parMoviePath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMoviePath", DbType.String, 0, "MoviePath")
                Dim parType As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parType", DbType.Boolean, 0, "type")
                Dim parListTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parListTitle", DbType.String, 0, "ListTitle")
                Dim parHasPoster As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasPoster", DbType.Boolean, 0, "HasPoster")
                Dim parHasFanart As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasFanart", DbType.Boolean, 0, "HasFanart")
                Dim parHasNfo As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasInfo", DbType.Boolean, 0, "HasNfo")
                Dim parHasTrailer As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasTrailer", DbType.Boolean, 0, "HasTrailer")
                Dim parHasSub As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasSub", DbType.Boolean, 0, "HasSub")
                Dim parHasExtra As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasExtra", DbType.Boolean, 0, "HasExtra")
                Dim parNew As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNew", DbType.Boolean, 0, "new")
                Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                Dim parIMDB As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parIMDB", DbType.String, 0, "imdb")
                Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")

                Dim parTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTitle", DbType.String, 0, "Title")
                Dim parOriginalTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parOriginalTitle", DbType.String, 0, "OriginalTitle")
                Dim parSortTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSortTitle", DbType.String, 0, "SortTitle")
                Dim parYear As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parYear", DbType.String, 0, "Year")
                Dim parRating As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parRating", DbType.String, 0, "Rating")
                Dim parVotes As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parVotes", DbType.String, 0, "Votes")
                Dim parMPAA As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMPAA", DbType.String, 0, "MPAA")
                Dim parTop250 As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTop250", DbType.String, 0, "Top250")
                Dim parOutline As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parOutline", DbType.String, 0, "Outline")
                Dim parPlot As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPlot", DbType.String, 0, "Plot")
                Dim parTagline As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTagline", DbType.String, 0, "Tagline")
                Dim parCertification As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parCertification", DbType.String, 0, "Certification")
                Dim parGenre As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parGenre", DbType.String, 0, "Genre")
                Dim parStudio As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parStudio", DbType.String, 0, "Studio")
                Dim parRuntime As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parRuntime", DbType.String, 0, "Runtime")
                Dim parReleaseDate As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parReleaseDate", DbType.String, 0, "ReleaseDate")
                Dim parDirector As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parDirector", DbType.String, 0, "Director")
                Dim parCredits As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parCredits", DbType.String, 0, "Credits")
                Dim parPlaycount As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPlaycount", DbType.String, 0, "Playcount")
                Dim parWatched As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parWatched", DbType.String, 0, "Watched")
                Dim parFile As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFile", DbType.String, 0, "File")
                Dim parPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPath", DbType.String, 0, "Path")
                Dim parFileNameAndPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFileNameAndPath", DbType.String, 0, "FileNameAndPath")
                Dim parStatus As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parStatus", DbType.String, 0, "Status")
                Dim parTrailer As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTrailer", DbType.String, 0, "Trailer")

                Dim parPosterPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPosterPath", DbType.String, 0, "PosterPath")
                Dim parFanartPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFanartPath", DbType.String, 0, "FanartPath")
                Dim parNfoPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNfoPath", DbType.String, 0, "NfoPath")
                Dim parTrailerPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTrailerPath", DbType.String, 0, "TrailerPath")
                Dim parSubPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSubPath", DbType.String, 0, "SubPath")
                Dim parExtraPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parExtraPath", DbType.String, 0, "ExtraPath")
                Dim parFanartURL As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFanartURL", DbType.String, 0, "FanartURL")
                Dim parUseFolder As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parUseFolder", DbType.Boolean, 0, "UseFolder")
                Dim parOutOfTolerance As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parOutOfTolerance", DbType.Boolean, 0, "OutOfTolerance")
                Dim parFileSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFileSource", DbType.String, 0, "FileSource")
                Dim parNeedsSave As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNeedsSave", DbType.Boolean, 0, "NeedsSave")

                ' First let's save it to NFO, even because we will need the NFO path
                'If ToNfo AndAlso Not String.IsNullOrEmpty(_movieDB.Movie.IMDBID) Then NFO.SaveMovieToNFO(_movieDB)
                'Why do we need IMDB to save to NFO?
                If ToNfo Then NFO.SaveMovieToNFO(_movieDB)

                parMoviePath.Value = _movieDB.Filename
                parType.Value = _movieDB.isSingle
                parListTitle.Value = _movieDB.ListTitle

                parPosterPath.Value = _movieDB.PosterPath
                parFanartPath.Value = _movieDB.FanartPath
                parNfoPath.Value = _movieDB.NfoPath
                parTrailerPath.Value = _movieDB.TrailerPath
                parSubPath.Value = _movieDB.SubPath
                parExtraPath.Value = _movieDB.ExtraPath
                parFanartURL.Value = _movieDB.Movie.Fanart.URL

                parHasPoster.Value = Not String.IsNullOrEmpty(_movieDB.PosterPath)
                parHasFanart.Value = Not String.IsNullOrEmpty(_movieDB.FanartPath)
                parHasNfo.Value = Not String.IsNullOrEmpty(_movieDB.NfoPath)
                parHasTrailer.Value = Not String.IsNullOrEmpty(_movieDB.TrailerPath)
                parHasSub.Value = Not String.IsNullOrEmpty(_movieDB.SubPath)
                parHasExtra.Value = Not String.IsNullOrEmpty(_movieDB.ExtraPath)

                parNew.Value = _movieDB.IsNew
                parMark.Value = _movieDB.IsMark
                parLock.Value = _movieDB.IsLock

                parIMDB.Value = _movieDB.Movie.IMDBID
                parTitle.Value = _movieDB.Movie.Title
                parOriginalTitle.Value = _movieDB.Movie.OriginalTitle
                parSortTitle.Value = _movieDB.Movie.SortTitle
                parYear.Value = _movieDB.Movie.Year
                parRating.Value = _movieDB.Movie.Rating
                parVotes.Value = _movieDB.Movie.Votes
                parMPAA.Value = _movieDB.Movie.MPAA
                parTop250.Value = _movieDB.Movie.Top250
                parOutline.Value = _movieDB.Movie.Outline
                parPlot.Value = _movieDB.Movie.Plot
                parTagline.Value = _movieDB.Movie.Tagline
                parCertification.Value = _movieDB.Movie.Certification
                parGenre.Value = _movieDB.Movie.Genre
                parStudio.Value = _movieDB.Movie.Studio
                parRuntime.Value = _movieDB.Movie.Runtime
                parReleaseDate.Value = _movieDB.Movie.ReleaseDate
                parDirector.Value = _movieDB.Movie.Director
                parCredits.Value = _movieDB.Movie.Credits
                parPlaycount.Value = _movieDB.Movie.PlayCount
                parWatched.Value = _movieDB.Movie.Watched
                parStatus.Value = _movieDB.Movie.Status
                parFile.Value = _movieDB.Movie.File
                parPath.Value = _movieDB.Movie.Path
                parFileNameAndPath.Value = _movieDB.Movie.FileNameAndPath
                parTrailer.Value = _movieDB.Movie.Trailer

                parUseFolder.Value = _movieDB.UseFolder
                parOutOfTolerance.Value = _movieDB.OutOfTolerance
                parFileSource.Value = _movieDB.FileSource
                parNeedsSave.Value = _movieDB.NeedsSave

                parSource.Value = _movieDB.Source
                If IsNew Then
                    If Master.eSettings.MarkNew Then
                        parMark.Value = True
                    Else
                        parMark.Value = False
                    End If
                    Using rdrMovie As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                        If rdrMovie.Read Then
                            _movieDB.ID = Convert.ToInt64(rdrMovie(0))
                        Else
                            Master.eLog.WriteToErrorLog("Something very wrong here: SaveMovieToDB", _movieDB.ToString, "Error")
                            _movieDB.ID = -1
                            Return _movieDB
                        End If
                    End Using
                Else
                    SQLcommand.ExecuteNonQuery()
                End If

                If Not _movieDB.ID = -1 Then
                    Using SQLcommandActor As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandActor.CommandText = String.Concat("INSERT OR REPLACE INTO Actors (Name,thumb) VALUES (?,?)")
                        Dim parActorName As SQLite.SQLiteParameter = SQLcommandActor.Parameters.Add("parActorName", DbType.String, 0, "Name")
                        Dim parActorThumb As SQLite.SQLiteParameter = SQLcommandActor.Parameters.Add("parActorThumb", DbType.String, 0, "thumb")
                        For Each actor As Media.Person In _movieDB.Movie.Actors
                            parActorName.Value = actor.Name
                            parActorThumb.Value = actor.Thumb
                            SQLcommandActor.ExecuteNonQuery()
                            Using SQLcommandMoviesActors As SQLite.SQLiteCommand = SQLcn.CreateCommand
                                SQLcommandMoviesActors.CommandText = String.Concat("INSERT OR REPLACE INTO MoviesActors (MovieID,ActorName,Role) VALUES (?,?,?);")
                                Dim parMoviesActorsMovieID As SQLite.SQLiteParameter = SQLcommandMoviesActors.Parameters.Add("parMoviesActorsMovieID", DbType.UInt64, 0, "MovieID")
                                Dim parMoviesActorsActorName As SQLite.SQLiteParameter = SQLcommandMoviesActors.Parameters.Add("parMoviesActorsActorName", DbType.String, 0, "ActorNAme")
                                Dim parMoviesActorsActorRole As SQLite.SQLiteParameter = SQLcommandMoviesActors.Parameters.Add("parMoviesActorsActorRole", DbType.String, 0, "Role")
                                parMoviesActorsMovieID.Value = _movieDB.ID
                                parMoviesActorsActorName.Value = actor.Name
                                parMoviesActorsActorRole.Value = actor.Role
                                SQLcommandMoviesActors.ExecuteNonQuery()
                            End Using
                        Next
                    End Using
                    Using SQLcommandMoviesVStreams As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandMoviesVStreams.CommandText = String.Concat("INSERT OR REPLACE INTO MoviesVStreams (", _
                                "MovieID, StreamID, Video_Width,Video_Height,Video_Codec,Video_Duration,", _
                                "Video_ScanType, Video_AspectDisplayRatio, Video_Language, Video_LongLanguage", _
                                ") VALUES (?,?,?,?,?,?,?,?,?,?);")
                        Dim parVideo_MovieID As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_MovieID", DbType.UInt64, 0, "MovieID")
                        Dim parVideo_StreamID As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_StreamID", DbType.UInt64, 0, "StreamID")
                        Dim parVideo_Width As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_Width", DbType.String, 0, "Video_Width")
                        Dim parVideo_Height As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_Height", DbType.String, 0, "Video_Height")
                        Dim parVideo_Codec As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_Codec", DbType.String, 0, "Video_Codec")
                        Dim parVideo_Duration As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_Duration", DbType.String, 0, "Video_Duration")
                        Dim parVideo_ScanType As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_ScanType", DbType.String, 0, "Video_ScanType")
                        Dim parVideo_AspectDisplayRatio As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_AspectDisplayRatio", DbType.String, 0, "Video_AspectDisplayRatio")
                        Dim parVideo_Language As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_Language", DbType.String, 0, "Video_Language")
                        Dim parVideo_LongLanguage As SQLite.SQLiteParameter = SQLcommandMoviesVStreams.Parameters.Add("parVideo_LongLanguage", DbType.String, 0, "Video_LongLanguage")
                        For i As Integer = 0 To _movieDB.Movie.FileInfo.StreamDetails.Video.Count - 1
                            parVideo_MovieID.Value = _movieDB.ID
                            parVideo_StreamID.Value = i
                            parVideo_Width.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).Width
                            parVideo_Height.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).Height
                            parVideo_Codec.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).Codec
                            parVideo_Duration.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).Duration
                            parVideo_ScanType.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).Scantype
                            parVideo_AspectDisplayRatio.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).Aspect
                            parVideo_Language.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).Language
                            parVideo_LongLanguage.Value = _movieDB.Movie.FileInfo.StreamDetails.Video(i).LongLanguage
                            SQLcommandMoviesVStreams.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandMoviesAStreams As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandMoviesAStreams.CommandText = String.Concat("INSERT OR REPLACE INTO MoviesAStreams (", _
                                "MovieID, StreamID, Audio_Language, Audio_LongLanguage, Audio_Codec, Audio_Channel", _
                                ") VALUES (?,?,?,?,?,?);")
                        Dim parAudio_MovieID As SQLite.SQLiteParameter = SQLcommandMoviesAStreams.Parameters.Add("parAudio_MovieID", DbType.UInt64, 0, "MovieID")
                        Dim parAudio_StreamID As SQLite.SQLiteParameter = SQLcommandMoviesAStreams.Parameters.Add("parAudio_StreamID", DbType.UInt64, 0, "StreamID")
                        Dim parAudio_Language As SQLite.SQLiteParameter = SQLcommandMoviesAStreams.Parameters.Add("parAudio_Language", DbType.String, 0, "Audio_Language")
                        Dim parAudio_LongLanguage As SQLite.SQLiteParameter = SQLcommandMoviesAStreams.Parameters.Add("parAudio_LongLanguage", DbType.String, 0, "Audio_LongLanguage")
                        Dim parAudio_Codec As SQLite.SQLiteParameter = SQLcommandMoviesAStreams.Parameters.Add("parAudio_Codec", DbType.String, 0, "Audio_Codec")
                        Dim parAudio_Channel As SQLite.SQLiteParameter = SQLcommandMoviesAStreams.Parameters.Add("parAudio_Channel", DbType.String, 0, "Audio_Channel")
                        For i As Integer = 0 To _movieDB.Movie.FileInfo.StreamDetails.Audio.Count - 1
                            parAudio_MovieID.Value = _movieDB.ID
                            parAudio_StreamID.Value = i
                            parAudio_Language.Value = _movieDB.Movie.FileInfo.StreamDetails.Audio(i).Language
                            parAudio_LongLanguage.Value = _movieDB.Movie.FileInfo.StreamDetails.Audio(i).LongLanguage
                            parAudio_Codec.Value = _movieDB.Movie.FileInfo.StreamDetails.Audio(i).Codec
                            parAudio_Channel.Value = _movieDB.Movie.FileInfo.StreamDetails.Audio(i).Channels
                            SQLcommandMoviesAStreams.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandMoviesSubs As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandMoviesSubs.CommandText = String.Concat("INSERT OR REPLACE INTO MoviesSubs (", _
                                "MovieID, StreamID, Subs_Language, Subs_LongLanguage", _
                                ") VALUES (?,?,?,?);")
                        Dim parSubs_MovieID As SQLite.SQLiteParameter = SQLcommandMoviesSubs.Parameters.Add("parSubs_MovieID", DbType.UInt64, 0, "MovieID")
                        Dim parSubs_StreamID As SQLite.SQLiteParameter = SQLcommandMoviesSubs.Parameters.Add("parSubs_StreamID", DbType.UInt64, 0, "StreamID")
                        Dim parSubs_Language As SQLite.SQLiteParameter = SQLcommandMoviesSubs.Parameters.Add("parSubs_Language", DbType.String, 0, "Subs_Language")
                        Dim parSubs_LongLanguage As SQLite.SQLiteParameter = SQLcommandMoviesSubs.Parameters.Add("parSubs_LongLanguage", DbType.String, 0, "Subs_LongLanguage")
                        For i As Integer = 0 To _movieDB.Movie.FileInfo.StreamDetails.Subtitle.Count - 1
                            parSubs_MovieID.Value = _movieDB.ID
                            parSubs_StreamID.Value = i
                            parSubs_Language.Value = _movieDB.Movie.FileInfo.StreamDetails.Subtitle(i).Language
                            parSubs_LongLanguage.Value = _movieDB.Movie.FileInfo.StreamDetails.Subtitle(i).LongLanguage
                            SQLcommandMoviesSubs.ExecuteNonQuery()
                        Next
                    End Using
                    ' For what i understand this is used from Poster/Fanart Modules... will not be read/wrtire directly when load/save Movie
                    Using SQLcommandMoviesPosters As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandMoviesPosters.CommandText = String.Concat("INSERT OR REPLACE INTO MoviesPosters (", _
                                "MovieID, thumbs", _
                                ") VALUES (?,?);")
                        Dim parPosters_MovieID As SQLite.SQLiteParameter = SQLcommandMoviesPosters.Parameters.Add("parPosters_MovieID", DbType.UInt64, 0, "MovieID")
                        Dim parPosters_thumb As SQLite.SQLiteParameter = SQLcommandMoviesPosters.Parameters.Add("parPosters_thumb", DbType.String, 0, "thumbs")
                        For Each p As String In _movieDB.Movie.Thumb
                            parPosters_MovieID.Value = _movieDB.ID
                            parPosters_thumb.Value = p
                            SQLcommandMoviesPosters.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandMoviesFanart As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandMoviesFanart.CommandText = String.Concat("INSERT OR REPLACE INTO MoviesFanart (", _
                                "MovieID, preview, thumbs", _
                                ") VALUES (?,?,?);")
                        Dim parFanart_MovieID As SQLite.SQLiteParameter = SQLcommandMoviesFanart.Parameters.Add("parFanart_MovieID", DbType.UInt64, 0, "MovieID")
                        Dim parFanart_Preview As SQLite.SQLiteParameter = SQLcommandMoviesFanart.Parameters.Add("parFanart_Preview", DbType.String, 0, "Preview")
                        Dim parFanart_thumb As SQLite.SQLiteParameter = SQLcommandMoviesFanart.Parameters.Add("parFanart_thumb", DbType.String, 0, "thumb")
                        For Each p As Media.Thumb In _movieDB.Movie.Fanart.Thumb
                            parFanart_MovieID.Value = _movieDB.ID
                            parFanart_Preview.Value = p.Preview
                            parFanart_thumb.Value = p.Text
                            SQLcommandMoviesFanart.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandSets As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandSets.CommandText = String.Concat("INSERT OR REPLACE INTO Sets (", _
                                "SetName", _
                                ") VALUES (?);")
                        Dim parSets_SetName As SQLite.SQLiteParameter = SQLcommandSets.Parameters.Add("parSets_SetName", DbType.String, 0, "SetName")
                        For Each s As Media.set In _movieDB.Movie.sets
                            parSets_SetName.Value = s.Set
                            SQLcommandSets.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandMoviesSets As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandMoviesSets.CommandText = String.Concat("INSERT OR REPLACE INTO MoviesSets (", _
                                "MovieID,SetName,SetOrder", _
                                ") VALUES (?,?,?);")
                        Dim parMovieSets_MovieID As SQLite.SQLiteParameter = SQLcommandMoviesSets.Parameters.Add("parMovieSets_MovieID", DbType.UInt64, 0, "MovieID")
                        Dim parMovieSets_SetName As SQLite.SQLiteParameter = SQLcommandMoviesSets.Parameters.Add("parMovieSets_SetName", DbType.String, 0, "SetName")
                        Dim parMovieSets_SetOrder As SQLite.SQLiteParameter = SQLcommandMoviesSets.Parameters.Add("parMovieSets_SetOrder", DbType.String, 0, "SetOrder")
                        For Each s As Media.set In _movieDB.Movie.sets
                            parMovieSets_MovieID.Value = _movieDB.ID
                            parMovieSets_SetName.Value = s.Set
                            parMovieSets_SetOrder.Value = s.Order
                            SQLcommandMoviesSets.ExecuteNonQuery()
                        Next
                    End Using
                End If
            End Using
            If Not BatchMode Then SQLtransaction.Commit()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return _movieDB
    End Function

    ''' <summary>
    ''' Load all the information for a TV Episode.
    ''' </summary>
    ''' <param name="id">ID of the episode to load, as stored in the database</param>
    ''' <returns>Master.DBTV object</returns>
    Public Function LoadTVEpFromDB(ByVal EpID As Long) As Master.DBTV
        Dim _TVDB As New Master.DBTV
        Try
            _TVDB.EpID = EpID
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM TVEps WHERE id = ", EpID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    If SQLreader.HasRows Then
                        If Not DBNull.Value.Equals(SQLreader("TVEpPath")) Then _TVDB.Filename = SQLreader("TVEpPath").ToString
                        If Not DBNull.Value.Equals(SQLreader("EpPosterPath")) Then _TVDB.EpPosterPath = SQLreader("EpPosterPath").ToString
                        If Not DBNull.Value.Equals(SQLreader("EpNfoPath")) Then _TVDB.EpNfoPath = SQLreader("EpNfoPath").ToString
                        If Not DBNull.Value.Equals(SQLreader("Source")) Then _TVDB.Source = SQLreader("Source").ToString
                        If Not DBNull.Value.Equals(SQLreader("TVShowID")) Then _TVDB.ShowID = Convert.ToInt64(SQLreader("TVShowID"))
                        _TVDB.IsMarkEp = Convert.ToBoolean(SQLreader("Mark"))
                        _TVDB.IsLockEp = Convert.ToBoolean(SQLreader("Lock"))
                        _TVDB.EpNeedsSave = Convert.ToBoolean(SQLreader("NeedsSave"))
                        _TVDB.TVEp = New Media.EpisodeDetails
                        With _TVDB.TVEp
                            .Clear()
                            If Not DBNull.Value.Equals(SQLreader("Title")) Then .Title = SQLreader("Title").ToString
                            If Not DBNull.Value.Equals(SQLreader("Season")) Then .Season = Convert.ToInt32(SQLreader("Season"))
                            If Not DBNull.Value.Equals(SQLreader("Episode")) Then .Episode = Convert.ToInt32(SQLreader("Episode"))
                            If Not DBNull.Value.Equals(SQLreader("Aired")) Then .Aired = SQLreader("Aired").ToString
                            If Not DBNull.Value.Equals(SQLreader("Rating")) Then .Rating = SQLreader("Rating").ToString
                            If Not DBNull.Value.Equals(SQLreader("Plot")) Then .Plot = SQLreader("Plot").ToString
                            If Not DBNull.Value.Equals(SQLreader("Director")) Then .Director = SQLreader("Director").ToString
                            If Not DBNull.Value.Equals(SQLreader("Credits")) Then .Credits = SQLreader("Credits").ToString
                        End With
                    End If
                End Using
            End Using

            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT TA.TVEpID, TA.ActorName, TA.Role, Act.Name, Act.thumb FROM TVEpActors AS TA ", _
                                                       "INNER JOIN Actors AS Act ON (TA.ActorName = Act.Name) WHERE TA.TVEpID = ", EpID, " ORDER BY TA.ROWID;")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim person As Media.Person
                    While SQLreader.Read
                        person = New Media.Person
                        person.Name = SQLreader("ActorName").ToString
                        person.Role = SQLreader("Role").ToString
                        person.Thumb = SQLreader("thumb").ToString
                        _TVDB.TVEp.Actors.Add(person)
                    End While
                End Using
            End Using

            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM TVVStreams WHERE TVEpID = ", EpID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim video As MediaInfo.Video
                    While SQLreader.Read
                        video = New MediaInfo.Video
                        If Not DBNull.Value.Equals(SQLreader("Video_Width")) Then video.Width = SQLreader("Video_Width").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Height")) Then video.Height = SQLreader("Video_Height").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Codec")) Then video.Codec = SQLreader("Video_Codec").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Duration")) Then video.Duration = SQLreader("Video_Duration").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_ScanType")) Then video.Scantype = SQLreader("Video_ScanType").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_AspectDisplayRatio")) Then video.Aspect = SQLreader("Video_AspectDisplayRatio").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_Language")) Then video.Language = SQLreader("Video_Language").ToString
                        If Not DBNull.Value.Equals(SQLreader("Video_LongLanguage")) Then video.LongLanguage = SQLreader("Video_LongLanguage").ToString
                        _TVDB.TVEp.FileInfo.StreamDetails.Video.Add(video)
                    End While
                End Using
            End Using

            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM TVAStreams WHERE TVEpID = ", EpID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim audio As MediaInfo.Audio
                    While SQLreader.Read
                        audio = New MediaInfo.Audio
                        If Not DBNull.Value.Equals(SQLreader("Audio_Language")) Then audio.Language = SQLreader("Audio_Language").ToString
                        If Not DBNull.Value.Equals(SQLreader("Audio_LongLanguage")) Then audio.LongLanguage = SQLreader("Audio_LongLanguage").ToString
                        If Not DBNull.Value.Equals(SQLreader("Audio_Codec")) Then audio.Codec = SQLreader("Audio_Codec").ToString
                        If Not DBNull.Value.Equals(SQLreader("Audio_Channel")) Then audio.Channels = SQLreader("Audio_Channel").ToString
                        _TVDB.TVEp.FileInfo.StreamDetails.Audio.Add(audio)
                    End While
                End Using
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM TVSubs WHERE TVEpID = ", EpID, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Dim subtitle As MediaInfo.Subtitle
                    While SQLreader.Read
                        subtitle = New MediaInfo.Subtitle
                        If Not DBNull.Value.Equals(SQLreader("Subs_Language")) Then subtitle.Language = SQLreader("Subs_Language").ToString
                        If Not DBNull.Value.Equals(SQLreader("Subs_LongLanguage")) Then subtitle.LongLanguage = SQLreader("Subs_LongLanguage").ToString
                        _TVDB.TVEp.FileInfo.StreamDetails.Subtitle.Add(subtitle)
                    End While
                End Using
            End Using

            If _TVDB.ShowID > -1 Then
                _TVDB.TVShow = New Media.TVShow
                Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                    SQLcommand.CommandText = String.Concat("SELECT * FROM TVShow WHERE id = ", _TVDB.ShowID, ";")
                    Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                        If SQLreader.HasRows Then
                            With _TVDB.TVShow
                                .Clear()
                                _TVDB.IsMarkShow = Convert.ToBoolean(SQLreader("Mark"))
                                _TVDB.IsLockShow = Convert.ToBoolean(SQLreader("Lock"))
                                _TVDB.ShowNeedsSave = Convert.ToBoolean(SQLreader("NeedsSave"))
                                If Not DBNull.Value.Equals(SQLreader("PosterPath")) Then _TVDB.ShowPosterPath = SQLreader("PosterPath").ToString
                                If Not DBNull.Value.Equals(SQLreader("FanartPath")) Then _TVDB.ShowFanartPath = SQLreader("FanartPath").ToString
                                If Not DBNull.Value.Equals(SQLreader("NfoPath")) Then _TVDB.ShowNfoPath = SQLreader("NfoPath").ToString
                                If Not DBNull.Value.Equals(SQLreader("Title")) Then .Title = SQLreader("Title").ToString
                                If Not DBNull.Value.Equals(SQLreader("TVDB")) Then .ID = SQLreader("TVDB").ToString
                                If Not DBNull.Value.Equals(SQLreader("Rating")) Then .Rating = SQLreader("Rating").ToString
                                If Not DBNull.Value.Equals(SQLreader("EpisodeGuideURL")) Then .EpisodeGuideURL = SQLreader("EpisodeGuideURL").ToString
                                If Not DBNull.Value.Equals(SQLreader("Plot")) Then .Plot = SQLreader("Plot").ToString
                                If Not DBNull.Value.Equals(SQLreader("MPAA")) Then .MPAA = SQLreader("MPAA").ToString
                                If Not DBNull.Value.Equals(SQLreader("Genre")) Then .Genre = SQLreader("Genre").ToString
                                If Not DBNull.Value.Equals(SQLreader("Premiered")) Then .Premiered = SQLreader("Premiered").ToString
                                If Not DBNull.Value.Equals(SQLreader("Studio")) Then .Studio = SQLreader("Studio").ToString
                            End With
                        End If
                    End Using
                End Using

                Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                    SQLcommand.CommandText = String.Concat("SELECT TA.TVShowID, TA.ActorName, TA.Role, Act.Name, Act.thumb FROM TVShowActors AS TA ", _
                                                           "INNER JOIN Actors AS Act ON (TA.ActorName = Act.Name) WHERE TA.TVShowID = ", _TVDB.ShowID, " ORDER BY TA.ROWID;")
                    Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                        Dim person As Media.Person
                        While SQLreader.Read
                            person = New Media.Person
                            person.Name = SQLreader("ActorName").ToString
                            person.Role = SQLreader("Role").ToString
                            person.Thumb = SQLreader("thumb").ToString
                            _TVDB.TVShow.Actors.Add(person)
                        End While
                    End Using
                End Using

                Using SQLcommandTVSeason As SQLite.SQLiteCommand = SQLcn.CreateCommand
                    SQLcommandTVSeason.CommandText = String.Concat("SELECT PosterPath FROM TVSeason WHERE TVShowID = ", _TVDB.ShowID, " AND TVEpID = ", EpID, ";")
                    Using SQLReader As SQLite.SQLiteDataReader = SQLcommandTVSeason.ExecuteReader
                        If SQLReader.HasRows Then
                            If Not DBNull.Value.Equals(SQLReader("PosterPath")) Then _TVDB.SeasonPosterPath = SQLReader("PosterPath").ToString
                        End If
                    End Using
                End Using
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            _TVDB.EpID = -1
        End Try
        Return _TVDB
    End Function

    ''' <summary>
    ''' Load all the information for a TV Episode (by episode path)
    ''' </summary>
    ''' <param name="sPath">Full path to the episode file</param>
    ''' <returns>Master.DBTV object</returns>
    Public Function LoadTVEpFromDB(ByVal sPath As String) As Master.DBTV
        Try
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                ' One more Query Better then re-write all function again
                SQLcommand.CommandText = String.Concat("SELECT ID FROM TVEps WHERE TVEpPath = ", sPath, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    If SQLreader.Read Then
                        Return LoadTVEpFromDB(Convert.ToInt64(SQLreader("ID")))
                    Else
                        Return New Master.DBTV With {.EpID = -1} ' No Movie Found
                    End If
                End Using
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return New Master.DBTV With {.EpID = -1}
    End Function

    ''' <summary>
    ''' Saves all episode information from a Master.DBTV object to the database
    ''' </summary>
    ''' <param name="_TVEpDB">Master.DBTV object to save to the database</param>
    ''' <param name="IsNew">Is this a new episode (not already present in database)?</param>
    ''' <param name="BatchMode">Is the function already part of a transaction?</param>
    ''' <param name="ToNfo">Save the information to an nfo file?</param>
    Public Sub SaveTVEpToDB(ByVal _TVEpDB As Master.DBTV, ByVal IsNew As Boolean, Optional ByVal BatchMode As Boolean = False, Optional ByVal ToNfo As Boolean = False)

        Try
            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing

            If Not BatchMode Then SQLtransaction = SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                If IsNew Then
                    SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO TVEps (", _
                        "TVShowID, TVEpPath, HasPoster, HasNfo, New, Mark, Source, Lock, Title, Season, Episode,", _
                        "Rating, Plot, Aired, Director, Credits, PosterPath, NfoPath, NeedsSave", _
                        ") VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); SELECT LAST_INSERT_ROWID() FROM TVEps;")
                Else
                    SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO TVEps (", _
                        "ID, TVShowID, TVEpPath, HasPoster, HasNfo, New, Mark, Source, Lock, Title, Season, Episode,", _
                        "Rating, Plot, Aired, Director, Credits, PosterPath, NfoPath, NeedsSave", _
                        ") VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); SELECT LAST_INSERT_ROWID() FROM TVEps;")
                    Dim parTVEpID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTVEpID", DbType.UInt64, 0, "ID")
                    parTVEpID.Value = _TVEpDB.EpID
                End If

                Dim parTVShowID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTVShowID", DbType.UInt64, 0, "TVShowID")
                Dim parTVEpPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTVEpPath", DbType.String, 0, "TVEpPath")
                Dim parHasPoster As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasPoster", DbType.Boolean, 0, "HasPoster")
                Dim parHasNfo As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasInfo", DbType.Boolean, 0, "HasNfo")
                Dim parNew As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNew", DbType.Boolean, 0, "new")
                Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")

                Dim parTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTitle", DbType.String, 0, "Title")
                Dim parSeason As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSeason", DbType.String, 0, "Season")
                Dim parEpisode As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parEpisode", DbType.String, 0, "Episode")
                Dim parRating As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parRating", DbType.String, 0, "Rating")
                Dim parPlot As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPlot", DbType.String, 0, "Plot")
                Dim parAired As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parAired", DbType.String, 0, "Aired")
                Dim parDirector As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parDirector", DbType.String, 0, "Director")
                Dim parCredits As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parCredits", DbType.String, 0, "Credits")
                Dim parPosterPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPosterPath", DbType.String, 0, "PosterPath")
                Dim parNfoPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNfoPath", DbType.String, 0, "NfoPath")
                Dim parNeedsSave As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNeedsSave", DbType.Boolean, 0, "NeedsSave")

                ' First let's save it to NFO, even because we will need the NFO path
                If ToNfo Then NFO.SaveTVEpToNFO(_TVEpDB)

                parTVShowID.Value = _TVEpDB.ShowID
                parTVEpPath.Value = _TVEpDB.Filename
                parPosterPath.Value = _TVEpDB.EpPosterPath
                parNfoPath.Value = _TVEpDB.EpNfoPath
                parHasPoster.Value = Not String.IsNullOrEmpty(_TVEpDB.EpPosterPath)
                parHasNfo.Value = Not String.IsNullOrEmpty(_TVEpDB.EpNfoPath)

                parNew.Value = _TVEpDB.IsNewEp
                parMark.Value = _TVEpDB.IsMarkEp
                parLock.Value = _TVEpDB.IsLockEp
                parSource.Value = _TVEpDB.Source
                parNeedsSave.Value = _TVEpDB.EpNeedsSave

                With _TVEpDB.TVEp
                    parTitle.Value = .Title
                    parSeason.Value = .Season
                    parEpisode.Value = .Episode
                    parRating.Value = .Rating
                    parPlot.Value = .Plot
                    parAired.Value = .Aired
                    parDirector.Value = .Director
                    parCredits.Value = .Credits
                End With

                If IsNew Then
                    If Master.eSettings.MarkNew Then
                        parMark.Value = True
                    Else
                        parMark.Value = False
                    End If
                    Using rdrTVEp As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                        If rdrTVEp.Read Then
                            _TVEpDB.EpID = Convert.ToInt64(rdrTVEp(0))
                        Else
                            Master.eLog.WriteToErrorLog("Something very wrong here: SaveTVEpToDB", _TVEpDB.ToString, "Error")
                            _TVEpDB.EpID = -1
                            Exit Sub
                        End If
                    End Using
                Else
                    SQLcommand.ExecuteNonQuery()
                End If

                If Not _TVEpDB.EpID = -1 Then
                    Using SQLcommandActor As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandActor.CommandText = String.Concat("INSERT OR REPLACE INTO Actors (Name,thumb) VALUES (?,?)")
                        Dim parActorName As SQLite.SQLiteParameter = SQLcommandActor.Parameters.Add("parActorName", DbType.String, 0, "Name")
                        Dim parActorThumb As SQLite.SQLiteParameter = SQLcommandActor.Parameters.Add("parActorThumb", DbType.String, 0, "thumb")
                        For Each actor As Media.Person In _TVEpDB.TVEp.Actors
                            parActorName.Value = actor.Name
                            parActorThumb.Value = actor.Thumb
                            SQLcommandActor.ExecuteNonQuery()
                            Using SQLcommandTVEpActors As SQLite.SQLiteCommand = SQLcn.CreateCommand
                                SQLcommandTVEpActors.CommandText = String.Concat("INSERT OR REPLACE INTO TVEpActors (TVEpID,ActorName,Role) VALUES (?,?,?);")
                                Dim parTVEpActorsEpID As SQLite.SQLiteParameter = SQLcommandTVEpActors.Parameters.Add("parTVEpActorsEpID", DbType.UInt64, 0, "TVEpID")
                                Dim parTVEpActorsActorName As SQLite.SQLiteParameter = SQLcommandTVEpActors.Parameters.Add("parTVEpActorsActorName", DbType.String, 0, "ActorNAme")
                                Dim parTVEpActorsActorRole As SQLite.SQLiteParameter = SQLcommandTVEpActors.Parameters.Add("parTVEpActorsActorRole", DbType.String, 0, "Role")
                                parTVEpActorsEpID.Value = _TVEpDB.EpID
                                parTVEpActorsActorName.Value = actor.Name
                                parTVEpActorsActorRole.Value = actor.Role
                                SQLcommandTVEpActors.ExecuteNonQuery()
                            End Using
                        Next
                    End Using
                    Using SQLcommandTVVStreams As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandTVVStreams.CommandText = String.Concat("INSERT OR REPLACE INTO TVVStreams (", _
                                "TVEpID, StreamID, Video_Width,Video_Height,Video_Codec,Video_Duration,", _
                                "Video_ScanType, Video_AspectDisplayRatio, Video_Language, Video_LongLanguage", _
                                ") VALUES (?,?,?,?,?,?,?,?,?,?);")
                        Dim parVideo_EpID As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_EpID", DbType.UInt64, 0, "TVEpID")
                        Dim parVideo_StreamID As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_StreamID", DbType.UInt64, 0, "StreamID")
                        Dim parVideo_Width As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_Width", DbType.String, 0, "Video_Width")
                        Dim parVideo_Height As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_Height", DbType.String, 0, "Video_Height")
                        Dim parVideo_Codec As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_Codec", DbType.String, 0, "Video_Codec")
                        Dim parVideo_Duration As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_Duration", DbType.String, 0, "Video_Duration")
                        Dim parVideo_ScanType As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_ScanType", DbType.String, 0, "Video_ScanType")
                        Dim parVideo_AspectDisplayRatio As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_AspectDisplayRatio", DbType.String, 0, "Video_AspectDisplayRatio")
                        Dim parVideo_Language As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_Language", DbType.String, 0, "Video_Language")
                        Dim parVideo_LongLanguage As SQLite.SQLiteParameter = SQLcommandTVVStreams.Parameters.Add("parVideo_LongLanguage", DbType.String, 0, "Video_LongLanguage")
                        For i As Integer = 0 To _TVEpDB.TVEp.FileInfo.StreamDetails.Video.Count - 1
                            parVideo_EpID.Value = _TVEpDB.EpID
                            parVideo_StreamID.Value = i
                            parVideo_Width.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).Width
                            parVideo_Height.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).Height
                            parVideo_Codec.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).Codec
                            parVideo_Duration.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).Duration
                            parVideo_ScanType.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).Scantype
                            parVideo_AspectDisplayRatio.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).Aspect
                            parVideo_Language.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).Language
                            parVideo_LongLanguage.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Video(i).LongLanguage
                            SQLcommandTVVStreams.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandTVAStreams As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandTVAStreams.CommandText = String.Concat("INSERT OR REPLACE INTO TVAStreams (", _
                                "TVEpID, StreamID, Audio_Language, Audio_LongLanguage, Audio_Codec, Audio_Channel", _
                                ") VALUES (?,?,?,?,?,?);")
                        Dim parAudio_EpID As SQLite.SQLiteParameter = SQLcommandTVAStreams.Parameters.Add("parAudio_EpID", DbType.UInt64, 0, "TVEpID")
                        Dim parAudio_StreamID As SQLite.SQLiteParameter = SQLcommandTVAStreams.Parameters.Add("parAudio_StreamID", DbType.UInt64, 0, "StreamID")
                        Dim parAudio_Language As SQLite.SQLiteParameter = SQLcommandTVAStreams.Parameters.Add("parAudio_Language", DbType.String, 0, "Audio_Language")
                        Dim parAudio_LongLanguage As SQLite.SQLiteParameter = SQLcommandTVAStreams.Parameters.Add("parAudio_LongLanguage", DbType.String, 0, "Audio_LongLanguage")
                        Dim parAudio_Codec As SQLite.SQLiteParameter = SQLcommandTVAStreams.Parameters.Add("parAudio_Codec", DbType.String, 0, "Audio_Codec")
                        Dim parAudio_Channel As SQLite.SQLiteParameter = SQLcommandTVAStreams.Parameters.Add("parAudio_Channel", DbType.String, 0, "Audio_Channel")
                        For i As Integer = 0 To _TVEpDB.TVEp.FileInfo.StreamDetails.Audio.Count - 1
                            parAudio_EpID.Value = _TVEpDB.EpID
                            parAudio_StreamID.Value = i
                            parAudio_Language.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Audio(i).Language
                            parAudio_LongLanguage.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Audio(i).LongLanguage
                            parAudio_Codec.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Audio(i).Codec
                            parAudio_Channel.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Audio(i).Channels
                            SQLcommandTVAStreams.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandTVSubs As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandTVSubs.CommandText = String.Concat("INSERT OR REPLACE INTO TVSubs (", _
                                "TVEpID, StreamID, Subs_Language, Subs_LongLanguage", _
                                ") VALUES (?,?,?,?);")
                        Dim parSubs_EpID As SQLite.SQLiteParameter = SQLcommandTVSubs.Parameters.Add("parSubs_EpID", DbType.UInt64, 0, "TVEpID")
                        Dim parSubs_StreamID As SQLite.SQLiteParameter = SQLcommandTVSubs.Parameters.Add("parSubs_StreamID", DbType.UInt64, 0, "StreamID")
                        Dim parSubs_Language As SQLite.SQLiteParameter = SQLcommandTVSubs.Parameters.Add("parSubs_Language", DbType.String, 0, "Subs_Language")
                        Dim parSubs_LongLanguage As SQLite.SQLiteParameter = SQLcommandTVSubs.Parameters.Add("parSubs_LongLanguage", DbType.String, 0, "Subs_LongLanguage")
                        For i As Integer = 0 To _TVEpDB.TVEp.FileInfo.StreamDetails.Subtitle.Count - 1
                            parSubs_EpID.Value = _TVEpDB.EpID
                            parSubs_StreamID.Value = i
                            parSubs_Language.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Subtitle(i).Language
                            parSubs_LongLanguage.Value = _TVEpDB.TVEp.FileInfo.StreamDetails.Subtitle(i).LongLanguage
                            SQLcommandTVSubs.ExecuteNonQuery()
                        Next
                    End Using
                    Using SQLcommandTVSeason As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandTVSeason.CommandText = String.Concat("INSERT OR REPLACE INTO TVSeason (", _
                                "TVShowID, TVEpID, SeasonText, Season, HasPoster, PosterPath", _
                                ") VALUES (?,?,?,?,?,?);")
                        Dim parSeasonShowID As SQLite.SQLiteParameter = SQLcommandTVSeason.Parameters.Add("parSeasonShowID", DbType.UInt64, 0, "TVShowID")
                        Dim parSeasonEpID As SQLite.SQLiteParameter = SQLcommandTVSeason.Parameters.Add("parSeasonEpID", DbType.UInt64, 0, "TVEpID")
                        Dim parSeasonSeasonText As SQLite.SQLiteParameter = SQLcommandTVSeason.Parameters.Add("parSeasonSeasonText", DbType.String, 0, "SeasonText")
                        Dim parSeasonSeason As SQLite.SQLiteParameter = SQLcommandTVSeason.Parameters.Add("parSeasonSeason", DbType.Int32, 0, "Season")
                        Dim parSeasonHasPoster As SQLite.SQLiteParameter = SQLcommandTVSeason.Parameters.Add("parSeasonHasPoster", DbType.Boolean, 0, "HasPoster")
                        Dim parSeasonPosterPath As SQLite.SQLiteParameter = SQLcommandTVSeason.Parameters.Add("parSeasonPosterPath", DbType.String, 0, "PosterPath")
                        parSeasonShowID.Value = _TVEpDB.ShowID
                        parSeasonEpID.Value = _TVEpDB.EpID
                        parSeasonSeasonText.Value = StringManip.FormatSeasonText(_TVEpDB.TVEp.Season)
                        parSeasonSeason.Value = _TVEpDB.TVEp.Season
                        parSeasonHasPoster.Value = Not String.IsNullOrEmpty(_TVEpDB.SeasonPosterPath)
                        parSeasonPosterPath.Value = _TVEpDB.SeasonPosterPath
                        SQLcommandTVSeason.ExecuteNonQuery()
                    End Using
                End If
            End Using
            If Not BatchMode Then SQLtransaction.Commit()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Saves all show information from a Master.DBTV object to the database
    ''' </summary>
    ''' <param name="_TVShowDB">Master.DBTV object to save to the database</param>
    ''' <param name="IsNew">Is this a new show (not already present in database)?</param>
    ''' <param name="BatchMode">Is the function already part of a transaction?</param>
    ''' <param name="ToNfo">Save the information to an nfo file?</param>
    Public Sub SaveTVShowToDB(ByRef _TVShowDB As Master.DBTV, ByVal IsNew As Boolean, Optional ByVal BatchMode As Boolean = False, Optional ByVal ToNfo As Boolean = False)

        Try
            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing

            If Not BatchMode Then SQLtransaction = SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                If IsNew Then
                    SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO TVShows (", _
                        "TVShowPath, HasPoster, HasFanart, HasNfo, New, Mark, Source, TVDB, Lock, Title,", _
                        "EpisodeGuide, Plot, Genre, Premiered, Studio, MPAA, PosterPath, FanartPath, NfoPath, NeedsSave", _
                        ") VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); SELECT LAST_INSERT_ROWID() FROM TVShows;")
                Else
                    SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO TVEps (", _
                        "ID, TVShowPath, HasPoster, HasFanart, HasNfo, New, Mark, Source, TVDB, Lock, Title,", _
                        "EpisodeGuide, Plot, Genre, Premiered, Studio, MPAA, PosterPath, FanartPath, NfoPath, NeedsSave", _
                        ") VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?); SELECT LAST_INSERT_ROWID() FROM TVShows;")
                    Dim parTVShowID As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTVShowID", DbType.UInt64, 0, "ID")
                    parTVShowID.Value = _TVShowDB.ShowID
                End If

                Dim parTVShowPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTVShowPath", DbType.String, 0, "TVShowPath")
                Dim parHasPoster As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasFanart", DbType.Boolean, 0, "HasFanart")
                Dim parHasFanart As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasPoster", DbType.Boolean, 0, "HasPoster")
                Dim parHasNfo As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parHasInfo", DbType.Boolean, 0, "HasNfo")
                Dim parNew As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNew", DbType.Boolean, 0, "new")
                Dim parMark As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMark", DbType.Boolean, 0, "mark")
                Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                Dim parTVDB As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTVDB", DbType.String, 0, "TVDB")
                Dim parLock As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parLock", DbType.Boolean, 0, "lock")
                Dim parTitle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parTitle", DbType.String, 0, "Title")
                Dim parEpisodeGuide As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parEpisodeGuide", DbType.String, 0, "EpisodeGuide")
                Dim parPlot As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPlot", DbType.String, 0, "Plot")
                Dim parGenre As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parGenre", DbType.String, 0, "Genre")
                Dim parPremiered As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPremiered", DbType.String, 0, "Premiered")
                Dim parStudio As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parStudio", DbType.String, 0, "Studio")
                Dim parMPAA As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parMPAA", DbType.String, 0, "MPAA")
                Dim parPosterPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPosterPath", DbType.String, 0, "PosterPath")
                Dim parFanartPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFanartPath", DbType.String, 0, "FanartPath")
                Dim parNfoPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNfoPath", DbType.String, 0, "NfoPath")
                Dim parNeedsSave As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNeedsSave", DbType.Boolean, 0, "NeedsSave")

                ' First let's save it to NFO, even because we will need the NFO path
                If ToNfo Then NFO.SaveTVShowToNFO(_TVShowDB)

                parTVShowPath.Value = _TVShowDB.ShowPath
                parPosterPath.Value = _TVShowDB.ShowPosterPath
                parFanartPath.Value = _TVShowDB.ShowFanartPath
                parNfoPath.Value = _TVShowDB.ShowNfoPath
                parHasPoster.Value = Not String.IsNullOrEmpty(_TVShowDB.ShowPosterPath)
                parHasFanart.Value = Not String.IsNullOrEmpty(_TVShowDB.ShowFanartPath)
                parHasNfo.Value = Not String.IsNullOrEmpty(_TVShowDB.ShowNfoPath)

                parNew.Value = _TVShowDB.IsNewShow
                parMark.Value = _TVShowDB.IsMarkShow
                parLock.Value = _TVShowDB.IsLockShow
                parSource.Value = _TVShowDB.Source
                parNeedsSave.Value = _TVShowDB.EpNeedsSave

                With _TVShowDB.TVShow
                    parTVDB.Value = .ID
                    parTitle.Value = .Title
                    parEpisodeGuide.Value = .EpisodeGuideURL
                    parPlot.Value = .Plot
                    parGenre.Value = .Genre
                    parPremiered.Value = .Premiered
                    parStudio.Value = .Studio
                    parMPAA.Value = .MPAA
                End With

                If IsNew Then
                    If Master.eSettings.MarkNew Then
                        parMark.Value = True
                    Else
                        parMark.Value = False
                    End If
                    Using rdrTVShow As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                        If rdrTVShow.Read Then
                            _TVShowDB.ShowID = Convert.ToInt64(rdrTVShow(0))
                        Else
                            Master.eLog.WriteToErrorLog("Something very wrong here: SaveTVShowToDB", _TVShowDB.ToString, "Error")
                            _TVShowDB.ShowID = -1
                            Exit Sub
                        End If
                    End Using
                Else
                    SQLcommand.ExecuteNonQuery()
                End If

                If Not _TVShowDB.ShowID = -1 Then
                    Using SQLcommandActor As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommandActor.CommandText = String.Concat("INSERT OR REPLACE INTO Actors (Name,thumb) VALUES (?,?)")
                        Dim parActorName As SQLite.SQLiteParameter = SQLcommandActor.Parameters.Add("parActorName", DbType.String, 0, "Name")
                        Dim parActorThumb As SQLite.SQLiteParameter = SQLcommandActor.Parameters.Add("parActorThumb", DbType.String, 0, "thumb")
                        For Each actor As Media.Person In _TVShowDB.TVShow.Actors
                            parActorName.Value = actor.Name
                            parActorThumb.Value = actor.Thumb
                            SQLcommandActor.ExecuteNonQuery()
                            Using SQLcommandTVShowActors As SQLite.SQLiteCommand = SQLcn.CreateCommand
                                SQLcommandTVShowActors.CommandText = String.Concat("INSERT OR REPLACE INTO TVShowActors (TVShowID,ActorName,Role) VALUES (?,?,?);")
                                Dim parTVShowActorsShowID As SQLite.SQLiteParameter = SQLcommandTVShowActors.Parameters.Add("parTVShowActorsEpID", DbType.UInt64, 0, "TVShowID")
                                Dim parTVShowActorsActorName As SQLite.SQLiteParameter = SQLcommandTVShowActors.Parameters.Add("parTVShowActorsActorName", DbType.String, 0, "ActorNAme")
                                Dim parTVShowActorsActorRole As SQLite.SQLiteParameter = SQLcommandTVShowActors.Parameters.Add("parTVShowActorsActorRole", DbType.String, 0, "Role")
                                parTVShowActorsShowID.Value = _TVShowDB.ShowID
                                parTVShowActorsActorName.Value = actor.Name
                                parTVShowActorsActorRole.Value = actor.Role
                                SQLcommandTVShowActors.ExecuteNonQuery()
                            End Using
                        Next
                    End Using
                End If
            End Using
            If Not BatchMode Then SQLtransaction.Commit()

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Remove all information related to a movie from the database.
    ''' </summary>
    ''' <param name="ID">ID of the movie to remove, as stored in the database.</param>
    ''' <param name="BatchMode">Is this function already part of a transaction?</param>
    ''' <returns>True if successful, false if deletion failed.</returns>
    Public Function DeleteFromDB(ByVal ID As Long, Optional ByVal BatchMode As Boolean = False) As Boolean
        Try
            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing
            If Not BatchMode Then SQLtransaction = SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("DELETE FROM movies WHERE id = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM MoviesAStreams WHERE MovieID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM MoviesVStreams WHERE MovieID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM MoviesActors WHERE MovieID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM MoviesSubs WHERE MovieID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM MoviesPosters WHERE MovieID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM MoviesFanart WHERE MovieID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM MoviesSets WHERE MovieID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
            End Using
            If Not BatchMode Then SQLtransaction.Commit()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Iterates db entries to check if the paths to the movie files are valid. If not, remove all entries pertaining to the movie.
    ''' </summary>
    Public Sub Clean()
        Try
            Using SQLtransaction As SQLite.SQLiteTransaction = SQLcn.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                    SQLcommand.CommandText = "SELECT MoviePath, Id FROM movies ORDER BY ListTitle COLLATE NOCASE;"
                    Using SQLReader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                        While SQLReader.Read
                            If Not File.Exists(SQLReader("MoviePath").ToString) Then
                                Me.DeleteFromDB(Convert.ToInt64(SQLReader("ID")), True)
                            End If
                        End While
                    End Using
                End Using
                SQLtransaction.Commit()
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Remove all information related to a TV show from the database.
    ''' </summary>
    ''' <param name="ID">ID of the tvshow to remove, as stored in the database.</param>
    ''' <param name="BatchMode">Is this function already part of a transaction?</param>
    ''' <returns>True if successful, false if deletion failed.</returns>
    Public Function DeleteTVShowFromDB(ByVal ID As Long, Optional ByVal BatchMode As Boolean = False) As Boolean
        Try
            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing
            If Not BatchMode Then SQLtransaction = SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT ID FROM TVEps WHERE TVShowID = ", ID, ";")
                Using SQLReader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    While SQLReader.Read
                        Master.DB.DeleteTVEpFromDB(Convert.ToInt64(SQLReader("ID")), True)
                    End While
                End Using
                SQLcommand.CommandText = String.Concat("DELETE FROM TVShows WHERE ID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM TVShowActors WHERE TVShowID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM TVSeason WHERE TVShowID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
            End Using
            If Not BatchMode Then SQLtransaction.Commit()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    ''' <summary>
    ''' Remove all information related to a TV episode from the database.
    ''' </summary>
    ''' <param name="ID">ID of the episode to remove, as stored in the database.</param>
    ''' <param name="BatchMode">Is this function already part of a transaction?</param>
    ''' <returns>True if successful, false if deletion failed.</returns>
    Public Function DeleteTVEpFromDB(ByVal ID As Long, Optional ByVal BatchMode As Boolean = False) As Boolean
        Try
            Dim SQLtransaction As SQLite.SQLiteTransaction = Nothing
            If Not BatchMode Then SQLtransaction = SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("DELETE FROM TVEps WHERE ID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM TVEpActors WHERE TVEpID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM TVVStreams WHERE TVEpID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM TVAStreams WHERE TVEpID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
                SQLcommand.CommandText = String.Concat("DELETE FROM TVSubs WHERE TVEpID = ", ID, ";")
                SQLcommand.ExecuteNonQuery()
            End Using
            If Not BatchMode Then SQLtransaction.Commit()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Sub SaveMovieList()
        Try
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                    SQLcommand.CommandText = "UPDATE movies SET new = (?);"
                    Dim parNew As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parNew", DbType.Boolean, 0, "new")
                    parNew.Value = False
                    SQLcommand.ExecuteNonQuery()
                End Using
                SQLtransaction.Commit()
            End Using
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "VACUUM;"
                SQLcommand.ExecuteNonQuery()
            End Using

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
End Class
