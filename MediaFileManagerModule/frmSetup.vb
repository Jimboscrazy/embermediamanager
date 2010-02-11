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

Imports System.Windows.Forms

Public Class frmSetup
    Dim isSelected As Boolean = False
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        CheckButtons()
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged
        CheckButtons()
    End Sub
    Sub CheckButtons()
        If Not String.IsNullOrEmpty(TextBox1.Text) AndAlso Not String.IsNullOrEmpty(TextBox2.Text) AndAlso Not isSelected Then
            btnNewSet.Enabled = True
        Else
            btnNewSet.Enabled = False
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            isSelected = True
            btnRemoveSet.Enabled = True
            btnEditSet.Enabled = True
            TextBox1.Text = ListView1.SelectedItems(0).SubItems(0).Text
            TextBox2.Text = ListView1.SelectedItems(0).SubItems(1).Text
        Else
            isSelected = False
            btnRemoveSet.Enabled = False
            btnEditSet.Enabled = False
        End If
        CheckButtons()
    End Sub

    Private Sub btnNewSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewSet.Click
        Dim li As New ListViewItem
        li.text = TextBox1.Text
        li.SubItems.Add(TextBox2.Text)
        ListView1.Items.Add(li)
        TextBox1.Text = ""
        TextBox2.Text = ""
        CheckButtons()
    End Sub

    Private Sub btnRemoveSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSet.Click
        ListView1.Items.RemoveAt(ListView1.SelectedItems(0).Index)
        TextBox1.Text = ""
        TextBox2.Text = ""
        isSelected = False
        CheckButtons()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim fl As New FolderBrowserDialog
        fl.ShowDialog()
        TextBox2.Text = fl.SelectedPath
    End Sub

    Private Sub btnEditSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditSet.Click
        ListView1.SelectedItems(0).SubItems(0).Text = TextBox1.Text
        ListView1.SelectedItems(0).SubItems(1).Text = TextBox2.Text
        TextBox1.Text = ""
        TextBox2.Text = ""
        isSelected = False
        CheckButtons()
    End Sub
End Class
