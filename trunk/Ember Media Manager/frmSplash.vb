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

Public NotInheritable Class frmSplash
    Delegate Sub DelegateToCloseForm()

    ' if the splash form, is closed by the main form, it is cross-thread
    ' operation. so we need to use the Invoke method to deal with it.
    Public Sub CloseForm()
        If (Me.InvokeRequired) Then
            Me.Invoke(New DelegateToCloseForm(AddressOf CloseForm))
        Else
            Me.Close()
        End If
    End Sub
    Private Sub frmSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Version.Text = String.Format("Version r{0}", My.Application.Info.Version.Revision)
    End Sub

End Class
