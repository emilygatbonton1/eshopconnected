' eShopCONNECT for Connected Business
' Module: SelectInstanceToCopySection.vb
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
' Updated 29 May 2013

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Microsoft.VisualBasic

#Region " SelectInstanceToCopySection "
<Interprise.Presentation.Base.PluginBinding(Interprise.Framework.Base.Shared.Enum.PluginBindingType.None, "Lerryn.Presentation.eShopCONNECT.SelectInstanceToCopySection")> _
Public Class SelectInstanceToCopySection
	
#Region " Variables "
    Private m_SelectInstanceToCopyDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_SelectInstanceToCopySectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_PublishedInstanceCount As Integer = 0
#End Region

#Region " Properties "
#Region " CurrentDataset "
	Public Overrides ReadOnly Property CurrentDataset() As Interprise.Framework.Base.DatasetComponent.BaseDataset
		Get
			Return Me.SelectInstanceToCopySectionGateway
		End Get
	End Property
#End Region

#Region " SelectInstanceToCopyDataset "
    Public ReadOnly Property SelectInstanceToCopyDataset() As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
        Get
            Return Me.m_SelectInstanceToCopyDataset
        End Get
    End Property
#End Region

#Region " CurrentFacade "
	Public Overrides ReadOnly Property CurrentFacade() As Interprise.Extendable.Base.Facade.IBaseInterface
		Get
			Return Me.m_SelectInstanceToCopySectionFacade			
			
		End Get
	End Property
#End Region

#Region " PublishedInstanceCount "
    Public WriteOnly Property PublishedInstanceCount() As Integer
        Set(value As Integer)
            m_PublishedInstanceCount = value
        End Set
    End Property
#End Region
#End Region

#Region " Methods "
#Region " Constructor "
	Public Sub New()
		MyBase.New()

        Me.m_SelectInstanceToCopyDataset = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway

		'This call is required by the Windows Form Designer.
		Me.InitializeComponent()

		'Add any initialization after the InitializeComponent() call
        If System.ComponentModel.LicenseManager.UsageMode = System.ComponentModel.LicenseUsageMode.Designtime Then Return
				
		' To solve this error, you must use any facade other than the two:
        Me.m_SelectInstanceToCopySectionFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(Me.m_SelectInstanceToCopyDataset, New Lerryn.Facade.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME)
		
	End Sub

    Public Sub New(ByVal SelectInstanceToCopyDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
       ByVal SelectInstanceToCopySectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
        MyBase.New()

        Me.m_SelectInstanceToCopyDataset = SelectInstanceToCopyDataset
        Me.m_SelectInstanceToCopySectionFacade = SelectInstanceToCopySectionFacade


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

#Region " SetSourceCode "
    Friend Sub SetSourceCode(ByVal SourceCode As String)

        Dim strMessagePt1 As String, strMessagePt2 As String

        If m_PublishedInstanceCount > 1 Then
            strMessagePt1 = "You have already published this Inventory Item to " & m_PublishedInstanceCount.ToString
        Else
            strMessagePt1 = "You have already published this Inventory Item to one "
        End If
        strMessagePt2 = "Do you want to use those settings as the basic for this "
        Select Case SourceCode
            Case AMAZON_SOURCE_CODE
                Me.lblUseSettings.Text = strMessagePt1 & " Amazon Account" & ChrW(13) & ChrW(10) & ChrW(13) & strMessagePt2 & " Account ?"

            Case ASP_STORE_FRONT_SOURCE_CODE
                Me.lblUseSettings.Text = strMessagePt1 & " ASPDotNetStorefront Website" & ChrW(13) & ChrW(10) & ChrW(13) & strMessagePt2 & " Website ?"

            Case CHANNEL_ADVISOR_SOURCE_CODE
                Me.lblUseSettings.Text = strMessagePt1 & " Channel Advisor Account" & ChrW(13) & ChrW(10) & ChrW(13) & strMessagePt2 & " Account ?"

            Case EBAY_SOURCE_CODE
                Me.lblUseSettings.Text = strMessagePt1 & " eBay Account" & ChrW(13) & ChrW(10) & ChrW(13) & strMessagePt2 & " Account ?"

            Case MAGENTO_SOURCE_CODE
                Me.lblUseSettings.Text = strMessagePt1 & " Magento Instance" & ChrW(13) & ChrW(10) & ChrW(13) & strMessagePt2 & " Instance ?"

            Case SHOP_COM_SOURCE_CODE
                Me.lblUseSettings.Text = strMessagePt1 & " Shop.com Catalog ID" & ChrW(13) & ChrW(10) & ChrW(13) & strMessagePt2 & " Catalog ID ?"

        End Select
    End Sub
#End Region
#End Region

#Region " Events "
#Region " rgSelectOption_EditValueChanged "
    Private Sub rgSelectOption_EditValueChanged(sender As Object, e As System.EventArgs) Handles rgSelectOption.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/05/13 | TJS/FA          | 2013.1.13 | Procedure added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'TJS/FA 29/05/13
        If Me.rgSelectOption.EditValue.ToString = "Y" Then
            Me.cbeInstanceToCopy.Enabled = True
            If Me.cbeInstanceToCopy.EditValue IsNot Nothing Then
                DirectCast(Me.ParentForm, SelectInstanceToCopyForm).OKButtonEnabled = True
            Else
                DirectCast(Me.ParentForm, SelectInstanceToCopyForm).OKButtonEnabled = False
            End If
        Else
            Me.cbeInstanceToCopy.Enabled = False
            DirectCast(Me.ParentForm, SelectInstanceToCopyForm).OKButtonEnabled = True
            DirectCast(Me.ParentForm, SelectInstanceToCopyForm).ButtonOk.Focus()
        End If
    End Sub
#End Region

#Region " cbeInstanceToCopy_EditValueChanged "
    Private Sub cbeInstanceToCopy_EditValueChanged(sender As Object, e As System.EventArgs) Handles cbeInstanceToCopy.EditValueChanged
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/05/13 | TJS/FA          | 2013.1.13 | Procedure added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'FA 29/05/13
        If Me.cbeInstanceToCopy.EditValue IsNot Nothing Then
            DirectCast(Me.ParentForm, SelectInstanceToCopyForm).OKButtonEnabled = True
        Else
            DirectCast(Me.ParentForm, SelectInstanceToCopyForm).OKButtonEnabled = False
        End If
    End Sub

#End Region
#End Region
End Class
#End Region

