using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiThatHome.Models;

namespace NoiThatHome.Controllers
{
    public class GiohangController : Controller
    {
        dbQLNoiThatDataContext data = new dbQLNoiThatDataContext();
        private List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang==null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        // GET: Giohang
        public ActionResult Themgiohang(int id,string strURL )
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang noithat = lstGiohang.Find(n => n.iMaNoiThat == id);
            if(noithat == null)
            {
                noithat = new Giohang(id);
                lstGiohang.Add(noithat);
                return Redirect(strURL);
            }   
            else
            {
                noithat.iSoluong++;
                return Redirect(strURL);
            }    
        }
        private int TongSoLuong()
        {
            int iTongSoluong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang != null)
            {
                iTongSoluong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoluong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        //hien thi gio hang
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
                return RedirectToAction("Sanpham", "NoiThat");
            else
            {
                ViewBag.Tongsoluong = TongSoLuong();
                ViewBag.Tongtien = TongTien();
                return View(lstGiohang);
            }
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            return PartialView();
        }
        public ActionResult XoagioHang(int id)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang noithat = lstGiohang.SingleOrDefault(n => n.iMaNoiThat == id);
            if (noithat != null)
            {
                lstGiohang.RemoveAll(n => n.iMaNoiThat == id);
                return RedirectToAction("GioHang");
            }    
            if(lstGiohang.Count == 0)
            {
                return RedirectToAction("Sanpham","NoiThat");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult Xoatatca()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Sanpham", "NoiThat");
        }
        public ActionResult Capnhat(int id, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang noithat = lstGiohang.SingleOrDefault(n => n.iMaNoiThat == id);
            if(noithat != null)
            {
                noithat.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
                return RedirectToAction("Dangnhap", "NguoiDung");
            if (Session["GioHang"] == null)
                return RedirectToAction("Trangchu", "NoiThat");
            else
            {
                ViewBag.Tongtien = TongTien();
                ViewBag.Tongsoluong = TongSoLuong();
                List<Giohang> lstGiohang = Laygiohang();
                return View(lstGiohang);
            }    
        }
        [HttpPost]
        public ActionResult Dathang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();

            ddh.MaKH = kh.MaKH;
            
            //ddh.TenNN = collection["Nguoinhan"];
            //ddh.DTGiao = collection["DTNguoiNhan"];
            
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:dd/MM/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach(var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaNoiThat = item.iMaNoiThat;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDonggia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");
        }
        public ActionResult Xacnhandonhang ()
        {
            return View();
        }
       
    }
}