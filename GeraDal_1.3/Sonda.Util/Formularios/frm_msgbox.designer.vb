<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_msgbox
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_msgbox))
        Me.lblTexto = New System.Windows.Forms.Label()
        Me.imgMsgBox = New System.Windows.Forms.PictureBox()
        Me.lblInfoFechar = New System.Windows.Forms.Label()
        Me.btn3 = New Sonda.Util.SuperButton()
        Me.btn2 = New Sonda.Util.SuperButton()
        Me.btn1 = New Sonda.Util.SuperButton()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.imgMsgBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTexto
        '
        Me.lblTexto.BackColor = System.Drawing.Color.Transparent
        Me.lblTexto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTexto.Location = New System.Drawing.Point(9, 51)
        Me.lblTexto.Name = "lblTexto"
        Me.lblTexto.Size = New System.Drawing.Size(488, 109)
        Me.lblTexto.TabIndex = 1
        Me.lblTexto.Text = "Texto label messagebox"
        Me.lblTexto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'imgMsgBox
        '
        Me.imgMsgBox.BackColor = System.Drawing.Color.Transparent
        Me.imgMsgBox.Image = Global.Sonda.Util.My.Resources.Resources.btnOk
        Me.imgMsgBox.Location = New System.Drawing.Point(7, 77)
        Me.imgMsgBox.Name = "imgMsgBox"
        Me.imgMsgBox.Size = New System.Drawing.Size(53, 53)
        Me.imgMsgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.imgMsgBox.TabIndex = 6
        Me.imgMsgBox.TabStop = False
        '
        'lblInfoFechar
        '
        Me.lblInfoFechar.AutoSize = True
        Me.lblInfoFechar.BackColor = System.Drawing.Color.Transparent
        Me.lblInfoFechar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfoFechar.Location = New System.Drawing.Point(4, 164)
        Me.lblInfoFechar.Name = "lblInfoFechar"
        Me.lblInfoFechar.Size = New System.Drawing.Size(121, 30)
        Me.lblInfoFechar.TabIndex = 7
        Me.lblInfoFechar.Text = "Mensagem Fechar" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Mensagem Fechar 2"
        '
        'btn3
        '
        Me.btn3.BackColor = System.Drawing.Color.Transparent
        Me.btn3.BackgroundImage = CType(resources.GetObject("btn3.BackgroundImage"), System.Drawing.Image)
        Me.btn3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn3.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.btn3.ForeColor = System.Drawing.Color.Black
        Me.btn3.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn3.Location = New System.Drawing.Point(305, 166)
        Me.btn3.Name = "btn3"
        Me.btn3.Size = New System.Drawing.Size(78, 26)
        Me.btn3.TabIndex = 4
        Me.btn3.Text = "Botão 3"
        Me.btn3.UseCompatibleTextRendering = True
        Me.btn3.UseVisualStyleBackColor = False
        '
        'btn2
        '
        Me.btn2.BackColor = System.Drawing.Color.Transparent
        Me.btn2.BackgroundImage = CType(resources.GetObject("btn2.BackgroundImage"), System.Drawing.Image)
        Me.btn2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn2.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.btn2.ForeColor = System.Drawing.Color.Black
        Me.btn2.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn2.Location = New System.Drawing.Point(221, 166)
        Me.btn2.Name = "btn2"
        Me.btn2.Size = New System.Drawing.Size(78, 26)
        Me.btn2.TabIndex = 3
        Me.btn2.Text = "Botão 2"
        Me.btn2.UseCompatibleTextRendering = True
        Me.btn2.UseVisualStyleBackColor = False
        '
        'btn1
        '
        Me.btn1.BackColor = System.Drawing.Color.Transparent
        Me.btn1.BackgroundImage = CType(resources.GetObject("btn1.BackgroundImage"), System.Drawing.Image)
        Me.btn1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.btn1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btn1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btn1.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.btn1.ForeColor = System.Drawing.Color.Black
        Me.btn1.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.btn1.Location = New System.Drawing.Point(137, 166)
        Me.btn1.Name = "btn1"
        Me.btn1.Size = New System.Drawing.Size(78, 26)
        Me.btn1.TabIndex = 2
        Me.btn1.Text = "Botão 1"
        Me.btn1.UseCompatibleTextRendering = True
        Me.btn1.UseVisualStyleBackColor = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = Global.Sonda.Util.My.Resources.Resources.botton1a
        Me.PictureBox2.Location = New System.Drawing.Point(-31, 197)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(600, 21)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox2.TabIndex = 5
        Me.PictureBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Sonda.Util.My.Resources.Resources.top1
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(1100, 42)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'frm_msgbox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(507, 208)
        Me.Controls.Add(Me.imgMsgBox)
        Me.Controls.Add(Me.lblTexto)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btn3)
        Me.Controls.Add(Me.btn2)
        Me.Controls.Add(Me.btn1)
        Me.Controls.Add(Me.lblInfoFechar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frm_msgbox"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Texto messagebox!"
        CType(Me.imgMsgBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents lblTexto As System.Windows.Forms.Label
    Friend WithEvents imgMsgBox As System.Windows.Forms.PictureBox
    Friend WithEvents btn3 As Sonda.Util.SuperButton
    Friend WithEvents btn2 As Sonda.Util.SuperButton
    Friend WithEvents btn1 As Sonda.Util.SuperButton
    Friend WithEvents lblInfoFechar As System.Windows.Forms.Label
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
End Class
