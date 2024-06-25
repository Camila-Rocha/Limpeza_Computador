using Microsoft.Win32;

namespace ProjetoLimpezaDePCRefatoracao.Domain;

public class MetodosAuxiliares
{
    public string BuscarUnidadeQueContemSistemaOperacional()
    {
        DriveInfo[] drives = DriveInfo.GetDrives();

        foreach (DriveInfo drive in drives)
        {
            if (drive.DriveType == DriveType.Fixed && Directory.Exists(Path.Combine(drive.RootDirectory.FullName, "Windows")))
            {
                char[] letraDaUnidade = drive.Name.ToCharArray();
                foreach (char c in letraDaUnidade)
                {
                    if (char.IsLetter(c))
                    {
                        return $"{c}";
                    }
                }
                break;
            }
            else
            {
                return "unidade não encontrada";
            }
        }
        return "erro ao buscar os  drives";
    }

    public string CaminhoPastaCache()
    {
        string diretorioAtual = Directory.GetCurrentDirectory();
        return Path.Combine(diretorioAtual, "Cache");
    }

    public string CaminhoArquivoChave()
    {
        string diretorioAtual = Directory.GetCurrentDirectory();
        return Path.Combine(diretorioAtual, "Cache", "Chave.txt");
    }

    public void CriaPastaEArquivo()
    {
        string pastaCache = CaminhoPastaCache();
        string arquivoChave = CaminhoArquivoChave();

        if (!Directory.Exists(pastaCache))
        {
            Directory.CreateDirectory(pastaCache);
            DirectoryInfo directoryInfo = new DirectoryInfo(pastaCache);
            directoryInfo.Attributes |= FileAttributes.Hidden;
        }

        if (!File.Exists(arquivoChave))
        {
            using (StreamWriter writer = File.CreateText(arquivoChave)) { }
        }
    }

    public void ApagarPastaEArquivo()
    {
        string pastaCache = CaminhoPastaCache();
        string arquivoChave = CaminhoArquivoChave();
        string[] linhas = File.ReadAllLines(CaminhoArquivoChave());
        string chaveDoArquivo = string.Empty;
        string unidade = BuscarUnidadeQueContemSistemaOperacional();
        if (!VerificarSeArquivoChaveEstaVazio())
        {
            chaveDoArquivo = linhas[0];

            if (File.Exists(arquivoChave))
            {
                if (VerificarSeChaveExisteNoRegistroDoWindows(chaveDoArquivo) == true)
                {
                    Registry.CurrentUser.DeleteSubKeyTree($@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\DiskCleanup\{unidade}\{chaveDoArquivo}");
                }
            }

            if (Directory.Exists(pastaCache))
            {
                Directory.Delete(pastaCache, true);
            }
        }

        else
        {
            MessageBox.Show("Nenhum dado para ser formatado");
        }
    }

    public void GravarChaveEmArquivo(string chave)
    {
        using (StreamWriter streamWriter = new StreamWriter(CaminhoArquivoChave(), false))
        {
            streamWriter.WriteLine(chave);
        }
    }

    public bool VerificarSeArquivoChaveEstaVazio()
    {
        if (new FileInfo(CaminhoArquivoChave()).Length == 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool VerificarSeChaveExisteNoRegistroDoWindows(string chave)
    {
        string unidade = BuscarUnidadeQueContemSistemaOperacional();

        using (RegistryKey key = Registry.CurrentUser.OpenSubKey($@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\DiskCleanup\{unidade}"))
        {
            if (key != null)
            {
                foreach (string subKeyName in key.GetSubKeyNames())
                {
                    if (subKeyName == chave)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void ApagaPasta_ChaveRegistro()
    {
        string pastaCache = CaminhoPastaCache();
        string arquivoChave = CaminhoArquivoChave();
        if(File.Exists(arquivoChave)) 
        {
            string[] linhas = File.ReadAllLines(CaminhoArquivoChave());
            string chaveDoArquivo;
            string unidade = BuscarUnidadeQueContemSistemaOperacional();
            if (!VerificarSeArquivoChaveEstaVazio())
            {
                chaveDoArquivo = linhas[0];

                if (File.Exists(arquivoChave))
                {
                    if (VerificarSeChaveExisteNoRegistroDoWindows(chaveDoArquivo) == true)
                    {
                        Registry.CurrentUser.DeleteSubKeyTree($@"Software\Microsoft\Windows\CurrentVersion\Explorer\RunMRU\DiskCleanup\{unidade}\{chaveDoArquivo}");
                    }
                }              
            }
            if (Directory.Exists(pastaCache))
            {
                Directory.Delete(pastaCache, true);
            }
            if(File.Exists(arquivoChave) || Directory.Exists(pastaCache))
            {
                MessageBox.Show("Configurações formatadas com sucesso!");
            }
            else
            {
                MessageBox.Show("Nenhum dado para ser formatado");
            }
        }
        else
        {
            MessageBox.Show("Nenhum dado para ser formatado");
        }       
    }
}
