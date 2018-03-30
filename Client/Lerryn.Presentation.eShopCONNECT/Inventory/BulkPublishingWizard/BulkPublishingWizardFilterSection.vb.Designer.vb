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

#Region " BulkPublishingWizardFilterSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BulkPublishingWizardFilterSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BulkPublishingWizardFilterSection))
        Me.BulkPublishingWizardFilterSectionGateway = Me.ImportExportDataset
        Me.BulkPublishingWizardFilterSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.TreeListDepartmentFilter = New DevExpress.XtraTreeList.TreeList()
        Me.colSelectDepartment = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colDepartmentCode = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colDepartmentDescription = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.TreeListCategoryFilter = New DevExpress.XtraTreeList.TreeList()
        Me.colSelectCategory = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colDisplayImage = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colCategoryCode = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colDescription = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colParentCategoryDescription = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.colLanguageCode = New DevExpress.XtraTreeList.Columns.TreeListColumn()
        Me.chkApplyCategoryFilter = New DevExpress.XtraEditors.CheckEdit()
        Me.cbeStatusFilter = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.cbeSupplierFilter = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.chkApplyStatusFilter = New DevExpress.XtraEditors.CheckEdit()
        Me.chkApplyDepartmentFilter = New DevExpress.XtraEditors.CheckEdit()
        Me.cbeManufacturerFilter = New DevExpress.XtraEditors.ImageComboBoxEdit()
        Me.chkApplyManufacturerFilter = New DevExpress.XtraEditors.CheckEdit()
        Me.chkApplySupplierFilter = New DevExpress.XtraEditors.CheckEdit()
        Me.BulkPublishingWizardFilterSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItemApplyManufacturerFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemManufacturerFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemApplyStatusFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemStatusFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemApplyCategoryFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemCategoryFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemApplyDepartmentFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemDepartmentFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemSupplierFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        Me.LayoutControlItemApplySupplierFilter = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BulkPublishingWizardFilterSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BulkPublishingWizardFilterSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.BulkPublishingWizardFilterSectionExtendedLayout.SuspendLayout()
        CType(Me.TreeListDepartmentFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TreeListCategoryFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkApplyCategoryFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeStatusFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeSupplierFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkApplyStatusFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkApplyDepartmentFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.cbeManufacturerFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkApplyManufacturerFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chkApplySupplierFilter.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.BulkPublishingWizardFilterSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemApplyManufacturerFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemManufacturerFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemApplyStatusFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemStatusFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemApplyCategoryFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemCategoryFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemApplyDepartmentFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemDepartmentFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemSupplierFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItemApplySupplierFilter, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'BulkPublishingWizardFilterSectionGateway
        '
        Me.BulkPublishingWizardFilterSectionGateway.DataSetName = "BulkPublishingWizardFilterSectionDataset"
        Me.BulkPublishingWizardFilterSectionGateway.Instantiate = False
        Me.BulkPublishingWizardFilterSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'BulkPublishingWizardFilterSectionExtendedLayout
        '
        Me.BulkPublishingWizardFilterSectionExtendedLayout.AllowCustomizationMenu = False
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.TreeListDepartmentFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.TreeListCategoryFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.chkApplyCategoryFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.cbeStatusFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.cbeSupplierFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.chkApplyStatusFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.chkApplyDepartmentFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.cbeManufacturerFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.chkApplyManufacturerFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Controls.Add(Me.chkApplySupplierFilter)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BulkPublishingWizardFilterSectionExtendedLayout.IsResetSection = False
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Name = "BulkPublishingWizardFilterSectionExtendedLayout"
        Me.BulkPublishingWizardFilterSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.BulkPublishingWizardFilterSectionExtendedLayout.PluginContainerDataset = Nothing
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Root = Me.BulkPublishingWizardFilterSectionLayoutGroup
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Size = New System.Drawing.Size(390, 283)
        Me.BulkPublishingWizardFilterSectionExtendedLayout.TabIndex = 0
        Me.BulkPublishingWizardFilterSectionExtendedLayout.Text = "BulkPublishingWizardFilterSectionExtendedLayout"
        Me.BulkPublishingWizardFilterSectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'TreeListDepartmentFilter
        '
        Me.TreeListDepartmentFilter.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colSelectDepartment, Me.colDepartmentCode, Me.colDepartmentDescription})
        Me.TreeListDepartmentFilter.DataMember = "InventorySellingDepartmentView"
        Me.TreeListDepartmentFilter.DataSource = Me.BulkPublishingWizardFilterSectionGateway
        Me.TreeListDepartmentFilter.Enabled = False
        Me.TreeListDepartmentFilter.KeyFieldName = "DepartmentCode"
        Me.TreeListDepartmentFilter.Location = New System.Drawing.Point(188, 174)
        Me.TreeListDepartmentFilter.MaximumSize = New System.Drawing.Size(0, 90)
        Me.TreeListDepartmentFilter.Name = "TreeListDepartmentFilter"
        Me.TreeListDepartmentFilter.ParentFieldName = "ParentDepartment"
        Me.TreeListDepartmentFilter.Size = New System.Drawing.Size(200, 90)
        Me.TreeListDepartmentFilter.TabIndex = 11
        '
        'colSelectDepartment
        '
        Me.colSelectDepartment.AppearanceCell.Options.UseTextOptions = True
        Me.colSelectDepartment.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colSelectDepartment.AppearanceHeader.Options.UseTextOptions = True
        Me.colSelectDepartment.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colSelectDepartment.Caption = "Select"
        Me.colSelectDepartment.FieldName = "SelectDepartment"
        Me.colSelectDepartment.Name = "colSelectDepartment"
        Me.colSelectDepartment.OptionsColumn.FixedWidth = True
        Me.colSelectDepartment.Visible = True
        Me.colSelectDepartment.VisibleIndex = 0
        Me.colSelectDepartment.Width = 45
        '
        'colDepartmentCode
        '
        Me.colDepartmentCode.FieldName = "DepartmentCode"
        Me.colDepartmentCode.Name = "colDepartmentCode"
        Me.colDepartmentCode.Width = 20
        '
        'colDepartmentDescription
        '
        Me.colDepartmentDescription.Caption = "Department"
        Me.colDepartmentDescription.FieldName = "Description"
        Me.colDepartmentDescription.Name = "colDepartmentDescription"
        Me.colDepartmentDescription.OptionsColumn.FixedWidth = True
        Me.colDepartmentDescription.Visible = True
        Me.colDepartmentDescription.VisibleIndex = 1
        Me.colDepartmentDescription.Width = 135
        '
        'TreeListCategoryFilter
        '
        Me.TreeListCategoryFilter.Columns.AddRange(New DevExpress.XtraTreeList.Columns.TreeListColumn() {Me.colSelectCategory, Me.colDisplayImage, Me.colCategoryCode, Me.colDescription, Me.colParentCategoryDescription, Me.colLanguageCode})
        Me.TreeListCategoryFilter.DataMember = "SystemCategoryView"
        Me.TreeListCategoryFilter.DataSource = Me.BulkPublishingWizardFilterSectionGateway
        Me.TreeListCategoryFilter.Enabled = False
        Me.TreeListCategoryFilter.KeyFieldName = "CategoryCode"
        Me.TreeListCategoryFilter.Location = New System.Drawing.Point(188, 80)
        Me.TreeListCategoryFilter.MinimumSize = New System.Drawing.Size(0, 90)
        Me.TreeListCategoryFilter.Name = "TreeListCategoryFilter"
        Me.TreeListCategoryFilter.ParentFieldName = "ParentCategory"
        Me.TreeListCategoryFilter.Size = New System.Drawing.Size(200, 90)
        Me.TreeListCategoryFilter.TabIndex = 9
        '
        'colSelectCategory
        '
        Me.colSelectCategory.AppearanceCell.Options.UseTextOptions = True
        Me.colSelectCategory.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colSelectCategory.AppearanceHeader.Options.UseTextOptions = True
        Me.colSelectCategory.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        Me.colSelectCategory.Caption = "Select"
        Me.colSelectCategory.FieldName = "SelectCategory"
        Me.colSelectCategory.Name = "colSelectCategory"
        Me.colSelectCategory.OptionsColumn.FixedWidth = True
        Me.colSelectCategory.Visible = True
        Me.colSelectCategory.VisibleIndex = 0
        Me.colSelectCategory.Width = 45
        '
        'colDisplayImage
        '
        Me.colDisplayImage.FieldName = "DisplayImage"
        Me.colDisplayImage.Name = "colDisplayImage"
        Me.colDisplayImage.Width = 20
        '
        'colCategoryCode
        '
        Me.colCategoryCode.Caption = "Category Code"
        Me.colCategoryCode.FieldName = "CategoryCode"
        Me.colCategoryCode.Name = "colCategoryCode"
        Me.colCategoryCode.Width = 20
        '
        'colDescription
        '
        Me.colDescription.Caption = "Category"
        Me.colDescription.FieldName = "Description"
        Me.colDescription.Name = "colDescription"
        Me.colDescription.OptionsColumn.FixedWidth = True
        Me.colDescription.Visible = True
        Me.colDescription.VisibleIndex = 1
        Me.colDescription.Width = 135
        '
        'colParentCategoryDescription
        '
        Me.colParentCategoryDescription.FieldName = "ParentCategoryDescription"
        Me.colParentCategoryDescription.Name = "colParentCategoryDescription"
        Me.colParentCategoryDescription.Width = 20
        '
        'colLanguageCode
        '
        Me.colLanguageCode.Caption = "TreeListColumn1"
        Me.colLanguageCode.FieldName = "LanguageCode"
        Me.colLanguageCode.Name = "colLanguageCode"
        Me.colLanguageCode.Width = 20
        '
        'chkApplyCategoryFilter
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkApplyCategoryFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkApplyCategoryFilter.Location = New System.Drawing.Point(2, 80)
        Me.chkApplyCategoryFilter.MaximumSize = New System.Drawing.Size(0, 22)
        Me.chkApplyCategoryFilter.Name = "chkApplyCategoryFilter"
        Me.chkApplyCategoryFilter.Properties.AutoHeight = False
        Me.chkApplyCategoryFilter.Properties.Caption = "Apply Category Filter"
        Me.chkApplyCategoryFilter.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkApplyCategoryFilter, System.Drawing.Color.Empty)
        Me.chkApplyCategoryFilter.Size = New System.Drawing.Size(182, 22)
        Me.chkApplyCategoryFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.chkApplyCategoryFilter.TabIndex = 8
        Me.ExtendControlProperty.SetTextDisplay(Me.chkApplyCategoryFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeStatusFilter
        '
        Me.cbeStatusFilter.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeStatusFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeStatusFilter.Location = New System.Drawing.Point(188, 54)
        Me.cbeStatusFilter.MaximumSize = New System.Drawing.Size(0, 22)
        Me.cbeStatusFilter.Name = "cbeStatusFilter"
        Me.cbeStatusFilter.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeStatusFilter.Properties.Appearance.Options.UseFont = True
        Me.cbeStatusFilter.Properties.AutoHeight = False
        Me.cbeStatusFilter.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeStatusFilter, System.Drawing.Color.Empty)
        Me.cbeStatusFilter.Size = New System.Drawing.Size(200, 22)
        Me.cbeStatusFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.cbeStatusFilter.TabIndex = 7
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeStatusFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeSupplierFilter
        '
        Me.cbeSupplierFilter.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeSupplierFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeSupplierFilter.Location = New System.Drawing.Point(188, 28)
        Me.cbeSupplierFilter.Name = "cbeSupplierFilter"
        Me.cbeSupplierFilter.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeSupplierFilter.Properties.Appearance.Options.UseFont = True
        Me.cbeSupplierFilter.Properties.AutoHeight = False
        Me.cbeSupplierFilter.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeSupplierFilter.Properties.Sorted = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeSupplierFilter, System.Drawing.Color.Empty)
        Me.cbeSupplierFilter.Size = New System.Drawing.Size(200, 22)
        Me.cbeSupplierFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.cbeSupplierFilter.TabIndex = 13
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeSupplierFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkApplyStatusFilter
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkApplyStatusFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkApplyStatusFilter.Location = New System.Drawing.Point(2, 54)
        Me.chkApplyStatusFilter.MaximumSize = New System.Drawing.Size(0, 22)
        Me.chkApplyStatusFilter.Name = "chkApplyStatusFilter"
        Me.chkApplyStatusFilter.Properties.AutoHeight = False
        Me.chkApplyStatusFilter.Properties.Caption = "Apply Status Filter"
        Me.chkApplyStatusFilter.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkApplyStatusFilter, System.Drawing.Color.Empty)
        Me.chkApplyStatusFilter.Size = New System.Drawing.Size(182, 22)
        Me.chkApplyStatusFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.chkApplyStatusFilter.TabIndex = 6
        Me.ExtendControlProperty.SetTextDisplay(Me.chkApplyStatusFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkApplyDepartmentFilter
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkApplyDepartmentFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkApplyDepartmentFilter.Location = New System.Drawing.Point(2, 174)
        Me.chkApplyDepartmentFilter.MaximumSize = New System.Drawing.Size(0, 22)
        Me.chkApplyDepartmentFilter.Name = "chkApplyDepartmentFilter"
        Me.chkApplyDepartmentFilter.Properties.AutoHeight = False
        Me.chkApplyDepartmentFilter.Properties.Caption = "Apply Department Filter"
        Me.chkApplyDepartmentFilter.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkApplyDepartmentFilter, System.Drawing.Color.Empty)
        Me.chkApplyDepartmentFilter.Size = New System.Drawing.Size(182, 22)
        Me.chkApplyDepartmentFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.chkApplyDepartmentFilter.TabIndex = 10
        Me.ExtendControlProperty.SetTextDisplay(Me.chkApplyDepartmentFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'cbeManufacturerFilter
        '
        Me.cbeManufacturerFilter.Enabled = False
        Me.ExtendControlProperty.SetHelpText(Me.cbeManufacturerFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.cbeManufacturerFilter.Location = New System.Drawing.Point(188, 2)
        Me.cbeManufacturerFilter.MaximumSize = New System.Drawing.Size(0, 22)
        Me.cbeManufacturerFilter.Name = "cbeManufacturerFilter"
        Me.cbeManufacturerFilter.Properties.Appearance.Font = New System.Drawing.Font("Lato", 8.25!)
        Me.cbeManufacturerFilter.Properties.Appearance.Options.UseFont = True
        Me.cbeManufacturerFilter.Properties.AutoHeight = False
        Me.cbeManufacturerFilter.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.cbeManufacturerFilter.Properties.Sorted = True
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.cbeManufacturerFilter, System.Drawing.Color.Empty)
        Me.cbeManufacturerFilter.Size = New System.Drawing.Size(200, 22)
        Me.cbeManufacturerFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.cbeManufacturerFilter.TabIndex = 5
        Me.ExtendControlProperty.SetTextDisplay(Me.cbeManufacturerFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkApplyManufacturerFilter
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkApplyManufacturerFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkApplyManufacturerFilter.Location = New System.Drawing.Point(2, 2)
        Me.chkApplyManufacturerFilter.MaximumSize = New System.Drawing.Size(0, 22)
        Me.chkApplyManufacturerFilter.Name = "chkApplyManufacturerFilter"
        Me.chkApplyManufacturerFilter.Properties.AutoHeight = False
        Me.chkApplyManufacturerFilter.Properties.Caption = "Apply Manufacturer Filter"
        Me.chkApplyManufacturerFilter.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkApplyManufacturerFilter, System.Drawing.Color.Empty)
        Me.chkApplyManufacturerFilter.Size = New System.Drawing.Size(182, 22)
        Me.chkApplyManufacturerFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.chkApplyManufacturerFilter.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.chkApplyManufacturerFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'chkApplySupplierFilter
        '
        Me.ExtendControlProperty.SetHelpText(Me.chkApplySupplierFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.chkApplySupplierFilter.Location = New System.Drawing.Point(2, 28)
        Me.chkApplySupplierFilter.MaximumSize = New System.Drawing.Size(0, 22)
        Me.chkApplySupplierFilter.Name = "chkApplySupplierFilter"
        Me.chkApplySupplierFilter.Properties.AutoHeight = False
        Me.chkApplySupplierFilter.Properties.Caption = "Apply Supplier Filter"
        Me.chkApplySupplierFilter.Properties.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.chkApplySupplierFilter, System.Drawing.Color.Empty)
        Me.chkApplySupplierFilter.Size = New System.Drawing.Size(182, 22)
        Me.chkApplySupplierFilter.StyleController = Me.BulkPublishingWizardFilterSectionExtendedLayout
        Me.chkApplySupplierFilter.TabIndex = 12
        Me.ExtendControlProperty.SetTextDisplay(Me.chkApplySupplierFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'BulkPublishingWizardFilterSectionLayoutGroup
        '
        Me.BulkPublishingWizardFilterSectionLayoutGroup.CustomizationFormText = "BulkPublishingWizardFilterSectionLayoutGroup"
        Me.BulkPublishingWizardFilterSectionLayoutGroup.GroupBordersVisible = False
        Me.BulkPublishingWizardFilterSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItemApplyManufacturerFilter, Me.LayoutControlItemManufacturerFilter, Me.LayoutControlItemApplyStatusFilter, Me.LayoutControlItemStatusFilter, Me.LayoutControlItemApplyCategoryFilter, Me.LayoutControlItemCategoryFilter, Me.LayoutControlItemApplyDepartmentFilter, Me.LayoutControlItemDepartmentFilter, Me.LayoutControlItemSupplierFilter, Me.LayoutControlItemApplySupplierFilter})
        Me.BulkPublishingWizardFilterSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.BulkPublishingWizardFilterSectionLayoutGroup.Name = "BulkPublishingWizardFilterSectionLayoutGroup"
        Me.BulkPublishingWizardFilterSectionLayoutGroup.Padding = New DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0)
        Me.BulkPublishingWizardFilterSectionLayoutGroup.Size = New System.Drawing.Size(390, 283)
        Me.BulkPublishingWizardFilterSectionLayoutGroup.Text = "BulkPublishingWizardFilterSectionLayoutGroup"
        Me.BulkPublishingWizardFilterSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItemApplyManufacturerFilter
        '
        Me.LayoutControlItemApplyManufacturerFilter.Control = Me.chkApplyManufacturerFilter
        Me.LayoutControlItemApplyManufacturerFilter.CustomizationFormText = "LayoutControlItemApplyManufacturerFilter"
        Me.LayoutControlItemApplyManufacturerFilter.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItemApplyManufacturerFilter.Name = "LayoutControlItemApplyManufacturerFilter"
        Me.LayoutControlItemApplyManufacturerFilter.Size = New System.Drawing.Size(186, 26)
        Me.LayoutControlItemApplyManufacturerFilter.Text = "LayoutControlItemApplyManufacturerFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemApplyManufacturerFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemApplyManufacturerFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemApplyManufacturerFilter.TextToControlDistance = 0
        Me.LayoutControlItemApplyManufacturerFilter.TextVisible = False
        '
        'LayoutControlItemManufacturerFilter
        '
        Me.LayoutControlItemManufacturerFilter.Control = Me.cbeManufacturerFilter
        Me.LayoutControlItemManufacturerFilter.CustomizationFormText = "LayoutControlItemManufacturerFilter"
        Me.LayoutControlItemManufacturerFilter.Location = New System.Drawing.Point(186, 0)
        Me.LayoutControlItemManufacturerFilter.Name = "LayoutControlItemManufacturerFilter"
        Me.LayoutControlItemManufacturerFilter.Size = New System.Drawing.Size(204, 26)
        Me.LayoutControlItemManufacturerFilter.Text = "LayoutControlItemManufacturerFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemManufacturerFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemManufacturerFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemManufacturerFilter.TextToControlDistance = 0
        Me.LayoutControlItemManufacturerFilter.TextVisible = False
        '
        'LayoutControlItemApplyStatusFilter
        '
        Me.LayoutControlItemApplyStatusFilter.Control = Me.chkApplyStatusFilter
        Me.LayoutControlItemApplyStatusFilter.CustomizationFormText = "LayoutControlItemApplyStatusFilter"
        Me.LayoutControlItemApplyStatusFilter.Location = New System.Drawing.Point(0, 52)
        Me.LayoutControlItemApplyStatusFilter.Name = "LayoutControlItemApplyStatusFilter"
        Me.LayoutControlItemApplyStatusFilter.Size = New System.Drawing.Size(186, 26)
        Me.LayoutControlItemApplyStatusFilter.Text = "LayoutControlItemApplyStatusFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemApplyStatusFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemApplyStatusFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemApplyStatusFilter.TextToControlDistance = 0
        Me.LayoutControlItemApplyStatusFilter.TextVisible = False
        '
        'LayoutControlItemStatusFilter
        '
        Me.LayoutControlItemStatusFilter.Control = Me.cbeStatusFilter
        Me.LayoutControlItemStatusFilter.CustomizationFormText = "LayoutControlItemStatusFilter"
        Me.LayoutControlItemStatusFilter.Location = New System.Drawing.Point(186, 52)
        Me.LayoutControlItemStatusFilter.Name = "LayoutControlItemStatusFilter"
        Me.LayoutControlItemStatusFilter.Size = New System.Drawing.Size(204, 26)
        Me.LayoutControlItemStatusFilter.Text = "LayoutControlItemStatusFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemStatusFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemStatusFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemStatusFilter.TextToControlDistance = 0
        Me.LayoutControlItemStatusFilter.TextVisible = False
        '
        'LayoutControlItemApplyCategoryFilter
        '
        Me.LayoutControlItemApplyCategoryFilter.Control = Me.chkApplyCategoryFilter
        Me.LayoutControlItemApplyCategoryFilter.CustomizationFormText = "LayoutControlItemApplyCategoryFilter"
        Me.LayoutControlItemApplyCategoryFilter.Location = New System.Drawing.Point(0, 78)
        Me.LayoutControlItemApplyCategoryFilter.Name = "LayoutControlItemApplyCategoryFilter"
        Me.LayoutControlItemApplyCategoryFilter.Size = New System.Drawing.Size(186, 94)
        Me.LayoutControlItemApplyCategoryFilter.Text = "LayoutControlItemApplyCategoryFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemApplyCategoryFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemApplyCategoryFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemApplyCategoryFilter.TextToControlDistance = 0
        Me.LayoutControlItemApplyCategoryFilter.TextVisible = False
        '
        'LayoutControlItemCategoryFilter
        '
        Me.LayoutControlItemCategoryFilter.Control = Me.TreeListCategoryFilter
        Me.LayoutControlItemCategoryFilter.CustomizationFormText = "LayoutControlItemCategoryFilter"
        Me.LayoutControlItemCategoryFilter.Location = New System.Drawing.Point(186, 78)
        Me.LayoutControlItemCategoryFilter.Name = "LayoutControlItemCategoryFilter"
        Me.LayoutControlItemCategoryFilter.Size = New System.Drawing.Size(204, 94)
        Me.LayoutControlItemCategoryFilter.Text = "LayoutControlItemCategoryFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemCategoryFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemCategoryFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemCategoryFilter.TextToControlDistance = 0
        Me.LayoutControlItemCategoryFilter.TextVisible = False
        '
        'LayoutControlItemApplyDepartmentFilter
        '
        Me.LayoutControlItemApplyDepartmentFilter.Control = Me.chkApplyDepartmentFilter
        Me.LayoutControlItemApplyDepartmentFilter.CustomizationFormText = "LayoutControlItemApplyDepartmentFilter"
        Me.LayoutControlItemApplyDepartmentFilter.Location = New System.Drawing.Point(0, 172)
        Me.LayoutControlItemApplyDepartmentFilter.Name = "LayoutControlItemApplyDepartmentFilter"
        Me.LayoutControlItemApplyDepartmentFilter.Size = New System.Drawing.Size(186, 111)
        Me.LayoutControlItemApplyDepartmentFilter.Text = "LayoutControlItemApplyDepartmentFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemApplyDepartmentFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemApplyDepartmentFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemApplyDepartmentFilter.TextToControlDistance = 0
        Me.LayoutControlItemApplyDepartmentFilter.TextVisible = False
        '
        'LayoutControlItemDepartmentFilter
        '
        Me.LayoutControlItemDepartmentFilter.Control = Me.TreeListDepartmentFilter
        Me.LayoutControlItemDepartmentFilter.CustomizationFormText = "LayoutControlItemDepartmentFilter"
        Me.LayoutControlItemDepartmentFilter.Location = New System.Drawing.Point(186, 172)
        Me.LayoutControlItemDepartmentFilter.Name = "LayoutControlItemDepartmentFilter"
        Me.LayoutControlItemDepartmentFilter.Size = New System.Drawing.Size(204, 111)
        Me.LayoutControlItemDepartmentFilter.Text = "LayoutControlItemDepartmentFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemDepartmentFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemDepartmentFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemDepartmentFilter.TextToControlDistance = 0
        Me.LayoutControlItemDepartmentFilter.TextVisible = False
        '
        'LayoutControlItemSupplierFilter
        '
        Me.LayoutControlItemSupplierFilter.Control = Me.cbeSupplierFilter
        Me.LayoutControlItemSupplierFilter.CustomizationFormText = "LayoutControlItemSupplierFilter"
        Me.LayoutControlItemSupplierFilter.Location = New System.Drawing.Point(186, 26)
        Me.LayoutControlItemSupplierFilter.Name = "LayoutControlItemSupplierFilter"
        Me.LayoutControlItemSupplierFilter.Size = New System.Drawing.Size(204, 26)
        Me.LayoutControlItemSupplierFilter.Text = "LayoutControlItemSupplierFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemSupplierFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemSupplierFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemSupplierFilter.TextToControlDistance = 0
        Me.LayoutControlItemSupplierFilter.TextVisible = False
        '
        'LayoutControlItemApplySupplierFilter
        '
        Me.LayoutControlItemApplySupplierFilter.Control = Me.chkApplySupplierFilter
        Me.LayoutControlItemApplySupplierFilter.CustomizationFormText = "LayoutControlItemApplySupplierFilter"
        Me.LayoutControlItemApplySupplierFilter.Location = New System.Drawing.Point(0, 26)
        Me.LayoutControlItemApplySupplierFilter.Name = "LayoutControlItemApplySupplierFilter"
        Me.LayoutControlItemApplySupplierFilter.Size = New System.Drawing.Size(186, 26)
        Me.LayoutControlItemApplySupplierFilter.Text = "LayoutControlItemApplySupplierFilter"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItemApplySupplierFilter, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItemApplySupplierFilter.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItemApplySupplierFilter.TextToControlDistance = 0
        Me.LayoutControlItemApplySupplierFilter.TextVisible = False
        '
        'BulkPublishingWizardFilterSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.BulkPublishingWizardFilterSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "BulkPublishingWizardFilterSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(390, 283)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BulkPublishingWizardFilterSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BulkPublishingWizardFilterSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.BulkPublishingWizardFilterSectionExtendedLayout.ResumeLayout(False)
        CType(Me.TreeListDepartmentFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TreeListCategoryFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkApplyCategoryFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeStatusFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeSupplierFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkApplyStatusFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkApplyDepartmentFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.cbeManufacturerFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkApplyManufacturerFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chkApplySupplierFilter.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.BulkPublishingWizardFilterSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemApplyManufacturerFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemManufacturerFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemApplyStatusFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemStatusFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemApplyCategoryFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemCategoryFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemApplyDepartmentFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemDepartmentFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemSupplierFilter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItemApplySupplierFilter, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents BulkPublishingWizardFilterSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents BulkPublishingWizardFilterSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents BulkPublishingWizardFilterSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents chkApplyStatusFilter As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cbeManufacturerFilter As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents chkApplyManufacturerFilter As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItemApplyManufacturerFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItemManufacturerFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItemApplyStatusFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents chkApplyCategoryFilter As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents cbeStatusFilter As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents LayoutControlItemStatusFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItemApplyCategoryFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents chkApplyDepartmentFilter As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents TreeListCategoryFilter As DevExpress.XtraTreeList.TreeList
    Friend WithEvents LayoutControlItemCategoryFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItemApplyDepartmentFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents TreeListDepartmentFilter As DevExpress.XtraTreeList.TreeList
    Friend WithEvents LayoutControlItemDepartmentFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents cbeSupplierFilter As DevExpress.XtraEditors.ImageComboBoxEdit
    Friend WithEvents chkApplySupplierFilter As DevExpress.XtraEditors.CheckEdit
    Friend WithEvents LayoutControlItemApplySupplierFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents LayoutControlItemSupplierFilter As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents colDisplayImage As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colDescription As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colParentCategoryDescription As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colLanguageCode As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colSelectCategory As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colSelectDepartment As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colDepartmentCode As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colDepartmentDescription As DevExpress.XtraTreeList.Columns.TreeListColumn
    Friend WithEvents colCategoryCode As DevExpress.XtraTreeList.Columns.TreeListColumn

#Region "ImportExportDataset"
    Public ReadOnly Property ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_BulkPublishingWizardFilterDataset
        End Get
    End Property
#End Region
End Class
#End Region

