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

    ''' <summary>
    ''' Get all directories/movies in the parent directory
    ''' </summary>
    ''' <param name="sSource">Name of source.</param>
    ''' <param name="sPath">Path of source.</param>
    ''' <param name="bRecur">Scan directory recursively?</param>
    ''' <param name="bUseFolder">Use the folder name for initial title? (else uses file name)</param>
    ''' <param name="bSingle">Only detect one movie from each folder?</param>
    Public Shared Sub ScanSourceDir(ByVal sSource As String, ByVal sPath As String, ByVal bRecur As Boolean, ByVal bUseFolder As Boolean, ByVal bSingle As Boolean)

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
    Public Shared Sub ScanForFiles(ByVal sPath As String, ByVal sSource As String, ByVal bUseFolder As Boolean, ByVal bSingle As Boolean)

        Try

            Dim tmpList As New ArrayList
            Dim di As DirectoryInfo
            Dim lFi As New List(Of FileInfo)
            Dim SkipStack As Boolean = False
            Dim fList As New List(Of Master.FileAndSource)
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
                    Next
                End If

                If vtsSingle AndAlso Not String.IsNullOrEmpty(tFile) Then
                    If Not tmpList.Contains(tFile.ToLower) AndAlso Not Master.MoviePaths.Contains(tFile.ToLower) AndAlso _
                    Not Path.GetFileName(tFile).ToLower.Contains("-trailer") AndAlso Not Path.GetFileName(tFile).ToLower.Contains("[trailer") AndAlso _
                    Not Path.GetFileName(tFile).ToLower.Contains("sample") Then
                        tmpList.Add(tFile.ToLower)
                        fList.Add(New Master.FileAndSource With {.Filename = tFile, .Source = sSource, .isSingle = bSingle, .UseFolder = bUseFolder, .Contents = lFi})
                    End If
                Else
                    lFi.Sort(AddressOf FileManip.Common.SortFileNames)

                    For Each lFile As FileInfo In lFi
                        If Not Master.MoviePaths.Contains(lFile.FullName.ToLower) AndAlso Master.eSettings.ValidExts.Contains(lFile.Extension.ToLower) AndAlso Not tmpList.Contains(StringManip.CleanStackingMarkers(lFile.FullName).ToLower) AndAlso _
                            Not lFile.Name.ToLower.Contains("-trailer") AndAlso Not lFile.Name.ToLower.Contains("[trailer") AndAlso Not lFile.Name.ToLower.Contains("sample") AndAlso _
                            ((Master.eSettings.SkipStackSizeCheck AndAlso StringManip.IsStacked(lFile.Name)) OrElse lFile.Length >= Master.eSettings.SkipLessThan * 1048576) Then
                            If Master.eSettings.NoStackExts.Contains(lFile.Extension.ToLower) Then
                                tmpList.Add(lFile.FullName.ToLower)
                                SkipStack = True
                            Else
                                tmpList.Add(StringManip.CleanStackingMarkers(lFile.FullName).ToLower)
                            End If
                            fList.Add(New Master.FileAndSource With {.Filename = lFile.FullName, .Source = sSource, .isSingle = bSingle, .UseFolder = If(bSingle, bUseFolder, False), .Contents = lFi})
                            If bSingle AndAlso Not SkipStack Then Exit For
                        End If
                    Next
                End If

                If fList.Count = 1 Then tSingle = True

                If tSingle Then
                    fList(0).isSingle = True
                    fList(0).UseFolder = bUseFolder
                    Master.MediaList.Add(fList(0))
                Else
                    Master.MediaList.AddRange(fList)
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
    Public Shared Function isValidDir(ByVal sPath As String) As Boolean

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
    Public Shared Function SubDirsHaveMovies(ByVal MovieDir As DirectoryInfo) As Boolean
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
    Public Shared Function ScanSubs(ByVal inDir As DirectoryInfo) As Boolean
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
    Public Shared Function GetFolderContents(ByVal sPath As String, ByVal bSingle As Boolean) As String()

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
    ''' Get the full path to a trailer, if it exists.
    ''' </summary>
    ''' <param name="sPath">Full path to a movie file for which you are trying to find the accompanying trailer.</param>
    ''' <returns>Full path of trailer file.</returns>
    Public Shared Function GetTrailerPath(ByVal sPath As String) As String

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
    Public Shared Sub ScanTVSourceDir(ByVal sSource As String, ByVal sPath As String)

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
                        ScanForTVFiles(inDir.FullName, sSource)
                        If Regex.IsMatch(Directory.GetParent(inDir.FullName).Name, "(s(eason)?)?([\._ ])?([0-9]+)") Then ScanTVSourceDir(sSource, inDir.FullName)
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
    Public Shared Sub ScanForTVFiles(ByVal sPath As String, ByVal sSource As String)

        Try

            Dim di As DirectoryInfo
            Dim lFi As New List(Of FileInfo)
            Dim fList As New List(Of Master.FileAndSource)
            Dim tFile As String = String.Empty

            di = New DirectoryInfo(sPath)

            Try
                lFi.AddRange(di.GetFiles())
            Catch
            End Try

            If lFi.Count > 0 Then

                lFi.Sort(AddressOf FileManip.Common.SortFileNames)

                For Each lFile As FileInfo In lFi
                    If Not Master.MoviePaths.Contains(lFile.FullName.ToLower) AndAlso Master.eSettings.ValidExts.Contains(lFile.Extension.ToLower) AndAlso _
                        Not lFile.Name.ToLower.Contains("-trailer") AndAlso Not lFile.Name.ToLower.Contains("[trailer") AndAlso Not lFile.Name.ToLower.Contains("sample") AndAlso _
                        lFile.Length >= Master.eSettings.SkipLessThan * 1048576 Then
                        fList.Add(New Master.FileAndSource With {.Filename = lFile.FullName, .Source = sSource, .Contents = lFi})
                    End If
                Next

                Master.MediaList.AddRange(fList)

                fList = Nothing
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
End Class