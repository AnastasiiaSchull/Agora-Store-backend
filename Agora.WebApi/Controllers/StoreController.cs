using Agora.BLL.DTO;
using Agora.BLL.Interfaces;
using Agora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Agora.Controllers
{
    [Route("api/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IProductService _productService;
        private readonly IStatisticsInitializer _statisticsInitializer;
        private readonly IUtilsService _utilsService;
        private readonly ILiqpayService _liqpayService;


        public StoreController(IStoreService storeService, IStatisticsInitializer statisticsInitializer, 
            IProductService productService, IUtilsService utilsService, ILiqpayService liqpayService)
        {
            _storeService = storeService;
            _productService = productService;
            _statisticsInitializer = statisticsInitializer;
            _utilsService = utilsService;
            _liqpayService = liqpayService;
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

        [HttpGet("{id}/with-products")]
        public async Task<IActionResult> GetStoreWithProducts(int id)
        {
            var store = await _storeService.Get(id);
            if (store == null)
                return NotFound($"Store with ID {id} not found");

            Console.WriteLine("_productService == null: " + (_productService == null));

            var products = await _productService.GetProductsByStore(id) ?? new List<ProductDTO>();
            
            foreach (var product in products)
            {
                Console.WriteLine($"Product: {product.Name}, imagePath: {product.ImagePath}, imagesPath: {product.ImagesPath}, imageUrls: {product.ImagesUrls}");
            }
            foreach (var p in products)
            {
                p.ImagePath = _utilsService.GetFirstImageUrl(p.ImagesPath, Request);
                p.ImagesUrls = _utilsService.GetImagesUrl(p.ImagesPath, Request);
            }
            return Ok(new
            {
                store,
                products
            });
        }


        // POST: api/stores
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StoreDTO store)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            store.CreatedAt = DateOnly.FromDateTime(DateTime.Now);

            var newStoreId = await _storeService.Create(store);

            await _statisticsInitializer.InitializeEmptyStatsForStore(newStoreId);

            return Ok(new { storeId = newStoreId });
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

        [HttpPost("get-liqpay-model")]
        public async Task<IActionResult> WithdrawFunds( WithdrawFundsModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("No data");

                var formModel = _liqpayService.GetLiqPayModelForWithdrawFunds(model.Amount, model.CardNumber, model.StoreId);
                var store = await _storeService.Get(model.StoreId);
                if (store == null)
                    return BadRequest($"Store with ID {model.StoreId} not found");
                if (store.FundsBalance < model.Amount)
                    return BadRequest("Your balance less than your amount");

                //store.FundsBalance -= model.Amount;
                //await _storeService.Update(store);
                return Ok(formModel);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error processing withdrawal: {ex.Message}");
            }
        }

    }

}
