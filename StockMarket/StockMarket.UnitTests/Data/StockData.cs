namespace StockMarket.UnitTests.Data
{
    using System.Collections.Generic;
    using StockMarket.Model.Stock;

    /// <summary>
    /// Create a list of Stock Data.
    /// </summary>
    public static class StockData
    {
        public static Stock Tea => new CommonStock("TEA", 0, 100);

        public static Stock Pop => new CommonStock("POP", 8, 100);

        public static Stock Ale => new CommonStock("ALE", 23, 60);

        public static Stock Gin => new PreferredStock("GIN", 8, 100, 2);

        public static Stock Joe => new CommonStock("JOE", 13, 250);

        /// <returns>The <see cref="Dictionary"/>.</returns>
        public static Dictionary<string, Stock> Stocks()
        {
            return new Dictionary<string, Stock>
                       {
                           { Tea.Symbol, Tea },
                           { Pop.Symbol, Pop },
                           { Ale.Symbol, Ale },
                           { Gin.Symbol, Gin },
                           { Joe.Symbol, Joe },
                       };
        }
    }
}
