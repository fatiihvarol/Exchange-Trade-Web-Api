using Microsoft.EntityFrameworkCore;
using Web.Data.DbContext;
using Web.Data.Entity;

namespace Web.Business.Service;

public class UpdateSharePrice:IUpdateSharePrice
{
    private readonly TradeDbContext _dbContext;

    public UpdateSharePrice(TradeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateSharePriceAfterSell(int shareId, int quantity)
    {
        var share = await _dbContext.Set<Share>()
            .FirstOrDefaultAsync(x => x.Id == shareId);

        if (share != null)
        {
            share.CurrentPrice = (share.CurrentPrice * share.TotalAmount + quantity) / (share.TotalAmount + quantity);
            share.TotalAmount = share.TotalAmount + quantity;
            await _dbContext.SaveChangesAsync();
        }
    }
    
    
    public async Task UpdateSharePriceAfterBuy(int shareId, int quantity)
    {
       
        
        var share = await _dbContext.Set<Share>()
            .FirstOrDefaultAsync(x => x.Id == shareId);

        if (share != null)
        {
            share.CurrentPrice = (share.CurrentPrice * share.TotalAmount - quantity) / (share.TotalAmount - quantity);
            share.TotalAmount = share.TotalAmount - quantity;
            await _dbContext.SaveChangesAsync();
        }
    }
}