Option Explicit On 
Option Strict On

Imports Interprise.Presentation.Base.Message
Imports Interprise.Presentation.Base

Namespace Search
    Public Class RepSearchTreeControl
        Inherits DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit

#Region " Component Designer generated code "

        Public Sub New(ByVal Container As System.ComponentModel.IContainer)
            MyClass.New()

            'Required for Windows.Forms Class Composition Designer support
            Container.Add(Me)
        End Sub

        'Component overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Component Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Component Designer
        'It can be modified using the Component Designer.
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
        End Sub

#End Region

#Region " Variables "
        Public m_btnSearchCombo As DevExpress.XtraEditors.ButtonEdit 'Variable to hold the actual button edit that will be used.
        Private m_grdParent As DevExpress.XtraTreeList.TreeList 'Variable to hold the grid to which this repository belongs to.
        Private m_drowSelected As DataRow 'Variable to hold the row selected by the user.

        Private m_strTableName As String
        Private m_strParentField As String
        Private m_strKeyField As String
        Private m_imgList As ImageList
        Private m_strDisplayField As String

        Private m_enmMovement As enmMovement

        Private m_strColumnNames As String()
        Private m_dsetDatasource As DataSet 'Data table that will be used for the data source, for offline mode.
        Private m_strOldValue As String 'Contains the old value of the control.
        Private m_intRowCount As Integer
        Private m_intRowHandle As Integer
        Private m_blnAlreadyDown As Boolean
        Private m_frmTreeSearch As Interprise.Presentation.Base.Search.MiniSearchTreeForm

        Private controlToRule As New Interprise.Presentation.Base.Search.ControlToRuleBridge
        Private m_retainvalue As Boolean
        Private m_additionalFilter As String
#End Region

#Region " Constants "
        Private Const NOT_FOUND_ERROR As String = "Value cannot be found."
#End Region

#Region " Enumerations "
        Public Enum enmMovement
            Vertical
            Horizontal
        End Enum
#End Region

