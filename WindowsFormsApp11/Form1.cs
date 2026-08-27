using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp11
{
    public partial class Form1 : Form
    {
   
        Color anaPembe = Color.FromArgb(236, 0, 140);
        Color acikArkaPlan = Color.FromArgb(255, 245, 250);
        Color yaziRengi = Color.FromArgb(80, 80, 80);

        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            GirisEkraniniTasarla();
        }

        private void GirisEkraniniTasarla()
        {
            
            this.Text = "Gratis | Kullanıcı Girişi";
            this.Size = new Size(400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = acikArkaPlan;
            this.FormBorderStyle = FormBorderStyle.FixedDialog; 

          
            Label lblBaslik = new Label();
            lblBaslik.Text = "gratis";
            lblBaslik.Font = new Font("Segoe UI", 42, FontStyle.Bold);
            lblBaslik.ForeColor = anaPembe;
            lblBaslik.AutoSize = true;
            lblBaslik.Location = new Point(105, 40);
            this.Controls.Add(lblBaslik);

          
            Label lblKullanici = new Label();
            lblKullanici.Text = "Kullanıcı Adı:";
            lblKullanici.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblKullanici.ForeColor = yaziRengi;
            lblKullanici.Location = new Point(70, 150);
            lblKullanici.AutoSize = true;
            this.Controls.Add(lblKullanici);

            TextBox txtKullanici = new TextBox();
            txtKullanici.Name = "txtKullanici";
            txtKullanici.Size = new Size(250, 30);
            txtKullanici.Location = new Point(70, 175);
            txtKullanici.Font = new Font("Segoe UI", 12);
            txtKullanici.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtKullanici);

            
            Label lblSifre = new Label();
            lblSifre.Text = "Şifre:";
            lblSifre.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSifre.ForeColor = yaziRengi;
            lblSifre.Location = new Point(70, 225);
            lblSifre.AutoSize = true;
            this.Controls.Add(lblSifre);
           

            TextBox txtSifre = new TextBox();
            txtSifre.Name = "txtSifre";
            txtSifre.Size = new Size(250, 30);
            txtSifre.Location = new Point(70, 250);
            txtSifre.Font = new Font("Segoe UI", 12);
            txtSifre.PasswordChar = '●'; 
            txtSifre.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtSifre);

            Label lblHata = new Label();
            lblHata.Name = "lblHata";
            lblHata.Text = "";
            lblHata.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblHata.ForeColor = Color.Red;
            lblHata.AutoSize = true;
            lblHata.Location = new Point(70, 285);
            this.Controls.Add(lblHata);
         
            Button btnGiris = new Button();
            btnGiris.Text = "GİRİŞ YAP";
            btnGiris.Size = new Size(250, 45);
            btnGiris.Location = new Point(70, 310);
            btnGiris.BackColor = anaPembe;
            btnGiris.ForeColor = Color.White;
            btnGiris.FlatStyle = FlatStyle.Flat;
            btnGiris.FlatAppearance.BorderSize = 0;
            btnGiris.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnGiris.Cursor = Cursors.Hand;
            this.Controls.Add(btnGiris);

            
            Panel pnlSus = new Panel();
            pnlSus.BackColor = anaPembe;
            pnlSus.Size = new Size(150, 5);
            pnlSus.Location = new Point(120, 420);
            this.Controls.Add(pnlSus);

     
            btnGiris.Click += BtnGiris_Click;
        }



private void BtnGiris_Click(object sender, EventArgs e)
        {
  
            string kAdi = (this.Controls["txtKullanici"] as TextBox).Text;
            string sifre = (this.Controls["txtSifre"] as TextBox).Text;

            string connectionString = @"Server=ELYSIAN\SQLEXPRESS01;Database=GratisDB;Trusted_Connection=True;";

            using (SqlConnection baglanti = new SqlConnection(connectionString))
            {
                try
                {
                    baglanti.Open();
                    string sorgu = "SELECT * FROM Kullanicilar WHERE KullaniciAdi=@user AND Sifre=@pass";
                    SqlCommand komut = new SqlCommand(sorgu, baglanti);
                    komut.Parameters.AddWithValue("@user", kAdi);
                    komut.Parameters.AddWithValue("@pass", sifre);

                    SqlDataReader oku = komut.ExecuteReader();

                    if (oku.Read())
                    {
                 
                        Form2 anaSayfa = new Form2();
                        anaSayfa.Show();
                        this.Hide(); 

                    }
                    else
                    {
                        Label lblHata = this.Controls["lblHata"] as Label;
                        lblHata.Text = "Hatalı kullanıcı adı veya şifre!";
                    }


            }
        }
    }
}

        
    

