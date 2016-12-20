<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Principal
    Inherits System.Windows.Forms.Form

    'Descartar substituições de formulário para limpar a lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Exigido pelo Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'OBSERVAÇÃO: O procedimento a seguir é exigido pelo Windows Form Designer
    'Ele pode ser modificado usando o Windows Form Designer.  
    'Não o modifique usando o editor de códigos.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Principal))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.lblStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.lvTabelas = New Sonda.Util.SuperLV()
        Me.cmbBanco = New Sonda.Util.SuperComboBox()
        Me.txtSenha = New Sonda.Util.SuperTextBox()
        Me.txtUsuario = New Sonda.Util.SuperTextBox()
        Me.txtServidor = New Sonda.Util.SuperTextBox()
        Me.cmdGeraVB = New Sonda.Util.SuperButton()
        Me.cmdGeraProc = New Sonda.Util.SuperButton()
        Me.cmdSair = New Sonda.Util.SuperButton()
        Me.cmdListaTabelas = New Sonda.Util.SuperButton()
        Me.cmdSobre = New Sonda.Util.SuperButton()
        Me.cmdDesconecta = New Sonda.Util.SuperButton()
        Me.cmdConectar = New Sonda.Util.SuperButton()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 45)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Usuário"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(25, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Senha"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(17, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Servidor"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(22, 97)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Banco"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblStatus})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 470)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(526, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 6
        Me.StatusStrip1.Text = "SStrip"
        '
        'lblStatus
        '
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(0, 17)
        '
        'lvTabelas
        '
        Me.lvTabelas.Location = New System.Drawing.Point(12, 126)
        Me.lvTabelas.Name = "lvTabelas"
        Me.lvTabelas.SelecionaVarios = False
        Me.lvTabelas.Size = New System.Drawing.Size(502, 257)
        Me.lvTabelas.TabIndex = 7
        Me.lvTabelas.UseCompatibleStateImageBehavior = False
        '
        'cmbBanco
        '
        Me.cmbBanco.Alterado = False
        Me.cmbBanco.FormattingEnabled = True
        Me.cmbBanco.Location = New System.Drawing.Point(66, 94)
        Me.cmbBanco.Name = "cmbBanco"
        Me.cmbBanco.Size = New System.Drawing.Size(253, 21)
        Me.cmbBanco.SuperObrigatorio = True
        Me.cmbBanco.SuperTxtObrigatorio = "Banco"
        Me.cmbBanco.TabIndex = 3
        '
        'txtSenha
        '
        Me.txtSenha.Alterado = False
        Me.txtSenha.BackColor = System.Drawing.Color.White
        Me.txtSenha.Location = New System.Drawing.Point(66, 68)
        Me.txtSenha.Name = "txtSenha"
        Me.txtSenha.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtSenha.Size = New System.Drawing.Size(253, 20)
        Me.txtSenha.SuperMascara = ""
        Me.txtSenha.SuperObrigatorio = True
        Me.txtSenha.SuperTravaErrors = False
        Me.txtSenha.SuperTxtCorDesabilitado = System.Drawing.Color.Empty
        Me.txtSenha.SuperTxtObrigatorio = "Senha"
        Me.txtSenha.SuperUsaMascara = Sonda.Util.SuperTextBox.TipoMascara_.NENHUMA
        Me.txtSenha.TabIndex = 2
        '
        'txtUsuario
        '
        Me.txtUsuario.Alterado = False
        Me.txtUsuario.BackColor = System.Drawing.Color.White
        Me.txtUsuario.Location = New System.Drawing.Point(66, 42)
        Me.txtUsuario.Name = "txtUsuario"
        Me.txtUsuario.Size = New System.Drawing.Size(253, 20)
        Me.txtUsuario.SuperMascara = ""
        Me.txtUsuario.SuperObrigatorio = True
        Me.txtUsuario.SuperTravaErrors = False
        Me.txtUsuario.SuperTxtCorDesabilitado = System.Drawing.Color.Empty
        Me.txtUsuario.SuperTxtObrigatorio = "Usuario"
        Me.txtUsuario.SuperUsaMascara = Sonda.Util.SuperTextBox.TipoMascara_.NENHUMA
        Me.txtUsuario.TabIndex = 1
        '
        'txtServidor
        '
        Me.txtServidor.Alterado = False
        Me.txtServidor.BackColor = System.Drawing.Color.White
        Me.txtServidor.Location = New System.Drawing.Point(66, 16)
        Me.txtServidor.Name = "txtServidor"
        Me.txtServidor.Size = New System.Drawing.Size(253, 20)
        Me.txtServidor.SuperMascara = ""
        Me.txtServidor.SuperObrigatorio = True
        Me.txtServidor.SuperTravaErrors = False
        Me.txtServidor.SuperTxtCorDesabilitado = System.Drawing.Color.Empty
        Me.txtServidor.SuperTxtObrigatorio = "Servidor"
        Me.txtServidor.SuperUsaMascara = Sonda.Util.SuperTextBox.TipoMascara_.NENHUMA
        Me.txtServidor.TabIndex = 0
        '
        'cmdGeraVB
        '
        Me.cmdGeraVB.BackColor = System.Drawing.Color.Transparent
        Me.cmdGeraVB.BackgroundImage = CType(resources.GetObject("cmdGeraVB.BackgroundImage"), System.Drawing.Image)
        Me.cmdGeraVB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdGeraVB.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdGeraVB.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdGeraVB.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdGeraVB.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdGeraVB.ForeColor = System.Drawing.Color.Black
        Me.cmdGeraVB.Image = CType(resources.GetObject("cmdGeraVB.Image"), System.Drawing.Image)
        Me.cmdGeraVB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdGeraVB.Location = New System.Drawing.Point(12, 389)
        Me.cmdGeraVB.Name = "cmdGeraVB"
        Me.cmdGeraVB.Size = New System.Drawing.Size(118, 60)
        Me.cmdGeraVB.TabIndex = 5
        Me.cmdGeraVB.Text = "Gerar" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Módulo VB"
        Me.cmdGeraVB.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdGeraVB.UseVisualStyleBackColor = False
        '
        'cmdGeraProc
        '
        Me.cmdGeraProc.BackColor = System.Drawing.Color.Transparent
        Me.cmdGeraProc.BackgroundImage = CType(resources.GetObject("cmdGeraProc.BackgroundImage"), System.Drawing.Image)
        Me.cmdGeraProc.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdGeraProc.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdGeraProc.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdGeraProc.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdGeraProc.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdGeraProc.ForeColor = System.Drawing.Color.Black
        Me.cmdGeraProc.Image = CType(resources.GetObject("cmdGeraProc.Image"), System.Drawing.Image)
        Me.cmdGeraProc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdGeraProc.Location = New System.Drawing.Point(136, 389)
        Me.cmdGeraProc.Name = "cmdGeraProc"
        Me.cmdGeraProc.Size = New System.Drawing.Size(118, 60)
        Me.cmdGeraProc.TabIndex = 5
        Me.cmdGeraProc.Text = "Gerar" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Procedure"
        Me.cmdGeraProc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdGeraProc.UseVisualStyleBackColor = False
        '
        'cmdSair
        '
        Me.cmdSair.BackColor = System.Drawing.Color.Transparent
        Me.cmdSair.BackgroundImage = CType(resources.GetObject("cmdSair.BackgroundImage"), System.Drawing.Image)
        Me.cmdSair.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdSair.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdSair.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdSair.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdSair.ForeColor = System.Drawing.Color.Black
        Me.cmdSair.Image = Global.GeraDal.My.Resources.Resources.Sair
        Me.cmdSair.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSair.Location = New System.Drawing.Point(396, 389)
        Me.cmdSair.Name = "cmdSair"
        Me.cmdSair.Size = New System.Drawing.Size(118, 60)
        Me.cmdSair.TabIndex = 5
        Me.cmdSair.Text = "&Sair"
        Me.cmdSair.UseVisualStyleBackColor = False
        '
        'cmdListaTabelas
        '
        Me.cmdListaTabelas.BackColor = System.Drawing.Color.Transparent
        Me.cmdListaTabelas.BackgroundImage = CType(resources.GetObject("cmdListaTabelas.BackgroundImage"), System.Drawing.Image)
        Me.cmdListaTabelas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdListaTabelas.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdListaTabelas.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdListaTabelas.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdListaTabelas.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdListaTabelas.ForeColor = System.Drawing.Color.Black
        Me.cmdListaTabelas.Image = Global.GeraDal.My.Resources.Resources.Listar
        Me.cmdListaTabelas.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdListaTabelas.Location = New System.Drawing.Point(345, 86)
        Me.cmdListaTabelas.Name = "cmdListaTabelas"
        Me.cmdListaTabelas.Size = New System.Drawing.Size(169, 31)
        Me.cmdListaTabelas.TabIndex = 5
        Me.cmdListaTabelas.Text = "Listar Tabelas"
        Me.cmdListaTabelas.UseVisualStyleBackColor = False
        '
        'cmdSobre
        '
        Me.cmdSobre.BackColor = System.Drawing.Color.Transparent
        Me.cmdSobre.BackgroundImage = CType(resources.GetObject("cmdSobre.BackgroundImage"), System.Drawing.Image)
        Me.cmdSobre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdSobre.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdSobre.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdSobre.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdSobre.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdSobre.ForeColor = System.Drawing.Color.Black
        Me.cmdSobre.Image = Global.GeraDal.My.Resources.Resources.Sobre
        Me.cmdSobre.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdSobre.Location = New System.Drawing.Point(260, 389)
        Me.cmdSobre.Name = "cmdSobre"
        Me.cmdSobre.Size = New System.Drawing.Size(130, 60)
        Me.cmdSobre.TabIndex = 5
        Me.cmdSobre.Text = "S&obre"
        Me.cmdSobre.UseVisualStyleBackColor = False
        '
        'cmdDesconecta
        '
        Me.cmdDesconecta.BackColor = System.Drawing.Color.Transparent
        Me.cmdDesconecta.BackgroundImage = CType(resources.GetObject("cmdDesconecta.BackgroundImage"), System.Drawing.Image)
        Me.cmdDesconecta.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdDesconecta.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdDesconecta.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdDesconecta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdDesconecta.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdDesconecta.ForeColor = System.Drawing.Color.Black
        Me.cmdDesconecta.Image = Global.GeraDal.My.Resources.Resources.Desconectar
        Me.cmdDesconecta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDesconecta.Location = New System.Drawing.Point(345, 49)
        Me.cmdDesconecta.Name = "cmdDesconecta"
        Me.cmdDesconecta.Size = New System.Drawing.Size(169, 31)
        Me.cmdDesconecta.TabIndex = 5
        Me.cmdDesconecta.Text = "Desconectar"
        Me.cmdDesconecta.UseVisualStyleBackColor = False
        '
        'cmdConectar
        '
        Me.cmdConectar.BackColor = System.Drawing.Color.Transparent
        Me.cmdConectar.BackgroundImage = CType(resources.GetObject("cmdConectar.BackgroundImage"), System.Drawing.Image)
        Me.cmdConectar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdConectar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.cmdConectar.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.cmdConectar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmdConectar.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdConectar.ForeColor = System.Drawing.Color.Black
        Me.cmdConectar.Image = Global.GeraDal.My.Resources.Resources.Conectar
        Me.cmdConectar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdConectar.Location = New System.Drawing.Point(345, 13)
        Me.cmdConectar.Name = "cmdConectar"
        Me.cmdConectar.Size = New System.Drawing.Size(169, 31)
        Me.cmdConectar.TabIndex = 5
        Me.cmdConectar.Text = "Conectar"
        Me.cmdConectar.UseVisualStyleBackColor = False
        '
        'frm_Principal
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(526, 492)
        Me.ControlBox = False
        Me.Controls.Add(Me.lvTabelas)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.cmdGeraVB)
        Me.Controls.Add(Me.cmdGeraProc)
        Me.Controls.Add(Me.cmdSair)
        Me.Controls.Add(Me.cmdListaTabelas)
        Me.Controls.Add(Me.cmdSobre)
        Me.Controls.Add(Me.cmdDesconecta)
        Me.Controls.Add(Me.cmdConectar)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbBanco)
        Me.Controls.Add(Me.txtSenha)
        Me.Controls.Add(Me.txtUsuario)
        Me.Controls.Add(Me.txtServidor)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frm_Principal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "GeraDal"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtServidor As Sonda.Util.SuperTextBox
    Friend WithEvents txtUsuario As Sonda.Util.SuperTextBox
    Friend WithEvents txtSenha As Sonda.Util.SuperTextBox
    Friend WithEvents cmbBanco As Sonda.Util.SuperComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents cmdConectar As Sonda.Util.SuperButton
    Friend WithEvents cmdSair As Sonda.Util.SuperButton
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents lblStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents cmdListaTabelas As Sonda.Util.SuperButton
    Friend WithEvents lvTabelas As Sonda.Util.SuperLV
    Friend WithEvents cmdSobre As Sonda.Util.SuperButton
    Friend WithEvents cmdGeraProc As Sonda.Util.SuperButton
    Friend WithEvents cmdGeraVB As Sonda.Util.SuperButton
    Friend WithEvents cmdDesconecta As Sonda.Util.SuperButton

End Class
