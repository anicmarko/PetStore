using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<UserEntity?> AddUserAsync(UserEntity user);

        Task SaveChanges();
    }
}
