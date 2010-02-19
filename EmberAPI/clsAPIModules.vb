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
' TODO: Need to background work some of the functions
' TODO: Need to change names of some of the buttons
'
'

'Option Strict Off
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class ModulesManager
    'Singleton Instace for module manager .. allways use this one
    Private Shared Singleton As ModulesManager = Nothing
    Public Shared ReadOnly Property Instance() As ModulesManager
        Get
            If (Singleton Is Nothing) Then
                Singleton = New ModulesManager()
            End If
            Return Singleton
        End Get
    End Property

    Class EmberRuntimeObjects

        'all runtime object (not classes or shared methods) that need to be exposed to Modules
        Private _TopMenu As System.Windows.Forms.MenuStrip
        Private _MenuMediaList As System.Windows.Forms.ContextMenuStrip
        Private _MediaList As System.Windows.Forms.DataGridView
        Private _MainTool As System.Windows.Forms.ToolStrip
        Sub New()
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
    End Class
    <XmlRoot("EmberModule")> _
    Class _XMLEmberModuleClass
        Public Enabled As Boolean
        Public ScraperEnabled As Boolean
        Public PostScraperEnabled As Boolean
        Public AssemblyName As String
        Public ScraperOrder As Integer
        Public PostScraperOrder As Integer
    End Class
    Class _externalProcessorModuleClass
        Public ProcessorModule As Interfaces.EmberExternalModule 'Object
        Public Enabled As Boolean
        Public AssemblyName As String
    End Class
    Class _externalScraperModuleClass
        Public ProcessorModule As Interfaces.EmberScraperModule 'Object
        Public ScraperEnabled As Boolean
        Public PostScraperEnabled As Boolean
        Public AssemblyName As String
        Public IsScraper As Boolean
        Public IsPostScraper As Boolean
        Public ScraperOrder As Integer
        Public PostScraperOrder As Integer
    End Class

    Public RuntimeObjects As New EmberRuntimeObjects
    Public externalProcessorModules As New List(Of _externalProcessorModuleClass)
    Public externalScrapersModules As New List(Of _externalScraperModuleClass)
    Private moduleLocation As String = Path.Combine(Functions.AppPath, "Modules")

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
                                Dim _externalProcessorModule As New _externalProcessorModuleClass
                                _externalProcessorModule.ProcessorModule = ProcessorModule
                                _externalProcessorModule.AssemblyName = String.Concat(Path.GetFileName(file), ".", fileType.FullName)
                                For Each i In Master.eSettings.EmberModules
                                    If i.AssemblyName = _externalProcessorModule.AssemblyName Then
                                        _externalProcessorModule.Enabled = i.Enabled
                                    End If
                                Next
                                externalProcessorModules.Add(_externalProcessorModule)
                                ProcessorModule.Init(RuntimeObjects)
                                If _externalProcessorModule.Enabled Then
                                    ProcessorModule.Enable()
                                Else
                                    ProcessorModule.Disable()
                                End If
                            End If
                        Catch ex As Exception
                        End Try
                    Next
                Catch ex As Exception
                End Try
            Next
        End If
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
            For Each file As String In System.IO.Directory.GetFiles(moduleLocation, "*.dll")
                assembly = System.Reflection.Assembly.LoadFile(file)
                'Loop through each of the assemeblies type
                For Each fileType As Type In assembly.GetTypes
                    Try
                        'Activate the located module
                        Dim t As Type = fileType.GetInterface("EmberScraperModule")
                        If Not t Is Nothing Then
                            Dim ProcessorModule As Interfaces.EmberScraperModule
                            ProcessorModule = CType(Activator.CreateInstance(fileType), Interfaces.EmberScraperModule)
                            'Add the activated module to the arraylist
                            Dim _externalScraperModule As New _externalScraperModuleClass
                            _externalScraperModule.ProcessorModule = ProcessorModule
                            _externalScraperModule.AssemblyName = String.Concat(Path.GetFileName(file), ".", fileType.FullName)
                            _externalScraperModule.IsScraper = ProcessorModule.IsScraper
                            _externalScraperModule.IsPostScraper = ProcessorModule.IsPostScraper
                            Dim found As Boolean = False
                            For Each i As _XMLEmberModuleClass In Master.eSettings.EmberModules.Where(Function(x) x.AssemblyName = _externalScraperModule.AssemblyName)
                                _externalScraperModule.ScraperEnabled = i.ScraperEnabled
                                ScraperAnyEnabled = ScraperAnyEnabled Or i.ScraperEnabled
                                _externalScraperModule.PostScraperEnabled = i.PostScraperEnabled
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
                        End If
                    Catch ex As Exception
                    End Try
                Next
            Next
            Dim c As Integer = 0
            For Each ext As _externalScraperModuleClass In externalScrapersModules.Where(Function(x) x.ScraperEnabled)
                ext.ScraperOrder = c
                c += 1
            Next
            For Each ext As _externalScraperModuleClass In externalScrapersModules.Where(Function(x) x.PostScraperEnabled)
                ext.PostScraperOrder = c
                c += 1
            Next
            If Not ScraperAnyEnabled Then
                SetScraperEnable("scraper.EmberCore.dll.EmberScraperModule.EmberNativeScraperModule", True)
                SetScraperOrder("scraper.EmberCore.dll.EmberScraperModule.EmberNativeScraperModule", 1)
            End If
            If Not PostScraperAnyEnabled Then
                SetPostScraperEnable("scraper.EmberCore.dll.EmberScraperModule.EmberNativeScraperModule", True)
                SetPostScraperOrder("scraper.EmberCore.dll.EmberScraperModule.EmberNativeScraperModule", 1)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Entry point to Scrape and Post Scrape .. will run all modules enabled
    ''' </summary>
    ''' <param name="movie">MediaContainers.Movie Object with Title or Id fieldIn</param>
    ''' <param Options="movie">ScrapeOptions Structure defining user scrape options</param>
    ''' <returns>boolean success</returns>
    Public Function FullScrape(ByRef DBMovie As Structures.DBMovie, ByVal Options As Structures.ScrapeOptions) As Boolean
        'AndAlso? Only return true if both complete successfully?

        Return ScrapeOnly(DBMovie, Options) 'OrElse PostScrapeOnly(movie)
    End Function

    Public Function ScrapeOnly(ByRef DBMovie As Structures.DBMovie, ByVal Options As Structures.ScrapeOptions) As Boolean
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.IsScraper AndAlso e.ScraperEnabled)
            If Not _externalScraperModule.ProcessorModule.Scraper(DBMovie, Options) Then Return False
        Next
        Return True
    End Function
    Public Function PostScrapeOnly(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal ScrapeType As EmberAPI.Enums.ScrapeType) As Boolean
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.IsPostScraper AndAlso e.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            AddHandler _externalScraperModule.ProcessorModule.ScraperUpdateMediaList, AddressOf Handler_ScraperUpdateMediaList
            Dim ret As Boolean = _externalScraperModule.ProcessorModule.PostScraper(DBMovie, ScrapeType)
            RemoveHandler _externalScraperModule.ProcessorModule.ScraperUpdateMediaList, AddressOf Handler_ScraperUpdateMediaList
            If Not ret Then Exit For
        Next
        Return True
    End Function
    Event ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean)

    Public Sub Handler_ScraperUpdateMediaList(ByVal col As Integer, ByVal v As Boolean)
        RaiseEvent ScraperUpdateMediaList(col, v)
    End Sub

    Sub New()
    End Sub

    Public Sub LoadAllModules()
        loadModules()
        loadScrapersModules()
    End Sub

    Public Sub Setup()
        Dim modulesSetup As New dlgModuleSettings
        For Each _externalProcessorModule As _externalProcessorModuleClass In externalProcessorModules
            Dim li As ListViewItem = modulesSetup.lstModules.Items.Add(_externalProcessorModule.ProcessorModule.ModuleName())
            li.SubItems.Add(If(_externalProcessorModule.Enabled, "Enabled", "Disabled"))
            li.Tag = _externalProcessorModule.AssemblyName
        Next
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.OrderBy(Function(x) x.ScraperOrder)
            Dim liS As New ListViewItem
            If _externalScraperModule.IsScraper Then
                liS = modulesSetup.lstScrapers.Items.Add(_externalScraperModule.ProcessorModule.ModuleName())
                liS.SubItems.Add(If(_externalScraperModule.ScraperEnabled, "Enabled", "Disabled"))
                liS.Tag = _externalScraperModule.AssemblyName
            End If
        Next
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.OrderBy(Function(x) x.PostScraperOrder)
            Dim liPS As New ListViewItem
            If _externalScraperModule.IsPostScraper Then
                liPS = modulesSetup.lstPostScrapers.Items.Add(_externalScraperModule.ProcessorModule.ModuleName())
                liPS.SubItems.Add(If(_externalScraperModule.PostScraperEnabled, "Enabled", "Disabled"))
                liPS.Tag = _externalScraperModule.AssemblyName
            End If
        Next
        modulesSetup.ModulesManager = Me
        modulesSetup.ShowDialog()
    End Sub

    Public Sub GetVersions()
        Dim dlgVersions As New dlgVersions
        Dim li As ListViewItem
        li = dlgVersions.lstVersions.Items.Add("Ember Application")
        li.SubItems.Add(My.Application.Info.Version.Revision.ToString)
        li = dlgVersions.lstVersions.Items.Add("Ember API")
        li.SubItems.Add(EmberAPI.Functions.EmberAPIVersion())
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules
            li = dlgVersions.lstVersions.Items.Add(_externalScraperModule.ProcessorModule.ModuleName)
            li.SubItems.Add(_externalScraperModule.ProcessorModule.ModuleVersion)
        Next
        For Each _externalModule As _externalProcessorModuleClass In externalProcessorModules
            li = dlgVersions.lstVersions.Items.Add(_externalModule.ProcessorModule.ModuleName)
            li.SubItems.Add(_externalModule.ProcessorModule.ModuleVersion)
        Next
        dlgVersions.ShowDialog()
    End Sub

    Public Sub RunModuleSetup(ByVal ModuleAssembly As String)
        For Each _externalProcessorModule As _externalProcessorModuleClass In externalProcessorModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalProcessorModule.ProcessorModule.Setup()
        Next
    End Sub
    Public Sub RunScraperSetup(ByVal ModuleAssembly As String)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ProcessorModule.SetupScraper()
        Next
    End Sub
    Public Sub RunPostScraperSetup(ByVal ModuleAssembly As String)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ProcessorModule.SetupPostScraper()
        Next
    End Sub

    Public Sub SetModuleEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalProcessorModule As _externalProcessorModuleClass In externalProcessorModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalProcessorModule.Enabled = value
            If value = True Then
                _externalProcessorModule.ProcessorModule.Enable()
            Else
                _externalProcessorModule.ProcessorModule.Disable()
            End If
        Next
    End Sub
    Public Sub SetScraperEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.ScraperEnabled = value
        Next
    End Sub
    Public Sub SetPostScraperEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalScraperModule.PostScraperEnabled = value
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
    Function ScraperSelectImageOfType(ByRef DBMovie As EmberAPI.Structures.DBMovie, ByVal _DLType As EmberAPI.Enums.ImageType, ByRef pResults As Containers.ImgResult, Optional ByVal _isEdit As Boolean = False) As Boolean
        Dim ret As Boolean
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.IsPostScraper AndAlso e.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externalScraperModule.ProcessorModule.SelectImageOfType(DBMovie, _DLType, pResults, _isEdit)
            If ret Then Exit For
        Next
        Return ret
    End Function

    Function ScraperDownlaodTrailer(ByRef DBMovie As EmberAPI.Structures.DBMovie) As String
        Dim ret As Boolean
        Dim sURL As String = String.Empty
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.IsPostScraper AndAlso e.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externalScraperModule.ProcessorModule.DownloadTrailer(DBMovie, sURL)
            If ret Then Exit For
        Next
        Return sURL
    End Function

    Function GetMovieStudio(ByRef DBMovie As EmberAPI.Structures.DBMovie) As List(Of String)
        Dim ret As Boolean
        Dim sStudio As New List(Of String)
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.IsPostScraper AndAlso e.PostScraperEnabled).OrderBy(Function(e) e.PostScraperOrder)
            ret = _externalScraperModule.ProcessorModule.GetMovieStudio(DBMovie, sStudio)
            If ret Then Exit For
        Next
        Return sStudio
    End Function


    Public Sub SaveSettings()
        Dim tmpForXML As New List(Of _XMLEmberModuleClass)

        For Each _externalProcessorModule As _externalProcessorModuleClass In externalProcessorModules
            Dim t As New _XMLEmberModuleClass
            t.AssemblyName = _externalProcessorModule.AssemblyName
            t.Enabled = _externalProcessorModule.Enabled
            tmpForXML.Add(t)
        Next
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules
            Dim t As New _XMLEmberModuleClass
            t.AssemblyName = _externalScraperModule.AssemblyName
            t.PostScraperEnabled = _externalScraperModule.PostScraperEnabled
            t.ScraperEnabled = _externalScraperModule.ScraperEnabled
            t.PostScraperOrder = _externalScraperModule.PostScraperOrder
            t.ScraperOrder = _externalScraperModule.ScraperOrder
            tmpForXML.Add(t)
        Next
        Master.eSettings.EmberModules = tmpForXML
        Master.eSettings.Save()
    End Sub
End Class
