' Lerryn Licence Generation and Validation
' Module: LicenceFunctions.vb
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
'-------------------------------------------------------------------
'
' Updated 02 December 2011

Option Explicit On
Option Strict On

Imports Lerryn.Framework.ImportExport.Shared.Licence
Imports Microsoft.VisualBasic ' TJS 26/01/09

Module LicenceFunctions

    ' LicenceCode structure
    ' Characters    Function
    '      1        character substitution offset (Note Char 1 is not substituted)
    '     2-4       Product Code
    '      5        OS Code
    '      6        Language Code
    '     7-9       User Count
    '    10-11	    Secondary count
    '    12-24      System ID
    '    25-27	    ExpiryDate
    '      28	    Eval Flag
    '      29	    Spare
    '    30-35      Checksum
    ' 
    ' The code seen by the user is the Activation Code which is the Licence Code after the 
    ' character substitution has taken place using GetActivationChars
    ' To get the Licence Code from the Activation Code, the character substitution has to 
    ' be reversed using GetLicenceChars

    Public bLicencesValid As Boolean = False


    Private Const csLicenceChars As String = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789"
    Private Const csDisplayChars As String = "K9WQ2D8BZ7PJ0VREH61NYC5USLA3GMFT4X"
    Private Const csLicenceCodeLength As Integer = 35 ' TJS 27/06/08

    Public Function TestChecksum(ByVal InputString As String) As Long

        Dim strCheckSum As String = "", lRetVal As Integer

        Try

            lRetVal = CalculateChecksum(InputString.Substring(0, 29), strCheckSum)
            If lRetVal = ErrorCodes.NoError Then ' TJS 16/09/08
                If Len(InputString) <> csLicenceCodeLength Then ' TJS 27/06/08
                    Return ErrorCodes.LicenceCharCountInvalid
                Else
                    If strCheckSum = Right(InputString, 6) Then
                        Return ErrorCodes.NoError ' TJS 16/09/08
                    Else
                        Return ErrorCodes.ChecksumInvalid ' TJS 16/09/08
                    End If
                End If
            Else
                Return lRetVal
            End If

        Catch ex As Exception
            Return ErrorCodes.CalulationError ' TJS 16/09/08

        End Try

    End Function

    Public Function CalculateChecksum(ByVal InputString As String, ByRef Checksum As String) As Integer

        Dim Factor(29) As Integer, lRetVal As Integer, LongValue As Integer
        Dim CheckValue As Integer, iLoop As Integer

        Try
            Factor(1) = 571
            Factor(2) = 937
            Factor(3) = 113
            Factor(4) = 821
            Factor(5) = 223
            Factor(6) = 1151
            Factor(7) = 499
            Factor(8) = 101
            Factor(9) = 43
            Factor(10) = 97
            Factor(11) = 1013
            Factor(12) = 677
            Factor(13) = 131
            Factor(14) = 73
            Factor(15) = 349
            Factor(16) = 719
            Factor(17) = 313
            Factor(18) = 751
            Factor(19) = 557
            Factor(20) = 859
            Factor(21) = 461
            Factor(22) = 251
            Factor(23) = 617
            Factor(24) = 61
            Factor(25) = 1303
            Factor(26) = 181
            Factor(27) = 73
            Factor(28) = 821
            Factor(29) = 991

            CheckValue = 0
            For iLoop = 1 To 29
                lRetVal = AlphaToLong(Mid(InputString, iLoop, 1), LongValue)
                If lRetVal <> ErrorCodes.NoError Then ' TJS 16/09/08
                    Return ErrorCodes.AlphaValueInvalid ' TJS 16/09/08
                End If
                CheckValue = CheckValue + (LongValue * Factor(iLoop))
            Next iLoop
            CheckValue = CheckValue * 5
            lRetVal = LongToAlpha(CheckValue, 6, Checksum)
            If lRetVal <> ErrorCodes.NoError Then ' TJS 16/09/08
                Return ErrorCodes.CalulationError ' TJS 16/09/08
            End If
            Return ErrorCodes.NoError ' TJS 16/09/08

        Catch ex As Exception
            Return ErrorCodes.CalulationError ' TJS 16/09/08

        End Try

    End Function

    Public Function AlphaToLong(ByVal AlphaValue As String, ByRef LongValue As Integer) As Integer

        Dim LoopCount As Integer, AlphaChar As String, Multiplier As Integer
        Try
            AlphaToLong = ErrorCodes.NoError ' TJS 16/09/08
            If AlphaValue = "" Then
                LongValue = -1
                AlphaToLong = ErrorCodes.AlphaValueEmpty ' TJS 16/09/08
            ElseIf Len(AlphaValue) > 5 Then
                LongValue = -1
                AlphaToLong = ErrorCodes.AplhaValueTooLong ' TJS 16/09/08
            Else
                LongValue = 0
                Multiplier = 1
                For LoopCount = Len(AlphaValue) To 1 Step -1
                    AlphaChar = Mid(AlphaValue, LoopCount, 1)
                    If Asc(AlphaChar) >= 48 And Asc(AlphaChar) < 58 Then
                        LongValue = LongValue + ((Asc(AlphaChar) - 48) * Multiplier)
                    ElseIf Asc(AlphaChar) >= 65 And Asc(AlphaChar) < 73 Then ' Don't use I as can be confused with 1
                        LongValue = LongValue + ((Asc(AlphaChar) - 65 + 10) * Multiplier)
                    ElseIf Asc(AlphaChar) >= 74 And Asc(AlphaChar) < 79 Then ' Don't use O as can be confused with 0
                        LongValue = LongValue + ((Asc(AlphaChar) - 66 + 10) * Multiplier)
                    ElseIf Asc(AlphaChar) >= 80 And Asc(AlphaChar) < 91 Then
                        LongValue = LongValue + ((Asc(AlphaChar) - 67 + 10) * Multiplier)
                    Else
                        LongValue = -1
                        AlphaToLong = ErrorCodes.AlphaValueInvalid ' TJS 16/09/08
                        Exit For
                    End If
                    Multiplier = Multiplier * 34
                Next LoopCount
            End If

        Catch ex As Exception

        End Try

    End Function

    Public Function LongToAlpha(ByVal LongValue As Integer, ByVal NoOfChars As Integer, ByRef AlphaValue As String) As Integer

        Dim Char1Value As Integer, Char2Value As Integer, Char3Value As Integer
        Dim Char4Value As Integer, Char5Value As Integer, Char6Value As Integer

        Try
            AlphaValue = ""
            If LongValue < 0 Then
                LongToAlpha = ErrorCodes.LongValueNegative ' TJS 16/09/08
            Else
                Select Case NoOfChars
                    Case 1
                        If LongValue < 34 Then
                            LongToAlpha = CharValueToAlpha(LongValue, AlphaValue)
                        Else
                            LongToAlpha = ErrorCodes.LongValueTooBig ' TJS 16/09/08
                        End If
                    Case 2
                        If LongValue < 1156 Then ' 34 * 34
                            Char1Value = CInt(Microsoft.VisualBasic.Int((LongValue / 34)))
                            Char2Value = CInt(LongValue - (Char1Value * 34))
                            LongToAlpha = CharValueToAlpha(Char1Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char2Value, AlphaValue)
                        Else
                            AlphaValue = ""
                            LongToAlpha = ErrorCodes.LongValueTooBig ' TJS 16/09/08
                        End If
                    Case 3
                        If LongValue < 39304 Then ' 34 * 34 * 34
                            Char1Value = CInt(Microsoft.VisualBasic.Int((LongValue / 1156)))
                            Char2Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 1156)) / 34)))
                            Char3Value = CInt(LongValue - (Char1Value * 1156) - (Char2Value * 34))
                            LongToAlpha = CharValueToAlpha(Char1Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char2Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char3Value, AlphaValue)
                        Else
                            AlphaValue = ""
                            LongToAlpha = ErrorCodes.LongValueTooBig ' TJS 16/09/08
                        End If
                    Case 4
                        If LongValue < 1336336 Then ' 34 * 34 * 34 * 34
                            Char1Value = CInt(Microsoft.VisualBasic.Int((LongValue / 39304)))
                            Char2Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 39304)) / 1156)))
                            Char3Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 39304) - (Char2Value * 1156)) / 34)))
                            Char4Value = CInt(Microsoft.VisualBasic.Int((LongValue - (Char1Value * 39304) - (Char2Value * 1156) - (Char3Value * 34))))
                            LongToAlpha = CharValueToAlpha(Char1Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char2Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char3Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char4Value, AlphaValue)
                        Else
                            AlphaValue = ""
                            LongToAlpha = ErrorCodes.LongValueTooBig ' TJS 16/09/08
                        End If
                    Case 5
                        If LongValue < 45435424 Then  ' 34 * 34 * 34 * 34 * 34
                            Char1Value = CInt(Microsoft.VisualBasic.Int((LongValue / 1336336)))
                            Char2Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 1336336)) / 39304)))
                            Char3Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 1336336) - (Char2Value * 39304)) / 1156)))
                            Char4Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 1336336) - (Char2Value * 39304) - (Char3Value * 1156)) / 34)))
                            Char5Value = CInt(Microsoft.VisualBasic.Int((LongValue - (Char1Value * 1336336) - (Char2Value * 39304) - (Char3Value * 1156) - (Char4Value * 34))))
                            LongToAlpha = CharValueToAlpha(Char1Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char2Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char3Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char4Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char5Value, AlphaValue)
                        Else
                            AlphaValue = ""
                            LongToAlpha = ErrorCodes.LongValueTooBig ' TJS 16/09/08
                        End If
                    Case 6
                        If LongValue < 1544804416 Then  ' 34 * 34 * 34 * 34 * 34 * 34
                            Char1Value = CInt(Microsoft.VisualBasic.Int((LongValue / 45435424)))
                            Char2Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 45435424)) / 1336336)))
                            Char3Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 45435424) - (Char2Value * 1336336)) / 39304)))
                            Char4Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 45435424) - (Char2Value * 1336336) - (Char3Value * 39304)) / 1156)))
                            Char5Value = CInt(Microsoft.VisualBasic.Int(((LongValue - (Char1Value * 45435424) - (Char2Value * 1336336) - (Char3Value * 39304) - (Char4Value * 1156)) / 34)))
                            Char6Value = CInt(Microsoft.VisualBasic.Int((LongValue - (Char1Value * 45435424) - (Char2Value * 1336336) - (Char3Value * 39304) - (Char4Value * 1156) - (Char5Value * 34))))
                            LongToAlpha = CharValueToAlpha(Char1Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char2Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char3Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char4Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char5Value, AlphaValue)
                            LongToAlpha = CharValueToAlpha(Char6Value, AlphaValue)
                        Else
                            AlphaValue = ""
                            LongToAlpha = ErrorCodes.LongValueTooBig ' TJS 16/09/08
                        End If
                    Case Else
                        AlphaValue = ""
                        LongToAlpha = ErrorCodes.LongValueTooBig ' TJS 16/09/08
                End Select
            End If

        Catch ex As Exception

        End Try

    End Function

    Private Function CharValueToAlpha(ByVal CharValue As Integer, ByRef AlphaValue As String) As Integer

        Try
            If CharValue < 0 Then
                AlphaValue = ""
                Return ErrorCodes.CalulationError ' TJS 16/09/08
            ElseIf CharValue < 10 Then
                AlphaValue = AlphaValue & Chr(48 + CharValue)
                Return ErrorCodes.NoError ' TJS 16/09/08
            ElseIf CharValue <= 17 Then
                AlphaValue = AlphaValue & Chr(65 + (CharValue - 10))
                Return ErrorCodes.NoError ' TJS 16/09/08
            ElseIf CharValue <= 22 Then
                AlphaValue = AlphaValue & Chr(65 + (CharValue - 9))
                Return ErrorCodes.NoError ' TJS 16/09/08
            ElseIf CharValue <= 33 Then
                AlphaValue = AlphaValue & Chr(65 + (CharValue - 8))
                Return ErrorCodes.NoError ' TJS 16/09/08
            Else
                AlphaValue = ""
                Return ErrorCodes.CalulationError ' TJS 16/09/08
            End If

        Catch ex As Exception

        End Try

    End Function

    Public Function ConvertLicenceCount(ByVal AlphaValue As String, ByRef LicenceCount As Integer) As Integer

        Dim LongValue As Integer

        Try
            If AlphaValue = "" Then
                LicenceCount = 0
                Return ErrorCodes.AlphaValueEmpty ' TJS 16/09/08
            ElseIf Len(AlphaValue) > 3 Then
                LicenceCount = 0
                Return ErrorCodes.AplhaValueTooLong ' TJS 16/09/08
            Else
                If AlphaToLong(AlphaValue, LongValue) = ErrorCodes.NoError Then ' TJS 16/09/08
                    If LongValue <= 500 Then
                        LicenceCount = LongValue
                        Return ErrorCodes.NoError ' TJS 16/09/08

                    ElseIf LongValue = 34 * 34 * 34 Then
                        LicenceCount = 999999999
                        Return ErrorCodes.NoError ' TJS 16/09/08

                    Else
                        LicenceCount = (LongValue - 500) * 100
                        Return ErrorCodes.NoError ' TJS 16/09/08

                    End If
                End If
            End If

        Catch ex As Exception

        End Try

    End Function

    Public Function GetLicenceChars(ByVal strActivationChars As String, ByVal iOffset As Integer) As String

        Dim strLicenceChars As String = "", iLoop As Integer, iCharPosn As Integer

        For iLoop = 0 To strActivationChars.Length - 1
            iCharPosn = csDisplayChars.IndexOf(strActivationChars.Substring(iLoop, 1))
            If iCharPosn >= 0 Then
                Do While iCharPosn + iOffset >= 34
                    iCharPosn = iCharPosn - 34
                Loop
                strLicenceChars = strLicenceChars & csLicenceChars.Substring(iCharPosn + iOffset, 1)
            Else
                strLicenceChars = strLicenceChars & "?"
            End If
            iOffset = iOffset + 2
        Next
        Return strLicenceChars

    End Function

    Public Function GetActivationChars(ByVal strLicenceChars As String, ByVal iOffset As Integer) As String

        Dim strActivationChars As String = "", iLoop As Integer, iCharPosn As Integer

        For iLoop = 0 To strLicenceChars.Length - 1
            iCharPosn = csLicenceChars.IndexOf(strLicenceChars.Substring(iLoop, 1))
            If iCharPosn >= 0 Then
                Do While iCharPosn - iOffset < 0
                    iCharPosn = iCharPosn + 34
                Loop
                strActivationChars = strActivationChars & csDisplayChars.Substring(iCharPosn - iOffset, 1)
            Else
                strActivationChars = strActivationChars & "?"
            End If
            iOffset = iOffset + 2
        Next
        Return strActivationChars

    End Function

