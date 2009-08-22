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



Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Reflection
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO.Compression

Public Class frmMainManager
    Public MasterDB As New DB
    Public CurrentEmberVersion As String
    Public CurrentEmberPlatform As String
    Public Platform As String = "x86"

    Public Shared OPaths As New List(Of OrigPaths)
    Public Shared EmberVersions As New UpgradeList

    Dim _cmds As New InstallCommands
    Dim CmdsChanged As Boolean = False
    Dim AddCommand As Boolean = False
    Dim CmdVersion As String
    Dim InCommandTab As Boolean

    Dim excludesFiles As New ArrayList
    Dim excludesDirs As New ArrayList

    Friend WithEvents bwFF As New System.ComponentModel.BackgroundWorker

    Public Shared Function AppPath() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory
    End Function

    Class OrigPaths
        Public origpath As String
        Public emberpath As String
        Public recursive As Boolean
        Public platform As String
    End Class

    Class EmberFiles
        Public FTI As FileToInstall
        Public UseFile As Boolean
    End Class

    Class DB
        Public SQLcn As New SQLite.SQLiteConnection()

        Public Function CreateCommand() As SQLite.SQLiteCommand
            Return SQLcn.CreateCommand
        End Function

        Public Function BeginTransaction() As SQLite.SQLiteTransaction
            Return SQLcn.BeginTransaction
        End Function
        Public Sub Close()
            SQLcn.Close()
        End Sub
        Public Sub Connect()
            Try
                SQLcn.ConnectionString = String.Format("Data Source=""{0}"";Compress=True", Path.Combine(AppPath, "SetupManager.emm"))
                SQLcn.Open()
                Using SQLtransaction As SQLite.SQLiteTransaction = SQLcn.BeginTransaction
                    Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS OrigPaths(" & _
                            "OrigPath TEXT NOT NULL, " & _
                            "EmberPath TEXT NOT NULL, " & _
                            "Platform TEXT NOT NULL, " & _
                            "Recursive BOOL NOT NULL DEFAULT False, " & _
                            "PRIMARY KEY (OrigPath,Platform) " & _
                             ");"
                        SQLcommand.ExecuteNonQuery()
                    End Using
                    Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS EmberFiles(" & _
                            "OrigPath TEXT NOT NULL, " & _
                            "EmberPath TEXT NOT NULL, " & _
                            "Filename TEXT NOT NULL, " & _
                            "Hash TEXT NOT NULL, " & _
                            "Platform TEXT NOT NULL, " & _
                            "UseFile BOOL NOT NULL DEFAULT False, " & _
                            "PRIMARY KEY (OrigPath,Filename ) " & _
                             ");"
                        SQLcommand.ExecuteNonQuery()
                    End Using
                    Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS ExcludeFiles(" & _
                            "Filename TEXT NOT NULL, " & _
                            "PRIMARY KEY (Filename) " & _
                             ");"
                        SQLcommand.ExecuteNonQuery()
                    End Using
                    Using SQLcommand As SQLite.SQLiteCommand = SQLcn.CreateCommand
                        SQLcommand.CommandText = "CREATE TABLE IF NOT EXISTS ExcludeDir(" & _
                            "Dirname TEXT NOT NULL, " & _
                            "PRIMARY KEY (Dirname) " & _
                             ");"
                        SQLcommand.ExecuteNonQuery()
                    End Using
                    SQLtransaction.Commit()
                End Using
            Catch ex As Exception
            End Try

        End Sub
        Public Sub DBAddChangeExcludeFile(ByVal p As String)
            Using SQLcommandFilename As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandFilename.CommandText = String.Concat("INSERT OR REPLACE INTO ExcludeFiles (Filename) VALUES (?);")
                Dim parFilename As SQLite.SQLiteParameter = SQLcommandFilename.Parameters.Add("parFilename", DbType.String, 0, "Filename")
                parFilename.Value = p
                SQLcommandFilename.ExecuteNonQuery()
            End Using
        End Sub
        Public Sub DBDeleteExcludeFile(ByVal s As String)
            Using SQLcommandFilename As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandFilename.CommandText = String.Concat("DELETE FROM ExcludeFiles Where Filename=(?);")
                Dim parFilename As SQLite.SQLiteParameter = SQLcommandFilename.Parameters.Add("parFilename", DbType.String, 0, "Filename")
                parFilename.Value = s
                SQLcommandFilename.ExecuteNonQuery()
            End Using
        End Sub
        Public Sub DBAddChangeExcludeDir(ByVal p As String)
            Using SQLcommandFilename As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandFilename.CommandText = String.Concat("INSERT OR REPLACE INTO ExcludeDir (Dirname) VALUES (?);")
                Dim parFilename As SQLite.SQLiteParameter = SQLcommandFilename.Parameters.Add("parFilename", DbType.String, 0, "Dirname")
                parFilename.Value = p
                SQLcommandFilename.ExecuteNonQuery()
            End Using
        End Sub
        Public Sub DBDeleteExcludeDir(ByVal s As String)
            Using SQLcommandFilename As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandFilename.CommandText = String.Concat("DELETE FROM ExcludeDir Where Dirname=(?);")
                Dim parFilename As SQLite.SQLiteParameter = SQLcommandFilename.Parameters.Add("parFilename", DbType.String, 0, "Dirname")
                parFilename.Value = s
                SQLcommandFilename.ExecuteNonQuery()
            End Using
        End Sub

        Public Sub DBAddChangeOirgPaths(ByVal p As OrigPaths)
            Using SQLcommandOrigPaths As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandOrigPaths.CommandText = String.Concat("INSERT OR REPLACE INTO OrigPaths (OrigPath,EmberPath,Recursive,Platform) VALUES (?,?,?,?);")
                Dim parorigpath As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parorigpath", DbType.String, 0, "OrigPath")
                Dim paremberpath As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("paremberpath", DbType.String, 0, "EmberPath")
                Dim parrecursive As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parrecursive", DbType.Boolean, 0, "Recursive")
                Dim parPlatform As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parPlatform", DbType.String, 0, "Platform")
                parrecursive.Value = p.recursive
                parorigpath.Value = p.origpath
                paremberpath.Value = p.emberpath
                parPlatform.Value = p.platform
                SQLcommandOrigPaths.ExecuteNonQuery()
            End Using
        End Sub
        Public Sub DBDeleteOirgPaths(ByVal s As String, ByVal s1 As String)
            Using SQLcommandOrigPaths As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandOrigPaths.CommandText = String.Concat("DELETE FROM OrigPaths Where OrigPath=(?) AND Platform=(?);")
                Dim parorigpath As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parorigpath", DbType.String, 0, "OrigPath")
                Dim parPlatform As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parPlatform", DbType.String, 0, "Platform")
                parorigpath.Value = s
                parPlatform.Value = s1
                SQLcommandOrigPaths.ExecuteNonQuery()
            End Using
        End Sub
        Public Sub DBAddChangeEmberFile(ByVal p As EmberFiles)
            Using SQLcommandOrigPaths As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandOrigPaths.CommandText = String.Concat("INSERT OR REPLACE INTO EmberFiles (OrigPath,EmberPath,Filename,Hash,UseFile,Platform) VALUES (?,?,?,?,?,?);")
                Dim parorigpath As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parorigpath", DbType.String, 0, "OrigPath")
                Dim paremberpath As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("paremberpath", DbType.String, 0, "EmberPath")
                Dim parfilename As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parfilename", DbType.String, 0, "Filename")
                Dim parhash As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parhash", DbType.String, 0, "Hash")
                Dim parUseFile As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parUseFile", DbType.Boolean, 0, "UseFile")
                Dim parPlatform As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parPlatform", DbType.String, 0, "Platform")
                parfilename.Value = p.FTI.Filename
                parorigpath.Value = p.FTI.OriginalPath
                paremberpath.Value = p.FTI.EmberPath
                parhash.Value = p.FTI.Hash
                parPlatform.Value = p.FTI.Platform
                parUseFile.Value = p.UseFile
                SQLcommandOrigPaths.ExecuteNonQuery()
            End Using
        End Sub
        Public Sub DBDeleteAllEmberFile()
            Using SQLcommandFilename As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandFilename.CommandText = String.Concat("DELETE FROM EmberFiles;")
                SQLcommandFilename.ExecuteNonQuery()
            End Using
        End Sub
        Public Sub DBDeleteEmberFile(ByVal s As String, ByVal s1 As String)
            Using SQLcommandOrigPaths As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandOrigPaths.CommandText = String.Concat("DELETE FROM EmberFiles Where OrigPath=(?) AND Filename=(?);")
                Dim parorigpath As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parorigpath", DbType.String, 0, "OrigPath")
                Dim parfilename As SQLite.SQLiteParameter = SQLcommandOrigPaths.Parameters.Add("parfilename", DbType.String, 0, "Filename")
                parorigpath.Value = s
                parfilename.Value = s1
                SQLcommandOrigPaths.ExecuteNonQuery()
            End Using
        End Sub
        Public Function DBGetEmberFile(ByVal s As String, ByVal s1 As String) As EmberFiles
            Dim o As New EmberFiles
            Using SQLcommandEmberFiles As SQLite.SQLiteCommand = SQLcn.CreateCommand
                SQLcommandEmberFiles.CommandText = String.Concat("Select * FROM EmberFiles Where OrigPath=(?) AND Filename=(?);")
                Dim parorigpath As SQLite.SQLiteParameter = SQLcommandEmberFiles.Parameters.Add("parorigpath", DbType.String, 0, "OrigPath")
                Dim parfilename As SQLite.SQLiteParameter = SQLcommandEmberFiles.Parameters.Add("parfilename", DbType.String, 0, "Filename")
                parorigpath.Value = s
                parfilename.Value = s1
                Using SQLreader As SQLite.SQLiteDataReader = SQLcommandEmberFiles.ExecuteReader()
                    While SQLreader.Read
                        o = New EmberFiles
                        o.FTI = New FileToInstall
                        If Not DBNull.Value.Equals(SQLreader("OrigPath")) Then o.FTI.OriginalPath = SQLreader("OrigPath").ToString
                        If Not DBNull.Value.Equals(SQLreader("EmberPath")) Then o.FTI.EmberPath = SQLreader("EmberPath").ToString
                        If Not DBNull.Value.Equals(SQLreader("Filename")) Then o.FTI.Filename = SQLreader("Filename").ToString
                        If Not DBNull.Value.Equals(SQLreader("Hash")) Then o.FTI.Hash = SQLreader("Hash").ToString
                        If Not DBNull.Value.Equals(SQLreader("Platform")) Then o.FTI.Platform = SQLreader("Platform").ToString
                        If Not DBNull.Value.Equals(SQLreader("UseFile")) Then o.UseFile = Convert.ToBoolean(SQLreader("UseFile"))
                        Exit While
                    End While
                End Using
            End Using
            Return o
        End Function
    End Class

    Public Function GetEmberPlatform(ByVal fpath As String) As String
        Dim _Assembly As Assembly = Assembly.ReflectionOnlyLoadFrom(Path.Combine(fpath, "Ember Media Manager.exe"))
        Dim kinds As PortableExecutableKinds
        Dim imgFileMachine As ImageFileMachine
        _Assembly.ManifestModule.GetPEKind(kinds, imgFileMachine)
        If kinds And PortableExecutableKinds.PE32Plus Then
            Return "64"
        End If
        Return "32"
    End Function

    Public Function GetEmberVersion(ByVal fpath As String) As String
        Dim myBuildInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(fpath, "Ember Media Manager.exe"))
        Return myBuildInfo.ProductPrivatePart
    End Function

    Sub LoadExcludes()
        excludesFiles.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = MasterDB.SQLcn.CreateCommand
            SQLcommand.CommandText = String.Concat("SELECT * FROM ExcludeFiles;")
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    If Not DBNull.Value.Equals(SQLreader("Filename")) Then excludesFiles.Add(SQLreader("Filename").ToString)
                End While
            End Using
        End Using
        excludesDirs.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = MasterDB.SQLcn.CreateCommand
            SQLcommand.CommandText = String.Concat("SELECT * FROM ExcludeDir;")
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                While SQLreader.Read
                    If Not DBNull.Value.Equals(SQLreader("Dirname")) Then excludesDirs.Add(SQLreader("Dirname").ToString)
                End While
            End Using
        End Using
    End Sub

    Public Sub DoPopulateFiles()
        DisableGui()
        lstFiles.Items.Clear()
        LoadExcludes()
        bwFF.WorkerSupportsCancellation = True
        bwFF.WorkerReportsProgress = True
        bwFF.RunWorkerAsync()
    End Sub

    Private Sub bwPopulate_Complete(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bwFF.RunWorkerCompleted
        LoadFiles(CheckBox1.Checked)
        EnableGui()
    End Sub

    Private Sub bwPopulate_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bwFF.ProgressChanged
    End Sub

    Private Sub bwPopulate_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bwFF.DoWork
        Try
            Dim t As SQLite.SQLiteTransaction
            t = MasterDB.BeginTransaction()
            For Each op As OrigPaths In OPaths
                bwPopulate(op.origpath, op.recursive, op)
            Next
            t.Commit()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub bwPopulate(ByVal SourcePath As String, ByVal recurse As Boolean, ByVal op As OrigPaths)
        Try
            If bwFF.CancellationPending Then
                Return
            End If
            Dim foundPath As String = String.Empty
            Dim SourceDir As DirectoryInfo = New DirectoryInfo(SourcePath)
            If excludesDirs.Contains(Path.GetFileName(SourceDir.FullName)) Then Return
            If SourceDir.Exists Then
                Dim ChildFile As FileInfo
                Try
                    For Each ChildFile In SourceDir.GetFiles()
                        'Me.bwFF.ReportProgress(1, New Object() {ChildFile.FullName, op})
                        AddToDB(ChildFile.FullName, op)
                    Next
        Catch ex As Exception
        End Try
                If recurse Then
                    Dim SubDir As DirectoryInfo
                    Try
                        For Each SubDir In SourceDir.GetDirectories()
                            bwPopulate(SubDir.FullName, recurse, op)
                            If bwFF.CancellationPending Then
                                Return
                            End If
                        Next
                    Catch ex As Exception
                    End Try
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Sub AddToDB(ByVal s As String, ByVal o As OrigPaths)
        Dim op As New OrigPaths
        op = o
        Dim fti As New EmberFiles
        fti.FTI = New FileToInstall
        fti.FTI.OriginalPath = Path.GetDirectoryName(s)
        fti.FTI.EmberPath = String.Concat(op.emberpath, Path.GetDirectoryName(s).Replace(op.origpath, String.Empty))
        fti.FTI.Filename = Path.GetFileName(s)
        fti.FTI.Hash = GetHash(s)
        fti.FTI.Platform = op.platform
        If Not excludesFiles.Contains(fti.FTI.Filename) Then
            fti.UseFile = True
        Else
            fti.UseFile = False
        End If
        MasterDB.DBAddChangeEmberFile(fti)
    End Sub

    Public Sub PopulateFileList(ByRef _f As FilesList)
        Dim o As New FileOfList
        Using SQLcommandEmberFiles As SQLite.SQLiteCommand = MasterDB.SQLcn.CreateCommand
            SQLcommandEmberFiles.CommandText = String.Concat("Select * FROM EmberFiles;")
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommandEmberFiles.ExecuteReader()
                While SQLreader.Read
                    If Convert.ToBoolean(SQLreader("UseFile")) Then
                        o = New FileOfList
                        If Not DBNull.Value.Equals(SQLreader("EmberPath")) Then o.Path = SQLreader("EmberPath").ToString
                        If Not DBNull.Value.Equals(SQLreader("Filename")) Then o.Filename = SQLreader("Filename").ToString
                        If Not DBNull.Value.Equals(SQLreader("Hash")) Then o.Hash = SQLreader("Hash").ToString
                        If Not DBNull.Value.Equals(SQLreader("Platform")) Then o.Platform = SQLreader("Platform").ToString
                        _f.Files.Add(o)
                    End If
                End While
            End Using
        End Using
    End Sub

    Sub LoadFiles(Optional ByVal showAll As Boolean = True)
        lstFiles.Items.Clear()
        txtEMMVersion.Text = ""
        RemoveHandler lstFiles.ItemCheck, AddressOf lstFiles_ItemCheck
        Using SQLcommand As SQLite.SQLiteCommand = MasterDB.SQLcn.CreateCommand
            SQLcommand.CommandText = String.Concat("SELECT OrigPath,EmberPath,Filename,Hash,UseFile,Platform FROM EmberFiles ORDER BY EmberPath,Filename  ;")
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                Dim o As EmberFiles
                While SQLreader.Read
                    o = New EmberFiles
                    o.FTI = New FileToInstall
                    If Not DBNull.Value.Equals(SQLreader("OrigPath")) Then o.FTI.OriginalPath = SQLreader("OrigPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("EmberPath")) Then o.FTI.EmberPath = SQLreader("EmberPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("Filename")) Then o.FTI.Filename = SQLreader("Filename").ToString
                    If Not DBNull.Value.Equals(SQLreader("Hash")) Then o.FTI.Hash = SQLreader("Hash").ToString
                    If Not DBNull.Value.Equals(SQLreader("Platform")) Then o.FTI.Platform = SQLreader("Platform").ToString
                    If Not DBNull.Value.Equals(SQLreader("UseFile")) Then o.UseFile = SQLreader("UseFile")
                    If (showAll OrElse o.UseFile) AndAlso (Platform = o.FTI.Platform OrElse o.FTI.Platform = "Common") Then
                        Dim i = New ListViewItem
                        i.Text = o.FTI.OriginalPath
                        i.SubItems.Add(o.FTI.Filename)
                        i.SubItems.Add(o.FTI.EmberPath)
                        i.SubItems.Add(o.FTI.Platform)
                        i.Checked = o.UseFile
                        lstFiles.Items.Add(i)
                        If o.FTI.EmberPath = "\" AndAlso o.FTI.Filename = "Ember Media Manager.exe" Then
                            If File.Exists(Path.Combine(o.FTI.OriginalPath, "Ember Media Manager.exe")) Then
                                CurrentEmberVersion = GetEmberVersion(o.FTI.OriginalPath)
                                CurrentEmberPlatform = GetEmberPlatform(o.FTI.OriginalPath)
                                txtEMMVersion.Text = CurrentEmberVersion
                            End If

                        End If
                    End If
                End While
            End Using
        End Using
        AddHandler lstFiles.ItemCheck, AddressOf lstFiles_ItemCheck
    End Sub

    Function GetHash(ByVal fname As String)
        Dim md5Hasher As New SHA1CryptoServiceProvider() 'As MD5
        'md5Hasher = MD5.Create()
        ' Convert the input string to a byte array and compute the hash.
        Dim fileReader As Byte()
        fileReader = My.Computer.FileSystem.ReadAllBytes(fname)
        Dim data As Byte() = md5Hasher.ComputeHash(fileReader)
        ' Create a new Stringbuilder to collect the bytes
        ' and create a string.
        Dim sBuilder As New StringBuilder()
        ' Loop through each byte of the hashed data 
        ' and format each one as a hexadecimal string.
        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i
        md5Hasher.Clear()
        ' Return the hexadecimal string.
        Return sBuilder.ToString()
    End Function

    Sub LoadVersions()
        lstVersions.Items.Clear()
        Me.lstVersions.ListViewItemSorter = New ListViewItemComparer(0)
        For Each v As Versions In EmberVersions.VersionList
            lstVersions.Items.Add(v.Version)
        Next
    End Sub

    Public Sub LoadOPaths()
        OPaths.Clear()
        Using SQLcommand As SQLite.SQLiteCommand = MasterDB.SQLcn.CreateCommand
            SQLcommand.CommandText = String.Concat("SELECT * FROM OrigPaths;")
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommand.ExecuteReader()
                Dim o As OrigPaths
                While SQLreader.Read
                    o = New OrigPaths
                    If Not DBNull.Value.Equals(SQLreader("OrigPath")) Then o.origpath = SQLreader("OrigPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("EmberPath")) Then o.emberpath = SQLreader("EmberPath").ToString
                    If Not DBNull.Value.Equals(SQLreader("Recursive")) Then o.recursive = SQLreader("Recursive").ToString
                    If Not DBNull.Value.Equals(SQLreader("Platform")) Then o.platform = SQLreader("Platform").ToString
                    OPaths.Add(o)
                End While
            End Using
        End Using
    End Sub

    Sub RemoveExcludeDirs(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs)
        MasterDB.DBDeleteExcludeDir(e.ClickedItem.Text)
        LoadExcludes()
        MasterDB.DBDeleteAllEmberFile()
        DoPopulateFiles()
    End Sub

    Sub PackFiles()
        Dim o As New FileOfList
        Using SQLcommandEmberFiles As SQLite.SQLiteCommand = MasterDB.SQLcn.CreateCommand
            SQLcommandEmberFiles.CommandText = String.Concat("Select * FROM EmberFiles;")
            Using SQLreader As SQLite.SQLiteDataReader = SQLcommandEmberFiles.ExecuteReader()
                While SQLreader.Read
                    If Convert.ToBoolean(SQLreader("UseFile")) Then
                        Dim srcFile As String = Path.Combine(SQLreader("OrigPath").ToString, SQLreader("Filename").ToString)
                        'Dim dstFile As String = Path.Combine(Path.Combine(AppPath, "Site\Files"), SQLreader("Filename").ToString) ' & ".gz")
                        Dim dstFile As String = Path.Combine(Path.Combine(AppPath, "Site\Files"), String.Concat(SQLreader("Hash").ToString, ".emm"))
                        If File.Exists(dstFile) Then File.Delete(dstFile)
                        File.Copy(srcFile, dstFile)
                        'CompressFile(srcFile, dstFile)
                    End If
                End While
            End Using
        End Using
    End Sub

    Public Shared Sub CompressFile(ByVal spath As String, ByVal dpath As String)
        Dim sourceFile As FileStream = File.OpenRead(spath)
        Dim destinationFile As FileStream = File.Create(dpath)

        Dim buffer(sourceFile.Length) As Byte
        sourceFile.Read(buffer, 0, buffer.Length)

        Using output As New GZipStream(destinationFile, _
            CompressionMode.Compress)

            Console.WriteLine("Compressing {0} to {1}.", sourceFile.Name, _
                destinationFile.Name, False)

            output.Write(buffer, 0, buffer.Length)
        End Using

        ' Close the files.
        sourceFile.Close()
        destinationFile.Close()
    End Sub

    Public Shared Sub UncompressFile(ByVal spath As String, ByVal dpath As String)
        Dim sourceFile As FileStream = File.OpenRead(spath)
        Dim destinationFile As FileStream = File.Create(dpath)

        ' Because the uncompressed size of the file is unknown, 
        ' we are imports an arbitrary buffer size.
        Dim buffer(4096) As Byte
        Dim n As Integer

        Using input As New GZipStream(sourceFile, _
            CompressionMode.Decompress, False)

            Console.WriteLine("Decompressing {0} to {1}.", sourceFile.Name, _
                destinationFile.Name)

            n = input.Read(buffer, 0, buffer.Length)
            destinationFile.Write(buffer, 0, n)
        End Using

        ' Close the files.
        sourceFile.Close()
        destinationFile.Close()
    End Sub

    Sub DisableGui()
        btnOriginPath.Enabled = False
        btnSaveVersion.Enabled = False
        btnRefresh.Enabled = False
        btnRescan.Enabled = False
        cbPlatform.Enabled = False
        pnlWork.Visible = True
        CheckBox1.Enabled = False
        lstFiles.Enabled = False
        ContextMenuStrip1.Enabled = False
        Button1.Enabled = False
    End Sub

    Sub EnableGui()
        pnlWork.Visible = False
        btnOriginPath.Enabled = True
        btnSaveVersion.Enabled = True
        btnRefresh.Enabled = True
        btnRescan.Enabled = True
        cbPlatform.Enabled = True
        CheckBox1.Enabled = True
        lstFiles.Enabled = True
        ContextMenuStrip1.Enabled = True
        Button1.Enabled = True
    End Sub

    Public Shared Sub DeleteDirectory(ByVal sPath As String)
        Try
            If Directory.Exists(sPath) Then
                Dim Dirs As New ArrayList
                Try
                    Dirs.AddRange(Directory.GetDirectories(sPath))
                Catch
                End Try
                For Each inDir As String In Dirs
                    DeleteDirectory(inDir)
                Next
                Dim fFiles As New ArrayList
                Try
                    fFiles.AddRange(Directory.GetFiles(sPath))
                Catch
                End Try
                For Each fFile As String In fFiles
                    Try
                        File.Delete(fFile)
                    Catch
                    End Try
                Next
                Directory.Delete(sPath, True)
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnSaveVesrion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveVersion.Click
        pnlWork.Visible = True
        Application.DoEvents()
        Dim v As New Versions
        v.Version = txtEMMVersion.Text
        Dim found As Boolean = False
        For Each f As Versions In EmberVersions.VersionList
            If f.Version = v.Version Then
                found = True
                Exit For
            End If
        Next
        If Not found Then
            EmberVersions.VersionList.Add(v)
        End If
        EmberVersions.Save(Path.Combine(AppPath, "site\versionlist.xml"))
        Dim _files As New FilesList
        _files.Files = New List(Of FileOfList)
        PopulateFileList(_files)
        _files.Save(Path.Combine(AppPath, String.Format("site\version_{0}.xml", v.Version)))
        Dim _cmds As New InstallCommands
        _cmds.Command = New List(Of InstallCommand)
        If Not File.Exists(Path.Combine(AppPath, String.Format("site\commands_base.xml"))) Then
            For Each s As String In DefaultStrings.Tables
                _cmds.Command.Add(New InstallCommand With {.CommandType = "DB", .CommandExecute = s})
            Next
            _cmds.Save(Path.Combine(AppPath, String.Format("site\commands_base.xml")))
            _cmds.Command.Clear()
        End If

        '_cmds.Save(Path.Combine(AppPath, String.Format("site\commands_{0}.xml", v.Version)))
        pnlWork.Visible = False
        LoadVersions()
    End Sub

    Private Sub btnAddPath_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOriginPath.Click
        Using opath As New dlgOrigPaths
            opath.ShowDialog()
        End Using
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmMainManager_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        MasterDB.Connect()
    End Sub

    Private Sub frmMainManager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Directory.CreateDirectory(Path.Combine(AppPath, "Site"))
        Directory.CreateDirectory(Path.Combine(AppPath, "Site\Files"))
        MasterDB.Connect()
        LoadOPaths()
        cbPlatform.SelectedIndex = 0
        LoadExcludes()
        EmberVersions.VersionList.Clear()
        If File.Exists(Path.Combine(AppPath, "site\versionlist.xml")) Then
            Dim xmlSer As New XmlSerializer(GetType(UpgradeList))
            Using xmlSW As New StreamReader(Path.Combine(AppPath, "site\versionlist.xml"))
                EmberVersions = xmlSer.Deserialize(xmlSW)
            End Using
            LoadVersions()
        End If
        _cmds.Command = New List(Of InstallCommand)
    End Sub

    Private Sub btnRescan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRescan.Click
        MasterDB.DBDeleteAllEmberFile()
        DoPopulateFiles()

    End Sub

    Private Sub lstFiles_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lstFiles.ItemCheck
        Dim o As New EmberFiles
        o = MasterDB.DBGetEmberFile(lstFiles.Items(e.Index).Text, lstFiles.Items(e.Index).SubItems(1).Text)
        o.UseFile = e.NewValue
        MasterDB.DBAddChangeEmberFile(o)
    End Sub

    Private Sub cbPlatform_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPlatform.SelectedIndexChanged
        Platform = cbPlatform.Text
        LoadFiles(CheckBox1.Checked)
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        LoadFiles(CheckBox1.Checked)
    End Sub

    Private Sub AllwaysExcludeFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllwaysExcludeFileToolStripMenuItem.Click
        If lstFiles.SelectedItems.Count > 0 Then
            Dim ee As New EmberFiles
            ee = MasterDB.DBGetEmberFile(lstFiles.SelectedItems(0).Text, lstFiles.SelectedItems(0).SubItems(1).Text)
            ee.UseFile = False
            MasterDB.DBAddChangeEmberFile(ee)
            MasterDB.DBAddChangeExcludeFile(lstFiles.SelectedItems(0).SubItems(1).Text)
            LoadExcludes()
            LoadFiles(CheckBox1.Checked)
        End If
    End Sub

    Private Sub ContextMenuStrip1_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        If lstFiles.SelectedItems.Count > 0 Then
            If excludesFiles.Contains(lstFiles.SelectedItems(0).SubItems(1).Text) Then
                AllwaysExcludeFileToolStripMenuItem.Visible = False
                RemoveExclusionToolStripMenuItem.Visible = True
            Else
                AllwaysExcludeFileToolStripMenuItem.Visible = True
                RemoveExclusionToolStripMenuItem.Visible = False
            End If
            If excludesDirs.Contains(Path.GetFileName(lstFiles.SelectedItems(0).SubItems(0).Text)) Then
                AllwaysExcludeFolderToolStripMenuItem.Visible = False
            Else
                AllwaysExcludeFolderToolStripMenuItem.Visible = True
            End If
            If excludesDirs.Count > 0 Then
                RemoveFolderExclusionToolStripMenuItem.DropDownItems.Clear()
                For Each s As String In excludesDirs
                    Dim i As ToolStripMenuItem
                    RemoveFolderExclusionToolStripMenuItem.DropDownItems.Add(s)
                    i = RemoveFolderExclusionToolStripMenuItem
                    AddHandler i.DropDownItemClicked, AddressOf RemoveExcludeDirs
                Next
                RemoveFolderExclusionToolStripMenuItem.Visible = True
            Else
                RemoveFolderExclusionToolStripMenuItem.Visible = False
            End If
        End If
    End Sub

    Private Sub RemoveExclusionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveExclusionToolStripMenuItem.Click
        MasterDB.DBDeleteExcludeFile(lstFiles.SelectedItems(0).SubItems(1).Text)
        LoadExcludes()
    End Sub

    Private Sub AllwaysExcludeFolderToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllwaysExcludeFolderToolStripMenuItem.Click
        If lstFiles.SelectedItems.Count > 0 Then
            MasterDB.DBAddChangeExcludeDir(Path.GetFileName(lstFiles.SelectedItems(0).SubItems(0).Text))
            MasterDB.DBDeleteAllEmberFile()
            DoPopulateFiles()
            'LoadExcludes()
            'LoadFiles(CheckBox1.Checked)
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        DoPopulateFiles()
    End Sub

    Private Sub txtEMMVersion_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEMMVersion.TextChanged
        If Not txtEMMVersion.Text = String.Empty Then
            btnSaveVersion.Enabled = True
        Else
            btnSaveVersion.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        pnlWork.Visible = True
        Application.DoEvents()
        DeleteDirectory(Path.Combine(AppPath, "Site\Files"))
        Directory.CreateDirectory(Path.Combine(AppPath, "Site\Files"))
        PackFiles()
        pnlWork.Visible = False
    End Sub


    Class ListViewItemComparer
        Implements IComparer
        ' Implements the manual sorting of items by columns.
        Private col As Integer

        Public Sub New()
            col = 0
        End Sub

        Public Sub New(ByVal column As Integer)
            col = column
        End Sub

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
           Implements IComparer.Compare
            Return -1 * Convert.ToInt32(CType(x, ListViewItem).SubItems(col).Text).CompareTo(Convert.ToInt32(CType(y, ListViewItem).SubItems(col).Text))
            'Return Convert.ToInt32(x.text).CompareTo(Convert.ToInt32(y))
        End Function
    End Class

    Private Sub lstVersions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstVersions.SelectedIndexChanged
        If lstVersions.SelectedItems.Count > 0 Then
            If Not InCommandTab Then
                lstVersions.SelectedItems(0).Selected = False
            Else
                LoadCommands(lstVersions.SelectedItems(0).Text)
                ShowCommands()
                CmdsChanged = False
                CheckCommandButtons()
                gbCommands.Visible = False
                btnNew.Enabled = True
            End If
        End If
    End Sub

    Private Sub TabPage2_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage2.Enter
        InCommandTab = True
        ShowCommands()
    End Sub

    Private Sub TabPage1_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabPage1.Enter
        InCommandTab = False
        CmdVersion = String.Empty
        lstCommands.Items.Clear()
        CmdsChanged = False
        CheckCommandButtons()
        btnNew.Enabled = False
        If lstVersions.SelectedItems.Count > 0 Then
            lstVersions.SelectedItems(0).Selected = False
        End If
    End Sub
    Sub ShowCommands()
        If Not CmdVersion = String.Empty Then
            Label8.Text = String.Format("Commands for Version: {0}", CmdVersion)
            lstCommands.Items.Clear()
            Dim i As ListViewItem
            For Each t As InstallCommand In _cmds.Command
                i = New ListViewItem
                i = lstCommands.Items.Add(t.CommandType)
                i.SubItems.Add(t.CommandDescription)
                i.Tag = t.CommandExecute
            Next
        End If
    End Sub
    Sub LoadCommands(ByVal n As String)
        Dim p As String = Path.Combine(AppPath, "Site")
        Dim f As String = String.Format("commands_{0}.xml", n)
        _cmds.Command.Clear()
        If File.Exists(Path.Combine(p, f)) Then
            Dim xmlSer As XmlSerializer
            xmlSer = New XmlSerializer(GetType(InstallCommands))
            Using xmlSW As New StreamReader(Path.Combine(p, f))
                _cmds = xmlSer.Deserialize(xmlSW)
            End Using
        End If
        CmdVersion = n
    End Sub


    Sub SaveCommands(Optional ByVal savetolist As Boolean = True)
        If savetolist Then
            lstCommands.SelectedItems(0).Text = cbType.SelectedItem
            lstCommands.SelectedItems(0).SubItems(1).Text = txtDescriptions.Text
            lstCommands.SelectedItems(0).Tag = txtCommand.Text
        End If
        Dim p As String = Path.Combine(AppPath, "Site")
        Dim f As String = String.Format("commands_{0}.xml", CmdVersion)
        _cmds.Command.Clear()
        For Each s As ListViewItem In lstCommands.Items
            _cmds.Command.Add(New InstallCommand With {.CommandType = s.Text, .CommandDescription = s.SubItems(1).Text, .CommandExecute = s.Tag})
        Next
        _cmds.Save(Path.Combine(p, f))
    End Sub
    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cbType.SelectedIndex = 0
        CmdsChanged = False
        CheckCommandButtons()
        gbCommands.Visible = True
        txtCommand.Text = String.Empty
        txtDescriptions.Text = String.Empty
        AddCommand = True
    End Sub

    Private Sub lstCommands_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstCommands.SelectedIndexChanged
        If lstCommands.SelectedItems.Count > 0 AndAlso Not AddCommand Then
            txtCommand.Text = lstCommands.SelectedItems(0).Tag
            cbType.SelectedIndex = cbType.FindString(lstCommands.SelectedItems(0).Text)
            txtDescriptions.Text = lstCommands.SelectedItems(0).SubItems(1).Text
            CmdsChanged = False
            CheckCommandButtons()
            gbCommands.Visible = True
            btnRemove.Enabled = True
        End If
    End Sub
    Sub CheckCommandButtons()
        If CmdsChanged Then
            btnSave.Enabled = True
            btnRemove.Enabled = True
            CmdsChanged = False
            btnRemove.Text = "Cancel"
        Else
            btnSave.Enabled = False
            btnRemove.Enabled = False
            gbCommands.Visible = False
            btnRemove.Text = "Remove"
        End If
    End Sub

    Private Sub txtCommand_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCommand.TextChanged
        CmdsChanged = True
        CheckCommandButtons()
    End Sub

    Private Sub cbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbType.SelectedIndexChanged
        CmdsChanged = True
        CheckCommandButtons()
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDescriptions.TextChanged
        CmdsChanged = True
        CheckCommandButtons()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If AddCommand Then
            Dim i As ListViewItem = lstCommands.Items.Add(cbType.SelectedItem)
            i.Selected = True
            i.SubItems.Add("")
        End If
        SaveCommands()
        LoadCommands(CmdVersion)
        ShowCommands()
        CmdsChanged = False
        CheckCommandButtons()
        gbCommands.Visible = False
        AddCommand = False
    End Sub

    Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        If btnRemove.Text = "Cancel" Then
            CmdsChanged = False
            CheckCommandButtons()
        End If
        If btnRemove.Text = "Remove" Then
            CmdsChanged = False
            CheckCommandButtons()
            lstCommands.SelectedItems(0).Remove()
            SaveCommands(False)
            LoadCommands(CmdVersion)
            ShowCommands()
        End If
        AddCommand = False
    End Sub
End Class