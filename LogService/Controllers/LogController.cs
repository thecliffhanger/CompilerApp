using EntityDAL;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace LogService.Controllers
{
    public class LogController : ApiController
    {
        // GET api/log
        public IEnumerable<LogItem> Get()
        {
            return new List<LogItem>();
        }

        // GET api/log/5
        public LogItem Get(int id)
        {
            return new LogItem();
        }

        
        // POST api/log
        public bool Post(LogItem value)
        {
            using (var db = new LogDBContext())
            {
                if (value != null)
                {
                    db.LogItems.Add(value);                    
                    db.SaveChanges();
                    return true;
                }                
            }
            return false;
        }

        // PUT api/log/5
        public void Put(int id, [FromBody]LogItem value)
        {
        }

        // DELETE api/log/5
        public void Delete(int id)
        {
        }
    }
}