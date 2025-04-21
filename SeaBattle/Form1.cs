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
        PictureBox[,] pictures = new PictureBox[10, 10];
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
            // Создание игрового поля из PictureBox
            this.Text = playerField ? "Поле игрока" : "Поле компьютера";
            
            int[,] field = new int[10, 10];
            int loc_x = playerField ? 120 : 700; // Поле игрока слева, поле компьютера справа
            int loc_y = 50;

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    pictures[i, j] = new PictureBox();
                    pictures[i, j].Left = loc_x;
                    pictures[i, j].Top = loc_y;
                    pictures[i, j].Width = 50;
                    pictures[i, j].Height = 50;
                    pictures[i, j].Image = Properties.Resources.template;
                    pictures[i, j].Name = "PictureBox" + i.ToString() + j.ToString();
                    pictures[i, j].Tag = "empty";
                    if (playerField) 
                    {
                        pictures[i, j].Click += SetShips;
                    }
                    else {
                        pictures[i, j].Click += ShootShips;
                    }
                    pictures[i, j].SizeMode = PictureBoxSizeMode.Zoom;
                    pictures[i, j] = pictures[i, j];
                    this.Controls.Add(pictures[i, j]);

                    loc_x += 55;
                }
                loc_y += 55;
                loc_x = playerField ? 120 : 700;
            }

            if (playerField)
            {
                playerPictures = pictures;
                playerFieldState = field;
            }
            else
            {
                computerPictures = pictures;
                computerFieldState = field;
            }
        }

        private void SetShips(object sender, EventArgs e)
        {
            
            PictureBox picture = sender as PictureBox;
            int row = Convert.ToInt32(picture.Name[10].ToString());
            int col = Convert.ToInt32(picture.Name[11].ToString());
            MessageBox.Show("Set ships: " + row + " " + col);

            for (int i = 0; i < ships[count]; i++) {
                player_field[row, col] = 1;
                col++;
            }
            
        }

        private void ShootShips(object sender, EventArgs e)
        {
            MessageBox.Show("Shoot Ship");
        }




        private PictureBox[,] playerPictures;
        private PictureBox[,] computerPictures;
        private int[,] playerFieldState;
        private int[,] computerFieldState;
    }
}