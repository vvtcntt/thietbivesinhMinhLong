using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;
namespace TOTO.Controllers.Display.Footter
{
    public class FootterController : Controller
    {
        private TOTOContext db = new TOTOContext();
        // GET: Footter
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ControlFootter()
        {
            tblConfig tblconfig = db.tblConfigs.First();
            var Url = db.tblUrls.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            string chuoi = "";
            for (int i = 0; i < Url.Count;i++ )
            {
                if(Url[i].Rel==true)
                { chuoi += "<a href=\"" + Url[i].Url + "\" title=\"" + Url[i].Name + "\" rel=\"nofollow\">" + Url[i].Name + "</a>"; }
                else
                chuoi += "<a href=\"" + Url[i].Url + "\" title=\"" + Url[i].Name + "\">" + Url[i].Name + "</a>";
            }
            ViewBag.chuoi = chuoi;
            var maps = db.tblMaps.First();
            ViewBag.maps = maps.Content;
            string baogia = "";
            var listBaogia = db.tblBaogias.Where(p => p.Active == true).OrderByDescending(p => p.Active == true).ToList();
             foreach (var item in listBaogia)
            {
                baogia += "<a href=\"/Bao-gia/" + item.Tag + "\" title=\"" + item.Name + "\">" + item.Name + "</a>";
            }
             ViewBag.baogia = baogia;
             var Imagesadw = db.tblImages.Where(p => p.Active == true && p.idCate == 9).OrderByDescending(p => p.Ord).Take(1).ToList();
            if (Imagesadw.Count > 0)
                ViewBag.Chuoiimg = "<a href=\"" + Imagesadw[0].Url + "\" title=\"" + Imagesadw[0].Name + "\"><img src=\"" + Imagesadw[0].Images + "\" alt=\"" + Imagesadw[0].Name + "\" style=\"max-width:100%;\" /> </a>";
            var listPartner = db.tblPartners.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            StringBuilder result = new StringBuilder();
            for(int i=0;i<listPartner.Count;i++)
            {
                result.Append("<span class=\"Phone\"><span class=\"icon\"></span> "+listPartner[i].Name+": <span>"+ listPartner[i].Mobile+ " - "+listPartner[i].Hotline+"</span></span>");
            }
            ViewBag.result = result.ToString();
            return PartialView(tblconfig);
        }
        public ActionResult Command(FormCollection collection, tblRegister registry)
        {
             string Name = collection["txtName"];
                string Hotline = collection["txtHotline"];
                string selectcate = collection["selectcate"];
                registry.Name = Name;
                registry.Mobile = Hotline;
                registry.idCate = int.Parse(selectcate);
                     db.tblRegisters.Add(registry);
                    db.SaveChanges();
                    Session["registry"] = "<script>$(document).ready(function(){ alert('Bạn đã đăng ký thành công') });</script>";
                 
            
            return Redirect("/Default/Index");
        }
        public PartialViewResult MenuMobine()
        {
            var listGroup = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            string chuoimenu = "";
            for (int i = 0; i < listGroup.Count; i++)
            {

                string tag = listGroup[i].Tag;
                chuoimenu += "<a href=\"/" + tag + ".html\" title=\"" + listGroup[i].Name + "\">" + listGroup[i].Name + "</a>";

            }
            ViewBag.chuimenu = chuoimenu;
            string chuoi = "";
            var ListMenu = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID==null).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                chuoi += "<li><a href=\"/" + ListMenu[i].Tag + ".html\" title=\"" + ListMenu[i].Name + "\">" + ListMenu[i].Name + "</a>";
                int idcate = ListMenu[i].id;
                var listmenuchild = db.tblGroupProducts.Where(p => p.ParentID == idcate & p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listmenuchild.Count > 0)
                {
                    chuoi += "<ul>";
                    for (int j = 0; j < listmenuchild.Count; j++)
                    {
                        chuoi += "<li><a href=\"/" + listmenuchild[j].Tag + ".html\" title=\"" + listmenuchild[j].Name + "\">" + listmenuchild[j].Name + "</a></li>";
                    }
                    chuoi += "</ul>";
                }
                chuoi += "</li>";
            }
            ViewBag.chuoi = chuoi;
            return PartialView();
        }
    }
}