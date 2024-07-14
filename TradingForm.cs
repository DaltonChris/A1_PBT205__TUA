using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace PBT_205_A1
{
    public partial class TradingForm : Form
    {
        private RabbitMqController rabbitMqController; // Controller for RabbitMQ operations
        private TextBox txtUsername; // TextBox for entering the username
        private ComboBox cmbStocks; // ComboBox for selecting stocks
        private TextBox txtBuyPrice; // TextBox for entering the buy price
        private TextBox txtSellPrice; // TextBox for entering the sell price
        private Button btnBuySubmit; // Button to submit a buy order
        private Button btnSellSubmit; // Button to submit a sell order
        private ListBox lstBuyOrders; // ListBox to display buy orders
        private ListBox lstSellOrders; // ListBox to display sell orders
        private Button btnBuyOrder; // Button for buy order
        private Button btnSellOrder; // Button for sell order
        private Button btnClearOrders; // Button to clear all existing orders
        private ListView lvStockPrices; // ListView to display stock prices
        private Dictionary<string, Dictionary<string, int>> userStocks; // Dictionary to store user stocks
        private const string OrdersFilePath = "orders.json"; // File path for storing orders
        private List<Order> orders; // List to store orders
        private Dictionary<string, double> stockPrices; // Dictionary to store stock prices

        // Constructor for TradingForm
        public TradingForm()
        {
            // Initialize the form components
            InitializeComponent();
            // Initialize the RabbitMQ controller with default credentials
            rabbitMqController = new RabbitMqController("guest", "guest");
            // Initialize the user stocks dictionary
            userStocks = new Dictionary<string, Dictionary<string, int>>();
            // Load orders from the file
            orders = LoadOrders();
            // Populate the buy and sell order lists
            PopulateOrderLists();
            // Populate the stock prices list view
            PopulateStockPrices();
        }     

        // Initializes the components of the TradingForm
        private void InitializeComponent()
        {
            // Set form properties
            this.Text = "Trading Form";
            this.Size = new System.Drawing.Size(1000, 600);

            // Username label and TextBox
            Label lblUsername = new Label() { Text = "Username", Location = new System.Drawing.Point(20, 20) };
            txtUsername = new TextBox() { Location = new System.Drawing.Point(150, 20), Width = 200 };

            // Stock selection label and ComboBox
            Label lblStocks = new Label() { Text = "Select Stock", Location = new System.Drawing.Point(20, 60) };
            cmbStocks = new ComboBox() { Location = new System.Drawing.Point(150, 60), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            // Dictionary of stocks with prices (prices updated 14/07/24)
            stockPrices = new Dictionary<string, double>
            {
                { "NVIDIA Corporation (NVDA)", 129.24 },
                { "Lucid Group, Inc. (LCID)", 4.2500 },
                { "Tesla, Inc. (TSLA)", 248.23 },
                { "SoundHound AI, Inc. (SOUN)", 6.20 },
                { "Rivian Automotive, Inc. (RIVN)", 18.11 },
                { "Ford Motor Company (F)", 14.03 },
                { "QuantumScape Corporation (QS)", 8.25 },
                { "Intel Corporation (INTC)", 34.49 },
                { "Marathon Digital Holdings, Inc. (MARA)", 20.77 },
                { "NIO Inc. (NIO)", 4.8700 },
                { "Advanced Micro Devices, Inc. (AMD)", 181.61 },
                { "Wells Fargo & Company (WFC)", 56.54 },
                { "Plug Power Inc. (PLUG)", 3.0700 },
                { "Apple Inc. (AAPL)", 230.54 },
                { "Arbor Realty Trust, Inc. (ABR)", 12.89 },
                { "AT&T Inc. (T)", 18.81 },
                { "Telefonaktiebolaget LM Ericsson (publ) (ERIC)", 6.68 },
                { "SoFi Technologies, Inc. (SOFI)", 6.97 },
                { "American Airlines Group Inc. (AAL)", 10.68 },
                { "CleanSpark, Inc. (CLSK)", 15.85 },
                { "Palantir Technologies Inc. (PLTR)", 28.07 },
                { "Amazon.com, Inc. (AMZN)", 194.49 },
                { "AGNC Investment Corp. (AGNC)", 10.45 },
                { "Citigroup Inc. (C)", 64.52 },
                { "Bank of America Corporation (BAC)", 41.59 }
            };

            // Populate ComboBox with stock names
            cmbStocks.Items.AddRange(stockPrices.Keys.ToArray());

            // Tab control for Buy and Sell tabs
            TabControl tabControl = new TabControl() { Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(450, 150) };

            // Buy and Sell tabs
            TabPage tabBuy = new TabPage("Buy");
            TabPage tabSell = new TabPage("Sell");

            // Add tabs to tab control
            tabControl.TabPages.Add(tabBuy);
            tabControl.TabPages.Add(tabSell);

            // Add controls to the form
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblStocks);
            this.Controls.Add(cmbStocks);
            this.Controls.Add(tabControl);

            // Buy tab controls
            Label lblBuyPrice = new Label() { Text = "Price", Location = new System.Drawing.Point(20, 20) };
            txtBuyPrice = new TextBox() { Location = new System.Drawing.Point(150, 20), Width = 100 };

            btnBuySubmit = new Button() { Text = "Submit Order", Location = new System.Drawing.Point(270, 20) };
            btnBuySubmit.Click += (sender, e) => SubmitOrder("BUY", txtBuyPrice.Text);

            tabBuy.Controls.Add(lblBuyPrice);
            tabBuy.Controls.Add(txtBuyPrice);
            tabBuy.Controls.Add(btnBuySubmit);

            // Sell tab controls
            Label lblSellPrice = new Label() { Text = "Price", Location = new System.Drawing.Point(20, 20) };
            txtSellPrice = new TextBox() { Location = new System.Drawing.Point(150, 20), Width = 100 };

            btnSellSubmit = new Button() { Text = "Submit Order", Location = new System.Drawing.Point(270, 20) };
            btnSellSubmit.Click += (sender, e) => SubmitOrder("SELL", txtSellPrice.Text);

            tabSell.Controls.Add(lblSellPrice);
            tabSell.Controls.Add(txtSellPrice);
            tabSell.Controls.Add(btnSellSubmit);

            // Buy orders list
            Label lblBuyOrders = new Label() { Text = "Buy Orders", Location = new System.Drawing.Point(20, 270) };
            lstBuyOrders = new ListBox() { Location = new System.Drawing.Point(20, 300), Size = new System.Drawing.Size(450, 200) };

            // Sell orders list
            Label lblSellOrders = new Label() { Text = "Sell Orders", Location = new System.Drawing.Point(500, 270) };
            lstSellOrders = new ListBox() { Location = new System.Drawing.Point(500, 300), Size = new System.Drawing.Size(450, 200) };

            // Buttons for executing and clearing orders
            btnSellOrder = new Button() { Text = "Sell Selected Order", Location = new System.Drawing.Point(20, 520) };
            btnSellOrder.Click += (sender, e) => ExecuteOrder("SELL");

            btnBuyOrder = new Button() { Text = "Buy Selected Order", Location = new System.Drawing.Point(500, 520) };
            btnBuyOrder.Click += (sender, e) => ExecuteOrder("BUY");

            btnClearOrders = new Button() { Text = "Clear All Orders", Location = new System.Drawing.Point(260, 520) };
            btnClearOrders.Click += (sender, e) => ClearAllOrders();

            // Add order controls to the form
            this.Controls.Add(lblBuyOrders);
            this.Controls.Add(lstBuyOrders);
            this.Controls.Add(lblSellOrders);
            this.Controls.Add(lstSellOrders);
            this.Controls.Add(btnSellOrder);
            this.Controls.Add(btnBuyOrder);
            this.Controls.Add(btnClearOrders);

            // Stock prices list view
            lvStockPrices = new ListView() { Location = new System.Drawing.Point(500, 20), Size = new System.Drawing.Size(450, 240) };
            lvStockPrices.View = View.Details;
            lvStockPrices.Columns.Add("Stock", 330, HorizontalAlignment.Left);
            lvStockPrices.Columns.Add("Price", 120, HorizontalAlignment.Left);
            this.Controls.Add(lvStockPrices);
        }

        // Method to load orders from file
        private List<Order> LoadOrders()
        {
            // Check if the orders file exists
            if (File.Exists(OrdersFilePath))
            {
                // Read the JSON content from the file
                var json = File.ReadAllText(OrdersFilePath);
                // Deserialize the JSON content to a list of orders
                return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
            }
            // Return an empty list if the file does not exist
            return new List<Order>();
        }

        // Method to save orders to a file
        private void SaveOrders()
        {
            // Serialize the orders list to JSON format with indented formatting
            var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            // Write the JSON content to the orders file
            File.WriteAllText(OrdersFilePath, json);
        }

        // Method to populate the buy and sell order lists
        private void PopulateOrderLists()
        {
            // Clear the current items in the buy and sell order lists
            lstBuyOrders.Items.Clear();
            lstSellOrders.Items.Clear();
            // Iterate through each order in the orders list
            foreach (var order in orders)
            {
                // Add the order to the appropriate list based on its side (BUY or SELL)
                if (order.Side == "BUY")
                {
                    lstBuyOrders.Items.Add(order);
                }
                else
                {
                    lstSellOrders.Items.Add(order);
                }
            }
        }

        // Method to populate the stock prices list view
        private void PopulateStockPrices()
        {
            // Clear the current items in the stock prices list view
            lvStockPrices.Items.Clear();
            // Iterate through each stock in the stock prices dictionary
            foreach (var stock in stockPrices)
            {
                // Create a new list view item for the stock
                var item = new ListViewItem(stock.Key);
                // Add the stock price as a subitem
                item.SubItems.Add($"${stock.Value:F2}");
                // Add the item to the stock prices list view
                lvStockPrices.Items.Add(item);
            }
        }

        // Method to submit a new order
        private void SubmitOrder(string side, string price)
        {
            // Check if the username textbox is empty or has whitespace
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                // Show a message box if the username is not entered
                MessageBox.Show("Please enter a username.");
                return;
            }

            // Check if a stock is selected in the combobox
            if (cmbStocks.SelectedItem == null)
            {
                // Show a message box if no stock is selected
                MessageBox.Show("Please select a stock.");
                return;
            }

            // Try to parse the price input to a double
            if (!double.TryParse(price, out double parsedPrice))
            {
                // Show a message box if the price is not valid
                MessageBox.Show("Please enter a valid price.");
                return;
            }

            // Format the parsed price to two decimal places
            string formattedPrice = parsedPrice.ToString("F2");

            // Get the selected stock from the combobox
            string stock = cmbStocks.SelectedItem.ToString();

            // Create a new order object with the provided details
            var order = new Order
            {
                Username = txtUsername.Text,
                Side = side,
                Stock = stock,
                Quantity = 100,
                Price = parsedPrice
            };

            // Add the new order to the orders list
            orders.Add(order);

            // Save the updated orders list to the file
            SaveOrders();

            // Add the order to the appropriate listbox based on the side (BUY or SELL)
            if (side == "BUY")
            {
                lstBuyOrders.Items.Add(order);
            }
            else
            {
                lstSellOrders.Items.Add(order);
            }

            // Show a message box confirming the order submission
            MessageBox.Show($"{side} order for {stock} at ${formattedPrice} submitted.");
        }

        // Method to execute an order based on the side (BUY or SELL)
        private void ExecuteOrder(string side)
        {
            // Check if the username textbox is empty or whitespace
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                // Show a message box if the username is not entered
                MessageBox.Show("Please enter a username.");
                return;
            }

            // Determine the source list based on the side (BUY or SELL)
            ListBox sourceList = side == "BUY" ? lstSellOrders : lstBuyOrders;
            // Check if an order is selected in the source list
            if (sourceList.SelectedItem == null)
            {
                // Show a message box if no order is selected
                MessageBox.Show($"Please select an order to {side.ToLower()}.");
                return;
            }

            // Get the selected order from the source list
            var order = (Order)sourceList.SelectedItem;
            // Remove the order from the orders list
            orders.Remove(order);
            // Save the updated orders list to the file
            SaveOrders();
            // Remove the order from the source list
            sourceList.Items.Remove(order);

            // Extract order details
            string otherUser = order.Username;
            string stock = order.Stock;
            int quantity = order.Quantity;
            double price = order.Price;

            // Update user stocks
            // Check if the user has any stocks
            if (!userStocks.ContainsKey(txtUsername.Text))
            {
                // Initialize the user's stock dictionary if it doesn't exist
                userStocks[txtUsername.Text] = new Dictionary<string, int>();
            }

            // Check if the user has the specific stock
            if (!userStocks[txtUsername.Text].ContainsKey(stock))
            {
                // Initialize the stock quantity if it doesn't exist
                userStocks[txtUsername.Text][stock] = 0;
            }

            // Update the stock quantity based on the side (BUY or SELL)
            if (side == "BUY")
            {
                // Increase the stock quantity for a BUY order
                userStocks[txtUsername.Text][stock] += quantity;
                // Show a message box confirming the purchase
                MessageBox.Show($"{txtUsername.Text} bought {quantity} of {stock} from {otherUser} at ${price}.");
            }
            else
            {
                // Decrease the stock quantity for a SELL order
                userStocks[txtUsername.Text][stock] -= quantity;
                // Show a message box confirming the sale
                MessageBox.Show($"{txtUsername.Text} sold {quantity} of {stock} to {otherUser} at ${price}.");
            }

            // Publish trade to RabbitMQ
            string trade = $"{txtUsername.Text},{side},{stock},{quantity},${price},{otherUser}";
            rabbitMqController.PublishQueryResponse(trade);
        }

        // Method to clear all orders, mostly for testing
        private void ClearAllOrders()
        {
            // Clear the orders list
            orders.Clear();
            // Save the updated orders list to the file
            SaveOrders();
            // Clear the items in the buy and sell order lists
            lstBuyOrders.Items.Clear();
            lstSellOrders.Items.Clear();
            // Show a message box confirming that all orders have been cleared
            MessageBox.Show("All orders have been cleared.");
        }
    }
}
