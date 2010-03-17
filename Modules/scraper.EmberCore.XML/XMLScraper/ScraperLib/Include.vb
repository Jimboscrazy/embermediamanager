Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq

Namespace XMLScraper
    Namespace ScraperLib

        Public Class Include
            Inherits ScraperFunctionContainer

#Region "Fields"

            Private m_Content As IncludeContent

#End Region 'Fields

#Region "Constructors"

            Public Sub New(ByVal xScraperFunctions As XElement, ByVal name As String, ByVal Content As IncludeContent)
                MyBase.New()
                Me.Content = Content
                For Each item As XElement In xScraperFunctions.Elements()
                    ScraperFunctions.Add(New ScraperFunction(item, Me))
                Next
                Me.Name = name
            End Sub

#End Region 'Constructors

#Region "Properties"

            Public Property Content() As IncludeContent
                Get
                    Return m_Content
                End Get
                Set(ByVal value As IncludeContent)
                    m_Content = value
                End Set
            End Property

            Public ReadOnly Property FriendlyName() As String
                Get
                    Return Name.Replace("common/", "")
                End Get
            End Property

#End Region 'Properties

        End Class

    End Namespace
End Namespace
