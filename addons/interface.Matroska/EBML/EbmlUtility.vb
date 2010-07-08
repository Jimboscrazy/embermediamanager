Imports System.IO

Namespace EBML
    Public NotInheritable Class EbmlUtility
        Private Sub New()
        End Sub
        Public Shared Function ReadVariableSizeInteger(ByVal strm As Stream, ByRef unknownSize As Long, ByRef width As Integer) As Long
            Return ReadVariableSizeInteger(strm, False, unknownSize, width)
        End Function

        Public Shared Function ReadID(ByVal strm As Stream, ByRef unknownSize As Long, ByRef width As Integer) As Long
            Return ReadVariableSizeInteger(strm, True, unknownSize, width)
        End Function

        Public Shared Function ReadVariableSizeInteger(ByVal strm As Stream, ByVal includeFirstBit As Boolean, ByRef unknownSize As Long, ByRef width As Integer) As Long
            Const MaxWidth As Integer = 8
            Dim mask As Byte = &H80, firstByte As Byte = 0
            Dim i As Integer, tmp As Integer
            Dim value As Long = 0
            unknownSize = &H7F
            width = 1

            For i = 0 To MaxWidth - 1
                If (InlineAssignHelper(tmp, strm.ReadByte())) < 0 Then
                    Return -1
                End If
                If i = 0 Then
                    firstByte = CByte(tmp)
                    value = firstByte
                Else
                    unknownSize = (unknownSize << 7) Or &H7F
                    value = (value << 8) Or CByte(tmp)
                    width += 1
                End If
                If (firstByte And mask) <> 0 Then
                    If includeFirstBit Then
                        Return value
                    Else
                        Return value And unknownSize
                    End If
                End If
                mask >>= 1
            Next
            Return -3
        End Function

        Public Shared Function ReadVariableSizeInteger(ByVal data As Byte(), ByVal offset As Integer, ByRef unknownSize As Long, ByRef width As Integer) As Long
            Return ReadVariableSizeInteger(data, offset, False, unknownSize, width)
        End Function

        Public Shared Function ReadID(ByVal data As Byte(), ByVal offset As Integer, ByRef unknownSize As Long, ByRef width As Integer) As Long
            Return ReadVariableSizeInteger(data, offset, True, unknownSize, width)
        End Function

        Public Shared Function ReadVariableSizeInteger(ByVal data As Byte(), ByVal offset As Integer, ByVal includeFirstBit As Boolean, ByRef unknownSize As Long, ByRef width As Integer) As Long
            Const MaxWidth As Integer = 8
            Dim mask As Byte = &H80, firstByte As Byte = 0
            Dim i As Integer
            Dim value As Long = 0
            unknownSize = &H7F
            width = 1

            For i = 0 To MaxWidth - 1
                If data.Length <= offset + i Then
                    Return -1
                End If
                If i = 0 Then
                    firstByte = data(offset + i)
                    value = firstByte
                Else
                    unknownSize = (unknownSize << 7) Or &H7F
                    value = (value << 8) Or data(offset + i)
                    width += 1
                End If
                If (firstByte And mask) <> 0 Then
                    If includeFirstBit Then
                        Return value
                    Else
                        Return value And unknownSize
                    End If
                End If
                mask >>= 1
            Next
            Throw New FormatException("Not found EBML VINT data in stream")
        End Function

        Public Shared Sub WriteID(ByVal strm As Stream, ByVal id As Long)
            Dim buf As Byte() = New Byte() {CByte(id >> 56), CByte(id >> 48), CByte(id >> 40), CByte(id >> 32), CByte(id >> 24), CByte(id >> 16), _
             CByte(id >> 8), CByte(id >> 0)}
            Dim i As Integer = 0
            While buf(i) = 0
                i += 1
            End While
            strm.Write(buf, i, buf.Length - i)
        End Sub
        Public Shared Sub WriteVariableSizeInteger(ByVal strm As Stream, ByVal value As Long)
            If value < 0 Then
                Throw New ArgumentOutOfRangeException()
            ElseIf value < &H7F Then
                strm.WriteByte(CByte(&H80 Or value))
            ElseIf value < &H3FFF Then
                strm.Write(New Byte() {CByte(&H40 Or (value >> 8)), CByte(value)}, 0, 2)
            ElseIf value < &H1FFFFF Then
                strm.Write(New Byte() {CByte(&H20 Or (value >> 16)), CByte(value >> 8), CByte(value)}, 0, 3)
            ElseIf value < &HFFFFFFF Then
                strm.Write(New Byte() {CByte(&H10 Or (value >> 24)), CByte(value >> 16), CByte(value >> 8), CByte(value)}, 0, 4)
            ElseIf value < &H7FFFFFFFFL Then
                strm.Write(New Byte() {CByte(&H8 Or (value >> 32)), CByte(value >> 24), CByte(value >> 16), CByte(value >> 8), CByte(value)}, 0, 5)
            ElseIf value < &H3FFFFFFFFFFL Then
                strm.Write(New Byte() {CByte(&H4 Or (value >> 40)), CByte(value >> 32), CByte(value >> 24), CByte(value >> 16), CByte(value >> 8), CByte(value)}, 0, 6)
            ElseIf value < &H1FFFFFFFFFFFFL Then
                strm.Write(New Byte() {CByte(&H2 Or (value >> 48)), CByte(value >> 40), CByte(value >> 32), CByte(value >> 24), CByte(value >> 16), CByte(value >> 8), _
                 CByte(value)}, 0, 7)
            ElseIf value < &HFFFFFFFFFFFFFFL Then
                strm.Write(New Byte() {CByte(&H1 Or (value >> 56)), CByte(value >> 48), CByte(value >> 40), CByte(value >> 32), CByte(value >> 24), CByte(value >> 16), _
                 CByte(value >> 8), CByte(value)}, 0, 8)
            Else
                Throw New ArgumentOutOfRangeException()
            End If
        End Sub

        Public Shared Sub WriteInfinityValue(ByVal strm As Stream)
            WriteInfinityValue(strm, 1)
        End Sub
        Public Shared Sub WriteInfinityValue(ByVal strm As Stream, ByVal width As Integer)
            Select Case width
                Case 1
                    strm.WriteByte(&HFF)
                    Exit Select
                Case 2
                    strm.Write(New Byte() {&H7F, &HFF}, 0, 2)
                    Exit Select
                Case 3
                    strm.Write(New Byte() {&H3F, &HFF, &HFF}, 0, 3)
                    Exit Select
                Case 4
                    strm.Write(New Byte() {&H1F, &HFF, &HFF, &HFF}, 0, 4)
                    Exit Select
                Case 5
                    strm.Write(New Byte() {&HF, &HFF, &HFF, &HFF, &HFF}, 0, 5)
                    Exit Select
                Case 6
                    strm.Write(New Byte() {&H7, &HFF, &HFF, &HFF, &HFF, &HFF}, 0, 6)
                    Exit Select
                Case 7
                    strm.Write(New Byte() {&H3, &HFF, &HFF, &HFF, &HFF, &HFF, _
                     &HFF}, 0, 7)
                    Exit Select
                Case 8
                    strm.Write(New Byte() {&H1, &HFF, &HFF, &HFF, &HFF, &HFF, _
                     &HFF, &HFF}, 0, 8)
                    Exit Select
                Case Else
                    Throw New ArgumentOutOfRangeException()
            End Select
        End Sub

        Public Shared Function GetBytes(ByVal value As Long) As Integer
            If value <= &H7F AndAlso value >= -&H80 Then
                Return 1
            End If
            If value <= &H7FFF AndAlso value >= -&H8000 Then
                Return 2
            End If
            If value <= &H7FFFFF AndAlso value >= -&H800000 Then
                Return 3
            End If
            If value <= &H7FFFFFFF AndAlso value >= -&H80000000UI Then
                Return 4
            End If
            If value <= &H7FFFFFFFFFL AndAlso value >= -&H8000000000L Then
                Return 5
            End If
            If value <= &H7FFFFFFFFFFFL AndAlso value >= -&H800000000000L Then
                Return 6
            End If
            If value <= &H7FFFFFFFFFFFFFL AndAlso value >= -&H80000000000000L Then
                Return 7
            End If
            Return 8
        End Function

        Public Shared Function GetBytes(ByVal value As ULong) As Integer
            Dim bytes As Integer = 0
            While value > 0
                value >>= 8
                bytes += 1
            End While
            Return If(bytes = 0, 1, bytes)
        End Function

        Shared _defaultInfWidth As Integer = 4
        Public Shared Property DefaultInfinityValueWidth() As Integer
            Get
                Return _defaultInfWidth
            End Get
            Set(ByVal value As Integer)
                _defaultInfWidth = value
            End Set
        End Property

        Public Shared Function VariableIntegerToID(ByVal value As Long) As Long
            If value < 0 Then
                Throw New ArgumentOutOfRangeException()
            ElseIf value < &H7F Then
                Return &H80 Or value
            ElseIf value < &H3FFF Then
                Return &H4000 Or value
            ElseIf value < &H1FFFFF Then
                Return &H200000 Or value
            ElseIf value < &HFFFFFFF Then
                Return &H10000000 Or value
            ElseIf value < &H7FFFFFFFFL Then
                Return &H800000000L Or value
            ElseIf value < &H3FFFFFFFFFFL Then
                Return &H40000000000L Or value
            ElseIf value < &H1FFFFFFFFFFFFL Then
                Return &H2000000000000L Or value
            ElseIf value < &HFFFFFFFFFFFFFFL Then
                Return &H100000000000000L Or value
            Else
                Throw New ArgumentOutOfRangeException()
            End If
        End Function

        Public Shared Function IDToVariableInteger(ByVal value As Long) As Long
            If value < 0 Then
                Throw New ArgumentOutOfRangeException()
            ElseIf value < &HFF Then
                Return value And &H7F
            ElseIf value < &H7FFF Then
                Return value And &H3FFF
            ElseIf value < &H3FFFFF Then
                Return value And &H1FFFFF
            ElseIf value < &H1FFFFFFF Then
                Return value And &HFFFFFFF
            ElseIf value < &HFFFFFFFFFL Then
                Return value And &H7FFFFFFFFL
            ElseIf value < &H7FFFFFFFFFFL Then
                Return value And &H3FFFFFFFFFFL
            ElseIf value < &H3FFFFFFFFFFFFL Then
                Return value And &H1FFFFFFFFFFFFL
            ElseIf value < &H1FFFFFFFFFFFFFFL Then
                Return value And &HFFFFFFFFFFFFFFL
            Else
                Throw New ArgumentOutOfRangeException()
            End If
        End Function
        Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
            target = value
            Return value
        End Function
    End Class
End Namespace
