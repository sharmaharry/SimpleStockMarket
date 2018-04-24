namespace StockMarket
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using StockMarket.Model.Trade;

    public static class Calculations
    {
        /// <summary>
        /// The last dividend yield.
        /// </summary>
        public static double LastDividendYield(double lastDividend, double price)
        {
            if (Convert.ToInt32(price) == 0)
            {
                throw new ArgumentException("Error, Cannot divide by zero.");
            }

            return (lastDividend / price);
        }

        /// <summary>
        /// The fixed dividend yield.
        /// </summary>
        public static double FixedDividendYield(double fixedDividend, double parValue, double price)
        {
            if (Convert.ToInt32(price) == 0)
            {
                throw new ArgumentException("Error, Cannot divide by zero.");
            }

            return (fixedDividend * parValue) / price;
        }

        /// <summary>
        /// The pe ratio.
        /// </summary>

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public static double PERatio(double price, double dividend)
        {
            if (Convert.ToInt32(dividend) == 0)
            {
                throw new ArgumentException("Error, Cannot divide by zero.");
            }

            return price / dividend;
        }

        /// <summary>
        /// The geometric mean.
        /// </summary>

        public static double GeometricMean(IList<double> values)
        {
            if (!values.Any())
            {
                throw new ArgumentException("Error,Non-zero root must be specified");
            }

            var total = values.Aggregate<double, double>(1, (x, y) => x * y);
            return Math.Pow(total, 1.0 / values.Count);
        }

        /// <summary>
        /// The volume weighted stock price.
        /// </summary>
        public static double VolumeWeightedStockPrice(IList<Trade> trades)
        {
            return trades.Sum(t => t.Price * t.Quantity) / trades.Sum(t => t.Quantity);
        }
    }
}
