using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Exceptions;
using Gvz.Laboratory.PartyService.Models;

namespace Gvz.Laboratory.PartyService.Services
{
    public class PartyService : IPartyService
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IPartyKafkaProducer _partyKafkaProducer;

        public PartyService(IPartyRepository partyRepository, IPartyKafkaProducer partyKafkaProducer)
        {
            _partyRepository = partyRepository;
            _partyKafkaProducer = partyKafkaProducer;
        }

        public async Task<Guid> CreatePartyAsync(Guid id, int batchNumber, DateTime dateOfReceipt, Guid productId, Guid supplierId,
            Guid manufacturerId, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, DateTime dateOfManufacture, DateTime expirationDate, string packaging, string marking,
            string result, Guid userId, string note)
        {
            var (errors, party) = PartyModel.Create(id, batchNumber, dateOfReceipt, batchSize, sampleSize,
                ttn, documentOnQualityAndSafety, testReport, dateOfManufacture, expirationDate, packaging, marking, result, note);

            if (errors.Count > 0)
            {
                throw new PartyValidationException(errors);
            }

            var partyModel = await _partyRepository.CreatePartyAsync(party, productId, supplierId, manufacturerId, userId);

            PartyDto partyDto = new PartyDto
            {
                Id = partyModel.Id,
                BatchNumber = partyModel.BatchNumber,
                DateOfReceipt = partyModel.DateOfReceipt,
                ProductId = partyModel.Product.Id,
                ProductName = partyModel.Product.ProductName,
                SupplierId = partyModel.Supplier.Id,
                SupplierName = partyModel.Supplier.SupplierName,
                ManufacturerId = partyModel.Manufacturer.Id,
                ManufacturerName = partyModel.Manufacturer.ManufacturerName,
                BatchSize = partyModel.BatchSize,
                SampleSize = partyModel.SampleSize,
                TTN = partyModel.TTN,
                DocumentOnQualityAndSafety = partyModel.DocumentOnQualityAndSafety,
                TestReport = partyModel.TestReport,
                DateOfManufacture = partyModel.DateOfManufacture,
                ExpirationDate = partyModel.ExpirationDate,
                Packaging = partyModel.Packaging,
                Marking = partyModel.Marking,
                Result = partyModel.Result,
                Note = partyModel.Note,
                UserId = partyModel.User.Id,
                UserName = partyModel.User.UserName,
            };

            await _partyKafkaProducer.SendToKafkaAsync(partyDto, "add-party-topic");

            return productId;
        }

        public async Task<(List<PartyModel> parties, int numberParties)> GetPartiesForPageAsync(int pageNumber)
        {
            return await _partyRepository.GetPartiesForPageAsync(pageNumber);
        }

        public async Task<Guid> UpdatePartyAsync(Guid id, int batchNumber, DateTime dateOfReceipt, Guid productId, Guid supplierId,
            Guid manufacturerId, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, DateTime dateOfManufacture, DateTime expirationDate, string packaging, string marking,
            string result, string note)
        {
            var (errors, party) = PartyModel.Create(id, batchNumber, dateOfReceipt, batchSize, sampleSize,
                ttn, documentOnQualityAndSafety, testReport, dateOfManufacture, expirationDate, packaging, marking, result, note);

            if (errors.Count > 0)
            {
                throw new PartyValidationException(errors);
            }

            var partyModel = await _partyRepository.UpdatePartyAsync(party, productId, supplierId, manufacturerId);

            PartyDto partyDto = new PartyDto
            {
                Id = partyModel.Id,
                BatchNumber = partyModel.BatchNumber,
                DateOfReceipt = partyModel.DateOfReceipt,
                ProductId = partyModel.Product.Id,
                ProductName = partyModel.Product.ProductName,
                SupplierId = partyModel.Supplier.Id,
                SupplierName = partyModel.Supplier.SupplierName,
                ManufacturerId = partyModel.Manufacturer.Id,
                ManufacturerName = partyModel.Manufacturer.ManufacturerName,
                BatchSize = partyModel.BatchSize,
                SampleSize = partyModel.SampleSize,
                TTN = partyModel.TTN,
                DocumentOnQualityAndSafety = partyModel.DocumentOnQualityAndSafety,
                TestReport = partyModel.TestReport,
                DateOfManufacture = partyModel.DateOfManufacture,
                ExpirationDate = partyModel.ExpirationDate,
                Packaging = partyModel.Packaging,
                Marking = partyModel.Marking,
                Result = partyModel.Result,
                Note = partyModel.Note,
                UserId = partyModel.User.Id,
                UserName = partyModel.User.UserName,
            };

            await _partyKafkaProducer.SendToKafkaAsync(partyDto, "update-party-topic");

            return productId;
        }

        public async Task DeletePartyAsync(List<Guid> ids)
        {
            await _partyRepository.DeletePartiesAsync(ids);
            await _partyKafkaProducer.SendToKafkaAsync(ids, "delete-party-topic");
        }
    }
}
