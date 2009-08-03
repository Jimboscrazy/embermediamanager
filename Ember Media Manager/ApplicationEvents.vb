﻿' ################################################################################
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

Namespace My

    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Try

                If Not Master.GetNETVersion Then
                    MsgBox(String.Concat("Ember Media Manager requires .NET Framework version 3.5 or higher.", vbNewLine, vbNewLine, _
                               "Please install .NET Framework version 3.5 or higher before attempting to use Ember."), MsgBoxStyle.Critical, "Unsupported .NET Version")
                    End
                End If

                Master.TestMediaInfoDLL()
                Master.eSettings.Load()
                Master.eLang.LoadLanguage(Master.eSettings.Language)
                Master.CreateDefaultOptions()

            Catch ex As Exception
                Master.eLog.WriteToErrorLog(ex.Message, ex.StackTrace, "Error")
            End Try
        End Sub

        Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Dim Args() As String = Environment.GetCommandLineArgs
            ' Check if is allready running
            If Args.Count = 1 Then
                MsgBox("Ember Media Manager is already running.", MsgBoxStyle.OkOnly, "Ember Media Manager")
                End
            End If
        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MsgBox(e.Exception, MsgBoxStyle.OkOnly, "Ember Media Manager")
            My.Application.Log.WriteException(e.Exception, TraceEventType.Critical, "Unhandled Exception.")
        End Sub
    End Class

End Namespace

