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
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Text.RegularExpressions

Public Class NFO

    Public Class NonConf
        Private _imdbid As String
        Private _text As String

        Public Property IMDBID() As String
            Get
                Return Me._imdbid
            End Get
            Set(ByVal value As String)
                Me._imdbid = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return Me._text
            End Get
            Set(ByVal value As String)
                Me._text = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._imdbid = String.Empty
            Me._text = String.Empty
        End Sub
    End Class

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
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function LoadMovieFromNFO(ByVal sPath As String, ByVal isSingle As Boolean) As Media.Movie

        '//
        ' Deserialze the NFO to pass all the data to a Media.Movie
        '\\

        Dim xmlSer As XmlSerializer = Nothing
        Dim xmlMov As New Media.Movie
        Try
            If File.Exists(sPath) AndAlso Path.GetExtension(sPath).ToLower = ".nfo" Then
                Using xmlSR As StreamReader = New StreamReader(sPath)
                    'xmlSer = New XmlSerializer(GetType(Media.Movie))
                    xmlSer = Media.Movie.GetSerialiser()
                    xmlMov = CType(xmlSer.Deserialize(xmlSR), Media.Movie)
                End Using
            Else
                If Not String.IsNullOrEmpty(sPath) Then
                    Dim sReturn As New NonConf
                    sReturn = GetIMDBFromNonConf(sPath, isSingle)
                    xmlMov.IMDBID = sReturn.IMDBID
                    Try
                        If Not String.IsNullOrEmpty(sReturn.Text) Then
                            Using xmlSTR As StringReader = New StringReader(sReturn.Text)
                                xmlSer = Media.Movie.GetSerialiser()
                                'xmlSer = New XmlSerializer(GetType(Media.Movie))
                                xmlMov = CType(xmlSer.Deserialize(xmlSTR), Media.Movie)
                                xmlMov.IMDBID = sReturn.IMDBID
                            End Using
                        End If
                    Catch
                    End Try
                End If
            End If

        Catch
            xmlMov.Clear()
            If Not String.IsNullOrEmpty(sPath) Then

                'go ahead and rename it now, will still be picked up in getimdbfromnonconf
                If Not Master.eSettings.OverwriteNfo Then
                    RenameNonConfNfo(sPath, True)
                End If

                Dim sReturn As New NonConf
                sReturn = GetIMDBFromNonConf(sPath, isSingle)
                xmlMov.IMDBID = sReturn.IMDBID
                Try
                    If Not String.IsNullOrEmpty(sReturn.Text) Then
                        Using xmlSTR As StringReader = New StringReader(sReturn.Text)
                            xmlSer = Media.Movie.GetSerialiser()
                            'xmlSer = New XmlSerializer(GetType(Media.Movie))
                            xmlMov = CType(xmlSer.Deserialize(xmlSTR), Media.Movie)
                            xmlMov.IMDBID = sReturn.IMDBID
                        End Using
                    End If
                Catch
                End Try
            End If
        End Try

        If Not IsNothing(xmlSer) Then
            xmlSer = Nothing
        End If

        Return xmlMov

    End Function

    Public Shared Function GetIMDBFromNonConf(ByVal sPath As String, ByVal isSingle As Boolean) As NonConf
        Dim tNonConf As New NonConf
        Dim dirPath As String = Directory.GetParent(sPath).FullName
        Dim lFiles As New ArrayList

        If isSingle Then
            Try
                lFiles.AddRange(Directory.GetFiles(dirPath, "*.nfo"))
            Catch
            End Try
            Try
                lFiles.AddRange(Directory.GetFiles(dirPath, "*.info"))
            Catch
            End Try
        Else
            Dim fName As String = Path.GetFileNameWithoutExtension(sPath)
            Dim oName As String = Directory.GetParent(sPath).Name
            Try
                lFiles.AddRange(Directory.GetFiles(dirPath, String.Concat(fName, "*.nfo")))
            Catch
            End Try
            Try
                lFiles.AddRange(Directory.GetFiles(dirPath, String.Concat(oName, "*.nfo")))
            Catch
            End Try
            Try
                lFiles.AddRange(Directory.GetFiles(dirPath, String.Concat(fName, "*.info")))
            Catch
            End Try
            Try
                lFiles.AddRange(Directory.GetFiles(dirPath, String.Concat(oName, "*.info")))
            Catch
            End Try
        End If

        For Each sFile As String In lFiles
            Using srInfo As New StreamReader(sFile)
                Dim sInfo As String = srInfo.ReadToEnd
                Dim sIMDBID As String = Regex.Match(sInfo, "tt\d\d\d\d\d\d\d", RegexOptions.Multiline Or RegexOptions.Singleline Or RegexOptions.IgnoreCase).ToString

                If Not String.IsNullOrEmpty(sIMDBID) Then
                    tNonConf.IMDBID = sIMDBID
                    'now lets try to see if the rest of the file is a proper nfo
                    If sInfo.ToLower.Contains("</movie>") Then
                        tNonConf.Text = XML.XMLToLowerCase(sInfo.Substring(0, sInfo.ToLower.IndexOf("</movie>") + 8))
                    End If
                    Exit For
                Else
                    sIMDBID = Regex.Match(sPath, "tt\d\d\d\d\d\d\d", RegexOptions.Multiline Or RegexOptions.Singleline Or RegexOptions.IgnoreCase).ToString
                    If Not String.IsNullOrEmpty(sIMDBID) Then
                        tNonConf.IMDBID = sIMDBID
                    End If
                End If
            End Using
        Next
        Return tNonConf
    End Function

    Public Shared Function FIToString(ByVal miFI As MediaInfo.Fileinfo) As String

        '//
        ' Convert Fileinfo into a string to be displayed in the GUI
        '\\

        Dim strOutput As New StringBuilder
        Dim iVS As Integer = 1
        Dim iAS As Integer = 1
        Dim iSS As Integer = 1

        Try
            If Not IsNothing(miFI) Then

                If Not IsNothing(miFI.StreamDetails) Then
                    If miFI.StreamDetails.Video.Count > 0 Then
                        strOutput.AppendFormat("{0}: {1}{2}", Master.eLang.GetString(595, "Video Streams"), miFI.StreamDetails.Video.Count.ToString, vbNewLine)
                    End If

                    If miFI.StreamDetails.Audio.Count > 0 Then
                        strOutput.AppendFormat("{0}: {1}{2}", Master.eLang.GetString(596, "Audio Streams"), miFI.StreamDetails.Audio.Count.ToString, vbNewLine)
                    End If

                    If miFI.StreamDetails.Subtitle.Count > 0 Then
                        strOutput.AppendFormat("{0}: {1}{2}", Master.eLang.GetString(597, "Subtitle  Streams"), miFI.StreamDetails.Subtitle.Count.ToString, vbNewLine)
                    End If

                    For Each miVideo As MediaInfo.Video In miFI.StreamDetails.Video
                        strOutput.AppendFormat("{0}{1} {2}{0}", vbNewLine, Master.eLang.GetString(617, "Video Stream"), iVS)
                        If Not String.IsNullOrEmpty(miVideo.Width) AndAlso Not String.IsNullOrEmpty(miVideo.Height) Then
                            strOutput.AppendFormat("- {0}{1}", String.Format(Master.eLang.GetString(269, "Size: {0}x{1}"), miVideo.Width, miVideo.Height), vbNewLine)
                        End If
                        If Not String.IsNullOrEmpty(miVideo.Aspect) Then strOutput.AppendFormat("- {0}: {1}{2}", Master.eLang.GetString(614, "Aspect Ratio"), miVideo.Aspect, vbNewLine)
                        If Not String.IsNullOrEmpty(miVideo.Scantype) Then strOutput.AppendFormat("- {0}: {1}{2}", Master.eLang.GetString(605, "Scan Type"), miVideo.Scantype, vbNewLine)
                        If Not String.IsNullOrEmpty(miVideo.Codec) Then strOutput.AppendFormat("- {0}: {1}{2}", Master.eLang.GetString(604, "Codec"), miVideo.Codec, vbNewLine)
                        If Not String.IsNullOrEmpty(miVideo.Duration) Then strOutput.AppendFormat("- {0}: {1}", Master.eLang.GetString(609, "Duration"), miVideo.Duration)
                        iVS += 1
                    Next

                    strOutput.Append(vbNewLine)

                    For Each miAudio As MediaInfo.Audio In miFI.StreamDetails.Audio
                        'audio
                        strOutput.AppendFormat("{0}{1} {2}{0}", vbNewLine, Master.eLang.GetString(618, "Audio Stream"), iAS.ToString)
                        If Not String.IsNullOrEmpty(miAudio.Codec) Then strOutput.AppendFormat("- {0}: {1}{2}", Master.eLang.GetString(604, "Codec"), miAudio.Codec, vbNewLine)
                        If Not String.IsNullOrEmpty(miAudio.Channels) Then strOutput.AppendFormat("- {0}: {1}{2}", Master.eLang.GetString(611, "Channels"), miAudio.Channels, vbNewLine)
                        If Not String.IsNullOrEmpty(miAudio.LongLanguage) Then strOutput.AppendFormat("- {0}: {1}", Master.eLang.GetString(610, "Language"), miAudio.LongLanguage)
                        iAS += 1
                    Next

                    strOutput.Append(vbNewLine)

                    For Each miSub As MediaInfo.Subtitle In miFI.StreamDetails.Subtitle
                        'subtitles
                        strOutput.AppendFormat("{0}{1} {2}{0}", vbNewLine, Master.eLang.GetString(619, "Subtitle Stream"), iSS.ToString)
                        If Not String.IsNullOrEmpty(miSub.LongLanguage) Then strOutput.AppendFormat("- {0}: {1}", Master.eLang.GetString(610, "Language"), miSub.LongLanguage)
                        iSS += 1
                    Next
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If strOutput.ToString.Trim.Length > 0 Then
            Return strOutput.ToString
        Else
            Return Master.eLang.GetString(419, "Meta Data is not available for this movie. Try rescanning.")
        End If
    End Function

    Public Shared Function GetNfoPath(ByVal sPath As String, ByVal isSingle As Boolean) As String

        '//
        ' Get the proper path to NFO
        '\\

        Dim nPath As String = String.Empty

        If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(sPath).Name.ToLower = "video_ts" Then
            nPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(sPath).FullName).FullName, Directory.GetParent(Directory.GetParent(sPath).FullName).Name), ".nfo")
            If File.Exists(nPath) Then
                Return nPath
            Else
                If Not isSingle Then
                    Return String.Empty
                Else
                    'return movie path so we can use it for looking for non-conforming nfos
                    Return sPath
                End If
            End If
        Else
            Dim tmpName As String = StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sPath))
            Dim tmpNameNoStack As String = Path.GetFileNameWithoutExtension(sPath)
            nPath = Path.Combine(Directory.GetParent(sPath).FullName, tmpName).ToLower
            Dim nPathWithStack As String = Path.Combine(Directory.GetParent(sPath).FullName, tmpNameNoStack).ToLower

            Dim fList As New ArrayList
            Dim tList As New ArrayList
            Try
                tList.AddRange(Directory.GetFiles(Directory.GetParent(sPath).FullName, "*.nfo"))
            Catch
            End Try
            fList.AddRange(tList.Cast(Of String)().Select(Function(AL) AL.ToLower).ToArray)

            If isSingle AndAlso Master.eSettings.MovieNFO AndAlso fList.Contains(Path.Combine(Directory.GetParent(sPath).FullName.ToLower, "movie.nfo")) Then
                Return Path.Combine(Directory.GetParent(nPath).FullName.ToLower, "movie.nfo")
            ElseIf Master.eSettings.MovieNameNFO AndAlso fList.Contains(String.Concat(nPathWithStack, ".nfo")) Then
                Return String.Concat(nPathWithStack, ".nfo")
            ElseIf Master.eSettings.MovieNameNFO AndAlso fList.Contains(String.Concat(nPath, ".nfo")) Then
                Return String.Concat(nPath, ".nfo")
            Else
                If Not isSingle Then
                    Return String.Empty
                Else
                    'return movie path so we can use it for looking for non-conforming nfos
                    Return sPath
                End If
            End If
        End If

    End Function

    Public Shared Function GetBestVideo(ByVal miFIV As MediaInfo.Fileinfo) As MediaInfo.Video

        '//
        ' Get the highest values from file info
        '\\

        Dim fivOut As New MediaInfo.Video
        Try
            Dim iWidest As Integer = 0
            Dim iWidth As Integer = 0

            'set some defaults to make it easy on ourselves
            fivOut.Width = String.Empty
            fivOut.Height = String.Empty
            fivOut.Aspect = String.Empty
            fivOut.Codec = String.Empty
            fivOut.Duration = String.Empty
            fivOut.Scantype = String.Empty

            For Each miVideo As MediaInfo.Video In miFIV.StreamDetails.Video
                If Not String.IsNullOrEmpty(miVideo.Width) Then
                    iWidth = Convert.ToInt32(miVideo.Width)
                    If iWidth > iWidest Then
                        iWidest = iWidth
                        fivOut.Width = miVideo.Width
                        fivOut.Height = miVideo.Height
                        fivOut.Aspect = miVideo.Aspect
                        fivOut.Codec = miVideo.Codec
                        fivOut.Duration = miVideo.Duration
                        fivOut.Scantype = miVideo.Scantype
                    End If
                End If
            Next

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return fivOut

    End Function

    Public Shared Function GetBestAudio(ByVal miFIA As MediaInfo.Fileinfo) As MediaInfo.Audio

        '//
        ' Get the highest values from file info
        '\\

        Dim fiaOut As New MediaInfo.Audio
        Try
            Dim sinMostChannels As Single = 0
            Dim sinChans As Single = 0

            fiaOut.Codec = String.Empty
            fiaOut.Channels = String.Empty
            fiaOut.Language = String.Empty

            For Each miAudio As MediaInfo.Audio In miFIA.StreamDetails.Audio
                If Not String.IsNullOrEmpty(miAudio.Channels) Then
                    sinChans = Master.ConvertToSingle(miAudio.Channels)
                    If sinChans > sinMostChannels Then
                        sinMostChannels = sinChans
                        fiaOut.Codec = miAudio.Codec
                        fiaOut.Channels = sinChans.ToString
                        fiaOut.Language = miAudio.Language
                    End If
                End If

                If Not String.IsNullOrEmpty(Master.eSettings.FlagLang) AndAlso miAudio.LongLanguage.ToLower = Master.eSettings.FlagLang.ToLower Then fiaOut.HasPreferred = True
            Next

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return fiaOut
    End Function

    Public Shared Function GetResFromDimensions(ByVal fiRes As MediaInfo.Video) As String

        '//
        ' Get the resolution of the video from the dimensions provided by MediaInfo.dll
        '\\

        Dim resOut As String = String.Empty
        Try
            If Not String.IsNullOrEmpty(fiRes.Width) AndAlso Not String.IsNullOrEmpty(fiRes.Height) AndAlso Not String.IsNullOrEmpty(fiRes.Aspect) Then
                Dim iWidth As Integer = Convert.ToInt32(fiRes.Width)
                Dim iHeight As Integer = Convert.ToInt32(fiRes.Height)
                Dim sinADR As Single = Master.ConvertToSingle(fiRes.Aspect)

                Select Case True
                    Case iWidth < 640
                        resOut = "SD"
                        'exact
                    Case (iWidth = 1920 AndAlso (iHeight = 1080 OrElse iHeight = 800)) OrElse (iWidth = 1440 AndAlso iHeight = 1080) OrElse (iWidth = 1280 AndAlso iHeight = 1080)
                        resOut = "1080"
                    Case (iWidth = 1366 AndAlso iHeight = 768) OrElse (iWidth = 1024 AndAlso iHeight = 768)
                        resOut = "768"
                    Case (iWidth = 960 AndAlso iHeight = 720) OrElse (iWidth = 1280 AndAlso (iHeight = 720 OrElse iHeight = 544))
                        resOut = "720"
                    Case (iWidth = 1024 AndAlso iHeight = 576) OrElse (iWidth = 720 AndAlso iHeight = 576)
                        resOut = "576"
                    Case (iWidth = 720 OrElse iWidth = 960) AndAlso iHeight = 540
                        resOut = "540"
                    Case (iWidth = 852 OrElse iWidth = 720 OrElse iWidth = 704 OrElse iWidth = 640) AndAlso iHeight = 480
                        resOut = "480"
                        'by ADR
                    Case sinADR >= 1.4 AndAlso iWidth = 1920
                        resOut = "1080"
                    Case sinADR >= 1.4 AndAlso iWidth = 1366
                        resOut = "768"
                    Case sinADR >= 1.4 AndAlso iWidth = 1280
                        resOut = "720"
                    Case sinADR >= 1.4 AndAlso iWidth = 1024
                        resOut = "576"
                    Case sinADR >= 1.4 AndAlso iWidth = 960
                        resOut = "540"
                    Case sinADR >= 1.4 AndAlso iWidth = 852
                        resOut = "480"
                        'loose
                    Case iWidth >= 1200 AndAlso iHeight > 768
                        resOut = "1080"
                    Case iWidth >= 1000 AndAlso iHeight > 720
                        resOut = "768"
                    Case iWidth >= 1000 AndAlso iHeight > 500
                        resOut = "720"
                    Case iWidth >= 700 AndAlso iHeight > 540
                        resOut = "576"
                    Case iWidth >= 700 AndAlso iHeight > 480
                        resOut = "540"
                    Case Else
                        resOut = "480"
                End Select
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        If Not String.IsNullOrEmpty(resOut) Then
            If String.IsNullOrEmpty(fiRes.Scantype) Then
                Return String.Concat(resOut)
            Else
                Return String.Concat(resOut, If(fiRes.Scantype.ToLower = "progressive", "p", "i"))
            End If
        Else
            Return String.Empty
        End If
    End Function


    Public Shared Sub SaveMovieToNFO(ByRef movieToSave As Master.DBMovie)

        '//
        ' Serialize Media.Movie to an NFO
        '\\

        Try

            'Dim xmlSer As New XmlSerializer(GetType(Media.Movie))
            Dim xmlSer As XmlSerializer = Media.Movie.GetSerialiser()

            Dim tPath As String = String.Empty
            Dim nPath As String = String.Empty

            If Master.eSettings.VideoTSParent AndAlso Directory.GetParent(movieToSave.Filename).Name.ToLower = "video_ts" Then
                nPath = String.Concat(Path.Combine(Directory.GetParent(Directory.GetParent(movieToSave.Filename).FullName).FullName, Directory.GetParent(Directory.GetParent(movieToSave.Filename).FullName).Name), ".nfo")

                If Not Master.eSettings.OverwriteNfo Then
                    RenameNonConfNfo(tPath, False)
                End If

                If Not File.Exists(nPath) OrElse (Not CBool(File.GetAttributes(nPath) And FileAttributes.ReadOnly)) Then
                    movieToSave.NfoPath = tPath
                    ''Using memData As New IO.MemoryStream
                    ''    xmlSer.Serialize(memData, movieToSave.Movie)
                    ''    memData.Position = 0
                    ''    Using sr As New StreamReader(memData, Encoding.UTF8)
                    ''        Dim sData As String = sr.ReadToEnd
                    ''        sData = sData.Replace("<sets>" & Environment.NewLine, "")
                    ''        sData = sData.Replace("</sets>" & Environment.NewLine, "")
                    ''        IO.File.WriteAllText(tPath, sData)
                    ''    End Using
                    ''End Using
                    Using xmlSW As New StreamWriter(nPath)
                        movieToSave.NfoPath = tPath
                        xmlSer.Serialize(xmlSW, movieToSave.Movie)
                    End Using
                End If
            Else
                Dim tmpName As String = Path.GetFileNameWithoutExtension(movieToSave.Filename)
                nPath = Path.Combine(Directory.GetParent(movieToSave.Filename).FullName, tmpName)

                If Master.eSettings.MovieNameNFO AndAlso (Not movieToSave.isSingle OrElse Not Master.eSettings.MovieNameMultiOnly) Then
                    If Directory.GetParent(movieToSave.Filename).Name.ToLower = "video_ts" Then
                        tPath = Path.Combine(Directory.GetParent(movieToSave.Filename).FullName, "video_ts.nfo")
                    Else
                        tPath = String.Concat(nPath, ".nfo")
                    End If

                    If Not Master.eSettings.OverwriteNfo Then
                        RenameNonConfNfo(tPath, False)
                    End If

                    If Not File.Exists(tPath) OrElse (Not CBool(File.GetAttributes(tPath) And FileAttributes.ReadOnly)) Then
                        ''Using memData As New IO.MemoryStream
                        ''    xmlSer.Serialize(memData, movieToSave.Movie)
                        ''    memData.Position = 0
                        ''    Using sr As New StreamReader(memData, Encoding.UTF8)
                        ''        Dim sData As String = sr.ReadToEnd
                        ''        sData = sData.Replace("<sets>" & Environment.NewLine, "")
                        ''        sData = sData.Replace("</sets>" & Environment.NewLine, "")
                        ''        IO.File.WriteAllText(tPath, sData)
                        ''    End Using
                        ''End Using
                        Using xmlSW As New StreamWriter(tPath)
                            movieToSave.NfoPath = tPath
                            xmlSer.Serialize(xmlSW, movieToSave.Movie)
                        End Using
                    End If
                End If

                If movieToSave.isSingle AndAlso Master.eSettings.MovieNFO Then
                    tPath = Path.Combine(Directory.GetParent(nPath).FullName, "movie.nfo")

                    If Not Master.eSettings.OverwriteNfo Then
                        RenameNonConfNfo(tPath, False)
                    End If

                    If Not File.Exists(tPath) OrElse (Not CBool(File.GetAttributes(tPath) And FileAttributes.ReadOnly)) Then
                        ''Using memData As New IO.MemoryStream
                        ''    xmlSer.Serialize(memData, movieToSave.Movie)
                        ''    memData.Position = 0
                        ''    Using sr As New StreamReader(memData, Encoding.UTF8)
                        ''        Dim sData As String = sr.ReadToEnd
                        ''        sData = sData.Replace("<sets>" & Environment.NewLine, "")
                        ''        sData = sData.Replace("</sets>" & Environment.NewLine, "")
                        ''        IO.File.WriteAllText(tPath, sData)
                        ''    End Using
                        ''End Using
                        Using xmlSW As New StreamWriter(tPath)
                            movieToSave.NfoPath = tPath
                            xmlSer.Serialize(xmlSW, movieToSave.Movie)
                        End Using
                    End If
                End If
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Shared Sub RenameNonConfNfo(ByVal sPath As String, ByVal isChecked As Boolean)
        'test if current nfo is non-conforming... rename per setting

        Try
            If isChecked OrElse Not IsConformingNfo(sPath) Then
                If isChecked OrElse File.Exists(sPath) Then
                    Dim i As Integer = 1
                    Dim strNewName As String = String.Concat(Master.RemoveExtFromPath(sPath), ".info")
                    'in case there is already a .info file
                    If File.Exists(strNewName) Then
                        Do
                            strNewName = String.Format("{0}({1}).info", Master.RemoveExtFromPath(sPath), i)
                            i += 1
                        Loop While File.Exists(strNewName)
                        strNewName = String.Format("{0}({1}).info", Master.RemoveExtFromPath(sPath), i)
                    End If
                    My.Computer.FileSystem.RenameFile(sPath, Path.GetFileName(strNewName))
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Public Shared Function IsConformingNfo(ByVal sPath As String) As Boolean
        Dim testSer As XmlSerializer = Nothing

        Try
            If File.Exists(sPath) AndAlso Not Master.eSettings.ValidExts.Contains(Path.GetExtension(sPath).ToLower) Then
                Using testSR As StreamReader = New StreamReader(sPath)
                    testSer = New XmlSerializer(GetType(Media.Movie))
                    Dim testMovie As Media.Movie = CType(testSer.Deserialize(testSR), Media.Movie)
                    testMovie = Nothing
                    testSer = Nothing
                End Using
                Return True
            Else
                Return False
            End If
        Catch
            If Not IsNothing(testSer) Then
                testSer = Nothing
            End If

            Return False
        End Try
    End Function
End Class
