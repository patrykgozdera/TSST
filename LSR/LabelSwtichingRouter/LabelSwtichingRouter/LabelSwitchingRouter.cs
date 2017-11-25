using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class LabelSwitchingRouter
    {
        static int ip;
        private List<InOutModule> inModules, outModules;
        private FIB fib;

        public LabelSwitchingRouter() {
            inModules = new List<InOutModule>();
            outModules = new List<InOutModule>();
            fib = new FIB();

        }

        


    }
}
