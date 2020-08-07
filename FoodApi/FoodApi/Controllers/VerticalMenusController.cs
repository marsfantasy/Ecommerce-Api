using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FoodApi.Data;
using FoodApi.Models;
using ImageUploader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VerticalMenusController : ControllerBase
    {
        private FoodDbContext _dbContext;
        public VerticalMenusController(FoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/VerticalMenus
        [HttpGet]
        public IActionResult Get()
        {
            var verticalMenus = from c in _dbContext.VerticalMenus
                                select new
                                {
                                    Id = c.Id,
                                    Title = c.Title,
                                    ImageUrl = c.ImageUrl
                                };


            return Ok(verticalMenus);
        }

        // GET: api/VerticalMenus/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var verticalMenu = (from c in _dbContext.VerticalMenus
                                where c.Id == id
                                select new
                                {
                                    Id = c.Id,
                                    Title = c.Title,
                                    ImageUrl = c.ImageUrl
                                }).FirstOrDefault();


            return Ok(verticalMenu);

        }

        // POST: api/VerticalMenus
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] VerticalMenu menu)
        {
            var stream = new MemoryStream(menu.ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot";
            var response = FilesHelper.UploadImage(stream, folder, file);
            if (!response)
            {
                return BadRequest();
            }
            else
            {
                menu.ImageUrl = file;
                _dbContext.VerticalMenus.Add(menu);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
        }

        // PUT: api/VerticalMenus/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] VerticalMenu menu)
        {
            var entity = _dbContext.VerticalMenus.Find(id);
            if (entity == null)
            {
                return NotFound("No menu found against this id...");
            }

            var stream = new MemoryStream(menu.ImageArray);
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var folder = "wwwroot";
            var response = FilesHelper.UploadImage(stream, folder, file);
            if (!response)
            {
                return BadRequest();
            }
            else
            {
                entity.Title = menu.Title;
                entity.ImageUrl = file;
                _dbContext.SaveChanges();
                return Ok("Menu Updated Successfully...");
            }
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var menu = _dbContext.VerticalMenus.Find(id);
            if (menu == null)
            {
                return NotFound("No menu found against this id...");
            }
            else
            {
                _dbContext.VerticalMenus.Remove(menu);
                _dbContext.SaveChanges();
                return Ok("Menu deleted...");
            }
        }
    }
}
