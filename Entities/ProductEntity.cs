namespace Gvz.Laboratory.PartyService.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string UnitsOfMeasurement { get; set; } = string.Empty;
        public List<PartyEntity> Parties { get; set; } = new List<PartyEntity>();
    }
}
