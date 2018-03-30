' eShopCONNECT for Connected Business
' Module: InventoryConst.vb
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
' Last Updated - 07 November 2009

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'   Description -    This module 
'
' Amendment Log
'------------------------------------------------------------------------------------------
' Date     | Name            | Vers.     | Description
'------------------------------------------------------------------------------------------
' 31/05/09 | TJS             | 2009.2.10 | Module added
' 27/10/09 | TJS             | 2009.3.09 | Added new Amazon constants
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Namespace [Shared]
    Public Module InventoryConst

        ' Amazon Tag Codes
        Public Const AMAZON_DESCRIPTION_DATA_TAG As Integer = 1
        Public Const AMAZON_PRODUCTDATA_CLOTHING_VARIATIONDATA_TAG As Integer = 2
        Public Const AMAZON_PRODUCTDATA_CLOTHING_CLASSIFICATIONDATA_TAG As Integer = 3
        Public Const AMAZON_PRODUCTDATA_ITEM_DIMENSIONS_TAG As Integer = 5 ' TJS 27/10/09
        Public Const AMAZON_PRODUCTDATA_PACKAGE_DIMENSIONS_TAG As Integer = 6 ' TJS 27/10/09
        Public Const AMAZON_PRODUCTDATA_TAG As Integer = 4 ' TJS 07/11/09

    End Module
End Namespace