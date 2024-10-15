using ClothingBrand.Application.Common.DTO.course;
using ClothingBrand.Application.Common.DTO.Request;
using ClothingBrand.Application.Common.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Services
{
    public interface ICourseService
    {
        IEnumerable<courseDto> GetAll();

         courseDto GetCourse(int id);
         void AddCourse(CreateCourse CourseDto);
         void Remove(int id);


        void update(int id, CreateCourse courseDto);
    }
}
