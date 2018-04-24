// --------------------------------------------------------------------------------------------------------------------
// <summary>
//   Defines the StockMarketService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StockMarket
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using StockMarket.Model.Stock;
    using StockMarket.Model.Trade;
    using StockMarket.Data;
    using StockMarket.Model.Enums;

    public sealed class StockMarketService
    {

        private readonly Dictionary<string, Stock> stockList;
        private readonly ITradeHistory tradeHistory;

        public StockMarketService(ITradeHistory tradeHistory, Dictionary<string, Stock> stockList)
        {
            this.tradeHistory = tradeHistory;
            this.stockList = stockList;
        }
        
        /// <summary>
        /// Calculate the all share index.
        /// </summary>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public double CalcAllShareIndex()
        {
            // calc volume weighted stock price for all stocks
            var stockPrices = this.stockList.Select(s => this.CalcVolumeWeightedStockPrice(s.Key)).ToList();

            // then calc the geometric mean for these values
            return Calculations.GeometricMean(stockPrices);
        }

        /// <summary>
        /// Calculates the volume weighted stock price for the given
        /// stock symbol based on trades in the past 5 minutes.
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <param name="timeFilter">Number of minutes to filter trades by</param>
        /// <returns>The stock price.</returns>
        public double CalcVolumeWeightedStockPrice(string stockSymbol, int timeFilter = 5)
        {
            return this.PerformCalculation(stockSymbol,stock =>
                    {
                        var fromDateTime = DateTime.UtcNow.Subtract(new TimeSpan(0, timeFilter, 0));
                        var stockTrades = this.tradeHistory.GetTrades(stock.Symbol, fromDateTime).ToList();
                   
                        return stockTrades.Any() ? Calculations.VolumeWeightedStockPrice(stockTrades) : 0;
                    });
        }

        /// <summary>
        /// Calculate dividend yield for stock.
        /// </summary>
        /// <param name="stockSymbol">The stock.</param>
        /// <param name="price">The price.</param>
        /// <returns>The calculated dividend yield.</returns>
        public double CalcDividendYield(string stockSymbol, double price)
        {
            return this.PerformCalculation(stockSymbol, stock => stock.CalcDividendYield(price));
        }

        /// <summary>
        /// Calculates the PE for a given stock.
        /// </summary>
        /// <param name="stockSymbol">The stock.</param>
        /// <param name="price">The price.</param>
        /// <returns>The calculated P/E ration.</returns>
        public double CalcPERatio(string stockSymbol, double price)
        {
            return this.PerformCalculation(stockSymbol, stock => stock.CalcPERatio(price));
        }

        /// <summary>
        /// Buy stock on the stock market.
        /// </summary>
        /// <param name="stockSymbol">The stock to buy.</param>
        /// <param name="price">The price of the stock.</param>
        /// <param name="quantity">The quantity to buy.</param>
        public void BuyStock(string stockSymbol, double price, int quantity)
        {
           this.PerformTrade(stockSymbol, price, quantity, TradeType.Buy);
        }

        /// <summary>
        /// Sell stock on the stock market.
        /// </summary>
        /// <param name="stockSymbol">The stock to sell.</param>
        /// <param name="price">The price of the stock.</param>
        /// <param name="quantity">The quantity to sell.</param>
        public void SellStock(string stockSymbol, double price, int quantity)
        {
            this.PerformTrade(stockSymbol, price, quantity, TradeType.Sell);
        }

        /// <summary>
        /// Safely perform a stock calculation
        /// </summary>
        /// <param name="stockSymbol">The stock.</param>
        /// <param name="calculationFunc">The calculation to perform</param>
        /// <returns>The result of the calculation that was performed.</returns>
        private double PerformCalculation(string stockSymbol, Func<Stock, double> calculationFunc)
        {
            var stock = this.GetStock(stockSymbol);
            if (stock == null)
            {
                throw new ArgumentException($"{stockSymbol} is not a tradable stock.");
            }

            return calculationFunc(this.stockList[stockSymbol]);
        }

        /// <summary>
        /// Safely performs a trade on the stock market.
        /// </summary>
        /// <param name="stockSymbol">The symbol of the stock to trade.</param>
        /// <param name="price">The price of the stock.</param>
        /// <param name="quantity">The quantity of stock.</param>
        /// <param name="tradeType">The type of trade to perform.</param>
        private void PerformTrade(string stockSymbol, double price, int quantity, TradeType tradeType)
        {
            var stock = this.GetStock(stockSymbol);
            if (stock == null)
            {
                throw new ArgumentException($"{stockSymbol} is not a tradable stock.");
            }
            
            var trade = new Trade(stock, tradeType, price, quantity);
            this.tradeHistory.RecordTrade(trade);
        }

        /// <summary>
        /// Return stock by stock symbol if tradable on this market.
        /// </summary>
        private Stock GetStock(string stockSymbol)
        {
            return this.stockList.ContainsKey(stockSymbol) 
                ? this.stockList[stockSymbol] 
                : null;
        }
    }
}
