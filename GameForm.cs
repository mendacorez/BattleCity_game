using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.IO;
using System.Runtime.CompilerServices;
using WMPLib;

namespace BattleCity
{
    public partial class GameForm : Form
    {
        static Map DrawMap = new Map();
        WindowsMediaPlayer MainMusic = new WindowsMediaPlayer();

        int lastX = 9; // позиция игрока Х
        int lastY = 9; // позиция игрока Y

        // изображения
        static Image PlayerUp;
        static Image PlayerLeft;
        static Image PlayerDown;
        static Image PlayerRight;
        static Image BotUp;
        static Image BotLeft;
        static Image BotDown;
        static Image BotRight;
        static Image Block;
        static Image Water;
        static Image BulletUp;
        static Image BulletRight;
        static Image BulletLeft;
        static Image BulletDown;
        static Image Finish;
        static Image BotBullet;

        static int rotate = 0;
        static int botDirection = 0; // направление бота. 0 - W, 1 - D, 2 - S - 3 - A
        static int bulletDirection = 0;  // направление снаряда игрока. 0 - W, 1 - D, 2 - S - 3 - A
        static int botBulletDirection = 0;   // направление снаряда бота. 0 - W, 1 - D, 2 - S - 3 - A
        static int botX = 4; // позиция бота Х
        static int botY = 0; // позиция бота Y
        static bool isBulletFlying = false; // отвечает за полёт снаряда
        static bool isKeyAlreadyPressed = false; // проверка на нажатие клавиши
        static bool isCheckPhoetic = false; // проверка на коллизию
        static bool IsBotBulletFlying = false; // отвечает за полёт снаряда бота
        int hpCout = 5; // HP
        int enemiesCount = 3; // количество противников

        public GameForm()
        {
            InitializeComponent();

            // иницализация изображений
            Block = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\Block.png");

            PlayerUp = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\PlayerUp.png");
            PlayerLeft = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\PlayerLeft.png");
            PlayerDown = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\PlayerDown.png");
            PlayerRight = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\PlayerRight.png");

            Water = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\Water.png");

            BulletUp = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BulletUp.png");
            BulletLeft = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BulletLeft.png");
            BulletDown = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BulletDown.png");
            BulletRight = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BulletRight.png");

            BotUp = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BotUp.png");
            BotLeft = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BotLeft.png");
            BotDown = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BotDown.png");
            BotRight = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BotRight.png");

            Finish = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\Finish.png");
            BotBullet = new Bitmap(Directory.GetCurrentDirectory() + @"\Source\Images\BotBullet.png");

            MainMusic.URL = Directory.GetCurrentDirectory() + @"\Source\Music\main_music.mp3"; // указываем путь для музыки
            MainMusic.settings.volume = 50; // задаём громкость
            MainMusic.controls.play(); // включаем

            SpawnBot(); // функция, отвечающая за появление бота

            DrawMap.GlobalMap[lastX, lastY] = 3; // рисуем игрока

            this.KeyDown += new KeyEventHandler(CheckKey); // проверяем нажатие клавиши

            globalTimer.Interval = 250; // таймер, отвечающий за обновление глобальных событий
            globalTimer.Tick += new EventHandler(GlobalUpdate); 
            globalTimer.Start();

            botTimer.Interval = 500;  // таймер, отвечающий за перемещение бота
            botTimer.Tick += new EventHandler(BotConroll);
            botTimer.Start();

        }
        private void OnPaint(object sender, PaintEventArgs e) // определяем окно, которое будет перересовано
        {
            Graphics gr = e.Graphics;
            Drawing(gr);
        }

