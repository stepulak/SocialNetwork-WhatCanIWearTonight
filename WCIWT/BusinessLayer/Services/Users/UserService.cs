﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.DataTransferObjects;
using BusinessLayer.DataTransferObjects.Common;
using BusinessLayer.DataTransferObjects.Filters;
using BusinessLayer.QueryObjects.Common;
using BusinessLayer.Services.Common;
using EntityDatabase;
using System.Linq;
using WCIWT.Infrastructure;
using WCIWT.Infrastructure.Query;
using System.Security.Cryptography;

namespace BusinessLayer.Services.Users
{
    public class UserService : CrudQueryServiceBase<User, UserDto, UserFilterDto>, IUserService
    {
        private const int PBKDF2IterCount = 100000;
        private const int PBKDF2SubkeyLength = 160 / 8;
        private const int saltSize = 128 / 8;
        
        public UserService(IMapper mapper, IRepository<User> repository, 
             QueryObjectBase<UserDto, User, UserFilterDto, IQuery<User>> userQueryObject)
            : base(mapper, repository, userQueryObject)
        {
        }

        public async Task<Guid> RegisterUserAsync(UserCreateDto userDto)
        {
            var user = Mapper.Map<User>(userDto);

            if (await DoesUserExistAsync(user.Username))
            {
                throw new ArgumentException($"User {userDto.Username} already exists");
            }

            var password = CreateHash(userDto.Password);
            user.PasswordHash = password.Item1;
            user.PasswordSalt = password.Item2;

            Repository.Create(user);

            return user.Id;
        }

        public async Task<bool> AuthorizeUserAsync(string username, string password)
        {
            var userResult = await Query.ExecuteQuery(new UserFilterDto { Username = username });
            var user = userResult.Items.SingleOrDefault();
            return user != null && VerifyHashedPassword(user.PasswordHash, user.PasswordSalt, password);
        }
        
        public async Task<QueryResultDto<UserDto, UserFilterDto>> ListUsersAsync(UserFilterDto filter)
        {
            return await Query.ExecuteQuery(filter);
        }

        protected override Task<User> GetWithIncludesAsync(Guid entityId)
        {
            return Repository.GetAsync(entityId);
        }


        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            UserFilterDto filter = new UserFilterDto
            {
                Username = username
            };
            var result = await Query.ExecuteQuery(filter);
            return result.Items.FirstOrDefault();
        }
        
        private async Task<bool> DoesUserExistAsync(string username)
        {
            var queryResult = await Query.ExecuteQuery(new UserFilterDto { Username = username });
            return (queryResult.Items.Count() == 1);
        }

        private bool VerifyHashedPassword(string hashedPassword, string salt, string password)
        {
            var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            var saltBytes = Convert.FromBase64String(salt);

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, PBKDF2IterCount))
            {
                var generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
                return hashedPasswordBytes.SequenceEqual(generatedSubkey);
            }
        }

        private Tuple<string, string> CreateHash(string password)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, PBKDF2IterCount))
            {
                byte[] salt = deriveBytes.Salt;
                byte[] subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);

                return Tuple.Create(Convert.ToBase64String(subkey), Convert.ToBase64String(salt));
            }
        }
    }
}
