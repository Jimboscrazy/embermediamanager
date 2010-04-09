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
'Originally created by Lawrence "nicezia" Winston (http://sourceforge.net/projects/scraperxml/)
'Converted to VB.NET and modified for use with Ember Media Manager

Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Xml.Linq

Imports EmberScraperModule.XMLScraper.ScraperLib
Imports EmberScraperModule.XMLScraper.Utilities

Namespace XMLScraper
    Namespace MediaTags

        Public Class MovieTag
            Inherits MediaTag

#Region "Constructors"

            Public Sub New()
                Clear()
            End Sub

            Public Sub New(ByVal element As XElement)
                Me.New()
                Deserialize(element)
            End Sub

            Public Sub New(ByVal other As MovieTag)
                Me.New()
                CopyFrom(other)
            End Sub

#End Region 'Constructors

#Region "Properties"

            Public Property Actors() As List(Of PersonTag)
                Get
                    Return MyBase._People
                End Get
                Set(ByVal value As List(Of PersonTag))
                    MyBase._People = value
                End Set
            End Property

            Public Property Certifications() As List(Of String)
                Get
                    Return MyBase.StringLists("certification")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("certification") = value
                End Set
            End Property

            Public Property Directors() As List(Of String)
                Get
                    Return MyBase.StringLists("director")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("director") = value
                End Set
            End Property

            Public Property Fanart() As Fanart
                Get
                    Return MyBase._Fanart
                End Get
                Set(ByVal value As Fanart)
                    MyBase._Fanart = value
                End Set
            End Property

            Public Property Genres() As List(Of String)
                Get
                    Return MyBase.StringLists("genre")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("genre") = value
                End Set
            End Property

            Public Property ID() As String
                Get
                    Return MyBase.UserProperties("id")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("id") = value
                End Set
            End Property

            Public Property LastPlayed() As String
                Get
                    Return UserProperties("lastplayed")
                End Get
                Set(ByVal value As String)
                    UserProperties("lastplayed") = value
                End Set
            End Property

            Public Property MPAA() As String
                Get
                    Return MyBase.UserProperties("mpaa")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("mpaa") = value
                End Set
            End Property

            Public Property Outline() As String
                Get
                    Return MyBase.UserProperties("outline")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("outline") = value
                End Set
            End Property

            Public Property PlayCount() As Integer
                Get
                    Return Convert.ToInt32(UserProperties("playcount"))
                End Get
                Set(ByVal value As Integer)
                    UserProperties("playcount") = value.ToString()
                End Set
            End Property

            Public Property Plot() As String
                Get
                    Return MyBase.UserProperties("plot")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("plot") = value
                End Set
            End Property

            Public Property Premiered() As String
                Get
                    Return MyBase.UserProperties("premiered")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("premiered") = value
                End Set
            End Property

            Public Property Rating() As Single
                Get
                    Return NumUtils.ConvertToSingle(MyBase.UserProperties("rating"))
                End Get
                Set(ByVal value As Single)
                    MyBase.UserProperties("rating") = value.ToString("#0.0")
                End Set
            End Property

            Public Property Runtime() As String
                Get
                    Return MyBase.UserProperties("runtime")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("runtime") = value
                End Set
            End Property

            Public Property Sets() As List(Of String)
                Get
                    Return MyBase.StringLists("set")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("set") = value
                End Set
            End Property

            Public Property Studio() As String
                Get
                    Return MyBase.UserProperties("studio")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("studio") = value
                End Set
            End Property

            Public Property Tagline() As String
                Get
                    Return MyBase.UserProperties("tagline")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("tagline") = value
                End Set
            End Property

            Public Overloads Overrides ReadOnly Property TagType() As MediaType
                Get
                    Return MediaType.movie
                End Get
            End Property

            Public Property Thumbs() As List(Of Thumbnail)
                Get
                    Return MyBase._Thumbs
                End Get
                Set(ByVal value As List(Of Thumbnail))
                    MyBase._Thumbs = value
                End Set
            End Property

            Public Shadows Property Title() As String
                Get
                    Return MyBase.UserProperties("title")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("title") = value
                End Set
            End Property

            Public Property Top250() As Integer
                Get
                    Return Convert.ToInt32(MyBase.UserProperties("top250"))
                End Get
                Set(ByVal value As Integer)
                    MyBase.UserProperties("top250") = value.ToString()
                End Set
            End Property

            Public Property Trailers() As List(Of String)
                Get
                    Return MyBase.StringLists("trailer")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("trailer") = value
                End Set
            End Property

            Public Property Votes() As Integer
                Get
                    Return Convert.ToInt32(MyBase.UserProperties("votes"))
                End Get
                Set(ByVal value As Integer)
                    MyBase.UserProperties("votes") = value.ToString()
                End Set
            End Property

            Public Property Writers() As List(Of String)
                Get
                    Return MyBase.StringLists("credits")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("credits") = value
                End Set
            End Property

            Public Property Year() As Integer
                Get
                    Return Convert.ToInt32(MyBase.UserProperties("year"))
                End Get
                Set(ByVal value As Integer)
                    MyBase.UserProperties("year") = value.ToString()
                End Set
            End Property

