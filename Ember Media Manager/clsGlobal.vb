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
Imports System.Reflection
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Public Class Master

    'Global Variables
    Public Shared eSettings As New emmSettings
    Public Shared eLang As New Localization
    'Public Shared MediaList As New List(Of FileAndSource)
    Public Shared eLog As New ErrorLogger
    Public Shared DefaultOptions As New ScrapeOptions
    Public Shared GlobalScrapeMod As New ScrapeModifier
    Public Shared DB As New Database
    Public Shared TempPath As String = Path.Combine(AppPath, "Temp")
    Public Shared currMovie As New DBMovie
    Public Shared currShow As New DBTV
    Public Shared CanScanDiscImage As Boolean
    Public Shared SourcesList As New List(Of String)
    Public Shared tmpMovie As New Media.Movie
    Public Shared tmpTVDBShow As New TVDB.TVDBShow
    Public Shared tmpTVImages As New TVImages

    'Global Enums
    Public Enum PosterSize As Integer
        Xlrg = 0
        Lrg = 1
        Mid = 2
        Small = 3
        Wide = 4
    End Enum

    Public Enum FanartSize As Integer
        Lrg = 0
        Mid = 1
        Small = 2
    End Enum

    Public Enum ShowPosterType As Integer
        None = 0
        Blank = 1
        Graphical = 2
        Text = 3
    End Enum

    Public Enum SeasonPosterType As Integer
        None = 0
        Poster = 1
        Wide = 2
    End Enum

    Public Enum ScrapeType As Integer
        SingleScrape = 0
        FullAuto = 1
        FullAsk = 2
        UpdateAuto = 3
        UpdateAsk = 4
        CleanFolders = 6
        NewAuto = 7
        NewAsk = 8
        MarkAuto = 9
        MarkAsk = 10
        FilterAuto = 11
        FilterAsk = 12
        CopyBD = 13
        RevertStudios = 14
    End Enum

    Public Enum ImageType As Integer
        Posters = 0
        Fanart = 1
    End Enum

    Public Enum TrailerPages As Integer
        AllHTPC = 0
        TMDB = 1
        IMDB = 2
    End Enum

    Public Enum TrailerQuality As Integer
        HD1080p = 0
        HD720p = 1
        Standard = 99
    End Enum

    Public Enum ModType As Integer
        NFO = 0
        Poster = 1
        Fanart = 2
        Extra = 3
        Trailer = 4
        Meta = 5
        All = 6
    End Enum

    Public Structure Scans
        Dim Movies As Boolean
        Dim TV As Boolean
    End Structure

    Public Structure ScrapeModifier
        Dim NFO As Boolean
        Dim Poster As Boolean
        Dim Fanart As Boolean
        Dim Extra As Boolean
        Dim Trailer As Boolean
        Dim Meta As Boolean
    End Structure

    Public Structure DBMovie
        Dim ID As Long
        Dim ListTitle As String
        Dim Movie As Media.Movie
        Dim IsNew As Boolean
        Dim IsMark As Boolean
        Dim IsLock As Boolean
        Dim NeedsSave As Boolean
        Dim UseFolder As Boolean
        Dim Filename As String
        Dim isSingle As Boolean
        Dim PosterPath As String
        Dim FanartPath As String
        Dim NfoPath As String
        Dim TrailerPath As String
        Dim SubPath As String
        Dim ExtraPath As String
        Dim Source As String
        Dim OutOfTolerance As Boolean
        Dim ClearExtras As Boolean
        Dim FileSource As String
    End Structure

    Public Structure DBTV
        Dim ShowID As Long
        Dim EpID As Long
        Dim TVShow As Media.TVShow
        Dim TVEp As Media.EpisodeDetails
        Dim IsNewShow As Boolean
        Dim IsMarkShow As Boolean
        Dim IsLockShow As Boolean
        Dim IsNewEp As Boolean
        Dim IsMarkEp As Boolean
        Dim IsLockEp As Boolean
        Dim ShowNeedsSave As Boolean
        Dim EpNeedsSave As Boolean
        Dim Filename As String
        Dim ShowPosterPath As String
        Dim ShowFanartPath As String
        Dim ShowNfoPath As String
        Dim EpPosterPath As String
        Dim EpFanartPath As String
        Dim EpNfoPath As String
        Dim Source As String
        Dim ShowPath As String
        Dim SeasonPosterPath As String
        Dim SeasonFanartPath As String
    End Structure

    Public Structure TVImages
        Dim ShowPoster As TVDB.TVDBShowPoster
        Dim ShowFanart As TVDB.TVDBFanart
        Dim SeasonImageList As List(Of Images.SeasonImage)
    End Structure

    Public Structure ScrapeOptions
        Dim bTitle As Boolean
        Dim bYear As Boolean
        Dim bMPAA As Boolean
        Dim bRelease As Boolean
        Dim bRating As Boolean
        Dim bTrailer As Boolean
        Dim bVotes As Boolean
        Dim bCast As Boolean
        Dim bTagline As Boolean
        Dim bDirector As Boolean
        Dim bGenre As Boolean
        Dim bOutline As Boolean
        Dim bPlot As Boolean
        Dim bRuntime As Boolean
        Dim bStudio As Boolean
        Dim bWriters As Boolean
        Dim bProducers As Boolean
        Dim bMusicBy As Boolean
        Dim bOtherCrew As Boolean
        Dim bTop250 As Boolean
    End Structure

    Public Structure CustomUpdaterStruct
        Dim Canceled As Boolean
        Dim ScrapeType As ScrapeType
        Dim Options As ScrapeOptions
    End Structure

    Public Structure SettingsResult
        Dim NeedsUpdate As Boolean
        Dim NeedsRefresh As Boolean
        Dim DidCancel As Boolean
    End Structure

    Public Class ImgResult
        Dim _imagepath As String
        Dim _posters As New List(Of String)
        Dim _fanart As New Media.Fanart

        Public Property ImagePath() As String
            Get
                Return _imagepath
            End Get
            Set(ByVal value As String)
                _imagepath = value
            End Set
        End Property

        Public Property Posters() As List(Of String)
            Get
                Return _posters
            End Get
            Set(ByVal value As List(Of String))
                _posters = value
            End Set
        End Property

        Public Property Fanart() As Media.Fanart
            Get
                Return _fanart
            End Get
            Set(ByVal value As Media.Fanart)
                _fanart = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            _imagepath = String.Empty
            _posters.Clear()
            _fanart.Clear()
        End Sub
    End Class

    Public Shared Sub CreateDefaultOptions()
        With DefaultOptions
            .bCast = eSettings.FieldCast
            .bDirector = eSettings.FieldDirector
            .bGenre = eSettings.FieldGenre
            .bMPAA = eSettings.FieldMPAA
            .bMusicBy = eSettings.FieldMusic
            .bOtherCrew = eSettings.FieldCrew
            .bOutline = eSettings.FieldOutline
            .bPlot = eSettings.FieldPlot
            .bProducers = eSettings.FieldProducers
            .bRating = eSettings.FieldRating
            .bRelease = eSettings.FieldRelease
            .bRuntime = eSettings.FieldRuntime
            .bStudio = eSettings.FieldStudio
            .bTagline = eSettings.FieldTagline
            .bTitle = eSettings.FieldTitle
            .bTrailer = eSettings.FieldTrailer
            .bVotes = eSettings.FieldVotes
            .bWriters = eSettings.FieldWriters
            .bYear = eSettings.FieldYear
            .bTop250 = eSettings.Field250
        End With
    End Sub

    Public Shared Sub SetScraperMod(ByVal MType As ModType, ByVal MValue As Boolean, Optional ByVal DoClear As Boolean = True)
        With GlobalScrapeMod
            If DoClear Then
                .Extra = False
                .Fanart = False
                .Meta = False
                .NFO = False
                .Poster = False
                .Trailer = False
            End If

            Select Case MType
                Case ModType.All
                    .Extra = MValue
                    .Fanart = MValue
                    .Meta = MValue
                    .NFO = MValue
                    .Poster = MValue
                    .Trailer = If(eSettings.UpdaterTrailers, MValue, False)
                Case ModType.Extra
                    .Extra = MValue
                Case ModType.Fanart
                    .Fanart = MValue
                Case ModType.Meta
                    .Meta = MValue
                Case ModType.NFO
                    .NFO = MValue
                Case ModType.Poster
                    .Poster = MValue
                Case ModType.Trailer
                    .Trailer = MValue
            End Select

        End With
    End Sub

    Public Shared Function HasModifier() As Boolean
        With GlobalScrapeMod
            If .Extra OrElse .Fanart OrElse .Meta OrElse .NFO OrElse .Poster OrElse .Trailer Then Return True
        End With

        Return False
    End Function

    ''' <summary>
    ''' Get the number of the last sequential extrathumb to make sure we're not overwriting current ones.
    ''' </summary>
    ''' <param name="sPath">Full path to extrathumbs directory</param>
    ''' <returns>Last detected number of the discovered extrathumbs.</returns>
    Public Shared Function GetExtraModifier(ByVal sPath As String) As Integer

        Dim iMod As Integer = 0
        Dim lThumbs As New List(Of String)

        Try
            If Directory.Exists(sPath) Then

                Try
                    lThumbs.AddRange(Directory.GetFiles(sPath, "thumb*.jpg"))
                Catch
                End Try

                If lThumbs.Count > 0 Then
                    iMod = Convert.ToInt32(Regex.Match(lThumbs.Item(lThumbs.Count - 1).ToString, "(\d+).jpg").Groups(1).ToString)
                End If
            End If

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return iMod
    End Function

    ''' <summary>
    ''' Convert a numerical string to single (internationally friendly method)
    ''' </summary>
    ''' <param name="sNumber">Number (as string) to convert</param>
    ''' <returns>Number as single</returns>
    Public Shared Function ConvertToSingle(ByVal sNumber As String) As Single
        Try
            If String.IsNullOrEmpty(sNumber) OrElse sNumber = "0" Then Return 0
            Dim numFormat As NumberFormatInfo = New NumberFormatInfo()
            numFormat.NumberDecimalSeparator = "."
            Return Single.Parse(sNumber.Replace(",", "."), NumberStyles.AllowDecimalPoint, numFormat)
        Catch
        End Try
        Return 0
    End Function

    ''' <summary>
    ''' Begin the process to extract extrathumbs
    ''' </summary>
    ''' <param name="mMovie">DBMovie object (for paths)</param>
    ''' <param name="ThumbCount">How many thumbs to extract</param>
    ''' <param name="isEdit"></param>
    ''' <returns>Fanart path if an extrathumb was set as fanart.</returns>
    Public Shared Function CreateRandomThumbs(ByVal mMovie As DBMovie, ByVal ThumbCount As Integer, ByVal isEdit As Boolean) As String
        Dim tThumb As New ThumbGenerator

        tThumb.Movie = mMovie
        tThumb.ThumbCount = ThumbCount
        tThumb.isEdit = isEdit

        tThumb.Start()

        Return tThumb.SetFA
    End Function

    ''' <summary>
    ''' Check for the lastest version of Ember
    ''' </summary>
    ''' <returns>Latest version as integer</returns>
    Public Shared Function CheckUpdate() As Integer
        Try
            Dim sHTTP As New HTTP
            Dim updateXML As String = sHTTP.DownloadData("http://www.embermm.com/Updates/Update.xml")
            sHTTP = Nothing

            If updateXML.Length > 0 Then
                Dim xmlUpdate As XDocument
                Try
                    xmlUpdate = XDocument.Parse(updateXML)
                Catch
                    Return 0
                End Try

                Dim xUdpate = From xUp In xmlUpdate...<version> Select xUp.@current
                If xUdpate.Count > 0 Then
                    Return Convert.ToInt32(xUdpate(0))
                Else
                    Return 0
                End If
            Else
                Return 0
            End If

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Get the changelog for the latest version
    ''' </summary>
    ''' <returns>Changelog as string</returns>
    Public Shared Function GetChangelog() As String
        Try
            Dim sHTTP As New HTTP
            Dim strChangelog As String = sHTTP.DownloadData("http://www.embermm.com/Updates/Changelog.txt")
            sHTTP = Nothing

            If strChangelog.Length > 0 Then
                Return strChangelog
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return "Unavailable"
    End Function

    ''' <summary>
    ''' Check to make sure user has at least .NET Framework 3.5 installed
    ''' </summary>
    ''' <returns>True if installed version is >= 3.5, false if not.</returns>
    Public Shared Function GetNETVersion() As Boolean
        Try
            Const regLocation As String = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP"
            Dim masterKey As RegistryKey = Registry.LocalMachine.OpenSubKey(regLocation)
            Dim tempKey As RegistryKey
            Dim sVersion As String = String.Empty

            If Not IsNothing(masterKey) Then
                Dim SubKeyNames As String() = masterKey.GetSubKeyNames()
                For i As Integer = 0 To SubKeyNames.Count - 1
                    tempKey = Registry.LocalMachine.OpenSubKey(String.Concat(regLocation, "\\", SubKeyNames(i)))
                    If Not IsNothing(tempKey) Then
                        Try
                            sVersion = tempKey.GetValue("Version").ToString
                        Catch ex As Exception
                            ' GetValue can Raise Exceptions  when some key are Close or Marked for Deletion
                            sVersion = String.Empty 'clear variable
                        End Try
                        If Not String.IsNullOrEmpty(sVersion) Then
                            If ConvertToSingle(sVersion.Substring(0, 3)) >= 3.5 Then Return True
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Constrain a number to the nearest multiple
    ''' </summary>
    ''' <param name="iNumber">Number to quantize</param>
    ''' <param name="iMultiple">Multiple of constraint.</param>
    Public Shared Function Quantize(ByVal iNumber As Integer, ByVal iMultiple As Integer) As Integer
        Return Convert.ToInt32(System.Math.Round(iNumber / iMultiple, 0) * iMultiple)
    End Function

    ''' <summary>
    ''' Force of habit
    ''' </summary>
    ''' <returns>Path of the directory containing the Ember executable</returns>
    Public Shared Function AppPath() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory
    End Function

    ''' <summary>
    ''' Check version of the MediaInfo dll. If newer than 0.7.11 then don't try to scan disc images with it.
    ''' </summary>
    Public Shared Sub TestMediaInfoDLL()
        Try
            'just assume dll is there since we're distributing full package... if it's not, user has bigger problems
            Dim dllPath As String = String.Concat(AppPath, "Bin", Path.DirectorySeparatorChar, "MediaInfo.DLL")
            Dim fVersion As FileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(dllPath)
            If fVersion.FileMinorPart <= 7 AndAlso fVersion.FileBuildPart <= 11 Then
                CanScanDiscImage = True
            Else
                CanScanDiscImage = False
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Convert a list(of T) to a string of separated values
    ''' </summary>
    ''' <param name="source">List(of T)</param>
    ''' <param name="separator">Character or string to use as a value separator</param>
    ''' <returns>String of separated values</returns>
    Public Shared Function ListToStringWithSeparator(Of T)(ByVal source As IList(Of T), ByVal separator As String) As String

        If source Is Nothing Then Throw New ArgumentNullException("Source parameter cannot be nothing")
        If String.IsNullOrEmpty(separator) Then Throw New ArgumentException("Separator parameter cannot be nothing or empty")

        Dim values As String() = source.Cast(Of Object)().Where(Function(n) n IsNot Nothing).Select(Function(s) s.ToString).ToArray

        Return String.Join(separator, values)
    End Function

    ''' <summary>
    ''' Get a list of paths to all sources stored in the database
    ''' </summary>
    Public Shared Sub GetListOfSources()
        SourcesList.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLcommand.CommandText = "SELECT sources.Path FROM sources;"
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    SourcesList.Add(SQLreader("Path").ToString)
                End While
            End Using
        End Using
    End Sub

    Public Shared Sub DGVDoubleBuffer(ByRef cDGV As DataGridView)
        Dim conType As Type = cDGV.GetType
        Dim pi As PropertyInfo = conType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(cDGV, True, Nothing)
    End Sub

    Public Shared Sub PNLDoubleBuffer(ByRef cPNL As Panel)
        Dim conType As Type = cPNL.GetType
        Dim pi As PropertyInfo = conType.GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        pi.SetValue(cPNL, True, Nothing)
    End Sub

    Public Shared Function StringToSize(ByVal sString As String) As Size
        If Regex.IsMatch(sString, "^[0-9]+x[0-9]+$", RegexOptions.IgnoreCase) Then
            Dim SplitSize() As String = Strings.Split(sString, "x")
            Return New Size With {.Width = Convert.ToInt32(SplitSize(0)), .Height = Convert.ToInt32(SplitSize(1))}
        Else
            Return New Size With {.Width = 0, .Height = 0}
        End If
    End Function

    Public Shared Function ReadStreamToEnd(ByVal rStream As Stream) As Byte()
        Dim StreamBuffer(4096) As Byte
        Dim BlockSize As Integer = 0
        Using mStream As MemoryStream = New MemoryStream()
            Do
                BlockSize = rStream.Read(StreamBuffer, 0, StreamBuffer.Length)
                If BlockSize > 0 Then mStream.Write(StreamBuffer, 0, BlockSize)
            Loop While BlockSize > 0
            Return mStream.ToArray
        End Using
    End Function

    Public Shared Function ReadImageStreamToEnd(ByVal rStream As Stream) As Image
        Dim StreamBuffer(4096) As Byte
        Dim BlockSize As Integer = 0
        Using mStream As MemoryStream = New MemoryStream()
            Do
                BlockSize = rStream.Read(StreamBuffer, 0, StreamBuffer.Length)
                If BlockSize > 0 Then mStream.Write(StreamBuffer, 0, BlockSize)
            Loop While BlockSize > 0
            Return Image.FromStream(mStream)
        End Using
    End Function

    Public Shared Sub LoadAllEpisodes(ByVal _ID As Integer)
        Try

            tmpTVDBShow = New TVDB.TVDBShow

            tmpTVDBShow.Show = Master.DB.LoadTVShowFromDB(_ID)

            Using SQLCount As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLCount.CommandText = String.Concat("SELECT COUNT(ID) AS eCount FROM TVEps WHERE TVShowID = ", _ID, ";")
                Using SQLRCount As SQLite.SQLiteDataReader = SQLCount.ExecuteReader
                    If Convert.ToInt32(SQLRCount("eCount")) > 0 Then
                        Using SQLCommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                            SQLCommand.CommandText = String.Concat("SELECT ID FROM TVEps WHERE TVShowID = ", _ID, ";")
                            Using SQLReader As SQLite.SQLiteDataReader = SQLCommand.ExecuteReader
                                While SQLReader.Read
                                    tmpTVDBShow.Episodes.Add(Master.DB.LoadTVEpFromDB(Convert.ToInt64(SQLReader("ID")), True))
                                End While
                            End Using
                        End Using
                    End If
                End Using
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

End Class
