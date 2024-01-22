using Base.Entity;

namespace Web.Data.Entity;

public class Portfolio:BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public decimal TotalBalance { get; set; }

    public ICollection<PortfolioItem> PortfolioItems { get; set; }
    public ICollection<TradeLog> TradeLogs { get; set; }
}