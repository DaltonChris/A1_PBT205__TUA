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
    public class Tracker
    {
        private Dictionary<string, PositionMarker> _EnvironmentView;
        private Dictionary<string, Dictionary<string, int>> _ContactLog;
        private RabbitMqController _RabbitMqController;

        // Define an event to notify when a position message is received
        public event Action<string, int, int> PositionMessageReceived;

        public Tracker(string username, string password)
        {
            _EnvironmentView = new Dictionary<string, PositionMarker>();
            _ContactLog = new Dictionary<string, Dictionary<string, int>>();
            _RabbitMqController = new RabbitMqController(username, password);
        }

        public void SubscribeToPositionTopic()
        {
            _RabbitMqController.SubscribeToPositions(HandlePositionMessage);
        }

        public void SendQuery(string personIdentifier, out string response)
        {
            response = ProcessQuery(personIdentifier);
        }

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

        private void HandlePositionMessage(string message)
        {
            var parts = message.Split(',');
            var username = parts[0];
            var x = int.Parse(parts[1]);
            var y = int.Parse(parts[2]);


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
