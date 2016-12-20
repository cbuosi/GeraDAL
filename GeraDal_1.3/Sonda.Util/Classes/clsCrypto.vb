#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2013, Todos direitos reservados, Sonda-IT - Serviços de TI - http://www.sondait.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: clsCrypto
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Modulo de manipulacao de criptografia.
' Uso..........: Não se aplica
' Produto......: GeraDAL
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

Imports System.Security.Cryptography
Imports System.Text

Public Class clsCrypto
    Implements IDisposable

    Dim myKey As String
    Dim des As TripleDESCryptoServiceProvider
    Dim hashmd5 As MD5CryptoServiceProvider

    Public Sub New()
        Try
            myKey = "cbuosi@gmail.comDÄD%A%DAD$%&ÄD" 'Chave de configuracao da classe
            des = New TripleDESCryptoServiceProvider()
            hashmd5 = New MD5CryptoServiceProvider()
        Catch ex As Exception
            LogaErro("Erro em clsCrypto::New(1): " & ex.Message)
        End Try
    End Sub

#If False Then
    Public Function clsCrypto(ByVal texto As String, ByVal operacao As Boolean) As String
        Try
            If operacao = True Then
                clsCrypto = Encripta(texto)
            Else
                clsCrypto = DeCripta(texto)
            End If
        Catch ex As Exception
            LogaErro("Erro em clsCrypto::clsCrypto: " & CStr(ex.Message))
            Return ""
        End Try
    End Function
#End If

    Public Function DeCripta(ByVal testo As String) As String
        Try
            des.Key = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(myKey))
            des.Mode = CipherMode.ECB
            Dim desdencrypt As ICryptoTransform = des.CreateDecryptor()
            Dim buff() As Byte = Convert.FromBase64String(testo)
            DeCripta = ASCIIEncoding.ASCII.GetString(desdencrypt.TransformFinalBlock(buff, 0, buff.Length))
        Catch ex As Exception
            LogaErro("Erro em clsCrypto::DeCripta: " & CStr(ex.Message))
            Return ""
        End Try
    End Function

    Public Function Encripta(ByVal testo As String) As String
        Try

            des.Key = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(myKey))
            des.Mode = CipherMode.ECB
            Dim desdencrypt As ICryptoTransform = des.CreateEncryptor()
            Dim MyASCIIEncoding = New ASCIIEncoding()
            Dim buff() As Byte = ASCIIEncoding.ASCII.GetBytes(testo)
            Encripta = Convert.ToBase64String(desdencrypt.TransformFinalBlock(buff, 0, buff.Length))
        Catch ex As Exception
            LogaErro("Erro em clsCrypto::Encripta: " & CStr(ex.Message))
            Return ""

        End Try
    End Function

    Public Function MD5_Hash(ByVal SourceText As String) As String
        Try

            'Create an encoding object to ensure the encoding standard for the source text
            Dim Ue As New UnicodeEncoding()
            'Retrieve a byte array based on the source text
            Dim ByteSourceText() As Byte = Ue.GetBytes(SourceText)
            'Instantiate an MD5 Provider object
            Dim Md5 As New MD5CryptoServiceProvider()
            'Compute the hash value from the source
            Dim ByteHash() As Byte = Md5.ComputeHash(ByteSourceText)
            'And convert it to String format for return
            Return Convert.ToBase64String(ByteHash)
        Catch ex As Exception
            LogaErro("Erro em clsCrypto::MD5_Hash: " & CStr(ex.Message))
            Return ""
        End Try

    End Function

    'Suporte para a interface IDisposable
#Region "IDisposable Support"
    Private disposedValue As Boolean

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                myKey = Nothing
                des = Nothing
                hashmd5 = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class