using ClothingBrand.Application.Common.DTO.course;
using ClothingBrand.Application.Common.DTO.EnrollmentDto;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public class EnrollmentCourseService:IEnrollmentCourseService
    {
        private readonly IUnitOfWork _unitRepository;
        public EnrollmentCourseService(IUnitOfWork unitRepository)
        {
            _unitRepository = unitRepository;
        }

        public IEnumerable<EnrollDto> GetAll()
        {
            var iList = _unitRepository.enrollmentRepository.GetAll(includeProperties: "SewingCourse,ApplicationUser")

                .Select(e => new EnrollDto
                {
                    CourseId=e.SewingCourseId,
                    EnrollDate=e.EnrollDate.ToString(),
                    UserId=e.ApplicationUserId
                    




                });
            return iList;
        }

        public EnrollDto GetCourse(int courseId,string userId)
        {
            var course = _unitRepository.enrollmentRepository.Get((x) => x.ApplicationUserId == userId&&x.SewingCourseId==courseId);

            var courses = new EnrollDto
            {
                CourseId = course.SewingCourseId,
                EnrollDate = course.EnrollDate.ToString(),
                UserId = course.ApplicationUserId



            };


            return courses;
        }


        public void AssignCourseToUser(CreateEnrollmentDto CourseDto)
        {
            var course = new Enrollment()
            {


                SewingCourseId = CourseDto.CourseId,
                EnrollDate = Convert.ToDateTime(CourseDto.EnrollDate),
                ApplicationUserId = CourseDto.UserId

            };
            _unitRepository.enrollmentRepository.Add(course);
            _unitRepository.Save();
        }
        public void Remove(int courseId, string userId)
        {
            _unitRepository.enrollmentRepository.Remove(_unitRepository.enrollmentRepository.Get((x) => x.ApplicationUserId == userId && x.SewingCourseId == courseId));
            _unitRepository.Save();

        }
        //public void update(int id, CreateCourse courseDto)
        //{
        //    var course = new SewingCourse()
        //    {

        //        Title = courseDto.Title,
        //        Description = courseDto.Description,
        //        Price = courseDto.Price,
        //        Duration = courseDto.Duration



        //    };
        //    _unitRepository.enrollmentRepository.Update(course);
        //    _unitRepository.Save();
        //}
    }
}
