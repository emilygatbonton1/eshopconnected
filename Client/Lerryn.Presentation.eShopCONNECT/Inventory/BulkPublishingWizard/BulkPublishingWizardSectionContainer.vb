' eShopCONNECT for Connected Business
' Module: BulkPublishingWizardSectionContainer.vb
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
' Last Updated - 04 January 2014

Option Explicit On
Option Strict On

Imports Interprise.Framework.Base.Shared.Const
Imports Interprise.Framework.Inventory.Shared.Const
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports Microsoft.VisualBasic
Imports System.Xml.Linq
Imports System.Xml.XPath

#Region " BulkPublishingWizardSectionContainer "
Public Class BulkPublishingWizardSectionContainer

#Region " Variables "
    Private m_BulkPublishWizardsectionContainerFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private WithEvents m_BackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing
    Private WithEvents m_MagentoImportFacade As Lerryn.Facade.ImportExport.MagentoImportFacade
    Private m_CategoryTable As System.Data.DataTable = Nothing
    Private m_ItemsToPublish As System.Data.DataView = Nothing ' TJS 09/12/13
    Private m_PublishingOptions As Lerryn.Facade.ImportExport.MagentoImportFacade.MagentoPublishingOptions ' TJS 15/11/13 TJS 09/12/13
    Private m_GetSourceCategoriesSuccess As Boolean
    Private m_GetSourceAttributeSetsSuccess As Boolean ' TJS 13/11/13
    Private m_GetSourceAttributeListSuccess As Boolean ' TJS 13/11/13
    Private m_PublishAttributesAndValuesSuccess As Boolean ' TJS 13/11/13
    Private GetAttributesForSetID As Integer = -1
    Private m_NoOfItemsProcessed As Integer
    Private m_NoOfItemsPublished As Integer
    Private m_NoOfItemsSkipped As Integer
    Private m_ErrorDetails As String
    Private bIgnoreWizardPageChangeEvent As Boolean = False
    Private bCategoriesCreated As Boolean = False ' TJs 13/11/13
    Private m_PublishingTarget As String = "" ' TJS 09/12/13
    Private m_SiteOrAccount As String = "" ' TJS 09/12/13
    Private m_CategoryFilter As String = "" ' TJS 13/11/13
    Private m_DepartmentFilter As String = "" ' TJS 13/11/13
    Private m_ManufacturerFilter As String = "" ' TJS 13/11/13
    Private m_StatusFilter As String = "" ' TJS 13/11/13
    Private m_SupplierFilter As String = "" ' TJS 13/11/13
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.BulkPublisingWizardSectionContainerGateway
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_BulkPublishWizardsectionContainerFacade

        End Get
    End Property
#End Region

#Region " IsActivated "
    Public ReadOnly Property IsActivated() As Boolean
        Get
            If m_BulkPublishWizardsectionContainerFacade IsNot Nothing Then
                Return m_BulkPublishWizardsectionContainerFacade.IsActivated
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
        MyBase.New()

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        Me.m_BulkPublishWizardsectionContainerFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.BulkPublisingWizardSectionContainerGateway, _
            New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

    End Sub
#End Region

#Region " InitializeControls "
    Public Sub InitializeControls()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected message text
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim strWelcomeMessage As String

        'Add any initialization after the InitializeControl() call
        If m_BulkPublishWizardsectionContainerFacade.IsActivated Then
            strWelcomeMessage = "This wizard will guide you through the bulk publishing of your existing Inventory Items to a supported external marketplace or shopping cart." & _
                vbCrLf & vbCrLf & "Please select the Target you wish to publish Inventory Items to." & vbCrLf & vbCrLf & "NOTE  You can only publish to Targets that you have already activated." ' TJS 13/11/13
            Coll = Me.cbePublishingTarget.Properties.Items
            Coll.BeginUpdate()
            Coll.Clear()
            CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem("Magento")
            Coll.Add(CollItem)
            Coll.EndUpdate()
        Else
            strWelcomeMessage = "You must activate eShopCONNECTED first."
        End If
        Me.WizardControlBulkPublish.WelcomeMessage = strWelcomeMessage

    End Sub
#End Region

