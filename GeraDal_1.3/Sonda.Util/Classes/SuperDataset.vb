#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: SuperDataset.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Manipulacao de dataset
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

Public Class SuperDataSet
    Inherits DataSet

    Public NomeClasse As String = "SuperDataset"
    Private _InfoPesq As String = ""

    Public Sub New(ByVal novoNome As String)
        NomeClasse = novoNome
    End Sub

    Public Sub New()
        NomeClasse = "SuperDataset"
    End Sub

    Public Function TotalRegistros(Optional ByVal nTabela As Integer = 0) As Integer
        Try
            Return Me.Tables(nTabela).Rows.Count
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::TotalRegistros: " & CStr(ex.Message))
            Return -1
        End Try
    End Function

    Public Property InfoPesquisa As String
        Get
            Return _InfoPesq
        End Get
        Set(ByVal value As String)
            _InfoPesq = value
        End Set
    End Property

    'Como DEFAULT, chama o vlrCampo....
    Default Property ValorCampoDefault(ByVal nomeCol As String,
                                       Optional ByVal pos As Integer = 0,
                                       Optional ByVal nTabela As Integer = 0) As Object
        Get
            Return Me.ValorCampo(nomeCol, pos, nTabela)
        End Get
        Set(value As Object)
            'Nao faz bulhufas....
        End Set
    End Property


    Public Function ValorCampo(ByVal posCol As Integer,
                               Optional ByVal pos As Integer = 0,
                               Optional ByVal nTabela As Integer = 0) As Object
        Try

            If IsDBNull(Me.Tables(nTabela).Rows(pos).Item(posCol)) = True Then
                Return DBNull.Value
            Else
                Return Me.Tables(nTabela).Rows(pos).Item(posCol)
            End If

        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::ValorCampo(1): " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Function ObterDataRow(ByVal nLinha As Integer, Optional ByVal nTabela As Integer = 0) As DataRow
        Try
            Return Me.Tables(nTabela).Rows(nLinha)
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::ObterDataRow: " & CStr(ex.Message))
            Return Nothing
        End Try
    End Function

    Public Function ObterDataRowColl(Optional ByVal nTabela As Integer = 0) As DataRowCollection
        Try
            Return Me.Tables(nTabela).Rows()
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::ObterDataRow: " & CStr(ex.Message))
            Return Nothing
        End Try
    End Function


    Public Function ValorCampo(ByVal nomeCol As String,
                             Optional ByVal pos As Integer = 0,
                             Optional ByVal nTabela As Integer = 0) As Object
        Try
            Dim ncol = Me.IndiceColuna(nomeCol, nTabela)
            If ncol <> -1 Then
                If IsDBNull(Me.Tables(nTabela).Rows(pos).Item(ncol)) = True Then
                    Return DBNull.Value
                Else
                    Return Me.Tables(nTabela).Rows(pos).Item(ncol)
                End If
            Else
                Return DBNull.Value
            End If
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::ValorCampo(2): " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Function TotalColunas(Optional ByVal nTabela As Integer = 0) As Integer
        Try
            Return Me.Tables(nTabela).Columns.Count
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::TotalColunas: " & CStr(ex.Message))
            Return -1
        End Try
    End Function

    Public Function TipoDadosColuna(ByVal nColuna As String, Optional ByVal nTabela As Integer = 0) As Type
        Try
            Return Me.Tables(nTabela).Columns(IndiceColuna(nColuna)).DataType
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::TipoDadosColuna: " & CStr(ex.Message))
            Return GetType(String)
        End Try
    End Function

    Public Function TipoDadosColuna(ByVal nColuna As Integer, Optional ByVal nTabela As Integer = 0) As Type
        Try
            Return Me.Tables(nTabela).Columns(nColuna).DataType
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::TipoDadosColuna: " & CStr(ex.Message))
            Return GetType(String)
        End Try
    End Function


    Public Function NomeColuna(ByVal nColuna As Integer, Optional ByVal nTabela As Integer = 0) As String
        Try
            Return Me.Tables(nTabela).Columns(nColuna).ColumnName
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::NomeColuna: " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Function IndiceColuna(ByVal nomeColuna As String, Optional ByVal nTabela As Integer = 0) As Integer
        Try
            Return Me.Tables(nTabela).Columns.IndexOf(nomeColuna)
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::IndiceColuna: " & CStr(ex.Message))
            Return -1
        End Try
    End Function


    Public Function Filtra(ByVal strFiltro As String,
                           Optional ByVal orderByColumn As String = "",
                           Optional ByVal nTabela As Integer = 0) As SuperDataSet

        Dim tmpSuperDataSet As SuperDataSet
        Dim tmpDataTable As DataTable
        Dim tmpDataRow As DataRow()
        Dim i As Integer = 0

        Try

            tmpSuperDataSet = New SuperDataSet
            tmpDataTable = New DataTable

            Dim idx As Integer = 0
            For Each x As DataColumn In Me.Tables(nTabela).Columns
                tmpDataTable.Columns.Add(x.ColumnName)
            Next

            tmpDataRow = Me.Tables(0).Select(strFiltro, orderByColumn)

            For i = 0 To tmpDataRow.Length - 1 Step 1
                tmpDataTable.ImportRow(tmpDataRow(i))
            Next i

            tmpSuperDataSet.Tables.Add(tmpDataTable)

            Return tmpSuperDataSet

        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::Filtra(): " & CStr(ex.Message))
            Return Nothing
        End Try

    End Function

    Public Function Maximo(ByVal nomeCol As String,
                           Optional ByVal nTabela As Integer = 0) As Object
        Try
            Dim strOperacao As String = "Max(" & nomeCol & ")"
            Maximo = Me.Tables(nTabela).Compute(strOperacao, "")
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::Maximo(" & nomeCol & "): " & CStr(ex.Message))
            Maximo = ""
        End Try
    End Function

    Public Function Minimo(ByVal nomeCol As String,
                           Optional ByVal nTabela As Integer = 0) As Object
        Try
            Dim strOperacao As String = "Min(" & nomeCol & ")"
            Minimo = Me.Tables(nTabela).Compute(strOperacao, "")
        Catch ex As Exception
            LogaErro("Erro em " & NomeClasse & "::Maximo(" & nomeCol & "): " & CStr(ex.Message))
            Minimo = ""
        End Try
    End Function

End Class
