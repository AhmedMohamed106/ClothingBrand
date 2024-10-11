//using ClothingBrand.Application.Common.DTO.EnrollmentDto;
//using ClothingBrand.Application.Common.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace ClothingBrand.Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EnrollController : ControllerBase
//    {
//        private readonly IUnitOfWork unitOfWork;
//        public EnrollController(IUnitOfWork unitOfWork)
//        {
//            this.unitOfWork = unitOfWork;
//        }

//        //[HttpGet]
//        //public IActionResult GetAllUser()
//        //{
           
//        //    var enrollDb = unitOfWork.enrollmentRepository.Get(e => e.UserId == userid);
//        //    if (enrollDb == null)
//        //        return NotFound();

//        //    EnrollDto enrollDto = new EnrollDto
//        //    {
//        //        CourseId = enrollDb.SewingCourseId,
//        //        UserId = enrollDb.UserId,
//        //        Courses = new List<string>()
//        //    };
//        //    foreach (var item in enrollDb.ApplicationUser.SewingCourses)
//        //    {
//        //        enrollDto.Users.Add(item.Title);
//        //    }
//        //    return Ok(enrollDto);
//        //}






//        [HttpGet("{id}")]
//        public IActionResult GetUsersForCourse(int courseid)
//        {
//            if (courseid == 0||courseid==null) 
//                return NotFound();
//            var enrollDb = unitOfWork.enrollmentRepository.Get(e=>e.SewingCourseId==courseid);
//            if(enrollDb ==null)
//                return NotFound();

//            EnrollDto enrollDto=new EnrollDto
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
//                UserId= enrollDb.UserId,
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
