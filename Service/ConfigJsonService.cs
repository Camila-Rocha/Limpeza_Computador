using Newtonsoft.Json.Linq;

namespace Limpeza_Computador.Service
{
    internal class ConfigJsonService
    {
        private static string CaminhoArquivoJson { get; set; } = @"C:\Users\stude\source\repos\Limpeza_Computador\ProjetoLimpezaDePCRefatoracao\Configs\appConfig.json";

        public static JObject CarregarConfiguracoes()
        {
            string textoJson = File.ReadAllText(CaminhoArquivoJson);
            return JObject.Parse(textoJson);
        }
    }
}
