namespace InveonBootcamp.SOLID_Principles;

//DIP UYGUN OLMAYAN KOD

//Low-level module
public class Android
{
    public void UpdateGame()
    {
        Console.WriteLine("Updating from Google Play.");
    }
}

//Low-level module
public class Ios
{
    public void UpdateGame()
    {
        Console.WriteLine("Updating from App Store.");
    }
}

//Low-level module
public class Windows
{
    public void UpdateGame()
    {
        Console.WriteLine("Updating from Steam.");
    }
}

//High-level module
public class Game
{
    //High-level module, low-level modüllere doğrudan bağımlı.
    private readonly Android _android = new();
    private readonly Ios _ios = new();
    private readonly Windows _windows = new();

    public void UpdateGame()
    {
        _android.UpdateGame();
        _ios.UpdateGame();
        _windows.UpdateGame();
    }
}

//DIP UYGUN OLAN KOD
public interface IPlatform
{
    void UpdateGame();
}

public class Android2 : IPlatform
{
    public void UpdateGame()
    {
        Console.WriteLine("Updating from Google Play.");
    }
}

public class Ios2 : IPlatform
{
    public void UpdateGame()
    {
        Console.WriteLine("Updating from App Store.");
    }
}

public class Windows2 : IPlatform
{
    public void UpdateGame()
    {
        Console.WriteLine("Updating from Steam.");
    }
}

public class Game2(List<IPlatform> platforms)
{
    public void UpdateGame()
    {
        foreach (var platform in platforms) platform.UpdateGame();
    }
}

/*
internal class DipProgram
{
    private static void Main(string[] args)
    {
        var game = new Game();
        game.UpdateGame();

        var game2 = new Game2([new Android2(), new Ios2(), new Windows2()]);
        game2.UpdateGame();
    }
}
*/