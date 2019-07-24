using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ICategoryService
    {
        List<CategoryViewModel> GetCategoriesByCourseId(int courseId);

        List<CategoryViewModel> GetListCategories(int courseId);
        List<CategoryViewModel> GetAllCategories();
        CategoryViewModel GetCategoryById(int categoryId);
        void AddCategory(CategoryViewModel model);
        void DeleteCategory(int categoryId);
        void UpdateCategory(CategoryViewModel model);
    }
}
