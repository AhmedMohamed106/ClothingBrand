using Application.DTOs.Response.Account;
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
                    UserId=e.ApplicationUserId,
                    Users = new List<string> { e.ApplicationUser?.UserName ?? string.Empty },
                    Courses= new List<string> { e.SewingCourse?.Title ?? string.Empty }
                    
                });
         
            return iList;
        }

        public EnrollDto GetEnrollCourse(int courseId,string userId)
        {
            var course = _unitRepository.enrollmentRepository.Get((x) => x.ApplicationUserId == userId&&x.SewingCourseId==courseId);

            var courses = new EnrollDto
            {
                CourseId = course.SewingCourseId,
                EnrollDate = course.EnrollDate.ToString(),
                UserId = course.ApplicationUserId,
                Users = new List<string> { course.ApplicationUser?.UserName ?? string.Empty },
                Courses = new List<string> { course.SewingCourse?.Title ?? string.Empty }



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
        public IEnumerable<courseDto> GetCoursesForUser(string UserId)
        {
           var courses= _unitRepository.enrollmentRepository.GetCoursesForUser(UserId);
            var courseDtos=new List<courseDto>() ;
            foreach (var course in courses)
            {
                var corDTo = new courseDto()
                {
                    Description = course.Description,
                    Duration = course.Duration,
                    Id = course.Id,
                    Price = course.Price,
                    Title = course.Title

                };
                courseDtos.Add(corDTo);
            }
            return courseDtos;
        }

        public IEnumerable<UserClaims> GetUserEnrollInCourse(int CourseId)
        {
            var users = _unitRepository.enrollmentRepository.GetUserEnrollInCourse(CourseId);
            var usersDtos = new List<UserClaims>();
            foreach (var user in users)
            {
                var userDTo = new UserClaims()
                {
                    Email=user.Email,
                    Fullname=user.Name,
                    UserName=user.UserName

                };
                usersDtos.Add(userDTo);
            }
            return usersDtos;
        }
    }
}
