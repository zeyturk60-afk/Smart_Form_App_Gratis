using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApp11
{
    public partial class Form2 : Form
    {
        // SQL Bağlantı Bilgin
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";
        
        FlowLayoutPanel pnlSonuc; // Arama sonuçları paneli
        


        Color gratisMor = Color.FromArgb(74, 20, 140);
        Color anaPembe = Color.FromArgb(236, 0, 140);
        Color arkaPlan = Color.FromArgb(254, 252, 243);

        public Form2()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            Color gratisMor = Color.FromArgb(74, 20, 140);
            Color anaPembe = Color.FromArgb(236, 0, 140); // Canlı Pembe
            Color softPembe = Color.FromArgb(255, 240, 245); // Lavender Blush (Arka plan için yumuşak pembe)
            Color panelPembe = Color.FromArgb(252, 228, 236); // Paneller için hafif pembe

            TasarimiBaslat();
            this.BackColor = softPembe;
            
        }

        private void TasarimiBaslat()
        {
            this.Text = "Gratis | Güzellik Dünyası";
            this.Size = new Size(400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Arka plan artık yumuşak pembe tonunda

            this.AutoScroll = true;

            AramaCubuguEkle();
            AIAssistanButonuEkle();
            KampanyaPanelleriniDoldur();
            YuvarlakMarkalariEkle();
            CokSatanUrunleriEkle();
            AltKategoriPanelleriniEkle();
            AltMenuEkle();
        }

        private void AIAssistanButonuEkle()
        {
            Button btnAI = new Button
            {
                Text = "✨ YAPAY ZEKA DESTEK",
                Size = new Size(200, 35),
                Location = new Point(170, 70), // Kampanyalar başlığının hizasında sağda
                BackColor = gratisMor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAI.FlatAppearance.BorderSize = 0;
            
            btnAI.Click += (s, e) =>
            {
                Form32 aiForm = new Form32();
                aiForm.ShowDialog();
            };
            
            this.Controls.Add(btnAI);
            btnAI.BringToFront();
        }

        private void AramaCubuguEkle()
        {
            // Arama çubuğu panelini de arka plana uyumlu beyaz-pembe yapıyoruz
            Panel pnlAra = new Panel { Size = new Size(400, 60), Dock = DockStyle.Top, BackColor = Color.White };
            TextBox txtAra = new TextBox { Text = " Gratis'te ara", Size = new Size(340, 35), Location = new Point(30, 15), Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };
            
            // Placeholder
            txtAra.Enter += (s, e) => { if (txtAra.Text == " Gratis'te ara") txtAra.Text = ""; };
            txtAra.Leave += (s, e) => { if (string.IsNullOrWhiteSpace(txtAra.Text)) txtAra.Text = " Gratis'te ara"; };

            // Arama mantığı
            txtAra.TextChanged += (s, e) => 
            {
                string sorgu = txtAra.Text.Trim();
                if (sorgu.Length > 0 && sorgu != " Gratis'te ara")
                {
                    AramaYap(sorgu);
                }
                else
                {
                    pnlSonuc.Controls.Clear();
                    pnlSonuc.Visible = false;
                }
            };

            pnlAra.Controls.Add(txtAra);
            this.Controls.Add(pnlAra);

            // Sonuç Paneli
            pnlSonuc = new FlowLayoutPanel
            {
                Size = new Size(340, 150),
                Location = new Point(30, 80), // Form2'de biraz daha aşağıda olabilir veya Z-index ile halledilir
                BackColor = Color.White,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(pnlSonuc);
            pnlSonuc.BringToFront(); // Her zaman en üstte
        }

        private void AramaYap(string sorgu)
        {
            pnlSonuc.Controls.Clear();
            var mappings = GetLocalSearchMapping();
            bool sonucVar = false;

            foreach (var item in mappings)
            {
                if (item.Key.ToLower().Contains(sorgu.ToLower()))
                {
                    Button btnSonuc = new Button
                    {
                        Text = item.Key,
                        Width = pnlSonuc.Width - 25,
                        Height = 40,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.WhiteSmoke,
                        TextAlign = ContentAlignment.MiddleLeft
                    };
                    btnSonuc.FlatAppearance.BorderSize = 0;

                    btnSonuc.Click += (s, e) =>
                    {
                        Form hedef = item.Value();
                        if (hedef != null)
                        {
                            hedef.Show();
                            this.Hide();
                        }
                    };

                    pnlSonuc.Controls.Add(btnSonuc);
                    sonucVar = true;
                }
            }

            pnlSonuc.Visible = sonucVar;
            if (sonucVar) pnlSonuc.BringToFront();
        }

        private void KampanyaPanelleriniDoldur()
        {
            // Başlık
            Label lblKampanyaBaslik = new Label
            {
                Text = "KAMPANYALAR",
                Location = new Point(20, 75),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = gratisMor,
                AutoSize = true
            };
            this.Controls.Add(lblKampanyaBaslik);

            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Size = new Size(480, 180),
                Location = new Point(10, 105),
                WrapContents = false,
                AutoScroll = true
            };

            try
            {
                using (SqlConnection baglanti = new SqlConnection(connString))
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("SELECT UrunResim FROM Kampanyalar", baglanti);
                    SqlDataReader oku = komut.ExecuteReader();

                    while (oku.Read())
                    {
                        string resimYolu = Path.Combine(Application.StartupPath, oku["UrunResim"].ToString());
                        Panel p = new Panel { Size = new Size(320, 150), BackColor = anaPembe, Margin = new Padding(5) };

                        PictureBox pb = new PictureBox
                        {
                            ImageLocation = resimYolu,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Dock = DockStyle.Fill,
                            Cursor = Cursors.Hand // Üzerine gelince el işareti çıksın
                        };

                        // TIKLAMA OLAYI BURADA EKLENİYOR
                        pb.Click += (s, e) =>
                        {
                            Form3 detayFormu = new Form3(resimYolu);
                            detayFormu.ShowDialog(); // Yeni formu açar
                        };

                        p.Controls.Add(pb);
                        flp.Controls.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı Hatası: " + ex.Message);
            }
            this.Controls.Add(flp);
        }


        private void YuvarlakMarkalariEkle()
        {
            // Başlık
            Label lbl = new Label
            {
                Text = "Popüler Markalar",
                Location = new Point(20, 290), // Kampanyaların biraz altına
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true
            };
            this.Controls.Add(lbl);

            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Size = new Size(480, 130),
                Location = new Point(10, 320),
                WrapContents = false,
                AutoScroll = true
            };

            try
            {
                using (SqlConnection baglanti = new SqlConnection(connString))
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("SELECT MarkaAd, MarkaResim FROM Markalar", baglanti);
                    SqlDataReader oku = komut.ExecuteReader();

                    while (oku.Read())
                    {
                        string resimYolu = Path.Combine(Application.StartupPath, oku["MarkaResim"].ToString());

                        Panel p = new Panel { Size = new Size(80, 80), BackColor = Color.White, Margin = new Padding(8) };

                        // Yuvarlak yapma kodu
                        p.Paint += (s, e) =>
                        {
                            GraphicsPath gp = new GraphicsPath();
                            gp.AddEllipse(0, 0, p.Width - 1, p.Height - 1);
                            p.Region = new Region(gp);
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.DrawEllipse(new Pen(gratisMor, 2), 0, 0, p.Width - 1, p.Height - 1);
                        };

                        PictureBox pbMarka = new PictureBox
                        {
                            ImageLocation = resimYolu,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Dock = DockStyle.Fill
                        };

                        Label lblMarkaAd = new Label
                        {
                            Text = oku["MarkaAd"].ToString(),
                            Dock = DockStyle.Bottom,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = new Font("Segoe UI", 7, FontStyle.Bold),
                            Height = 15
                        };

                        p.Controls.Add(pbMarka);
                        // Yazıyı panelin dışına, hemen altına eklemek için FlowLayoutPanel'e ayrı ekleyebiliriz 
                        // ama şimdilik panel içine sığdırdım.

                        flp.Controls.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Marka Hatası: " + ex.Message);
            }
            this.Controls.Add(flp);
        }

        private void CokSatanUrunleriEkle()
        {
            // 1. Başlık Etiketi
            Label lbl = new Label
            {
                Text = "ÇOK SATAN ÜRÜNLER",
                Location = new Point(20, 460),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = gratisMor,
                AutoSize = true
            };
            this.Controls.Add(lbl);

            // 2. Panel (İşte hata veren nesne burada tanımlı)
            FlowLayoutPanel flp = new FlowLayoutPanel
            {
                Size = new Size(460, 200),
                Location = new Point(10, 490),
                WrapContents = false,
                AutoScroll = true
            };

            try
            {
                using (SqlConnection baglanti = new SqlConnection(connString))
                {
                    baglanti.Open();
                    // Tablo adının 'Tumurunler' olduğundan emin ol
                    SqlCommand komut = new SqlCommand("SELECT TumID, TumAd, TumFiyat, TumYolu FROM Tumurunler", baglanti);
                    SqlDataReader oku = komut.ExecuteReader();

                    while (oku.Read())
                    {
                        int secilenID = Convert.ToInt32(oku["TumID"]);
                        string resimYolu = Path.Combine(Application.StartupPath, oku["TumYolu"].ToString());

                        // Ürün Kartı Paneli
                        Panel p = new Panel { Size = new Size(130, 180), BackColor = Color.White, Margin = new Padding(8), Cursor = Cursors.Hand };
                        p.BorderStyle = BorderStyle.FixedSingle;

                        // Tıklama Olayı
                        p.Click += (s, e) =>
                        {
                            FormUrunDetay detayFormu = new FormUrunDetay(secilenID);
                            detayFormu.ShowDialog();
                        };

                        PictureBox pb = new PictureBox
                        {
                            ImageLocation = resimYolu,
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Size = new Size(130, 100),
                            Dock = DockStyle.Top,
                            Enabled = false // Panel tıklamasını engellememesi için şart
                        };

                        Label lblFiyat = new Label
                        {
                            Text = oku["TumFiyat"].ToString() + " TL",
                            Dock = DockStyle.Bottom,
                            TextAlign = ContentAlignment.MiddleCenter,
                            Font = new Font("Segoe UI", 9, FontStyle.Bold),
                            ForeColor = anaPembe,
                            Height = 25,
                            Enabled = false
                        };

                        // Önce alt bileşenleri Ürün Paneline ekliyoruz
                        p.Controls.Add(pb);
                        p.Controls.Add(lblFiyat);

                        // SONRA bu Ürün Panelini ana FlowLayoutPanel'e (flp) ekliyoruz
                        flp.Controls.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ürün Yükleme Hatası: " + ex.Message);
            }

            // En son flp'yi formun kendisine ekliyoruz
            this.Controls.Add(flp);
        }
        private void AltKategoriPanelleriniEkle()
        {
            Label lbl = new Label { Text = "ÜRÜN KATEGORİLERİ", Location = new Point(20, 600), Font = new Font("Segoe UI", 12, FontStyle.Bold), ForeColor = gratisMor, AutoSize = true };
            this.Controls.Add(lbl);
            FlowLayoutPanel flp = new FlowLayoutPanel { Size = new Size(480, 100), Location = new Point(10, 630), WrapContents = false, AutoScroll = true };
            string[] kategoriler = { "Makyaj", "Cilt Bakım", "Parfüm", "Saç Bakım" };
            foreach (var kat in kategoriler)
            {
                Panel p = new Panel { Size = new Size(120, 60), BackColor = Color.Lavender, Margin = new Padding(5) };
                p.Controls.Add(new Label { Text = kat, ForeColor = gratisMor, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 9, FontStyle.Bold) });
                flp.Controls.Add(p);
            }
            this.Controls.Add(flp);
        }

        private void AltMenuEkle()
        {
            // 1. Alt Paneli Oluştur
            Panel pnlAltNav = new Panel
            {
                Name = "pnlAltNavigasyon",
                Size = new Size(480, 80),
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

                    int butonGenislik = pnlAltNav.Width / 5;
                    int i = 0;

                    while (oku.Read())
                    {
                        string mAd = oku["MenuAd"].ToString();
                        string iYolu = Path.Combine(Application.StartupPath, oku["IkonYolu"].ToString());

                        Panel pnlButon = new Panel
                        {
                            Size = new Size(butonGenislik, 80),
                            Location = new Point(i * butonGenislik, 0),
                            Cursor = Cursors.Hand,
                            Tag = mAd
                        };

                        PictureBox pb = new PictureBox
                        {
                            ImageLocation = iYolu,
                            Size = new Size(28, 28),
                            Location = new Point((butonGenislik - 28) / 2, 12),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            Enabled = false
                        };

                        Label lbl = new Label
                        {
                            Text = mAd,
                            Font = new Font("Segoe UI", 8, mAd == "Anasayfa" ? FontStyle.Bold : FontStyle.Regular),
                            ForeColor = mAd == "Anasayfa" ? Color.FromArgb(74, 20, 140) : Color.Gray,
                            TextAlign = ContentAlignment.BottomCenter,
                            Dock = DockStyle.Bottom,
                            Height = 30,
                            Enabled = false
                        };
                        // TIKLAMA OLAYI: Tüm butonlar için yönlendirme burada yapılıyor
                        pnlButon.Click += (s, ev) =>
                        {
                            string tag = ((Panel)s).Tag.ToString();

                            if (tag == "Anasayfa")
                            {
                                // Eğer zaten Form2'deysek sadece formu yeniler veya hiçbir şey yapmazsın
                                // Form2 ana = new Form2(); ana.Show(); this.Close(); // İstersen bu şekilde yenileyebilirsin
                            }
                            else if (tag == "Kategoriler")
                            {
                                Form4 frm4 = new Form4();
                                frm4.Show();
                                this.Hide();
                            }
                            else if (tag == "Sepet") // Veritabanındaki "MenuAd" ne ise birebir aynısı olmalı!
                            {
                                Form5 frm5 = new Form5();
                                frm5.Show();
                                this.Hide();
                            }
                            else if (tag == "Favoriler")
                            {
                                Form6 frm6 = new Form6();
                                frm6.Show();
                                this.Hide();
                            }
                            else if (tag == "Profil")
                            {
                                Form7 frm7 = new Form7();
                                frm7.Show();
                                this.Hide();
                            } // Profil bloğunun sonu
                        }; // Tıklama olayının (pnlButon.Click += (s, ev) => { ... }) sonu ve BURADA NOKTALI VİRGÜL OLMALI

                        pnlButon.Controls.Add(pb);
                        pnlButon.Controls.Add(lbl);
                        pnlAltNav.Controls.Add(pnlButon);
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Alt Menü Hatası: " + ex.Message);
            }
            this.Controls.Add(pnlAltNav);
        }


        private Dictionary<string, Func<Form>> GetLocalSearchMapping()
        {
            return new Dictionary<string, Func<Form>>
            {
                // --- MAKYAJ ---
                { "Dudak Makyajı", () => new Form8() },
                { "Ruj", () => new Form8() },
                { "Likit Ruj", () => new Form8() },
                { "Kalem Ruj", () => new Form8() },
                
                { "Göz Makyajı", () => new Form9() },
                { "Maskara", () => new Form9() },
                { "Eyeliner", () => new Form9() },
                { "Göz Kalemi", () => new Form9() },

                { "Yüz Makyajı", () => new Form10() },
                { "Aydınlatıcı", () => new Form10() },
                { "Allık", () => new Form10() },
                { "Bronzer", () => new Form10() },

                { "Tırnak Bakım", () => new Form11() },
                { "Oje", () => new Form11() },
                { "Tırnak Bakım Ürünleri", () => new Form11() },

                { "Makyaj Fırçaları", () => new Form12() },

                // --- CİLT BAKIM ---
                { "Cilt Temizleme Ürünleri", () => new Form13() },
                { "Jel", () => new Form13() },
                { "Köpük", () => new Form13() },
                { "Tonik", () => new Form13() },

                { "Cilt Nemlendirici Ürünler", () => new Form14() },
                { "Krem", () => new Form14() },
                { "Losyonlar", () => new Form14() },

                { "El Bakım", () => new Form15() },
                { "El Kremi", () => new Form15() },
                { "Vücut Kremi", () => new Form15() },

                { "Vücut Bakım", () => new Form16() },
                { "Vücut Sıkılaştırıcı", () => new Form16() },

                { "Ayak Bakım", () => new Form17() },
                { "Ayak Kremi", () => new Form17() },
                { "Ayak Maskesi", () => new Form17() },

                // --- SAÇ BAKIM ---
                { "Şampuanlar", () => new Form18() },
                { "Şampuan", () => new Form18() },
                { "Erkek Şampuan", () => new Form18() },

                { "Saç Kremleri", () => new Form19() },

                { "Saç Bakım Ürünleri", () => new Form20() },
                { "Saç Bakım Kremi", () => new Form20() },
                { "Saç Köpüğü", () => new Form20() },

                { "Saç Aksesuarları", () => new Form21() },
                { "Saç Fırçası", () => new Form21() },
                { "Tarak", () => new Form21() },

                // --- PARFÜM & DEODORANT ---
                { "Parfüm", () => new Form22() },
                { "Deodorant", () => new Form23() },
                { "Roll-On", () => new Form24() },
                { "Stick", () => new Form24() },
                { "Roll-On&Stick", () => new Form24() },

                // --- ERKEK BAKIM ---
                { "Erkek Bakım", () => new Form25() }, 
                { "Erkek Tıraş Ürünleri", () => new Form25() },
                { "Erkek Duş Jeli", () => new Form26() },
                { "Erkek Cilt Bakım", () => new Form27() },

                // --- KİŞİSEL BAKIM ---
                { "Ağız&Diş Bakımı", () => new Form28() },
                { "Ağız Gargarası", () => new Form28() },
                { "Diş Fırçası", () => new Form28() },

                { "Hijyen", () => new Form29() },

                { "Duş&Banyo", () => new Form30() },
                { "Sabunlar", () => new Form30() },
                { "Duş Jeli", () => new Form30() }
            };
        }
    }
}
