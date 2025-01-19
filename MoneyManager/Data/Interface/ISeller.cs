using MoneyManager.Data.Models;

namespace MoneyManager.Data.Interface
{
    public interface ISeller
    {
        string CreateSeller(SellerModel seller);
        string UpdateSeller(SellerModel seller);
        SellerModel GetSeller(Guid id);
        List<SellerModel> SearchSellers();
        string DeleteSeller(Guid id);
    }
}
