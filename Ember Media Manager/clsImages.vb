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
    Private _isedit As Boolean
    Private Ret As Byte()
    Private ms As MemoryStream = New MemoryStream()

    Public Property [Image]() As Image
        Get
            Return _image
        End Get
        Set(ByVal value As Image)
            _image = value
        End Set
    End Property

    Public Property IsEdit() As Boolean
        Get
            Return _isedit
        End Get
        Set(ByVal value As Boolean)
            _isedit = value
        End Set
    End Property

    Public Sub New()
        Clear()
    End Sub

    Public Sub Clear()
        _isedit = False
        _image = Nothing
    End Sub

    Public Sub Dispose()
        ms.Flush()
        ms.Close()
        ms = Nothing
        Clear()
    End Sub

    Public Sub FromFile(ByVal sPath As String)
        Try
            Using fsImage As New FileStream(sPath, FileMode.Open, FileAccess.Read)
                ms.SetLength(fsImage.Length)
                fsImage.Read(ms.GetBuffer(), 0, Convert.ToInt32(fsImage.Length))
                ms.Flush()
            End Using
            _image = Image.FromStream(ms)
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub FromWeb(ByVal sURL As String)
        Try
            Dim wrRequest As WebRequest = WebRequest.Create(sURL)
            wrRequest.Timeout = 10000
            Using wrResponse As WebResponse = wrRequest.GetResponse()
                _image = Image.FromStream(wrResponse.GetResponseStream)
                wrResponse.Close()
            End Using
            wrRequest = Nothing
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub Delete(ByVal sPath As String, ByVal isFile As Boolean, ByVal fType As Master.ImageType)
        Dim tPath As String = String.Empty

        If fType = Master.ImageType.Fanart Then
            tPath = GetFanartPath(sPath, isFile)
            If Not String.IsNullOrEmpty(tPath) Then
                File.Delete(tPath)
            End If
        Else
            tPath = GetPosterPath(sPath, isFile)
            If Not String.IsNullOrEmpty(tPath) Then
                File.Delete(tPath)
            End If
        End If
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
            Dim pPath As String = String.Empty

            If Master.eSettings.ResizePoster AndAlso (_image.Width > Master.eSettings.PosterWidth OrElse _image.Height > Master.eSettings.PosterHeight) Then
                ResizeImage(Master.eSettings.PosterWidth, Master.eSettings.PosterHeight)
            End If

            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then

                If Master.eSettings.MovieNameJPG Then
                    pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), ".jpg")
                    If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(pPath)
                    End If
                End If

                If Master.eSettings.MovieNameTBN Then
                    pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), ".tbn")
                    If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(pPath)
                    End If
                End If

            Else
                Dim tPath = String.Empty
                Dim tmpName As String = Path.GetFileNameWithoutExtension(sPath)
                pPath = Path.Combine(Directory.GetParent(sPath).FullName, tmpName)

                If Master.eSettings.MovieTBN AndAlso Not isFile Then
                    tPath = Path.Combine(Directory.GetParent(sPath).FullName, "movie.tbn")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath)
                    End If
                End If

                If Master.eSettings.MovieNameTBN Then
                    If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                        tPath = Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.tbn")
                    Else
                        tPath = String.Concat(pPath, ".tbn")
                    End If
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath)
                    End If
                End If

                If Master.eSettings.MovieJPG AndAlso Not isFile Then
                    tPath = Path.Combine(Directory.GetParent(sPath).FullName, "movie.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath)
                    End If
                End If

                If Master.eSettings.MovieNameJPG Then
                    If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                        tPath = Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.jpg")
                    Else
                        tPath = String.Concat(pPath, ".jpg")
                    End If
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath)
                    End If
                End If

                If Master.eSettings.PosterTBN AndAlso Not isFile Then
                    tPath = Path.Combine(Directory.GetParent(sPath).FullName, "poster.tbn")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath)
                    End If
                End If

                If Master.eSettings.PosterJPG AndAlso Not isFile Then
                    tPath = Path.Combine(Directory.GetParent(sPath).FullName, "poster.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath)
                    End If
                End If

                If Master.eSettings.FolderJPG AndAlso Not isFile Then
                    tPath = Path.Combine(Directory.GetParent(sPath).FullName, "folder.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath)
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub ResizeExtraThumb(ByVal fromPath As String, ByVal toPath As String)
        'there has to be a better way to do this. lol
        Me.FromFile(fromPath)
        Dim int As Integer = _image.Width
        Me.ResizeImage(1280, 720)
        Dim bgBMP As Bitmap = New Bitmap(1280, 720, Imaging.PixelFormat.Format32bppRgb)
        Dim grOverlay As Graphics = Graphics.FromImage(bgBMP)
        grOverlay.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        grOverlay.FillRectangle(New SolidBrush(Color.Black), New RectangleF(0, 0, 1280, 720))
        Dim iLeft As Integer = If(_image.Width = 1280, 0, (1280 - _image.Width) / 2)
        Dim iTop As Integer = If(_image.Height = 720, 0, (720 - _image.Height) / 2)
        grOverlay.DrawImage(_image, iLeft, iTop, _image.Width, _image.Height)
        _image = bgBMP
        Me.Save(toPath)
    End Sub

    Public Sub SaveAsFanart(ByVal sPath As String, ByVal isFile As Boolean)
        Try
            Dim fPath As String = String.Empty
            Dim tPath As String = String.Empty

            If Master.eSettings.ResizeFanart AndAlso (_image.Width > Master.eSettings.FanartWidth OrElse _image.Height > Master.eSettings.FanartHeight) Then
                ResizeImage(Master.eSettings.FanartWidth, Master.eSettings.FanartHeight)
            End If

            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                fPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), ".fanart.jpg")
                If Not File.Exists(fPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                    Save(fPath)
                    If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                        Save(Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath)))
                    End If
                End If
            Else
                Dim tmpName As String = Path.GetFileNameWithoutExtension(sPath)
                fPath = Path.Combine(Directory.GetParent(sPath).FullName, tmpName)

                If Master.eSettings.FanartJPG AndAlso Not isFile Then
                    tPath = Path.Combine(Directory.GetParent(sPath).FullName, "fanart.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteFanart) Then
                        Save(tPath)
                        If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                            If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")))
                            Else
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(tmpName, "-fanart.jpg")))
                            End If
                        End If
                    End If
                End If

                If Master.eSettings.MovieNameFanartJPG Then
                    If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                        tPath = Path.Combine(Directory.GetParent(sPath).FullName, "video_ts-fanart.jpg")
                    Else
                        tPath = String.Concat(fPath, "-fanart.jpg")
                    End If
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteFanart) Then
                        Save(tPath)
                        If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                            If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")))
                            Else
                                Save(Path.Combine(Master.eSettings.BDPath, Path.GetFileName(tPath)))
                            End If
                        End If
                    End If
                End If

                If Master.eSettings.MovieNameDotFanartJPG Then
                    If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                        tPath = Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.fanart.jpg")
                    Else
                        tPath = String.Concat(fPath, ".fanart.jpg")
                    End If
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteFanart) Then
                        Save(tPath)
                        If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                            If Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(sPath).FullName).Name, "-fanart.jpg")))
                            Else
                                Save(Path.Combine(Master.eSettings.BDPath, Path.GetFileName(tPath)))
                            End If
                        End If
                    End If
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
                wrRequest.Timeout = 10000
                Using wrResponse As WebResponse = wrRequest.GetResponse()
                    If wrResponse.ContentType.Contains("image") Then
                        tmpImage = Image.FromStream(wrResponse.GetResponseStream)
                    End If
                End Using
                wrRequest = Nothing
            End If
        Catch
        End Try
        Return tmpImage
    End Function

    Public Function GetPreferredImage(ByVal IMDBID As String, ByVal iType As Master.ImageType, ByRef fArt As Media.Fanart, ByRef pThumbs As Media.Poster, Optional ByVal doAsk As Boolean = False) As Boolean

        '//
        ' Try to get the best match between what the user selected in settings and the actual posters downloaded
        '\\

        Dim hasImages As Boolean = False
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

        Try

            If iType = Master.ImageType.Posters Then 'posters
                pThumbs = New Media.Poster
                If Master.eSettings.UseTMDB Then
                    'download all TMBD images
                    tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "poster")

                    'check each one for it's size to see if it matched the preferred size
                    If tmpListTMDB.Count > 0 Then
                        hasImages = True

                        For Each tmdbThumb As Media.Image In tmpListTMDB
                            pThumbs.Thumb.Add(New Media.Posters With {.URL = tmdbThumb.URL})
                        Next

                        For Each iMovie As Media.Image In tmpListTMDB
                            Select Case Master.eSettings.PreferredPosterSize
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

                If Master.eSettings.UseIMPA Then
                    If IsNothing(tmpImage) Then
                        'no poster of the proper size from TMDB found... try IMPA

                        tmpListIMPA = IMPA.GetIMPAPosters(IMDBID)

                        If tmpListIMPA.Count > 0 Then
                            hasImages = True
                            For Each iMovie As Media.Image In tmpListIMPA
                                tmpImage = GenericFromWeb(iMovie.URL)
                                If Not IsNothing(tmpImage) Then
                                    pThumbs.Thumb.Add(New Media.Posters With {.URL = iMovie.URL})
                                    Dim tmpSize As Master.PosterSize = GetImageDims(tmpImage, Master.ImageType.Posters)
                                    If Not tmpSize = Master.eSettings.PreferredPosterSize Then
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
                                        tmpImage = Nothing
                                    Else
                                        'image found
                                        GoTo foundIT
                                    End If
                                End If
                            Next
                        End If
                    End If
                End If

                If Master.eSettings.UseMPDB Then
                    If IsNothing(tmpImage) Then
                        'no poster of the proper size from TMDB or IMPA found... try MPDB

                        tmpListMPDB = MPDB.GetMPDBPosters(IMDBID)

                        If tmpListMPDB.Count > 0 Then
                            hasImages = True
                            For Each iMovie As Media.Image In tmpListMPDB
                                tmpImage = GenericFromWeb(iMovie.URL)
                                If Not IsNothing(tmpImage) Then
                                    pThumbs.Thumb.Add(New Media.Posters With {.URL = iMovie.URL})
                                    Dim tmpSize As Master.PosterSize = GetImageDims(tmpImage, Master.ImageType.Posters)
                                    If Not tmpSize = Master.eSettings.PreferredPosterSize Then
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
                                        tmpImage = Nothing
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
                    If Master.eSettings.UseTMDB Then
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

                    If Master.eSettings.UseIMPA Then
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

                    If Master.eSettings.UseMPDB Then
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

                If Master.eSettings.UseTMDB Then

                    'download all the fanart from TMDB
                    tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "backdrop")

                    If tmpListTMDB.Count > 0 Then
                        hasImages = True
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
                            Select Case Master.eSettings.PreferredPosterSize
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

        _image = tmpImage

        Return hasImages
    End Function

    Public Function GetFanartPath(ByVal sPath As String, ByVal isFile As Boolean) As String

        '//
        ' Get the proper path to fanart
        '\\

        Try
            Dim fPath As String = String.Empty

            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                fPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), ".fanart.jpg")
                If File.Exists(fPath) Then
                    Return fPath
                Else
                    Return String.Empty
                End If
            Else

                Dim tmpName As String = Master.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath))
                fPath = Path.Combine(Directory.GetParent(sPath).FullName, tmpName)

                Dim tmpNameWithStack As String = Path.GetFileNameWithoutExtension(sPath)
                Dim fPathWithStack As String = Path.Combine(Directory.GetParent(sPath).FullName, tmpNameWithStack)

                If Master.eSettings.MovieNameFanartJPG AndAlso File.Exists(String.Concat(fPathWithStack, "-fanart.jpg")) Then
                    Return String.Concat(fPathWithStack, "-fanart.jpg")
                ElseIf Master.eSettings.MovieNameFanartJPG AndAlso File.Exists(String.Concat(fPath, "-fanart.jpg")) Then
                    Return String.Concat(fPath, "-fanart.jpg")
                ElseIf Master.eSettings.MovieNameFanartJPG AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "video_ts-fanart.jpg")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "video_ts-fanart.jpg")
                ElseIf Not isFile AndAlso Master.eSettings.FanartJPG AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "fanart.jpg")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "fanart.jpg")
                ElseIf Master.eSettings.MovieNameDotFanartJPG AndAlso File.Exists(String.Concat(fPathWithStack, ".fanart.jpg")) Then
                    Return String.Concat(fPathWithStack, ".fanart.jpg")
                ElseIf Master.eSettings.MovieNameDotFanartJPG AndAlso File.Exists(String.Concat(fPath, ".fanart.jpg")) Then
                    Return String.Concat(fPath, ".fanart.jpg")
                ElseIf Master.eSettings.MovieNameDotFanartJPG AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.fanart.jpg")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.fanart.jpg")
                Else
                    Return String.Empty
                End If
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
            Dim pPath As String = String.Empty

            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
                pPath = Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name)
                If Master.eSettings.MovieNameJPG AndAlso File.Exists(String.Concat(pPath, ".jpg")) Then
                    Return String.Concat(pPath, ".jpg")
                ElseIf Master.eSettings.MovieNameTBN AndAlso File.Exists(String.Concat(pPath, ".tbn")) Then
                    Return String.Concat(pPath, ".tbn")
                Else
                    Return String.Empty
                End If
            Else
                Dim tmpName As String = Master.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath))
                pPath = Path.Combine(Directory.GetParent(sPath).FullName, tmpName)

                Dim tmpNameWithStack As String = Path.GetFileNameWithoutExtension(sPath)
                Dim pPathWithStack As String = Path.Combine(Directory.GetParent(sPath).FullName, tmpNameWithStack)

                If Not isFile AndAlso Master.eSettings.MovieTBN AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "movie.tbn")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "movie.tbn")
                ElseIf Master.eSettings.MovieNameTBN AndAlso File.Exists(String.Concat(pPathWithStack, ".tbn")) Then
                    Return String.Concat(pPathWithStack, ".tbn")
                ElseIf Master.eSettings.MovieNameTBN AndAlso File.Exists(String.Concat(pPath, ".tbn")) Then
                    Return String.Concat(pPath, ".tbn")
                ElseIf Master.eSettings.MovieNameTBN AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.tbn")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.tbn")
                ElseIf Not isFile AndAlso Master.eSettings.MovieJPG AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "movie.jpg")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "movie.jpg")
                ElseIf Master.eSettings.MovieNameJPG AndAlso File.Exists(String.Concat(pPathWithStack, ".jpg")) Then
                    Return String.Concat(pPathWithStack, ".jpg")
                ElseIf Master.eSettings.MovieNameJPG AndAlso File.Exists(String.Concat(pPath, ".jpg")) Then
                    Return String.Concat(pPath, ".jpg")
                ElseIf Master.eSettings.MovieNameJPG AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.jpg")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "video_ts.jpg")
                ElseIf Not isFile AndAlso Master.eSettings.PosterTBN AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "poster.tbn")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "poster.jpg")
                ElseIf Not isFile AndAlso Master.eSettings.FolderJPG AndAlso File.Exists(Path.Combine(Directory.GetParent(sPath).FullName, "folder.jpg")) Then
                    Return Path.Combine(Directory.GetParent(sPath).FullName, "folder.jpg")
                Else
                    Return String.Empty
                End If
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
                    If (String.IsNullOrEmpty(GetFanartPath(sPath, isFile)) OrElse Master.eSettings.OverwriteFanart) AndAlso _
                    (Master.eSettings.MovieNameDotFanartJPG OrElse Master.eSettings.MovieNameFanartJPG OrElse Master.eSettings.FanartJPG) AndAlso _
                    Master.eSettings.UseTMDB Then
                        Return True
                    Else
                        Return False
                    End If
                Case Else
                    If (String.IsNullOrEmpty(GetPosterPath(sPath, isFile)) OrElse Master.eSettings.OverwritePoster) AndAlso _
                    (Master.eSettings.MovieTBN OrElse Master.eSettings.MovieNameTBN OrElse Master.eSettings.MovieJPG OrElse _
                     Master.eSettings.MovieNameJPG OrElse Master.eSettings.PosterTBN OrElse Master.eSettings.PosterTBN) AndAlso _
                     (Master.eSettings.UseIMPA OrElse Master.eSettings.UseMPDB OrElse Master.eSettings.UseTMDB) Then
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

    Public Sub ResizeImage(ByVal maxWidth As Integer, ByVal maxHeight As Integer)

        Try
            If Not IsNothing(_image) Then
                Dim sPropPerc As Single = 1.0 'no default scaling

                If _image.Width > _image.Height Then
                    sPropPerc = CSng(maxWidth / _image.Width)
                Else
                    sPropPerc = CSng(maxHeight / _image.Height)
                End If

                ' Get the source bitmap.
                Using bmSource As New Bitmap(_image)
                    ' Make a bitmap for the result.
                    Dim bmDest As New Bitmap( _
                    Convert.ToInt32(bmSource.Width * sPropPerc), _
                    Convert.ToInt32(bmSource.Height * sPropPerc))
                    ' Make a Graphics object for the result Bitmap.
                    Using grDest As Graphics = Graphics.FromImage(bmDest)
                        grDest.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                        ' Copy the source image into the destination bitmap.
                        grDest.DrawImage(bmSource, New Rectangle(0, 0, _
                        bmDest.Width, bmDest.Height), New Rectangle(0, 0, _
                        bmSource.Width, bmSource.Height), GraphicsUnit.Pixel)
                    End Using

                    _image = bmDest

                End Using

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
End Class
