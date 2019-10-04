//using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Models
{
    public class IPDModel
    {
        public int? IPD_Id { get; set; }
        public IList<SelectListItem> PatientList { get; set; }
        [Required]
        [Remote("IsIPDIdExist", "IPD",AdditionalFields="Get_View", ErrorMessage = "Already Exist")]
        public int? IPD_Id_Edit { get; set; }
        public int? IPD_Id_Delete { get; set; }
        public int? IPD_Id_Print { get; set; }
        [Required]
        public string IPD_Type { get; set; }
        [Required]
        public int? Patient_Id { get; set; }
        [RequiredIf("IPD_Type", "Delivery", ErrorMessage = "Please Select Atleast One Delivery Type")]
        public string[] Delivery_Type { get; set; }
        [Required]
        public string Room_Type { get; set; }
        [RequiredIf("IPD_Type", "Delivery", ErrorMessage = "Please Select Baby Gender")]
        public string Baby_Gender { get; set; }
        [RequiredIf("IPD_Type", "Delivery", ErrorMessage = "Please Select Baby Weight")]
        public decimal? Baby_Weight { get; set; }
        [RequiredIf("IPD_Type", "Delivery", ErrorMessage = "Please Enter Delivery Diagnosis")]
        public string Delivery_Diagnosis { get; set; }
        [RequiredIf("IPD_Type", "General", ErrorMessage = "Please Select General Diagnosis")]
        public string[] General_Diagnosis { get; set; }
        [RequiredIf("IPD_Type", "Operation", ErrorMessage = "Please Select Atleast One Operation Diagnosis")]
        public string[] Operation_Diagnosis { get; set; }
        [RequiredIf("IPD_Type", "Operation", ErrorMessage = "Please Select Atleast One Operation Type")]
        public string[] Operation_Type { get; set; }
        public IList<SelectListItem> Room_Type_List { get; set; }
        public IList<SelectListItem> Delivery_Diagnosis_List { get; set; }
        public IList<SelectListItem> Baby_Gender_List { get; set; }
        public IList<SelectListItem> IPD_Type_List { get; set; }
        public IList<SelectListItem> Delivery_Type_List { get; set; }
        public IList<SelectListItem> General_Diagnosis_List { get; set; }
        public IList<SelectListItem> Operation_Diagnosis_List { get; set; }
        public IList<SelectListItem> Operation_Type_List { get; set; }
        [Required]
        public string Addmission_Date { get; set; }
        [RequiredIf("IPD_Type", "Delivery", ErrorMessage = "Please Enter Date")]
        public string Delivery_Date { get; set; }
        [RequiredIf("IPD_Type", "Delivery", ErrorMessage = "Please Enter Time")]
        public string Delivery_Time { get; set; }
        [RequiredIf("IPD_Type", "Operation", ErrorMessage = "Please Enter Date")]
        public string Operation_Date { get; set; }
        [Required]
        public string Discharge_Date { get; set; }
        public double Bill { get; set; }
      
        public string Get_View { get; set; }
        
    }
}