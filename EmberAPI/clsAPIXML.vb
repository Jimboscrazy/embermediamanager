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
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Serialization

Public Class APIXML

    #Region "Fields"

    Public Shared alFlags As New List(Of String)
    Public Shared alGenres As New List(Of String)
    Public Shared dStudios As New Dictionary(Of String, String)
    Public Shared FlagsXML As New XDocument
    Public Shared GenreXML As New XDocument
    Public Shared RatingXML As New XDocument

    #End Region 'Fields

    #Region "Methods"

    Public Shared Sub CacheXMLs()
        Try
            Dim fPath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Flags", Path.DirectorySeparatorChar, "Flags.xml")
            If File.Exists(fPath) Then
                FlagsXML = XDocument.Load(fPath)
            End If

            If Directory.Exists(Directory.GetParent(fPath).FullName) Then
                Try
                    alFlags.AddRange(Directory.GetFiles(Directory.GetParent(fPath).FullName, "*.png"))
                Catch
                End Try
                alFlags = alFlags.ConvertAll(Function(s) s.ToLower)
            End If

            Dim gPath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Genres", Path.DirectorySeparatorChar, "Genres.xml")
            If File.Exists(gPath) Then
                GenreXML = XDocument.Load(gPath)
            End If

            If Directory.Exists(Directory.GetParent(gPath).FullName) Then
                Try
                    alGenres.AddRange(Directory.GetFiles(Directory.GetParent(gPath).FullName, "*.jpg"))
                Catch
                End Try
                alGenres = alGenres.ConvertAll(Function(s) s.ToLower)
            End If

            Dim sPath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Studios", Path.DirectorySeparatorChar, "Studios.xml")

            If Directory.Exists(Directory.GetParent(sPath).FullName) Then
                Try
                    'get all images in the main folder
                    For Each lFile As String In Directory.GetFiles(Directory.GetParent(sPath).FullName, "*.png")
                        dStudios.Add(Path.GetFileNameWithoutExtension(lFile).ToLower, lFile)
                    Next

                    'now get all images in sub folders
                    For Each iDir As String In Directory.GetDirectories(Directory.GetParent(sPath).FullName)
                        For Each lFile As String In Directory.GetFiles(iDir, "*.png")
                            'hard code "\" here, then resplace when retrieving images
                            dStudios.Add(String.Concat(Directory.GetParent(iDir).Name, "\", Path.GetFileNameWithoutExtension(lFile).ToLower), lFile)
                        Next
                    Next
                Catch
                End Try
            End If

            Dim rPath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Ratings", Path.DirectorySeparatorChar, "Ratings.xml")
            If File.Exists(rPath) Then
                RatingXML = XDocument.Load(rPath)
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function GetAVImages(ByVal fiAV As MediaInfo.Fileinfo, ByVal fName As String, ByVal ForTV As Boolean) As Image()

        Dim iReturn(5) As Image
        Dim tVideo As MediaInfo.Video = NFO.GetBestVideo(fiAV)
        Dim tAudio As MediaInfo.Audio = NFO.GetBestAudio(fiAV, ForTV)

        If FlagsXML.Nodes.Count > 0 Then
            Dim mePath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Flags")
            Try
                Dim atypeRef As String = String.Empty
                Dim vresImage As String = String.Empty
                Dim vsourceImage As String = String.Empty
                Dim vtypeImage As String = String.Empty
                Dim atypeImage As String = String.Empty
                Dim achanImage As String = String.Empty
                Dim sourceCheck As String = String.Empty

                If FileUtils.Common.isVideoTS(fName) Then
                    sourceCheck = "dvd"
                ElseIf FileUtils.Common.isBDRip(fName) Then
                    sourceCheck = "bluray"
                Else
                    sourceCheck = If(Master.eSettings.SourceFromFolder, String.Concat(Directory.GetParent(fName).Name.ToLower, Path.DirectorySeparatorChar, Path.GetFileName(fName).ToLower), Path.GetFileName(fName).ToLower)
                End If

                'video resolution
                Dim xVResDefault = From xDef In FlagsXML...<vres> Select xDef.Element("default").Element("icon").Value
                If xVResDefault.Count > 0 Then
                    vresImage = Path.Combine(mePath, xVResDefault(0).ToString)
                End If

                Dim strRes As String = NFO.GetResFromDimensions(tVideo).ToLower
                If Not String.IsNullOrEmpty(strRes) Then
                    Dim xVResFlag = From xVRes In FlagsXML...<vres>...<name> Where Regex.IsMatch(strRes, xVRes.@searchstring) Select xVRes.<icon>.Value
                    If xVResFlag.Count > 0 Then
                        vresImage = Path.Combine(mePath, xVResFlag(0).ToString)
                    End If
                End If

                'video source
                Dim xVSourceDefault = From xDef In FlagsXML...<vsource> Select xDef.Element("default").Element("icon").Value
                If xVSourceDefault.Count > 0 Then
                    vsourceImage = Path.Combine(mePath, xVSourceDefault(0).ToString)
                End If

                Dim xVSourceFlag = From xVSource In FlagsXML...<vsource>...<name> Where Regex.IsMatch(sourceCheck, xVSource.@searchstring) Select xVSource.<icon>.Value
                If xVSourceFlag.Count > 0 Then
                    vsourceImage = Path.Combine(mePath, xVSourceFlag(0).ToString)
                End If

                'video type
                Dim xVTypeDefault = From xDef In FlagsXML...<vtype> Select xDef.Element("default").Element("icon").Value
                If xVTypeDefault.Count > 0 Then
                    vtypeImage = Path.Combine(mePath, xVTypeDefault(0).ToString)
                End If

                Dim vCodec As String = tVideo.Codec.ToLower
                If Not String.IsNullOrEmpty(vCodec) Then
                    Dim xVTypeFlag = From xVType In FlagsXML...<vtype>...<name> Where Regex.IsMatch(vCodec, xVType.@searchstring) Select xVType.<icon>.Value
                    If xVTypeFlag.Count > 0 Then
                        vtypeImage = Path.Combine(mePath, xVTypeFlag(0).ToString)
                    End If
                End If

                'audio type
                Dim xATypeDefault = From xDef In FlagsXML...<atype> Select xDef.Element("default").Element("icon").Value
                If xATypeDefault.Count > 0 Then
                    atypeImage = Path.Combine(mePath, xATypeDefault(0).ToString)
                End If

                Dim aCodec As String = tAudio.Codec.ToLower
                If Not String.IsNullOrEmpty(aCodec) Then
                    Dim xATypeFlag = From xAType In FlagsXML...<atype>...<name> Where Regex.IsMatch(aCodec, xAType.@searchstring) Select xAType.<icon>.Value, xAType.<ref>.Value
                    If xATypeFlag.Count > 0 Then
                        atypeImage = Path.Combine(mePath, xATypeFlag(0).icon.ToString)
                        If Not IsNothing(xATypeFlag(0).ref) Then
                            atypeRef = xATypeFlag(0).ref.ToString
                        End If
                    End If
                End If

                'audio channels
                Dim xAChanDefault = From xDef In FlagsXML...<achan> Select xDef.Element("default").Element("icon").Value
                If xAChanDefault.Count > 0 Then
                    achanImage = Path.Combine(mePath, xAChanDefault(0).ToString)
                End If

                If Not String.IsNullOrEmpty(tAudio.Channels) Then
                    Dim xAChanFlag = From xAChan In FlagsXML...<achan>...<name> Where Regex.IsMatch(tAudio.Channels, Regex.Replace(xAChan.@searchstring, "(\{[^\}]+\})", String.Empty)) AndAlso Regex.IsMatch(atypeRef, Regex.Match(xAChan.@searchstring, "\{atype=([^\}]+)\}").Groups(1).Value.ToString) Select xAChan.<icon>.Value
                    If xAChanFlag.Count > 0 Then
                        achanImage = Path.Combine(mePath, xAChanFlag(0).ToString)
                    End If
                End If

                If Not String.IsNullOrEmpty(vresImage) AndAlso alFlags.Contains(vresImage.ToLower) Then
                    Using fsImage As New FileStream(vresImage, FileMode.Open, FileAccess.Read)
                        iReturn(0) = Image.FromStream(fsImage)
                    End Using
                Else
                    iReturn(0) = My.Resources.defaultscreen
                End If

                If Not String.IsNullOrEmpty(vsourceImage) AndAlso alFlags.Contains(vsourceImage.ToLower) Then
                    Using fsImage As New FileStream(vsourceImage, FileMode.Open, FileAccess.Read)
                        iReturn(1) = Image.FromStream(fsImage)
                    End Using
                Else
                    iReturn(1) = My.Resources.defaultscreen
                End If

                If Not String.IsNullOrEmpty(vtypeImage) AndAlso alFlags.Contains(vtypeImage.ToLower) Then
                    Using fsImage As New FileStream(vtypeImage, FileMode.Open, FileAccess.Read)
                        iReturn(2) = Image.FromStream(fsImage)
                    End Using
                Else
                    iReturn(2) = My.Resources.defaultscreen
                End If

                If Not String.IsNullOrEmpty(atypeImage) AndAlso alFlags.Contains(atypeImage.ToLower) Then
                    Using fsImage As New FileStream(atypeImage, FileMode.Open, FileAccess.Read)
                        If tAudio.HasPreferred Then
                            iReturn(3) = ImageUtils.SetOverlay(Image.FromStream(fsImage), 64, 44, My.Resources.haslanguage, 4)
                        Else
                            iReturn(3) = Image.FromStream(fsImage)
                        End If
                    End Using
                Else
                    If tAudio.HasPreferred Then
                        iReturn(3) = ImageUtils.SetOverlay(My.Resources.defaultsound, 64, 44, My.Resources.haslanguage, 4)
                    Else
                        iReturn(3) = My.Resources.defaultsound
                    End If
                End If

                If Not String.IsNullOrEmpty(achanImage) AndAlso alFlags.Contains(achanImage.ToLower) Then
                    Using fsImage As New FileStream(achanImage, FileMode.Open, FileAccess.Read)
                        iReturn(4) = Image.FromStream(fsImage)
                    End Using
                Else
                    iReturn(4) = My.Resources.defaultsound
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            iReturn(0) = My.Resources.defaultscreen
            iReturn(1) = My.Resources.defaultscreen
            iReturn(2) = My.Resources.defaultscreen
            If tAudio.HasPreferred Then
                iReturn(3) = ImageUtils.SetOverlay(My.Resources.defaultsound, 64, 44, My.Resources.haslanguage, 4)
            Else
                iReturn(3) = My.Resources.defaultsound
            End If
            iReturn(4) = My.Resources.defaultsound
        End If

        Return iReturn
    End Function

    Public Shared Function GetFileSource(ByVal sPath As String) As String
        Dim sourceCheck As String = String.Empty

        Try
            If FileUtils.Common.isVideoTS(sPath) Then
                sourceCheck = "dvd"
            ElseIf FileUtils.Common.isBDRip(sPath) Then
                sourceCheck = "bluray"
            Else
                sourceCheck = If(Master.eSettings.SourceFromFolder, String.Concat(Directory.GetParent(sPath).Name.ToLower, Path.DirectorySeparatorChar, Path.GetFileName(sPath).ToLower), Path.GetFileName(sPath).ToLower)
            End If

            Dim xVSourceFlag = From xVSource In FlagsXML...<vsource>...<name> Where Regex.IsMatch(sourceCheck, xVSource.@searchstring) Select xVSource.@searchstring
            If xVSourceFlag.Count > 0 Then
                Return xVSourceFlag(0).ToString
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return String.Empty
    End Function

    Public Shared Function GetGenreImage(ByVal strGenre As String) As Image
        '//
        ' Set the proper images based on the genre string
        '\\

        Dim imgGenre As Image = Nothing
        Dim imgGenreStr As String = String.Empty

        If GenreXML.Nodes.Count > 0 Then

            Dim mePath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Genres")

            Try

                Dim xDefault = From xDef In GenreXML...<default> Select xDef.<icon>.Value
                If xDefault.Count > 0 Then
                    imgGenreStr = Path.Combine(mePath, xDefault(0).ToString)
                End If

                Dim xGenre = From xGen In GenreXML...<name> Where strGenre.ToLower = xGen.@searchstring.ToLower Select xGen.<icon>.Value
                If xGenre.Count > 0 Then
                    imgGenreStr = Path.Combine(mePath, xGenre(0).ToString)
                End If

                If Not String.IsNullOrEmpty(imgGenreStr) AndAlso alGenres.Contains(imgGenreStr.ToLower) Then
                    Using fsImage As New FileStream(imgGenreStr, FileMode.Open, FileAccess.Read)
                        imgGenre = Image.FromStream(fsImage)
                    End Using
                Else
                    imgGenre = My.Resources.defaultgenre
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            imgGenre = My.Resources.defaultgenre
        End If

        Return imgGenre
    End Function

    Public Shared Function GetGenreList(Optional ByVal LangsOnly As Boolean = False) As Object()
        Dim retGenre As New List(Of String)
        Try
            If LangsOnly Then
                Dim xGenre = From xGen In GenreXML...<supported>.Descendants Select xGen.Value
                If xGenre.Count > 0 Then
                    retGenre.AddRange(xGenre.ToArray)
                End If
            Else
                Dim splitLang() As String
                Dim xGenre = From xGen In GenreXML...<name> Select xGen.@searchstring, xGen.@language
                If xGenre.Count > 0 Then
                    For i As Integer = 0 To xGenre.Count - 1
                        splitLang = xGenre(i).language.Split(Convert.ToChar("|"))
                        For Each strGen As String In splitLang
                            If Not retGenre.Contains(xGenre(i).searchstring) AndAlso (Master.eSettings.GenreFilter.Contains(String.Format("{0}", Master.eLang.GetString(569, Master.eLang.All))) OrElse Master.eSettings.GenreFilter.Split(Convert.ToChar(",")).Contains(strGen)) Then
                                retGenre.Add(xGenre(i).searchstring)
                            End If
                        Next
                    Next
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return retGenre.ToArray
    End Function

    Public Shared Function GetRatingImage(ByVal strRating As String) As Image
        '//
        ' Parse the floating Rating box
        '\\

        Dim imgRating As Image = Nothing
        Dim imgRatingStr As String = String.Empty

        If RatingXML.Nodes.Count > 0 Then
            Dim mePath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Ratings")

            Try

                If Master.eSettings.UseCertForMPAA AndAlso Not Master.eSettings.CertificationLang = "USA" AndAlso RatingXML.Element("ratings").Element(Master.eSettings.CertificationLang.ToLower)...<movie>.Descendants.Count > 0 Then
                    Dim xRating = From xRat In RatingXML.Element("ratings").Element(Master.eSettings.CertificationLang.ToLower)...<movie>...<name> Where strRating.ToLower = xRat.@searchstring.ToLower OrElse strRating.ToLower = xRat.@searchstring.ToLower.Split(Convert.ToChar(":"))(1) Select xRat.<icon>.Value
                    If xRating.Count > 0 Then
                        imgRatingStr = Path.Combine(mePath, xRating(xRating.Count - 1).ToString)
                    End If
                Else
                    Dim xRating = From xRat In RatingXML...<usa>...<movie>...<name> Where strRating.ToLower.StartsWith(xRat.@searchstring.ToLower) Select xRat.<icon>.Value
                    If xRating.Count > 0 Then
                        imgRatingStr = Path.Combine(mePath, xRating(xRating.Count - 1).ToString)
                    End If
                End If

                If Not String.IsNullOrEmpty(imgRatingStr) AndAlso File.Exists(imgRatingStr) Then
                    Using fsImage As New FileStream(imgRatingStr, FileMode.Open, FileAccess.Read)
                        imgRating = Image.FromStream(fsImage)
                    End Using
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If

        Return imgRating
    End Function

    Public Shared Function GetRatingList() As Object()
        Dim retRatings As New List(Of String)
        Try
            If Master.eSettings.UseCertForMPAA AndAlso Not Master.eSettings.CertificationLang = "USA" AndAlso RatingXML.Element("ratings").Element(Master.eSettings.CertificationLang.ToLower).Descendants("movie").Count > 0 Then
                Dim xRating = From xRat In RatingXML.Element("ratings").Element(Master.eSettings.CertificationLang.ToLower)...<movie>...<name> Select xRat.@searchstring
                If xRating.Count > 0 Then
                    retRatings.AddRange(xRating.ToArray)
                End If
            Else
                Dim xRating = From xRat In RatingXML...<usa>...<movie>...<name> Select xRat.@searchstring
                If xRating.Count > 0 Then
                    retRatings.AddRange(xRating.ToArray)
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return retRatings.ToArray
    End Function

    Public Shared Function GetRatingRegions() As Object()
        Dim retRatings As New List(Of String)
        Try
            Dim xRating = From xRat In RatingXML...<ratings>.Elements.Descendants("tv") Select (xRat.Parent.Name.ToString)
            If xRating.Count > 0 Then
                retRatings.AddRange(xRating.ToArray)
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return retRatings.ToArray
    End Function

    Public Shared Function GetSourceList() As Object()
        Dim retSources As New List(Of String)
        Try

            Dim xSource = From xSrc In FlagsXML...<vsource>...<name> Select xSrc.@searchstring.ToString.Replace("|", " | ")
            If xSource.Count > 0 Then
                retSources.AddRange(xSource.ToArray)
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return retSources.ToArray
    End Function

    Public Shared Function GetStudioImage(ByVal strStudio As String) As Image
        Dim imgStudio As Image = Nothing

        If dStudios.ContainsKey(strStudio.ToLower) Then
            Using fsImage As New FileStream(dStudios.Item(strStudio.ToLower).Replace(Convert.ToChar("\"), Path.DirectorySeparatorChar), FileMode.Open, FileAccess.Read)
                imgStudio = Image.FromStream(fsImage)
            End Using
        End If

        If IsNothing(imgStudio) Then imgStudio = My.Resources.defaultscreen

        Return imgStudio
    End Function

    Public Shared Function GetTVRatingImage(ByVal strRating As String) As Image
        Dim imgRating As Image = Nothing
        Dim imgRatingStr As String = String.Empty

        If RatingXML.Nodes.Count > 0 Then
            Dim mePath As String = String.Concat(Functions.AppPath, "Images", Path.DirectorySeparatorChar, "Ratings")

            Try

                If Master.eSettings.ShowRatingRegion = "usa" AndAlso RatingXML.Element("ratings").Element(Master.eSettings.ShowRatingRegion.ToLower)...<tv>.Descendants.Count > 0 Then
                    Dim xRating = From xRat In RatingXML.Element("ratings").Element(Master.eSettings.ShowRatingRegion.ToLower)...<tv>...<name> Where strRating.ToLower = xRat.@searchstring.ToLower Select xRat.<icon>.Value
                    If xRating.Count > 0 Then
                        imgRatingStr = Path.Combine(mePath, xRating(xRating.Count - 1).ToString)
                    End If
                Else
                    Dim xRating = From xRat In RatingXML...<usa>...<tv>...<name> Where strRating.ToLower.StartsWith(xRat.@searchstring.ToLower) Select xRat.<icon>.Value
                    If xRating.Count > 0 Then
                        imgRatingStr = Path.Combine(mePath, xRating(xRating.Count - 1).ToString)
                    End If
                End If

                If Not String.IsNullOrEmpty(imgRatingStr) AndAlso File.Exists(imgRatingStr) Then
                    Using fsImage As New FileStream(imgRatingStr, FileMode.Open, FileAccess.Read)
                        imgRating = Image.FromStream(fsImage)
                    End Using
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End If

        Return imgRating
    End Function

    Public Shared Function GetTVRatingList() As Object()
        Dim retRatings As New List(Of String)
        Try
            If Not Master.eSettings.ShowRatingRegion = "USA" AndAlso RatingXML.Element("ratings").Element(Master.eSettings.ShowRatingRegion.ToLower).Descendants("tv").Count > 0 Then
                Dim xRating = From xRat In RatingXML.Element("ratings").Element(Master.eSettings.ShowRatingRegion.ToLower)...<tv>...<name> Select xRat.@searchstring
                If xRating.Count > 0 Then
                    retRatings.AddRange(xRating.ToArray)
                End If
            Else
                Dim xRating = From xRat In RatingXML...<usa>...<tv>...<name> Select xRat.@searchstring
                If xRating.Count > 0 Then
                    retRatings.AddRange(xRating.ToArray)
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return retRatings.ToArray
    End Function

    Public Shared Function XMLToLowerCase(ByVal sXML As String) As String
        Dim sMatches As MatchCollection = Regex.Matches(sXML, "\<(.*?)\>", RegexOptions.IgnoreCase)
        For Each sMatch As Match In sMatches
            sXML = sXML.Replace(sMatch.Groups(1).Value, sMatch.Groups(1).Value.ToLower)
        Next
        Return sXML
    End Function

    #End Region 'Methods

End Class