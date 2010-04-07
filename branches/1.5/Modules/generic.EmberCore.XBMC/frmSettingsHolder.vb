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
        Using dlg As New dlgXBMCHost
            dlg.XComs = XComs
            If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                XComs = dlg.XComs
                RaiseEvent ModuleSettingsChanged()
                Me.LoadXComs()
            End If
        End Using
    End Sub

    Private Sub btnEditCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditCom.Click
        Dim iSel As Integer = Me.lbXBMCCom.SelectedIndex

        Using dlg As New dlgXBMCHost
            dlg.XComs = XComs
            dlg.hostid = Me.lbXBMCCom.SelectedItem.ToString
            If dlg.ShowDialog() = Windows.Forms.DialogResult.OK Then
                XComs = dlg.XComs
                RaiseEvent ModuleSettingsChanged()
                Me.LoadXComs()
            End If
        End Using
    End Sub

    Private Sub cbEnabled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEnabled.CheckedChanged
        RaiseEvent ModuleEnabledChanged(cbEnabled.Checked, 0)
    End Sub

    Public Sub New()
        InitializeComponent()
        Me.SetUp()
        chkRealTime.Checked = AdvancedSettings.GetBooleanSetting("XBMCSync", False)
    End Sub

    Private Sub lbXBMCCom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lbXBMCCom.KeyDown
        If e.KeyCode = Keys.Delete Then Me.RemoveXCom()
    End Sub

    Private Sub lbXBMCCom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbXBMCCom.SelectedIndexChanged
        Try
            If Me.lbXBMCCom.SelectedItems.Count > 0 Then
                btnEditCom.Enabled = True
            Else
                btnEditCom.Enabled = False
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub RemoveXCom()
        If Me.lbXBMCCom.SelectedItems.Count > 0 Then
            Me.XComs.Remove(Me.XComs.FirstOrDefault(Function(y) y.Name = Me.lbXBMCCom.SelectedItem.ToString))
            LoadXComs()
            RaiseEvent ModuleSettingsChanged()
        End If
    End Sub

    Sub SetUp()
        Me.GroupBox11.Text = Master.eLang.GetString(9, "XBMC Communication")
        Me.btnEditCom.Text = Master.eLang.GetString(10, "Edit")
        Me.btnAddCom.Text = Master.eLang.GetString(12, "Add New")
        Me.btnRemoveCom.Text = Master.eLang.GetString(15, "Remove Selected")
        Me.cbEnabled.Text = Master.eLang.GetString(774, "Enabled", True)
        Me.chkRealTime.Text = Master.eLang.GetString(20, "Enable Real Time synchronization")
    End Sub

    Private Sub btnRemoveCom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveCom.Click
        RemoveXCom()
    End Sub
    Private Sub chkRealTime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRealTime.CheckedChanged
        AdvancedSettings.SetBooleanSetting("XBMCSync", chkRealTime.Checked)
    End Sub

    #End Region 'Methods



End Class