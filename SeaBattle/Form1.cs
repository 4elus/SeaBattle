using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaBattle
{
    public partial class Form1 : Form
    {
        int[] ships = new int[10] { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };
        int count = 0;
        int[,] player_field = new int[10, 10];
        int[,] computer_field = new int[10, 10];
        PictureBox[,] field_player_pictures = new PictureBox[10, 10];
        PictureBox[,] field_computer_pictures = new PictureBox[10, 10];
        bool dir = false; // false - горизонтально, true - вертикально
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateGameField(playerField: true);
            CreateGameField(playerField: false);
           
        }

        private void CreateGameField(bool playerField)
        {
            this.Text = playerField ? "Поле игрока" : "Поле компьютера";

            int loc_x = playerField ? 120 : 700;
            int loc_y = 50;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    PictureBox pic = new PictureBox();
                    pic.Left = loc_x;
                    pic.Top = loc_y;
                    pic.Width = 50;
                    pic.Height = 50;
                    pic.Image = Properties.Resources.template;
                    pic.Name = "PictureBox" + i.ToString() + j.ToString();
                    pic.Tag = "empty";
                    pic.SizeMode = PictureBoxSizeMode.Zoom;

                    if (playerField)
                    {
                        pic.Click += SetShips;
                        pic.MouseHover += PictureBox_MouseHover;
                        pic.MouseLeave += PictureBox_MouseEnd;
                        field_player_pictures[i, j] = pic;
                    }
                    else
                    {
                        pic.Click += ShootShips;
                        field_computer_pictures[i, j] = pic;
                    }

                    this.Controls.Add(pic);

                    loc_x += 55;
                }
                loc_y += 55;
                loc_x = playerField ? 120 : 700;
            }
        }

        private void SetShips(object sender, EventArgs e)
        {
            if (count >= ships.Length)
            {
                MessageBox.Show("Все корабли уже расставлены.");
                return;
            }

            PictureBox picture = sender as PictureBox;
            int row = Convert.ToInt32(picture.Name[10].ToString());
            int col = Convert.ToInt32(picture.Name[11].ToString());
            int shipLength = ships[count];

            // Проверка на выход за границы
            if ((dir && row + shipLength > 10) || (!dir && col + shipLength > 10))
            {
                MessageBox.Show("Корабль выходит за границы поля.");
                return;
            }

            // Проверка на пересечения
            for (int i = 0; i < shipLength; i++)
            {
                int r = dir ? row + i : row;
                int c = dir ? col : col + i;

                if (player_field[r, c] == 1)
                {
                    MessageBox.Show("На этом месте уже есть корабль.");
                    return;
                }
            }

            // Установка корабля
            for (int i = 0; i < shipLength; i++)
            {
                int r = dir ? row + i : row;
                int c = dir ? col : col + i;

                player_field[r, c] = 1;
                field_player_pictures[r, c].Image = Properties.Resources.shipset;
                field_player_pictures[r, c].Tag = "ship";
            }

            count++;
        }

        private void ShootShips(object sender, EventArgs e)
        {
            MessageBox.Show("Выстрел произведён!");
        }

        private void PictureBox_MouseHover(object sender, EventArgs e)
        {
            if (count < 10)
            {
                PictureBox picture = sender as PictureBox;
                int row = Convert.ToInt32(picture.Name[10].ToString());
                int col = Convert.ToInt32(picture.Name[11].ToString());
                int shipLength = ships[count];

                // Проверка на выход за границы
                if ((dir && row + shipLength > 10) || (!dir && col + shipLength > 10))
                    return;

                // Окрашивание клеток
                for (int i = 0; i < shipLength; i++)
                {
                    int r = dir ? row + i : row;
                    int c = dir ? col : col + i;

                    if (field_player_pictures[r, c].Tag.ToString() != "ship")
                    {
                        field_player_pictures[r, c].Image = Properties.Resources.shipset;
                    }
                }
            }
        }

        private void PictureBox_MouseEnd(object sender, EventArgs e)
        {
            if (count < 10)
            {
                PictureBox picture = sender as PictureBox;
                int row = Convert.ToInt32(picture.Name[10].ToString());
                int col = Convert.ToInt32(picture.Name[11].ToString());
                int shipLength = ships[count];

                // Проверка на выход за границы
                if ((dir && row + shipLength > 10) || (!dir && col + shipLength > 10))
                    return;

                // Сброс окрашивания клеток
                for (int i = 0; i < shipLength; i++)
                {
                    int r = dir ? row + i : row;
                    int c = dir ? col : col + i;

                    if (field_player_pictures[r, c].Tag.ToString() != "ship")
                    {
                        field_player_pictures[r, c].Image = Properties.Resources.template;
                    }
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.E)
            {
                dir = !dir; // Переключение направления
                
            }

            for (int i = 0; i < field_player_pictures.GetUpperBound(0)+1; i++)
            {
                for (int j = 0; j < field_player_pictures.GetUpperBound(0)+1; j++)
                {
                    if (field_player_pictures[i, j].Tag.ToString() != "ship")
                    {
                        field_player_pictures[i, j].Image = Properties.Resources.template;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count_pc = 0;

            while (count_pc < 10)
            {
                bool out_of_border = false;
                bool is_empty = false;
                int row = random.Next(0, 10);
                int col = random.Next(0, 10);
                int random_dir = random.Next(0, 2); // 0 - гор ; 1 - верт
                bool dir = random_dir == 1 ? true : false;
                MessageBox.Show(row + " : " + col);

                // проверка выхода за границу поля и проверка на пустые клетки
                int shipLength = ships[count_pc];

                // Проверка на выход за границы
                if ((dir && row + shipLength > 10) || (!dir && col + shipLength > 10))
                {
                    MessageBox.Show("Корабль выходит за границы поля.");
                    out_of_border = true;
                }

                // Проверка на пересечения
                for (int i = 0; i < shipLength; i++)
                {
                    int r = dir ? row + i : row;
                    int c = dir ? col : col + i;

                    if (computer_field[r, c] == 1)
                    {
                        MessageBox.Show("На этом месте уже есть корабль.");
                        is_empty = true;
                    }
                }

                // Установка корабля
                for (int i = 0; i < shipLength; i++)
                {
                    int r = dir ? row + i : row;
                    int c = dir ? col : col + i;

                    computer_field[r, c] = 1;
                    field_computer_pictures[r, c].Image = Properties.Resources.shipset;
                    field_computer_pictures[r, c].Tag = "ship";
                }
               if (out_of_border == false && is_empty == false)
                {
                    count_pc++;
                }

            }

            MessageBox.Show("Компьютер готов!");
        }
    }
}
