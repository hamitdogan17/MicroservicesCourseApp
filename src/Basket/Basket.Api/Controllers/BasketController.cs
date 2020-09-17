using Basket.Api.Entities;
using Basket.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;

        public BasketController(IBasketRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> GetBasketAsync(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasketAsync([FromBody]BasketCart basket)
        {
            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasketAsync(string userName)
        {
            return Ok(await _repository.DeleteBasket(userName));
        }
    }
}
