Public NotInheritable Class frmSplash
    Delegate Sub DelegateToCloseForm()

    ' if the welcome form is closed by the main form, it is cross-thread
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

        If Not Master.GetNETVersion Then
            MsgBox(String.Concat("Ember Media Manager requires .NET Framework version 3.5 or higher.", vbNewLine, vbNewLine, _
                       "Please install .NET Framework version 3.5 or higher before attempting to use Ember."), MsgBoxStyle.Critical, "Unsupported .NET Version")
        End If
    End Sub

End Class
