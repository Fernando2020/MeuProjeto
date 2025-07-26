namespace MeuProjeto.Core.Data
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
