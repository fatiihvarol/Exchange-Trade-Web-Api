namespace Web.Business.Service;

public interface IUpdateSharePrice
{
    Task UpdateSharePriceAfterSell(int shareId, int quantity);
    Task UpdateSharePriceAfterBuy(int shareId, int quantity);
}