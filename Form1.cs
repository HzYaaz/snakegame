using System;
using System.Diagnostics;
using System.Drawing;       // PictureBox için
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Yilan
{
    

    public partial class Form1 : Form
    {
        class Snake
        {
            SnakeParts[] snakePart;           //
            int snakeSize;                   //      Yılanımız için , Yılanımızın boyutu (parçaları) , büyüklüğü ve yönünü tanımlıyoruz.
            Direction ourDirection;         //
            public Snake()
            {
                snakePart = new SnakeParts[1];                  
                snakeSize = 1;                                     // Yılanın büyüklüğünün değerini en başta 1 (başı) yapıyoruz. 
                snakePart[0] = new SnakeParts(170, 170);           // Yılanın başının başlangıç noktasını buradan ayarlıyoruz.

            }


     //----------------------YILANIN HAREKET ETTİĞİ , KUYRUĞUNUN BAŞINI TAKİP ETTİĞİ KISIM----------------------------------//
            public void Move(Direction direction)
            {
                ourDirection = direction;

                for (int i = snakePart.Length - 1; i > 0; i--)                              // Yılanın parçalarının , yılanın başını takip ettiği döngümüz.
                {
                    snakePart[i] = new SnakeParts(snakePart[i - 1].x_, snakePart[i - 1].y_);
                }
                snakePart[0] = new SnakeParts(snakePart[0].x_ + direction._x, snakePart[0].y_ + direction._y);
            }
            //------------------------------------------------------------------------------------------------//

            //------------------------YILAANIMIZIN BÜYÜDÜĞÜ KISIM-----------------------------------------//
            public void Grow()
            {
                Array.Resize(ref snakePart, snakePart.Length + 1);    // Array.Resize yaparak , yılan parçalarımıza yeni bir boyut , parça ekleyeceğimiz için burada Resize komutumuzu kullanıp yeniden boyutlandırıyoruz.
                snakePart[snakePart.Length - 1] = new SnakeParts(snakePart[snakePart.Length - 2].x_ - ourDirection._x, snakePart[snakePart.Length - 2].y_ - ourDirection._y);       // Yılanımızın son parçası , gittiğimiz yön ve konuma göre eklenecek
                snakeSize++;
            }
            //------------------------------------------------------------------------------------------------//


            public Point GetPos(int number)
            {
                return new Point(snakePart[number].x_, snakePart[number].y_);   // Her parçamızın olduğu konumu alıyoruz. Burada Point kullanmmızın nedeni X ve Y noktalarını belirlemek için. Bunu kullanabilmemiz için System.Drawing'i dahil etmemiz gerekiyor.
            }
            public int SnakeSize
            {
                get
                {
                    return snakeSize;
                }
            }
        }
        class SnakeParts
        {
            public int x_;
            public int y_;
            public int size_x;
            public int size_y;
            public SnakeParts(int x, int y)
            {
                x_ = x;
                y_ = y;
                size_x = 10;
                size_y = 10;
            }
        }
        class Direction
        {
            public int _x;
            public int _y;
            public Direction(int x, int y)
            {
                _x = x;
                _y = y;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        int second = 0, minute = 0;
        Snake ourSnake;
        Direction ourDirection;
        PictureBox[] pbSnakeParts;    // Yılanın parçalarını göstermek için...
        bool isBait = false;
        Random random = new Random();
        PictureBox pbBait;           // Yemimizi göstermek için...
        double score = 0;
        public bool game = true;
        DialogResult playAgain = new DialogResult();
        private void Form1_Load(object sender, EventArgs e)
        {
            panel1.Enabled = false;
            panel2.Enabled = true;
            panel2.Visible = false;
        }

        //--------------------------YENİ OYUN OLUŞTURMAK İÇİN YAPTIĞIMIZ FONKSİYON-------------------------------------//
        private void NewGame()
        {
            isBait = false;
            score = 0;
            second = 0;
            minute = 0;
            ourSnake = new Snake();
            ourDirection = new Direction(-10, 0);   // Burası değiştirilebilir. -10 demek , X pozisyonunda 10 piksel ile hareketine başlıyor demek.
            pbSnakeParts = new PictureBox[0];
            for (int i = 0; i < 1; i++)
            {
                Array.Resize(ref pbSnakeParts, pbSnakeParts.Length + 1);
                pbSnakeParts[i] = PbAdd();
            }
            timer2.Enabled = true;
            timer1.Start();
            timer2.Start();
        }
        //-----------------------------------------------------------------------------------------------------------//


        private PictureBox PbAdd()
        {         
            PictureBox pb = new PictureBox();
            pb.Size = new Size(10, 10);     //Her bir parçanın boyutunu 10,10 piksel yapıyoruz. Bu , yılanımızın en ve boy değerleridir.
            pb.BackColor = Color.Green;     // Yılanımızın rengi ayarlıyoruz
            pb.Location = ourSnake.GetPos(pbSnakeParts.Length - 1); 
            panel1.Controls.Add(pb);
            return pb;
        }
        private void PbUpdate()
        {
            for (int i = 0; i < pbSnakeParts.Length; i++)
            {
                pbSnakeParts[i].Location = ourSnake.GetPos(i);   // Yılan parçalarımızın pozisyonunu devamlı alıp güncelliyoruz.
            }
        }


        //-------------------------TUŞLARA GÖRE YÖNÜMÜZ-----------------------------------------//
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (ourDirection._y != 10)                          // Yılan , üste giderken , aşağı bsılırsa aşağı gidemez. Aynı şey alt satırdaki yön kodları için de geçerlidir
                {
                    ourDirection = new Direction(0, -10);
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (ourDirection._y != -10)
                {
                    ourDirection = new Direction(0, 10);
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (ourDirection._x != 10)
                {
                    ourDirection = new Direction(-10, 0);
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (ourDirection._x != -10)
                {
                    ourDirection = new Direction(10, -0);
                }
            }
             else if(e.KeyCode == Keys.B)
            {
                panel1.Controls.Clear();
                panel1.Enabled = true;
                pictureBox1.Visible = false;
                lblSnakeText.Visible = false;
                timer1.Start();
                timer2.Start();
                NewGame();           // Oyuncu B'ye basarsa yeni oyuna başlıyor.
            }
             else if(e.KeyCode == Keys.D && game == true)   // Oyuncu D ye basarsa oyun duruyor.
            {
                game = false;
                timer1.Stop();
                timer2.Stop();
                MessageBox.Show("Bu kısma erişim izniniz yok !");
            }
            else if (e.KeyCode == Keys.D && game == false)  // Tekrar D ye basarsa oyun başlıyor.
            {
                game = true;
                MessageBox.Show("Oyun Tekrar Başlatıldı.");
                timer1.Start();
                timer2.Start();
            }
        }
        //--------------------------------------------------------------------------------------------//

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "Skor: " + score.ToString();    
            ourSnake.Move(ourDirection);
            PbUpdate();
            CreateBait();
            Feed();
            HitItself();
            HitWall();
        }

        //------------------------------YEMİ OLUŞTURDUĞUMUZ KISIM----------------------------------------//
        public void CreateBait()
        {
            if (!isBait)
            {
                PictureBox pb = new PictureBox();
                pb.BackColor = Color.Red;
                pb.Size = new Size(10, 10);   // 10,10 piksellik , kırmızı renkli bir yem oluşturduk.
                pb.Location = new Point(random.Next(panel1.Width / 10) * 10, random.Next(panel1.Height / 10) * 10);  // Panelin içinde rastgele bir yerde oluşacak.
                pbBait = pb;
                isBait = true;
                panel1.Controls.Add(pb);

            }
        }
        //--------------------------------------------------------------------------------------------//


        //----------------------------YILANIN YEMİ YEDİĞİNDE OLACAKLAR-------------------------------------//
        public void Feed()
        {
            if (ourSnake.GetPos(0) == pbBait.Location)
            {
                score += 12.7;
                ourSnake.Grow();
                Array.Resize(ref pbSnakeParts, pbSnakeParts.Length + 1);   //Yılanın parçası (kuyruğu) uzuyor.
                pbSnakeParts[pbSnakeParts.Length - 1] = PbAdd();
                isBait = false;
                panel1.Controls.Remove(pbBait);   // Yemi yediğimiz zaman , paneldeki yenilen yemi siliyor
            }
        }
        //--------------------------------------------------------------------------------------------//


        //-------------------------------------YILANIN KENDİNE ÇARPMASI-------------------------------------------------//
        public void HitItself()
        { 
            for (int i = 1; i < ourSnake.SnakeSize; i++)
			{
                if (ourSnake.GetPos(0) == ourSnake.GetPos(i))       // Yılanın başının pozisyonu , kuyruğundan birine denk gelirse...
                {
                    LoseGame();
                }
			}
         //--------------------------------------------------------------------------------------------------------//

        }


        //---------------------------------YILANIN DUVARA ÇARPTIĞI KISIM----------------------------------------------//
        public void HitWall()
        {
            Point p = ourSnake.GetPos(0);
            if (p.X < 0 || p.X > panel1.Width - 10 || p.Y < 0 || p.Y > panel1.Height - 10) 
            {
                LoseGame();
            }
        }
        //----------------------------------------------------------------------------------------------------------//


        //---------------------------------------OYUNU KAYBETTİĞİMİZ KISIM------------------------------------------//
        private void LoseGame()
        {
            timer1.Stop();
            timer2.Stop();
            playAgain = MessageBox.Show("Tekrar Oynamak istiyor musunuz ?", "Kaybettiniz", MessageBoxButtons.YesNo);
            if(playAgain == DialogResult.Yes)
            {
                panel1.Controls.Clear();
                NewGame();
            }
            if(playAgain == DialogResult.No)
            {
                MessageBox.Show("Program Kapatıldı. Oynadığınız için teşekkürler.");
                Application.Exit();
            }
           
        }
        //---------------------------------------------------------------------------------------------------------//


        //------------------------------KİŞİYİ KAYDETTİĞİMİZ KISIM---------------------------------//
        private void button2_Click(object sender, EventArgs e)
        {
            if(!File.Exists("SCORE.txt"))
            {
                File.Create("SCORE.txt").Close();
                using(StreamWriter sw = File.AppendText("SCORE.txt"))
                {
                    sw.WriteLine(textBox1.Text);
                }
            }
            else
            {
                using(StreamWriter sw = File.AppendText("SCORE.txt"))
                {
                    sw.WriteLine(textBox1.Text);
                }
            }

            MessageBox.Show("Kayıt Yapıldı");
            panel1.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;

        }
        //--------------------------------------------------------------------------------------------//


        //-----------------------YARDIM BUTONU------------------------------//
        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            panel2.Visible = true;
            panel2.Enabled = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }
        //----------------------------------------------------------------//

        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            panel2.Visible = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Kolay Mod Seçildi");
            btnSave.Enabled = false;
            btnHelp.Enabled = false;
            btnShowScore.Enabled = false;
            textBox1.Enabled = false;
            btnSave.Visible = false;
            btnHelp.Visible = false;
            btnShowScore.Visible = false;
            textBox1.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            timer1.Interval = 100;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Zor Mod Seçildi");
            btnSave.Enabled = false;
            btnHelp.Enabled = false;
            btnShowScore.Enabled = false;
            textBox1.Enabled = false;
            btnSave.Visible = false;
            btnHelp.Visible = false;
            btnShowScore.Visible = false;
            textBox1.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            timer1.Interval = 20;    // Eğer zor mod seçilirse zamanı hızlandırıyoruz.
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Zamanı hızlandırdığımız için Süre de hızlanacaktır. Bunun için böyle bir mantık yaptım.
            int carpan = 10;
            if (second == carpan * 60)
            {
                second = 0;
                minute++;
            }
            lblMinute.Text = string.Format("{0:00}", minute);
            lblSecond.Text = string.Format("{0:00}", second / 10);
            second++;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            ProcessStartInfo ps = new ProcessStartInfo("C:\\Users\\YAAZ\\Desktop\\Yilan\\Yilan\\bin\\Debug\\SCORE.txt");
            ps.UseShellExecute = false;
            ps.RedirectStandardInput = true;

            p.StartInfo = ps;
            Process.Start("C:\\Users\\YAAZ\\Desktop\\Yilan\\Yilan\\bin\\Debug\\SCORE.txt");
        }
    }
}
