using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ITopicService
    {
        List<TopicViewModel> GetTopicByCourseId(int? CourseId);
        List<TopicViewModel> GetAllTopic();
        bool UpdateDisable(int id);
        bool UpdateTopic(TopicViewModel model);
        TopicViewModel GetTopicById(int id);
        bool AddTopic(TopicViewModel model);
    }

}
