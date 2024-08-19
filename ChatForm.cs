﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
/*
  ##################################################################
  ### Dalton Christopher - ID: A00122255                         ###
  ### TUA - PBT205—Project-based Learning Studio: Technology     ###
  ### - Assessment - 3                                           ###
  ### - 08/2024                                                  ###
  ##################################################################
*/
namespace PBT_205_A1
{
    public partial class ChatForm : Form
    {
        IConnection? _Connection;
        IModel? _Channel;

        readonly string _HostName = "localhost";
        readonly string _RoutingKey = "chat_room";
        readonly string _ExchangeName = "chat_exchange";
        string _QueueName;
        string _Username;
        string _Password;


        public ChatForm(string username, string password)
        {
            this._Username = username;
            this._Password = password;
            this._QueueName = "chat_queue_" + username; // Unique queue for each user
            InitializeComponent();
            InitRabbitMQ();
            StartChatroom();

            // Set the DrawMode of the ListBox
            ChatListBox.DrawItem += ChatListBox_DrawItem;
            ChatListBox.MeasureItem += ChatListBox_MeasureItem;

            NotifyUserJoined();

        }

        /// <summary>
        /// Inits RabbitMQ connection and channel.
        /// </summary>
        private void InitRabbitMQ()
        {
            try // Try set up the RabbitMq connection
            {
                var factory = new ConnectionFactory
                {
                    HostName = _HostName,
                    UserName = _Username,
                    Password = _Password
                };

                _Connection = factory.CreateConnection();
                _Channel = _Connection.CreateModel();
                _Channel.ExchangeDeclare(exchange: _ExchangeName, type: ExchangeType.Topic);

                // Unique queue for each client
                _Channel.QueueDeclare(queue: _QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: true,  // Auto-delete the queue when user exits
                                     arguments: null);

                // Bind queue to the Rabbits exchange using chatroom routing key
                _Channel.QueueBind(queue: _QueueName,
                                  exchange: _ExchangeName,
                                  routingKey: _RoutingKey);
            }
            catch
            {
                MessageBox.Show($"Error connecting: Couldn't Connect to RabbitMq");
            }
        }

        /// <summary>
        /// Starts receiving messages from the RabbitMQ queue.
        /// </summary>
        private void StartChatroom()
        {
            try
            {
                if (_Channel == null)
                {
                    MessageBox.Show("RabbitMQ channel is not initialized.");
                    return;
                }

                var consumer = new EventingBasicConsumer(_Channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Invoke((Action)(() =>
                    {
                        // Check for join notification
                        if (message.Contains("has joined the chat."))
                        {
                            string username = message.Split(' ')[0];
                            if (!UsersListBox.Items.Contains(username))
                            {
                                UsersListBox.Items.Add(username);
                            }
                        }

                        // If not a Joined msg, add the msg to the chat list box
                        ChatListBox.Items.Add(message);
                        // & set the topIndex to the newest msg to keep the new chats visable
                        ChatListBox.TopIndex = ChatListBox.Items.Count - 1;
                    }));
                };
                _Channel.BasicConsume(queue: _QueueName, autoAck: true, consumer: consumer);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting chat-room: {ex.Message}");
            }
        }


        // Handle custom drawing of items in the ListBox
        private void ChatListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = ChatListBox.Items[e.Index].ToString();

