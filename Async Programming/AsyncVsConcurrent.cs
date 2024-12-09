using System.Text.Json;

namespace InveonBootcamp.Async_Programming;

public class AsyncVsConcurrent
{
    private const string ApiUrl = "https://restcountries.com/v3.1/alpha/";

    private readonly HttpClient _client = new();

    private readonly List<string> _countryCodes =
    [
        "TR", "US", "DE", "FR", "RU", "JP", "HR", "IT", "ES", "GB", "QA", "AU", "BE", "BR", "CA", "CN", "DK", "EG",
        "FI", "GR", "HU", "IN", "ID", "IR", "IQ", "IE", "IL", "JO", "KZ", "KW", "LB", "MY", "MX", "MA", "NL", "NZ",
        "NO", "PK", "PH", "PL", "PT", "RO", "SA", "RS", "SG", "ZA", "KR", "SE", "CH", "SY", "TH", "TN", "UA", "AE", "VN"
    ];

    private void PrintCountryData(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode) return;
        var jsonResponse = response.Content.ReadAsStringAsync().Result;
        var doc = JsonDocument.Parse(jsonResponse);
        var country = doc.RootElement[0];

        var name = country.GetProperty("name").GetProperty("common").GetString();
        var population = country.GetProperty("population").GetInt64();
        var startOfWeek = country.GetProperty("startOfWeek").GetString();

        Console.WriteLine(
            $"Ülke: {name}, Nüfus: {population}, Haftanın Başlangıcı: {startOfWeek}");

        doc.Dispose();
    }

    // Asenkron metot
    public async Task FetchCountryDataAsync()
    {
        Console.WriteLine("Asenkron requestler gönderiliyor...");
        var tasks = _countryCodes
            .Select(code => _client.GetAsync(ApiUrl + code))
            .ToList();

        var responses = await Task.WhenAll(tasks); //tüm requstler dönene kadar bekler

        Console.WriteLine("Asenkron request sonuçları geldi:");
        foreach (var response in responses) PrintCountryData(response); //country listesindeki sırayla yazdırır.
    }

    // Paralel metot
    public ParallelLoopResult FetchCountryDataMultiThread()
    {
        var tasks = Parallel.ForEach(_countryCodes, code =>
        {
            var response = _client.GetAsync(ApiUrl + code).Result;
            PrintCountryData(response); //random sırayla yazdırır.
        });

        return tasks;
    }

    /*
    public static async Task Main(string[] args)
    {
        var asyncVsConcurrent = new AsyncVsConcurrent();

        await asyncVsConcurrent.FetchCountryDataAsync();

        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("Paralel Sonuçlar: ");
        asyncVsConcurrent.FetchCountryDataMultiThread();
    }
    */
}