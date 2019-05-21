using QBCS.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class QuestionController : Controller
    {
        private IQuestionService q;
        private IOptionService o;

        public QuestionController()
        {
            q = new QuestionService();
            o = new OptionService();
        }


        // GET: Question
        public ActionResult GetListQuestion(int id)
        {
            List<QuestionViewModel> ListQuestion = new List<QuestionViewModel>();

            List<Question> Questions = q.GetQuestionsByCourse(id);
            foreach (Question ques in Questions )
            {
                List<Option> op = o.GetOptionsByQuestion(ques.Id);
                QuestionViewModel qvm = new QuestionViewModel
                {
                    Question = ques,
                    Options = op
                };
                ListQuestion.Add(qvm);
            }
            return View("ListQuestion", ListQuestion);
        }
    }
}