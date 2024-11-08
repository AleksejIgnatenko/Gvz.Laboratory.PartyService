using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;

namespace Gvz.Laboratory.PartyService.Abstractions
{
    public interface IUserRepository
    {
        Task<Guid> CreateUserAsync(UserDto user);
        Task<UserEntity?> GetUserByIdAsync(Guid userId);
        Task<Guid> UpdateUserAsync(UserDto user);
    }
}