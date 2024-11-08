namespace Gvz.Laboratory.PartyService.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Surname { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Patronymic { get; set; } = string.Empty;
        public List<PartyEntity> Parties { get; set; } = new List<PartyEntity>();
    }
}
