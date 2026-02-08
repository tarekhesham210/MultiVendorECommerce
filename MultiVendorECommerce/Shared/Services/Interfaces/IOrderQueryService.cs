using PermissionBasedAuz.Areas.Customer.ViewModels;

namespace PermissionBasedAuz.Shared.Services.Interfaces
{
    public interface IOrderQueryService
    {
        Task<OrderDetailsVM?> GetOrderDetails(int id);
        Task<IEnumerable<OrdersSummaryVM>> GetCustomerOrders(int customerId);
    }
}
