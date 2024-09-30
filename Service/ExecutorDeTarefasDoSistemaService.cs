using System.Diagnostics;

namespace Limpeza_Computador.Service
{
    public class ExecutorDeTarefasDoSistemaService : SistemaOperacionalHelperService
    {
        public string ExecutarLimpezaArquivosTemporarios()
        {
            int contPastas = 0;
            int contArquivos = 0;

            string usuarioDoSistema = Environment.UserName;
            string[] caminhosPastas =
            {
                $@"C:\Users\{usuarioDoSistema}\AppData\Local\Temp",
                $@"C:\Windows\SoftwareDistribution\Download",
                $@"C:\Windows\Temp",
                Environment.GetFolderPath(Environment.SpecialFolder.Recent)
            };

            try
            {
                foreach (string caminho in caminhosPastas)
                {
                    string[] arquivos = Directory.GetFiles(caminho);
                    string[] pastas = Directory.GetDirectories(caminho);

                    foreach (string arquivo in arquivos)
                    {
                        try
                        {
                            File.Delete(arquivo);
                            contArquivos++;
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    foreach (string pasta in pastas)
                    {
                        try
                        {
                            Directory.Delete(pasta);
                            contPastas++;
                        }

                        catch
                        {
                            continue;
                        }
                    }
                }
                return $"Limpeza de arquivos temporários executado com sucesso.\nForam deletados: {contPastas} pastas e {contArquivos} arquivos";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "Erro ao executar limpeza de arquivos temporários";
            }
        }

        public string ExecutarDesfragmentacaoOuOtimizacaoDeAcordoComMidia()
        {
            string resultadoOtimizacaoDesfragmentacao;
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

            try
            {
                using (Process process = new())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = "defrag";
                    process.StartInfo.Arguments = $"{unidade}: /o /v";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

                    process.Start();
                    resultadoOtimizacaoDesfragmentacao = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    resultadoOtimizacaoDesfragmentacao = EditarTextoComSimbolosAlterados(resultadoOtimizacaoDesfragmentacao);
                    return resultadoOtimizacaoDesfragmentacao;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "Erro ao executar Otimização/Desfragmentação de disco!";
            }
        }

        public string ExecutarSomenteOtimizacao()
        {
            string unidade = BuscarUnidadeQueContemSistemaOperacional();
            string resultadoOtimizacao = string.Empty;
            try
            {
                using (Process process = new())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = "defrag";
                    process.StartInfo.Arguments = $"{unidade}: /l /v";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                    process.Start();
                    resultadoOtimizacao = process.StandardOutput.ReadToEnd();

                    process.WaitForExit();

                    resultadoOtimizacao = EditarTextoComSimbolosAlterados(resultadoOtimizacao);

                    return resultadoOtimizacao;
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return "erro ao executar otimização de Disco!";
            }
        }

        public string ExecutarLimpezaDeDiscoPadrão()
        {
            try
            {
                string unidade = BuscarUnidadeQueContemSistemaOperacional();
                using (Process? processoCleanmgr = Process.Start(new ProcessStartInfo("cleanmgr", $"/d {unidade} /c /sagerun: /verylowdisk")))
                {
                    processoCleanmgr?.WaitForExit();
                    processoCleanmgr?.Dispose();
                }
                return $"Limpeza de disco executada com sucesso!";

            }
            catch (Exception ex)
            {
                return $"{ex}: Erro ao tentar executar a Limpeza de Disco";
            }
        }

        public string EditarTextoComSimbolosAlterados(string texto)
        {
            Dictionary<string, string> substituicoes = new Dictionary<string, string>
                    {
                        { "opera��o", "operação" },
                        { "�xito", "êxito" },
                        { "conclu�da", "concluída" },
                        { "Informa��es", "Informações" },
                        { "Espa�o", "Espaço" },
                        { "Aloca��es", "Alocações" },
                        { "compat�veis", "Compatíveis" }
                    };

            foreach (var substituicao in substituicoes)
            {
                texto = texto.Replace(substituicao.Key, substituicao.Value);
            }

            return texto;
        }
    }
}
