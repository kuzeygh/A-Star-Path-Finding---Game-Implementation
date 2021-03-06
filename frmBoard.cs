﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
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
        static int boyut = 10;
        Node[,] Board ;
        Koordinat bitis;
        string Yol;
        string SolutionPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        SoundPlayer player = new SoundPlayer();
        private void frmBoard_Load(object sender, EventArgs e)
        {
            Yol = "";
            boyut = frmGame.boyut;
            Board = new Node[boyut, boyut];
            //DataTable dt = new DataTable();
            //for (int i = 0; i < boyut; i++)
            //    dt.Columns.Add();
            //for (int i = 0; i < boyut; i++)
            //    dt.Rows.Add();
            //dtgBoard.DataSource = dt;
            DataTable dt = new DataTable();
            for (int i = 0;i<boyut;i++)
            {
                Image image = Image.FromFile(SolutionPath + @"\Content\arkaplan.jpg");
                dtgBoard.Columns.Add(new DataGridViewImageColumn()
                {
                    Image = image,
                    ImageLayout = DataGridViewImageCellLayout.Stretch                    
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
            
            baslangic = frmGame.Baslangic;
            bitis = frmGame.Bitis;
            MessageBox.Show("Başlangıç Koordinatlar: " + (baslangic.X+1).ToString() + "," + (baslangic.Y+1).ToString()+"\n"+
                            "Bitiş Koordinatlar: " + (bitis.X+1).ToString() + ","+ (bitis.Y+1).ToString());
            dtgBoard.Rows[baslangic.Y].Cells[baslangic.X].Value = Bitmap.FromFile(SolutionPath + @"\Content\fare.jpg");
            dtgBoard.Rows[bitis.Y].Cells[bitis.X].Value = Bitmap.FromFile(SolutionPath + @"\Content\peynir.jpg");
            string path = SolutionPath + @"\Content\Fare_Sesi.wav"; // Müzik adresi
            player.SoundLocation = path;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dtgBoard.Rows[baslangic.Y].Cells[baslangic.X].Value = Bitmap.FromFile(SolutionPath + @"\Content\arkaplan.jpg");
            AStar astar = new AStar();
            List<Node> path = astar.Search(Board[baslangic.X, baslangic.Y], Board[bitis.X, bitis.Y]);
            if (path[path.Count - 1] == null) path.Remove(null);
            if (path.Count > 1)
            {
                if (path[0].X != bitis.X && path[0].Y != bitis.Y)
                {
                    timer1.Stop();
                    player.Stop();
                    MessageBox.Show("Yol bitti");
                }
                    
                baslangic.X = path[path.Count - 2].X;
                baslangic.Y = path[path.Count - 2].Y;
            }
            else if (path.Count >= 0)
            {
                baslangic.X = path[path.Count - 1].X;
                baslangic.Y = path[path.Count - 1].Y;
            }
            else
            {
                timer1.Stop();
                player.Stop();
            }
            Yol += baslangic.X.ToString() + " - " + baslangic.Y.ToString()+"\n";
            dtgBoard.Rows[baslangic.Y].Cells[baslangic.X].Value = Bitmap.FromFile(SolutionPath + @"\Content\fare.jpg");
            if (baslangic.X == bitis.X && baslangic.Y == bitis.Y)
            {
                timer1.Stop();
                player.Stop();
                MessageBox.Show("Oyun Bitti");
                MessageBox.Show(Yol);
            }



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

        private void dtgBoard_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Koordinat Wall;
            for (int i = 0;i< dtgBoard.SelectedCells.Count;i++)
            {
                Wall = new Koordinat();
                Wall.X = dtgBoard.SelectedCells[i].ColumnIndex;
                Wall.Y = dtgBoard.SelectedCells[i].RowIndex;
                Board[Wall.X, Wall.Y].isWall = true;
                dtgBoard.SelectedCells[i].Value = Bitmap.FromFile(SolutionPath + @"\Content\duvar.jpg");
            }
        }

        private void başlatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
            player.Play();
        }
    }
}
