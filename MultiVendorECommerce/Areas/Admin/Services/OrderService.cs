using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Interfaces;

namespace MultiVendorECommerce.Areas.Admin.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderQueryService _orderQueryService;
        private readonly ApplicationDb _context;

        public OrderService(IOrderRepository orderRepository, ApplicationDb context, IOrderQueryService orderQueryService)
        {
            _orderRepository = orderRepository;
            _context = context;
            _orderQueryService = orderQueryService;
        }

        public async Task<IEnumerable<PendingOrderVM>> GetPendingOrdersAsync() 
        { 
            var orders= await _orderRepository.GetPendingOrdersAsync();
            var pendingOrders = orders.Select(o=>new PendingOrderVM
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                Total = o.TotalAmount,
                OrderDate = o.CreatedAt,
                OrderStatus = o.Status.ToString(),
                ShippingAddress = o.ShippingAddress,
                ShippingCity = o.City,
                PaymentMethod = o.PaymentMethod.ToString(),
                PaymentStatus = o.PaymentStatus.ToString(),
                CustomerPhone = o.Phone           
            });
            return pendingOrders;
        }
        public async Task ConfirmOrder(int orderId) 
        { 
            if(orderId <= 0)
                throw new ValidationException("Invalid order ID.");
            var order =await _orderRepository.GetOrderByIdAsync(orderId);
            if(order == null)
                throw new NotFoundException("Order not found.");
            if(order.Status!=OrderStatus.Pending)
                throw new BusinessRuleException("Only pending orders can be confirmed.");
            order.Status = OrderStatus.Confirmed;
            await _orderRepository.SaveAsync();
        }
        public async Task RejectOrder(int orderId)
        {
            if (orderId <= 0)
                throw new ValidationException("Invalid order ID.");
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new NotFoundException("Order not found.");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                order.Status = OrderStatus.Rejected;
                foreach (var variant in order.Items)
                {
                    var oldQuantit = variant.Variant.StockQuantity;
                    variant.Variant.SetStockQuantity(oldQuantit + variant.Quantity);
                }
                order.Items.Clear();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<OrderDetailsVM> GetNewOrderDetails(int orderId)
        {
            if (orderId <= 0) throw new ValidationException("Invalid Order");
            var order=await _orderQueryService.GetNewOrderDetails(orderId)??throw new NotFoundException("order not found");
            return order;
        }
    }
}
