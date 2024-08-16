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
            SuspendLayout();
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location = new Point(12, 38);
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
            loginButton.Size = new Size(75, 27);
            loginButton.TabIndex = 2;
            loginButton.Text = "Login";
            loginButton.Click += LoginButtonClick;
            // 
            // ChatLogin
            // 
            ClientSize = new Size(226, 156);
            Controls.Add(usernameTextBox);
            Controls.Add(passwordTextBox);
            Controls.Add(loginButton);
            Name = "ChatLogin";
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
