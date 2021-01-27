using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnowEmergency.Models.ViewModels
{
    public class SnowEmergencyAlert
    {
        public bool IsThereASnowEmergency { get; set; }
        public string Body { get; set; }
    }
}
