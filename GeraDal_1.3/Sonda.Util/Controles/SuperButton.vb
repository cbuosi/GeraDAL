#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Servi�os de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: SuperButton.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Prop�sito....: Modulo de Button
' Uso..........: N�o se aplica
' Produto......: GerCor
'
' Legal........: Este c�digo � de propriedade do Banco Bradesco S/A e/ou Sonda-IT - Servi�os de TI, sua c�pia
'                e/ou distribui��o � proibida.
'
' GUID.........: {7CC82C98-9E60-4498-9681-7102635D1782}
' Observa��es..: nenhuma.
'
'************************************************************************************************************************
#End Region

Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Drawing

Public Class SuperButton
    Inherits System.Windows.Forms.Button

    Dim _corTextoSelecionado As Color = Color.Black
    Dim _corTextoNaoSelecionado As Color = Color.Black

    Dim _imagemNaoSelecionado As Image = My.Resources.botao2
    Dim _imagemSelecionado As Image = My.Resources.botao2a

    Dim _fonte As Font = New Font("Verdana", 9, FontStyle.Regular)
    Const TempoPisca As Integer = 60
    Const NumPiscadas As Integer = 1

    Public Sub New()
        Try
            Me.DoubleBuffered = True
            Me.BackColor = System.Drawing.Color.Transparent
            Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            Me.Cursor = System.Windows.Forms.Cursors.Hand
            Me.BackgroundImage = _imagemNaoSelecionado
            Me.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.FlatAppearance.BorderSize = 1
            Me.FlatAppearance.BorderColor = Color.FromArgb(150, 0, 0)
            Me.Font = _fonte
            Me.ForeColor = _corTextoNaoSelecionado
            'Me.UseCompatibleTextRendering = True
            Me.UseVisualStyleBackColor = False
            Me.TextAlign = ContentAlignment.MiddleCenter
            Me.Invalidate()

        Catch ex As Exception
            LogaErro("Erro em SuperButton::New: " & CStr(ex.Message))
        End Try

    End Sub

    Protected Overrides Sub OnClick(e As System.EventArgs)

        Dim tmpImg As Image = Me.BackgroundImage

        If NumPiscadas >= 1 Then

            'Dim btnAtual As Bitmap = CType(Me.BackgroundImage, Bitmap)

            For i As Integer = 1 To NumPiscadas Step 1

                System.Threading.Thread.Sleep(TempoPisca)
                Me.BackgroundImage = _imagemNaoSelecionado
                Me.Refresh()

                System.Threading.Thread.Sleep(TempoPisca)
                Me.BackgroundImage = _imagemSelecionado

                Me.Refresh()

            Next i

        End If


        Me.BackgroundImage = tmpImg

        MyBase.OnClick(e)
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As System.EventArgs)

        Me.ForeColor = _corTextoSelecionado

        If Me.Enabled = True Then
            Me.BackgroundImage = _imagemNaoSelecionado
        Else
            Me.BackgroundImage = My.Resources.botao2Desligado
        End If

        MyBase.OnEnabledChanged(e)

    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)

        If Me.Enabled = True Then
            Me.ForeColor = _corTextoNaoSelecionado
            Me.BackgroundImage = _imagemNaoSelecionado
        End If

        MyBase.OnLeave(e)

    End Sub

    Protected Overrides Sub OnMouseEnter(ByVal e As System.EventArgs)

        If Me.Enabled = True Then
            Me.ForeColor = _corTextoSelecionado
            Me.BackgroundImage = _imagemSelecionado
        End If

        MyBase.OnMouseEnter(e)

    End Sub

    Protected Overrides Sub OnMouseLeave(ByVal e As System.EventArgs)

        If Me.Enabled = True Then
            Me.ForeColor = _corTextoNaoSelecionado
            Me.BackgroundImage = _imagemNaoSelecionado
        End If

        MyBase.OnMouseLeave(e)

    End Sub


    Protected Overrides Sub OnMouseDown(mevent As System.Windows.Forms.MouseEventArgs)

        If Me.Enabled = True Then
            Me.ForeColor = _corTextoSelecionado
            Me.BackgroundImage = _imagemNaoSelecionado
        End If

        MyBase.OnMouseDown(mevent)

    End Sub

    Protected Overrides Sub OnMouseUp(mevent As System.Windows.Forms.MouseEventArgs)

        If Me.Enabled = True Then
            Me.ForeColor = _corTextoNaoSelecionado
            Me.BackgroundImage = _imagemSelecionado
        End If

        MyBase.OnMouseUp(mevent)

    End Sub

End Class
