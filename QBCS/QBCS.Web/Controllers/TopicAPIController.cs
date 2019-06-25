using QBCS.Service.Implement;
using QBCS.Service.Interface;
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
        private ITopicService topicService;
        private ILearningOutcomeService learningOutcomeService;

        public TopicAPIController()
        {
            topicService = new TopicService();
            learningOutcomeService = new LearningOutcomeService();
        }

        [HttpGet]
        [ActionName("topics")]
        public ListLearningOutcomeViewModel GetListTopic(int CourseId)
        {            
            List<LearningOutcomeViewModel> learningOutcomeViewModels = learningOutcomeService.GetLearningOutcomeByCourseId(CourseId);
            ListLearningOutcomeViewModel listTopicLearningOutcomeViewModel = new ListLearningOutcomeViewModel()
            {                
                LearningOutcomes = learningOutcomeViewModels
            };
            return listTopicLearningOutcomeViewModel;
        }
    } 
}
