' eShopCONNECT for Connected Business - Windows Service
' Module: AmazonMWSThrottling.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

'-------------------------------------------------------------------
'
' Last Updated - 05 July 2013

Imports Microsoft.VisualBasic

Public Class AmazonMWSThrottling

    Public Structure AmazonThrottleLimit
        Public AmazonSite As String
        Public OperationName As String
        Public RequestLimit As Integer
        Public RestoreRate As Double
        Public AvailableCount As Integer
        Public LastRequest As Date
    End Structure

    Private AmazonThrottleLimits() As AmazonThrottleLimit

    Public Function CheckNotThrottleLimited(ByVal AmazonSite As String, ByVal OperationToCheck As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -      Checks if we have reached the Amazon throttle limit
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/07/13 | TJS             | 2013.1.24 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, bOperationFound As Boolean

        bOperationFound = False
        For iLoop = 0 To AmazonThrottleLimits.Length - 1
            If AmazonThrottleLimits(iLoop).AmazonSite = AmazonSite And AmazonThrottleLimits(iLoop).OperationName = OperationToCheck Then
                bOperationFound = True
                ' have any requests been sent ?
                If AmazonThrottleLimits(iLoop).AvailableCount < AmazonThrottleLimits(iLoop).RequestLimit Then
                    ' yes, add in any restore rate
                    AmazonThrottleLimits(iLoop).AvailableCount = AmazonThrottleLimits(iLoop).AvailableCount + CInt(AmazonThrottleLimits(iLoop).RestoreRate * DateDiff(DateInterval.Minute, AmazonThrottleLimits(iLoop).LastRequest, Date.Now))
                    ' has count gone above Request Limit ?
                    If AmazonThrottleLimits(iLoop).AvailableCount > AmazonThrottleLimits(iLoop).RequestLimit Then
                        ' yes, set as Request Limit
                        AmazonThrottleLimits(iLoop).AvailableCount = AmazonThrottleLimits(iLoop).RequestLimit
                    End If
                End If
                ' are we throttled ?
                If AmazonThrottleLimits(iLoop).AvailableCount > 0 Then
                    ' no, reduce available count by 1
                    AmazonThrottleLimits(iLoop).AvailableCount = AmazonThrottleLimits(iLoop).AvailableCount - 1
                    AmazonThrottleLimits(iLoop).LastRequest = Date.Now
                    CheckNotThrottleLimited = True
                Else
                    ' yes
                    CheckNotThrottleLimited = False
                End If
                Exit For
            End If
        Next
        If Not bOperationFound Then
            ' new function in array, not throttled yet
            ' is last entry empty ?
            If Not String.IsNullOrEmpty(AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).OperationName) Then
                ' no, add another entry
                ReDim AmazonThrottleLimits(AmazonThrottleLimits.Length)
            End If
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).OperationName = OperationToCheck
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).AmazonSite = AmazonSite
            Select Case OperationToCheck
                Case "GetReport", "SubmitFeed"
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RequestLimit = 15
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RestoreRate = 0.5
                Case Else
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RequestLimit = 10
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RestoreRate = 1
            End Select
            ' set available count as 1 less than maximum to cover current operation
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).AvailableCount = AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RequestLimit - 1
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).LastRequest = Date.Now
            CheckNotThrottleLimited = True
        End If

    End Function

    Public Sub SetThrottledFlag(ByVal AmazonSite As String, ByVal MerchantToken As String, _
        ByVal OperationThrottled As String, ByRef ThrottlingDetails As String)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -      Marks current operation as fully throttled
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/07/13 | TJS             | 2013.1.24 | Function added
        ' 05/07/13 | TJS             | 2013.1.25 | Modified throttling message to include Merchant Token
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iLoop As Integer, bOperationFound As Boolean

        ThrottlingDetails = ""
        bOperationFound = False
        For iLoop = 0 To AmazonThrottleLimits.Length - 1
            If AmazonThrottleLimits(iLoop).AmazonSite = AmazonSite And AmazonThrottleLimits(iLoop).OperationName = OperationThrottled Then
                bOperationFound = True
                ThrottlingDetails = "Throttling occurred on Merchant Account " & MerchantToken & " on Amazon" & AmazonSite & ", Operation " & OperationThrottled & " - Request Limit set to " & AmazonThrottleLimits(iLoop).RequestLimit & ", Restore Rate set to " & AmazonThrottleLimits(iLoop).RestoreRate & ", Expected Available count was " & AmazonThrottleLimits(iLoop).AvailableCount & " with last request at " & AmazonThrottleLimits(iLoop).LastRequest.ToString("MM/dd/yyyy HH:mm:ss") ' TJS 05/07/13
                AmazonThrottleLimits(iLoop).AvailableCount = 0
                AmazonThrottleLimits(iLoop).LastRequest = Date.Now
                Exit For
            End If
        Next
        If Not bOperationFound Then
            ' new function in array, fully throttled 
            ReDim AmazonThrottleLimits(AmazonThrottleLimits.Length)
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).OperationName = OperationThrottled
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).AmazonSite = AmazonSite
            Select Case OperationThrottled
                Case "GetReport", "SubmitFeed"
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RequestLimit = 15
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RestoreRate = 0.5
                Case Else
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RequestLimit = 10
                    AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RestoreRate = 1
            End Select
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).AvailableCount = 0
            AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).LastRequest = Date.Now
            ThrottlingDetails = "Merchant Account " & MerchantToken & " on Amazon " & AmazonSite & ", Operation " & OperationThrottled & " experienced throttling - No existing record found, Request Limit set to " & AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RequestLimit & ", Restore Rate set to " & AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).RestoreRate & " at " & AmazonThrottleLimits(AmazonThrottleLimits.Length - 1).LastRequest.ToString("MM/dd/yyyy HH:mm:ss") ' TJS 05/07/13
        End If

    End Sub

    Public Sub New()
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -      Marks current operation as fully throttled
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 03/07/13 | TJS             | 2013.1.24 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        ReDim AmazonThrottleLimits(0)

    End Sub
End Class
