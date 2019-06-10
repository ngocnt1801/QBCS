using DuplicateQuestion;
using Newtonsoft.Json;
using QBCS.Service.Implement;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace QBCS.Web.Controllers
{

    public class ValuesController : ApiController
    {
        private TestService testService;

        public ValuesController()
        {
            testService = new TestService();
        }

        // GET api/values
        public string Get()
        {
            //var result = testService.CheckDuplidate();
            List<string> options = new List<string>();
            options.Add("Option 1");
            options.Add("Option 2");
            options.Add("Option 3");
            return JsonConvert.SerializeObject(options, Formatting.Indented);
        }

        [HttpGet]
        public double TestTF(string s1, string s2)
        {
            return TFAlgorithm.CaculateSimilar(s1, s2);
        }

        // GET api/values/test
        [HttpGet]
        [ActionName("test")]
        public IHttpActionResult TestFunction()
        {
            //return NotFound();
            return Ok(new {
                name = "ngoc",
                subject = "capstone"
            });
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
