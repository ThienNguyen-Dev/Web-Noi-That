using NoiThatHome.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Collections.Generic;

namespace NoiThatHome.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        dbQLNoiThatDataContext db = new dbQLNoiThatDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NOITHAT(int? page)
        {
            //return View(db.NOITHATs.ToList());
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.NOITHATs.ToList().OrderBy(n => n.MaNoiThat).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection data)
        {
            var tendn = data["username"];
            var matkhau = data["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui lòng nhập tài khoản ";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu ";
            }

            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {

                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tài khoản hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themmoinoithat()
        {
            ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoaiSP), "MaLoaiSP", "TenLoaiSP");
            return View();
        }
        [HttpPost]
        public ActionResult Themmoinoithat(NOITHAT noithat, HttpPostedFileBase fileupload)
        {
            //dua du lieu vao dropdownload
            ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong");
            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoaiSP), "MaLoaiSP", "TenLoaiSP");
            // kiem tra duong dan file 
            if (fileupload == null)
            {
                ViewBag.ThongBao = "Vui Lòng Chọn Ảnh Bìa ";
                return View();
            }
            //thêm vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu Tên file , bổ sung thư viện IO
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu Đường Dẫn của File
                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    //Kiểm Tra hình ảnh đã tồn tại chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.ThongBao = "Hình Ảnh Đã Tồn Tại";
                    }
                    else
                    {
                        //Lưu Hình Ảnh vào Đường Dẫn
                        fileupload.SaveAs(path);
                    }
                    noithat.Anhbia = fileName;
                    //Lưu vào CSDL
                    db.NOITHATs.InsertOnSubmit(noithat);
                    db.SubmitChanges();
                }
                return RedirectToAction("NOITHAT", "Admin");
            }
        }

        [HttpGet]
        public ActionResult XoaNoiThat(int id)
        {
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNoiThat == id);
            ViewBag.MaNoiThat = noithat.MaNoiThat;
            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(noithat);

        }
        [HttpPost, ActionName("XoaNoiThat")]
        public ActionResult XacNhanXoa(int id)
        {
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNoiThat == id);
            ViewBag.MaNoiThat = noithat.MaNoiThat;
            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NOITHATs.DeleteOnSubmit(noithat);
            db.SubmitChanges();
            return RedirectToAction("NoiThat");
        }
        public ActionResult CTNT(int id)
        {
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNoiThat == id);
            ViewBag.MaNoiThat = noithat.MaNoiThat;
            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(noithat);
        }

        [HttpGet]
        public ActionResult SuaNoiThat(int id)
        {
            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNoiThat == id);

            if (noithat == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoaiSP), "MaLoaiSP", "TenLoaiSP", noithat.MaLoaiSP);
            ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong", noithat.MaLoaiPhong);
            return View(noithat);
        }

        [HttpPost, ActionName("SuaNoiThat")]


        public ActionResult XacNhanSua(int id, HttpPostedFileBase fileUpload)
        {

            NOITHAT noithat = db.NOITHATs.SingleOrDefault(n => n.MaNoiThat == id);

            ViewBag.MaLoaiSP = new SelectList(db.LOAISPs.ToList().OrderBy(n => n.TenLoaiSP), "MaLoaiSP", "TenLoaiSP", noithat.MaLoaiSP);
            ViewBag.MaLoaiPhong = new SelectList(db.LOAIPHONGs.ToList().OrderBy(n => n.TenLoaiPhong), "MaLoaiPhong", "TenLoaiPhong", noithat.MaLoaiPhong);
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "chọn ảnh";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);

                    var path = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "ảnh đã có";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    noithat.Anhbia = fileName;

                    UpdateModel(noithat);
                    db.SubmitChanges();
                }
                return RedirectToAction("NOITHAT", "Admin");
            }




        }


        //Loại SP
        public ActionResult LoaiSP()
        {
            return View(db.LOAISPs.ToList());

        }
        [HttpGet]
        public ActionResult XoaLoaiNoiThat(int id)
        {
            LOAISP lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoaiSP == id);
            ViewBag.MaLoaiSP = lsp.MaLoaiSP;
            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lsp);

        }
        [HttpPost, ActionName("XoaLoaiNoiThat")]
        public ActionResult XacNhanXoaLSP(int id)
        {
            LOAISP lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoaiSP == id);
            ViewBag.MaLoaiSP = lsp.MaLoaiSP;
            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LOAISPs.DeleteOnSubmit(lsp);
            db.SubmitChanges();
            return RedirectToAction("LoaiSP");
        }
        public ActionResult CTLSP(int id)
        {
            LOAISP lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoaiSP == id);
            ViewBag.MaLoaiSP = lsp.MaLoaiSP;
            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lsp);
        }
        public ActionResult ThemLoaiSP()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemLoaiSP(LOAISP lsp)
        {
            db.LOAISPs.InsertOnSubmit(lsp);
            db.SubmitChanges();
            return RedirectToAction("LoaiSP", "Admin");
        }
        [HttpGet]
        public ActionResult SuaLoaiSP(int id)
        {
            LOAISP lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoaiSP == id);

            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lsp);
        }
        [HttpPost, ActionName("SuaLoaiSP")]

        public ActionResult xacnhansua(int id)
        {
            LOAISP lsp = db.LOAISPs.SingleOrDefault(n => n.MaLoaiSP == id);

            UpdateModel(lsp);
            db.SubmitChanges();

            return RedirectToAction("LoaiSP", "Admin");
        }




        //Loại Phòng
        public ActionResult LoaiPhong()
        {
            return View(db.LOAIPHONGs.ToList());

        }
        [HttpGet]
        public ActionResult XoaLoaiPhong(int id)
        {
            LOAIPHONG lp = db.LOAIPHONGs.SingleOrDefault(n => n.MaLoaiPhong == id);
            ViewBag.MaLoaiPhong = lp.MaLoaiPhong;
            if (lp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lp);

        }
        [HttpPost, ActionName("XoaLoaiPhong")]
        public ActionResult XacNhanXoaPhong(int id)
        {
            LOAIPHONG lp = db.LOAIPHONGs.SingleOrDefault(n => n.MaLoaiPhong == id);
            ViewBag.MaLoaiPhong = lp.MaLoaiPhong;
            if (lp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LOAIPHONGs.DeleteOnSubmit(lp);
            db.SubmitChanges();
            return RedirectToAction("LoaiPhong");
        }
        public ActionResult CTLP(int id)
        {
            LOAIPHONG lp = db.LOAIPHONGs.SingleOrDefault(n => n.MaLoaiPhong == id);
            ViewBag.MaLoaiPhong = lp.MaLoaiPhong;
            if (lp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lp);
        }
        public ActionResult ThemLoaiPhong()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemLoaiPhong(LOAIPHONG lp)
        {
            db.LOAIPHONGs.InsertOnSubmit(lp);
            db.SubmitChanges();
            return RedirectToAction("LoaiPhong", "Admin");
        }
        [HttpGet]
        public ActionResult SuaLoaiPhong(int id)
        {
            LOAIPHONG lp = db.LOAIPHONGs.SingleOrDefault(n => n.MaLoaiPhong == id);

            if (lp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lp);
        }
        [HttpPost, ActionName("SuaLoaiPhong")]

        public ActionResult xacnhansuaphong(int id)
        {
            LOAIPHONG lp = db.LOAIPHONGs.SingleOrDefault(n => n.MaLoaiPhong == id);

            UpdateModel(lp);
            db.SubmitChanges();

            return RedirectToAction("LoaiPhong", "Admin");
        }


        //Khách Hàng
        public ActionResult KhachHang()
        {
            return View(db.KHACHHANGs.ToList());

        }
        [HttpGet]
        public ActionResult XoaKhachHang(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);

        }
        [HttpPost, ActionName("XoaKhachHang")]
        public ActionResult XacNhanXoaKhachHang(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHACHHANGs.DeleteOnSubmit(kh);
            db.SubmitChanges();
            return RedirectToAction("KhachHang");
        }
        public ActionResult CTKH(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = kh.MaKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpGet]
        public ActionResult SuaKhachHang(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);

            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpPost, ActionName("SuaKhachHang")]

        public ActionResult xacnhansuaKhachHang(int id)
        {
            KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);

            UpdateModel(kh);
            db.SubmitChanges();

            return RedirectToAction("KhachHang", "Admin");
        }


        //Liên Hệ
        public ActionResult LienHe()
        {
            return View(db.LIENHEs.ToList());

        }
        [HttpGet]
        public ActionResult Xoalienhe(int id)
        {
            LIENHE lh = db.LIENHEs.SingleOrDefault(n => n.MaLienHe == id);
            ViewBag.MaLienHe = lh.MaLienHe;
            if (lh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lh);

        }
        [HttpPost, ActionName("XoaLienHe")]
        public ActionResult XacNhanXoaLienHe(int id)
        {
            LIENHE lh = db.LIENHEs.SingleOrDefault(n => n.MaLienHe == id);
            ViewBag.MaLienHe = lh.MaLienHe;
            if (lh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LIENHEs.DeleteOnSubmit(lh);
            db.SubmitChanges();
            return RedirectToAction("LienHe");
        }
        public ActionResult CTLH(int id)
        {
            LIENHE lh = db.LIENHEs.SingleOrDefault(n => n.MaLienHe == id);
            ViewBag.MaLienHe = lh.MaLienHe;
            if (lh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lh);
        }



        //Đơn Hàng
        public ActionResult DonHang()
        {
            return View(db.DONDATHANGs.ToList());

        }

        public ActionResult CTDH(int id)
        {
            DONDATHANG dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            ViewBag.MaDonHang = dh.MaDonHang;
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }

        [HttpGet]
        public ActionResult SuaDonHang(int id)
        {
            DONDATHANG dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);

            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }
        [HttpPost, ActionName("SuaDonHang")]

        public ActionResult xacnhansuadonhang(int id)
        {
            DONDATHANG dh = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);

            UpdateModel(dh);
            db.SubmitChanges();

            return RedirectToAction("DonHang", "Admin");
        }


    }
}
