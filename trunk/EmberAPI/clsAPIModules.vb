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

' Nuno Reminders:
' TODO: Need to do "strings" on all this stuff..
'
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class ModulesManager
    Public RuntimeObjects As New EmberRuntimeObjects
    Public externalProcessorModules As New List(Of _externalGenericModuleClass)
    Public externalScrapersModules As New List(Of _externalScraperModuleClass)
    Public externalTVScrapersModules As New List(Of _externalTVScraperModuleClass)
    Public Shared AssemblyList As New List(Of AssemblyListItem)
    Public Event GenericEvent(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object))
    'Singleton Instace for module manager .. allways use this one
    Private Shared Singleton As ModulesManager = Nothing
    Private moduleLocation As String = Path.Combine(Functions.AppPath, "Modules")
    Public Shared VersionList As New List(Of VersionItem)

    Public Shared ReadOnly Property Instance() As ModulesManager
        Get
            If (Singleton Is Nothing) Then
                Singleton = New ModulesManager()
            End If
            Return Singleton
        End Get
    End Property
    Structure AssemblyListItem
        Public AssemblyName As String
        Public Assembly As System.Reflection.Assembly
    End Structure
    Structure VersionItem
        Public Name As String
        Public Version As String
        Public AssemblyFileName As String
        Public NeedUpdate As Boolean
    End Structure

    Class EmberRuntimeObjects
        'all runtime object including Function (delegate) that need to be exposed to Modules
        Delegate Sub OpenImageViewer(ByVal _Image As Image)
        Delegate Sub LoadMedia(ByVal Scan As Structures.Scans, ByVal SourceName As String)

        Private _OpenImageViewer As OpenImageViewer
        Private _LoadMedia As LoadMedia
        Private _TopMenu As System.Windows.Forms.MenuStrip
        Private _MenuMediaList As System.Windows.Forms.ContextMenuStrip
        Private _MediaList As System.Windows.Forms.DataGridView
        Private _MainTool As System.Windows.Forms.ToolStrip
        Private _TrayMenu As System.Windows.Forms.ContextMenuStrip

        Public Sub DelegateLoadMedia(ByRef lm As LoadMedia) 'Setup from EmberAPP
            _LoadMedia = lm
        End Sub
        Public Sub InvokeLoadMedia(ByVal Scan As Structures.Scans, ByVal SourceName As String) 'Invoked from Modules
            _LoadMedia.Invoke(Scan, SourceName)
        End Sub
        Public Sub DelegateOpenImageViewer(ByRef IV As OpenImageViewer)
            _OpenImageViewer = IV
        End Sub
        Public Sub InvokeOpenImageViewer(ByRef _image As Image)
            _OpenImageViewer.Invoke(_image)
        End Sub
        Public Property MainTool() As System.Windows.Forms.ToolStrip
            Get
                Return _MainTool
            End Get
            Set(ByVal value As System.Windows.Forms.ToolStrip)
                _MainTool = value
            End Set
        End Property
        Public Property TopMenu() As System.Windows.Forms.MenuStrip
            Get
                Return _TopMenu
            End Get
            Set(ByVal value As System.Windows.Forms.MenuStrip)
                _TopMenu = value
            End Set
        End Property
        Public Property MenuMediaList() As System.Windows.Forms.ContextMenuStrip
            Get
                Return _MenuMediaList
            End Get
            Set(ByVal value As System.Windows.Forms.ContextMenuStrip)
                _MenuMediaList = value
            End Set
        End Property
        Public Property MediaList() As System.Windows.Forms.DataGridView
            Get
                Return _MediaList
            End Get
            Set(ByVal value As System.Windows.Forms.DataGridView)
                _MediaList = value
            End Set
        End Property
        Public Property TrayMenu() As System.Windows.Forms.ContextMenuStrip
            Get
                Return _TrayMenu
            End Get
            Set(ByVal value As System.Windows.Forms.ContextMenuStrip)
                _TrayMenu = value
            End Set
        End Property
    End Class

    <XmlRoot("EmberModule")> _
    Class _XMLEmberModuleClass
        Public Enabled As Boolean
        Public ScraperEnabled As Boolean
        Public PostScraperEnabled As Boolean
        Public AssemblyName As String
        Public AssemblyFileName As String
        Public ScraperOrder As Integer
        Public PostScraperOrder As Integer
    End Class
    Class _externalGenericModuleClass
        Public ProcessorModule As Interfaces.EmberExternalModule 'Object
        'Public Enabled As Boolean
        Public AssemblyName As String
        Public AssemblyFileName As String
        Public Type As List(Of Enums.ModuleEventType)
        Public ModuleOrder As Integer 'TODO: not important at this point.. for 1.5
    End Class
    Class _externalScraperModuleClass
        Public ProcessorModule As Interfaces.EmberMovieScraperModule 'Object
        Public AssemblyName As String
        Public AssemblyFileName As String
        Public ScraperOrder As Integer
        Public PostScraperOrder As Integer
    End Class
    Class _externalTVScraperModuleClass
        Public ProcessorModule As Interfaces.EmberTVScraperModule  'Object
        Public AssemblyName As String
        Public AssemblyFileName As String
        Public ScraperOrder As Integer
        Public PostScraperOrder As Integer
    End Class
    ''' <summary>
    ''' Load all Generic Modules and field in externalProcessorModules List
    ''' </summary>
    Public Sub loadModules()
        If Directory.Exists(moduleLocation) Then
            'Assembly to load the file
            Dim assembly As System.Reflection.Assembly
            'For each .dll file in the module directory
            For Each file As String In System.IO.Directory.GetFiles(moduleLocation, "*.dll")
                Try

                    'Load the assembly
                    assembly = System.Reflection.Assembly.LoadFile(file)
                    'Loop through each of the assemeblies type
                    For Each fileType As Type In assembly.GetTypes
                        Try
                            'Activate the located module
                            Dim t As Type = fileType.GetInterface("EmberExternalModule")
                            If Not t Is Nothing Then
                                Dim ProcessorModule As Interfaces.EmberExternalModule 'Object
                                ProcessorModule = CType(Activator.CreateInstance(fileType), Interfaces.EmberExternalModule)
                                'Add the activated module to the arraylist
                                Dim _externalProcessorModule As New _externalGenericModuleClass
                                Dim filename As String = file
                                If String.IsNullOrEmpty(AssemblyList.FirstOrDefault(Function(x) x.AssemblyName = Path.GetFileNameWithoutExtension(filename)).AssemblyName) Then
                                    AssemblyList.Add(New AssemblyListItem With {.AssemblyName = Path.GetFileNameWithoutExtension(filename), .Assembly = assembly})
                                End If
                                _externalProcessorModule.ProcessorModule = ProcessorModule
                                _externalProcessorModule.AssemblyName = String.Concat(Path.GetFileNameWithoutExtension(file), ".", fileType.FullName)
                                _externalProcessorModule.AssemblyFileName = Path.GetFileName(file)
                                _externalProcessorModule.Type = ProcessorModule.ModuleType
                                Dim found As Boolean = False
                                For Each i In Master.eSettings.EmberModules
                                    If i.AssemblyName = _externalProcessorModule.AssemblyName Then
                                        _externalProcessorModule.ProcessorModule.Enabled = i.Enabled
                                        found = True
                                    End If
                                Next
                                If Not found AndAlso Path.GetFileNameWithoutExtension(file).Contains("generic.EmberCore") Then
                                    _externalProcessorModule.ProcessorModule.Enabled = True
                                    SetModuleEnable(_externalProcessorModule.AssemblyName, True)
                                End If
                                externalProcessorModules.Add(_externalProcessorModule)
                                ProcessorModule.Init(_externalProcessorModule.AssemblyName)
                                AddHandler ProcessorModule.GenericEvent, AddressOf GenericRunCallBack
                                ProcessorModule.Enabled = _externalProcessorModule.ProcessorModule.Enabled
                            End If
                        Catch ex As Exception
                        End Try
                    Next
                Catch ex As Exception
                End Try
            Next
            Dim c As Integer = 0
            For Each ext As _externalGenericModuleClass In externalProcessorModules.OrderBy(Function(x) x.ModuleOrder)
                ext.ModuleOrder = c
                c += 1
            Next

        End If
    End Sub
    Private Sub GenericRunCallBack(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object))
        RaiseEvent GenericEvent(mType, _params)
    End Sub
    ''' <summary>
    ''' Load all Scraper Modules and field in externalScrapersModules List
    ''' </summary>

    Public Sub loadScrapersModules()
        Dim ScraperAnyEnabled As Boolean = False
        Dim PostScraperAnyEnabled As Boolean = False
        If Directory.Exists(moduleLocation) Then
            'Assembly to load the file
            Dim assembly As System.Reflection.Assembly
            'For each .dll file in the module directory
            Dim loaded As Boolean = False
            For Each file As String In System.IO.Directory.GetFiles(moduleLocation, "*.dll")
                assembly = System.Reflection.Assembly.LoadFile(file)
                'Loop through each of the assemeblies type
                Try
                    For Each fileType As Type In assembly.GetTypes

                        'Activate the located module
                        Dim t As Type = fileType.GetInterface("EmberMovieScraperModule")
                        If Not t Is Nothing Then
                            Dim ProcessorModule As Interfaces.EmberMovieScraperModule
                            ProcessorModule = CType(Activator.CreateInstance(fileType), Interfaces.EmberMovieScraperModule)
                            'Add the activated module to the arraylist
                            Dim _externalScraperModule As New _externalScraperModuleClass
                            Dim filename As String = file
                            If String.IsNullOrEmpty(AssemblyList.FirstOrDefault(Function(x) x.AssemblyName = Path.GetFileNameWithoutExtension(filename)).AssemblyName) Then
                                AssemblyList.Add(New AssemblyListItem With {.AssemblyName = Path.GetFileNameWithoutExtension(filename), .Assembly = assembly})
                            End If
                            _externalScraperModule.ProcessorModule = ProcessorModule
                            _externalScraperModule.AssemblyName = String.Concat(Path.GetFileNameWithoutExtension(file), ".", fileType.FullName)
                            _externalScraperModule.AssemblyFileName = Path.GetFileName(file)
                            Dim found As Boolean = False

                            For Each i As _XMLEmberModuleClass In Master.eSettings.EmberModules.Where(Function(x) x.AssemblyName = _externalScraperModule.AssemblyName)
                                _externalScraperModule.ProcessorModule.ScraperEnabled = i.ScraperEnabled
                                ScraperAnyEnabled = ScraperAnyEnabled Or i.ScraperEnabled
                                _externalScraperModule.ProcessorModule.PostScraperEnabled = i.PostScraperEnabled
                                PostScraperAnyEnabled = PostScraperAnyEnabled Or i.PostScraperEnabled
                                _externalScraperModule.ScraperOrder = i.ScraperOrder
                                _externalScraperModule.PostScraperOrder = i.PostScraperOrder
                                found = True
                            Next
                            If Not found Then
                                _externalScraperModule.ScraperOrder = 999
                                _externalScraperModule.PostScraperOrder = 999
                            End If
                            externalScrapersModules.Add(_externalScraperModule)
                            _externalScraperModule.ProcessorModule.Init(_externalScraperModule.AssemblyName)
                            loaded = True
                        End If
                    Next
                    If loaded Then Master.eLang.LoadLanguage(Master.eSettings.Language, Path.GetFileNameWithoutExtension(file))
                    loaded = False
                Catch ex As Exception
                End Try
            Next
            Dim c As Integer = 0
            For Each ext As _externalScraperModuleClass In externalScrapersModules.OrderBy(Function(x) x.ScraperOrder) ' .Where(Function(x) x.ProcessorModule.ScraperEnabled)
                ext.ScraperOrder = c
                c += 1
            Next
            c = 0
            For Each ext As _externalScraperModuleClass In externalScrapersModules.OrderBy(Function(x) x.PostScraperOrder) '.Where(Function(x) x.ProcessorModule.PostScraperEnabled)
                ext.PostScraperOrder = c
                c += 1
            Next
            If Not ScraperAnyEnabled Then
                SetScraperEnable("scraper.EmberCore.EmberScraperModule.EmberNativeScraperModule", True)
                SetScraperOrder("scraper.EmberCore.EmberScraperModule.EmberNativeScraperModule", 1)
            End If
            If Not PostScraperAnyEnabled Then
                SetPostScraperEnable("scraper.EmberCore.EmberScraperModule.EmberNativeScraperModule", True)
                SetPostScraperOrder("scraper.EmberCore.EmberScraperModule.EmberNativeScraperModule", 1)
            End If
        End If
    End Sub


    Public Sub loadTVScrapersModules()
        Dim ScraperAnyEnabled As Boolean = False
        Dim PostScraperAnyEnabled As Boolean = False
        If Directory.Exists(moduleLocation) Then
            'Assembly to load the file
            Dim assembly As System.Reflection.Assembly
            'For each .dll file in the module directory
            Dim loaded As Boolean = False
            For Each file As String In System.IO.Directory.GetFiles(moduleLocation, "*.dll")
                assembly = System.Reflection.Assembly.LoadFile(file)
                'Loop through each of the assemeblies type
                Try
                    For Each fileType As Type In assembly.GetTypes

                        'Activate the located module
                        Dim t As Type = fileType.GetInterface("EmberTVScraperModule")
                        If Not t Is Nothing Then
                            Dim ProcessorModule As Interfaces.EmberTVScraperModule
                            ProcessorModule = CType(Activator.CreateInstance(fileType), Interfaces.EmberTVScraperModule)
                            'Add the activated module to the arraylist
                            Dim _externaltvScraperModule As New _externalTVScraperModuleClass
                            Dim filename As String = file
                            If String.IsNullOrEmpty(AssemblyList.FirstOrDefault(Function(x) x.AssemblyName = Path.GetFileNameWithoutExtension(filename)).AssemblyName) Then
                                AssemblyList.Add(New AssemblyListItem With {.AssemblyName = Path.GetFileNameWithoutExtension(filename), .Assembly = assembly})
                            End If

                            _externaltvScraperModule.ProcessorModule = ProcessorModule
                            _externaltvScraperModule.AssemblyName = String.Concat(Path.GetFileNameWithoutExtension(file), ".", fileType.FullName)
                            _externaltvScraperModule.AssemblyFileName = Path.GetFileName(file)
                            Dim found As Boolean = False

                            For Each i As _XMLEmberModuleClass In Master.eSettings.EmberModules.Where(Function(x) x.AssemblyName = _externaltvScraperModule.AssemblyName)
                                _externaltvScraperModule.ProcessorModule.ScraperEnabled = i.ScraperEnabled
                                ScraperAnyEnabled = ScraperAnyEnabled Or i.ScraperEnabled
                                _externaltvScraperModule.ProcessorModule.PostScraperEnabled = i.PostScraperEnabled
                                PostScraperAnyEnabled = PostScraperAnyEnabled Or i.PostScraperEnabled
                                _externaltvScraperModule.ScraperOrder = i.ScraperOrder
                                _externaltvScraperModule.PostScraperOrder = i.PostScraperOrder
                                found = True
                            Next
                            If Not found Then
                                _externaltvScraperModule.ScraperOrder = 999
                                _externaltvScraperModule.PostScraperOrder = 999
                            End If
                            externalTVScrapersModules.Add(_externaltvScraperModule)
                            _externaltvScraperModule.ProcessorModule.Init(_externaltvScraperModule.AssemblyName)
                            AddHandler _externaltvScraperModule.ProcessorModule.TVScraperEvent, AddressOf Handler_TVScraperEvent
                            loaded = True
                        End If
                    Next
                    If loaded Then Master.eLang.LoadLanguage(Master.eSettings.Language, Path.GetFileNameWithoutExtension(file))
                    loaded = False
                Catch ex As Exception
                End Try
            Next
            Dim c As Integer = 0
            For Each ext As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(x) x.ProcessorModule.ScraperEnabled)
                ext.ScraperOrder = c
                c += 1
            Next
            c = 0
            For Each ext As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(x) x.ProcessorModule.PostScraperEnabled)
                ext.PostScraperOrder = c
                c += 1
            Next
            If Not ScraperAnyEnabled Then
                SetTVScraperEnable("scraper.EmberCore.EmberScraperModule.EmberNativeTVScraperModule", True)
                SetTVScraperOrder("scraper.EmberCore.EmberScraperModule.EmberNativeTVScraperModule", 1)

            End If
            If Not PostScraperAnyEnabled Then
                SetTVPostScraperEnable("scraper.EmberCore.EmberScraperModule.EmberNativeTVScraperModule", True)
                SetTVPostScraperOrder("scraper.EmberCore.EmberScraperModule.EmberNativeTVScraperModule", 1)
            End If
        End If
    End Sub

    Public Sub LoadAllModules()
        loadModules()
        loadScrapersModules()
        loadTVScrapersModules()
        BuildVersionList()
    End Sub
    Public Function ScrapersCount() As Integer
        Return externalScrapersModules.Count
    End Function
    Public Function GetScraperByIdx(ByVal idx As Integer) As _externalScraperModuleClass
        Return externalScrapersModules(idx)
    End Function
    Public Sub Setup()
        Dim modulesSetup As New dlgModuleSettings
        For Each _externalProcessorModule As _externalGenericModuleClass In externalProcessorModules
            Dim li As ListViewItem = modulesSetup.lstModules.Items.Add(_externalProcessorModule.ProcessorModule.ModuleName())
            li.SubItems.Add(If(_externalProcessorModule.ProcessorModule.Enabled, Master.eLang.GetString(774, "Enabled"), Master.eLang.GetString(775, "Disabled")))
            li.Tag = _externalProcessorModule.AssemblyName
        Next
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.OrderBy(Function(x) x.ScraperOrder)
            Dim liS As New ListViewItem
            If _externalScraperModule.ProcessorModule.IsScraper Then
                liS = modulesSetup.lstScrapers.Items.Add(_externalScraperModule.ProcessorModule.ModuleName())
                liS.SubItems.Add(If(_externalScraperModule.ProcessorModule.ScraperEnabled, Master.eLang.GetString(774, "Enabled"), Master.eLang.GetString(775, "Disabled")))
                liS.Tag = _externalScraperModule.AssemblyName
            End If
        Next
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.OrderBy(Function(x) x.PostScraperOrder)
            Dim liPS As New ListViewItem
            If _externalScraperModule.ProcessorModule.IsPostScraper Then
                liPS = modulesSetup.lstPostScrapers.Items.Add(_externalScraperModule.ProcessorModule.ModuleName())
                liPS.SubItems.Add(If(_externalScraperModule.ProcessorModule.PostScraperEnabled, Master.eLang.GetString(774, "Enabled"), Master.eLang.GetString(775, "Disabled")))
                liPS.Tag = _externalScraperModule.AssemblyName
            End If
        Next
        modulesSetup.ModulesManager = Me
        modulesSetup.ShowDialog()
    End Sub
    Private Sub BuildVersionList()
        VersionList.Clear()
        VersionList.Add(New VersionItem With {.AssemblyFileName = "*EmberAPP", .Name = "Ember Application", .Version = My.Application.Info.Version.Revision.ToString})
        VersionList.Add(New VersionItem With {.AssemblyFileName = "*EmberAPI", .Name = "Ember API", .Version = Functions.EmberAPIVersion()})
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules
            VersionList.Add(New VersionItem With {.Name = _externalScraperModule.ProcessorModule.ModuleName, _
                    .AssemblyFileName = _externalScraperModule.AssemblyFileName, _
                    .Version = _externalScraperModule.ProcessorModule.ModuleVersion})
        Next
        For Each _externalModule As _externalGenericModuleClass In externalProcessorModules
            VersionList.Add(New VersionItem With {.Name = _externalModule.ProcessorModule.ModuleName, _
                    .AssemblyFileName = _externalModule.AssemblyFileName, _
                    .Version = _externalModule.ProcessorModule.ModuleVersion})
        Next
    End Sub

    Public Sub GetVersions()
        Dim dlgVersions As New dlgVersions
        Dim li As ListViewItem
        For Each v As VersionItem In VersionList
            li = dlgVersions.lstVersions.Items.Add(v.Name)
            li.SubItems.Add(v.Version)
        Next
        dlgVersions.ShowDialog()
    End Sub

    Public Sub RunModuleSetup(ByVal ModuleAssembly As String)
        For Each _externalProcessorModule As _externalGenericModuleClass In externalProcessorModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            '_externalProcessorModule.ProcessorModule.InjectSetup()
        Next
    End Sub
    Public Function RunGeneric(ByVal mType As Enums.ModuleEventType, ByRef _params As List(Of Object), Optional ByVal RunOnlyOne As Boolean = False) As Boolean
        Dim ret As Interfaces.ModuleResult
        For Each _externalGenericModule As _externalGenericModuleClass In externalProcessorModules.Where(Function(e) e.ProcessorModule.ModuleType.Contains(mType) AndAlso e.ProcessorModule.Enabled)
            ret = _externalGenericModule.ProcessorModule.RunGeneric(mType, _params)
            If ret.breakChain OrElse RunOnlyOne Then Exit For
        Next
        Return ret.Cancelled
    End Function

    Public Sub SetModuleEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalProcessorModule As _externalGenericModuleClass In externalProcessorModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalProcessorModule.ProcessorModule.Enabled = value
            _externalProcessorModule.ProcessorModule.Enabled = value
        Next
    End Sub

    Public Sub SetScraperEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ProcessorModule.ScraperEnabled = value
        Next
    End Sub

    Public Sub SetPostScraperEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ProcessorModule.PostScraperEnabled = value
        Next
    End Sub
    Public Sub SetScraperOrder(ByVal ModuleAssembly As String, ByVal value As Integer)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ScraperOrder = value
        Next
    End Sub
    Public Sub SetPostScraperOrder(ByVal ModuleAssembly As String, ByVal value As Integer)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.PostScraperOrder = value
        Next
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Event MovieScraperEvent(ByVal eType As Enums.MovieScraperEventType, ByVal Parameter As Object)
    Public Sub Handler_MovieScraperEvent(ByVal eType As Enums.MovieScraperEventType, ByVal Parameter As Object)
        RaiseEvent MovieScraperEvent(eType, Parameter)
    End Sub

    Public Function MovieScrapeOnly(ByRef DBMovie As Structures.DBMovie, ByVal ScrapeType As Enums.ScrapeType, ByVal Options As Structures.ScrapeOptions) As Boolean
        Dim ret As Interfaces.ModuleResult
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
            AddHandler _externalScraperModule.ProcessorModule.MovieScraperEvent, AddressOf Handler_MovieScraperEvent
            ret = _externalScraperModule.ProcessorModule.Scraper(DBMovie, ScrapeType, Options)
            RemoveHandler _externalScraperModule.ProcessorModule.MovieScraperEvent, AddressOf Handler_MovieScraperEvent
            If ret.breakChain Then Exit For
        Next
        Return ret.Cancelled
    End Function
    Public Function MoviePostScrapeOnly(ByRef DBMovie As Structures.DBMovie, ByVal ScrapeType As Enums.ScrapeType) As Boolean
        Dim ret As Interfaces.ModuleResult
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.ProcessorModule.IsPostScraper AndAlso e.ProcessorModule.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            AddHandler _externalScraperModule.ProcessorModule.MovieScraperEvent, AddressOf Handler_MovieScraperEvent
            ret = _externalScraperModule.ProcessorModule.PostScraper(DBMovie, ScrapeType)
            RemoveHandler _externalScraperModule.ProcessorModule.MovieScraperEvent, AddressOf Handler_MovieScraperEvent
            If ret.breakChain Then Exit For
        Next
        Return ret.Cancelled
    End Function

    Function ScraperSelectImageOfType(ByRef DBMovie As Structures.DBMovie, ByVal _DLType As Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False, Optional ByVal preload As Boolean = False) As Boolean

        Dim ret As Interfaces.ModuleResult
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.ProcessorModule.IsPostScraper AndAlso e.ProcessorModule.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externalScraperModule.ProcessorModule.SelectImageOfType(DBMovie, _DLType, pResults, _isEdit, preload)
            If ret.breakChain Then Exit For
        Next
        Return ret.Cancelled
    End Function

    Function ScraperDownloadTrailer(ByRef DBMovie As Structures.DBMovie) As String

        Dim ret As Interfaces.ModuleResult
        Dim sURL As String = String.Empty
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.ProcessorModule.IsPostScraper AndAlso e.ProcessorModule.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externalScraperModule.ProcessorModule.DownloadTrailer(DBMovie, sURL)
            If ret.breakChain Then Exit For
        Next
        Return sURL
    End Function
    Function GetMovieStudio(ByRef DBMovie As Structures.DBMovie) As List(Of String)
        Dim ret As Interfaces.ModuleResult
        Dim sStudio As New List(Of String)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.ProcessorModule.IsPostScraper AndAlso e.ProcessorModule.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externalScraperModule.ProcessorModule.GetMovieStudio(DBMovie, sStudio)
            If ret.breakChain Then Exit For
        Next
        Return sStudio
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Event TVScraperEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
    Public Sub Handler_TVScraperEvent(ByVal eType As Enums.TVScraperEventType, ByVal iProgress As Integer, ByVal Parameter As Object)
        RaiseEvent TVScraperEvent(eType, iProgress, Parameter)
    End Sub

    Public Sub SetTVScraperEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ProcessorModule.ScraperEnabled = value
        Next
    End Sub
    Public Sub SetTVPostScraperEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ProcessorModule.PostScraperEnabled = value
        Next
    End Sub
    Public Sub SetTVScraperOrder(ByVal ModuleAssembly As String, ByVal value As Integer)
        For Each _externalScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ScraperOrder = value
        Next
    End Sub
    Public Sub SetTVPostScraperOrder(ByVal ModuleAssembly As String, ByVal value As Integer)
        For Each _externalScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.PostScraperOrder = value
        Next
    End Sub
    Function ChangeEpisode(ByVal ShowID As Integer, ByVal TVDBID As String) As MediaContainers.EpisodeDetails
        Dim ret As Interfaces.ModuleResult
        Dim epDetails As New MediaContainers.EpisodeDetails
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsPostScraper AndAlso e.ProcessorModule.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externaltvScraperModule.ProcessorModule.ChangeEpisode(ShowID, TVDBID, epDetails)
            If ret.breakChain Then Exit For
        Next
        Return epDetails
    End Function
    Sub TVSaveImages()
        Dim ret As Interfaces.ModuleResult
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsPostScraper AndAlso e.ProcessorModule.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externaltvScraperModule.ProcessorModule.SaveImages()
            If ret.breakChain Then Exit For
        Next
    End Sub
    Public Function TVGetLangs(ByVal sMirror As String) As List(Of Containers.TVLanguage)
        Dim ret As Interfaces.ModuleResult
        Dim Langs As New List(Of Containers.TVLanguage)
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsPostScraper AndAlso e.ProcessorModule.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externaltvScraperModule.ProcessorModule.GetLangs(sMirror, Langs)
            If ret.breakChain Then Exit For
        Next
        Return Langs
    End Function

    Public Function TVScrapeOnly(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions, ByVal WithCurrent As Boolean) As Boolean
        Dim ret As Interfaces.ModuleResult
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
            ret = _externaltvScraperModule.ProcessorModule.Scraper(ShowID, ShowTitle, TVDBID, Lang, Options, WithCurrent)
            If ret.breakChain Then Exit For
        Next
        Return ret.Cancelled
    End Function

    Public Function TVScrapeEpisode(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iEpisode As Integer, ByVal iSeason As Integer, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions) As Boolean
        Dim ret As Interfaces.ModuleResult
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
            ret = _externaltvScraperModule.ProcessorModule.ScrapeEpisode(ShowID, ShowTitle, TVDBID, iEpisode, iSeason, Lang, Options)
            If ret.breakChain Then Exit For
        Next
        Return ret.Cancelled
    End Function

    Public Function GetSingleEpisode(ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Season As Integer, ByVal Episode As Integer, ByVal Options As Structures.TVScrapeOptions) As MediaContainers.EpisodeDetails
        Dim epDetails As New MediaContainers.EpisodeDetails
        Dim ret As Interfaces.ModuleResult
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
            ret = _externaltvScraperModule.ProcessorModule.GetSingleEpisode(ShowID, TVDBID, Season, Episode, Options, epDetails)
            If ret.breakChain Then Exit For
        Next
        Return epDetails
    End Function

    Public Function TVScrapeSeason(ByVal ShowID As Integer, ByVal ShowTitle As String, ByVal TVDBID As String, ByVal iSeason As Integer, ByVal Lang As String, ByVal Options As Structures.TVScrapeOptions) As Boolean
        Dim ret As Interfaces.ModuleResult
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
            ret = _externaltvScraperModule.ProcessorModule.ScrapeSeason(ShowID, ShowTitle, TVDBID, iSeason, Lang, Options)
            If ret.breakChain Then Exit For
        Next
        Return ret.Cancelled
    End Function

    Public Function TVSingleImageOnly(ByVal Title As String, ByVal ShowID As Integer, ByVal TVDBID As String, ByVal Type As Enums.TVImageType, ByVal Season As Integer, ByVal Episode As Integer, ByVal Lang As String, ByVal CurrentImage As Image) As Image
        Dim Image As Image = Nothing
        Dim ret As Interfaces.ModuleResult
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
            ret = _externaltvScraperModule.ProcessorModule.GetSingleImage(Title, ShowID, TVDBID, Type, Season, Episode, Lang, CurrentImage, Image)
            If ret.breakChain Then Exit For
        Next
        Return Image
    End Function

    Public ReadOnly Property TVIsBusy() As Boolean
        Get
            Dim ret As Boolean = False
            For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
                ret = ret OrElse _externaltvScraperModule.ProcessorModule.IsBusy
            Next
            Return ret
        End Get
    End Property

    Public Sub TVCancelAsync()
        For Each _externaltvScraperModule As _externalTVScraperModuleClass In externalTVScrapersModules.Where(Function(e) e.ProcessorModule.IsScraper AndAlso e.ProcessorModule.ScraperEnabled)
            _externaltvScraperModule.ProcessorModule.CancelAsync()
        Next
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub SaveSettings()
        Dim tmpForXML As New List(Of _XMLEmberModuleClass)

        For Each _externalProcessorModule As _externalGenericModuleClass In externalProcessorModules
            Dim t As New _XMLEmberModuleClass
            t.AssemblyName = _externalProcessorModule.AssemblyName
            t.AssemblyFileName = _externalProcessorModule.AssemblyFileName
            t.Enabled = _externalProcessorModule.ProcessorModule.Enabled
            tmpForXML.Add(t)
        Next
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules
            Dim t As New _XMLEmberModuleClass
            t.AssemblyName = _externalScraperModule.AssemblyName
            t.AssemblyFileName = _externalScraperModule.AssemblyFileName
            t.PostScraperEnabled = _externalScraperModule.ProcessorModule.PostScraperEnabled
            t.ScraperEnabled = _externalScraperModule.ProcessorModule.ScraperEnabled
            t.PostScraperOrder = _externalScraperModule.PostScraperOrder
            t.ScraperOrder = _externalScraperModule.ScraperOrder
            tmpForXML.Add(t)
        Next
        Master.eSettings.EmberModules = tmpForXML
        Master.eSettings.Save()
    End Sub
End Class
