using AASTHA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Globalization;
using AASTHAEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AASTHA.Controllers
{
    public class OPDController : BaseController
    {
        AASTHAEntities db = new AASTHAEntities();
        //
        // GET: /OPD/

        public ActionResult Manage_OPD_Entry()
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
        public string Getall_OPD_Entry(GridSettings grid)
        {
            var query = (from opd in db.tbl_opd
                         join patient in db.tbl_patient on opd.patient_id equals patient.patient_Id
                         select new { opd.opd_Id, opd.opd_receipt_id, patient.full_name, opd.date, opd.case_type, opd.consult_charge, opd.usg_charge, opd.upt_charge, opd.inj_charge, opd.other_charge }).AsQueryable();

            int count;
            var data = query.GridCommonSettings(grid, out count);

            var result = new
            {
                total = (int)Math.Ceiling((double)count / grid.PageSize),
                page = grid.PageIndex,
                records = count,
                rows = data.Select(o => new
                {
                    opd_Id = o.opd_Id,
                    opd_receipt_id = o.opd_receipt_id,
                    full_name = o.full_name,
                    date = o.date,
                    case_type = o.case_type,
                    Consult_Charge = o.consult_charge ?? 0,
                    USG_Charge = o.usg_charge ?? 0,
                    UPT_Charge = o.upt_charge ?? 0,
                    Inj_Charge = o.inj_charge ?? 0,
                    Other_Charge = o.other_charge ?? 0,
                    Total = (o.consult_charge ?? 0) + (o.usg_charge ?? 0) + (o.upt_charge ?? 0) + (o.inj_charge ?? 0) + (o.other_charge ?? 0),
                    action = o.opd_Id
                })
            };
            return JsonConvert.SerializeObject(result, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }
        public ActionResult Fetch_OPD_Details(int? opd_id)
        {
            var opddata = from opd in db.tbl_opd
                          join patient in db.tbl_patient
                              on opd.patient_id equals patient.patient_Id
                          where opd.opd_Id == opd_id
                          select new { patient.patient_Id, patient.full_name, opd.opd_Id, opd.date, opd.consult_charge, opd.usg_charge, opd.upt_charge, opd.inj_charge, opd.other_charge, opd.case_type, opd.opd_receipt_id, patient.address };
            var opd_detail = "";
            foreach (var item in opddata)
            {
                opd_detail = item.opd_Id + "," + Convert.ToDateTime(item.date).ToString("dd/MM/yyyy") + "," + item.consult_charge + "," + item.usg_charge + "," + item.upt_charge + "," + item.inj_charge + "," + item.other_charge + "," + item.full_name + "," + item.case_type + "," + item.patient_Id + "," + item.opd_receipt_id + "," + item.address;
            }
            return Json(opd_detail);
        }
        public ActionResult Modal_Edit()
        {
            Bind_CaseType();
            return View();
        }
        [HttpPost]
        public ActionResult Modal_Edit(OPDModel model, int? Patient_id)
        {
            try
            {
                Bind_CaseType();

                if (ModelState.IsValid)
                {
                    tbl_opd table = new tbl_opd();
                    if (model.OPD_Id_Edit == null)
                    {
                        db.Add_OPD_Record(model.Patient_Id, model.OPD_Date, model.Case_Type_Name, model.Cons_Charge, model.USG_Charge, model.UPT_Charge, model.Inj_Charge, model.Other_Charge);
                        TempData["successmsg"] = "Record Inserted Successfully";
                    }
                    else
                    {
                        db.Edit_OPD_Record(model.OPD_Id_Edit, model.Patient_Id, model.Case_Type_Name, model.OPD_Date, model.Cons_Charge, model.USG_Charge, model.UPT_Charge, model.Inj_Charge, model.Other_Charge);
                        TempData["successmsg"] = "Record Updated Successfully";
                    }
                    return RedirectToAction("OPD_Entry", "OPD");

                }
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
            }
            return View(model);

        }

        public void Bind_CaseType()
        {
            string[] List = { "Old", "New" };
            ViewBag.OPD_Case_TypeList = (from temp in List
                                         select new SelectListItem() { Text = temp, Value = temp }).ToList<SelectListItem>();
        }
        public ActionResult Modal_Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Modal_Delete(OPDModel model)
        {
            try
            {
                db.delete_opd_entry(model.OPD_Id_Delete);
                TempData["successmsg"] = "Record Deleted Successfully";
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
            }

            return View();
        }
        public ActionResult OPD_Entry()
        {
            Bind_CaseType();
            return View();
        }

        public ActionResult OPD_Invoice()
        {
            return View();
        }       
    }
}
