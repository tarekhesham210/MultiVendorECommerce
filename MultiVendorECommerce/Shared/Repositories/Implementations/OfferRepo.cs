using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
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
