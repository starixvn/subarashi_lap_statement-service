namespace Fanex.Athena.ExtService.Core.Services
{
    public interface IUserManagementService
    {
        bool IsValidUser(string username, string password);
    }
}