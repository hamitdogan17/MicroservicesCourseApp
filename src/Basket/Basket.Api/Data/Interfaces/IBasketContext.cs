using StackExchange.Redis;

namespace Basket.Api.Data.Interfaces
{
    public interface IBasketContext
    {
        IDatabase Redis { get; }
    }
}
