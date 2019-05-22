using QBCS.Entity;
using QBCS.Service.Interface;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class CourseService: ICourseService
    {

        private IUnitOfWork unitOfWork;

        public CourseService()
        {
            unitOfWork = new UnitOfWork();
        }

        public List<Course> GetCoursesByName(string name)
        {
            IQueryable<Course> courses = unitOfWork.Repository<Course>().GetAll().Where(c => c.Name.Contains(name));
            List<Course> result = courses.ToList();
            return result;
        }
    }
}
