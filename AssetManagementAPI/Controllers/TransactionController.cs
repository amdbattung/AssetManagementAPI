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
    [Route("api/transactions")]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IValidator<CreateTransactionDTO> _createTransactionValidator;
        private readonly IValidator<UpdateTransactionDTO> _updateTransactionValidator;
        private readonly IValidator<QueryObject> _queryObjectValidator;

        public TransactionController(ITransactionRepository transactionRepository,
            IValidator<CreateTransactionDTO> createTransactionValidator,
            IValidator<UpdateTransactionDTO> updateTransactionValidator,
            IValidator<QueryObject> queryObjectValidator)
        {
            this._transactionRepository = transactionRepository;
            this._createTransactionValidator = createTransactionValidator;
            this._updateTransactionValidator = updateTransactionValidator;
            this._queryObjectValidator = queryObjectValidator;
        }

        [HttpGet(Name = "IndexTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetManyTransactionsDTO>> IndexAsync([FromQuery] QueryObject? queryObject)
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

            var transactions = await _transactionRepository.GetAllAsync(queryObject);

            return Ok(new GetManyTransactionsDTO(
                pageNumber: transactions.PageNumber,
                pageSize: transactions.PageSize,
                itemCount: transactions.ItemCount,
                transactions: transactions.Data.Select(t => t.ToDto())
            ));
        }

        [HttpPost(Name = "CreateTransaction")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetTransactionDTO>> CreateAsync([FromBody] CreateTransactionDTO transaction)
        {
            ValidationResult validationResult = await _createTransactionValidator.ValidateAsync(transaction);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            transaction.Reason = transaction.Reason?.Trim();
            transaction.Remark = transaction.Remark?.Trim();

            Transaction? response = await _transactionRepository.CreateAsync(transaction);
            return response == null ? BadRequest(ModelState) : CreatedAtAction(nameof(ShowAsync), new { id = response.Id }, response.ToDto());
        }

        [HttpGet("{id}", Name = "ShowTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetDepartmentDTO>> ShowAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            Transaction? transaction = await _transactionRepository.GetByIdAsync(id);

            return transaction == null ? NotFound() : Ok(transaction.ToDto());
        }

        [HttpPut("{id}", Name = "UpdateTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTransactionDTO>> UpdateAsync([FromRoute] string id, [FromBody] UpdateTransactionDTO transaction)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            ValidationResult validationResult = await _updateTransactionValidator.ValidateAsync(transaction);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            transaction.Reason = transaction.Reason?.Trim();
            transaction.Remark = transaction.Remark?.Trim();

            Transaction? response = await _transactionRepository.UpdateAsync(id, transaction);
            return response == null ? NotFound() : Ok(response.ToDto());
        }

        [HttpDelete("{id}", Name = "DestroyTransaction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTransactionDTO>> DestroyAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            Transaction? response = await _transactionRepository.DeleteAsync(id);
            return response == null ? NotFound() : Ok(response.ToDto());
        }
    }
}
