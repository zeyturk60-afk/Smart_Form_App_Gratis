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
    public partial class Form5 : Form
    {
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";
        Color gratisMor = Color.FromArgb(74, 20, 140);
        Color anaPembe = Color.FromArgb(236, 0, 140);
        Color arkaPlan = Color.FromArgb(254, 252, 243);

        private Label lblBaslik;
        private Label lblMesaj;
        private PictureBox picSepetIkon;
        private Button btnAlisveriseBasla;
        private Panel pnlAltMenu;

        public Form5()
        {
            InitializeComponent();
          
    this.AutoScroll = true;
            // İçerik bittiğinde en altta biraz boşluk kalmasını sağlar (isteğe bağlı)
            this.AutoScrollMinSize = new Size(0, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            TasarimiOlustur();


            this.Size = new Size(400, 700);
            this.Text = "Sepetim";
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void TasarimiOlustur()
        {
            // Başlık
            lblBaslik = new Label();
            lblBaslik.Text = "Sepetim";
            lblBaslik.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblBaslik.ForeColor = gratisMor;
            lblBaslik.Size = new Size(400, 50);
            lblBaslik.Location = new Point(0, 10);
            lblBaslik.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblBaslik);

            // Sepet ürünlerini göster
            SepetUrunleriniGoster();

            // Alt menüyü ekle
            AltMenuEkle();
        }

        private void SepetUrunleriniGoster()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Sepetteki ürün sayısını kontrol et
                    SqlCommand countCmd = new SqlCommand("SELECT COUNT(*) FROM Sepet", conn);
                    int urunSayisi = 0;
                    try
                    {
                        urunSayisi = Convert.ToInt32(countCmd.ExecuteScalar());
                    }
                    catch
                    {
                        BosSepetiGoster();
                        return;
                    }

                    if (urunSayisi == 0)
                    {
                        // Boş sepet göster
                        BosSepetiGoster();
                        return;
                    }

                    // Sepet ürünlerini çek
                    FlowLayoutPanel flpSepet = new FlowLayoutPanel
                    {
                        Size = new Size(380, 450),
                        Location = new Point(10, 70),
                        FlowDirection = FlowDirection.TopDown,
                        WrapContents = false,
                        AutoScroll = true,
                        BackColor = Color.White
                    };

                    SqlCommand cmd = new SqlCommand("SELECT UrunAd, UrunFiyat, Adet, UrunResim, SepetID FROM Sepet ORDER BY EklenmeTarihi DESC", conn); 
                    SqlDataReader dr = cmd.ExecuteReader();

                    decimal toplamFiyat = 0;

                    while (dr.Read())
                    {
                        string urunAd = dr["UrunAd"].ToString();
                        decimal urunFiyat = Convert.ToDecimal(dr["UrunFiyat"]);
                        int adet = Convert.ToInt32(dr["Adet"]);
                        string resimYolu = dr["UrunResim"].ToString();
                        int sepetID = Convert.ToInt32(dr["SepetID"]);

                        toplamFiyat += urunFiyat * adet;

                        // Her ürün için panel
                        Panel pnlUrun = new Panel
                        {
                            Size = new Size(360, 100),
                            BackColor = Color.FromArgb(250, 250, 250),
                            Margin = new Padding(5),
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        // Ürün resmi
                        PictureBox pbUrun = new PictureBox
                        {
                            Size = new Size(80, 80),
                            Location = new Point(10, 10),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            ImageLocation = resimYolu,
                            BackColor = Color.White
                        };

                        // Ürün adı
                        Label lblUrunAd = new Label
                        {
                            Text = urunAd,
                            Location = new Point(100, 15),
                            Size = new Size(180, 40),
                            Font = new Font("Segoe UI", 10, FontStyle.Bold)
                        };

                        // Fiyat ve adet
                        Label lblFiyat = new Label
                        {
                            Text = $"{urunFiyat:0.00} TL x {adet}",
                            Location = new Point(100, 55),
                            Size = new Size(150, 20),
                            Font = new Font("Segoe UI", 9),
                            ForeColor = Color.DarkGreen
                        };

                        // Toplam fiyat
                        Label lblToplamUrun = new Label
                        {
                            Text = $"{(urunFiyat * adet):0.00} TL",
                            Location = new Point(100, 75),
                            Size = new Size(150, 20),
                            Font = new Font("Segoe UI", 10, FontStyle.Bold),
                            ForeColor = anaPembe
                        };

                        // Sil butonu
                        Button btnSil = new Button
                        {
                            Text = "🗑",
                            Size = new Size(40, 40),
                            Location = new Point(310, 30),
                            BackColor = Color.FromArgb(255, 100, 100),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat,
                            Font = new Font("Segoe UI", 14),
                            Tag = sepetID
                        };
                        btnSil.FlatAppearance.BorderSize = 0;
                        btnSil.Click += BtnSil_Click;

                        pnlUrun.Controls.Add(pbUrun);
                        pnlUrun.Controls.Add(lblUrunAd);
                        pnlUrun.Controls.Add(lblFiyat);
                        pnlUrun.Controls.Add(lblToplamUrun);
                        pnlUrun.Controls.Add(btnSil);
                        flpSepet.Controls.Add(pnlUrun);
                    }

                    dr.Close();
                    this.Controls.Add(flpSepet);

                    // Toplam fiyat paneli
                    Panel pnlToplam = new Panel
                    {
                        Size = new Size(380, 80),
                        Location = new Point(10, 530),
                        BackColor = gratisMor,
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    Label lblToplamBaslik = new Label
                    {
                        Text = "TOPLAM:",
                        Location = new Point(20, 20),
                        Size = new Size(150, 40),
                        Font = new Font("Segoe UI", 14, FontStyle.Bold),
                        ForeColor = Color.White
                    };

                    Label lblToplamTutar = new Label
                    {
                        Text = $"{toplamFiyat:0.00} TL",
                        Location = new Point(200, 20),
                        Size = new Size(160, 40),
                        Font = new Font("Segoe UI", 16, FontStyle.Bold),
                        ForeColor = Color.White,
                        TextAlign = ContentAlignment.MiddleRight
                    };

                    pnlToplam.Controls.Add(lblToplamBaslik);
                    pnlToplam.Controls.Add(lblToplamTutar);
                    this.Controls.Add(pnlToplam);

                    pnlToplam.Controls.Add(lblToplamBaslik);
                    pnlToplam.Controls.Add(lblToplamTutar);
                    this.Controls.Add(pnlToplam);
                }
            }

            catch (Exception ex)

            {
                MessageBox.Show("Sepet yüklenirken hata: " + ex.Message);
            }
        }

        private void BosSepetiGoster()
        {
            picSepetIkon = new PictureBox();
            picSepetIkon.Size = new Size(250, 200);
            picSepetIkon.Location = new Point(75, 120);
            picSepetIkon.SizeMode = PictureBoxSizeMode.Zoom;
            picSepetIkon.BackColor = Color.FromArgb(255, 240, 245);
            this.Controls.Add(picSepetIkon);

            SqlDenResmiYukle();

            lblMesaj = new Label();
            lblMesaj.Text = "Sepetinizde henüz ürün bulunmuyor.\nÜrünleri keşfedin ve dilediklerinizi\nsepetinize ekleyin.";
            lblMesaj.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            lblMesaj.ForeColor = Color.DimGray;
            lblMesaj.Size = new Size(350, 80);
            lblMesaj.Location = new Point(25, 330);
            lblMesaj.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblMesaj);

            btnAlisveriseBasla = new Button();
            btnAlisveriseBasla.Text = "ALIŞVERİŞE BAŞLA";
            btnAlisveriseBasla.Size = new Size(340, 55);
            btnAlisveriseBasla.Location = new Point(30, 430);
            btnAlisveriseBasla.BackColor = anaPembe;
            btnAlisveriseBasla.ForeColor = Color.White;
            btnAlisveriseBasla.FlatStyle = FlatStyle.Flat;
            btnAlisveriseBasla.FlatAppearance.BorderSize = 0;
            btnAlisveriseBasla.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnAlisveriseBasla.Cursor = Cursors.Hand;
            btnAlisveriseBasla.Click += BtnAlisveriseBasla_Click;
            this.Controls.Add(btnAlisveriseBasla);
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int sepetID = Convert.ToInt32(btn.Tag);

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Sepet WHERE SepetID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", sepetID);
                    cmd.ExecuteNonQuery();
                }

                // Sayfayı yenile
                this.Controls.Clear();
                TasarimiOlustur();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme hatası: " + ex.Message);
            }
        }

        private void SqlDenResmiYukle()
        {
            try
            {
                using (SqlConnection baglanti = new SqlConnection(connString))
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("SELECT IkonYolu FROM AltMenu WHERE Sira = 6", baglanti);
                    object sonuc = komut.ExecuteScalar();

                    if (sonuc != null)
                    {
                        string dbYolu = sonuc.ToString().Trim();
                        string tamYol = Path.Combine(Application.StartupPath, dbYolu);
                        picSepetIkon.ImageLocation = tamYol;
                        picSepetIkon.BringToFront();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Resim Hatası: " + ex.Message);
            }
        }

        private void BtnAlisveriseBasla_Click(object sender, EventArgs e)
        {
            try
            {
                tumurunler fr = new tumurunler();
                fr.Show();
                this.Hide();
            }
            catch (Exception)
            {
                MessageBox.Show("Hata: 'tumurunler.cs' bulunamadı.");
            }
        }

        private void AltMenuEkle()
        {
            Panel pnlAltNav = new Panel
            {
                Size = new Size(400, 80), // Genişliği formuna göre 400 yaptım
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

                    int butonGenislik = 400 / 5;
                    int i = 0;

                    while (oku.Read())
                    {
                        string mAd = oku["MenuAd"].ToString();
                        string iYolu = Path.Combine(Application.StartupPath, oku["IkonYolu"].ToString());

                        Panel pnlButon = new Panel { Size = new Size(butonGenislik, 80), Location = new Point(i * butonGenislik, 0), Cursor = Cursors.Hand, Tag = mAd };
                        PictureBox pb = new PictureBox { ImageLocation = iYolu, Size = new Size(28, 28), Location = new Point((butonGenislik - 28) / 2, 12), SizeMode = PictureBoxSizeMode.Zoom, Enabled = false };
                        Label lbl = new Label { Text = mAd, TextAlign = ContentAlignment.BottomCenter, Dock = DockStyle.Bottom, Height = 30, Enabled = false, Font = new Font("Segoe UI", 8) };

                        pnlButon.Click += (s, ev) =>
                        {
                            string tag = ((Panel)s).Tag.ToString();
                            if (tag == "Anasayfa") { new Form2().Show(); this.Hide(); }
                            else if (tag == "Kategoriler") { new Form4().Show(); this.Hide(); }
                            else if (tag == "Sepet") { new Form5().Show(); this.Hide(); }
                            else if (tag == "Favoriler") { new Form6().Show(); this.Hide(); }
                            else if (tag == "Profil") { new Form7().Show(); this.Hide(); }
                        };

                        pnlButon.Controls.Add(pb);
                        pnlButon.Controls.Add(lbl);
                        pnlAltNav.Controls.Add(pnlButon);
                        i++;
                    }
                }
            }
            catch { }

            this.Controls.Add(pnlAltNav);
            pnlAltNav.BringToFront();
        }
    }
}