using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Entity;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;

namespace QBCS.Service.Implement
{
    public class LevelService : ILevelService
    {

        private IUnitOfWork u;

        public LevelService()
        {
            u = new UnitOfWork();
        }
        public List<Level> GetLevelByCourse(int? CourseId)
        {
            List<LevelInCourse> LevelInCourses = u.Repository<LevelInCourse>().GetAll().ToList();
            List<LevelInCourse> LevelInCourseIds = LevelInCourses.Where(lic => lic.CourseId == CourseId).ToList();

            List<Level> Levels = u.Repository<Level>().GetAll().ToList();
            List<Level> LevelByCourseId = new List<Level>();
            foreach(LevelInCourse lic in LevelInCourseIds)
            {
                Level lv =Levels.Where(lvl => lvl.Id == lic.LevelId).FirstOrDefault();
                LevelByCourseId.Add(lv);
            }
            return LevelByCourseId;
        }
    }
}
