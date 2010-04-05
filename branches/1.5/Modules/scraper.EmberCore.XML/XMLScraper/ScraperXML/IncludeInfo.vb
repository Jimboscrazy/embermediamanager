Imports System.Xml.Linq

Namespace XMLScraper
    Namespace ScraperXML

        Public NotInheritable Class IncludeInfo

#Region "Fields"

            Private _document As XElement
            Private _name As String

#End Region 'Fields

#Region "Constructors"

            Public Sub New(ByVal doc As XDocument, ByVal xmlFilePath As String)
                Me.Clear()
                Me._document = doc.Root
                Me._name = String.Concat("common/", xmlFilePath.Substring(xmlFilePath.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1))
            End Sub

#End Region 'Constructors

#Region "Properties"

            Public Property Document() As XElement
                Get
                    Return Me._document
                End Get
                Set(ByVal value As XElement)
                    Me._document = value
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

#End Region 'Properties

#Region "Methods"

            Public Sub Clear()
                Me._name = String.Empty
                Me._document = Nothing
            End Sub

#End Region 'Methods

        End Class

    End Namespace
End Namespace