#Region " GetSourceCategoriesForPublishing "
    Private Sub GetSourceCategoriesForPublishing(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to also get Magento Attributes list and values
        '                                        | and stop on first error
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Select Case m_PublishingTarget ' TJS 09/12/13
            Case "Magento"
                m_GetSourceCategoriesSuccess = m_MagentoImportFacade.GetMagentoCategories(m_SiteOrAccount, worker.CancellationPending, m_CategoryTable) ' TJS 09/12/13

                If m_GetSourceCategoriesSuccess Then ' TJS 13/11/13
                    m_GetSourceAttributeSetsSuccess = m_MagentoImportFacade.GetMagentoAttributeSets(m_SiteOrAccount, worker.CancellationPending) ' TJS 13/11/13 TJS 09/12/13
                    If m_GetSourceAttributeSetsSuccess Then ' TJS 13/11/13
                        m_GetSourceAttributeListSuccess = m_MagentoImportFacade.GetMagentoAttributeList(m_SiteOrAccount, worker.CancellationPending) ' TJS 13/11/13 TJS 09/12/13

                    End If
                End If
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
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to display errors from attribute sets and lists
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

        If m_GetSourceCategoriesSuccess Then
            Me.TreeListCategories.DataSource = m_CategoryTable
            Me.TreeListCategories.ExpandAll()
            Me.TreeListCategories.Enabled = True
            Me.pnlGetCategoryProgress.Visible = False

            If m_MagentoImportFacade.MagentoAttributeSets.Length > 0 And m_GetSourceAttributeSetsSuccess Then ' TJS 13/11/13
                Coll = Me.ImageComboBoxEditAttributeSet.Properties.Items
                Coll.BeginUpdate()
                Coll.Clear()
                For Each AttributeSet In m_MagentoImportFacade.MagentoAttributeSets
                    CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
                    CollItem.Description = AttributeSet.SetName
                    CollItem.Value = AttributeSet.SetID
                    Coll.Add(CollItem)
                Next
                Coll.EndUpdate()
                ' start of code added TJS 13/11/13
                If Not m_GetSourceAttributeListSuccess Then
                    Select Case Me.cbePublishingTarget.EditValue.ToString
                        Case "Magento"
                            Interprise.Presentation.Base.Message.MessageWindow.Show(m_MagentoImportFacade.LastError)

                    End Select
                Else
                    ' start of code moved TJS 13/11/13
                    Me.lblSelectCategories.Enabled = True
                    Me.lblSelectAttributeSet.Enabled = True
                    Me.ImageComboBoxEditAttributeSet.Enabled = True
                    Me.WizardControlBulkPublish.EnableNextButton = True
                    ' end of code moved TJS 13/11/13
                End If
            Else
                Select Case Me.cbePublishingTarget.EditValue.ToString
                    Case "Magento"
                        Interprise.Presentation.Base.Message.MessageWindow.Show(m_MagentoImportFacade.LastError)

                End Select
                ' end of code added TJS 13/11/13
            End If

        Else
            Select Case Me.cbePublishingTarget.EditValue.ToString
                Case "Magento"
                    Interprise.Presentation.Base.Message.MessageWindow.Show(m_MagentoImportFacade.LastError)

            End Select
            Me.pnlGetCategoryProgress.Visible = False ' TJS 13/11/13

        End If
        RemoveHandler m_BackgroundWorker.DoWork, AddressOf GetSourceCategoriesForPublishing
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetSourceCategoriesCompleted

    End Sub
#End Region

#Region " PublishAttributes "
    Private Sub PublishAttributes(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ' 15/11/13 | TJS             | 2013.3.09 | Moved code to clear attribute pulishing errors to WizardPageChanged
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Select Case m_PublishingTarget ' TJS 09/12/13
            Case "Magento"
                m_PublishAttributesAndValuesSuccess = m_MagentoImportFacade.PublishAttributesAndValues(m_SiteOrAccount, worker.CancellationPending, m_ErrorDetails) ' TJS 09/12/13

        End Select

    End Sub

    Private Sub PublishAttributesCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strMessage As String

        If m_PublishAttributesAndValuesSuccess Then
            ' don't set Ignore page change event as we want to process settings
            ' can't do Attribute Sets yet as we can't get structure from the template
            'Me.WizardControlBulkPublish.FocusPage = Me.TabPageAttributeSets
            ' do we have any Attribute Sets in MAgento ?
            If m_MagentoImportFacade.MagentoAttributeSets.Length = 0 Then
                ' no
                strMessage = "Due to omissions in the Magento API, we cannot currently" & vbCrLf & "create Attribute Sets in Magento" & vbCrLf & vbCrLf
                strMessage = strMessage & "As Magento requires Attribute Sets in order to publish products," & vbCrLf & "please exit this Wizard and create the Attribute Sets in Magento,"
                strMessage = strMessage & vbCrLf & "then re-run this Wizard to complet the publishing process."
                Me.WizardControlBulkPublish.FocusPage = Me.TabPageComplete

            Else
                ' so go straight to Select Items to publish
                Me.WizardControlBulkPublish.FocusPage = Me.TabPageSelectItems

            End If
            Me.WizardControlBulkPublish.EnableNextButton = True
        Else
            Me.lblAttributePublishingErrors.Text = "The following errors were encountered whilst publishing your Attributes" & vbCrLf & vbCrLf
            Select Case Me.cbePublishingTarget.EditValue.ToString
                Case "Magento"
                    Me.lblAttributePublishingErrors.Text = Me.lblAttributePublishingErrors.Text & m_MagentoImportFacade.ProcessLog
            End Select
            Me.lblAttributePublishingErrors.Text = Me.lblAttributePublishingErrors.Text & vbCrLf & vbCrLf & "Please correct these errors before any Inventory Items can be published."
        End If

        Me.pnlPublishingAttributesProgress.Visible = False

        RemoveHandler m_BackgroundWorker.DoWork, AddressOf PublishAttributes
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf PublishAttributesCompleted

    End Sub
#End Region

#Region " GetAttributesForSet "
    Private Sub GetAttributesForSet(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        Select Case m_PublishingTarget ' TJS 09/12/13
            Case "Magento"
                m_MagentoImportFacade.GetMagentoAttributesForSet(m_SiteOrAccount, worker.CancellationPending, GetAttributesForSetID) ' TJS 09/12/13

        End Select

    End Sub

    Private Sub GetAttributesForSetCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Added code to populate attribute list from WizardPageChanged
        ' 15/11/13 | TJS             | 2013.3.09 | Modifid to onlynlist attributes in current attribute set
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to add tax_class_id as a parameter instead of excluding it
        ' 04/01/14 | TJS             | 2013.4.03 | Modified to include weight in list to be completed automatically
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowMagentoTagDetails As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.InventoryMagentoTagDetails_DEV000221Row
        Dim iLoop As Integer, iAttributeLoop As Integer ' TJS 15/11/13
        Dim bTagExists As Boolean, bTagsUpdated As Boolean, bAddToList2 As Boolean ' TJS 13/11/13
        Dim bWeightAddedToList As Boolean ' TJS 04/01/14

        If m_MagentoImportFacade.ProductAttributes IsNot Nothing Then
            bTagsUpdated = False
            For Each ProductAttribute In m_MagentoImportFacade.ProductAttributes
                bTagExists = False
                For iLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221.Count - 1
                    If Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221(iLoop).TagName_DEV000221 = ProductAttribute.AttributeName Then
                        bTagExists = True
                        If Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221(iLoop).IsTagRequired_DEV000221Null OrElse _
                            Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221(iLoop).TagRequired_DEV000221 <> ProductAttribute.AttributeReqd Then
                            Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221(iLoop).TagRequired_DEV000221 = ProductAttribute.AttributeReqd
                            bTagsUpdated = True
                        End If
                    End If
                Next
                If Not bTagExists Then
                    Select Case ProductAttribute.AttributeName
                        Case "product_id", "sku", "name", "short_description", "description", "in_depth", "set", "type", "categories", "websites", "price", _
                            "cost", "news_from_date", "news_to_date", "special_from_date", "special_to_date", "special_price", "visibility", "status", _
                            "manufacturer", "type_id", "old_id", "category_ids", "created_at", "updated_at", "tier_price", "sku type" ' TJS 09/12/13
                            ' ignore as these are either included in the MagentoDetails table or not handled

                        Case Else
                            rowMagentoTagDetails = Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221.NewInventoryMagentoTagDetails_DEV000221Row
                            rowMagentoTagDetails.ItemCode_DEV000221 = "Dummy"
                            rowMagentoTagDetails.InstanceID_DEV000221 = Me.cbeSiteOrAccount.EditValue.ToString
                            rowMagentoTagDetails.TagName_DEV000221 = ProductAttribute.AttributeName
                            rowMagentoTagDetails.TagDisplayName_DEV000221 = ProductAttribute.AttributeName.Replace("_", " ")
                            rowMagentoTagDetails.LineNumber_DEV000221 = 1
                            rowMagentoTagDetails.TagRequired_DEV000221 = ProductAttribute.AttributeReqd
                            rowMagentoTagDetails.AttributeID_DEV000221 = ProductAttribute.AttributeID ' TJS 13/11/13
                            rowMagentoTagDetails.AttributeHasSelectValues_DEV000221 = False ' TJS 13/11/13
                            Select Case ProductAttribute.AttributeType.ToLower
                                Case "text"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                                Case "textarea"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Memo"
                                Case "date"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Date"
                                Case "price"
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Numeric"
                                    ' start of code added TJS 13/11/13
                                Case "select"
                                    If ProductAttribute.AttributeName.ToLower = "color" Or ProductAttribute.AttributeName.ToLower = "status" Then
                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Integer"
                                    Else
                                        rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                                    End If
                                    rowMagentoTagDetails.AttributeHasSelectValues_DEV000221 = True
                                    ' end of code added TJS 13/11/13
                                Case Else
                                    rowMagentoTagDetails.TagDataType_DEV000221 = "Text"
                            End Select
                            Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221.AddInventoryMagentoTagDetails_DEV000221Row(rowMagentoTagDetails)
                            bTagsUpdated = True

                    End Select
                End If
            Next
            Me.pnlGetAttributeProgress.Visible = False
            Me.lblMagentoAttributes.Enabled = True
            Me.GridControlProperties.Enabled = True

            ' start of code added TJS 13/11/13
            Select Case Me.cbePublishingTarget.EditValue.ToString
                Case "Magento"
                    Me.lblAttributeAutoFill.Text = "If left blank, the following Magento Attributes will be "
            End Select
            Me.lblAttributeAutoFill.Text = Me.lblAttributeAutoFill.Text & vbCrLf & "automatically populated with their CB Attribute Values :-" & vbCrLf
            bAddToList2 = False
            Me.lblAutoFillList1.Text = ""
            Me.lblAutoFillList2.Text = ""
            bWeightAddedToList = False ' TJS 04/01/14
            For iLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221.Count - 1
                If Not Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).IsSourceAttributeID_DEV000221Null AndAlso _
                    Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeID_DEV000221 > 0 Then
                    ' start of code added TJS 04/01/14
                    If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeName_DEV000221 = "weight" Then
                        If bAddToList2 Then
                            Me.lblAutoFillList2.Text = Me.lblAutoFillList2.Text & "Weight" & vbCrLf
                        Else
                            Me.lblAutoFillList1.Text = Me.lblAutoFillList1.Text & "Weight" & vbCrLf
                        End If
                        bAddToList2 = Not bAddToList2
                    Else
                        If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeName_DEV000221 > "weight" And Not bWeightAddedToList Then
                            If bAddToList2 Then
                                Me.lblAutoFillList2.Text = Me.lblAutoFillList2.Text & "Weight" & vbCrLf
                            Else
                                Me.lblAutoFillList1.Text = Me.lblAutoFillList1.Text & "Weight" & vbCrLf
                            End If
                            bAddToList2 = Not bAddToList2
                            bWeightAddedToList = True
                        End If
                        ' end of code added TJS 04/01/14
                        For iAttributeLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221.Count - 1 ' TJS 15/11/13
                            If Me.BulkPublisingWizardSectionContainerGateway.InventoryMagentoTagDetails_DEV000221(iAttributeLoop).AttributeID_DEV000221 = _
                                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeID_DEV000221 Then ' TJS 15/11/13
                                If bAddToList2 Then
                                    Me.lblAutoFillList2.Text = Me.lblAutoFillList2.Text & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode & vbCrLf
                                Else
                                    Me.lblAutoFillList1.Text = Me.lblAutoFillList1.Text & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode & vbCrLf
                                End If
                                bAddToList2 = Not bAddToList2
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next
            ' end of code added TJS 13/11/13

            Me.WizardControlBulkPublish.EnableNextButton = True
        End If
        RemoveHandler m_BackgroundWorker.DoWork, AddressOf GetAttributesForSet
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetAttributesForSetCompleted

    End Sub
#End Region

#Region " SetCategoryMappingLabelsAndSource "
    Private Sub SetCategoryMappingLabelsAndSource(ByVal TargetName As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.lblSelectCatgoryMapping.Text = "As " & TargetName & " already contains a one or more Categories, we cannot publish the Categories from "
        Me.lblSelectCatgoryMapping.Text += IS_PRODUCT_NAME & " to " & TargetName & vbCrLf & "Please select the appropriate " & IS_PRODUCT_NAME
        Me.lblSelectCatgoryMapping.Text += " Category for each " & TargetName & " Category so that this Wizard can use this mapping" & vbCrLf
        Me.lblSelectCatgoryMapping.Text += "for the Inventory Items to be published."
        Me.colTargetCategoryName.Caption = TargetName & " Category"
        Me.lblMappingNotes.Text = "NOTE  If a " & TargetName & " Category does not have a mapped " & vbCrLf & "CB Category, then any published products assigned "
        Me.lblMappingNotes.Text += vbCrLf & "to that CB Category will be published, but they will" & vbCrLf & "NOT be assigned to any " & TargetName & " Category."
        Me.lblMappingNotes.Text += vbCrLf & vbCrLf & "Categories will have to be manually assigned once " & vbCrLf & "the publishing process is complete."
        Me.TreeListCategoryMapping.DataSource = m_CategoryTable
        Me.TreeListCategoryMapping.ExpandAll()
        Me.TreeListCategoryMapping.Enabled = True
        Me.rbeISCategoryCode.AdditionalFilter = "LanguageCode = '" & Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing) & "'"

    End Sub
#End Region

#Region " GetAttributesToPublish "
    Private Sub GetAttributesToPublish(ByVal MagentoInstanceID As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem
        Dim strSQL As String, strTemp As String, iLoop As Integer

        m_BulkPublishWizardsectionContainerFacade.LoadDataSet(New String()() {New String() {Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221.TableName, _
            "ReadSystemAttributeMagentoMappedView_DEV000221", AT_IS_ACTIVE, "1", AT_INSTANCE_ID, MagentoInstanceID, AT_LANGUAGE_CODE, Me.GetField("CompanyLanguage", "SystemCompanyInformation", Nothing)}}, _
            Interprise.Framework.Base.Shared.ClearType.Specific)

        For iLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221.Count - 1
            Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).Publish = True
            If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).IsInstanceID_DEV000221Null Then
                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).InstanceID_DEV000221 = MagentoInstanceID
                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode
            End If
            If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).IsSourceAttributeID_DEV000221Null Then
                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceDisplayName = Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeDescription
                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeName_DEV000221 = SetMagentoAttributeName(Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode)
            End If
            If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "M" Or _
                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "." Then
                strSQL = "SELECT ISNULL(COUNT(*), 0) FROM InventoryMatrixItem WHERE AttributeCode1 = '" & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode
                strSQL = strSQL & "' OR AttributeCode2 = '" & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode
                strSQL = strSQL & "' OR AttributeCode3 = '" & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode
                strSQL = strSQL & "' OR AttributeCode4 = '" & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode
                strSQL = strSQL & "' OR AttributeCode5 = '" & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode
                strSQL = strSQL & "' OR AttributeCode6 = '" & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode & "'"
                strTemp = m_BulkPublishWizardsectionContainerFacade.GetField(strSQL, CommandType.Text, Nothing)
                If Not String.IsNullOrEmpty(strTemp) AndAlso CInt(strTemp) > 0 Then
                    Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).UsedInMatrixGroups = True
                    If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "." Then
                        Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "M"
                    End If
                Else
                    Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).UsedInMatrixGroups = False
                End If
            Else
                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).UsedInMatrixGroups = False
            End If
        Next

        m_BulkPublishWizardsectionContainerFacade.LoadDataSet(New String()() {New String() {Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221.TableName, _
            "ReadLerrynImportExportMagentoAttributes_DEV000221", AT_INSTANCE_ID, MagentoInstanceID}}, Interprise.Framework.Base.Shared.ClearType.Specific)

        Coll = Me.rbeSourceAttribute.Items
        Coll.BeginUpdate()
        Coll.Clear()
        For iLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221.Count - 1
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeName_DEV000221
            CollItem.Value = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeID_DEV000221
            Coll.Add(CollItem)
        Next
        Coll.EndUpdate()
        Me.rbeSourceAttribute.Sorted = True

    End Sub
