using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QBCS.Web.Controllers
{
    public class TopicAPIController : ApiController
    {
        [HttpGet]
        [ActionName("topics")]
        public List<TopicViewModel> GetListTopic(int CourseId)
        {
            List<TopicViewModel> topicViewModels = new List<TopicViewModel>();
            for (int i = CourseId; i < 6; i++)
            {
                TopicViewModel topicViewModel = new TopicViewModel()
                {
                    Id = i,
                    Code = "Topic " + i,
                    Name = "Topic " + i,
                    CourseId = 0,
                    IsDisable = false
                };
                topicViewModels.Add(topicViewModel);
            }
            return topicViewModels;
        }
    } 
}
