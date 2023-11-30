using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Entity.Blockroom
{
    public class CloudBed_BlockroomEntity
    {
        public string roomBlockID { get; set; }
        public string propertyID { get; set; }
        public string roomBlockType { get; set; }
        public string roomBlockReason { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public List<Blockroom.Room> rooms { get; set; }
        public string version { get; set; }
        public string Event { get; set; }
        public double timestamp { get; set; }
    }

    public class Room
    {
        public string roomID { get; set; }
        public int roomTypeID { get; set; }
    }
}
