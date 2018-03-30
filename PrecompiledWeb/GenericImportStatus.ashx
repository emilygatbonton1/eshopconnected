<%@ WebHandler Language="VB" Class="GenericImportStatus" %>

Imports System
Imports System.Web

Public Class GenericImportStatus : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        ProcessGenericImportStatusRequest(context)
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class