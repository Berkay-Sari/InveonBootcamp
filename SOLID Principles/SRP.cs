namespace InveonBootcamp.SOLID_Principles;

//SRP'YE UYGUN OLMAYAN KOD
internal class OldStockModule(int quantity)
{
    public int Quantity { get; set; } = quantity;

    public void DecreaseStock(int quantity)
    {
        Quantity -= quantity;
        Console.WriteLine($"Stoktan {quantity} ürün azaldı.");
    }
}

internal class OldOrderModule(OldStockModule oldStockModule)
{
    public void PlaceOrder(int quantity)
    {
        oldStockModule.DecreaseStock(quantity);
        //Stok modulüne ait bir metot doğrudan çağrılıyor.
        //coupling artıyor.
        Console.WriteLine("Sipariş alındı.");
    }
}

//SRP'YE UYGUN OLAN EVENT BASED KOD (Observer Pattern)
internal class NewStockModule(int quantity)
{
    public int Quantity { get; set; } = quantity;

    public void DecreaseStock(object? sender, OrderEventArgs e)
    {
        Quantity -= e.Quantity;
        Console.WriteLine($"Stoktan {e.Quantity} ürün azaldı.");
    }
}

internal class OrderEventArgs : EventArgs
{
    public int Quantity { get; set; }
}

internal class NewOrderModule
{
    public event EventHandler<OrderEventArgs>? OrderPlaced;

    public void PlaceOrder(int quantity)
    {
        var data = new OrderEventArgs
        {
            Quantity = quantity
        };
        OrderPlaced?.Invoke(this, data);
        //Event aracılığıyla stok modülüne ait metot tetikleniyor.
        Console.WriteLine("Sipariş alındı.");
    }
}

/*
internal class SrpProgram
{
    private static void Run(string[] args)
    {
        OldStockModule oldStockModule = new(10);
        OldOrderModule oldOrderModule = new(oldStockModule);
        oldOrderModule.PlaceOrder(2);

        NewStockModule newStockModule = new(10);
        NewOrderModule newOrderModule = new();

        newOrderModule.OrderPlaced += newStockModule.DecreaseStock;

        newOrderModule.PlaceOrder(2);
    }
}
*/