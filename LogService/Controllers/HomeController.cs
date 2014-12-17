using EntityDAL;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<LogItem> list = new List<LogItem>();

            using (var db = new LogDBContext())
            {
                if (db.LogItems.Count() == 0)
                {
                    db.LogItems.Add(new LogItem() { Id = Guid.NewGuid(), LogType = "Error", LogText = "Test", Source = "AnySource", Created = DateTime.Now });
                    db.LogItems.Add(new LogItem() { Id = Guid.NewGuid(), LogType = "Error", LogText = "Test1", Source = "AnySource", Created = DateTime.Now });
                    db.LogItems.Add(new LogItem() { Id = Guid.NewGuid(), LogType = "Error", LogText = "Test2", Source = "AnySource", Created = DateTime.Now });
                    db.LogItems.Add(new LogItem() { Id = Guid.NewGuid(), LogType = "Error", LogText = "Test3", Source = "AnySource", Created = DateTime.Now });
                    db.SaveChanges();
                }
                list = db.LogItems.ToList();
            }
            
            return View(list);

        }
    }
}
