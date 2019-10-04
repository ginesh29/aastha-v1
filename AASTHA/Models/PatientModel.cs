using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Models
{
    public class PatientModel
    {
        public int? Patient_Id { get; set; }
        public int? Patient_Id_Edit { get; set; }
        public int? Patient_Id_Delete { get; set; }

        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Middlename { get; set; }
        [Required]
        [Remote("IsFullnameExist", "Patient", AdditionalFields = "Firstname,Middlename,Lastname,Get_View", ErrorMessage = "Patient is Already Exist")]
        public string Lastname { get; set; }
        [Required]
       
        public string Fullname { get; set; }
        [Required]
        [Range(typeof(int), "1", "100")]
        public int? Age { get; set; }
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Enter Valid Mobile Number")]
        public Decimal? Mobile { get; set; }
        [Required]
        public string Address { get; set; }
        public string Get_View { get; set; }
    }
}