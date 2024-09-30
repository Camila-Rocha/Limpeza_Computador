using Microsoft.Win32;

namespace Limpeza_Computador.Service;

public class SistemaOperacionalHelperService
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
}
