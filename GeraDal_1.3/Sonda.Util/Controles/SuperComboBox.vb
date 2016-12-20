#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: SuperComboBox.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Modulo de ComboBox
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
Imports System.Windows.Forms
Imports Sonda.Util.clsMsgBox

Public Class SuperComboBox
    Inherits System.Windows.Forms.ComboBox

    Private Obrigatorio As Boolean = False
    Private txtObrigatorio As String = ""
    Private bAlterado As Boolean = False
    Dim oErrorProvider As System.Windows.Forms.ErrorProvider

    Enum PrimeiroValor
        Nada = 0
        Todos
        Selecione
    End Enum


    <Category("SuperComboBox"), Description("")>
    Public Property SuperObrigatorio As Boolean
        Get
            Return Obrigatorio
        End Get
        Set(ByVal value As Boolean)
            Obrigatorio = value
            Me.Invalidate()
        End Set
    End Property

    <Category("SuperComboBox"), Description("")>
    Public Property SuperTxtObrigatorio As String
        Get
            Return txtObrigatorio
        End Get
        Set(ByVal value As String)
            txtObrigatorio = value
            Me.Invalidate()
        End Set
    End Property

    <Category("SuperComboBox"), Description("Retorna false se o conteúdo do componente nao foi alterado")>
    Public Property Alterado As Boolean
        Get
            Return bAlterado
        End Get
        Set(ByVal value As Boolean)
            bAlterado = value
            Me.Invalidate()
        End Set
    End Property

    Public Sub New()
        Try

            InitializeComponent()

            'Instancia o errorProvider
            oErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
            'Binda o errorProvider com o objeto texto...
            oErrorProvider.ContainerControl = CType(Me.Container, ContainerControl)

        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::New: " & CStr(ex.Message))
        End Try
    End Sub

    Public Const strTODOS As String = ":: Todos ::"
    Public Const strSELECT As String = ":: Selecione ::"

    Public Structure DuplaCombo
        Dim chave As Object
        Dim descricao As String
        Public Sub New(ByVal _chave As Object, ByVal _descricao As String)
            chave = _chave
            descricao = _descricao
        End Sub
        Public Overrides Function ToString() As String
            Return Me.descricao
        End Function
    End Structure

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)

        Try


            If e.KeyCode = Keys.Back Then
                SendKeys.Send("+{TAB}")
            End If


            If e.KeyCode = Keys.Return Then
                SendKeys.Send("{TAB}")
            End If

            MyBase.OnKeyDown(e)

        Catch ex As Exception

            LogaErro("Erro em SuperComboBox::OnKeyDown: " & CStr(ex.Message))

        End Try

    End Sub

    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        Try
            Me.SelectAll()
            Me.BackColor = corObjSelecionado
            MyBase.OnGotFocus(e)
        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::OnGotFocus: " & CStr(ex.Message))
        End Try
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        Try
            Me.BackColor = corObjNaoSelecionado
            MyBase.OnLostFocus(e)
        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::OnLostFocus: " & CStr(ex.Message))
        End Try
    End Sub

    Public Sub ResetaAvisos(Optional ByVal bLimpaCampo As Boolean = False)
        Try
            'Resetou avisos, nao esta mais alterado!
            If oErrorProvider.GetError(Me) <> "" Then
                oErrorProvider.SetError(Me, "")
            End If

            Me.Alterado = False

            If bLimpaCampo = True Then
                If Me.SelectedIndex <> -1 Then
                    Me.SelectedIndex = 0
                End If
            End If

        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::ResetaAvisos: " & CStr(ex.Message))
        End Try
    End Sub

    Public Function VerificaObrigatorio(Optional ByVal bZerado As Boolean = False,
                                        Optional ByVal vlrMin As Decimal = Nothing,
                                        Optional ByVal vlrMax As Decimal = Nothing) As Boolean

        Try
            'Nao eh obrigatorio, retorna ok
            If Obrigatorio = False Then
                Return True
            End If

            Dim chaveCombo As String = Me.ObterChaveCombo()
            'If x(0) = "1" Then          'se estiver marcado com '1' (obrigatorio)
            If chaveCombo = "0" Or chaveCombo = "" Then 'Vazio ou Todos...
                oErrorProvider.SetError(Me, "O campo '" & txtObrigatorio & "' é obrigatório.")
                S_MsgBox("O campo '" & Me.SuperTxtObrigatorio & "' é obrigatório.", eBotoes.Ok, , , eImagens.Atencao)
                Me.Focus()
                Return False
                Exit Function
            End If

            Return True

        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::VerificaObrigatorio: " & CStr(ex.Message))
            Return False
        End Try

    End Function

    '----------------------------------------------------------------------------------------------------

    'Public Function PreencheComboDS(ByRef rs As SuperDataSet,
    '                                ByVal nCampoString As String,
    '                                ByVal ncampoChave As String,
    '                                Optional ByVal bZerado As Boolean = True,
    '                                Optional ByVal bTodos As Boolean = False) As Boolean

    Public Function PreencheComboDS(ByRef rs As SuperDataSet,
                            ByVal nCampoString As String,
                            ByVal ncampoChave As String,
                            Optional ByVal pValor As PrimeiroValor = PrimeiroValor.Selecione,
                            Optional ByVal valorZerado As Object = Nothing) As Boolean


        Try

            Dim i As Integer
            Dim idx As Integer
            Dim chave As Object
            Dim descricao As String
            'Dim valorZerado As Object


            LogaErro("Preenhendo ComboBox [" & Me.Name & "] - N.Registros [" & rs.TotalRegistros.ToString & "]")

            Me.DropDownStyle = ComboBoxStyle.DropDownList
            Me.Items.Clear()

            'Prepara campos Todos/Selecione
            If pValor = PrimeiroValor.Selecione Or pValor = PrimeiroValor.Todos Then

                'Dependendo do tipo (System.Type) do campo, preenche com 0 ou ""
                If valorZerado Is Nothing Then
                    If (rs.TipoDadosColuna(ncampoChave) Is GetType(Decimal)) Then
                        valorZerado = 0
                    Else
                        valorZerado = ""
                    End If
                End If

                Select Case pValor
                    Case PrimeiroValor.Nada
                        'Não adicionada nada a lista.
                    Case PrimeiroValor.Selecione
                        Me.Items.Add(New DuplaCombo(valorZerado, strSELECT))
                    Case PrimeiroValor.Todos
                        Me.Items.Add(New DuplaCombo(valorZerado, strTODOS))
                    Case Else
                        'Não adicionada nada a lista.
                End Select

            End If

            For i = 0 To rs.TotalRegistros() - 1 Step 1
                If IsNumeric(ncampoChave) Then
                    idx = CInt(Val(ncampoChave))
                    chave = rs.ValorCampo(idx, i)
                Else
                    chave = rs.ValorCampo(ncampoChave, i)
                End If

                If IsNumeric(nCampoString) Then
                    idx = CInt(Val(nCampoString))
                    descricao = CStr(rs.ValorCampo(idx, i))
                Else
                    descricao = CStr(rs.ValorCampo(nCampoString, i))
                End If

                Me.Items.Add(New DuplaCombo(chave, descricao))

            Next i

            Me.SelectedIndex = 0
            Me.Alterado = False

            Return True

        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::PreencheComboDS: " & CStr(ex.Message))
            Return False
        End Try

    End Function


    'Collection simples contendo Duplacombo já preenchida
    'Public Function PreencheComboColl(ByRef col As Collection,
    '                                  Optional ByVal bZerado As Boolean = True,
    '                                  Optional ByVal bTodos As Boolean = False) As Boolean

    Public Function PreencheComboColl(ByRef col As Collection,
                                      Optional ByVal pValor As PrimeiroValor = PrimeiroValor.Nada) As Boolean



        Try

            'LogaErro("Preenhendo ComboBox [" & Me.Name & "] - N.Registros [" & col.Count.ToString & "]")

            Me.DropDownStyle = ComboBoxStyle.DropDownList
            Me.Items.Clear()

            'Prepara campos Todos/Selecione
            Select Case pValor
                Case PrimeiroValor.Nada
                    'Não adicionada nada a lista.
                Case PrimeiroValor.Selecione
                    Me.Items.Add(New DuplaCombo(0, strSELECT))
                Case PrimeiroValor.Todos
                    Me.Items.Add(New DuplaCombo(0, strTODOS))
            End Select



            For i = 1 To col.Count Step 1
                Me.Items.Add(col.Item(i))
            Next i

            Me.SelectedIndex = 0
            Me.Alterado = False

            Return True
        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::PreencheComboColl: " & CStr(ex.Message))
            Return False
        End Try
    End Function

    Public Function ObterChaveCombo() As String
        Dim sel As DuplaCombo = Nothing
        Try
            If Me.SelectedIndex = -1 Then
                Return "0"
            Else
                sel = CType(Me.SelectedItem, DuplaCombo)
                Return CStr(sel.chave)
            End If
        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::ObterChaveCombo: " & CStr(ex.Message))
            Return "0"
        End Try
    End Function

    Public Function ObterDescicaoCombo() As String
        Dim sel As DuplaCombo = Nothing
        Try
            If Me.SelectedIndex = -1 Then
                Return ""
            Else
                sel = CType(Me.SelectedItem, DuplaCombo)
                Return CStr(sel.descricao)
            End If
        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::ObterDescicaoCombo: " & CStr(ex.Message))
            Return ""
        End Try
    End Function


    Public Function PosicionaRegistroCombo(ByVal idx As Object) As Boolean
        Try
            Dim i As Integer
            i = 0

            If IsDBNull(idx) = True Then
                Me.SelectedIndex = -1
                Return False 'Se posicionou, sai da funcao...
            End If

            For i = 0 To (Me.Items.Count - 1)
                Dim sel As DuplaCombo = CType(Me.Items(i), DuplaCombo)
                'Tomar cuidado para o tipo de dados ser igual (na chave) pois pode dar erro oculto (nao posicionar corretamente): ex: cint(4) <> cdec(4)
                If sel.chave.GetType.ToString() = idx.GetType.ToString() Then
                    If sel.chave.ToString = idx.ToString Then
                        Me.SelectedIndex = i
                        Return True 'Se posicionou, sai da funcao...
                    End If
                End If
            Next i

            Return False 'nao achou o indice passado :(

        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::PosicionaRegistroCombo: " & CStr(ex.Message))
            Return False
        End Try

    End Function

    Protected Overrides Sub OnSelectedIndexChanged(ByVal e As System.EventArgs)
        Try
            'Alterado!
            bAlterado = True

            'Se estiver sinalizando algum erro, limpa
            If oErrorProvider.GetError(Me) <> "" Then
                oErrorProvider.SetError(Me, "")
            End If

            MyBase.OnSelectedIndexChanged(e)
        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::OnSelectedIndexChanged: " & CStr(ex.Message))
        End Try
    End Sub

    Sub Adiciona(ByVal chave As Object,
                 ByVal valor As String)
        Try
            Me.Items.Add(New DuplaCombo(chave, valor))
        Catch ex As Exception
            LogaErro("Erro em SuperComboBox::Adiciona: " & CStr(ex.Message))
        End Try
    End Sub


    Sub Limpa()
        Try
            Me.DropDownStyle = ComboBoxStyle.DropDownList
            Me.Items.Clear()
            Me.Alterado = False
        Catch ex As Exception
        End Try
    End Sub
End Class
