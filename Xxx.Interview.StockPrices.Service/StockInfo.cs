using System.Diagnostics;

namespace Xxx.Interview.StockPrices.Service;

[DebuggerDisplay("Name=[{Name}], Price=[{Price}]")]
public readonly struct StockInfo
{
    public StockInfo(string name, DateTimeOffset timestamp, decimal price)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException(@"Stock name is invalid!", nameof(name));

        Name = string.Intern(name);
        Timestamp = timestamp;
        Price = price;
    }

    public string Name { get; }

    public DateTimeOffset Timestamp { get; }

    public decimal Price { get; }
}