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
Imports System.Text.RegularExpressions
Imports System.Drawing.Imaging

Public Class Images : Implements IDisposable

    Private _image As Image
    Private _isedit As Boolean
    Private Ret As Byte()
    Private ms As MemoryStream = New MemoryStream()
    Private sHTTP As New HTTP

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

    Public Sub Dispose() Implements IDisposable.Dispose
        ms.Flush()
        ms.Close()
        ms.Dispose()
        ms = Nothing
        Clear()
    End Sub

    Public Sub FromFile(ByVal sPath As String)
        If Not String.IsNullOrEmpty(sPath) AndAlso File.Exists(sPath) Then
            Try
                Using fsImage As New FileStream(sPath, FileMode.Open, FileAccess.Read)
                    ms.SetLength(fsImage.Length)
                    fsImage.Read(ms.GetBuffer(), 0, Convert.ToInt32(fsImage.Length))
                    ms.Flush()
                    _image = New Bitmap(ms)
                End Using
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error: " & sPath)
            End Try
        End If
    End Sub

    Public Sub FromWeb(ByVal sURL As String)
        Try
            _image = New Bitmap(sHTTP.DownloadImage(sURL))
        Catch
        End Try
    End Sub

    Public Sub Delete(ByVal sPath As String)
        If Not String.IsNullOrEmpty(sPath) Then
            File.Delete(sPath)
        End If
    End Sub

    Public Sub Save(ByVal sPath As String, Optional ByVal iQuality As Long = 0)
        Try
            If IsNothing(_image) Then Exit Sub

            Dim doesExist As Boolean = File.Exists(sPath)
            Dim fAtt As New FileAttributes
            If Not String.IsNullOrEmpty(sPath) AndAlso (Not doesExist OrElse (Not CBool(File.GetAttributes(sPath) And FileAttributes.ReadOnly))) Then
                If doesExist Then
                    'get the current attributes to set them back after writing
                    fAtt = File.GetAttributes(sPath)
                    'set attributes to none for writing
                    File.SetAttributes(sPath, FileAttributes.Normal)
                End If

                Using msSave As New MemoryStream
                    Dim retSave() As Byte
                    Dim ICI As ImageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg)
                    Dim EncPars As EncoderParameters = New EncoderParameters(If(iQuality > 0, 2, 1))

                    EncPars.Param(0) = New EncoderParameter(Encoder.RenderMethod, EncoderValue.RenderNonProgressive)

                    If iQuality > 0 Then
                        EncPars.Param(1) = New EncoderParameter(Encoder.Quality, iQuality)
                    End If

                    _image.Save(msSave, ICI, EncPars)

                    retSave = msSave.ToArray

                    Using fs As New FileStream(sPath, FileMode.Create, FileAccess.Write)
                        fs.Write(retSave, 0, retSave.Length)
                        fs.Flush()
                    End Using
                    msSave.Flush()
                End Using

                If doesExist Then File.SetAttributes(sPath, fAtt)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Shared Function GetEncoderInfo(ByVal Format As ImageFormat) As ImageCodecInfo
        Dim Encoders() As ImageCodecInfo = ImageCodecInfo.GetImageEncoders()

        For i As Integer = 0 To UBound(Encoders)
            If Encoders(i).FormatID = Format.Guid Then
                Return Encoders(i)
            End If
        Next

        Return Nothing
    End Function

    Public Function SaveAsPoster(ByVal mMovie As Master.DBMovie) As String

        Dim strReturn As String = String.Empty

        Try
            Dim pPath As String = String.Empty

            If Master.eSettings.ResizePoster AndAlso (_image.Width > Master.eSettings.PosterWidth OrElse _image.Height > Master.eSettings.PosterHeight) Then
                ImageManip.ResizeImage(_image, Master.eSettings.PosterWidth, Master.eSettings.PosterHeight)
            End If

            If Master.eSettings.VideoTSParent AndAlso FileManip.Common.isVideoTS(mMovie.Filename) Then

                If Master.eSettings.MovieNameJPG OrElse Master.eSettings.MovieJPG OrElse Master.eSettings.FolderJPG OrElse Master.eSettings.PosterJPG Then
                    pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".jpg")
                    If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(pPath, Master.eSettings.PosterQuality)
                        strReturn = pPath
                    End If
                ElseIf Master.eSettings.MovieNameTBN OrElse Master.eSettings.MovieTBN OrElse Master.eSettings.PosterTBN Then
                    pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".tbn")
                    If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(pPath, Master.eSettings.PosterQuality)
                        strReturn = pPath
                    End If
                End If
            ElseIf Master.eSettings.VideoTSParent AndAlso FileManip.Common.isBDRip(mMovie.Filename) Then

                If Master.eSettings.MovieNameJPG OrElse Master.eSettings.MovieJPG OrElse Master.eSettings.FolderJPG OrElse Master.eSettings.PosterJPG Then
                    pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).FullName, Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name), ".jpg")
                    If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(pPath, Master.eSettings.PosterQuality)
                        strReturn = pPath
                    End If
                ElseIf Master.eSettings.MovieNameTBN OrElse Master.eSettings.MovieTBN OrElse Master.eSettings.PosterTBN Then
                    pPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).FullName, Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name), ".tbn")
                    If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(pPath, Master.eSettings.PosterQuality)
                        strReturn = pPath
                    End If
                End If
            Else
                Dim tPath As String = String.Empty
                Dim tmpName As String = Path.GetFileNameWithoutExtension(mMovie.Filename)
                pPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, tmpName)

                If Master.eSettings.FolderJPG AndAlso mMovie.isSingle Then
                    tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "folder.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath, Master.eSettings.PosterQuality)
                        strReturn = tPath
                    End If
                End If

                If Master.eSettings.PosterJPG AndAlso mMovie.isSingle Then
                    tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "poster.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath, Master.eSettings.PosterQuality)
                        strReturn = tPath
                    End If
                End If

                If Master.eSettings.PosterTBN AndAlso mMovie.isSingle Then
                    tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "poster.tbn")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath, Master.eSettings.PosterQuality)
                        strReturn = tPath
                    End If
                End If

                If Master.eSettings.MovieNameJPG AndAlso (Not mMovie.isSingle OrElse Not Master.eSettings.MovieNameMultiOnly) Then
                    If FileManip.Common.isVideoTS(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts.jpg")
                    ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index.jpg")
                    Else
                        tPath = String.Concat(pPath, ".jpg")
                    End If
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath, Master.eSettings.PosterQuality)
                        strReturn = tPath
                    End If
                End If

                If Master.eSettings.MovieJPG AndAlso mMovie.isSingle Then
                    tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "movie.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath, Master.eSettings.PosterQuality)
                        strReturn = tPath
                    End If
                End If

                If Master.eSettings.MovieNameTBN AndAlso (Not mMovie.isSingle OrElse Not Master.eSettings.MovieNameMultiOnly) Then
                    If FileManip.Common.isVideoTS(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts.tbn")
                    ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index.tbn")
                    Else
                        tPath = String.Concat(pPath, ".tbn")
                    End If
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath, Master.eSettings.PosterQuality)
                        strReturn = tPath
                    End If
                End If

                If Master.eSettings.MovieTBN AndAlso mMovie.isSingle Then
                    tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "movie.tbn")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                        Save(tPath, Master.eSettings.PosterQuality)
                        strReturn = tPath
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Function SaveAsShowPoster(ByVal mShow As Master.DBTV) As String

        Dim strReturn As String = String.Empty

        Try
            Dim pPath As String = String.Empty

            If Master.eSettings.ResizeShowPoster AndAlso (_image.Width > Master.eSettings.ShowPosterWidth OrElse _image.Height > Master.eSettings.ShowPosterHeight) Then
                ImageManip.ResizeImage(_image, Master.eSettings.ShowPosterWidth, Master.eSettings.ShowPosterHeight)
            End If

            If Master.eSettings.ShowPosterJPG Then
                pPath = Path.Combine(mShow.ShowPath, "poster.jpg")
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteShowPoster) Then
                    Save(pPath, Master.eSettings.ShowPosterQuality)
                    strReturn = pPath
                End If
            End If

            If Master.eSettings.ShowPosterTBN Then
                pPath = Path.Combine(mShow.ShowPath, "poster.tbn")
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteShowPoster) Then
                    Save(pPath, Master.eSettings.ShowPosterQuality)
                    strReturn = pPath
                End If
            End If

            If Master.eSettings.ShowFolderJPG Then
                pPath = Path.Combine(mShow.ShowPath, "folder.jpg")
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteShowPoster) Then
                    Save(pPath, Master.eSettings.ShowPosterQuality)
                    strReturn = pPath
                End If
            End If

            If Master.eSettings.ShowSeasonAll Then
                pPath = Path.Combine(mShow.ShowPath, "season-all.tbn")
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteShowPoster) Then
                    Save(pPath, Master.eSettings.ShowPosterQuality)
                    strReturn = pPath
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Function SaveAsEpPoster(ByVal mShow As Master.DBTV) As String

        Dim strReturn As String = String.Empty

        Try
            Dim pPath As String = String.Empty

            If Master.eSettings.ResizeEpPoster AndAlso (_image.Width > Master.eSettings.EpPosterWidth OrElse _image.Height > Master.eSettings.EpPosterHeight) Then
                ImageManip.ResizeImage(_image, Master.eSettings.EpPosterWidth, Master.eSettings.EpPosterHeight)
            End If

            If Master.eSettings.EpisodeJPG Then
                pPath = String.Concat(FileManip.Common.RemoveExtFromPath(mShow.Filename), ".jpg")
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteEpPoster) Then
                    Save(pPath, Master.eSettings.EpPosterQuality)
                    strReturn = pPath
                End If
            End If

            If Master.eSettings.EpisodeTBN Then
                pPath = String.Concat(FileManip.Common.RemoveExtFromPath(mShow.Filename), ".tbn")
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteEpPoster) Then
                    Save(pPath, Master.eSettings.EpPosterQuality)
                    strReturn = pPath
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Function SaveAsSeasonPoster(ByVal mShow As Master.DBTV) As String
        Dim strReturn As String = String.Empty

        Try
            Dim pPath As String = String.Empty

            If Master.eSettings.ResizeSeasonPoster AndAlso (_image.Width > Master.eSettings.SeasonPosterWidth OrElse _image.Height > Master.eSettings.SeasonPosterHeight) Then
                ImageManip.ResizeImage(_image, Master.eSettings.SeasonPosterWidth, Master.eSettings.SeasonPosterHeight)
            End If

            If Master.eSettings.SeasonPosterTBN OrElse Master.eSettings.SeasonPosterJPG OrElse Master.eSettings.SeasonNameTBN OrElse _
            Master.eSettings.SeasonNameJPG OrElse Master.eSettings.FolderJPG Then
                Dim tPath As String = String.Empty
                Dim tDir As New DirectoryInfo(mShow.ShowPath)
                tPath = tDir.GetDirectories(mShow.ShowPath).FirstOrDefault(Function(s) Regex.IsMatch(s.Name, String.Concat("^(s(eason)?)?[\W_]*0?", mShow.TVEp.Season.ToString, "$"))).FullName
                If Not String.IsNullOrEmpty(tPath) Then
                    If Master.eSettings.SeasonPosterTBN Then
                        pPath = Path.Combine(tPath, "Poster.tbn")
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonPoster) Then
                            Save(pPath, Master.eSettings.SeasonPosterQuality)
                            strReturn = pPath
                        End If
                    End If

                    If Master.eSettings.SeasonPosterJPG Then
                        pPath = Path.Combine(tPath, "Poster.jpg")
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonPoster) Then
                            Save(pPath, Master.eSettings.SeasonPosterQuality)
                            strReturn = pPath
                        End If
                    End If

                    If Master.eSettings.SeasonNameTBN Then
                        pPath = Path.Combine(tPath, String.Concat(FileManip.Common.GetDirectory(tPath), ".tbn"))
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonPoster) Then
                            Save(pPath, Master.eSettings.SeasonPosterQuality)
                            strReturn = pPath
                        End If
                    End If

                    If Master.eSettings.SeasonNameJPG Then
                        pPath = Path.Combine(tPath, String.Concat(FileManip.Common.GetDirectory(tPath), ".jpg"))
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonPoster) Then
                            Save(pPath, Master.eSettings.SeasonPosterQuality)
                            strReturn = pPath
                        End If
                    End If

                    If Master.eSettings.SeasonFolderJPG Then
                        pPath = Path.Combine(tPath, "Folder.jpg")
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonPoster) Then
                            Save(pPath, Master.eSettings.SeasonPosterQuality)
                            strReturn = pPath
                        End If
                    End If
                End If
            End If

            If Master.eSettings.SeasonX Then
                If mShow.TVEp.Season = 0 Then
                    pPath = Path.Combine(mShow.ShowPath, "Season-Specials.jpg")
                Else
                    pPath = Path.Combine(mShow.ShowPath, String.Format("Season{0}.jpg", mShow.TVEp.Season))
                End If
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonPoster) Then
                    Save(pPath, Master.eSettings.SeasonPosterQuality)
                    strReturn = pPath
                End If
            End If

            If Master.eSettings.SeasonXX Then
                If mShow.TVEp.Season = 0 Then
                    pPath = Path.Combine(mShow.ShowPath, "Season-Specials.jpg")
                Else
                    pPath = Path.Combine(mShow.ShowPath, String.Format("Season{0}.jpg", mShow.TVEp.Season.ToString.PadLeft(2, Convert.ToChar("0"))))
                End If
                If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonPoster) Then
                    Save(pPath, Master.eSettings.SeasonPosterQuality)
                    strReturn = pPath
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Sub ResizeExtraThumb(ByVal fromPath As String, ByVal toPath As String)
        Me.FromFile(fromPath)
        If Not Master.eSettings.ETNative Then
            Dim iWidth As Integer = Master.eSettings.ETWidth
            Dim iHeight As Integer = Master.eSettings.ETHeight
            ImageManip.ResizeImage(_image, iWidth, iHeight, Master.eSettings.ETPadding, Color.Black.ToArgb)
        End If
        Me.Save(toPath)
    End Sub

    Public Sub SaveFAasET(ByVal faPath As String, ByVal inPath As String)
        Dim iMod As Integer = 0
        Dim iVal As Integer = 1
        Dim extraPath As String = String.Empty

        If Master.eSettings.VideoTSParent AndAlso FileManip.Common.isVideoTS(inPath) Then
            extraPath = Path.Combine(Directory.GetParent(Directory.GetParent(inPath).FullName).FullName, "extrathumbs")
        ElseIf Master.eSettings.VideoTSParent AndAlso FileManip.Common.isBDRip(inPath) Then
            extraPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(inPath).FullName).FullName).FullName, "extrathumbs")
        Else
            extraPath = Path.Combine(Directory.GetParent(inPath).FullName, "extrathumbs")
        End If

        iMod = Master.GetExtraModifier(extraPath)
        iVal = iMod + 1

        If Not Directory.Exists(extraPath) Then
            Directory.CreateDirectory(extraPath)
        End If

        FileManip.Common.MoveFileWithStream(faPath, Path.Combine(extraPath, String.Concat("thumb", iVal, ".jpg")))

    End Sub

    Public Function SaveAsFanart(ByVal mMovie As Master.DBMovie) As String

        Dim strReturn As String = String.Empty

        Try
            Dim fPath As String = String.Empty
            Dim tPath As String = String.Empty

            If Master.eSettings.ResizeFanart AndAlso (_image.Width > Master.eSettings.FanartWidth OrElse _image.Height > Master.eSettings.FanartHeight) Then
                ImageManip.ResizeImage(_image, Master.eSettings.FanartWidth, Master.eSettings.FanartHeight)
            End If

            If Master.eSettings.VideoTSParent AndAlso FileManip.Common.isVideoTS(mMovie.Filename) Then
                fPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name), ".fanart.jpg")
                If Not File.Exists(fPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                    Save(fPath, Master.eSettings.FanartQuality)
                    strReturn = fPath
                    If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                        Save(Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath)), Master.eSettings.FanartQuality)
                    End If
                End If
            ElseIf Master.eSettings.VideoTSParent AndAlso FileManip.Common.isVideoTS(mMovie.Filename) Then
                fPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).FullName, Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name), ".fanart.jpg")
                If Not File.Exists(fPath) OrElse (IsEdit OrElse Master.eSettings.OverwritePoster) Then
                    Save(fPath, Master.eSettings.FanartQuality)
                    strReturn = fPath
                    If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                        Save(Path.Combine(Master.eSettings.BDPath, Path.GetFileName(fPath)), Master.eSettings.FanartQuality)
                    End If
                End If
            Else
                Dim tmpName As String = Path.GetFileNameWithoutExtension(mMovie.Filename)
                fPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, tmpName)

                If Master.eSettings.MovieNameDotFanartJPG AndAlso (Not mMovie.isSingle OrElse Not Master.eSettings.MovieNameMultiOnly) Then
                    If FileManip.Common.isVideoTS(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts.fanart.jpg")
                    ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index.fanart.jpg")
                    Else
                        tPath = String.Concat(fPath, ".fanart.jpg")
                    End If
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteFanart) Then
                        Save(tPath, Master.eSettings.FanartQuality)
                        strReturn = tPath
                        If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                            If FileManip.Common.isVideoTS(mMovie.Filename) Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name, "-fanart.jpg")), Master.eSettings.FanartQuality)
                            ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name, "-fanart.jpg")), Master.eSettings.FanartQuality)
                            Else
                                Save(Path.Combine(Master.eSettings.BDPath, Path.GetFileName(tPath)), Master.eSettings.FanartQuality)
                            End If
                        End If
                    End If
                End If

                If Master.eSettings.MovieNameFanartJPG AndAlso (Not mMovie.isSingle OrElse Not Master.eSettings.MovieNameMultiOnly) Then
                    If FileManip.Common.isVideoTS(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "video_ts-fanart.jpg")
                    ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                        tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "index-fanart.jpg")
                    Else
                        tPath = String.Concat(fPath, "-fanart.jpg")
                    End If

                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteFanart) Then
                        Save(tPath, Master.eSettings.FanartQuality)
                        strReturn = tPath
                        If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                            If FileManip.Common.isVideoTS(mMovie.Filename) Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name, "-fanart.jpg")), Master.eSettings.FanartQuality)
                            ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name, "-fanart.jpg")), Master.eSettings.FanartQuality)
                            Else
                                Save(Path.Combine(Master.eSettings.BDPath, Path.GetFileName(tPath)), Master.eSettings.FanartQuality)
                            End If
                        End If
                    End If
                End If

                If Master.eSettings.FanartJPG AndAlso mMovie.isSingle Then
                    tPath = Path.Combine(Directory.GetParent(mMovie.Filename).FullName, "fanart.jpg")
                    If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteFanart) Then
                        Save(tPath, Master.eSettings.FanartQuality)
                        strReturn = tPath
                        If Master.eSettings.AutoBD AndAlso Directory.Exists(Master.eSettings.BDPath) Then
                            If FileManip.Common.isVideoTS(mMovie.Filename) Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).Name, "-fanart.jpg")), Master.eSettings.FanartQuality)
                            ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(Directory.GetParent(Directory.GetParent(Directory.GetParent(mMovie.Filename).FullName).FullName).Name, "-fanart.jpg")), Master.eSettings.FanartQuality)
                            Else
                                Save(Path.Combine(Master.eSettings.BDPath, String.Concat(tmpName, "-fanart.jpg")), Master.eSettings.FanartQuality)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Function SaveAsShowFanart(ByVal mShow As Master.DBTV) As String

        Dim strReturn As String = String.Empty

        Try
            Dim tPath As String = String.Empty

            If Master.eSettings.ResizeShowFanart AndAlso (_image.Width > Master.eSettings.ShowFanartWidth OrElse _image.Height > Master.eSettings.ShowFanartHeight) Then
                ImageManip.ResizeImage(_image, Master.eSettings.ShowFanartWidth, Master.eSettings.ShowFanartHeight)
            End If

            If Master.eSettings.ShowDotFanart Then
                tPath = Path.Combine(mShow.ShowPath, String.Concat(Path.GetFileNameWithoutExtension(mShow.ShowPath), ".fanart.jpg"))
                If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteShowFanart) Then
                    Save(tPath, Master.eSettings.ShowFanartQuality)
                    strReturn = tPath
                End If
            End If

            If Master.eSettings.ShowDashFanart Then
                tPath = Path.Combine(mShow.ShowPath, String.Concat(Path.GetFileNameWithoutExtension(mShow.ShowPath), "-fanart.jpg"))
                If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteShowFanart) Then
                    Save(tPath, Master.eSettings.ShowFanartQuality)
                    strReturn = tPath
                End If
            End If

            If Master.eSettings.ShowFanartJPG Then
                tPath = Path.Combine(mShow.ShowPath, "fanart.jpg")
                If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteShowFanart) Then
                    Save(tPath, Master.eSettings.ShowFanartQuality)
                    strReturn = tPath
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Function SaveAsEpFanart(ByVal mShow As Master.DBTV) As String

        Dim strReturn As String = String.Empty

        Try
            Dim tPath As String = String.Empty

            If Master.eSettings.ResizeEpFanart AndAlso (_image.Width > Master.eSettings.EpFanartWidth OrElse _image.Height > Master.eSettings.EpFanartHeight) Then
                ImageManip.ResizeImage(_image, Master.eSettings.EpFanartWidth, Master.eSettings.EpFanartHeight)
            End If

            If Master.eSettings.EpisodeDotFanart Then
                tPath = String.Concat(FileManip.Common.RemoveExtFromPath(mShow.Filename), ".fanart.jpg")
                If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteEpFanart) Then
                    Save(tPath, Master.eSettings.EpFanartQuality)
                    strReturn = tPath
                End If
            End If

            If Master.eSettings.EpisodeDashFanart Then
                tPath = String.Concat(FileManip.Common.RemoveExtFromPath(mShow.Filename), "-fanart.jpg")
                If Not File.Exists(tPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteEpFanart) Then
                    Save(tPath, Master.eSettings.EpFanartQuality)
                    strReturn = tPath
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Function SaveAsSeasonFanart(ByVal mShow As Master.DBTV) As String
        Dim strReturn As String = String.Empty

        Try
            Dim pPath As String = String.Empty

            If Master.eSettings.SeasonFanartJPG OrElse Master.eSettings.SeasonDashFanart OrElse Master.eSettings.SeasonDotFanart Then
                If Master.eSettings.ResizeSeasonFanart AndAlso (_image.Width > Master.eSettings.SeasonFanartWidth OrElse _image.Height > Master.eSettings.SeasonFanartHeight) Then
                    ImageManip.ResizeImage(_image, Master.eSettings.SeasonFanartWidth, Master.eSettings.SeasonFanartHeight)
                End If

                Dim tPath As String = String.Empty
                Dim tDir As New DirectoryInfo(mShow.ShowPath)

                tPath = tDir.GetDirectories(mShow.ShowPath).FirstOrDefault(Function(s) Regex.IsMatch(s.Name, String.Concat("^(s(eason)?)?[\W_]*0?", mShow.TVEp.Season.ToString, "$"))).FullName
                If Not String.IsNullOrEmpty(tPath) Then
                    If Master.eSettings.SeasonDotFanart Then
                        pPath = Path.Combine(tPath, String.Concat(FileManip.Common.GetDirectory(tPath), ".fanart.jpg"))
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonFanart) Then
                            Save(pPath, Master.eSettings.SeasonFanartQuality)
                            strReturn = pPath
                        End If
                    End If

                    If Master.eSettings.SeasonDashFanart Then
                        pPath = Path.Combine(tPath, String.Concat(FileManip.Common.GetDirectory(tPath), "-fanart.jpg"))
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonFanart) Then
                            Save(pPath, Master.eSettings.SeasonFanartQuality)
                            strReturn = pPath
                        End If
                    End If

                    If Master.eSettings.SeasonFanartJPG Then
                        pPath = Path.Combine(tPath, "Fanart.jpg")
                        If Not File.Exists(pPath) OrElse (IsEdit OrElse Master.eSettings.OverwriteSeasonFanart) Then
                            Save(pPath, Master.eSettings.SeasonFanartQuality)
                            strReturn = pPath
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return strReturn
    End Function

    Public Shared Function GetPosterDims(ByVal imgImage As Image) As Master.PosterSize

        '//
        ' Check the size of the image and return a generic name for the size
        '\\

        Dim x As Integer = imgImage.Width
        Dim y As Integer = imgImage.Height

        Try
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
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Function

    Public Shared Function GetFanartDims(ByVal imgImage As Image) As Master.FanartSize

        '//
        ' Check the size of the image and return a generic name for the size
        '\\

        Dim x As Integer = imgImage.Width
        Dim y As Integer = imgImage.Height

        Try
            If (y > 1000 AndAlso x > 750) OrElse (x > 1000 AndAlso y > 750) Then
                Return Master.FanartSize.Lrg
            ElseIf (y > 700 AndAlso x > 400) OrElse (x > 700 AndAlso y > 400) Then
                Return Master.FanartSize.Mid
            Else
                Return Master.FanartSize.Small
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Function

    Public Function GetPreferredImage(ByVal IMDBID As String, ByVal iType As Master.ImageType, ByRef imgResult As Master.ImgResult, ByVal sPath As String, ByVal doETs As Boolean, Optional ByVal doAsk As Boolean = False) As Boolean

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

        Dim CachePath As String = String.Concat(Master.TempPath, Path.DirectorySeparatorChar, IMDBID, Path.DirectorySeparatorChar, If(iType = Master.ImageType.Posters, "posters", "fanart"))

        Try

            If iType = Master.ImageType.Posters Then 'posters

                If Master.eSettings.UseImgCacheUpdaters Then
                    Dim lFi As New List(Of FileInfo)

                    If Not Directory.Exists(CachePath) Then
                        Directory.CreateDirectory(CachePath)
                    Else
                        Dim di As New DirectoryInfo(CachePath)

                        Try
                            lFi.AddRange(di.GetFiles("*.jpg"))
                        Catch
                        End Try
                    End If

                    If lFi.Count > 0 Then
                        Dim tImage As Media.Image
                        For Each sFile As FileInfo In lFi
                            tImage = New Media.Image
                            tImage.WebImage.FromFile(sFile.FullName)
                            Select Case True
                                Case sFile.Name.Contains("(original)")
                                    tImage.Description = "original"
                                Case sFile.Name.Contains("(mid)")
                                    tImage.Description = "mid"
                                Case sFile.Name.Contains("(cover)")
                                    tImage.Description = "cover"
                                Case sFile.Name.Contains("(thumb)")
                                    tImage.Description = "thumb"
                                Case sFile.Name.Contains("(poster)")
                                    tImage.Description = "poster"
                            End Select
                            tImage.URL = Regex.Match(sFile.Name, "\(url=(.*?)\)").Groups(1).ToString
                            If Not Master.eSettings.NoSaveImagesToNfo Then imgResult.Posters.Add(tImage.URL)
                            tmpListTMDB.Add(tImage)
                            Me.Clear()
                        Next
                    Else
                        If Master.eSettings.UseTMDB Then
                            tmpListTMDB.AddRange(TMDB.GetTMDBImages(IMDBID, "poster"))
                        End If

                        If Master.eSettings.UseIMPA Then
                            tmpListTMDB.AddRange(IMPA.GetIMPAPosters(IMDBID))
                        End If

                        If Master.eSettings.UseMPDB Then
                            tmpListTMDB.AddRange(MPDB.GetMPDBPosters(IMDBID))
                        End If

                        For Each tmdbThumb As Media.Image In tmpListTMDB
                            tmdbThumb.WebImage.FromWeb(tmdbThumb.URL)
                            If Not IsNothing(tmdbThumb.WebImage.Image) Then
                                If Not Master.eSettings.NoSaveImagesToNfo Then imgResult.Posters.Add(tmdbThumb.URL)
                                _image = tmdbThumb.WebImage.Image
                                Save(Path.Combine(CachePath, String.Concat("poster_(", tmdbThumb.Description, ")_(url=", StringManip.CleanURL(tmdbThumb.URL), ").jpg")))
                            End If
                            Me.Clear()
                        Next
                    End If

                    If tmpListTMDB.Count > 0 Then
                        hasImages = True

                        'remove all entries without images
                        For i As Integer = tmpListTMDB.Count - 1 To 0 Step -1
                            If IsNothing(tmpListTMDB(i).WebImage.Image) Then
                                tmpListTMDB.RemoveAt(i)
                            End If
                        Next

                        For Each iMovie As Media.Image In tmpListTMDB
                            If GetPosterDims(iMovie.WebImage.Image) = Master.eSettings.PreferredPosterSize Then
                                _image = iMovie.WebImage.Image
                                GoTo foundit
                            End If
                        Next

                        If Not doAsk Then
                            _image = tmpListTMDB.OrderBy(Function(i) i.WebImage.Image.Height + i.WebImage.Image.Height).FirstOrDefault.WebImage.Image
                        End If
                    End If
                Else
                    If Master.eSettings.UseTMDB Then
                        'download all TMBD images
                        tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "poster")

                        'check each one for it's size to see if it matched the preferred size
                        If tmpListTMDB.Count > 0 Then
                            hasImages = True

                            If Not Master.eSettings.NoSaveImagesToNfo Then
                                For Each tmdbThumb As Media.Image In tmpListTMDB
                                    imgResult.Posters.Add(tmdbThumb.URL)
                                Next
                            End If

                            For Each iMovie As Media.Image In tmpListTMDB
                                Select Case Master.eSettings.PreferredPosterSize
                                    Case Master.PosterSize.Xlrg
                                        If iMovie.Description.ToLower = "original" Then
                                            FromWeb(iMovie.URL)
                                            If Not IsNothing(_image) Then GoTo foundIT
                                        End If
                                    Case Master.PosterSize.Lrg
                                        If iMovie.Description.ToLower = "mid" Then
                                            FromWeb(iMovie.URL)
                                            If Not IsNothing(_image) Then GoTo foundIT
                                        End If
                                    Case Master.PosterSize.Mid
                                        If iMovie.Description.ToLower = "cover" Then
                                            FromWeb(iMovie.URL)
                                            If Not IsNothing(_image) Then GoTo foundIT
                                        End If
                                    Case Master.PosterSize.Small
                                        If iMovie.Description.ToLower = "thumb" Then
                                            FromWeb(iMovie.URL)
                                            If Not IsNothing(_image) Then GoTo foundIT
                                        End If
                                        'no "wide" for TMDB
                                End Select
                                Me.Clear()
                            Next
                        End If
                    End If

                    If Master.eSettings.UseIMPA Then
                        If IsNothing(_image) Then
                            'no poster of the proper size from TMDB found... try IMPA

                            tmpListIMPA = IMPA.GetIMPAPosters(IMDBID)

                            If tmpListIMPA.Count > 0 Then
                                hasImages = True
                                For Each iImage As Media.Image In tmpListIMPA
                                    FromWeb(iImage.URL)
                                    If Not IsNothing(_image) Then
                                        If Not Master.eSettings.NoSaveImagesToNfo Then imgResult.Posters.Add(iImage.URL)
                                        Dim tmpSize As Master.PosterSize = GetPosterDims(_image)
                                        If Not tmpSize = Master.eSettings.PreferredPosterSize Then
                                            'cache the first result from each type in case the preferred size is not available
                                            Select Case tmpSize
                                                Case Master.PosterSize.Xlrg
                                                    If IsNothing(tmpIMPAX) Then
                                                        tmpIMPAX = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Lrg
                                                    If IsNothing(tmpIMPAL) Then
                                                        tmpIMPAL = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Mid
                                                    If IsNothing(tmpIMPAM) Then
                                                        tmpIMPAM = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Small
                                                    If IsNothing(tmpIMPAS) Then
                                                        tmpIMPAS = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Wide
                                                    If IsNothing(tmpIMPAW) Then
                                                        tmpIMPAW = New Bitmap(_image)
                                                    End If
                                            End Select
                                        Else
                                            'image found
                                            GoTo foundIT
                                        End If
                                    End If
                                    Me.Clear()
                                Next
                            End If
                        End If
                    End If

                    If Master.eSettings.UseMPDB Then
                        If IsNothing(_image) Then
                            'no poster of the proper size from TMDB or IMPA found... try MPDB

                            tmpListMPDB = MPDB.GetMPDBPosters(IMDBID)

                            If tmpListMPDB.Count > 0 Then
                                hasImages = True
                                For Each iImage As Media.Image In tmpListMPDB
                                    FromWeb(iImage.URL)
                                    If Not IsNothing(_image) Then
                                        If Not Master.eSettings.NoSaveImagesToNfo Then imgResult.Posters.Add(iImage.URL)
                                        Dim tmpSize As Master.PosterSize = GetPosterDims(_image)
                                        If Not tmpSize = Master.eSettings.PreferredPosterSize Then
                                            'cache the first result from each type in case the preferred size is not available
                                            Select Case tmpSize
                                                Case Master.PosterSize.Xlrg
                                                    If IsNothing(tmpMPDBX) Then
                                                        tmpMPDBX = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Lrg
                                                    If IsNothing(tmpMPDBL) Then
                                                        tmpMPDBL = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Mid
                                                    If IsNothing(tmpMPDBM) Then
                                                        tmpMPDBM = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Small
                                                    If IsNothing(tmpMPDBS) Then
                                                        tmpMPDBS = New Bitmap(_image)
                                                    End If
                                                Case Master.PosterSize.Wide
                                                    If IsNothing(tmpMPDBW) Then
                                                        tmpMPDBW = New Bitmap(_image)
                                                    End If
                                            End Select
                                        Else
                                            'image found
                                            GoTo foundIT
                                        End If
                                    End If
                                    Me.Clear()
                                Next
                            End If
                        End If
                    End If

                    If IsNothing(_image) AndAlso Not doAsk Then
                        'STILL no image found, just get the first available image, starting with the largest
                        If Master.eSettings.UseTMDB Then
                            'check TMDB first
                            If tmpListTMDB.Count > 0 Then
                                Dim x = From MI As Media.Image In tmpListTMDB Where MI.Description = "original"
                                If x.Count > 0 Then
                                    FromWeb(x(0).URL)
                                    If Not IsNothing(_image) Then GoTo foundIT
                                End If

                                Dim l = From MI As Media.Image In tmpListTMDB Where MI.Description = "mid"
                                If l.Count > 0 Then
                                    FromWeb(l(0).URL)
                                    If Not IsNothing(_image) Then GoTo foundIT
                                End If

                                Dim m = From MI As Media.Image In tmpListTMDB Where MI.Description = "cover"
                                If m.Count > 0 Then
                                    FromWeb(m(0).URL)
                                    If Not IsNothing(_image) Then GoTo foundIT
                                End If

                                Dim s = From MI As Media.Image In tmpListTMDB Where MI.Description = "thumb"
                                If s.Count > 0 Then
                                    FromWeb(s(0).URL)
                                    If Not IsNothing(_image) Then GoTo foundIT
                                End If

                            End If
                        End If

                        Me.Clear()

                        If Master.eSettings.UseIMPA Then
                            If tmpListIMPA.Count > 0 Then
                                If Not IsNothing(tmpIMPAX) Then
                                    _image = New Bitmap(tmpIMPAX)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAL) Then
                                    _image = New Bitmap(tmpIMPAL)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAM) Then
                                    _image = New Bitmap(tmpIMPAM)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAS) Then
                                    _image = New Bitmap(tmpIMPAS)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAW) Then
                                    _image = New Bitmap(tmpIMPAW)
                                    GoTo foundIT
                                End If
                            End If
                        End If

                        Me.Clear()

                        If Master.eSettings.UseMPDB Then
                            If tmpListMPDB.Count > 0 Then
                                If Not IsNothing(tmpMPDBX) Then
                                    _image = New Bitmap(tmpMPDBX)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBL) Then
                                    _image = New Bitmap(tmpMPDBL)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBM) Then
                                    _image = New Bitmap(tmpMPDBM)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBS) Then
                                    _image = New Bitmap(tmpMPDBS)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBW) Then
                                    _image = New Bitmap(tmpMPDBW)
                                    GoTo foundIT
                                End If
                            End If
                        End If

                        Me.Clear()

                    End If

                End If

            Else 'fanart

                If Master.eSettings.UseTMDB Then

                    Dim ETHashes As New List(Of String)
                    If Master.eSettings.AutoET AndAlso doETs Then
                        ETHashes = HashFile.CurrentETHashes(sPath)
                    End If

                    If Master.eSettings.UseImgCacheUpdaters Then
                        Dim lFi As New List(Of FileInfo)

                        If Not Directory.Exists(CachePath) Then
                            Directory.CreateDirectory(CachePath)
                        Else
                            Dim di As New DirectoryInfo(CachePath)

                            Try
                                lFi.AddRange(di.GetFiles("*.jpg"))
                            Catch
                            End Try
                        End If

                        If lFi.Count > 0 Then
                            Dim tImage As Media.Image
                            For Each sFile As FileInfo In lFi
                                tImage = New Media.Image
                                tImage.WebImage.FromFile(sFile.FullName)
                                Select Case True
                                    Case sFile.Name.Contains("(original)")
                                        tImage.Description = "original"
                                        If Master.eSettings.AutoET AndAlso doETs AndAlso Master.eSettings.AutoETSize = Master.FanartSize.Lrg Then
                                            If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                                SaveFAasET(sFile.FullName, sPath)
                                            End If
                                        End If
                                    Case sFile.Name.Contains("(mid)")
                                        tImage.Description = "mid"
                                        If Master.eSettings.AutoET AndAlso doETs AndAlso Master.eSettings.AutoETSize = Master.FanartSize.Mid Then
                                            If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                                SaveFAasET(sFile.FullName, sPath)
                                            End If
                                        End If
                                    Case sFile.Name.Contains("(thumb)")
                                        tImage.Description = "thumb"
                                        If Master.eSettings.AutoET AndAlso doETs AndAlso Master.eSettings.AutoETSize = Master.FanartSize.Small Then
                                            If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                                SaveFAasET(sFile.FullName, sPath)
                                            End If
                                        End If
                                End Select
                                tImage.URL = Regex.Match(sFile.Name, "\(url=(.*?)\)").Groups(1).ToString
                                tmpListTMDB.Add(tImage)
                                Me.Clear()
                            Next
                        Else
                            'download all the fanart from TMDB
                            tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "backdrop")

                            If tmpListTMDB.Count > 0 Then

                                'setup fanart for nfo
                                Dim thumbLink As String = String.Empty
                                imgResult.Fanart.URL = "http://images.themoviedb.org"
                                For Each miFanart As Media.Image In tmpListTMDB
                                    miFanart.WebImage.FromWeb(miFanart.URL)
                                    If Not IsNothing(miFanart.WebImage.Image) Then
                                        _image = miFanart.WebImage.Image
                                        Dim savePath As String = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                        Save(savePath)
                                        If Master.eSettings.AutoET AndAlso doETs Then
                                            Select Case miFanart.Description.ToLower
                                                Case "original"
                                                    If Master.eSettings.AutoETSize = Master.FanartSize.Lrg Then
                                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                            SaveFAasET(savePath, sPath)
                                                        End If
                                                    End If
                                                Case "mid"
                                                    If Master.eSettings.AutoETSize = Master.FanartSize.Mid Then
                                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                            SaveFAasET(savePath, sPath)
                                                        End If
                                                    End If
                                                Case "thumb"
                                                    If Master.eSettings.AutoETSize = Master.FanartSize.Small Then
                                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                            SaveFAasET(savePath, sPath)
                                                        End If
                                                    End If
                                            End Select
                                        End If
                                        If Not Master.eSettings.NoSaveImagesToNfo Then
                                            If Not miFanart.URL.Contains("_thumb.") Then
                                                thumbLink = miFanart.URL.Replace("http://images.themoviedb.org", String.Empty)
                                                If thumbLink.Contains("_poster.") Then
                                                    thumbLink = thumbLink.Replace("_poster.", "_thumb.")
                                                Else
                                                    thumbLink = thumbLink.Insert(thumbLink.LastIndexOf("."), "_thumb")
                                                End If
                                                imgResult.Fanart.Thumb.Add(New Media.Thumb With {.Preview = thumbLink, .Text = miFanart.URL.Replace("http://images.themoviedb.org", String.Empty)})
                                            End If
                                        End If
                                    End If
                                    Me.Clear()
                                Next
                            End If
                        End If

                        If tmpListTMDB.Count > 0 Then
                            hasImages = True
                            'remove all entries without images
                            For i As Integer = tmpListTMDB.Count - 1 To 0 Step -1
                                If IsNothing(tmpListTMDB(i).WebImage.Image) Then
                                    tmpListTMDB.RemoveAt(i)
                                End If
                            Next

                            For Each iMovie As Media.Image In tmpListTMDB
                                If GetFanartDims(iMovie.WebImage.Image) = Master.eSettings.PreferredFanartSize Then
                                    _image = iMovie.WebImage.Image
                                    GoTo foundit
                                End If
                            Next

                            Me.Clear()

                            If Not doAsk Then
                                _image = tmpListTMDB.OrderBy(Function(i) i.WebImage.Image.Height + i.WebImage.Image.Height).FirstOrDefault.WebImage.Image
                            End If

                        End If
                    Else
                        'download all the fanart from TMDB
                        tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "backdrop")

                        If tmpListTMDB.Count > 0 Then
                            hasImages = True

                            'setup fanart for nfo
                            Dim thumbLink As String = String.Empty
                            If Not Master.eSettings.NoSaveImagesToNfo Then
                                imgResult.Fanart.URL = "http://images.themoviedb.org"
                                For Each miFanart As Media.Image In tmpListTMDB
                                    If Not miFanart.URL.Contains("_thumb.") Then
                                        thumbLink = miFanart.URL.Replace("http://images.themoviedb.org", String.Empty)
                                        If thumbLink.Contains("_poster.") Then
                                            thumbLink = thumbLink.Replace("_poster.", "_thumb.")
                                        Else
                                            thumbLink = thumbLink.Insert(thumbLink.LastIndexOf("."), "_thumb")
                                        End If
                                        imgResult.Fanart.Thumb.Add(New Media.Thumb With {.Preview = thumbLink, .Text = miFanart.URL.Replace("http://images.themoviedb.org", String.Empty)})
                                    End If
                                Next
                            End If


                            If Master.eSettings.AutoET AndAlso doETs Then

                                If Not Directory.Exists(CachePath) Then
                                    Directory.CreateDirectory(CachePath)
                                End If

                                Dim savePath As String = String.Empty
                                For Each miFanart As Media.Image In tmpListTMDB
                                    Select Case miFanart.Description.ToLower
                                        Case "original"
                                            If Master.eSettings.AutoETSize = Master.FanartSize.Lrg Then
                                                miFanart.WebImage.FromWeb(miFanart.URL)
                                                If Not IsNothing(miFanart.WebImage.Image) Then
                                                    _image = miFanart.WebImage.Image
                                                    savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                                    Save(savePath)
                                                    If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                        SaveFAasET(savePath, sPath)
                                                    End If
                                                End If
                                            End If
                                        Case "mid"
                                            If Master.eSettings.AutoETSize = Master.FanartSize.Mid Then
                                                miFanart.WebImage.FromWeb(miFanart.URL)
                                                If Not IsNothing(miFanart.WebImage.Image) Then
                                                    _image = miFanart.WebImage.Image
                                                    savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                                    Save(savePath)
                                                    If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                        SaveFAasET(savePath, sPath)
                                                    End If
                                                End If
                                            End If
                                        Case "thumb"
                                            If Master.eSettings.AutoETSize = Master.FanartSize.Small Then
                                                miFanart.WebImage.FromWeb(miFanart.URL)
                                                If Not IsNothing(miFanart.WebImage.Image) Then
                                                    _image = miFanart.WebImage.Image
                                                    savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                                    Save(savePath)
                                                    If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                        SaveFAasET(savePath, sPath)
                                                    End If
                                                End If
                                            End If
                                    End Select
                                Next

                                Me.Clear()
                                FileManip.Delete.DeleteDirectory(CachePath)
                            End If

                            For Each iMovie As Media.Image In tmpListTMDB
                                Select Case Master.eSettings.PreferredFanartSize
                                    Case Master.FanartSize.Lrg
                                        If iMovie.Description.ToLower = "original" Then
                                            If Not IsNothing(iMovie.WebImage.Image) Then
                                                _image = iMovie.WebImage.Image
                                            Else
                                                FromWeb(iMovie.URL)
                                            End If
                                            GoTo foundIT
                                        End If
                                    Case Master.FanartSize.Mid
                                        If iMovie.Description.ToLower = "mid" Then
                                            If Not IsNothing(iMovie.WebImage.Image) Then
                                                _image = iMovie.WebImage.Image
                                            Else
                                                FromWeb(iMovie.URL)
                                            End If
                                            GoTo foundIT
                                        End If
                                    Case Master.FanartSize.Small
                                        If iMovie.Description.ToLower = "thumb" Then
                                            If Not IsNothing(iMovie.WebImage.Image) Then
                                                _image = iMovie.WebImage.Image
                                            Else
                                                FromWeb(iMovie.URL)
                                            End If
                                            GoTo foundIT
                                        End If
                                End Select
                            Next

                            Me.Clear()

                            If IsNothing(_image) AndAlso Not doAsk Then

                                'STILL no image found, just get the first available image, starting with the largest

                                Dim l = From MI As Media.Image In tmpListTMDB Where MI.Description = "original"
                                If l.Count > 0 Then
                                    FromWeb(l(0).URL)
                                    GoTo foundIT
                                End If

                                Dim m = From MI As Media.Image In tmpListTMDB Where MI.Description = "mid"
                                If m.Count > 0 Then
                                    FromWeb(m(0).URL)
                                    GoTo foundIT
                                End If

                                Dim s = From MI As Media.Image In tmpListTMDB Where MI.Description = "thumb"
                                If s.Count > 0 Then
                                    FromWeb(s(0).URL)
                                    GoTo foundIT
                                End If

                            End If

                            Me.Clear()

                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

