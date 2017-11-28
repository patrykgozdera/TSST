using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabelSwitchingRouter;

namespace LabelSwitchingRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            LabelSwitchingRouter lsr = new LabelSwitchingRouter();
            InputManager inputManager = new InputManager();
            inputManager.ProcessPackage += lsr.passToInModule;
            inputManager.waitForInput();
        }

        
    }
}
