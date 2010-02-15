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
'
' Based on Ciné-Passion Code

Imports System.Windows.Forms
Imports System.IO
Imports System.IO.Compression
Imports System.Text.RegularExpressions
Imports System.Text
Imports System.Net


Namespace CinePassion

    Public Class Scraper

        Private SPEEDMovie As EmberAPI.MediaContainers.Movie
        Private AlloID As String = String.Empty
        Public Sub New(ByVal strID As String, ByRef mMovie As EmberAPI.MediaContainers.Movie)
            SPEEDMovie = mMovie
            AlloID = strID
            GetSPEEDDetails()
        End Sub

        Private Sub GetSPEEDDetails()

            Dim sURL As String = String.Empty

            If Not String.IsNullOrEmpty(AlloID) Then
                sURL = "http://passion-xbmc.org/scraper/index.php?id=" + AlloID + "&Version=0&Lang=fr"
            End If

            Try
                If Not String.IsNullOrEmpty(sURL) Then
                    Dim HTML As String
                    Dim sHTTPSpeed As New HTTP
                    HTML = sHTTPSpeed.DownloadData(sURL)
                    SPEEDMovie.Clear()

                    If Not String.IsNullOrEmpty(HTML) Then
                        HTML = Regex.Replace(HTML, "[\r\n]", "")

                        'title
                        If String.IsNullOrEmpty(SPEEDMovie.Title) OrElse Not Master.eSettings.LockTitle Then
                            If Regex.Match(HTML, "<title>(.*)</title>").Success Then
                                SPEEDMovie.Title = Regex.Match(HTML, "<title>(.*)</title>").Groups(1).ToString()
                            End If
                        End If

                        'originaltitle
                        If String.IsNullOrEmpty(SPEEDMovie.OriginalTitle) Then
                            If Regex.Match(HTML, "<originaltitle>(.*)</originaltitle>").Success Then
                                SPEEDMovie.OriginalTitle = Regex.Match(HTML, "<originaltitle>(.*)</originaltitle>").Groups(1).ToString()
                            End If
                        End If

                        'id
                        'If Regex.Match(HTML, "<id>(.*)</id>").Success Then
                        'SPEEDMovie.IMDBID = Regex.Match(HTML, "<id>(.*)</id>").Groups(1).ToString()
                        'End If
                        SPEEDMovie.IMDBID = AlloID

                        'year
                        If String.IsNullOrEmpty(SPEEDMovie.Year) Then
                            If Regex.Match(HTML, "<year>(.*)</year>").Success Then
                                SPEEDMovie.Year = Regex.Match(HTML, "<year>(.*)</year>").Groups(1).ToString()
                            End If
                        End If

                        'director
                        If String.IsNullOrEmpty(SPEEDMovie.Director) Then
                            If Regex.Match(HTML, "<director>(.*)</director>").Success Then
                                SPEEDMovie.Director = Regex.Match(HTML, "<director>(.*)</director>").Groups(1).ToString()
                            End If
                        End If

                        'runtime
                        If String.IsNullOrEmpty(SPEEDMovie.Runtime) Then
                            If Regex.Match(HTML, "<runtime>(.*)</runtime>").Success Then
                                SPEEDMovie.Runtime = Regex.Match(HTML, "<runtime>(.*)</runtime>").Groups(1).ToString()
                            End If
                        End If

                        'studio
                        If String.IsNullOrEmpty(SPEEDMovie.Studio) Then
                            If Regex.Match(HTML, "<studio>(.*)</studio>").Success Then
                                SPEEDMovie.Studio = Regex.Match(HTML, "<studio>(.*)</studio>").Groups(1).ToString()
                            End If
                        End If

                        'credits
                        If String.IsNullOrEmpty(SPEEDMovie.Credits) Then
                            If Regex.Match(HTML, "<credits>(.*)</credits>").Success Then
                                SPEEDMovie.Credits = Regex.Match(HTML, "<credits>(.*)</credits>").Groups(1).ToString()
                            End If
                        End If

                        'votes
                        If String.IsNullOrEmpty(SPEEDMovie.Votes) Then
                            If Regex.Match(HTML, "<votes>(.*)</votes>").Success Then
                                SPEEDMovie.Votes = Regex.Match(HTML, "<votes>(.*)</votes>").Groups(1).ToString()
                            End If
                        End If

                        'tagline Accroche
                        If String.IsNullOrEmpty(SPEEDMovie.Tagline) OrElse Not Master.eSettings.LockTagline Then
                            If Regex.Match(HTML, "<tagline>(.*)</tagline>").Success Then
                                SPEEDMovie.Tagline = Regex.Match(HTML, "<tagline>(.*)</tagline>").Groups(1).ToString()
                                If SPEEDMovie.Tagline.Contains("Voir la critique") Then
                                    SPEEDMovie.Tagline = String.Empty
                                End If
                            End If
                        End If

                        'mpaa
                        If String.IsNullOrEmpty(SPEEDMovie.MPAA) Then
                            If Regex.Match(HTML, "<mpaa>(.*)</mpaa>").Success Then
                                SPEEDMovie.MPAA = Regex.Match(HTML, "<mpaa>(.*)</mpaa>").Groups(1).ToString()
                            End If
                        End If

                        'rating
                        If String.IsNullOrEmpty(SPEEDMovie.Rating) OrElse Not Master.eSettings.LockRating Then
                            If Regex.Match(HTML, "<rating>(.*)</rating>").Success Then
                                SPEEDMovie.Rating = Regex.Match(HTML, "<rating>(.*)</rating>").Groups(1).ToString()
                                Try
                                    SPEEDMovie.Rating = (Convert.ToDouble(SPEEDMovie.Rating) * 2.5).ToString()
                                Catch ex As Exception
                                    SPEEDMovie.Rating = "0"
                                    ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                                End Try

                            End If
                        End If

                        'outline Résumé
                        If String.IsNullOrEmpty(SPEEDMovie.Outline) OrElse Not Master.eSettings.LockOutline Then
                            If Regex.Match(HTML, "<outline>(.*)</outline>", RegexOptions.Multiline).Success Then
                                SPEEDMovie.Outline = Regex.Match(HTML, "<outline>(.*)</outline>", RegexOptions.Multiline).Groups(1).ToString()
                            End If
                        End If

                        'plot Synopsis
                        If String.IsNullOrEmpty(SPEEDMovie.Plot) OrElse Not Master.eSettings.LockPlot Then
                            If Regex.Match(HTML, "<plot>(.*)</plot>").Success Then
                                SPEEDMovie.Plot = Regex.Match(HTML, "<plot>(.*)</plot>").Groups(1).ToString()
                            End If
                        End If

                        'genre
                        If String.IsNullOrEmpty(SPEEDMovie.Genre) OrElse Not Master.eSettings.LockGenre Then
                            If Regex.Match(HTML, "<genre>(.*)</genre>").Success Then
                                SPEEDMovie.Genre = Regex.Match(HTML, "<genre>(.*)</genre>").Groups(1).ToString()
                            End If
                        End If

                        'trailer
                        If String.IsNullOrEmpty(SPEEDMovie.Trailer) OrElse Not Master.eSettings.LockTrailer Then
                            If Regex.Match(HTML, "<trailer>(.*)</trailer>").Success Then
                                SPEEDMovie.Trailer = Regex.Match(HTML, "<trailer>(.*)</trailer>").Groups(1).ToString()
                            End If
                        End If

                        'thumbs
                        If Regex.Match(HTML, "<thumbs>(.*)</thumbs>").Success Then

                            Dim _Affiches As String = Regex.Match(HTML, "<thumbs>(.*?)</thumbs>").Groups(1).ToString()
                            Dim myMatchesThumb As MatchCollection
                            myMatchesThumb = Regex.Matches(_Affiches, "<thumb>(.*?)</thumb>")
                            For Each _Match As Match In myMatchesThumb
                                If Regex.Match(_Match.ToString, "<thumb>(.*?)</thumb>").Success Then
                                    SPEEDMovie.Thumb.Add(_Match.Groups(1).ToString())
                                End If

                            Next
                        End If

                        'fanarts
                        If Regex.Match(HTML, "<fanart>(.*)</fanart>").Success Then

                            Dim _Affiches As String = Regex.Match(HTML, "<fanart>(.*?)</fanart>").Groups(1).ToString()
                            Dim myMatchesThumb As MatchCollection
                            myMatchesThumb = Regex.Matches(_Affiches, "<thumb>(.*?)</thumb>")
                            For Each _Match As Match In myMatchesThumb
                                If Regex.Match(_Match.ToString, "<thumb>(.*?)</thumb>").Success Then
                                    Dim _Th As EmberAPI.MediaContainers.Thumb = New EmberAPI.MediaContainers.Thumb
                                    _Th.Preview = _Match.Groups(1).ToString()
                                    _Th.Text = _Match.Groups(1).ToString()
                                    SPEEDMovie.Fanart.Thumb.Add(_Th)
                                End If

                            Next
                        End If

                        'acteur
                        If Regex.Match(HTML, "<actor>(.*)</actor>").Success Then
                            Dim myMatchesActeurs As MatchCollection
                            myMatchesActeurs = Regex.Matches(HTML, "<actor>(.*?)</actor>")
                            For Each _Match As Match In myMatchesActeurs
                                Dim _act As EmberAPI.MediaContainers.Person = New EmberAPI.MediaContainers.Person()

                                If Regex.Match(_Match.ToString, "<name>(.*?)</name>").Success Then
                                    _act.Name = Regex.Match(_Match.ToString, "<name>(.*)</name>").Groups(1).ToString()
                                End If
                                If Regex.Match(_Match.ToString, "<role>(.*?)</role>").Success Then
                                    _act.Role = Regex.Match(_Match.ToString, "<role>(.*)</role>").Groups(1).ToString()
                                End If
                                If Regex.Match(_Match.ToString, "<thumb>(.*?)</thumb>").Success Then
                                    _act.Thumb = Regex.Match(_Match.ToString, "<thumb>(.*)</thumb>").Groups(1).ToString()
                                End If
                                SPEEDMovie.Actors.Add(_act)
                            Next
                        End If

                    End If
                End If
            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Public Function DownloadDataISO(ByVal URL As String) As String
            Dim sResponse As String = String.Empty

            Try

                Dim wrRequest As WebRequest = HttpWebRequest.Create(URL)
                wrRequest.Method = "GET"
                wrRequest.Timeout = 10000
                Dim wrResponse As WebResponse = wrRequest.GetResponse
                Using Ms As Stream = wrResponse.GetResponseStream
                    sResponse = New StreamReader(Ms, Encoding.GetEncoding("ISO-8859-1")).ReadToEnd
                End Using
                wrResponse.Close()
                wrResponse = Nothing
                wrRequest = Nothing
            Catch ex As Exception
            End Try
            Return sResponse

        End Function


    End Class

    Public Class ScraperImages
        Friend WithEvents bwSPEED As New System.ComponentModel.BackgroundWorker

        Public Event PostersDownloaded(ByVal Posters As List(Of EmberAPI.MediaContainers.Image))
        Public Event ProgressUpdated(ByVal iPercent As Integer)

        Private Structure Arguments
            Dim sType As String
            Dim Parameter As EmberAPI.MediaContainers.Movie
        End Structure

        Private Structure Results
            Dim ResultList As List(Of EmberAPI.MediaContainers.Image)
            Dim Result As Object
        End Structure

        Public Sub Cancel()
            If Me.bwSPEED.IsBusy Then Me.bwSPEED.CancelAsync()

            Do While Me.bwSPEED.IsBusy
                Application.DoEvents()
            Loop

        End Sub

        Public Sub GetImagesAsync(ByVal _Movie As EmberAPI.MediaContainers.Movie, ByVal _type As String)
            Try
                If Not Me.bwSPEED.IsBusy Then
                    Me.bwSPEED.WorkerSupportsCancellation = True
                    Me.bwSPEED.WorkerReportsProgress = True
                    Me.bwSPEED.RunWorkerAsync(New Arguments With {.Parameter = _Movie, .sType = _type})
                End If
            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function GetSPEEDPosters(ByVal _Movie As EmberAPI.MediaContainers.Movie, ByVal _Type As String) As List(Of EmberAPI.MediaContainers.Image)
            Dim alPosters As New List(Of EmberAPI.MediaContainers.Image)

            If Me.bwSPEED.CancellationPending Then Return Nothing
            If bwSPEED.WorkerReportsProgress Then
                bwSPEED.ReportProgress(1)
            End If

            Try
                'Poster
                If _Type = "poster" Then
                    For Each mPoster As String In _Movie.Thumb

                        alPosters.Add(New EmberAPI.MediaContainers.Image With {.Description = "poster", .URL = mPoster})
                    Next
                End If
                'Fanart
                If _Type = "backdrop" Then
                    For Each mPoster As EmberAPI.MediaContainers.Thumb In _Movie.Fanart.Thumb

                        alPosters.Add(New EmberAPI.MediaContainers.Image With {.Description = "original", .URL = mPoster.Text})
                    Next
                End If


                If bwSPEED.WorkerReportsProgress Then
                    bwSPEED.ReportProgress(3)
                End If

            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return alPosters
        End Function

        Private Sub bwSPEED_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwSPEED.DoWork
            Dim Args As Arguments = DirectCast(e.Argument, Arguments)
            Try
                e.Result = GetSPEEDPosters(Args.Parameter, Args.sType)
            Catch ex As Exception
                ErrorLogger.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                e.Result = Nothing
            End Try
        End Sub

        Private Sub bwSPEED_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwSPEED.ProgressChanged
            If Not bwSPEED.CancellationPending Then
                RaiseEvent ProgressUpdated(e.ProgressPercentage)
            End If
        End Sub

        Private Sub bwSPEED_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwSPEED.RunWorkerCompleted
            If Not IsNothing(e.Result) Then
                RaiseEvent PostersDownloaded(DirectCast(e.Result, List(Of EmberAPI.MediaContainers.Image)))
            End If
        End Sub
    End Class
End Namespace
