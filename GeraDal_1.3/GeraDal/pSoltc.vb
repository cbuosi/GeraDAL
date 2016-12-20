Option Strict On
Option Explicit On
Option Compare Text
Option Infer On

Imports Sonda.Util
Imports Sonda.Util.BancoDados


Public Class pSoltc

    'Operacao
    Public Shared OPERACAO As campo = New campo("OPERACAO", DbType.String, 4)
    'Estrutura da Tabela tSoltc
    Class tSoltc
        Public Shared nSoltc As campo = New campo("nSoltc", DbType.String, 21)
        Public Shared cProdt As campo = New campo("cProdt", DbType.Decimal, 5, 0)
        Public Shared cSProd As campo = New campo("cSProd", DbType.Decimal, 5, 0)
        Public Shared cTpoSoltc As campo = New campo("cTpoSoltc", DbType.Decimal, 5, 0)
        Public Shared cNomeSoltc As campo = New campo("cNomeSoltc", DbType.Decimal, 5, 0)
        Public Shared cJuncAg As campo = New campo("cJuncAg", DbType.Decimal, 5, 0)
        Public Shared cSttus As campo = New campo("cSttus", DbType.Decimal, 2, 0)
        Public Shared cSgmto As campo = New campo("cSgmto", DbType.Decimal, 5, 0)
        Public Shared eEmail As campo = New campo("eEmail", DbType.String, 300)
        Public Shared cUndNegocOrigd As campo = New campo("cUndNegocOrigd", DbType.Decimal, 3, 0)
        Public Shared nContr As campo = New campo("nContr", DbType.String, 20)
        Public Shared cCtaCorr As campo = New campo("cCtaCorr", DbType.Decimal, 9, 0)
        Public Shared nCpfCnpj As campo = New campo("nCpfCnpj", DbType.String, 11)
        Public Shared rCli As campo = New campo("rCli", DbType.String, 50)
        Public Shared cUsuarResponsavel As campo = New campo("cUsuarResponsavel", DbType.String, 7)
        Public Shared nSeq As campo = New campo("nSeq", DbType.Decimal, 2, 0)
        Public Shared cFaseEtapa As campo = New campo("cFaseEtapa", DbType.Decimal, 5, 0)
        Public Shared rRetor As campo = New campo("rRetor", DbType.String, 1)
        Public Shared rIndcdAtdmt As campo = New campo("rIndcdAtdmt", DbType.String, 1)
        Public Shared rIndcdAprvt As campo = New campo("rIndcdAprvt", DbType.String, 1)
        Public Shared rAtivoCancd As campo = New campo("rAtivoCancd", DbType.String, 1)
        Public Shared rPrior As campo = New campo("rPrior", DbType.String, 1)
        Public Shared dHoraCadto As campo = New campo("dHoraCadto", DbType.DateTime, 8)
        Public Shared cUsuar As campo = New campo("cUsuar", DbType.String, 7)
        Public Shared dHoraUltAtulz As campo = New campo("dHoraUltAtulz", DbType.DateTime, 8)
        Public Shared nConsult As campo = New campo("nConsult", DbType.String, 20)
        Public Shared rCart As campo = New campo("rCart", DbType.String, 5)
        Public Shared vOper As campo = New campo("vOper", DbType.Decimal, 18, 2)
        Public Shared nJuncao As campo = New campo("nJuncao", DbType.String, 10)
    End Class

    '01-Incluir
    Public Shared Function Incluir(ByVal _nSoltc As String,
                                   ByVal _cProdt As Decimal,
                                   ByVal _cSProd As Decimal,
                                   ByVal _cTpoSoltc As Decimal,
                                   ByVal _cNomeSoltc As Decimal,
                                   ByVal _cJuncAg As Decimal,
                                   ByVal _cSttus As Decimal,
                                   ByVal _cSgmto As Decimal,
                                   ByVal _eEmail As String,
                                   ByVal _cUndNegocOrigd As Decimal,
                                   ByVal _nContr As String,
                                   ByVal _cCtaCorr As Decimal,
                                   ByVal _nCpfCnpj As String,
                                   ByVal _rCli As String,
                                   ByVal _cUsuarResponsavel As String,
                                   ByVal _nSeq As Decimal,
                                   ByVal _cFaseEtapa As Decimal,
                                   ByVal _rRetor As String,
                                   ByVal _rIndcdAtdmt As String,
                                   ByVal _rIndcdAprvt As String,
                                   ByVal _rAtivoCancd As String,
                                   ByVal _rPrior As String,
                                   ByVal _dHoraCadto As Date,
                                   ByVal _cUsuar As String,
                                   ByVal _dHoraUltAtulz As Date,
                                   ByVal _nConsult As String,
                                   ByVal _rCart As String,
                                   ByVal _vOper As Decimal,
                                   ByVal _nJuncao As String) As Boolean

        Dim bDados As BancoDados

        Try


            bDados = New BancoDados()

            bDados.LimpaParametros()
            bDados.AdicionaParametro(OPERACAO, "INSE")
            bDados.AdicionaParametro(tSoltc.nSoltc, _nSoltc)
            bDados.AdicionaParametro(tSoltc.cProdt, _cProdt)
            bDados.AdicionaParametro(tSoltc.cSProd, _cSProd)
            bDados.AdicionaParametro(tSoltc.cTpoSoltc, _cTpoSoltc)
            bDados.AdicionaParametro(tSoltc.cNomeSoltc, _cNomeSoltc)
            bDados.AdicionaParametro(tSoltc.cJuncAg, _cJuncAg)
            bDados.AdicionaParametro(tSoltc.cSttus, _cSttus)
            bDados.AdicionaParametro(tSoltc.cSgmto, _cSgmto)
            bDados.AdicionaParametro(tSoltc.eEmail, _eEmail)
            bDados.AdicionaParametro(tSoltc.cUndNegocOrigd, _cUndNegocOrigd)
            bDados.AdicionaParametro(tSoltc.nContr, _nContr)
            bDados.AdicionaParametro(tSoltc.cCtaCorr, _cCtaCorr)
            bDados.AdicionaParametro(tSoltc.nCpfCnpj, _nCpfCnpj)
            bDados.AdicionaParametro(tSoltc.rCli, _rCli)
            bDados.AdicionaParametro(tSoltc.cUsuarResponsavel, _cUsuarResponsavel)
            bDados.AdicionaParametro(tSoltc.nSeq, _nSeq)
            bDados.AdicionaParametro(tSoltc.cFaseEtapa, _cFaseEtapa)
            bDados.AdicionaParametro(tSoltc.rRetor, _rRetor)
            bDados.AdicionaParametro(tSoltc.rIndcdAtdmt, _rIndcdAtdmt)
            bDados.AdicionaParametro(tSoltc.rIndcdAprvt, _rIndcdAprvt)
            bDados.AdicionaParametro(tSoltc.rAtivoCancd, _rAtivoCancd)
            bDados.AdicionaParametro(tSoltc.rPrior, _rPrior)
            bDados.AdicionaParametro(tSoltc.dHoraCadto, _dHoraCadto)
            bDados.AdicionaParametro(tSoltc.cUsuar, _cUsuar)
            bDados.AdicionaParametro(tSoltc.dHoraUltAtulz, _dHoraUltAtulz)
            bDados.AdicionaParametro(tSoltc.nConsult, _nConsult)
            bDados.AdicionaParametro(tSoltc.rCart, _rCart)
            bDados.AdicionaParametro(tSoltc.vOper, _vOper)
            bDados.AdicionaParametro(tSoltc.nJuncao, _nJuncao)

            bDados.Executar("pCadUsuarTela")

            If bDados.ObterUltimoErro = "" Then 'Sucesso!
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            LogaErro("Erro em " & NomeMetodo("pSoltc") & ": " & ex.Message)
            Return False
        Finally
            bDados = Nothing
        End Try

    End Function

    '02-Alterar
    Public Shared Function Alterar(ByVal _nSoltc As String,
                                   ByVal _cProdt As Decimal,
                                   ByVal _cSProd As Decimal,
                                   ByVal _cTpoSoltc As Decimal,
                                   ByVal _cNomeSoltc As Decimal,
                                   ByVal _cJuncAg As Decimal,
                                   ByVal _cSttus As Decimal,
                                   ByVal _cSgmto As Decimal,
                                   ByVal _eEmail As String,
                                   ByVal _cUndNegocOrigd As Decimal,
                                   ByVal _nContr As String,
                                   ByVal _cCtaCorr As Decimal,
                                   ByVal _nCpfCnpj As String,
                                   ByVal _rCli As String,
                                   ByVal _cUsuarResponsavel As String,
                                   ByVal _nSeq As Decimal,
                                   ByVal _cFaseEtapa As Decimal,
                                   ByVal _rRetor As String,
                                   ByVal _rIndcdAtdmt As String,
                                   ByVal _rIndcdAprvt As String,
                                   ByVal _rAtivoCancd As String,
                                   ByVal _rPrior As String,
                                   ByVal _dHoraCadto As Date,
                                   ByVal _cUsuar As String,
                                   ByVal _dHoraUltAtulz As Date,
                                   ByVal _nConsult As String,
                                   ByVal _rCart As String,
                                   ByVal _vOper As Decimal,
                                   ByVal _nJuncao As String) As Boolean

        Dim bDados As BancoDados

        Try

            bDados = New BancoDados()

            bDados.LimpaParametros()
            bDados.AdicionaParametro(OPERACAO, "ALTE")
            bDados.AdicionaParametro(tSoltc.nSoltc, _nSoltc)
            bDados.AdicionaParametro(tSoltc.cProdt, _cProdt)
            bDados.AdicionaParametro(tSoltc.cSProd, _cSProd)
            bDados.AdicionaParametro(tSoltc.cTpoSoltc, _cTpoSoltc)
            bDados.AdicionaParametro(tSoltc.cNomeSoltc, _cNomeSoltc)
            bDados.AdicionaParametro(tSoltc.cJuncAg, _cJuncAg)
            bDados.AdicionaParametro(tSoltc.cSttus, _cSttus)
            bDados.AdicionaParametro(tSoltc.cSgmto, _cSgmto)
            bDados.AdicionaParametro(tSoltc.eEmail, _eEmail)
            bDados.AdicionaParametro(tSoltc.cUndNegocOrigd, _cUndNegocOrigd)
            bDados.AdicionaParametro(tSoltc.nContr, _nContr)
            bDados.AdicionaParametro(tSoltc.cCtaCorr, _cCtaCorr)
            bDados.AdicionaParametro(tSoltc.nCpfCnpj, _nCpfCnpj)
            bDados.AdicionaParametro(tSoltc.rCli, _rCli)
            bDados.AdicionaParametro(tSoltc.cUsuarResponsavel, _cUsuarResponsavel)
            bDados.AdicionaParametro(tSoltc.nSeq, _nSeq)
            bDados.AdicionaParametro(tSoltc.cFaseEtapa, _cFaseEtapa)
            bDados.AdicionaParametro(tSoltc.rRetor, _rRetor)
            bDados.AdicionaParametro(tSoltc.rIndcdAtdmt, _rIndcdAtdmt)
            bDados.AdicionaParametro(tSoltc.rIndcdAprvt, _rIndcdAprvt)
            bDados.AdicionaParametro(tSoltc.rAtivoCancd, _rAtivoCancd)
            bDados.AdicionaParametro(tSoltc.rPrior, _rPrior)
            bDados.AdicionaParametro(tSoltc.dHoraCadto, _dHoraCadto)
            bDados.AdicionaParametro(tSoltc.cUsuar, _cUsuar)
            bDados.AdicionaParametro(tSoltc.dHoraUltAtulz, _dHoraUltAtulz)
            bDados.AdicionaParametro(tSoltc.nConsult, _nConsult)
            bDados.AdicionaParametro(tSoltc.rCart, _rCart)
            bDados.AdicionaParametro(tSoltc.vOper, _vOper)
            bDados.AdicionaParametro(tSoltc.nJuncao, _nJuncao)


            bDados.Executar("pCadUsuarTela")

            If bDados.ObterUltimoErro = "" Then 'Sucesso!
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            LogaErro("Erro em " & NomeMetodo("pSoltc") & ": " & ex.Message)
            Return False
        Finally
            bDados = Nothing
        End Try
    End Function

    '03-Excluir
    Public Shared Function Excluir(ByVal _nSoltc As String) As Boolean

        Dim bDados As BancoDados

        Try

            bDados = New BancoDados()

            bDados.LimpaParametros()
            bDados.AdicionaParametro(OPERACAO, "DELE")
            bDados.AdicionaParametro(tSoltc.nSoltc, _nSoltc)

            bDados.Executar("pCadUsuarTela")

            If bDados.ObterUltimoErro = "" Then 'Sucesso!
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            LogaErro("Erro em " & NomeMetodo("pSoltc") & ": " & ex.Message)
            Return False
        Finally
            bDados = Nothing
        End Try
    End Function

    '04-Listar
    Public Shared Function Listar(ByVal _nSoltc As String) As SuperDataSet

        Dim rsRet As SuperDataSet
        Dim bDados As BancoDados

        Try

            bDados = New BancoDados()

            bDados.LimpaParametros()
            bDados.AdicionaParametro(OPERACAO, "LIST")
            bDados.AdicionaParametro(tSoltc.nSoltc, _nSoltc)

            rsRet = bDados.Obter("pCadUsuarTela")

            Return rsRet

        Catch ex As Exception
            LogaErro("Erro em " & NomeMetodo("pSoltc") & ": " & ex.Message)
            Return Nothing
        Finally
            rsRet = Nothing
        End Try

    End Function


End Class
