#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: frm_msgbox.vb
' Tipo.........: Formulario VB.
' Versao.......: 2.02+
' Propósito....: Formulario com o "msgbox" personalizado do sistema.
' Uso..........: Não se aplica
' Produto......: GerCor
'
' Legal........: Este código é de propriedade do Banco Bradesco S/A e/ou Sonda-IT - Serviços de TI, sua cópia
'                e/ou distribuição é proibida.
'
' GUID.........: {7CC82C98-9E60-4498-9681-7102635D1782}
' Observações..: nenhuma.
'
'************************************************************************************************************************
#End Region

Option Explicit On
Option Strict On

Imports Sonda.Util.clsMsgBox
Imports System.Drawing


#Const FIRULA = True

Public Class frm_msgbox

    Dim bClicou As Boolean = False

    Property iTempoAutoFecha As Integer

    Dim oReloginhoMsg As Timer = Nothing

#If FIRULA Then
    Dim ImgNormal As Bitmap
    Dim ImgNormalJoia1 As Bitmap
    Dim ImgNormalJoia2 As Bitmap
#End If

    Public Sub New()

        MyBase.New()
        Try
            Me.DoubleBuffered = True
            InitializeComponent()
        Catch ex As Exception
            Debug.Print("Erro em frm_msgbox::New: " & ex.Message())
        End Try

    End Sub

    Private Sub ReloginhoBatendo(ByVal sender As Object, ByVal e As EventArgs)
        Try
            iTempoAutoFecha -= 1
            lblInfoFechar.Text = ObterMensagemTempoFecha(iTempoAutoFecha)

            If iTempoAutoFecha = 0 Then
                oReloginhoMsg.Enabled = False
                btn1.PerformClick()
            End If

        Catch ex As Exception
            LogaErro("Erro em ReloginhoBatendo: " & ex.Message())
        End Try
    End Sub

    Private Sub frm_msgbox_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Try

            If iTempoAutoFecha > 0 Then
                oReloginhoMsg = New Timer()
                oReloginhoMsg.Enabled = False
                oReloginhoMsg.Interval = 1000

                AddHandler oReloginhoMsg.Tick, AddressOf ReloginhoBatendo

                lblInfoFechar.Text = ObterMensagemTempoFecha(iTempoAutoFecha)
                lblInfoFechar.Visible = True
                oReloginhoMsg.Enabled = True
            Else
                lblInfoFechar.Visible = False
            End If

            Me.Focus()

            bClicou = False

            If btn1.Visible = True Then
                If btn1.Tag IsNot Nothing Then
                    If btn1.Tag.ToString = "1" Then
                        btn1.Focus()
                        Exit Sub
                    End If
                End If
            End If

            If btn2.Visible = True Then
                If btn2.Tag IsNot Nothing Then
                    If btn2.Tag.ToString = "1" Then
                        btn2.Focus()
                        Exit Sub
                    End If
                End If
            End If

            If btn3.Visible = True Then
                If btn3.Tag IsNot Nothing Then
                    If btn3.Tag.ToString = "1" Then
                        btn3.Focus()
                        Exit Sub
                    End If
                End If
            End If

            If btn1.Visible = True Then
                btn1.Focus()
            End If

        Catch ex As Exception
            LogaErro("Erro em frm_msgbox_Activated: " & CStr(ex.ToString()))
        End Try
    End Sub

    Private Sub btn1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn1.Click
        Try

            bClicou = True

            If CInt(Me.Tag) = eBotoes.Ok Then
                vMsgBox = eRet.Ok
            End If

            If CInt(Me.Tag) = eBotoes.SimNao Then
                vMsgBox = eRet.Sim
            End If

            If CInt(Me.Tag) = eBotoes.OkCancel Then
                vMsgBox = eRet.Ok
            End If

            If CInt(Me.Tag) = eBotoes.SimNaoCancel Then
                vMsgBox = eRet.Sim
            End If

            Me.Close()

        Catch ex As Exception
            LogaErro("Erro em btn1_Click: " & CStr(ex.ToString()))
        End Try
    End Sub

    Private Sub btn2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn2.Click
        Try

            bClicou = True


            If CInt(Me.Tag) = eBotoes.SimNao Then
                vMsgBox = eRet.Nao
            End If

            If CInt(Me.Tag) = eBotoes.OkCancel Then
                vMsgBox = eRet.Cancel
            End If

            If CInt(Me.Tag) = eBotoes.SimNaoCancel Then
                vMsgBox = eRet.Nao
            End If

            Me.Close()

        Catch ex As Exception
            LogaErro("Erro em btn2_Click: " & CStr(ex.ToString()))
        End Try
    End Sub

    Private Sub btn3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn3.Click
        Try

            bClicou = True

            If CInt(Me.Tag) = eBotoes.SimNaoCancel Then
                vMsgBox = eRet.Cancel
            End If

            Me.Close()

        Catch ex As Exception
            LogaErro("Erro em btn3_Click: " & ex.ToString())
        End Try

    End Sub

    Private Sub frm_msgbox_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            'se nao clicou nos botoes, é pq o usuario fechou o form via [X]....
            If bClicou = False Then

                e.Cancel = True 'Nao assume nada... obrigatorio usuario clicar em um botao
