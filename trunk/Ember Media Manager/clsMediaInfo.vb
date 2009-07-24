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

Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Xml.Serialization
Imports System.Text
Imports System.Text.RegularExpressions

Public Class MediaInfo

    Public Enum StreamKind As UInteger
        General
        Visual
        Audio
        Text
    End Enum

    Public Enum InfoKind As UInteger
        Name
        Text
    End Enum

    Private Declare Unicode Function MediaInfo_New Lib "Bin\MediaInfo.DLL" () As IntPtr
    Private Declare Unicode Sub MediaInfo_Delete Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr)
    Private Declare Unicode Function MediaInfo_Open Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr, ByVal FileName As String) As UIntPtr
    Private Declare Unicode Sub MediaInfo_Close Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr)
    Private Declare Unicode Function MediaInfo_Get Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr, ByVal StreamKind As UIntPtr, ByVal StreamNumber As UIntPtr, ByVal Parameter As String, ByVal KindOfInfo As UIntPtr, ByVal KindOfSearch As UIntPtr) As IntPtr
    Private Declare Unicode Function MediaInfo_Count_Get Lib "Bin\MediaInfo.DLL" (ByVal Handle As IntPtr, ByVal StreamKind As UIntPtr, ByVal StreamNumber As IntPtr) As UIntPtr

    Private Handle As IntPtr

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub Open(ByVal FileName As String)
        MediaInfo_Open(Handle, FileName)
    End Sub

    Private Sub Close()
        MediaInfo_Close(Handle)
        MediaInfo_Delete(Handle)
        Handle = Nothing
    End Sub

    Private Function Get_(ByVal StreamKind As StreamKind, ByVal StreamNumber As Integer, ByVal Parameter As String, Optional ByVal KindOfInfo As InfoKind = InfoKind.Text, Optional ByVal KindOfSearch As InfoKind = InfoKind.Name) As String
        Return Marshal.PtrToStringUni(MediaInfo_Get(Handle, StreamKind, StreamNumber, Parameter, KindOfInfo, KindOfSearch))
    End Function

    Private Function Count_Get(ByVal StreamKind As StreamKind, Optional ByVal StreamNumber As UInteger = UInteger.MaxValue) As Integer
        If StreamNumber = UInteger.MaxValue Then
            Return MediaInfo_Count_Get(Handle, StreamKind, -1)
        Else
            Return MediaInfo_Count_Get(Handle, StreamKind, StreamNumber)
        End If
    End Function

    Public Sub GetMovieMIFromPath(ByRef fiInfo As Fileinfo, ByVal sPath As String)

        If Not String.IsNullOrEmpty(sPath) AndAlso File.Exists(sPath) Then
            Dim sExt As String = Path.GetExtension(sPath).ToLower
            Dim fiOut As New Fileinfo
            Dim miVideo As New Video
            Dim miAudio As New Audio
            Dim miSubtitle As New Subtitle
            Dim AudioStreams As Integer
            Dim SubtitleStreams As Integer
            Dim aLang As String = String.Empty
            Dim sLang As String = String.Empty
            Dim cDVD As New clsDVD

            Dim ifoVideo(2) As String
            Dim ifoAudio(2) As String

            If (sExt = ".ifo" OrElse sExt = ".vob" OrElse sExt = ".bup") AndAlso cDVD.fctOpenIFOFile(sPath) Then
                Try
                    ifoVideo = cDVD.GetIFOVideo
                    Dim vRes() As String = ifoVideo(1).Split(New Char() {"x"})
                    miVideo.Width = vRes(0)
                    miVideo.Height = vRes(1)
                    miVideo.Codec = ifoVideo(0)
                    miVideo.Duration = cDVD.GetProgramChainPlayBackTime(1)
                    miVideo.Aspect = ifoVideo(2)

                    With miVideo
                        If Not String.IsNullOrEmpty(.Codec) OrElse Not String.IsNullOrEmpty(.Duration) OrElse Not String.IsNullOrEmpty(.Aspect) OrElse _
                        Not String.IsNullOrEmpty(.Height) OrElse Not String.IsNullOrEmpty(.Width) Then
                            fiOut.StreamDetails.Video.Add(miVideo)
                        End If
                    End With

                    AudioStreams = cDVD.GetIFOAudioNumberOfTracks
                    For a As Integer = 1 To AudioStreams
                        miAudio = New MediaInfo.Audio
                        ifoAudio = cDVD.GetIFOAudio(a)
                        miAudio.Codec = ifoAudio(0)
                        miAudio.Channels = ifoAudio(2)
                        aLang = ifoAudio(1)
                        If Not String.IsNullOrEmpty(aLang) Then
                            miAudio.LongLanguage = aLang
                            miAudio.Language = ConvertL(miAudio.LongLanguage)
                        End If
                        With miAudio
                            If Not String.IsNullOrEmpty(.Codec) OrElse Not String.IsNullOrEmpty(.Channels) OrElse Not String.IsNullOrEmpty(.Language) Then
                                fiOut.StreamDetails.Audio.Add(miAudio)
                            End If
                        End With
                    Next

                    SubtitleStreams = cDVD.GetIFOSubPicNumberOf
                    For s As Integer = 1 To SubtitleStreams
                        miSubtitle = New MediaInfo.Subtitle
                        sLang = cDVD.GetIFOSubPic(s)
                        If Not String.IsNullOrEmpty(sLang) Then
                            miSubtitle.LongLanguage = sLang
                            miSubtitle.Language = ConvertL(miSubtitle.LongLanguage)
                            If Not String.IsNullOrEmpty(miSubtitle.Language) Then
                                fiOut.StreamDetails.Subtitle.Add(miSubtitle)
                            End If
                        End If
                    Next

                    cDVD.Close()
                    cDVD = Nothing

                    fiInfo = fiOut
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            ElseIf StringManip.IsStacked(Path.GetFileNameWithoutExtension(sPath), True) Then
                Try
                    Dim oFile As String = StringManip.CleanStackingMarkers(sPath, False)
                    Dim sFile As New ArrayList
                    Dim bIsVTS As Boolean = False
                    If sExt = ".ifo" OrElse sExt = ".bup" OrElse sExt = ".vob" Then
                        bIsVTS = True
                    End If

                    If bIsVTS Then
                        Try
                            sFile.AddRange(Directory.GetFiles(Directory.GetParent(sPath).FullName, "VTS*.VOB"))
                        Catch
                        End Try
                    Else
                        Try
                            sFile.AddRange(Directory.GetFiles(Directory.GetParent(sPath).FullName, StringManip.CleanStackingMarkers(Path.GetFileName(sPath), True)))
                        Catch
                        End Try
                    End If

                    Dim TotalDur As Integer = 0
                    Dim tInfo As New Fileinfo
                    Dim tVideo As New Video
                    Dim tAudio As New Audio

                    miVideo.Width = 0
                    miAudio.Channels = 0

                    For Each File As String In sFile
                        'make sure the file is actually part of the stack
                        'handles movie.cd1.ext, movie.cd2.ext and movie.extras.ext
                        'disregards movie.extras.ext in this case
                        If bIsVTS OrElse (oFile = StringManip.CleanStackingMarkers(File, False)) Then
                            tInfo = ScanMI(File)

                            tVideo = NFO.GetBestVideo(tInfo)
                            tAudio = NFO.GetBestAudio(tInfo)

                            If String.IsNullOrEmpty(miVideo.Codec) OrElse Not String.IsNullOrEmpty(tVideo.Codec) Then
                                If Not String.IsNullOrEmpty(tVideo.Width) AndAlso Convert.ToInt32(tVideo.Width) >= Convert.ToInt32(miVideo.Width) Then
                                    miVideo = tVideo
                                End If
                            End If

                            If String.IsNullOrEmpty(miAudio.Codec) OrElse Not String.IsNullOrEmpty(tAudio.Codec) Then
                                If Not String.IsNullOrEmpty(tAudio.Channels) AndAlso Convert.ToInt32(tAudio.Channels) >= Convert.ToInt32(miAudio.Channels) Then
                                    miAudio = tAudio
                                End If
                            End If

                            If Not String.IsNullOrEmpty(tVideo.Duration) Then TotalDur += Convert.ToInt32(DurationToMins(tVideo.Duration, False))

                            For Each sSub As Subtitle In tInfo.StreamDetails.Subtitle
                                If Not fiOut.StreamDetails.Subtitle.Contains(sSub) Then
                                    fiOut.StreamDetails.Subtitle.Add(sSub)
                                End If
                            Next
                        End If
                    Next

                    fiOut.StreamDetails.Video.Add(miVideo)
                    fiOut.StreamDetails.Audio.Add(miAudio)
                    fiOut.StreamDetails.Video(0).Duration = DurationToMins(TotalDur, True)

                    fiInfo = fiOut
                Catch ex As Exception
                    Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                End Try
            Else
                fiInfo = ScanMI(sPath)
            End If
        End If

    End Sub

    Private Function ScanMI(ByVal sPath As String) As Fileinfo
        Dim fiOut As New Fileinfo
        Try
            If Not String.IsNullOrEmpty(sPath) Then
                Dim miVideo As New Video
                Dim miAudio As New Audio
                Dim miSubtitle As New Subtitle
                Dim VideoStreams As Integer
                Dim AudioStreams As Integer
                Dim SubtitleStreams As Integer
                Dim aLang As String = String.Empty
                Dim sLang As String = String.Empty

                Me.Handle = MediaInfo_New()

                Me.Open(sPath)

                VideoStreams = Me.Count_Get(StreamKind.Visual)
                Dim vCodec As String = String.Empty
                For v As Integer = 0 To VideoStreams - 1
                    miVideo = New Video
                    miVideo.Width = Me.Get_(StreamKind.Visual, v, "Width")
                    miVideo.Height = Me.Get_(StreamKind.Visual, v, "Height")
                    miVideo.Codec = ConvertVFormat(Me.Get_(StreamKind.Visual, v, "CodecID/Hint"))
                    If String.IsNullOrEmpty(miVideo.Codec) OrElse IsNumeric(miVideo.Codec) Then
                        vCodec = ConvertVFormat(Me.Get_(StreamKind.Visual, v, "CodecID"))
                        If IsNumeric(vCodec) OrElse String.IsNullOrEmpty(vCodec) Then
                            miVideo.Codec = ConvertVFormat(Me.Get_(StreamKind.Visual, v, "Format"), Me.Get_(StreamKind.Visual, v, "Format_Version"))
                        Else
                            miVideo.Codec = vCodec
                        End If
                    End If

                    miVideo.Duration = Me.Get_(StreamKind.Visual, v, "Duration/String")
                    miVideo.Aspect = Me.Get_(StreamKind.Visual, v, "DisplayAspectRatio")
                    miVideo.Scantype = Me.Get_(StreamKind.Visual, v, "ScanType")
                    With miVideo
                        If Not String.IsNullOrEmpty(.Codec) OrElse Not String.IsNullOrEmpty(.Duration) OrElse Not String.IsNullOrEmpty(.Aspect) OrElse _
                        Not String.IsNullOrEmpty(.Height) OrElse Not String.IsNullOrEmpty(.Width) OrElse Not String.IsNullOrEmpty(.Scantype) Then
                            fiOut.StreamDetails.Video.Add(miVideo)
                        End If
                    End With
                Next

                AudioStreams = Me.Count_Get(StreamKind.Audio)
                Dim aCodec As String = String.Empty
                For a As Integer = 0 To AudioStreams - 1
                    miAudio = New Audio
                    miAudio.Codec = ConvertAFormat(Me.Get_(StreamKind.Audio, a, "CodecID/Hint"))
                    If String.IsNullOrEmpty(miAudio.Codec) OrElse IsNumeric(miAudio.Codec) Then
                        aCodec = ConvertAFormat(Me.Get_(StreamKind.Audio, a, "CodecID"))
                        miAudio.Codec = If(IsNumeric(aCodec) OrElse String.IsNullOrEmpty(aCodec), ConvertAFormat(Me.Get_(StreamKind.Audio, a, "Format")), aCodec)
                    End If
                    miAudio.Channels = Me.Get_(StreamKind.Audio, a, "Channel(s)")
                    aLang = Me.Get_(StreamKind.Audio, a, "Language/String")
                    If Not String.IsNullOrEmpty(aLang) Then
                        miAudio.LongLanguage = aLang
                        miAudio.Language = ConvertL(miAudio.LongLanguage)
                    End If
                    With miAudio
                        If Not String.IsNullOrEmpty(.Codec) OrElse Not String.IsNullOrEmpty(.Channels) OrElse Not String.IsNullOrEmpty(.Language) Then
                            fiOut.StreamDetails.Audio.Add(miAudio)
                        End If
                    End With
                Next

                SubtitleStreams = Me.Count_Get(StreamKind.Text)
                For s As Integer = 0 To SubtitleStreams - 1
                    miSubtitle = New MediaInfo.Subtitle
                    sLang = Me.Get_(StreamKind.Text, s, "Language/String")
                    If Not String.IsNullOrEmpty(sLang) Then
                        miSubtitle.LongLanguage = sLang
                        miSubtitle.Language = ConvertL(miSubtitle.LongLanguage)
                    End If
                    If Not String.IsNullOrEmpty(miSubtitle.Language) Then
                        fiOut.StreamDetails.Subtitle.Add(miSubtitle)
                    End If
                Next

                Me.Close()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        Return fiOut
    End Function

    Private Function DurationToMins(ByVal Duration As String, ByVal Reverse As Boolean) As String
        If Not String.IsNullOrEmpty(Duration) Then
            If Reverse Then
                Dim ts As New TimeSpan(0, Convert.ToInt32(Duration), 0)
                Return String.Format("{0}h {1}mn", ts.Hours, ts.Minutes)
            Else
                Dim sDuration As Match = Regex.Match(Duration, "(([0-9]+)h)?\s?(([0-9]+)mn)?")
                Dim sHour As Integer = If(Not String.IsNullOrEmpty(sDuration.Groups(2).Value), (Convert.ToInt32(sDuration.Groups(2).Value)), 0)
                Dim sMin As Integer = If(Not String.IsNullOrEmpty(sDuration.Groups(4).Value), (Convert.ToInt32(sDuration.Groups(4).Value)), 0)
                Return ((sHour * 60) + sMin)
            End If
        End If
        Return 0
    End Function

    Private Function ConvertVFormat(ByVal sFormat As String, Optional ByVal sModifier As String = "") As String
        If Not String.IsNullOrEmpty(sFormat) Then
            Dim tFormat As String = sFormat.ToLower
            Select Case True
                Case tFormat = "divx 5"
                    Return "dx50"
                Case tFormat.Contains("divx 3")
                    Return "div3"
                Case tFormat.Contains("lmp4"), tFormat.Contains("svq3"), tFormat.Contains("x264"), tFormat.Contains("avc"), tFormat.Contains("h264")
                    Return "h264"
                Case tFormat.Contains("flv"), tFormat.Contains("swf")
                    Return "flv"
                Case tFormat.Contains("3iv")
                    Return "3ivx"
                Case tFormat = "mpeg video"
                    If sModifier.ToLower = "version 2" Then
                        Return "mpeg2"
                    Else
                        Return "mpeg"
                    End If
                Case tFormat = "mpeg-4 video"
                    Return "mpeg4"
                Case Else
                    Return tFormat
            End Select
        Else
            Return String.Empty
        End If
    End Function

    Private Function ConvertAFormat(ByVal sFormat As String) As String
        If Not String.IsNullOrEmpty(sFormat) Then
            Select Case sFormat.ToLower
                Case "ac-3", "a_ac3"
                    Return "ac3"
                Case "wma2"
                    Return "wmav2"
                Case "dts", "a_dts"
                    Return "dca"
                Case Else
                    Return sFormat.ToLower
            End Select
        Else
            Return String.Empty
        End If
    End Function

    Private Function ConvertL(ByVal sLang As String) As String

        If XML.LanguageXML.Nodes.Count > 0 Then
            Dim xShortLang = From xLang In XML.LanguageXML.Descendants("Language") Where xLang.Element("Name").Value = sLang Select xLang.Element("Code").Value
            If xShortLang.Count > 0 Then
                Return xShortLang(0)
            Else
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If

    End Function

    <XmlRoot("fileinfo")> _
    Public Class Fileinfo

        Private _streamdetails As New StreamData

        <XmlElement("streamdetails")> _
        Property StreamDetails() As StreamData
            Get
                Return _streamdetails
            End Get
            Set(ByVal value As StreamData)
                _streamdetails = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property StreamDetailsSpecified() As Boolean
            Get
                Return (Not IsNothing(_streamdetails.Video) AndAlso _streamdetails.Video.Count > 0) OrElse _
                (Not IsNothing(_streamdetails.Audio) AndAlso _streamdetails.Audio.Count > 0) OrElse _
                (Not IsNothing(_streamdetails.Subtitle) AndAlso _streamdetails.Subtitle.Count > 0)
            End Get
        End Property

    End Class

    <XmlRoot("streamdata")> _
    Public Class StreamData

        Private _video As New List(Of Video)
        Private _audio As New List(Of Audio)
        Private _subtitle As New List(Of Subtitle)

        <XmlElement("video")> _
        Public Property Video() As List(Of Video)
            Get
                Return Me._video
            End Get
            Set(ByVal Value As List(Of Video))
                Me._video = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property VideoSpecified() As Boolean
            Get
                Return Me._video.Count > 0
            End Get
        End Property

        <XmlElement("audio")> _
         Public Property Audio() As List(Of Audio)
            Get
                Return Me._audio
            End Get
            Set(ByVal Value As List(Of Audio))
                Me._audio = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property AudioSpecified() As Boolean
            Get
                Return Me._audio.Count > 0
            End Get
        End Property

        <XmlElement("subtitle")> _
        Public Property Subtitle() As List(Of Subtitle)
            Get
                Return Me._subtitle
            End Get
            Set(ByVal Value As List(Of Subtitle))
                Me._subtitle = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property SubtitleSpecified() As Boolean
            Get
                Return Me._subtitle.Count > 0
            End Get
        End Property

    End Class

    Public Class Video

        Private _width As String = String.Empty
        Private _height As String = String.Empty
        Private _codec As String = String.Empty
        Private _duration As String = String.Empty
        Private _aspect As String = String.Empty
        Private _scantype As String = String.Empty

        <XmlElement("width")> _
        Public Property Width() As String
            Get
                Return Me._width
            End Get
            Set(ByVal Value As String)
                Me._width = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property WidthSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._width)
            End Get
        End Property

        <XmlElement("height")> _
        Public Property Height() As String
            Get
                Return Me._height
            End Get
            Set(ByVal Value As String)
                Me._height = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property HeightSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._height)
            End Get
        End Property

        <XmlElement("codec")> _
        Public Property Codec() As String
            Get
                Return Me._codec
            End Get
            Set(ByVal Value As String)
                Me._codec = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property CodecSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._codec)
            End Get
        End Property

        <XmlElement("duration")> _
        Public Property Duration() As String
            Get
                Return Me._duration
            End Get
            Set(ByVal Value As String)
                Me._duration = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property DurationSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._duration)
            End Get
        End Property

        <XmlElement("aspect")> _
        Public Property Aspect() As String
            Get
                Return Me._aspect
            End Get
            Set(ByVal Value As String)
                Me._aspect = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property AspectSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._aspect)
            End Get
        End Property

        <XmlElement("scantype")> _
        Public Property Scantype() As String
            Get
                Return Me._scantype
            End Get
            Set(ByVal Value As String)
                Me._scantype = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ScantypeSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._scantype)
            End Get
        End Property

    End Class

    Public Class Audio

        Private _codec As String = String.Empty
        Private _channels As String = String.Empty
        Private _language As String = String.Empty
        Private _longlanguage As String = String.Empty
        Private _haspreferred As Boolean = False

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._language)
            End Get
        End Property

        <XmlElement("longlanguage")> _
        Public Property LongLanguage() As String
            Get
                Return Me._longlanguage
            End Get
            Set(ByVal value As String)
                Me._longlanguage = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LongLanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._longlanguage)
            End Get
        End Property

        <XmlElement("codec")> _
        Public Property Codec() As String
            Get
                Return Me._codec
            End Get
            Set(ByVal Value As String)
                Me._codec = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property CodecSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._codec)
            End Get
        End Property

        <XmlElement("channels")> _
        Public Property Channels() As String
            Get
                Return Me._channels
            End Get
            Set(ByVal Value As String)
                Me._channels = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property ChannelsSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._channels)
            End Get
        End Property

        <XmlIgnore()> _
        Public Property HasPreferred() As Boolean
            Get
                Return Me._haspreferred
            End Get
            Set(ByVal value As Boolean)
                Me._haspreferred = value
            End Set
        End Property

    End Class

    Public Class Subtitle

        Private _language As String = String.Empty
        Private _longlanguage As String = String.Empty
        Private _haspreferred As Boolean = False

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._language)
            End Get
        End Property

        <XmlElement("longlanguage")> _
        Public Property LongLanguage() As String
            Get
                Return Me._longlanguage
            End Get
            Set(ByVal value As String)
                Me._longlanguage = value
            End Set
        End Property

        <XmlIgnore()> _
        Public ReadOnly Property LongLanguageSpecified() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Me._longlanguage)
            End Get
        End Property

        <XmlIgnore()> _
        Public Property HasPreferred() As Boolean
            Get
                Return Me._haspreferred
            End Get
            Set(ByVal value As Boolean)
                Me._haspreferred = value
            End Set
        End Property
    End Class

End Class
