using ProjetoLimpezaDePCRefatoracao.Domain;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ProjetoLimpezaDePCRefatoracao
{
    public delegate void meuDelegate(object sender, PaintEventArgs e);

    public partial class JanelaLimpezaPC : Form
    {
        Panel panel_1;
        Panel panel_2;
        Panel panel_3;
        List<Panel> ListaPanelCheckbox;
        List<Panel> ListaPanelInfo;

        public JanelaLimpezaPC()
        {

            ListaPanelCheckbox = new List<Panel>();
            ListaPanelInfo = new List<Panel>();
            InitializeComponent();
            CriarPanelsBody();
            CriarCheckboxs();           
            btnVoltar.Enabled = false;
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            HelperIdentificadorComponenteAtivo(true);
        }
        private void btnVoltar_Click(object sender, EventArgs e)
        {
            HelperIdentificadorComponenteAtivo(false);
        }
        private List<ElementosCheckbox> CriarCheckboxs()
        {
            string[] arrayNomeCheckbox =
            {
                "Limpeza de Arquivos Temporários",
                "Limpeza de Disco",
                "Otimização/Desfragmentação de Disco",
                "Otimização de Disco(somente)",
                "Limpeza de Log do Windows"
            };

            string[] arrayTextoInformacao =
            {
                "Executa a limpeza de arquivos temporários das pastas:\nSerão excluídos apenas Arquivos temporários:\r\nPasta Tempdo usuário\r\nPasta Download e Temp do Windows\r\ne pasta Recent",
                "Executa a Limpeza de Disco do Windows\r\nVocê pode executar:\r\nLimpeza de Disco Padrão - Funcionalidade mais básica de limpeza,\r\nLimpeza de Disco Personalizada - Abre a janela de Limpeza de Disco para que selecione os arquivos que deseja limpar manualmente\r\nÚltima Limpeza Personalizada realizada - Executa a Limpeza com as mesmas seleções escolhidas na última Limpeza de Disco Personalizada executada.",
                "Executa a Desfragmentação e Otimização em caso de HDD e Otimização em caso de SSD",
                "Executa somente Otimização de Disco - recomendado para SSD",
                "Executa Limpeza de Log do Windows - (INDISPONÍVEL)"
            };

            ElementosCheckbox checkboxs = new ElementosCheckbox();
            List<ElementosCheckbox> listaCheckBoxes = checkboxs.CriarLinkLabels(arrayNomeCheckbox, arrayTextoInformacao);

            for (int i = 0; i < listaCheckBoxes.Count(); i++)
            {
                System.Windows.Forms.CheckBox checkBox = new()
                {
                    Text = $"{listaCheckBoxes[i].TextoCheckBox}",
                    Location = new Point(0, 0),
                    AutoSize = true,
                    Font = new Font(Font.FontFamily, 12)
                };

                ListaPanelCheckbox[i].Controls.Add(checkBox);
            }

            return listaCheckBoxes;
        }

        private void CriarInfo(Panel panel_1)
        {
            List<ElementosCheckbox> listaCheckBoxes = CriarPanelsCheckbox(panel_1, 5);

            for (int i = 0; i < listaCheckBoxes.Count(); i++)
            {
                ListaPanelInfo[i].Controls.Add(listaCheckBoxes[i].LinkInformacao);
            }
        }

        private void CriarPanelsBody()
        {
            int larguraPanelBody = 550;
            int alturaPanelBody = 530;
            int localizacaoX = 70;
            int localizacaoY = 81;

            panel_1 = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                //BackColor = Color.Red,
                Enabled = true,
                Visible = true,
            };
            this.Controls.Add(panel_1);

            CriarInfo(panel_1);

             panel_2 = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                //BackColor = Color.Blue,
                Enabled = false,
                Visible = false
            };
            this.Controls.Add(panel_2);

            panel_3 = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                //BackColor = Color.Black,
                Enabled = false,
                Visible = false
            };
            this.Controls.Add(panel_3);
        }

        private List<ElementosCheckbox> CriarPanelsCheckbox(Panel panelPai, int qtdPanel)
        {
            int margemBody = 20;
            int alturaPanel = 35;
            int espacamento = 5;

            for (int i = 0; i < qtdPanel; i++)
            {
                Panel panelCheckbox = new Panel()
                {
                    Location = new Point(margemBody, margemBody + (i * (alturaPanel + espacamento))),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                    //Size = new Size(500, alturaPanel),
                    //BackColor = Color.AliceBlue,
                };

                panelPai.Controls.Add(panelCheckbox);

                ListaPanelCheckbox.Add(panelCheckbox);
            }

            List<ElementosCheckbox> qualquecoisax = CriarCheckboxs();

            for (int i = 0; i < qtdPanel; i++)
            {
                Panel panelInfo = new Panel()
                {
                    Location = new Point(margemBody + ListaPanelCheckbox[i].Width + espacamento, margemBody + (i * (alturaPanel + espacamento))),
                    //Size = new Size(25, alturaPanel),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,

                };

                panelPai.Controls.Add(panelInfo);
                ListaPanelInfo.Add(panelInfo);
            }

            return qualquecoisax;
        }

        private void HelperIdentificadorComponenteAtivo(bool isContinuar)
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
                    btnContinuar.Text = "Finalizar";
                    btnVoltar.Enabled = false;
                    btnVoltar.Visible = false;
                }
                else
                {
                    panel_2.Visible = false;
                    panel_2.Enabled = false;
                    panel_1.Visible = true;
                    panel_1.Enabled = true;
                    btnVoltar.Enabled = false;
                    btnContinuar.Text = "Continuar";
                }
            }
            else if (panel_3.Enabled)
            {

            }
        }
    }
}
