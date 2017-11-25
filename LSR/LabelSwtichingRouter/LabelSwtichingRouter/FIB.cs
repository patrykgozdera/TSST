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
            public String InPort { get; set; }
            public String OutPort { get; set; }
            public int InLabel { get; set; }
            public int OutLabel { get; set; }

            public Entry(String ip, int il, String op, int ol)
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

        public void AddEntry(String inport, int inlabel, String outport, int outlabel)
        {
            if (!routingTable.Contains(FindInput(inport, inlabel)))
            {
                routingTable.Add(new Entry(inport, inlabel, outport, outlabel));
                Console.WriteLine("Added new entry in FIB: inport {0} inlabel {1} outport {2} outlabel {3}", inport, inlabel, outport, outlabel);
            }
            else Console.WriteLine("Entry with such input parameters already exists. Delete it before adding new one.");
        }
        public void RemoveEntry(String inport, int inlabel)
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

        public List<Entry> ReturnSubTable(String inport)
        {
            return routingTable.FindAll(x => x.InPort == inport);
        }

        private Entry FindInput(String iport, int ilabel)
        {
            return routingTable.FindAll(x => x.InPort == iport).Find(y => y.InLabel == ilabel);
        }

        public Tuple<String, int> GetOutput(String iport, int ilabel)
        {
            Entry result = routingTable.FindAll(x => x.InPort == iport).Find(y => y.InLabel == ilabel);
            Tuple<String, int> tuple = new Tuple<String, int>(result.OutPort, result.OutLabel);
            return tuple;
        }

    }
}
