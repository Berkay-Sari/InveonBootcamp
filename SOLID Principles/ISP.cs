namespace InveonBootcamp.SOLID_Principles;

//ISP UYGUN OLMAYAN KOD
public interface IHasCryptoTrading
{
    void BuySpotCrypto(string tokenId, int quantity);
    void SellSpotCrypto(string tokenId, int quantity);

    void BuyFutureCrypto(string tokenId, int quantity, int leverage);
    void SellFutureCrypto(string tokenId, int quantity, int leverage);
}

public class BasicCryptoTrader : IHasCryptoTrading
{
    public void BuySpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Buying spot crypto...");
    }

    public void SellSpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Selling spot crypto...");
    }

    public void BuyFutureCrypto(string tokenId, int quantity, int leverage)
    {
        Console.WriteLine("You must pass the qualification test to trade futures.");
    }

    public void SellFutureCrypto(string tokenId, int quantity, int leverage)
    {
        Console.WriteLine("You must pass the qualification test to trade futures.");
    }
}

public class AdvancedCryptoTrader : IHasCryptoTrading
{
    public void BuySpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Buying spot crypto...");
    }

    public void SellSpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Selling spot crypto...");
    }

    public void BuyFutureCrypto(string tokenId, int quantity, int leverage)
    {
        Console.WriteLine("Buying future crypto...");
    }

    public void SellFutureCrypto(string tokenId, int quantity, int leverage)
    {
        Console.WriteLine("Selling future crypto...");
    }
}

//ISP UYGUN OLAN KOD
public interface IHasSpotCryptoTrading
{
    void BuySpotCrypto(string tokenId, int quantity);
    void SellSpotCrypto(string tokenId, int quantity);
}

public interface IHasFutureCryptoTrading
{
    void BuyFutureCrypto(string tokenId, int quantity, int leverage);
    void SellFutureCrypto(string tokenId, int quantity, int leverage);
}

public class BasicCryptoTrader2 : IHasSpotCryptoTrading
{
    public void BuySpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Buying spot crypto...");
    }

    public void SellSpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Selling spot crypto...");
    }
}

public class AdvancedCryptoTrader2 : IHasSpotCryptoTrading, IHasFutureCryptoTrading
{
    public void BuyFutureCrypto(string tokenId, int quantity, int leverage)
    {
        Console.WriteLine("Buying future crypto...");
    }

    public void SellFutureCrypto(string tokenId, int quantity, int leverage)
    {
        Console.WriteLine("Selling future crypto...");
    }

    public void BuySpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Buying spot crypto...");
    }

    public void SellSpotCrypto(string tokenId, int quantity)
    {
        Console.WriteLine("Selling spot crypto...");
    }
}

/*
internal class IspProgram
{
    private static void Main(string[] args)
    {
        var basicCryptoTrader = new BasicCryptoTrader();
        basicCryptoTrader.BuySpotCrypto("BTC", 1);
        basicCryptoTrader.SellSpotCrypto("BTC", 1);
        basicCryptoTrader.BuyFutureCrypto("BTC", 1, 10);
        basicCryptoTrader.SellFutureCrypto("BTC", 1, 10);
        var advancedCryptoTrader = new AdvancedCryptoTrader();
        advancedCryptoTrader.BuySpotCrypto("BTC", 1);
        advancedCryptoTrader.SellSpotCrypto("BTC", 1);
        advancedCryptoTrader.BuyFutureCrypto("BTC", 1, 10);
        advancedCryptoTrader.SellFutureCrypto("BTC", 1, 10);
        var basicCryptoTrader2 = new BasicCryptoTrader2();
        basicCryptoTrader2.BuySpotCrypto("BTC", 1);
        basicCryptoTrader2.SellSpotCrypto("BTC", 1);
        var advancedCryptoTrader2 = new AdvancedCryptoTrader2();
        advancedCryptoTrader2.BuySpotCrypto("BTC", 1);
        advancedCryptoTrader2.SellSpotCrypto("BTC", 1);
        advancedCryptoTrader2.BuyFutureCrypto("BTC", 1, 10);
        advancedCryptoTrader2.SellFutureCrypto("BTC", 1, 10);
    }
}
*/