using AssetManagementAPI.DTO;
using AssetManagementAPI.Interfaces;
using AssetManagementAPI.Models;
using AssetManagementAPI.Services.Helpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssetManagementAPI.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IValidator<CreateEmployeeDTO> _createEmployeeValidator;
        private readonly IValidator<UpdateEmployeeDTO> _updateEmployeeValidator;
        private readonly IValidator<QueryObject> _queryObjectValidator;

        public EmployeeController(IEmployeeRepository employeeRepository,
            IValidator<CreateEmployeeDTO> createEmployeeValidator,
            IValidator<UpdateEmployeeDTO> updateEmployeeValidator,
            IValidator<QueryObject> queryObjectValidator)
        {
            this._employeeRepository = employeeRepository;
            this._createEmployeeValidator = createEmployeeValidator;
            this._updateEmployeeValidator = updateEmployeeValidator;
            this._queryObjectValidator = queryObjectValidator;
        }

        [HttpGet(Name = "IndexEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GetEmployeeDTO>>> IndexAsync([FromQuery] QueryObject? queryObject)
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

            var employees = await _employeeRepository.GetAllAsync(queryObject);

            return Ok(employees.Select(e => e.ToDto()));
        }

        [HttpPost(Name = "CreateEmployee")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetEmployeeDTO>> CreateAsync([FromBody] CreateEmployeeDTO employee)
        {
            ValidationResult validationResult = await _createEmployeeValidator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Employee? response = await _employeeRepository.CreateAsync(employee);
            return response == null ? NotFound() : CreatedAtAction(nameof(ShowAsync), new { id = response.Id }, response.ToDto());
        }

        [HttpGet("{id}", Name = "ShowEmployee")]
        [Authorize(Policy = "UserIsEmployeePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetEmployeeDTO>> ShowAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            Employee? employee = await _employeeRepository.GetByIdAsync(id);

            return employee == null ? NotFound() : Ok(employee.ToDto());
        }

        [HttpPut("{id}", Name = "UpdateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetEmployeeDTO>> UpdateAsync([FromRoute] string id, [FromBody] UpdateEmployeeDTO employee)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            ValidationResult validationResult = await _updateEmployeeValidator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Employee? response = await _employeeRepository.UpdateAsync(id, employee);
            return response == null ? NotFound() : Ok(response.ToDto());
        }

        [HttpDelete("{id}", Name = "DestroyEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetEmployeeDTO>> DestroyAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Employee? response = await _employeeRepository.DeleteAsync(id);
            return response == null ? NotFound() : Ok(response.ToDto());
        }
    }
}
