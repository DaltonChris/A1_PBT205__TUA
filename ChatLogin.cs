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

            ClientSize = new Size(300, 200);
            Text = "Login";

            // Username
            usernameTextBox.Location = new Point(50, 50);
            usernameTextBox.Size = new Size(200, 30);
            usernameTextBox.PlaceholderText = "Username";
            Controls.Add(usernameTextBox);

            // Password
            passwordTextBox.Location = new Point(50, 100);
            passwordTextBox.Size = new Size(200, 30);
            passwordTextBox.UseSystemPasswordChar = true;
            passwordTextBox.PlaceholderText = "Password";
            Controls.Add(passwordTextBox);

            // Button
            loginButton.Text = "Login";
            loginButton.Location = new Point(100, 150);
            loginButton.Click += new EventHandler(LoginButtonClick);
            Controls.Add(loginButton);
        }

        private void LoginButtonClick(object sender, EventArgs e)
        {
            Username = usernameTextBox.Text;
            Password = passwordTextBox.Text;

            if (ValidateCredentials(Username, Password))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateCredentials(string username, string password)
        {
            // You can add your actual validation logic here
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }
    }
}
