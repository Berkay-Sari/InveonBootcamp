namespace LibraryManagement.Web.Data.Repositories;

public interface IUnitOfWork
{
    Task<int> CompleteAsync();
}