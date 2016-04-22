using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication8.Service;

namespace WebApplication8.Controllers
{
    public class HomeController : Controller
    {
        Service.Service1Client client = new Service1Client();
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public JsonResult GetContact()
        {
            int i = Convert.ToInt32(Request["position"]);
            return Json(client.GetOdataCollectionByAuthByHttpExample(i), JsonRequestBehavior.AllowGet);
        }
        public int CreateUpdataContact(string Id, string Name, string MobilePhone, string Dear, string JobTitle, string BirthDate)
        {
            if (Id.Equals("NAN"))
            {
                client.CreateBpmEntityByOdataHttpExample(Name, MobilePhone, Dear, JobTitle, Convert.ToDateTime(BirthDate));
            return 0;
            }
            else { 
            client.UpdateExistingBpmEnyityByOdataHttpExample(Id, Name, MobilePhone, Dear, JobTitle, Convert.ToDateTime(BirthDate));
            return 1;}
        }
        public int DeleteContact(string Id)
        {

            return Convert.ToInt32(client.DeleteBpmEntityByOdataHttpExample(Id));
        }
    }
}