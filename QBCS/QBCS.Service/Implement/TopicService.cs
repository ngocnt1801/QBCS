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
    public class TopicService : ITopicService
    {

        private IUnitOfWork unitOfWork;

        public TopicService()
        {
            unitOfWork = new UnitOfWork();
        }

        public List<TopicViewModel> GetTopicByCourseId(int CourseId)
        {
            IQueryable<Topic> Topics = unitOfWork.Repository<Topic>().GetAll();

            List<Topic> TopicByCourse = Topics.Where(t => t.CourseId == CourseId).ToList();

            List<TopicViewModel> TopicViewModels = new List<TopicViewModel>();

            foreach (var topic in TopicByCourse)
            {
                TopicViewModel topicViewModel = new TopicViewModel()
                {
                    Id = topic.Id,
                    Name = topic.Name,
                    Code = topic.Code,
                    CourseId = (int)topic.CourseId,
                    IsDisable = (bool)topic.IsDisable
                };
                topicViewModel.UpdateIdValue();
                TopicViewModels.Add(topicViewModel);
            }

            return TopicViewModels;
        }
    }
}
