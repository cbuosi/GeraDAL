#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: Util.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Utilitarios
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

Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports Sonda.Util.clsMsgBox
Imports System.Text
Imports System.Xml

Module Util

    Private xmlConfig As XmlDocument = Nothing

    Public Const strTituloApp As String = "GeraDAL"

    Public Const strOk As String = "&OK"
    Public Const strCancela As String = "&CANCELA"
    Public Const strSim As String = "&SIM"
    Public Const strNao As String = "&NÃO"
    Public vMsgBox As eRet

    Const bLogaErro As Boolean = True

    Public strCaminhoLog As String = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

    Public corObjSelecionado As Color = Color.FromArgb(255, 251, 206)
    Public corObjNaoSelecionado As Color = Color.White
    Public corObjBorda As Color = Color.Blue

    Public corSelecionadaDentro As Color = Color.FromArgb(177, 153, 92)
    Public corSelecionadaFora As Color = Color.FromArgb(230, 194, 124)

    Public corDesselecionada As Color = Color.FromArgb(127, 157, 185)

    Public corGrid1 As Color = Color.Transparent
    Public corGrid2 As Color = Color.FromArgb(240, 240, 240)
    Public corObjDisabled As Color = Color.FromArgb(213, 220, 232)

    Dim oCrypto As New clsCrypto()

    Public Function Encripta(ByVal strTexto As String) As String
        Try
            Return oCrypto.Encripta(strTexto)
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function Decripta(ByVal strTexto As String) As String
        Try
            Return oCrypto.DeCripta(strTexto)
        Catch ex As Exception
            Return ""
        End Try
    End Function


    Public Sub LogaErro(ByVal texto As String,
                        Optional bForcaLog As Boolean = False)
#If True Then
        Dim hArq As StreamWriter = Nothing

        Try

            If bLogaErro = False And bForcaLog = False Then
                Exit Sub
            End If

            strCaminhoLog = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "GeraDAL_Log")

            hArq = New StreamWriter(strCaminhoLog &
                                    "\Log_" &
                                    Format(Now.Year, "0000") &
                                    Format(Now.Month, "00") &
                                    Format(Now.Day, "00") & "_" &
                                    ObterNomeMaquina() & "_" &
                                    ObterNomeUsuarioLogado() & ".txt",
                                    True,
                                    Encoding.Default)


            hArq.AutoFlush = True
            hArq.WriteLine(Format(Now().Hour, "00") & ":" & Format(Now().Minute, "00") & ":" & Format(Now().Second, "00") & "." & Format(Now().Millisecond, "0000") & "-UTL-" & texto)
            Debug.Print(texto)
            hArq.Close()
        Catch ex As Exception
            Debug.Print("Erro em LogaErro: " & ex.Message)
        Finally
            If Not hArq Is Nothing Then
                hArq.Dispose()
                hArq = Nothing
            End If
        End Try
#End If
    End Sub

    Public Function ObterNomeUsuarioLogado() As String
        Try
            Return Environment.UserName
        Catch ex As Exception
            LogaErro("Erro em Util::obterNomeUsuarioLogado: " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Function ObterNomeMaquina() As String
        Try
            Return Environment.MachineName
        Catch ex As Exception
            LogaErro("Erro em Util::obterNomeMaquina: " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Sub ApagaArquivo(ByVal strArquivo As String)
        File.Delete(strArquivo)
    End Sub


    Public Function ExisteArquivo(ByVal strArquivo As String) As Boolean
        Return File.Exists(strArquivo)
    End Function

    Public Function ObterConfig(ByVal chave As String) As String
        Try

#If False Then
    Conteudo do Config.xml
    -------------------------------------------------------
    <?xml version="1.0" encoding="utf-8" ?>
    <GCOR>
      <Servidor valor="SPW5378SPBW7P" />
      <Usuario  valor="sa" />
      <Senha    valor="hUVg+BkcUhk=" />
      <Banco    valor="GCor" />
    </GCOR>
    -------------------------------------------------------
#End If

            If xmlConfig Is Nothing Then
                xmlConfig = New XmlDocument()
                xmlConfig.Load("Config.xml")
            End If

            Return xmlConfig.DocumentElement.SelectSingleNode("//GeraDAL").SelectSingleNode("//" & chave).Attributes.ItemOf("valor").InnerText

            'Return My.Settings(chave).ToString
        Catch ex As Exception
            LogaErro("Erro em Util::GetIniString: " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Function AppVersion() As String
        Try
            Return Application.ProductVersion

            'Application.p
        Catch ex As Exception
            LogaErro("Erro em Util::AppVersion: " & CStr(ex.ToString()))
            Return "Erro em AppVersion()"
        End Try
    End Function

    Public Function FormataTrunca(ByVal dValor As Decimal,
                                  ByVal nTam As Integer) As String
        Try

            Dim strValor As String
            Dim Formato As String

            Formato = "{0:" & Replicar("0", nTam) & "}" '=> {0:000000000}
            strValor = String.Format(Formato, dValor) 'OK

            If strValor.Length > nTam Then '0000000001
                strValor = strValor.Substring(strValor.Length - nTam, nTam)
            End If

            Return strValor
        Catch ex As Exception
            LogaErro("Erro em Util::FormataTrunca: " & CStr(ex.ToString()))
            Return ""
        End Try

    End Function

    Private Function Replicar(ByVal str As String, ByVal Times As Integer) As String
        Try

            Dim ret As String = ""
            For i As Integer = 1 To Times
                ret += str
            Next
            Return ret
        Catch ex As Exception
            LogaErro("Erro em Util::Replicar: " & CStr(ex.ToString()))
            Return ""
        End Try
    End Function


    Public Function fncFormataString(ByVal par1 As Object,
                                     ByVal tamanho As Decimal,
                                     ByVal caracter As String) As String

        Try

            Dim strAux As String

            If par1 Is Nothing Then
                Return Replicar(caracter, CInt(tamanho))
            End If

            If IsNumeric(caracter) = True Then
                strAux = Replicar(caracter, CInt(tamanho)) + CStr(par1).Replace(".", "")
                Return strAux.Substring(CInt(strAux.Length - tamanho), CInt(tamanho))
            Else
                Return par1.ToString.Trim() & Replicar(caracter, CInt(tamanho - par1.ToString.Length))
            End If

        Catch ex As Exception
            LogaErro("Erro em Util::fncFormataString: " & CStr(ex.ToString()))
            Return ""
        End Try

    End Function


End Module
