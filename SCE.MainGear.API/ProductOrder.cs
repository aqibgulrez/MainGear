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
    
    public partial class ProductOrder
    {
        public long OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public System.DateTime ShippingDate { get; set; }
        public System.DateTime PlacedOn { get; set; }
        public string OrderStatus { get; set; }
    }
}
