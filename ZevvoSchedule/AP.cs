using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZevvoSchedule
{
    class AP
    {
        public string Name { get; set; }
        public bool EligibleMain { get; set; }
        public bool EligibleBackup { get; set; }
        public bool Available { get; set; }

        public AP(string name)
        {
            Name = name;
            ResetAvailability();
            ResetMain();
        }

        // Per week
        public void ResetAvailability()
        {
            Available = true;
        }

        // Per rotation
        public void ResetMain()
        {
            EligibleMain = true;
            EligibleBackup = true;
        }
    }
}
