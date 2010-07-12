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
    <Serialization.EbmlType(GetType(Single))> _
 <Serialization.EbmlType(GetType(Double))> _
 Public Class EbmlFloatElement
        Inherits EbmlValueElement
        Private _size As Integer = 4
        Private _value As Double

        Public Sub New(ByVal id As Long)
            Me.New(id, 0.0F)
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As Single)
            MyBase.New(id)
            _value = value
            _size = 4
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As Double)
            MyBase.New(id)
            _value = value
            _size = 8
        End Sub

        Public Property Value() As Double
            Get
                Return _value
            End Get
            Set(ByVal value As Double)
                _value = value
            End Set
        End Property

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            If size <> 4 AndAlso size <> 8 Then
                Throw New ArgumentOutOfRangeException()
            End If

            _size = size
            If size = 4 Then
                _value = BitConverter.ToSingle(data, offset)
            ElseIf size = 8 Then
                _value = BitConverter.ToDouble(data, offset)
            End If
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            If TypeOf o Is Double Then
                _value = CDbl(o)
                _size = 8
            ElseIf TypeOf o Is Single Then
                _value = CSng(o)
                _size = 4
            Else
                Throw New ArgumentException()
            End If
        End Sub

        Public Overrides Function GetValue() As Object
            Return _value
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            EbmlUtility.WriteID(strm, _id)
            EbmlUtility.WriteVariableSizeInteger(strm, _size)
            If _size = 4 Then
                strm.Write(BitConverter.GetBytes(CSng(_value)), 0, 4)
            Else
                strm.Write(BitConverter.GetBytes(_value), 0, 8)
            End If
        End Sub
    End Class
End Namespace
