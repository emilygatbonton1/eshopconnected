' eShopCONNECT for Connected Business
' Module: PropertyHelpForm.vb
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
' Updated 26 June 2009

Option Explicit On
Option Strict On

#Region " PropertyHelpForm "
<MenuActionAttribute.MenuAction("OpenPropertyHelpForm")> _
Public Class PropertyHelpForm

#Region " Variables "
    Private m_InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway
    Private m_InventorySettingsFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade
    Private m_PropertyHelpSection As PropertyHelpSection
#End Region

#Region " Methods "
#Region " Constructor "
    Public Sub New(ByVal InventorySettingsDataset As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway, _
          ByVal InventorySettingsSectionFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade)
        MyBase.New()

        Me.m_InventorySettingsDataset = InventorySettingsDataset
        Me.m_InventorySettingsFacade = InventorySettingsSectionFacade

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.CreatePropertyHelpSection()
    End Sub
#End Region

#Region " CreatePropertyHelpSection "
    Protected Overridable Sub CreatePropertyHelpSection()
        Me.m_PropertyHelpSection = New PropertyHelpSection(Me.m_InventorySettingsDataset, Me.m_InventorySettingsFacade)
        Me.m_PropertyHelpSection.Dock = DockStyle.Fill
        Me.CurrentControl = Me.m_PropertyHelpSection
        Me.PanelBody.Controls.Add(Me.m_PropertyHelpSection)
    End Sub
#End Region

#Region " DisplayTagHelp "
    Public Sub DisplayTagHelp(ByVal ConnectorName As String, ByVal TagName As String, ByVal TagDescription As String, ByVal TagAcceptedValues As String, ByRef TagExample As String, ByVal TagStatus As String, ByVal TagConditionality As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/05/09 | TJS             | 2009.2.08 | Modified to display tag name
        ' 26/06/09 | TJS             | 2009.3.00 | Modified to display Connector name
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Me.Text = ConnectorName & " Property Help" ' TJS 25/05/09 TJS 26/6/09
        If TagName <> "" Then ' TJS 25/05/09
            Me.Text += " - " & TagName ' TJS 25/05/09
        End If

        If Me.m_PropertyHelpSection IsNot Nothing Then
            Me.m_PropertyHelpSection.DisplayTagHelp(TagDescription, TagAcceptedValues, TagExample, TagStatus, TagConditionality)
        End If
    End Sub
#End Region

#Region " ShowHelpTabs "
    Public Sub ShowHelpTabs(ByVal ShowDescriptionTag As Boolean, ByVal ShowAcceptedValuesTab As Boolean, _
        ByRef ShowTagExampleTab As Boolean, ByVal ShowTagStatusTab As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 26/06/09 | TJS             | 2009.3.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Me.m_PropertyHelpSection IsNot Nothing Then
            Me.m_PropertyHelpSection.ShowHelpTabs(ShowDescriptionTag, ShowAcceptedValuesTab, ShowTagExampleTab, ShowTagStatusTab)
        End If

    End Sub
#End Region
#End Region

End Class
#End Region
