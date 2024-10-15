//using ClothingBrand.Application.Common.DTO.course;
//using ClothingBrand.Application.Common.DTO.EnrollmentDto;
//using ClothingBrand.Application.Common.Interfaces;
//using ClothingBrand.Application.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ClothingBrand.Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EnrollController : ControllerBase
//    {
//        private readonly IEnrollmentCourseService _enrollService;
//        public EnrollController(IEnrollmentCourseService courseService)
//        {
//            this._enrollService = courseService;
//        }

//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var courses = _enrollService.();


//            return Ok(courses);
//        }
//        [HttpPost]
//        public IActionResult Create(CreateCourse course)
//        {
//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _enrollService.AddCourse(course);

//                    return Ok();
//                }
//                catch (Exception e)
//                {
//                    return BadRequest(e.InnerException?.Message);
//                }
//            }
//            return BadRequest();
//        }


//        [HttpPut]
//        [Route("{id:int}")]
//        public IActionResult Update(int id, CreateCourse course)
//        {
//            if (ModelState.IsValid)
//            {



//                try
//                {
//                    _enrollService.update(id, course);

//                    return Ok();
//                }
//                catch (Exception e)
//                {
//                    return BadRequest(e.InnerException?.Message);
//                }

//            }
//            return BadRequest();
//        }

//        [HttpDelete]
//        [Route("{id:int}")]
//        public IActionResult Delete(int id)
//        {

//            try
//            {
//                _enrollService.Remove(id);

//                return Ok();
//            }
//            catch (Exception e)
//            {
//                return BadRequest(e.InnerException?.Message);
//            }


//        }

//        [HttpGet("{id}")]
//        public IActionResult Get(int id)
//        {
//            var course = _enrollService.GetCourse(id);
//            return Ok(course);
//        }








//        [HttpGet("{id}")]
//        public IActionResult GetUsersForCourse(int courseid)
//        {
//            if (courseid == 0 || courseid == null)
//                return NotFound();
//            var enrollDb = unitOfWork.enrollmentRepository.Get(e => e.SewingCourseId == courseid);
//            if (enrollDb == null)
//                return NotFound();

//            EnrollDto enrollDto = new EnrollDto
//            {
//                CourseId = enrollDb.SewingCourseId,
//                Users = new List<string>()
//            };
//            foreach (var item in enrollDb.SewingCourse.EnrollmentUsers)
//            {
//                enrollDto.Users.Add(item.UserName);
//            }
//            return Ok(enrollDto);
//        }


//        [HttpGet("{id}/{course}")]
//        public IActionResult GetCoursesForUser(int userid)
//        {
//            if (userid == 0 || userid == null)
//                return NotFound();
//            var enrollDb = unitOfWork.enrollmentRepository.Get(e => e.UserId == userid);
//            if (enrollDb == null)
//                return NotFound();

//            EnrollDto enrollDto = new EnrollDto
//            {
//                CourseId = enrollDb.SewingCourseId,
//                UserId = enrollDb.UserId,
//                Courses = new List<string>()
//            };
//            foreach (var item in enrollDb.ApplicationUser.SewingCourses)
//            {
//                enrollDto.Users.Add(item.Title);
//            }
//            return Ok(enrollDto);
//        }

//    }
//}
