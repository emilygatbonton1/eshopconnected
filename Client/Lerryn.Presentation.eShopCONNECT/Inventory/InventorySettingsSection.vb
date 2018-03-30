' eShopCONNECT for Connected Business
' Module: InventorySettingsSection.vb
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
' Last Updated - 20 November 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst

#Region " InventorySettingsSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, _
    "Lerryn.Presentation.ImportExport.InventorySettingsSection")> _
Public Class InventorySettingsSection

#Region " Variables "
    Private m_InventoryItemDataset As Interprise.Framework.Inventory.DatasetGateway.ItemDetailDatasetGateway
    Private m_InventoryItemControl As Interprise.Presentation.Inventory.Item.MainControl
    Private m_InventoryItemFacade As Interprise.Facade.Inventory.ItemDetailFacade
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.InventorySettingsSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_InventorySettingsFacade
        End Get
    End Property
#End Region
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
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Cursor = Cursors.WaitCursor ' TJS 18/03/11
        Me.m_InventorySettingsDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Me.m_InventorySettingsFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_InventorySettingsDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then
            Cursor = Cursors.Default
            Return
        End If
        Cursor = Cursors.Default ' TJS 18/03/11

    End Sub

    Public Sub New(ByVal InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
          ByVal InventorySettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)

        MyBase.New()

        Cursor = Cursors.WaitCursor ' TJS 18/03/11
        Me.m_InventorySettingsDataset = InventorySettingsDataset
        Me.m_InventorySettingsFacade = InventorySettingsSectionFacade

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then
            Cursor = Cursors.Default
            Return
        End If
        Cursor = Cursors.Default ' TJS 18/03/11

    End Sub
#End Region

#Region " UndoChanges "
    Public Overrides Sub UndoChanges()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/09/13 | TJS             | 2013.3.02 | Function added
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.UndoChanges()
        Try
            Select Case Me.XtraTabPublishOn.SelectedTabPage.Name
                Case Me.PageTab3DCart.Name ' TJS 20/11/13
                    DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).UndoChanges() ' TJS 20/11/13

                Case Me.PageTabAmazon.Name
                    DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).UndoChanges()

                Case Me.PageTabASPStorefront.Name
                    DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).UndoChanges()

                Case Me.PageTabChannelAdv.Name
                    DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).UndoChanges()

                Case Me.PageTabEBay.Name
                    DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).UndoChanges()

                Case Me.PageTabMagento.Name
                    DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).UndoChanges()

                Case Me.PageTabShopCom.Name
                    DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).UndoChanges()

            End Select

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try
    End Sub
#End Region
#End Region

