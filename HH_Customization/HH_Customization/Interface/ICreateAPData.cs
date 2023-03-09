using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_Customization.Interface
{
    public interface ICreateAPData
    {
        bool? Selected { get; set; }
        int? VendorID { get; set; }
        int? InventoryID { get; set; }
        int? AccountID { get; set; }
        int? SubID { get; set; }
        decimal? ExtCost { get; set; }

    }
}