        private void GlobalUpdate(object sender, EventArgs e)
        {
            hpCounter.Text = hpCout.ToString();
            enemiesCounter.Text = enemiesCount.ToString();
            if (isBulletFlying == true)
            {
                BulletContact(); // проверяет, есть ли в следующем блоке объект
                BulletFlying(); // если блока нет, пуля продолжает движение
            }
            if(IsBotBulletFlying == true)
            {
                BotBulletFlying(); // проверяет, есть ли в следующем блоке объект
                BotBulletContact();// если блока нет, пуля продолжает движение
            }
            isKeyAlreadyPressed = false; // разрешаем пользователю нажать клавишу
            this.Invalidate(); // перерисовываем окно для обновления
        }
        private void CheckKey(object sender, KeyEventArgs e)
        {
            if (isKeyAlreadyPressed == false)
            {
                isKeyAlreadyPressed = true;
                DrawMap.GlobalMap[lastX, lastY] = 0; // при нажатии клавиши удаляем персонажа с карты
                switch (e.KeyCode.ToString())
                {
                    case "D":
                        rotate = 1;
                        if (lastX+1 < 10)
                        {
                            isCheckPhoetic = CheckPhoetic(rotate); // проверяем на коллизию с объектами
                            if (isCheckPhoetic != true)
                            {
                                DrawMap.GlobalMap[lastX + 1, lastY] = 3;
                                lastX++;
                            }
                            else
                            {
                                DrawMap.GlobalMap[lastX, lastY] = 3;
                            }
                        }
                        else
                        {
                            DrawMap.GlobalMap[lastX, lastY] = 3;
                        }
                        break;
                    case "W":
                        if ((lastX == 0) &&lastY == 1)
                        {
                            Finished();
                        }
                        rotate = 0;
                        if (lastY-1 > -1)
                        {
                            isCheckPhoetic = CheckPhoetic(rotate);
                            if (isCheckPhoetic != true)
                            {
                                DrawMap.GlobalMap[lastX, lastY - 1] = 3;
                                lastY--;
                            }
                            else
                            {
                                DrawMap.GlobalMap[lastX, lastY] = 3;
                            }
                        }
                        else
                        {
                            DrawMap.GlobalMap[lastX, lastY] = 3;
                        }
                        break;
                    case "A":
                        if ((lastX == 1) && lastY == 0)
                        {
                            Finished();
                        }
                        rotate = 3;
                        if (lastX-1 > -1)
                        {
                            isCheckPhoetic = CheckPhoetic(rotate);
                            if (isCheckPhoetic != true)
                            {
                                DrawMap.GlobalMap[lastX - 1, lastY] = 3;
                                lastX--;
                            }
                            else
                            {
                                DrawMap.GlobalMap[lastX, lastY] = 3;
                            }
                        } else
                        {
                            DrawMap.GlobalMap[lastX, lastY] = 3;
                        }
                        break;
                    case "S":
                        rotate = 2;
                        if (lastY+1 < 10)
                        {
                            isCheckPhoetic = CheckPhoetic(rotate);
                            if (isCheckPhoetic != true)
                            {
                                DrawMap.GlobalMap[lastX, lastY + 1] = 3;
                                lastY++;
                            }
                            else
                            {
                                DrawMap.GlobalMap[lastX, lastY] = 3;
                            }
                        } else
                        {
                            DrawMap.GlobalMap[lastX, lastY] = 3;
                        }
                        break;
                    case "Space":
                        DrawMap.GlobalMap[lastX, lastY] = 3;
                        CreateShoot();
                        break;
                    default:
                        DrawMap.GlobalMap[lastX, lastY] = 3;
                        break;
                }
            }
        }
       
        private void BotConroll(object sender, EventArgs e)
        {
            BotRoute();
        }
        private void SpawnBot()
        {
            DrawMap.GlobalMap[4, 0] = 6;
        }

