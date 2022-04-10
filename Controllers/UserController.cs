using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopWebApi.Data;
using ShopWebApi.Models;
using ShopWebApi.Services;

namespace ShopWebApi.Controllers
{
    [Route("v1/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {
            var users = await context.Users.AsNoTracking().ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromBody] User model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(model);

            try
            {
                // Força o usuario sempre ser fucioinario
                model.Role = "employee";

                context.Users.Add(model);
                await context.SaveChangesAsync();

                // Caso retorne o no json o usuario pelo ok() esconde o password
                model.Password = "";

                return Ok(new { message = "User created sucess!" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Unable to create user." });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(int id, User model, [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return NotFound(ModelState);

            if (id != model.Id)
                return NotFound(new { message = "User not found." });

            try
            {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(new { message = "User update sucess" });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Unable to update category." });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model, [FromServices] DataContext context)
        {
            var user = await context.Users
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "User not found!" });

            var token = TokenService.GenerateToken(user);

            // Esconder o password do cliente
            user.Password = "";

            return new
            {
                // user = new
                // {
                //     id = user.Id,
                //     username = user.Username,
                //     role = user.Role

                // },
                user = user,
                token = token
            };
        }
    }
}
