Namespace EBML
    Public MustInherit Class EbmlValueElement
        Inherits EbmlElement
        Public Sub New(ByVal id As Long)
            MyBase.New(id)
        End Sub

        Public Overrides ReadOnly Property IsContainer() As Boolean
            Get
                Return False
            End Get
        End Property

        Public MustOverride Sub UpdateValue(ByVal data As Byte(), ByVal offset As Integer, ByVal size As Integer)
        Public MustOverride Sub UpdateValue(ByVal o As Object)
        Public MustOverride Function GetValue() As Object
    End Class
End Namespace
