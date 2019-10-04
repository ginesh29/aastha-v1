using AASTHA.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Collections;
using AASTHAEntity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AASTHA.Controllers
{
    public class IPDController : Controller
    {

        AASTHAEntities db = new AASTHAEntities();

        public void Bind_IPD_Type()
        {
            string[] type = { "Delivery", "Operation", "General" };
            ViewBag.IPD_Type_List = type.Select(m => new SelectListItem { Text = m, Value = m });
        }
        public void Bind_Room_Type()
        {
            string[] Room = { "General", "Special", "Semi-Special" };
            ViewBag.Room_Type_List = Room.Select(m => new SelectListItem { Text = m, Value = m });
        }
        public void Bind_Delivery_Type()
        {
            ViewBag.Delivery_Type_List = db.delivery_master.ToList().Select(m => new SelectListItem { Text = m.delivery, Value = m.delivery_typeId.ToString() });
        }
        public void Bind_Baby_Gender()
        {
            string[] Gender = { "Boy", "Girl" };
            ViewBag.Baby_Gender_List = Gender.Select(m => new SelectListItem { Text = m, Value = m });
        }
        public void Bind_Delivery_Diagnosis()
        {
            string[] Delivery_Diagnosis = { "Labour Pain", "Licking P/V" };
            ViewBag.Delivery_Diagnosis_List = Delivery_Diagnosis.Select(m => new SelectListItem { Text = m, Value = m });
        }
        public void Bind_General_Diagnosis()
        {
            ViewBag.General_Diagnosis_List = db.general_diagnosis.ToList().Select(m => new SelectListItem { Text = m.general_diagnosis_name, Value = m.general_diagnosis_Id.ToString() });
        }
        public void Bind_Operation_Diagnosis()
        {
            ViewBag.Operation_Diagnosis_List = db.diagnosis_master.ToList().Select(m => new SelectListItem { Text = m.diagnosis_type, Value = m.diagnosis_type.ToString() });
        }
        public void Bind_Operation_Type()
        {
            ViewBag.Operation_Type_List = db.operation_master.ToList().Select(m => new SelectListItem { Text = m.operation_type, Value = m.operation_typeId.ToString() });
        }
        public ActionResult IPD_Entry()
        {
            if (Request.Cookies["User"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Bind_IPD_Type();
                Bind_Room_Type();
                Bind_Delivery_Type();
                Bind_Baby_Gender();
                Bind_Delivery_Diagnosis();
                Bind_General_Diagnosis();
                Bind_Operation_Diagnosis();
                Bind_Operation_Type();
                return View();
            }
        }

        public ActionResult Modal_Edit(int? ipd_id)
        {
            Bind_IPD_Type();
            Bind_Room_Type();
            Bind_Delivery_Type();
            Bind_Baby_Gender();
            Bind_Delivery_Diagnosis();
            Bind_General_Diagnosis();
            Bind_Operation_Diagnosis();
            Bind_Operation_Type();
            return View();
        }

        public ActionResult Get_IpdType(int? Ipd_Id)
        {
            var type = db.tbl_Ipd.FirstOrDefault(m => m.ipd_Id == Ipd_Id).dept_name;
            return Json(type, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Charges_Table()
        {
            return View(db.IPD_Charges_Master.ToList());
        }
        [HttpPost]
        public ActionResult Modal_Edit(IPDModel model, List<int> Charge_Id, List<float> day, List<int> rate, List<int> amt, int? total, int? Patient_Id, int? Concession)
        {
            try
            {

                var exist = db.tbl_Ipd.FirstOrDefault(c => c.ipd_Id == model.IPD_Id_Edit);
                if (exist == null)
                {
                    if (model.IPD_Type == "Delivery")
                    {
                        string Delivery_Type = string.Join(",", model.Delivery_Type);
                        db.insert_ipd_entry(model.IPD_Type, model.Patient_Id, model.IPD_Id_Edit, model.Room_Type, model.Addmission_Date, model.Discharge_Date, total, Concession, null, null, null, null, model.Delivery_Diagnosis, Delivery_Type, model.Delivery_Date, model.Delivery_Time, model.Baby_Gender, model.Baby_Weight);
                    }
                    else if (model.IPD_Type == "Operation")
                    {
                        string Operation_Diagnosis = string.Join(",", model.Operation_Diagnosis);
                        string Operation_Type = string.Join(",", model.Operation_Type);
                        db.insert_ipd_entry(model.IPD_Type, model.Patient_Id, model.IPD_Id_Edit, model.Room_Type, model.Addmission_Date, model.Discharge_Date, total, Concession, null, Operation_Diagnosis, model.Operation_Date, Operation_Type, null, null, null, null, null, null);
                    }
                    else if (model.IPD_Type == "General")
                    {
                        string General_Diagnosis = string.Join(",", model.General_Diagnosis);
                        db.insert_ipd_entry(model.IPD_Type, model.Patient_Id, model.IPD_Id_Edit, model.Room_Type, model.Addmission_Date, model.Discharge_Date, total, Concession, General_Diagnosis, null, null, null, null, null, null, null, null, null);
                    }
                    for (int i = 0; i < Charge_Id.Count; i++)
                    {
                        db.Insert_IPDCharge_Table(model.IPD_Id_Edit, Charge_Id[i], day[i], rate[i], amt[i]);
                    }
                    TempData["successmsg"] = "Record Inserted Successfully";
                }
                else if (exist != null)
                {
                    if (exist.dept_name == "Delivery")
                    {
                        string Delivery_Type = string.Join(",", model.Delivery_Type);

                        db.edit_delivery_entry(model.IPD_Id_Edit, model.Room_Type, model.Addmission_Date, model.Delivery_Date, model.Delivery_Time, model.Delivery_Diagnosis, Delivery_Type, model.Discharge_Date, model.Baby_Gender, model.Baby_Weight, total, Concession);
                    }
                    else if (exist.dept_name == "Operation")
                    {
                        string Operation_Diagnosis = string.Join(",", model.Operation_Diagnosis);
                        string Operation_Type = string.Join(",", model.Operation_Type);
                        db.edit_operation_entry(model.IPD_Id_Edit, model.Room_Type, model.Addmission_Date, model.Operation_Date, Operation_Diagnosis, Operation_Type, model.Discharge_Date, total, Concession);
                    }
                    else if (exist.dept_name == "General")
                    {
                        string General_Diagnosis = string.Join(",", model.General_Diagnosis);
                        db.edit_general_entry(model.IPD_Id_Edit, model.Room_Type, model.Addmission_Date, General_Diagnosis, model.Discharge_Date, total, Concession);
                    }

                    for (int i = 0; i < Charge_Id.Count; i++)
                    {
                        db.Edit_IPD_Charge_Table(model.IPD_Id_Edit, Charge_Id[i], day[i], rate[i], amt[i]);
                    }
                    TempData["successmsg"] = "Record Updated Successfully";
                }

            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
            }

            return RedirectToAction("IPD_Entry");
        }
        public ActionResult Manage_IPD_Entry()
        {
            if (Request.Cookies["User"] != null)
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
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public string Getall_IPD_Entry(GridSettings grid)
        {
            var query = from ipd in db.tbl_Ipd
                        join patient in db.tbl_patient on ipd.patient_id equals patient.patient_Id
                        select new { ipd.ipd_Id, ipd.ipd_receipt_id, patient.full_name, ipd.dept_name, ipd.addmission_date, ipd.discharge_date, ipd.bill, ipd.conssesion };

            int count;
            var data = query.GridCommonSettings(grid, out count);

            var result = new
            {
                total = (int)Math.Ceiling((double)count / grid.PageSize),
                page = grid.PageIndex,
                records = count,
                rows = (from i in data
                        select new
                        {
                            ipd_Id = i.ipd_Id,
                            full_name = i.full_name,
                            dept_name = i.dept_name,
                            addmission_date = i.addmission_date,
                            discharge_date = i.discharge_date,
                            Bill = i.bill ?? 0,
                            Concession = i.conssesion ?? 0,
                            Final_Amount = (i.bill - i.conssesion) ?? 0,
                            action = i.ipd_Id
                        }).ToArray()
            };
            return JsonConvert.SerializeObject(result, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
        }


        public ActionResult Charge_Detail()
        {
            return View();
        }
        public ActionResult Fetch_IPD_Details(int? ipd_id, string ipd_type)
        {
            var result = "";
            if (ipd_type == "Delivery")
            {
                var delivery_detail = from ipd in db.tbl_Ipd
                                      join patient in db.tbl_patient on ipd.patient_id equals patient.patient_Id
                                      join delivery in db.tbl_delivery on ipd.ipd_Id equals delivery.Ipd_Id
                                      where ipd.ipd_Id == ipd_id
                                      select new { patient.patient_Id, patient.full_name, ipd.addmission_date, ipd.discharge_date, ipd.dept_name, delivery.delivery_date, delivery.delivery_time, delivery.delivery_typeId, delivery.diagnosis, delivery.baby_gender, delivery.baby_weight, ipd.ipd_Id, ipd.ipd_receipt_id, patient.address, ipd.room_type, ipd.conssesion };


                foreach (var item in delivery_detail)
                {
                    result = item.patient_Id + "|" + item.full_name + "|" + Convert.ToDateTime(item.addmission_date).ToString("dd/MM/yyyy") + "|" + item.dept_name + "|" + Convert.ToDateTime(item.discharge_date).ToString("dd/MM/yyyy") + "|" + Convert.ToDateTime(item.delivery_date).ToString("dd/MM/yyyy") + "|" + item.delivery_time + "|" + item.delivery_typeId + "|" + item.diagnosis + "|" + item.baby_gender + "|" + item.baby_weight + "|" + item.ipd_Id + "|" + item.ipd_receipt_id + "|" + item.address + "|" + item.room_type + "|" + item.conssesion;
                }
            }
            else if (ipd_type == "Operation")
            {
                var operation_detail = from ipd in db.tbl_Ipd
                                       join patient in db.tbl_patient on ipd.patient_id equals patient.patient_Id
                                       join operation in db.tbl_operation on ipd.ipd_Id equals operation.Ipd_Id
                                       where ipd.ipd_Id == ipd_id
                                       select new { patient.patient_Id, patient.full_name, ipd.addmission_date, ipd.discharge_date, ipd.dept_name, operation.operation_date, operation.diagnosis, operation.operation_type, ipd.ipd_Id, ipd.ipd_receipt_id, patient.address, ipd.room_type, ipd.conssesion };

                foreach (var item in operation_detail)
                {
                    result = item.patient_Id + "|" + item.full_name + "|" + Convert.ToDateTime(item.addmission_date).ToString("dd/MM/yyyy") + "|" + item.dept_name + "|" + Convert.ToDateTime(item.discharge_date).ToString("dd/MM/yyyy") + "|" + Convert.ToDateTime(item.operation_date).ToString("dd/MM/yyyy") + "|" + item.diagnosis + "|" + item.operation_type + "|" + item.ipd_Id + "|" + item.ipd_receipt_id + "|" + item.address + "|" + item.room_type + "|" + item.conssesion;
                }
            }
            else
            {
                var general_detail = from ipd in db.tbl_Ipd
                                     join patient in db.tbl_patient on ipd.patient_id equals patient.patient_Id
                                     join general in db.tbl_general on ipd.ipd_Id equals general.Ipd_Id
                                     where ipd.ipd_Id == ipd_id
                                     select new { patient.patient_Id, patient.full_name, ipd.addmission_date, ipd.discharge_date, ipd.dept_name, general.general_diagnosis, ipd.ipd_Id, ipd.ipd_receipt_id, patient.address, ipd.room_type, ipd.conssesion };

                foreach (var item in general_detail)
                {
                    result = item.patient_Id + "|" + item.full_name + "|" + Convert.ToDateTime(item.addmission_date).ToString("dd/MM/yyyy") + "|" + item.dept_name + "|" + Convert.ToDateTime(item.discharge_date).ToString("dd/MM/yyyy") + "|" + item.general_diagnosis + "|" + item.ipd_Id + "|" + item.ipd_receipt_id + "|" + item.address + "|" + item.room_type + "|" + item.conssesion;
                }
            }
            return Json(result);
        }
        public ActionResult Edit_Charges_Table(int ipd_id)
        {
            ViewBag.total = db.IPD_Charge_Details.Where(m => m.IPD_Id == ipd_id).Sum(x => x.Amount);
            ViewBag.conssesion = db.tbl_Ipd.Where(m => m.ipd_Id == ipd_id).Sum(x => x.conssesion);
            ViewBag.Final_Amount = ViewBag.total - ViewBag.conssesion;

            return View(db.Get_IPD_Charge_Tabele().Where(m => m.IPD_Id == ipd_id).ToList());
        }
        public ActionResult Modal_Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Modal_Delete(IPDModel model)
        {
            try
            {
                db.delete_ipd_entry(model.IPD_Id_Delete);
                TempData["successmsg"] = "Record Updated Successfully";
            }
            catch (Exception ex)
            {
                TempData["errormsg"] = ex.Message;
            }

            return View();
        }
        public ActionResult GetLastIPD()
        {
            var a = db.tbl_Ipd.OrderByDescending(c => c.ipd_Id).FirstOrDefault();
            var IPD_Id_Edit = 0;
            if (a != null)
            {
                IPD_Id_Edit = a.ipd_Id + 1;
            }
            else
            {
                IPD_Id_Edit = 1;
            }
            return Json(IPD_Id_Edit);
        }
        public ActionResult IPD_Invoice(IPDModel model)
        {
            return View(model);
        }
        public ActionResult Print_Charges_Table(int ipd_id)
        {
            ViewBag.total = db.IPD_Charge_Details.Where(m => m.IPD_Id == ipd_id).Sum(x => x.Amount);
            var charge = db.Get_IPD_Charge_Tabele().Where(m => m.IPD_Id == ipd_id);
            return View(charge);
        }


        public JsonResult Check_Exist_IPD(int? IPD_Id_Edit)
        {

            var exist = db.tbl_Ipd.FirstOrDefault(m => m.ipd_Id == IPD_Id_Edit);

            if (exist != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult Manage_DeliveryType()
        {
            if (Request.Cookies["User"] != null)
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
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult AddEdit_DeliveryType(int? id, string page)
        {
            AdminModel model = new AdminModel();
            model.page = Convert.ToInt32(page);
            if (id != null)
            {
                var user = db.delivery_master.FirstOrDefault(m => m.delivery_typeId == id);
                model.Delivery_Type = user.delivery;

                // 
                return View(model);
            }
            //  
            return View(model);

        }
        public ActionResult DeliveryTypeGrid(int? page)
        {
            if (page == 0)
            {
                page = 1;
            }
            return View(db.delivery_master.OrderBy(m => m.delivery).ToList().ToPagedList(page ?? 1, 10));
        }

        [HttpPost]
        public ActionResult AddEdit_DeliveryType(AdminModel model, int? id, string c, string a)
        {
            if (id != null)
            {
                var user = db.delivery_master.FirstOrDefault(m => m.delivery_typeId == id);
                user.delivery = model.Delivery_Type;
            }
            else
            {
                delivery_master table = new delivery_master();
                table.delivery = model.Delivery_Type;
                db.delivery_master.Add(table);
            }

            db.SaveChanges();
            return RedirectToAction("DeliveryTypeGrid", "IPD", new { page = model.page });
        }
        public ActionResult Delete_DeliveryType(int? id, int? page)
        {
            var user = db.delivery_master.FirstOrDefault(m => m.delivery_typeId == id);
            db.delivery_master.Remove(user);
            db.SaveChanges();
            return RedirectToAction("DeliveryTypeGrid", "IPD", new { page = page });
        }
        public ActionResult Manage_Delivery_Operation_Type()
        {
            if (Request.Cookies["User"] != null)
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
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult Manage_OperationType()
        {
            if (Request.Cookies["User"] != null)
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
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult AddEdit_OperationType(int? id, int? page)
        {
            AdminModel model = new AdminModel();
            if (id != null)
            {
                var user = db.operation_master.FirstOrDefault(m => m.operation_typeId == id);
                model.Operation_Type = user.operation_type;
                model.page = Convert.ToInt32(page);
                // 
                return View(model);
            }
            //
            return View();
        }
        public ActionResult OperationTypeGrid(int? page)
        {
            if (page == 0)
            {
                page = 1;
            }
            var type = db.operation_master.OrderBy(m => m.operation_type).ToList().ToPagedList(page ?? 1, 10);
            return View(type);
        }
        [HttpPost]
        public ActionResult AddEdit_OperationType(AdminModel model, int? id, string a)
        {
            if (id != null)
            {
                var user = db.operation_master.FirstOrDefault(m => m.operation_typeId == id);
                user.operation_type = model.Operation_Type;
            }
            else
            {
                operation_master table = new operation_master();
                table.operation_type = model.Operation_Type;
                db.operation_master.Add(table);
            }
            db.SaveChanges();
            return RedirectToAction("OPerationTypeGrid", "IPD", new { page = model.page });
        }
        public ActionResult Delete_OperationType(int? id, int? page)
        {
            var user = db.operation_master.FirstOrDefault(m => m.operation_typeId == id);
            db.operation_master.Remove(user);
            db.SaveChanges();
            return RedirectToAction("OPerationTypeGrid", "IPD", new { page = page });
        }
        public ActionResult Manage_OperationDiagnosis()
        {
            if (Request.Cookies["User"] != null)
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
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult AddEdit_OperationDiagnosis(int? id, int? page)
        {
            AdminModel model = new AdminModel();
            if (id != null)
            {
                var user = db.diagnosis_master.FirstOrDefault(m => m.digagnosis_typeId == id);
                model.Operation_Diagnosis = user.diagnosis_type;
                model.page = Convert.ToInt32(page);
                //
                return View(model);
            }
            // 
            return View();

        }
        public ActionResult OperationDiagnosisGrid(int? page)
        {
            if (page == 0)
            {
                page = 1;
            }
            return View(db.diagnosis_master.OrderBy(m => m.diagnosis_type).ToList().ToPagedList(page ?? 1, 10));
        }
        [HttpPost]
        public ActionResult AddEdit_OperationDiagnosis(AdminModel model, int? id, string a)
        {
            if (id != null)
            {
                var user = db.diagnosis_master.FirstOrDefault(m => m.digagnosis_typeId == id);
                user.diagnosis_type = model.Operation_Diagnosis;
            }
            else
            {
                diagnosis_master table = new diagnosis_master();
                table.diagnosis_type = model.Operation_Diagnosis;
                db.diagnosis_master.Add(table);
            }
            db.SaveChanges();
            return RedirectToAction("OperationDiagnosisGrid", "IPD", new { page = model.page });
        }
        public ActionResult Delete_OperationDiagnosis(int? id, int? page)
        {
            var user = db.diagnosis_master.FirstOrDefault(m => m.digagnosis_typeId == id);
            db.diagnosis_master.Remove(user);
            db.SaveChanges();
            return RedirectToAction("OperationDiagnosisGrid", "IPD", new { page = page });
        }
        public ActionResult Manage_Operation_General_Diagnosis()
        {
            if (Request.Cookies["User"] != null)
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
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult Manage_GeneralDiagnosis()
        {
            if (Request.Cookies["User"] != null)
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
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }
        public ActionResult AddEdit_GeneralDiagnosis(int? id, int? page)
        {
            AdminModel model = new AdminModel();
            if (id != null)
            {
                var user = db.general_diagnosis.FirstOrDefault(m => m.general_diagnosis_Id == id);
                model.General_Diagnosis = user.general_diagnosis_name;
                model.page = Convert.ToInt32(page);
                // 
                return View(model);
            }
            // 
            return View();

        }
        public ActionResult GeneralDiagnosisGrid(int? page)
        {
            if (page == 0)
            {
                page = 1;
            }
            var type = db.general_diagnosis.OrderBy(m => m.general_diagnosis_name).ToList().ToPagedList(page ?? 1, 10);
            return View(type);
        }
        [HttpPost]
        public ActionResult AddEdit_GeneralDiagnosis(AdminModel model, int? id, string a)
        {
            if (id != null)
            {
                var user = db.general_diagnosis.FirstOrDefault(m => m.general_diagnosis_Id == id);
                user.general_diagnosis_name = model.General_Diagnosis;
            }
            else
            {
                general_diagnosis table = new general_diagnosis();
                table.general_diagnosis_name = model.General_Diagnosis;
                db.general_diagnosis.Add(table);
            }
            db.SaveChanges();
            return RedirectToAction("GeneralDiagnosisGrid", "IPD", new { page = model.page });
        }
        public ActionResult Delete_GeneralDiagnosis(int? id, int? page)
        {
            var user = db.general_diagnosis.FirstOrDefault(m => m.general_diagnosis_Id == id);
            db.general_diagnosis.Remove(user);
            db.SaveChanges();
            return RedirectToAction("GeneralDiagnosisGrid", "IPD", new { page = page });
        }
        public JsonResult IsDeliveryTypeExist(String Delivery_Type)
        {
            return Json(!db.delivery_master.Any(m => m.delivery == Delivery_Type), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsOperationTypeExist(String Operation_Type)
        {
            return Json(!db.operation_master.Any(m => m.operation_type == Operation_Type), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsIPDIdExist(int? IPD_Id_Edit, string Get_View)
        {
            var data = true;
            if (Get_View != "Edit")
            {
                data = !db.Get_All_IPD_Entry().Any(m => m.ipd_Id == IPD_Id_Edit);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsGeneralDiagnosisExist(string General_Diagnosis)
        {
            return Json(!db.general_diagnosis.Any(m => m.general_diagnosis_name == General_Diagnosis), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsOperationDiagnosisExist(string Operation_Diagnosis)
        {
            return Json(!db.diagnosis_master.Any(m => m.diagnosis_type == Operation_Diagnosis), JsonRequestBehavior.AllowGet);
        }

        public ActionResult demo(int? page)
        {

            return View(db.Get_All_OPD_Entry());
        }

        public ActionResult get_patient()
        {
            ArrayList result = new ArrayList();
            result.Add(new { id = "frdcsx", text = "fdcx" });
            result.Add(new { id = "frdcsx", text = "fdcx" });
            result.Add(new { id = "frdcsx", text = "fdcx" });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult demo(OPDModel model)
        {
            if (string.IsNullOrEmpty(model.OPD_Date))
            {
                ModelState.AddModelError("OPD_Date", "Error");
            }
            if (ModelState.IsValid)
            {

            }
            return View();
        }
        public JsonResult Getall_Charge_Detail(GridSettings grid, int? id)
        {
            var query = from charge_master in db.IPD_Charges_Master
                        join charge_detail in db.IPD_Charge_Details on charge_master.Charge_Id equals charge_detail.Charge_Id
                        where charge_detail.IPD_Id == id
                        select new { charge_detail.Charge_Id, charge_master.Charge_Title, charge_detail.Days, charge_detail.Rate, charge_detail.Amount };

            var result = new
            {
                rows = query.Select(m => new { m.Charge_Id, m.Charge_Title, m.Days, m.Rate, m.Amount })
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
