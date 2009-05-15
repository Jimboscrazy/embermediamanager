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
Imports System.Xml

Namespace MPDB
    Public Class Scraper
        Friend WithEvents bwMPDB As New System.ComponentModel.BackgroundWorker

        Public Event PostersDownloaded(ByVal Posters As List(Of Media.Image))
        Public Event ProgressUpdated(ByVal iPercent As Integer)

        Private Structure Arguments
            Dim Parameter As String
        End Structure

        Private Structure Results
            Dim ResultList As List(Of Media.Image)
            Dim Result As Object
        End Structure

        Public Sub GetImagesAsync(ByVal imdbID As String)
            Try
                If Not bwMPDB.IsBusy Then
                    bwMPDB.WorkerReportsProgress = True
                    bwMPDB.RunWorkerAsync(New Arguments With {.Parameter = imdbID})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function GetMPDBPosters(ByVal imdbID As String) As List(Of Media.Image)
            Dim Html As String
            Dim alPosters As New List(Of Media.Image)
            Dim sUrl As String = String.Concat("http://www.movieposterdb.com/movie/", imdbID.Replace("tt", String.Empty))
            Dim Wc As New WebClient

            Try
                Wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate")
                Wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 3.5;)")

                Dim Ms As New MemoryStream(Wc.DownloadData(sUrl))

                If Wc.ResponseHeaders(HttpResponseHeader.ContentEncoding) = "gzip" Then
                    Html = New StreamReader(New GZipStream(Ms, CompressionMode.Decompress)).ReadToEnd
                Else
                    Html = New StreamReader(Ms).ReadToEnd
                End If

                Ms.Close()
                Ms = Nothing

                If bwMPDB.WorkerReportsProgress Then
                    bwMPDB.ReportProgress(1)
                End If

                Dim mcPoster As MatchCollection = Regex.Matches(Html, "http://www.movieposterdb.com/posters/[0-9_](.*?)/[0-9](.*?)/[0-9](.*?)/[a-z0-9_](.*?).jpg")

                Dim PosterURL As String = String.Empty

                For Each mPoster As Match In mcPoster
                    PosterURL = mPoster.Value.Remove(mPoster.Value.LastIndexOf("/") + 1, 1)
                    PosterURL = PosterURL.Insert(mPoster.Value.LastIndexOf("/") + 1, "l")
                    alPosters.Add(New Media.Image With {.Description = "poster", .URL = PosterURL})
                Next
                If bwMPDB.WorkerReportsProgress Then
                    bwMPDB.ReportProgress(3)
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return alPosters
        End Function

        Private Sub bwMPDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwMPDB.DoWork
            Dim Args As Arguments = e.Argument
            Try
                e.Result = GetMPDBPosters(Args.Parameter)
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                e.Result = Nothing
            End Try
        End Sub

        Private Sub bwMPDB_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwMPDB.ProgressChanged
            RaiseEvent ProgressUpdated(e.ProgressPercentage)
        End Sub

        Private Sub bwMPDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwMPDB.RunWorkerCompleted
            RaiseEvent PostersDownloaded(e.Result)
        End Sub
    End Class
End Namespace
