Imports System.Windows.Forms
Imports System.IO
Public Class dlgRenameManual

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        OK_Button.Enabled = False
        Cancel_Button.Enabled = False
        Application.DoEvents()
        FileFolderRenamer.RenameSingle(Master.currMovie, txtFolder.Text, txtFile.Text, True, True)
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgRenameManual_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim FileName = Path.GetFileNameWithoutExtension(StringManip.CleanStackingMarkers(Master.currMovie.Filename))
        Dim stackMark As String = Path.GetFileNameWithoutExtension(Master.currMovie.Filename).Replace(FileName, String.Empty).ToLower
        If Master.currMovie.Movie.Title.ToLower.EndsWith(stackMark) Then
            FileName = Path.GetFileNameWithoutExtension(Master.currMovie.Filename)
        End If
        If Master.currMovie.isSingle Then
            txtFolder.Text = Path.GetFileName(Path.GetDirectoryName(Master.currMovie.Filename))
        Else
            txtFolder.Text = "$D"
            txtFolder.Visible = False
        End If
        txtFile.Text = FileName
    End Sub

    Private Sub txtFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFolder.TextChanged
        If txtFolder.Text <> "" AndAlso txtFile.Text <> "" Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub

    Private Sub txtFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFile.TextChanged
        If txtFolder.Text <> "" AndAlso txtFile.Text <> "" Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub
    Sub SetUp()
        Me.Text = Master.eLang.GetString(632, "Manual Rename")
        Me.Label1.Text = Master.eLang.GetString(633, "Folder Name")
        Me.Label2.Text = Master.eLang.GetString(634, "File Name")
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(19, "Close")
    End Sub
End Class
