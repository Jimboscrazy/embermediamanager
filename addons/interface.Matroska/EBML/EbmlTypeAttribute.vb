Namespace EBML.Serialization
    <AttributeUsage(AttributeTargets.[Class] Or AttributeTargets.Struct, AllowMultiple:=True, Inherited:=False)> _
    Public Class EbmlTypeAttribute
        Inherits Attribute
        Private _type As Type

        Public Sub New(ByVal type As Type)
            _type = type
        End Sub

        Friend ReadOnly Property Type() As Type
            Get
                Return _type
            End Get
        End Property
    End Class
End Namespace
