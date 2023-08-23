namespace Api.Models
{
    public class SystemConfig
    {
        public int Id { get; set; }
        public decimal BaseCost { get; set; }
        public decimal DependentCost { get; set; }
        public decimal AdditionalSalaryThreshold { get; set; }
        public decimal SalaryDeductionRate { get; set; }
        public decimal AgeBasedDeduction { get; set; }
        public decimal PaycheckPerYear { get; set; }
    }
}