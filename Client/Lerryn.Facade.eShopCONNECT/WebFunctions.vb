' Lerryn Licence Generation and Validation
' Module: WebFunctions.vb
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
'-------------------------------------------------------------------
'
' Updated 24 February 2012

Imports Microsoft.VisualBasic ' TJS 26/01/09
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Module WebFunctions

    Public Function ConvertEntitiesForXML(ByVal StringToConvert As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.  | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String

        sTemp = StringToConvert.Replace("&", "&amp;")
        sTemp = sTemp.Replace("<", "&lt;")
        sTemp = sTemp.Replace(">", "&gt;")
        sTemp = sTemp.Replace("""", "&quot;")
        sTemp = sTemp.Replace("'", "&apos;")
        Return sTemp

    End Function

    Public Function ConvertCharsFromWeb(ByVal StringToConvert As String) As String ' TJS 11/08/08
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    Converts characters received from the web for loading into an XML document
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.  | Description
        '------------------------------------------------------------------------------------------
        ' 29/05/08 | TJS             | 1.0.00 | Original
        ' 11/08/08 | TJS             | 1.0.02 | Renamed procedure to clarify function
        ' 16/09/08 | TJS             | 1.0.03 | Modified to cater for additional characters 
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to prevent + being converted to space and to correctly conver %20 to space
        ' 24/02/12 | TJS             | 2011.2.08 | Corrected conversion of %26
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String

        sTemp = StringToConvert.Replace("%09", vbTab) ' TJS 16/09/08
        sTemp = sTemp.Replace("%0A", vbLf) ' TJS 16/09/08
        sTemp = sTemp.Replace("%0a", vbLf) ' TJS 16/09/08
        sTemp = sTemp.Replace("%0D", vbCr) ' TJS 16/09/08
        sTemp = sTemp.Replace("%0d", vbCr) ' TJS 16/09/08
        sTemp = sTemp.Replace("%20", " ") ' TJS 18/03/11
        sTemp = sTemp.Replace("%21", "!")
        sTemp = sTemp.Replace("%22", """") ' TJS 16/09/08
        sTemp = sTemp.Replace("%23", "#") ' TJS 16/09/08
        sTemp = sTemp.Replace("%24", "$") ' TJS 16/09/08
        ' do %25 last to prevent it causing secondary substitutions
        sTemp = sTemp.Replace("%26", "&") ' TJS 24/02/12
        sTemp = sTemp.Replace("%2B", "+")
        sTemp = sTemp.Replace("%2b", "+") ' TJS 16/09/08
        sTemp = sTemp.Replace("%2C", ",") ' TJS 16/09/08
        sTemp = sTemp.Replace("%2c", ",") ' TJS 16/09/08

        sTemp = sTemp.Replace("%2F", "/") ' TJS 16/09/08
        sTemp = sTemp.Replace("%2f", "/") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3A", ":")
        sTemp = sTemp.Replace("%3a", ":") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3B", ";") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3b", ";") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3C", "<") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3c", "<") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3D", "=")
        sTemp = sTemp.Replace("%3d", "=") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3E", ">") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3e", ">") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3F", "?") ' TJS 16/09/08
        sTemp = sTemp.Replace("%3f", "?") ' TJS 16/09/08
        sTemp = sTemp.Replace("%40", "@") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5B", "[") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5b", "[") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5C", "\") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5c", "\") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5D", "]") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5d", "]") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5E", "^") ' TJS 16/09/08
        sTemp = sTemp.Replace("%5e", "^") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7B", "{") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7b", "{") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7C", "|") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7c", "|") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7D", "}") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7d", "}") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7E", "~") ' TJS 16/09/08
        sTemp = sTemp.Replace("%7e", "~") ' TJS 16/09/08
        sTemp = sTemp.Replace("%u00a3", "£") ' TJS 16/09/08
        ' do this last to prevent it causing secondary substitutions
        sTemp = sTemp.Replace("%25", "%")

        Return sTemp

    End Function

    Public Function ConvertEntitiesFromXML(ByVal StringToConvert As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.  | Description
        '------------------------------------------------------------------------------------------
        ' 30/07/08 | TJS             | 1.0.01 | function added
        ' 24/02/12 | TJS             | 2011.2.08 | Modified to convert &amp; first as it can be nested with others
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String = ""

        sTemp = Replace(StringToConvert, "&amp;", "&") & "" ' TJS 30/07/08 TJS 24/02/12
        sTemp = Replace(sTemp, "&lt;", "<") & "" ' TJS 30/07/08
        sTemp = Replace(sTemp, "&gt;", ">") & "" ' TJS 30/07/08
        sTemp = Replace(sTemp, "&quot;", """") & "" ' TJS 30/07/08
        sTemp = Replace(sTemp, "&apos;", "'") & "" ' TJS 30/07/08
        Return sTemp & ""

    End Function

    Public Function GetElementText(ByVal XMLMessage As XElement, ByVal ElementName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 07/05/09 | TJS             | 2009.2.05 | Modified to remove leading and trailing spaces
        ' 19/08/10 | TJS             | 2010.1.00 | Modified to cater for the use of CDATA tags
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNode As XElement, iSlashPosn As Integer

        ' XPathSelectElement on an XElement requires root element name to be omitted from XPath so check if 
        ' ElementName starts with / and remove root element name if necessary
        iSlashPosn = InStr(ElementName, "/")
        If iSlashPosn > 1 Then ' TJS 02/12/11
            ElementName = Mid(ElementName, iSlashPosn)
        End If

        ' Use an XPATH query to select the specified node
        If XMLNSMan IsNot Nothing Then ' TJS 02/12/11
            XMLNode = XMLMessage.XPathSelectElement(ElementName, XMLNSMan) ' TJS 02/12/11
        Else
            XMLNode = XMLMessage.XPathSelectElement(ElementName) ' TJS 02/12/11
        End If

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

    Public Function GetElementText(ByVal XMLMessage As XDocument, ByVal ElementName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNode As XElement

        ' NOTE XPathSelectElement on an XDocument DOES NOT require root element name to be omitted from XPath 
        ' Use an XPATH query to select the specified node
        If XMLNSMan IsNot Nothing Then ' TJS 02/12/11
            XMLNode = XMLMessage.XPathSelectElement(ElementName, XMLNSMan) ' TJS 02/12/11
        Else
            XMLNode = XMLMessage.XPathSelectElement(ElementName) ' TJS 02/12/11
        End If

        If Not XMLNode Is Nothing Then
            If Left(XMLNode.Value, 9) = "<![CDATA[" And Right(XMLNode.Value, 3) = "]]>" Then
                Return Mid(XMLNode.Value, 10, Len(XMLNode.Value) - 12)
            Else
                Return XMLNode.Value.Trim
            End If

            XMLNode = Nothing
        Else
            Return ""
        End If

    End Function

    Public Function GetElementAttribute(ByVal XMLMessage As XElement, ByVal ElementName As String, ByVal AttributeName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNode As XElement, XMLAttrib As XAttribute, XMLAttributes As System.Collections.Generic.IEnumerable(Of XAttribute) ' TJS 02/12/11
        Dim iSlashPosn As Integer ' TJS 02/12/11

        ' XPathSelectElement requires root element name to be omitted from XPath so check if 
        ' ElementName starts with / and remove root element name if necessary
        iSlashPosn = InStr(ElementName, "/") ' TJS 02/12/11
        If iSlashPosn > 0 Then ' TJS 02/12/11
            ElementName = Mid(ElementName, iSlashPosn) ' TJS 02/12/11
        End If

        ' Use an XPATH query to select the specified node
        If XMLNSMan IsNot Nothing Then ' TJS 02/12/11
            XMLNode = XMLMessage.XPathSelectElement(ElementName, XMLNSMan) ' TJS 02/12/11
        Else
            XMLNode = XMLMessage.XPathSelectElement(ElementName) ' TJS 02/12/11
        End If

        If Not XMLNode Is Nothing Then
            ' 
            XMLAttributes = XMLNode.Attributes() ' TJS 02/12/11
            For Each XMLAttrib In XMLAttributes ' TJS 02/12/11
                If XMLAttrib.Name = AttributeName Then ' TJS 02/12/11
                    Return XMLAttrib.Value ' and then get the attribute value
                End If
            Next
            Return ""
        Else
            Return ""
        End If

    End Function

    Public Function GetElementAttribute(ByVal XMLMessage As XDocument, ByVal ElementName As String, ByVal AttributeName As String, Optional ByRef XMLNSMan As XmlNamespaceManager = Nothing) As String ' TJS 02/12/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNode As XElement, XMLAttrib As XAttribute, XMLAttributes As System.Collections.Generic.IEnumerable(Of XAttribute)

        ' Use an XPATH query to select the specified node
        If XMLNSMan IsNot Nothing Then
            XMLNode = XMLMessage.XPathSelectElement(ElementName, XMLNSMan)
        Else
            XMLNode = XMLMessage.XPathSelectElement(ElementName)
        End If

        If Not XMLNode Is Nothing Then
            ' 
            XMLAttributes = XMLNode.Attributes()
            For Each XMLAttrib In XMLAttributes
                If XMLAttrib.Name = AttributeName Then
                    Return XMLAttrib.Value ' and then get the attribute value
                End If
            Next
            Return ""
        Else
            Return ""
        End If

    End Function

    Public Function GetElementListCount(ByRef XMLElementList As System.Collections.Generic.IEnumerable(Of XElement)) As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iCount As Integer, XmlElement As XElement

        iCount = 0
        For Each XmlElement In XMLElementList
            iCount += 1
        Next
        Return iCount

    End Function

    Public Function GetElementListCount(ByRef XMLElementList As System.Collections.Generic.IEnumerable(Of XNode)) As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iCount As Integer, XmlElement As XElement

        iCount = 0
        For Each XmlElement In XMLElementList
            iCount += 1
        Next
        Return iCount

    End Function
End Module
