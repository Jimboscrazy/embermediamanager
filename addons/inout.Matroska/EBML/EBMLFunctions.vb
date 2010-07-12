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

Namespace EBML
    Partial Public Class EbmlDocument
        Public Function GetValueUniqueElement(ByVal s As String) As Object
            Dim e As EbmlElement = GetUniqueElement(s)
            Dim o As EbmlValueElement = Nothing
            If e IsNot Nothing Then o = TryCast(e, EbmlValueElement)
            Return If(Not IsNothing(o) AndAlso Not IsNothing(o.GetValue), o.GetValue, Nothing)
        End Function

        Public Function GetUniqueElement(ByVal s As String, Optional ByVal parentElement As EbmlContainerElement = Nothing) As EbmlElement
            Dim l_doc As List(Of EbmlElement)
            If parentElement Is Nothing Then
                l_doc = Me.RootElements
            Else
                l_doc = parentElement.Children
            End If
            For Each element As EbmlElement In l_doc
                Dim container As EbmlContainerElement = TryCast(element, EbmlContainerElement)
                Dim name As String = _semantic.GetElementName(element.ID)
                If name = s Then Return element
                If container IsNot Nothing Then
                    Dim e As EbmlElement = GetUniqueElement(s, container)
                    If e IsNot Nothing Then Return e
                End If
            Next
            Return Nothing
        End Function
    End Class

End Namespace
