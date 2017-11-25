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
        List<InOutModule> inModules;
        List<InOutModule> outModules;

        public LabelSwitchingRouter()
        {
            int numberOfInputModules = getInputModulesNumber();
            int numberOfOutputModules = getOutputModulesNumber();
            inModules = new List<InOutModule>();
            outModules = new List<InOutModule>();
<<<<<<< HEAD
            fib = new FIB();

            inModules.Add(new InOutModule());

=======
            createInModules(numberOfInputModules);
            createOutModules(numberOfOutputModules);
        }
        private int getInputModulesNumber()
        {
            int number= Config.getIntegerProperty("NumberOfInputModules");
            return number;
        }
        private int getOutputModulesNumber()
        {
            int number=Config.getIntegerProperty("NumberOfOutputModules");
            return number;
>>>>>>> 1ef5590760b524e584a7869318504ba2536f9f84
        }

        
        private void createOutModules(int numberOfOutputModules)
        {
            for(int i=0; i<numberOfOutputModules; i++)
            {
                String interfaceAddress = Config.getProperty("InModuleAddress" + numberOfOutputModules);
                outModules.Add(new InOutModule(interfaceAddress));
            }

        }

        private void createInModules(int numberOfInputModules)
        {
            for (int i = 0; i < numberOfInputModules; i++)
            {
                String interfaceAddress = Config.getProperty("OutModuleAddress" + numberOfInputModules);
                inModules.Add(new InOutModule(interfaceAddress));
            }
        }

    }
}
