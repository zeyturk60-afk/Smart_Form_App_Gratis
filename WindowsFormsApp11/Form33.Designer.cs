namespace WindowsFormsApp11
{
    partial class Form33
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblAd = new System.Windows.Forms.Label();
            this.txtAd = new System.Windows.Forms.TextBox();
            this.lblPuan = new System.Windows.Forms.Label();
            this.cmbPuan = new System.Windows.Forms.ComboBox();
            this.lblYorum = new System.Windows.Forms.Label();
            this.txtYorum = new System.Windows.Forms.TextBox();
            this.btnGonder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(20)))), ((int)(((byte)(140)))));
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(161, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Yorum Ekle";
            // 
            // lblAd
            // 
            this.lblAd.AutoSize = true;
            this.lblAd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAd.Location = new System.Drawing.Point(32, 80);
            this.lblAd.Name = "lblAd";
            this.lblAd.Size = new System.Drawing.Size(117, 23);
            this.lblAd.TabIndex = 1;
            this.lblAd.Text = "Adınız Soyadınız";
            // 
            // txtAd
            // 
            this.txtAd.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtAd.Location = new System.Drawing.Point(36, 110);
            this.txtAd.Name = "txtAd";
            this.txtAd.Size = new System.Drawing.Size(364, 30);
            this.txtAd.TabIndex = 2;
            // 
            // lblPuan
            // 
            this.lblPuan.AutoSize = true;
            this.lblPuan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPuan.Location = new System.Drawing.Point(32, 160);
            this.lblPuan.Name = "lblPuan";
            this.lblPuan.Size = new System.Drawing.Size(95, 23);
            this.lblPuan.TabIndex = 3;
            this.lblPuan.Text = "Puanınız";
            // 
            // cmbPuan
            // 
            this.cmbPuan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPuan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPuan.FormattingEnabled = true;
            this.cmbPuan.Items.AddRange(new object[] {
            "5 - Mükemmel",
            "4 - İyi",
            "3 - Normal",
            "2 - Kötü",
            "1 - Çok Kötü"});
            this.cmbPuan.Location = new System.Drawing.Point(36, 190);
            this.cmbPuan.Name = "cmbPuan";
            this.cmbPuan.Size = new System.Drawing.Size(150, 31);
            this.cmbPuan.TabIndex = 4;
            // 
            // lblYorum
            // 
            this.lblYorum.AutoSize = true;
            this.lblYorum.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblYorum.Location = new System.Drawing.Point(32, 240);
            this.lblYorum.Name = "lblYorum";
            this.lblYorum.Size = new System.Drawing.Size(95, 23);
            this.lblYorum.TabIndex = 5;
            this.lblYorum.Text = "Yorumunuz";
            // 
            // txtYorum
            // 
            this.txtYorum.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtYorum.Location = new System.Drawing.Point(36, 270);
            this.txtYorum.Multiline = true;
            this.txtYorum.Name = "txtYorum";
            this.txtYorum.Size = new System.Drawing.Size(364, 120);
            this.txtYorum.TabIndex = 6;
            // 
            // btnGonder
            // 
            this.btnGonder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            this.btnGonder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGonder.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGonder.ForeColor = System.Drawing.Color.White;
            this.btnGonder.Location = new System.Drawing.Point(36, 420);
            this.btnGonder.Name = "btnGonder";
            this.btnGonder.Size = new System.Drawing.Size(364, 50);
            this.btnGonder.TabIndex = 7;
            this.btnGonder.Text = "YORUMU GÖNDER";
            this.btnGonder.UseVisualStyleBackColor = false;
            this.btnGonder.Click += new System.EventHandler(this.btnGonder_Click);
            // 
            // Form33
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(434, 511);
            this.Controls.Add(this.btnGonder);
            this.Controls.Add(this.txtYorum);
            this.Controls.Add(this.lblYorum);
            this.Controls.Add(this.cmbPuan);
            this.Controls.Add(this.lblPuan);
            this.Controls.Add(this.txtAd);
            this.Controls.Add(this.lblAd);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form33";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yorum Yap";
            this.Load += new System.EventHandler(this.Form33_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAd;
        private System.Windows.Forms.TextBox txtAd;
        private System.Windows.Forms.Label lblPuan;
        private System.Windows.Forms.ComboBox cmbPuan;
        private System.Windows.Forms.Label lblYorum;
        private System.Windows.Forms.TextBox txtYorum;
        private System.Windows.Forms.Button btnGonder;
    }
}