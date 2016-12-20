Option Explicit On
Option Strict On

Imports OfficeOpenXml
Imports System.IO
Imports System.Drawing
Imports Sonda.Util.clsMsgBox
Imports OfficeOpenXml.Style
Imports OfficeOpenXml.Drawing

Public Class SuperXLS

    Structure sPropriedadeCelula

        Enum eAlinhamento
            General = 0
            Left = 1
            Center = 2
            CenterContinuous = 3
            Right = 4
            Fill = 5
            Distributed = 6
            Justify = 7
        End Enum

        Sub New(ByVal _Linha As Integer,
                ByVal _Coluna As Integer,
                ByVal _Valor As Object,
                ByVal _Fonte As Font,
                ByVal _Cor As Color,
                ByVal _Negrito As Boolean)

            Linha = _Linha
            Coluna = _Coluna
            Valor = _Valor
            'TamanhoCelula = _TamanhoCelula
            Fonte = _Fonte
            Cor = _Cor
            Negrito = _Negrito
            Alinhamento = eAlinhamento.General

        End Sub

        Sub New(ByVal _Linha As Integer,
                ByVal _Coluna As Integer,
                ByVal _Valor As Object,
                ByVal _Fonte As Font,
                ByVal _Cor As Color,
                ByVal _Negrito As Boolean,
                ByVal _Alinhamento As eAlinhamento)

            Linha = _Linha
            Coluna = _Coluna
            Valor = _Valor
            'TamanhoCelula = _TamanhoCelula
            Fonte = _Fonte
            Cor = _Cor
            Negrito = _Negrito
            Alinhamento = _Alinhamento

        End Sub

        Dim Linha As Integer
        Dim Coluna As Integer
        Dim Valor As Object
        Dim TamanhoCelula As Integer
        Dim Fonte As Font
        Dim Cor As Color
        Dim Negrito As Boolean
        Dim Alinhamento As eAlinhamento
    End Structure

    Private strArquivoXLS As String = ""
    Private strAba As String = "BRADESCO"
    Private strTitulo As String = "GeraDAL - Relatório"
    Private strAutor As String = "DCO"
    Private strComentario As String = "SGSS - Relatório de tempos - DCO - Bradesco S/A"
    Private strCompania As String = "Bradesco S/A"

    Const LINHA_CABECALHO_BRADESCO As Integer = 2
    Const LINHA_CABECALHO_RELATORIO As Integer = 4
    Const LINHA_CABECALHO_TABELA As Integer = 6
    Const COLUNA_INICIAL_DADOS As Integer = 2

    Private Pacote As ExcelPackage
    Private Planilha As ExcelWorksheet

    Sub New(ByVal _strArquivo As String)
        Me.Arquivo = _strArquivo
    End Sub

    Sub New()
    End Sub

#Region "Get_Set"
    Public Property Arquivo() As String
        Get
            Return strArquivoXLS
        End Get
        Set(ByVal Value As String)
            strArquivoXLS = Value
        End Set
    End Property

    Public Property Aba() As String
        Get
            Return strAba
        End Get
        Set(ByVal Value As String)
            strAba = Value
        End Set
    End Property

    Public Property Titulo() As String
        Get
            Return strTitulo
        End Get
        Set(ByVal Value As String)
            strTitulo = Value
        End Set
    End Property

    Public Property Autor() As String
        Get
            Return strAutor
        End Get
        Set(ByVal Value As String)
            strAutor = Value
        End Set
    End Property

    Public Property Comentario() As String
        Get
            Return strComentario
        End Get
        Set(ByVal Value As String)
            strComentario = Value
        End Set
    End Property

    Public Property Compania() As String
        Get
            Return strCompania
        End Get
        Set(ByVal Value As String)
            strCompania = Value
        End Set
    End Property
