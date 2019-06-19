using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Entity;
using QBCS.Service.Enum;

namespace QBCS.Service.Implement
{
    public class RuleService: IRuleService
    {
        private IUnitOfWork unitOfWork;

        public RuleService()
        {
            unitOfWork = new UnitOfWork();
        }
        public List<RuleViewModel> getAllRule()
        {
            List<RuleViewModel> result = new List<RuleViewModel>();
            List<RuleValueViewModel> rvvm = new List<RuleValueViewModel>();
            List<RuleKey> rules = unitOfWork.Repository<RuleKey>().GetAll().ToList();
            List<Rule> values = unitOfWork.Repository<Rule>().GetAll().Where(r => r.IsDisable == false).ToList();
            foreach(var rule in rules)
            {
                foreach(var value in values)
                {
                    if(value.KeyId == rule.Id)
                    {
                        var addValue = new RuleValueViewModel()
                        {
                            Id = value.Id,
                            KeyId = (int)value.KeyId,
                            IsCaseSensitive = value.Value.Contains("·case_sensitive·"),
                            Value = value.Value.Replace("·case_sensitive·",""),
                            CreateDate = (DateTime)value.CreateDate,
                            ActivateDate = (DateTime)value.ActivateDate,
                            ValueGroup = value.ValueGroup,
                            IsUse = (bool)value.IsUse
                        };
                        rvvm.Add(addValue);
                    }
                }
                var addResult = new RuleViewModel()
                {
                    Id = rule.Id,
                    Code = rule.Code,
                    Name = rule.Name,
                    Value = rvvm,
                    GroupType = (int) rule.GroupType,
                    GroupTypeEnum = (RuleEnum) rule.GroupType
                };
                result.Add(addResult);
                rvvm = new List<RuleValueViewModel>();
            }

            return result;
        }
        public bool UpdateRule(List<RuleAjaxHandleViewModel> rules)
        {
            List<Rule> disableValues = unitOfWork.Repository<Rule>().GetAll().Where(r => r.IsDisable == false).ToList();
            foreach(var disableValue in disableValues)
            {
                disableValue.IsDisable = true;
                unitOfWork.Repository<Rule>().Update(disableValue);
            }
            unitOfWork.SaveChanges();
            foreach (var rule in rules)
            {
                var entity = new Rule()
                {
                    KeyId = rule.KeyId,
                    Value = rule.Value,
                    ActivateDate = rule.ActivateDate,
                    CreateDate = DateTime.Now,
                    IsDisable = false,
                    IsUse = rule.IsUse
                };
                unitOfWork.Repository<Rule>().Insert(entity);
            }
            unitOfWork.SaveChanges();

            return false;
        }
    }
}
