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
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Public Class Master

    'Global Variables
    Public Shared eSettings As New emmSettings
    Public Shared eLang As New Localization
    Public Shared MediaList As New List(Of FileAndSource)
    Public Shared eLog As New ErrorLogger
    Public Shared DefaultOptions As New ScrapeOptions
    Public Shared GlobalScrapeMod As New ScrapeModifier
    Public Shared alMoviePaths As New ArrayList
    Public Shared DB As New Database
    Public Shared TempPath As String = Path.Combine(AppPath, "Temp")
    Public Shared currMovie As New DBMovie
    Public Shared CanScanDiscImage As Boolean

    Public Shared tmpMovie As New Media.Movie

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
        Small = 3
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
        YouTube = 0
        Imdb = 1
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

    Public Class FileAndSource
        Dim _filename As String
        Dim _source As String
        Dim _single As Boolean
        Dim _usefolder As Boolean
        Dim _poster As String
        Dim _fanart As String
        Dim _nfo As String
        Dim _extra As String
        Dim _trailer As String
        Dim _subs As String
        Dim _contents As New List(Of FileInfo)

        Public Property Filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal value As String)
                _filename = value
            End Set
        End Property

        Public Property Source() As String
            Get
                Return _source
            End Get
            Set(ByVal value As String)
                _source = value
            End Set
        End Property

        Public Property isSingle() As Boolean
            Get
                Return _single
            End Get
            Set(ByVal value As Boolean)
                _single = value
            End Set
        End Property

        Public Property UseFolder() As Boolean
            Get
                Return _usefolder
            End Get
            Set(ByVal value As Boolean)
                _usefolder = value
            End Set
        End Property

        Public Property Poster() As String
            Get
                Return _poster
            End Get
            Set(ByVal value As String)
                _poster = value
            End Set
        End Property

        Public Property Fanart() As String
            Get
                Return _fanart
            End Get
            Set(ByVal value As String)
                _fanart = value
            End Set
        End Property

        Public Property Nfo() As String
            Get
                Return _nfo
            End Get
            Set(ByVal value As String)
                _nfo = value
            End Set
        End Property

        Public Property Extra() As String
            Get
                Return _extra
            End Get
            Set(ByVal value As String)
                _extra = value
            End Set
        End Property

        Public Property Trailer() As String
            Get
                Return _trailer
            End Get
            Set(ByVal value As String)
                _trailer = value
            End Set
        End Property

        Public Property Subs() As String
            Get
                Return _subs
            End Get
            Set(ByVal value As String)
                _subs = value
            End Set
        End Property

        Public Property Contents() As List(Of FileInfo)
            Get
                Return _contents
            End Get
            Set(ByVal value As List(Of FileInfo))
                _contents = value
            End Set
        End Property

        Public Sub New()
            Clear()
        End Sub

        Public Sub Clear()
            _filename = String.Empty
            _source = String.Empty
            _usefolder = False
            _poster = String.Empty
            _fanart = String.Empty
            _nfo = String.Empty
            _extra = String.Empty
            _trailer = String.Empty
            _subs = String.Empty
            _contents.Clear()
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

    Public Shared Sub SetScraperMod(ByVal MType As ModType, ByVal MValue As Boolean, Optional ByVal Clear As Boolean = False)
        With GlobalScrapeMod
            If Clear Then
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

    Public Shared Sub ScanSourceDir(ByVal sSource As String, ByVal sPath As String, ByVal bRecur As Boolean, ByVal bUseFolder As Boolean, ByVal bSingle As Boolean)

        '//
        ' Get all directories in the parent directory
        '\\

        Try
            Dim sMoviePath As String = String.Empty
            If Directory.Exists(sPath) Then

                'check if there are any movies in the parent folder
                ScanForFiles(sPath, sSource, bUseFolder, bSingle)

                Dim Dirs As New ArrayList

                Try
                    Dirs.AddRange(Directory.GetDirectories(sPath))
                Catch
                End Try

                For Each inDir As String In Dirs
                    If isValidDir(inDir) Then
                        ScanForFiles(inDir, sSource, bUseFolder, bSingle)
                        If bRecur Then
                            ScanSourceDir(sSource, inDir, bRecur, bUseFolder, bSingle)
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Sub ScanForFiles(ByVal sPath As String, ByVal sSource As String, ByVal bUseFolder As Boolean, ByVal bSingle As Boolean)

        '//
        ' Get all files in the directory
        '\\

        Try

            Dim tmpList As New ArrayList
            Dim di As DirectoryInfo
            Dim lFi As New List(Of FileInfo)
            Dim SkipStack As Boolean = False
            Dim fList As New List(Of FileAndSource)
            Dim tSingle As Boolean = False
            Dim vtsSingle As Boolean = False
            Dim tFile As String = String.Empty

            If Directory.Exists(Path.Combine(sPath, "VIDEO_TS")) Then
                di = New DirectoryInfo(Path.Combine(sPath, "VIDEO_TS"))
                bSingle = True
            Else
                di = New DirectoryInfo(sPath)
            End If

            Try
                lFi.AddRange(di.GetFiles())
            Catch
            End Try

            If lFi.Count > 0 Then

                If eSettings.AutoDetectVTS Then
                    Dim hasIfo As Integer = 0
                    Dim hasVob As Integer = 0
                    Dim hasBup As Integer = 0
                    For Each lfile As FileInfo In lFi
                        If Path.GetExtension(lfile.FullName).ToLower = ".ifo" Then hasIfo = 1
                        If Path.GetExtension(lfile.FullName).ToLower = ".vob" Then hasVob = 1
                        If Path.GetExtension(lfile.FullName).ToLower = ".bup" Then hasBup = 1
                        If Path.GetFileName(lfile.FullName).ToLower = "video_ts.vob" Then
                            'video_ts.vob takes precedence
                            tFile = lfile.FullName
                        ElseIf String.IsNullOrEmpty(tFile) AndAlso (Path.GetFileName(lfile.FullName).ToLower = "video_ts.ifo" _
                        OrElse Path.GetFileName(lfile.FullName).ToLower = "video_ts.bup") Then
                            tFile = lfile.FullName
                        End If
                        vtsSingle = (hasIfo + hasVob + hasBup) > 1
                        If vtsSingle AndAlso Path.GetFileName(tFile).ToLower = "video_ts.vob" Then Exit For
                    Next
                End If

                If vtsSingle AndAlso Not String.IsNullOrEmpty(tFile) Then
                    If Not tmpList.Contains(tFile.ToLower) AndAlso _
                    Not Path.GetFileName(tFile).ToLower.Contains("-trailer") AndAlso Not Path.GetFileName(tFile).ToLower.Contains("[trailer") AndAlso _
                    Not Path.GetFileName(tFile).ToLower.Contains("sample") Then
                        tmpList.Add(tFile.ToLower)
                        If alMoviePaths.Contains(tFile.ToLower) Then
                            fList.Add(New FileAndSource With {.Filename = tFile, .Source = "[!FROMDB!]"})
                        Else
                            fList.Add(New FileAndSource With {.Filename = tFile, .Source = sSource, .isSingle = bSingle, .UseFolder = bUseFolder, .Contents = lFi})
                        End If
                    End If
                Else
                    lFi.Sort(AddressOf SortFileNames)

                    For Each lFile As FileInfo In lFi

                        If eSettings.ValidExts.Contains(lFile.Extension.ToLower) AndAlso Not tmpList.Contains(StringManip.CleanStackingMarkers(lFile.FullName).ToLower) AndAlso _
                        Not lFile.Name.ToLower.Contains("-trailer") AndAlso Not lFile.Name.ToLower.Contains("[trailer") AndAlso Not lFile.Name.ToLower.Contains("sample") AndAlso _
                        ((eSettings.SkipStackSizeCheck AndAlso StringManip.IsStacked(lFile.Name)) OrElse lFile.Length >= eSettings.SkipLessThan * 1048576) Then
                            If eSettings.NoStackExts.Contains(lFile.Extension.ToLower) Then
                                tmpList.Add(lFile.FullName.ToLower)
                                SkipStack = True
                            Else
                                tmpList.Add(StringManip.CleanStackingMarkers(lFile.FullName).ToLower)
                            End If
                            If alMoviePaths.Contains(lFile.FullName.ToLower) Then
                                fList.Add(New FileAndSource With {.Filename = lFile.FullName, .Source = "[!FROMDB!]"})
                            Else
                                fList.Add(New FileAndSource With {.Filename = lFile.FullName, .Source = sSource, .isSingle = bSingle, .UseFolder = If(bSingle, bUseFolder, False), .Contents = lFi})
                            End If
                            If bSingle AndAlso Not SkipStack Then Exit For
                        End If
                    Next
                End If

                If fList.Count = 1 Then tSingle = True

                If tSingle Then
                    fList(0).isSingle = True
                    fList(0).UseFolder = bUseFolder
                    MediaList.Add(fList(0))
                Else
                    MediaList.AddRange(fList)
                End If

                fList = Nothing
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function isValidDir(ByVal sPath As String) As Boolean

        '//
        ' Make sure it's a valid directory
        '\\

        Try

            sPath = sPath.Remove(0, sPath.IndexOf("\"))
            If Path.GetDirectoryName(sPath).ToLower = "extrathumbs" OrElse _
            Path.GetDirectoryName(sPath).ToLower = "extras" OrElse _
            Path.GetDirectoryName(sPath).ToLower = "video_ts" OrElse _
            Path.GetDirectoryName(sPath).ToLower = "audio_ts" OrElse _
            Path.GetDirectoryName(sPath).ToLower = "recycler" OrElse _
            Path.GetDirectoryName(sPath).ToLower = "subs" OrElse _
            Path.GetDirectoryName(sPath).ToLower = "subtitles" OrElse _
            sPath.ToLower.Contains("-trailer") OrElse _
            sPath.ToLower.Contains("[trailer") OrElse _
            sPath.ToLower.Contains("temporary files") OrElse _
            sPath.ToLower.Contains("(noscan)") OrElse _
            sPath.ToLower.Contains("$recycle.bin") OrElse _
            sPath.ToLower.Contains("lost+found") OrElse _
            sPath.ToLower.Contains("system volume information") OrElse _
            sPath.ToLower.Contains("sample") OrElse _
            sPath.Contains(":") Then
                Return False
            End If

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return False
        End Try
        Return True 'This is the Else
    End Function


    Public Shared Function RemoveExtFromPath(ByVal sPath As String) As String

        '//
        ' Get the entire path without the extension
        '\\

        Try
            Return Path.Combine(Directory.GetParent(sPath).FullName, Path.GetFileNameWithoutExtension(sPath))
        Catch
            Return String.Empty
        End Try

    End Function

    Public Shared Function GetFolderContents(ByVal sPath As String, ByVal bSingle As Boolean) As String()

        '//
        ' Check if a folder has all the items (nfo, poster, fanart, etc)
        '\\

        Dim NfoPath As String = String.Empty
        Dim PosterPath As String = String.Empty
        Dim FanartPath As String = String.Empty
        Dim TrailerPath As String = String.Empty
        Dim SubPath As String = String.Empty
        Dim ExtraPath As String = String.Empty
        Dim aResults(6) As String
        Dim tmpName As String = String.Empty
        Dim tmpNameNoStack As String = String.Empty
        Dim currname As String = String.Empty
        Dim parPath As String = String.Empty
        Dim fList As New ArrayList
        Try
            If eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                Try
                    fList.AddRange(Directory.GetFiles(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName))
                Catch
                End Try

                If bSingle AndAlso File.Exists(String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    ExtraPath = String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")
                End If
            Else
                If bSingle Then
                    Try
                        fList.AddRange(Directory.GetFiles(Directory.GetParent(sPath).FullName))
                    Catch
                    End Try
                Else
                    Try
                        Dim sName As String = StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath), True)
                        fList.AddRange(Directory.GetFiles(Directory.GetParent(sPath).FullName, If(sName.EndsWith("*"), sName, String.Concat(sName, "*"))))
                    Catch
                    End Try
                End If

                If bSingle AndAlso File.Exists(String.Concat(Directory.GetParent(sPath).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    ExtraPath = String.Concat(Directory.GetParent(sPath).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")
                End If
            End If

            parPath = Directory.GetParent(sPath).FullName.ToLower
            tmpName = Path.Combine(parPath, StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath))).ToLower
            tmpNameNoStack = Path.Combine(parPath, Path.GetFileNameWithoutExtension(sPath)).ToLower

            For Each fFile As String In fList
                'fanart
                If String.IsNullOrEmpty(FanartPath) Then
                    If (bSingle AndAlso eSettings.FanartJPG AndAlso fFile.ToLower = Path.Combine(parPath, "fanart.jpg")) _
                        OrElse ((Not bSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                        ((eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, "-fanart.jpg")) _
                        OrElse (eSettings.MovieNameDotFanartJPG AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".fanart.jpg")) _
                        OrElse (eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = String.Concat(tmpName, "-fanart.jpg")) _
                        OrElse (eSettings.MovieNameDotFanartJPG AndAlso fFile.ToLower = String.Concat(tmpName, ".fanart.jpg")) _
                        OrElse (eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts-fanart.jpg")) _
                        OrElse (eSettings.MovieNameDotFanartJPG AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.fanart.jpg")))) Then
                        FanartPath = fFile
                        Continue For
                    End If
                End If

                'poster
                If String.IsNullOrEmpty(PosterPath) Then
                    If (bSingle AndAlso (eSettings.MovieTBN AndAlso fFile.ToLower = Path.Combine(parPath, "movie.tbn")) _
                        OrElse (eSettings.PosterTBN AndAlso fFile.ToLower = Path.Combine(parPath, "poster.tbn")) _
                        OrElse (eSettings.MovieJPG AndAlso fFile.ToLower = Path.Combine(parPath, "movie.jpg")) _
                        OrElse (eSettings.PosterJPG AndAlso fFile.ToLower = Path.Combine(parPath, "poster.jpg")) _
                        OrElse (eSettings.FolderJPG AndAlso fFile.ToLower = Path.Combine(parPath, "folder.jpg"))) _
                        OrElse ((Not bSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                        ((eSettings.MovieNameTBN AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".tbn")) _
                        OrElse (eSettings.MovieNameTBN AndAlso fFile.ToLower = String.Concat(tmpName, ".tbn")) _
                        OrElse (eSettings.MovieNameJPG AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".jpg")) _
                        OrElse (eSettings.MovieNameJPG AndAlso fFile.ToLower = String.Concat(tmpName, ".jpg")) _
                        OrElse (eSettings.MovieNameTBN AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.tbn")) _
                        OrElse (eSettings.MovieNameJPG AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.jpg")))) Then
                        PosterPath = fFile
                        Continue For
                    End If
                End If

                'nfo
                If String.IsNullOrEmpty(NfoPath) Then
                    If (bSingle AndAlso eSettings.MovieNFO AndAlso fFile.ToLower = Path.Combine(parPath, "movie.nfo")) _
                    OrElse ((Not bSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                    ((eSettings.MovieNameNFO AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".nfo")) _
                    OrElse (eSettings.MovieNameNFO AndAlso fFile.ToLower = String.Concat(tmpName, ".nfo")))) Then
                        NfoPath = fFile
                        Continue For
                    End If
                End If

                If String.IsNullOrEmpty(SubPath) Then
                    If Regex.IsMatch(fFile, String.Concat("^", Regex.Escape(tmpNameNoStack), "(\.(.*?))?\.(sst|srt|sub|ssa|aqt|smi|sami|jss|mpl|rt|idx|ass)$"), RegexOptions.IgnoreCase) OrElse _
                                Regex.IsMatch(fFile, String.Concat("^", Regex.Escape(tmpName), "(\.(.*?))?\.(sst|srt|sub|ssa|aqt|smi|sami|jss|mpl|rt|idx|ass)$"), RegexOptions.IgnoreCase) Then
                        SubPath = fFile
                        Continue For
                    End If
                End If

                If String.IsNullOrEmpty(TrailerPath) Then
                    For Each t As String In eSettings.ValidExts
                        Select Case True
                            Case fFile.ToLower = String.Concat(tmpNameNoStack, "-trailer", t.ToLower)
                                TrailerPath = fFile
                                Exit For
                            Case fFile.ToLower = String.Concat(tmpName, "-trailer", t.ToLower)
                                TrailerPath = fFile
                                Exit For
                            Case fFile.ToLower = String.Concat(tmpNameNoStack, "[trailer]", t.ToLower)
                                TrailerPath = fFile
                                Exit For
                            Case fFile.ToLower = String.Concat(tmpName, "[trailer]", t.ToLower)
                                TrailerPath = fFile
                                Exit For
                            Case bSingle AndAlso fFile.ToLower = Path.Combine(parPath, String.Concat("movie-trailer", t.ToLower))
                                TrailerPath = fFile
                                Exit For
                            Case bSingle AndAlso fFile.ToLower = Path.Combine(parPath, String.Concat("movie[trailer]", t.ToLower))
                                TrailerPath = fFile
                                Exit For
                        End Select
                    Next
                End If

                If Not String.IsNullOrEmpty(PosterPath) AndAlso Not String.IsNullOrEmpty(FanartPath) _
                AndAlso Not String.IsNullOrEmpty(NfoPath) AndAlso Not String.IsNullOrEmpty(TrailerPath) _
                AndAlso Not String.IsNullOrEmpty(SubPath) AndAlso Not String.IsNullOrEmpty(ExtraPath) Then
                    Exit For
                End If
            Next

            aResults(0) = PosterPath
            aResults(1) = FanartPath
            aResults(2) = NfoPath
            aResults(3) = TrailerPath
            aResults(4) = SubPath
            aResults(5) = ExtraPath
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return aResults
    End Function

    Public Shared Function GetTrailerPath(ByVal sPath As String) As String

        '//
        ' Get the proper path to trailer
        '\\

        Dim tFile As String = String.Empty

        Dim parPath As String = Directory.GetParent(sPath).FullName
        Dim tmpName As String = Path.Combine(parPath, StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath)))
        Dim tmpNameNoStack As String = Path.Combine(parPath, Path.GetFileNameWithoutExtension(sPath))
        For Each t As String In eSettings.ValidExts
            If File.Exists(String.Concat(tmpName, "-trailer", t)) Then
                tFile = String.Concat(tmpName, "-trailer", t)
                Exit For
            ElseIf File.Exists(String.Concat(tmpName, "[trailer]", t)) Then
                tFile = String.Concat(tmpName, "[trailer]", t)
                Exit For
            ElseIf File.Exists(String.Concat(tmpNameNoStack, "-trailer", t)) Then
                tFile = String.Concat(tmpNameNoStack, "-trailer", t)
                Exit For
            ElseIf File.Exists(String.Concat(tmpNameNoStack, "[trailer]", t)) Then
                tFile = String.Concat(tmpNameNoStack, "[trailer]", t)
                Exit For
            End If
        Next

        Return tFile

    End Function

    Public Shared Function SortFileNames(ByVal x As FileInfo, ByVal y As FileInfo) As Integer

        Try
            If String.IsNullOrEmpty(x.Name) Then
                Return -1
            End If
            If String.IsNullOrEmpty(y.Name) Then
                Return 1
            End If

            Return x.Name.CompareTo(y.Name)
        Catch
            Return 0
        End Try

    End Function

    Public Shared Function SortThumbFileNames(ByVal x As FileInfo, ByVal y As FileInfo) As Integer

        Try
            Dim ObjectCompare As New CaseInsensitiveComparer

            If String.IsNullOrEmpty(x.Name) Then
                Return -1
            End If
            If String.IsNullOrEmpty(y.Name) Then
                Return 1
            End If

            Return ObjectCompare.Compare(Convert.ToInt32(Regex.Match(x.Name, "(\d+)").Groups(0).ToString), Convert.ToInt32(Regex.Match(y.Name, "(\d+)").Groups(0).ToString))
        Catch
            Return 0
        End Try

    End Function

    Public Shared Function GetExtraModifier(ByVal sPath As String) As Integer

        '//
        ' Get the number of the last thumb#.jpg to make sure we're not overwriting current ones
        '\\

        Dim iMod As Integer = 0
        Dim lThumbs As New ArrayList

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

    Public Shared Sub MoveFileWithStream(ByVal sPathFrom As String, ByVal sPathTo As String)

        '//
        ' Copy a file from one location to another using a stream/buffer
        '\\

        Try
            Using SourceStream As FileStream = New FileStream(String.Concat("", sPathFrom, ""), FileMode.Open, FileAccess.Read)
                Using DestinationStream As FileStream = New FileStream(String.Concat("", sPathTo, ""), FileMode.Create, FileAccess.Write)
                    Dim StreamBuffer(Convert.ToInt32(SourceStream.Length - 1)) As Byte

                    SourceStream.Read(StreamBuffer, 0, StreamBuffer.Length)
                    DestinationStream.Write(StreamBuffer, 0, StreamBuffer.Length)

                    StreamBuffer = Nothing
                End Using
            End Using
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

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

    Public Shared Function DeleteFiles(ByVal isCleaner As Boolean, ByVal mMovie As DBMovie) As Boolean
        Dim dPath As String = String.Empty
        Dim bReturn As Boolean = False
        Try
            If eSettings.VideoTSParent AndAlso Directory.GetParent(mMovie.Filename).Name.ToLower = "video_ts" Then
                dPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".ext")
            Else
                dPath = mMovie.Filename
            End If

            Dim sOrName As String = StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(dPath))
            Dim sPathShort As String = Directory.GetParent(dPath).FullName
            Dim sPathNoExt As String = RemoveExtFromPath(dPath)

            Dim dirInfo As New DirectoryInfo(sPathShort)
            Dim ioFi As New List(Of FileInfo)

            Try
                ioFi.AddRange(dirInfo.GetFiles())
            Catch
            End Try

            If isCleaner And eSettings.ExpertCleaner Then

                For Each sFile As FileInfo In ioFi
                    If Not eSettings.CleanWhitelistExts.Contains(sFile.Extension.ToLower) AndAlso ((eSettings.CleanWhitelistVideo AndAlso Not eSettings.ValidExts.Contains(sFile.Extension.ToLower)) OrElse Not eSettings.CleanWhitelistVideo) Then
                        File.Delete(sFile.FullName)
                        bReturn = True
                    End If
                Next

            Else

                If Not isCleaner Then
                    Dim fPath As String = mMovie.FanartPath
                    Dim tPath As String = String.Empty
                    If Not String.IsNullOrEmpty(fPath) Then
                        If Directory.GetParent(fPath).Name.ToLower = "video_ts" Then
                            If Path.GetFileName(fPath).ToLower = "fanart.jpg" Then
                                tPath = Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(fPath).FullName).Name, "-fanart.jpg"))
                            Else
                                tPath = Path.Combine(eSettings.BDPath, Path.GetFileName(fPath))
                            End If
                        Else
                            If Path.GetFileName(fPath).ToLower = "fanart.jpg" Then
                                tPath = Path.Combine(eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(mMovie.Filename), "-fanart.jpg"))
                            Else
                                tPath = Path.Combine(eSettings.BDPath, Path.GetFileName(fPath))
                            End If
                        End If
                    End If
                    If Not String.IsNullOrEmpty(tPath) Then
                        File.Delete(tPath)
                    End If
                End If

                If Not isCleaner AndAlso mMovie.isSingle Then
                    If Directory.GetParent(mMovie.Filename).Name.ToLower = "video_ts" Then
                        DeleteDirectory(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName)
                    Else
                        DeleteDirectory(Directory.GetParent(mMovie.Filename).FullName)
                    End If
                Else
                    For Each lFI As FileInfo In ioFi
                        If isCleaner Then
                            If (eSettings.CleanFolderJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "folder.jpg")) _
                                OrElse (eSettings.CleanFanartJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "fanart.jpg")) _
                                OrElse (eSettings.CleanMovieTBN AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.tbn")) _
                                OrElse (eSettings.CleanMovieNFO AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.nfo")) _
                                OrElse (eSettings.CleanPosterTBN AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "poster.tbn")) _
                                OrElse (eSettings.CleanPosterJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "poster.jpg")) _
                                OrElse (eSettings.CleanMovieJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.jpg")) Then
                                File.Delete(lFI.FullName)
                                bReturn = True
                                Continue For
                            End If
                        End If

                        If (eSettings.CleanMovieTBNB AndAlso isCleaner) OrElse (Not isCleaner) Then
                            If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".tbn") _
                            OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.tbn") _
                            OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".tbn") Then
                                File.Delete(lFI.FullName)
                                bReturn = True
                                Continue For
                            End If
                        End If

                        If (eSettings.CleanMovieFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                            If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, "-fanart.jpg") _
                                OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts-fanart.jpg") _
                                OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), "-fanart.jpg") Then
                                File.Delete(lFI.FullName)
                                bReturn = True
                                Continue For
                            End If
                        End If

                        If (eSettings.CleanMovieNFOB AndAlso isCleaner) OrElse (Not isCleaner) Then
                            If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".nfo") _
                                OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.nfo") _
                                OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".nfo") Then
                                File.Delete(lFI.FullName)
                                bReturn = True
                                Continue For
                            End If
                        End If

                        If (eSettings.CleanDotFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                            If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".fanart.jpg") _
                                OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.fanart.jpg") _
                                OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".fanart.jpg") Then
                                File.Delete(lFI.FullName)
                                bReturn = True
                                Continue For
                            End If
                        End If

                        If (eSettings.CleanMovieNameJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                            If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".jpg") _
                                OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.jpg") _
                                OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".jpg") Then
                                File.Delete(lFI.FullName)
                                bReturn = True
                                Continue For
                            End If
                        End If
                    Next

                    If Not isCleaner Then

                        ioFi.Clear()
                        Try
                            ioFi.AddRange(dirInfo.GetFiles(String.Concat(sOrName, "*.*")))
                        Catch
                        End Try

                        Try
                            ioFi.AddRange(dirInfo.GetFiles(String.Concat(Path.GetFileNameWithoutExtension(mMovie.Filename), ".*")))
                        Catch
                        End Try

                        For Each sFile As FileInfo In ioFi
                            File.Delete(sFile.FullName)
                        Next
                    End If

                    If eSettings.CleanExtraThumbs Then
                        If Directory.Exists(Path.Combine(sPathShort, "extrathumbs")) Then
                            DeleteDirectory(Path.Combine(sPathShort, "extrathumbs"))
                            bReturn = True
                        End If
                    End If

                End If
            End If

            ioFi = Nothing
            dirInfo = Nothing
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return bReturn
    End Function

    Public Shared Function CreateRandomThumbs(ByVal mMovie As DBMovie, ByVal ThumbCount As Integer, ByVal isEdit As Boolean) As String
        Dim tThumb As New ThumbGenerator

        tThumb.Movie = mMovie
        tThumb.ThumbCount = ThumbCount
        tThumb.isEdit = isEdit

        tThumb.Start()

        Return tThumb.SetFA
    End Function

    Public Shared Function CheckUpdate() As Integer
        Try
            Dim sHTTP As New HTTP
            Dim updateXML As String = sHTTP.DownloadData("http://www.embermm.com/Updates/Update.xml")
            sHTTP = Nothing

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
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return 0
        End Try
    End Function

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

    Public Shared Sub DeleteDirectory(ByVal sPath As String)
        Try
            If Directory.Exists(sPath) Then

                Dim Dirs As New ArrayList

                Try
                    Dirs.AddRange(Directory.GetDirectories(sPath))
                Catch
                End Try

                For Each inDir As String In Dirs
                    DeleteDirectory(inDir)
                Next


                Dim fFiles As New ArrayList

                Try
                    fFiles.AddRange(Directory.GetFiles(sPath))
                Catch
                End Try

                For Each fFile As String In fFiles
                    Try
                        File.Delete(fFile)
                    Catch
                    End Try
                Next

                Directory.Delete(sPath, True)
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function Quantize(ByVal iNumber As Integer, ByVal iMultiple As Integer) As Integer
        Return Convert.ToInt32(System.Math.Round(iNumber / iMultiple, 0) * iMultiple)
    End Function

    Public Shared Function AppPath() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory
    End Function

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
End Class
