using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;
namespace TOTO.Controllers.Display.Session.Product
{
    public class ProductController : Controller
    {
        // GET: Product
        private TOTOContext db = new TOTOContext();
        public ActionResult Index()
        {
            return View();
        }
        List<string> Mangphantu = new List<string>();
        public List<string> Arrayid(int idParent)
        {

            var ListMenu = db.tblGroupProducts.Where(p => p.ParentID == idParent).ToList();

            for (int i = 0; i < ListMenu.Count; i++)
            {
                Mangphantu.Add(ListMenu[i].id.ToString());
                int id = int.Parse(ListMenu[i].id.ToString());
                Arrayid(id);

            }

            return Mangphantu;
        }
        string nUrl = "";
        public string UrlProduct(int idCate)
        {
            var ListMenu = db.tblGroupProducts.Where(p => p.id == idCate).ToList();
            for (int i = 0; i < ListMenu.Count; i++)
            {
                nUrl = " <a href=\"/" + ListMenu[i].Tag + ".html\" title=\"" + ListMenu[i].Name + "\"> " + " " + ListMenu[i].Name + "</a> <i></i>" + nUrl;
                string ids = ListMenu[i].ParentID.ToString();
                if (ids != null && ids != "")
                {
                    int id = int.Parse(ListMenu[i].ParentID.ToString());
                    UrlProduct(id);
                }
            }
            return nUrl;
        }
        public PartialViewResult PartialProductSaleHomes()
        {
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.ProductSale == true).OrderBy(p => p.Ord).ToList();
            StringBuilder chuoi = new StringBuilder();
            if(listProduct.Count>0)
            { 
            for (int i = 0; i < listProduct.Count; i++)
            {
                int idcates = int.Parse(listProduct[i].idCate.ToString());

                string Url = db.tblGroupProducts.First(p => p.id == idcates).Tag;
                chuoi.Append("<div class=\"Tear_Sale\">");
                chuoi.Append(" <div class=\"Box_Sale\" style=\"background:url(" + listProduct[i].ImageSale + ") no-repeat\"></div>");
                chuoi.Append("<div class=\"img\">");
                chuoi.Append(" <a href=\"/" + listProduct[i].Tag + "-pd\" title=\"" + listProduct[i].Name + "\"><img src=\"" + listProduct[i].ImageLinkThumb + "\" alt=\"" + listProduct[i].Name + "\" title=\"" + listProduct[i].Name + "\"/></a>");
                chuoi.Append("</div>");
                chuoi.Append("<h3><a href=\"/" + listProduct[i].Tag + "-pd\" title=\"" + listProduct[i].Name + "\" class=\"Name\">" + listProduct[i].Name + "</a></h3>");
                chuoi.Append("<div class=\"Info\">");
                chuoi.Append("<div class=\"Left_Info\">");
                chuoi.Append("<div class=\"Top_Left_Info\">");
                if (listProduct[i].Status == true)
                { chuoi.Append("<span class=\"Status\"></span>"); }
                else
                { chuoi.Append("<span class=\"Status1\"></span>"); }
                 chuoi.Append("</div>");
                chuoi.Append("<div class=\"Buttom_Left_Info\">");
                //load tính năng
            
                int id = int.Parse(listProduct[i].id.ToString());
                var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == id).ToList();
                if (checkfun.Count > 0)
                {
                    for (int j = 0; j < listfuc.Count; j++)
                    {
                        int idfun = int.Parse(listfuc[j].id.ToString());
                        var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == id).ToList();
                        if (connectfun.Count > 0)
                        {
                            chuoi.Append("<a href=\"" + listfuc[j].Url + "\" rel=\"nofollow\" title=\"" + listfuc[j].Name + "\"><img src=\"" + listfuc[j].Images + "\" alt=\"" + listfuc[j].Name + "\" /></a>");
                        }
                    }

                }
                chuoi.Append("</div>");
                chuoi.Append("</div>");
                chuoi.Append("<div class=\"Right_Info\">");
                chuoi.Append("<span class=\"Pricesale\">" + string.Format("{0:#,#}", listProduct[i].PriceSale) + "<span>đ</span></span>");
                chuoi.Append("<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[i].Price) + "đ</span>");
                chuoi.Append("</div>");
                chuoi.Append("</div>");
                chuoi.Append("</div>");

            }
            }
            ViewBag.chuoi = chuoi;
            return PartialView();
        }
        public PartialViewResult PartialProductHomes()
        {
            StringBuilder chuoi = new StringBuilder();
            var MenuParent = db.tblGroupProducts.Where(p => p.Active == true && p.Priority == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < MenuParent.Count; i++)
            {
                chuoi.Append("<div class=\"ProductHomes\">");
                chuoi.Append("<div class=\"nVar\">");
                chuoi.Append("<div class=\"Left_nVar\">");
                chuoi.Append("<div class=\"Name\">");
                chuoi.Append("<span class=\"iCon\" style=\"background:url(" + MenuParent[i].iCon + ") no-repeat\"></span>");
                chuoi.Append("<h2><a href=\"/" + MenuParent[i].Tag + ".html\" title=\"" + MenuParent[i].Name + "\">" + MenuParent[i].Name + "</a></h2>");
                chuoi.Append("</div>");
                chuoi.Append("<div class=\"iCon_nVar\">");
                chuoi.Append("<span>T" + (i + 1) + "</span>");
                chuoi.Append("</div>");
                chuoi.Append("</div>");
                chuoi.Append("<div class=\"Right_nVar\">");
                int idCate = MenuParent[i].id;
                var Menuchild = db.tblGroupProducts.Where(p =>p.ParentID==idCate && p.Active == true).OrderBy(p => p.Ord).Take(4).ToList();
                if (Menuchild.Count > 0)
                {
                    chuoi.Append("<ul class=\"ul_1\">");
                    for (int j = 0; j < Menuchild.Count; j++)
                    {
                        string tag = Menuchild[j].Tag;
                       
                        chuoi.Append("<li class=\"li_1\">");

                        chuoi.Append(" <h2><a href=\"/" + tag + ".html\" title=\"" + Menuchild[j].Name + "\"> " + Menuchild[j].Name + "<span></span></a></h2>");
                        int idCate1 = Menuchild[j].id;
                        var Menuchild1 = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID==idCate1).OrderBy(p => p.Ord).ToList();
                        if (Menuchild1.Count > 0)
                        {
                            chuoi.Append("<ul class=\"ul_2\">");
                            for (int k = 0; k < Menuchild1.Count; k++)
                            {
                                chuoi.Append("<li class=\"li_2\">");
                                chuoi.Append("<a href=\"/" + Menuchild1[k].Tag + ".html\" title=\"" + Menuchild1[k].Name + "\">› " + Menuchild1[k].Name + "</a>");
                                chuoi.Append("</li>");
                            }
                            chuoi.Append("</ul>");
                        }

                        chuoi.Append(" </li>");
                    }
                    chuoi.Append("  </ul>");
                }
                
                chuoi.Append("<a href=\"/" + MenuParent[i].Tag + ".html\" title=\"Xem thêm sản phẩm\" class=\"Xemchitiet\">Xem thêm &raquo;</a>");
                chuoi.Append("</div>");
                chuoi.Append("</div>");

                chuoi.Append(" <div class=\"bg_ProductHomes\">");
                chuoi.Append("<div class=\"Content_ProductHomes\">");
                chuoi.Append("<div class=\"Left_Content_ProductHomes\">");
                int idmenu = int.Parse(MenuParent[i].id.ToString());
                var listImagesconnect = db.tblConnectImages.Where(p => p.idCate == idmenu).ToList();
                List<int> MangImage = new List<int>();
                foreach(var item in listImagesconnect)
                {
                    int idm=int.Parse(item.idImg.ToString());
                    MangImage.Add(idm);
                }
                var listImages = db.tblImages.Where(p =>MangImage.Contains(p.id) && p.Active == true).OrderBy(p => p.Ord).ToList();
                for (int x = 0; x < listImages.Count; x++)
                {
                    chuoi.Append(" <a href=\"" + listImages[x].Url + "\" title=\"" + listImages[x].Name + "\" rel=\"nofollow\">");
                    chuoi.Append("<img src=\"" + listImages[x].Images + "\" alt=\"" + listImages[x].Name + "\" title=\"" + listImages[x].Name + "\"/>");
                    chuoi.Append("</a>");
                }
                chuoi.Append("</div>");
                chuoi.Append("<div class=\"Right_Content_ProductHomes\">");

                List<string> Mang = new List<string>();
                Mang = Arrayid(idCate);
                Mang.Add(idCate.ToString());
                var listProduct = db.tblProducts.Where(p => p.Active == true && Mang.Contains(p.idCate.ToString()) && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
                    for (int y = 0; y < listProduct.Count; y++)
                    {
                        chuoi.Append("<div class=\"Tear_1\">");
                        chuoi.Append("<div class=\"img\">");
                        chuoi.Append("<a href=\"/" + listProduct[y].Tag + "-pd\" title=\"" + listProduct[y].Name + "\">");
                        chuoi.Append("<img src=\"" + listProduct[y].ImageLinkThumb + "\" alt=\"" + listProduct[y].Name + "\" title=\"" + listProduct[y].Name + "\" />");
                        chuoi.Append("</a>");
                        chuoi.Append("</div>");
                        chuoi.Append("<h3><a href=\"/" + listProduct[y].Tag + "-pd\" title=\"" + listProduct[y].Name + "\" class=\"Name\">" + listProduct[y].Name + "</a></h3>");
                        chuoi.Append("<div class=\"Info\">");

                        chuoi.Append(" <div class=\"LeftInfo\">");
                        if (listProduct[y].PriceSale<10)
                        { chuoi.Append("<span class=\"PriceSale\">Liên hệ</span>"); }
                        else
                        chuoi.Append("<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[y].PriceSale) + "đ</span>");
                        if (listProduct[y].Price<10)
                        { chuoi.Append("<span class=\"Price\">Liên hệ</span>"); }
                        else
                        chuoi.Append("<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[y].Price) + "đ</span>");
                        chuoi.Append("</div>");
                        chuoi.Append("<div class=\"RightInfo\">");
                        chuoi.Append("<div class=\"Top_RightInfo\">");
                        chuoi.Append("<a href=\"/Order/OrderIndex?idp=" + listProduct[y].id + "\" title=\"Đặt hàng\" rel=\"nofollow\"><span></span></a>");
                        chuoi.Append("</div>");
                        chuoi.Append("<div class=\"Bottom_RightInfo\">");
                        int id = int.Parse(listProduct[y].id.ToString());
                        var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                        var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == id).ToList();
                        if (checkfun.Count > 0)
                        {
                            for (int j = 0; j < listfuc.Count; j++)
                            {
                                int idfun = int.Parse(listfuc[j].id.ToString());
                                var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == id).ToList();
                                if (connectfun.Count > 0)
                                {
                                    chuoi.Append("<a href=\"" + listfuc[j].Url + "\" rel=\"nofollow\" title=\"" + listfuc[j].Name + "\"><img src=\"" + listfuc[j].Images + "\" alt=\"" + listfuc[j].Name + "\" title=\"" + listfuc[j].Name + "\"/></a>");
                                }
                            }

                        }

                        chuoi.Append("</div>");
                        chuoi.Append("</div>");
                        chuoi.Append("</div>");
                        chuoi.Append("</div>");
                    }
               
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                    Mangphantu.Clear();
                }
            
            ViewBag.chuoi = chuoi;
            return PartialView();
        }
        public ActionResult ProductDetail(string tag,string idp)
        {
            string url = Request.Url.ToString();
            string[] mang = url.Split('/');
            if (mang.Length > 4)
            {
                return Redirect("/" + tag + "-pd");
            }
            if (idp != null && idp != "")
            {
                int nid = int.Parse(idp);
                var Products = db.tblProducts.FirstOrDefault(p => p.id == nid);
                return Redirect("/" + Products.Tag + "-pd");
            }
            tblProduct Product = db.tblProducts.First(p=>p.Tag==tag);
            int visit = int.Parse(Product.Visit.ToString());
            if (visit > 0)
            {
                Product.Visit = Product.Visit + 1;
                db.SaveChanges();
            }
            else
            {
                Product.Visit = Product.Visit + 1;
                db.SaveChanges();
            } 
            int id = Product.id;

            ViewBag.Title = "<title>" + Product.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + Product.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + Product.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + Product.Keyword + "\" /> ";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"/" + Product.Tag+ "-pd\" />";
            meta += "<meta itemprop=\"name\" content=\"" + Product.Title + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + Product.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"\" />";
            meta += "<meta property=\"og:title\" content=\"" + Product.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"\" " + Product.ImageLinkThumb + "/>";
            meta += "<meta property=\"og:site_name\" content=\"http://thietbivesinhminhlong.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + Product.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
            //Load Images Liên Quan
            var listImage = db.tblImageProducts.Where(p => p.idProduct == id).ToList();
            string chuoiimages = "";
            for (int i = 0; i < listImage.Count; i++)
            {
                chuoiimages += " <li class=\"Tear_pl\"><a href=\"" + listImage[i].Images + "\" rel=\"prettyPhoto[gallery1]\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\"  title=\"" + listImage[i].Name + "\"  alt=\"" + listImage[i].Name + "\" /></a></li>";
            }
            ViewBag.chuoiimage = chuoiimages;

            int idMenu = int.Parse(Product.idCate.ToString());
            ViewBag.Nhomsp = db.tblGroupProducts.First(p => p.id == idMenu).Name;
            string code = Product.Code;
            //Load sản phẩm đổng bộ
            var ProductSyn = db.ProductConnects.Where(p => p.idpd == code).ToList();
            List<int> exceptionList = new List<int>();
            for (int i = 0; i < ProductSyn.Count; i++)
            {
                exceptionList.Add(int.Parse(ProductSyn[i].idSyn.ToString()));
            }
            StringBuilder chuoisym = new StringBuilder();
            var listSyn = db.tblProductSyns.Where(x => exceptionList.Contains(x.id)).ToList();
            if (listSyn.Count > 0)
            {
                chuoisym.Append("<div id=\"Top7\">");
               chuoisym.Append("<div id=\"iCon\"></div>");
               chuoisym.Append("<div id=\"Content_Top7\"><p>Hiện tại sản phẩm <span>" + Product.Name + "</span> có giá rẻ hơn khi mua sản phẩm đồng bộ, bạn có thể xem sản phẩm đồng bộ này</p>");
               chuoisym.Append("<ul>");
                for (int i = 0; i < listSyn.Count; i++)
                {
                   chuoisym.Append("<li><a href=\"/Syn/" + listSyn[i].Tag + "\" title=\"" + listSyn[i].Name + "\" class=\"Syn\" rel=\"nofollow\"><span></span> " + listSyn[i].Name + "</a></li>");
                }
               chuoisym.Append("</ul>");
               chuoisym.Append(" </div>");
               chuoisym.Append("</div>");
            }
            ViewBag.chuoisym = chuoisym;
            
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> /" + UrlProduct(idMenu);
            // Load màu sản phẩm
            StringBuilder chuoicolor = new StringBuilder();

            var listcolor = db.tblColorProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            var kiemtracolor = db.tblConnectColorProducts.Where(p => p.idPro == id).ToList();
            if (kiemtracolor.Count > 0)
            {

                chuoicolor.Append("<div id=\"Top4\">");
                chuoicolor.Append("<span> Màu sản phẩm : </span>");
                chuoicolor.Append(" <div id=\"Left_Top4\">");
                for (int i = 0; i < listcolor.Count; i++)
                {
                    int idcolor = int.Parse(listcolor[i].id.ToString());
                    var listconnectcolor = db.tblConnectColorProducts.Where(p => p.idPro == id && p.idColor == idcolor).ToList();
                    if (listconnectcolor.Count > 0)
                    {

                        chuoicolor.Append("<span class=\"Mau\" style=\"background:url(" + listcolor[i].Images + ") no-repeat; background-size:100%\"></span> ");
                    }

                }
                chuoicolor.Append("</div>");
                chuoicolor.Append("</div>");
            }

            ViewBag.color = chuoicolor;
            //load tính năng
            StringBuilder chuoifun = new StringBuilder();
            var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == id).ToList();
            if (checkfun.Count > 0)
            {

                chuoifun.Append(" <div id=\"Tech\">");
                chuoifun.Append("<span class=\"tinhnang\">Những tính năng nổi bật của " + Product.Name + "</span>");
                for (int i = 0; i < listfuc.Count; i++)
                {
                    int idfun = int.Parse(listfuc[i].id.ToString());
                    var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == id).ToList();
                    if (connectfun.Count > 0)
                    {
                        chuoifun.Append("<div class=\"Tear_tech\">");
                        chuoifun.Append("<span class=\"imagetech\" style=\"background:url(" + listfuc[i].Images + ") no-repeat center center scroll transparent;\"></span>");
                        chuoifun.Append("<span class=\"Destech\">" + listfuc[i].Name + "</span>");
                        chuoifun.Append("<p>Xem chi tiết về " + listfuc[i].Name + " <a href=\"" + listfuc[i].Url + "\" title=\"" + listfuc[i].Name + "\">Tại đây &raquo;</a></p>");
                        chuoifun.Append("</div>");
                    }
                }
                chuoifun.Append("</div>");

            }
            ViewBag.chuoifun = chuoifun;
            //Load file kỹ thuật

            var filesbaogia = db.tblFiles.Where(p => p.idp == id & p.Cate == 1).Take(1).ToList();
            string files="";
            if(filesbaogia.Count>0)
            {
                files += "<object src=\"" + filesbaogia[0].File + "\"><embed src=\"" + filesbaogia[0].File + "\"></embed></object>";
                ViewBag.thongso = files;
            }
            return View(Product);
        }
        public PartialViewResult PartialRightProductDetail(string tag)
        {
         
            tblProduct Product = db.tblProducts.First(p=>p.Tag==tag);
            tblConfig tblconfig = db.tblConfigs.First();
            StringBuilder chuoisupport = new StringBuilder();
            var listSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listSupport.Count; i++)
            {
                chuoisupport.Append("<div class=\"Line_Buttom\"></div>");
                chuoisupport.Append("<div class=\"Tear_Supports\">");
                chuoisupport.Append("<div class=\"Left_Tear_Support\">");
                chuoisupport.Append("<span class=\"htv1\">" + listSupport[i].Mission + ":</span>");
                chuoisupport.Append("<span class=\"htv2\">" + listSupport[i].Name + " :</span>");
                chuoisupport.Append("</div>");
                chuoisupport.Append("<div class=\"Right_Tear_Support\">");
                chuoisupport.Append("<a href=\"ymsgr:sendim?" + listSupport[i].Yahoo + "\">");
                chuoisupport.Append("<img src=\"http://opi.yahoo.com/online?u=" + listSupport[i].Yahoo + "&m=g&t=1\" alt=\"Yahoo\" class=\"imgYahoo\" />");
                chuoisupport.Append(" </a>");
                chuoisupport.Append("<a href=\"Skype:" + listSupport[i].Skyper + "?chat\">");
                chuoisupport.Append("<img class=\"imgSkype\" src=\"/Content/Display/iCon/skype-icon.png\" title=\"" + listSupport[i].Name + "\" alt=\"" + listSupport[i].Name + "\">");
                chuoisupport.Append("</a>");
                chuoisupport.Append("</div>");
                chuoisupport.Append("</div>");
            }
            ViewBag.chuoisupport = chuoisupport;

            //lIST Menu
            int idCate = int.Parse(Product.idCate.ToString());
            tblGroupProduct grouproduct = db.tblGroupProducts.Find(idCate);
            int idParent = int.Parse(grouproduct.ParentID.ToString());
             string chuoimenu = "";
            var listGroupProduct = db.tblGroupProducts.Where(p => p.ParentID==idParent && p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listGroupProduct.Count; i++)
            {

                chuoimenu += "<h2><a href=\"/" + listGroupProduct[i].Tag + ".html\" title=\"\">› " + listGroupProduct[i].Name + "</a><h2>";

            }
            ViewBag.chuoimenu = chuoimenu;
            //Load sản phẩm liên quan
            string Url = grouproduct.Tag;
            StringBuilder chuoiproduct = new StringBuilder();
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.idCate == idCate).OrderByDescending(p => p.Visit).OrderBy(p => p.Ord).Take(5).ToList();
            for (int i = 0; i < listProduct.Count; i++)
            {

                chuoiproduct.Append(" <div class=\"Tear_1\">");
                chuoiproduct.Append("<div class=\"img\">");
                chuoiproduct.Append("<a href=\"/" + listProduct[i].Tag + "-pd\" title=\"" + listProduct[i].Name + "\">");
                chuoiproduct.Append("<img src=\"" + listProduct[i].ImageLinkThumb + "\" alt=\"" + listProduct[i].Name + "\" />");
                chuoiproduct.Append("</a>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("<h3><a href=\"/" + listProduct[i].Tag + "-pd\" title=\"" + listProduct[i].Name + "\" class=\"Name\">" + listProduct[i].Name + "</a><h3>");
                chuoiproduct.Append("<div class=\"Info\">");
                chuoiproduct.Append("<div class=\"LeftInfo\">");
                if (listProduct[i].PriceSale<10)
                chuoiproduct.Append("<span class=\"PriceSale\">Liên hệ</span>");
                else
                    chuoiproduct.Append("<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[i].PriceSale) + "đ</span>");
                if (listProduct[i].Price < 10)
                chuoiproduct.Append("<span class=\"Price\">Liên hệ</span>");
                else
                    chuoiproduct.Append("<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[i].Price) + "đ</span>");
                chuoiproduct.Append(" </div>");
                chuoiproduct.Append("<div class=\"RightInfo\">");
                chuoiproduct.Append("<div class=\"Top_RightInfo\">");
                chuoiproduct.Append("<a href=\"/Order/OrderIndex?idp=" + listProduct[i].id + "\" title=\"" + listProduct[i].Name + "\" rel=\"nofollow\"><span></span></a>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("<div class=\"Bottom_RightInfo\">");
                int ids = int.Parse(listProduct[i].id.ToString());
                var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                if (checkfun.Count > 0)
                {
                    for (int j = 0; j < listfuc.Count; j++)
                    {
                        int idfun = int.Parse(listfuc[j].id.ToString());
                        var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                        if (connectfun.Count > 0)
                        {
                            chuoiproduct.Append("<a href=\"" + listfuc[j].Url + "\" rel=\"nofollow\" title=\"" + listfuc[j].Name + "\"><img src=\"" + listfuc[j].Images + "\" alt=\"" + listfuc[j].Name + "\" /></a>");
                        }
                    }

                }
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("</div>");
            }
            ViewBag.chuoiproduct = chuoiproduct;
            return PartialView(tblconfig);
         }
        public ActionResult ListProduct(string tag)
        {
            StringBuilder chuoi = new StringBuilder();
            string url = Request.Url.ToString();
            string[] mang = url.Split('/');
            if (mang.Length > 4)
            {
                return Redirect("/" + tag + ".html");
            }
            
            var GroupProduct = db.tblGroupProducts.First(p => p.Tag == tag && p.Active==true);


            ViewBag.Title = "<title>" + GroupProduct.Title + "</title>";
            ViewBag.dcTitle = "<meta name=\"DC.title\" content=\"" + GroupProduct.Title + "\" />";
            ViewBag.Description = "<meta name=\"description\" content=\"" + GroupProduct.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + GroupProduct.Keyword + "\" /> ";
            string meta = "";
            ViewBag.canonical = "<link rel=\"canonical\" href=\"http://thietbivesinhminhlong.vn/" + GroupProduct.Tag + ".html\" />";
            meta += "<meta itemprop=\"name\" content=\"" + GroupProduct.Title + "\" />";
            meta += "<meta itemprop=\"url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta itemprop=\"description\" content=\"" + GroupProduct.Description + "\" />";
            meta += "<meta itemprop=\"image\" content=\"\" />";
            meta += "<meta property=\"og:title\" content=\"" + GroupProduct.Title + "\" />";
            meta += "<meta property=\"og:type\" content=\"product\" />";
            meta += "<meta property=\"og:url\" content=\"" + Request.Url.ToString() + "\" />";
            meta += "<meta property=\"og:image\" content=\"\" " + GroupProduct.Images + "/>";
            meta += "<meta property=\"og:site_name\" content=\"http://thietbivesinhminhlong.vn\" />";
            meta += "<meta property=\"og:description\" content=\"" + GroupProduct.Description + "\" />";
            meta += "<meta property=\"fb:admins\" content=\"\" />";
            ViewBag.Meta = meta; 
            int idCate = GroupProduct.id;
            ViewBag.nUrl = "<a href=\"/\" title=\"Trang chủ\" rel=\"nofollow\"><span class=\"iCon\"></span> Trang chủ</a> /" + UrlProduct(idCate) +"/ <h1>"+GroupProduct.Title+"</h1>";
            var listGroupProduct = db.tblGroupProducts.Where(p => p.Active == true && p.ParentID==idCate).OrderBy(p => p.Ord).ToList();
            if (listGroupProduct.Count > 0)
            {
                for (int i = 0; i < listGroupProduct.Count; i++)
                {
                    chuoi.Append(" <div class=\"List_product\">");
                    chuoi.Append(" <div id=\"Box_Name\">");
                    chuoi.Append("<div id=\"Leff_BoxName\"><h2><a href=\"/" + listGroupProduct[i].Tag + ".html\" title=\"" + listGroupProduct[i].Name + "\">" + listGroupProduct[i].Name + "</a></h2></div>");
                    chuoi.Append("</div>");
                    chuoi.Append("<div class=\"Clear\"></div>");
                    chuoi.Append("<div class=\"ContentProduct\">");
                    int idcate = int.Parse(listGroupProduct[i].id.ToString());
                    List<string> Mang = new List<string>();
                    Mang = Arrayid(idcate);
                    Mang.Add(idcate.ToString());
                    var listProduct = db.tblProducts.Where(p => Mang.Contains(p.idCate.ToString()) && p.Active == true).OrderBy(p => p.Ord).Take(18).ToList();
                    for (int j = 0; j < listProduct.Count; j++)
                    {
                        chuoi.Append("<div class=\"Tear_1\">"); 
                        chuoi.Append("<div class=\"img\">");
                        chuoi.Append("<a href=\"/" + listProduct[j].Tag + "-pd\" title=\"" + listProduct[j].Name + "\">");
                        chuoi.Append("<img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\"/>");
                        chuoi.Append("</a>");
                        chuoi.Append("</div>");
                        chuoi.Append("<h3><a href=\"/" + listProduct[j].Tag + "-pd\" title=\"" + listProduct[j].Name + "\" class=\"Name\">" + listProduct[j].Name + "</a></h3>");
                        chuoi.Append("<div class=\"Info\">");
                        chuoi.Append("<div class=\"LeftInfo\">");
                        if (listProduct[j].PriceSale<10)
                            chuoi.Append("<span class=\"PriceSale\">Liên hệ</span>");
                        else
                        chuoi.Append("<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                        if (listProduct[j].Price < 10)
                        { chuoi.Append("<span class=\"Price\">Liên hệ</span>"); }
                        else
                        chuoi.Append("<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                        chuoi.Append("</div>");
                        chuoi.Append("<div class=\"RightInfo\">");
                        chuoi.Append("<div class=\"Top_RightInfo\">");
                        chuoi.Append("<a href=\"\" title=\"\"><span></span></a>");
                        chuoi.Append("</div>");
                        chuoi.Append(" <div class=\"Bottom_RightInfo\">");
                        int ids = int.Parse(listProduct[j].id.ToString());
                        var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                        var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                        if (checkfun.Count > 0)
                        {
                            for (int z = 0; z < listfuc.Count; z++)
                            {
                                int idfun = int.Parse(listfuc[z].id.ToString());
                                var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                                if (connectfun.Count > 0)
                                {
                                    chuoi.Append("<a href=\"" + listfuc[z].Url + "\" rel=\"nofollow\" title=\"" + listfuc[z].Name + "\"><img src=\"" + listfuc[z].Images + "\" alt=\"" + listfuc[z].Name + "\" /></a>");
                                }
                            }

                        }
                        chuoi.Append("</div>");
                        chuoi.Append("</div>");
                        chuoi.Append("</div>");
                        chuoi.Append("</div>");
                    }
                    chuoi.Append("<div class=\"Clear\"></div>");
                    chuoi.Append("</div>");
                    var listProduct1 = db.tblProducts.Where(p => Mang.Contains(p.idCate.ToString()) && p.Active == true).OrderBy(p => p.Ord).ToList();

                    if (listProduct1.Count > 18)
                    {
                        chuoi.Append("<div class=\"box_xemthem\"><a href=\"/" + listGroupProduct[i].Tag + ".html\" title=\"" + listGroupProduct[i].Name + "\" class=\"Xemthem\" rel=\"nofollow\">Xem tất cả sản phẩm >></a></div>");

                    }
                    chuoi.Append("</div>");
                    Mangphantu.Clear();
                }
            }
            else
            {
                chuoi.Append(" <div class=\"List_product\">");
                chuoi.Append(" <div id=\"Box_Name\">");
                chuoi.Append("<div id=\"Leff_BoxName\">   <h2><a href=\"" + GroupProduct.Tag + ".html\" title=\"" + GroupProduct.Name + "\">" + GroupProduct.Name + "</a></h2></div>");
                 chuoi.Append("<div id=\"Rigt_Box_Name\">");
                         chuoi.Append("<select>");
                             chuoi.Append("<option value=\"0\"> - Sắp xếp -</option>");
                             chuoi.Append("<option value=\"1\"> - Giá tăng dần -</option>");
                             chuoi.Append("<option value=\"1\"> - GIá giảm giần -</option>");
                        chuoi.Append(" </select>");
                        chuoi.Append("</div>");
                chuoi.Append("</div>");
                chuoi.Append("<div class=\"Clear\"></div>");
                chuoi.Append("<div class=\"ContentProduct\">");
                int idcate = int.Parse(GroupProduct.id.ToString());
                var listProduct = db.tblProducts.Where(p => p.idCate == idcate && p.Active == true).OrderBy(p => p.Ord).ToList();
                for (int j = 0; j < listProduct.Count; j++)
                {
                    chuoi.Append("<div class=\"Tear_1\">");
                    chuoi.Append("<div class=\"img\">");
                    chuoi.Append("<a href=\"/" + listProduct[j].Tag + "-pd\" title=\"" + listProduct[j].Name + "\">");
                    chuoi.Append("<img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" title=\"" + listProduct[j].Name + "\" />");
                    chuoi.Append("</a>");
                    chuoi.Append("</div>");
                    chuoi.Append("<h3><a href=\"/" + listProduct[j].Tag + "-pd\" title=\"" + listProduct[j].Name + "\" class=\"Name\">" + listProduct[j].Name + "</a></h3>");
                    chuoi.Append("<div class=\"Info\">");
                    chuoi.Append("<div class=\"LeftInfo\">");
                    if (listProduct[j].PriceSale<10)
                    chuoi.Append("<span class=\"PriceSale\">Liên hệ</span>");
                    else
                    chuoi.Append("<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                    if (listProduct[j].Price<10)
                    chuoi.Append("<span class=\"Price\">Liên hệ</span>");
                    else
                     chuoi.Append("<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");   
                    chuoi.Append("</div>");
                    chuoi.Append("<div class=\"RightInfo\">");
                    chuoi.Append("<div class=\"Top_RightInfo\">");
                    chuoi.Append("<a href=\"\" title=\"\"><span></span></a>");
                    chuoi.Append("</div>");
                    chuoi.Append(" <div class=\"Bottom_RightInfo\">");
                    int ids = int.Parse(listProduct[j].id.ToString());
                    var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                    var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                    if (checkfun.Count > 0)
                    {
                        for (int z = 0; z < listfuc.Count; z++)
                        {
                            int idfun = int.Parse(listfuc[z].id.ToString());
                            var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                            if (connectfun.Count > 0)
                            {
                                chuoi.Append("<a href=\"" + listfuc[z].Url + "\" rel=\"nofollow\" title=\"" + listfuc[z].Name + "\"><img src=\"" + listfuc[z].Images + "\" alt=\"" + listfuc[z].Name + "\" title=\"" + listfuc[z].Name + "\" /></a>");
                            }
                        }

                    }
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                    chuoi.Append("</div>");
                }
                chuoi.Append("<div class=\"Clear\"></div>");
                chuoi.Append("</div>");
                chuoi.Append("</div>");
            }
            ViewBag.chuoi = chuoi;
            //
            StringBuilder catalogis = new StringBuilder();
            int idg=int.Parse(GroupProduct.id.ToString());
            var tblcatalogis = db.tblFiles.Where(p => p.idg == idg).ToList();
            if(tblcatalogis.Count>0)
            { 
            catalogis.Append("<div id=\"Download\">");
           catalogis.Append("<a href=\""+tblcatalogis[0].File+"\" title=\""+tblcatalogis[0].Name+"\"><span></span></a>");
           catalogis.Append("</div>");
            }
            ViewBag.catalogis = catalogis;
            return View(GroupProduct);
        }
        public ActionResult Command(FormCollection collection, string tag)
        {
            if (collection["btnOrder"] != null)
            {

                Session["idProduct"] = collection["idPro"];
                Session["idMenu"] = collection["idCate"];
                Session["OrdProduct"] = collection["txtOrd"];
                Session["Url"] = Request.Url.ToString();
                return RedirectToAction("OrderIndex", "Order");
            }
            return View();
        }
        public ActionResult SearchProduct(string tag)
        {
            StringBuilder chuoi = new StringBuilder();
            ViewBag.Title = "<title> Tìm kiếm : " + tag + "</title>";
            ViewBag.name = tag;
            ViewBag.Description = "<meta name=\"description\" content=\"" + tag + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tag + "\" /> ";
            chuoi.Append("   <div class=\"Name_Cate\">");

            StringBuilder chuoiproduct = new StringBuilder();
            var listProduct = db.tblProducts.Where(p => p.Active == true && p.Name.Contains(tag)).OrderBy(p => p.Ord).ToList();
            for (int j = 0; j < listProduct.Count; j++)
            {
                int idcate = int.Parse(listProduct[j].idCate.ToString());
                string GroupProduct = db.tblGroupProducts.First(p => p.id == idcate).Tag;
                string Url = GroupProduct;
                chuoiproduct.Append(" <div class=\"Tear_1\">");
                chuoiproduct.Append("<div class=\"img\">");
                chuoiproduct.Append("<a href=\"/" + listProduct[j].Tag + "-pd\" title=\"" + listProduct[j].Name + "\">");
                chuoiproduct.Append("<img src=\"" + listProduct[j].ImageLinkThumb + "\" alt=\"" + listProduct[j].Name + "\" />");
                chuoiproduct.Append("</a>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("<a href=\"/" + listProduct[j].Tag + "-pd\" title=\"" + listProduct[j].Name + "\" class=\"Name\">" + listProduct[j].Name + "</a>");
                chuoiproduct.Append("<div class=\"Info\">");
                chuoiproduct.Append("<div class=\"LeftInfo\">");
                if(listProduct[j].PriceSale<10)
                    chuoiproduct.Append("<span class=\"PriceSale\">Liên hệ</span>");
                else
                chuoiproduct.Append("<span class=\"PriceSale\">" + string.Format("{0:#,#}", listProduct[j].PriceSale) + "đ</span>");
                if (listProduct[j].Price<10)
                { 
                chuoiproduct.Append("<span class=\"Price\">Liên hệ</span>");
                } 
                else
                    chuoiproduct.Append("<span class=\"Price\">" + string.Format("{0:#,#}", listProduct[j].Price) + "đ</span>");
                chuoiproduct.Append(" </div>");
                chuoiproduct.Append("<div class=\"RightInfo\">");
                chuoiproduct.Append("<div class=\"Top_RightInfo\">");
                chuoiproduct.Append("<a href=\"/Order/OrderIndex?idp=" + listProduct[j].id + "\" title=\"" + listProduct[j].Name + "\" rel=\"nofollow\"><span></span></a>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("<div class=\"Bottom_RightInfo\">");
                int ids = int.Parse(listProduct[j].id.ToString());
                var listfuc = db.tblFunctionProducts.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
                var checkfun = db.tblConnectFunProuducts.Where(p => p.idPro == ids).ToList();
                if (checkfun.Count > 0)
                {
                    for (int z = 0; z < listfuc.Count; z++)
                    {
                        int idfun = int.Parse(listfuc[z].id.ToString());
                        var connectfun = db.tblConnectFunProuducts.Where(p => p.idFunc == idfun && p.idPro == ids).ToList();
                        if (connectfun.Count > 0)
                        {
                            chuoiproduct.Append("<a href=\"" + listfuc[z].Url + "\" rel=\"nofollow\" title=\"" + listfuc[z].Name + "\"><img src=\"" + listfuc[z].Images + "\" alt=\"" + listfuc[z].Name + "\" /></a>");
                        }
                    }

                }
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("</div>");
                chuoiproduct.Append("</div>");
            }

            ViewBag.chuoisanpham = chuoiproduct;
            return View();
        }
        [HttpPost]
        public ActionResult Search(FormCollection collection)
        {
            string tag = collection["txtSearch"];
            return Redirect("/Search/" + tag + "");
        }
        public string GetSale(int id)
        {
            string chuoi = "";
            var GroupPrice = db.tblGroupPrices.Find(id);
            chuoi = GroupPrice.Sale.ToString();
            return chuoi;
        }
    }
}