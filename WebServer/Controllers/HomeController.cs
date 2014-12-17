using EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebServer.Models;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        protected ClientModel _clientModel = new ClientModel();

        public ActionResult Index()
        {
            List<Client> list = new List<Client>();
            list = _clientModel.GetClients();           
            return View(list);

        }
    }
}
