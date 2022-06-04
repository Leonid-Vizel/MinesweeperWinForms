using System.Windows.Forms;

namespace MinesweeperWinForms
{
    public partial class MainForm : Form
    {
        private Game game;
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnBeginClick(object sender, System.EventArgs e)
        {
            if (game != null)
            {
                game.Dispose();
            }
            game = new Game(panel1, (int)widthNumeric.Value, (int)heightNumeric.Value, (int)bombNumeric.Value, (int)cellNumeric.Value);
        }

        private void OnGameSizeChanged(object sender, System.EventArgs e)
        {
            bombNumeric.Maximum = widthNumeric.Value * heightNumeric.Value - 3;
        }
    }
}
