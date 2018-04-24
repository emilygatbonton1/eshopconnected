' eShopCONNECT for Connected Business - Windows Service
' Module: FileOutput.vb
'
' This software is the copyright of Connected Business and
' may not be copied, duplicated or modified other than as permitted
' in the licence agreement.
'

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
