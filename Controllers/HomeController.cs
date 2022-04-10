using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopWebApi.Data;
using ShopWebApi.Models;

namespace ShopWebApi.Controllers
{
    [Route("v1")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var employee = new User { Id = 1, Username = "Robin", Password = "GarotoProdigio", Role = "Employee" };
            var manager = new User { Id = 2, Username = "Batman", Password = "CavaleiroDasTrevas", Role = "Manager" };
            var category = new Category { Id = 1, Title = "Informática" };
            var product = new Product { Id = 1, CategoryId = 1, Category = category, Title = "Mouse", Description = "Gamer", Price = 89.99M };

            context.Users.Add(employee);
            context.Users.Add(manager);
            context.Categories.Add(category);
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return Ok(new { message = "Datas created success!" });
        }
    }
}