using System.ComponentModel;

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
    // The Boot selection menu
    public partial class StartMenu : Form
    {
        #region Default Form Resources (componets / garbage collection)

        private IContainer components = null; // Container to hold the Componets 
        
        /// <summary>
        ///  Clean up any resources being used.
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
        #endregion

        /// <summary>
        /// Inits the Start menu
        /// </summary>
        private void InitializeComponent()
        {
            // Form
            this.components = new Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Gray;
            this.ForeColor = Color.White;
            this.ClientSize = new Size(600, 400);
            this.Text = "App Menu";

            // Title
            Label titleLabel = new Label();
            titleLabel.Text = "Start Menu";
            titleLabel.Font = new Font("OCR A Extended", 36f, FontStyle.Bold);
            titleLabel.BackColor = Color.DimGray;
            titleLabel.Size = new Size(350, 50);
            titleLabel.Location = new Point((this.ClientSize.Width - titleLabel.Width) / 2, 50);
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;

            // Chatting App
            Button chatButton = CreateButton("Chatting App", 150);
            chatButton.Click += new EventHandler(this.ChatAppBtnClick);

            // Trading App
            Button tradeButton = CreateButton("Trading App", 225);
            tradeButton.Click += new EventHandler(this.TradeAppBtnClick);

            // Contact Tracing App
            Button tracingButton = CreateButton("Contact Tracing App", 300);
            tracingButton.Click += new EventHandler(this.TracingAppBtnClick);

            // Add the buttons to the Form
            this.Controls.Add(titleLabel);
            this.Controls.Add(chatButton);
            this.Controls.Add(tradeButton);
            this.Controls.Add(tracingButton);
        }

        #region Button Creation / Hover func
        /// <summary>
        /// Method to make the buttons Sets style / Text / Font Etc.
        /// </summary>
        /// <param name="buttonText"> The buttons display text </param>
        /// <param name="yPos"> the Y position of the BTN </param>
        /// <returns> The Created button </returns>
        private Button CreateButton(string buttonText, int yPos)
        {
            Button button = new Button();
            button.Text = buttonText;
            button.Font = new Font("OCR A Extended", 16f, FontStyle.Bold);
            button.BackColor = Color.Black;
            button.ForeColor = Color.White;
            button.Size = new Size(350, 50);
            button.Location = new Point((this.ClientSize.Width - button.Width) / 2, yPos);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 2;
            button.FlatAppearance.BorderColor = Color.Black;
            button.MouseEnter += new EventHandler(this.ButtonMouseEnter);
            button.MouseLeave += new EventHandler(this.ButtonMouseExit);
            return button;
        }

        /// <summary>
        /// Makes the Buttons Border go Dark red if the mosue hovers the button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.FlatAppearance.BorderColor = Color.DarkRed;
        }

        /// <summary>
        /// Makes the Buttons Border go Black if mouse aint on button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonMouseExit(object sender, EventArgs e)
        {
            Button button = sender as Button;
            button.FlatAppearance.BorderColor = Color.Black;
        }
        #endregion

        #region Application Start Buttons
        /// <summary>
        /// Button to Load the Chatting App...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatAppBtnClick(object sender, EventArgs e)
        {
            var chatLogin = new ChatLogin();
            if (chatLogin.ShowDialog() == DialogResult.OK)
            {
                ChatApp chatApp = new ChatApp(chatLogin.Username, chatLogin.Password);
                chatApp.ShowDialog();
            }
        }

        /// <summary>
        /// Button to Load the Trading App...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TradeAppBtnClick(object sender, EventArgs e)
        {
            // grand exchange trade system or sumshit
        }

        /// <summary>
        /// Button to Load the Tracing App...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TracingAppBtnClick(object sender, EventArgs e)
        {
            // dank tracing of something idk bruh
        }
        #endregion
    }
}
