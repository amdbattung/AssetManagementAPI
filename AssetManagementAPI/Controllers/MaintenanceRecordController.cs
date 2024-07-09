using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Helpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementAPI.Controllers
{
    [ApiController]
    /*[Authorize]*/
    [Route("api/maintenance-records")]
    public class MaintenanceRecordController : Controller
    {
        private readonly IMaintenanceRecordRepository _maintenanceRecordRepository;
        private readonly IValidator<CreateMaintenanceRecordDTO> _createMaintenanceRecordValidator;
        private readonly IValidator<UpdateMaintenanceRecordDTO> _updateMaintenanceRecordValidator;
        private readonly IValidator<QueryObject> _queryObjectValidator;

        public MaintenanceRecordController(IMaintenanceRecordRepository maintenanceRecordRepository,
            IValidator<CreateMaintenanceRecordDTO> createMaintenanceRecordValidator,
            IValidator<UpdateMaintenanceRecordDTO> updateMaintenanceRecordValidator,
            IValidator<QueryObject> queryObjectValidator)
        {
            _maintenanceRecordRepository = maintenanceRecordRepository;
            _createMaintenanceRecordValidator = createMaintenanceRecordValidator;
            _updateMaintenanceRecordValidator = updateMaintenanceRecordValidator;
            _queryObjectValidator = queryObjectValidator;
        }

        [HttpGet(Name = "IndexMaintenanceRecords")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetManyMaintenanceRecordsDTO>> IndexAsync([FromQuery] QueryObject? queryObject)
        {
            if (queryObject != null)
            {
                ValidationResult validationResult = await _queryObjectValidator.ValidateAsync(queryObject);

                foreach (var error in validationResult.Errors)
                {
                    if (error.ErrorCode == "QERR0001")
                    {
                        queryObject.PageNumber = null;
                    }

                    if (error.ErrorCode == "QERR0002")
                    {
                        queryObject.PageSize = null;
                    }
                }
            }

            var records = await _maintenanceRecordRepository.GetAllAsync(queryObject);

            return Ok(new GetManyMaintenanceRecordsDTO(
                pageNumber: records.PageNumber,
                pageSize: records.PageSize,
                itemCount: records.ItemCount,
                maintenanceRecords: records.Data.Select(t => t.ToDto())
            ));
        }

        [HttpPost(Name = "CreateMaintenanceRecord")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetMaintenanceRecordDTO>> CreateAsync([FromBody] CreateMaintenanceRecordDTO record)
        {
            ValidationResult validationResult = await _createMaintenanceRecordValidator.ValidateAsync(record);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            record.Reason = record.Reason?.Trim();
            record.Comment = record.Comment?.Trim();

            MaintenanceRecord? response = await _maintenanceRecordRepository.CreateAsync(record);
            return response == null ? BadRequest(ModelState) : CreatedAtAction(nameof(ShowAsync), new { id = response.Id }, response.ToDto());
        }

        [HttpGet("{id}", Name = "ShowMaintenanceRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetMaintenanceRecordDTO>> ShowAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            MaintenanceRecord? record = await _maintenanceRecordRepository.GetByIdAsync(id);

            return record == null ? NotFound() : Ok(record.ToDto());
        }

        [HttpPut("{id}", Name = "UpdateMaintenanceRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTransactionDTO>> UpdateAsync([FromRoute] string id, [FromBody] UpdateMaintenanceRecordDTO record)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            ValidationResult validationResult = await _updateMaintenanceRecordValidator.ValidateAsync(record);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            record.Reason = record.Reason?.Trim();
            record.Comment = record.Comment?.Trim();

            MaintenanceRecord? response = await _maintenanceRecordRepository.UpdateAsync(id, record);
            return response == null ? NotFound() : Ok(response.ToDto());
        }

        [HttpDelete("{id}", Name = "DestroyMaintenanceRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetMaintenanceRecordDTO>> DestroyAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            MaintenanceRecord? response = await _maintenanceRecordRepository.DeleteAsync(id);
            return response == null ? NotFound() : Ok(response.ToDto());
        }
    }
}
