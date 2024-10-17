using Application.DTOs.Response.Account;
using ClothingBrand.Application.Common.DTO.course;
using ClothingBrand.Application.Common.DTO.EnrollmentDto;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface IEnrollmentCourseService
    {
        public IEnumerable<EnrollDto> GetAll();

        public EnrollDto GetEnrollCourse(int courseId, string userId);


        public void AssignCourseToUser(CreateEnrollmentDto CourseDto);
        public void Remove(int courseId, string userId);
        public IEnumerable<courseDto> GetCoursesForUser(string UserId);

        public IEnumerable<UserClaims> GetUserEnrollInCourse(int CourseId);
    }
}
