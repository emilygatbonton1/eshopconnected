' eShopCONNECT for Connected Business
' Module: ImportWizardSectionContainer.vb
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
' Last Updated - 06 Febrary 2014

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Microsoft.VisualBasic
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

#Region " ImportWizardSectionContainer "
Public Class ImportWizardSectionContainer

#Region " Variables "
    Private m_ImportWizardsectionContainerFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_ItemsForImport As DataTable
    Private bIgnoreWizardPageChangeEvent As Boolean
    Private WithEvents m_MagentoImportFacade As Lerryn.Facade.ImportExport.MagentoImportFacade ' TJS 04/05/11
    Private WithEvents m_ASPStorefrontImportFacade As Lerryn.Facade.ImportExport.ASPStorefrontImportFacade
    Private WithEvents m_ChanAdvImportFacade As Lerryn.Facade.ImportExport.ChannelAdvImportFacade ' TJS 04/05/11
    Private WithEvents m_AmazonImportFacade As Lerryn.Facade.ImportExport.AmazonImportFacade ' TJS 05/07/12
    Private m_CategoryTable As System.Data.DataTable = Nothing

    Private WithEvents m_BackgroundWorker As System.ComponentModel.BackgroundWorker
    Public tmr As System.Threading.Timer = Nothing ' TJS 05/07/12
    Private ts As TimerState ' TJS 05/07/12
    Private m_RequestProductReportSuccess As Boolean ' TJS 05/07/12
    Private m_PollProductReportSuccess As Boolean ' TJS 05/07/12
    Private m_GetProductListSuccess As Boolean
    Private m_GetSourceCategoriesSuccess As Boolean ' TJS 02/12/11
    Private m_NoOfProductsImported As Integer
    Private m_NoOfSourceProductsProcessed As Integer ' TJS 16/06/13
    Private m_NoOfProductsSkipped As Integer
    Private m_ImportLimitReached As Boolean ' TJS 02/12/11
    Private m_ManualActionRequired As Boolean ' TJS 24/03/13
    Private m_InhibitRemoveEventHandlers As Boolean = False ' TJS 05/07/12
    Private m_AmazonReportRequestID As String = "" ' TJS 05/07/12
    Private m_ImportErrorUpdateConfirmed As Boolean = False ' TJS 09/08/13
    Private m_ImportSKUChangeUpdateConfirmed As Boolean = False ' TJS 09/08/13
    Private m_ImportSourceIDUpdateConfirmed As Boolean = False ' TJS 09/08/13
    Private m_ImportUpdateExistingItemConfirmed As Boolean = False ' TJS 09/08/13
    Private m_ImportSource As String = "" ' TJS 09/12/13
    Private m_SiteOrAccount As String = "" ' TJS 09/12/13
    Private m_SiteOrAccountID As String = "" ' TJS 09/12/13

    Private Class TimerState ' TJS 05/07/12

        Public TimerCounter As Integer = 0 ' TJS 05/07/12
        Public ServiceState As Integer = 0 ' TJS 05/07/12

    End Class

    Private Delegate Sub GetProductListCompletedCallback() ' TJS 05/07/12

#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.ImportWizardSectionContainerGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_ImportWizardsectionContainerFacade

        End Get
    End Property
#End Region

#Region " IsActivated "
    Public ReadOnly Property IsActivated() As Boolean
        Get
            If m_ImportWizardsectionContainerFacade IsNot Nothing Then
                Return m_ImportWizardsectionContainerFacade.IsActivated
            Else
                Return False
            End If
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
        ' 20/04/12 | TJS             | 2012.1.02 | Modified to call CheckAndUpdateConfigSettings
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        '------------------------------------------------------------------------------------------

        MyBase.New()

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.m_ImportWizardsectionContainerFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.ImportWizardSectionContainerGateway, _
            New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12
        Me.m_ImportWizardsectionContainerFacade.CheckAndUpdateConfigSettings() ' TJS 20/04/12

        bIgnoreWizardPageChangeEvent = False

    End Sub
#End Region

