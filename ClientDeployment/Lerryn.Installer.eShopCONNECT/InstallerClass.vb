Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Configuration.Install

Public Class InstallerClass
    Inherits Interprise.Presentation.Base.PluginManager.PluginAssemblyInstaller

    Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
        MyBase.Install(stateSaver)
    End Sub

    Public Overrides Sub Commit(ByVal savedState As System.Collections.IDictionary)
        MyBase.Commit(savedState)
    End Sub

    Public Overrides Sub Rollback(ByVal savedState As System.Collections.IDictionary)
        MyBase.Rollback(savedState)
    End Sub

    Public Overrides Sub Uninstall(ByVal savedState As System.Collections.IDictionary)
        MyBase.Uninstall(savedState)
    End Sub
End Class
