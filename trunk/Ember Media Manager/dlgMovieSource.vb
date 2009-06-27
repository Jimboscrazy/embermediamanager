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
Imports System
Imports System.IO

Public Class dlgMovieSource

    Private currNameText As String = String.Empty
    Private prevNameText As String = String.Empty
    Private currPathText As String = String.Empty
    Private prevPathText As String = String.Empty

    Private _id As Integer = -1

    Public Overloads Function ShowDialog(ByVal id As Integer) As Windows.Forms.DialogResult

        '//
        ' Overload to pass data
        '\\

        Me._id = id

        Return MyBase.ShowDialog()
    End Function

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                If Me._id >= 0 Then
                    SQLcommand.CommandText = String.Concat("UPDATE sources SET name = (?), path = (?), recursive = (?), foldername = (?), single = (?) WHERE ID =", Me._id, ";")
                Else
                    SQLcommand.CommandText = "INSERT OR REPLACE INTO sources (name, path, recursive, foldername, single) VALUES (?,?,?,?,?);"
                End If
                Dim parName As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parName", DbType.String, 0, "name")
                Dim parPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPath", DbType.String, 0, "path")
                Dim parRecur As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parRecur", DbType.Boolean, 0, "recursive")
                Dim parFolder As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFolder", DbType.Boolean, 0, "foldername")
                Dim parSingle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSingle", DbType.Boolean, 0, "single")

                parName.Value = txtSourceName.Text.Trim
                parPath.Value = txtSourcePath.Text.Trim
                parRecur.Value = chkScanRecursive.Checked
                parFolder.Value = chkUseFolderName.Checked
                parSingle.Value = chkSingle.Checked

                SQLcommand.ExecuteNonQuery()
            End Using
            SQLtransaction.Commit()
        End Using



        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click

        Try
            With Me.fbdBrowse
                If .ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not String.IsNullOrEmpty(.SelectedPath) Then
                        Me.txtSourcePath.Text = .SelectedPath
                    End If
                End If
            End With
        Catch ex As Exception
            Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
        End Try
    End Sub

    Private Sub txtSourceName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSourceName.TextChanged
        Me.currNameText = Me.txtSourceName.Text

        Me.tmrWait.Enabled = False
        Me.tmrWait.Enabled = True
    End Sub

    Private Sub tmrWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrWait.Tick
        If Me.prevNameText = Me.currNameText Then
            Me.tmrName.Enabled = True
        Else
            Me.prevNameText = Me.currNameText
        End If
    End Sub

    Private Sub tmrName_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrName.Tick
        Me.tmrWait.Enabled = False
        Me.CheckConditions()
        Me.tmrName.Enabled = False
    End Sub

    Private Sub CheckConditions()
        Dim isValid As Boolean = False

        If String.IsNullOrEmpty(Me.txtSourceName.Text) Then
            pbValid.Image = My.Resources.invalid
        Else
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT ID FROM Sources WHERE Name LIKE """, Me.txtSourceName.Text.Trim, """ AND ID != ", Me._id, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    If Not String.IsNullOrEmpty(SQLreader("ID").ToString) Then
                        pbValid.Image = My.Resources.invalid
                    Else
                        pbValid.Image = My.Resources.valid
                        isValid = True
                    End If
                End Using
            End Using
        End If

        If Not String.IsNullOrEmpty(Me.txtSourcePath.Text) AndAlso Directory.Exists(Me.txtSourcePath.Text.Trim) AndAlso isValid Then
            Me.OK_Button.Enabled = True
        End If
    End Sub

    Private Sub tmrPathWait_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrPathWait.Tick
        If Me.prevPathText = Me.currPathText Then
            Me.tmrPath.Enabled = True
        Else
            Me.prevPathText = Me.currPathText
        End If
    End Sub

    Private Sub tmrPath_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrPath.Tick
        Me.tmrPathWait.Enabled = False
        Me.CheckConditions()
        Me.tmrPath.Enabled = False
    End Sub

    Private Sub txtSourcePath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSourcePath.TextChanged
        Me.currPathText = Me.txtSourcePath.Text

        Me.tmrPathWait.Enabled = False
        Me.tmrPathWait.Enabled = True
    End Sub


    Private Sub dlgMovieSource_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me._id >= 0 Then
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("SELECT * FROM Sources WHERE ID = ", Me._id, ";")
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                    Me.txtSourceName.Text = SQLreader("Name").ToString
                    Me.txtSourcePath.Text = SQLreader("Path").ToString
                    Me.chkScanRecursive.Checked = SQLreader("Recursive")
                    Me.chkSingle.Checked = SQLreader("Single")
                    Me.chkUseFolderName.Checked = SQLreader("Foldername")
                End Using
            End Using
        End If
    End Sub

    Private Sub dlgMovieSource_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Activate()
    End Sub
End Class
