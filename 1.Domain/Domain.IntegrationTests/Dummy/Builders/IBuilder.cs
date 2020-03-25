namespace Domain.Services.Impl.IntegrationTests.Dummy.Builders
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
