using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinesweeperWinForms
{
    public class GameCell : IDisposable
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

        public void Open()
        {
            if (IsMine)
            {
                Label.Text = "💣";
                return;
            }
            Label.BackColor = Color.Gray;
            if (MinesAround > 0)
            {
                Label.Text = MinesAround.ToString();
            }
        }

        public void Flag(bool openAnyways = false)
        {
            if (openAnyways)
            {
                Label.Text = "🚩";
                return;
            }
            if (Label.Text.Equals("🚩"))
            {
                Label.Text = "";
            }
            else
            {
                Label.Text = "🚩";
            }
        }

        public void Dispose()
        {
            Label.Parent.Controls.Remove(Label);
            Label.Dispose();
        }
    }
}
