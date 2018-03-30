' eShopCONNECT for Connected Business
' Module: SKUAliasWizardSection.vb
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
' Updated 12 July 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Microsoft.VisualBasic
Imports System.Data.Odbc
Imports System.IO

#Region " SKUAliasWizardSection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.SKUAliasWizardSection")> _
Public Class SKUAliasWizardSection

#Region " Variables "
    Private m_SKUAliasWizardDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_SKUAliasWizardSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private bIgnoreWizardPageChangeEvent As Boolean
    Private m_SourceCode As String
    Private m_SourceName As String
#End Region

#Region " Properties "
#Region " CurrentDataset "
    Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
        Get
            Return Me.SKUAliasWizardSectionGateway
        End Get
    End Property
#End Region

#Region " SKUAliasWizardDataset "
    Public ReadOnly Property SKUAliasWizardDataset() As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_SKUAliasWizardDataset
        End Get
    End Property
#End Region

#Region " CurrentFacade "
    Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
        Get
            Return Me.m_SKUAliasWizardSectionFacade

        End Get
    End Property
#End Region

#Region " SourceCode "
    Public Property SourceCode() As String
        Get
            Return m_SourceCode
        End Get
        Set(ByVal value As String)
            m_SourceCode = value
            Select Case value
                Case AMAZON_SOURCE_CODE
                    m_SourceName = "Amazon"
                Case ASP_STORE_FRONT_SOURCE_CODE
                    m_SourceName = "ASPDotNetStorefornt"
                Case CHANNEL_ADVISOR_SOURCE_CODE
                    m_SourceName = "Channel Advisor"
                Case EBAY_SOURCE_CODE
                    m_SourceName = "eBay"
                Case MAGENTO_SOURCE_CODE
                    m_SourceName = "Magento"
                Case SEARS_COM_SOURCE_CODE
                    m_SourceName = "Sears.com"
                Case SHOP_COM_SOURCE_CODE
                    m_SourceName = "Shop.com"
                Case VOLUSION_SOURCE_CODE
                    m_SourceName = "Volusion"
                Case Else
                    m_SourceName = "Unknown source"
            End Select
        End Set
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
        ' 12/06/13 | TJS             | 2013.1.19 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Me.m_SKUAliasWizardDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return

        Me.m_SKUAliasWizardSectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(m_SKUAliasWizardDataset, _
            New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)

    End Sub

    Public Sub New(ByVal SKUAliasWizardDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
          ByVal SKUAliasWizardSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        MyBase.New()

        Me.m_SKUAliasWizardDataset = SKUAliasWizardDataset
        Me.m_SKUAliasWizardSectionFacade = SKUAliasWizardSectionFacade


        'This call is required by the Windows Form Designer.
        Me.InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return
    End Sub
#End Region

#Region " InitialiseControls "
    Public Sub InitialiseControls() ' TJS 03/07/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Original
        ' 03/07/13 | TJS             | 2013.1.24 | Renamed function as InitializeControl doesn't get called
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'This call is required by the Presentation Layer.
        MyBase.InitializeControl()

        'Add any initialization after the InitializeControl() call
        lblPluginsURL.Text = PLUGINS_WEBSITE_URL.Replace("http://", "")
        lblWelcomeFurtherDetails.Text = lblWelcomeFurtherDetails.Text.Replace("IS_PRODUCT_NAME", IS_PRODUCT_NAME)
        Me.WizardControlImport.WelcomeMessage = "This Wizard will guide you through the process of importing your SKU Alias records into eShopCONNECTED." & _
            vbCrLf & vbCrLf & "Please select the function you want to use below :-"
        Me.WizardControlImport.EnableNextButton = False

    End Sub
#End Region
#End Region

#Region " Events "
    Private Sub btnGetFile_Click(sender As System.Object, e As System.EventArgs) Handles btnGetFile.Click
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | 
        ' 03/07/13 | TJS             | 2013.1.24 | Corrected file type and set CheckFileExists
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iSlashPosn As Integer

        Select Case Me.RadioGroupSelectFunction.EditValue.ToString
            Case "N", "E"
                Me.TabPageSelectImportFile.Text = "Select directory for Template creation"
                Me.OpenFileDialog1.Title = "Select Path for Output csv file" ' TJS 03/07/13
                Me.OpenFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*" ' TJS 03/07/13
                Me.OpenFileDialog1.CheckFileExists = False ' TJS 03/07/13
                If Me.TextEditFilePath.Text <> "" Then
                    iSlashPosn = InStrRev(Me.TextEditFilePath.Text, "\")
                    If iSlashPosn > 0 Then
                        Me.OpenFileDialog1.FileName = Me.TextEditFilePath.Text.Substring(iSlashPosn)
                        Me.OpenFileDialog1.InitialDirectory = Me.TextEditFilePath.Text.Substring(0, iSlashPosn)
                    Else
                        Me.OpenFileDialog1.FileName = ""
                    End If
                Else
                    Me.OpenFileDialog1.FileName = ""
                End If
                If Me.OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Me.TextEditFilePath.Text = Me.OpenFileDialog1.FileName.ToString
                    Me.lblFilePathError.Text = ""
                    Me.WizardControlImport.EnableNextButton = True
                Else
                    Me.WizardControlImport.EnableNextButton = False
                End If

            Case "A", "R"
                Me.OpenFileDialog1.Title = "Select SKU Alias file to import"
                Me.OpenFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
                Me.OpenFileDialog1.CheckFileExists = True ' TJS 03/07/13
                If Me.TextEditFilePath.Text <> "" Then
                    iSlashPosn = InStrRev(Me.TextEditFilePath.Text, "\")
                    If iSlashPosn > 0 Then
                        Me.OpenFileDialog1.FileName = Me.TextEditFilePath.Text.Substring(iSlashPosn)
                        Me.OpenFileDialog1.InitialDirectory = Me.TextEditFilePath.Text.Substring(0, iSlashPosn)
                    Else
                        Me.OpenFileDialog1.FileName = ""
                    End If
                Else
                    Me.OpenFileDialog1.FileName = ""
                End If
                If Me.OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Me.TextEditFilePath.Text = Me.OpenFileDialog1.FileName.ToString
                    Me.lblFilePathError.Text = ""
                    Me.WizardControlImport.EnableNextButton = True
                Else
                    Me.WizardControlImport.EnableNextButton = False
                End If

            Case Else
                Me.WizardControlImport.EnableNextButton = False
        End Select

    End Sub

    Private Sub RadioGroupSelectFunction_EditValueChanged(sender As Object, e As System.EventArgs) Handles RadioGroupSelectFunction.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/07/13 | TJS             | 2013.1.27 | Original
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.RadioGroupSelectFunction.EditValue.ToString <> "" Then
            Me.WizardControlImport.EnableNextButton = True
        End If

    End Sub

    Private Sub WizardControlImport_SelectedPageChanged(sender As Object, e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles WizardControlImport.SelectedPageChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/06/13 | TJS             | 2013.1.19 | Original
        ' 03/07/13 | TJS             | 2013.1.24 | completed code
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strErrorDetails As String, strRejectedRecordFile As String, iRejectRecordCount As Integer
        Dim bSelectedSourceOnly As Boolean, bReturnedValue As Boolean ' TJS 03/07/13

        ' have we triggered this event by changing the focused page ?
        If Not Me.bIgnoreWizardPageChangeEvent Then
            ' no, which page is being displayed ?
            Cursor = Cursors.WaitCursor
            If ReferenceEquals(e.Page, Me.TabPageWelcome) Then
                ' Welcome Code page, must have clicked Back

            ElseIf ReferenceEquals(e.Page, Me.TabPageSelectImportFile) Then
                ' Select file page, was last page the Welcome page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageWelcome) Then
                    ' yes, must have clicked Next
                    Select Case Me.RadioGroupSelectFunction.EditValue.ToString
                        Case "N", "E"
                            Me.TabPageSelectImportFile.Text = "Select directory for Template creation"

                        Case "A", "R"
                            Me.TabPageSelectImportFile.Text = "Select file to Import"
                    End Select
                    Me.WizardControlImport.EnableNextButton = False

                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageProcessing) Then
                ' Processing page, was last page the Select file page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageSelectImportFile) Then
                    ' yes, must have clicked Next
                    Me.WizardControlImport.EnableNextButton = False

                    strErrorDetails = ""
                    Select Case Me.RadioGroupSelectFunction.EditValue.ToString
                        Case "N"
                            Me.TabPageProcessing.Text = "Creating empty SKU Alias Template file"
                            Me.lblProgress.Text = ""
                            If Me.m_SKUAliasWizardSectionFacade.CreateSKUAliasTemplate(Me.TextEditFilePath.EditValue.ToString, strErrorDetails) Then
                                Me.WizardControlImport.FinishMessage = "You have successfully completed the eShopCONNECTED SKU Alias Wizard Import Template file creation." & vbCrLf & vbCrLf & "When you have populated the file, run this Wizard again to import the records." ' TJS 03/07/13
                                Me.lblProgress.Text = "Empty SKU Alias template created as " & vbCrLf & Me.TextEditFilePath.EditValue.ToString ' TJS 03/07/13
                                Me.WizardControlImport.EnableNextButton = True ' TJS 03/07/13
                            Else
                                Me.lblProgress.Text = strErrorDetails
                            End If

                        Case "E"
                            Me.TabPageProcessing.Text = "Exporting existing SKU Alias records"
                            Me.lblProgress.Text = ""
                            If Interprise.Presentation.Base.Message.MessageWindow.Show("Do you want to export SKU Aliases for ALL sources ?" & vbCrLf & vbCrLf & "Click No to only export SKU Aliases for " & m_SourceName & ".", "All Sources or only " & m_SourceName, Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.No Then
                                bSelectedSourceOnly = True ' TJS 03/07/13
                                bReturnedValue = Me.m_SKUAliasWizardSectionFacade.ExportSKUAliasRecords(Me.TextEditFilePath.EditValue.ToString, m_SourceCode, strErrorDetails)
                            Else
                                bSelectedSourceOnly = False ' TJS 03/07/13
                                bReturnedValue = Me.m_SKUAliasWizardSectionFacade.ExportSKUAliasRecords(Me.TextEditFilePath.EditValue.ToString, "", strErrorDetails)
                            End If
                            If bReturnedValue Then
                                Me.WizardControlImport.FinishMessage = "You have successfully completed the eShopCONNECTED SKU Alias Wizard existing record export." & vbCrLf & vbCrLf & "When you have updated the file, run this Wizard again to import the new/amended records." ' TJS 03/07/13
                                ' set displayed page to Finish page (don't set Ignore page change flag as we want this routine run again)
                                Me.lblProgress.Text = "Existing SKU Alias records exported for " ' TJS 03/07/13
                                If bSelectedSourceOnly Then ' TJS 03/07/13
                                    Me.lblProgress.Text = Me.lblProgress.Text & m_SourceName & " source" ' TJS 03/07/13
                                Else
                                    Me.lblProgress.Text = Me.lblProgress.Text & "all sources" ' TJS 03/07/13
                                End If
                                Me.lblProgress.Text = Me.lblProgress.Text & " as" & vbCrLf & Me.TextEditFilePath.EditValue.ToString ' TJS 03/07/13
                                Me.WizardControlImport.EnableNextButton = True ' TJS 03/07/13
                            Else
                                Me.lblProgress.Text = strErrorDetails
                            End If

                        Case "A"
                            Me.TabPageProcessing.Text = "Importing additional SKU Alias records"
                            Me.lblProgress.Text = ""
                            If Me.m_SKUAliasWizardSectionFacade.ImportSKUAliasRecords(Me.TextEditFilePath.EditValue.ToString, m_SourceCode, False, False, strErrorDetails, strRejectedRecordFile, iRejectRecordCount) Then' TJS 03/07/13
                                Me.WizardControlImport.EnableNextButton = True
                                If iRejectRecordCount > 0 Then
                                    Me.lblProgress.Text = strErrorDetails
                                    Me.WizardControlImport.FinishMessage = "Some errors were encountered and not all records were successfully imported." & vbCrLf & vbCrLf & "The rejected records and their reason for rejection can be found in file" & vbCrLf & strRejectedRecordFile' TJS 03/07/13
                                Else
                                    Me.lblProgress.Text = "All records imported suggessfully"
                                    ' set displayed page to Finish page (don't set Ignore page change flag as we want this routine run again)
                                    Me.WizardControlImport.FocusPage = Me.TabPageComplete ' TJS 03/07/13
                                    Me.WizardControlImport.FinishMessage = "All SKU Alias records imported successfully and added to existing records" ' TJS 03/07/13
                                End If
                            Else
                                Me.lblProgress.Text = strErrorDetails
                                Me.WizardControlImport.FinishMessage = "Some errors were encountered and not all records were successfully imported." & vbCrLf & vbCrLf & "The rejected records and their reason for rejection can be found in file" & vbCrLf & strRejectedRecordFile' TJS 03/07/13
                            End If

                        Case "R"
                            Me.TabPageProcessing.Text = "Importing replacement SKU Alias records"
                            Me.lblProgress.Text = ""
                            If Interprise.Presentation.Base.Message.MessageWindow.Show("Do you want to delete SKU Aliases for ALL sources ?" & vbCrLf & vbCrLf & "Click No to only delete SKU Aliases for " & m_SourceName & ".", "All Sources or only " & m_SourceName, Interprise.Framework.Base.Shared.MessageWindowButtons.YesNo, Interprise.Framework.Base.Shared.MessageWindowIcon.Question, Interprise.Framework.Base.Shared.MessageWindowDefaultButton.Button2) = DialogResult.Yes Then ' TJS 12/07/13
                                bSelectedSourceOnly = False ' TJS 03/07/13
                                bReturnedValue = Me.m_SKUAliasWizardSectionFacade.ImportSKUAliasRecords(Me.TextEditFilePath.EditValue.ToString, m_SourceCode, True, True, strErrorDetails,strRejectedRecordFile, iRejectRecordCount)' TJS 03/07/13
                            Else
                                bSelectedSourceOnly = True ' TJS 03/07/13
                                bReturnedValue = Me.m_SKUAliasWizardSectionFacade.ImportSKUAliasRecords(Me.TextEditFilePath.EditValue.ToString, m_SourceCode, True, False, strErrorDetails,strRejectedRecordFile, iRejectRecordCount)' TJS 03/07/13
                            End If
                            If bReturnedValue Then
                                Me.WizardControlImport.EnableNextButton = True
                                If iRejectRecordCount > 0 Then
                                    Me.lblProgress.Text = strErrorDetails
                                    Me.WizardControlImport.FinishMessage = "Some errors were encountered and not all records were successfully imported." & vbCrLf & vbCrLf & "The rejected records and their reason for rejection can be found in file" & vbCrLf & strRejectedRecordFile
                                Else
                                    Me.lblProgress.Text = "All records imported suggessfully"
                                    ' set displayed page to Finish page (don't set Ignore page change flag as we want this routine run again)
                                    Me.WizardControlImport.FocusPage = Me.TabPageComplete ' TJS 03/07/13
                                    Me.WizardControlImport.FinishMessage = "All SKU Alias records imported successfully and existing records deleted/replaced " ' TJS 03/07/13
                                    If bSelectedSourceOnly Then ' TJS 03/07/13
                                        Me.WizardControlImport.FinishMessage = Me.WizardControlImport.FinishMessage & " for all sources" ' TJS 03/07/13
                                    Else
                                        Me.WizardControlImport.FinishMessage = Me.WizardControlImport.FinishMessage & " for the " & m_SourceName & " source" ' TJS 03/07/13
                                    End If
                                End If
                            Else
                                Me.lblProgress.Text = strErrorDetails
                                Me.WizardControlImport.FinishMessage = "Some errors were encountered and not all records were successfully imported." & vbCrLf & vbCrLf & "The rejected records and their reason for rejection can be found in file" & vbCrLf & strRejectedRecordFile
                            End If

                    End Select

                Else
                    ' no, must have clicked Back

                End If

            ElseIf ReferenceEquals(e.Page, Me.TabPageComplete) Then
                ' Finish page, was last page the Processing page ?
                If ReferenceEquals(e.PrevPage, Me.TabPageProcessing) Then
                    ' yes, must have clicked Next
                    Me.WizardControlImport.FinishButtonEnabled = True ' TJS 03/07/13
                    Me.WizardControlImport.buttonBack.Visible = False ' TJS 03/07/13
                    Me.WizardControlImport.buttonCancel.Visible = False ' TJS 03/07/13

                End If
            End If
            Cursor = Cursors.Default
        End If

    End Sub

#Region " GotoPluginsWebSite "
    Private Sub GotoPluginsWebSite(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPluginsURL.Click

        System.Diagnostics.Process.Start(Me.lblPluginsURL.Text)

    End Sub
#End Region
#End Region
End Class
#End Region

