Namespace EBML
    Partial Public Class EbmlDocument
        Public Function GetValueUniqueElement(ByVal s As String) As Object
            Dim e As EbmlElement = GetUniqueElement(s)
            Dim o As EbmlValueElement = Nothing
            If e IsNot Nothing Then o = TryCast(e, EbmlValueElement)
            Return If(Not IsNothing(o) AndAlso Not IsNothing(o.GetValue), o.GetValue, Nothing)
        End Function

        Public Function GetUniqueElement(ByVal s As String, Optional ByVal parentElement As EbmlContainerElement = Nothing) As EbmlElement
            Dim l_doc As List(Of EbmlElement)
            If parentElement Is Nothing Then
                l_doc = Me.RootElements
            Else
                l_doc = parentElement.Children
            End If
            For Each element As EbmlElement In l_doc
                Dim container As EbmlContainerElement = TryCast(element, EbmlContainerElement)
                Dim name As String = _semantic.GetElementName(element.ID)
                If name = s Then Return element
                If container IsNot Nothing Then
                    Dim e As EbmlElement = GetUniqueElement(s, container)
                    If e IsNot Nothing Then Return e
                End If
            Next
            Return Nothing
        End Function
    End Class

End Namespace
