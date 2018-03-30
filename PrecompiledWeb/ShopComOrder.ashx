<%@ WebHandler Language="VB" Class="ShopComOrder" %>

Imports System
Imports System.Web

Public Class ShopComOrder : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        ProcessShopComRequest(context)
        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class