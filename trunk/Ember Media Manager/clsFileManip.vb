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

Namespace FileManip
    Public Class Delete

        ''' <summary>
        ''' Delete files as selected by cleaner settings or all files related to a movie
        ''' </summary>
        ''' <param name="isCleaner">Called from cleaner?</param>
        ''' <param name="mMovie">DBMovie object to get paths from.</param>
        ''' <param name="SourcesList">List(of String) of paths for each source.</param>
        ''' <returns>True if files were deleted, false if not.</returns>
        ''' <remarks>Deprecated for all but cleaner... needs cleanup to reflect</remarks>
        Public Function DeleteFiles(ByVal isCleaner As Boolean, ByVal mMovie As Master.DBMovie) As Boolean
            Dim dPath As String = String.Empty
            Dim bReturn As Boolean = False
            Try
                If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(mMovie.Filename).Name.ToLower = "video_ts" Then
                    dPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".ext")
                Else
                    dPath = mMovie.Filename
                End If

                Dim sOrName As String = StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(dPath))
                Dim sPathShort As String = Directory.GetParent(dPath).FullName
                Dim sPathNoExt As String = Common.RemoveExtFromPath(dPath)

                Dim dirInfo As New DirectoryInfo(sPathShort)
                Dim ioFi As New List(Of FileInfo)

                Try
                    ioFi.AddRange(dirInfo.GetFiles())
                Catch
                End Try

                If isCleaner And Master.eSettings.ExpertCleaner Then

                    For Each sFile As FileInfo In ioFi
                        If Not Master.eSettings.CleanWhitelistExts.Contains(sFile.Extension.ToLower) AndAlso ((Master.eSettings.CleanWhitelistVideo AndAlso Not Master.eSettings.ValidExts.Contains(sFile.Extension.ToLower)) OrElse Not Master.eSettings.CleanWhitelistVideo) Then
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
                                    tPath = Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(fPath).FullName).Name, "-fanart.jpg"))
                                Else
                                    tPath = Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath))
                                End If
                            Else
                                If Path.GetFileName(fPath).ToLower = "fanart.jpg" Then
                                    tPath = Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(mMovie.Filename), "-fanart.jpg"))
                                Else
                                    tPath = Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath))
                                End If
                            End If
                        End If
                        If Not String.IsNullOrEmpty(tPath) Then
                            File.Delete(tPath)
                        End If
                    End If

                    If Not isCleaner AndAlso mMovie.isSingle AndAlso Not Master.SourcesList.Contains(Directory.GetParent(mMovie.Filename).FullName) Then
                        If Directory.GetParent(mMovie.Filename).Name.ToLower = "video_ts" Then
                            DeleteDirectory(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName)
                        Else
                            'check if there are other folders with movies in them
                            If Not Scanner.SubDirsHaveMovies(New DirectoryInfo(Directory.GetParent(mMovie.Filename).FullName)) Then
                                'no movies in sub dirs... delete the whole thing
                                DeleteDirectory(Directory.GetParent(mMovie.Filename).FullName)
                            Else
                                'just delete the movie file itself
                                File.Delete(mMovie.Filename)
                            End If

                        End If
                    Else
                        For Each lFI As FileInfo In ioFi
                            If isCleaner Then
                                If (Master.eSettings.CleanFolderJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "folder.jpg")) _
                                    OrElse (Master.eSettings.CleanFanartJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "fanart.jpg")) _
                                    OrElse (Master.eSettings.CleanMovieTBN AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.tbn")) _
                                    OrElse (Master.eSettings.CleanMovieNFO AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.nfo")) _
                                    OrElse (Master.eSettings.CleanPosterTBN AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "poster.tbn")) _
                                    OrElse (Master.eSettings.CleanPosterJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "poster.jpg")) _
                                    OrElse (Master.eSettings.CleanMovieJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.jpg")) Then
                                    File.Delete(lFI.FullName)
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieTBNB AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".tbn") _
                                OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.tbn") _
                                OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".tbn") Then
                                    File.Delete(lFI.FullName)
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, "-fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts-fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), "-fanart.jpg") Then
                                    File.Delete(lFI.FullName)
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieNFOB AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".nfo") _
                                    OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.nfo") _
                                    OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".nfo") Then
                                    File.Delete(lFI.FullName)
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanDotFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".fanart.jpg") Then
                                    File.Delete(lFI.FullName)
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieNameJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
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
                                If mMovie.isSingle Then ioFi.AddRange(dirInfo.GetFiles(String.Concat(sOrName, "*.*")))
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

                        If Master.eSettings.CleanExtraThumbs Then
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
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
            Return bReturn
        End Function

        ''' <summary>
        ''' Gather a list of all files to be deleted for display in a confirmation dialog.
        ''' </summary>
        ''' <param name="isCleaner">Is the function being called from the cleaner?</param>
        ''' <param name="mMovie">DBMovie object to get paths from</param>
        ''' <param name="SourcesList">List(of String) of paths for each source.</param>
        ''' <returns>True if files were found that are to be deleted, false if not.</returns>
        ''' <remarks>Not used for cleaner, needs to be modified to reflect.</remarks>
        Public Function GetItemsToDelete(ByVal isCleaner As Boolean, ByVal mMovie As Master.DBMovie) As List(Of IO.FileSystemInfo)
            Dim dPath As String = String.Empty
            Dim bReturn As Boolean = False
            Dim ItemsToDelete As New List(Of FileSystemInfo)
            Try
                Dim MovieFile As New FileInfo(mMovie.Filename)
                Dim MovieDir As DirectoryInfo = MovieFile.Directory

                If Master.eSettings.VideoTSParent AndAlso MovieDir.Name.ToLower = "video_ts" Then
                    dPath = String.Concat(Path.Combine(MovieDir.Parent.FullName, MovieDir.Parent.Name), ".ext")
                Else
                    dPath = mMovie.Filename
                End If

                Dim sOrName As String = StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(dPath))
                Dim sPathShort As String = Directory.GetParent(dPath).FullName
                Dim sPathNoExt As String = Common.RemoveExtFromPath(dPath)

                Dim dirInfo As New DirectoryInfo(sPathShort)
                Dim ioFi As New List(Of FileSystemInfo)

                Try
                    ioFi.AddRange(dirInfo.GetFiles())
                Catch
                End Try

                If isCleaner And Master.eSettings.ExpertCleaner Then

                    For Each sFile As FileInfo In ioFi
                        If Not Master.eSettings.CleanWhitelistExts.Contains(sFile.Extension.ToLower) AndAlso ((Master.eSettings.CleanWhitelistVideo AndAlso Not Master.eSettings.ValidExts.Contains(sFile.Extension.ToLower)) OrElse Not Master.eSettings.CleanWhitelistVideo) Then
                            sFile.Delete()
                            bReturn = True
                        End If
                    Next

                Else

                    If Not isCleaner Then
                        Dim fPath As String = mMovie.FanartPath
                        Dim tPath As String = String.Empty
                        If Not String.IsNullOrEmpty(fPath) AndAlso File.Exists(fPath) Then
                            If Directory.GetParent(fPath).Name.ToLower = "video_ts" Then
                                If Path.GetFileName(fPath).ToLower = "fanart.jpg" Then
                                    tPath = Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(fPath).FullName).Name, "-fanart.jpg"))
                                Else
                                    tPath = Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath))
                                End If
                            Else
                                If Path.GetFileName(fPath).ToLower = "fanart.jpg" Then
                                    tPath = Path.Combine(Master.eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(mMovie.Filename), "-fanart.jpg"))
                                Else
                                    tPath = Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath))
                                End If
                            End If
                        End If
                        If Not String.IsNullOrEmpty(tPath) Then
                            If IO.File.Exists(tPath) Then
                                ItemsToDelete.Add(New IO.FileInfo(tPath))
                            End If
                        End If
                    End If

                    If Not isCleaner AndAlso mMovie.isSingle AndAlso Not Master.SourcesList.Contains(MovieDir.Parent.ToString) Then
                        If MovieDir.Name.ToLower = "video_ts" Then
                            ItemsToDelete.Add(MovieDir.Parent)
                        Else
                            'check if there are other folders with movies in them
                            If Not Scanner.SubDirsHaveMovies(MovieDir) Then
                                'no movies in sub dirs... delete the whole thing
                                ItemsToDelete.Add(MovieDir)
                            Else
                                'just delete the movie file itself
                                ItemsToDelete.Add(New IO.FileInfo(mMovie.Filename))
                            End If
                        End If
                    Else
                        For Each lFI As FileInfo In ioFi
                            If isCleaner Then
                                If (Master.eSettings.CleanFolderJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "folder.jpg")) _
                                    OrElse (Master.eSettings.CleanFanartJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "fanart.jpg")) _
                                    OrElse (Master.eSettings.CleanMovieTBN AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.tbn")) _
                                    OrElse (Master.eSettings.CleanMovieNFO AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.nfo")) _
                                    OrElse (Master.eSettings.CleanPosterTBN AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "poster.tbn")) _
                                    OrElse (Master.eSettings.CleanPosterJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "poster.jpg")) _
                                    OrElse (Master.eSettings.CleanMovieJPG AndAlso lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "movie.jpg")) Then
                                    File.Delete(lFI.FullName)
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieTBNB AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".tbn") _
                                OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.tbn") _
                                OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".tbn") Then
                                    If isCleaner Then
                                        File.Delete(lFI.FullName)
                                    Else
                                        ItemsToDelete.Add(lFI)
                                    End If
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, "-fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts-fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), "-fanart.jpg") Then
                                    If isCleaner Then
                                        File.Delete(lFI.FullName)
                                    Else
                                        ItemsToDelete.Add(lFI)
                                    End If
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieNFOB AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".nfo") _
                                    OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.nfo") _
                                    OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".nfo") Then
                                    If isCleaner Then
                                        File.Delete(lFI.FullName)
                                    Else
                                        ItemsToDelete.Add(lFI)
                                    End If
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanDotFanartJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.fanart.jpg") _
                                    OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".fanart.jpg") Then
                                    If isCleaner Then
                                        File.Delete(lFI.FullName)
                                    Else
                                        ItemsToDelete.Add(lFI)
                                    End If
                                    bReturn = True
                                    Continue For
                                End If
                            End If

                            If (Master.eSettings.CleanMovieNameJPG AndAlso isCleaner) OrElse (Not isCleaner) Then
                                If lFI.FullName.ToLower = String.Concat(sPathNoExt.ToLower, ".jpg") _
                                    OrElse lFI.FullName.ToLower = Path.Combine(sPathShort.ToLower, "video_ts.jpg") _
                                    OrElse lFI.FullName.ToLower = String.Concat(Path.Combine(sPathShort.ToLower, sOrName.ToLower), ".jpg") Then
                                    If isCleaner Then
                                        File.Delete(lFI.FullName)
                                    Else
                                        ItemsToDelete.Add(lFI)
                                    End If
                                    bReturn = True
                                    Continue For
                                End If
                            End If
                        Next

                        If Not isCleaner Then

                            ioFi.Clear()
                            Try
                                If mMovie.isSingle Then ioFi.AddRange(dirInfo.GetFiles(String.Concat(sOrName, "*.*")))
                            Catch
                            End Try

                            Try
                                ioFi.AddRange(dirInfo.GetFiles(String.Concat(Path.GetFileNameWithoutExtension(mMovie.Filename), ".*")))
                            Catch
                            End Try

                            ItemsToDelete.AddRange(ioFi)

                        End If

                        If Master.eSettings.CleanExtraThumbs Then
                            If Directory.Exists(Path.Combine(sPathShort, "extrathumbs")) Then
                                If isCleaner Then
                                    DeleteDirectory(Path.Combine(sPathShort, "extrathumbs"))
                                Else
                                    Dim dir As New DirectoryInfo(Path.Combine(sPathShort, "extrathumbs"))
                                    If dir.Exists Then
                                        ItemsToDelete.Add(dir)
                                    End If
                                End If
                                bReturn = True
                            End If
                        End If

                    End If
                End If

                ioFi = Nothing
                dirInfo = Nothing
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
            Return ItemsToDelete
        End Function

        ''' <summary>
        ''' Safer method of deleting a diretory and all it's contents
        ''' </summary>
        ''' <param name="sPath">Full path of directory to delete</param>
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
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub
    End Class

    Public Class Common
        ''' <summary>
        ''' Get the entire path and filename of a file, but without the extension
        ''' </summary>
        ''' <param name="sPath">Full path to file.</param>
        ''' <returns>Path and filename of a file, without the extension</returns>
        Public Shared Function RemoveExtFromPath(ByVal sPath As String) As String

            Try
                Return Path.Combine(Directory.GetParent(sPath).FullName, Path.GetFileNameWithoutExtension(sPath))
            Catch
                Return String.Empty
            End Try

        End Function

        ''' <summary>
        ''' Copy a file from one location to another using a stream/buffer
        ''' </summary>
        ''' <param name="sPathFrom">Old path of file to move.</param>
        ''' <param name="sPathTo">New path of file to move.</param>
        Public Shared Sub MoveFileWithStream(ByVal sPathFrom As String, ByVal sPathTo As String)

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
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        ''' <summary>
        ''' Function to compare filenames for programmatic sorting (utilizes CompareTo)
        ''' </summary>
        ''' <param name="x">Fileinfo object</param>
        ''' <param name="y">Fileinfo object</param>
        ''' <returns>Integer (-1 = x first, 0 = same, 1 = y first)</returns>
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
    End Class
End Namespace