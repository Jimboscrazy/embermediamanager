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
Imports System.Text

Public Class ErrorLogger

    Public Sub WriteToErrorLog(ByVal msg As String, ByVal stkTrace As String, ByVal title As String)

        '//
        ' Write the error to our log file, if the option is set
        '\\

        Try
            If Master.eSettings.LogErrors Then
                Dim sPath As String = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Log")

                If Not System.IO.Directory.Exists(sPath) Then
                    System.IO.Directory.CreateDirectory(sPath)
                End If

                'check the file
                Dim fs As FileStream = New FileStream(Path.Combine(sPath, "errlog.txt"), FileMode.OpenOrCreate, FileAccess.ReadWrite)
                Dim s As StreamWriter = New StreamWriter(fs)
                s.Close()
                fs.Close()

                'log it
                Dim fs1 As FileStream = New FileStream(Path.Combine(sPath, "errlog.txt"), FileMode.Append, FileAccess.Write)
                Dim s1 As StreamWriter = New StreamWriter(fs1)
                s1.Write(String.Concat("Title: ", title, vbNewLine))
                s1.Write(String.Concat("Message: ", msg, vbNewLine))
                s1.Write(String.Concat("StackTrace: ", stkTrace, vbNewLine))
                s1.Write(String.Concat("Date/Time: ", DateTime.Now.ToString(), vbNewLine))
                s1.Write(String.Concat("===========================================================================================", vbNewLine, vbNewLine))
                s1.Close()
                fs1.Close()
            End If
        Catch
        End Try
    End Sub

End Class