using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace MinesweeperWinForms
{
    //Class only for saving and reading settings
    public class GameSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mines { get; set; }
        public int WidthCell { get; set; }

        public static GameSettings Standard => new GameSettings()
        {
            Width = 10,
            Height = 10,
            Mines = 10,
            WidthCell = 10
        };

        public void Save()
        {
            try
            {
                File.WriteAllText("settings.json", JsonConvert.SerializeObject(this));
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Ошибка записи:\n{exception}");
            }
        }
    }
}
