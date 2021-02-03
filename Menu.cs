using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleCity
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Form crt = new GameForm();
            crt.Show();
            this.Hide();
        }

        private void infoButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Основная информация по игре: перемещение выполняется клавишами WASD, стрельба - SPACE. Ваша цель - уничтожить 3 единицы техники противника или же достичь точки, отмеченной на карте. Вражеский танк уничтожается от одного выстрела, у Вас же есть пять попыток.","Info");
        }
    }
}
