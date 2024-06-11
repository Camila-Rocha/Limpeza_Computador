using System;
using System.Runtime.CompilerServices;

namespace ProjetoLimpezaDePCRefatoracao
{
    public delegate void meuDelegate(object sender, PaintEventArgs e);

    public partial class JanelaLimpezaPC : Form
    {
        Panel panel_1;
        Panel panel_2;
        Panel panel_3;

        public JanelaLimpezaPC()
        {
            InitializeComponent();

            btnVoltar.Enabled = false;

            panel_1 = new Panel()
            {
                Size = new Size(658, 535),
                Location = new Point(12, 81),
                BackColor = Color.Red,
                Enabled = true,
                Visible = true
            };
            this.Controls.Add(panel_1);

            panel_2 = new Panel()
            {
                Size = new Size(658, 535),
                Location = new Point(12, 81),
                BackColor = Color.Blue,
                Enabled = false,
                Visible = false
            };

            panel_3 = new Panel()
            {
                Size = new Size(658, 535),
                Location = new Point(12, 81),
                BackColor = Color.Black,
                Enabled = false,
                Visible = false
            };
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            IdentificaComponenteAtivo(true);
        }

        public void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void IdentificaComponenteAtivo(bool isContinuar)
        {
            if (panel_1.Enabled)
            {
                var nome = nameof(panel_1).Split('_').Last() == "1";

                if (isContinuar)
                {
                    panel_1.Visible = false;
                    panel_1.Enabled = false;
                    panel_2.Visible = true;
                    panel_2.Enabled = true;
                    btnVoltar.Enabled = true;
                    this.Controls.Add(panel_2);
                    btnContinuar.Text = "Executar";
                }
            }

            else if (panel_2.Enabled)
            {
                if (isContinuar)
                {
                    panel_2.Visible = false;
                    panel_2.Enabled = false;
                    panel_3.Visible = true;
                    panel_3.Enabled = true;
                    this.Controls.Add(panel_3);
                }
                else
                {
                    panel_2.Visible = false;
                    panel_2.Enabled = false;
                    panel_1.Visible = true;
                    panel_1.Enabled = true;
                    btnVoltar.Enabled = false;
                    this.Controls.Add(panel_1);
                }
            }
            else if (panel_3.Enabled)
            {
                btnContinuar.Text = "Finalizar";
                btnVoltar.Enabled = false;
                btnVoltar.Visible = false;
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            IdentificaComponenteAtivo(false);
        }
    }
}
