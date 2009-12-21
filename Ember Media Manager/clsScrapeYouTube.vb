Imports System.Text.RegularExpressions

Namespace YouTube
    Public Class Scraper

        Friend WithEvents bwYT As New System.ComponentModel.BackgroundWorker

        Public Event VideoLinksRetrieved(ByVal bSuccess As Boolean)
        Public Event Exception(ByVal ex As Exception)
        Public Event ProgressUpdated(ByVal iPercent As Integer)



        Private _VideoLinks As VideoLinkItemCollection
        Public ReadOnly Property VideoLinks() As VideoLinkItemCollection
            Get
                If _VideoLinks Is Nothing Then
                    _VideoLinks = New VideoLinkItemCollection
                End If
                Return _VideoLinks
            End Get
        End Property

        Public Sub GetVideoLinksAsync(ByVal url As String)
            Try
                If Not bwYT.IsBusy Then
                    bwYT.WorkerReportsProgress = True
                    bwYT.WorkerSupportsCancellation = True
                    _VideoLinks = Nothing
                    bwYT.RunWorkerAsync(url)
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub


        Public Sub GetVideoLinks(ByVal url As String)
            Try
                _VideoLinks = ParseImdbForVideoLinks(url, False)

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub


        Private Function ParseImdbForVideoLinks(ByVal url As String, ByVal doProgress As Boolean) As VideoLinkItemCollection
            Dim DownloadLinks As New VideoLinkItemCollection
            Dim sHTTP As New HTTP

            Try
                If bwYT.CancellationPending Then Return DownloadLinks

                Dim Html As String = sHTTP.DownloadData(url)
                If Html.ToLower.Contains("page not found") Then
                    Html = String.Empty
                End If

                If String.IsNullOrEmpty(Html.Trim) Then Return DownloadLinks
                If bwYT.CancellationPending Then Return DownloadLinks


                Dim Args As Dictionary(Of String, String) = GetSWFArgs(Html)
                If Args.Count > 0 Then

                    Dim VideoTitle As String = GetVideoTitle(Html)
                    VideoTitle = Regex.Replace(VideoTitle, "['?\\:*<>]*", "")

                    If Args.ContainsKey("fmt_url_map") Then
                        Dim FormatMap As String = Args("fmt_url_map")


                        Dim Formats As String() = Web.HttpUtility.UrlDecode(FormatMap).Split(Convert.ToChar(","))
                        For Each fmt As String In Formats
                            Dim FormatElements As String() = fmt.Split(Convert.ToChar("|"))

                            Dim Link As New VideoLinkItem
                            Select Case FormatElements(0).Trim
                                Case "22"
                                    Link.URL = FormatElements(1) & "&title=" & Web.HttpUtility.UrlEncode(VideoTitle)
                                    Link.Description = "720p"
                                    Link.FormatQuality = 3
                                Case "37"
                                    Link.URL = FormatElements(1) & "&title=" & Web.HttpUtility.UrlEncode(VideoTitle)
                                    Link.Description = "1080p"
                                    Link.FormatQuality = 1
                            End Select

                            If bwYT.CancellationPending Then Return DownloadLinks

                            If Link.URL <> "" AndAlso sHTTP.IsValidURL(Link.URL) Then
                                DownloadLinks.Add(Link)
                            End If

                            If bwYT.CancellationPending Then Return DownloadLinks

                        Next

                        If Args.ContainsKey("video_id") AndAlso Args.ContainsKey("t") Then
                            'add standard link
                            Dim VideoId As String = Args("video_id")
                            Dim VideoHash As String = Args("t")
                            Dim StdLink As New VideoLinkItem
                            StdLink.URL = "http://www.youtube.com/get_video?fmt=18&video_id=" & VideoId & "&t=" & VideoHash
                            StdLink.Description = "Standard"
                            StdLink.FormatQuality = 100
                            DownloadLinks.Add(StdLink)
                        End If

                    End If
                End If

                Return DownloadLinks

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                Return New VideoLinkItemCollection
            Finally
                sHTTP = Nothing
            End Try


        End Function


        Private Function GetSWFArgs(ByVal HTML As String) As Dictionary(Of String, String)
            Dim result As New Dictionary(Of String, String)
            Dim args As String()
            Dim ArgsPattern As String = "'SWF_ARGS':\s*{([^}]*?)}"
            Dim KeyValuesPattern As String = """(.*?)"": ""(.*?)"""

            If Regex.IsMatch(HTML, ArgsPattern) Then
                args = Regex.Match(HTML, ArgsPattern).Groups(1).Value.ToString.Split(Convert.ToChar(","))
                For Each argString As String In args
                    If Regex.IsMatch(argString, KeyValuesPattern) Then
                        Dim itemMatch As Match = Regex.Match(argString, KeyValuesPattern)
                        result.Add(itemMatch.Groups(1).Value, itemMatch.Groups(2).Value)
                    End If
                Next
            End If

            Return result
        End Function

        Private Function GetVideoTitle(ByVal HTML As String) As String
            Dim result As String = ""
            Dim KeyPattern As String = "'VIDEO_TITLE':\s*'([^']*?)'"

            If Regex.IsMatch(HTML, KeyPattern) Then
                result = Regex.Match(HTML, KeyPattern).Groups(1).Value
            End If

            Return result
        End Function



        Public Sub CancelAsync()
            If bwYT.IsBusy Then bwYT.CancelAsync()

            Do While bwYT.IsBusy
                Application.DoEvents()
            Loop
        End Sub



        Private Sub bwYT_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwYT.DoWork
            Dim Url As String = DirectCast(e.Argument, String)

            Try
                e.Result = ParseImdbForVideoLinks(Url, True)
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Private Sub bwYT_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwYT.ProgressChanged
            RaiseEvent ProgressUpdated(e.ProgressPercentage)
        End Sub

        Private Sub bwYT_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwYT.RunWorkerCompleted

            Try
                If e.Cancelled Then
                    'user cancelled
                    RaiseEvent VideoLinksRetrieved(False)
                ElseIf e.Error IsNot Nothing Then
                    'exception occurred
                    RaiseEvent Exception(e.Error)
                Else
                    'all good
                    If e.Result IsNot Nothing Then
                        _VideoLinks = DirectCast(e.Result, VideoLinkItemCollection)
                        RaiseEvent VideoLinksRetrieved(True)
                    Else
                        RaiseEvent VideoLinksRetrieved(False)
                    End If
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub
    End Class

    Public Class VideoLinkItem

        Private _Description As String
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property


        Private _URL As String
        Public Property URL() As String
            Get
                Return _URL
            End Get
            Set(ByVal value As String)
                _URL = value
            End Set
        End Property


        Private _FormatQuality As Integer
        Friend Property FormatQuality() As Integer
            Get
                Return _FormatQuality
            End Get
            Set(ByVal value As Integer)
                _FormatQuality = value
            End Set
        End Property

    End Class

    Public Class VideoLinkItemCollection
        Inherits Generic.SortedList(Of Integer, VideoLinkItem)

        Public Shadows Sub Add(ByVal Link As VideoLinkItem)
            MyBase.Add(Link.FormatQuality, Link)
        End Sub

    End Class

End Namespace
