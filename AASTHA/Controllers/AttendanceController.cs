using AASTHA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Controllers
{
    public class AttendanceController : BaseController
    {
        //

        // GET: /Attendance/

        AASTHAEntities db = new AASTHAEntities();

        public ActionResult Manage_Attendance()
        {
            var exclude = "ginesh29,bhaumik";
            var user = db.tbl_user.Where(i => !exclude.Contains(i.username)).ToList();
            return View(user);
        }
        [HttpPost]
        public ActionResult Manage_Attendance(List<int> uid, List<string> day1, List<string> day2, List<string> day3, List<string> day4, List<string> day5, List<string> day6, List<string> day7)
        {
            DateTime firstDate = CommonFun.GetFirstDateOfWeek(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).Date, DayOfWeek.Monday);
            DateTime lastDate = CommonFun.GetLastDateOfWeek(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).Date, DayOfWeek.Sunday);

            for (int i = 0; i < day1.Count; i++)
            {
                db.Add_Shift(uid[i], firstDate.AddDays(i), day1[i]);
            }

            for (int i = 0; i < day2.Count; i++)
            {
                db.Add_Shift(uid[i], firstDate.AddDays(i), day2[i]);
            }

            for (int i = 0; i < day3.Count; i++)
            {
                db.Add_Shift(uid[i], firstDate.AddDays(i), day3[i]);
            }

            for (int i = 0; i < day4.Count; i++)
            {
                db.Add_Shift(uid[i], firstDate.AddDays(i), day4[i]);
            }
            for (int i = 0; i < day5.Count; i++)
            {
                db.Add_Shift(uid[i], firstDate.AddDays(i), day5[i]);
            }
            for (int i = 0; i < day6.Count; i++)
            {
                db.Add_Shift(uid[i], firstDate.AddDays(i), day6[i]);
            }
            for (int i = 0; i < day7.Count; i++)
            {
                db.Add_Shift(uid[i], firstDate.AddDays(i), day7[i]);
            }
            var exclude = "ginesh29,bhaumik";
            var user = db.tbl_user.Where(i => !exclude.Contains(i.username)).ToList();
            return View(user);
        }

        public ActionResult ImportAttendance()
        {
            return View();
        }
        //public ActionResult Import(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);
        //        var path = Path.Combine(Server.MapPath("~/Views"), fileName);
        //        file.SaveAs(path);
        //    }
        //    var list = new List<string>();
        //    List<tbl_attendance> lstattendance = new List<tbl_attendance>();
        //    List<tbl_attendance> diff = new List<tbl_attendance>();
        //    using (var stream = new StreamReader(Server.MapPath("~/Views/AGL_001.txt")))
        //    {
        //        string inLine = stream.ReadToEnd();
        //        list = inLine.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
        //        list.RemoveAt(0);

        //        foreach (var s in list)
        //        {
        //            string[] splited = s.Split('\t');
        //            int Id = int.Parse(splited[0]);
        //            int Enroll = int.Parse(splited[2]);
        //            DateTime Date = Convert.ToDateTime(splited[6]);
        //            lstattendance.Add(new tbl_attendance { No = Id, EnNo = Enroll, DateTime = Date });
        //        }
        //        diff = lstattendance.Where(p => !db.tbl_attendance.Any(l => p.No == l.No)).ToList();

        //        foreach (var item in diff)
        //        {
        //            db.Entry(item).State = EntityState.Added;
        //            db.SaveChanges();
        //        }
        //    }
        //    TempData["successmsg"] = diff.Count + " Record added Successfully";
        //    return RedirectToAction("ImportAttendance");
        //}
    }
}