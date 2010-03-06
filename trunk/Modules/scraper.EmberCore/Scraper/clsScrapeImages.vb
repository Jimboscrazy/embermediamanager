Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Drawing.Imaging
Public Class ScrapeImages
    Public Shared Function GetPreferredImage(ByRef Image As Images, ByVal IMDBID As String, ByVal iType As Enums.ImageType, ByRef imgResult As Containers.ImgResult, ByVal sPath As String, ByVal doETs As Boolean, Optional ByVal doAsk As Boolean = False) As Boolean

        '//
        ' Try to get the best match between what the user selected in settings and the actual posters downloaded
        '\\

        Dim hasImages As Boolean = False
        Dim TMDB As New TMDB.Scraper
        Dim IMPA As New IMPA.Scraper
        Dim MPDB As New MPDB.Scraper
        Dim tmpListTMDB As New List(Of MediaContainers.Image)
        Dim tmpListIMPA As New List(Of MediaContainers.Image)
        Dim tmpListMPDB As New List(Of MediaContainers.Image)
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

        Dim CachePath As String = String.Concat(Master.TempPath, Path.DirectorySeparatorChar, IMDBID, Path.DirectorySeparatorChar, If(iType = Enums.ImageType.Posters, "posters", "fanart"))

        Try

            If iType = Enums.ImageType.Posters Then 'posters

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
                        Dim tImage As MediaContainers.Image
                        For Each sFile As FileInfo In lFi
                            tImage = New MediaContainers.Image
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
                            Image.Clear()
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

                        For Each tmdbThumb As MediaContainers.Image In tmpListTMDB
                            tmdbThumb.WebImage.FromWeb(tmdbThumb.URL)
                            If Not IsNothing(tmdbThumb.WebImage.Image) Then
                                If Not Master.eSettings.NoSaveImagesToNfo Then imgResult.Posters.Add(tmdbThumb.URL)
                                Image.Image = tmdbThumb.WebImage.Image
                                Image.Save(Path.Combine(CachePath, String.Concat("poster_(", tmdbThumb.Description, ")_(url=", StringUtils.CleanURL(tmdbThumb.URL), ").jpg")))
                            End If
                            Image.Clear()
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

                        For Each iMovie As MediaContainers.Image In tmpListTMDB
                            If Images.GetPosterDims(iMovie.WebImage.Image) = Master.eSettings.PreferredPosterSize Then
                                Image.Image = iMovie.WebImage.Image
                                GoTo foundit
                            End If
                        Next

                        If Not doAsk Then
                            Image.Image = tmpListTMDB.OrderBy(Function(i) i.WebImage.Image.Height + i.WebImage.Image.Height).FirstOrDefault.WebImage.Image
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
                                For Each tmdbThumb As MediaContainers.Image In tmpListTMDB
                                    imgResult.Posters.Add(tmdbThumb.URL)
                                Next
                            End If

                            For Each iMovie As MediaContainers.Image In tmpListTMDB
                                Select Case Master.eSettings.PreferredPosterSize
                                    Case Enums.PosterSize.Xlrg
                                        If iMovie.Description.ToLower = "original" Then
                                            Image.FromWeb(iMovie.URL)
                                            If Not IsNothing(Image.Image) Then GoTo foundIT
                                        End If
                                    Case Enums.PosterSize.Lrg
                                        If iMovie.Description.ToLower = "mid" Then
                                            Image.FromWeb(iMovie.URL)
                                            If Not IsNothing(Image.Image) Then GoTo foundIT
                                        End If
                                    Case Enums.PosterSize.Mid
                                        If iMovie.Description.ToLower = "cover" Then
                                            Image.FromWeb(iMovie.URL)
                                            If Not IsNothing(Image.Image) Then GoTo foundIT
                                        End If
                                    Case Enums.PosterSize.Small
                                        If iMovie.Description.ToLower = "thumb" Then
                                            Image.FromWeb(iMovie.URL)
                                            If Not IsNothing(Image.Image) Then GoTo foundIT
                                        End If
                                        'no "wide" for TMDB
                                End Select
                                Image.Clear()
                            Next
                        End If
                    End If

                    If Master.eSettings.UseIMPA Then
                        If IsNothing(Image.Image) Then
                            'no poster of the proper size from TMDB found... try IMPA

                            tmpListIMPA = IMPA.GetIMPAPosters(IMDBID)

                            If tmpListIMPA.Count > 0 Then
                                hasImages = True
                                For Each iImage As MediaContainers.Image In tmpListIMPA
                                    Image.FromWeb(iImage.URL)
                                    If Not IsNothing(Image.Image) Then
                                        If Not Master.eSettings.NoSaveImagesToNfo Then imgResult.Posters.Add(iImage.URL)
                                        Dim tmpSize As Enums.PosterSize = Images.GetPosterDims(Image.Image)
                                        If Not tmpSize = Master.eSettings.PreferredPosterSize Then
                                            'cache the first result from each type in case the preferred size is not available
                                            Select Case tmpSize
                                                Case Enums.PosterSize.Xlrg
                                                    If IsNothing(tmpIMPAX) Then
                                                        tmpIMPAX = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Lrg
                                                    If IsNothing(tmpIMPAL) Then
                                                        tmpIMPAL = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Mid
                                                    If IsNothing(tmpIMPAM) Then
                                                        tmpIMPAM = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Small
                                                    If IsNothing(tmpIMPAS) Then
                                                        tmpIMPAS = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Wide
                                                    If IsNothing(tmpIMPAW) Then
                                                        tmpIMPAW = New Bitmap(Image.Image)
                                                    End If
                                            End Select
                                        Else
                                            'image found
                                            GoTo foundIT
                                        End If
                                    End If
                                    Image.Clear()
                                Next
                            End If
                        End If
                    End If

                    If Master.eSettings.UseMPDB Then
                        If IsNothing(Image.Image) Then
                            'no poster of the proper size from TMDB or IMPA found... try MPDB

                            tmpListMPDB = MPDB.GetMPDBPosters(IMDBID)

                            If tmpListMPDB.Count > 0 Then
                                hasImages = True
                                For Each iImage As MediaContainers.Image In tmpListMPDB
                                    Image.FromWeb(iImage.URL)
                                    If Not IsNothing(Image.Image) Then
                                        If Not Master.eSettings.NoSaveImagesToNfo Then imgResult.Posters.Add(iImage.URL)
                                        Dim tmpSize As Enums.PosterSize = Images.GetPosterDims(Image.Image)
                                        If Not tmpSize = Master.eSettings.PreferredPosterSize Then
                                            'cache the first result from each type in case the preferred size is not available
                                            Select Case tmpSize
                                                Case Enums.PosterSize.Xlrg
                                                    If IsNothing(tmpMPDBX) Then
                                                        tmpMPDBX = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Lrg
                                                    If IsNothing(tmpMPDBL) Then
                                                        tmpMPDBL = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Mid
                                                    If IsNothing(tmpMPDBM) Then
                                                        tmpMPDBM = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Small
                                                    If IsNothing(tmpMPDBS) Then
                                                        tmpMPDBS = New Bitmap(Image.Image)
                                                    End If
                                                Case Enums.PosterSize.Wide
                                                    If IsNothing(tmpMPDBW) Then
                                                        tmpMPDBW = New Bitmap(Image.Image)
                                                    End If
                                            End Select
                                        Else
                                            'image found
                                            GoTo foundIT
                                        End If
                                    End If
                                    Image.Clear()
                                Next
                            End If
                        End If
                    End If

                    If IsNothing(Image.Image) AndAlso Not doAsk Then
                        'STILL no image found, just get the first available image, starting with the largest
                        If Master.eSettings.UseTMDB Then
                            'check TMDB first
                            If tmpListTMDB.Count > 0 Then
                                Dim x = From MI As MediaContainers.Image In tmpListTMDB Where MI.Description = "original"
                                If x.Count > 0 Then
                                    Image.FromWeb(x(0).URL)
                                    If Not IsNothing(Image.Image) Then GoTo foundIT
                                End If

                                Dim l = From MI As MediaContainers.Image In tmpListTMDB Where MI.Description = "mid"
                                If l.Count > 0 Then
                                    Image.FromWeb(l(0).URL)
                                    If Not IsNothing(Image.Image) Then GoTo foundIT
                                End If

                                Dim m = From MI As MediaContainers.Image In tmpListTMDB Where MI.Description = "cover"
                                If m.Count > 0 Then
                                    Image.FromWeb(m(0).URL)
                                    If Not IsNothing(Image.Image) Then GoTo foundIT
                                End If

                                Dim s = From MI As MediaContainers.Image In tmpListTMDB Where MI.Description = "thumb"
                                If s.Count > 0 Then
                                    Image.FromWeb(s(0).URL)
                                    If Not IsNothing(Image.Image) Then GoTo foundIT
                                End If

                            End If
                        End If

                        Image.Clear()

                        If Master.eSettings.UseIMPA Then
                            If tmpListIMPA.Count > 0 Then
                                If Not IsNothing(tmpIMPAX) Then
                                    Image.Image = New Bitmap(tmpIMPAX)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAL) Then
                                    Image.Image = New Bitmap(tmpIMPAL)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAM) Then
                                    Image.Image = New Bitmap(tmpIMPAM)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAS) Then
                                    Image.Image = New Bitmap(tmpIMPAS)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpIMPAW) Then
                                    Image.Image = New Bitmap(tmpIMPAW)
                                    GoTo foundIT
                                End If
                            End If
                        End If

                        Image.Clear()

                        If Master.eSettings.UseMPDB Then
                            If tmpListMPDB.Count > 0 Then
                                If Not IsNothing(tmpMPDBX) Then
                                    Image.Image = New Bitmap(tmpMPDBX)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBL) Then
                                    Image.Image = New Bitmap(tmpMPDBL)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBM) Then
                                    Image.Image = New Bitmap(tmpMPDBM)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBS) Then
                                    Image.Image = New Bitmap(tmpMPDBS)
                                    GoTo foundIT
                                End If
                                If Not IsNothing(tmpMPDBW) Then
                                    Image.Image = New Bitmap(tmpMPDBW)
                                    GoTo foundIT
                                End If
                            End If
                        End If

                        Image.Clear()

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
                            Dim tImage As MediaContainers.Image
                            For Each sFile As FileInfo In lFi
                                tImage = New MediaContainers.Image
                                tImage.WebImage.FromFile(sFile.FullName)
                                Select Case True
                                    Case sFile.Name.Contains("(original)")
                                        tImage.Description = "original"
                                        If Master.eSettings.AutoET AndAlso doETs AndAlso Master.eSettings.AutoETSize = Enums.FanartSize.Lrg Then
                                            If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                                Image.SaveFAasET(sFile.FullName, sPath)
                                            End If
                                        End If
                                    Case sFile.Name.Contains("(mid)")
                                        tImage.Description = "mid"
                                        If Master.eSettings.AutoET AndAlso doETs AndAlso Master.eSettings.AutoETSize = Enums.FanartSize.Mid Then
                                            If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                                Image.SaveFAasET(sFile.FullName, sPath)
                                            End If
                                        End If
                                    Case sFile.Name.Contains("(thumb)")
                                        tImage.Description = "thumb"
                                        If Master.eSettings.AutoET AndAlso doETs AndAlso Master.eSettings.AutoETSize = Enums.FanartSize.Small Then
                                            If Not ETHashes.Contains(HashFile.HashCalcFile(sFile.FullName)) Then
                                                Image.SaveFAasET(sFile.FullName, sPath)
                                            End If
                                        End If
                                End Select
                                tImage.URL = Regex.Match(sFile.Name, "\(url=(.*?)\)").Groups(1).ToString
                                tmpListTMDB.Add(tImage)
                                Image.Clear()
                            Next
                        Else
                            'download all the fanart from TMDB
                            tmpListTMDB = TMDB.GetTMDBImages(IMDBID, "backdrop")

                            If tmpListTMDB.Count > 0 Then

                                'setup fanart for nfo
                                Dim thumbLink As String = String.Empty
                                imgResult.Fanart.URL = "http://images.themoviedb.org"
                                For Each miFanart As MediaContainers.Image In tmpListTMDB
                                    miFanart.WebImage.FromWeb(miFanart.URL)
                                    If Not IsNothing(miFanart.WebImage.Image) Then
                                        Image.Image = miFanart.WebImage.Image
                                        Dim savePath As String = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringUtils.CleanURL(miFanart.URL), ").jpg"))
                                        Image.Save(savePath)
                                        If Master.eSettings.AutoET AndAlso doETs Then
                                            Select Case miFanart.Description.ToLower
                                                Case "original"
                                                    If Master.eSettings.AutoETSize = Enums.FanartSize.Lrg Then
                                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                            Image.SaveFAasET(savePath, sPath)
                                                        End If
                                                    End If
                                                Case "mid"
                                                    If Master.eSettings.AutoETSize = Enums.FanartSize.Mid Then
                                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                            Image.SaveFAasET(savePath, sPath)
                                                        End If
                                                    End If
                                                Case "thumb"
                                                    If Master.eSettings.AutoETSize = Enums.FanartSize.Small Then
                                                        If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                            Image.SaveFAasET(savePath, sPath)
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
                                                imgResult.Fanart.Thumb.Add(New MediaContainers.Thumb With {.Preview = thumbLink, .Text = miFanart.URL.Replace("http://images.themoviedb.org", String.Empty)})
                                            End If
                                        End If
                                    End If
                                    Image.Clear()
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

                            For Each iMovie As MediaContainers.Image In tmpListTMDB
                                If Images.GetFanartDims(iMovie.WebImage.Image) = Master.eSettings.PreferredFanartSize Then
                                    Image.Image = iMovie.WebImage.Image
                                    GoTo foundit
                                End If
                            Next

                            Image.Clear()

                            If Not doAsk Then
                                Image.Image = tmpListTMDB.OrderBy(Function(i) i.WebImage.Image.Height + i.WebImage.Image.Height).FirstOrDefault.WebImage.Image
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
                                For Each miFanart As MediaContainers.Image In tmpListTMDB
                                    If Not miFanart.URL.Contains("_thumb.") Then
                                        thumbLink = miFanart.URL.Replace("http://images.themoviedb.org", String.Empty)
                                        If thumbLink.Contains("_poster.") Then
                                            thumbLink = thumbLink.Replace("_poster.", "_thumb.")
                                        Else
                                            thumbLink = thumbLink.Insert(thumbLink.LastIndexOf("."), "_thumb")
                                        End If
                                        imgResult.Fanart.Thumb.Add(New MediaContainers.Thumb With {.Preview = thumbLink, .Text = miFanart.URL.Replace("http://images.themoviedb.org", String.Empty)})
                                    End If
                                Next
                            End If


                            If Master.eSettings.AutoET AndAlso doETs Then

                                If Not Directory.Exists(CachePath) Then
                                    Directory.CreateDirectory(CachePath)
                                End If

                                Dim savePath As String = String.Empty
                                For Each miFanart As MediaContainers.Image In tmpListTMDB
                                    Select Case miFanart.Description.ToLower
                                        Case "original"
                                            If Master.eSettings.AutoETSize = Enums.FanartSize.Lrg Then
                                                miFanart.WebImage.FromWeb(miFanart.URL)
                                                If Not IsNothing(miFanart.WebImage.Image) Then
                                                    Image.Image = miFanart.WebImage.Image
                                                    savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringUtils.CleanURL(miFanart.URL), ").jpg"))
                                                    Image.Save(savePath)
                                                    If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                        Image.SaveFAasET(savePath, sPath)
                                                    End If
                                                End If
                                            End If
                                        Case "mid"
                                            If Master.eSettings.AutoETSize = Enums.FanartSize.Mid Then
                                                miFanart.WebImage.FromWeb(miFanart.URL)
                                                If Not IsNothing(miFanart.WebImage.Image) Then
                                                    Image.Image = miFanart.WebImage.Image
                                                    savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringUtils.CleanURL(miFanart.URL), ").jpg"))
                                                    Image.Save(savePath)
                                                    If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                        Image.SaveFAasET(savePath, sPath)
                                                    End If
                                                End If
                                            End If
                                        Case "thumb"
                                            If Master.eSettings.AutoETSize = Enums.FanartSize.Small Then
                                                miFanart.WebImage.FromWeb(miFanart.URL)
                                                If Not IsNothing(miFanart.WebImage.Image) Then
                                                    Image.Image = miFanart.WebImage.Image
                                                    savePath = Path.Combine(CachePath, String.Concat("fanart_(", miFanart.Description, ")_(url=", StringUtils.CleanURL(miFanart.URL), ").jpg"))
                                                    Image.Save(savePath)
                                                    If Not ETHashes.Contains(HashFile.HashCalcFile(savePath)) Then
                                                        Image.SaveFAasET(savePath, sPath)
                                                    End If
                                                End If
                                            End If
                                    End Select
                                Next

                                Image.Clear()
                                FileUtils.Delete.DeleteDirectory(CachePath)
                            End If

                            For Each iMovie As MediaContainers.Image In tmpListTMDB
                                Select Case Master.eSettings.PreferredFanartSize
                                    Case Enums.FanartSize.Lrg
                                        If iMovie.Description.ToLower = "original" Then
                                            If Not IsNothing(iMovie.WebImage.Image) Then
                                                Image.Image = iMovie.WebImage.Image
                                            Else
                                                Image.FromWeb(iMovie.URL)
                                            End If
                                            GoTo foundIT
                                        End If
                                    Case Enums.FanartSize.Mid
                                        If iMovie.Description.ToLower = "mid" Then
                                            If Not IsNothing(iMovie.WebImage.Image) Then
                                                Image.Image = iMovie.WebImage.Image
                                            Else
                                                Image.FromWeb(iMovie.URL)
                                            End If
                                            GoTo foundIT
                                        End If
                                    Case Enums.FanartSize.Small
                                        If iMovie.Description.ToLower = "thumb" Then
                                            If Not IsNothing(iMovie.WebImage.Image) Then
                                                Image.Image = iMovie.WebImage.Image
                                            Else
                                                Image.FromWeb(iMovie.URL)
                                            End If
                                            GoTo foundIT
                                        End If
                                End Select
                            Next

                            Image.Clear()

                            If IsNothing(Image.Image) AndAlso Not doAsk Then

                                'STILL no image found, just get the first available image, starting with the largest

                                Dim l = From MI As MediaContainers.Image In tmpListTMDB Where MI.Description = "original"
                                If l.Count > 0 Then
                                    Image.FromWeb(l(0).URL)
                                    GoTo foundIT
                                End If

                                Dim m = From MI As MediaContainers.Image In tmpListTMDB Where MI.Description = "mid"
                                If m.Count > 0 Then
                                    Image.FromWeb(m(0).URL)
                                    GoTo foundIT
                                End If

                                Dim s = From MI As MediaContainers.Image In tmpListTMDB Where MI.Description = "thumb"
                                If s.Count > 0 Then
                                    Image.FromWeb(s(0).URL)
                                    GoTo foundIT
                                End If

                            End If

                            Image.Clear()

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
End Class
