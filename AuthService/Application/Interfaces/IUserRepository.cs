using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
}
