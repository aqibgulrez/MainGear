using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCE.MainGear.API.Models
{
    public class ProductOrderVM
    {
        public List<ProductOrderItemVM> Items { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime EstimatedShippingDate { get; set; }
    }


    public class ProductOrderItemVM
    {
        public long SectionItemID { get; set; }
        public long SectionItemOptionID { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
    }

}