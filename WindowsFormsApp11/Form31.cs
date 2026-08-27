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
    public partial class Form31 : Form
    {
        Color anaPembe = Color.FromArgb(236, 0, 140);
        Color tozPembe = Color.FromArgb(255, 240, 245);

        public Form31()
        {
            InitializeComponent();
            TasarimiOlustur();
        }

        private void TasarimiOlustur()
        {
            this.Size = new Size(400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = tozPembe;
            this.Text = "Adreslerim";

            Label lblBaslik = new Label
            {
                Text = "Adreslerim",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = anaPembe,
                Size = new Size(400, 50),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblBaslik);

            Panel pnlAdres = new Panel
            {
                Size = new Size(360, 120),
                Location = new Point(20, 100),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(pnlAdres);

            Label lblEv = new Label
            {
                Text = "Ev Adresim",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };
            pnlAdres.Controls.Add(lblEv);

            Label lblAdresDetay = new Label
            {
                Text = "Zuhuratbaba, İncirli Cd. No:45, 34147 Bakırköy/İstanbul",
                Font = new Font("Segoe UI", 10),
                Location = new Point(10, 40),
                Size = new Size(340, 60),
                ForeColor = Color.DimGray
            };
            pnlAdres.Controls.Add(lblAdresDetay);

            Button btnGeri = new Button
            {
                Text = "GERİ DÖN",
                Size = new Size(360, 45),
                Location = new Point(20, 600),
                BackColor = anaPembe,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnGeri.FlatAppearance.BorderSize = 0;
            btnGeri.Click += (s, e) => {
                Form7 profil = new Form7();
                profil.Show();
                this.Hide();
            };
            this.Controls.Add(btnGeri);
        }
    }
}
