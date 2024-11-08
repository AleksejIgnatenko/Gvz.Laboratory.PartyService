namespace Gvz.Laboratory.PartyService.Entities
{
    public class SupplierEntity
    {
        public Guid Id { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public List<PartyEntity> Parties { get; set; } = new List<PartyEntity>();
    }
}
