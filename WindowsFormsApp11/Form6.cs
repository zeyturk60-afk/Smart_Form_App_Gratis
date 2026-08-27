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
    public partial class Form6 : Form
    {
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";
        Color gratisMor = Color.FromArgb(74, 20, 140);
        Color anaPembe = Color.FromArgb(236, 0, 140);
        Color arkaPlan = Color.FromArgb(254, 252, 243);

        public Form6()
        {
            InitializeComponent();
            this.AutoScroll = true;
            this.Size = new Size(400, 700);
            // İçerik bittiğinde en altta biraz boşluk kalmasını sağlar (isteğe bağlı)
            this.AutoScrollMinSize = new Size(0, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            AltMenuEkle();
            this.Size = new Size(400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            TasarimiOlustur();
        }
                private void TasarimiOlustur()
        {
            Label lblBaslik = new Label();
            lblBaslik.Text = "Favorilerim";
            lblBaslik.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblBaslik.ForeColor = gratisMor;
            lblBaslik.Size = new Size(400, 50);
            lblBaslik.Location = new Point(0, 10);
            lblBaslik.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblBaslik);
            FavoriUrunleriniGoster();
        }


        private void FavoriUrunleriniGoster()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    
                    SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM Favoriler", conn);
                    int urunSayisi = 0;
                    
                    try
                    {
                        urunSayisi = Convert.ToInt32(countCmd.ExecuteScalar());
                    }
                    catch
                    {
                        BosFavorileriGoster();
                        return;
                    }

                    if (urunSayisi == 0)
                    {
                        BosFavorileriGoster();
                        return;
                    }

                    FlowLayoutPanel flpFavoriler = new FlowLayoutPanel
                    {
                        Size = new Size(380, 520),
                        Location = new Point(10, 70),
                        FlowDirection = FlowDirection.TopDown,
                        WrapContents = false,
                        AutoScroll = true,
                        BackColor = Color.White
                    };

                    SqlCommand cmd = new SqlCommand("SELECT * FROM Favoriler ORDER BY EklenmeTarihi DESC", conn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string urunAd = dr["UrunAd"].ToString();
                        decimal urunFiyat = Convert.ToDecimal(dr["UrunFiyat"]);
                        string resimYolu = dr["UrunResim"].ToString();
                        int favoriID = Convert.ToInt32(dr["FavoriID"]);

                        Panel pnlUrun = new Panel
                        {
                            Size = new Size(360, 100),
                            BackColor = Color.FromArgb(255, 240, 245),
                            Margin = new Padding(5),
                            BorderStyle = BorderStyle.FixedSingle,
                            Cursor = Cursors.Hand
                        };

                        PictureBox pbUrun = new PictureBox
                        {
                            Size = new Size(80, 80),
                            Location = new Point(10, 10),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            ImageLocation = resimYolu,
                            BackColor = Color.White,
                            Cursor = Cursors.Hand
                        };

                        Label lblUrunAd = new Label
                        {
                            Text = urunAd,
                            Location = new Point(100, 20),
                            Size = new Size(180, 40),
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            Cursor = Cursors.Hand
                        };

                        Label lblFiyat = new Label
                        {
                            Text = $"{urunFiyat:0.00} TL",
                            Location = new Point(100, 60),
                            Size = new Size(150, 20),
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            ForeColor = anaPembe,
                            Cursor = Cursors.Hand
                        };

                        Button btnSil = new Button
                        {
                            Text = "sl",
                            Size = new Size(40, 40),
                            Location = new Point(310, 30),
                            BackColor = Color.FromArgb(255, 100, 150),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat,
                            Font = new Font("Segoe UI", 18),
                            Tag = favoriID
                        };
                        btnSil.FlatAppearance.BorderSize = 0;
                        btnSil.Click += BtnFavoriSil_Click;

                        EventHandler detayGit = (s, e) =>
                        {
                            FormUrunDetay detay = new FormUrunDetay(urunAd, urunFiyat.ToString(), resimYolu);
                            detay.ShowDialog();
                        };

                        pnlUrun.Click += detayGit;
                        pbUrun.Click += detayGit;
                        lblUrunAd.Click += detayGit;
                        lblFiyat.Click += detayGit;

                        pnlUrun.Controls.Add(pbUrun);
                        pnlUrun.Controls.Add(lblUrunAd);
                        pnlUrun.Controls.Add(lblFiyat);
                        pnlUrun.Controls.Add(btnSil);
                        flpFavoriler.Controls.Add(pnlUrun);
                    }

                    dr.Close();
                    this.Controls.Add(flpFavoriler);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Favoriler yüklenirken hata: " + ex.Message);
                BosFavorileriGoster();
            }
        }

        private void BosFavorileriGoster()
        {
            Label lblKalp = new Label();
            lblKalp.Text = ">";
            lblKalp.Font = new Font("Segoe UI", 60);
            lblKalp.ForeColor = anaPembe;
            lblKalp.Location = new Point(150, 150);
            lblKalp.AutoSize = true;
            this.Controls.Add(lblKalp);

            Label lblMesaj = new Label();
            lblMesaj.Text = "Henüz favori ürününüz yok.\nBğendiğiniz ürünleri favorilere ekleyin!";
            lblMesaj.Font = new Font("Segoe UI", 11);
            lblMesaj.ForeColor = Color.DimGray;
            lblMesaj.Size = new Size(350, 60);
            lblMesaj.Location = new Point(25, 280);
            lblMesaj.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblMesaj);

            Button btnYeniListe = new Button();
            btnYeniListe.Text = "ÜRÜNLERE GÖZ AT";
            btnYeniListe.Size = new Size(340, 55);
            btnYeniListe.Location = new Point(30, 430);
            btnYeniListe.BackColor = anaPembe;
            btnYeniListe.ForeColor = Color.White;
            btnYeniListe.FlatStyle = FlatStyle.Flat;
            btnYeniListe.FlatAppearance.BorderSize = 0;
            btnYeniListe.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnYeniListe.Cursor = Cursors.Hand;
            btnYeniListe.Click += BtnYeniListe_Click;
            this.Controls.Add(btnYeniListe);
        }

        private void BtnFavoriSil_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int favoriID = Convert.ToInt32(btn.Tag);

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Favoriler WHERE FavoriID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", favoriID);
                    cmd.ExecuteNonQuery();
                }

                this.Controls.Clear();
                TasarimiOlustur();
                AltMenuEkle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme hatasÄ±: " + ex.Message);
            }
        }

        private void BtnYeniListe_Click(object sender, EventArgs e)
        {
            // tumurunler.cs formunu açıyoruz
            tumurunler urunler = new tumurunler();
            urunler.Show();
            this.Hide();
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
    




