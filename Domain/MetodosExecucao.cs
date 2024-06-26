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
                    process.StartInfo.Arguments = $"{unidade}: /o";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

                    process.Start();
                    resultadoOtimizacaoDesfragmentacao = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

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
            string resultadoOtimizacao;
            try
            {
                using (Process process = new())
                {
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.FileName = "defrag";
                    process.StartInfo.Arguments = $"{unidade}: /l";
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;

                    process.Start();
                    resultadoOtimizacao = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

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
            int chave = 1000;
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

            if (VerificarSeArquivoChaveEstaVazio() == true)
            {
                do
                {
                    if (VerificarSeChaveExisteNoRegistroDoWindows(chave.ToString()) == false)
                    {
                        GravarChaveEmArquivo(chave.ToString());

                        return ExecutarLimpezaDeDiscoBase(unidade, chave.ToString());                                              
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
                            return ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo);
                        }
                        else
                        {
                            GravarChaveEmArquivo(chaveDoArquivo);
                            return ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo);                           
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
                Process processoCleanmgr = Process.Start("cleanmgr", $"/d {unidade} /c /sagerun: /verylowdisk");

                //processoCleanmgr.WaitForExit();

                return "Limpeza de disco executada com sucesso!";
            }
            catch (Exception ex)
            {
                return $"{ex}: Erro ao tentar executar a Limpeza de Disco";
            }
        }

        public string ExecutarLimpezaDeDiscoComChaveExistente()
        {
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

            if (VerificarSeArquivoChaveEstaVazio() == false)
            {
                string[] linhas = File.ReadAllLines(CaminhoArquivoChave());
                string chaveDoArquivo = linhas[0];
                return ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo);
                
            }
            else
            {
                return "Configuração não existe!";
            }
        }

        public string ExecutarLimpezaDeDiscoBase(string unidade, string chave)
        {
            // fazer uma verificação aqui para executar ditero quando houver chave
            try
            {
                Process processoCleanmgr = Process.Start("cleanmgr", $"/sageset:{chave} /d {unidade}"); //abre a janela para selecionar as opções

                if (processoCleanmgr == null)
                {
                    return "Chave não existe";
                }

                processoCleanmgr.WaitForExit();

                //var subprocessos = Subprocessos(processoCleanmgr);

                //foreach (var childProcess in subprocessos)
                //{
                //    childProcess.WaitForExit();
                //}

                var processoSagerun = Process.Start("cleanmgr", $"sagerun:{chave}");

                processoSagerun.WaitForExit();
              
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey($@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\DiskCleanup\{unidade}\{chave}"))
                {
                    key.SetValue("Settings", $"/sagerun:{chave}");
                }

                return "Limpeza de disco executada com sucesso!";
            }
            catch (Exception ex)
            {
                return $"{ex}: Erro ao tentar executar a Limpeza de Disco";
            }
        }

        public int PegaIdProcessoPai(int id)
        {
            int IdPai = 0;
            try
            {
                using (var mo = new System.Management.ManagementObject($"win32_process.handle='{id}'"))
                {
                    mo.Get();
                    IdPai = Convert.ToInt32(mo["ParentProcessId"]);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter o ID do processo pai: {ex.Message}");
            }

            return IdPai;
        }
        public Process[] Subprocessos(Process processoCleanmgr)
        {
            var subprocessos = from proc in Process.GetProcesses()
                               where PegaIdProcessoPai(proc.Id) == processoCleanmgr.Id
                               select proc;

            return subprocessos.ToArray();
        }

    }
}
