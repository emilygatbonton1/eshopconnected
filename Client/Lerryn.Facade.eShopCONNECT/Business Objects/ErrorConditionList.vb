Public Class ErrorConditionList

    Public Property ErrorMessage As String
    Public Property ErrorDate As Date

    Public Sub New()

    End Sub

    Public Sub New(_errorMessage As String, _errorDate As Date)
        ErrorMessage = _errorMessage
        ErrorDate = _errorDate
    End Sub

End Class
