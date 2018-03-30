'===============================================================================
' Interprise Suite SDK
' Copyright © 2004-2008 Interprise Software Systems International Inc.
' All rights reserved.
' 
' Interprise Plugin Factory - Generated Code
'
' This code and information is provided "as is" without warranty
' of any kind, either expressed or implied, including but not
' limited to the implied warranties of merchantability and
' fitness for a particular purpose.
'===============================================================================

Option Explicit On
Option Strict On

Imports System.ComponentModel.Design.Serialization

Namespace DatasetGateway

#Region " ImportExportDatasetGateway "
	' TODO: Set the 'Custom Tool Namespace' property of the ImportExportDataset.xsd file to 'DatasetComponent' from the property window.
	<DesignerSerializer(GetType(ImportExportDatasetGatewaySerializer), GetType(CodeDomSerializer))> _
	Public Class ImportExportDatasetGateway
        Inherits DatasetComponent.ImportExportDataset
	End Class	
#End Region

#Region " ImportExportDatasetGatewaySerializer "
	Friend Class ImportExportDatasetGatewaySerializer
		Inherits Interprise.Framework.Base.DatasetGateway.BaseDatasetGatewaySerializer

	#Region " Property "
	#Region " DerivedType "
		''' <summary>
		''' Overrides the property Interprise.Framework.Base.DatasetGateway.BaseDatasetGatewaySerializer.DerivedType
		''' </summary>
		''' <value>The Type object of the gateway (ImportExportDatasetGateway)</value>
		''' <returns>The Type object of the gateway (ImportExportDatasetGateway)</returns>
		''' <remarks></remarks>
		Public Overrides ReadOnly Property DerivedType() As System.Type
			Get
				Return GetType(ImportExportDatasetGateway)
			End Get
		End Property
	#End Region
	#End Region

	End Class
#End Region

End Namespace
