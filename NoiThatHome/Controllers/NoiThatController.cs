using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoiThatHome.Models;

using PagedList;
using PagedList.Mvc;

namespace NoiThatHome.Controllers
{
    public class NoiThatController : Controller
    {
        dbQLNoiThatDataContext db = new dbQLNoiThatDataContext ();
        
        private List<NOITHAT> laynoithat(int count)
        {
            return db.NOITHATs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        // GET: NoiThat
        public ActionResult Trangchu()
        {
            return View();
        }
        public ActionResult Gioithieu()
        {
            return View();
        }
        public ActionResult Sanpham(int ? page)
        {
            //Tạo biến quyi định số sản phảm trên mỗi trag
            int pageSize = 6;
            //Tạo biến số trang
            int pageNum = (page ?? 1);


            var noithatmoi = laynoithat(21);
            return View(noithatmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Loai()
        {
            var loai = from g in db.LOAISPs select g;
            return PartialView(loai);
        }
        public ActionResult Noithattheoloai (int id)
        {
            var noithat = from g in db.NOITHATs where g.MaLoaiSP == id select g;
            return View(noithat);
        }
        public ActionResult Loaiphong()
        {
            var loaiphong = from g in db.LOAIPHONGs select g;
            return PartialView(loaiphong);
        }
        public ActionResult Noithattheophong(int id)
        {
            var noithat = from g in db.NOITHATs where g.MaLoaiPhong == id select g;
            return View(noithat);
        }
        public ActionResult TenNoiThat(int id)
        {
            var noithat = from g in db.NOITHATs where g.MaNoiThat == id select g;
            return View(noithat.Single());
        }

        public ActionResult Lienhe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Lienhe(FormCollection data, LIENHE lh)
        {

            var hoten = data["HoTen"];
            var email = data["Email"];
            var noidung = data["NoiDung"];

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống ";
            }

            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "Email không được để trống  ";

            }
            if (String.IsNullOrEmpty(noidung))
            {
                ViewData["Loi3"] = "Nhập nội dung  ";
            }
            else
            {
                lh.HoTen = hoten;
                lh.Email = email;
                lh.NoiDung = noidung;
                db.LIENHEs.InsertOnSubmit(lh);
                db.SubmitChanges();

                return RedirectToAction("Trangchu", "NoiThat");
            }

            return this.Lienhe();
        }
    }
}