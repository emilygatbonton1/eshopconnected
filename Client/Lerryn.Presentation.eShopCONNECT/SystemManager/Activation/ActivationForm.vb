' eShopCONNECT for Connected Business
' Module: ActivationForm.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.  This software has been generated using 
' the Interprise Suite SDK and may incorporate certain intellectual 
' property of Interprise Software Solutions International Inc who's
' rights are hereby recognised.
'
'       © 2012 - 2013  Lerryn Business Solutions Ltd
'                      2 East View
'                      Bessie Lane
'                      Bradwell
'                      Hope Valley
'                      S33 9HZ
'
'  Tel +44 (0)1433 621584
'  Email Support@lerryn.com
'
' Lerryn is a Trademark of Lerryn Business Solutions Ltd
' eShopCONNECT is a Trademark of Lerryn Business Solutions Ltd
'-------------------------------------------------------------------
'
' Last Updated - 10 June 2012

Option Explicit On
Option Strict On

Namespace SystemManager.Activation
    <MenuActionAttribute.MenuAction("OpenImpExpActivationForm")> _
Public Class ActivationForm

#Region " Variables "
        Private m_ActivationDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Private m_ActivationFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
#End Region

#Region " Methods "
#Region " Constructor "
        Public Sub New()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -    
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 02/12/11 | TJS             | 2011.2.00 | Modified for IS 6
            ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            MyBase.New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call
            m_ActivationDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            Me.m_ActivationFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_ActivationDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
            Me.Text = PRODUCT_NAME & " Activation Wizard" ' TJS 02/12/11
            Me.ToolBar.Visible = False ' TJS 02/12/11
            Me.MainMenuBar.Visible = False ' TJS 02/12/11
            Me.StatusBarPanel.Visible = False ' TJS 02/12/11
        End Sub
#End Region

#Region " InitializeControl "
        Public Overrides Sub InitializeControl()

            Try ' TJS 16/01/12
                MyBase.InitializeControl()
                DirectCast(Me.PanelBodyPluginInstance, ActivationWizardSectionContainer).InitialiseControls() ' TJS 02/12/11

            Catch ex As Exception
                Interprise.Presentation.Base.Message.MessageWindow.Show(ex) ' TJS 16/01/12

            End Try

        End Sub
#End Region

#Region " SetFocus "
        Public Sub SetFocus()
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '
            '   Description -    
            '
            ' Amendment Log
            '------------------------------------------------------------------------------------------
            ' Date     | Name            | Vers.     | Description
            '------------------------------------------------------------------------------------------
            ' 18/02/10 | TJS             | 2010.0.05 | Function added
            ' 18/03/11 | TJs             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
            ' 02/12/11 | TJS             | 2011.2.00 | Modified for IS 6
            ' 29/01/13 | TJS             | 2013.0.00 | Removed support for eCommerce Plus version
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If Me.PanelBodyPluginInstance IsNot Nothing Then
                DirectCast(Me.PanelBodyPluginInstance, ActivationWizardSectionContainer).SetFocus() ' TJS 02/12/11
            End If
        End Sub
#End Region
#End Region
    End Class
End Namespace
