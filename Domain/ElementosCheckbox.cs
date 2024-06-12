using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoLimpezaDePCRefatoracao.Domain
{
    internal class ElementosCheckbox
    {
        public string TextoCheckBox { get; set; } = string.Empty;
        public string TextoInformacao { get; set; } = string.Empty;
        public LinkLabel LinkInformacao { get; } = new LinkLabel()
        {
            Text = "Saiba mais"
        };

        //ToolTip toolTip1 = new ToolTip();
        //toolTip1.AutoPopDelay = 5000; // Duração da exibição
        //toolTip1.InitialDelay = 1000; // Atraso inicial antes de exibir a dica
        //toolTip1.ReshowDelay = 500; // Atraso antes de exibir novamente após sumir
        //toolTip1.ShowAlways = true;

        //toolTip1.SetToolTip(LinkInformacao, TextoInformacao);

        public List<ElementosCheckbox> CriarLinkLabels(string[] nomesCheckbox, string[] textosInfo)
        {
            var list = new List<ElementosCheckbox>();

            for (int i = 0; i < nomesCheckbox.Count(); i++)
            {
                list.Add(new ElementosCheckbox
                {
                    TextoCheckBox = nomesCheckbox[i],
                    TextoInformacao = textosInfo[i]
                });
            }

            return list;
        }
    }
}
