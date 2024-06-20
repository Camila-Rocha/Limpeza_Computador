namespace ProjetoLimpezaDePCRefatoracao
{
    partial class JanelaLimpezaPC
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JanelaLimpezaPC));
            tituloHeader = new Label();
            linkExcluirChavesCriadas = new LinkLabel();
            panelFooter = new Panel();
            btnVoltar = new Button();
            btnContinuar = new Button();
            panelHeader = new Panel();
            panelFooter.SuspendLayout();
            panelHeader.SuspendLayout();
            SuspendLayout();
            // 
            // tituloHeader
            // 
            tituloHeader.AutoSize = true;
            tituloHeader.Font = new Font("Arial Narrow", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tituloHeader.Location = new Point(41, 23);
            tituloHeader.Name = "tituloHeader";
            tituloHeader.Size = new Size(366, 26);
            tituloHeader.TabIndex = 1;
            tituloHeader.Text = "Selecione as opções que deseja executar";
            // 
            // linkExcluirChavesCriadas
            // 
            linkExcluirChavesCriadas.AutoSize = true;
            linkExcluirChavesCriadas.LinkColor = Color.FromArgb(192, 0, 0);
            linkExcluirChavesCriadas.Location = new Point(500, 28);
            linkExcluirChavesCriadas.Name = "linkExcluirChavesCriadas";
            linkExcluirChavesCriadas.Size = new Size(137, 20);
            linkExcluirChavesCriadas.TabIndex = 0;
            linkExcluirChavesCriadas.TabStop = true;
            linkExcluirChavesCriadas.Text = "Excluir Chaves Criadas";
            linkExcluirChavesCriadas.LinkClicked += LinkExcluirChavesCriadas_LinkClicked;
            // 
            // panelFooter
            // 
            panelFooter.BackColor = SystemColors.Menu;
            panelFooter.Controls.Add(btnVoltar);
            panelFooter.Controls.Add(btnContinuar);
            panelFooter.Location = new Point(1, 622);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(680, 79);
            panelFooter.TabIndex = 4;
            // 
            // btnVoltar
            // 
            btnVoltar.Location = new Point(41, 18);
            btnVoltar.Name = "btnVoltar";
            btnVoltar.Size = new Size(115, 40);
            btnVoltar.TabIndex = 1;
            btnVoltar.Text = "Voltar";
            btnVoltar.UseVisualStyleBackColor = true;
            btnVoltar.Click += btnVoltar_Click;
            // 
            // btnContinuar
            // 
            btnContinuar.Enabled = false;
            btnContinuar.Location = new Point(522, 18);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new Size(115, 40);
            btnContinuar.TabIndex = 0;
            btnContinuar.Text = "Continuar";
            btnContinuar.UseVisualStyleBackColor = true;
            btnContinuar.Click += BtnContinuar_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = SystemColors.Menu;
            panelHeader.Controls.Add(tituloHeader);
            panelHeader.Controls.Add(linkExcluirChavesCriadas);
            panelHeader.Location = new Point(1, 2);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(680, 73);
            panelHeader.TabIndex = 3;
            // 
            // JanelaLimpezaPC
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(682, 703);
            Controls.Add(panelFooter);
            Controls.Add(panelHeader);
            Font = new Font("Arial Narrow", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "JanelaLimpezaPC";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Limpeza de Computador";
            panelFooter.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label tituloHeader;
        private LinkLabel linkExcluirChavesCriadas;
        private Panel panelFooter;
        private Button btnVoltar;
        private Button btnContinuar;
        private Panel panelHeader;
    }
}
