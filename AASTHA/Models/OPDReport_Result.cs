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
    
    public partial class OPDReport_Result
    {
        public int opd_Id { get; set; }
        public string opd_receipt_id { get; set; }
        public Nullable<int> patient_id { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string case_type { get; set; }
        public int Consult_Charge { get; set; }
        public int USG_Charge { get; set; }
        public int UPT_Charge { get; set; }
        public int Inj_Charge { get; set; }
        public int Other_Charge { get; set; }
        public string full_name { get; set; }
        public int Total { get; set; }
    }
}
