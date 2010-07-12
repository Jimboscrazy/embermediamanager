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
Imports InOut.Matroska.EBML
Imports InOut.Matroska.EBML.Serialization


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
