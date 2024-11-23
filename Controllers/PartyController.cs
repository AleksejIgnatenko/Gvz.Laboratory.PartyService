using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvz.Laboratory.PartyService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService _partyService;
        private readonly IJwtProvider _jwtProvider;

        public PartyController(IPartyService partyService, IJwtProvider jwtProvider)
        {
            _partyService = partyService;
            _jwtProvider = jwtProvider;
        }

        [HttpPost]
        public async Task<ActionResult> CreatePartyAsync([FromBody] CreatePartyRequest createPartyRequest)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            Console.WriteLine(createPartyRequest.DateOfReceipt);

            var userId = _jwtProvider.GetUserIdFromToken(token);

            var id = await _partyService.CreatePartyAsync(Guid.NewGuid(),
                createPartyRequest.BatchNumber,
                createPartyRequest.DateOfReceipt,
                createPartyRequest.ProductId,
                createPartyRequest.SupplierId,
                createPartyRequest.ManufacturerId,
                createPartyRequest.BatchSize,
                createPartyRequest.SampleSize,
                createPartyRequest.TTN,
                createPartyRequest.DocumentOnQualityAndSafety,
                createPartyRequest.TestReport,
                createPartyRequest.DateOfManufacture,
                createPartyRequest.ExpirationDate,
                createPartyRequest.Packaging,
                createPartyRequest.Marking,
                createPartyRequest.Result,
                userId,
                createPartyRequest.Note);

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetPartiesForPageAsync(int pageNumber)
        {
            var (parties, numberParties) = await _partyService.GetPartiesForPageAsync(pageNumber);

            var response = parties.Select(p => new GetPartiesResponse(p.Id,
                p.BatchNumber,
                p.DateOfReceipt,
                p.Product.ProductName,
                p.Supplier.SupplierName,
                p.Manufacturer.ManufacturerName,
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
                p.User.Surname,
                p.Note)).ToList();

            var responseWrapper = new GetPartiesForPageResponseWrapper(response, numberParties);

            return Ok(responseWrapper);
        }

        [HttpGet]
        [Route("exportPartiesToExcel")]
        [Authorize]
        public async Task<ActionResult> ExportPartiesToExcelAsync()
        {
            var stream = await _partyService.ExportPartiesToExcelAsync();
            var fileName = "Parties.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        [HttpGet]
        [Route("searchParties")]
        [Authorize]
        public async Task<ActionResult> SearchPartiesAsync(string searchQuery, int pageNumber)
        {
            var (parties, numberParties) = await _partyService.SearchPartiesAsync(searchQuery, pageNumber);
            var response = parties.Select(p => new GetPartiesResponse(p.Id,
                 p.BatchNumber,
                 p.DateOfReceipt,
                 p.Product.ProductName,
                 p.Supplier.SupplierName,
                 p.Manufacturer.ManufacturerName,
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
                 p.User.Surname,
                 p.Note)).ToList();

            var responseWrapper = new GetPartiesForPageResponseWrapper(response, numberParties);
            return Ok(responseWrapper);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdatePartyAsync(Guid id, [FromBody] UpdatePartyRequest updateProductRequest)
        {
            //var userId = Guid.NewGuid();//get the user Id from jwtToken or updateProductRequest

            var partyId = await _partyService.UpdatePartyAsync(updateProductRequest.Id,
                updateProductRequest.BatchNumber,
                updateProductRequest.DateOfReceipt,
                updateProductRequest.ProductId,
                updateProductRequest.SupplierId,
                updateProductRequest.ManufacturerId,
                updateProductRequest.BatchSize,
                updateProductRequest.SampleSize,
                updateProductRequest.TTN,
                updateProductRequest.DocumentOnQualityAndSafety,
                updateProductRequest.TestReport,
                updateProductRequest.DateOfManufacture,
                updateProductRequest.ExpirationDate,
                updateProductRequest.Packaging,
                updateProductRequest.Marking,
                updateProductRequest.Result,
                updateProductRequest.Note);

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProductAsync([FromBody] List<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                return BadRequest("No supplier IDs provided.");
            }

            await _partyService.DeletePartyAsync(ids);

            return Ok();
        }
    }
}
