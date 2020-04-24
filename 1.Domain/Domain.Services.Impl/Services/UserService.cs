using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Model.Enum;
using Domain.Services.Contracts.User;
using Domain.Services.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services.Impl.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog<UserService> _log;

        public UserService(IMapper mapper, IRepository<User> userRepository,
                           IUnitOfWork unitOfWork, ILog<UserService> log)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _log = log;
        }

        public ReadedUserContract Authenticate(string username, string password)
        {
            var user = Login(username, password);

            if (user != null)
            {
                return _mapper.Map<ReadedUserContract>(user);
            }
            else
            {
                return null;
            }
        }

        public ReadedUserContract Authenticate(string username)
        {
            var user = ExternalLogin(username);

            if (user != null)
            {
                return _mapper.Map<ReadedUserContract>(user);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ReadedUserContract> GetAll()
        {
            var userQuery = _userRepository.QueryEager();

            var users = userQuery.ToList();

            return _mapper.Map<List<ReadedUserContract>>(users);
        }

        public IEnumerable<ReadedUserContract> GetFilteredForTech()
        {
            var role = Roles.TechnicalInterviewer;
            var userQuery = _userRepository.QueryEager().Where(x => x.Role == role
            || x.Username == "mariana.castrofreyre@softvision.com"
            || x.Username == "mauro.falduto@softvision.com"
            || x.Username == "gonzalo.vazquez@softvision.com");

            var users = userQuery.ToList();

            return _mapper.Map<List<ReadedUserContract>>(users);
        }

        public IEnumerable<ReadedUserContract> GetFilteredForHr()
        {
            var userQuery = _userRepository.QueryEager()
                .Where(x => 
                x.Role == Roles.HRManagement
                || x.Role == Roles.HRUser
                || x.Role == Roles.Recruiter
                || x.Username == "mariana.castrofreyre@softvision.com");

            var users = userQuery.ToList();

            return _mapper.Map<List<ReadedUserContract>>(users);
        }

        public ReadedUserContract GetById(int id)
        {
            var user = _userRepository.Query().FirstOrDefault(x => x.Id == id);

            return _mapper.Map<ReadedUserContract>(user);
        }

        public ReadedUserRoleContract GetUserRole(string username)
        {
            var user = _userRepository.Query().FirstOrDefault(x => x.Username.ToUpper() == username.ToUpper());
            return _mapper.Map<ReadedUserRoleContract>(user);
        }

        private User Login(string username, string password)
        {
            var user = _userRepository.Query()
                .Include(r => r.Community)
                .FirstOrDefault(x => x.Username == username && x.Password == HashUtility.GetStringSha256Hash(password));

            return user;
        }

        private User ExternalLogin(string username)
        {
            var user = _userRepository.Query()
                .Include(r => r.Community)
                .FirstOrDefault(x => x.Username == username );

            if (user == null)
                return null;

            return user;
        }
    }
}
