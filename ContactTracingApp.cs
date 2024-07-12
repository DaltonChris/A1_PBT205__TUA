using System;
using System.Diagnostics;
using Timer = System.Threading.Timer;

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
    /// <summary>
    /// Form Class to Manage / Display the Tracing app
    /// </summary>
    public partial class ContactTracingApp : Form
    {
        GridGenerator _GridGenerator;
        Random _Random;
        string _Username;
        string _Password;
        PositionMarker _PositionMarker;
        Timer _MoveTimer;
        int _GridSize;
        public static GridTile[,] _Grid;
        Tracker _Tracker;
        RabbitMqController _RabbitMqController;

        private TextBox _QueryTextBox;
        private Button _QueryButton;
        private Label _QueryResponse;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactTracingApp(string username, string password)
        {
            this._Password = password;
            this._Username = username;
            InitializeComponent();
            InitializeGrid();
            _Random = new Random();
            _RabbitMqController = new RabbitMqController(_Username, _Password);
            InitializeTracker();
            PlaceUserRandomly();
            PublishPosition();
            StartMoveTimer();
        }

        void InitializeTracker()
        {
            _Tracker = new Tracker(_Username, _Password);
            _Tracker.PositionMessageReceived += UpdateGrid;
            _Tracker.SubscribeToPositionTopic();
        }

        private void InitializeComponent()
        {
            this._QueryTextBox = new TextBox();
            this._QueryButton = new Button();
            this._QueryResponse = new Label();

            this.SuspendLayout();

            // Query TextBox settings
            this._QueryTextBox.Location = new Point(20, 620);
            this._QueryTextBox.Size = new Size(200, 20);
            this._QueryTextBox.Name = "queryTextBox";

            // Send Query Button settings
            this._QueryButton.Location = new Point(240, 620);
            this._QueryButton.Size = new Size(100, 20);
            this._QueryButton.Name = "sendQueryButton";
            this._QueryButton.Text = "Send Query";
            this._QueryButton.Click += new EventHandler(SendQueryButton_Click);

            // Response Label settings
            this._QueryResponse.Location = new Point(20, 660);
            this._QueryResponse.Size = new Size(560, 20);
            this._QueryResponse.Name = "responseLabel";

            // Add controls to the form
            this.Controls.Add(this._QueryTextBox);
            this.Controls.Add(this._QueryButton);
            this.Controls.Add(this._QueryResponse);

            // Form settings
            this.ClientSize = new Size(588, 700);
            this.Name = "ContactTracingApp";
            this.Text = "Contact Tracing App";
            this.ResumeLayout(false);
        }
        private void SendQueryButton_Click(object sender, EventArgs e)
        {
            string personIdentifier = _QueryTextBox.Text;
            if (!string.IsNullOrEmpty(personIdentifier))
            {
                // Directly call Tracker to get the query response
                _Tracker.SendQuery(personIdentifier, out string response);
                UpdateQueryResponse(response);
            }
            else
            {
                _QueryResponse.Text = "Please enter a valid identifier.";
            }
        }

        private void UpdatePositionOnGrid(string username, int x, int y)
        {
            // Update the UI grid with the new position
            this.Invoke((MethodInvoker)delegate {
                // Update the UI elements
            });
        }

        private void UpdateQueryResponse(string responseMessage)
        {
            this.Invoke((MethodInvoker)delegate {
                _QueryResponse.Text = "Query Response: " + responseMessage;
            });
        }

        /// <summary>
        /// Init the grid by constructing a new GridGen at the given size
        /// </summary>
        private void InitializeGrid()
        {
            _GridSize = 4; //28
            _GridGenerator = new GridGenerator(_GridSize);
            _Grid = _GridGenerator.Tiles;
        }

        /// <summary>
        /// Place the user at a random point to begin with
        /// </summary>
        private void PlaceUserRandomly()
        {
            int x = _Random.Next(_GridSize);
            int y = _Random.Next(_GridSize);
            _PositionMarker = new PositionMarker(_Username, x, y);
            _Grid[x, y].AddUser(_Username);
        }

        /// <summary>
        /// The timer / delay of the user's update rate pos.
        /// </summary>
        private void StartMoveTimer()
        {
            // Move user after 2s
            _MoveTimer = new Timer(MoveUser, null, 2000, 2000);
        }

        /// <summary>
        /// Method to move the user to its next tile
        /// </summary>
        /// <param name="state"></param>
        private void MoveUser(object state)
        {
            PositionMarker newPosition = GetNewPosition(_PositionMarker);
            _Grid[_PositionMarker.X, _PositionMarker.Y].RemoveUser(_Username);
            _Grid[newPosition.X, newPosition.Y].AddUser(_Username);
            _PositionMarker = newPosition;
            PublishPosition();
            this.Invalidate();
        }

        /// <summary>
        /// Method to randomly select a neighboring tile to move to
        /// </summary>
        /// <param name="positionMarker"> The User's Marker to be moved </param>
        /// <returns></returns>
        private PositionMarker GetNewPosition(PositionMarker positionMarker)
        {
            int newX, newY;

            do
            {
                var direction = _Random.Next(4);
                newX = positionMarker.X;
                newY = positionMarker.Y;

                switch (direction)
                {
                    case 0: newX = positionMarker.X - 1; break; // Left
                    case 1: newX = positionMarker.X + 1; break; // Right
                    case 2: newY = positionMarker.Y - 1; break; // Up
                    case 3: newY = positionMarker.Y + 1; break; // Down
                }

            } while (newX < 0 || newX >= _GridSize || newY < 0 || newY >= _GridSize);

            return new PositionMarker(positionMarker.Username, newX, newY);
        }

        /// <summary>
        /// Method to post the changed position to rabbitMQ middleware
        /// </summary>
        private void PublishPosition()
        {
            var positionMessage = $"{_Username},{_PositionMarker.X},{_PositionMarker.Y}";
            _RabbitMqController.PublishPosition(positionMessage);
        }

        /// <summary>
        /// Overrides the Form OnPaint Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGrid(e.Graphics);
        }

        /// <summary>
        /// Method to draw the Tiles/Grid on the Ui/Form
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawGrid(Graphics graphics)
        {
            int tileSize = 20;
            int gap = 1;
            int circleDiameter = tileSize; // Set the diameter of the circle

            for (int x = 0; x < _GridSize; x++)
            {
                for (int y = 0; y < _GridSize; y++)
                {
                    GridTile tile = _Grid[x, y];
                    int drawX = x * (tileSize + gap);
                    int drawY = y * (tileSize + gap);

                    // Draw the tile image
                    graphics.DrawImage(tile.TileSprite, new Rectangle(drawX, drawY, tileSize, tileSize));

                    // Draw the users on the tile
                    if (tile.UsersOnTile.Count > 0)
                    {
                        // Draw the circle
                        graphics.FillEllipse(Brushes.LightGreen, drawX, drawY, circleDiameter, circleDiameter);

                        // Draw the user text
                        string users = string.Join(",", tile.UsersOnTile);
                        SizeF textSize = graphics.MeasureString(users, this.Font);
                        float textX = drawX + (circleDiameter - textSize.Width) / 2;
                        float textY = drawY + (circleDiameter - textSize.Height) / 2;
                        graphics.DrawString(users, this.Font, Brushes.Black, textX, textY);
                    }
                }
            }
        }

        /// <summary>
        /// Udates the Position marker to the changed tile
        /// </summary>
        /// <param name="username"> User marker to move <param>
        /// <param name="x"> new x pos </param>
        /// <param name="y"> new y pos </param>
        private void UpdateGrid(string username, int x, int y)
        {
            foreach (var tile in _Grid)
            {
                tile.RemoveUser(username);
            }
            _Grid[x, y].AddUser(username);
            this.Invalidate();
        }
    }

    /// <summary>
    /// Class to Generate a grid of GridTiles
    /// </summary>
    class GridGenerator
    {
        public GridTile[,] Tiles { get; private set; }

        public GridGenerator(int size)
        {
            Tiles = new GridTile[size, size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Tiles[x, y] = new GridTile(x, y);
                }
            }
        }
    }

    /// <summary>
    /// Class for each Tile obj in the Grid
    /// </summary>
    public class GridTile
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Size { get; private set; }
        public Image TileSprite { get; private set; }

        public List<string> UsersOnTile { get; private set; }

        public GridTile(int x, int y)
        {
            X = x;
            Y = y;
            UsersOnTile = new List<string>();
            TileSprite = new Bitmap(Resources.Back_Tile); // Tile Image
        }

        public void AddUser(string user)
        {
            if (!UsersOnTile.Contains(user))
            {
                UsersOnTile.Add(user);
            }
        }

        public void RemoveUser(string user)
        {
            if (UsersOnTile.Contains(user))
            {
                UsersOnTile.Remove(user);
            }
        }
    }

    /// <summary>
    /// Class to mark a user on a given Tile
    /// </summary>
    class PositionMarker
    {
        public string Username { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public PositionMarker(string user, int x, int y)
        {
            Username = user;
            X = x;
            Y = y;
        }

        public void Move(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }
    }
}
