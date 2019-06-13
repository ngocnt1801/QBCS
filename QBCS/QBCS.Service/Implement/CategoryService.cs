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
    public class CategoryService : ICategoryService
    {
        private IUnitOfWork unitOfWork;
        public CategoryService()
        {
            unitOfWork = new UnitOfWork();
        }
        public List<CategoryViewModel> GetCategoriesByCourseId(int courseId)
        {
            List<CategoryViewModel> categoriesByCourseId = unitOfWork.Repository<Category>().GetAll().Where(c => c.CourseId == courseId).Select(c => new CategoryViewModel 
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return categoriesByCourseId;
        }
    }
}
