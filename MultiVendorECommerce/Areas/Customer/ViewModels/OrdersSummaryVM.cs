namespace PermissionBasedAuz.Areas.Customer.ViewModels
{
    public class OrdersSummaryVM
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        public decimal Shipping = 10;
        public decimal Total { get; set; }
        public int ItemsCount { get; set; }
    }
}
