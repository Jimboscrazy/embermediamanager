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

Imports System.IO
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports Microsoft.Win32

Public Class Master

    'Global Variables
    Public Shared eSettings As New emmSettings
    Public Shared MediaList As New List(Of FileAndSource)
    Public Shared eLog As New ErrorLogger
    Public Shared DefaultOptions As New ScrapeOptions
    Public Shared alMoviePaths As New ArrayList
    Public Shared DB As New Database
    Public Shared TempPath As String = Path.Combine(Application.StartupPath, "Temp")
    Public Shared currMovie As New DBMovie

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
        CopyBD = 11
        RevertStudios = 12
    End Enum

    Public Enum ImageType As Integer
        Posters = 0
        Fanart = 1
    End Enum

    Public Enum TrailerPages As Integer
        YouTube = 0
        Imdb = 1
    End Enum

    Public Enum ScrapeModifier As Integer
        All = 0
        NFO = 1
        Poster = 2
        Fanart = 3
        Extra = 4
        Trailer = 5
        MI = 6
    End Enum

    Public Structure DBMovie
        Dim ID As Integer
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
    End Structure

    Public Structure CustomUpdaterStruct
        Dim Canceled As Boolean
        Dim ScrapeType As ScrapeType
        Dim Modifier As ScrapeModifier
        Dim Options As ScrapeOptions
    End Structure

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
            .bCast = True
            .bDirector = True
            .bGenre = True
            .bMPAA = True
            .bMusicBy = True
            .bOtherCrew = True
            .bOutline = True
            .bPlot = True
            .bProducers = True
            .bRating = True
            .bRelease = True
            .bRuntime = True
            .bStudio = True
            .bTagline = True
            .bTitle = True
            .bTrailer = True
            .bVotes = True
            .bWriters = True
            .bYear = True
        End With
    End Sub

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

                'Check folder if it contains ifo, vob, and bup, consider it a video_ts folder (if bSingle is not already true)
                If eSettings.AutoDetectVTS AndAlso Not bSingle Then
                    Dim hasIfo As Integer = 0
                    Dim hasVob As Integer = 0
                    Dim hasBup As Integer = 0
                    For Each lfile As FileInfo In lFi
                        If Path.GetExtension(lfile.FullName).ToLower = ".ifo" Then
                            hasIfo = 1
                        End If
                        If Path.GetExtension(lfile.FullName).ToLower = ".vob" Then
                            hasVob = 1
                        End If
                        If Path.GetExtension(lfile.FullName).ToLower = ".bup" Then
                            hasBup = 1
                        End If
                        If (hasIfo + hasVob + hasBup) > 1 Then Exit For
                    Next
                    bSingle = (hasIfo + hasVob + hasBup) > 1
                End If

                lFi.Sort(AddressOf SortFileNames)

                For Each lFile As FileInfo In lFi

                    If eSettings.ValidExts.Contains(lFile.Extension.ToLower) AndAlso Not tmpList.Contains(StringManip.CleanStackingMarkers(lFile.FullName).ToLower) AndAlso _
                    Not lFile.Name.ToLower.Contains("-trailer") AndAlso Not lFile.Name.ToLower.Contains("[trailer") AndAlso Not lFile.Name.ToLower.Contains("sample") AndAlso _
                    ((eSettings.SkipStackSizeCheck AndAlso StringManip.IsStacked(lFile.Name)) OrElse lFile.Length >= eSettings.SkipLessThan * 1048576) Then
                        If Master.eSettings.NoStackExts.Contains(lFile.Extension.ToLower) Then
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


    Public Shared Function RemoveExtFromPath(ByVal sPath As String)

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
        Dim tList As New ArrayList
        Try
            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                Try
                    tList.AddRange(Directory.GetFiles(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName))
                Catch
                End Try
                fList.AddRange(tList.Cast(Of String)().Select(Function(AL) AL.ToLower).ToArray)

                If bSingle AndAlso File.Exists(String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    ExtraPath = String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")
                End If
            Else
                Try
                    tList.AddRange(Directory.GetFiles(Directory.GetParent(sPath).FullName))
                Catch
                End Try
                fList.AddRange(tList.Cast(Of String)().Select(Function(AL) AL.ToLower).ToArray)
                If bSingle AndAlso File.Exists(String.Concat(Directory.GetParent(sPath).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    ExtraPath = String.Concat(Directory.GetParent(sPath).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")
                End If
            End If

            parPath = Directory.GetParent(sPath).FullName.ToLower
            tmpName = Path.Combine(parPath, StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath))).ToLower
            tmpNameNoStack = Path.Combine(parPath, Path.GetFileNameWithoutExtension(sPath)).ToLower

            'fanart
            Select Case True
                Case bSingle AndAlso eSettings.FanartJPG AndAlso fList.Contains(Path.Combine(parPath, "fanart.jpg"))
                    FanartPath = Path.Combine(parPath, "fanart.jpg")
                Case eSettings.MovieNameFanartJPG AndAlso fList.Contains(String.Concat(tmpNameNoStack, "-fanart.jpg"))
                    FanartPath = String.Concat(tmpNameNoStack, "-fanart.jpg")
                Case eSettings.MovieNameDotFanartJPG AndAlso fList.Contains(String.Concat(tmpNameNoStack, ".fanart.jpg"))
                    FanartPath = String.Concat(tmpNameNoStack, ".fanart.jpg")
                Case eSettings.MovieNameFanartJPG AndAlso fList.Contains(String.Concat(tmpName, "-fanart.jpg"))
                    FanartPath = String.Concat(tmpName, "-fanart.jpg")
                Case eSettings.MovieNameDotFanartJPG AndAlso fList.Contains(String.Concat(tmpName, ".fanart.jpg"))
                    FanartPath = String.Concat(tmpName, ".fanart.jpg")
                Case eSettings.MovieNameFanartJPG AndAlso fList.Contains(Path.Combine(parPath, "video_ts-fanart.jpg"))
                    FanartPath = Path.Combine(parPath, "video_ts-fanart.jpg")
                Case eSettings.MovieNameDotFanartJPG AndAlso fList.Contains(Path.Combine(parPath, "video_ts.fanart.jpg"))
                    FanartPath = Path.Combine(parPath, "video_ts.fanart.jpg")
            End Select

            'poster
            Select Case True
                Case bSingle AndAlso eSettings.MovieTBN AndAlso fList.Contains(Path.Combine(parPath, "movie.tbn"))
                    PosterPath = Path.Combine(parPath, "movie.tbn")
                Case bSingle AndAlso eSettings.PosterTBN AndAlso fList.Contains(Path.Combine(parPath, "poster.tbn"))
                    PosterPath = Path.Combine(parPath, "poster.tbn")
                Case bSingle AndAlso eSettings.MovieJPG AndAlso fList.Contains(Path.Combine(parPath, "movie.jpg"))
                    PosterPath = Path.Combine(parPath, "movie.jpg")
                Case eSettings.MovieNameTBN AndAlso fList.Contains(String.Concat(tmpNameNoStack, ".tbn"))
                    PosterPath = String.Concat(tmpNameNoStack, ".tbn")
                Case eSettings.MovieNameTBN AndAlso fList.Contains(String.Concat(tmpName, ".tbn"))
                    PosterPath = String.Concat(tmpName, ".tbn")
                Case bSingle AndAlso eSettings.PosterJPG AndAlso fList.Contains(Path.Combine(parPath, "poster.jpg"))
                    PosterPath = Path.Combine(parPath, "poster.jpg")
                Case eSettings.MovieNameJPG AndAlso fList.Contains(String.Concat(tmpNameNoStack, ".jpg"))
                    PosterPath = String.Concat(tmpNameNoStack, ".jpg")
                Case eSettings.MovieNameJPG AndAlso fList.Contains(String.Concat(tmpName, ".jpg"))
                    PosterPath = String.Concat(tmpName, ".jpg")
                Case eSettings.MovieNameTBN AndAlso fList.Contains(Path.Combine(parPath, "video_ts.tbn"))
                    PosterPath = Path.Combine(parPath, "video_ts.tbn")
                Case eSettings.MovieNameJPG AndAlso fList.Contains(Path.Combine(parPath, "video_ts.jpg"))
                    PosterPath = Path.Combine(parPath, "video_ts.jpg")
                Case bSingle AndAlso eSettings.FolderJPG AndAlso fList.Contains(Path.Combine(parPath, "folder.jpg"))
                    PosterPath = Path.Combine(parPath, "folder.jpg")
            End Select

            'nfo
            Select Case True
                Case bSingle AndAlso eSettings.MovieNFO AndAlso fList.Contains(Path.Combine(parPath, "movie.nfo"))
                    NfoPath = Path.Combine(parPath, "movie.nfo")
                Case eSettings.MovieNameNFO AndAlso fList.Contains(String.Concat(tmpNameNoStack, ".nfo"))
                    NfoPath = String.Concat(tmpNameNoStack, ".nfo")
                Case eSettings.MovieNameNFO AndAlso fList.Contains(String.Concat(tmpName, ".nfo"))
                    NfoPath = String.Concat(tmpName, ".nfo")
            End Select

            For Each t As String In fList
                If Regex.IsMatch(t, String.Concat("(i?)^", Regex.Escape(tmpNameNoStack), "(\.(.*?))?\.(sst|srt|sub|ssa|aqt|smi|sami|jss|mpl|rt|idx|ass)$")) OrElse _
                        Regex.IsMatch(t, String.Concat("(i?)^", Regex.Escape(tmpName), "(\.(.*?))?\.(sst|srt|sub|ssa|aqt|smi|sami|jss|mpl|rt|idx|ass)$")) Then
                    SubPath = t
                    Exit For
                End If
            Next

            For Each t As String In Master.eSettings.ValidExts
                Select Case True
                    Case fList.Contains(String.Concat(tmpNameNoStack, "-trailer", t))
                        TrailerPath = String.Concat(tmpNameNoStack, "-trailer", t)
                        Exit For
                    Case fList.Contains(String.Concat(tmpName, "-trailer", t))
                        TrailerPath = String.Concat(tmpName, "-trailer", t)
                        Exit For
                    Case fList.Contains(String.Concat(tmpNameNoStack, "[trailer]", t))
                        TrailerPath = String.Concat(tmpNameNoStack, "[trailer]", t)
                        Exit For
                    Case fList.Contains(String.Concat(tmpName, "[trailer]", t))
                        TrailerPath = String.Concat(tmpName, "[trailer]", t)
                        Exit For
                    Case bSingle AndAlso fList.Contains(Path.Combine(parPath, String.Concat("movie-trailer", t)))
                        TrailerPath = Path.Combine(parPath, String.Concat("movie-trailer", t))
                        Exit For
                    Case bSingle AndAlso fList.Contains(Path.Combine(parPath, String.Concat("movie[trailer]", t)))
                        TrailerPath = Path.Combine(parPath, String.Concat("movie[trailer]", t))
                        Exit For
                End Select
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
        For Each t As String In Master.eSettings.ValidExts
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
            Dim extraPath As String = Path.Combine(Directory.GetParent(sPath).FullName, "extrathumbs")

            If Directory.Exists(extraPath) Then

                Try
                    lThumbs.AddRange(Directory.GetFiles(extraPath, "thumb*.jpg"))
                Catch
                End Try

                If lThumbs.Count > 0 Then
                    iMod = Convert.ToInt32(Regex.Match(lThumbs.Item(lThumbs.Count - 1), "(\d+).jpg").Groups(1).ToString)
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
            Using SourceStream As FileStream = New FileStream(sPathFrom, FileMode.Open, FileAccess.Read)
                Using DestinationStream As FileStream = New FileStream(sPathTo, FileMode.Create, FileAccess.Write)
                    Dim StreamBuffer(SourceStream.Length - 1) As Byte

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
        Dim fList As New ArrayList
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

            If isCleaner And Master.eSettings.ExpertCleaner Then

                For Each sFile As FileInfo In ioFi
                    Dim test As String = sFile.Extension
                    If Not Master.eSettings.CleanWhitelistExts.Contains(sFile.Extension) AndAlso ((Master.eSettings.CleanWhitelistVideo AndAlso Not Master.eSettings.ValidExts.Contains(sFile.Extension)) OrElse Not Master.eSettings.CleanWhitelistVideo) Then
                        File.Delete(sFile.FullName)
                        bReturn = True
                    End If
                Next

            Else

                If Not isCleaner Then
                    Dim Fanart As New Images
                    Dim fPath As String = mMovie.FanartPath
                    Dim tPath As String = String.Empty
                    If Not String.IsNullOrEmpty(fPath) Then
                        If Directory.GetParent(fPath).Name.ToLower = "video_ts" Then
                            If Master.eSettings.VideoTSParent Then
                                tPath = Path.Combine(Master.eSettings.BDPath, String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(fPath).FullName).FullName, Directory.GetParent(Directory.GetParent(fPath).FullName).Name), "-fanart.jpg"))
                            Else
                                If Path.GetFileName(fPath).ToLower = "fanart.jpg" Then
                                    tPath = Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(fPath).FullName).Name, "-fanart.jpg"))
                                Else
                                    tPath = Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath))
                                End If
                            End If
                        Else
                            If Path.GetFileName(fPath).ToLower = "fanart.jpg" Then
                                tPath = Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(mMovie.Filename), "-fanart.jpg"))
                            Else
                                tPath = Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath))
                            End If
                        End If
                    End If
                    If Not String.IsNullOrEmpty(tPath) AndAlso File.Exists(tPath) Then
                        File.Delete(tPath)
                    End If
                    Fanart = Nothing
                End If

                If Not isCleaner AndAlso mMovie.isSingle Then
                    If Directory.GetParent(mMovie.Filename).Name.ToLower = "video_ts" Then
                        DeleteDirectory(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName)
                    Else
                        DeleteDirectory(Directory.GetParent(mMovie.Filename).FullName)
                    End If
                Else
                    For Each lFI As FileInfo In ioFi
                        fList.Add(lFI.FullName)
                    Next

                    If (Master.eSettings.CleanFolderJPG AndAlso isCleaner) Then
                        If fList.Contains(Path.Combine(sPathShort, "folder.jpg")) Then
                            File.Delete(Path.Combine(sPathShort, "folder.jpg"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanFanartJPG AndAlso isCleaner) Then
                        If fList.Contains(Path.Combine(sPathShort, "fanart.jpg")) Then
                            File.Delete(Path.Combine(sPathShort, "fanart.jpg"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanMovieTBN AndAlso isCleaner) Then
                        If fList.Contains(Path.Combine(sPathShort, "movie.tbn")) Then
                            File.Delete(Path.Combine(sPathShort, "movie.tbn"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanMovieNFO AndAlso isCleaner) Then
                        If fList.Contains(Path.Combine(sPathShort, "movie.nfo")) Then
                            File.Delete(Path.Combine(sPathShort, "movie.nfo"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanPosterTBN AndAlso isCleaner) Then
                        If fList.Contains(Path.Combine(sPathShort, "poster.tbn")) Then
                            File.Delete(Path.Combine(sPathShort, "poster.tbn"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanPosterJPG AndAlso isCleaner) Then
                        If fList.Contains(Path.Combine(sPathShort, "poster.jpg")) Then
                            File.Delete(Path.Combine(sPathShort, "poster.jpg"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanMovieJPG AndAlso isCleaner) Then
                        If fList.Contains(Path.Combine(sPathShort, "movie.jpg")) Then
                            File.Delete(Path.Combine(sPathShort, "movie.jpg"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanExtraThumbs AndAlso isCleaner) Then
                        If Directory.Exists(Path.Combine(sPathShort, "extrathumbs")) Then
                            DeleteDirectory(Path.Combine(sPathShort, "extrathumbs"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanMovieTBNB AndAlso isCleaner) OrElse (Not isCleaner) Then
                        If fList.Contains(String.Concat(sPathNoExt, ".tbn")) Then
                            File.Delete(String.Concat(sPathNoExt, ".tbn"))
                            bReturn = True
                        End If
                        If fList.Contains(Path.Combine(sPathShort, "video_ts.tbn")) Then
                            File.Delete(Path.Combine(sPathShort, "video_ts.tbn"))
                            bReturn = True
                        End If
                        If fList.Contains(String.Concat(Path.Combine(sPathShort, sOrName), ".tbn")) Then
                            File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".tbn"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanMovieFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                        If fList.Contains(String.Concat(sPathNoExt, "-fanart.jpg")) Then
                            File.Delete(String.Concat(sPathNoExt, "-fanart.jpg"))
                            bReturn = True
                        End If
                        If fList.Contains(Path.Combine(sPathShort, "video_ts-fanart.jpg")) Then
                            File.Delete(Path.Combine(sPathShort, "video_ts-fanart.jpg"))
                            bReturn = True
                        End If
                        If fList.Contains(String.Concat(Path.Combine(sPathShort, sOrName), "-fanart.jpg")) Then
                            File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), "-fanart.jpg"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanMovieNFOB AndAlso isCleaner) OrElse (Not isCleaner) Then
                        If fList.Contains(String.Concat(sPathNoExt, ".nfo")) Then
                            File.Delete(String.Concat(sPathNoExt, ".nfo"))
                            bReturn = True
                        End If
                        If fList.Contains(Path.Combine(sPathShort, "video_ts.nfo")) Then
                            File.Delete(Path.Combine(sPathShort, "video_ts.nfo"))
                            bReturn = True
                        End If
                        If fList.Contains(String.Concat(Path.Combine(sPathShort, sOrName), ".nfo")) Then
                            File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".nfo"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanDotFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                        If fList.Contains(String.Concat(sPathNoExt, ".fanart.jpg")) Then
                            File.Delete(String.Concat(sPathNoExt, ".fanart.jpg"))
                            bReturn = True
                        End If
                        If fList.Contains(Path.Combine(sPathShort, "video_ts.fanart.jpg")) Then
                            File.Delete(Path.Combine(sPathShort, "video_ts.fanart.jpg"))
                            bReturn = True
                        End If
                        If fList.Contains(String.Concat(Path.Combine(sPathShort, sOrName), ".fanart.jpg")) Then
                            File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".fanart.jpg"))
                            bReturn = True
                        End If
                    End If

                    If (Master.eSettings.CleanMovieNameJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                        If fList.Contains(String.Concat(sPathNoExt, ".jpg")) Then
                            File.Delete(String.Concat(sPathNoExt, ".jpg"))
                            bReturn = True
                        End If
                        If fList.Contains(Path.Combine(sPathShort, "video_ts.jpg")) Then
                            File.Delete(Path.Combine(sPathShort, "video_ts.jpg"))
                            bReturn = True
                        End If
                        If fList.Contains(String.Concat(Path.Combine(sPathShort, sOrName), ".jpg")) Then
                            File.Delete(String.Concat(Path.Combine(sPathShort, sOrName), ".jpg"))
                            bReturn = True
                        End If
                    End If

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
                End If
            End If

            ioFi = Nothing
            dirInfo = Nothing
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return bReturn
    End Function

    Public Shared Function CreateRandomThumbs(ByVal mMovie As DBMovie, ByVal ThumbCount As Integer) As String

        Dim SetFA As String = String.Empty

        Try
            Dim pExt As String = Path.GetExtension(mMovie.Filename).ToLower
            If Not pExt = ".rar" AndAlso Not pExt = ".iso" AndAlso Not pExt = ".img" AndAlso _
            Not pExt = ".bin" AndAlso Not pExt = ".cue" Then

                Using ffmpeg As New Process()
                    Dim intSeconds As Integer = 0
                    Dim intAdd As Integer = 0
                    Dim tPath As String = String.Empty
                    Dim exImage As New Images

                    If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(mMovie.Filename).Name.ToLower = "video_ts" Then
                        tPath = Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, "extrathumbs")
                    Else
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "extrathumbs")
                    End If

                    If Not Directory.Exists(tPath) Then
                        Directory.CreateDirectory(tPath)
                    End If

                    ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
                    ffmpeg.EnableRaisingEvents = False
                    ffmpeg.StartInfo.UseShellExecute = False
                    ffmpeg.StartInfo.CreateNoWindow = True
                    ffmpeg.StartInfo.RedirectStandardOutput = True
                    ffmpeg.StartInfo.RedirectStandardError = True

                    'first get the duration
                    ffmpeg.StartInfo.Arguments = String.Format("-i ""{0}"" -an", mMovie.Filename)
                    ffmpeg.Start()
                    Dim d As StreamReader = ffmpeg.StandardError
                    Do
                        Dim s As String = d.ReadLine()
                        If s.Contains("Duration: ") Then
                            Dim sTime As String = Regex.Match(s, "Duration: (?<dur>.*?),").Groups("dur").ToString
                            If Not sTime = "N/A" Then
                                Dim ts As TimeSpan = CDate(CDate(String.Format("{0} {1}", DateTime.Today.ToString("d"), sTime))).Subtract(CDate(DateTime.Today))
                                intSeconds = ((ts.Hours * 60) + ts.Minutes) * 60 + ts.Seconds
                            End If
                        End If
                    Loop While Not d.EndOfStream

                    ffmpeg.WaitForExit()
                    ffmpeg.Close()

                    If intSeconds > 0 AndAlso ((Master.eSettings.AutoThumbsNoSpoilers AndAlso intSeconds / 2 > ThumbCount + 300) OrElse (Not Master.eSettings.AutoThumbsNoSpoilers AndAlso intSeconds > ThumbCount + 2)) Then
                        If Master.eSettings.AutoThumbsNoSpoilers Then
                            intSeconds = ((intSeconds / 2) - 300) / ThumbCount
                            intAdd = intSeconds
                            intSeconds += intAdd + 300
                        Else
                            intSeconds = intSeconds / (ThumbCount + 2)
                            intAdd = intSeconds
                            intSeconds += intAdd
                        End If

                        For i = 0 To (ThumbCount - 1)
                            'check to see if file already exists... if so, don't bother running ffmpeg since we're not
                            'overwriting current thumbs anyway
                            If Not File.Exists(Path.Combine(tPath, String.Concat("thumb", (i + 1), ".jpg"))) Then
                                ffmpeg.StartInfo.Arguments = String.Format("-ss {0} -i ""{1}"" -an -f rawvideo -vframes 1 -vcodec mjpeg ""{2}""", intSeconds, mMovie.Filename, Path.Combine(tPath, String.Concat("thumb", (i + 1), ".jpg")))
                                ffmpeg.Start()
                                ffmpeg.WaitForExit()
                                ffmpeg.Close()

                                exImage = New Images
                                exImage.ResizeExtraThumb(Path.Combine(tPath, String.Concat("thumb", (i + 1), ".jpg")), Path.Combine(tPath, String.Concat("thumb", (i + 1), ".jpg")))
                                exImage.Dispose()
                                exImage = Nothing

                            End If
                            intSeconds += intAdd
                        Next
                    End If

                    Dim fThumbs As New ArrayList
                    Try
                        fThumbs.AddRange(Directory.GetFiles(tPath, "thumb*.jpg"))
                    Catch
                    End Try

                    If fThumbs.Count <= 0 Then
                        DeleteDirectory(tPath)
                    Else
                        Dim exFanart As New Images
                        'always set to something if extrathumbs are created so we know during scrapers
                        SetFA = "TRUE"
                        If Master.eSettings.UseETasFA AndAlso String.IsNullOrEmpty(mMovie.FanartPath) Then
                            exFanart.FromFile(Path.Combine(tPath, "thumb1.jpg"))
                            SetFA = exFanart.SaveAsFanart(mMovie)
                        End If
                        exFanart.Dispose()
                        exFanart = Nothing
                    End If

                End Using

            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return SetFA
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
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
                    sVersion = tempKey.GetValue("Version")
                    If Not String.IsNullOrEmpty(sVersion) Then
                        Dim tVersion() As String = sVersion.Split(New Char() {"."})
                        If tVersion(0) >= 3 AndAlso tVersion(1) >= 5 Then
                            Return True
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

End Class
