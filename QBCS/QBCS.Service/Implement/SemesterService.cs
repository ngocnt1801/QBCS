using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Service.ViewModel;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;
using QBCS.Entity;

namespace QBCS.Service.Implement
{
    public class SemesterService : ISemesterService
    {
        private IUnitOfWork unitOfWork;
        public SemesterService()
        {
            unitOfWork = new UnitOfWork();
        }
        public List<SemesterViewModel> GetAllSemester()
        {
            List<Semester> semesters = unitOfWork.Repository<Semester>().GetAll().ToList();
            List<SemesterViewModel> semesterViewModel = new List<SemesterViewModel>();
            if (semesters != null)
            {
                semesterViewModel = semesters.Select(s => new SemesterViewModel
                {
                    Id = (int)s.Id,
                    Name = s.Name
                }).ToList();
            }
            return semesterViewModel;
        }

        public SemesterViewModel GetById(int semesterId)
        {
            SemesterViewModel result;
            Semester semester = unitOfWork.Repository<Semester>().GetById(semesterId);
            if(semester != null)
            {
                result = new SemesterViewModel
                {
                    Id = (int) semester.Id,
                    Name = semester.Name
                };
            } else
            {
                result = new SemesterViewModel
                {
                    Id = 0,
                    Name = "[NoSemester]"
                };
            }
            return result;
        }
    }
}
