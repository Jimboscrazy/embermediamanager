Imports System.Text.RegularExpressions

Public Class dlgAddons
    Friend WithEvents bwDownload As New System.ComponentModel.BackgroundWorker

    Private SessionID As String = String.Empty
    Private currType As String = String.Empty

    Private AddonItem() As AddonItem

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub dlgAddons_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim iBackground As New Bitmap(Me.pnlCurrent.Width, Me.pnlCurrent.Height)
        Using b As Graphics = Graphics.FromImage(iBackground)
            b.FillRectangle(New Drawing2D.LinearGradientBrush(Me.pnlCurrent.ClientRectangle, Color.SteelBlue, Color.FromKnownColor(KnownColor.Control), Drawing2D.LinearGradientMode.Horizontal), pnlCurrent.ClientRectangle)
            Me.pnlCurrent.BackgroundImage = iBackground
        End Using

        Me.txtUsername.Text = Master.eSettings.Username
        Me.txtPassword.Text = Master.eSettings.Password

        If Not String.IsNullOrEmpty(Me.txtUsername.Text) AndAlso Not String.IsNullOrEmpty(txtPassword.Text) Then
            Me.Login()
        End If
    End Sub

    Function GetStatus(ByVal status As String) As String
        Dim regStat As Match = Regex.Match(status, "\<status\>(?<status>.*?)\<\/status\>", RegexOptions.IgnoreCase)
        If regStat.Success Then
            Dim tStatus As String = regStat.Groups("status").Value
            If Not String.IsNullOrEmpty(tStatus) Then
                Return tStatus
            End If
        End If
        Return String.Empty
    End Function

    Private Sub DoUpload(ByVal tAddon As Containers.Addon)
        Dim sHTTP As New HTTP
        Dim postData As New List(Of String())
        postData.Add((New String() {"username", Me.txtUsername.Text}))
        postData.Add((New String() {"password", Me.txtPassword.Text}))
        postData.Add((New String() {"func", "add"}))
        postData.Add((New String() {"id", If(tAddon.ID > -1, tAddon.ID.ToString, String.Empty)}))
        postData.Add((New String() {"Name", tAddon.Name}))
        postData.Add((New String() {"Description", tAddon.Description}))
        postData.Add((New String() {"Category", tAddon.Category}))
        postData.Add((New String() {"AddonVersion", tAddon.Version.ToString}))
        postData.Add((New String() {"EmberVersion_Min", tAddon.MinEVersion.ToString}))
        postData.Add((New String() {"EmberVersion_Max", tAddon.MaxEVersion.ToString}))
        postData.Add((New String() {"screenshot", tAddon.ScreenShotPath}))
        If Not tAddon.ScreenShotPath = "!KEEP!" Then
            postData.Add((New String() {tAddon.ScreenShotPath, tAddon.ScreenShotPath, "file"}))
        End If
        Me.SessionID = sHTTP.PostDownloadData("http://www.embermm.com/addons/addons.php", postData)

        If IsNumeric(GetStatus(Me.SessionID)) Then
            If Not tAddon.ID > -1 Then tAddon.ID = Convert.ToInt32(GetStatus(Me.SessionID))
            For Each f As String In tAddon.DeleteFiles
                postData.Clear()
                postData.Add((New String() {"username", Me.txtUsername.Text}))
                postData.Add((New String() {"password", Me.txtPassword.Text}))
                postData.Add((New String() {"func", "deletefile"}))
                postData.Add((New String() {"addon_id", tAddon.ID.ToString}))
                postData.Add((New String() {"filename", f.Substring(Functions.AppPath.Length).Replace(System.IO.Path.DirectorySeparatorChar, "/")}))
                Me.SessionID = sHTTP.PostDownloadData("http://www.embermm.com/addons/addons.php", postData)
                If IsNumeric(GetStatus(Me.SessionID)) Then
                    'ok
                End If
            Next
            For Each f As Generic.KeyValuePair(Of String, String) In tAddon.Files
                postData.Clear()
                postData.Add((New String() {"username", Me.txtUsername.Text}))
                postData.Add((New String() {"password", Me.txtPassword.Text}))
                postData.Add((New String() {"func", "addfile"}))
                postData.Add((New String() {"addon_id", tAddon.ID.ToString}))
                postData.Add((New String() {"Description", f.Value}))
                postData.Add((New String() {"Filename", f.Key.Substring(Functions.AppPath.Length).Replace(System.IO.Path.DirectorySeparatorChar, "/")}))
                postData.Add((New String() {System.IO.Path.GetFileName(f.Key), f.Key, "file"}))
                Me.SessionID = sHTTP.PostDownloadData("http://www.embermm.com/addons/addons.php", postData)
                If IsNumeric(GetStatus(Me.SessionID)) Then
                    'ok
                End If
            Next
        Else
            'error
        End If

        If Me.currType = tAddon.Category Then Me.RefreshItems()
    End Sub

    Private Sub tsCategories_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles tsCategories.ItemClicked
        If e.ClickedItem.Text = "Create New" Then
            Using dNewAddon As New dlgAddEditAddon
                Dim tAddon As Containers.Addon = dNewAddon.ShowDialog(New Containers.Addon)
                If Not IsNothing(tAddon) Then
                    Me.DoUpload(tAddon)
                End If
            End Using
        Else
            If Not Me.currType = e.ClickedItem.Text Then
                Me.currType = e.ClickedItem.Text
                Me.pbCurrent.Image = e.ClickedItem.Image
                Me.lblCurrent.Text = e.ClickedItem.Text
                Me.LoadItems(e.ClickedItem.Text)
            End If
        End If
    End Sub

    Public Sub LoadItems(ByVal sType As String)
        Me.ClearList()

        Me.tsCategories.Enabled = False
        Me.lblStatus.Text = String.Format("Fetching ""{0}"" Addons...", sType)
        Me.pnlStatus.Visible = True

        Me.bwDownload = New System.ComponentModel.BackgroundWorker
        Me.bwDownload.WorkerReportsProgress = True
        Me.bwDownload.RunWorkerAsync(sType)
    End Sub

    Public Sub RefreshItems()
        Me.LoadItems(Me.currType)
    End Sub

    Private Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Me.Login()
    End Sub

    Private Sub Login()
        Dim sHTTP As New HTTP

        Me.SessionID = String.Empty

        Me.pnlLogin.Visible = False
        Me.lblStatus.Text = "Logging in..."
        Me.pnlStatus.Visible = True

        Application.DoEvents()
        Dim postData As New List(Of String())
        postData.Add((New String() {"username", Me.txtUsername.Text}))
        postData.Add((New String() {"password", Me.txtPassword.Text}))
        postData.Add((New String() {"func", "login"}))
        Me.SessionID = sHTTP.PostDownloadData("http://www.embermm.com/addons/addons.php", postData)
        If Not String.IsNullOrEmpty(Me.SessionID) AndAlso Me.SessionID.Contains("OK") Then
            Me.pnlStatus.Visible = False
            Me.tsCategories.Enabled = True
            Master.eSettings.Username = Me.txtUsername.Text
            Master.eSettings.Password = Me.txtPassword.Text
            Master.eSettings.Save()
        Else
            Me.pnlStatus.Visible = False
            Me.pnlLogin.Visible = True
        End If

        sHTTP = Nothing
    End Sub

    Private Sub bwDownload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownload.DoWork
        Dim aoXML As String = String.Empty
        Dim sHTTP As New HTTP
        Dim postData As New List(Of String())
        postData.Add((New String() {"username", Me.txtUsername.Text}))
        postData.Add((New String() {"password", Me.txtPassword.Text}))
        postData.Add((New String() {"type", e.Argument.ToString}))
        postData.Add((New String() {"func", "fetch"}))
        aoXML = sHTTP.PostDownloadData("http://www.embermm.com/addons/addons.php", postData)

        If Not String.IsNullOrEmpty(aoXML) Then
            Dim xdAddons As XDocument = XDocument.Parse(aoXML)
            Dim tTop As Integer = 0
            Dim iIndex As Integer = 0

            For Each xAddon In xdAddons.Descendants("entry")
                If AllowedVersion(xAddon.Element("EmberVersion_Min").Value, xAddon.Element("EmberVersion_Max").Value) Then
                    ReDim Preserve Me.AddonItem(iIndex)
                    Me.AddonItem(iIndex) = New AddonItem
                    Me.AddonItem(iIndex).ID = Convert.ToInt32(xAddon.Element("id").Value)
                    Me.AddonItem(iIndex).AddonName = xAddon.Element("Name").Value
                    Me.AddonItem(iIndex).Author = xAddon.Element("User").Value
                    Me.AddonItem(iIndex).Version = NumUtils.ConvertToSingle(xAddon.Element("AddonVersion").Value)
                    Me.AddonItem(iIndex).MinEVersion = NumUtils.ConvertToSingle(xAddon.Element("EmberVersion_Min").Value)
                    Me.AddonItem(iIndex).MaxEVersion = NumUtils.ConvertToSingle(xAddon.Element("EmberVersion_Max").Value)
                    Me.AddonItem(iIndex).Summary = xAddon.Element("Description").Value
                    Me.AddonItem(iIndex).Category = e.Argument.ToString
                    sHTTP.StartDownloadImage(String.Format("http://www.embermm.com/addons/addons.php?screenshot={0}", xAddon.Element("id").Value))
                    While sHTTP.IsDownloading
                        Application.DoEvents()
                    End While
                    Me.AddonItem(iIndex).ScreenShot = sHTTP.Image

                    Dim fList As New Generic.SortedList(Of String, String)
                    For Each fFile As XElement In xAddon.Descendants("file")
                        fList.Add(fFile.Element("Filename").Value, fFile.Element("Description").Value)
                    Next
                    Me.AddonItem(iIndex).FileList = fList

                    Me.AddonItem(iIndex).Left = 0
                    Me.AddonItem(iIndex).Top = tTop

                    Me.bwDownload.ReportProgress(0, Me.AddonItem(iIndex))

                    tTop += 105
                    iIndex += 1
                End If
            Next
        End If

        sHTTP = Nothing

    End Sub

    Private Function AllowedVersion(ByVal MinVersion As String, ByVal MaxVersion As String) As Boolean
        Dim MinAllowed As Boolean = False
        Dim MaxAllowed As Boolean = False

        If String.IsNullOrEmpty(MinVersion) OrElse NumUtils.ConvertToSingle(MinVersion) <= Master.MajorVersion Then
            MinAllowed = True
        End If

        If String.IsNullOrEmpty(MaxVersion) OrElse NumUtils.ConvertToSingle(MaxVersion) >= Master.MajorVersion Then
            MaxAllowed = True
        End If

        Return MinAllowed AndAlso MaxAllowed
    End Function

    Private Sub bwDownload_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwDownload.ProgressChanged
        Dim tAOI As AddonItem = DirectCast(e.UserState, AddonItem)
        AddHandler tAOI.NeedsRefresh, AddressOf Me.RefreshItems
        AddHandler tAOI.SendEdit, AddressOf Me.DoUpload
        tAOI.Owned = tAOI.Author = Master.eSettings.Username AndAlso Not String.IsNullOrEmpty(Me.SessionID)
        Me.pnlList.Controls.Add(tAOI)
    End Sub

    Private Sub bwDownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownload.RunWorkerCompleted
        Me.tsCategories.Enabled = True
        Me.pnlStatus.Visible = False
    End Sub

    Private Sub ClearList()
        If Me.pnlList.Controls.Count > 0 Then
            For i As Integer = UBound(Me.AddonItem) To 0 Step -1
                If Not IsNothing(Me.AddonItem(i)) Then
                    RemoveHandler Me.AddonItem(i).NeedsRefresh, AddressOf Me.RefreshItems
                    RemoveHandler Me.AddonItem(i).SendEdit, AddressOf Me.DoUpload
                    Me.pnlList.Controls.Remove(Me.AddonItem(i))
                End If
            Next
        End If
    End Sub
End Class
