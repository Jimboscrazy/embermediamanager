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
    Class EmberRuntimeObjects
        'all runtime object (not classes or shared methods) that need to be exposed to Modules
        Private _MenuMediaList As System.Windows.Forms.ContextMenuStrip
        Private _MediaList As System.Windows.Forms.DataGridView
        Sub New()
        End Sub
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
        Public Enabled As Boolean
        Public AssemblyName As String
        Public IsScraper As Boolean
        Public IsPostScraper As Boolean
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
                            _externalProcessorModule.AssemblyName = Path.GetFileName(file)
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
            Next
        End If
    End Sub

    ''' <summary>
    ''' Load all Scraper Modules and field in externalScrapersModules List
    ''' </summary>

    Public Sub loadScrapersModules()
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
                            _externalScraperModule.AssemblyName = Path.GetFileName(file)
                            _externalScraperModule.IsScraper = ProcessorModule.IsScraper
                            _externalScraperModule.IsPostScraper = ProcessorModule.IsPostScraper
                            For Each i As _XMLEmberModuleClass In Master.eSettings.EmberModules.Where(Function(x) x.AssemblyName = _externalScraperModule.AssemblyName)
                                _externalScraperModule.Enabled = i.Enabled
                            Next
                            externalScrapersModules.Add(_externalScraperModule)
                        End If
                    Catch ex As Exception
                    End Try
                Next
            Next
        End If
    End Sub
    'TODO : Bellow functions should go to Background worker ?

    ''' <summary>
    ''' Entry point to Scrape and Post Scrape .. will run all modules enabled
    ''' </summary>
    ''' <param name="movie">MediaContainers.Movie Object with Title or Id fieldIn</param>
    ''' <param Options="movie">ScrapeOptions Structure defining user scrape options</param>
    ''' <returns>boolean success</returns>
    Public Function FullScrape(ByRef movie As MediaContainers.Movie, ByVal Options As Structures.ScrapeOptions) As Boolean
        'AndAlso? Only return true if both complete successfully?

        Return ScrapeOnly(movie, Options) OrElse PostScrapeOnly(movie)
    End Function

    Public Function ScrapeOnly(ByRef movie As MediaContainers.Movie, ByVal Options As Structures.ScrapeOptions) As Boolean
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.IsScraper AndAlso e.Enabled)
            If Not _externalScraperModule.ProcessorModule.Scraper(movie, Options) Then Exit For
        Next
        Return True
    End Function
    Public Function PostScrapeOnly(ByRef movie As MediaContainers.Movie) As Boolean
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules.Where(Function(e) e.IsPostScraper AndAlso e.Enabled)
            If Not _externalScraperModule.ProcessorModule.PostScraper(movie) Then Exit For
        Next
        Return True
    End Function

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
            If _externalProcessorModule.Enabled Then
                li.SubItems.Add("Enabled")
            Else
                li.SubItems.Add("Disabled")
            End If
            li.Tag = _externalProcessorModule.AssemblyName
        Next
        For Each _externalScraperModule As _externalScraperModuleClass In externalScrapersModules
            Dim li As New ListViewItem
            If _externalScraperModule.IsScraper Then
                li = modulesSetup.lstScrapers.Items.Add(_externalScraperModule.ProcessorModule.ModuleName())
            End If
            If _externalScraperModule.IsPostScraper Then
                li = modulesSetup.lstPostScrapers.Items.Add(_externalScraperModule.ProcessorModule.ModuleName())
            End If
            If _externalScraperModule.IsScraper OrElse _externalScraperModule.IsPostScraper Then
                If _externalScraperModule.Enabled Then
                    li.SubItems.Add("Enabled")
                Else
                    li.SubItems.Add("Disabled")
                End If
                li.Tag = _externalScraperModule.AssemblyName
            End If
        Next
        modulesSetup.ModulesManager = Me
        modulesSetup.ShowDialog()
    End Sub
    Public Sub RunModuleSetup(ByVal ModuleAssembly As String)
        For Each _externalProcessorModule As _externalProcessorModuleClass In externalProcessorModules.Where(Function(p) p.AssemblyName = ModuleAssembly)
            _externalProcessorModule.ProcessorModule.Setup()
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
            t.Enabled = _externalScraperModule.Enabled
            tmpForXML.Add(t)
        Next
        Master.eSettings.EmberModules = tmpForXML
        Master.eSettings.Save()
    End Sub
End Class
