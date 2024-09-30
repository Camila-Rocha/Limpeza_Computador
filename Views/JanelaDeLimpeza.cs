using Limpeza_Computador.Views;
using Newtonsoft.Json.Linq;
using Limpeza_Computador.Domain;
using Limpeza_Computador.Service;
using Limpeza_Computador.Views;
using CheckBox = System.Windows.Forms.CheckBox;
using RadioButton = System.Windows.Forms.RadioButton;

namespace Limpeza_Computador
{
    public partial class JanelaLimpezaPC : Form
    {
        private Panel? Panel_1_Body { get; set; }
        private Panel? Panel_2_Body { get; set; }
        private Panel? Panel_3_Body { get; set; }

        private List<ElementosDeOpcaoCheckbox> OpcoesCriadas { get; set; } = [];
        private Dictionary<CheckBox, Func<string>> CorrelacaoMetodosEOpcoesSelecionadas { get; set; } = new();

        public JanelaLimpezaPC()
        {
            InitializeComponent();
            CriarPanelsBody();
            btnVoltar.Enabled = false;
        }

        private void CriarPanelsBody()
        {

            int larguraPanelBody = 570;
            int alturaPanelBody = 451;
            int localizacaoX = 44;
            int localizacaoY = 115;

            Panel_1_Body = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = true,
                Visible = true,
                BorderStyle = BorderStyle.FixedSingle
            };
            panelBase.Controls.Add(Panel_1_Body);

            ComecarCriacaoOpcoesPanel_1_Body();

            Panel_2_Body = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = false,
                Visible = false
            };
            panelBase.Controls.Add(Panel_2_Body);

            Panel_3_Body = new Panel()
            {
                Size = new Size(larguraPanelBody, alturaPanelBody),
                Location = new Point(localizacaoX, localizacaoY),
                AutoScroll = true,
                Enabled = false,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle,
            };

