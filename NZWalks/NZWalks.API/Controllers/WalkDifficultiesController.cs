using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();

            // Convert Domain to DTOs
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);
            return Ok(walkDifficultiesDTO);
        }
        
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if(walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Validate incoming data
            if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            // Convert DTO to Domain Model
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                code = addWalkDifficultyRequest.code
            };

            // Call repository
            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            // Convert Domain to DTO to be sent to user
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            // return response
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficulty([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Validate incoming data
            if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                code = updateWalkDifficultyRequest.code
            };

            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            if(walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            if(walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        #region Private Methods

        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if(addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"cannot be empty!");
                return false;
            }

            if (string.IsNullOrEmpty(addWalkDifficultyRequest.code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.code),$"{nameof(addWalkDifficultyRequest.code)} cannot be null or empty");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if(updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"This cannot be empty");
                return false;
            }

            if (string.IsNullOrEmpty(updateWalkDifficultyRequest.code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.code), $"{nameof(updateWalkDifficultyRequest.code)} cannot be empty or null");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
