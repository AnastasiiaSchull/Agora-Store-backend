using Microsoft.AspNetCore.Mvc;
using Agora.BLL.DTO;
using Agora.BLL.Infrastructure;
using Agora.BLL.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;
    private readonly IUtilsService _utilService;

    public WishlistController(IWishlistService wishlistService, IUtilsService utilService)
    {
        _wishlistService = wishlistService;
        _utilService = utilService;
    }

    // GET: api/Wishlist
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WishlistDTO>>> GetAll()
    {
        var wishlists = await _wishlistService.GetAll();
        return Ok(wishlists);
    }

    // GET: api/Wishlist/5
    [HttpGet("{id}")]
    public async Task<ActionResult<WishlistDTO>> Get(int id)
    {
        try
        {
            var wishlist = await _wishlistService.Get(id);
            return Ok(wishlist);
        }
        catch (ValidationExceptionFromService ex)
        {
            return NotFound(ex.Message);
        }
    }

    // POST: api/Wishlist
    [HttpPost]    
    public async Task<ActionResult> Create([FromBody] WishlistDTO wishlistDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        wishlistDTO.DateAdded = DateOnly.FromDateTime(DateTime.Today); 
        wishlistDTO.Customer.Id = wishlistDTO.Customer.Id;
        await _wishlistService.Create(wishlistDTO);
        return Ok();
    }

    // PUT: api/Wishlist/5
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] WishlistDTO wishlistDTO)
    {
        if (id != wishlistDTO.Id)
            return BadRequest("ID mismatch.");

        await _wishlistService.Update(wishlistDTO);
        return NoContent();
    }

    // DELETE: api/Wishlist/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _wishlistService.Delete(id);
        return NoContent();
    }

    [HttpPost("{wishlistId}/products/{productId}")]
    public async Task<IActionResult> AddProductToWishlist(int wishlistId, int productId)
    {
        await _wishlistService.AddProductToWishlist(wishlistId, productId);
        return Ok();
    }

    [HttpDelete("{wishlistId}/products/{productId}")]
    public async Task<IActionResult> RemoveProductFromWishlist(int wishlistId, int productId)
    {
        await _wishlistService.RemoveProductFromWishlist(wishlistId, productId);
        return NoContent();
    }

    [HttpGet("{id}/with-products")]
    public async Task<ActionResult<WishlistDTO>> GetWithProducts(int id)
    {
        try
        {
            var wishlist = await _wishlistService.GetWithProducts(id);
            return Ok(wishlist);
        }
        catch (ValidationExceptionFromService ex)
        {
            return NotFound(ex.Message);
        }
    }
    // GET: api/Wishlist/by-customer/5
    /*[HttpGet("by-customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<WishlistDTO>>> GetByCustomerId(int customerId)
    {
        var wishlists = await _wishlistService.GetByCustomerId(customerId);
        return Ok(wishlists);
    }*/
    [HttpGet("by-customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<WishlistDTO>>> GetByCustomerId(int customerId)
    {
        var wishlists = await _wishlistService.GetByCustomerId(customerId);

        foreach (var wishlist in wishlists)
        {
            foreach (var product in wishlist.Products)
            {
                // Например, если путь к папке с фото — product.ImagesPath (например: "images/products/1")
                product.ImagePath = _utilService.GetFirstImageUrl(product.ImagesPath, Request);
            }
        }

        return Ok(wishlists);
    }


}
