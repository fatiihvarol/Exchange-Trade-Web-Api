using Base.Entity;

namespace Web.Data.Entity;

public class PortfolioItem:BaseEntity
{
    public int  ShareId { get; set; }
    public Share Share { get; set; }
    public int PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }

    public int Quantity { get; set; }


}