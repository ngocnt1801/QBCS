using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{ 
    public class CourseService : ICourse
    {
        private IUnitOfWork unitOfWork;
        public CourseService()
        {
            unitOfWork = new UnitOfWork();
        }
        public ArrayList GetAllCourses()
        {
            ArrayList listCourse = new ArrayList();
            var list = unitOfWork.Repository<Course>().GetAll().ToList();
            listCourse.Add(list);
            return listCourse;
        }
    }
}
