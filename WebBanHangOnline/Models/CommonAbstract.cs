using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanHangOnline.Models
{
    public class CommonAbstract
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedrDate { get; set; }
        public string ModifierBy { get; set; }
    }
}