Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Windows.Forms

Public Class SuperLV
    Inherits System.Windows.Forms.ListView


    Private _mSelect As Boolean = False
    Private _iChave As Integer = 0
    Private _Chave As Object = ""
    Private Checado As String = "ü"
    Private Deschecado As String = "û"
    Public Atualizando As Boolean = False


    Protected Overrides Sub OnGotFocus(ByVal e As System.EventArgs)
        Try
            Me.BackColor = corObjSelecionado
            MyBase.OnGotFocus(e)
        Catch ex As Exception
            LogaErro("Erro em SuperLV::OnGotFocus: " & CStr(ex.ToString()))
        End Try
    End Sub

    Protected Overrides Sub OnLostFocus(ByVal e As System.EventArgs)
        Try
            Me.BackColor = corObjNaoSelecionado
            MyBase.OnLostFocus(e)
        Catch ex As Exception
            LogaErro("Erro em SuperLV::OnLostFocus: " & CStr(ex.ToString()))
        End Try
    End Sub

    Public Sub New()
        Try
            Me.DoubleBuffered = True
            InitializeComponent()
        Catch ex As Exception
            LogaErro("Erro em SuperLV::New: " & CStr(ex.ToString()))
        End Try
    End Sub

    <Category("SuperLV"), Description("Deixa selecionar varias linhas na grid")> _
    Public Property SelecionaVarios As Boolean
        Get
            Return _mSelect
        End Get
        Set(ByVal value As Boolean)
            _mSelect = value
            Me.Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnItemCheck(ByVal ice As System.Windows.Forms.ItemCheckEventArgs)
        Try

            If Me.Atualizando = True Then
                Exit Sub
            End If

            If _mSelect = False Then
                Dim i As Integer
                If ice.NewValue = CheckState.Checked Then
                    For i = 0 To Me.Items.Count - 1
                        If Me.Items(i).Checked = True Then
                            Me.Items(i).Checked = False
                        End If
                    Next i
                End If
            End If

            If ice.NewValue = CheckState.Checked Then
                If _iChave = 0 Then 'se for da primeira coluna....
                    _Chave = Me.Items(ice.Index).Text
                Else 'se for da segunda em diante...
                    _Chave = Me.Items(ice.Index).SubItems(_iChave).Text
                End If
            Else
                _Chave = Nothing
            End If

            MyBase.OnItemCheck(ice)

        Catch ex As Exception
            LogaErro("Erro em SuperLV::OnItemCheck: " & ex.ToString())
        End Try
    End Sub

    Public Function ObterCodigoLinha(ByVal nLinha As Integer) As String
        Try

            If Me.Items.Count < nLinha Then
                Return ""
            End If

            If _iChave = 0 Then 'se for da primeira coluna....
                Return Me.Items(nLinha).Text
            Else 'se for da segunda em diante...
                Return Me.Items(nLinha).SubItems(_iChave).Text
            End If

        Catch ex As Exception
            LogaErro("Erro em SuperLV::ObterCodigoLinha: " & ex.ToString())
            Return ""
        End Try
    End Function

    Public Function ObterChaveS() As String
        Try
            Return _Chave.ToString
        Catch ex As Exception
            LogaErro("Erro em SuperLV::ObterChave: " & ex.ToString())
            Return ""
        End Try
    End Function

    Public Function ObterChave() As Integer
        Try
            If IsNumeric(_Chave) = True Then
                Return CInt(_Chave)
            Else
                Return 0
            End If
        Catch ex As Exception
            LogaErro("Erro em SuperLV::ObterChave: " & ex.ToString())
            Return 0
        End Try
    End Function

    Public Function ObterChaveComposta() As String
        Try
            Return _Chave.ToString
        Catch ex As Exception
            LogaErro("Erro em SuperLV::ObterChaveComposta: " & ex.ToString())
            Return ""
        End Try
    End Function


    Public Function ObterTotalChecados() As Integer
        Try

            Dim i As Integer = 0
            Dim totChecked As Integer = 0

            For i = 0 To Me.ObterTotalLinhas() - 1 Step 1
                If Me.Items(i).Checked = True Then
                    totChecked += 1
                End If
            Next i

            Return totChecked

        Catch ex As Exception
            LogaErro("Erro em SuperLV::ObterTotalChecados")
            Return -1
        End Try

    End Function

    Public Function ObterTotalLinhas() As Integer
        Try
            Return Me.Items.Count
        Catch ex As Exception
            LogaErro("Erro em SuperLV::ObterTotalLinhas")
            Return -1
        End Try
    End Function


    Public Sub PreencheGridDS(ByVal rs1 As SuperDataSet,
                              ByVal chk_box As Boolean,
                              Optional ByVal BarraTitulo As Boolean = True,
                              Optional ByVal Contador As Boolean = False,
                              Optional ByVal Zebrado As Boolean = True,
                              Optional ByVal MultiSelect As Boolean = False)
        Try
            Dim idxColuna As Integer
            Dim clmX As ColumnHeader
            Dim itmX As ListViewItem
            Dim strTamanho As String
            Dim intTamanho As Integer
            Dim nContador As Long
            Dim strCampo As String
            Dim posReg As Integer
            Dim ValorCampo As Object

            Me.Atualizando = True
            Me.BeginUpdate()

            If rs1 Is Nothing Then
                LogaErro("SuperLV::PreencheGridDS [" & Me.Name & "] - ATENCAO: RecordSet=Nothing, favor verificar...")
                Exit Sub
            End If

            LogaErro("SuperLV::PreencheGridDS [" & Me.Name & "] - N.Registros [" & rs1.TotalRegistros.ToString & "]")
            _iChave = 0

            Me.View = View.Details                              ' define o modo de exibição do listview
            Me.LabelEdit = False                                ' permite o usuario editar o item
            Me.AllowColumnReorder = False                       ' permite o usuario rearranjar as colunas
            Me.CheckBoxes = chk_box                             ' exibe as caixas de marcacao (check boxes.)
            Me.FullRowSelect = True                             ' seleciona um item e subitem quando a seleção é feita
            Me.GridLines = True                                 ' exibe as linhas
            Me.Sorting = SortOrder.None                         ' ordena os itens na list na ordem ascendente
            Me.MultiSelect = MultiSelect                        ' Deixa selecionar celula individual
            Me.BackgroundImageTiled = True                      ' Liga fundo com imagem
            Me.BackgroundImage = My.Resources.SuperLV           ' Seleciona a imagem
            Me.DoubleBuffered = True                            ' Duplo Manteigado

            Dim lvChecado As New ListViewItem.ListViewSubItem

            If BarraTitulo = True Then
                Me.HeaderStyle = ColumnHeaderStyle.Clickable
            Else
                Me.HeaderStyle = ColumnHeaderStyle.None
            End If

            'PreencheGridDS = 0 'tirar depois
            _iChave = 0

            nContador = 0

            If rs1 Is Nothing Then
                Exit Sub
            End If

            'Limpa a bagaça...
            Me.Clear()

            'Se tiver Coluna de contador, inclui...
            idxColuna = 0

            If Contador = True Then
                clmX = New ColumnHeader()
                clmX.Text = "Nº"
                clmX.Width = 45
                Me.Columns.Add(clmX)
                idxColuna = idxColuna + 1
            End If

            ''#############################################
            ''#####     FOR pra montar as colunas.    #####
            ''#############################################
            For i = 0 To rs1.TotalColunas() - 1 'FieldCount() - 1

                strCampo = rs1.NomeColuna(i)

                'procura por # e tenta pegar um numerico depois dele. Ex: as_coluna#123 <- pega 123
                'strTamanho = Right(strCampo, Len(strCampo) - InStr(strCampo, "#"))
                strTamanho = strCampo.Substring(strCampo.IndexOf("#") + 1, strCampo.Length - strCampo.IndexOf("#") - 1)

                'se deu certo o comando anterior OK, senao faz calculo (tam * 10)
                If IsNumeric(strTamanho) = True Then
                    intTamanho = CInt(Val(strTamanho))
                Else
                    intTamanho = Len(strCampo) * 10
                End If

                'So entra no grid se for as_ ou id_
                If (strCampo.Substring(0, 3) = "as_") Then
                    clmX = New ColumnHeader()
                    clmX.Text = Formata_Coluna(strCampo)
                    clmX.Width = intTamanho
                    'clmX.AutoResize = True
                    'Se for numerico = alinha a direita
                    If (rs1.TipoDadosColuna(i) Is GetType(Decimal)) Then
                        clmX.TextAlign = HorizontalAlignment.Right
                    ElseIf (rs1.TipoDadosColuna(i) Is GetType(DateTime)) Then
                        clmX.TextAlign = HorizontalAlignment.Center
                    Else  'Se for alfanumerico = alinha a esquerda
                        clmX.TextAlign = HorizontalAlignment.Left
                    End If
                    Me.Columns.Add(clmX)
                ElseIf (strCampo.Substring(0, 3) = "id_") Then
                    clmX = New ColumnHeader()
                    clmX.Text = Formata_Coluna(strCampo, 1)
                    clmX.Width = 0
                    Me.Columns.Add(clmX)
                ElseIf (strCampo.Substring(0, 3) = "ck_") Then
                    clmX = New ColumnHeader()
                    clmX.Text = Formata_Coluna(strCampo)
                    clmX.Width = intTamanho
                    clmX.TextAlign = HorizontalAlignment.Center
                    Me.Columns.Add(clmX)
                End If

                'retorna em qual coluna esta o id_ (chave)
                If (strCampo.Substring(0, 3) = "as_") Or _
                   (strCampo.Substring(0, 3) = "id_") Or
                   (strCampo.Substring(0, 3) = "ck_") Then
                    If (strCampo.Substring(0, 3) = "id_") Then
                        'PreencheGridDS = idxColuna 'GUARDA A COLUNA QUE ESTA O ID DO REGISTRO 'tirar depois
                        _iChave = idxColuna
                    End If
                    idxColuna = idxColuna + 1 'aumenta o indice se entrar na grid ("id_" ou "as_")
                End If
            Next i
            '#############################################
            '#####  FIM FOR pra montar as colunas.   #####
            '#############################################

            '################################################
            '#####     FOR pra Preencher as colunas.    #####
            '################################################
            'Do While rs1.Read
            For posReg = 0 To (rs1.TotalRegistros() - 1) Step 1

                idxColuna = 0
                nContador = nContador + 1

                itmX = New ListViewItem()

                itmX.UseItemStyleForSubItems = False

                If Contador = True Then
                    itmX.Text = CStr(posReg + 1)
                    idxColuna = idxColuna + 1
                End If

                'i = indice da coluna (campo) do recordset
                For i = 0 To (rs1.TotalColunas() - 1)

                    strCampo = rs1.NomeColuna(i)
                    'tipoCampo = rs1.TipoDadosColuna(i)
                    ValorCampo = rs1.ValorCampo(i, posReg)

                    ' XD
                    If chk_box = True And idxColuna = 0 Then 'se campo eh do tipo checkbox
                        'itmX.Font = New Drawing.Font("Wingdings", 10)
                        If IsDBNull(rs1.ValorCampo("chk", posReg)) = True Then
                            If itmX.Checked = True Then
                                itmX.Checked = False
                            End If
                        Else
                            If CStr(rs1.ValorCampo("chk", posReg)) = "" Then
                                If itmX.Checked = True Then
                                    itmX.Checked = False
                                End If
                            Else
                                If itmX.Checked = False Then
                                    itmX.Checked = True
                                End If
                            End If
                            'itmX.Checked = CBool(IIf(CStr(rs1.ValorCampo("chk", posReg)) = "", False, True))
                        End If
                    End If

                    If (strCampo.Substring(0, 3) = "as_") Then
                        If idxColuna = 0 Then
                            itmX.Text = ValorCampo.ToString
                        Else
                            itmX.SubItems.Add(ValorCampo.ToString)
                        End If
                        idxColuna = idxColuna + 1
                    ElseIf (strCampo.Substring(0, 3) = "id_") Then
                        If idxColuna = 0 Then
                            itmX.Text = ValorCampo.ToString
                        Else
                            itmX.SubItems.Add(ValorCampo.ToString)
                        End If
                        idxColuna = idxColuna + 1
                    ElseIf (strCampo.Substring(0, 3) = "ck_") And idxColuna > 0 Then

                        lvChecado = New ListViewItem.ListViewSubItem

                        lvChecado.Font = New Drawing.Font("Wingdings", 14)

                        If IsDBNull(rs1.ValorCampo(i, posReg)) = True Then
                            lvChecado.Text = Deschecado
                            lvChecado.ForeColor = Drawing.Color.Red
                        ElseIf CDec(rs1.ValorCampo(i, posReg)) = 1 Then
                            lvChecado.Text = Checado
                            lvChecado.ForeColor = Drawing.Color.Green
                        Else
                            lvChecado.Text = Deschecado
                            lvChecado.ForeColor = Drawing.Color.Red
                        End If
                        idxColuna = idxColuna + 1
                        itmX.SubItems.Add(lvChecado)
                    End If

                Next i

                If Zebrado = True Then

                    Dim cor As Drawing.Color

                    If posReg Mod 2 = 0 Then
                        cor = corGrid1
                    Else
                        cor = corGrid2
                    End If

                    itmX.BackColor = cor
                    For k = 0 To itmX.SubItems.Count - 1
                        itmX.SubItems(k).BackColor = cor
                    Next k
                End If

                Me.Items.Add(itmX)

            Next posReg
            '################################################
            '#####  FIM FOR pra Preencher as colunas.   #####
            '################################################



        Catch ex As Exception
            LogaErro("Erro em Util::PreencheGridDS: " & ex.ToString())
        Finally
            Me.EndUpdate()
            Me.Atualizando = False
        End Try


    End Sub

    Public Function ObterCSVChaves(Optional ByVal separador As String = ";") As String
        Try
            Dim i As Integer
            Dim ret As String = ""

            For i = 0 To Me.Items.Count - 1
                If Me.Items(i).Checked = True Then
                    If _iChave = 0 Then 'se for da primeira coluna....
                        ret = ret & CStr(IIf(Len(ret) = 0, "", separador)) & Me.Items(i).Text
                    Else 'se for da segunda em diante...
                        ret = ret & CStr(IIf(Len(ret) = 0, "", separador)) & Me.Items(i).SubItems(_iChave).Text
                    End If
                End If
            Next i
            Return ret
        Catch ex As Exception
            LogaErro("Erro em " & Me.Name & "::obtemCSVChaves: " & CStr(ex.ToString()))
            Return ""
        End Try
    End Function

    Public Sub SelecionarTodos(ByVal bSelected As Boolean)
        Dim i As Integer
        Try
            Me.BeginUpdate()
            For i = 0 To Me.Items.Count - 1
                If Me.Items(i).Checked <> bSelected Then
                    Me.Items(i).Checked = bSelected
                End If
            Next
            Me.EndUpdate()
        Catch ex As Exception
            LogaErro("Erro em Util::SelecionarTodos: " & ex.ToString())
        End Try
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

    Public Function LimpaGrid() As Boolean
        Me.Items.Clear()
        Return True
    End Function

End Class
