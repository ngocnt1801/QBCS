using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Service.ViewModel;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;

namespace QBCS.Service.Implement
{
    public class ExaminationService : IExaminationService
    {
        private IUnitOfWork unitOfWork;
        public ExaminationService()
        {
            unitOfWork = new UnitOfWork();
        }
        public GenerateExamViewModel GenerateExamination(GenerateExamViewModel exam)
        {
            int questionEasy = (exam.TotalQuestion * exam.EasyPercent) / 100;
            int questionNormal = (exam.TotalQuestion * exam.NormalPercent) / 100;
            int questionHard = exam.TotalQuestion - questionEasy - questionNormal;
            exam.EasyQuestion = questionEasy;
            exam.NormalQuestion = questionNormal;
            exam.HardQuestion = questionHard;
            List<TopicInExamination> topics = new List<TopicInExamination>();
            foreach (string topic in exam.Topic)
            {
                int id = int.Parse(topic.Substring(3));
                bool isLearingOutcome = false;
                if (topic.Contains("LO_"))
                {
                    isLearingOutcome = true;
                }
                TopicInExamination topicInExam = new TopicInExamination()
                {
                    Id = id,
                    IsLearingOutcome = isLearingOutcome
                };
                topics.Add(topicInExam);
            }
            while (questionEasy != 0 || questionNormal != 0 || questionHard != 0) {
                for (int i = 0; i < topics.Count; i++)
                {
                    if (questionEasy != 0)
                    {
                        topics[i].EasyQuestion = topics[i].EasyQuestion + 1;
                        questionEasy--;
                    } else if (questionNormal != 0)
                    {
                        topics[i].NormalQuestion = topics[i].NormalQuestion + 1;
                        questionNormal--;
                    } else if (questionHard != 0)
                    {
                        topics[i].HardQuestion = topics[i].HardQuestion + 1;
                        questionHard--;
                    } else
                    {
                        break;
                    }
                }
            }
            return exam;
        }
    }
}
