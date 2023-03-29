public class Exchange
{
    private readonly List<Order> orders = new List<Order>();
    private readonly List<Trade> trades = new List<Trade>();

    private IEnumerable<Order> GetMatchingCandidates(Order order) =>
        orders
            .Where(o => o.Instrument == order.Instrument && o.RemainingQuantity != 0)
            .OrderBy(o => order.SignedQuantity > 0 ? (o.SignedQuantity < 0 ? o.LimitPrice : double.MaxValue) : (o.SignedQuantity > 0 ? -o.LimitPrice : double.MinValue))
            .ThenBy(o => orders.IndexOf(o));

    private void ProcessOrder(Order? order)
    {
        if (order == null)
        {
            return;
        }

        var candidates = GetMatchingCandidates(order).ToList();

        foreach (var candidate in candidates)
        {
            if (candidate.SignedQuantity * order.SignedQuantity < 0 && (order.SignedQuantity > 0 ? order.LimitPrice >= candidate.LimitPrice : order.LimitPrice <= candidate.LimitPrice))
            {
                int matchQuantity = Math.Min(Math.Abs(candidate.RemainingQuantity), Math.Abs(order.RemainingQuantity));
                double matchPrice = candidate.LimitPrice;

                trades.Add(new Trade
                {
                    BuyerId = order.SignedQuantity > 0 ? order.BuyerSellerId : candidate.BuyerSellerId,
                    SellerId = order.SignedQuantity > 0 ? candidate.BuyerSellerId : order.BuyerSellerId,
                    Instrument = order.Instrument,
                    MatchQuantity = matchQuantity,
                    MatchPrice = matchPrice
                });

                candidate.RemainingQuantity -= matchQuantity * (candidate.SignedQuantity > 0 ? 1 : -1);
                order.RemainingQuantity -= matchQuantity * (order.SignedQuantity > 0 ? 1 : -1);

                if (order.RemainingQuantity == 0)
                    break;
            }
        }

        if (order.RemainingQuantity != 0)
            orders.Add(order);
    }

    public void Run()
    {
        string? line;
        while ((line = Console.ReadLine()) != null)
        {
            line = line.Trim();
            if (Order.TryParse(line, out var order) && order != null)
            {
                ProcessOrder(order);
            }
        }

        foreach (var trade in trades)
        {
            Console.WriteLine(trade);
        }
    }

    public List<Trade> GetTrades()
    {
        return trades;
    }
}