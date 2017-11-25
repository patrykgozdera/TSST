using LabelSwitchingRouter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class LabelSwitchingRouter
    {
        FIB fib;
        List<InOutModule> inModules;
        List<InOutModule> outModules;
        int numberOfInputModules, numberOfOutputModules;

        public LabelSwitchingRouter()
        {
            fib = new FIB();
            numberOfInputModules = getInputModulesNumber();
            numberOfOutputModules = getOutputModulesNumber();
            inModules = new List<InOutModule>();
            outModules = new List<InOutModule>();

            createInModules(numberOfInputModules);
            createOutModules(numberOfOutputModules);
        }
        private int getInputModulesNumber()
        {
            int number = Config.getIntegerProperty("NumberOfInputModules");
            return number;
        }
        private int getOutputModulesNumber()
        {
            int number = Config.getIntegerProperty("NumberOfOutputModules");
            return number;

        }

        private void createOutModules(int numberOfOutputModules)
        {
            for (int i = 0; i < numberOfOutputModules; i++)
            {
                String interfaceAddress = Config.getProperty("InModuleAddress" + i);
                outModules.Add(new OutModule(interfaceAddress));
            }

        }

        private void createInModules(int numberOfInputModules)
        {
            for (int i = 0; i < numberOfInputModules; i++)
            {
                String interfaceAddress = Config.getProperty("OutModuleAddress" + i);
                inModules.Add(new InModule(interfaceAddress, fib.ReturnSubTable(interfaceAddress)));
            }
        }

    }
}
