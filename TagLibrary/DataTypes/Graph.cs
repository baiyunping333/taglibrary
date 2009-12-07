using System.Collections.Generic;
using System.IO;
using System; 


namespace TagLibrary.DataTypes
{
   public class Graph
    {
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

        public Graph()
        {
            nodes = new List<Node>();
        }


        //Preprocess to store the greedy choice for non FIFO
        public void PreProcessArc(ref Arc edge)
        {
            int minWeightIndex;
            float minWeight = 9999;
            minWeightIndex = -1;

            edge.BestTravelTimeSeries[LenghtOfTimeSeries - 1] = -1;

            for (int j = LenghtOfTimeSeries - 1; j >= 0; j--)
            {
                if (edge.TravelTimeSeries[j] != -1 && (minWeight > (edge.TravelTimeSeries[j] + (j + 1))))
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
        public int GetNodeIndex(int nodeId)
        {
            int i;

            for (i = 0; i < Nodes.Count; i++)
                if (Nodes[i].Id == nodeId)
                    return i;

            return -1;
        }

        public void ShortestPaths(int startNodeID, StreamWriter file)
        {
            int i, j;                                   /* counters */
            int[] visited = new int[Nodes.Count];       /* is the node visited? */
            int[] distance = new int[Nodes.Count];      /* minimum distance(time) to each node from start node*/
            int[] parent = new int[Nodes.Count];        /* previous node to reach the current node*/
            //int[] parent_wait = new int[Nodes.Count]; /* waiting time at parent node */
            int v;                           /* current vertex to process */
            int w;                           /* candidate next vertex */
            int weight;                      /* edge weight */
            int dist;                        /* best current distance from start */

            for (i = 0; i < Nodes.Count; i++)
            {
                visited[i] = 0;
                distance[i] = 9999; //INFINITY;
                parent[i] = -1;
                //parent_wait[i] = 0;
            }

            int start = GetNodeIndex(startNodeID);
            List<Arc> aList;
            Arc a;
            distance[start] = 1; // Santhosh - why this has to be 1.. cant it be 0??
            v = start;

            while (visited[v] == 0)
            {
                visited[v] = 1;
                aList = Nodes[v].Arcs;

                //printf(" Examining Node: %d \n", graph->nodes[v].id);
                for (i = 0; i < Nodes[v].NumberOfNeighbours; i++)
                {
                    a = aList[i];
                    w = GetNodeIndex(a.EndNode);

                    weight = GetWeight(v, distance[v], i);
                    //printf("%d, %d, %d\t", a->end, distance[v], weight);

                    if (weight != -1)
                    {
                        if (distance[w] > (distance[v] + weight))
                        {
                            distance[w] = distance[v] + weight;
                            //parent_wait[w] = parent_wait[v];
                            parent[w] = v;
                            Nodes[w].ParentId = Nodes[v].Id;
                            Nodes[w].ParentTime = distance[v];
                            //printf("Weight is less than current minimum\n");
                        }
                    }
                }

                //v = 1;
                dist = 10000; //INFINITY;
                for (i = 0; i < Nodes.Count; i++)
                    if ((visited[i] == 0) && (dist > distance[i]))
                    {
                        dist = distance[i];
                        v = i;
                    }
            }

            for (i = 0; i < Nodes.Count; i++)
            {
                file.WriteLine(string.Format("{0},{1},{2},{3},{4}", startNodeID, Nodes[i].Id, distance[i], Nodes[i].ParentId, nodes[i].ParentTime));
                //Console.WriteLine(string.Format("{0},{1},{2},{3},{4}", startNodeID, Nodes[i].Id, distance[i], Nodes[i].ParentId, nodes[i].ParentTime));
            }
        }

        public bool ShortestPaths(int startNodeID, int endNodeID, StreamWriter file)
        {
            int i, j;                                   /* counters */
            int[] visited = new int[Nodes.Count];       /* is the node visited? */
            int[] distance = new int[Nodes.Count];      /* minimum distance(time) to each node from start node*/
            int[] parent = new int[Nodes.Count];        /* previous node to reach the current node*/
            //int[] parent_wait = new int[Nodes.Count]; /* waiting time at parent node */
            int v;                           /* current vertex to process */
            int w;                           /* candidate next vertex */
            int weight;                      /* edge weight */
            int dist;                        /* best current distance from start */
            bool spFound = false;

            for (i = 0; i < Nodes.Count; i++)
            {
                visited[i] = 0;
                distance[i] = 9999; //INFINITY;
                parent[i] = -1;
                //parent_wait[i] = 0;
            }

            int start = GetNodeIndex(startNodeID);
            // check start is valid
            int dest = GetNodeIndex(endNodeID);
            // check dest is valid

            List<Arc> aList;
            Arc a;
            distance[start] = 1; // Santhosh - why this has to be 1.. cant it be 0??
            v = start;

            while (visited[v] == 0)
            {
                visited[v] = 1;
                aList = Nodes[v].Arcs;

                //printf(" Examining Node: %d \n", graph->nodes[v].id);
                for (i = 0; i < Nodes[v].NumberOfNeighbours; i++)
                {
                    a = aList[i];
                    w = GetNodeIndex(a.EndNode);

                    weight = GetWeight(v, distance[v], i);
                    //printf("%d, %d, %d\t", a->end, distance[v], weight);

                    if (weight != -1)
                    {
                        if (distance[w] > (distance[v] + weight))
                        {
                            distance[w] = distance[v] + weight;
                            //parent_wait[w] = parent_wait[v];
                            parent[w] = v;
                            Nodes[w].ParentId = Nodes[v].Id;
                            Nodes[w].ParentTime = distance[v];
                            //printf("Weight is less than current minimum\n");
                        }
                    }
                }

                //v = 1;
                dist = 10000; //INFINITY;
                for (i = 0; i < Nodes.Count; i++)
                    if ((visited[i] == 0) && (dist > distance[i]))
                    {
                        dist = distance[i];
                        v = i;
                    }
                if (dest != -1 && v == dest)
                {
                    spFound = true;
                    break;
                }
            }

            if (spFound)
            {
                for (i = 0; i < Nodes.Count; i++)
                {
                    if (endNodeID == Nodes[i].Id)
                        file.WriteLine(string.Format("{0},{1},{2},{3},{4}", startNodeID, endNodeID, distance[i], Nodes[i].ParentId, nodes[i].ParentTime));
                    //Console.WriteLine(string.Format("{0},{1},{2},{3},{4}", startNodeID, Nodes[i].Id, distance[i], Nodes[i].ParentId, nodes[i].ParentTime));
                }
            }
            return spFound;
        }

        #region Accessors

        public Node GetNode(int nodeId1, int time)
        {
            // Current structure does not handle time
            return this.nodes.Find(item => item.Id == nodeId1);
        }

        public Node GetNode(int nodeId1)
        {
            // Current structure does not handle time
            return this.nodes.Find(item => item.Id == nodeId1);
        }

        public Arc GetArc(int nodeId1, int nodeId2, int time)
        {
            Node node;
            Arc arc = null;

            node = this.nodes.Find(item => item.Id == nodeId1);

            if (node != null)
            {
                arc = node.Arcs.Find(item => item.EndNode == nodeId2);

                if (arc != null)
                {
                    if (!arc.TravelTimeSeries.Exists(item => item == time))
                        arc = null;
                }
            }

            return arc;

        }

        public Arc GetArc(int nodeId1, int nodeId2)
        {
            Node node;
            Arc arc = null;

            node = this.nodes.Find(item => item.Id == nodeId1);

            if (null != node)
            {
                arc = node.Arcs.Find(item => item.EndNode == nodeId2);
            }

            return arc;
        }



        public Graph GetGraph()
        {
            return this;
        }

        public Graph GetGraph(int time)
        {
            Graph newGraph = new Graph();

            foreach (Node node in this.Nodes)
            {
                for (int i = 0; i < node.Arcs.Count; i++)
                {
                    if (node.Arcs[i].TravelTimeSeries.Exists(item => item == time))
                    {
                        Node tempNode = new Node();
                        Arc tempArc = new Arc();

                        tempArc.EndNode = node.Arcs[i].EndNode;
                        tempArc.TravelTimeSeries.Add(time);
                        tempNode.Arcs.Add(tempArc);

                        newGraph.nodes.Add(tempNode);
                    }
                }

            }

            return newGraph;
        }

        #endregion

        #region Modifiers

        public bool InsertNode(int nodeId1)
        {
            if (!NodeExists(nodeId1))
            {
                Node node1 = new Node();
                node1.Id = nodeId1;
                node1.NumberOfNeighbours = 0;
                this.Nodes.Add(node1); 
                return true;
            }
            // Node already exists
            return false;
        }

        public bool DeleteNode(int nodeId1)
        {
            Node node = GetNode(nodeId1);
            if (node != null)
            {
                node.Arcs.Clear();
                this.Nodes.Remove(node);
                return true;
            }
            // Node not found
            return false;
        }

        public bool DeleteNode(int nodeId1, int t)
        {
            Node node = GetNode(nodeId1);
            if (node != null)
            {
                // set the time series of node at time t to -1;
                return true;
            }
            // Node not found
            return false;
        }

       // This needs to be changed.. time just represents the index on the time series..
       // have one more parameter in the arguments that tells the value of time..
       public bool InsertArc(int nodeId1, int nodeId2, int time)
        {
            //Node node;
            //Arc arc;
            bool result = false;
            bool isArcFound = false;

            foreach (Node node in this.Nodes)
            {
                if (node.Id == nodeId1)
                {
                    foreach (Arc arc in node.Arcs)
                    {
                        if (arc.EndNode == nodeId2)
                        {
                            isArcFound = true;
                            if (!arc.TravelTimeSeries.Exists(item => item == time))
                            {
                                arc.TravelTimeSeries.Add(time);
                                result = true;
                            }
                        }
                    }

                    if (!isArcFound)
                    {
                        node.Arcs.Add(new Arc(nodeId2, time));
                        result = true;
                    }
                }
            }



            return result;
        }


        public bool InsertArc(int nodeId1, int nodeId2, List<int> time)
        {
            //Node node;
            //Arc arc;
            bool result = false;
            bool isArcFound = false;

            foreach (Node node in this.Nodes)
            {
                if (node.Id == nodeId1)
                {
                    foreach (Arc arc in node.Arcs)
                    {
                        if (arc.EndNode == nodeId2)
                        {
                            isArcFound = true;
                            arc.TravelTimeSeries = time;
                            result = true;
                        }
                    }
                }

                if (!isArcFound)
                {
                    Arc arc = new Arc(nodeId2);
                    arc.TravelTimeSeries = time;
                    node.Arcs.Add(arc);
                    result = true;
                }
            }

            return result;

        }


        public bool DeleteArc(int nodeId1, int nodeId2, int time)
        {
            //Node node;
            //Arc arc;
            bool result = false;

            foreach (Node node in this.Nodes)
            {
                if (node.Id == nodeId1)
                {
                    foreach (Arc arc in node.Arcs)
                    {
                        if (arc.EndNode == nodeId2)
                        {
                            if (arc.TravelTimeSeries.Exists(item => item == time))
                            {
                                arc.TravelTimeSeries.Remove(time);
                            }
                        }
                    }

                    result = true;
                }
            }

            return result;
        }


        public bool DeleteArc(int nodeId1, int nodeId2)
        {
            bool result = false;
            int index;

            foreach (Node node in this.Nodes)
            {
                if (node.Id == nodeId1)
                {
                    index = node.Arcs.FindIndex(item => item.EndNode == nodeId2);
                    if (index >= 0)
                        node.Arcs.RemoveAt(index);
                    result = true;
                }
            }

            return result;

        }


        public bool UpdateArc(int nodeId1, int nodeId2, int time)
        {
            //Node node;
            //Arc arc;
            bool result = false;
            int nodeIndex, arcIndex, timeIndex;

            nodeIndex = this.Nodes.FindIndex(item => item.Id == nodeId1);
            if (nodeIndex >= 0)
            {
                arcIndex = this.nodes[nodeIndex].Arcs.FindIndex(item => item.EndNode == nodeId2);
                if (arcIndex >= 0)
                {
                    timeIndex = this.Nodes[nodeIndex].Arcs[arcIndex].TravelTimeSeries.FindIndex(item => item == time);
                    if (timeIndex >= 0)
                    {
                        this.Nodes[nodeIndex].Arcs[arcIndex].TravelTimeSeries[timeIndex] = time;
                        result = true;
                    }
                }
            }

            return result;
        }


        public bool UpdateArc(int nodeId1, int nodeId2)
        {
            bool result = false;
            int index;

            foreach (Node node in this.Nodes)
            {
                if (node.Id == nodeId1)
                {
                    index = node.Arcs.FindIndex(item => item.EndNode == nodeId2);
                    if (index >= 0)
                        node.Arcs.RemoveAt(index);
                    result = true;
                }
            }

            return result;

        }

        public bool InsertGraph(Graph graph)
        {
            bool result = false;
            int index;
            if (null != graph)
            {

                foreach (Node node in graph.Nodes)
                {
                    index = this.Nodes.FindIndex(item => item.Id == node.Id);
                    if (index >= 0)
                    {
                        this.Nodes.Add(node);
                        result = true;
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }


        public bool InsertGraph(int time)
        {
            bool result = false;

            foreach (Node node in this.Nodes)
            {
                foreach (Arc arc in node.Arcs)
                {
                    if (!arc.TravelTimeSeries.Exists(item => item == time))
                    {
                        arc.TravelTimeSeries.Add(time);
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool DeleteGraph(int time)
        {
            bool result = false;

            foreach (Node node in this.Nodes)
            {
                foreach (Arc arc in node.Arcs)
                {
                    if (arc.TravelTimeSeries.Exists(item => item == time))
                    {
                        arc.TravelTimeSeries.Remove(time);
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool DeleteGraph(Graph graph)
        {
            bool result = false;
            int index;

            foreach (Node node in graph.Nodes)
            {
                index = this.Nodes.FindIndex(item => item.Id == node.Id);
                if (index >= 0)
                {
                    this.Nodes.RemoveAt(index);
                }
            }

            return result;
        }

        public bool UpdateGraph(int time)
        {
            bool result = false;
            int index;

            foreach (Node node in this.Nodes)
            {
                foreach (Arc arc in node.Arcs)
                {
                    index = arc.TravelTimeSeries.FindIndex(item => item == time);
                    if (index >= 0)
                    {
                        arc.TravelTimeSeries[index] = time;
                        result = true;
                    }
                }
            }

            return result;
        }
        #endregion

        #region Predicates

        public bool NodeExists(int nodeId1)
        { 
            if(GetNode(nodeId1) != null)
                return true;
            else
                return false;
        }

        public bool ArcExists(int nodeId1, int nodeId2, int time)
        {
            Node node;
            Arc arc = null;

            node = this.nodes.Find(item => item.Id == nodeId1);

            if (null != node)
            {
                arc = node.Arcs.Find(item => item.EndNode == nodeId2);

                if (null != arc)
                {
                    return (arc.TravelTimeSeries.Exists(item => item == time));
                }
            }

            return false;

        }

        public bool ArcExists(int nodeId1, int nodeId2)
        {
            Node node;

            node = this.nodes.Find(item => item.Id == nodeId1);

            if (null != node)
            {
                return node.Arcs.Exists(item => item.EndNode == nodeId2);
            }

            return false;
        }

        #endregion

        // Time to reach nth neighbor from node_idx, starting at time 'distance'
        public int GetWeight(int nodeId, int distance, int nNeighbor)
        {
            //Console.WriteLine(String.Format("Node ID: {0} {1} {2}",node_idx, distance, n_neighbor));
            List<Arc> arcs = Nodes[nodeId].Arcs;
            Arc a = new Arc();
            if ( nNeighbor + 1 <= arcs.Count )
                a = arcs[nNeighbor];
            else a = arcs[arcs.Count - 1];

            // printf("starts at %d Arc ends at: %d :: Time %d\n",node_idx , a->end, distance);
            int time_series_index = distance - 1;
            if (time_series_index >= LenghtOfTimeSeries)
                return -1;
            int wait = 0;
            while (a.BestTravelTimeSeries[time_series_index] == -1)
            {
                time_series_index++;
                wait++;
            }

            //check for bounds
            if (time_series_index > LenghtOfTimeSeries)
                return -1;
            else
            {
                //the weight should also include the waiting period at the node.
                // printf("Index and Weight: %d %d\n",time_series_index, a->best_travel_time_series[time_series_index]+wait);
                return a.TravelTimeSeries[a.BestTravelTimeSeries[time_series_index]] + wait;
            }
        }

        public void PrintGraph()
        {
            foreach (Node node in this.Nodes)
            {
                Console.WriteLine(node.Id);
                foreach (Arc arc in node.Arcs)
                {
                    foreach (int ts in arc.TravelTimeSeries)
                        Console.Write(string.Format("{0}, ", ts.ToString()));
                    Console.Write("\n");
                }

            }
        }

        public bool LoadGraph (string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    FileStream file = new FileStream(filePath, FileMode.Open);
                    int numberofArcs=0;
                    StreamReader streamReader = new StreamReader(file);

                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        if (line.StartsWith("c") || line.StartsWith("n"))
                        {
                            continue;
                        }
                        else if (line.StartsWith("p"))
                        {
                            string[] graphInfo = line.Split(' ');
                            if (graphInfo.Length > 4)
                            {
                                if (graphInfo[4] != null)
                                {
                                    this.LenghtOfTimeSeries = int.Parse(graphInfo[4]);
                                }
                                else
                                {
                                    this.LenghtOfTimeSeries = 0;
                                }
                            }
                            if (graphInfo[3] != null)
                            {
                                numberofArcs = int.Parse(graphInfo[3]);
                            }
                        }
                        else if (line.StartsWith("a"))
                        {
                            string[] tempArcLine = line.Split(' ');


                            Node node1, node2;

                            node1 = this.nodes.Find(item => item.Id == int.Parse(tempArcLine[1]));

                            if (null == node1)
                            {
                               node1 = new Node();
                               node1.Id = int.Parse(tempArcLine[1]);
                               node1.NumberOfNeighbours = 0;
                               this.Nodes.Add(node1);    
                            }

                            node2 = this.nodes.Find(item => item.Id == int.Parse(tempArcLine[2]));

                            if (null == node2)
                            {
                                node2 = new Node();
                                node2.Id = int.Parse(tempArcLine[2]);
                                node2.NumberOfNeighbours = 0;
                                this.Nodes.Add(node2);
                            }

                            Arc tempArc = new Arc();
                            tempArc.EndNode = node2.Id;

                            for (int i = 3; i < tempArcLine.Length; i++)
                            {
                                tempArc.TravelTimeSeries.Add(Convert.ToInt16(Convert.ToDouble(tempArcLine[i])));
                                //tempArc.TravelTimeSeries.Add(int.Parse(tempArcLine[i]));
                                tempArc.BestTravelTimeSeries.Add(-1);
                            }
                            PreProcessArc(ref tempArc);
                            node1.Arcs.Add(tempArc);
                            node1.NumberOfNeighbours++;
                        }                       
                    }
                    streamReader.Close();
                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