#End Region

#Region " SetMagentoAttributeName "
    Private Function SetMagentoAttributeName(ByVal CBAttributeName As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Create a Magento Attribute name by removing any restricted characters
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strTemp As String, iLoop As Integer, iAscValue As Integer

        strTemp = ""
        CBAttributeName = CBAttributeName.ToLower
        For iLoop = 1 To CBAttributeName.Length
            iAscValue = Asc(Mid(CBAttributeName, iLoop, 1))
            If iAscValue >= Asc("0") And iAscValue <= Asc("9") Then
                strTemp = strTemp & Mid(CBAttributeName, iLoop, 1)
            ElseIf iAscValue >= Asc("a") And iAscValue <= Asc("z") Then
                strTemp = strTemp & Mid(CBAttributeName, iLoop, 1)
            Else
                strTemp = strTemp & "_"
            End If
        Next
        Return strTemp

    End Function

#End Region

#Region " PublishItems "
    Private Sub PublishItems(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 15/11/13 | TJS             | 2013.3.09 | Modified to pass options vis MagentoPublishingOptions including an option to pick up Web descriptions and weight
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to module variables instead of reading controls directly to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        m_ItemsToPublish = DirectCast(Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.DataSource, System.Data.DataView) ' TJS 09/12/13

        Select Case m_PublishingTarget ' TJS 09/12/13
            Case "Magento"
                If Me.chkPublishedOtherInstances.Checked Then
                    m_MagentoImportFacade.PublishBulkInventoryItems(m_SiteOrAccount, m_ItemsToPublish, worker.CancellationPending, _
                        PRODUCT_CODE, PRODUCT_NAME, Me.cbePublishedOtherInstances.EditValue.ToString, m_NoOfItemsProcessed, m_NoOfItemsPublished, m_NoOfItemsSkipped) ' TJS 09/12/13
                Else
                    m_MagentoImportFacade.PublishBulkInventoryItems(m_SiteOrAccount, m_ItemsToPublish, worker.CancellationPending, _
                        PRODUCT_CODE, PRODUCT_NAME, m_CategoryTable, m_PublishingOptions, m_NoOfItemsProcessed, m_NoOfItemsPublished, m_NoOfItemsSkipped) ' TJS 15/11/13 TJS 09/12/13
                End If

        End Select

    End Sub

    Private Sub PublishItemsCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 04/01/14 | TJS             | 2013.4.03 | Modified message to mention service actions
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strFinishMessage As String, strLastError As String = "", strDestination As String ' TJS 04/01/14

        strDestination = "" ' TJS 04/01/14
        Select Case Me.cbePublishingTarget.EditValue.ToString
            Case "Magento"
                strLastError = m_MagentoImportFacade.LastError
                strDestination = "Magento" ' TJS 04/01/14
        End Select

        If m_NoOfItemsPublished > 0 Then
            If strLastError <> "" Then
                strFinishMessage = "The eShopCONNECTED Bulk Inventory Publishing Wizard encountered an error which prevented it completing the publishing." & vbCrLf & vbCrLf & _
                    m_NoOfItemsPublished & " products were published and " & m_NoOfItemsSkipped & " were skipped." & vbCrLf & vbCrLf
            Else
                strFinishMessage = "You have successfully completed the eShopCONNECTED Bulk Inventory Publishing Wizard." & vbCrLf & vbCrLf & _
                    m_NoOfItemsPublished & " products were published and " & m_NoOfItemsSkipped & " were skipped." & vbCrLf & vbCrLf
            End If
            strFinishMessage = strFinishMessage & "The published products will be updated in " & strDestination & " by the eShopCONNECTED Service shortly" & vbCrLf & vbCrLf ' TJS 04/01/14
        Else
            If strLastError <> "" Then
                strFinishMessage = "The eShopCONNECTED Bulk Inventory Publishing Wizard encountered an error which prevented it completing the import." & vbCrLf & vbCrLf & _
                    "No products were published and " & m_NoOfItemsSkipped & " were skipped." & vbCrLf & vbCrLf
            Else
                strFinishMessage = "You have successfully completed the eShopCONNECTED Bulk Inventory Publishing Wizard, but " & _
                    "no products were published and " & m_NoOfItemsSkipped & " were skipped." & vbCrLf & vbCrLf
            End If
        End If
        strFinishMessage = strFinishMessage & "Please click Finish to exit the Wizard."
        Me.WizardControlBulkPublish.FinishMessage = strFinishMessage
        Me.pnlPublishingProductsProgress.Visible = False
        Me.WizardControlBulkPublish.SelectedTabPage = Me.TabPageComplete
        Me.WizardControlBulkPublish.EnableNextButton = True
        Me.WizardControlBulkPublish.EnableCancelButton = False
        Me.WizardControlBulkPublish.EnableBackButton = False
        RemoveHandler m_BackgroundWorker.DoWork, AddressOf PublishItems
        RemoveHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf PublishItemsCompleted

    End Sub

#End Region
#End Region

#Region " Events "
#Region " WizardPageChanged "
    Private Sub WizardPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles WizardControlBulkPublish.SelectedPageChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for creating Magento Categories and 
        '                                        | Attribute Sets in empty Magento sites
        ' 15/11/13 | TJS             | 2013.3.08 | Modified to prevent cross thread errors after correcting attribute publishing errors
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to populate module variables to prevent cross-thread errors
        ' 04/01/14 | TJS             | 2013.4.03 | Modified to detect and report errors when saving attribute mapping
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim rowCategoryMapping As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportCategoryMappingView_DEV000221Row ' TJS 13/11/13
        Dim Coll As DevExpress.XtraEditors.Controls.ComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ComboBoxItem
        Dim strInstances()() As String, strValueCount As String, strTemp As String, iInstLoop As Integer
        Dim iRowLoop As Integer, iColumnLoop As Integer ' TJS 12/12/13
        Dim bItemsToPublish As Boolean, bAtLeast1CategoryActive As Boolean, bValidationErrors As Boolean
        Dim bCopySettings As Boolean, bAttributesToPublish As Boolean, bValidationError As Boolean ' TJS 13/11/13

        ' have we triggered this event by changing the focused page ?
        If Not Me.bIgnoreWizardPageChangeEvent Then
            ' no, which page is being displayed ?
            Cursor = Cursors.WaitCursor
            If ReferenceEquals(e.Page, Me.TabPageWelcome) Then
                ' Welcome Code page, must have clicked Back

                ' start of code added TJS 13/11/13
            ElseIf ReferenceEquals(e.Page, Me.TabPageCreateCategories) Then
                ' Create Categories page, was last page the Welcome page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageWelcome) Then
                    ' yes, must have clicked Next
                    If Me.cbePublishingTarget.EditValue.ToString = "Magento" Then
                        ' are there any Categories in Magento ?
                        If m_CategoryTable.Rows.Count > 0 Then
                            ' yes, display Map Categories page 
                            bIgnoreWizardPageChangeEvent = True
                            Me.WizardControlBulkPublish.FocusPage = Me.TabPageMapCategories
                            bIgnoreWizardPageChangeEvent = False
                            SetCategoryMappingLabelsAndSource(cbePublishingTarget.EditValue.ToString)
                        Else
                            ' no Categories or Attribute Sets exist
                            Me.WizardControlBulkPublish.buttonNext.Enabled = False

                        End If
                    Else
                        ' skip creating Categories and Attribute Sets as not publishing to Magento
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlBulkPublish.FocusPage = Me.TabPageSelectItems
                        bIgnoreWizardPageChangeEvent = False
                    End If

                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageMapCategories) Then
                ' Map Categories page, was last page the Create Categories page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageCreateCategories) Then
                    ' yes, must have clicked Next 
                    ' were categories created ?
                    If bCategoriesCreated Then
                        ' yes, no need to map them - are there any Attribute Sets
                        If m_MagentoImportFacade.MagentoAttributeSets.Length > 0 Then
                            ' yes, display Attributes page 
                            bIgnoreWizardPageChangeEvent = True
                            Me.WizardControlBulkPublish.FocusPage = Me.TabPageAttributes
                            bIgnoreWizardPageChangeEvent = False
                        Else
                            ' no, display Attribute Sets page
                            bIgnoreWizardPageChangeEvent = True
                            'Me.WizardControlBulkPublish.FocusPage = Me.TabPageAttributeSets
                            ' for now go straight to Attributes page
                            Me.WizardControlBulkPublish.FocusPage = Me.TabPageAttributes
                            bIgnoreWizardPageChangeEvent = False
                        End If
                    Else
                        ' no, need to map categories
                        SetCategoryMappingLabelsAndSource(cbePublishingTarget.EditValue.ToString)
                    End If
                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageCreateAndMapAttributes) Then
                ' Create/Map Magento Attributes page, was last page the Create Categories or Map Categories page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageCreateCategories) Or ReferenceEquals(e.PrevPage, Me.TabPageMapCategories) Then
                    ' yes, must have clicked Next
                    ' start of code added TJS 13/11/13
                    Me.LoadDataSet(New String()() {New String() {Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.TableName, _
                        "ReadLerrynImportExportCategoryMapping_DEV000221", AT_SOURCE_CODE, "MagentoOrder", AT_INSTANCE_ID, Me.cbeSiteOrAccount.EditValue.ToString}}, _
                        Interprise.Framework.Base.Shared.ClearType.Specific)

                    For iloop = 0 To m_CategoryTable.Rows.Count - 1
                        If m_CategoryTable.Rows(iloop).Item("ISCategoryCode").ToString <> "" Then
                            rowCategoryMapping = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.FindBySourceCode_DEV000221InstanceAccountID_DEV000221SourceCategoryID_DEV000221SourceParentID_DEV000221("MagentoOrder", Me.cbeSiteOrAccount.EditValue.ToString, m_CategoryTable.Rows(iloop).Item("SourceCategoryID").ToString, m_CategoryTable.Rows(iloop).Item("SourceParentID").ToString)
                            If rowCategoryMapping IsNot Nothing Then
                                If rowCategoryMapping.ISCategoryCode_DEV000221 <> m_CategoryTable.Rows(iloop).Item("ISCategoryCode").ToString Then
                                    rowCategoryMapping.ISCategoryCode_DEV000221 = m_CategoryTable.Rows(iloop).Item("ISCategoryCode").ToString
                                End If
                            Else
                                rowCategoryMapping = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.NewLerrynImportExportCategoryMappingView_DEV000221Row
                                rowCategoryMapping.SourceCode_DEV000221 = "MagentoOrder"
                                rowCategoryMapping.InstanceAccountID_DEV000221 = Me.cbeSiteOrAccount.EditValue.ToString
                                rowCategoryMapping.SourceCategoryID_DEV000221 = m_CategoryTable.Rows(iloop).Item("SourceCategoryID").ToString
                                rowCategoryMapping.SourceParentID_DEV000221 = m_CategoryTable.Rows(iloop).Item("SourceParentID").ToString
                                rowCategoryMapping.ISCategoryCode_DEV000221 = m_CategoryTable.Rows(iloop).Item("ISCategoryCode").ToString
                                Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.AddLerrynImportExportCategoryMappingView_DEV000221Row(rowCategoryMapping)
                            End If
                        End If
                    Next
                    Me.m_BulkPublishWizardsectionContainerFacade.UpdateDataSet(New String()() {New String() {Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportCategoryMappingView_DEV000221.TableName, _
                            "CreateLerrynImportExportCategoryMapping_DEV000221", "UpdateLerrynImportExportCategoryMapping_DEV000221", "DeleteLerrynImportExportCategoryMapping_DEV000221"}, _
                        New String() {Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221.TableName, _
                            "CreateLerrynImportExportMagentoAttributes_DEV000221", "UpdateLerrynImportExportMagentoAttributes_DEV000221", "DeleteLerrynImportExportMagentoAttributes_DEV000221"}}, _
                        Interprise.Framework.Base.Shared.TransactionType.None, "Update Category and Attribute Mapping", False)
                    ' end of code added TJS 13/11/13

                    GetAttributesToPublish(Me.cbeSiteOrAccount.EditValue.ToString)
                    Me.GridControlAttributes.Enabled = True

                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPagePublishingAttributes) Then
                ' Publishing Magento Attributes page, was last page the Create/Map Magento Attributes
                If ReferenceEquals(e.PrevPage, Me.TabPageCreateAndMapAttributes) Then ' TJS 13/11/13
                    ' are there any Attributes to publish ?
                    bAttributesToPublish = False
                    bValidationError = False
                    For iLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221.Count - 1
                        If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).Publish And _
                            (Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).IsSourceAttributeID_DEV000221Null OrElse _
                            Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeID_DEV000221 < 0) Then
                            bAttributesToPublish = True
                            If Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).IsSourceAttributeName_DEV000221Null OrElse _
                                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeName_DEV000221 = "" Then
                                bValidationError = True
                                Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).SetColumnError("SourceAttributeName_DEV000221", "Must not be blank")
                            End If

                            ' start of code added TJS 13/11/13
                        ElseIf Not bAttributesToPublish Then
                            strTemp = "SELECT ISNULL(Count(*), 0) FROM SystemAttributeValueMagentoMappedView_DEV000221 WHERE AttributeCode = '"
                            strTemp = strTemp & Me.BulkPublisingWizardSectionContainerGateway.SystemAttributeMagentoMappedView_DEV000221(iLoop).AttributeCode
                            strTemp = strTemp & "' AND SourceValueID_DEV000221 IS Null AND (InstanceID_DEV000221 is NULL OR InstanceID_DEV000221 <> '"
                            strTemp = strTemp & Me.cbeSiteOrAccount.EditValue.ToString & "')"
                            strValueCount = Me.m_BulkPublishWizardsectionContainerFacade.GetField(strTemp, CommandType.Text, Nothing)
                            If Not String.IsNullOrEmpty(strValueCount) AndAlso CInt(strValueCount) > 0 Then
                                bAttributesToPublish = True
                            End If
                            ' end of code added TJS 13/11/13
                        End If
                    Next
                    If bValidationError Then
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlBulkPublish.FocusPage = Me.TabPageCreateAndMapAttributes
                        bIgnoreWizardPageChangeEvent = False

                    ElseIf bAttributesToPublish Then
                        ' save any updated attribute mapping
                        If Not m_BulkPublishWizardsectionContainerFacade.UpdateDataSet(New String()() {New String() {Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221.TableName, _
                                "CreateLerrynImportExportMagentoAttributes_DEV000221", "UpdateLerrynImportExportMagentoAttributes_DEV000221", "DeleteLerrynImportExportMagentoAttributes_DEV000221"}, _
                            New String() {Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221.TableName, _
                                "CreateLerrynImportExportMagentoAttributeValues_DEV000221", "UpdateLerrynImportExportMagentoAttributeValues_DEV000221", "DeleteLerrynImportExportMagentoAttributeValues_DEV000221"}}, _
                          Interprise.Framework.Base.Shared.TransactionType.None, "Update Magento Attribute mapping", False) Then ' TJS 04/01/14
                            ' start of code added TJS 04/01/14
                            With Me.BulkPublisingWizardSectionContainerGateway
                                For iRowLoop = 0 To .LerrynImportExportMagentoAttributes_DEV000221.Rows.Count - 1
                                    strTemp = ""
                                    For iColumnLoop = 0 To .LerrynImportExportMagentoAttributes_DEV000221.Columns.Count - 1
                                        If .LerrynImportExportMagentoAttributes_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop) <> "" Then
                                            If strTemp <> "" Then
                                                strTemp = strTemp & ", "
                                            End If
                                            strTemp = strTemp & .LerrynImportExportMagentoAttributes_DEV000221.Columns(iColumnLoop).ColumnName & " " & _
                                                .LerrynImportExportMagentoAttributes_DEV000221.Rows(iRowLoop).GetColumnError(iColumnLoop)
                                        End If
                                    Next
                                    ' look for displayed attribute record
                                    If strTemp <> "" Then
                                        For iLoop = 0 To .SystemAttributeMagentoMappedView_DEV000221.Count - 1
                                            If .SystemAttributeMagentoMappedView_DEV000221(iLoop).InstanceID_DEV000221 = .LerrynImportExportMagentoAttributes_DEV000221(iRowLoop).InstanceID_DEV000221 AndAlso _
                                                .SystemAttributeMagentoMappedView_DEV000221(iLoop).SourceAttributeID_DEV000221 = .LerrynImportExportMagentoAttributes_DEV000221(iRowLoop).SourceAttributeID_DEV000221 Then
                                                ' found it, show error message
                                                .SystemAttributeMagentoMappedView_DEV000221(iRowLoop).SetColumnError("AttributeCode", strTemp)
                                            End If
                                        Next
                                    End If
                                Next
                            End With
                            bIgnoreWizardPageChangeEvent = True
                            Me.WizardControlBulkPublish.FocusPage = Me.TabPageCreateAndMapAttributes
                            bIgnoreWizardPageChangeEvent = False
                            ' end of code added TJS 04/01/14
                        End If

                        Me.lblAttributePublishingErrors.Text = "" ' TJS 15/11/13
                        Me.pnlPublishingAttributesProgress.Visible = True
                        Me.lblPublishingAttributeProgress.Text = "Publishing Attributes to " & Me.cbePublishingTarget.EditValue.ToString
                        If m_BackgroundWorker Is Nothing Then
                            m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                            m_BackgroundWorker.WorkerSupportsCancellation = True
                            m_BackgroundWorker.WorkerReportsProgress = False
                        End If
                        AddHandler m_BackgroundWorker.DoWork, AddressOf PublishAttributes
                        AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf PublishAttributesCompleted
                        Me.WizardControlBulkPublish.EnableNextButton = False
                        m_BackgroundWorker.RunWorkerAsync()

                    Else
                        ' don't set Ignore page change event as we want to process setting
                        Me.WizardControlBulkPublish.FocusPage = Me.TabPageSelectItems

                    End If
                    ' end of code added TJS 13/11/13
                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageAttributeSets) Then
                ' Create Magento Attribute Sets page, was last page the Publish Attributes page ?
                If ReferenceEquals(e.PrevPage, Me.TabPagePublishingAttributes) Then
                    ' yes, must have clicked Next
                    ' start of code added TJS 13/11/13
                    ' are there any Attribute Sets ?
                    If m_MagentoImportFacade.MagentoAttributeSets.Length > 0 Then
                        ' yes, ask if user wants to create more
                        strTemp = "You already have " & m_MagentoImportFacade.MagentoAttributeSets.Length & " Attribute Sets in Magento." & vbCrLf & vbCrLf
                        strTemp = strTemp & "Due to limitations in the Magento API, we cannot display" & vbCrLf & "the structure of those Attribute Sets." & vbCrLf & vbCrLf
                        strTemp = strTemp & "Do you want to create any additional Attribute Sets in Magento " & vbCrLf & "using this Wizard ?"
                        If Interprise.Presentation.Base.Message.MessageWindow.Show(strTemp, "Create Attribute Sets", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, _
                            Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button1) = DialogResult.No Then
                            ' skip creating Attribute Sets 
                            bIgnoreWizardPageChangeEvent = True
                            Me.WizardControlBulkPublish.FocusPage = Me.TabPageCreateAndMapAttributes
                            bIgnoreWizardPageChangeEvent = False
                        Else

                        End If
                    End If
                    ' end of code added TJS 13/11/13
                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPagePublishAttributeSets) Then
                ' Publish Magento Attribute Sets page, was last page the Create Magento Attribute Sets page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageAttributeSets) Then
                    ' yes, must have clicked Next

                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageSelectItems) Then
                ' Select Items page, was last page the Publishing Magento Attributes page ?
                If ReferenceEquals(e.PrevPage, Me.TabPagePublishingAttributes) Then
                    ' yes, must have clicked Next
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.OptionsView.ShowGroupPanel = False
                    strInstances = Me.m_BulkPublishWizardsectionContainerFacade.GetRows(New String() {"InstanceID_DEV000221"}, "InventoryMagentoDetails_DEV000221", "Publish_DEV000221 = 1 AND InstanceID_DEV000221 <> '" & Me.cbeSiteOrAccount.EditValue.ToString.Replace("'", "''") & "' GROUP BY InstanceID_DEV000221")
                    If strInstances IsNot Nothing AndAlso strInstances.Length > 0 Then
                        Me.LayoutItemPublishedOtherInstances1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                        Me.LayoutItemPublishedOtherInstances2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always
                        Me.chkPublishedOtherInstances.Visible = True
                        Me.cbePublishedOtherInstances.Visible = True
                        Coll = cbePublishedOtherInstances.Properties.Items
                        Coll.BeginUpdate()
                        Coll.Clear()
                        For iInstLoop = 0 To strInstances.Length - 1
                            If Not String.IsNullOrEmpty(strInstances(iInstLoop)(0)) Then
                                CollItem = New DevExpress.XtraEditors.Controls.ComboBoxItem(strInstances(iInstLoop)(0))
                                Coll.Add(CollItem)
                            End If
                        Next
                        Coll.EndUpdate()
                    Else
                        Me.LayoutItemPublishedOtherInstances1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.LayoutItemPublishedOtherInstances2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never
                        Me.chkPublishedOtherInstances.Visible = False
                        Me.cbePublishedOtherInstances.Visible = False
                    End If


                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageCategories) Then
                ' Categories page, was last page the Select Items page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageSelectItems) Then
                    ' yes, must have clicked Next

                    bItemsToPublish = False
                    bCopySettings = False
                    If Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.RowCount > 0 Then
                        For iLoop = 0 To Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.RowCount - 1
                            If CBool(Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.GetRowCellValue(iLoop, "Select")) Then
                                bItemsToPublish = True
                                Exit For
                            End If
                        Next
                    End If
                    If Not bItemsToPublish Then
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlBulkPublish.FocusPage = Me.TabPageSelectItems
                        bIgnoreWizardPageChangeEvent = False
                        Interprise.Presentation.Base.Message.MessageWindow.Show("You must select at least 1 Item to publish")
                    End If

                    If Me.chkPublishedOtherInstances.Checked Then
                        If Interprise.Presentation.Base.Message.MessageWindow.Show("As you have selected Items which are already published to another Magento" & vbCrLf & "Instance, do you want to copy those settings for the target Instance ?", _
                            "Use existing Settings", Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, _
                            Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button1) = DialogResult.Yes Then
                            bCopySettings = True
                        End If
                    End If
                    If bCopySettings Then
                        ' copying settings so go straight to publish
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlBulkPublish.FocusPage = Me.TabPagePublishing
                        bIgnoreWizardPageChangeEvent = False

                        Me.pnlPublishingProductsProgress.Visible = True
                        Me.lblPublishingProductProgress.Text = "Publishing Products to " & Me.cbePublishingTarget.EditValue.ToString ' TJS 15/11/13
                        If m_BackgroundWorker Is Nothing Then ' TJS 13/11/13
                            m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                            m_BackgroundWorker.WorkerSupportsCancellation = True
                            m_BackgroundWorker.WorkerReportsProgress = False
                        End If
                        m_ItemsToPublish = DirectCast(Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.DataSource, System.Data.DataView) ' TJS 09/12/13

                        AddHandler m_BackgroundWorker.DoWork, AddressOf PublishItems
                        AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf PublishItemsCompleted
                        Me.WizardControlBulkPublish.EnableNextButton = False
                        m_BackgroundWorker.RunWorkerAsync()

                    End If

                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageAttributes) Then
                ' Attributes page, was last page Categories page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageCategories) Then
                    ' yes, must have clicked Next
                    If Me.chkUseMappedCBCategories.Checked Then
                        bAtLeast1CategoryActive = True
                    Else
                        bAtLeast1CategoryActive = False
                        For iloop = 1 To m_CategoryTable.Rows.Count - 1
                            If CBool(m_CategoryTable.Rows(iloop).Item("Active")) Then
                                bAtLeast1CategoryActive = True
                                Exit For
                            End If
                        Next
                    End If
                    If Not bAtLeast1CategoryActive Or Me.ImageComboBoxEditAttributeSet.SelectedIndex < 0 Then
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlBulkPublish.FocusPage = Me.TabPageCategories
                        bIgnoreWizardPageChangeEvent = False
                        If Not bAtLeast1CategoryActive Then
                            Interprise.Presentation.Base.Message.MessageWindow.Show("You must select at least 1 Category as Active")
                        End If
                        If Me.ImageComboBoxEditAttributeSet.SelectedIndex < 0 Then
                            Me.ImageComboBoxEditAttributeSet.ErrorText = "You must select the Attribute Set to use"
                        End If

                    Else
                        Me.pnlGetAttributeProgress.Visible = True
                        Me.lblMagentoAttributes.Enabled = False
                        Me.GridControlProperties.Enabled = False

                        GetAttributesForSetID = CInt(ImageComboBoxEditAttributeSet.EditValue)

                        m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                        m_BackgroundWorker.WorkerSupportsCancellation = True
                        m_BackgroundWorker.WorkerReportsProgress = False
                        AddHandler m_BackgroundWorker.DoWork, AddressOf GetAttributesForSet
                        AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetAttributesForSetCompleted
                        Me.WizardControlBulkPublish.EnableNextButton = False
                        m_BackgroundWorker.RunWorkerAsync()

                    End If

                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageOptions) Then
                ' Options page, was last page the Attributes page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageAttributes) Then
                    ' yes, must have clicked Next


                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPagePublishing) Then
                ' Publishing page, was last page the Options page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageOptions) Then

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

                    If Me.cbeTargetPriceSource.EditValue Is Nothing OrElse Me.cbeTargetPriceSource.EditValue.ToString = "" Then
                        bValidationErrors = True
                        Me.cbeTargetPriceSource.ErrorText = "Must not be blank"
                    End If
                    If Me.cbeMagentoSpecialPriceSource.Enabled Then ' TJS 13/11/13
                        If Me.cbeMagentoSpecialPriceSource.EditValue Is Nothing OrElse Me.cbeMagentoSpecialPriceSource.EditValue.ToString = "" Then
                            bValidationErrors = True
                            Me.cbeMagentoSpecialPriceSource.ErrorText = "Must not be blank"
                        Else
                            Me.cbeMagentoSpecialPriceSource.ErrorText = ""
                            If Me.cbeMagentoSpecialPriceSource.EditValue.ToString = "W" Or Me.cbeMagentoSpecialPriceSource.EditValue.ToString = "R" AndAlso _
                                Me.DateEditSpecialFrom.EditValue Is Nothing Then
                                bValidationErrors = True
                                Me.DateEditSpecialFrom.ErrorText = "Must not be blank"
                            End If
                        End If
                    End If

                    ' start of code added TJS 15/11/13
                    If Me.cbeMagentoShortDescSource.EditValue Is Nothing OrElse Me.cbeMagentoShortDescSource.EditValue.ToString = "" Then
                        bValidationErrors = True
                        Me.cbeMagentoShortDescSource.ErrorText = "Must not be blank"
                    End If
                    If Me.cbeMagentoDescriptionSource.EditValue Is Nothing OrElse Me.cbeMagentoDescriptionSource.EditValue.ToString = "" Then
                        bValidationErrors = True
                        Me.cbeMagentoDescriptionSource.ErrorText = "Must not be blank"
                    End If
                    If Me.cbeMagentoWeightUnits.EditValue Is Nothing OrElse Me.cbeMagentoWeightUnits.EditValue.ToString = "" Then
                        bValidationErrors = True
                        Me.cbeMagentoWeightUnits.ErrorText = "Must not be blank"
                    End If
                    ' end of code added TJS 15/11/13

                    If bValidationErrors Then
                        bIgnoreWizardPageChangeEvent = True
                        Me.WizardControlBulkPublish.FocusPage = Me.TabPageOptions
                        bIgnoreWizardPageChangeEvent = False
                    Else
                        ' start of code added TJS 15/11/13 and moved TJS 09/12/13
                        m_PublishingOptions = New Lerryn.Facade.ImportExport.MagentoImportFacade.MagentoPublishingOptions
                        m_PublishingOptions.UseMappedCBCategories = Me.chkUseMappedCBCategories.Checked
                        m_PublishingOptions.AttributeSetID = GetAttributesForSetID
                        If Me.cbeTargetPriceSource.EditValue IsNot Nothing Then
                            m_PublishingOptions.MagentoPriceSource = Me.cbeTargetPriceSource.EditValue.ToString
                        Else
                            m_PublishingOptions.MagentoPriceSource = ""
                        End If
                        If Me.cbeMagentoSpecialPriceSource.EditValue IsNot Nothing Then
                            m_PublishingOptions.MagentoSpecialPriceSource = Me.cbeMagentoSpecialPriceSource.EditValue.ToString
                        Else
                            m_PublishingOptions.MagentoSpecialPriceSource = ""
                        End If
                        If Me.DateEditSpecialFrom.EditValue IsNot Nothing Then
                            m_PublishingOptions.MagentoSpecialPriceFrom = CDate(Me.DateEditSpecialFrom.EditValue)
                        End If
                        If Me.DateEditSpecialTo.EditValue IsNot Nothing Then
                            m_PublishingOptions.MagentoSpecialPriceTo = CDate(Me.DateEditSpecialTo.EditValue)
                        End If
                        If Me.RadioGroupQtyPublishing.EditValue IsNot Nothing Then
                            m_PublishingOptions.MagentoQtyPublishingOption = Me.RadioGroupQtyPublishing.EditValue.ToString
                        Else
                            m_PublishingOptions.MagentoQtyPublishingOption = ""
                        End If
                        If Me.TextEditQtyPublishingValue.EditValue IsNot Nothing Then
                            m_PublishingOptions.MagentoQtyPublishingValue = CDec(Me.TextEditQtyPublishingValue.EditValue)
                        Else
                            m_PublishingOptions.MagentoQtyPublishingValue = 0
                        End If
                        ' end of code added TJS 15/11/13 and moved TJS 09/12/13
                        ' start of code added TJS 09/12/13
                        m_PublishingOptions.MagentoDescriptionSource = Me.cbeMagentoDescriptionSource.EditValue.ToString
                        m_PublishingOptions.MagentoShortDescSource = Me.cbeMagentoShortDescSource.EditValue.ToString
                        m_PublishingOptions.MagentoWeightUnits = Me.cbeMagentoWeightUnits.EditValue.ToString
                        ' end of code added TJS 09/12/13

                        Me.pnlPublishingProductsProgress.Visible = True
                        Me.lblPublishingProductProgress.Text = "Publishing Products to " & Me.cbePublishingTarget.EditValue.ToString ' TJS 15/11/13
                        AddHandler m_BackgroundWorker.DoWork, AddressOf PublishItems
                        AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf PublishItemsCompleted
                        Me.WizardControlBulkPublish.EnableNextButton = False
                        m_BackgroundWorker.RunWorkerAsync()

                    End If
                End If


            ElseIf ReferenceEquals(e.Page, Me.TabPageComplete) Then
                ' Finish page, was last page the Publishing progress page ?
                If ReferenceEquals(e.PrevPage, Me.TabPagePublishing) Then
                    ' yes, must have clicked Next 

                Else
                    ' no, must have clicked Back

                End If
            End If
            Cursor = Cursors.Default
        End If

    End Sub
