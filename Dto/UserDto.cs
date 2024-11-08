namespace Gvz.Laboratory.PartyService.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Surname { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
    }
}
