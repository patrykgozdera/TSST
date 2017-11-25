using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class InOutModule
    {
        private String interfaceAddress;
        public delegate void packageIsReadyDelegate();
        public event packageIsReadyDelegate sendPackage;

        public InOutModule(String interfaceAddress)
        {
            this.interfaceAddress = interfaceAddress;
        }
    }
}
