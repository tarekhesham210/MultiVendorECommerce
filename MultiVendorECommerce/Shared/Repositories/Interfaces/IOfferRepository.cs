using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Shared.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        Task<Offer?> GetById(int id);
        Task AddOfferAsync(Offer offer);
        Task SaveAsync();
        void DeleteOffer(Offer offer);
    }
}
