Imports System.Windows.Forms

Public Class frmMediaSources
    Public Event ModuleSettingsChanged()

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Try

            ' Add any initialization after the InitializeComponent() call.
            LoadSources()
        Catch ex As Exception
        End Try
        SetUp()
    End Sub

    Private Sub LoadSources()
        dgvSources.Rows.Clear()
        Dim sources As Hashtable = AdvancedSettings.GetComplexSetting("MovieSources", "*EmberAPP")
        For Each sett As Object In sources.Keys
            Dim i As Integer = dgvSources.Rows.Add(New Object() {sett.ToString, sources.Item(sett.ToString)})
        Next
        dgvSources.ClearSelection()
    End Sub


    Private Sub btnAddSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSource.Click
        Dim i As Integer = dgvSources.Rows.Add(New Object() {String.Empty, String.Empty})
        dgvSources.Rows(i).Tag = False
        dgvSources.CurrentCell = dgvSources.Rows(i).Cells(0)
        dgvSources.BeginEdit(True)
        RaiseEvent ModuleSettingsChanged()
    End Sub
    Private Sub btnRemoveSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveSource.Click
        If dgvSources.SelectedCells.Count > 0 AndAlso Not Convert.ToBoolean(dgvSources.Rows(dgvSources.SelectedCells(0).RowIndex).Tag) Then
            dgvSources.Rows.RemoveAt(dgvSources.SelectedCells(0).RowIndex)
            RaiseEvent ModuleSettingsChanged()
        End If
    End Sub
    Private Sub dgvSources_CurrentCellDirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvSources.CurrentCellDirtyStateChanged
        RaiseEvent ModuleSettingsChanged()
    End Sub


    Private Sub dgvSources_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvSources.SelectionChanged
        If dgvSources.SelectedCells.Count > 0 AndAlso Not Convert.ToBoolean(dgvSources.Rows(dgvSources.SelectedCells(0).RowIndex).Tag) Then
            btnRemoveSource.Enabled = True
        Else
            btnRemoveSource.Enabled = False
        End If
    End Sub

    Private Sub dgvSources_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvSources.KeyDown
        e.Handled = (e.KeyCode = Keys.Enter)
    End Sub

    Private Sub dgvVideo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        e.Handled = (e.KeyCode = Keys.Enter)
    End Sub

    Sub SetUp()
        btnAddSource.Text = Master.eLang.GetString(28, "Add", True)

        btnRemoveSource.Text = Master.eLang.GetString(30, "Remove", True)

        Label1.Text = Master.eLang.GetString(62, "Sources")

        Me.dgvSources.Columns(0).HeaderText = Master.eLang.GetString(63, "Search String")
        Me.dgvSources.Columns(1).HeaderText = Master.eLang.GetString(64, "Source Name")

    End Sub

    Public Sub SaveChanges()
        Dim sources As New Hashtable
        For Each r As DataGridViewRow In dgvSources.Rows
            If Not String.IsNullOrEmpty(r.Cells(0).Value.ToString) AndAlso Not sources.ContainsKey(r.Cells(0).Value.ToString) Then
                sources.Add(r.Cells(0).Value.ToString, r.Cells(1).Value.ToString)
            End If
        Next
        AdvancedSettings.SetComplexSetting("MovieSources", sources, "*EmberAPP")
    End Sub

    Private Sub btnSetDefaults_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetDefaults.Click
        AdvancedSettings.SetDefaults(True, "MovieSources")
        LoadSources()
    End Sub
End Class