#Region " InitialiseControls "
    Public Sub InitialiseControls()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/04/11 | TJS             | 2011.0.08 | Modified to cater for IS 4.8 build using conditional compile
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to initise the Quantity Publishing options and enable Channel Advisor
        ' 05/07/12 | TJS             | 2012.1.08 | Enabled Amazon import
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for non-stock items
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim strWelcomeMessage As String, strTemp As String
        Dim iProductsAlreadyImported As Integer ' TJS 29/03/11
        Dim m_CheckConnectorHasActivated() As DataRow ' mark kee 2/12/2015

        'Add any initialization after the InitializeControl() call
        If Me.m_ImportWizardsectionContainerFacade.IsActivated Then
            iProductsAlreadyImported = CInt(m_ImportWizardsectionContainerFacade.GetField("SELECT (SELECT COUNT(*) FROM InventoryAmazonDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                "(SELECT COUNT(*) FROM InventoryASPStorefrontDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                "(SELECT COUNT(*) FROM InventoryChannelAdvDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1) + " & _
                "(SELECT COUNT(*) FROM InventoryMagentoDetails_DEV000221 WHERE FromImportWizard_DEV000221 = 1)", CommandType.Text, Nothing)) ' TJS 29/03/11 TJS 05/07/12
            If iProductsAlreadyImported < m_ImportWizardsectionContainerFacade.InventoryImportLimit Then ' TJS 29/03/11

                strWelcomeMessage = "This wizard will guide you through the initial import of your existing Inventory Items from a supported external marketplace or shopping cart." & vbCrLf & vbCrLf & "Please select the Source you wish to import Inventory Items from." & vbCrLf & vbCrLf & "NOTE  You can only import from Sources that you have already activated."
                Coll = Me.cbeImportSource.Properties.Items
                Coll.BeginUpdate()
                Coll.Clear()
                ' sources not yet implemented are commeted out
                ' Inventory import is possible up to import limit even if source is not activated

                'additional code Check Connector Has Activated 'mark kee 6/12/2015 
                'm_CheckConnectorHasActivated = Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221.Select("IsActive = 1")
                'For iLoop = 0 To m_CheckConnectorHasActivated.Length - 1
                '    If m_CheckConnectorHasActivated(iLoop)("SourceCode_DEV000221").ToString.Contains("AmazonOrder") Then
                '        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Amazon") ' TJS 05/07/12
                '        Coll.Add(CollItem)
                '    ElseIf m_CheckConnectorHasActivated(iLoop)("SourceCode_DEV000221").ToString.Contains("ASPStoreFrontOrder") Then
                '        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("ASPDotNetStorefront")
                '        Coll.Add(CollItem)
                '    ElseIf m_CheckConnectorHasActivated(iLoop)("SourceCode_DEV000221").ToString.Contains("ChanAdvOrder") Then
                '        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Channel Advisor")
                '        Coll.Add(CollItem)
                '    ElseIf m_CheckConnectorHasActivated(iLoop)("SourceCode_DEV000221").ToString.Contains("MagentoOrder") Then
                '        CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Magento")
                '        Coll.Add(CollItem)
                '    End If
                'Next

                'end additional Code


                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Amazon") ' TJS 05/07/12 
                Coll.Add(CollItem) ' TJS 05/07/12
                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("ASPDotNetStorefront")
                Coll.Add(CollItem)
                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Channel Advisor") ' TJS 02/12/11
                Coll.Add(CollItem) ' TJS 02/12/11
                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Magento")
                Coll.Add(CollItem)


                '    CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Volusion")
                '    Coll.Add(CollItem)
                Coll.EndUpdate()
                strTemp = Me.m_ImportWizardsectionContainerFacade.GetField("SELECT COUNT(*) FROM SystemCategory", CommandType.Text, Nothing)
                If strTemp <> "" Then
                    If CInt(strTemp) > 1 Then
                        Me.chkCreateCategories.Enabled = False
                        Me.lblCategoriesExist.Visible = True
                    Else
                        Me.chkCreateCategories.Enabled = True
                        Me.lblCategoriesExist.Visible = False
                    End If
                Else
                    Me.chkCreateCategories.Enabled = True
                    Me.lblCategoriesExist.Visible = False
                End If
                Me.RadioGroupQtyPublishing.SelectedIndex = -1 ' TJS 02/12/11

            Else
                ' start of code added TJS 29/03/11
                strWelcomeMessage = "You have already imported the maximum number of Inventory Items permitted on your "
                If m_ImportWizardsectionContainerFacade.IsFullActivation Then
                    strWelcomeMessage = strWelcomeMessage & "activation"
                Else
                    strWelcomeMessage = strWelcomeMessage & "evaluation activation"
                End If
                strWelcomeMessage = strWelcomeMessage & "." & vbCrLf & vbCrLf & "You must run the eShopCONNECTED Activation Wizard and purchase "
                If m_ImportWizardsectionContainerFacade.IsFullActivation Then
                    strWelcomeMessage = strWelcomeMessage & "an upgraded activation"
                Else
                    strWelcomeMessage = strWelcomeMessage & "a full activation"
                End If
                strWelcomeMessage = strWelcomeMessage & " before you can import any more Inventory Items."
                ' end of code added TJS 29/03/11
            End If
        Else
            strWelcomeMessage = "You must activate eShopCONNECTED first."
        End If
        Me.WizardControlImport.WelcomeMessage = strWelcomeMessage
        Me.chkCreateCategories.Properties.Caption = "Create " & IS_PRODUCT_NAME & " Categories from source Categories" ' TJS 24/08/12
        Me.colTreeListISCategoryName.Caption = "Map to " & IS_PRODUCT_NAME & " Category" ' TJS 24/08/12
        Me.lblMappingNotes.Text = "NOTE  If a Source Category does not have a mapped" & vbCrLf & IS_PRODUCT_INITIALS & " Category, then any imported products assigned to" & _
            vbCrLf & "that Source Category WILL be imported, but they will" & vbCrLf & "NOT be assigned to any " & IS_PRODUCT_NAME & " Category." & vbCrLf & vbCrLf & _
            "Categories will have to be manually assigned once" & vbCrLf & "the import process is complete." ' TJS 24/08/12
        Me.colItemName.Caption = "Source Item Name -> " & IS_PRODUCT_INITIALS & " Item Description" ' TJS 24/08/12
        Me.colItemSKU.Caption = "Source Item SKU -> " & IS_PRODUCT_INITIALS & " Item Code" ' TJS 24/08/12
        Me.colISItemType.Caption = IS_PRODUCT_INITIALS & " Item Type" ' TJS  09/03/13
        Me.WizardControlImport.buttonNext.Enabled = False

    End Sub
#End Region

#Region " RequestProductListingReport "
    Private Sub RequestProductListingReport(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Select Case m_ImportSource ' TJS 09/12/13
            Case "Amazon"
                ' issue request for Product Listing report to Amazon
                m_RequestProductReportSuccess = m_AmazonImportFacade.RequestAmazonProductListingReport(m_AmazonReportRequestID, worker.CancellationPending)

        End Select

    End Sub

    Private Sub RequestProductListingReportCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Select Case Me.cbeImportSource.EditValue.ToString
            Case "Amazon"
                If m_AmazonImportFacade.LastError <> "" Then
                    m_RequestProductReportSuccess = False
                End If

        End Select
        RemoveHandler m_BackgroundWorker.DoWork, AddressOf RequestProductListingReport
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf RequestProductListingReportCompleted

        If m_RequestProductReportSuccess Then
            tmr.Change(60000, 60000)
        Else
            Select Case Me.cbeImportSource.EditValue.ToString
                Case "Amazon"
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Error occured whilst requesting Amazon Product Listing report - " & m_AmazonImportFacade.LastError)
                    Me.lblGetListError.Text = Me.lblGetListError.Text & vbCrLf & "Error occured whilst requesting Amazon Product Listing report" & vbCrLf & m_AmazonImportFacade.LastError

            End Select
        End If

    End Sub

    Private Sub timerCallback(ByVal state As Object)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        tmr.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite)

        m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
        m_BackgroundWorker.WorkerSupportsCancellation = True
        m_BackgroundWorker.WorkerReportsProgress = False
        AddHandler m_BackgroundWorker.DoWork, AddressOf PollForProductListingReport
        AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf PollForProductListingReportCompleted
        m_BackgroundWorker.RunWorkerAsync()

    End Sub
#End Region

#Region " PollForProductListingReport "
    Private Sub PollForProductListingReport(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        Dim bInhibitWebPosts As Boolean

        ' check registry for inhibit web post setting (prevents sending posts during testing)
        bInhibitWebPosts = (m_ImportWizardsectionContainerFacade.CheckRegistryValue(REGISTRY_KEY_ROOT, "InhibitWebPosts", "NO").ToUpper = "YES")

        m_PollProductReportSuccess = m_AmazonImportFacade.PollAmazonProductListingReport(m_AmazonReportRequestID, worker.CancellationPending, bInhibitWebPosts, m_ItemsForImport)

    End Sub

    Private Sub PollForProductListingReportCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        RemoveHandler m_BackgroundWorker.DoWork, AddressOf PollForProductListingReport
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf PollForProductListingReportCompleted
        If m_PollProductReportSuccess Then
            ' remove callback timer
            tmr.Dispose()

            m_InhibitRemoveEventHandlers = True
            m_GetProductListSuccess = True
            If Me.GridControlSelectItems.InvokeRequired Then

                Dim d As New GetProductListCompletedCallback(AddressOf GetProductListCompleted)
                Me.Invoke(d, Nothing)
            Else
                GetProductListCompleted()
            End If

        Else
            ' set poll timer for 1 minute
            tmr.Change(60000, 60000)
        End If

    End Sub
#End Region

#Region " GetProductListToImport "
    Private Sub GetProductListToImport(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to add Channel Advisor
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        m_InhibitRemoveEventHandlers = False ' TJS 05/07/12
        Select Case m_ImportSource ' TJS 09/12/13
            Case "ASPDotNetStorefront"
                ' get list of products from Amazon
                m_GetProductListSuccess = m_ASPStorefrontImportFacade.GetASPStorefrontProductList(m_SiteOrAccount, worker.CancellationPending, m_ItemsForImport) ' TJS 09/12/13

            Case "Channel Advisor" ' TJS 02/12/11
                ' get list of products from Channel Advisor
                m_GetProductListSuccess = m_ChanAdvImportFacade.GetChannelAdvProductList(m_SiteOrAccountID, worker.CancellationPending, m_ItemsForImport) ' TJS 02/12/11 TJS 09/12/13

            Case "Magento"
                ' get list of products from Magento
                m_GetProductListSuccess = m_MagentoImportFacade.GetMagentoProductList(m_SiteOrAccount, worker.CancellationPending, m_ItemsForImport) ' TJS 09/12/13
        End Select

    End Sub

    Private Sub GetProductListCompleted()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/03/11 | TJS             | 2011.0.04 | Modified to disable grid and select buttons during grid population
        ' 06/04/11 | TJS             | 2011.0.08 | Moved setting of GridControlSelectItems DataSource from WizardPageChanged
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to ensure error messages are not lost if user not watching screen when IS error popup appears
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for Magento simple types
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

        m_ImportLimitReached = False ' TJS 02/12/11
        Select Case Me.cbeImportSource.EditValue.ToString ' TJS 02/12/11
            Case "Amazon" ' TJS 05/07/12
                If m_AmazonImportFacade.LastError <> "" Then ' TJS 05/07/12
                    m_GetProductListSuccess = False ' TJS 05/07/12

                ElseIf m_AmazonImportFacade.ImportLimitReached Then ' TJS 05/07/12
                    m_GetProductListSuccess = False ' TJS 05/07/12
                    m_ImportLimitReached = True ' TJS 05/07/12
                End If

            Case "ASPDotNetStorefront" ' TJS 02/12/11
                ' get list of products from ASPDotNetStorefront
                If m_GetProductListSuccess Then
                    Coll = Me.repSourceItemType.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each ASPStorefrontProductType As Lerryn.Facade.ImportExport.ASPStorefrontImportFacade.ProductType In m_ASPStorefrontImportFacade.ProductTypeList
                        CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(ASPStorefrontProductType.Description, ASPStorefrontProductType.TypeID)
                        Coll.Add(CollItem)
                    Next
                    Coll.EndUpdate()
                    Me.colSourceType.ColumnEdit = Me.repSourceItemType
                End If

                If m_ASPStorefrontImportFacade.LastError <> "" Then ' TJS 02/12/11
                    m_GetProductListSuccess = False ' TJS 02/12/11

                ElseIf m_ASPStorefrontImportFacade.ImportLimitReached Then ' TJS 02/12/11
                    m_GetProductListSuccess = False ' TJS 02/12/11
                    m_ImportLimitReached = True ' TJS 02/12/11
                End If

            Case "Channel Advisor" ' TJS 02/12/11
                If m_ChanAdvImportFacade.LastError <> "" Then ' TJS 02/12/11
                    m_GetProductListSuccess = False ' TJS 02/12/11

                ElseIf m_ChanAdvImportFacade.ImportLimitReached Then ' TJS 02/12/11
                    m_GetProductListSuccess = False ' TJS 02/12/11
                    m_ImportLimitReached = True ' TJS 02/12/11
                End If

            Case "Magento" ' TJS 02/12/11
                ' start of code added TJS 09/03/13
                ' populate IS/CB Item type selector
                If m_GetProductListSuccess Then
                    Coll = Me.repISItemType.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Stock Item", "Stock")
                    Coll.Add(CollItem)
                    CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Non-Stock Item", "Non-Stock")
                    Coll.Add(CollItem)
                    Coll.EndUpdate()
                    Me.colISItemType.ColumnEdit = Me.repISItemType
                End If
                ' end of code added TJS 09/03/13

                If m_MagentoImportFacade.LastError <> "" Then ' TJS 02/12/11
                    m_GetProductListSuccess = False ' TJS 02/12/11

                ElseIf m_MagentoImportFacade.ImportLimitReached Then ' TJS 02/12/11
                    m_GetProductListSuccess = False ' TJS 02/12/11
                    m_ImportLimitReached = True ' TJS 02/12/11
                End If

        End Select

        If m_GetProductListSuccess Then
            Me.GridControlSelectItems.DataSource = m_ItemsForImport.DefaultView ' TJS 06/04/11
            Me.WizardControlImport.EnableNextButton = True
            Me.GridControlSelectItems.Enabled = True ' TJS 29/03/11
            Me.btnSelectAll.Enabled = True ' TJS 29/03/11
            Me.btnSelectNone.Enabled = True ' TJS 29/03/11
            Me.pnlGetListError.Visible = False ' TJS 14/02/12
        Else
            Select Case Me.cbeImportSource.EditValue.ToString ' TJS 02/12/11
                Case "Amazon" ' TJS 05/07/12
                    Interprise.Presentation.Base.Message.MessageWindow.Show(m_AmazonImportFacade.LastError) ' TJS 05/07/12
                    Me.lblGetListError.Text = m_AmazonImportFacade.LastError ' TJS 05/07/12
                    Me.pnlGetListError.Visible = True ' TJS 22/06/122

                Case "ASPDotNetStorefront" ' TJS 02/12/11
                    Interprise.Presentation.Base.Message.MessageWindow.Show(m_ASPStorefrontImportFacade.LastError) ' TJS 02/12/11
                    Me.lblGetListError.Text = m_ASPStorefrontImportFacade.LastError ' TJS 14/02/12
                    Me.pnlGetListError.Visible = True ' TJS 14/02/12

                Case "Channel Advisor" ' TJS 02/12/11
                    Interprise.Presentation.Base.Message.MessageWindow.Show(m_ChanAdvImportFacade.LastError) ' TJS 02/12/11
                    Me.lblGetListError.Text = m_ChanAdvImportFacade.LastError ' TJS 14/02/12
                    Me.pnlGetListError.Visible = True ' TJS 14/02/12

                Case "Magento" ' TJS 02/12/11
                    Interprise.Presentation.Base.Message.MessageWindow.Show(m_MagentoImportFacade.LastError) ' TJS 02/12/11
                    Me.lblGetListError.Text = m_MagentoImportFacade.LastError ' TJS 14/02/12
                    Me.pnlGetListError.Visible = True ' TJS 14/02/12

            End Select
        End If
        Me.pnlGetListProgress.Visible = False
        If Not m_InhibitRemoveEventHandlers Then ' TJS 05/07/12
            RemoveHandler m_BackgroundWorker.DoWork, AddressOf GetProductListToImport
            RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf ProcessGetProductListCompleted ' TJS 05/07/12
        Else
            m_InhibitRemoveEventHandlers = False ' TJS 05/07/12
        End If

    End Sub

    Private Sub ProcessGetProductListCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Function added to allow cross thread calls to GetProductListCompleted on Amazon import
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        GetProductListCompleted()

    End Sub
#End Region

#Region " GetSourceCategoriesForMapping "
    Private Sub GetSourceCategoriesForMapping(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/04/11 | TJS             | 2011.0.10 | Function added
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Select Case m_ImportSource ' TJS 09/12/13
            Case "ASPDotNetStorefront"
                m_GetSourceCategoriesSuccess = m_ASPStorefrontImportFacade.GetASPStorefrontCategories(m_SiteOrAccount, worker.CancellationPending, m_CategoryTable) ' TJS 09/12/13

            Case "Magento"
                m_GetSourceCategoriesSuccess = m_MagentoImportFacade.GetMagentoCategories(m_SiteOrAccount, worker.CancellationPending, m_CategoryTable) ' TJS 09/12/13

        End Select

    End Sub

    Private Sub GetSourceCategoriesCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/04/11 | TJS             | 2011.0.10 | Function added
        ' 20/05/12 | TJS             | 2012.1.04 | Modified to remove handlers on completion to prevent function being triggered again on import
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If m_GetSourceCategoriesSuccess Then ' TJS 02/12/11
            Me.TreeListCategories.DataSource = m_CategoryTable
            Me.TreeListCategories.ExpandAll()
            Me.TreeListCategories.Enabled = True
            Me.pnlGetCategoryProgress.Visible = False
            Me.WizardControlImport.EnableNextButton = True
        Else
            Select Case Me.cbeImportSource.EditValue.ToString ' TJS 02/12/11

                Case "ASPDotNetStorefront" ' TJS 02/12/11
                    Interprise.Presentation.Base.Message.MessageWindow.Show(m_ASPStorefrontImportFacade.LastError) ' TJS 02/12/11

                Case "Magento" ' TJS 02/12/11
                    Interprise.Presentation.Base.Message.MessageWindow.Show(m_MagentoImportFacade.LastError) ' TJS 02/12/11

            End Select
        End If
        RemoveHandler m_BackgroundWorker.DoWork, AddressOf GetSourceCategoriesForMapping ' TJS 20/05/12
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetSourceCategoriesCompleted ' TJS 20/05/12

    End Sub
#End Region

#Region " ImportSelectedProducts "
    Private Sub ImportSelectedProducts(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to add Channel Advisor and for Import Limit Reached
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon
        ' 24/03/13 | TJS             | 2013.1.06 | Modified to cater for Magento Manual Action required
        ' 16/06/13 | TJS             | 2013.1.20 | Modified to change reporting of number of items 
        '                                        | processed and imported on Magento
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento Atribute values table
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Select Case m_ImportSource ' TJS 09/12/13
            Case "Amazon" ' TJS 05/07/12
                m_AmazonImportFacade.ImportAmazonProducts(m_ItemsForImport, worker.CancellationPending, m_NoOfProductsImported, m_NoOfProductsSkipped) ' TJS 05/07/12
                m_ImportLimitReached = m_AmazonImportFacade.ImportLimitReached ' TJS 05/07/12
                m_ManualActionRequired = False ' TJS 24/03/13

            Case "ASPDotNetStorefront"
                m_ASPStorefrontImportFacade.ImportASPStorefrontProducts(m_SiteOrAccount, m_ItemsForImport, worker.CancellationPending, m_NoOfProductsImported, m_NoOfProductsSkipped) ' TJS 09/12/13
                m_ImportLimitReached = m_ASPStorefrontImportFacade.ImportLimitReached ' TJS 02/12/11
                m_ManualActionRequired = False ' TJS 24/03/13

            Case "Channel Advisor" ' TJS 02/12/11
                m_ChanAdvImportFacade.ImportChannelAdvProducts(m_SiteOrAccountID, m_ItemsForImport, worker.CancellationPending, m_NoOfProductsImported, m_NoOfProductsSkipped) ' TJS 02/12/11 TJS 09/12/13
                m_ImportLimitReached = m_ChanAdvImportFacade.ImportLimitReached ' TJS 02/12/11
                m_ManualActionRequired = False ' TJS 24/03/13

            Case "Magento"
                m_MagentoImportFacade.GetMagentoAttributeList(m_SiteOrAccount, worker.CancellationPending) ' TJS 13/11/13 TJS 09/12/13

                m_MagentoImportFacade.ImportMagentoProducts(m_SiteOrAccount, m_ItemsForImport, worker.CancellationPending, m_NoOfSourceProductsProcessed, m_NoOfProductsImported, m_NoOfProductsSkipped) ' TJS 16/06/13 TJS 09/12/13
                m_ImportLimitReached = m_MagentoImportFacade.ImportLimitReached ' TJS 02/12/11
                m_ManualActionRequired = m_MagentoImportFacade.ManualActionRequired ' TJS 24/03/13

        End Select

    End Sub

    Private Sub ProductImportCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for Import Limit reached flag
        ' 20/05/12 | TJS             | 2012.1.04 | Modified to remove handlers on completion to prevent function being triggered again 
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to check for errors which aborted the import
        ' 24/03/13 | TJS             | 2013.1.06 | Modified to cater for Magento Manual Action required
        ' 16/06/13 | TJS             | 2013.1.20 | Modified to change reporting of number of items 
        '                                        | processed and imported on Magento
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strFinishMessage As String, strLastError As String ' TJS 02/12/11 TJS 22/03/13

        Select Case Me.cbeImportSource.EditValue.ToString ' TJS 22/03/13
            Case "ASPDotNetStorefront"
                strLastError = m_ASPStorefrontImportFacade.LastError ' TJS 22/03/13

            Case "Magento"
                strLastError = m_MagentoImportFacade.LastError ' TJS 22/03/13

            Case Else ' TJS 22/03/13
                strLastError = "" ' TJS 22/03/13

        End Select

        If m_NoOfProductsImported > 0 Then
            If strLastError <> "" Then ' TJS 22/03/13
                strFinishMessage = "The eShopCONNECTED Inventory Import Wizard encountered an error which prevented it completing the import." & vbCrLf & vbCrLf & _
                    m_NoOfProductsImported & " products were imported and " & m_NoOfProductsSkipped & " were skipped." & vbCrLf & vbCrLf ' TJS 22/03/13
            ElseIf Me.cbeImportSource.EditValue.ToString = "Magento" Then ' TJS 16/06/13
                strFinishMessage = "You have successfully completed the eShopCONNECTED Inventory Import Wizard." & vbCrLf & vbCrLf & m_NoOfSourceProductsProcessed & _
                   " Magento products were processed, " & m_NoOfProductsImported & " products were imported and " & m_NoOfProductsSkipped & " were skipped." & vbCrLf & vbCrLf ' TJS 16/06/13
            Else
                strFinishMessage = "You have successfully completed the eShopCONNECTED Inventory Import Wizard." & vbCrLf & vbCrLf & _
                    m_NoOfProductsImported & " products were imported and " & m_NoOfProductsSkipped & " were skipped." & vbCrLf & vbCrLf
            End If
            If m_ManualActionRequired Then ' TJS 24/03/13
                strFinishMessage = strFinishMessage & "There are some manual actions that you need to carry out - see the Import Log for more details" & vbCrLf & vbCrLf ' TJS 24/03/13
            End If
            If m_ImportLimitReached Then ' TJS 02/12/11
                strFinishMessage = strFinishMessage & "However you have reached the maximum number of Inventory Items permitted to be imported on your Activation." ' TJS 02/12/11
                strFinishMessage = strFinishMessage & vbCrLf & vbCrLf & "You must run the eShopCONNECTED Activation Wizard and purchase " ' TJS 02/12/11
                If m_ImportWizardsectionContainerFacade.IsFullActivation Then ' TJS 02/12/11
                    strFinishMessage = strFinishMessage & "an upgraded activation" ' TJS 02/12/11
                Else
                    strFinishMessage = strFinishMessage & "a full activation" ' TJS 02/12/11
                End If
                strFinishMessage = strFinishMessage & " before you can import any more Inventory Items." & vbCrLf & vbCrLf ' TJS 02/12/11
            End If
        Else
            If strLastError <> "" Then ' TJS 22/03/13
                strFinishMessage = "The eShopCONNECTED Inventory Import Wizard encountered an error which prevented it completing the import." & vbCrLf & vbCrLf & _
                    "No products were imported and " & m_NoOfProductsSkipped & " were skipped." & vbCrLf & vbCrLf ' TJS 22/03/13
            Else
                strFinishMessage = "You have successfully completed the eShopCONNECTED Inventory Import Wizard, but " & _
                    "no products were imported and " & m_NoOfProductsSkipped & " were skipped." & vbCrLf & vbCrLf
            End If
            If m_ManualActionRequired Then ' TJS 24/03/13
                strFinishMessage = strFinishMessage & "There are some manual actions that you need to carry out - see the Import Log for more details" & vbCrLf & vbCrLf ' TJS 24/03/13
            End If
            If m_ImportLimitReached Then ' TJS 02/12/11
                strFinishMessage = strFinishMessage & "You have reached the maximum number of Inventory Items permitted to be imported on your Activation."
                strFinishMessage = strFinishMessage & vbCrLf & vbCrLf & "You must run the eShopCONNECTED Activation Wizard and purchase " ' TJS 02/12/11
                If m_ImportWizardsectionContainerFacade.IsFullActivation Then ' TJS 02/12/11
                    strFinishMessage = strFinishMessage & "an upgraded activation" ' TJS 02/12/11
                Else
                    strFinishMessage = strFinishMessage & "a full activation" ' TJS 02/12/11
                End If
                strFinishMessage = strFinishMessage & " before you can import any more Inventory Items." & vbCrLf & vbCrLf ' TJS 02/12/11
            End If
        End If
        strFinishMessage = strFinishMessage & "Please click Finish to exit the Wizard."
        Me.WizardControlImport.FinishMessage = strFinishMessage
        Me.pnlImportProgress.Visible = False
        Me.WizardControlImport.SelectedTabPage = Me.TabPageComplete
        Me.WizardControlImport.EnableNextButton = True
        Me.WizardControlImport.EnableCancelButton = False ' TJS 02/12/11
        Me.WizardControlImport.EnableBackButton = False ' TJS 02/12/11
        RemoveHandler m_BackgroundWorker.DoWork, AddressOf ImportSelectedProducts ' TJS 20/05/12
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf ProductImportCompleted ' TJS 20/05/12

    End Sub
#End Region

#Region " SetCategoryMappingLabels "
    Private Sub SetCategoryMappingLabels(ByVal TargetName As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added using code from WizardPageChanged plus additional lable settings
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.lblSelectCatgoryMapping.Text = "As " & IS_PRODUCT_NAME & " already contains a one or more Categories, we cannot directly import the Categories from " & TargetName & vbCrLf ' TJS 24/08/12
        Me.lblSelectCatgoryMapping.Text += "Please select the appropriate " & IS_PRODUCT_NAME & " Category for each " & TargetName & " Category so that the import can use this mapping" & vbCrLf ' TJS 24/08/12
        Me.lblSelectCatgoryMapping.Text += "for the Inventory Items to be imported."
        Me.colTreeListCategoryName.Caption = TargetName & " Category"
        Me.lblMappingNotes.Text = "NOTE  If a " & TargetName & " Category does not have a mapped " & vbCrLf & "CB Category, then any imported products assigned to " & vbCrLf
        Me.lblMappingNotes.Text += "that Source Category WILL be imported, but they will" & vbCrLf & "NOT be assigned to any CB Product Category." & vbCrLf & vbCrLf
        Me.lblMappingNotes.Text += "Categories will have to be manually assigned once " & vbCrLf & "the import process is complete."

    End Sub
#End Region
#End Region

#Region " Events "
#Region " WizardPageChanged "
    Private Sub WizardPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles WizardControlImport.SelectedPageChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 29/03/11 | TJS             | 2011.0.04 | Modified to disable grid and select buttons during grid population
        '                                        | and to cater for options
        ' 01/04/11 | TJS             | 2011.0.06 | Corrected population of custom field list boxes
        ' 06/04/11 | TJS             | 2011.0.08 | Moved setting of GridControlSelectItems DataSource to GetProductListCompleted
        ' 09/04/11 | TJS             | 2011.0.09 | Modified to cater for category mapping
        ' 04/05/11 | TJS             | 2011.0.13 | Modified to ensure import occurs if Category mapping is skipped
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to cater for Inventory stock quantity publishing and for Channel Advisor
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to hide error message panel
        ' 05/07/12 | TJS             | 2012.1.08 | Modified for Amazon
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for Magento simple types
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to cater for Magento import Special Price options
        ' 21/06/13 | TJS             | 2013.1.21 | Modified to cater for Magento import Cost options
        ' 09/08/13 | TJS             | 2013.1.32 | Modified to cater for Inhibit multiple warnings checkbox
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to save category mapping details for re-use
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to use SetCategoryMappingLabels function and set Category Description on category mapping
        ' 06/02/14 | TJS             | 2013.4.08 | Added options to control further pricing and cost updates
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowCategoryMapping As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportCategoryMappingView_DEV000221Row ' TJS 19/09/13
        Dim Coll1 As DevExpress.XtraEditors.Controls.ComboBoxItemCollection ' TJS 29/03/11
        Dim Coll2 As DevExpress.XtraEditors.Controls.ComboBoxItemCollection ' TJS 29/03/11
        Dim Coll3 As DevExpress.XtraEditors.Controls.ComboBoxItemCollection ' TJS 29/03/11
        Dim Coll4 As DevExpress.XtraEditors.Controls.ComboBoxItemCollection ' TJS 29/03/11
        Dim Coll5 As DevExpress.XtraEditors.Controls.ComboBoxItemCollection ' TJS 29/03/11
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem ' TJS 29/03/11
        Dim colNewColumn As DataColumn ' TJS 09/04/11
        Dim strCustomFields As String()(), strSourceCode As String ' TJS 29/03/11 TJS 19/09/13
        Dim iLoop As Integer, bValidationErrors As Boolean ' TJS 29/03/11 TJS 19/09/13
        Dim strExistingReportRequestID As String, dteExistingRequestDate As Date ' TJS 05/07/12

        ' have we triggered this event by changing the focused page ?
        If Not Me.bIgnoreWizardPageChangeEvent Then
            ' no, which page is being displayed ?
            Cursor = Cursors.WaitCursor
            If ReferenceEquals(e.Page, Me.TabPageWelcome) Then
                ' Welcome Code page, must have clicked Back

            ElseIf ReferenceEquals(e.Page, Me.TabPageSelectItems) Then
                ' Select Items page, was last page the Welcome page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageWelcome) Then
                    ' yes, must have clicked Next
                    Me.WizardControlImport.Controls.Add(Me.chkInhibitWarnings) ' TJS 09/08/13
                    Me.chkInhibitWarnings.Top = Me.WizardControlImport.Controls(1).Top ' TJS 09/08/13
                    Me.chkInhibitWarnings.Visible = True ' TJS 09/08/13
                    If Me.cbeSiteOrAccount.SelectedIndex >= 0 Then ' TJS 02/12/11
                        If Me.cbeImportSource.EditValue.ToString = "Magento" Then ' TJS 09/03/13
                            Me.colISItemType.Width = 100 ' TJS 09/03/13
                            Me.colISItemType.VisibleIndex = 4 ' TJS 09/03/13
                            Me.colISItemType.Visible = True ' TJS 09/03/13
                        Else
                            Me.colISItemType.Width = 20 ' TJS 09/03/13
                            Me.colISItemType.VisibleIndex = -1 ' TJS 09/03/13
                            Me.colISItemType.Visible = False ' TJS 09/03/13
                        End If

                        m_ItemsForImport = New DataTable
                        m_GetProductListSuccess = False
                        Me.pnlGetListError.Visible = False ' TJS 14/02/12
                        Me.pnlGetListProgress.Visible = True
                        Me.lblGetListProgress.Text = "Getting Product list from " & Me.cbeImportSource.EditValue.ToString

                        m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                        m_BackgroundWorker.WorkerSupportsCancellation = True
                        m_BackgroundWorker.WorkerReportsProgress = False
                        If Me.cbeImportSource.EditValue.ToString = "Amazon" Then ' TJS 05/07/12
                            strExistingReportRequestID = "" ' TJS 05/07/12
                            tmr = New System.Threading.Timer(New System.Threading.TimerCallback(AddressOf timerCallback), ts, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite) ' TJS 05/07/12
                            If m_AmazonImportFacade.CheckForExistingProductListingReport(Me.cbeSiteOrAccount.EditValue.ToString, strExistingReportRequestID, dteExistingRequestDate) Then ' TJS 05/07/12
                                If Interprise.Presentation.Base.Message.MessageWindow.Show("A request for a Product Listing Report was sent to Amazon on " & _
                                    dteExistingRequestDate.ToShortDateString & " at " & dteExistingRequestDate.ToShortTimeString & " which has not been retrieved yet." & vbCrLf & vbCrLf & _
                                    "Click Yes if you want to retrieve and use this report for your Inventory import," & vbCrLf & "or click No to request a new updated Product Listing Report from Amazon.", _
                                    "Retrieve existing report", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, _
                                    Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button1) = DialogResult.No Then ' TJS 05/07/12
                                    AddHandler m_BackgroundWorker.DoWork, AddressOf RequestProductListingReport ' TJS 05/07/12
                                    AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf RequestProductListingReportCompleted ' TJS 05/07/12
                                    m_BackgroundWorker.RunWorkerAsync() ' TJS 05/07/12

                                Else
                                    ' as report already requested previo, we can make the first poll very quick (after 1 sec)
                                    tmr.Change(1000, 1000) ' TJS 05/07/12
                                    m_AmazonReportRequestID = strExistingReportRequestID
                                End If
                            Else
                                AddHandler m_BackgroundWorker.DoWork, AddressOf RequestProductListingReport ' TJS 05/07/12
                                AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf RequestProductListingReportCompleted ' TJS 05/07/12
                                m_BackgroundWorker.RunWorkerAsync() ' TJS 05/07/12
                            End If
                        Else
                            AddHandler m_BackgroundWorker.DoWork, AddressOf GetProductListToImport
                            AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf ProcessGetProductListCompleted ' TJS 05/07/12
                            m_BackgroundWorker.RunWorkerAsync()
                        End If
                        Me.colSourceType.ColumnEdit = Nothing
                        Me.WizardControlImport.EnableNextButton = False
                        Me.GridControlSelectItems.Enabled = False ' TJS 29/03/11
                        Me.btnSelectAll.Enabled = False ' TJS 29/03/11
                        Me.btnSelectNone.Enabled = False ' TJS 29/03/11
                    Else
                        bIgnoreWizardPageChangeEvent = True ' TJS 02/12/11
                        Me.cbeSiteOrAccount.ErrorText = "Must select Site or Account" ' TJS 02/12/11
                        Me.WizardControlImport.FocusPage = Me.TabPageWelcome ' TJS 02/12/11
                        bIgnoreWizardPageChangeEvent = False ' TJS 02/12/11

                    End If
                Else
                    ' no, must have clicked Back
                    Me.chkInhibitWarnings.Visible = True ' TJS 09/08/13

                End If

                ' start of code aadded TJS 29/03/11
            ElseIf ReferenceEquals(e.Page, Me.TabPageOptions) Then
                ' Import Options page, was last page the Select Items page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageSelectItems) Then
                    ' yes, must have clicked Next
                    Me.chkInhibitWarnings.Visible = False ' TJS 09/08/13
                    Select Case Me.cbeImportSource.EditValue.ToString
                        ' start of code added TJS 05/07/12
                        Case "Amazon"
                            Me.chkImportASPStorefrontExtData1.Visible = False
                            Me.chkImportASPStorefrontExtData2.Visible = False
                            Me.chkImportASPStorefrontExtData3.Visible = False
                            Me.chkImportASPStorefrontExtData4.Visible = False
                            Me.chkImportASPStorefrontExtData5.Visible = False
                            Me.cbeASPExtData1CustomField.Visible = False
                            Me.cbeASPExtData2CustomField.Visible = False
                            Me.cbeASPExtData3CustomField.Visible = False
                            Me.cbeASPExtData4CustomField.Visible = False
                            Me.cbeASPExtData5CustomField.Visible = False
                            Me.chkImportMagentoSellingPriceAsSuggestedRetail.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsRetail.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsWholesale.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSpecialPriceAsRetail.Visible = False ' TJS 13/03/13
                            Me.chkImportMagentoSpecialPriceAsWholesale.Visible = False ' TJS 13/03/13
                            Me.chkImportMagentoAverageCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoStandardCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoLastCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoCostAsPricingCost.Visible = False ' TJS 06/02/14
                            Me.lblCustomFields.Visible = False
                            Me.lblSourceOptions.Visible = False
                            Me.lblCategoriesExist.Visible = False
                            Me.chkCreateCategories.Visible = False

                            ' end of code added TJS 05/07/12

                        Case "ASPDotNetStorefront"
                            strCustomFields = m_ASPStorefrontImportFacade.GetRows(New String() {"ColumnName"}, "DataDictionaryColumn", "TableName = 'InventoryItem' AND IsCustomField = 1")
                            Coll1 = Me.cbeASPExtData1CustomField.Properties.Items
                            Coll2 = Me.cbeASPExtData2CustomField.Properties.Items ' TJS 01/04/11
                            Coll3 = Me.cbeASPExtData3CustomField.Properties.Items ' TJS 01/04/11
                            Coll4 = Me.cbeASPExtData4CustomField.Properties.Items ' TJS 01/04/11
                            Coll5 = Me.cbeASPExtData5CustomField.Properties.Items ' TJS 01/04/11
                            Coll1.BeginUpdate()
                            Coll1.Clear()
                            Coll2.BeginUpdate()
                            Coll2.Clear()
                            Coll3.BeginUpdate()
                            Coll3.Clear()
                            Coll4.BeginUpdate()
                            Coll4.Clear()
                            Coll5.BeginUpdate()
                            Coll5.Clear()
                            For Each CustomField As String() In strCustomFields
                                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(CustomField(0))
                                Coll1.Add(CollItem)
                                Coll2.Add(CollItem)
                                Coll3.Add(CollItem)
                                Coll4.Add(CollItem)
                                Coll5.Add(CollItem)
                            Next
                            Coll1.EndUpdate()
                            Coll2.EndUpdate()
                            Coll3.EndUpdate()
                            Coll4.EndUpdate()
                            Coll5.EndUpdate()

                            Me.chkImportASPStorefrontExtData1.Visible = True
                            Me.chkImportASPStorefrontExtData2.Visible = True
                            Me.chkImportASPStorefrontExtData3.Visible = True
                            Me.chkImportASPStorefrontExtData4.Visible = True
                            Me.chkImportASPStorefrontExtData5.Visible = True
                            Me.cbeASPExtData1CustomField.Visible = True
                            Me.cbeASPExtData2CustomField.Visible = True
                            Me.cbeASPExtData3CustomField.Visible = True
                            Me.cbeASPExtData4CustomField.Visible = True
                            Me.cbeASPExtData5CustomField.Visible = True
                            Me.chkImportMagentoSellingPriceAsSuggestedRetail.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsRetail.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsWholesale.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSpecialPriceAsRetail.Visible = False ' TJS 13/03/13
                            Me.chkImportMagentoSpecialPriceAsWholesale.Visible = False ' TJS 13/03/13
                            Me.chkImportMagentoAverageCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoStandardCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoLastCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoCostAsPricingCost.Visible = False ' TJS 06/02/14
                            Me.lblCustomFields.Visible = True
                            Me.lblSourceOptions.Visible = True ' TJS 04/05/11
                            Me.chkCreateCategories.Visible = True ' TJS 02/12/11

                            ' start of code added TJS 02/12/11
                        Case "Channel Advisor"
                            Me.chkImportASPStorefrontExtData1.Visible = False
                            Me.chkImportASPStorefrontExtData2.Visible = False
                            Me.chkImportASPStorefrontExtData3.Visible = False
                            Me.chkImportASPStorefrontExtData4.Visible = False
                            Me.chkImportASPStorefrontExtData5.Visible = False
                            Me.cbeASPExtData1CustomField.Visible = False
                            Me.cbeASPExtData2CustomField.Visible = False
                            Me.cbeASPExtData3CustomField.Visible = False
                            Me.cbeASPExtData4CustomField.Visible = False
                            Me.cbeASPExtData5CustomField.Visible = False
                            Me.chkImportMagentoSellingPriceAsSuggestedRetail.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsRetail.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsWholesale.Visible = False ' TJS 06/02/14
                            Me.chkImportMagentoSpecialPriceAsRetail.Visible = False ' TJS 13/03/13
                            Me.chkImportMagentoSpecialPriceAsWholesale.Visible = False ' TJS 13/03/13
                            Me.chkImportMagentoAverageCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoStandardCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoLastCost.Visible = False ' TJS 21/06/13
                            Me.chkImportMagentoCostAsPricingCost.Visible = False ' TJS 06/02/14
                            Me.lblCustomFields.Visible = False
                            Me.lblSourceOptions.Visible = False
                            Me.chkCreateCategories.Visible = False
                            ' end of code added TJS 02/12/11

                        Case "Magento"
                            Me.chkImportASPStorefrontExtData1.Visible = False
                            Me.chkImportASPStorefrontExtData2.Visible = False
                            Me.chkImportASPStorefrontExtData3.Visible = False
                            Me.chkImportASPStorefrontExtData4.Visible = False
                            Me.chkImportASPStorefrontExtData5.Visible = False
                            Me.cbeASPExtData1CustomField.Visible = False
                            Me.cbeASPExtData2CustomField.Visible = False
                            Me.cbeASPExtData3CustomField.Visible = False
                            Me.cbeASPExtData4CustomField.Visible = False
                            Me.cbeASPExtData5CustomField.Visible = False
                            Me.chkImportMagentoSellingPriceAsSuggestedRetail.Visible = True ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsRetail.Visible = True ' TJS 06/02/14
                            Me.chkImportMagentoSellingPriceAsWholesale.Visible = True ' TJS 06/02/14
                            Me.chkImportMagentoSpecialPriceAsRetail.Visible = True ' TJS 13/03/13
                            Me.chkImportMagentoSpecialPriceAsWholesale.Visible = True ' TJS 13/03/13
                            Me.chkImportMagentoAverageCost.Visible = True ' TJS 21/06/13
                            Me.chkImportMagentoStandardCost.Visible = True ' TJS 21/06/13
                            Me.chkImportMagentoLastCost.Visible = True ' TJS 21/06/13
                            Me.chkImportMagentoCostAsPricingCost.Visible = True ' TJS 06/02/14
                            Me.lblCustomFields.Visible = False
                            Me.lblSourceOptions.Visible = True ' TJS 04/05/11 TJS 13/03/13
                            Me.chkCreateCategories.Visible = True ' TJS 02/12/11

                    End Select
                Else
                    ' no, must have clicked Back

                End If
                ' end of code added TJS 29/03/11

                ' start of code aadded TJS 09/04/11
            ElseIf ReferenceEquals(e.Page, Me.TabPageCategoryMapping) Then
                ' Category Mapping page, was last page the Options page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageOptions) Then
                    ' yes, must have clicked Next
                    Me.chkInhibitWarnings.Visible = False ' TJS 09/08/13
                    ' start of code added TJS 02/12/11
                    bValidationErrors = False
                    If Me.RadioGroupQtyPublishing.SelectedIndex < 0 Then
                        bValidationErrors = True
                        Me.RadioGroupQtyPublishing.ErrorText = "Must not be blank"
                    Else
                        If Me.RadioGroupQtyPublishing.EditValue.ToString = "Fixed" Or Me.RadioGroupQtyPublishing.EditValue.ToString = "Percent" Then
                            If Me.TextEditQtyPublishingValue.EditValue Is Nothing OrElse Me.TextEditQtyPublishingValue.EditValue.ToString = "" Then
                                bValidationErrors = True
                                Me.TextEditQtyPublishingValue.ErrorText = "Must not be blank"

                            ElseIf Not IsNumeric(Me.TextEditQtyPublishingValue.EditValue) Then
                                bValidationErrors = True
                                Me.TextEditQtyPublishingValue.ErrorText = "Must be a number"

                            ElseIf Me.TextEditQtyPublishingValue.EditValue.ToString.IndexOf(",") > 0 Then
                                bValidationErrors = True
                                Me.TextEditQtyPublishingValue.ErrorText = "Must not contain ,"

                            ElseIf CDec(Me.TextEditQtyPublishingValue.EditValue) < 0 Then
                                bValidationErrors = True
                                Me.TextEditQtyPublishingValue.ErrorText = "Must not be negative"

                            ElseIf Me.RadioGroupQtyPublishing.EditValue.ToString = "Percent" And CDec(Me.TextEditQtyPublishingValue.EditValue) > 100 Then
                                bValidationErrors = True
                                Me.TextEditQtyPublishingValue.ErrorText = "Must not be greater then 100%"
                            End If
                        End If
                    End If
                    If bValidationErrors Then
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlImport.FocusPage = Me.TabPageOptions
                        bIgnoreWizardPageChangeEvent = False
                    Else
                        Select Case Me.cbeImportSource.EditValue.ToString
                            ' start of code added TJS 05/07/12
                            Case "Amazon"
                                m_AmazonImportFacade.QuantityPublishingType = Me.RadioGroupQtyPublishing.EditValue.ToString
                                If Me.RadioGroupQtyPublishing.EditValue.ToString = "Fixed" Or Me.RadioGroupQtyPublishing.EditValue.ToString = "Percent" Then
                                    m_AmazonImportFacade.QuantityPublishingValue = CDec(Me.TextEditQtyPublishingValue.EditValue)
                                Else
                                    m_AmazonImportFacade.QuantityPublishingValue = 0
                                End If
                                ' end of code added TJS 05/07/12

                            Case "ASPDotNetStorefront"
                                m_ASPStorefrontImportFacade.QuantityPublishingType = Me.RadioGroupQtyPublishing.EditValue.ToString
                                If Me.RadioGroupQtyPublishing.EditValue.ToString = "Fixed" Or Me.RadioGroupQtyPublishing.EditValue.ToString = "Percent" Then
                                    m_ASPStorefrontImportFacade.QuantityPublishingValue = CDec(Me.TextEditQtyPublishingValue.EditValue)
                                Else
                                    m_ASPStorefrontImportFacade.QuantityPublishingValue = 0
                                End If

                            Case "Channel Advisor"
                                m_ChanAdvImportFacade.QuantityPublishingType = Me.RadioGroupQtyPublishing.EditValue.ToString
                                If Me.RadioGroupQtyPublishing.EditValue.ToString = "Fixed" Or Me.RadioGroupQtyPublishing.EditValue.ToString = "Percent" Then
                                    m_ChanAdvImportFacade.QuantityPublishingValue = CDec(Me.TextEditQtyPublishingValue.EditValue)
                                Else
                                    m_ChanAdvImportFacade.QuantityPublishingValue = 0
                                End If

                            Case "Magento"
                                m_MagentoImportFacade.QuantityPublishingType = Me.RadioGroupQtyPublishing.EditValue.ToString
                                If Me.RadioGroupQtyPublishing.EditValue.ToString = "Fixed" Or Me.RadioGroupQtyPublishing.EditValue.ToString = "Percent" Then
                                    m_MagentoImportFacade.QuantityPublishingValue = CDec(Me.TextEditQtyPublishingValue.EditValue)
                                Else
                                    m_MagentoImportFacade.QuantityPublishingValue = 0
                                End If

                        End Select
                        ' end of code added TJS 02/12/11

                        ' have any categories already been created (checkbox disabled) ?
                        ' Amazon & Channel Advisor doesn't have categories so can't do mapping
                        If Me.chkCreateCategories.Enabled Or Me.cbeImportSource.EditValue.ToString = "Amazon" Or _
                            Me.cbeImportSource.EditValue.ToString = "Channel Advisor" Then ' TJS 02/12/11 TJS 05/07/12
                            ' no, skip category mapping setup (don't set ignore page change flag as we want import to be triggered
                            Me.WizardControlImport.FocusPage = Me.TabPageImporting

                        Else
                            Select Case Me.cbeImportSource.EditValue.ToString
                                Case "ASPDotNetStorefront"
                                    Me.pnlGetCategoryProgress.Text = "Getting ASPDotNetStorefront Categories" & vbCrLf & vbCrLf & "Please Wait"
                                    ReDim m_ASPStorefrontImportFacade.ASPStorefrontCategories(0)
                                    m_ASPStorefrontImportFacade.ASPStorefrontCategories(0).CategoryID = 0

                                Case "Magento"
                                    Me.pnlGetCategoryProgress.Text = "Getting Magento Categories" & vbCrLf & vbCrLf & "Please Wait"
                                    ReDim m_MagentoImportFacade.MagentoCategories(0)
                                    m_MagentoImportFacade.MagentoCategories(0).CategoryID = 0

                            End Select
                            SetCategoryMappingLabels(Me.cbeImportSource.EditValue.ToString) ' TJS 13/11/13
                            m_CategoryTable = New DataTable

                            colNewColumn = New DataColumn
                            colNewColumn.Caption = "Active"
                            colNewColumn.ColumnName = "Active"
                            colNewColumn.DataType = System.Type.GetType("System.Boolean")
                            m_CategoryTable.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            colNewColumn = New DataColumn
                            colNewColumn.Caption = "Source Category"
                            colNewColumn.ColumnName = "SourceCategoryName"
                            colNewColumn.DataType = System.Type.GetType("System.String")
                            m_CategoryTable.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            colNewColumn = New DataColumn
                            colNewColumn.Caption = "Source ID"
                            colNewColumn.ColumnName = "SourceCategoryID"
                            colNewColumn.DataType = System.Type.GetType("System.Decimal")
                            m_CategoryTable.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            colNewColumn = New DataColumn
                            colNewColumn.Caption = "Parent"
                            colNewColumn.ColumnName = "SourceParentID"
                            colNewColumn.DataType = System.Type.GetType("System.Decimal")
                            m_CategoryTable.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            colNewColumn = New DataColumn
                            colNewColumn.Caption = "IS Category Name"
                            colNewColumn.ColumnName = "ISCategoryName"
                            colNewColumn.DataType = System.Type.GetType("System.String")
                            m_CategoryTable.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            colNewColumn = New DataColumn
                            colNewColumn.Caption = "IS Category Code"
                            colNewColumn.ColumnName = "ISCategoryCode"
                            colNewColumn.DataType = System.Type.GetType("System.String")
                            m_CategoryTable.Columns.Add(colNewColumn)
                            colNewColumn.Dispose()

                            If Me.cbeImportSource.EditValue.ToString = "ASPDotNetStorefront" Then
                                colNewColumn = New DataColumn
                                colNewColumn.Caption = "GUID"
                                colNewColumn.ColumnName = "CategoryGUID"
                                colNewColumn.DataType = System.Type.GetType("System.String")
                                m_CategoryTable.Columns.Add(colNewColumn)
                                colNewColumn.Dispose()
                            End If

                            Me.pnlGetCategoryProgress.Visible = True
                            Me.rbeISCategoryCode.AdditionalFilter = "LanguageCode = '" & Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing) & "'"
                            AddHandler m_BackgroundWorker.DoWork, AddressOf GetSourceCategoriesForMapping
                            AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetSourceCategoriesCompleted
                            Me.WizardControlImport.EnableNextButton = False
                            m_BackgroundWorker.RunWorkerAsync()

                        End If
                    End If
                Else
                    ' no, must have clicked Back

                End If
                ' end of code aadded TJS 09/04/11

            ElseIf ReferenceEquals(e.Page, Me.TabPageImporting) Then
                ' Import progress page, was last page the Category Mapping or Options page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageOptions) Or ReferenceEquals(e.PrevPage, Me.TabPageCategoryMapping) Then
                    ' yes, must have clicked Next
                    ' start of code added TJS 19/09/13
                    If ReferenceEquals(e.PrevPage, Me.TabPageCategoryMapping) Then
                        Select Case Me.cbeImportSource.EditValue.ToString
                            Case "Magento"
                                strSourceCode = "MagentoOrder"
                            Case Else
                                strSourceCode = ""
                        End Select
                        If strSourceCode <> "" AndAlso m_CategoryTable IsNot Nothing AndAlso m_CategoryTable.Rows.Count > 0 Then ' TJS 13/11/13
                            m_ImportWizardsectionContainerFacade.LoadDataSet(New String()() {New String() {Me.ImportWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.TableName, _
                                "ReadLerrynImportExportCategoryMapping_DEV000221", AT_SOURCE_CODE, strSourceCode}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                            For iLoop = 0 To m_CategoryTable.Rows.Count - 1
                                If "" & m_CategoryTable.Rows(iLoop).Item("ISCategoryCode").ToString <> "" Then
                                    rowCategoryMapping = Me.ImportWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.FindBySourceCode_DEV000221InstanceAccountID_DEV000221SourceCategoryID_DEV000221SourceParentID_DEV000221(strSourceCode, Me.cbeSiteOrAccount.EditValue.ToString, m_CategoryTable.Rows(iLoop).Item("SourceCategoryID").ToString, m_CategoryTable.Rows(iLoop).Item("SourceParentID").ToString) ' TJS 13/11/13
                                    If rowCategoryMapping Is Nothing Then
                                        rowCategoryMapping = Me.ImportWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.NewLerrynImportExportCategoryMappingView_DEV000221Row
                                        rowCategoryMapping.SourceCode_DEV000221 = strSourceCode
                                        rowCategoryMapping.InstanceAccountID_DEV000221 = Me.cbeSiteOrAccount.EditValue.ToString ' TJS 13/11/13
                                        rowCategoryMapping.SourceCategoryID_DEV000221 = m_CategoryTable.Rows(iLoop).Item("SourceCategoryID").ToString
                                        rowCategoryMapping.SourceParentID_DEV000221 = m_CategoryTable.Rows(iLoop).Item("SourceParentID").ToString
                                        rowCategoryMapping.ISCategoryCode_DEV000221 = m_CategoryTable.Rows(iLoop).Item("ISCategoryCode").ToString
                                        Me.ImportWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.AddLerrynImportExportCategoryMappingView_DEV000221Row(rowCategoryMapping)

                                    ElseIf rowCategoryMapping.ISCategoryCode_DEV000221 <> m_CategoryTable.Rows(iLoop).Item("ISCategoryCode").ToString OrElse _
                                        rowCategoryMapping.IsDescriptionNull OrElse rowCategoryMapping.Description <> m_CategoryTable.Rows(iLoop).Item("ISCategoryName").ToString Then ' TJS 13/11/13
                                        rowCategoryMapping.ISCategoryCode_DEV000221 = m_CategoryTable.Rows(iLoop).Item("ISCategoryCode").ToString
                                        rowCategoryMapping.Description = m_CategoryTable.Rows(iLoop).Item("ISCategoryName").ToString ' TJS 13/11/13
                                    End If
                                End If
                            Next
                            m_ImportWizardsectionContainerFacade.UpdateDataSet(New String()() {New String() {Me.ImportWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.TableName, _
                                "CreateLerrynImportExportCategoryMapping_DEV000221", "UpdateLerrynImportExportCategoryMapping_DEV000221", "DeleteLerrynImportExportCategoryMapping_DEV000221"}}, _
                                Interprise.Framework.Base.Shared.TransactionType.None, "Update Source Category Mapping", False)
                        End If
                    End If
                    ' end of code added TJS 19/09/13

                    Me.chkInhibitWarnings.Visible = False ' TJS 09/08/13
                    ' start of code aadded TJS 29/03/11
                    bValidationErrors = False
                    Select Case Me.cbeImportSource.EditValue.ToString
                        Case "ASPDotNetStorefront"
                            If Me.chkImportASPStorefrontExtData1.Checked Then
                                If Me.cbeASPExtData1CustomField.Text <> "" Then
                                    If (Me.cbeASPExtData1CustomField.Text = Me.cbeASPExtData2CustomField.Text And Me.chkImportASPStorefrontExtData2.Checked) Or _
                                        (Me.cbeASPExtData1CustomField.Text = Me.cbeASPExtData3CustomField.Text And Me.chkImportASPStorefrontExtData3.Checked) Or _
                                        (Me.cbeASPExtData1CustomField.Text = Me.cbeASPExtData4CustomField.Text And Me.chkImportASPStorefrontExtData4.Checked) Or _
                                        (Me.cbeASPExtData1CustomField.Text = Me.cbeASPExtData5CustomField.Text And Me.chkImportASPStorefrontExtData5.Checked) Then
                                        bValidationErrors = True
                                        Me.cbeASPExtData1CustomField.ErrorText = "You cannot use the same Custom Field more than once"
                                    Else
                                        Me.cbeASPExtData1CustomField.ErrorText = ""
                                        m_ASPStorefrontImportFacade.ImportExtData1CustomField = Me.cbeASPExtData1CustomField.Text
                                    End If
                                Else
                                    bValidationErrors = True
                                    Me.cbeASPExtData1CustomField.ErrorText = "You must select a Custom Field for the data import"
                                End If
                            Else
                                m_ASPStorefrontImportFacade.ImportExtData1CustomField = ""
                            End If
                            If Me.chkImportASPStorefrontExtData2.Checked Then
                                If Me.cbeASPExtData2CustomField.Text <> "" Then
                                    If (Me.cbeASPExtData2CustomField.Text = Me.cbeASPExtData1CustomField.Text And Me.chkImportASPStorefrontExtData1.Checked) Or _
                                        (Me.cbeASPExtData2CustomField.Text = Me.cbeASPExtData3CustomField.Text And Me.chkImportASPStorefrontExtData3.Checked) Or _
                                        (Me.cbeASPExtData2CustomField.Text = Me.cbeASPExtData4CustomField.Text And Me.chkImportASPStorefrontExtData4.Checked) Or _
                                        (Me.cbeASPExtData2CustomField.Text = Me.cbeASPExtData5CustomField.Text And Me.chkImportASPStorefrontExtData5.Checked) Then
                                        bValidationErrors = True
                                        Me.cbeASPExtData2CustomField.ErrorText = "You cannot use the same Custom Field more than once"
                                    Else
                                        Me.cbeASPExtData2CustomField.ErrorText = ""
                                        m_ASPStorefrontImportFacade.ImportExtData2CustomField = Me.cbeASPExtData2CustomField.Text
                                    End If
                                Else
                                    bValidationErrors = True
                                    Me.cbeASPExtData2CustomField.ErrorText = "You must select a Custom Field for the data import"
                                End If
                            Else
                                m_ASPStorefrontImportFacade.ImportExtData2CustomField = ""
                            End If
                            If Me.chkImportASPStorefrontExtData3.Checked Then
                                If Me.cbeASPExtData3CustomField.Text <> "" Then
                                    If (Me.cbeASPExtData3CustomField.Text = Me.cbeASPExtData1CustomField.Text And Me.chkImportASPStorefrontExtData1.Checked) Or _
                                        (Me.cbeASPExtData3CustomField.Text = Me.cbeASPExtData2CustomField.Text And Me.chkImportASPStorefrontExtData2.Checked) Or _
                                        (Me.cbeASPExtData3CustomField.Text = Me.cbeASPExtData4CustomField.Text And Me.chkImportASPStorefrontExtData5.Checked) Or _
                                        (Me.cbeASPExtData3CustomField.Text = Me.cbeASPExtData5CustomField.Text And Me.chkImportASPStorefrontExtData4.Checked) Then
                                        bValidationErrors = True
                                        Me.cbeASPExtData3CustomField.ErrorText = "You cannot use the same Custom Field more than once"
                                    Else
                                        Me.cbeASPExtData3CustomField.ErrorText = ""
                                        m_ASPStorefrontImportFacade.ImportExtData3CustomField = Me.cbeASPExtData3CustomField.Text
                                    End If
                                Else
                                    bValidationErrors = True
                                    Me.cbeASPExtData3CustomField.ErrorText = "You must select a Custom Field for the data import"
                                End If
                            Else
                                m_ASPStorefrontImportFacade.ImportExtData3CustomField = ""
                            End If
                            If Me.chkImportASPStorefrontExtData4.Checked Then
                                If Me.cbeASPExtData4CustomField.Text <> "" Then
                                    If (Me.cbeASPExtData4CustomField.Text = Me.cbeASPExtData1CustomField.Text And Me.chkImportASPStorefrontExtData1.Checked) Or _
                                        (Me.cbeASPExtData4CustomField.Text = Me.cbeASPExtData2CustomField.Text And Me.chkImportASPStorefrontExtData2.Checked) Or _
                                        (Me.cbeASPExtData4CustomField.Text = Me.cbeASPExtData3CustomField.Text And Me.chkImportASPStorefrontExtData3.Checked) Or _
                                        (Me.cbeASPExtData4CustomField.Text = Me.cbeASPExtData5CustomField.Text And Me.chkImportASPStorefrontExtData5.Checked) Then
                                        bValidationErrors = True
                                        Me.cbeASPExtData4CustomField.ErrorText = "You cannot use the same Custom Field more than once"
                                    Else
                                        Me.cbeASPExtData4CustomField.ErrorText = ""
                                        m_ASPStorefrontImportFacade.ImportExtData4CustomField = Me.cbeASPExtData4CustomField.Text
                                    End If
                                Else
                                    bValidationErrors = True
                                    Me.cbeASPExtData4CustomField.ErrorText = "You must select a Custom Field for the data import"
                                End If
                            Else
                                m_ASPStorefrontImportFacade.ImportExtData4CustomField = ""
                            End If
                            If Me.chkImportASPStorefrontExtData5.Checked Then
                                If Me.cbeASPExtData5CustomField.Text <> "" Then
                                    If (Me.cbeASPExtData5CustomField.Text = Me.cbeASPExtData1CustomField.Text And Me.chkImportASPStorefrontExtData1.Checked) Or _
                                        (Me.cbeASPExtData5CustomField.Text = Me.cbeASPExtData2CustomField.Text And Me.chkImportASPStorefrontExtData2.Checked) Or _
                                        (Me.cbeASPExtData5CustomField.Text = Me.cbeASPExtData3CustomField.Text And Me.chkImportASPStorefrontExtData3.Checked) Or _
                                        (Me.cbeASPExtData5CustomField.Text = Me.cbeASPExtData4CustomField.Text And Me.chkImportASPStorefrontExtData4.Checked) Then
                                        bValidationErrors = True
                                        Me.cbeASPExtData5CustomField.ErrorText = "You cannot use the same Custom Field more than once"
                                    Else
                                        Me.cbeASPExtData5CustomField.ErrorText = ""
                                        m_ASPStorefrontImportFacade.ImportExtData5CustomField = Me.cbeASPExtData5CustomField.Text
                                    End If
                                Else
                                    bValidationErrors = True
                                    Me.cbeASPExtData5CustomField.ErrorText = "You must select a Custom Field for the data import"
                                End If
                            Else
                                m_ASPStorefrontImportFacade.ImportExtData5CustomField = ""
                            End If

                        Case "Magento" ' TJS 13/03/13
                            m_MagentoImportFacade.ImportSellingPriceAsSuggestedRetail = Me.chkImportMagentoSellingPriceAsSuggestedRetail.Checked ' TJS 06/02/14
                            m_MagentoImportFacade.ImportSellingPriceAsRetail = Me.chkImportMagentoSellingPriceAsRetail.Checked ' TJS 06/02/14
                            m_MagentoImportFacade.ImportSellingPriceAsWholesale = Me.chkImportMagentoSellingPriceAsWholesale.Checked ' TJS 06/02/14
                            m_MagentoImportFacade.ImportSpecialPriceAsRetail = Me.chkImportMagentoSpecialPriceAsRetail.Checked ' TJS 13/03/13
                            m_MagentoImportFacade.ImportSpecialPriceAsWholesale = Me.chkImportMagentoSpecialPriceAsWholesale.Checked ' TJS 13/03/13
                            m_MagentoImportFacade.ImportCostAsAverage = Me.chkImportMagentoAverageCost.Checked ' TJS 21/06/13
                            m_MagentoImportFacade.ImportCostAsStandard = Me.chkImportMagentoStandardCost.Checked ' TJS 21/06/13
                            m_MagentoImportFacade.ImportCostAsLast = Me.chkImportMagentoLastCost.Checked ' TJS 21/06/13
                            m_MagentoImportFacade.ImportCostAsPricingCost = Me.chkImportMagentoCostAsPricingCost.Checked ' TJS 06/02/14

                    End Select
                    If bValidationErrors Then
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlImport.FocusPage = Me.TabPageOptions
                        bIgnoreWizardPageChangeEvent = False
                    Else
                        ' end of code aadded TJS 29/03/11
                        m_NoOfProductsImported = 0
                        m_NoOfProductsSkipped = 0
                        Me.pnlImportProgress.Visible = True
                        Me.lblImportProgress.Text = "Importing Products from " & Me.cbeImportSource.EditValue.ToString
                        Select Case Me.cbeImportSource.EditValue.ToString ' TJS 29/03/11
                            Case "ASPDotNetStorefront" ' TJS 29/03/11
                                m_ASPStorefrontImportFacade.CreateISCategories = Me.chkCreateCategories.Checked ' TJS 29/03/11

                            Case "Magento" ' TJS 29/03/11
                                m_MagentoImportFacade.CreateISCategories = Me.chkCreateCategories.Checked ' TJS 29/03/11

                        End Select

                        AddHandler m_BackgroundWorker.DoWork, AddressOf ImportSelectedProducts
                        AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf ProductImportCompleted
                        Me.WizardControlImport.EnableNextButton = False
                        Me.GridControlSelectItems.Enabled = False ' TJS 29/03/11
                        Me.btnSelectAll.Enabled = False ' TJS 29/03/11
                        Me.btnSelectNone.Enabled = False ' TJS 29/03/11
                        m_BackgroundWorker.RunWorkerAsync()
                    End If

                Else
                    ' no, must have clicked Back - display Select Items page 
                    Me.WizardControlImport.SelectedTabPage = Me.TabPageSelectItems
                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageComplete) Then
                ' Finish page, was last page the Import progress page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageImporting) Then
                    ' yes, must have clicked Next 
                    Me.chkInhibitWarnings.Visible = False ' TJS 09/08/13
                    Select Case Me.cbeImportSource.EditValue.ToString
                        ' start of code added TJS 05/07/12
                        Case "Amazon"
                            If m_AmazonImportFacade.ImportLog <> "" Then
                                Me.CheckEditViewImportLog.Visible = True
                                Me.MemoEditImportLog.Text = m_AmazonImportFacade.ImportLog
                            Else
                                Me.CheckEditViewImportLog.Visible = False
                                Me.MemoEditImportLog.Text = ""
                            End If
                            ' end of code added TJS 05/07/12

                        Case "ASPDotNetStorefront"
                            If m_ASPStorefrontImportFacade.ImportLog <> "" Then
                                Me.CheckEditViewImportLog.Visible = True
                                Me.MemoEditImportLog.Text = m_ASPStorefrontImportFacade.ImportLog
                            Else
                                Me.CheckEditViewImportLog.Visible = False
                                Me.MemoEditImportLog.Text = ""
                            End If

                            ' start of code added TJS 02/12/11
                        Case "Channel Advisor"
                            If m_ChanAdvImportFacade.ImportLog <> "" Then
                                Me.CheckEditViewImportLog.Visible = True
                                Me.MemoEditImportLog.Text = m_ChanAdvImportFacade.ImportLog
                            Else
                                Me.CheckEditViewImportLog.Visible = False
                                Me.MemoEditImportLog.Text = ""
                            End If

                        Case "Magento"
                            If m_MagentoImportFacade.ProcessLog <> "" Then
                                Me.CheckEditViewImportLog.Visible = True
                                Me.MemoEditImportLog.Text = m_MagentoImportFacade.ProcessLog
                            Else
                                Me.CheckEditViewImportLog.Visible = False
                                Me.MemoEditImportLog.Text = ""
                            End If
                            ' end of code added TJS 02/12/11
                    End Select
                End If
            End If
            Cursor = Cursors.Default
        End If

    End Sub
#End Region

#Region " SourceSelected "
    Private Sub SourceSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbeImportSource.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 22/03/11 | TJS             | 2011.0.03 | Modified to cater for all accounts/Instances being disabled
        ' 04/04/11 | TJS             | 2011.0.07 | Modified to cater for Inventory Import only activation
        ' 05/04/11 | TJS             | 2011.0.08 | Modified to cater for IS 4.8 build using conditional compile
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2, 
        '                                        | to cater for the Quantity Publishing options and for Channel Advisor
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to set Item Qty Publishing default for ASPDotNetStorefront
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 05/07/12 | TJS             | 2012.1.08 | Added Amazon
        ' 09/03/13 | TJS             | 2013.1.01 | Modified to allow selection of IS Item Type for non-stock items
        ' 22/03/13 | TJS             | 2013.1.05 | Modified to cater renaming of Amazon MerchantID as MerchantToken
        ' 09/08/13 | TJS             | 2013.1.32 | Modified to sater for Source ID changed as well as SKU on colour key chart
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to store Import Source in module variable to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

        m_ImportSource = Me.cbeImportSource.EditValue.ToString ' TJS 09/12/13
        Select Case Me.cbeImportSource.EditValue.ToString
            ' start of code added TJS 05/07/12
            Case "Amazon"
                m_AmazonImportFacade = New Lerryn.Facade.ImportExport.AmazonImportFacade(Me.ImportWizardSectionContainerGateway, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

                Me.LoadDataSet(New String()() {New String() {Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, AMAZON_SOURCE_CODE}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                If Me.m_ImportWizardsectionContainerFacade.IsActivated Then
                    XMLConfig = XDocument.Parse(Trim(Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    Me.cbeSiteOrAccount.Visible = True
                    Me.lblSiteOrAccount.Visible = True
                    Me.lblMultipleSiteOrAccount.Visible = True
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Me.colImportAsKit.Caption = "Not Used"
                    Me.colImportAsKit.Width = 20
                    Me.colImportAsKit.Visible = False
                    Me.colImportAsKit.VisibleIndex = -1
                    Me.colISItemType.Caption = "Not Used" ' TJS 09/03/13
                    Me.colISItemType.Width = 20 ' TJS 09/03/13
                    Me.colISItemType.Visible = False ' TJS 09/03/13
                    Me.colISItemType.VisibleIndex = -1 ' TJS 09/03/13
                    Me.colSelect.Width = 45
                    Me.colItemName.Width = 330
                    Me.colItemSKU.Width = 281
                    Me.colSourceType.Width = 131
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_AMAZON_MERCHANT_LIST)
                    Coll = Me.cbeSiteOrAccount.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_ACCOUNT_DISABLED).ToUpper <> "YES" And _
                            Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN) <> "" Then ' TJS 22/03/13
                            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_NAME), Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_AMAZON_MERCHANT_TOKEN)) ' TJS 22/03/13
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    ' check Instance count in case some Instances were disabled 
                    If Coll.Count = 0 Then
                        ' no active sources
                        Me.cbeSiteOrAccount.Visible = False
                        Me.lblSiteOrAccount.Visible = False
                        Me.lblMultipleSiteOrAccount.Visible = False
                        Me.WizardControlImport.buttonNext.Enabled = True
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Your Amazon Connector sources are all disabled.  Please check and set the AccountDisabled Config Setting to No on at least 1 Amazon Config Group.")
                    ElseIf Coll.Count = 1 Then
                        ' only have 1 active source so select it
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLConfig, "eShopCONNECTConfig/" & SOURCE_CONFIG_XML_AMAZON_IS_ITEM_ID_FIELD) = "ItemName" Then
                            Me.cbeSiteOrAccount.SelectedIndex = 0
                            Me.cbeSiteOrAccount.Visible = False
                            Me.lblSiteOrAccount.Visible = False
                            Me.lblMultipleSiteOrAccount.Visible = False
                            Me.WizardControlImport.buttonNext.Enabled = True
                        Else
                            Interprise.Presentation.Base.Message.MessageWindow.Show("The Inventory Import Wizard can only be used when the ISItemIDField setting in the Amazon Config settings is set to ItemName")
                        End If
                    End If
                    Me.lblQtyPublishing.Visible = True
                    Me.lblQtyPublishingValue.Text = ""
                    Me.lblQtyPublishingValue.Visible = True
                    Me.RadioGroupQtyPublishing.Visible = True
                    Me.TextEditQtyPublishingValue.Visible = True
                    Me.lblGridColorKey2.Text = "= Item with this SKU exists - importing will add Amazon properties"
                    Me.lblGridColorKey3.Text = "= SKU/Source ID has changed - importing will update existing Item" ' TJS 09/08/13

                Else
                    Me.cbeSiteOrAccount.Visible = False
                    Me.lblSiteOrAccount.Visible = False
                    Me.lblMultipleSiteOrAccount.Visible = False
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Source not activated")
                End If
                ' end of code added TJS 05/07/12

            Case "ASPDotNetStorefront"
                m_ASPStorefrontImportFacade = New Lerryn.Facade.ImportExport.ASPStorefrontImportFacade(Me.ImportWizardSectionContainerGateway, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

                Me.LoadDataSet(New String()() {New String() {Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, ASP_STORE_FRONT_SOURCE_CODE}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                If Me.m_ImportWizardsectionContainerFacade.IsActivated Then ' TJS 04/04/11
                    XMLConfig = XDocument.Parse(Trim(Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    Me.cbeSiteOrAccount.Visible = True
                    Me.lblSiteOrAccount.Visible = True
                    Me.lblMultipleSiteOrAccount.Visible = True
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Me.colImportAsKit.Caption = "Not Used" ' TJS 02/12/11
                    Me.colImportAsKit.Width = 20 ' TJS 02/12/11
                    Me.colImportAsKit.Visible = False ' TJS 02/12/11
                    Me.colImportAsKit.VisibleIndex = -1 ' TJS 02/12/11
                    Me.colISItemType.Caption = "Not Used" ' TJS 09/03/13
                    Me.colISItemType.Width = 20 ' TJS 09/03/13
                    Me.colISItemType.Visible = False ' TJS 09/03/13
                    Me.colISItemType.VisibleIndex = -1 ' TJS 09/03/13
                    Me.colSelect.Width = 45 ' TJS 02/12/11
                    Me.colItemName.Width = 330 ' TJS 02/12/11
                    Me.colItemSKU.Width = 281 ' TJS 02/12/11
                    Me.colSourceType.Width = 131 ' TJS 02/12/11
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_ASP_STORE_FRONT_LIST)
                    Coll = Me.cbeSiteOrAccount.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_ACCOUNT_DISABLED).ToUpper <> "YES" And _
                            Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID) <> "" Then ' TJS 02/12/11
                            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID), Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_ASP_STORE_FRONT_SITE_ID))
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    ' check Instance count in case some Instances were disabled 
                    If Coll.Count = 0 Then ' TJS 22/03/11
                        ' no active sources
                        Me.cbeSiteOrAccount.Visible = False ' TJS 22/03/11
                        Me.lblSiteOrAccount.Visible = False ' TJS 22/03/11
                        Me.lblMultipleSiteOrAccount.Visible = False ' TJS 22/03/11
                        Me.WizardControlImport.buttonNext.Enabled = True ' TJS 22/03/11
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Your ASPDotNetStorefront Connector sources are all disabled.  Please check and set the AccountDisabled Config Setting to No on at least 1 ASPStorefront Config Group.") ' TJS 22/03/11
                    ElseIf Coll.Count = 1 Then ' TJS 22/03/11
                        ' only have 1 active source so select it
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLConfig, "eShopCONNECTConfig/" & SOURCE_CONFIG_XML_ASP_STORE_FRONT_IS_ITEM_ID_FIELD) = "ItemName" Then
                            Me.cbeSiteOrAccount.SelectedIndex = 0
                            Me.cbeSiteOrAccount.Visible = False
                            Me.lblSiteOrAccount.Visible = False
                            Me.lblMultipleSiteOrAccount.Visible = False
                            Me.WizardControlImport.buttonNext.Enabled = True
                        Else
                            Interprise.Presentation.Base.Message.MessageWindow.Show("The Inventory Import Wizard can only be used when the ISItemIDField setting in the ASPDotNetStoreFront Config settings is set to ItemName")
                        End If
                    End If
                    Me.lblQtyPublishing.Visible = False ' TJS 02/12/11
                    Me.lblQtyPublishingValue.Text = "" ' TJS 02/12/11
                    Me.lblQtyPublishingValue.Visible = False ' TJS 02/12/11
                    Me.RadioGroupQtyPublishing.Visible = False ' TJS 02/12/11
                    Me.RadioGroupQtyPublishing.SelectedIndex = 0 ' TJS 14/02/12
                    Me.TextEditQtyPublishingValue.Visible = False ' TJS 02/12/11
                    Me.lblGridColorKey2.Text = "= Item with this SKU exists - importing will add ASPDNSF properties" ' TJS 05/07/12
                    Me.lblGridColorKey3.Text = "= SKU/Source ID has changed - importing will update existing Item" ' ' TJS 02/12/11 TJS 09/08/13

                Else
                    Me.cbeSiteOrAccount.Visible = False
                    Me.lblSiteOrAccount.Visible = False
                    Me.lblMultipleSiteOrAccount.Visible = False
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Source not activated")

                End If

                ' start of code added TJS 02/12/11
            Case "Channel Advisor"
                m_ChanAdvImportFacade = New Lerryn.Facade.ImportExport.ChannelAdvImportFacade(Me.ImportWizardSectionContainerGateway, PRODUCT_CODE, PRODUCT_NAME)

                Me.LoadDataSet(New String()() {New String() {Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, CHANNEL_ADVISOR_SOURCE_CODE}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                If Me.m_ImportWizardsectionContainerFacade.IsActivated Then
                    XMLConfig = XDocument.Parse(Trim(Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    Me.cbeSiteOrAccount.Visible = True
                    Me.lblSiteOrAccount.Visible = True
                    Me.lblMultipleSiteOrAccount.Visible = True
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Me.colImportAsKit.Width = 80 ' TJS 02/12/11
                    Me.colImportAsKit.Caption = "Import as Kit" ' TJS 02/12/11
                    Me.colImportAsKit.VisibleIndex = 4 ' TJS 02/12/11
                    Me.colImportAsKit.Visible = True ' TJS 02/12/11
                    Me.colISItemType.Caption = "Not Used" ' TJS 09/03/13
                    Me.colISItemType.Width = 20 ' TJS 09/03/13
                    Me.colISItemType.Visible = False ' TJS 09/03/13
                    Me.colISItemType.VisibleIndex = -1 ' TJS 09/03/13
                    Me.colSelect.Width = 45 ' TJS 02/12/11
                    Me.colSelect.VisibleIndex = 0 ' TJS 02/12/11
                    Me.colSelect.Visible = True ' TJS 02/12/11
                    Me.colItemName.Width = 320 ' TJS 02/12/11
                    Me.colItemName.VisibleIndex = 1 ' TJS 02/12/11
                    Me.colItemName.Visible = True ' TJS 02/12/11
                    Me.colItemSKU.Width = 280 ' TJS 02/12/11
                    Me.colItemSKU.VisibleIndex = 2 ' TJS 02/12/11
                    Me.colItemSKU.Visible = True ' TJS 02/12/11
                    Me.colSourceType.Width = 100 ' TJS 02/12/11
                    Me.colSourceType.VisibleIndex = 3 ' TJS 02/12/11
                    Me.colSourceType.Visible = True ' TJS 02/12/11
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_CHANNEL_ADV_LIST)
                    Coll = Me.cbeSiteOrAccount.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_DISABLED).ToUpper <> "YES" And _
                            Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME) <> "" And _
                            Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID) <> "" Then
                            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_NAME), Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_CHANNEL_ADV_ACCOUNT_ID))
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    ' check Instance count in case some Instances were disabled 
                    If Coll.Count = 0 Then
                        ' no active sources
                        Me.cbeSiteOrAccount.Visible = False
                        Me.lblSiteOrAccount.Visible = False
                        Me.lblMultipleSiteOrAccount.Visible = False
                        Me.WizardControlImport.buttonNext.Enabled = True
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Your Channel Advisor Connector sources are all disabled.  Please check and set the AccountDisabled Config Setting to No on at least 1 Channel Advisor Config Group.")
                    ElseIf Coll.Count = 1 Then
                        ' only have 1 active source so select it
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLConfig, "eShopCONNECTConfig/" & SOURCE_CONFIG_XML_CHANNEL_ADV_IS_ITEM_ID_FIELD) = "ItemName" Then
                            Me.cbeSiteOrAccount.SelectedIndex = 0
                            Me.cbeSiteOrAccount.Visible = False
                            Me.lblSiteOrAccount.Visible = False
                            Me.lblMultipleSiteOrAccount.Visible = False
                            Me.WizardControlImport.buttonNext.Enabled = True
                        Else
                            Interprise.Presentation.Base.Message.MessageWindow.Show("The Inventory Import Wizard can only be used when the ISItemIDField setting in the Channel Advisor Config settings is set to ItemName")
                        End If
                    End If
                    Me.lblQtyPublishing.Visible = True
                    Me.lblQtyPublishingValue.Text = ""
                    Me.lblQtyPublishingValue.Visible = True
                    Me.RadioGroupQtyPublishing.Visible = True
                    Me.TextEditQtyPublishingValue.Visible = True
                    Me.lblGridColorKey2.Text = "= Item with this SKU exists - importing will add ChanAdv properties" ' TJS 05/07/12
                    Me.lblGridColorKey3.Text = "= ASIN already exists on a different SKU - importing will create a new Item"

                Else
                    Me.cbeSiteOrAccount.Visible = False
                    Me.lblSiteOrAccount.Visible = False
                    Me.lblMultipleSiteOrAccount.Visible = False
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Source not activated")

                End If
                ' end of code added TJS 02/12/11

            Case "Magento"
                m_MagentoImportFacade = New Lerryn.Facade.ImportExport.MagentoImportFacade(Me.ImportWizardSectionContainerGateway, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 10/06/12

                Me.LoadDataSet(New String()() {New String() {Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, "MagentoOrder"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                If Me.m_ImportWizardsectionContainerFacade.IsActivated Then ' TJS 04/04/11
                    XMLConfig = XDocument.Parse(Trim(Me.ImportWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    Me.cbeSiteOrAccount.Visible = True
                    Me.lblSiteOrAccount.Visible = True
                    Me.lblMultipleSiteOrAccount.Visible = True
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Me.colImportAsKit.Caption = "Not Used" ' TJS 02/12/11
                    Me.colImportAsKit.Width = 20 ' TJS 02/12/11
                    Me.colImportAsKit.Visible = False ' TJS 02/12/11
                    Me.colImportAsKit.VisibleIndex = -1 ' TJS 02/12/11
                    Me.colSelect.Width = 45 ' TJS 02/12/11
                    Me.colItemName.Width = 330 ' TJS 02/12/11
                    Me.colItemSKU.Width = 281 ' TJS 02/12/11
                    Me.colSourceType.Width = 131 ' TJS 02/12/11
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                    Coll = Me.cbeSiteOrAccount.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ACCOUNT_DISABLED).ToUpper <> "YES" And _
                            Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) <> "" Then ' TJS 02/12/11
                            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID), Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID))
                            Coll.Add(CollItem)
                        End If
                    Next
                    Coll.EndUpdate()
                    ' check Instance count in case some Instances were disabled 
                    If Coll.Count = 0 Then ' TJS 22/03/11
                        ' no active sources
                        Me.cbeSiteOrAccount.Visible = False ' TJS 22/03/11
                        Me.lblSiteOrAccount.Visible = False ' TJS 22/03/11
                        Me.lblMultipleSiteOrAccount.Visible = False ' TJS 22/03/11
                        Me.WizardControlImport.buttonNext.Enabled = True ' TJS 22/03/11
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Your Magento Connector sources are all disabled.  Please check and set the AccountDisabled Config Setting to No on at least 1 Magento Config Group.") ' TJS 22/03/11

                    ElseIf Coll.Count = 1 Then ' TJS 22/03/11
                        ' only have 1 active source so select it
                        If Me.m_ImportWizardsectionContainerFacade.GetXMLElementText(XMLConfig, "eShopCONNECTConfig/" & SOURCE_CONFIG_XML_MAGENTO_IS_ITEM_ID_FIELD) = "ItemName" Then
                            Me.cbeSiteOrAccount.SelectedIndex = 0
                            Me.cbeSiteOrAccount.Visible = False
                            Me.lblSiteOrAccount.Visible = False
                            Me.lblMultipleSiteOrAccount.Visible = False
                            Me.WizardControlImport.buttonNext.Enabled = True
                        Else
                            Interprise.Presentation.Base.Message.MessageWindow.Show("The Inventory Import Wizard can only be used when the ISItemIDField setting in the Magento Config settings is set to ItemName")
                        End If
                    End If
                    Me.lblQtyPublishing.Visible = True ' TJS 02/12/11
                    Me.lblQtyPublishingValue.Text = "" ' TJS 02/12/11
                    Me.lblQtyPublishingValue.Visible = True ' TJS 02/12/11
                    Me.RadioGroupQtyPublishing.Visible = True ' TJS 02/12/11
                    Me.TextEditQtyPublishingValue.Visible = True ' TJS 02/12/11
                    Me.lblGridColorKey2.Text = "= Item with this SKU exists - importing will add Magento properties" ' TJS 05/07/12
                    Me.lblGridColorKey3.Text = "= SKU/Source ID has changed - importing will update existing Item" ' ' TJS 02/12/11 TJS 09/08/13

                Else
                    Me.cbeSiteOrAccount.Visible = False
                    Me.lblSiteOrAccount.Visible = False
                    Me.lblMultipleSiteOrAccount.Visible = False
                    Me.WizardControlImport.buttonNext.Enabled = False
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Source not activated")

                End If

            Case Else
                Me.cbeSiteOrAccount.Visible = False
                Me.lblSiteOrAccount.Visible = False
                Me.lblMultipleSiteOrAccount.Visible = False
                Me.WizardControlImport.buttonNext.Enabled = False
                Interprise.Presentation.Base.Message.MessageWindow.Show("Unknown Source")

        End Select

    End Sub
#End Region

#Region " SiteOrAccountSelected "
    Private Sub SiteOrAccountSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbeSiteOrAccount.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/04/11 | TJS             | 2011.0.08 | Modified to cater for IS 4.8 build using conditional compile
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to store Site or Account in module variable to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        m_SiteOrAccount = Me.cbeSiteOrAccount.EditValue.ToString ' TJS 09/12/13
        m_SiteOrAccountID = Me.cbeSiteOrAccount.Properties.Items(Me.cbeSiteOrAccount.SelectedIndex).Value.ToString ' TJS 09/12/13
        If Me.cbeSiteOrAccount.EditValue.ToString <> "" Then
            Me.WizardControlImport.buttonNext.Enabled = True
        Else
            Me.WizardControlImport.buttonNext.Enabled = False
        End If

    End Sub
#End Region

#Region " SelectItemsGridFunctions "
    Private Sub GridViewSelectItems_CellValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridViewSelectItems.CellValueChanging
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 31/03/11 | TJS             | 2011.0.05 | Function added
        ' 04/11/11 | TJS             | 2011.1.10 | modified for SKU CHanged 
        ' 02/12/11 | TJS             | 2011.2.00 | Modified for Channel ADvisor
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 13/03/13 | TJS             | 2013.1.02 | Modified to ensure Import checkbox is unchecked when cancelling import
        ' 09/08/13 | TJS             | 2013.1.32 | Modified to cater for Inhibit multiple warnings checkbox
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strMessage As String, strTitle As String, iloop As Integer, bAskGetConfirmation As Boolean ' TJS 04/11/11 TJS 09/08/13

        If e.Column.FieldName = "Import" Then
            If CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SKUError")) And CBool(e.Value) Then
                If Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "ItemSKU").ToString = "" Then
                    Me.GridViewSelectItems.SetRowCellValue(e.RowHandle, "Import", False)

                ElseIf CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SKUChanged")) Then ' TJS 04/11/11
                    If Not m_ImportErrorUpdateConfirmed Or Not Me.chkInhibitWarnings.Checked Then ' TJS 09/08/13
                        If Me.cbeImportSource.EditValue.ToString = "Channel Advisor" Then ' TJS 02/12/11
                            strMessage = "Another Item with this ASIN already exists." & vbCrLf & vbCrLf & "If you set the Import checkbox for this product, it will be imported into" & vbCrLf & IS_PRODUCT_NAME & " and you will have 2 Items with the same Amazon ASIN." ' TJS 02/12/11 TJS 24/08/12
                            strTitle = "Duplicate ASIN" ' TJS 02/12/11
                        Else
                            strMessage = "The SKU for this product has been changed in either the Source" & vbCrLf & "or " & IS_PRODUCT_NAME & " since it was imported." & vbCrLf & vbCrLf & "If you set the Import checkbox for this product, the SKU" & vbCrLf & "in " & IS_PRODUCT_NAME & " will be updated to that from the Source." ' TJS 02/12/11 TJS 24/08/12
                            strTitle = "SKU Changed" ' TJS 02/12/11
                        End If
                        If Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage, strTitle, Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.No Then ' TJS 04/11/11
                            ' hiding the editor apparently undoes any changes
                            Me.GridViewSelectItems.HideEditor() ' TJS 13/03/13
                            Me.GridViewSelectItems.SetRowCellValue(e.RowHandle, "Import", False) ' TJS 04/11/11
                        Else
                            m_ImportErrorUpdateConfirmed = True ' TJS 09/08/13
                        End If
                    End If

                Else
                    For iloop = 0 To Me.GridViewSelectItems.RowCount - 1
                        If Me.GridViewSelectItems.GetRowCellValue(iloop, "ItemSKU").ToString = Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "ItemSKU").ToString And iloop <> e.RowHandle Then
                            ' hiding the editor apparently undoes any changes
                            Me.GridViewSelectItems.HideEditor() ' TJS 13/03/13
                            Me.GridViewSelectItems.SetRowCellValue(e.RowHandle, "Import", False)
                            Me.GridViewSelectItems.SetRowCellValue(iloop, "Import", False)
                            Interprise.Presentation.Base.Message.MessageWindow.Show("Cannot import products with duplicate SKUs")
                        End If
                    Next
                End If

            ElseIf CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "AlreadyImported")) And CBool(e.Value) Then ' TJS 10/06/12
                ' start of code added TJS 09/08/13
                If CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SKUChanged")) Then
                    bAskGetConfirmation = Not m_ImportSKUChangeUpdateConfirmed Or Not Me.chkInhibitWarnings.Checked
                    strMessage = "The SKU for this item has changed in "
                    Select Case Me.cbeImportSource.EditValue.ToString
                        Case "Amazon"
                            strMessage = strMessage & "Amazon" & vbCrLf & vbCrLf
                        Case "ASPDotNetStorefront"
                            strMessage = strMessage & "ASPDotNetStorefront" & vbCrLf & vbCrLf
                        Case "Channel Advisor"
                            strMessage = strMessage & "Channel Advisor" & vbCrLf & vbCrLf
                        Case "Magento"
                            strMessage = strMessage & "Magento" & vbCrLf & vbCrLf
                    End Select

                ElseIf CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SourceIDChanged")) Then
                    bAskGetConfirmation = Not m_ImportSourceIDUpdateConfirmed Or Not Me.chkInhibitWarnings.Checked
                    Select Case Me.cbeImportSource.EditValue.ToString
                        Case "Amazon"

                        Case "ASPDotNetStorefront"
                            strMessage = "The ASPDotNetStorefront Product ID for this item has changed" & vbCrLf & vbCrLf
                        Case "Magento"
                            strMessage = "The Magento Product ID for this item has changed" & vbCrLf & vbCrLf
                        Case Else
                            strMessage = "The source Product ID for this item has changed" & vbCrLf & vbCrLf
                    End Select

                Else
                    bAskGetConfirmation = Not m_ImportUpdateExistingItemConfirmed Or Not Me.chkInhibitWarnings.Checked
                    strMessage = ""
                End If
                ' end of code added TJS 09/08/13
                strMessage = strMessage & "Are you sure you want to import this item again and overwrite the" & vbCrLf & "settings in " & IS_PRODUCT_NAME & " with the latest values from " ' TJS 10/06/12 TJS 24/08/12 TJS 09/08/13
                Select Case Me.cbeImportSource.EditValue.ToString ' TJS 24/08/12
                    Case "Amazon" ' TJS 24/08/12
                        strMessage = strMessage & "Amazon ?" ' TJS 24/08/12
                    Case "ASPDotNetStorefront" ' TJS 24/08/12
                        strMessage = strMessage & "ASPDotNetStorefront ?" ' TJS 24/08/12
                    Case "Channel Advisor" ' TJS 24/08/12
                        strMessage = strMessage & "Channel Advisor ?" ' TJS 24/08/12
                    Case "Magento" ' TJS 24/08/12
                        strMessage = strMessage & "Magento ?" ' TJS 24/08/12
                End Select
                If bAskGetConfirmation Then ' TJS 09/08/13
                    If Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage, "Import and Overwrite Values", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.No Then ' TJS 10/06/12
                        ' hiding the editor apparently undoes any changes
                        Me.GridViewSelectItems.HideEditor() ' TJS 13/03/13
                        Me.GridViewSelectItems.SetRowCellValue(e.RowHandle, "Import", False) ' TJS 10/06/12
                        ' start of code added TJS 09/08/13
                    Else
                        If CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SKUChanged")) Then
                            m_ImportSKUChangeUpdateConfirmed = True

                        ElseIf CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SourceIDChanged")) Then
                            m_ImportSourceIDUpdateConfirmed = True

                        Else
                            m_ImportUpdateExistingItemConfirmed = True
                        End If
                        ' end of code added TJS 09/08/13
                    End If
                End If
            End If
        End If

    End Sub

    Private Sub GridViewSelectItems_CustomDrawCell(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs) Handles GridViewSelectItems.CustomDrawCell
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/11/11 | TJS             | 2011.1.10 | Mod1fied to cater for products where SKU is changed in source after import
        ' 18/03/13 | TJS             | 2013.1.04 | Miodified to cater for flag combinations
        ' 09/08/13 | TJs             | 2013.1.32 | Modified to cater for SourceIDChanged 
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SKUChanged")) Or _
            CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SourceIDChanged")) Then ' TJS 04/11/11 TJS 18/03/13 TJS 09/08/13
            e.Appearance.BackColor = Color.LightPink ' TJS 04/11/11
        ElseIf CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "SKUExists")) Then ' TJS 18/03/13
            e.Appearance.BackColor = Color.LightGray
        ElseIf CBool(Me.GridViewSelectItems.GetRowCellValue(e.RowHandle, "AlreadyImported")) Then
            e.Appearance.BackColor = Color.Gray
        End If

    End Sub

    Private Sub GridViewSelectItems_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewSelectItems.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 09/03/13 | TJS             | 2013.1.01 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.cbeImportSource.EditValue.ToString = "Magento" Then
            Select Case Me.GridViewSelectItems.GetRowCellValue(e.FocusedRowHandle, "SourceType").ToString
                Case "simple"
                    Me.colISItemType.ColumnEdit = Me.repISItemType
                    Me.colISItemType.OptionsColumn.AllowEdit = True
                    Me.colISItemType.OptionsColumn.ReadOnly = False

                Case Else
                    Me.colISItemType.ColumnEdit = Nothing
                    Me.colISItemType.OptionsColumn.AllowEdit = False
                    Me.colISItemType.OptionsColumn.ReadOnly = True

            End Select
        End If

    End Sub
