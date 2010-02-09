Public Interface EmberExternalModule
    Sub Enable()
    Sub Disable()
    Sub Setup()
    Sub Init(ByRef emm As Object)
    ReadOnly Property ModuleName() As String
    ReadOnly Property ModuleVersion() As String
End Interface

Public Class TestEmberExternalModule
    Implements EmberExternalModule
    Dim emmAPI As New Object
    Private Enabled As Boolean = False
    Private _Name As String = "Teste Module"
    Private _Version As String = "1.0"
    Sub Setup() Implements EmberExternalModule.Setup
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub
    Sub Enable() Implements EmberExternalModule.Enable
        If Not Enabled Then
            MyMenu.Text = "Master Library"
            MySubMenu.Text = "Move To"
            MyMenu.DropDownItems.Add(MySubMenu)
            emmAPI.MenuMediaList.Items.Add(MyMenu)
            Enabled = True
        End If
    End Sub
    Sub Disable() Implements EmberExternalModule.Disable
        If Enabled Then
            emmAPI.MenuMediaList.Items.Remove(MyMenu)
            Enabled = False
        End If
    End Sub
    Sub Init(ByRef emm As Object) Implements EmberExternalModule.Init
        emmAPI = emm
    End Sub
    ReadOnly Property ModuleName() As String Implements EmberExternalModule.ModuleName
        Get
            Return _Name
        End Get
    End Property
    ReadOnly Property ModuleVersion() As String Implements EmberExternalModule.ModuleVersion
        Get
            Return _Version
        End Get
    End Property
    Dim MyMenu As New System.Windows.Forms.ToolStripMenuItem
    Dim WithEvents MySubMenu As New System.Windows.Forms.ToolStripMenuItem
    Private Sub TestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MySubMenu.Click
        Dim _setup As New frmSetup
        _setup.ShowDialog()
    End Sub

End Class
