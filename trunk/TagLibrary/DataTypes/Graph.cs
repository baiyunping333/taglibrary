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


        //Preprocess to store the greedy choice for non FIFO
        public void PreProcessArc(Arc edge, Graph graph)
        {
            int minWeight, minWeightIndex;
            int timeSeriesLength = graph.LenghtOfTimeSeries;
            
            minWeight = 10000;
            minWeightIndex = -1;

            edge.BestTravelTimeSeries[timeSeriesLength - 1] = -1;
            
            int j;
            for (j = timeSeriesLength - 1; j >= 0; j--)
            {
                if (edge.TravelTimeSeries[j] != -1 && minWeight > (edge.TravelTimeSeries[j] + (j + 1)))
                {
                    minWeight = edge.TravelTimeSeries[j] + (j + 1);
                    minWeightIndex = j;
                }
                edge.BestTravelTimeSeries[j] = minWeightIndex;
            }
            return;
        }


        // return the index of 'nodeId' from the list of nodes;
        // if nodeId not in the list , return -1.
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
