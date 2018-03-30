' eShopCONNECT for Connected Business
' Module: BulkPublishingWizardFilterSection.vb
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
' Last Updated - 04 January 2014

Option Explicit On
Option Strict On

Imports Interprise.Framework.Inventory.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Microsoft.VisualBasic

#Region " BulkPublishingWizardFilterSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterSection")> _
Public Class BulkPublishingWizardFilterSection
	
#Region " Variables "
    Private m_BulkPublishingWizardFilterDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_BulkPublishingWizardFilterSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_CategoryFilter As String = ""
    Private m_DepartmentFilter As String = ""
    Private m_ManufacturerFilter As String = ""
    Private m_StatusFilter As String = ""
    Private m_SupplierFilter As String = ""
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.BulkPublishingWizardFilterSectionGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_BulkPublishingWizardFilterSectionFacade
        End Get
    End Property
#End Region

#Region " CategoryFilter "
    Public Property ApplyCategoryFilter() As Boolean
        Set(value As Boolean)
            Me.chkApplyCategoryFilter.Checked = value
        End Set
        Get
            Return Me.chkApplyCategoryFilter.Checked
        End Get
    End Property

    Public Property CategoriesToFilter() As String
        Set(value As String)
            Dim iLoop As Integer
            m_CategoryFilter = value
            For iLoop = 0 To Me.m_BulkPublishingWizardFilterDataset.SystemCategoryView.Count - 1
                If InStr(m_CategoryFilter, "'" & Me.m_BulkPublishingWizardFilterDataset.SystemCategoryView(iLoop).Description & "'") > 0 Then
                    Me.m_BulkPublishingWizardFilterDataset.SystemCategoryView(iLoop).SelectCategory = True
                Else
                    Me.m_BulkPublishingWizardFilterDataset.SystemCategoryView(iLoop).SelectCategory = False
                End If
            Next
        End Set
        Get
            If Me.chkApplyCategoryFilter.Checked Then
                Dim sReturn As String, iLoop As Integer
                sReturn = ""
                For iLoop = 0 To Me.m_BulkPublishingWizardFilterDataset.SystemCategoryView.Count - 1
                    If Me.m_BulkPublishingWizardFilterDataset.SystemCategoryView(iLoop).SelectCategory Then
                        If sReturn <> "" Then
                            sReturn = sReturn & ","
                        End If
                        sReturn = sReturn & "'" & Me.m_BulkPublishingWizardFilterDataset.SystemCategoryView(iLoop).Description & "'"
                    End If
                Next
                Return sReturn
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " DepartmentFilter "
    Public Property ApplyDepartmentFilter() As Boolean
        Set(value As Boolean)
            Me.chkApplyDepartmentFilter.Checked = value
        End Set
        Get
            Return Me.chkApplyDepartmentFilter.Checked
        End Get
    End Property

    Public Property DepartmentsToFilter() As String
        Set(value As String)
            Dim iLoop As Integer
            m_DepartmentFilter = value
            For iLoop = 0 To Me.m_BulkPublishingWizardFilterDataset.InventorySellingDepartmentView.Count - 1
                If InStr(m_DepartmentFilter, "'" & Me.m_BulkPublishingWizardFilterDataset.InventorySellingDepartmentView(iLoop).Description & "'") > 0 Then
                    Me.m_BulkPublishingWizardFilterDataset.InventorySellingDepartmentView(iLoop).SelectDepartment = True
                Else
                    Me.m_BulkPublishingWizardFilterDataset.InventorySellingDepartmentView(iLoop).SelectDepartment = False
                End If
            Next
        End Set
        Get
            If Me.chkApplyDepartmentFilter.Checked Then
                Dim sReturn As String, iLoop As Integer
                sReturn = ""
                For iLoop = 0 To Me.m_BulkPublishingWizardFilterDataset.InventorySellingDepartmentView.Count - 1
                    If Me.m_BulkPublishingWizardFilterDataset.InventorySellingDepartmentView(iLoop).SelectDepartment Then
                        If sReturn <> "" Then
                            sReturn = sReturn & ","
                        End If
                        sReturn = sReturn & "'" & Me.m_BulkPublishingWizardFilterDataset.InventorySellingDepartmentView(iLoop).Description & "'"
                    End If
                Next
                Return sReturn
            Else
                Return ""
            End If
        End Get
    End Property
#End Region

#Region " ManufacturerFilter "
    Public Property ApplyManufacturerFilter() As Boolean
        Set(value As Boolean)
            Me.chkApplyManufacturerFilter.Checked = value
        End Set
        Get
            Return Me.chkApplyManufacturerFilter.Checked
        End Get
    End Property

    Public Property ManufacturerToFilter() As String
        Set(value As String)
            Me.cbeManufacturerFilter.EditValue = value
        End Set
        Get
            Return Me.cbeManufacturerFilter.EditValue.ToString
        End Get
    End Property
#End Region

#Region " StatusFilter "
    Public Property ApplyStatusFilter() As Boolean
        Set(value As Boolean)
            Me.chkApplyStatusFilter.Checked = value
        End Set
        Get
            Return Me.chkApplyStatusFilter.Checked
        End Get
    End Property

    Public Property StatusToFilter() As String
        Set(value As String)
            Me.cbeStatusFilter.EditValue = value
        End Set
        Get
            Return Me.cbeStatusFilter.EditValue.ToString
        End Get
    End Property
#End Region

