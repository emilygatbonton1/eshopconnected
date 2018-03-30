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

#Region " InventoryManufacturerIDsSection "
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InventoryManufacturerIDsSection
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InventoryManufacturerIDsSection))
        Me.InventoryManufacturerIDsSectionGateway = Me.ImportExportDataset
        Me.InventoryManufacturerIDsSectionExtendedLayout = New Interprise.Presentation.Base.ExtendedLayoutControl(Me.components)
        Me.GridControlSourceManufacturerIDs = New DevExpress.XtraGrid.GridControl()
        Me.GridViewSourceManufacturerIDs = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colManufacturerCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colSourceCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repSourceCode = New Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl()
        Me.colAccountOrInstanceID_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.repAccountOrInstance = New DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox()
        Me.colSourceManufacturerCode_DEV000221 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.InventoryManufacturerIDsSectionLayoutGroup = New DevExpress.XtraLayout.LayoutControlGroup()
        Me.LayoutControlItem1 = New Interprise.Presentation.Base.ExtendedLayoutControlItem()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryManufacturerIDsSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryManufacturerIDsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InventoryManufacturerIDsSectionExtendedLayout.SuspendLayout()
        CType(Me.GridControlSourceManufacturerIDs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridViewSourceManufacturerIDs, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repSourceCode, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.repAccountOrInstance, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.InventoryManufacturerIDsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'InventoryManufacturerIDsSectionGateway
        '
        Me.InventoryManufacturerIDsSectionGateway.DataSetName = "InventoryManufacturerIDsSectionDataset"
        Me.InventoryManufacturerIDsSectionGateway.Instantiate = False
        Me.InventoryManufacturerIDsSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'InventoryManufacturerIDsSectionExtendedLayout
        '
        Me.InventoryManufacturerIDsSectionExtendedLayout.Controls.Add(Me.GridControlSourceManufacturerIDs)
        Me.InventoryManufacturerIDsSectionExtendedLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InventoryManufacturerIDsSectionExtendedLayout.Location = New System.Drawing.Point(0, 0)
        Me.InventoryManufacturerIDsSectionExtendedLayout.Name = "InventoryManufacturerIDsSectionExtendedLayout"
        Me.InventoryManufacturerIDsSectionExtendedLayout.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross
        Me.InventoryManufacturerIDsSectionExtendedLayout.Root = Me.InventoryManufacturerIDsSectionLayoutGroup
        Me.InventoryManufacturerIDsSectionExtendedLayout.Size = New System.Drawing.Size(783, 463)
        Me.InventoryManufacturerIDsSectionExtendedLayout.TabIndex = 0
        Me.InventoryManufacturerIDsSectionExtendedLayout.Text = "InventoryManufacturerIDsSectionExtendedLayout"
        '
        'GridControlSourceManufacturerIDs
        '
        Me.GridControlSourceManufacturerIDs.DataMember = "SystemManufacturerSourceID_DEV000221"
        Me.GridControlSourceManufacturerIDs.DataSource = Me.InventoryManufacturerIDsSectionGateway
        Me.ExtendControlProperty.SetGridPopupMenu(Me.GridControlSourceManufacturerIDs, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.GridControlSourceManufacturerIDs, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSourceManufacturerIDs.Location = New System.Drawing.Point(3, 3)
        Me.GridControlSourceManufacturerIDs.MainView = Me.GridViewSourceManufacturerIDs
        Me.GridControlSourceManufacturerIDs.Name = "GridControlSourceManufacturerIDs"
        Me.GridControlSourceManufacturerIDs.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.repSourceCode, Me.repAccountOrInstance})
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me.GridControlSourceManufacturerIDs, False)
        Me.GridControlSourceManufacturerIDs.Size = New System.Drawing.Size(777, 457)
        Me.GridControlSourceManufacturerIDs.TabIndex = 4
        Me.ExtendControlProperty.SetTextDisplay(Me.GridControlSourceManufacturerIDs, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.GridControlSourceManufacturerIDs.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridViewSourceManufacturerIDs})
        '
        'GridViewSourceManufacturerIDs
        '
        Me.GridViewSourceManufacturerIDs.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colManufacturerCode_DEV000221, Me.colSourceCode_DEV000221, Me.colAccountOrInstanceID_DEV000221, Me.colSourceManufacturerCode_DEV000221})
        Me.GridViewSourceManufacturerIDs.GridControl = Me.GridControlSourceManufacturerIDs
        Me.GridViewSourceManufacturerIDs.Name = "GridViewSourceManufacturerIDs"
        Me.GridViewSourceManufacturerIDs.OptionsView.ShowGroupPanel = False
        '
        'colManufacturerCode_DEV000221
        '
        Me.colManufacturerCode_DEV000221.Caption = "Manufacturer Code"
        Me.colManufacturerCode_DEV000221.FieldName = "ManufacturerCode_DEV000221"
        Me.colManufacturerCode_DEV000221.Name = "colManufacturerCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colManufacturerCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        '
        'colSourceCode_DEV000221
        '
        Me.colSourceCode_DEV000221.Caption = "Source Code"
        Me.colSourceCode_DEV000221.ColumnEdit = Me.repSourceCode
        Me.colSourceCode_DEV000221.FieldName = "SourceCode_DEV000221"
        Me.colSourceCode_DEV000221.Name = "colSourceCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceCode_DEV000221.Visible = True
        Me.colSourceCode_DEV000221.VisibleIndex = 0
        Me.colSourceCode_DEV000221.Width = 193
        '
        'repSourceCode
        '
        Me.repSourceCode.AdditionalFilter = " and HasSourceIDs_DEV000221 = 1"
        Me.repSourceCode.AllowEdit = True
        Me.repSourceCode.AutoHeight = False
        Me.repSourceCode.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repSourceCode.ColumnDescriptions = Nothing
        Me.repSourceCode.ColumnNames = Nothing
        Me.repSourceCode.DataSource = Nothing
        Me.repSourceCode.DataSourceColumns = Nothing
        Me.repSourceCode.DefaultSort = Nothing
        Me.repSourceCode.DisplayField = "SourceName_DEV000221"
        Me.repSourceCode.IsMultiSelect = False
        Me.repSourceCode.Movement = Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl.enmMovement.Vertical
        Me.repSourceCode.Name = "repSourceCode"
        Me.repSourceCode.TableName = "LerrynImportExportConfig_DEV000221"
        Me.repSourceCode.TargetValueMember = "SourceCode_DEV000221"
        Me.repSourceCode.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
        Me.repSourceCode.UseCache = False
        Me.repSourceCode.UseSpecifiedColumns = False
        Me.repSourceCode.ValueMember = "SourceCode_DEV000221"
        '
        'colAccountOrInstanceID_DEV000221
        '
        Me.colAccountOrInstanceID_DEV000221.Caption = "Account / InstanceID"
        Me.colAccountOrInstanceID_DEV000221.ColumnEdit = Me.repAccountOrInstance
        Me.colAccountOrInstanceID_DEV000221.FieldName = "AccountOrInstanceID_DEV000221"
        Me.colAccountOrInstanceID_DEV000221.Name = "colAccountOrInstanceID_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colAccountOrInstanceID_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colAccountOrInstanceID_DEV000221.Visible = True
        Me.colAccountOrInstanceID_DEV000221.VisibleIndex = 1
        Me.colAccountOrInstanceID_DEV000221.Width = 257
        '
        'repAccountOrInstance
        '
        Me.repAccountOrInstance.AutoHeight = False
        Me.repAccountOrInstance.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.repAccountOrInstance.Name = "repAccountOrInstance"
        '
        'colSourceManufacturerCode_DEV000221
        '
        Me.colSourceManufacturerCode_DEV000221.Caption = "Source Manufacturer Code/ID"
        Me.colSourceManufacturerCode_DEV000221.FieldName = "SourceManufacturerCode_DEV000221"
        Me.colSourceManufacturerCode_DEV000221.Name = "colSourceManufacturerCode_DEV000221"
        Me.ExtendControlProperty.SetTextDisplay(Me.colSourceManufacturerCode_DEV000221, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colSourceManufacturerCode_DEV000221.Visible = True
        Me.colSourceManufacturerCode_DEV000221.VisibleIndex = 2
        Me.colSourceManufacturerCode_DEV000221.Width = 166
        '
        'InventoryManufacturerIDsSectionLayoutGroup
        '
        Me.InventoryManufacturerIDsSectionLayoutGroup.CustomizationFormText = "InventoryManufacturerIDsSectionLayoutGroup"
        Me.InventoryManufacturerIDsSectionLayoutGroup.GroupBordersVisible = False
        Me.InventoryManufacturerIDsSectionLayoutGroup.Items.AddRange(New DevExpress.XtraLayout.BaseLayoutItem() {Me.LayoutControlItem1})
        Me.InventoryManufacturerIDsSectionLayoutGroup.Location = New System.Drawing.Point(0, 0)
        Me.InventoryManufacturerIDsSectionLayoutGroup.Name = "InventoryManufacturerIDsSectionLayoutGroup"
        Me.InventoryManufacturerIDsSectionLayoutGroup.Size = New System.Drawing.Size(783, 463)
        Me.InventoryManufacturerIDsSectionLayoutGroup.Text = "InventoryManufacturerIDsSectionLayoutGroup"
        Me.InventoryManufacturerIDsSectionLayoutGroup.TextVisible = False
        '
        'LayoutControlItem1
        '
        Me.LayoutControlItem1.Control = Me.GridControlSourceManufacturerIDs
        Me.LayoutControlItem1.CustomizationFormText = "LayoutControlItem1"
        Me.LayoutControlItem1.Location = New System.Drawing.Point(0, 0)
        Me.LayoutControlItem1.Name = "LayoutControlItem1"
        Me.LayoutControlItem1.Padding = New DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3)
        Me.LayoutControlItem1.Size = New System.Drawing.Size(783, 463)
        Me.LayoutControlItem1.Text = "LayoutControlItem1"
        Me.ExtendControlProperty.SetTextDisplay(Me.LayoutControlItem1, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.LayoutControlItem1.TextSize = New System.Drawing.Size(0, 0)
        Me.LayoutControlItem1.TextToControlDistance = 0
        Me.LayoutControlItem1.TextVisible = False
        '
        'InventoryManufacturerIDsSection
        '
        Me.Controls.Add(Me.InventoryManufacturerIDsSectionExtendedLayout)
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "InventoryManufacturerIDsSection"
        Me.ExtendControlProperty.SetSelectNextControlOnEnterKey(Me, False)
        Me.Size = New System.Drawing.Size(783, 463)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryManufacturerIDsSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryManufacturerIDsSectionExtendedLayout, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InventoryManufacturerIDsSectionExtendedLayout.ResumeLayout(False)
        CType(Me.GridControlSourceManufacturerIDs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridViewSourceManufacturerIDs, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repSourceCode, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.repAccountOrInstance, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.InventoryManufacturerIDsSectionLayoutGroup, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LayoutControlItem1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents InventoryManufacturerIDsSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents InventoryManufacturerIDsSectionExtendedLayout As Interprise.Presentation.Base.ExtendedLayoutControl
    Friend WithEvents InventoryManufacturerIDsSectionLayoutGroup As DevExpress.XtraLayout.LayoutControlGroup
    Friend WithEvents GridControlSourceManufacturerIDs As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridViewSourceManufacturerIDs As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents LayoutControlItem1 As Interprise.Presentation.Base.ExtendedLayoutControlItem
    Friend WithEvents colManufacturerCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colAccountOrInstanceID_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colSourceManufacturerCode_DEV000221 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents repSourceCode As Interprise.Presentation.Base.Search.RepHyperlinkSearchComboControl
    Friend WithEvents repAccountOrInstance As DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox

#Region "ImportExportDataset"
    Public ReadOnly Property ImportExportDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_InventoryManufacturerIDsDataset
        End Get
    End Property
#End Region

End Class
#End Region
