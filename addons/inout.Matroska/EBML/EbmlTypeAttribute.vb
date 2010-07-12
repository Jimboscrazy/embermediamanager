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

Namespace EBML.Serialization
    <AttributeUsage(AttributeTargets.[Class] Or AttributeTargets.Struct, AllowMultiple:=True, Inherited:=False)> _
    Public Class EbmlTypeAttribute
        Inherits Attribute
        Private _type As Type

        Public Sub New(ByVal type As Type)
            _type = type
        End Sub

        Friend ReadOnly Property Type() As Type
            Get
                Return _type
            End Get
        End Property
    End Class
End Namespace
