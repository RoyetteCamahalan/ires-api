namespace ires_api.DTO.Company
{
    public class CompanyPlanDto
    {
        public int planid { get; set; }
        public DateTime? subscriptionexpiry { get; set; }
        public decimal storage { get; set; }
        public int surveylimit { get; set; }
        public int billingcycle { get; set; }
        public decimal amount { get; set; }
        public bool isexpired { get; set; }
        public PlanDto? subscriptionPlan { get; set; }
    }
}
