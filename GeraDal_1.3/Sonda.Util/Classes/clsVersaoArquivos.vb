#Region "Legal"
'************************************************************************************************************************
' Copyright (c) 2010, Todos direitos reservados, DEDIC GPTI - Tecnologia da Informação - http://www.gpti.com.br/
'
' Autor........: Carlos Buosi (cbuosi@gmail.com)
' Arquivo......: VersaoArquivos.vb
' Tipo.........: Modulo VB.
' Versao.......: 2.02+
' Propósito....: Garantir que a versao dos arquivos batam no Bradesco <-> Cliente
' Uso..........: Não se aplica
' Produto......: SAUC - Bradesco.
'
' Legal........: Este código é de propriedade do Banco Bradesco S/A e/ou DEDIC GPTI - Tecnologia da Informação, sua cópia
'                e/ou distribuição é proibida.
'
' GUID.........: {52313876-0D4A-4DA9-81BF-7F3D985F4927}
' Observações..: nenhuma.
'
'************************************************************************************************************************
#End Region
Option Explicit On
Option Strict On

Public Class clsVersaoArquivos

    Public Shared Function ObterVersaoBancoDados() As String
        Return AppVersion()
    End Function

    Public Function ObterVersaoArquivos() As String
        Return AppVersion()
    End Function

    Public Function ObterSenhaZip() As String
        Return "hunter2"
    End Function

    Public Function ObterSenhaSQLite() As String
        Return "" 'TODO: mudar depois
    End Function

    Public Shared Function AppData() As String
        Return "Janeiro 2013"
    End Function

End Class
