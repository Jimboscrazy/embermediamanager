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

Option Explicit On

Public Class dlgNewVersion

    Private Sub llClick_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llClick.LinkClicked
        Process.Start("http://code.google.com/p/embermediamanager/downloads/list")
    End Sub

    Public Overloads Function ShowDialog(ByVal iNew As Integer) As Windows.Forms.DialogResult

        '//
        ' Overload to pass data
        '\\

        Me.lblNew.Text = String.Format(Master.eLang.GetString(210, "Version r{0} is now available on Ember Media Manager's Google Code page."), iNew)
        Me.txtChangelog.Text = Master.GetChangelog.Replace("\n", vbNewLine)

        Return MyBase.ShowDialog()
    End Function

    Private Sub dlgNewVersion_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub

    Private Sub dlgNewVersion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SetUp()
    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(209, "A New Version Is Available")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.llClick.Text = Master.eLang.GetString(211, "Click Here")
        Me.Label2.Text = Master.eLang.GetString(212, "to visit the Google Code page.")
    End Sub
End Class