#Region " SupplierFilter "
    Public Property ApplySupplierFilter() As Boolean
        Set(value As Boolean)
            Me.chkApplySupplierFilter.Checked = value
        End Set
        Get
            Return Me.chkApplySupplierFilter.Checked
        End Get
    End Property

    Public Property SupplierToFilter() As String
        Set(value As String)
            Me.cbeSupplierFilter.EditValue = value
        End Set
        Get
            Return Me.cbeSupplierFilter.EditValue.ToString
        End Get
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New()
        MyBase.New()

        Me.m_BulkPublishingWizardFilterDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        ' To solve this error, you must use any facade other than the two:
        Me.m_BulkPublishingWizardFilterSectionFacade = New Facade.ImportExport.ImportExportConfigFacade(Me.m_BulkPublishingWizardFilterDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

    End Sub

    Public Sub New(ByVal BulkPublishingWizardFilterDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
       ByVal BulkPublishingWizardFilterSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
        MyBase.New()

        Me.m_BulkPublishingWizardFilterDataset = BulkPublishingWizardFilterDataset
        Me.m_BulkPublishingWizardFilterSectionFacade = BulkPublishingWizardFilterSectionFacade


        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return
    End Sub
#End Region

#Region " InitializeControl "
    Protected Overrides Sub InitializeControl()
        'This call is required by the Presentation Layer.
        MyBase.InitializeControl()

        'Add any initialization after the InitializeControl() call

    End Sub
#End Region
#End Region

#Region " Events "
#Region " ApplyCategoryFilterChanged "
    Private Sub ApplyCategoryFilterChanged(sender As Object, e As System.EventArgs) Handles chkApplyCategoryFilter.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkApplyCategoryFilter.Checked Then
            Me.TreeListCategoryFilter.Enabled = True
            Me.m_BulkPublishingWizardFilterSectionFacade.LoadDataSet(New String()() {New String() {Me.BulkPublishingWizardFilterSectionGateway.SystemCategoryView.TableName, _
                "ReadSystemCategoryView_DEV000221", AT_LANGUAGE_CODE, Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)}}, _
                Interprise.Framework.Base.Shared.ClearType.Specific)
        Else
            Me.TreeListCategoryFilter.Enabled = False
        End If

    End Sub
#End Region

#Region " ApplyDepartmentFilterChanged "
    Private Sub ApplyDepartmentFilterChanged(sender As Object, e As System.EventArgs) Handles chkApplyDepartmentFilter.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkApplyDepartmentFilter.Checked Then
            Me.TreeListDepartmentFilter.Enabled = True
            Me.m_BulkPublishingWizardFilterSectionFacade.LoadDataSet(New String()() {New String() {Me.BulkPublishingWizardFilterSectionGateway.InventorySellingDepartmentView.TableName, _
                "ReadInventorySellingDepartmentView_DEV000221", AT_LANGUAGE_CODE, Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing), _
                AT_ISACTIVE, "1"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
        Else
            Me.TreeListDepartmentFilter.Enabled = False
        End If

    End Sub
#End Region

#Region " ApplyManufacturerFilterChanged "
    Private Sub ApplyManufacturerFilterChanged(sender As Object, e As System.EventArgs) Handles chkApplyManufacturerFilter.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim strManufacturerList As String()()

        If Me.chkApplyManufacturerFilter.Checked Then
            Me.cbeManufacturerFilter.Enabled = True
            strManufacturerList = Me.m_BulkPublishingWizardFilterSectionFacade.GetRows(New String() {"ManufacturerCode", "Description"}, "SystemManufacturerDescription", "LanguageCode = '" & Me.m_BulkPublishingWizardFilterSectionFacade.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing) & "' and ManufacturerCode IN (SELECT ManufacturerCode FROM SystemManufacturer WHERE IsActive = 1)")
            Coll = Me.cbeManufacturerFilter.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            For Each strManufacturer As String() In strManufacturerList
                CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
                CollItem.Description = strManufacturer(1)
                CollItem.Value = strManufacturer(0)
                Coll.Add(CollItem)
            Next
            Coll.EndUpdate()
        Else
            Me.cbeManufacturerFilter.Enabled = False
        End If

    End Sub
#End Region

#Region " ApplyStatusFilterChanged "
    Private Sub ApplyStatusFilterChanged(sender As Object, e As System.EventArgs) Handles chkApplyStatusFilter.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

        If Me.chkApplyStatusFilter.Checked Then
            Me.cbeStatusFilter.Enabled = True
            Coll = Me.cbeStatusFilter.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Active"
            CollItem.Value = "A"
            Coll.Add(CollItem)
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Discontinued"
            CollItem.Value = "P"
            Coll.Add(CollItem)
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Phased Out"
            CollItem.Value = "P"
            Coll.Add(CollItem)
            Coll.EndUpdate()
        Else
            Me.cbeStatusFilter.Enabled = False
        End If

    End Sub
#End Region

#Region " ApplySupplierFilterChanged "
    Private Sub ApplySupplierFilterChanged(sender As Object, e As System.EventArgs) Handles chkApplySupplierFilter.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -  
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim strSupplierList As String()()

        If Me.chkApplySupplierFilter.Checked Then
            Me.cbeSupplierFilter.Enabled = True
            strSupplierList = Me.m_BulkPublishingWizardFilterSectionFacade.GetRows(New String() {"SupplierCode", "SupplierName"}, "Supplier", Nothing)
            Coll = Me.cbeSupplierFilter.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            For Each strSupplier As String() In strSupplierList
                CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
                CollItem.Description = strSupplier(1)
                CollItem.Value = strSupplier(0)
                Coll.Add(CollItem)
            Next
            Coll.EndUpdate()
        Else
            Me.cbeSupplierFilter.Enabled = False
        End If

    End Sub
#End Region
#End Region

End Class
#End Region

