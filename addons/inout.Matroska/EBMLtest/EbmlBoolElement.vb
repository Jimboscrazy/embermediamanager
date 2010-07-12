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
