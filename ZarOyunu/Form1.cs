using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ZarOyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool siraBizde, oyunBitti;
        private int zarbiz, zarpc, count, el;
        private Random rnd;
        private int[] puanbiz, puanpc;
        private string OyuncuAdi;

        private void btn_kaydet_Click(object sender, EventArgs e)
        {
            string kontrol = textBox1.Text;

            if (kontrol != "Kayıt Başarılı..." && kontrol != "Başka İsim Giriniz...")
            {
                SqlConnection baglan = new SqlConnection("Data Source=101-08; Initial Catalog=DondurGelsin; User ID=sa; Password=1234;");
                baglan.Open();
                string durum = "";
                SqlCommand OyuncuEkle = new SqlCommand("declare @Durum int exec PlayerInsert '" + textBox1.Text + "' , @Status=@Durum output select @Durum as Durum", baglan);
                SqlDataReader kayit_kontrol = OyuncuEkle.ExecuteReader();
                while (kayit_kontrol.Read()) { durum = kayit_kontrol["Durum"].ToString(); }
                if (durum == "1") { textBox1.Text = "Kayıt Başarılı..."; }
                else { textBox1.Text = "Başka İsim Giriniz..."; }
                baglan.Close();
                baglan.Dispose();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection baglan = new SqlConnection("Data Source=101-08; Initial Catalog=DondurGelsin; User ID=sa; Password=1234;");
            baglan.Open();
            string durum = "";
            SqlCommand OyuncuEkle = new SqlCommand("select dbo.PlayerControl('" + textBox2.Text + "') as Durum", baglan);
            SqlDataReader kayit_kontrol = OyuncuEkle.ExecuteReader();
            while (kayit_kontrol.Read()) { durum = kayit_kontrol["Durum"].ToString(); }
            if (durum == "1") { OyunAc(); }
            else { label3.Text = "Oyuncu Bulunamadı..."; }
            baglan.Close();
            baglan.Dispose();
        }

        private void OyunAc()
        {
            OyuncuAdi = textBox2.Text;
            oyunBitti = false; this.Width = 727;
            pnl_kayit.Visible = false; panel1.Visible = false;
            panel2.Visible = true; siraBizde = true;
            rnd = new Random(); puanbiz = new int[5];
            puanpc = new int[5]; lblEl.Text = (el + 1).ToString() + ". El";
        }
        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //pictureBox1.Load("http://thumbs.dreamstime.com/t/dice-cube-wooden-texture-background-49192655.jpg");
            //pictureBox1.Load("C:\\Users\\selcuk.alan\\Desktop\\zar6.png");
            if (!oyunBitti)
            {
                timer1.Enabled = true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            ZarDon();
            count++;
            if (count == 20)
            {
                if (siraBizde)
                {
                    siraBizde = false;
                }
                else
                {
                    timer1.Enabled = false;
                    siraBizde = true;
                    //****
                    PuanAl();
                }
                count = 0;
            }
        }

        private void PuanGiris()
        {
            int toplambiz = 0, toplampc = 0, i = 0;

            while (i < 5)
            {
                toplambiz += puanbiz[i];
                toplampc += puanpc[i];
                i++;
            }

            SqlConnection baglan = new SqlConnection("Data Source=101-08; Initial Catalog=DondurGelsin; User ID=sa; Password=1234;");
            baglan.Open(); string durum = "";
            SqlCommand OyuncuEkle = new SqlCommand("declare @date datetime = getdate() exec ScoreBoardInsert '" 
                + OyuncuAdi + "',@date,"+ toplambiz + "," + toplampc, baglan);
            SqlDataReader kayit_kontrol = OyuncuEkle.ExecuteReader();
            //while (kayit_kontrol.Read()) { durum = kayit_kontrol["Durum"].ToString(); }
            //if (durum == "1") { OyunAc(); }
            //else { label3.Text = "Oyuncu Bulunamadı..."; }
            baglan.Close();
            baglan.Dispose();
        }
        private void PuanAl()
        {
            if (oyunBitti) return;
            if (zarbiz != zarpc && el < 5)
            {
                puanbiz[el] = zarbiz;
                puanpc[el] = zarpc;
                el++;
            }
            PuanScala();
            if (el == 5) oyunBitti = true;
            if (zarbiz == zarpc) { lblEl.Text = "Zarlar Eşit"; return; }
            if (!oyunBitti) lblEl.Text = (el + 1).ToString() + ". El";
            else { lblEl.Text = "Oyun Bitti"; PuanGiris(); }
        }

        private void PuanScala()
        {
            ZarGoster(pictureZar1, puanbiz[0]);
            ZarGoster(picture_Zar1, puanpc[0]);
            if (el == 0) return;
            ZarGoster(pictureZar2, puanbiz[1]);
            ZarGoster(picture_Zar2, puanpc[1]);
            if (el == 1) return;
            ZarGoster(pictureZar3, puanbiz[2]);
            ZarGoster(picture_Zar3, puanpc[2]);
            if (el == 2) return;
            ZarGoster(pictureZar4, puanbiz[3]);
            ZarGoster(picture_Zar4, puanpc[3]);
            if (el == 3) return;
            ZarGoster(pictureZar5, puanbiz[4]);
            ZarGoster(picture_Zar5, puanpc[4]);
            if (el == 4) return;
        }

        private void ZarDon()
        {
            if (siraBizde)
            {
                zarbiz = rnd.Next(1, 7);
                switch (zarbiz)
                {
                    case 1: pictureBox1.Image = Properties.Resources.zar1; break;
                    case 2: pictureBox1.Image = Properties.Resources.zar2; break;
                    case 3: pictureBox1.Image = Properties.Resources.zar3; break;
                    case 4: pictureBox1.Image = Properties.Resources.zar4; break;
                    case 5: pictureBox1.Image = Properties.Resources.zar5; break;
                    case 6: pictureBox1.Image = Properties.Resources.zar6; break;
                }
            }
            else
            {
                zarpc = rnd.Next(1, 7);
                switch (zarpc)
                {
                    case 1: pictureBox2.Image = Properties.Resources.zar1; break;
                    case 2: pictureBox2.Image = Properties.Resources.zar2; break;
                    case 3: pictureBox2.Image = Properties.Resources.zar3; break;
                    case 4: pictureBox2.Image = Properties.Resources.zar4; break;
                    case 5: pictureBox2.Image = Properties.Resources.zar5; break;
                    case 6: pictureBox2.Image = Properties.Resources.zar6; break;
                }
            }
        }

        private void ZarGoster(PictureBox pic, int zar)
        {
            switch (zar)
            {
                case 1: pic.Image = Properties.Resources.zar1; break;
                case 2: pic.Image = Properties.Resources.zar2; break;
                case 3: pic.Image = Properties.Resources.zar3; break;
                case 4: pic.Image = Properties.Resources.zar4; break;
                case 5: pic.Image = Properties.Resources.zar5; break;
                case 6: pic.Image = Properties.Resources.zar6; break;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}