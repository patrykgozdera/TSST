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
            Console.WriteLine("Halo");

            LabelSwitchingRouter lsr = new LabelSwitchingRouter();

            Packet testPacket = new Packet("message", "AkademikRiviera", "Jaworzynska", 2, 3, "time", "interface");
            List<MPLSPacket> testList = new List<MPLSPacket>();

            MPLSPacket testmplsPacket1 = new MPLSPacket(testPacket, 3);
            testmplsPacket1.DestinationPort = 2;
            testList.Add(testmplsPacket1);

            //MPLSPacket testmplsPacket2 = new MPLSPacket(testPacket, 5);
            //testmplsPacket2.DestinationPort = 2;
            //testList.Add(testmplsPacket2);

            MPLSPack testPack = new MPLSPack(testList);
            testPack.DestinationPort = 2;
            Console.ReadLine();
            //lsr.PassToInModule(null, testPacket);
            lsr.PassToInModule(null, testPack);


           


        }

        
    }
}