#End Region

#Region " CategoryMapGridFunctions "
    Private Sub rbeISCategoryCode_PopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles rbeISCategoryCode.PopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/04/11 | TJS             | 2011.0.10 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iCategoryCount As Integer

        Select Case Me.cbeImportSource.EditValue.ToString
            Case "ASPDotNetStorefront"
                iCategoryCount = m_ASPStorefrontImportFacade.ASPStorefrontCategories.Length - 1

            Case "Magento"
                iCategoryCount = m_MagentoImportFacade.MagentoCategories.Length - 1

        End Select

        Me.TreeListCategories.FocusedNode.Item(Me.colTreeListISCategoryName) = eRow.DataRowSelected.Item("Description").ToString
        Me.TreeListCategories.FocusedNode.Item(Me.colTreeListISCategoryCode) = eRow.DataRowSelected.Item("CategoryCode").ToString
        For iLoop = 0 To iCategoryCount
            Select Case Me.cbeImportSource.EditValue.ToString
                Case "ASPDotNetStorefront"
                    If m_ASPStorefrontImportFacade.ASPStorefrontCategories(iLoop).CategoryID = CInt(Me.TreeListCategories.FocusedNode.Item(Me.colTreeListSourceCategoryID)) Then
                        m_ASPStorefrontImportFacade.ASPStorefrontCategories(iLoop).ISCategoryCode = eRow.DataRowSelected.Item("CategoryCode").ToString
                        Exit For
                    End If

                Case "Magento"
                    If m_MagentoImportFacade.MagentoCategories(iLoop).CategoryID = CInt(Me.TreeListCategories.FocusedNode.Item(Me.colTreeListSourceCategoryID)) Then
                        m_MagentoImportFacade.MagentoCategories(iLoop).ISCategoryCode = eRow.DataRowSelected.Item("CategoryCode").ToString
                        Exit For
                    End If

            End Select
        Next
    End Sub

