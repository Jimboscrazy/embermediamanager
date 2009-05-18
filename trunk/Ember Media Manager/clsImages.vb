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

Public Class Images

    Private _image As Image
    Dim Ret As Byte()
    Dim ms As MemoryStream = New MemoryStream()

    Public Property [Image]() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Image)
            _image = value
        End Set
    End Property

    Public Sub New()
        Clear()
    End Sub

    Public Sub Clear()
        If Not IsNothing(_image) Then
            _image = Nothing
        End If
    End Sub

    Public Sub Dispose()
        ms.Flush()
        ms.Close()
        ms = Nothing
        Clear()
    End Sub

    Public Sub FromFile(ByVal sPath As String)
        Try
            Dim fsImage As New FileStream(sPath, FileMode.Open, FileAccess.Read)
            ms.SetLength(fsImage.Length)
            fsImage.Read(ms.GetBuffer(), 0, CInt(fsImage.Length))
            ms.Flush()
            fsImage.Close()
            _image = Image.FromStream(ms)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub FromWeb(ByVal sURL As String)
        Try
            Dim wrRequest As WebRequest = WebRequest.Create(sURL)
            Dim wrResponse As WebResponse = wrRequest.GetResponse()
            _image = Image.FromStream(wrResponse.GetResponseStream)
            wrResponse.Close()
            wrResponse = Nothing
            wrRequest = Nothing
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub Load(ByVal sPath As String, ByVal isFile As Boolean, ByVal fType As Master.ImageType)
        Try
            Dim tPath As String = String.Empty

            If fType = Master.ImageType.Fanart Then
                tPath = GetFanartPath(sPath, isFile)
                If Not String.IsNullOrEmpty(tPath) Then
                    FromFile(tPath)
                End If
            Else
                tPath = GetPosterPath(sPath, isFile)
                If Not String.IsNullOrEmpty(tPath) Then
                    FromFile(tPath)
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub Save(ByVal sPath As String)
        Try
            If Not File.Exists(sPath) OrElse (Not CBool(File.GetAttributes(sPath) And FileAttributes.ReadOnly)) Then
                ms.Position = 0
                _image.Save(ms, Imaging.ImageFormat.Jpeg)
                Ret = ms.ToArray

                Dim fs As New FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Write)
                fs.Write(Ret, 0, Ret.Length)
                fs.Flush()
                fs.Close()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub SaveAsPoster(ByVal sPath As String, ByVal isFile As Boolean)
        Try
            Dim tmpName As String = Master.CleanStackingMarkers(Master.GetNameFromPath(sPath))
            Dim pPath As String = String.Concat(Directory.GetParent(sPath).FullName, "\", tmpName)
            Dim tPath = String.Empty

            If Master.uSettings.MovieTBN AndAlso Not isFile Then
                tPath = String.Concat(Directory.GetParent(pPath).ToString, "\movie.tbn")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwritePoster Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.MovieNameTBN Then
                tPath = String.Concat(Master.RemoveExtFromPath(pPath), ".tbn")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwritePoster Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.MovieJPG AndAlso Not isFile Then
                tPath = String.Concat(Directory.GetParent(pPath).ToString, "\movie.jpg")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwritePoster Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.MovieNameJPG Then
                tPath = String.Concat(Master.RemoveExtFromPath(pPath), ".jpg")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwritePoster Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.PosterTBN AndAlso Not isFile Then
                tPath = String.Concat(Directory.GetParent(pPath).ToString, "\poster.tbn")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwritePoster Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.PosterJPG AndAlso Not isFile Then
                tPath = String.Concat(Directory.GetParent(pPath).ToString, "\poster.jpg")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwritePoster Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.FolderJPG AndAlso Not isFile Then
                tPath = String.Concat(Directory.GetParent(pPath).ToString, "\folder.jpg")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwritePoster Then
                    Save(tPath)
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub SaveAsFanart(ByVal sPath As String, ByVal isFile As Boolean)
        Try
            Dim tmpName As String = Master.CleanStackingMarkers(Master.GetNameFromPath(sPath))
            Dim fPath As String = String.Concat(Directory.GetParent(sPath).FullName, "\", tmpName)
            Dim tPath As String = String.Empty

            If Master.uSettings.ResizeFanart AndAlso (_image.Width > Master.uSettings.FanartWidth OrElse _image.Height > Master.uSettings.FanartHeight) Then
                _image = ResizeImage(_image, Master.ImageType.Fanart)
            End If
            If Master.uSettings.FanartJPG AndAlso Not isFile Then
                tPath = String.Concat(Directory.GetParent(fPath).ToString, "\fanart.jpg")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwriteFanart Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.MovieNameFanartJPG Then
                tPath = String.Concat(Master.RemoveExtFromPath(fPath), "-fanart.jpg")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwriteFanart Then
                    Save(tPath)
                End If
            End If

            If Master.uSettings.MovieNameDotFanartJPG Then
                tPath = String.Concat(Master.RemoveExtFromPath(fPath), ".fanart.jpg")
                If Not File.Exists(tPath) OrElse Master.uSettings.OverwriteFanart Then
                    Save(tPath)
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function GetImageDims(ByVal imgImage As Image, ByVal imgType As Master.ImageType) As Integer

        '//
        ' Check the size of the image and return a generic name for the size
        '\\

        Dim x As Integer = imgImage.Width
        Dim y As Integer = imgImage.Height

        Try
            If imgType = Master.ImageType.Posters Then
                If (x > y) AndAlso (x > (y * 2)) AndAlso (x > 300) Then
                    'at least twice as wide than tall... consider it wide (also make sure it's big enough)
                    Return Master.PosterSize.Wide
                ElseIf (y > 1000 AndAlso x > 750) OrElse (x > 1000 AndAlso y > 750) Then
                    Return Master.PosterSize.Xlrg
                ElseIf (y > 700 AndAlso x > 500) OrElse (x > 700 AndAlso y > 500) Then
                    Return Master.PosterSize.Lrg
                ElseIf (y > 250 AndAlso x > 150) OrElse (x > 250 AndAlso y > 150) Then
                    Return Master.PosterSize.Mid
                Else
                    Return Master.PosterSize.Small
                End If
            Else
                If (y > 1000 AndAlso x > 750) OrElse (x > 1000 AndAlso y > 750) Then
                    Return Master.FanartSize.Lrg
                ElseIf (y > 700 AndAlso x > 400) OrElse (x > 700 AndAlso y > 400) Then
                    Return Master.FanartSize.Mid
                Else
                    Return Master.FanartSize.Small
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Function

    Private Function GenericFromWeb(ByVal sURL As String) As Image
        Dim tmpImage As Image = Nothing
        Try
            If Regex.IsMatch(sURL, "^(https?://)?(([\w!~*'().&=+$%-]+: )?[\w!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([\w!~*'()-]+\.)*([\w^-][\w-]{0,61})?[\w]\.[a-z]{2,6})(:[0-9]{1,4})?((/*)|(/+[\w!~*'().;?:@&=+$,%#-]+)+/*)$", RegexOptions.IgnoreCase) Then
                Dim wrRequest As WebRequest = WebRequest.Create(sURL)
                Dim wrResponse As WebResponse = wrRequest.GetResponse()
                tmpImage = Image.FromStream(wrResponse.GetResponseStream)
                wrResponse.Close()
                wrResponse = Nothing
                wrRequest = Nothing
            End If
        Catch
        End Try
        Return tmpImage
    End Function

    Public Sub GetPreferredImage(ByVal iType As Master.ImageType, ByRef fArt As Media.Fanart, Optional ByVal doAsk As Boolean = False)

        '//
        ' Try to get the best match between what the user selected in settings and the actual posters downloaded
        '\\


        Dim TMDB As New TMDB.Scraper
        Dim IMPA As New IMPA.Scraper
        Dim MPDB As New MPDB.Scraper
        Dim tmpListTMDB As New List(Of Media.Image)
        Dim tmpListIMPA As New List(Of Media.Image)
        Dim tmpListMPDB As New List(Of Media.Image)
        Dim tmpImage As Image = Nothing
        Dim tmpIMPAX As Image = Nothing
        Dim tmpIMPAL As Image = Nothing
        Dim tmpIMPAM As Image = Nothing
        Dim tmpIMPAS As Image = Nothing
        Dim tmpIMPAW As Image = Nothing
        Dim tmpMPDBX As Image = Nothing
        Dim tmpMPDBL As Image = Nothing
        Dim tmpMPDBM As Image = Nothing
        Dim tmpMPDBS As Image = Nothing
        Dim tmpMPDBW As Image = Nothing
        Dim wrRequest As WebRequest
        Dim wrResponse As WebResponse

        Try

            If iType = Master.ImageType.Posters Then 'posters
                If Master.uSettings.UseTMDB Then
                    'download all TMBD images
                    tmpListTMDB = TMDB.GetTMDBImages(Master.currMovie.IMDBID, "poster")

                    'check each one for it's size to see if it matched the preferred size
                    If tmpListTMDB.Count > 0 Then
                        For Each iMovie As Media.Image In tmpListTMDB
                            Select Case Master.uSettings.PreferredPosterSize
                                Case Master.PosterSize.Xlrg
                                    If iMovie.Description.ToLower = "original" Then
                                        tmpImage = GenericFromWeb(iMovie.URL)
                                        If Not IsNothing(tmpImage) Then GoTo foundIT
                                    End If
                                Case Master.PosterSize.Lrg
                                    If iMovie.Description.ToLower = "mid" Then
                                        tmpImage = GenericFromWeb(iMovie.URL)
                                        If Not IsNothing(tmpImage) Then GoTo foundIT
                                    End If
                                Case Master.PosterSize.Mid
                                    If iMovie.Description.ToLower = "cover" Then
                                        tmpImage = GenericFromWeb(iMovie.URL)
                                        If Not IsNothing(tmpImage) Then GoTo foundIT
                                    End If
                                Case Master.PosterSize.Small
                                    If iMovie.Description.ToLower = "thumb" Then
                                        tmpImage = GenericFromWeb(iMovie.URL)
                                        If Not IsNothing(tmpImage) Then GoTo foundIT
                                    End If
                                    'no "wide" for TMDB
                            End Select
                        Next
                    End If
                End If

                If Master.uSettings.UseIMPA Then
                    If IsNothing(tmpImage) Then
                        'no poster of the proper size from TMDB found... try IMPA

                        tmpListIMPA = IMPA.GetIMPAPosters(Master.currMovie.IMDBID)

                        If tmpListIMPA.Count > 0 Then
                            For Each iMovie As Media.Image In tmpListIMPA
                                tmpImage = GenericFromWeb(iMovie.URL)
                                If Not IsNothing(tmpImage) Then
                                    Dim tmpSize As Master.PosterSize = GetImageDims(tmpImage, Master.ImageType.Posters)
                                    If Not tmpSize = Master.uSettings.PreferredPosterSize Then
                                        tmpImage = Nothing
                                        'cache the first result from each type in case the preferred size is not available
                                        Select Case tmpSize
                                            Case Master.PosterSize.Xlrg
                                                If IsNothing(tmpIMPAX) Then
                                                    tmpIMPAX = tmpImage
                                                End If
                                            Case Master.PosterSize.Lrg
                                                If IsNothing(tmpIMPAL) Then
                                                    tmpIMPAL = tmpImage
                                                End If
                                            Case Master.PosterSize.Mid
                                                If IsNothing(tmpIMPAM) Then
                                                    tmpIMPAM = tmpImage
                                                End If
                                            Case Master.PosterSize.Small
                                                If IsNothing(tmpIMPAS) Then
                                                    tmpIMPAS = tmpImage
                                                End If
                                            Case Master.PosterSize.Wide
                                                If IsNothing(tmpIMPAW) Then
                                                    tmpIMPAW = tmpImage
                                                End If
                                        End Select
                                    Else
                                        'image found
                                        GoTo foundIT
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If

                If Master.uSettings.UseMPDB Then
                    If IsNothing(tmpImage) Then
                        'no poster of the proper size from TMDB or IMPA found... try MPDB

                        tmpListMPDB = MPDB.GetMPDBPosters(Master.currMovie.IMDBID)

                        If tmpListMPDB.Count > 0 Then
                            For Each iMovie As Media.Image In tmpListMPDB
                                tmpImage = GenericFromWeb(iMovie.URL)
                                If Not IsNothing(tmpImage) Then
                                    Dim tmpSize As Master.PosterSize = GetImageDims(tmpImage, Master.ImageType.Posters)
                                    If Not tmpSize = Master.uSettings.PreferredPosterSize Then
                                        tmpImage = Nothing
                                        'cache the first result from each type in case the preferred size is not available
                                        Select Case tmpSize
                                            Case Master.PosterSize.Xlrg
                                                If IsNothing(tmpMPDBX) Then
                                                    tmpMPDBX = tmpImage
                                                End If
                                            Case Master.PosterSize.Lrg
                                                If IsNothing(tmpMPDBL) Then
                                                    tmpMPDBL = tmpImage
                                                End If
                                            Case Master.PosterSize.Mid
                                                If IsNothing(tmpMPDBM) Then
                                                    tmpMPDBM = tmpImage
                                                End If
                                            Case Master.PosterSize.Small
                                                If IsNothing(tmpMPDBS) Then
                                                    tmpMPDBS = tmpImage
                                                End If
                                            Case Master.PosterSize.Wide
                                                If IsNothing(tmpMPDBW) Then
                                                    tmpMPDBW = tmpImage
                                                End If
                                        End Select
                                    Else
                                        'image found
                                        GoTo foundIT
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If

                If IsNothing(tmpImage) AndAlso Not doAsk Then
                    'STILL no image found, just get the first available image, starting with the largest
                    If Master.uSettings.UseTMDB Then
                        'check TMDB first
                        If tmpListTMDB.Count > 0 Then
                            Dim x = From MI As Media.Image In tmpListTMDB Where MI.Description = "original"
                            If x.Count > 0 Then
                                tmpImage = GenericFromWeb(x(0).URL)
                                If Not IsNothing(tmpImage) Then GoTo foundIT
                            End If

                            Dim l = From MI As Media.Image In tmpListTMDB Where MI.Description = "mid"
                            If l.Count > 0 Then
                                tmpImage = GenericFromWeb(l(0).URL)
                                If Not IsNothing(tmpImage) Then GoTo foundIT
                            End If

                            Dim m = From MI As Media.Image In tmpListTMDB Where MI.Description = "cover"
                            If m.Count > 0 Then
                                tmpImage = GenericFromWeb(m(0).URL)
                                If Not IsNothing(tmpImage) Then GoTo foundIT
                            End If

                            Dim s = From MI As Media.Image In tmpListTMDB Where MI.Description = "thumb"
                            If s.Count > 0 Then
                                tmpImage = GenericFromWeb(s(0).URL)
                                If Not IsNothing(tmpImage) Then GoTo foundIT
                            End If

                        End If
                    End If

                    If Master.uSettings.UseIMPA Then
                        If tmpListIMPA.Count > 0 Then
                            If Not IsNothing(tmpIMPAX) Then
                                tmpImage = tmpIMPAX
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpIMPAL) Then
                                tmpImage = tmpIMPAL
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpIMPAM) Then
                                tmpImage = tmpIMPAM
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpIMPAS) Then
                                tmpImage = tmpIMPAS
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpIMPAW) Then
                                tmpImage = tmpIMPAW
                                GoTo foundIT
                            End If
                        End If
                    End If

                    If Master.uSettings.UseMPDB Then
                        If tmpListMPDB.Count > 0 Then
                            If Not IsNothing(tmpMPDBX) Then
                                tmpImage = tmpMPDBX
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpMPDBL) Then
                                tmpImage = tmpMPDBL
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpMPDBM) Then
                                tmpImage = tmpMPDBM
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpMPDBS) Then
                                tmpImage = tmpMPDBS
                                GoTo foundIT
                            End If
                            If Not IsNothing(tmpMPDBW) Then
                                tmpImage = tmpMPDBW
                                GoTo foundIT
                            End If
                        End If
                    End If

                End If


            Else 'fanart

                If Master.uSettings.UseTMDB Then

                    'download all the fanart from TMDB
                    tmpListTMDB = TMDB.GetTMDBImages(Master.currMovie.IMDBID, "backdrop")

                    If tmpListTMDB.Count > 0 Then

                        'setup fanart for nfo
                        Dim thumbLink As String = String.Empty
                        fArt = New Media.Fanart
                        For Each miFanart As Media.Image In tmpListTMDB
                            fArt.URL = "http://www.themoviedb.org"
                            thumbLink = Strings.Replace(miFanart.URL, "http://www.themoviedb.org", String.Empty)
                            If Not Strings.InStr(miFanart.URL, "_thumb") > 0 Then
                                thumbLink = thumbLink.Insert(thumbLink.LastIndexOf("."), "_thumb")
                            End If
                            fArt.Thumb.Add(New Media.Thumb With {.Preview = thumbLink, .Text = Strings.Replace(miFanart.URL, "http://www.themoviedb.org", String.Empty)})
                        Next

                        For Each iMovie As Media.Image In tmpListTMDB
                            Select Case Master.uSettings.PreferredPosterSize
                                Case Master.FanartSize.Lrg
                                    If iMovie.Description.ToLower = "original" Then
                                        tmpImage = GenericFromWeb(iMovie.URL)
                                        GoTo foundIT
                                    End If
                                Case Master.FanartSize.Mid
                                    If iMovie.Description.ToLower = "mid" Then
                                        tmpImage = GenericFromWeb(iMovie.URL)
                                        GoTo foundIT
                                    End If
                                Case Master.FanartSize.Small
                                    If iMovie.Description.ToLower = "thumb" Then
                                        tmpImage = GenericFromWeb(iMovie.URL)
                                        GoTo foundIT
                                    End If
                                    'no "wide" for TMDB
                            End Select
                        Next
                    End If
                End If

                If IsNothing(tmpImage) AndAlso Not doAsk Then
                    'STILL no image found, just get the first available image, starting with the largest

                    If tmpListTMDB.Count > 0 Then
                        Dim l = From MI As Media.Image In tmpListTMDB Where MI.Description = "original"
                        If l.Count > 0 Then
                            tmpImage = GenericFromWeb(l(0).URL)
                            GoTo foundIT
                        End If

                        Dim m = From MI As Media.Image In tmpListTMDB Where MI.Description = "mid"
                        If m.Count > 0 Then
                            tmpImage = GenericFromWeb(m(0).URL)
                            GoTo foundIT
                        End If

                        Dim s = From MI As Media.Image In tmpListTMDB Where MI.Description = "thumb"
                        If s.Count > 0 Then
                            tmpImage = GenericFromWeb(s(0).URL)
                            GoTo foundIT
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