#If False Then
            If CInt(Me.Tag) = vbOKOnly Then
                vMsgBox = vbOK
            End If

            If CInt(Me.Tag) = vbYesNo Then
                vMsgBox = vbNo
            End If

            If CInt(Me.Tag) = vbOKCancel Then
                vMsgBox = vbNo
            End If

            If CInt(Me.Tag) = vbYesNoCancel Then
                vMsgBox = vbCancel
            End If
#End If
            End If

        Catch ex As Exception
            LogaErro("Erro em frm_msgbox_FormClosing: " & ex.ToString())
        End Try

    End Sub

#If FIRULA Then
    Private Function gerajoia(ByVal ImgNormal As Bitmap) As Bitmap

        Try
            If ImgNormal Is Nothing Then
                Return Nothing
            End If

            Dim joia As Bitmap

            joia = My.Resources.m_like_icon

            Dim BMP As New Bitmap(ImgNormal.Width, ImgNormal.Height)

            Dim GR As Graphics = Graphics.FromImage(BMP)

            GR.DrawImage(ImgNormal, 0, 0)
            GR.DrawImage(joia, 28, 28)

            Return BMP

        Catch ex As Exception
            LogaErro("Erro em gerajoia: " & ex.ToString())
            Return Nothing
        End Try

    End Function

    Private Function gerajoia2(ByVal ImgNormal As Bitmap) As Bitmap
        Try

            If ImgNormal Is Nothing Then
                Return Nothing
            End If

            Dim joia As Bitmap

            joia = My.Resources.m_dislike_icon

            Dim BMP As New Bitmap(ImgNormal.Width, ImgNormal.Height)

            Dim GR As Graphics = Graphics.FromImage(BMP)

            GR.DrawImage(ImgNormal, 0, 0)
            GR.DrawImage(joia, 28, 28)

            Return BMP

        Catch ex As Exception
            LogaErro("Erro em gerajoia2: " & ex.ToString())
            Return Nothing
        End Try

    End Function

    Private Sub frm_msgbox_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            ImgNormal = CType(imgMsgBox.Image, Bitmap)
            ImgNormalJoia1 = gerajoia(ImgNormal)
            ImgNormalJoia2 = gerajoia2(ImgNormal)
        Catch ex As Exception
            LogaErro("Erro em frm_msgbox_Load: " & ex.ToString())
        End Try
    End Sub


    Private Sub btn1_MouseHover(sender As Object, e As EventArgs) Handles btn1.MouseHover
        Try
            If Not ImgNormalJoia1 Is Nothing Then
                imgMsgBox.Image = ImgNormalJoia1
            End If
        Catch ex As Exception
            LogaErro("Erro em btn1_MouseHover: " & ex.ToString())
        End Try
    End Sub

    Private Sub btn1_MouseLeave(sender As Object, e As EventArgs) Handles btn1.MouseLeave
        Try
            If Not ImgNormal Is Nothing Then
                imgMsgBox.Image = ImgNormal
            End If
        Catch ex As Exception
            LogaErro("Erro em btn1_MouseLeave: " & ex.ToString())
        End Try
    End Sub

    Private Sub btn2_MouseHover(sender As Object, e As EventArgs) Handles btn2.MouseHover
        Try
            If Not ImgNormalJoia1 Is Nothing Then
                imgMsgBox.Image = ImgNormalJoia2
            End If
        Catch ex As Exception
            LogaErro("Erro em btn2_MouseHover: " & ex.ToString())
        End Try
    End Sub

    Private Sub btn2_MouseLeave(sender As Object, e As EventArgs) Handles btn2.MouseLeave
        Try
            If Not ImgNormal Is Nothing Then
                imgMsgBox.Image = ImgNormal
            End If
        Catch ex As Exception
            LogaErro("Erro em btn2_MouseLeave: " & ex.ToString())
        End Try
    End Sub
#End If

    Private Function ObterMensagemTempoFecha(ByVal iTempo As Integer) As String
        Return "Esta janela se fechará" & vbNewLine & "em " & iTempo.ToString & " segundo(s)"
    End Function

End Class