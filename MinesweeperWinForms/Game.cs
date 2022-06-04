using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinesweeperWinForms
{
    public class Game : IDisposable
    {
        private int side;
        private bool gameEnded;
        public GameCell[,] Cells { get; private set; }

        public Game(Control parent, int width, int height, int mines, int side)
        {
            this.side = side;
            Cells = new GameCell[width, height];
            bool[,] mineMap = LocateMines(width, height, mines);
            byte[,] numberMap = CalculateMineMap(mineMap);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Cells[x, y] = new GameCell(mineMap[x, y], numberMap[x, y]);
                    ApplyLabelSetting(parent, x, y, Cells[x, y]);
                }
            }
        }

        public void Dispose()
        {
            foreach(GameCell cell in Cells)
            {
                cell.Dispose();
            }
        }

        private bool[,] LocateMines(int width, int height, int amount)
        {
            bool[,] map = new bool[width, height];
            Random random = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < amount; i++)
            {
                int x = random.Next(width);
                int y = random.Next(height);
                while (map[x, y])
                {
                    x = random.Next(width);
                    y = random.Next(height);
                }
                map[x, y] = true;
            }
            return map;
        }

        private byte[,] CalculateMineMap(bool[,] mines)
        {
            byte[,] numberMap = new byte[mines.GetLength(0), mines.GetLength(1)];
            for (int y = 0; y < mines.GetLength(1); y++)
            {
                for (int x = 0; x < mines.GetLength(0); x++)
                {
                    if (mines[x, y])
                    {
                        continue;
                    }
                    bool xMinus = x > 0;
                    bool yMinus = y > 0;
                    bool xPlus = x < mines.GetLength(0) - 1;
                    bool yPlus = y < mines.GetLength(1) - 1;
                    byte minesAround = 0;
                    if (xMinus && yMinus && mines[x - 1, y - 1])
                    {
                        minesAround++;
                    }
                    if (xPlus && yMinus && mines[x + 1, y - 1])
                    {
                        minesAround++;
                    }
                    if (xMinus && yPlus && mines[x - 1, y + 1])
                    {
                        minesAround++;
                    }
                    if (xPlus && yPlus && mines[x + 1, y + 1])
                    {
                        minesAround++;
                    }
                    if (xMinus && mines[x - 1, y])
                    {
                        minesAround++;
                    }
                    if (xPlus && mines[x + 1, y])
                    {
                        minesAround++;
                    }
                    if (yMinus && mines[x, y - 1])
                    {
                        minesAround++;
                    }
                    if (yPlus && mines[x, y + 1])
                    {
                        minesAround++;
                    }
                    numberMap[x, y] = minesAround;
                }
            }
            return numberMap;
        }

        private void OpenAllMines()
        {
            foreach (GameCell cell in Cells)
            {
                if (cell.IsMine)
                {
                    cell.Open();
                }
            }
        }

        private void FlagAllMines()
        {
            foreach (GameCell cell in Cells)
            {
                if (cell.IsMine)
                {
                    cell.Flag(true);
                }
            }
        }

        private bool CheckAllOpened()
        {
            foreach (GameCell cell in Cells)
            {
                if (!cell.IsMine && cell.Label.BackColor == Color.White)
                {
                    return false;
                }
            }
            return true;
        }

        private void OpenCellRecursive(int x, int y, GameCell cell, bool inital = false)
        {
            cell.Open();
            if (cell.MinesAround > 0)
            {
                if (!inital)
                {
                    return;
                }
            }
            bool xMinus = x > 0;
            bool yMinus = y > 0;
            bool xPlus = x < Cells.GetLength(0) - 1;
            bool yPlus = y < Cells.GetLength(1) - 1;
            if (xMinus && yMinus && !Cells[x - 1, y - 1].IsMine && Cells[x - 1, y - 1].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x - 1, y - 1, Cells[x - 1, y - 1]);
            }
            if (xPlus && yMinus && !Cells[x + 1, y - 1].IsMine && Cells[x + 1, y - 1].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x + 1, y - 1, Cells[x + 1, y - 1]);
            }
            if (xMinus && yPlus && !Cells[x - 1, y + 1].IsMine && Cells[x - 1, y + 1].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x - 1, y + 1, Cells[x - 1, y + 1]);
            }
            if (xPlus && yPlus && !Cells[x + 1, y + 1].IsMine && Cells[x + 1, y + 1].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x + 1, y + 1, Cells[x + 1, y + 1]);
            }
            if (xMinus && !Cells[x - 1, y].IsMine && Cells[x - 1, y].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x - 1, y, Cells[x - 1, y]);
            }
            if (xPlus && !Cells[x + 1, y].IsMine && Cells[x + 1, y].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x + 1, y, Cells[x + 1, y]);
            }
            if (yMinus && !Cells[x, y - 1].IsMine && Cells[x, y - 1].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x, y - 1, Cells[x, y - 1]);
            }
            if (yPlus && !Cells[x, y + 1].IsMine && Cells[x, y + 1].Label.BackColor == Color.White)
            {
                OpenCellRecursive(x, y + 1, Cells[x, y + 1]);
            }
        }

        private void ApplyLabelSetting(Control parent, int x, int y, GameCell cell)
        {
            cell.Label.BackColor = Color.White;
            cell.Label.TextAlign = ContentAlignment.MiddleCenter;
            cell.Label.Location = new Point(x * side, y * side);
            cell.Label.Size = new Size(side, side);
            cell.Label.BorderStyle = BorderStyle.FixedSingle;
            cell.Label.MouseClick += (s, e) =>
            {
                if (gameEnded || cell.Label.BackColor == Color.Gray || cell.Label.Text.Equals("🚩"))
                {
                    return;
                }
                if (e.Button == MouseButtons.Left)
                {
                    if (cell.IsMine)
                    {
                        OpenAllMines();
                        MessageBox.Show("Игра проиграна!");
                        gameEnded = true;
                        return;
                    }
                    OpenCellRecursive(x, y, cell, true);
                    if (CheckAllOpened())
                    {
                        FlagAllMines();
                        gameEnded = true;
                        MessageBox.Show("Победа!");
                    }
                    return;
                }
                cell.Flag();
            };
            parent.Controls.Add(cell.Label);

        }
    }
}
