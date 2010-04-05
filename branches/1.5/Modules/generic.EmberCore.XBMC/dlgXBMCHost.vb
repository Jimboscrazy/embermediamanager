Imports System.Windows.Forms
Imports EmberAPI

Public Class dlgXBMCHost
    Public XComs As New List(Of XBMCxCom.XBMCCom)
    Dim xCom As New XBMCxCom.XBMCCom
    Public hostid As String = Nothing
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Not String.IsNullOrEmpty(txtName.Text) Then

            'have to iterate the list instead of using .comtains so we can convert each to lower case
            If xCom Is Nothing Then
                For i As Integer = 0 To XComs.Count - 1
                    If XComs(i).Name.ToLower = Me.txtName.Text.ToLower Then
                        MsgBox(Master.eLang.GetString(1, "The name you are attempting to use for this XBMC installation is already in use. Please choose another."), MsgBoxStyle.Exclamation, Master.eLang.GetString(2, "Each name must be unique"))
                        txtName.Focus()
                        Exit Sub
                    End If
                Next
            End If

            If Not String.IsNullOrEmpty(txtIP.Text) Then
                If Not String.IsNullOrEmpty(txtPort.Text) Then
                    If Not xCom Is Nothing Then
                        Me.xCom.Name = Me.txtName.Text
                        Me.xCom.IP = Me.txtIP.Text
                        Me.xCom.Port = Me.txtPort.Text
                        Me.xCom.Username = Me.txtUsername.Text
                        Me.xCom.Password = Me.txtPassword.Text
                    Else
                        XComs.Add(New XBMCxCom.XBMCCom With {.Name = txtName.Text, .IP = txtIP.Text, .Port = txtPort.Text, .Username = txtUsername.Text, .Password = txtPassword.Text})
                    End If

                Else
                    MsgBox(Master.eLang.GetString(3, "You must enter a port for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(4, "Please Enter a Port"))
                    txtPort.Focus()
                End If
            Else
                MsgBox(Master.eLang.GetString(5, "You must enter an IP for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(6, "Please Enter an IP"))
                txtIP.Focus()
            End If
        Else
            MsgBox(Master.eLang.GetString(7, "You must enter a name for this XBMC installation."), MsgBoxStyle.Exclamation, Master.eLang.GetString(8, "Please Enter a Unique Name"))
            txtName.Focus()
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Sub Setup()
        'Me.GroupBox11.Text = Master.eLang.GetString(9, "XBMC Communication")
        'Me.btnEditCom.Text = Master.eLang.GetString(10, "Commit Edit")
        Me.Label16.Text = Master.eLang.GetString(11, "Name:")
        'Me.btnAddCom.Text = Master.eLang.GetString(12, "Add New")
        Me.Label13.Text = Master.eLang.GetString(425, "Username:", True)
        Me.Label14.Text = Master.eLang.GetString(426, "Password:", True)
        Me.Label7.Text = Master.eLang.GetString(13, "XBMC IP:")
        Me.Label6.Text = Master.eLang.GetString(14, "XBMC Port:")
        'Me.btnRemoveCom.Text = Master.eLang.GetString(15, "Remove Selected")
        'Me.cbEnabled.Text = Master.eLang.GetString(774, "Enabled", True)
    End Sub

    Private Sub dlgXBMCHost_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Setup()
        xCom = XComs.FirstOrDefault(Function(y) y.Name = hostid)
        If Not xCom Is Nothing Then
            Me.txtName.Text = xCom.Name
            Me.txtIP.Text = xCom.IP
            Me.txtPort.Text = xCom.Port
            Me.txtUsername.Text = xCom.Username
            Me.txtPassword.Text = xCom.Password
        End If
    End Sub

    Public Shared Function XBMCGetSources(ByVal xc As XBMCxCom.XBMCCom) As List(Of String)
        Dim cmd As String = "command=fileDownload(special://profile/sources.xml)"
        Dim updateXML As String = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(XBMCxCom.SendCmd(xc, cmd)))
        Dim listSources As New List(Of String)

        If updateXML.Length > 0 Then
            Dim n As String = String.Empty
            Dim xmlUpdate As XDocument
            Try
                xmlUpdate = XDocument.Parse(updateXML)
            Catch
                Return listSources
            End Try
            Dim xUdpate = From xUp In xmlUpdate...<video>...<source>...<path> Select xUp.Value
            Try
                For Each x As String In xUdpate
                    listSources.Add(x)
                Next
            Catch ex As Exception
            End Try
        End If
        Return listSources
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPopulate.Click
        Dim xc As New XBMCxCom.XBMCCom With {.Name = txtName.Text, .IP = txtIP.Text, .Port = txtPort.Text, .Username = txtUsername.Text, .Password = txtPassword.Text}
        Dim list As List(Of String) = XBMCGetSources(xc)
    End Sub
End Class
