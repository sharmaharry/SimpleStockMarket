namespace StockMarket.UnitTests.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Rhino.Mocks;
    using StockMarket.UnitTests.Data;

    using StockMarket;
    using StockMarket.Model.Trade;
    using StockMarket.Data;

    /// <summary>
    /// The stock market unit tests.
    /// </summary>
    [TestClass]
    public class StockMarketServiceUnitTests
    {
        /// <summary>
        /// The stock market.
        /// </summary>
        private StockMarketService stockMarketService;

        /// <summary>
        /// The setup of the unit tests.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            // Configure Ninject
            // var kernel = new StandardKernel();
            // kernel.Load(Assembly.GetExecutingAssembly());
            this.stockMarketService = new StockMarketService(new InMemoryTradeHistory(), StockData.Stocks());
        }

        [TestMethod]
        public void BuyStockRecordedTrade()
        {
            var mockTradeHistory = MockRepository.GenerateStub<ITradeHistory>();
            var service = new StockMarketService(mockTradeHistory, StockData.Stocks());

            service.BuyStock(StockData.Tea.Symbol, 0.75, 2);
            mockTradeHistory.AssertWasCalled(mth => mth.RecordTrade(Arg<Trade>.Is.Anything));  
        }

        [TestMethod]
        public void SellStockRecordedTrade()
        {
            var mockTradeHistory = MockRepository.GenerateStub<ITradeHistory>();
            var service = new StockMarketService(mockTradeHistory, StockData.Stocks());

            service.SellStock(StockData.Tea.Symbol, 0.75, 2);
            mockTradeHistory.AssertWasCalled(mth => mth.RecordTrade(Arg<Trade>.Is.Anything));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "selling of stock inappropriately allowed.")]
        public void SellStockThrowsIfInvalidStock()
        {
            this.stockMarketService.SellStock("TEST", 0.75, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "buying of stock inappropriately allowed.")]
        public void BuyStockThrowsIfInvalidStock()
        {
            this.stockMarketService.BuyStock("TEST", 0.75, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "performing calculation for invalid stock.")]
        public void CalculateCommonDividendsThrowsExceptionForInvalidStock()
        {
            this.stockMarketService.CalcDividendYield("TEST", 5);
        }

        [TestMethod]
        public void CanCalculateCommonDividends()
        {
            Assert.AreEqual(0, this.stockMarketService.CalcDividendYield(StockData.Tea.Symbol, 5), "Expected result is 0");
        }

        [TestMethod]
        public void CanCalculatePreferredDividends()
        {
            Assert.AreEqual(40, this.stockMarketService.CalcDividendYield(StockData.Gin.Symbol, 5), "Expected result is 40");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "performing calculation for invalid stock.")]
        public void CalculatePERatioThrowsIfInvalidStock()
        {
            this.stockMarketService.CalcPERatio("TEST", 5);
        }

        [TestMethod]
        public void CanCalculatePERatio()
        {
            Assert.AreEqual(0.125, this.stockMarketService.CalcPERatio(StockData.Gin.Symbol, 5), "Expected result is 0.125");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "performing calculation for invalid stock.")]
        public void CalculateStockPriceThrowsIfInvalidStock()
        {
            this.stockMarketService.CalcVolumeWeightedStockPrice("TEST");
        }

        [TestMethod]
        public void CalculateStockPriceReturnsZeroIfNoTradesForStock()
        {
            this.stockMarketService.BuyStock(StockData.Tea.Symbol, 0.75, 2);
            this.stockMarketService.BuyStock(StockData.Pop.Symbol, 0.80, 3);
            this.stockMarketService.BuyStock(StockData.Joe.Symbol, 0.85, 4);

            Assert.AreEqual(0, this.stockMarketService.CalcVolumeWeightedStockPrice(StockData.Gin.Symbol), "Expected result is 0");
        }

        [TestMethod]
        public void CanCalculateStockPrice()
        {
            this.stockMarketService.BuyStock(StockData.Tea.Symbol, 0.75, 2);
            this.stockMarketService.BuyStock(StockData.Pop.Symbol, 0.80, 3);
            this.stockMarketService.BuyStock(StockData.Joe.Symbol, 0.85, 4);
            this.stockMarketService.BuyStock(StockData.Pop.Symbol, 0.90, 7);

            Assert.AreEqual(0.87, Math.Round(this.stockMarketService.CalcVolumeWeightedStockPrice(StockData.Pop.Symbol), 2), "Expected result is 0.87");
        }

        [TestMethod]
        public void CalculateStockPriceDoesntThrowWhenNoTradeHistory()
        {
            Assert.AreEqual(0,this.stockMarketService.CalcVolumeWeightedStockPrice(StockData.Pop.Symbol), "Expected result is 0 as no trade history");
        }
    }
}
