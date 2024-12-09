namespace InveonBootcamp.SOLID_Principles;

//OCP'YE UYGUN OLMAYAN KOD
internal class OldCalculator
{
    public static double Calculate(string operation, double num1, double num2)
    {
        double result = 0;
        switch (operation)
        {
            case "add":
                result = num1 + num2;
                break;
            case "sub":
                result = num1 - num2;
                break;
            case "mul":
                result = num1 * num2;
                break;
            case "div":
                if (num2 == 0) throw new DivideByZeroException();
                result = num1 / num2;
                break;

            /* Yeni bir işlem eklemek istediğimizde mevcut fonksiyonu değiştirmemiz gerekiyor.
            case "log":
                if (num1 <= 0 || num2 <= 0)
                {
                    throw new ArgumentException("Verilen sayılar log işlemi için uygun değil.");
                }
                result = Math.Log(num1, num2);
                break;
            */
        }

        return result;
    }
}

//OCP'YE UYGUN OLAN KOD
internal class NewCalculator
{
    public static double Calculate(Func<double, double, double> calculateFunc, double num1, double num2)
    {
        return calculateFunc(num1, num2);
    }

    public static double Add(double num1, double num2)
    {
        return num1 + num2;
    }

    public static double Sub(double num1, double num2)
    {
        return num1 - num2;
    }

    public static double Mul(double num1, double num2)
    {
        return num1 * num2;
    }

    public static double Div(double num1, double num2)
    {
        if (num2 == 0) throw new DivideByZeroException();
        return num1 / num2;
    }
}

/*
internal class OcpProgram
{
    private static void Main(string[] args)
    {
        Console.WriteLine(OldCalculator.Calculate("add", 5, 3));
        Console.WriteLine(OldCalculator.Calculate("sub", 5, 3));
        Console.WriteLine(OldCalculator.Calculate("mul", 5, 3));
        Console.WriteLine(OldCalculator.Calculate("div", 5, 3));

        Console.WriteLine(NewCalculator.Calculate(NewCalculator.Add, 5, 3));
        Console.WriteLine(NewCalculator.Calculate(NewCalculator.Sub, 5, 3));
        Console.WriteLine(NewCalculator.Calculate(NewCalculator.Mul, 5, 3));
        Console.WriteLine(NewCalculator.Calculate(NewCalculator.Div, 5, 3));

        Console.WriteLine(NewCalculator.Calculate(Log, 5, 3));

        //Herhangi bir custom işlem eklemek istediğimizde
        Console.WriteLine(NewCalculator.Calculate((num1, num2) => num1 * 3 + Math.Pow(num2, 2), 5, 3));
        return;

        //Log işlemi eklememiz istensin
        static double Log(double num1, double num2)
        {
            if (num1 <= 0 || num2 <= 0) throw new ArgumentException("Verilen sayılar log işlemi için uygun değil.");
            return Math.Log(num1, num2);
        }
    }
}
*/