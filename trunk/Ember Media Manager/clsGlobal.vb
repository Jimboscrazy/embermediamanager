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

Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Security.Cryptography
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Net

Public Class Master

    'Global Variables
    Public Shared uSettings As New emmSettings
    Public Shared currMovie As New Media.Movie
    Public Shared tmpMovie As New Media.Movie
    Public Shared currNFO As String = String.Empty
    Public Shared currPath As String = String.Empty
    Public Shared currMark As Boolean = False
    Public Shared alFolderList As New ArrayList
    Public Shared alFileList As New ArrayList
    Public Shared eLog As New ErrorLogger
    Public Shared isFile As Boolean = False

    'Global Enums
    Public Enum PosterSize As Integer
        Xlrg = 0
        Lrg = 1
        Mid = 2
        Small = 3
        Wide = 4
    End Enum

    Public Enum FanartSize As Integer
        Lrg = 0
        Mid = 1
        Small = 3
    End Enum

    Public Enum ScrapeType As Integer
        SingleScrape = 0
        FullAuto = 1
        FullAsk = 2
        UpdateAuto = 3
        UpdateAsk = 4
        MIOnly = 5
        CleanFolders = 6
    End Enum

    Public Enum ImageType As Integer
        Posters = 0
        Fanart = 1
    End Enum

    Public Shared Sub ResizePB(ByRef pbResize As PictureBox, ByRef pbCache As PictureBox, ByVal maxHeight As Integer, ByVal maxWidth As Integer)

        '//
        ' Resize the picture box based on the dimensions of the image and the dimensions
        ' of the available space... try to use the most screen real estate
        '
        ' Why not use "Zoom" for fanart background? - To keep the image at the top. Zoom centers vertically.
        '\\

        Try
            If Not IsNothing(pbCache.Image) Then
                pbResize.SizeMode = PictureBoxSizeMode.Normal
                Dim sPropPerc As Single = 1.0 'no default scaling

                pbResize.Size = New Size(maxWidth, maxHeight)

                ' Height
                If pbCache.Image.Height > pbResize.Height Then
                    ' Reduce height first
                    sPropPerc = CSng(pbResize.Height / pbCache.Image.Height)
                End If

                ' Width
                If (pbCache.Image.Width * sPropPerc) > pbResize.Width Then
                    ' Scaled width exceeds Box's width, recalculate scale_factor
                    sPropPerc = CSng(pbResize.Width / pbCache.Image.Width)
                End If

                ' Get the source bitmap.
                Dim bmSource As New Bitmap(pbCache.Image)
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
                pbResize.Image = bmDest

                'tweak pb after resizing pic
                pbResize.Width = pbResize.Image.Width
                pbResize.Height = pbResize.Image.Height
                'center it

                'Clean up
                bmSource = Nothing
                bmDest = Nothing
                grDest = Nothing
            Else
                pbResize.Left = 0
                pbResize.Size = New Size(maxWidth, maxHeight)
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function GetThumbnail(ByVal theImage As Image, ByVal maxHeight As Integer, ByVal maxWidth As Integer) As Image

        '//
        ' Resize the picture box based on the dimensions of the image and the dimensions
        ' of the available space... try to use the most screen real estate
        '
        ' Why not use "Zoom" for fanart background? - To keep the image at the top. Zoom centers vertically.
        '\\

        Dim imgOut As Image = Nothing

        Try
            If Not IsNothing(theImage) Then
                Dim sPropPerc As Single = 1.0 'no default scaling

                If theImage.Width > theImage.Height Then
                    sPropPerc = CSng(maxWidth / theImage.Width)
                Else
                    sPropPerc = CSng(maxHeight / theImage.Height)
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
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return imgout
    End Function

    Public Shared Sub SetOverlay(ByRef pbOverlay As PictureBox)

        '//
        ' Put our crappy glossy overlay over the poster
        '\\

        Try
            Dim bmOverlay As New Bitmap(pbOverlay.Image)
            Dim grOverlay As Graphics = Graphics.FromImage(bmOverlay)
            Dim bmHeight As Integer = pbOverlay.Image.Height * 0.65

            grOverlay.DrawImage(My.Resources.overlay, 0, 0, pbOverlay.Image.Width, bmHeight)
            pbOverlay.Image = bmOverlay

            bmOverlay = New Bitmap(pbOverlay.Image)
            grOverlay = Graphics.FromImage(bmOverlay)

            grOverlay.DrawImage(My.Resources.overlay2, 0, 0, pbOverlay.Image.Width, pbOverlay.Image.Height)
            pbOverlay.Image = bmOverlay

            grOverlay.Dispose()
            bmOverlay = Nothing
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function FilterName(ByRef movieName As String) As String

        '//
        ' Clean all the crap out of the name
        '\\

        Dim strSplit() As String
        Try

            'run through each of the custom filters
            If uSettings.FilterCustom.Count > 0 Then
                For Each Str As String In uSettings.FilterCustom

                    If Strings.InStr(Str, "[->]") > 0 Then
                        strSplit = Strings.Split(Str, "[->]")
                        movieName = Strings.Replace(movieName, Regex.Match(movieName, strSplit(0)).ToString, strSplit(1))
                    Else
                        movieName = Strings.Replace(movieName, Regex.Match(movieName, Str).ToString, String.Empty)
                    End If
                Next
            End If

            'Convert String To Proper Case
            If uSettings.ProperCase Then
                movieName = Strings.StrConv(movieName, VbStrConv.ProperCase)
            End If

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return CleanStackingMarkers(movieName.Trim)

    End Function

    Public Shared Function CleanStackingMarkers(ByVal sPath As String)

        '//
        ' Removes the stacking indicators from the file name
        '\\

        Dim newPath As String = String.Empty
        Dim filename As String = String.Empty
        Dim strTemp As String = String.Empty

        newPath = sPath

        strTemp = Regex.Match(sPath, "(?i)[ _\.-]+cd[ _\.-]*([0-9a-d]+)").ToString
        If Not String.IsNullOrEmpty(strTemp) Then
            newPath = sPath.Replace(strTemp, String.Empty)
            GoTo quickExit
        End If

        strTemp = Regex.Match(sPath, "(?i)[ _\.-]+dvd[ _\.-]*([0-9a-d]+)").ToString
        If Not String.IsNullOrEmpty(strTemp) Then
            newPath = sPath.Replace(strTemp, String.Empty)
            GoTo quickExit
        End If

        strTemp = Regex.Match(sPath, "(?i)[ _\.-]+part[ _\.-]*([0-9a-d]+)").ToString
        If Not String.IsNullOrEmpty(strTemp) Then
            newPath = sPath.Replace(strTemp, String.Empty)
            GoTo quickExit
        End If

