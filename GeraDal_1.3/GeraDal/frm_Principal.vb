Option Strict On
Option Explicit On
Option Compare Text
Option Infer On

Imports System.IO
Imports Sonda.Util

Public Class frm_Principal

    Structure sAlvoGera
        Dim strServidor As String
        Dim strUsuario As String
        Dim strSenha As String
        Dim strBanco As String
        Dim strTabela As String
    End Structure

    Dim Alvo As sAlvoGera
    Dim strModeloVB As String = ""
    Dim strModeloProc As String = ""

    Enum eTipoModelo
        Proc = 1
        VB = 2
    End Enum


    Private Sub frm_Principal_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        Try

            Me.Text = "GeraDAL V. " & AppVersion()

            txtServidor.Text = "SPW5378SPBW7P"
            txtUsuario.Text = "sa"
            txtSenha.Text = "qweasd"

            AtualizaTela(False, "Pronto!")

        Catch ex As Exception
            LogaErro("Erro em frm_Principal_Load: " & ex.Message)
        End Try

    End Sub

    Private Sub cmdConectar_Click(sender As Object, e As EventArgs) Handles cmdConectar.Click

        Dim bDados As BancoDados
        Dim oDs As SuperDataSet

        Try

            If txtServidor.VerificaObrigatorio = False Then
                Exit Sub
            End If

            If txtUsuario.VerificaObrigatorio = False Then
                Exit Sub
            End If

            If txtSenha.VerificaObrigatorio = False Then
                Exit Sub
            End If


            lblStatus.Text = "Tentando conectar em [" & txtServidor.Text & "]"
            Me.Refresh()

            bDados = New BancoDados(txtServidor.Text, "master", txtUsuario.Text, txtSenha.Text, True)

            bDados.TestaConexao()


            If bDados.ObterUltimoErro = "" Then
                lblStatus.Text = "OK - " & bDados.ObterTempoComando
            Else
                AtualizaTela(False, bDados.ObterUltimoErro())
                Exit Sub
            End If

            oDs = bDados.ObterSQL("SELECT name FROM master..sysdatabases order by name")

            If oDs.TotalRegistros > 0 Then

                cmbBanco.PreencheComboDS(oDs, "name", "name", SuperComboBox.PrimeiroValor.Selecione)
                cmbBanco.Enabled = True
                cmdListaTabelas.Enabled = True
                Alvo.strServidor = txtServidor.Text
                Alvo.strUsuario = txtUsuario.Text
                Alvo.strSenha = txtSenha.Text

            Else
                cmbBanco.Limpa()
                cmbBanco.Enabled = False
                cmdListaTabelas.Enabled = False
            End If

        Catch ex As Exception
            LogaErro("Erro em cmdConectar: " & ex.Message)
            lblStatus.Text = "Erro em cmdConectar: " & ex.Message
        End Try

    End Sub

    Private Sub cmdSair_Click(sender As Object, e As EventArgs) Handles cmdSair.Click

        If clsMsgBox.S_MsgBox("Sair da aplicação?", clsMsgBox.eBotoes.SimNao, , 2, clsMsgBox.eImagens.Interrogacao, , 5) = clsMsgBox.eRet.Nao Then
            Exit Sub
        End If

        GC.Collect()
        GC.WaitForFullGCComplete()
        Application.Exit()

    End Sub

    Private Sub cmdListaTabelas_Click(sender As Object, e As EventArgs) Handles cmdListaTabelas.Click

        Try

            Dim bDados As BancoDados
            Dim rsTabelas As SuperDataSet

            If cmbBanco.VerificaObrigatorio() = False Then
                Exit Sub
            End If

            Dim strSQL As String

            lblStatus.Text = "Listando tabelas..."

            strSQL = " SELECT " & _
                     " TABLE_CATALOG as as_Banco#120, " & _
                     " TABLE_SCHEMA as as_Schema#80, " & _
                     " TABLE_NAME as as_Tabela#150, " & _
                     " TABLE_NAME as id_Tabela, " & _
                     " TABLE_TYPE as as_Tipo#100 " & _
                     " from " & _
                     " INFORMATION_SCHEMA.tables " & _
                     " order by TABLE_NAME "

            bDados = New BancoDados(Alvo.strServidor, cmbBanco.ObterChaveCombo(), Alvo.strUsuario, Alvo.strSenha)
            rsTabelas = bDados.ObterSQL(strSQL)

            If bDados.ObterUltimoErro() = "" Then
                If Not rsTabelas Is Nothing Then
                    lvTabelas.PreencheGridDS(rsTabelas, True)
                    lblStatus.Text = "OK - " & rsTabelas.InfoPesquisa
                    Alvo.strBanco = cmbBanco.ObterChaveCombo()
                End If
            Else
                lvTabelas.LimpaGrid()
                lblStatus.Text = bDados.ObterUltimoErro()
            End If
        Catch ex As Exception
            LogaErro("Erro em cmdListaTabelas_Click: " & ex.Message)
            lblStatus.Text = "Erro em cmdListaTabelas_Click: " & ex.Message
        End Try
    End Sub

    Private Sub cmdSobre_Click(sender As Object, e As EventArgs) Handles cmdSobre.Click

        clsMsgBox.S_MsgBox("Carlos Buosi - <cbuosi@gmail.com>", clsMsgBox.eBotoes.Ok, , , clsMsgBox.eImagens.Info)

    End Sub

    Private Sub lvTabelas_ItemChecked(sender As Object, e As ItemCheckedEventArgs) Handles lvTabelas.ItemChecked

        If lvTabelas.Atualizando = True Then
            Exit Sub
        End If

        If lvTabelas.ObterTotalChecados > 0 Then
            Alvo.strTabela = lvTabelas.ObterChaveS

            AtualizaTela(True, "Tabela [" & Alvo.strTabela & "] selecionada.")


        Else
            lblStatus.Text = ""
            cmdGeraProc.Enabled = False
            cmdGeraVB.Enabled = False
        End If


    End Sub

    Private Sub cmdDesconecta_Click(sender As Object, e As EventArgs) Handles cmdDesconecta.Click
        AtualizaTela(False, "Desconectado!")
    End Sub


    Public Sub AtualizaTela(ByVal bLiga As Boolean, _
                            ByVal strTexto As String)

        lblStatus.Text = strTexto
        If bLiga = False Then
            cmbBanco.Limpa()
            lvTabelas.LimpaGrid()
            Alvo.strServidor = ""
            Alvo.strUsuario = ""
            Alvo.strSenha = ""
            Alvo.strBanco = ""
            Alvo.strTabela = ""
        End If

        cmbBanco.Enabled = bLiga
        cmdListaTabelas.Enabled = bLiga
        cmdGeraProc.Enabled = bLiga
        cmdGeraVB.Enabled = bLiga

    End Sub



    Private Sub cmdGeraProc_Click(sender As Object, e As EventArgs) Handles cmdGeraProc.Click

        Dim bDados As BancoDados

        Dim Schema As String = ""
        Dim Banco As String = ""
        Dim tabela As String = ""
        Dim Procedure As String = ""
        Dim rsRet As SuperDataSet
        Dim strArquivoDestino As String = ""
        Dim i As Integer = 0

        Dim __LOOP_PARAMETROS_PROC__ As String = ""
        Dim __LOOP_NOME_CAMPOS__ As String = ""
        Dim __LOOP_NOME_PARAMETROS__ As String = ""
        Dim __CAMPO_CHAVE__ As String = ""
        Dim __CONDICAO__ As String = ""
        Dim __LOOP_SET_CAMPOS__ As String = ""
        Dim __BANCO__ As String = ""


        Dim oArquivo As StreamWriter

        Try

            For Each item As ListViewItem In lvTabelas.CheckedItems
                Banco = item.SubItems(0).Text
                Schema = item.SubItems(1).Text
                tabela = item.SubItems(2).Text
            Next

            If tabela.Substring(0, 1).ToUpper() = "T" Then
                Procedure = "p" & tabela.Substring(1, tabela.Length() - 1)
            Else
                Procedure = "p_" & tabela
            End If


            LeModelos(eTipoModelo.Proc)
            strModeloProc = strModeloProc.Replace("__NOME_TABELA__", tabela)
            strModeloProc = strModeloProc.Replace("__NOME_PROC__", Procedure)

            'Dim myStream As Stream
            Dim saveFileDialog1 As New SaveFileDialog()

            saveFileDialog1.FileName = Procedure & ".SQL"
            saveFileDialog1.Filter = "Procedimento armazenado T-SQL (*.SQL)|*.SQL"
            saveFileDialog1.FilterIndex = 2
            saveFileDialog1.RestoreDirectory = True

            If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                strArquivoDestino = saveFileDialog1.FileName
            Else
                Return
            End If

            bDados = New BancoDados(txtServidor.Text, Banco, txtUsuario.Text, txtSenha.Text, True)

            bDados.LimpaParametros()
            bDados.AdicionaParametro("objname", Schema & "." & tabela, DbType.String, 100)

            rsRet = bDados.Obter("sp_help")

            __BANCO__ = Banco
            __LOOP_PARAMETROS_PROC__ = gera__LOOP_PARAMETROS_PROC__(rsRet)
            __LOOP_NOME_CAMPOS__ = gera__LOOP_NOME_CAMPOS__(rsRet)
            __LOOP_NOME_PARAMETROS__ = gera__LOOP_NOME_PARAMETROS__(rsRet)
            __LOOP_SET_CAMPOS__ = gera__LOOP_SET_CAMPOS__(rsRet)
            __CAMPO_CHAVE__ = gera__CAMPO_CHAVE__(rsRet)
            __CONDICAO__ = gera__CONDICAO__(rsRet)


            strModeloProc = strModeloProc.Replace("__DATA_CRIACAO__", Now.ToString())
            strModeloProc = strModeloProc.Replace("__BANCO__", __BANCO__)
            strModeloProc = strModeloProc.Replace("__NOME_PROC__", Procedure)
            strModeloProc = strModeloProc.Replace("__LOOP_PARAMETROS_PROC__", __LOOP_PARAMETROS_PROC__)
            strModeloProc = strModeloProc.Replace("__LOOP_NOME_CAMPOS__", __LOOP_NOME_CAMPOS__)
            strModeloProc = strModeloProc.Replace("__LOOP_NOME_PARAMETROS__", __LOOP_NOME_PARAMETROS__)
            strModeloProc = strModeloProc.Replace("__LOOP_SET_CAMPOS__", __LOOP_SET_CAMPOS__)
            strModeloProc = strModeloProc.Replace("__CAMPO_CHAVE__", __CAMPO_CHAVE__)
            strModeloProc = strModeloProc.Replace("__CONDICAO__", __CONDICAO__)

            oArquivo = New StreamWriter(strArquivoDestino)

            oArquivo.Write(strModeloProc)

            oArquivo.Close()

            If clsMsgBox.S_MsgBox("Abrir arquivo gerado?", clsMsgBox.eBotoes.SimNao, , 1, clsMsgBox.eImagens.Interrogacao) = clsMsgBox.eRet.Sim Then
                Process.Start(strArquivoDestino)
            End If

        Catch ex As Exception
            LogaErro("Erro em " & NomeMetodo(Me) & " : " & ex.Message())
        End Try

    End Sub

    Private Sub cmdGeraVB_Click(sender As Object, e As EventArgs) Handles cmdGeraVB.Click

        Dim bDados As BancoDados

        Dim Schema As String = ""
        Dim Banco As String = ""
        Dim tabela As String = ""
        Dim Procedure As String = ""
        Dim rsRet As SuperDataSet
        Dim strArquivoDestino As String = ""
        Dim i As Integer = 0

        Dim __LOOP_ESTRUTURA__ As String = ""
        Dim __LOOP_PARM_FUNC_COMPLETO__ As String = ""
        Dim __LOOP_PARM_FUNC_CHAVE__ As String = ""

        Dim __LOOP_ADD_PARM_CHAVE__ As String = ""
        Dim __LOOP_ADD_PARM_COMPLETO__ As String = ""

        Dim oArquivo As StreamWriter

        Try

            For Each item As ListViewItem In lvTabelas.CheckedItems
                Banco = item.SubItems(0).Text
                Schema = item.SubItems(1).Text
                tabela = item.SubItems(2).Text
            Next

            If tabela.Substring(0, 1).ToUpper() = "T" Then
                Procedure = "p" & tabela.Substring(1, tabela.Length() - 1)
            Else
                Procedure = "p_" & tabela
            End If


            LeModelos(eTipoModelo.VB)
            strModeloVB = strModeloVB.Replace("__NOME_TABELA__", tabela)
            strModeloVB = strModeloVB.Replace("__NOME_PROC__", Procedure)


            'Dim myStream As Stream
            Dim saveFileDialog1 As New SaveFileDialog()

            saveFileDialog1.FileName = Procedure & ".vb"
            saveFileDialog1.Filter = "Classe Visual Basic.NET (*.vb)|*.vb"
            saveFileDialog1.FilterIndex = 2
            saveFileDialog1.RestoreDirectory = True

            If saveFileDialog1.ShowDialog() = DialogResult.OK Then
                strArquivoDestino = saveFileDialog1.FileName
            Else
                Return
            End If



            bDados = New BancoDados(txtServidor.Text, Banco, txtUsuario.Text, txtSenha.Text, True)

            bDados.LimpaParametros()
            bDados.AdicionaParametro("objname", Schema & "." & tabela, DbType.String, 100)

            rsRet = bDados.Obter("sp_help")

