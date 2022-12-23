using Basket.Api.Entities;
using System.Threading.Tasks;

namespace Basket.Api.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<BasketCart> GetBasket(string userName);
        Task<BasketCart> UpdateBasket(BasketCart basketCard);
        Task DeleteBasket(string userName);
    }
}
