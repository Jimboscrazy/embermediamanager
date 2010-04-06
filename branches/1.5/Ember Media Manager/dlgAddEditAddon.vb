Imports System.Text.RegularExpressions
Imports System.IO

Public Class dlgAddEditAddon
    Dim _image As Image = Nothing
    Dim _imagecache As Image = Nothing
    Dim _addon As New Containers.Addon

    Public Overloads Function ShowDialog(ByVal Addon As Containers.Addon) As Containers.Addon
        If Not Addon.ID = -1 Then
            Me.Text = String.Concat("Edit Addon - ", Addon.Name)
            Me.txtName.Text = Addon.Name
            Me.txtDescription.Text = Addon.Description
            Me.txtVersion.Text = Addon.Version.ToString
            Me.txtMinEVersion.Text = Addon.MinEVersion.ToString
            Me.txtMaxEVersion.Text = Addon.MaxEVersion.ToString
            Me.cboCategory.Text = Addon.Category
            Me.pbScreenShot.Image = Addon.ScreenShotImage

            Dim lvItem As New ListViewItem
            For Each _file As KeyValuePair(Of String, String) In Addon.Files
                lvItem = lvFiles.Items.Add(Path.Combine(Functions.AppPath, _file.Key.Replace("/", Path.DirectorySeparatorChar)))
                lvItem.SubItems.Add(_file.Value)
            Next
        Else
            Me.Text = "New Addon"
        End If

        If MyBase.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return _addon
        Else
            Return Nothing
        End If
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If ValidateEntry() Then

            Me._addon.Name = Me.txtName.Text
            Me._addon.Description = Me.txtDescription.Text
            Me._addon.Version = NumUtils.ConvertToSingle(Me.txtVersion.Text)
            Me._addon.MinEVersion = NumUtils.ConvertToSingle(Me.txtMinEVersion.Text)
            Me._addon.MaxEVersion = NumUtils.ConvertToSingle(Me.txtMaxEVersion.Text)
            Me._addon.Category = Me.cboCategory.Text

            If String.IsNullOrEmpty(Me.txtScreenShotPath.Text) Then
                Me._addon.ScreenShotPath = "!KEEP!"
            Else
                Me._addon.ScreenShotPath = Me.txtScreenShotPath.Text
            End If

            For Each lvItem As ListViewItem In lvFiles.Items
                Me._addon.Files.Add(lvItem.Text, lvItem.SubItems(1).Text)
            Next

            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Version_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtVersion.KeyPress, txtMinEVersion.KeyPress, txtMaxEVersion.KeyPress
        e.Handled = StringUtils.NumericOnly(e.KeyChar, True) OrElse (e.KeyChar = "." AndAlso Regex.Matches(DirectCast(sender, TextBox).Text, "\.").Count = 1)
    End Sub

    Private Function ValidateEntry() As Boolean
        Return Not String.IsNullOrEmpty(Me.txtName.Text) AndAlso Not String.IsNullOrEmpty(Me.txtDescription.Text) AndAlso _
        (Not String.IsNullOrEmpty(Me.txtVersion.Text) AndAlso Convert.ToSingle(Me.txtVersion.Text) > 0) AndAlso _
        ValidateFiles() AndAlso ValidateSS(Me.txtScreenShotPath.Text)
    End Function

    Private Function ValidateFiles() As Boolean
        For Each lvItem As ListViewItem In lvFiles.Items
            If Not File.Exists(lvItem.Text) Then Return False
        Next
    End Function

    Private Function ValidateSS(ByVal ssPath As String) As Boolean
        Dim bReturn As Boolean = False
        Try
            If Not String.IsNullOrEmpty(ssPath) Then
                Dim fInfo As New FileInfo(ssPath)
                If fInfo.Exists AndAlso fInfo.Length <= 153600 Then
                    Me._image = Image.FromFile(ssPath)
                    If Me._image.Width <= 133 AndAlso Me._image.Height <= 95 Then
                        bReturn = True
                    End If
                End If
                fInfo = Nothing
            ElseIf Not IsNothing(Me._imagecache) Then
                bReturn = True
            End If
        Catch
        End Try
        Return bReturn
    End Function

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Try
            Using ofdImage As New OpenFileDialog
                With ofdImage
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                    .Filter = "Supported Images(*.jpg, *.jpeg)|*.jpg;*.jpeg|jpeg (*.jpg, *.jpeg)|*.jpg;*.jpeg"
                    .FilterIndex = 0
                End With

                If ofdImage.ShowDialog() = DialogResult.OK Then
                    If ValidateSS(ofdImage.FileName) Then
                        Me.txtScreenShotPath.Text = ofdImage.FileName
                        Me.pbScreenShot.Image = Me._image
                    Else
                        Me.txtScreenShotPath.Text = String.Empty
                        Me.pbScreenShot.Image = Me._imagecache
                        Me._image = Nothing
                    End If
                End If
            End Using
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Me.DeleteFiles()
    End Sub

    Private Sub DeleteFiles()
        Try
            If Me.lvFiles.Items.Count > 0 Then
                While Me.lvFiles.SelectedItems.Count > 0
                    Me.lvFiles.Items.Remove(Me.lvFiles.SelectedItems(0))
                End While
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub lvFiles_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvFiles.DoubleClick
        Me.EditFile()
    End Sub

    Private Sub lvFiles_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lvFiles.KeyDown
        If e.KeyCode = Keys.Delete Then Me.DeleteFiles()
    End Sub

    Private Sub btnAddFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFile.Click
        Using dNewFile As New dlgAddonFile
            Dim KVP As KeyValuePair(Of String, String) = dNewFile.ShowDialog(String.Empty, String.Empty)
            If Not IsNothing(KVP.Key) Then
                If IsNothing(lvFiles.FindItemWithText(KVP.Key)) Then
                    Dim lvItem As ListViewItem = lvFiles.Items.Add(KVP.Key)
                    lvItem.SubItems.Add(KVP.Value)
                End If
            End If
        End Using
    End Sub

    Private Sub btnEditFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditFile.Click
        Me.EditFile()
    End Sub

    Private Sub EditFile()
        If lvFiles.SelectedItems.Count > 0 Then
            Dim lvItem As ListViewItem = lvFiles.SelectedItems(0)
            Using dEditFile As New dlgAddonFile
                Dim KVP As KeyValuePair(Of String, String) = dEditFile.ShowDialog(lvItem.Text, lvItem.SubItems(1).Text)
                If Not IsNothing(KVP.Key) Then
                    lvItem.Text = KVP.Key
                    lvItem.SubItems(1).Text = KVP.Value
                End If
            End Using
        End If
    End Sub
End Class
