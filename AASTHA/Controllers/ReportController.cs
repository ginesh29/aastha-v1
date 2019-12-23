using AASTHA.Models;
using AASTHAEntity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            var data = db.OPD_Report(report, opddate, fromdate, todate).ToList();
            var summary = db.DateWiseOPD_Collection(report, opddate, fromdate, todate);
            TempData["OPDReportData"] = data;
            return JsonConvert.SerializeObject(new { summary = summary, data = data }, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public string GetIPD_Report(string ipddate, string fromdate, string todate, string report)
        {
            if (ipddate == "")
            {
                ipddate = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yy");
            }
            var data = db.IPD_Report(report, ipddate, fromdate, todate).OrderBy(m => m.discharge_date).ToList();
            var summary = db.DateWiseIPD_Collection(report, ipddate, fromdate, todate);
            TempData["IPDReportData"] = data;
            return JsonConvert.SerializeObject(new { summary = summary, data = data }, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
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
        //public class OPDReportDataModel
        //{
        //    public int OPDReport { get; set; }
        //}
        public ActionResult ExportOPD()
        {
            var stream = new MemoryStream();
            var OPDReportData = TempData["OPDReportData"] as List<OPD_Report_Result>;
            TempData.Keep("OPDReportData");
            using (var package = new ExcelPackage(stream))
            {
                var workSheet2 = package.Workbook.Worksheets.Add("Report");
                workSheet2.DefaultColWidth = 15;

                workSheet2.Cells["A1"].Value = "Invoice No";
                workSheet2.Cells["B1"].Value = "OPD Id";
                workSheet2.Cells["C1"].Value = "Patient's Name";
                workSheet2.Cells["D1"].Value = "Case Type";
                workSheet2.Cells["E1"].Value = "OPD Date";
                workSheet2.Cells["f1"].Value = "Cons";
                workSheet2.Cells["G1"].Value = "USG";
                workSheet2.Cells["H1"].Value = "UPT";
                workSheet2.Cells["I1"].Value = "Inj";
                workSheet2.Cells["J1"].Value = "Other";
                workSheet2.Cells["K1"].Value = "Total";

                var headerCell = workSheet2.Cells["A1:K1"];
                headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                headerCell.Style.Font.Bold = true;                

                int row1 = 2;
                foreach (var item in OPDReportData)
                {
                    workSheet2.Cells[string.Format("A{0}", row1)].Value = item.opd_Id;
                    workSheet2.Cells[string.Format("B{0}", row1)].Value = item.opd_receipt_id;
                    workSheet2.Cells[string.Format("C{0}", row1)].Value = item.full_name;
                    workSheet2.Cells[string.Format("D{0}", row1)].Value = item.case_type;
                    workSheet2.Cells[string.Format("E{0}", row1)].Value = item.date;
                    workSheet2.Cells[string.Format("F{0}", row1)].Value = Convert.ToDecimal(item.Consult_Charge);
                    workSheet2.Cells[string.Format("G{0}", row1)].Value = Convert.ToDecimal(item.USG_Charge);
                    workSheet2.Cells[string.Format("H{0}", row1)].Value = Convert.ToDecimal(item.UPT_Charge);
                    workSheet2.Cells[string.Format("I{0}", row1)].Value = Convert.ToDecimal(item.Inj_Charge);
                    workSheet2.Cells[string.Format("J{0}", row1)].Value = Convert.ToDecimal(item.Other_Charge);
                    workSheet2.Cells[string.Format("K{0}", row1)].Value = Convert.ToDecimal(item.Total);
                    workSheet2.Cells[string.Format("E{0}", row1)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    row1++;
                }
                workSheet2.Cells[string.Format("A{0}", row1)].Value = "Total";
                workSheet2.Cells[string.Format("F{0}", row1)].Formula = "=SUM(F2:F" + row1 + ")";
                workSheet2.Cells[string.Format("G{0}", row1)].Formula = "=SUM(G2:G" + row1 + ")";
                workSheet2.Cells[string.Format("H{0}", row1)].Formula = "=SUM(H2:H" + row1 + ")";
                workSheet2.Cells[string.Format("I{0}", row1)].Formula = "=SUM(I2:I" + row1 + ")";
                workSheet2.Cells[string.Format("J{0}", row1)].Formula = "=SUM(J2:J" + row1 + ")";
                workSheet2.Cells[string.Format("K{0}", row1)].Formula = "=SUM(K2:K" + row1 + ")";

                var footerCell = workSheet2.Cells[string.Format("A{0}:K{0}", row1)];
                footerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                footerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                footerCell.Style.Font.Bold = true;

                workSheet2.Column(1).Width = 10;
                workSheet2.Column(3).Width = 30;
                workSheet2.Column(4).Width = 10;
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"OPDReport.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
        public ActionResult ExportIPD()
        {
            var stream = new MemoryStream();
            var IPDReportData = TempData["IPDReportData"] as List<IPD_Report_Result>;
            TempData.Keep("IPDReportData");
            using (var package = new ExcelPackage(stream))
            {
                //var workSheet = package.Workbook.Worksheets.Add("Summery");
                var workSheet2 = package.Workbook.Worksheets.Add("Report");
                workSheet2.DefaultColWidth = 15;

                workSheet2.Cells["A1"].Value = "Invoice No";
                workSheet2.Cells["B1"].Value = "Patient's Name";
                workSheet2.Cells["C1"].Value = "Ipd Type";
                workSheet2.Cells["D1"].Value = "Add. Date";
                workSheet2.Cells["E1"].Value = "Dis. Date";
                workSheet2.Cells["F1"].Value = "Addmission";
                workSheet2.Cells["G1"].Value = "Consulting";
                workSheet2.Cells["H1"].Value = "Delivery/Operation";
                workSheet2.Cells["I1"].Value = "Labour/Operation Room";
                workSheet2.Cells["J1"].Value = "Room";
                workSheet2.Cells["K1"].Value = "Doctor Visting";
                workSheet2.Cells["L1"].Value = "Nursing";
                workSheet2.Cells["M1"].Value = "Bio-Medical Waste";
                workSheet2.Cells["N1"].Value = "Oxygen";
                workSheet2.Cells["O1"].Value = "Baby Care";
                workSheet2.Cells["P1"].Value = "Dressing";
                workSheet2.Cells["Q1"].Value = "Paediatrician";
                workSheet2.Cells["R1"].Value = "Other";
                workSheet2.Cells["S1"].Value = "Total";

                var headerCell = workSheet2.Cells["A1:S1"];
                headerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                headerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                headerCell.Style.Font.Bold = true;

                int row1 = 2;
                foreach (var item in IPDReportData)
                {
                    workSheet2.Cells[string.Format("A{0}", row1)].Value = item.ipd_Id;
                    workSheet2.Cells[string.Format("B{0}", row1)].Value = item.full_name;
                    workSheet2.Cells[string.Format("C{0}", row1)].Value = item.dept_name;
                    workSheet2.Cells[string.Format("D{0}", row1)].Value = item.addmission_date.Value;
                    workSheet2.Cells[string.Format("E{0}", row1)].Value = item.discharge_date.Value;
                    workSheet2.Cells[string.Format("F{0}", row1)].Value = Convert.ToDecimal(item.C1);
                    workSheet2.Cells[string.Format("G{0}", row1)].Value = Convert.ToDecimal(item.C2);
                    workSheet2.Cells[string.Format("H{0}", row1)].Value = Convert.ToDecimal(item.C3);
                    workSheet2.Cells[string.Format("I{0}", row1)].Value = Convert.ToDecimal(item.C4);
                    workSheet2.Cells[string.Format("J{0}", row1)].Value = Convert.ToDecimal(item.C5);
                    workSheet2.Cells[string.Format("K{0}", row1)].Value = Convert.ToDecimal(item.C6);
                    workSheet2.Cells[string.Format("L{0}", row1)].Value = Convert.ToDecimal(item.C7);
                    workSheet2.Cells[string.Format("M{0}", row1)].Value = Convert.ToDecimal(item.C8);
                    workSheet2.Cells[string.Format("N{0}", row1)].Value = Convert.ToDecimal(item.C9);
                    workSheet2.Cells[string.Format("O{0}", row1)].Value = Convert.ToDecimal(item.C10);
                    workSheet2.Cells[string.Format("P{0}", row1)].Value = Convert.ToDecimal(item.C11);
                    workSheet2.Cells[string.Format("Q{0}", row1)].Value = Convert.ToDecimal(item.C12);
                    workSheet2.Cells[string.Format("R{0}", row1)].Value = Convert.ToDecimal(item.C13);
                    workSheet2.Cells[string.Format("S{0}", row1)].Value = Convert.ToDecimal(item.Final_Total);

                    workSheet2.Cells[string.Format("D{0}", row1)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    workSheet2.Cells[string.Format("E{0}", row1)].Style.Numberformat.Format = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
                    row1++;
                }
                workSheet2.Cells[string.Format("A{0}", row1)].Value = "Total";
                workSheet2.Cells[string.Format("F{0}", row1)].Formula = "=SUM(F2:F" + row1 + ")";
                workSheet2.Cells[string.Format("G{0}", row1)].Formula = "=SUM(G2:G" + row1 + ")";
                workSheet2.Cells[string.Format("H{0}", row1)].Formula = "=SUM(H2:H" + row1 + ")";
                workSheet2.Cells[string.Format("I{0}", row1)].Formula = "=SUM(I2:I" + row1 + ")";
                workSheet2.Cells[string.Format("J{0}", row1)].Formula = "=SUM(J2:J" + row1 + ")";
                workSheet2.Cells[string.Format("K{0}", row1)].Formula = "=SUM(K2:K" + row1 + ")";
                workSheet2.Cells[string.Format("L{0}", row1)].Formula = "=SUM(L2:L" + row1 + ")";
                workSheet2.Cells[string.Format("M{0}", row1)].Formula = "=SUM(M2:M" + row1 + ")";
                workSheet2.Cells[string.Format("N{0}", row1)].Formula = "=SUM(N2:N" + row1 + ")";
                workSheet2.Cells[string.Format("O{0}", row1)].Formula = "=SUM(O2:O" + row1 + ")";
                workSheet2.Cells[string.Format("P{0}", row1)].Formula = "=SUM(P2:P" + row1 + ")";
                workSheet2.Cells[string.Format("Q{0}", row1)].Formula = "=SUM(Q2:Q" + row1 + ")";
                workSheet2.Cells[string.Format("R{0}", row1)].Formula = "=SUM(R2:R" + row1 + ")";
                workSheet2.Cells[string.Format("S{0}", row1)].Formula = "=SUM(S2:S" + row1 + ")";

                var footerCell = workSheet2.Cells[string.Format("A{0}:S{0}", row1)];
                footerCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                footerCell.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                footerCell.Style.Font.Bold = true;

                workSheet2.Column(1).Width = 10;
                workSheet2.Column(2).Width = 30;
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"IPDReport.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
