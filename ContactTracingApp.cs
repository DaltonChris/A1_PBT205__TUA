using Timer = System.Threading.Timer;

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
        PositionMarker _PositionMarker;
        Timer _MoveTimer;
        int _GridSize;
        public static GridTile[,] _Grid;
        Tracker _Tracker;
        RabbitMqController _RabbitMqController;

        /// <summary>
        /// Constructor
        /// </summary>
        public ContactTracingApp()
        {
            ShowLoginForm();
            InitializeComponent();
            InitializeGrid();
            _Random = new Random();
            _RabbitMqController = new RabbitMqController(_Username, "Room1", "position");
            InitializeTracker();
            PlaceUserRandomly();
            PublishPosition();
            StartMoveTimer();
        }

        /// <summary>
        /// Get users to login
        /// </summary>
        private void ShowLoginForm()
        {
            var loginForm = new ChatLogin();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                _Username = loginForm.Username;
            }
            else
            {
                Environment.Exit(0);
            }
        }

        void InitializeTracker() {
            _Tracker = new Tracker(_Username, "Room1");
            _Tracker.SubscribeToPositionTopic();
            _Tracker.SubscribeToQueryTopic();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // Form settings
            this.ClientSize = new Size(600, 600); 
            this.Name = "ContactTracingApp";
            this.Text = "Contact Tracing App";
            this.ResumeLayout(false);
        }

        /// <summary>
        /// Init the grid by contructing a new GridGen at the given size
        /// </summary>
        private void InitializeGrid()
        {
            _GridSize = 20;
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
        /// The timer / delay of the users update rate pos.
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
            this.Invalidate();
        }

        /// <summary>
        /// Method to randomly select a neighbouring tile to move to
        /// </summary>
        /// <param name="positionMarker"> The Users Marker to be moved </param>
        /// <returns></returns>
        private PositionMarker GetNewPosition(PositionMarker positionMarker)
        {
            var direction = _Random.Next(4);
            var newX = positionMarker.X;
            var newY = positionMarker.Y;

            switch (direction)
            {
                case 0: newX = Math.Max(0, positionMarker.X - 1); break; // Left
                case 1: newX = Math.Min(_GridSize - 1, positionMarker.X + 1); break; // Right
                case 2: newY = Math.Max(0, positionMarker.Y - 1); break; // Up
                case 3: newY = Math.Min(_GridSize - 1, positionMarker.Y + 1); break; // Down
            }

            return new PositionMarker(positionMarker.Username, newX, newY);
        }

        private void PublishPosition()
        {
            // Post pos to rabbit
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

            for (int x = 0; x < _GridSize; x++)
            {
                for (int y = 0; y < _GridSize; y++)
                {
                    GridTile tile = _Grid[x, y];
                    int drawX = x * (tileSize + gap);
                    int drawY = y * (tileSize + gap);

                    graphics.DrawImage(tile.TileSprite, new Rectangle(drawX, drawY, tileSize, tileSize));

                    // draw the users on the tile
                    if (tile.UsersOnTile.Count > 0)
                    {
                        string users = string.Join(",", tile.UsersOnTile);
                        graphics.DrawString(users, this.Font, Brushes.Black, drawX, drawY);
                    }
                }
            }
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
