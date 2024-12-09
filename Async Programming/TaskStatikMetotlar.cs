namespace InveonBootcamp.Async_Programming;

// Kaynak: https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-9.0
public class TaskDelay
{
    /* Task.Delay()
     * Belirli bir zaman gecikmesinden sonra tamamlanacak bir görev oluşturur
     * Ancak bu sırada ana iş parçacığı meşgul edilmez ve diğer işlemler çalışmaya devam edebilir.
     * Senaryo: Bir oyunda arkamızdan bizi kovalayan canavar belli bir süre sonra koşmaya başlasın.
     */
    public static async Task TaskDelayExample()
    {
        Console.WriteLine("Canavar bekliyor...");
        await Task.Delay(5000); // 5 saniye sonra bitecek bir görev oluşturdu.
        Console.WriteLine("Canavar koşmaya başladı.");
    }
}

public class TaskFromCanceled
{
    /* Task.FromCanceled()
     * CancellationToken ile iptal nedeniyle tamamlanan bir Task oluşturur.
     * Senaryo: Dosya yüklemeyi iptal etmek isteyelim.
     */
    internal interface IFileProcessor
    {
        Task FileUploadTask(string filePath, CancellationToken cancellationToken);
    }

    public class FileProcessor : IFileProcessor
    {
        public Task FileUploadTask(string filePath, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);

            // Dosya yükleme işlemi

            return Task.CompletedTask;
        }
    }
    // Kaynak: https://stackoverflow.com/questions/67597488/whats-the-purpose-of-task-fromcanceled
}

public class TaskFromException
{
    /* Task.FromException()
     * Status propu Faulted olan ve Exception propu exception içeren bir Task nesnesi döner.
     * Genelde Task'in yapacağı işin exception atacağını tahmin edebiliyorsak,
     * execute etmesi uzun sürecek kodlara girmeden early return etmek için kullanılır.
     * Senaryo: İnput olarak aldığımız e-mail'i kontrol ederken, e-mail formatı hatalıysa hemen exception atalım.
     */

    public static Task SomeEmailTask(string email)
    {
        if (!email.Contains('@')) return Task.FromException(new FormatException("E-mail formatı hatalı."));

        return Task.Run(() =>
        {
            // execute etmesi uzun sürecek birtakım işlemler
        });
    }
}

public class TaskFromResult
{
    /* Task.FromResult()
     * Belirli bir değeri içeren ve tamamlanmış bir Task döndürür.
     * Genellikle async metodun asenkron çalışmak yerine hızlı bir şekilde tamamlanmış bir Task döndürebildiği durumlarda kullanılır.
     * Senaryo: Kullanıcı ismi cache'de varsa, cache'den alıp hemen döndürelim. Yoksa async olarak veritabanından alalım.
     */

    private readonly Dictionary<int, string> _userCache = new()
    {
        { 1, "ahmet" },
        { 2, "mehmet" }
    };

    public Task<string> GetUser(int userId)
    {
        if (_userCache.TryGetValue(userId, out var value)) return Task.FromResult(value);
        return GetUserFromDb(userId);
    }

    public Task<string> GetUserFromDb(int userId)
    {
        return Task.Run(() => "db'den gelen isim");
    }
}

public class TaskRun
{
    /* Task.Run()
     * Bodysindeki işi ThreadPool'da çalıştırmak üzere sıraya koyar. Bu execution ana iş parçacığından bağımsız olarak çalışır.
     * Senaryo: uzun sürecek bir loglamamız var ve bunu non-blocking bir şekilde yapmak istiyoruz.
     */
    public static void LogAsync(string message)
    {
        Task.Run(() =>
        {
            //logger.log("Kaydetmesi uzun sürecek log işlemi.");
            Console.WriteLine("arka planda çalışır.");
        });
    }
}

