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
            
            IQueryable<Level> Levels = u.Repository<Level>().GetAll();
            List<LevelViewModel> levelViewModels = new List<LevelViewModel>();
            foreach(Level leval in Levels)
            {
                LevelViewModel lvm = new LevelViewModel()
                {
                    Id = leval.Id,
                    Name = leval.Name
                };
                levelViewModels.Add(lvm);

            }
            return levelViewModels;
        }
    }
}
