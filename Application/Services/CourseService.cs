using ClothingBrand.Application.Common.DTO.course;
using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using ClothingBrand.Application.Common.Interfaces;
using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public class CourseService:ICourseService
    {

        private readonly IUnitOfWork _unitRepository;
        public CourseService(IUnitOfWork unitRepository)
        {
            _unitRepository = unitRepository;
        }
        public IEnumerable<courseDto> GetAll()
        {
            var iList = _unitRepository.sewingCourseRepository.GetAll()
                .Select(e => new courseDto
                {

                    Title = e.Title,
                    Description = e.Description,
                    Id = e.Id,
                    Duration = e.Duration,
                    Price=e.Price


                });
            return iList;
        }

        public courseDto GetCourse(int id)
        {
            var course = _unitRepository.sewingCourseRepository.Get((x) => x.Id == id,tracked:true);

            var courses = new courseDto
            {


                Title = course.Title,
                Description = course.Description,
                Id = course.Id,
                Duration = course.Duration,
                Price = course.Price

            };


            return courses;
        }


        public void AddCourse(CreateCourse CourseDto)
        {
            var course = new SewingCourse()
            {

                Title = CourseDto.Title,
                Description = CourseDto.Description,
                Price =CourseDto.Price,
                Duration = CourseDto.Duration

            };
            _unitRepository.sewingCourseRepository.Add(course);
            _unitRepository.Save();
        }
        public void Remove(int id)
        {
            _unitRepository.sewingCourseRepository.Remove(_unitRepository.sewingCourseRepository.Get((x) => x.Id == id));
            _unitRepository.Save();

        }
        public void update(int id, CreateCourse courseDto)
        {
            var course = new SewingCourse()
            {

                Title = courseDto.Title,
                Description = courseDto.Description,
                Price = courseDto.Price,
                Duration = courseDto.Duration
                
                

            };
            _unitRepository.sewingCourseRepository.Update(course,id);
            _unitRepository.Save();
        }
    }
}
