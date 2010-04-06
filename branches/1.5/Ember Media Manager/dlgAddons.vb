Public Class dlgAddons
    Friend WithEvents bwDownload As New System.ComponentModel.BackgroundWorker

    Private SessionID As String = String.Empty

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

    Private Sub tsCategories_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles tsCategories.ItemClicked
        Me.ClearList()

        Me.pbCurrent.Image = e.ClickedItem.Image
        Me.lblCurrent.Text = e.ClickedItem.Text

        Select Case e.ClickedItem.Text
            Case "Translations"
            Case "Themes"
            Case "Templates"
            Case "Modules"
            Case "Other"
        End Select

        Me.tsCategories.Enabled = False
        Me.lblStatus.Text = String.Format("Fetching ""{0}"" Addons...", e.ClickedItem.Text)
        Me.pnlStatus.Visible = True

        Me.bwDownload = New System.ComponentModel.BackgroundWorker
        Me.bwDownload.WorkerReportsProgress = True
        Me.bwDownload.RunWorkerAsync(e.ClickedItem.Text)

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

        Me.SessionID = sHTTP.DownloadData(String.Format("http://www.embermm.com/addons/addons.php?username={0}&password={1}", Me.txtUsername.Text, Me.txtPassword.Text))

        If Not String.IsNullOrEmpty(Me.SessionID) Then
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
        aoXML = sHTTP.DownloadData(String.Format("http://www.embermm.com/addons/addons.php?type={0}", e.Argument.ToString))

        If Not String.IsNullOrEmpty(aoXML) Then
            Dim xdAddons As XDocument = XDocument.Parse(aoXML)
            Dim tTop As Integer = 0
            Dim iIndex As Integer = 0

            For Each xAddon In xdAddons.Descendants("entry")
                If AllowedVersion(xAddon.Element("EmberVersion_Min").Value, xAddon.Element("EmberVersion_Max").Value) Then
                    ReDim Preserve Me.AddonItem(iIndex)
                    Me.AddonItem(iIndex) = New AddonItem
                    Me.AddonItem(iIndex).AddonName = xAddon.Element("Name").Value
                    Me.AddonItem(iIndex).Author = xAddon.Element("User").Value
                    Me.AddonItem(iIndex).Version = xAddon.Element("AddonVersion").Value
                    Me.AddonItem(iIndex).Summary = xAddon.Element("Description").Value
                    'sHTTP.StartDownloadImage(xAddon.Element("Screenshot").Value)
                    'While sHTTP.IsDownloading
                    '    Application.DoEvents()
                    'End While
                    'Me.AddonItem(iIndex).ScreenShot = sHTTP.Image

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
        Me.pnlList.Controls.Add(DirectCast(e.UserState, AddonItem))
    End Sub

    Private Sub bwDownload_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwDownload.RunWorkerCompleted
        Me.tsCategories.Enabled = True
        Me.pnlStatus.Visible = False
    End Sub

    Private Sub ClearList()
        If Me.pnlList.Controls.Count > 0 Then
            For i As Integer = UBound(Me.AddonItem) To 0 Step -1
                If Not IsNothing(Me.AddonItem(i)) Then Me.pnlList.Controls.Remove(Me.AddonItem(i))
            Next
        End If
    End Sub

End Class
