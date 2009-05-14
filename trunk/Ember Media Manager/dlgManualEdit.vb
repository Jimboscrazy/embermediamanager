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

Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Xml.Schema

Public Class dlgManualEdit

#Region "Declarations"


    Private currFile As String
    Private Changed As Boolean = False
    Private ReturnOK As Boolean = False
    Private TagStack As New Stack()
    Private DtdDt As DataTable

    Private IsValid As Boolean
    Private ErrStr As String
    Private lineInf As IXmlLineInfo


#End Region


#Region "Functions/Routines"


    Private Function ConstructTag(ByVal ElementNameParam As String) As String


        Dim ElementName As String
        ElementName = ElementNameParam
        Dim myRow As DataRow

        Try


            Dim currRows() As DataRow = DtdDt.Select(Nothing, Nothing, DataViewRowState.CurrentRows)

            If (currRows.Length < 1) Then
                RichTextBox1.Text += "No Current Rows Found"
            Else

                For Each myRow In currRows
                    If myRow(2).ToString = "Att" Then

                        If myRow(0).ToString = ElementNameParam.Trim Then
                            ElementName = ElementName + " " + myRow(1).ToString + "="" """
                        End If
                    End If

                Next
            End If

            ElementName += ">"


        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try



        Return ElementName
    End Function

    Private Sub IndentFormat()

        Try
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            Dim tempfile As String = Path.GetTempPath + "nfo-uf.tmp"
            RichTextBox1.SaveFile(tempfile, RichTextBoxStreamType.PlainText)

            Dim IfErr As Boolean = False
            Dim StrR As New StreamReader(tempfile)
            Dim StrW As New StreamWriter(Path.GetTempPath + "nfo.tmp", False)
            Dim AllData As String = StrR.ReadToEnd
            Dim m As Match

            Dim TagS As String, i As Integer

            'Converting entire file to a single line

            AllData = AllData.Replace(vbNewLine, String.Empty)
            AllData = AllData.Replace(vbCrLf, String.Empty)
            AllData = AllData.Replace(vbLf, String.Empty)
            AllData = AllData.Replace(vbCr, String.Empty)
            AllData = AllData.Replace(vbTab, String.Empty).Trim



            'Looking for Processing Instruction and DTD declaration

            For i = 0 To 3 'We assume only first 4 lines have Processing Instruction and DTD declaration
                m = Regex.Match(AllData, "^\<\?([^>]+)\>", RegexOptions.IgnoreCase) 'go to MSDN for RegularExpression Help
                TagS = String.Empty
                If m.Success Then
                    TagS = Regex.Replace(AllData, "^\<\?([^>]+)\>(.*)", "<?$1>", RegexOptions.IgnoreCase)
                    AllData = Regex.Replace(AllData, "^\<\?([^>]+)\>(.*)", "$2", RegexOptions.IgnoreCase)
                    StrW.WriteLine(TagS)
                Else
                    m = Regex.Match(AllData, "^\<\!DOCTYPE([^>]+)\>", RegexOptions.IgnoreCase)
                    If m.Success Then
                        TagS = Regex.Replace(AllData, "^\<\!DOCTYPE([^>]+)\>(.*)", "<!DOCTYPE$1>", RegexOptions.IgnoreCase)
                        AllData = Regex.Replace(AllData, "^\<\!DOCTYPE([^>]+)\>(.*)", "$2", RegexOptions.IgnoreCase)
                        StrW.WriteLine(TagS)
                    End If

                End If



            Next

            Dim LevelX, j As Integer, TabC As String




            Do
                TagS = String.Empty
                TabC = String.Empty

                m = Regex.Match(AllData, "^\<([^>]+) \/\>") 'Single Tag
                If m.Success Then

                    TagS = Regex.Replace(AllData, "^\<([^>]+) \/\>(.*)", "<$1 />").Trim
                    AllData = Regex.Replace(AllData, "^\<([^>]+) \/\>(.*)", "$2").Trim

                    For j = 1 To LevelX
                        TabC += vbTab
                    Next
                    StrW.WriteLine(TabC & TagS)
                Else
                    m = Regex.Match(AllData, "^\<([^>/]+)\>([^<]+)\<\/([^>/]+)\>") 'Opening Tag
                    If m.Success Then
                        TagS = Regex.Replace(AllData, "^\<([^>/]+)\>([^<]+)\<\/([^>/]+)\>(.*)", "<$1>$2</$3>").Trim

                        AllData = Regex.Replace(AllData, "^\<([^>/]+)\>([^<]+)\<\/([^>/]+)\>(.*)", "$4").Trim

                        For j = 1 To LevelX 'Calculating depth of tag
                            TabC += vbTab
                        Next
                        StrW.WriteLine(TabC & TagS)
                    Else
                        m = Regex.Match(AllData, "^\<\/([^>]+)\>(.*)") 'Closing Tag
                        If m.Success Then

                            TagS = Regex.Replace(AllData, "^\<\/([^>]+)\>(.*)", "</$1>").Trim
                            AllData = Regex.Replace(AllData, "^\<\/([^>]+)\>(.*)", "$2").Trim
                            LevelX -= 1

                            For j = 1 To LevelX
                                TabC += vbTab
                            Next

                            StrW.WriteLine(TabC & TagS)

                        Else
                            m = Regex.Match(AllData, "^\<([^>]+)\>(.*)")
                            If m.Success Then
                                TagS = Regex.Replace(AllData, "^\<([^>]+)\>(.*)", "<$1>").Trim
                                AllData = Regex.Replace(AllData, "^\<([^>]+)\>(.*)", "$2").Trim
                                LevelX += 1
                                For j = 1 To LevelX - 1
                                    TabC += vbTab
                                Next

                                StrW.WriteLine(TabC & TagS)

                            Else
                                m = Regex.Match(AllData, "^([^<]+)\<")
                                If m.Success Then
                                    TagS = Regex.Replace(AllData, "^([^<]+)\<(.*)", "$1").Trim

                                    AllData = Regex.Replace(AllData, "^([^<]+)\<(.*)", "<$2").Trim

                                    For j = 0 To LevelX - 1
                                        TabC += vbTab
                                    Next

                                    StrW.WriteLine(TabC & TagS)

                                Else
                                    MsgBox("This is not a proper XML document", MsgBoxStyle.Information)
                                    IfErr = True
                                    Exit Do

                                End If

                            End If

                        End If

                    End If

                End If


                If AllData.Length < 2 Then
                    Exit Do
                End If

            Loop While True






            StrR.Close()
            StrW.Close()

            If IfErr = False Then
                RichTextBox1.LoadFile(Path.GetTempPath + "nfo.tmp", RichTextBoxStreamType.PlainText)
            End If

        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub



    Private Sub ParseFile()


        If currFile Is Nothing Then
            MsgBox("Please open an XML file for parsing", MsgBoxStyle.Information, "Error")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim tempFile As String = Path.GetTempPath + "nfo.tmp"
        RichTextBox1.SaveFile(tempFile, RichTextBoxStreamType.PlainText)

        Dim xmlP As New XmlTextReader(tempFile)
        ' Set the validation settings.
        Dim settings As XmlReaderSettings = New XmlReaderSettings()
        settings.ValidationType = ValidationType.Schema
        settings.ValidationFlags = settings.ValidationFlags Or XmlSchemaValidationFlags.ProcessInlineSchema
        settings.ValidationFlags = settings.ValidationFlags Or XmlSchemaValidationFlags.ReportValidationWarnings

        Dim xmlV As XmlReader = XmlReader.Create(xmlP, settings)
        ErrStr = String.Empty
        ListBox1.Items.Clear()

        IsValid = True


        Do
            Try
                If xmlV.Read() Then
                    lineInf = CType(xmlV, IXmlLineInfo)
                End If


            Catch exx As Exception

                Try

                    IsValid = False

                    If lineInf.HasLineInfo Then
                        ErrStr = lineInf.LineNumber.ToString + ": " + lineInf.LinePosition.ToString + " " + exx.Message
                    End If

                    If exx.Message.IndexOf("EndElement") > 1 Then
                        Exit Do
                    End If

                    ListBox1.Items.Add(ErrStr)

                Catch eeex As Exception
                    MsgBox("Some unexpected error occurred " + vbNewLine + eeex.Message, MsgBoxStyle.Information, "Error")
                    Exit Do
                End Try

            End Try


        Loop While Not xmlP.EOF

        xmlV.Close()
        xmlP.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default


        If IsValid = False Then
            MsgBox("File is not valid", MsgBoxStyle.Exclamation, "Error")
        Else
            MsgBox("File is valid", MsgBoxStyle.Information, "OK")
        End If



    End Sub

    Private Sub WriteErrorLog(ByVal sender As Object, ByVal args As ValidationEventArgs)

        IsValid = False
        ErrStr = lineInf.LineNumber.ToString + ": " + lineInf.LinePosition.ToString + " " + args.Message
        ListBox1.Items.Add(ErrStr)

    End Sub

