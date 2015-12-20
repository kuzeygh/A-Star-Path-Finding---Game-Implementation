﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_Star_Path_Finding_Implementation
{
    public partial class frmBoard : Form
    {
        public frmBoard()
        {
            InitializeComponent();
        }
        Koordinat baslangic;
        public static int boyut = 10;
        Node[,] Board = new Node[boyut, boyut];
        Koordinat bitis;
        private void frmBoard_Load(object sender, EventArgs e)
        {
            //DataTable dt = new DataTable();
            //for (int i = 0; i < boyut; i++)
            //    dt.Columns.Add();
            //for (int i = 0; i < boyut; i++)
            //    dt.Rows.Add();
            //dtgBoard.DataSource = dt;
            DataTable dt = new DataTable();
            for (int i = 0;i<boyut;i++)
            {
                Image image = Image.FromFile(@"C:\Users\Muhammet Kaya\Pictures\arkaplan.jpg");
                dtgBoard.Columns.Add(new DataGridViewImageColumn()
                {
                    Image = image,
                    ImageLayout = DataGridViewImageCellLayout.Stretch,
                });
            }

            for (int i = 0; i < boyut; i++)
                dtgBoard.Rows.Add();
            //Koordinatları belirleme
            for (int i = 0; i< boyut; i++)
            {
                for (int j = 0; j< boyut; j++)
                {
                    Board[i, j] = new Node(i, j);
                }
            }
            //Node Komşu Tanımlama
            for (int i = 0; i < boyut; i++)
            {
                for (int j = 0; j < boyut; j++)
                {
                    if (i - 1 >= 0)
                        Board[i, j].Neighbors.Add(Board[i - 1, j]);
                    if (i + 1 < boyut)
                        Board[i, j].Neighbors.Add(Board[i + 1, j]);
                    if (j - 1 >= 0)
                        Board[i, j].Neighbors.Add(Board[i, j - 1]);
                    if (j + 1 < boyut)
                        Board[i, j].Neighbors.Add(Board[i, j + 1]);
                }
            }
            
            baslangic = new Koordinat();
            bitis = new Koordinat();
            MessageBox.Show("Başlangıç Koordinatlar: " + (baslangic.X+1).ToString() + "," + (baslangic.Y+1).ToString()+"\n"+
                            "Bitiş Koordinatlar: " + (bitis.X+1).ToString() + ","+ (bitis.Y+1).ToString());
            timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dtgBoard.Rows[baslangic.Y].Cells[baslangic.X].Value = Bitmap.FromFile(@"C:\Users\Muhammet Kaya\Pictures\fare.jpg");
            AStar astar = new AStar();
            List<Node> path = astar.Search(Board[baslangic.X, baslangic.Y], Board[bitis.X, bitis.Y]);
            if (path[path.Count - 1] == null) path.Remove(null);
            if (path.Count > 1)
            {
                baslangic.X = path[path.Count - 2].X;
                baslangic.Y = path[path.Count - 2].Y;
            }
            else if (path.Count >= 0)
            {
                baslangic.X = path[path.Count - 1].X;
                baslangic.Y = path[path.Count - 1].Y;
            }
            else
                timer1.Stop();
            


            
        }
        public class Koordinat
        {
            TimeSpan t = new TimeSpan();
            
            public int X { get; set; }
            public int Y { get; set; }

            public Koordinat()
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                X = rnd.Next(0, 9);
                Y = rnd.Next(0, 9);
            }
        }
    }
}