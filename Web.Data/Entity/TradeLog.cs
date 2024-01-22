using Base.Entity;
using Base.Enum;

namespace Web.Data.Entity;

public class TradeLog:BaseEntity
{
    public int  PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }
    public int ShareId { get; set; }
    public Share Share { get; set; }
    
    public int Quantity { get; set; }
    public TradeType Type { get; set; } 
}