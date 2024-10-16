using ClothingBrand.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrand.Application.Common.Interfaces
{
    public interface ISewingCourseRepository : IRepository<SewingCourse>
    {

        void Update(SewingCourse sewingCourse,int id);

    }
}
