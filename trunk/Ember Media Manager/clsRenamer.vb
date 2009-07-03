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
Imports System.Text.RegularExpressions


Public Class FileFolderRenamer
    Class FileRename
        Public ID As Integer = -1 ' support for bulkRenamer 
        Private _title As String
        Public Year As String
        Public BasePath As String
        Private _path As String
        Private _fileName As String
        Private _newPath As String
        Private _newFileName As String
        Private _islocked As Boolean ' support for bulkRenamer 
        Private _dirExist As Boolean ' support for bulkRenamer 
        Private _fileExist As Boolean ' support for bulkRenamer 
        Public fType As Integer
        Public Resolution As String
        Public Audio As String
        Public Source As String = String.Empty


        Public Property Title() As String
            Get
                Return Me._title
            End Get
            Set(ByVal value As String)
                Me._title = value
            End Set
        End Property

        Public Property Path() As String
            Get
                Return Me._path
            End Get
            Set(ByVal value As String)
                Me._path = value
            End Set
        End Property
        Public Property FileName() As String
            Get
                Return Me._fileName
            End Get
            Set(ByVal value As String)
                Me._fileName = value
            End Set
        End Property
        Public Property NewPath() As String
            Get
                Return Me._newPath
            End Get
            Set(ByVal value As String)
                Me._newPath = value
            End Set
        End Property
        Public Property NewFileName() As String
            Get
                Return Me._newFileName
            End Get
            Set(ByVal value As String)
                Me._newFileName = value
            End Set
        End Property
        Public Property IsLocked() As Boolean
            Get
                Return Me._islocked
            End Get
            Set(ByVal value As Boolean)
                Me._islocked = value
            End Set
        End Property
        Public Property DirExist() As Boolean
            Get
                Return Me._dirExist
            End Get
            Set(ByVal value As Boolean)
                Me._dirExist = value
            End Set
        End Property
        Public Property FileExist() As Boolean
            Get
                Return Me._fileExist
            End Get
            Set(ByVal value As Boolean)
                Me._fileExist = value
            End Set
        End Property
    End Class
    Private _movies As New List(Of FileRename)
    Public MovieFolders As New ArrayList

    Public Sub New()
        Dim mePath As String = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Images", Path.DirectorySeparatorChar, "Flags")

        _movies.Clear()
        Using SQLNewcommand As SQLite.SQLiteCommand = Master.DB.CreateCommand
            SQLNewcommand.CommandText = String.Concat("SELECT Path FROM Sources;")
            Using SQLReader As SQLite.SQLiteDataReader = SQLNewcommand.ExecuteReader()
                While SQLReader.Read
                    MovieFolders.Add(SQLReader("Path"))
                End While
            End Using
        End Using
    End Sub

    Public Sub AddMovie(ByVal _movie As FileRename)
        _movies.Add(_movie)
    End Sub

    Public Function GetMovies() As List(Of FileRename)
        Return _movies
    End Function

    Public Function GetMoviesCount() As Integer
        Return _movies.Count
    End Function

    Public Sub ProccessFiles(ByVal folderPattern As String, ByVal filePattern As String)
        Try
            For Each f As FileRename In _movies
                f.NewFileName = ProccessPattern(f, filePattern)
                f.NewPath = ProccessPattern(f, folderPattern)
                f.FileExist = File.Exists(Path.Combine(f.Source, f.NewFileName)) AndAlso Not (f.FileExist = f.NewFileName)
                f.DirExist = File.Exists(Path.Combine(f.Source, f.NewPath)) AndAlso Not (f.Path = f.NewPath)

            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Function ProccessPattern(ByVal f As FileRename, ByVal opattern As String) As String
        Try
            Dim pattern As String = opattern
            Dim strSource As String = String.Empty
            Try
                Dim xVSourceFlag = From xVSource In Master.FlagsXML...<vsource>...<name> Where Regex.IsMatch(Path.Combine(f.Path.ToLower, f.FileName.ToLower), xVSource.@searchstring) Select Regex.Match(Path.Combine(f.Path.ToLower, f.FileName.ToLower), xVSource.@searchstring)
                'Dim xVSourceFlag = From xVSource In xmlFlags...<vsource>...<name> Select xVSource.@searchstring
                If xVSourceFlag.Count > 0 Then
                    strSource = xVSourceFlag(0).ToString
                End If
            Catch ex As Exception
            End Try
            'pattern = "$T{($S.$S)}"
            Dim nextC = pattern.IndexOf("$")
            Dim nextIB = pattern.IndexOf("{")
            Dim nextEB = pattern.IndexOf("}")
            Dim strCond As String
            Dim strBase As String
            While Not nextC = -1
                If nextC > nextIB AndAlso nextC < nextEB AndAlso Not nextC = -1 AndAlso Not nextIB = -1 AndAlso Not nextEB = -1 Then
                    strCond = pattern.Substring(nextIB, nextEB - nextIB + 1)
                    strBase = strCond
                    strCond = ApplyPattern(strCond, "D", f.Path)
                    strCond = ApplyPattern(strCond, "F", f.FileName)
                    strCond = ApplyPattern(strCond, "T", f.Title)
                    strCond = ApplyPattern(strCond, "Y", f.Year)
                    strCond = ApplyPattern(strCond, "R", f.Resolution)
                    strCond = ApplyPattern(strCond, "A", f.Audio)
                    strCond = ApplyPattern(strCond, "S", strSource)

                    Dim strNoFlags As String = Regex.Replace(pattern, "\$((?:[DFTYRAS]))", "") '"(?i)\$([DFTYRAS])"  "\$((?i:[DFTYRAS]))"
                    If strCond.Trim = strNoFlags.Trim Then
                        strCond = String.Empty
                    Else
                        strCond = strCond.Substring(1, strCond.Length - 2)
                    End If
                    pattern = pattern.Replace(strBase, strCond)
                    nextC = pattern.IndexOf("$")
                Else
                    nextC = pattern.IndexOf("$", nextC + 1)
                End If
                nextIB = pattern.IndexOf("{")
                nextEB = pattern.IndexOf("}")
            End While
            pattern = ApplyPattern(pattern, "D", f.Path)
            pattern = ApplyPattern(pattern, "F", f.FileName)
            pattern = ApplyPattern(pattern, "T", f.Title)
            pattern = ApplyPattern(pattern, "Y", f.Year)
            pattern = ApplyPattern(pattern, "R", f.Resolution)
            pattern = ApplyPattern(pattern, "A", f.Audio)
            pattern = ApplyPattern(pattern, "S", strSource)
            nextC = pattern.IndexOf("$X")
            If Not nextC = -1 AndAlso pattern.Length > nextC + 2 Then
                strCond = pattern.Substring(nextC + 2, 1)
                pattern = pattern.Replace(String.Concat("$X", strCond), "")
                pattern = pattern.Replace(" ", strCond)
            End If
            'Remove/Replace some Invalid chars from path/filename
            pattern = pattern.Replace(":", " -")
            pattern = pattern.Replace("?", String.Empty)
            pattern = pattern.Replace("*", String.Empty)
            pattern = pattern.Replace("  ", " ")
            For Each Invalid As Char In Path.GetInvalidPathChars
                pattern = pattern.Replace(Invalid, String.Empty)
            Next
            Return pattern
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return vbNullString
        End Try
    End Function
    Private Function ApplyPattern(ByVal pattern As String, ByVal flag As String, ByVal v As String) As String

        pattern = pattern.Replace(String.Concat("$", flag), v)
        If Not v = String.Empty Then
            pattern = pattern.Replace(String.Concat("$-", flag), v)
            pattern = pattern.Replace(String.Concat("$+", flag), v)
        Else
            Dim pos = -1
            Dim size = 3
            Dim nextC = pattern.IndexOf(String.Concat("$-", flag))
            If nextC >= 0 Then
                If nextC + 3 < pattern.Length Then size += 1
                pos = nextC
            End If
            Dim prevC = pattern.IndexOf(String.Concat("$+", flag))
            If prevC >= 0 Then
                If prevC + 3 < pattern.Length Then size += 1
                If prevC > 0 Then
                    size += 1
                    prevC -= 1
                End If
                pos = prevC
            End If

            If Not pos = -1 Then pattern = pattern.Remove(pos, size)
        End If
        Return pattern
    End Function

    Private Function UpdateFaSPaths(ByVal _fas As Master.FileAndSource, ByVal oldPath As String, ByVal newPath As String) As Master.FileAndSource
        If Not String.IsNullOrEmpty(_fas.Fanart) Then _fas.Fanart = Path.Combine(Path.GetDirectoryName(_fas.Fanart).Replace(oldPath, newPath), Path.GetFileName(_fas.Fanart))
        If Not String.IsNullOrEmpty(_fas.Extra) Then _fas.Extra = Path.Combine(Path.GetDirectoryName(_fas.Extra).Replace(oldPath, newPath), Path.GetFileName(_fas.Extra))
        If Not String.IsNullOrEmpty(_fas.Filename) Then _fas.Filename = Path.Combine(Path.GetDirectoryName(_fas.Filename).Replace(oldPath, newPath), Path.GetFileName(_fas.Filename))
        If Not String.IsNullOrEmpty(_fas.Nfo) Then _fas.Nfo = Path.Combine(Path.GetDirectoryName(_fas.Nfo).Replace(oldPath, newPath), Path.GetFileName(_fas.Nfo))
        If Not String.IsNullOrEmpty(_fas.Poster) Then _fas.Poster = Path.Combine(Path.GetDirectoryName(_fas.Poster).Replace(oldPath, newPath), Path.GetFileName(_fas.Poster))
        If Not String.IsNullOrEmpty(_fas.Subs) Then _fas.Subs = Path.Combine(Path.GetDirectoryName(_fas.Subs).Replace(oldPath, newPath), Path.GetFileName(_fas.Subs))
        If Not String.IsNullOrEmpty(_fas.Trailer) Then _fas.Trailer = Path.Combine(Path.GetDirectoryName(_fas.Trailer).Replace(oldPath, newPath), Path.GetFileName(_fas.Trailer))
        Return _fas
    End Function
    Private Function UpdateFaSFiles(ByVal _fas As Master.FileAndSource, ByVal oldPath As String, ByVal newPath As String) As Master.FileAndSource
        If Not String.IsNullOrEmpty(_fas.Fanart) Then _fas.Fanart = Path.Combine(Path.GetDirectoryName(_fas.Fanart), Path.GetFileName(_fas.Fanart).Replace(oldPath, newPath))
        If Not String.IsNullOrEmpty(_fas.Extra) Then _fas.Extra = Path.Combine(Path.GetDirectoryName(_fas.Extra), Path.GetFileName(_fas.Extra).Replace(oldPath, newPath))
        If Not String.IsNullOrEmpty(_fas.Filename) Then _fas.Filename = Path.Combine(Path.GetDirectoryName(_fas.Filename), Path.GetFileName(_fas.Filename).Replace(oldPath, newPath))
        If Not String.IsNullOrEmpty(_fas.Nfo) Then _fas.Nfo = Path.Combine(Path.GetDirectoryName(_fas.Nfo), Path.GetFileName(_fas.Nfo).Replace(oldPath, newPath))
        If Not String.IsNullOrEmpty(_fas.Poster) Then _fas.Poster = Path.Combine(Path.GetDirectoryName(_fas.Poster), Path.GetFileName(_fas.Poster).Replace(oldPath, newPath))
        If Not String.IsNullOrEmpty(_fas.Subs) Then _fas.Subs = Path.Combine(Path.GetDirectoryName(_fas.Subs), Path.GetFileName(_fas.Subs).Replace(oldPath, newPath))
        If Not String.IsNullOrEmpty(_fas.Trailer) Then _fas.Trailer = Path.Combine(Path.GetDirectoryName(_fas.Trailer), Path.GetFileName(_fas.Trailer).Replace(oldPath, newPath))
        Return _fas
    End Function


    Public Delegate Function ShowProgress(ByVal movie As String, ByVal iProgress As Integer) As Boolean

    Public Sub DoRename(Optional ByVal sfunction As ShowProgress = Nothing)
        Dim DoDB As Boolean
        Dim DoUpdate As Boolean
        Dim _movieDB As Master.DBMovie = Nothing
        Dim iProg As Integer = 0
        Try
            For Each f As FileFolderRenamer.FileRename In _movies
                iProg += 1
                DoUpdate = False
                If Not f.IsLocked Then
                    If Not f.ID = -1 AndAlso ((Not f.NewPath.ToLower = f.Path.ToLower) OrElse (Not f.NewFileName.ToLower = f.FileName.ToLower)) Then
                        _movieDB = Master.DB.LoadMovieFromDB(f.ID)
                        DoDB = True
                    Else
                        _movieDB = Nothing
                        DoDB = False
                    End If
                    'Rename Directory
                    If Not f.NewPath.ToLower = f.Path.ToLower Then

                        Dim srcDir As String = Path.Combine(f.BasePath, f.Path)
                        Dim destDir As String = Path.Combine(f.BasePath, f.NewPath)
                        If Not sfunction Is Nothing Then
                            If Not sfunction(f.NewPath, iProg) Then Return
                        End If
                        Try
                            System.IO.Directory.Move(srcDir, destDir)
                            If DoDB = True Then
                                _movieDB.FaS = UpdateFaSPaths(_movieDB.FaS, f.Path, f.NewPath)
                            End If
                            DoUpdate = True
                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                            Continue For
                        End Try

                    End If
                    'Rename Files
                    If Not f.NewFileName.ToLower = f.FileName.ToLower Then
                        Dim tmpList As New ArrayList

                        Dim di As New DirectoryInfo(Path.Combine(f.BasePath, f.NewPath))
                        Dim lFi As New List(Of FileInfo)
                        Try
                            lFi.AddRange(di.GetFiles())
                        Catch
                        End Try
                        If lFi.Count > 0 Then
                            lFi.Sort(AddressOf Master.SortFileNames)
                            Dim srcFile As String
                            Dim dstFile As String
                            For Each lFile As FileInfo In lFi
                                srcFile = lFile.FullName
                                dstFile = Path.Combine(Path.GetDirectoryName(lFile.FullName), Path.GetFileName(lFile.FullName).Replace(f.FileName, f.NewFileName))
                                If Not srcFile = dstFile Then
                                    Try
                                        Dim fr = New System.IO.FileInfo(srcFile)
                                        fr.MoveTo(dstFile)
                                        DoUpdate = True
                                    Catch ex As Exception
                                        Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                                    End Try
                                End If
                            Next
                            If DoDB = True AndAlso DoUpdate Then
                                _movieDB.FaS = UpdateFaSFiles(_movieDB.FaS, f.FileName, f.NewFileName)
                            End If
                        End If
                    End If
                End If
                If DoDB AndAlso DoUpdate Then
                    Master.DB.SaveMovieToDB(_movieDB, False)
                End If
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub
    Public Shared Function RenameSingle(ByVal _tmpPath As String, ByVal _tmpMovie As Media.Movie, ByVal folderPattern As String, ByVal filePattern As String) As Boolean
        Dim bulkRename As New FileFolderRenamer
        Dim MovieFile As FileFolderRenamer.FileRename = New FileFolderRenamer.FileRename
        'bulkRename._movies.Clear()
        MovieFile.Title = _tmpMovie.Title
        MovieFile.Year = _tmpMovie.Year
        If Not IsNothing(_tmpMovie.FileInfo) Then
            If _tmpMovie.FileInfo.StreamDetails.Video.Count > 0 Then MovieFile.Resolution = Master.GetResFromDimensions(Master.GetBestVideo(_tmpMovie.FileInfo))
            If _tmpMovie.FileInfo.StreamDetails.Audio.Count > 0 Then MovieFile.Audio = Master.GetBestAudio(_tmpMovie.FileInfo).Codec
        End If

        MovieFile.BasePath = Path.GetDirectoryName(_tmpPath)
        MovieFile.Path = Path.GetDirectoryName(_tmpPath)
        'MovieFile.NewFileName = bulkRename.ProccessPattern(MovieFile, folderPattern)
        'MovieFile.NewPath = bulkRename.ProccessPattern(MovieFile, filePattern)
        For Each i As String In bulkRename.MovieFolders
            If i = MovieFile.Path.Substring(0, i.Length) Then
                MovieFile.Path = MovieFile.Path.Substring(i.Length)
                If MovieFile.Path.Substring(0, 1) = Path.DirectorySeparatorChar Then
                    MovieFile.Path = MovieFile.Path.Substring(1)
                End If
                MovieFile.BasePath = i
                Exit For
            End If
        Next
        MovieFile.FileName = Path.GetFileNameWithoutExtension(Master.CleanStackingMarkers(_tmpPath))

        bulkRename.AddMovie(MovieFile)
        bulkRename.ProccessFiles(folderPattern, filePattern)
        bulkRename.DoRename()
        Return True
    End Function
End Class