#End Region

#Region " TargetSelected "
    Private Sub TargetSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbePublishingTarget.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Added initialisation of various captions and labels etc
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to store Publishing Target in module variable to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLConfig As XDocument, XMLTemp As XDocument
        Dim XMLNodeList As System.Collections.Generic.IEnumerable(Of XElement), XMLNode As XElement
        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem

        m_PublishingTarget = Me.cbePublishingTarget.EditValue.ToString ' TJS 09/12/13
        Select Case Me.cbePublishingTarget.EditValue.ToString
            Case "Magento"
                m_MagentoImportFacade = New Lerryn.Facade.ImportExport.MagentoImportFacade(Me.BulkPublisingWizardSectionContainerGateway, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

                Me.LoadDataSet(New String()() {New String() {Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221.TableName, _
                    "ReadLerrynImportExportConfig_DEV000221", AT_SOURCE_CODE, "MagentoOrder"}}, Interprise.Framework.Base.Shared.ClearType.Specific)
                If Me.m_BulkPublishWizardsectionContainerFacade.IsActivated Then
                    XMLConfig = XDocument.Parse(Trim(Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportConfig_DEV000221(0).ConfigSettings_DEV000221))
                    Me.cbeSiteOrAccount.Visible = True
                    Me.lblSiteOrAccount.Visible = True
                    Me.lblMultipleSiteOrAccount.Visible = True
                    Me.WizardControlBulkPublish.buttonNext.Enabled = False
                    XMLNodeList = XMLConfig.XPathSelectElements(SOURCE_CONFIG_XML_MAGENTO_LIST)
                    Coll = Me.cbeSiteOrAccount.Properties.Items
                    Coll.BeginUpdate()
                    Coll.Clear()
                    For Each XMLNode In XMLNodeList
                        XMLTemp = XDocument.Parse(XMLNode.ToString)
                        If Me.m_BulkPublishWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_ACCOUNT_DISABLED).ToUpper <> "YES" And _
                            Me.m_BulkPublishWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID) <> "" Then
                            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem(Me.m_BulkPublishWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID), Me.m_BulkPublishWizardsectionContainerFacade.GetXMLElementText(XMLTemp, SOURCE_CONFIG_XML_MAGENTO_INSTANCE_ID))
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
                        Me.WizardControlBulkPublish.buttonNext.Enabled = False ' TJS 13/11/13
                        Interprise.Presentation.Base.Message.MessageWindow.Show("Your Magento Connector sources are all disabled.  Please check and set the AccountDisabled Config Setting to No on at least 1 Magento Config Group.")

                    ElseIf Coll.Count = 1 Then
                        ' only have 1 active source so select it
                        Me.cbeSiteOrAccount.SelectedIndex = 0
                        Me.cbeSiteOrAccount.Visible = False
                        Me.lblSiteOrAccount.Visible = False
                        Me.lblMultipleSiteOrAccount.Visible = False
                        ' Next button enabled in SiteOrAccountSelected
                        'Me.WizardControlBulkPublish.buttonNext.Enabled = True TJS 13/11/13
                    End If
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.pageInitial.Text = "Items to Publish"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.EntityName = Interprise.Framework.Base.Shared.Entity.Inventory
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.DisplayField = "ItemName"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.DefaultSort = "ItemName"

                    ' start of code added TJS 13/11/13
                    Me.colAttributeCode.Caption = "CB Attribute Code"
                    Me.colAttributeDescription.Caption = "CB Attribute Description"
                    Me.colSourceAttributeID_DEV000221.Caption = "Magento Attribute Name"
                    Me.colSourceAttributeName_DEV000221.Caption = "Magento Attribute Name"
                    Me.colPropertiesName_DEV000221.Caption = "Magento Attribute Name"

                    Me.lblAttributeInstructions.Text = "eShopCONNECTED will create the Attributes shown" & vbCrLf & "in Magento." & vbCrLf & vbCrLf
                    Me.lblAttributeInstructions.Text += "Please clear the Publish checkbox on any Attributes" & vbCrLf
                    Me.lblAttributeInstructions.Text += "that you don't to be created in Magento e.g. becasue " & vbCrLf & "they are no longer required."
                    Me.lblAttributeInstructions.Text += vbCrLf & vbCrLf & "If an Attribute has already been created in Magento" & vbCrLf
                    Me.lblAttributeInstructions.Text += "for one or more of the CB Attribute Codes shown, " & vbCrLf & "please select the relevant Magento Attribute Name "
                    Me.lblAttributeInstructions.Text += vbCrLf & "from the list box and eShopCONNECTED will use that " & vbCrLf & "instead of creating a new Magento Attribute "
                    ' end of code added TJS 13/11/13

                Else
                    Me.cbeSiteOrAccount.Visible = False
                    Me.lblSiteOrAccount.Visible = False
                    Me.lblMultipleSiteOrAccount.Visible = False
                    Me.WizardControlBulkPublish.buttonNext.Enabled = False
                    Interprise.Presentation.Base.Message.MessageWindow.Show("Target not activated")

                End If

            Case Else
                Me.cbeSiteOrAccount.Visible = False
                Me.lblSiteOrAccount.Visible = False
                Me.lblMultipleSiteOrAccount.Visible = False
                Me.WizardControlBulkPublish.buttonNext.Enabled = False
                Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = ""
                Interprise.Presentation.Base.Message.MessageWindow.Show("Unknown Target")

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
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to check for existing Magento Categories and Attribute Sets
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to store Site or Account in module variable to prevent cross-thread errors
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim colNewColumn As DataColumn

        m_SiteOrAccount = Me.cbeSiteOrAccount.EditValue.ToString ' TJS 09/12/13
        If Me.cbeSiteOrAccount.EditValue.ToString <> "" Then
            ' start of code added TJS 13/11/13
            If Me.cbePublishingTarget.EditValue.ToString = "Magento" Then
                Me.WizardControlBulkPublish.buttonNext.Enabled = False
                Me.pnlGetCategoryProgress.Visible = True
                Me.lblSelectCategories.Enabled = False
                Me.lblSelectAttributeSet.Enabled = False
                Me.ImageComboBoxEditAttributeSet.Enabled = False

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

                m_BackgroundWorker = New System.ComponentModel.BackgroundWorker
                m_BackgroundWorker.WorkerSupportsCancellation = True
                m_BackgroundWorker.WorkerReportsProgress = False
                AddHandler m_BackgroundWorker.DoWork, AddressOf GetSourceCategoriesForPublishing
                AddHandler m_BackgroundWorker.RunWorkerCompleted, AddressOf GetSourceCategoriesCompleted
                Me.WizardControlBulkPublish.EnableNextButton = False
                m_BackgroundWorker.RunWorkerAsync()
                ' end of code added TJS 13/11/13
            Else
                Me.WizardControlBulkPublish.buttonNext.Enabled = True
            End If
        Else
            Me.WizardControlBulkPublish.buttonNext.Enabled = False
        End If

    End Sub
