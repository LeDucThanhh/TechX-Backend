using Microsoft.AspNetCore.Mvc;
using TechX.API.Models.DTOs;
using TechX.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoreDTO>>> GetAllStores()
        {
            try
            {
                var stores = await _storeService.GetAllStoresAsync();
                return Ok(stores);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving stores" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StoreDTO>> GetStoreById(int id)
        {
            try
            {
                var store = await _storeService.GetStoreByIdAsync(id);
                if (store == null)
                {
                    return NotFound(new { message = "Store not found" });
                }
                return Ok(store);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving store" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<StoreDTO>> CreateStore([FromBody] CreateStoreDTO createDto)
        {
            try
            {
                var store = await _storeService.CreateStoreAsync(createDto);
                return CreatedAtAction(nameof(GetStoreById), new { id = store.Id }, store);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while creating store" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<StoreDTO>> UpdateStore(int id, [FromBody] UpdateStoreDTO updateDto)
        {
            try
            {
                var store = await _storeService.UpdateStoreAsync(id, updateDto);
                if (store == null)
                {
                    return NotFound(new { message = "Store not found" });
                }
                return Ok(store);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating store" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStore(int id)
        {
            try
            {
                var result = await _storeService.DeleteStoreAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "Store not found" });
                }
                return Ok(new { message = "Store deleted successfully" });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while deleting store" });
            }
        }

        [HttpGet("nearby")]
        public async Task<ActionResult<List<StoreDTO>>> GetNearbyStores([FromQuery] decimal latitude, [FromQuery] decimal longitude, [FromQuery] decimal radiusKm = 10)
        {
            try
            {
                var stores = await _storeService.GetNearbyStoresAsync(latitude, longitude, radiusKm);
                return Ok(stores);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while retrieving nearby stores" });
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<StoreDTO>>> SearchStores([FromQuery] string searchTerm)
        {
            try
            {
                var stores = await _storeService.SearchStoresAsync(searchTerm);
                return Ok(stores);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while searching stores" });
            }
        }
    }
} 