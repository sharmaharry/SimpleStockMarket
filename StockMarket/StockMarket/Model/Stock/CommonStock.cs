namespace StockMarket.Model.Stock
{
    public class CommonStock : Stock
    {
        public override string StockType => Enums.StockType.Common.ToString();
        public CommonStock(string symbol, double lastDividend, double parValue)
            : base(symbol, lastDividend, parValue)
        {
        }

        public override double CalcDividendYield(double price)
        {

           return Calculations.LastDividendYield(this.LastDividend, price);
        }
    }
}
