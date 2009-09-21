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



Public Class dlgStudioSelect

    Private _imdbid As String = String.Empty
    Private _studio As String = String.Empty

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgStudioSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SetUp()

        Dim IMDB As New IMDB.Scraper
        Dim alStudio As ArrayList = IMDB.GetMovieStudios(Me._imdbid)

        For i As Integer = 0 To alStudio.Count - 1
            ilStudios.Images.Add(alStudio(i).ToString, XML.GetStudioImage(alStudio(i).ToString))
            Dim lvItem As ListViewItem = lvStudios.Items.Add(alStudio(i).ToString, i)
        Next

    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(223, "Select Studio")
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
    End Sub

    Public Overloads Function ShowDialog(ByVal IMDBID As String) As String

        '//
        ' Overload to pass data
        '\\

        Me._imdbid = IMDBID

        If MyBase.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return Me._studio
        Else
            Return String.Empty
        End If
    End Function

    Private Sub lvStudios_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvStudios.SelectedIndexChanged
        If lvStudios.SelectedItems.Count > 0 Then
            Me._studio = lvStudios.SelectedItems(0).Text
        End If
    End Sub

    Private Sub dlgStudioSelect_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub
End Class
