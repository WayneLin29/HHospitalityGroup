using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_Customization.Interface
{
    public interface IAPLink
    {
        string APDocType { get; set; }
        string APRefNbr { get; set; }
        int? APLineNbr { get; set; }
    }
}
