Public Class clsXLS


End Class
<Serializable()> _
Public Class UpgradeList
    Private _VersionList As List(Of Versions)

    Public Property VersionList() As List(Of Versions)
        Get
            Return Me._VersionList
        End Get
        Set(ByVal value As List(Of Versions))
            Me._VersionList = value
        End Set
    End Property
End Class

<Serializable()> _
Public Class Versions
    Private _Version As String
    Private _VersionNumber As String

    Public Property Version() As String
        Get
            Return Me._Version
        End Get
        Set(ByVal value As String)
            Me._Version = value
        End Set
    End Property
    Public Property VersionNumber() As String
        Get
            Return Me._VersionNumber
        End Get
        Set(ByVal value As String)
            Me._VersionNumber = value
        End Set
    End Property
End Class