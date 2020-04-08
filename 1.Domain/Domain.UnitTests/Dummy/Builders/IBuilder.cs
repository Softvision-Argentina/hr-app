namespace Domain.Services.Impl.UnitTests.Dummy.Builders
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
