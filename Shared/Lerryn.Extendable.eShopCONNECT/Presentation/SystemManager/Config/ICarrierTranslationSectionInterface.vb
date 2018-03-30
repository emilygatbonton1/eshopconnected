Option Explicit On
Option Strict On

Imports System.Collections.Generic
Imports Lerryn.Framework.ImportExport.Shared

Namespace Presentation.SystemManager.Config

    Public Interface ICarrierTranslationSectionInterface
        Inherits Interprise.Extendable.Base.Presentation.Generic.IBaseFormSectionInterface

        WriteOnly Property SourceCode As String

        WriteOnly Property SourceConfig As String

        'Property ChannelAdvCarrierList As List(Of String)

        'Property ChannelAdvCarrierServiceType As List(Of String)

    End Interface
End Namespace
