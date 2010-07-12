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
    <EbmlTypeAttribute(GetType(Byte()))> _
 Public Class EbmlBinaryElement
        Inherits EbmlValueElement
        Private _raw As Byte()

        Public Sub New(ByVal id As Long)
            MyBase.New(id)
        End Sub

        Public Sub New(ByVal id As Long, ByVal raw As Byte())
            MyBase.New(id)
            _raw = raw
        End Sub

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            _raw = New Byte(size - 1) {}
            For i As Integer = 0 To size - 1
                _raw(i) = data(offset + i)
            Next
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            _raw = DirectCast(o, Byte())
        End Sub

        Public ReadOnly Property Value() As Byte()
            Get
                Return _raw
            End Get
        End Property

        Public Overrides Function GetValue() As Object
            Return _raw
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            EbmlUtility.WriteID(strm, _id)
            EbmlUtility.WriteVariableSizeInteger(strm, _raw.Length)
            strm.Write(_raw, 0, _raw.Length)
        End Sub
    End Class
End Namespace
