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
Imports System.Xml.Serialization

Public Class Containers
    Public Class TVLanguage
        Private _longlang As String
        Private _shortlang As String

        Public Property LongLang() As String
            Get
                Return Me._longlang
            End Get
            Set(ByVal value As String)
                Me._longlang = value
            End Set
        End Property

        Public Property ShortLang() As String
            Get
                Return Me._shortlang
            End Get
            Set(ByVal value As String)
                Me._shortlang = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._longlang = String.Empty
            Me._shortlang = String.Empty
        End Sub
    End Class

    Public Class ImgResult
        Dim _imagepath As String
        Dim _posters As New List(Of String)
        Dim _fanart As New MediaContainers.Fanart

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

        Public Property Fanart() As MediaContainers.Fanart
            Get
                Return _fanart
            End Get
            Set(ByVal value As MediaContainers.Fanart)
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

    Public Class SettingsPanel
        Dim _name As String
        Dim _text As String
        Dim _imageindex As Integer
        Dim _type As String

        Dim _panel As Panel
        Dim _order As Integer
        Dim _parent As String

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)
                Me._name = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return Me._text
            End Get
            Set(ByVal value As String)
                Me._text = value
            End Set
        End Property

        Public Property ImageIndex() As Integer
            Get
                Return Me._imageindex
            End Get
            Set(ByVal value As Integer)
                Me._imageindex = value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return Me._type
            End Get
            Set(ByVal value As String)
                Me._type = value
            End Set
        End Property
        <XmlIgnore()> _
        Public Property Panel() As Panel
            Get
                Return Me._panel
            End Get
            Set(ByVal value As Panel)
                Me._panel = value
            End Set
        End Property

        Public Property Order() As Integer
            Get
                Return Me._order
            End Get
            Set(ByVal value As Integer)
                Me._order = value
            End Set
        End Property

        Public Property Parent() As String
            Get
                Return Me._parent
            End Get
            Set(ByVal value As String)
                Me._parent = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._name = String.Empty
            Me._text = String.Empty
            Me._imageindex = 0
            Me._type = String.Empty
            Me._panel = New Panel
            Me._order = 0
            Me._parent = String.Empty
        End Sub
    End Class
End Class

Public Class Enums
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

    Public Enum ImageType As Integer
        Posters = 0
        Fanart = 1
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
    End Enum

    Public Enum ModType As Integer
        NFO = 0
        Poster = 1
        Fanart = 2
        Extra = 3
        Trailer = 4
        Meta = 5
        All = 6
        DoSearch = 7
    End Enum

    Public Enum TVUpdateTime As Integer
        Week = 0
        BiWeekly = 1
        Month = 2
        Always = 3
        Never = 4
    End Enum

    Public Enum TVScraperEventType As Integer
        Progress = 0
        SearchResultsDownloaded = 1
        StartingDownload = 2
        ShowDownloaded = 3
        SavingStarted = 4
        ScraperDone = 5
        LoadingEpisodes = 6
        Searching = 7
        SelectImages = 8
        Verifying = 9
        Cancelled = 10
    End Enum

    Public Enum MovieScraperEventType As Integer
        NFOItem = 1
        PosterItem = 2
        FanartItem = 3
        TrailerItem = 4
        ThumbsItem = 5
        SortTitle = 6
        ListTitle = 7
    End Enum

    Public Enum ModuleType As Integer
        Generic = 0
        Notification = 1
    End Enum
End Class

Public Class Structures
    Public Structure DBMovie
        Dim ID As Long
        Dim DateAdd As Long
        Dim ListTitle As String
        Dim Movie As MediaContainers.Movie
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
        Dim TVShow As MediaContainers.TVShow
        Dim TVEp As MediaContainers.EpisodeDetails
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
        Dim IsNewSeason As Boolean
        Dim IsMarkSeason As Boolean
        Dim IsLockSeason As Boolean
        Dim ShowLanguage As String
    End Structure

    Public Structure Scans
        Dim Movies As Boolean
        Dim TV As Boolean
    End Structure

    Public Structure SettingsResult
        Dim NeedsUpdate As Boolean
        Dim NeedsRefresh As Boolean
        Dim DidCancel As Boolean
    End Structure

    Public Structure ScrapeOptions
        Dim bTitle As Boolean
        Dim bYear As Boolean
        Dim bMPAA As Boolean
        Dim bCert As Boolean
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
        ' Why this 2 arent here?
        Dim bFullCrew As Boolean
        Dim bFullCast As Boolean
    End Structure


    Public Structure ScrapeModifier
        Dim NFO As Boolean
        Dim Poster As Boolean
        Dim Fanart As Boolean
        Dim Extra As Boolean
        Dim Trailer As Boolean
        Dim Meta As Boolean
        Dim DoSearch As Boolean
    End Structure

    Public Structure CustomUpdaterStruct
        Dim Canceled As Boolean
        Dim ScrapeType As Enums.ScrapeType
        Dim Options As ScrapeOptions
    End Structure

    Public Structure TVScrapeOptions
        Dim bShowTitle As Boolean
        Dim bShowEpisodeGuide As Boolean
        Dim bShowGenre As Boolean
        Dim bShowMPAA As Boolean
        Dim bShowPlot As Boolean
        Dim bShowPremiered As Boolean
        Dim bShowRating As Boolean
        Dim bShowStudio As Boolean
        Dim bShowActors As Boolean
        Dim bEpTitle As Boolean
        Dim bEpSeason As Boolean
        Dim bEpEpisode As Boolean
        Dim bEpAired As Boolean
        Dim bEpRating As Boolean
        Dim bEpPlot As Boolean
        Dim bEpDirector As Boolean
        Dim bEpCredits As Boolean
        Dim bEpActors As Boolean
    End Structure

    Public Structure ScrapeInfo
        Dim ShowID As Integer
        Dim ShowTitle As String
        Dim TVDBID As String
        Dim iEpisode As Integer
        Dim iSeason As Integer
        Dim Options As Structures.TVScrapeOptions
        Dim SelectedLang As String
    End Structure

