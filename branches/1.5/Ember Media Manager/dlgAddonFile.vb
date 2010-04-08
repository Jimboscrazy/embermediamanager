Imports System.IO

Public Class dlgAddonFile

    Public Overloads Function ShowDialog(ByVal sPath As String, ByVal sDescription As String) As KeyValuePair(Of String, String)
        Me.txtPath.Text = sPath
        Me.txtDescription.Text = sDescription

        If MyBase.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return New KeyValuePair(Of String, String)(Me.txtPath.Text, Me.txtDescription.Text)
        Else
            Return Nothing
        End If
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If Not String.IsNullOrEmpty(Me.txtPath.Text) AndAlso File.Exists(Me.txtPath.Text) AndAlso Not String.IsNullOrEmpty(Me.txtDescription.Text) Then
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Try
            Using ofdFile As New OpenFileDialog
                If ofdFile.ShowDialog() = DialogResult.OK Then
                    If ofdFile.FileName.StartsWith(Functions.AppPath) Then
                        Me.txtPath.Text = ofdFile.FileName
                    Else
                        MsgBox("Addon files must be within the current Ember installation directory.", MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, "Invalid File")
                    End If
                End If
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgAddonFile_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SetUp()
    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(272, "Addon File")
        Me.lblFile.Text = String.Concat(Master.eLang.GetString(444, "File"), ":")
        Me.lblDescription.Text = Master.eLang.GetString(172, "Description:")

        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
    End Sub
End Class
