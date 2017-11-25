using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabelSwtichingRouter;

namespace LabelSwitchingRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            String str=Config.getProperty("a");
            Console.WriteLine(str);
            Console.ReadLine();
            LabelSwitchingRouter lsr = new LabelSwitchingRouter();
            InputManager inputManager = new InputManager();
            inputManager.ProcessPackage += passToInOutModule;
            inputManager.waitForInput();
        }

        private static void passToInOutModule(object oSender, PacketPackage packet)
        {
            throw new NotImplementedException();
        }
    }
}
