namespace InveonBootcamp.Async_Programming;

public class AsyncErrorHandling
{
    /*
    public static async Task Main(string[] args)
    {
        await FetchHtmlFromHostsAsync();
    }
    */

    public static async Task FetchHtmlFromHostsAsync()
    {
        Task? allTasks = null;
        List<Task<string>> htmlList =
        [
            FetchHtmlAsync("https://www.google.com"),
            FetchHtmlAsync("https://www.slaqwdqwd.com"),
            FetchHtmlAsync("https://www.qwqqqqq.com")
        ];
        try
        {
            allTasks = Task.WhenAll(htmlList);
            await allTasks;

            foreach (var html in htmlList) Console.WriteLine($"Html length: {html.Result.Length}");
        }
        catch
        {
            Console.WriteLine("The following exceptions have occurred:");

            AggregateException? allExceptions = allTasks?.Exception;

            if (allExceptions != null)
            {
                foreach (var ex in allExceptions.InnerExceptions)
                {
                    Console.WriteLine($"Exception Type: {ex.GetType()}, Message: {ex.Message}");
                }
            }
        }

    }

    private static async Task<string> FetchHtmlAsync(string url)
    {
        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request exception for {url}: {ex.Message}");
            throw; // Call stack orjinal hatayı içerecek.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected exception for {url}: {ex.Message}");
            throw; // Call stack orjinal hatayı içerecek.
        }
    }
}