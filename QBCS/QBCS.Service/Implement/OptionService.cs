using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Service.ViewModel;

namespace QBCS.Service.Implement
{
    
    public class OptionService : IOptionService
    {
        private IUnitOfWork unitOfWork;
        public OptionService()
        {
            unitOfWork = new UnitOfWork();
        }

        public List<Option> GetOptionsByQuestion(int id)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOptions(List<OptionViewModel> OptionViewModels)
        {
            foreach(var optionViewModel in OptionViewModels)
            {
                Option option = unitOfWork.Repository<Option>().GetById(optionViewModel.Id);
                option.OptionContent = optionViewModel.OptionContent;
                option.IsCorrect = optionViewModel.IsCorrect;
                unitOfWork.Repository<Option>().Update(option);
                unitOfWork.SaveChanges();
            }

            return true;
        }
    }
}
