﻿Imports System.Windows.Forms
Imports System.Xml.Serialization
Imports System.IO

Public Class frmGenresEditor
    Public Event ModuleSettingsChanged()
    Private xmlGenres As xGenres

    Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        xmlGenres = xGenres.Load(Path.Combine(Functions.AppPath, String.Format("Images{0}Genres{0}Genres.xml", Path.DirectorySeparatorChar)))
        GetLanguages()
        'xmlGenres.Save(Path.Combine(Functions.AppPath, "Images\Genres\Genres2.xml"))
    End Sub

    Sub GetLanguages()
        Try
            cbLangs.Items.Add("< All >")
            cbLangs.Items.AddRange(xmlGenres.listOfLanguages.ToArray)
            cbLangs.SelectedIndex = 0
            For Each s As String In xmlGenres.listOfLanguages
                dgvLang.Rows.Add(New Object() {False, s})
            Next
            dgvLang.ClearSelection()
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Sub PopulateGenres()
        Try
            dgvGenres.Rows.Clear()
            ClearLangSelection()
            If cbLangs.SelectedItem.ToString = "< All >" Then
                For Each sett As xGenre In xmlGenres.listOfGenres
                    Dim i As Integer = dgvGenres.Rows.Add(New Object() {sett.searchstring})
                    dgvGenres.Rows(i).Tag = sett
                Next
            Else
                btnRemoveGenre.Enabled = False
                For Each sett As xGenre In xmlGenres.listOfGenres.Where(Function(y) y.Langs.Contains(cbLangs.SelectedItem.ToString))
                    Dim i As Integer = dgvGenres.Rows.Add(New Object() {sett.searchstring})
                    dgvGenres.Rows(i).Tag = sett
                Next
            End If
        Catch ex As Exception
        End Try
    End Sub

    Sub ClearLangSelection()
        For Each r As DataGridViewRow In dgvLang.Rows
            r.Cells(0).Value = False
        Next
    End Sub

    Private Sub dgvGenres_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvGenres.SelectionChanged
        btnRemoveGenre.Enabled = False
        btnChangeImg.Enabled = False
        pbIcon.Image = Nothing
        Try

            dgvLang.ClearSelection()
            If dgvGenres.SelectedCells.Count > 0 Then
                Dim g As xGenre = DirectCast(dgvGenres.CurrentRow.Tag, xGenre)
                If Not g Is Nothing Then
                    ClearLangSelection()
                    For Each r As DataGridViewRow In dgvLang.Rows
                        For Each s As String In g.Langs
                            r.Cells(0).Value = If(r.Cells(1).Value.ToString = s, True, r.Cells(0).Value)
                        Next
                    Next
                    btnRemoveGenre.Enabled = True
                    btnChangeImg.Enabled = True
                    If g.icon Is Nothing Then
                        pbIcon.Image = Nothing
                    Else
                        pbIcon.Load(Path.Combine(Functions.AppPath, String.Format("Images{0}Genres{0}{1}", Path.DirectorySeparatorChar, g.icon)))
                    End If

                Else
                    If dgvGenres.Rows.Count > 0 Then
                        dgvGenres.ClearSelection()
                    End If
                End If
            Else
                ClearLangSelection()
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

    End Sub

    Private Sub dgvLang_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvLang.SelectionChanged
        If dgvLang.SelectedRows.Count > 0 AndAlso Not dgvLang.CurrentRow.Cells(1).Value Is Nothing Then
            btnRemoveLang.Enabled = True
        Else
            btnRemoveLang.Enabled = False
        End If
    End Sub

    Private Sub cbLangs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbLangs.SelectedIndexChanged
        PopulateGenres()
    End Sub

    Private Sub btnAddGenre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddGenre.Click
        Dim g As New xGenre
        Dim i As Integer = dgvGenres.Rows.Add(New Object() {String.Empty})
        dgvGenres.Rows(i).Tag = g
        xmlGenres.listOfGenres.Add(g)
        dgvGenres.CurrentCell = dgvGenres.Rows(i).Cells(0)
        dgvGenres.BeginEdit(True)
    End Sub

    Private Sub btnAddLang_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddLang.Click
        Dim i As Integer = dgvLang.Rows.Add(New Object() {False, String.Empty})
        dgvLang.CurrentCell = dgvLang.Rows(i).Cells(1)
        dgvLang.BeginEdit(True)
    End Sub

#Region "Nested Types"
    <XmlRoot("genres")> _
    Class xGenres
        <XmlArray("supported")> _
        <XmlArrayItem("language")> _
        Public listOfLanguages As New List(Of String)
        <XmlElement("name")> _
        Public listOfGenres As New List(Of xGenre)
        <XmlElement("default")> _
        <XmlText()> _
        Public icon As String

        Public Shared Function Load(ByVal fpath As String) As xGenres
            Dim conf As xGenres = Nothing
            Try
                If Not File.Exists(fpath) Then Return New xGenres
                Dim xmlSer As XmlSerializer
                xmlSer = New XmlSerializer(GetType(xGenres))
                Using xmlSW As New StreamReader(Path.Combine(Functions.AppPath, fpath))
                    conf = DirectCast(xmlSer.Deserialize(xmlSW), xGenres)
                End Using
                For i As Integer = 0 To conf.listOfGenres.Count - 1
                    conf.listOfGenres(i).Langs.AddRange(conf.listOfGenres(i).language.Split(Convert.ToChar("|")))
                Next
            Catch ex As Exception
            End Try
            Return conf
        End Function
        Public Sub Save(ByVal fpath As String)
            Dim xmlSer As New XmlSerializer(GetType(xGenres))
            Using xmlSW As New StreamWriter(fpath)
                xmlSer.Serialize(xmlSW, Me)
            End Using
        End Sub
    End Class
    Class xGenre
        <XmlIgnore()> _
        Public Langs As New List(Of String)
        <XmlAttribute("searchstring")> _
        Public searchstring As String
        <XmlAttribute("language")> _
        Public language As String
        <XmlText()> _
        <XmlElement()> _
        Public icon As String
    End Class
#End Region 'Nested Types



End Class
