using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    public partial class Form7 : Form
    {
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";
        Color gratisMor = Color.FromArgb(74, 20, 140);
        Color anaPembe = Color.FromArgb(236, 0, 140);
        Color arkaPlan = Color.FromArgb(254, 252, 243);
        Color tozPembe = Color.FromArgb(255, 240, 245); // LavenderBlush (Toz Pembe)
       
        public Form7()
        {
            InitializeComponent();
            this.AutoScroll = true;
            // İçerik bittiğinde en altta biraz boşluk kalmasını sağlar (isteğe bağlı)
            this.AutoScrollMinSize = new Size(0, 800);
            this.Size = new Size(400, 700); // İçerik çok olduğu için boyutu biraz uzattım
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = tozPembe; // Arka plan toz pembe
            this.AutoScroll = true;    // İçerik taşarsa kaydırma çubuğu çıksın
            TasarimiOlustur();
        }

        private void TasarimiOlustur()

        {
            // 1. Profil Başlığı
            Label lblBaslik = new Label();
            lblBaslik.Text = "Profil";
            lblBaslik.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblBaslik.ForeColor = anaPembe;
            lblBaslik.Size = new Size(400, 40);
            lblBaslik.Location = new Point(0, 10);
            lblBaslik.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblBaslik);

            // 2. Kullanıcı Bilgi Kartı (ZT - Zeynep Türk)
            Panel pnlUser = new Panel();
            pnlUser.Size = new Size(360, 90);
            pnlUser.Location = new Point(20, 60);
            pnlUser.BackColor = Color.White;
            this.Controls.Add(pnlUser);

            // ZT Logosu
            Label lblLogo = new Label();
            lblLogo.Text = "ZT";
            lblLogo.Size = new Size(60, 60);
            lblLogo.Location = new Point(15, 15);
            lblLogo.BackColor = anaPembe;
            lblLogo.ForeColor = Color.White;
            lblLogo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            pnlUser.Controls.Add(lblLogo);

            // İsim
            Label lblIsim = new Label();
            lblIsim.Text = "Zeynep Türk";
            lblIsim.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblIsim.Location = new Point(85, 25);
            lblIsim.AutoSize = true;
            pnlUser.Controls.Add(lblIsim);

            // 3. Menü Butonları Izgarası (Grid Sistemi)
            string[] menuIsimleri = { "Siparişlerim", "Favorilerim", "Adreslerim", "Gratis Kartım", "Kampanyalarım", "Tekrar Satın Al", "Puanlarım", "Hesabım" };
            int baslangicX = 20;
            int baslangicY = 170;

            for (int i = 0; i < menuIsimleri.Length; i++)
            {
                string menuAd = menuIsimleri[i];
                Button btn = new Button();
                btn.Text = menuAd;
                btn.Size = new Size(175, 50);
                btn.BackColor = Color.White;
                btn.ForeColor = Color.DimGray;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderColor = anaPembe; // Kenarlıklar pembe
                btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                btn.Tag = menuAd;

                // 2 sütunlu yerleşim hesabı
                int satir = i / 2;
                int sutun = i % 2;
                btn.Location = new Point(baslangicX + (sutun * 185), baslangicY + (satir * 60));

                btn.Click += (s, e) =>
                {
                    string tag = ((Button)s).Tag.ToString();
                    Form hedefForm = null;

                    if (tag == "Siparişlerim") hedefForm = new Form5();
                    else if (tag == "Favorilerim") hedefForm = new Form6();
                    else if (tag == "Adreslerim") hedefForm = new Form31();
                    else if (tag == "Gratis Kartım") hedefForm = new Form32();
                    else if (tag == "Kampanyalarım") hedefForm = new Form2();

                    if (hedefForm != null)
                    {
                        hedefForm.Show();
                        this.Hide();
                    }
                };

                this.Controls.Add(btn);
            }

            // 4. Sosyal Medya Takip Yazısı
            Label lblSosyal = new Label();
            lblSosyal.Text = "BİZİ SOSYAL MEDYADA TAKİP EDİN";
            lblSosyal.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSosyal.ForeColor = Color.DimGray;
            lblSosyal.Location = new Point(20, 450);
            lblSosyal.AutoSize = true;
            this.Controls.Add(lblSosyal);

            // Alt Menüyü Ekle (Zaten sende olan metot)
            // AltMenuEkle(); 

            AltMenuEkle();
        
        }
            private void AltMenuEkle()
        {
            Panel pnlAltNav = new Panel
            {
                Size = new Size(500, 80), // Form genişliğine göre ayarla
                Dock = DockStyle.Bottom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            try
            {
                using (SqlConnection baglanti = new SqlConnection(connString))
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("SELECT MenuAd, IkonYolu FROM AltMenu ORDER BY Sira", baglanti);
                    SqlDataReader oku = komut.ExecuteReader();

                    int butonGenislik = 500 / 5; // 5 buton olduğu için
                    int i = 0;

                    while (oku.Read())
                    {
                        string mAd = oku["MenuAd"].ToString();
                        string iYolu = Path.Combine(Application.StartupPath, oku["IkonYolu"].ToString());

                        Panel pnlButon = new Panel { Size = new Size(butonGenislik, 80), Location = new Point(i * butonGenislik, 0), Cursor = Cursors.Hand, Tag = mAd };
                        PictureBox pb = new PictureBox { ImageLocation = iYolu, Size = new Size(28, 28), Location = new Point((butonGenislik - 28) / 2, 12), SizeMode = PictureBoxSizeMode.Zoom, Enabled = false };
                        Label lbl = new Label { Text = mAd, TextAlign = ContentAlignment.BottomCenter, Dock = DockStyle.Bottom, Height = 30, Enabled = false, Font = new Font("Segoe UI", 8) };

                        // TIKLAMA OLAYI
                        pnlButon.Click += (s, ev) =>
                        {
                            string tag = ((Panel)s).Tag.ToString();
                            Form yeniForm = null;

                            if (tag == "Anasayfa") yeniForm = new Form2();
                            else if (tag == "Kategoriler") yeniForm = new Form4();
                            else if (tag == "Sepet") yeniForm = new Form5();
                            else if (tag == "Favoriler") yeniForm = new Form6();
                            else if (tag == "Profil") yeniForm = new Form7();

                            if (yeniForm != null)
                            {
                                yeniForm.Show();
                                this.Hide(); // Mevcut formu gizle
                            }
                        };

                        pnlButon.Controls.Add(pb);
                        pnlButon.Controls.Add(lbl);
                        pnlAltNav.Controls.Add(pnlButon);
                        i++;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }

            this.Controls.Add(pnlAltNav);
            pnlAltNav.BringToFront();

            }
    }
}


    


