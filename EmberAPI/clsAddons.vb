﻿Public Class EmberAddons
    Friend WithEvents bwDownload As New System.ComponentModel.BackgroundWorker
    Public Shared NewVersonAddonList As New List(Of Addon)


    Public Shared Function AllowedVersion(ByVal MinVersion As String, ByVal MaxVersion As String, Optional ByVal currentVersion As String = "") As Boolean
        Dim MinAllowed As Boolean = False
        Dim MaxAllowed As Boolean = False
        Dim tMinVersion As Single = 0
        Dim tMinRevision As Integer = 0
        Dim tMaxVersion As Single = 9999
        Dim tMaxRevision As Integer = 99999
        Dim MajorVersion As Single = 0
        If String.IsNullOrEmpty(currentVersion) Then currentVersion = Master.MajorVersion.ToString
        MajorVersion = If(String.IsNullOrEmpty(currentVersion), 0, NumUtils.ConvertToSingle(currentVersion))
        MinVersion = MinVersion.Replace(",", ".")
        MaxVersion = MaxVersion.Replace(",", ".")
        If MinVersion.Split(Convert.ToChar(".")).Count = 3 Then
            tMinVersion = NumUtils.ConvertToSingle(MinVersion.Substring(0, MinVersion.LastIndexOf(Convert.ToChar("."))))
        Else
            tMinVersion = If(String.IsNullOrEmpty(MinVersion), 0, NumUtils.ConvertToSingle(MinVersion))
        End If
        If MaxVersion.Split(Convert.ToChar(".")).Count = 3 Then
            tMinVersion = NumUtils.ConvertToSingle(MaxVersion.Substring(0, MaxVersion.LastIndexOf(Convert.ToChar("."))))
        Else
            tMaxVersion = If(String.IsNullOrEmpty(MaxVersion), 99999, NumUtils.ConvertToSingle(MaxVersion))
        End If

        Dim myRevision As Integer = My.Application.Info.Version.Revision

        If tMinVersion <= MajorVersion AndAlso tMinRevision <= myRevision Then
            MinAllowed = True
        End If

        If tMaxVersion >= MajorVersion AndAlso tMaxRevision >= myRevision Then
            MaxAllowed = True
        End If

        Return MinAllowed AndAlso MaxAllowed
    End Function

    Structure Addon
        Public ID As Integer
        Public Name As String
        Public Version As String
        Public InstalledVersion As String
        Public Category As String
        Public MinEVersion As Single
        Public MaxEVersion As Single
    End Structure

    Public Shared Function CheckUpdates() As Integer
        Dim aCheck As New EmberAddons
        NewVersonAddonList.Clear()
        aCheck.bwDownload.RunWorkerAsync()
        While aCheck.bwDownload.IsBusy
            Application.DoEvents()
        End While
        Return NewVersonAddonList.Count
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
                            If (xAddon.Element("User").Value = Master.eSettings.Username) OrElse AllowedVersion(xAddon.Element("EmberVersion_Min").Value, xAddon.Element("EmberVersion_Max").Value) Then
                                Dim AddonItem As New Addon
                                AddonItem.ID = Convert.ToInt32(xAddon.Element("id").Value)
                                AddonItem.Name = xAddon.Element("Name").Value
                                AddonItem.Version = (xAddon.Element("AddonVersion").Value)
                                AddonItem.MinEVersion = NumUtils.ConvertToSingle(xAddon.Element("EmberVersion_Min").Value)
                                AddonItem.MaxEVersion = NumUtils.ConvertToSingle(xAddon.Element("EmberVersion_Max").Value)
                                AddonItem.Category = sType
                                AddonItem.InstalledVersion = Master.DB.IsAddonInstalled(AddonItem.ID)
                                If Not AddonItem.InstalledVersion = "0" AndAlso AddonItem.Version > AddonItem.InstalledVersion Then
                                    NewVersonAddonList.Add(AddonItem)
                                End If
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
