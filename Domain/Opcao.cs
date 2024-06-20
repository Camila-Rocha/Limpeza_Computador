namespace ProjetoLimpezaDePCRefatoracao.Domain;

public class Opcao
{
   
    public Panel PanelCheckBox { get; set; }
    public Panel PanelIcone { get; set; }
    public CheckBox CheckBox { get; set; }
    public IconeInformacao IconeInformacao { get; set; }
    public List<RadioButton>? RadioButtons { get; set; } = new();
    public static List<Color> CoresCadaOpcao { get; set; } = new()
    {       
        ColorTranslator.FromHtml("#FF00FF"),//magenta
        ColorTranslator.FromHtml("#8B4513"), // SaddleBrown
        ColorTranslator.FromHtml("#4B0082"), //amarelo 
        ColorTranslator.FromHtml("#FFD700")       
    };
}
