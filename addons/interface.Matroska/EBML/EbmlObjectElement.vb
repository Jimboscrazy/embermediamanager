Imports System.IO
Imports EBML
Imports EBML.Serialization


Namespace Serialization
    <EBML.Serialization.EbmlType(GetType(Object))> _
    Class EbmlObjectElement
        Inherits EbmlValueElement
        Private _value As Object

        Public Sub New(ByVal id As Long)
            MyBase.New(id)
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As Object)
            MyBase.New(id)
            _value = value
        End Sub

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            If size = 0 Then
                _value = Nothing
                Return
            End If

            Using ms As New MemoryStream(data, offset, size)
                _value = EbmlFormatter.Instance.Deserialize(ms)
            End Using
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            _value = o
        End Sub

        Public Overrides Function GetValue() As Object
            Return _value
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            EbmlUtility.WriteID(strm, _id)
            If _value Is Nothing Then
                EbmlUtility.WriteVariableSizeInteger(strm, 0)
                Return
            End If

            Using ms As New MemoryStream()
                EbmlFormatter.Instance.Serialize(ms, _value)
                EbmlUtility.WriteVariableSizeInteger(strm, ms.Length)
                ms.WriteTo(strm)
            End Using
        End Sub
    End Class
End Namespace
