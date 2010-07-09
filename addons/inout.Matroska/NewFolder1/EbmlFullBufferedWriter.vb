Imports System.Collections.Generic
Imports System.IO

Namespace EBML
    Public Class EbmlFullBufferedWriter
        Implements IDisposable
        Private _stack As New Stack(Of EbmlContainerElement)()
        Private _doc As EbmlDocument
        Private _semantic As EbmlSemantic
        Private _parent As EbmlContainerElement = Nothing
        Private _strm As Stream
        Private _closed As Boolean = False
        Private _writeOptions As EBMLWriteOptions

        Public Sub New(ByVal strm As Stream, ByVal semantic As EbmlSemantic)
            Me.New(strm, semantic, EBMLWriteOptions.WriteFixedSizeUseBigMemory)
        End Sub

        Public Sub New(ByVal strm As Stream, ByVal semantic As EbmlSemantic, ByVal options As EBMLWriteOptions)
            _strm = strm
            _semantic = semantic
            _doc = New EbmlDocument(semantic)
            _doc.LoadPayLoad = True
            _writeOptions = options
        End Sub

        Public Sub WriteStartElement(ByVal id As Long)
            Dim element As EbmlContainerElement = TryCast(_semantic.CreateElement(id), EbmlContainerElement)
            If element Is Nothing Then
                Throw New ArgumentException()
            End If
            AppendElement(element)
            _stack.Push(_parent)
            _parent = element
        End Sub

        Public Sub WriteAsciiElement(ByVal id As Long, ByVal text As String)
            AppendElement(New EbmlAsciiElement(id, text))
        End Sub

        Public Sub WriteValueElement(ByVal id As Long, ByVal value As Long)
            AppendElement(New EbmlSIntElement(id, value))
        End Sub

        Public Sub WriteValueElement(ByVal id As Long, ByVal value As ULong)
            AppendElement(New EbmlUIntElement(id, value))
        End Sub

        Public Sub WriteValueElement(ByVal id As Long, ByVal value As Byte())
            AppendElement(New EbmlBinaryElement(id, value))
        End Sub

        Public Sub WriteVoidElement(ByVal padding As Integer)
            AppendElement(EbmlVoidElement.Create(padding))
        End Sub

        Public Sub WriteEndElement()
            If _stack.Count = 0 Then
                _parent = Nothing
            Else
                _parent = _stack.Pop()
            End If
        End Sub

        Private Sub AppendElement(ByVal element As EbmlElement)
            If _parent Is Nothing Then
                _doc.RootElements.Add(element)
            Else
                _parent.Children.Add(element)
            End If
        End Sub

        Public Sub Close()
            If _closed Then
                Return
            End If
            _doc.WriteTo(_strm, _writeOptions)
        End Sub

        Private Sub IDisposable_Dispose() Implements IDisposable.Dispose
            Close()
        End Sub
    End Class
End Namespace
