Namespace MenuActionAttribute
    Public Class MenuActionAttribute
        Inherits Interprise.Presentation.Base.MenuActionAttribute
        Implements Interprise.Extendable.Base.Presentation.Generic.IMenuActionAttributeInterface

#Region " Methods "
#Region " Constructor "
        Public Sub New(ByVal action As String)
            MyBase.New(action)
        End Sub
#End Region
#End Region

    End Class
End Namespace