using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    public partial class Form33 : Form
    {
        string connString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";
        string _urunAd;

        public Form33()
        {
            InitializeComponent();
            _urunAd = "Bilinmeyen Ürün";
        }
        
        public Form33(string urunAd)
        {
            InitializeComponent();
            _urunAd = urunAd;
        }

        private void Form33_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Yorum Yap: " + (_urunAd.Length > 20 ? _urunAd.Substring(0, 20) + "..." : _urunAd);
            cmbPuan.SelectedIndex = 4; 
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAd.Text) || string.IsNullOrWhiteSpace(txtYorum.Text))
            {
                MessageBox.Show("Lütfen adınızı ve yorumunuzu giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int puan = 5;
            if (cmbPuan.SelectedItem != null)
            {
                puan = int.Parse(cmbPuan.SelectedItem.ToString().Substring(0, 1));
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    
                    string createTableQuery = @"
                        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Yorumlar')
                        BEGIN
                            CREATE TABLE Yorumlar (
                                YorumID INT PRIMARY KEY IDENTITY(1,1),
                                UrunAd NVARCHAR(200),
                                KullaniciAdi NVARCHAR(100),
                                YorumMetni NVARCHAR(MAX),
                                Yildiz INT,
                                Tarih DATETIME DEFAULT GETDATE()
                            )
                        END";
                    
                    SqlCommand createCmd = new SqlCommand(createTableQuery, conn);
                    createCmd.ExecuteNonQuery();

                    string insertQuery = "INSERT INTO Yorumlar (UrunAd, KullaniciAdi, YorumMetni, Yildiz) VALUES (@urun, @kullanici, @yorum, @yildiz)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@urun", _urunAd);
                    insertCmd.Parameters.AddWithValue("@kullanici", txtAd.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@yorum", txtYorum.Text.Trim());
                    insertCmd.Parameters.AddWithValue("@yildiz", puan);

                    insertCmd.ExecuteNonQuery();
                    
                    MessageBox.Show("Yorumunuz başarıyla eklendi! Teşekkür ederiz.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yorum eklenirken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
