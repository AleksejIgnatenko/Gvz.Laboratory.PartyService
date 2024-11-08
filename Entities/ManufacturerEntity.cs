namespace Gvz.Laboratory.PartyService.Entities
{
    public class ManufacturerEntity
    {
        public Guid Id { get; set; }
        public string ManufacturerName { get; set; } = string.Empty;
        public List<PartyEntity> Parties { get; set; } = new List<PartyEntity>();
    }
}
