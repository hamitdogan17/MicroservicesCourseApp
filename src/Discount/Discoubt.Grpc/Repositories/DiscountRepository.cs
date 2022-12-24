using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await conn.ExecuteAsync
                ("insert into Coupon (ProductName, Description, Amount) values (@ProductName, @Description, @Amount)",
                    new { ProductName = coupon.ProductName, Desciption = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await conn.ExecuteAsync("Delete from Coupon where ProductName = @ProductName",
                    new { ProductName = productName });

            if (affected == 0)
                return false;

            return true;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>("Select * from Coupon Where ProductName = @ProductName", new { ProductName = productName });

            if (coupon is null)
                return new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var conn = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await conn.ExecuteAsync
                ("Update Coupon set ProductName = @ProductName, Description = @Description, Amount = @Amount) where Id = @Id",
                    new { ProductName = coupon.ProductName, Desciption = coupon.Description, Amount = coupon.Amount });

            if (affected == 0)
                return false;

            return true;
        }
    }
}
