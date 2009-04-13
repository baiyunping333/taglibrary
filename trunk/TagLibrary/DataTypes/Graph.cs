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

            for (i = 0; i < graph.Nodes.Count; i++)
                if (graph.Nodes[i].Id == nodeId)
                    return i;

            return -1;
        }


        // prints the graph to console
        public static int printGraph()
        {
            Console.Write("Graph:\n");
            Console.Write("-- # Nodes: {0}\n", numberOfNodes);
            Console.Write("-- # Edges: {0}\n", numberOfArcs);
            Console.Write("-- Length of Time Series: {0}\n", lenghtOfTimeSeries);
            int l, m, n;
            Arc a;
            List<int> tempTravelTimeSeries;
            List<int> tempBestTravelTimeSeries;

            for (n = 0; n < numberOfNodes; n++)
            {
                a = nodes[n].getArcs();
                tempTravelTimeSeries = a.getTravelTimeSeries();
                tempBestTravelTimeSeries = a.getBestTravelTimeSeries();
                Console.Write("\n\n Node: {0}", nodes[n].getId());
                Console.Write(" #Neighbors: {0}\n", nodes[n].NumberOfNeighbours());
                Console.Write("\nNode xx|");
                for (m = 0; m < nodes[n].NumberOfNeighbours(); m++)
                {
                    Console.Write("\nNode {0}|", a.getEndNode());

                    //Print the time-series for arc.
                    for (l = 0; l < lenghtOfTimeSeries; l++)
                        Console.Write(" {0}", tempTravelTimeSeries[l]);

                    //Print the best travel time-series for arc.
                    Console.Write(" | ");
                    for (l = 0; l < lenghtOfTimeSeries; l++)
                        Console.Write(" {0}", tempBestTravelTimeSeries[l]);

                    // point to next arc 
                    a = a.getArcNext();
                    tempTravelTimeSeries = a.getTravelTimeSeries();
                    tempBestTravelTimeSeries = a.getBestTravelTimeSeries();
                }
            }
            Console.Write("\n");
            return 1;
        }

        /* APIs to access Graph properties: */
        // Time to reach nth neighbor from node_idx, starting at time 'distance'
        int getWeight(int nodeIndex, int distance, int numberNeighbor)
        {
            //   Console.Write("\nNode ID: {0} {1} {2}\n", nodeIndex, distance, numberNeighbor);
            Arc a = nodes[nodeIndex].getArcs();
            List<int> tempBestTravelTimeSeries = a.getBestTravelTimeSeries();
            List<int> tempTravelTimeSeries = a.getTravelTimeSeries();

            while (numberNeighbor != 0)
            {
                a = a.getArcNext();
                numberNeighbor--;
            }
            // Console.Write("starts at {0} Arc ends at: {1} :: Time {2}\n",nodeIndex , a.getEndNode(), distance);
            int timeSeriesIndex = distance - 1;
            if (timeSeriesIndex >= numberOfArcs)
                return -1;
            int wait = 0;
            while (tempBestTravelTimeSeries[timeSeriesIndex] == -1)
            {
                timeSeriesIndex++;
                wait++;
            }

            //check for bounds
            if (timeSeriesIndex > lenghtOfTimeSeries)
                return -1;
            else
            {
                //the weight should also include the waiting period at the node.
                // Console.Write("Index and Weight: {0} {1}\n",timeSeriesIndex, tempTravelTimeSeries[tempBestTravelTimeSeries[timeSeriesIndex]] + wait);
                return tempTravelTimeSeries[tempBestTravelTimeSeries[timeSeriesIndex]] + wait;
            }
        }

        // Load the Graph from the modified DIMACS input file
        // Highly incomplete as on 20090412 - Santhosh
        int loadGraph(char[] fileName)
        {
            /* 1. Open the input modified DIMACS format file.
             * 2. Parse the file and populate data structures.
             * 3. Close the file.
             */

            char[] text = new char(80);
            int letter;
            int nodeIndex = 0;
            int timeIndex, timeWeight;
            int m, n, numberNode = 0, returnVal;
            int node1, node2;
            int nodeCapacity, arcCapacity;

            // <!-- Ask Chintan -->
            graph = (tag_graph*)malloc(sizeof(tag_graph));
            FILE* f = fopen(fileName, "r");

            if (f == NULL)
            {
                Console.Write("Error in opening file.\n");
                return -1;
            }


            // Parse the DIMACS file to populate data structures. 
            while ((letter = fgetc(f)) != EOF)
            {
                //Console.Write( "%c", letter); fflush(stdout);
                switch (letter)
                {

                    // comment
                    case 'c':
                        while (fgetc(f) != '\n') ;
                        break;

                    // properties of the graph: #nodes, #arcs, #time series units
                    case 'p':
                        fscanf(f, " %s %d %d %d ", &text, &numberOfNodes, &graph->n_arcs, &graph->length_time_series);
                        graph->nodes = (tag_node*)malloc(sizeof(tag_node) * numberOfNodes);
                        //Console.Write("Graph: {0} {1} {2}", numberOfNodes, numberOfArcs, lenghtOfTimeSeries);
                        for (m = 0; m < numberOfNodes; m++)
                        {
                            nodes[m].setId(-1);
                            nodes[m].setIntialOcuupancy(-1);
                            nodes[m].setNumberOfNeighbours(0);
                            nodes[m].setArcs(NULL);
                        }
                        break;

                    // just ignore "n" for now; use it if capacity is considered
                    // also, it can have time-dependent information.
                    case 'n':
                        while (fgetc(f) != '\n') ;
                        break;
                    //if initial occupancy is to be considered
                    /*
                    int node_id, nodeCapacity;
                    fscanf(f," %d %d",&node_id, &nodeCapacity);
                    node[node_id].init_occupancy = nodeCapacity;
                    */

                    // edge(arc) information
                    case 'a':
                        fscanf(f, " %d", &node1);
                        fscanf(f, " %d", &node2);
                        // fscanf(f, " %d", &arcCapacity); //not used for now

                        tag_arc* arc_ptr = malloc(sizeof(tag_arc));

                        arc_ptr->end = node2;
                        arc_ptr->next = NULL;
                        arc_ptr->travel_time_series = malloc(graph->length_time_series * sizeof(int));
                        // Index of the time instant that has the shortest travel time
                        // Greedy choice
                        arc_ptr->best_travel_time_series = malloc(graph->length_time_series * sizeof(int));

                        // Initialize the time series to -1.
                        for (m = 0; m < graph->length_time_series; m++)
                        {
                            arc_ptr->travel_time_series[m] = -1;
                            arc_ptr->best_travel_time_series[m] = -1;
                        }
                        timeIndex = 0;
                        //------------------------------------------------------
                        // Read the time-series data from the file
                        while (fscanf(f, " %d", &timeWeight) != EOF)
                        {
                            //                       if(fscanf(f," %d", &timeWeight)==EOF) break;

                            arc_ptr->travel_time_series[timeIndex++] = timeWeight; // timeIndex starts from 1
                            if (!(timeIndex % (graph->length_time_series)))
                            {
                                preProcessArc(arc_ptr);
                                break;
                            }
                        }



                        tag_arc* a;

                        nodeIndex = getNodeIDX(node2);
                        if (nodeIndex == -1)
                        {
                            nodeIndex = numberNode;
                            graph->nodes[nodeIndex].id = node2;
                            numberNode++;
                        }

                        nodeIndex = getNodeIDX(node1);
                        if (nodeIndex == -1)
                        {
                            nodeIndex = numberNode;
                            graph->nodes[nodeIndex].id = node1;
                            numberNode++;
                        }


                        /* Attach arc to correspoing node */
                        if (graph->nodes[nodeIndex].n_neighbors == 0)
                        {
                            graph->nodes[nodeIndex].list_arcs = arc_ptr;
                        }
                        else
                        {
                            a = graph->nodes[nodeIndex].list_arcs;
                            while (a->next != NULL) a = a->next;
                            a->next = arc_ptr;
                        }
                        graph->nodes[nodeIndex].n_neighbors++;
                        while (fgetc(f) != '\n') ;
                        break;
                }
            }
            fclose(f);
            // printGraph();
        }



    }
}
