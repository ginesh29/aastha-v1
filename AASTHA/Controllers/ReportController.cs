using AASTHA.Models;
using AASTHAEntity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Controllers
{
    public class ReportController : BaseController
    {
        //
        // GET: /Report/
        AASTHAEntities db = new AASTHAEntities();
        public ActionResult OPD_Report()
        {
            if (Request.Cookies["Role"].Value == "Doctor")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public string Getall_OPD_Report(string opddate, string fromdate, string todate, string report)
        {
            if (opddate == "")
            {
                opddate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            var data = from temp in db.OPD_Report(report, opddate, fromdate, todate).ToList()
                       select new
                       {
                           temp.opd_Id,
                           temp.opd_receipt_id,
                           temp.full_name,
                           temp.date,
                           temp.case_type,
                           temp.Consult_Charge,
                           temp.USG_Charge,
                           temp.UPT_Charge,
                           temp.Inj_Charge,
                           temp.Other_Charge,
                           temp.Total
                       };
            return JsonConvert.SerializeObject(new { data = data }, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public ActionResult IPD_Report()
        {
            if (Request.Cookies["Role"].Value == "Doctor")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public string Getall_IPD_Report(DatatableModel param, string ipddate, string fromdate, string todate, string report)
        {
            if (ipddate == "")
            {
                ipddate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            var data = from temp in db.IPD_Report(report, ipddate, fromdate, todate).ToList()
                       select new
                       {
                           temp.ipd_Id,
                           temp.full_name,
                           temp.dept_name,
                           temp.addmission_date,
                           temp.discharge_date,
                           temp.C1,
                           temp.C2,
                           temp.C3,
                           temp.C4,
                           temp.C5,
                           temp.C6,
                           temp.C7,
                           temp.C8,
                           temp.C9,
                           temp.C10,
                           temp.C11,
                           temp.C12,
                           temp.C13,
                           temp.Bill,
                           temp.Final_Total
                       };
            return JsonConvert.SerializeObject(new { data = data }, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public ActionResult Statistics()
        {
            return View();
        }
        public ActionResult YearTab()
        {
            var a = (from temp in db.tbl_opd
                     where temp.date.Value.Year != 2013
                     select new { temp.date.Value.Year }).Distinct().OrderByDescending(m => m.Year);
            List<tbl_opd> list = new List<tbl_opd>();

            foreach (var item in a)
            {
                tbl_opd d = new tbl_opd();
                d.case_type = item.Year.ToString();
                list.Add(d);
            }
            return View(list);
        }
        public ActionResult YearTab2()
        {
            var a = (from temp in db.tbl_opd
                     where temp.date.Value.Year != 2013
                     select new { temp.date.Value.Year }).Distinct().OrderByDescending(m => m.Year);
            List<tbl_opd> list = new List<tbl_opd>();

            foreach (var item in a)
            {
                tbl_opd d = new tbl_opd();
                d.case_type = item.Year.ToString();
                list.Add(d);
            }
            return View(list);
        }
        public ActionResult OPD_Statistics()
        {
            //ViewBag.total_patients = db.tbl_opd.Count();
            //ViewBag.total_collction = db.tbl_opd.Sum(m => m.consult_charge + m.usg_charge + m.upt_charge + m.inj_charge + m.other_charge);
            return View(db.Get_OPD_Monthwise_Collection());
        }
        public ActionResult IPD_Statistics()
        {
            //ViewBag.total_patients = db.tbl_Ipd.Count();
            //ViewBag.total_collction = db.tbl_Ipd.Sum(m => m.bill) - db.tbl_Ipd.Sum(m => m.conssesion);
            return View(db.Get_IPD_Monthwise_Collection());
        }

        public ActionResult OPDReport()
        {
            return View();
        }
        public ActionResult AngularOPDReport()
        {
            return View();
        }
        public ActionResult demo()
        {
            return View();
        }
        public string GetOPD_Report(string opddate, string fromdate, string todate, string report)
        {
            if (opddate == "")
            {
                opddate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            var data = db.OPD_Report(report, opddate, fromdate, todate).Select(temp => new
            {
                temp.opd_Id,
                temp.opd_receipt_id,
                temp.full_name,
                temp.date,
                temp.case_type,
                temp.Consult_Charge,
                temp.USG_Charge,
                temp.UPT_Charge,
                temp.Inj_Charge,
                temp.Other_Charge,
                temp.Total
            });
            var summary = db.DateWiseOPD_Collection(report, opddate, fromdate, todate);

            // UsersContext u = new UsersContext();           

            return JsonConvert.SerializeObject(new { data = data, summary = summary }, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public string GetIPD_Report(string ipddate, string fromdate, string todate, string report)
        {
            if (ipddate == "")
            {
                ipddate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            var data = db.IPD_Report(report, ipddate, fromdate, todate).OrderBy(m => m.discharge_date).Select(temp => new
            {
                temp.ipd_receipt_id,
                temp.ipd_Id,
                temp.full_name,
                temp.room_type,
                temp.address,
                temp.dept_name,
                temp.addmission_date,
                temp.discharge_date,
                temp.C1,
                temp.C2,
                temp.C3,
                temp.C4,
                temp.C5,
                temp.C6,
                temp.C7,
                temp.C8,
                temp.C9,
                temp.C10,
                temp.C11,
                temp.C12,
                temp.C13,
                temp.Bill,
                temp.Final_Total,
                AmtInWord = NumbersToWords((int)temp.Final_Total),
                charge = from charge_master in db.IPD_Charges_Master
                         join charge_detail in db.IPD_Charge_Details on charge_master.Charge_Id equals charge_detail.Charge_Id
                         where charge_detail.IPD_Id == temp.ipd_Id
                         select new { charge_detail.Charge_Id, charge_master.Charge_Title, charge_detail.Days, charge_detail.Rate, charge_detail.Amount }
            });
            var summary = db.DateWiseIPD_Collection(report, ipddate, fromdate, todate);
            return JsonConvert.SerializeObject(new { data = data, summary = summary }, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public string GetAttendance_Report(string ipddate, string fromdate, string todate, string report)
        {
            if (ipddate == "")
            {
                ipddate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            var data = db.IPD_Report(report, ipddate, fromdate, todate).OrderBy(m => m.discharge_date).Select(temp => new
            {
                temp.ipd_receipt_id,
                temp.ipd_Id,
                temp.full_name,
                temp.room_type,
                temp.address,
                temp.dept_name,
                temp.addmission_date,
                temp.discharge_date,
                temp.C1,
                temp.C2,
                temp.C3,
                temp.C4,
                temp.C5,
                temp.C6,
                temp.C7,
                temp.C8,
                temp.C9,
                temp.C10,
                temp.C11,
                temp.C12,
                temp.C13,
                temp.Bill,
                temp.Final_Total,
                AmtInWord = NumbersToWords((int)temp.Final_Total),
                charge = from charge_master in db.IPD_Charges_Master
                         join charge_detail in db.IPD_Charge_Details on charge_master.Charge_Id equals charge_detail.Charge_Id
                         where charge_detail.IPD_Id == temp.ipd_Id
                         select new { charge_detail.Charge_Id, charge_master.Charge_Title, charge_detail.Days, charge_detail.Rate, charge_detail.Amount }
            });
            return JsonConvert.SerializeObject(data, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public ActionResult Biometric_Report()
        {
            return View();
        }
        public string Get_BiometricReport(string date, string report)
        {
            if (date == "")
            {
                date = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            //var data = (from temp in db.Biometric_Report(report, date)
            //            select new
            //            {
            //                temp.username,
            //                temp.PunchDatetime
            //            });
            var data = db.Biometric_Report(report, date).Select(temp =>
                new
                {
                    temp.username,
                    PunchDate = temp.DateTime,
                    PunchTime = temp.DateTime.Value.ToString("hh:mm tt"),
                    //PunchInTime = db.Tran_MachineRawPunch.FirstOrDefault(m => m.PunchDatetime == temp.PunchDatetime).PunchDatetime.ToString("hh:mm tt"),
                    //PunchOutTime = db.Tran_MachineRawPunch.OrderBy(m => m.PunchDatetime).FirstOrDefault(m => m.PunchDatetime == temp.PunchDatetime).PunchDatetime.ToString("hh:mm tt")

                });
            return JsonConvert.SerializeObject(data, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public ActionResult Attendance_Report()
        {
            return View();
        }
        public string Get_MonthlyAttendance_Report(string date)
        {
            if (date == "")
            {
                date = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            var data = db.Monthly_AttendanceReport(date).Select(m => new
            {
                m.username,
                C1 = m.C1 == 0 ? "A" : "P",
                C2 = m.C2 == 0 ? "A" : "P",
                C3 = m.C3 == 0 ? "A" : "P",
                C4 = m.C4 == 0 ? "A" : "P",
                C5 = m.C5 == 0 ? "A" : "P",
                C6 = m.C6 == 0 ? "A" : "P",
                C7 = m.C7 == 0 ? "A" : "P",
                C8 = m.C8 == 0 ? "A" : "P",
                C9 = m.C9 == 0 ? "A" : "P",
                C10 = m.C10 == 0 ? "A" : "P",
                C11 = m.C11 == 0 ? "A" : "P",
                C12 = m.C12 == 0 ? "A" : "P",
                C13 = m.C13 == 0 ? "A" : "P",
                C14 = m.C14 == 0 ? "A" : "P",
                C15 = m.C15 == 0 ? "A" : "P",
                C16 = m.C16 == 0 ? "A" : "P",
                C17 = m.C17 == 0 ? "A" : "P",
                C18 = m.C18 == 0 ? "A" : "P",
                C19 = m.C19 == 0 ? "A" : "P",
                C20 = m.C20 == 0 ? "A" : "P",
                C21 = m.C21 == 0 ? "A" : "P",
                C22 = m.C22 == 0 ? "A" : "P",
                C23 = m.C23 == 0 ? "A" : "P",
                C24 = m.C24 == 0 ? "A" : "P",
                C25 = m.C25 == 0 ? "A" : "P",
                C26 = m.C26 == 0 ? "A" : "P",
                C27 = m.C27 == 0 ? "A" : "P",
                C28 = m.C28 == 0 ? "A" : "P",
                C29 = m.C29 == 0 ? "A" : "P",
                C30 = m.C30 == 0 ? "A" : "P",
                C31 = m.C31 == 0 ? "A" : "P",
                PresentDays = (m.C1 == 0 ? 0 : m.C1) +
                (m.C1 != 0 ? 1 : m.C1) +
                (m.C2 != 0 ? 1 : m.C2) +
                (m.C3 != 0 ? 1 : m.C3) +
                (m.C4 != 0 ? 1 : m.C4) +
                (m.C5 != 0 ? 1 : m.C5) +
                (m.C6 != 0 ? 1 : m.C6) +
                (m.C7 != 0 ? 1 : m.C7) +
                (m.C8 != 0 ? 1 : m.C8) +
                (m.C9 != 0 ? 1 : m.C9) +
                (m.C10 != 0 ? 1 : m.C10) +
                (m.C11 != 0 ? 1 : m.C11) +
                (m.C12 != 0 ? 1 : m.C12) +
                (m.C13 != 0 ? 1 : m.C13) +
                (m.C14 != 0 ? 1 : m.C14) +
                (m.C15 != 0 ? 1 : m.C15) +
                (m.C16 != 0 ? 1 : m.C16) +
                (m.C17 != 0 ? 1 : m.C17) +
                (m.C18 != 0 ? 1 : m.C18) +
                (m.C19 != 0 ? 1 : m.C19) +
                (m.C20 != 0 ? 1 : m.C20) +
                (m.C21 != 0 ? 1 : m.C21) +
                (m.C22 != 0 ? 1 : m.C22) +
                (m.C23 != 0 ? 1 : m.C23) +
                (m.C24 != 0 ? 1 : m.C24) +
                (m.C25 != 0 ? 1 : m.C25) +
                (m.C26 != 0 ? 1 : m.C26) +
                (m.C27 != 0 ? 1 : m.C27) +
                (m.C28 != 0 ? 1 : m.C28) +
                (m.C29 != 0 ? 1 : m.C29) +
                (m.C30 != 0 ? 1 : m.C30) +
                (m.C31 != 0 ? 1 : m.C31)
            });
            return JsonConvert.SerializeObject(data, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public ActionResult InvoicesList()
        {
            return View();
        }
        public string NumbersToWords(int inputNumber)
        {
            int inputNo = inputNumber;

            if (inputNo == 0)
                return "Zero";

            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    //if (h > 0 || i == 0) sb.Append("");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
        public JsonResult InvoiceCharges_Table(int ipd_id)
        {
            var charges_Table = db.Get_IPD_Charge_Tabele().Where(m => m.IPD_Id == ipd_id);
            return Json(charges_Table, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A5, 10f, 10f, 20f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Exported.pdf");
            }
        }
        public class EmailModel
        {
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public HttpPostedFileBase Attachment { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        [HttpPost]
        public ActionResult Index(EmailModel model)
        {
            using (MailMessage mm = new MailMessage(model.Email, model.To))
            {
                mm.Subject = model.Subject;
                mm.Body = model.Body;
                if (model.Attachment.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(model.Attachment.FileName);
                    mm.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
                }
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email sent.";
                }
            }

            return View();
        }
    }
}
