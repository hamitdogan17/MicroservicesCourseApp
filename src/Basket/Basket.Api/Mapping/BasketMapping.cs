using AutoMapper;
using Basket.Api.Entities;
using EventBusRabbitMQ.Events;

namespace Basket.Api.Mapping
{
    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
