// <copyright file="UserService.cs" company="Softvision">
// Copyright (c) Softvision. All rights reserved.
// </copyright>

namespace Domain.Services.Impl.Services
{
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

    public class UserService : IUserService
    {
        private readonly IMapper mapper;
        private readonly IRepository<User> userRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IMapper mapper, IRepository<User> userRepository, IUnitOfWork unitOfWork, ILog<UserService> log)
        {
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        public ReadedUserContract Authenticate(string username, string password)
        {
            var user = this.Login(username, password);

            if (user != null)
            {
                return this.mapper.Map<ReadedUserContract>(user);
            }
            else
            {
                return null;
            }
        }

        public ReadedUserContract AuthenticateExternal(string username)
        {
            var user = this.ExternalLogin(username);
            var getUser = this.userRepository.QueryEager().FirstOrDefault(x => x.Username == username);

            if (getUser == null && this.IsSoftvisionEmail(username))
            {
                var newUser = new User()
                {
                    Role = Roles.Employee,
                    Username = username,
                };

                this.userRepository.Create(newUser);

                this.unitOfWork.Complete();

                return this.mapper.Map<ReadedUserContract>(newUser);
            }
            else if (user != null)
            {
                return this.mapper.Map<ReadedUserContract>(user);
            }
            else
            {
                return null;
            }
        }

        private bool IsSoftvisionEmail(string user)
        {
            var email = user.Split("@");

            if (email[1] == "softvision.com")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<ReadedUserContract> GetAll()
        {
            var userQuery = this.userRepository.QueryEager();

            var users = userQuery.ToList();

            return this.mapper.Map<List<ReadedUserContract>>(users);
        }

        public IEnumerable<ReadedUserContract> GetFilteredForTech()
        {
            var role = Roles.TechnicalInterviewer;
            var userQuery = this.userRepository.QueryEager().Where(x => x.Role == role
            || x.Username == "mariana.castrofreyre@softvision.com"
            || x.Username == "mauro.falduto@softvision.com"
            || x.Username == "gonzalo.vazquez@softvision.com");

            var users = userQuery.ToList();

            return this.mapper.Map<List<ReadedUserContract>>(users);
        }

        public IEnumerable<ReadedUserContract> GetFilteredForHr()
        {
            var userQuery = this.userRepository.QueryEager()
                .Where(x =>
                x.Role == Roles.HRManagement
                || x.Role == Roles.HRUser
                || x.Role == Roles.Recruiter
                || x.Username == "mariana.castrofreyre@softvision.com");

            var users = userQuery.ToList();

            return this.mapper.Map<List<ReadedUserContract>>(users);
        }

        public ReadedUserContract GetById(int id)
        {
            var user = this.userRepository.Query().FirstOrDefault(x => x.Id == id);

            return this.mapper.Map<ReadedUserContract>(user);
        }

        public ReadedUserRoleContract GetUserRole(string username)
        {
            var user = this.userRepository.Query().FirstOrDefault(x => x.Username.ToUpper() == username.ToUpper());
            return this.mapper.Map<ReadedUserRoleContract>(user);
        }

        private User Login(string username, string password)
        {
            var user = this.userRepository.Query()
                .Include(r => r.Community)
                .FirstOrDefault(x => x.Username == username && x.Password == HashUtility.GetStringSha256Hash(password));

            return user;
        }

        private User ExternalLogin(string username)
        {
            var user = this.userRepository.Query()
                .Include(r => r.Community)
                .FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return null;
            }

            return user;
        }
    }
}
