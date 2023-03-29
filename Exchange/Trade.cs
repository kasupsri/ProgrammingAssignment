public class Trade
{
    public string BuyerId { get; set; } = string.Empty!;
    public string SellerId { get; set; } = string.Empty!;
    public string Instrument { get; set; } = string.Empty!;
    public int MatchQuantity { get; set; }
    public double MatchPrice { get; set; }

    public override string ToString()
    {
        return $"{BuyerId}:{SellerId}:{Instrument}:{MatchQuantity}:{MatchPrice}";
    }

    public static Trade Parse(string s)
    {
        var parts = s.Split(':');
        return new Trade
        {
            BuyerId = parts[0],
            SellerId = parts[1],
            Instrument = parts[2],
            MatchQuantity = int.Parse(parts[3]),
            MatchPrice = double.Parse(parts[4])
        };
    }
}