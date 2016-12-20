#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: SuperTextBox.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Manipulacao de objetos do tipo TextBox
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

Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports Sonda.Util.clsMsgBox

Public Class SuperTextBox
    Inherits Windows.Forms.TextBox

    'Private usaMascara As Boolean = False
    Private bTravaErros As Boolean = False
    Private mascara As String = ""
    Private Obrigatorio As Boolean = False
    Private txtObrigatorio As String = ""
    Private corDesabilitado As Color = Nothing
    Private bAlterado As Boolean = False
    Private bMudaAuto As Boolean = True
    Dim oErrorProvider As System.Windows.Forms.ErrorProvider

    Dim colDescricaoErros As Collection


    Public Enum TipoMascara_
        NENHUMA
        NumerosInteiros
        NumerosReais
        Email
        Data
        Nome
        Customizado
    End Enum


    Dim TipoMascara As TipoMascara_

    <Category("SuperTextBox"), DefaultValue(True), Description("Define se o campo perde o foco automaticamente ao se apertar ENTER, Cima, Baixo ou Backspace")> _
    Public Property MudaCampoAutomatico As Boolean
        Get
            Return bMudaAuto
        End Get
        Set(value As Boolean)
            bMudaAuto = value
        End Set
    End Property


    <Category("SuperTextBox"), DefaultValue("NENHUMA"), Description("")> _
    <TypeConverter(GetType(TipoMascara_))> _
    Public Property SuperTravaErrors As Boolean
        Get
            Return bTravaErros
        End Get
        Set(value As Boolean)
            bTravaErros = value
        End Set
    End Property


    <Category("SuperTextBox"), DefaultValue("NENHUMA"), Description("")> _
    <TypeConverter(GetType(TipoMascara_))> _
    Public Property SuperUsaMascara As TipoMascara_
        Get
            Return TipoMascara
        End Get
        Set(value As TipoMascara_)
            TipoMascara = value
        End Set
    End Property

    <Category("SuperTextBox"), Description("Retorna false se o conteúdo do componente nao foi alterado")> _
    Public Property Alterado As Boolean
        Get
            Return bAlterado
        End Get
        Set(ByVal value As Boolean)
            bAlterado = value
            Me.Invalidate()
        End Set
    End Property

    <Category("SuperTextBox"), Description("Mascara: Expressao regular")> _
    Public Property SuperMascara As String
        Get
            Return mascara
        End Get
        Set(ByVal value As String)
            mascara = value
            Me.Invalidate()
        End Set
    End Property

    <Category("SuperTextBox"), Description("")> _
    Public Property SuperObrigatorio As Boolean
        Get
            Return Obrigatorio
        End Get
        Set(ByVal value As Boolean)
            Obrigatorio = value
            Me.Invalidate()
        End Set
    End Property

    <Category("SuperTextBox"), Description("")> _
    Public Property SuperTxtObrigatorio As String
        Get
            Return txtObrigatorio
        End Get
        Set(ByVal value As String)
            txtObrigatorio = value
            Me.Invalidate()
        End Set
    End Property

    <Category("SuperTextBox"), Description("")> _
    Public Property SuperTxtCorDesabilitado As Color
        Get
            Return corDesabilitado
        End Get
        Set(ByVal value As Color)
            corDesabilitado = value
            Me.Invalidate()
        End Set
    End Property

    Public Sub New()
        Try
            InitializeComponent()

            oErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components) 'Instancia o errorProvider
            oErrorProvider.ContainerControl = CType(Me.Container, ContainerControl) 'Binda o errorProvider com o objeto texto...



            Me.BackColor = corObjNaoSelecionado
            Me.Invalidate()

            colDescricaoErros = New Collection

            colDescricaoErros.Add("", TipoMascara_.NENHUMA.ToString)
            colDescricaoErros.Add("Favor digitar um número INTEIRO válido.", TipoMascara_.NumerosInteiros.ToString)
            colDescricaoErros.Add("Favor digitar um número válido.", TipoMascara_.NumerosReais.ToString)
            colDescricaoErros.Add("Favor digitar uma e-mail válido.", TipoMascara_.Email.ToString)
            colDescricaoErros.Add("Favor digitar uma data válida.", TipoMascara_.Data.ToString)
            colDescricaoErros.Add("Favor digitar um nome válido.", TipoMascara_.Nome.ToString)
            colDescricaoErros.Add("Por favor, verifique o campo digitado.", TipoMascara_.Customizado.ToString)

            bAlterado = False

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::New: " & CStr(ex.Message))
        End Try
    End Sub

    Public Sub ResetaAvisos(Optional ByVal bLimpaCampos As Boolean = False)
        Try
            If oErrorProvider.GetError(Me) <> "" Then
                oErrorProvider.SetError(Me, "")
            End If

            If bLimpaCampos = True AndAlso Me.Text <> "" Then
                Me.Text = ""
            End If

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::ResetaAvisos: " & CStr(ex.Message))
        End Try
    End Sub


    Public Function VerificaObrigatorio(Optional ByVal bZerado As Boolean = False) As Boolean

        Try

            'Nao eh obrigatorio, retorna ok
            If Obrigatorio = False Then
                Return True
            End If

            If (Me.Text.Trim) = "" Then       've se tem texto
                oErrorProvider.SetError(Me, "O campo '" & txtObrigatorio & "' é obrigatório.")
                'MsgBox("O campo '" & txtObrigatorio & "' é obrigatório.", vbOKOnly)
                S_MsgBox("O campo '" & txtObrigatorio & "' é obrigatório.", eBotoes.Ok, , , eImagens.Atencao)
                Me.Focus()
                Return False
            End If

            If (IsNumeric(Me.Text) = True) Then
                If bZerado = True AndAlso CDec(Me.Text) = 0 Then
                    oErrorProvider.SetError(Me, "O campo '" & txtObrigatorio & "' não pode ser zero.")
                    'MsgBox("O campo '" & txtObrigatorio & "' não pode ser zero.", vbOKOnly)
                    S_MsgBox("O campo '" & txtObrigatorio & "' não pode ser zero.", eBotoes.Ok, , , eImagens.Atencao)
                    Me.Focus()
                    Return False
                End If
            End If

            Return True

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::VerificaObrigatorio: " & CStr(ex.Message))
            Return False
        End Try

    End Function

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        Try
            Me.SelectAll()
            Me.BackColor = corObjSelecionado
            MyBase.OnGotFocus(e)
        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::OnGotFocus: " & CStr(ex.Message))
        End Try
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        Try
            Me.BackColor = corObjNaoSelecionado
            MyBase.OnLostFocus(e)
        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::OnLostFocus: " & CStr(ex.Message))
        End Try
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        Try

            If (e.KeyCode = Keys.Delete) Then
                bAlterado = True
            End If

            If bMudaAuto = True And _
               Me.Multiline = False Then

                If (e.KeyCode = Keys.Back And Me.Text = "") Or _
                    e.KeyCode = Keys.Up Then
                    SendKeys.Send("+{TAB}")
                End If

                If e.KeyCode = Keys.Down Or _
                    e.KeyCode = Keys.Return Then
                    SendKeys.Send("{TAB}")
                End If

            End If

            If e.KeyCode = Keys.Return And
               Me.Multiline = False Then
                e.SuppressKeyPress = True
            End If

            MyBase.OnKeyDown(e)

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::OnKeyDown: " & CStr(ex.Message))
        End Try
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Try

            'Se estiver sinalizando algum erro, limpa
            Me.ResetaAvisos()

            MyBase.OnTextChanged(e)

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::OnTextChanged: " & CStr(ex.Message))
        End Try
    End Sub

    Protected Overrides Sub OnValidating(e As System.ComponentModel.CancelEventArgs)

        'Nao eh obrigatorio, retorna ok
        If SuperUsaMascara <> TipoMascara_.NENHUMA Then

            Dim strRegexReg As String = ""

            If Me.Text.Trim <> "" Then

                Select Case SuperUsaMascara
                    Case TipoMascara_.NENHUMA
                        strRegexReg = ""
                    Case TipoMascara_.NumerosInteiros
                        strRegexReg = "^[0-9]+$"
                    Case TipoMascara_.NumerosReais
                        strRegexReg = "^[0-9]+?(.|,[0-9]+)$"
                    Case TipoMascara_.Email
                        strRegexReg = "^([\w\-]+\.)*[\w\- ]+@([\w\- ]+\.)+([\w\-]{2,3})$"
                    Case TipoMascara_.Data
                        strRegexReg = "^((0[1-9]|[12]\d)\/(0[1-9]|1[0-2])|30\/(0[13-9]|1[0-2])|31\/(0[13578]|1[02]))\/\d{4}$"
                    Case TipoMascara_.Nome
                        strRegexReg = "^([\'\.\^\~\´\`\\áÁ\\àÀ\\ãÃ\\âÂ\\éÉ\\èÈ\\êÊ\\íÍ\\ìÌ\\óÓ\\òÒ\\õÕ\\ôÔ\\úÚ\\ùÙ\\çÇaA-zZ]+)+((\s[\'\.\^\~\´\`\\áÁ\\àÀ\\ãÃ\\âÂ\\éÉ\\èÈ\\êÊ\\íÍ\\ìÌ\\óÓ\\òÒ\\õÕ\\ôÔ\\úÚ\\ùÙ\\çÇaA-zZ]+)+)?$"
                    Case TipoMascara_.Customizado
                        strRegexReg = SuperMascara
                    Case Else
                        strRegexReg = ""
                End Select

                If strRegexReg <> "" Then
                    If Regex.IsMatch(Me.Text, strRegexReg) = False Then
                        oErrorProvider.SetError(Me, colDescricaoErros(SuperUsaMascara.ToString).ToString)
                        If bTravaErros = True Then
                            S_MsgBox(colDescricaoErros(SuperUsaMascara.ToString).ToString, eBotoes.Ok, , , eImagens.Atencao)
                            Me.SelectAll()
                            e.Cancel = True
                        End If
                    End If
                End If
                'Else
                'oErrorProvider.SetError(Me, "O campo '" & txtObrigatorio & "' é obrigatório.")
            End If

        End If

        MyBase.OnValidating(e)

    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try

            bAlterado = True

            'Se estiver sinalizando algum erro, limpa
            If oErrorProvider.GetError(Me) <> "" Then
                oErrorProvider.SetError(Me, "")
            End If

            MyBase.OnKeyPress(e)

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::OnKeyPress: " & CStr(ex.Message))
        End Try

    End Sub

    Public Sub SetarTextoAnimado(ByVal strValorTexto As String)
        Try
            Me.Text = ""
            For i = 1 To strValorTexto.Length Step 1
                Me.Text = Me.Text & Mid(strValorTexto, i, 1)
                System.Threading.Thread.Sleep(40)
                System.Windows.Forms.Application.DoEvents()
            Next i

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::SetarTextoAnimado: " & CStr(ex.Message))
        End Try
    End Sub

    Function VerificaValorNumerico() As Boolean
        Try

            If (Me.Text.Trim) = "" Then       've se tem texto
                oErrorProvider.SetError(Me, "O campo '" & txtObrigatorio & "' deve ser numérico.")
                S_MsgBox("O campo '" & txtObrigatorio & "' deve ser numérico.", eBotoes.Ok, , , eImagens.Atencao)
                Me.Focus()
                Return False
            End If

            If (IsNumeric(Me.Text) = False) Then
                oErrorProvider.SetError(Me, "O campo '" & txtObrigatorio & "' deve ser numérico.")
                S_MsgBox("O campo '" & txtObrigatorio & "' deve ser numérico.", eBotoes.Ok, , , eImagens.Atencao)
                Me.Focus()
                Return False
            End If

            Return True

        Catch ex As Exception
            LogaErro("Erro em SuperTextBox::VerificaValorNumerico: " & CStr(ex.Message))
            Return False
        End Try

    End Function

End Class
