using Domain.Services.Contracts.Room;
using System.Collections.Generic;

namespace Domain.Services.Interfaces.Services
{
    public interface IRoomService
    {
        CreatedRoomContract Create(CreateRoomContract contract);
        ReadedRoomContract Read(int id);
        void Update(UpdateRoomContract contract);
        void Delete(int id);
        IEnumerable<ReadedRoomContract> List();
    }
}
