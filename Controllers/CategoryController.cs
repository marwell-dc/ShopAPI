using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWebApi.Data;
using ShopWebApi.Models;

namespace ShopWebApi.Controllers
{
    // Endpioint -> URL
    //   -> http://localhost:5000
    //   -> https://localhost:5001
    //  Para acessar essa controller segue abaixo o endereço:
    //   -> https://localhost:5001/categories
    [Route("v1/categories")]
    // Quando toda a aplicação não esta cacheada e somente aquele metodo você que estaja em cache
    // [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    // Quando toda a aplicação esta cacheada e somente aquele metodo você não que estaja em cache (Geralmente usa quando se precisa da informação em tempo real)
    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class CategoryController : ControllerBase
    {
        //   -> https://localhost:5001/categories/
        // Por estar vazio o Route então ele herda a URL de cima
        // Na mesma url(padrão do REST) é criado o CRUD mas ele separa o que cada um faz pelo verbo HTTP (HttpGet, HttpPost, HttpPut, ...)
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {

            var categories = await context.Categories.AsNoTracking().ToListAsync<Category>();
            return Ok(categories);
        }

        // id:int 
        //  -> id = parametro
        //  -> :int = é uma restrição de rota(faz com que só aceite inteiro, qualquer outro tipo de dado retorna 404)
        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new { message = "Category not found." });

            return Ok(category);
        }

        // FromBody = Define de onde vem o modelo que no caso é do Corpo HTTP
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Post([FromBody] Category model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return Ok(new { message = "Category created success." });
            }
            catch
            {
                return BadRequest(new { message = "Unable to create category." });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
        {
            // Verifica se o Id é o mesmo do modelo
            if (model.Id != id)
                return NotFound(new { message = "Category not found." });

            // Verifica se os dados passados são validos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "Category updated success." });
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "This record has already been updated." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Unable to update category." });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Delete(int id, [FromServices] DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new { message = "Category not found." });

            try
            {
                context.Remove<Category>(category);
                await context.SaveChangesAsync();
                return Ok(new { message = "Category removed success." });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Unable to remove category." });
            }
        }
    }
}