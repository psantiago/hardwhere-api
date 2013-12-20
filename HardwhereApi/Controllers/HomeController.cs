using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using HardwhereApi.Core.Models;
using HardwhereApi.Infrastructure;

namespace HardwhereApi.Controllers
{
    public class HomeController : Controller
    {
        private HardwhereApiContext _db = new HardwhereApiContext();
        public ActionResult Index()
        {
            var entry = _db.Users.ToList();
            //_db.SaveChanges();

            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
