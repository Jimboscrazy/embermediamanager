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

Namespace EBML
    Public MustInherit Class EbmlElement
        Protected _id As Long
        Private _streamPos As Long
        Private _loaded As Boolean
        Private _ElementSize As Long
        Private _PayLoadSize As Long
        Protected Sub New(ByVal id As Long)
            _id = id
        End Sub

        Public ReadOnly Property ID() As Long
            Get
                Return _id
            End Get
        End Property

        Public Property StreamPosition() As Long
            Get
                Return _streamPos
            End Get
            Set(ByVal value As Long)
                _streamPos = value
            End Set
        End Property

        Public Property ElementSize() As Long
            Get
                Return _ElementSize
            End Get
            Set(ByVal value As Long)
                _ElementSize = value
            End Set
        End Property

        Public Property PayLoadSize() As Long
            Get
                Return _PayLoadSize
            End Get
            Set(ByVal value As Long)
                _PayLoadSize = value
            End Set
        End Property

        Public Property Loaded() As Boolean
            Get
                Return _loaded
            End Get
            Set(ByVal value As Boolean)
                _loaded = value
            End Set
        End Property

        Public MustOverride ReadOnly Property IsContainer() As Boolean
        Public MustOverride Sub WriteTo(ByVal strm As Stream, ByVal options As EBMLWriteOptions)
    End Class
End Namespace
