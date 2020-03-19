using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core;
using Core.Persistance;
using Domain.Model;
using Domain.Services.Contracts.User;
using Domain.Services.Impl.Services;
using Moq;
using Xunit;

namespace Domain.Services.Tests.Impl.Services
{
    public class UserServiceTest : BaseDomainTest
    {
        private readonly UserService service;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepository<User>> mockUserRepository;
        private readonly Mock<IUnitOfWork> mockUnitOfWork;
        private readonly Mock<ILog<UserService>> mockLog;

        public UserServiceTest()
        {
            mockMapper = new Mock<IMapper>();
            mockUserRepository = new Mock<IRepository<User>>();
            mockLog = new Mock<ILog<UserService>>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            service = new UserService(mockMapper.Object, mockUserRepository.Object, mockUnitOfWork.Object, mockLog.Object);
        }

        [Fact(DisplayName = "Verify that authenticate returns ReadedUserContract when user and password is valid")]
        public void Should_ReturnReadedUserContract_When_AuthenticateUserAndPasswordIsValid()
        {
            string username = "testUser";
            string password = "testPass";
            var queryValue = new List<User>()
                { new User()
                    {
                        Username = username,
                        Password = HashUtility.GetStringSha256Hash(password)
                    }
                }
                .AsQueryable();
            var expectedValue = new ReadedUserContract();
            mockUserRepository.Setup(_ => _.Query()).Returns(queryValue);
            mockMapper.Setup(_ => _.Map<ReadedUserContract>(It.IsAny<User>())).Returns(expectedValue);

            var readedUser = service.Authenticate(username, password);

            Assert.NotNull(readedUser);
            Assert.Equal(expectedValue, readedUser);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserContract>(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that authenticate returns null when user and password is invalid")]
        public void Should_ReturnNull_When_AuthenticateUserAndPasswordIsInvalid()
        {
            string username = null;
            string password = null;

            var readedUser = service.Authenticate(username, password);

            Assert.Null(readedUser);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(mm => mm.Map<ReadedUserContract>(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that authenticate returns ReadedUserContract when user is valid")]
        public void Should_ReturnReadedUserContract_When_AuthenticateUserIsValid()
        {
            string username = "testUser";
            var queryValue = new List<User>()
                { new User()
                    {
                        Username = username
                    }
                }
                .AsQueryable();
            var expectedValue = new ReadedUserContract();
            mockUserRepository.Setup(_ => _.Query()).Returns(queryValue);
            mockMapper.Setup(_ => _.Map<ReadedUserContract>(It.IsAny<User>())).Returns(expectedValue);

            var readedUser = service.Authenticate(username);

            Assert.NotNull(readedUser);
            Assert.Equal(expectedValue, readedUser);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserContract>(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that authenticate returns null when user is invalid")]
        public void Should_ReturnNull_When_AuthenticateUserdIsInvalid()
        {
            string username = "testUser";

            var readedUser = service.Authenticate(username);

            Assert.Null(readedUser);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserContract>(It.IsAny<User>()), Times.Never);
        }

        [Fact(DisplayName = "Verify that get all returns ReadedUserContract list")]
        public void Should_ReturnReadedUserContracts_When_Getall()
        {
            string username = "testUser";
            var queryValue = new List<User>()
                { new User()
                    {
                        Username = username
                    }
                }
                .AsQueryable();
            var expectedValues = new List<ReadedUserContract>();
            mockUserRepository.Setup(_ => _.QueryEager()).Returns(queryValue);
            mockMapper.Setup(_ => _.Map<List<ReadedUserContract>>(It.IsAny<List<User>>())).Returns(expectedValues);

            var readedUsers = service.GetAll();

            Assert.NotNull(readedUsers);
            Assert.Equal(expectedValues, readedUsers);
            mockUserRepository.Verify(_ => _.QueryEager(), Times.Once);
            mockMapper.Verify(_ => _.Map<List<ReadedUserContract>>(It.IsAny<List<User>>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get by id returns ReadedUserContract when data is valid")]
        public void Should_ReturnReadedUserContract_When_GetByIdDataIsValid()
        {
            int id = 0;
            var queryValue = new List<User>()
                { new User()
                    {
                        Id = id
                    }
                }
                .AsQueryable();
            var expectedValue = new ReadedUserContract();
            mockUserRepository.Setup(_ => _.Query()).Returns(queryValue);
            mockMapper.Setup(_ => _.Map<ReadedUserContract>(It.IsAny<User>())).Returns(expectedValue);

            var readedUsers = service.GetById(id);

            Assert.NotNull(readedUsers);
            Assert.Equal(expectedValue, readedUsers);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserContract>(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get by id returns null when data is invalid")]
        public void Should_ReturnReadedUserContracts_When_GetByIdDataIsInvalid()
        {
            int id = 0;

            var readedUsers = service.GetById(id);

            Assert.Null(readedUsers);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserContract>(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get user role by user name returns ReadedUserRoleContract when data is valid")]
        public void Should_ReturnReadedUserRoleContract_When_GetUserRoleDataIsValid()
        {
            string username = "testUser";
            var queryValue = new List<User>()
                { new User()
                    {
                        Username= username
                    }
                }
                .AsQueryable();
            var expectedValue = new ReadedUserRoleContract();
            mockUserRepository.Setup(_ => _.Query()).Returns(queryValue);
            mockMapper.Setup(_ => _.Map<ReadedUserRoleContract>(It.IsAny<User>())).Returns(expectedValue);

            var readedUsers = service.GetUserRole(username);

            Assert.NotNull(readedUsers);
            Assert.Equal(expectedValue, readedUsers);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserRoleContract>(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Verify that get user role by user name returns null when data is invalid")]
        public void Should_ReturnNull_When_GetUserRoleDataIsInvalid()
        {
            string username = "testUser";

            var readedUsers = service.GetUserRole(username);

            Assert.Null(readedUsers);
            mockUserRepository.Verify(_ => _.Query(), Times.Once);
            mockMapper.Verify(_ => _.Map<ReadedUserRoleContract>(It.IsAny<User>()), Times.Once);
        }
    }
}
