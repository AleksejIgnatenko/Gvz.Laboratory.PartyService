using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.PartyService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GvzLaboratoryPartyServiceDbContext _context;

        public UserRepository(GvzLaboratoryPartyServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateUserAsync(UserDto user)
        {

            var userEntity = new UserEntity
            {
                Id = user.Id,
                Surname = user.Surname,
                UserName = user.UserName,
                Patronymic = user.Patronymic,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<UserEntity?> GetUserByIdAsync(Guid userId)
        {
            var userEntities = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            return userEntities;
        }

        public async Task<Guid> UpdateUserAsync(UserDto user)
        {
            await _context.Users
                .Where(u => u.Id == user.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.Surname, user.Surname)
                    .SetProperty(u => u.UserName, user.UserName)
                    .SetProperty(u => u.Patronymic, user.Patronymic));

            return user.Id;
        }
    }
}
