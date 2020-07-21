using Core.Persistance;
using Domain.Model;

namespace Domain.Services.Interfaces.Repositories
{
    public interface IProcessRepository : IRepository<Process>
    {
        Process GetByIdFullProcess(int id);
        void Approve(int id);
        void Reject(int id, string rejectionReason);
    }
}
