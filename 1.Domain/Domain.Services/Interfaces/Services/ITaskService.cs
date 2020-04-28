using Domain.Services.Contracts.Task;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface ITaskService
    {
        CreatedTaskContract Create(CreateTaskContract contract);
        ReadedTaskContract Read(int id);
        IEnumerable<ReadedTaskContract> ListByUser(string userEmail);
        void Update(UpdateTaskContract contract);
        void Approve(int id);
        void Delete(int id);
        IEnumerable<ReadedTaskContract> List();
    }
}
