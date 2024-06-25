using Microsoft.Win32;
using System.Diagnostics;

namespace ProjetoLimpezaDePCRefatoracao.Domain
{
    internal class MetodosExecucao : MetodosAuxiliares
    {  
        public string ExecutaLimpezaLogWindows()
        {
            string logName = "Application"; 
            DateTime thresholdDate = DateTime.Now.AddDays(-30);         

            try
            {
                // Verifica se o log especificado existe
                if (EventLog.Exists(logName))
                {
                    // Cria um objeto EventLog associado ao log especificado
                    using (EventLog eventLog = new EventLog(logName))
                    {
                        // Itera através das entradas do log
                        foreach (EventLogEntry entry in eventLog.Entries)
                        {
                            // Verifica se a data da entrada é anterior à data limite                          
                                // Exibe ou processa a entrada conforme necessário
                                return $"Data: {entry.TimeGenerated}, Tipo: {entry.EntryType}, Fonte: {entry.Source}, Mensagem: {entry.Message}";
                            
                        }
                    }
                    return "nada";
                }

                else
                {
                    return $"O log {logName} não existe.";
                }
            }
            catch (Exception ex)
            {
                return $"Erro ao tentar acessar o log {logName}: {ex.Message}";
            }
        }
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
                Process.Start("cleanmgr", $"/d {unidade} /c /sagerun: /verylowdisk");

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
            Process processoCleanmgr = Process.Start("cleanmgr", $"/sageset:{chave} /d {unidade}");
            try
            {
                if (processoCleanmgr == null)
                {
                    return "Chave não existe";
                }

                processoCleanmgr.WaitForExit();

                var subprocessos = from proc in Process.GetProcesses()
                                   where PegaIdProcessoPai(proc.Id) == processoCleanmgr.Id
                                   select proc;

                subprocessos = subprocessos.ToArray();

                foreach (var childProcess in subprocessos)
                {
                    childProcess.WaitForExit();
                }

                Process.Start("cleanmgr", $"sagerun:{chave}");

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

    }
}
