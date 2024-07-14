public class Order
{
    public string Username { get; set; } // Store the username of the person placing the order

    public string Side { get; set; } // Store the side of the order (BUY or SELL)

    public string Stock { get; set; } // Store the stock symbol for the order

    public int Quantity { get; set; } // Store the quantity of stocks in the order

    public double Price { get; set; } // Store the price per stock in the order

    public override string ToString() // Override the ToString method to provide a custom string representation of the order
    {
        // Return a formatted string with order details
        return $"{Side} {Stock} - {Quantity} @ ${Price:F2} by {Username}";
    }
}