using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;
namespace TOTO.Controllers.Display
{
    public class DefaultController : Controller
    {
        // GET: Default
        private TOTOContext db = new TOTOContext(); 
        public ActionResult Index()
        {
            tblConfig config = db.tblConfigs.First();
            ViewBag.Title = "<title>" + config.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + config.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + config.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + config.Keywords + "\" /> ";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://Thietbivesinhtoto.vn/\" />";
            meta += "<meta itemprop=\"name\" content=\"" + config.Title + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + config.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"\" />";
            meta += "<meta property=\"og:title\" content=\"" + config.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"\" "+config.Logo+"/>";
            meta += "<meta property=\"og:site_name\" content=\"http://Thietbivesinhtoto.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + config.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
            ViewBag.h1 = "<h1 class=\"h1\">"+config.Title+"</h1>";
            if (Session["registry"] != null)
            {
                ViewBag.Thongbao = Session["registry"].ToString();
            }
            Session["registry"] = null;
            return View();
        }
        public PartialViewResult AdwLeftRight()
        {
            string chuoileft = "";
            string chuoiright = "";
            var listLeft = db.tblImages.Where(p => p.Active == true & p.idCate == 4).ToList();
            for (int i = 0; i < listLeft.Count; i++)
            {
                chuoileft += "<a href=\"" + listLeft[i].Url + "\" title=\"" + listLeft[i].Name + "\"><img src=\"" + listLeft[i].Images + "\" alt=\"" + listLeft[i].Name + "\" width=\"130\" /></a>";
            }
            var listright = db.tblImages.Where(p => p.Active == true & p.idCate == 5).ToList();
            for (int i = 0; i < listright.Count; i++)
            {
                chuoiright += "<a href=\"" + listright[i].Url + "\" title=\"" + listright[i].Name + "\"><img src=\"" + listright[i].Images + "\" alt=\"" + listright[i].Name + "\" width=\"130\" /></a>";
            }
            ViewBag.chuoileft = chuoileft;
            ViewBag.chuoiright = chuoiright;
            return PartialView();
        }
        public PartialViewResult AdwLeftExt()
        {
            tblConfig config = db.tblConfigs.First();
            string chuoi = "";
            if (config.PopupSupport == true)
            {
                var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 8).OrderByDescending(p => p.Ord).Take(1).ToList();
                if (listImage.Count > 0)
                {
                    chuoi += "<div class=\"float-ck\" style=\"left: 0px\">";
                    chuoi += "<div id=\"hide_float_left\">";
                    chuoi += "<a href=\"javascript:hide_float_left()\">Tắt Quảng Cáo [X]</a>";
                    chuoi += "</div>";
                    chuoi += "<div id=\"float_content_left\"> ";
                    if (listImage[0].Link ==true)
                    { chuoi += "<a href=\"" + listImage[0].Url + "\" target=\"_blank\" title=\"" + listImage[0].Name + "\"><img src=\"" + listImage[0].Images + "\" alt=\"" + listImage[0].Name + "\"/></a>"; }
                    else
                    { chuoi += "<a href=\"" + listImage[0].Url + "\" target=\"_blank\" title=\"" + listImage[0].Name + "\" rel=\"" + listImage[0].Link + "\"><img src=\"" + listImage[0].Images + "\" alt=\"" + listImage[0].Name + "\"/></a>"; }

                    chuoi += "</div>";
                    chuoi += "</div>";
                }
            }
            ViewBag.popupSuport = chuoi;
            return PartialView();
        }
        public PartialViewResult Popup()
        {
            tblConfig config = db.tblConfigs.First();
            string chuoi = "";
            if (config.Popup == true)
            {
                var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 7).OrderByDescending(p => p.Ord).Take(1).ToList();
                if (listImage.Count > 0)
                {
                    chuoi += "<div id=\"myModal\" class=\"linhnguyen-modal\">";
                    chuoi += "<a class=\"close-linhnguyen-modal\" title=\"đóng\">X</a>";
                    if (listImage[0].Link == true)
                    { chuoi += "<a href=\"" + listImage[0].Url + "\" target=\"_blank\" title=\"" + listImage[0].Name + "\"><img src=\"" + listImage[0].Images + "\" alt=\"" + listImage[0].Name + "\"/></a>"; }
                    else
                    { chuoi += "<a href=\"" + listImage[0].Url + "\" target=\"_blank\" title=\"" + listImage[0].Name + "\" rel=\"" + listImage[0].Link + "\"><img src=\"" + listImage[0].Images + "\" alt=\"" + listImage[0].Name + "\"/></a>"; }

                    chuoi += "</div>";
                }
            }
            ViewBag.Popup = chuoi;
            return PartialView();
        }
       public ActionResult tagurl()
        {
            var listGroup = db.tblGroupProducts.ToList();
            for (int i = 0; i < listGroup.Count;i++ )
            {
                int id = listGroup[i].id;
                tblGroupProduct groupproduct = db.tblGroupProducts.Find(id);
                groupproduct.Tag = StringClass.NameToTag(groupproduct.Name);
                db.SaveChanges();
            }
                return View();
        }
       public ActionResult Action()
       {
           var ListGroupProduct = db.tblGroupProducts.ToList();
           foreach (var item in ListGroupProduct)
           {
               int id = item.id;
               string name = item.Name;
               string tag = StringClass.NameToTag(name);
               clsSitemap.CreateSitemap(tag + ".html", id.ToString(), "GroupProduct");
           }
           var ListProduct = db.tblProducts.ToList();
           foreach (var item in ListProduct)
           {
               int id = item.id;
               string name = item.Name;
               string tag = StringClass.NameToTag(name);
               clsSitemap.CreateSitemap(tag + "-pd", id.ToString(), "Product");
           }
           var ListNews = db.tblNews.ToList();
           foreach (var item in ListNews)
           {
               int id = item.id;
               string name = item.Name;
               string tag = StringClass.NameToTag(name);
               clsSitemap.CreateSitemap("tin-tuc/" + tag, id.ToString(), "News");
           }
           return View();
       }
    }
}