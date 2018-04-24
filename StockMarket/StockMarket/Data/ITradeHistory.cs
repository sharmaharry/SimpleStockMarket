namespace StockMarket.Data
{
    using System;
    using System.Collections.Generic;
    using StockMarket.Model.Trade;

    /// <summary>
    /// The TradeRepository interface.
    /// </summary>
    public interface ITradeHistory
    {
        /// <summary>
        /// The record trade.
        /// </summary>
        /// <param name="trade">The trade to record.</param>
        void RecordTrade(Trade trade);

        /// <summary>
        /// The get trades by stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol.</param>
        /// <param name="dateTime">Get trades from specified time onwards</param>
        /// <returns>The <see cref="IEnumerable"/> of filtered trades.</returns>
        IEnumerable<Trade> GetTrades(string stockSymbol, DateTime dateTime);

        /// <summary>
        /// The get trades by stock symbol.
        /// </summary>
        /// <param name="dateTime">Get trades from specified time onwards</param>
        /// <returns>The <see cref="IEnumerable"/> of filtered trades.</returns>
        IEnumerable<Trade> GetTrades(DateTime dateTime);

        /// <summary>
        /// The get trades by stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol.</param>
        /// <returns>The <see cref="IEnumerable"/> of filtered trades.</returns>
        IEnumerable<Trade> GetTrades(string stockSymbol);

        /// <summary>
        /// The get all trades.
        /// </summary>
        /// <returns>The <see cref="IEnumerable"/> of all trades.</returns>
        IEnumerable<Trade> GetTrades();
    }
}
