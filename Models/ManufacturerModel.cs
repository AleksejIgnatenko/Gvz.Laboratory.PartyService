namespace Gvz.Laboratory.PartyService.Models
{
    public class ManufacturerModel
    {
        public Guid Id { get; }
        public string ManufacturerName { get; } = string.Empty;

        public ManufacturerModel(Guid id, string manufacturerName)
        {
            Id = id;
            ManufacturerName = manufacturerName;
        }

        public ManufacturerModel()
        {
        }

        public static ManufacturerModel Create(Guid id, string manufacturerName)
        {
            ManufacturerModel manufacturer = new ManufacturerModel(id, manufacturerName);
            return manufacturer;
        }
    }
}
