using ProjetoLimpezaDePCRefatoracao.Domain;
using CheckBox = System.Windows.Forms.CheckBox;
using RadioButton = System.Windows.Forms.RadioButton;

namespace ProjetoLimpezaDePCRefatoracao
{
    public delegate void meuDelegate(object sender, PaintEventArgs e);

    public partial class JanelaLimpezaPC : Form
    {
        private Panel Panel_1_Body { get; set; }
        private Panel Panel_2_Body { get; set; }
        private Panel Panel_3_Body { get; set; }
        private List<Opcao> OpcoesCriadas { get; set; } = new List<Opcao>();

        public JanelaLimpezaPC()
        {
            InitializeComponent();
            CriarPanelsBody();
            btnVoltar.Enabled = false;
        }

        private void CriarPanelsBody()
        {
            int larguraPanelBody = 550;
            int alturaPanelBody = 530;
            int localizacaoX = 70;
            int localizacaoY = 81;

            Panel_1_Body = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = true,
                Visible = true,
            };
            this.Controls.Add(Panel_1_Body);

            ComecarCriacaoOpcoesPanel_1_Body(Panel_1_Body);

            Panel_2_Body = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = false,
                Visible = false
            };
            this.Controls.Add(Panel_2_Body);

            Panel_3_Body = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = false,
                Visible = false
            };

            this.Controls.Add(Panel_3_Body);
        }

        private void ComecarCriacaoOpcoesPanel_1_Body(Panel panel_1_Body)
        {
            #region criacao de opcoes

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
                OpcoesCriadas.Add(opcaoCriada);

                OpcoesCriadas[posicaoOpcao].CheckBox.CheckedChanged += new EventHandler(HelperCheckBoxIsChecked);

                posicaoOpcao++;
            }            
            #endregion
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
                Font = new Font(Font.FontFamily, 12)
            };

            if (posicaoOpcao == 4)
            {
                opcao.CheckBox.Enabled = false;
            }

            Panel_1_Body.Controls.Add(opcao.PanelCheckBox);
            opcao.PanelCheckBox.Controls.Add(opcao.CheckBox);

            opcao.PanelIcone = new Panel()
            {
                Location = new Point(margemBody + opcao.CheckBox.Width + espacamento, margemBody + (posicaoOpcao * (alturaPanel + espacamento))),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            IconeInformacao iconeInformacao = new();
            iconeInformacao.Texto = textoInformacaoIcone;
            iconeInformacao.EventoExibeMensagemFlutuante.SetToolTip(iconeInformacao.Icone, iconeInformacao.Texto);
            opcao.IconeInformacao = iconeInformacao;

            Panel_1_Body.Controls.Add(opcao.PanelIcone);
            opcao.PanelIcone.Controls.Add(opcao.IconeInformacao.Icone);

            return opcao;
        }

        private void HelperCheckBoxIsChecked(object? sender, EventArgs? e)
        {
            CheckBox checkBox = sender as CheckBox;
            
            if(OpcoesCriadas[1].CheckBox == checkBox)
            {
                LifeTimeOpcoesSecundariasPanelCheckBoxPosicao_1(checkBox);
            }

            HelperHabilitacaoBtnContinuar_CheckedChanged();

            _ = OpcoesCriadas[2].CheckBox.Checked ? OpcoesCriadas[3].CheckBox.Enabled = false : OpcoesCriadas[3].CheckBox.Enabled = true;
            _ = OpcoesCriadas[3].CheckBox.Checked ? OpcoesCriadas[2].CheckBox.Enabled = false : OpcoesCriadas[2].CheckBox.Enabled = true;
        }

        private void HelperHabilitacaoBtnContinuar_CheckedChanged()
        {
            bool algumCheckBoxSelecionado = false;

            foreach (Opcao opcao in OpcoesCriadas)
            {
                if (opcao.CheckBox.Checked)
                {
                    algumCheckBoxSelecionado = true;
                }
            }

            if (algumCheckBoxSelecionado)
            {
                btnContinuar.Enabled = true;
                btnContinuar.ForeColor = Color.Black;
            }
            else
            {
                btnContinuar.Enabled = false;
                btnContinuar.ForeColor = Color.Gray;
            }          
        }

        private void LifeTimeOpcoesSecundariasPanelCheckBoxPosicao_1(CheckBox componenteEscutado)
        {
            List<RadioButton> radioButtons = new List<RadioButton>();

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
                Location = new Point(margemBody, OpcoesCriadas[1].PanelCheckBox.Height),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            if (componenteEscutado.Checked == true)
            {
                OpcoesCriadas[1].PanelCheckBox.Controls.Add(panelRadioButton);

                for (int i = 0; i < ListaTiposLimpezaDeDisco.Count; i++)
                {
                    RadioButton radioButton = new RadioButton
                    {
                        Text = $"{ListaTiposLimpezaDeDisco[i]}",
                        Font = new Font(Font.FontFamily, 11),
                        AutoSize = true,
                        Location = new Point(0, 30 * i + espacamento),
                        Checked = (i == 0)
                    };
                    radioButtons.Add(radioButton);
                    panelRadioButton.Controls.Add(radioButton);
                }
            }
            else
            {
                foreach (Control control in OpcoesCriadas[1].PanelCheckBox.Controls)
                {
                    if (control.GetType() == typeof(Panel))
                    {
                        OpcoesCriadas[1].PanelCheckBox.Controls.Remove(control);
                    }
                }
            }

            for (int i = 0; i < OpcoesCriadas.Count(); i++)
            {
                OpcoesCriadas[i].PanelCheckBox.Location = new Point(margemBody, margemBody + (alturaPanelCheckbox + espacamento));
                OpcoesCriadas[i].PanelIcone.Location = new Point(margemBody + OpcoesCriadas[i].PanelCheckBox.Width + espacamento, margemBody + alturaPanelCheckbox + espacamento);

                alturaPanelCheckbox += OpcoesCriadas[i].PanelCheckBox.Height;
            }
    
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            int posicao = 0;
            MetodosExecucao metodosExecucao = new MetodosExecucao();

            Dictionary<CheckBox, Action> metodoCorrespondenteCheckBox = new()
            {
                {OpcoesCriadas[0].CheckBox, metodosExecucao.ExecutarLimpezaArquivosTemporarios },                
                {OpcoesCriadas[2].CheckBox, metodosExecucao.ExecutarDesfragmentacaoOuOtimizacaoDeAcordoComMidia },
                {OpcoesCriadas[3].CheckBox, metodosExecucao.ExecutarSomenteOtimizacao }
               
            };
            switch (posicao)
            {
                case 0: metodosExecucao.ExecutarLimpezaArquivosTemporarios();
                    break;

                case 1:
                    foreach (Control control in OpcoesCriadas[1].PanelCheckBox.Controls)
                    {
                        if (control is RadioButton radioButton)
                        {
                            if (radioButton.Checked)
                            {

                            }
                        }
                    }
                    break;
            }

            Dictionary<CheckBox, Action> metodoCorrespondenteCheckBoxMarcado = new();

            

            for (int i = 0; i < OpcoesCriadas.Count(); i++)
            {
                if (OpcoesCriadas[i].CheckBox.Checked)
                {
                        
                }
            }
            

            HelperIdentificadorComponenteAtivo(true);

        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            HelperIdentificadorComponenteAtivo(false);
        }

        private void HelperIdentificadorComponenteAtivo(bool isContinuar)
        {
            if (Panel_1_Body.Enabled)
            {            
                if (isContinuar)
                {
                    Panel_1_Body.Visible = false;
                    Panel_1_Body.Enabled = false;
                    Panel_2_Body.Visible = true;
                    Panel_2_Body.Enabled = true;
                    btnVoltar.Enabled = true;
                    btnContinuar.Text = "Executar";
                }
            }

            else if (Panel_2_Body.Enabled)
            {
                if (isContinuar)
                {
                    Panel_2_Body.Visible = false;
                    Panel_2_Body.Enabled = false;
                    Panel_3_Body.Visible = true;
                    Panel_3_Body.Enabled = true;
                    btnContinuar.Text = "Finalizar";
                    btnVoltar.Enabled = false;
                    btnVoltar.Visible = false;
                }
                else
                {
                    Panel_2_Body.Visible = false;
                    Panel_2_Body.Enabled = false;
                    Panel_1_Body.Visible = true;
                    Panel_1_Body.Enabled = true;
                    btnVoltar.Enabled = false;
                    btnContinuar.Text = "Continuar";
                }
            }
            else if (Panel_3_Body.Enabled)
            {

            }
        }
    }
}