foundIT:
        TMDB = Nothing
        IMPA = Nothing
        MPDB = Nothing
        tmpListTMDB = Nothing
        tmpListIMPA = Nothing
        tmpListMPDB = Nothing
        Return hasImages
    End Function

    Public Sub GetPreferredFAasET(ByVal IMDBID As String, ByVal sPath As String)
        If Master.eSettings.UseTMDB Then
            Dim TMDB As New TMDB.Scraper
            Dim tmpListTMDB As New List(Of Media.Image)
            Dim ETHashes As New List(Of String)

            Dim CachePath As String = String.Concat(Master.TempPath, Path.DirectorySeparatorChar, IMDBID, Path.DirectorySeparatorChar, "fanart")

            If Master.eSettings.AutoET Then
                ETHashes = HashFile.CurrentETHashes(sPath)
            End If

            If Master.eSettings.UseImgCacheUpdaters Then
                Dim lFi As New List(Of FileInfo)

                If Not Directory.Exists(CachePath) Then
                    Directory.CreateDirectory(CachePath)
                Else
                    Dim di As New DirectoryInfo(CachePath)

                    Try
                        lFi.AddRange(di.GetFiles("*.jpg"))
                    Catch
                    End Try
                End If

                If lFi.Count > 0 Then
                    For Each sFile As FileInfo In lFi
                        Select Case True
                            Case sFile.Name.Contains("(original)")
                                If Master.eSettings.AutoET AndAlso Master.eSettings.AutoETSize = Master.FanartSize.Lrg Then
                                    If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                        SaveFAasET(sFile.FullName, sPath)
                                    End If
                                End If
                            Case sFile.Name.Contains("(mid)")
                                If Master.eSettings.AutoET AndAlso Master.eSettings.AutoETSize = Master.FanartSize.Mid Then
                                    If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                        SaveFAasET(sFile.FullName, sPath)
                                    End If
                                End If
                            Case sFile.Name.Contains("(thumb)")
                                If Master.eSettings.AutoET AndAlso Master.eSettings.AutoETSize = Master.FanartSize.Small Then
                                    If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                        SaveFAasET(sFile.FullName, sPath)
                                    End If
                                End If
                        End Select
                    Next
                Else
                    'download all the fanart from TMDB
                    tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "backdrop")

                    If tmpListTMDB.Count > 0 Then

                        'setup fanart for nfo
                        Dim thumbLink As String = String.Empty
                        For Each miFanart As Media.Image In tmpListTMDB
                            miFanart.WebImage.FromWeb(miFanart.URL)
                            If Not IsNothing(miFanart.WebImage.Image) Then
                                _image = miFanart.WebImage.Image
                                Dim savePath As String = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                Save(savePath)
                                If Master.eSettings.AutoET Then
                                    Select Case miFanart.Description.ToLower
                                        Case "original"
                                            If Master.eSettings.AutoETSize = Master.FanartSize.Lrg Then
                                                If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                    SaveFAasET(savePath, sPath)
                                                End If
                                            End If
                                        Case "mid"
                                            If Master.eSettings.AutoETSize = Master.FanartSize.Mid Then
                                                If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                    SaveFAasET(savePath, sPath)
                                                End If
                                            End If
                                        Case "thumb"
                                            If Master.eSettings.AutoETSize = Master.FanartSize.Small Then
                                                If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                    SaveFAasET(savePath, sPath)
                                                End If
                                            End If
                                    End Select
                                End If
                            End If
                        Next
                    End If
                End If
            Else
                'download all the fanart from TMDB
                tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "backdrop")

                If tmpListTMDB.Count > 0 Then


                    If Not Directory.Exists(CachePath) Then
                        Directory.CreateDirectory(CachePath)
                    End If

                    Dim savePath As String = String.Empty
                    For Each miFanart As Media.Image In tmpListTMDB
                        Select Case miFanart.Description.ToLower
                            Case "original"
                                If Master.eSettings.AutoETSize = Master.FanartSize.Lrg Then
                                    miFanart.WebImage.FromWeb(miFanart.URL)
                                    If Not IsNothing(miFanart.WebImage.Image) Then
                                        _image = miFanart.WebImage.Image
                                        savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                        Save(savePath)
                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                            SaveFAasET(savePath, sPath)
                                        End If
                                    End If
                                End If
                            Case "mid"
                                If Master.eSettings.AutoETSize = Master.FanartSize.Mid Then
                                    miFanart.WebImage.FromWeb(miFanart.URL)
                                    If Not IsNothing(miFanart.WebImage.Image) Then
                                        _image = miFanart.WebImage.Image
                                        savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                        Save(savePath)
                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                            SaveFAasET(savePath, sPath)
                                        End If
                                    End If
                                End If
                            Case "thumb"
                                If Master.eSettings.AutoETSize = Master.FanartSize.Small Then
                                    miFanart.WebImage.FromWeb(miFanart.URL)
                                    If Not IsNothing(miFanart.WebImage.Image) Then
                                        _image = miFanart.WebImage.Image
                                        savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringManip.CleanURL(miFanart.URL), ").jpg"))
                                        Save(savePath)
                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                            SaveFAasET(savePath, sPath)
                                        End If
                                    End If
                                End If
                        End Select
                        Me.Clear()
                    Next

                    _image = Nothing
                    FileManip.Delete.DeleteDirectory(CachePath)

                End If
            End If
        End If
    End Sub

    Public Function IsAllowedToDownload(ByVal mMovie As Master.DBMovie, ByVal fType As Master.ImageType, Optional ByVal isChange As Boolean = False) As Boolean

        Try
            Select Case fType
                Case Master.ImageType.Fanart
                    If (isChange OrElse (String.IsNullOrEmpty(mMovie.FanartPath) OrElse Master.eSettings.OverwriteFanart)) AndAlso _
                    (Master.eSettings.MovieNameDotFanartJPG OrElse Master.eSettings.MovieNameFanartJPG OrElse Master.eSettings.FanartJPG) AndAlso _
                    Master.eSettings.UseTMDB Then
                        Return True
                    Else
                        Return False
                    End If
                Case Else
                    If (isChange OrElse (String.IsNullOrEmpty(mMovie.PosterPath) OrElse Master.eSettings.OverwritePoster)) AndAlso _
                    (Master.eSettings.MovieTBN OrElse Master.eSettings.MovieNameTBN OrElse Master.eSettings.MovieJPG OrElse _
                     Master.eSettings.MovieNameJPG OrElse Master.eSettings.PosterTBN OrElse Master.eSettings.PosterJPG OrElse Master.eSettings.FolderJPG) AndAlso _
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

    Public Sub DeletePosters(ByVal mMovie As Master.DBMovie)
        Try
            Dim tPath As String = Directory.GetParent(mMovie.Filename).FullName

            If Master.eSettings.VideoTSParent AndAlso FileManip.Common.isVideoTS(mMovie.Filename) Then
                Delete(String.Concat(Path.Combine(Directory.GetParent(tPath).FullName, Directory.GetParent(tPath).Name), ".jpg"))
                Delete(String.Concat(Path.Combine(Directory.GetParent(tPath).FullName, Directory.GetParent(tPath).Name), ".tbn"))
            ElseIf Master.eSettings.VideoTSParent AndAlso FileManip.Common.isBDRip(mMovie.Filename) Then
                Delete(String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(tPath).FullName).FullName, Directory.GetParent(Directory.GetParent(tPath).FullName).Name), ".jpg"))
                Delete(String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(tPath).FullName).FullName, Directory.GetParent(Directory.GetParent(tPath).FullName).Name), ".tbn"))
            Else

                If mMovie.isSingle Then
                    Delete(Path.Combine(tPath, "movie.tbn"))
                    Delete(Path.Combine(tPath, "movie.jpg"))
                    Delete(Path.Combine(tPath, "poster.tbn"))
                    Delete(Path.Combine(tPath, "poster.jpg"))
                    Delete(Path.Combine(tPath, "folder.jpg"))
                End If

                If FileManip.Common.isVideoTS(mMovie.Filename) Then
                    Delete(Path.Combine(tPath, "video_ts.tbn"))
                    Delete(Path.Combine(tPath, "video_ts.jpg"))
                ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                    Delete(Path.Combine(tPath, "index.tbn"))
                    Delete(Path.Combine(tPath, "index.jpg"))
                Else
                    Dim pPath As String = Path.Combine(tPath, Path.GetFileNameWithoutExtension(mMovie.Filename))
                    Delete(String.Concat(pPath, ".tbn"))
                    Delete(String.Concat(pPath, ".jpg"))
                End If

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub DeleteShowPosters(ByVal mShow As Master.DBTV)
        Try
            Dim tPath As String = mShow.ShowPath

            Delete(Path.Combine(tPath, "season-all.tbn"))
            Delete(Path.Combine(tPath, "folder.jpg"))
            Delete(Path.Combine(tPath, "poster.tbn"))
            Delete(Path.Combine(tPath, "poster.jpg"))

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub DeleteEpPosters(ByVal mShow As Master.DBTV)
        Try
            Dim tPath As String = FileManip.Common.RemoveExtFromPath(mShow.Filename)

            Delete(String.Concat(tPath, ".tbn"))
            Delete(String.Concat(tPath, ".jpg"))

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub DeleteFanart(ByVal mMovie As Master.DBMovie)
        Try
            Dim tPath As String = Directory.GetParent(mMovie.Filename).FullName

            If Master.eSettings.VideoTSParent AndAlso FileManip.Common.isVideoTS(mMovie.Filename) Then
                Delete(String.Concat(Path.Combine(Directory.GetParent(tPath).FullName, Directory.GetParent(tPath).Name), ".fanart.jpg"))
            ElseIf Master.eSettings.VideoTSParent AndAlso FileManip.Common.isBDRip(mMovie.Filename) Then
                Delete(String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(tPath).FullName).FullName, Directory.GetParent(Directory.GetParent(tPath).FullName).Name), ".fanart.jpg"))
            Else
                If mMovie.isSingle Then Delete(Path.Combine(tPath, "fanart.jpg"))

                If FileManip.Common.isVideoTS(mMovie.Filename) Then
                    Delete(Path.Combine(tPath, "video_ts-fanart.jpg"))
                    Delete(Path.Combine(tPath, "video_ts.fanart.jpg"))
                ElseIf FileManip.Common.isBDRip(mMovie.Filename) Then
                    Delete(Path.Combine(tPath, "index-fanart.jpg"))
                    Delete(Path.Combine(tPath, "index.fanart.jpg"))
                Else
                    Dim fPath As String = Path.Combine(tPath, Path.GetFileNameWithoutExtension(mMovie.Filename))
                    Delete(String.Concat(fPath, "-fanart.jpg"))
                    Delete(String.Concat(fPath, ".fanart.jpg"))
                End If

            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub DeleteShowFanart(ByVal mShow As Master.DBTV)
        Try
            Delete(Path.Combine(mShow.ShowPath, "fanart.jpg"))
            Delete(Path.Combine(mShow.ShowPath, String.Concat(Path.GetFileNameWithoutExtension(mShow.ShowPath), "-fanart.jpg")))
            Delete(Path.Combine(mShow.ShowPath, String.Concat(Path.GetFileNameWithoutExtension(mShow.ShowPath), ".fanart.jpg")))
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Sub DeleteEpFanart(ByVal mShow As Master.DBTV)
        Try
            Delete(String.Concat(FileManip.Common.RemoveExtFromPath(mShow.ShowPath), "-fanart.jpg"))
            Delete(String.Concat(FileManip.Common.RemoveExtFromPath(mShow.ShowPath), ".fanart.jpg"))
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

End Class
