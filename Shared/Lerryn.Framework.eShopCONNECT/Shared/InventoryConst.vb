' eShopCONNECT for Connected Business
' Module: InventoryConst.vb
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