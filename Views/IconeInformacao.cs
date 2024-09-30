namespace Limpeza_Computador.Views;

public class IconeInformacao
{
    public string Texto { get; set; } = string.Empty;
    public PictureBox Icone { get; } = new PictureBox
    {
        Image = Properties.Resources.ico_info,
        Size = new Size(20, 20),
        Location = new Point(0, 0),
        SizeMode = PictureBoxSizeMode.Zoom
    };
    public ToolTip EventoExibeMensagemFlutuante { get; set; } = new ToolTip() { AutoPopDelay = 5000, InitialDelay = 500, ReshowDelay = 500, ShowAlways = true };
}
