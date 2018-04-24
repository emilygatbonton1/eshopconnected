' eShopCONNECT for Connected Business
' Module: ConfigSettingsForm.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Connected Business SDK and may incorporate certain intellectual 
' property of Interprise Solutions Inc. who's
' rights are hereby recognised.
'

'-------------------------------------------------------------------
'
' Updated 05 Jult 2012

Imports Interprise.Presentation.Base.Const
Imports Microsoft.VisualBasic ' TJS 02/12/11

Namespace SystemManager.Config
    <MenuActionAttribute.MenuAction("OpenImpExpConfigSettingsForm"), _
        Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.Add, _
        "Lerryn.Presentation.ImportExport.SystemManager.ConfigSettingsForm")> _
    Public Class ConfigSettingsForm
        Inherits Interprise.Presentation.SystemManager.Country.CountryForm
        Implements Interprise.Extendable.SystemManager.Presentation.Country.ICountryFormInterface

#Region " Properties "
        Public ReadOnly Property IsActivated() As Boolean
            Get
                If Me.PanelBodyPluginInstance IsNot Nothing Then
                    Return DirectCast(Me.PanelBodyPluginInstance, ConfigSettingsListSection).IsActivated
                Else
                    Return False
                End If
            End Get
        End Property
#End Region

#Region " Methods "
#Region " Constructor "
        Public Sub New()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -    This module 
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.       | Description
            '------------------------------------------------------------------------------------------
            ' 02/12/11 | TJS             | 2011.2.0.0  | Modified for IS v6 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            MyBase.New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me.Text = PRODUCT_NAME & " Configuration Settings"
            Me.MenuItemDelete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never ' TJS 02/12/11
            Me.MenuItemNew.Visibility = DevExpress.XtraBars.BarItemVisibility.Never ' TJS 02/12/11
            Me.MenuItemRefresh.Visibility = DevExpress.XtraBars.BarItemVisibility.Never ' TJS 02/12/11
            Me.MenuItemPrint.Visibility = DevExpress.XtraBars.BarItemVisibility.Never ' TJS 02/12/11

        End Sub
#End Region

#Region " DisplayActivationStatus "
        Public Sub DisplayActivationStatus()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -   
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 21/03/11 | TJs             | 2011.0.02 | Function added
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Me.PanelBodyPluginInstance IsNot Nothing Then
                DirectCast(Me.PanelBodyPluginInstance, ConfigSettingsListSection).DisplayActivationStatus()
            End If
        End Sub

#End Region
#End Region

#Region " Events "
#Region " ConfigSettingsForm_Resize "
        Private Sub ConfigSettingsForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -    This function makes sure that the Plugins website label remains 
            '                    positioned correctly when the form is resized 
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 24/01/09 | TJS             | 2009.1.01 | Function added
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Me.LabelPluginsForInterprise.Top = Me.Height - 52

        End Sub
#End Region

#Region " Events "
        Private Sub CheckServiceRunning(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -    This function checks to see if the eShopCONNECT service appears to be installed and working  
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 02/12/11 | TJS             | 2011.2.00 | Function added
            ' 05/07/12 | TJS             | 2012.1.08 | Added wait cursor
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Cursor = Cursors.WaitCursor ' TJS 05/07/12
            Application.DoEvents() ' TJS 05/07/12
            If Me.PanelBodyPluginInstance IsNot Nothing AndAlso DirectCast(Me.PanelBodyPluginInstance, ConfigSettingsListSection).IsActivated Then
                DirectCast(Me.PanelBodyPluginInstance, ConfigSettingsListSection).CheckServicesOperational()
            End If
            Cursor = Cursors.Default ' TJS 05/07/12

        End Sub
#End Region
#End Region
    End Class
End Namespace
