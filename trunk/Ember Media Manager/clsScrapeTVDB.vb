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

Namespace TVDB
    Public Class Scraper
        Private Const APIKey As String = "7B090234F418D074"
        Public Event SearchResultsDownloaded(ByVal mResults As List(Of TVSearchResults))
        Friend WithEvents bwTVDB As New System.ComponentModel.BackgroundWorker

        Private Structure Results
            Dim Result As Object
        End Structure

        Private Structure Arguments
            Dim Parameter As String
        End Structure

        Public Function GetLangs() As List(Of TVDBLanguage)
            Dim tvdbLangs As New List(Of TVDBLanguage)
            Dim cLang As TVDBLanguage
            Dim xmlTVDB As XDocument
            Dim sHTTP As New HTTP

            Dim apiXML As String = sHTTP.DownloadData(String.Format("http://{0}/api/{1}/languages.xml", Master.eSettings.TVDBMirror, APIKey))
            sHTTP = Nothing

            If Not String.IsNullOrEmpty(apiXML) Then
                Try
                    xmlTVDB = XDocument.Parse(apiXML)
                Catch
                    Return tvdbLangs
                End Try

                Dim xLangs = From xLanguages In xmlTVDB.Descendants("Language")

                For Each xL As XElement In xLangs
                    cLang = New TVDBLanguage
                    cLang.LongLang = xL.Element("name").Value
                    cLang.ShortLang = xL.Element("abbreviation").Value
                    tvdbLangs.Add(cLang)
                Next
            End If
            Return tvdbLangs
        End Function

        Public Sub GetSearchResultsAsync(ByVal sName As String)
            Try
                If Not bwTVDB.IsBusy Then
                    bwTVDB.WorkerReportsProgress = False
                    bwTVDB.WorkerSupportsCancellation = True
                    bwTVDB.RunWorkerAsync(New Arguments With {.Parameter = sName})
                End If
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Public Function SearchSeries(ByVal sName As String) As List(Of TVSearchResults)
            Dim tvdbResults As New List(Of TVSearchResults)
            Dim cResult As New TVSearchResults
            Dim xmlTVDB As XDocument
            Dim sHTTP As New HTTP
            Dim sLang As String = String.Empty

            Try
                Dim apiXML As String = sHTTP.DownloadData(String.Format("http://{0}/api/GetSeries.php?seriesname={1}&language={2}", Master.eSettings.TVDBMirror, sName, Master.eSettings.TVDBLanguage))
                sHTTP = Nothing

                If Not String.IsNullOrEmpty(apiXML) Then
                    Try
                        xmlTVDB = XDocument.Parse(apiXML)
                    Catch
                        Return tvdbResults
                    End Try

                    Dim xSer = From xSeries In xmlTVDB.Descendants("Series") Where xSeries.HasElements

                    For Each xS As XElement In xSer
                        cResult = New TVDB.TVSearchResults
                        cResult.ID = Convert.ToInt32(xS.Element("seriesid").Value)
                        cResult.Name = If(Not IsNothing(xS.Element("SeriesName")), xS.Element("SeriesName").Value, String.Empty)
                        If Not IsNothing(xS.Element("language")) Then
                            sLang = xS.Element("language").Value
                            cResult.Language = Master.eSettings.TVDBLanguages.SingleOrDefault(Function(s) s.ShortLang = sLang)
                        End If
                        cResult.Aired = If(Not IsNothing(xS.Element("FirstAired")), xS.Element("FirstAired").Value, String.Empty)
                        cResult.Overview = If(Not IsNothing(xS.Element("Overview")), xS.Element("Overview").Value, String.Empty)
                        cResult.Banner = If(Not IsNothing(xS.Element("banner")), xS.Element("banner").Value, String.Empty)
                        tvdbResults.Add(cResult)
                    Next
                End If

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

            Return tvdbResults
        End Function

        Public Sub CancelAsync()
            If bwTVDB.IsBusy Then bwTVDB.CancelAsync()

            While bwTVDB.IsBusy
                Application.DoEvents()
            End While
        End Sub

        Private Sub bwtvDB_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwTVDB.DoWork
            Dim Args As Arguments = DirectCast(e.Argument, Arguments)

            Try
                e.Result = New Results With {.Result = SearchSeries(Args.Parameter)}
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub

        Private Sub bwTVDB_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwTVDB.RunWorkerCompleted
            Dim Res As Results = DirectCast(e.Result, Results)

            Try
                RaiseEvent SearchResultsDownloaded(DirectCast(Res.Result, List(Of TVSearchResults)))
            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try

        End Sub
    End Class

    Public Class TVDBLanguage
        Private _longlang As String
        Private _shortlang As String

        Public Property LongLang() As String
            Get
                Return Me._longlang
            End Get
            Set(ByVal value As String)
                Me._longlang = value
            End Set
        End Property

        Public Property ShortLang() As String
            Get
                Return Me._shortlang
            End Get
            Set(ByVal value As String)
                Me._shortlang = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._longlang = String.Empty
            Me._shortlang = String.Empty
        End Sub
    End Class

    Public Class TVSearchResults
        Private _id As Integer
        Private _name As String
        Private _aired As String
        Private _language As TVDBLanguage
        Private _overview As String
        Private _banner As String

        Public Property ID() As Integer
            Get
                Return Me._id
            End Get
            Set(ByVal value As Integer)
                Me._id = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)
                Me._name = value
            End Set
        End Property

        Public Property Aired() As String
            Get
                Return Me._aired
            End Get
            Set(ByVal value As String)
                Me._aired = value
            End Set
        End Property

        Public Property Language() As TVDBLanguage
            Get
                Return Me._language
            End Get
            Set(ByVal value As TVDBLanguage)
                Me._language = value
            End Set
        End Property

        Public Property Overview() As String
            Get
                Return Me._overview
            End Get
            Set(ByVal value As String)
                Me._overview = value
            End Set
        End Property

        Public Property Banner() As String
            Get
                Return Me._banner
            End Get
            Set(ByVal value As String)
                Me._banner = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._id = 0
            Me._name = String.Empty
            Me._aired = String.Empty
            Me._language = New TVDBLanguage
            Me._overview = String.Empty
            Me._banner = String.Empty
        End Sub
    End Class
End Namespace

