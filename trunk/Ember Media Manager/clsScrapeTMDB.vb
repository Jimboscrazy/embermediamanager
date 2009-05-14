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

Namespace TMDB
    Public Class Scraper
        Private Const APIKey As String = "6f96ee0ee3e734bcf5924584d0948020"

        Friend WithEvents bwTMDB As New System.ComponentModel.BackgroundWorker

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

        Public Sub GetImagesAsync(ByVal imdbID As String, ByVal sType As String)
            Try
                If Not bwTMDB.IsBusy Then
                    bwTMDB.WorkerReportsProgress = True
                    bwTMDB.RunWorkerAsync(New Arguments With {.Parameter = imdbID, .sType = sType})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function GetTMDBImages(ByVal imdbID As String, ByVal sType As String) As List(Of Media.Image)
            Dim alPosters As New List(Of Media.Image)
            Dim xmlTMDB As New XmlDocument
            Dim Url As String = String.Format("{0}{1}&api_key={2}", "http://api.themoviedb.org/2.0/Movie.imdbLookup?imdb_id=", imdbID, APIKey)
            Dim ApiXML As String
            Dim Wc As New WebClient

            Try
                Wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate")
                Wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 3.5;)")

                Dim Ms As New MemoryStream(Wc.DownloadData(Url))

                If Wc.ResponseHeaders(HttpResponseHeader.ContentEncoding) = "gzip" Then
                    ApiXML = New StreamReader(New GZipStream(Ms, CompressionMode.Decompress)).ReadToEnd
                Else
                    ApiXML = New StreamReader(Ms).ReadToEnd
                End If

                Ms.Close()
                Ms = Nothing

                Try
                    xmlTMDB.LoadXml(ApiXML)
                Catch
                    Return alPosters
                End Try

                If bwTMDB.WorkerReportsProgress Then
                    bwTMDB.ReportProgress(1)
                End If

                Dim tmdbNode As XmlNodeList = xmlTMDB.SelectNodes("//results/moviematches/movie")

                If Not tmdbNode(0).InnerText = "Your query didn't return any results." Then
                    Dim movieID As String = tmdbNode(0).ChildNodes(5).InnerText

                    xmlTMDB.Load(String.Format("{0}{1}&api_key={2}", "http://api.themoviedb.org/2.0/Movie.getInfo?id=", movieID, APIKey))

                    If bwTMDB.WorkerReportsProgress Then
                        bwTMDB.ReportProgress(2)
                    End If

                    Dim resultsNode As XmlNodeList = xmlTMDB.SelectNodes("//results/moviematches/movie")

                    Dim xmlPosters As XmlNode = resultsNode(0)

                    For i As Integer = 17 To (xmlPosters.ChildNodes.Count - 3)
                        If xmlPosters.ChildNodes(i).Name = sType Then
                            Dim tmpPoster As New Media.Image With {.URL = xmlPosters.ChildNodes(i).InnerText, .Description = xmlPosters.ChildNodes(i).Attributes(0).InnerText}
                            alPosters.Add(tmpPoster)
                        End If
                    Next
                End If
                If bwTMDB.WorkerReportsProgress Then
                    bwTMDB.ReportProgress(3)
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return alPosters
        End Function

        Private Sub bwTMDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwTMDB.DoWork
            Dim Args As Arguments = e.Argument
            Try
                e.Result = GetTMDBImages(Args.Parameter, Args.sType)
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                e.Result = Nothing
            End Try
        End Sub

        Private Sub bwTMDB_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwTMDB.ProgressChanged
            RaiseEvent ProgressUpdated(e.ProgressPercentage)
        End Sub

        Private Sub bwTMDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwTMDB.RunWorkerCompleted
            RaiseEvent PostersDownloaded(e.Result)
        End Sub
    End Class
End Namespace
