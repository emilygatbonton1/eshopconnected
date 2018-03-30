' eShopCONNECT for Connected Business
' Module: Enum.vb
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
' Updated 19 September 2013

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'   Description -    This module 
'
' Amendment Log
'------------------------------------------------------------------------------------------
' Date     | Name            | Vers.     | Description
'------------------------------------------------------------------------------------------
' 26/01/09 | TJS             | 2009.1.01 | Restructured projects to make separate OrderImporter product
' 05/07/12 | TJS             | 2012.1.08 | Added OpenAmazonSettlement to MenuAction MenuType
' 19/09/13 | TJS             | 2013.3.00 | Added OpenBulkPublishingForm to MenuAction MenuType
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Namespace [Shared]
    Public Module [Enum]
        Public Class MenuAction
            Public Enum MenuType
                OpenImpExpConfigSettingsForm ' TJS 26/01/09
                OpenImpExpActivationForm ' TJS 26/01/09
                OpenImpExpInventoryImportForm ' TJS 18/03/11
                OpenAmazonSettlementForm ' TJS 05/07/12
                OpenBulkPublishingForm ' TJS 19/09/13
            End Enum
        End Class

        Public Class Licence
            Public Enum ErrorCodes As Integer
                NoError = -1
                NoLicenceFound = 1
                WrongProductCode = 10
                WrongVersion = 11
                WrongLanguage = 12
                WrongSystemID = 13
                LicenceExpired = 14

                OSCodeNotFound = 20
                LicenceCountInvalid = 21

                LicenceCharCountInvalid = 30
                ChecksumInvalid = 31

                LicencesNotValidated = 50
                CannotRetrieveLicenceNoResponse = 51
                CannotRetrieveLicenceInvalidResponse = 52
                NoSystemIDFoundinDB = 53
                LicenceAlreadyInUse = 54
                EvalAlreadyExpired = 55

                LongValueTooBig = 100
                LongValueNegative = 101
                CalulationError = 102
                AlphaValueEmpty = 110
                AplhaValueTooLong = 111
                AlphaValueInvalid = 112

                ProcessError = 200

            End Enum
        End Class
    End Module
End Namespace