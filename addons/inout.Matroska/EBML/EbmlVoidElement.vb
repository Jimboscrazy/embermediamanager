Imports System.IO

Namespace EBML
    Public Class EbmlVoidElement
        Inherits EbmlValueElement
        Private _size As Long

        Public Sub New()
            Me.New(&HEC)
        End Sub

        Public Sub New(ByVal id As Long)
            MyBase.New(id)
            If id <> &HEC Then
                Throw New ArgumentException()
            End If
            _size = 0
        End Sub

        Public Shared Function Create(ByVal padding As Integer) As EbmlVoidElement
            Dim element As New EbmlVoidElement()
            element.PaddingSize = padding
            Return element
        End Function

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            _size = size
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
        End Sub

        Public Overrides Function GetValue() As Object
            Return Nothing
        End Function

        Public Property PaddingSize() As Long
            Get
                Return _size
            End Get
            Set(ByVal value As Long)
                _size = value
            End Set
        End Property

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            Dim buffer As Byte() = New Byte(1024 * 8 - 1) {}
            Dim size As Long = _size
            EbmlUtility.WriteID(strm, _id)
            EbmlUtility.WriteVariableSizeInteger(strm, size)
            While size >= buffer.Length
                strm.Write(buffer, 0, buffer.Length)
                size -= buffer.Length
            End While
            strm.Write(buffer, 0, CInt(size))
        End Sub
    End Class
End Namespace
