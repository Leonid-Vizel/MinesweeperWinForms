using System.Windows.Forms;

namespace MinesweeperWinForms
{
    public class GameCell
    {
        public bool IsMine { get; private set; }
        public byte MinesAround { get; private set; }
        public Label Label { get; private set; }

        public GameCell(bool isMine, byte minesAround)
        {
            IsMine = isMine;
            MinesAround = minesAround;
            Label = new Label();
        }
    }
}
