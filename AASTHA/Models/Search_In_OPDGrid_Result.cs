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
    
    public partial class Search_In_OPDGrid_Result
    {
        public int opd_Id { get; set; }
        public string opd_receipt_id { get; set; }
        public Nullable<int> patient_id { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string case_type { get; set; }
        public Nullable<int> consult_charge { get; set; }
        public Nullable<int> usg_charge { get; set; }
        public Nullable<int> upt_charge { get; set; }
        public Nullable<int> inj_charge { get; set; }
        public Nullable<int> other_charge { get; set; }
        public string full_name { get; set; }
        public Nullable<int> Total { get; set; }
    }
}