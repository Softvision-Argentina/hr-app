using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface ICvRepository
    {
        Cv GetCv(int id);
        bool SaveAll(Cv cv);
    }
}
