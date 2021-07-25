using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NoiThatHome.Models;

namespace NoiThatHome.Models
{
    public class Giohang
    {
        dbQLNoiThatDataContext data = new dbQLNoiThatDataContext();
        public int iMaNoiThat { get; set; }
        public string sTenNoiThat { get; set; }
        public string sAnhBia { get; set; }
        public Double dDonggia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien
        {
            get { return iSoluong * dDonggia; }
        }
        public Giohang(int MaNoiThat)
        {
            iMaNoiThat = MaNoiThat;
            NOITHAT noithat = data.NOITHATs.Single(n => n.MaNoiThat == iMaNoiThat);
            sTenNoiThat = noithat.TenNoiThat;
            sAnhBia = noithat.Anhbia;
            dDonggia = double.Parse(noithat.Giaban.ToString());
            iSoluong = 1;
        }
    }
}