#Region " Events "
#Region " PublishedOnPageChanged "
    Private Sub PublishedOnPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XtraTabPublishOn.SelectedPageChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to pass InventoryItemFacade reference to Amazon and Shop.com controls
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to add Channel ADvisor, ASPStorefront and Magento panels
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to display dummy panel with message on connectors not activated
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for IS 6 to remove Dummy section and add relevant labels to main sections
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to allow display of eBay form during development
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Try
            Select Case e.Page.Name
                ' start of code added TJS 20/11/13
                Case Me.PageTab3DCart.Name
                    If DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).InventoryItemDataset Is Nothing Then
                        DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset
                        DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade
                        DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).InitialiseControls()
                    End If
                    ' is connector activated ?
                    If Me.m_InventorySettingsFacade.IsConnectorActivated(THREE_D_CART_CONNECTOR_CODE) Then
                        ' yes, hide requires activation message
                        DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).ShowActivateMessage = False
                    Else
                        ' no, show requires activation message
                        DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).ShowActivateMessage = True
                    End If
                    ' end of code added TJS 20/11/13

                Case Me.PageTabAmazon.Name
                    If DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).InventoryItemDataset Is Nothing Then ' TJS 02/12/11
                        DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                        DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11
                        DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).InitialiseControls() ' TJS 02/12/11
                    End If
                    ' is connector activated ?
                    If Me.m_InventorySettingsFacade.IsConnectorActivated(AMAZON_SELLER_CENTRAL_CONNECTOR_CODE) Then ' TJS 18/03/11
                        ' yes, show under development massage for now
                        DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).ShowActivateMessage = False ' TJS 02/12/11
                        'DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).ShowDevelopmentMessage = True ' TJS 02/12/11
                    Else
                        ' no, show requires activation message
                        DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).ShowActivateMessage = True ' TJS 02/12/11
                    End If

                    ' start of code added TJS 19/08/10
                Case Me.PageTabASPStorefront.Name
                    If DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).InventoryItemDataset Is Nothing Then ' TJS 02/12/11
                        DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                        DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11
                        DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).InitialiseControls() ' TJS 02/12/11
                    End If
                    ' is connector activated ?
                    If Me.m_InventorySettingsFacade.IsConnectorActivated(ASP_STORE_FRONT_CONNECTOR_CODE) Then ' TJS 18/03/11 TJS 02/12/11
                        ' yes, hide requires activation message
                        DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).ShowActivateMessage = False ' TJS 02/12/11
                    Else
                        ' no, show requires activation message
                        DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).ShowActivateMessage = True ' TJS 02/12/11
                    End If

                Case Me.PageTabChannelAdv.Name
                    If DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).InventoryItemDataset Is Nothing Then ' TJS 02/12/11
                        DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                        DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11
                        DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).InitialiseControls() ' TJS 02/12/11
                    End If
                    ' is connector activated ?
                    If Me.m_InventorySettingsFacade.IsConnectorActivated(CHANNEL_ADVISOR_CONNECTOR_CODE) Then ' TJS 18/03/11
                        ' yes, hide requires activation message
                        DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).ShowActivateMessage = False ' TJS 02/12/11
                    Else
                        ' no, show requires activation message
                        DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).ShowActivateMessage = True ' TJS 02/12/11
                    End If

                Case Me.PageTabEBay.Name
                    If DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).InventoryItemDataset Is Nothing Then ' TJS 02/12/11
                        DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                        DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11
                        DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).InitialiseControls() ' TJS 02/12/11
                    End If
                    ' is connector activated ?
                    If Me.m_InventorySettingsFacade.IsConnectorActivated(EBAY_CONNECTOR_CODE) Then ' TJS 18/03/11
                        ' yes, show under development massage for now
                        DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).ShowActivateMessage = False ' TJS 02/12/11
                        If m_InventorySettingsFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "ShowEBayWizard", "NO").ToUpper <> "YES" Then ' TJS 14/02/12
                            'DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).ShowDevelopmentMessage = True ' TJS 02/12/11
                        End If
                    Else
                        ' no, show requires activation message
                        DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).ShowActivateMessage = True ' TJS 02/12/11
                    End If

                Case Me.PageTabMagento.Name
                    If DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).InventoryItemDataset Is Nothing Then ' TJS 02/12/11
                        DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                        DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11
                        DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).InitialiseControls() ' TJS 02/12/11
                    End If
                    ' is connector activated ?
                    If Me.m_InventorySettingsFacade.IsConnectorActivated(MAGENTO_CONNECTOR_CODE) Then ' TJS 18/03/11
                        ' yes, hide requires activation message
                        DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).ShowActivateMessage = False ' TJS 02/12/11
                    Else
                        ' no, show requires activation message
                        DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).ShowActivateMessage = True ' TJS 02/12/11
                    End If
                    ' end of code added TJS 19/08/10

                Case Me.PageTabShopCom.Name
                    If DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).InventoryItemDataset Is Nothing Then ' TJS 02/12/11
                        DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                        DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11
                        DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).InitialiseControls() ' TJS 02/12/11
                    End If
                    ' is connector activated ?
                    If Me.m_InventorySettingsFacade.IsConnectorActivated(SHOP_DOT_COM_CONNECTOR_CODE) Then ' TJS 18/03/11
                        ' yes, hide requires activation message
                        DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).ShowActivateMessage = False ' TJS 02/12/11
                    Else
                        ' no, show requires activation message
                        DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).ShowActivateMessage = True ' TJS 02/12/11
                    End If

            End Select

        Catch ex As Exception
            Interprise.Presentation.Base.Message.MessageWindow.Show(ex)

        End Try

    End Sub
#End Region

