Imports System.Collections.Generic
Imports System.Xml.Linq

Namespace XMLScraper
    Namespace MediaTags

        Public Class Fanart

#Region "Fields"

            Private _thumbs As List(Of Thumbnail)
            Private _url As String

#End Region 'Fields

#Region "Constructors"

            Public Sub New()
                Me.Clear()
            End Sub

            Public Sub New(ByVal element As XElement)
                Me.Clear()
                Deserialize(element)
            End Sub

#End Region 'Constructors

#Region "Properties"

            Public ReadOnly Property Count() As Integer
                Get
                    Return _thumbs.Count
                End Get
            End Property

            Public Property Thumbs() As List(Of Thumbnail)
                Get
                    Return _thumbs
                End Get
                Private Set(ByVal value As List(Of Thumbnail))
                    _thumbs = value
                End Set
            End Property

            Public Property Url() As String
                Get
                    Return _url
                End Get
                Set(ByVal value As String)
                    _url = value
                End Set
            End Property

#End Region 'Properties

#Region "Methods"

            Public Sub Add(ByVal newFanart As Thumbnail)
                Thumbs.Add(newFanart)
            End Sub

            Public Sub Clear()
                Me._url = String.Empty
                Me._thumbs = New List(Of Thumbnail)
            End Sub

            Public Sub Deserialize(ByVal element As XElement)
                Me.Clear()

                If Not IsNothing(element.Attribute("url")) Then
                    _url = element.Attribute("url").Value
                End If
                For Each item As XElement In element.Elements("thumb")
                    _thumbs.Add(New Thumbnail(item))
                Next
            End Sub

            Public Function GetFanartLink(ByVal index As Integer) As String
                If index < Me.Count AndAlso index >= 0 AndAlso Not String.IsNullOrEmpty(_thumbs(index).Thumb) Then
                    Return String.Concat(_url, _thumbs(index).Thumb)
                End If

                Return Nothing
            End Function

            Public Function GetPreviewLink(ByVal index As Integer) As String
                If index < Me.Count AndAlso index >= 0 AndAlso Not String.IsNullOrEmpty(_thumbs(index).Preview) Then
                    Return String.Concat(_url, _thumbs(index).Preview)
                End If

                Return Nothing
            End Function

            Public Function Serialize(ByVal elementName As String) As XElement
                Dim tmp As New XElement("fanart")
                If Me.Count > 0 Then
                    tmp.Add(New XAttribute("url", _url))
                    For Each item As Thumbnail In _thumbs
                        tmp.Add(item.Serialize("thumb"))
                    Next
                End If
                Return tmp
            End Function

            Friend Shared Function BlankSerialize(ByVal p As String) As Object()
                Throw New NotImplementedException()
            End Function

#End Region 'Methods

        End Class

    End Namespace
End Namespace
