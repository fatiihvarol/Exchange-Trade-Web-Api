namespace Web.Schema;

public class ShareRequest
{
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
    public int TotalAmount { get; set; }
}

public class ShareResponse
{
    public string Symbol { get; set; }
    public decimal CurrentPrice { get; set; }
    public int TotalAmount { get; set; }
}