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
    public partial class Form4 : Form
    {
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";

        FlowLayoutPanel flpSolMenu;

        Panel pnlSagIcerik;
        FlowLayoutPanel pnlSonuc; // Arama sonuçlarını gösterecek panel
        
        public Form4()
        {
            InitializeComponent();
            this.AutoScroll = true;
            this.Size = new Size(400, 700);
           
            this.Text = "Gratis | Pembe Dünya";
            this.StartPosition = FormStartPosition.CenterScreen;

           
            KategoriSayfasiTasarla();
            AltMenuEkle(); // Alt navigasyon çubuğunu ekler
            AramaCubuguEkle();
        }
        private void AramaCubuguEkle()
        {
            // Arama çubuğu panelini de arka plana uyumlu beyaz-pembe yapıyoruz
            Panel pnlAra = new Panel { Size = new Size(400, 60), Dock = DockStyle.Top, BackColor = Color.White };
            TextBox txtAra = new TextBox { Text = " Gratis'te ara", Size = new Size(340, 35), Location = new Point(30, 15), Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };
            
            // Placeholder (Yer tutucu) mantığı
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
                Size = new Size(340, 150), // TextBox ile aynı genişlikte
                Location = new Point(30, 50), // Hemen altında
                BackColor = Color.White,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(pnlSonuc);
            pnlSonuc.BringToFront();
        }

        private void AramaYap(string sorgu)
        {
            pnlSonuc.Controls.Clear();
            var mappings = GetLocalSearchMapping();
            bool sonucVar = false;

            foreach (var item in mappings)
            {
                // Küçük/büyük harf duyarsız arama
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
                        // İlgili formu aç
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
            if(sonucVar) pnlSonuc.BringToFront();
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

        private void KategoriSayfasiTasarla()
        {
            this.Controls.Clear(); // Formu temizle ki üst üste binmesin

          
          
            // 2. SOL MENÜ (Makyaj, Cilt Bakım listesi)
            flpSolMenu = new FlowLayoutPanel
            {
                Location = new Point(0, 80), // Arama çubuğunun hemen altı
                Width = 140, // Sabit genişlik
                Height = this.Height - 150,
                BackColor = Color.LavenderBlush,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            this.Controls.Add(flpSolMenu);

            // 3. SAĞ İÇERİK (Yazıların olduğu alan)
            // ÖNEMLİ: Location.X değerini sol menünün genişliğinden (140) daha büyük (150) yapıyoruz.
            pnlSagIcerik = new Panel
            {
                Location = new Point(150, 80),
                Width = this.Width - 170,
                Height = this.Height - 150,
                BackColor = Color.White,
                AutoScroll = true,
                Name = "pnlSagIcerik"
            };
            this.Controls.Add(pnlSagIcerik);

            // Sağ paneli en öne getir (Sol panelin altında kalmasın)
            pnlSagIcerik.BringToFront();

            AnaKategorileriDoldur();
        }

        // Form4 sınıfı içinde, KategoriSayfasiTasarla ve diğer metotlarla aynı erişim seviyesinde ekleyin.
        private void AnaKategorileriDoldur()
        {
            flpSolMenu.Controls.Clear();

            // Ana kategoriler listesi
            var anaKategoriler = new List<string>
            {
                "Makyaj",
                "Cilt Bakım",
                "Saç Bakım",
                "Parfüm&Deodorant",
                "Erkek Bakım",
                "Kişisel Bakım",

                // Diğer ana kategoriler buraya eklenebilir: "Saç", "Parfüm", "Erkek", "Kişisel", "Anne-Bebek"
            };

            foreach (var kategori in anaKategoriler)
            {
                Button btn = new Button
                {
                    Text = kategori,
                    Width = flpSolMenu.Width - 20,
                    Height = 50,
                    Margin = new Padding(10, 10, 10, 0),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };
                btn.FlatAppearance.BorderSize = 0;

                btn.Click += (s, e) =>
                {
                    SagIcerigiDoldur(kategori);
                };

                flpSolMenu.Controls.Add(btn);
            }
        }

        private void SagIcerigiDoldur(string anaKat)
        {
            pnlSagIcerik.Controls.Clear();
            var altDatalar = new List<Tuple<string, string>>();

            // Kategorileri belirle (Daha önce saydığın tüm listeyi buraya ekle)
            if (anaKat == "Makyaj")
            {
                altDatalar.Add(new Tuple<string, string>("Dudak Makyajı", "Ruj, Likit Ruj, Kalem Ruj..."));
                altDatalar.Add(new Tuple<string, string>("Göz Makyajı", "Maskara, Eyeliner, Göz Kalemi..."));
                altDatalar.Add(new Tuple<string, string>("Yüz Makyajı", "Aydınlatıcı,Allık,Bronzer..."));
                altDatalar.Add(new Tuple<string, string>("Tırnak Bakım", "Oje,Tırnak Bakım Ürünleri..."));
                altDatalar.Add(new Tuple<string, string>("Makyaj Fırçaları", ""));


            }
            else if (anaKat == "Cilt Bakım")
            {
                altDatalar.Add(new Tuple<string, string>("Cilt Temizleme Ürünleri", "Jel, Köpük, Tonik..."));
                altDatalar.Add(new Tuple<string, string>("Cilt Nemlendirici Ürünler", "Krem ve Losyonlar..."));
                altDatalar.Add(new Tuple<string, string>("El Bakım", "El&Vücut Kremleri..."));
                altDatalar.Add(new Tuple<string, string>("Vücut Bakım", "Vücut Sıkılaştırıcı..."));
                altDatalar.Add(new Tuple<string, string>("Ayak Bakım", "Ayak Krem & MAske..."));

            }
            else if (anaKat == "Saç Bakım")
            {
                altDatalar.Add(new Tuple<string, string>("Şampuanlar", "Şampuan,Erkek Şampuan..."));
                altDatalar.Add(new Tuple<string, string>("Saç Kremleri", ""));
                altDatalar.Add(new Tuple<string, string>("Saç Bakım Ürünleri", "Saç Bakım Krem,Saç Köpüğü..."));
                altDatalar.Add(new Tuple<string, string>("Saç Aksesuarları", "Saç Fırçası,Tarak..."));
            }
            else if (anaKat == "Parfüm&Deodorant")
            {
                altDatalar.Add(new Tuple<string, string>("Parfüm", ""));
                altDatalar.Add(new Tuple<string, string>("Deodorant", ""));
                altDatalar.Add(new Tuple<string, string>("Roll-On&Stick", ""));

            }
            else if (anaKat == "Erkek Bakım")
            {
                altDatalar.Add(new Tuple<string, string>("Erkek Tıraş Ürünleri", ""));
                altDatalar.Add(new Tuple<string, string>("Erkek Duş Jeli", ""));
                altDatalar.Add(new Tuple<string, string>("Erkek Cilt Bakım", ""));

            }
            else if (anaKat == "Kişisel Bakım")
            {
                altDatalar.Add(new Tuple<string, string>("Ağız&Diş Bakımı", "Ağız Gargarası,Diş Fırçası..."));
                altDatalar.Add(new Tuple<string, string>("Hijyen", ""));
                altDatalar.Add(new Tuple<string, string>("Duş&Banyo", "Sabunlar,Duş Jeli..."));

            }

            int y = 5;
            foreach (var item in altDatalar)
            {
                Button btn = new Button
                {
                    Size = new Size(pnlSagIcerik.Width - 25, 80),
                    Location = new Point(5, y),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White
                };
                btn.FlatAppearance.BorderSize = 0;

                // Yazıların butonun en soluna yapışmaması için X: 20 veriyoruz
                Label lblBaslik = new Label
                {
                    Text = item.Item1,
                    Font = new Font("Segoe UI Black", 10),
                    Location = new Point(20, 15),
                    AutoSize = true,
                    Enabled = false
                };

                Label lblDetay = new Label
                {
                    Text = item.Item2,
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.HotPink,
                    Location = new Point(20, 40),
                    AutoSize = true,
                    Enabled = false
                };

                btn.Controls.Add(lblBaslik);
                btn.Controls.Add(lblDetay);

                // Alt kategori butonlarına tıklama olayı
                btn.Click += (sender, e) =>
                {
                    Form hedefForm = null;
                    string baslik = item.Item1;

                    // Makyaj (Form8 - Form12)
                    if (baslik == "Dudak Makyajı") hedefForm = new Form8();
                    else if (baslik == "Göz Makyajı") hedefForm = new Form9();
                    else if (baslik == "Yüz Makyajı") hedefForm = new Form10();
                    else if (baslik == "Tırnak Bakım") hedefForm = new Form11();
                    else if (baslik == "Makyaj Fırçaları") hedefForm = new Form12();
                    
                    // Cilt Bakım (Form13 - Form17)
                    else if (baslik == "Cilt Temizleme Ürünleri") hedefForm = new Form13();
                    else if (baslik == "Cilt Nemlendirici Ürünler") hedefForm = new Form14();
                    else if (baslik == "El Bakım") hedefForm = new Form15();
                    else if (baslik == "Vücut Bakım") hedefForm = new Form16();
                    else if (baslik == "Ayak Bakım") hedefForm = new Form17();
                    
                    // Saç Bakım (Form18 - Form21)
                    else if (baslik == "Şampuanlar") hedefForm = new Form18();
                    else if (baslik == "Saç Kremleri") hedefForm = new Form19();
                    else if (baslik == "Saç Bakım Ürünleri") hedefForm = new Form20();
                    else if (baslik == "Saç Aksesuarları") hedefForm = new Form21();
                    
                    // Parfüm & Deodorant (Form22 - Form24)
                    else if (baslik == "Parfüm") hedefForm = new Form22();
                    else if (baslik == "Deodorant") hedefForm = new Form23();
                    else if (baslik == "Roll-On&Stick") hedefForm = new Form24();
                    
                    // Erkek Bakım (Form25 - Form27)
                    else if (baslik == "Erkek Tıraş Ürünleri") hedefForm = new Form25();
                    else if (baslik == "Erkek Duş Jeli") hedefForm = new Form26();
                    else if (baslik == "Erkek Cilt Bakım") hedefForm = new Form27();
                    
                    // Kişisel Bakım (Form28 - Form30)
                    else if (baslik == "Ağız&Diş Bakımı") hedefForm = new Form28();
                    else if (baslik == "Hijyen") hedefForm = new Form29();
                    else if (baslik == "Duş&Banyo") hedefForm = new Form30();

                    if (hedefForm != null)
                    {
                        hedefForm.Show();
                        this.Hide();
                    }
                };

                pnlSagIcerik.Controls.Add(btn);
                y += 85;

            }
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



