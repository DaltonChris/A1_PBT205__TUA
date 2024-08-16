/*
  ##################################################################
  ### Dalton Christopher - ID: A00122255                         ###
  ### TUA - PBT205—Project-based Learning Studio: Technology     ###
  ### - Assesment - 1                                            ###
  ### - 06/2024                                                  ###
  ##################################################################
*/

namespace PBT_205_A1
{
    public partial class ChatLogin : Form
    {
        TextBox usernameTextBox;
        TextBox passwordTextBox;
        private Label label1;
        private Button button1;
        Button loginButton;

        public string Username { get; private set; }
        public string Password { get; private set; }

        public ChatLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            usernameTextBox = new TextBox();
            passwordTextBox = new TextBox();
            loginButton = new Button();
            label1 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(12, 48);
            usernameTextBox.Name = "usernameTextBox";
            usernameTextBox.PlaceholderText = "Username";
            usernameTextBox.Size = new Size(200, 23);
            usernameTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location = new Point(12, 77);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.PlaceholderText = "Password";
            passwordTextBox.Size = new Size(200, 23);
            passwordTextBox.TabIndex = 1;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // loginButton
            // 
            loginButton.Location = new Point(12, 116);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(90, 27);
            loginButton.TabIndex = 2;
            loginButton.Text = "Login";
            loginButton.Click += LoginButtonClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Consolas", 14.1030931F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(90, 22);
            label1.TabIndex = 3;
            label1.Text = "Chat Now";
            // 
            // button1
            // 
            button1.Location = new Point(122, 116);
            button1.Name = "button1";
            button1.Size = new Size(90, 28);
            button1.TabIndex = 4;
            button1.Text = "SignUp";
            // 
            // ChatLogin
            // 
            ClientSize = new Size(226, 156);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(usernameTextBox);
            Controls.Add(passwordTextBox);
            Controls.Add(loginButton);
            MaximumSize = new Size(242, 195);
            MinimumSize = new Size(242, 195);
            Name = "ChatLogin";
            Opacity = 0.95D;
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        private void LoginButtonClick(object sender, EventArgs e)
        {
            Username = usernameTextBox.Text;
            Password = passwordTextBox.Text;

            if (ValidateLogin(Username, Password))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        static bool ValidateLogin(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }
    }
}
