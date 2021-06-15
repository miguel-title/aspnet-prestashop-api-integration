using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AtomicSeller.Helpers;
using AtomicSeller.ViewModels;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using AtomicSeller;

namespace AtomicSeller.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            //SessionBag.SetSessionBagInitData();
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SetCulture(string culture)
        {
            // Validate input
            culture = CultureHelper.GetImplementedCulture(culture);
            // Save culture in a cookie
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);

            if (Request != null &&
                Request.UrlReferrer != null &&
                !string.IsNullOrWhiteSpace(Request.UrlReferrer.ToString()))
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult GetNewOrders()
        {
            List<CSOPIVM> toto = Prestashop.PrestaShop_GetOrdersToCSOPIVM("open", "2020-09-12T16:15:47-04:00", "2020-09-13T19:15:47-04:00");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult GetProducts()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult PutTrackingInfo()
        {
            string toto = Prestashop.PrestaShop_PutTrackingInfo("6", "", "tracking_number001", "");

            toto = Prestashop.PrestaShop_PutTrackingInfo("4", "", "tracking_TEST32154", "");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult GetOrder()
        {
            string orderStatus = "delvery";
            Prestashop.PrestaShop_GetOrdersToCSOPIVM(orderStatus); 
            return RedirectToAction("Index", "Home");
        }

    }
}