public class TaskWhenAll
{
    /* Task.WhenAll()
     * Parametre olarak aldığı tüm görevler tamamlanınca bitecek olan awaitable bir task oluşturur. Main thread'i bloklamaz.
     * asenkron görevlerin sonuçlarını beklemek için kullanılır.
     * TimeSpan ya da CancellationToken parametreleri ile timeout süresi belirlenebilir.
     * Seneryo: iki farklı db'den veri çekip birleştirmek istiyoruz.
     * Önce birine bağlanıp veriyi çekip sonra diğerine bağlanalım dersek 1. db süresi + 2. db süresi kadar beklememiz gerekecek.
     * Bunun yerine iki Task oluşturup WhenAll ile en uzun süreni bekleyelim.
     */
    public static async Task WhenAllExample()
    {
        var task1 = Task.Run(() => new Sample(1, "name1")); //1. db'den gelen veri
        var task2 = Task.Run(() => new Sample(2, "name2")); //2. db'den gelen veri

        var samples = await Task.WhenAll(task1, task2); //iki task de tamamlana kadar bekler

        foreach (var sample in samples) Console.WriteLine($"{sample.Id} {sample.Name}");
    }

    private record Sample(int Id, string Name);
}

public class TaskWhenAny
{
    /* Task.WhenAny()
     * Verilen görevlerden herhangi biri tamamlandığında tamamlanacak bir Task oluşturur.
     * WhenAll'dan tek farkı, ilk biten taskle birlikte tamamlanmasıdır. WhenAll'da tüm taskler bitmeli.
     * Seneryo: 3 farklı servisten veri çekip, en hızlısını kullanmak istiyoruz.
     */
    public static async Task WhenAnyExample()
    {
        var task1 = Task.Run(() =>
        {
            // 1. servisten veri çekme işlemi simulasyonu
            return "servis1";
        });

        var task2 = Task.Run(() =>
        {
            // 2. servisten veri çekme işlemi simulasyonu
            return "servis2";
        });

        var task3 = Task.Run(() =>
        {
            // 3. servisten veri çekme işlemi simulasyonu
            return "servis3";
        });

        var tasks = new[] { task1, task2, task3 };
        var firstTask = await Task.WhenAny(tasks); //ilk tamamlanan task'ı bekler
        Console.WriteLine(firstTask.Result);
    }
}

public class TaskWhenEach
{
    /* Task.WhenEach() .NET9 ile hayatımıza girmiştir.
     * Task.WhenAll ve Task.WhenAny'nin sınırlamalarını ortadan kaldırır.
     * WhenAll'da tüm taskleri bekliyorduk, WhenAny'de ilk biten taski alıp diğerlerini ignore ediyorduk.
     * WhenEach'te taskleri tamamlandıkça işliyoruz.
     * Bunu elimize her task tamamlandığında iterate etmemizi sağlayan bir IAsyncEnumerable<Task> vererek sağlıyor.
     * Senaryo:Birden fazla HttpClient çağrısı yapıp, bunlar tamamlandıkça sonuçları üzerinde işlem yapmak istiyoruz.
     */

    public static async Task WhenEachExample()
    {
        using HttpClient http = new();
        var tasks = new List<Task<HttpResponseMessage>>
        {
            http.GetAsync("https://jsonplaceholder.typicode.com/posts/1"),
            http.GetAsync("https://jsonplaceholder.typicode.com/posts/2"),
            http.GetAsync("https://jsonplaceholder.typicode.com/posts/3")
        };

        await foreach (var task in Task.WhenEach(tasks)) ProcessData(await task);
    }

    private static void ProcessData(HttpResponseMessage response)
    {
        var content = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(content);
    }
}

