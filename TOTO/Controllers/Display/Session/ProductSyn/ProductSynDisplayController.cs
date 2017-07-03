﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TOTO.Models;
namespace TOTO.Controllers.Display.Session.ProductSyn
{
    public class ProductSynDisplayController : Controller
    {
        TOTOContext db = new TOTOContext();
        //
        // GET: /ProductSynDisplay/
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult ListProductSyn()
        {
            string chuoi = "";
            var listProductSyn = db.tblProductSyns.Where(p => p.Active == true && p.ViewHomes == true).OrderBy(p => p.Ord).ToList();
             
                chuoi += "<div id=\"ProductSYN\">";
                if (listProductSyn.Count > 0)
                {
                    chuoi += "<div id=\"nVar_ProductSYN\">";
                    chuoi += "<span> Sản phẩm đồng bộ</span>";
                    chuoi += "</div>";
                }
                chuoi += "<div class=\"Adw_Syn\">";
                var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 2).OrderBy(p => p.Ord).ToList();
                for (int i = 0; i < listImage.Count; i++)
                {
                    chuoi += "<a href=\"/" + listImage[i].Url + "\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\" alt=\"" + listImage[i].Name + "\"/></a>";

                }
                chuoi += " </div>";
                if (listProductSyn.Count > 0)
                {
                    chuoi += "<div id=\"Content_ProductSYN\">";

                    for (int i = 0; i < listProductSyn.Count; i++)
                    {
                        chuoi += " <div class=\"spdb\">";
                        chuoi += " <div class=\"sptb\"></div>";

                        chuoi += " <div class=\"img_spdb\">";
                        chuoi += " <a href=\"/Syn/" + listProductSyn[i].Tag + "\" title=\"" + listProductSyn[i].Name + "\"><img src=\"" + listProductSyn[i].ImageLinkThumb + "\" alt=\"" + listProductSyn[i].Name + "\" /></a>";
                        chuoi += " </div>";
                        chuoi += " <a href=\"/Syn/" + listProductSyn[i].Tag + "\" class=\"Name\" title=\"" + listProductSyn[i].Name + "\">" + listProductSyn[i].Name + "</a>";
                        chuoi += "<div class=\"Bottom_Tear_Sale\">";
                        chuoi += "<div class=\"Price\">";
                        if (listProductSyn[i].PriceSale>2)
                        {chuoi += "<p class=\"PriceSale\">" + string.Format("{0:#,#}", listProductSyn[i].PriceSale) + " <span>đ</span></p>";}
                        else
                        {chuoi += "<p class=\"PriceSale\"> Liên hệ</p>";}
                        if (listProductSyn[i].Price > 2)
                        { chuoi += "<p class=\"Price_s\">" + string.Format("{0:#,#}", listProductSyn[i].Price) + "đ</p>"; }
                        else
                        { chuoi += "<p class=\"Price_s\">Liên hệ</p>"; }
                        chuoi += "</div>";
                        chuoi += "<div class=\"sevices\">";
                        if (listProductSyn[i].Status == true)
                        {
                            chuoi += "<span class=\"Status\"></span>";
                        }
                        else
                        {
                            chuoi += "<span class=\"StatusNo\"></span>";
                        }

                        chuoi += "<span class=\"Transport\"><span class=\"icon\">";
                        if (listProductSyn[i].Transport == true)
                        {
                            chuoi += "</span> Toàn quốc</span>";
                        }
                        else
                        {
                            chuoi += "</span> Liên hệ</span>";
                        }
                        chuoi += "<span class=\"View\"></span>";
                        chuoi += "</div>";
                        chuoi += "</div>";

                        chuoi += "  </div>";
                    }
                    chuoi += "</div>";
                }
                chuoi += "</div>";
           
            ViewBag.chuoisyc = chuoi;
            return PartialView();
        }
     
