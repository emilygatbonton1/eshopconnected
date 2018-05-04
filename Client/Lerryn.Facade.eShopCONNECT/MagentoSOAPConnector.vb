' eShopCONNECT for Connected Business 
' Module: MagentoSOAPConnector.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 15 January 2014

Imports Microsoft.VisualBasic
Imports System.IO ' TJS 02/12/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 02/12/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Public Class MagentoSOAPConnector

    Public Structure CategoryType
        Public ISCategoryCode As String ' TJS 30/03/11 
        Public CategoryName As String
        Public CategoryID As Integer
        Public ParentID As Integer
        Public Position As Integer
        Public Level As Integer
    End Structure

    Public Structure AttributeSetType ' TJS 25/04/11
        Public SetID As Integer ' TJS 25/04/11
        Public SetName As String ' TJS 25/04/11
    End Structure

    Public Structure ProductAttributeType ' TJS 25/04/11
        Public AttributeID As Integer ' TJS 25/04/11
        Public AttributeName As String ' TJS 25/04/11
        Public AttributeDescription As String ' TJS 13/11/13
        Public AttributeType As String ' TJS 25/04/11
        Public AttributeReqd As Boolean ' TJS 25/04/11
        Public AttributeScope As String ' TJS 25/04/11
        Public AttributeValue As String ' TJS 25/04/11
        Public UsedForConfigurables As Boolean ' TJS 13/11/13
    End Structure

    Public Structure ProductSuperAttributeType ' TJS 10/06/12
        Public ProductID As Integer ' TJS 10/06/12
        Public SuperAttributeID As Integer ' TJS 10/06/12
        Public AttributeID As Integer ' TJS 10/06/12
        Public AttributeName As String ' TJS 10/06/12
        Public AttributeCode As String ' TJS 10/06/12
        Public AttributePosition As Integer ' TJS 10/06/12
    End Structure

    ' start of code added TJS 13/11/13
    Public Structure ProductLinkDetails
        Public LinkProductID As Integer
        Public LinkType As String
        Public AttributeSet As Integer
        Public LinkProductSKU As String
        Public PositionInList As Integer
        Public LinkProductQty As Integer
    End Structure

    Public Structure ShipmentInvoiceItems
        Public OrderItemID As String
        Public QuantityShipped As Double
    End Structure
    ' end of code added TJS 13/11/13

    ' Magento API URL 
    Private m_MagentoAPIURL As String
    ' Session ID created by Magento
    Private m_SessionID As String
    ' Lerryn API extension version number
    Private m_APIVersion As Decimal = 0 ' TJS 19/09/13
    ' Magento version number
    Private m_MagentoVersion As String = "" ' TJS 02/10/13
    ' Magento V2 Soap API WSI Compliant
    Private m_V2SoapAPIWSICompliant As Boolean = False ' TJS 13/11/13
    ' Magento V2 SOAP API working
    Private m_V2SoapAPIWorks As Boolean = False ' TJS 13/11/13
    ' Returned XML is from Magento V2 SOAP API
    Private m_V2SoapAPIResponse As Boolean = False ' TJS 13/11/13
    ' Returned XML is from Magento V2 SOAP WSI Compliant API
    Private m_V2SoapWSIAPIResponse As Boolean = False ' TJS 13/11/13

    ' XML Posted to Magento
    Private m_PostedXML As String
    Public ReadOnly Property PostedXML() As String
        Get
            Return m_PostedXML
        End Get
    End Property

    ' Magento returned data (or error response)
    Private m_ReturnedData As String
    Public ReadOnly Property ReturnedData() As String
        Get
            Return m_ReturnedData
        End Get
    End Property

    ' Magento returned XML (if valid XML)
    Private m_ReturnedXML As XDocument
    Public ReadOnly Property ReturnedXML() As XDocument
        Get
            Return m_ReturnedXML
        End Get
    End Property

    ' Last error (e.g. from web server)
    Private m_LastError As String
    Public ReadOnly Property LastError() As String
        Get
            Return m_LastError
        End Get
    End Property

    ' Last error message (e.g. from web server)
    Private m_LastErrorMessage As String ' TJS 14/02/12
    Public ReadOnly Property LastErrorMessage() As String ' TJS 14/02/12
        Get
            Return m_LastErrorMessage ' TJS 14/02/12
        End Get
    End Property

    ' Logged into web site status
    Private m_LoggedIn As Boolean
    Public ReadOnly Property LoggedIn() As Boolean
        Get
            Return m_LoggedIn
        End Get
    End Property

    Public Property MagentoVersion() As String ' TJS 02/10/13 TJS 13/11/13
        Get ' TJS 02/10/13
            Return m_MagentoVersion ' TJS 02/10/13
        End Get
        Set(value As String) ' TJS 13/11/13
            m_MagentoVersion = value ' TJS 13/11/13
        End Set
    End Property

    Public Property LerrynAPIVersion() As Decimal ' TJS 13/11/13
        Get ' TJS 13/11/13
            Return m_APIVersion ' TJS 13/11/13
        End Get
        Set(value As Decimal) ' TJS 13/11/13
            m_APIVersion = value ' TJS 13/11/13
        End Set
    End Property

    Public Property V2SoapAPIWSICompliant() As Boolean ' TJS 13/11/13
        Get
            Return m_V2SoapAPIWSICompliant ' TJS 13/11/13
        End Get
        Set(value As Boolean) ' TJS 13/11/13
            m_V2SoapAPIWSICompliant = value ' TJS 13/11/13
        End Set
    End Property

    Public ReadOnly Property V2SoapAPIWorks As Boolean ' TJS 13/11/13
        Get
            Return m_V2SoapAPIWorks ' TJS 13/11/13
        End Get
    End Property

    Public ReadOnly Property APIURL() As String ' TJS 13/11/13
        Get
            Return m_MagentoAPIURL ' TJS 13/11/13
        End Get
    End Property

    Public ReadOnly Property V2SoapAPIResponse() As Boolean ' TJS 13/11/13
        Get
            Return m_V2SoapAPIResponse ' TJS 13/11/13
        End Get
    End Property

    Public ReadOnly Property V2SoapWSIAPIResponse() As Boolean ' TJS 13/11/13
        Get
            Return m_V2SoapWSIAPIResponse ' TJS 13/11/13
        End Get
    End Property

    Private Function PostSOAP(ByVal SOAPToPost As String, ByVal SoapAction As String, ByVal SOAPBaseURL As String, _
        Optional ByVal CallingV2SoapAPI As Boolean = False, Optional ByVal ReceiveTimeout As Integer = 60000) As Boolean ' TJS 04/05/11 TJS 13/11/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Posts SOAP data to Magento
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Added error message if URL is blank
        ' 04/05/11 | TJS             | 2011.0.13 | Added timeout settings
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 09/08/13 | TJS             | 2013.2.02 | Modified to detect invalid URL errors and simplify error message
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for V2 SOAP API calls
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader ' TJS 02/12/11
        Dim bSuccess As Boolean = False

        If SOAPBaseURL = "" Then
            m_LastError = "Magento API URL is blank" ' TJS 25/04/11
            m_LastErrorMessage = m_LastError ' TJS 14/02/12
            Return False
        End If

        m_LastError = "" ' Clear any previous errors
        m_LastErrorMessage = "" ' TJS 14/02/12
        m_V2SoapAPIResponse = CallingV2SoapAPI ' TJS 13/11/13
        If CallingV2SoapAPI And V2SoapAPIWSICompliant Then ' TJS 13/11/13
            m_V2SoapWSIAPIResponse = True ' TJS 13/11/13
        Else
            m_V2SoapWSIAPIResponse = False ' TJS 13/11/13
        End If

        ' Store the posted SOAP
        m_PostedXML = SOAPToPost

        ' Send the SOAP message
        ' start of code replaced TJS 02/12/11
        Try
            WebSubmit = System.Net.WebRequest.Create(SOAPBaseURL)
            WebSubmit.Method = "POST"
            WebSubmit.ContentType = "text/xml; charset=utf-8"
            WebSubmit.ContentLength = SOAPToPost.Length
            WebSubmit.Headers.Add("SOAPAction", SoapAction)
            WebSubmit.Timeout = ReceiveTimeout

            byteData = UTF8Encoding.UTF8.GetBytes(SOAPToPost)

            ' send to LerrynSecure.com (or the URL defined in the Registry)
            postStream = WebSubmit.GetRequestStream()
            postStream.Write(byteData, 0, byteData.Length)

            WebResponse = WebSubmit.GetResponse
            reader = New StreamReader(WebResponse.GetResponseStream())
            m_ReturnedData = reader.ReadToEnd()

            If m_ReturnedData <> "" Then
                bSuccess = True                 ' Success (this just means no error occured and we got something back)
            Else
                m_LastError = "No response received from Magento API"
                m_LastErrorMessage = m_LastError ' TJS 14/02/12
                bSuccess = False
            End If
            Return bSuccess

        Catch ex As Exception
            If ex.Message.StartsWith("Invalid URI: The format of the URI could not be determined") Then ' TJS 09/08/13
                m_LastError = ex.Message ' Expose the error - TJS 09/08/13
            Else
                m_LastError = ex.Message & " - " & ex.StackTrace ' Expose the error and location
            End If
            m_LastErrorMessage = ex.Message ' and just the message TJS 14/02/12
            Return False

        Finally
            If Not postStream Is Nothing Then postStream.Close()
            If Not WebResponse Is Nothing Then WebResponse.Close()

        End Try
        ' end of code replaced TJS 02/12/11

    End Function

    Public Function Login(ByVal MagentoAPIURL As String, ByVal UserName As String, ByVal Password As String, Optional ByVal CheckVersion As Boolean = False) As Boolean ' TJS 13/11/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Login to Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to get Lerryn API version 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API and Lerryn API v1.11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim strSOAP As String, sSOAPAction As String, sSOAPResponseXPath As String ' TJS 13/11/13
        Dim sSOAPErrorXPath As String, sSOAPURLToUse As String, bTryV1SoapAPI As Boolean ' TJS 13/11/13

        ' We are logging into Magento, erase any existing info
        m_SessionID = ""
        m_LoggedIn = False
        m_LastError = ""
        m_LastErrorMessage = "" ' TJS 14/02/12

        ' start of code added TJS 13/11/13
        ' first try V2 SOAP API
        bTryV1SoapAPI = False
        ' save Magento URL
        m_MagentoAPIURL = MagentoAPIURL
        sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/") ' TJS 13/11/13
        If m_V2SoapAPIWSICompliant Then
            sSOAPAction = ""
            strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
            strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
            strSOAP = strSOAP & "<loginParam xmlns=""urn:Magento"">"
            strSOAP = strSOAP & "<username xmlns="""">" & UserName & "</username>"
            strSOAP = strSOAP & "<apiKey xmlns="""">" & Password & "</apiKey>"
            strSOAP = strSOAP & "</loginParam></s:Body></s:Envelope>"
            sSOAPResponseXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:loginResponseParam/result"
            sSOAPErrorXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring"
        Else
            sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
            strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
            strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
            strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
            strSOAP = strSOAP & "<q1:login xmlns:q1=""urn:Magento"">"
            strSOAP = strSOAP & "<username xsi:type=""xsd:string"">" & UserName & "</username>"
            strSOAP = strSOAP & "<apiKey xsi:type=""xsd:string"">" & Password & "</apiKey>"
            strSOAP = strSOAP & "</q1:login></s:Body></s:Envelope>"
            sSOAPResponseXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:loginResponse/loginReturn"
            sSOAPErrorXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring"
        End If
        If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse, True) Then
            ' Feed the SOAP response into the XMLResponse object for processing
            ' does it start correctly ?
            If m_ReturnedData.Length > 19 Then
                If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                    ' yes, try loading into XML document
                    Try
                        m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                        ' yes, this means we got some valid XML back, but maybe not the correct XML 
                        XMLNameTabMagento = New System.Xml.NameTable
                        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                        XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                        If m_ReturnedXML.XPathSelectElement(sSOAPResponseXPath, XMLNSManMagento) IsNot Nothing Then
                            ' We have a login session string, grab it
                            m_SessionID = m_ReturnedXML.XPathSelectElement(sSOAPResponseXPath, XMLNSManMagento).Value
                            ' check session string not empty ?
                            If m_SessionID.Trim <> "" Then
                                ' no, we are logged in
                                m_V2SoapAPIWorks = True

                                'www.dynenttech.com davidonelson 5/4/2018
                                'OLD CODE forces API extention to be installed on Magento, but it is not required for some functions
                                'If CheckVersion Or m_APIVersion = 0 Then ' TJS 13/11/13

                                'NEW CODE allows us to proceed to use the functions that don't need the Lerry API extension
                                If CheckVersion And m_APIVersion = 0 Then ' TJS 13/11/13
                                    ' yes, try getting API and Magento versions
                                    If Not GetAPIVersion() And m_APIVersion = 0 Then
                                        ' didn't work, try older API namespace
                                        m_APIVersion = 1
                                        If Not GetAPIVersion() Then
                                            ' didn't work, see if we got an invalid API path error
                                            CheckCustomAPIWorking()
                                            Logout()
                                            m_LoggedIn = False

                                        ElseIf m_APIVersion < 1.11 Then ' TJS 13/11/13
                                            ' pre V1.11 had to get API and Magento versions in separate API calls
                                            If Not GetMagentoVersion() Then
                                                ' failed to get Magento version, see if we got an invalid API path error
                                                CheckCustomAPIWorking()
                                                Logout()
                                                m_LoggedIn = False
                                            Else
                                                m_LoggedIn = True
                                            End If
                                        Else
                                            m_LoggedIn = True ' TJS 13/11/13
                                        End If

                                    ElseIf m_APIVersion < 1.11 Then ' TJS 13/11/13
                                        ' pre V1.11 had to get API and Magento versions in separate API calls
                                        If Not GetMagentoVersion() Then
                                            ' failed to get Magento version, see if we got an invalid API path error
                                            CheckCustomAPIWorking()
                                            Logout()
                                            m_LoggedIn = False
                                        Else
                                            m_LoggedIn = True
                                        End If
                                    Else
                                        m_LoggedIn = True ' TJS 13/11/13
                                    End If
                                Else
                                    ' no, return
                                    m_LoggedIn = True ' TJS 13/11/13
                                End If
                            Else
                                m_LoggedIn = False
                            End If

                        ElseIf m_ReturnedXML.XPathSelectElement(sSOAPErrorXPath, XMLNSManMagento) IsNot Nothing Then
                            m_LastError = m_ReturnedXML.XPathSelectElement(sSOAPErrorXPath, XMLNSManMagento).Value
                            m_LastErrorMessage = m_LastError
                            m_LoggedIn = False
                        Else
                            m_LastErrorMessage = "Unknown Login response from " & m_MagentoAPIURL
                            m_LastError = m_LastErrorMessage & " - " & m_ReturnedXML.ToString.Replace(vbCrLf, "")
                            m_LoggedIn = False
                            bTryV1SoapAPI = True
                        End If

                    Catch ex As Exception
                        m_LastErrorMessage = "Login response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                        m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")
                        m_LoggedIn = False
                        bTryV1SoapAPI = True

                    End Try
                Else
                    m_LastErrorMessage = "Login response from " & m_MagentoAPIURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>"
                    m_LastError = m_LastErrorMessage & " - " & m_ReturnedData
                    m_LoggedIn = False
                    bTryV1SoapAPI = True
                End If
            Else
                m_LastErrorMessage = "Login response from " & m_MagentoAPIURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>"
                m_LastError = m_LastErrorMessage & " - " & m_ReturnedData
                m_LoggedIn = False
                bTryV1SoapAPI = True
            End If
        Else
            m_LoggedIn = False
            bTryV1SoapAPI = True
        End If

        If bTryV1SoapAPI Then
            m_LastError = ""
            m_LastErrorMessage = ""
            ' end of code added TJS 13/11/13
            ' try V1 SOAP API
            ' save Magento URL
            sSOAPURLToUse = MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/") ' TJS 13/11/13
            sSOAPAction = "urn:Mage_Api_Model_Server_HandlerAction"
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
            strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" "
            strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
            strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:login><username xsi:type=""xsd:string"">" & UserName
            strSOAP = strSOAP & "</username><apiKey xsi:type=""xsd:string"">" & Password
            strSOAP = strSOAP & "</apiKey></ns1:login></SOAP-ENV:Body></SOAP-ENV:Envelope>"
            sSOAPResponseXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:loginResponse/loginReturn" ' TJS 13/11/13

            If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                ' does it start correctly ?
                If m_ReturnedData.Length > 19 Then
                    If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                        ' yes, try loading into XML document
                        Try
                            m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                            ' yes, this means we got some valid XML back, but maybe not the correct XML 
                            XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                            XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                            XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                            If m_ReturnedXML.XPathSelectElement(sSOAPResponseXPath, XMLNSManMagento) IsNot Nothing Then ' TJS 13/11/13
                                ' We have a login session string, grab it
                                m_SessionID = m_ReturnedXML.XPathSelectElement(sSOAPResponseXPath, XMLNSManMagento).Value ' TJS 13/11/13
                                ' check session string not empty ?
                                If m_SessionID.Trim <> "" Then
                                    ' no, we are logged in
                                    ' start of code added TJS 19/09/13

                                    'www.dynenttech.com davidonelson 5/4/2018
                                    'OLD CODE forces API extention to be installed on Magento, but it is not required for some functions
                                    'If CheckVersion Or m_APIVersion = 0 Then ' TJS 13/11/13

                                    'NEW CODE allows us to proceed to use the functions that don't need the Lerry API extension
                                    If CheckVersion And m_APIVersion = 0 Then ' TJS 13/11/13

                                        ' yes, try getting API and Magento versions
                                        If Not GetAPIVersion() And m_APIVersion = 0 Then
                                            ' didn't work, try older API namespace
                                            m_APIVersion = 1
                                            If Not GetAPIVersion() Then
                                                ' didn't work, see if we got an invalid API path error
                                                CheckCustomAPIWorking()
                                                Logout()
                                                m_LoggedIn = False

                                            ElseIf m_APIVersion < 1.11 Then ' TJS 13/11/13
                                                ' pre V1.11 had to get API and Magento versions in separate API calls
                                                If Not GetMagentoVersion() Then ' TJS 02/10/13
                                                    ' failed to get Magento version, see if we got an invalid API path error
                                                    CheckCustomAPIWorking() ' TJS 02/10/13
                                                    Logout() ' TJS 02/10/13
                                                    m_LoggedIn = False ' TJS 02/10/13
                                                Else
                                                    m_LoggedIn = True
                                                End If
                                            Else
                                                m_LoggedIn = True ' TJS 13/11/13
                                            End If
                                            ' end of code added TJS 19/09/13

                                        ElseIf m_APIVersion < 1.11 Then ' TJS 13/11/13
                                            ' pre V1.11 had to get API and Magento versions in separate API calls
                                            If Not GetMagentoVersion() Then ' TJS 13/11/13
                                                ' failed to get Magento version, see if we got an invalid API path error
                                                CheckCustomAPIWorking() ' TJS 13/11/13
                                                Logout() ' TJS 13/11/13
                                                m_LoggedIn = False ' TJS 13/11/13
                                            Else
                                                m_LoggedIn = True ' TJS 13/11/13
                                            End If
                                        End If
                                    Else
                                        ' no, return
                                        m_LoggedIn = True ' TJS 13/11/13
                                    End If
                                Else
                                    m_LoggedIn = False
                                End If

                            ElseIf m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) IsNot Nothing Then
                                m_LastError = m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value
                                m_LastErrorMessage = m_LastError ' TJS 14/02/12
                                m_LoggedIn = False
                            Else
                                m_LastErrorMessage = "Unknown Login response from " & m_MagentoAPIURL ' TJS 14/02/12
                                m_LastError = m_LastErrorMessage & " - " & m_ReturnedXML.ToString.Replace(vbCrLf, "") ' TJS 14/02/12
                                m_LoggedIn = False
                            End If

                        Catch ex As Exception
                            m_LastErrorMessage = "Login response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                            m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12
                            m_LoggedIn = False

                        End Try
                    Else
                        m_LastErrorMessage = "Login response from " & m_MagentoAPIURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>" ' TJS 14/02/12
                        m_LastError = m_LastErrorMessage & " - " & m_ReturnedData ' TJS 14/02/12
                        m_LoggedIn = False
                    End If
                Else
                    m_LastErrorMessage = "Login response from " & m_MagentoAPIURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?>" ' TJS 14/02/12
                    m_LastError = m_LastErrorMessage & " - " & m_ReturnedData ' TJS 14/02/12
                    m_LoggedIn = False
                End If
            Else
                m_LoggedIn = False
            End If
        End If

        Return m_LoggedIn

    End Function

    Public Function GetAPIVersion() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Gets the version of the Lerryn API Extension and the Magento Version
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 19/09/13 | TJS             | 2013.3.00 | Function added
        ' 02/10/13 | TJS             | 2013.3.03 | Corrected function name in error response
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API and Lerryn API V1.11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement ' TJS 13/11/13
        Dim XMLTemp As XDocument ' TJS 13/11/13
        Dim strSOAP As String, strAPIVersion As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version unknown or at least 1.09 ?
        If m_APIVersion = 0 Or m_APIVersion >= 1.09 Then
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetAPIVersion</resourcePath>"
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetAPIVersion</resourcePath>"
        End If
        strSOAP = strSOAP & "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            ' does it start correctly ?
            If m_ReturnedData.Length > 19 Then
                If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                    ' yes, try loading into XML document
                    Try
                        m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                        ' yes, this means we got some valid XML back, but maybe not the correct XML 
                        XMLNameTabMagento = New System.Xml.NameTable
                        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                        XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                        If m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento) IsNot Nothing Then
                            ' We have returned data - is it an array i.e. v1.11. or later ?
                            ' start of code added TJS 13/11/13
                            strAPIVersion = ""
                            XMLItemList = m_ReturnedXML.XPathSelectElements("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item", XMLNSManMagento)
                            For Each XMLItemNode In XMLItemList
                                XMLTemp = XDocument.Parse(XMLItemNode.ToString)
                                If XMLItemNode.XPathSelectElement("key").Value = "APIVersion" Then
                                    strAPIVersion = XMLItemNode.XPathSelectElement("value").Value
                                ElseIf XMLItemNode.XPathSelectElement("key").Value = "MagentoVersion" Then
                                    m_MagentoVersion = XMLItemNode.XPathSelectElement("value").Value
                                End If
                            Next
                            ' did we find an API version
                            If strAPIVersion = "" Then
                                ' no, must be older version
                                ' end of code added TJS 13/11/13
                                strAPIVersion = m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento).Value
                            End If
                            ' check version number string not empty ?
                            If strAPIVersion.Trim <> "" Then
                                m_APIVersion = CDec(strAPIVersion)
                                Return True
                            End If
                        End If

                    Catch ex As Exception
                        m_LastErrorMessage = "GetAPIVersion response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 02/10/13
                        m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")
                        m_LoggedIn = False

                    End Try

                End If
            End If
        End If
        Return False

    End Function

    Private Function GetMagentoVersion() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/10/13 | TJS             | 2013.3.03 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim strSOAP As String, strMagentoVersion As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version unknown or at least 1.09 ?
        If m_APIVersion = 0 Or m_APIVersion >= 1.09 Then
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetMagentoVersion</resourcePath>"
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetMagentoVersion</resourcePath>"
        End If
        strSOAP = strSOAP & "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            ' does it start correctly ?
            If m_ReturnedData.Length > 19 Then
                If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                    ' yes, try loading into XML document
                    Try
                        m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                        ' yes, this means we got some valid XML back, but maybe not the correct XML 
                        XMLNameTabMagento = New System.Xml.NameTable
                        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
                        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                        XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                        If m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento) IsNot Nothing Then
                            ' We have a version number string, grab it
                            strMagentoVersion = m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn", XMLNSManMagento).Value
                            ' check version number string not empty ?
                            If strMagentoVersion.Trim <> "" Then
                                m_MagentoVersion = strMagentoVersion
                                Return True
                            End If
                        End If

                    Catch ex As Exception
                        m_LastErrorMessage = "GetMagentoVersion response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                        m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")
                        m_LoggedIn = False

                    End Try

                End If
            End If
        End If
        Return False

    End Function

    Public Function GetCatalogTree() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get the Category Tree from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 09/03/13 | TJS             | 2013.0.00 | Corrected error message procedure name
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPURLToUse As String ' TJS 13/11/13
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">catalog_category.tree</resourcePath>"
        strSOAP = strSOAP & "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, sSOAPURLToUse, False) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetCatalogTree response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12 TJS 09/03/13
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCatalogTree response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetProductAttributeList(ByVal AttributeSetID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get list, and details, of product attributes for the specified Attribute Set ID 
        '                   from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 09/03/13 | TJS             | 2013.0.00 | Corrected error message procedure name
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPURLToUse As String ' TJS 13/11/13
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product_attribute.list</resourcePath>"
        strSOAP = strSOAP & "<args xsi:type=""xsd:string"">" & AttributeSetID & "</args>"
        strSOAP = strSOAP & "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, sSOAPURLToUse, False) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetProductAttributeList response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12 TJS 09/03/13
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetProductAttributeList response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetProductAttributeSetList() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get a list of the Product Attribute Sets from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function enabled
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 09/03/13 | TJS             | 2013.0.00 | Corrected error message procedure name
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPURLToUse As String ' TJS 13/11/13
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product_attribute_set.list</resourcePath>"
        strSOAP = strSOAP & "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, sSOAPURLToUse, False) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetProductAttributeSetList response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12 TJS 09/03/13
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetProductAttributeSetList response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Function GetCatalogProductListMaxID() As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get the maximum product id from the Magento product list 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/01/12 | TJS             | 2011.2.01 | Function added
        ' 14/02/12 | TJS             | 2011.2.05 | Extended timout and modified to return both full error details and just the message
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetMaxProductID</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetMaxProductID</resourcePath>"
        End If
        strSOAP = strSOAP & "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL, 90000) Then ' TJS 14/02/12
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                ' yes, this means we got some valid XML back, but maybe not the correct XML 
                XMLNameTabMagento = New System.Xml.NameTable
                XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
                XMLNSManMagento.AddNamespace("ns1", "urn:Magento")
                If m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item/item/value", XMLNSManMagento) IsNot Nothing Then
                    ' We have a max product ID, grab it
                    GetCatalogProductListMaxID = CInt(m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:callResponse/callReturn/item/item/value", XMLNSManMagento).Value)

                ElseIf m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) IsNot Nothing Then
                    If CheckCustomAPIWorking() Then
                        m_LastError = m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value
                        m_LastErrorMessage = m_LastError ' TJS 14/02/12
                    Else
                        m_LastError = "The Magento API returned an error - please check that the Lerryn API extension is properly installed"
                        m_LastErrorMessage = m_LastError ' TJS 14/02/12
                    End If
                    GetCatalogProductListMaxID = -1
                Else
                    m_LastErrorMessage = "Unknown GetCatalogProductListMaxID response from " & m_MagentoAPIURL ' TJS 14/02/12
                    m_LastError = m_LastErrorMessage & " - " & m_ReturnedXML.ToString.Replace(vbCrLf, "") ' TJS 14/02/12
                    GetCatalogProductListMaxID = -1
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCatalogProductListMaxID response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12
                GetCatalogProductListMaxID = -1

            End Try

        Else
            Return -1
        End If

    End Function

    Public Function GetCatalogProductListBatch(ByVal sStartProductID As Integer, ByVal sQuantityToReturn As Integer, ByVal bReturnDeleted As Boolean) As Boolean ' TJS 14/02/12
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get a batch of details from the Magento product list 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 12/01/12 | TJS             | 2011.2.01 | Function added
        ' 14/02/12 | TJS             | 2011.2.05 | Extended timout and modified to cater for variable return 
        '                                        | quantity and inhibiting of deleted record return
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento Custom API errors
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetOneProductDetailsByProductID</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetOneProductDetailsByProductID</resourcePath>"
        End If
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[3]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<value xsi:type=""xsd:int"">" & sStartProductID & "</value>"
        strSOAP = strSOAP & "<value xsi:type=""xsd:int"">" & sQuantityToReturn & "</value>" ' TJS 14/02/12
        If bReturnDeleted Then ' TJS 14/02/12
            strSOAP = strSOAP & "<value xsi:type=""xsd:string"">true</value>" ' TJS 14/02/12
        Else
            strSOAP = strSOAP & "<value xsi:type=""xsd:string"">false</value>" ' TJS 14/02/12
        End If
        strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL, 300000) Then ' TJS 14/02/12-
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then ' TJS 10/06/12
                    Return True
                Else
                    Return False ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCatalogProductListBatch response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetCatalogProductDetail(ByVal ProductID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get details of the specified Product from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:ns2=""http://xml.apache.org/xml-soap"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">catalog_product.info</resourcePath>"
        strSOAP = strSOAP & "<args xsi:type=""ns2:Map"">"
        strSOAP = strSOAP & "<item><key xsi:type=""xsd:string"">product_id</key>"
        strSOAP = strSOAP & "<value xsi:type=""xsd:int"">" & ProductID & "</value>"
        strSOAP = strSOAP & "</item></args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetCatalogProductDetail response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCatalogProductDetail response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetAllProductDetails_SOAP(ByVal ProductID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get all details of the specified Products from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/04/11 | TJS             | 2011.0.09 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Added check for custom API errors
        ' 02/11/11 | TJS             | 2011.1.09 | Modified to correct case of Magneto Lerryn API call for Unix based systems
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetAllProductDetails</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetAllProductDetails</resourcePath>" ' TJS 02/11/11
        End If
        strSOAP = strSOAP & "<args xsi:type=""xsd:string"">" & ProductID & "</args>"
        strSOAP = strSOAP & "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then ' TJS 25/04/11
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetAllProductDetails_SOAP response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetCatalogProductLinks(ByVal ProductID As String, ByVal LinkType As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get details of the specified Products from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">catalog_product_link.list</resourcePath>"
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:string[2]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & LinkType & "</item>"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & ProductID & "</item>"
        strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetCatalogProductLinks response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCatalogProductLinks response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetConfigurableProductAttributes(ByVal SKU As String, ByVal StoreID As String) As Boolean ' TJS 13/04/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get list of Attributes for a specific configurable product from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 01/04/11 | TJS             | 2011.0.06 | Function added
        ' 13/04/11 | TJs             | 2011.0.10 | Corrected api function name and changed ProductID to SKU
        ' 25/04/11 | TJS             | 2011.0.12 | Added check for custom API errors
        ' 02/11/11 | TJS             | 2011.1.09 | Modified to correct case of Magneto Lerryn API call for Unix based systems
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetConfigurableProductAttributes</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetConfigurableProductAttributes</resourcePath>" ' TJS 13/04/11 TJS 31/10/11
        End If
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[2]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & SKU & "</item>" ' TJS 13/04/11
        strSOAP = strSOAP & "<item xsi:type=""xsd:int"">" & StoreID & "</item>"
        strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then ' TJS 25/04/11
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetConfigurableProductAttributes response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetCatalogRelatedProducts(ByVal SKU As String) As Boolean ' TJS 13/04/11
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get list of Related Products for a specific configurable product from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 01/04/11 | TJS             | 2011.0.06 | Function added
        ' 13/04/11 | TJs             | 2011.0.10 | Changed ProductID to SKU
        ' 25/04/11 | TJS             | 2011.0.12 | Added check for custom API errors
        ' 02/11/11 | TJS             | 2011.1.09 | Modified to correct case of Magneto Lerryn API call for Unix based systems
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetRelatedProducts</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetRelatedProducts</resourcePath>" ' TJS 02/11/11
        End If
        strSOAP = strSOAP & "<args xsi:type=""xsd:string"">" & SKU & "</args>" ' TJS 13/04/11
        strSOAP = strSOAP & "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then ' TJS 25/04/11
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCatalogRelatedProducts response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetCatalogProductCustomOptions(ByVal ProductID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get details of the Custom Options of the specified Product from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 24/02/12 | TJS             | 2011.2.08 | Function added
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento Custom API errors
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP + "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP + "<sessionId xsi:type=""xsd:string"">" + m_SessionID + "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetProductCustomOptions</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP + "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetProductCustomOptions</resourcePath>"
        End If
        strSOAP = strSOAP + "<args SOAP-ENC:arrayType=""xsd:ur-type[1]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP + "<item xsi:type=""xsd:int"">" & ProductID & "</item>"
        strSOAP = strSOAP + "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then ' TJS 10/06/12
                    Return True
                Else
                    Return False ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCatalogProductCustomOptions response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetProductAttributes(ByVal ProductID As String, ByVal AttributeSetID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get the Attributes for the Product and its attribute set 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | Function added
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetOneProductAttributes</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetOneProductAttributes</resourcePath>"
        End If
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[2]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<value xsi:type=""xsd:int"">" & ProductID & "</value>"
        strSOAP = strSOAP & "<value xsi:type=""xsd:int"">" & AttributeSetID & "</value>"
        strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetProductAttributes response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetStoreList() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get list of Stores from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Added check for custom API errors
        ' 02/11/11 | TJS             | 2011.1.09 | Modified to correct case of Magneto Lerryn API call for Unix based systems
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetStoreInfo</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetStoreInfo</resourcePath>" ' TJS 02/11/11
        End If
        strSOAP = strSOAP & "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then ' TJS 25/04/11
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetStoreList response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetAttributeTable() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get the contents of the Attribute table from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/04/11 | TJS             | 2011.0.09 | Function added
        ' 25/04/11 | TJS             | 2011.0.12 | Added check for custom API errors
        ' 02/11/11 | TJS             | 2011.1.09 | Modified to correct case of Magneto Lerryn API call for Unix based systems
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2 
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 19/09/13 | TJS             | 2013.3.00 | Modified to cater for Lerryn API version 1.09
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        ' is API version at least 1.09 ?
        If m_APIVersion >= 1.09 Then ' TJS 19/09/13
            ' yes, use new API name etc
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetAttributeTable</resourcePath>" ' TJS 19/09/13
        Else
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_Ldtapi.ldtGetAttributeTable</resourcePath>" ' TJS 02/11/11
        End If
        strSOAP = strSOAP & "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckCustomAPIWorking() Then ' TJS 25/04/11
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetAttributeTable response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetSalesOrderList(ByVal CreatedSince As Date) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get list of Orders created since specified date/time from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 04/05/11 | TJS             | 2011.0.13 | Added timeout settings
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:ns2=""http://xml.apache.org/xml-soap"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">sales_order.list</resourcePath>"
        strSOAP = strSOAP + "<args SOAP-ENC:arrayType=""ns2:Map[1]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP + "<item xsi:type=""ns2:Map""><item><key xsi:type=""xsd:string"">created_at</key>"
        strSOAP = strSOAP + "<value xsi:type=""ns2:Map""><item><key xsi:type=""xsd:string"">from</key>"
        strSOAP = strSOAP + "<value xsi:type=""xsd:string"">" & CreatedSince.Year & "-" & Right("00" & CreatedSince.Month, 2)
        strSOAP = strSOAP & "-" & Right("00" & CreatedSince.Day, 2) & "T" & Right("00" & CreatedSince.Hour, 2) & ":"
        strSOAP = strSOAP & Right("00" & CreatedSince.Minute, 2) & ":" & Right("00" & CreatedSince.Second, 2) & "</value>"
        strSOAP = strSOAP + "</item></value></item></item></args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL, False, 300000) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetSalesOrderList response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetSalesOrderList response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetSalesOrderDetail(ByVal OrderID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get details of the specified Order from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">sales_order.info</resourcePath>"
        strSOAP = strSOAP & "<args xsi:type=""xsd:string"">" & OrderID & "</args>"
        strSOAP = strSOAP & "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetSalesOrderDetail response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetSalesOrderDetail response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetCustomerDetail(ByVal CustomerID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get details of the specified Customer from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">customer.info</resourcePath>"
        strSOAP = strSOAP & "<args xsi:type=""xsd:int"">" & CustomerID & "</args>"
        strSOAP = strSOAP & "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetCustomerDetail response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCustomerDetail response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetCustomerAddressList(ByVal CustomerID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Get list, and details, of Addresses for the specified customer from Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
        strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">customer_address.list</resourcePath>"
        strSOAP = strSOAP & "<args xsi:type=""xsd:int"">" & CustomerID & "</args>"
        strSOAP = strSOAP & "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "GetCustomerAddressList response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetCustomerAddressList response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function SalesOrderSetAsCancel(ByVal OrderIncrementID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Updates the status of a Sales Order in Magento to Cancel using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return SetSalesOrderAsHoldUnholdCancel(OrderIncrementID, "sales_order.cancel")

    End Function

    Public Function SalesOrderSetAsHold(ByVal OrderIncrementID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Updates the status of a Sales Order in Magento to Hold using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return SetSalesOrderAsHoldUnholdCancel(OrderIncrementID, "sales_order.hold")

    End Function

    Public Function SalesOrderSetAsUnheld(ByVal OrderIncrementID As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Updates the status of a Sales Order in Magento to UnHold using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Return SetSalesOrderAsHoldUnholdCancel(OrderIncrementID, "sales_order.unhold")

    End Function

    Private Function SetSalesOrderAsHoldUnholdCancel(ByVal OrderIncrementID As String, ByVal SOAPFunction As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Updates the status of a Sales Order in Magento to Hold, UnHold or Cancel using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">" & SOAPFunction & "</resourcePath>"
        strSOAP = strSOAP & "<args xsi:type=""xsd:string"">" & OrderIncrementID & "</args>"
        strSOAP = strSOAP & "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "SetSalesOrderAsHoldUnholdCancel response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "SetSalesOrderAsHoldUnholdCancel response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function AddCommentToSalesOrder(ByVal OrderIncrementID As String, ByVal OrderStatus As String, ByVal sOrderComment As String, ByVal bNotificationFlag As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Adds a comment to a Sales Order in Magento and also updates Status using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">sales_order_shipment.addComment</resourcePath>"
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[4]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & OrderIncrementID & "</item>"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & OrderStatus & "</item>"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & sOrderComment & "</item>"
        If bNotificationFlag Then
            strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">true</item>"
        Else
            strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">false</item>"
        End If
        strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "AddCommentToSalesOrder response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "AddCommentToSalesOrder response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function CreateSalesOrderShipment(ByVal OrderID As String, ByVal ItemsShipped As ShipmentInvoiceItems(), _
        ByVal ShipmentComment As String, ByVal SendEmail As Boolean, ByVal IncludeCommentInEmail As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Creates a Shipment for the specified Sales Order in Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 10/08/12 | TJS             | 2012.1.12 | Added missing code for shipment details
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String, sSOAPURLToUse As String ' TJS 13/11/13
        Dim iItemLoop As Integer, bCallingV2API As Boolean ' TJS 13/11/13

        ' start of code added TJS 13/11/13
        If V2SoapAPIWorks Then
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/")
            bCallingV2API = True
            If V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<salesOrderShipmentCreateRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<orderIncrementId xmlns="""">" & OrderID & "</orderIncrementId>"
                If ItemsShipped.Length > 0 Then
                    strSOAP = strSOAP & "<itemsQty xmlns="""">"
                    For iItemLoop = 0 To ItemsShipped.Length - 1
                        strSOAP = strSOAP & "<complexObjectArray>"
                        strSOAP = strSOAP & "<order_item_id>" & ItemsShipped(iItemLoop).OrderItemID & "</order_item_id>"
                        strSOAP = strSOAP & "<qty>" & ItemsShipped(iItemLoop).QuantityShipped & "</qty>"
                        strSOAP = strSOAP & "</complexObjectArray>"
                    Next
                    strSOAP = strSOAP & "</itemsQty>"
                End If
                strSOAP = strSOAP & "<comment xmlns="""">" & ShipmentComment & "</comment>"
                If SendEmail Then
                    strSOAP = strSOAP & "<email xmlns="""">1</email>"
                Else
                    strSOAP = strSOAP & "<email xmlns="""">0</email>"
                End If
                If IncludeCommentInEmail Then
                    strSOAP = strSOAP & "<includeComment xmlns="""">1</includeComment>"
                Else
                    strSOAP = strSOAP & "<includeComment xmlns="""">0</includeComment>"
                End If
                strSOAP = strSOAP & "</salesOrderShipmentCreateRequestParam>"
                strSOAP = strSOAP & "</s:Body></s:Envelope>"

            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:salesOrderShipmentCreate xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<orderIncrementId xsi:type=""xsd:string"">" & OrderID & "</orderIncrementId>"
                If ItemsShipped.Length > 0 Then
                    strSOAP = strSOAP & "<itemsQty href=""#id1""/>"
                End If
                strSOAP = strSOAP & "<comment xsi:type=""xsd:string"">" & ShipmentComment & "</comment>"""
                If SendEmail Then
                    strSOAP = strSOAP & "<email xsi:type=""xsd:string"">1</email>"
                Else
                    strSOAP = strSOAP & "<email xsi:type=""xsd:string"">0</email>"
                End If
                If IncludeCommentInEmail Then
                    strSOAP = strSOAP & "<includeComment xsi:type=""xsd:string"">1</includeComment>"
                Else
                    strSOAP = strSOAP & "<includeComment xsi:type=""xsd:string"">0</includeComment>"
                End If
                strSOAP = strSOAP & "</q1:salesOrderShipmentCreate>"
                If ItemsShipped.Length > 0 Then
                    strSOAP = strSOAP & "<q2:Array id=""id1"" q2:arrayType=""q3:orderItemIdQty[" & ItemsShipped.Length & "]"" "
                    strSOAP = strSOAP & "xmlns:q2=""http://schemas.xmlsoap.org/soap/encoding/"" "
                    strSOAP = strSOAP & "xmlns:q3=""urn:Magento"">"
                    For iItemLoop = 0 To ItemsShipped.Length - 1
                        strSOAP = strSOAP & "<Item href=""#id" & iItemLoop + 2 & """/>"
                    Next
                    strSOAP = strSOAP & "</q2:Array>"
                    For iItemLoop = 0 To ItemsShipped.Length - 1
                        strSOAP = strSOAP & "<q" & iItemLoop + 4 & ":orderItemIdQty id=""id" & iItemLoop + 2 & """ "
                        strSOAP = strSOAP & "xsi:type=""q" & iItemLoop + 4 & ":orderItemIdQty"" xmlns:q" & iItemLoop + 4 & "=""urn:Magento"">"
                        strSOAP = strSOAP & "<order_item_id xsi:type=""xsd:int"">" & ItemsShipped(iItemLoop).OrderItemID & "</order_item_id>"
                        strSOAP = strSOAP & "<qty xsi:type=""xsd:double"">" & ItemsShipped(iItemLoop).QuantityShipped & "</qty>"
                        strSOAP = strSOAP & "</q" & iItemLoop + 4 & ":orderItemIdQty>"
                    Next
                End If
                strSOAP = strSOAP & "</s:Body></s:Envelope>"
            End If
        Else
            sSOAPAction = "urn:Mage_Api_Model_Server_HandlerAction"
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
            bCallingV2API = False
            ' end of code added TJS 13/11/13
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
            strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
            strSOAP = strSOAP & "xmlns:ns2=""http://xml.apache.org/xml-soap"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" >"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">sales_order_shipment.create</resourcePath>"
            strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[5]"" xsi:type=""SOAP-ENC:Array"">"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & OrderID & "</item>"
            ' Here we build the array
            If ItemsShipped.Length > 0 Then  ' TJS 10/08/12 TJS 13/11/13
                strSOAP = strSOAP & "<item xsi:type=""ns2:Map"">" ' TJS 13/11/13
                For iItemLoop = 0 To ItemsShipped.Length - 1 ' TJS 13/11/13
                    strSOAP = strSOAP & "<item><key xsi:type=""xsd:int"">" & ItemsShipped(iItemLoop).OrderItemID ' TJS 13/11/13
                    strSOAP = strSOAP & "</key><value xsi:type=""xsd:string"">" & ItemsShipped(iItemLoop).QuantityShipped ' TJS 13/11/13
                    strSOAP = strSOAP & "</value></item>" ' TJS 13/11/13
                Next
                strSOAP = strSOAP & "</item>" ' TJS 13/11/13
            Else
                strSOAP = strSOAP & "<item SOAP-ENC:arrayType=""xsd:ur-type[0]"" xsi:type=""SOAP-ENC:Array"" />" ' TJS 10/08/12
            End If
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & ShipmentComment & "</item>"
            If SendEmail Then
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">true</item>"
            Else
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">false</item>"
            End If
            If IncludeCommentInEmail Then
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">true</item>"
            Else
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">false</item>"
            End If
            strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"
        End If

        If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse, bCallingV2API) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "CreateSalesOrderShipment response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "CreateSalesOrderShipment response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function AddTrackingNoToShipment(ByVal ShipmentID As String, ByVal CarrierCode As String, ByVal TrackingTitle As String, _
        ByVal TrackingNumber As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Adds a Tracking No. to the specified Magento Shipment
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String, sSOAPURLToUse As String ' TJS 13/11/13
        Dim bCallingV2API As Boolean ' TJS 13/11/13

        ' start of code added TJS 13/11/13
        If V2SoapAPIWorks Then
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/")
            bCallingV2API = True
            If V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<salesOrderShipmentAddTrackRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<shipmentIncrementId xmlns="""">" & ShipmentID & "</shipmentIncrementId>"
                strSOAP = strSOAP & "<carrier xmlns="""">" & CarrierCode & "</carrier>"
                strSOAP = strSOAP & "<title xmlns="""">" & TrackingTitle & "</title>"
                strSOAP = strSOAP & "<trackNumber xmlns="""">" & TrackingNumber & "</trackNumber>"
                strSOAP = strSOAP & "</salesOrderShipmentAddTrackRequestParam>"
                strSOAP = strSOAP & "</s:Body></s:Envelope>"

            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:salesOrderShipmentAddTrack xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<shipmentIncrementId xsi:type=""xsd:string"">" & ShipmentID & "</shipmentIncrementId>"
                strSOAP = strSOAP & "<carrier xsi:type=""xsd:string"">" & CarrierCode & "</carrier>"
                strSOAP = strSOAP & "<title xsi:type=""xsd:string"">" & TrackingTitle & "</title>"
                strSOAP = strSOAP & "<trackNumber xsi:type=""xsd:string"">" & TrackingNumber & "</trackNumber>"
                strSOAP = strSOAP & "</q1:salesOrderShipmentAddTrack></s:Body></s:Envelope>"
            End If
        Else
            sSOAPAction = "urn:Mage_Api_Model_Server_HandlerAction"
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
            bCallingV2API = False
            ' end of code added TJS 13/11/13
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">sales_order_shipment.addTrack</resourcePath>"
            strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:string[4]"" xsi:type=""SOAP-ENC:Array"">"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & ShipmentID & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & CarrierCode & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & TrackingTitle & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & TrackingNumber & "</item>"
            strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"
        End If

        If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse, bCallingV2API) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "AddTrackingNoToShipment response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "AddTrackingNoToShipment response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function CreateSalesOrderInvoice(ByVal OrderID As String, ByVal ItemsShipped As ShipmentInvoiceItems(), _
        ByVal InvoiceComment As String, ByVal SendEmail As Boolean, ByVal IncludeCommentInEmail As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Creates a Invoice for the specified Sales Order in Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 10/08/12 | TJS             | 2012.1.12 | Added missing code for shipment details
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String, sSOAPURLToUse As String ' TJS 13/11/13
        Dim bCallingV2API As Boolean ' TJS 13/11/13

        ' start of code added TJS 13/11/13
        If V2SoapAPIWorks Then
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/")
            bCallingV2API = True
            If V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<salesOrderInvoiceCreateRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<invoiceIncrementId xmlns="""">" & OrderID & "</invoiceIncrementId>"
                If ItemsShipped.Length > 0 Then
                    strSOAP = strSOAP & "<itemsQty xmlns="""">"
                    For iItemLoop = 0 To ItemsShipped.Length - 1
                        strSOAP = strSOAP & "<complexObjectArray>"
                        strSOAP = strSOAP & "<order_item_id>" & ItemsShipped(iItemLoop).OrderItemID & "</order_item_id>"
                        strSOAP = strSOAP & "<qty>" & ItemsShipped(iItemLoop).QuantityShipped & "</qty>"
                        strSOAP = strSOAP & "</complexObjectArray>"
                    Next
                    strSOAP = strSOAP & "</itemsQty>"
                End If
                strSOAP = strSOAP & "<comment xmlns="""">" & InvoiceComment & "</comment>"
                If SendEmail Then
                    strSOAP = strSOAP & "<email xmlns="""">1</email>"
                Else
                    strSOAP = strSOAP & "<email xmlns="""">0</email>"
                End If
                If IncludeCommentInEmail Then
                    strSOAP = strSOAP & "<includeComment xmlns="""">1</includeComment>"
                Else
                    strSOAP = strSOAP & "<includeComment xmlns="""">0</includeComment>"
                End If
                strSOAP = strSOAP & "</salesOrderInvoiceCreateRequestParam>"
                strSOAP = strSOAP & "</s:Body></s:Envelope>"

            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:salesOrderInvoiceCreate xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<invoiceIncrementId xsi:type=""xsd:string"">" & OrderID & "</invoiceIncrementId>"
                If ItemsShipped.Length > 0 Then
                    strSOAP = strSOAP & "<itemsQty href=""#id1""/>"
                End If
                strSOAP = strSOAP & "<comment xsi:type=""xsd:string"">" & InvoiceComment & "</comment>"""
                If SendEmail Then
                    strSOAP = strSOAP & "<email xsi:type=""xsd:string"">1</email>"
                Else
                    strSOAP = strSOAP & "<email xsi:type=""xsd:string"">0</email>"
                End If
                If IncludeCommentInEmail Then
                    strSOAP = strSOAP & "<includeComment xsi:type=""xsd:string"">1</includeComment>"
                Else
                    strSOAP = strSOAP & "<includeComment xsi:type=""xsd:string"">0</includeComment>"
                End If
                strSOAP = strSOAP & "</q1:salesOrderInvoiceCreate>"
                If ItemsShipped.Length > 0 Then
                    strSOAP = strSOAP & "<q2:Array id=""id1"" q2:arrayType=""q3:orderItemIdQty[2]"" "
                    strSOAP = strSOAP & "xmlns:q2=""http://schemas.xmlsoap.org/soap/encoding/"" "
                    strSOAP = strSOAP & "xmlns:q3=""urn:Magento"">"
                    For iItemLoop = 0 To ItemsShipped.Length - 1
                        strSOAP = strSOAP & "<Item href=""#id" & iItemLoop + 2 & """/>"
                    Next
                    strSOAP = strSOAP & "</q2:Array>"
                    For iItemLoop = 0 To ItemsShipped.Length - 1
                        strSOAP = strSOAP & "<q" & iItemLoop + 4 & ":orderItemIdQty id=""id" & iItemLoop + 2 & """ "
                        strSOAP = strSOAP & "xsi:type=""q" & iItemLoop + 4 & ":orderItemIdQty"" xmlns:q" & iItemLoop + 4 & "=""urn:Magento"">"
                        strSOAP = strSOAP & "<order_item_id xsi:type=""xsd:int"">" & ItemsShipped(iItemLoop).OrderItemID & "</order_item_id>"
                        strSOAP = strSOAP & "<qty xsi:type=""xsd:double"">" & ItemsShipped(iItemLoop).QuantityShipped & "</qty>"
                        strSOAP = strSOAP & "</q" & iItemLoop + 4 & ":orderItemIdQty>"
                    Next
                End If
                strSOAP = strSOAP & "</s:Body></s:Envelope>"
            End If
        Else
            sSOAPAction = "urn:Mage_Api_Model_Server_HandlerAction"
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
            bCallingV2API = False
            ' end of code added TJS 13/11/13
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
            strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
            strSOAP = strSOAP & "xmlns:ns2=""http://xml.apache.org/xml-soap"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" >"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">sales_order_invoice.create</resourcePath>"
            strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[5]"" xsi:type=""SOAP-ENC:Array"">"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & OrderID & "</item>"
            ' Here we build the array
            If ItemsShipped.Length > 0 Then  ' TJS 10/08/12 TJS 13/11/13
                strSOAP = strSOAP & "<item xsi:type=""ns2:Map"">" ' TJS 13/11/13
                For iItemLoop = 0 To ItemsShipped.Length - 1 ' TJS 13/11/13
                    strSOAP = strSOAP & "<item><key xsi:type=""xsd:int"">" & ItemsShipped(iItemLoop).OrderItemID ' TJS 13/11/13
                    strSOAP = strSOAP & "</key><value xsi:type=""xsd:string"">" & ItemsShipped(iItemLoop).QuantityShipped ' TJS 13/11/13
                    strSOAP = strSOAP & "</value></item>" ' TJS 13/11/13
                Next
                strSOAP = strSOAP & "</item>" ' TJS 13/11/13
            Else
                strSOAP = strSOAP & "<item SOAP-ENC:arrayType=""xsd:ur-type[0]"" xsi:type=""SOAP-ENC:Array"" />" ' TJS 10/08/12
            End If
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & InvoiceComment & "</item>"
            If SendEmail Then
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">true</item>"
            Else
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">false</item>"
            End If
            If IncludeCommentInEmail Then
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">true</item>"
            Else
                strSOAP = strSOAP & "<item xsi:type=""xsd:boolean"">false</item>"
            End If
            strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"
        End If

        If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse, bCallingV2API) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "CreateSalesOrderInvoice response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "CreateSalesOrderInvoice response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function CreateCatalogProduct(ByVal SKU As String, ByVal ProductType As String, ByVal AttributeSet As String, _
        ByRef ProductAttributes As ProductAttributeType(), ByRef ProductCategories As Integer(), ByRef WebsiteIDs As Integer()) As Boolean ' TJS 22/08/12 TJS 13/11/13
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Create a new Product with the specified Attributes in Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 04/05/11 | TJS             | 2011.0.13 | Corrected XML
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 22/08/12 | TJS             | 2012.1.14 | Modified to cater for Magento product Categories 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API and to set Store View on V1 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ProductAttributeToUse As ProductAttributeType = Nothing ' TJS 13/11/13
        Dim strSOAP As String, sSOAPAction As String, sSOAPURLToUse As String ' TJS 13/11/13
        Dim iLoop As Integer, bCallingV2API As Boolean ' TJS 13/11/13

        ' start of code added TJS 13/11/13
        If V2SoapAPIWorks Then
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/")
            bCallingV2API = True
            If V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<catalogProductCreateRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<type xmlns="""">" & ProductType & "</type>"
                strSOAP = strSOAP & "<set xmlns="""">" & AttributeSet & "</set>"
                ' need to mark sku as used
                GetProductAttributeFromArray(ProductAttributes, "sku", ProductAttributeToUse)
                strSOAP = strSOAP & "<sku xmlns="""">" & SKU & "</sku>"
                strSOAP = strSOAP & catalogProductCreateEntity(True, ProductAttributes, ProductCategories, WebsiteIDs)
                strSOAP = strSOAP & "<store xmlns="""">0</store>"
                strSOAP = strSOAP & "</catalogProductCreateRequestParam></s:Body></s:Envelope>"
            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:catalogProductCreate xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<type xsi:type=""xsd:string"">" & ProductType & "</type>"
                strSOAP = strSOAP & "<set xsi:type=""xsd:string"">" & AttributeSet & "</set>"
                ' need to mark sku as used
                GetProductAttributeFromArray(ProductAttributes, "sku", ProductAttributeToUse)
                strSOAP = strSOAP & "<sku xsi:type=""xsd:string"">" & SKU & "</sku>"
                strSOAP = strSOAP & "<productData href=""#id1""/>"
                strSOAP = strSOAP & "<storeView xsi:type=""xsd:string"">default</storeView>"
                strSOAP = strSOAP & "</q1:catalogProductCreate>"
                ' product attributes
                strSOAP = strSOAP & catalogProductCreateEntity(False, ProductAttributes, ProductCategories, WebsiteIDs)
                strSOAP = strSOAP & "</s:Body></s:Envelope>"
            End If
        Else
            sSOAPAction = "urn:Mage_Api_Model_Server_HandlerAction"
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
            bCallingV2API = False
            ' end of code added TJS 13/11/13
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:ns2=""http://xml.apache.org/xml-soap"" "
            strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product.create</resourcePath>"
            strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[4]"" xsi:type=""SOAP-ENC:Array"">"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & ProductType & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & AttributeSet & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & SKU & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""ns2:Map"">"

            For Each ProductAttribute As ProductAttributeType In ProductAttributes
                ' has value been provided for attribute ?
                If ProductAttribute.AttributeValue <> "" Then
                    ' yes, add the attribute name
                    strSOAP = strSOAP & "<item><key xsi:type=""xsd:string"">" & ProductAttribute.AttributeName & "</key>" ' TJS 04/05/11
                    ' Now the attribute value based upon the data type
                    Select Case ProductAttribute.AttributeType
                        Case "Integer"
                            ' value will already have been formatted as required by Magento
                            strSOAP = strSOAP & "<value xsi:type=""xsd:int"">" & ProductAttribute.AttributeValue & "</value>"
                        Case "Numeric"
                            ' value will already have been formatted as required by Magento
                            strSOAP = strSOAP & "<value xsi:type=""xsd:float"">" & ProductAttribute.AttributeValue & "</value>"
                        Case "Date", "DateTime"
                            ' value will already have been formatted as required by Magento
                            strSOAP = strSOAP & "<value xsi:type=""xsd:date"">" & ProductAttribute.AttributeValue & "</value>"
                        Case Else
                            ' value will already have been formatted as required by Magento
                            strSOAP = strSOAP & "<value xsi:type=""xsd:string"">" & ProductAttribute.AttributeValue & "</value>"
                    End Select
                    ' Now end the XML for the attribute
                    strSOAP = strSOAP & "</item>"
                End If
            Next
            strSOAP = strSOAP & "</item>"
            ' start of code added TJS 22/08/12
            If ProductCategories.Length >= 0 Then
                strSOAP = strSOAP & "<key xsi:type=""xsd:string"">categories</key>"""
                strSOAP = strSOAP & "<value SOAP-ENC:arrayType=""xsd:int[" & ProductCategories.Length - 1 & "]"" xsi:type=""SOAP-ENC:Array"">"""
                For iLoop = 0 To ProductCategories.Length - 1
                    strSOAP = strSOAP & "<item xsi:type=""xsd:int"">" & ProductCategories(iLoop).ToString & "</item>"
                Next
                strSOAP = strSOAP & "</value>"
            End If
            ' end of code added TJS 22/08/12
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">1</item>" ' TJS 13/11/13
            strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"
        End If

        If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse, bCallingV2API) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "CreateCatalogProduct response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "CreateCatalogProduct response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Private Function GetProductAttributeFromArray(ByRef ProductAttributes As ProductAttributeType(), ByVal AttributeName As String, _
        ByRef ProductAttributeToUse As ProductAttributeType) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iloop As Integer

        ProductAttributeToUse = Nothing
        For iloop = 0 To ProductAttributes.Length - 1
            If ProductAttributes(iloop).AttributeName = AttributeName Then
                ' clear name to indicate attribute has been used
                ProductAttributes(iloop).AttributeName = ""
                ProductAttributeToUse = ProductAttributes(iloop)
                Return True
            End If
        Next
        Return False

    End Function

    Public Function LinkConfigWithSimpleProducts(ByVal ConfigProductID As String, ByVal SimpleProductIDs As String()) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        If m_APIVersion >= 1.1 Then
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsAssignSimplesToConfigurable</resourcePath>"
            strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[2]"" xsi:type=""SOAP-ENC:Array"">"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & ConfigProductID & "</item>"
            strSOAP = strSOAP & "<item SOAP-ENC:arrayType=""xsd:int[" & SimpleProductIDs.Length - 1 & "]"" xsi:type=""SOAP-ENC:Array"">"""
            For iLoopCount = 0 To SimpleProductIDs.Length - 1
                strSOAP = strSOAP & "<item xsi:type=""xsd:int"">" & SimpleProductIDs(iLoopCount) & "</item>"
            Next
            strSOAP = strSOAP & "</item>"
            strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

            If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                ' does it start correctly ?
                If m_ReturnedData.Length > 19 Then
                    If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                        ' yes, try loading into XML document
                        Try
                            m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                            If CheckCustomAPIWorking() Then
                                Return True
                            Else
                                m_LastErrorMessage = "LinkConfigWithSimpleProducts response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                                m_LastError = m_LastErrorMessage
                            End If

                        Catch ex As Exception
                            m_LastErrorMessage = "LinkConfigWithSimpleProducts response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                            m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")
                            m_LoggedIn = False

                        End Try

                    End If
                End If
            End If
            Return False
        Else
            m_LastErrorMessage = "Lerryn API extension in version " & m_APIVersion & " does not support lbsAssignSimplesToConfigurable function - Please install the latest Lerryn Magento API extension"
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function AddRelatedProductLink(ByVal ProductID As String, ByRef LinkDetails As ProductLinkDetails) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String

        If V2SoapAPIWorks Then
            If m_V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<catalogProductLinkAssignRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<type xmlns="""">" & LinkDetails.LinkType & "</type>"
                strSOAP = strSOAP & "<productId xmlns="""">" & ProductID & "</productId>"
                strSOAP = strSOAP & "<linkedProductId xmlns="""">" & LinkDetails.LinkProductID & "</linkedProductId>"
                strSOAP = strSOAP & "<data xmlns="""">"
                strSOAP = strSOAP & "<product_id>" & LinkDetails.LinkProductID & "</product_id>"
                strSOAP = strSOAP & "<type>" & LinkDetails.LinkType & "</type>"
                strSOAP = strSOAP & "<set>" & LinkDetails.AttributeSet & "</set>"
                strSOAP = strSOAP & "<sku>" & LinkDetails.LinkProductSKU & "</sku>"
                strSOAP = strSOAP & "<position>" & LinkDetails.PositionInList & "</position>"
                strSOAP = strSOAP & "<qty>" & LinkDetails.LinkProductQty & "</qty>"
                strSOAP = strSOAP & "</data>"
                strSOAP = strSOAP & "<identifierType xmlns="""">id</identifierType>"
                strSOAP = strSOAP & "</catalogProductLinkAssignRequestParam></s:Body></s:Envelope>"

            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:catalogProductLinkAssign xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<type xsi:type=""xsd:string"">" & LinkDetails.LinkType & "</type>"
                strSOAP = strSOAP & "<product xsi:type=""xsd:string"">" & ProductID & "</product>"
                strSOAP = strSOAP & "<linkedProduct xsi:type=""xsd:string"">" & LinkDetails.LinkProductID & "</linkedProduct>"
                strSOAP = strSOAP & "<data href=""#id1""/>"
                strSOAP = strSOAP & "<identifierType xsi:type=""xsd:string"">id</identifierType>"
                strSOAP = strSOAP & "</q1:catalogProductLinkAssign>"
                strSOAP = strSOAP & "<q2:catalogProductLinkEntity id=""id1"" xsi:type=""q2:catalogProductLinkEntity"" "
                strSOAP = strSOAP & "xmlns:q2=""urn:Magento"">"
                strSOAP = strSOAP & "<product_id xsi:type=""xsd:string"">" & LinkDetails.LinkProductID & "</product_id>"
                strSOAP = strSOAP & "<type xsi:type=""xsd:string"">" & LinkDetails.LinkType & "</type>"
                strSOAP = strSOAP & "<set xsi:type=""xsd:string"">" & LinkDetails.AttributeSet & "</set>"
                strSOAP = strSOAP & "<sku xsi:type=""xsd:string"">" & LinkDetails.LinkProductSKU & "</sku>"
                strSOAP = strSOAP & "<position xsi:type=""xsd:string"">" & LinkDetails.PositionInList & "</position>"
                strSOAP = strSOAP & "<qty xsi:type=""xsd:string"">" & LinkDetails.LinkProductQty & "</qty>"
                strSOAP = strSOAP & "</q2:catalogProductLinkEntity></s:Body></s:Envelope>"

            End If
            If PostSOAP(strSOAP, sSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/"), True) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                Try
                    m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                    If CheckNoErrors() Then
                        Return True
                    Else
                        m_LastErrorMessage = "AddRelatedProductLink response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                        m_LastError = m_LastErrorMessage
                    End If

                Catch ex As Exception
                    m_ReturnedXML = Nothing
                    m_LastErrorMessage = "AddRelatedProductLink response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                    m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

                End Try

                Return False
            Else
                Return False
            End If
        Else
            m_ReturnedXML = Nothing
            m_LastErrorMessage = "AddRelatedProductLink requires the Magento V2 SOAP API which is not working on " & m_MagentoAPIURL
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function UpdateCatalogProductDetail(ByVal ProductID As String, ByRef ProductAttributes As ProductAttributeType(), _
        ByRef ProductCategories As Integer(), ByRef WebsiteIDs As Integer()) As Boolean ' TJS 22/08/12
        '
        '   Description -   Updates the specified Attributes in Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 22/08/12 | TJS             | 2012.1.14 | Modified to cater for Magento product Categories 
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API and to set Store View on V1 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String, sSOAPURLToUse As String ' TJS 13/11/13
        Dim iLoop As Integer, bCallingV2API As Boolean ' TJS 13/11/13

        ' start of code added TJS 13/11/13
        If V2SoapAPIWorks Then
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/")
            bCallingV2API = True
            If V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<catalogProductUpdateRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<productId xmlns="""">" & ProductID & "</productId>"
                strSOAP = strSOAP & catalogProductCreateEntity(True, ProductAttributes, ProductCategories, WebsiteIDs)
                strSOAP = strSOAP & "<store xmlns="""">0</store>"
                strSOAP = strSOAP & "<identifierType xmlns=""""/>"
                strSOAP = strSOAP & "</catalogProductUpdateRequestParam></s:Body></s:Envelope>"
            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:catalogProductUpdate xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<product xsi:type=""xsd:string"">" & ProductID & "</product>"
                strSOAP = strSOAP & "<productData href=""#id1""/>"
                strSOAP = strSOAP & "<storeView xsi:type=""xsd:string"">0</storeView>"
                strSOAP = strSOAP & "<identifierType xsi:type=""xsd:string""/>"
                strSOAP = strSOAP & "</q1:catalogProductUpdate>"
                strSOAP = strSOAP & catalogProductCreateEntity(False, ProductAttributes, ProductCategories, WebsiteIDs)
                strSOAP = strSOAP & "</s:Body></s:Envelope>"
            End If
        Else
            sSOAPAction = "urn:Mage_Api_Model_Server_HandlerAction"
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
            bCallingV2API = False
            ' end of code added TJS 13/11/13
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:ns2=""http://xml.apache.org/xml-soap"" "
            strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product.update</resourcePath>"
            strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[2]"" xsi:type=""SOAP-ENC:Array"">"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & ProductID & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""ns2:Map"">"
            For iLoop = 0 To ProductAttributes.Length - 1
                If ProductAttributes(iLoop).AttributeValue <> "" Then
                    strSOAP = strSOAP & "<item>"
                    strSOAP = strSOAP & "<key xsi:type=""xsd:string"">" & ProductAttributes(iLoop).AttributeName & "</key>"
                    strSOAP = strSOAP & "<value xsi:type=""xsd:string"">" & ProductAttributes(iLoop).AttributeValue & "</value>"
                    strSOAP = strSOAP & "</item>"
                End If
            Next
            strSOAP = strSOAP & "</item>"
            ' start of code added TJS 22/08/12
            If ProductCategories.Length >= 0 Then
                strSOAP = strSOAP & "<key xsi:type=""xsd:string"">categories</key>"""
                strSOAP = strSOAP & "<value SOAP-ENC:arrayType=""xsd:int[" & ProductCategories.Length - 1 & "]"" xsi:type=""SOAP-ENC:Array"">"""
                For iLoop = 0 To ProductCategories.Length - 1
                    strSOAP = strSOAP & "<item xsi:type=""xsd:int"">" & ProductCategories(iLoop).ToString & "</item>"
                Next
                strSOAP = strSOAP & "</value>"
            End If
            ' end of code added TJS 22/08/12
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">1</item>" ' TJS 13/11/13
            strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"
        End If

        If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse, bCallingV2API) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "UpdateCatalogProductDetail response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "UpdateCatalogProductDetail response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Private Function catalogProductCreateEntity(ByVal WSICompliant As Boolean, ByRef ProductAttributes As ProductAttributeType(), _
        ByRef ProductCategories As Integer(), ByRef WebsiteIDs As Integer()) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Create the V2 SOAP XML for the 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        ' 15/01/14 | TJS             | 2013.4.05 | Modified to detect and handle AttributeValue being nothing
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim ProductAttributeToUse As ProductAttributeType = Nothing
        Dim strSOAP As String, iAddlAttributeCount As Integer, iLoop As Integer
        Dim iAttributePtr As Integer

        If WSICompliant Then
            strSOAP = "<productData xmlns="""">"
            If GetProductAttributeFromArray(ProductAttributes, "name", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<name>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</name>"
            Else
                strSOAP = strSOAP & "<name/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "description", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<description>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</description>"
            Else
                strSOAP = strSOAP & "<description/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "short_description", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<short_description>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</short_description>"
            Else
                strSOAP = strSOAP & "<short_description/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "weight", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<weight>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</weight>"
            Else
                strSOAP = strSOAP & "<weight/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "status", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<status>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</status>"
            Else
                strSOAP = strSOAP & "<status/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "url_key", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<url_key>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</url_key>"
            Else
                strSOAP = strSOAP & "<url_key/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "url_path", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<url_path>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</url_path>"
            Else
                strSOAP = strSOAP & "<url_path/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "visibility", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<visibility>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</visibility>"
            Else
                strSOAP = strSOAP & "<visibility/>"
            End If
            If ProductCategories.Length > 0 Then
                strSOAP = strSOAP & "<category_ids>"
                For iLoop = 0 To ProductCategories.Length - 1
                    strSOAP = strSOAP & "<complexObjectArray>" & ProductCategories(iLoop).ToString & "</complexObjectArray>"
                Next
                strSOAP = strSOAP & "</category_ids>"
            End If
            If WebsiteIDs.Length > 0 Then
                strSOAP = strSOAP & "<website_ids>"
                For iLoop = 0 To WebsiteIDs.Length - 1
                    strSOAP = strSOAP & "<complexObjectArray>" & WebsiteIDs(iLoop).ToString & "</complexObjectArray>"
                Next
                strSOAP = strSOAP & "</website_ids>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "has_options", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<has_options>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</has_options>"
            Else
                strSOAP = strSOAP & "<has_options/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "gift_message_available", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<gift_message_available>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</gift_message_available>"
            Else
                strSOAP = strSOAP & "<gift_message_available/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "price", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<price>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</price>"
            Else
                strSOAP = strSOAP & "<price/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "special_price", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<special_price>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</special_price>"
            Else
                strSOAP = strSOAP & "<special_price/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "special_from_date", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<special_from_date>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</special_from_date>"
            Else
                strSOAP = strSOAP & "<special_from_date/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "special_to_date", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<special_to_date>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</special_to_date>"
            Else
                strSOAP = strSOAP & "<special_to_date/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "tax_class_id", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<tax_class_id>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</tax_class_id>"
            Else
                strSOAP = strSOAP & "<tax_class_id/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "meta_title", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<meta_title>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</meta_title>"
            Else
                strSOAP = strSOAP & "<meta_title/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "meta_keyword", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<meta_keyword>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</meta_keyword>"
            Else
                strSOAP = strSOAP & "<meta_keyword/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "meta_description", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<meta_description>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</meta_description>"
            Else
                strSOAP = strSOAP & "<meta_description/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "custom_design", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<custom_design>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</custom_design>"
            Else
                strSOAP = strSOAP & "<custom_design/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "custom_layout_update", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<custom_layout_update>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</custom_layout_update>"
            Else
                strSOAP = strSOAP & "<custom_layout_update/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "options_container", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<options_container>" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</options_container>"
            Else
                strSOAP = strSOAP & "<options_container/>"
            End If
            strSOAP = strSOAP & "<additional_attributes>"
            For Each ProductAttribute As ProductAttributeType In ProductAttributes
                ' has value been provided for attribute and is name present (names are cleared if attribute was used above?
                If Not String.IsNullOrEmpty(ProductAttribute.AttributeValue) AndAlso Not String.IsNullOrEmpty(ProductAttribute.AttributeName) Then ' TJS 15/01/14
                    ' yes, add the attribute name
                    strSOAP = strSOAP & "<complexObjectArray><key>" & ConvertStringForXML(ProductAttribute.AttributeName) & "</key>"
                    ' and value
                    strSOAP = strSOAP & "<value>" & ConvertStringForXML(ProductAttribute.AttributeValue) & "</value></complexObjectArray>"
                End If
            Next
            strSOAP = strSOAP & "</additional_attributes>"
            strSOAP = strSOAP & "<stock_data/>"
            strSOAP = strSOAP & "</productData>"

        Else
            strSOAP = "<q2:catalogProductCreateEntity id=""id1"" xsi:type=""q2:catalogProductCreateEntity"" xmlns:q2=""urn:Magento"">"
            If GetProductAttributeFromArray(ProductAttributes, "name", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<name xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</name>"
            Else
                strSOAP = strSOAP & "<name xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "description", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<description xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</description>"
            Else
                strSOAP = strSOAP & "<description xsi:type=""xsd:string""></description>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "short_description", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<short_description xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</short_description>"
            Else
                strSOAP = strSOAP & "<short_description xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "weight", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<weight xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</weight>"
            Else
                strSOAP = strSOAP & "<weight xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "status", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<status xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</status>"
            Else
                strSOAP = strSOAP & "<status xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "url_key", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<url_key xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</url_key>"
            Else
                strSOAP = strSOAP & "<url_key xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "url_path", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<url_path xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</url_path>"
            Else
                strSOAP = strSOAP & "<url_path xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "visibility", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<visibility xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</visibility>"
            Else
                strSOAP = strSOAP & "<visibility xsi:type=""xsd:string""/>"
            End If
            If ProductCategories.Length >= 0 Then
                strSOAP = strSOAP & "<category_ids href=""#id2""/>"
            End If
            If WebsiteIDs.Length > 0 Then
                strSOAP = strSOAP & "<website_ids href=""#id3""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "has_options", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<has_options xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</has_options>"
            Else
                strSOAP = strSOAP & "<has_options xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "gift_message_available", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<gift_message_available xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</gift_message_available>"
            Else
                strSOAP = strSOAP & "<gift_message_available xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "price", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<price xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</price>"
            Else
                strSOAP = strSOAP & "<price xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "special_price", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<special_price xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</special_price>"
            Else
                strSOAP = strSOAP & "<special_price xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "special_from_date", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<special_from_date xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</special_from_date>"
            Else
                strSOAP = strSOAP & "<special_from_date xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "special_to_date", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<special_to_date xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</special_to_date>"
            Else
                strSOAP = strSOAP & "<special_to_date xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "tax_class_id", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<tax_class_id xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</tax_class_id>"
            Else
                strSOAP = strSOAP & "<tax_class_id xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "meta_title", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<meta_title xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</meta_title>"
            Else
                strSOAP = strSOAP & "<meta_title xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "meta_keyword", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<meta_keyword xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</meta_keyword>"
            Else
                strSOAP = strSOAP & "<meta_keyword xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "meta_description", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<meta_description xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</meta_description>"
            Else
                strSOAP = strSOAP & "<meta_description xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "custom_design", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<custom_design xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</custom_design>"
            Else
                strSOAP = strSOAP & "<custom_design xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "custom_layout_update", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<custom_layout_update xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</custom_layout_update>"
            Else
                strSOAP = strSOAP & "<custom_layout_update xsi:type=""xsd:string""/>"
            End If
            If GetProductAttributeFromArray(ProductAttributes, "options_container", ProductAttributeToUse) AndAlso Not String.IsNullOrEmpty(ProductAttributeToUse.AttributeValue) Then ' TJS 15/01/14
                strSOAP = strSOAP & "<options_container xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributeToUse.AttributeValue) & "</options_container>"
            Else
                strSOAP = strSOAP & "<options_container xsi:type=""xsd:string""/>"
            End If
            ' additional_attributes array ref 1
            strSOAP = strSOAP & "<additional_attributes href=""#id4""/>"
            ' Inv
            strSOAP = strSOAP & "<stock_data href=""#id5""/>"
            strSOAP = strSOAP & "</q2:catalogProductCreateEntity>"
            ' Category IDs
            If ProductCategories.Length >= 0 Then
                strSOAP = strSOAP & "<q3:Array id=""id2"" q3:arrayType=""xsd:string[" & ProductCategories.Length
                strSOAP = strSOAP & "]"" xmlns:q3=""http://schemas.xmlsoap.org/soap/encoding/"">"
                For iLoop = 0 To ProductCategories.Length - 1
                    strSOAP = strSOAP & "<Item>" & ProductCategories(iLoop).ToString & "</Item>"
                Next
                strSOAP = strSOAP & "</q3:Array>"
            End If
            ' website IDs
            If WebsiteIDs.Length > 0 Then
                strSOAP = strSOAP & "<q4:Array id=""id3"" q4:arrayType=""xsd:string[" & WebsiteIDs.Length
                strSOAP = strSOAP & "]"" xmlns:q4=""http://schemas.xmlsoap.org/soap/encoding/"">"
                For iLoop = 0 To WebsiteIDs.Length - 1
                    strSOAP = strSOAP & "<Item>" & WebsiteIDs(iLoop).ToString & "</Item>"
                Next
                strSOAP = strSOAP & "</q4:Array>"
            End If
            ' additional attributes array
            strSOAP = strSOAP & "<q5:catalogProductAdditionalAttributesEntity id=""id4"" xsi:type=""q5:catalogProductAdditionalAttributesEntity"" xmlns:q5=""urn:Magento"">"
            strSOAP = strSOAP & "<single_data href=""#id6""/>"
            strSOAP = strSOAP & "</q5:catalogProductAdditionalAttributesEntity>"
            ' get count of additional attributes to include
            iAddlAttributeCount = 0
            For iLoop = 0 To ProductAttributes.Length - 1
                ' has value been provided for attribute and is name present (names are cleared if attribute was used above?
                If Not String.IsNullOrEmpty(ProductAttributes(iLoop).AttributeName) And Not String.IsNullOrEmpty(ProductAttributes(iLoop).AttributeValue) Then ' TJS 15/01/14
                    ' no, need to include in additional attributes
                    iAddlAttributeCount += 1
                End If
            Next
            ' Inventory Stock Mgmt
            strSOAP = strSOAP & "<q6:catalogInventoryStockItemUpdateEntity id=""id5"" xsi:type=""q6:catalogInventoryStockItemUpdateEntity"" xmlns:q6=""urn:Magento""/>"
            ' additional attributes
            If iAddlAttributeCount > 0 Then
                strSOAP = strSOAP & "<q7:Array id=""id6"" q7:arrayType=""q8:associativeEntity[" & iAddlAttributeCount & "]"" xmlns:q7=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:q8=""urn:Magento"">"
                For iLoop = 0 To iAddlAttributeCount - 1
                    strSOAP = strSOAP & "<Item href=""#id" & iLoop + 7 & """/>"
                Next
                strSOAP = strSOAP & "</q7:Array>"
                iAttributePtr = 0
                For iLoop = 0 To ProductAttributes.Length - 1
                    ' has value been provided for attribute and is name present (names are cleared if attribute was used above?
                    If Not String.IsNullOrEmpty(ProductAttributes(iLoop).AttributeName) And Not String.IsNullOrEmpty(ProductAttributes(iLoop).AttributeValue) Then ' TJS 15/01/14
                        ' no, need to include in additional attributes
                        strSOAP = strSOAP & "<q" & iAttributePtr + 9 & ":associativeEntity id=""id" & iAttributePtr + 7 & """ xsi:type=""q"
                        strSOAP = strSOAP & iAttributePtr + 9 & ":associativeEntity"" xmlns:q" & iAttributePtr + 9 & "=""urn:Magento"">"
                        strSOAP = strSOAP & "<key xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributes(iLoop).AttributeName) & "</key>"
                        strSOAP = strSOAP & "<value xsi:type=""xsd:string"">" & ConvertStringForXML(ProductAttributes(iLoop).AttributeValue) & "</value>"
                        strSOAP = strSOAP & "</q" & iAttributePtr + 9 & ":associativeEntity>"
                        iAttributePtr += 1
                    End If
                Next
            End If
        End If
        Return strSOAP

    End Function

    Public Function UpdateCatalogProductStockQty(ByVal ProductID As String, ByVal Quantity As Decimal, ByVal InStock As Boolean) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Updates the Product Stock quantity in Magento using SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/12/11 | TJS             | 2011.2.00 | Function added
        ' 14/02/12 | TJS             | 2011.2.05 | Modified to return both full error details and just the message
        ' 10/06/12 | TJS             | 2012.1.05 | Modified to check for Magento errors
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:ns2=""http://xml.apache.org/xml-soap"" "
        strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product_stock.update</resourcePath>"
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[2]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & ProductID & "</item>"
        strSOAP = strSOAP & "<item xsi:type=""ns2:Map""><item><key xsi:type=""xsd:string"">qty</key>"
        strSOAP = strSOAP & "<value xsi:type=""xsd:int"">" & Quantity & "</value></item>"
        strSOAP = strSOAP & "<item><key xsi:type=""xsd:string"">is_in_stock</key>"
        If InStock Then
            strSOAP = strSOAP & "<value xsi:type=""xsd:int"">1</value></item>"
        Else
            strSOAP = strSOAP & "<value xsi:type=""xsd:int"">0</value></item>"
        End If
        strSOAP = strSOAP & "</item></args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then ' TJS 10/06/12
                    Return True
                Else
                    m_LastErrorMessage = "UpdateCatalogProductStockQty response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage ' TJS 10/06/12
                    m_LastError = m_LastErrorMessage ' TJS 10/06/12
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "UpdateCatalogProductStockQty response from " & m_MagentoAPIURL & " could not be processed due to XML error" ' TJS 14/02/12
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "") ' TJS 14/02/12

            End Try

            Return False
        Else
            Return False
        End If


    End Function

    Public Function CreateProductAttributeSet(ByVal AttributeSetName As String, ByVal SkeletonID As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/10/13 | TJS             | 2013.3.03 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        If CheckMagentoVersionGreaterThan("1.5.0.0") Then
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product_attribute_set.create</resourcePath>"
            strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[2]"" xsi:type=""SOAP-ENC:Array"">"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & AttributeSetName & "</item>"
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & SkeletonID.ToString & "</item>"
            strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

            If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
                ' Feed the SOAP response into the XMLResponse object for processing
                Try
                    m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                    If CheckNoErrors() Then
                        Return True
                    Else
                        m_LastErrorMessage = "CreateProductAttributeSet response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                        m_LastError = m_LastErrorMessage
                    End If

                Catch ex As Exception
                    m_ReturnedXML = Nothing
                    m_LastErrorMessage = "CreateProductAttributeSet response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                    m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

                End Try

                Return False
            Else
                Return False
            End If
        Else
            m_LastErrorMessage = "Magento API in version " & m_MagentoVersion & " does not support product_attribute_set.create function" ' TJS 13/11/13
            m_LastError = m_LastErrorMessage
        End If

    End Function

    Public Function CreateProductAttribute(ByRef AttributeParams As ProductAttributeType) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String

        If V2SoapAPIWorks Then
            If m_V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<catalogProductAttributeCreateRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<data xmlns="""">"
                strSOAP = strSOAP & "<attribute_code>" & ConvertStringForXML(AttributeParams.AttributeName) & "</attribute_code>"
                strSOAP = strSOAP & "<frontend_input>" & AttributeParams.AttributeType & "</frontend_input>"
                strSOAP = strSOAP & "<scope>" & AttributeParams.AttributeScope & "</scope>"
                strSOAP = strSOAP & "<default_value></default_value>"
                strSOAP = strSOAP & "<is_unique>0</is_unique>"
                strSOAP = strSOAP & "<is_required>0</is_required>"
                strSOAP = strSOAP & "<apply_to>"
                strSOAP = strSOAP & "<complexObjectArray>simple</complexObjectArray>"
                strSOAP = strSOAP & "</apply_to>"
                If AttributeParams.UsedForConfigurables Then
                    strSOAP = strSOAP & "<is_configurable>1</is_configurable>"
                Else
                    strSOAP = strSOAP & "<is_configurable>0</is_configurable>"
                End If
                strSOAP = strSOAP & "<is_searchable>1</is_searchable>"
                strSOAP = strSOAP & "<is_visible_in_advanced_search>0</is_visible_in_advanced_search>"
                strSOAP = strSOAP & "<is_comparable>0</is_comparable>"
                strSOAP = strSOAP & "<is_used_for_promo_rules>0</is_used_for_promo_rules>"
                strSOAP = strSOAP & "<is_visible_on_front>1</is_visible_on_front>"
                strSOAP = strSOAP & "<used_in_product_listing>1</used_in_product_listing>"
                strSOAP = strSOAP & "<additional_fields/>"
                strSOAP = strSOAP & "<frontend_label><complexObjectArray>"
                strSOAP = strSOAP & "<store_id>0</store_id>"
                strSOAP = strSOAP & "<label>" & ConvertStringForXML(AttributeParams.AttributeDescription) & "</label>"
                strSOAP = strSOAP & "</complexObjectArray></frontend_label>"
                strSOAP = strSOAP & "</data>"
                strSOAP = strSOAP & "</catalogProductAttributeCreateRequestParam>"
                strSOAP = strSOAP & "</s:Body></s:Envelope>"

            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:catalogProductAttributeCreate xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<data href=""#id1""/>"
                strSOAP = strSOAP & "</q1:catalogProductAttributeCreate>"
                strSOAP = strSOAP & "<q2:catalogProductAttributeEntityToCreate id=""id1"" "
                strSOAP = strSOAP & "xsi:type=""q2:catalogProductAttributeEntityToCreate"" xmlns:q2=""urn:Magento"">"
                strSOAP = strSOAP & "<attribute_code xsi:type=""xsd:string"">" & ConvertStringForXML(AttributeParams.AttributeName) & "</attribute_code>"
                strSOAP = strSOAP & "<frontend_input xsi:type=""xsd:string"">" & AttributeParams.AttributeType & "</frontend_input>"
                strSOAP = strSOAP & "<scope xsi:type=""xsd:string"">" & AttributeParams.AttributeScope & "</scope>"
                strSOAP = strSOAP & "<default_value xsi:type=""xsd:string""></default_value>"
                strSOAP = strSOAP & "<is_unique xsi:type=""xsd:int"">0</is_unique>"
                strSOAP = strSOAP & "<is_required xsi:type=""xsd:int"">0</is_required>"
                strSOAP = strSOAP & "<apply_to href=""#id2""/>"
                If AttributeParams.UsedForConfigurables Then
                    strSOAP = strSOAP & "<is_configurable xsi:type=""xsd:int"">1</is_configurable>"
                Else
                    strSOAP = strSOAP & "<is_configurable xsi:type=""xsd:int"">0</is_configurable>"
                End If
                strSOAP = strSOAP & "<is_searchable xsi:type=""xsd:int"">1</is_searchable>"
                strSOAP = strSOAP & "<is_visible_in_advanced_search xsi:type=""xsd:int"">0</is_visible_in_advanced_search>"
                strSOAP = strSOAP & "<is_comparable xsi:type=""xsd:int"">0</is_comparable>"
                strSOAP = strSOAP & "<is_used_for_promo_rules xsi:type=""xsd:int"">0</is_used_for_promo_rules>"
                strSOAP = strSOAP & "<is_visible_on_front xsi:type=""xsd:int"">1</is_visible_on_front>"
                strSOAP = strSOAP & "<used_in_product_listing xsi:type=""xsd:int"">1</used_in_product_listing>"
                strSOAP = strSOAP & "<additional_fields href=""#id3""/>"
                strSOAP = strSOAP & "<frontend_label href=""#id4""/>"
                strSOAP = strSOAP & "</q2:catalogProductAttributeEntityToCreate>"
                strSOAP = strSOAP & "<q3:Array id=""id2"" q3:arrayType=""xsd:string[1]"" "
                strSOAP = strSOAP & "xmlns:q3=""http://schemas.xmlsoap.org/soap/encoding/"">"
                strSOAP = strSOAP & "<Item>simple</Item></q3:Array>"
                strSOAP = strSOAP & "<q4:Array id=""id3"" q4:arrayType=""q5:associativeEntity[0]"" "
                strSOAP = strSOAP & "xmlns:q4=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:q5=""urn:Magento""/>"
                strSOAP = strSOAP & "<q6:Array id=""id4"" q6:arrayType=""q7:catalogProductAttributeFrontendLabelEntity[1]"" "
                strSOAP = strSOAP & "xmlns:q6=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:q7=""urn:Magento"">"
                strSOAP = strSOAP & "<Item href=""#id5""/></q6:Array>"
                strSOAP = strSOAP & "<q8:catalogProductAttributeFrontendLabelEntity id=""id5"" "
                strSOAP = strSOAP & "xsi:type=""q8:catalogProductAttributeFrontendLabelEntity"" xmlns:q8=""urn:Magento"">"
                strSOAP = strSOAP & "<store_id xsi:type=""xsd:string"">0</store_id>"
                strSOAP = strSOAP & "<label xsi:type=""xsd:string"">" & ConvertStringForXML(AttributeParams.AttributeDescription) & "</label>"
                strSOAP = strSOAP & "</q8:catalogProductAttributeFrontendLabelEntity>"
                strSOAP = strSOAP & "</s:Body></s:Envelope>"
            End If
            If PostSOAP(strSOAP, sSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/"), True) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                Try
                    m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                    If CheckNoErrors() Then
                        Return True
                    Else
                        m_LastErrorMessage = "CreateProductAttribute response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                        m_LastError = m_LastErrorMessage
                    End If

                Catch ex As Exception
                    m_ReturnedXML = Nothing
                    m_LastErrorMessage = "CreateProductAttribute response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                    m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

                End Try

                Return False
            Else
                Return False
            End If
        Else
            m_ReturnedXML = Nothing
            m_LastErrorMessage = "CreateProductAttribute requires the Magento V2 SOAP API which is not working on " & m_MagentoAPIURL
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function AddOptionValueToProductAttribute(ByVal AttributeID As Integer, ByVal AttributeValue As String, ByVal PositionInList As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String

        If V2SoapAPIWorks Then
            If m_V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<catalogProductAttributeAddOptionRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<attribute xmlns="""">" & AttributeID & "</attribute>"
                strSOAP = strSOAP & "<data xmlns=""""><label><complexObjectArray>"
                strSOAP = strSOAP & "<store_id><complexObjectArray>0</complexObjectArray></store_id>"
                strSOAP = strSOAP & "<value>" & ConvertStringForXML(AttributeValue) & "</value>"
                strSOAP = strSOAP & "</complexObjectArray></label>"
                strSOAP = strSOAP & "<order>" & PositionInList & "</order>"
                strSOAP = strSOAP & "<is_default>0</is_default>"
                strSOAP = strSOAP & "</data>"
                strSOAP = strSOAP & "</catalogProductAttributeAddOptionRequestParam></s:Body></s:Envelope>"

            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<q1:catalogProductAttributeAddOption xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<attribute xsi:type=""xsd:string"">" & AttributeID & "</attribute>"
                strSOAP = strSOAP & "<data href=""#id1""/></q1:catalogProductAttributeAddOption>"
                strSOAP = strSOAP & "<q2:catalogProductAttributeOptionEntityToAdd id=""id1"" "
                strSOAP = strSOAP & "xsi:type=""q2:catalogProductAttributeOptionEntityToAdd"" "
                strSOAP = strSOAP & "xmlns:q2=""urn:Magento""><label href=""#id2""/>"
                strSOAP = strSOAP & "<order xsi:type=""xsd:int"">" & PositionInList & "</order>"
                strSOAP = strSOAP & "<is_default xsi:type=""xsd:int"">0</is_default>"
                strSOAP = strSOAP & "</q2:catalogProductAttributeOptionEntityToAdd>"
                strSOAP = strSOAP & "<q3:Array id=""id2"" q3:arrayType=""q4:catalogProductAttributeOptionLabelEntity[1]"" "
                strSOAP = strSOAP & "xmlns:q3=""http://schemas.xmlsoap.org/soap/encoding/"" xmlns:q4=""urn:Magento"">"
                strSOAP = strSOAP & "<Item href=""#id3""/></q3:Array>"
                strSOAP = strSOAP & "<q5:catalogProductAttributeOptionLabelEntity id=""id3"" "
                strSOAP = strSOAP & "xsi:type=""q5:catalogProductAttributeOptionLabelEntity"" xmlns:q5=""urn:Magento"">"
                strSOAP = strSOAP & "<store_id href=""#id4""/><value xsi:type=""xsd:string"">" & ConvertStringForXML(AttributeValue) & "</value>"
                strSOAP = strSOAP & "</q5:catalogProductAttributeOptionLabelEntity>"
                strSOAP = strSOAP & "<q6:Array id=""id4"" q6:arrayType=""xsd:string[1]"" "
                strSOAP = strSOAP & "xmlns:q6=""http://schemas.xmlsoap.org/soap/encoding/"">"
                strSOAP = strSOAP & "<Item>0</Item></q6:Array>"
                strSOAP = strSOAP & "</s:Body></s:Envelope>"

            End If
            If PostSOAP(strSOAP, sSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/"), True) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                Try
                    m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                    If CheckNoErrors() Then
                        Return True
                    Else
                        m_LastErrorMessage = "AddOptionValueToProductAttribute response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                        m_LastError = m_LastErrorMessage
                    End If

                Catch ex As Exception
                    m_ReturnedXML = Nothing
                    m_LastErrorMessage = "AddOptionValueToProductAttribute response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                    m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

                End Try

                Return False
            Else
                Return False
            End If
        Else
            m_ReturnedXML = Nothing
            m_LastErrorMessage = "AddOptionValueToProductAttribute requires the Magento V2 SOAP API which is not working on " & m_MagentoAPIURL
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function AddGroupToProductAttributeSet(ByVal AttributeSetID As Integer, ByVal GroupName As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/10/13 | TJS             | 2013.3.03 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product_attribute_set.groupAdd</resourcePath>"
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[2]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & AttributeSetID.ToString & "</item>"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & GroupName & "</item>"
        strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then
                    Return True
                Else
                    m_LastErrorMessage = "AddGroupToProductAttributeSet response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                    m_LastError = m_LastErrorMessage
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "AddGroupToProductAttributeSet response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function AddAttributeToProductAttributeSet(ByVal AttributeSetID As Integer, ByVal AttributeID As Integer, _
        Optional ByVal AttributeGroupID As Integer = -1, Optional ByVal SortOrder As Integer = -1) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 02/10/13 | TJS             | 2013.3.03 | Function added
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, iParamCount As Integer
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
        strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
        strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
        strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
        strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:call>"
        strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
        strSOAP = strSOAP & "<resourcePath xsi:type=""xsd:string"">product_attribute_set.attributeAdd</resourcePath>"
        If SortOrder >= 0 Then
            iParamCount = 4
        ElseIf AttributeGroupID >= 0 Then
            iParamCount = 3
        Else
            iParamCount = 2
        End If
        strSOAP = strSOAP & "<args SOAP-ENC:arrayType=""xsd:ur-type[" & iParamCount & "]"" xsi:type=""SOAP-ENC:Array"">"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & AttributeID.ToString & "</item>"
        strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & AttributeSetID.ToString & "</item>"
        If iParamCount >= 3 Then
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & AttributeGroupID.ToString & "</item>"
        End If
        If iParamCount = 4 Then
            strSOAP = strSOAP & "<item xsi:type=""xsd:string"">" & SortOrder.ToString & "</item>"
        End If
        strSOAP = strSOAP & "</args></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

        If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            Try
                m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                If CheckNoErrors() Then
                    Return True
                Else
                    m_LastErrorMessage = "AddGroupToProductAttributeSet response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                    m_LastError = m_LastErrorMessage
                End If

            Catch ex As Exception
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "AddGroupToProductAttributeSet response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

            End Try

            Return False
        Else
            Return False
        End If

    End Function

    Public Function GetProductAttributeOptions(ByVal AttributeID As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, cSOAPAction As String

        If V2SoapAPIWorks Then
            If m_V2SoapAPIWSICompliant Then
                cSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP & "<catalogProductAttributeOptionsRequestParam xmlns=""urn:Magento"">"
                strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                strSOAP = strSOAP & "<attributeId xmlns="""">" & AttributeID & "</attributeId>"
                strSOAP = strSOAP & "<store xmlns="""">default</store>"
                strSOAP = strSOAP & "</catalogProductAttributeOptionsRequestParam>"
                strSOAP = strSOAP & "</s:Body></s:Envelope>"
            Else
                cSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = "<q1:catalogProductAttributeOptions xmlns:q1=""urn:Magento"">"
                strSOAP = "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                strSOAP = "<attributeId xsi:type=""xsd:string"">" & AttributeID & "</attributeId>"
                strSOAP = "<storeView xsi:type=""xsd:string"">default</storeView>"
                strSOAP = "</q1:catalogProductAttributeOptions></s:Body></s:Envelope>"
            End If
            If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/"), True) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                Try
                    m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                    If CheckNoErrors() Then
                        Return True
                    Else
                        m_LastErrorMessage = "GetProductAttributeOptions response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                        m_LastError = m_LastErrorMessage
                    End If

                Catch ex As Exception
                    m_ReturnedXML = Nothing
                    m_LastErrorMessage = "GetProductAttributeOptions response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                    m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

                End Try

                Return False
            Else
                Return False
            End If
        Else
            m_ReturnedXML = Nothing
            m_LastErrorMessage = "GetProductAttributeOptions requires the Magento V2 SOAP API which is not working on " & m_MagentoAPIURL
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function GetAttributeList(ByVal ModifiedAfter As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Return a list of all attributes with an option to only get those modified after a specified date/time
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        If m_APIVersion >= 1.1 Then
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP + "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP + "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP + "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsCatalogProductAttributeList</resourcePath>"
            If ModifiedAfter <> "" Then
                strSOAP = strSOAP & "<args xsi:type=""xsd:string"">" & ModifiedAfter & "</args>"
            Else
                strSOAP = strSOAP + "<args xsi:nil=""true""/>"
            End If
            strSOAP = strSOAP + "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

            If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                ' does it start correctly ?
                If m_ReturnedData.Length > 19 Then
                    If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                        ' yes, try loading into XML document
                        Try
                            m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                            If CheckCustomAPIWorking() Then
                                Return True
                            Else
                                m_LastErrorMessage = "GetAttributeList response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                                m_LastError = m_LastErrorMessage
                            End If

                        Catch ex As Exception
                            m_LastErrorMessage = "GetAttributeList response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                            m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")
                            m_LoggedIn = False

                        End Try

                    End If
                End If
            End If
            Return False
        Else
            m_LastErrorMessage = "Lerryn API extension in version " & m_APIVersion & " does not support lbsCatalogProductAttributeList function - Please install the latest Lerryn Magento API extension"
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function GetAllAttributeValues(ByVal ModifiedAfter As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Return a list of all attributes values with an option to only get those modified after a specified date/time
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        If m_APIVersion >= 1.1 Then
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP + "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP + "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP + "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsCatalogProductAttributeInfoList</resourcePath>"
            If ModifiedAfter <> "" Then
                strSOAP = strSOAP & "<args xsi:type=""xsd:string"">" & ModifiedAfter & "</args>"
            Else
                strSOAP = strSOAP + "<args xsi:nil=""true""/>"
            End If
            strSOAP = strSOAP + "</ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

            If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                ' does it start correctly ?
                If m_ReturnedData.Length > 19 Then
                    If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                        ' yes, try loading into XML document
                        Try
                            m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                            If CheckCustomAPIWorking() Then
                                Return True
                            Else
                                m_LastErrorMessage = "GetAllAttributeValues response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                                m_LastError = m_LastErrorMessage
                            End If

                        Catch ex As Exception
                            m_LastErrorMessage = "GetAllAttributeValues response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                            m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")
                            m_LoggedIn = False

                        End Try

                    End If
                End If
            End If
            Return False
        Else
            m_LastErrorMessage = "Lerryn API extension in version " & m_APIVersion & " does not support lbsCatalogProductAttributeInfoList function - Please install the latest Lerryn Magento API extension"
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function GetSingleAttributeValues(ByVal AttributeID As Integer) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Return a list of all attributes values for the specified Attribute ID
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String, sSOAPAction As String, sSOAPURLToUse As String
        Dim bCallingV2API As Boolean

        If CheckMagentoVersionGreaterThan("1.8.0.0") Then
            If V2SoapAPIWorks Then
                sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/")
                bCallingV2API = True
                If V2SoapAPIWSICompliant Then
                    sSOAPAction = ""
                    strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                    strSOAP = strSOAP & "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                    strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                    strSOAP = strSOAP & "<catalogProductAttributeInfoRequestParam xmlns=""urn:Magento"">"
                    strSOAP = strSOAP & "<sessionId xmlns="""">" & m_SessionID & "</sessionId>"
                    strSOAP = strSOAP & "<attribute xmlns="""">" & AttributeID & "</attribute>"
                    strSOAP = strSOAP & "</catalogProductAttributeInfoRequestParam></s:Body></s:Envelope>"
                Else
                    sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                    strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                    strSOAP = strSOAP & "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                    strSOAP = strSOAP & "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                    strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                    strSOAP = strSOAP & "<q1:catalogProductAttributeInfo xmlns:q1=""urn:Magento"">"
                    strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
                    strSOAP = strSOAP & "<attribute xsi:type=""xsd:string"">" & AttributeID & "</attribute>"
                    strSOAP = strSOAP & "</q1:catalogProductAttributeInfo></s:Body></s:Envelope>"
                End If
                If PostSOAP(strSOAP, sSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/"), True) Then
                    ' Feed the SOAP response into the XMLResponse object for processing
                    Try
                        m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                        If CheckNoErrors() Then
                            Return True
                        Else
                            m_LastErrorMessage = "GetSingleAttributeValues response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                            m_LastError = m_LastErrorMessage
                        End If

                    Catch ex As Exception
                        m_ReturnedXML = Nothing
                        m_LastErrorMessage = "GetSingleAttributeValues response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                        m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")

                    End Try

                    Return False
                Else
                    Return False
                End If

            Else
                m_ReturnedXML = Nothing
                m_LastErrorMessage = "GetSingleAttributeValues requires the Magento V2 SOAP API which is not working on " & m_MagentoAPIURL
                m_LastError = m_LastErrorMessage
                Return False
            End If
        Else

        End If

    End Function

    Public Function GetAttributeTimestamp() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Return the latest timestamp value from the lerryn_lbsapi_attributesave_log table
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strSOAP As String
        Const cSOAPAction As String = "urn:Mage_Api_Model_Server_HandlerAction"

        If m_APIVersion >= 1.1 Then
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""urn:Magento"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP + "<SOAP-ENV:Body><ns1:call>"
            strSOAP = strSOAP + "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP + "<resourcePath xsi:type=""xsd:string"">Lerryn_LBSapi.lbsGetAttributeTimestamp</resourcePath>"
            strSOAP = strSOAP + "<args xsi:nil=""true""/></ns1:call></SOAP-ENV:Body></SOAP-ENV:Envelope>"

            If PostSOAP(strSOAP, cSOAPAction, m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")) Then
                ' Feed the SOAP response into the XMLResponse object for processing
                ' does it start correctly ?
                If m_ReturnedData.Length > 19 Then
                    If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                        ' yes, try loading into XML document
                        Try
                            m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                            If CheckCustomAPIWorking() Then
                                Return True
                            Else
                                m_LastErrorMessage = "GetAttributeTimestamp response from " & m_MagentoAPIURL & " contained error - " & m_LastErrorMessage
                                m_LastError = m_LastErrorMessage
                            End If

                        Catch ex As Exception
                            m_LastErrorMessage = "GetAttributeTimestamp response from " & m_MagentoAPIURL & " could not be processed due to XML error"
                            m_LastError = m_LastErrorMessage & " - " & ex.Message.Replace(vbCrLf, "")
                            m_LoggedIn = False

                        End Try

                    End If
                End If
            End If
            Return False
        Else
            m_LastErrorMessage = "Lerryn API extension in version " & m_APIVersion & " does not support lbsGetAttributeTimestamp function - Please install the latest Lerryn Magento API extension"
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Public Function Logout() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Login out of Magento SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 13/11/13 | TJS             | 2013.3.08 | Modified to cater for Magento V2 SOAP API
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable
        Dim strSOAP As String, sSOAPAction As String, sSOAPResponseXPath As String ' TJS 13/11/13
        Dim sSOAPURLToUse As String ' TJS 13/11/13
        Dim bReturnValue As Boolean, bCallingV2API As Boolean ' TJS 13/11/13

        ' start of code added TJS 13/11/13
        If V2SoapAPIWorks Then
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/soap/", "/api/v2_soap/")
            bCallingV2API = True
            If m_V2SoapAPIWSICompliant Then
                sSOAPAction = ""
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP + "<s:Body xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP + "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP + "<endSessionParam xmlns=""urn:Magento""/>"
                strSOAP = strSOAP + "</s:Body></s:Envelope>"
                sSOAPResponseXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:endSessionResponseParam/result"
            Else
                sSOAPAction = "urn:Mage_Api_Model_Server_V2_HandlerAction"
                strSOAP = "<s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"">"
                strSOAP = strSOAP + "<s:Body s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"" "
                strSOAP = strSOAP + "xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
                strSOAP = strSOAP + "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">"
                strSOAP = strSOAP + "<q1:endSession xmlns:q1=""urn:Magento"">"
                strSOAP = strSOAP + "<sessionId xsi:type=""xsd:string"">" + m_SessionID + "</sessionId>"
                strSOAP = strSOAP + "</q1:endSession></s:Body></s:Envelope>"
                sSOAPResponseXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:endSessionResponse/endSessionReturn"
            End If
        Else
            bCallingV2API = False
            sSOAPAction = "urn:Mage_Api_Model_Server_HandlerAction"
            sSOAPURLToUse = m_MagentoAPIURL.ToLower.Replace("/api/v2_soap/", "/api/soap/")
            ' end of code added TJS 13/11/13
            strSOAP = "<?xml version=""1.0"" encoding=""UTF-8""?>"
            strSOAP = strSOAP & "<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" "
            strSOAP = strSOAP & "xmlns:ns1=""urn:Magento"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" "
            strSOAP = strSOAP & "xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/"" "
            strSOAP = strSOAP & "SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">"
            strSOAP = strSOAP & "<SOAP-ENV:Body><ns1:endSession>"
            strSOAP = strSOAP & "<sessionId xsi:type=""xsd:string"">" & m_SessionID & "</sessionId>"
            strSOAP = strSOAP & "</ns1:endSession></SOAP-ENV:Body></SOAP-ENV:Envelope>"
            sSOAPResponseXPath = "SOAP-ENV:Envelope/SOAP-ENV:Body/ns1:endSessionResponse/endSessionReturn" ' TJS 13/11/13
        End If

        bReturnValue = False
        If PostSOAP(strSOAP, sSOAPAction, sSOAPURLToUse, bCallingV2API) Then ' TJS 13/11/13
            ' Feed the SOAP response into the XMLResponse object for processing
            ' does it start correctly ?
            If m_ReturnedData.Length > 19 Then
                If m_ReturnedData.Trim.Substring(0, 19).ToLower = "<?xml version=""1.0""" Then
                    ' yes, try loading into XML document
                    Try
                        m_ReturnedXML = XDocument.Parse(m_ReturnedData)
                        ' yes, this means we got some valid XML back, but maybe not the correct XML 
                        XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
                        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
                        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
                        XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11
                        If m_ReturnedXML.XPathSelectElement(sSOAPResponseXPath, XMLNSManMagento) IsNot Nothing Then ' TJS 13/11/13
                            ' we have a response, was it confirmation of log out ?
                            If m_ReturnedXML.XPathSelectElement(sSOAPResponseXPath, XMLNSManMagento).Value.ToUpper = "TRUE" Then ' TJS 13/11/13
                                ' yes, confirmed log out
                                m_LoggedIn = False
                                bReturnValue = True

                            Else
                                ' have to assume we are no longer logged in even though we didn't get expected response
                                m_LoggedIn = False

                            End If
                        Else
                            ' have to assume we are no longer logged in even though we didn't get expected response
                            m_LoggedIn = False

                        End If

                    Catch ex As Exception
                        ' have to assume we are no longer logged in even though we didn't get expected response
                        m_LoggedIn = False

                    End Try
                Else
                    ' have to assume we are no longer logged in even though we didn't get expected response
                    m_LoggedIn = False
                End If
            Else
                ' have to assume we are no longer logged in even though we didn't get expected response
                m_LoggedIn = False
            End If
        Else
            ' have to assume we are no longer logged in even though we didn't get expected response
            m_LoggedIn = False
        End If
        Return bReturnValue

    End Function

    Public Sub ExtractMagentoCategories(ByVal XMLCategoryList As System.Collections.Generic.IEnumerable(Of XElement), _
        ByRef MagentoCategories As Lerryn.Facade.ImportExport.MagentoSOAPConnector.CategoryType(), _
        ByRef MagentoCategoryCount As Integer, ByRef Cancel As Boolean)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Login out of Magento SOAP API
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 29/03/11 | TJS             | 2011.0.04 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLItemList As System.Collections.Generic.IEnumerable(Of XElement), XMLCategoryNode As XElement
        Dim XMLChildList As System.Collections.Generic.IEnumerable(Of XElement), XMLItemNode As XElement
        Dim iCategoryCount As Integer, bIsActive As Boolean, bHasChildNodes As Boolean

        iCategoryCount = 0
        For Each XmlElement As XElement In XMLCategoryList
            iCategoryCount += 1
        Next

        If MagentoCategories(0).CategoryID > 0 Then
            ReDim Preserve MagentoCategories(MagentoCategories.Length + (iCategoryCount - 1))
        Else
            ReDim Preserve MagentoCategories((MagentoCategories.Length - 1) + (iCategoryCount - 1))
        End If
        For Each XMLCategoryNode In XMLCategoryList
            XMLItemList = XMLCategoryNode.XPathSelectElements("item")
            bIsActive = True
            bHasChildNodes = False
            XMLChildList = Nothing
            For Each XMLItemNode In XMLItemList
                Select Case XMLItemNode.XPathSelectElement("key").Value.ToLower
                    Case "category_id"
                        MagentoCategories(MagentoCategoryCount).CategoryID = CInt(XMLItemNode.XPathSelectElement("value").Value)

                    Case "parent_id"
                        MagentoCategories(MagentoCategoryCount).ParentID = CInt(XMLItemNode.XPathSelectElement("value").Value)

                    Case "name"
                        MagentoCategories(MagentoCategoryCount).CategoryName = XMLItemNode.XPathSelectElement("value").Value

                    Case "is_active"
                        If XMLItemNode.XPathSelectElement("value").Value = "0" Then
                            bIsActive = False
                        End If

                    Case "position"
                        MagentoCategories(MagentoCategoryCount).Position = CInt(XMLItemNode.XPathSelectElement("value").Value)

                    Case "level"
                        MagentoCategories(MagentoCategoryCount).Level = CInt(XMLItemNode.XPathSelectElement("value").Value)

                    Case "children"
                        bHasChildNodes = True
                        XMLChildList = XMLItemNode.XPathSelectElements("value/item")

                End Select
                If Cancel Then
                    Exit For
                End If
            Next
            If bIsActive And MagentoCategories(MagentoCategoryCount).Level > 0 Then
                MagentoCategoryCount = MagentoCategoryCount + 1
            End If
            If bHasChildNodes Then
                ExtractMagentoCategories(XMLChildList, MagentoCategories, MagentoCategoryCount, Cancel)
            End If
            If Cancel Then
                Exit For
            End If
        Next

    End Sub

    Private Function CheckNoErrors() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Checks returned XML for standard Magento error messages 
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 10/06/12 | TJS             | 2012.1.05 | Function added
        ' 15/03/13 | TJS             | 2013.1.10 | Added check to detect session expired errors
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable

        XMLNameTabMagento = New System.Xml.NameTable
        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento)
        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/")
        XMLNSManMagento.AddNamespace("ns1", "urn:Magento")

        If m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) Is Nothing Then
            Return True
        Else
            If m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value = "Session expired. Try to relogin." Then ' TJS 15/04/13
                m_LoggedIn = False ' TJS 15/04/13
                m_SessionID = "" ' TJS 15/04/13
            End If
            m_LastErrorMessage = m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value
            m_LastError = m_LastErrorMessage
            Return False
        End If

    End Function

    Private Function CheckCustomAPIWorking() As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   Checks returned XML for Invalid API PAth messages which normally 
        '                   indicate our custom API is not installed
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 25/04/11 | TJS             | 2011.0.12 | Function added
        ' 04/05/11 | TJS             | 2011.0.13 | corrected logic
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ' 03/04/13 | TJS             | 2013.1.08 | Modified to check for Magento errors and return an error message 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLNSManMagento As System.Xml.XmlNamespaceManager, XMLNameTabMagento As System.Xml.NameTable

        XMLNameTabMagento = New System.Xml.NameTable ' TJS 02/12/11
        XMLNSManMagento = New System.Xml.XmlNamespaceManager(XMLNameTabMagento) ' TJS 02/12/11
        XMLNSManMagento.AddNamespace("SOAP-ENV", "http://schemas.xmlsoap.org/soap/envelope/") ' TJS 02/12/11
        XMLNSManMagento.AddNamespace("ns1", "urn:Magento") ' TJS 02/12/11

        If m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento) Is Nothing Then ' TJS 04/05/11
            Return True
        ElseIf m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value.Length < 16 OrElse _
            m_ReturnedXML.XPathSelectElement("SOAP-ENV:Envelope/SOAP-ENV:Body/SOAP-ENV:Fault/faultstring", XMLNSManMagento).Value.Substring(0, 16).ToLower <> "invalid api path" Then ' TJS 04/05/11
            Return CheckNoErrors() ' TJS 03/04/13
        Else
            m_LastError = "The Magento API returned an error - please check that the Lerryn API extension is properly installed" ' TJS 03/04/13
            m_LastErrorMessage = m_LastError ' TJS 03/04/13
            Return False
        End If

    End Function

    Public Function CheckMagentoVersionGreaterThan(ByVal MinumumVersion As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | function added
        ' 04/01/14 | TJS             | 2013.4.03 | Function made public so magento version can be checked in the service
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strMagentoVersionElements As String(), strMinimumVersionElements As String()
        Dim iLoop As Integer, bVersionEqual As Boolean

        bVersionEqual = False
        strMagentoVersionElements = m_MagentoVersion.Split(CChar("."))
        strMinimumVersionElements = MinumumVersion.Split(CChar("."))
        For iLoop = 0 To strMagentoVersionElements.Length - 1
            If iLoop < strMinimumVersionElements.Length Then
                If CInt(strMagentoVersionElements(iLoop)) > CInt(strMinimumVersionElements(iLoop)) Then
                    Return True
                ElseIf CInt(strMagentoVersionElements(iLoop)) = CInt(strMinimumVersionElements(iLoop)) Then
                    bVersionEqual = True
                Else
                    Return False
                End If
            End If
        Next
        Return bVersionEqual

    End Function

    Public Function ConvertStringForXML(ByVal StringToConvert As String) As String
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 13/11/13 | TJS             | 2013.3.08 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim sTemp As String

        sTemp = StringToConvert.Replace("&", "&amp;")
        sTemp = sTemp.Replace("<", "&lt;")
        sTemp = sTemp.Replace(">", "&gt;")
        sTemp = sTemp.Replace("""", "&quot;")
        sTemp = sTemp.Replace("'", "&apos;")
        sTemp = sTemp.Replace(vbCr, "&#xD;")
        Return sTemp

    End Function

    Public Sub New()
        ' www.dynenttech.com davidoelson 5/4/2018
        ' Default was SSL3 or TLS, we need to allow TLS11 and TLS12 also, some sites are going to TLS12 only these day
        ' We need to find a more generic place to put this code os all connectors are converted, but I didn't have to to figure that out now
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or SecurityProtocolType.Tls Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls12
    End Sub
End Class
