namespace Projeto
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Userslabel = new System.Windows.Forms.Label();
            this.enviar_bt_TP = new System.Windows.Forms.Button();
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ConnectedCounterlabel = new System.Windows.Forms.Label();
            this.ChatlistBox = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbIsVerified = new System.Windows.Forms.TextBox();
            this.labelHashVerificada = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.tbDataToSign = new System.Windows.Forms.TextBox();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.buttonSignHash = new System.Windows.Forms.Button();
            this.buttonSignData = new System.Windows.Forms.Button();
            this.tbHashData = new System.Windows.Forms.TextBox();
            this.labelHashData = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSignature = new System.Windows.Forms.TextBox();
            this.buttonVerifyHash = new System.Windows.Forms.Button();
            this.buttonVerifyData = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // Userslabel
            // 
            this.Userslabel.AutoSize = true;
            this.Userslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.Userslabel.Location = new System.Drawing.Point(19, 25);
            this.Userslabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Userslabel.Name = "Userslabel";
            this.Userslabel.Size = new System.Drawing.Size(258, 25);
            this.Userslabel.TabIndex = 9;
            this.Userslabel.Text = "Utilizadores Conectados: ";
            // 
            // enviar_bt_TP
            // 
            this.enviar_bt_TP.BackColor = System.Drawing.SystemColors.Window;
            this.enviar_bt_TP.Location = new System.Drawing.Point(965, 441);
            this.enviar_bt_TP.Margin = new System.Windows.Forms.Padding(4);
            this.enviar_bt_TP.Name = "enviar_bt_TP";
            this.enviar_bt_TP.Size = new System.Drawing.Size(100, 50);
            this.enviar_bt_TP.TabIndex = 6;
            this.enviar_bt_TP.Text = "Enviar";
            this.enviar_bt_TP.UseVisualStyleBackColor = false;
            this.enviar_bt_TP.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(215)))));
            this.textBoxMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMsg.Location = new System.Drawing.Point(19, 441);
            this.textBoxMsg.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Size = new System.Drawing.Size(930, 48);
            this.textBoxMsg.TabIndex = 5;
            // 
            // ConnectedCounterlabel
            // 
            this.ConnectedCounterlabel.AutoSize = true;
            this.ConnectedCounterlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.ConnectedCounterlabel.Location = new System.Drawing.Point(276, 25);
            this.ConnectedCounterlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ConnectedCounterlabel.Name = "ConnectedCounterlabel";
            this.ConnectedCounterlabel.Size = new System.Drawing.Size(77, 25);
            this.ConnectedCounterlabel.TabIndex = 10;
            this.ConnectedCounterlabel.Text = "<num>";
            // 
            // ChatlistBox
            // 
            this.ChatlistBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(220)))), ((int)(((byte)(215)))));
            this.ChatlistBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ChatlistBox.FormattingEnabled = true;
            this.ChatlistBox.ItemHeight = 16;
            this.ChatlistBox.Location = new System.Drawing.Point(16, 71);
            this.ChatlistBox.Margin = new System.Windows.Forms.Padding(4);
            this.ChatlistBox.Name = "ChatlistBox";
            this.ChatlistBox.Size = new System.Drawing.Size(1049, 354);
            this.ChatlistBox.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(188)))), ((int)(((byte)(226)))), ((int)(((byte)(198)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.ConnectedCounterlabel);
            this.panel1.Controls.Add(this.Userslabel);
            this.panel1.Location = new System.Drawing.Point(-5, -5);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1104, 66);
            this.panel1.TabIndex = 13;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(996, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(77, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // tbIsVerified
            // 
            this.tbIsVerified.Location = new System.Drawing.Point(1222, 470);
            this.tbIsVerified.Name = "tbIsVerified";
            this.tbIsVerified.Size = new System.Drawing.Size(100, 22);
            this.tbIsVerified.TabIndex = 17;
            // 
            // labelHashVerificada
            // 
            this.labelHashVerificada.AutoSize = true;
            this.labelHashVerificada.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.labelHashVerificada.Location = new System.Drawing.Point(1105, 466);
            this.labelHashVerificada.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHashVerificada.Name = "labelHashVerificada";
            this.labelHashVerificada.Size = new System.Drawing.Size(110, 25);
            this.labelHashVerificada.TabIndex = 15;
            this.labelHashVerificada.Text = "Is verified:";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(-115, 58);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(203, 470);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(183, 336);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(588, 246);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 16;
            this.pictureBox3.TabStop = false;
            // 
            // tbDataToSign
            // 
            this.tbDataToSign.Location = new System.Drawing.Point(1132, 71);
            this.tbDataToSign.Name = "tbDataToSign";
            this.tbDataToSign.Size = new System.Drawing.Size(348, 22);
            this.tbDataToSign.TabIndex = 18;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(1132, 113);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(348, 37);
            this.buttonGenerate.TabIndex = 19;
            this.buttonGenerate.Text = "Instantiate Asymmetric Algorithm and Generate Keys";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // buttonSignHash
            // 
            this.buttonSignHash.Location = new System.Drawing.Point(1132, 167);
            this.buttonSignHash.Name = "buttonSignHash";
            this.buttonSignHash.Size = new System.Drawing.Size(163, 32);
            this.buttonSignHash.TabIndex = 20;
            this.buttonSignHash.Text = "SignHash";
            this.buttonSignHash.UseVisualStyleBackColor = true;
            this.buttonSignHash.Click += new System.EventHandler(this.buttonSignHash_Click);
            // 
            // buttonSignData
            // 
            this.buttonSignData.Location = new System.Drawing.Point(1317, 167);
            this.buttonSignData.Name = "buttonSignData";
            this.buttonSignData.Size = new System.Drawing.Size(163, 32);
            this.buttonSignData.TabIndex = 21;
            this.buttonSignData.Text = "SignData";
            this.buttonSignData.UseVisualStyleBackColor = true;
            this.buttonSignData.Click += new System.EventHandler(this.buttonSignData_Click);
            // 
            // tbHashData
            // 
            this.tbHashData.Location = new System.Drawing.Point(1087, 233);
            this.tbHashData.Multiline = true;
            this.tbHashData.Name = "tbHashData";
            this.tbHashData.Size = new System.Drawing.Size(348, 50);
            this.tbHashData.TabIndex = 22;
            // 
            // labelHashData
            // 
            this.labelHashData.AutoSize = true;
            this.labelHashData.Location = new System.Drawing.Point(1084, 214);
            this.labelHashData.Name = "labelHashData";
            this.labelHashData.Size = new System.Drawing.Size(85, 16);
            this.labelHashData.TabIndex = 23;
            this.labelHashData.Text = "Hash of Data";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1084, 297);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Signature of Data";
            // 
            // tbSignature
            // 
            this.tbSignature.Location = new System.Drawing.Point(1082, 327);
            this.tbSignature.Multiline = true;
            this.tbSignature.Name = "tbSignature";
            this.tbSignature.Size = new System.Drawing.Size(398, 88);
            this.tbSignature.TabIndex = 24;
            // 
            // buttonVerifyHash
            // 
            this.buttonVerifyHash.Location = new System.Drawing.Point(1115, 421);
            this.buttonVerifyHash.Name = "buttonVerifyHash";
            this.buttonVerifyHash.Size = new System.Drawing.Size(100, 31);
            this.buttonVerifyHash.TabIndex = 26;
            this.buttonVerifyHash.Text = "VerifyHash";
            this.buttonVerifyHash.UseVisualStyleBackColor = true;
            this.buttonVerifyHash.Click += new System.EventHandler(this.buttonVerifyHash_Click);
            // 
            // buttonVerifyData
            // 
            this.buttonVerifyData.Location = new System.Drawing.Point(1222, 421);
            this.buttonVerifyData.Name = "buttonVerifyData";
            this.buttonVerifyData.Size = new System.Drawing.Size(100, 31);
            this.buttonVerifyData.TabIndex = 27;
            this.buttonVerifyData.Text = "VerifyData";
            this.buttonVerifyData.UseVisualStyleBackColor = true;
            this.buttonVerifyData.Click += new System.EventHandler(this.buttonVerifyData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(211)))), ((int)(((byte)(208)))));
            this.ClientSize = new System.Drawing.Size(1486, 502);
            this.Controls.Add(this.buttonVerifyData);
            this.Controls.Add(this.buttonVerifyHash);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbSignature);
            this.Controls.Add(this.labelHashData);
            this.Controls.Add(this.tbHashData);
            this.Controls.Add(this.buttonSignData);
            this.Controls.Add(this.buttonSignHash);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.tbDataToSign);
            this.Controls.Add(this.tbIsVerified);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelHashVerificada);
            this.Controls.Add(this.ChatlistBox);
            this.Controls.Add(this.enviar_bt_TP);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Dichcorde";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Userslabel;
        private System.Windows.Forms.Button enviar_bt_TP;
        private System.Windows.Forms.TextBox textBoxMsg;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label ConnectedCounterlabel;
        private System.Windows.Forms.ListBox ChatlistBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label labelHashVerificada;
        private System.Windows.Forms.TextBox tbIsVerified;
        private System.Windows.Forms.TextBox tbDataToSign;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonSignHash;
        private System.Windows.Forms.Button buttonSignData;
        private System.Windows.Forms.TextBox tbHashData;
        private System.Windows.Forms.Label labelHashData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSignature;
        private System.Windows.Forms.Button buttonVerifyHash;
        private System.Windows.Forms.Button buttonVerifyData;
    }
}

