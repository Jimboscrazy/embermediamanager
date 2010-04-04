Imports System.Windows.Forms

Public Class dlgPleaseWait

    Private Sub Cancel_Button_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub dlgPleaseWait_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetUp()
    End Sub
    Sub SetUp()
        Me.Text = Master.eLang.GetString(9, "Loading Please Wait")
        Me.Label1.Text = Master.eLang.GetString(6, "Downloading details...")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel", True)
    End Sub
End Class
