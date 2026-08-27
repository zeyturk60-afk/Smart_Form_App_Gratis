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
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    public partial class FormUrunDetay : Form
    {
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";
        int _secilenID;
        Color gratisMor = Color.FromArgb(74, 20, 140);
        Color gratisPembe = Color.FromArgb(236, 0, 140);

        public FormUrunDetay(int gelenID)
        {
            InitializeComponent();
            this._secilenID = gelenID;
            this.Size = new Size(460, 950);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.AutoScroll = true;
            TasarimiOlustur();
        }

        // Yeni Constructor: Direkt veri ile çalışır
        public FormUrunDetay(string ad, string fiyat, string resimYolu)
        {
            InitializeComponent();
            this.Size = new Size(460, 950);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.AutoScroll = true;
            
            // Direkt verilen verilerle tasarımı oluştur
            ManuelTasarimOlustur(ad, fiyat, resimYolu);
        }

        private void TasarimiOlustur()
        {
            try
            {
                using (SqlConnection baglanti = new SqlConnection(connString))
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("SELECT * FROM Tumurunler WHERE TumID = @id", baglanti);
                    komut.Parameters.AddWithValue("@id", _secilenID);
                    SqlDataReader oku = komut.ExecuteReader();

                    if (oku.Read())
                    {
                        // 1. Resim ve Temel Bilgiler
                        PictureBox pb = new PictureBox
                        {
                            ImageLocation = Path.Combine(Application.StartupPath, oku["TumYolu"].ToString()),
                            Size = new Size(400, 300),
                            Location = new Point(25, 10),
                            SizeMode = PictureBoxSizeMode.Zoom
                        };
                        this.Controls.Add(pb);

                        Label lblAd = new Label
                        {
                            Text = oku["TumAd"].ToString(),
                            Font = new Font("Segoe UI", 14, FontStyle.Bold),
                            Location = new Point(20, 320),
                            Size = new Size(400, 50)
                        };
                        this.Controls.Add(lblAd);

                        // ... (özellikler, yorumlar, iade kısımları aynı kalabilir veya güncellenebilir) ...

                        // 5. Fiyat ve Satın Al Barı
                        Panel pnlAlt = new Panel { Size = new Size(460, 100), Dock = DockStyle.Bottom, BackColor = Color.FromArgb(250, 250, 250), BorderStyle = BorderStyle.FixedSingle };

                        Label lblFiyat = new Label
                        {
                            Text = oku["TumFiyat"].ToString() + " TL",
                            Font = new Font("Segoe UI", 15, FontStyle.Bold),
                            ForeColor = gratisPembe,
                            Location = new Point(10, 30),
                            AutoSize = true
                        };

                        // Verileri al
                        string urunAd = oku["TumAd"].ToString();
                        string urunFiyat = oku["TumFiyat"].ToString();
                        string urunResim = oku["TumYolu"].ToString();

                        Button btnSepet = new Button { Text = "SEPETE EKLE", Size = new Size(150, 50), Location = new Point(210, 20), BackColor = gratisMor, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
                        btnSepet.Click += (s, ev) => SepeteEkle(urunAd, urunFiyat, urunResim);

                        Button btnFavori = new Button { Text = "♥", Size = new Size(50, 50), Location = new Point(370, 20), BackColor = Color.FromArgb(255, 100, 150), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 20) };
                        btnFavori.Click += (s, ev) => FavorilereEkle(urunAd, urunFiyat, urunResim);

                        pnlAlt.Controls.Add(lblFiyat);
                        pnlAlt.Controls.Add(btnSepet);
                        pnlAlt.Controls.Add(btnFavori);
                        this.Controls.Add(pnlAlt);
                        pnlAlt.BringToFront();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void ManuelTasarimOlustur(string ad, string fiyat, string resimYolu)
        {
             // 1. Resim ve Temel Bilgiler
            PictureBox pb = new PictureBox
            {
                ImageLocation = resimYolu,
                Size = new Size(400, 300),
                Location = new Point(25, 10),
                SizeMode = PictureBoxSizeMode.Zoom
            };
            this.Controls.Add(pb);

            Label lblAd = new Label
            {
                Text = ad,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 320),
                Size = new Size(400, 50)
            };
            this.Controls.Add(lblAd);

            // 2. Ürün Özellikleri Bölümü (Dummy)
            BilgiBolumuEkle("ÜRÜN ÖZELLİKLERİ",
                "• Tüm cilt tipleri için uygundur.\n" +
                "• Gün boyu kalıcılık ve taze görünüm sağlar.\n" +
                "• Paraben ve alkol içermez, dermatolojik test edilmiştir.", 380);

            // 3. Yorumlar Bölümü (Dinamik)
            Panel pnlYorumlar = YorumBolumunuOlustur(ad, 480);
            this.Controls.Add(pnlYorumlar);

            // 4. İade Koşulları Bölümü
            // Yorum bolumu yoruma göre alta doğru kayabilir, o yüzden dinamik bir Y konumu hesaplayalım.
            int iadeYKonumu = pnlYorumlar.Bottom + 10;
            BilgiBolumuEkle("İADE KOŞULLARI",
                "• 14 gün içerisinde ücretsiz iade hakkı.\n" +
                "• Ambalajı açılmış veya kullanılmış ürünlerde iade kabul edilmemektedir.", iadeYKonumu);

            // 5. Fiyat ve Satın Al Barı
            Panel pnlAlt = new Panel
            {
                Size = new Size(460, 100),
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblFiyat = new Label
            {
                Text = fiyat + (fiyat.Contains("TL") ? "" : " TL"),
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = gratisPembe,
                Location = new Point(10, 30),
                AutoSize = true
            };

            Button btnSepet = new Button
            {
                Text = "SEPETE EKLE",
                Size = new Size(150, 50),
                Location = new Point(210, 20),
                BackColor = gratisMor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            
            // Sepete Ekle Butonu Tıklama Olayı
            btnSepet.Click += (s, e) => SepeteEkle(ad, fiyat, resimYolu);

            // Favorilere Ekle Butonu (Kalp)
            Button btnFavori = new Button
            {
                Text = "♥",
                Size = new Size(50, 50),
                Location = new Point(370, 20),
                BackColor = Color.FromArgb(255, 100, 150),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 20)
            };
            btnFavori.FlatAppearance.BorderSize = 0;
            btnFavori.Click += (s, e) => FavorilereEkle(ad, fiyat, resimYolu);

            pnlAlt.Controls.Add(lblFiyat);
            pnlAlt.Controls.Add(btnSepet);
            pnlAlt.Controls.Add(btnFavori);
            this.Controls.Add(pnlAlt);
            pnlAlt.BringToFront();
        }

        // Tekrarlayan bölümler için yardımcı metot
        private void BilgiBolumuEkle(string baslik, string icerik, int yKonumu)
        {
            Panel pnl = new Panel
            {
                Size = new Size(410, 90),
                Location = new Point(20, yKonumu),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(252, 252, 252)
            };

            Label lblBaslik = new Label
            {
                Text = baslik,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = gratisMor,
                Location = new Point(5, 5),
                AutoSize = true
            };

            Label lblIcerik = new Label
            {
                Text = icerik,
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Location = new Point(5, 30),
                Size = new Size(390, 55)
            };

            pnl.Controls.Add(lblBaslik);
            pnl.Controls.Add(lblIcerik);
            this.Controls.Add(pnl);
        }
    
        private Panel YorumBolumunuOlustur(string urunAd, int yKonumu)
        {
            Panel pnl = new Panel
            {
                Size = new Size(410, 150),
                Location = new Point(20, yKonumu),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(252, 252, 252),
                AutoScroll = true
            };

            Label lblBaslik = new Label
            {
                Text = "ÜRÜN YORUMLARI",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = gratisMor,
                Location = new Point(5, 10),
                AutoSize = true
            };
            pnl.Controls.Add(lblBaslik);

            Button btnYorumYap = new Button
            {
                Text = "Yorum Yap",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                BackColor = gratisPembe,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(95, 25),
                Location = new Point(pnl.Width - 110, 5)
            };
            btnYorumYap.Click += (s, e) =>
            {
                Form33 f33 = new Form33(urunAd);
                f33.ShowDialog();
                YorumlariGetir(pnl, urunAd);
            };
            pnl.Controls.Add(btnYorumYap);

            // İlk Yükleme
            YorumlariGetir(pnl, urunAd);

            return pnl;
        }

        private void YorumlariGetir(Panel pnl, string urunAd)
        {
            // Paneli temizle (sadece başlık ve buton kalsın)
            for (int i = pnl.Controls.Count - 1; i >= 0; i--)
            {
                if (pnl.Controls[i] is Label && ((Label)pnl.Controls[i]).Text != "ÜRÜN YORUMLARI")
                    pnl.Controls.RemoveAt(i);
                else if (pnl.Controls[i] is Panel) // Alt panelleri (yorum blokları) temizle
                    pnl.Controls.RemoveAt(i);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    
                    // Tablo yoksa sessizce pas geçebiliriz, ilk yorum eklendiğinde oluşturulur
                    string query = "IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Yorumlar') " +
                                   "SELECT * FROM Yorumlar WHERE UrunAd = @urun ORDER BY Tarih DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@urun", urunAd);
                    
                    using (SqlDataReader oku = cmd.ExecuteReader())
                    {
                        int yPos = 40;
                        int count = 0;
                        while (oku.Read())
                        {
                            count++;
                            string yildizlar = new string('★', Convert.ToInt32(oku["Yildiz"])) + new string('☆', 5 - Convert.ToInt32(oku["Yildiz"]));
                            string userName = oku["KullaniciAdi"].ToString();
                            string date = Convert.ToDateTime(oku["Tarih"]).ToString("dd.MM.yyyy");
                            string text = oku["YorumMetni"].ToString();

                            Label lblUser = new Label
                            {
                                Text = $"{yildizlar} {userName} ({date}):",
                                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                                ForeColor = Color.DimGray,
                                Location = new Point(5, yPos),
                                AutoSize = true
                            };
                            
                            Label lblText = new Label
                            {
                                Text = text,
                                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                                ForeColor = Color.Black,
                                Location = new Point(5, yPos + 18),
                                Size = new Size(380, 0),
                                AutoSize = true,
                                MaximumSize = new Size(380, 0)
                            };

                            pnl.Controls.Add(lblUser);
                            pnl.Controls.Add(lblText);
                            yPos = lblText.Bottom + 10;
                        }

                        if (count == 0)
                        {
                            Label lblYok = new Label
                            {
                                Text = "Henüz yorum yapılmamış. İlk yorumu siz yapın!",
                                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                                ForeColor = Color.Gray,
                                Location = new Point(5, 40),
                                AutoSize = true
                            };
                            pnl.Controls.Add(lblYok);
                        }
                    }
                }
            }
            catch { }
        }

        // Sepete Ekleme Metodu
        private void SepeteEkle(string ad, string fiyat, string resimYolu)
        {
            try
            {
                // Fiyatı temizle
                string temizFiyat = fiyat.Replace(" TL", "").Replace("TL", "").Replace(",", ".").Trim();
                decimal fiyatDecimal = 0;
                
                try
                {
                    fiyatDecimal = decimal.Parse(temizFiyat, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("Fiyat formatı hatalı: " + fiyat, "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    
                    // Sepet tablosu yoksa oluştur
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sepet')
                        BEGIN
                            CREATE TABLE Sepet (
                                SepetID INT PRIMARY KEY IDENTITY(1,1),
                                UrunAd NVARCHAR(200),
                                UrunFiyat DECIMAL(10,2),
                                UrunResim NVARCHAR(MAX),
                                Adet INT DEFAULT 1,
                                EklenmeTarihi DATETIME DEFAULT GETDATE()
                            )
                        END";
                    
                    SqlCommand createCmd = new SqlCommand(createTableQuery, conn);
                    createCmd.ExecuteNonQuery();
                    
                    // Ürün sepette var mı kontrol et
                    string checkQuery = "SELECT SepetID, Adet FROM Sepet WHERE UrunAd = @ad";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@ad", ad);
                    
                    object result = checkCmd.ExecuteScalar();
                    
                    if (result != null)
                    {
                        // Ürün var, adeti artır
                        string updateQuery = "UPDATE Sepet SET Adet = Adet + 1 WHERE UrunAd = @ad";
                        SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                        updateCmd.Parameters.AddWithValue("@ad", ad);
                        updateCmd.ExecuteNonQuery();
                        
                        MessageBox.Show("Ürün sepetteki adedi artırıldı!", "Sepet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Yeni ürün ekle
                        string insertQuery = "INSERT INTO Sepet (UrunAd, UrunFiyat, UrunResim, Adet) VALUES (@ad, @fiyat, @resim, 1)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@ad", ad);
                        insertCmd.Parameters.AddWithValue("@fiyat", fiyatDecimal);
                        insertCmd.Parameters.AddWithValue("@resim", resimYolu ?? "");
                        insertCmd.ExecuteNonQuery();
                        
                        MessageBox.Show("Ürün sepete eklendi!", "Sepet", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Veritabanı hatası:\n" + sqlEx.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Beklenmeyen hata:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Favorilere Ekleme Metodu
        private void FavorilereEkle(string ad, string fiyat, string resimYolu)
        {
            try
            {
                string temizFiyat = fiyat.Replace(" TL", "").Replace("TL", "").Replace(",", ".").Trim();
                decimal fiyatDecimal = 0;
                try { fiyatDecimal = decimal.Parse(temizFiyat, System.Globalization.CultureInfo.InvariantCulture); } catch { }

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    
                    // Favoriler tablosu yoksa oluştur
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Favoriler')
                        BEGIN
                            CREATE TABLE Favoriler (
                                FavoriID INT PRIMARY KEY IDENTITY(1,1),
                                UrunAd NVARCHAR(200),
                                UrunFiyat DECIMAL(10,2),
                                UrunResim NVARCHAR(MAX),
                                EklenmeTarihi DATETIME DEFAULT GETDATE()
                            )
                        END";
                    
                    SqlCommand createCmd = new SqlCommand(createTableQuery, conn);
                    createCmd.ExecuteNonQuery();
                    
                    string checkQuery = "SELECT FavoriID FROM Favoriler WHERE UrunAd = @ad";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@ad", ad);
                    
                    object result = checkCmd.ExecuteScalar();
                    
                    if (result != null)
                    {
                        MessageBox.Show("Bu ürün zaten favorilerinizde!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Favoriler (UrunAd, UrunFiyat, UrunResim) VALUES (@ad, @fiyat, @resim)";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                        insertCmd.Parameters.AddWithValue("@ad", ad);
                        insertCmd.Parameters.AddWithValue("@fiyat", fiyatDecimal);
                        insertCmd.Parameters.AddWithValue("@resim", resimYolu ?? "");
                        insertCmd.ExecuteNonQuery();
                        
                        MessageBox.Show("Ürün favorilere eklendi! ♥", "Favoriler", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException sqlEx) { MessageBox.Show("Veritabanı hatası:\n" + sqlEx.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            catch (Exception ex) { MessageBox.Show("Beklenmeyen hata:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

    }
}

