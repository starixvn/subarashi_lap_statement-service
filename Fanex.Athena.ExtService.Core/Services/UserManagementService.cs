namespace Fanex.Athena.ExtService.Core.Services
{
    using System.Collections.Generic;
    using Extensions;
    using Microsoft.Extensions.Options;
    using Models.DTO;

    public class UserManagementService : IUserManagementService
    {
        private readonly List<User> _users;

        public UserManagementService(IOptions<List<User>> userListAccessor)
        {
            _users = userListAccessor.Value;
        }

        public bool IsValidUser(string username, string password)
            => _users.Exists(x => x.UserName.Equals(username) && x.Password.Equals(password.Sha256()));
    }
}