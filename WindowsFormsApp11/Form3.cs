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
    public partial class Form3 : Form
    {
         public Form3(string resimYolu)
        {
            InitializeComponent();
            this.Text = "Kampanya Detayı";
            this.Size = new Size(400, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Resmi tam boy gösterecek kutu
            PictureBox pbBuyuk = new PictureBox
            {
                ImageLocation = resimYolu,
                SizeMode = PictureBoxSizeMode.Zoom, // Resmi bozmadan sığdırır
                Dock = DockStyle.Fill
            };
            this.Controls.Add(pbBuyuk);
        }

    }
}