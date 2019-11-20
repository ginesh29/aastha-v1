using AASTHA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Text;
using System.Collections;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AASTHA.Controllers
{
    [DoctorActionFilter]
    public class AdminController : BaseController
    {
        AASTHAEntities db = new AASTHAEntities();
        public ActionResult Dashboard()
        {
            ViewBag.total_patient = db.tbl_patient.Count();
            ViewBag.total_opd = db.tbl_opd.Count();
            ViewBag.thismonth_opd = db.tbl_opd.Where(m => m.date.Value.Month == DateTime.Now.Month).Count();
            ViewBag.total_ipd = db.Get_All_IPD_Entry().Count();
            ViewBag.thismonth_ipd = db.Get_All_IPD_Entry().Where(m => m.addmission_date.Value.Month == DateTime.Now.Month).Count();
            return View();
        }

        public void BindRole(AdminModel model)
        {
            string[] Role = { "Doctor", "Staff" };
            model.Role_List = (from temp in Role
                               select new SelectListItem { Text = temp, Value = temp }).ToList<SelectListItem>();
        }
        public void BindLayout(AdminModel model)
        {
            string[] Layout = { "_LayoutDoctor.cshtml", "_LayoutStaff.cshtml" };

            model.Layout_List = (from temp in Layout
                                 select new SelectListItem { Text = temp, Value = temp }).ToList<SelectListItem>();
        }
        public void BindAppointmentType(AdminModel model)
        {
            string[] Appointment = { "Date", "Sonography", "Anomaly" };
            model.Appointment_Type_List = (from temp in Appointment
                                           select new SelectListItem { Text = temp, Value = temp }).ToList<SelectListItem>();
        }
        public ActionResult Manage_User()
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

        public ActionResult AddEdit_User(int? id)
        {
            AdminModel model = new AdminModel();
            BindRole(model);
            BindLayout(model);
            if (id != null)
            {
                var user = db.tbl_user.FirstOrDefault(m => m.Id == id);
                model.Username = user.username;
                model.Password = user.password;
                model.Biometric_Id = user.Biometric_Id;
                model.Role = user.type;
                model.Layout = user.Layout;
            }
            // 
            return PartialView(model);
        }

        public ActionResult UserGrid(int? page)
        {
            var exclude = "ginesh29";
            var user = db.tbl_user.Where(i => !exclude.Contains(i.username)).ToList().ToPagedList(page ?? 1, 10);
            return PartialView(user);
        }
        [HttpPost]
        public ActionResult AddEdit_User(AdminModel model, int? id, string a)
        {
            BindRole(model);
            BindLayout(model);
            if (id != null)
            {
                var user = db.tbl_user.FirstOrDefault(m => m.Id == id);
                user.username = model.Username;
                user.password = model.Password;
                user.Biometric_Id = model.Biometric_Id;
                user.type = model.Role;
                user.Layout = model.Layout;
            }
            else
            {
                tbl_user table = new tbl_user();
                table.username = model.Username;
                table.password = model.Password;
                table.Biometric_Id = model.Biometric_Id;
                table.type = model.Role;
                table.Layout = model.Layout;
                db.tbl_user.Add(table);
            }
            db.SaveChanges();
            // 
            return RedirectToAction("UserGrid", "Admin");
        }
        public ActionResult Delete_User(int? id)
        {
            var user = db.tbl_user.FirstOrDefault(m => m.Id == id);
            db.tbl_user.Remove(user);
            db.SaveChanges();
            return RedirectToAction("UserGrid", "Admin");
        }
        public void Bind_Medicine_Type(AdminModel model)
        {
            model.Medicine_Type_List = (from temp in db.tbl_medicine_type.ToList()
                                        select new SelectListItem() { Text = temp.medicine_type, Value = temp.medicine_Id.ToString() }).ToList<SelectListItem>();
        }
        public ActionResult Prescription()
        {
            AdminModel model = new AdminModel();
            if (Request.Cookies["Role"].Value == "Doctor")
            {
                Bind_Medicine_Type(model);
                // 
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public JsonResult BindMedicine(int? Id)
        {
            var medicine = db.medicine_master.Where(m => m.medicine_type == Id).Select(m => new { id = m.medicine_type, text = m.medicine_name });
            return Json(medicine, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Manage_MedicineAndType()
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
        public ActionResult Manage_Medicine_Type()
        {
            return View();
        }
        public ActionResult AddEdit_Medicine_Type(int? id)
        {
            AdminModel model = new AdminModel();
            if (id != null)
            {
                var medicinetype = db.tbl_medicine_type.FirstOrDefault(m => m.medicine_Id == id);
                model.Medicine_Type = medicinetype.medicine_type;
                model.page = Convert.ToInt32(model.page);
            }
            else
            {
                model.ViewOperation = "Add";
            }
            // 
            return View(model);
        }
        [HttpPost]
        public ActionResult AddEdit_Medicine_Type(int? id, AdminModel model, string a)
        {
            if (id != null)
            {
                var medicinetype = db.tbl_medicine_type.FirstOrDefault(m => m.medicine_Id == id);
                medicinetype.medicine_type = model.Medicine_Type;
            }
            else
            {
                tbl_medicine_type table = new tbl_medicine_type();
                table.medicine_type = model.Medicine_Type;
                db.tbl_medicine_type.Add(table);
            }
            db.SaveChanges();
            return RedirectToAction("MedicineTypeGrid", "Admin", new { page = model.page });
        }
        public ActionResult MedicineTypeGrid(int? page)
        {
            if (page == 0)
            {
                page = 1;
            }
            return View(db.tbl_medicine_type.OrderBy(m => m.medicine_type).ToList().ToPagedList(page ?? 1, 10));
        }
        public ActionResult Delete_Medicine_Type(int? id, int? page)
        {
            var medicinetype = db.tbl_medicine_type.FirstOrDefault(m => m.medicine_Id == id);
            db.tbl_medicine_type.Remove(medicinetype);
            db.SaveChanges();
            return RedirectToAction("MedicineTypeGrid", "Admin", new { page = page });
        }
        public ActionResult Manage_Medicine(int? page)
        {
            return View();
        }
        public ActionResult AddEdit_Medicine(int? id, int? page)
        {
            AdminModel model = new AdminModel();
            Bind_Medicine_Type(model);
            if (id != null)
            {
                var medicine = db.GetAllMedicineWithType().FirstOrDefault(m => m.medicine_typeId == id);
                model.Medicine_TypeId = medicine.medicine_type;
                model.Medicine_Name = medicine.medicine_name;
                model.page = Convert.ToInt32(page);
            }
            else
            {
                model.ViewOperation = "Add";
            }
            //
            return View(model);
        }
        [HttpPost]
        public ActionResult AddEdit_Medicine(AdminModel model, int? id, int? page, string a)
        {
            Bind_Medicine_Type(model);
            if (id != null)
            {
                var medicine = db.medicine_master.FirstOrDefault(m => m.medicine_typeId == id);
                medicine.medicine_type = model.Medicine_TypeId;
                medicine.medicine_name = model.Medicine_Name;
            }
            else
            {
                medicine_master table = new medicine_master();
                table.medicine_name = model.Medicine_Name;
                table.medicine_type = model.Medicine_TypeId;
                db.medicine_master.Add(table);
            }
            db.SaveChanges();
            return RedirectToAction("MedicineGrid", "Admin", new { page = page });
        }
        public ActionResult Delete_Medicine(int? id, int? page)
        {
            var medicine = db.medicine_master.FirstOrDefault(m => m.medicine_typeId == id);
            db.medicine_master.Remove(medicine);
            db.SaveChanges();
            return RedirectToAction("MedicineGrid", "Admin", new { page = page });
        }
        public ActionResult MedicineGrid(int? page)
        {
            if (page == 0)
            {
                page = 1;
            }
            return View(db.GetAllMedicineWithType().OrderBy(m => m.medicine_name).ToList().ToPagedList(page ?? 1, 10));
        }
        public JsonResult BindOPDId(int id)
        {
            var result = (from r in db.tbl_opd
                          where r.patient_id == id
                          select new { r.opd_receipt_id }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Add_Appointment(AdminModel model)
        {
            if (model.Appointment_Id == 0)
            {
                db.Add_Appointment(model.Patient_Id, model.Appointment_Date, model.Appointment_Type);
                return Json("Appointment has been arrange on " + model.Appointment_Date, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var appoint = db.tbl_appointment.FirstOrDefault(m => m.Id == model.Appointment_Id);
                if (appoint != null)
                {
                    appoint.Patient_Id = model.Patient_Id;
                    appoint.Type = model.Appointment_Type;
                    db.SaveChanges();
                }
                return Json("Appointment has been arrange on " + model.Appointment_Date, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Add_PreceptionAppointment(AdminModel model)
        {
            var date = Convert.ToDateTime(model.Appoint_Date);
            var exist = db.tbl_appointment.Where(m => m.Patient_Id == model.Patient_Id && m.Appointment_Date == date);

            if (!exist.Any())
            {
                db.Add_Appointment(model.Patient_Id, model.Appoint_Date, model.Appointment_Type);
                return Json("Appointment has been arrange on " + model.Appoint_Date, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var appoint = exist.FirstOrDefault();
                appoint.Type = model.Appointment_Type;
                db.SaveChanges();
                return Json("Appointment has been already arranged on " + model.Appoint_Date, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FillAppointment(int Appoint_Id)
        {
            var appointment = from appoint in db.tbl_appointment
                              join patient in db.tbl_patient on appoint.Patient_Id equals patient.patient_Id
                              where appoint.Id == Appoint_Id
                              select new
                              {
                                  appoint.Id,
                                  appoint.Patient_Id,
                                  appoint.Type,
                                  appoint.Appointment_Date,
                                  patient.full_name
                              };
            var appointment_detail = "";
            foreach (var item in appointment)
            {
                appointment_detail = item.Id + "," + item.Patient_Id + "," + Convert.ToDateTime(item.Appointment_Date).ToString("dd/MM/yyyy") + "," + item.Type + "," + item.full_name;
            }
            return Json(appointment_detail);
        }
        public JsonResult Fetch_Appointment(int month, int year)
        {
            ArrayList list = new ArrayList();
            var appointed = from appoint in db.tbl_appointment
                            join patient in db.tbl_patient
                            on appoint.Patient_Id equals patient.patient_Id
                            where appoint.Appointment_Date.Value.Month == month && appoint.Appointment_Date.Value.Year == year
                            select new { appoint.Id, appoint.Appointment_Date, patient.full_name, appoint.Type };
            foreach (var item in appointed)
            {
                string[] splited = item.full_name.Split(' ');
                string firstname = splited[0];
                list.Add(new { Id = item.Id, title = firstname, start = Convert.ToDateTime(item.Appointment_Date).ToString("yyyy-MM-dd"), type = item.Type, description = item.full_name });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public void Bind_Appointment_Type(AdminModel model)
        {
            string[] Appointment = { "Date", "Sonography", "Anomaly", "Ovulation" };
            model.Appointment_List = (from temp in Appointment
                                      select new SelectListItem() { Text = temp, Value = temp }).ToList<SelectListItem>();
        }
        public ActionResult View_Appointment()
        {
            AdminModel model = new AdminModel();
            if (Request.Cookies["Role"].Value == "Doctor")
            {
                Bind_Appointment_Type(model);
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult DeleteAppointment(int? Appoint_Id)
        {
            var appoint = db.tbl_appointment.FirstOrDefault(m => m.Id == Appoint_Id);
            db.tbl_appointment.Remove(appoint);
            db.SaveChanges();
            return Json("True");
        }
        public JsonResult IsUserExist(string Username)
        {
            return Json(!db.tbl_user.Any(m => m.username == Username), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsMedicineTypeExist(string Medicine_Type, string ViewOperation)
        {
            if (ViewOperation == "Add")
            {
                return Json(!db.tbl_medicine_type.Any(m => m.medicine_type == Medicine_Type), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult IsMedicineExist(int? Medicine_TypeId, string Medicine_Name, string ViewOperation)
        {
            if (ViewOperation == "Add")
            {
                return Json(!db.medicine_master.Any(m => m.medicine_type == Medicine_TypeId && m.medicine_name == Medicine_Name), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult IsAppointed(string Appointment_Date, int? Patient_Id, string Get_View)
        {
            var data = false;
            if (Patient_Id > 0)
            {
                data = db.IsAppointed(Patient_Id, Appointment_Date).Any();
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsAppointedPatient(string Followup_date, int? Patient_Id, string Sonography_date)
        {
            if (Followup_date != null)
            {
                return Json(!db.IsAppointed(Patient_Id, Followup_date).Any(), JsonRequestBehavior.AllowGet);
            }
            if (Sonography_date != null)
            {
                return Json(!db.IsAppointed(Patient_Id, Sonography_date).Any(), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult add()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult add(AdminModel model)
        {
            tbl_user table = new tbl_user();
            table.username = model.Username;
            db.Entry(table).State = EntityState.Added;
            db.SaveChanges();
            return RedirectToAction("manage");
        }
        public ActionResult manage()
        {
            return View();
        }
        public ActionResult grid()
        {
            var exclude = "ginesh29";
            var user = db.tbl_user.Where(i => !exclude.Contains(i.username)).ToList();
            return View(user);
        }
        public JsonResult BindMedicineType()
        {
            var medicinetype = db.tbl_medicine_type.Select(m => new { id = m.medicine_Id, text = m.medicine_type });
            return Json(medicinetype, JsonRequestBehavior.AllowGet);
        }
    }
}
