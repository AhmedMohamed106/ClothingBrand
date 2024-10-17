using ClothingBrand.Application.Common.DTO.course;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            this._courseService = courseService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseService.GetAll();
           

            return Ok(courses);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateCourse course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _courseService.AddCourse(course);
                    
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException?.Message);
                }
            }
            return BadRequest();
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, CreateCourse course)
        {
            if (ModelState.IsValid)
            {
               

               
                try
                {
                    _courseService.update(id,course);
                   
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e.InnerException?.Message);
                }

            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
          
            try
            {
                _courseService.Remove(id);
                
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException?.Message);
            }


        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var course = _courseService.GetCourse(id);
            return Ok(course);
        }


    }
}