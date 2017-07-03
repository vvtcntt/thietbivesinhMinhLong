using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
namespace TOTO.Controllers.Display.Session.Baogia
{
    public class BaogiaController : Controller
    {
        //
        // GET: /Baogia/
         private TOTOContext db = new TOTOContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BaogiaDetail(string tag, int? page)
        {
            var baogia = db.tblBaogias.First(p => p.Tag == tag);
            int idbg = int.Parse(baogia.id.ToString()); 
            var ListProduct = (from a in db.tblConnectBaogias join b in db.tblProducts on a.idCate equals b.idCate where a.idBG == idbg && b.Active==true select b).OrderBy(p=>p.idCate).ToList();
            const int pageSize = 20;
            var pageNumber = (page ?? 1);
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
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> /" + baogia.Name;
             ViewBag.Title = "<title>" + baogia.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + baogia.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + baogia.Keyword + "\" /> ";
            ViewBag.Name = baogia.Name;
            ViewBag.Des = baogia.Description;
            ViewBag.Date = "<span class=\"date1\">Hà Nội, ngày " + baogia.DateCreate.Value.Day + " tháng " + baogia.DateCreate.Value.Month + " năm " + baogia.DateCreate.Value.Year + "</span>";

            var ListProducts = ListProduct.ToPagedList(pageNumber, pageSize).ToList();
            string chuoi = "";
            int dem = 0;
            for (int i = 0; i < ListProducts.Count;i++ )
            {
                int idcate = int.Parse(ListProducts[i].idCate.ToString());
                string url = db.tblGroupProducts.First(p => p.id == idcate).Tag;
                chuoi += "<tr><td class=\"Ords\">" + i + "</td><td class=\"Names\"><h2><a href=\"/" + url + "" + ListProducts[i].Tag + "_" + ListProducts[i].id + ".html\" title=\"" + ListProducts[i].Name + "\">" + ListProducts[i].Name + "</a> </h2></td><td class=\"Codes\">" + ListProducts[i].Code + "</td><td class=\"Prices\">" + string.Format("{0:#,#}", ListProducts[i].PriceSale) + "đ</td><td class=\"Qualitys\">01</td><td class=\"SumPrices\">" + string.Format("{0:#,#}", ListProducts[i].PriceSale) + "đ</td> <td class=\"Images\"><a href=\"/" + url + "" + ListProducts[i].Tag + "_" + ListProducts[i].id + ".html\" title=\"" + ListProducts[i].Name + "\"><img src=\"" + ListProducts[i].ImageLinkThumb + "\" alt=\"" + ListProducts[i].Name + "\" title=\"" + ListProducts[i].Name + "\"></a></td></tr>";
             }
            ViewBag.chuoi = chuoi;
            var tblcongif = db.tblConfigs.First();
            string config = "";
            config += "<span class=\"hd1\">" + tblcongif.Name + "</span>";
            config += "<span class=\"hd2\">" + tblcongif.Address + "</span>";
            config += "<span class=\"hd3\">Điện thoại:" + tblcongif.MobileIN + "   - Hotline : " + tblcongif.HotlineIN + "</span>";
            config += "<span class=\"hd4\">Email : " + tblcongif.Email + " - website: Thietbivesinhinax.vn</span>";
            ViewBag.chuoiconfig = config;
            ViewBag.Content = baogia.Content;
            ViewBag.Logo = "<div id=\"Left_Head\" style=\"background:url("+tblcongif.Logo+") no-repeat\"></div>"; 
            return View(ListProduct.ToPagedList(pageNumber, pageSize));
        }
    }
}
