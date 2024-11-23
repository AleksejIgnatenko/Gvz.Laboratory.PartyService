using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Entities;
using Gvz.Laboratory.PartyService.Exceptions;
using Gvz.Laboratory.PartyService.Models;
using Microsoft.EntityFrameworkCore;

namespace Gvz.Laboratory.PartyService.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly GvzLaboratoryPartyServiceDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IUserRepository _userRepository;

        public PartyRepository(GvzLaboratoryPartyServiceDbContext context, IProductRepository productRepository, ISupplierRepository supplierRepository, IManufacturerRepository manufacturerRepository, IUserRepository userRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _supplierRepository = supplierRepository;
            _manufacturerRepository = manufacturerRepository;
            _userRepository = userRepository;
        }

        public async Task<PartyModel> CreatePartyAsync(PartyModel party, Guid productId, Guid supplierId, Guid manufacturerId, Guid userId)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.BatchNumber == party.BatchNumber);

            if (existingParty != null) { throw new RepositoryException("Партия с таким номером уже существует"); }

            var existingProduct = await _productRepository.GetProductByIdAsync(productId)
                ?? throw new RepositoryException("Продукт не найден");

            var existingSupplier = await _supplierRepository.GetSupplierByIdAsync(supplierId)
                ?? throw new RepositoryException("Поставщик не найден");

            var existingManufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerId)
                ?? throw new RepositoryException("Производитель не найден");

            var existingUser = await _userRepository.GetUserByIdAsync(userId)
                ?? throw new RepositoryException("Пользователь не найден");

            var partyEntity = new PartyEntity
            {
                Id = party.Id, 
                BatchNumber = party.BatchNumber,
                DateOfReceipt = party.DateOfReceipt,
                Product = existingProduct,
                Supplier = existingSupplier,
                Manufacturer = existingManufacturer,
                BatchSize = party.BatchSize,
                SampleSize = party.SampleSize,
                TTN = party.TTN,
                DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety,
                TestReport = party.TestReport,
                DateOfManufacture = party.DateOfManufacture,
                ExpirationDate = party.ExpirationDate,
                Packaging = party.Packaging,
                Marking = party.Marking,
                Result = party.Result,
                Note = party.Note,
                User = existingUser,
                DateCreate = DateTime.UtcNow,
            };

            await _context.Parties.AddAsync(partyEntity);
            await _context.SaveChangesAsync();

            var partyModel = PartyModel.Create(
                partyEntity.Id,
                partyEntity.BatchNumber,
                partyEntity.DateOfReceipt,
                ProductModel.Create(partyEntity.Product.Id, partyEntity.Product.ProductName),
                SupplierModel.Create(partyEntity.Supplier.Id, partyEntity.Supplier.SupplierName),
                ManufacturerModel.Create(partyEntity.Manufacturer.Id, partyEntity.Manufacturer.ManufacturerName),
                partyEntity.BatchSize,
                partyEntity.SampleSize,
                partyEntity.TTN,
                partyEntity.DocumentOnQualityAndSafety,
                partyEntity.TestReport,
                partyEntity.DateOfManufacture,
                partyEntity.ExpirationDate,
                partyEntity.Packaging,
                partyEntity.Marking,
                partyEntity.Result,
                UserModel.Create(partyEntity.User.Id, partyEntity.User.Surname, partyEntity.User.UserName, partyEntity.User.Patronymic),
                partyEntity.Note,
                false).party;

            return partyModel;
        }

        public async Task<(List<PartyModel> parties, int numberParties)> GetPartiesForPageAsync(int pageNumber)
        {
            var partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Include(p => p.Product)
                    .Include(p => p.Supplier)
                    .Include(p => p.Manufacturer)
                    .Include(p =>p.User)
                    .OrderByDescending(p => p.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();

            if (!partyEntities.Any() && pageNumber != 0)
            {
                pageNumber--;
                partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Include(p => p.Product)
                    .Include(p => p.Supplier)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.User)
                    .OrderByDescending(p => p.DateCreate)
                    .Skip(pageNumber * 20)
                    .Take(20)
                    .ToListAsync();
            }

            var numberParties = await _context.Parties.CountAsync();

            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                ProductModel.Create(p.Product.Id, p.Product.ProductName),
                SupplierModel.Create(p.Supplier.Id, p.Supplier.SupplierName),
                ManufacturerModel.Create(p.Manufacturer.Id, p.Manufacturer.ManufacturerName),
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                UserModel.Create(p.User.Id, p.User.Surname, p.User.UserName, p.User.Patronymic),
                p.Note,
                false).party).ToList();

            return (parties, numberParties);
        }

        public async Task<List<PartyModel>> GetPartiesAsync()
        {
            var partyEntities = await _context.Parties
                    .AsNoTracking()
                    .Include(p => p.Product)
                    .Include(p => p.Supplier)
                    .Include(p => p.Manufacturer)
                    .Include(p => p.User)
                    .OrderByDescending(p => p.DateCreate)
                    .Take(20)
                    .ToListAsync();


            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                ProductModel.Create(p.Product.Id, p.Product.ProductName),
                SupplierModel.Create(p.Supplier.Id, p.Supplier.SupplierName),
                ManufacturerModel.Create(p.Manufacturer.Id, p.Manufacturer.ManufacturerName),
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                UserModel.Create(p.User.Id, p.User.Surname, p.User.UserName, p.User.Patronymic),
                p.Note,
                false).party).ToList();

            return parties;
        }

        public async Task<(List<PartyModel> parties, int numberParties)> SearchPartiesAsync(string searchQuery, int pageNumber)
        {
            var partyEntities = await _context.Parties
                .AsNoTracking()
                .Where(p =>
                    p.BatchNumber.ToString().Contains(searchQuery) ||
                    p.DateOfReceipt.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Product.ProductName.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Supplier.SupplierName.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Manufacturer.ManufacturerName.ToLower().Contains(searchQuery.ToLower()) ||
                    p.BatchSize.ToString().Contains(searchQuery) ||
                    p.SampleSize.ToString().Contains(searchQuery) ||
                    p.TTN.ToString().Contains(searchQuery) ||
                    p.DocumentOnQualityAndSafety.ToLower().Contains(searchQuery.ToLower()) ||
                    p.TestReport.ToLower().Contains(searchQuery.ToLower()) ||
                    p.DateOfManufacture.ToLower().Contains(searchQuery.ToLower()) ||
                    p.ExpirationDate.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Packaging.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Marking.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Result.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Note.ToLower().Contains(searchQuery.ToLower()) ||
                    p.User.UserName.ToLower().Contains(searchQuery.ToLower())
                )
                .OrderByDescending(p => p.DateCreate)
                .Skip(pageNumber * 20)
                .Take(20)
                .ToListAsync();

            var numberParties = await _context.Parties
                    .AsNoTracking()
                    .CountAsync(p =>
                    p.BatchNumber.ToString().Contains(searchQuery) ||
                    p.DateOfReceipt.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Product.ProductName.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Supplier.SupplierName.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Manufacturer.ManufacturerName.ToLower().Contains(searchQuery.ToLower()) ||
                    p.BatchSize.ToString().Contains(searchQuery) ||
                    p.SampleSize.ToString().Contains(searchQuery) ||
                    p.TTN.ToString().Contains(searchQuery) ||
                    p.DocumentOnQualityAndSafety.ToLower().Contains(searchQuery.ToLower()) ||
                    p.TestReport.ToLower().Contains(searchQuery.ToLower()) ||
                    p.DateOfManufacture.ToLower().Contains(searchQuery.ToLower()) ||
                    p.ExpirationDate.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Packaging.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Marking.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Result.ToLower().Contains(searchQuery.ToLower()) ||
                    p.Note.ToLower().Contains(searchQuery.ToLower()) ||
                    p.User.UserName.ToLower().Contains(searchQuery.ToLower()));

            var parties = partyEntities.Select(p => PartyModel.Create(
                p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                ProductModel.Create(p.Product.Id, p.Product.ProductName),
                SupplierModel.Create(p.Supplier.Id, p.Supplier.SupplierName),
                ManufacturerModel.Create(p.Manufacturer.Id, p.Manufacturer.ManufacturerName),
                p.BatchSize,
                p.SampleSize,
                p.TTN,
                p.DocumentOnQualityAndSafety,
                p.TestReport,
                p.DateOfManufacture,
                p.ExpirationDate,
                p.Packaging,
                p.Marking,
                p.Result,
                UserModel.Create(p.User.Id, p.User.Surname, p.User.UserName, p.User.Patronymic),
                p.Note,
                false).party).ToList();

            return (parties, numberParties);
        }

        public async Task<PartyModel> UpdatePartyAsync(PartyModel party, Guid productId, Guid supplierId, Guid manufacturerId)
        {
            var existingParty = await _context.Parties.FirstOrDefaultAsync(p => p.Id == party.Id)
                ?? throw new RepositoryException("Партия не найдена");

            var existingProduct = await _productRepository.GetProductByIdAsync(productId)
                ?? throw new RepositoryException("Продукт не найден");

            var existingSupplier = await _supplierRepository.GetSupplierByIdAsync(supplierId)
                ?? throw new RepositoryException("Поставщик не найден");

            var existingManufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(manufacturerId)
                ?? throw new RepositoryException("Производитель не найден не найден");

            var existingSupplierName = await _context.Parties
            .FirstOrDefaultAsync(p => (p.BatchNumber == party.BatchNumber) && (p.BatchNumber != existingParty.BatchNumber));
            if (existingSupplierName != null)
            {
                throw new RepositoryException("Партия с таким номером партии уже существеут.");
            }

            existingParty.BatchNumber = party.BatchNumber;
            existingParty.DateOfReceipt = party.DateOfReceipt;
            existingParty.Product = existingProduct;
            existingParty.Supplier = existingSupplier;
            existingParty.Manufacturer = existingManufacturer;
            existingParty.BatchSize = party.BatchSize;
            existingParty.SampleSize = party.SampleSize;
            existingParty.TTN = party.TTN;
            existingParty.DocumentOnQualityAndSafety = party.DocumentOnQualityAndSafety;
            existingParty.TestReport = party.TestReport;
            existingParty.DateOfManufacture = party.DateOfManufacture;
            existingParty.ExpirationDate = party.ExpirationDate;
            existingParty.Packaging = party.Packaging;
            existingParty.Marking = party.Marking;
            existingParty.Result = party.Result;
            existingParty.Note = party.Note;

            await _context.SaveChangesAsync();

            var partyModel = PartyModel.Create(
                existingParty.Id,
                existingParty.BatchNumber,
                existingParty.DateOfReceipt,
                ProductModel.Create(existingParty.Product.Id, existingParty.Product.ProductName),
                SupplierModel.Create(existingParty.Supplier.Id, existingParty.Supplier.SupplierName),
                ManufacturerModel.Create(existingParty.Manufacturer.Id, existingParty.Manufacturer.ManufacturerName),
                existingParty.BatchSize,
                existingParty.SampleSize,
                existingParty.TTN,
                existingParty.DocumentOnQualityAndSafety,
                existingParty.TestReport,
                existingParty.DateOfManufacture,
                existingParty.ExpirationDate,
                existingParty.Packaging,
                existingParty.Marking,
                existingParty.Result,
                UserModel.Create(existingParty.User.Id, existingParty.User.Surname, existingParty.User.UserName, existingParty.User.Patronymic),
                existingParty.Note,
                false).party;

            return partyModel;
        }

        public async Task DeletePartiesAsync(List<Guid> ids)
        {
            await _context.Parties
                .Where(s => ids.Contains(s.Id))
                .ExecuteDeleteAsync();
        }
    }
}
