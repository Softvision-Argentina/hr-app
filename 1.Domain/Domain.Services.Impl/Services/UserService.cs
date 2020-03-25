using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.User;
using Domain.Services.Interfaces.Services;
using Domain.Services.Repositories.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            //var user = _dbcontext.Users.FirstOrDefault(x => x.Username == username && x.Password == HashUtility.GetStringSha256Hash(password));
            var user = _userRepository.Query()
                .Include(r => r.Community)
                .FirstOrDefault(x => x.Username == username && x.Password == HashUtility.GetStringSha256Hash(password));

            if (user == null)
                return null;

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