#End Region

#Region " InventoryTypeFilterSelected "
    Private Sub InventoryTypeFilterSelected(sender As Object, e As System.EventArgs) Handles cbeInventoryTypeFilter.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Added Matrix Items option
        ' 04/01/14 | TJS             | 2013.4.03 | Modified to cater for ignore discontinued Item option
        '                                        | and for Apply Filter form
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim bShowItemsPublishedOnOtherInstances As Boolean

        Select Case Me.cbePublishingTarget.EditValue.ToString
            Case "Magento"
                bShowItemsPublishedOnOtherInstances = False
                If Me.chkPublishedOtherInstances.Visible = True AndAlso Me.chkPublishedOtherInstances.Checked Then
                    If Me.cbePublishedOtherInstances.EditValue IsNot Nothing Then
                        bShowItemsPublishedOnOtherInstances = True
                    Else
                        Me.cbePublishedOtherInstances.ErrorText = "Must not be blank"
                        Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter = ""
                        Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = ""
                        Return
                    End If
                End If
                If bShowItemsPublishedOnOtherInstances Then
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter = " and LanguageCode = '" & Me.m_BulkPublishWizardsectionContainerFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE) & _
                        "' and ItemCode IN (SELECT ItemCode_DEV000221 FROM InventoryMagentoDetails_DEV000221 WHERE Publish_DEV000221 = 1 AND InstanceID_DEV000221 = '" & Me.cbePublishedOtherInstances.EditValue.ToString.Replace("'", "''") & "')"
                Else
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter = " and LanguageCode = '" & Me.m_BulkPublishWizardsectionContainerFacade.GetCacheField(SYSTEMCOMPANYINFORMATION_COMPANYLANGUAGE_COLUMN, SYSTEMCOMPANYINFORMATION_TABLE) & _
                        "' and ItemCode NOT IN (SELECT ItemCode_DEV000221 FROM InventoryMagentoDetails_DEV000221 WHERE Publish_DEV000221 = 1 AND InstanceID_DEV000221 = '" & Me.cbeSiteOrAccount.EditValue.ToString.Replace("'", "''") & "')"
                End If

            Case Else
                Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = ""

        End Select
        If Me.cbeInventoryTypeFilter.EditValue IsNot Nothing Then
            Select Case Me.cbeInventoryTypeFilter.EditValue.ToString
                Case "Stock Items"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemType = '" & ITEM_TYPE_STOCK & "'"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = "InventoryItemNotPublishedView_DEV000221" ' TJS 04/01/14
                Case "Non-Stock Items"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemType = '" & ITEM_TYPE_NON_STOCK & "'"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = "InventoryItemNotPublishedView_DEV000221" ' TJS 04/01/14
                Case "Service Items"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemType = '" & ITEM_TYPE_SERVICE & "'"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = "InventoryItemNotPublishedView_DEV000221" ' TJS 04/01/14
                Case "Matrix Groups"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemType = '" & ITEM_TYPE_MATRIX_GROUP & "'"
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = "InventoryItemNotPublishedView_DEV000221" ' TJS 04/01/14
                Case "Matrix Items" ' TJS 30/10/12
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemType = '" & ITEM_TYPE_MATRIX_ITEM & "'" ' TJS 30/10/12
                    Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = "InventoryItemNotPublishedView_DEV000221" ' TJS 04/01/14
            End Select
        Else
            Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName = ""
        End If
        ' start of code added TJS 04/01/14
        If m_CategoryFilter <> "" Then
            Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemCode IN (SELECT ItemCode FROM InventoryCategory WHERE CategoryCode IN (" & m_CategoryFilter & "))"
        End If
        If m_DepartmentFilter <> "" Then
            Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemCode IN (SELECT ItemCode FROM InventoryItemDepartment WHERE DepartmentCode IN (" & m_DepartmentFilter & "))"
        End If
        If m_ManufacturerFilter <> "" Then
            Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ManufacturerCode = '" & m_ManufacturerFilter & "'"
        End If
        If m_StatusFilter <> "" Then
            Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and [Status] = '" & m_StatusFilter & "'"
        End If
        If m_SupplierFilter <> "" Then
            Me.BaseSearchDashboardItemToPublish.ListControlDashboard.AdditionalFilter += " and ItemCode IN (SELECT ItemCode FROM InventorySupplier WHERE SupplierCode = '" & m_SupplierFilter & "')"
        End If
        ' end of code added TJS 04/01/14
        If Me.BaseSearchDashboardItemToPublish.ListControlDashboard.TableName <> "" Then
            Me.BaseSearchDashboardItemToPublish.Enabled = True
            Me.BaseSearchDashboardItemToPublish.ListControlDashboard.LoadList()
        Else
            Me.BaseSearchDashboardItemToPublish.Enabled = False
        End If

    End Sub