#End Region

#Region " ImportOptionsSet "
    Private Sub RadioGroupQtyPublishing_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroupQtyPublishing.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If RadioGroupQtyPublishing.SelectedIndex >= 0 Then
            Select Case RadioGroupQtyPublishing.EditValue.ToString
                Case "Fixed"
                    Me.TextEditQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Text = "Value to Publish"

                Case "Percent"
                    Me.TextEditQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Enabled = True
                    Me.lblQtyPublishingValue.Text = "% of Total Qty"

                Case Else
                    Me.TextEditQtyPublishingValue.Enabled = False
                    Me.lblQtyPublishingValue.Enabled = False
                    Me.lblQtyPublishingValue.Text = ""

            End Select
            Me.RadioGroupQtyPublishing.ErrorText = ""

        End If

    End Sub

    Private Sub chkImportASPStorefrontExtData1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkImportASPStorefrontExtData1.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportASPStorefrontExtData1.Checked Then
            Me.cbeASPExtData1CustomField.Enabled = True
        Else
            Me.cbeASPExtData1CustomField.Enabled = False
            Me.cbeASPExtData1CustomField.ErrorText = ""
            Me.cbeASPExtData1CustomField.Text = ""
        End If

    End Sub

    Private Sub chkImportASPStorefrontExtData2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkImportASPStorefrontExtData2.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportASPStorefrontExtData2.Checked Then
            Me.cbeASPExtData2CustomField.Enabled = True
        Else
            Me.cbeASPExtData2CustomField.Enabled = False
            Me.cbeASPExtData2CustomField.ErrorText = ""
            Me.cbeASPExtData2CustomField.Text = ""
        End If

    End Sub

    Private Sub chkImportASPStorefrontExtData3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkImportASPStorefrontExtData3.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportASPStorefrontExtData3.Checked Then
            Me.cbeASPExtData3CustomField.Enabled = True
        Else
            Me.cbeASPExtData3CustomField.Enabled = False
            Me.cbeASPExtData3CustomField.ErrorText = ""
            Me.cbeASPExtData3CustomField.Text = ""
        End If

    End Sub

    Private Sub chkImportASPStorefrontExtData4_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkImportASPStorefrontExtData4.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportASPStorefrontExtData4.Checked Then
            Me.cbeASPExtData4CustomField.Enabled = True
        Else
            Me.cbeASPExtData4CustomField.Enabled = False
            Me.cbeASPExtData4CustomField.ErrorText = ""
            Me.cbeASPExtData4CustomField.Text = ""
        End If

    End Sub

    Private Sub chkImportASPStorefrontExtData5_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkImportASPStorefrontExtData5.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportASPStorefrontExtData5.Checked Then
            Me.cbeASPExtData5CustomField.Enabled = True
        Else
            Me.cbeASPExtData5CustomField.Enabled = False
            Me.cbeASPExtData5CustomField.ErrorText = ""
            Me.cbeASPExtData5CustomField.Text = ""
        End If

    End Sub

    Private Sub chkImportMagentoSellingPriceAsRetail_Click(sender As Object, e As System.EventArgs) Handles chkImportMagentoSellingPriceAsRetail.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/14 | TJS             | 2013.4.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportMagentoSellingPriceAsRetail.Checked Then
            Me.chkImportMagentoSpecialPriceAsRetail.Checked = False
        End If

    End Sub

    Private Sub chkImportMagentoSellingPriceAsWholesale_Click(sender As Object, e As System.EventArgs) Handles chkImportMagentoSellingPriceAsWholesale.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/14 | TJS             | 2013.4.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportMagentoSellingPriceAsWholesale.Checked Then
            Me.chkImportMagentoSpecialPriceAsWholesale.Checked = False
        End If

    End Sub

    Private Sub chkImportMagentoSpecialPriceAsRetail_Click(sender As Object, e As System.EventArgs) Handles chkImportMagentoSpecialPriceAsRetail.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/14 | TJS             | 2013.4.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportMagentoSpecialPriceAsRetail.Checked Then
            Me.chkImportMagentoSellingPriceAsRetail.Checked = False
        End If

    End Sub

    Private Sub chkImportMagentoSpecialPriceAsWholesale_Click(sender As Object, e As System.EventArgs) Handles chkImportMagentoSpecialPriceAsWholesale.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 06/02/14 | TJS             | 2013.4.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkImportMagentoSpecialPriceAsWholesale.Checked Then
            Me.chkImportMagentoSellingPriceAsWholesale.Checked = False
        End If

    End Sub
