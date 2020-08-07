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
    public class MarqueeBannersController : ControllerBase
    {
        private FoodDbContext _dbContext;
        public MarqueeBannersController(FoodDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/MarqueeBanners
        [HttpGet]
        public IActionResult Get()
        {
            var marqueeBanners = from c in _dbContext.MarqueeBanners
                                  select new
                                  {
                                      Id = c.Id,
                                      Title = c.Title,
                                      BannerContent = c.BannerContent,
                                      Image = c.Image,
                                      PublishDate = c.PublishDate,
                                      IsEnable = c.IsEnable,
                                      Seq = c.Seq
                                  };

            return Ok(marqueeBanners);
        }

        // GET: api/MarqueeBanners/5
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var marqueeBanner = (from c in _dbContext.MarqueeBanners
                                  where c.Id == id
                                  select new
                                  {
                                      Id = c.Id,
                                      Title = c.Title,
                                      BannerContent = c.BannerContent,
                                      Image = c.Image,
                                      PublishDate = c.PublishDate,
                                      IsEnable = c.IsEnable,
                                      Seq = c.Seq
                                  }).FirstOrDefault();

            return Ok(marqueeBanner);

        }

        // POST: api/MarqueeBanners
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromBody] MarqueeBanner marqueeBanner)
        {
            var stream = new MemoryStream(marqueeBanner.ImageArray);
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
                marqueeBanner.Image = file;
                _dbContext.MarqueeBanners.Add(marqueeBanner);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
        }

        // PUT: api/MarqueeBanners/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MarqueeBanner marqueeBanner)
        {
            var entity = _dbContext.MarqueeBanners.Find(id);
            if (entity == null)
            {
                return NotFound("No marqueebanner found against this id...");
            }

            var stream = new MemoryStream(marqueeBanner.ImageArray);
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
                entity.Title = marqueeBanner.Title;
                entity.Image = file;
                _dbContext.SaveChanges();

                return Ok("MarqueeBanner Updated Successfully...");
            }
        }

        // DELETE: api/ApiWithActions/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var menu = _dbContext.MarqueeBanners.Find(id);
            if (menu == null)
            {
                return NotFound("No marqueebanner found against this id...");
            }
            else
            {
                _dbContext.MarqueeBanners.Remove(menu);
                _dbContext.SaveChanges();
                return Ok("MarqueeBanner deleted...");
            }
        }
    }
}
