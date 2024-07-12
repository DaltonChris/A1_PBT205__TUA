using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

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
    /// <summary>
    /// 
    /// </summary>
    public class Tracker
    {
        private Dictionary<string, PositionMarker> _EnvironmentView;
        private Dictionary<string, Dictionary<string, int>> _ContactLog;
        private RabbitMqController _RabbitMqController;
        ContactTracingApp _ContactTracingApp;

        // Event to notify when a position message is received
        public event Action<string, int, int> PositionMessageReceived;
        // Event to notify when a position message is received
        public event Action<string> QueryMessageReceived;

        public event Action<int> GridSizeUpdated; // Grid size changed event

        public Tracker(string username, string password, ContactTracingApp contactTracingApp) // Constructor
        {
            _EnvironmentView = new Dictionary<string, PositionMarker>();
            _ContactLog = new Dictionary<string, Dictionary<string, int>>();
            _RabbitMqController = new RabbitMqController(username, password);
            _ContactTracingApp = contactTracingApp;
        }

        public void SendGridSizeUpdate(int newSize)
        {
            _RabbitMqController.PublishPosition($"GRID_SIZE,{newSize}");
        }


        /// <summary>
        /// 
        /// </summary>
        public void SubscribeToPositionTopic()
        {
            _RabbitMqController.SubscribeToPositions(HandlePositionMessage);
        }
        /// <summary>
        /// 
        /// </summary>
        public void SubscribeToQueryTopic()
        {
            _RabbitMqController.SubscribeToQuery(OpenQuery);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personIdentifier"></param>
        /// <param name="response"></param>
        public void SendQuery(string personIdentifier)
        {
            _RabbitMqController.PublishQuery(personIdentifier);
        }
        public void OpenQuery(string message)
        {
            string response = ProcessQuery(message);
            QueryMessageReceived?.Invoke(response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personIdentifier"></param>
        /// <returns></returns>
        private string ProcessQuery(string personIdentifier)
        {
            var username = personIdentifier.Trim();
            if (_ContactLog.ContainsKey(username))
            {
                var contacts = _ContactLog[username];
                List<string> contactStrings = new List<string>();

                foreach (var contact in contacts)
                {
                    string contactString = $"{username} has contacted {contact.Key} {contact.Value} time(s)";
                    contactStrings.Add(contactString);
                }

                return string.Join("\n", contactStrings);
            }
            else
            {
                return "No contacts found.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void HandlePositionMessage(string message)
        {
            var parts = message.Split(',');

            if (parts[0] == "GRID_SIZE")
            {
                int newSize = int.Parse(parts[1]);
                GridSizeUpdated?.Invoke(newSize);
                return;
            }

            var username = parts[0];
            var x = int.Parse(parts[1]);
            var y = int.Parse(parts[2]);

            // Check if the new position is within bounds before updating
            if (x < 0 || x >= ContactTracingApp._GridSize || y < 0 || y >= ContactTracingApp._GridSize)
            {
                // Log or handle out-of-bounds error
                Debug.WriteLine($"Received out-of-bounds position update for user {username}: ({x}, {y})");
                return; // Skip this update
            }

            if (_EnvironmentView.ContainsKey(username))
            {
                var oldPosition = _EnvironmentView[username];
                var oldTile = ContactTracingApp._Grid[oldPosition.X, oldPosition.Y];
                oldTile.RemoveUser(username);
            }

            var newPosition = new PositionMarker(username, x, y);
            _EnvironmentView[username] = newPosition;
            var newTile = ContactTracingApp._Grid[x, y];
            newTile.AddUser(username);

            if (newTile.UsersOnTile.Count > 1)
            {
                LogContact(newTile.UsersOnTile);
            }

            // Raise the event -- position update
            PositionMessageReceived?.Invoke(username, x, y);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usersOnTile"></param>
        private void LogContact(List<string> usersOnTile)
        {
            foreach (var user in usersOnTile)
            {
                if (!_ContactLog.ContainsKey(user))
                {
                    _ContactLog[user] = new Dictionary<string, int>();
                }

                foreach (var otherUser in usersOnTile)
                {
                    if (user != otherUser)
                    {
                        if (!_ContactLog[user].ContainsKey(otherUser))
                        {
                            _ContactLog[user][otherUser] = 0;
                        }
                        _ContactLog[user][otherUser]++;
                    }
                }
            }
        }
    }
}
