using Newtonsoft.Json.Linq;
using Limpeza_Computador.Service;

namespace Limpeza_Computador.Domain
{
    public class ConteudoOpcaoCheckbox
    {
        private static JObject JsonObj { get; set; } = ConfigJsonService.CarregarConfiguracoes();      
        
        public Dictionary<string, string> RetornarNomesEDescricoesOpcaosCheckbox()
        {
            Dictionary<string, string> DescricaoOpcao = new Dictionary<string, string>()
                {
                    { JsonObj["checkBox1"]!.ToString(), "Serão excluídos Arquivos temporários:\r\n- Pasta Temp (usuário)\r\n- Pasta Download e pasta Temp (Windows)\r\n- Pasta Recent." },
                    { JsonObj["checkBox2"]!.ToString(), "Executa a Limpeza PADRÂO de Disco do Windows" },
                    { JsonObj["checkBox3"]!.ToString(), "Executa a Desfragmentação e Otimização em caso de HDD e somente Otimização\nem caso de SSD - Seleção inteligente do Windows." },
                    { JsonObj["checkBox4"]!.ToString(), "Executa somente Otimização de Disco." }
                };
            return DescricaoOpcao;
        }
    }
}
