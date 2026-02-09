using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Shared.Services.Interfaces;

namespace MultiVendorECommerce.Shared.Services.Implementation
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

        public async Task<Areas.Admin.ViewModels.OrderDetailsVM> GetNewOrderDetails(int id)
        {
           var order=await _context.Orders.Where(o => o.Id == id).Select(o => new Areas.Admin.ViewModels.OrderDetailsVM
            {
                Id = o.Id,
                CustomerName = o.Customer.FirstName + " " + o.Customer.LastName,
                OrderDate = o.CreatedAt,
                PaymentMethod = o.PaymentMethod.ToString(),
                PaymentStatus = o.PaymentStatus.ToString(),
                OrderStatus = o.Status.ToString(),
                ShippingAddress = o.ShippingAddress,
                Phone = o.Phone,
                CustomerId = o.CustomerId,
               Products = o.Items.Select(i => new Areas.Admin.ViewModels.OrderItemDetailsVM
                {
                    ProductName = i.Variant.Product.Name,
                    Price = i.Price,
                    Quantity = i.Quantity,
                    Total = i.Price * i.Quantity,
                    ItemID=i.Id,
                    ProductId=i.Variant.ProductId,
                    VariantId=i.ProductVariantId,
                    VendorId=i.VendorId,
                    IsVendorConfirmed=i.VendorConfirmation,
                    
                    
                }).ToList(),
                GrandTotal = o.Items.Sum(i => (i.Price * i.Quantity)) ,
            }).FirstOrDefaultAsync();
            order.GrandTotal += order.Shipping;
            return order;
        }
    }
}
