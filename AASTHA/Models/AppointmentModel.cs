using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Models
{
    public class AppointmentModel
    {
        public DateTime Appointment_Date { get; set; }
        [Required]       
        public int? Patient_Id { get; set; }
        [Required]
        [Remote("IsAppointed", "Admin", AdditionalFields = "Appointment_Date,Patient_Id", ErrorMessage = "Patient Already Appointed")]
        public string Appointment_Type { get; set; }
        public List<SelectListItem> Appointment_List { get; set; }

    }
}