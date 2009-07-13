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

Option Explicit On

Imports System.IO
Imports System.Text.RegularExpressions

Public Class dlgSortFiles
    Private _hitgo As Boolean = False

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = If(Me._hitgo, System.Windows.Forms.DialogResult.OK, System.Windows.Forms.DialogResult.Cancel)
        Me.Close()
    End Sub

    Private Sub btnGo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGo.Click
        '//
        ' Convert a file source into a folder source by separating everything into separate folders
        '\\

        If Not String.IsNullOrEmpty(Me.txtPath.Text) AndAlso Directory.Exists(Me.txtPath.Text) Then
            If MsgBox(String.Concat("WARNING: If you continue, all files will be sorted into separate folders.", vbNewLine, vbNewLine, "Are you sure you want to continue?"), MsgBoxStyle.Critical Or MsgBoxStyle.YesNo, "Are you sure?") = MsgBoxResult.Yes Then
                Me._hitgo = True
                SortFiles(Me.txtPath.Text)
                lblStatus.Text = "Done!"
                pbStatus.Value = 0

                Master.eSettings.SortPath = Me.txtPath.Text
            End If
        Else
            MsgBox("The folder you entered does not exist. Please enter a valid path.", MsgBoxStyle.Exclamation, "Directory Not Found")
            Me.txtPath.Focus()
        End If
    End Sub

    Private Sub SortFiles(ByVal sPath As String)
        Dim tmpAL As New ArrayList
        Dim tmpPath As String = String.Empty
        Dim tmpName As String = String.Empty
        Dim iCount As Integer = 0
        Try
            If Directory.Exists(sPath) Then
                Dim di As New DirectoryInfo(sPath)
                Dim lFi As New List(Of FileInfo)

                Try
                    lFi.AddRange(di.GetFiles())
                Catch
                End Try

                pbStatus.Maximum = lFi.Count

                For Each sFile As FileInfo In lFi
                    lblStatus.Text = String.Concat("Moving ", sFile.Name)
                    tmpName = StringManip.CleanStackingMarkers(Path.GetFileNameWithoutExtension(sFile.Name))
                    tmpName = tmpName.Replace(".fanart", String.Empty)
                    tmpName = tmpName.Replace("-fanart", String.Empty)
                    tmpName = tmpName.Replace("-trailer", String.Empty)
                    tmpName = Regex.Replace(tmpName, "\[trailer(\d+)\]", String.Empty)
                    tmpPath = Path.Combine(sPath, tmpName)
                    If Not Directory.Exists(tmpPath) Then
                        Directory.CreateDirectory(tmpPath)
                    End If

                    File.Move(sFile.FullName, Path.Combine(tmpPath, sFile.Name))
                    pbStatus.Value = iCount
                    iCount += 1
                Next

                lFi = Nothing
                di = Nothing
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With Me.fbdBrowse
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not String.IsNullOrEmpty(.SelectedPath) Then
                    Me.txtPath.Text = .SelectedPath
                End If
            End If
        End With
    End Sub

    Private Sub dlgSortFiles_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.txtPath.Text = Master.eSettings.SortPath
    End Sub

    Private Sub dlgSortFiles_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub
End Class
