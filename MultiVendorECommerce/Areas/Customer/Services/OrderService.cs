using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Interfaces;
using MultiVendorECommerce.Shared.ViewModels;

namespace MultiVendorECommerce.Areas.Customer.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductVariantRepository _variantRepository;
        private readonly ICartQueryService _cartQueryService;
        private readonly IOrderQueryService _orderQueryService;
        private readonly ApplicationDb _context;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, ICartRepository cartRepository, ICartQueryService cartQueryService, IProductVariantRepository variantRepository, ApplicationDb context, IOrderItemRepository orderItemRepository, IOrderQueryService orderQueryService)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _cartRepository = cartRepository;
            _cartQueryService = cartQueryService;
            _variantRepository = variantRepository;
            _context = context;
            _orderItemRepository = orderItemRepository;
            _orderQueryService = orderQueryService;
        }

        //get
        public async Task<CheckoutVM> PlaceOrderAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("you have to login first");
            }
            var customer = await _customerRepository.GetCustomerByUserIdAsync(userId) ?? throw new NotFoundException("Customer profile not found");
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            if (cart == null || !cart.Items.Any())
            {
                throw new BusinessRuleException("Can not make order with empty cart");
            }
            var cartDetails = await _cartQueryService.GetCartDetailsAsync(cart.Id);
            var checkout = new CheckoutVM
            {
                CartItems = cartDetails.cartItems,
                SubTotal = cartDetails.cartItems.Sum(i => i.TotalPrice),
                PaymentMethod=PaymentMethod.Cash
            };
            return checkout;
        }
        //post
        public async Task<int> PlaceOrderAsync(string userId, CheckoutVM checkout)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("you have to login first");
            }
            var customer = await _customerRepository.GetCustomerByUserIdAsync(userId) ?? throw new NotFoundException("Customer profile not found");
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            if (cart == null || !cart.Items.Any())
            {
                throw new BusinessRuleException("Can not make order with empty cart");
            }
            var order = new Order
            {
                CustomerId = customer.Id,
                City = checkout.City,
                PaymentMethod = checkout.PaymentMethod,
                Status = OrderStatus.Pending,
                Phone = checkout.Phone,
                CreatedAt = DateTime.UtcNow,
                ShippingAddress = checkout.Address,
                TotalAmount = cart.Items.Sum(i => (i.PriceAtTime * i.Quantity)) + checkout.Shipping,
                Items = new List<OrderItem>()

            };
            order.PaymentStatus = order.PaymentMethod == PaymentMethod.Cash ? PaymentStatus.Unpaid : PaymentStatus.Paid;
            foreach (var cartItem in cart.Items)
            {
                var variant = await _variantRepository.GetProductByIdAsync(cartItem.ProductVariantId) ?? throw new NotFoundException("product not found");

                if (variant.StockQuantity < cartItem.Quantity)
                    throw new Exception($"Sorry, product {variant.Product.Name} is out of stock.");

                var orderItem = new OrderItem
                {
                    ProductVariantId = cartItem.ProductVariantId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.PriceAtTime,
                    VendorId = variant.Product.VendorId
                };
                variant.SetStockQuantity(variant.StockQuantity - cartItem.Quantity);

                order.Items.Add(orderItem);
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var item in order.Items)
                {
                    await _orderItemRepository.AddOrderItemAsync(item);
                }
                await _orderRepository.AddOrderAsync(order);
                _cartRepository.DeleteCart(cart);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return order.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderDetailsVM> GetOrderDetails(int id)
        {
            var order = await _orderQueryService.GetOrderDetails(id);
            if (order == null)
            {
                throw new NotFoundException("Order not found");
            }
            return order;
        }

        public async Task<PagedResult<OrdersSummaryVM>> GetCustomerOrders(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("you have to login first");
            }
            var customer = await _customerRepository.GetCustomerByUserIdAsync(userId) ?? throw new NotFoundException("Customer profile not found");
            var orders = await _orderQueryService.GetCustomerOrders(customer.Id);
            return orders;
        }

        public async Task CancelOrder(string userId, int orderId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("you have to login first");
            }
            if (orderId <= 0) throw new ValidationException("Invalid order");
            var customer = await _customerRepository.GetCustomerByUserIdAsync(userId) ?? throw new NotFoundException("Customer profile not found");
            var order = await _orderRepository.GetOrderByCustomerIdAsync(customer.Id,orderId) ?? throw new NotFoundException("Order not found");
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                order.Status = OrderStatus.Cancelled;
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
    }
}