#Region " CustomEvents "
        Public Event PopupClose(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs) 'Event that will be raised when the popup form closes.
        Public Event AddNewValue(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) 'Event that will be raised when the user wants to add a new value.
        Public Event BeforePopup(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
        Public Event BeforeDelete(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) 'Event that will be raised before the record is deleted
        Public Event CancelPopup(ByVal sender As Object, ByVal e As EventArgs)
#End Region

#Region " Properties "
        '<Title RowSelected>
        '
        'Property that reads the row selected by the user.
#Region "RowSelected"
        Public ReadOnly Property RowSelected() As DataRow
            Get
                Return m_drowSelected
            End Get
        End Property
#End Region


#Region "KeyField"
        '

        Public Property KeyField() As String
            Get
                Return m_strKeyField
            End Get
            Set(ByVal Value As String)
                m_strKeyField = Value
            End Set
        End Property
#End Region

#Region "ParentField"
        Public Property ParentField() As String
            Get
                Return m_strParentField
            End Get
            Set(ByVal Value As String)
                m_strParentField = Value
            End Set
        End Property
#End Region

#Region "AdditionalFilter"
      
        Public Property AdditionalFilter() As String
            Get
                Return m_additionalFilter
            End Get
            Set(ByVal Value As String)
                m_additionalFilter = Value
            End Set
        End Property
#End Region


#Region "Movement"
        '
        'Property that determines the focus movement of the grid when the row is selected.
        Public Property Movement() As enmMovement
            Get
                Return m_enmMovement
            End Get
            Set(ByVal Value As enmMovement)
                m_enmMovement = Value
            End Set
        End Property
#End Region

#Region "Columns"
        '<Title ColumnNames>
        '
        'Property to read or write the column names of the datasource used by the list control.
        Public Property Columns() As String()
            Get
                Return m_strColumnNames
            End Get
            Set(ByVal Value As String())
                m_strColumnNames = Value
            End Set
        End Property
#End Region

#Region "TreeImageList"
        Public Property TreeImageList() As ImageList
            Get
                Return m_imgList
            End Get
            Set(ByVal Value As ImageList)
                m_imgList = Value
            End Set
        End Property
#End Region

#Region "DataSource"
        '<Title DataSource>
        '
        'Property to read or write the datasource used by the list control.
        Public Property DataSource() As DataSet
            Get
                Return m_dsetDatasource
            End Get
            Set(ByVal Value As DataSet)
                m_dsetDatasource = Value
            End Set
        End Property
#End Region

#Region "TableName"
        Public Property TableName() As String
            Get
                Return m_strTableName
            End Get
            Set(ByVal Value As String)
                m_strTableName = Value
            End Set
        End Property
#End Region

#Region "DisplayField"
        Public Property DisplayField() As String
            Get
                Return m_strDisplayField
            End Get
            Set(ByVal Value As String)
                m_strDisplayField = Value
            End Set
        End Property
#End Region

#Region "RowCount"
        Public ReadOnly Property RowCount() As Integer
            Get
                Return m_intRowCount
            End Get
        End Property
#End Region

#Region "RetainValue"
        Public Property RetainValue() As Boolean
            Get
                Return m_retainvalue
            End Get
            Set(ByVal value As Boolean)
                m_retainvalue = value
            End Set
        End Property
#End Region



#End Region

#Region " Procedures "

#Region " New "
        '<Title New>
        '
        'Constructor.  Assigns the first button image as a combo image.
        Public Sub New()
            MyBase.New()
            '  Me.Buttons(0).Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Combo
        End Sub
#End Region

#Region " ShowComboSearchForm "
        '<Title ShowComboSearchForm>
        '
        'Instantiates, positions and loads the ComboSearchForm.
        Public Sub ShowComboSearchForm(Optional ByVal blnBypassCache As Boolean = False)
            Try
                If Not m_frmTreeSearch Is Nothing Then Return
                Dim eCancel As New System.ComponentModel.CancelEventArgs
                RaiseEvent BeforePopup(Me.m_btnSearchCombo, eCancel)

                If eCancel.Cancel = True Then
                    m_blnAlreadyDown = False
                    Return
                End If


                'm_frmTreeSearch = New SearchTreeForm
                m_frmTreeSearch = New Interprise.Presentation.Base.Search.MiniSearchTreeForm(TableName, ParentField, KeyField, Columns, TreeImageList, AdditionalFilter, DisplayField)


                Me.m_btnSearchCombo.Cursor = System.Windows.Forms.Cursors.WaitCursor
                '
                'Use the CalcLocation function of DevExpress.Utils.ControlUtils to position properly the form.
                '
                Dim pntLocation As Point = m_btnSearchCombo.PointToScreen(Point.Empty) 'Location of the search combo control.
                Dim pntBottom As New Point(pntLocation.X, pntLocation.Y + m_btnSearchCombo.Height) 'Gets the lower left corner location of the control.
                m_frmTreeSearch.Size = New Size(CInt((2 * m_grdParent.FindForm.Width) / 3), CInt(m_grdParent.FindForm.Height / 2))
                m_frmTreeSearch.Location = DevExpress.Utils.ControlUtils.CalcLocation(pntBottom, pntLocation, m_frmTreeSearch.Size)
                'm_frmTreeSearch.TableName = m_strTableName
                'm_frmTreeSearch.Columns = m_strColumnNames
                'm_frmTreeSearch.ParentField = m_strParentField
                'm_frmTreeSearch.KeyField = m_strKeyField
                'm_frmTreeSearch.TreeImageList = m_imgList


                m_frmTreeSearch.Show()

                m_intRowCount = 0 '? 'm_frmTreeSearch.RowCount
                '
                'Add events when the form closes.
                '
                AddHandler m_frmTreeSearch.RowSelected, AddressOf OnRowSelected
                AddHandler m_frmTreeSearch.NoRowSelected, AddressOf OnNoRowSelected

            Catch ex As Exception
                MessageWindow.Show(ex.Message)
            Finally
                m_btnSearchCombo.Cursor = System.Windows.Forms.Cursors.Default
            End Try
        End Sub
#End Region

#Region " DeleteRow "
        Public Sub DeleteRow()
            '
            'Popup a messagebox asking if the user wants to delete the line item or not
            If MessageWindow.Show("Do you want to delete this line item?") = DialogResult.Yes Then
                '
                '
                'Delete the selected row in the grid.
                '
                m_grdParent.DeleteNode(m_grdParent.FocusedNode)
            End If
        End Sub
#End Region

#End Region

#Region " Events "

#Region "Event Handlers"

#Region " RepSearchComboControl_ButtonClick "
        '<Title RepSearchComboControl_ButtonClick>
        '
        'Event triggered when the button of the button edit is clicked.
        Private Sub RepSearchComboControl_ButtonClick(ByVal sender As Object, _
        ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles MyBase.ButtonClick

            '
            'Assign the sender to the button edit.
            '
            m_btnSearchCombo = DirectCast(sender, DevExpress.XtraEditors.ButtonEdit)
            '
            'Assign the parent of the button edit to the grid.
            '
            m_grdParent = DirectCast(m_btnSearchCombo.Parent, DevExpress.XtraTreeList.TreeList)

            If e.Button.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Combo And (Not m_blnAlreadyDown) Then
                '
                'Show the form containing the list control.
                '
                ShowComboSearchForm()
                m_blnAlreadyDown = True
            ElseIf e.Button.Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Delete Then
                '
                'Trigger BeforeDelete event
                Dim argCancel As New System.ComponentModel.CancelEventArgs
                RaiseEvent BeforeDelete(Me, argCancel)
                '
                'Check if delete is cancelled
                If argCancel.Cancel = True Then
                    Return
                End If

                DeleteRow()
            End If
        End Sub
#End Region

#Region " RepSearchComboControl_Validating "
        '<Title RepSearchComboControl_Validating>
        '
        'Event that will be triggered when when the editor loses focus, will check if the user input is valid.
        Private Sub RepSearchComboControl_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Validating
            Try
                If m_strTableName = String.Empty Or m_strDisplayField = String.Empty Then Return

                Dim frmComboSearch As New Interprise.Presentation.Base.Search.MiniSearchForm

                '
                'Assign the sender to the button edit.
                '
                m_btnSearchCombo = DirectCast(sender, DevExpress.XtraEditors.ButtonEdit)
                '
                'Assign the parent of the button edit to the grid.
                '
                m_grdParent = DirectCast(m_btnSearchCombo.Parent, DevExpress.XtraTreeList.TreeList)

                '
                'Check if the old value and the validating value are the same.
                '
                'If m_strOldValue = m_btnSearchCombo.Text And m_intRowHandle = m_gvwParent.FocusedRowHandle Then
                '    If m_btnSearchCombo.IsModified Then
                '        m_gvwParent.SetRowCellValue(Me.m_gvwParent.FocusedRowHandle, _
                '     m_gvwParent.FocusedColumn, m_btnSearchCombo.Text)
                '    End If
                '    Return
                'End If


                '
                'Check the validation.
                '
                Dim drowValid As DataRowCollection
                If CStr(Interprise.Framework.Base.Shared.Common.IsNull(DirectCast(sender, DevExpress.XtraEditors.ButtonEdit).Text)) = String.Empty Then
                    drowValid = DirectCast(controlToRule.ValidateEntry(m_strTableName, m_strDisplayField, _
                    CStr(Interprise.Framework.Base.Shared.Common.IsNull(DirectCast(sender, DevExpress.XtraEditors.ButtonEdit).Text)), String.Empty, True), DataRowCollection)
                Else
                    Dim strValue As String = CStr(Interprise.Framework.Base.Shared.Common.IsNull(DirectCast(sender, DevExpress.XtraEditors.ButtonEdit).Text)).Trim
                    strValue = strValue.Replace("[", "[[]")
                    strValue = strValue.Replace("]", "[]]")
                    strValue = strValue.Replace("_", "[_]")
                    'strValue = strValue.Replace("'", "''")
                    'strValue = strValue.Replace("%", "[%]")
                    'drowValid = DirectCast(controlToRule.ValidateEntry(m_strTableName, m_strDisplayField, _
                    '"'" & strValue & "%' ", String.Empty, True), DataRowCollection)
                    drowValid = DirectCast(controlToRule.ValidateEntry(m_strTableName, m_strDisplayField, _
                    strValue, AdditionalFilter, True), DataRowCollection)

                End If


                '
                'If there is nothing returned, cancel the validation.
                '
                '
                'Clear previous errors from the grid view.
                '
                'Me.m_gvwParent.ClearColumnErrors()
                If drowValid Is Nothing Then
                    '
                    'If the input is invalid, pop up a messagebox then select the text.
                    '
                    'MessageBox.Show("You have entered an invalid value.", "Interprise Financials", _
                    'MessageBox.MessageBoxButtons.OK, MessageBox.MessageBoxIcon.Error, MessageBox.MessageBoxDefaultButton.Button1)
                    'Me.m_gvwParent.ShowEditor()
                    RaiseEvent AddNewValue(Me, e)

                    If CStr(Interprise.Framework.Base.Shared.Common.IsNull(DirectCast(sender, DevExpress.XtraEditors.ButtonEdit).Text)) = String.Empty Then Return

                    Dim strValue As String = CStr(Interprise.Framework.Base.Shared.Common.IsNull(DirectCast(sender, DevExpress.XtraEditors.ButtonEdit).Text)).Trim
                    strValue = strValue.Replace("[", "[[]")
                    strValue = strValue.Replace("]", "[]]")
                    strValue = strValue.Replace("_", "[_]")
                    'strValue = strValue.Replace("'", "''")
                    'strValue = strValue.Replace("%", "[%]")
                    Dim drowValidNewValue As DataRowCollection = DirectCast(controlToRule.ValidateEntry(m_strTableName, m_strDisplayField, _
                    strValue, AdditionalFilter, True), DataRowCollection)

                    If drowValidNewValue Is Nothing Then
                        e.Cancel = True
                        'Me.m_gvwParent.CancelUpdateCurrentRow()
                        'Me.m_gvwParent.ShowEditor()
                    Else
                        If drowValidNewValue.Count = 1 Then
                            RaiseEvent PopupClose(Me, New Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs(drowValidNewValue(0), New DataRow() {drowValidNewValue(0)}))
                        ElseIf drowValidNewValue.Count > 1 Then
                            e.Cancel = True
                            ShowComboSearchForm(True)
                            Return
                        End If
                    End If

                    'ShowComboSearchForm()
                    'Me.m_gvwParent.GetDataRow(Me.m_gvwParent.FocusedRowHandle).SetColumnError(m_gvwParent.FocusedColumn.FieldName, Me.NOT_FOUND_ERROR)
                    'Me.m_gvwParent.SetColumnError(Me.m_gvwParent.FocusedColumn, Me.NOT_FOUND_ERROR)
                Else
                    If drowValid.Count = 1 Then
                        '
                        'To fix the case of the text to the right one in the database.
                        '
                        'Me.m_gvwParent.SetRowCellValue(Me.m_gvwParent.FocusedRowHandle, _
                        'Me.m_gvwParent.FocusedColumn, drowValid(0)(Me.m_strDisplayField))
                        RaiseEvent PopupClose(Me, New Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs(drowValid(0), New DataRow() {drowValid(0)}))

                    ElseIf drowValid.Count > 1 Then
                        e.Cancel = True
                        ShowComboSearchForm(True)
                        Return
                    End If
                End If

                m_strOldValue = m_btnSearchCombo.Text
                'm_intRowHandle = Me.m_gvwParent.FocusedRowHandle
            Catch ex As Exception
                MessageWindow.Show(ex.Message)
            End Try
        End Sub
#End Region

#Region " RepSearchComboControl_KeyDown "
        Private Sub RepSearchComboControl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
            '
            'If the key pressed is the F4 key, show the search form.
            '
            If e.KeyCode = Keys.F4 Or (e.Alt And e.KeyCode = Keys.Down) Then
                Dim butDelete As DevExpress.XtraEditors.Controls.EditorButton = Nothing
                For intButton As Integer = 0 To Me.Buttons.Count - 1
                    If Me.Buttons(intButton).Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Combo Then
                        butDelete = Me.Buttons(intButton)
                    End If
                Next
                Dim argsButton As New DevExpress.XtraEditors.Controls.ButtonPressedEventArgs(butDelete)
                RepSearchComboControl_ButtonClick(sender, argsButton)
            End If
        End Sub
#End Region

#End Region

#Region "Procedures"

#Region " OnRowSelected "
        '<Title OnRowSelected>
        '
        'Procedure that will be triggered when a row is selected in the list control.
        Private Sub OnRowSelected(ByVal sender As Object, ByVal eRow As Interprise.Framework.Base.EventArguments.Search.RowSelectedEventArgs)
            RaiseEvent PopupClose(Me, eRow)
            m_blnAlreadyDown = False
            OnMoveNext()
            m_frmTreeSearch = Nothing
        End Sub
#End Region

#Region " OnNoRowSelected "
        '<Title OnNoRowSelected>
        '
        'Procedure that will be triggered when there is no row selected.
        Private Sub OnNoRowSelected(ByVal sender As Object, ByVal e As EventArgs)
            'Me.m_gvwParent.CancelUpdateCurrentRow()
            '
            'This is needed so that the calling form will not be deactivated and sent to the back.
            '
            Me.m_grdParent.FindForm.Activate()
            m_blnAlreadyDown = False
            RaiseEvent CancelPopup(sender, New EventArgs)
            m_frmTreeSearch = Nothing
        End Sub
#End Region

#Region " OnMoveNext "
        '<Title OnMoveNext>
        '
        'Overridable procedure to move the focus of the grid when a row is selected.
        Public Overridable Sub OnMoveNext()
            Select Case m_enmMovement
                Case enmMovement.Horizontal
                    'Me.m_gvwParent.FocusedColumn = Me.m_gvwParent.VisibleColumns(Me.m_gvwParent.FocusedColumn.VisibleIndex + 1)
                    'If Me.m_gvwParent.FocusedColumn.OptionsColumn.ReadOnly Then
                    '    OnMoveNext()
                    'Else
                    '    Me.m_gvwParent.ShowEditor()
                    '    If m_gvwParent.ActiveEditor IsNot Nothing Then m_gvwParent.ActiveEditor.SelectAll()
                    'End If
                Case enmMovement.Vertical
                    'Me.m_gvwParent.MoveNext()
            End Select
        End Sub
#End Region

#End Region

#End Region

    End Class
End Namespace