#End Region

#Region " Event handlers..."

    Private Sub MenuItem10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Me.Close()

    End Sub


    Private Sub Editor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        currFile = Master.currNFO
        RichTextBox1.LoadFile(Master.currNFO, RichTextBoxStreamType.PlainText)
        Me.Text = "Manual NFO Editor | " + Master.currNFO.Substring(currFile.LastIndexOf("\") + 1)

    End Sub

    Private Sub MenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RichTextBox1.SaveFile(currFile, RichTextBoxStreamType.PlainText)
        ReturnOK = True
        Changed = False
    End Sub

    Private Sub RichTextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Changed = True
    End Sub

    Private Sub Editor_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Changed = True Then
            Dim DResult As DialogResult
            DResult = MsgBox("Do you want to save changes?", MsgBoxStyle.YesNoCancel, "Save")
            If DResult = MsgBoxResult.Yes Then

                RichTextBox1.SaveFile(currFile, RichTextBoxStreamType.PlainText)
                Me.DialogResult = Windows.Forms.DialogResult.OK

            Else

                e.Cancel = True

            End If

        Else

            If ReturnOK Then Me.DialogResult = Windows.Forms.DialogResult.OK


        End If


    End Sub

    Private Sub MenuItem21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        IndentFormat()
    End Sub

    Private Sub MenuItem20_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ParseFile()
    End Sub


    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim SelItem As String
        Dim linN, colN As Integer

        SelItem = ListBox1.SelectedItem.ToString

        If Not String.IsNullOrEmpty(SelItem) Then

            linN = CType(Regex.Replace(SelItem, "([0-9]+): ([0-9]+)(.*)", "$1"), Integer)
            colN = CType(Regex.Replace(SelItem, "([0-9]+): ([0-9]+)(.*)", "$2"), Integer)


            Dim mc As MatchCollection
            Dim i As Integer = 0

            mc = Regex.Matches(RichTextBox1.Text, "\n", RegexOptions.Singleline)

            Try
                RichTextBox1.Select(mc(linN - 2).Index + colN, 2)
                RichTextBox1.SelectionColor = Color.Blue
                RichTextBox1.Focus()

            Catch ex As Exception
                RichTextBox1.Focus()
            End Try
        End If


    End Sub

    Private Sub Editor_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        RichTextBox1.Focus()
    End Sub

#End Region

End Class
