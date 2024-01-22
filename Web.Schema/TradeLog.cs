using Base.Enum;

namespace Web.Schema;

public class TradeRequest
{
    public int  PortfolioId { get; set; }
    public int ShareId { get; set; }
    public int Quantity { get; set; }
}

public class TradeResponse
{
    public int  PortfolioId { get; set; }
    public int ShareId { get; set; }
    public int Quantity { get; set; }
    public TradeType Type { get; set; } 
    public DateTime Timestamp { get; set; }
}