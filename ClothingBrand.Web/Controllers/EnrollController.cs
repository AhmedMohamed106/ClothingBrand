using ClothingBrand.Application.Common.DTO.course;
using ClothingBrand.Application.Common.DTO.EnrollmentDto;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingBrand.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollController : ControllerBase
    {
        private readonly IEnrollmentCourseService _enrollService;
        public EnrollController(IEnrollmentCourseService courseService)
        {
            this._enrollService = courseService;
        }

        [HttpGet]
        [Authorize(Roles = "user , Admin")] // Require authentication
        public IActionResult GetAll()
        {
            var courses = _enrollService.GetAll();


            return Ok(courses);
        }
        [HttpPost]
        [Authorize(Roles = "user,Admin")] // Require authentication
        public IActionResult Create(CreateEnrollmentDto Enrollcourse)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _enrollService.AssignCourseToUser(Enrollcourse);

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
        [Authorize(Roles = "Admin,user")] // Require authentication and specific roles
        [Route("{CourseID:int}/userId/{userID}")]
        public IActionResult Delete(int CourseID,string userID)
        {

            try
            {
                _enrollService.Remove(CourseID,userID);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException?.Message);
            }


        }


        [Authorize(Roles = "user , Admin")] // Require authentication
        [HttpGet("{CourseID:int}/userId/{userID}")]
        public IActionResult Get(int CourseID, string userID)
        {
            var course = _enrollService.GetEnrollCourse(CourseID, userID);
            return Ok(course);
        }








        [HttpGet("{courseid}")]
        [Authorize(Roles = "Admin")] // Require Admin role
        public IActionResult GetUsersForCourse(int courseid)
        {
           var userDtos = _enrollService.GetUserEnrollInCourse(courseid);
            return Ok(userDtos);
        }


        [HttpGet("GetCoursesForUser/{userid}")]
        [Authorize(Roles = "Admin , user")] // Require Admin role
        public IActionResult GetCoursesForUser(string userid)
        {
            var CoursesDtos = _enrollService.GetCoursesForUser(userid);
            return Ok(CoursesDtos);
        }

    }
}
