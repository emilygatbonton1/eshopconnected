' eShopCONNECT for Connected Business
' Module: ImportTest.aspx
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
' Last Updated - 24 August 2012

Imports Lerryn.Framework.ImportExport.Shared.ConfigConst
Imports Lerryn.Framework.ImportExport.Shared.Const
Imports System.IO ' TJS 25/04/11
Imports System.Net ' TJS 02/12/11
Imports System.Web ' TJS 18/03/11
Imports System.Xml.Linq ' TJS 02/12/11
Imports System.Xml.XPath ' TJS 02/12/11

Partial Class ImportTest
    Inherits System.Web.UI.Page

    Protected Sub SendXML_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles SendXML.ServerClick

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -   
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 08/03/09 | TJS             | 2009.1.09 | Modified to display Exception stack and cater for case variations in input URL
        ' 17/04/09 | TJS             | 2009.2.03 | Modifeid to cater for base url being an editable field
        ' 07/05/09 | TJS             | 2009.1.06 | Modified to use POST for Shop.com as well as other options
        ' 18/03/11 | TJS             | 2011.0.01 | Modified to cater for IS eCommerce Plus version
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to use XML.Linq and XML.XPath instead of MSXML2
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim WebSubmit As WebRequest, WebResponse As WebResponse = Nothing, postStream As Stream = Nothing ' TJS 02/12/11
        Dim byteData() As Byte, reader As StreamReader ' TJS 02/12/11
        Dim strURL As String ' TJS 08/03/09

        If XMLToSend.Value <> "" Then
            Select Case SelectURL.Value
                Case "GenericXMLImport.ashx", "GenericImportStatus.ashx", "ShopComOrder.ashx"

                    If Right(BaseURL.Value, 1) <> "/" Then ' TJS 17/04/09
                        BaseURL.Value = BaseURL.Value & "/" ' TJS 17/04/09
                    End If
                    strURL = BaseURL.Value & SelectURL.Value ' TJS 17/04/09

                    ' start of code replaced TJS 18/07/11
                    Try
                        WebSubmit = System.Net.WebRequest.Create(strURL)
                        WebSubmit.Method = "POST"
                        WebSubmit.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                        '  WebSubmit.ContentType = "application/xml"
                        WebSubmit.ContentLength = XMLToSend.Value.Replace(" ", "%20").Length
                        WebSubmit.Timeout = 30000

                        byteData = UTF8Encoding.UTF8.GetBytes(XMLToSend.Value.Replace(" ", "%20"))

                        ' send to LerrynSecure.com (or the URL defined in the Registry)
                        postStream = WebSubmit.GetRequestStream()
                        postStream.Write(byteData, 0, byteData.Length)

                        WebResponse = WebSubmit.GetResponse
                        reader = New StreamReader(WebResponse.GetResponseStream())
                        ResponseXML.Value = reader.ReadToEnd()

                    Catch ex As Exception
                        ResponseXML.Value = ex.Message & vbCrLf & ex.StackTrace ' TJS 08/03/09

                    Finally
                        If Not postStream Is Nothing Then postStream.Close()
                        If Not WebResponse Is Nothing Then WebResponse.Close()

                    End Try
                    ' end of code replaced TJS 02/12/11

                Case Else

            End Select

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 11/02/09 | TJS             | 2009.1.07 | Function added to check IS DB connection and activation
        ' 06/03/09 | TJS             | 2009.1.09 | Modified to display Exception stack
        ' 17/04/09 | TJS             | 2009.2.03 | Modifeid to cater for base url being an editable field
        ' 02/12/11 | TJS             | 2011.2.00 | Modified to set LastWebPost_DEV000221 so config form can check if web service is installed and active
        ' 02/04/12 | TJS             | 2011.2.12 | Modified to include connection details in error message
        ' 10/06/12 | TJS             | 2012.1.05 | Added Error Notification object to simplify facade login/logout
        ' 24/08/12 | TJS             | 2012.1.14 | Modified for Connected Business 7
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim eShopCONNECTDatasetGateway As Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway ' TJS 11/02/09
        Dim eShopCONNECTFacade As Lerryn.Facade.ImportExport.ImportExportConfigFacade ' TJS 11/02/09
        Dim strProtocol As String, lPasswordPosn As Integer ' TJS 17/04/09

        Try
            ' try creating the 
            eShopCONNECTDatasetGateway = New Lerryn.Framework.ImportExport.DatasetGateway.ImportExportDatasetGateway ' TJS 11/02/09
            eShopCONNECTFacade = New Lerryn.Facade.ImportExport.ImportExportConfigFacade(eShopCONNECTDatasetGateway, New Lerryn.Business.ImportExport.ErrorNotification, PRODUCT_CODE, PRODUCT_NAME) ' TJS 11/02/09 TJS 18/03/11 TJS 10/06/12

            If Not eShopCONNECTFacade.IsActivated Then ' TJS 11/02/09
                ResponseXML.Value = "eShopCONNECT is not Activated." & vbCrLf & vbCrLf & "Please select the Activation Wizard on the Utilities/Setup menu of the eCommerce module in " & IS_PRODUCT_NAME & "." ' TJS 11/02/09 TJS 24/08/12
                '       XMLToSend.Disabled = True ' TJS 11/02/09
                '      SendXML.Disabled = True ' TJS 11/02/09
                '     SelectURL.Disabled = True ' TJS 11/02/09
            ElseIf BaseURL.Value = "" Then ' TJS 17/04/09
                If Context.Request.ServerVariables("SERVER_PORT_SECURE") = 1 Then ' TJS 17/04/09
                    strProtocol = "https://" ' TJS 17/04/09
                Else
                    strProtocol = "http://" ' TJS 17/04/09
                End If

                If Context.Request.ServerVariables("SERVER_PORT") <> "80" And Context.Request.ServerVariables("SERVER_PORT") <> "443" Then ' TJS 17/04/09
                    BaseURL.Value = strProtocol & Context.Request.ServerVariables("SERVER_NAME") & ":" & Context.Request.ServerVariables("SERVER_PORT") & Context.Request.ServerVariables("PATH_INFO").ToLower.Replace("importtest.aspx", "") ' TJS 17/04/09
                Else
                    BaseURL.Value = strProtocol & Context.Request.ServerVariables("SERVER_NAME") & Context.Request.ServerVariables("PATH_INFO").ToLower.Replace("importtest.aspx", "") ' TJS 17/04/09
                End If

                Try ' TJS 02/12/11
                    eShopCONNECTFacade.ExecuteNonQuery(System.Data.CommandType.Text, "UPDATE LerrynImportExportServiceAction_DEV000221 SET LastWebPost_DEV000221 = getdate()", Nothing) ' TJS 02/12/11
                Catch ex As Exception
                    eShopCONNECTFacade.SendErrorEmail(eShopCONNECTFacade.SourceConfig, "RunRoutines", ex) ' TJS 02/12/11
                End Try

            End If

        Catch ex As Exception
            If Interprise.Facade.Base.SimpleFacade.Instance IsNot Nothing Then ' TJS 02/04/12
                lPasswordPosn = InStr(Interprise.Facade.Base.SimpleFacade.Instance.OnlineConnectionString, ";Password=") ' TJS 02/04/12
                If lPasswordPosn > 0 Then
                    ResponseXML.Value = Left(Interprise.Facade.Base.SimpleFacade.Instance.OnlineConnectionString, lPasswordPosn + 9) & "******" & vbCrLf & ex.Message & vbCrLf & ex.StackTrace ' TJS 02/04/12
                Else
                    ResponseXML.Value = Interprise.Facade.Base.SimpleFacade.Instance.OnlineConnectionString & vbCrLf & ex.Message & vbCrLf & ex.StackTrace ' TJS 02/04/12
                End If
            Else
                ResponseXML.Value = ex.Message & vbCrLf & ex.StackTrace ' TJS 11/02/09 TJS 06/03/09
            End If
            XMLToSend.Disabled = True ' TJS 11/02/09
            SendXML.Disabled = True ' TJS 11/02/09
            SelectURL.Disabled = True ' TJS 11/02/09

        End Try

    End Sub
End Class
