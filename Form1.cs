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
        private List<Opcao> OpcoesCriadas { get; set; } = new List<Opcao>();
        private List<RadioButton> RadioButtons { get; set; } = new List<RadioButton>();
        private static string CaminhoArquivoJson { get; set; } = @"C:\Users\stude\source\repos\ProjetoLimpezaDePCRefatoracao\ProjetoLimpezaDePCRefatoracao\Configs\appConfig.json";
        private static string TextoJson { get; set; } = File.ReadAllText(CaminhoArquivoJson);
        private JObject JsonObj { get; set; } = JObject.Parse(TextoJson);
        private Dictionary<string, string> textosOpcoes { get; set; }

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

             textosOpcoes = new Dictionary<string, string>()
            {
                { JsonObj["checkBox1"].ToString(), "Ser�o exclu�dos apenas Arquivos tempor�rios:\r\nPasta Temp do usu�rio\r\nPasta Download e Temp do Windows\r\nPasta Recent" },
                { JsonObj["checkBox2"].ToString(), "Executa a Limpeza de Disco do Windows\r\nVoc� pode executar:\r\nLimpeza de Disco Padr�o - Funcionalidade mais b�sica de limpeza,\r\nLimpeza de Disco Personalizada - Abre a janela de Limpeza de Disco para que selecione os arquivos que deseja limpar manualmente\r\n�ltima Limpeza Personalizada realizada - Executa a Limpeza com as mesmas sele��es escolhidas na �ltima Limpeza de Disco Personalizada executada." },
                { JsonObj["checkBox3"].ToString(), "Executa a Desfragmenta��o e Otimiza��o em caso de HDD e Otimiza��o em caso de SSD" },
                { JsonObj["checkBox4"].ToString(), "Executa somente Otimiza��o de Disco - recomendado para SSD" },
                { JsonObj["checkBox5"].ToString(), "Executa Limpeza de Log do Windows - (INDISPON�VEL)" }
            };

                int posicaoOpcao = 0;
                foreach (var opcao in textosOpcoes)
                {
                    Opcao opcaoCriada = PopularAdicionarOpcao(opcao.Key, opcao.Value, posicaoOpcao);
                    OpcoesCriadas.Add(opcaoCriada);

                    OpcoesCriadas[posicaoOpcao].CheckBox.CheckedChanged += new EventHandler(HelperCheckBoxIsChecked);

                    posicaoOpcao++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex} N�o foi poss�vel proceguir com a execu��o!");
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

            if (OpcoesCriadas[1].CheckBox == checkBox)
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
            try
            {
                List<string> ListaTiposLimpezaDeDisco = new List<string>()
                {
                    JsonObj["radioButton1"].ToString(),
                    JsonObj["radioButton2"].ToString(),
                    JsonObj["radioButton3"].ToString()
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
                        RadioButtons.Add(radioButton);
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
            catch (Exception ex)
            {
                MessageBox.Show($"{ex} N�o foi poss�vel proceguir com a execu��o!");
            }
        }

        private void btnContinuar_Click(object sender, EventArgs e)
        {
            Dictionary<CheckBox, Action> metodoCorrespondenteOpcoesSelecionadas = new();

            MetodosExecucao metodosExecucao = new MetodosExecucao();

            for (int posicao = 0; posicao < OpcoesCriadas.Count(); posicao++)
            {
                if (OpcoesCriadas[posicao].CheckBox.Checked)
                {
                    switch (posicao)
                    {
                        case 0:
                            metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesCriadas[0].CheckBox, metodosExecucao.ExecutarLimpezaArquivosTemporarios);
                            break;

                        case 1:
                            foreach (RadioButton r in RadioButtons)
                            {
                                if (r.Checked)
                                {
                                    if (r.Text == JsonObj["radioButton1"].ToString())
                                    {
                                        metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesCriadas[1].CheckBox, metodosExecucao.ExecutarLimpezaDeDiscoPadr�o);
                                        break;
                                    }
                                    if (r.Text == JsonObj["radioButton2"].ToString())
                                    {
                                        metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesCriadas[1].CheckBox, metodosExecucao.ExecutarLimpezaComConfigManual);
                                        break;
                                    }
                                    if (r.Text == JsonObj["radioButton3"].ToString())
                                    {
                                        metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesCriadas[1].CheckBox, metodosExecucao.ExecutarLimpezaDeDiscoComChaveExistente);
                                        break;
                                    }
                                }
                            }
                            break;

                        case 2:
                            metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesCriadas[2].CheckBox, metodosExecucao.ExecutarDesfragmentacaoOuOtimizacaoDeAcordoComMidia);
                            break;

                        case 3:
                            metodoCorrespondenteOpcoesSelecionadas.Add(OpcoesCriadas[3].CheckBox, metodosExecucao.ExecutarSomenteOtimizacao);
                            break;
                    }
                }
            }

            HelperIdentificadorComponenteAtivo(true);
            ComecaCriacaoPanel_2_Body(metodoCorrespondenteOpcoesSelecionadas);
        }

        private void ComecaCriacaoPanel_2_Body(Dictionary<CheckBox, Action> metodoCorrespondenteOpcoesSelecionadas)
        {
            tituloHeader.Text = "Op��es selecionadas";
            linkExcluirChavesCriadas.Enabled = false;
            linkExcluirChavesCriadas.Visible = false;

            Panel panelOpcoesSelecionadas = new Panel()
            {
                Location = new Point(0,0),
                Size = new Size(500, 100), 
                BackColor = Color.Azure
            };
            Panel_2_Body.Controls.Add(panelOpcoesSelecionadas);

            Panel panelDescricaoOpcoes = new Panel() //
            {
                Location = new Point(0, panelOpcoesSelecionadas.Height + 5),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Azure
            };
            Panel_2_Body.Controls.Add(panelDescricaoOpcoes);
            int cont = 0;

            foreach (var opcao in metodoCorrespondenteOpcoesSelecionadas)
            {              
                int tamanho = 20;
                int espacamento = 5;

                Panel panelLabelOpcaoSelecionada = new Panel()
                {
                    Location = new Point(0, tamanho * cont + espacamento),
                    Size = new Size(500, tamanho),
                    //BackColor= Color.Black
                };

                panelOpcoesSelecionadas.Controls.Add(panelLabelOpcaoSelecionada);

                Label opcaoEscolhida = new Label()
                {
                    Text = opcao.Key.Text,
                    AutoSize = true
                };

                if(opcao.Key.Text == JsonObj["checkBox2"].ToString())
                {
                    foreach( RadioButton r in RadioButtons)
                    {
                        if (r.Checked)
                        {
                            opcaoEscolhida.Text += $" - {r.Text}";
                        }
                    }                   
                }
                panelLabelOpcaoSelecionada.Controls.Add(opcaoEscolhida);

                foreach (var opcaoSelecionada in metodoCorrespondenteOpcoesSelecionadas)
                {
                    foreach (var textoOpcao in textosOpcoes)
                    {
                        if(textoOpcao.Key == opcaoSelecionada.Key.Text)
                        {
                            Panel panelDescricao = new Panel()
                            {
                                Location = new Point(0, cont * (tamanho + 20)),
                                AutoSize = true,
                                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                             
                            };
                            panelDescricaoOpcoes.Controls.Add(panelDescricao);

                            Label descricaoOpcaoSelecionada = new Label()
                            {
                                Text = textoOpcao.Value,
                                AutoSize = true,
                            };
                            panelDescricao.Controls.Add(descricaoOpcaoSelecionada);
                        }
                    } 
                }
                
                cont++;
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
                }
            }
            else if (Panel_3_Body.Enabled)
            {

            }
        }
   
    }
}

