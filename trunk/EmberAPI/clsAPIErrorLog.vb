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
Imports System.Text

Public Class ErrorLogger

    Public Event ErrorOccurred()

    ''' <summary>
    ''' Write the error to our log file, if enabled in settings.
    ''' </summary>
    ''' <param name="msg">Error summary</param>
    ''' <param name="stkTrace">Full stack trace</param>
    ''' <param name="title">Error title</param>
    Public Sub WriteToErrorLog(ByVal msg As String, ByVal stkTrace As String, ByVal title As String, Optional ByVal Notify As Boolean = True)

        Try
            If Master.eSettings.LogErrors Then
                Dim sPath As String = Path.Combine(Functions.AppPath, "Log")

                If Not System.IO.Directory.Exists(sPath) Then
                    System.IO.Directory.CreateDirectory(sPath)
                End If

                'check the file
                Using fs As FileStream = New FileStream(Path.Combine(sPath, "errlog.txt"), FileMode.OpenOrCreate, FileAccess.ReadWrite)
                    Using s As StreamWriter = New StreamWriter(fs)
                    End Using
                End Using

                'log it
                Using fs1 As FileStream = New FileStream(Path.Combine(sPath, "errlog.txt"), FileMode.Append, FileAccess.Write)
                    Using s1 As StreamWriter = New StreamWriter(fs1)
                        s1.Write(String.Concat("Title: ", title, vbNewLine))
                        s1.Write(String.Concat("Message: ", msg, vbNewLine))
                        s1.Write(String.Concat("StackTrace: ", stkTrace, vbNewLine))
                        s1.Write(String.Concat("Date/Time: ", DateTime.Now.ToString(), vbNewLine))
                        s1.Write(String.Concat("===========================================================================================", vbNewLine, vbNewLine))
                    End Using
                End Using

                If Notify Then ModulesManager.Instance.RunGeneric(Enums.ModuleEventType.Notification, New List(Of Object)(New Object() {"error", 1, Master.eLang.GetString(816, "An Error Has Occurred"), msg, Nothing}))

                RaiseEvent ErrorOccurred()
            End If
        Catch
        End Try
    End Sub

End Class

'#################################################################################################################################
Public Class JobLogger
    Public Shared Enabled As Boolean = True
    Private Shared InternalCounter As Integer = 0
    Public Shared JobsList As New List(Of Job)
    Private lastId As Double
    ' Job is a Group of items.. example: scrape all will be a job .. scrape each movie will be a item
    Public Class Job
        Public JobID As Double
        Public JobType As Integer 'Some Enus Here
        Public JobStatus As Integer 'Some Enus Here
        Public JobName As String
        Public Message As String
        Public Items As New List(Of JobItem)
    End Class
    Public Class JobItem
        Public ItemID As Double
        Public ItemType As Integer 'Some Enus Here
        Public JobID As Double
        Public ItemStatus As Integer 'Some Enus Here
        Public Message As String
    End Class
    'Example of type ,, can be enum or string (pseudo code)
    Enum JobTypes
        GenericLog = 0
        ScrapeMovie = 1
        ScrapeTV = 2
        DoMovieScan = 3
        ' Etc
    End Enum
    'Example of type ,, can be enum or string (pseudo code)
    Enum ItemTypes
        Generic = 0
        ScrapeMovie = 1
        ScrapeTVEpisode = 2
        ScanMovie = 3
    End Enum
    Enum ItemStatus
        OK = 0
        Fail = 1
        Abort = 2
        Cancel = 3
        Pending = 3
        Skiped = 5
    End Enum
    Public Function AddJobItem(ByVal JobId As Double, ByVal ItemType As Integer, ByVal message As String, Optional ByVal Status As Integer = 0) As Double
        If Not Enabled Then Return 0
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobID = JobId)
        If Not currJob Is Nothing Then
            currJob.Items.Add(New JobItem With {.JobID = currJob.JobID, .ItemID = currJob.Items.Count, .ItemType = ItemType, .Message = message, .ItemStatus = Status})
            'Call module JobLogger now
            Return currJob.Items.Count - 1
        End If
    End Function
    Public Overloads Function AddJob(ByVal id As Double, ByVal JobType As Integer, ByVal message As String, Optional ByVal Status As Integer = 0, Optional ByVal name As String = "") As Double
        If Not Enabled Then Return 0
        'Dim localId As Double = 0
        If id = 0 Then
            id = Functions.ConvertToUnixTimestamp(Now) * 10
            If id = lastId Then
                lastId = id
                InternalCounter += 1
                id += InternalCounter
            Else
                lastId = id
                InternalCounter = 0
            End If
            If name = "" Then name = id.ToString
            JobsList.Add(New Job With {.JobID = id, .JobType = JobType, .JobName = name, .Message = message})
        End If
        'Call module JobLogger now
        Return id
    End Function
    Public Overloads Function AddJob(ByVal name As String, ByVal JobType As Integer, ByVal message As String, Optional ByVal Status As Integer = 0) As Double
        If Not Enabled Then Return 0
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobName = name)
        Dim id As Double = 0
        If Not currJob Is Nothing Then id = currJob.JobID
        Return AddJob(id, JobType, message, Status, name)
    End Function
    Public Sub CloseJob(ByVal name As String)
        If Not Enabled Then Return
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobName = name)
        If Not currJob Is Nothing Then JobsList.Remove(currJob)
    End Sub
End Class