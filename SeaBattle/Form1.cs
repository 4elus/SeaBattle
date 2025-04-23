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
        PictureBox[,] field_player_pictures = new PictureBox[10, 10];
        PictureBox[,] field_computer_pictures = new PictureBox[10, 10]; 
        bool dir = false; // false - гор, true - верт
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
                        pic.MouseHover += Picturebox_MouseHover;
                        pic.MouseLeave += Picturebox_MouseEnd;
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

            if (col + shipLength > 10)
            {
                MessageBox.Show("Корабль выходит за границы поля.");
                return;
            }

            // Проверка на пересечения
            for (int i = 0; i < shipLength; i++)
            {
                if (player_field[row, col + i] == 1)
                {
                    MessageBox.Show("На этом месте уже есть корабль.");
                    return;
                }
            }

            // Установка корабля
            for (int i = 0; i < shipLength; i++)
            {
                player_field[row, col + i] = 1;
                field_player_pictures[row, col + i].Image = Properties.Resources.shipset; 
                field_player_pictures[row, col + i].Tag = "ship";
            }

            count++;
        }

        private void ShootShips(object sender, EventArgs e)
        {
            MessageBox.Show("Выстрел произведён!");
        }

        private void Picturebox_MouseHover(object sender, EventArgs e)
        {
            PictureBox picture = sender as PictureBox;
            int row = Convert.ToInt32(picture.Name[10].ToString());
            int col = Convert.ToInt32(picture.Name[11].ToString());

            if (field_player_pictures[row, col].Tag.ToString() != "ship")
            {
                field_player_pictures[row, col].Image = Properties.Resources.posship;
            }  
        }
        private void Picturebox_MouseEnd(object sender, EventArgs e)
        {

            PictureBox picture = sender as PictureBox;
            int row = Convert.ToInt32(picture.Name[10].ToString());
            int col = Convert.ToInt32(picture.Name[11].ToString());

            if (field_player_pictures[row, col].Tag.ToString() != "ship")
            {
                field_player_pictures[row, col].Image = Properties.Resources.template;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.E) 
            {
                dir = !dir;
                MessageBox.Show(dir.ToString());
            }
        }
    }
}