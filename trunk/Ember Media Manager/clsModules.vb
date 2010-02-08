
Option Strict Off
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Interface EmberExternalModule
    Sub Enable()
    Sub Disable()
    Sub Setup()
    Sub Init(ByRef emm As Object)
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
End Interface

Public Class EmberModules
    <XmlRoot("EmberModule")> _
    Class _XMLEmberModuleClass
        Public Enabled As Boolean
        Public AssemblyName As String
    End Class
    Class _externalProcessorModuleClass
        Public ProcessorModule As Object
        Public Enabled As Boolean
        Public AssemblyName As String
    End Class
    Class _EmberAPI
        Private _MenuMediaList As System.Windows.Forms.ContextMenuStrip
        Public EmberAPP As Object = frmMain
        Sub New()
            _MenuMediaList = frmMain.mnuMediaList
        End Sub
        Public ReadOnly Property MenuMediaList() As System.Windows.Forms.ContextMenuStrip
            Get
                Return _MenuMediaList
            End Get
        End Property
    End Class

    Public EmberAPI As New _EmberAPI
    Public externalProcessorModules As New List(Of _externalProcessorModuleClass)
    Private moduleLocation As String = Path.Combine(Application.StartupPath, "Modules")
    Public Sub loadModules()
        'Assembly to load the file
        Dim assembly As System.Reflection.Assembly
        'For each .dll file in the module directory
        For Each file As String In System.IO.Directory.GetFiles(moduleLocation, "*.dll")
            'Load the assembly
            assembly = System.Reflection.Assembly.LoadFrom(file)
            'Loop through each of the assemeblies type
            For Each fileType As Type In assembly.GetTypes
                Try
                    'Activate the located module
                    Dim t As Type = fileType.GetInterface("EmberExternalModule")
                    If Not t Is Nothing Then
                        Dim ProcessorModule As New Object
                        ProcessorModule = Activator.CreateInstance(fileType)
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
                        ProcessorModule.Init(EmberAPI)
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
    End Sub

    Dim WithEvents ModulesMenu As New System.Windows.Forms.ToolStripMenuItem
    Sub New()
        ModulesMenu.Text = "Modules Settings"
        frmMain.EditToolStripMenuItem.DropDownItems.Add(ModulesMenu)
        loadModules()
    End Sub
    Private Sub ModulesMenutem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModulesMenu.Click
        Dim modulesSetup As New dlgModuleSettings
        For Each _externalProcessorModule In externalProcessorModules
            Dim li As ListViewItem = modulesSetup.lstModules.Items.Add(_externalProcessorModule.ProcessorModule.ModuleName())
            li.SubItems.Add("Disabled")
            li.Tag = _externalProcessorModule.AssemblyName
        Next
        modulesSetup.ModulesManager = Me
        modulesSetup.ShowDialog()
    End Sub
    Public Sub RunModuleSetup(ByVal ModuleAssembly As String)
        For Each _externalProcessorModule In externalProcessorModules
            If _externalProcessorModule.AssemblyName = ModuleAssembly Then
                _externalProcessorModule.ProcessorModule.Setup()
            End If
        Next
    End Sub
    Public Sub SetModuleEnable(ByVal ModuleAssembly As String, ByVal value As Boolean)
        For Each _externalProcessorModule In externalProcessorModules
            If _externalProcessorModule.AssemblyName = ModuleAssembly Then
                _externalProcessorModule.Enabled = value
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
        Master.eSettings.EmberModules = tmpForXML
        Master.eSettings.Save()
    End Sub
End Class
