Imports System.Windows.Forms

Public Class dlgDeleteConfirm
    Private PropogatingUp As Boolean = False
    Private PropogatingDown As Boolean = False

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
        Me.Text = Master.eLang.GetString(649, "Confirm Items To Be Deleted")
        Me.btnToggleAllFiles.Text = Master.eLang.GetString(650, "Toggle All Files")

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

                    Using SQLtransaction As SQLite.SQLiteTransaction = Master.DB.BeginTransaction 'Only on Batch Mode
                        Master.DB.DeleteFromDB(mMovie.ID, True)
                        SQLtransaction.Commit()
                    End Using

                    If MovieParentNode.Nodes.Count > 0 Then
                        For Each node As TreeNode In MovieParentNode.Nodes
                            If node.Checked Then
                                Select Case node.ImageKey
                                    Case "FOLDER"
                                        Dim oDir As New IO.DirectoryInfo(node.Tag.ToString)
                                        If oDir.Exists Then
                                            oDir.Delete(True)
                                            Exit For
                                        End If

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
        Dim mMovie As New Master.DBMovie
        Dim hadError As Boolean = False
        Dim fDeleter As New FileManip.Delete
        Dim ItemsToDelete As New List(Of IO.FileSystemInfo)
        Dim MovieParentNode As New TreeNode
        Try
            With tvwFiles


                For Each MovieId As Long In MoviesToDelete
                    hadError = False
                    mMovie = Master.DB.LoadMovieFromDB(MovieId)

                    MovieParentNode = .Nodes.Add(mMovie.ID.ToString, mMovie.ListTitle)
                    MovieParentNode.ImageKey = "MOVIE"
                    MovieParentNode.SelectedImageKey = "MOVIE"
                    MovieParentNode.Tag = mMovie

                    'get the associated files
                    ItemsToDelete = fDeleter.GetItemsToDelete(False, mMovie)

                    For Each fileItem As IO.FileSystemInfo In ItemsToDelete
                        If Not MovieParentNode.Nodes.ContainsKey(fileItem.FullName) Then
                            If TypeOf fileItem Is IO.DirectoryInfo Then
                                Try
                                    AddFolderNode(MovieParentNode, DirectCast(fileItem, IO.DirectoryInfo))
                                Catch
                                    hadError = True
                                    Exit For
                                End Try
                            Else
                                Try
                                    AddFileNode(MovieParentNode, DirectCast(fileItem, IO.FileInfo))
                                Catch
                                    hadError = True
                                    Exit For
                                End Try
                            End If
                        End If
                    Next

                    If hadError Then .Nodes.Remove(MovieParentNode)
                Next

                'check all the nodes
                For Each node As TreeNode In .Nodes
                    node.Checked = True
                    node.Expand()
                Next

            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub AddFolderNode(ByVal ParentNode As TreeNode, ByVal dir As IO.DirectoryInfo)
        Try
            Dim NewNode As TreeNode = ParentNode.Nodes.Add(dir.FullName, dir.Name)
            NewNode.Tag = dir.FullName
            NewNode.ImageKey = "FOLDER"
            NewNode.SelectedImageKey = "FOLDER"

            If Not Master.SourcesList.Contains(dir.FullName) Then
                'populate all the sub-folders in the folder
                For Each item As IO.DirectoryInfo In dir.GetDirectories
                    AddFolderNode(NewNode, item)
                Next
            End If

            'populate all the files in the folder
            For Each item As IO.FileInfo In dir.GetFiles()
                AddFileNode(NewNode, item)
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Throw
        End Try

    End Sub

    Private Sub AddFileNode(ByVal ParentNode As TreeNode, ByVal item As IO.FileInfo)
        Try
            Dim NewNode As TreeNode = ParentNode.Nodes.Add(item.FullName, item.Name)
            NewNode.Tag = item.FullName
            NewNode.ImageKey = "FILE"
            NewNode.SelectedImageKey = "FILE"
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            Throw
        End Try
    End Sub

    Private Sub tvwFiles_AfterCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwFiles.AfterCheck
        Try
            If e.Node.Parent Is Nothing Then
                'this is a movie node
                If PropogatingUp Then Return

                'check/uncheck all children
                PropogatingDown = True
                For Each node As TreeNode In e.Node.Nodes
                    node.Checked = e.Node.Checked
                Next
                PropogatingDown = False
            Else
                'this is a file/folder node
                If e.Node.Checked Then
                    If Not PropogatingUp Then
                        PropogatingDown = True
                        For Each node As TreeNode In e.Node.Nodes
                            node.Checked = True
                        Next
                        PropogatingDown = False
                    End If

                    'if all children are checked then check root node
                    For Each node As TreeNode In e.Node.Parent.Nodes
                        If Not node.Checked Then Return
                    Next
                    PropogatingUp = True
                    e.Node.Parent.Checked = True
                    PropogatingUp = False
                Else
                    If Not PropogatingUp Then
                        'uncheck any children
                        PropogatingDown = True
                        For Each node As TreeNode In e.Node.Nodes
                            node.Checked = False
                        Next
                        PropogatingDown = False
                    End If

                    'make sure parent is no longer checked
                    PropogatingUp = True
                    e.Node.Parent.Checked = False
                    PropogatingUp = False
                End If
            End If
        Catch
            'swallow this - not a critical function
        End Try
    End Sub

    Private Sub tvwFiles_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwFiles.AfterSelect
        Try
            Select Case e.Node.ImageKey
                Case "MOVIE"
                    lblNodeSelected.Text = CType(e.Node.Tag, Master.DBMovie).ListTitle
                Case "RECORD"
                    lblNodeSelected.Text = CType(e.Node.Tag, Master.DBMovie).ListTitle
                Case "FOLDER"
                    lblNodeSelected.Text = e.Node.Tag.ToString
                Case "FILE"
                    lblNodeSelected.Text = e.Node.Tag.ToString
            End Select
        Catch ex As Exception
            lblNodeSelected.Text = ""
        End Try
    End Sub

    Private Sub btnToggleAllFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToggleAllFiles.Click
        ToggleAllNodes()
    End Sub

    Private Sub ToggleAllNodes()
        Try
            Dim Checked As Nullable(Of Boolean)
            With tvwFiles
                If .Nodes.Count = 0 Then Return
                For Each node As TreeNode In .Nodes
                    If Not Checked.HasValue Then
                        'this is the first node of this type, set toggle status based on this
                        Checked = Not node.Checked
                    End If
                    node.Checked = Checked.Value
                Next
            End With
        Catch
            'swallow this - not a critical function
        End Try
    End Sub
End Class
