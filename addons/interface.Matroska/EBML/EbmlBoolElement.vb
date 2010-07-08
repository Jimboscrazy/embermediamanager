Imports System.IO
Imports EBML.Serialization

Namespace EBML
    <EbmlTypeAttribute(GetType(Boolean))> _
 Public Class EbmlBoolElement
        Inherits EbmlValueElement
        Private _value As Boolean

        Public Sub New(ByVal id As Long)
            Me.New(id, False)
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As Boolean)
            MyBase.New(id)
            _value = value
        End Sub

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            If size <> 0 AndAlso size <> 1 Then
                Throw New FormatException()
            End If
            _value = (size = 1)
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            _value = CBool(o)
        End Sub

        Public Overrides Function GetValue() As Object
            Return _value
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            EbmlUtility.WriteID(strm, _id)
            If _value Then
                EbmlUtility.WriteVariableSizeInteger(strm, 1)
                strm.WriteByte(1)
            Else
                EbmlUtility.WriteVariableSizeInteger(strm, 0)
            End If
        End Sub
    End Class
End Namespace
