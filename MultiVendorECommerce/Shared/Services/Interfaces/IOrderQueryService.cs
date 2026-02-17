using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Shared.ViewModels;

namespace MultiVendorECommerce.Shared.Services.Interfaces
{
    public interface IOrderQueryService
    {
        Task<OrderDetailsVM?> GetOrderDetails(int id);
        Task<PagedResult<OrdersSummaryVM>> GetCustomerOrders(int customerId, int pageSize = 10, int pageNumber = 1);
        Task<Areas.Admin.ViewModels.OrderDetailsVM> GetNewOrderDetails(int id);
    }
}
