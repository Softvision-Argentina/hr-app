namespace Core.Persistance
{
    public interface IUnitOfWork
    {
        int Complete();
    }
}
