' eShopCONNECT for Connected Business - Windows Service
' Module: Settings.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 22 April 2013

Imports System.io
Imports System.xml
Imports System.Text
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath
Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.SourceConfig ' TJS 09/07/11

Public Class Settings

    Private pTemp As Boolean
    Public Property Temp() As Boolean
        Get
            Return pTemp
        End Get
        Set(ByVal value As Boolean)
            pTemp = value
        End Set
    End Property

    Public ActiveSources As Collection

    Public Function LoadXMLConfig(ByRef SourceConfig As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway.LerrynImportExportConfig_DEV000221Row) As Boolean ' TJS 19/08/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Extracts the source settings
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 05/07/12 | TJS             | 2012.1.08 | Code moved to ImportExportRule LoadConfigSettings function
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim SourceSet As SourceSettings = New SourceSettings ' TJS 05/07/12
        Dim strErrorDetails As String = "" ' TJS 05/07/12

        If m_ImportExportConfigFacade.LoadConfigSettings(SourceConfig, SourceSet, bFileImportRequired, bWebIORequired, strErrorDetails) Then ' TJS 05/07/12

            ' has collection been started, if not create it
            If IsNothing(ActiveSources) Then ActiveSources = New Collection
            ' add source to collection
            ActiveSources.Add(SourceSet, SourceConfig.SourceCode_DEV000221)
            Return True
        Else
            m_ErrorNotification.WriteErrorToEventLog(strErrorDetails) ' TJS 05/07/12
            Return False
        End If

    End Function

    Public Function GetXMLElementText(ByVal XMLMessage As XDocument, ByVal ElementName As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Extracts the source settings
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 20/01/09 | TJS             | 2009.1.02 | Function added
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for the use of CDATA tags and XML containing control characters
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNode As XElement

        ' NOTE XPathSelectElement on an XDocument DOES NOT require root element name to be omitted from XPath 
        ' Use an XPATH query to select the specified node
        XMLNode = XMLMessage.XPathSelectElement(ElementName)

        If Not XMLNode Is Nothing Then
            If Left(XMLNode.Value, 9) = "<![CDATA[" And Right(XMLNode.Value, 3) = "]]>" Then ' TJS 19/08/10
                Return Mid(XMLNode.Value, 10, Len(XMLNode.Value) - 12) ' TJS 19/08/10
            Else
                Return XMLNode.Value.Trim ' TJS 07/05/09 
            End If

            XMLNode = Nothing
        Else
            Return ""
        End If

    End Function

    Private Function DecodeConfigXMLValue(ByVal XMLValue As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Extracts the source settings
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/08/10 | TJS             | 2010.1.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim builder As New StringBuilder(), iLoop As Integer
        For iLoop = 1 To Len(XMLValue)
            If Mid(XMLValue, iLoop, 1) = "%" Then
                builder.Append(Chr(CInt("&H" & Mid(XMLValue, iLoop + 1, 2))))
                iLoop = iLoop + 2
            Else
                builder.Append(Mid(XMLValue, iLoop, 1))
            End If
        Next
        Return builder.ToString

    End Function

End Class

