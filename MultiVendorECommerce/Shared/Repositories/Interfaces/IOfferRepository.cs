using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        Task<Offer?> GetById(int id);
        Task AddOfferAsync(Offer offer);
        Task SaveAsync();
        void DeleteOffer(Offer offer);
    }
}
