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
    
    public partial class medicine_master
    {
        public int medicine_typeId { get; set; }
        public string medicine_name { get; set; }
        public Nullable<int> medicine_type { get; set; }
    
        public virtual tbl_medicine_type tbl_medicine_type { get; set; }
        public virtual tbl_medicine_type tbl_medicine_type1 { get; set; }
    }
}
