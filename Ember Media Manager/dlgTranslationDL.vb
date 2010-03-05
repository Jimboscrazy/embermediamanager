' ################################################################################
' #                             EMBER MEDIA MANAGER                              #
' ################################################################################
' ################################################################################
' # This file is part of Ember Media Manager.                                    #
' #                                                                              #
' # Ember Media Manager is free software: you can redistribute it and/or modify  #
' # it under the terms of the GNU General Public License as published by         #
' # the Free Software Foundation, either version 3 of the License, or            #
' # (at your option) any later version.                                          #
' #                                                                              #
' # Ember Media Manager is distributed in the hope that it will be useful,       #
' # but WITHOUT ANY WARRANTY; without even the implied warranty of               #
' # MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                #
' # GNU General Public License for more details.                                 #
' #                                                                              #
' # You should have received a copy of the GNU General Public License            #
' # along with Ember Media Manager.  If not, see <http://www.gnu.org/licenses/>. #
' ################################################################################

Public Class dlgTranslationDL

    Dim Templates As New List(Of Template)

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DownloadSelected()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub DownloadSelected()
        Me.lblStatus.Text = Master.eLang.GetString(447, "Downloading selected addon files...")
        Me.pnlStatus.Visible = True
        Application.DoEvents()

        Dim sHTTP As New HTTP
        Dim tFind As New TemplateFind
        Dim tFound As New Template
        Try
            For Each lItem As ListViewItem In lvDownload.Items
                If lItem.Checked Then
                    Select Case lItem.Tag.ToString
                        Case "translation"
                            sHTTP.DownloadFile(lItem.SubItems(0).Tag.ToString, String.Empty, False, "translation")
                        Case "template"
                            tFind.SetSearchString(lItem.Group.Header.ToString, lItem.Text.Replace(Master.eLang.GetString(449, "Export Template: "), String.Empty).Trim)
                            tFound = Templates.Find(AddressOf tFind.Find)
                            If Not IsNothing(tFound) Then
                                For Each sFile As String In tFound.Files
                                    sHTTP.DownloadFile(sFile, String.Empty, False, "template")
                                Next
                            End If
                        Case "movietheme"
                            sHTTP.DownloadFile(lItem.SubItems(0).Tag.ToString, String.Empty, False, "movietheme")
                    End Select
                End If
            Next
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        sHTTP = Nothing
    End Sub

    Private Sub dlgTranslationDL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SetUp()
    End Sub

    Private Sub SetUp()
        Me.Text = Master.eLang.GetString(443, "Download Addons")
        Me.OK_Button.Text = Master.eLang.GetString(179, "OK")
        Me.Cancel_Button.Text = Master.eLang.GetString(167, "Cancel")
        Me.lvDownload.Columns(0).Text = Master.eLang.GetString(444, "File")
        Me.lvDownload.Columns(1).Text = Master.eLang.GetString(445, "Last Update")
        Me.lblStatus.Text = Master.eLang.GetString(446, "Downloading available addons list...")
    End Sub

    Private Sub dlgTranslationDL_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
        Me.DownloadList()
        Me.pnlStatus.Visible = False
    End Sub

    Private Sub DownloadList()
        Dim sHTTP As New HTTP
        Try
            Dim transXML As String = sHTTP.DownloadData("http://www.embermm.com/Updates/Download.xml")
            sHTTP = Nothing

            Dim xmlTrans As XDocument = XDocument.Parse(transXML)

            Dim xTheme = From xThemes In xmlTrans...<addons>...<themes>...<movie>...<theme>
            If xTheme.Count > 0 Then
                Dim tGroup As New ListViewGroup
                Dim lItem As New ListViewItem
                tGroup = New ListViewGroup
                tGroup.Header = Master.eLang.GetString(620, "Movie Theme")
                lvDownload.Groups.Add(tGroup)
                For Each Theme In xTheme
                    lItem = lvDownload.Items.Add(Theme.@name)
                    lItem.Tag = "movietheme"
                    lItem.SubItems.Add(Theme.<lastupdate>.Value)
                    lItem.SubItems(0).Tag = Theme.<url>.Value
                    tGroup.Items.Add(lItem)
                Next
            End If

            Dim xTrans = From xTran In xmlTrans...<addons>...<translations>...<language>
            If xTrans.Count > 0 Then
                Dim lGroup As New ListViewGroup
                Dim lItem As New ListViewItem
                Dim lItemTemplate As New ListViewItem
                Dim xTemplate As New Template
                For Each Trans In xTrans
                    lGroup = New ListViewGroup
                    lGroup.Header = Trans.@name
                    lvDownload.Groups.Add(lGroup)
                    lItem = lvDownload.Items.Add(Master.eLang.GetString(448, "Translation File"))
                    lItem.Tag = "translation"
                    lItem.SubItems.Add(Trans.<lastupdate>.Value)
                    lItem.SubItems(0).Tag = Trans.<url>.Value
                    lGroup.Items.Add(lItem)
                    Dim xTemp = From xTemps In Trans...<templates>...<template>
                    If xTemp.Count > 0 Then
                        For Each Temp In xTemp
                            xTemplate = New Template
                            xTemplate.Language = Trans.@name
                            xTemplate.Name = Temp.@name
                            Dim xFile = From xFiles In Temp...<files>...<file>
                            If xFile.Count > 0 Then
                                For Each tFile In xFile
                                    xTemplate.Files.Add(tFile.Value)
                                Next
                                Templates.Add(xTemplate)
                                lItemTemplate = lvDownload.Items.Add(String.Concat(Master.eLang.GetString(449, "Export Template: "), Temp.@name))
                                lItemTemplate.Tag = "template"
                                lItemTemplate.SubItems.Add(Temp.<lastupdate>.Value)
                                lGroup.Items.Add(lItemTemplate)
                            End If
                        Next
                    End If
                    lItem = Nothing
                    lItemTemplate = Nothing
                    lGroup = Nothing
                Next
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
        sHTTP = Nothing
    End Sub

    Friend Class Template
        Private _language As String
        Private _name As String
        Private _files As New List(Of String)

        Public Property Language() As String
            Get
                Return _language
            End Get
            Set(ByVal value As String)
                _language = value
            End Set
        End Property

        Public Property Name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property

        Public Property Files() As List(Of String)
            Get
                Return _files
            End Get
            Set(ByVal value As List(Of String))
                _files = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            _language = String.Empty
            _name = String.Empty
            _files.Clear()
        End Sub
    End Class

    Friend Class TemplateFind

        Private _searchlang As String = String.Empty
        Private _searchname As String = String.Empty

        Public Sub SetSearchString(ByVal sLang As String, ByVal sName As String)
            _searchlang = sLang
            _searchname = sName
        End Sub

        Public Function Find(ByVal xTemp As Template) As Boolean
            If Not IsNothing(xTemp) AndAlso xTemp.Language = _searchlang AndAlso xTemp.Name = _searchname Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class
End Class
