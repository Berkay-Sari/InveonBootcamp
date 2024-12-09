namespace InveonBootcamp.SOLID_Principles;

//LSP UYGUN OLMAYAN KOD
public abstract class OldAnimal
{
    public void Eat()
    {
        Console.WriteLine("Animal is eating.");
    }

    public void Sleep()
    {
        Console.WriteLine("Animal is sleeping.");
    }

    public abstract void Fly();
}

public class OldBird : OldAnimal
{
    public void LayEggs()
    {
        Console.WriteLine("Bird is laying eggs.");
    }

    public override void Fly()
    {
        Console.WriteLine("Bird is flying.");
    }
}

public class OldLion : OldAnimal
{
    public void Hunt()
    {
        Console.WriteLine("Lion is hunting.");
    }

    public override void Fly()
    {
        throw new NotImplementedException();
    }
}

//LSP UYGUN OLAN KOD
public interface IFlyable
{
    void Fly();
}

public class NewAnimal
{
    public void Eat()
    {
        Console.WriteLine("Animal is eating.");
    }

    public void Sleep()
    {
        Console.WriteLine("Animal is sleeping.");
    }
}

public class NewBird : NewAnimal, IFlyable
{
    public void Fly()
    {
        Console.WriteLine("Bird is flying.");
    }

    public void LayEggs()
    {
        Console.WriteLine("Bird is laying eggs.");
    }
}

public class NewLion : NewAnimal
{
    public void Hunt()
    {
        Console.WriteLine("Lion is hunting.");
    }
}

/*
internal class Lsp
{
    private static void Main(string[] args)
    {

        OldAnimal bird = new OldBird();
        bird.Eat();
        bird.Sleep();
        bird.Fly();
        OldAnimal lion = new OldLion();
        lion.Eat();
        lion.Sleep();
        lion.Fly(); //Hata verir çünkü Lion uçamaz.
        
        NewAnimal bird = new NewBird();
        bird.Eat();
        bird.Sleep();
        if (bird is IFlyable flyableBird) flyableBird.Fly();
        NewAnimal lion = new NewLion();
        lion.Eat();
        lion.Sleep();
        if (lion is IFlyable flyableLion) flyableLion.Fly();
    }
}
*/