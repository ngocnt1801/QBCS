using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Entity;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;
using QBCS.Service.ViewModel;

namespace QBCS.Service.Implement
{
    public class LevelService : ILevelService
    {

        private IUnitOfWork u;

        public LevelService()
        {
            u = new UnitOfWork();
        }
        public List<LevelViewModel> GetLevel()
        {
            
            List<Level> Levels = u.Repository<Level>().GetAll().ToList();
            List<LevelViewModel> LevelViewModels = new List<LevelViewModel>();
            foreach(Level lv in Levels)
            {
                LevelViewModel lvm = new LevelViewModel()
                {
                    Id = lv.Id,
                    Name = lv.Name
                };
                LevelViewModels.Add(lvm);

            }
            return LevelViewModels;
        }
    }
}
