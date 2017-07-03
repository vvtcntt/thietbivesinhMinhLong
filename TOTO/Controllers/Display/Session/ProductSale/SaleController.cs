using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;
using PagedList;
using PagedList.Mvc;
using System.Globalization;
using System.Text;
namespace TOTO.Controllers.Display.Session.ProductSale
{
    public class SaleController : Controller
    {
        //
        // GET: /Sale/
        private TOTOContext db = new TOTOContext();
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
        public ActionResult Index(string tag)
        {
            var ProductSale = db.tblProductSales.First(p => p.Tag == tag);
            DateTime thoigian1 = DateTime.Parse(ProductSale.StartSale.ToString());
            DateTime thoigian2 = DateTime.Parse(ProductSale.StopSale.ToString());
            TimeSpan conlai = thoigian2 - thoigian1;
            TimeSpan kiemtra = thoigian2 - DateTime.Now;
            if (kiemtra.TotalDays > -1)
            {
                if (conlai.TotalDays < 0)
                {
                    ViewBag.ngay = "<span style=\"margin-top:30px;display:block;font-weight:bold; font-size:15px\"> Đã hết khuyến mại</span> ";
                }
                else
                {
                    if (conlai.TotalDays == 0)
                    { ViewBag.ngay = "<span>Chỉ còn <span class=\"blink\"> 1 </span> ngày</span>"; }
                    else
                        ViewBag.ngay = "<span>Chỉ còn <span class=\"blink\">" +Convert.ToInt32(kiemtra.TotalDays) + "</span> ngày</span>";
                }
            }
            else
            {
                ViewBag.ngay = "<span style=\"margin-top:30px;display:block;font-weight:bold; font-size:15px\"> Đã hết khuyến mại</span> ";

            }


            ViewBag.Title = "<title>" + ProductSale.Name + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + ProductSale.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + ProductSale.Name + "\" /> ";
            //Load sản phẩm hot khuyến mại
            string chuoihot = ProductSale.CodeOne;
            if (chuoihot != null)
            {
                StringBuilder hienthihot = new StringBuilder();
                string[] manghot = chuoihot.Split(',');
                if (manghot.Length > 0)
                {
                    for (int i = 0; i < manghot.Length; i++)
                    {
                        string code = manghot[i];
                        var listProduct = db.tblProducts.Where(p => p.Code == code).Take(1).ToList();
                        if (listProduct.Count > 0)
                        {
                            int idcate = int.Parse(listProduct[0].idCate.ToString());
                            var listgroup = db.tblGroupProducts.Find(idcate);
                            string url = listgroup.Tag;
                            hienthihot.Append("<div class=\"Tear_top\">");
                            hienthihot.Append("<a href=\"/"+ listProduct[0].Tag + "-dt\" class=\"Name\" title=\"" + listProduct[0].Name + "\">" + listProduct[0].Name + "</a>");
                            hienthihot.Append("<span class=\"model\">Model: " + listProduct[0].Code + " | Giá NY : " + string.Format("{0:#,#}", listProduct[0].Price) + " đ</span>");
                            if (listProduct[0].Note == true)
                            {
                                hienthihot.Append("<div class=\"Soluongcohan\"></div>");
                            }
                            hienthihot.Append("<div class=\"Box_Price\">");
                            int price;
                            string pricesale = (listProduct[0].Price - listProduct[0].PriceSale).ToString();
                            if (pricesale.Length > 3)
                                price = int.Parse(pricesale.Substring(0, pricesale.Length - 3));
                            else
                                price = int.Parse(pricesale);

                            hienthihot.Append("<span>" + string.Format("{0:#,#}", price) + "K</span>");
                            hienthihot.Append("</div>");
                            hienthihot.Append("<div class=\"Box_Sale\">");
                            hienthihot.Append("<div class=\"Left\" style=\"background:url(" + listProduct[0].ImageSale + ") no-repeat left center scroll transparent; background-size:100%\"></div>");
                            hienthihot.Append("<div class=\"Right\"><span>" + listProduct[0].Sale + "</span></div>");
                            hienthihot.Append("</div>");
                            hienthihot.Append("<div class=\"img\">");
                            hienthihot.Append("<a href=\"/"+ listProduct[0].Tag + "-dt\" title=\"" + listProduct[0].Name + "\">");
                            hienthihot.Append("<img src=\"" + listProduct[0].ImageLinkThumb + "\" alt=\"" + listProduct[0].Name + "\" />");
                            hienthihot.Append("</a>");
                            hienthihot.Append("</div>");
                            hienthihot.Append("</div>");
                        }
                    }
                }
                ViewBag.sanphamhot = hienthihot;
            }

            //Load sản phẩm khuyến mại bên dưới
            StringBuilder chuoimenu = new StringBuilder(); StringBuilder chuoiProduct = new StringBuilder() ;

            string CodeTrue = ProductSale.CodeTrue;
            if (CodeTrue != null)
            {


                string[] chuoicodetrue = CodeTrue.Split(',');
                List<int> arrayMenu = new List<int>();
                //List<int> arrayMenuParent = new List<int>();
                List<int> ArrayProduct = new List<int>();
                for (int i = 0; i < chuoicodetrue.Length; i++)
                {
                    string code = chuoicodetrue[i];
                    var listProduct = db.tblProducts.Where(p => p.Code == code).Take(1).ToList();
                    if (listProduct.Count > 0)
                    {
                        int idMenu = int.Parse(listProduct[0].idCate.ToString());
                        arrayMenu.Add(idMenu);
                        ArrayProduct.Add(listProduct[0].id);

                    }
                }

                //var listMenuProduct = db.tblGroupProducts.Where(x => arrayMenu.Contains(x.id) && x.Active == true).ToList();
                //for (int i = 0; i < listMenuProduct.Count; i++)
                //{
                //    int idcate1 = listMenuProduct[i].id;
                //    string level = listMenuProduct[i].Level;
                //    var listMenuParent = db.tblGroupProducts.First(p => p.Level.Substring(0, 5) == level.Substring(0, 5) && p.Level.Length == 5);

                //    arrayMenuParent.Add(listMenuParent.id);

                //}
                var listMenudisplay = db.tblGroupProducts.Where(p => arrayMenu.Contains(p.id)&& p.ParentID==null && p.Active == true).OrderBy(p => p.Ord).ToList();
                if (listMenudisplay.Count > 0)
                {
                    ViewBag.Slogan = " <div id=\"No_Sale\">span>" + ProductSale.Slogan + "</span> </div>";
                    chuoimenu.Append("<div id=\"Menu\">");
                    chuoimenu.Append( "<div id=\"Top_Menu\">");
                    chuoimenu.Append( "<div id=\"Left\">");
                    chuoimenu.Append( "<span>Danh mục khuyến mại</span>");
                    chuoimenu.Append( "</div>");
                    chuoimenu.Append( "<div id=\"Right\">");
                    chuoimenu.Append( "<ul>");

                    for (int i = 0; i < listMenudisplay.Count; i++)
                    {
                        chuoimenu.Append( "  <li><a href=\"#" + listMenudisplay[i].id + "\" title=\"" + listMenudisplay[i].Name + "\">" + listMenudisplay[i].Name + "</a></li>");
                        //string level = listMenudisplay[i].Level;
                        //var listmenu1 = db.tblGroupProducts.Where(p => p.Level.Substring(0, 5) == level && arrayMenu.Contains(p.Id)).ToList();
                        chuoiProduct.Append("<div class=\"Banner1\">");
                        chuoiProduct.Append("<div class=\"Left\" style=\"background:url(" + listMenudisplay[i].Images + ") no-repeat center center scroll #fff\"></div>");
                        chuoiProduct.Append("<div class=\"Right\"></div>");
                        chuoiProduct.Append("<div class=\"Center\">");
                        chuoiProduct.Append("<span>Danh sách " + listMenudisplay[i].Name + " đang được khuyến mại </span>");
                        chuoiProduct.Append("</div>");
                        chuoiProduct.Append("</div>");
                        chuoiProduct.Append("<div class=\"Content_Top2\" id=\"" + listMenudisplay[i].id + "\">");
                        chuoiProduct.Append("<div class=\"Left\">");
                        int idimage = int.Parse(listMenudisplay[i].id.ToString());
                        var listImage = db.tblImages.Where(p => p.idCate == 3 && p.idCate == idimage && p.Active == true).OrderBy(p => p.Ord).ToList();
                        for (int m = 0; m < listImage.Count; m++)
                        {
                            chuoiProduct.Append("<a href=\"" + listImage[m].Url + "\" title=\"" + listImage[m].Name + "\"><img src=\"" + listImage[m].Images + "\" alt=\"" + listImage[m].Name + "\" /></a>");
                        }
                        chuoiProduct.Append("</div>");
                        chuoiProduct.Append("<div class=\"Right\">");
                        //for (int j = 0; j < listmenu1.Count; j++)
                        //{
                        //    string Url = listmenu1[j].Tag;
                        //    int idcate = int.Parse(listmenu1[j].id.ToString());
                        var listproductdisplay = db.tblProducts.Where(p => ArrayProduct.Contains(p.id) && p.idCate == idimage && p.Active == true).OrderBy(p => p.Ord).ToList();
                            for (int k = 0; k < listproductdisplay.Count; k++)
                            {
                                chuoiProduct.Append("<div class=\"Tear_pro\">");
                                chuoiProduct.Append("<a href=\"/"+ listproductdisplay[k].Tag + "-dt\" title=\"" + listproductdisplay[k].Name + "\" class=\"Name\">" + listproductdisplay[k].Name + "</a>");
                                chuoiProduct.Append("<span class=\"model\">Model: " + listproductdisplay[k].Code + " | Giá NY : " + string.Format("{0:#,#}", listproductdisplay[k].Price) + " đ</span>");
                                if (listproductdisplay[k].Note == true)
                                {
                                    chuoiProduct.Append("<div class=\"Soluongcohan\"></div>");
                                }
                                chuoiProduct.Append("<div class=\"Box_Price\">");
                                string pricesale = (listproductdisplay[k].Price - listproductdisplay[k].PriceSale).ToString();
                                int price;
                                if (pricesale.Length > 3)
                                {
                                    price = int.Parse(pricesale.Substring(0, pricesale.Length - 3));
                                }
                                else
                                {
                                    price = int.Parse(pricesale);
                                }

                                chuoiProduct.Append("<span>" + string.Format("{0:#,#}", price) + "k</span>");
                                chuoiProduct.Append("</div>");
                                chuoiProduct.Append("<div class=\"Box_Sale\">");
                                chuoiProduct.Append("<div class=\"Left\" style=\"background:url(" + listproductdisplay[k].ImageSale + ") no-repeat left center scroll transparent; background-size:100%\"></div>");
                                chuoiProduct.Append("<div class=\"Right\"><span>" + listproductdisplay[k].Sale + "</span></div>");
                                chuoiProduct.Append("</div>");
                                chuoiProduct.Append("<div class=\"img\">");
                                chuoiProduct.Append("<a href=\"/" +listproductdisplay[k].Tag + "-dt\" title=\"" + listproductdisplay[k].Name + "\">");
                                chuoiProduct.Append("<img src=\"" + listproductdisplay[k].ImageLinkThumb + "\" alt=\"" + listproductdisplay[k].Name + "\" />");
                                chuoiProduct.Append("</a>");
                                chuoiProduct.Append("</div>");
                                chuoiProduct.Append("</div>");
                            }

                        //}
                        chuoiProduct.Append("</div>");
                        chuoiProduct.Append("</div>");
                        chuoiProduct.Append("<div class=\"Clear\"></div>");
                    }

                    chuoimenu.Append( "</ul>");
                    chuoimenu.Append( "</div>");
                    chuoimenu.Append( "</div>");
                    chuoimenu.Append( "<div id=\"Bottom_Menu\"></div>");
                    chuoimenu.Append( " </div>");
                }

                ViewBag.chuoimenu = chuoimenu;
                ViewBag.chuoiProduct = chuoiProduct;

            }
            //danh sách sản phẩm đồng bộ
            StringBuilder hienthidongbo = new StringBuilder();
            string chuoidongbo = ProductSale.CodeSale;
            if (chuoidongbo != null)
            {
                string[] mangdongbo = chuoidongbo.Split(',');
                for (int i = 0; i < mangdongbo.Length; i++)
                {
                    string codedongbo = mangdongbo[i];
                    var listProductSyn = db.tblProductSyns.Where(p => p.Code == codedongbo && p.Active == true).OrderBy(p => p.Ord).ToList();
                    if (listProductSyn.Count > 0)
                    {
                        hienthidongbo.Append("<div class=\"spdb\">");
                        hienthidongbo.Append("<div class=\"sptb\"></div>");
                        hienthidongbo.Append("<div class=\"Box_Price\">");
                        hienthidongbo.Append("<span>" + string.Format("{0:#,#}", listProductSyn[0].PriceSale) + " <span>đ</span></span>");
                        hienthidongbo.Append("</div>");
                        hienthidongbo.Append("<div class=\"img_spdb\">");
                        hienthidongbo.Append("<a href=\"/Syn/" + listProductSyn[0].Tag + "\" title=\"" + listProductSyn[0].Name + "\"><img src=\"" + listProductSyn[0].ImageLinkThumb + "\" alt=\"" + listProductSyn[0].Name + "\" /></a>");
                        hienthidongbo.Append("</div>");
                        hienthidongbo.Append("<a href=\"/Syn/" + listProductSyn[0].Tag + "\" class=\"Name\" title=\"" + listProductSyn[0].Name + "\">" + listProductSyn[0].Name + "</a>");
                        hienthidongbo.Append("<span class=\"Note\">" + listProductSyn[0].Description + "</span>");
                        hienthidongbo.Append("</div>");
                    }
                }

            }
            ViewBag.hienthichuoi = hienthidongbo;
            return View(ProductSale);
        }
        public ActionResult ListSale(int? page, string id)
        {
            ViewBag.Title = "<title> Cập nhật chương trình khuyến mại Thiết bị vệ sinh Inax chính hãng</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Sau đây là những Cập nhật chương trình khuyến mại Thiết bị vệ sinh Inax chính hãng do trung tâm phân phối sản phẩm inax gửi tới quý khách hàng\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"khuyên mại inax, thiết bị vệ sinh inax khuyến mại\" /> ";
            var listnews = db.tblProductSales.Where(p => p.Active == true).OrderByDescending(p => p.DateCreate).ToList();
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
            return View(listnews.ToPagedList(pageNumber, pageSize));

        }
    }
}
