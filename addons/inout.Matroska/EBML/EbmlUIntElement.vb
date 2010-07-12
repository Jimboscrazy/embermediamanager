' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################

Imports System.IO
Imports InOut.Matroska.EBML.Serialization

Namespace EBML
    <Serialization.EbmlType(GetType(Byte))> _
    <Serialization.EbmlType(GetType(UShort))> _
    <Serialization.EbmlType(GetType(UInteger))> _
    <Serialization.EbmlType(GetType(ULong))> _
    Public Class EbmlUIntElement
        Inherits EbmlValueElement
        Private _value As ULong

        Public Sub New(ByVal id As Long)
            Me.New(id, 0)
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As ULong)
            MyBase.New(id)
            _value = value
        End Sub

        Public Property Value() As ULong
            Get
                Return _value
            End Get
            Set(ByVal value As ULong)
                _value = value
            End Set
        End Property

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            If size = 0 OrElse size > 8 Then
                Throw New ArgumentOutOfRangeException()
            End If

            size += offset
            _value = data(offset)
            System.Threading.Interlocked.Increment(offset)
            While offset < size
                _value = (_value << 8) Or data(offset)
                System.Threading.Interlocked.Increment(offset)
            End While
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            If TypeOf o Is ULong Then
                _value = CULng(o)
            ElseIf TypeOf o Is UInteger Then
                _value = CUInt(o)
            ElseIf TypeOf o Is UShort Then
                _value = CUShort(o)
            ElseIf TypeOf o Is Byte Then
                _value = CByte(o)
            Else
                Throw New ArgumentException()
            End If
        End Sub

        Public Overrides Function GetValue() As Object
            Return _value
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            Dim bytes As Integer = EbmlUtility.GetBytes(_value)
            EbmlUtility.WriteID(strm, _id)
            EbmlUtility.WriteVariableSizeInteger(strm, bytes)

            For i As Integer = bytes - 1 To 0 Step -1
                strm.WriteByte(CByte(_value >> (i << 3)))
            Next
        End Sub
    End Class
End Namespace
