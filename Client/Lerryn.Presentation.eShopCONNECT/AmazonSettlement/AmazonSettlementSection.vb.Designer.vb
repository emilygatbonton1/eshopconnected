'===============================================================================
' Interprise Suite SDK
' Copyright © 2009-2010 Interprise Software Solutions Incorporated
' All rights reserved.
' 
' Interprise Plugin Factory - Generated Code
'
' This code and information is provided "as is" without warranty
' of any kind, either expressed or implied, including but not
' limited to the implied warranties of merchantability and
' fitness for a particular purpose.
'===============================================================================

Option Explicit On
Option Strict On

#Region " AmazonSettlementSection "
Namespace AmazonSettlement
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AmazonSettlementSection
	Inherits Interprise.Presentation.Base.BaseControl
	Implements Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Protected Overridable Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AmazonSettlementSection))
            Me.AmazonSettlementSectionGateway = Me.ImportExportDataset
            Me.AmazonSettlementSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
            Me.TextEditTotalCharges = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalGrossSales = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalServiceFeeSalesTax = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalInboundShipping = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalOthers = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalSubscriptionFees = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalCommission = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalFBAWeightFees = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalFBAPerUnitFees = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalFBAPerOrderFees = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalSalesTax = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalShipping = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditTotalPrincipal = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditExchangeRate = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditCurrencyCode = New DevExpress.XtraEditors.TextEdit()
            Me.DateEditDepositDate = New DevExpress.XtraEditors.DateEdit()
            Me.TextEditTotalValue = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditAmazonSettlementID = New DevExpress.XtraEditors.TextEdit()
            Me.DateEditEndDate = New DevExpress.XtraEditors.DateEdit()
            Me.GridControlSettlementDetail = New DevExpress.XtraGrid.GridControl()
            Me.GridViewSettlementDetail = New DevExpress.XtraGrid.Views.Grid.GridView()
            Me.colCounter = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colSettlementCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colTransactionGroup_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colTransactionType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colAmazonOrderID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colAmazonOrderItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colShipmentID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colPostedDate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colItemSKU_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colItemQuantity_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colFulfillmentType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colCurrencyCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colExchangeRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colPrincipalAmountRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colPrincipalAmount_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colShippingAmountRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colShippingAmount_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colTaxAmountRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colTaxAmount_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colCommissionAmountRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colCommissionAmount_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colFBAPerOrderFulfillmentFee_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colFBAPerUnitFulfillmentFee_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colFBAWeightBasedFeeRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colFBAWeightBasedFee_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colSalesTaxServiceFeeRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colSalesTaxServiceFee_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colMerchantPromotionID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colPromotionType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colPromotionAmountRate_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colPromotionAmount_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colISSalesOrderCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.repISSalesOrder = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit()
            Me.colISItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.repISItem = New DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit()
            Me.colISItemLineNum_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colReconciled_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colReconciliationComments_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.repComments = New DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit()
            Me.colUserCreated = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colDateCreated = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colUserModified = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.colDateModified = New DevExpress.XtraGrid.Columns.GridColumn()
            Me.DateEditStartDate = New DevExpress.XtraEditors.DateEdit()
            Me.TextEditMerchantName = New DevExpress.XtraEditors.TextEdit()
            Me.TextEditMerchantID = New DevExpress.XtraEditors.TextEdit()
            Me.AmazonSettlementSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
            Me.LayoutControlItemEndDate = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemAmazonSettlementID = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemStartDate = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemMerchantName = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemMerchantID = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemSettlementDetail = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemCurrencyCode = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemExchangeRate = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalValue = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalPrincipal = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalShipping = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalSalesTax = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemDepositDate = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalFBAPerOrderFees = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalFBAPerUnitFees = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalFBAWeightFees = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalCommission = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalSubscriptionFees = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalOthers = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalInboundShipping = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalServiceFeeSalesTax = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalGrossSales = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            Me.LayoutControlItemTotalCharges = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
            CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmazonSettlementSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmazonSettlementSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.AmazonSettlementSectionExtendedLayout.SuspendLayout()
            CType(Me.TextEditTotalCharges.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalGrossSales.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalServiceFeeSalesTax.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalInboundShipping.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalOthers.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalSubscriptionFees.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalCommission.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalFBAWeightFees.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalFBAPerUnitFees.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalFBAPerOrderFees.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalSalesTax.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalShipping.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalPrincipal.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditExchangeRate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditCurrencyCode.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditDepositDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditDepositDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditTotalValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditAmazonSettlementID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditEndDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.GridControlSettlementDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.GridViewSettlementDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.repISSalesOrder, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.repISItem, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.repComments, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.DateEditStartDate.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditMerchantName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextEditMerchantID.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.AmazonSettlementSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemEndDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemAmazonSettlementID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemMerchantName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemMerchantID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemSettlementDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemCurrencyCode, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemExchangeRate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalValue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalPrincipal, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalShipping, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalSalesTax, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemDepositDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalFBAPerOrderFees, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalFBAPerUnitFees, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalFBAWeightFees, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalCommission, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalSubscriptionFees, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalOthers, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalInboundShipping, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalServiceFeeSalesTax, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalGrossSales, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LayoutControlItemTotalCharges, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ImageCollectionContextMenu
            '
            Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
            '
            'AmazonSettlementSectionGateway
            '
            Me.AmazonSettlementSectionGateway.DataSetName = "AmazonSettlementSectionDataset"
            Me.AmazonSettlementSectionGateway.Instantiate = False
            Me.AmazonSettlementSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
            '
            '****************************************************************************************
            '
            'ImportExportDatasetGateway COMPONENT DESIGNER GENERATED CODE
            'STRICTLY FOLLOW THE STEPS BELOW IN ORDER TO IMPLEMENT DATASET SHARING
            'ACCROSS MULTIPLE USER CONTROLS AND/OR WINFORM CONTROLS
            '
            'NOTE: MAKE SURE YOU HAVE A REFERENCE TO THE FRAMEWORK DATASET COMPONENT IN YOUR TOOLBOX
            '
            '1.  SWITCH TO DESIGN VIEW OF YOUR PROJECT
            '2.  ADD AN "
            'ImportExportDatasetGateway" COMPONENT3.  IF THIS IS A PUGIN CONTROL, SET THE "Instantiate" PROPERTY TO "False"
            '4.  IF THIS IS THE MAIN PLUGIN CONTAINER, SET THE "Instantiate" PROPERTY TO "True"
            '5.  SWITCH TO CODE VIEW OF YOUR PROJECT
            '6.  ADD THE FF. CODES BELOW AND PLACE IT OUTSIDE THIS FUNCTION
            '
            '        #Region " Private Variables "
            '            Private m_importExportDataset as Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway 
            '        #End Region
            '
            '        #Region " Properties "
            '
            '        #Region "ImportExportDataset"
            '            Public ReadOnly Property ImportExportDataset AS Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway 
            '                Get
            '                    Return Me.m_importExportDataset
            '                End Get
            '            End Property
            '        #End Region
            '
            '        #End Region
            '
            '****************************************************************************************
            '
            '
            'AmazonSettlementSectionExtendedLayout
            '
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalCharges)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalGrossSales)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalServiceFeeSalesTax)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalInboundShipping)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalOthers)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalSubscriptionFees)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalCommission)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalFBAWeightFees)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalFBAPerUnitFees)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalFBAPerOrderFees)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalSalesTax)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalShipping)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalPrincipal)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditExchangeRate)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditCurrencyCode)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.DateEditDepositDate)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditTotalValue)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditAmazonSettlementID)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.DateEditEndDate)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.GridControlSettlementDetail)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.DateEditStartDate)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditMerchantName)
            Me.AmazonSettlementSectionExtendedLayout.Controls.Add(Me.TextEditMerchantID)
            Me.AmazonSettlementSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
            Me.AmazonSettlementSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
            Me.AmazonSettlementSectionExtendedLayout.Name = "AmazonSettlementSectionExtendedLayout"
            Me.AmazonSettlementSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
            Me.AmazonSettlementSectionExtendedLayout.Root = Me.AmazonSettlementSectionLayoutGroup
            Me.AmazonSettlementSectionExtendedLayout.Size = New System.Drawing.Size(930, 566)
            Me.AmazonSettlementSectionExtendedLayout.TabIndex = 0
            Me.AmazonSettlementSectionExtendedLayout.Text = "AmazonSettlementSectionExtendedLayout"
            '
            'TextEditTotalCharges
            '
            Me.TextEditTotalCharges.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalChargesRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalCharges, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalCharges.Location = New System.Drawing.Point(843, 125)
            Me.TextEditTotalCharges.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalCharges.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalCharges.Name = "TextEditTotalCharges"
            Me.TextEditTotalCharges.Properties.AutoHeight = False
            Me.TextEditTotalCharges.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalCharges.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalCharges.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalCharges.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalCharges.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalCharges, System.Drawing.Color.Empty)
            Me.TextEditTotalCharges.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalCharges.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalCharges.TabIndex = 27
            Me.TextEditTotalCharges.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalCharges, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalGrossSales
            '
            Me.TextEditTotalGrossSales.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalGrossSalesRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalGrossSales, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalGrossSales.Location = New System.Drawing.Point(571, 76)
            Me.TextEditTotalGrossSales.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalGrossSales.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalGrossSales.Name = "TextEditTotalGrossSales"
            Me.TextEditTotalGrossSales.Properties.AutoHeight = False
            Me.TextEditTotalGrossSales.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalGrossSales.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalGrossSales.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalGrossSales.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalGrossSales.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalGrossSales, System.Drawing.Color.Empty)
            Me.TextEditTotalGrossSales.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalGrossSales.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalGrossSales.TabIndex = 26
            Me.TextEditTotalGrossSales.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalGrossSales, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalServiceFeeSalesTax
            '
            Me.TextEditTotalServiceFeeSalesTax.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalServiceFeeSalesTaxRate_DEV00022" & _
                        "1", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalServiceFeeSalesTax, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalServiceFeeSalesTax.Location = New System.Drawing.Point(681, 125)
            Me.TextEditTotalServiceFeeSalesTax.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalServiceFeeSalesTax.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalServiceFeeSalesTax.Name = "TextEditTotalServiceFeeSalesTax"
            Me.TextEditTotalServiceFeeSalesTax.Properties.AutoHeight = False
            Me.TextEditTotalServiceFeeSalesTax.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalServiceFeeSalesTax.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalServiceFeeSalesTax.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalServiceFeeSalesTax.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalServiceFeeSalesTax.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalServiceFeeSalesTax, System.Drawing.Color.Empty)
            Me.TextEditTotalServiceFeeSalesTax.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalServiceFeeSalesTax.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalServiceFeeSalesTax.TabIndex = 25
            Me.TextEditTotalServiceFeeSalesTax.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalServiceFeeSalesTax, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalInboundShipping
            '
            Me.TextEditTotalInboundShipping.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalInboundShippingRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalInboundShipping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalInboundShipping.Location = New System.Drawing.Point(681, 101)
            Me.TextEditTotalInboundShipping.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalInboundShipping.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalInboundShipping.Name = "TextEditTotalInboundShipping"
            Me.TextEditTotalInboundShipping.Properties.AutoHeight = False
            Me.TextEditTotalInboundShipping.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalInboundShipping.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalInboundShipping.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalInboundShipping.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalInboundShipping.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalInboundShipping, System.Drawing.Color.Empty)
            Me.TextEditTotalInboundShipping.Size = New System.Drawing.Size(80, 20)
            Me.TextEditTotalInboundShipping.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalInboundShipping.TabIndex = 24
            Me.TextEditTotalInboundShipping.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalInboundShipping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalOthers
            '
            Me.TextEditTotalOthers.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalOthersRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalOthers, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalOthers.Location = New System.Drawing.Point(459, 125)
            Me.TextEditTotalOthers.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalOthers.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalOthers.Name = "TextEditTotalOthers"
            Me.TextEditTotalOthers.Properties.AutoHeight = False
            Me.TextEditTotalOthers.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalOthers.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalOthers.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalOthers.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalOthers.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalOthers, System.Drawing.Color.Empty)
            Me.TextEditTotalOthers.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalOthers.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalOthers.TabIndex = 23
            Me.TextEditTotalOthers.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalOthers, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalSubscriptionFees
            '
            Me.TextEditTotalSubscriptionFees.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalSubscriptionFeesRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalSubscriptionFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalSubscriptionFees.Location = New System.Drawing.Point(282, 125)
            Me.TextEditTotalSubscriptionFees.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalSubscriptionFees.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalSubscriptionFees.Name = "TextEditTotalSubscriptionFees"
            Me.TextEditTotalSubscriptionFees.Properties.AutoHeight = False
            Me.TextEditTotalSubscriptionFees.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalSubscriptionFees.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalSubscriptionFees.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalSubscriptionFees.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalSubscriptionFees.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalSubscriptionFees, System.Drawing.Color.Empty)
            Me.TextEditTotalSubscriptionFees.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalSubscriptionFees.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalSubscriptionFees.TabIndex = 22
            Me.TextEditTotalSubscriptionFees.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalSubscriptionFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalCommission
            '
            Me.TextEditTotalCommission.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalCommissionRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalCommission, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalCommission.Location = New System.Drawing.Point(105, 125)
            Me.TextEditTotalCommission.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalCommission.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalCommission.Name = "TextEditTotalCommission"
            Me.TextEditTotalCommission.Properties.AutoHeight = False
            Me.TextEditTotalCommission.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalCommission.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalCommission.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalCommission.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalCommission.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalCommission, System.Drawing.Color.Empty)
            Me.TextEditTotalCommission.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalCommission.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalCommission.TabIndex = 21
            Me.TextEditTotalCommission.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalCommission, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalFBAWeightFees
            '
            Me.TextEditTotalFBAWeightFees.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalFBAWeightFeesRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalFBAWeightFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalFBAWeightFees.Location = New System.Drawing.Point(459, 101)
            Me.TextEditTotalFBAWeightFees.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalFBAWeightFees.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalFBAWeightFees.Name = "TextEditTotalFBAWeightFees"
            Me.TextEditTotalFBAWeightFees.Properties.AutoHeight = False
            Me.TextEditTotalFBAWeightFees.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalFBAWeightFees.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalFBAWeightFees.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalFBAWeightFees.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalFBAWeightFees.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalFBAWeightFees, System.Drawing.Color.Empty)
            Me.TextEditTotalFBAWeightFees.Size = New System.Drawing.Size(80, 20)
            Me.TextEditTotalFBAWeightFees.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalFBAWeightFees.TabIndex = 20
            Me.TextEditTotalFBAWeightFees.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalFBAWeightFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalFBAPerUnitFees
            '
            Me.TextEditTotalFBAPerUnitFees.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalFBAPerUnitFeesRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalFBAPerUnitFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalFBAPerUnitFees.Location = New System.Drawing.Point(282, 101)
            Me.TextEditTotalFBAPerUnitFees.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalFBAPerUnitFees.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalFBAPerUnitFees.Name = "TextEditTotalFBAPerUnitFees"
            Me.TextEditTotalFBAPerUnitFees.Properties.AutoHeight = False
            Me.TextEditTotalFBAPerUnitFees.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalFBAPerUnitFees.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalFBAPerUnitFees.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalFBAPerUnitFees.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalFBAPerUnitFees.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalFBAPerUnitFees, System.Drawing.Color.Empty)
            Me.TextEditTotalFBAPerUnitFees.Size = New System.Drawing.Size(80, 20)
            Me.TextEditTotalFBAPerUnitFees.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalFBAPerUnitFees.TabIndex = 19
            Me.TextEditTotalFBAPerUnitFees.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalFBAPerUnitFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalFBAPerOrderFees
            '
            Me.TextEditTotalFBAPerOrderFees.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalFBAPerOrderFeesRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalFBAPerOrderFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalFBAPerOrderFees.Location = New System.Drawing.Point(105, 101)
            Me.TextEditTotalFBAPerOrderFees.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalFBAPerOrderFees.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalFBAPerOrderFees.Name = "TextEditTotalFBAPerOrderFees"
            Me.TextEditTotalFBAPerOrderFees.Properties.AutoHeight = False
            Me.TextEditTotalFBAPerOrderFees.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalFBAPerOrderFees.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalFBAPerOrderFees.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalFBAPerOrderFees.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalFBAPerOrderFees.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalFBAPerOrderFees, System.Drawing.Color.Empty)
            Me.TextEditTotalFBAPerOrderFees.Size = New System.Drawing.Size(80, 20)
            Me.TextEditTotalFBAPerOrderFees.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalFBAPerOrderFees.TabIndex = 18
            Me.TextEditTotalFBAPerOrderFees.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalFBAPerOrderFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalSalesTax
            '
            Me.TextEditTotalSalesTax.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalSalesTaxRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalSalesTax, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalSalesTax.Location = New System.Drawing.Point(399, 76)
            Me.TextEditTotalSalesTax.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalSalesTax.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalSalesTax.Name = "TextEditTotalSalesTax"
            Me.TextEditTotalSalesTax.Properties.AutoHeight = False
            Me.TextEditTotalSalesTax.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalSalesTax.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalSalesTax.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalSalesTax.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalSalesTax.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalSalesTax, System.Drawing.Color.Empty)
            Me.TextEditTotalSalesTax.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalSalesTax.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalSalesTax.TabIndex = 16
            Me.TextEditTotalSalesTax.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalSalesTax, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalShipping
            '
            Me.TextEditTotalShipping.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalShippingRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalShipping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalShipping.Location = New System.Drawing.Point(222, 76)
            Me.TextEditTotalShipping.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalShipping.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalShipping.Name = "TextEditTotalShipping"
            Me.TextEditTotalShipping.Properties.AutoHeight = False
            Me.TextEditTotalShipping.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalShipping.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalShipping.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalShipping.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalShipping.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalShipping, System.Drawing.Color.Empty)
            Me.TextEditTotalShipping.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalShipping.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalShipping.TabIndex = 15
            Me.TextEditTotalShipping.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalShipping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalPrincipal
            '
            Me.TextEditTotalPrincipal.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalPrincipalRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalPrincipal, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalPrincipal.Location = New System.Drawing.Point(80, 76)
            Me.TextEditTotalPrincipal.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalPrincipal.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalPrincipal.Name = "TextEditTotalPrincipal"
            Me.TextEditTotalPrincipal.Properties.AutoHeight = False
            Me.TextEditTotalPrincipal.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalPrincipal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalPrincipal.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalPrincipal.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalPrincipal.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalPrincipal, System.Drawing.Color.Empty)
            Me.TextEditTotalPrincipal.Size = New System.Drawing.Size(80, 21)
            Me.TextEditTotalPrincipal.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalPrincipal.TabIndex = 14
            Me.TextEditTotalPrincipal.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalPrincipal, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditExchangeRate
            '
            Me.TextEditExchangeRate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.ExchangeRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditExchangeRate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditExchangeRate.Location = New System.Drawing.Point(399, 52)
            Me.TextEditExchangeRate.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditExchangeRate.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditExchangeRate.Name = "TextEditExchangeRate"
            Me.TextEditExchangeRate.Properties.AutoHeight = False
            Me.TextEditExchangeRate.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditExchangeRate, System.Drawing.Color.Empty)
            Me.TextEditExchangeRate.Size = New System.Drawing.Size(80, 20)
            Me.TextEditExchangeRate.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditExchangeRate.TabIndex = 13
            Me.TextEditExchangeRate.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditExchangeRate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditCurrencyCode
            '
            Me.TextEditCurrencyCode.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.CurrencyCode_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditCurrencyCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditCurrencyCode.Location = New System.Drawing.Point(222, 52)
            Me.TextEditCurrencyCode.MaximumSize = New System.Drawing.Size(60, 22)
            Me.TextEditCurrencyCode.MinimumSize = New System.Drawing.Size(60, 20)
            Me.TextEditCurrencyCode.Name = "TextEditCurrencyCode"
            Me.TextEditCurrencyCode.Properties.AutoHeight = False
            Me.TextEditCurrencyCode.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditCurrencyCode, System.Drawing.Color.Empty)
            Me.TextEditCurrencyCode.Size = New System.Drawing.Size(60, 20)
            Me.TextEditCurrencyCode.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditCurrencyCode.TabIndex = 12
            Me.TextEditCurrencyCode.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditCurrencyCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'DateEditDepositDate
            '
            Me.DateEditDepositDate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.DepositDate_DEV000221", True))
            Me.DateEditDepositDate.EditValue = Nothing
            Me.ExtendControlProperty.SetHelpText(Me.DateEditDepositDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.DateEditDepositDate.Location = New System.Drawing.Point(571, 52)
            Me.DateEditDepositDate.MaximumSize = New System.Drawing.Size(100, 22)
            Me.DateEditDepositDate.Name = "DateEditDepositDate"
            Me.DateEditDepositDate.Properties.AutoHeight = False
            Me.DateEditDepositDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.DateEditDepositDate.Properties.ReadOnly = True
            Me.DateEditDepositDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditDepositDate, System.Drawing.Color.Empty)
            Me.DateEditDepositDate.Size = New System.Drawing.Size(100, 20)
            Me.DateEditDepositDate.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.DateEditDepositDate.TabIndex = 10
            Me.DateEditDepositDate.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.DateEditDepositDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditTotalValue
            '
            Me.TextEditTotalValue.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.TotalAmountRate_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditTotalValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditTotalValue.Location = New System.Drawing.Point(80, 52)
            Me.TextEditTotalValue.MaximumSize = New System.Drawing.Size(80, 22)
            Me.TextEditTotalValue.MinimumSize = New System.Drawing.Size(80, 20)
            Me.TextEditTotalValue.Name = "TextEditTotalValue"
            Me.TextEditTotalValue.Properties.AutoHeight = False
            Me.TextEditTotalValue.Properties.DisplayFormat.FormatString = "n2"
            Me.TextEditTotalValue.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalValue.Properties.EditFormat.FormatString = "n2"
            Me.TextEditTotalValue.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.TextEditTotalValue.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditTotalValue, System.Drawing.Color.Empty)
            Me.TextEditTotalValue.Size = New System.Drawing.Size(80, 20)
            Me.TextEditTotalValue.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditTotalValue.TabIndex = 9
            Me.TextEditTotalValue.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditTotalValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditAmazonSettlementID
            '
            Me.TextEditAmazonSettlementID.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.MerchantID_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditAmazonSettlementID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditAmazonSettlementID.Location = New System.Drawing.Point(116, 27)
            Me.TextEditAmazonSettlementID.MaximumSize = New System.Drawing.Size(200, 22)
            Me.TextEditAmazonSettlementID.MinimumSize = New System.Drawing.Size(200, 0)
            Me.TextEditAmazonSettlementID.Name = "TextEditAmazonSettlementID"
            Me.TextEditAmazonSettlementID.Properties.AutoHeight = False
            Me.TextEditAmazonSettlementID.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditAmazonSettlementID, System.Drawing.Color.Empty)
            Me.TextEditAmazonSettlementID.Size = New System.Drawing.Size(200, 21)
            Me.TextEditAmazonSettlementID.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditAmazonSettlementID.TabIndex = 8
            Me.TextEditAmazonSettlementID.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditAmazonSettlementID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'DateEditEndDate
            '
            Me.DateEditEndDate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.SettlementEndDate_DEV000221", True))
            Me.DateEditEndDate.EditValue = Nothing
            Me.ExtendControlProperty.SetHelpText(Me.DateEditEndDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.DateEditEndDate.Location = New System.Drawing.Point(640, 27)
            Me.DateEditEndDate.MaximumSize = New System.Drawing.Size(100, 22)
            Me.DateEditEndDate.Name = "DateEditEndDate"
            Me.DateEditEndDate.Properties.AutoHeight = False
            Me.DateEditEndDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.DateEditEndDate.Properties.ReadOnly = True
            Me.DateEditEndDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditEndDate, System.Drawing.Color.Empty)
            Me.DateEditEndDate.Size = New System.Drawing.Size(100, 21)
            Me.DateEditEndDate.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.DateEditEndDate.TabIndex = 7
            Me.DateEditEndDate.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.DateEditEndDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'GridControlSettlementDetail
            '
            Me.GridControlSettlementDetail.DataMember = "LerrynImportExportAmazonSettlementDetail_DEV000221"
            Me.GridControlSettlementDetail.DataSource = Me.AmazonSettlementSectionGateway
            Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlSettlementDetail, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
            Me.ExtendControlProperty.SetHelpText(Me.GridControlSettlementDetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.GridControlSettlementDetail.Location = New System.Drawing.Point(2, 150)
            Me.GridControlSettlementDetail.MainView = Me.GridViewSettlementDetail
            Me.GridControlSettlementDetail.Name = "GridControlSettlementDetail"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlSettlementDetail, System.Drawing.Color.Empty)
            Me.GridControlSettlementDetail.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repISSalesOrder, Me.repISItem, Me.repComments})
            Me.GridControlSettlementDetail.Size = New System.Drawing.Size(926, 414)
            Me.GridControlSettlementDetail.TabIndex = 11
            Me.ExtendControlProperty.SetTextDisplay(Me.GridControlSettlementDetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.GridControlSettlementDetail.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewSettlementDetail})
            '
            'GridViewSettlementDetail
            '
            Me.GridViewSettlementDetail.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colCounter, Me.colSettlementCode_DEV000221, Me.colTransactionGroup_DEV000221, Me.colTransactionType_DEV000221, Me.colAmazonOrderID_DEV000221, Me.colAmazonOrderItemCode_DEV000221, Me.colShipmentID_DEV000221, Me.colPostedDate_DEV000221, Me.colItemSKU_DEV000221, Me.colItemQuantity_DEV000221, Me.colFulfillmentType_DEV000221, Me.colCurrencyCode_DEV000221, Me.colExchangeRate_DEV000221, Me.colPrincipalAmountRate_DEV000221, Me.colPrincipalAmount_DEV000221, Me.colShippingAmountRate_DEV000221, Me.colShippingAmount_DEV000221, Me.colTaxAmountRate_DEV000221, Me.colTaxAmount_DEV000221, Me.colCommissionAmountRate_DEV000221, Me.colCommissionAmount_DEV000221, Me.colFBAPerOrderFulfillmentFeeRate_DEV000221, Me.colFBAPerOrderFulfillmentFee_DEV000221, Me.colFBAPerUnitFulfillmentFeeRate_DEV000221, Me.colFBAPerUnitFulfillmentFee_DEV000221, Me.colFBAWeightBasedFeeRate_DEV000221, Me.colFBAWeightBasedFee_DEV000221, Me.colSalesTaxServiceFeeRate_DEV000221, Me.colSalesTaxServiceFee_DEV000221, Me.colMerchantPromotionID_DEV000221, Me.colPromotionType_DEV000221, Me.colPromotionAmountRate_DEV000221, Me.colPromotionAmount_DEV000221, Me.colISSalesOrderCode_DEV000221, Me.colISItemCode_DEV000221, Me.colISItemLineNum_DEV000221, Me.colReconciled_DEV000221, Me.colReconciliationComments_DEV000221, Me.colUserCreated, Me.colDateCreated, Me.colUserModified, Me.colDateModified})
            Me.GridViewSettlementDetail.GridControl = Me.GridControlSettlementDetail
            Me.GridViewSettlementDetail.GroupCount = 3
            Me.GridViewSettlementDetail.Name = "GridViewSettlementDetail"
            Me.GridViewSettlementDetail.OptionsView.ColumnAutoWidth = False
            Me.GridViewSettlementDetail.OptionsView.ShowGroupPanel = False
            Me.GridViewSettlementDetail.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colTransactionGroup_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPostedDate_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending), New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colAmazonOrderID_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending)})
            '
            'colCounter
            '
            Me.colCounter.FieldName = "Counter"
            Me.colCounter.Name = "colCounter"
            Me.colCounter.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colCounter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colSettlementCode_DEV000221
            '
            Me.colSettlementCode_DEV000221.FieldName = "SettlementCode_DEV000221"
            Me.colSettlementCode_DEV000221.Name = "colSettlementCode_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colSettlementCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colTransactionGroup_DEV000221
            '
            Me.colTransactionGroup_DEV000221.Caption = "Type"
            Me.colTransactionGroup_DEV000221.FieldName = "TransactionGroup_DEV000221"
            Me.colTransactionGroup_DEV000221.Name = "colTransactionGroup_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colTransactionGroup_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colTransactionType_DEV000221
            '
            Me.colTransactionType_DEV000221.Caption = "Type"
            Me.colTransactionType_DEV000221.FieldName = "TransactionType_DEV000221"
            Me.colTransactionType_DEV000221.Name = "colTransactionType_DEV000221"
            Me.colTransactionType_DEV000221.OptionsColumn.AllowEdit = False
            Me.colTransactionType_DEV000221.OptionsColumn.FixedWidth = True
            Me.colTransactionType_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colTransactionType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colAmazonOrderID_DEV000221
            '
            Me.colAmazonOrderID_DEV000221.Caption = "Amazon Order ID"
            Me.colAmazonOrderID_DEV000221.FieldName = "AmazonOrderID_DEV000221"
            Me.colAmazonOrderID_DEV000221.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
            Me.colAmazonOrderID_DEV000221.Name = "colAmazonOrderID_DEV000221"
            Me.colAmazonOrderID_DEV000221.OptionsColumn.AllowEdit = False
            Me.colAmazonOrderID_DEV000221.OptionsColumn.FixedWidth = True
            Me.colAmazonOrderID_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colAmazonOrderID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colAmazonOrderID_DEV000221.Visible = True
            Me.colAmazonOrderID_DEV000221.VisibleIndex = 0
            Me.colAmazonOrderID_DEV000221.Width = 60
            '
            'colAmazonOrderItemCode_DEV000221
            '
            Me.colAmazonOrderItemCode_DEV000221.Caption = "Item Order ID"
            Me.colAmazonOrderItemCode_DEV000221.FieldName = "AmazonOrderItemCode_DEV000221"
            Me.colAmazonOrderItemCode_DEV000221.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
            Me.colAmazonOrderItemCode_DEV000221.Name = "colAmazonOrderItemCode_DEV000221"
            Me.colAmazonOrderItemCode_DEV000221.OptionsColumn.AllowEdit = False
            Me.colAmazonOrderItemCode_DEV000221.OptionsColumn.FixedWidth = True
            Me.colAmazonOrderItemCode_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colAmazonOrderItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colAmazonOrderItemCode_DEV000221.Visible = True
            Me.colAmazonOrderItemCode_DEV000221.VisibleIndex = 0
            Me.colAmazonOrderItemCode_DEV000221.Width = 160
            '
            'colShipmentID_DEV000221
            '
            Me.colShipmentID_DEV000221.Caption = "Ship/Adjust ID"
            Me.colShipmentID_DEV000221.FieldName = "ShipmentID_DEV000221"
            Me.colShipmentID_DEV000221.Name = "colShipmentID_DEV000221"
            Me.colShipmentID_DEV000221.OptionsColumn.AllowEdit = False
            Me.colShipmentID_DEV000221.OptionsColumn.FixedWidth = True
            Me.colShipmentID_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colShipmentID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colShipmentID_DEV000221.Visible = True
            Me.colShipmentID_DEV000221.VisibleIndex = 1
            Me.colShipmentID_DEV000221.Width = 85
            '
            'colPostedDate_DEV000221
            '
            Me.colPostedDate_DEV000221.Caption = "Posted Date"
            Me.colPostedDate_DEV000221.FieldName = "PostedDate_DEV000221"
            Me.colPostedDate_DEV000221.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.[Date]
            Me.colPostedDate_DEV000221.Name = "colPostedDate_DEV000221"
            Me.colPostedDate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colPostedDate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colPostedDate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colPostedDate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colPostedDate_DEV000221.Visible = True
            Me.colPostedDate_DEV000221.VisibleIndex = 3
            '
            'colItemSKU_DEV000221
            '
            Me.colItemSKU_DEV000221.Caption = "Item SKU"
            Me.colItemSKU_DEV000221.FieldName = "ItemSKU_DEV000221"
            Me.colItemSKU_DEV000221.Name = "colItemSKU_DEV000221"
            Me.colItemSKU_DEV000221.OptionsColumn.AllowEdit = False
            Me.colItemSKU_DEV000221.OptionsColumn.FixedWidth = True
            Me.colItemSKU_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colItemSKU_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colItemSKU_DEV000221.Visible = True
            Me.colItemSKU_DEV000221.VisibleIndex = 2
            '
            'colItemQuantity_DEV000221
            '
            Me.colItemQuantity_DEV000221.Caption = "Qty"
            Me.colItemQuantity_DEV000221.FieldName = "ItemQuantity_DEV000221"
            Me.colItemQuantity_DEV000221.Name = "colItemQuantity_DEV000221"
            Me.colItemQuantity_DEV000221.OptionsColumn.AllowEdit = False
            Me.colItemQuantity_DEV000221.OptionsColumn.FixedWidth = True
            Me.colItemQuantity_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colItemQuantity_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colItemQuantity_DEV000221.Visible = True
            Me.colItemQuantity_DEV000221.VisibleIndex = 3
            Me.colItemQuantity_DEV000221.Width = 40
            '
            'colFulfillmentType_DEV000221
            '
            Me.colFulfillmentType_DEV000221.Caption = "Fulfillment"
            Me.colFulfillmentType_DEV000221.FieldName = "FulfillmentType_DEV000221"
            Me.colFulfillmentType_DEV000221.Name = "colFulfillmentType_DEV000221"
            Me.colFulfillmentType_DEV000221.OptionsColumn.AllowEdit = False
            Me.colFulfillmentType_DEV000221.OptionsColumn.FixedWidth = True
            Me.colFulfillmentType_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colFulfillmentType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colFulfillmentType_DEV000221.Visible = True
            Me.colFulfillmentType_DEV000221.VisibleIndex = 4
            Me.colFulfillmentType_DEV000221.Width = 60
            '
            'colCurrencyCode_DEV000221
            '
            Me.colCurrencyCode_DEV000221.Caption = "Currency"
            Me.colCurrencyCode_DEV000221.FieldName = "CurrencyCode_DEV000221"
            Me.colCurrencyCode_DEV000221.Name = "colCurrencyCode_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colCurrencyCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colExchangeRate_DEV000221
            '
            Me.colExchangeRate_DEV000221.Caption = "Exchange Rate"
            Me.colExchangeRate_DEV000221.FieldName = "ExchangeRate_DEV000221"
            Me.colExchangeRate_DEV000221.Name = "colExchangeRate_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colExchangeRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colPrincipalAmountRate_DEV000221
            '
            Me.colPrincipalAmountRate_DEV000221.Caption = "Principal"
            Me.colPrincipalAmountRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colPrincipalAmountRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colPrincipalAmountRate_DEV000221.FieldName = "PrincipalAmountRate_DEV000221"
            Me.colPrincipalAmountRate_DEV000221.Name = "colPrincipalAmountRate_DEV000221"
            Me.colPrincipalAmountRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colPrincipalAmountRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colPrincipalAmountRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colPrincipalAmountRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colPrincipalAmountRate_DEV000221.Visible = True
            Me.colPrincipalAmountRate_DEV000221.VisibleIndex = 5
            Me.colPrincipalAmountRate_DEV000221.Width = 60
            '
            'colPrincipalAmount_DEV000221
            '
            Me.colPrincipalAmount_DEV000221.Caption = "Principal Amount"
            Me.colPrincipalAmount_DEV000221.FieldName = "PrincipalAmount_DEV000221"
            Me.colPrincipalAmount_DEV000221.Name = "colPrincipalAmount_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colPrincipalAmount_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colShippingAmountRate_DEV000221
            '
            Me.colShippingAmountRate_DEV000221.Caption = "Shipping"
            Me.colShippingAmountRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colShippingAmountRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colShippingAmountRate_DEV000221.FieldName = "ShippingAmountRate_DEV000221"
            Me.colShippingAmountRate_DEV000221.Name = "colShippingAmountRate_DEV000221"
            Me.colShippingAmountRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colShippingAmountRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colShippingAmountRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colShippingAmountRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colShippingAmountRate_DEV000221.Visible = True
            Me.colShippingAmountRate_DEV000221.VisibleIndex = 6
            Me.colShippingAmountRate_DEV000221.Width = 60
            '
            'colShippingAmount_DEV000221
            '
            Me.colShippingAmount_DEV000221.Caption = "Shipping Amount"
            Me.colShippingAmount_DEV000221.FieldName = "ShippingAmount_DEV000221"
            Me.colShippingAmount_DEV000221.Name = "colShippingAmount_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colShippingAmount_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colTaxAmountRate_DEV000221
            '
            Me.colTaxAmountRate_DEV000221.Caption = "Tax"
            Me.colTaxAmountRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colTaxAmountRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colTaxAmountRate_DEV000221.FieldName = "TaxAmountRate_DEV000221"
            Me.colTaxAmountRate_DEV000221.Name = "colTaxAmountRate_DEV000221"
            Me.colTaxAmountRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colTaxAmountRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colTaxAmountRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colTaxAmountRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colTaxAmountRate_DEV000221.Visible = True
            Me.colTaxAmountRate_DEV000221.VisibleIndex = 7
            Me.colTaxAmountRate_DEV000221.Width = 50
            '
            'colTaxAmount_DEV000221
            '
            Me.colTaxAmount_DEV000221.Caption = "Tax Amount"
            Me.colTaxAmount_DEV000221.FieldName = "TaxAmount_DEV000221"
            Me.colTaxAmount_DEV000221.Name = "colTaxAmount_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colTaxAmount_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colCommissionAmountRate_DEV000221
            '
            Me.colCommissionAmountRate_DEV000221.Caption = "Commission"
            Me.colCommissionAmountRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colCommissionAmountRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colCommissionAmountRate_DEV000221.FieldName = "CommissionAmountRate_DEV000221"
            Me.colCommissionAmountRate_DEV000221.Name = "colCommissionAmountRate_DEV000221"
            Me.colCommissionAmountRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colCommissionAmountRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colCommissionAmountRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colCommissionAmountRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colCommissionAmountRate_DEV000221.Visible = True
            Me.colCommissionAmountRate_DEV000221.VisibleIndex = 8
            Me.colCommissionAmountRate_DEV000221.Width = 65
            '
            'colCommissionAmount_DEV000221
            '
            Me.colCommissionAmount_DEV000221.Caption = "Commission Amount"
            Me.colCommissionAmount_DEV000221.FieldName = "CommissionAmount_DEV000221"
            Me.colCommissionAmount_DEV000221.Name = "colCommissionAmount_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colCommissionAmount_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colFBAPerOrderFulfillmentFeeRate_DEV000221
            '
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.Caption = "FBA per Order Fee"
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.FieldName = "FBAPerOrderFulfillmentFeeRate_DEV000221"
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.Name = "colFBAPerOrderFulfillmentFeeRate_DEV000221"
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colFBAPerOrderFulfillmentFeeRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.Visible = True
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.VisibleIndex = 9
            Me.colFBAPerOrderFulfillmentFeeRate_DEV000221.Width = 100
            '
            'colFBAPerOrderFulfillmentFee_DEV000221
            '
            Me.colFBAPerOrderFulfillmentFee_DEV000221.Caption = "FBA per Order Fee"
            Me.colFBAPerOrderFulfillmentFee_DEV000221.FieldName = "FBAPerOrderFulfillmentFee_DEV000221"
            Me.colFBAPerOrderFulfillmentFee_DEV000221.Name = "colFBAPerOrderFulfillmentFee_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colFBAPerOrderFulfillmentFee_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colFBAPerUnitFulfillmentFeeRate_DEV000221
            '
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.Caption = "FBA per Unit Fee"
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.FieldName = "FBAPerUnitFulfillmentFeeRate_DEV000221"
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.Name = "colFBAPerUnitFulfillmentFeeRate_DEV000221"
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colFBAPerUnitFulfillmentFeeRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.Visible = True
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.VisibleIndex = 10
            Me.colFBAPerUnitFulfillmentFeeRate_DEV000221.Width = 100
            '
            'colFBAPerUnitFulfillmentFee_DEV000221
            '
            Me.colFBAPerUnitFulfillmentFee_DEV000221.Caption = "FBA per Unit Fee"
            Me.colFBAPerUnitFulfillmentFee_DEV000221.FieldName = "FBAPerUnitFulfillmentFee_DEV000221"
            Me.colFBAPerUnitFulfillmentFee_DEV000221.Name = "colFBAPerUnitFulfillmentFee_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colFBAPerUnitFulfillmentFee_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colFBAWeightBasedFeeRate_DEV000221
            '
            Me.colFBAWeightBasedFeeRate_DEV000221.Caption = "FBA Weight Fee"
            Me.colFBAWeightBasedFeeRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colFBAWeightBasedFeeRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colFBAWeightBasedFeeRate_DEV000221.FieldName = "FBAWeightBasedFeeRate_DEV000221"
            Me.colFBAWeightBasedFeeRate_DEV000221.Name = "colFBAWeightBasedFeeRate_DEV000221"
            Me.colFBAWeightBasedFeeRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colFBAWeightBasedFeeRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colFBAWeightBasedFeeRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colFBAWeightBasedFeeRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colFBAWeightBasedFeeRate_DEV000221.Visible = True
            Me.colFBAWeightBasedFeeRate_DEV000221.VisibleIndex = 11
            Me.colFBAWeightBasedFeeRate_DEV000221.Width = 90
            '
            'colFBAWeightBasedFee_DEV000221
            '
            Me.colFBAWeightBasedFee_DEV000221.Caption = "FBA Weight Fee"
            Me.colFBAWeightBasedFee_DEV000221.FieldName = "FBAWeightBasedFee_DEV000221"
            Me.colFBAWeightBasedFee_DEV000221.Name = "colFBAWeightBasedFee_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colFBAWeightBasedFee_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colSalesTaxServiceFeeRate_DEV000221
            '
            Me.colSalesTaxServiceFeeRate_DEV000221.Caption = "Fee Taxes"
            Me.colSalesTaxServiceFeeRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colSalesTaxServiceFeeRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colSalesTaxServiceFeeRate_DEV000221.FieldName = "SalesTaxServiceFeeRate_DEV000221"
            Me.colSalesTaxServiceFeeRate_DEV000221.Name = "colSalesTaxServiceFeeRate_DEV000221"
            Me.colSalesTaxServiceFeeRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colSalesTaxServiceFeeRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colSalesTaxServiceFeeRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colSalesTaxServiceFeeRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colSalesTaxServiceFeeRate_DEV000221.Visible = True
            Me.colSalesTaxServiceFeeRate_DEV000221.VisibleIndex = 12
            Me.colSalesTaxServiceFeeRate_DEV000221.Width = 60
            '
            'colSalesTaxServiceFee_DEV000221
            '
            Me.colSalesTaxServiceFee_DEV000221.Caption = "Fee Taxes"
            Me.colSalesTaxServiceFee_DEV000221.FieldName = "SalesTaxServiceFee_DEV000221"
            Me.colSalesTaxServiceFee_DEV000221.Name = "colSalesTaxServiceFee_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colSalesTaxServiceFee_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colMerchantPromotionID_DEV000221
            '
            Me.colMerchantPromotionID_DEV000221.Caption = "Promotion ID"
            Me.colMerchantPromotionID_DEV000221.FieldName = "MerchantPromotionID_DEV000221"
            Me.colMerchantPromotionID_DEV000221.Name = "colMerchantPromotionID_DEV000221"
            Me.colMerchantPromotionID_DEV000221.OptionsColumn.AllowEdit = False
            Me.colMerchantPromotionID_DEV000221.OptionsColumn.FixedWidth = True
            Me.colMerchantPromotionID_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colMerchantPromotionID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colMerchantPromotionID_DEV000221.Visible = True
            Me.colMerchantPromotionID_DEV000221.VisibleIndex = 13
            Me.colMerchantPromotionID_DEV000221.Width = 90
            '
            'colPromotionType_DEV000221
            '
            Me.colPromotionType_DEV000221.Caption = "Type"
            Me.colPromotionType_DEV000221.FieldName = "PromotionType_DEV000221"
            Me.colPromotionType_DEV000221.Name = "colPromotionType_DEV000221"
            Me.colPromotionType_DEV000221.OptionsColumn.AllowEdit = False
            Me.colPromotionType_DEV000221.OptionsColumn.FixedWidth = True
            Me.colPromotionType_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colPromotionType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colPromotionType_DEV000221.Visible = True
            Me.colPromotionType_DEV000221.VisibleIndex = 14
            '
            'colPromotionAmountRate_DEV000221
            '
            Me.colPromotionAmountRate_DEV000221.Caption = "Amount"
            Me.colPromotionAmountRate_DEV000221.DisplayFormat.FormatString = "n2"
            Me.colPromotionAmountRate_DEV000221.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric
            Me.colPromotionAmountRate_DEV000221.FieldName = "PromotionAmountRate_DEV000221"
            Me.colPromotionAmountRate_DEV000221.Name = "colPromotionAmountRate_DEV000221"
            Me.colPromotionAmountRate_DEV000221.OptionsColumn.AllowEdit = False
            Me.colPromotionAmountRate_DEV000221.OptionsColumn.FixedWidth = True
            Me.colPromotionAmountRate_DEV000221.OptionsColumn.ReadOnly = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colPromotionAmountRate_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colPromotionAmountRate_DEV000221.Visible = True
            Me.colPromotionAmountRate_DEV000221.VisibleIndex = 15
            Me.colPromotionAmountRate_DEV000221.Width = 60
            '
            'colPromotionAmount_DEV000221
            '
            Me.colPromotionAmount_DEV000221.Caption = "Amount"
            Me.colPromotionAmount_DEV000221.FieldName = "PromotionAmount_DEV000221"
            Me.colPromotionAmount_DEV000221.Name = "colPromotionAmount_DEV000221"
            Me.ExtendControlProperty.SetTextDisplay(Me.colPromotionAmount_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colISSalesOrderCode_DEV000221
            '
            Me.colISSalesOrderCode_DEV000221.Caption = "IS Order Code"
            Me.colISSalesOrderCode_DEV000221.ColumnEdit = Me.repISSalesOrder
            Me.colISSalesOrderCode_DEV000221.FieldName = "ISSalesOrderCode_DEV000221"
            Me.colISSalesOrderCode_DEV000221.Name = "colISSalesOrderCode_DEV000221"
            Me.colISSalesOrderCode_DEV000221.OptionsColumn.FixedWidth = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colISSalesOrderCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colISSalesOrderCode_DEV000221.Visible = True
            Me.colISSalesOrderCode_DEV000221.VisibleIndex = 16
            Me.colISSalesOrderCode_DEV000221.Width = 90
            '
            'repISSalesOrder
            '
            Me.repISSalesOrder.AutoHeight = False
            Me.repISSalesOrder.Name = "repISSalesOrder"
            '
            'colISItemCode_DEV000221
            '
            Me.colISItemCode_DEV000221.Caption = "IS Item Code"
            Me.colISItemCode_DEV000221.ColumnEdit = Me.repISItem
            Me.colISItemCode_DEV000221.FieldName = "ISItemCode_DEV000221"
            Me.colISItemCode_DEV000221.Name = "colISItemCode_DEV000221"
            Me.colISItemCode_DEV000221.OptionsColumn.FixedWidth = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colISItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colISItemCode_DEV000221.Visible = True
            Me.colISItemCode_DEV000221.VisibleIndex = 17
            '
            'repISItem
            '
            Me.repISItem.AutoHeight = False
            Me.repISItem.Name = "repISItem"
            '
            'colISItemLineNum_DEV000221
            '
            Me.colISItemLineNum_DEV000221.Caption = "IS Line Num"
            Me.colISItemLineNum_DEV000221.FieldName = "ISItemLineNum_DEV000221"
            Me.colISItemLineNum_DEV000221.Name = "colISItemLineNum_DEV000221"
            Me.colISItemLineNum_DEV000221.OptionsColumn.FixedWidth = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colISItemLineNum_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colISItemLineNum_DEV000221.Visible = True
            Me.colISItemLineNum_DEV000221.VisibleIndex = 18
            Me.colISItemLineNum_DEV000221.Width = 70
            '
            'colReconciled_DEV000221
            '
            Me.colReconciled_DEV000221.Caption = "Reconciled"
            Me.colReconciled_DEV000221.FieldName = "Reconciled_DEV000221"
            Me.colReconciled_DEV000221.Name = "colReconciled_DEV000221"
            Me.colReconciled_DEV000221.OptionsColumn.FixedWidth = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colReconciled_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colReconciled_DEV000221.Visible = True
            Me.colReconciled_DEV000221.VisibleIndex = 19
            Me.colReconciled_DEV000221.Width = 70
            '
            'colReconciliationComments_DEV000221
            '
            Me.colReconciliationComments_DEV000221.Caption = "Comments"
            Me.colReconciliationComments_DEV000221.ColumnEdit = Me.repComments
            Me.colReconciliationComments_DEV000221.FieldName = "ReconciliationComments_DEV000221"
            Me.colReconciliationComments_DEV000221.Name = "colReconciliationComments_DEV000221"
            Me.colReconciliationComments_DEV000221.OptionsColumn.FixedWidth = True
            Me.ExtendControlProperty.SetTextDisplay(Me.colReconciliationComments_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.colReconciliationComments_DEV000221.Visible = True
            Me.colReconciliationComments_DEV000221.VisibleIndex = 20
            Me.colReconciliationComments_DEV000221.Width = 120
            '
            'repComments
            '
            Me.repComments.AutoHeight = False
            Me.repComments.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.repComments.Name = "repComments"
            '
            'colUserCreated
            '
            Me.colUserCreated.FieldName = "UserCreated"
            Me.colUserCreated.Name = "colUserCreated"
            Me.ExtendControlProperty.SetTextDisplay(Me.colUserCreated, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colDateCreated
            '
            Me.colDateCreated.FieldName = "DateCreated"
            Me.colDateCreated.Name = "colDateCreated"
            Me.ExtendControlProperty.SetTextDisplay(Me.colDateCreated, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colUserModified
            '
            Me.colUserModified.FieldName = "UserModified"
            Me.colUserModified.Name = "colUserModified"
            Me.ExtendControlProperty.SetTextDisplay(Me.colUserModified, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'colDateModified
            '
            Me.colDateModified.FieldName = "DateModified"
            Me.colDateModified.Name = "colDateModified"
            Me.ExtendControlProperty.SetTextDisplay(Me.colDateModified, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'DateEditStartDate
            '
            Me.DateEditStartDate.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.SettlementStartDate_DEV000221", True))
            Me.DateEditStartDate.EditValue = Nothing
            Me.ExtendControlProperty.SetHelpText(Me.DateEditStartDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.DateEditStartDate.Location = New System.Drawing.Point(473, 27)
            Me.DateEditStartDate.MaximumSize = New System.Drawing.Size(100, 22)
            Me.DateEditStartDate.Name = "DateEditStartDate"
            Me.DateEditStartDate.Properties.AutoHeight = False
            Me.DateEditStartDate.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
            Me.DateEditStartDate.Properties.ReadOnly = True
            Me.DateEditStartDate.Properties.VistaTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditStartDate, System.Drawing.Color.Empty)
            Me.DateEditStartDate.Size = New System.Drawing.Size(100, 21)
            Me.DateEditStartDate.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.DateEditStartDate.TabIndex = 6
            Me.DateEditStartDate.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.DateEditStartDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditMerchantName
            '
            Me.TextEditMerchantName.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.MerchantName_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditMerchantName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditMerchantName.Location = New System.Drawing.Point(418, 2)
            Me.TextEditMerchantName.MaximumSize = New System.Drawing.Size(300, 22)
            Me.TextEditMerchantName.Name = "TextEditMerchantName"
            Me.TextEditMerchantName.Properties.AutoHeight = False
            Me.TextEditMerchantName.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditMerchantName, System.Drawing.Color.Empty)
            Me.TextEditMerchantName.Size = New System.Drawing.Size(300, 21)
            Me.TextEditMerchantName.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditMerchantName.TabIndex = 5
            Me.TextEditMerchantName.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditMerchantName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'TextEditMerchantID
            '
            Me.TextEditMerchantID.DataBindings.Add(New System.Windows.Forms.Binding("EditValue", Me.AmazonSettlementSectionGateway, "LerrynImportExportAmazonSettlement_DEV000221.MerchantID_DEV000221", True))
            Me.ExtendControlProperty.SetHelpText(Me.TextEditMerchantID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.TextEditMerchantID.Location = New System.Drawing.Point(117, 2)
            Me.TextEditMerchantID.MaximumSize = New System.Drawing.Size(150, 22)
            Me.TextEditMerchantID.MinimumSize = New System.Drawing.Size(150, 0)
            Me.TextEditMerchantID.Name = "TextEditMerchantID"
            Me.TextEditMerchantID.Properties.AutoHeight = False
            Me.TextEditMerchantID.Properties.ReadOnly = True
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditMerchantID, System.Drawing.Color.Empty)
            Me.TextEditMerchantID.Size = New System.Drawing.Size(150, 21)
            Me.TextEditMerchantID.StyleController = Me.AmazonSettlementSectionExtendedLayout
            Me.TextEditMerchantID.TabIndex = 4
            Me.TextEditMerchantID.TabStop = False
            Me.ExtendControlProperty.SetTextDisplay(Me.TextEditMerchantID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            '
            'AmazonSettlementSectionLayoutGroup
            '
            Me.AmazonSettlementSectionLayoutGroup.CustomizationFormText = "AmazonSettlementSectionLayoutGroup"
            Me.AmazonSettlementSectionLayoutGroup.GroupBordersVisible = False
            Me.AmazonSettlementSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItemEndDate, Me.LayoutControlItemAmazonSettlementID, Me.LayoutControlItemStartDate, Me.LayoutControlItemMerchantName, Me.LayoutControlItemMerchantID, Me.LayoutControlItemSettlementDetail, Me.LayoutControlItemCurrencyCode, Me.LayoutControlItemExchangeRate, Me.LayoutControlItemTotalValue, Me.LayoutControlItemTotalPrincipal, Me.LayoutControlItemTotalShipping, Me.LayoutControlItemTotalSalesTax, Me.LayoutControlItemDepositDate, Me.LayoutControlItemTotalFBAPerOrderFees, Me.LayoutControlItemTotalFBAPerUnitFees, Me.LayoutControlItemTotalFBAWeightFees, Me.LayoutControlItemTotalCommission, Me.LayoutControlItemTotalSubscriptionFees, Me.LayoutControlItemTotalOthers, Me.LayoutControlItemTotalInboundShipping, Me.LayoutControlItemTotalServiceFeeSalesTax, Me.LayoutControlItemTotalGrossSales, Me.LayoutControlItemTotalCharges})
            Me.AmazonSettlementSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
            Me.AmazonSettlementSectionLayoutGroup.Name = "AmazonSettlementSectionLayoutGroup"
            Me.AmazonSettlementSectionLayoutGroup.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
            Me.AmazonSettlementSectionLayoutGroup.Size = New System.Drawing.Size(930, 566)
            Me.AmazonSettlementSectionLayoutGroup.Text = "AmazonSettlementSectionLayoutGroup"
            Me.AmazonSettlementSectionLayoutGroup.TextVisible = False
            '
            'LayoutControlItemEndDate
            '
            Me.LayoutControlItemEndDate.Control = Me.DateEditEndDate
            Me.LayoutControlItemEndDate.CustomizationFormText = "End Date"
            Me.LayoutControlItemEndDate.Location = New System.Drawing.Point(575, 25)
            Me.LayoutControlItemEndDate.Name = "LayoutControlItemEndDate"
            Me.LayoutControlItemEndDate.Padding = New DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2)
            Me.LayoutControlItemEndDate.Size = New System.Drawing.Size(355, 25)
            Me.LayoutControlItemEndDate.Text = "End Date"
            Me.LayoutControlItemEndDate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemEndDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemEndDate.TextSize = New System.Drawing.Size(50, 13)
            Me.LayoutControlItemEndDate.TextToControlDistance = 5
            '
            'LayoutControlItemAmazonSettlementID
            '
            Me.LayoutControlItemAmazonSettlementID.Control = Me.TextEditAmazonSettlementID
            Me.LayoutControlItemAmazonSettlementID.CustomizationFormText = "Amazon Settlement ID"
            Me.LayoutControlItemAmazonSettlementID.Location = New System.Drawing.Point(0, 25)
            Me.LayoutControlItemAmazonSettlementID.Name = "LayoutControlItemAmazonSettlementID"
            Me.LayoutControlItemAmazonSettlementID.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemAmazonSettlementID.Size = New System.Drawing.Size(318, 25)
            Me.LayoutControlItemAmazonSettlementID.Text = "Amazon Settlement ID"
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemAmazonSettlementID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemAmazonSettlementID.TextSize = New System.Drawing.Size(107, 13)
            '
            'LayoutControlItemStartDate
            '
            Me.LayoutControlItemStartDate.Control = Me.DateEditStartDate
            Me.LayoutControlItemStartDate.CustomizationFormText = "Start Date"
            Me.LayoutControlItemStartDate.Location = New System.Drawing.Point(318, 25)
            Me.LayoutControlItemStartDate.Name = "LayoutControlItemStartDate"
            Me.LayoutControlItemStartDate.Padding = New DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2)
            Me.LayoutControlItemStartDate.Size = New System.Drawing.Size(257, 25)
            Me.LayoutControlItemStartDate.Text = "Settlement Period Start Date"
            Me.LayoutControlItemStartDate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemStartDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemStartDate.TextSize = New System.Drawing.Size(140, 13)
            Me.LayoutControlItemStartDate.TextToControlDistance = 5
            '
            'LayoutControlItemMerchantName
            '
            Me.LayoutControlItemMerchantName.Control = Me.TextEditMerchantName
            Me.LayoutControlItemMerchantName.CustomizationFormText = "Merchant Name"
            Me.LayoutControlItemMerchantName.Location = New System.Drawing.Point(318, 0)
            Me.LayoutControlItemMerchantName.MaxSize = New System.Drawing.Size(402, 26)
            Me.LayoutControlItemMerchantName.MinSize = New System.Drawing.Size(152, 24)
            Me.LayoutControlItemMerchantName.Name = "LayoutControlItemMerchantName"
            Me.LayoutControlItemMerchantName.Padding = New DevExpress.XtraLayout.Utils.Padding(10, 2, 2, 2)
            Me.LayoutControlItemMerchantName.Size = New System.Drawing.Size(612, 25)
            Me.LayoutControlItemMerchantName.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItemMerchantName.Text = "Merchant Name"
            Me.LayoutControlItemMerchantName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemMerchantName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemMerchantName.TextSize = New System.Drawing.Size(85, 13)
            Me.LayoutControlItemMerchantName.TextToControlDistance = 5
            '
            'LayoutControlItemMerchantID
            '
            Me.LayoutControlItemMerchantID.Control = Me.TextEditMerchantID
            Me.LayoutControlItemMerchantID.CustomizationFormText = "Merchant ID"
            Me.LayoutControlItemMerchantID.Location = New System.Drawing.Point(0, 0)
            Me.LayoutControlItemMerchantID.MaxSize = New System.Drawing.Size(318, 26)
            Me.LayoutControlItemMerchantID.MinSize = New System.Drawing.Size(269, 24)
            Me.LayoutControlItemMerchantID.Name = "LayoutControlItemMerchantID"
            Me.LayoutControlItemMerchantID.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemMerchantID.Size = New System.Drawing.Size(318, 25)
            Me.LayoutControlItemMerchantID.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItemMerchantID.Text = "Merchant ID"
            Me.LayoutControlItemMerchantID.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemMerchantID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemMerchantID.TextSize = New System.Drawing.Size(107, 13)
            Me.LayoutControlItemMerchantID.TextToControlDistance = 5
            '
            'LayoutControlItemSettlementDetail
            '
            Me.LayoutControlItemSettlementDetail.Control = Me.GridControlSettlementDetail
            Me.LayoutControlItemSettlementDetail.CustomizationFormText = "LayoutControlItemSettlementDetail"
            Me.LayoutControlItemSettlementDetail.Location = New System.Drawing.Point(0, 148)
            Me.LayoutControlItemSettlementDetail.Name = "LayoutControlItemSettlementDetail"
            Me.LayoutControlItemSettlementDetail.Size = New System.Drawing.Size(930, 418)
            Me.LayoutControlItemSettlementDetail.Text = "LayoutControlItemSettlementDetail"
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemSettlementDetail, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemSettlementDetail.TextLocation = DevExpress.Utils.Locations.Top
            Me.LayoutControlItemSettlementDetail.TextSize = New System.Drawing.Size(0, 0)
            Me.LayoutControlItemSettlementDetail.TextToControlDistance = 0
            Me.LayoutControlItemSettlementDetail.TextVisible = False
            '
            'LayoutControlItemCurrencyCode
            '
            Me.LayoutControlItemCurrencyCode.Control = Me.TextEditCurrencyCode
            Me.LayoutControlItemCurrencyCode.CustomizationFormText = "Currency"
            Me.LayoutControlItemCurrencyCode.Location = New System.Drawing.Point(162, 50)
            Me.LayoutControlItemCurrencyCode.MaxSize = New System.Drawing.Size(142, 26)
            Me.LayoutControlItemCurrencyCode.MinSize = New System.Drawing.Size(142, 24)
            Me.LayoutControlItemCurrencyCode.Name = "LayoutControlItemCurrencyCode"
            Me.LayoutControlItemCurrencyCode.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemCurrencyCode.Size = New System.Drawing.Size(142, 24)
            Me.LayoutControlItemCurrencyCode.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom
            Me.LayoutControlItemCurrencyCode.Text = "Currency"
            Me.LayoutControlItemCurrencyCode.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemCurrencyCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemCurrencyCode.TextSize = New System.Drawing.Size(50, 13)
            Me.LayoutControlItemCurrencyCode.TextToControlDistance = 5
            '
            'LayoutControlItemExchangeRate
            '
            Me.LayoutControlItemExchangeRate.Control = Me.TextEditExchangeRate
            Me.LayoutControlItemExchangeRate.CustomizationFormText = "Exchange Rate"
            Me.LayoutControlItemExchangeRate.Location = New System.Drawing.Point(304, 50)
            Me.LayoutControlItemExchangeRate.Name = "LayoutControlItemExchangeRate"
            Me.LayoutControlItemExchangeRate.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemExchangeRate.Size = New System.Drawing.Size(177, 24)
            Me.LayoutControlItemExchangeRate.Text = "Exchange Rate"
            Me.LayoutControlItemExchangeRate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemExchangeRate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemExchangeRate.TextSize = New System.Drawing.Size(85, 13)
            Me.LayoutControlItemExchangeRate.TextToControlDistance = 5
            '
            'LayoutControlItemTotalValue
            '
            Me.LayoutControlItemTotalValue.Control = Me.TextEditTotalValue
            Me.LayoutControlItemTotalValue.CustomizationFormText = "Total Value"
            Me.LayoutControlItemTotalValue.Location = New System.Drawing.Point(0, 50)
            Me.LayoutControlItemTotalValue.Name = "LayoutControlItemTotalValue"
            Me.LayoutControlItemTotalValue.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalValue.Size = New System.Drawing.Size(162, 24)
            Me.LayoutControlItemTotalValue.Text = "Total Value"
            Me.LayoutControlItemTotalValue.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalValue.TextSize = New System.Drawing.Size(70, 13)
            Me.LayoutControlItemTotalValue.TextToControlDistance = 5
            '
            'LayoutControlItemTotalPrincipal
            '
            Me.LayoutControlItemTotalPrincipal.Control = Me.TextEditTotalPrincipal
            Me.LayoutControlItemTotalPrincipal.CustomizationFormText = "Total Principal"
            Me.LayoutControlItemTotalPrincipal.Location = New System.Drawing.Point(0, 74)
            Me.LayoutControlItemTotalPrincipal.Name = "LayoutControlItemTotalPrincipal"
            Me.LayoutControlItemTotalPrincipal.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalPrincipal.Size = New System.Drawing.Size(162, 25)
            Me.LayoutControlItemTotalPrincipal.Text = "Total Principal"
            Me.LayoutControlItemTotalPrincipal.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalPrincipal, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalPrincipal.TextSize = New System.Drawing.Size(70, 20)
            Me.LayoutControlItemTotalPrincipal.TextToControlDistance = 5
            '
            'LayoutControlItemTotalShipping
            '
            Me.LayoutControlItemTotalShipping.Control = Me.TextEditTotalShipping
            Me.LayoutControlItemTotalShipping.CustomizationFormText = "Shipping"
            Me.LayoutControlItemTotalShipping.Location = New System.Drawing.Point(162, 74)
            Me.LayoutControlItemTotalShipping.Name = "LayoutControlItemTotalShipping"
            Me.LayoutControlItemTotalShipping.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalShipping.Size = New System.Drawing.Size(142, 25)
            Me.LayoutControlItemTotalShipping.Text = "Shipping"
            Me.LayoutControlItemTotalShipping.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalShipping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalShipping.TextSize = New System.Drawing.Size(50, 20)
            Me.LayoutControlItemTotalShipping.TextToControlDistance = 5
            '
            'LayoutControlItemTotalSalesTax
            '
            Me.LayoutControlItemTotalSalesTax.Control = Me.TextEditTotalSalesTax
            Me.LayoutControlItemTotalSalesTax.CustomizationFormText = "LayoutControlItem1"
            Me.LayoutControlItemTotalSalesTax.Location = New System.Drawing.Point(304, 74)
            Me.LayoutControlItemTotalSalesTax.Name = "LayoutControlItemTotalSalesTax"
            Me.LayoutControlItemTotalSalesTax.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalSalesTax.Size = New System.Drawing.Size(177, 25)
            Me.LayoutControlItemTotalSalesTax.Text = "Sales Tax"
            Me.LayoutControlItemTotalSalesTax.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalSalesTax, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalSalesTax.TextSize = New System.Drawing.Size(85, 13)
            Me.LayoutControlItemTotalSalesTax.TextToControlDistance = 5
            '
            'LayoutControlItemDepositDate
            '
            Me.LayoutControlItemDepositDate.Control = Me.DateEditDepositDate
            Me.LayoutControlItemDepositDate.CustomizationFormText = "Deposit Date"
            Me.LayoutControlItemDepositDate.Location = New System.Drawing.Point(481, 50)
            Me.LayoutControlItemDepositDate.Name = "LayoutControlItemDepositDate"
            Me.LayoutControlItemDepositDate.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemDepositDate.Size = New System.Drawing.Size(449, 24)
            Me.LayoutControlItemDepositDate.Text = "Deposit Date"
            Me.LayoutControlItemDepositDate.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemDepositDate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemDepositDate.TextSize = New System.Drawing.Size(80, 13)
            Me.LayoutControlItemDepositDate.TextToControlDistance = 5
            '
            'LayoutControlItemTotalFBAPerOrderFees
            '
            Me.LayoutControlItemTotalFBAPerOrderFees.Control = Me.TextEditTotalFBAPerOrderFees
            Me.LayoutControlItemTotalFBAPerOrderFees.CustomizationFormText = "FBA per Order Fees"
            Me.LayoutControlItemTotalFBAPerOrderFees.Location = New System.Drawing.Point(0, 99)
            Me.LayoutControlItemTotalFBAPerOrderFees.Name = "LayoutControlItemTotalFBAPerOrderFees"
            Me.LayoutControlItemTotalFBAPerOrderFees.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalFBAPerOrderFees.Size = New System.Drawing.Size(187, 24)
            Me.LayoutControlItemTotalFBAPerOrderFees.Text = "FBA per Order Fees"
            Me.LayoutControlItemTotalFBAPerOrderFees.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalFBAPerOrderFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalFBAPerOrderFees.TextSize = New System.Drawing.Size(95, 13)
            Me.LayoutControlItemTotalFBAPerOrderFees.TextToControlDistance = 5
            '
            'LayoutControlItemTotalFBAPerUnitFees
            '
            Me.LayoutControlItemTotalFBAPerUnitFees.Control = Me.TextEditTotalFBAPerUnitFees
            Me.LayoutControlItemTotalFBAPerUnitFees.CustomizationFormText = "FBA per Unit Fees"
            Me.LayoutControlItemTotalFBAPerUnitFees.Location = New System.Drawing.Point(187, 99)
            Me.LayoutControlItemTotalFBAPerUnitFees.Name = "LayoutControlItemTotalFBAPerUnitFees"
            Me.LayoutControlItemTotalFBAPerUnitFees.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalFBAPerUnitFees.Size = New System.Drawing.Size(177, 24)
            Me.LayoutControlItemTotalFBAPerUnitFees.Text = "FBA per Unit Fees"
            Me.LayoutControlItemTotalFBAPerUnitFees.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalFBAPerUnitFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalFBAPerUnitFees.TextSize = New System.Drawing.Size(85, 20)
            Me.LayoutControlItemTotalFBAPerUnitFees.TextToControlDistance = 5
            '
            'LayoutControlItemTotalFBAWeightFees
            '
            Me.LayoutControlItemTotalFBAWeightFees.Control = Me.TextEditTotalFBAWeightFees
            Me.LayoutControlItemTotalFBAWeightFees.CustomizationFormText = "FBA Weight Fees"
            Me.LayoutControlItemTotalFBAWeightFees.Location = New System.Drawing.Point(364, 99)
            Me.LayoutControlItemTotalFBAWeightFees.Name = "LayoutControlItemTotalFBAWeightFees"
            Me.LayoutControlItemTotalFBAWeightFees.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalFBAWeightFees.Size = New System.Drawing.Size(177, 24)
            Me.LayoutControlItemTotalFBAWeightFees.Text = "FBA Weight Fees"
            Me.LayoutControlItemTotalFBAWeightFees.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalFBAWeightFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalFBAWeightFees.TextSize = New System.Drawing.Size(85, 20)
            Me.LayoutControlItemTotalFBAWeightFees.TextToControlDistance = 5
            '
            'LayoutControlItemTotalCommission
            '
            Me.LayoutControlItemTotalCommission.Control = Me.TextEditTotalCommission
            Me.LayoutControlItemTotalCommission.CustomizationFormText = "Commission"
            Me.LayoutControlItemTotalCommission.Location = New System.Drawing.Point(0, 123)
            Me.LayoutControlItemTotalCommission.Name = "LayoutControlItemTotalCommission"
            Me.LayoutControlItemTotalCommission.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalCommission.Size = New System.Drawing.Size(187, 25)
            Me.LayoutControlItemTotalCommission.Text = "Commission"
            Me.LayoutControlItemTotalCommission.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalCommission, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalCommission.TextSize = New System.Drawing.Size(95, 20)
            Me.LayoutControlItemTotalCommission.TextToControlDistance = 5
            '
            'LayoutControlItemTotalSubscriptionFees
            '
            Me.LayoutControlItemTotalSubscriptionFees.Control = Me.TextEditTotalSubscriptionFees
            Me.LayoutControlItemTotalSubscriptionFees.CustomizationFormText = "Subscription Fees"
            Me.LayoutControlItemTotalSubscriptionFees.Location = New System.Drawing.Point(187, 123)
            Me.LayoutControlItemTotalSubscriptionFees.Name = "LayoutControlItemTotalSubscriptionFees"
            Me.LayoutControlItemTotalSubscriptionFees.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalSubscriptionFees.Size = New System.Drawing.Size(177, 25)
            Me.LayoutControlItemTotalSubscriptionFees.Text = "Subscription Fees"
            Me.LayoutControlItemTotalSubscriptionFees.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalSubscriptionFees, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalSubscriptionFees.TextSize = New System.Drawing.Size(85, 20)
            Me.LayoutControlItemTotalSubscriptionFees.TextToControlDistance = 5
            '
            'LayoutControlItemTotalOthers
            '
            Me.LayoutControlItemTotalOthers.Control = Me.TextEditTotalOthers
            Me.LayoutControlItemTotalOthers.CustomizationFormText = "Others Charges"
            Me.LayoutControlItemTotalOthers.Location = New System.Drawing.Point(364, 123)
            Me.LayoutControlItemTotalOthers.Name = "LayoutControlItemTotalOthers"
            Me.LayoutControlItemTotalOthers.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalOthers.Size = New System.Drawing.Size(177, 25)
            Me.LayoutControlItemTotalOthers.Text = "Others Charges"
            Me.LayoutControlItemTotalOthers.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalOthers, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalOthers.TextSize = New System.Drawing.Size(85, 20)
            Me.LayoutControlItemTotalOthers.TextToControlDistance = 5
            '
            'LayoutControlItemTotalInboundShipping
            '
            Me.LayoutControlItemTotalInboundShipping.Control = Me.TextEditTotalInboundShipping
            Me.LayoutControlItemTotalInboundShipping.CustomizationFormText = "Inbound Shipping Charges"
            Me.LayoutControlItemTotalInboundShipping.Location = New System.Drawing.Point(541, 99)
            Me.LayoutControlItemTotalInboundShipping.Name = "LayoutControlItemTotalInboundShipping"
            Me.LayoutControlItemTotalInboundShipping.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalInboundShipping.Size = New System.Drawing.Size(389, 24)
            Me.LayoutControlItemTotalInboundShipping.Text = "Inbound Shipping Charges"
            Me.LayoutControlItemTotalInboundShipping.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalInboundShipping, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalInboundShipping.TextSize = New System.Drawing.Size(130, 20)
            Me.LayoutControlItemTotalInboundShipping.TextToControlDistance = 5
            '
            'LayoutControlItemTotalServiceFeeSalesTax
            '
            Me.LayoutControlItemTotalServiceFeeSalesTax.Control = Me.TextEditTotalServiceFeeSalesTax
            Me.LayoutControlItemTotalServiceFeeSalesTax.CustomizationFormText = "Service Fee Sales Tax"
            Me.LayoutControlItemTotalServiceFeeSalesTax.Location = New System.Drawing.Point(541, 123)
            Me.LayoutControlItemTotalServiceFeeSalesTax.Name = "LayoutControlItemTotalServiceFeeSalesTax"
            Me.LayoutControlItemTotalServiceFeeSalesTax.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalServiceFeeSalesTax.Size = New System.Drawing.Size(222, 25)
            Me.LayoutControlItemTotalServiceFeeSalesTax.Text = "Service Fee Sales Tax"
            Me.LayoutControlItemTotalServiceFeeSalesTax.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalServiceFeeSalesTax, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalServiceFeeSalesTax.TextSize = New System.Drawing.Size(130, 20)
            Me.LayoutControlItemTotalServiceFeeSalesTax.TextToControlDistance = 5
            '
            'LayoutControlItemTotalGrossSales
            '
            Me.LayoutControlItemTotalGrossSales.Control = Me.TextEditTotalGrossSales
            Me.LayoutControlItemTotalGrossSales.CustomizationFormText = "Gross Sales"
            Me.LayoutControlItemTotalGrossSales.Location = New System.Drawing.Point(481, 74)
            Me.LayoutControlItemTotalGrossSales.Name = "LayoutControlItemTotalGrossSales"
            Me.LayoutControlItemTotalGrossSales.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalGrossSales.Size = New System.Drawing.Size(449, 25)
            Me.LayoutControlItemTotalGrossSales.Text = "Gross Sales"
            Me.LayoutControlItemTotalGrossSales.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalGrossSales, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalGrossSales.TextSize = New System.Drawing.Size(80, 20)
            Me.LayoutControlItemTotalGrossSales.TextToControlDistance = 5
            '
            'LayoutControlItemTotalCharges
            '
            Me.LayoutControlItemTotalCharges.Control = Me.TextEditTotalCharges
            Me.LayoutControlItemTotalCharges.CustomizationFormText = "Total Charges"
            Me.LayoutControlItemTotalCharges.Location = New System.Drawing.Point(763, 123)
            Me.LayoutControlItemTotalCharges.Name = "LayoutControlItemTotalCharges"
            Me.LayoutControlItemTotalCharges.Padding = New DevExpress.XtraLayout.Utils.Padding(5, 2, 2, 2)
            Me.LayoutControlItemTotalCharges.Size = New System.Drawing.Size(167, 25)
            Me.LayoutControlItemTotalCharges.Text = "Total Charges"
            Me.LayoutControlItemTotalCharges.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
            Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemTotalCharges, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.LayoutControlItemTotalCharges.TextSize = New System.Drawing.Size(70, 20)
            Me.LayoutControlItemTotalCharges.TextToControlDistance = 5
            '
            'AmazonSettlementSection
            '
            Me.Controls.Add(Me.AmazonSettlementSectionExtendedLayout)
            Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            Me.Name = "AmazonSettlementSection"
            Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
            Me.Size = New System.Drawing.Size(930, 566)
            Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
            CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmazonSettlementSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmazonSettlementSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
            Me.AmazonSettlementSectionExtendedLayout.ResumeLayout(False)
            CType(Me.TextEditTotalCharges.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalGrossSales.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalServiceFeeSalesTax.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalInboundShipping.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalOthers.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalSubscriptionFees.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalCommission.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalFBAWeightFees.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalFBAPerUnitFees.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalFBAPerOrderFees.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalSalesTax.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalShipping.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalPrincipal.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditExchangeRate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditCurrencyCode.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditDepositDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditDepositDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditTotalValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditAmazonSettlementID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditEndDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditEndDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.GridControlSettlementDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.GridViewSettlementDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.repISSalesOrder, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.repISItem, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.repComments, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditStartDate.Properties.VistaTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.DateEditStartDate.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditMerchantName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextEditMerchantID.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.AmazonSettlementSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemEndDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemAmazonSettlementID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemStartDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemMerchantName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemMerchantID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemSettlementDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemCurrencyCode, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemExchangeRate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalValue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalPrincipal, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalShipping, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalSalesTax, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemDepositDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalFBAPerOrderFees, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalFBAPerUnitFees, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalFBAWeightFees, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalCommission, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalSubscriptionFees, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalOthers, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalInboundShipping, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalServiceFeeSalesTax, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalGrossSales, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LayoutControlItemTotalCharges, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Protected WithEvents AmazonSettlementSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Friend WithEvents AmazonSettlementSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
        Friend WithEvents AmazonSettlementSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
        Friend WithEvents GridControlSettlementDetail As DevExpress.XtraGrid.GridControl
        Friend WithEvents GridViewSettlementDetail As DevExpress.XtraGrid.Views.Grid.GridView
        Friend WithEvents colCounter As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colSettlementCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colAmazonOrderID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colAmazonOrderItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colShipmentID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colPostedDate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colItemSKU_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colItemQuantity_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colFulfillmentType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colPrincipalAmountRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colShippingAmountRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colTaxAmountRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colCommissionAmountRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colFBAPerOrderFulfillmentFeeRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colFBAPerUnitFulfillmentFeeRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colFBAWeightBasedFeeRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colSalesTaxServiceFeeRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colMerchantPromotionID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colPromotionType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colPromotionAmountRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colISSalesOrderCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colISItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colISItemLineNum_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colReconciled_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colUserCreated As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colDateCreated As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colUserModified As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colDateModified As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents DateEditDepositDate As DevExpress.XtraEditors.DateEdit
        Friend WithEvents TextEditTotalValue As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditAmazonSettlementID As DevExpress.XtraEditors.TextEdit
        Friend WithEvents DateEditEndDate As DevExpress.XtraEditors.DateEdit
        Friend WithEvents DateEditStartDate As DevExpress.XtraEditors.DateEdit
        Friend WithEvents TextEditMerchantName As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditMerchantID As DevExpress.XtraEditors.TextEdit
        Friend WithEvents LayoutControlItemEndDate As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemAmazonSettlementID As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemStartDate As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemMerchantName As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemMerchantID As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalValue As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemDepositDate As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemSettlementDetail As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents TextEditExchangeRate As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditCurrencyCode As DevExpress.XtraEditors.TextEdit
        Friend WithEvents LayoutControlItemCurrencyCode As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemExchangeRate As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents colCurrencyCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colExchangeRate_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colPrincipalAmount_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colShippingAmount_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colTaxAmount_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colCommissionAmount_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colFBAPerOrderFulfillmentFee_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colFBAPerUnitFulfillmentFee_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colFBAWeightBasedFee_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colSalesTaxServiceFee_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colPromotionAmount_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colTransactionGroup_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colTransactionType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents colReconciliationComments_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
        Friend WithEvents repISSalesOrder As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
        Friend WithEvents repISItem As DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit
        Friend WithEvents repComments As DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit
        Friend WithEvents TextEditTotalSalesTax As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalShipping As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalPrincipal As DevExpress.XtraEditors.TextEdit
        Friend WithEvents LayoutControlItemTotalPrincipal As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalShipping As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalSalesTax As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents TextEditTotalFBAPerUnitFees As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalFBAPerOrderFees As DevExpress.XtraEditors.TextEdit
        Friend WithEvents LayoutControlItemTotalFBAPerOrderFees As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalFBAPerUnitFees As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents TextEditTotalFBAWeightFees As DevExpress.XtraEditors.TextEdit
        Friend WithEvents LayoutControlItemTotalFBAWeightFees As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents TextEditTotalCharges As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalGrossSales As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalServiceFeeSalesTax As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalInboundShipping As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalOthers As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalSubscriptionFees As DevExpress.XtraEditors.TextEdit
        Friend WithEvents TextEditTotalCommission As DevExpress.XtraEditors.TextEdit
        Friend WithEvents LayoutControlItemTotalCommission As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalSubscriptionFees As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalOthers As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalInboundShipping As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalServiceFeeSalesTax As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalGrossSales As Interprise.Presentation.Base.ExtendedLayoutControlItem
        Friend WithEvents LayoutControlItemTotalCharges As Interprise.Presentation.Base.ExtendedLayoutControlItem

#Region "ImportExportDataset"
        Public ReadOnly Property ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
            Get
                Return Me.m_AmazonSettlementDataset
            End Get
        End Property
#End Region

End Class
End Namespace
#End Region

