Imports System.IO
Imports System.Net
Imports EBML.Serialization

Namespace EBML
    <Serialization.EbmlType(GetType(IPEndPoint))> _
 Public Class EbmlIPEndPointElement
        Inherits EbmlValueElement
        Private _value As IPEndPoint

        Public Sub New(ByVal id As Long)
            MyBase.New(id)
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As IPEndPoint)
            MyBase.New(id)
            _value = value
        End Sub

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            Dim adrsBytes As Byte() = New Byte(data(offset) - 1) {}
            Buffer.BlockCopy(data, offset + 1, adrsBytes, 0, adrsBytes.Length)
            Dim adrs As New IPAddress(adrsBytes)
            Dim port As Integer = BitConverter.ToInt32(data, offset + 1 + adrsBytes.Length)
            _value = New IPEndPoint(adrs, port)
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            _value = DirectCast(o, IPEndPoint)
        End Sub

        Public Overrides Function GetValue() As Object
            Return _value
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            Dim adrsBytes As Byte() = _value.Address.GetAddressBytes()
            Dim portBytes As Byte() = BitConverter.GetBytes(_value.Port)

            EbmlUtility.WriteID(strm, _id)
            EbmlUtility.WriteVariableSizeInteger(strm, 1 + adrsBytes.Length + portBytes.Length)
            strm.WriteByte(CByte(adrsBytes.Length))
            strm.Write(adrsBytes, 0, adrsBytes.Length)
            strm.Write(portBytes, 0, portBytes.Length)
        End Sub
    End Class
End Namespace
