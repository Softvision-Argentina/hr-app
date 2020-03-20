namespace Domain.Services.Impl.UnitTests.Builders
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