End Class

Public Class Functions
    Public Shared Function EmberAPIVersion() As String
        Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
    End Function

    Public Shared Function ConvertFromUnixTimestamp(ByVal timestamp As Double) As DateTime
        Dim origin As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
        Return origin.AddSeconds(timestamp)
    End Function

    Public Shared Function ConvertToUnixTimestamp(ByVal data As DateTime) As Double
        Dim origin As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0)
        Dim diff As System.TimeSpan = data - origin
        Return Math.Floor(diff.TotalSeconds)
    End Function

    ''' <summary>
    ''' Force of habit
    ''' </summary>
    ''' <returns>Path of the directory containing the Ember executable</returns>
    Public Shared Function AppPath() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory
    End Function

    Public Shared Function ScrapeOptionsAndAlso(ByVal Options As Structures.ScrapeOptions, ByVal Options2 As Structures.ScrapeOptions) As Structures.ScrapeOptions
        Dim filterOptions As New Structures.ScrapeOptions
        filterOptions.bTitle = Options.bTitle AndAlso Options2.bTitle
        filterOptions.bYear = Options.bYear AndAlso Options2.bYear
        filterOptions.bMPAA = Options.bMPAA AndAlso Options2.bMPAA
        filterOptions.bCert = Options.bCert AndAlso Options2.bCert
        filterOptions.bRelease = Options.bRelease AndAlso Options2.bRelease
        filterOptions.bRating = Options.bRating AndAlso Options2.bRating
        filterOptions.bTrailer = Options.bTrailer AndAlso Options2.bTrailer
        filterOptions.bVotes = Options.bVotes AndAlso Options2.bVotes
        filterOptions.bCast = Options.bCast AndAlso Options2.bCast
        filterOptions.bTagline = Options.bTagline AndAlso Options2.bTagline
        filterOptions.bDirector = Options.bDirector AndAlso Options2.bDirector
        filterOptions.bGenre = Options.bGenre AndAlso Options2.bGenre
        filterOptions.bOutline = Options.bOutline AndAlso Options2.bOutline
        filterOptions.bPlot = Options.bPlot AndAlso Options2.bPlot
        filterOptions.bRuntime = Options.bRuntime AndAlso Options2.bRuntime
        filterOptions.bStudio = Options.bStudio AndAlso Options2.bStudio
        filterOptions.bWriters = Options.bWriters AndAlso Options2.bWriters
        filterOptions.bProducers = Options.bProducers AndAlso Options2.bProducers
        filterOptions.bMusicBy = Options.bMusicBy AndAlso Options2.bMusicBy
        filterOptions.bOtherCrew = Options.bOtherCrew AndAlso Options2.bOtherCrew
        filterOptions.bTop250 = Options.bTop250 AndAlso Options2.bTop250
        filterOptions.bFullCrew = Options.bFullCrew AndAlso Options2.bFullCrew
        filterOptions.bFullCast = Options.bFullCast AndAlso Options2.bFullCast
        Return filterOptions
    End Function

    Public Shared Function ScrapeModifierAndAlso(ByVal Options As Structures.ScrapeModifier, ByVal Options2 As Structures.ScrapeModifier) As Structures.ScrapeModifier
        Dim filterModifier As New Structures.ScrapeModifier
        filterModifier.DoSearch = Options.DoSearch AndAlso Options2.DoSearch
        filterModifier.Extra = Options.Extra AndAlso Options2.Extra
        filterModifier.Fanart = Options.Fanart AndAlso Options2.Fanart
        filterModifier.Meta = Options.Meta AndAlso Options2.Meta
        filterModifier.NFO = Options.NFO AndAlso Options2.NFO
        filterModifier.Poster = Options.Poster AndAlso Options2.Poster
        filterModifier.Trailer = Options.Trailer AndAlso Options2.Trailer
        Return filterModifier
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return iMod
    End Function

    ''' <summary>
    ''' Get a list of paths to all sources stored in the database
    ''' </summary>
    Public Shared Sub GetListOfSources()
        Master.SourcesList.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLcommand.CommandText = "SELECT sources.Path FROM sources;"
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    Master.SourcesList.Add(SQLreader("Path").ToString)
                End While
            End Using
        End Using
    End Sub

    Public Shared Sub CreateDefaultOptions()
        With Master.DefaultOptions
            .bCast = Master.eSettings.FieldCast
            .bDirector = Master.eSettings.FieldDirector
            .bGenre = Master.eSettings.FieldGenre
            .bMPAA = Master.eSettings.FieldMPAA
            .bCert = Master.eSettings.FieldCert
            .bMusicBy = Master.eSettings.FieldMusic
            .bOtherCrew = Master.eSettings.FieldCrew
            .bOutline = Master.eSettings.FieldOutline
            .bPlot = Master.eSettings.FieldPlot
            .bProducers = Master.eSettings.FieldProducers
            .bRating = Master.eSettings.FieldRating
            .bRelease = Master.eSettings.FieldRelease
            .bRuntime = Master.eSettings.FieldRuntime
            .bStudio = Master.eSettings.FieldStudio
            .bTagline = Master.eSettings.FieldTagline
            .bTitle = Master.eSettings.FieldTitle
            .bTrailer = Master.eSettings.FieldTrailer
            .bVotes = Master.eSettings.FieldVotes
            .bWriters = Master.eSettings.FieldWriters
            .bYear = Master.eSettings.FieldYear
            .bTop250 = Master.eSettings.Field250
            ' Why this 2 arent here?
            .bFullCrew = Master.eSettings.FullCrew
            .bFullCast = Master.eSettings.FullCast
        End With

        With Master.DefaultTVOptions
            .bShowTitle = Master.eSettings.ScraperShowTitle
            .bShowEpisodeGuide = Master.eSettings.ScraperShowEGU
            .bShowGenre = Master.eSettings.ScraperShowGenre
            .bShowMPAA = Master.eSettings.ScraperShowMPAA
            .bShowPlot = Master.eSettings.ScraperShowPlot
            .bShowPremiered = Master.eSettings.ScraperShowPremiered
            .bShowRating = Master.eSettings.ScraperShowRating
            .bShowStudio = Master.eSettings.ScraperShowStudio
            .bShowActors = Master.eSettings.ScraperShowActors
            .bEpTitle = Master.eSettings.ScraperEpTitle
            .bEpSeason = Master.eSettings.ScraperEpSeason
            .bEpEpisode = Master.eSettings.ScraperEpEpisode
            .bEpAired = Master.eSettings.ScraperEpAired
            .bEpRating = Master.eSettings.ScraperEpRating
            .bEpPlot = Master.eSettings.ScraperEpPlot
            .bEpDirector = Master.eSettings.ScraperEpDirector
            .bEpCredits = Master.eSettings.ScraperEpCredits
            .bEpActors = Master.eSettings.ScraperEpActors
        End With
    End Sub

    Public Shared Sub SetScraperMod(ByVal MType As Enums.ModType, ByVal MValue As Boolean, Optional ByVal DoClear As Boolean = True)
        With Master.GlobalScrapeMod
            If DoClear Then
                .Extra = False
                .Fanart = False
                .Meta = False
                .NFO = False
                .Poster = False
                .Trailer = False
                .DoSearch = False
            End If

            Select Case MType
                Case Enums.ModType.All
                    .Extra = MValue
                    .Fanart = MValue
                    .Meta = MValue
                    .NFO = MValue
                    .Poster = MValue
                    .Trailer = If(Master.eSettings.UpdaterTrailers, MValue, False)
                Case Enums.ModType.Extra
                    .Extra = MValue
                Case Enums.ModType.Fanart
                    .Fanart = MValue
                Case Enums.ModType.Meta
                    .Meta = MValue
                Case Enums.ModType.NFO
                    .NFO = MValue
                Case Enums.ModType.Poster
                    .Poster = MValue
                Case Enums.ModType.Trailer
                    .Trailer = MValue
                Case Enums.ModType.DoSearch
                    .DoSearch = MValue
            End Select

        End With
    End Sub

    Public Shared Function HasModifier() As Boolean
        With Master.GlobalScrapeMod
            If .Extra OrElse .Fanart OrElse .Meta OrElse .NFO OrElse .Poster OrElse .Trailer Then Return True
        End With

        Return False
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
                Master.CanScanDiscImage = True
            Else
                Master.CanScanDiscImage = False
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Constrain a number to the nearest multiple
    ''' </summary>
    ''' <param name="iNumber">Number to quantize</param>
    ''' <param name="iMultiple">Multiple of constraint.</param>
    Public Shared Function Quantize(ByVal iNumber As Integer, ByVal iMultiple As Integer) As Integer
        Return Convert.ToInt32(System.Math.Round(iNumber / iMultiple, 0) * iMultiple)
    End Function

    Public Shared Sub DGVDoubleBuffer(ByRef cDGV As DataGridView)
        Dim conType As Type = cDGV.GetType
        Dim pi As System.Reflection.PropertyInfo = conType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        pi.SetValue(cDGV, True, Nothing)
    End Sub

    Public Shared Sub PNLDoubleBuffer(ByRef cPNL As Panel)
        Dim conType As Type = cPNL.GetType
        Dim pi As System.Reflection.PropertyInfo = conType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        pi.SetValue(cPNL, True, Nothing)
    End Sub

    ''' <summary>
    ''' Check to make sure user has at least .NET Framework 3.5 installed
    ''' </summary>
    ''' <returns>True if installed version is >= 3.5, false if not.</returns>
    Public Shared Function GetNETVersion() As Boolean
        Try
            Const regLocation As String = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP"
            Dim masterKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(regLocation)
            Dim tempKey As Microsoft.Win32.RegistryKey
            Dim sVersion As String = String.Empty

            If Not IsNothing(masterKey) Then
                Dim SubKeyNames As String() = masterKey.GetSubKeyNames()
                For i As Integer = 0 To SubKeyNames.Count - 1
                    tempKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(String.Concat(regLocation, "\\", SubKeyNames(i)))
                    If Not IsNothing(tempKey) Then
                        Try
                            sVersion = tempKey.GetValue("Version").ToString
                        Catch ex As Exception
                            ' GetValue can Raise Exceptions  when some key are Close or Marked for Deletion
                            sVersion = String.Empty 'clear variable
                        End Try
                        If Not String.IsNullOrEmpty(sVersion) Then
                            If NumUtils.ConvertToSingle(sVersion.Substring(0, 3)) >= 3.5 Then Return True
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
        End Try
        Return False
    End Function

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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return "Unavailable"
    End Function

    Public Shared Function Check64Bit() As Boolean
        Return (IntPtr.Size = 8)
    End Function

    ''' <summary>
    ''' Check for the lastest version of Ember
    ''' </summary>
    ''' <returns>Latest version as integer</returns>
    Public Shared Function CheckNeedUpdate() As Boolean
        Dim sHTTP As New HTTP
        Dim needUpdate As Boolean = False
        Dim platform As String = If(Master.is64Bit, "x64", "x86")
        Dim updateXML As String = sHTTP.DownloadData("http://www.embermm.com/Updates/versions.xml")
        sHTTP = Nothing
        If updateXML.Length > 0 Then
            For Each v As ModulesManager.VersionItem In ModulesManager.VersionList
                Dim vl As ModulesManager.VersionItem = v
                Dim n As String = String.Empty
                Dim xmlUpdate As XDocument
                Try
                    xmlUpdate = XDocument.Parse(updateXML)
                Catch
                    Return False
                End Try
                Dim xUdpate = From xUp In xmlUpdate...<Config>...<Modules>...<File> Where (xUp.<Version>.Value <> "" AndAlso xUp.<Name>.Value = vl.AssemblyFileName AndAlso xUp.<Platform>.Value = platform) Select xUp.<Version>.Value
                Try
                    If Convert.ToInt16(xUdpate(0)) > Convert.ToInt16(v.Version) Then
                        v.NeedUpdate = True
                        needUpdate = True
                    End If

                Catch ex As Exception
                End Try
            Next
            Return needUpdate
        End If
    End Function
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return 0
        End Try
    End Function

    Public Shared Function CheckIfWindows() As Boolean
        Return Environment.OSVersion.ToString.ToLower.IndexOf("windows") > 0
    End Function

    Public Shared Function GetFFMpeg() As String
        If Master.isWindows Then
            Return String.Concat(Functions.AppPath, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
        Else
            Return "ffmpeg"
        End If
    End Function

    Public Shared Sub Notify(ByVal Type As String, ByVal Icon As Integer, ByVal Title As String, ByVal Message As String, Optional ByVal CustomIcon As Image = Nothing)
        Try
            If Not IsNothing(Master.NotifierModule) Then
                Master.NotifierModule.RunGeneric(Enums.ModuleType.Notification, New List(Of Object)(New Object() {Type, Icon, Title, Message, CustomIcon}))
            End If
        Catch
        End Try
    End Sub
End Class
