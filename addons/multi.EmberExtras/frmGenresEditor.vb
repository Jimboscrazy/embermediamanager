Imports System.Windows.Forms

Public Class frmGenresEditor
    Public Event ModuleSettingsChanged()
    Private Genres As New List(Of cGenre)
    Private Langs As New List(Of String)

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        GetGenres()

    End Sub

    Sub GetGenres()
        Try
            'Dim xGenre = From xGen In GenreXML...<supported>.Descendants Select xGen.Value
            'If xGenre.Count > 0 Then
            'retGenre.AddRange(xGenre.ToArray)
            'End If

            Dim xGenre = From xGen In APIXML.GenreXML...<name> Select xGen.@searchstring, xGen.@language, xGen.Value
            If xGenre.Count > 0 Then
                cbLangs.Items.Add("< All >")
                For i As Integer = 0 To xGenre.Count - 1
                    Dim genre As New cGenre
                    genre.langs.AddRange(xGenre(i).language.Split(Convert.ToChar("|")))
                    genre.searchstring = xGenre(i).searchstring
                    genre.image = xGenre(i).Value
                    Genres.Add(genre)
                    For Each s As String In xGenre(i).language.Split(Convert.ToChar("|"))
                        If Not Langs.Contains(s) Then
                            Langs.Add(s)
                            dgvLang.Rows.Add(New Object() {False, s})
                        End If
                    Next
                Next
                cbLangs.Items.AddRange(Langs.ToArray)
                cbLangs.SelectedIndex = 0
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Sub PopulateGenres()
        dgvGenres.Rows.Clear()
        ClearLangSelection()
        If cbLangs.SelectedItem.ToString = "< All >" Then
            For Each sett As cGenre In Genres
                Dim i As Integer = dgvGenres.Rows.Add(New Object() {sett.searchstring})
                dgvGenres.Rows(i).Tag = sett
            Next
        Else
            btnRemoveString.Enabled = False
            For Each sett As cGenre In Genres.Where(Function(y) y.langs.Contains(cbLangs.SelectedItem.ToString))
                Dim i As Integer = dgvGenres.Rows.Add(New Object() {sett.searchstring})
                dgvGenres.Rows(i).Tag = sett
            Next
        End If
    End Sub

    Sub ClearLangSelection()
        For Each r As DataGridViewRow In dgvLang.Rows
            r.Cells(0).Value = False
        Next
    End Sub

    Private Sub dgvGenres_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvGenres.SelectionChanged
        btnRemoveString.Enabled = False
        If Not dgvGenres.CurrentRow Is Nothing Then
            Dim g As cGenre = DirectCast(dgvGenres.CurrentRow.Tag, cGenre)
            If Not g Is Nothing Then
                ClearLangSelection()
                For Each r As DataGridViewRow In dgvLang.Rows
                    For Each s As String In g.langs
                        r.Cells(0).Value = If(r.Cells(1).Value.ToString = s, True, r.Cells(0).Value)
                    Next
                Next
                btnRemoveString.Enabled = True
            Else
                If dgvGenres.Rows.Count > 0 Then
                    dgvGenres.ClearSelection()
                End If
            End If
        Else
            ClearLangSelection()
        End If
    End Sub
    Private Sub cbLangs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLangs.SelectedIndexChanged
        PopulateGenres()
    End Sub

    Class cGenre
        Public searchstring As String
        Public langs As New List(Of String)
        Public image As String
    End Class


End Class
