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

        private IUnitOfWork unitOfWork;

        public LevelService()
        {
            unitOfWork = new UnitOfWork();
        }

        public int GetIdByName(string levelName)
        {
            IQueryable<Level> levels = unitOfWork.Repository<Level>().GetAll();
            Level level = levels.Where(l => (l.Name.ToLower()).Equals(levelName.ToLower())).FirstOrDefault();
            if (level == null)
            {
                return 0;
            }
            else
            {
                return (int)level.Id;
            }
        }

        public List<LevelViewModel> GetLevel()
        {

            IQueryable<Level> levels = unitOfWork.Repository<Level>().GetAll();
            List<LevelViewModel> levelViewModels = new List<LevelViewModel>();
            foreach (Level leval in levels)
            {
                LevelViewModel levelViewModel = new LevelViewModel()
                {
                    Id = leval.Id,
                    Name = leval.Name
                };
                levelViewModels.Add(levelViewModel);

            }
            return levelViewModels;
        }

        public LevelViewModel GetLevelById(int levelId)
        {
            Level levelById = unitOfWork.Repository<Level>().GetById(levelId);
            LevelViewModel result = new LevelViewModel();
            if (levelById == null)
            {
                result = new LevelViewModel()
                {
                    Id = 0,
                    Name = ""
                };
                return result;
            }
            result = new LevelViewModel()
            {
                Id = levelById.Id,
                Name = levelById.Name
            };
            return result;
        }
    }
}
