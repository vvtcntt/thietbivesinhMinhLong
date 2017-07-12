using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
namespace TOTO.Controllers.Display.Session.News
{
    public class NewsController : Controller
    {
        private TOTOContext db = new TOTOContext();

        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        string nUrl = "";
        public string UrlNews(int idCate)
        {
            var ListMenu = db.tblGroupNews.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <a href=\"/2/" + ListMenu[i].Tag + "\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlNews(id);
                }

            }
            return nUrl;
        }
        public ActionResult NewsDetail(string tag)
        {
            string url = Request.Url.ToString();
            string[] mang = url.Split('/');
            if (mang.Length > 5)
            {
                return Redirect("/tin-tuc/" + tag + "");
            }
            var tblnews = db.tblNews.First(p => p.Tag == tag);
            int id = tblnews.id;
            ViewBag.Title = "<title>" + tblnews.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + tblnews.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblnews.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblnews.Keyword + "\" /> ";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://thietbivesinhminhlong.vn/tin-tuc/" + StringClass.NameToTag(tag) + "\" />";
            meta += "<meta itemprop=\"name\" content=\"" + tblnews.Name + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"\" />";
            meta += "<meta property=\"og:title\" content=\"" + tblnews.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"News\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"\" />";
            meta += "<meta property=\"og:site_name\" content=\"http://thietbivesinhminhlong.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + tblnews.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
            int idCates = int.Parse(tblnews.idCate.ToString());
            
            string Urlgroup = db.tblGroupNews.Find(idCates).Tag;
            if (tblnews.Tabs != null)
            {
                string Chuoi = tblnews.Tabs;
                string[] Mang = Chuoi.Split(',');

                List<int> araylist = new List<int>();
                for (int i = 0; i < Mang.Length; i++)
                {

                    string tabs = Mang[i].ToString();
                    var listnew = db.tblNews.Where(p => p.Tabs.Contains(tabs) && p.id != id && p.Active == true).ToList();
                    for (int j = 0; j < listnew.Count; j++)
                    {
                        araylist.Add(listnew[j].id);
                    }

                }


                var listnewlienquan = db.tblNews.Where(p => araylist.Contains(p.id) && p.Active == true && p.id != id).OrderByDescending(p => p.Ord).Take(3).ToList();
                string chuoinew = "";
                if (listnewlienquan.Count > 0)
                {

                    chuoinew += " <div class=\"Tintuclienquan\">";
                    for (int i = 0; i > listnewlienquan.Count; i++)
                    {
                        chuoinew += "<a href=\"/tin-tuc/" + listnewlienquan[i].Tag + "\" title=\"\">› " + listnewlienquan[i].Name + "</a>";
                    }
                    chuoinew += "</div>";
                }
                ViewBag.chuoinew = chuoinew;


                //Load tin mới

            }
            int iduser = int.Parse(tblnews.idUser.ToString());
            var User = db.tblUsers.Find(iduser);
            ViewBag.UserName = User.FullName;
            string chuoinewnew = "";
            var NewsNew = db.tblNews.Where(p => p.Active == true).OrderByDescending(p => p.DateCreate).Take(5).ToList();
            for (int i = 0; i < NewsNew.Count; i++)
            {
                chuoinewnew += "<li><a href=\"/tin-tuc/" + NewsNew[i].Tag + "\" title=\"" + NewsNew[i].Name + "\" rel=\"nofollow\">› " + NewsNew[i].Name + " <span>" + NewsNew[i].DateCreate + "</span></a></li>";
            }
            ViewBag.chuoinewnews = chuoinewnew;

            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> /" + UrlNews(idCates) + " " + tblnews.Name;
            tblConfig tblconfig = db.tblConfigs.First();
            if(tblconfig.Coppy==true)
            {
                ViewBag.Coppy="<link href=\"/Content/Display/Css/Coppy.css\" rel=\"stylesheet\" /><script src=\"/Scripts/disable-copyright.js\"></script>";
            }
            return View(tblnews);
        }
        public ActionResult ListNews(string tag, int? page, string id)
        {
            int idCate = int.Parse(db.tblGroupNews.First(p => p.Tag == tag).id.ToString());
            var listnews = db.tblNews.Where(p => p.idCate == idCate && p.Active == true).OrderByDescending(p => p.Ord).ToList();
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
            // Thiết lập phân trang
            var ship = new PagedListRenderOptions
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                DisplayPageCountAndCurrentLocation = false,
                MaximumPageNumbersToDisplay = 5,
                DisplayEllipsesWhenNotShowingAllPageNumbers = true,
                EllipsesFormat = "&#8230;",
                LinkToFirstPageFormat = "Trang đầu",
                LinkToPreviousPageFormat = "«",
                LinkToIndividualPageFormat = "{0}",
                LinkToNextPageFormat = "»",
                LinkToLastPageFormat = "Trang cuối",
                PageCountAndCurrentLocationFormat = "Page {0} of {1}.",
                ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.",
                FunctionToDisplayEachPageNumber = null,
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                ContainerDivClasses = new[] { "pagination-container" },
                UlElementClasses = new[] { "pagination" },
                LiElementClasses = Enumerable.Empty<string>()
            };
            ViewBag.ship = ship;

            var groupnew = db.tblGroupNews.First(p => p.Tag == tag);
            //int dodai = groupnew.Level.Length / 5;
            //string nUrl = "";
            //for (int i = 0; i < dodai; i++)
            //{
            //    var NameGroups = db.tblGroupNews.First(p => p.Level.Substring(0, (i + 1) * 5) == groupnew.Level.Substring(0, (i + 1) * 5) && p.Level.Length == (i + 1) * 5);
            //    nUrl = nUrl + " <a href=\"/2/" + NameGroups.Tag + "\" title=\"" + NameGroups.Name + "\"> " + " " + NameGroups.Name + "</a> /";
            //}
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> /" + UrlNews(idCate);
            ViewBag.Title = "<title>" + groupnew.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + groupnew.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + groupnew.Keyword + "\" /> ";
            ViewBag.Name = groupnew.Name;
            return View(listnews.ToPagedList(pageNumber, pageSize));
         }
        public  PartialViewResult PartialRightNews()
        {
            return PartialView();
        }

    }
}