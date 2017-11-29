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

        public class Entry
        {
            public int InPort { get; set; }
            public int OutPort { get; set; }
            public int InLabel { get; set; }
            public int OutLabel { get; set; }
            public int NewLabel { get; set; }
            public int RemoveLabel { get; set; }
            public String IPAddress { get; set; }

            public Entry(int ip, int il, int op, int ol, int nl, int rl, String address)
            {
                InPort = ip;
                OutPort = op;
                InLabel = il;
                OutLabel = ol;
                NewLabel = nl;
                RemoveLabel = rl;
                IPAddress = address;
            }
        }

        public FIB()
        {
            routingTable = new List<Entry>();
            AddEntry(2, 3, 7, 4, 0, 0, "AkademikRiviera");
            AddEntry(2, 8, 8, 2, 0, 0, "");
            AddEntry(2, 5, 6, 1, 0, 0, "");

        }

        public FIB(List<Entry> rtable) : this()
        {
            routingTable = rtable;
        }

        public void UpdatePortsRoutingTables(List<InPort> ports)
        {
            foreach (InPort port in ports) {
                int inPort = port.GetPortNumber();
                UpdateRoutingTable(ReturnSubTable(inPort).routingTable);
            }
        }

        private void UpdateRoutingTable(List<Entry> routingTable)
        {
            this.routingTable = routingTable;
        }

        public void AddEntry(int inport, int inlabel, int outport, int outlabel, int newlabel, int removelabel, String address)
        {
            if (!routingTable.Contains(FindInput(inport, inlabel)))
            {
                routingTable.Add(new Entry(inport, inlabel, outport, outlabel, newlabel, removelabel, address));
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
            Entry result = routingTable.FindAll(x => x.InPort == iport).Find(x => x.InLabel == ilabel);
            int[] outPair = { result.OutPort, result.OutLabel };
            return outPair;
        }

        public int LookForLabelToBeAdded(int iport, int ilabel)
        {
            Entry result = FindInput(iport, ilabel);
            int label = result.NewLabel;
            return label;
        }

        public int LookForLabelToBeRemoved(int iport, int ilabel)
        {
            Entry result = FindInput(iport, ilabel);
            int label = result.RemoveLabel;
            return label;
        }

        public int ExchangeIpAddressForLabel(String ipaddress)
        {
            Entry result = routingTable.Find(x => x.IPAddress == ipaddress);
            int label = result.InLabel;
            return label;
        }

    }
}
