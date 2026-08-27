using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp11
{
    public partial class Form32 : Form
    {
        public Form32()
        {
            InitializeComponent();
        }

        private void Form32_Load(object sender, EventArgs e)
        {
            AppendMessage("AI Asistan", "Merhaba! Size nasıl yardımcı olabilirim?", Color.FromArgb(74, 20, 140));
            
           
            txtInput.Text = "Mesajınızı buraya yazın...";
            txtInput.ForeColor = Color.Gray;
            
            txtInput.GotFocus += (s, ev) => 
            {
                if (txtInput.Text == "Mesajınızı buraya yazın...")
                {
                    txtInput.Text = "";
                    txtInput.ForeColor = Color.Black;
                }
            };
            
            txtInput.LostFocus += (s, ev) => 
            {
                if (string.IsNullOrWhiteSpace(txtInput.Text))
                {
                    txtInput.Text = "Mesajınızı buraya yazın...";
                    txtInput.ForeColor = Color.Gray;
                }
            };

            this.ActiveControl = txtInput;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void SendMessage()
        {
            string query = txtInput.Text.Trim();
            if (string.IsNullOrEmpty(query) || query == "Mesajınızı buraya yazın...") return;

            AppendMessage("Sen", query, Color.Black);
            txtInput.Clear();

            
            Application.DoEvents();
            System.Threading.Thread.Sleep(400);

            string response = AnalyzeInput(query.ToLower());
            AppendMessage("AI Asistan", response, Color.FromArgb(74, 20, 140));
        }

        private string AnalyzeInput(string input)
        {
            if (input.Contains("merhaba") || input.Contains("selam"))
                return "Merhaba! Hangi kategoride ürün arıyorsunuz? (Makyaj, Cilt Bakımı vb.)";

            else if (input.Contains("fiyat") || input.Contains("ne kadar") || input.Contains("kaç tl"))
                return "Ürünlerin güncel fiyatlarını 'Tüm Ürünler' sayfamızdan görüntüleyebilirsiniz. Belirli bir ürünü merak ediyorsanız detaylarına bakabilirsiniz.";

            else if (input.Contains("kampanya") || input.Contains("indirim"))
                return "Şu an seçkin makyaj ürünlerinde %30 indirim fırsatımız var! Fırsatları kaçırmayın.";

            else if (input.Contains("kargo"))
                return "150 TL ve üzeri alışverişlerinizde kargo ücretsizdir. Kargonuz en geç 3 iş günü içinde yola çıkar.";

            else if (input.Contains("teşekkür ederim"))
                return "Rica ederim, başka bir sorunuz olursa buradayım! 😊";

            else if (input.Contains("teşekkürler"))
                return "Rica ederim, başka bir sorunuz olursa buradayım! 😊";

            else if (input.Contains("ürün") || input.Contains("makyaj"))
                return "Makyaj, cilt bakımı ve kişisel bakım dahil yüzlerce ürünümüz mevcuttur. Arama kutusunu kullanarak beğendiğiniz ürünleri Sepete veya Favorilere ekleyebilirsiniz.";

            else if (input.Contains("kapıda ödeme var mı") || input.Contains("makyaj"))
                return " ürünlerimizi banka veya kredi kartı kullanarak sipariş edebilirsiniz.";

            else if (input.Contains("uygulamaya giremiyorum") || input.Contains("makyaj"))
                return "uygulama ile ilgili detaylı sorularınız ve sorunlarınız için iletişim numaramız 444 44 44";

            else if (input.Contains("sipariş veremiyorum") || input.Contains("makyaj"))
                return "siparişleriniz ile ilgili detaylı sorularınız ve sorunlarınız için iletişim numaramız 444 44 44 .";

            else if (input.Contains("indirim") || input.Contains("makyaj"))
                return "Mağazamızda 10-12 Ağustos tarihleri arasında %70 e varan indirim sizleri bekliyor.";

            else if (input.Contains("ürün iadesi") || input.Contains("iade"))
                return "ürün iade süresi 14 iş günüdür, siparişlerim kısmından iade edebilirsiniz";

            else if (input.Contains("siparişim gelmedi") || input.Contains("makyaj"))
                return "Kargonuz en geç 3 iş günü içinde yola çıkar,detaylı sorularınız ve sorunlarınız için iletişim numaramız 444 44 44.";


            else if (input.Contains("bakım ürünleri") || input.Contains("makyaj"))
                return "Makyaj, cilt bakımı ve kişisel bakım dahil yüzlerce ürünümüz mevcuttur. Arama kutusunu kullanarak beğendiğiniz ürünleri Sepete veya Favorilere ekleyebilirsiniz.";
            else
                return "Anlayamadım, lütfen farklı kelimelerle tekrar sorar mısınız? Stok veya fiyat bilgisi için kelime kullanabilirsiniz.";
        }

        private void AppendMessage(string sender, string message, Color color)
        {
            rtbChat.SelectionStart = rtbChat.TextLength;
            rtbChat.SelectionLength = 0;
            
            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Bold);
            rtbChat.SelectionColor = color;
            rtbChat.AppendText(sender + ": ");

            rtbChat.SelectionFont = new Font(rtbChat.Font, FontStyle.Regular);
            rtbChat.SelectionColor = Color.Black;
            rtbChat.AppendText(message + Environment.NewLine + Environment.NewLine);

            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();
        }
    }
}
