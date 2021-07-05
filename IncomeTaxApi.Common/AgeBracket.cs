namespace IncomeTaxApi.Common
{
    public class AgeBracket
    {
        public AgeBracket(int lowerBoundAge, int upperBoundAge, double rebateAmount)
        {
            LowerBoundAge = lowerBoundAge;
            RebateAmount = rebateAmount;
            UpperBoundAge = upperBoundAge;
        }
        public int LowerBoundAge { get; set; }
        public int UpperBoundAge { get; set; }
        public double RebateAmount { get; set; } 
    }
}