#End Region

#Region " PublishedOtherInstancesClicked "
    Private Sub PublishedOtherInstancesClicked(sender As Object, e As System.EventArgs)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkPublishedOtherInstances.Checked Then
            Me.cbePublishedOtherInstances.Enabled = True
        Else
            Me.cbePublishedOtherInstances.Enabled = False
            Me.cbePublishedOtherInstances.ErrorText = ""
        End If
        InventoryTypeFilterSelected(sender, e)

    End Sub

    Private Sub cbePublishedOtherInstances_EditValueChanged(sender As Object, e As System.EventArgs)

        InventoryTypeFilterSelected(sender, e)

    End Sub
#End Region

#Region " UseMappedCBCategories "
    Private Sub UseMappedCBCategories(sender As System.Object, e As System.EventArgs) Handles chkUseMappedCBCategories.CheckedChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.chkUseMappedCBCategories.Checked Then
            Me.TreeListCategories.Enabled = False
        Else
            Me.TreeListCategories.Enabled = True
        End If

    End Sub
#End Region

#Region " BulkPublishingWizardResized "
    Private Sub BulkPublishingWizardResized(sender As Object, e As System.EventArgs) Handles Me.Resize
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.LayoutControl1.Height = Me.Height - 121

    End Sub
#End Region

