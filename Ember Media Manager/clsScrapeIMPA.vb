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
Imports System.Text
Imports System.IO.Compression

Namespace IMPA
    Public Class Scraper
        Friend WithEvents bwIMPA As New System.ComponentModel.BackgroundWorker

        Public Event PostersDownloaded(ByVal Posters As List(Of Media.Image))
        Public Event ProgressUpdated(ByVal iPercent As Integer)

        Private Structure Arguments
            Dim sType As String
            Dim Parameter As String
        End Structure

        Private Structure Results
            Dim ResultList As List(Of Media.Image)
            Dim Result As Object
        End Structure

        Public Sub Cancel()
            If Me.bwIMPA.IsBusy Then Me.bwIMPA.CancelAsync()

            Do While Me.bwIMPA.IsBusy
                Application.DoEvents()
            Loop

        End Sub

        Private Function GetLink(ByVal IMDBID As String) As String

            Try

                Dim sHTTP As New HTTP(String.Concat("http://", Master.eSettings.IMDBURL, "/title/tt", IMDBID, "/posters"))
                Dim HTML As String = sHTTP.Response
                sHTTP = Nothing

                Dim mcIMPA As MatchCollection = Regex.Matches(Html, "http://([^""]*)impawards.com/([^""]*)")
                If mcIMPA.Count > 0 Then
                    'just use the first one if more are found
                    Return mcIMPA(0).Value.ToString
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                Return String.Empty
            End Try
        End Function

        Public Sub GetImagesAsync(ByVal sURL As String)
            Try
                If Not bwIMPA.IsBusy Then
                    bwIMPA.WorkerSupportsCancellation = True
                    bwIMPA.WorkerReportsProgress = True
                    bwIMPA.RunWorkerAsync(New Arguments With {.Parameter = sURL})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function GetIMPAPosters(ByVal imdbID As String) As List(Of Media.Image)
            Dim alPoster As New List(Of Media.Image)

            Try
                If bwIMPA.CancellationPending Then Return Nothing
                Dim sURL As String = GetLink(imdbID)

                If Not String.IsNullOrEmpty(sURL) Then

                    Dim sHTTP As New HTTP(sURL)
                    Dim HTML As String = sHTTP.Response
                    sHTTP = Nothing

                    If bwIMPA.CancellationPending Then Return Nothing

                    Dim mcPoster As MatchCollection = Regex.Matches(Html, "(thumbs/imp_([^>]*ver[^>]*.jpg))|(thumbs/imp_([^>]*.jpg))")

                    Dim PosterURL As String

                    For Each mPoster As Match In mcPoster
                        If bwIMPA.CancellationPending Then Return Nothing
                        PosterURL = Strings.Replace(String.Format("{0}/{1}", sURL.Substring(0, sURL.LastIndexOf("/")), mPoster.Value.ToString()).Replace("thumbs", "posters"), "imp_", String.Empty)

                        alPoster.Add(New Media.Image With {.Description = "poster", .URL = PosterURL})

                        PosterURL = PosterURL.Insert(PosterURL.LastIndexOf("."), "_xlg")
                        alPoster.Add(New Media.Image With {.Description = "original", .URL = PosterURL})
                    Next
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return alPoster
        End Function

        Private Sub bwIMPA_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwIMPA.DoWork
            Dim Args As Arguments = e.Argument
            Try
                e.Result = GetIMPAPosters(Args.Parameter)
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                e.Result = Nothing
            End Try
        End Sub

        Private Sub bwIMPA_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwIMPA.ProgressChanged
            If Not bwIMPA.CancellationPending Then
                RaiseEvent ProgressUpdated(e.ProgressPercentage)
            End If
        End Sub

        Private Sub bwIMPA_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwIMPA.RunWorkerCompleted
            If Not IsNothing(e.Result) Then
                RaiseEvent PostersDownloaded(e.Result)
            End If
        End Sub

    End Class
End Namespace
