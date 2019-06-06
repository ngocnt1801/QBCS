using DuplicateQuestion.Entity;
using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class ImportService : IImportService
    {
        private IUnitOfWork unitOfWork;

        public ImportService()
        {
            unitOfWork = new UnitOfWork();
        }

        public ImportResultViewModel GetImportResult(int importId)
        {
            var bankQuery = unitOfWork.Repository<Question>().GetAll();
            var import = unitOfWork.Repository<Import>().GetAll().Where(i => i.Id == importId && i.Status == (int)StatusEnum.Checked).FirstOrDefault();
            if (import != null)
            {
                return new ImportResultViewModel
                {
                    Id = import.Id,
                    Questions = import.QuestionTemps.Select(q => new QuestionTempViewModel
                    {
                        Id = q.Id,
                        QuesitonContent = q.QuestionContent,
                        Status = (StatusEnum)q.Status
                    }).ToList()
                };
            }

            return null;
        }
    }
}
