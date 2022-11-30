using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Entity
{
    public class TouchBistro_DataReceivedEntity
    {
        public Guid? FileID { get; set; }
        public string FileName { get; set; }
        public string RestaurantCD { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Type { get; set; }

    }
}
