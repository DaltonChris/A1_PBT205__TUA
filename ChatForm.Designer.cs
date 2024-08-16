namespace PBT_205_A1
{
    partial class ChatForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatForm));
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();
            textBox3 = new TextBox();
            button2 = new Button();
            label1 = new Label();
            button3 = new Button();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.BackColor = Color.Gainsboro;
            textBox1.Font = new Font("Consolas", 11F);
            textBox1.Location = new Point(6, 11);
            textBox1.MinimumSize = new Size(250, 350);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(631, 350);
            textBox1.TabIndex = 0;
            textBox1.TextChanged += textBox1_TextChanged_1;
            // 
            // textBox2
            // 
            textBox2.BackColor = Color.Gainsboro;
            textBox2.Font = new Font("Consolas", 11F);
            textBox2.Location = new Point(6, 367);
            textBox2.MinimumSize = new Size(250, 50);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(555, 50);
            textBox2.TabIndex = 1;
            // 
            // button1
            // 
            button1.BackColor = Color.WhiteSmoke;
            button1.FlatAppearance.BorderColor = Color.Black;
            button1.FlatAppearance.BorderSize = 2;
            button1.FlatAppearance.MouseDownBackColor = Color.White;
            button1.FlatAppearance.MouseOverBackColor = Color.Gray;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            button1.Location = new Point(567, 367);
            button1.Name = "button1";
            button1.Size = new Size(70, 50);
            button1.TabIndex = 2;
            button1.Text = "SEND";
            button1.UseVisualStyleBackColor = false;
            // 
            // textBox3
            // 
            textBox3.BackColor = Color.Gainsboro;
            textBox3.Font = new Font("Consolas", 11F);
            textBox3.Location = new Point(643, 41);
            textBox3.MinimumSize = new Size(140, 320);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(140, 320);
            textBox3.TabIndex = 3;
            // 
            // button2
            // 
            button2.BackColor = Color.WhiteSmoke;
            button2.FlatAppearance.BorderColor = Color.Black;
            button2.FlatAppearance.BorderSize = 2;
            button2.FlatAppearance.MouseDownBackColor = Color.White;
            button2.FlatAppearance.MouseOverBackColor = Color.Gray;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 8.907217F, FontStyle.Bold);
            button2.Location = new Point(653, 319);
            button2.Name = "button2";
            button2.Size = new Size(118, 33);
            button2.TabIndex = 4;
            button2.Text = "LOGOUT";
            button2.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Silver;
            label1.Font = new Font("Consolas", 14.1030931F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(643, 395);
            label1.MinimumSize = new Size(140, 0);
            label1.Name = "label1";
            label1.Size = new Size(140, 22);
            label1.TabIndex = 5;
            label1.Text = "TUA - 2024";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            // 
            // button3
            // 
            button3.BackColor = Color.Transparent;
            button3.BackgroundImageLayout = ImageLayout.None;
            button3.FlatAppearance.BorderColor = Color.Black;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Font = new Font("Consolas", 14.1030931F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button3.Location = new Point(519, 377);
            button3.Margin = new Padding(0);
            button3.Name = "button3";
            button3.Size = new Size(32, 32);
            button3.TabIndex = 6;
            button3.Text = "+";
            button3.UseVisualStyleBackColor = false;
            button3.Click += button3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Silver;
            label2.Font = new Font("Consolas", 16.3298969F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(643, 11);
            label2.MinimumSize = new Size(140, 0);
            label2.Name = "label2";
            label2.Size = new Size(140, 26);
            label2.TabIndex = 7;
            label2.Text = "User List";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImageLayout = ImageLayout.Center;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(567, 292);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(60, 60);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Silver;
            label3.Font = new Font("Consolas", 14.1030931F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(643, 367);
            label3.MinimumSize = new Size(140, 0);
            label3.Name = "label3";
            label3.Size = new Size(140, 22);
            label3.TabIndex = 9;
            label3.Text = "Chatting App";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(795, 427);
            Controls.Add(label3);
            Controls.Add(pictureBox1);
            Controls.Add(label2);
            Controls.Add(button3);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(textBox3);
            Controls.Add(button1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Font = new Font("Consolas", 8.907217F, FontStyle.Bold);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(815, 438);
            Name = "ChatForm";
            Opacity = 0.88D;
            RightToLeft = RightToLeft.No;
            Text = "Chat Now";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;
        private TextBox textBox3;
        private Button button2;
        private Label label1;
        private Button button3;
        private Label label2;
        private PictureBox pictureBox1;
        private Label label3;
    }
}