using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet("{sellerId}/stores")]
        public async Task<IActionResult> GetSellerStores(int sellerId)
        {
            var stores = await _storeService.GetAll();
            var sellerStores = stores.Where(r => r.SellerId == sellerId).ToList();

            return Ok(sellerStores);
        }

        // GET: api/stores
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stores = await _storeService.GetAll();
            return Ok(stores);
        }

        // GET: api/stores/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var store = await _storeService.Get(id);
            return Ok(store);
        }

        // POST: api/stores
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StoreDTO store)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _storeService.Create(store);
            return Ok("Store created");
        }

        // PUT: api/stores/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StoreDTO store)
        {
            if (id != store.Id)
                return BadRequest("Store ID mismatch");

            await _storeService.Update(store);
            return Ok("Store updated");
        }

        // DELETE: api/stores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _storeService.Delete(id);
            return Ok("Store deleted");
        }
    }

}
