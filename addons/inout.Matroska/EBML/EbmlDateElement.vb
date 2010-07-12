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

Imports System.Text
Imports System.IO
Imports InOut.Matroska.EBML.Serialization

Namespace EBML
    <EbmlTypeAttribute(GetType(DateTime))> _
 Public Class EbmlDateElement
        Inherits EbmlValueElement
        Private _value As DateTime
        Shared BaseDateTime As New DateTime(2001, 1, 1, 0, 0, 0, _
         DateTimeKind.Utc)

        Public Sub New(ByVal id As Long)
            MyBase.New(id)
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As DateTime)
            MyBase.New(id)
            _value = value
        End Sub

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            If size <> 8 Then
                Throw New ArgumentOutOfRangeException()
            End If

            size += offset
            Dim value As Long = (If((data(offset) And &H80) <> 0, -1, 0))
            While offset < size
                value = (value << 8) Or data(offset)
                System.Threading.Interlocked.Increment(offset)
            End While
            _value = BaseDateTime.AddTicks(value)
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            _value = CType(o, DateTime)
        End Sub

        Public ReadOnly Property Value() As DateTime
            Get
                Return _value
            End Get
        End Property

        Public Overrides Function GetValue() As Object
            Return _value
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            Dim value As Long = _value.Subtract(BaseDateTime).Ticks
            Dim bytes As Integer = EbmlUtility.GetBytes(value)

            EbmlUtility.WriteID(strm, _id)
            EbmlUtility.WriteVariableSizeInteger(strm, 8)

            For i As Integer = 7 To bytes Step -1
                strm.WriteByte(0)
            Next
            For i As Integer = bytes - 1 To 0 Step -1
                strm.WriteByte(CByte(value >> (i << 3)))
            Next
        End Sub
    End Class
End Namespace
