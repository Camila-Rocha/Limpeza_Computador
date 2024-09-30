namespace Limpeza_Computador
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
            panelFooter = new Panel();
            btnVoltar = new Button();
            btnContinuar = new Button();
            panelHeader = new Panel();
            panelBase = new Panel();
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
            // panelFooter
            // 
            panelFooter.BackColor = Color.AliceBlue;
            panelFooter.Controls.Add(btnVoltar);
            panelFooter.Controls.Add(btnContinuar);
            panelFooter.Location = new Point(15, 612);
            panelFooter.Name = "panelFooter";
            panelFooter.Size = new Size(652, 79);
            panelFooter.TabIndex = 4;
            // 
            // btnVoltar
            // 
            btnVoltar.BackColor = Color.White;
            btnVoltar.Location = new Point(41, 18);
            btnVoltar.Name = "btnVoltar";
            btnVoltar.Size = new Size(115, 40);
            btnVoltar.TabIndex = 1;
            btnVoltar.Text = "Voltar";
            btnVoltar.UseVisualStyleBackColor = false;
            btnVoltar.Click += BtnVoltar_Click;
            // 
            // btnContinuar
            // 
            btnContinuar.BackColor = Color.White;
            btnContinuar.Enabled = false;
            btnContinuar.Location = new Point(522, 18);
            btnContinuar.Name = "btnContinuar";
            btnContinuar.Size = new Size(115, 40);
            btnContinuar.TabIndex = 0;
            btnContinuar.Text = "Continuar";
            btnContinuar.UseVisualStyleBackColor = false;
            btnContinuar.Click += BtnContinuar_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.AliceBlue;
            panelHeader.Controls.Add(tituloHeader);
            panelHeader.Location = new Point(15, 14);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(655, 73);
            panelHeader.TabIndex = 3;
            panelHeader.Paint += panelHeader_Paint;
            // 
            // panelBase
            // 
            panelBase.BackColor = Color.Lavender;
            panelBase.BorderStyle = BorderStyle.FixedSingle;
            panelBase.Location = new Point(12, 12);
            panelBase.Name = "panelBase";
            panelBase.Size = new Size(659, 680);
            panelBase.TabIndex = 5;
            // 
            // JanelaLimpezaPC
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.Lavender;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(682, 703);
            Controls.Add(panelFooter);
            Controls.Add(panelHeader);
            Controls.Add(panelBase);
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
        private Panel panelFooter;
        private Button btnVoltar;
        private Button btnContinuar;
        private Panel panelHeader;
        private Panel panelBase;
    }
}
