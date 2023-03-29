namespace Tests;

public class ExchangeTests
{
    private List<Trade> RunExchangeWithInput(string input)
    {
        using var stringReader = new StringReader(input);
        var originalConsoleIn = Console.In;
        Console.SetIn(stringReader);

        using var stringWriter = new StringWriter();
        var originalConsoleOut = Console.Out;
        Console.SetOut(stringWriter);

        try
        {
            var exchange = new Exchange();
            exchange.Run();

            var output = stringWriter.ToString();
            var outputLines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            return outputLines.Select(Trade.Parse).ToList();
        }
        finally
        {
            Console.SetIn(originalConsoleIn);
            Console.SetOut(originalConsoleOut);
        }
    }

    [Fact]
    public void Run_Example1_ReturnsCorrectTrades()
    {
        // Arrange
        string input = @"A:AUDUSD:100:1.47
                        B:AUDUSD:-50:1.45";
        List<Trade> expectedTrades = new List<Trade>
        {
            Trade.Parse("A:B:AUDUSD:50:1.47")
        };

        // Act
        List<Trade> actualTrades = RunExchangeWithInput(input);

        // Assert
        Assert.Equal(expectedTrades.Count, actualTrades.Count);
        for (int i = 0; i < expectedTrades.Count; i++)
        {
            Assert.Equal(expectedTrades[i].ToString(), actualTrades[i].ToString());
        }
    }

    [Fact]
    public void Run_Example2_ReturnsCorrectTrades()
    {
        // Arrange
        string input = @"A:GBPUSD:100:1.66
        B:EURUSD:-100:1.11
        F:EURUSD:-50:1.1
        C:GBPUSD:-10:1.5
        C:GBPUSD:-20:1.6
        C:GBPUSD:-20:1.7
        D:EURUSD:100:1.11";
        List<Trade> expectedTrades = new List<Trade>
        {
            Trade.Parse("A:C:GBPUSD:10:1.66"),
            Trade.Parse("A:C:GBPUSD:20:1.66"),
            Trade.Parse("D:F:EURUSD:50:1.1"),
            Trade.Parse("D:B:EURUSD:50:1.11")
        };

        // Act
        List<Trade> actualTrades = RunExchangeWithInput(input);

        // Assert
        Assert.Equal(expectedTrades.Count, actualTrades.Count);
        for (int i = 0; i < expectedTrades.Count; i++)
        {
            Assert.Equal(expectedTrades[i].ToString(), actualTrades[i].ToString());
        }
    }

    [Fact]
    public void TestOrdersDoNotMatch()
    {
        // Arrange
        string input = @"A:AUDUSD:100:1.48
                    B:AUDUSD:-100:1.49";
        List<Trade> expectedTrades = new List<Trade>();

        // Act
        List<Trade> actualTrades = RunExchangeWithInput(input);

        // Assert
        Assert.Equal(expectedTrades.Count, actualTrades.Count);
    }

    [Fact]
    public void TestDifferentInstrumentsDoNotMatch()
    {
        // Arrange
        string input = @"A:AUDUSD:100:1.47
                    B:EURUSD:-100:1.47";
        List<Trade> expectedTrades = new List<Trade>();

        // Act
        List<Trade> actualTrades = RunExchangeWithInput(input);

        // Assert
        Assert.Equal(expectedTrades.Count, actualTrades.Count);
    }

    [Fact]
    public void TestMultipleMatches()
    {
        // Arrange
        string input = @"A:AUDUSD:100:1.47
                    B:AUDUSD:-50:1.45
                    C:AUDUSD:-25:1.46
                    D:AUDUSD:-25:1.47";
        List<Trade> expectedTrades = new List<Trade>
    {
        Trade.Parse("A:B:AUDUSD:50:1.47"),
        Trade.Parse("A:C:AUDUSD:25:1.47"),
        Trade.Parse("A:D:AUDUSD:25:1.47")
    };

        // Act
        List<Trade> actualTrades = RunExchangeWithInput(input);

        // Assert
        Assert.Equal(expectedTrades.Count, actualTrades.Count);
        for (int i = 0; i < expectedTrades.Count; i++)
        {
            Assert.Equal(expectedTrades[i].ToString(), actualTrades[i].ToString());
        }
    }

    [Fact]
    public void TestSameBuyerSellerMatch()
    {
        // Arrange
        string input = @"A:AUDUSD:100:1.47
                    A:AUDUSD:-100:1.45";
        List<Trade> expectedTrades = new List<Trade>
    {
        Trade.Parse("A:A:AUDUSD:100:1.47")
    };

        // Act
        List<Trade> actualTrades = RunExchangeWithInput(input);

        // Assert
        Assert.Equal(expectedTrades.Count, actualTrades.Count);
        for (int i = 0; i < expectedTrades.Count; i++)
        {
            Assert.Equal(expectedTrades[i].ToString(), actualTrades[i].ToString());
        }
    }

    [Fact]
    public void TestPartialMatch()
    {
        // Arrange
        string input = @"A:AUDUSD:100:1.47
                    B:AUDUSD:-50:1.45";
        List<Trade> expectedTrades = new List<Trade>
    {
        Trade.Parse("A:B:AUDUSD:50:1.47")
    };

        // Act
        List<Trade> actualTrades = RunExchangeWithInput(input);

        // Assert
        Assert.Equal(expectedTrades.Count, actualTrades.Count);
        for (int i = 0; i < expectedTrades.Count; i++)
        {
            Assert.Equal(expectedTrades[i].ToString(), actualTrades[i].ToString());
        }
    }

}
