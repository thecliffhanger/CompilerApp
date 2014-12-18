using EntityDAL;
using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServer.Filters;
using WebServer.Models;

namespace WebServer.Controllers
{
    [HandleException]
    public class HomeController : Controller
    {
        protected ClientModel _clientModel = new ClientModel();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Clients()
        {
            List<Client> list = new List<Client>();
            list = _clientModel.GetClients();           
            return View("Clients",list);

        }

        public ActionResult Logs()
        {
            List<LogItem> list = new List<LogItem>();
            using (var db = new LogDBContext())
            {
                list = db.LogItems.OrderByDescending(x => x.Created).ToList();
            }
            return View("Logs",list);
        }
    }
}