#Region " MagentoPriceSourceSelected "
    Private Sub MagentoPriceSourceSelected(sender As Object, e As System.EventArgs) Handles cbeTargetPriceSource.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Promotional pricing option on special price
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Dim Coll As DevExpress.XtraEditors.Controls.ImageComboBoxItemCollection ' TJS 13/11/13
        Dim CollItem As DevExpress.XtraEditors.Controls.ImageComboBoxItem ' TJS 13/11/13
        Dim strTemp As String

        ' start of code added TJS 13/11/13
        If Me.cbeMagentoSpecialPriceSource.EditValue IsNot Nothing Then
            strTemp = Me.cbeMagentoSpecialPriceSource.EditValue.ToString
        Else
            strTemp = ""
        End If
        Coll = Me.cbeMagentoSpecialPriceSource.Properties.Items
        Coll.BeginUpdate()
        Coll.Clear()
        If Me.cbeTargetPriceSource.EditValue.ToString = "S" Then
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Retail Price"
            CollItem.Value = "R"
            Coll.Add(CollItem)
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Wholesale Price"
            CollItem.Value = "W"
            Coll.Add(CollItem)
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Promotional Price"
            CollItem.Value = "P"
            Coll.Add(CollItem)
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Not Set"
            CollItem.Value = "N"
            Coll.Add(CollItem)
            ' end of code added TJS 13/11/13
            Me.cbeMagentoSpecialPriceSource.Enabled = True
            Me.DateEditSpecialFrom.Enabled = True
            Me.DateEditSpecialTo.Enabled = True
            Me.lblMagentoSpecialPrice.Enabled = True
            Me.lblSpecialFrom.Enabled = True
            Me.lblSpecialTo.Enabled = True
        Else
            ' start of code added TJS 13/11/13
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Promotional Price"
            CollItem.Value = "P"
            Coll.Add(CollItem)
            CollItem = New DevExpress.XtraEditors.Controls.ImageComboBoxItem()
            CollItem.Description = "Not Set"
            CollItem.Value = "N"
            Coll.Add(CollItem)
            ' end of code added TJS 13/11/13
            Me.cbeMagentoSpecialPriceSource.Enabled = True
            Me.DateEditSpecialFrom.Enabled = True
            Me.DateEditSpecialTo.Enabled = True
            Me.lblMagentoSpecialPrice.Enabled = True
            Me.lblSpecialFrom.Enabled = True
            Me.lblSpecialTo.Enabled = True
        End If
        ' start of code added TJS 13/11/13
        Coll.EndUpdate()
        If strTemp <> "" Then
            Me.cbeMagentoSpecialPriceSource.EditValue = strTemp
            If strTemp = "P" Then
                Me.DateEditSpecialFrom.Enabled = False
                Me.DateEditSpecialTo.Enabled = False
                Me.lblSpecialFrom.Enabled = False
                Me.lblSpecialTo.Enabled = False
            End If
        End If
        ' end of code added TJS 13/11/13

    End Sub

    Private Sub cbeMagentoSpecialPriceSource_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cbeMagentoSpecialPriceSource.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.cbeMagentoSpecialPriceSource.EditValue IsNot Nothing AndAlso Me.cbeMagentoSpecialPriceSource.EditValue.ToString = "P" Then
            Me.DateEditSpecialFrom.Enabled = False
            Me.DateEditSpecialTo.Enabled = False
            Me.lblSpecialFrom.Enabled = False
            Me.lblSpecialTo.Enabled = False
        Else
            Me.DateEditSpecialFrom.Enabled = True
            Me.DateEditSpecialTo.Enabled = True
            Me.lblSpecialFrom.Enabled = True
            Me.lblSpecialTo.Enabled = True
        End If

    End Sub
#End Region

