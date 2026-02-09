using MultiVendorECommerce.Areas.Vendor.ViewModels;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Shared.Repositories.Interfaces;

namespace MultiVendorECommerce.Areas.Vendor.Services
{
    public class OrderService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IVendorRepository _vendorRepository;

        public OrderService(IOrderItemRepository orderItemRepository, IVendorRepository vendorRepository)
        {
            _orderItemRepository = orderItemRepository;
            _vendorRepository = vendorRepository;
        }
        public async Task<IEnumerable<NewOrderItemVM>> GetVendornewOrdersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Yo must be Logged in");
            var vendor = await _vendorRepository.GetVendorByUserIdAsync(userId) ?? throw new NotFoundException("Vendor Profile not found");

            var orders = await _orderItemRepository.GetVendorPendingOrderItemsAsync(vendor.Id);
            return orders.Select(oi => new NewOrderItemVM
            {
                Id = oi.Id,
                Name = oi.Variant.Product.Name,
                Quantity = oi.Quantity,
                Price = oi.Price,
                OrderId = oi.OrderId,
                ProductId = oi.Variant.ProductId,
                VariantId = oi.ProductVariantId,
                SKU = oi.Variant.SKU,
                Total = oi.Quantity * oi.Price,

            });
        }
        public async Task ConfirmOrder(string userId, int orderItemId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedException("Yo must be Logged in");
            if (orderItemId <= 0)
                throw new ValidationException("Invalid Order Item Id");
            var vendor = await _vendorRepository.GetVendorByUserIdAsync(userId) ?? throw new NotFoundException("Vendor Profile not found");
            var orderItem = await _orderItemRepository.GetOrderItemsByIdAsync(orderItemId) ?? throw new NotFoundException("Order Item not found");
            if (orderItem.VendorId != vendor.Id)
                throw new UnauthorizedException("You are not authorized to confirm this order item");
            orderItem.VendorConfirmation = true;
            await _orderItemRepository.SaveAsync();

        }
    }
}
