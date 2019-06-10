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

        public int GetCourseIdByTopicId(int topicId)
        {
            Topic topic = unitOfWork.Repository<Topic>().GetById(topicId);
            return (int)topic.CourseId;
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
        public List<TopicViewModel> GetAllTopic()
        {
            List<Topic> topics = unitOfWork.Repository<Topic>().GetAll().Where(t => t.IsDisable == false).ToList();
            List<TopicViewModel> result = new List<TopicViewModel>();
            foreach (var topic in topics)
            {
                TopicViewModel model = new TopicViewModel()
                {
                    Id = topic.Id,
                    Code = topic.Code,
                    Name = topic.Name,
                    CourseId = (int)topic.CourseId,
                    CourseName = topic.Course.Name
                };
                result.Add(model);
            }
            return result;
        }
        public bool UpdateDisable(int id)
        {
            try
            {
                var topic = unitOfWork.Repository<Topic>().GetById(id);
                topic.IsDisable = true;
                unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool AddTopic(TopicViewModel model)
        {
            try
            {
                var topic = new Topic()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CourseId = model.CourseId,
                    IsDisable = false
                };
                unitOfWork.Repository<Topic>().Insert(topic);
                unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateTopic(TopicViewModel model)
        {
            try
            {
                var topic = unitOfWork.Repository<Topic>().GetById(model.Id);
                topic.Code = model.Code;
                topic.Name = model.Name;
                topic.CourseId = model.CourseId;
                unitOfWork.Repository<Topic>().Update(topic);
                unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }
        public TopicViewModel GetTopicById(int id)
        {
            var topic = unitOfWork.Repository<Topic>().GetById(id);
            var result = new TopicViewModel()
            {
                Id = topic.Id,
                Code = topic.Code,
                Name = topic.Name,
                CourseId = (int)topic.CourseId,
                CourseName = topic.Course.Name,
                IsDisable = (bool)topic.IsDisable
            };
            return result;
        }
    }
}
