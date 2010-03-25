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

Imports EmberAPI

Public Class frmSettingsHolder

    #Region "Fields"

    Public XComs As New List(Of XBMCxCom.XBMCCom)

    #End Region 'Fields

    #Region "Events"

    Public Event ModuleEnabledChanged(ByVal State As Boolean, ByVal difforder As Integer)

    Public Event ModuleSettingsChanged()

    #End Region 'Events

    #Region "Methods"

    Public Sub LoadXComs()
        Me.lbXBMCCom.Items.Clear()
        For Each x As XBMCxCom.XBMCCom In Me.XComs
            Me.lbXBMCCom.Items.Add(x.Name)
        Next
    End Sub

    Private Sub btnAddCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddCom.Click
        If Not String.IsNullOrEmpty(txtName.Text) Then

            'have to iterate the list instead of using .comtains so we can convert each to lower case
            For i As Integer = 0 To lbXBMCCom.Items.Count - 1
                If lbXBMCCom.Items(i).ToString.ToLower = Me.txtName.Text.ToLower Then
                    MsgBox(Master.eLang.GetString(1, "The name you are attempting to use for this XBMC installation is already in use. Please choose another."), MsgBoxStyle.Exclamation, Master.eLang.GetString(2, "Each name must be unique"))
                    txtName.Focus()
                    Exit Sub
                End If
            Next

            If Not String.IsNullOrEmpty(txtIP.Text) Then
                If Not String.IsNullOrEmpty(txtPort.Text) Then
                    XComs.Add(New XBMCxCom.XBMCCom With {.Name = txtName.Text, .IP = txtIP.Text, .Port = txtPort.Text, .Username = txtUsername.Text, .Password = txtPassword.Text})
                    Me.LoadXComs()

                    Me.txtName.Text = String.Empty
                    Me.txtIP.Text = String.Empty
                    Me.txtPort.Text = String.Empty
                    Me.txtUsername.Text = String.Empty
                    Me.txtPassword.Text = String.Empty

                    Me.btnEditCom.Enabled = False
                    RaiseEvent ModuleSettingsChanged()
                Else
                    MsgBox(Master.eLang.GetString(3, "You must enter a port for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(4, "Please Enter a Port"))
                    txtPort.Focus()
                End If
            Else
                MsgBox(Master.eLang.GetString(5, "You must enter an IP for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(6, "Please Enter an IP"))
                txtIP.Focus()
            End If
        Else
            MsgBox(Master.eLang.GetString(7, "You must enter a name for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(8, "Please Enter a Unique Name"))
            txtName.Focus()
        End If
    End Sub

    Private Sub btnEditCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditCom.Click
        Dim iSel As Integer = Me.lbXBMCCom.SelectedIndex

        If Not String.IsNullOrEmpty(txtName.Text) Then

            For i As Integer = 0 To lbXBMCCom.Items.Count - 1
                If Not iSel = i AndAlso lbXBMCCom.Items(i).ToString.ToLower = Me.txtName.Text.ToLower Then
                    MsgBox(Master.eLang.GetString(1, "The name you are attempting to use for this XBMC installation is already in use. Please choose another."), MsgBoxStyle.Exclamation, Master.eLang.GetString(2, "Each name must be unique"))
                    txtName.Focus()
                    Exit Sub
                End If
            Next

            If Not String.IsNullOrEmpty(txtIP.Text) Then
                If Not String.IsNullOrEmpty(txtPort.Text) Then

                    Me.XComs.Item(iSel).Name = Me.txtName.Text
                    Me.XComs.Item(iSel).IP = Me.txtIP.Text
                    Me.XComs.Item(iSel).Port = Me.txtPort.Text
                    Me.XComs.Item(iSel).Username = Me.txtUsername.Text
                    Me.XComs.Item(iSel).Password = Me.txtPassword.Text

                    btnEditCom.Enabled = False

                    Me.txtName.Text = String.Empty
                    Me.txtIP.Text = String.Empty
                    Me.txtPort.Text = String.Empty
                    Me.txtUsername.Text = String.Empty
                    Me.txtPassword.Text = String.Empty
                    Me.LoadXComs()
                    'Me.SetApplyButton(True)
                    RaiseEvent ModuleSettingsChanged()
                Else
                    MsgBox(Master.eLang.GetString(3, "You must enter a port for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(4, "Please Enter a Port"))
                    txtPort.Focus()
                End If
            Else
                MsgBox(Master.eLang.GetString(5, "You must enter an IP for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(6, "Please Enter an IP"))
                txtIP.Focus()
            End If

        Else
            MsgBox(Master.eLang.GetString(7, "You must enter a name for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(8, "Please Enter a Unique Name"))
            txtName.Focus()
        End If

    End Sub

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEnabled.CheckedChanged
        RaiseEvent ModuleEnabledChanged(cbEnabled.Checked, 0)
    End Sub

    Private Sub frmSettingsHolder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetUp()
    End Sub

    Private Sub lbXBMCCom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lbXBMCCom.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveXCom()
    End Sub

    Private Sub lbXBMCCom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbXBMCCom.SelectedIndexChanged
        Dim iSel As Integer = Me.lbXBMCCom.SelectedIndex

        Me.txtName.Text = Me.XComs.Item(iSel).Name
        Me.txtIP.Text = Me.XComs.Item(iSel).IP
        Me.txtPort.Text = Me.XComs.Item(iSel).Port
        Me.txtUsername.Text = Me.XComs.Item(iSel).Username
        Me.txtPassword.Text = Me.XComs.Item(iSel).Password

        btnEditCom.Enabled = True
    End Sub

    Private Sub RemoveXCom()
        If Me.lbXBMCCom.SelectedItems.Count > 0 Then
            Me.XComs.RemoveAt(Me.lbXBMCCom.SelectedIndex)
            LoadXComs()
            RaiseEvent ModuleSettingsChanged()
        End If
    End Sub

    Sub SetUp()
        Me.GroupBox11.Text = Master.eLang.GetString(9, "XBMC Communication")
        Me.btnEditCom.Text = Master.eLang.GetString(10, "Commit Edit")
        Me.Label16.Text = Master.eLang.GetString(11, "Name:")
        Me.btnAddCom.Text = Master.eLang.GetString(12, "Add New")
        Me.Label13.Text = Master.eLang.GetString(425, "Username:", True)
        Me.Label14.Text = Master.eLang.GetString(426, "Password:", True)
        Me.Label7.Text = Master.eLang.GetString(13, "XBMC IP:")
        Me.Label6.Text = Master.eLang.GetString(14, "XBMC Port:")
        Me.btnRemoveCom.Text = Master.eLang.GetString(15, "Remove Selected", True)
    End Sub

    Private Sub btnRemoveCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveCom.Click
        RemoveXCom()
    End Sub
    #End Region 'Methods


End Class