using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HardwhereApi.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AssetController : ApiController
    {
        // GET api/asset
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/asset/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/asset
        public void Post([FromBody]string value)
        {
        }

        // PUT api/asset/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/asset/5
        public void Delete(int id)
        {
        }
    }
}
