using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;

namespace TOTO.Controllers.Display.Header
{
    public class HeaderController : Controller
    {
        private TOTOContext db = new TOTOContext();
        // GET: Header
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ControlHeader()
        {
            return PartialView();
        }
        public PartialViewResult PartialSearch()
        {
            return PartialView();
        }
        public PartialViewResult Partialsidebar()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            var listMenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID==null).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            for (int i = 0; i < listMenu.Count; i++)
            {
                string tag = listMenu[i].Tag;

                chuoi += " <li class=\"li1\">";
               
                    chuoi += " <a href=\"/" + listMenu[i].Tag + ".html\" title=\"" + listMenu[i].Name + "\">› " + listMenu[i].Name + "</a>";
               
                int idCate = listMenu[i].id;
                var listMenu1 = db.tblGroupProducts.Where(p => p.ParentID==idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listMenu1.Count > 0)
                {
                    chuoi += "<ul>";
                    for (int j = 0; j < listMenu1.Count; j++)
                    {
                        chuoi += "<li><a href=\"/" + listMenu1[j].Tag + ".html\" title=\"" + listMenu1[j].Name + "\">" + listMenu1[j].Name + "</a></li>";
                    }
                    chuoi += "</ul>";
                }


                chuoi += "</li>";
            }
            ViewBag.chuoi = chuoi;
            return PartialView(tblconfig);
        }
        public PartialViewResult PartialBanner()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            return PartialView(tblconfig);
        }
        public PartialViewResult ParitalMenuWidth()
        {
            var MenuParent = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID==null).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            string nStyle = "";
            for (int i = 0; i < MenuParent.Count; i++)
            {
                if(i>3)
                   nStyle= "style=\"right:0px\"";

                chuoi += " <li class=\"li1\">";
                string tag = MenuParent[i].Tag;
           

                chuoi += " <li class=\"li1\">";
                

                    chuoi += " <a href=\"/" + MenuParent[i].Tag + ".html\" title=\"" + MenuParent[i].Name + "\">" + MenuParent[i].Name + "</a>";
                 
                int idCate = MenuParent[i].id;
                var listMenu = db.tblGroupProducts.Where(p => p.ParentID==idCate && p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listMenu.Count > 0)
                {
                    
                    chuoi += "<ul class=\"ul2\" "+nStyle+">";
                    for (int j = 0; j < listMenu.Count; j++)
                    {
                        chuoi += "<li class=\"li2\">";
                      
                        chuoi += "<a class=\"image\" href=\"/" + listMenu[j].Tag + ".html\" title=\"\"><img src=\"" + listMenu[j].Images + "\" alt=\"" + listMenu[j].Name + "\" /></a>";
                        chuoi += "<div class=\"Line\"></div>";
                        chuoi += "<a href=\"/" + listMenu[j].Tag + ".html\" title=\"" + listMenu[j].Name + "\">" + listMenu[j].Name + "</a>";
                   
                        chuoi += "</li>";
                    }
                    chuoi += "</ul>";
                }
                chuoi += "</li>";

            }
            ViewBag.chuoi = chuoi;
                return PartialView();
        }
        public PartialViewResult PatialHeader()
        {
            var listimageslide = db.tblImages.Where(p => p.Active == true && p.idCate == 1).OrderByDescending(p => p.Ord).Take(4).ToList();
            string chuoislide = "";
            for (int i = 0; i < listimageslide.Count; i++)
            {
                if (i == 0)
                {
                    chuoislide += "url(" + listimageslide[i].Images + ") " + (770 * i) + "px 0 no-repeat";
                }
                else
                {

                    chuoislide += ", url(" + listimageslide[i].Images + ") " + (770 * i) + "px 0 no-repeat";
                }
            }
            ViewBag.chuoislide = chuoislide;
            var listnew = db.tblNews.Where(p => p.Active == true && p.ViewHomes == true).OrderByDescending(p => p.DateCreate).Take(2).ToList();
            string chuoi = "";
            for (int i = 0; i < listnew.Count;i++ )
            { 
                chuoi += "<a href=\"/tin-tuc/" + listnew[i].Tag + "\" title=\"" + listnew[i].Name + "\">- " + listnew[i].Name + "</a>";

            }
            ViewBag.chuoi = chuoi;
                return PartialView(listimageslide);
        }
    }
}