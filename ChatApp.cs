using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Windows.Forms;

/*
  ##################################################################
  ### Dalton Christopher - ID: A00122255                         ###
  ### TUA - PBT205—Project-based Learning Studio: Technology     ###
  ### - Assessment - 1                                           ###
  ### - 06/2024                                                  ###
  ##################################################################
*/

namespace PBT_205_A1
{
    public partial class ChatApp : Form
    {
        #region Windows Form Designer generated code
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            connection?.Close();
        }
        private System.ComponentModel.IContainer components = null;
        #endregion

        IConnection? connection;
        IModel? channel;

        readonly string exchangeName = "chat_exchange";
        string queueName;
        readonly string routingKey = "chat_room";
        string username;
        string password;
        readonly string hostName = "localhost";

        readonly Color textColour = ColorTranslator.FromHtml("#03FF00");
        readonly Color textColourAlt = ColorTranslator.FromHtml("#0E9901");
        readonly Color backgroundColour = ColorTranslator.FromHtml("#1E1E1E");
        readonly Color clientColour = ColorTranslator.FromHtml("#5B5B5B");

        TextBox messageTextBox;
        ListBox chatListBox;
        Button sendButton;
        TextBox usersTextBox;

        public ChatApp(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.queueName = "chat_queue_" + username; // Unique queue for each user

            InitializeComponent();
            InitRabbitMQ();
            StartChatroom();
        }

        /// <summary>
        /// Init UI
        /// </summary>
        void InitializeComponent()
        {
            messageTextBox = new TextBox();
            chatListBox = new ListBox();
            sendButton = new Button();
            usersTextBox = new TextBox();

            // Form Settings
            ClientSize = new Size(800, 440);
            BackColor = clientColour; // Set form background color
            ForeColor = textColour;
            Font = new Font("OCR A Extended", 18f, FontStyle.Bold);
            Controls.Add(messageTextBox);
            Controls.Add(chatListBox);
            Controls.Add(sendButton);
            Controls.Add(usersTextBox); // Add the users TextBox
            Text = "Chatting App";

            // Chat ListBox
            chatListBox.Location = new Point(10, 12);
            chatListBox.Size = new Size(670, 390);

            // Users TextBox
            usersTextBox.Location = new Point(690, 12);
            usersTextBox.Size = new Size(100, 380);
            usersTextBox.Multiline = true;
            usersTextBox.BackColor = backgroundColour;
            usersTextBox.ForeColor = textColourAlt;
            usersTextBox.BorderStyle = BorderStyle.Fixed3D;
            usersTextBox.ReadOnly = true;
            usersTextBox.Font = new Font("OCR A Extended", 12f, FontStyle.Italic);
            usersTextBox.Text = "Online:\n";
            usersTextBox.Text = usersTextBox.Text += $"{username}\n" + "--------\n Offline:\n";

            // Users-Message TextBox
            messageTextBox.Location = new Point(10, 400);
            messageTextBox.Size = new Size(670, 35);
            messageTextBox.KeyDown += new KeyEventHandler(MessageTextBoxKeyDown);
            // Send Button
            sendButton.Text = "Send";
            sendButton.Size = new Size(100, 35);
            sendButton.Location = new Point(690, 400);
            sendButton.Click += new EventHandler(SendButtonClick);

            // Additional UI Settings
            messageTextBox.BackColor = backgroundColour;
            messageTextBox.BorderStyle = BorderStyle.Fixed3D;
            messageTextBox.ForeColor = textColourAlt;
            chatListBox.BackColor = backgroundColour;
            chatListBox.ForeColor = textColour;
            sendButton.BackColor = backgroundColour;
            sendButton.ForeColor = textColour;
        }

        /// <summary>
        /// Inits RabbitMQ connection and channel.
        /// </summary>
        private void InitRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = hostName,
                    UserName = username,
                    Password = password
                };

                connection = factory.CreateConnection();
                channel = connection.CreateModel();

                // Declare an exchange of type 'topic'
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                // Declare a unique queue for each client
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: true,  // Auto-delete the queue when user exits
                                     arguments: null);

                // Bind the queue to the Rabbits exchange using chatroom routing key
                channel.QueueBind(queue: queueName,
                                  exchange: exchangeName,
                                  routingKey: routingKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting: {ex.Message}");
            }
        }

        /// <summary>
        /// Starts receiving messages from the RabbitMQ queue.
        /// </summary>
        private void StartChatroom()
        {
            try
            {
                if (channel == null)
                {
                    MessageBox.Show("RabbitMQ channel is not initialized.");
                    return;
                }

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Invoke((Action)(() =>
                    {
                        chatListBox.Items.Add(message);
                    }));
                };
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting chat-room: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a message to the RabbitMQ exchange.
        /// </summary>
        private void SendMessage()
        {
            string? message = null;
            if (channel == null)
            {
                MessageBox.Show("Channel is not initialized.");
                return;
            }

            if (messageTextBox != null)
                message = messageTextBox.Text;
            if (string.IsNullOrWhiteSpace(message)) return;
            else
            {
                string fullMessage = $"{username}: {message}";
                var body = Encoding.UTF8.GetBytes(fullMessage);
                channel.BasicPublish(exchange: exchangeName, routingKey: routingKey, basicProperties: null, body: body);
                messageTextBox.Clear();
            }
        }

        /// <summary>
        /// Enter-Key Down event for the messageTextBox.
        /// </summary>
        private void MessageTextBoxKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // if enter is pressed
            {
                SendMessage(); // send msg
                e.SuppressKeyPress = true; // Stop annoying ding sound
            }
        }

        /// <summary>
        /// Click event for the sendButton.
        /// </summary>
        private void SendButtonClick(object? sender, EventArgs e)
        {
            SendMessage();
        }

        /// <summary>
        /// Updates the users list in the usersTextBox.
        /// </summary>
        public void UpdateUsersList(string[] users)
        {
            usersTextBox.Text = "Online:\n";// Online header
            foreach (var user in users) // For each user online
            {
                usersTextBox.Text += $"{user}\n"; // Add to list
            }
            usersTextBox.Text += "--------\nOffline:\n"; // Offline header
        }
    }
}
