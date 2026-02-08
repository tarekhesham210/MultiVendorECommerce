using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Areas.Customer.ViewModels;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Shared.Services.Interfaces;

namespace PermissionBasedAuz.Shared.Services.Implementation
{
    public class OrderQueryService : IOrderQueryService
    {
        private readonly ApplicationDb _context;

        public OrderQueryService(ApplicationDb context)
        {
            _context = context;
        }

        public async Task<OrderDetailsVM?> GetOrderDetails(int id)
        {
          return await _context.Orders.Where(o => o.Id == id).Select(o => new OrderDetailsVM
            {
                Id = o.Id,
                CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                OrderDate = o.CreatedAt,
                PaymentMethod = o.PaymentMethod.ToString(),
                PaymentStatus = o.PaymentStatus.ToString(),
                OrderStatus = o.Status.ToString(),
                ShippingAddress = o.ShippingAddress,
                Phone = o.Phone,
              Products = o.Items.Select(i => new ProductSummaryVM
                {
                    ProductName = i.Variant.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    Total = i.Price * i.Quantity

                }).ToList(),
                GrandTotal = o.Items.Sum(i => (i.Price * i.Quantity)) + 10,
                
            }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<OrdersSummaryVM>> GetCustomerOrders(int customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Select(o => new OrdersSummaryVM
                {
                    OrderId = o.Id,
                    Date = o.CreatedAt,
                    Status = o.Status.ToString(),
                    ItemsCount = o.Items.Sum(i=>i.Quantity),
                    Total = o.Items.Sum(i => (i.Quantity * i.Price)) 

                }).OrderByDescending(o=>o.Date).ToListAsync();
        }
    }
}
