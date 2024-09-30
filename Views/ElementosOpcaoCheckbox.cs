using Limpeza_Computador.Views;

namespace Limpeza_Computador.Domain;

public class ElementosDeOpcaoCheckbox
{
   
    public Panel? PanelCheckBox { get; set; }
    public Panel? PanelIcone { get; set; }
    public CheckBox? CheckBox { get; set; }
    public IconeInformacao? IconeInformacao { get; set; }
    public List<RadioButton>? RadioButtons { get; set; } = new();
 
}
