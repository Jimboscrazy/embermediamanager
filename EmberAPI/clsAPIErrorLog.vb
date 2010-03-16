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

    #Region "Events"

    Public Event ErrorOccurred()

    #End Region 'Events

    #Region "Methods"

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

    #End Region 'Methods

End Class

'#################################################################################################################################
Public Class JobLogger

    #Region "Fields"

    Public Shared Enabled As Boolean = True
    Public Shared JobsList As New List(Of Job)

    Private Shared InternalCounter As Integer = 0

    Private lastId As Double

    #End Region 'Fields

    #Region "Enumerations"

    Enum ItemStatus
        OK = 0
        Fail = 1
        Abort = 2
        Cancel = 3
        Pending = 3
        Skiped = 5
    End Enum

    'Example of type ,, can be enum or string (pseudo code)
    Enum ItemTypes
        Generic = 0
        ScrapeMovie = 1
        ScrapeTVEpisode = 2
        ScanMovie = 3
    End Enum

    Enum JobStatus
        Open = 0
        Closed = 1
    End Enum

    'Example of type ,, can be enum or string (pseudo code)
    Enum JobTypes
        GenericLog = 0
        Movies = 1
        TVShows = 2
        DoScan = 3
        Rename = 4
        ' Etc
    End Enum

    #End Region 'Enumerations

    #Region "Methods"

    Public Overloads Function AddJob(ByVal id As Long, ByVal JobType As JobTypes, ByVal message As String, Optional ByVal Status As JobStatus = JobStatus.Open, Optional ByVal name As String = "") As Double
        If Not Enabled Then Return 0
        'Dim localId As Double = 0
        If id = 0 Then
            id = Convert.ToInt64(Functions.ConvertToUnixTimestamp(Now) * 10)
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

    Public Overloads Function AddJob(ByVal name As String, ByVal JobType As JobTypes, ByVal message As String, Optional ByVal Status As JobStatus = JobStatus.Open) As Double
        If Not Enabled Then Return 0
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobName = name)
        Dim id As Long = 0
        If Not currJob Is Nothing Then id = currJob.JobID
        Return AddJob(id, JobType, message, Status, name)
    End Function

    Public Function AddJobItem(ByVal JobId As Double, ByVal ItemType As ItemTypes, ByVal message As String, ByVal Status As ItemStatus, Optional ByVal detail As String = "") As Double
        If Not Enabled Then Return 0
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobID = JobId)
        If Not currJob Is Nothing Then
            currJob.Items.Add(New JobItem With {.JobID = currJob.JobID, .ItemID = currJob.Items.Count, .ItemType = ItemType, .Message = message, .Detail = detail, .ItemStatus = Status})
            'Call module JobLogger now
            Return currJob.Items.Count - 1
        End If
    End Function

    Public Function AddJobItem(ByVal name As String, ByVal ItemType As ItemTypes, ByVal message As String, ByVal Status As ItemStatus, Optional ByVal detail As String = "") As Double
        If Not Enabled Then Return -1
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobName = name And y.JobStatus <> JobStatus.Closed)
        If Not currJob Is Nothing Then
            Return AddJobItem(currJob.JobID, ItemType, message, Status, detail)
        Else
            Return -1
        End If
    End Function

    Public Overloads Sub CloseJob(ByVal JobId As Double)
        If Not Enabled Then Return
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobID = JobId And y.JobStatus <> JobStatus.Closed)
        If Not currJob Is Nothing Then currJob.JobStatus = JobStatus.Closed
    End Sub

    Public Overloads Sub CloseJob(ByVal name As String)
        If Not Enabled Then Return
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobName = name And y.JobStatus <> JobStatus.Closed)
        If Not currJob Is Nothing Then currJob.JobStatus = JobStatus.Closed
    End Sub

    Public Sub RemoveJob(ByVal name As String)
        If Not Enabled Then Return
        Dim currJob As Job = JobsList.FirstOrDefault(Function(y) y.JobName = name)
        If Not currJob Is Nothing Then JobsList.Remove(currJob)
    End Sub

    #End Region 'Methods

    #Region "Nested Types"

    ' Job is a Group of items.. example: scrape all will be a job .. scrape each movie will be a item
    Public Class Job

        #Region "Fields"

        Public Items As New List(Of JobItem)
        Public JobID As Long
        Public JobName As String
        Public JobStatus As JobStatus 'Some Enus Here
        Public JobType As JobTypes 'Some Enus Here
        Public Message As String

        #End Region 'Fields

    End Class

    Public Class JobItem

        #Region "Fields"

        Public Detail As String
        Public ItemID As Integer
        Public ItemStatus As ItemStatus 'Some Enus Here
        Public ItemType As ItemTypes 'Some Enus Here
        Public JobID As Double
        Public Message As String

        #End Region 'Fields

    End Class

    #End Region 'Nested Types

End Class