#End Region

    Public Function Imprimir(ByVal rs1 As SuperDataSet,
                             ByVal strNomeRelatorio As String,
                             Optional ByVal bAbre As Boolean = False) As Boolean

        Dim oArquivo As FileInfo
        Dim strCampo As String

        Dim iRow As Integer
        Dim iCol As Integer

        Dim ValorCampo As Object
        Dim sCabecalho As String

        Dim iTotalColunasDetalhe As Integer

        Try

            Me.Arquivo = Me.Arquivo & "." & Format(Now, "yyyy.MM.dd-hh.mm.ss") & ".xlsx"
            Me.Arquivo = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Me.Arquivo)


            If ExisteArquivo(Me.Arquivo) Then
                If S_MsgBox("Já existe o arquivo [" & Me.Arquivo & "]." & vbNewLine &
                         "Deseja sobrescrever?.", eBotoes.SimNao, , , eImagens.Interrogacao) = eRet.Nao Then
                    Return False
                Else
                    ApagaArquivo(Me.Arquivo)
                End If
            End If

            oArquivo = New FileInfo(Me.Arquivo)
            Pacote = New ExcelPackage(oArquivo)

            Dim iLinha As Integer = 1
            Dim iColuna As Integer = 1

            'Ajusta cabeçalho
            'sCabecalho = strTituloApp & " :: " & strTitulo & " : Detalhes"
            sCabecalho = strNomeRelatorio & " (" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ")"

            'ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");

           
                Planilha = Pacote.Workbook.Worksheets.Add(Me.Aba)

                'Planilha.Cells("A1:AB1").Style.Font.Bold = True
                iCol = COLUNA_INICIAL_DADOS

                'Guarda o total de colunas
                iTotalColunasDetalhe = 0

                ''#############################################
                ''#####     FOR pra montar as colunas.    #####
                ''#############################################
                For i = 0 To rs1.TotalColunas() - 1 'FieldCount() - 1
                    strCampo = rs1.NomeColuna(i)
                    If (strCampo.Substring(0, 3) = "as_") Then
                        strCampo = Formata_Coluna(strCampo)
                        Planilha.Cells(LINHA_CABECALHO_TABELA, iCol).Value = strCampo 'ajustarTituloColuna(oDataSet.NomeColuna(j))
                        Planilha.Cells(LINHA_CABECALHO_TABELA, iCol).Style.Font.Bold = True
                        Planilha.Cells(LINHA_CABECALHO_TABELA, iCol).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                        Planilha.Cells(LINHA_CABECALHO_TABELA, iCol).Style.Fill.BackgroundColor.SetColor(Color.LightGray)
                        Planilha.Cells(LINHA_CABECALHO_TABELA, iCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                        iCol = iCol + 1
                        iTotalColunasDetalhe += 1
                    End If
                Next i

                iRow = LINHA_CABECALHO_TABELA + 1

                '################################################
                '#####     FOR pra Preencher as colunas.    #####
                '################################################
                'Do While rs1.Read
                For posReg = 0 To (rs1.TotalRegistros() - 1) Step 1
                    iCol = 2
                    For i = 0 To (rs1.TotalColunas() - 1) 'i = indice da coluna (campo) do recordset
                        strCampo = rs1.NomeColuna(i)
                        If (strCampo.Substring(0, 3) = "as_") Then
                            ValorCampo = rs1.ValorCampo(i, posReg)

                            Planilha.Cells(iRow, iCol).Value = ValorCampo

                            If (rs1.TipoDadosColuna(i) Is GetType(Decimal)) Or
                                (rs1.TipoDadosColuna(i) Is GetType(Integer)) Then
                                Planilha.Cells(iRow, iCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right
                            ElseIf (rs1.TipoDadosColuna(i) Is GetType(DateTime)) Then
                                Planilha.Cells(iRow, iCol).Style.Numberformat.Format = "dd/MM/yyyy"
                                Planilha.Cells(iRow, iCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center
                            Else  'Se for alfanumerico = alinha a esquerda
                                Planilha.Cells(iRow, iCol).Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left
                            End If

                            iCol = iCol + 1
                        End If
                    Next i
                    iRow = iRow + 1
                Next posReg
                '################################################
                '#####  FIM FOR pra Preencher as colunas.   #####
                '################################################

                'Autoajuste das colunas
                For iCol = COLUNA_INICIAL_DADOS To COLUNA_INICIAL_DADOS + iTotalColunasDetalhe - 1
                    Planilha.Column(iCol).AutoFit()
                Next

                'Coloca o cabeçalho do Bradesco
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Value = "BRADESCO"
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Color.SetColor(Color.White)
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Bold = True
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Size = 22

                'Ajuste da Cor da linha de cabeçalho bradesco das colunas
                For iCol = COLUNA_INICIAL_DADOS To COLUNA_INICIAL_DADOS + iTotalColunasDetalhe - 1
                    Planilha.Cells(LINHA_CABECALHO_BRADESCO, iCol).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                    Planilha.Cells(LINHA_CABECALHO_BRADESCO, iCol).Style.Fill.BackgroundColor.SetColor(Color.DarkRed)
                Next iCol

                'Coloca o cabeçalho do relatório
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Value = sCabecalho
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Bold = True
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Color.SetColor(Color.DarkBlue)
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Size = 16

                Pacote.Workbook.Properties.Title = Me.Titulo ' "SGSS - Relatório de tempos"
                Pacote.Workbook.Properties.Author = Me.Autor '"DCO"
                Pacote.Workbook.Properties.Comments = Me.Comentario '"SGSS - Relatório de tempos - DCO - Bradesco S/A"
                Pacote.Workbook.Properties.Company = Me.Compania '"Bradesco S/A"

                Pacote.Workbook.Properties.SetCustomPropertyValue("Criado em", Now.ToString)

                Pacote.Save()



            If bAbre = True Then
                Me.Visualizar()
            End If



            Return True

        Catch ex As Exception
            LogaErro("Erro em SuperXLS::Imprimir: " & CStr(ex.Message))
            Return False
        Finally
            Pacote.Dispose()
            Planilha = Nothing
            Pacote = Nothing
        End Try

    End Function

    Function NumeroParaColuna(ByVal Numero As Integer) As String
        Try

            Numero = Numero - 1

            If Numero < 0 Or Numero >= 27 * 26 Then
                NumeroParaColuna = "-" 'Invalido, retorna nada
            Else
                If Numero < 26 Then 'uma letra, apenas retorna a letra corresp.
                    NumeroParaColuna = Chr(Numero + 65)
                Else 'duas letras, obtem letra baseado no modulo e divisao de inteiro
                    NumeroParaColuna = Chr(Numero \ 26 + 64) + Chr(Numero Mod 26 + 65)
                End If
            End If
        Catch ex As Exception
            LogaErro("Erro em SuperXLS::NumeroParaColuna: " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Sub Visualizar()
        Process.Start(Me.Arquivo)
    End Sub

    'se id <> 0, a coluna eh do tipo id_ (codigo)
    Private Function Formata_Coluna(ByVal sString As String,
                                    Optional ByVal id As Integer = 0) As String
        Try
            Dim nCerquilha As Integer
            Dim sstring2 As String

            If id = 0 Then
                sstring2 = Replace(xRight(sString, Len(sString) - 3), "_", " ")
            Else
                sstring2 = xRight(sString, Len(sString) - 3)
            End If

            nCerquilha = InStr(sstring2, "#")

            If nCerquilha > 0 Then
                sstring2 = xLeft(sstring2, nCerquilha - 1)
            End If

            Return sstring2
        Catch ex As Exception
            LogaErro("Erro em Util::Formata_Coluna: " & ex.ToString())
            Return ""
        End Try

    End Function

    Public Function xRight(ByVal s As String, ByVal n As Integer) As String
        If n > s.Length Then
            Return s
        ElseIf n < 1 Then
            Return ""
        Else
            Return s.Substring(s.Length - n, n)
        End If
    End Function

    Public Function xLeft(ByVal s As String, ByVal n As Integer) As String
        If n > s.Length Then
            Return s
        ElseIf n < 1 Then
            Return ""
        Else
            Return s.Substring(0, n)
        End If
    End Function



    Public Function ImprimirCol(ByVal colDados As Collection,
                                ByVal strNomeRelatorio As String,
                                Optional ByVal colTam As Collection = Nothing,
                                Optional ByVal bAbre As Boolean = False,
                                Optional ByVal registroCorreio As String = "",
                                Optional ByVal qtdeAbas As Decimal = 1) As Boolean

        Dim oArquivo As FileInfo
        Dim iCol As Integer
        Dim sCabecalho As String
        Dim iTotalColunasDetalhe As Integer

        'Dim img As System.Drawing.Image
        'Dim pic As ExcelPicture

        Dim xTemp As sPropriedadeCelula

        Try

            'img = My.Resources.btnQuestion

            Me.Arquivo = Me.Arquivo & "." & Format(Now, "yyyy.MM.dd-hh.mm.ss") & ".xlsx"
            Me.Arquivo = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Me.Arquivo)


            If ExisteArquivo(Me.Arquivo) Then
                If S_MsgBox("Já existe o arquivo [" & Me.Arquivo & "]." & vbNewLine &
                         "Deseja sobrescrever?.", eBotoes.SimNao, , , eImagens.Interrogacao) = eRet.Nao Then
                    Return False
                Else
                    ApagaArquivo(Me.Arquivo)
                End If
            End If

            oArquivo = New FileInfo(Me.Arquivo)
            Pacote = New ExcelPackage(oArquivo)

            'Dim iLinha As Integer = 1
            'Dim iColuna As Integer = 1

            'Ajusta cabeçalho
            sCabecalho = strNomeRelatorio & " (" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ")"

            'ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");


            Planilha = Pacote.Workbook.Worksheets.Add("Bradesco")

            'registroCorreio = registroCorreio.Substring(registroCorreio.IndexOf(",") + 1, 3)

            'Planilha = Pacote.Workbook.Worksheets.Add(Me.Aba)


            'pic = Planilha.Drawings.AddPicture("PictureUniqueName", img)

            'pic.From.Column = 1
            'pic.From.Row = 1
            'pic.From.ColumnOff = 1
            'pic.From.RowOff = 1
            '//set picture size to fit inside the cell
            'pic.SetSize(200, 200)


            'Planilha.Cells("A1:AB1").Style.Font.Bold = True
            iCol = COLUNA_INICIAL_DADOS

            'Guarda o total de colunas
            iTotalColunasDetalhe = 0


            '####INICIO LOOP PERCORRE COLLECTION#########################################
            If Not colTam Is Nothing Then
                For i = 1 To colTam.Count Step 1
                    Planilha.Column(i).Width = CDbl(colTam.Item(i))
                Next i
            End If
            '####FIM    LOOP PERCORRE COLLECTION#########################################


            '####INICIO LOOP PERCORRE COLLECTION#########################################
            For i = 1 To colDados.Count Step 1

                xTemp = CType(colDados.Item(i), sPropriedadeCelula)

                'Pega a coluna maxima com dados
                If iTotalColunasDetalhe < xTemp.Coluna Then
                    iTotalColunasDetalhe = xTemp.Coluna
                End If

                Planilha.Cells(xTemp.Linha, xTemp.Coluna).Value = xTemp.Valor
                Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Color.SetColor(xTemp.Cor)

                Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Size = xTemp.Fonte.Size
                Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Name = xTemp.Fonte.Name
                Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Bold = xTemp.Negrito

                If xTemp.Alinhamento <> sPropriedadeCelula.eAlinhamento.General Then
                    Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.HorizontalAlignment = CType(xTemp.Alinhamento, ExcelHorizontalAlignment)
                End If

                'Planilha.Cells(xTemp.Linha, xTemp.Coluna).????? = xTemp.TamanhoCelula

            Next i
            '####FIM LOOP PERCORRE COLLECTION#########################################


            'Coloca o cabeçalho do Bradesco
            Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Value = "BRADESCO"
            Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Color.SetColor(Color.White)
            Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Bold = True
            Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Size = 22

            'Ajuste da Cor da linha de cabeçalho bradesco das colunas
            For iCol = COLUNA_INICIAL_DADOS To COLUNA_INICIAL_DADOS + iTotalColunasDetalhe - 2
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, iCol).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, iCol).Style.Fill.BackgroundColor.SetColor(Color.DarkRed)
            Next iCol

            'Coloca o cabeçalho do relatório
            Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Value = sCabecalho
            Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Bold = True
            Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Color.SetColor(Color.DarkBlue)
            Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Size = 16

            Pacote.Workbook.Properties.Title = Me.Titulo ' "SGSS - Relatório de tempos"
            Pacote.Workbook.Properties.Author = Me.Autor '"DCO"
            Pacote.Workbook.Properties.Comments = Me.Comentario '"SGSS - Relatório de tempos - DCO - Bradesco S/A"
            Pacote.Workbook.Properties.Company = Me.Compania '"Bradesco S/A"

            Pacote.Workbook.Properties.SetCustomPropertyValue("Criado em", Now.ToString)

            Pacote.Save()

            If bAbre = True Then
                Me.Visualizar()
            End If

            Return True

        Catch ex As Exception
            LogaErro("Erro em SuperXLS::Imprimir: " & CStr(ex.Message))
            Return False
        Finally
            Pacote.Dispose()
            Planilha = Nothing
            Pacote = Nothing
        End Try

    End Function



    Public Function ImprimirDataSet(ByVal dataSet As SuperDataSet,
                                    ByVal strNomeRelatorio As String,
                                    Optional ByVal colTam As Collection = Nothing,
                                    Optional ByVal bAbre As Boolean = False) As Boolean

        Dim oArquivo As FileInfo
        Dim iCol As Integer
        Dim sCabecalho As String
        Dim iTotalColunasDetalhe As Integer

        'Dim img As System.Drawing.Image
        'Dim pic As ExcelPicture

        Dim xTemp As sPropriedadeCelula

        Dim fntConsole10 As Font = New Font("Lucida Console", 10)
        Dim fntConsole14 As Font = New Font("Lucida Console", 14)

        Dim CorpoCorreio As String
        Dim strLinhaCorpoCorreio() As String



        Try

            'img = Nothing


            Me.Arquivo = Me.Arquivo & "." & Format(Now, "yyyy.MM.dd-hh.mm.ss") & ".xlsx"
            Me.Arquivo = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Me.Arquivo)


            If ExisteArquivo(Me.Arquivo) Then
                If S_MsgBox("Já existe o arquivo [" & Me.Arquivo & "]." & vbNewLine &
                         "Deseja sobrescrever?.", eBotoes.SimNao, , , eImagens.Interrogacao) = eRet.Nao Then
                    Return False
                Else
                    ApagaArquivo(Me.Arquivo)
                End If
            End If

            oArquivo = New FileInfo(Me.Arquivo)
            Pacote = New ExcelPackage(oArquivo)

            'Dim iLinha As Integer = 1
            'Dim iColuna As Integer = 1

            'Ajusta cabeçalho
            sCabecalho = strNomeRelatorio & " (" & Format(Now, "dd/MM/yyyy hh:mm:ss") & ")"

            'ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");

            For iContador = 0 To dataSet.TotalRegistros - 1 Step 1

                Dim colDados As New Collection

                Dim dtEnvio As Date = Nothing
                Dim dtRecep As Date = Nothing

                If (IsDBNull(dataSet.ObterDataRow(iContador).Item(3)) = False) Then
                    dtEnvio = CDate(dataSet.ObterDataRow(iContador).Item(3).ToString)
                End If

                If (IsDBNull(dataSet.ObterDataRow(iContador).Item(4)) = False) Then

                    dtRecep = CDate(dataSet.ObterDataRow(iContador).Item(4).ToString)
                End If

                colDados.Add(New sPropriedadeCelula(7, 2, "Referência", fntConsole10, Color.Red, True))
                colDados.Add(New sPropriedadeCelula(7, 3, dataSet.ObterDataRow(iContador).Item(2), fntConsole10, Color.Black, False))

                colDados.Add(New sPropriedadeCelula(8, 2, "Tit.Correio", fntConsole10, Color.Black, True))
                colDados.Add(New sPropriedadeCelula(8, 3, dataSet.ObterDataRow(iContador).Item(6), fntConsole10, Color.Black, False))

                colDados.Add(New sPropriedadeCelula(9, 2, "Origem", fntConsole10, Color.Black, True))
                colDados.Add(New sPropriedadeCelula(9, 3, dataSet.ObterDataRow(iContador).Item(8), fntConsole10, Color.Black, False))


                colDados.Add(New sPropriedadeCelula(7, 4, "Data Envio", fntConsole10, Color.Black, True))
                If (dtEnvio <> Nothing) Then
                    colDados.Add(New sPropriedadeCelula(7, 5, dtEnvio.Day.ToString("D2") + "/" + dtEnvio.Month.ToString("D2") & "/" & dtEnvio.Year.ToString("D4"), fntConsole10, Color.Black, False, sPropriedadeCelula.eAlinhamento.Right))
                End If
                colDados.Add(New sPropriedadeCelula(8, 4, "Data Recep", fntConsole10, Color.Black, True))
                If (dtRecep <> Nothing) Then
                    colDados.Add(New sPropriedadeCelula(8, 5, dtRecep.Day.ToString("D2") + "/" + dtRecep.Month.ToString("D2") & "/" & dtRecep.Year.ToString("D4"), fntConsole10, Color.Black, False, sPropriedadeCelula.eAlinhamento.Right))
                End If

                colDados.Add(New sPropriedadeCelula(7, 6, "Hora Envio", fntConsole10, Color.Black, True))
                If (dtEnvio <> Nothing) Then
                    colDados.Add(New sPropriedadeCelula(7, 7, dtEnvio.Hour.ToString("D2") & ":" & dtEnvio.Minute.ToString("D2") & ":" & dtEnvio.Second.ToString("D2"), fntConsole10, Color.Black, False, sPropriedadeCelula.eAlinhamento.Right))
                End If
                colDados.Add(New sPropriedadeCelula(8, 6, "Hora Recep", fntConsole10, Color.Black, True))
                If (dtRecep <> Nothing) Then
                    colDados.Add(New sPropriedadeCelula(8, 7, dtRecep.Hour.ToString("D2") & ":" & dtRecep.Minute.ToString("D2") & ":" & dtRecep.Second.ToString("D2"), fntConsole10, Color.Black, False, sPropriedadeCelula.eAlinhamento.Right))
                End If
                colDados.Add(New sPropriedadeCelula(9, 6, "N. Corresp", fntConsole10, Color.Black, True))
                colDados.Add(New sPropriedadeCelula(9, 7, dataSet.ObterDataRow(iContador).Item(2), fntConsole10, Color.Black, False, sPropriedadeCelula.eAlinhamento.Right))

                colDados.Add(New sPropriedadeCelula(10, 2, "", fntConsole10, Color.Black, True))
                colDados.Add(New sPropriedadeCelula(10, 3, dataSet.ObterDataRow(iContador).Item(12), fntConsole10, Color.Black, False))

                CorpoCorreio = dataSet.ObterDataRow(iContador).Item(9).ToString


                strLinhaCorpoCorreio = CorpoCorreio.Split(CChar(vbNewLine))

                Dim iLinha As Integer
                iLinha = 12
                For Each strLinha As String In strLinhaCorpoCorreio
                    colDados.Add(New sPropriedadeCelula(iLinha, 2, strLinha, fntConsole14, Color.Black, False))
                    iLinha += 1
                Next strLinha



                Planilha = Pacote.Workbook.Worksheets.Add(dataSet.ObterDataRow(iContador).Item(2).ToString + iContador.ToString)


                'Planilha = Pacote.Workbook.Worksheets.Add(Me.Aba)


                ' pic = Planilha.Drawings.AddPicture("PictureUniqueName", img)

                ' pic.From.Column = 1
                ' pic.From.Row = 1
                ' pic.From.ColumnOff = 1
                ' pic.From.RowOff = 1
                '//set picture size to fit inside the cell
                'pic.SetSize(50, 50)


                'Planilha.Cells("A1:AB1").Style.Font.Bold = True
                iCol = COLUNA_INICIAL_DADOS

                'Guarda o total de colunas
                iTotalColunasDetalhe = 0


                '####INICIO LOOP PERCORRE COLLECTION#########################################
                If Not colTam Is Nothing Then
                    For i = 1 To colTam.Count Step 1
                        Planilha.Column(i).Width = CDbl(colTam.Item(i))
                    Next i
                End If
                '####FIM    LOOP PERCORRE COLLECTION#########################################


                '####INICIO LOOP PERCORRE COLLECTION#########################################
                For i = 1 To colDados.Count Step 1

                    xTemp = CType(colDados.Item(i), sPropriedadeCelula)

                    'Pega a coluna maxima com dados
                    If iTotalColunasDetalhe < xTemp.Coluna Then
                        iTotalColunasDetalhe = xTemp.Coluna
                    End If

                    Planilha.Cells(xTemp.Linha, xTemp.Coluna).Value = xTemp.Valor
                    Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Color.SetColor(xTemp.Cor)

                    Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Size = xTemp.Fonte.Size
                    Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Name = xTemp.Fonte.Name
                    Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.Font.Bold = xTemp.Negrito

                    If xTemp.Alinhamento <> sPropriedadeCelula.eAlinhamento.General Then
                        Planilha.Cells(xTemp.Linha, xTemp.Coluna).Style.HorizontalAlignment = CType(xTemp.Alinhamento, ExcelHorizontalAlignment)
                    End If

                    'Planilha.Cells(xTemp.Linha, xTemp.Coluna).????? = xTemp.TamanhoCelula

                Next i
                '####FIM LOOP PERCORRE COLLECTION#########################################


                'Coloca o cabeçalho do Bradesco
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Value = "BRADESCO"
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Color.SetColor(Color.White)
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Bold = True
                Planilha.Cells(LINHA_CABECALHO_BRADESCO, COLUNA_INICIAL_DADOS).Style.Font.Size = 22

                'Ajuste da Cor da linha de cabeçalho bradesco das colunas
                For iCol = COLUNA_INICIAL_DADOS To COLUNA_INICIAL_DADOS + iTotalColunasDetalhe - 2
                    Planilha.Cells(LINHA_CABECALHO_BRADESCO, iCol).Style.Fill.PatternType = Style.ExcelFillStyle.Solid
                    Planilha.Cells(LINHA_CABECALHO_BRADESCO, iCol).Style.Fill.BackgroundColor.SetColor(Color.DarkRed)
                Next iCol

                'Coloca o cabeçalho do relatório
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Value = sCabecalho
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Bold = True
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Color.SetColor(Color.DarkBlue)
                Planilha.Cells(LINHA_CABECALHO_RELATORIO, COLUNA_INICIAL_DADOS).Style.Font.Size = 16

            Next
            Pacote.Workbook.Properties.Title = Me.Titulo ' "SGSS - Relatório de tempos"
            Pacote.Workbook.Properties.Author = Me.Autor '"DCO"
            Pacote.Workbook.Properties.Comments = Me.Comentario '"SGSS - Relatório de tempos - DCO - Bradesco S/A"
            Pacote.Workbook.Properties.Company = Me.Compania '"Bradesco S/A"

            Pacote.Workbook.Properties.SetCustomPropertyValue("Criado em", Now.ToString)



            Pacote.Save()




            If bAbre = True Then
                Me.Visualizar()
            End If

            Return True

        Catch ex As Exception
            LogaErro("Erro em SuperXLS::Imprimir: " & CStr(ex.Message))
            Return False
        Finally
            Pacote.Dispose()
            Planilha = Nothing
            Pacote = Nothing
        End Try

    End Function



End Class
