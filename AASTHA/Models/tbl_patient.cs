//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AASTHA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_patient
    {
        public tbl_patient()
        {
            this.tbl_appointment = new HashSet<tbl_appointment>();
            this.tbl_Ipd = new HashSet<tbl_Ipd>();
            this.tbl_opd = new HashSet<tbl_opd>();
            this.tbl_opd1 = new HashSet<tbl_opd>();
        }
    
        public int patient_Id { get; set; }
        public string full_name { get; set; }
        public string address { get; set; }
        public Nullable<decimal> mobile { get; set; }
        public Nullable<decimal> age { get; set; }
        public string opd_id123 { get; set; }
    
        public virtual ICollection<tbl_appointment> tbl_appointment { get; set; }
        public virtual ICollection<tbl_Ipd> tbl_Ipd { get; set; }
        public virtual ICollection<tbl_opd> tbl_opd { get; set; }
        public virtual ICollection<tbl_opd> tbl_opd1 { get; set; }
    }
}
