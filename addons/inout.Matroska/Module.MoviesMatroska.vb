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
Imports System.Text.RegularExpressions
Imports System.Xml.Serialization

Public Class InputMatroska_Module
    Implements Interfaces.EmberMovieInputModule

    Public Shared _AssemblyName As String
    Private fInputSettings As frmMovieInputSettings
    Public Shared eSettings As New MySettings


    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.Enabled
        Get
            Return False
        End Get
        Set(ByVal value As Boolean)
        End Set
    End Property

    Public Function GetFilesFolderContents(ByRef Movie As Scanner.MovieContainer) As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.GetFilesFolderContents

        Dim tmpName As String = String.Empty
        Dim tmpNameNoStack As String = String.Empty
        Dim currname As String = String.Empty
        Dim parPath As String = String.Empty
        Dim fList As New List(Of String)
        Try

            If Movie.isSingle Then
                fList.AddRange(Directory.GetFiles(Directory.GetParent(Movie.Filename).FullName))
            Else
                Try
                    Dim sName As String = StringUtils.CleanStackingMarkers(Path.GetFileNameWithoutExtension(Movie.Filename), True)
                    fList.AddRange(Directory.GetFiles(Directory.GetParent(Movie.Filename).FullName, If(sName.EndsWith("*"), sName, String.Concat(sName, "*"))))
                Catch
                End Try
            End If

            parPath = Directory.GetParent(Movie.Filename).FullName.ToLower
            tmpName = Path.Combine(parPath, StringUtils.CleanStackingMarkers(Path.GetFileNameWithoutExtension(Movie.Filename))).ToLower
            tmpNameNoStack = Path.Combine(parPath, Path.GetFileNameWithoutExtension(Movie.Filename)).ToLower

            If Movie.isSingle AndAlso File.Exists(String.Concat(Directory.GetParent(Movie.Filename).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                Movie.Extra = String.Concat(Directory.GetParent(Movie.Filename).FullName, Path.DirectorySeparatorChar, "extrathumbs", Path.DirectorySeparatorChar, "thumb1.jpg")
            End If

            For Each fFile As String In fList
                'fanart
                If String.IsNullOrEmpty(Movie.Fanart) Then
                    If (Movie.isSingle AndAlso eSettings.FanartJPG AndAlso fFile.ToLower = Path.Combine(parPath, "fanart.jpg")) _
                        OrElse ((Not Movie.isSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                                ((eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, "-fanart.jpg")) _
                                OrElse (eSettings.MovieNameFanartJPG AndAlso fFile.ToLower = String.Concat(tmpName, "-fanart.jpg")) _
                                OrElse (eSettings.MovieNameFanartJPG AndAlso (fFile.ToLower = Path.Combine(parPath, "video_ts-fanart.jpg") OrElse fFile.ToLower = Path.Combine(parPath, "index-fanart.jpg")))) _
                                OrElse (eSettings.MovieNameDotFanartJPG AndAlso (fFile.ToLower = Path.Combine(parPath, "video_ts.fanart.jpg") OrElse fFile.ToLower = Path.Combine(parPath, "index.fanart.jpg")))) _
                        OrElse ((Not Movie.isSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                                (((eSettings.MovieNameDotFanartJPG) AndAlso fFile.ToLower = String.Concat(tmpName, ".fanart.jpg")) _
                                OrElse ((eSettings.MovieNameDotFanartJPG) AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".fanart.jpg")))) Then

                        Movie.Fanart = fFile
                        Continue For
                    End If
                End If

                'poster
                If String.IsNullOrEmpty(Movie.Poster) Then
                    If (Movie.isSingle AndAlso (eSettings.MovieTBN AndAlso fFile.ToLower = Path.Combine(parPath, "movie.tbn")) _
                                OrElse (eSettings.PosterTBN AndAlso fFile.ToLower = Path.Combine(parPath, "poster.tbn")) _
                                OrElse (eSettings.MovieJPG AndAlso fFile.ToLower = Path.Combine(parPath, "movie.jpg")) _
                                OrElse (eSettings.PosterJPG AndAlso fFile.ToLower = Path.Combine(parPath, "poster.jpg")) _
                                OrElse (eSettings.FolderJPG AndAlso fFile.ToLower = Path.Combine(parPath, "folder.jpg"))) _
                        OrElse ((Not Movie.isSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                                ((eSettings.MovieNameTBN AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.tbn")) _
                                OrElse (eSettings.MovieNameJPG AndAlso fFile.ToLower = Path.Combine(parPath, "video_ts.jpg")))) _
                        OrElse ((Not Movie.isSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                                ((eSettings.MovieNameTBN AndAlso fFile.ToLower = Path.Combine(parPath, "index.tbn")) _
                                OrElse (eSettings.MovieNameJPG AndAlso fFile.ToLower = Path.Combine(parPath, "index.jpg")))) _
                        OrElse ((Not Movie.isSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                                (((eSettings.MovieNameTBN) AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".tbn")) _
                                OrElse ((eSettings.MovieNameTBN) AndAlso fFile.ToLower = String.Concat(tmpName, ".tbn")) _
                                OrElse ((eSettings.MovieNameJPG) AndAlso fFile.ToLower = String.Concat(tmpNameNoStack, ".jpg")) _
                                OrElse ((eSettings.MovieNameJPG) AndAlso fFile.ToLower = String.Concat(tmpName, ".jpg")))) Then
                        Movie.Poster = fFile
                        Continue For
                    End If
                End If

                'nfo
                If String.IsNullOrEmpty(Movie.Nfo) Then
                    If (Movie.isSingle AndAlso eSettings.MovieNFO AndAlso fFile.ToLower = Path.Combine(parPath, "movie.nfo")) _
                    OrElse ((Not Movie.isSingle OrElse Not eSettings.MovieNameMultiOnly) AndAlso _
                    (((eSettings.MovieNameNFO) AndAlso (fFile.ToLower = String.Concat(tmpNameNoStack, ".nfo") OrElse _
                                                                             fFile.ToLower = String.Concat(tmpName, ".nfo") OrElse _
                                                                             fFile.ToLower = Path.Combine(parPath, "video_ts.nfo") OrElse _
                                                                             fFile.ToLower = Path.Combine(parPath, "index.nfo"))))) Then
                        Movie.Nfo = fFile
                        Continue For
                    End If
                End If

                If String.IsNullOrEmpty(Movie.Subs) Then
                    If Regex.IsMatch(fFile.ToLower, String.Concat("^", Regex.Escape(tmpNameNoStack), AdvancedSettings.GetSetting("SubtitleExtension", ".*\.(sst|srt|sub|ssa|aqt|smi|sami|jss|mpl|rt|idx|ass)$")), RegexOptions.IgnoreCase) OrElse _
                                Regex.IsMatch(fFile.ToLower, String.Concat("^", Regex.Escape(tmpName), AdvancedSettings.GetSetting("SubtitleExtension", ".*\.(sst|srt|sub|ssa|aqt|smi|sami|jss|mpl|rt|idx|ass)$")), RegexOptions.IgnoreCase) Then
                        Movie.Subs = fFile
                        Continue For
                    End If
                End If

                If String.IsNullOrEmpty(Movie.Trailer) Then
                    For Each t As String In Master.eSettings.ValidExts
                        Select Case True
                            Case fFile.ToLower = String.Concat(tmpNameNoStack, "-trailer", t.ToLower)
                                Movie.Trailer = fFile
                                Exit For
                            Case fFile.ToLower = String.Concat(tmpName, "-trailer", t.ToLower)
                                Movie.Trailer = fFile
                                Exit For
                            Case fFile.ToLower = String.Concat(tmpNameNoStack, "[trailer]", t.ToLower)
                                Movie.Trailer = fFile
                                Exit For
                            Case fFile.ToLower = String.Concat(tmpName, "[trailer]", t.ToLower)
                                Movie.Trailer = fFile
                                Exit For
                            Case Movie.isSingle AndAlso fFile.ToLower = Path.Combine(parPath, String.Concat("movie-trailer", t.ToLower))
                                Movie.Trailer = fFile
                                Exit For
                            Case Movie.isSingle AndAlso fFile.ToLower = Path.Combine(parPath, String.Concat("movie[trailer]", t.ToLower))
                                Movie.Trailer = fFile
                                Exit For
                        End Select
                    Next
                End If

                If Not String.IsNullOrEmpty(Movie.Poster) AndAlso Not String.IsNullOrEmpty(Movie.Fanart) _
                AndAlso Not String.IsNullOrEmpty(Movie.Nfo) AndAlso Not String.IsNullOrEmpty(Movie.Trailer) _
                AndAlso Not String.IsNullOrEmpty(Movie.Subs) AndAlso Not String.IsNullOrEmpty(Movie.Extra) Then
                    Exit For
                End If
            Next

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        fList = Nothing

        Return False
    End Function

    Public Function LoadMovieInfoSheet(ByVal sPath As String, ByVal isSingle As Boolean, ByRef mMovie As EmberAPI.Structures.DBMovie) As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.LoadMovieInfoSheet
        Return False
    End Function

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberMovieInputModule.ModuleName
        Get
            Return "Matroska Input Module"
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberMovieInputModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function InjectSetup() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberMovieInputModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me.fInputSettings = New frmMovieInputSettings
        Me.fInputSettings.chkEnabled.Checked = Me.Enabled
        SPanel.Name = Me.ModuleName
        SPanel.Text = Master.eLang.GetString(91, "Matroska Input Module")
        SPanel.Prefix = "MatroskaInputModule_"
        SPanel.Parent = "pnlMovieInput"
        SPanel.Type = Master.eLang.GetString(36, "Movies", True)
        SPanel.ImageIndex = If(Me.Enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me.fInputSettings.pnlSettings
        'AddHandler Me.fInputSettings.SettingsChanged, AddressOf Handle_ModuleSettingsChanged
        'AddHandler me.fInputSettings.EnabledChanged, AddressOf Handle_SetupChanged
        Return SPanel
        'Return Nothing
    End Function

    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberMovieInputModule.SaveSetup

    End Sub

    Public Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements EmberAPI.Interfaces.EmberMovieInputModule.SetupChanged
    Public Event ModuleSettingsChanged() Implements Interfaces.EmberMovieInputModule.ModuleSettingsChanged
    Public Sub SetupOrderChanged() Implements EmberAPI.Interfaces.EmberMovieInputModule.SetupOrderChanged

    End Sub

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberMovieInputModule.Init
        _AssemblyName = sAssemblyName
    End Sub
    Sub SaveMySettings()

    End Sub
    Sub LoadMySettings()

    End Sub

End Class

Public Class MySettings
    Private _videotsparent As Boolean
    Private _fanartjpg As Boolean
    Private _movienamemultionly As Boolean
    Private _moviejpg As Boolean
    Private _movienamedotfanartjpg As Boolean
    Private _movienamefanartjpg As Boolean
    Private _movienamejpg As Boolean
    Private _movietbn As Boolean
    Private _postertbn As Boolean
    Private _posterjpg As Boolean
    Private _folderjpg As Boolean
    Private _movienametbn As Boolean
    Private _movienamenfo As Boolean
    Private _movienfo As Boolean

    Sub New()

    End Sub
    Public Property VideoTSParent() As Boolean
        Get
            Return Me._videotsparent
        End Get
        Set(ByVal value As Boolean)
            Me._videotsparent = value
        End Set
    End Property
    Public Property FanartJPG() As Boolean
        Get
            Return Me._fanartjpg
        End Get
        Set(ByVal value As Boolean)
            Me._fanartjpg = value
        End Set
    End Property
    Public Property MovieNameMultiOnly() As Boolean
        Get
            Return Me._movienamemultionly
        End Get
        Set(ByVal value As Boolean)
            Me._movienamemultionly = value
        End Set
    End Property
    Public Property MovieJPG() As Boolean
        Get
            Return Me._moviejpg
        End Get
        Set(ByVal value As Boolean)
            Me._moviejpg = value
        End Set
    End Property

    Public Property MovieNameDotFanartJPG() As Boolean
        Get
            Return Me._movienamedotfanartjpg
        End Get
        Set(ByVal value As Boolean)
            Me._movienamedotfanartjpg = value
        End Set
    End Property

    Public Property MovieNameFanartJPG() As Boolean
        Get
            Return Me._movienamefanartjpg
        End Get
        Set(ByVal value As Boolean)
            Me._movienamefanartjpg = value
        End Set
    End Property

    Public Property MovieNameJPG() As Boolean
        Get
            Return Me._movienamejpg
        End Get
        Set(ByVal value As Boolean)
            Me._movienamejpg = value
        End Set
    End Property
    Public Property MovieTBN() As Boolean
        Get
            Return Me._movietbn
        End Get
        Set(ByVal value As Boolean)
            Me._movietbn = value
        End Set
    End Property
    Public Property PosterTBN() As Boolean
        Get
            Return Me._postertbn
        End Get
        Set(ByVal value As Boolean)
            Me._postertbn = value
        End Set
    End Property
    Public Property PosterJPG() As Boolean
        Get
            Return Me._posterjpg
        End Get
        Set(ByVal value As Boolean)
            Me._posterjpg = value
        End Set
    End Property
    Public Property FolderJPG() As Boolean
        Get
            Return Me._folderjpg
        End Get
        Set(ByVal value As Boolean)
            Me._folderjpg = value
        End Set
    End Property
    Public Property MovieNameTBN() As Boolean
        Get
            Return Me._movienametbn
        End Get
        Set(ByVal value As Boolean)
            Me._movienametbn = value
        End Set
    End Property
    Public Property MovieNameNFO() As Boolean
        Get
            Return Me._movienamenfo
        End Get
        Set(ByVal value As Boolean)
            Me._movienamenfo = value
        End Set
    End Property
    Public Property MovieNFO() As Boolean
        Get
            Return Me._movienfo
        End Get
        Set(ByVal value As Boolean)
            Me._movienfo = value
        End Set
    End Property
End Class