quickExit:
        Return newPath.Trim
    End Function

    Public Shared Sub EnumerateDirectory(ByVal sPath As String)

        '//
        ' Get all directories in the parent directory
        '\\

        Try
            If Directory.Exists(sPath) Then
                Dim Dirs As String() = Directory.GetDirectories(sPath)

                For Each inDir As String In Dirs
                    If isValidDir(inDir) Then
                        alFolderList.Add(inDir)
                    End If
                Next
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Sub EnumerateFiles(ByVal sPath As String)

        '//
        ' Get all files in the parent directory
        '\\

        Try

            If Directory.Exists(sPath) Then
                Dim di As New DirectoryInfo(sPath)
                Dim lFi As New List(Of FileInfo)

                lFi.AddRange(di.GetFiles())

                'only process proper file types
                lFi = lFi.FindAll(Function(f As FileInfo) f.Extension.ToLower() = ".avi" _
                                      OrElse f.Extension.ToLower() = ".divx" _
                                      OrElse f.Extension.ToLower() = ".mkv" _
                                      OrElse f.Extension.ToLower() = ".iso" _
                                      OrElse f.Extension.ToLower() = ".mpg" _
                                      OrElse f.Extension.ToLower() = ".mp4" _
                                      OrElse f.Extension.ToLower() = ".wmv" _
                                      OrElse f.Extension.ToLower() = ".wma" _
                                      OrElse f.Extension.ToLower() = ".mov" _
                                      OrElse f.Extension.ToLower() = ".mts" _
                                      OrElse f.Extension.ToLower() = ".m2t" _
                                      OrElse f.Extension.ToLower() = ".img" _
                                      OrElse f.Extension.ToLower() = ".dat" _
                                      OrElse f.Extension.ToLower() = ".bin" _
                                      OrElse f.Extension.ToLower() = ".cue" _
                                      OrElse f.Extension.ToLower() = ".vob" _
                                      OrElse f.Extension.ToLower() = ".dvb" _
                                      OrElse f.Extension.ToLower() = ".evo" _
                                      OrElse f.Extension.ToLower() = ".asf" _
                                      OrElse f.Extension.ToLower() = ".asx" _
                                      OrElse f.Extension.ToLower() = ".avs" _
                                      OrElse f.Extension.ToLower() = ".nsv" _
                                      OrElse f.Extension.ToLower() = ".ram" _
                                      OrElse f.Extension.ToLower() = ".ogg" _
                                      OrElse f.Extension.ToLower() = ".ogm" _
                                      OrElse f.Extension.ToLower() = ".ogv" _
                                      OrElse f.Extension.ToLower() = ".flv" _
                                      OrElse f.Extension.ToLower() = ".swf" _
                                      OrElse f.Extension.ToLower() = ".nut" _
                                      OrElse f.Extension.ToLower() = ".viv" _
                                      OrElse f.Extension.ToLower() = ".rar" _
                                      OrElse f.Extension.ToLower() = ".m2ts" _
                                      OrElse f.Extension.ToLower() = ".dvr-ms" _
                                      AndAlso Not f.Name.Contains("-trailer"))



                lFi.Sort(AddressOf CompFilesByName)

                alFileList.AddRange(lFi)
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Shared Function CompFilesByName(ByVal X As FileInfo, ByVal Y As FileInfo) As Integer

        '//
        ' Sort function for sorting our List(Of T) for FileInfo
        '\\

        Return X.FullName.CompareTo(Y.FullName)
    End Function

    Public Shared Sub GetAVImages(ByVal strAV As String, ByVal strPath As String)

        '//
        ' Parse the Flags XML and set the proper images
        '\\

        Dim mePath As String = Application.StartupPath & "\Images\Flags\"
        If File.Exists(mePath & "Flags.xml") Then
            Try
                Dim strWithoutFirst As String = String.Format("{0} {1}", Strings.Trim(Strings.Right(strAV, strAV.Length - Strings.InStr(strAV, "/"))).ToLower, Path.GetFileName(strPath).ToLower)
                Dim atypeRef As String = String.Empty
                Dim vresImage As String = String.Empty
                Dim vsourceImage As String = String.Empty
                Dim atypeImage As String = String.Empty
                Dim achanImage As String = String.Empty

                Dim xmlFlags As XDocument = XDocument.Load(mePath & "Flags.xml")

                'video resolution
                Dim xVResDefault = From xDef In xmlFlags...<vres> Select xDef.Element("default").Element("icon").Value
                If xVResDefault.Count > 0 Then
                    vresImage = mePath & xVResDefault(0).ToString
                End If

                Dim xVResFlag = From xVRes In xmlFlags...<vres>...<name> Where Regex.IsMatch(strWithoutFirst, xVRes.@searchstring) Select xVRes.<icon>.Value
                If xVResFlag.Count > 0 Then
                    vresImage = mePath & xVResFlag(0).ToString
                End If

                'video source
                Dim xVSourceDefault = From xDef In xmlFlags...<vtype> Select xDef.Element("default").Element("icon").Value
                If xVSourceDefault.Count > 0 Then
                    vsourceImage = mePath & xVSourceDefault(0).ToString
                End If

                Dim xVSourceFlag = From xVSource In xmlFlags...<vtype>...<name> Where Regex.IsMatch(strWithoutFirst, xVSource.@searchstring) Select xVSource.<icon>.Value
                If xVSourceFlag.Count > 0 Then
                    vsourceImage = mePath & xVSourceFlag(0).ToString
                End If

                'audio type
                Dim xATypeDefault = From xDef In xmlFlags...<atype> Select xDef.Element("default").Element("icon").Value
                If xATypeDefault.Count > 0 Then
                    atypeImage = mePath & xATypeDefault(0).ToString
                End If

                Dim xATypeFlag = From xAType In xmlFlags...<atype>...<name> Where Regex.IsMatch(strWithoutFirst, xAType.@searchstring) Select xAType.<icon>.Value, xAType.<ref>.Value
                If xATypeFlag.Count > 0 Then
                    atypeImage = mePath & xATypeFlag(0).icon.ToString
                    If Not IsNothing(xATypeFlag(0).ref) Then
                        atypeRef = xATypeFlag(0).ref.ToString
                    End If
                End If

                'audio channels
                Dim xAChanDefault = From xDef In xmlFlags...<achan> Select xDef.Element("default").Element("icon").Value
                If xAChanDefault.Count > 0 Then
                    achanImage = mePath & xAChanDefault(0).ToString
                End If

                Dim xAChanFlag = From xAChan In xmlFlags...<achan>...<name> Where Regex.IsMatch(strWithoutFirst, Regex.Replace(xAChan.@searchstring, "(\{[^\}]+\})", String.Empty)) And Regex.IsMatch(atypeRef, Regex.Match(xAChan.@searchstring, "\{atype=([^\}]+)\}").Groups(1).Value.ToString) Select xAChan.<icon>.Value
                If xAChanFlag.Count > 0 Then
                    achanImage = mePath & xAChanFlag(0).ToString
                End If

                If File.Exists(vresImage) Then
                    Dim fsImage As New FileStream(vresImage, FileMode.Open, FileAccess.Read)
                    frmMain.pbResolution.Image = Image.FromStream(fsImage)
                    fsImage.Close()
                    fsImage = Nothing
                End If

                If File.Exists(vsourceImage) Then
                    Dim fsImage As New FileStream(vsourceImage, FileMode.Open, FileAccess.Read)
                    frmMain.pbVideo.Image = Image.FromStream(fsImage)
                    fsImage.Close()
                    fsImage = Nothing
                End If

                If File.Exists(atypeImage) Then
                    Dim fsImage As New FileStream(atypeImage, FileMode.Open, FileAccess.Read)
                    frmMain.pbAudio.Image = Image.FromStream(fsImage)
                    fsImage.Close()
                    fsImage = Nothing
                End If

                If File.Exists(achanImage) Then
                    Dim fsImage As New FileStream(achanImage, FileMode.Open, FileAccess.Read)
                    frmMain.pbChannels.Image = Image.FromStream(fsImage)
                    fsImage.Close()
                    fsImage = Nothing
                End If
            Catch ex As Exception
                eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            MsgBox("Cannot find Flags.xml." & vbNewLine & vbNewLine & "Expected path:" & vbNewLine & mePath & "Flags.xml", MsgBoxStyle.Critical, "File Not Found")
        End If
    End Sub

    Public Shared Function GetStudioImage(ByVal strStudio As String) As Image

        '//
        ' Parse the Studio XML and set the proper image
        '\\

        Dim imgStudioStr As String = String.Empty
        Dim imgStudio As Image = Nothing
        Dim mePath As String = Application.StartupPath & "\Images\Studios\"

        If File.Exists(mePath & "Studios.xml") Then
            Try
                Dim xmlStudio As XDocument = XDocument.Load(mePath & "Studios.xml")

                Dim xDefault = From xDef In xmlStudio...<default> Select xDef.<icon>.Value
                If xDefault.Count > 0 Then
                    imgStudioStr = mePath & xDefault(0).ToString
                End If

                Dim xStudio = From xStu In xmlStudio...<name> Where Regex.IsMatch(Strings.Trim(strStudio).ToLower, xStu.@searchstring) Select xStu.<icon>.Value
                If xStudio.Count > 0 Then
                    imgStudioStr = mePath & xStudio(0).ToString
                End If

                If Not String.IsNullOrEmpty(imgStudioStr) AndAlso File.Exists(imgStudioStr) Then
                    Dim fsImage As New FileStream(imgStudioStr, FileMode.Open, FileAccess.Read)
                    imgStudio = Image.FromStream(fsImage)
                    fsImage.Close()
                    fsImage = Nothing
                End If

            Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Else
            MsgBox("Cannot find Studios.xml." & vbNewLine & vbNewLine & "Expected path:" & vbNewLine & mePath & "Studios.xml", MsgBoxStyle.Critical, "File Not Found")
        End If

        Return imgStudio

    End Function

    Public Shared Function GetGenreImage(ByVal strGenre As String) As Image

        '//
        ' Set the proper images based on the genre string
        '\\

        Dim imgGenre As Image = Nothing
        Dim imgGenreStr As String = String.Empty

        Dim mePath As String = Application.StartupPath & "\Images\Genres\"

        If File.Exists(mePath & "Genres.xml") Then
            Try
                Dim xmlGenre As XDocument = XDocument.Load(mePath & "Genres.xml")

                Dim xDefault = From xDef In xmlGenre...<default> Select xDef.<icon>.Value
                If xDefault.Count > 0 Then
                    imgGenreStr = mePath & xDefault(0).ToString
                End If

                Dim xGenre = From xGen In xmlGenre...<name> Where strGenre.ToLower = xGen.@searchstring.ToLower Select xGen.<icon>.Value
                If xGenre.Count > 0 Then
                    imgGenreStr = mePath & xGenre(0).ToString
                End If

                If Not String.IsNullOrEmpty(imgGenreStr) AndAlso File.Exists(imgGenreStr) Then
                    Dim fsImage As New FileStream(imgGenreStr, FileMode.Open, FileAccess.Read)
                    imgGenre = Image.FromStream(fsImage)
                    fsImage.Close()
                    fsImage = Nothing
                End If

            Catch ex As Exception
                eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        Else
            MsgBox("Cannot find Genres.xml." & vbNewLine & vbNewLine & "Expected path:" & vbNewLine & mePath & "Genres.xml", MsgBoxStyle.Critical, "File Not Found")
        End If

        Return imgGenre
    End Function

    Public Shared Function isValidDir(ByVal sPath As String) As Boolean

        '//
        ' Make sure it's a valid directory
        '\\

        Try
            If Strings.Right(sPath.ToLower, 11) = "extrathumbs" OrElse _
            Strings.Right(sPath.ToLower, 3) = "cd1" OrElse _
            Strings.Right(sPath.ToLower, 3) = "cd2" OrElse _
            Strings.Right(sPath.ToLower, 3) = "cd3" OrElse _
            Strings.Right(sPath.ToLower, 4) = "subs" OrElse _
            Strings.Right(sPath.ToLower, 9) = "subtitles" OrElse _
            Strings.Right(sPath.ToLower, 6) = "extras" OrElse _
            Strings.Right(sPath.ToLower, 8) = "video_ts" OrElse _
            Strings.Right(sPath.ToLower, 8) = "audio_ts" OrElse _
            Strings.Right(sPath.ToLower, 7) = "trailer" OrElse _
            Strings.Right(sPath.ToLower, 15) = "temporary files" OrElse _
            Strings.Right(sPath.ToLower, 8) = "(noscan)" OrElse _
            Strings.Right(sPath.ToLower, 6) = "sample" OrElse _
            Strings.Right(sPath.ToLower, 8) = "recycler" OrElse _
            Strings.Right(sPath.ToLower, 12) = "$recycle.bin" OrElse _
            Strings.Right(sPath.ToLower, 10) = "lost+found" OrElse _
            sPath.ToLower.Contains("system volume information") Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Return False
        End Try

    End Function

    Public Shared Function GetNameFromPath(ByVal sPath As String)

        '//
        ' Return the folder name
        '\\

        Try
            'this seems to be the best way to do this. I could use Directory.GetParent, but the path that is
            'passed to the function may be a folder so it would return the folder name above the one we want
            Return Strings.Right(sPath, Len(sPath) - (Strings.InStrRev(sPath, "\")))
        Catch
            Return String.Empty
        End Try

    End Function

    Public Shared Function GetExtFromPath(ByVal sPath As String)

        '//
        ' Get the extention
        '\\

        Try
            Return Path.GetExtension(sPath).ToString
        Catch
            Return String.Empty
        End Try

    End Function

    Public Shared Function RemoveExtFromFile(ByVal sFile As String)

        '//
        ' Get the filename without the extension
        '\\

        Try
            Return Path.GetFileNameWithoutExtension(sFile).ToString
        Catch
            Return String.Empty
        End Try

    End Function

    Public Shared Function RemoveExtFromPath(ByVal sPath As String)

        '//
        ' Get the entire path without the extension
        '\\

        Try
            Return String.Concat(Directory.GetParent(sPath).FullName.ToString, "\", Path.GetFileNameWithoutExtension(sPath).ToString)
        Catch
            Return String.Empty
        End Try

    End Function

    Public Shared Sub SaveSingleNFOItem(ByVal sPath As String, ByVal strToWrite As String, ByVal strNode As String)

        '//
        ' Save just one item of an NFO file
        '\\

        Try
            Dim xmlDoc As New XmlDocument()
            'use streamreader to open NFO so we don't get any access violations when trying to save
            Dim xmlSR As New StreamReader(sPath)
            'copy NFO to string
            Dim xmlString As String = xmlSR.ReadToEnd
            'close the streamreader... we're done with it
            xmlSR.Close()
            xmlSR = Nothing

            xmlDoc.LoadXml(xmlString)
            Dim xNode As XmlNode = xmlDoc.SelectSingleNode(strNode)
            xNode.InnerText = strToWrite
            xmlDoc.Save(sPath)

            xmlDoc = Nothing
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function LoadMovieFromNFO(ByVal sPath As String) As Media.Movie

        '//
        ' Deserialze the NFO to pass all the data to a Media.Movie
        '\\

        Dim xmlSR As StreamReader = Nothing
        Dim xmlSer As XmlSerializer = Nothing
        Try
            If File.Exists(sPath) Then
                xmlSer = New XmlSerializer(GetType(Media.Movie))
                xmlSR = New StreamReader(sPath)
                Dim xmlMov As Media.Movie = CType(xmlSer.Deserialize(xmlSR), Media.Movie)

                xmlSR.Close()
                xmlSR = Nothing
                xmlSer = Nothing

                Return xmlMov
            Else
                Return New Media.Movie
            End If
        Catch
            If Not IsNothing(xmlSR) Then
                xmlSR.Close()
                xmlSR = Nothing
            End If

            If Not IsNothing(xmlSer) Then
                xmlSer = Nothing
            End If
            'non-conforming nfo... rename per setting
            If Not uSettings.OverwriteNfo Then
                Dim i As Integer = 2
                Dim strNewName As String = GetNameFromPath(RemoveExtFromPath(sPath)) & ".info"
                Do While File.Exists(strNewName)
                    strNewName = String.Format("{0}({1}).info", GetNameFromPath(RemoveExtFromPath(sPath)), i)
                    i += 1
                Loop
                My.Computer.FileSystem.RenameFile(sPath, strNewName)
            End If

            Return New Media.Movie
        End Try

    End Function

    Public Shared Function GetFolderContents(ByVal sPath As String, ByVal isFile As Boolean)

        '//
        ' Check if a folder has all the items (nfo, poster, fanart, etc)
        ' Why 2 methods? Because it's faster to scan each file in folder mode and faster to scan
        ' for specific files in file mode.
        '\\

        Dim hasNfo As Boolean = False
        Dim hasPoster As Boolean = False
        Dim hasFanart As Boolean = False
        Dim hasTrailer As Boolean = False
        Dim aResults(3) As Boolean
        Dim tmpName As String = String.Empty
        Try


            If isFile Then
                tmpName = String.Format("{0}\{1}", Directory.GetParent(sPath).FullName.ToString, CleanStackingMarkers(RemoveExtFromFile(GetNameFromPath(sPath))))
                'fanart
                If File.Exists(String.Concat(tmpName, "-fanart.jpg")) OrElse File.Exists(String.Concat(tmpName, ".fanart.jpg")) OrElse File.Exists(String.Concat(Directory.GetParent(sPath).ToString, "\fanart.jpg")) Then
                    hasFanart = True
                End If

                'poster
                If File.Exists(String.Concat(tmpName, ".jpg")) OrElse File.Exists(String.Concat(Directory.GetParent(sPath).ToString, "\movie.jpg")) OrElse _
                    File.Exists(String.Concat(Directory.GetParent(sPath).ToString, "\poster.jpg")) OrElse File.Exists(String.Concat(Directory.GetParent(sPath).ToString, "\folder.jpg")) OrElse _
                    File.Exists(String.Concat(tmpName, ".tbn")) OrElse File.Exists(String.Concat(Directory.GetParent(sPath).ToString, "\movie.tbn")) OrElse _
                    File.Exists(String.Concat(Directory.GetParent(sPath).ToString, "\poster.tbn")) Then
                    hasPoster = True
                End If

                'nfo
                If File.Exists(String.Concat(tmpName, ".nfo")) OrElse File.Exists(String.Concat(Directory.GetParent(sPath).ToString, "\movie.nfo")) Then
                    hasNfo = True
                End If

                Dim sExt() As String = Split(".avi,.divx,.mkv,.iso,.mpg,.mp4,.wmv,.wma,.mov,.mts,.m2t,.img,.dat,.bin,.cue,.vob,.dvb,.evo,.asf,.asx,.avs,.nsv,.ram,.ogg,.ogm,.ogv,.flv,.swf,.nut,.viv,.rar,.m2ts,.dvr-ms")

                For Each t As String In sExt
                    If File.Exists(String.Concat(tmpName, "-trailer", t)) Then
                        hasTrailer = True
                        Exit For
                    End If
                Next
            Else
                Dim di As New DirectoryInfo(Directory.GetParent(sPath).FullName.ToString)
                Dim lFi As New List(Of FileInfo)()

                lFi.AddRange(di.GetFiles())

                For Each sfile As FileInfo In lFi
                    tmpName = CleanStackingMarkers(RemoveExtFromFile(GetNameFromPath(sPath)))
                    Select Case sfile.Extension.ToLower
                        Case ".jpg"
                            If sfile.Name.ToLower = String.Concat(tmpName, "-fanart.jpg") OrElse sfile.Name.ToLower = String.Concat(tmpName, ".fanart.jpg") OrElse sfile.FullName.ToLower = String.Concat(Directory.GetParent(sPath).ToString, "\fanart.jpg") Then
                                hasFanart = True
                            ElseIf sfile.Name.ToLower = String.Concat(tmpName, ".jpg") OrElse sfile.FullName.ToLower = String.Concat(Directory.GetParent(sPath).ToString, "\movie.jpg") OrElse _
                            sfile.FullName.ToLower = String.Concat(Directory.GetParent(sPath).ToString, "\poster.jpg") OrElse sfile.FullName.ToLower = String.Concat(Directory.GetParent(sPath).ToString, "\folder.jpg") Then
                                hasPoster = True
                            End If
                        Case ".tbn"
                            If sfile.Name.ToLower = String.Concat(tmpName, ".tbn") OrElse sfile.FullName.ToLower = String.Concat(Directory.GetParent(sPath).ToString, "\movie.tbn") OrElse _
                                sfile.FullName.ToLower = String.Concat(Directory.GetParent(sPath).ToString, "\poster.tbn") Then
                                hasPoster = True
                            End If
                        Case ".nfo"
                            If sfile.Name.ToLower = String.Concat(tmpName, ".nfo") OrElse sfile.FullName.ToLower = String.Concat(Directory.GetParent(sPath).ToString, "\movie.nfo") Then
                                hasNfo = True
                            End If
                        Case ".avi", ".divx", ".mkv", ".iso", ".mpg", ".mp4", ".wmv", ".wma", ".mov", ".mts", ".m2t", ".img", ".dat", ".bin", ".cue", ".vob", ".dvb", ".evo", ".asf", ".asx", ".avs", ".nsv", ".ram", ".ogg", ".ogm", ".ogv", ".flv", ".swf", ".nut", ".viv", ".rar", ".m2ts", ".dvr-ms"
                            If sfile.Name.Contains("-trailer") Then
                                hasTrailer = True
                            End If
                    End Select

                Next
            End If
            aResults(0) = hasPoster
            aResults(1) = hasFanart
            aResults(2) = hasNfo
            aResults(3) = hasTrailer
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return aResults
    End Function

    Public Shared Function FIToString(ByRef miFI As MediaInfo.Fileinfo) As String

        '//
        ' Convert Fileinfo into a string to be displayed in the GUI
        '\\

        Dim strOutput As String = String.Empty
        Dim iVS As Integer = 1
        Dim iAS As Integer = 1
        Dim iSS As Integer = 1

        Try
            Dim strTag As String = FITagData(miFI)
            If Not String.IsNullOrEmpty(strTag) Then
                strOutput += String.Format("Tag: {0}{1}{2}", strTag, vbNewLine, vbNewLine)
            End If
            If Not miFI.StreamDetails Is Nothing Then
                If miFI.StreamDetails.Video.Count > 0 Then
                    strOutput += String.Format("Video Streams: {0}{1}", miFI.StreamDetails.Video.Count.ToString, vbNewLine)
                End If

                If miFI.StreamDetails.Audio.Count > 0 Then
                    strOutput += String.Format("Audio Streams: {0}{1}", miFI.StreamDetails.Audio.Count.ToString, vbNewLine)
                End If

                If miFI.StreamDetails.Subtitle.Count > 0 Then
                    strOutput += String.Format("Subtitle Streams: {0}{1}", miFI.StreamDetails.Subtitle.Count.ToString, vbNewLine)
                End If

                For Each miVideo As MediaInfo.Video In miFI.StreamDetails.Video
                    strOutput += String.Format("{0}Video Stream {1}{0}", vbNewLine, iVS.ToString)
                    strOutput += String.Format("- Size: {0}x{1}{2}", miVideo.Width, miVideo.Height, vbNewLine)
                    strOutput += String.Format("- Display Aspect Ratio: {0}{1}", miVideo.AspectDisplayRatio, vbNewLine)
                    strOutput += String.Format("- Codec: {0}{1}", miVideo.Codec, vbNewLine)
                    strOutput += String.Format("- Format Info: {0}{1}", miVideo.FormatInfo, vbNewLine)
                    strOutput += String.Format("- Duration: {0}{1}", miVideo.Duration, vbNewLine)
                    strOutput += String.Format("- BitRate: {0}{1}", miVideo.Bitrate, vbNewLine)
                    strOutput += String.Format("- BitRate_Mode: {0}{1}", miVideo.BitrateMode, vbNewLine)
                    strOutput += String.Format("- BitRate_Maximum: {0}{1}", miVideo.BitrateMax, vbNewLine)
                    strOutput += String.Format("- CodecID: {0}{1}", miVideo.CodecID, vbNewLine)
                    strOutput += String.Format("- CodecID Info: {0}{1}", miVideo.CodecidInfo, vbNewLine)
                    strOutput += String.Format("- Scan type: {0}{1}", miVideo.ScanType, vbNewLine)
                    iVS += 1
                Next

                For Each miAudio As MediaInfo.Audio In miFI.StreamDetails.Audio
                    'audio
                    strOutput += String.Format("{0}Audio Stream {1}{0}", vbNewLine, iAS.ToString)
                    strOutput += String.Format("- Codec: {0}{1}", miAudio.Codec, vbNewLine)
                    strOutput += String.Format("- Channels: {0}{1}", miAudio.Channels, vbNewLine)
                    strOutput += String.Format("- BitRate: {0}{1}", miAudio.Bitrate, vbNewLine)
                    strOutput += String.Format("- Language: {0}{1}", miAudio.Language, vbNewLine)
                    iAS += 1
                Next

                For Each miSub As MediaInfo.Subtitle In miFI.StreamDetails.Subtitle
                    'subtitles
                    strOutput += String.Format("{0}Subtitle {1}{0}", vbNewLine, iSS.ToString)
                    strOutput += String.Format("- Language: {0}", miSub.Language)
                    iSS += 1
                Next
            End If

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If strOutput.Length > 0 Then
            Return strOutput
        Else
            Return "Media Info is not available for this movie. Try rescanning."
        End If
    End Function

    Public Shared Function FITagData(ByRef miFI As MediaInfo.Fileinfo) As String

        '//
        ' Convert FileInfo into the studio tag
        '\\

        Dim statusStr As String = String.Empty
        Try
            If Not IsNothing(miFI.StreamDetails) Then
                Dim hasVS As Boolean = False
                Dim sinWidest As Single = 0
                Dim sinWidth As Single = 0
                Dim sinHeight As Single = 0
                Dim sinADR As Single = 0
                Dim sScanType As String = String.Empty
                Dim sCodec As String = String.Empty
                Dim sinMostChannels As Single = 0
                Dim sinChans As Single = 0
                Dim sACodec As String = String.Empty
                Dim sLang As String = String.Empty
                Dim sSubLang As String = String.Empty

                For Each miVideo As MediaInfo.Video In miFI.StreamDetails.Video
                    hasVS = True
                    Single.TryParse(miVideo.Width, sinWidth)
                    If sinWidth > sinWidest Then
                        sinWidest = sinWidth
                        Single.TryParse(miVideo.Height, sinHeight)
                        Single.TryParse(miVideo.AspectDisplayRatio, sinADR)
                        sScanType = If(miVideo.ScanType.ToLower.Contains("progressive"), "p", "i")
                        sCodec = miVideo.CodecID
                    End If
                Next

                For Each miAudio As MediaInfo.Audio In miFI.StreamDetails.Audio
                    'audio
                    If Not String.IsNullOrEmpty(miAudio.Channels) Then
                        Single.TryParse(miAudio.Channels, sinChans)
                        If sinChans > sinMostChannels Then
                            sACodec = miAudio.Codec
                            sinMostChannels = sinChans
                            sLang = miAudio.Language
                        End If
                    End If

                Next

                For Each curSS As MediaInfo.Subtitle In miFI.StreamDetails.Subtitle
                    'audio
                    sSubLang += String.Concat(" / sub", curSS.Language)
                Next

                If hasVS Then
                    statusStr = String.Format(" / {0}{1} / {2} / {3} / {4}ch / {5}{6}", GetResFromDimensions(sinWidest, sinHeight), sScanType, sCodec, sACodec, sinMostChannels, sLang, sSubLang)
                Else
                    Return String.Empty
                End If

            End If
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return statusStr
    End Function

    Private Shared Function GetResFromDimensions(ByVal sinWidth As Single, ByVal sinHeight As Single) As String

        '//
        ' Get the resolution of the video from the dimensions provided by MediaInfo.dll
        '\\

        Try
            If sinWidth >= 1600 AndAlso sinHeight >= 800 Then Return "1080"
            If sinWidth >= 1350 AndAlso sinHeight >= 750 Then Return "768"
            If sinWidth >= 960 AndAlso sinHeight >= 577 Then Return "720"
            If sinWidth >= 720 AndAlso sinHeight >= 521 Then Return "576"
            If sinWidth <= 720 AndAlso sinHeight >= 520 Then Return "540"
            If sinWidth < 640 Then Return "SD"
            If sinWidth <= 720 AndAlso sinHeight <= 520 Then Return "480"

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return String.Empty
    End Function

    Public Shared Function TruncateURL(ByVal sString As String, ByVal MaxLength As Integer) As String

        '//
        ' Shorten a URL to fit on the GUI
        '\\

        Try
            Dim sEnd As String = sString.Substring(sString.LastIndexOf("/"), sString.Length - sString.LastIndexOf("/"))
            If ((MaxLength - sEnd.Length) - 3) > 0 Then
                Return String.Format("{0}...{1}", Strings.Left(sString, (MaxLength - sEnd.Length) - 3), sEnd)
            Else
                Return sEnd
            End If

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return String.Empty
    End Function

    Public Shared Function GetNfoPath(ByVal sPath As String, ByVal isFile As Boolean) As String

        '//
        ' Get the proper path to NFO
        '\\

        Dim tmpName As String = CleanStackingMarkers(GetNameFromPath(sPath))
        Dim nPath As String = String.Concat(Directory.GetParent(sPath).FullName, "\", tmpName)

        If uSettings.MovieNameNFO AndAlso File.Exists(String.Concat(RemoveExtFromPath(nPath), ".nfo")) Then
            Return String.Concat(RemoveExtFromPath(nPath), ".nfo")
        ElseIf Not isFile AndAlso uSettings.MovieNFO AndAlso File.Exists(String.Concat(Directory.GetParent(nPath).ToString, "\movie.nfo")) Then
            Return String.Concat(Directory.GetParent(nPath).ToString, "\movie.nfo")
        Else
            Return String.Empty
        End If

    End Function

    Public Shared Sub SaveMovieToNFO(ByVal movieToSave As Media.Movie, ByVal sPath As String, ByVal isFile As Boolean)

        '//
        ' Serialize Media.Movie to an NFO
        '\\

        Try

            Dim tmpName As String = CleanStackingMarkers(GetNameFromPath(sPath))
            Dim nPath As String = String.Concat(Directory.GetParent(sPath).FullName, "\", tmpName)
            Dim xmlSer As New XmlSerializer(GetType(Media.Movie))
            Dim tPath As String = String.Empty

            If uSettings.MovieNameNFO OrElse isFile Then
                tPath = String.Concat(RemoveExtFromPath(nPath), ".nfo")
                If Not File.Exists(tPath) OrElse (Not CBool(File.GetAttributes(tPath) And FileAttributes.ReadOnly)) Then
                    Dim xmlSW As New StreamWriter(tPath)
                    xmlSer.Serialize(xmlSW, movieToSave)
                    xmlSW.Close()
                    xmlSW.Dispose()
                End If
            End If

            If Not isFile AndAlso uSettings.MovieNFO Then
                tPath = String.Concat(Directory.GetParent(nPath).ToString, "\movie.nfo")
                If Not File.Exists(tPath) OrElse (Not CBool(File.GetAttributes(tPath) And FileAttributes.ReadOnly)) Then
                    Dim xmlSW As New StreamWriter(tPath)
                    xmlSer.Serialize(xmlSW, movieToSave)
                    xmlSW.Close()
                    xmlSW.Dispose()
                End If
            End If

        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function GetTrailerPath(ByVal sPath As String) As String

        '//
        ' Get the proper path to trailer
        '\\

        Dim di As New DirectoryInfo(Directory.GetParent(sPath).FullName.ToString)
        Dim lFi As New List(Of FileInfo)()

        lFi.AddRange(di.GetFiles())

        lFi = lFi.FindAll(Function(f As FileInfo) (f.Extension.ToLower() = ".avi" _
                              OrElse f.Extension.ToLower() = ".divx" _
                              OrElse f.Extension.ToLower() = ".mkv" _
                              OrElse f.Extension.ToLower() = ".iso" _
                              OrElse f.Extension.ToLower() = ".mpg" _
                              OrElse f.Extension.ToLower() = ".mp4" _
                              OrElse f.Extension.ToLower() = ".wmv" _
                              OrElse f.Extension.ToLower() = ".wma" _
                              OrElse f.Extension.ToLower() = ".mov" _
                              OrElse f.Extension.ToLower() = ".mts" _
                              OrElse f.Extension.ToLower() = ".m2t" _
                              OrElse f.Extension.ToLower() = ".img" _
                              OrElse f.Extension.ToLower() = ".dat" _
                              OrElse f.Extension.ToLower() = ".bin" _
                              OrElse f.Extension.ToLower() = ".cue" _
                              OrElse f.Extension.ToLower() = ".vob" _
                              OrElse f.Extension.ToLower() = ".dvb" _
                              OrElse f.Extension.ToLower() = ".evo" _
                              OrElse f.Extension.ToLower() = ".asf" _
                              OrElse f.Extension.ToLower() = ".asx" _
                              OrElse f.Extension.ToLower() = ".avs" _
                              OrElse f.Extension.ToLower() = ".nsv" _
                              OrElse f.Extension.ToLower() = ".ram" _
                              OrElse f.Extension.ToLower() = ".ogg" _
                              OrElse f.Extension.ToLower() = ".ogm" _
                              OrElse f.Extension.ToLower() = ".ogv" _
                              OrElse f.Extension.ToLower() = ".flv" _
                              OrElse f.Extension.ToLower() = ".swf" _
                              OrElse f.Extension.ToLower() = ".nut" _
                              OrElse f.Extension.ToLower() = ".viv" _
                              OrElse f.Extension.ToLower() = ".rar" _
                              OrElse f.Extension.ToLower() = ".m2ts" _
                              OrElse f.Extension.ToLower() = ".dvr-ms") _
                              AndAlso f.Name.Contains("-trailer"))
        lFi.Sort(AddressOf CompFilesByName)

        If lFi.Count > 0 Then
            Return lFi(0).FullName.ToString
        Else
            Return String.Empty
        End If
    End Function

    Public Shared Function GetMoviePath(ByVal sPath As String) As String

        '//
        ' Get the proper path to movie
        '\\

        Dim di As New DirectoryInfo(sPath)
        Dim lFi As New List(Of FileInfo)()

        lFi.AddRange(di.GetFiles())

        lFi = lFi.FindAll(Function(f As FileInfo) (f.Extension.ToLower() = ".avi" _
                              OrElse f.Extension.ToLower() = ".divx" _
                              OrElse f.Extension.ToLower() = ".mkv" _
                              OrElse f.Extension.ToLower() = ".iso" _
                              OrElse f.Extension.ToLower() = ".mpg" _
                              OrElse f.Extension.ToLower() = ".mp4" _
                              OrElse f.Extension.ToLower() = ".wmv" _
                              OrElse f.Extension.ToLower() = ".wma" _
                              OrElse f.Extension.ToLower() = ".mov" _
                              OrElse f.Extension.ToLower() = ".mts" _
                              OrElse f.Extension.ToLower() = ".m2t" _
                              OrElse f.Extension.ToLower() = ".img" _
                              OrElse f.Extension.ToLower() = ".dat" _
                              OrElse f.Extension.ToLower() = ".bin" _
                              OrElse f.Extension.ToLower() = ".cue" _
                              OrElse f.Extension.ToLower() = ".vob" _
                              OrElse f.Extension.ToLower() = ".dvb" _
                              OrElse f.Extension.ToLower() = ".evo" _
                              OrElse f.Extension.ToLower() = ".asf" _
                              OrElse f.Extension.ToLower() = ".asx" _
                              OrElse f.Extension.ToLower() = ".avs" _
                              OrElse f.Extension.ToLower() = ".nsv" _
                              OrElse f.Extension.ToLower() = ".ram" _
                              OrElse f.Extension.ToLower() = ".ogg" _
                              OrElse f.Extension.ToLower() = ".ogm" _
                              OrElse f.Extension.ToLower() = ".ogv" _
                              OrElse f.Extension.ToLower() = ".flv" _
                              OrElse f.Extension.ToLower() = ".swf" _
                              OrElse f.Extension.ToLower() = ".nut" _
                              OrElse f.Extension.ToLower() = ".viv" _
                              OrElse f.Extension.ToLower() = ".rar" _
                              OrElse f.Extension.ToLower() = ".m2ts" _
                              OrElse f.Extension.ToLower() = ".dvr-ms") _
                              AndAlso Not f.Name.Contains("-trailer"))

        lFi.Sort(AddressOf CompFilesByName)

        If lFi.Count > 0 Then
            Return lFi(0).FullName.ToString
        Else
            Return String.Empty
        End If
    End Function


    Public Shared Function GetExtraModifier(ByVal sPath As String) As Integer

        '//
        ' Get the number of the last thumb#.jpg to make sure we're not overwriting current ones
        '\\

        Dim iMod As Integer = 0
        Dim alThumbs As New ArrayList

        Try
            Dim d As New DirectoryInfo(sPath)
            Dim extraPath As String = String.Concat(d.Parent.FullName, "\extrathumbs")

            If Not Directory.Exists(extraPath) Then
                iMod = -1
            Else
                Dim dirInfo As New DirectoryInfo(extraPath)

                Dim ioFi As IO.FileInfo() = dirInfo.GetFiles("thumb*.jpg")

                For Each sFile As IO.FileInfo In ioFi
                    alThumbs.Add(sFile.Name)
                Next

                ioFi = Nothing

                If alThumbs.Count > 0 Then
                    alThumbs.Sort()
                    iMod = CInt(Regex.Match(alThumbs.Item(alThumbs.Count - 1), "\d+").ToString)
                Else
                    iMod = -1
                End If
            End If

            alThumbs = Nothing
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Return iMod
    End Function

    Public Shared Sub MoveFileWithStream(ByVal sPathFrom As String, ByVal sPathTo As String)

        '//
        ' Copy a file from one location to another using a stream/buffer
        '\\

        Try
            Dim sourceStream As FileStream = New FileStream(sPathFrom, FileMode.Open, FileAccess.Read)
            Dim destinationStream As FileStream = New FileStream(sPathTo, FileMode.OpenOrCreate, FileAccess.Write)
            Dim streamBuffer(sourceStream.Length - 1) As Byte

            sourceStream.Read(streamBuffer, 0, streamBuffer.Length)
            destinationStream.Write(streamBuffer, 0, streamBuffer.Length)

            streamBuffer = Nothing
            destinationStream.Close()
            destinationStream.Dispose()
            sourceStream.Close()
            sourceStream.Dispose()
        Catch ex As Exception
            eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub
End Class
