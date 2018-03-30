'===============================================================================
' Connected Business SDK
' Copyright © 2012 Interprise Solutions
' All rights reserved.
' 
' Connected Business SDK - Generated Code
'
' This code and information is provided "as is" without warranty
' of any kind, either expressed or implied, including but not
' limited to the implied warranties of merchantability and
' fitness for a particular purpose.
'===============================================================================

Option Explicit On
Option Strict On

#Region " BulkPublisingWizardSectionContainer "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BulkPublishingWizardSectionContainer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BulkPublishingWizardSectionContainer))
        Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
        Dim GridLevelNode2 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
        Me.BulkPublisingWizardSectionContainerGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway()
        Me.GridViewAttributeSetGroups = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colGroupAttributeSetID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeGroupID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeGroupName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridControlAttributeSets = New DevExpress.XtraGrid.GridControl()
        Me.GridViewAttributeSets = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colAttributeSetID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeSetName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeSetTemplate = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridViewAttributeInGroup = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colAttributesInGroupSetID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributesInGroupGroupID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributesInGroupAttributeID = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributesInGroupAttributeName = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributesInGroupSortOrder = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.LayoutControl1 = New DevExpress.XtraLayout.LayoutControl()
        Me.btnAdditionalFilters = New DevExpress.XtraEditors.SimpleButton()
        Me.lblSpacer2 = New DevExpress.XtraEditors.LabelControl()
        Me.cbePublishedOtherInstances = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.lblSpacer1 = New DevExpress.XtraEditors.LabelControl()
        Me.chkPublishedOtherInstances = New DevExpress.XtraEditors.CheckEdit()
        Me.lblAlreadyPublishedNote = New DevExpress.XtraEditors.LabelControl()
        Me.cbeInventoryTypeFilter = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.BaseSearchDashboardItemToPublish = New Interprise.Presentation.Base.Search.BaseSearchDashboard()
        Me.LayoutControlGroup1 = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem2 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemAlreadyPubNote = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutItemPublishedOtherInstances1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutItemPublishedOtherInstances2 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem6 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem3 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItem4 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.BulkPublisingWizardPluginContainerControl = New Interprise.Presentation.Base.PluginContainerControl()
        Me.WizardControlBulkPublish = New Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl()
        Me.TabPageComplete = New DevExpress.XtraTab.XtraTabPage()
        Me.PageDescriptionCollection1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.PageDescriptionCollection()
        Me.TabPageWelcome = New DevExpress.XtraTab.XtraTabPage()
        Me.pnlGetCategoryProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetCategoryProgress = New DevExpress.XtraEditors.LabelControl()
        Me.lblMultipleSiteOrAccount = New DevExpress.XtraEditors.LabelControl()
        Me.lblSiteOrAccount = New DevExpress.XtraEditors.LabelControl()
        Me.lblPublishingTarget = New DevExpress.XtraEditors.LabelControl()
        Me.cbeSiteOrAccount = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.cbePublishingTarget = New DevExpress.XtraEditors.ComboBoxEdit()
        Me.PictureBoxLerrynLogo = New System.Windows.Forms.PictureBox()
        Me.TabPageSelectItems = New DevExpress.XtraTab.XtraTabPage()
        Me.TabPageCategories = New DevExpress.XtraTab.XtraTabPage()
        Me.chkUseMappedCBCategories = New DevExpress.XtraEditors.CheckEdit()
        Me.ImageComboBoxEditAttributeSet = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblSelectAttributeSet = New DevExpress.XtraEditors.LabelControl()
        Me.lblSelectCategories = New DevExpress.XtraEditors.LabelControl()
        Me.TreeListCategories = New DevExpress.XtraTreeList.TreeList()
        Me.colTreeListCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryActive = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListCategoryID = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.rbeISCategoryCode = New Lerryn.Presentation.eShopCONNECT.Search.RepSearchTreeControl(Me.components)
        Me.imagelistCategory = New System.Windows.Forms.ImageList(Me.components)
        Me.TabPageOptions = New DevExpress.XtraTab.XtraTabPage()
        Me.lblMagentoWeightUnits = New DevExpress.XtraEditors.LabelControl()
        Me.cbeMagentoWeightUnits = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblMagentoDescriptionSource = New DevExpress.XtraEditors.LabelControl()
        Me.cbeMagentoDescriptionSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblMagentoShortDescSource = New DevExpress.XtraEditors.LabelControl()
        Me.cbeMagentoShortDescSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.lblSpecialTo = New DevExpress.XtraEditors.LabelControl()
        Me.lblSpecialFrom = New DevExpress.XtraEditors.LabelControl()
        Me.DateEditSpecialTo = New DevExpress.XtraEditors.DateEdit()
        Me.DateEditSpecialFrom = New DevExpress.XtraEditors.DateEdit()
        Me.lblMagentoSpecialPrice = New DevExpress.XtraEditors.LabelControl()
        Me.lblMagentoPrice = New DevExpress.XtraEditors.LabelControl()
        Me.TextEditQtyPublishingValue = New DevExpress.XtraEditors.TextEdit()
        Me.lblQtyPublishingValue = New DevExpress.XtraEditors.LabelControl()
        Me.lblQtyPublishing = New DevExpress.XtraEditors.LabelControl()
        Me.RadioGroupQtyPublishing = New DevExpress.XtraEditors.RadioGroup()
        Me.cbeTargetPriceSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.cbeMagentoSpecialPriceSource = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.TabPagePublishing = New DevExpress.XtraTab.XtraTabPage()
        Me.pnlPublishingProductsProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblPublishingProductPleaseWait = New DevExpress.XtraEditors.LabelControl()
        Me.lblPublishingProductProgress = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageAttributes = New DevExpress.XtraTab.XtraTabPage()
        Me.lblAutoFillList1 = New DevExpress.XtraEditors.LabelControl()
        Me.lblAutoFillList2 = New DevExpress.XtraEditors.LabelControl()
        Me.lblAttributeAutoFill = New DevExpress.XtraEditors.LabelControl()
        Me.lblMagentoAttributes = New DevExpress.XtraEditors.LabelControl()
        Me.pnlGetAttributeProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblGetAttributeProgress = New DevExpress.XtraEditors.LabelControl()
        Me.GridControlProperties = New DevExpress.XtraGrid.GridControl()
        Me.GridViewProperties = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colPropertiesItemCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesName_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesDataType_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDataType = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.colPropertiesDateValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesNumericValue_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesTextValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesMemoValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colPropertiesDisplayValue = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeHasSelectValues_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeDateEdit = New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()
        Me.rbeTextEdit = New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
        Me.rbeYesNoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemComboBox()
        Me.rbeMemoEdit = New DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit()
        Me.rbeSelectEdit = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.TabPageCreateCategories = New DevExpress.XtraTab.XtraTabPage()
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl()
        Me.btnCreateCatrgories = New DevExpress.XtraEditors.SimpleButton()
        Me.TabPageAttributeSets = New DevExpress.XtraTab.XtraTabPage()
        Me.btnAddAttribute = New DevExpress.XtraEditors.SimpleButton()
        Me.btnAddGroup = New DevExpress.XtraEditors.SimpleButton()
        Me.btnNewAttributeSet = New DevExpress.XtraEditors.SimpleButton()
        Me.TabPageCreateAndMapAttributes = New DevExpress.XtraTab.XtraTabPage()
        Me.lblAttributeInstructions = New DevExpress.XtraEditors.LabelControl()
        Me.GridControlAttributes = New DevExpress.XtraGrid.GridControl()
        Me.GridViewAttributes = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colPublish = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colAttributeDescription = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colIsActive = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colInstanceID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceAttributeID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.rbeSourceAttribute = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.colSourceAttributeName_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colUsedInMatrixGroups = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TabPageMapCategories = New DevExpress.XtraTab.XtraTabPage()
        Me.lblSelectCatgoryMapping = New DevExpress.XtraEditors.LabelControl()
        Me.lblMappingNotes = New DevExpress.XtraEditors.LabelControl()
        Me.TreeListCategoryMapping = New DevExpress.XtraTreeList.TreeList()
        Me.colTargetCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListTargetCategoryID = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListISCategoryCode = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colTreeListISCategoryName = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TabPagePublishingAttributes = New DevExpress.XtraTab.XtraTabPage()
        Me.lblAttributePublishingErrors = New DevExpress.XtraEditors.LabelControl()
        Me.pnlPublishingAttributesProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblPublishingAttributesPleaseWait = New DevExpress.XtraEditors.LabelControl()
        Me.lblPublishingAttributeProgress = New DevExpress.XtraEditors.LabelControl()
        Me.TabPagePublishAttributeSets = New DevExpress.XtraTab.XtraTabPage()
        Me.pnlPublishingAttributeSetsProgress = New DevExpress.XtraEditors.PanelControl()
        Me.lblPublishingAttributeSetsPleaseWait = New DevExpress.XtraEditors.LabelControl()
        Me.lblPublishingAttributeSetProgress = New DevExpress.XtraEditors.LabelControl()
        Me.TabPageShared1 = New Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BulkPublisingWizardSectionContainerGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewAttributeSetGroups, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControlAttributeSets, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewAttributeSets, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewAttributeInGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.LayoutControl1.SuspendLayout()
        CType(Me.cbePublishedOtherInstances.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkPublishedOtherInstances.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeInventoryTypeFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemAlreadyPubNote, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemPublishedOtherInstances1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutItemPublishedOtherInstances2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BulkPublisingWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BulkPublisingWizardPluginContainerControl.SuspendLayout()
        CType(Me.WizardControlBulkPublish, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.WizardControlBulkPublish.SuspendLayout()
        Me.TabPageWelcome.SuspendLayout()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetCategoryProgress.SuspendLayout()
        CType(Me.cbeSiteOrAccount.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbePublishingTarget.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxLerrynLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageSelectItems.SuspendLayout()
        Me.TabPageCategories.SuspendLayout()
        CType(Me.chkUseMappedCBCategories.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ImageComboBoxEditAttributeSet.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeISCategoryCode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageOptions.SuspendLayout()
        CType(Me.cbeMagentoWeightUnits.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeMagentoDescriptionSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeMagentoShortDescSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialTo.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialTo.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialFrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DateEditSpecialFrom.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeTargetPriceSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeMagentoSpecialPriceSource.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPagePublishing.SuspendLayout()
        CType(Me.pnlPublishingProductsProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPublishingProductsProgress.SuspendLayout()
        Me.TabPageAttributes.SuspendLayout()
        CType(Me.pnlGetAttributeProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGetAttributeProgress.SuspendLayout()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeDateEdit.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeSelectEdit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageCreateCategories.SuspendLayout()
        Me.TabPageAttributeSets.SuspendLayout()
        Me.TabPageCreateAndMapAttributes.SuspendLayout()
        CType(Me.GridControlAttributes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewAttributes, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.rbeSourceAttribute, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPageMapCategories.SuspendLayout()
        CType(Me.TreeListCategoryMapping, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPagePublishingAttributes.SuspendLayout()
        CType(Me.pnlPublishingAttributesProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPublishingAttributesProgress.SuspendLayout()
        Me.TabPagePublishAttributeSets.SuspendLayout()
        CType(Me.pnlPublishingAttributeSetsProgress, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPublishingAttributeSetsProgress.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'BulkPublisingWizardSectionContainerGateway
        '
        Me.BulkPublisingWizardSectionContainerGateway.DataSetName = "BulkPublisingWizardSectionContainerDataset"
        Me.BulkPublisingWizardSectionContainerGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'GridViewAttributeSetGroups
        '
        Me.GridViewAttributeSetGroups.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colGroupAttributeSetID, Me.colAttributeGroupID, Me.colAttributeGroupName})
        Me.GridViewAttributeSetGroups.GridControl = Me.GridControlAttributeSets
        Me.GridViewAttributeSetGroups.Name = "GridViewAttributeSetGroups"
        Me.GridViewAttributeSetGroups.OptionsNavigation.UseTabKey = False
        Me.GridViewAttributeSetGroups.OptionsView.ShowGroupPanel = False
        '
        'colGroupAttributeSetID
        '
        Me.colGroupAttributeSetID.Caption = "Attribute Set ID"
        Me.colGroupAttributeSetID.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.AttributeSetID"
        Me.colGroupAttributeSetID.Name = "colGroupAttributeSetID"
        Me.colGroupAttributeSetID.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colGroupAttributeSetID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colGroupAttributeSetID.Width = 20
        '
        'colAttributeGroupID
        '
        Me.colAttributeGroupID.Caption = "Attribute Group ID"
        Me.colAttributeGroupID.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.AttributeGroupID"
        Me.colAttributeGroupID.Name = "colAttributeGroupID"
        Me.colAttributeGroupID.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeGroupID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeGroupID.Width = 20
        '
        'colAttributeGroupName
        '
        Me.colAttributeGroupName.Caption = "Group Name"
        Me.colAttributeGroupName.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.AttributeGroupName"
        Me.colAttributeGroupName.Name = "colAttributeGroupName"
        Me.colAttributeGroupName.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeGroupName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeGroupName.Visible = True
        Me.colAttributeGroupName.VisibleIndex = 0
        '
        'GridControlAttributeSets
        '
        Me.GridControlAttributeSets.DataMember = "MagentoAttributeSets"
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlAttributeSets, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlAttributeSets, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        GridLevelNode2.RelationName = "MagentoAttributeSetGroups_MagentoAttributeGroupAttributes"
        GridLevelNode1.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode2})
        GridLevelNode1.RelationName = "MagentoAttributeSets_MagentoAttributeSetGroups"
        Me.GridControlAttributeSets.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1})
        Me.GridControlAttributeSets.Location = New System.Drawing.Point(31, 92)
        Me.GridControlAttributeSets.MainView = Me.GridViewAttributeSets
        Me.GridControlAttributeSets.Name = "GridControlAttributeSets"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlAttributeSets, System.Drawing.Color.Empty)
        Me.GridControlAttributeSets.Size = New System.Drawing.Size(437, 268)
        Me.GridControlAttributeSets.TabIndex = 1
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlAttributeSets, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlAttributeSets.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewAttributeSets, Me.GridViewAttributeInGroup, Me.GridViewAttributeSetGroups})
        '
        'GridViewAttributeSets
        '
        Me.GridViewAttributeSets.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colAttributeSetID, Me.colAttributeSetName, Me.colAttributeSetTemplate})
        Me.GridViewAttributeSets.GridControl = Me.GridControlAttributeSets
        Me.GridViewAttributeSets.Name = "GridViewAttributeSets"
        Me.GridViewAttributeSets.OptionsView.ShowGroupPanel = False
        '
        'colAttributeSetID
        '
        Me.colAttributeSetID.Caption = "Attribute Set ID"
        Me.colAttributeSetID.FieldName = "AttributeSetID"
        Me.colAttributeSetID.Name = "colAttributeSetID"
        Me.colAttributeSetID.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeSetID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeSetID.Width = 20
        '
        'colAttributeSetName
        '
        Me.colAttributeSetName.Caption = "Attribute Set Name"
        Me.colAttributeSetName.FieldName = "AttributeSetName"
        Me.colAttributeSetName.Name = "colAttributeSetName"
        Me.colAttributeSetName.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeSetName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeSetName.Visible = True
        Me.colAttributeSetName.VisibleIndex = 0
        '
        'colAttributeSetTemplate
        '
        Me.colAttributeSetTemplate.Caption = "Template"
        Me.colAttributeSetTemplate.FieldName = "AttributeSetTemplate"
        Me.colAttributeSetTemplate.Name = "colAttributeSetTemplate"
        Me.colAttributeSetTemplate.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeSetTemplate, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeSetTemplate.Visible = True
        Me.colAttributeSetTemplate.VisibleIndex = 1
        '
        'GridViewAttributeInGroup
        '
        Me.GridViewAttributeInGroup.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colAttributesInGroupSetID, Me.colAttributesInGroupGroupID, Me.colAttributesInGroupAttributeID, Me.colAttributesInGroupAttributeName, Me.colAttributesInGroupSortOrder})
        Me.GridViewAttributeInGroup.GridControl = Me.GridControlAttributeSets
        Me.GridViewAttributeInGroup.Name = "GridViewAttributeInGroup"
        Me.GridViewAttributeInGroup.OptionsView.ShowGroupPanel = False
        '
        'colAttributesInGroupSetID
        '
        Me.colAttributesInGroupSetID.Caption = "Attribute Set ID"
        Me.colAttributesInGroupSetID.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.MagentoAttributeSetGroups_MagentoA" & _
    "ttributeGroupAttributes.AttributeSetID"
        Me.colAttributesInGroupSetID.Name = "colAttributesInGroupSetID"
        Me.colAttributesInGroupSetID.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributesInGroupSetID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributesInGroupSetID.Width = 20
        '
        'colAttributesInGroupGroupID
        '
        Me.colAttributesInGroupGroupID.Caption = "Attribute Group ID"
        Me.colAttributesInGroupGroupID.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.MagentoAttributeSetGroups_MagentoA" & _
    "ttributeGroupAttributes.AttributeGroupID"
        Me.colAttributesInGroupGroupID.Name = "colAttributesInGroupGroupID"
        Me.colAttributesInGroupGroupID.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributesInGroupGroupID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributesInGroupGroupID.Width = 20
        '
        'colAttributesInGroupAttributeID
        '
        Me.colAttributesInGroupAttributeID.Caption = "Attribute ID"
        Me.colAttributesInGroupAttributeID.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.MagentoAttributeSetGroups_MagentoA" & _
    "ttributeGroupAttributes.AttributeID"
        Me.colAttributesInGroupAttributeID.Name = "colAttributesInGroupAttributeID"
        Me.colAttributesInGroupAttributeID.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributesInGroupAttributeID, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributesInGroupAttributeID.Width = 20
        '
        'colAttributesInGroupAttributeName
        '
        Me.colAttributesInGroupAttributeName.Caption = "Attribute Name"
        Me.colAttributesInGroupAttributeName.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.MagentoAttributeSetGroups_MagentoA" & _
    "ttributeGroupAttributes.AttributeName"
        Me.colAttributesInGroupAttributeName.Name = "colAttributesInGroupAttributeName"
        Me.colAttributesInGroupAttributeName.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributesInGroupAttributeName, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributesInGroupAttributeName.Visible = True
        Me.colAttributesInGroupAttributeName.VisibleIndex = 0
        '
        'colAttributesInGroupSortOrder
        '
        Me.colAttributesInGroupSortOrder.Caption = "Sort Order"
        Me.colAttributesInGroupSortOrder.FieldName = "MagentoAttributeSets_MagentoAttributeSetGroups.MagentoAttributeSetGroups_MagentoA" & _
    "ttributeGroupAttributes.SortOrder"
        Me.colAttributesInGroupSortOrder.Name = "colAttributesInGroupSortOrder"
        Me.colAttributesInGroupSortOrder.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributesInGroupSortOrder, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributesInGroupSortOrder.Visible = True
        Me.colAttributesInGroupSortOrder.VisibleIndex = 1
        '
        'LayoutControl1
        '
        Me.LayoutControl1.Controls.Add(Me.btnAdditionalFilters)
        Me.LayoutControl1.Controls.Add(Me.lblSpacer2)
        Me.LayoutControl1.Controls.Add(Me.cbePublishedOtherInstances)
        Me.LayoutControl1.Controls.Add(Me.lblSpacer1)
        Me.LayoutControl1.Controls.Add(Me.chkPublishedOtherInstances)
        Me.LayoutControl1.Controls.Add(Me.lblAlreadyPublishedNote)
        Me.LayoutControl1.Controls.Add(Me.cbeInventoryTypeFilter)
        Me.LayoutControl1.Controls.Add(Me.BaseSearchDashboardItemToPublish)
        Me.LayoutControl1.Location = New System.Drawing.Point(0, 63)
        Me.LayoutControl1.Name = "LayoutControl1"
        Me.LayoutControl1.Root = Me.LayoutControlGroup1
        Me.LayoutControl1.Size = New System.Drawing.Size(969, 463)
        Me.LayoutControl1.TabIndex = 3
        Me.LayoutControl1.Text = "LayoutControl1"
        '
        'btnAdditionalFilters
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnAdditionalFilters, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnAdditionalFilters.Location = New System.Drawing.Point(788, 2)
        Me.btnAdditionalFilters.MaximumSize = New System.Drawing.Size(100, 0)
        Me.btnAdditionalFilters.Name = "btnAdditionalFilters"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnAdditionalFilters, System.Drawing.Color.Empty)
        Me.btnAdditionalFilters.Size = New System.Drawing.Size(100, 22)
        Me.btnAdditionalFilters.StyleController = Me.LayoutControl1
        Me.btnAdditionalFilters.TabIndex = 13
        Me.btnAdditionalFilters.Text = "Additional Filter"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnAdditionalFilters, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSpacer2
        '
        Me.lblSpacer2.Location = New System.Drawing.Point(455, 28)
        Me.lblSpacer2.MinimumSize = New System.Drawing.Size(100, 0)
        Me.lblSpacer2.Name = "lblSpacer2"
        Me.lblSpacer2.Size = New System.Drawing.Size(100, 13)
        Me.lblSpacer2.StyleController = Me.LayoutControl1
        Me.lblSpacer2.TabIndex = 12
        Me.lblSpacer2.Text = " "
        '
        'cbePublishedOtherInstances
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbePublishedOtherInstances, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbePublishedOtherInstances.Location = New System.Drawing.Point(301, 28)
        Me.cbePublishedOtherInstances.MaximumSize = New System.Drawing.Size(150, 0)
        Me.cbePublishedOtherInstances.Name = "cbePublishedOtherInstances"
        Me.cbePublishedOtherInstances.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbePublishedOtherInstances.Properties.Appearance.Options.UseFont = True
        Me.cbePublishedOtherInstances.Properties.AutoHeight = False
        Me.cbePublishedOtherInstances.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbePublishedOtherInstances, System.Drawing.Color.Empty)
        Me.cbePublishedOtherInstances.Size = New System.Drawing.Size(150, 20)
        Me.cbePublishedOtherInstances.StyleController = Me.LayoutControl1
        Me.cbePublishedOtherInstances.TabIndex = 8
        Me.ExtendControlProperty.SetTextDisplay(Me.cbePublishedOtherInstances, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSpacer1
        '
        Me.lblSpacer1.Location = New System.Drawing.Point(734, 2)
        Me.lblSpacer1.MinimumSize = New System.Drawing.Size(50, 0)
        Me.lblSpacer1.Name = "lblSpacer1"
        Me.lblSpacer1.Size = New System.Drawing.Size(50, 13)
        Me.lblSpacer1.StyleController = Me.LayoutControl1
        Me.lblSpacer1.TabIndex = 10
        Me.lblSpacer1.Text = " "
        '
        'chkPublishedOtherInstances
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkPublishedOtherInstances, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkPublishedOtherInstances.Location = New System.Drawing.Point(2, 28)
        Me.chkPublishedOtherInstances.MaximumSize = New System.Drawing.Size(295, 0)
        Me.chkPublishedOtherInstances.Name = "chkPublishedOtherInstances"
        Me.chkPublishedOtherInstances.Properties.AutoHeight = False
        Me.chkPublishedOtherInstances.Properties.Caption = "Only show Items already published to Magento Instance "
        Me.chkPublishedOtherInstances.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkPublishedOtherInstances, System.Drawing.Color.Empty)
        Me.chkPublishedOtherInstances.Size = New System.Drawing.Size(295, 20)
        Me.chkPublishedOtherInstances.StyleController = Me.LayoutControl1
        Me.chkPublishedOtherInstances.TabIndex = 7
        Me.ExtendControlProperty.SetTextDisplay(Me.chkPublishedOtherInstances, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblAlreadyPublishedNote
        '
        Me.lblAlreadyPublishedNote.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblAlreadyPublishedNote.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.lblAlreadyPublishedNote.Location = New System.Drawing.Point(263, 2)
        Me.lblAlreadyPublishedNote.Name = "lblAlreadyPublishedNote"
        Me.lblAlreadyPublishedNote.Padding = New System.Windows.Forms.Padding(0, 2, 0, 0)
        Me.lblAlreadyPublishedNote.Size = New System.Drawing.Size(467, 15)
        Me.lblAlreadyPublishedNote.StyleController = Me.LayoutControl1
        Me.lblAlreadyPublishedNote.TabIndex = 4
        Me.lblAlreadyPublishedNote.Text = "   NOTE: This list will exclude any Items which have already been published to th" & _
    "e selected Target"
        '
        'cbeInventoryTypeFilter
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeInventoryTypeFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeInventoryTypeFilter.Location = New System.Drawing.Point(109, 2)
        Me.cbeInventoryTypeFilter.MaximumSize = New System.Drawing.Size(150, 22)
        Me.cbeInventoryTypeFilter.MinimumSize = New System.Drawing.Size(150, 0)
        Me.cbeInventoryTypeFilter.Name = "cbeInventoryTypeFilter"
        Me.cbeInventoryTypeFilter.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeInventoryTypeFilter.Properties.Appearance.Options.UseFont = True
        Me.cbeInventoryTypeFilter.Properties.AutoHeight = False
        Me.cbeInventoryTypeFilter.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeInventoryTypeFilter.Properties.Items.AddRange(New Object() {"All Inventory Types", "Matrix Groups", "Matrix Items", "Non-Stock Items", "Stock Items", "Service Items"})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeInventoryTypeFilter, System.Drawing.Color.Empty)
        Me.cbeInventoryTypeFilter.Size = New System.Drawing.Size(150, 22)
        Me.cbeInventoryTypeFilter.StyleController = Me.LayoutControl1
        Me.cbeInventoryTypeFilter.TabIndex = 2
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeInventoryTypeFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'BaseSearchDashboardItemToPublish
        '
        Me.BaseSearchDashboardItemToPublish.AdditionalFilter = Nothing
        Me.BaseSearchDashboardItemToPublish.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.BaseSearchDashboardItemToPublish.Appearance.Options.UseBackColor = True
        Me.BaseSearchDashboardItemToPublish.BypassEntityFilter = False
        Me.BaseSearchDashboardItemToPublish.Caption = ""
        Me.BaseSearchDashboardItemToPublish.ChildControls = New Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface(-1) {}
        Me.BaseSearchDashboardItemToPublish.CloseOnRowSelected = True
        Me.BaseSearchDashboardItemToPublish.CurrentMenuAction = Nothing
        Me.BaseSearchDashboardItemToPublish.DateRangeText = Nothing
        Me.BaseSearchDashboardItemToPublish.DisplayField = "VendorCode"
        Me.BaseSearchDashboardItemToPublish.DocumentCode = Nothing
        Me.BaseSearchDashboardItemToPublish.Enabled = False
        Me.BaseSearchDashboardItemToPublish.Entity = Interprise.Framework.Base.[Shared].[Enum].Entity.Vendor
        Me.BaseSearchDashboardItemToPublish.EntityName = Nothing
        Me.BaseSearchDashboardItemToPublish.GroupColumns = Nothing
        Me.ExtendControlProperty.SetHelpText(Me.BaseSearchDashboardItemToPublish, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.BaseSearchDashboardItemToPublish.IsDisposeCurrentFacadeAndDataset = True
        Me.BaseSearchDashboardItemToPublish.IsFixedSize = False
        Me.BaseSearchDashboardItemToPublish.IsMonthEnd = False
        Me.BaseSearchDashboardItemToPublish.IsReadOnly = False
        Me.BaseSearchDashboardItemToPublish.IsWithSaveCounterIDField = False
        Me.BaseSearchDashboardItemToPublish.Location = New System.Drawing.Point(2, 52)
        Me.BaseSearchDashboardItemToPublish.m_grid = Nothing
        Me.BaseSearchDashboardItemToPublish.m_VGrid = Nothing
        Me.BaseSearchDashboardItemToPublish.Margin = New System.Windows.Forms.Padding(0)
        Me.BaseSearchDashboardItemToPublish.MenuBase = Nothing
        Me.BaseSearchDashboardItemToPublish.mnukeyDeleteAll = Nothing
        Me.BaseSearchDashboardItemToPublish.mnukeyDeleteSelected = Nothing
        Me.BaseSearchDashboardItemToPublish.mnukeyVDeleteAll = Nothing
        Me.BaseSearchDashboardItemToPublish.mnukeyVDeleteSelected = Nothing
        Me.BaseSearchDashboardItemToPublish.ModuleClassType = Nothing
        Me.BaseSearchDashboardItemToPublish.Name = "BaseSearchDashboardItemToPublish"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.BaseSearchDashboardItemToPublish, System.Drawing.Color.Empty)
        Me.BaseSearchDashboardItemToPublish.RefreshFindDashboardDelegate = Nothing
        Me.BaseSearchDashboardItemToPublish.ReminderListDataset = Nothing
        Me.BaseSearchDashboardItemToPublish.ReportAction = Interprise.Framework.Base.[Shared].[Enum].ReportAction.None
        Me.BaseSearchDashboardItemToPublish.ReportDetail = Nothing
        Me.BaseSearchDashboardItemToPublish.Ribbon = Nothing
        Me.BaseSearchDashboardItemToPublish.SearchOption = ""
        Me.BaseSearchDashboardItemToPublish.SelectedTopicCode = New System.Guid("00000000-0000-0000-0000-000000000000")
        Me.BaseSearchDashboardItemToPublish.Size = New System.Drawing.Size(965, 409)
        Me.BaseSearchDashboardItemToPublish.SpinEditorText = Nothing
        Me.BaseSearchDashboardItemToPublish.StickRibbonItems = False
        Me.BaseSearchDashboardItemToPublish.TabIndex = 1
        Me.BaseSearchDashboardItemToPublish.TableNames = Nothing
        Me.ExtendControlProperty.SetTextDisplay(Me.BaseSearchDashboardItemToPublish, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.BaseSearchDashboardItemToPublish.UseFilter = False
        Me.BaseSearchDashboardItemToPublish.ZoomState = Interprise.Framework.Base.[Shared].[Enum].ZoomState.ZoomOut
        '
        'LayoutControlGroup1
        '
        Me.LayoutControlGroup1.CustomizationFormText = "LayoutControlGroup1"
        Me.LayoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.[True]
        Me.LayoutControlGroup1.GroupBordersVisible = False
        Me.LayoutControlGroup1.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1, Me.LayoutControlItem2, Me.LayoutControlItemAlreadyPubNote, Me.LayoutItemPublishedOtherInstances1, Me.LayoutItemPublishedOtherInstances2, Me.LayoutControlItem6, Me.LayoutControlItem3, Me.LayoutControlItem4})
        Me.LayoutControlGroup1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlGroup1.Name = "LayoutControlGroup1"
        Me.LayoutControlGroup1.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.LayoutControlGroup1.Size = New System.Drawing.Size(969, 463)
        Me.LayoutControlGroup1.Text = "LayoutControlGroup1"
        Me.LayoutControlGroup1.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.cbeInventoryTypeFilter
        Me.LayoutControlItem1.CustomizationFormText = "Inverntory Item Filter"
        Me.LayoutControlItem1.Drag = False
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.ReadOnly = False
        Me.LayoutControlItem1.Size = New System.Drawing.Size(261, 26)
        Me.LayoutControlItem1.Text = "Inverntory Item Filter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(104, 13)
        '
        'LayoutControlItem2
        '
        Me.LayoutControlItem2.Control = Me.BaseSearchDashboardItemToPublish
        Me.LayoutControlItem2.CustomizationFormText = "LayoutControlItem2"
        Me.LayoutControlItem2.Drag = False
        Me.LayoutControlItem2.Location = New System.Drawing.Point(0, 50)
        Me.LayoutControlItem2.Name = "LayoutControlItem2"
        Me.LayoutControlItem2.ReadOnly = False
        Me.LayoutControlItem2.Size = New System.Drawing.Size(969, 413)
        Me.LayoutControlItem2.Text = "LayoutControlItem2"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem2.TextToControlDistance = 0
        Me.LayoutControlItem2.TextVisible = False
        '
        'LayoutControlItemAlreadyPubNote
        '
        Me.LayoutControlItemAlreadyPubNote.Control = Me.lblAlreadyPublishedNote
        Me.LayoutControlItemAlreadyPubNote.CustomizationFormText = "LayoutControlItemAlreadyPubNote"
        Me.LayoutControlItemAlreadyPubNote.Drag = False
        Me.LayoutControlItemAlreadyPubNote.Location = New System.Drawing.Point(261, 0)
        Me.LayoutControlItemAlreadyPubNote.Name = "LayoutControlItemAlreadyPubNote"
        Me.LayoutControlItemAlreadyPubNote.ReadOnly = False
        Me.LayoutControlItemAlreadyPubNote.Size = New System.Drawing.Size(471, 26)
        Me.LayoutControlItemAlreadyPubNote.Text = "LayoutControlItemAlreadyPubNote"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemAlreadyPubNote, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemAlreadyPubNote.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemAlreadyPubNote.TextToControlDistance = 0
        Me.LayoutControlItemAlreadyPubNote.TextVisible = False
        '
        'LayoutItemPublishedOtherInstances1
        '
        Me.LayoutItemPublishedOtherInstances1.Control = Me.chkPublishedOtherInstances
        Me.LayoutItemPublishedOtherInstances1.CustomizationFormText = "LayoutItemPublishedOtherInstances1"
        Me.LayoutItemPublishedOtherInstances1.Drag = False
        Me.LayoutItemPublishedOtherInstances1.Location = New System.Drawing.Point(0, 26)
        Me.LayoutItemPublishedOtherInstances1.Name = "LayoutItemPublishedOtherInstances1"
        Me.LayoutItemPublishedOtherInstances1.ReadOnly = False
        Me.LayoutItemPublishedOtherInstances1.Size = New System.Drawing.Size(299, 24)
        Me.LayoutItemPublishedOtherInstances1.Text = "LayoutItemPublishedOtherInstances1"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemPublishedOtherInstances1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemPublishedOtherInstances1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutItemPublishedOtherInstances1.TextToControlDistance = 0
        Me.LayoutItemPublishedOtherInstances1.TextVisible = False
        '
        'LayoutItemPublishedOtherInstances2
        '
        Me.LayoutItemPublishedOtherInstances2.Control = Me.cbePublishedOtherInstances
        Me.LayoutItemPublishedOtherInstances2.CustomizationFormText = "LayoutItemPublishedOtherInstances2"
        Me.LayoutItemPublishedOtherInstances2.Drag = False
        Me.LayoutItemPublishedOtherInstances2.Location = New System.Drawing.Point(299, 26)
        Me.LayoutItemPublishedOtherInstances2.Name = "LayoutItemPublishedOtherInstances2"
        Me.LayoutItemPublishedOtherInstances2.ReadOnly = False
        Me.LayoutItemPublishedOtherInstances2.Size = New System.Drawing.Size(154, 24)
        Me.LayoutItemPublishedOtherInstances2.Text = "LayoutItemPublishedOtherInstances2"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutItemPublishedOtherInstances2, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutItemPublishedOtherInstances2.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutItemPublishedOtherInstances2.TextToControlDistance = 0
        Me.LayoutItemPublishedOtherInstances2.TextVisible = False
        '
        'LayoutControlItem6
        '
        Me.LayoutControlItem6.Control = Me.lblSpacer2
        Me.LayoutControlItem6.CustomizationFormText = "LayoutControlItem6"
        Me.LayoutControlItem6.Drag = False
        Me.LayoutControlItem6.Location = New System.Drawing.Point(453, 26)
        Me.LayoutControlItem6.Name = "LayoutControlItem6"
        Me.LayoutControlItem6.ReadOnly = False
        Me.LayoutControlItem6.Size = New System.Drawing.Size(516, 24)
        Me.LayoutControlItem6.Text = "LayoutControlItem6"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem6, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem6.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem6.TextToControlDistance = 0
        Me.LayoutControlItem6.TextVisible = False
        '
        'LayoutControlItem3
        '
        Me.LayoutControlItem3.Control = Me.btnAdditionalFilters
        Me.LayoutControlItem3.CustomizationFormText = "LayoutControlItem3"
        Me.LayoutControlItem3.Drag = False
        Me.LayoutControlItem3.Location = New System.Drawing.Point(786, 0)
        Me.LayoutControlItem3.Name = "LayoutControlItem3"
        Me.LayoutControlItem3.ReadOnly = False
        Me.LayoutControlItem3.Size = New System.Drawing.Size(183, 26)
        Me.LayoutControlItem3.Text = "LayoutControlItem3"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem3, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem3.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem3.TextToControlDistance = 0
        Me.LayoutControlItem3.TextVisible = False
        '
        'LayoutControlItem4
        '
        Me.LayoutControlItem4.Control = Me.lblSpacer1
        Me.LayoutControlItem4.CustomizationFormText = "LayoutControlItem4"
        Me.LayoutControlItem4.Drag = False
        Me.LayoutControlItem4.Location = New System.Drawing.Point(732, 0)
        Me.LayoutControlItem4.Name = "LayoutControlItem4"
        Me.LayoutControlItem4.ReadOnly = False
        Me.LayoutControlItem4.Size = New System.Drawing.Size(54, 26)
        Me.LayoutControlItem4.Text = "LayoutControlItem4"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem4, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem4.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem4.TextToControlDistance = 0
        Me.LayoutControlItem4.TextVisible = False
        '
        'BulkPublisingWizardPluginContainerControl
        '
        Me.BulkPublisingWizardPluginContainerControl.AppearanceCaption.Options.UseTextOptions = True
        Me.BulkPublisingWizardPluginContainerControl.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center
        Me.BulkPublisingWizardPluginContainerControl.BaseLayoutControl = Nothing
        Me.BulkPublisingWizardPluginContainerControl.ContextMenuButtonCaption = Nothing
        Me.BulkPublisingWizardPluginContainerControl.Controls.Add(Me.WizardControlBulkPublish)
        Me.BulkPublisingWizardPluginContainerControl.CurrentControl = Nothing
        Me.BulkPublisingWizardPluginContainerControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BulkPublisingWizardPluginContainerControl.EditorsHeight = 0
        Me.BulkPublisingWizardPluginContainerControl.GroupContextMenu = Nothing
        Me.BulkPublisingWizardPluginContainerControl.HelpCode = Nothing
        Me.BulkPublisingWizardPluginContainerControl.IsCustomTab = False
        Me.BulkPublisingWizardPluginContainerControl.LayoutMode = False
        Me.BulkPublisingWizardPluginContainerControl.Location = New System.Drawing.Point(0, 0)
        Me.BulkPublisingWizardPluginContainerControl.Name = "BulkPublisingWizardPluginContainerControl"
        Me.BulkPublisingWizardPluginContainerControl.OverrideUserRoleMode = False
        Me.BulkPublisingWizardPluginContainerControl.PluginManagerButton = Nothing
        Me.BulkPublisingWizardPluginContainerControl.PluginRows = Nothing
        Me.BulkPublisingWizardPluginContainerControl.SearchPluginButton = Nothing
        Me.BulkPublisingWizardPluginContainerControl.ShowCaption = False
        Me.BulkPublisingWizardPluginContainerControl.Size = New System.Drawing.Size(977, 589)
        Me.BulkPublisingWizardPluginContainerControl.TabIndex = 4
        Me.BulkPublisingWizardPluginContainerControl.Text = "BulkPublisingWizardPluginContainerControl"
        '
        'WizardControlBulkPublish
        '
        Me.WizardControlBulkPublish.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.WizardControlBulkPublish.Appearance.Options.UseBackColor = True
        Me.WizardControlBulkPublish.AppearancePage.Header.Options.UseTextOptions = True
        Me.WizardControlBulkPublish.AppearancePage.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.WizardControlBulkPublish.DisplayFinishButton = False
        Me.WizardControlBulkPublish.DisplayNextButton = True
        Me.WizardControlBulkPublish.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WizardControlBulkPublish.FinishMessage = "Finish message set dynamically"
        Me.WizardControlBulkPublish.FinishPage = Me.TabPageComplete
        Me.WizardControlBulkPublish.IsCustom = False
        Me.WizardControlBulkPublish.IsPlugIn = True
        Me.WizardControlBulkPublish.Location = New System.Drawing.Point(2, 2)
        Me.WizardControlBulkPublish.Name = "WizardControlBulkPublish"
        Me.WizardControlBulkPublish.NextButtonText = "&Finish"
        Me.WizardControlBulkPublish.PageDescription = Me.PageDescriptionCollection1
        Me.WizardControlBulkPublish.SelectedTabPage = Me.TabPageWelcome
        Me.WizardControlBulkPublish.SharedPage = Me.TabPageShared1
        Me.WizardControlBulkPublish.Size = New System.Drawing.Size(973, 585)
        Me.WizardControlBulkPublish.TabIndex = 0
        Me.WizardControlBulkPublish.TabPages.AddRange(New DevExpress.XtraTab.XtraTabPage() {Me.TabPageWelcome, Me.TabPageCreateCategories, Me.TabPageMapCategories, Me.TabPageCreateAndMapAttributes, Me.TabPagePublishingAttributes, Me.TabPageAttributeSets, Me.TabPagePublishAttributeSets, Me.TabPageSelectItems, Me.TabPageCategories, Me.TabPageAttributes, Me.TabPageOptions, Me.TabPagePublishing, Me.TabPageComplete})
        Me.WizardControlBulkPublish.Title = "the Bulk Inventory Publishing"
        Me.WizardControlBulkPublish.WelcomeMessage = "Welcome message set dynamically"
        Me.WizardControlBulkPublish.WelcomePage = Me.TabPageWelcome
        '
        'TabPageComplete
        '
        Me.TabPageComplete.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageComplete.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageComplete.Name = "TabPageComplete"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageComplete, "")
        Me.TabPageComplete.Size = New System.Drawing.Size(967, 527)
        Me.TabPageComplete.Text = "TabPageComplete"
        '
        'TabPageWelcome
        '
        Me.TabPageWelcome.Appearance.PageClient.BackColor = System.Drawing.Color.Violet
        Me.TabPageWelcome.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageWelcome.Controls.Add(Me.pnlGetCategoryProgress)
        Me.TabPageWelcome.Controls.Add(Me.lblMultipleSiteOrAccount)
        Me.TabPageWelcome.Controls.Add(Me.lblSiteOrAccount)
        Me.TabPageWelcome.Controls.Add(Me.lblPublishingTarget)
        Me.TabPageWelcome.Controls.Add(Me.cbeSiteOrAccount)
        Me.TabPageWelcome.Controls.Add(Me.cbePublishingTarget)
        Me.TabPageWelcome.Controls.Add(Me.PictureBoxLerrynLogo)
        Me.TabPageWelcome.Name = "TabPageWelcome"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageWelcome, "")
        Me.TabPageWelcome.Size = New System.Drawing.Size(967, 527)
        Me.TabPageWelcome.Text = "TabPageWelcome"
        '
        'pnlGetCategoryProgress
        '
        Me.pnlGetCategoryProgress.Controls.Add(Me.lblGetCategoryProgress)
        Me.pnlGetCategoryProgress.Location = New System.Drawing.Point(407, 305)
        Me.pnlGetCategoryProgress.Name = "pnlGetCategoryProgress"
        Me.pnlGetCategoryProgress.Size = New System.Drawing.Size(200, 100)
        Me.pnlGetCategoryProgress.TabIndex = 92
        Me.pnlGetCategoryProgress.Visible = False
        '
        'lblGetCategoryProgress
        '
        Me.lblGetCategoryProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblGetCategoryProgress.Location = New System.Drawing.Point(10, 26)
        Me.lblGetCategoryProgress.MinimumSize = New System.Drawing.Size(180, 0)
        Me.lblGetCategoryProgress.Name = "lblGetCategoryProgress"
        Me.lblGetCategoryProgress.Size = New System.Drawing.Size(180, 52)
        Me.lblGetCategoryProgress.TabIndex = 0
        Me.lblGetCategoryProgress.Text = "Getting Magento Categories" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "and Attributes" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please Wait"
        '
        'lblMultipleSiteOrAccount
        '
        Me.lblMultipleSiteOrAccount.Location = New System.Drawing.Point(288, 235)
        Me.lblMultipleSiteOrAccount.Name = "lblMultipleSiteOrAccount"
        Me.lblMultipleSiteOrAccount.Size = New System.Drawing.Size(356, 13)
        Me.lblMultipleSiteOrAccount.TabIndex = 43
        Me.lblMultipleSiteOrAccount.Text = "This Target has multiple Instances/Accounts - Please select the one to use"
        Me.lblMultipleSiteOrAccount.Visible = False
        '
        'lblSiteOrAccount
        '
        Me.lblSiteOrAccount.Location = New System.Drawing.Point(288, 259)
        Me.lblSiteOrAccount.Name = "lblSiteOrAccount"
        Me.lblSiteOrAccount.Size = New System.Drawing.Size(104, 13)
        Me.lblSiteOrAccount.TabIndex = 42
        Me.lblSiteOrAccount.Text = "Instance to publish to" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblSiteOrAccount.Visible = False
        '
        'lblPublishingTarget
        '
        Me.lblPublishingTarget.Location = New System.Drawing.Point(216, 201)
        Me.lblPublishingTarget.Name = "lblPublishingTarget"
        Me.lblPublishingTarget.Size = New System.Drawing.Size(175, 13)
        Me.lblPublishingTarget.TabIndex = 41
        Me.lblPublishingTarget.Text = "Target to publish Inventory Items to" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'cbeSiteOrAccount
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeSiteOrAccount, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSiteOrAccount.Location = New System.Drawing.Point(407, 255)
        Me.cbeSiteOrAccount.Name = "cbeSiteOrAccount"
        Me.cbeSiteOrAccount.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbeSiteOrAccount.Properties.Appearance.Options.UseBackColor = True
        Me.cbeSiteOrAccount.Properties.AutoHeight = False
        Me.cbeSiteOrAccount.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeSiteOrAccount, System.Drawing.Color.Empty)
        Me.cbeSiteOrAccount.Size = New System.Drawing.Size(234, 22)
        Me.cbeSiteOrAccount.TabIndex = 40
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSiteOrAccount, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSiteOrAccount.Visible = False
        '
        'cbePublishingTarget
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbePublishingTarget, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbePublishingTarget.Location = New System.Drawing.Point(407, 197)
        Me.cbePublishingTarget.Name = "cbePublishingTarget"
        Me.cbePublishingTarget.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.cbePublishingTarget.Properties.Appearance.Options.UseBackColor = True
        Me.cbePublishingTarget.Properties.AutoHeight = False
        Me.cbePublishingTarget.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbePublishingTarget, System.Drawing.Color.Empty)
        Me.cbePublishingTarget.Size = New System.Drawing.Size(234, 22)
        Me.cbePublishingTarget.TabIndex = 39
        Me.ExtendControlProperty.SetTextDisplay(Me.cbePublishingTarget, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'PictureBoxLerrynLogo
        '
        Me.PictureBoxLerrynLogo.Image = Global.Lerryn.Presentation.eShopCONNECT.My.Resources.Resources.lerrynlogo
        Me.PictureBoxLerrynLogo.Location = New System.Drawing.Point(753, 450)
        Me.PictureBoxLerrynLogo.Name = "PictureBoxLerrynLogo"
        Me.PictureBoxLerrynLogo.Size = New System.Drawing.Size(164, 54)
        Me.PictureBoxLerrynLogo.TabIndex = 34
        Me.PictureBoxLerrynLogo.TabStop = False
        Me.PictureBoxLerrynLogo.Visible = False
        '
        'TabPageSelectItems
        '
        Me.TabPageSelectItems.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageSelectItems.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageSelectItems.Controls.Add(Me.LayoutControl1)
        Me.TabPageSelectItems.Name = "TabPageSelectItems"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageSelectItems, "")
        Me.TabPageSelectItems.Size = New System.Drawing.Size(967, 527)
        Me.TabPageSelectItems.Text = "Select Items to Publish"
        '
        'TabPageCategories
        '
        Me.TabPageCategories.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageCategories.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageCategories.Controls.Add(Me.chkUseMappedCBCategories)
        Me.TabPageCategories.Controls.Add(Me.ImageComboBoxEditAttributeSet)
        Me.TabPageCategories.Controls.Add(Me.lblSelectAttributeSet)
        Me.TabPageCategories.Controls.Add(Me.lblSelectCategories)
        Me.TabPageCategories.Controls.Add(Me.TreeListCategories)
        Me.TabPageCategories.Name = "TabPageCategories"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageCategories, "")
        Me.TabPageCategories.Size = New System.Drawing.Size(967, 527)
        Me.TabPageCategories.Text = "Magento Categories"
        '
        'chkUseMappedCBCategories
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkUseMappedCBCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkUseMappedCBCategories.Location = New System.Drawing.Point(661, 77)
        Me.chkUseMappedCBCategories.Name = "chkUseMappedCBCategories"
        Me.chkUseMappedCBCategories.Properties.AutoHeight = False
        Me.chkUseMappedCBCategories.Properties.Caption = ""
        Me.chkUseMappedCBCategories.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkUseMappedCBCategories, System.Drawing.Color.Empty)
        Me.chkUseMappedCBCategories.Size = New System.Drawing.Size(24, 22)
        Me.chkUseMappedCBCategories.TabIndex = 95
        Me.ExtendControlProperty.SetTextDisplay(Me.chkUseMappedCBCategories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'ImageComboBoxEditAttributeSet
        '
        Me.ExtendControlProperty.SetHelpText(Me.ImageComboBoxEditAttributeSet, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.ImageComboBoxEditAttributeSet.Location = New System.Drawing.Point(595, 170)
        Me.ImageComboBoxEditAttributeSet.Name = "ImageComboBoxEditAttributeSet"
        Me.ImageComboBoxEditAttributeSet.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.ImageComboBoxEditAttributeSet.Properties.Appearance.Options.UseBackColor = True
        Me.ImageComboBoxEditAttributeSet.Properties.AutoHeight = False
        Me.ImageComboBoxEditAttributeSet.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.ImageComboBoxEditAttributeSet, System.Drawing.Color.Empty)
        Me.ImageComboBoxEditAttributeSet.Size = New System.Drawing.Size(175, 22)
        Me.ImageComboBoxEditAttributeSet.TabIndex = 94
        Me.ExtendControlProperty.SetTextDisplay(Me.ImageComboBoxEditAttributeSet, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSelectAttributeSet
        '
        Me.lblSelectAttributeSet.Location = New System.Drawing.Point(595, 140)
        Me.lblSelectAttributeSet.Name = "lblSelectAttributeSet"
        Me.lblSelectAttributeSet.Size = New System.Drawing.Size(245, 13)
        Me.lblSelectAttributeSet.TabIndex = 93
        Me.lblSelectAttributeSet.Text = "Please select the Magento Attribute Set to be used"
        '
        'lblSelectCategories
        '
        Me.lblSelectCategories.Location = New System.Drawing.Point(16, 80)
        Me.lblSelectCategories.Name = "lblSelectCategories"
        Me.lblSelectCategories.Size = New System.Drawing.Size(639, 13)
        Me.lblSelectCategories.TabIndex = 92
        Me.lblSelectCategories.Text = "Please select the Magento categorie(s) that the products should be listed under, " & _
    "or check the box to use their mapped CB Categories"
        '
        'TreeListCategories
        '
        Me.TreeListCategories.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colTreeListCategoryName, Me.colTreeListCategoryActive, Me.colTreeListCategoryID})
        Me.TreeListCategories.Enabled = False
        Me.TreeListCategories.KeyFieldName = "SourceCategoryID"
        Me.TreeListCategories.Location = New System.Drawing.Point(16, 109)
        Me.TreeListCategories.Name = "TreeListCategories"
        Me.TreeListCategories.ParentFieldName = "SourceParentID"
        Me.TreeListCategories.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeISCategoryCode})
        Me.TreeListCategories.Size = New System.Drawing.Size(462, 412)
        Me.TreeListCategories.TabIndex = 90
        '
        'colTreeListCategoryName
        '
        Me.colTreeListCategoryName.Caption = "Category"
        Me.colTreeListCategoryName.FieldName = "SourceCategoryName"
        Me.colTreeListCategoryName.Name = "colTreeListCategoryName"
        Me.colTreeListCategoryName.OptionsColumn.AllowEdit = False
        Me.colTreeListCategoryName.OptionsColumn.ReadOnly = True
        Me.colTreeListCategoryName.Visible = True
        Me.colTreeListCategoryName.VisibleIndex = 0
        Me.colTreeListCategoryName.Width = 368
        '
        'colTreeListCategoryActive
        '
        Me.colTreeListCategoryActive.Caption = "Active"
        Me.colTreeListCategoryActive.FieldName = "Active"
        Me.colTreeListCategoryActive.Name = "colTreeListCategoryActive"
        Me.colTreeListCategoryActive.Visible = True
        Me.colTreeListCategoryActive.VisibleIndex = 1
        Me.colTreeListCategoryActive.Width = 76
        '
        'colTreeListCategoryID
        '
        Me.colTreeListCategoryID.Caption = "Category ID"
        Me.colTreeListCategoryID.FieldName = "SourceCategoryID"
        Me.colTreeListCategoryID.Name = "colTreeListCategoryID"
        Me.colTreeListCategoryID.Width = 20
        '
        'rbeISCategoryCode
        '
        Me.rbeISCategoryCode.AdditionalFilter = ""
        Me.rbeISCategoryCode.AutoHeight = False
        Me.rbeISCategoryCode.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeISCategoryCode.Columns = New String() {"Description", "ParentCategory"}
        Me.rbeISCategoryCode.DataSource = Nothing
        Me.rbeISCategoryCode.DisplayField = "Description"
        Me.rbeISCategoryCode.KeyField = "CategoryCode"
        Me.rbeISCategoryCode.Movement = Lerryn.Presentation.eShopCONNECT.Search.RepSearchTreeControl.enmMovement.Vertical
        Me.rbeISCategoryCode.Name = "rbeISCategoryCode"
        Me.rbeISCategoryCode.ParentField = "ParentCategory"
        Me.rbeISCategoryCode.RetainValue = False
        Me.rbeISCategoryCode.TableName = "SystemCategoryView"
        Me.rbeISCategoryCode.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.rbeISCategoryCode.TreeImageList = Me.imagelistCategory
        Me.rbeISCategoryCode.ValidateOnEnterKey = True
        '
        'imagelistCategory
        '
        Me.imagelistCategory.ImageStream = CType(resources.GetObject("imagelistCategory.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imagelistCategory.TransparentColor = System.Drawing.Color.Transparent
        Me.imagelistCategory.Images.SetKeyName(0, "ChildCategory.png")
        Me.imagelistCategory.Images.SetKeyName(1, "ParentCategory.png")
        '
        'TabPageOptions
        '
        Me.TabPageOptions.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageOptions.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageOptions.Controls.Add(Me.lblMagentoWeightUnits)
        Me.TabPageOptions.Controls.Add(Me.cbeMagentoWeightUnits)
        Me.TabPageOptions.Controls.Add(Me.lblMagentoDescriptionSource)
        Me.TabPageOptions.Controls.Add(Me.cbeMagentoDescriptionSource)
        Me.TabPageOptions.Controls.Add(Me.lblMagentoShortDescSource)
        Me.TabPageOptions.Controls.Add(Me.cbeMagentoShortDescSource)
        Me.TabPageOptions.Controls.Add(Me.lblSpecialTo)
        Me.TabPageOptions.Controls.Add(Me.lblSpecialFrom)
        Me.TabPageOptions.Controls.Add(Me.DateEditSpecialTo)
        Me.TabPageOptions.Controls.Add(Me.DateEditSpecialFrom)
        Me.TabPageOptions.Controls.Add(Me.lblMagentoSpecialPrice)
        Me.TabPageOptions.Controls.Add(Me.lblMagentoPrice)
        Me.TabPageOptions.Controls.Add(Me.TextEditQtyPublishingValue)
        Me.TabPageOptions.Controls.Add(Me.lblQtyPublishingValue)
        Me.TabPageOptions.Controls.Add(Me.lblQtyPublishing)
        Me.TabPageOptions.Controls.Add(Me.RadioGroupQtyPublishing)
        Me.TabPageOptions.Controls.Add(Me.cbeTargetPriceSource)
        Me.TabPageOptions.Controls.Add(Me.cbeMagentoSpecialPriceSource)
        Me.TabPageOptions.Name = "TabPageOptions"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageOptions, "")
        Me.TabPageOptions.Size = New System.Drawing.Size(967, 527)
        Me.TabPageOptions.Text = "Options"
        '
        'lblMagentoWeightUnits
        '
        Me.lblMagentoWeightUnits.Location = New System.Drawing.Point(360, 166)
        Me.lblMagentoWeightUnits.Name = "lblMagentoWeightUnits"
        Me.lblMagentoWeightUnits.Size = New System.Drawing.Size(92, 13)
        Me.lblMagentoWeightUnits.TabIndex = 113
        Me.lblMagentoWeightUnits.Text = "Magento Weight is "
        '
        'cbeMagentoWeightUnits
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeMagentoWeightUnits, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeMagentoWeightUnits.Location = New System.Drawing.Point(458, 162)
        Me.cbeMagentoWeightUnits.Name = "cbeMagentoWeightUnits"
        Me.cbeMagentoWeightUnits.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeMagentoWeightUnits.Properties.Appearance.Options.UseFont = True
        Me.cbeMagentoWeightUnits.Properties.AutoHeight = False
        Me.cbeMagentoWeightUnits.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeMagentoWeightUnits.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Pounds (lbs)", "L", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Kilograms (Kg)", "K", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeMagentoWeightUnits, System.Drawing.Color.Empty)
        Me.cbeMagentoWeightUnits.Size = New System.Drawing.Size(137, 22)
        Me.cbeMagentoWeightUnits.TabIndex = 114
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeMagentoWeightUnits, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblMagentoDescriptionSource
        '
        Me.lblMagentoDescriptionSource.Location = New System.Drawing.Point(278, 128)
        Me.lblMagentoDescriptionSource.Name = "lblMagentoDescriptionSource"
        Me.lblMagentoDescriptionSource.Size = New System.Drawing.Size(174, 13)
        Me.lblMagentoDescriptionSource.TabIndex = 111
        Me.lblMagentoDescriptionSource.Text = "Magento Description to be set as CB"
        '
        'cbeMagentoDescriptionSource
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeMagentoDescriptionSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeMagentoDescriptionSource.Location = New System.Drawing.Point(458, 124)
        Me.cbeMagentoDescriptionSource.Name = "cbeMagentoDescriptionSource"
        Me.cbeMagentoDescriptionSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeMagentoDescriptionSource.Properties.Appearance.Options.UseFont = True
        Me.cbeMagentoDescriptionSource.Properties.AutoHeight = False
        Me.cbeMagentoDescriptionSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeMagentoDescriptionSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Leave Blank", "B", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Item Ext Description", "D", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Item Web Option Description", "W", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeMagentoDescriptionSource, System.Drawing.Color.Empty)
        Me.cbeMagentoDescriptionSource.Size = New System.Drawing.Size(137, 22)
        Me.cbeMagentoDescriptionSource.TabIndex = 112
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeMagentoDescriptionSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblMagentoShortDescSource
        '
        Me.lblMagentoShortDescSource.Location = New System.Drawing.Point(249, 100)
        Me.lblMagentoShortDescSource.Name = "lblMagentoShortDescSource"
        Me.lblMagentoShortDescSource.Size = New System.Drawing.Size(203, 13)
        Me.lblMagentoShortDescSource.TabIndex = 109
        Me.lblMagentoShortDescSource.Text = "Magento Short Description to be set as CB"
        '
        'cbeMagentoShortDescSource
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeMagentoShortDescSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeMagentoShortDescSource.Location = New System.Drawing.Point(458, 96)
        Me.cbeMagentoShortDescSource.Name = "cbeMagentoShortDescSource"
        Me.cbeMagentoShortDescSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeMagentoShortDescSource.Properties.Appearance.Options.UseFont = True
        Me.cbeMagentoShortDescSource.Properties.AutoHeight = False
        Me.cbeMagentoShortDescSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeMagentoShortDescSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Item Ext Description", "D", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Item Web Option Summary", "W", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeMagentoShortDescSource, System.Drawing.Color.Empty)
        Me.cbeMagentoShortDescSource.Size = New System.Drawing.Size(137, 22)
        Me.cbeMagentoShortDescSource.TabIndex = 110
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeMagentoShortDescSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblSpecialTo
        '
        Me.lblSpecialTo.Enabled = False
        Me.lblSpecialTo.Location = New System.Drawing.Point(348, 299)
        Me.lblSpecialTo.Name = "lblSpecialTo"
        Me.lblSpecialTo.Size = New System.Drawing.Size(104, 13)
        Me.lblSpecialTo.TabIndex = 108
        Me.lblSpecialTo.Text = "Special Price active to"
        '
        'lblSpecialFrom
        '
        Me.lblSpecialFrom.Enabled = False
        Me.lblSpecialFrom.Location = New System.Drawing.Point(336, 271)
        Me.lblSpecialFrom.Name = "lblSpecialFrom"
        Me.lblSpecialFrom.Size = New System.Drawing.Size(116, 13)
        Me.lblSpecialFrom.TabIndex = 107
        Me.lblSpecialFrom.Text = "Special Price active from"
        '
        'DateEditSpecialTo
        '
        Me.DateEditSpecialTo.EditValue = Nothing
        Me.DateEditSpecialTo.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditSpecialTo, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditSpecialTo.Location = New System.Drawing.Point(458, 295)
        Me.DateEditSpecialTo.Name = "DateEditSpecialTo"
        Me.DateEditSpecialTo.Properties.AllowMouseWheel = False
        Me.DateEditSpecialTo.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditSpecialTo.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditSpecialTo.Properties.AutoHeight = False
        Me.DateEditSpecialTo.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditSpecialTo.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditSpecialTo, System.Drawing.Color.Empty)
        Me.DateEditSpecialTo.Size = New System.Drawing.Size(85, 22)
        Me.DateEditSpecialTo.TabIndex = 106
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditSpecialTo, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'DateEditSpecialFrom
        '
        Me.DateEditSpecialFrom.EditValue = Nothing
        Me.DateEditSpecialFrom.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.DateEditSpecialFrom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.DateEditSpecialFrom.Location = New System.Drawing.Point(458, 267)
        Me.DateEditSpecialFrom.Name = "DateEditSpecialFrom"
        Me.DateEditSpecialFrom.Properties.AllowMouseWheel = False
        Me.DateEditSpecialFrom.Properties.Appearance.BackColor = System.Drawing.SystemColors.Window
        Me.DateEditSpecialFrom.Properties.Appearance.Options.UseBackColor = True
        Me.DateEditSpecialFrom.Properties.AutoHeight = False
        Me.DateEditSpecialFrom.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DateEditSpecialFrom.Properties.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.DateEditSpecialFrom, System.Drawing.Color.Empty)
        Me.DateEditSpecialFrom.Size = New System.Drawing.Size(85, 22)
        Me.DateEditSpecialFrom.TabIndex = 105
        Me.ExtendControlProperty.SetTextDisplay(Me.DateEditSpecialFrom, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblMagentoSpecialPrice
        '
        Me.lblMagentoSpecialPrice.Enabled = False
        Me.lblMagentoSpecialPrice.Location = New System.Drawing.Point(272, 243)
        Me.lblMagentoSpecialPrice.Name = "lblMagentoSpecialPrice"
        Me.lblMagentoSpecialPrice.Size = New System.Drawing.Size(183, 13)
        Me.lblMagentoSpecialPrice.TabIndex = 103
        Me.lblMagentoSpecialPrice.Text = "Magento Special Price to be set as CB "
        '
        'lblMagentoPrice
        '
        Me.lblMagentoPrice.Location = New System.Drawing.Point(272, 204)
        Me.lblMagentoPrice.Name = "lblMagentoPrice"
        Me.lblMagentoPrice.Size = New System.Drawing.Size(180, 13)
        Me.lblMagentoPrice.TabIndex = 101
        Me.lblMagentoPrice.Text = "Magento Selling Price to be set as CB "
        '
        'TextEditQtyPublishingValue
        '
        Me.TextEditQtyPublishingValue.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TextEditQtyPublishingValue.Location = New System.Drawing.Point(46, 226)
        Me.TextEditQtyPublishingValue.Name = "TextEditQtyPublishingValue"
        Me.TextEditQtyPublishingValue.Properties.AutoHeight = False
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.TextEditQtyPublishingValue, System.Drawing.Color.Empty)
        Me.TextEditQtyPublishingValue.Size = New System.Drawing.Size(75, 22)
        Me.TextEditQtyPublishingValue.TabIndex = 100
        Me.ExtendControlProperty.SetTextDisplay(Me.TextEditQtyPublishingValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'lblQtyPublishingValue
        '
        Me.lblQtyPublishingValue.Enabled = False
        Me.lblQtyPublishingValue.Location = New System.Drawing.Point(46, 207)
        Me.lblQtyPublishingValue.Name = "lblQtyPublishingValue"
        Me.lblQtyPublishingValue.Size = New System.Drawing.Size(75, 13)
        Me.lblQtyPublishingValue.TabIndex = 99
        Me.lblQtyPublishingValue.Text = "Value to Publish"
        '
        'lblQtyPublishing
        '
        Me.lblQtyPublishing.Location = New System.Drawing.Point(26, 100)
        Me.lblQtyPublishing.Name = "lblQtyPublishing"
        Me.lblQtyPublishing.Size = New System.Drawing.Size(121, 13)
        Me.lblQtyPublishing.TabIndex = 98
        Me.lblQtyPublishing.Text = "Stock Quantity Publishing"
        '
        'RadioGroupQtyPublishing
        '
        Me.ExtendControlProperty.SetHelpText(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.RadioGroupQtyPublishing.Location = New System.Drawing.Point(37, 119)
        Me.RadioGroupQtyPublishing.Name = "RadioGroupQtyPublishing"
        Me.RadioGroupQtyPublishing.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("None", "None"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Fixed", "Fixed Value"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Percent", "% of Total")})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.RadioGroupQtyPublishing, System.Drawing.Color.Empty)
        Me.RadioGroupQtyPublishing.Size = New System.Drawing.Size(100, 83)
        Me.RadioGroupQtyPublishing.TabIndex = 97
        Me.ExtendControlProperty.SetTextDisplay(Me.RadioGroupQtyPublishing, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeTargetPriceSource
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeTargetPriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeTargetPriceSource.Location = New System.Drawing.Point(458, 200)
        Me.cbeTargetPriceSource.Name = "cbeTargetPriceSource"
        Me.cbeTargetPriceSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeTargetPriceSource.Properties.Appearance.Options.UseFont = True
        Me.cbeTargetPriceSource.Properties.AutoHeight = False
        Me.cbeTargetPriceSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeTargetPriceSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Retail Price", "R", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Wholesale Price", "W", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Sug Retail Price", "S", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeTargetPriceSource, System.Drawing.Color.Empty)
        Me.cbeTargetPriceSource.Size = New System.Drawing.Size(137, 22)
        Me.cbeTargetPriceSource.TabIndex = 102
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeTargetPriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeMagentoSpecialPriceSource
        '
        Me.ExtendControlProperty.SetHelpText(Me.cbeMagentoSpecialPriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeMagentoSpecialPriceSource.Location = New System.Drawing.Point(458, 239)
        Me.cbeMagentoSpecialPriceSource.Name = "cbeMagentoSpecialPriceSource"
        Me.cbeMagentoSpecialPriceSource.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeMagentoSpecialPriceSource.Properties.Appearance.Options.UseFont = True
        Me.cbeMagentoSpecialPriceSource.Properties.AutoHeight = False
        Me.cbeMagentoSpecialPriceSource.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeMagentoSpecialPriceSource.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.ImageComboBoxItem() {New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Retail Price", "R", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Wholesale Price", "W", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Promotional Price", "P", -1), New DevExpress.XtraEditors.Controls.ImageComboBoxItem("Not Set", "N", -1)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeMagentoSpecialPriceSource, System.Drawing.Color.Empty)
        Me.cbeMagentoSpecialPriceSource.Size = New System.Drawing.Size(137, 22)
        Me.cbeMagentoSpecialPriceSource.TabIndex = 104
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeMagentoSpecialPriceSource, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TabPagePublishing
        '
        Me.TabPagePublishing.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPagePublishing.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPagePublishing.Controls.Add(Me.pnlPublishingProductsProgress)
        Me.TabPagePublishing.Name = "TabPagePublishing"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPagePublishing, "")
        Me.TabPagePublishing.Size = New System.Drawing.Size(967, 527)
        Me.TabPagePublishing.Text = "Publishing"
        '
        'pnlPublishingProductsProgress
        '
        Me.pnlPublishingProductsProgress.Controls.Add(Me.lblPublishingProductPleaseWait)
        Me.pnlPublishingProductsProgress.Controls.Add(Me.lblPublishingProductProgress)
        Me.pnlPublishingProductsProgress.Location = New System.Drawing.Point(363, 259)
        Me.pnlPublishingProductsProgress.Name = "pnlPublishingProductsProgress"
        Me.pnlPublishingProductsProgress.Size = New System.Drawing.Size(240, 100)
        Me.pnlPublishingProductsProgress.TabIndex = 12
        Me.pnlPublishingProductsProgress.Visible = False
        '
        'lblPublishingProductPleaseWait
        '
        Me.lblPublishingProductPleaseWait.Location = New System.Drawing.Point(93, 62)
        Me.lblPublishingProductPleaseWait.Name = "lblPublishingProductPleaseWait"
        Me.lblPublishingProductPleaseWait.Size = New System.Drawing.Size(54, 13)
        Me.lblPublishingProductPleaseWait.TabIndex = 1
        Me.lblPublishingProductPleaseWait.Text = "Please wait"
        '
        'lblPublishingProductProgress
        '
        Me.lblPublishingProductProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblPublishingProductProgress.Location = New System.Drawing.Point(3, 28)
        Me.lblPublishingProductProgress.MinimumSize = New System.Drawing.Size(234, 25)
        Me.lblPublishingProductProgress.Name = "lblPublishingProductProgress"
        Me.lblPublishingProductProgress.Size = New System.Drawing.Size(234, 25)
        Me.lblPublishingProductProgress.TabIndex = 0
        Me.lblPublishingProductProgress.Text = "Publishing Products to"
        '
        'TabPageAttributes
        '
        Me.TabPageAttributes.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageAttributes.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageAttributes.Controls.Add(Me.lblAutoFillList1)
        Me.TabPageAttributes.Controls.Add(Me.lblAutoFillList2)
        Me.TabPageAttributes.Controls.Add(Me.lblAttributeAutoFill)
        Me.TabPageAttributes.Controls.Add(Me.lblMagentoAttributes)
        Me.TabPageAttributes.Controls.Add(Me.pnlGetAttributeProgress)
        Me.TabPageAttributes.Controls.Add(Me.GridControlProperties)
        Me.TabPageAttributes.Name = "TabPageAttributes"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageAttributes, "")
        Me.TabPageAttributes.Size = New System.Drawing.Size(967, 527)
        Me.TabPageAttributes.Text = "Product Attributes"
        '
        'lblAutoFillList1
        '
        Me.lblAutoFillList1.Location = New System.Drawing.Point(564, 126)
        Me.lblAutoFillList1.Name = "lblAutoFillList1"
        Me.lblAutoFillList1.Size = New System.Drawing.Size(22, 13)
        Me.lblAutoFillList1.TabIndex = 97
        Me.lblAutoFillList1.Text = "list 1"
        '
        'lblAutoFillList2
        '
        Me.lblAutoFillList2.Location = New System.Drawing.Point(750, 126)
        Me.lblAutoFillList2.Name = "lblAutoFillList2"
        Me.lblAutoFillList2.Size = New System.Drawing.Size(22, 13)
        Me.lblAutoFillList2.TabIndex = 96
        Me.lblAutoFillList2.Text = "list 2"
        '
        'lblAttributeAutoFill
        '
        Me.lblAttributeAutoFill.Location = New System.Drawing.Point(564, 94)
        Me.lblAttributeAutoFill.Name = "lblAttributeAutoFill"
        Me.lblAttributeAutoFill.Size = New System.Drawing.Size(269, 26)
        Me.lblAttributeAutoFill.TabIndex = 94
        Me.lblAttributeAutoFill.Text = "If left blank, the following Magento Attributes will be " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "automatically populated" & _
    " with their CB Attribute Values :-"
        '
        'lblMagentoAttributes
        '
        Me.lblMagentoAttributes.Enabled = False
        Me.lblMagentoAttributes.Location = New System.Drawing.Point(21, 75)
        Me.lblMagentoAttributes.Name = "lblMagentoAttributes"
        Me.lblMagentoAttributes.Size = New System.Drawing.Size(373, 13)
        Me.lblMagentoAttributes.TabIndex = 93
        Me.lblMagentoAttributes.Text = "Please populate the Magento Product Attributes for the Items to be published"
        '
        'pnlGetAttributeProgress
        '
        Me.pnlGetAttributeProgress.Controls.Add(Me.lblGetAttributeProgress)
        Me.pnlGetAttributeProgress.Location = New System.Drawing.Point(173, 259)
        Me.pnlGetAttributeProgress.Name = "pnlGetAttributeProgress"
        Me.pnlGetAttributeProgress.Size = New System.Drawing.Size(200, 100)
        Me.pnlGetAttributeProgress.TabIndex = 92
        Me.pnlGetAttributeProgress.Visible = False
        '
        'lblGetAttributeProgress
        '
        Me.lblGetAttributeProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblGetAttributeProgress.Location = New System.Drawing.Point(10, 28)
        Me.lblGetAttributeProgress.MinimumSize = New System.Drawing.Size(180, 0)
        Me.lblGetAttributeProgress.Name = "lblGetAttributeProgress"
        Me.lblGetAttributeProgress.Size = New System.Drawing.Size(180, 39)
        Me.lblGetAttributeProgress.TabIndex = 0
        Me.lblGetAttributeProgress.Text = "Getting Magento Attributes" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Please Wait"
        '
        'GridControlProperties
        '
        Me.GridControlProperties.DataMember = "InventoryMagentoTagDetails_DEV000221"
        Me.GridControlProperties.DataSource = Me.BulkPublisingWizardSectionContainerGateway
        Me.GridControlProperties.Enabled = False
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlProperties, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.Location = New System.Drawing.Point(21, 94)
        Me.GridControlProperties.MainView = Me.GridViewProperties
        Me.GridControlProperties.Name = "GridControlProperties"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlProperties, System.Drawing.Color.Empty)
        Me.GridControlProperties.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeDataType, Me.rbeDateEdit, Me.rbeTextEdit, Me.rbeYesNoEdit, Me.rbeMemoEdit, Me.rbeSelectEdit})
        Me.GridControlProperties.Size = New System.Drawing.Size(522, 427)
        Me.GridControlProperties.TabIndex = 91
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlProperties, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlProperties.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewProperties})
        '
        'GridViewProperties
        '
        Me.GridViewProperties.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPropertiesItemCode_DEV000221, Me.colPropertiesName_DEV000221, Me.colPropertiesDataType_DEV000221, Me.colPropertiesDateValue_DEV000221, Me.colPropertiesNumericValue_DEV000221, Me.colPropertiesTextValue, Me.colPropertiesMemoValue, Me.colPropertiesDisplayValue, Me.colAttributeID_DEV000221, Me.colAttributeHasSelectValues_DEV000221})
        Me.GridViewProperties.GridControl = Me.GridControlProperties
        Me.GridViewProperties.Name = "GridViewProperties"
        Me.GridViewProperties.OptionsView.ShowGroupPanel = False
        Me.GridViewProperties.SortInfo.AddRange(New DevExpress.XtraGrid.Columns.GridColumnSortInfo() {New DevExpress.XtraGrid.Columns.GridColumnSortInfo(Me.colPropertiesName_DEV000221, DevExpress.Data.ColumnSortOrder.Ascending)})
        '
        'colPropertiesItemCode_DEV000221
        '
        Me.colPropertiesItemCode_DEV000221.Caption = "Item Code"
        Me.colPropertiesItemCode_DEV000221.FieldName = "ItemCode_DEV000221"
        Me.colPropertiesItemCode_DEV000221.Name = "colPropertiesItemCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesItemCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesItemCode_DEV000221.Width = 20
        '
        'colPropertiesName_DEV000221
        '
        Me.colPropertiesName_DEV000221.Caption = "Name"
        Me.colPropertiesName_DEV000221.FieldName = "TagName_DEV000221"
        Me.colPropertiesName_DEV000221.Name = "colPropertiesName_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesName_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesName_DEV000221.Visible = True
        Me.colPropertiesName_DEV000221.VisibleIndex = 0
        Me.colPropertiesName_DEV000221.Width = 157
        '
        'colPropertiesDataType_DEV000221
        '
        Me.colPropertiesDataType_DEV000221.Caption = "Format"
        Me.colPropertiesDataType_DEV000221.ColumnEdit = Me.rbeDataType
        Me.colPropertiesDataType_DEV000221.FieldName = "TagDataType_DEV000221"
        Me.colPropertiesDataType_DEV000221.Name = "colPropertiesDataType_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDataType_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDataType_DEV000221.Visible = True
        Me.colPropertiesDataType_DEV000221.VisibleIndex = 1
        Me.colPropertiesDataType_DEV000221.Width = 78
        '
        'rbeDataType
        '
        Me.rbeDataType.AutoHeight = False
        Me.rbeDataType.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeDataType.Items.AddRange(New Object() {"Text", "Memo", "Date", "DateTime", "Integer", "Numeric", "Boolean"})
        Me.rbeDataType.Name = "rbeDataType"
        Me.rbeDataType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'colPropertiesDateValue_DEV000221
        '
        Me.colPropertiesDateValue_DEV000221.Caption = "Date Value"
        Me.colPropertiesDateValue_DEV000221.FieldName = "TagDateValue_DEV000221"
        Me.colPropertiesDateValue_DEV000221.Name = "colPropertiesDateValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDateValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDateValue_DEV000221.Width = 20
        '
        'colPropertiesNumericValue_DEV000221
        '
        Me.colPropertiesNumericValue_DEV000221.Caption = "Numeric Value"
        Me.colPropertiesNumericValue_DEV000221.FieldName = "TagNumericValue_DEV000221"
        Me.colPropertiesNumericValue_DEV000221.Name = "colPropertiesNumericValue_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesNumericValue_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesNumericValue_DEV000221.Width = 20
        '
        'colPropertiesTextValue
        '
        Me.colPropertiesTextValue.Caption = "Text Value"
        Me.colPropertiesTextValue.FieldName = "TagTextValue_DEV000221"
        Me.colPropertiesTextValue.Name = "colPropertiesTextValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesTextValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesTextValue.Width = 20
        '
        'colPropertiesMemoValue
        '
        Me.colPropertiesMemoValue.Caption = "Memo Value"
        Me.colPropertiesMemoValue.FieldName = "TagMemoValue_DEV000221"
        Me.colPropertiesMemoValue.Name = "colPropertiesMemoValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesMemoValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesMemoValue.Width = 20
        '
        'colPropertiesDisplayValue
        '
        Me.colPropertiesDisplayValue.Caption = "Value"
        Me.colPropertiesDisplayValue.FieldName = "DisplayedValue"
        Me.colPropertiesDisplayValue.Name = "colPropertiesDisplayValue"
        Me.ExtendControlProperty.SetTextDisplay(Me.colPropertiesDisplayValue, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPropertiesDisplayValue.UnboundType = DevExpress.Data.UnboundColumnType.[String]
        Me.colPropertiesDisplayValue.Visible = True
        Me.colPropertiesDisplayValue.VisibleIndex = 2
        Me.colPropertiesDisplayValue.Width = 275
        '
        'colAttributeID_DEV000221
        '
        Me.colAttributeID_DEV000221.Caption = "Attribute ID"
        Me.colAttributeID_DEV000221.FieldName = "AttributeID_DEV000221"
        Me.colAttributeID_DEV000221.Name = "colAttributeID_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeID_DEV000221.Width = 20
        '
        'colAttributeHasSelectValues_DEV000221
        '
        Me.colAttributeHasSelectValues_DEV000221.Caption = "Attribute Has Select Values"
        Me.colAttributeHasSelectValues_DEV000221.FieldName = "AttributeHasSelectValues_DEV000221"
        Me.colAttributeHasSelectValues_DEV000221.Name = "colAttributeHasSelectValues_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeHasSelectValues_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeHasSelectValues_DEV000221.Width = 20
        '
        'rbeDateEdit
        '
        Me.rbeDateEdit.AutoHeight = False
        Me.rbeDateEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeDateEdit.CalendarTimeProperties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.rbeDateEdit.Name = "rbeDateEdit"
        '
        'rbeTextEdit
        '
        Me.rbeTextEdit.AutoHeight = False
        Me.rbeTextEdit.Name = "rbeTextEdit"
        '
        'rbeYesNoEdit
        '
        Me.rbeYesNoEdit.AutoHeight = False
        Me.rbeYesNoEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeYesNoEdit.Items.AddRange(New Object() {"Y", "N"})
        Me.rbeYesNoEdit.Name = "rbeYesNoEdit"
        Me.rbeYesNoEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
        '
        'rbeMemoEdit
        '
        Me.rbeMemoEdit.Name = "rbeMemoEdit"
        '
        'rbeSelectEdit
        '
        Me.rbeSelectEdit.AdditionalFilter = Nothing
        Me.rbeSelectEdit.AllowEdit = True
        Me.rbeSelectEdit.AutoHeight = False
        Me.rbeSelectEdit.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeSelectEdit.ColumnDescriptions = Nothing
        Me.rbeSelectEdit.ColumnNames = Nothing
        Me.rbeSelectEdit.DataSource = Nothing
        Me.rbeSelectEdit.DataSourceColumns = Nothing
        Me.rbeSelectEdit.DefaultSort = Nothing
        Me.rbeSelectEdit.DisplayField = Nothing
        Me.rbeSelectEdit.IsMultiSelect = False
        Me.rbeSelectEdit.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.rbeSelectEdit.Name = "rbeSelectEdit"
        Me.rbeSelectEdit.TableName = Nothing
        Me.rbeSelectEdit.TargetDisplayField = Nothing
        Me.rbeSelectEdit.TargetValueMember = "DisplayedValue"
        Me.rbeSelectEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.rbeSelectEdit.UseCache = False
        Me.rbeSelectEdit.UseSpecifiedColumns = False
        Me.rbeSelectEdit.ValueMember = ""
        '
        'TabPageCreateCategories
        '
        Me.TabPageCreateCategories.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageCreateCategories.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageCreateCategories.Controls.Add(Me.LabelControl1)
        Me.TabPageCreateCategories.Controls.Add(Me.btnCreateCatrgories)
        Me.TabPageCreateCategories.Name = "TabPageCreateCategories"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageCreateCategories, "")
        Me.TabPageCreateCategories.Size = New System.Drawing.Size(967, 527)
        Me.TabPageCreateCategories.Text = "Create Magento Categories"
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(55, 97)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(0, 13)
        Me.LabelControl1.TabIndex = 1
        '
        'btnCreateCatrgories
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnCreateCatrgories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnCreateCatrgories.Location = New System.Drawing.Point(55, 189)
        Me.btnCreateCatrgories.Name = "btnCreateCatrgories"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnCreateCatrgories, System.Drawing.Color.Empty)
        Me.btnCreateCatrgories.Size = New System.Drawing.Size(160, 23)
        Me.btnCreateCatrgories.TabIndex = 2
        Me.btnCreateCatrgories.Text = "Create Magento Categories"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnCreateCatrgories, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TabPageAttributeSets
        '
        Me.TabPageAttributeSets.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageAttributeSets.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageAttributeSets.Controls.Add(Me.btnAddAttribute)
        Me.TabPageAttributeSets.Controls.Add(Me.btnAddGroup)
        Me.TabPageAttributeSets.Controls.Add(Me.btnNewAttributeSet)
        Me.TabPageAttributeSets.Controls.Add(Me.GridControlAttributeSets)
        Me.TabPageAttributeSets.Name = "TabPageAttributeSets"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageAttributeSets, "")
        Me.TabPageAttributeSets.Size = New System.Drawing.Size(967, 527)
        Me.TabPageAttributeSets.Text = "Create Magento Attribute Sets"
        '
        'btnAddAttribute
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnAddAttribute, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnAddAttribute.Location = New System.Drawing.Point(513, 225)
        Me.btnAddAttribute.Name = "btnAddAttribute"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnAddAttribute, System.Drawing.Color.Empty)
        Me.btnAddAttribute.Size = New System.Drawing.Size(143, 23)
        Me.btnAddAttribute.TabIndex = 4
        Me.btnAddAttribute.Text = "Add Attribute to Group"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnAddAttribute, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnAddGroup
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnAddGroup, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnAddGroup.Location = New System.Drawing.Point(513, 175)
        Me.btnAddGroup.Name = "btnAddGroup"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnAddGroup, System.Drawing.Color.Empty)
        Me.btnAddGroup.Size = New System.Drawing.Size(143, 23)
        Me.btnAddGroup.TabIndex = 3
        Me.btnAddGroup.Text = "Add Group to Attribute Set"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnAddGroup, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'btnNewAttributeSet
        '
        Me.ExtendControlProperty.SetHelpText(Me.btnNewAttributeSet, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.btnNewAttributeSet.Location = New System.Drawing.Point(513, 127)
        Me.btnNewAttributeSet.Name = "btnNewAttributeSet"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.btnNewAttributeSet, System.Drawing.Color.Empty)
        Me.btnNewAttributeSet.Size = New System.Drawing.Size(104, 23)
        Me.btnNewAttributeSet.TabIndex = 2
        Me.btnNewAttributeSet.Text = "New Attribute Set"
        Me.ExtendControlProperty.SetTextDisplay(Me.btnNewAttributeSet, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'TabPageCreateAndMapAttributes
        '
        Me.TabPageCreateAndMapAttributes.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageCreateAndMapAttributes.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageCreateAndMapAttributes.Controls.Add(Me.lblAttributeInstructions)
        Me.TabPageCreateAndMapAttributes.Controls.Add(Me.GridControlAttributes)
        Me.TabPageCreateAndMapAttributes.Name = "TabPageCreateAndMapAttributes"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageCreateAndMapAttributes, "")
        Me.TabPageCreateAndMapAttributes.Size = New System.Drawing.Size(967, 527)
        Me.TabPageCreateAndMapAttributes.Text = "Create/Map Magneto Attributes"
        '
        'lblAttributeInstructions
        '
        Me.lblAttributeInstructions.Location = New System.Drawing.Point(702, 84)
        Me.lblAttributeInstructions.Name = "lblAttributeInstructions"
        Me.lblAttributeInstructions.Size = New System.Drawing.Size(253, 156)
        Me.lblAttributeInstructions.TabIndex = 2
        Me.lblAttributeInstructions.Text = resources.GetString("lblAttributeInstructions.Text")
        '
        'GridControlAttributes
        '
        Me.GridControlAttributes.DataMember = "SystemAttributeMagentoMappedView_DEV000221"
        Me.GridControlAttributes.DataSource = Me.BulkPublisingWizardSectionContainerGateway
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlAttributes, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlAttributes, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlAttributes.Location = New System.Drawing.Point(14, 84)
        Me.GridControlAttributes.MainView = Me.GridViewAttributes
        Me.GridControlAttributes.Name = "GridControlAttributes"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlAttributes, System.Drawing.Color.Empty)
        Me.GridControlAttributes.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeSourceAttribute})
        Me.GridControlAttributes.Size = New System.Drawing.Size(682, 437)
        Me.GridControlAttributes.TabIndex = 1
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlAttributes, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlAttributes.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewAttributes})
        '
        'GridViewAttributes
        '
        Me.GridViewAttributes.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colPublish, Me.colAttributeCode, Me.colAttributeDescription, Me.colIsActive, Me.colInstanceID_DEV000221, Me.colSourceAttributeID_DEV000221, Me.colSourceAttributeName_DEV000221, Me.colUsedInMatrixGroups})
        Me.GridViewAttributes.GridControl = Me.GridControlAttributes
        Me.GridViewAttributes.Name = "GridViewAttributes"
        Me.GridViewAttributes.OptionsView.ShowGroupPanel = False
        '
        'colPublish
        '
        Me.colPublish.FieldName = "Publish"
        Me.colPublish.Name = "colPublish"
        Me.colPublish.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colPublish, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colPublish.Visible = True
        Me.colPublish.VisibleIndex = 0
        Me.colPublish.Width = 50
        '
        'colAttributeCode
        '
        Me.colAttributeCode.FieldName = "AttributeCode"
        Me.colAttributeCode.Name = "colAttributeCode"
        Me.colAttributeCode.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeCode.Visible = True
        Me.colAttributeCode.VisibleIndex = 1
        Me.colAttributeCode.Width = 150
        '
        'colAttributeDescription
        '
        Me.colAttributeDescription.FieldName = "AttributeDescription"
        Me.colAttributeDescription.Name = "colAttributeDescription"
        Me.colAttributeDescription.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colAttributeDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAttributeDescription.Visible = True
        Me.colAttributeDescription.VisibleIndex = 2
        Me.colAttributeDescription.Width = 261
        '
        'colIsActive
        '
        Me.colIsActive.FieldName = "IsActive"
        Me.colIsActive.Name = "colIsActive"
        Me.ExtendControlProperty.SetTextDisplay(Me.colIsActive, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colIsActive.Width = 20
        '
        'colInstanceID_DEV000221
        '
        Me.colInstanceID_DEV000221.FieldName = "InstanceID_DEV000221"
        Me.colInstanceID_DEV000221.Name = "colInstanceID_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colInstanceID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colInstanceID_DEV000221.Width = 20
        '
        'colSourceAttributeID_DEV000221
        '
        Me.colSourceAttributeID_DEV000221.Caption = "Source Attribute Name"
        Me.colSourceAttributeID_DEV000221.ColumnEdit = Me.rbeSourceAttribute
        Me.colSourceAttributeID_DEV000221.FieldName = "SourceAttributeID_DEV000221"
        Me.colSourceAttributeID_DEV000221.Name = "colSourceAttributeID_DEV000221"
        Me.colSourceAttributeID_DEV000221.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceAttributeID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceAttributeID_DEV000221.Visible = True
        Me.colSourceAttributeID_DEV000221.VisibleIndex = 3
        Me.colSourceAttributeID_DEV000221.Width = 140
        '
        'rbeSourceAttribute
        '
        Me.rbeSourceAttribute.AutoHeight = False
        Me.rbeSourceAttribute.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.rbeSourceAttribute.Name = "rbeSourceAttribute"
        '
        'colSourceAttributeName_DEV000221
        '
        Me.colSourceAttributeName_DEV000221.Caption = "Source Attribute Name"
        Me.colSourceAttributeName_DEV000221.FieldName = "SourceDisplayName"
        Me.colSourceAttributeName_DEV000221.Name = "colSourceAttributeName_DEV000221"
        Me.colSourceAttributeName_DEV000221.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceAttributeName_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceAttributeName_DEV000221.Width = 20
        '
        'colUsedInMatrixGroups
        '
        Me.colUsedInMatrixGroups.Caption = "Used for Configurables"
        Me.colUsedInMatrixGroups.FieldName = "UsedInMatrixGroups"
        Me.colUsedInMatrixGroups.Name = "colUsedInMatrixGroups"
        Me.colUsedInMatrixGroups.OptionsColumn.FixedWidth = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colUsedInMatrixGroups, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colUsedInMatrixGroups.Visible = True
        Me.colUsedInMatrixGroups.VisibleIndex = 4
        Me.colUsedInMatrixGroups.Width = 135
        '
        'TabPageMapCategories
        '
        Me.TabPageMapCategories.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPageMapCategories.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPageMapCategories.Controls.Add(Me.lblSelectCatgoryMapping)
        Me.TabPageMapCategories.Controls.Add(Me.lblMappingNotes)
        Me.TabPageMapCategories.Controls.Add(Me.TreeListCategoryMapping)
        Me.TabPageMapCategories.Name = "TabPageMapCategories"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPageMapCategories, "")
        Me.TabPageMapCategories.Size = New System.Drawing.Size(967, 527)
        Me.TabPageMapCategories.Text = "Map Magento Categories"
        '
        'lblSelectCatgoryMapping
        '
        Me.lblSelectCatgoryMapping.Location = New System.Drawing.Point(16, 72)
        Me.lblSelectCatgoryMapping.MinimumSize = New System.Drawing.Size(500, 45)
        Me.lblSelectCatgoryMapping.Name = "lblSelectCatgoryMapping"
        Me.lblSelectCatgoryMapping.Size = New System.Drawing.Size(500, 45)
        Me.lblSelectCatgoryMapping.TabIndex = 95
        Me.lblSelectCatgoryMapping.Text = "Text set in code"
        '
        'lblMappingNotes
        '
        Me.lblMappingNotes.Location = New System.Drawing.Point(562, 121)
        Me.lblMappingNotes.Name = "lblMappingNotes"
        Me.lblMappingNotes.Size = New System.Drawing.Size(75, 13)
        Me.lblMappingNotes.TabIndex = 94
        Me.lblMappingNotes.Text = "text set in code"
        '
        'TreeListCategoryMapping
        '
        Me.TreeListCategoryMapping.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colTargetCategoryName, Me.colTreeListTargetCategoryID, Me.colTreeListISCategoryCode, Me.colTreeListISCategoryName})
        Me.TreeListCategoryMapping.Enabled = False
        Me.TreeListCategoryMapping.KeyFieldName = "SourceCategoryID"
        Me.TreeListCategoryMapping.Location = New System.Drawing.Point(14, 119)
        Me.TreeListCategoryMapping.Name = "TreeListCategoryMapping"
        Me.TreeListCategoryMapping.ParentFieldName = "SourceParentID"
        Me.TreeListCategoryMapping.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.rbeISCategoryCode})
        Me.TreeListCategoryMapping.Size = New System.Drawing.Size(520, 403)
        Me.TreeListCategoryMapping.TabIndex = 91
        '
        'colTargetCategoryName
        '
        Me.colTargetCategoryName.Caption = "Target Category"
        Me.colTargetCategoryName.FieldName = "SourceCategoryName"
        Me.colTargetCategoryName.Name = "colTargetCategoryName"
        Me.colTargetCategoryName.OptionsColumn.AllowEdit = False
        Me.colTargetCategoryName.OptionsColumn.ReadOnly = True
        Me.colTargetCategoryName.Visible = True
        Me.colTargetCategoryName.VisibleIndex = 0
        Me.colTargetCategoryName.Width = 150
        '
        'colTreeListTargetCategoryID
        '
        Me.colTreeListTargetCategoryID.Caption = "Source Category ID"
        Me.colTreeListTargetCategoryID.FieldName = "SourceCategoryID"
        Me.colTreeListTargetCategoryID.Name = "colTreeListTargetCategoryID"
        Me.colTreeListTargetCategoryID.Width = 20
        '
        'colTreeListISCategoryCode
        '
        Me.colTreeListISCategoryCode.Caption = "CategoryCode"
        Me.colTreeListISCategoryCode.FieldName = "ISCategoryCode"
        Me.colTreeListISCategoryCode.Name = "colTreeListISCategoryCode"
        Me.colTreeListISCategoryCode.Width = 20
        '
        'colTreeListISCategoryName
        '
        Me.colTreeListISCategoryName.Caption = "Map to CB Product Category"
        Me.colTreeListISCategoryName.ColumnEdit = Me.rbeISCategoryCode
        Me.colTreeListISCategoryName.FieldName = "ISCategoryName"
        Me.colTreeListISCategoryName.Name = "colTreeListISCategoryName"
        Me.colTreeListISCategoryName.Visible = True
        Me.colTreeListISCategoryName.VisibleIndex = 1
        '
        'TabPagePublishingAttributes
        '
        Me.TabPagePublishingAttributes.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPagePublishingAttributes.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPagePublishingAttributes.Controls.Add(Me.lblAttributePublishingErrors)
        Me.TabPagePublishingAttributes.Controls.Add(Me.pnlPublishingAttributesProgress)
        Me.TabPagePublishingAttributes.Name = "TabPagePublishingAttributes"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPagePublishingAttributes, "")
        Me.TabPagePublishingAttributes.Size = New System.Drawing.Size(967, 527)
        Me.TabPagePublishingAttributes.Text = "Create Attributes"
        '
        'lblAttributePublishingErrors
        '
        Me.lblAttributePublishingErrors.Location = New System.Drawing.Point(45, 99)
        Me.lblAttributePublishingErrors.Name = "lblAttributePublishingErrors"
        Me.lblAttributePublishingErrors.Size = New System.Drawing.Size(0, 13)
        Me.lblAttributePublishingErrors.TabIndex = 14
        '
        'pnlPublishingAttributesProgress
        '
        Me.pnlPublishingAttributesProgress.Controls.Add(Me.lblPublishingAttributesPleaseWait)
        Me.pnlPublishingAttributesProgress.Controls.Add(Me.lblPublishingAttributeProgress)
        Me.pnlPublishingAttributesProgress.Location = New System.Drawing.Point(363, 259)
        Me.pnlPublishingAttributesProgress.Name = "pnlPublishingAttributesProgress"
        Me.pnlPublishingAttributesProgress.Size = New System.Drawing.Size(240, 100)
        Me.pnlPublishingAttributesProgress.TabIndex = 13
        Me.pnlPublishingAttributesProgress.Visible = False
        '
        'lblPublishingAttributesPleaseWait
        '
        Me.lblPublishingAttributesPleaseWait.Location = New System.Drawing.Point(93, 62)
        Me.lblPublishingAttributesPleaseWait.Name = "lblPublishingAttributesPleaseWait"
        Me.lblPublishingAttributesPleaseWait.Size = New System.Drawing.Size(54, 13)
        Me.lblPublishingAttributesPleaseWait.TabIndex = 1
        Me.lblPublishingAttributesPleaseWait.Text = "Please wait"
        '
        'lblPublishingAttributeProgress
        '
        Me.lblPublishingAttributeProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblPublishingAttributeProgress.Location = New System.Drawing.Point(3, 28)
        Me.lblPublishingAttributeProgress.MinimumSize = New System.Drawing.Size(234, 25)
        Me.lblPublishingAttributeProgress.Name = "lblPublishingAttributeProgress"
        Me.lblPublishingAttributeProgress.Size = New System.Drawing.Size(234, 25)
        Me.lblPublishingAttributeProgress.TabIndex = 0
        Me.lblPublishingAttributeProgress.Text = "Publishing Attributes to"
        '
        'TabPagePublishAttributeSets
        '
        Me.TabPagePublishAttributeSets.Appearance.PageClient.BackColor = System.Drawing.Color.Transparent
        Me.TabPagePublishAttributeSets.Appearance.PageClient.Options.UseBackColor = True
        Me.TabPagePublishAttributeSets.Controls.Add(Me.pnlPublishingAttributeSetsProgress)
        Me.TabPagePublishAttributeSets.Name = "TabPagePublishAttributeSets"
        Me.PageDescriptionCollection1.SetPageDescription(Me.TabPagePublishAttributeSets, "")
        Me.TabPagePublishAttributeSets.Size = New System.Drawing.Size(967, 527)
        Me.TabPagePublishAttributeSets.Text = "Create Attribute Sets"
        '
        'pnlPublishingAttributeSetsProgress
        '
        Me.pnlPublishingAttributeSetsProgress.Controls.Add(Me.lblPublishingAttributeSetsPleaseWait)
        Me.pnlPublishingAttributeSetsProgress.Controls.Add(Me.lblPublishingAttributeSetProgress)
        Me.pnlPublishingAttributeSetsProgress.Location = New System.Drawing.Point(363, 213)
        Me.pnlPublishingAttributeSetsProgress.Name = "pnlPublishingAttributeSetsProgress"
        Me.pnlPublishingAttributeSetsProgress.Size = New System.Drawing.Size(240, 100)
        Me.pnlPublishingAttributeSetsProgress.TabIndex = 14
        Me.pnlPublishingAttributeSetsProgress.Visible = False
        '
        'lblPublishingAttributeSetsPleaseWait
        '
        Me.lblPublishingAttributeSetsPleaseWait.Location = New System.Drawing.Point(93, 62)
        Me.lblPublishingAttributeSetsPleaseWait.Name = "lblPublishingAttributeSetsPleaseWait"
        Me.lblPublishingAttributeSetsPleaseWait.Size = New System.Drawing.Size(54, 13)
        Me.lblPublishingAttributeSetsPleaseWait.TabIndex = 1
        Me.lblPublishingAttributeSetsPleaseWait.Text = "Please wait"
        '
        'lblPublishingAttributeSetProgress
        '
        Me.lblPublishingAttributeSetProgress.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.lblPublishingAttributeSetProgress.Location = New System.Drawing.Point(3, 28)
        Me.lblPublishingAttributeSetProgress.MinimumSize = New System.Drawing.Size(234, 25)
        Me.lblPublishingAttributeSetProgress.Name = "lblPublishingAttributeSetProgress"
        Me.lblPublishingAttributeSetProgress.Size = New System.Drawing.Size(234, 25)
        Me.lblPublishingAttributeSetProgress.TabIndex = 0
        Me.lblPublishingAttributeSetProgress.Text = "Publishing Attribute Sets to"
        '
        'TabPageShared1
        '
        Me.TabPageShared1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabPageShared1.ForeColor = System.Drawing.Color.Transparent
        Me.TabPageShared1.Location = New System.Drawing.Point(1, 1)
        Me.TabPageShared1.Name = "TabPageShared1"
        Me.TabPageShared1.Size = New System.Drawing.Size(967, 527)
        Me.TabPageShared1.TabIndex = 5
        '
        'BulkPublishingWizardSectionContainer
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.BulkPublisingWizardPluginContainerControl)
        Me.FindSearch = Interprise.Framework.Base.[Shared].[Enum].FindSearch.None
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "BulkPublishingWizardSectionContainer"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(977, 589)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TransactionType = Interprise.Framework.Base.[Shared].[Enum].TransactionType.None
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BulkPublisingWizardSectionContainerGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewAttributeSetGroups, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControlAttributeSets, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewAttributeSets, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewAttributeInGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.LayoutControl1.ResumeLayout(False)
        CType(Me.cbePublishedOtherInstances.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkPublishedOtherInstances.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeInventoryTypeFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlGroup1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemAlreadyPubNote, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemPublishedOtherInstances1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutItemPublishedOtherInstances2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BulkPublisingWizardPluginContainerControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BulkPublisingWizardPluginContainerControl.ResumeLayout(False)
        CType(Me.WizardControlBulkPublish, System.ComponentModel.ISupportInitialize).EndInit()
        Me.WizardControlBulkPublish.ResumeLayout(False)
        Me.TabPageWelcome.ResumeLayout(False)
        Me.TabPageWelcome.PerformLayout()
        CType(Me.pnlGetCategoryProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetCategoryProgress.ResumeLayout(False)
        Me.pnlGetCategoryProgress.PerformLayout()
        CType(Me.cbeSiteOrAccount.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbePublishingTarget.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxLerrynLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageSelectItems.ResumeLayout(False)
        Me.TabPageCategories.ResumeLayout(False)
        Me.TabPageCategories.PerformLayout()
        CType(Me.chkUseMappedCBCategories.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ImageComboBoxEditAttributeSet.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TreeListCategories, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeISCategoryCode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageOptions.ResumeLayout(False)
        Me.TabPageOptions.PerformLayout()
        CType(Me.cbeMagentoWeightUnits.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeMagentoDescriptionSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeMagentoShortDescSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialTo.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialTo.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialFrom.Properties.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DateEditSpecialFrom.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TextEditQtyPublishingValue.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadioGroupQtyPublishing.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeTargetPriceSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeMagentoSpecialPriceSource.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPagePublishing.ResumeLayout(False)
        CType(Me.pnlPublishingProductsProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPublishingProductsProgress.ResumeLayout(False)
        Me.pnlPublishingProductsProgress.PerformLayout()
        Me.TabPageAttributes.ResumeLayout(False)
        Me.TabPageAttributes.PerformLayout()
        CType(Me.pnlGetAttributeProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGetAttributeProgress.ResumeLayout(False)
        Me.pnlGetAttributeProgress.PerformLayout()
        CType(Me.GridControlProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDataType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit.CalendarTimeProperties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeDateEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeTextEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeYesNoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeMemoEdit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeSelectEdit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageCreateCategories.ResumeLayout(False)
        Me.TabPageCreateCategories.PerformLayout()
        Me.TabPageAttributeSets.ResumeLayout(False)
        Me.TabPageCreateAndMapAttributes.ResumeLayout(False)
        Me.TabPageCreateAndMapAttributes.PerformLayout()
        CType(Me.GridControlAttributes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewAttributes, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.rbeSourceAttribute, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPageMapCategories.ResumeLayout(False)
        Me.TabPageMapCategories.PerformLayout()
        CType(Me.TreeListCategoryMapping, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPagePublishingAttributes.ResumeLayout(False)
        Me.TabPagePublishingAttributes.PerformLayout()
        CType(Me.pnlPublishingAttributesProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPublishingAttributesProgress.ResumeLayout(False)
        Me.pnlPublishingAttributesProgress.PerformLayout()
        Me.TabPagePublishAttributeSets.ResumeLayout(False)
        CType(Me.pnlPublishingAttributeSetsProgress, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPublishingAttributeSetsProgress.ResumeLayout(False)
        Me.pnlPublishingAttributeSetsProgress.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend BulkPublisingWizardSectionContainerGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents BulkPublisingWizardPluginContainerControl As Interprise.Presentation.Base.PluginContainerControl
    Friend WithEvents WizardControlBulkPublish As Interprise.Presentation.Base.ExtendedXtraTabContol.WizardControl
    Friend WithEvents TabPageComplete As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents PageDescriptionCollection1 As Interprise.Presentation.Base.ExtendedXtraTabContol.PageDescriptionCollection
    Friend WithEvents TabPageWelcome As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageShared1 As Interprise.Presentation.Base.ExtendedXtraTabContol.TabPageShared
    Friend WithEvents PictureBoxLerrynLogo As System.Windows.Forms.PictureBox
    Friend WithEvents lblMultipleSiteOrAccount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSiteOrAccount As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblPublishingTarget As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeSiteOrAccount As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents cbePublishingTarget As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents TabPageSelectItems As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents LayoutControl1 As DevExpress.XtraLayout.LayoutControl
    Friend WithEvents cbeInventoryTypeFilter As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents BaseSearchDashboardItemToPublish As Interprise.Presentation.Base.Search.BaseSearchDashboard
    Friend WithEvents LayoutControlGroup1 As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItem2 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents TabPageCategories As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageOptions As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TreeListCategories As DevExpress.XtraTreeList.TreeList
    Friend WithEvents colTreeListCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryActive As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListCategoryID As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents TextEditQtyPublishingValue As DevExpress.XtraEditors.TextEdit
    Friend WithEvents lblQtyPublishingValue As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblQtyPublishing As DevExpress.XtraEditors.LabelControl
    Friend WithEvents RadioGroupQtyPublishing As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents TabPagePublishing As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents lblSelectCategories As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAlreadyPublishedNote As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControlItemAlreadyPubNote As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents lblMagentoPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMagentoSpecialPrice As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSelectAttributeSet As DevExpress.XtraEditors.LabelControl
    Friend WithEvents ImageComboBoxEditAttributeSet As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents TabPageAttributes As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents lblMagentoAttributes As DevExpress.XtraEditors.LabelControl
    Friend WithEvents pnlGetAttributeProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetAttributeProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GridControlProperties As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewProperties As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colPropertiesItemCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesName_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesDataType_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeDataType As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents colPropertiesDateValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesNumericValue_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesTextValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesMemoValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colPropertiesDisplayValue As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeDateEdit As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
    Friend WithEvents rbeTextEdit As DevExpress.XtraEditors.Repository.RepositoryItemTextEdit
    Friend WithEvents rbeYesNoEdit As DevExpress.XtraEditors.Repository.RepositoryItemComboBox
    Friend WithEvents rbeMemoEdit As DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit
    Friend WithEvents lblSpecialTo As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblSpecialFrom As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DateEditSpecialTo As DevExpress.XtraEditors.DateEdit
    Friend WithEvents DateEditSpecialFrom As DevExpress.XtraEditors.DateEdit
    Friend WithEvents pnlPublishingProductsProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblPublishingProductPleaseWait As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblPublishingProductProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents TabPageCreateCategories As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageAttributeSets As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents pnlGetCategoryProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblGetCategoryProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents btnCreateCatrgories As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TabPageMapCategories As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TabPageCreateAndMapAttributes As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents TreeListCategoryMapping As DevExpress.XtraTreeList.TreeList
    Friend WithEvents colTargetCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListTargetCategoryID As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListISCategoryCode As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colTreeListISCategoryName As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents rbeISCategoryCode As Lerryn.Presentation.eShopCONNECT.Search.RepSearchTreeControl
    Friend WithEvents lblMappingNotes As DevExpress.XtraEditors.LabelControl
    Friend WithEvents GridControlAttributes As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewAttributes As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents lblSelectCatgoryMapping As DevExpress.XtraEditors.LabelControl
    Friend WithEvents colPublish As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeDescription As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colIsActive As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colInstanceID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceAttributeID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceAttributeName_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeSourceAttribute As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents TabPagePublishingAttributes As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents pnlPublishingAttributesProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblPublishingAttributesPleaseWait As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblPublishingAttributeProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents imagelistCategory As System.Windows.Forms.ImageList
    Friend WithEvents colAttributeID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeHasSelectValues_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents rbeSelectEdit As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
    Friend WithEvents lblAttributeAutoFill As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAttributePublishingErrors As DevExpress.XtraEditors.LabelControl
    Friend WithEvents colUsedInMatrixGroups As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents cbePublishedOtherInstances As DevExpress.XtraEditors.ComboBoxEdit
    Friend WithEvents chkPublishedOtherInstances As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutItemPublishedOtherInstances1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutItemPublishedOtherInstances2 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents GridControlAttributeSets As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewAttributeSetGroups As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridViewAttributeSets As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colAttributeSetID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeSetName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeSetTemplate As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colGroupAttributeSetID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeGroupID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributeGroupName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridViewAttributeInGroup As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colAttributesInGroupSetID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributesInGroupGroupID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributesInGroupAttributeID As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributesInGroupAttributeName As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAttributesInGroupSortOrder As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnAddAttribute As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnAddGroup As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnNewAttributeSet As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents TabPagePublishAttributeSets As DevExpress.XtraTab.XtraTabPage
    Friend WithEvents pnlPublishingAttributeSetsProgress As DevExpress.XtraEditors.PanelControl
    Friend WithEvents lblPublishingAttributeSetsPleaseWait As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblPublishingAttributeSetProgress As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAttributeInstructions As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeTargetPriceSource As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents cbeMagentoSpecialPriceSource As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents chkUseMappedCBCategories As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents lblAutoFillList1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblAutoFillList2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents lblMagentoDescriptionSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeMagentoDescriptionSource As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents lblMagentoShortDescSource As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeMagentoShortDescSource As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents lblMagentoWeightUnits As DevExpress.XtraEditors.LabelControl
    Friend WithEvents cbeMagentoWeightUnits As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents lblSpacer1 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControlItem4 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents lblSpacer2 As DevExpress.XtraEditors.LabelControl
    Friend WithEvents LayoutControlItem6 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents btnAdditionalFilters As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents LayoutControlItem3 As Interprise.Presentation.Base.ExtendedLayoutControlItem
End Class
#End Region

