using MultiVendorECommerce.Areas.Customer.ViewModels;

namespace MultiVendorECommerce.Shared.Services.Interfaces
{
    public interface IOrderQueryService
    {
        Task<OrderDetailsVM?> GetOrderDetails(int id);
        Task<IEnumerable<OrdersSummaryVM>> GetCustomerOrders(int customerId);
        Task<Areas.Admin.ViewModels.OrderDetailsVM> GetNewOrderDetails(int id);
    }
}