#End Region 'Properties

#Region "Methods"

            Public Overloads Overrides Function BlankSerialize(ByVal elementName As String) As XElement
                Throw New NotImplementedException()
            End Function

            Public Overloads Overrides Sub Clear()
                Me.ID = ""
                Me.MPAA = ""
                Me.Outline = ""
                Me.Plot = ""
                Me.Premiered = ""
                Me.Runtime = ""
                Me.Studio = ""
                Me.Tagline = ""
                Me.Title = ""

                Me.Rating = 0.0
                Me.Year = 0
                Me.Top250 = 0
                Me.Votes = 0

                Me.Trailers = New List(Of String)()
                Me.Genres = New List(Of String)()
                Me.Certifications = New List(Of String)()
                Me.Directors = New List(Of String)()
                Me.Writers = New List(Of String)()

                Me.Thumbs = New List(Of Thumbnail)()
                Me.Actors = New List(Of PersonTag)()
                Me.Fanart = New Fanart()

                Dim tmpstring As String = String.Empty
                Dim TmpList As New List(Of String)

                If Not StringLists.TryGetValue("set", TmpList) Then
                    Me.Sets = New List(Of String)()
                End If

                If Not UserProperties.TryGetValue("lastplayed", tmpstring) Then
                    Me.LastPlayed = ""
                End If
                If Not UserProperties.TryGetValue("playcount", tmpstring) Then
                    Me.PlayCount = 0
                End If
            End Sub

            Public Shadows Sub CopyFrom(ByVal other As MovieTag)
                Me.Clear()
                Deserialize(other.Serialize("movie"))

                Genres.Clean("/")
                Directors.Clean("/")
                Directors.Clean("|")
                Writers.Clean("/")
                Writers.Clean("|")
            End Sub

            Public Sub CopyTo(ByVal other As MovieTag)
                other.Clear()
                other.Deserialize(Me.Serialize("movie"))

                other.Genres.Clean("/")
                other.Directors.Clean("/")
                other.Writers.Clean("/")
            End Sub

            Public Overloads Sub Deserialize(ByVal xInfo As XDocument)
                Deserialize(xInfo.Root)
            End Sub

            Public Overloads Overrides Sub Deserialize(ByVal element As XElement)
                Me.Title = element.GetStringElement("title", Title)
                Me.ID = element.GetStringElement("id", ID)
                Me.MPAA = element.GetStringElement("mpaa", MPAA)
                Me.Plot = element.GetStringElement("plot", Plot)
                Me.Premiered = element.GetStringElement("premiered", Premiered)
                Tagline = element.GetStringElement("tagline", Tagline)
                Outline = element.GetStringElement("outline", Outline)
                Studio = element.GetStringElement("studio", Studio)
                Runtime = element.GetStringElement("runtime", Runtime)

                Top250 = element.GetIntElement("top250", Top250)
                Year = element.GetIntElement("year", Year)
                Rating = ProcessRating(element.Element("rating"))

                element.UpdateStringList("certification", Certifications)

                element.UpdateStringList("genre", Genres)

                element.UpdateStringList("director", Me.Directors)

                element.UpdateStringList("credits", Me.Writers)

                element.UpdatePersonList("actor", Actors)

                element.UpdateThumbList("thumb", Thumbs)

                Dim trailerelements As New List(Of XElement)(element.Elements("trailer"))

                If trailerelements.Count > 0 Then
                    For Each trailerelement As XElement In trailerelements
                        If Not IsNothing(trailerelement.Attribute("urlencoded")) Then
                            If trailerelement.Attribute("urlencoded").Value = "yes" Then
                                Trailers.Add(UrlInfo.UrlDecode(trailerelement.Value))
                            Else
                                Trailers.Add(trailerelement.Value)
                            End If
                        End If
                    Next
                End If

                If Not IsNothing(element.Element("fanart")) Then
                    Fanart.Deserialize(element.Element("fanart"))
                End If

                element.UpdateStringList("set", Sets)
                PlayCount = element.GetIntElement("playcount", PlayCount)
                LastPlayed = element.GetStringElement("lastplayed", LastPlayed)

                Genres.Clean("/")
                Genres.Clean("|")
                Directors.Clean("|")
                Directors.Clean("/")
                Writers.Clean("/")
                Writers.Clean("|")
            End Sub

            Public Overloads Overrides Sub Deserialize(ByVal xmlFilePath As String)
                Dim tmpDocument As XDocument = XDocument.Load(xmlFilePath)
                Deserialize(tmpDocument)
            End Sub

            Public Overloads Function Serialize(ByVal rootElementName As String, ByVal xmlVersion As String, ByVal xmlEncoding As String, ByVal standalone As String) As XDocument
                Dim tmp As New XDocument(New XDeclaration(xmlVersion, xmlEncoding, standalone), New Object() {Serialize(rootElementName)})
                Return tmp
            End Function

            Public Overloads Function Serialize(ByVal elementName As String) As XElement
                Dim xTemp As New XElement(elementName)

                xTemp.AddStringElement("title", Title)
                xTemp.AddStringElement("id", ID)
                xTemp.AddStringElement("mpaa", MPAA)
                xTemp.AddStringElement("tagline", Tagline)
                xTemp.AddStringElement("outline", Outline)
                xTemp.AddStringElement("plot", Plot)
                xTemp.AddStringElement("studio", Studio)
                xTemp.AddStringElement("runtime", Runtime)
                xTemp.AddStringElement("premiered", Premiered)
                xTemp.AddStringElement("rating", Rating.ToString("#0.0"))
                xTemp.AddIntElement("year", Year, 1)

                xTemp.AddIntElement("top250", Top250, 1)
                xTemp.AddIntElement("votes", Votes, 1)

                For Each Trailer As String In Trailers
                    If Not String.IsNullOrEmpty(Trailer) Then
                        If Trailer.Contains("://") Then
                            Dim tmp As New XElement("trailer", UrlInfo.UrlEncode(Trailer))
                            tmp.Add(New XAttribute("urlencoded", "yes"))
                        Else
                            xTemp.AddStringElement("trailer", Trailer)
                        End If
                    End If
                Next

                xTemp.AddStringList("certification", Certifications)
                xTemp.AddStringList("genre", Genres)
                xTemp.AddStringList("credits", Writers)
                xTemp.AddStringList("director", Directors)
                xTemp.AddStringList("set", Sets)
                xTemp.AddThumbList("thumb", Thumbs)
                xTemp.Add(Fanart.Serialize("fanart"))
                xTemp.AddPersonList("actor", Actors)
                xTemp.AddIntElement("playcount", PlayCount, 0)
                xTemp.AddStringElement("lastplayed", LastPlayed)
                Return xTemp
            End Function

            Public Overloads Overrides Function ToString() As String
                Return Serialize("movie").ToString()
            End Function

#End Region 'Methods

        End Class

    End Namespace
End Namespace