            panelBase.Controls.Add(Panel_3_Body);
        }

        private void ComecarCriacaoOpcoesPanel_1_Body()
        {
            #region criacao de opcoes
            try
            {
                int posicaoOpcao = 0;
                ConteudoOpcaoCheckbox conteudoOpcaoCheckbox = new();
                Dictionary<string, string> NomesEDescricoesOpcaosCheckbox = new Dictionary<string, string>();
                NomesEDescricoesOpcaosCheckbox = conteudoOpcaoCheckbox.RetornarNomesEDescricoesOpcaosCheckbox();

                foreach (var opcao in NomesEDescricoesOpcaosCheckbox)
                {
                    ElementosDeOpcaoCheckbox? opcaoCriada = AdicionarOpcao(opcao.Key, opcao.Value, posicaoOpcao);
                    OpcoesCriadas.Add(opcaoCriada);
                    OpcoesCriadas[posicaoOpcao].CheckBox.CheckedChanged += new EventHandler(HelperCheckBoxIsChecked);
                    posicaoOpcao++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex} Não foi possível prosseguir com a execução!");
            }

            #endregion
        }

        private ElementosDeOpcaoCheckbox AdicionarOpcao(string textoCheckBox, string textoInformacaoIcone, int posicaoOpcao)
        {
            int margemBody = 20;
            int alturaPanel = 35;
            int espacamento = 5;

            ElementosDeOpcaoCheckbox opcao = new ElementosDeOpcaoCheckbox();

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

            if (posicaoOpcao == 1)
            {
                opcao.CheckBox.Enabled = true;
            }

            Panel_1_Body?.Controls.Add(opcao.PanelCheckBox);
            opcao.PanelCheckBox.Controls.Add(opcao.CheckBox);

            opcao.PanelIcone = new Panel()
            {
                Location = new Point(margemBody + opcao.CheckBox.Width + espacamento, margemBody + (posicaoOpcao * (alturaPanel + espacamento))),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };

            IconeInformacao iconeInformacao = new()
            {
                Texto = textoInformacaoIcone
            };
            iconeInformacao.EventoExibeMensagemFlutuante.SetToolTip(iconeInformacao.Icone, iconeInformacao.Texto);
            opcao.IconeInformacao = iconeInformacao;

            Panel_1_Body?.Controls.Add(opcao.PanelIcone);
            opcao.PanelIcone.Controls.Add(opcao.IconeInformacao.Icone);

            return opcao;
        }

        private void HelperCheckBoxIsChecked(object? sender, EventArgs? e)
        {
            HelperHabilitacaoBtnContinuar_CheckedChanged();

            _ = OpcoesCriadas[2].CheckBox.Checked ? OpcoesCriadas[3].CheckBox.Enabled = false : OpcoesCriadas[3].CheckBox.Enabled = true;
            _ = OpcoesCriadas[3].CheckBox.Checked ? OpcoesCriadas[2].CheckBox.Enabled = false : OpcoesCriadas[2].CheckBox.Enabled = true;
        }

        private void HelperHabilitacaoBtnContinuar_CheckedChanged()
        {
            bool algumCheckBoxSelecionado = false;

            foreach (ElementosDeOpcaoCheckbox opcao in OpcoesCriadas)
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

        private void BtnContinuar_Click(object sender, EventArgs e)
        {
            HelperIdentificadorComponenteAtivo(true);
            ExecutorDeTarefasDoSistemaService executorDeTarefasDoSistemaService = new();

            if (Panel_1_Body.Enabled)
            {
                btnContinuar.Text = "Continuar";
                btnVoltar.Visible = true;
            }
            else if (Panel_2_Body.Enabled)
            {
                Panel_2_Body.Controls.Clear();
                btnVoltar.Enabled = true;
                CorrelacaoMetodosEOpcoesSelecionadas.Clear();
                for (int posicao = 0; posicao < OpcoesCriadas.Count(); posicao++)
                {
                    if (OpcoesCriadas[posicao].CheckBox.Checked)
                    {
                        switch (posicao)
                        {
                            case 0:
                                CorrelacaoMetodosEOpcoesSelecionadas.Add(OpcoesCriadas[0].CheckBox, executorDeTarefasDoSistemaService.ExecutarLimpezaArquivosTemporarios);
                                break;

                            case 1:
                                CorrelacaoMetodosEOpcoesSelecionadas.Add(OpcoesCriadas[1].CheckBox, executorDeTarefasDoSistemaService.ExecutarLimpezaDeDiscoPadrão);
                                break;

                            case 2:
                                CorrelacaoMetodosEOpcoesSelecionadas.Add(OpcoesCriadas[2].CheckBox, executorDeTarefasDoSistemaService.ExecutarDesfragmentacaoOuOtimizacaoDeAcordoComMidia);
                                break;

                            case 3:
                                CorrelacaoMetodosEOpcoesSelecionadas.Add(OpcoesCriadas[3].CheckBox, executorDeTarefasDoSistemaService.ExecutarSomenteOtimizacao);
                                break;
                        }
                    }
                }
                ComecaCriacaoPanel_2_Body(CorrelacaoMetodosEOpcoesSelecionadas);
            }
            else
            {
                tituloHeader.Text = "Processando... Aguarde.";
                Application.DoEvents();
                ComecaCriacaoPanel_3_Body(CorrelacaoMetodosEOpcoesSelecionadas);
                Application.DoEvents();
                tituloHeader.Text = "Resumo da execução";
                CorrelacaoMetodosEOpcoesSelecionadas.Clear();
                btnContinuar.Enabled = true;
            }
        }

        private void ComecaCriacaoPanel_2_Body(Dictionary<CheckBox, Func<string>> correlacaoMetodosEOpcoesSelecionadas)
        {
            JObject JsonObj = ConfigJsonService.CarregarConfiguracoes();
            ConteudoOpcaoCheckbox conteudoOpcaoCheckbox = new();
            Dictionary<string, string> NomesEDescricoesOpcaosCheckbox = conteudoOpcaoCheckbox.RetornarNomesEDescricoesOpcaosCheckbox();

            tituloHeader.Text = "Resumo de seleção";

            int localizacaoY = 0;
            int cont = 0;
            int espacamento = 5;

            ////  // //
            Panel panelTituloOpcoes = new()
            {
                Location = new Point(0, 0),
                Size = new Size(550, 0),
                AutoSize = true
            };
            Panel_2_Body.Controls.Add(panelTituloOpcoes);

            Label LabelTituloOpcoes = new()
            {
                Location = new Point(0, 0),
                Text = "Opções selecionadas:",
                Font = new Font(Font.FontFamily, 13, FontStyle.Bold),
                AutoSize = true
            };
            panelTituloOpcoes.Controls.Add(LabelTituloOpcoes);
            // // //

            Panel panelOpcoesSelecionadas = new()
            {
                Location = new Point(0, (espacamento * 3) + panelTituloOpcoes.Height),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            Panel_2_Body.Controls.Add(panelOpcoesSelecionadas);

            foreach (var opcao in correlacaoMetodosEOpcoesSelecionadas)
            {
                Panel panelOpcao = new()
                {
                    Location = new Point(0, cont * espacamento + localizacaoY),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink
                };
                panelOpcoesSelecionadas.Controls.Add(panelOpcao);

                Label opcaoEscolhida = new()
                {
                    Text = opcao.Key.Text,
                    AutoSize = true,
                    MaximumSize = new Size(540, 0),
                    Font = new Font(Font.FontFamily, 10, FontStyle.Bold)
                };
                panelOpcao.Controls.Add(opcaoEscolhida);

                localizacaoY += panelOpcao.Height;
                cont++;
            } // primeiro panel pronto           
        }

        private void ComecaCriacaoPanel_3_Body(Dictionary<CheckBox, Func<string>> metodoCorrespondenteOpcoesSelecionadas)
        {
            List<Panel> panelsResultadoExecucao = new();
            int contador = 0;
            int espacamento = 25;
            int localizacaoY = 0;

            foreach (var metodo in metodoCorrespondenteOpcoesSelecionadas)
            {
                Panel panelResultado = new()
                {
                    Location = new Point(0, contador * espacamento + localizacaoY),
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                };
                panelsResultadoExecucao.Add(panelResultado);
                Panel_3_Body.Controls.Add(panelResultado);

                Label resultadoExecucao = new()
                {
                    Text = metodo.Value(),
                    AutoSize = true,
                    MaximumSize = new Size(540, 0),
                    Font = new Font(Font.FontFamily, 13)
                };
                panelResultado.Controls.Add(resultadoExecucao);

                localizacaoY += panelResultado.Height;
                contador++;
            }
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
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
                    btnContinuar.BackColor = Color.LightGreen;
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
                    btnContinuar.BackColor = Color.White;
                    btnContinuar.Enabled = false;
                    btnVoltar.Enabled = false;
                    btnVoltar.Visible = false;

                }
                else
                {
                    Panel_2_Body.Visible = false;
                    Panel_2_Body.Enabled = false;
                    Panel_2_Body.Controls.Clear();
                    Panel_1_Body.Visible = true;
                    Panel_1_Body.Enabled = true;
                    btnVoltar.Enabled = false;
                    btnContinuar.Text = "Continuar";
                    btnContinuar.BackColor = Color.White;
                    tituloHeader.Text = "Selecione as opções que deseja executar";
                }
            }
            else if (Panel_3_Body.Enabled)
            {
                LimparSelecoesCheckbox();
                Panel_3_Body.Visible = false;
                Panel_3_Body.Enabled = false;
                Panel_3_Body.Controls.Clear();
                Panel_1_Body.Visible = true;
                Panel_1_Body.Enabled = true;
                tituloHeader.Text = "Selecione as opções que deseja executar";
            }
        }

        public void LimparSelecoesCheckbox()
        {
            for (int i = 0; i < OpcoesCriadas.Count(); i++)
            {
                OpcoesCriadas[i].CheckBox.Checked = false;
            }
        }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

