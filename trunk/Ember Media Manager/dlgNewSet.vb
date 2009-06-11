Imports System.Windows.Forms

Public Class dlgNewSet

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Dim tAL As New ArrayList
        tAL = Master.eSettings.Sets

        If Not tAL.Contains(txtSetName.Text) Then
            tAL.Add(txtSetName.Text)
        End If

        Master.eSettings.Save()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Public Overloads Function ShowDialog(Optional ByVal SetName As String = "") As String

        If Not String.IsNullOrEmpty(SetName) Then txtSetName.Text = SetName

        If MyBase.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Return txtSetName.Text
        Else
            Return String.Empty
        End If
    End Function
End Class
