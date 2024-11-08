using Gvz.Laboratory.PartyService.Entities;

namespace Gvz.Laboratory.PartyService.Models
{
    public class UserModel
    {
        public Guid Id { get; }
        public string Surname { get; } = string.Empty;
        public string UserName { get; } = string.Empty;
        public string Patronymic { get; } = string.Empty;

        public UserModel(Guid id, string surname, string userName, string patronymic)
        {
            Id = id;
            Surname = surname;
            UserName = userName;
            Patronymic = patronymic;
        }

        public UserModel()
        {
        }

        public static UserModel Create(Guid id, string surname, string userName, string patronymic)
        {
            UserModel user = new UserModel(id, surname, userName, patronymic);
            return user;
        }
    }
}
