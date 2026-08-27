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
    public partial class Form26 : Form
    {
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";

        public Form26()
        {
            InitializeComponent();
            this.Size = new Size(400, 700);
            this.Text = "Gratis | Pembe Dünya";
            this.StartPosition = FormStartPosition.CenterScreen;
            ResimPanelleriEkle();
            AltMenuEkle();
        }

                                private void ResimPanelleriEkle()
        {
            // 1. Ana Taşıyıcı Paneli (FlowLayoutPanel) oluşturuyoruz
            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Size = new Size(385, 550),
                Location = new Point(5, 10),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true
            };

            // 2. SQL Bağlantı Bilgileri (Server kısmına nokta (.) koyarak yerel sunucuya bağlanıyoruz)
            // Database kısmına kendi veritabanı adını yazmalısın.
            string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";
            string query = "SELECT TumAd, TumFiyat, TumYolu FROM Tumurunler ORDER BY TumID ASC OFFSET 54 ROWS FETCH NEXT 3 ROWS ONLY";

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string uAd = dr["TumAd"].ToString();
                        string uFiyat = dr["TumFiyat"].ToString();
                        string uYol = dr["TumYolu"].ToString();

                        // Her ürün için bir dış panel
                        Panel p = new Panel
                        {
                            Size = new Size(155, 230),
                            BackColor = Color.White,
                            Margin = new Padding(5),
                            BorderStyle = BorderStyle.FixedSingle,
                            Cursor = Cursors.Hand
                        };

                        // Ürün Resmi
                        PictureBox pb = new PictureBox
                        {
                            Size = new Size(135, 135),
                            Location = new Point(10, 10),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            ImageLocation = uYol,
                            Cursor = Cursors.Hand
                        };

                        // Ürün Adı Etiketi
                        Label lblAd = new Label
                        {
                            Text = uAd,
                            Location = new Point(5, 155),
                            Width = 145,
                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                            TextAlign = ContentAlignment.TopCenter,
                            Cursor = Cursors.Hand
                        };

                        // Ürün Fiyat Etiketi
                        Label lblFiyat = new Label
                        {
                            Text = String.Format("{0:0.00} TL", Convert.ToDecimal(uFiyat)),
                            Location = new Point(5, 195),
                            Width = 145,
                            ForeColor = Color.DarkGreen,
                            TextAlign = ContentAlignment.TopCenter,
                            Cursor = Cursors.Hand
                        };

                        // Tıklama Olayı (Hepsine aynı olayı veriyoruz)
                        EventHandler detayAc = (s, e) =>
                        {
                            FormUrunDetay detay = new FormUrunDetay(uAd, uFiyat, uYol);
                            detay.ShowDialog();
                        };

                        p.Click += detayAc;
                        pb.Click += detayAc;
                        lblAd.Click += detayAc;
                        lblFiyat.Click += detayAc;

                        // Kontrolleri panele, paneli de ana listeye ekliyoruz
                        p.Controls.Add(pb);
                        p.Controls.Add(lblAd);
                        p.Controls.Add(lblFiyat);
                        flp.Controls.Add(p);
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }

            // Form üzerine FlowLayoutPanel'i ekliyoruz
            this.Controls.Add(flp);
            flp.BringToFront();
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





