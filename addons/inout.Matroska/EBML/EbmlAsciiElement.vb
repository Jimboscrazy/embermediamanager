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

Namespace EBML
    Public Class EbmlAsciiElement
        Inherits EbmlValueElement
        Private _value As String

        Public Sub New(ByVal id As Long)
            MyBase.New(id)
        End Sub

        Public Sub New(ByVal id As Long, ByVal value As String)
            MyBase.New(id)
            _value = value
        End Sub

        Public Overrides Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
            _value = Encoding.ASCII.GetString(data, offset, size)
        End Sub

        Public Overrides Sub UpdateValue(ByVal o As Object)
            _value = DirectCast(o, String)
        End Sub

        Public ReadOnly Property Value() As String
            Get
                Return _value
            End Get
        End Property

        Public Overrides Function GetValue() As Object
            Return _value
        End Function

        Public Overrides Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
            Dim raw As Byte() = Encoding.ASCII.GetBytes(_value)
            EbmlUtility.WriteID(strm, _id)
            EbmlUtility.WriteVariableSizeInteger(strm, raw.Length)
            strm.Write(raw, 0, raw.Length)
        End Sub
    End Class
End Namespace
