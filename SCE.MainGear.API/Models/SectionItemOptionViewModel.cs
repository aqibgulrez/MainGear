using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCE.MainGear.API.Models
{
    public class SectionItemOptionViewModel
    {
        public long SectionItemOptionID { get; set; }
        public string OptionName { get; set; }
        public string OptionChoice { get; set; }
        public long SectionItemID { get; set; }
    }
}