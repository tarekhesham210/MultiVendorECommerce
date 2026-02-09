using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Interfaces;

namespace MultiVendorECommerce.Areas.Customer.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductVariantRepository _variantRepository;
        private readonly ICartQueryService _cartQueryService;
        public CartService(ICartRepository cartRepository, ICustomerRepository customerRepository, IProductVariantRepository variantRepository, ICartItemRepository cartItemRepository, ICartQueryService cartQueryService)
        {
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _variantRepository = variantRepository;
            _cartItemRepository = cartItemRepository;
            _cartQueryService = cartQueryService;
        }
        public async Task<CartDetailsVM> GetCartDetailsAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedException("you have to login first");
            }
            var customer = await _customerRepository.GetCustomerByUserIdAsync(userId);
            if (customer == null)
                throw new NotFoundException("Customer profile not found");

            var cart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            if (cart == null)
            {
                return new CartDetailsVM();
            }

            var cartDetails = await _cartQueryService.GetCartDetailsAsync(cart.Id);


            return cartDetails;
        }

        public async Task<int> AddToCustomerCart(string userId,int variantId,int quantity = 1)
        {
            var customer= await _customerRepository.GetCustomerByUserIdAsync(userId);
            if (customer == null) 
            { 
                throw new ValidationException("Invalid Customer Data");
            }
            var cart =await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            if (cart == null)
            {
                cart = new Cart
                {
                    CustomerId = customer.Id,
                    CreatedAt = DateTime.UtcNow,
                };
                await _cartRepository.AddCart(cart);
                await _cartRepository.SaveAsync();
            }
            var existingItem = cart.Items
                    .FirstOrDefault(i => i.ProductVariantId == variantId);
            var variant = await _variantRepository.GetProductByIdAsync(variantId);
            if (variant == null)
                throw new NotFoundException("product not found");
            int currentInCart = existingItem?.Quantity ?? 0;
            int requestedTotal = currentInCart + quantity;

            if (requestedTotal > variant.StockQuantity) 
            {
                return 0; // Indicate failure due to insufficient stock
            }
            if (existingItem != null) 
            { 
                existingItem.Quantity += quantity;

            }
            else
            {
                
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    Quantity = quantity,
                    PriceAtTime = variant.FinalPrice,
                    ProductVariantId = variant.Id,                   
                };
                await _cartItemRepository.AddCartItemAsync(newItem);
                await _cartItemRepository.SaveAsync();
                cart.Items.Add(newItem);
            }
            await _cartRepository.SaveAsync();
            return cart.Items.Sum(i => i.Quantity);
        }
    
        public async Task<bool> UpdateItemQuantityAsync(string userId,int quantity,int variantId)
        {
            var customer =await _customerRepository.GetCustomerByUserIdAsync(userId);
            if(customer == null)
                throw new ValidationException("Invalid Customer Data");

            var cart =await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductVariantId == variantId);

            var variant = await _variantRepository.GetProductByIdAsync(variantId);
            if (variant == null)
                throw new NotFoundException("product not found");
            int currentInCart = cartItem?.Quantity ?? 0;
            

            if (quantity > variant.StockQuantity)
            {
                 return false; // Indicate failure due to insufficient stock
            }
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _cartItemRepository.SaveAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> RemoveItemAsync(string userId, int variantId)
        {
            var customer = await _customerRepository.GetCustomerByUserIdAsync(userId);
            if (customer == null)
                throw new ValidationException("Invalid Customer Data");

            var cart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductVariantId == variantId);
            if (cartItem != null)
            {
                 _cartItemRepository.RemoveCartItem(cartItem);
                await _cartItemRepository.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<int> GetCartItemsCountAsync(string userId)
        {
            var customer =await _customerRepository.GetCustomerByUserIdAsync(userId);
            if (customer == null)
                return 0;
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customer.Id);
            if (cart==null)
            {
                return 0;
            }
            var cartcount=cart.Items.Sum(i=>i.Quantity);

            return cartcount;

        }
    }
}
