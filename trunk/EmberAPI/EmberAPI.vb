Public Class Containers
    Public Class TVLanguage
        Private _longlang As String
        Private _shortlang As String

        Public Property LongLang() As String
            Get
                Return Me._longlang
            End Get
            Set(ByVal value As String)
                Me._longlang = value
            End Set
        End Property

        Public Property ShortLang() As String
            Get
                Return Me._shortlang
            End Get
            Set(ByVal value As String)
                Me._shortlang = value
            End Set
        End Property

        Public Sub New()
            Me.Clear()
        End Sub

        Public Sub Clear()
            Me._longlang = String.Empty
            Me._shortlang = String.Empty
        End Sub
    End Class
End Class
