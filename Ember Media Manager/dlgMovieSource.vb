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

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Using SQLtransaction As SQLite.SQLiteTransaction = Master.SQLcn.BeginTransaction
            Using SQLcommand As SQLite.SQLiteCommand = Master.SQLcn.CreateCommand
                SQLcommand.CommandText = String.Concat("INSERT OR REPLACE INTO sources (name, path, recursive, foldername, single) VALUES (?,?,?,?,?);")
                Dim parName As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parName", DbType.String, 0, "name")
                Dim parPath As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parPath", DbType.String, 0, "path")
                Dim parRecur As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parRecur", DbType.Boolean, 0, "recursive")
                Dim parFolder As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parFolder", DbType.Boolean, 0, "foldername")
                Dim parSingle As SQLite.SQLiteParameter = SQLcommand.Parameters.Add("parSingle", DbType.Boolean, 0, "single")

                parName.Value = txtSourceName.Text
                parPath.Value = txtSourcePath.Text
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
        Me.OK_Button.Enabled = True
    End Sub
End Class
