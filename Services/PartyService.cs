using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Dto;
using Gvz.Laboratory.PartyService.Exceptions;
using Gvz.Laboratory.PartyService.Models;
using OfficeOpenXml;

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

        public async Task<Guid> CreatePartyAsync(Guid id, int batchNumber, string dateOfReceipt, Guid productId, Guid supplierId,
            Guid manufacturerId, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking,
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
                Surname = partyModel.User.Surname,
            };

            await _partyKafkaProducer.SendToKafkaAsync(partyDto, "add-party-topic");

            return productId;
        }

        public async Task<(List<PartyModel> parties, int numberParties)> GetPartiesForPageAsync(int pageNumber)
        {
            return await _partyRepository.GetPartiesForPageAsync(pageNumber);
        }

        public async Task<MemoryStream> ExportPartiesToExcelAsync()
        {
            var manufacturers = await _partyRepository.GetPartiesAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Manufacturers");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Номер партии";
                worksheet.Cells[1, 3].Value = "Дата поступления";
                worksheet.Cells[1, 4].Value = "Название продукта";
                worksheet.Cells[1, 5].Value = "Поставщик";
                worksheet.Cells[1, 6].Value = "Производитель";
                worksheet.Cells[1, 7].Value = "Объем партии";
                worksheet.Cells[1, 8].Value = "Объем выборки";
                worksheet.Cells[1, 9].Value = "ТТН";
                worksheet.Cells[1, 10].Value = "Документ по качеству и безопасности";
                worksheet.Cells[1, 11].Value = "Протокол испытаний";
                worksheet.Cells[1, 12].Value = "Дата изготовления";
                worksheet.Cells[1, 13].Value = "Срок годности";
                worksheet.Cells[1, 14].Value = "Упаковка";
                worksheet.Cells[1, 15].Value = "Маркировка";
                worksheet.Cells[1, 16].Value = "Заключение";
                worksheet.Cells[1, 17].Value = "Примечание";
                worksheet.Cells[1, 18].Value = "Ответственный";

                for (int i = 0; i < manufacturers.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = manufacturers[i].Id;
                    worksheet.Cells[i + 2, 2].Value = manufacturers[i].BatchNumber;
                    worksheet.Cells[i + 2, 3].Value = manufacturers[i].DateOfReceipt;
                    worksheet.Cells[i + 2, 4].Value = manufacturers[i].Product.ProductName;
                    worksheet.Cells[i + 2, 5].Value = manufacturers[i].Supplier.SupplierName;
                    worksheet.Cells[i + 2, 6].Value = manufacturers[i].Manufacturer.ManufacturerName;
                    worksheet.Cells[i + 2, 7].Value = manufacturers[i].BatchSize;
                    worksheet.Cells[i + 2, 8].Value = manufacturers[i].SampleSize;
                    worksheet.Cells[i + 2, 9].Value = manufacturers[i].TTN;
                    worksheet.Cells[i + 2, 10].Value = manufacturers[i].DocumentOnQualityAndSafety;
                    worksheet.Cells[i + 2, 11].Value = manufacturers[i].TestReport;
                    worksheet.Cells[i + 2, 12].Value = manufacturers[i].DateOfManufacture;
                    worksheet.Cells[i + 2, 13].Value = manufacturers[i].ExpirationDate;
                    worksheet.Cells[i + 2, 14].Value = manufacturers[i].Packaging;
                    worksheet.Cells[i + 2, 15].Value = manufacturers[i].Marking;
                    worksheet.Cells[i + 2, 16].Value = manufacturers[i].Result;
                    worksheet.Cells[i + 2, 17].Value = manufacturers[i].Note;
                    worksheet.Cells[i + 2, 18].Value = manufacturers[i].User.Surname;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);

                stream.Position = 0; // Сбрасываем поток
                return stream;
            }
        }

        public async Task<(List<PartyModel> parties, int numberParties)> SearchPartiesAsync(string searchQuery, int pageNumber)
        {
            return await _partyRepository.SearchPartiesAsync(searchQuery, pageNumber);
        }

        public async Task<Guid> UpdatePartyAsync(Guid id, int batchNumber, string dateOfReceipt, Guid productId, Guid supplierId,
            Guid manufacturerId, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, string dateOfManufacture, string expirationDate, string packaging, string marking,
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
                Surname = partyModel.User.UserName,
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
