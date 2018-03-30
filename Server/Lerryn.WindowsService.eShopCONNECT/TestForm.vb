Public Class TestForm

    Dim ServiceTest As ServiceMain

    Private Sub btnStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStart.Click

        Dim args As String()

        Me.btnStart.Enabled = False
        Me.btnStart.Text = "Starting"
        ReDim args(0)
        ServiceTest.TestStart(args)
        Me.btnStop.Enabled = True
        Me.btnStart.Text = "Start"

    End Sub

    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click

        ServiceTest.TestStop()
        Me.btnStop.Enabled = False
        Me.btnStart.Enabled = True
        Me.btnStart.Text = "Start"

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ServiceTest = New ServiceMain

    End Sub
End Class