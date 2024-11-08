﻿
using FluentValidation.Results;
using Gvz.Laboratory.PartyService.Validations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Gvz.Laboratory.PartyService.Models
{
    public class PartyModel
    {
        public Guid Id { get; }
        public int BatchNumber { get; }
        public DateTime DateOfReceipt { get; }
        public ProductModel Product { get; } = new ProductModel();
        public SupplierModel Supplier { get; } = new SupplierModel();
        public ManufacturerModel Manufacturer { get; } = new ManufacturerModel();
        public double BatchSize { get; }
        public double SampleSize { get; }
        public int TTN { get; }
        public string DocumentOnQualityAndSafety { get; } = string.Empty;
        public string TestReport { get; } = string.Empty;
        public DateTime DateOfManufacture { get; }
        public DateTime ExpirationDate { get; }
        public string Packaging { get; } = string.Empty;
        public string Marking { get; } = string.Empty;
        public string Result { get; } = string.Empty;
        public string Note { get; } = string.Empty;
        public UserModel User { get; } = new UserModel();

        public PartyModel(Guid id, int batchNumber, DateTime dateOfReceipt,
            double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, DateTime dateOfManufacture, DateTime expirationDate, string packaging, string marking,
            string result, string note)
        {
            Id = id;
            BatchNumber = batchNumber;
            DateOfReceipt = dateOfReceipt;

            BatchSize = batchSize;
            SampleSize = sampleSize;
            TTN = ttn;
            DocumentOnQualityAndSafety = documentOnQualityAndSafety;
            TestReport = testReport;
            DateOfManufacture = dateOfManufacture;
            ExpirationDate = expirationDate;
            Packaging = packaging;
            Marking = marking;
            Result = result;
            Note = note;
        }

        public PartyModel(Guid id, int batchNumber, DateTime dateOfReceipt, ProductModel product, SupplierModel supplier,
            ManufacturerModel manufacturer, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, DateTime dateOfManufacture, DateTime expirationDate, string packaging, string marking,
            string result, string note, UserModel user)
        {
            Id = id;
            BatchNumber = batchNumber;
            DateOfReceipt = dateOfReceipt;
            Product = product;
            Supplier = supplier;
            Manufacturer = manufacturer;
            BatchSize = batchSize;
            SampleSize = sampleSize;
            TTN = ttn;
            DocumentOnQualityAndSafety = documentOnQualityAndSafety;
            TestReport = testReport;
            DateOfManufacture = dateOfManufacture;
            ExpirationDate = expirationDate;
            Packaging = packaging;
            Marking = marking;
            Result = result;
            Note = note;
            User = user;
        }

        public static (Dictionary<string, string> errors, PartyModel party) Create(Guid id, int batchNumber, DateTime dateOfReceipt,
            double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, DateTime dateOfManufacture, DateTime expirationDate, string packaging, string marking,
            string result, string note, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            PartyModel party = new PartyModel(id, batchNumber, dateOfReceipt, batchSize, sampleSize,
                ttn, documentOnQualityAndSafety, testReport, dateOfManufacture, expirationDate, packaging, marking, result, note);

            if (!useValidation) { return (errors, party); }

            PartyValidation partyValidation = new PartyValidation();
            ValidationResult validationResult = partyValidation.Validate(party);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, party);
        }

        public static (Dictionary<string, string> errors, PartyModel party) Create(Guid id, int batchNumber, DateTime dateOfReceipt, ProductModel product, SupplierModel supplier,
            ManufacturerModel manufacturer, double batchSize, double sampleSize, int ttn, string documentOnQualityAndSafety,
            string testReport, DateTime dateOfManufacture, DateTime expirationDate, string packaging, string marking, 
            string result, string note, UserModel user, bool useValidation = true)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            PartyModel party = new PartyModel(id, batchNumber, dateOfReceipt, product, supplier, manufacturer, batchSize, sampleSize,
                ttn, documentOnQualityAndSafety, testReport, dateOfManufacture, expirationDate, packaging, marking, result, note, user);

            if (!useValidation) { return (errors, party); }

            PartyValidation partyValidation = new PartyValidation();
            ValidationResult validationResult = partyValidation.Validate(party);
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    errors[failure.PropertyName] = failure.ErrorMessage;
                }
            }

            return (errors, party);
        }
    }
}