#Region " XtraTabPublishOn_VisibleChanged "
    Private Sub XtraTabPublishOn_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles XtraTabPublishOn.VisibleChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 27/06/09 | TJS             | 2009.3.00 | Modified to pass InventoryItemFacade reference to Amazon and Shop.com controls
        ' 18/03/11 | TJS             | 2011.0.01 | Added wait cursor and modified to display dummy panel with message on connectors 
        '                                        | not activated 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for IS 6
        ' 19/04/12 | TJS             | 2012.1.01 | Moved code from InitialiseControls to cater for IS6 loading control before InventoryItemCode is known
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to cater for CB 13.1 renaming MainControl as MainCBNControl
        ' 20/11/13 | TJS             | 2013.4.00 | Modified for 3DCart connector
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If Interprise.Presentation.Base.BaseRibbonForm.UserRoleMode Then Return

        Dim strMainControlName As String ' TJS 13/03/13

        Cursor = Cursors.WaitCursor ' TJS 18/03/11
        ' get reference to InventoryItemControl
        If Me.ParentForm.Controls("PanelBody").Controls("MainCBNControl") IsNot Nothing Then ' TJS 13/03/13
            strMainControlName = "MainCBNControl" ' TJS 13/03/13
        ElseIf Me.ParentForm.Controls("PanelBody").Controls("MainControl") IsNot Nothing Then ' TJS 13/03/13
            strMainControlName = "MainControl" ' TJS 13/03/13
        Else
            strMainControlName = "" ' TJS 13/03/13
        End If
        If Me.ParentForm.Controls("PanelBody") IsNot Nothing AndAlso strMainControlName <> "" AndAlso Me.m_InventoryItemControl Is Nothing Then ' TJS 02/12/11 TJS 19/04/12 TJS 13/03/13
            DirectCast(Me.Parent, Interprise.Presentation.Base.PluginContainerControl).ShowCaption = False ' TJS 02/12/11
            Me.m_InventoryItemControl = DirectCast(Me.ParentForm.Controls("PanelBody").Controls(strMainControlName), Interprise.Presentation.Inventory.Item.MainControl) ' TJS 02/12/11 TJS 13/03/13
            ' from this, get reference to ItemDetailDataset
            Me.m_InventoryItemDataset = m_InventoryItemControl.ItemDetailDataset ' TJS 02/12/11
            ' and ItemDetailFacade
            Me.m_InventoryItemFacade = DirectCast(m_InventoryItemControl.CurrentFacade, Interprise.Facade.Inventory.ItemDetailFacade) ' TJS 02/12/11
            If Me.PluginContainer3DCartSettingsPluginInstance IsNot Nothing Then ' TJS 20/11/13
                DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 20/11/13
                DirectCast(Me.PluginContainer3DCartSettingsPluginInstance, Inventory3DCartSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 20/11/13

            ElseIf Me.PluginContainerAmazonSettingsPluginInstance IsNot Nothing Then ' TJS 02/12/11
                DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                DirectCast(Me.PluginContainerAmazonSettingsPluginInstance, InventoryAmazonSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11

            ElseIf Me.PluginContainerASPStorefrontSettingsPluginInstance IsNot Nothing Then ' TJS 02/12/11
                DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                DirectCast(Me.PluginContainerASPStorefrontSettingsPluginInstance, InventoryASPStorefrontSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11

            ElseIf Me.PluginContainerChannelAdvSettingsPluginInstance IsNot Nothing Then ' TJS 02/12/11
                DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                DirectCast(Me.PluginContainerChannelAdvSettingsPluginInstance, InventoryChannelAdvSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11

            ElseIf Me.PluginContainerEBaySettingsPluginInstance IsNot Nothing Then ' TJS 02/12/11
                DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                DirectCast(Me.PluginContainerEBaySettingsPluginInstance, InventoryEBaySettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11

            ElseIf Me.PluginContainerMagentoSettingsPluginInstance IsNot Nothing Then ' TJS 02/12/11
                DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                DirectCast(Me.PluginContainerMagentoSettingsPluginInstance, InventoryMagentoSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11

            ElseIf Me.PluginContainerShopComSettingsPluginInstance IsNot Nothing Then ' TJS 02/12/11
                DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).InventoryItemDataset = Me.m_InventoryItemDataset ' TJS 02/12/11
                DirectCast(Me.PluginContainerShopComSettingsPluginInstance, InventoryShopComSettingsSection).InventoryItemFacade = Me.m_InventoryItemFacade ' TJS 02/12/11

            End If
        End If
        Cursor = Cursors.Default ' TJS 18/03/11

    End Sub
#End Region
#End Region
End Class
#End Region
