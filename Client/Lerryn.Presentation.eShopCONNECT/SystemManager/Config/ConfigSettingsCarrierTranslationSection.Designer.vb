<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigSettingsCarrierTranslationSection
    Inherits Interprise.Presentation.Base.BaseControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ConfigSettingsCarrierTranslationSection))
        Me.ConfigSettingsCarrierTranslationSectionGateway = Me.ImportExportDataset
        Me.DataColumn1 = New System.Data.DataColumn()
        Me.DataColumn2 = New System.Data.DataColumn()
        Me.CBCarrierCodeSearchComboControl = New Interprise.Presentation.Base.Search.RepSearchComboControl()
        Me.CBServiceTypeSearchComboControl = New Interprise.Presentation.Base.Search.RepSearchComboControl()
        Me.MarketplaceCarrierCodeSearchComboControl = New Interprise.Presentation.Base.Search.RepSearchComboControl()
        Me.MarketplaceServiceTypeComboControl = New Interprise.Presentation.Base.Search.RepSearchComboControl()
        Me.CarrierTranslationGridView = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.colCBCarrierCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCBServiceType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMarketplaceCarrierCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colMarketplaceServiceType = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colWarehouseCode = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.colCarrierDescription = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.CarrierTranslationGridControl = New DevExpress.XtraGrid.GridControl()
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ConfigSettingsCarrierTranslationSectionGateway, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CBCarrierCodeSearchComboControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CBServiceTypeSearchComboControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MarketplaceCarrierCodeSearchComboControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MarketplaceServiceTypeComboControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CarrierTranslationGridView, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CarrierTranslationGridControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageCollectionContextMenu
        '
        Me.ImageCollectionContextMenu.ImageStream = CType(resources.GetObject("ImageCollectionContextMenu.ImageStream"), DevExpress.Utils.ImageCollectionStreamer)
        '
        'ConfigSettingsCarrierTranslationSectionGateway
        '
        Me.ConfigSettingsCarrierTranslationSectionGateway.DataSetName = "ConfigSettingsCarrierTranslationSectionDataset"
        Me.ConfigSettingsCarrierTranslationSectionGateway.Instantiate = False
        Me.ConfigSettingsCarrierTranslationSectionGateway.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
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
        'DataColumn1
        '
        Me.DataColumn1.ColumnName = "Column1"
        '
        'DataColumn2
        '
        Me.DataColumn2.ColumnName = "Column2"
        '
        'CBCarrierCodeSearchComboControl
        '
        Me.CBCarrierCodeSearchComboControl.AdditionalFilter = Nothing
        Me.CBCarrierCodeSearchComboControl.AllowEdit = False
        Me.CBCarrierCodeSearchComboControl.AutoHeight = False
        Me.CBCarrierCodeSearchComboControl.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CBCarrierCodeSearchComboControl.ColumnDescriptions = New String(-1) {}
        Me.CBCarrierCodeSearchComboControl.ColumnNames = Nothing
        Me.CBCarrierCodeSearchComboControl.DataSource = Nothing
        Me.CBCarrierCodeSearchComboControl.DataSourceColumns = New String(-1) {}
        Me.CBCarrierCodeSearchComboControl.DefaultSort = Nothing
        Me.CBCarrierCodeSearchComboControl.DisplayField = Nothing
        Me.CBCarrierCodeSearchComboControl.IsMultiSelect = False
        Me.CBCarrierCodeSearchComboControl.Movement = Interprise.Presentation.Base.Search.RepSearchComboControl.enmMovement.Vertical
        Me.CBCarrierCodeSearchComboControl.Name = "CBCarrierCodeSearchComboControl"
        Me.CBCarrierCodeSearchComboControl.SkipDataValidation = False
        Me.CBCarrierCodeSearchComboControl.TableName = "ShipmentCarrier"
        Me.CBCarrierCodeSearchComboControl.TargetDisplayField = "CBCarrierCode_DEV000221"
        Me.CBCarrierCodeSearchComboControl.TargetValueMember = "CBCarrierCode_DEV000221"
        Me.CBCarrierCodeSearchComboControl.UseCache = False
        Me.CBCarrierCodeSearchComboControl.UseSpecifiedColumns = False
        Me.CBCarrierCodeSearchComboControl.ValueMember = Nothing
        '
        'CBServiceTypeSearchComboControl
        '
        Me.CBServiceTypeSearchComboControl.AdditionalFilter = Nothing
        Me.CBServiceTypeSearchComboControl.AllowEdit = False
        Me.CBServiceTypeSearchComboControl.AutoHeight = False
        Me.CBServiceTypeSearchComboControl.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.CBServiceTypeSearchComboControl.ColumnDescriptions = Nothing
        Me.CBServiceTypeSearchComboControl.ColumnNames = Nothing
        Me.CBServiceTypeSearchComboControl.DataSource = Nothing
        Me.CBServiceTypeSearchComboControl.DataSourceColumns = Nothing
        Me.CBServiceTypeSearchComboControl.DefaultSort = Nothing
        Me.CBServiceTypeSearchComboControl.DisplayField = Nothing
        Me.CBServiceTypeSearchComboControl.IsMultiSelect = False
        Me.CBServiceTypeSearchComboControl.Movement = Interprise.Presentation.Base.Search.RepSearchComboControl.enmMovement.Vertical
        Me.CBServiceTypeSearchComboControl.Name = "CBServiceTypeSearchComboControl"
        Me.CBServiceTypeSearchComboControl.SkipDataValidation = False
        Me.CBServiceTypeSearchComboControl.TableName = "ShipmentCarrierServices"
        Me.CBServiceTypeSearchComboControl.TargetDisplayField = Nothing
        Me.CBServiceTypeSearchComboControl.TargetValueMember = Nothing
        Me.CBServiceTypeSearchComboControl.UseCache = False
        Me.CBServiceTypeSearchComboControl.UseSpecifiedColumns = False
        Me.CBServiceTypeSearchComboControl.ValueMember = Nothing
        '
        'MarketplaceCarrierCodeSearchComboControl
        '
        Me.MarketplaceCarrierCodeSearchComboControl.AdditionalFilter = Nothing
        Me.MarketplaceCarrierCodeSearchComboControl.AllowEdit = False
        Me.MarketplaceCarrierCodeSearchComboControl.AutoHeight = False
        Me.MarketplaceCarrierCodeSearchComboControl.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.MarketplaceCarrierCodeSearchComboControl.ColumnDescriptions = New String(-1) {}
        Me.MarketplaceCarrierCodeSearchComboControl.ColumnNames = Nothing
        Me.MarketplaceCarrierCodeSearchComboControl.DataSource = Nothing
        Me.MarketplaceCarrierCodeSearchComboControl.DataSourceColumns = New String() {"CarrierCode"}
        Me.MarketplaceCarrierCodeSearchComboControl.DefaultSort = Nothing
        Me.MarketplaceCarrierCodeSearchComboControl.DisplayField = Nothing
        Me.MarketplaceCarrierCodeSearchComboControl.IsMultiSelect = False
        Me.MarketplaceCarrierCodeSearchComboControl.Movement = Interprise.Presentation.Base.Search.RepSearchComboControl.enmMovement.Vertical
        Me.MarketplaceCarrierCodeSearchComboControl.Name = "MarketplaceCarrierCodeSearchComboControl"
        Me.MarketplaceCarrierCodeSearchComboControl.SkipDataValidation = False
        Me.MarketplaceCarrierCodeSearchComboControl.TableName = "eShopAmazonCarrierCodes_DEV000221"
        Me.MarketplaceCarrierCodeSearchComboControl.TargetDisplayField = Nothing
        Me.MarketplaceCarrierCodeSearchComboControl.TargetValueMember = Nothing
        Me.MarketplaceCarrierCodeSearchComboControl.UseCache = False
        Me.MarketplaceCarrierCodeSearchComboControl.UseSpecifiedColumns = False
        Me.MarketplaceCarrierCodeSearchComboControl.ValueMember = Nothing
        '
        'MarketplaceServiceTypeComboControl
        '
        Me.MarketplaceServiceTypeComboControl.AdditionalFilter = Nothing
        Me.MarketplaceServiceTypeComboControl.AllowEdit = False
        Me.MarketplaceServiceTypeComboControl.AutoHeight = False
        Me.MarketplaceServiceTypeComboControl.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.MarketplaceServiceTypeComboControl.ColumnDescriptions = Nothing
        Me.MarketplaceServiceTypeComboControl.ColumnNames = Nothing
        Me.MarketplaceServiceTypeComboControl.DataSource = Nothing
        Me.MarketplaceServiceTypeComboControl.DataSourceColumns = Nothing
        Me.MarketplaceServiceTypeComboControl.DefaultSort = Nothing
        Me.MarketplaceServiceTypeComboControl.DisplayField = Nothing
        Me.MarketplaceServiceTypeComboControl.IsMultiSelect = False
        Me.MarketplaceServiceTypeComboControl.Movement = Interprise.Presentation.Base.Search.RepSearchComboControl.enmMovement.Vertical
        Me.MarketplaceServiceTypeComboControl.Name = "MarketplaceServiceTypeComboControl"
        Me.MarketplaceServiceTypeComboControl.SkipDataValidation = False
        Me.MarketplaceServiceTypeComboControl.TableName = Nothing
        Me.MarketplaceServiceTypeComboControl.TargetDisplayField = Nothing
        Me.MarketplaceServiceTypeComboControl.TargetValueMember = Nothing
        Me.MarketplaceServiceTypeComboControl.UseCache = False
        Me.MarketplaceServiceTypeComboControl.UseSpecifiedColumns = False
        Me.MarketplaceServiceTypeComboControl.ValueMember = Nothing
        '
        'CarrierTranslationGridView
        '
        Me.CarrierTranslationGridView.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.colCBCarrierCode, Me.colCBServiceType, Me.colMarketplaceCarrierCode, Me.colMarketplaceServiceType, Me.colWarehouseCode, Me.colCarrierDescription})
        Me.CarrierTranslationGridView.GridControl = Me.CarrierTranslationGridControl
        Me.CarrierTranslationGridView.Name = "CarrierTranslationGridView"
        Me.CarrierTranslationGridView.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom
        Me.CarrierTranslationGridView.OptionsView.ShowGroupPanel = False
        '
        'colCBCarrierCode
        '
        Me.colCBCarrierCode.Caption = "CB Carrier Code"
        Me.colCBCarrierCode.ColumnEdit = Me.CBCarrierCodeSearchComboControl
        Me.colCBCarrierCode.FieldName = "CBCarrierCode_DEV000221"
        Me.colCBCarrierCode.Name = "colCBCarrierCode"
        Me.ExtendControlProperty.SetTextDisplay(Me.colCBCarrierCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colCBCarrierCode.Visible = True
        Me.colCBCarrierCode.VisibleIndex = 0
        '
        'colCBServiceType
        '
        Me.colCBServiceType.Caption = "CB Service Type"
        Me.colCBServiceType.ColumnEdit = Me.CBServiceTypeSearchComboControl
        Me.colCBServiceType.FieldName = "CBServiceType_DEV000221"
        Me.colCBServiceType.Name = "colCBServiceType"
        Me.ExtendControlProperty.SetTextDisplay(Me.colCBServiceType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colCBServiceType.Visible = True
        Me.colCBServiceType.VisibleIndex = 3
        '
        'colMarketplaceCarrierCode
        '
        Me.colMarketplaceCarrierCode.Caption = "Marketplace Carrier Code"
        Me.colMarketplaceCarrierCode.ColumnEdit = Me.MarketplaceCarrierCodeSearchComboControl
        Me.colMarketplaceCarrierCode.FieldName = "MarketplaceCarrierCode_DEV000221"
        Me.colMarketplaceCarrierCode.Name = "colMarketplaceCarrierCode"
        Me.ExtendControlProperty.SetTextDisplay(Me.colMarketplaceCarrierCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colMarketplaceCarrierCode.Visible = True
        Me.colMarketplaceCarrierCode.VisibleIndex = 4
        '
        'colMarketplaceServiceType
        '
        Me.colMarketplaceServiceType.Caption = "Marketplace Service Type"
        Me.colMarketplaceServiceType.ColumnEdit = Me.MarketplaceServiceTypeComboControl
        Me.colMarketplaceServiceType.FieldName = "MarketplaceServiceType_DEV000221"
        Me.colMarketplaceServiceType.Name = "colMarketplaceServiceType"
        Me.ExtendControlProperty.SetTextDisplay(Me.colMarketplaceServiceType, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colMarketplaceServiceType.Visible = True
        Me.colMarketplaceServiceType.VisibleIndex = 5
        '
        'colWarehouseCode
        '
        Me.colWarehouseCode.Caption = "Warehouse Code"
        Me.colWarehouseCode.FieldName = "WarehouseCode"
        Me.colWarehouseCode.Name = "colWarehouseCode"
        Me.colWarehouseCode.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colWarehouseCode, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colWarehouseCode.Visible = True
        Me.colWarehouseCode.VisibleIndex = 2
        '
        'colCarrierDescription
        '
        Me.colCarrierDescription.Caption = "Carrier Description"
        Me.colCarrierDescription.FieldName = "CarrierDescription"
        Me.colCarrierDescription.Name = "colCarrierDescription"
        Me.colCarrierDescription.OptionsColumn.ReadOnly = True
        Me.ExtendControlProperty.SetTextDisplay(Me.colCarrierDescription, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.colCarrierDescription.Visible = True
        Me.colCarrierDescription.VisibleIndex = 1
        '
        'CarrierTranslationGridControl
        '
        Me.CarrierTranslationGridControl.DataMember = "eShopCarrierTranslationView_DEV000221"
        Me.CarrierTranslationGridControl.DataSource = Me.ConfigSettingsCarrierTranslationSectionGateway
        Me.ExtendControlProperty.SetGridPopupMenu(Me.CarrierTranslationGridControl, New Interprise.Presentation.Base.GridPopupMenu(Nothing, Nothing))
        Me.ExtendControlProperty.SetHelpText(Me.CarrierTranslationGridControl, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CarrierTranslationGridControl.Location = New System.Drawing.Point(3, 0)
        Me.CarrierTranslationGridControl.MainView = Me.CarrierTranslationGridView
        Me.CarrierTranslationGridControl.Name = "CarrierTranslationGridControl"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me.CarrierTranslationGridControl, System.Drawing.Color.Empty)
        Me.CarrierTranslationGridControl.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.CBCarrierCodeSearchComboControl, Me.CBServiceTypeSearchComboControl, Me.MarketplaceCarrierCodeSearchComboControl, Me.MarketplaceServiceTypeComboControl})
        Me.CarrierTranslationGridControl.Size = New System.Drawing.Size(803, 491)
        Me.CarrierTranslationGridControl.TabIndex = 0
        Me.ExtendControlProperty.SetTextDisplay(Me.CarrierTranslationGridControl, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.CarrierTranslationGridControl.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.CarrierTranslationGridView})
        '
        'ConfigSettingsCarrierTranslationSection
        '
        Me.Appearance.BackColor = System.Drawing.Color.Transparent
        Me.Appearance.Options.UseBackColor = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.CarrierTranslationGridControl)
        Me.FindSearch = Interprise.Framework.Base.[Shared].[Enum].FindSearch.None
        Me.ExtendControlProperty.SetHelpText(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.Name = "ConfigSettingsCarrierTranslationSection"
        Me.ExtendControlProperty.SetReadOnlyBackColor(Me, System.Drawing.Color.Empty)
        Me.Size = New System.Drawing.Size(809, 491)
        Me.ExtendControlProperty.SetTextDisplay(Me, New Interprise.Presentation.Base.DisplayText(Nothing, Nothing, Nothing))
        Me.TransactionType = Interprise.Framework.Base.[Shared].[Enum].TransactionType.None
        CType(Me.ImageCollectionContextMenu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ConfigSettingsCarrierTranslationSectionGateway, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CBCarrierCodeSearchComboControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CBServiceTypeSearchComboControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MarketplaceCarrierCodeSearchComboControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MarketplaceServiceTypeComboControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CarrierTranslationGridView, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CarrierTranslationGridControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ConfigSettingsCarrierTranslationSectionGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Friend WithEvents DataColumn1 As System.Data.DataColumn
    Friend WithEvents DataColumn2 As System.Data.DataColumn
    Friend WithEvents CBCarrierCodeSearchComboControl As Interprise.Presentation.Base.Search.RepSearchComboControl
    Friend WithEvents CBServiceTypeSearchComboControl As Interprise.Presentation.Base.Search.RepSearchComboControl
    Friend WithEvents MarketplaceCarrierCodeSearchComboControl As Interprise.Presentation.Base.Search.RepSearchComboControl
    Friend WithEvents MarketplaceServiceTypeComboControl As Interprise.Presentation.Base.Search.RepSearchComboControl
    Friend WithEvents CarrierTranslationGridView As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents colCBCarrierCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCBServiceType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMarketplaceCarrierCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colMarketplaceServiceType As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colWarehouseCode As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents colCarrierDescription As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents CarrierTranslationGridControl As DevExpress.XtraGrid.GridControl


End Class
