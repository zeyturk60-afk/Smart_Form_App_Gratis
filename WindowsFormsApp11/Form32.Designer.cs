namespace WindowsFormsApp11
{
    partial class Form32
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.rtbChat = new System.Windows.Forms.RichTextBox();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(0)))), ((int)(((byte)(140)))));
            this.lblHeader.Location = new System.Drawing.Point(12, 18);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(262, 37);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Gratis Yapay Zeka";
            // 
            // rtbChat
            // 
            this.rtbChat.BackColor = System.Drawing.Color.White;
            this.rtbChat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbChat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rtbChat.Location = new System.Drawing.Point(19, 70);
            this.rtbChat.Name = "rtbChat";
            this.rtbChat.ReadOnly = true;
            this.rtbChat.Size = new System.Drawing.Size(447, 360);
            this.rtbChat.TabIndex = 1;
            this.rtbChat.Text = "";
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtInput.Location = new System.Drawing.Point(19, 440);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(342, 32);
            this.txtInput.TabIndex = 2;
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(20)))), ((int)(((byte)(140)))));
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(367, 440);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(99, 32);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Gönder";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // Form32
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(484, 491);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.rtbChat);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form32";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Yapay Zeka Destek";
            this.Load += new System.EventHandler(this.Form32_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnSend;
    }
}