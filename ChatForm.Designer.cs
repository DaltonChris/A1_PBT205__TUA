﻿namespace PBT_205_A1
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
            MessageTextBox = new TextBox();
            SendButton = new Button();
            LogoutButton = new Button();
            label1 = new Label();
            AttachButton = new Button();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            label3 = new Label();
            ChatListBox = new ListBox();
            UsersListBox = new ListBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // MessageTextBox
            // 
            MessageTextBox.BackColor = SystemColors.Window;
            MessageTextBox.Font = new Font("Consolas", 11F);
            MessageTextBox.Location = new Point(6, 367);
            MessageTextBox.MinimumSize = new Size(250, 50);
            MessageTextBox.Name = "MessageTextBox";
            MessageTextBox.Size = new Size(555, 50);
            MessageTextBox.TabIndex = 1;
            // 
            // SendButton
            // 
            SendButton.BackColor = Color.WhiteSmoke;
            SendButton.FlatAppearance.BorderColor = Color.Black;
            SendButton.FlatAppearance.BorderSize = 2;
            SendButton.FlatAppearance.MouseDownBackColor = Color.White;
            SendButton.FlatAppearance.MouseOverBackColor = Color.Gray;
            SendButton.FlatStyle = FlatStyle.Flat;
            SendButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            SendButton.Location = new Point(567, 367);
            SendButton.Name = "SendButton";
            SendButton.Size = new Size(70, 50);
            SendButton.TabIndex = 2;
            SendButton.Text = "SEND";
            SendButton.UseVisualStyleBackColor = false;
            SendButton.Click += SendButton_Click;
            // 
            // LogoutButton
            // 
            LogoutButton.BackColor = Color.WhiteSmoke;
            LogoutButton.FlatAppearance.BorderColor = Color.Black;
            LogoutButton.FlatAppearance.BorderSize = 2;
            LogoutButton.FlatAppearance.MouseDownBackColor = Color.White;
            LogoutButton.FlatAppearance.MouseOverBackColor = Color.Gray;
            LogoutButton.FlatStyle = FlatStyle.Flat;
            LogoutButton.Font = new Font("Segoe UI", 8.907217F, FontStyle.Bold);
            LogoutButton.Location = new Point(652, 319);
            LogoutButton.Name = "LogoutButton";
            LogoutButton.Size = new Size(118, 33);
            LogoutButton.TabIndex = 4;
            LogoutButton.Text = "LOGOUT";
            LogoutButton.UseVisualStyleBackColor = false;
            LogoutButton.Click += LogoutButton_Click;
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
            // 
            // AttachButton
            // 
            AttachButton.BackColor = Color.White;
            AttachButton.BackgroundImageLayout = ImageLayout.None;
            AttachButton.FlatAppearance.BorderColor = Color.Black;
            AttachButton.FlatStyle = FlatStyle.Flat;
            AttachButton.Font = new Font("Consolas", 14.1030931F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AttachButton.Location = new Point(518, 377);
            AttachButton.Margin = new Padding(0);
            AttachButton.Name = "AttachButton";
            AttachButton.Size = new Size(33, 33);
            AttachButton.TabIndex = 6;
            AttachButton.Text = "+";
            AttachButton.UseVisualStyleBackColor = false;
            AttachButton.Click += AttachButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Silver;
            label2.Font = new Font("Consolas", 16.3298969F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(643, 6);
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
            // ChatListBox
            // 
            ChatListBox.FormattingEnabled = true;
            ChatListBox.ItemHeight = 14;
            ChatListBox.Location = new Point(6, 7);
            ChatListBox.Name = "ChatListBox";
            ChatListBox.Size = new Size(631, 354);
            ChatListBox.TabIndex = 10;
            // 
            // UsersListBox
            // 
            UsersListBox.BackColor = SystemColors.ScrollBar;
            UsersListBox.FormattingEnabled = true;
            UsersListBox.ItemHeight = 14;
            UsersListBox.Location = new Point(643, 35);
            UsersListBox.Name = "UsersListBox";
            UsersListBox.Size = new Size(140, 326);
            UsersListBox.TabIndex = 11;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.DimGray;
            ClientSize = new Size(795, 427);
            Controls.Add(pictureBox1);
            Controls.Add(ChatListBox);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(AttachButton);
            Controls.Add(label1);
            Controls.Add(LogoutButton);
            Controls.Add(SendButton);
            Controls.Add(MessageTextBox);
            Controls.Add(UsersListBox);
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
        private TextBox MessageTextBox;
        private Button SendButton;
        private Button LogoutButton;
        private Label label1;
        private Button AttachButton;
        private Label label2;
        private PictureBox pictureBox1;
        private Label label3;
        private ListBox ChatListBox;
        private ListBox UsersListBox;
    }
}