#Region " BuildSystemRegID "
    Public Function BuildSystemRegID(ByVal SystemID As String, ByVal CacheID As String) As String

        Dim iLoop As Integer, strSystemRegID As String

        strSystemRegID = ""
        For iLoop = 0 To SystemID.Length - 1
            ' ignore - chars in SystemID
            If SystemID.Substring(iLoop, 1) <> "-" Then
                strSystemRegID = strSystemRegID & SystemID.Substring(iLoop, 1)
            End If
            If CacheID.Length >= iLoop + 1 Then
                ' ignore - chars in CacheID
                If CacheID.Substring(iLoop, 1) <> "-" Then
                    strSystemRegID = strSystemRegID & CacheID.Substring(iLoop, 1)
                End If
            End If
        Next
        Return strSystemRegID

    End Function
#End Region

#Region " ValidateActivationCode "
    Public Function ValidateActivationCode(ByVal ThisProductCode As String, ByVal ActivationCode As String, ByVal SystemHashCode As String) As Integer ' TJS 04/06/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 04/06/10 | TJS             | 2010.0.07 | Modified to use System Hash code from DB
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strLicenceCode As String

        Dim iOffset As Integer, iErrorCode As Integer

        If ActivationCode.Length > 0 Then ' TJS 27/06/08
            iErrorCode = AlphaToLong(ActivationCode.Substring(0, 1), iOffset)
            If iErrorCode = ErrorCodes.NoError Then ' TJS 16/09/08
                strLicenceCode = ActivationCode.Substring(0, 1) & GetLicenceChars(ActivationCode.Substring(1), iOffset)
                If TestChecksum(strLicenceCode) = ErrorCodes.NoError Then ' TJS 16/09/08
                    If GetLicenceProductCode(strLicenceCode) = ThisProductCode Then
                        If CheckLicenceSystemID(strLicenceCode, SystemHashCode, True) Then ' TJS 04/06/10
                            If GetLicenceExpiryDate(strLicenceCode) >= Date.Today Then
                                Return ErrorCodes.NoError ' TJS 16/09/08
                            Else
                                Return ErrorCodes.LicenceExpired ' TJS 16/09/08
                            End If
                        Else
                            Return ErrorCodes.WrongSystemID ' TJS 16/09/08
                        End If
                    Else
                        Return ErrorCodes.WrongProductCode ' TJS 16/09/08
                    End If
                Else
                    Return ErrorCodes.ChecksumInvalid ' TJS 16/09/08
                End If
            Else
                Return iErrorCode
            End If
        Else
            Return ErrorCodes.NoLicenceFound ' TJS 27/06/08' TJS 16/09/08
        End If
    End Function
