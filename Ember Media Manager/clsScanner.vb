'################################################################################
'#                             EMBER MEDIA MANAGER                              #
'################################################################################
'################################################################################
'# This file is part of Ember Media Manager.                                    #
'#                                                                              #
'# Ember Media Manager is free software: you can redistribute it and/or modify  #
'# it under the terms of the GNU General Public License as published by         #
'# the Free Software Foundation, either version 3 of the License, or            #
'# (at your option) any later version.                                          #
'#                                                                              #
'# Ember Media Manager is distributed in the hope that it will be useful,       #
'# but WITHOUT ANY WARRANTY; without even the implied warranty of               #
'# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
'# GNU General Public License for more details.                                 #
'#                                                                              #
'# You should have received a copy of the GNU General Public License            #
'# along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
'################################################################################

Imports System.IO
Imports System.Text.RegularExpressions

Public Class Scanner

    Public MediaList As New List(Of FileAndSource)
    Public MoviePaths As New List(Of String)
    Public TVPaths As New List(Of String)
    Public ShowPath As String = String.Empty

    Friend WithEvents bwFolderData As New System.ComponentModel.BackgroundWorker
    Friend WithEvents bwPrelim As New System.ComponentModel.BackgroundWorker

    Public Event ProgressUpdated(ByVal iPercent As Integer, ByVal sText As String)
    Public Event ScanningCompleted(ByVal iStatus As Integer, ByVal iMax As Integer)

    Private Structure Arguments
        Dim SourceName As String
    End Structure

    Public Enum MediaType As Integer
        Movie = 0
        TVShow = 1
    End Enum

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
        Dim _epposter As String
        Dim _epnfo As String
        Dim _showpath As String
        Dim _type As MediaType

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

        Public Property Type() As MediaType
            Get
                Return _type
            End Get
            Set(ByVal value As MediaType)
                _type = value
            End Set
        End Property

        Public Property EpPoster() As String
            Get
                Return _epposter
            End Get
            Set(ByVal value As String)
                _epposter = value
            End Set
        End Property

        Public Property EpNfo() As String
            Get
                Return _epnfo
            End Get
            Set(ByVal value As String)
                _epnfo = value
            End Set
        End Property

        Public Property ShowPath() As String
            Get
                Return _showpath
            End Get
            Set(ByVal value As String)
                _showpath = value
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
            _epposter = String.Empty
            _epnfo = String.Empty
            _showpath = String.Empty
            _type = MediaType.Movie
        End Sub
    End Class

    Public Sub Start(ByVal SourceName As String)
        Me.bwPrelim = New System.ComponentModel.BackgroundWorker
        Me.bwPrelim.WorkerReportsProgress = True
        Me.bwPrelim.WorkerSupportsCancellation = True
        Me.bwPrelim.RunWorkerAsync(New Arguments With {.SourceName = SourceName})
    End Sub

    Public Function IsBusy() As Boolean
        Return bwPrelim.IsBusy OrElse bwFolderData.IsBusy
    End Function

    Public Sub Cancel()
        If Me.bwPrelim.IsBusy Then Me.bwPrelim.CancelAsync()
        If Me.bwFolderData.IsBusy Then Me.bwFolderData.CancelAsync()
    End Sub

    Public Sub CancelAndWait()
        If bwPrelim.IsBusy Then bwPrelim.CancelAsync()
        If bwFolderData.IsBusy Then bwFolderData.CancelAsync()
        While bwPrelim.IsBusy OrElse bwFolderData.IsBusy
            Application.DoEvents()
        End While
    End Sub

    ''' <summary>
    ''' Get all directories/movies in the parent directory
    ''' </summary>
    ''' <param name="sSource">Name of source.</param>
    ''' <param name="sPath">Path of source.</param>
    ''' <param name="bRecur">Scan directory recursively?</param>
    ''' <param name="bUseFolder">Use the folder name for initial title? (else uses file name)</param>
    ''' <param name="bSingle">Only detect one movie from each folder?</param>
    Public Sub ScanSourceDir(ByVal sSource As String, ByVal sPath As String, ByVal bRecur As Boolean, ByVal bUseFolder As Boolean, ByVal bSingle As Boolean)

        Try
            Dim sMoviePath As String = String.Empty
            If Directory.Exists(sPath) Then

                'check if there are any movies in the parent folder
                ScanForFiles(sPath, sSource, bUseFolder, bSingle)

                Dim Dirs As New List(Of DirectoryInfo)
                Dim dInfo As New DirectoryInfo(sPath)

                Try
                    Dirs.AddRange(dInfo.GetDirectories)
                Catch
                End Try

                Dim upDir = From uD As DirectoryInfo In Dirs Where (Master.eSettings.IgnoreLastScan Or uD.LastWriteTime > Master.SourceLastScan) And isValidDir(uD.FullName)
                If upDir.Count > 0 Then
                    For Each inDir As DirectoryInfo In upDir
                        If Me.bwPrelim.CancellationPending Then Return

                        ScanForFiles(inDir.FullName, sSource, bUseFolder, bSingle)
                        If bRecur Then
                            ScanSourceDir(sSource, inDir.FullName, bRecur, bUseFolder, bSingle)
                        End If
                    Next
                End If

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Find all related files in a directory.
    ''' </summary>
    ''' <param name="sPath">Full path of the directory.</param>
    ''' <param name="sSource">Name of source.</param>
    ''' <param name="bUseFolder">Use the folder name for initial title? (else uses file name)</param>
    ''' <param name="bSingle">Only detect one movie from each folder?</param>
    Public Sub ScanForFiles(ByVal sPath As String, ByVal sSource As String, ByVal bUseFolder As Boolean, ByVal bSingle As Boolean)

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

                If Master.eSettings.AutoDetectVTS Then
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
                        If Me.bwPrelim.CancellationPending Then Return
                    Next
                End If

                If vtsSingle AndAlso Not String.IsNullOrEmpty(tFile) Then
                    If Not tmpList.Contains(tFile.ToLower) AndAlso Not MoviePaths.Contains(tFile.ToLower) AndAlso _
                    Not Path.GetFileName(tFile).ToLower.Contains("-trailer") AndAlso Not Path.GetFileName(tFile).ToLower.Contains("[trailer") AndAlso _
                    Not Path.GetFileName(tFile).ToLower.Contains("sample") Then
                        tmpList.Add(tFile.ToLower)
                        fList.Add(New FileAndSource With {.Filename = tFile, .Source = sSource, .isSingle = bSingle, .UseFolder = bUseFolder, .Contents = lFi, .Type = MediaType.Movie})
                    End If
                Else
                    lFi.Sort(AddressOf FileManip.Common.SortFileNames)

                    For Each lFile As FileInfo In lFi
                        If Master.eSettings.NoStackExts.Contains(lFile.Extension.ToLower) Then
                            tmpList.Add(lFile.FullName.ToLower)
                            SkipStack = True
                        Else
                            tmpList.Add(StringManip.CleanStackingMarkers(lFile.FullName).ToLower)
                        End If

                        If Not MoviePaths.Contains(lFile.FullName.ToLower) AndAlso Master.eSettings.ValidExts.Contains(lFile.Extension.ToLower) AndAlso Not tmpList.Contains(StringManip.CleanStackingMarkers(lFile.FullName).ToLower) AndAlso _
                            Not lFile.Name.ToLower.Contains("-trailer") AndAlso Not lFile.Name.ToLower.Contains("[trailer") AndAlso Not lFile.Name.ToLower.Contains("sample") AndAlso _
                            ((Master.eSettings.SkipStackSizeCheck AndAlso StringManip.IsStacked(lFile.Name)) OrElse lFile.Length >= Master.eSettings.SkipLessThan * 1048576) Then
                            fList.Add(New FileAndSource With {.Filename = lFile.FullName, .Source = sSource, .isSingle = bSingle, .UseFolder = If(bSingle, bUseFolder, False), .Contents = lFi, .Type = MediaType.Movie})
                            If bSingle AndAlso Not SkipStack Then Exit For
                        End If
                        If Me.bwPrelim.CancellationPending Then Return
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Check if we should scan the directory.
    ''' </summary>
    ''' <param name="sPath">Full path of the directory to check</param>
    ''' <returns>True if directory is valid, false if not.</returns>
    Public Function isValidDir(ByVal sPath As String) As Boolean

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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return False
        End Try
        Return True 'This is the Else
    End Function

    ''' <summary>
    ''' Check if there are movies in the subdirectorys of a path.
    ''' </summary>
    ''' <param name="MovieDir">DirectoryInfo object of directory to scan.</param>
    ''' <returns>True if the path's subdirectories contain movie files, else false.</returns>
    Public Function SubDirsHaveMovies(ByVal MovieDir As DirectoryInfo) As Boolean
        Try
            If Directory.Exists(MovieDir.FullName) Then

                Dim Dirs As New List(Of DirectoryInfo)

                Try
                    Dirs.AddRange(MovieDir.GetDirectories)
                Catch
                End Try

                For Each inDir As DirectoryInfo In Dirs
                    If isValidDir(inDir.FullName) Then
                        If ScanSubs(inDir) Then Return True
                        SubDirsHaveMovies(inDir)
                    End If
                Next
            End If
            Return False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Check if a path contains movies.
    ''' </summary>
    ''' <param name="inDir">DirectoryInfo object of directory to scan</param>
    ''' <returns>True if directory contains movie files.</returns>
    Public Function ScanSubs(ByVal inDir As DirectoryInfo) As Boolean
        Try
            Dim lFi As New List(Of FileInfo)

            Try
                lFi.AddRange(inDir.GetFiles)
            Catch
            End Try

            lFi.Sort(AddressOf FileManip.Common.SortFileNames)

            For Each lFile As FileInfo In lFi

                If Master.eSettings.ValidExts.Contains(lFile.Extension.ToLower) AndAlso _
                    Not lFile.Name.ToLower.Contains("-trailer") AndAlso Not lFile.Name.ToLower.Contains("[trailer") AndAlso _
                    Not lFile.Name.ToLower.Contains("sample") Then Return True

            Next

            Return False
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Check if a directory contains supporting files (nfo, poster, fanart, etc)
    ''' </summary>
    ''' <param name="sPath">Full path to directory.</param>
    ''' <param name="bSingle">Only check for <movie> type files if there could be more than one movie in the directory, else check for all types.</param>
    ''' <returns>String array (0 = poster, 1 = fanart, 2 = nfo, 3 = trailer, 4 = subtitle, 5 = extrathumbs)</returns>
    Public Function GetFolderContents(ByVal sPath As String, ByVal bSingle As Boolean) As String()

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
        Dim isYAMJ As Boolean = False

        Try
            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                isYAMJ = True

                Try
                    fList.AddRange(Directory.GetFiles(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName))
                Catch
                End Try

                parPath = Directory.GetParent(Directory.GetParent(sPath).FullName).FullName.ToLower
                tmpName = Path.Combine(parPath, StringManip.CleanStackingMarkers(Directory.GetParent(Directory.GetParent(sPath).FullName).Name)).ToLower
                tmpNameNoStack = Path.Combine(parPath, Directory.GetParent(Directory.GetParent(sPath).FullName).Name).ToLower

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

                parPath = Directory.GetParent(sPath).FullName.ToLower
                tmpName = Path.Combine(parPath, StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath))).ToLower
                tmpNameNoStack = Path.Combine(parPath, Path.GetFileNameWithoutExtension(sPath)).ToLower


                If bSingle AndAlso File.Exists(String.Concat(Directory.GetParent(sPath).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                    ExtraPath = String.Concat(Directory.GetParent(sPath).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")
                End If
            End If

            For Each fFile As String In fList
                'fanart
                If String.IsNullOrEmpty(FanartPath) Then
                    If (bSingle AndAlso Master.eSettings.FanartJPG AndAlso fFile.ToLower = Path.Combine(parPath, "fanart.jpg")) _
                        OrElse ((Not bSingle OrElse Not Master.eSettings.MovieNameMultiOnly) AndAlso _
                                ((Master.eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, "-fanart.jpg")) _
                                OrElse (Master.eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = String.Concat(tmpName, "-fanart.jpg")) _
                                OrElse (Master.eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts-fanart.jpg")) _
                                OrElse (Master.eSettings.MovieNameDotFanartJPG AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.fanart.jpg")))) _
                        OrElse ((Not bSingle OrElse isYAMJ OrElse Not Master.eSettings.MovieNameMultiOnly) AndAlso _
                                (((Master.eSettings.MovieNameDotFanartJPG OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpName, ".fanart.jpg")) _
                                OrElse ((Master.eSettings.MovieNameDotFanartJPG OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".fanart.jpg")))) Then

                        FanartPath = fFile
                        Continue For
                    End If
                End If

                'poster
                If String.IsNullOrEmpty(PosterPath) Then
                    If (bSingle AndAlso (Master.eSettings.MovieTBN AndAlso fFile.ToLower = Path.Combine(parPath, "movie.tbn")) _
                                OrElse (Master.eSettings.PosterTBN AndAlso fFile.ToLower = Path.Combine(parPath, "poster.tbn")) _
                                OrElse (Master.eSettings.MovieJPG AndAlso fFile.ToLower = Path.Combine(parPath, "movie.jpg")) _
                                OrElse (Master.eSettings.PosterJPG AndAlso fFile.ToLower = Path.Combine(parPath, "poster.jpg")) _
                                OrElse (Master.eSettings.FolderJPG AndAlso fFile.ToLower = Path.Combine(parPath, "folder.jpg"))) _
                        OrElse ((Not bSingle OrElse Not Master.eSettings.MovieNameMultiOnly) AndAlso _
                                ((Master.eSettings.MovieNameTBN AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.tbn")) _
                                OrElse (Master.eSettings.MovieNameJPG AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.jpg")))) _
                        OrElse ((Not bSingle OrElse isYAMJ OrElse Not Master.eSettings.MovieNameMultiOnly) AndAlso _
                                (((Master.eSettings.MovieNameTBN OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".tbn")) _
                                OrElse ((Master.eSettings.MovieNameTBN OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpName, ".tbn")) _
                                OrElse ((Master.eSettings.MovieNameJPG OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".jpg")) _
                                OrElse ((Master.eSettings.MovieNameJPG OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpName, ".jpg")))) Then
                        PosterPath = fFile
                        Continue For
                    End If
                End If

                'nfo
                If String.IsNullOrEmpty(NfoPath) Then
                    If (bSingle AndAlso Master.eSettings.MovieNFO AndAlso fFile.ToLower = Path.Combine(parPath, "movie.nfo")) _
                    OrElse ((Not bSingle OrElse isYAMJ OrElse Not Master.eSettings.MovieNameMultiOnly) AndAlso _
                    (((Master.eSettings.MovieNameNFO OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".nfo")) _
                    OrElse ((Master.eSettings.MovieNameNFO OrElse isYAMJ) AndAlso fFile.ToLower = String.Concat(tmpName, ".nfo")))) Then
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
                    For Each t As String In Master.eSettings.ValidExts
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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return aResults
    End Function

    ''' <summary>
    ''' Check if a directory contains supporting files (nfo, poster)
    ''' </summary>
    ''' <param name="sPath">Full path to directory.</param>
    ''' <returns>String array (0 = poster, 1 = nfo)</returns>
    Public Function GetEpFolderContents(ByVal sPath As String) As String()

        Dim NfoPath As String = String.Empty
        Dim PosterPath As String = String.Empty
        Dim aResults(2) As String
        Dim parPath As String = String.Empty
        Dim tmpName As String = String.Empty
        Dim fList As New List(Of String)

        Try
            Try
                fList.AddRange(Directory.GetFiles(Directory.GetParent(sPath).FullName, String.Concat(Path.GetFileNameWithoutExtension(sPath), ".*")))
            Catch
            End Try

            parPath = Directory.GetParent(sPath).FullName.ToLower
            tmpName = Path.Combine(parPath, Path.GetFileNameWithoutExtension(sPath)).ToLower

            Dim pFile = From fFiles As String In fList Where fFiles.ToLower = String.Concat(tmpName, ".tbn")
            If pFile.count > 0 Then
                PosterPath = pFile(0).ToString
            End If

            Dim nFile = From fFiles As String In fList Where fFiles.ToLower = String.Concat(tmpName, ".nfo")
            If nFile.Count > 0 Then
                NfoPath = nFile(0).ToString
            End If

            aResults(0) = PosterPath
            aResults(1) = NfoPath
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return aResults
    End Function

    ''' <summary>
    ''' Check if a directory contains supporting files (nfo, poster)
    ''' </summary>
    ''' <param name="sPath">Full path to directory.</param>
    ''' <returns>String array (0 = poster, 1 = nfo, 2 = fanart)</returns>
    Public Function GetShowFolderContents(ByVal sPath As String) As String()

        Dim NfoPath As String = String.Empty
        Dim PosterPath As String = String.Empty
        Dim FanartPath As String = String.Empty
        Dim aResults(3) As String
        Dim parPath As String = String.Empty
        Dim tmpName As String = String.Empty
        Dim fList As New List(Of String)

        Try
            Try
                fList.AddRange(Directory.GetFiles(sPath))
            Catch
            End Try

            Dim pFile = From fFiles As String In fList Where fFiles.ToLower = Path.Combine(sPath.ToLower, "season-all.tbn")
            If pFile.Count > 0 Then
                PosterPath = pFile(0).ToString
            End If

            Dim nFile = From fFiles As String In fList Where fFiles.ToLower = Path.Combine(sPath.ToLower, "tvshow.nfo")
            If nFile.Count > 0 Then
                NfoPath = nFile(0).ToString
            End If

            Dim fFile = From fFiles As String In fList Where fFiles.ToLower = Path.Combine(sPath.ToLower, "fanart.jpg")
            If fFile.Count > 0 Then
                FanartPath = fFile(0).ToString
            End If

            aResults(0) = PosterPath
            aResults(1) = NfoPath
            aResults(2) = FanartPath

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return aResults
    End Function

    ''' <summary>
    ''' Get the full path to a trailer, if it exists.
    ''' </summary>
    ''' <param name="sPath">Full path to a movie file for which you are trying to find the accompanying trailer.</param>
    ''' <returns>Full path of trailer file.</returns>
    Public Function GetTrailerPath(ByVal sPath As String) As String

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

    ''' <summary>
    ''' Get all directories in the parent directory
    ''' </summary>
    ''' <param name="sSource">Name of source.</param>
    ''' <param name="sPath">Path of source.</param>
    Public Sub ScanTVSourceDir(ByVal sSource As String, ByVal sPath As String, Optional ByVal isInner As Boolean = False)

        Try
            Dim sTVEpPath As String = String.Empty
            If Directory.Exists(sPath) Then

                Dim Dirs As New List(Of DirectoryInfo)
                Dim dInfo As New DirectoryInfo(sPath)

                Try
                    Dirs.AddRange(dInfo.GetDirectories)
                Catch
                End Try

                Dim upDir = From uD As DirectoryInfo In Dirs Where (Master.eSettings.IgnoreLastScan Or uD.LastWriteTime > Master.SourceLastScan) And isValidDir(uD.FullName)
                If upDir.Count > 0 Then
                    For Each inDir As DirectoryInfo In upDir
                        If Not isInner Then Me.ShowPath = inDir.FullName
                        ScanForTVFiles(inDir.FullName, sSource)
                        'If Regex.IsMatch(inDir.Name, "(s(eason)?)?([\._ ])?([0-9]+)", RegexOptions.IgnoreCase) Then
                        ScanTVSourceDir(sSource, inDir.FullName, True)
                    Next
                End If

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    ''' <summary>
    ''' Find all related files in a directory.
    ''' </summary>
    ''' <param name="sPath">Full path of the directory.</param>
    ''' <param name="sSource">Name of source.</param>
    Public Sub ScanForTVFiles(ByVal sPath As String, ByVal sSource As String)

        Try

            Dim di As DirectoryInfo
            Dim lFi As New List(Of FileInfo)
            Dim fList As New List(Of FileAndSource)
            Dim tFile As String = String.Empty

            di = New DirectoryInfo(sPath)

            Try
                lFi.AddRange(di.GetFiles())
            Catch
            End Try

            If lFi.Count > 0 Then

                lFi.Sort(AddressOf FileManip.Common.SortFileNames)

                For Each lFile As FileInfo In lFi
                    If Not TVPaths.Contains(lFile.FullName.ToLower) AndAlso Master.eSettings.ValidExts.Contains(lFile.Extension.ToLower) AndAlso _
                        Not lFile.Name.ToLower.Contains("-trailer") AndAlso Not lFile.Name.ToLower.Contains("[trailer") AndAlso Not lFile.Name.ToLower.Contains("sample") AndAlso _
                        lFile.Length >= Master.eSettings.SkipLessThan * 1048576 Then
                        fList.Add(New FileAndSource With {.Filename = lFile.FullName, .Source = sSource, .Contents = lFi, .Type = MediaType.TVShow, .ShowPath = ShowPath})
                    End If
                Next

                Me.MediaList.AddRange(fList)

                fList = Nothing
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwPrelim_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwPrelim.DoWork

        Try
            Dim Args As Arguments = DirectCast(e.Argument, Arguments)

            Master.DB.SaveMovieList()

            MoviePaths.Clear()
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "SELECT MoviePath FROM movies;"
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    While SQLreader.Read
                        MoviePaths.Add(SQLreader("MoviePath").ToString.ToLower)
                        If Me.bwPrelim.CancellationPending Then
                            e.Cancel = True
                            Return
                        End If
                    End While
                End Using
            End Using

            TVPaths.Clear()
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                SQLcommand.CommandText = "SELECT TVEpPath FROM TVEps;"
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    While SQLreader.Read
                        TVPaths.Add(SQLreader("TVEpPath").ToString.ToLower)
                        If Me.bwPrelim.CancellationPending Then
                            e.Cancel = True
                            Return
                        End If
                    End While
                End Using
            End Using

            Me.MediaList.Clear()
            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                If Not String.IsNullOrEmpty(Args.SourceName) Then
                    SQLcommand.CommandText = String.Format("SELECT * FROM sources WHERE Name = ""{0}"";", Args.SourceName)
                Else
                    SQLcommand.CommandText = "SELECT * FROM sources;"
                End If

                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Using SQLUpdatecommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                        SQLUpdatecommand.CommandText = "UPDATE sources SET LastScan = (?) WHERE ID = (?);"
                        Dim parLastScan As SQLite.SQLiteParameter = SQLUpdatecommand.Parameters.Add("parLastScan", DbType.String, 0, "LastScan")
                        Dim parID As SQLite.SQLiteParameter = SQLUpdatecommand.Parameters.Add("parID", DbType.Int32, 0, "ID")
                        While SQLreader.Read
                            Master.SourceLastScan = Convert.ToDateTime(SQLreader("LastScan").ToString)
                            If Convert.ToBoolean(SQLreader("Recursive")) OrElse (Master.eSettings.IgnoreLastScan OrElse Directory.GetLastWriteTime(SQLreader("Path").ToString) > Master.SourceLastScan) Then
                                'save the scan time back to the db
                                parLastScan.Value = Now
                                parID.Value = SQLreader("ID")
                                SQLUpdatecommand.ExecuteNonQuery()
                                ScanSourceDir(SQLreader("Name").ToString, SQLreader("Path").ToString, Convert.ToBoolean(SQLreader("Recursive")), Convert.ToBoolean(SQLreader("Foldername")), Convert.ToBoolean(SQLreader("Single")))
                            End If
                            If Me.bwPrelim.CancellationPending Then
                                e.Cancel = True
                                Return
                            End If
                        End While
                    End Using
                End Using
            End Using

            Using SQLcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                If Not String.IsNullOrEmpty(Args.SourceName) Then
                    SQLcommand.CommandText = String.Format("SELECT * FROM TVSources WHERE Name = ""{0}"";", Args.SourceName)
                Else
                    SQLcommand.CommandText = "SELECT * FROM TVSources;"
                End If

                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Using SQLUpdatecommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
                        SQLUpdatecommand.CommandText = "UPDATE TVSources SET LastScan = (?) WHERE ID = (?);"
                        Dim parLastScan As SQLite.SQLiteParameter = SQLUpdatecommand.Parameters.Add("parLastScan", DbType.String, 0, "LastScan")
                        Dim parID As SQLite.SQLiteParameter = SQLUpdatecommand.Parameters.Add("parID", DbType.Int32, 0, "ID")
                        While SQLreader.Read
                            Master.SourceLastScan = Convert.ToDateTime(SQLreader("LastScan").ToString)
                            'save the scan time back to the db
                            parLastScan.Value = Now
                            parID.Value = SQLreader("ID")
                            SQLUpdatecommand.ExecuteNonQuery()
                            ScanTVSourceDir(SQLreader("Name").ToString, SQLreader("Path").ToString)
                            If Me.bwPrelim.CancellationPending Then
                                e.Cancel = True
                                Return
                            End If
                        End While
                    End Using
                End Using
            End Using

            'remove any db entries that no longer exist
            If Master.eSettings.CleanDB Then Master.DB.Clean()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub bwPrelim_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwPrelim.RunWorkerCompleted

        '//
        ' Thread finished: set up progress bar, display count, and begin thread to load data
        '\\
        If Not e.Cancelled Then
            Try
                If MediaList.Count = 0 Then
                    RaiseEvent ScanningCompleted(0, 0)
                Else
                    RaiseEvent ScanningCompleted(1, MediaList.Count + 1)

                    Me.bwFolderData = New System.ComponentModel.BackgroundWorker
                    Me.bwFolderData.WorkerReportsProgress = True
                    Me.bwFolderData.WorkerSupportsCancellation = True
                    Me.bwFolderData.RunWorkerAsync()
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If
    End Sub

    Private Sub bwFolderData_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwFolderData.DoWork

        '//
        ' Thread to fill a datatable with basic media data
        '\\
        Dim currentIndex As Integer = 0
        Dim tmpMovieDB As New Master.DBMovie
        Dim tmpTVDB As New Master.DBTV
        Dim aContents(6) As String

        Try
            tmpMovieDB.Movie = New Media.Movie
            tmpTVDB.TVEp = New Media.EpisodeDetails
            tmpTVDB.TVShow = New Media.TVShow

            'process the folder type media
            Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction
                For Each sFile As FileAndSource In MediaList
                    If Me.bwFolderData.CancellationPending Then
                        e.Cancel = True
                        Return
                    End If
                    Select Case sFile.Type
                        Case MediaType.TVShow
                            If Not String.IsNullOrEmpty(sFile.Filename) Then
                                aContents = GetShowFolderContents(sFile.ShowPath)
                                sFile.Poster = aContents(0)
                                sFile.Nfo = aContents(1)
                                sFile.Fanart = aContents(2)

                                If Not String.IsNullOrEmpty(sFile.Nfo) Then
                                    tmpTVDB.TVShow = NFO.LoadTVShowFromNFO(sFile.Nfo)
                                Else
                                    tmpTVDB.TVShow = NFO.LoadTVShowFromNFO(sFile.ShowPath)
                                End If

                                If String.IsNullOrEmpty(tmpTVDB.TVShow.Title) Then
                                    'no title so assume it's an invalid nfo, clear nfo path if exists
                                    sFile.Nfo = String.Empty

                                    'set title based on show folder name
                                End If

                                aContents = GetEpFolderContents(sFile.Filename)
                                sFile.EpPoster = aContents(0)
                                sFile.EpNfo = aContents(1)

                                If Not String.IsNullOrEmpty(sFile.EpNfo) Then
                                    tmpTVDB.TVEp = NFO.LoadTVEpFromNFO(sFile.EpNfo)
                                Else
                                    tmpTVDB.TVEp = NFO.LoadTVEpFromNFO(sFile.Filename)
                                End If

                                If String.IsNullOrEmpty(tmpTVDB.TVEp.Title) Then
                                    'no title so assume it's an invalid nfo, clear nfo path if exists
                                    sFile.EpNfo = String.Empty

                                    'set title based on episode file
                                End If

                                Me.bwFolderData.ReportProgress(currentIndex, tmpTVDB.TVEp.Title)
                                tmpTVDB.ShowNfoPath = sFile.Nfo
                                tmpTVDB.ShowPosterPath = sFile.Poster
                                tmpTVDB.ShowFanartPath = sFile.Fanart
                                tmpTVDB.IsNewShow = True
                                tmpTVDB.IsLockShow = False
                                tmpTVDB.IsMarkShow = Master.eSettings.MarkNew
                                tmpTVDB.EpNfoPath = sFile.EpNfo
                                tmpTVDB.EpPosterPath = sFile.EpPoster
                                tmpTVDB.Filename = sFile.Filename
                                tmpTVDB.Source = sFile.Source
                                tmpTVDB.IsNewEp = True
                                tmpTVDB.IsLockEp = False
                                tmpTVDB.IsMarkEp = Master.eSettings.MarkNew
                                'Do the Save
                                tmpTVDB = Master.DB.SaveTVShowToDB(tmpTVDB, True, True)
                                If Not String.IsNullOrEmpty(tmpTVDB.ShowID.ToString) Then
                                    tmpTVDB = Master.DB.SaveTVEpToDB(tmpTVDB, True, True)
                                End If
                                currentIndex += 1
                            End If
                        Case Else 'assume movie
                                If Not String.IsNullOrEmpty(sFile.Filename) Then
                                    'first, lets get the contents
                                    aContents = GetFolderContents(sFile.Filename, sFile.isSingle)
                                    sFile.Poster = aContents(0)
                                    sFile.Fanart = aContents(1)
                                    sFile.Nfo = aContents(2)
                                    sFile.Trailer = aContents(3)
                                    sFile.Subs = aContents(4)
                                    sFile.Extra = aContents(5)

                                    If Not String.IsNullOrEmpty(sFile.Nfo) Then
                                        tmpMovieDB.Movie = NFO.LoadMovieFromNFO(sFile.Nfo, sFile.isSingle)
                                    Else
                                        tmpMovieDB.Movie = NFO.LoadMovieFromNFO(sFile.Filename, sFile.isSingle)
                                    End If

                                    If String.IsNullOrEmpty(tmpMovieDB.Movie.Title) Then
                                        'no title so assume it's an invalid nfo, clear nfo path if exists
                                        sFile.Nfo = String.Empty

                                        If Directory.GetParent(sFile.Filename).Name.ToLower = "video_ts" Then
                                            tmpMovieDB.ListTitle = StringManip.FilterName(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name)
                                            tmpMovieDB.Movie.Title = StringManip.FilterName(Directory.GetParent(Directory.GetParent(sFile.Filename).FullName).Name, False)
                                        Else
                                            If sFile.UseFolder AndAlso sFile.isSingle Then
                                                tmpMovieDB.ListTitle = StringManip.FilterName(Directory.GetParent(sFile.Filename).Name)
                                                tmpMovieDB.Movie.Title = StringManip.FilterName(Directory.GetParent(sFile.Filename).Name, False)
                                            Else
                                                tmpMovieDB.ListTitle = StringManip.FilterName(Path.GetFileNameWithoutExtension(sFile.Filename))
                                                tmpMovieDB.Movie.Title = StringManip.FilterName(Path.GetFileNameWithoutExtension(sFile.Filename), False)
                                            End If
                                        End If
                                        If String.IsNullOrEmpty(tmpMovieDB.Movie.SortTitle) Then tmpMovieDB.Movie.SortTitle = tmpMovieDB.ListTitle
                                    Else
                                        Dim tTitle As String = StringManip.FilterTokens(tmpMovieDB.Movie.Title)
                                        If String.IsNullOrEmpty(tmpMovieDB.Movie.SortTitle) Then tmpMovieDB.Movie.SortTitle = tTitle
                                        If Master.eSettings.DisplayYear AndAlso Not String.IsNullOrEmpty(tmpMovieDB.Movie.Year) Then
                                            tmpMovieDB.ListTitle = String.Format("{0} ({1})", tTitle, tmpMovieDB.Movie.Year)
                                        Else
                                            tmpMovieDB.ListTitle = StringManip.FilterTokens(tmpMovieDB.Movie.Title)
                                        End If
                                    End If

                                    Me.bwFolderData.ReportProgress(currentIndex, tmpMovieDB.ListTitle)
                                    If Not String.IsNullOrEmpty(tmpMovieDB.ListTitle) Then
                                        tmpMovieDB.NfoPath = sFile.Nfo
                                        tmpMovieDB.PosterPath = sFile.Poster
                                        tmpMovieDB.FanartPath = sFile.Fanart
                                        tmpMovieDB.TrailerPath = sFile.Trailer
                                        tmpMovieDB.SubPath = sFile.Subs
                                        tmpMovieDB.ExtraPath = sFile.Extra
                                        tmpMovieDB.Filename = sFile.Filename
                                        tmpMovieDB.isSingle = sFile.isSingle
                                        tmpMovieDB.UseFolder = sFile.UseFolder
                                        tmpMovieDB.Source = sFile.Source
                                        tmpMovieDB.FileSource = XML.GetFileSource(sFile.Filename)
                                        tmpMovieDB.IsNew = True
                                        tmpMovieDB.IsLock = False
                                        tmpMovieDB.IsMark = Master.eSettings.MarkNew
                                        'Do the Save
                                        tmpMovieDB = Master.DB.SaveMovieToDB(tmpMovieDB, True, True)
                                    End If
                                    currentIndex += 1
                                End If
                    End Select
                Next
                SQLtransaction.Commit()
            End Using
            If Me.bwFolderData.CancellationPending Then
                e.Cancel = True
                Return
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub bwFolderData_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwFolderData.ProgressChanged

        If Not bwFolderData.CancellationPending Then RaiseEvent ProgressUpdated(e.ProgressPercentage, e.UserState.ToString)

    End Sub

    Private Sub bwFolderData_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwFolderData.RunWorkerCompleted

        If Not e.Cancelled Then
            RaiseEvent ScanningCompleted(2, 0)
        End If

    End Sub

End Class