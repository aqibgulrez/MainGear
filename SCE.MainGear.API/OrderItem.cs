//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SCE.MainGear.API
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderItem
    {
        public long OrderItemID { get; set; }
        public long OrderID { get; set; }
        public long SectionItemID { get; set; }
        public long BTOItemID { get; set; }
        public long WholesaleID { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }
}