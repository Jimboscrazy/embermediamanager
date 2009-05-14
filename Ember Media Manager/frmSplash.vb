Public NotInheritable Class frmSplash

    Private Sub frmSplash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Version.Text = String.Format("Version r{0}", My.Application.Info.Version.Revision)
    End Sub

End Class
