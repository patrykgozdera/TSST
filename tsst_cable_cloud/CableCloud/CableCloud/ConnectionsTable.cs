using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableCloud
{
    class ConnectionsTable
    {
        public const char SEPARATOR= ' ';
        public const int NODENAME_POSITION = 0;
        public const int INPORT_POSITION = 1;
        public const int OUTPORT_POSITION = 2;
        public const int SOURCE_NODE_NAME_POSITION = 0;
        public const int SOURCE_NODE_PORT_POSITION = 1;
        public const int DESTINATION_NODE_NAME_POSITION = 2;

        private static List<LinksRow> connectionsBetweenNodes;
        private static List<CloudPortsRow> portsBetweenNodesAndCloud;

        public static void init()
        {
            connectionsBetweenNodes = new List<LinksRow>();
            portsBetweenNodesAndCloud = new List<CloudPortsRow>();
            loadPorts();
            loadConnectionsBetweenNodes();
        }

        private static void loadPorts()
        {
            StreamReader reader=getFileStreamReader(Config.getProperty("CloudPortsFile"));
            String textLine = null;
            String[] parameters = null;
            String nodeName = null;
            String inPort = null;
            String outPort = null;
            while ((textLine=reader.ReadLine())!= null)
                {
                    parameters = textLine.Split(SEPARATOR);
                    nodeName = parameters[NODENAME_POSITION];
                    inPort = parameters[INPORT_POSITION];
                    outPort = parameters[OUTPORT_POSITION];        
                    CloudPortsRow row = createCloudPortsRow(nodeName, int.Parse(inPort), int.Parse(outPort));
                    portsBetweenNodesAndCloud.Add(row);
                }
        }
        
        private static void loadConnectionsBetweenNodes()
        {
            StreamReader reader = getFileStreamReader(Config.getProperty("NodeLinksFile"));
            String textLine = null;
            String[] parameters=null;
            String sourceNodeName = null;
            String sourceNodePort = null;
            String destinationNodeName = null;
            while ((textLine = reader.ReadLine())!=null)
            {
                parameters = textLine.Split(SEPARATOR);
                sourceNodeName = parameters[SOURCE_NODE_NAME_POSITION];
                sourceNodePort = parameters[SOURCE_NODE_PORT_POSITION];
                destinationNodeName = parameters[DESTINATION_NODE_NAME_POSITION];
                LinksRow row = createLinksRow(sourceNodeName, int.Parse(sourceNodePort), destinationNodeName);
                connectionsBetweenNodes.Add(row);
            }

            


        }

        private static StreamReader getFileStreamReader(String fileName)
        {
            var fileStream = new System.IO.FileStream(fileName,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read,
                                          System.IO.FileShare.ReadWrite);
            var file = new System.IO.StreamReader(fileStream, System.Text.Encoding.UTF8, true, 128);
            return file;
        }
        private static CloudPortsRow createCloudPortsRow(String nodeName, int inPort, int outPort)
        {
            CloudPortsRow row = new CloudPortsRow(nodeName, inPort, outPort);
            return row;
        }
        private static LinksRow createLinksRow(String sourceNodeName, int sourcePort, String destinationNode)
        {
            LinksRow row = new LinksRow(sourceNodeName, sourcePort, destinationNode);
            return row;
        }

        public static String getDestinationNodeName(String sourceNodeName, int sourcePort)
        {
            String destinationNodeName = null;
            for (int i=0; i<connectionsBetweenNodes.Count; i++)
            {
                LinksRow checkedRow = connectionsBetweenNodes.ElementAt(i);
                String nodeName = checkedRow.getSourceNodeName();
                int port = checkedRow.getSourceNodePort();
                if(sourceNodeName.Equals(nodeName) && port==sourcePort)
                {
                    destinationNodeName = checkedRow.getDestinationNodeName();
                }
            }
            return destinationNodeName;
        }
        public static List<CloudPortsRow> getPortsList()
        {
            return portsBetweenNodesAndCloud;
        }
    }
}
