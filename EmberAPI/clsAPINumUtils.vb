Imports System.Globalization

Public Class NumUtils
    ''' <summary>
    ''' Convert a numerical string to single (internationally friendly method)
    ''' </summary>
    ''' <param name="sNumber">Number (as string) to convert</param>
    ''' <returns>Number as single</returns>
    Public Shared Function ConvertToSingle(ByVal sNumber As String) As Single
        Try
            If String.IsNullOrEmpty(sNumber) OrElse sNumber = "0" Then Return 0
            Dim numFormat As NumberFormatInfo = New NumberFormatInfo()
            numFormat.NumberDecimalSeparator = "."
            Return Single.Parse(sNumber.Replace(",", "."), NumberStyles.AllowDecimalPoint, numFormat)
        Catch
        End Try
        Return 0
    End Function
End Class