        private void BulletFlying()
        {
            // рисуем полёт пули в зависимости от направления пули
            for(int i = 0; i < 10; i++)
                {
                for(int j = 0; j < 10; j++)
                    {
                    try // может получиться исключение, когда попытаемся переместить пулю за границы массива
                    {
                        if (DrawMap.ShootMap[i,j] == 4)
                        {
                            if(bulletDirection == 0)
                            {
                                DrawMap.ShootMap[i, j - 1] = 4;
                                DrawMap.ShootMap[i, j] = 0;
                                return;
                            } else if (bulletDirection == 1)
                            {
                                DrawMap.ShootMap[i+1, j] = 4;
                                DrawMap.ShootMap[i, j] = 0;
                                return;
                            }
                            else if (bulletDirection == 2)
                            {
                                DrawMap.ShootMap[i, j + 1] = 4;
                                DrawMap.ShootMap[i, j] = 0;
                                return;
                            }
                            else if (bulletDirection == 3)
                            {
                                DrawMap.ShootMap[i - 1, j] = 4;
                                DrawMap.ShootMap[i, j] = 0;
                                return;
                            }
                        }
                    }
                    catch
                    {
                        DrawMap.ShootMap[i, j] = 0;
                        isBulletFlying = false;
                        return;
                    }
                }
            }

        }
        private void BotBulletFlying()
        {

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    try
                    {
                        if (DrawMap.BotShootMap[i, j] == 4)
                        {
                            if (botBulletDirection == 0)
                            {
                                DrawMap.BotShootMap[i, j - 1] = 4;
                                DrawMap.BotShootMap[i, j] = 0;
                                return;
                            }
                            else if (botBulletDirection == 1)
                            {
                                DrawMap.BotShootMap[i + 1, j] = 4;
                                DrawMap.BotShootMap[i, j] = 0;
                                return;
                            }
                            else if (botBulletDirection == 2)
                            {
                                DrawMap.BotShootMap[i, j + 1] = 4;
                                DrawMap.BotShootMap[i, j] = 0;
                                return;
                            }
                            else if (botBulletDirection == 3)
                            {
                                DrawMap.BotShootMap[i - 1, j] = 4;
                                DrawMap.BotShootMap[i, j] = 0;
                                return;
                            }
                        }
                    }
                    catch
                    {
                        DrawMap.BotShootMap[i, j] = 0;
                        IsBotBulletFlying = false;
                        return;
                    }
                }
            }

        }
        private void Finished()
        {
            if (hpCout <= 0)
            {
                DialogResult result = MessageBox.Show(
                    "К сожалению, Вы проиграли. Попробуйте ещё раз!",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "Поздравляем, вы победили!",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                if (result == DialogResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }

        private void BulletContact()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if ((DrawMap.ShootMap[i, j] == 4) && (DrawMap.GlobalMap[i, j] == 1)) // если совпали позиции снаряда и блока
                    {
                        DrawMap.GlobalMap[i, j] = 0; // уничтожаем блок
                        DrawMap.ShootMap[i, j] = 0; // уничтожаем снаряд
                        isBulletFlying = false; // говорим, что снаряд не летит
                        SoundHit();
                        break;
                    }

                    if ((DrawMap.ShootMap[i, j] == 4) && (DrawMap.GlobalMap[i, j] == 7)) // если совпали позиции снаряда и блока
                    {
                        DrawMap.ShootMap[i, j] = 0; // уничтожаем снаряд
                        isBulletFlying = false; // говорим, что снаряд не летит
                        SoundHit();
                    }

                        if (((DrawMap.ShootMap[i, j]) == 4) && (DrawMap.GlobalMap[i, j] == 6)) { // если совпали позиции снаряда и бота
                        DrawMap.GlobalMap[i, j] = 0; // уничтожаем бота
                        DrawMap.ShootMap[i, j] = 0; // уничтожаем снаряд
                        SpawnBot(); // спавним бота
                        isBulletFlying = false; // говорим, что снаряд не летит
                        enemiesCount--; // уменьшаем счётчик ботов
                        SoundCrash();
                        if(enemiesCount <= 0) // проверяем, остались ли боты на карте
                        {
                            Finished();
                        }
                        break;
                    }
                }
            }
        }
        private void BotBulletContact()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if ((DrawMap.BotShootMap[i, j] == 4) && (DrawMap.GlobalMap[i, j] == 1))
                    {
                        DrawMap.GlobalMap[i, j] = 0;
                        DrawMap.BotShootMap[i, j] = 0;
                        IsBotBulletFlying = false;
                        SoundHit();
                        break;
                    }
                    if (((DrawMap.BotShootMap[i, j]) == 4) && (DrawMap.GlobalMap[i, j] == 3))
                    {
                        DrawMap.BotShootMap[i, j] = 0;
                        IsBotBulletFlying = false;
                        hpCout--;
                        if(hpCout <= 0)
                        {
                            SoundCrash();
                            Finished();
                        }
                    }
                    if (((DrawMap.ShootMap[i, j]) == 4) && (DrawMap.BotShootMap[i, j] == 4))
                    {
                        DrawMap.ShootMap[i, j] = 0;
                        DrawMap.BotShootMap[i, j] = 0;
                        isBulletFlying = false;
                        IsBotBulletFlying = false;
                        break;
                    }
                }
            }
        }
        private void BotRoute()
        {
            bool tried = false;
            // определяем положение бота
            for(int i = 0; i < 10; i++)
            {
                for (int j = 0; j<10; j++)
                {
                    if(DrawMap.GlobalMap[i,j] == 6)
                    {
                        botX = i;
                        botY = j;
                        break;
                    }
                }
            }
            int botLastX = botX;    // проверяем на коллизию по X
            int botLastY = botY;    // проверяем на коллизию по Y

            if (botX == lastX) // если совпали Х, стреляем
            {
                if (botY < lastY)
                {
                    botDirection = 2;
                }
                else if (botY > lastY)
                {
                    botDirection = 0;
                }
                BotCreateShoot();
            }
            else if (botY == lastY){  // если совпали Y, стреляем
                if (botX < lastX)
                {
                    botDirection = 1;
                }
                else if (botX > lastX)
                {
                    botDirection = 3;
                }
                BotCreateShoot();
            } else 
                // проверка на позицию и перемещение, в зависимости от положения игрока
                if (botX <= lastX)
                {
                DrawMap.GlobalMap[botX, botY] = 0;
                botDirection = 3;
                botLastX = botX;
                botX++;
                if ((DrawMap.GlobalMap[botX, botY] == 2) || (DrawMap.GlobalMap[botX, botY] == 1))
                {
                    DrawMap.GlobalMap[botLastX, botY] = 6;
                    tried = true;
                }
                else
                {
                    DrawMap.GlobalMap[botX, botY] = 6;
                }
                }
                else if (botX >= lastX)
                {
                    DrawMap.GlobalMap[botX, botY] = 0;
                    botDirection = 1;
                    botLastX = botX;
                    botX--;
                    if ((DrawMap.GlobalMap[botX, botY] == 2) || (DrawMap.GlobalMap[botX, botY] == 1))
                    {
                        DrawMap.GlobalMap[botLastX, botY] = 6;
                        tried = true;
                    }
                    else 
                    {
                        DrawMap.GlobalMap[botX, botY] = 6;
                    }
                }
                if(tried == true)
                {
                    if (botY <= lastY)
                    {
                        botX--;
                        DrawMap.GlobalMap[botLastX, botY] = 0;
                        botDirection = 2;
                        botLastY = botY;
                        botY++;
                        if ((DrawMap.GlobalMap[botX, botY] == 2) | (DrawMap.GlobalMap[botX, botY] == 1)) // если возникло, что бот стал в углу и не может двигаться, он стреляет
                        {
                            DrawMap.GlobalMap[botLastX, botLastY] = 6;
                            BotCreateShoot();
                    }
                        else
                        {
                            DrawMap.GlobalMap[botX, botY] = 6;
                        }
                    } 
                    else if (botY >= lastY)
                {
                    botX++;
                    DrawMap.GlobalMap[botLastX, botY] = 0;
                    botDirection = 0;
                    botLastY = botY;
                    botY--;
                    if ((DrawMap.GlobalMap[botX, botY] == 2) | (DrawMap.GlobalMap[botX, botY] == 1))
                    {
                        DrawMap.GlobalMap[botLastX, botLastY] = 6;
                            BotCreateShoot();
                    }
                    else
                    {
                        DrawMap.GlobalMap[botX, botY] = 6;
                    }
                }
                }
        }

        private void BotCreateShoot()
        {
            if (IsBotBulletFlying == false)
            {
                botBulletDirection = botDirection;
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (DrawMap.GlobalMap[i, j] == 6)
                        {
                            botX = i;
                            botY = j;
                            break;
                        }
                    }
                }
                switch (botBulletDirection)
                {
                    case 0:
                        {
                            try
                            {
                                if ((DrawMap.GlobalMap[botX, botY - 1] == 0) || (DrawMap.GlobalMap[botX, botY - 1] == 1) || (DrawMap.GlobalMap[botX, botY - 1] == 2))
                                {
                                    DrawMap.BotShootMap[botX, botY] = 4;
                                    IsBotBulletFlying = true;
                                    SoundPunch();
                                }
                            }
                            catch
                            {
                                break;
                            }

                            break;
                        }
                    case 1:
                        {
                            {
                                try
                                {
                                    if ((DrawMap.GlobalMap[botX + 1, botY] == 0) || (DrawMap.GlobalMap[botX + 1, botY] == 1) || (DrawMap.GlobalMap[botX + 1, botY] == 2))
                                    {
                                        DrawMap.BotShootMap[botX, botY] = 4;
                                        IsBotBulletFlying = true;
                                        botDirection = 3;
                                        SoundPunch();
                                    }
                                }
                                catch
                                {
                                    break;
                                }

                                break;
                            }
                        }
                    case 2:
                        {
                            try
                            {
                                if ((DrawMap.GlobalMap[botX, botY + 1] == 0) || (DrawMap.GlobalMap[botX, botY + 1] == 1) || (DrawMap.GlobalMap[botX, botY + 1] == 2))
                                {
                                    DrawMap.BotShootMap[botX, botY] = 4;
                                    IsBotBulletFlying = true;
                                    SoundPunch();
                                }
                            }
                            catch
                            {
                                break;
                            }
                            break;
                        }
                    case 3:
                        {
                            try
                            {
                                if ((DrawMap.GlobalMap[botX - 1, botY] == 0) || (DrawMap.GlobalMap[botY - 1, botY] == 1) || (DrawMap.GlobalMap[botY - 1, botY] == 2))
                                {
                                    DrawMap.BotShootMap[botX, botY] = 4;
                                    IsBotBulletFlying = true;
                                    SoundPunch();
                                    botDirection = 1;
                                }
                            }
                            catch
                            {
                                break;
                            }

                            break;
                        }
                }
            }
        }

        private void CreateShoot()
        {
            if (isBulletFlying == false)
            {
                // задаём направление снаряда в зависимости от текущего положения танка
                if(rotate == 0)
                {
                    bulletDirection = 0;
                } else if(rotate == 1)
                {
                    bulletDirection = 1;
                } else if( rotate == 2)
                {
                    bulletDirection = 2;
                } else if (rotate == 3)
                {
                    bulletDirection = 3;
                }
                // следующий код создаёт на соседней клетке относительно игрока снаряд, летящий в сторону, куда направлен ствол игрока
                switch (bulletDirection)
                {
                    case 0:
                        {
                            try
                            {
                                if ((DrawMap.GlobalMap[lastX, lastY - 1] == 0) || (DrawMap.GlobalMap[lastX, lastY - 1] == 1) || (DrawMap.GlobalMap[lastX, lastY - 1] == 2))
                                {
                                    DrawMap.ShootMap[lastX, lastY] = 4;
                                    isBulletFlying = true;
                                    SoundPunch();
                                }
                            }
                            catch
                            {
                                break;
                            }

                            break;
                        }
                    case 1:
                        {
                            try
                            {
                                if ((DrawMap.GlobalMap[lastX + 1, lastY] == 0) || (DrawMap.GlobalMap[lastX + 1, lastY] == 1) || (DrawMap.GlobalMap[lastX + 1, lastY] == 2))
                                {
                                    DrawMap.ShootMap[lastX, lastY] = 4;
                                    isBulletFlying = true;
                                    SoundPunch();
                                }
                            }
                            catch
                            {
                                break;
                            }

                            break;
                        }
                    case 2:
                        {
                            try
                            {
                                if ((DrawMap.GlobalMap[lastX, lastY + 1] == 0) || (DrawMap.GlobalMap[lastX, lastY + 1] == 1) || (DrawMap.GlobalMap[lastX, lastY + 1] == 2))
                                {
                                    DrawMap.ShootMap[lastX, lastY] = 4;
                                    isBulletFlying = true;
                                    SoundPunch();
                                }
                            }
                            catch
                            {
                                break;
                            }
                            break;
                        }
                    case 3:
                        {
                            try
                            {
                                if ((DrawMap.GlobalMap[lastX - 1, lastY] == 0) || (DrawMap.GlobalMap[lastX - 1, lastY] == 1) || (DrawMap.GlobalMap[lastX - 1, lastY] == 2))
                                {
                                    DrawMap.ShootMap[lastX, lastY] = 4;
                                    isBulletFlying = true;
                                    SoundPunch();
                                }
                            }
                            catch
                            {
                                break;
                            }

                            break;
                        }
                }
            }
        }
        private bool CheckPhoetic(int _rotate)
        {
            // определяет, есть ли что-то на позиции, в которую необходимо попасть
            if(_rotate == 1)
            {
                if (DrawMap.GlobalMap[lastX + 1, lastY] != 0)
                {
                    return true;
                }
                else return false;
            }
            if (_rotate == 0)
            {
                if (DrawMap.GlobalMap[lastX, lastY - 1] != 0)
                {
                    return true;
                }
                else return false;
            }
            if (_rotate == 3)
            {
                if (DrawMap.GlobalMap[lastX - 1, lastY] != 0)
                {
                    return true;
                }
                else return false;
            }
            if (_rotate == 2)
            {
                if (DrawMap.GlobalMap[lastX, lastY + 1] != 0)
                {
                    return true;
                }
                else return false;
            }
            return false;
        }
           
        private void SoundPunch()
        {
            WindowsMediaPlayer punch = new WindowsMediaPlayer();
            punch.URL = Directory.GetCurrentDirectory() + @"\Source\Music\punch.mp3";
            punch.settings.volume = 60;
            punch.controls.play();
        }

        private void SoundHit()
        {
            WindowsMediaPlayer hit = new WindowsMediaPlayer();
            hit.URL = Directory.GetCurrentDirectory() + @"\Source\Music\hit.mp3";
            hit.settings.volume = 60;
            hit.controls.play();
        }
        private void SoundCrash()
        {
            WindowsMediaPlayer crash = new WindowsMediaPlayer();
            crash.URL = Directory.GetCurrentDirectory() + @"\Source\Music\crash.mp3";
            crash.settings.volume = 60;
            crash.controls.play();
        }

        private static void Drawing(Graphics gr)
        {
            gr.Clear(Color.Black);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (DrawMap.GlobalMap[i, j] == 1)
                    {
                        Bitmap bitmap = new Bitmap(Block);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    } else if ((DrawMap.GlobalMap[i, j] == 3)&&(rotate == 0))
                    {
                        Bitmap bitmap = new Bitmap(PlayerUp);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 3) && (rotate == 1))
                    {
                        Bitmap bitmap = new Bitmap(PlayerRight);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 3) && (rotate == 2))
                    {
                        Bitmap bitmap = new Bitmap(PlayerDown);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 3) && (rotate == 3))
                    {
                        Bitmap bitmap = new Bitmap(PlayerLeft);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.ShootMap[i, j] == 4)&&(bulletDirection == 0))
                    {
                        Bitmap bitmap = new Bitmap(BulletUp);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.ShootMap[i, j] == 4) && (bulletDirection == 1))
                    {
                        Bitmap bitmap = new Bitmap(BulletRight);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.ShootMap[i, j] == 4) && (bulletDirection == 2))
                    {
                        Bitmap bitmap = new Bitmap(BulletDown);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.ShootMap[i, j] == 4) && (bulletDirection == 3))
                    {
                        Bitmap bitmap = new Bitmap(BulletLeft);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 2))
                    {
                        Bitmap bitmap = new Bitmap(Water);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 5))
                    {
                        Bitmap bitmap = new Bitmap(Finish);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 6) && (botDirection == 0))
                    {
                        Bitmap bitmap = new Bitmap(BotUp);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 6) && (botDirection == 1))
                    {
                        Bitmap bitmap = new Bitmap(BotLeft);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 6) && (botDirection == 2))
                    {
                        Bitmap bitmap = new Bitmap(BotDown);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                    else if ((DrawMap.GlobalMap[i, j] == 6) && (botDirection == 3))
                    {
                        Bitmap bitmap = new Bitmap(BotRight);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    } else if((DrawMap.BotShootMap[i,j]) == 4)
                    {
                        Bitmap bitmap = new Bitmap(BotBullet);
                        gr.DrawImage(bitmap, 50 * i, 50 * j);
                    }
                }
            }
        }

        private void closePicture_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