#Region " QtyPublishingOptionSet "
    Private Sub QtyPublishingOptionSet(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioGroupQtyPublishing.SelectedIndexChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
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

#End Region

#Region " AttbibuteGridFunctions "
    Private Sub GridViewProperties_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridViewProperties.FocusedRowChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento Attribute Value selection
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.PrevFocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then
            Select Case Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text", "Boolean"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagTextValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagTextValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Memo"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagMemoValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagMemoValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Date", "DateTime"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagDateValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagDateValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

                Case "Integer", "Numeric"
                    If Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue) IsNot Nothing AndAlso _
                        Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, "TagNumericValue_DEV000221").ToString <> Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue).ToString Then ' TJS 20/05/12
                        Me.GridViewProperties.SetRowCellValue(e.PrevFocusedRowHandle, "TagNumericValue_DEV000221", Me.GridViewProperties.GetRowCellValue(e.PrevFocusedRowHandle, Me.colPropertiesDisplayValue))
                    End If

            End Select
        End If

        If e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.InvalidRowHandle And _
            e.FocusedRowHandle <> DevExpress.XtraGrid.GridControl.NewItemRowHandle And Me.GridViewProperties.RowCount > 0 Then

            Select Case Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "TagDataType_DEV000221").ToString
                Case "Text"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSiteOrAccount.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit
                    End If

                Case "Memo"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSiteOrAccount.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeMemoEdit
                    End If

                Case "Date", "DateTime"
                    Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeDateEdit

                Case "Integer", "Numeric"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSiteOrAccount.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeTextEdit
                    End If

                Case "Boolean"
                    ' start of code added TJS 13/11/13
                    If CBool(Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeHasSelectValues_DEV000221")) Then
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeSelectEdit
                        Me.rbeSelectEdit.AdditionalFilter = " and InstanceID_DEV000221 = '" & Me.cbeSiteOrAccount.EditValue.ToString & "' and SourceAttributeID_DEV000221 = " & Me.GridViewProperties.GetRowCellValue(e.FocusedRowHandle, "AttributeID_DEV000221").ToString
                        Me.rbeSelectEdit.DisplayField = "SourceValueSetting_DEV000221"
                        Me.rbeSelectEdit.TableName = "LerrynImportExportMagentoAttributeValues_DEV000221"
                        Me.rbeSelectEdit.ValueMember = "SourceValueID_DEV000221"
                    Else
                        ' end of code added TJS 13/11/13
                        Me.colPropertiesDisplayValue.ColumnEdit = Me.rbeYesNoEdit
                    End If

            End Select
        End If

    End Sub
#End Region

#Region " GridColumnsInitialised "
    Private Sub GridColumnsInitialised(sender As Object, e As System.EventArgs) Handles BaseSearchDashboardItemToPublish.EndInitializeColumns
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.Columns("Select").Width = 50
        Me.BaseSearchDashboardItemToPublish.ListControlDashboard.gvwSearch.Columns("Select").OptionsColumn.FixedWidth = True

    End Sub
#End Region

#Region " CategoryMapGridFunctions "
    Private Sub rbeISCategoryCode_PopupClose(sender As Object, eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) Handles rbeISCategoryCode.PopupClose
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | function added
        ' 13/11/13 | TJS             | 2013.3.08 | Corrected tree list name
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iCategoryCount As Integer

        Select Case Me.cbePublishingTarget.EditValue.ToString
            Case "Magento"
                iCategoryCount = m_MagentoImportFacade.MagentoCategories.Length - 1

        End Select

        Me.TreeListCategoryMapping.FocusedNode.Item(Me.colTreeListISCategoryName) = eRow.DataRowSelected.Item("Description").ToString ' TJS 13/11/13
        Me.TreeListCategoryMapping.FocusedNode.Item(Me.colTreeListISCategoryCode) = eRow.DataRowSelected.Item("CategoryCode").ToString ' TJS 13/11/13
        For iLoop = 0 To iCategoryCount
            Select Case Me.cbePublishingTarget.EditValue.ToString
                Case "Magento"
                    If m_MagentoImportFacade.MagentoCategories(iLoop).CategoryID = CInt(Me.TreeListCategoryMapping.FocusedNode.Item(Me.colTreeListTargetCategoryID)) Then ' TJS 13/11/13
                        m_MagentoImportFacade.MagentoCategories(iLoop).ISCategoryCode = eRow.DataRowSelected.Item("CategoryCode").ToString
                        Exit For
                    End If

            End Select
        Next

    End Sub
#End Region

#Region " SourceAttributeCloseUp "
    Private Sub SourceAttributeCloseUp(sender As Object, e As DevExpress.XtraEditors.Controls.CloseUpEventArgs) Handles rbeSourceAttribute.CloseUp
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ' 09/12/13 | TJS             | 2013.4.02 | Modified to detect changes which result in duplicate primary key values and delete them
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, iValueLoop As Integer, iCheckDuplicates As Integer ' TJS 09/12/13

        Cursor = Cursors.WaitCursor ' TJS 09/12/13
        If e.Value IsNot Nothing AndAlso e.Value.GetType.Name <> "DBNull" Then
            Me.GridViewAttributes.SetFocusedRowCellValue("SourceAttributeID_DEV000221", DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value)
            Me.GridViewAttributes.SetFocusedRowCellValue("SourceAttributeName_DEV000221", DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Description)
            ' look for attribute record
            For iLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221.Count - 1
                If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).InstanceID_DEV000221 = Me.cbeSiteOrAccount.EditValue.ToString AndAlso _
                    Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeID_DEV000221 = CInt(DirectCast(e.Value, DevExpress.XtraEditors.Controls.ImageComboBoxItem).Value) Then
                    ' found it, update Attribute code and Use if different
                    If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeCode_DEV000221 <> Me.GridViewAttributes.GetFocusedRowCellValue("AttributeCode").ToString Then
                        Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeCode_DEV000221 = Me.GridViewAttributes.GetFocusedRowCellValue("AttributeCode").ToString
                    End If
                    If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 <> Me.GridViewAttributes.GetFocusedRowCellValue("AttributeUseMatrixOrFilter_DEV000221").ToString Then
                        If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "." Then
                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = Me.GridViewAttributes.GetFocusedRowCellValue("AttributeUseMatrixOrFilter_DEV000221").ToString
                        Else
                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 = "B"
                        End If
                    End If
                    ' now look for attribute values for this attribute
                    For iValueLoop = 0 To Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221.Count - 1
                        If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).RowState <> DataRowState.Deleted Then ' TJS 09/12/13
                            If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).InstanceID_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).InstanceID_DEV000221 AndAlso _
                                Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).SourceAttributeID_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).SourceAttributeID_DEV000221 Then
                                ' found one, update Attribute code and Use if different
                                If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).AttributeCode_DEV000221 <> Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeCode_DEV000221 Then
                                    Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).AttributeCode_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeCode_DEV000221
                                End If
                                If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).AttributeUseMatrixOrFilter_DEV000221 <> Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221 Then
                                    Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).AttributeUseMatrixOrFilter_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributes_DEV000221(iLoop).AttributeUseMatrixOrFilter_DEV000221
                                End If
                                ' start of code added TJS 09/12/13
                                For iCheckDuplicates = 0 To Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221.Count - 1
                                    If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).RowState <> DataRowState.Deleted Then
                                        If Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).InstanceID_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).InstanceID_DEV000221 AndAlso _
                                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).AttributeCode_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).AttributeCode_DEV000221 AndAlso _
                                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).AttributeValueCode_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).AttributeValueCode_DEV000221 AndAlso _
                                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).AttributeUseMatrixOrFilter_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).AttributeUseMatrixOrFilter_DEV000221 AndAlso _
                                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).SourceAttributeID_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).SourceAttributeID_DEV000221 AndAlso _
                                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).SourceValueID_DEV000221 = Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).SourceValueID_DEV000221 AndAlso _
                                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iCheckDuplicates).Counter <> Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).Counter Then
                                            Me.BulkPublisingWizardSectionContainerGateway.LerrynImportExportMagentoAttributeValues_DEV000221(iValueLoop).Delete()
                                            Exit For
                                        End If
                                    End If
                                Next
                                ' end of code added TJS 09/12/13
                            End If
                        End If
                    Next
                End If
            Next
        End If
        Cursor = Cursors.Default ' TJS 09/12/13

    End Sub
#End Region

#Region " ApplyFilter "
    Private Sub ApplyFilter(sender As System.Object, e As System.EventArgs) Handles btnAdditionalFilters.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/01/14 | TJS             | 2013.4.03 | function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim frmApplyFilter As Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterForm

        frmApplyFilter = New Lerryn.Presentation.eShopCONNECT.BulkPublishingWizardFilterForm
        frmApplyFilter.CategoriesToFilter = m_CategoryFilter
        frmApplyFilter.DepartmentsToFilter = m_DepartmentFilter
        frmApplyFilter.ManufacturerToFilter = m_ManufacturerFilter
        frmApplyFilter.StatusToFilter = m_StatusFilter
        frmApplyFilter.SupplierToFilter = m_SupplierFilter
        If frmApplyFilter.ShowDialog = DialogResult.OK Then
            If frmApplyFilter.ApplyCategoryFilter Then
                m_CategoryFilter = frmApplyFilter.CategoriesToFilter
            Else
                m_CategoryFilter = ""
            End If
            If frmApplyFilter.ApplyDepartmentFilter Then
                m_DepartmentFilter = frmApplyFilter.DepartmentsToFilter
            Else
                m_DepartmentFilter = ""
            End If
            If frmApplyFilter.ApplyManufacturerFilter Then
                m_ManufacturerFilter = frmApplyFilter.ManufacturerToFilter
            Else
                m_ManufacturerFilter = ""
            End If
            If frmApplyFilter.ApplyStatusFilter Then
                m_StatusFilter = frmApplyFilter.StatusToFilter
            Else
                m_StatusFilter = ""
            End If
            If frmApplyFilter.ApplySupplierFilter Then
                m_SupplierFilter = frmApplyFilter.SupplierToFilter
            Else
                m_SupplierFilter = ""
            End If
            InventoryTypeFilterSelected(Me, Nothing)
        End If

    End Sub
#End Region
#End Region

End Class
#End Region

