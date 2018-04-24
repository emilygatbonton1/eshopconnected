' eShopCONNECT for Connected Business - Windows Service
' Module: ASPStorefrontConnector.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Updated 02 December 2011

Imports Microsoft.VisualBasic
Imports System.IO ' TJS 02/12/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 02/12/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Public Class ASPStorefrontConnector

    Public Structure CategoryList ' TJS 29/03/11
        Public ISCategoryCode As String ' TJS 30/03/11 
        Public CategoryName As String ' TJS 29/03/11
        Public CategoryDescription As String ' TJS 29/03/11
        Public CategoryID As Integer ' TJS 29/03/11
        Public ParentID As Integer ' TJS 29/03/11
    End Structure

    Public Function SendXMLToASPStorefront(ByVal UseWSE3Authentication As Boolean, ByVal APIURL As String, ByVal APIUser As String, _
        ByVal APIPassword As String, ByVal DataToSubmit As String, ByRef ErrorDetails As String) As XDocument
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 18/03/11 | TJS             | 2011.0.01 | Function added
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim XMLResponse As XDocument = Nothing
        Dim ASPDotNetUserToken As Microsoft.Web.Services3.Security.Tokens.UsernameToken
        Dim ASPDotNetWebServiceWse3 As ASPDotNetStorefront.AspDotNetStorefrontImportWebServiceWse
        Dim ASPDotNetWebService As ASPDotNetStorefront.AspDotNetStorefrontImportWebService
        Dim strReturn As String

        ErrorDetails = ""

        Try
            If UseWSE3Authentication Then
                ASPDotNetUserToken = New Microsoft.Web.Services3.Security.Tokens.UsernameToken(APIUser, APIPassword, Microsoft.Web.Services3.Security.Tokens.PasswordOption.SendHashed)
                ASPDotNetWebServiceWse3 = New ASPDotNetStorefront.AspDotNetStorefrontImportWebServiceWse()
                ASPDotNetWebServiceWse3.Url = APIURL
                'ASPDotNetWebServiceWse3.SetPolicy("UserNameToken");
                'ASPDotNetWebServiceWse3.SetClientCredential(ASPDotNetUserToken);
                ASPDotNetWebServiceWse3.RequestSoapContext.Security.Tokens.Add(ASPDotNetUserToken)
                strReturn = ASPDotNetWebServiceWse3.DoItWSE3(DataToSubmit)
            Else
                ASPDotNetWebService = New ASPDotNetStorefront.AspDotNetStorefrontImportWebService()
                ASPDotNetWebService.Url = APIURL
                strReturn = ASPDotNetWebService.DoItUsernamePwd(APIUser, APIPassword, DataToSubmit)
            End If
            If strReturn <> "" Then
                If strReturn.Length > 38 Then
                    If strReturn.Trim.Substring(0, 38).ToLower = "<?xml version=""1.0"" encoding=""utf-8""?>" Then
                        Try
                            XMLResponse = XDocument.Parse(strReturn)

                        Catch ex As Exception
                            ErrorDetails = "XML from " & APIURL & " could not be processed due to XML error - " & ex.Message.Replace(vbCrLf, "")

                        End Try
                    Else
                        ErrorDetails = "Response from " & APIURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?> - " & vbCrLf & vbCrLf & strReturn
                    End If

                Else
                    ErrorDetails = "Response from " & APIURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?> - " & vbCrLf & vbCrLf & strReturn
                End If

            Else
                ErrorDetails = "Response from " & APIURL & " does not begin with <?xml version=""1.0"" encoding=""UTF-8""?> - " & vbCrLf & vbCrLf & strReturn
            End If

        Catch ex As Exception
            If UseWSE3Authentication And ex.Message.ToLowerInvariant().IndexOf("the computed password digest doesn't match that of the incoming username token") <> -1 Then
                ErrorDetails = "ASPDotNetStorefront Authentication Failure when connecting to " & APIURL & ". Probably an invalid Password specified - you must enter the master hashed password from the AspDotNetStorefront database (do not use the clear text password."
            ElseIf UseWSE3Authentication And ex.Message.ToLowerInvariant().IndexOf("the incoming username token contains a password hash") <> -1 Then
                ErrorDetails = "ASPDotNetStorefront Authentication Failure when connecting to " & APIURL & ". Probably an invalid E-Mail specified."
            Else
                ErrorDetails = "ASPDotNetStorefront Authentication Failure when connecting to " & APIURL & " - " & ex.Message & ", " & ex.StackTrace
            End If

        End Try

        Return XMLResponse

    End Function

End Class
