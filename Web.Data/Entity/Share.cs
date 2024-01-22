using Base.Entity;

namespace Web.Data.Entity;

public class Share:BaseEntity
{
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
    public int TotalAmount { get; set; }

    // Navigation Property
    public ICollection<TradeLog> TradeLogs { get; set; }
}