#End Region

#Region " GetLicenceProductCode "
    Public Function GetLicenceProductCode(ByVal LicenceCode As String) As String

        If LicenceCode.Length = csLicenceCodeLength Then ' TJS 27/06/08
            Return LicenceCode.Substring(1, 3)
        Else
            Return ""
        End If

    End Function
#End Region

#Region " GetLicenceUserCount "
    Public Function GetLicenceUserCount(ByVal LicenceCode As String) As Integer
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.       | Description
        '------------------------------------------------------------------------------------------
        ' 24/10/11 | TJS             | 2011.2.00   | Corrected extraction of User Count length
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim iUserCount As Integer

        If LicenceCode.Length = csLicenceCodeLength Then ' TJS 27/06/08
            If AlphaToLong(LicenceCode.Substring(6, 3), iUserCount) = ErrorCodes.NoError Then ' TJS 16/09/08 TJS 24/10/11
                Return iUserCount
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function
#End Region

#Region " GetLicenceSecondaryCount "
    Public Function GetLicenceSecondaryCount(ByVal LicenceCode As String) As Integer

        Dim iSecCount As Integer

        If LicenceCode.Length = csLicenceCodeLength Then ' TJS 27/06/08
            If AlphaToLong(LicenceCode.Substring(9, 2), iSecCount) = ErrorCodes.NoError Then ' TJS 16/09/08
                Return iSecCount
            Else
                Return 0
            End If
        Else
            Return 0
        End If

    End Function
