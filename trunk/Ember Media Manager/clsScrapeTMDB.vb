﻿' ################################################################################
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

        Public Sub Cancel()
            If Me.bwTMDB.IsBusy Then Me.bwTMDB.CancelAsync()

            Do While Me.bwTMDB.IsBusy
                Application.DoEvents()
            Loop
        End Sub

        Public Function GetTrailers(ByVal imdbID As String) As String
            Dim xmlTMDB As XDocument
            Dim sHTTP As New HTTP

            If Me.bwTMDB.CancellationPending Then Return Nothing
            Try
                Dim ApiXML As String = sHTTP.DownloadData(String.Format("http://api.themoviedb.org/2.1/Movie.imdbLookup/en/xml/{0}/tt{1}", APIKey, imdbID))
                sHTTP = Nothing

                If Not String.IsNullOrEmpty(ApiXML) Then
                    Try
                        xmlTMDB = XDocument.Parse(ApiXML)
                    Catch
                        Return String.Empty
                    End Try

                    If bwTMDB.WorkerReportsProgress Then
                        bwTMDB.ReportProgress(1)
                    End If
                    If Me.bwTMDB.CancellationPending Then Return Nothing

                    Dim tmdbNode = From xNode In xmlTMDB.Elements

                    If tmdbNode.Count > 0 Then
                        If Not tmdbNode(0).Value = "Your query didn't return any results." Then
                            Dim movieID As String = xmlTMDB...<OpenSearchDescription>...<movies>...<movie>...<id>.Value

                            sHTTP = New HTTP
                            ApiXML = sHTTP.DownloadData(String.Format("http://api.themoviedb.org/2.1/Movie.getInfo/en/xml/{0}/{1}", APIKey, movieID))
                            sHTTP = Nothing

                            If Not String.IsNullOrEmpty(ApiXML) Then

                                Try
                                    xmlTMDB = XDocument.Parse(ApiXML)
                                Catch
                                    Return String.Empty
                                End Try

                                If bwTMDB.WorkerReportsProgress Then
                                    bwTMDB.ReportProgress(2)
                                End If

                                If Me.bwTMDB.CancellationPending Then Return Nothing

                                Dim Trailers = From tNode In xmlTMDB...<OpenSearchDescription>...<movies>...<movie> Select tNode.<trailer>
                                If Trailers.Count > 0 AndAlso Not String.IsNullOrEmpty(Trailers(0).Value) Then
                                    If InStr(Trailers(0).Value.ToLower, "youtube.com") <> -1 Then
                                        Return Trailers(0).Value
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                If bwTMDB.WorkerReportsProgress Then
                    bwTMDB.ReportProgress(3)
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return String.Empty

        End Function

        Public Sub GetImagesAsync(ByVal imdbID As String, ByVal sType As String)
            Try
                If Not Me.bwTMDB.IsBusy Then
                    Me.bwTMDB.WorkerSupportsCancellation = True
                    Me.bwTMDB.WorkerReportsProgress = True
                    Me.bwTMDB.RunWorkerAsync(New Arguments With {.Parameter = imdbID, .sType = sType})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function GetTMDBImages(ByVal imdbID As String, ByVal sType As String) As List(Of Media.Image)
            Dim alPosters As New List(Of Media.Image)
            Dim xmlTMDB As XDocument
            Dim sHTTP As New HTTP

            If Me.bwTMDB.CancellationPending Then Return Nothing
            Try
                Dim ApiXML As String = sHTTP.DownloadData(String.Format("http://api.themoviedb.org/2.1/Movie.getImages/en/xml/{0}/tt{1}", APIKey, imdbID))

                If Not String.IsNullOrEmpty(ApiXML) Then
                    Try
                        xmlTMDB = XDocument.Parse(ApiXML)
                    Catch
                        Return alPosters
                    End Try

                    If bwTMDB.WorkerReportsProgress Then
                        bwTMDB.ReportProgress(1)
                    End If

                    If Me.bwTMDB.CancellationPending Then Return Nothing

                    If Not xmlTMDB...<OpenSearchDescription>...<movies>.Value = "Nothing found." Then
                        If sType = "poster" Then
                            Dim tmdbImages = From iNode In xmlTMDB...<OpenSearchDescription>...<movies>...<movie>...<images>...<poster>.Elements Select iNode
                            If tmdbImages.Count > 0 Then
                                For Each tmdbI As XElement In tmdbImages
                                    If Me.bwTMDB.CancellationPending Then Return Nothing
                                    Dim tmpPoster As New Media.Image With {.URL = tmdbI.@url, .Description = tmdbI.@size}
                                    alPosters.Add(tmpPoster)
                                Next
                            End If
                        ElseIf sType = "backdrop" Then
                            Dim tmdbImages = From iNode In xmlTMDB...<OpenSearchDescription>...<movies>...<movie>...<images>...<backdrop>.Elements Select iNode
                            If tmdbImages.Count > 0 Then
                                For Each tmdbI As XElement In tmdbImages
                                    If Me.bwTMDB.CancellationPending Then Return Nothing
                                    If sType = "backdrop" AndAlso Master.eSettings.FanartPrefSizeOnly Then
                                        Select Case Master.eSettings.PreferredFanartSize
                                            Case Master.FanartSize.Lrg
                                                If Not tmdbI.@size.ToLower = "original" Then Continue For
                                            Case Master.FanartSize.Mid
                                                If Not tmdbI.@size.ToLower = "mid" Then Continue For
                                            Case Master.FanartSize.Small
                                                If Not tmdbI.@size.ToLower = "thumb" Then Continue For
                                        End Select
                                    End If
                                    Dim tmpPoster As New Media.Image With {.URL = tmdbI.@url, .Description = tmdbI.@size}
                                    alPosters.Add(tmpPoster)
                                Next
                            End If
                        End If
                    End If
                End If

                If bwTMDB.WorkerReportsProgress Then
                    bwTMDB.ReportProgress(2)
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            sHTTP = Nothing

            Return alPosters
        End Function

        Private Sub bwTMDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwTMDB.DoWork
            Dim Args As Arguments = DirectCast(e.Argument, Arguments)
            Try
                e.Result = GetTMDBImages(Args.Parameter, Args.sType)
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                e.Result = Nothing
            End Try
        End Sub

        Private Sub bwTMDB_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwTMDB.ProgressChanged
            If Not Me.bwTMDB.CancellationPending Then
                RaiseEvent ProgressUpdated(e.ProgressPercentage)
            End If
        End Sub

        Private Sub bwTMDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwTMDB.RunWorkerCompleted
            If Not IsNothing(e.Result) Then
                RaiseEvent PostersDownloaded(DirectCast(e.Result, List(Of Media.Image)))
            End If
        End Sub
    End Class
End Namespace