#If False Then
        For i = 0 To rsRet.TotalRegistros(1) - 1
            Debug.Print(rsRet.ValorCampo("11111", i, 1).ToString)
            Debug.Print(rsRet.ValorCampo("Column_name", i, 1).ToString)
            Debug.Print(rsRet.ValorCampo("Type", i, 1).ToString)
            Debug.Print(rsRet.ValorCampo("Computed", i, 1).ToString)
            Debug.Print(rsRet.ValorCampo("Length", i, 1).ToString)
            Debug.Print(rsRet.ValorCampo("Prec", i, 1).ToString)
            Debug.Print(rsRet.ValorCampo("Scale", i, 1).ToString)
        Next i
#End If

            __LOOP_ESTRUTURA__ = gera__LOOP_ESTRUTURA__(rsRet)
            __LOOP_PARM_FUNC_COMPLETO__ = gera__LOOP_PARM_FUNC_COMPLETO__(rsRet)
            __LOOP_PARM_FUNC_CHAVE__ = gera__LOOP_PARM_FUNC_CHAVE__(rsRet)
            __LOOP_ADD_PARM_COMPLETO__ = gera__LOOP_ADD_PARM_COMPLETO__(tabela, rsRet)
            __LOOP_ADD_PARM_CHAVE__ = gera__LOOP_ADD_PARM_CHAVE__(tabela, rsRet)

            strModeloVB = strModeloVB.Replace("__DATA_CRIACAO__", Now.ToString())
            strModeloVB = strModeloVB.Replace("__LOOP_ESTRUTURA__", __LOOP_ESTRUTURA__)
            strModeloVB = strModeloVB.Replace("__LOOP_PARM_FUNC_COMPLETO__", __LOOP_PARM_FUNC_COMPLETO__)
            strModeloVB = strModeloVB.Replace("__LOOP_PARM_FUNC_CHAVE__", __LOOP_PARM_FUNC_CHAVE__)
            strModeloVB = strModeloVB.Replace("__LOOP_ADD_PARM_COMPLETO__", __LOOP_ADD_PARM_COMPLETO__)
            strModeloVB = strModeloVB.Replace("__LOOP_ADD_PARM_CHAVE__", __LOOP_ADD_PARM_CHAVE__)

            oArquivo = New StreamWriter(strArquivoDestino)

            oArquivo.Write(strModeloVB)

            oArquivo.Close()

            If clsMsgBox.S_MsgBox("Abrir arquivo gerado?", clsMsgBox.eBotoes.SimNao, , 1, clsMsgBox.eImagens.Interrogacao) = clsMsgBox.eRet.Sim Then
                Process.Start(strArquivoDestino)
            End If

        Catch ex As Exception
            LogaErro("Erro em " & NomeMetodo(Me) & " : " & ex.Message())
        End Try

    End Sub

    Private Function gera__LOOP_ESTRUTURA__(p_rsRet As SuperDataSet) As String

        Dim strRet As String = ""
        Dim i As Integer

        Dim Coluna As String
        Dim Tipo As String
        Dim Len As Decimal
        Dim Prec As Decimal
        Dim Scale As Decimal

        For i = 0 To p_rsRet.TotalRegistros(1) - 1
            If i > 0 Then
                strRet &= vbNewLine
            End If

            Coluna = p_rsRet.ValorCampo("Column_name", i, 1).ToString
            Tipo = p_rsRet.ValorCampo("Type", i, 1).ToString
            Len = CDec(p_rsRet.ValorCampo("Length", i, 1))

            'Debug.Print(p_rsRet.ValorCampo("Prec", i, 1).ToString())

            If IsNumeric(p_rsRet.ValorCampo("Prec", i, 1)) = True Then
                Prec = CDec(p_rsRet.ValorCampo("Prec", i, 1))
            Else
                Prec = 0
            End If

            ''Debug.Print("SAIDA: " & p_rsRet.ValorCampo("Scale", i, 1).ToString())

            If IsNumeric(p_rsRet.ValorCampo("Scale", i, 1)) = True Then
                Scale = CDec(p_rsRet.ValorCampo("Scale", i, 1))
            Else
                Scale = 0
            End If



            'Debug.Print(rsRet.ValorCampo("Length", i, 1).ToString)
            'Debug.Print(rsRet.ValorCampo("Prec", i, 1).ToString)
            'Debug.Print(rsRet.ValorCampo("Scale", i, 1).ToString)

            strRet &= vbTab & vbTab & "Public Shared " & Coluna & " As campo = New campo(""" & Coluna & """, " & obterTipo(Tipo, Len, Prec, Scale)

        Next i

        Return strRet

    End Function

    Private Function gera__LOOP_PARM_FUNC_COMPLETO__(p_rsRet As SuperDataSet) As String

        Dim strRet As String = ""
        Dim i As Integer

        Dim Coluna As String
        Dim Tipo As String

        For i = 0 To p_rsRet.TotalRegistros(1) - 1
            If i > 0 Then
                strRet &= "," & vbNewLine & vbTab & vbTab & vbTab & vbTab & vbTab & vbTab & vbTab & vbTab & vbTab
            End If

            Coluna = p_rsRet.ValorCampo("Column_name", i, 1).ToString
            Tipo = p_rsRet.ValorCampo("Type", i, 1).ToString


#If False Then
ByVal _nSoltc As String,
ByVal _cProdt As Decimal,
ByVal _cSProd As Decimal,
ByVal _cTpoSoltc As Decimal,
ByVal _cNomeSoltc As Decimal,
ByVal _cJuncAg As Decimal,
ByVal _cSttus As Decimal,
ByVal _cSgmto As Decimal,
ByVal _eEmail As String,
ByVal _cUndNegocOrigd A
#End If

            strRet &= "ByVal _" & Coluna & " As "

            Select Case Tipo
                Case "decimal"
                    strRet &= "Decimal"
                Case "varchar"
                    strRet &= "String"
                Case "DateTime"
                    strRet &= "Date"
                Case Else
                    'Return "DESCONHECIDO!"
            End Select


        Next i

        Return strRet

    End Function

    Private Function gera__LOOP_PARM_FUNC_CHAVE__(p_rsRet As SuperDataSet) As String

        Dim strRet As String = ""

        Dim Coluna As String
        Dim Tipo As String

        Coluna = p_rsRet.ValorCampo("Column_name", 0, 1).ToString
        Tipo = p_rsRet.ValorCampo("Type", 0, 1).ToString

        strRet &= "ByVal _" & Coluna & " As "

        Select Case Tipo
            Case "decimal"
                strRet &= "Decimal"
            Case "varchar"
                strRet &= "String"
            Case "DateTime"
                strRet &= "Date"
            Case Else
                'Return "DESCONHECIDO!"
        End Select


        Return strRet

    End Function

    Private Function obterTipo(ByVal strTipo As String,
                               ByVal Len As Decimal,
                               ByVal Prec As Decimal,
                               ByVal Scale As Decimal) As String

        Dim strRet As String = ""

        'Debug.Print(rsRet.ValorCampo("Length", i, 1).ToString)
        'Debug.Print(rsRet.ValorCampo("Prec", i, 1).ToString)
        'Debug.Print(rsRet.ValorCampo("Scale", i, 1).ToString)

        'Public Shared dHoraUltAtulz As campo = New campo("dHoraUltAtulz", DbType.DateTime, 8)
        'Public Shared nConsult As campo = New campo("nConsult", DbType.String, 20)
        'Public Shared rCart As campo = New campo("rCart", DbType.String, 5)
        'Public Shared vOper As campo = New campo("vOper", DbType.Decimal, 18, 2)

        Select Case strTipo
            Case "decimal"
                strRet = "DbType.Decimal, " & Prec.ToString() & ", " & Scale.ToString() & ")"
            Case "varchar"
                strRet = "DbType.String, " & Len.ToString() & ")"
            Case "DateTime"
                strRet = "DbType.DateTime, 8,0)"
            Case Else
                Return "DESCONHECIDO!"
        End Select

        Return strRet

    End Function


    Private Function gera__LOOP_ADD_PARM_COMPLETO__(ByVal NomeTabela As String,
                                                    ByVal p_rsRet As SuperDataSet) As String

        Dim strRet As String = ""
        Dim i As Integer

        Dim Coluna As String

        For i = 0 To p_rsRet.TotalRegistros(1) - 1
            If i > 0 Then
                strRet &= vbNewLine
            End If

            Coluna = p_rsRet.ValorCampo("Column_name", i, 1).ToString
            strRet &= vbTab & vbTab & vbTab & "bDados.AdicionaParametro(" & NomeTabela & "." & Coluna & ", _" & Coluna & ")"
        Next i

        Return strRet

    End Function

    Private Function gera__LOOP_ADD_PARM_CHAVE__(ByVal NomeTabela As String,
                                                    ByVal p_rsRet As SuperDataSet) As String

        Dim strRet As String = ""

        Dim Coluna As String

        Coluna = p_rsRet.ValorCampo("Column_name", 0, 1).ToString
        strRet &= vbTab & vbTab & vbTab & "bDados.AdicionaParametro(" & NomeTabela & "." & Coluna & ", _" & Coluna & ")"

        Return strRet

    End Function

    Private Function gera__LOOP_PARAMETROS_PROC__(ByVal rsRet As SuperDataSet) As String

#If False Then
    @OPERACAO			char(4),
	@cUsuar				decimal(6)	= null,
	@cMatrFunclEmpr		varchar(10)	= null,
	@iUsuar				varchar(60) = null,
	@rEmailUsuar 		varchar(40) = null,
	@nFoneUsuar			decimal(20) = null,
	@cLogin				varchar(20) = null,
	@iSetorEmpr 		varchar(60) = null,
	@rJuncUsuar 		varchar(60) = null,
	@cPrfilAcsso		decimal(1)	= null,
	@cEstAtivo			decimal(1)  = null,
	@cUsuarUltAtulz		decimal(6)	= null
#End If
        Dim strRet As String = "@OPERACAO char(4)"
        Dim strTam As String
        Dim i As Integer

        Dim Coluna As String
        Dim Tipo As String

        For i = 0 To rsRet.TotalRegistros(1) - 1


            Tipo = rsRet.ValorCampo("Type", i, 1).ToString

            Select Case Tipo
                Case "decimal"
                    strTam = "(" & rsRet.ValorCampo("Prec", i, 1).ToString().Trim() & "," & rsRet.ValorCampo("Scale", i, 1).ToString().Trim() & ") "
                Case "varchar"
                    strTam = "(" & rsRet.ValorCampo("Length", i, 1).ToString().Trim() & ")"
                Case "DateTime"
                    strTam = ""
                Case Else
                    strTam = ""
            End Select

            strRet &= "," & vbNewLine
            Coluna = rsRet.ValorCampo("Column_name", i, 1).ToString
            strRet &= vbTab & "@" & Coluna & " " & Tipo & strTam & " = NULL"


        Next i

        Return strRet

    End Function

    Private Function gera__LOOP_NOME_CAMPOS__(ByVal rsRet As SuperDataSet,
                                              Optional ByVal par As Boolean = False) As String

        Dim strRet As String = ""
        Dim i As Integer
        Dim strPar As String

        If par = True Then
            strPar = "@"
        Else
            strPar = ""
        End If


        For i = 0 To rsRet.TotalRegistros(1) - 1

            If i > 0 Then
                strRet &= "," & vbNewLine
            End If


            strRet &= strPar & rsRet.ValorCampo("Column_name", i, 1).ToString().Trim()

        Next i

        Return strRet
    End Function

    Private Function gera__LOOP_NOME_PARAMETROS__(ByVal rsRet As SuperDataSet) As String
        Return gera__LOOP_NOME_CAMPOS__(rsRet, True)
    End Function

    Private Function gera__CAMPO_CHAVE__(rsRet As SuperDataSet) As String
        Return rsRet.ValorCampo("Identity", 0, 2).ToString().Trim()
    End Function

    Private Function gera__CONDICAO__(rsRet As SuperDataSet) As String
        Return rsRet.ValorCampo("Identity", 0, 2).ToString().Trim() & " = @" & rsRet.ValorCampo("Identity", 0, 2).ToString().Trim()
    End Function

    Private Function gera__LOOP_SET_CAMPOS__(ByVal rsRet As SuperDataSet) As String

        Dim strRet As String = ""
        Dim i As Integer

        For i = 0 To rsRet.TotalRegistros(1) - 1

            If i > 0 Then
                strRet &= "," & vbNewLine
            End If


            strRet &= rsRet.ValorCampo("Column_name", i, 1).ToString().Trim() & " = @" & rsRet.ValorCampo("Column_name", i, 1).ToString().Trim()

        Next i

        Return strRet

    End Function

    Private Sub LeModelos(ByVal TipoModelo As eTipoModelo)

        Dim oStreamReader As StreamReader

        Try


            strModeloVB = ""
            strModeloProc = ""

            If TipoModelo = eTipoModelo.Proc Then
                oStreamReader = New StreamReader("ModeloProc.txt")
                strModeloProc = oStreamReader.ReadToEnd()
            ElseIf TipoModelo = eTipoModelo.VB Then
                oStreamReader = New StreamReader("ModeloVB.txt")
                strModeloVB = oStreamReader.ReadToEnd()
            Else
                Exit Sub
            End If

            oStreamReader.Close()
            oStreamReader.Dispose()
            oStreamReader = Nothing

        Catch ex As Exception
            LogaErro("Erro em " & NomeMetodo(Me) & " : " & ex.Message())
        End Try

    End Sub


End Class
