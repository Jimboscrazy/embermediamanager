Imports System.Collections.Generic
Imports System.Xml.Linq

Imports EmberScraperModule.XMLScraper.ScraperLib
Imports EmberScraperModule.XMLScraper.Utilities

Namespace XMLScraper
    Namespace MediaTags

        Public Class TvShowTag
            Inherits MediaTag

#Region "Fields"

            Private m_EpisodeGuide As List(Of UrlInfo)
            Private m_Episodes As List(Of TVEpisodeTag)

#End Region 'Fields

#Region "Constructors"

            Public Sub New()
                MyBase.New()
                Clear()
            End Sub

#End Region 'Constructors

#Region "Properties"

            Public Property Actors() As List(Of PersonTag)
                Get
                    Return MyBase._People
                End Get
                Set(ByVal value As List(Of PersonTag))
                    MyBase._People = Value
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

            Public Property EpisodeGuide() As List(Of UrlInfo)
                Get
                    Return m_EpisodeGuide
                End Get
                Private Set(ByVal value As List(Of UrlInfo))
                    m_EpisodeGuide = Value
                End Set
            End Property

            Public Property Episodes() As List(Of TVEpisodeTag)
                Get
                    Return m_Episodes
                End Get
                Set(ByVal value As List(Of TVEpisodeTag))
                    m_Episodes = Value
                End Set
            End Property

            Public Property Fanart() As Fanart
                Get
                    Return MyBase._Fanart
                End Get
                Set(ByVal value As Fanart)
                    MyBase._Fanart = Value
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

            Public Property MPAA() As String
                Get
                    Return MyBase.UserProperties("mpaa")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("mpaa") = value
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

            Public Property ScraperName() As String
                Get
                    Return MyBase.UserProperties("scraper")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("scraper") = Value
                End Set
            End Property

            Public Property Status() As String
                Get
                    Return MyBase.UserProperties("status")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("status") = Value
                End Set
            End Property

            Public Overloads Overrides ReadOnly Property TagType() As MediaType
                Get
                    Return MediaType.tvshow
                End Get
            End Property

            Public Property Thumbs() As List(Of Thumbnail)
                Get
                    Return MyBase._Thumbs
                End Get
                Set(ByVal value As List(Of Thumbnail))
                    MyBase._Thumbs = Value
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
                Title = ""
                Premiered = ""
                Plot = ""
                MPAA = ""
                ID = ""
                Status = ""
                Year = 0
                Rating = 0.0

                Genres = New List(Of String)()
                Directors = New List(Of String)()
                Writers = New List(Of String)()

                Thumbs = New List(Of Thumbnail)()
                Me.Fanart = New Fanart()
                Actors = New List(Of PersonTag)()
                EpisodeGuide = New List(Of UrlInfo)()
            End Sub

            Public Overloads Sub Deserialize(ByVal xInfo As XDocument)
                Deserialize(xInfo.Root)
            End Sub

            Public Overloads Overrides Sub Deserialize(ByVal element As XElement)
                Status = element.GetStringElement("status", Status)
                Dim episodeGuide__1 As XElement = element.Element("episodeguide")
                If Not IsNothing(episodeGuide__1) Then
                    For Each xUrl As XElement In episodeGuide__1.Elements("url")
                        EpisodeGuide.Add(New UrlInfo(xUrl))
                    Next
                End If
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
                Dim tvshow As New XElement(elementName)

                tvshow.AddStringElement("status", Status)

                If EpisodeGuide.Count > 0 Then
                    Dim episodeGuide__1 As New XElement("episodeguide")

                    For Each objUrl As UrlInfo In EpisodeGuide
                        episodeGuide__1.Add(New XElement(objUrl.Serialize("url")))
                    Next
                    tvshow.Add(episodeGuide__1)
                End If
                Return tvshow
            End Function

            Public Overloads Overrides Function ToString() As String
                Return Serialize("tvshow").ToString()
            End Function

#End Region 'Methods

        End Class

    End Namespace
End Namespace
