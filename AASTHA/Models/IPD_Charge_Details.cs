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
    
    public partial class IPD_Charge_Details
    {
        public int Id { get; set; }
        public Nullable<int> IPD_Id { get; set; }
        public Nullable<int> Charge_Id { get; set; }
        public Nullable<double> Days { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<int> Amount { get; set; }
    
        public virtual tbl_Ipd tbl_Ipd { get; set; }
    }
}