<%@ WebHandler Language="VB" Class="GenericXMLImport" %>

Imports System
Imports System.Web

Public Class GenericXMLImport : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        ProcessGenericXMLRequest(context)
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class