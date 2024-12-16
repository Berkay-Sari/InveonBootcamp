namespace LibraryManagement.Web.Data.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }
}