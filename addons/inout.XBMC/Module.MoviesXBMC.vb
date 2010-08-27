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

Public Class InputXBMC_Module
    Implements Interfaces.EmberMovieInputModule

    Public Shared _AssemblyName As String
    Private fInputSettings As frmMovieInputSettings
    Private _Enabled As Boolean
    Public Shared eSettings As New MySettings
    Public Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements EmberAPI.Interfaces.EmberMovieInputModule.SetupChanged
    Public Event ModuleSettingsChanged() Implements Interfaces.EmberMovieInputModule.ModuleSettingsChanged

    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.Enabled
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
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

            If Movie.isSingle AndAlso File.Exists(String.Concat(Directory.GetParent(Movie.Filename).FullName, Path.DirectorySeparatorChar, eSettings.ETPath, Path.DirectorySeparatorChar, "thumb1.jpg")) Then
                Movie.Extra = String.Concat(Directory.GetParent(Movie.Filename).FullName, Path.DirectorySeparatorChar, eSettings.ETPath, Path.DirectorySeparatorChar, "thumb1.jpg")
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

    Public Function LoadMovieInfoSheet(ByVal sPath As String, ByVal isSingle As Boolean, ByRef mMovie As MediaContainers.Movie) As Boolean Implements EmberAPI.Interfaces.EmberMovieInputModule.LoadMovieInfoSheet
        mMovie = NFO.LoadMovieFromNFO(sPath, isSingle)
        Return If(String.IsNullOrEmpty(mMovie.Title), False, True)
    End Function

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberMovieInputModule.ModuleName
        Get
            Return "XBMC Input Module"
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
        RetrieveMySettings()
        Me.fInputSettings.chkEnabled.Checked = Me.Enabled
        SPanel.Name = Me.ModuleName
        SPanel.Text = Master.eLang.GetString(91, "XBMC")
        SPanel.Prefix = "XBMCInputModule_"
        SPanel.Parent = "pnlMovieInput"
        SPanel.Type = Master.eLang.GetString(36, "Movies", True)
        SPanel.ImageIndex = If(Me.Enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me.fInputSettings.pnlSettings
        AddHandler Me.fInputSettings.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        AddHandler Me.fInputSettings.ModuleEnabledChanged, AddressOf Handle_SetupChanged
        fInputSettings.orderChanged()
        Return SPanel
        'Return Nothing
    End Function
    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub
    Private Sub Handle_SetupChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent SetupChanged(Me.ModuleName, state, difforder)
    End Sub
    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberMovieInputModule.SaveSetup
        StoreMySettings()
    End Sub

    Public Sub SetupOrderChanged() Implements EmberAPI.Interfaces.EmberMovieInputModule.SetupOrderChanged

    End Sub

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberMovieInputModule.Init
        _AssemblyName = sAssemblyName
        eSettings.Load("Input.")
    End Sub
    Sub StoreMySettings()
        Enabled = fInputSettings.chkEnabled.Checked
        eSettings.Enabled = fInputSettings.chkEnabled.Checked
        eSettings.FanartJPG = fInputSettings.chkFanartJPG.Checked
        eSettings.FolderJPG = fInputSettings.chkFolderJPG.Checked
        eSettings.MovieJPG = fInputSettings.chkMovieJPG.Checked
        eSettings.MovieNameDotFanartJPG = fInputSettings.chkMovieNameDotFanartJPG.Checked
        eSettings.MovieNameFanartJPG = fInputSettings.chkMovieNameFanartJPG.Checked
        eSettings.MovieNameJPG = fInputSettings.chkMovieNameJPG.Checked
        eSettings.MovieNameMultiOnly = fInputSettings.chkMovieNameMultiOnly.Checked
        eSettings.MovieNameNFO = fInputSettings.chkMovieNameNFO.Checked
        eSettings.MovieNameTBN = fInputSettings.chkMovieNameTBN.Checked
        eSettings.MovieNFO = fInputSettings.chkMovieNFO.Checked
        eSettings.MovieTBN = fInputSettings.chkMovieTBN.Checked
        eSettings.PosterJPG = fInputSettings.chkPosterJPG.Checked
        eSettings.PosterTBN = fInputSettings.chkPosterTBN.Checked
        eSettings.Save("Input.")
    End Sub
    Sub RetrieveMySettings()
        eSettings.Load("Input.")
        fInputSettings.chkEnabled.Checked = eSettings.Enabled
        fInputSettings.chkFanartJPG.Checked = eSettings.FanartJPG
        fInputSettings.chkFolderJPG.Checked = eSettings.FolderJPG
        fInputSettings.chkMovieJPG.Checked = eSettings.MovieJPG
        fInputSettings.chkMovieNameDotFanartJPG.Checked = eSettings.MovieNameDotFanartJPG
        fInputSettings.chkMovieNameFanartJPG.Checked = eSettings.MovieNameFanartJPG
        fInputSettings.chkMovieNameJPG.Checked = eSettings.MovieNameJPG
        fInputSettings.chkMovieNameMultiOnly.Checked = eSettings.MovieNameMultiOnly
        fInputSettings.chkMovieNameNFO.Checked = eSettings.MovieNameNFO
        fInputSettings.chkMovieNameTBN.Checked = eSettings.MovieNameTBN
        fInputSettings.chkMovieNFO.Checked = eSettings.MovieNFO
        fInputSettings.chkMovieTBN.Checked = eSettings.MovieTBN
        fInputSettings.chkPosterJPG.Checked = eSettings.PosterJPG
        fInputSettings.chkPosterTBN.Checked = eSettings.PosterTBN
    End Sub

End Class


Public Class OutputXBMC_Module
    Implements Interfaces.EmberMovieOutputModule
    Private _Enabled As Boolean
    Public Shared _AssemblyName As String
    Private fOutputSettings As frmMovieOutputSettings
    Public Shared eSettings As New MySettings
    Public Event ModuleSettingsChanged() Implements EmberAPI.Interfaces.EmberMovieOutputModule.ModuleSettingsChanged
    Public Event SetupChanged(ByVal name As String, ByVal State As Boolean, ByVal difforder As Integer) Implements EmberAPI.Interfaces.EmberMovieOutputModule.SetupChanged

    Public Property Enabled() As Boolean Implements EmberAPI.Interfaces.EmberMovieOutputModule.Enabled
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
        End Set
    End Property

    Public ReadOnly Property ModuleName() As String Implements EmberAPI.Interfaces.EmberMovieOutputModule.ModuleName
        Get
            Return "XBMC Output Module"
        End Get
    End Property

    Public ReadOnly Property ModuleVersion() As String Implements EmberAPI.Interfaces.EmberMovieOutputModule.ModuleVersion
        Get
            Return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly.Location).FilePrivatePart.ToString
        End Get
    End Property

    Public Function SaveImageAs(ByVal imageType As EmberAPI.Enums.ImageType, ByRef mMovie As EmberAPI.Structures.DBMovie) As List(Of String) Implements EmberAPI.Interfaces.EmberMovieOutputModule.SaveImageAs
        Dim imageList As New List(Of String)
        Dim fPath As String = String.Empty
        Dim tPath As String = String.Empty
        Dim pPath As String = String.Empty
        Select Case imageType
            Case Enums.ImageType.Posters
                Try
                    'YAMJ Options, Remove after cloning the module
                    If eSettings.VideoTSParent AndAlso FileUtils.Common.isVideoTS(mMovie.Filename) Then
                        If eSettings.MovieNameJPG OrElse eSettings.MovieJPG OrElse eSettings.FolderJPG OrElse eSettings.PosterJPG Then
                            pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".jpg")
                            If Not File.Exists(pPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(pPath)
                            End If
                        ElseIf eSettings.MovieNameTBN OrElse eSettings.MovieTBN OrElse eSettings.PosterTBN Then
                            pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".tbn")
                            If Not File.Exists(pPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(pPath)
                            End If
                        End If
                        'YAMJ Options, Remove after cloning the module
                    ElseIf eSettings.VideoTSParent AndAlso FileUtils.Common.isBDRip(mMovie.Filename) Then

                        If eSettings.MovieNameJPG OrElse eSettings.MovieJPG OrElse eSettings.FolderJPG OrElse eSettings.PosterJPG Then
                            pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).FullName, Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name), ".jpg")
                            If Not File.Exists(pPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(pPath)
                            End If
                        ElseIf eSettings.MovieNameTBN OrElse eSettings.MovieTBN OrElse eSettings.PosterTBN Then
                            pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).FullName, Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name), ".tbn")
                            If Not File.Exists(pPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(pPath)
                            End If
                        End If
                    Else
                        Dim tmpName As String = Path.GetFileNameWithoutExtension(mMovie.Filename)
                        pPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, tmpName)

                        If eSettings.FolderJPG AndAlso mMovie.isSingle Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "folder.jpg")
                            If Not File.Exists(tPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(tPath)
                            End If
                        End If

                        If eSettings.PosterJPG AndAlso mMovie.isSingle Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "poster.jpg")
                            If Not File.Exists(tPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(tPath)
                            End If
                        End If

                        If eSettings.PosterTBN AndAlso mMovie.isSingle Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "poster.tbn")
                            If Not File.Exists(tPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(tPath)
                            End If
                        End If

                        If eSettings.MovieNameJPG AndAlso (Not mMovie.isSingle OrElse Not eSettings.MovieNameMultiOnly) Then
                            If FileUtils.Common.isVideoTS(mMovie.Filename) Then
                                tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts.jpg")
                            ElseIf FileUtils.Common.isBDRip(mMovie.Filename) Then
                                tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index.jpg")
                            Else
                                tPath = String.Concat(pPath, ".jpg")
                            End If
                            If Not File.Exists(tPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(tPath)
                            End If
                        End If

                        If eSettings.MovieJPG AndAlso mMovie.isSingle Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "movie.jpg")
                            If Not File.Exists(tPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(tPath)
                            End If
                        End If

                        If eSettings.MovieNameTBN AndAlso (Not mMovie.isSingle OrElse Not eSettings.MovieNameMultiOnly) Then
                            If FileUtils.Common.isVideoTS(mMovie.Filename) Then
                                tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts.tbn")
                            ElseIf FileUtils.Common.isBDRip(mMovie.Filename) Then
                                tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index.tbn")
                            Else
                                tPath = String.Concat(pPath, ".tbn")
                            End If
                            If Not File.Exists(tPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(tPath)
                            End If
                        End If

                        If eSettings.MovieTBN AndAlso mMovie.isSingle Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "movie.tbn")
                            If Not File.Exists(tPath) OrElse (Master.eSettings.OverwritePoster) Then
                                imageList.Add(tPath)
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try

            Case Enums.ImageType.Fanart
                'YAMJ Options, Remove after cloning the module
                If eSettings.VideoTSParent AndAlso FileUtils.Common.isVideoTS(mMovie.Filename) Then
                    fPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".fanart.jpg")
                    If Not File.Exists(fPath) OrElse (Master.eSettings.OverwriteFanart) Then
                        imageList.Add(fPath)
                        If eSettings.AutoBD AndAlso Directory.Exists(eSettings.BDPath) Then
                            imageList.Add(Path.Combine(eSettings.BDPath, Path.GetFileName(fPath)))
                        End If
                    End If
                    'YAMJ Options, Remove after cloning the module
                ElseIf eSettings.VideoTSParent AndAlso FileUtils.Common.isBDRip(mMovie.Filename) Then
                    fPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).FullName, Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name), ".fanart.jpg")
                    If Not File.Exists(fPath) OrElse (Master.eSettings.OverwriteFanart) Then
                        imageList.Add(fPath)
                        If eSettings.AutoBD AndAlso Directory.Exists(eSettings.BDPath) Then
                            imageList.Add(Path.Combine(eSettings.BDPath, Path.GetFileName(fPath)))
                        End If
                    End If
                Else
                    Dim tmpName As String = Path.GetFileNameWithoutExtension(mMovie.Filename)
                    fPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, tmpName)

                    If eSettings.MovieNameDotFanartJPG AndAlso (Not mMovie.isSingle OrElse Not eSettings.MovieNameMultiOnly) Then
                        If FileUtils.Common.isVideoTS(mMovie.Filename) Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts.fanart.jpg")
                        ElseIf FileUtils.Common.isBDRip(mMovie.Filename) Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index.fanart.jpg")
                        Else
                            tPath = String.Concat(fPath, ".fanart.jpg")
                        End If
                        If Not File.Exists(tPath) OrElse (Master.eSettings.OverwriteFanart) Then
                            imageList.Add(tPath)
                            If eSettings.AutoBD AndAlso Directory.Exists(eSettings.BDPath) Then
                                If FileUtils.Common.isVideoTS(mMovie.Filename) Then
                                    imageList.Add(Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name, "-fanart.jpg")))
                                ElseIf FileUtils.Common.isBDRip(mMovie.Filename) Then
                                    imageList.Add(Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name, "-fanart.jpg")))
                                Else
                                    imageList.Add(Path.Combine(eSettings.BDPath, Path.GetFileName(tPath)))
                                End If
                            End If
                        End If
                    End If

                    If eSettings.MovieNameFanartJPG AndAlso (Not mMovie.isSingle OrElse Not OutputXBMC_Module.eSettings.MovieNameMultiOnly) Then
                        If FileUtils.Common.isVideoTS(mMovie.Filename) Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts-fanart.jpg")
                        ElseIf FileUtils.Common.isBDRip(mMovie.Filename) Then
                            tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index-fanart.jpg")
                        Else
                            tPath = String.Concat(fPath, "-fanart.jpg")
                        End If

                        If Not File.Exists(tPath) OrElse (Master.eSettings.OverwriteFanart) Then
                            imageList.Add(tPath)
                            If eSettings.AutoBD AndAlso Directory.Exists(eSettings.BDPath) Then
                                If FileUtils.Common.isVideoTS(mMovie.Filename) Then
                                    imageList.Add(Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name, "-fanart.jpg")))
                                ElseIf FileUtils.Common.isBDRip(mMovie.Filename) Then
                                    imageList.Add(Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name, "-fanart.jpg")))
                                    imageList.Add(Path.Combine(eSettings.BDPath, Path.GetFileName(tPath)))
                                End If
                            End If
                        End If
                    End If

                    If eSettings.FanartJPG AndAlso mMovie.isSingle Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "fanart.jpg")
                        If Not File.Exists(tPath) OrElse (Master.eSettings.OverwriteFanart) Then
                            imageList.Add(fPath)
                            If eSettings.AutoBD AndAlso Directory.Exists(eSettings.BDPath) Then
                                If FileUtils.Common.isVideoTS(mMovie.Filename) Then
                                    imageList.Add(Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name, "-fanart.jpg")))
                                ElseIf FileUtils.Common.isBDRip(mMovie.Filename) Then
                                    imageList.Add(Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name, "-fanart.jpg")))
                                Else
                                    imageList.Add(Path.Combine(eSettings.BDPath, String.Concat(tmpName, "-fanart.jpg")))
                                End If
                            End If
                        End If
                    End If
                End If
        End Select

        Return imageList
    End Function

    Public Function SaveMovieInfoSheet(ByRef movieToSave As EmberAPI.Structures.DBMovie) As Boolean Implements EmberAPI.Interfaces.EmberMovieOutputModule.SaveMovieInfoSheet
        Try
            NFO.SaveMovieToNFO(movieToSave)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function InjectSetup() As EmberAPI.Containers.SettingsPanel Implements EmberAPI.Interfaces.EmberMovieOutputModule.InjectSetup
        Dim SPanel As New Containers.SettingsPanel
        Me.fOutputSettings = New frmMovieOutputSettings
        RetrieveMySettings()
        'Me.fOutputSettings.chkEnabled.Checked = Me.Enabled
        SPanel.Name = Me.ModuleName
        SPanel.Text = Master.eLang.GetString(92, "XBMC")
        SPanel.Prefix = "XBMCOutputModule_"
        SPanel.Parent = "pnlMovieOutput"
        SPanel.Type = Master.eLang.GetString(36, "Movies", True)
        SPanel.ImageIndex = If(Me.Enabled, 9, 10)
        SPanel.Order = 100
        SPanel.Panel = Me.fOutputSettings.pnlSettings
        AddHandler Me.fOutputSettings.ModuleSettingsChanged, AddressOf Handle_ModuleSettingsChanged
        AddHandler Me.fOutputSettings.ModuleEnabledChanged, AddressOf Handle_SetupChanged
        fOutputSettings.orderChanged()
        Return SPanel
        'Return Nothing
    End Function
    Private Sub Handle_ModuleSettingsChanged()
        RaiseEvent ModuleSettingsChanged()
    End Sub
    Private Sub Handle_SetupChanged(ByVal state As Boolean, ByVal difforder As Integer)
        RaiseEvent SetupChanged(Me.ModuleName, state, difforder)
    End Sub
    Public Sub SaveSetup(ByVal DoDispose As Boolean) Implements EmberAPI.Interfaces.EmberMovieOutputModule.SaveSetup
        StoreMySettings()
    End Sub

    Public Sub SetupOrderChanged() Implements EmberAPI.Interfaces.EmberMovieOutputModule.SetupOrderChanged

    End Sub

    Public Sub Init(ByVal sAssemblyName As String) Implements EmberAPI.Interfaces.EmberMovieOutputModule.Init
        _AssemblyName = sAssemblyName
        eSettings.Load("Output.")
    End Sub

    Sub StoreMySettings()
        Enabled = fOutputSettings.chkEnabled.Checked
        eSettings.Enabled = fOutputSettings.chkEnabled.Checked
        eSettings.Enabled = fOutputSettings.chkEnabled.Checked
        eSettings.FanartJPG = fOutputSettings.chkFanartJPG.Checked
        eSettings.FolderJPG = fOutputSettings.chkFolderJPG.Checked
        eSettings.MovieJPG = fOutputSettings.chkMovieJPG.Checked
        eSettings.MovieNameDotFanartJPG = fOutputSettings.chkMovieNameDotFanartJPG.Checked
        eSettings.MovieNameFanartJPG = fOutputSettings.chkMovieNameFanartJPG.Checked
        eSettings.MovieNameJPG = fOutputSettings.chkMovieNameJPG.Checked
        eSettings.MovieNameMultiOnly = fOutputSettings.chkMovieNameMultiOnly.Checked
        eSettings.MovieNameNFO = fOutputSettings.chkMovieNameNFO.Checked
        eSettings.MovieNameTBN = fOutputSettings.chkMovieNameTBN.Checked
        eSettings.MovieNFO = fOutputSettings.chkMovieNFO.Checked
        eSettings.MovieTBN = fOutputSettings.chkMovieTBN.Checked
        eSettings.PosterJPG = fOutputSettings.chkPosterJPG.Checked
        eSettings.PosterTBN = fOutputSettings.chkPosterTBN.Checked
        eSettings.AutoBD = fOutputSettings.chkAutoBD.Checked
        eSettings.BDPath = fOutputSettings.txtBDPath.Text
        eSettings.NoSaveImagesToNfo = fOutputSettings.chkNoSaveImagesToNfo.Checked

        eSettings.Save("Output.")
    End Sub
    Sub RetrieveMySettings()
        eSettings.Load("Output.")
        fOutputSettings.chkEnabled.Checked = eSettings.Enabled
        fOutputSettings.chkEnabled.Checked = eSettings.Enabled
        fOutputSettings.chkFanartJPG.Checked = eSettings.FanartJPG
        fOutputSettings.chkFolderJPG.Checked = eSettings.FolderJPG
        fOutputSettings.chkMovieJPG.Checked = eSettings.MovieJPG
        fOutputSettings.chkMovieNameDotFanartJPG.Checked = eSettings.MovieNameDotFanartJPG
        fOutputSettings.chkMovieNameFanartJPG.Checked = eSettings.MovieNameFanartJPG
        fOutputSettings.chkMovieNameJPG.Checked = eSettings.MovieNameJPG
        fOutputSettings.chkMovieNameMultiOnly.Checked = eSettings.MovieNameMultiOnly
        fOutputSettings.chkMovieNameNFO.Checked = eSettings.MovieNameNFO
        fOutputSettings.chkMovieNameTBN.Checked = eSettings.MovieNameTBN
        fOutputSettings.chkMovieNFO.Checked = eSettings.MovieNFO
        fOutputSettings.chkMovieTBN.Checked = eSettings.MovieTBN
        fOutputSettings.chkPosterJPG.Checked = eSettings.PosterJPG
        fOutputSettings.chkPosterTBN.Checked = eSettings.PosterTBN
        fOutputSettings.chkAutoBD.Checked = eSettings.AutoBD
        fOutputSettings.txtBDPath.Text = eSettings.BDPath
        fOutputSettings.chkNoSaveImagesToNfo.Checked = eSettings.NoSaveImagesToNfo
    End Sub

    Public Function IsAllowedToDownload(ByVal mMovie As EmberAPI.Structures.DBMovie, ByVal fType As EmberAPI.Enums.ImageType, Optional ByVal isChange As Boolean = False) As Boolean Implements EmberAPI.Interfaces.EmberMovieOutputModule.IsAllowedToDownload
        Try
            Select Case fType
                Case Enums.ImageType.Fanart
                    If (isChange OrElse (String.IsNullOrEmpty(mMovie.FanartPath) OrElse Master.eSettings.OverwriteFanart)) AndAlso _
                    (eSettings.MovieNameDotFanartJPG OrElse eSettings.MovieNameFanartJPG OrElse eSettings.FanartJPG) AndAlso _
                    AdvancedSettings.GetBooleanSetting("UseTMDB", True) Then
                        Return True
                    Else
                        Return False
                    End If
                Case Else
                    If (isChange OrElse (String.IsNullOrEmpty(mMovie.PosterPath) OrElse Master.eSettings.OverwritePoster)) AndAlso _
                    (eSettings.MovieTBN OrElse eSettings.MovieNameTBN OrElse eSettings.MovieJPG OrElse _
                     eSettings.MovieNameJPG OrElse eSettings.PosterTBN OrElse eSettings.PosterJPG OrElse eSettings.FolderJPG) AndAlso _
                     (AdvancedSettings.GetBooleanSetting("UseIMPA", False) OrElse AdvancedSettings.GetBooleanSetting("UseMPDB", False) OrElse AdvancedSettings.GetBooleanSetting("UseTMDB", True)) Then
                        Return True
                    Else
                        Return False
                    End If
            End Select
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return False
        End Try
    End Function

    Public Function CopyBackDrops(ByVal sPath As String, ByVal sFile As String) As Boolean Implements EmberAPI.Interfaces.EmberMovieOutputModule.CopyBackDrops
        If Not String.IsNullOrEmpty(sPath) Then
            If FileUtils.Common.isVideoTS(sPath) Then
                If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                    FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")))
                Else
                    FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(eSettings.BDPath, Path.GetFileName(sPath)))
                End If
            ElseIf FileUtils.Common.isBDRip(sPath) Then
                If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                    FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName).Name, "-fanart.jpg")))
                Else
                    FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(eSettings.BDPath, Path.GetFileName(sPath)))
                End If
            Else
                If Path.GetFileName(sPath).ToLower = "fanart.jpg" Then
                    FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(eSettings.BDPath, String.Concat(Path.GetFileNameWithoutExtension(sFile), "-fanart.jpg")))
                Else
                    FileUtils.Common.MoveFileWithStream(sPath, Path.Combine(eSettings.BDPath, Path.GetFileName(sPath)))
                End If
            End If
        End If
        Return True
    End Function
