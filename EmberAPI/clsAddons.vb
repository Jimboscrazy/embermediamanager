Public Class EmberAddons
    Friend WithEvents bwDownload As New System.ComponentModel.BackgroundWorker
    Public Shared AddonList As New List(Of Addon)
    Structure Addon
        Public ID As Integer
        Public Name As String
        Public Version As Single
        Public InstalledVersion As Single
        Public Category As String
        Public MinEVersion As Single
        Public MaxEVersion As Single
    End Structure

    Public Shared Function CheckUpdates() As Integer
        Dim aCheck As New EmberAddons
        AddonList.Clear()
        aCheck.bwDownload.RunWorkerAsync()
        While aCheck.bwDownload.IsBusy
            Application.DoEvents()
        End While
        Return AddonList.Count
    End Function


    Private Sub bwDownload_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwDownload.DoWork
        Dim aoXML As String = String.Empty
        Dim sHTTP As New HTTP
        Dim postData As New List(Of String())

        Try
            For Each sType As String In New String() {"Modules"}
                postData.Add((New String() {"username", Master.eSettings.Username}))
                postData.Add((New String() {"password", Master.eSettings.Password}))
                postData.Add((New String() {"type", sType}))
                postData.Add((New String() {"func", "fetch"}))
                aoXML = sHTTP.PostDownloadData("http://www.embermm.com/addons/addons.php", postData)

                If Not String.IsNullOrEmpty(aoXML) Then
                    Dim xdAddons As XDocument = XDocument.Parse(aoXML)

                    For Each xAddon In xdAddons.Descendants("entry")
                        Try
                            Dim AddonItem As New Addon
                            AddonItem.ID = Convert.ToInt32(xAddon.Element("id").Value)
                            AddonItem.Name = xAddon.Element("Name").Value
                            'AddonItem.Author = xAddon.Element("User").Value
                            AddonItem.Version = NumUtils.ConvertToSingle(xAddon.Element("AddonVersion").Value)
                            AddonItem.MinEVersion = NumUtils.ConvertToSingle(xAddon.Element("EmberVersion_Min").Value)
                            AddonItem.MaxEVersion = NumUtils.ConvertToSingle(xAddon.Element("EmberVersion_Max").Value)
                            'AddonItem.Summary = xAddon.Element("Description").Value
                            AddonItem.Category = sType
                            'sHTTP.StartDownloadImage(String.Format("http://www.embermm.com/addons/addons.php?screenshot={0}", xAddon.Element("id").Value))
                            'While sHTTP.IsDownloading
                            'Application.DoEvents()
                            'End While
                            'Me.bwDownload.ReportProgress(0, AddonItem)
                            AddonItem.InstalledVersion = Master.DB.IsAddonInstalled(AddonItem.ID)
                            If AddonItem.InstalledVersion > 0 AndAlso AddonItem.Version > AddonItem.InstalledVersion Then
                                AddonList.Add(AddonItem)
                            End If

                        Catch ex As Exception
                            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
                        End Try
                    Next
                End If
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        postData = Nothing
        sHTTP = Nothing

    End Sub
End Class
