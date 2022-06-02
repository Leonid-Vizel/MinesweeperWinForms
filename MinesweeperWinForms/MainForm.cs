using System.Windows.Forms;

namespace MinesweeperWinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Game game = new Game(this,10,10,20);
        }
    }
}
