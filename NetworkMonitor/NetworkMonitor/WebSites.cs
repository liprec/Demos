using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMonitor
{
    public sealed class WebSites
    {
        public string Url { get; set; }
        public string Response { get; set; }
        public int ResponseTime { get; set; }
        public DateTimeOffset RequestDateTime { get; set; }
    }
}
