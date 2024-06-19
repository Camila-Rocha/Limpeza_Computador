namespace ProjetoLimpezaDePCRefatoracao.Domain
{
    public class SubOpcoesRadiobuttons
    {
        public static RadioButton RadiobuttonLimpezaPadrao { get; } = new() 
        {
            Text = "Limpeza Padrão",
            Font = new Font("Arial", 10),
            AutoSize = true,
            Location = new Point(0, 30 * 0 + 5),
            Checked = true
        };

        public static RadioButton RadiobuttonLimpezaPersonalizada { get; } = new()
        {
            Text = "Limpeza Personalizada",
            Font = new Font("Arial", 10),
            AutoSize = true,
            Location = new Point(0, 30 * 1 + 5),     
        };
        public static RadioButton RadiobuttonLimpezaChaveExistente { get; } = new()
        {
            Text = "Usar Última Limpeza Personalizada Realizada (Se houver)",
            Font = new Font("Arial", 10),
            AutoSize = true,
            Location = new Point(0, 30 * 2 + 5),
        };

        public  List<RadioButton>? RadioButtons { get; set; } = new()
        {
            RadiobuttonLimpezaPadrao,
            RadiobuttonLimpezaPersonalizada,
            RadiobuttonLimpezaChaveExistente            
        };

    }
}
