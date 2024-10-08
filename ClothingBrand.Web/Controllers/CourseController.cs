using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//namespace ClothingBrand.Web.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
////    public class CourseController : ControllerBase
////    {
////        private readonly IUnitOfWork unitOfWork;
////        public CourseController(IUnitOfWork unitOfWork)
////        {
////            this.unitOfWork = unitOfWork;
////        }

////        [HttpGet]
////        public IActionResult GetAll()
////        {
////            var courses = unitOfWork.sewingCourseRepository.GetAll();
////            return Ok(courses);
////        }
////        [HttpPost]
////        public IActionResult Create(SewingCourse course)
////        {
////            if (ModelState.IsValid)
////            {
////                try
////                {
////                    unitOfWork.sewingCourseRepository.Add(course);
////                    unitOfWork.Save();
////                    return Ok();
////                }
////                catch (Exception e)
////                {
////                    return BadRequest(e.InnerException?.Message);
////                }
////            }
////            return BadRequest();
////        }


////        [HttpPut]
////        [Route("{id:int}")]
////        public IActionResult Update(int id, SewingCourse course)
////        {
////            if (ModelState.IsValid)
////            {
////                var oldcourse =unitOfWork.sewingCourseRepository.Get(x => x.Id == id);

////                if (oldcourse == null)
////                    return BadRequest();
////                try
////                {
////                    unitOfWork.sewingCourseRepository.Update(course);
////                    unitOfWork.Save();
////                    return Ok();
////                }
////                catch (Exception e)
////                {
////                    return BadRequest(e.InnerException?.Message);
////                }

////            }
////            return BadRequest();
////        }

////        [HttpDelete]
////        [Route("{id:int}")]
////        public IActionResult Delete(int? id)
////        {
////            if(id==null)
////                return BadRequest();
////            var course = unitOfWork.sewingCourseRepository.Get(c => c.Id == id); //GetAll().Where(c=>c.Id==id).FirstOrDefault();
////            if(course == null)
////                return BadRequest();
////            try
////            {
////                unitOfWork.sewingCourseRepository.Remove(course);
////                unitOfWork.Save();
////                return Ok();
////            }
////            catch (Exception e)
////            {
////                return BadRequest(e.InnerException?.Message);
////            }


////        }


////    }
////}
