namespace Web.Schema;

public class PortfolioRequest
{
    public int UserId { get; set; }
    public decimal TotalBalance { get; set; }

}

public class PortfolioResponse
{
    public int UserId { get; set; }

    public decimal TotalBalance { get; set; }

    public ICollection<PortfolioItemResponse> PortfolioItems { get; set; }
    public ICollection<TradeResponse> TradeLogs { get; set; }
}