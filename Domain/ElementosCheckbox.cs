using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProjetoLimpezaDePCRefatoracao.Domain
{
    internal class ElementosCheckbox
    {
        public string TextoCheckBox { get; set; } = string.Empty;
        public string TextoInformacao { get; set; } = string.Empty;
        public PictureBox ImagemInformacao { get; } = new PictureBox()
        {
            Image = Properties.Resources.ico_info,
            Size = new Size(20, 20),
            Location = new Point(0, 0),
            SizeMode = PictureBoxSizeMode.Zoom,

        };
        public System.Windows.Forms.ToolTip ToolTipImagemInformacao { get; set; } = new System.Windows.Forms.ToolTip()
        {
            AutoPopDelay = 5000,// Duração da exibição
            InitialDelay = 500, // Atraso inicial antes de exibir a dica
            ReshowDelay = 500, // Atraso antes de exibir novamente após sumir
            ShowAlways = true,
        };
        
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
