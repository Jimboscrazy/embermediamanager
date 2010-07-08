Imports EBML

Namespace EBML.Serialization
    Public Class EbmlFieldSemanticAttribute
        Inherits Attribute
        Private _id As Long
        Private _elementType As Type

        Public Sub New(ByVal id As Long)
            _id = id
            _elementType = Nothing
        End Sub

        Public Sub New(ByVal id As Long, ByVal elementType As Type)
            If Not elementType.IsSubclassOf(GetType(EbmlElement)) Then
                Throw New ArgumentException()
            End If
            _id = id
            _elementType = elementType
        End Sub

        Public ReadOnly Property ID() As Long
            Get
                Return _id
            End Get
        End Property

        Public Property ElementType() As Type
            Get
                Return _elementType
            End Get
            Friend Set(ByVal value As Type)
                _elementType = Value
            End Set
        End Property
    End Class
End Namespace
