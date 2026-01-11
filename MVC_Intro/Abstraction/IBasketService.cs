namespace MVC_Intro.Abstraction
{
    public interface IBasketService
    {
        Task<List<BasketItem>> GetBasketItemsAsync();
    }
}
