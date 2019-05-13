using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;

namespace QBCS.Web.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
