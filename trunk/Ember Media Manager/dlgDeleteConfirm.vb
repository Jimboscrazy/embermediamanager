Imports System.Windows.Forms

Public Class dlgDeleteConfirm


    Private Sub dlgDeleteConfirm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SetUp()
    End Sub



    Public Overloads Function ShowDialog(ByVal MoviesToDelete As List(Of Long)) As System.Windows.Forms.DialogResult
        Populate_FileList(MoviesToDelete)
        Return MyBase.ShowDialog
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        DeleteSelectedMovies()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub


    Private Sub SetUp()
        'TODO: create language keys for new texts
        Me.Text = Master.eLang.GetString(999, "Confirm Items To Be Deleted")
        Me.btnToggleAllFiles.Text = Master.eLang.GetString(999, "Toggle All Files")
        Me.btnToggleAllRecords.Text = Master.eLang.GetString(999, "Toggle All Records")

        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
    End Sub

    Private Function DeleteSelectedMovies() As Boolean
        Dim result As Boolean = True
        Try
            With tvwFiles
                If .Nodes.Count = 0 Then Return False

                For Each MovieParentNode As TreeNode In .Nodes
                    Dim mMovie As Master.DBMovie = CType(MovieParentNode.Tag, Master.DBMovie)

                    If MovieParentNode.Nodes.Count > 0 Then
                        For Each node As TreeNode In MovieParentNode.Nodes
                            If node.Checked Then
                                Select Case node.ImageKey
                                    Case "RECORD"
                                        Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction 'Only on Batch Mode
                                            Master.DB.DeleteFromDB(mMovie.ID, True)
                                            SQLtransaction.Commit()
                                        End Using

                                    Case "FILE"
                                        Dim oFile As New IO.FileInfo(node.Tag.ToString)
                                        If oFile.Exists Then
                                            oFile.Delete()
                                        End If
                                End Select

                            End If
                        Next

                    End If

                Next
                Return result
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Function

    Private Sub Populate_FileList(ByVal MoviesToDelete As List(Of Long))
        With tvwFiles

            Dim mMovie As Master.DBMovie
            For Each MovieId As Long In MoviesToDelete
                mMovie = Master.DB.LoadMovieFromDB(MovieId)

                Dim MovieParentNode As TreeNode = .Nodes.Add(mMovie.ID.ToString, mMovie.ListTitle)
                MovieParentNode.ImageKey = "MOVIE"
                MovieParentNode.SelectedImageKey = "MOVIE"
                MovieParentNode.Tag = mMovie

                Dim MovieNode As TreeNode = MovieParentNode.Nodes.Add(mMovie.ID.ToString, Master.eLang.GetString(999, "Ember Database Record"))
                MovieNode.ImageKey = "RECORD"
                MovieNode.SelectedImageKey = "RECORD"
                MovieNode.Tag = mMovie


                'get the associated files
                Dim FilesToDelete As List(Of IO.FileInfo) = Master.GetFilesToDelete(False, mMovie)

                For Each file As IO.FileInfo In FilesToDelete
                    If Not MovieParentNode.Nodes.ContainsKey(file.FullName) Then
                        Dim NewNode As TreeNode = MovieParentNode.Nodes.Add(file.FullName, file.Name)
                        NewNode.Tag = file.FullName
                        NewNode.ImageKey = "FILE"
                        NewNode.SelectedImageKey = "FILE"
                    End If
                Next

            Next

            'check all the nodes
            For Each node As TreeNode In .Nodes
                node.Checked = True
                node.Expand()
            Next

        End With
    End Sub

    Private CodeIsChecking As Boolean = False

    Private Sub tvwFiles_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwFiles.AfterCheck
        Try
            If e.Node.Parent Is Nothing Then
                'this is a movie node
                If CodeIsChecking Then Return
                CodeIsChecking = True
                'check/uncheck all children
                For Each node As TreeNode In e.Node.Nodes
                    node.Checked = e.Node.Checked
                Next
                CodeIsChecking = False
            Else
                'this is a file node
                If e.Node.Checked Then
                    'if all children are checked then check root node
                    For Each node As TreeNode In e.Node.Parent.Nodes
                        If Not node.Checked Then Return
                    Next
                    CodeIsChecking = True
                    e.Node.Parent.Checked = True
                    CodeIsChecking = False
                Else
                    'make sure root is no longer checked
                    If CodeIsChecking Then
                        e.Node.Parent.Checked = False
                    Else
                        CodeIsChecking = True
                        e.Node.Parent.Checked = False
                        CodeIsChecking = False
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tvwFiles_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwFiles.AfterSelect
        Try
            Select Case e.Node.ImageKey
                Case "MOVIE"
                    lblNodeSelected.Text = CType(e.Node.Tag, Master.DBMovie).ListTitle
                Case "RECORD"
                    lblNodeSelected.Text = CType(e.Node.Tag, Master.DBMovie).ListTitle
                Case "FILE"
                    lblNodeSelected.Text = e.Node.Tag.ToString
            End Select
        Catch ex As Exception
            lblNodeSelected.Text = ""
        End Try
    End Sub

    Private Sub btnToggleAllRecords_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToggleAllRecords.Click
        ToggleAllNodes("RECORD")
    End Sub

    Private Sub btnToggleAllFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToggleAllFiles.Click
        ToggleAllNodes("FILE")
    End Sub

    Private Sub ToggleAllNodes(ByVal ImageKey As String)
        Try
            Dim Checked As Nullable(Of Boolean)
            With tvwFiles
                If .Nodes.Count = 0 Then Return

                For Each MovieParentNode As TreeNode In .Nodes
                    If MovieParentNode.Nodes.Count > 0 Then
                        For Each node As TreeNode In MovieParentNode.Nodes
                            If node.ImageKey = ImageKey Then
                                If Not Checked.HasValue Then
                                    'this is the first node of this type, set toggle status based on this
                                    Checked = Not node.Checked
                                End If
                                node.Checked = Checked.Value
                            End If
                        Next
                    End If

                Next
            End With

        Catch
            'swallow this - not a critical function
        End Try
    End Sub
End Class
