using Microsoft.Win32;
using System.Diagnostics;

namespace ProjetoLimpezaDePCRefatoracao.Domain
{
    internal class MetodosExecucao : MetodosAuxiliares
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

        public string ExecutarLimpezaComConfigManual()
        {
            bool chaveExistente = false;
            int chave = 1000;
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

            if (VerificarSeArquivoChaveEstaVazio() == true)
            {
                do
                {
                    if (VerificarSeChaveExisteNoRegistroDoWindows(chave.ToString()) == false)
                    {
                        GravarChaveEmArquivo(chave.ToString());

                        return ExecutarLimpezaDeDiscoBase(unidade, chave.ToString(), chaveExistente);
                    }
                    else
                    {
                        chave++;
                    }
                }

                while (true);
            }
            else
            {
                for (; ; )
                {
                    try
                    {
                        string[] linhas = File.ReadAllLines(CaminhoArquivoChave());
                        string chaveDoArquivo = linhas[0];

                        if (VerificarSeChaveExisteNoRegistroDoWindows(chaveDoArquivo) == true)
                        {
                            Registry.CurrentUser.DeleteSubKeyTree($@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\DiskCleanup\{unidade}\{chaveDoArquivo}");
                            GravarChaveEmArquivo(chaveDoArquivo);
                            return ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo, chaveExistente);
                        }
                        else
                        {
                            GravarChaveEmArquivo(chaveDoArquivo);
                            return ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo, chaveExistente);
                        }

                    }
                    catch (Exception ex)
                    {
                        return $"{ex}: Erro ao tentar executar a Limpeza de Disco";
                    }
                }
            }
        }

        public string ExecutarLimpezaDeDiscoPadrão()
        {
            try
            {
                string unidade = BuscarUnidadeQueContemSistemaOperacional();
                using (Process processoCleanmgr = Process.Start(new ProcessStartInfo("cleanmgr", $"/d {unidade} /c /sagerun: /verylowdisk")))
                {
                    processoCleanmgr.WaitForExit();
                    processoCleanmgr.Dispose();
                }
                return $"Limpeza de disco executada com sucesso!";

            }
            catch (Exception ex)
            {
                return $"{ex}: Erro ao tentar executar a Limpeza de Disco";
            }
        }

        public string ExecutarLimpezaDeDiscoComChaveExistente()
        {
            bool chaveExistente = true;
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

            if (VerificarSeArquivoChaveEstaVazio() == false)
            {
                string[] linhas = File.ReadAllLines(CaminhoArquivoChave());
                string chaveDoArquivo = linhas[0];
                return ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo, chaveExistente);
            }
            else
            {
                return "Configuração não existe!";
            }
        }

        public string ExecutarLimpezaDeDiscoBase(string unidade, string chave, bool chaveExistente)
        {
            if (chaveExistente == false)
            {
                try
                {
                    using (Process processoCleanmgr = Process.Start(new ProcessStartInfo("cleanmgr", $"/sageset: {chave} /d {unidade}")))
                    {
                        processoCleanmgr.WaitForExit();

                    }

                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\DiskCleanup\{unidade}\{chave}"))
                    {
                        key.SetValue("Settings", $"/sagerun:{chave}");
                    }

                    using (Process processoSagerun = Process.Start(new ProcessStartInfo("cleanmgr", $"sagerun:{chave}")))
                    {
                        processoSagerun.WaitForExit();
                    }

                    return "Limpeza de disco Personalizada executada com sucesso!";
                }
                catch (Exception ex)
                {
                    return $"{ex}: Erro ao tentar executar a Limpeza de Disco";
                }
            }
            else
            {
                try
                {
                    using (Process processoSagerun = Process.Start(new ProcessStartInfo("cleanmgr", $"sagerun:{chave}") { UseShellExecute = true }))
                    {
                        processoSagerun.WaitForExit();

                    }

                    using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\DiskCleanup\{unidade}\{chave}"))
                    {
                        key.SetValue("Settings", $"/sagerun:{chave}");
                    }

                    return "Limpeza de disco com última configuração executada com sucesso!";
                }
                catch (Exception ex)
                {
                    return $"{ex}: Erro ao tentar executar a Limpeza de Disco";
                }

            }

        }

        public string EditarTextoComSimbolosAlterados(String texto)
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
