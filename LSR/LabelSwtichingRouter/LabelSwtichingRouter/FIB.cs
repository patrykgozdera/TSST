using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class FIB
    {
        private List<Entry> routingTable;
        private List<int[]> ipTable;

        public class Entry
        {
            public int InPort { get; set; }
            public int OutPort { get; set; }
            public int InLabel { get; set; }
            public int OutLabel { get; set; }

            public Entry(int ip, int il, int op, int ol)
            {
                InPort = ip;
                OutPort = op;
                InLabel = il;
                OutLabel = ol;
            }
        }

        public FIB()
        {
            routingTable = new List<Entry>();
        }

        public FIB(List<Entry> rtable) : this()
        {
            routingTable = rtable;
        }

        public void AddEntry(int inport, int inlabel, int outport, int outlabel)
        {
            if (!routingTable.Contains(FindInput(inport, inlabel)))
            {
                routingTable.Add(new Entry(inport, inlabel, outport, outlabel));
                Console.WriteLine("Added new entry in FIB: inport {0} inlabel {1} outport {2} outlabel {3}", inport, inlabel, outport, outlabel);
            }
            else Console.WriteLine("Entry with such input parameters already exists. Delete it before adding new one.");
        }
        public void RemoveEntry(int inport, int inlabel)
        {
            Entry entryToBeDeleted = FindInput(inport, inlabel);
            if (entryToBeDeleted != null)
            {
                routingTable.Remove(entryToBeDeleted);
                Console.WriteLine("Entry:  inport {0} inlabel {1} outport {2} outlabel {3} removed from FIB.",
                    entryToBeDeleted.InPort, entryToBeDeleted.InLabel, entryToBeDeleted.OutPort, entryToBeDeleted.OutLabel);
            }
            else Console.WriteLine("Entry with such input parameters doesn't exist in this FIB.");
        }

        public FIB ReturnSubTable(int inport)
        {
            return new FIB(routingTable.FindAll(x => x.InPort == inport));
        }

        private Entry FindInput(int iport, int ilabel)
        {
            return routingTable.FindAll(x => x.InPort == iport).Find(y => y.InLabel == ilabel);
        }

        public int[] GetOutput(int iport, int ilabel)
        {
            Entry result = routingTable.FindAll(x => x.InPort == iport).Find(y => y.InLabel == ilabel);
            int[] outPair = { result.OutPort, result.OutLabel };
            return outPair;
        }

        public int ExchangeIpAddressForLabel(String ipaddress)
        {
            int address = Int32.Parse(ipaddress);
            int[] pair = ipTable.Find(x => x[1] == address);
            int label = pair[0];
            return label;
        }

    }
}
