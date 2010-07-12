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

Imports InOut.Matroska.EBML

Namespace EBML.Serialization
    Public Class EbmlFieldSemanticAttribute
        Inherits Attribute
        Private _id As Long
        Private _elementType As Type

        Public Sub New(ByVal id As Long)
            _id = id
            _elementType = Nothing
        End Sub

        Public Sub New(ByVal id As Long, ByVal elementType As Type)
            If Not elementType.IsSubclassOf(GetType(EbmlElement)) Then
                Throw New ArgumentException()
            End If
            _id = id
            _elementType = elementType
        End Sub

        Public ReadOnly Property ID() As Long
            Get
                Return _id
            End Get
        End Property

        Public Property ElementType() As Type
            Get
                Return _elementType
            End Get
            Friend Set(ByVal value As Type)
                _elementType = Value
            End Set
        End Property
    End Class
End Namespace
