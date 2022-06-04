using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System;

namespace MinesweeperWinForms
{
    public partial class MainForm : Form
    {
        private Game game;
        private GameSettings settings;
        public MainForm()
        {
            InitializeComponent();
            settings = null;
            if (File.Exists("settings.json"))
            {
                try
                {
                    settings = JsonConvert.DeserializeObject<GameSettings>(File.ReadAllText("settings.json"));
                }
                catch (Exception exception)
                {
                    settings = GameSettings.Standard;
                    MessageBox.Show($"Ошибка чтения:\n{exception}");
                }
            }
            else
            {
                settings = GameSettings.Standard;
                string standardJson = JsonConvert.SerializeObject(settings);
                try
                {
                    File.WriteAllText("settings.json", standardJson);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Ошибка записи:\n{exception}");
                }
            }

            widthNumeric.Value = settings.Width;
            heightNumeric.Value = settings.Height;
            bombNumeric.Value = settings.Mines;
            widthNumeric.Value = settings.WidthCell;
        }

        private void OnBeginClick(object sender, EventArgs e)
        {
            if (game != null)
            {
                game.Dispose();
            }
            game = new Game(panel1, (int)widthNumeric.Value, (int)heightNumeric.Value, (int)bombNumeric.Value, (int)cellNumeric.Value);
            settings.Height = (int)widthNumeric.Value;
            settings.Height = (int)heightNumeric.Value;
            settings.Mines = (int)bombNumeric.Value;
            settings.WidthCell = (int)widthNumeric.Value;
            settings.Save();
        }

        private void OnGameSizeChanged(object sender, EventArgs e)
        {
            bombNumeric.Maximum = widthNumeric.Value * heightNumeric.Value - 3;
        }
    }
}
