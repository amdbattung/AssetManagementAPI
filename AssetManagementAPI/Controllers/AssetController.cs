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
    [Route("api/assets")]
    public class AssetController : Controller
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IValidator<CreateAssetDTO> _createAssetValidator;
        private readonly IValidator<UpdateAssetDTO> _updateAssetValidator;
        private readonly IValidator<QueryObject> _queryObjectValidator;

        public AssetController(IAssetRepository assetRepository,
            IValidator<CreateAssetDTO> createAssetValidator,
            IValidator<UpdateAssetDTO> updateAssetValidator,
            IValidator<QueryObject> queryObjectValidator)
        {
            this._assetRepository = assetRepository;
            this._createAssetValidator = createAssetValidator;
            this._updateAssetValidator = updateAssetValidator;
            this._queryObjectValidator = queryObjectValidator;
        }

        [HttpGet(Name = "IndexAssets")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GetAssetDTO>>> IndexAsync([FromQuery] QueryObject? queryObject)
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

            var assets = await _assetRepository.GetAllAsync(queryObject);

            return Ok(assets.Select(a => a.ToDto()));
        }

        [HttpPost(Name = "CreateAsset")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAssetDTO>> CreateAsync([FromBody] CreateAssetDTO asset)
        {
            ValidationResult validationResult = await _createAssetValidator.ValidateAsync(asset);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Asset? response = await _assetRepository.CreateAsync(asset);
            return response == null ? NotFound() : CreatedAtAction(nameof(ShowAsync), new { id = response.Id }, response.ToDto());
        }

        [HttpGet("{id}", Name = "ShowAsset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAssetDTO>> ShowAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            Asset? asset = await _assetRepository.GetByIdAsync(id);

            return asset == null ? NotFound() : Ok(asset.ToDto());
        }

        [HttpPut("{id}", Name = "UpdateAsset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAssetDTO>> UpdateAsync([FromRoute] string id, [FromBody] UpdateAssetDTO asset)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            ValidationResult validationResult = await _updateAssetValidator.ValidateAsync(asset);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(this.ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Asset? response = await _assetRepository.UpdateAsync(id, asset);
            return response == null ? NotFound() : Ok(response.ToDto());
        }

        [HttpDelete("{id}", Name = "DestroyAsset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAssetDTO>> DestroyAsync([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Null or invalid id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Asset? response = await _assetRepository.DeleteAsync(id);
            return response == null ? NotFound() : Ok(response.ToDto());
        }
    }
}
