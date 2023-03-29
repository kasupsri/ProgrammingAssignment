public class Order
{
    public string BuyerSellerId { get; set; } = string.Empty!;
    public string Instrument { get; set; } = string.Empty!;
    public int SignedQuantity { get; set; }
    public double LimitPrice { get; set; }
    public int RemainingQuantity { get; set; }
    public static Order Parse(string s)
    {
        var parts = s.Split(':');
        var signedQuantity = int.Parse(parts[2]);
        return new Order
        {
            BuyerSellerId = parts[0],
            Instrument = parts[1],
            SignedQuantity = signedQuantity,
            LimitPrice = double.Parse(parts[3]),
            RemainingQuantity = signedQuantity
        };
    }

    public static bool TryParse(string s, out Order? order)
    {
        var parts = s.Split(':');
        if (parts.Length != 4)
        {
            order = null;
            return false;
        }

        bool isIntParsed = int.TryParse(parts[2], out var signedQuantity);
        bool isDoubleParsed = double.TryParse(parts[3], out var limitPrice);

        if (isIntParsed && isDoubleParsed)
        {
            order = new Order
            {
                BuyerSellerId = parts[0],
                Instrument = parts[1],
                SignedQuantity = signedQuantity,
                LimitPrice = limitPrice,
                RemainingQuantity = signedQuantity
            };
            return true;
        }
        else
        {
            order = null;
            return false;
        }
    }

    public override string ToString()
    {
        return $"{BuyerSellerId}:{Instrument}:{SignedQuantity - RemainingQuantity}:{LimitPrice}";
    }
}