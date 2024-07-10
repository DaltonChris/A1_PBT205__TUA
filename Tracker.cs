using System;
using System.Collections.Generic;
using System.Threading;
using Timer = System.Threading.Timer;

namespace PBT_205_A1
{
    public class Tracker
    {
        private Dictionary<string, PositionMarker> _EnvironmentView;
        private Dictionary<string, List<string>> _ContactLog;
        private RabbitMqController _RabbitMqController;
        private Timer _Timer;
        

        public Tracker(string username, string roomName)
        {
            _EnvironmentView = new Dictionary<string, PositionMarker>();
            _ContactLog = new Dictionary<string, List<string>>();
            _RabbitMqController = new RabbitMqController(username, roomName, "position");
            StartQueryTimer();
        }

        public void StartQueryTimer()
        {
            _Timer = new Timer(ProcessQueries, null, 0, 1000);
        }

        public void SubscribeToPositionTopic()
        {
            _RabbitMqController.SubscribeToPositions(HandlePositionMessage);
        }

        public void SubscribeToQueryTopic()
        {
            _RabbitMqController.SubscribeToPositions(HandleQueryMessage);
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
        }

        private void LogContact(List<string> usersOnTile)
        {
            foreach (var user in usersOnTile)
            {
                if (!_ContactLog.ContainsKey(user))
                {
                    _ContactLog[user] = new List<string>();
                }

                foreach (var otherUser in usersOnTile)
                {
                    if (user != otherUser && !_ContactLog[user].Contains(otherUser))
                    {
                        _ContactLog[user].Add(otherUser);
                    }
                }
            }
        }

        private void HandleQueryMessage(string message)
        {
            var username = message.Trim();
            if (_ContactLog.ContainsKey(username))
            {
                var contacts = _ContactLog[username];
                contacts.Reverse();
                var response = string.Join(",", contacts);
                _RabbitMqController.PublishPosition(response); // Publish to 'query-response' topic
            }
            else
            {
                _RabbitMqController.PublishPosition(""); // Publish empty response to 'query-response' topic
            }
        }

        private void ProcessQueries(object state)
        {
            // check for new queries
        }
    }
}
