' eShopCONNECT for Connected Business - Windows Service
' Module: FileOutput.vb
'
' This software is the copyright of Lerryn Business Solutions Ltd and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
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
' Last Updated - 28 January 2009

Imports System
Imports System.Text
Imports System.Data
Imports System.Data.OleDb
Imports System.io

Module FileOutput

    Public Function CreateFiles() As Boolean

        Dim bAllComplete As Boolean

        dtFileCreationDueAt = dtFileCreationDueAt.AddMinutes(5)
        bAllComplete = True

        Try
            Return bAllComplete ' TJS 28/01/09

        Catch Ex As Exception
            m_ImportExportConfigFacade.SendErrorEmail(m_ImportExportConfigFacade.SourceConfig, "CreateFiles", Ex) ' TJS 28/01/09
            Return False
        End Try


    End Function

End Module
