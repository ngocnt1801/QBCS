﻿using QBCS.Repository.Implement;
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
                            Value = value.Value,
                            CreateDate = (DateTime)value.CreateDate,
                            ActivateDate = (DateTime)value.ActivateDate,
                            ValueGroup = value.ValueGroup
                        };
                        rvvm.Add(addValue);
                    }
                }
                var addResult = new RuleViewModel()
                {
                    Code = rule.Code,
                    Name = rule.Name,
                    Value = rvvm,
                    //GroupType = (int) rule.GroupType,
                    //GroupTypeEnum = (RuleEnum) rule.GroupType
                };
                result.Add(addResult);
                rvvm = new List<RuleValueViewModel>();
            }

            return result;
        }
        public bool UpdateRule(RuleViewModel rules)
        {
            var ruleKeyId = rules.Value.FirstOrDefault().KeyId;
            List<Rule> disableValues = unitOfWork.Repository<Rule>().GetAll().Where(r => r.IsDisable == false).ToList();
            foreach(var disableValue in disableValues)
            {
                disableValue.IsDisable = true;
                unitOfWork.Repository<Rule>().Update(disableValue);
            }
            unitOfWork.SaveChanges();
            foreach (var rule in rules.Value)
            {
                var entity = new Rule()
                {
                    KeyId = ruleKeyId,
                    Value = rule.Value,
                    ActivateDate = rule.ActivateDate,
                    CreateDate = DateTime.Now,
                    ValueGroup = rule.ValueGroup,
                    IsDisable = false
                };
                unitOfWork.Repository<Rule>().Insert(entity);
            }
            unitOfWork.SaveChanges();

            return false;
        }
    }
}