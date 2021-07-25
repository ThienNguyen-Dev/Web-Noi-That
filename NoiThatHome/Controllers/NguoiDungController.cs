using NoiThatHome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoiThatHome.Controllers
{
    public class NguoiDungController : Controller
    {
        dbQLNoiThatDataContext db = new dbQLNoiThatDataContext();
        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangky(FormCollection data, KHACHHANG kh)
        {

            var hoten = data["HoTen"];
            var taikhoan = data["Taikhoan"];
            var matkhau = data["Matkhau"];
            var nhaplaimatkhau = data["Nhaplaimatkhau"];
            var email = data["Email"];
            var dienthoai = data["Dienthoai"];
            var diachi = data["Diachi"];

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Vui lòng nhập họ và tên ";
            }
            else if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi2"] = "Vui lòng nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Vui lòng nhập mật khẩu  ";
            }
            else if (nhaplaimatkhau != matkhau)
            {
                ViewData["Loi4"] = "Mật khẩu không trùng khớp ";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "Vui lòng nhập Email  ";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi6"] = "Vui lòng nhập điện thoại  ";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Vui lòng nhập địa chỉ  ";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = taikhoan;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DienthoaiKH = dienthoai;
                kh.Diachi = diachi;
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();

                return RedirectToAction("Dangnhap");
            }

            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection data)
        {
            var taikhoan = data["Taikhoan"];
            var matkhau = data["Matkhau"];
            if (String.IsNullOrEmpty(taikhoan))
            {
                ViewData["Loi1"] = "Vui lòng nhập tài khoản ";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu ";
            }

            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == taikhoan && n.Matkhau == matkhau);
                if (kh != null)
                {
                    //ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["TaiKhoan2"] = kh.HoTen;
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Trangchu", "NoiThat");
                }
                else
                    ViewBag.Thongbao = "Tài khoản hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Trangchu", "NoiThat");
        }
        [HttpGet]
        public ActionResult Quenmatkhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Quenmatkhau(FormCollection collection)
        {
            var taikhoan = collection["Taikhoan"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Nhaplaimatkhau"];

            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan.Trim() == taikhoan.Trim());

            if (kh != null)
            {
                if (kh.DienthoaiKH.Trim() == dienthoai && kh.Email.Trim() == email)
                {
                    if (matkhau != nhaplaimatkhau)
                    {
                        ViewBag.Thongbao = "Nhập lại mật khẩu không đúng";
                    }
                    else
                    if (matkhau == nhaplaimatkhau)
                    {
                        kh.Taikhoan = taikhoan;
                        kh.Matkhau = matkhau;
                        kh.Email = email;
                        kh.DienthoaiKH = dienthoai;
                        db.SubmitChanges();
                        return RedirectToAction("Dangnhap");
                    }
                }
                ViewBag.Thongbao = "Nhập thông tin sai vui lòng nhập lại";
            }
            else
            {
                ViewBag.Thongbao = "Nhập thông tin không đúng";
            }
            
            return this.Dangnhap();
        }
        
    }
}