namespace SI_Kasir_Toko
{
    partial class KasirDashboardForm
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
            this.btnBarang = new System.Windows.Forms.Button();
            this.btnPetugas = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnRiwayat = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFullname = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBarang
            // 
            this.btnBarang.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnBarang.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBarang.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBarang.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnBarang.Location = new System.Drawing.Point(36, 132);
            this.btnBarang.Name = "btnBarang";
            this.btnBarang.Size = new System.Drawing.Size(170, 35);
            this.btnBarang.TabIndex = 7;
            this.btnBarang.Text = "Data Barang";
            this.btnBarang.UseVisualStyleBackColor = false;
            this.btnBarang.Click += new System.EventHandler(this.btnBarang_Click);
            // 
            // btnPetugas
            // 
            this.btnPetugas.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnPetugas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPetugas.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPetugas.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnPetugas.Location = new System.Drawing.Point(36, 91);
            this.btnPetugas.Name = "btnPetugas";
            this.btnPetugas.Size = new System.Drawing.Size(170, 35);
            this.btnPetugas.TabIndex = 8;
            this.btnPetugas.Text = "Transaksi";
            this.btnPetugas.UseVisualStyleBackColor = false;
            this.btnPetugas.Click += new System.EventHandler(this.btnPetugas_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(36, 215);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(170, 36);
            this.btnLogout.TabIndex = 9;
            this.btnLogout.Text = "LogOut";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnRiwayat
            // 
            this.btnRiwayat.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnRiwayat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRiwayat.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRiwayat.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRiwayat.Location = new System.Drawing.Point(36, 173);
            this.btnRiwayat.Name = "btnRiwayat";
            this.btnRiwayat.Size = new System.Drawing.Size(170, 36);
            this.btnRiwayat.TabIndex = 10;
            this.btnRiwayat.Text = "Riwayat";
            this.btnRiwayat.UseVisualStyleBackColor = false;
            this.btnRiwayat.Click += new System.EventHandler(this.btnRiwayat_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Apa Yang Anda Butuhkan ?";
            // 
            // txtFullname
            // 
            this.txtFullname.AutoSize = true;
            this.txtFullname.Font = new System.Drawing.Font("Javanese Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFullname.Location = new System.Drawing.Point(78, 29);
            this.txtFullname.Name = "txtFullname";
            this.txtFullname.Size = new System.Drawing.Size(81, 36);
            this.txtFullname.TabIndex = 4;
            this.txtFullname.Text = "Fullname";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Javanese Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(30, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 36);
            this.label1.TabIndex = 5;
            this.label1.Text = "Hallo,";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SI_Kasir_Toko.Properties.Resources.DasboardAdmin;
            this.pictureBox1.Location = new System.Drawing.Point(258, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(244, 216);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // KasirDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(553, 300);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnBarang);
            this.Controls.Add(this.btnPetugas);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnRiwayat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFullname);
            this.Controls.Add(this.label1);
            this.Name = "KasirDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KasirDashboardForm";
            this.Load += new System.EventHandler(this.KasirDashboardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnBarang;
        private System.Windows.Forms.Button btnPetugas;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnRiwayat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label txtFullname;
        private System.Windows.Forms.Label label1;
    }
}