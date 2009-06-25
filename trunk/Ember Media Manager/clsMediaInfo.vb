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
Imports System.Xml.Serialization
Imports System.Text
Imports System.Text.RegularExpressions 

Public Class MediaInfo

    Public Sub GetMovieMIFromPath(ByRef fiInfo As FileInfo, ByVal sPath As String)
        Dim miInfo As New Fileinfo
        Dim miVideo As New Video
        Dim miAudio As Audio
        Dim miSub As Subtitle
        Dim sbOutput As New StringBuilder
        Using ffmpeg As New Process()

            ffmpeg.StartInfo.FileName = String.Concat(Application.StartupPath, Path.DirectorySeparatorChar, "Bin", Path.DirectorySeparatorChar, "ffmpeg.exe")
            ffmpeg.StartInfo.Arguments = String.Format("-i ""{0}""", Master.currPath)
            ffmpeg.EnableRaisingEvents = False
            ffmpeg.StartInfo.UseShellExecute = False
            ffmpeg.StartInfo.CreateNoWindow = True
            ffmpeg.StartInfo.RedirectStandardOutput = True
            ffmpeg.StartInfo.RedirectStandardError = True
            ffmpeg.Start()
            Using d As StreamReader = ffmpeg.StandardError

                Do
                    sbOutput.AppendLine(d.ReadLine())

                Loop While Not d.EndOfStream
            End Using
            ffmpeg.WaitForExit()
            ffmpeg.Close()
        End Using

        If sbOutput.ToString.Contains("Duration: ") Then
            miVideo.Duration = Regex.Match(sbOutput.ToString, "Duration: (?<dur>.*?),").Groups("dur").ToString
        End If

        Dim vStream As Match = Regex.Match(sbOutput.ToString, "Stream #\d\.\d(\(.*?\))?: Video: (?<codec>.*?),(.*?)?(?<width>\d+?)x(?<height>\d+?)[,\s\b]", RegexOptions.IgnoreCase)
        '"Stream #(?<stream>\d?)\.(?<num>\d?)(\((?<lang>.*?)\))?: Video: (?<codec>.*?),(.*?)?(?<width>\d+?)x(?<height>\d+?)[,\s\b]"
        If vStream.Success Then
            miVideo.Codec = vStream.Groups("codec").ToString.Trim
            miVideo.Width = vStream.Groups("width").ToString.Trim
            miVideo.Height = vStream.Groups("height").ToString.Trim
            miVideo.Aspect = Convert.ToInt32(miVideo.Width) / Convert.ToInt32(miVideo.Height)
        End If
        miInfo.StreamDetails.Video = miVideo

        Dim aMatches As MatchCollection = Regex.Matches(sbOutput.ToString, "Stream #\d\.\d(\((?<lang>.*?)\))?: Audio: (?<codec>.*?),(.*?),(?<channels>.*?),", RegexOptions.IgnoreCase)
        For Each aStream As Match In aMatches
            miAudio = New Audio
            miAudio.Language = aStream.Groups("lang").ToString.Trim
            miAudio.Codec = aStream.Groups("codec").ToString.Trim
            Dim sChan As String = aStream.Groups("channels").ToString.Trim
            miAudio.Channels = If(sChan.ToLower = "stereo", "2", If(sChan.ToLower = "mono", "1", sChan))
            If Not String.IsNullOrEmpty(miAudio.Language) OrElse Not String.IsNullOrEmpty(miAudio.Codec) OrElse _
            Not String.IsNullOrEmpty(miAudio.Channels) Then
                miInfo.StreamDetails.Audio.Add(miAudio)
            End If
        Next

        Dim sMatches As MatchCollection = Regex.Matches(sbOutput.ToString, "", RegexOptions.IgnoreCase)
        For Each sStream As Match In sMatches
            miSub = New Subtitle
            miSub.Language = sStream.Groups("lang").ToString.Trim
            If Not String.IsNullOrEmpty(miSub.Language) Then
                miInfo.StreamDetails.Subtitle.Add(miSub)
            End If
        Next

        fiInfo = miInfo
    End Sub

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

    End Class

    <XmlRoot("streamdata")> _
    Public Class StreamData

        Private _video As New Video
        Private _audio As New List(Of Audio)
        Private _subtitle As New List(Of Subtitle)

        <XmlElement("video")> _
        Public Property Video() As Video
            Get
                If Not IsNothing(Me._video.Width) AndAlso Not IsNothing(Me._video.Height) AndAlso Not IsNothing(Me._video.Codec) AndAlso _
                Not IsNothing(Me._video.Aspect) AndAlso Not IsNothing(Me._video.Duration) Then
                    Return Me._video
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal Value As Video)
                Me._video = Value
            End Set
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

        <XmlElement("subtitle")> _
        Public Property Subtitle() As List(Of Subtitle)
            Get
                Return Me._subtitle
            End Get
            Set(ByVal Value As List(Of Subtitle))
                Me._subtitle = Value
            End Set
        End Property

    End Class

    Public Class Video

        Private _width As String
        Private _height As String
        Private _codec As String
        Private _duration As String
        Private _aspect As String

        <XmlElement("width")> _
        Public Property Width() As String
            Get
                Return Me._width
            End Get
            Set(ByVal Value As String)
                Me._width = Value
            End Set
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

        <XmlElement("codec")> _
        Public Property Codec() As String
            Get
                Return Me._codec
            End Get
            Set(ByVal Value As String)
                Me._codec = Value
            End Set
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

        <XmlElement("aspect")> _
        Public Property Aspect() As String
            Get
                Return Me._aspect
            End Get
            Set(ByVal Value As String)
                Me._aspect = Value
            End Set
        End Property

    End Class

    Public Class Audio

        Private _codec As String
        Private _channels As String
        Private _language As String

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
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

        <XmlElement("channels")> _
        Public Property Channels() As String
            Get
                Return Me._channels
            End Get
            Set(ByVal Value As String)
                Me._channels = Value
            End Set
        End Property

    End Class

    Public Class Subtitle

        Private _language As String

        <XmlElement("language")> _
        Public Property Language() As String
            Get
                Return Me._language
            End Get
            Set(ByVal Value As String)
                Me._language = Value
            End Set
        End Property

    End Class

End Class
