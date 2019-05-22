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
    public class TopicService : ITopicService
    {

        private IUnitOfWork u;

        public TopicService()
        {
            u = new UnitOfWork();
        }
        public List<Topic> GetTopicByCourseId(int? CourseId)
        {
            List<Topic> Topics = u.Repository<Topic>().GetAll().ToList();

            List<Topic> TopicByCourse = Topics.Where(t => t.CourseId == CourseId).ToList();

            return TopicByCourse;
        }
    }
}
