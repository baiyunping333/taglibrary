using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TagLibrary.DataTypes
{
    class Graph
    {
        private int numberOfNodes;

        public int NumberOfNodes
        {
            get { return numberOfNodes; }
            set { numberOfNodes = value; }
        }
        private int numberOfArcs;

        public int NumberOfArcs
        {
            get { return numberOfArcs; }
            set { numberOfArcs = value; }
        }
        private int lenghtOfTimeSeries;

        public int LenghtOfTimeSeries
        {
            get { return lenghtOfTimeSeries; }
            set { lenghtOfTimeSeries = value; }
        }
        private List<Node> nodes;
        public List<Node> Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }



        // return the index of 'node_id' from the list of nodes;
        // if node_id not in the list , return -1.
        public int getNodeIndex(int nodeId, Graph graph)
        {
            int i;
            
            for (i=0; i< graph.Nodes.Count; i++)
                if (graph.Nodes[i].Id == nodeId)
                    return i;
            
            return -1;
        }
    }
}
