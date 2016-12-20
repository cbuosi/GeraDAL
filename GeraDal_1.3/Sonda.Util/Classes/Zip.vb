#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: Zip.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Modulo de manipulacao de arquivos ZIP (Compactados) Zipa / Deszipa.
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

Imports Ionic.Zip
Imports System.IO
Imports System.IO.Compression

Public Class Zip
    ''' <summary>
    ''' '''''''''''''''''''''''''''''''''''''
    ''' </summary>
    ''' <param name="ArquivoEntrada"></param>
    ''' <param name="ArquivoZip">'</param>
    ''' <param name="Senha"></param>
    ''' <returns>''''''''''''''''''''''''''''''''1</returns>
    ''' <remarks></remarks>
    Public Shared Function ZiparArq(ByVal ArquivoEntrada As String,
                                    ByVal ArquivoZip As String,
                                    Optional ByVal Senha As String = "") As Boolean

        Dim zip1 As ZipFile = Nothing

        Try

            zip1 = New ZipFile

            If ExisteArquivo(ArquivoZip) = True Then
                zip1 = ZipFile.Read(ArquivoZip)
            End If

            Using zip1
                If Senha <> "" Then
                    zip1.Password = Senha
                End If
                zip1.AddFile(ArquivoEntrada, "")
                zip1.Comment = strTituloApp & " V." & AppVersion() & vbNewLine & Now().ToString()
                zip1.CompressionLevel = Ionic.Zlib.CompressionLevel.BEST_COMPRESSION
                'Se ja existir o arquivo, apenas atualiza
                zip1.Save(ArquivoZip)
            End Using

            zip1.Dispose()

            Return True

        Catch ex As Exception
            LogaErro("Erro em Zip::ZiparArq: " & CStr(ex.Message))
            Return False
        Finally
            zip1 = Nothing
        End Try

    End Function

    Public Shared Function ZiparDir(ByVal DiretorioEntrada As String,
                             ByVal ArquivoZip As String,
                             Optional ByVal Senha As String = "") As Boolean

        Dim zip1 As ZipFile = Nothing

        Try

            zip1 = New ZipFile

            Using zip1
                If Senha <> "" Then
                    zip1.Password = Senha
                End If
                zip1.AddDirectory(DiretorioEntrada)
                zip1.Save(ArquivoZip)
            End Using

            zip1.Dispose()
            Return True

        Catch ex As Exception
            LogaErro("Erro em Zip::ZiparDir: " & CStr(ex.Message))
            Return False
        Finally
            zip1 = Nothing
        End Try

    End Function

    Public Shared Function Deszipar(ByVal ArquivoZip As String,
                             ByVal ArquivoSaida As String,
                             Optional ByVal Senha As String = "") As Integer

        Dim ret As Integer = 0
        Dim zip1 As ZipFile = Nothing

        Try

            zip1 = New ZipFile(ArquivoZip)

            Using zip1

                If Senha <> "" Then
                    zip1.Password = Senha
                End If
                zip1.ExtractAll(ArquivoSaida, True)
                ret = zip1.Count
            End Using

            zip1.Dispose()

            Return ret

        Catch ex As Exception
            LogaErro("Erro em Zip::Deszipar: " & CStr(ex.Message))
            Return -1
        Finally
            zip1 = Nothing
        End Try

    End Function

    Public Shared Function ParaBase64(ByVal data() As Byte) As String
        Try
            If data Is Nothing Then
                Return Nothing
            End If
            Return Convert.ToBase64String(data)
        Catch ex As Exception
            LogaErro("Erro em Zip::ParaBase64: " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Shared Function DeBase64(ByVal base64 As String) As Byte()
        Try
            If base64 Is Nothing Then
                Return Nothing
            End If
            Return Convert.FromBase64String(base64)
        Catch ex As Exception
            LogaErro("Erro em Zip::DeBase64: " & CStr(ex.Message))
            Return Nothing
        End Try
    End Function


    Public Shared Function ZipaByte(ByVal input As Byte()) As Byte()
        Try
            Dim output() As Byte
            Dim ms As MemoryStream
            Dim gs As GZipStream

            ms = New MemoryStream()
            gs = New GZipStream(ms, CompressionMode.Compress)

            gs.Write(input, 0, input.Length)
            gs.Close()
            output = ms.ToArray()
            ms.Close()

            Return output
        Catch ex As Exception
            LogaErro("Erro em Zip::ZipaByte: " & CStr(ex.Message))
            Return Nothing
        End Try
    End Function

    Public Shared Function DeszipaByte(input As Byte()) As Byte()
        Try
            Dim output As List(Of Byte)
            Dim ms As MemoryStream
            Dim gs As GZipStream
            Dim readByte As Integer


            output = New List(Of Byte)()

            ms = New MemoryStream(input)
            gs = New GZipStream(ms, CompressionMode.Decompress)

            readByte = gs.ReadByte()

            While readByte <> -1
                output.Add(CByte(readByte))
                readByte = gs.ReadByte()
            End While

            gs.Close()
            ms.Close()

            Return output.ToArray()
        Catch ex As Exception
            LogaErro("Erro em Zip::DeszipaByte: " & CStr(ex.Message))
            Return Nothing
        End Try
    End Function

End Class
