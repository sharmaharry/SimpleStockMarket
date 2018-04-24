namespace StockMarket.Model.Stock
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class Stock
    {
       
        public string Symbol { get; private set; }

        public abstract string StockType { get; }

        protected double LastDividend { get; private set; }
        protected double ParValue { get; private set; }

        public abstract double CalcDividendYield(double price);

        /// Initializes a new instance of the <see cref="Stock"/> class. 
        protected Stock(string symbol, double lastDividend, double parValue)
        {
            this.Symbol = symbol;
            this.LastDividend = lastDividend;
            this.ParValue = parValue;
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        public double CalcPERatio(double price)
        {
            return Calculations.PERatio(price, this.CalcDividendYield(price));
        }
    }
}