        public ActionResult ProductSyn_Detail(string tag)
        {
            var tblproductSyn = db.tblProductSyns.First(p => p.Tag == tag);
            int id = int.Parse(tblproductSyn.id.ToString());
            string chuoi = "Khách hàng vui lòng kích vào chi tiết từng sản phẩm ở trên để xem thông thông số kỹ thuật !";
            ViewBag.Title = "<title>" + tblproductSyn.Title + "</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"" + tblproductSyn.Description + "\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"" + tblproductSyn.Keyword + "\" /> ";
            //Load Images Liên Quan
            var listImage = db.tblImageProducts.Where(p => p.idProduct == id && p.Type==1).ToList();
            string chuoiimages = "";
            for (int i = 0; i < listImage.Count; i++)
            {
                chuoiimages += " <li class=\"Tear_pl\"><a href=\"" + listImage[i].Images + "\" rel=\"prettyPhoto[gallery1]\" title=\"" + listImage[i].Name + "\"><img src=\"" + listImage[i].Images + "\"   alt=\"" + listImage[i].Name + "\" /></a></li>";
            }
            ViewBag.chuoiimage = chuoiimages;
            int idsyn = int.Parse(tblproductSyn.id.ToString());
            int visit = int.Parse(tblproductSyn.Visit.ToString());
            if (visit > 0)
            {
                tblproductSyn.Visit = tblproductSyn.Visit + 1;
                db.SaveChanges();
            }
            else
            {
                tblproductSyn.Visit = tblproductSyn.Visit + 1;
                db.SaveChanges();
            }
            var Product = db.ProductConnects.Where(p => p.idSyn == idsyn).ToList();
            string chuoipr = "";
            string chuoisosanh = "";
            float tonggia = 0;
            if (Product.Count > 0)
            {
                chuoipr += "<div id=\"Content_spdb\">";
                chuoipr += "<span class=\"tinhnang\">&diams; Danh sách sản phẩm có trong " + tblproductSyn.Name + "</span>";
                chuoisosanh += "<div id=\"equa\">";
                chuoisosanh += "<div class=\"nVar_Equa\"><span>Bảng so sánh giá mua lẻ và mua theo bộ</span></div>";
                chuoisosanh += "<div class=\"Clear\"></div>";
                chuoisosanh += "<table width=\"200\" border=\"1\">";
                chuoisosanh += "<tr style=\"color:#333; text-transform:uppercase; line-height:25px; text-align:center\">";
                chuoisosanh += "<td style=\"width:5%;text-align:center\">STT</td>";
                chuoisosanh += "<td style=\"width:40%\">Tên Sản phẩm</td>";
                chuoisosanh += "<td style=\"width:10%;text-align:center\">Số lượng</td>";
                chuoisosanh += "<td style=\"width:20%;text-align:center\">Đơn Giá</td>";
                chuoisosanh += "<td style=\"text-align:center; width:20%\">Thành Tiền</td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "</div>";
                for (int i = 0; i < Product.Count; i++)
                {
                    string codepd = Product[i].idpd;

                    var Productdetail = db.tblProducts.Where(p => p.Code == codepd && p.Active == true).Take(1).ToList();
                    if (Productdetail.Count > 0)
                    {
                        int idCate = int.Parse(Productdetail[0].idCate.ToString());
                        var ListGroup = db.tblGroupProducts.Find(idCate);
                        chuoipr += "<div class=\"Tear_syn\">";
                        chuoipr += "<div class=\"img_syn\">";
                        chuoipr += "<div class=\"nvar_Syn\">";
                        chuoipr += "<h2><a href=\"/" + ListGroup.Tag + "" + Productdetail[0].Tag + "_" + Productdetail[0].id + ".html\" title=\"" + Productdetail[0].Name + "\">" + Productdetail[0].Name + "</a></h2>";
                        chuoipr += "</div>";
                        chuoipr += "<div class=\"img_syn\">";
                        chuoipr += "<a href=\"/" + ListGroup.Tag + "" + Productdetail[0].Tag + "_" + Productdetail[0].id + ".html\" title=\"" + Productdetail[0].Name + "\"><img src=\"" + Productdetail[0].ImageLinkThumb + "\" alt=\"" + Productdetail[0].Name + "\" /></a>";
                        chuoipr += "</div>";
                        chuoipr += "</div>";
                        chuoipr += "</div>";
                        chuoisosanh += "<tr style=\"line-height:20px\">";
                        chuoisosanh += "<td style=\"width:5%;text-align:center\">" + (i + 1) + "</td>";
                        chuoisosanh += "<td style=\"width:40%; text-indent:7px\">" + Productdetail[0].Name + "</td>";
                        chuoisosanh += "<td style=\"width:10%;text-align:center\"> 1 </td>";
                        chuoisosanh += "<td style=\"width:20%;text-align:center\">" + string.Format("{0:#,#}", Productdetail[0].PriceSale) + "</td>";
                        chuoisosanh += "<td style=\"text-align:center; width:20%\">" + string.Format("{0:#,#}", Productdetail[0].PriceSale) + "</td>";
                        chuoisosanh += " </tr>";
                        tonggia = tonggia + float.Parse(Productdetail[0].PriceSale.ToString());
                    }

                }
                chuoipr += "</div>";
                chuoisosanh += "<tr style=\"line-height:25px \">";
                chuoisosanh += "<td colspan=\"4\"><span style=\"text-align:center; margin-right:5px; font-weight:bold; display:block\">TỔNG GIÁ MUA LẺ</span></td>";
                chuoisosanh += "<td style=\"font-weight:bold; font-size:16px; text-align:center\">" + string.Format("{0:#,#}", Convert.ToInt32(tonggia)) + " đ</td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "<tr>";
                int sodu = Convert.ToInt32(tonggia) - int.Parse(tblproductSyn.PriceSale.ToString());

                chuoisosanh += "<td colspan=\"4\"><span style=\"text-align:center; margin-right:5px; font-weight:bold; display:block; color:#ff5400\">GIÁ MUA THEO BỘ</span></td>";
                chuoisosanh += "<td style=\"font-weight:bold; color:#ff5400; font-size:18px; font-family:UTMSwiss; text-align:center\">" + string.Format("{0:#,#}", tblproductSyn.PriceSale) + "đ<span style=\"font-style:italic; color:#333; font-size:12px; font-family:Arial, Helvetica, sans-serif; margin:5px; display:block; font-weight:normal\">Bạn đã tiết kiệm : " + string.Format("{0:#,#}", sodu) + "đ khi mua theo bộ</span></td>";
                chuoisosanh += "</tr>";
                chuoisosanh += "</table>";
            }

            ViewBag.chuoi = chuoi;
            ViewBag.chuoisosanh = chuoisosanh;
            ViewBag.chuoipr = chuoipr;
            return View(tblproductSyn);
        }
        public PartialViewResult RightProductSyn(string tag)
        {
            var tblproductSyn = db.tblProductSyns.First(p => p.Tag == tag);
            string chuoisupport = "";
            var listSupport = db.tblSupports.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            for (int i = 0; i < listSupport.Count; i++)
            {
                chuoisupport += "<div class=\"Line_Buttom\"></div>";
                chuoisupport += "<div class=\"Tear_Supports\">";
                chuoisupport += "<div class=\"Left_Tear_Support\">";
                chuoisupport += "<span class=\"htv1\">" + listSupport[i].Mission + ":</span>";
                chuoisupport += "<span class=\"htv2\">" + listSupport[i].Name + " :</span>";
                chuoisupport += "</div>";
                chuoisupport += "<div class=\"Right_Tear_Support\">";
                chuoisupport += "<a href=\"ymsgr:sendim?" + listSupport[i].Yahoo + "\">";
                chuoisupport += "<img src=\"http://opi.yahoo.com/online?u=" + listSupport[i].Yahoo + "&m=g&t=1\" alt=\"Yahoo\" class=\"imgYahoo\" />";
                chuoisupport += " </a>";
                chuoisupport += "<a href=\"Skype:" + listSupport[i].Skyper + "?chat\">";
                chuoisupport += "<img class=\"imgSkype\" src=\"/Content/Display/iCon/skype-icon.png\" title=\"Kangaroo\" alt=\"" + listSupport[i].Name + "\">";
                chuoisupport += "</a>";
                chuoisupport += "</div>";
                chuoisupport += "</div>";
            }
            ViewBag.chuoisupport = chuoisupport;


            //Load sản phẩm liên quan
            string chuoiproduct = "";
            var listProductSyn = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).Take(7).ToList();
            for (int i = 0; i < listProductSyn.Count; i++)
            {

                

             
                chuoiproduct += " <div class=\"spdb\">";
                chuoiproduct += " <div class=\"sptb\"></div>";

                chuoiproduct += " <div class=\"img_spdb\">";
                chuoiproduct += " <a href=\"/Syn/" + listProductSyn[i].Tag + "\" title=\"" + listProductSyn[i].Name + "\"><img src=\"" + listProductSyn[i].ImageLinkThumb + "\" alt=\"" + listProductSyn[i].Name + "\" /></a>";
                chuoiproduct += " </div>";
                chuoiproduct += " <a href=\"/Syn/" + listProductSyn[i].Tag + "\" class=\"Name\" title=\"" + listProductSyn[i].Name + "\">" + listProductSyn[i].Name + "</a>";
                chuoiproduct += "<div class=\"Bottom_Tear_Sale\">";
                chuoiproduct += "<div class=\"Price\">";
                chuoiproduct += "<p class=\"PriceSale\">" + string.Format("{0:#,#}", listProductSyn[i].PriceSale) + " <span>đ</span></p>";
                chuoiproduct += "<p class=\"Price_s\">" + string.Format("{0:#,#}", listProductSyn[i].Price) + "</p>";
                chuoiproduct += "</div>";
                chuoiproduct += "<div class=\"sevices\">";
                if (listProductSyn[i].Status == true)
                {
                    chuoiproduct += "<span class=\"Status\"></span>";
                }
                else
                {
                    chuoiproduct += "<span class=\"StatusNo\"></span>";
                }

                chuoiproduct += "<span class=\"Transport\"><span class=\"icon\">";
                if (listProductSyn[i].Transport == true)
                {
                    chuoiproduct += "</span> Toàn quốc</span>";
                }
                else
                {
                    chuoiproduct += "</span> Liên hệ</span>";
                }
                chuoiproduct += "<span class=\"View\"></span>";
                chuoiproduct += "</div>";
                chuoiproduct += "</div>";

                chuoiproduct += "  </div>";
            }
            ViewBag.chuoiproduct = chuoiproduct;
            tblConfig tblconfig = db.tblConfigs.First();
            return PartialView(tblconfig);

        }
        public ActionResult Hienthidongbo()
        {
            ViewBag.Title = "<title> Danh sách sản phẩm TOTO đồng bộ - Khuyến mại lớn !</title>";
            ViewBag.Description = "<meta name=\"description\" content=\"Danh sách những sản phẩm TOTO đồng bộ áp dụng khuyến mại dành cho khách hàng khi mua thiết bị vệ sinh TOTO\"/>";
            ViewBag.Keyword = "<meta name=\"keywords\" content=\"Sản phẩm TOTO đồng bộ, thiết bị vệ sinh TOTO đồng bộ\" /> ";
            var listsanphamdongbo = db.tblProductSyns.Where(p => p.Active == true).OrderBy(p => p.Ord).ToList();
            var listImage = db.tblImages.Where(p => p.Active == true && p.idCate == 2).ToList();
            string chuoi = "";
            for (int i = 0; i < listImage.Count; i++)
            {
                chuoi += "<a href=\"" + listImage[i].Url + "\" title=\"" + listImage[i].Name + "\" rel=\"" + listImage[i].Link + "\"><img src=\"" + listImage[i].Images + "\" alt=\"" + listImage[i].Name + "\" /></a>";
            }
            ViewBag.hienthianh = chuoi;
            return View(listsanphamdongbo);
        }
	}
}