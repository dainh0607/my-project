namespace GUI_QuanLyVatTu
{
    partial class frmLoadding
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2CircleProgressBar1 = new Guna.UI2.WinForms.Guna2CircleProgressBar();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label_val = new Label();
            label1 = new Label();
            guna2PictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(components);
            guna2CircleProgressBar1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).BeginInit();
            SuspendLayout();
            // 
            // guna2CircleProgressBar1
            // 
            guna2CircleProgressBar1.BackColor = Color.White;
            guna2CircleProgressBar1.Controls.Add(label5);
            guna2CircleProgressBar1.Controls.Add(label4);
            guna2CircleProgressBar1.Controls.Add(label3);
            guna2CircleProgressBar1.Controls.Add(label_val);
            guna2CircleProgressBar1.Controls.Add(label1);
            guna2CircleProgressBar1.Controls.Add(guna2PictureBox1);
            guna2CircleProgressBar1.FillColor = Color.Transparent;
            guna2CircleProgressBar1.FillThickness = 800;
            guna2CircleProgressBar1.Font = new Font("Segoe UI", 12F);
            guna2CircleProgressBar1.ForeColor = Color.White;
            guna2CircleProgressBar1.Location = new Point(19, -495);
            guna2CircleProgressBar1.Minimum = 0;
            guna2CircleProgressBar1.Name = "guna2CircleProgressBar1";
            guna2CircleProgressBar1.ProgressColor = Color.FromArgb(18, 48, 100);
            guna2CircleProgressBar1.ProgressColor2 = Color.Empty;
            guna2CircleProgressBar1.ProgressThickness = 700;
            guna2CircleProgressBar1.ShadowDecoration.CustomizableEdges = customizableEdges3;
            guna2CircleProgressBar1.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            guna2CircleProgressBar1.Size = new Size(1800, 1800);
            guna2CircleProgressBar1.TabIndex = 0;
            guna2CircleProgressBar1.Text = "guna2CircleProgressBar1";
            guna2CircleProgressBar1.ValueChanged += guna2CircleProgressBar1_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Arial", 72F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label5.ForeColor = Color.White;
            label5.Location = new Point(215, 747);
            label5.Name = "label5";
            label5.Size = new Size(438, 139);
            label5.TabIndex = 5;
            label5.Text = "Vật Tư";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Arial", 36F, FontStyle.Bold);
            label4.Location = new Point(58, 649);
            label4.Name = "label4";
            label4.Size = new Size(589, 70);
            label4.TabIndex = 4;
            label4.Text = "Phần Mềm Quản Lý";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Arial", 36F, FontStyle.Bold);
            label3.Location = new Point(58, 549);
            label3.Name = "label3";
            label3.Size = new Size(725, 70);
            label3.TabIndex = 3;
            label3.Text = "Chào mừng bạn đến với";
            // 
            // label_val
            // 
            label_val.AutoSize = true;
            label_val.BackColor = Color.Transparent;
            label_val.Font = new Font("Bahnschrift", 48F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label_val.ForeColor = Color.FromArgb(77, 186, 235);
            label_val.Location = new Point(296, 953);
            label_val.Name = "label_val";
            label_val.Size = new Size(84, 96);
            label_val.TabIndex = 2;
            label_val.Text = "0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(827, 1011);
            label1.Name = "label1";
            label1.Size = new Size(163, 38);
            label1.TabIndex = 1;
            label1.Text = "Loadding...";
            // 
            // guna2PictureBox1
            // 
            guna2PictureBox1.BackColor = Color.Transparent;
            guna2PictureBox1.CustomizableEdges = customizableEdges1;
            guna2PictureBox1.Image = Properties.Resources.Logo_KhongNen;
            guna2PictureBox1.ImageRotate = 0F;
            guna2PictureBox1.Location = new Point(724, 754);
            guna2PictureBox1.Name = "guna2PictureBox1";
            guna2PictureBox1.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2PictureBox1.Size = new Size(353, 272);
            guna2PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            guna2PictureBox1.TabIndex = 0;
            guna2PictureBox1.TabStop = false;
            // 
            // timer1
            // 
            timer1.Tick += timer1_Tick;
            // 
            // frmLoadding
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1208, 754);
            Controls.Add(guna2CircleProgressBar1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLoadding";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Loadding";
            Load += frmLoadding_Load;
            guna2CircleProgressBar1.ResumeLayout(false);
            guna2CircleProgressBar1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)guna2PictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2CircleProgressBar guna2CircleProgressBar1;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBox1;
        private Label label1;
        private Label label4;
        private Label label3;
        private Label label_val;
        private Label label5;
        private System.Windows.Forms.Timer timer1;
        private Guna.UI2.WinForms.Guna2ShadowForm guna2ShadowForm1;
    }
}