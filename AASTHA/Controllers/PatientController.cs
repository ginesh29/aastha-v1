using AASTHA.Models;
using AASTHAEntity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace AASTHA.Controllers
{
    public class PatientController : BaseController
    {
        AASTHAEntities db = new AASTHAEntities();
        //
        // GET: /Doctor/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Modal_Edit(int? Patient_Id)
        {

            return PartialView();
        }
        public ActionResult _Edit_Patient(int? Patient_Id)
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Modal_Edit(PatientModel model)
        {
            try
            {
                if (model.Patient_Id_Edit != null)
                {
                    var patient = db.tbl_patient.FirstOrDefault(c => c.patient_Id == model.Patient_Id_Edit);
                    patient.full_name = model.Firstname + " " + model.Middlename + " " + model.Lastname;
                    patient.age = model.Age;
                    patient.mobile = model.Mobile;
                    patient.address = model.Address;
                    db.SaveChanges();
                    TempData["successmsg"] = "Record Upadated Successfully";
                    return RedirectToAction("Manage_Patient", "Patient");
                }
                else
                {
                    tbl_patient table = new tbl_patient();
                    table.full_name = model.Firstname + " " + model.Middlename + " " + model.Lastname;
                    table.age = model.Age;
                    table.mobile = model.Mobile;
                    table.address = model.Address;
                    db.tbl_patient.Add(table);
                    db.SaveChanges();
                    TempData["successmsg"] = "Record Inserted Successfully";
                    return RedirectToAction("Patient_Entry", "Patient");

                }
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
                return RedirectToAction("Manage_Patient", "Patient");
            }

        }


        public ActionResult Modal_Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Modal_Delete(PatientModel model)
        {
            try
            {
                db.delete_patient_entry(model.Patient_Id_Delete);
                TempData["successmsg"] = "Record Deleted Successfully";
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
            }
            return View();
        }

        public ActionResult Manage_Patient()
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

        public JsonResult Getall_Patient(GridSettings grid)
        {
            var query = db.tbl_patient.AsQueryable();

            int count;
            var data = query.GridCommonSettings(grid, out count);

            var result = new
            {
                total = (int)Math.Ceiling((double)count / grid.PageSize),
                page = grid.PageIndex,
                records = count,
                rows = (from p in data
                        select new
                        {
                            patient_id = p.patient_Id,
                            full_name = p.full_name,
                            age = p.age,
                            mobile = p.mobile,
                            address = p.address,
                            action = p.patient_Id
                        }).ToArray()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Fetch_Patient_Details(int? patient_id)
        {
            var patient = db.tbl_patient.Where(c => c.patient_Id == patient_id);
            var patient_detail = "";

            foreach (var item in patient)
            {
                patient_detail = item.full_name + "," + item.age + "," + item.mobile + "," + item.address;
            }
            return Json(patient_detail);
        }
        public ActionResult Patient_Entry()
        {
            return View();
        }
        public JsonResult BindPatient(string term)
        {

            var patients = db.tbl_patient.Where(c => c.full_name.Contains(term)).Select(m => new { m.patient_Id, m.full_name });

            return Json(patients, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsFullnameExist(string Firstname, string Middlename, string Lastname, String Get_View)
        {
            var fullname = Firstname + " " + Middlename + " " + Lastname;
            var data = true;
            if (Get_View != "Edit")
            {
                data = !db.tbl_patient.Any(m => m.full_name == fullname);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult get_data()
        {
            var page = Convert.ToInt32(Request.Form.GetValues("draw").FirstOrDefault());
            var start = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            var rows = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());

            int pageIndex = start;
            int pageSize = Convert.ToInt32(rows);
            var data = db.tbl_patient.Select(m => new { m.patient_Id, m.full_name, m.age, m.mobile, m.address });
            int totalRecords = data.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)Convert.ToDouble(rows));

            return Json(new { recordsFiltered = 5, recordsTotal = 10, data = data }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult AddressList()
        {
            var result = (from r in db.tbl_patient
                          select new { r.address }).Distinct();
            ArrayList list = new ArrayList();
            list.Add(":[All]");
            foreach (var item in result)
            {
                list.Add(item.address + ":" + item.address);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AutocompleteAddress(string term)
        {
            var address = db.tbl_patient.Where(m => m.address.Contains(term)).Select(m => new { name = m.address }).Distinct();
            return Json(address, JsonRequestBehavior.AllowGet);
        }
    }
}