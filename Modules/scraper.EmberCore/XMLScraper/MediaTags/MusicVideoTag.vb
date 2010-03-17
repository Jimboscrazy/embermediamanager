Imports System.Collections.Generic
Imports System.Xml.Linq

Imports EmberScraperModule.XMLScraper.ScraperLib
Imports EmberScraperModule.XMLScraper.Utilities

Namespace XMLScraper
    Namespace MediaTags

        Public Class MusicVideoTag
            Inherits MediaTag

#Region "Constructors"

            Public Sub New()
                Clear()
            End Sub

            Public Sub New(ByVal element As XElement)
                Me.New()
                Deserialize(element)
            End Sub

#End Region 'Constructors

#Region "Properties"

            Public Property Album() As String
                Get
                    Return MyBase.UserProperties("album")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("album") = value
                End Set
            End Property

            Public Property Artist() As String
                Get
                    Return MyBase.UserProperties("artist")
                End Get
                Set(ByVal value As String)
                    MyBase.UserProperties("artist") = value
                End Set
            End Property

            Public Property Directors() As List(Of String)
                Get
                    Return MyBase.StringLists("director")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("director") = Value
                End Set
            End Property

            Public Property Genres() As List(Of String)
                Get
                    Return MyBase.StringLists("genre")
                End Get
                Set(ByVal value As List(Of String))
                    MyBase.StringLists("genre") = Value
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

            Public Overloads Overrides ReadOnly Property TagType() As MediaType
                Get
                    Return MediaType.musicvideo
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
                Artist = ""
                Album = ""
                Studio = ""
                Year = 0
                Directors = New List(Of String)()
                Thumbs = New List(Of Thumbnail)()
                Genres = New List(Of [String])()
            End Sub

            Public Overloads Sub Deserialize(ByVal xInfo As XDocument)
                Deserialize(xInfo.Root)
            End Sub

            Public Overloads Overrides Sub Deserialize(ByVal element As XElement)
                Title = element.GetStringElement("title", Title)
                Artist = element.GetStringElement("artist", Artist)
                Year = element.GetIntElement("year", Year)
                Album = element.GetStringElement("album", Album)
                Studio = element.GetStringElement("studio", Studio)
                element.UpdateStringList("director", Directors)
                element.UpdateThumbList("thumb", Thumbs)
                element.UpdateStringList("director", Directors)
                element.UpdateStringList("genre", Genres)
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
                Dim xTemp As New XElement("musicvideo")

                xTemp.AddStringElement("title", Title)
                xTemp.AddIntElement("year", Year, 1)
                xTemp.AddStringElement("album", Album)
                xTemp.AddStringElement("studio", Studio)
                xTemp.AddThumbList("thumb", Thumbs)
                xTemp.AddStringElement("artist", Artist)
                xTemp.AddStringList("director", Directors)
                xTemp.AddStringList("genre", Genres)

                Return xTemp
            End Function

            Public Overloads Overrides Function ToString() As String
                Return Serialize("musicvideo").ToString()
            End Function

#End Region 'Methods

        End Class

    End Namespace
End Namespace