foundIT:

        wrResponse = Nothing
        wrRequest = Nothing
        _image = tmpImage
    End Sub

    Public Shared Function GetFanartPath(ByVal sPath As String, ByVal isFile As Boolean) As String

        '//
        ' Get the proper path to fanart
        '\\

        Try
            Dim tmpName As String = Master.CleanStackingMarkers(Master.GetNameFromPath(sPath))
            Dim fPath As String = String.Concat(Directory.GetParent(sPath).FullName, "\", tmpName)

            If Master.uSettings.MovieNameFanartJPG AndAlso File.Exists(String.Concat(Master.RemoveExtFromPath(fPath), "-fanart.jpg")) Then
                Return String.Concat(Master.RemoveExtFromPath(fPath), "-fanart.jpg")
            ElseIf Not isFile AndAlso Master.uSettings.FanartJPG AndAlso File.Exists(String.Concat(Directory.GetParent(fPath).ToString, "\fanart.jpg")) Then
                Return String.Concat(Directory.GetParent(fPath).ToString, "\fanart.jpg")
            ElseIf Master.uSettings.MovieNameDotFanartJPG AndAlso File.Exists(String.Concat(Master.RemoveExtFromPath(fPath), ".fanart.jpg")) Then
                Return String.Concat(Master.RemoveExtFromPath(fPath), ".fanart.jpg")
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return String.Empty
        End Try

    End Function

    Public Function GetPosterPath(ByVal sPath As String, ByVal isFile As Boolean) As String

        '//
        ' Get the proper path to poster
        '\\
        Try
            Dim tmpName As String = Master.CleanStackingMarkers(Master.GetNameFromPath(sPath))
            Dim pPath As String = String.Concat(Directory.GetParent(sPath).FullName, "\", tmpName)

            If Not isFile AndAlso Master.uSettings.MovieTBN AndAlso File.Exists(String.Concat(Directory.GetParent(pPath).ToString, "\movie.tbn")) Then
                Return String.Concat(Directory.GetParent(pPath).ToString, "\movie.tbn")
            ElseIf Master.uSettings.MovieNameTBN AndAlso File.Exists(String.Concat(Master.RemoveExtFromPath(pPath), ".tbn")) Then
                Return String.Concat(Master.RemoveExtFromPath(pPath), ".tbn")
            ElseIf Not isFile AndAlso Master.uSettings.MovieJPG AndAlso File.Exists(String.Concat(Directory.GetParent(pPath).ToString, "\movie.jpg")) Then
                Return String.Concat(Directory.GetParent(pPath).ToString, "\movie.jpg")
            ElseIf Master.uSettings.MovieNameJPG AndAlso File.Exists(String.Concat(Master.RemoveExtFromPath(pPath), ".jpg")) Then
                Return String.Concat(Master.RemoveExtFromPath(pPath), ".jpg")
            ElseIf Not isFile AndAlso Master.uSettings.PosterTBN AndAlso File.Exists(String.Concat(Directory.GetParent(pPath).ToString, "\poster.tbn")) Then
                Return String.Concat(Directory.GetParent(pPath).ToString, "\poster.jpg")
            ElseIf Not isFile AndAlso Master.uSettings.FolderJPG AndAlso File.Exists(String.Concat(Directory.GetParent(pPath).ToString, "\folder.jpg")) Then
                Return String.Concat(Directory.GetParent(pPath).ToString, "\folder.jpg")
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return String.Empty
        End Try

    End Function

    Public Function IsAllowedToDownload(ByVal sPath As String, ByVal isFile As Boolean, ByVal fType As Master.ImageType) As Boolean

        Try
            Select Case fType
                Case Master.ImageType.Fanart
                    If String.IsNullOrEmpty(GetFanartPath(sPath, isFile)) OrElse Master.uSettings.OverwriteFanart Then
                        Return True
                    Else
                        Return False
                    End If
                Case Else
                    If String.IsNullOrEmpty(GetPosterPath(sPath, isFile)) OrElse Master.uSettings.OverwritePoster Then
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

    Private Function ResizeImage(ByVal theImage As Image, ByVal imgType As Master.ImageType) As Image

        Dim imgOut As Image = Nothing
        Dim maxHeight = Master.uSettings.FanartHeight
        Dim maxWidth = Master.uSettings.FanartWidth
        'for later
        'Dim maxheight = If(imgType = Master.ImageType.Fanart, Master.uSettings.FanartHeight, Master.uSettings.Posterheight)
        Try
            If Not IsNothing(theImage) Then
                Dim sPropPerc As Single = 1.0 'no default scaling

                If theImage.Width > theImage.Height Then
                    sPropPerc = CSng(maxWidth / theImage.Width)
                Else
                    sPropPerc = CSng(maxheight / theImage.Height)
                End If

                ' Get the source bitmap.
                Dim bmSource As New Bitmap(theImage)
                ' Make a bitmap for the result.
                Dim bmDest As New Bitmap( _
                CInt(bmSource.Width * sPropPerc), _
                CInt(bmSource.Height * sPropPerc))
                ' Make a Graphics object for the result Bitmap.
                Dim grDest As Graphics = Graphics.FromImage(bmDest)
                ' Copy the source image into the destination bitmap.
                grDest.DrawImage(bmSource, 0, 0, _
                bmDest.Width + 1, _
                bmDest.Height + 1)
                ' Display the result.
                imgOut = bmDest

                'Clean up
                bmSource = Nothing
                bmDest = Nothing
                grDest = Nothing
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return imgOut
    End Function
End Class
