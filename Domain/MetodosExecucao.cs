using Microsoft.Win32;
using System.Diagnostics;

namespace ProjetoLimpezaDePCRefatoracao.Domain
{
    internal class MetodosExecucao : MetodosAuxiliares
    {  
        public void ExecutarLimpezaArquivosTemporarios()
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        public void ExecutarDesfragmentacaoOuOtimizacaoDeAcordoComMidia()
        {
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

                    process.WaitForExit();

                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ExecutarSomenteOtimizacao()
        {
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

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

                    process.WaitForExit();
                }
            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void ExecutarLimpezaComConfigManual()
        {
            int chave = 1000;
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

            if (VerificarSeArquivoChaveEstaVazio() == true)
            {
                do
                {
                    if (VerificarSeChaveExisteNoRegistroDoWindows(chave.ToString()) == false)
                    {
                        ExecutarLimpezaDeDiscoBase(unidade, chave.ToString());
                        GravarChaveEmArquivo(chave.ToString());

                        break;
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
                            ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo);
                            GravarChaveEmArquivo(chaveDoArquivo);

                            break;
                        }
                        else
                        {
                            ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo);
                            GravarChaveEmArquivo(chaveDoArquivo);

                            break; //adicionei 
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex}: Erro ao tentar ler as configurações existentes");
                    }
                }
            }
        }

        public void ExecutarLimpezaDeDiscoPadrão()
        {
            string unidade = BuscarUnidadeQueContemSistemaOperacional();
            Process.Start("cleanmgr", $"/d {unidade} /c /sagerun: /verylowdisk");
        }

        public bool ExecutarLimpezaDeDiscoComChaveExistente()
        {
            string unidade = BuscarUnidadeQueContemSistemaOperacional();

            if (VerificarSeArquivoChaveEstaVazio() == false)
            {
                string[] linhas = File.ReadAllLines(CaminhoArquivoChave());
                string chaveDoArquivo = linhas[0];
                ExecutarLimpezaDeDiscoBase(unidade, chaveDoArquivo);

                return true;
            }

            else
            {
                return false;
            }
        }

        public void ExecutarLimpezaDeDiscoBase(string unidade, string chave)
        {
            Process processoCleanmgr = Process.Start("cleanmgr", $"/sageset:{chave} /d {unidade}");

            if (processoCleanmgr == null)
            {
                return;
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