#End Region

#Region " SelectAllSelectNone "
    Private Sub btnSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAll.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/11/11 | TJS             | 2011.1.09 | Modified to use field names instead of index numbers and to 
        '                                        | prevent setting of Import flag on duplicate or erroneous SKUs 
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to cater for updating imported items
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to display wait cursor while updating import check box
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        ' 01/06/13 | TJS             | 2013.1.19 | Modified to prevent setting of Import flag on erroneous SKUs after update confirmed
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strMessage As String, iLoop As Integer, bUpdateAsked As Boolean, bUpdateImportedConfirmed As Boolean ' TJS 10/06/12

        Cursor = Cursors.WaitCursor ' TJS 05/07/12
        Application.DoEvents() ' TJS 05/07/12
        bUpdateAsked = False ' TJS 10/06/12
        bUpdateImportedConfirmed = False ' TJS 10/06/12
        For iLoop = 0 To m_ItemsForImport.Rows.Count - 1
            If Not CBool(m_ItemsForImport.Rows(iLoop).Item("AlreadyImported")) And _
                Not CBool(m_ItemsForImport.Rows(iLoop).Item("SKUExists")) And _
                Not CBool(m_ItemsForImport.Rows(iLoop).Item("SKUError")) Then ' TJS 02/11/11
                m_ItemsForImport.Rows(iLoop).Item("Import") = True ' TJS 02/11/11

            ElseIf CBool(m_ItemsForImport.Rows(iLoop).Item("AlreadyImported")) And _
                Not CBool(m_ItemsForImport.Rows(iLoop).Item("SKUError")) And bUpdateImportedConfirmed Then ' TJS 10/06/12 TJS 01/06/13
                m_ItemsForImport.Rows(iLoop).Item("Import") = True ' TJS 10/06/12

            ElseIf CBool(m_ItemsForImport.Rows(iLoop).Item("AlreadyImported")) And Not bUpdateAsked Then ' TJS 10/06/12
                strMessage = "Are you sure you want to import all items again and overwrite the" & vbCrLf & "settings in " & IS_PRODUCT_NAME & " with the latest values from " ' TJS 10/06/12 TJS 24/08/12
                Select Case Me.cbeImportSource.EditValue.ToString ' TJS 24/08/12
                    Case "Amazon" ' TJS 24/08/12
                        strMessage = strMessage & "Amazon ?" ' TJS 24/08/12
                    Case "ASPDotNetStorefront" ' TJS 24/08/12
                        strMessage = strMessage & "ASPDotNetStorefront ?" ' TJS 24/08/12
                    Case "Channel Advisor" ' TJS 24/08/12
                        strMessage = strMessage & "Channel Advisor ?" ' TJS 24/08/12
                    Case "Magento" ' TJS 24/08/12
                        strMessage = strMessage & "Magento ?" ' TJS 24/08/12
                End Select
                If Interprise.Presentation.Base.Message.MessageWindow.Show(strMessage, "Import and Overwrite Values", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Yes Then ' TJS 10/06/12
                    bUpdateImportedConfirmed = True ' TJS 10/06/12
                    m_ItemsForImport.Rows(iLoop).Item("Import") = True ' TJS 10/06/12
                End If
                bUpdateAsked = True ' TJS 10/06/12
            End If
        Next
        Cursor = Cursors.Default ' TJS 05/07/12

    End Sub

    Private Sub btnSelectNone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelectNone.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/11/11 | TJS             | 2011.1.09 | Modified to use field name instead of index number
        ' 05/07/12 | TJS             | 2012.1.08 | Modified to display wait cursor while updating import check box
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer

        Cursor = Cursors.WaitCursor ' TJS 05/07/12
        Application.DoEvents() ' TJS 05/07/12
        For iLoop = 0 To m_ItemsForImport.Rows.Count - 1
            m_ItemsForImport.Rows(iLoop).Item("Import") = False ' TJS 02/11/11
        Next
        Cursor = Cursors.Default ' TJS 05/07/12

    End Sub
#End Region
#End Region

#Region " ViewImportLog "
    Private Sub ViewImportLog(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEditViewImportLog.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.CheckEditViewImportLog.Checked Then
            Me.lblImportLog.Visible = True
            Me.MemoEditImportLog.Visible = True
        Else
            Me.lblImportLog.Visible = False
            Me.MemoEditImportLog.Visible = False
        End If

    End Sub
#End Region
End Class
#End Region