            // Check for a image
            int imgIndex = item.IndexOf("IMG:");
            if (imgIndex != -1)
            {
                // add the username
                string usernamePart = item.Substring(0, imgIndex);
                e.Graphics.DrawString(usernamePart, e.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y);

                // Draw the image
                string base64String = item.Substring(imgIndex + 4); // Skip "IMG:"
                byte[] imageBytes = Convert.FromBase64String(base64String);
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    Image image = Image.FromStream(ms);

                    // Calculate the aspect ratio
                    float aspectRatio = (float)image.Width / image.Height;
                    int newWidth = 125;
                    int newHeight = 125;

                    // Scale down keeping aspect ratio
                    if (aspectRatio > 1){
                        newHeight = (int)(125 / aspectRatio);
                    }
                    else{
                        newWidth = (int)(125 * aspectRatio);
                    }

                    // Position the image within the bounds
                    int x = e.Bounds.X + (125 - newWidth) / 2 + 75; // Add offset 
                    int y = e.Bounds.Y + (125 - newHeight) / 2; // add padding

                    e.Graphics.DrawImage(image, x, y, newWidth, newHeight);
                }
            }
            else
            {
                // Draw the text
                e.Graphics.DrawString(item, e.Font, Brushes.Black, e.Bounds);
            }
        }

        /// <summary>
        /// Method to work out the sizing of each item for the chat list box, This is so when a user sends a image
        /// It can be properly formatted to fit within the other msgs, or imgs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChatListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0) return;

            var item = ChatListBox.Items[e.Index].ToString();

            if (item.Contains("IMG:"))
            {
                // Get height based on image size and username text
                int textHeight = (int)e.Graphics.MeasureString(_Username, 
                                        ChatListBox.Font, ChatListBox.Width).Height;
                int imageHeight = 100; // Fixed image height

                e.ItemHeight = textHeight + imageHeight + 1; // Add some padding
            }
            else
            {
                // Adjust the height for text msgs
                e.ItemHeight = (int)e.Graphics.MeasureString(item, 
                    ChatListBox.Font, ChatListBox.Width).Height + 1; // Add padding
            }
        }

        /// <summary>
        /// Send Msg click button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(object sender, EventArgs e)
        {
            if (_Channel == null)
            {
                MessageBox.Show("Channel is not initialized.");
                return;
            }

            // Ensure the messageTextBox has text
            if (!string.IsNullOrWhiteSpace(messageTextBox.Text))
            {
                string message = messageTextBox.Text;
                string fullMessage = $"{_Username}: {message}";
                var body = Encoding.UTF8.GetBytes(fullMessage);
                _Channel.BasicPublish(exchange: _ExchangeName, 
                                    routingKey: _RoutingKey, 
                                    basicProperties: null, 
                                    body: body);
                messageTextBox.Clear();
                ChatListBox.TopIndex = ChatListBox.Items.Count - 1;
            }
        }


        /// <summary>
        /// Updates the users list in the usersTextBox.
        /// </summary>
        public void UpdateUsersList(string[] users)
        {
            UsersListBox.Text = "Online:\n";// Online header
            foreach (var user in users) // For each user online
            {
                UsersListBox.Text += $"{user}\n"; // Add to list
            }
            UsersListBox.Text += "--------\nOffline:\n"; // Offline header
        }
        private void NotifyUserJoined()
        {
            if (_Channel == null) return;

            string joinMessage = $"{_Username} has joined the chat.";
            var body = Encoding.UTF8.GetBytes(joinMessage);
            _Channel.BasicPublish(exchange: _ExchangeName, 
                                routingKey: _RoutingKey, 
                                basicProperties: null, 
                                body: body);
        }

        /// <summary>
        /// Method for users to attach images to be sent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AttachButton_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog();

            // Filter valid image types for attachments
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            openFileDialog.Title = "Select an Image"; // Dialog box title

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;// Get the file's path

                // Convert img to a byte array
                byte[] imageBytes = File.ReadAllBytes(filePath);
                string base64String = Convert.ToBase64String(imageBytes); // byte array to Base64 string

                string message = $"{_Username}: IMG:{base64String}"; // msg with the username and image
                                                                     // Send encoded img as a message
                var body = Encoding.UTF8.GetBytes(message);
                _Channel.BasicPublish(exchange: _ExchangeName, 
                                    routingKey: _RoutingKey, 
                                    basicProperties: null, 
                                    body: body);

                //MessageBox.Show("Image sent successfully!"); // For Testing
            }
        }

        /// <summary>
        /// Enter-Key Down event for the messageTextBox.
        /// </summary>
        private void MessageTextBoxKeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // if enter is pressed
            {
                SendButton_Click(this,e); // send msg
                e.SuppressKeyPress = true; // Stop annoying ding sound
            }
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            // Add a msg for others that the user has left
            if (_Channel != null)
            {
                string leaveMessage = $"{_Username} has left the chat.";
                var body = Encoding.UTF8.GetBytes(leaveMessage);
                _Channel.BasicPublish(exchange: _ExchangeName, 
                                    routingKey: _RoutingKey, 
                                    basicProperties: null, 
                                    body: body);
            }
            if (_Channel != null) // Close the Channel
            {
                _Channel.Close();
                _Channel.Dispose();
                _Channel = null;
            }
            if (_Connection != null) // Close the connection
            {
                _Connection.Close();
                _Connection.Dispose();
                _Connection = null;
            }
            this.Close(); // Close the chat form
            Application.Exit(); // Close tha app
        }
    }
}