End Class

Public Class MySettings
    Private _Enabled As Boolean
    Private _bdpath As String
    Private _autobd As Boolean
    Private _etpath As String
    Private _videotsparent As Boolean ' to remove
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
    Private _nosaveimagestonfo As Boolean
    Sub New()
        _movienamenfo = True
        _movienamefanartjpg = True
        _movienamejpg = True
        _bdpath = String.Empty
        _etpath = String.Empty
    End Sub
    Sub Save(ByVal prefix As String)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "Enabled"), Enabled)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "FanartJPG"), FanartJPG)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "FolderJPG"), FolderJPG)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieJPG"), MovieJPG)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieNameDotFanartJPG"), MovieNameDotFanartJPG)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieNameFanartJPG"), MovieNameFanartJPG)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieNameJPG"), MovieNameJPG)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieNameMultiOnly"), MovieNameMultiOnly)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieNameNFO"), MovieNameNFO)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieNameTBN"), MovieNameTBN)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieNFO"), MovieNFO)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "MovieTBN"), MovieTBN)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "PosterJPG"), PosterJPG)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "PosterTBN"), PosterTBN)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "AutoBD"), AutoBD)
        AdvancedSettings.SetSetting(String.Concat(prefix, "BDPath"), BDPath)
        AdvancedSettings.SetSetting(String.Concat(prefix, "ETPath"), ETPath)
        AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "NoSaveImagesToNfo"), NoSaveImagesToNfo)
        'AdvancedSettings.SetBooleanSetting(String.Concat(prefix, "VideoTSParent"), VideoTSParent)
    End Sub
    Sub Load(ByVal prefix As String)
        Enabled = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "Enabled"), Enabled)
        FanartJPG = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "FanartJPG"), FanartJPG)
        FolderJPG = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "FolderJPG"), FolderJPG)
        MovieJPG = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieJPG"), MovieJPG)
        MovieNameDotFanartJPG = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieNameDotFanartJPG"), MovieNameDotFanartJPG)
        MovieNameFanartJPG = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieNameFanartJPG"), MovieNameFanartJPG)
        MovieNameJPG = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieNameJPG"), MovieNameJPG)
        MovieNameMultiOnly = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieNameMultiOnly"), MovieNameMultiOnly)
        MovieNameNFO = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieNameNFO"), MovieNameNFO)
        MovieNameTBN = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieNameTBN"), MovieNameTBN)
        MovieNFO = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieNFO"), MovieNFO)
        MovieTBN = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "MovieTBN"), MovieTBN)
        PosterJPG = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "PosterJPG"), PosterJPG)
        PosterTBN = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "PosterTBN"), PosterTBN)
        AutoBD = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "AutoBD"), AutoBD)
        BDPath = AdvancedSettings.GetSetting(String.Concat(prefix, "BDPath"), BDPath)
        ETPath = AdvancedSettings.GetSetting(String.Concat(prefix, "ETPath"), ETPath)
        NoSaveImagesToNfo = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "NoSaveImagesToNfo"), NoSaveImagesToNfo)
        'VideoTSParent = AdvancedSettings.GetBooleanSetting(String.Concat(prefix, "VideoTSParent"), VideoTSParent)
    End Sub

    Public Property Enabled() As Boolean
        Get
            Return Me._Enabled
        End Get
        Set(ByVal value As Boolean)
            Me._Enabled = value
        End Set
    End Property
    Public Property AutoBD() As Boolean
        Get
            Return Me._autobd
        End Get
        Set(ByVal value As Boolean)
            Me._autobd = value
        End Set
    End Property
    Public Property BDPath() As String
        Get
            Return Me._bdpath
        End Get
        Set(ByVal value As String)
            Me._bdpath = value
        End Set
    End Property
    Public Property ETPath() As String
        Get
            Return Me._etpath
        End Get
        Set(ByVal value As String)
            Me._etpath = value
        End Set
    End Property
    Public Property VideoTSParent() As Boolean
        Get
            Return False 'Me._videotsparent
        End Get
        Set(ByVal value As Boolean)
            'Me._videotsparent = value
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
    Public Property NoSaveImagesToNfo() As Boolean
        Get
            Return Me._nosaveimagestonfo
        End Get
        Set(ByVal value As Boolean)
            Me._nosaveimagestonfo = value
        End Set
    End Property
End Class