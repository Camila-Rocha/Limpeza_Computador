namespace ProjetoLimpezaDePCRefatoracao.Domain;

public class Opcao
{
   
    public Panel PanelCheckBox { get; set; }
    public Panel PanelIcone { get; set; }
    public CheckBox CheckBox { get; set; }
    public IconeInformacao IconeInformacao { get; set; }
    public List<RadioButton>? RadioButtons { get; set; } = new();
}