#End Region

#Region " GetLicenceExpiryDate "
    Public Function GetLicenceExpiryDate(ByVal LicenceCode As String) As Date

        Dim iDateOffset As Integer

        If LicenceCode.Length = csLicenceCodeLength Then ' TJS 27/06/08
            If AlphaToLong(LicenceCode.Substring(24, 3), iDateOffset) = ErrorCodes.NoError Then ' TJS 16/09/08
                ' use 16/07/2005 as base date - difficult to guess !
                Return DateSerial(2005, 7, 16).AddDays(iDateOffset)
            Else
                Return Date.Today.AddDays(-1)
            End If
        Else
            Return Date.Today.AddDays(-1)
        End If

    End Function
#End Region

#Region " GetLicenceIsFull "
    Public Function GetLicenceIsFull(ByVal LicenceCode As String) As Boolean
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.     | Description
        '------------------------------------------------------------------------------------------
        ' 30/07/08 | TJS             | 2008.1.2.00 | Function added
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If LicenceCode.Length = csLicenceCodeLength Then
            If LicenceCode.Substring(27, 1) = "F" Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
#End Region

#Region " CheckLicenceSystemID "
    Public Function CheckLicenceSystemID(ByVal LicenceCode As String, ByVal SystemHashCode As String, ByVal AcceptDummyID As Boolean) As Boolean ' TJS 04/06/10
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '
        '   Description -    
        '
        ' Amendment Log
        '------------------------------------------------------------------------------------------
        ' Date     | Name            | Vers.       | Description
        '------------------------------------------------------------------------------------------
        ' 27/06/08 | TJS             | 2008.1.00   | Modified to check for empty Activation Codes
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Dim strLicenceSystemID As String

        If LicenceCode.Length = csLicenceCodeLength Then ' TJS 27/06/08
            ' do we have a saved hash code ?
            If "" & SystemHashCode = "" Then ' TJS 04/06/10
                ' if we haven't got a hash code, then assume system ID matches
                Return True ' TJS 04/06/10
            End If

            ' get System Hash Code from licence
            strLicenceSystemID = LicenceCode.Substring(11, 13)

            ' do they match (also accept dummy values starting with as system may not be registered when licence issued ?
            If strLicenceSystemID = SystemHashCode Or (strLicenceSystemID.Substring(0, 1) = "T" And AcceptDummyID) Then ' TJS 04/06/10
                ' yes, licence valid
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
#End Region

End Module
