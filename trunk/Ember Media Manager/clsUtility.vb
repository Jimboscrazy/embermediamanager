Imports System.Text.RegularExpressions

Public Class Utility



    Public Class Wildcard

        Public Shared Function IsMatch(ByVal ExpressionToMatch As String, ByVal FilterExpression As String, Optional ByVal IgnoreCase As Boolean = True) As Boolean
            'TODO: include the [charlist] and [!charlist] functionality?
            If FilterExpression.Contains("*") _
                OrElse FilterExpression.Contains("?") _
                OrElse FilterExpression.Contains("#") Then

                If IgnoreCase Then
                    Return (ExpressionToMatch.ToLower Like FilterExpression.ToLower)
                Else
                    Return (ExpressionToMatch Like FilterExpression)
                End If

            Else
                If IgnoreCase Then
                    Return ExpressionToMatch.ToLower.Contains(FilterExpression.ToLower)
                Else
                    Return ExpressionToMatch.Contains(FilterExpression)
                End If
            End If
        End Function

    End Class


End Class
