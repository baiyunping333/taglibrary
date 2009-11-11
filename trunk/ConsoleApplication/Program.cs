using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagLibrary.DataTypes;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

            Graph graph = new Graph();
            /*graph.LenghtOfTimeSeries = 3;
            
            Arc arc1 = new Arc();
            arc1.TravelTimeSeries.Add(1);
            arc1.TravelTimeSeries.Add(1);
            arc1.TravelTimeSeries.Add(-1);
            arc1.EndNode = 2;

            Arc arc2 = new Arc();
            arc2.TravelTimeSeries.Add(2);
            arc2.TravelTimeSeries.Add(2);
            arc2.TravelTimeSeries.Add(3);
            arc2.EndNode = 4;

            Arc arc3 = new Arc();
            arc3.TravelTimeSeries.Add(1);
            arc3.TravelTimeSeries.Add(-1);
            arc3.TravelTimeSeries.Add(4);
            arc3.EndNode = 4;

            Arc arc4 = new Arc();
            arc4.TravelTimeSeries.Add(2);
            arc4.TravelTimeSeries.Add(2);
            arc4.TravelTimeSeries.Add(2);
            arc4.EndNode = 3;


            Node node1 = new Node();
            node1.Id = 1;
            node1.Arcs.Add(arc1);
            node1.Arcs.Add(arc4);

            Node node2 = new Node();
            node2.Id = 2;
            node2.Arcs.Add(arc2);

            Node node3 = new Node();
            node3.Id = 3;
            node3.Arcs.Add(arc3);

            Node node4 = new Node();
            node4.Id = 4;

            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);
            graph.Nodes.Add(node3);
            graph.Nodes.Add(node4);*/

            
            Program program = new Program();
            graph.LoadGraph("..\\..\\..\\myData");
            //program.PrintGraph(graph);

            StreamWriter file = new StreamWriter("output.csv", true);
            file.WriteLine("Origin, Destination, Distance, Previous Node, Reached Previous Node at");
            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                if( i%100 == 0)
                    Console.WriteLine(string.Format("Processing node {0} of {1}", i, graph.Nodes.Count-1));
                graph.ShortestPaths(i + 1, file);
            }
            //Console.ReadKey();
            file.Close();
        }

        public void PrintGraph(Graph graph)
        {
            foreach (Node node in graph.Nodes)
            {
                Console.WriteLine(node.Id);

                foreach (Arc arc in node.Arcs)
                {
                    foreach( int ts in arc.TravelTimeSeries)
                        Console.Write(string.Format("{0}, ",ts.ToString()));
                    Console.Write("\n");
                }
            
            }

            //Console.ReadKey();
        }
    }
}
