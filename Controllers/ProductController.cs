using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWebApi.Data;
using ShopWebApi.Models;

namespace ShopWebApi.Controllers
{
    [Route("v1/product")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            // Propriedade include é o mesmo que um Join no banco de dados
            var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync<Product>();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound(new { message = "Product not found." });

            return product;
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetByCategory(int id, [FromServices] DataContext context)
        {
            var products = await context.Products.Include(x => x.Category).AsNoTracking().Where(x => x.CategoryId == id).ToListAsync();

            if (products == null)
                return NotFound(new { message = "Product not found." });

            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Product>>> Post([FromBody] Product model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return Ok(new { messagem = "Product created with success." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Unable to create product." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Product>>> Put(int id, [FromBody] Product model, [FromServices] DataContext context)
        {
            if (model.Id == id)
                return NotFound(new { message = "Product Not Found." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "Product updated with success." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "This record has already been updated." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Unable to update product." });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<Product>>> Delete(int id, [FromServices] DataContext context)
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound(new { message = "Product not found." });

            try
            {
                context.Remove<Product>(product);
                await context.SaveChangesAsync();
                return Ok(new { message = "Product removed with success." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Unable to remove category." });
            }
        }

    }
}