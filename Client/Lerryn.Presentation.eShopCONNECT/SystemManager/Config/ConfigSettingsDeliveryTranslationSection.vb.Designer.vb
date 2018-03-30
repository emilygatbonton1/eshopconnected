'===============================================================================
' Interprise Suite SDK
' Copyright © 2004-2008 Interprise Software Solutions Incorporated
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

#Region " ConfigSettingsDeliveryTranslationSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigSettingsDeliveryTranslationSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigSettingsDeliveryTranslationSection))
        Me.ConfigSettingsDeliveryTranslationSectionGateway = Me.ImportExportDataset
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.GridControlDeliveryTranslation = New DevExpress.XtraGrid.GridControl()
        Me.GridViewDeliveryTranslation = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colSourceCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceDeliveryMethod_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceDeliveryMethodCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceDeliveryClass_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceDeliveryClassCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colShippingMethodGroup_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repDeliveryMethodGroup = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.colShippingMethodCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repDeliveryMethod = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.repSourceDeliveryMethod = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.repSourceDeliveryClass = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsDeliveryTranslationSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.SuspendLayout()
        CType(Me.GridControlDeliveryTranslation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewDeliveryTranslation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repDeliveryMethodGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repDeliveryMethod, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repSourceDeliveryMethod, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repSourceDeliveryClass, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'ConfigSettingsDeliveryTranslationSectionGateway
        '
        Me.ConfigSettingsDeliveryTranslationSectionGateway.DataSetName = "ConfigSettingsDeliveryTranslationSectionDataset"
        Me.ConfigSettingsDeliveryTranslationSectionGateway.Instantiate = False
        Me.ConfigSettingsDeliveryTranslationSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'ConfigSettingsDeliveryTranslationSectionExtendedLayout
        '
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.AllowCustomizationMenu = False
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.Controls.Add(Me.GridControlDeliveryTranslation)
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.IsResetSection = False
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.Name = "ConfigSettingsDeliveryTranslationSectionExtendedLayout"
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.PluginContainerDataset = Nothing
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.Root = Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.TabIndex = 0
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.Text = "ConfigSettingsDeliveryTranslationSectionExtendedLayout"
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.UsedMaxCounter = Nothing
        '
        'GridControlDeliveryTranslation
        '
        Me.GridControlDeliveryTranslation.DataMember = "LerrynImportExportDeliveryMethods_DEV000221"
        Me.GridControlDeliveryTranslation.DataSource = Me.ConfigSettingsDeliveryTranslationSectionGateway
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlDeliveryTranslation, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlDeliveryTranslation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlDeliveryTranslation.Location = New System.Drawing.Point(3, 8)
        Me.GridControlDeliveryTranslation.MainView = Me.GridViewDeliveryTranslation
        Me.GridControlDeliveryTranslation.Name = "GridControlDeliveryTranslation"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.GridControlDeliveryTranslation, System.Drawing.Color.Empty)
        Me.GridControlDeliveryTranslation.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repDeliveryMethodGroup, Me.repDeliveryMethod, Me.repSourceDeliveryMethod, Me.repSourceDeliveryClass})
        Me.GridControlDeliveryTranslation.Size = New System.Drawing.Size(685, 408)
        Me.GridControlDeliveryTranslation.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlDeliveryTranslation, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlDeliveryTranslation.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewDeliveryTranslation})
        '
        'GridViewDeliveryTranslation
        '
        Me.GridViewDeliveryTranslation.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colSourceCode_DEV000221, Me.colSourceDeliveryMethod_DEV000221, Me.colSourceDeliveryMethodCode_DEV000221, Me.colSourceDeliveryClass_DEV000221, Me.colSourceDeliveryClassCode_DEV000221, Me.colShippingMethodGroup_DEV000221, Me.colShippingMethodCode_DEV000221})
        Me.GridViewDeliveryTranslation.GridControl = Me.GridControlDeliveryTranslation
        Me.GridViewDeliveryTranslation.Name = "GridViewDeliveryTranslation"
        Me.GridViewDeliveryTranslation.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.GridViewDeliveryTranslation.OptionsView.ShowGroupPanel = False
        '
        'colSourceCode_DEV000221
        '
        Me.colSourceCode_DEV000221.Caption = "Source Code"
        Me.colSourceCode_DEV000221.FieldName = "SourceCode_DEV000221"
        Me.colSourceCode_DEV000221.Name = "colSourceCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceCode_DEV000221.Width = 20
        '
        'colSourceDeliveryMethod_DEV000221
        '
        Me.colSourceDeliveryMethod_DEV000221.Caption = "Source Delivery Method"
        Me.colSourceDeliveryMethod_DEV000221.FieldName = "SourceDeliveryMethod_DEV000221"
        Me.colSourceDeliveryMethod_DEV000221.Name = "colSourceDeliveryMethod_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceDeliveryMethod_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceDeliveryMethod_DEV000221.Visible = True
        Me.colSourceDeliveryMethod_DEV000221.VisibleIndex = 0
        Me.colSourceDeliveryMethod_DEV000221.Width = 170
        '
        'colSourceDeliveryMethodCode_DEV000221
        '
        Me.colSourceDeliveryMethodCode_DEV000221.Caption = "Source Delivery Method"
        Me.colSourceDeliveryMethodCode_DEV000221.FieldName = "SourceDeliveryMethodCode_DEV000221"
        Me.colSourceDeliveryMethodCode_DEV000221.Name = "colSourceDeliveryMethodCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceDeliveryMethodCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceDeliveryMethodCode_DEV000221.Width = 170
        '
        'colSourceDeliveryClass_DEV000221
        '
        Me.colSourceDeliveryClass_DEV000221.Caption = "Source Delivery Class"
        Me.colSourceDeliveryClass_DEV000221.FieldName = "SourceDeliveryClass_DEV000221"
        Me.colSourceDeliveryClass_DEV000221.Name = "colSourceDeliveryClass_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceDeliveryClass_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceDeliveryClass_DEV000221.Visible = True
        Me.colSourceDeliveryClass_DEV000221.VisibleIndex = 1
        Me.colSourceDeliveryClass_DEV000221.Width = 160
        '
        'colSourceDeliveryClassCode_DEV000221
        '
        Me.colSourceDeliveryClassCode_DEV000221.Caption = "Source Delivery Class"
        Me.colSourceDeliveryClassCode_DEV000221.FieldName = "SourceDeliveryClassCode_DEV000221"
        Me.colSourceDeliveryClassCode_DEV000221.Name = "colSourceDeliveryClassCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceDeliveryClassCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceDeliveryClassCode_DEV000221.Width = 160
        '
        'colShippingMethodGroup_DEV000221
        '
        Me.colShippingMethodGroup_DEV000221.Caption = "Shipping Method Group"
        Me.colShippingMethodGroup_DEV000221.ColumnEdit = Me.repDeliveryMethodGroup
        Me.colShippingMethodGroup_DEV000221.FieldName = "ShippingMethodGroup_DEV000221"
        Me.colShippingMethodGroup_DEV000221.Name = "colShippingMethodGroup_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colShippingMethodGroup_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colShippingMethodGroup_DEV000221.Visible = True
        Me.colShippingMethodGroup_DEV000221.VisibleIndex = 2
        Me.colShippingMethodGroup_DEV000221.Width = 160
        '
        'repDeliveryMethodGroup
        '
        Me.repDeliveryMethodGroup.AdditionalFilter = " and IsActive = 1"
        Me.repDeliveryMethodGroup.AllowEdit = True
        Me.repDeliveryMethodGroup.AutoHeight = False
        Me.repDeliveryMethodGroup.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repDeliveryMethodGroup.ColumnDescriptions = Nothing
        Me.repDeliveryMethodGroup.ColumnNames = Nothing
        Me.repDeliveryMethodGroup.DataSource = Nothing
        Me.repDeliveryMethodGroup.DataSourceColumns = Nothing
        Me.repDeliveryMethodGroup.DefaultSort = Nothing
        Me.repDeliveryMethodGroup.DisplayField = ""
        Me.repDeliveryMethodGroup.IsMultiSelect = False
        Me.repDeliveryMethodGroup.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.repDeliveryMethodGroup.Name = "repDeliveryMethodGroup"
        Me.repDeliveryMethodGroup.TableName = "SystemShippingMethodGroup"
        Me.repDeliveryMethodGroup.TargetDisplayField = Nothing
        Me.repDeliveryMethodGroup.TargetValueMember = ""
        Me.repDeliveryMethodGroup.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.repDeliveryMethodGroup.UseCache = False
        Me.repDeliveryMethodGroup.UseSpecifiedColumns = False
        Me.repDeliveryMethodGroup.ValueMember = ""
        '
        'colShippingMethodCode_DEV000221
        '
        Me.colShippingMethodCode_DEV000221.Caption = "Shipping Method Code"
        Me.colShippingMethodCode_DEV000221.ColumnEdit = Me.repDeliveryMethod
        Me.colShippingMethodCode_DEV000221.FieldName = "ShippingMethodCode_DEV000221"
        Me.colShippingMethodCode_DEV000221.Name = "colShippingMethodCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colShippingMethodCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colShippingMethodCode_DEV000221.Visible = True
        Me.colShippingMethodCode_DEV000221.VisibleIndex = 3
        Me.colShippingMethodCode_DEV000221.Width = 177
        '
        'repDeliveryMethod
        '
        Me.repDeliveryMethod.AdditionalFilter = " and IsActive = 1"
        Me.repDeliveryMethod.AllowEdit = True
        Me.repDeliveryMethod.AutoHeight = False
        Me.repDeliveryMethod.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repDeliveryMethod.ColumnDescriptions = Nothing
        Me.repDeliveryMethod.ColumnNames = Nothing
        Me.repDeliveryMethod.DataSource = Nothing
        Me.repDeliveryMethod.DataSourceColumns = Nothing
        Me.repDeliveryMethod.DefaultSort = Nothing
        Me.repDeliveryMethod.DisplayField = ""
        Me.repDeliveryMethod.IsMultiSelect = False
        Me.repDeliveryMethod.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.repDeliveryMethod.Name = "repDeliveryMethod"
        Me.repDeliveryMethod.TableName = "SystemShippingMethod"
        Me.repDeliveryMethod.TargetDisplayField = Nothing
        Me.repDeliveryMethod.TargetValueMember = ""
        Me.repDeliveryMethod.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.repDeliveryMethod.UseCache = False
        Me.repDeliveryMethod.UseSpecifiedColumns = False
        Me.repDeliveryMethod.ValueMember = ""
        '
        'repSourceDeliveryMethod
        '
        Me.repSourceDeliveryMethod.AutoHeight = False
        Me.repSourceDeliveryMethod.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repSourceDeliveryMethod.Name = "repSourceDeliveryMethod"
        '
        'repSourceDeliveryClass
        '
        Me.repSourceDeliveryClass.AutoHeight = False
        Me.repSourceDeliveryClass.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repSourceDeliveryClass.Name = "repSourceDeliveryClass"
        '
        'ConfigSettingsDeliveryTranslationSectionLayoutGroup
        '
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.CustomizationFormText = "ConfigSettingsDeliveryTranslationSectionLayoutGroup"
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.GroupBordersVisible = False
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.Name = "ConfigSettingsDeliveryTranslationSectionLayoutGroup"
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.Size = New System.Drawing.Size(691, 419)
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.Text = "ConfigSettingsDeliveryTranslationSectionLayoutGroup"
        Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControlDeliveryTranslation
        Me.LayoutControlItem1.CustomizationFormText = "LayoutItemDeliveryTranslation"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(691, 419)
        Me.LayoutControlItem1.Text = " "
        Me.LayoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 5
        '
        'ConfigSettingsDeliveryTranslationSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.Controls.Add(Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "ConfigSettingsDeliveryTranslationSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(691, 419)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsDeliveryTranslationSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ConfigSettingsDeliveryTranslationSectionExtendedLayout.ResumeLayout(False)
        CType(Me.GridControlDeliveryTranslation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewDeliveryTranslation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repDeliveryMethodGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repDeliveryMethod, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repSourceDeliveryMethod, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repSourceDeliveryClass, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsDeliveryTranslationSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents ConfigSettingsDeliveryTranslationSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents ConfigSettingsDeliveryTranslationSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents ConfigSettingsDeliveryTranslationSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControlDeliveryTranslation As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewDeliveryTranslation As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents colSourceCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceDeliveryMethod_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShippingMethodGroup_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colShippingMethodCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repDeliveryMethodGroup As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
    Friend WithEvents repDeliveryMethod As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
    Friend WithEvents colSourceDeliveryClass_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceDeliveryMethodCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceDeliveryClassCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repSourceDeliveryMethod As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox
    Friend WithEvents repSourceDeliveryClass As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox

    Public ReadOnly Property ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_ConfigSettingsDeliveryTranslationDataset
        End Get
    End Property

End Class
#End Region
