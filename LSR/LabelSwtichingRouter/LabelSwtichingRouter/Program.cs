using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class Program
    {
        static void Main(string[] args)
        {
            String str=Config.getProperty("a");
            Console.WriteLine(str);
            Console.ReadLine();
        }
    }
}
