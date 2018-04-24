namespace StockMarket.Model.Stock
{
    public class PreferredStock : Stock
    {
        private readonly double fixedDividend;

        public override string StockType => Enums.StockType.Preferred.ToString();
        public PreferredStock(string symbol, double lastDividend, double parValue, double fixedDividend)
            : base(symbol, lastDividend, parValue)
        {
            this.fixedDividend = fixedDividend;
        }

        public override double CalcDividendYield(double price)
        {
            return Calculations.FixedDividendYield(this.fixedDividend, this.ParValue, price);
        }
    }
}
