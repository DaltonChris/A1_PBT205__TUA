using Timer = System.Threading.Timer;

namespace PBT_205_A1
{
    public partial class ContactTracingApp : Form
    {
        GridGenerator _GridGenerator;
        Random _Random;
        string _Username;
        PositionMarker _PositionMarker;
        Timer _MoveTimer;
        int _GridSize;
        GridTile[,] _Grid;

        public ContactTracingApp()
        {
            ShowLoginForm();
            InitializeComponent();
            InitializeGrid();
            _Random = new Random();

            PlaceUserRandomly();
            PublishPosition();
            StartMoveTimer();
        }

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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // Form settings
            this.ClientSize = new Size(600, 600); 
            this.Name = "ContactTracingApp";
            this.Text = "Contact Tracing App";
            this.ResumeLayout(false);
        }

        private void InitializeGrid()
        {
            _GridSize = 20;
            _GridGenerator = new GridGenerator(_GridSize);
            _Grid = _GridGenerator.Tiles;
        }

        private void PlaceUserRandomly()
        {
            int x = _Random.Next(_GridSize);
            int y = _Random.Next(_GridSize);
            _PositionMarker = new PositionMarker(_Username, x, y);
            _Grid[x, y].AddUser(_Username);
        }

        private void StartMoveTimer()
        {
            _MoveTimer = new Timer(MoveUser, null, 2000, 2000);
        }

        private void MoveUser(object state)
        {
            PositionMarker newPosition = GetNewPosition(_PositionMarker);
            _Grid[_PositionMarker.X, _PositionMarker.Y].RemoveUser(_Username);
            _Grid[newPosition.X, newPosition.Y].AddUser(_Username);
            _PositionMarker = newPosition;
            this.Invalidate();
        }

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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawGrid(e.Graphics);
        }

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

    class GridTile
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
