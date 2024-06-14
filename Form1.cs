using ProjetoLimpezaDePCRefatoracao.Domain;
using CheckBox = System.Windows.Forms.CheckBox;
using RadioButton = System.Windows.Forms.RadioButton;

namespace ProjetoLimpezaDePCRefatoracao
{
    public delegate void meuDelegate(object sender, PaintEventArgs e);

    public partial class JanelaLimpezaPC : Form
    {
        Panel panel_1_Body;
        Panel panel_2;
        Panel panel_3;

        public JanelaLimpezaPC()
        {
            InitializeComponent();
            CriarPanelsBody();
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

        private void ComecarCriacaoOpcoesPanel_1_Body(Panel panel_1_Body)
        {
            #region criacao de opcoes
            List<Opcao> opcoesCriadas = new();

            Dictionary<string, string> textosOpcoes = new Dictionary<string, string>()
            {
                { "Limpeza de Arquivos Temporários", "Executa a limpeza de arquivos temporários das pastas:\nSerão excluídos apenas Arquivos temporários:\r\nPasta Tempdo usuário\r\nPasta Download e Temp do Windows\r\ne pasta Recent" },
                { "Limpeza de Disco", "Executa a Limpeza de Disco do Windows\r\nVocê pode executar:\r\nLimpeza de Disco Padrão - Funcionalidade mais básica de limpeza,\r\nLimpeza de Disco Personalizada - Abre a janela de Limpeza de Disco para que selecione os arquivos que deseja limpar manualmente\r\nÚltima Limpeza Personalizada realizada - Executa a Limpeza com as mesmas seleções escolhidas na última Limpeza de Disco Personalizada executada." },
                { "Otimização/Desfragmentação de Disco", "Executa a Desfragmentação e Otimização em caso de HDD e Otimização em caso de SSD" },
                { "Otimização de Disco(somente)", "Executa somente Otimização de Disco - recomendado para SSD" },
                { "Limpeza de Log do Windows", "Executa Limpeza de Log do Windows - (INDISPONÍVEL)" }
            };

            int posicaoOpcao = 0;
            foreach (var opcao in textosOpcoes)
            {
                Opcao opcaoCriada = PopularAdicionarOpcao(opcao.Key, opcao.Value, posicaoOpcao);
                opcoesCriadas.Add(opcaoCriada);

                posicaoOpcao++;
            }

            opcoesCriadas[1].CheckBox.CheckedChanged += (sender, e) => HelperCheckBoxIsChecked(sender, e, opcoesCriadas);
            #endregion
        }

        private void HelperCheckBoxIsChecked(object? sender, EventArgs? e, List<Opcao> opcoesCriadas)
        {
            CheckBox checkBox = sender as CheckBox;

            ComecarCriacaoOpcoesSecundariasPanelCheckBoxPosicao_1(opcoesCriadas, checkBox);           
          
        }
        private void ComecarCriacaoOpcoesSecundariasPanelCheckBoxPosicao_1(List<Opcao> opcoesCriadas, CheckBox componenteEscutado)
        {
            List<string> ListaTiposLimpezaDeDisco = new List<string>()
             {
                "Limpeza Padrão",
                "Limpeza Personalizada",
                "Usar Última Limpeza Personalizada Realizada (Se houver)"
             };
            int margemBody = 20;
            int espacamento = 5;
            int alturaPanelCheckbox = 0;

            Panel panelRadioButton = new()
            {
                Location = new Point(margemBody, opcoesCriadas[1].PanelCheckBox.Height),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };

            if (componenteEscutado.Checked == true)
            {               
                opcoesCriadas[1].PanelCheckBox.Controls.Add(panelRadioButton);

                for (int i = 0; i < ListaTiposLimpezaDeDisco.Count; i++)
                {
                    RadioButton radioButton = new RadioButton
                    {
                        Text = $"{ListaTiposLimpezaDeDisco[i]}",
                        Font = new Font(Font.FontFamily, 11),
                        AutoSize = true,
                        Location = new Point(0, 30 * i + espacamento)
                    };
                    panelRadioButton.Controls.Add(radioButton);
                }

                for (int i = 0; i < opcoesCriadas.Count(); i++)
                {
                    opcoesCriadas[i].PanelCheckBox.Location = new Point(margemBody, margemBody + (alturaPanelCheckbox + espacamento));
                    opcoesCriadas[i].PanelIcone.Location = new Point(margemBody + opcoesCriadas[i].PanelCheckBox.Width + espacamento, margemBody + alturaPanelCheckbox + espacamento);

                    alturaPanelCheckbox += opcoesCriadas[i].PanelCheckBox.Height;
                }
            }
            else
            {       // panel nao esta sendo apagado. Corrigir
                panel_1_Body.Controls.Remove(panelRadioButton);
                ListaTiposLimpezaDeDisco.Clear();

                for (int i = 0; i < opcoesCriadas.Count(); i++)
                {
                    opcoesCriadas[i].PanelCheckBox.Location = new Point(margemBody, margemBody + (alturaPanelCheckbox + espacamento));
                    opcoesCriadas[i].PanelIcone.Location = new Point(margemBody + opcoesCriadas[i].PanelCheckBox.Width + espacamento, margemBody + alturaPanelCheckbox + espacamento);

                    alturaPanelCheckbox += opcoesCriadas[i].PanelCheckBox.Height;
                }
            }
            
        }
        private void RemoverCriacaoOpcoesSecundariasPanelCheckBoxPosicao_1()
        {

        }
        private Opcao PopularAdicionarOpcao(string textoCheckBox, string textoInformacaoIcone, int posicaoOpcao)
        {
            int margemBody = 20;
            int alturaPanel = 35;
            int espacamento = 5;

            Opcao opcao = new Opcao();

            opcao.PanelCheckBox = new Panel()
            {
                Location = new Point(margemBody, margemBody + (posicaoOpcao * (alturaPanel + espacamento))),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink

            };

            opcao.CheckBox = new CheckBox()
            {
                Text = $"{textoCheckBox}",
                Location = new Point(0, 0),
                AutoSize = true,
                Font = new Font(Font.FontFamily, 12),
                 BackColor = Color.Plum
            };

            panel_1_Body.Controls.Add(opcao.PanelCheckBox);
            opcao.PanelCheckBox.Controls.Add(opcao.CheckBox);

            opcao.PanelIcone = new Panel()
            {
                Location = new Point(margemBody + opcao.CheckBox.Width + espacamento, margemBody + (posicaoOpcao * (alturaPanel + espacamento))),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Bisque
            };

            IconeInformacao iconeInformacao = new();
            iconeInformacao.Texto = textoInformacaoIcone;
            iconeInformacao.EventoExibeMensagemFlutuante.SetToolTip(iconeInformacao.Icone, iconeInformacao.Texto);
            opcao.IconeInformacao = iconeInformacao;

            panel_1_Body.Controls.Add(opcao.PanelIcone);
            opcao.PanelIcone.Controls.Add(opcao.IconeInformacao.Icone);

            return opcao;
        }

        private void CriarPanelsBody()
        {
            int larguraPanelBody = 550;
            int alturaPanelBody = 530;
            int localizacaoX = 70;
            int localizacaoY = 81;

            panel_1_Body = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = true,
                Visible = true,
            };
            this.Controls.Add(panel_1_Body);

            ComecarCriacaoOpcoesPanel_1_Body(panel_1_Body);

            panel_2 = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = false,
                Visible = false
            };
            this.Controls.Add(panel_2);

            panel_3 = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = false,
                Visible = false
            };

            this.Controls.Add(panel_3);
        }

        private void HelperIdentificadorComponenteAtivo(bool isContinuar)
        {
            if (panel_1_Body.Enabled)
            {
                var nome = nameof(panel_1_Body).Split('_').Last() == "1";

                if (isContinuar)
                {
                    panel_1_Body.Visible = false;
                    panel_1_Body.Enabled = false;
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
                    panel_1_Body.Visible = true;
                    panel_1_Body.Enabled = true;
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

