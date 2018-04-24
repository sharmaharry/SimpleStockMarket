namespace StockMarket.Model.Trade
{
    using System;
    using Stock;
    using Enums;

    public class Trade
    {
        public readonly DateTime Timestamp;

        public readonly string StockSymbol;

        private readonly TradeType tradeType;

        public readonly int Quantity;

        public readonly double Price;

        public Trade(Stock stock, TradeType tradeType, double price, int quantity)
            : this(stock, tradeType, price, quantity, DateTime.UtcNow)
        {
        }

        public Trade(Stock stock, TradeType tradeType, double price, int quantity, DateTime timestamp)
        {
            this.StockSymbol = stock.Symbol;
            this.tradeType = tradeType;
            this.Price = price;
            this.Quantity = quantity;
            this.Timestamp = timestamp;
        }

        public override string ToString()
        {
            return $"{this.Timestamp}   TRADE/{this.tradeType}    STOCK: {this.StockSymbol}   QTY: {this.Quantity}    PRICE: {this.Price}";
        }
    }
}
