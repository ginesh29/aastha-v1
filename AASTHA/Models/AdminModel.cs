using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Models
{
    public class AdminModel
    {
        [Required]
        [Remote("IsUserExist", "Admin", ErrorMessage = "Username is Already Exist")]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public IList<SelectListItem> Role_List { get; set; }
        [Required]
        public string Layout { get; set; }
        [Required]
        [StringLength(8, ErrorMessage = "Biometric Id Must be 8 Character Length", MinimumLength = 1)]
        public string Biometric_Id { get; set; }
        public IList<SelectListItem> Layout_List { get; set; }
        public bool Remember { get; set; }
        [Required]
        [Remote("IsAdviceExist", "IPD", ErrorMessage = "Advice is Already Exist")]
        public string Advice_Text { get; set; }
        [Required]
        [Remote("IsDeliveryTypeExist", "IPD", ErrorMessage = "Delivery Type is Already Exist")]
        public string Delivery_Type { get; set; }
        [Required]
        [Remote("IsOperationTypeExist", "IPD", ErrorMessage = "Operation Diagnosis is Already Exist")]
        public string Operation_Type { get; set; }
        [Required]
        [Remote("IsGeneralDiagnosisExist", "IPD", ErrorMessage = "General Diagnosis is Already Exist")]
        public string General_Diagnosis { get; set; }
        [Required]
        [Remote("IsOperationDiagnosisExist", "IPD", ErrorMessage = "Operation Type is Already Exist")]
        public string Operation_Diagnosis { get; set; }
        [Required]
        [Remote("IsMedicineTypeExist", "Admin", AdditionalFields = "ViewOperation", ErrorMessage = "Medicine Type is Already Exist")]
        public string Medicine_Type { get; set; }
        [Required]
        [Remote("IsMedicineExist", "Admin", AdditionalFields = "Medicine_TypeId,ViewOperation", ErrorMessage = "Already Exist")]

        public string Medicine_Name { get; set; }
        [Required(ErrorMessage = "Select Type")]

        public int? Medicine_TypeId { get; set; }
        public IList<SelectListItem> Medicine_Type_List { get; set; }
        public IList<SelectListItem> Appointment_Type_List { get; set; }

        public string Medicine_Fullname { get; set; }
        [Required]
        public int? Patient_Id { get; set; }
        [Required]
        public string opd_date { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Numberic Only ")]
        public int? day { get; set; }
        //[Remote("IsAppointedPatient", "Admin", AdditionalFields = "Patient_Id", ErrorMessage = "Patient Already Appointed")]
        //[RequiredIf("Appointment_Type", "Date", ErrorMessage = "Please Enter Date")]
        public string Followup_date { get; set; }
        //[RequiredIf("IsSonography", "true", ErrorMessage = "Please Enter Date")]
        //[Remote("IsAppointedPatient", "Admin", AdditionalFields = "Patient_Id", ErrorMessage = "Patient Already Appointed")]
        public string Sonography_date { get; set; }
        //public string Followup { get; set; }
        public bool IsSonography { get; set; }
        public string Appointment_Date { get; set; }
        public string Appoint_Date { get; set; }
        [Required]
        //[Remote("IsAppointed", "Admin", AdditionalFields = "Appointment_Date,Patient_Id,Get_View", ErrorMessage = "Patient Already Appointed")]
        public string Appointment_Type { get; set; }
        public List<SelectListItem> Appointment_List { get; set; }

        public string ViewOperation { get; set; }
        public string Get_View { get; set; }
        public int Appointment_Id { get; set; }

        public int page { get; set; }
    }
}