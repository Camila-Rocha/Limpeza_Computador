using Newtonsoft.Json.Linq;
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

        private Dictionary<string, string> TextosOpcoes { get; set; }
        private List<Opcao> OpcoesESubOpcoesCriadas { get; set; } = new();

        private static string CaminhoArquivoJson { get; set; } = @"C:\Users\stude\source\repos\ProjetoLimpezaDePCRefatoracao\ProjetoLimpezaDePCRefatoracao\Configs\appConfig.json";
        private static string TextoJson { get; set; } = File.ReadAllText(CaminhoArquivoJson);
        private JObject JsonObj { get; set; } = JObject.Parse(TextoJson);
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
            try
            {
                TextosOpcoes = new Dictionary<string, string>()
            {
                { JsonObj["checkBox1"]!.ToString(), "Serão excluídos apenas Arquivos temporários:\r\nPasta Temp do usuário\r\nPasta Download e Temp do Windows\r\nPasta Recent" },
                { JsonObj["checkBox2"]!.ToString(), "Executa a Limpeza de Disco do Windows\r\nVocê pode executar:\r\nLimpeza de Disco Padrão - Funcionalidade mais básica de limpeza,\r\nLimpeza de Disco Personalizada - Abre a janela de Limpeza de Disco para que selecione os arquivos que deseja limpar manualmente\r\nÚltima Limpeza Personalizada realizada - Executa a Limpeza com as mesmas seleções escolhidas na última Limpeza de Disco Personalizada executada." },
                { JsonObj["checkBox3"]!.ToString(), "Executa a Desfragmentação e Otimização em caso de HDD e Otimização em caso de SSD" },
                { JsonObj["checkBox4"]!.ToString(), "Executa somente Otimização de Disco - recomendado para SSD" },
                { JsonObj["checkBox5"]!.ToString(), "Executa Limpeza de Log do Windows - (INDISPONÍVEL)" }
            };

                int posicaoOpcao = 0;
                SubOpcoesRadiobuttons subOpcoesRadiobuttons = new SubOpcoesRadiobuttons();

                foreach (var opcao in TextosOpcoes)
                {
                    Opcao opcaoCriada = PopularAdicionarOpcao(opcao.Key, opcao.Value, posicaoOpcao);

                    if (opcao.Key == JsonObj["checkBox2"].ToString())
                    {
                        opcaoCriada.RadioButtons = subOpcoesRadiobuttons.RadioButtons;
                    }
                    OpcoesESubOpcoesCriadas.Add(opcaoCriada);
                    OpcoesESubOpcoesCriadas[posicaoOpcao].CheckBox.CheckedChanged += new EventHandler(HelperCheckBoxIsChecked);

                    posicaoOpcao++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex} Não foi possível proceguir com a execução!");
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

            if (OpcoesESubOpcoesCriadas[1].CheckBox == checkBox)
            {
                LifeTimeOpcoesSecundariasPanelCheckBoxPosicao_1(checkBox);
            }

            HelperHabilitacaoBtnContinuar_CheckedChanged();

            _ = OpcoesESubOpcoesCriadas[2].CheckBox.Checked ? OpcoesESubOpcoesCriadas[3].CheckBox.Enabled = false : OpcoesESubOpcoesCriadas[3].CheckBox.Enabled = true;
            _ = OpcoesESubOpcoesCriadas[3].CheckBox.Checked ? OpcoesESubOpcoesCriadas[2].CheckBox.Enabled = false : OpcoesESubOpcoesCriadas[2].CheckBox.Enabled = true;
        }

        private void HelperHabilitacaoBtnContinuar_CheckedChanged()
        {
            bool algumCheckBoxSelecionado = false;

            foreach (Opcao opcao in OpcoesESubOpcoesCriadas)
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
            try
            {
                int margemBody = 20;
                int espacamento = 5;
                int alturaPanelCheckbox = 0;

                Panel panelRadioButton = new()
                {
                    Location = new Point(margemBody, OpcoesESubOpcoesCriadas[1].PanelCheckBox.Height),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };

                if (componenteEscutado.Checked == true)
                {
                    OpcoesESubOpcoesCriadas[1].PanelCheckBox.Controls.Add(panelRadioButton);

                    for (int i = 0; i < OpcoesESubOpcoesCriadas[1].RadioButtons.Count; i++)
                    {
                        panelRadioButton.Controls.Add(OpcoesESubOpcoesCriadas[1].RadioButtons[i]);
                    }
                }
                else
                {
                    foreach (Control control in OpcoesESubOpcoesCriadas[1].PanelCheckBox.Controls)
                    {
                        if (control.GetType() == typeof(Panel))
                        {
                            OpcoesESubOpcoesCriadas[1].PanelCheckBox.Controls.Remove(control);
                        }
                    }
                }

                for (int i = 0; i < OpcoesESubOpcoesCriadas.Count(); i++)
                {
                    OpcoesESubOpcoesCriadas[i].PanelCheckBox.Location = new Point(margemBody, margemBody + (alturaPanelCheckbox + espacamento));
                    OpcoesESubOpcoesCriadas[i].PanelIcone.Location = new Point(margemBody + OpcoesESubOpcoesCriadas[i].PanelCheckBox.Width + espacamento, margemBody + alturaPanelCheckbox + espacamento);

                    alturaPanelCheckbox += OpcoesESubOpcoesCriadas[i].PanelCheckBox.Height;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex} Não foi possível proceguir com a execução!");
            }
        }

        private void BtnContinuar_Click(object sender, EventArgs e)
        {
            Dictionary<CheckBox, Action> metodoCorrespondenteOpcoesSelecionadas = new();

            MetodosExecucao metodosExecucao = new MetodosExecucao();

            for (int posicao = 0; posicao < OpcoesESubOpcoesCriadas.Count(); posicao++)
            {
                if (OpcoesESubOpcoesCriadas[posicao].CheckBox.Checked)
                {
                    switch (posicao)
                    {
                        case 0:
                            metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesESubOpcoesCriadas[0].CheckBox, metodosExecucao.ExecutarLimpezaArquivosTemporarios);
                            break;

                        case 1:
                            foreach (RadioButton r in OpcoesESubOpcoesCriadas[1].RadioButtons)
                            {
                                if (r.Checked)
                                {
                                    if (r.Text == JsonObj["radioButton1"].ToString())
                                    {
                                        metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesESubOpcoesCriadas[1].CheckBox, metodosExecucao.ExecutarLimpezaDeDiscoPadrão);
                                        break;
                                    }
                                    if (r.Text == JsonObj["radioButton2"].ToString())
                                    {
                                        metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesESubOpcoesCriadas[1].CheckBox, metodosExecucao.ExecutarLimpezaComConfigManual);
                                        break;
                                    }
                                    if (r.Text == JsonObj["radioButton3"].ToString())
                                    {
                                        metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesESubOpcoesCriadas[1].CheckBox, metodosExecucao.ExecutarLimpezaDeDiscoComChaveExistente);
                                        break;
                                    }
                                }
                            }
                            break;

                        case 2:
                            metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesESubOpcoesCriadas[2].CheckBox, metodosExecucao.ExecutarDesfragmentacaoOuOtimizacaoDeAcordoComMidia);
                            break;

                        case 3:
                            metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesESubOpcoesCriadas[3].CheckBox, metodosExecucao.ExecutarSomenteOtimizacao);
                            break;
                    }
                }
            }

            HelperIdentificadorComponenteAtivo(true);
            ComecaCriacaoPanel_2_Body(metodoCorrespondenteOpcoesSelecionadas);
        }

        private void ComecaCriacaoPanel_2_Body(Dictionary<CheckBox, Action> metodoCorrespondenteOpcoesSelecionadas)
        {
            tituloHeader.Text = "Opções selecionadas";
            linkExcluirChavesCriadas.Enabled = false;
            linkExcluirChavesCriadas.Visible = false;

            Panel panelOpcoesSelecionadas = new()
            {
                Location = new Point(0, 0),
                Size = new Size(550, 90),
            };
            Panel_2_Body.Controls.Add(panelOpcoesSelecionadas);

            Panel panelDescricaoOpcoes = new()
            {
                Location = new Point(0, panelOpcoesSelecionadas.Height + 150),
                Size = new Size(550, 0),
                AutoSize = true,
                BackColor = Color.AliceBlue,
            };
            Panel_2_Body.Controls.Add(panelDescricaoOpcoes);

            int cont = 0;
            int tamanho = 20;
            int espacamento = 5;

            foreach (var opcao in metodoCorrespondenteOpcoesSelecionadas)
            {
                Panel panelOpcao = new()
                {
                    Location = new Point(0, tamanho * cont + espacamento),
                    Size = new Size(550, tamanho)
                };

                panelOpcoesSelecionadas.Controls.Add(panelOpcao);

                Label opcaoEscolhida = new()
                {
                    Text = opcao.Key.Text,
                    AutoSize = true,
                    MaximumSize = new Size(550, 0),
                    ForeColor = Opcao.CoresCadaOpcao[cont],
                    Font = new Font(Font.FontFamily, 12, FontStyle.Bold),

                };

                if (opcao.Key.Text == JsonObj["checkBox2"].ToString())
                {
                    foreach (RadioButton r in OpcoesESubOpcoesCriadas[1].RadioButtons)
                    {
                        if (r.Checked)
                        {
                            opcaoEscolhida.Text += $" - {r.Text}";
                        }
                    }
                }
                espacamento += espacamento;
                panelOpcao.Controls.Add(opcaoEscolhida);
                cont++;
            }

            List<Panel> ContemPanelsDescricao = new();
            int alturaPanelDescricao = 0;
            int contador = 0;
            cont = 0;
            espacamento = 10;

            foreach (var opcaoSelecionada in metodoCorrespondenteOpcoesSelecionadas)
            {
                foreach (var textoOpcao in TextosOpcoes)
                {
                    if (textoOpcao.Key == opcaoSelecionada.Key.Text)
                    {
                        if (ContemPanelsDescricao.Count == 0)
                        {
                            Panel panelContemDescricao_1 = new()
                            {
                                Location = new Point(0, 0),
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowAndShrink
                            };
                            ContemPanelsDescricao.Add(panelContemDescricao_1);
                            panelDescricaoOpcoes.Controls.Add(panelContemDescricao_1);

                            Label labelDescricao = new()
                            {
                                Text = textoOpcao.Value,
                                AutoSize = true,
                                MaximumSize = new Size(550, 0),
                                ForeColor = Opcao.CoresCadaOpcao[cont]
                            };
                            panelContemDescricao_1.Controls.Add(labelDescricao);
                        }
                        else
                        {
                            alturaPanelDescricao += ContemPanelsDescricao[contador].Height;
                            Panel panelContemDescricao = new()
                            {
                                Location = new Point(0, alturaPanelDescricao + espacamento),
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowAndShrink
                            };
                            ContemPanelsDescricao.Add(panelContemDescricao);
                            panelDescricaoOpcoes.Controls.Add(panelContemDescricao);

                            Label labelDescricao = new()
                            {
                                Text = textoOpcao.Value,
                                AutoSize = true,
                                MaximumSize = new Size(550, 0),
                                ForeColor = Opcao.CoresCadaOpcao[cont]
                            };
                            panelContemDescricao.Controls.Add(labelDescricao);
                            contador++;
                            espacamento += espacamento;
                        }
                        cont++;
                    }
                }
            }
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
                    tituloHeader.Text = "Selecione as opções que deseja executar";
                }
            }
            else if (Panel_3_Body.Enabled)
            {

            }
        }

        private void LinkExcluirChavesCriadas_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}

