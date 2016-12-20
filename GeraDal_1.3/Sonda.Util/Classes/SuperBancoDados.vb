#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: SuperBancoDados.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Modulo de banco de dados (SQL Server 2000+).
' Uso..........: Não se aplica
' Produto......: CCON
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

Imports System.Data.Common
Imports System.Data.SqlClient

Public Class BancoDados
    Implements IDisposable

    Private Const MAX_PARAM = 300 'No maximo 300 parametros por procedure!
    Private Const SQL_SERVER_DEFAULT_PORT = 1433 'Caso o windows do usuario esteja com a configuracao fodida...
    Private Const CMD_TIME_OUT = 300 '5 fodendo minutos!
    Private Const BULK_INSERT_BATCH_SIZE = 1000 'Insere lotes de 1000 registros (evita time out)
    Private Const BULK_INSERT_TIME_OUT = 1200 '20 minutos
    Private CURSOR_OCUPADO As System.Windows.Forms.Cursor = Cursors.WaitCursor

    Private strConexao As String = ""
    Private strServer As String = ""
    Private strDatabase As String = ""
    Private strUserID As String = ""
    Private strPassword As String = ""
    Private strTabelaDestino As String = ""
    Private strTempoComando As String = ""

    Dim ParametrosProc() As DbParameter

    Private strCodErro As String = ""

    Structure campo
        Dim nome As String
        Dim tipo As DbType
        Dim tamanho As Integer
        Dim tamEscala As Integer

        Public Sub New(ByVal _nome As String,
                       ByVal _tipo As DbType,
                       ByVal _tamanho As Integer,
                       Optional ByVal _tamEscala As Integer = 0)
            nome = _nome
            tipo = _tipo
            tamanho = _tamanho
            tamEscala = _tamEscala
        End Sub

        Public Overrides Function ToString() As String
            Return Me.nome
        End Function

    End Structure



    Public Sub New(ByVal _strServer As String,
                   ByVal _strDatabase As String,
                   ByVal _strUserID As String,
                   ByVal _strPassword As String,
                   Optional ByVal _bLogaErro As Boolean = True)

        strServer = _strServer
        strDatabase = _strDatabase
        strUserID = _strUserID
        strPassword = _strPassword
        LimpaParametros()
    End Sub

    Sub New()
        strServer = ObterConfig("Servidor")
        strDatabase = ObterConfig("Banco")
        strUserID = ObterConfig("Usuario")
        strPassword = Decripta(ObterConfig("Senha"))
        LimpaParametros()
    End Sub

    Public Property TabelaDestino() As String
        Get
            Return strTabelaDestino
        End Get
        Set(ByVal value As String)
            strTabelaDestino = value
        End Set
    End Property

    Public Sub LimpaParametros()
        Try
            ReDim ParametrosProc(0)
        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::LimpaParametros: " & ex.Message)
            strCodErro = "SuperBancoDados::LimpaParametros: " & ex.Message
        End Try
    End Sub

    Public Sub AdicionaParametro(ByVal _Campo As campo,
                                 ByVal valor As Object)
        Try
            Dim tamArray As Integer
            tamArray = ParametrosProc.Length - 1

            If (tamArray + 1) > MAX_PARAM Then
                LogaErro("Erro em SuperBancoDados::Muitos parametros! :)")
                Exit Sub
            End If

            ParametrosProc(tamArray) = New SqlParameter()
            ParametrosProc(tamArray).ParameterName = _Campo.nome
            ParametrosProc(tamArray).SourceVersion = DataRowVersion.Current
            ParametrosProc(tamArray).SourceColumn = String.Empty
            ParametrosProc(tamArray).SourceColumnNullMapping = False
            ParametrosProc(tamArray).Size = _Campo.tamanho
            ParametrosProc(tamArray).Direction = ParameterDirection.Input
            ParametrosProc(tamArray).DbType = _Campo.tipo
            ParametrosProc(tamArray).Value = valor

            ReDim Preserve ParametrosProc(tamArray + 1)

        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::AdicionaParametro(1): " & ex.Message)
            strCodErro = "SuperBancoDados::AdicionaParametro(1): " & ex.Message
        End Try
    End Sub



    Public Sub AdicionaParametro(ByVal nome As String,
                                 ByVal valor As Object,
                                 ByVal tipo As DbType,
                                 ByVal tamanho As Integer,
                                 Optional ByVal tamEscala As Integer = 0)
        Try
            Dim tamArray As Integer
            tamArray = ParametrosProc.Length - 1

            If (tamArray + 1) > MAX_PARAM Then
                LogaErro("Erro em SuperBancoDados::Muitos parametros! :)")
                Exit Sub
            End If

            ParametrosProc(tamArray) = New SqlParameter()
            ParametrosProc(tamArray).ParameterName = nome
            ParametrosProc(tamArray).SourceVersion = DataRowVersion.Current
            ParametrosProc(tamArray).SourceColumn = String.Empty
            ParametrosProc(tamArray).SourceColumnNullMapping = False
            ParametrosProc(tamArray).Size = tamanho
            ParametrosProc(tamArray).Direction = ParameterDirection.Input
            ParametrosProc(tamArray).DbType = tipo
            ParametrosProc(tamArray).Value = valor

            ReDim Preserve ParametrosProc(tamArray + 1)

        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::AdicionaParametro(2): " & ex.Message)
            strCodErro = "SuperBancoDados::AdicionaParametro(2): " & ex.Message
        End Try
    End Sub

    Private Function ObterConnectionString() As String

        Try

            Return "SERVER=" & strServer & _
                   "," & SQL_SERVER_DEFAULT_PORT & _
                   ";DataBase=" & strDatabase & _
                   ";User Id=" & strUserID & _
                   ";Password=" & strPassword & ";"

        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::ObterConnectionString: " & ex.Message)
            strCodErro = "SuperBancoDados::ObterConnectionString: " & ex.Message
            Return ""
        End Try
    End Function

    Public Function Obter(ByVal txtProcedure As String) As SuperDataSet

        Dim con As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim dap As SqlDataAdapter = Nothing

        Dim oRelogio As Stopwatch = Nothing
        Dim oDataSet As SuperDataSet = Nothing

        Try

            Cursor.Current = CURSOR_OCUPADO

            strCodErro = ""
            oDataSet = New SuperDataSet
            oRelogio = New Stopwatch

            oRelogio.Start()

            If ParametrosProc(0) Is Nothing Then
                LogaErro(">>>>>>INICIO " & txtProcedure & "[SEM PARAMETROS]...")
            Else
                LogaErro(">>>>>>INICIO " & txtProcedure & "[" & ParametrosProc(0).Value.ToString & "]...")
            End If

            con = New SqlConnection(ObterConnectionString())
            cmd = New SqlCommand(txtProcedure, con)
            cmd.CommandType = CommandType.StoredProcedure

            For Each DbParameter In ParametrosProc
                If Not DbParameter Is Nothing Then
                    cmd.Parameters.Add(DbParameter)
                End If
            Next

            dap = New SqlDataAdapter(txtProcedure, con)
            cmd.CommandTimeout = CMD_TIME_OUT
            dap.SelectCommand = cmd

            con.Open()
            dap.Fill(oDataSet)
            oRelogio.Stop()
            oDataSet.InfoPesquisa = "Quantidade de registro(s): " & oDataSet.TotalRegistros().ToString() & ". Tempo: " & (oRelogio.ElapsedMilliseconds / 1000).ToString & " segundo(s)."
            strTempoComando = (oRelogio.ElapsedMilliseconds / 1000).ToString & " segundo(s)."
            LogaErro(">>>>>>FIM    OK! Tempo exec: [" & (oRelogio.ElapsedMilliseconds / 1000).ToString & "] segs. Registros: [" & oDataSet.TotalRegistros & "] registro(s) <<<<<<FIM")

            con.Close()

            Return oDataSet

        Catch ex As SqlException
            For i = 0 To ex.Errors.Count - 1
                strCodErro += " Message: " & ex.Errors(i).Message & " Line#:" & ex.Errors(i).LineNumber.ToString & " Src:" & ex.Errors(i).Source & " Proc:" & ex.Errors(i).Procedure
            Next i
            LogaErro("Erro em SuperBancoDados::Obter: " & strCodErro)
            oDataSet.InfoPesquisa = strCodErro
            Return Nothing
        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::Obter: [" & strServer & "] " & ex.Message)
            strCodErro = "SuperBancoDados::Obter: [" & strServer & "] " & ex.Message
            oDataSet.InfoPesquisa = strCodErro
            Return Nothing
        Finally
            If Not con Is Nothing Then
                con.Close()
                con.Dispose()
                con = Nothing
            End If
            If Not oDataSet Is Nothing Then
                oDataSet.Dispose()
                oDataSet = Nothing
            End If
            If Not dap Is Nothing Then
                dap.Dispose()
                dap = Nothing
            End If
            If Not cmd Is Nothing Then
                cmd.Dispose()
                cmd = Nothing
            End If
            oRelogio = Nothing
        End Try
    End Function

    Public Function Executar(ByVal txtProcedure As String) As Boolean

        Dim con As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim oRelogio As Stopwatch = Nothing

        Try

            Cursor.Current = CURSOR_OCUPADO

            strCodErro = ""

            oRelogio = New Stopwatch

            oRelogio.Start()

            If ParametrosProc(0) Is Nothing Then
                LogaErro(">>>>>>INICIO " & txtProcedure & "[SEM PARAMETROS]...")
            Else
                LogaErro(">>>>>>INICIO " & txtProcedure & "[" & ParametrosProc(0).Value.ToString & "]...")
            End If

            con = New SqlConnection(ObterConnectionString())
            cmd = New SqlCommand(txtProcedure, con)
            cmd.CommandType = CommandType.StoredProcedure

            For Each DbParameter In ParametrosProc
                If Not DbParameter Is Nothing Then
                    cmd.Parameters.Add(DbParameter)
                End If
            Next

            con.Open()

            cmd.CommandTimeout = CMD_TIME_OUT
            cmd.ExecuteNonQuery()
            strTempoComando = (oRelogio.ElapsedMilliseconds / 1000).ToString & " segundo(s)."
            LogaErro(">>>>>>FIM    OK! Tempo exec: [" & (oRelogio.ElapsedMilliseconds / 1000).ToString & "] segs. <<<<<<FIM")

            con.Close()

            Return True

        Catch ex As SqlException
            For i = 0 To ex.Errors.Count - 1
                strCodErro += "ID:[" & ex.Errors(i).Number.ToString & "] Message: " & ex.Errors(i).Message & " Line#:" & ex.Errors(i).LineNumber.ToString & " Src:" & ex.Errors(i).Source & " Proc:" & ex.Errors(i).Procedure
            Next i
            LogaErro("Erro em SuperBancoDados::Obter: " & strCodErro)
            Return False
        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::Obter: [" & strServer & "] " & ex.Message)
            strCodErro = "SuperBancoDados::Obter: [" & strServer & "] " & ex.Message
            Return False
        Finally
            If Not con Is Nothing Then
                con.Dispose()
                con = Nothing
            End If

            If Not cmd Is Nothing Then
                cmd.Dispose()
                cmd = Nothing
            End If
            oRelogio = Nothing
        End Try

    End Function


    Public Function ObterSQL(ByVal txtSQL As String) As SuperDataSet

        Dim con As SqlConnection = Nothing
        Dim cmd As SqlCommand = Nothing
        Dim dap As SqlDataAdapter = Nothing

        Dim oRelogio As Stopwatch = Nothing
        Dim oDataSet As SuperDataSet = Nothing

        Try

            Cursor.Current = CURSOR_OCUPADO

            strCodErro = ""
            oDataSet = New SuperDataSet
            oRelogio = New Stopwatch

            oRelogio.Start()

            LogaErro(">>>>>>INICIO QUERY")

            con = New SqlConnection(ObterConnectionString())
            cmd = New SqlCommand(txtSQL, con)
            cmd.CommandType = CommandType.Text

            For Each DbParameter In ParametrosProc
                If Not DbParameter Is Nothing Then
                    cmd.Parameters.Add(DbParameter)
                End If
            Next

            dap = New SqlDataAdapter(txtSQL, con)
            cmd.CommandTimeout = CMD_TIME_OUT
            dap.SelectCommand = cmd

            con.Open()
            dap.Fill(oDataSet)
            oRelogio.Stop()
            oDataSet.InfoPesquisa = "Quantidade de registro(s): " & oDataSet.TotalRegistros().ToString() & ". Tempo: " & (oRelogio.ElapsedMilliseconds / 1000).ToString & " segundo(s)."
            strTempoComando = (oRelogio.ElapsedMilliseconds / 1000).ToString & " segundo(s)."
            LogaErro(">>>>>>FIM    OK! Tempo exec: [" & (oRelogio.ElapsedMilliseconds / 1000).ToString & "] segs. Registros: [" & oDataSet.TotalRegistros & "] registro(s) <<<<<<FIM")

            con.Close()

            Return oDataSet

        Catch ex As SqlException
            For i = 0 To ex.Errors.Count - 1
                strCodErro += " Message: " & ex.Errors(i).Message & " Line#:" & ex.Errors(i).LineNumber.ToString & " Src:" & ex.Errors(i).Source & " Proc:" & ex.Errors(i).Procedure
            Next i
            LogaErro("Erro em SuperBancoDados::Obter: " & strCodErro)
            oDataSet.InfoPesquisa = strCodErro
            Return Nothing
        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::Obter: [" & strServer & "] " & ex.Message)
            strCodErro = "SuperBancoDados::Obter: [" & strServer & "] " & ex.Message
            oDataSet.InfoPesquisa = strCodErro
            Return Nothing
        Finally
            If Not con Is Nothing Then
                con.Close()
                con.Dispose()
                con = Nothing
            End If
            If Not oDataSet Is Nothing Then
                oDataSet.Dispose()
                oDataSet = Nothing
            End If
            If Not dap Is Nothing Then
                dap.Dispose()
                dap = Nothing
            End If
            If Not cmd Is Nothing Then
                cmd.Dispose()
                cmd = Nothing
            End If
            oRelogio = Nothing
        End Try
    End Function


    Public Function ObterUltimoErro() As String
        Return strCodErro
    End Function

    Public Function TestaConexao() As Boolean

        Dim con As SqlConnection = Nothing
        Dim oRelogio As Stopwatch = Nothing

        Try

            strCodErro = ""
            Cursor.Current = CURSOR_OCUPADO

            oRelogio = New Stopwatch

            oRelogio.Start()

            LogaErro(">>>>>>INICIO TestaConexao")

            con = New SqlConnection(ObterConnectionString())
            con.Open()
            oRelogio.Stop()
            strTempoComando = (oRelogio.ElapsedMilliseconds / 1000).ToString & " segundo(s)."
            LogaErro(">>>>>>FIM    OK! Tempo exec: [" & (oRelogio.ElapsedMilliseconds / 1000).ToString & "] segs. <<<<<<FIM")

            con.Close()

            Return True

        Catch ex As SqlException
            For i = 0 To ex.Errors.Count - 1
                strCodErro += " Message: " & ex.Errors(i).Message & " Line#:" & ex.Errors(i).LineNumber.ToString & " Src:" & ex.Errors(i).Source & " Proc:" & ex.Errors(i).Procedure
            Next i
            LogaErro("Erro em SuperBancoDados::Obter: " & strCodErro)
            Return False
        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::Obter: [" & strServer & "] " & ex.Message)
            strCodErro = "Conexao em [" & strServer & "\" & strDatabase & "] Usuário:[" & strUserID & "] ERRO: " & ex.Message
            Return False
        Finally
            If Not con Is Nothing Then
                con.Close()
                con.Dispose()
            End If
            con = Nothing
            oRelogio = Nothing
        End Try
    End Function

    Public Function InserirDataTableLote(ByVal oDataTable As SuperDataTable) As Boolean

        Dim connection As SqlConnection = Nothing
        Dim oRelogio As Stopwatch = Nothing
        Dim oSqlBulkCopy As SqlBulkCopy = Nothing

        Try
            strCodErro = ""
            Cursor.Current = CURSOR_OCUPADO
            oRelogio = New Stopwatch
            oRelogio.Start()
            LogaErro(">>>>>>INICIO InserirDataTableLote. Tabela destino: " & strTabelaDestino)
            connection = New SqlConnection(ObterConnectionString())
            connection.Open()
            oSqlBulkCopy = New SqlBulkCopy(connection)

            'Adiciona manipulador 
            AddHandler oSqlBulkCopy.SqlRowsCopied, AddressOf OnRegistrosCopiados

            oSqlBulkCopy.DestinationTableName = strTabelaDestino
            oSqlBulkCopy.BatchSize = BULK_INSERT_BATCH_SIZE
            oSqlBulkCopy.BulkCopyTimeout = BULK_INSERT_TIME_OUT
            oSqlBulkCopy.WriteToServer(oDataTable)
            oRelogio.Stop()
            strTempoComando = (oRelogio.ElapsedMilliseconds / 1000).ToString & " segundo(s)."
            LogaErro("...OK! Tempo exec: [" & (oRelogio.ElapsedMilliseconds / 1000).ToString & "] segs. <<<<<<FIM")
            Return True
        Catch ex As SqlException
            For i = 0 To ex.Errors.Count - 1
                strCodErro += " Message: " & ex.Errors(i).Message & " Line#:" & ex.Errors(i).LineNumber.ToString & " Src:" & ex.Errors(i).Source & " Proc:" & ex.Errors(i).Procedure
            Next i
            LogaErro("Erro em SuperBancoDados::Obter: " & strCodErro)
            Return False
        Catch ex As Exception
            LogaErro("Erro em SuperBancoDados::Obter: [" & strServer & "] " & ex.Message)
            strCodErro = "SuperBancoDados::Obter: [" & strServer & "] " & ex.Message
            Return False
        Finally
            If Not connection Is Nothing Then
                connection.Close()
                connection.Dispose()
                connection = Nothing
            End If
            If Not oSqlBulkCopy Is Nothing Then
                oSqlBulkCopy.Close()
                oSqlBulkCopy = Nothing
            End If
            oRelogio = Nothing
        End Try

    End Function

    Private Sub OnRegistrosCopiados(ByVal sender As Object, ByVal args As SqlRowsCopiedEventArgs)
        Debug.Print("Registros copiados: {0}", args.RowsCopied)
    End Sub

    Public Function ObterTempoComando() As String
        Return strTempoComando
    End Function

#Region "___DISPOSE___"
    Private disposed As Boolean = False
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not (disposed) Then
            If disposing Then
                Dim i As Integer = 0
                'VER ATEH ONDE VAI O INDICE DE PARAM (UBOUND)
                For i = 0 To ParametrosProc.Length - 1
                    ParametrosProc(i) = Nothing
                Next
                ParametrosProc = Nothing
                i = Nothing
                strConexao = Nothing
                strServer = Nothing
                strDatabase = Nothing
                strUserID = Nothing
                strPassword = Nothing
                strCodErro = Nothing
            End If
        End If
        disposed = True
    End Sub
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub
#End Region


End Class