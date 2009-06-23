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

Public Class dlgWizard

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.SaveSettings()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        Select Case True
            Case Me.Panel1.Visible
                Me.btnBack.Enabled = True
                Me.Panel1.Visible = False
                Me.Panel2.Visible = True
            Case Me.Panel2.Visible
                Me.Panel2.Visible = False
                Me.Panel3.Visible = True
            Case Me.Panel3.Visible
                Me.Panel3.Visible = False
                Me.Panel4.Visible = True
                Me.btnNext.Enabled = False
                Me.OK_Button.Enabled = True
        End Select
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Select Case True
            Case Me.Panel2.Visible
                Me.btnBack.Enabled = False
                Me.Panel2.Visible = False
                Me.Panel1.Visible = True
            Case Me.Panel3.Visible
                Me.Panel3.Visible = False
                Me.Panel2.Visible = True
            Case Me.Panel4.Visible
                Me.Panel4.Visible = False
                Me.Panel3.Visible = True
                Me.btnNext.Enabled = True
                Me.OK_Button.Enabled = False
        End Select
    End Sub

    Private Sub btnMovieAddFolders_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieAddFolder.Click
        Me.AddItem(True)
    End Sub

    Private Sub btnMovieAddFiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieAddFiles.Click
        Me.AddItem(False)
    End Sub

    Private Sub btnMovieRem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMovieRem.Click
        Try
            If Me.lvMovies.SelectedItems.Count > 0 Then
                If MsgBox("Are you sure you want to remove the selected sources? This will remove the movies from these sources from the Ember database.", MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Are You Sure?") = MsgBoxResult.Yes Then
                    Me.lvMovies.BeginUpdate()

                    Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
                        Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                            Dim parSource As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSource", DbType.String, 0, "source")
                            SQLcommand.CommandText = String.Concat("DELETE FROM movies WHERE source = (?);")
                            For i As Integer = lvMovies.SelectedItems.Count - 1 To 0 Step -1
                                parSource.Value = lvMovies.Items(i).Text
                                SQLcommand.ExecuteNonQuery()
                                lvMovies.Items.RemoveAt(i)
                            Next
                        End Using
                        SQLtransaction.Commit()
                    End Using

                    Me.lvMovies.Sort()
                    Me.lvMovies.EndUpdate()
                    Me.lvMovies.Refresh()
                End If
            End If
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub AddItem(ByVal blnIsFolder As Boolean)

        '//
        ' Add folder to folder list. Check to make sure it exists before adding it
        '\\


        Try
            Dim iFound As Integer = -1
            Dim lvItem As ListViewItem

            With Me.fbdBrowse
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not String.IsNullOrEmpty(.SelectedPath.ToString) AndAlso Directory.Exists(.SelectedPath) Then

                        If Me.lvMovies.Items.Count > 0 Then
                            For i As Integer = 0 To Me.lvMovies.Items.Count - 1
                                If Me.lvMovies.Items(i).ToString.ToLower = .SelectedPath.ToString.ToLower Then
                                    iFound = i
                                    Exit For
                                End If
                            Next
                        Else
                            iFound = -1
                        End If


                        If iFound >= 0 Then
                            Me.lvMovies.Items(iFound).Selected = True
                            Me.lvMovies.Items(iFound).Focused = True
                            Me.lvMovies.Focus()
                        Else
                            Me.lvMovies.BeginUpdate()
                            lvItem = Me.lvMovies.Items.Add(.SelectedPath)
                            If blnIsFolder = True Then
                                lvItem.SubItems.Add("Folders")
                            Else
                                lvItem.SubItems.Add("Files")
                            End If
                            lvItem = Nothing
                            Me.lvMovies.Sort()
                            Me.lvMovies.EndUpdate()
                            Me.lvMovies.Columns(0).Width = 388
                            Me.lvMovies.Columns(1).Width = 74
                            Me.lvMovies.Refresh()
                        End If

                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub dlgWizard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.FillSettings()
    End Sub

    Private Sub FillSettings()
        Dim dirArray() As String
        Dim lvItem As ListViewItem
        For Each strFolders As String In Master.eSettings.MovieFolders
            dirArray = Split(strFolders, "|")
            lvItem = Me.lvMovies.Items.Add(dirArray(0).ToString)
            lvItem.SubItems.Add(dirArray(1).ToString)
        Next

        Me.chkMovieTBN.Checked = Master.eSettings.MovieTBN
        Me.chkMovieNameTBN.Checked = Master.eSettings.MovieNameTBN
        Me.chkMovieJPG.Checked = Master.eSettings.MovieJPG
        Me.chkMovieNameJPG.Checked = Master.eSettings.MovieNameJPG
        Me.chkPosterTBN.Checked = Master.eSettings.PosterTBN
        Me.chkPosterJPG.Checked = Master.eSettings.PosterJPG
        Me.chkFolderJPG.Checked = Master.eSettings.FolderJPG
        Me.chkFanartJPG.Checked = Master.eSettings.FanartJPG
        Me.chkMovieNameFanartJPG.Checked = Master.eSettings.MovieNameFanartJPG
        Me.chkMovieNameDotFanartJPG.Checked = Master.eSettings.MovieNameDotFanartJPG
        Me.chkMovieNFO.Checked = Master.eSettings.MovieNFO
        Me.chkMovieNameNFO.Checked = Master.eSettings.MovieNameNFO
    End Sub

    Private Sub SaveSettings()
        Master.eSettings.MovieFolders.Clear()
        For Each lvItem As ListViewItem In Me.lvMovies.Items
            Master.eSettings.MovieFolders.Add(String.Concat(lvItem.Text, "|", lvItem.SubItems(1).Text))
        Next

        Master.eSettings.MovieTBN = Me.chkMovieTBN.Checked
        Master.eSettings.MovieNameTBN = Me.chkMovieNameTBN.Checked
        Master.eSettings.MovieJPG = Me.chkMovieJPG.Checked
        Master.eSettings.MovieNameJPG = Me.chkMovieNameJPG.Checked
        Master.eSettings.PosterTBN = Me.chkPosterTBN.Checked
        Master.eSettings.PosterJPG = Me.chkPosterJPG.Checked
        Master.eSettings.FolderJPG = Me.chkFolderJPG.Checked
        Master.eSettings.FanartJPG = Me.chkFanartJPG.Checked
        Master.eSettings.MovieNameFanartJPG = Me.chkMovieNameFanartJPG.Checked
        Master.eSettings.MovieNameDotFanartJPG = Me.chkMovieNameDotFanartJPG.Checked
        Master.eSettings.MovieNFO = Me.chkMovieNFO.Checked
        Master.eSettings.MovieNameNFO = Me.chkMovieNameNFO.Checked
    End Sub

    Private Sub dlgWizard_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub
End Class
