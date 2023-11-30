using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Entity
{
    public class CloudBed_SubscribeWebhookEntity
    {
        public bool Success { get; set; }

        public string message { get; set; }

        public Data data { get; set; }
    }

    public class Data
    {
        public string subscriptionID { get; set; }
    }
}
