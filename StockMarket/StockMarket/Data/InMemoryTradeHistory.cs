namespace StockMarket.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using StockMarket.Model.Trade;

    public class InMemoryTradeHistory : ITradeHistory
    {
        private readonly List<Trade> history;

        public InMemoryTradeHistory()
        {
            this.history = new List<Trade>();
        }

        public void RecordTrade(Trade trade)
        {
            this.history.Add(trade);
        }

        public IEnumerable<Trade> GetTrades(string stockSymbol, DateTime dateTime)
        {
            return this.history.Where(t => t.StockSymbol == stockSymbol && t.Timestamp >= dateTime);
        }

        public IEnumerable<Trade> GetTrades(DateTime dateTime)
        {
            return this.history.Where(t => t.Timestamp >= dateTime);
        }

        public IEnumerable<Trade> GetTrades(string stockSymbol)
        {
            return this.history.Where(t => t.StockSymbol == stockSymbol);
        }

        public IEnumerable<Trade> GetTrades()
        {
            return this.history;
        } 
    }
}
