using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AASTHA.Models
{
    public class OPDModel
    {
        public int? OPD_Id { get; set; }
        public int? OPD_Id_Edit { get; set; }
        public int? OPD_Id_Delete { get; set; }
        public int? OPD_Id_Print { get; set; }
        public string OPD_No { get; set; }
        [Required]
        public String OPD_Date { get; set; }

        public int? Cons_Charge { get; set; }
        public int? USG_Charge { get; set; }
        public int? UPT_Charge { get; set; }
        public int? Inj_Charge { get; set; }
        public int? Other_Charge { get; set; }
        public int? Total_Charge { get; set; }
        [Required]
        public int? Patient_Id { get; set; }
        [Required]
        public string Case_Type_Name { get; set; }
        public IList<SelectListItem> OPD_Case_TypeList { get; set; }
        public IList<SelectListItem> PatientList { get; set; }
    }
}