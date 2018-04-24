namespace StockMarket.UnitTests.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Data;

    using StockMarket.Model.Stock;
    using StockMarket.Model.Trade;
    using StockMarket.Data;
    using StockMarket.Model.Enums;
    /// <summary>
    /// The In Memory Trade History Unit Tests
    /// </summary>
    [TestClass]
    public class InMemoryTradeHistoryUnitTests
    {
        /// <summary>
        /// The trade history.
        /// </summary>
        private InMemoryTradeHistory tradeHistory;

        /// <summary>
        /// The sample stocks.
        /// </summary>
        private Dictionary<string, Stock> stockList;

        /// <summary>
        /// The setup of the unit tests.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            this.tradeHistory = new InMemoryTradeHistory(); /*Initialise */
            this.stockList = StockData.Stocks();       /*Create Sample Stock Data List*/
        }

        [TestMethod]
        public void CanRecordSingleTradeEvent()
        {
            /* create a Buy trade fot it.*/ 
            var trade = new Trade(StockData.Pop, TradeType.Buy, 50, 1);

            /*Verify trade history list should be empty*/
            Assert.AreEqual(0, this.tradeHistory.GetTrades().Count(), "Trade History should be empty");

            /*Add trade to the history*/
            this.tradeHistory.RecordTrade(trade);

            /*Verify trade has been added to the history*/
            Assert.AreEqual(1, this.tradeHistory.GetTrades().Count(), "Trade History should have a single value");
        }
        
        [TestMethod]
        public void CanReturnAllTradeHistory()
        {
            /*Verify trade history list should be empty*/
            Assert.AreEqual(0, this.tradeHistory.GetTrades().Count(), "Trade History should be empty");

            foreach (var trade in StockData.Stocks().Select(stock => new Trade(stock.Value, TradeType.Buy, 50, 1)))
            {
                this.tradeHistory.RecordTrade(trade);
            }

            /*Verify the trade history count, A buy trade should be created for each elemnt in the stockList list*/
            Assert.AreEqual(this.stockList.Count, this.tradeHistory.GetTrades().Count(), $"Trade History should contain {this.stockList.Count} elements");
        }

        [TestMethod]
        public void CanReturnAllTradeHistoryFromGivenPointInTime()
        {
            var startDateTime = DateTime.UtcNow;
            Assert.AreEqual(0, this.tradeHistory.GetTrades().Count(), "Trade History should be empty");

            var oldTrade = new Trade(StockData.Tea, TradeType.Buy, 5, 2, DateTime.MinValue);
            this.tradeHistory.RecordTrade(oldTrade);

            var newTrade = new Trade(StockData.Pop, TradeType.Buy, 10, 5, DateTime.UtcNow);
            this.tradeHistory.RecordTrade(newTrade);

            Assert.AreEqual(2, this.tradeHistory.GetTrades().Count(), "Trade History should be 2");
            Assert.AreEqual(1, this.tradeHistory.GetTrades(startDateTime).Count(), "Trade History should be 1");
            Assert.AreEqual(StockData.Pop.Symbol, this.tradeHistory.GetTrades(startDateTime).First().StockSymbol, "Should be POP");

            Assert.AreEqual(0, this.tradeHistory.GetTrades(StockData.Tea.Symbol, startDateTime).Count(), "Should not be able to find TEA");
            Assert.AreEqual(1, this.tradeHistory.GetTrades(StockData.Pop.Symbol, startDateTime).Count(), "Should able to find Pop");
        }
    }
}
