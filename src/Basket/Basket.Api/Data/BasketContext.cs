using Basket.Api.Data.Interfaces;
using StackExchange.Redis;

namespace Basket.Api.Data
{
    public class BasketContext : IBasketContext
    {
        private readonly ConnectionMultiplexer _redisConnection;

        public BasketContext(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = _redisConnection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}
