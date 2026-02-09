using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Repositories.Interfaces;

namespace MultiVendorECommerce.Shared.Repositories.Implementations
{
    public class OfferRepo:IOfferRepository
    {
        private readonly ApplicationDb _context;

        public OfferRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddOfferAsync(Offer offer)
        {
            await _context.Offers.AddAsync(offer);
        }
        public async Task<Offer?> GetById(int id)
        {
           return await _context.Offers.FindAsync(id);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void DeleteOffer(Offer offer)
        {
            _context.Offers.Remove(offer);
        }
    }
}