public class TaskWaitAll
{
    /* Task.WaitAll()
     * Parametresinde aldığı tüm taskler execution'ını tamamlayana kadar çağrıldığı thread'i bloklayan sync. bir metottur.
     * WhenAll'un aksine awaitable bir Task dönmez yani await keywordu kullanılamaz.
     * Senaryo: İki resmin benzerliklerini local feature'lar üzerinden karşılaştıran bir uygulamamız olsun,
     * featureları dönen SURF algoritmasının iki resim üzerinde de bitmesini bekleyelelim.
     */
    private static FeatureSet[] ExtractSURFFeatures(string imagePath)
    {
        Console.WriteLine($"{imagePath} için SURF özellikleri çıkarılıyor...");
        var random = new Random();
        var featureCount = random.Next(50, 150);
        var features = new FeatureSet[featureCount];

        // Temsili Random FeatureSet oluşsun. (normalde algoritmadan döner)
        for (var i = 0; i < featureCount; i++)
        {
            // SURF'ün Descriptor length'i sabit (64 boyutlu)
            var descriptors = new float[64];
            for (var j = 0; j < 64; j++) descriptors[j] = (float)random.NextDouble();

            features[i] = new FeatureSet
            {
                X = random.Next(0, 1000),
                Y = random.Next(0, 1000),
                Descriptors = descriptors
            };
        }

        Console.WriteLine($"{imagePath} için SURF özellikleri çıkarıldı.");
        return features;
    }

    public static void WaitAllExample()
    {
        var task1 = Task.Run(() => ExtractSURFFeatures("imagePath1"));
        var task2 = Task.Run(() => ExtractSURFFeatures("imagePath2"));

        Task.WaitAll(task1, task2); // Bu noktada iki resmin de SURF feature'ları çıkarılmış olacak.

        // Artık karşılaştırabiliriz.
        // CompareFeatures(task1.Result, task2.Result);
    }

    private record FeatureSet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public required float[] Descriptors { get; set; }
    }
}

public class TaskWaitAny
{
    /* Task.WaitAny()
     * Verilen Tasklerden herhangi biri tamamlanana kadar ana iş parçacığını bloklar.Tamamlnan taskin indexini döner. WaitAll gibi bu da senkron.
     * Senaryo: Yine farklı enpointlere istek atıp en hızlısını kullanmak isteyebiliriz. WhenAll'dan farklı istek dönene kadar başka işlem yapmaz thread.
     */

    public static void WaitAnyExample()
    {
        using HttpClient http = new();
        var request1 = http.GetAsync("https://jsonplaceholder.typicode.com/posts/1");
        var request2 = http.GetAsync("https://jsonplaceholder.typicode.com/posts/2");

        Task[] tasks = [request1, request2];
        var finishedTaskIndex = Task.WaitAny(tasks); //ilk tamamlanan task'ın indexini döner

        //ProcessData(tasks[firstTask].Result);
    }
}

public class TaskYield
{
    /* Task.Yield()
     * await olduğunda geçerli contexte asenkron olarak geri dönen awaitable bir Task oluşturur.
     * asenkron bir metodun, gerçekten asenkron tamamlanmasına force'layabiliriz.
     * Senaryo: UI'da bir butona tıklandığında bir işlem başlasın ve bu işlemin UI'ı dondurmaması için hesaplama metodunda yield çağıralım.
     */
    public async Task<int> Calculate(int key)
    {
        await Task.Yield();
        //burdan sonra execution caller'dan ayrılır ve asenkron olarak free bir threadde devam eder.
        //caller'daki akış bloklanmadan devam eder.
        var j = 0;
        for (var i = 0; i < 999999999; i++)
        {
            j = j + 1;
            if (i % key == 0)
                Console.WriteLine($"i: {i}, j: {j}");
        }

        await Task.Delay(1000); //temsili bazı uzun işlemler
        return j;
    }

    public void UI()
    {
        var a = Calculate(100000000);
        //Calculate yield ile başlamasaydı asenkron metodun senkron kısmı "inline" olarak çalışacaktı.
        //UI await demese bile bu kısmın bitmesini bekleyecek ve donacaktı.

        Console.WriteLine("UI thread çalışıyor.");

        //Result'a kadar olan işlemler non-blocking devam eder.
        Console.WriteLine(a.Result);
        Console.WriteLine("bitti");
    }

    /*

    public static void Main(string[] args)
    {
        TaskYield taskYield = new();
        //Calculate metodunda yield'ı kaldırırsak "UI thread çalışıyor." konsola hesaplamalardan sonra gelcekti.
        taskYield.UI();
    }
    */
}